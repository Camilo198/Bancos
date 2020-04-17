using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.LN.Utilidades;
using Bancos.EN.Definicion;

using System.IO;
using Bancos.PS.Servicios.Correo;

using System.Threading;

//Servidor: SBOGCHE028F
//Base de Datos: EpicSat
//Tabla: Pago_Confirmacion

//--Actualiza la información para limpiarla y poder volver a usarla
//update dbo.Pago_Confirmacion
//set Reportado = 0,
//      Lote = 0
//where Year(fecha_transaccion) = 2014 and MONTH(fecha_transaccion) = 2

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "PagosOnline" en el código, en svc y en el archivo de configuración a la vez.
    
    public class PagosOnline : IPagosOnline
    {

        #region DEFINICION DE VARIABLES
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea1EA { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea2EL { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea3DT { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea4CL { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea5CA { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoEncabezadoArchivo { get; set; }//LISTA QUE TRAE EL DATO DE LA FECHA DE CORTE
        
        StreamWriter sw;
        String LineaArmada1EA = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO ARCHIVO
        String LineaArmada2EL = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO LOTE
        String LineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE Y VA VARIANDO EN CADA CICLO 
        String LineaArmada4CL = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE LOTE
        String LineaArmada5CA = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE ARCHIVO
        String ConsecutivoArchivo; // CONTIENE UN CONSECUTIVO QUE SE USA PARA CONTROLAR CUANTAS VECES SE HA GENERADO EL ARCHIVO EN EL DIA
        String Fecha;
        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO PAGOS ONLINE
        String Directorio = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA EL ARCHIVO PAGOS ONLINE       
        int registrosLote = 0;//NUMERO DE LINEAS QUE CONTIENE EL ARCHIVO PAGOS ONLINE  
        String tipoArchivo = String.Empty;
        ArrayList LimitesSuperior = new ArrayList();
        int ciclo = 0;
        String tipocuentabanco = String.Empty;
        String TipoProcesoXCuenta = String.Empty;

        String[] CorreosNoti = new  String[1];
        #endregion

        #region TABLA CON LINEAS PARA GUARDAR EN LA BD
        DataTable LineasArmadasdt = new DataTable();
        #endregion

        public String ServicioPagosOnline(String NombreCuenta, String IdCuentaBancoEpicor, String ArchivoSalidaPagosOnline,
                                          ArrayList CorreosControl, String CodigoBanco, String NumCuenta,
                                          String TipoCuenta, String Remitente, String Usuario, String TipoProceso)
        {
            TipoProcesoXCuenta = TipoProceso;
            switch (TipoCuenta)
            {
                case "Ahorros": 
                    { tipocuentabanco = "1";
                    break;
                    }
                case "Corriente": 
                    { tipocuentabanco = "2";
                    break;
                    }                
                default: 
                    { tipocuentabanco = "0";
                    break;
                    }                    
            }


           
            DataSet tablaContratos = new DataSet();
            PagosOnlineLN PagosOnlineABR1 = new PagosOnlineLN();
            tablaContratos = PagosOnlineABR1.consultar(); // AQUI MRT consultar en paginaweb,chp_confirmacion_pago
           // contratos.Timeout = -1;
            Logs objL = new Logs();
            EnviarCorreo Correo = new EnviarCorreo();
            
            if (tablaContratos.Tables.Count == 0 || tablaContratos.Tables[0].Rows.Count == 0)
            {
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = NombreCuenta + ", Pagos Online : No se encontraron registros para procesar";
                objL.pTipoArchivo = TipoProcesoXCuenta;
                objL.pTipoProceso = "GEN";
                new LogsLN().insertar(objL);  
                return "No se encontraron registros para procesar";
            }

            //ESTRUCTURA DEL ARCHIVO DEL BANCO (ARCHIVO DE ENTRADA)
            //*********************************************************************************
            ListaEstructuraArchivoBancoEncabezadoArchivo = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaEstructuraArchivoBancoEncabezadoArchivo.AddRange(consultarEstructuraArchivoBanco(IdCuentaBancoEpicor, "1EA"));
            //*********************************************************************************

          // AQUI se elima porque el contro no va a ser la fecha sino los registros sin procesar. Y este registro en la tabla de estructura archivo estaba generando inconsistencia al generar el encabexado de archivo
            //DateTime fechaLimite = new DateTime(Convert.ToInt32(ListaEstructuraArchivoBancoEncabezadoArchivo[0].pValor.Substring(0, 4)),
            //                                   Convert.ToInt32(ListaEstructuraArchivoBancoEncabezadoArchivo[0].pValor.Substring(5, 2)),
            //                                   Convert.ToInt32(ListaEstructuraArchivoBancoEncabezadoArchivo[0].pValor.Substring(8, 2)));
       

            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("referencia");
            dt1.Columns.Add("firma");
            dt1.Columns.Add("valor");
            dt1.Columns.Add("fecha");
            dt1.Columns.Add("contratofisico");// se usaba para EPICOR
            dt1.Columns.Add("medioPagoAplicaSico");//Tarjeta Credito
            dt1.Columns.Add("medioPagoAplicaVentas");//Tarjeta Credito
            dt1.Columns.Add("CodigoAutorizacion");//Tarjeta Credito

            foreach (DataRow row in tablaContratos.Tables[0].Rows)
            {
                DateTime fechaPago = Convert.ToDateTime(row[3].ToString());
                if ((fechaPago <= DateTime.Now)) // AQUI   if ((fechaPago <= fechaLimite))
                {
                    DataRow dr1 = dt1.NewRow();
                    dr1["referencia"] = row[0].ToString();
                    dr1["firma"] = row[1].ToString();
                    dr1["valor"] = row[2].ToString();
                    dr1["fecha"] = row[3].ToString();
                    dr1["contratofisico"] = row[4].ToString();
                    dr1["medioPagoAplicaSico"] = row[5].ToString();//Tarjeta Credito
                    dr1["medioPagoAplicaVentas"] = row[6].ToString();//Tarjeta Credito
                    dr1["CodigoAutorizacion"] = row[7].ToString();//Tarjeta Credito
                    dt1.Rows.Add(dr1);
                }
            }
            //ds1.Tables.Add(dt1);////ordenar por fecha

            DataView dv = dt1.DefaultView;
            dv.Sort = "fecha";
            dt1 = dv.ToTable();
            ds1.Tables.Add(dt1);


            tipoArchivo = "Pagos Online";
            Directorio = ArchivoSalidaPagosOnline;

            //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS DE PAGOS ONLINE SI NO EXISTE
            if (!Directory.Exists(Directorio))
            {
                System.IO.Directory.CreateDirectory(Directorio);
            }

            #region CREAR COLUMNAS DE LA TABLA CON LOS DATOS PARA GUARDAR LAS LINEAS EN UNA BD
            LineasArmadasdt.Columns.Add("fecha");
            LineasArmadasdt.Columns.Add("fechaContrato");
            LineasArmadasdt.Columns.Add("IdCuentaBanco");
            LineasArmadasdt.Columns.Add("consecutivo");
            LineasArmadasdt.Columns.Add("lineaArmada");
            #endregion

            try
            {
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    LimitesSuperior = obtenerLimites(ds1);
                    for (int i = 0; i < LimitesSuperior.Count; i++)
                    {

                        escribirArchivo(CodigoBanco, IdCuentaBancoEpicor, NumCuenta, tipocuentabanco,
                                        ds1, Convert.ToInt16(LimitesSuperior[i]));

                        int cicloArchivos = 0;
                        for (int j = ciclo; j < ciclo + Convert.ToInt64(LimitesSuperior[i]); j++)
                        {
                           //AQUI
                            // contratos.Pagosonline_Actualizacion(ds1.Tables[0].Rows[j].ItemArray[0].ToString(),
                              //                            ds1.Tables[0].Rows[j].ItemArray[1].ToString(),
                              //                             DateTime.Now.ToString("yyMMdd"));
                            cicloArchivos += 1;
                        }

                        foreach (DataRow row in LineasArmadasdt.Rows)
                        {
                            guardarLineas(row[0].ToString(), row[1].ToString(), row[2].ToString(),
                                          row[3].ToString(), row[4].ToString());
                        }

                        LineasArmadasdt.Clear();
                       
                        //AQUI MRT

                        // CorreosNoti[0] = "marina.ramirez@Chevyplan.com.co";
// Cambiar al pasar a produccion
                        CorreosNoti[0] = "cristian.munoz@Chevyplan.com.co";
                            Correo.enviarNotificaciones(Directorio, CorreosNoti , nombreArchivo, Remitente,
                                                     registrosLote, tipoArchivo);

                        ciclo = ciclo + Convert.ToInt16(LimitesSuperior[i]);

                        //SE GUARDA EL ESTADO DEL PROCESO
                        objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                        objL.pUsuario = Usuario;
                        objL.pDetalle = NombreCuenta + ", Pagos Online : Archivo Pagos Online con fecha de transaccion " + nombreArchivo.Substring(15, 10) + " fue generado correctamente";
                        objL.pTipoArchivo = TipoProcesoXCuenta;
                        objL.pTipoProceso = "GEN";
                        new LogsLN().insertar(objL);
                        nombreArchivo = String.Empty;
                        registrosLote = 0;

                        //AQUI ACTUALIZA PAGOS PROCESADOS EN LA BASE DE DATOS CHEVYPLAN.

                        string Actualiza = PagosOnlineABR1.Ejecutar();

                        Thread.Sleep(1000);
                    }
                    ciclo = 0;
                }
                return "Proceso Pagos Online ejecutado con exito!!";
            }
            catch (Exception ex)
            {
                CorreosNoti[0] = "cristian.munoz@Chevyplan.com.co";
 // Cambiar paso produccion            //CorreosNoti[0]  = "marina.ramirez@chevyplan.com.co";
                Correo.enviarNotificacionesError(NombreCuenta,CorreosNoti, Remitente, ex.Message, tipoArchivo);
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = NombreCuenta + ", Pagos Online : " + ex.Message;
                objL.pTipoArchivo = TipoProcesoXCuenta;
                objL.pTipoProceso = "GEN";
                new LogsLN().insertar(objL);                  
                return ex.Message;
            }

        }

        private ArrayList obtenerLimites(DataSet tablaContrato)
        {
            int registros = 0, tot = 1;
            int veces = 0;
            DateTime fechas = new DateTime();
            ArrayList limite = new ArrayList();
            foreach (DataRow renglon in tablaContrato.Tables[0].Rows)
            {
                if (registros == 0)
                    fechas = Convert.ToDateTime(renglon[3].ToString());
                if (fechas.ToShortDateString() == Convert.ToDateTime(renglon[3].ToString()).ToShortDateString())
                {
                    registros += 1;
                }
                else
                {
                    limite.Add(registros);
                    registros = 1;
                    veces += 1;
                }
                if (tot == tablaContrato.Tables[0].Rows.Count)
                {
                    limite.Add(registros);
                    break;
                }
                fechas = Convert.ToDateTime(renglon[3].ToString());
                tot += 1;
            }
            return limite;
        }

        public void llenarTablaParaActualizados(String fecha, String fechaTransaccion, String IdCuentaBanco, String Consecutivo, String Linea)
        {
            DataRow dr1 = LineasArmadasdt.NewRow();
            dr1["fecha"] = fecha;
            dr1["fechaContrato"] = fechaTransaccion;
            dr1["IdCuentaBanco"] = IdCuentaBanco;
            dr1["consecutivo"] = Consecutivo;
            dr1["lineaArmada"] = Linea;
            LineasArmadasdt.Rows.Add(dr1);

        }


        //SE CREA EL ARCHIVO PAGOS ONLINE
        private void escribirArchivo(String CodigoBanco, String IdCuentaBancoEpicor,
                                     String NumCuenta, String TipoCuenta, DataSet tabla, int Limite)
        {
            Double valorServicio = 0;
            try
            {
                ListaLinea1EA = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea2EL = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea3DT = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea4CL = new List<Bancos.EN.Tablas.EstructuraArchivo>();
                ListaLinea5CA = new List<Bancos.EN.Tablas.EstructuraArchivo>();

                ListaLinea1EA.AddRange(consultarEstructuraPagosOnline("1EA", TipoProcesoXCuenta));
                ListaLinea2EL.AddRange(consultarEstructuraPagosOnline("2EL", TipoProcesoXCuenta));
                ListaLinea3DT.AddRange(consultarEstructuraPagosOnline("3DT", TipoProcesoXCuenta));
                ListaLinea4CL.AddRange(consultarEstructuraPagosOnline("4CL", TipoProcesoXCuenta));
                ListaLinea5CA.AddRange(consultarEstructuraPagosOnline("5CA", TipoProcesoXCuenta));

                DateTime Contrato = Convert.ToDateTime(tabla.Tables[0].Rows[ciclo].ItemArray[3].ToString());
                //SE CREA EL NOMBRE DEL ARCHIVO SEGUN LOS PARAMETROS PAGOS ONLINE
                nombreArchivo = String.Concat(IdCuentaBancoEpicor, "_", DateTime.Now.ToString("ddMMyyyy"), "_PO", Contrato.ToString("ddMMyyyy"),
                                              "_", writeMilitaryTime(DateTime.Now), ".txt");

                sw = new StreamWriter(Directorio + nombreArchivo, false);
                ArrayList line1EA = new ArrayList() { "1EA", tabla.Tables[0].Rows[ciclo].ItemArray[3].ToString(),
                                                    CodigoBanco, NumCuenta, TipoCuenta}; //ARREGLO QUE LLEVA LA FECHA DE TRANSACCION, CODIGO DE TRANSITO,
                //NUMERO DE CUENTA, TIPO DE CUENTA Y CONSECUTIVO DEL ARCHIVO.
                ArrayList line2EL = new ArrayList() { "2EL" };
                ArrayList line3DT = new ArrayList(); //ARREGLO LLEVA EL CONTRATO,VALOR
                ArrayList line4CL = new ArrayList() { "4CL" }; //ARREGLO LLEVA EL TOTAL DEL REGISTRO DEL LOTE Y EL TOTAL DEL VALOR DE SERVICIO
                ArrayList line5CA = new ArrayList() { "5CA" }; //ARREGLO LLEVA EL TOTAL DEL REGISTRO DEL LOTE Y EL TOTAL DEL VALOR DE SERVICIO

                Fecha = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));

                //SE ADICIONA UN CONSECUTIVO SEGUN EL NUMERO DE ARCHIVO QUE EXISTAN DEL MISMO DIA
                int numColumnas = 0;
                HistorialArchivosEntradaLN columnas = new HistorialArchivosEntradaLN();
                numColumnas = columnas.consultarConsecutivoXBanco(IdCuentaBancoEpicor, TipoProcesoXCuenta, Fecha, Contrato.ToString("dd/MM/yyyy")).Rows.Count;

                if (numColumnas == 0)
                    ConsecutivoArchivo = "A";
                else
                    ConsecutivoArchivo = consecutivo(numColumnas);
                

                #region Armar Linea Encabezado Archivo

                line1EA.Add(ConsecutivoArchivo);
                LineaArmada1EA = armarLineas(line1EA, ListaLinea1EA);
                sw.WriteLine(LineaArmada1EA);
                llenarTablaParaActualizados(Fecha, Contrato.ToString("dd/MM/yyyy"), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada1EA);
                registrosLote += 1;

                #endregion

                #region Armar Linea Encabezado Lote

                LineaArmada2EL = armarLineas(line2EL, ListaLinea2EL);
                sw.WriteLine(LineaArmada2EL);
                llenarTablaParaActualizados(Fecha, Contrato.ToString("dd/MM/yyyy"), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada2EL);
                registrosLote += 1;

                #endregion

                #region Armar Linea Detalle

                for (int i = ciclo; i < ciclo + Limite; i++)
                {          

                    line3DT.Add("3DT");
                    if(String.IsNullOrEmpty(tabla.Tables[0].Rows[i].ItemArray[4].ToString().Trim()))
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[0].ToString().Trim());//CONTRATO
                    else
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[4].ToString().Trim());//CONTRATO

                    if (tabla.Tables[0].Rows[i].ItemArray[2].ToString().Contains("."))
                    {
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[2].ToString().Replace(".", ",").Trim());//SERVICIO PRINCIPAL    (MRT Valor?)                   
                    }
                    else
                    {
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[2].ToString().Trim());//SERVICIO PRINCIPAL                    
                    }

                    //Tarjeta Credito

                    if (String.IsNullOrEmpty(tabla.Tables[0].Rows[i].ItemArray[5].ToString().Trim())) { }
                    //Nada
                    else
                    {
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[5].ToString().Trim());//Medio Pago Aplica Sico
                    }

                    if (String.IsNullOrEmpty(tabla.Tables[0].Rows[i].ItemArray[6].ToString().Trim())) { }
                    //Nada
                    else
                    {
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[6].ToString().Trim());//Medio Pago Aplica Ventas
                    }

                    if (String.IsNullOrEmpty(tabla.Tables[0].Rows[i].ItemArray[7].ToString().Trim())) { }
                    //Nada
                    else
                    {
                        line3DT.Add(tabla.Tables[0].Rows[i].ItemArray[7].ToString().Trim());//Codigo Autorizacion
                    }
