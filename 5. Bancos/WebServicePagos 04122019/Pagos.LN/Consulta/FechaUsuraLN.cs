using Pagos.AD.Consultas;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class FechaUsuraLN
    {
        private String SP_ConsultaFechaUsura = "pa_BAN_Fecha_Usura";
        public IList<FechaUsuraEN> ConsultarFechaUsuraLN()
        {
            IList<FechaUsuraEN> lista = new FechaUsuraAD().ConsultarFechaUsuraAD(SP_ConsultaFechaUsura);
            return lista;
        }
    }
}
