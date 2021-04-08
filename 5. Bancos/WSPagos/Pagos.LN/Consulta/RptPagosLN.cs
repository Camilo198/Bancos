using Pagos.AD.Consultas;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class RptPagosLN
    {
        private String SP_ConsultarPagosBanco = "pa_BAN_CON_RPT_BANCO_PAGOS";
        private String SP_ConsultarPagosPSE = "pa_BAN_CON_RPT_BANCO_PAGOS_PSE";
        private String SP_ActualizarRptPagoSico = "pa_BAN_CON_ACT_RPT_PAGOS_SICO";
        private String SP_LogErrores = "pa_BAN_RPT_ERROR_LOG";
        public IList<RptPagosEN> ConsultarBancoFechaLN(RptPagosEN objEntidad)
        {

            IList<RptPagosEN> lista = new RptPagosAD().ConsultarBancoFechaAD(SP_ConsultarPagosBanco, objEntidad);
            return lista;
        }
        public string insertarBancoFechaLN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().insertaBancoFechaAD(SP_ConsultarPagosBanco, objEntidad);
            return resultado;

        }
        public string actualizarBancoCantPagosRecaudoLN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().actualizaBancoCantPagosRecAD(SP_ConsultarPagosBanco, objEntidad);
            return resultado;

        }
        public string actualizarCantPagosArchPSELN(RptPagosEN objEntidad)
        {

            string resultado = new RptPagosAD().actualizaCantPagosArchPSEAD(SP_ConsultarPagosPSE, objEntidad);
            return resultado;

        }
        public string actualizarRptPagoSicoLN(RptPagosEN objEntidad)
        {
            string resultado = new RptPagosAD().actualizarRptPagoSicoAD(SP_ActualizarRptPagoSico, objEntidad);
            return resultado;
        }
        public void almacenaRegistroSicoLN(WcfUtilidades Util, string NombreArchivoSico, string UsuFTP, string PassFTP, int codBanco,
            string fechaRecaudo, DateTime FeModificacion, string parteFijaAbstracta)
        {
            string error_mensaje = String.Empty;
            string msj_err = String.Empty;
            IList<string> recaudoSico;
            IList<String> inconsistentes;
            IList<String> consistentes;
            //SAU Revisar fichero en SICO
            string[] stringSeparators = new string[] { " " };
            RptPagosEN pagosEN = new RptPagosEN();

            IList<RptPagosEN> arrPagos = null;

            pagosEN.codigoBanco = codBanco;
            pagosEN.fechaPago = fechaRecaudo;
            pagosEN.fechaModificacionArch = FeModificacion;
            pagosEN.parteFija = parteFijaAbstracta;
            // Leo los resultados consistentes e inconsistentes de System
            try
            {
                //  inconsistentes = Util.LeerFicheroFTP(ServidorSico, "IR" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                inconsistentes = Util.LeerFicheroFTP("IR" + NombreArchivoSico, UsuFTP, PassFTP, out msj_err, pagosEN.fechaPago, pagosEN.codigoBanco );

                if (inconsistentes.Count > 0)
                {
                    try
                    {
                        string txtPago = inconsistentes.ToList().Find(x => x.Contains("Total Registros"));
                        if (txtPago != null)
                        {
                            recaudoSico = txtPago.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            pagosEN.cantPagosSicoInc = Convert.ToInt32(recaudoSico.ElementAt(2).Replace(',', '.'));
                            pagosEN.valorMontoSicoInc = Convert.ToDouble(recaudoSico.ElementAt(3).Replace(',', '.'));
                            recaudoSico = null;
                        }
                        else
                        {
                            String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            if (res == "1")
                            {
                                this.actualizaLogErroresLN("Archivo" + "IR" + NombreArchivoSico + "cortado o vacío " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            else
                            {
                                this.insertaLogErroresLN("Archivo" + "IR" + NombreArchivoSico + "cortado o vacío " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (res == "1")
                        {
                            this.actualizaLogErroresLN(e.Message.ToString() + " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            this.insertaLogErroresLN(e.Message.ToString()+ " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                    }

                }
                else
                {
                    String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    if (res == "1")
                    {
                        this.actualizaLogErroresLN(msj_err + "Ha ocurrido un error leyendo el Archivo " + "IR" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                    else
                    {
                        this.insertaLogErroresLN(msj_err + "Ha ocurrido un error leyendo el Archivo " + "IR" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                }

            }
            catch (Exception ex)
            {
                String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                if (res == "1")
                {
                    this.actualizaLogErroresLN(ex.Message.ToString()+ " "+ msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }
                else
                {
                    this.insertaLogErroresLN("Error en lectura de archivo IR" + NombreArchivoSico + " " + ex.Message.ToString()+ " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }

                inconsistentes = new List<string>();
            }
            msj_err = String.Empty;
            try
            {
                //  consistentes = Util.LeerFicheroFTP(ServidorSico, "R" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, pagosEN.fechaPago, pagosEN.codigoBanco);
                consistentes = Util.LeerFicheroFTP("R" + NombreArchivoSico, UsuFTP, PassFTP, out msj_err, pagosEN.fechaPago, pagosEN.codigoBanco);
                if (consistentes.Count > 0)
                {
                    try
                    {
                        string txtPago = consistentes.ToList().Find(x => x.Contains("Total Registros"));
                        if (txtPago != null)
                        {
                            recaudoSico = txtPago.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            pagosEN.cantPagosSicoCon = Convert.ToInt32(recaudoSico.ElementAt(2).Replace(',', '.'));
                            pagosEN.valorMontoSicoCon = Convert.ToDouble(recaudoSico.ElementAt(3).Replace(',', '.'));
                            recaudoSico = null;
                        }
                        else
                        {
                            String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            if (res == "1")
                            {
                                this.actualizaLogErroresLN("Archivo" + "R" + NombreArchivoSico + "cortado o vacío o en construcción" + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                            else
                            {
                                this.insertaLogErroresLN("Archivo" + "R" + NombreArchivoSico + "cortado o vacío o en construcción " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (res == "1")
                        {
                            this.actualizaLogErroresLN(e.Message.ToString()+ " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            this.insertaLogErroresLN(e.Message.ToString() + " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                    }
                }
                else
                {
                    String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    if (res == "1")
                    {
                        this.actualizaLogErroresLN(msj_err + "Ha ocurrido un error leyendo el Archivo " + "IR" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                    else
                    {
                        this.insertaLogErroresLN(msj_err + "Ha ocurrido un error leyendo el Archivo " + "IR" + NombreArchivoSico, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                }
            }
            catch (Exception ex)
            {
                String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                if (res == "1")
                {
                    this.actualizaLogErroresLN(ex.Message.ToString() + " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }
                else
                {
                    this.insertaLogErroresLN("Error en lectura de archivo R" + NombreArchivoSico + " " + ex.Message.ToString() + " " + msj_err, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                }

                consistentes = new List<string>();
            }
            msj_err = String.Empty;
            arrPagos = this.ConsultarBancoFechaLN(pagosEN);
            if (arrPagos.Count > 0) //Si existe
            {
                try
                {
                    int result = Convert.ToInt32(this.actualizarRptPagoSicoLN(pagosEN));
                    if (result == 0)
                    {
                        error_mensaje = "Error en la actualizacion Pagos/Montos Sico I/IR banco: " +
                                                pagosEN.codigoBanco + " " + pagosEN.fechaPago;
                        
                        String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        if (res == "1")
                        {
                            this.actualizaLogErroresLN(error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                        }
                        else
                        {
                            this.insertaLogErroresLN(error_mensaje, pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                            error_mensaje = String.Empty;
                        }
                        error_mensaje = String.Empty;
                    }
                }
                catch (Exception e)
                {
                    String res = this.consultaLogErroresLN("", pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    if (res == "1")
                    {
                        this.actualizaLogErroresLN(e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                    else
                    {
                        this.insertaLogErroresLN(e.Message.ToString(), pagosEN.fechaPago, pagosEN.codigoBanco, pagosEN.parteFija);
                    }
                }
            }
        }
        public void insertaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0, String parteFija = "")
        {
            String resultado = new RptPagosAD().insertaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fechaPago, parteFija);
        }
        public String consultaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0, String parteFija = "")
        {
            return new RptPagosAD().consultaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fechaPago, parteFija);
        }
        public String actualizaLogErroresLN(String mensaje, String fechaPago, int codigoBanco = 0, String parteFija = "")
        {
            return new RptPagosAD().actualizaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fechaPago, parteFija);
        }
    }
}
