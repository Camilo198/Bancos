using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class Configuracion
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public int? pArchivoPlano { get; set; }
        public String pTipoLinea { get; set; }
        public String pArchivoAsobancaria { get; set; }
    }
}
