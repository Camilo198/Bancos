using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RutasFtp.EN.Tablas
{
    [Serializable()]
    public class Banco
    {

        public String pOperacion { get; set; }

        public int? pRutaArchivosEntrada { get; set; }
        public int? pRutaArchivosSalida { get; set; }
        public int? pRutaArchivosSalidaEpicor { get; set; }
        public bool? pFtp { get; set; }
        public String pUrlFtp { get; set; }
        public String pUsuarioFtp { get; set; }
        public String pClave { get; set; }
        public String pCorreoControl { get; set; }
        public String pCorreoEnvio { get; set; }
        public bool? pActivo { get; set; }
        public String pRemitente { get; set; }
        public String pTipoProceso { get; set; }
        public int? pId { get; set; }

        public String pNombreCuenta { get; set; }
        public String pCodigoBanco { get; set; }
        public String pNumCuenta { get; set; }
        public String pTipoCuenta { get; set; }
        public String pIdCuentaBanco { get; set; }

    }
}
