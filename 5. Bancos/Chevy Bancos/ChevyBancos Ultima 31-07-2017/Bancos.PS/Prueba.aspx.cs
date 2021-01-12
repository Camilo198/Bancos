using Bancos.PS.Servicios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bancos.PS
{
    public partial class Prueba : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ArrayList arrMail = new ArrayList();
            TarjetasCredito tc = new TarjetasCredito();
            string msg;
            // Tarjetas
            ////// AMEX NUEVA
            //msg = tc.ServicioTarjetasCredito("TCR", "14", "00015",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Bancolombia\\Recibidos\\",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Bancolombia\\Procesados\\",
            //    arrMail, "steven.aguilar@chevyplan.com.co",
            //    "00", "45", "00", "TAREA PROGRAMADA",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Bancolombia\\Historico\\",
            //    "TCR1");

            //  msg = tc.ServicioTarjetasCredito("TCR", "60", "00013",
            //"\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Occidente\\Recibidos\\",
            //"\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Occidente\\Procesados\\",
            //arrMail, "steven.aguilar@chevyplan.com.co",
            //"00", "304", "00", "TAREA PROGRAMADA",
            //"\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Occidente\\Historico\\",
            //"TCR1");
            //Recaudo rec = new Recaudo();
            // GNB  Parciales
            //msg = rec.ServicioRecaudoDiario("Lugar Pago 23", "50", "00022",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\GNB\\Parciales\\",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\GNB\\Recibidos\\",
            //    arrMail, "steven.aguilar@chevyplan.com.co", "00", "1", "00", "TAREA PROGRAMADA",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\GNB\\SICO\\", "ABR1");
            // Dvivienda parciales
            //msg = rec.ServicioRecaudoDiario("Lugar Pago 19", "47", "00010",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\Davivienda\\Parciales\\",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\Davivienda\\Recibidos\\",
            //    arrMail, "steven.aguilar@chevyplan.com.co", "00", "006700175570", "00", "TAREA PROGRAMADA",
            //    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\Davivienda\\SICO\\", "ABR1");

            //PagosOnline pol = new PagosOnline();
            //msg = pol.ServicioPagosOnline("Lugar Pago 66", "00015", "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\PagosOnline\\Recibidos\\", arrMail, "00", "016525785990", "00", "steven.aguilar@chevyplan.com.co", "TAREA PROGRAMADA", "POL1");

            ////// TARJETA CODENSA NUEVA
            msg = tc.ServicioTarjetasCredito("TCR", "64", "00031",
                "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Codensa\\Recibidos\\",
                "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Codensa\\Procesados\\",
                //"D:\\Codensa\\Recibidos\\",
                //"D:\\Codensa\\Procesados\\",
                arrMail, "steven.aguilar@chevyplan.com.co",
                "00", "95", "00", "TAREA PROGRAMADA",
                //"D:\\Codensa\\Historico\\",
                "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Codensa\\Historico\\",
                "TCR1");

            //// TARJETA ALKOSTO REVISION
            //msg = tc.ServicioTarjetasCredito("TCR", "63", "00026",
            //    "D:\\Alkosto\\Recibidos\\",
            //    "D:\\Alkosto\\Procesados\\",
            //    //"\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Alkosto\\Recibidos\\",
            //    //"\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Alkosto\\Procesados\\",
            //    arrMail, "steven.aguilar@chevyplan.com.co",
            //    "00", "0", "00", "TAREA PROGRAMADA",
            //    "D:\\Alkosto\\Historico\\",
            //    //"\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Alkosto\\Historico\\",
            //    "TCR1");

        }
        }
}