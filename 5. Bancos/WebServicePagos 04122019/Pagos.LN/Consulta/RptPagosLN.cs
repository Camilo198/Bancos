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
        private String SP_ConsultarPagosPSE   = "pa_BAN_CON_PSE_BANCO_PAGOS";
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

    }
}
