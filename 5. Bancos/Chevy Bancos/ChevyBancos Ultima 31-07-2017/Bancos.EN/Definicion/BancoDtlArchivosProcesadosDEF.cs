using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bancos.EN.Definicion
{
    [Serializable()]
    public class BancoDtlArchivosProcesadosDEF
    {
        public const String _NombreTabla = "tb_BAN_DETALLES_ARCHIVOS_PROCESADOS";

        public const String Oid = "ID";
        public const String TipoRegistro = "TipoRegistro";
        public const String ReferenciaPrincipal = "ReferenciaPrincipal";
        public const String ValorRecaudado = "ValorRecaudado";
        public const String ProcedenciaPago = "ProcedenciaPago";
        public const String MediosPago = "MediosPago";
        public const String NoOperacion = "NoOperacion";
        public const String NoAutorizacion = "NoAutorizacion";
        public const String CodigoEFD = "CodigoEFD";
        public const String CodigoSucursal = "CodigoSucursal";
        public const String Secuencia = "Secuencia";
        public const String CausalDevo = "CausalDevo";
        public const String Reservado = "Reservado";
        public const String NombreArchivo = "NombreArchivo";
        public const String FechaP = "FechaP";
        public const String Procesado = "Procesado";
        public const String NombreArchivoProceso = "NombreArchivoProceso";
        public const String CodBanco = "CodBanco";
        public const String CodError = "CodError";
        public const String DescripcionError = "DescripcionError";
        public const String Corregido = "Corregido";

        public const String Datafono = "Datafono";
        public const String FechaRecaudo = "FechaRecaudo";
        public const String TipoProceso = "TipoProceso";
    }
}