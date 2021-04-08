using Pagos.AD.Consultas;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class ArchivoLN
    {
        private string SP_Insert_Arch_Banco = "pa_Ban_Inserta_Lineas_Arch_Banco";
        private string SP_Elimina_Arch_Banco = "pa_Ban_Elimina_Lineas_Arch_Banco";
        private string SP_ArchNoAplicadosSICO = "pa_Ban_FTP_ArchNoAplicadosSICO";
        public IList<String> almacenaLineasArchivoLN(String RutaArchivo, String NombreArchivo, String PagosOnline, ArchivoEN archivoLinea, out String mensaje)
        {
            String NombreBanco, RutaEpicor, RutaProceso, aux = "";
            String parteFijaAbstracta = "";
            String CodBanco = "";
            int contLineas = 0;
            string parteFijaOriginal = "";
            String Fiducia = "F1";
            DataSet CodigoArchivos = new DataSet();
            PagosLN PagoValdLN = new PagosLN();
            List<string[,]> ConsultaCodigoBanco = new List<string[,]>();
            List<string> lineaDetalle = new List<string>();
            bool flag_06 = false;
            bool flag_01 = false;
            bool flag_09 = false;
            bool flag_archivo = false;
            String error_mensaje = "";
            String Fichero = RutaArchivo + NombreArchivo;

            RecaudoLN objRecaudo = new RecaudoLN();

            try
            {
                if (Fichero.Contains(".xlsx") || Fichero.Contains(".xls") || Fichero.Contains(".csv") || Fichero.Contains(".db")) // Mueve fichero de otros formatos
                {
                    string fecha = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString() + "_" +
                      DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + "_";

                    aux = RutaArchivo;
                    String rutaProc = aux.Replace("Recibidos", "Procesados");
                    String RutaDestino = System.IO.Path.Combine(rutaProc + fecha + NombreArchivo);

                    if (!Directory.Exists(rutaProc))
                    {
                        System.IO.Directory.CreateDirectory(rutaProc);
                    }
                    
                    System.IO.File.Move(Fichero, RutaDestino );
                    mensaje = "Archivo con formato diferente al admitido movido "+ Fichero;
                    return null;
                }
                if (new FileInfo(Fichero).Length == 0)  // Archivo vacío
                {
                    aux = RutaArchivo;

                    String rutaProc = aux.Replace("Recibidos", "Procesados");
                    String RutaDestino = System.IO.Path.Combine(rutaProc + NombreArchivo);

                    string fecha = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString() + "_" +
                      DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";

                    if (!Directory.Exists(rutaProc))
                    {
                        System.IO.Directory.CreateDirectory(rutaProc);
                    }
                    System.IO.File.Move(Fichero, RutaDestino + "_" + fecha);
                    RptPagosLN pagosLN = new RptPagosLN();
                    error_mensaje = "Fichero vacio " + RutaArchivo + NombreArchivo;
                    pagosLN.insertaLogErroresLN(error_mensaje, DateTime.Now.ToString(), 0, "");
                    mensaje = "Archivo vacío movido " + Fichero;
                    return null;
                }

                #region datos del banco
                if (PagosOnline == "S")
                    archivoLinea.parteFija = NombreArchivo; //Nombre del archivo del banco para pagos PSE
                else
                    archivoLinea.parteFija = NombreArchivo.TrimStart('0'); //Nombre del archivo del banco para recaudo

                CodigoArchivos = PagoValdLN.ConsultaArchivo(Fiducia, "S");

                if (CodigoArchivos.Tables["TablaBanPagos"].Rows.Count > 0)
                {
                    for (int i = 0; i < CodigoArchivos.Tables["TablaBanPagos"].Rows.Count; i++)
                    {
                        if (Fichero.Contains(CodigoArchivos.Tables["TablaBanPagos"].Rows[i]["ParteFija"].ToString()))
                        {
                            ConsultaCodigoBanco = PagoValdLN.ValidaCodigoBanco(PagosOnline, CodigoArchivos.Tables["TablaBanPagos"].Rows[i]["ParteFija"].ToString(),
                                                                               Fiducia, "ConsultaLugarPago");
                            if (ConsultaCodigoBanco.Count > 0)
                            {
                                string[,] ArregloCodigo = ConsultaCodigoBanco[0];
                                CodBanco = ArregloCodigo[0, 1].ToString();
                                NombreBanco = ArregloCodigo[1, 1].ToString();
                                RutaEpicor = ArregloCodigo[2, 1].ToString();
                                RutaProceso = ArregloCodigo[3, 1].ToString();
                                parteFijaAbstracta = CodigoArchivos.Tables["TablaBanPagos"].Rows[i]["ParteFija"].ToString();
                                parteFijaOriginal = parteFijaAbstracta;
                                flag_archivo = true;
                            }
                            else
                            {
                                RptPagosLN pagosLN = new RptPagosLN();
                                error_mensaje = "OCURRIO UN ERROR CON EL NOMBRE DEL ARCHIVO" + RutaArchivo + NombreArchivo;
                                pagosLN.insertaLogErroresLN(error_mensaje, DateTime.Now.ToString(), 0, "");
                                mensaje = error_mensaje;
                                return null;
                            }
                        }
                    }
                }
                else
                {
                    mensaje = "Error en consulta tabla Ban Pagos";
                    return null;
                }
                if (flag_archivo == false)
                {
                    RptPagosLN pagosLN = new RptPagosLN();
                    error_mensaje = "OCURRIO UN ERROR CON EL NOMBRE DEL ARCHIVO" + RutaArchivo + NombreArchivo;
                    pagosLN.insertaLogErroresLN(error_mensaje, DateTime.Now.ToString(), 0, "");
                    mensaje = error_mensaje;
                    return null;
                }
                #endregion
                foreach (var linea in File.ReadLines(Fichero))
                {
                    try
                    {
                        if (linea.Substring(0, 2) == "01")
                        {
                            flag_01 = true;
                        }
                        if (Convert.ToInt32(linea.Substring(0, 2)) == 06)
                        {
                            flag_06 = true;
                        }
                        if (Convert.ToInt32(linea.Substring(0, 2)) == 09)
                        {
                            flag_09 = true;
                            lineaDetalle.Add(linea);
                            break;
                        }
                        lineaDetalle.Add(linea);
                    }
                    catch (Exception)
                    {

                    }

                }

                if (lineaDetalle == null || lineaDetalle.Count == 0)
                {
                    mensaje = "Archivo vacío sin pagos" + Fichero;
                    return null;
                }

                // Archivo con otra estructura
                if (flag_01 == false && flag_06 == false && flag_09 == false)
                {

                    aux = RutaArchivo;
                    String rutaProc = aux.Replace("Recibidos", "Procesados");
                    String RutaDestino = System.IO.Path.Combine(rutaProc + NombreArchivo);

                    string fecha = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString() + "_" +
                      DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";

                    if (!Directory.Exists(rutaProc))
                    {
                        System.IO.Directory.CreateDirectory(rutaProc);
                    }
                    System.IO.File.Move(Fichero, RutaDestino + "_" + fecha);
                    mensaje = "Archivo con otra estructura movido "+ Fichero;
                    return null;
                }

                if (flag_06 == false) // Archivo sin pagos sin linea 06
                {
                    aux = RutaArchivo;
                    String rutaProc = aux.Replace("Recibidos", "Procesados");
                    String RutaDestino = System.IO.Path.Combine(rutaProc + NombreArchivo);

                    if (!Directory.Exists(rutaProc))
                    {
                        System.IO.Directory.CreateDirectory(rutaProc);
                    }
                    string fecha = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString() + "_" +
                      DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";

                    System.IO.File.Move(Fichero, RutaDestino + "_" + fecha);
                    mensaje = "Archivo sin pagos, linea 06 inexistente "+ Fichero;
                    RptPagosLN pagosLN = new RptPagosLN();
                    pagosLN.insertaLogErroresLN(mensaje, DateTime.Now.ToString(), 0, "");
                    return null;
                }

                if (CodBanco != "")
                {
                    archivoLinea.codBanco = Convert.ToInt32(CodBanco);
                }
                archivoLinea.parteFija = parteFijaOriginal;

                if (flag_01)
                {
                    aux = lineaDetalle.FirstOrDefault(s => Convert.ToInt32(s.Substring(0, 2)) == 01);
                    archivoLinea.fechaRecaudo = aux.Substring(12, 4) + "/" + aux.Substring(16, 2) + "/" + aux.Substring(18, 2);
                }
                aux = "";
                if (flag_09)
                {
                    aux = lineaDetalle.FirstOrDefault(s => Convert.ToInt32(s.Substring(0, 2)) == 09);
                    archivoLinea.cantPagos = Convert.ToInt32(aux.Substring(3, 8));
                }

                List<string[,]> res_rec = objRecaudo.consultarDisponibilidad(CodBanco, parteFijaOriginal);/*DisponibilidadArchivos*/

                if (res_rec.Count > 0) // ESTA OCUPADA LA DISPONIBILIDAD
                {
                    mensaje = "NO HAY DISPONIBILIDAD DE EJECUCION PARA ESTE BANCO" + CodBanco + " " + DateTime.Now.ToString() + Fichero;
                    return null;
                }

                List<ArchivoEN> lineas = new List<ArchivoEN>();

                foreach (var item in lineaDetalle)
                {
                    contLineas++;
                    ArchivoEN lineArchivo = new ArchivoEN();
                    lineArchivo.numLinea = contLineas;
                    lineArchivo.codBanco = Convert.ToInt32(CodBanco);
                    lineArchivo.parteFija = archivoLinea.parteFija;
                    lineArchivo.fechaRecaudo = archivoLinea.fechaRecaudo;
                    lineArchivo.cantPagos = archivoLinea.cantPagos;
                    lineArchivo.fechaProceso = archivoLinea.fechaProceso;
                    lineArchivo.linea = item;
                    lineas.Add(lineArchivo);
                    lineArchivo = null;
                }

                var res = InsertarLineasPagoLN(lineas);

                mensaje = "OK";
                return lineaDetalle;
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                return null;
            }

        }

        public String InsertarLineasPagoLN(IList<ArchivoEN> pagos)
        {
            return new ArchivoAD().InsertarLineasPagoAD(pagos, SP_Insert_Arch_Banco);
        }

        public String EliminarLineasPagoLN(ArchivoEN objEntidad, String Operacion)
        {
            return new ArchivoAD().EliminarLineasPagoAD(SP_Elimina_Arch_Banco, objEntidad, Operacion);
        }

        public String InsertarArchNoAplicadosSICOLN(ArchNoAPSICOEN objEntidad, String Operacion)
        {
            return new ArchivoAD().InsertarArchNoAplicadosSICOAD(SP_ArchNoAplicadosSICO, objEntidad, Operacion);
        }
        public IList<ArchNoAPSICOEN> ConsultarArchNoAplicadosSICOLN(ArchNoAPSICOEN objEntidad, String Operacion)
        {
            return new ArchivoAD().ConsultarArchNoAplicadosSICOLN(SP_ArchNoAplicadosSICO, objEntidad, Operacion);
        }
        public String EliminarArchNoAplicadosSICOLN(ArchNoAPSICOEN objEntidad, String Operacion)
        {
            return new ArchivoAD().EliminarArchNoAplicadosSICOLN(SP_ArchNoAplicadosSICO, objEntidad, Operacion);
        }
    }
}
