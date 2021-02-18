using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Procesos.EN.Tablas;


using DidiSoft.Pgp;
using Procesos.PS.Codigo;
using System.IO;
using System.Data;
using Procesos.LN.Consultas;


namespace Procesos.PS.Procesos
{
    public class Recaudo
    {
        String mensaje;
        String mensajePagos;

        string[] CorreoControlB;


        /*
         SE AGREGAN PARA ARCHIVOS GRANDES,CIFRADO y PAGOS FUERA DEL PERIODO
         */

        RecaudoLN objRecaudo = new RecaudoLN();
        private wsenviocorreos.Service EnvioCorreo = new wsenviocorreos.Service();
        private DataRow rowC;
        bool flagCorreo = false;
        int tiempoEspera = 0;
        /*Descifrado*/
        TransferenciaArchivos objTransferencia = new TransferenciaArchivos();
        TransferenciaArchivosLN objTranferenciaLN = new TransferenciaArchivosLN();

        CorreoLN objCorreoLN = new CorreoLN();

        public List<String> obtenerBancosRecaudoDiario(ref bool procesoConError, List<ArchivoEN> archivosOrdenados)
        {

            #region "Atributos Bancos"
            String NombreCuenta;
            String IdCuentaBanco;
            String IdCuentaBancoEpicor;
            String RutaEntrada;
            String RutaProcesado;
            String RutaSalidaEpicor;
            String TipoProceso;
            ArrayList CorreosControl = new ArrayList();
            String CodigoBanco;
            String Remitente;
            String TipoCuenta;
            String NumCuenta;
            String Asobancaria;
            #endregion

            List<String> RespuestaProceso = new List<String>();
            List<string[,]> resDis;

            //BANFAS2 duvan ramirez 

            string fechaActual = null, resp;
            //List<Ftp_control> res;
            Correo objCorreo = new Correo();
            Correo objCorreoRes = new Correo();

            try
            {
                //OBTIENE UNA LISTA DE OBJETOS DE BANCOS           
                List<Banco> lista = new List<Banco>();
                EN.Tablas.Banco objB = new EN.Tablas.Banco();
                objB.pActivo = true;
                objB.pTipoProceso = "ABR_";
                BancoLN objBancoLN = new BancoLN();
                lista = objBancoLN.consultar(objB);
                //OBTIENE LAS RUTAS DE LOS BANCOS
                objCorreo = ObtenerCorreos(); //DESCOMENTAREAR A PRD
                objCorreoRes = objCorreo;
                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                //RECORREN TODOS LOS BANCOS Y LOS ENVIA UNO A UNO AL SERVICIO WEB Bancos
                foreach (Banco bank in lista)
                {
                    TipoProceso = bank.pTipoProceso;
                    IdCuentaBanco = Convert.ToString(bank.pId.Value);
                    NombreCuenta = bank.pNombreCuenta;
                    IdCuentaBancoEpicor = bank.pIdCuentaBanco;
                    CorreoControlB = bank.pCorreoControl.Split(';');

                    if (IdCuentaBanco == "61")
                    {
                        CodigoBanco = bank.pCodigoBanco;
                    }


                    foreach (string cc in CorreoControlB)
                    {
                        if (string.IsNullOrEmpty(cc)) break;
                        CorreosControl.Add(cc);
                    }

                    CodigoBanco = bank.pCodigoBanco;
                    Remitente = bank.pRemitente;
                    objRuta.pOid = bank.pRutaArchivosEntrada;
                    RutaEntrada = objRutaLN.consultar(objRuta)[0].pRuta;
                    objRuta.pOid = bank.pRutaArchivosSalida;
                    RutaProcesado = objRutaLN.consultar(objRuta)[0].pRuta;
                    objRuta.pOid = bank.pRutaArchivosSalidaEpicor;
                    RutaSalidaEpicor = objRutaLN.consultar(objRuta)[0].pRuta;
                    TipoCuenta = bank.pTipoCuenta;
                    NumCuenta = bank.pNumCuenta;
                    Asobancaria = bank.pAsobancaria;


                    #region DESENCRIPCION ARCHIVO

                    LectorArchivos objLector = new LectorArchivos();
                    List<String> listaArchivos = objLector.listarDirectorioPGP(RutaEntrada);
                    if (listaArchivos.Count > 0)
                    {
                        foreach (String archivo in listaArchivos)
                        {
                            //Objeto trasnferencia GMF 
                            try
                            {//Solicitar cifrado enviando ruta

                                string inputFileLocation = @archivo;
                                string respActualiza = "";



                                bool esperar = true, actualiza = true;

                                objTransferencia.id = "6";
                                objTransferencia.rutaRepositorio = RutaEntrada;
                                objTransferencia.nomArchivo = inputFileLocation.Substring(RutaEntrada.Length, inputFileLocation.Length - RutaEntrada.Length);
                                objTransferencia.nomArchivo = objTransferencia.nomArchivo;
                                if (File.Exists(RutaEntrada + objTransferencia.nomArchivo))
                                {
                                    objTransferencia.fechaCreacion = File.GetLastWriteTime(RutaEntrada + objTransferencia.nomArchivo);


                                }

                                do
                                {
                                    if (actualiza)
                                    {
                                        respActualiza = objTranferenciaLN.ActualizaDescifrado(objTransferencia);

                                        if (!respActualiza.Equals("1"))
                                        {
                                            EnvioMail("", "Error en la actualizacion de descifrado recaudo", "error actualizando rutas para el decifrado del archivo" + inputFileLocation, objCorreo.mailTo, objCorreo.mailFrom, objCorreo.mailCC);
                                            esperar = false;
                                        }
                                        actualiza = false;

                                    }
                                    resp = objTranferenciaLN.consultaDisponibilidadDescifrado(objTransferencia);

                                    if (resp.Equals("False"))
                                    {
                                        esperar = false;
                                    }


                                } while (esperar);
                                modificarFechaCreacion(objTransferencia);
                            }
                            catch (Exception ex)
                            {
                                //RespuestaProceso.Add(bank.pNombreCuenta + ": " + ex.Message);
                                procesoConError = true;
                                goto line;
                            }
                        }
                    }

                    #endregion

                    //Marina RT  Se valida si la estructura del archivo origen no es asobancaria para correr el proceso de conversion de estructura
                    //LLenar tabla temporal con la rutaEntrada y las fechas de los archivos que tiene adentro 
        
                    


             

                    if (Asobancaria == "N")
                    {

                        mensaje = CorrerRecaudoDiario(NombreCuenta, IdCuentaBanco, IdCuentaBancoEpicor, RutaEntrada, RutaProcesado,
                                                    CorreosControl, Remitente, CodigoBanco, NumCuenta,
                                                    TipoCuenta, RutaSalidaEpicor, TipoProceso);
                        RutaEntrada = RutaProcesado;
                        if (!mensaje.Equals("Proceso Recaudo Diario ejecutado con exito!!"))
                        {
                            procesoConError = true;
                        }
                    }
                    else
                    {
                        mensaje = RutaEntrada + ": Archivo original es Asobancaria";
                        procesoConError = true;
                    }


                    // AQUI ..Marina RT se adiciona línea que invoca el servicio web que aplica pagos de primeras inversiones y genera plano para SICO


                    if (procesoConError)
                    {
                        if (archivosOrdenados.Count > 0)
                        {
                            ArchivoEN topOne = archivosOrdenados.ElementAt(0);
                            ServicioBancos.WsBancos ProcesoPagos = new ServicioBancos.WsBancos();
                            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(RutaEntrada);

                            System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");

                            foreach (System.IO.FileInfo archivos in fileNames)
                            {
                                if (topOne.RutaArchivo == RutaEntrada + archivos.Name)
                                {
                                    ProcesoPagos.Timeout = 300000;
                                    mensaje = ProcesoPagos.LecturaPagos("usuario", "Pasword", RutaEntrada, archivos.Name, "N");

                                    //RespuestaProceso.Add(bank.pNombreCuenta + ": " + mensaje);
                                    if (mensaje == "PROCESO REALIZADO CON EXITO")
                                    {
                                        ArchivoLN archivoLN = new ArchivoLN();
                                        ArchivoEN archivoEN = new ArchivoEN();
                                        archivoEN.Fecha = System.DateTime.Now;
                                        archivoEN.RutaArchivo = topOne.RutaArchivo;
                                        archivoLN.eliminarArchivoBolsaLN(archivoEN, "D");
                                    }
                                }

                            }
                        }
                    }

                }

            line:

                CorreosControl.Clear();


                return RespuestaProceso;



            }
            catch (Exception ex)
            {
                //RespuestaProceso.Add(ex.Message);
                return RespuestaProceso;
            }
        }

