using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Procesos.PS.Codigo
{
    public class EnviarCorreo
    {
        public void enviarNotificaciones(String correoControl, String remitente,
                                         String Mensaje, String Proceso)
        {
            try
            {
                String Destinatarios = String.Empty;
                MailMessage _Correo = new MailMessage();
                _Correo.From = new MailAddress(remitente);

                if (email_valido(correoControl))
                    _Correo.To.Add(correoControl);
            
                if (_Correo.To.Count == 0)
                    return;
                _Correo.Subject = "Proceso " + Proceso;
                _Correo.Priority = MailPriority.High;
                _Correo.Body =  Proceso + ": \n \n" + Mensaje  + " \n \n";
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
