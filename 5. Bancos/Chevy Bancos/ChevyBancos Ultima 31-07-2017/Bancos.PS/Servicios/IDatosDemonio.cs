using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IDatosDemonio" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IDatosDemonio
    {
        [OperationContract]
        String consultarRutaEntrada(String codigoBanco);

        [OperationContract]
        String consultarRutaSalida(String codigoBanco);

        [OperationContract]
        List<Banco> consultarBancos();       

        [OperationContract]
        List<EstructuraArchivo> consultarEstructuraArchivoAsobancaria(String tipoLinea, String tipoProceso);

        [OperationContract]
        List<EstructuraArchivo> consultarEstructuraArchivoBanco(String codBanco);

        [OperationContract]
        List<Equivalencias> obtenerCausales(String banco);

        [OperationContract]
        List<Equivalencias> obtenerValores(String banco);

        [OperationContract]
        List<EquivalenciasArchivo> obtenerTransformacionArchivo(String banco);

        [OperationContract]
        List<String> obtenerLineasBanco(String banco);
    }

    [DataContract]
    public class Banco
    {
        [DataMember]
        public String aRutaArchivosEntrada { get; set; }

        [DataMember]
        public String aRutaArchivosSalida { get; set; }

        [DataMember]
        public int aIntervaloTiempo { get; set; }

        [DataMember]
        public bool aFtp { get; set; }

        [DataMember]
        public String aCodigoBanco { get; set; }

        [DataMember]
        public String aNombre { get; set; }
        
        [DataMember]
        public String aUrlFtp { get; set; }

        [DataMember]
        public String aUsuarioFtp { get; set; }

        [DataMember]
        public String aClave { get; set; }


        [DataMember]
        public String aCodigoEntidadFinanciera { get; set; }

        [DataMember]
        public String aCorreoControl { get; set; }

        [DataMember]
        public String aCorreoEnvio { get; set; }

        [DataMember]
        public bool aActivo { get; set; }

        [DataMember]
        public bool aRecFac { get; set; }

        [DataMember]
        public String aRemitente { get; set; }

        [DataMember]
        public String aNumeCuenta { get; set; }

        [DataMember]
        public String aNombCuenta { get; set; }

        [DataMember]
        public String aTipoCuenta { get; set; } 
    }

    [DataContract]
    public class EstructuraArchivo
    {
        [DataMember]
        public int aOid { get; set; }

        [DataMember]
        public int aConfiguracion { get; set; }

        [DataMember]
        public int aOrdenColumna { get; set; }

        [DataMember]
        public int aLongitud { get; set; }

        [DataMember]
        public int aCantidadDecimales { get; set; }

        [DataMember]
        public bool aEsContador { get; set; }

        [DataMember]
        public bool aRequiereCambio { get; set; }

        [DataMember]
        public String aTipoDato { get; set; }

        [DataMember]
        public String aNombreCampo { get; set; }

        [DataMember]
        public String aCaracterRelleno { get; set; }

        [DataMember]
        public String aAlineacion { get; set; }

        [DataMember]
        public String aFormatoFecha { get; set; }

        [DataMember]
        public String aTipoLinea { get; set; }

        [DataMember]
        public bool aValorPorDefecto { get; set; }

        [DataMember]
        public String aValor { get; set; }
    }

    [DataContract]
    public class Equivalencias
    {
        [DataMember]
        public String aAsobancaria { get; set; }

        [DataMember]
        public String aBanco { get; set; }
    }

    [DataContract]
    public class EquivalenciasArchivo
    {
        [DataMember]
        public int? aAsobancaria { get; set; }

        [DataMember]
        public int? aBanco { get; set; }
    }
}
