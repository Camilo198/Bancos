using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Procesos.EN;
using Procesos.EN.Tablas;
using Procesos.LN.Consultas;

namespace Procesos.PS.Procesos
{
    public class Asobancaria
    {

        string[] CorreoControlB;
        string[] CorreoEnvioB;
        String mensaje;

        public List<String> obtenerBancosAsobancaria(ref bool procesoConError)
        {
             
            #region "Atributos Bancos"
            String NombreCuenta;
            String CodigoCuenta;
            String ArchivoSalidaAsobancaria;
            String TipoProceso;
            bool EsFTP;
            String UrlFTP;
            String UsuarioFTP;
            String ClaveFTP;
            ArrayList CorreosControl = new ArrayList();
            ArrayList CorreosEnvio = new ArrayList();
            String CodigoBanco;
            String Remitente;
            #endregion

            List<String> RespuestaProceso = new List<String>();

            try
            {

                //OBTIENE UNA LISTA DE OBJETOS DE BANCOS           
                List<Banco> lista = new List<Banco>();
                EN.Tablas.Banco objB = new EN.Tablas.Banco();
                objB.pActivo = true;
                objB.pTipoProceso = "ABP_";
                BancoLN objBancoLN = new BancoLN();
                lista = objBancoLN.consultar(objB);
                //OBTIENE LAS RUTAS DE LOS BANCOS
                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                //RECORREN TODOS LOS BANCOS Y LOS ENVIA UNO A UNO AL SERVICIO WEB AsoBancaria
                foreach (Banco bank in lista)
                {
                    TipoProceso = bank.pTipoProceso;
                    NombreCuenta = bank.pNombreCuenta;
                    CodigoCuenta = bank.pIdCuentaBanco;
                    EsFTP = bank.pFtp.Value;
                    UrlFTP = bank.pUrlFtp;
                    UsuarioFTP = bank.pUsuarioFtp;
                    ClaveFTP = bank.pClave;
                    CorreoControlB = bank.pCorreoControl.Split(';');
                    CorreoEnvioB = bank.pCorreoEnvio.Split(';');
                    foreach (string cc in CorreoControlB)
                    {
                        if (string.IsNullOrEmpty(cc)) break;
                        CorreosControl.Add(cc);
                    }
                    foreach (string ce in CorreoEnvioB)
                    {
                        if (string.IsNullOrEmpty(ce)) break;
                        CorreosEnvio.Add(ce);
                    }
                    CodigoBanco = bank.pCodigoBanco;
                    Remitente = bank.pRemitente;
                    objRuta.pOid = bank.pRutaArchivosSalida;
                    ArchivoSalidaAsobancaria = objRutaLN.consultar(objRuta)[0].pRuta;
                    mensaje = CorrerAsobancaria(NombreCuenta, CodigoCuenta, ArchivoSalidaAsobancaria, EsFTP, UrlFTP, UsuarioFTP,
                                      ClaveFTP, CorreosControl, CorreosEnvio, CodigoBanco, Remitente, TipoProceso);
                    if (!mensaje.Equals("Archivo Asobancaria fue generado correctamente"))
                    {
                        procesoConError = true;
                    }

                    //RespuestaProceso.Add(bank.pNombreCuenta + ": " + mensaje);

                    CorreosControl.Clear();
                    CorreosEnvio.Clear();
                }

                return RespuestaProceso;

            }
            catch (Exception ex)
            {
                procesoConError = true;
                //RespuestaProceso.Add(ex.Message);
                return RespuestaProceso;
            }

        }

        public String CorrerAsobancaria(String NombreCuenta, String CodigoCuenta, String ArchivoSalidaAsobancaria, bool EsFTP, String UrlFTP, String UsuarioFTP,
                                               String ClaveFTP, ArrayList CorreosControl, ArrayList CorreosEnvio, String CodigoBanco, String Remitente, String TipoProceso)
        {
            String mens;
            try
            {
                ServicioAsoBancaria.AsoBancaria procesoAso;
                procesoAso = new ServicioAsoBancaria.AsoBancaria();
                mens = procesoAso.ServicioAso(NombreCuenta, CodigoCuenta, ArchivoSalidaAsobancaria, EsFTP, EsFTP, UrlFTP, UsuarioFTP,
                                       ClaveFTP, (String[])CorreosControl.ToArray(typeof(String)),
                                                 (String[])CorreosEnvio.ToArray(typeof(String)), CodigoBanco, Remitente, "TAREA PROGRAMADA", TipoProceso);
                return mens;
            }
            catch
            {
                return "Error en el servicio";
            }
        }

    }
}