        public String CorrerRecaudoDiario(String NombreCuenta, String IdCuentaBanco, String IdCuentaBancoEpicor, String RutaEntrada,
                                              String RutaSalidaEpicor, ArrayList CorreosControl, String Remitente,
                                              String CodigoBanco, String NumCuenta, String TipoCuenta, String RutaProcesado, String TipoProceso)
        {
            String mens;
            try
            {
                ServicioRecaudo.Recaudo procesoRD = new ServicioRecaudo.Recaudo();
                mens = procesoRD.ServicioRecaudoDiario(NombreCuenta, IdCuentaBanco, IdCuentaBancoEpicor, RutaEntrada, RutaSalidaEpicor,
                                                        (String[])CorreosControl.ToArray(typeof(String)),
                                                        Remitente, CodigoBanco, NumCuenta, TipoCuenta,
                                                        "TAREA PROGRAMADA", RutaProcesado, TipoProceso);
                return mens;
            }
            catch
            {
                return "Error en el servicio";
            }
        }


        /// <summary>
        /// Prueba desencripción con GNUPG
        /// </summary>
        /// <param name="encryptedFile"></param>
        /// <returns></returns>
        public string DecryptFile(FileInfo encryptedFile, string ArchivoSalida)
        {
            // decrypts the file using GnuPG, saves it to the file system
            // and returns the new (decrypted) file name.

            // encrypted file: thefile.xml.gpg decrypted: thefile.xml

            string outputFileName = encryptedFile.Name; // this.GetDecryptedFileName(encryptedFile);
            // whatever you want here - just a convention

            string path = encryptedFile.DirectoryName;
            string outputFileNameFullPath = path + "\\" + outputFileName;
            try
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.WorkingDirectory = @"C:\Program Files (x86)\GNU\GnuPG\pub\"; //C:\\Program Files\\GnuPP\\GnuPG";

                System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
                // actually NEED to set this as a local string variable
                // and pass it - bombs otherwise!
                // string sCommandLine = "type " + "d:\\GNUPG\\passphrase.txt"  + "| gpg --passphrase-fd 0 --batch --decrypt-files " +  "\""+ encryptedFile.FullName + "\"";

                string sCommandLine = "type " + "C:\\GNUPG\\passphrase.txt" + "| gpg.exe --passphrase-fd 0 --batch  --decrypt-files \"" + encryptedFile.FullName + "\"";
                // string sCommandLine  = "type d:\\GNUPG\\passphrase.txt | gpg --passphrase-fd 0 --batch --decrypt-files D:\\Prueba\\PAT4500557.txt.pgp";
                process.StandardInput.WriteLine(sCommandLine);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                return outputFileName;
            }
            return outputFileName;
        }

