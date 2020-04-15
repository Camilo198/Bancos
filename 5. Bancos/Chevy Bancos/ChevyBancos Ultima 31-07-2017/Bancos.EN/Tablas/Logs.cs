using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class Logs
    {
        public String pOperacion { get; set; }
        public String pFecha { get; set; }
        public String pUsuario { get; set; }
        public String pDetalle { get; set; }
        public String pTipoArchivo { get; set; }
        public String pTipoProceso { get; set; }
    }
}
