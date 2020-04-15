using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN
{
    public class TiposConsultas
    {
        private TiposConsultas() { }

        public const String ACTUALIZAR = "A";
        public const String CONSULTAR = "C";
        public const String CONSULTAR_2 = "C2";
        public const String CONSULTA_COMPUESTA = "CC";
        public const String INSERTAR = "I";
        public const String INSERTAR_2 = "I2";
        public const String ELIMINAR = "E";
    }
}
