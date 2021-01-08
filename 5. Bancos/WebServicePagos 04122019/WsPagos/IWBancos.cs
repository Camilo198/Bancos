using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebServiceBancos
{
    [ServiceContract]
    public interface IWBancos
    {

        [OperationContract]
        string LecturaPagos(string usuario, string pasword, string RutaArchivo, string NombreArchivo, string PagosOnline);

        [OperationContract]
        string PagosTarjeta(string usuario, string password, string RutaArchivo, string NombreArchivo);

        [OperationContract]
        string ProcesarPagosSinSubir(string usuario, string password);

    }

}
