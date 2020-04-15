using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class TransRespuesta
    {
        public String pOperacion { get; set; }

        public int? pRespuestaTAsoba { get; set; }
        public int? pRespuestaTBanco { get; set; }

        public String pCodigoRespuestaTAsoba { get; set; }
        public String pCodigoRespuestaTBanco { get; set; }

        public String pDetalleRespuestaTAsoba { get; set; }
        public String pDetalleRespuestaTBanco { get; set; }
    }
}
