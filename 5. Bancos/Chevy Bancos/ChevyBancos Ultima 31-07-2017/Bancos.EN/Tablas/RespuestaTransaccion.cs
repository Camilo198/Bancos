using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class RespuestaTransaccion
    {
        public String pOperacion { get; set; }

        public String pCausal { get; set; }
        public String pDescripcionEstandar { get; set; }
        public String pDetalleAdicional { get; set; }
        public int? pOid { get; set; }
        public String pPrenotificacion { get; set; }
        public int? pTabla { get; set; }
        public String pTransaccionDebito { get; set; }
    }
}
