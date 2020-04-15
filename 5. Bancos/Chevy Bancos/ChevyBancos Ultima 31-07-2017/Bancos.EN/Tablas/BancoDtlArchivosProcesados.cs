using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Tablas
{
    [Serializable()]
    public class BancoDtlArchivosProcesados
    {
        public String pOperacion { get; set; }

        public int? pOid { get; set; }
        public String pTipoRegistro { get; set; }
        public String pReferenciaPrincipal { get; set; }
        public Decimal pValorRecaudado { get; set; }
        public String pProcedenciaPago { get; set; }
        public String pMediosPago { get; set; }
        public String pNoOperacion { get; set; }
        public String pNoAutorizacion { get; set; }
        public String pCodigoEFD { get; set; }
        public String pCodigoSucursal { get; set; }
        public String pSecuencia { get; set; }
        public String pCausalDevo { get; set; }
        public String pReservado { get; set; }
        public String pNombreArchivo { get; set; }
        public String pFechaP { get; set; }
        public bool? pProcesado { get; set; }
        public String pNombreArchivoProceso { get; set; }
        public String pCodBanco { get; set; }
        public int? pCodError { get; set; }
        public String pDescripcionError { get; set; }
        public bool? pCorregido { get; set; }

        public String pDatafono { get; set; }
        public String pFechaRecaudo { get; set; }
        public String pTipoProceso { get; set; }
    }
}