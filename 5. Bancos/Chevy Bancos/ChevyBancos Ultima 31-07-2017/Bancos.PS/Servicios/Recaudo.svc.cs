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
using System.Data.OleDb;

using System.Text.RegularExpressions;
using SpreadsheetLight;
using System.Globalization;

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Recaudo" en el código, en svc y en el archivo de configuración a la vez.
    public class Recaudo : IRecaudo
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
        private List<Bancos.EN.Tablas.EstructuraArchivo> ListaEstructuraArchivoBancoDetalle { get; set; }//LISTA CON LA ESTRUCTURA DE UN BANCO (DETALLE)
        private List<Bancos.EN.Tablas.InterpreteArchivo> ListaEquivalenciaArchivo { get; set; }//OBJETO CON LAS EQUIVALENCIAS ENTRE ESTRUCTURA BANCOS Y ESTRUCTURA ASOBANCARIA RECAUDO

        DataTable tablaCE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA DE LOS CAMPOS_EQUIVALENCIAS
        DataTable tablaE = new DataTable(); //EN ESTA DATATABLE SE CARGA LA TABLA EQUIVALENCIAS 

        String LineaArmada1EA = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO ARCHIVO
        String LineaArmada2EL = String.Empty; // CONTIENE LA LINEA ARMADA DEL ENCABEZADO LOTE
        String LineaArmada3DT = String.Empty; // CONTIENE LA LINEA ARMADA DEL DETALLE Y VA VARIANDO EN CADA CICLO 
        String LineaArmada4CL = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE LOTE
        String LineaArmada5CA = String.Empty; // CONTIENE LA LINEA ARMADA DEL CONTROL DE ARCHIVO
        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO ASOBANCARIA
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
        bool EsExcel = false;
        string CaracterDecimal = "";

        #region TABLA CON LINEAS PARA GUARDAR EN LA BD
        DataTable LineasArmadasdt = new DataTable();
        #endregion

        public String ServicioRecaudoDiario(String NombreCuenta, String IdCuentaBanco, String IdCuentaBancoEpicor, String RutaEntrada,
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
            tipoArchivo = "Recaudo Diario";
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
            ListaLinea3DT.AddRange(consultarEstructuraAsobancaria("3DT", TipoProcesoXCuenta));
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
            String archivo = "";
            int errores = 0;
            string err_log = "";
            try
            {
                LectorArchivos objLector = new LectorArchivos();
                List<String> listaArchivos = objLector.listarDirectorio(RutaEntrada);

                //AQUI MRT VALIDAR CODIGO SERVICIO EPICSAT
                //ServicioContrato.ChevyplanWS validar = new ServicioContrato.ChevyplanWS();
                //validar.Timeout = -1;

                foreach (String archivo_ax in listaArchivos)
                {
                    try
                    {
                        archivo = archivo_ax;
                        string nombreOriginal = System.IO.Path.GetFileNameWithoutExtension(archivo);
                        List<String> lineasArchivo = new List<string>();

                        #region LECTURA ARCHIVO EXCEL
                        System.Data.DataSet DsExcel = new System.Data.DataSet();
                        if (EsExcel)
                        {
                            string path = System.IO.Path.GetFullPath(archivo);
                            System.Data.DataTable DtExcel = new System.Data.DataTable();
                            System.Data.DataRow DrExcel = null;

                            //OleDbConnection oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
                            try
                            {
                                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                sl = new SLDocument(fs);

                                String[] hojas = sl.GetSheetNames().ToArray();

                                sl.SelectWorksheet(hojas[NumeroHoja]);

                                SLWorksheetStatistics stats = sl.GetWorksheetStatistics();

                                int ultimaCol = stats.EndColumnIndex;
                                int ultimaFil = stats.EndRowIndex;
                                int columnaInicio = stats.StartColumnIndex;
                                int filaInicio = stats.StartRowIndex;

                                for (int h = 1; h <= stats.EndColumnIndex; h++)
                                {
                                    DtExcel.Columns.Add(h.ToString());
                                }

                                string val = String.Empty;
                                int pivote = 0;
                                bool esEntero = false;

                                for (int i = filaInicio; i <= ultimaFil; i++)
                                {
                                    DrExcel = DtExcel.NewRow();
                                    for (int j = columnaInicio; j < ultimaCol; j++)
                                    {
                                        //tomamos los valores de las celdas para el datatable
                                        if (sl.GetCellValueAsString(i, j) != null)
                                        {
                                            var formato = sl.GetCellStyle(i, j).FormatCode;
                                            val = sl.GetCellValueAsString(i, j);

                                            esEntero = int.TryParse(val, out pivote);
                                            if ((formato.Contains("m/d/yyyy") || formato.Contains("d/mm/yyyy")) && esEntero)
                                            {
                                                val = sl.GetCellValueAsDateTime(i, j).ToString("d", new CultureInfo("es-CO"));
                                            }
                                            else
                                            {
                                                val = sl.GetCellValueAsString(i, j);
                                            }
                                            DrExcel[j.ToString()] = val;
                                            esEntero = false;
                                            pivote = 0;
                                            val = String.Empty;
                                        }

                                    }
                                    DtExcel.Rows.Add(DrExcel);
                                }
                                DsExcel.Tables.Add(DtExcel);
                                fs.Close();
                                sl.Dispose();
                                //oledbConn.Open();

                                //OleDbCommand cmd = new OleDbCommand(); ;

                                //OleDbDataAdapter oleda = new OleDbDataAdapter();

                                //DataTable dt = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                                //string sheetName = string.Empty;

                                //if (dt != null)
                                //{

                                //    sheetName = dt.Rows[NumeroHoja]["TABLE_NAME"].ToString();

                                //}

                                //cmd.Connection = oledbConn;

                                //cmd.CommandType = CommandType.Text;

                                //cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                                //oleda = new OleDbDataAdapter(cmd);

                                //oleda.Fill(DsExcel, "excelData");

                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Error leyendo excel: " + tipoArchivo + " " + archivo + " " + ex.ToString());
                                //Correo.enviarNotificacionesError(NombreCuenta, (String[])CorreosControl.ToArray(typeof(String)), Remitente, ex.Message, tipoArchivo + " " + archivo);
                                //return ex.Message;
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
                                    lineaDatos.Add(row[i].ToString().Replace(@"$", "").Replace(@"-", "")); // aqui Replace(@".", "").Replace(@",", "").
                                }

                                if (lineaExcel != null && lineaExcel != String.Empty)
                                {
                                    if (ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0 && lineasArchivo.Count == 0)
                                        lineaExcel = armarLineasExcel(lineaDatos, ListaEstructuraArchivoBancoEncabezadoArchivo);
                                    else if ((ListaEstructuraArchivoBancoEncabezadoLote.Count > 0 && lineasArchivo.Count == 0) ||
                                    (ListaEstructuraArchivoBancoEncabezadoLote.Count > 0 && lineasArchivo.Count == 1 && ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0))
                                        lineaExcel = armarLineasExcel(lineaDatos, ListaEstructuraArchivoBancoEncabezadoLote);
                                    else
                                        lineaExcel = armarLineasExcel(lineaDatos, ListaEstructuraArchivoBancoDetalle);

                                    lineasArchivo.Add(lineaExcel);
                                }
                            }
                        }

                        #endregion
                        #region LECTURA ARCHIVO PLANO
                        else
                        {
                            lineasArchivo = objLector.leerArchivo(archivo);
                        }
                        #endregion

                        DataSet ds1 = new DataSet(); // DATASET CON LOS PAGOS                 
                        DataTable dt1 = new DataTable();
                        dt1.Columns.Add("fecha");
                        dt1.Columns.Add("linea");

                        //**************************************************************************
                        //**************************************************************************
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

                            //if (NombreCuenta != "19")
                            //{
                            //    if (ListaEstructuraArchivoBancoEncabezadoArchivo.Count > 0)
                            //        lineasArchivo.RemoveAt(0);
                            //    if (ListaEstructuraArchivoBancoEncabezadoLote.Count > 0)
                            //        lineasArchivo.RemoveAt(0);
                            //    if (ListaEstructuraArchivoBancoControlArchivo.Count > 0)
                            //        lineasArchivo.RemoveAt(lineasArchivo.Count - 2);
                            //    if (ListaEstructuraArchivoBancoControlLote.Count > 0)
                            //        lineasArchivo.RemoveAt(lineasArchivo.Count - 1);

                            //    lineasArchivo.RemoveRange(lineasArchivo.Count - LineaExcluidaFin, LineaExcluidaFin);//ELIMINA TODAS LAS LINEAS DEL FIN QUE SON EXCLUIDAS POR CONFIGURACION    
                            //}
                            if (LineaExcluidaFin.ToString() != "")
                                lineasArchivo.RemoveRange(lineasArchivo.Count - LineaExcluidaFin, LineaExcluidaFin);//ELIMINA TODAS LAS LINEAS DEL FIN QUE SON EXCLUIDAS POR CONFIGURACION    

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

                        //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS DE RECAUDO DIARIO SI NO EXISTE
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
                                    armarArchivo(ds1, limite, IdCuentaBanco, IdCuentaBancoEpicor, CodigoBanco, NumCuenta, tipocuentabanco, nombreOriginal);
                                }
                                else
                                {
                                    limite = (int)LimitesSuperior[i];
                                    armarArchivo(ds1, limite, IdCuentaBanco, IdCuentaBancoEpicor, CodigoBanco, NumCuenta, tipocuentabanco, nombreOriginal);
                                }

                                if (CorreosControl.Count > 0)
                                    Correo.enviarNotificaciones(Directorio, (String[])CorreosControl.ToArray(typeof(String)), nombreArchivo, Remitente,
                                                                registrosLote, tipoArchivo);
                                ciclo = ciclo + limite; //+ Convert.ToInt16(LimitesSuperior[i]);

                                //SE GUARDA EL ESTADO DEL PROCESO
                                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                                objL.pUsuario = Usuario;
                                objL.pDetalle = NombreCuenta + ", " + tipoArchivo + " : Archivo " + tipoArchivo + " con fecha de transaccion " + DateTime.Now.ToString("dd/MM/yyyy") + " fue generado correctamente";
                                objL.pTipoArchivo = TipoProcesoXCuenta;
                                objL.pTipoProceso = "GEN";
                                new LogsLN().insertar(objL);
                                //    nombreArchivo = String.Empty; Se comentarea linea para traer el nombre del archivo
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

                        objLector.moverArchivo(archivo, RutaSalida, tipoArchivo, nombreArchivo);
                        //File.Move(archivo, RutaSalida + nombre);

                        System.Threading.Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        errores += 1;
                        err_log = err_log + " --- " + ex.ToString();
                    }

                }
                if (errores > 0)
                {
                    throw new Exception("Proceso " + tipoArchivo + "Con errores: " + errores.ToString() + " en " + listaArchivos.Count.ToString() + " archivos procesados " + err_log);
                }
                else
                {
                    return "Proceso " + tipoArchivo + " ejecutado con exito!!";
                }

            }
            catch (Exception ex)
            {
                //if (CorreosControl.Count > 0)
                Correo.enviarNotificacionesError(NombreCuenta, (String[])CorreosControl.ToArray(typeof(String)), Remitente, archivo + " IniExcl " + LineaExcluidaIncio + " FinExcl " + LineaExcluidaFin + " " + ex.ToString(), tipoArchivo);
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = Usuario;
                objL.pDetalle = NombreCuenta + ",  " + tipoArchivo + " : " + ex.Message;
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

        private void armarArchivo(DataSet tabla, long Limite, String IdCuentaBanco, String IdCuentaBancoEpicor, String CodigoBanco,
                                     String NumCuenta, String TipoCuenta, string NombreOriginal)
        {

            try
            {
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
                                              "_", "RD", "_",
                                              Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("ddMMyyyy")),
                                              "_", writeMilitaryTime(DateTime.Now), "_", ConsecutivoArchivo, ".txt");
                //MRT Cambio nombre asignado por el sistema por nombre original
                //  nombreArchivo = NombreOriginal.Trim()+ ".txt";
                //SE CREA ESTE SLEEP PARA QUE NO SOBRESCRIBA EL ARCHIVO Y GENERE LOS N ARCHIVOS POR FECHA
                System.Threading.Thread.Sleep(2000);
                //   nombreArchivo = NombreOriginal.Trim() + DateTime.Now.ToString().Replace("/", "").Replace(".", "").Replace(" ", "").Replace(":", "") + ".txt";
                nombreArchivo = NombreOriginal.Trim() + Convert.ToString(Convert.ToDateTime(FechaTransaccion).ToString("ddMMyyyy")) + ".txt";
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
        private String armarLineasExcel(DataRow lineaDatos, List<Bancos.EN.Tablas.EstructuraArchivo> lineasAsobancaria)
        {
            String linea = String.Empty;
            String valor = String.Empty;
            char caracterRelleno;
            int i = 0;
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
                    valor = lineaDatos[i].ToString();
                    linea += rellenarCampo(valor, objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                }
                i++;
            }
            return linea;
        }

        //SE ARMA CADA LINEA DE ASOBANCARIA SEGUN LOS PARAMETROS DE LA ESTRUCTURA
        private String armarLineasExcel(List<String> lineaDatos, List<Bancos.EN.Tablas.EstructuraArchivo> lineasAsobancaria)
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
                            else if (objAso.pNombreCampo.Equals("Hora de grabación del Archivo"))
                                valor = armarCampo(objAso, String.Empty);
                            else if (objAso.pNombreCampo.Equals("Modificador de archivo"))
                                valor = armarCampo(objAso, campos[4]);
                            else if (objAso.pNombreCampo.Equals("Tipo de Cuenta"))
                                valor = armarCampo(objAso, campos[5]);
                            else if (objAso.pNombreCampo.Equals("Fecha del archivo"))
                                valor = armarCampo(objAso, String.Empty);
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
                    //AQUI VALIDACION DEL MEDIO DE PAGO CON UN IF
                    if (objAso.pNombreCampo.Trim() == "Medios de pago")
                    {
                        if (valor.Trim() == "000000000000" || valor.Trim() == "EFECTIVO" || valor.Trim() == "00.00" || valor.Trim() == "Recaudo en Efectivo")
                        {
                            valor = "01";
                        }
                        else
                        {
                            valor = "02";
                        }

                    }


                    linea += rellenarCampo(valor, objAso.pAlineacion, Convert.ToInt32(objAso.pLongitud), caracterRelleno);
                }
            }
            return linea;
        }

        //SE GUARDA TODAS LAS LINEAS DEL ARCHIVO RECAUDO DIARIO EN LA BASE DE DATOS
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
            asignacion.Add(0, "3DT");
            Int32 inicio = 0;
            string j = "";
            String valor = String.Empty;
            foreach (Bancos.EN.Tablas.EstructuraArchivo objEst in ListaDeEstructuraArchivoBanco)
            {

                if (objEst.pNombreCampo == "Forma de pago")
                    j = String.Empty;

                valor = String.Empty;
                valor = armarCampoBanco(objEst, evaluarCampo(objEst, Linea.Substring(inicio, objEst.pLongitud.Value).Trim()));

                //valor = armarCampoBanco(objEst, evaluarCampo(objEst, columna.Trim()));
                //AQUI MRT PARECE QUE BUSCA EL CONTRATO DEL SSITEMA PORQUE LA REFERENCIA LLEGA CONTRATO FISICO - ERA PARA EPICOR
                //if (objEst.pNombreCampo.Equals("referencia"))
                //{
                //    ServicioContrato.ChevyplanWS BuscarContrato = new ServicioContrato.ChevyplanWS();
                //    valor = BuscarContrato.Recaudo_BuscarContrato(valor.TrimStart('0'));
                //}
                asignacion.Add(objEst.pOid.Value, valor);
                inicio = inicio + objEst.pLongitud.Value;
            }
            return asignacion;
        }

        //EN ESTA RUTINA SE EVALUA EL CAMPO PARA HACERLE SUMATORIA SI EN UN CAMPO QUE SE DEBE SUMAR, SI ES UNA CAMPO QUE REQUIERE
        //TRANSFORMACION OBTIENE EL CODIGO O SI ES UN CAMPO CONTADOR OBTIENE LA SUMATORIA DEL CAMPO ASIGNADO.
        private String evaluarCampo(Bancos.EN.Tablas.EstructuraArchivo objEA, String valor)
        {

            //CUANDO EL OBJETO REQUIERE TRANSFORMACION BUSCA UN CODIGO EN UNA TABLA PRECARGADA
            if (objEA.pRequiereCambio == true)
            {
                return obtenerEquivalencia(objEA, Convert.ToString(valor), true);//RETORNA
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

        public List<Bancos.EN.Tablas.InterpreteArchivo> obtenerTransformacionArchivo(String IdCuentaBancoEpicor)
        {

            List<Bancos.EN.Tablas.InterpreteArchivo> datos = new InterpreteArchivoLN().consultar(IdCuentaBancoEpicor, TipoProcesoXCuenta);
            return datos;

        }

        private String armarCampo(Bancos.EN.Tablas.EstructuraArchivo objAso, String valor)
        {
            string j = "W";
            if (objAso.pNombreCampo == "Forma de Pago" || objAso.pNombreCampo == "Medios de Pago")
                j = "e";

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
            string j = "w";
            int valorNuevo = 0;
            bool valido = false;
            if (objBan.pNombreCampo == "Forma de Pago" || objBan.pNombreCampo == "Medios de Pago")
                j = "w";

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
                    char[] xd = valor.ToCharArray();
                    foreach (char VARIABLE in xd)
                    {
                        if (char.IsLetter(VARIABLE))//verifica si es una letra
                        {
                            campo = Convert.ToString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
                            return campo;
                        }
                    }
                    if ((!String.IsNullOrEmpty(valor)) && (valor.Length > 5 & valor.Length < 11))
                    {
                        campo = fechaTransformado(valor, objBan.pFormatoFecha);
                    }
                    else
                    {
                        campo = Convert.ToString(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0));
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
            int valorNuevo = 0;
            bool valido = false;
            decimal valor = 0;

            int divisor = Convert.ToInt32(Math.Pow(10, decimales));

            valido = int.TryParse(numero, out valorNuevo);
            if (valido)
            {
                valor = Convert.ToDecimal((valorNuevo)) / divisor;
            }
            else
            {
                valor = Convert.ToDecimal(Convert.ToInt64(numero)) / divisor;
            }

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
            DateTime fecha1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
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
                {
                    ano = Convert.ToInt32(f[i].ToString());
                    if (format[i].ToString().Equals("yy"))
                        ano = ano + 2000;
                }
                else if (format[i].ToString().Equals("MM"))
                    mes = Convert.ToInt32(f[i]);
                else if (format[i].ToString().Equals("dd"))
                    dia = Convert.ToInt32(f[i]);
            }

            if (ano > 0 && mes > 0 && dia > 0)
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
            String reemplazado = "";
            long numero_reem = 0;
            long res = 0;
            long dec = 0;
            String[] numeros = numero.Split(',');

            if (!numero.Contains(","))
            {
                reemplazado = numero.Replace(",", "");
                Int64.TryParse(numero, out numero_reem);
                dec = (long)Math.Pow(10, decimales);
                res = numero_reem * dec;

                return res.ToString();
            }
            else
            {
                reemplazado = numero.Replace(",", "");
                Int64.TryParse(numero, out numero_reem);
                dec = (long)Math.Pow(10, decimales - numeros[1].Length);
                res = numero_reem * dec;

                return res.ToString();
            }

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
