using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IPagosOnline" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IPagosOnline
    {
        [OperationContract]
        String ServicioPagosOnline(String NombreBanco, String IdCuentaBancoEpicor, String ArchivoSalidaPagosOnline,
                                   ArrayList CorreosControl, String CodigoTransito, String NumCuenta,
                                          String TipoCuenta, String Remitente, String Usuario, String TipoProceso);
    }   
}
