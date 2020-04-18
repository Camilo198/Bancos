using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class PagosErradosLN
    {
        WcfData wsc = new Pagos.AD.Servicios.WcfData();


        public List<string[,]> ValidaExistePagoErrado(ObjetoTablas ObjPago, string procedimiento)
        {
            ObjetoTablas objPagoValidacion = new ObjetoTablas();
            List<string[,]> listaPagoValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaCampoPago = new List<ObjetoTablas>();

                string[, ,] Valor = new string[7, 3, 1];

                Valor[0, 0, 0] = ObjPago.pProcesoErr;
                Valor[0, 1, 0] = "@Proceso";
                Valor[0, 2, 0] = "char(10)";

                Valor[1, 0, 0] = ObjPago.pReferenciaErr;
                Valor[1, 1, 0] = "@Referencia";
                Valor[1, 2, 0] = "numeric(10, 0)";

                Valor[2, 0, 0] = ObjPago.pCodBancoErr;
                Valor[2, 1, 0] = "@CodBanco";
                Valor[2, 2, 0] = "int";

                Valor[3, 0, 0] = ObjPago.pValorPagoErr;
                Valor[3, 1, 0] = "@ValPago";
                Valor[3, 2, 0] = "numeric(18, 0)";

                Valor[4, 0, 0] = ObjPago.pFechaErr;
                Valor[4, 1, 0] = "@FecPago";
                Valor[4, 2, 0] = "datetime";

                Valor[5, 0, 0] = ObjPago.pForPagoErr;
                Valor[5, 1, 0] = "@ForPago";
                Valor[5, 2, 0] = "varchar(50)";

                Valor[6, 0, 0] = ObjPago.pUsuarioProcesoErr;
                Valor[6, 1, 0] = "@UsuProceso";
                Valor[6, 2, 0] = "varchar(30)";

                return listaPagoValida = wsc.LlenarLista(Valor, procedimiento, "SQLVentas", "SP", "Sql");

            }
            catch (Exception)
            {
                return listaPagoValida;
            }
        }

        public string InsertPagosErrados(ObjetoTablas ObjPago, string procedimiento)
        {
            try
            {
                string[, ,] Valor = new string[7, 3, 1];

                Valor[0, 0, 0] = ObjPago.pProcesoErr;
                Valor[0, 1, 0] = "@Proceso";
                Valor[0, 2, 0] = "char(10)";

                Valor[1, 0, 0] = ObjPago.pReferenciaErr;
                Valor[1, 1, 0] = "@Referencia";
                Valor[1, 2, 0] = "numeric(10, 0)";

                Valor[2, 0, 0] = ObjPago.pCodBancoErr;
                Valor[2, 1, 0] = "@CodBanco";
                Valor[2, 2, 0] = "int";

                Valor[3, 0, 0] = ObjPago.pValorPagoErr;
                Valor[3, 1, 0] = "@ValPago";
                Valor[3, 2, 0] = "numeric(18, 0)";

                Valor[4, 0, 0] = ObjPago.pFechaErr;
                Valor[4, 1, 0] = "@FecPago";
                Valor[4, 2, 0] = "datetime";

                Valor[5, 0, 0] = ObjPago.pForPagoErr;
                Valor[5, 1, 0] = "@ForPago";
                Valor[5, 2, 0] = "varchar(50)";

                Valor[6, 0, 0] = ObjPago.pUsuarioProcesoErr;
                Valor[6, 1, 0] = "@UsuProceso";
                Valor[6, 2, 0] = "varchar(30)";

                return wsc.Ejecutar(Valor, procedimiento, "SQLVentas");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
