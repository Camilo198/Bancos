using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class InterpreteArchivo
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public int? pCampoBanco { get; set; }
        public int? pCampoAsobancaria { get; set; }

        public String pNombreCampoBanco { get; set; }
        public String pNombreCampoAsobancaria { get; set; }
        public String pTipoLinea { get; set; }


    }
}
