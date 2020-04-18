using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class PagosInconsistentesLN
    {
        WcfData wsc = new Pagos.AD.Servicios.WcfData();

        public string PagosIncon(ObjetoTablas ObjPagoIn, string procedimiento)
        {
            try
            {
                string[, ,] Valor = new string[9, 3, 1];

                Valor[0, 0, 0] = ObjPagoIn.pCodBanco.ToString();
                Valor[0, 1, 0] = "@CodBanco";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = ObjPagoIn.pFecPago;
                Valor[1, 1, 0] = "@PagFecha";
                Valor[1, 2, 0] = "datetime";

                Valor[2, 0, 0] = ObjPagoIn.pContrato;
                Valor[2, 1, 0] = "@Referencia";
                Valor[2, 2, 0] = "varchar(15)";

                Valor[3, 0, 0] = ObjPagoIn.pValPago;
                Valor[3, 1, 0] = "@PagValor";
                Valor[3, 2, 0] = "numeric(38)";

                Valor[4, 0, 0] = ObjPagoIn.pForPago;
                Valor[4, 1, 0] = "@PagForma";
                Valor[4, 2, 0] = "varchar(10)";

                Valor[5, 0, 0] = ObjPagoIn.pPagOficina;
                Valor[5, 1, 0] = "@PagOficina";
                Valor[5, 2, 0] = "varchar(50)";

                Valor[6, 0, 0] = ObjPagoIn.pNomArchivo;
                Valor[6, 1, 0] = "@NomArchivo";
                Valor[6, 2, 0] = "varchar(50)";

                Valor[7, 0, 0] = ObjPagoIn.pNumLote;
                Valor[7, 1, 0] = "@NumLote";
                Valor[7, 2, 0] = "numeric(38)";

                Valor[8, 0, 0] = ObjPagoIn.pUsuProceso.ToString();
                Valor[8, 1, 0] = "@UsuProceso";
                Valor[8, 2, 0] = "varchar(30)";

                return wsc.Ejecutar(Valor, procedimiento, "SQLVentas");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string[,]> ValidaExistePagoInconsistente(ObjetoTablas PagosInconsistentes, string procedimiento)
        {
            ObjetoTablas objPagoInconsistenteValidacion = new ObjetoTablas();
            List<string[,]> listaPagoInconsistenteValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaCampoPago = new List<ObjetoTablas>();
                string[, ,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = PagosInconsistentes.pCodBanco.ToString();
                Param[0, 1, 0] = "@CodBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = PagosInconsistentes.pContrato;
                Param[1, 1, 0] = "@Referencia";
                Param[1, 2, 0] = "varchar(15)";

                Param[2, 0, 0] = PagosInconsistentes.pValPago;
                Param[2, 1, 0] = "@PagValor";
                Param[2, 2, 0] = "numeric(38)";

                Param[3, 0, 0] = PagosInconsistentes.pForPago;
                Param[3, 1, 0] = "@PagForma";
                Param[3, 2, 0] = "varchar(10)";

                Param[4, 0, 0] = PagosInconsistentes.pFecPago;
                Param[4, 1, 0] = "@PagFecha";
                Param[4, 2, 0] = "datetime";

                listaPagoInconsistenteValida = wsc.LlenarLista(Param, procedimiento, "SQLVentas", "SP", "Sql");
                string[,] ValidaPago_;
                for (int l = 0; l < listaPagoInconsistenteValida.Count; l++)
                {
                    ValidaPago_ = listaPagoInconsistenteValida[l];
                    objPagoInconsistenteValidacion.pContrato = ValidaPago_[0, 1].ToString();
                    ListValidaCampoPago.Add(objPagoInconsistenteValidacion);
                }

                return listaPagoInconsistenteValida;
            }
            catch (Exception)
            {
                return listaPagoInconsistenteValida;
            }
        }
    }
}
