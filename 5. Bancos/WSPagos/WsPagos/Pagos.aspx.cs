using Pagos.LN;
using Pagos.LN.Consulta;
using Renci.SshNet;
using SSH;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            ////Pagina para realizar debug al servicio
            //WebServiceBancos.WsBancos prueba = new WebServiceBancos.WsBancos();

            //string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", @"00015_1903202E_PO17032021_1419.txt", "S");
            //string resultado = prueba.PagosTarjeta("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\TC\Davivienda\Procesados\", @"L395DCHEVYPDIACJHC03A0287.txt");
            //String mens;
            //WcfUtilidades util = new WcfUtilidades();
            //util.LeerFicheroFTP("R066210303093909.txt", "chevyplan\\usaftppagos", "Chevy789", out mens);
        }
    }
}