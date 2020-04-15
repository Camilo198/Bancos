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

namespace Bancos.PS.Modulos.Banco
{
    public partial class BancoPagosErrados : System.Web.UI.Page
    {

        private List<BancoDtlArchivosProcesados> CamposEstructura
        {
            get
            {
                List<BancoDtlArchivosProcesados> lista = new List<BancoDtlArchivosProcesados>();
                if (ViewState["CamposEstructura"] != null)
                    lista = (List<BancoDtlArchivosProcesados>)ViewState["CamposEstructura"];
                return lista;
            }
            set
            {
                ViewState["CamposEstructura"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlNombreBanco.DataSource = new BancoLN().consultarBancos();
                ddlNombreBanco.DataTextField = "NOMBRE";
                ddlNombreBanco.DataValueField = "CODIGO_BANCO";
                ddlNombreBanco.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlNombreBanco);

                BancoDtlArchivosProcesadosLN objAP = new BancoDtlArchivosProcesadosLN();
                gvTipoLinea.DataSource = objAP.consultarLineasErradas();
                gvTipoLinea.DataBind();
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CamposEstructura = null;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void gvBusquedaBanco_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvBusquedaBanco_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvTipoLinea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BancoDtlArchivosProcesadosLN objAP = new BancoDtlArchivosProcesadosLN();
            gvTipoLinea.PageIndex = e.NewPageIndex;
            gvTipoLinea.DataSource = objAP.consultarLineasErradas();
            gvTipoLinea.DataBind();
        }

        protected void gvTipoLinea_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnCodigoBanco_Click(object sender, EventArgs e)
        {
            gvTipoLinea.DataSource = null;
            gvTipoLinea.DataBind();

            if (this.txbFechaInicial.Text.Length > 0 & this.txbFechaFin.Text.Length > 0)
            {
                if (this.ddlNombreBanco.SelectedIndex == 0)
                {
                    BancoDtlArchivosProcesadosLN objAP = new BancoDtlArchivosProcesadosLN();
                    gvTipoLinea.DataSource = objAP.consultarLineasErradas(DateTime.Parse(this.txbFechaInicial.Text), DateTime.Parse(this.txbFechaFin.Text));
                    gvTipoLinea.DataBind();
                }
                else
                {
                    BancoDtlArchivosProcesadosLN objAP = new BancoDtlArchivosProcesadosLN();
                    gvTipoLinea.DataSource = objAP.consultarLineasErradas(this.ddlNombreBanco.SelectedValue.ToString(), DateTime.Parse(this.txbFechaInicial.Text), DateTime.Parse(this.txbFechaFin.Text));
                    gvTipoLinea.DataBind();
                }
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "Ingrese Fechas, para la busquedad...!", TipoMensaje.Error);
            }
        }

        protected void ddlNombreBanco_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}