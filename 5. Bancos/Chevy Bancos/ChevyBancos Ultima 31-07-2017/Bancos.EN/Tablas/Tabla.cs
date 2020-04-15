using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class Tabla
    {
        public String pOperacion { get; set; }

        public String pBanco { get; set; }
        public bool? pEsAsobancaria { get; set; }
        public String pNombre { get; set; }
        public int? pOid { get; set; }
    }
}
