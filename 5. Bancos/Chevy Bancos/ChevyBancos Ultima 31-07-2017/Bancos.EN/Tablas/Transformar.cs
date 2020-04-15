using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class Transformar
    {
        public String pOperacion { get; set; }

        public int? pValorAsobancaria { get; set; }
        public int? pValorBanco { get; set; }
        public String pDescripcionBanco { get; set; }
        public String pCodigoBanco { get; set; }
        public String pDescripcionAsobancaria { get; set; }
        public String pCodigoAsobancaria { get; set; }
    }
}
