using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bancos.PS.Codigo;
using Bancos.EN;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.EN.Definicion;

using System.Data;


namespace Bancos.PS.Modulos.Interpretaciones
{
    public partial class LogBAN : System.Web.UI.Page
    {

        private DateTime fechaIn
        {
            get
            {
                DateTime valor = DateTime.Now;
                if (ViewState["fechaIn"] != null)
                    valor = (DateTime)ViewState["fechaIn"];
                return valor;
            }
            set
            {
                ViewState["fechaIn"] = value;
            }
        }

        private DateTime fechaFn
        {
            get
            {
                DateTime valor = DateTime.Now;
                if (ViewState["fechaFn"] != null)
                    valor = (DateTime)ViewState["fechaFn"];
                return valor;
            }
            set
            {
                ViewState["fechaFn"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {   
                this.ddlTipoArchivo.DataSource = new ArchivoAsobancariaLN().consultar(new ArchivoAsobancaria());
                this.ddlTipoArchivo.DataTextField = "pNombre";
                this.ddlTipoArchivo.DataValueField = "pOid";
                this.ddlTipoArchivo.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoArchivo);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            fechaIn = Convert.ToDateTime(txbFechaInicial.Text);
            fechaFn = Convert.ToDateTime(txbFechaFin.Text);
            llenarGrillaLogs();
        }

        protected void gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvLogs.PageIndex = e.NewPageIndex;
            llenarGrillaLogs();
        }

        private void llenarGrillaLogs()
        {
            this.gvLogs.DataSource = new LogsLN().consultarLogs(this.ddlTipoArchivo.Text, this.ddlTipoProceso.Text,
                                                              Convert.ToString(fechaIn.ToString("yyyy-MM-dd")),
                                                              Convert.ToString(fechaFn.ToString("yyyy-MM-dd")));
            this.gvLogs.DataBind();
        }

    }
}