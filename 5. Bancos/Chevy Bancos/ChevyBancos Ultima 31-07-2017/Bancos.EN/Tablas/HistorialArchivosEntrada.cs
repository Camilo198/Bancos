using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class HistorialArchivosEntrada
    {
        public String pOperacion { get; set; }
        public String pFecha { get; set; }
        public String pFechaTransaccion { get; set; }
        public String pIdCuentaBanco { get; set; }
        public String pTipoArchivo { get; set; }
        public String pConsecutivo { get; set; }
        public String pLineaDetalle { get; set; }
    }
}
