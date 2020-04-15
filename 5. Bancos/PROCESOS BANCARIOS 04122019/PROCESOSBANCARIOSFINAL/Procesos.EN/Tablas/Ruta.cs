using System;

namespace Procesos.EN.Tablas
{
    [Serializable()]
    public class Ruta
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public String pRuta { get; set; }
    }
}
