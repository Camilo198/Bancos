using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RutasFtp.PS.Codigo;
using RutasFtp.EN;
using RutasFtp.EN.Tablas;
using RutasFtp.LN.Consultas;
using RutasFtp.EN.Definicion;
using DidiSoft.Pgp;



namespace RutasFtp.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "moverArchivosFTP" en el código, en svc y en el archivo de configuración a la vez.
    public class moverArchivosFTP : ImoverArchivosFTP
    {
        String ConexionError = String.Empty;
        public String trasladarArchivosFTP()
        {
            try
            {
                Ftp objF = new Ftp();
                FtpLN objFtpLN = new FtpLN();
                List<Ftp> listaF = objFtpLN.consultar(objF);
                if (listaF.Count > 0)
                {
                    foreach (Ftp objFtp in listaF)
                    {
                        int valor = 0;
                        try
                        {
                            ArrayList archivos = new ArrayList();
                            //  archivos = ConectorFTP.listarArchivos(objFtp.pUrlFtp, objFtp.pUsuarioFtp, objFtp.pClaveFtp, objFtp.pFtpSeguro, Convert.ToDateTime(objFtp.pFechaUltimoIngreso), objFtp.pFormato, objFtp.pPrefijo);

                            foreach (string x in archivos)
                            {
                                ConectorFTP.moverArchivos(objFtp.pUrlFtp + x.Substring(39), objFtp.pUsuarioFtp, objFtp.pClaveFtp,
                                                          objFtp.pRutaDestino + x.Substring(39), objFtp.pFtpSeguro.Value);
                            }

                            objFtp.pFechaUltimoIngreso = Convert.ToString(DateTime.Now);
                            valor = objFtpLN.actualizarFecha(objFtp);

                            if (valor == 0)
                                throw new Exception("Ocurrio un error a actualizar fecha de ultimo ingreso. Reemplazela para evitar archivos duplicados");

                        }
                        catch (Exception ex)
                        {
                            ConexionError = ConexionError + "Error en ftp : " + objFtp.pUrlFtp + " (" + ex.Message + "),";
                        }
                    }

                    if (String.IsNullOrEmpty(ConexionError))
                        return "Se han movido todos los archivos";
                    else
                        return "Se han movido algunos archivos y ocurrio un " + ConexionError;
                }
                else
                {
                    return "No se encontraron registros de configuración FTP";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public String trasladarArchivos()
        {
            try
            {
                Ftp objF = new Ftp();
                FtpLN objFtpLN = new FtpLN();
                List<Ftp> listaF = objFtpLN.consultar(objF);

                if (listaF.Count > 0)
                {
                    foreach (Ftp objFtp in listaF)
                    {
                        int valor = 0;
                        string FechaUltimaCopia = "";
                        try
                        {
                            ArrayList archivos = new ArrayList();
                            FechaUltimaCopia = objFtp.pFechaUltimaCopia;
               
                            archivos = ConectorFTP.listarArchivosIn(objFtp.pUrlFtp, Convert.ToDateTime(objFtp.pFechaUltimoIngreso), Convert.ToDateTime(objFtp.pFechaUltimaCopia), objFtp.pFormato, objFtp.pPrefijo, objFtp.pRutaDestino);

                            archivos.Sort();
                            List<DateTime> results = archivos.Cast<DateTime>().ToList();

                            objFtp.pFechaUltimoIngreso = Convert.ToString(DateTime.Now);
                            //Ordena Descendentemente la ultima fecha del archivo
                            if (results.Count > 0)
                            {
                                results = results.OrderByDescending(q => q).ToList();
                                objFtp.pFechaUltimaCopia = Convert.ToString(results[0]);
                            }
                            else
                            {
                                objFtp.pFechaUltimaCopia = FechaUltimaCopia;
                            }

                            valor = objFtpLN.actualizarFechaUltimaCopia(objFtp);

                            if (valor == 0)
                                throw new Exception("Ocurrio un error a actualizar fecha de ultimo ingreso. Reemplazela para evitar archivos duplicados");

                        }
                        catch (Exception ex)
                        {
                            ConexionError = ConexionError + "Error en ftp : " + objFtp.pUrlFtp + " (" + ex.Message + "),";
                        }
                    }

                    if (String.IsNullOrEmpty(ConexionError))
                        return "Se han movido todos los archivos";
                    else
                        return "Se han movido algunos archivos y ocurrio un " + ConexionError;
                }
                else
                {
                    return "No se encontraron registros de configuración ";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private void DesencripcionPGP()
        {
            throw new NotImplementedException();
        }

    }

}
