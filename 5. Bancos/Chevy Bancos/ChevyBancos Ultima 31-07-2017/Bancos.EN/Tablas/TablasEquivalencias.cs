using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class TablasEquivalencias
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public String pNombre { get; set; }
        public String pTipoArchivo { get; set; }
        public int? pIdCuentaBanco { get; set; }
    }
}
