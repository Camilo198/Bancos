using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class Valor
    {
        public String pOperacion { get; set; }

        public String pCodigo { get; set; }
        public String pDescripcion { get; set; }
        public int? pOid { get; set; }
        public int? pTabla { get; set; }
    }
}
