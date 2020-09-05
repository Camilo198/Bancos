using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.AD.Consultas
{
    public class RptPagosAD
    {
        WcfData wsc = new WcfData();

        //Retorna en una lista los parametros de la fecha de aplicacion del banco.
        public IList<RptPagosEN> ConsultarBancoFechaAD(string Procedimiento, RptPagosEN objEntidad)
        {
            List<RptPagosEN> listParametro = new List<RptPagosEN>();
            List<string[,]> lista = new List<string[,]>();

            try
            {
                string[,,] Param = new string[5, 3, 1]; // solo cuando el procedimiento almacenado tiene parametros


                Param[0, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodBanco";
                Param[0, 2, 0] = "varchar(10)";

                Param[1, 0, 0] = objEntidad.fechaPago.ToString();
                Param[1, 1, 0] = "@inFechPago";
                Param[1, 2, 0] = "date";

                Param[2, 0, 0] = "C";
                Param[2, 1, 0] = "@inOperacion";
                Param[2, 2, 0] = "varchar(2)";

                Param[3, 0, 0] = "0";
                Param[3, 1, 0] = "@inMontoarchivo";
                Param[3, 2, 0] = "numeric(18,0)";

                Param[4, 0, 0] = objEntidad.cantPagosArchivo.ToString();
                Param[4, 1, 0] = "@inPagosArchivo";
                Param[4, 2, 0] = "int";

                lista = wsc.LlenarLista(Param, Procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        RptPagosEN objParametros = new RptPagosEN();
                        Valida = lista[i];

                        objParametros.codigoBanco = Valida[0, 1].ToString();
                        objParametros.fechaPago = Valida[1, 1].ToString();
                        objParametros.cantPagosReacudo = Convert.ToInt32(Valida[2, 1].ToString());
                        listParametro.Add(objParametros);
                    }
                }

                return listParametro;
            }
            catch (Exception)
            {
                return listParametro;
            }
        }
        public string insertaBancoFechaAD(string procedimiento, RptPagosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodBanco";
                Param[0, 2, 0] = "varchar(10)";

                Param[1, 0, 0] = objEntidad.fechaPago.ToString();
                Param[1, 1, 0] = "@inFechPago";
                Param[1, 2, 0] = "date";

                Param[2, 0, 0] = "I";
                Param[2, 1, 0] = "@inOperacion";
                Param[2, 2, 0] = "varchar(2)";

                Param[3, 0, 0] = objEntidad.valorMontoArchivo.ToString();
                Param[3, 1, 0] = "@inMontoarchivo";
                Param[3, 2, 0] = "numeric(18,0)";

                Param[4, 0, 0] = objEntidad.cantPagosArchivo.ToString();
                Param[4, 1, 0] = "@inPagosArchivo";
                Param[4, 2, 0] = "int";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string actualizaBancoCantPagosRecAD(string procedimiento, RptPagosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = objEntidad.codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodBanco";
                Param[0, 2, 0] = "varchar(10)";

                Param[1, 0, 0] = objEntidad.fechaPago.ToString();
                Param[1, 1, 0] = "@inFechPago";
                Param[1, 2, 0] = "date";

                Param[2, 0, 0] = "U";
                Param[2, 1, 0] = "@inOperacion";
                Param[2, 2, 0] = "varchar(2)";

                Param[3, 0, 0] = objEntidad.valorMontoArchivo.ToString();
                Param[3, 1, 0] = "@inMontoarchivo";
                Param[3, 2, 0] = "numeric(18,0)";

                Param[4, 0, 0] = objEntidad.cantPagosArchivo.ToString();
                Param[4, 1, 0] = "@inPagosArchivo";
                Param[4, 2, 0] = "int";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string actualizaCantPagosArchPSEAD(string procedimiento, RptPagosEN objEntidad)
        {
            try
            {
                string[,,] Param = new string[1, 3, 1];

                Param[0, 0, 0] = objEntidad.fechaPago.ToString();
                Param[0, 1, 0] = "@inFechPago";
                Param[0, 2, 0] = "date";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