        private void EnvioMail(string _Adjunto, string _Asunto, string _Mensaje, string _Para, string _Desde, string _Copia)
        {
            try
            {
                DataSet DsCorreos = new DataSet();
                DsCorreos.Tables.Add("Reportes");
                DsCorreos.Tables["Reportes"].Columns.Add("strTo");
                DsCorreos.Tables["Reportes"].Columns.Add("strCc");
                DsCorreos.Tables["Reportes"].Columns.Add("strCo");
                DsCorreos.Tables["Reportes"].Columns.Add("strSubject");
                DsCorreos.Tables["Reportes"].Columns.Add("strMessaje");
                DsCorreos.Tables["Reportes"].Columns.Add("strPath");

                rowC = DsCorreos.Tables["Reportes"].NewRow();
                rowC["strTo"] = _Para;
                rowC["strCo"] = _Copia;
                rowC["strSubject"] = _Asunto;
                rowC["strMessaje"] = _Mensaje;
                rowC["strPath"] = _Adjunto.Trim();
                DsCorreos.Tables["Reportes"].Rows.Add(rowC);
                EnvioCorreo.EnvioCorreos(DsCorreos, _Desde, false);
            }
            catch (Exception)
            {

            }
        }


        public void modificarFechaCreacion(TransferenciaArchivos objAuxArchivo)
        {
            string nombresinExt = objAuxArchivo.nomArchivo.Substring(0, objAuxArchivo.nomArchivo.Length - 4);
            string path = objAuxArchivo.rutaRepositorio + nombresinExt;


            if (File.Exists(path))
                File.SetLastWriteTime(path, objAuxArchivo.fechaCreacion);
        }

        private static Correo ObtenerCorreos()
        {
            Correo objCorreo = new Correo();
            CorreoLN objCorreoLN = new CorreoLN();

            objCorreo.id = 2;
            List<Correo> ListCorreo = objCorreoLN.Consulta(objCorreo);
            if (ListCorreo.Count > 0)
            {
                objCorreo = ListCorreo[0];
            }
            return objCorreo;
        }




    }
}
