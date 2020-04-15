using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Bancos.PS.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ITarjetasCredito" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ITarjetasCredito
    {
        [OperationContract]
        String ServicioTarjetasCredito(String NombreBanco, String IdCuentaBanco, String IdCuentaBancoEpicor, String RutaEntrada,
                                       String RutaSalida, ArrayList CorreosControl, String Remitente,
                                       String CodigoTransito, String NumCuenta, String TipoCuenta,
                                       String Usuario, String RutaProcesado, String TipoProceso);
    }
}
