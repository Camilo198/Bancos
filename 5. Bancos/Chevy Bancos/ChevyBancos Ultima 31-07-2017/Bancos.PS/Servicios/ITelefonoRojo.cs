using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ITelefonoRojo" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ITelefonoRojo
    {
        [OperationContract]
        String ServicioTelefonoRojo(String NombreBanco, String IdCuentaBancoEpicor, String ArchivoSalidaAsobancaria,
                      bool EsFTP, String UrlFTP, String UsuarioFTP, String ClaveFTP, ArrayList CorreosControl,
                      ArrayList CorreosEnvio, String Remitente, String Usuario, String TipoProceso);
    }
}
