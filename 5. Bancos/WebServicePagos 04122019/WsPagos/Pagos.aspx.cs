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
            //Pagina para realizar debug al servicio
            WebServiceBancos.WsBancos prueba = new WebServiceBancos.WsBancos();

            //Descomentariar pruebas
            //string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE020V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_01012018_PO31122017_0000.txt", "S");
            //Comentariar a produccion
            //string resultado = prueba.LecturaPagos("", "", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Fiducia1\\Recaudo\\Bogota\\Recibidos\\", "05003146.OW3", "N");
            string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_15102020_PO14102020_0000.txt", "S");
           // string resultado = prueba.PagosTarjeta("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\Recaudo\ATH\Recibidos\", "ChevyPlan_OCC_20200901_03092020_07_45_10.txt");
        }
        
        public void HandleKeyEvent(Object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (Renci.SshNet.Common.AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = "prueba123";
                }
            }
        }
    }
}