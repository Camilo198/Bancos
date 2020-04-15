using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;

using RutasFtp.EN;
using log4net;

namespace RutasFtp.LN.Utilidades
{
    public class Mensajero
    {
        public String Error { get; set; }
        public String RutaXML
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory + "Modulos\\XML\\Configuracion.xml";
            }
        }
        public ILog Registrador { get; set; }

        public Mensajero()
        {
            Registrador = LogManager.GetLogger(this.GetType());
            Error = String.Empty;
        }


        public void enviarFuncionariosCorreo(String destinatarios, String asunto, String mensaje)
        {
            CamposXML objCampos = new CamposXML();
            objCampos.pCampo = "FromMail";
            objCampos.pTabla = "BD";
            LectorXML objLector = new LectorXML();
            objLector.RutaXML = RutaXML;
            objLector.leerDatosXML(objCampos);
            String correoEnvio = objLector.leerDatosXML(objCampos);

            objCampos = new CamposXML();
            objCampos.pCampo = "ServidorExchange";
            objCampos.pTabla = "BD";
            String servidorEx = objLector.leerDatosXML(objCampos);

            try
            {
                SmtpClient objSmtpClient = new SmtpClient("exchange");
                objSmtpClient.Host = servidorEx;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //sMail.Credentials = new NetworkCredential("username","password");
                objSmtpClient.Send(correoEnvio, destinatarios, asunto, mensaje);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Registrador.Error(Error);
            }
        }
    }
}
