using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Data;

namespace RutasFtp.PS.Codigo
{
    public class ConectorFTP
    {

        public static void moverArchivos(string url, string usuario, string clave,string destino,bool FtpSeguro)
        {

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpRequest.Credentials = new NetworkCredential(usuario, clave);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            if (FtpSeguro)
            ftpRequest.EnableSsl = true;

            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            Stream stream = null;
            StreamReader reader = null;
            StreamWriter writer = null;

            try
            {

                stream = ftpResponse.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(destino,false);
                writer.Write(reader.ReadToEnd());
                
                //FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(url);
                //requestFileDelete.Credentials = new NetworkCredential(usuario, clave);
                //requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                //FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();

            }
            finally
            {
                stream.Close();
                reader.Close();
                writer.Close();
            }

        }

        public static ArrayList listarArchivos(string url, string usuario, string clave, bool FtpSeguro,DateTime fechaltimoIngreso,String Formato,String Prefijo)
        {
            FtpWebRequest dirFtp = ((FtpWebRequest)FtpWebRequest.Create(url));
          
            // Los datos del usuario (credenciales)
            NetworkCredential cr = new NetworkCredential(usuario, clave);
            dirFtp.Credentials = cr;

            // El comando a ejecutar
            dirFtp.Method = "LIST";

            // También usando la enumeración de WebRequestMethods.Ftp
            dirFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            if (FtpSeguro)
            dirFtp.EnableSsl = true;
            // Obtener el resultado del comando
            StreamReader reader = new StreamReader(dirFtp.GetResponse().GetResponseStream());

            ArrayList lista= new ArrayList();
            string res = string.Empty;
            do
            {
                DateTime fechaArchivo;               
                res = reader.ReadLine();
               
                if(res != null)
                {
                    fechaArchivo = new DateTime(Convert.ToInt32(res.Substring(6, 2)) + 2000,
                                                   Convert.ToInt32(res.Substring(0, 2)),
                                                   Convert.ToInt32(res.Substring(3, 2)),
                                                   Convert.ToInt32(res.Substring(10, 2)),
                                                   Convert.ToInt32(res.Substring(13, 2)), 0);

                    if ((fechaArchivo > fechaltimoIngreso) && res.Contains(Formato) && res.Contains(Prefijo))
                    {
                        lista.Add(res);
                    }
                }

            } while(res != null);
            
            // Cerrar el stream abierto.
            reader.Close();

            return lista;
        }



        public static void moverArchivosIn(string url, string usuario, string clave, string destino, bool FtpSeguro)
        {

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpRequest.Credentials = new NetworkCredential(usuario, clave);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            if (FtpSeguro)
                ftpRequest.EnableSsl = true;

            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            Stream stream = null;
            StreamReader reader = null;
            StreamWriter writer = null;

            try
            {

                stream = ftpResponse.GetResponseStream();
                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(destino, false);
                writer.Write(reader.ReadToEnd());

                //FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(url);
                //requestFileDelete.Credentials = new NetworkCredential(usuario, clave);
                //requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                //FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();

            }
            finally
            {
                stream.Close();
                reader.Close();
                writer.Close();
            }

        }

        public static ArrayList listarArchivosIn(string origen, DateTime fechaUltimoIngreso,DateTime fechaUltimaCopia, String Formato, String Prefijo, string destino)
        {
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(origen);
           
                   
            // Get the files in the directory and print out some information about them.
            System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");
             ArrayList lista = new ArrayList();
           
            foreach (System.IO.FileInfo fi in fileNames)
            {
                string ultimoingreso = fi.LastAccessTime.ToString();
                string ultimoescritura = fi.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");

                if (Convert.ToDateTime(ultimoescritura) > fechaUltimaCopia && fi.Name.Contains(Formato) && fi.Name.Contains(Prefijo))
                {
                    //lista.Add(fi.Name);
                    lista.Add(fi.LastWriteTime);
                    fi.CopyTo(destino+fi.Name,true);
                    //fi.CopyTo(destino + fi.Name.Substring(0,3) + DateTime.Now.Month.ToString()+DateTime.Now.Day.ToString()+".TXT", true);
                }
            }
            return lista;
        }


    }
}