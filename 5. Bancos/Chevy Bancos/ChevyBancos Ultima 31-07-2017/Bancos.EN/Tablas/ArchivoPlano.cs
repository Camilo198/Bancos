using System;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class ArchivoPlano
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public String pIdCuentaBanco { get; set; }
        public String pTipoProceso { get; set; }
        public int? pLineasExcluidasInicio { get; set; }
        public int? pLineasExcluidasFin { get; set; }
        public bool? pEsExcel { get; set; }
        public int? pNumeroHojaExcel { get; set; }
        public String pNomHoja { get; set; }
        public String pCaracterDecimal { get; set; }


    }
}
