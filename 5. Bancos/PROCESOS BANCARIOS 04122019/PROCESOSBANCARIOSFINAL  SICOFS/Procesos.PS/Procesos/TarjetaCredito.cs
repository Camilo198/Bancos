using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesos.EN;
using Procesos.EN.Tablas;
using Procesos.LN.Consultas;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace Procesos.PS.Procesos
{
    public class TarjetaCredito
    {
        String mensaje;
        string[] CorreoControlB;

        public List<String> obtenerBancosTarjetasCredito(ref bool procesoConError)
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
            #endregion

            List<String> RespuestaProceso = new List<String>();

            try
            {
                //OBTIENE UNA LISTA DE OBJETOS DE BANCOS           
                List<Banco> lista = new List<Banco>();
                EN.Tablas.Banco objB = new EN.Tablas.Banco();
                objB.pActivo = true;
                objB.pTipoProceso = "TCR_";
                BancoLN objBancoLN = new BancoLN();
                lista = objBancoLN.consultar(objB);
                //OBTIENE LAS RUTAS DE LOS BANCOS
                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                //RECORREN TODOS LOS BANCOS Y LOS ENVIA UNO A UNO AL SERVICIO WEB AsoBancaria
                foreach (Banco bank in lista)
                {
                    TipoProceso = bank.pTipoProceso;
                    IdCuentaBanco = Convert.ToString(bank.pId.Value);
                    NombreCuenta = bank.pNombreCuenta;
                    IdCuentaBancoEpicor = bank.pIdCuentaBanco;
                    CorreoControlB = bank.pCorreoControl.Split(';');

                    foreach (string cc in CorreoControlB)
                    {
                        if (string.IsNullOrEmpty(cc)) break;
                        CorreosControl.Add(cc);// DESCOMENTAREAR A PRD
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

                    mensaje = CorrerTarjetaCredito(NombreCuenta, IdCuentaBanco, IdCuentaBancoEpicor, RutaEntrada, RutaProcesado,
                                                CorreosControl, Remitente, CodigoBanco, NumCuenta,
                                                TipoCuenta, RutaSalidaEpicor, TipoProceso);


                    if (!mensaje.Equals("Proceso Archivos Tarjeta Credito ejecutado con exito!!"))
                    {
                        procesoConError = true;
                    }

                    //RespuestaProceso.Add(bank.pNombreCuenta + ": " + mensaje);

                    CorreosControl.Clear();

                    //AQUI INVOCA SERVICIO WSBANCOS PARA PROCESAR LOS PAGOS DE CADA UNO DE LOS ARCHIVOS
                    ServicioBancos.WsBancos ProcesoPagos = new ServicioBancos.WsBancos();
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(RutaProcesado);

                    System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*").Where(file => !file.FullName.EndsWith(".xls")).ToArray();
                    fileNames = fileNames.Where(file => !file.FullName.EndsWith(".xlsx")).ToArray();
                    fileNames = fileNames.Where(file => !file.FullName.EndsWith(".db")).ToArray();
                    fileNames = fileNames.Where(file => !file.FullName.EndsWith(".csv")).ToArray();

                    try
                    {
                        foreach (System.IO.FileInfo archivos in fileNames)
                        {
                            mensaje = ProcesoPagos.PagosTarjeta("usuario", "Pasword", RutaProcesado, archivos.Name);
                            //RespuestaProceso.Add("Proceso Pagos TC: " + mensaje);
                        }

                    }
                    catch (Exception ex)
                    {

                        mensaje = ex.ToString();
                    }


                }

                return RespuestaProceso;

            }
            catch (Exception ex)
            {
                procesoConError = true;
                // RespuestaProceso.Add(ex.Message);
                return RespuestaProceso;
            }

        }

        public String CorrerTarjetaCredito(String NombreCuenta, String IdCuentaBanco, String IdCuentaBancoEpicor, String RutaEntrada,
                                              String RutaSalida, ArrayList CorreosControl, String Remitente,
                                              String CodigoBanco, String NumCuenta, String TipoCuenta, String RutaProcesado, String TipoProceso)
        {
            String mens;
            try
            {
                CorreosControl = new ArrayList();
                ServicioTarjetasCredito.TarjetasCredito procesoTC = new ServicioTarjetasCredito.TarjetasCredito();
                mens = procesoTC.ServicioTarjetasCredito("TCR", IdCuentaBanco, IdCuentaBancoEpicor, RutaEntrada, RutaSalida,
                                                    (String[])CorreosControl.ToArray(typeof(String)),
                                                    Remitente, CodigoBanco, NumCuenta, TipoCuenta,
                                                    "TAREA PROGRAMADA", RutaProcesado, TipoProceso);
                return mens;
            }
            catch (Exception ex)
            {
                return "Error en el servicio";
            }
        }
    }
}
