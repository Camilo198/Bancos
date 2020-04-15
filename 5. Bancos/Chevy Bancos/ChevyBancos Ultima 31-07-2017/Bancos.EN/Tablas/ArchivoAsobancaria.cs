using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class ArchivoAsobancaria
    {
        public String pOperacion { get; set; }

        public String pOid { get; set; }
        public String pNombre { get; set; }
    }
}
