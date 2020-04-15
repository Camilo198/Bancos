using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


namespace Bancos.PS.Servicios.Ftp
{
    public class ConectorFTP
    {
        //ENVIA LOS ARCHIVOS POR FTP        
        public void enviarArchivoXFtp(String ArchivoSalidaAsobancaria, String UrlFTP, String UsuarioFTP, String ClaveFTP, String nombreArchivo)
        {
            try
            {

                FtpWebRequest dirFtp = ((FtpWebRequest)FtpWebRequest.Create("ftp://" + UrlFTP + "//" + nombreArchivo));
                // Los datos del usuario (credenciales)
                NetworkCredential cr = new NetworkCredential(UsuarioFTP, ClaveFTP);
                dirFtp.Credentials = cr;
                dirFtp.UsePassive = true;
                dirFtp.UseBinary = true;
                dirFtp.KeepAlive = true;
                dirFtp.Method = WebRequestMethods.Ftp.UploadFile;
                FileStream stream = File.OpenRead(ArchivoSalidaAsobancaria + nombreArchivo);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                Stream reqStream = dirFtp.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Flush();
                reqStream.Close();               
            }
            catch
            {
                throw new System.Exception("Ocurrio un error al enviar archivo por FTP");
            }
        }
    }
}
