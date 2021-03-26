using Procesos.AD.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.AD.Consultas
{
    public class LogAD
    {
        WcfData wsc = new WcfData();
        public string insertaLogErroresAD(String procedimiento, String mensaje, int codigoBanco,
                                          String fechaPago, String parteFija)
        {
            try
            {
                string[,,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = codigoBanco.ToString();
                Param[0, 1, 0] = "@inCodigoBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = mensaje;
                Param[1, 1, 0] = "@inMensaje";
                Param[1, 2, 0] = "nvarchar(MAX)";

                Param[2, 0, 0] = fechaPago;
                Param[2, 1, 0] = "@inFechaPago";
                Param[2, 2, 0] = "date";

                Param[3, 0, 0] = parteFija;
                Param[3, 1, 0] = "@inParteFija";
                Param[3, 2, 0] = "varchar(20)";

                Param[4, 0, 0] = "I";
                Param[4, 1, 0] = "@inOperacion";
                Param[4, 2, 0] = "varchar(3)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }
    }
}
