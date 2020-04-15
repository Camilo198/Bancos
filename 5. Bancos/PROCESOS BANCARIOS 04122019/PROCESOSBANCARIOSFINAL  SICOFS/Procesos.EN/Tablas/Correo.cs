using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.EN.Tablas
{
    public class Correo
    {
        public int id { get; set; }
        public string mailFrom { get; set; }
        public string mailTo { get; set; }
        public string mailCC { get; set; }
        public string mailInfra { get; set; }
        public string mailSopo { get; set; }
        public string contromail { get; set; }
    }
}
