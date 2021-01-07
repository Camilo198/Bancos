using Procesos.AD.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.LN.Consultas
{
    public class LogLN
    {
        private String SP_LogErrores = "pa_BAN_RPT_ERROR_LOG";
        public void insertaLogErroresLN(String mensaje, String fecha, int codigoBanco = 0, String parteFija = "")
        {
            String resultado = new LogAD().insertaLogErroresAD(SP_LogErrores, mensaje, codigoBanco, fecha, parteFija);
        }
    }
}
