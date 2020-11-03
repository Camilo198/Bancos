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
            ////msg = tc.ServicioTarjetasCredito("TCR", "14", "00015",
            ////    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Bancolombia\\Recibidos\\",
            ////    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Bancolombia\\Procesados\\",
            ////    arrMail, "steven.aguilar@chevyplan.com.co",
            ////    "00", "45", "00", "TAREA PROGRAMADA",
            ////    "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\TC\\Bancolombia\\Historico\\",
            ////    "TCR1");
            Recaudo rec = new Recaudo();
            msg = rec.ServicioRecaudoDiario("Lugar Pago 19", "47", "00010",
                "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\Davivienda\\Parciales\\",
                "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\Davivienda\\Recibidos\\",
                arrMail, "steven.aguilar@chevyplan.com.co", "00", "006700175570", "00", "TAREA PROGRAMADA",
                "\\\\sbogche016v\\ArchPlanos\\PAGOS\\Fiducia1\\Recaudo\\Davivienda\\SICO\\", "ABR1");

        }
    }
}