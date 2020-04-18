using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.EN.Tablas
{
    [Serializable()]
    public class ObjetoTablas
    {
        //TABLA PAGOS
        public String pCodBanco { get; set; }
        public String pFecPago { get; set; }
        public String pFecPagoBancos { get; set; }
        public String pValPago { get; set; }
        public String pForPago { get; set; }
        public String pContrato { get; set; }
        public String pEstado { get; set; }
        public String pUsuProceso { get; set; }
        public String pFecProceso { get; set; }
        public String pContAnterior { get; set; }
        public Int32? pValComision { get; set; }
        public Int32? pValRetFuente { get; set; }
        public Int32? pValRetIva { get; set; }
        public Int32? pValRetIca { get; set; }
        public String pNumAutorizacion { get; set; }
        public String pHoraPago { get; set; }
        public String pNumLote { get; set; }
        public String pLegalizado { get; set; }

        //TABLA PAGOS INCONSISTENTES
        public String pPagOficina { get; set; }
        public String pNomArchivo { get; set; }

        //TABLA CIERRE
        public String pidtitular { get; set; }

        //TABLA BANCOS
        public String pNumCuenta { get; set; }

        //TABLA BANCOS PAGOS
        public String pParteFija { get; set; }
        public String pNombreBanco { get; set; }
        public String pParteInicial { get; set; }
        public String pParteFinal { get; set; }

        //TABLA PAGOS TARJETA CREDITO
        public Int32? pCodUnico { get; set; }
        public String pFranquicia { get; set; }
        public String pCuenta { get; set; }
        public String pFecVale { get; set; }
        public String pFecAbono { get; set; }
        public String pNumTarjeta { get; set; }
        public String pHora { get; set; }
        public String pComprobante { get; set; }
        public String pTerminal { get; set; }
        public Int32? pValVenta { get; set; }
        public Int32? pValIva { get; set; }
        public Int32? pValPropina { get; set; }
        public Int32? pValTotal { get; set; }
        public Int32? pValAbono { get; set; }
        public String pTipTarjeta { get; set; }
        public String pPlazo { get; set; }
        public String pPorComision { get; set; }

        //PARAMETROS CONTRATOS FIDUCIA 3
        public String pNombre { get; set; }
        public String pArea { get; set; }
        public String pParametro { get; set; }
        public String pValor { get; set; }
        public String pVigDesde { get; set; }
        public String pVigHasta { get; set; }
        public String pUsuProcesoPara { get; set; }
        public String pFecProcesoPara { get; set; }
        public String pVigente { get; set; }

        //TABLA PAGOS ERRADOS
        public String pProcesoErr { get; set; }
        public String pReferenciaErr { get; set; }
        public String pCodBancoErr { get; set; }
        public String pValorPagoErr { get; set; }
        public String pFechaErr { get; set; }
        public String pForPagoErr { get; set; }
        public String pUsuarioProcesoErr { get; set; }
        public String pReferenciaPago { get; set; }
    }
}
