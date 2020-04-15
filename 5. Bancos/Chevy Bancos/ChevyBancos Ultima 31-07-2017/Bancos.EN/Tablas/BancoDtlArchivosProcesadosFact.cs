using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class BancoDtlArchivosProcesadosFact
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public String pTipodeRegistro { get; set; }
        public String pReferenciaP { get; set; }
        public String pReferenciaS { get; set; }
        public String pPeriodoF { get; set; }
        public String pCiclo { get; set; }
        public Decimal pValorServicioP { get; set; }
        public String pCodigoServicioF { get; set; }
        public Decimal pValorServicioA { get; set; }
        public DateTime pFechaVenc { get; set; }
        public String pCodigoEFR { get; set; }
        public String pNoCtaClienteRece { get; set; }
        public String pTipoCtaClienteRece { get; set; }
        public String pIdentificacionCliente { get; set; }
        public String pNombreCliente { get; set; }
        public String pCodigoEFO { get; set; }
        public String pReservado { get; set; }
        public String pNombreArchivo { get; set; }
        public DateTime pFechaP { get; set; }
        public DateTime pHoraP { get; set; }
        public bool? pProcesado { get; set; }
        public String pNombreArchivoProceso { get; set; }
        public String pCodBanco { get; set; }
        public int? pCodError { get; set; }
        public String pDescripcionError { get; set; }
        public bool? pCorregido { get; set; }
    }
}
