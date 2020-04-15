using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Bancos.PS.Servicios.Correo
{
    public class EnviarCorreo
    {
        //ENVIA LOS CORREOS ADJUNTANDO EL ARCHIVO, INFORMACION DEL NOMBRE DEL ARCHIVO Y NUMERO DE LINEAS.
        //ESTA INFORMACION SE ENVIA A LOS CORREOS DEL BANCO
        public void enviarArchivoXCorreoEnvio(String archivoSalida, ArrayList correosEnvio,
                                              String nombreArchivo, String remitente, int lineasArchivo,
                                              String tipoArchivo)
        {
            try
            {
                MailMessage _Correo = new MailMessage();
                _Correo.From = new MailAddress(remitente);
                foreach (String correo in correosEnvio)
                {
                    if (email_valido(correo))
                    _Correo.To.Add(correo);
                }
                if (_Correo.To.Count == 0)
                    return;
                _Correo.Subject = "Archivo " + tipoArchivo;
                _Correo.Priority = MailPriority.High;
                _Correo.Body = "Buenos Días, \n \n" + "Se adjunta el siguiente Archivo " + tipoArchivo  + " : \n \n" +
                               "Nombre del Archivo : " + nombreArchivo + " \n \n" +
                               "Número de Lineas : " + Convert.ToInt32(lineasArchivo);
                _Correo.Attachments.Add(new Attachment(@archivoSalida + nombreArchivo));

                SmtpClient objSmtpClient = new SmtpClient("exchange");
                objSmtpClient.Host = "chevyplan-com-co.mail.protection.outlook.com";
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                objSmtpClient.Send(_Correo);
            }
            catch
            {
                throw new System.Exception("Ocurrio un error al enviar archivo por correo");
            }
        }
        //ENVIA LOS CORREOS ADJUNTANDO EL ARCHIVO, INFORMACION DEL NOMBRE DEL ARCHIVO, NUMERO DE LINEAS Y CORREOS DEL BANCO(DESTINATARIOS)
        //ESTA INFORMACION SE ENVIA A LOS CORREOS DE CONTROL DE CHEVYPLAN
        public void enviarArchivoXCorreoControlC(String archivoSalida, ArrayList correosControl,
                                               ArrayList correosEnvio, String nombreArchivo, String remitente,
                                               int lineasArchivo, String tipoArchivo)
        {
            try
            {
                String Destinatarios = String.Empty;
                MailMessage _Correo = new MailMessage();
                _Correo.From = new MailAddress(remitente);

                foreach (String correo in correosControl)
                {
                    if (email_valido(correo))
                    _Correo.To.Add(correo);
                }
                foreach (String correo in correosEnvio)
                {
                    if (email_valido(correo))
                    Destinatarios = Destinatarios + correo + "\n";
                }
                if (_Correo.To.Count == 0)
                    return;
                _Correo.Subject = "Archivo " + tipoArchivo;
                _Correo.Priority = MailPriority.High;
                _Correo.Body = "Buenos Días, \n \n" + "Se Genero y Envio por Correo el siguiente Archivo " + tipoArchivo + " : \n \n" +
                                   "Nombre del Archivo : " + nombreArchivo + " \n \n" +
                                   "Número de Lineas : " + Convert.ToInt32(lineasArchivo) + " \n \n" +
                                   "Destinatario : \n \n" + Destinatarios;
                _Correo.Attachments.Add(new Attachment(@archivoSalida + nombreArchivo));

                SmtpClient objSmtpClient = new SmtpClient("exchange");
                objSmtpClient.Host = "chevyplan-com-co.mail.protection.outlook.com";
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                objSmtpClient.Send(_Correo);
            }
            catch
            {
                throw new System.Exception("Ocurrio un error al enviar archivo por correo");
            }
        }
        //ENVIA LOS CORREOS ADJUNTANDO EL ARCHIVO, INFORMACION DEL NOMBRE DEL ARCHIVO, NUMERO DE LINEAS, (URL Y USUARIO) FTP
        //ESTA INFORMACION SE ENVIA A LOS CORREOS DE CONTROL DE CHEVYPLAN
        public void enviarArchivoXCorreoControlE(String archivoSalida, ArrayList correosControl, String nombreArchivo,
                                                 String remitente, int lineasArchivo, String urlFTP, String usuarioFTP,
                                                 String tipoArchivo)
        {
            try
            {
                String Destinatarios = String.Empty;
                MailMessage _Correo = new MailMessage();
                _Correo.From = new MailAddress(remitente);

                foreach (String correo in correosControl)
                {
                    if (email_valido(correo))
                    _Correo.To.Add(correo);
                }
                if (_Correo.To.Count == 0)
                    return;
                _Correo.Subject = "Archivo " + tipoArchivo;
                _Correo.Priority = MailPriority.High;
                _Correo.Body = "Buenos Días, \n \n" + "Se Genero y Envio por Ftp el siguiente Archivo " + tipoArchivo + " : \n \n" +
                                   "Nombre del Archivo : " + nombreArchivo + " \n \n" +
                                   "Número de Lineas : " + Convert.ToInt32(lineasArchivo) + " \n \n" +
                                   "Url : " + urlFTP + " \n \n" +
                                   "Usuario : " + usuarioFTP;
                _Correo.Attachments.Add(new Attachment(@archivoSalida + nombreArchivo));

                SmtpClient objSmtpClient = new SmtpClient("exchange");
                objSmtpClient.Host = "chevyplan-com-co.mail.protection.outlook.com";
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                objSmtpClient.Send(_Correo);
            }
            catch
            {
                throw new System.Exception("Ocurrio un error al enviar archivo por correo");
            }
        }
        ////ENVIA LOS CORREOS ADJUNTANDO LOS FALLOS PRESENTADOS A LA HORA DE LLAMAR LOS SERVICIOS
        ////ESTA INFORMACION SE ENVIA A LOS CORREOS DE CONTROL DE CHEVYPLAN
        //public void enviarArchivoXCorreoControlError(String nombreBanco, ArrayList correosControl, String remitente,
        //                                             String error, String tipoArchivo)
        //{
        //    try
        //    {
        //        String Destinatarios = String.Empty;
        //        MailMessage _Correo = new MailMessage();
        //        _Correo.From = new MailAddress(remitente);

        //        foreach (String correo in correosControl)
        //        {
        //            _Correo.To.Add(correo);
        //        }
        //        _Correo.Subject = "Archivo " + tipoArchivo + " - Error en el Proceso";
        //        _Correo.Priority = MailPriority.High;
        //        _Correo.Body = "Buenos Días, \n \n" + "Ocurrio un error en el proceso de generación del archivo " + tipoArchivo + " del Banco: \n \n" +
        //                            nombreBanco.ToUpper() + " \n \n" +
        //                           "Fallo : " + error + " \n \n";
        //        SmtpClient objSmtpClient = new SmtpClient("exchange");
        //        objSmtpClient.Host = "SBOGCHE012F.chevyplan.col";
        //        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

        //        objSmtpClient.Send(_Correo);
        //    }
        //    catch
        //    {
        //        throw new System.Exception("Ocurrio un error al enviar archivo por correo");
        //    }
        //}
        //ENVIA LOS CORREOS ADJUNTANDO INFORMACION DE LA RUTA Y NOMBRE DEL ARCHIVO, NUMERO DE LINEAS.
        //ESTA INFORMACION SE ENVIA A LOS CORREOS DE NOTIFICACION QUE SE ENCUENTRAN EN UN SERVICIO WEB
        public void enviarNotificaciones(String archivoSalida, String[] correosControl, String nombreArchivo, String remitente,
                                               int lineasArchivo, String tipoArchivo)
        {
            try
            {
                String Destinatarios = String.Empty;
                MailMessage _Correo = new MailMessage();
                _Correo.From = new MailAddress(remitente);

                foreach (String correo in correosControl)
                {
                    if (email_valido(correo))
                    _Correo.To.Add(correo);
                }
                if (_Correo.To.Count == 0)
                    return;
                _Correo.Subject = "Archivo " + tipoArchivo;
                _Correo.Priority = MailPriority.High;
                _Correo.Body = "Buenos Días, \n \n" + "Se Genero el Archivo " + tipoArchivo + " : \n \n" +
                                   "Ruta del Archivo : " + archivoSalida + " \n \n" +
                                   "Nombre del Archivo : " + nombreArchivo + " \n \n" +
                                   "Número de Lineas : " + Convert.ToInt32(lineasArchivo) + " \n \n";
                //_Correo.Attachments.Add(new Attachment(@archivoSalida + nombreArchivo));
                SmtpClient objSmtpClient = new SmtpClient("exchange");
                objSmtpClient.Host = "chevyplan-com-co.mail.protection.outlook.com";
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                objSmtpClient.Send(_Correo);
            }
            catch
            {
                throw new System.Exception("Ocurrio un error al enviar archivo por correo");
            }
        }
        //ENVIA LOS CORREOS NOTIFICANDO LOS ERRORES CUANDO SE IBAN A CREAR LOS ARCHIVOS PAGOS ONLINE
        //ESTA INFORMACION SE ENVIA A LOS CORREOS DE NOTIFICACION QUE SE ENCUENTRAN EN UN SERVICIO WEB
        public void enviarNotificacionesError(String nombreBanco, String[] correosControl, String remitente,
                                                     String error, String tipoArchivo)
        {
            try
            {
                String Destinatarios = String.Empty;
                MailMessage _Correo = new MailMessage();
                _Correo.From = new MailAddress(remitente);

                foreach (String correo in correosControl)
                {
                    if (email_valido(correo))
                    _Correo.To.Add(correo);
                }
                if (_Correo.To.Count == 0)
                    return;
                _Correo.Subject = "Archivo " + tipoArchivo + " - Error en el Proceso";
                _Correo.Priority = MailPriority.High;
                _Correo.Body = "Buenos Días, \n \n" + "Ocurrio un error en el proceso de generación del archivo " + tipoArchivo + " del Banco: \n \n" +
                                    nombreBanco.ToUpper() + " \n \n" +
                                   "Fallo : " + error + " \n \n";
                SmtpClient objSmtpClient = new SmtpClient("exchange");
                objSmtpClient.Host = "chevyplan-com-co.mail.protection.outlook.com";
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                objSmtpClient.Send(_Correo);
            }
            catch
            {
                throw new System.Exception("Ocurrio un error al enviar correo");
            }
        }
        private Boolean email_valido(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}