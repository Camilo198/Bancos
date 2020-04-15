using System;


namespace RutasFtp.EN.Tablas
{
    [Serializable()]

    public class Ftp
    {
        public String pOperacion { get; set; }

        public int? pId { get; set; }
        public String pUrlFtp { get; set; }
        public String pUsuarioFtp { get; set; }
        public String pClaveFtp { get; set; }
        public String pRutaDestino { get; set; }
        public String pIdCuentaBanco { get; set; }
        public String pTipoProceso { get; set; }

        public String pPrefijo { get; set; }
        public String pFormato { get; set; }
        public String pFechaUltimoIngreso { get; set; }
        public String pFechaUltimaCopia { get; set; }
        public bool? pFtpSeguro { get; set; }
    }
}
