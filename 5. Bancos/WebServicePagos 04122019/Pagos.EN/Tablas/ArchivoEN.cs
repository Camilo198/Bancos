using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.EN.Tablas
{
    public class ArchivoEN
    {
        public int numLinea { get; set; }
        public int codBanco { get; set; }
        public String parteFija { get; set; }
        public String fechaRecaudo { get; set; }
        public DateTime fechaProceso { get; set; } 
        public String linea { get; set; }
        public int cantPagos { get; set; }
    }
}
