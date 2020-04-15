using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class EstructuraArchivo
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public int? pConfiguracion { get; set; }
        public int? pOrdenColumna { get; set; }
        public int? pLongitud { get; set; }
        public int? pCantidadDecimales { get; set; }
        public bool? pEsContador { get; set; }
        public String pTipoDato { get; set; }
        public String pNombreCampo { get; set; }
        public String pCaracterRelleno { get; set; }
        public String pAlineacion { get; set; }
        public String pFormatoFecha { get; set; }
        public bool? pRequiereCambio { get; set; }
        public bool? pValorPorDefecto { get; set; }
        public String pValor { get; set; }
        


    }
}