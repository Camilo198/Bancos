using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pagos.AD.Consultas;
using Pagos.EN.Tablas;

namespace Pagos.LN.Consulta
{
   public class TiemposLN
   {
       private String SP_ConsultarHoraAplicacionBanco = "pa_Ban_Consulta_Hora_Aplicacion";
       private String SP_ConsultarSleepAplicacionBanco = "pa_Ban_Consulta_Sleep_Aplicacion";
       public IList<ObjetoTablas> ConsultarHoraAplicacionBancoLN( ObjetoTablas objEntidad)
       {

           IList<ObjetoTablas> lista = new TiemposAD().ConsultarHoraAplicacionBancoAD(SP_ConsultarHoraAplicacionBanco, objEntidad);
           return lista;
       }

       public IList<ObjetoTablas> ConsultarSleepAplicacionBancoLN(ObjetoTablas objEntidad)
       {

           IList<ObjetoTablas> lista = new TiemposAD().ConsultarSleepAplicacionBancoAD(SP_ConsultarSleepAplicacionBanco, objEntidad);
           return lista;
       }
    }
}
