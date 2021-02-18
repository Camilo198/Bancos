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
using Bancos.PS.Servicios.Archivos;

using System.Data.Common;
//using System.Data.OleDb;

using System.Text.RegularExpressions;
using SpreadsheetLight;

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "TarjetasCredito" en el código, en svc y en el archivo de configuración a la vez.
    public class TarjetasCredito : ITarjetasCredito
    {

        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea1EA { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea2EL { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea3DT { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea4CL { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaLinea5CA { get; set; }
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoEncabezadoArchivo { get; set; }//LISTA CON LA ESTRUCTURA DE UN BANCO ( ENCABEZADO ARCHIVO )
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoEncabezadoLote { get; set; }//LISTA CON LA ESTRUCTURA DE UN BANCO ( ENCABEZADO LOTE )
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoControlArchivo { get; set; }//LISTA CON LA ESTRUCTURA DE UN BANCO ( CONTROL ARCHIVO )
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoControlLote { get; set; }//LISTA CON LA ESTRUCTURA DE UN BANCO ( CONTROL LOTE )
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoDetalle { get; set; }//LISTA CON LA ESTRUCTURA DE UN BANCO ( DETALLE)
        private List<Bancos.EN.Tablas.InterpreteArchivo> ListaEquivalenciaArchivo { get; set; }//OBJETO CON LAS EQUIVALENCIAS ENTRE ESTRUCTURA BANCOS Y ESTRUCTURA ASOBANCARIA RECAUDO

        String LineaArmada1EA = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO ARCHIVO
        String LineaArmada2EL = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO LOTE
        String LineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE Y VA VARIANDO EN CADA CICLO 
        String LineaArmada4CL = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE LOTE
        String LineaArmada5CA = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE ARCHIVO

        DataTable tablaCE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA DE LOS CAMPOS_EQUIVALENCIAS
        DataTable tablaE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA EQUIVALENCIAS 

        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO ASOBANCARIA
        String nombre = String.Empty;  // contiene nombre del archivo original
        String Directorio = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA EL ARCHIVO ASOBANCARIA     
        String tipoArchivo = String.Empty;
        static StreamWriter sw;
        String ConsecutivoArchivo; // CONTIENE UN CONSECUTIVO QUE SE USA PARA CONTROLAR CUANTAS VECES SE HA GENERADO EL ARCHIVO EN EL DIA
        String Fecha;
        int registrosLote = 0;
        ArrayList LimitesSuperior = new ArrayList();
        int ciclo = 0;
        String tipocuentabanco = String.Empty;
        String TipoProcesoXCuenta = String.Empty;

        int LineaExcluidaIncio = 0;
        int LineaExcluidaFin = 0;
        int NumeroHoja = 0;
        String NomHoja = " ";
        bool EsExcel = false;
        string CaracterDecimal = "";

        #region TABLA CON LINEAS PARA GUARDAR EN LA BD
        DataTable LineasArmadasdt = new DataTable();
        #endregion

        public String ServicioTarjetasCredito(String NombreCuenta, String IdCuentaBanco, String IdCuentaBancoEpicor, String RutaEntrada,
                                              String RutaSalida, ArrayList CorreosControl, String Remitente,
                                              String CodigoBanco, String NumCuenta, String TipoCuenta,
                                              String Usuario, String RutaProcesado, String TipoProceso)
        {

            TipoProcesoXCuenta = TipoProceso;

            switch (TipoCuenta)
            {
                case "Ahorros":
                    {
                        tipocuentabanco = "1";
                        break;
                    }
                case "Corriente":
                    {
                        tipocuentabanco = "2";
                        break;
                    }
                default:
                    {
                        tipocuentabanco = "0";
                        break;
                    }
            }

            #region OBTENER FORMATO, NUMERO HOJA Y LINEAS EXCLUIDAS EN EL ARCHIVO
            ArchivoPlano objAP = new ArchivoPlano();
            objAP.pIdCuentaBanco = IdCuentaBancoEpicor;
            objAP.pTipoProceso = TipoProcesoXCuenta;
            List<ArchivoPlano> lista = new ArchivoPlanoLN().consultar(objAP);
            if (lista.Count > 0)
            {
                LineaExcluidaIncio = lista[0].pLineasExcluidasInicio.Value;
                LineaExcluidaFin = lista[0].pLineasExcluidasFin.Value;
                NumeroHoja = lista[0].pNumeroHojaExcel.Value - 1;
                NomHoja = lista[0].pNomHoja.ToString().Trim() + "$";
                EsExcel = lista[0].pEsExcel.Value;
                CaracterDecimal = lista[0].pCaracterDecimal.ToString();
            }

            #endregion

            #region CREAR COLUMNAS DE LA TABLA CON LOS DATOS PARA GUARDAR LAS LINEAS EN UNA BD
            LineasArmadasdt.Columns.Add("fecha");
            LineasArmadasdt.Columns.Add("fechaContrato");
            LineasArmadasdt.Columns.Add("IdCuentaBanco");
            LineasArmadasdt.Columns.Add("consecutivo");
            LineasArmadasdt.Columns.Add("lineaArmada");
            #endregion

            Logs objL = new Logs();
            EnviarCorreo Correo = new EnviarCorreo();
            ConectorFTP Ftp = new ConectorFTP();
            tipoArchivo = "Tarjeta Credito";
            Directorio = @RutaSalida;
            //ESTRUCTURA DEL ARCHIVO DEL BANCO (ARCHIVO DE ENTRADA)
            //*********************************************************************************
            ListaEstructuraArchivoBancoEncabezadoArchivo = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaEstructuraArchivoBancoEncabezadoArchivo.AddRange(consultarEstructuraArchivoBanco(IdCuentaBancoEpicor, "1EA"));
            ListaEstructuraArchivoBancoEncabezadoLote = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaEstructuraArchivoBancoEncabezadoLote.AddRange(consultarEstructuraArchivoBanco(IdCuentaBancoEpicor, "2EL"));
            ListaEstructuraArchivoBancoDetalle = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaEstructuraArchivoBancoDetalle.AddRange(consultarEstructuraArchivoBanco(IdCuentaBancoEpicor, "3DT"));
            ListaEstructuraArchivoBancoControlLote = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaEstructuraArchivoBancoControlLote.AddRange(consultarEstructuraArchivoBanco(IdCuentaBancoEpicor, "4CL"));
            ListaEstructuraArchivoBancoControlArchivo = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaEstructuraArchivoBancoControlArchivo.AddRange(consultarEstructuraArchivoBanco(IdCuentaBancoEpicor, "5CA"));
            //*********************************************************************************

            //ESTRUCTURA DEL ARCHIVO ASOBANCARIA RECAUDO (ARCHIVO DE SALIDA)
            //*********************************************************************************
            ListaLinea1EA = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaLinea2EL = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaLinea3DT = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaLinea4CL = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaLinea5CA = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            ListaLinea1EA.AddRange(consultarEstructuraAsobancaria("1EA", TipoProcesoXCuenta));
            ListaLinea2EL.AddRange(consultarEstructuraAsobancaria("2EL", TipoProcesoXCuenta));
            ListaLinea3DT.AddRange(consultarEstructuraAsobancaria("3DT", "ABT1")); //TipoProcesoXCuenta));
            ListaLinea4CL.AddRange(consultarEstructuraAsobancaria("4CL", TipoProcesoXCuenta));
            ListaLinea5CA.AddRange(consultarEstructuraAsobancaria("5CA", TipoProcesoXCuenta));
            //*********************************************************************************

            //LISTA DE OBJETOS CON LAS EQUIVALENCIAS ENTRE LAS ESTRUCTURAS DEL ARCHIVO DEL BANCO
            //Y LA ASOBANCARIA RECAUDO
            //*********************************************************************************
            ListaEquivalenciaArchivo = new List<Bancos.EN.Tablas.InterpreteArchivo>();
            ListaEquivalenciaArchivo.AddRange(obtenerTransformacionArchivo(IdCuentaBancoEpicor));
            //*********************************************************************************
            FileStream fs;
            SLDocument sl;
            int errores = 0;
            string err_log = "";
            try
            {
                LectorArchivos objLector = new LectorArchivos();
                List<String> listaArchivos = objLector.listarDirectorio(RutaEntrada);

                foreach (String archivo in listaArchivos)
                {
                    try
                    {


                        List<String> lineasArchivo = new List<string>();
                        nombre = System.IO.Path.GetFileName(archivo);
                        #region LECTURA ARCHIVO EXCEL
                        System.Data.DataSet DsExcel = new System.Data.DataSet();
                        if (EsExcel)
                        {
                            System.Data.DataTable DtExcel = new System.Data.DataTable();
                            System.Data.DataRow DrExcel = null;
                            string path = System.IO.Path.GetFullPath(archivo);

                            //  OleDbConnection oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=" + "'" + "Excel 12.0;HDR=YES;IMEX=0;" + "'");
                            //OleDbConnection oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source="   + path + ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"");

                            try
                            {
                                //OleDbConnection oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                sl = new SLDocument(fs);

                                String[] hojas = sl.GetSheetNames().ToArray();

                                if (RutaEntrada.Contains("Falabella"))
                                {
                                    sl.SelectWorksheet(NomHoja);
                                }
                                else
                                {
                                    sl.SelectWorksheet(hojas[NumeroHoja]);
                                }

                                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();

                                int ultimaCol = stats.EndColumnIndex;
                                int ultimaFil = stats.EndRowIndex;
                                int columnaInicio = stats.StartColumnIndex;
                                int filaInicio = stats.StartRowIndex;

                                for (int h = 1; h <= stats.EndColumnIndex; h++)
                                {
                                    DtExcel.Columns.Add(h.ToString());
                                }

                                for (int i = filaInicio; i <= ultimaFil; i++)
                                {
                                    DrExcel = DtExcel.NewRow();
                                    for (int j = columnaInicio; j < ultimaCol; j++)
                                    {
                                        //tomamos los valores de las celdas para el datatable
                                        if (sl.GetCellValueAsString(i, j) != null)
                                        {
                                            string val = sl.GetCellValueAsString(i, j);
                                            DrExcel[j.ToString()] = val;
                                        }

                                    }
                                    DtExcel.Rows.Add(DrExcel);
                                }
                                DsExcel.Tables.Add(DtExcel);

                                fs.Close();
                                sl.Dispose();
                                //oledbConn.Open();

                                //OleDbCommand cmd = new OleDbCommand();

                                //OleDbDataAdapter oleda = new OleDbDataAdapter();

                                //DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                                //string sheetName = string.Empty;

                                //if (dt != null)
                                //{

                                //    sheetName = dt.Rows[NumeroHoja]["TABLE_NAME"].ToString();

                                //}

                                //cmd.Connection = oledbConn;

                                //cmd.CommandType = CommandType.Text;
                                //if (RutaEntrada.Contains("Falabella"))
                                //    cmd.CommandText = "SELECT * FROM [" + NomHoja + "]";
                                //else
                                //    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                                //oleda = new OleDbDataAdapter(cmd);

                                //oleda.Fill(DsExcel, "excelData");

                                //oledbConn.Close();

                            }
                            catch (Exception ex)
                            {
                                Correo.enviarNotificacionesError(NombreCuenta, (String[])CorreosControl.ToArray(typeof(String)), Remitente, ex.Message, tipoArchivo + " " + archivo);
                                return ex.Message;
                            }
                            //finally
                            //{
                            //    oledbConn.Close();
                            //}
                            //Llenamos una lista de String con todas las filas del dataset, agregando ceros o espacion segun su parametrizacion
                            foreach (DataRow row in DsExcel.Tables[0].Rows)
                            {
                                String lineaExcel = String.Empty;
                                List<String> lineaDatos = new List<String>();

                                for (int i = 0; i < row.ItemArray.Length; i++)
                                {

                                    lineaExcel = lineaExcel + row[i].ToString();
                                    lineaDatos.Add(row[i].ToString().Replace(@"$", "").Replace(@"-", "")); //.Replace(@".", "")); //.Replace(@",", "") se quita para no elimar la parte decimal y luego poder redondear
                                }

                                if (lineaExcel != null && lineaExcel != String.Empty)
                                {
                                    if (ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0 && lineasArchivo.Count == 0)
                                        lineaExcel = armarLineasExcel(lineaDatos, ListaEstructuraArchivoBancoEncabezadoArchivo, CaracterDecimal);
                                    else if ((ListaEstructuraArchivoBancoEncabezadoLote.Count > 0 && lineasArchivo.Count == 0) ||
                                    (ListaEstructuraArchivoBancoEncabezadoLote.Count > 0 && lineasArchivo.Count == 1 && ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0))
                                        lineaExcel = armarLineasExcel(lineaDatos, ListaEstructuraArchivoBancoEncabezadoLote, CaracterDecimal);
                                    else
                                        lineaExcel = armarLineasExcel(lineaDatos, ListaEstructuraArchivoBancoDetalle, CaracterDecimal);

                                    lineasArchivo.Add(lineaExcel);
                                }
                            }
                        }

                        #endregion
                        #region LECTURA ARCHIVO PLANO
                        else
                        {
                            if (archivo.Contains(".csv"))
                            {
                                lineasArchivo = objLector.leerArchivoTarjetasCSV(archivo, ListaEstructuraArchivoBancoDetalle);
                            }
                            else
                            {
                                lineasArchivo = objLector.leerArchivoTarjetas(archivo);
                            }

                        }
                        #endregion

                        DataSet ds1 = new DataSet(); // DATASET CON LOS PAGOS
                        DataTable dt1 = new DataTable();
                        dt1.Columns.Add("fecha");
                        dt1.Columns.Add("linea");

                        if (lineasArchivo.Count > 0)
                        {

                            lineasArchivo.RemoveRange(0, LineaExcluidaIncio);//ELIMINA TODAS LAS LINEAS DEL INICIO QUE SON EXCLUIDAS POR CONFIGURACION

                            String fechaRecaudoGeneral = String.Empty;
                            if (ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0)
                            {
                                //ESTE CODIGO SIRVE PARA SACAR LA FECHA DEL RECAUDO SI VIENE EN EL ENCABEZADO                            
                                Dictionary<Int32, String> CamposEncab = new Dictionary<Int32, String>();
                                //LOS SIGUIENTES DOS FOREACH SE CREA PARA BUSCAR LA FECHA DE RECAUDO EN LA 
                                //LINEA DEL ARCHIVO 
                                //**************************************************************************                      
                                foreach (Bancos.EN.Tablas.EstructuraArchivo objAso in ListaEstructuraArchivoBancoEncabezadoArchivo)
                                {
                                    foreach (Bancos.EN.Tablas.InterpreteArchivo objEq in ListaEquivalenciaArchivo)
                                    {
                                        if (objEq.pCampoBanco == objAso.pOid)
                                        {
                                            if (objEq.pNombreCampoAsobancaria.Equals("Fecha del recaudo"))
                                            {
                                                CamposEncab = valorCampo(lineasArchivo[0], ListaEstructuraArchivoBancoEncabezadoArchivo);
                                                fechaRecaudoGeneral = CamposEncab[objEq.pCampoBanco.Value];
                                                break;
                                            }
                                        }
                                    }
                                }
                                //**************************************************************************
                            }


                            //if (NombreCuenta != "TCR")
                            //{
                            if (ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0)
                                lineasArchivo.RemoveAt(0);
                            if (ListaEstructuraArchivoBancoEncabezadoLote.Count > 0)
                                lineasArchivo.RemoveAt(0);
                            if (ListaEstructuraArchivoBancoControlArchivo.Count > 0)
                                lineasArchivo.RemoveAt(lineasArchivo.Count - 2);
                            if (ListaEstructuraArchivoBancoControlLote.Count > 0)
                                lineasArchivo.RemoveAt(lineasArchivo.Count - 1);

                            lineasArchivo.RemoveRange(lineasArchivo.Count - LineaExcluidaFin, LineaExcluidaFin);//ELIMINA TODAS LAS LINEAS DEL FIN QUE SON EXCLUIDAS POR CONFIGURACION    
                                                                                                                //}


                            foreach (String linea in lineasArchivo)
                            {
                                if (linea != null)
                                {
                                    String fechaRecaudo = String.Empty;
                                    if (String.IsNullOrEmpty(fechaRecaudoGeneral))
                                    {
                                        Dictionary<Int32, String> Campos = new Dictionary<Int32, String>();
                                        Campos = valorCampo(linea, ListaEstructuraArchivoBancoDetalle);
                                        //LOS SIGUIENTES DOS FOREACH SE CREA PARA BUSCAR LA FECHA DE RECAUDO EN CADA 
                                        //LINEA DEL ARCHIVO PARA SEPARAR EL ARCHIVO POR FECHAS                               
                                        //**************************************************************************
                                        //**************************************************************************

                                        foreach (Bancos.EN.Tablas.EstructuraArchivo objAso in ListaLinea1EA)
                                        {
                                            foreach (Bancos.EN.Tablas.InterpreteArchivo objEq in ListaEquivalenciaArchivo)
                                            {
                                                if (objEq.pCampoAsobancaria == objAso.pOid)
                                                {
                                                    if (objAso.pNombreCampo.Equals("Fecha del recaudo"))
                                                    {
                                                        fechaRecaudo = Campos[objEq.pCampoBanco.Value];
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        //**************************************************************************
                                        //**************************************************************************
                                    }
                                    DataRow dr1 = dt1.NewRow();
                                    if (String.IsNullOrEmpty(fechaRecaudo))
                                        dr1["fecha"] = fechaRecaudoGeneral;
                                    else
                                        dr1["fecha"] = fechaRecaudo;
                                    dr1["linea"] = linea;
                                    dt1.Rows.Add(dr1);
                                }
                            }
                        }
                        DataView dv = dt1.DefaultView;
                        dv.Sort = "fecha";
                        dt1 = dv.ToTable();
                        ds1.Tables.Add(dt1);
                        //**************************************************************************
                        //**************************************************************************

                        Dictionary<Int32, String> asignacionCampos = new Dictionary<Int32, String>();
                        asignacionCampos = valorCampo(dt1.Rows[0].ItemArray[1].ToString(), ListaEstructuraArchivoBancoDetalle);

                        //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS DE TARJETAS CREDITO SI NO EXISTE
                        if (!Directory.Exists(Directorio))
                        {
                            System.IO.Directory.CreateDirectory(Directorio);
                        }
                        int limite = 0;
                        bool valido = false;
                        //EN ESTE IF SE CREAN LOS ARCHIVOS CON LOS PAGOS 
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            LimitesSuperior = obtenerLimites(ds1);
                            for (int i = 0; i < LimitesSuperior.Count; i++)
                            {
                                valido = int.TryParse(LimitesSuperior[i].ToString(), out limite);
                                if (valido)
                                {
                                    armarArchivo(ds1, limite, IdCuentaBanco, IdCuentaBancoEpicor, CodigoBanco, NumCuenta, tipocuentabanco);
                                }
                                else
                                {
                                    limite = (int)LimitesSuperior[i];
                                    armarArchivo(ds1, limite, IdCuentaBanco, IdCuentaBancoEpicor, CodigoBanco, NumCuenta, tipocuentabanco);
                                }

                                if (CorreosControl.Count > 0)
                                    Correo.enviarNotificaciones(Directorio, (String[])CorreosControl.ToArray(typeof(String)), nombreArchivo, Remitente,
                                                                registrosLote, tipoArchivo);
                                ciclo = ciclo + Convert.ToInt16(LimitesSuperior[i]);
                                //SE GUARDA EL ESTADO DEL PROCESO
                                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                                objL.pUsuario = Usuario;
                                objL.pDetalle = NombreCuenta + ", " + tipoArchivo + " : Archivo " + tipoArchivo + " con fecha de transaccion " + nombreArchivo.Substring(18, 8) + " fue generado correctamente";
                                objL.pTipoArchivo = TipoProcesoXCuenta;
                                objL.pTipoProceso = "GEN";
                                new LogsLN().insertar(objL); // DESCOMENTAREAR A PRD
                                nombreArchivo = String.Empty;
                                registrosLote = 0;

                                limite = 0;
                                valido = false;
                            }
                            ciclo = 0;
                            LimitesSuperior.Clear();
                        }

                        //AQUI Validar porque genera error y descomentariar
                        //foreach (DataRow row in LineasArmadasdt.Rows)
                        //{
                        //    guardarLineas(row[0].ToString(), row[1].ToString(), row[2].ToString(),
                        //                  row[3].ToString(), row[4].ToString());
                        //}
                        LineasArmadasdt.Clear();

                        // objLector.borrarArchivo(archivo);

                        System.Threading.Thread.Sleep(1000);
                        string fecha = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString() + "_" +
                          DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + "_";

                        File.Move(archivo, RutaSalida + fecha + nombre);

                    }
                    catch (Exception ex)
                    {
                        errores += 1;
                        err_log = err_log + " --- " + ex.ToString();

                    }
                }

                if (errores > 0)
                {
                    throw new Exception("Proceso " + tipoArchivo + "Con errores: " + errores.ToString() + " en "+ listaArchivos.Count.ToString() + " archivos procesados " + err_log );
                }
                else
                {
                    return "Proceso " + tipoArchivo + " ejecutado con exito!!";
                }
            }
            catch (Exception ex)
            {
                //if (CorreosControl.Count > 0)

                Correo.enviarNotificacionesError(NombreCuenta, (String[])CorreosControl.ToArray(typeof(String)), Remitente, ex.ToString(), tipoArchivo + " " + nombreArchivo);

                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = NombreCuenta + ", " + tipoArchivo + " : " + ex.Message;
                objL.pTipoArchivo = TipoProcesoXCuenta;
                objL.pTipoProceso = "GEN";
                new LogsLN().insertar(objL);   // DESCOMENTAREAR A PRD
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
                    fechas = Convert.ToDateTime(renglon[0].ToString());
                if (fechas.ToShortDateString() == Convert.ToDateTime(renglon[0].ToString()).ToShortDateString())
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
                fechas = Convert.ToDateTime(renglon[0].ToString());
                tot += 1;
            }
            return limite;
        }

        private void armarArchivo(DataSet tabla, int Limite, String IdCuentaBanco, String IdCuentaBancoEpicor, String CodigoBanco,
                                     String NumCuenta, String TipoCuenta)
        {

            try
            {
                System.Threading.Thread.Sleep(1000);
                String ControlHora = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');
                tablaCE = new CamposEquivalenciasLN().consultarCampoEquivalencias(TipoProcesoXCuenta, IdCuentaBanco);
                tablaE = new EquivalenciasLN().consultarEquivalenciasXTipoArchivo(TipoProcesoXCuenta, IdCuentaBancoEpicor);

                Dictionary<Int32, String> asignacionCampos = new Dictionary<Int32, String>();
                double valorRecaudado = 0;
                String FechaTransaccion = tabla.Tables[0].Rows[ciclo].ItemArray[0].ToString().Trim();

                Fecha = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));

                //SE ADICIONA UN CONSECUTIVO SEGUN EL NUMERO DE ARCHIVO QUE EXISTAN DEL MISMO DIA
                int numColumnas = 0;

                HistorialArchivosEntradaLN columnas = new HistorialArchivosEntradaLN();
                numColumnas = columnas.consultarConsecutivoXBanco(IdCuentaBancoEpicor, TipoProcesoXCuenta, Fecha, Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("dd/MM/yyyy"))).Rows.Count;

                if (numColumnas == 0)
                    ConsecutivoArchivo = "A";
                else
                    ConsecutivoArchivo = consecutivo(numColumnas);

                //SE CREA EL NOMBRE DEL ARCHIVO SEGUN LOS PARAMETROS ASOBANCARIA
                nombreArchivo = String.Concat(IdCuentaBancoEpicor, "_", DateTime.Now.ToString("ddMMyyyy"),
                                              "_", "TC", "_",
                                              Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("ddMMyyyy")),
                                              "_", ControlHora, "_", ConsecutivoArchivo, ".txt");
                sw = new StreamWriter(Directorio + nombreArchivo, false);


                #region Armar Linea Encabezado Archivo

                asignacionCampos.Add(0, "1EA");
                asignacionCampos.Add(1, CodigoBanco);
                asignacionCampos.Add(2, NumCuenta);
                asignacionCampos.Add(3, FechaTransaccion);
                asignacionCampos.Add(4, ConsecutivoArchivo);
                asignacionCampos.Add(5, TipoCuenta);
                LineaArmada1EA = armarLineas(asignacionCampos, ListaLinea1EA, ListaEquivalenciaArchivo);
                sw.WriteLine(LineaArmada1EA);
                llenarTablaParaActualizados(Fecha, Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("dd/MM/yyyy")), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada1EA);
                registrosLote += 1;
                asignacionCampos.Clear();

                #endregion

                #region Armar Linea Encabezado Lote

                asignacionCampos.Add(0, "2EL");
                LineaArmada2EL = armarLineas(asignacionCampos, ListaLinea2EL, ListaEquivalenciaArchivo);
                sw.WriteLine(LineaArmada2EL);
                llenarTablaParaActualizados(Fecha, Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("dd/MM/yyyy")), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada2EL);
                registrosLote += 1;
                asignacionCampos.Clear();

                #endregion

                #region Armar Linea Detalle

                for (int i = ciclo; i < ciclo + Limite; i++)
                {

                    asignacionCampos.Clear();
                    asignacionCampos = valorCampo(tabla.Tables[0].Rows[i].ItemArray[1].ToString(), ListaEstructuraArchivoBancoDetalle);

                    //ESTE FOREACH SE CREO PARA PODER HACER LA OPERACION DE CALCULAR EL VALOR RECAUDADO
                    //**************************************************************************
                    foreach (Bancos.EN.Tablas.EstructuraArchivo objAso in ListaLinea3DT)
                    {
                        foreach (Bancos.EN.Tablas.InterpreteArchivo objEq in ListaEquivalenciaArchivo)
                        {
                            if (objEq.pCampoAsobancaria == objAso.pOid)
                            {
                                if (objAso.pNombreCampo.Equals("Valor recaudado"))
                                {
                                    valorRecaudado += Convert.ToDouble(asignacionCampos[objEq.pCampoBanco.Value]);
                                    break;
                                }
                            }
                        }
                    }

                    LineaArmada3DT = armarLineas(asignacionCampos, ListaLinea3DT, ListaEquivalenciaArchivo);
                    sw.WriteLine(LineaArmada3DT);
                    llenarTablaParaActualizados(Fecha, Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("dd/MM/yyyy")), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada3DT);
                    registrosLote += 1;
                    LineaArmada3DT = String.Empty;

                }

                asignacionCampos.Clear();

                #endregion

                #region Armar Linea Control Lote

                asignacionCampos.Add(0, "4CL");
                asignacionCampos.Add(1, Convert.ToString(registrosLote - 2));
                asignacionCampos.Add(2, Convert.ToString(valorRecaudado));
                LineaArmada4CL = armarLineas(asignacionCampos, ListaLinea4CL, ListaEquivalenciaArchivo);
                sw.WriteLine(LineaArmada4CL);
                llenarTablaParaActualizados(Fecha, Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("dd/MM/yyyy")), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada4CL);
                registrosLote += 1;
                asignacionCampos.Clear();

                #endregion

                #region Armar Linea Control Archivo

                asignacionCampos.Add(0, "5CA");
                asignacionCampos.Add(1, Convert.ToString(registrosLote - 3));
                asignacionCampos.Add(2, Convert.ToString(valorRecaudado));
                LineaArmada5CA = armarLineas(asignacionCampos, ListaLinea5CA, ListaEquivalenciaArchivo);
                sw.WriteLine(LineaArmada5CA);
                llenarTablaParaActualizados(Fecha, Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("dd/MM/yyyy")), IdCuentaBancoEpicor, ConsecutivoArchivo, LineaArmada5CA);
                registrosLote += 1;
                asignacionCampos.Clear();

                #endregion

                sw.Close();
            }
            catch (Exception ex)
            {
                sw.Close();
                File.Delete(Directorio + nombreArchivo);
                throw new System.Exception(ex.Message);
            }
        }

        public void llenarTablaParaActualizados(String fecha, String fechaTransaccion, String IdCuentaBancoEpicor, String Consecutivo, String Linea)
        {
            DataRow dr1 = LineasArmadasdt.NewRow();
            dr1["fecha"] = fecha;
            dr1["fechaContrato"] = fechaTransaccion;
            dr1["IdCuentaBanco"] = IdCuentaBancoEpicor;
            dr1["consecutivo"] = Consecutivo;
            dr1["lineaArmada"] = Linea;
            LineasArmadasdt.Rows.Add(dr1);

        }

        //SE ARMA CADA LINEA DE ASOBANCARIA SEGUN LOS PARAMETROS DE LA ESTRUCTURA
        private String armarLineasExcel(List<String> lineaDatos, List<Bancos.EN.Tablas.EstructuraArchivo> lineasAsobancaria, string CaracterDecimal)
        {
            int i = 0;
            try
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

                    //Aqui MRT
                    string Valor_ = lineaDatos[i].ToString();
                    string aqui = "";
                    decimal ValorD = 0;
                    if (objAso.pTipoDato == "DE")
                    {
                        if (objAso.pTipoDato == "DE" && lineaDatos[i].Contains(CaracterDecimal))
                        {

                            Valor_ = lineaDatos[i].ToString();
                            if (CaracterDecimal == ",")
                                ValorD = Math.Round(Convert.ToDecimal(Valor_), 0);
                            else
                                ValorD = Math.Round(Convert.ToDecimal(Valor_.Replace(".", ",")), 0);
                            linea += rellenarCampo(ValorD.ToString().Trim() + "00", objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                        }
                        else
                        {
                            linea += rellenarCampo(lineaDatos[i].ToString().Trim() + "00", objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                        }
                    }
                    else
                    {
                        if (lineaDatos[i].ToString() != null) // SAU Nueva estructura AMEX 
                        {
                            if (lineaDatos[i].ToString().Contains("*"))
                            {
                                lineaDatos[i] = lineaDatos[i].ToString().Replace("*", "").Trim(' ');
                            }
                        }

                        linea += rellenarCampo(lineaDatos[i].ToString().Trim(), objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                    }
                    i++;
                }
                return linea;
            }

            catch
            {
                throw new Exception(i.ToString());
            }
        }

        private String armarLineas(Dictionary<Int32, String> campos,
                                   List<Bancos.EN.Tablas.EstructuraArchivo> lineasAsobancaria,
                                   List<Bancos.EN.Tablas.InterpreteArchivo> equivalencias)
        {
            String linea = String.Empty;
            String valor = String.Empty;
            char caracterRelleno;

            //ArrayList camposSuma = new ArrayList();
            //ESTE FOREACH SE CREO PARA PODER HACER LA OPERACION DE CALCULAR LA COMISION
            //**************************************************************************
            //foreach (Bancos.EN.Tablas.EstructuraArchivo objAso in lineasAsobancaria)
            //{
            //    foreach (Bancos.EN.Tablas.InterpreteArchivo objEq in equivalencias)
            //    {
            //        if (objEq.pCampoAsobancaria == objAso.pOid)
            //        {
            //            if (objAso.pOid >= 98 && objAso.pOid <= 101)
            //                camposSuma.Add(campos[objEq.pCampoBanco.Value]);
            //        }
            //    }
            //}
            //**************************************************************************

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
                    switch (campos[0])
                    {
                        case "1EA":
                            if (objAso.pNombreCampo.Equals("Fecha del recaudo"))
                                valor = armarCampo(objAso, campos[3]);
                            else if (objAso.pNombreCampo.Equals("Código Entidad Financiera Recaudadora"))
                                valor = armarCampo(objAso, campos[1]);
                            else if (objAso.pNombreCampo.Equals("Número de cuenta"))
                                valor = armarCampo(objAso, campos[2]);
                            else if (objAso.pNombreCampo.Equals("Fecha del archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Hora de grabación del archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Modificador de archivo"))
                                valor = armarCampo(objAso, campos[4]);
                            else if (objAso.pNombreCampo.Equals("Tipo de Cuenta"))
                                valor = armarCampo(objAso, campos[5]);
                            break;
                        case "2EL":
                            break;
                        case "3DT":
                            //ESTE IF FUNCIONA PARA ASIGNAR VALORES A DECIMALES LOS CUALES NO SE LES ENCUENTRE EQUIVALENCIA
                            if (objAso.pTipoDato.Equals("DE"))
                                valor = armarCampo(objAso, "0");
                            else
                                valor = armarCampo(objAso, String.Empty);

                            foreach (Bancos.EN.Tablas.InterpreteArchivo objEq in equivalencias)
                            {
                                if (objEq.pCampoAsobancaria == objAso.pOid)
                                {
                                    valor = armarCampo(objAso, campos[objEq.pCampoBanco.Value]);
                                    break;
                                }
                            }
                            break;
                        case "4CL":
                            if (objAso.pNombreCampo.Equals("Total registros en lote"))
                                valor = armarCampo(objAso, campos[1]);
                            else if (objAso.pNombreCampo.Equals("Valor total recaudado en lote"))
                                valor = armarCampo(objAso, campos[2]);
                            break;
                        case "5CA":
                            if (objAso.pNombreCampo.Equals("Total registros recaudados en archivo"))
                                valor = armarCampo(objAso, campos[1]);
                            else if (objAso.pNombreCampo.Equals("Valor total recaudado en archivo"))
                                valor = armarCampo(objAso, campos[2]);
                            break;
                    }
                    linea += rellenarCampo(valor, objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                }
            }
            return linea;
        }

        //SE GUARDA TODAS LAS LINEAS DEL ARCHIVO TARJETAS DE CREDITO EN LA BASE DE DATOS
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

        //SE OBTIENE EL VALOR DEL CAMPO CON RELACION A LA ESTRUCTURA DEL ARCHIVO
        private Dictionary<Int32, String> valorCampo(String Linea, List<Bancos.EN.Tablas.EstructuraArchivo> ListaDeEstructuraArchivoBanco)
        {
            Dictionary<Int32, String> asignacion = new Dictionary<Int32, String>();
            try
            {
                asignacion.Add(0, "3DT");
                Int32 inicio = 0;
                String valor = String.Empty;
                //aqui
                string j = "";
                foreach (Bancos.EN.Tablas.EstructuraArchivo objEst in ListaDeEstructuraArchivoBanco)
                {
                    if (objEst.pNombreCampo == "Código Autorización")
                        j = "";
                    valor = String.Empty;
                    valor = armarCampoBanco(objEst, evaluarCampo(objEst, Linea.Substring(inicio, objEst.pLongitud.Value).Trim()));
                    asignacion.Add(objEst.pOid.Value, valor);
                    inicio = inicio + objEst.pLongitud.Value;
                }
                return asignacion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        //EN ESTA RUTINA SE EVALUA EL CAMPO PARA HACERLE SUMATORIA SI EN UN CAMPO QUE SE DEBE SUMAR, SI ES UNA CAMPO QUE REQUIERE
        //TRANSFORMACION OBTIENE EL CODIGO O SI ES UN CAMPO CONTADOR OBTIENE LA SUMATORIA DEL CAMPO ASIGNADO.
        private String evaluarCampo(Bancos.EN.Tablas.EstructuraArchivo objEA, String valor)
        {

            //CUANDO EL OBJETO REQUIERE TRANSFORMACION BUSCA UN CODIGO EN UNA TABLA PRECARGADA
            if (objEA.pRequiereCambio == true && tablaE.Rows.Count > 0)
            {
                return obtenerEquivalencia(objEA, Convert.ToString(valor), true);//RETORNA
            }
            if (objEA.pRequiereCambio == true && valor.Contains("0000/00/00")) // SAU FECHA DE CODENSA
            {
                valor = DateTime.Now.ToString(objEA.pFormatoFecha);
            }
            return valor;
        }

        //EN ESTA RUTINA BUSCA LA EQUIVALENCIA DEL OBJETO objEA. SI TIENE UNA EQUIVALENCIA ASIGNADA A UN CAMPO DIRECTAMENTE
        //OBTIENE EL CODIGO DEL CAMPO, PERO SI LA EQUIVALENCIA ESTA ASIGNADA A UNA TABLA ENTONCES DEL OBJETO objEA SACA EL
        //VALOR Y LO COMPARA CON TODOS LOS VALORES DE LOS CAMPOS DE LA TABLA Y CUANDO ENCUENTRE COINCIDENCIA DE ESE CAMPO
        //ENTONCES OBTIENE EL CODIGO DEL CAMPO. SI NO SE CUMPLE NINGUNA DE LAS ANTERIORES RETORNA VACIO.        
        private String obtenerEquivalencia(Bancos.EN.Tablas.EstructuraArchivo objEA, String valor, bool sacarCodigo)
        {

            if (tablaE.Rows.Count == 0 || tablaCE.Rows.Count == 0)
                return String.Empty;

            DataRow[] objE = tablaE.Select("IDEA=" + objEA.pOid.Value);

            if (objE.Length == 0)
                return String.Empty;

            if (objE[0].ItemArray[3].ToString().Length != 0)
            {
                DataRow[] objCE = tablaCE.Select("ID=" + objE[0].ItemArray[3].ToString());
                if (sacarCodigo)
                    return objCE[0].ItemArray[2].ToString();
                else
                    return objCE[0].ItemArray[3].ToString();
            }
            else
            {
                DataRow[] objCE = tablaCE.Select("TABLAS_EQUIVALENCIAS=" + objE[0].ItemArray[2].ToString());

                foreach (DataRow CE in objCE)
                {
                    if (CE.ItemArray[4].ToString().Equals(valor) || (CE.ItemArray[4].ToString().Equals(objEA.pValor) && (objEA.pValorPorDefecto == true)))
                    {
                        if (sacarCodigo)
                            return CE.ItemArray[2].ToString();
                        else
                            return CE.ItemArray[3].ToString();
                    }
                }
            }
            return String.Empty;

        }

        //SE RELLENAN LOS CAMPOS DEL ARCHIVO ASOBANCARIA CON CEROS O ESPACIOS SEGUN EL PARAMETRO 
        private String rellenarCampo(String campo, String alineacion, int longitud, char caracterRelleno)
        {
            if (campo.Trim() == "R04088")
                campo = campo;
            // campo = campo.Trim();
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

        // SE CARGA LOS PARAMETROS DE LA ESTRUCTURA DE LA LINEAS ASOBANCARIA RECAUDO PARA GENERAR EL ARCHIVO
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

        //LA ESTRUCTURA DEL BANCO ESTA CREADA PARA QUE TENGA SOLO LINEA DE DETALLES
        public List<Bancos.EN.Tablas.EstructuraArchivo> consultarEstructuraArchivoBanco(String codBanco, String tipLinea)
        {
            List<Bancos.EN.Tablas.EstructuraArchivo> lista = new List<Bancos.EN.Tablas.EstructuraArchivo>();
            DataTable tabla = new EstructuraArchivoLN().consultarEstructuraBanco(codBanco, tipLinea, TipoProcesoXCuenta);
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

        public List<Bancos.EN.Tablas.InterpreteArchivo> obtenerTransformacionArchivo(String IdCuentaBancoEpicor)
        {

            List<Bancos.EN.Tablas.InterpreteArchivo> datos = new InterpreteArchivoLN().consultar(IdCuentaBancoEpicor, "ABT1"); /// TipoProcesoXCuenta);
            return datos;

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

                        //campo = "0";
                    }
                    break;
                case "HH":
                    campo = writeMilitaryTime(DateTime.Now);
                    break;
            }
            return campo;
        }

        private String armarCampoBanco(Bancos.EN.Tablas.EstructuraArchivo objBan, String valor)
        {
            String campo = String.Empty;
            switch (objBan.pTipoDato)
            {
                case "AN":
                    campo = valor.Trim(); //AQUI valor.TrimStart('0');
                    break;
                case "DE":
                    campo = numeroTransformado(valor, objBan.pCantidadDecimales.Value);
                    break;
                case "FE":
                    if ((!String.IsNullOrEmpty(valor)) && (valor.Length > 10))
                        valor = valor.Substring(0, 10);
                    if ((!String.IsNullOrEmpty(valor)) && (valor.Length > 5 & valor.Length < 11))
                    {
                        campo = fechaTransformado(valor, objBan.pFormatoFecha);
                    }
                    else
                    {
                        campo = "0";
                    }
                    break;
                case "HH":
                    campo = writeMilitaryTime(DateTime.Now);
                    break;
            }
            return campo;
        }

        //EJ: 
        //numero=1000 decimales=2 resul=10,0
        //numero=1000,20 decimales=2 resul=10,00
        //numero=1030,20 decimales=2 resul=10,30
        //FUNCIONA SOLO PARA NUMEROS SIN COMAS NI PUNTOS, CUANDO LLEVA UNA COMA DESCARTA LOS VALORES
        //DESPUES DE LA COMA.
        private String numeroTransformado(string numero, int decimales)
        {

            int divisor = Convert.ToInt32(Math.Pow(10, decimales));
            decimal valor = Convert.ToDecimal((numero)) / divisor;
            return Convert.ToString(decimal.Round(valor, 2));

        }

        //EJ: 
        //fecha='2014-03-06' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //fecha='2014/03/06' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //fecha='20140306' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //fecha='2014 03 06' formato='yyyy/MM/dd' resul='06/03/2014 12:00:00 a.m.'
        //FUNCIONA SOLO PARA FECHAS QUE NO CONTENGAN MAS DE 10 CARACTERES, POR LO TANTO DEBEN
        //VENIR SIN NUMERO
        private String fechaTransformado(String fecha, String formato)
        {

            int x = 0, dia = 0, mes = 0, ano = 0;
            DateTime fecha1 = DateTime.Now;
            string[] format = formato.Split('/');

            for (int i = 0; i < format.Length; i++)
            {
                if ((fecha.Length == 6) & (format[i].ToString().Equals("yyyy")))
                    format[i] = "yy";
            }

            if (fecha.Length == 10)
                x = 1;

            string[] f = new string[3];
            if (fecha.Length > 10)
                fecha = fecha.Substring(0, 10);
            if (fecha.Contains("/"))
            {
                f = fecha.Split('/');
            }
            else
            {
                f[0] = fecha.Substring(0, format[0].Length);
                f[1] = fecha.Substring(format[0].Length + x, format[1].Length);
                f[2] = fecha.Substring(format[0].Length + format[1].Length + 2 * x, format[2].Length);
            }
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i].ToString().Equals("yyyy") || format[i].ToString().Equals("yy"))
                    ano = Convert.ToInt32(f[i].ToString());
                if (format[i].ToString().Equals("yy"))
                    ano = ano + 2000;
                else if (format[i].ToString().Equals("MM"))
                    mes = Convert.ToInt32(f[i]);
                else if (format[i].ToString().Equals("dd"))
                    dia = Convert.ToInt32(f[i]);
            }

            fecha1 = new DateTime(ano, mes, dia, 0, 0, 0);

            return fecha1.ToString();

        }

        //SE CONVIERTE UNA HORA NORMAL A HORA MILITAR
        private String writeMilitaryTime(DateTime date)
        {
            string value = date.ToString("HHmm");
            return value;
        }

        private String convertirNumero(String numero, int decimales)
        {

            String[] numeros = numero.Split(',');
            if (!numero.Contains(","))
                return (Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales)).ToString();
            else
                return Convert.ToInt64((Convert.ToInt64(numero.Replace(",", "")) * Math.Pow(10, decimales - numeros[1].Length))).ToString();

        }

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

        private String SumatoriaCampos(ArrayList campos)
        {
            double suma = 0;

            foreach (string field in campos)
            {
                suma = suma + Convert.ToDouble(field);
            }

            return Convert.ToString(suma);
        }

    }
}