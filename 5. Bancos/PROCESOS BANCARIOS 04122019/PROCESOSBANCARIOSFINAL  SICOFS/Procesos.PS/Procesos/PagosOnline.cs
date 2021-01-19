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
    public class PagosOnline
    {
        string[] CorreoControlB;
        String mensaje;

        public List<String> obtenerBancosPagosOnline(ref bool procesoConError, List<ArchivoEN> archivosOrdenados)
        {
            #region "Atributos Bancos"
            String NombreCuenta;
            String CodigoCuenta;
            String ArchivoSalida = "";
            String TipoProceso;
            ArrayList CorreosControl = new ArrayList();
            String CodigoBanco;
            String NumCuenta;
            String TipoCuenta;
            String Remitente;
            #endregion

            List<String> RespuestaProceso = new List<String>();

            try
            {

                //OBTIENE UNA LISTA DE OBJETOS DE BANCOS           
                List<Banco> lista = new List<Banco>();
                EN.Tablas.Banco objB = new EN.Tablas.Banco();
                objB.pActivo = true;
                objB.pTipoProceso = "POL_";
                BancoLN objBancoLN = new BancoLN();
                lista = objBancoLN.consultar(objB);
                //OBTIENE LAS RUTAS DE LOS BANCOS
                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                //RECORREN TODOS LOS BANCOS Y LOS ENVIA UNO A UNO AL SERVICIO WEB PagosOnline
                foreach (Banco bank in lista)
                {
                    TipoProceso = bank.pTipoProceso;
                    NombreCuenta = bank.pNombreCuenta;
                    CodigoCuenta = bank.pIdCuentaBanco;
                    TipoCuenta = bank.pTipoCuenta;
                    NumCuenta = bank.pNumCuenta;
                    CodigoBanco = bank.pCodigoBanco;
                    CorreoControlB = bank.pCorreoControl.Split(';');
                    foreach (string cc in CorreoControlB)
                    {
                        if (string.IsNullOrEmpty(cc)) break;
                        CorreosControl.Add(cc);
                    }
                    Remitente = bank.pRemitente;
                    objRuta.pOid = bank.pRutaArchivosEntrada;
                    ArchivoSalida = objRutaLN.consultar(objRuta)[0].pRuta;

                    mensaje = CorrerPagosOnline(NombreCuenta, CodigoCuenta, ArchivoSalida, CorreosControl,
                                                CodigoBanco, NumCuenta, TipoCuenta, Remitente, TipoProceso);
                    if (!mensaje.Equals("Proceso Pagos Online ejecutado con exito!!"))
                    {
                        procesoConError = true;
                    }
                    RespuestaProceso.Add(bank.pNombreCuenta + ": " + mensaje);
                    CorreosControl.Clear();
                }

                //AQUI INVOCA SERVICIO WSBANCOS PARA PROCESAR LOS PAGOS DE CADA UNO DE LOS ARCHIVOS
                #region Consultar  tabla bolsa  
                //ArchivoLN archivoLN = new ArchivoLN();
                //ArchivoEN archivoEN = new ArchivoEN();
                //archivoEN.Fecha = System.DateTime.Now;
                //archivoLN.consultarArchivoBolsaLN(archivoEN);
                if (archivosOrdenados.Count > 0)
                {

                    ArchivoEN topOne = archivosOrdenados.ElementAt(0);
                    #endregion

                    ServicioBancos.WsBancos ProcesoPagos = new ServicioBancos.WsBancos();
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(ArchivoSalida);

                    System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");

                    foreach (System.IO.FileInfo archivos in fileNames)
                    {
                        if (topOne.RutaArchivo == ArchivoSalida + archivos.Name)
                        {
                            mensaje = ProcesoPagos.LecturaPagos("usuario", "Pasword", ArchivoSalida, archivos.Name, "S");
                            //   mensaje = ProcesoPagos.LecturaPagos("", "", ArchivoSalida, archivos.Name, "S");
                            //RespuestaProceso.Add("PagosOnline: " + mensaje);
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
                    return RespuestaProceso;
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

        public String CorrerPagosOnline(String NombreCuenta, String CodigoCuenta, String ArchivoSalida,
                                        ArrayList CorreosControl, String CodigoBanco, String NumCuenta,
                                        String TipoCuenta, String Remitente, String TipoProceso)
        {
            String mens;
            try
            {
                ServicioPagosOnline.PagosOnline procesoPayOnline;
                procesoPayOnline = new ServicioPagosOnline.PagosOnline();
                mens = procesoPayOnline.ServicioPagosOnline(NombreCuenta, CodigoCuenta, ArchivoSalida,
                                                            (String[])CorreosControl.ToArray(typeof(String)),
                                                            CodigoBanco, NumCuenta, TipoCuenta, Remitente,
                                                            "TAREA PROGRAMADA", TipoProceso);
                ServicioBancos.WsBancos ProcesoPagos = new ServicioBancos.WsBancos();
                mensaje = ProcesoPagos.LecturaPagos("usuario", "Pasword", ArchivoSalida, ArchivoSalida, "S");

                return mens + "-" + mensaje;
            }
            catch (Exception ex)
            {
                return ex.Message;
                //return "Error en el servicio";
            }
        }
    }
}
