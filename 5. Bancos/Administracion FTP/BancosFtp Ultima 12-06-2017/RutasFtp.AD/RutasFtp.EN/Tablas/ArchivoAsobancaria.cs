using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RutasFtp.EN.Tablas
{
    [Serializable()]
    public class ArchivoAsobancaria
    {
        public String pOperacion { get; set; }

        public String pOid { get; set; }
        public String pNombre { get; set; }
    }
}
