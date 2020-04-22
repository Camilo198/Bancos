using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceBancos
{
    public partial class Pruebas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Pagina para realizar debug al servicio
            WebServiceBancos.WsBancos prueba = new WebServiceBancos.WsBancos();

            //Descomentariar
           // string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE020V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_13022020_PO12022020_0000.txt", "S");
            //Comentariar a produccion
            string resultado = prueba.LecturaPagos("", "", "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\PagosOnline\\Recibidos\\", "00015_21042020_PO01042020_0918.txt", "S");

           // string resultado = prueba.PagosTarjeta("", "", @"\\sbogche016v\ArchPlanos\Pagos\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_16042020_PO01042020_1034.txt");
        }
    }
}