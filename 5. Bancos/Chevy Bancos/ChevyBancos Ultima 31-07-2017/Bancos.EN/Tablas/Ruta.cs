﻿using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class Ruta
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public String pRuta { get; set; }
    }
}
