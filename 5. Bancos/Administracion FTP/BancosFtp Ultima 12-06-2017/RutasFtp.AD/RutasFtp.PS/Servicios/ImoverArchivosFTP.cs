using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RutasFtp.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ImoverArchivosFTP" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ImoverArchivosFTP
    {
        [OperationContract]
        String trasladarArchivos();
    }
}
