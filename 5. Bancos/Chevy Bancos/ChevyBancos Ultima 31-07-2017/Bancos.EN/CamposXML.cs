using System;
using System.Collections.Generic;
using System.Text;

namespace Bancos.EN
{
    [Serializable()]
    public class CamposXML
    {
        public String pTabla { get; set; }
        public String pCampo { get; set; }
        public String pValor { get; set; }
    }
}