//---------------------------------
                    
                    LineaArmada3DT = armarLineas(line3DT, ListaLinea3DT);
                    sw.WriteLine(LineaArmada3DT);
                    llenarTablaParaActualizados(Fecha, Contrato.ToString("dd/MM/yyyy"), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada3DT);
                    line3DT.Clear();
                    registrosLote += 1;

                    if (tabla.Tables[0].Rows[i].ItemArray[2].ToString().Contains("."))
                        valorServicio += Convert.ToDouble(tabla.Tables[0].Rows[i].ItemArray[2].ToString().Replace(".", ",").Trim());
                    else
                        valorServicio += Convert.ToDouble(tabla.Tables[0].Rows[i].ItemArray[2].ToString().Trim());

                    LineaArmada3DT = String.Empty;

                }

                #endregion

                #region Armar Linea Control Lote

                line4CL.Add(registrosLote - 2);
                line4CL.Add(valorServicio);
                LineaArmada4CL = armarLineas(line4CL, ListaLinea4CL);
                sw.WriteLine(LineaArmada4CL);
                llenarTablaParaActualizados(Fecha, Contrato.ToString("dd/MM/yyyy"), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada4CL);
                registrosLote += 1;

                #endregion

                #region Armar Linea Control Archivo

                line5CA.Add(registrosLote - 3);
                line5CA.Add(valorServicio);
                LineaArmada5CA = armarLineas(line5CA, ListaLinea5CA);
                sw.WriteLine(LineaArmada5CA);
                llenarTablaParaActualizados(Fecha, Contrato.ToString("dd/MM/yyyy"), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada5CA);
                registrosLote += 1;

                #endregion

                sw.Close();

                //AQUI

               // Invocar servicio BancoDtlArchivosProcesados pagos


            }
            catch
            {
                sw.Close();
                File.Delete(Directorio + nombreArchivo);
                throw new System.Exception("Ocurrio un error al crear archivo plano");
            }

        }

        //SE GUARDA TODAS LAS LINEAS DEL ARCHIVO PAGOS ONLINE EN LA BASE DE DATOS
        private void guardarLineas(String fecha, String fechaTransaccion, String IdCuentaBancoEpicor, String Consecutivo, String Linea)
        {

            int valor = 0;
            HistorialArchivosEntrada objEntidad = new HistorialArchivosEntrada();
            HistorialArchivosEntradaLN objHA = new HistorialArchivosEntradaLN();
            objEntidad.pFecha = fecha;
            objEntidad.pFechaTransaccion = fechaTransaccion;
            objEntidad.pIdCuentaBanco = IdCuentaBancoEpicor;
            objEntidad.pTipoArchivo = TipoProcesoXCuenta;
            objEntidad.pConsecutivo = Consecutivo;
            objEntidad.pLineaDetalle = Linea;
            valor = objHA.insertar(objEntidad);
            if (valor <= 0)
            {
                throw new System.Exception("Ocurrio un error al guardar archivo plano en la base de datos");
            }
        }

        // SE CARGA LOS PARAMETROS DE LA ESTRUCTURA DE LA LINEAS PAGOS ONLINE PARA GENERAR EL ARCHIVO PAGOS ONLINE
        private List<Bancos.EN.Tablas.EstructuraArchivo> consultarEstructuraPagosOnline(String tipoLinea, String tipoProceso)
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

        //SE ARMA CADA LINEA DE PAGOS ONLINE SEGUN LOS PARAMETROS DE LA ESTRUCTURA
        private String armarLineas(ArrayList lineaDatos, List<Bancos.EN.Tablas.EstructuraArchivo> lineasPagosOnline)
        {

            String linea = String.Empty;
            String valor = String.Empty;
            char caracterRelleno;

            foreach (Bancos.EN.Tablas.EstructuraArchivo objAso in lineasPagosOnline)
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
                            if (objAso.pNombreCampo.Equals("Fecha del recaudo"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Codigo Entidad Financiera Recaudadora"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            else if (objAso.pNombreCampo.Equals("Numero de cuenta"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[3]));
                            else if (objAso.pNombreCampo.Equals("Fecha del archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Hora de grabacion del archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Modificador de archivo"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[5]));
                            else if (objAso.pNombreCampo.Equals("Tipo de Cuenta"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[4]));
                            break;
                        case "2EL":
                            break;
                        case "3DT":
                            if (objAso.pNombreCampo.Equals("Referencia principal del usuario"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Valor recaudado"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            //Tarjeta Credito       
                            else if (objAso.pNombreCampo.Equals("Codigo de la sucursal"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[3]));//Medio PAgo Aplica Sico
                            else if (objAso.pNombreCampo.Equals("Codigo de la entidad financiera debitada"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[4]));//Medio PAgo Aplica Ventas
                            else if (objAso.pNombreCampo.Equals("No. de Autorizacion"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[5]));//Codigo Autorizacion
                            //-------
                            break;
                        

                        case "4CL":
                            if (objAso.pNombreCampo.Equals("Total registros en lote"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Valor total recaudado en lote"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            break;
                        case "5CA":
                            if (objAso.pNombreCampo.Equals("Total registros recaudados en archivo"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[1]));
                            else if (objAso.pNombreCampo.Equals("Valor total recaudado en archivo"))
                                valor = armarCampo(objAso, Convert.ToString(lineaDatos[2]));
                            break;
                    }
                    linea += rellenarCampo(valor, objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                }
            }
            return linea;
        }

        //SE RELLENAN LOS CAMPOS DEL ARCHIVO PAGOS ONLINE CON CEROS O ESPACIOS SEGUN EL PARAMETRO
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

        //SE CALCULA EL CONSECUTIVO DE UN ARCHIVO PAGOS ONLINE QUE FUE GENEREADO MAS DE UNA VEZ EN EL MISMO DIA
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

        //LA ESTRUCTURA DEL BANCO ESTA CREADA PARA QUE TENGA SOLO LINEA DE DETALLES
        public List<Bancos.EN.Tablas.EstructuraArchivo> consultarEstructuraArchivoBanco(String IdCuentaBancoEpicor, String tipLinea)
        {
            List<Bancos.EN.Tablas.EstructuraArchivo> lista = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            DataTable tabla = new EstructuraArchivoLN().consultarEstructuraBanco(IdCuentaBancoEpicor, tipLinea, TipoProcesoXCuenta);
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

    }
}
