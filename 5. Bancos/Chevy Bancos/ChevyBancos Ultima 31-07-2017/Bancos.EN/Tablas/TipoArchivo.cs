﻿using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class TipoArchivo
    {
        public String pOperacion { get; set; }

        public String pNombre { get; set; }
        public String pOid { get; set; }
    }
}