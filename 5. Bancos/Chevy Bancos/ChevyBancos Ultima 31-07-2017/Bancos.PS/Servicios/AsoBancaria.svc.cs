using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.IO;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.LN.Utilidades;
using Bancos.EN.Definicion;

using Bancos.PS.Servicios.Ftp;
using Bancos.PS.Servicios.Correo;


namespace Bancos.PS.Servicios
{

    public class AsoBancaria : IAsoBancaria
    {

        #region DEFINICION DE VARIABLES
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea1EA { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea2EL { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea3DT { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea4CL { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea5CA { get; set; }
        int registrosLote = 0;
        double valorServicio = 0;
        static StreamWriter sw;
        String LineaArmada1EA = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO ARCHIVO
        String LineaArmada2EL = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO LOTE
        String LineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE Y VA VARIANDO EN CADA CICLO 
        String LineaArmada4CL = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE LOTE
        String LineaArmada5CA = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE ARCHIVO
        String ConsecutivoArchivo; // CONTIENE UN CONSECUTIVO QUE SE USA PARA CONTROLAR CUANTAS VECES SE HA GENERADO EL ARCHIVO EN EL DIA
        String Fecha;
        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO ASOBANCARIA
        String Directorio = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA EL ARCHIVO ASOBANCARIA               
        String tipoArchivo = String.Empty;
        String TipoProcesoXCuenta = String.Empty;
        #endregion

        #region TABLA CON LINEAS PARA GUARDAR EN LA BD
        DataTable LineasArmadasdt = new DataTable();
        #endregion

        public String ServicioAso(String NombreCuenta, String IdCuentaBancoEpicor, String ArchivoSalidaAsobancaria, bool EsFTP,
                                  String UrlFTP, String UsuarioFTP, String ClaveFTP, ArrayList CorreosControl,
                                  ArrayList CorreosEnvio, String CodigoBanco, String Remitente, String Usuario, String TipoProceso)
        {
            TipoProcesoXCuenta = TipoProceso;
            //AQUI MARINA RT VALIDAR UTILIDAD DE CODIGO SERVICIO DE EPICOR
          //  ServicioContrato.ChevyplanWS contratos = new ServicioContrato.ChevyplanWS();
           // contratos.Timeout = -1;
            Logs objL = new Logs();
            EnviarCorreo Correo = new EnviarCorreo();
            ConectorFTP Ftp = new ConectorFTP();

           DataSet tablaContratos = new DataSet();
          // tablaContratos = contratos.Asobancaria_ArchivoPlano();

           if (tablaContratos.Tables.Count == 0 || tablaContratos.Tables[0].Rows.Count == 0)
           {
               objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
               objL.pUsuario = Usuario;
               objL.pDetalle = NombreCuenta + ", Asobancaria : No se encontraron registros para procesar";
               objL.pTipoArchivo = TipoProcesoXCuenta;
               objL.pTipoProceso = "GEN";
               new LogsLN().insertar(objL);
               return "No se encontraron registros para procesar";
           }

            tipoArchivo = "Asobancaria";
            Directorio = @ArchivoSalidaAsobancaria ;

            //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS DE ASOBANCARIA SI NO EXISTE
            if (!Directory.Exists(Directorio))
            {
                System.IO.Directory.CreateDirectory(Directorio);
            }

            #region CREAR COLUMNAS DE LA TABLA CON LOS DATOS PARA GUARDAR LAS LINEAS EN UNA BD
            LineasArmadasdt.Columns.Add("fecha");
            LineasArmadasdt.Columns.Add("IdCuentaBanco");
            LineasArmadasdt.Columns.Add("consecutivo");
            LineasArmadasdt.Columns.Add("lineaArmada");
            #endregion

            try
            {
                escribirArchivo(CodigoBanco, IdCuentaBancoEpicor, tablaContratos);

                foreach (DataRow row in LineasArmadasdt.Rows)
                {
                    guardarLineas(row[0].ToString(), row[1].ToString(), row[2].ToString(),
                                  row[3].ToString());
                }

                LineasArmadasdt.Clear();

                if (EsFTP)
                {
                    Ftp.enviarArchivoXFtp(Directorio, UrlFTP, UsuarioFTP, ClaveFTP, nombreArchivo);
                    if (CorreosControl.Count > 0)
                    Correo.enviarArchivoXCorreoControlE(Directorio, CorreosControl,
                                                nombreArchivo, Remitente, registrosLote,
                                                UrlFTP, UsuarioFTP, tipoArchivo);
                }
                else
                {
                    if (CorreosEnvio.Count > 0)
                    Correo.enviarArchivoXCorreoEnvio(Directorio, CorreosEnvio,
                                                nombreArchivo, Remitente, registrosLote, tipoArchivo);
                    if ((CorreosControl.Count > 0) && (CorreosEnvio.Count > 0))
                    Correo.enviarArchivoXCorreoControlC(Directorio, CorreosControl,
                                              CorreosEnvio, nombreArchivo, Remitente, registrosLote, tipoArchivo);
                }
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = NombreCuenta + ", Asobancaria : Archivo Asobancaria fue generado correctamente";
                objL.pTipoArchivo = TipoProcesoXCuenta;
                objL.pTipoProceso = "GEN";
                new LogsLN().insertar(objL);
                return "Archivo Asobancaria fue generado correctamente";
            }
            catch (Exception ex)
            {
                if (CorreosControl.Count > 0)
                    Correo.enviarNotificacionesError(NombreCuenta, (String[])CorreosControl.ToArray(typeof(String)), Remitente, ex.Message, tipoArchivo);
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = NombreCuenta + ", Asobancaria : " + ex.Message;
                objL.pTipoArchivo = TipoProcesoXCuenta;
                objL.pTipoProceso = "GEN";
                new LogsLN().insertar(objL);
                return ex.Message;
            }
        }

        public void llenarTablaParaActualizados(String fecha, String IdCuentaBanco, String Consecutivo, String Linea)
        {
            DataRow dr1 = LineasArmadasdt.NewRow();
            dr1["fecha"] = fecha;
            dr1["IdCuentaBanco"] = IdCuentaBanco;
            dr1["consecutivo"] = Consecutivo;
            dr1["lineaArmada"] = Linea;
            LineasArmadasdt.Rows.Add(dr1);
        }

        //SE CREA EL ARCHIVO ASOBANCARIA
        private void escribirArchivo(String CodigoBanco, String IdCuentaBanco, DataSet tabla)
        {
            try
            {
                ListaLinea1EA = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea2EL = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea3DT = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea4CL = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea5CA = new List<Bancos.EN.Tablas.EstructuraArchivo>();

                ListaLinea1EA.AddRange(consultarEstructuraAsobancaria("1EA", TipoProcesoXCuenta));
                ListaLinea2EL.AddRange(consultarEstructuraAsobancaria("2EL", TipoProcesoXCuenta));
                ListaLinea3DT.AddRange(consultarEstructuraAsobancaria("3DT", TipoProcesoXCuenta));
                ListaLinea4CL.AddRange(consultarEstructuraAsobancaria("4CL", TipoProcesoXCuenta));
                ListaLinea5CA.AddRange(consultarEstructuraAsobancaria("5CA", TipoProcesoXCuenta));

                //SE CREA EL NOMBRE DEL ARCHIVO SEGUN LOS PARAMETROS ASOBANCARIA
                nombreArchivo = String.Concat(IdCuentaBanco, "_", DateTime.Now.ToString("ddMMyyyy"),
                                                 "_", writeMilitaryTime(DateTime.Now), ".txt");

                sw = new StreamWriter(Directorio + nombreArchivo, false);
                ArrayList line1EA = new ArrayList() { "1EA", CodigoBanco }; //ARREGLO LLEVA EL CODIGO DE TRANSITO QUE CAMBIA PARA CADA BANCO
                ArrayList line2EL = new ArrayList() { "2EL" };
                ArrayList line3DT = new ArrayList(); //ARREGLO LLEVA EL CONTRATO,VALOR,FECHA VENCIMIENTO,IDENTIFICACION Y CODIGO DE TRANSITO
                ArrayList line4CL = new ArrayList() { "4CL" }; //ARREGLO LLEVA EL TOTAL DEL REGISTRO DEL LOTE Y EL TOTAL DEL VALOR DE SERVICIO
                ArrayList line5CA = new ArrayList() { "5CA" }; //ARREGLO LLEVA EL TOTAL DEL VALOR DE SERVICIO

                Fecha = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));

                //SE ADICIONA UN CONSECUTIVO SEGUN EL NUMERO DE ARCHIVO QUE EXISTAN DEL MISMO DIA                
                int numColumnas = 0;
                HistorialArchivosSalidaLN columnas = new HistorialArchivosSalidaLN();
                numColumnas = columnas.consultarConsecutivoXBanco(IdCuentaBanco, TipoProcesoXCuenta, Fecha).Rows.Count;

                if (numColumnas == 0)
                    ConsecutivoArchivo = "A";
                else
                    ConsecutivoArchivo = consecutivo(numColumnas);


                #region Armar Linea Encabezado Archivo

                line1EA.Add(ConsecutivoArchivo);
                LineaArmada1EA = armarLineas(line1EA, ListaLinea1EA);
                sw.WriteLine(LineaArmada1EA);
                llenarTablaParaActualizados(Fecha, IdCuentaBanco, ConsecutivoArchivo, LineaArmada1EA);
                registrosLote += 1;

                #endregion

                #region Armar Linea Encabezado Lote

                LineaArmada2EL = armarLineas(line2EL, ListaLinea2EL);
                sw.WriteLine(LineaArmada2EL);
                llenarTablaParaActualizados(Fecha, IdCuentaBanco, ConsecutivoArchivo, LineaArmada2EL);
                registrosLote += 1;

                #endregion

                #region Armar Linea Detalle

                foreach (DataRow renglon in tabla.Tables[0].Rows)
                {
                    line3DT.Add("3DT");
                    line3DT.Add(renglon[0].ToString().Trim());//CONTRATO

                    if (renglon[1].ToString().Contains("."))
                        line3DT.Add(renglon[1].ToString().Replace(".", ",").Trim());//VALOR SERVICIO PRINCIPAL
                    else
                        line3DT.Add(renglon[1].ToString().Trim());//VALOR SERVICIO PRINCIPAL

                    line3DT.Add(renglon[2].ToString().Trim());
                    line3DT.Add(renglon[3].ToString().Trim());
                    line3DT.Add(CodigoBanco);
                    LineaArmada3DT = armarLineas(line3DT, ListaLinea3DT);
                    sw.WriteLine(LineaArmada3DT);
                    llenarTablaParaActualizados(Fecha, IdCuentaBanco, ConsecutivoArchivo, LineaArmada3DT);
                    line3DT.Clear();
                    registrosLote += 1;

                    if (renglon[1].ToString().Contains("."))
                        valorServicio += Convert.ToDouble(renglon[1].ToString().Replace(".", ",").Trim());
                    else
                        valorServicio += Convert.ToDouble(renglon[1].ToString().Trim());

                    LineaArmada3DT = String.Empty;
                }

                #endregion

                #region Armar Linea Control Lote

                line4CL.Add(registrosLote - 2);
                line4CL.Add(valorServicio);
                LineaArmada4CL = armarLineas(line4CL, ListaLinea4CL);
                sw.WriteLine(LineaArmada4CL);
                llenarTablaParaActualizados(Fecha, IdCuentaBanco, ConsecutivoArchivo, LineaArmada4CL);
                registrosLote += 1;

                #endregion

                #region Armar Linea Control Archivo

                line5CA.Add(valorServicio);
                LineaArmada5CA = armarLineas(line5CA, ListaLinea5CA);
                sw.WriteLine(LineaArmada5CA);
                llenarTablaParaActualizados(Fecha, IdCuentaBanco, ConsecutivoArchivo, LineaArmada5CA);
                registrosLote += 1;

                #endregion

                sw.Close();
            }
            catch
            {
                sw.Close();
                File.Delete(Directorio + nombreArchivo);
                throw new System.Exception("Ocurrio un error al crear archivo plano");
            }

        }

        //SE GUARDA TODAS LAS LINEAS DEL ARCHIVO ASOBANCARIA EN LA BASE DE DATOS
        private void guardarLineas(String fecha, String IdCuentaBanco, String Consecutivo, String Linea)
        {

            int valor = 0;
            HistorialArchivosSalida objEntidad = new HistorialArchivosSalida();
            HistorialArchivosSalidaLN objHA = new HistorialArchivosSalidaLN();
            objEntidad.pFecha = fecha;
            objEntidad.pIdCuentaBanco = IdCuentaBanco;
            objEntidad.pTipoArchivo = TipoProcesoXCuenta;
            objEntidad.pConsecutivo = Consecutivo;
            objEntidad.pLineaDetalle = Linea;
            valor = objHA.insertar(objEntidad);
            if (valor <= 0)
            {
                throw new System.Exception("Ocurrio un error al guardar archivo plano en la base de datos");
            }
        }

        // SE CARGA LOS PARAMETROS DE LA ESTRUCTURA DE LA LINEAS ASOBANCARIA PARA GENERAR EL ARCHIVO ASOBANCARIA
        private List<Bancos.EN.Tablas.EstructuraArchivo> consultarEstructuraAsobancaria(String tipoLinea, String tipoProceso)
        {
            DataTable tabla = new EstructuraArchivoLN().consultarEstructuraArchivoSalida(tipoLinea, tipoProceso);
            List<Bancos.EN.Tablas.EstructuraArchivo> lista = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            Bancos.EN.Tablas.EstructuraArchivo Entidad;

            foreach (DataRow fila in tabla.Rows)
            {

                Entidad = new Bancos.EN.Tablas.EstructuraArchivo();
                Entidad.pAlineacion = Convertidor.aCadena(fila["ALINEACION"]);
                Entidad.pCantidadDecimales = Convertidor.aEntero32(fila["CANTIDAD_DECIMALES"]);
                Entidad.pCaracterRelleno = Convertidor.aCadena(fila["CARACTER_RELLENO"]);
                Entidad.pConfiguracion = Convertidor.aEntero32(fila["Configuracion"]);
                Entidad.pEsContador = Convertidor.aBooleano(fila["ES_CONTADOR"]);
                Entidad.pFormatoFecha = Convertidor.aCadena(fila["FORMATO_FECHA"]);
                Entidad.pLongitud = Convertidor.aEntero32(fila["LONGITUD"]);
                Entidad.pNombreCampo = Convertidor.aCadena(fila["NOMBRE_CAMPO"]);
                Entidad.pOid = Convertidor.aEntero32(fila["OID"]);
                Entidad.pOrdenColumna = Convertidor.aEntero32(fila["ORDEN_COLUMNA"]);
                Entidad.pRequiereCambio = Convertidor.aBooleano(fila[EstructuraArchivoDEF.RequiereCambio]);
                Entidad.pTipoDato = Convertidor.aCadena(fila["Tipo_Dato"]);
                Entidad.pValor = Convertidor.aCadena(fila[EstructuraArchivoDEF.Valor]);
                Entidad.pValorPorDefecto = Convertidor.aBooleano(fila[EstructuraArchivoDEF.ValorPorDefecto]);
                lista.Add(Entidad);

            }

            return lista;
        }

        //SE ARMA CADA LINEA DE ASOBANCARIA SEGUN LOS PARAMETROS DE LA ESTRUCTURA
        private String armarLineas(ArrayList lineaDatos, List<Bancos.EN.Tablas.EstructuraArchivo> lineasAsobancaria)
        {
            String linea = String.Empty;
            String valor = String.Empty;
            char caracterRelleno;
            foreach (Bancos.EN.Tablas.EstructuraArchivo objAso in lineasAsobancaria)
            {
                valor = String.Empty;
                if (objAso.pCaracterRelleno.Equals("CR"))
                    caracterRelleno = '0';
                else
                    caracterRelleno = ' ';
                if (objAso.pValorPorDefecto == true)
                {
                    linea += rellenarCampo(objAso.pValor, objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                }
                else
                {
                    switch (Convert.ToString(lineaDatos[0]))
                    {
                        case "1EA":
                            if (objAso.pNombreCampo.Equals("Código de Entidad Financiera Originadora"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Fecha del archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Hora de grabación del archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Modificador de archivo"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            break;
                        case "2EL":
                            break;
                        case "3DT":
                            if (objAso.pNombreCampo.Equals("Referencia principal del usuario"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Valor de servicio principal"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            else if (objAso.pNombreCampo.Equals("Fecha de vencimiento"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[3]));
                            else if (objAso.pNombreCampo.Equals("No. Identificación del cliente"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[4]));
                            else if (objAso.pNombreCampo.Equals("Código de Entidad Financiera Originadora"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[5]));
                            break;
                        case "4CL":
                            if (objAso.pNombreCampo.Equals("Tot. Registros del lote"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Valor de servicio principal"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            break;
                        case "5CA":
                            if (objAso.pNombreCampo.Equals("Valor total de servicio principal"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            break;
                    }
                    linea += rellenarCampo(valor, objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                }
            }
            return linea;
        }

        //SE RELLENAN LOS CAMPOS DEL ARCHIVO ASOBANCARIA CON CEROS O ESPACIOS SEGUN EL PARAMETRO 
        private String rellenarCampo(String campo, String alineacion, int longitud, char caracterRelleno)
        {
            if (campo.Length <= longitud)
            {
                if (alineacion.Equals("der"))
                {
                    campo = campo.PadLeft(longitud, caracterRelleno);
                }
                else if (alineacion.Equals("izq"))
                {
                    campo = campo.PadRight(longitud, caracterRelleno);
                }
            }
            else
            {
                campo = campo.Substring(0, longitud);
            }

            return campo;
        }

        //SE CONVIERTE UNA HORA NORMAL A HORA MILITAR
        private String writeMilitaryTime(DateTime date)
        {
            string value = date.ToString("HHmm");
            return value;
        }

        private String armarCampo(Bancos.EN.Tablas.EstructuraArchivo objAso, String valor)
        {
            String campo = String.Empty;
            switch (objAso.pTipoDato)
            {
                case "AN":
                    campo = valor;
                    break;
                case "DE":
                    campo = convertirNumero(valor, objAso.pCantidadDecimales.Value);
                    break;
                case "FE":
                    if (!String.IsNullOrEmpty(valor))
                    {
                        DateTime fechaContrato;
                        string[] fecha = objAso.pFormatoFecha.Split('/');
                        fechaContrato = Convert.ToDateTime(valor);
                        campo = fechaContrato.ToString(fecha[0].ToString()) +
                                fechaContrato.ToString(fecha[1].ToString()) +
                                fechaContrato.ToString(fecha[2].ToString());
                    }
                    else
                    {
                        string[] fecha = objAso.pFormatoFecha.Split('/');
                        campo = DateTime.Today.ToString(fecha[0].ToString()) +
                                DateTime.Today.ToString(fecha[1].ToString()) +
                                DateTime.Today.ToString(fecha[2].ToString());
                    }
                    break;
                case "HH":
                    campo = writeMilitaryTime(DateTime.Now);
                    break;
            }
            return campo;
        }
        //SE CALCULA EL CONSECUTIVO DE UN ARCHIVO ASOBANCARIA QUE FUE GENEREADO MAS DE UNA VEZ EN EL MISMO DIA
        private String consecutivo(int columna)
        {
            string[] caracter = {"A","B","C","D","E","F","G","H","I","J","K",
                                "L","M","N","O","P","Q","R","S","T","U","W",
                                "X","Y","Z","0","1","2","3","4","5","6","7","8","9"};

            if (columna > 34)
                return caracter[0];
            else
                return caracter[columna];
        }

        private String convertirNumero(String numero, int decimales)
        {

            String[] numeros = numero.Split(',');
            if (!numero.Contains(","))
                return (Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales)).ToString();
            else
                return Convert.ToInt64((Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales - numeros[1].Length))).ToString();

        }

    }

}
