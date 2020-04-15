using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Definicion
{
    [Serializable()]
    public class BancoDtlArchivosProcesadosFactDEF
    {
        public const String _NombreTabla = "tb_BAN_DETALLES_ARCHIVOS_PROCESADOS_FACT";

        public const String Oid = "ID";
        public const String TipodeRegistro = "TipodeRegistro";
        public const String ReferenciaP = "ReferenciaP";
        public const String ReferenciaS = "ReferenciaS";
        public const String PeriodoF = "PeriodoF";
        public const String Ciclo = "Ciclo";
        public const String ValorServicioP = "ValorServicioP ";
        public const String CodigoServicioF = "CodigoServicioF";
        public const String ValorServicioA = "ValorServicioA";
        public const String FechaVenc = "FechaVenc";
        public const String CodigoEFR = "CodigoEFR";
        public const String NoCtaClienteRece = "NoCtaClienteRece";
        public const String TipoCtaClienteRece = "TipoCtaClienteRece";
        public const String IdentificacionCliente = "IdentificacionCliente";
        public const String NombreCliente = "NombreCliente";
        public const String CodigoEFO = "CodigoEFO";
        public const String Reservado = "Reservado";
        public const String NombreArchivo = "NombreArchivo";
        public const String FechaP = "FechaP";
        public const String HoraP = "HoraP";
        public const String Procesado = "Procesado";
        public const String NombreArchivoProceso = "NombreArchivoProceso";
        public const String CodBanco = "CodBanco";
        public const String CodError = "CodError";
        public const String DescripcionError = "DescripcionError";
        public const String Corregido = "Corregido";
    }
}