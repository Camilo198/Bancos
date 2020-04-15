using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Bancos.EN;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;

namespace Bancos.PS.Modulos.Banco
{
    public partial class TablasEq : System.Web.UI.Page
    {

        #region "Propiedades: IdTablasEquivalencias, IdCamposEquivalencias, IdTipoArchivo,IdCuentaBanco"

        private int IdTablasEquivalencias
        {
            get
            {
                int valor = 0;
                if (ViewState["IdTablasEquivalencias"] != null)
                    valor = (int)ViewState["IdTablasEquivalencias"];
                return valor;
            }
            set
            {
                ViewState["IdTablasEquivalencias"] = value;
            }
        }

        private int IdCamposEquivalencias
        {
            get
            {
                int valor = 0;
                if (ViewState["IdCamposEquivalencias"] != null)
                    valor = (int)ViewState["IdCamposEquivalencias"];
                return valor;
            }
            set
            {
                ViewState["IdCamposEquivalencias"] = value;
            }
        }

        private String IdTipoArchivo
        {
            get
            {
                String valor = String.Empty;
                if (ViewState["IdTipoArchivo"] != null)
                    valor = (String)ViewState["IdTipoArchivo"];
                return valor;
            }
            set
            {
                ViewState["IdTipoArchivo"] = value;
            }
        }

        private int IdCuentaBanco
        {
            get
            {
                int valor = 0;
                if (ViewState["IdCuentaBanco"] != null)
                    valor = (int)ViewState["IdCuentaBanco"];
                return valor;
            }
            set
            {
                ViewState["IdCuentaBanco"] = value;
            }
        }

        private List<EN.Tablas.Banco> BancosEncontrados
        {
            get
            {
                List<EN.Tablas.Banco> lista = new List<EN.Tablas.Banco>();
                if (ViewState["BancosEncontrados"] != null)
                    lista = (List<EN.Tablas.Banco>)ViewState["BancosEncontrados"];
                return lista;
            }
            set
            {
                ViewState["BancosEncontrados"] = value;
            }
        }

        #endregion

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

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Banco/TablasEquivalencias.aspx?tv=bs%2fcg%2fcgte");
        }

        private void limpiarTodo(bool limpiarBanco)
        {

            BancosEncontrados = null;
            if (limpiarBanco)
            {
                txbCodigoCuenta.Text = txbNombreCuenta.Text = String.Empty;
            }
             txbNombreCuentaB.Text = txbCodigoCuentaB.Text = String.Empty;
           
            chbTieneValor.Checked = false;

            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            gvCampos.DataSource = null;
            gvCampos.DataBind();
            lbEstado.Text = string.Empty;
        }

        protected void ddlTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdTipoArchivo = this.ddlTipoArchivo.SelectedValue;
            llenarGrillaTablas();
            limpiarTodo(true);
           
        }

        protected void imgBtnAddTabla_Click(object sender, ImageClickEventArgs e)
        {
            agregarTabla();
        }

        protected void imgBtnAddField_Click(object sender, ImageClickEventArgs e)
        {
            agregarCampo();
        }

        protected void gvTablas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                IdTablasEquivalencias = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                this.txbNombreTabla.Text = this.lbEditando.Text = HttpUtility.HtmlDecode(this.gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                this.imgBtnAddTabla.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
                this.imgBtnAddField.Enabled = true;
                llenarGrillaCampos();
                this.txbValor.ValidationGroup = this.rfvValor.ValidationGroup = "3";
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                TablasEquivalencias objEA = new TablasEquivalencias();
                objEA.pId = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                int valor = new TablasEquivalenciasLN().borrar(objEA);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(this.lbEstado, "Se eliminó la tabla exitosamente!", TipoMensaje.Informacion);
                    llenarGrillaTablas();
                }
                else
                    UtilidadesWeb.ajustarMensaje(this.lbEstado, "No fue posible eliminar la tabla", TipoMensaje.Error);
            }
        }

        protected void gvTablas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvTablas.PageIndex = e.NewPageIndex;
            llenarGrillaTablas(); 
        }

        protected void gvCampos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                IdCamposEquivalencias = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                this.txbCodigo.Text = HttpUtility.HtmlDecode(this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text);
                this.txbDescripcion.Text = HttpUtility.HtmlDecode(this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);
                this.chbTieneValor.Checked = ((CheckBox)this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Controls[0]).Checked;
                this.txbValor.Text = HttpUtility.HtmlDecode(this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text);
                this.imgBtnAddField.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
                if (this.chbTieneValor.Checked == true)
                    this.txbValor.Enabled = true;
                else
                    this.txbValor.Enabled = false;

            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                CamposEquivalencias objEA = new CamposEquivalencias();
                objEA.pId = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                int valor = new CamposEquivalenciasLN().borrar(objEA);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(this.lbEstado, "Se eliminó el campo exitosamente!", TipoMensaje.Informacion);
                    llenarGrillaCampos();
                }
                else
                    UtilidadesWeb.ajustarMensaje(this.lbEstado, "No fue posible eliminar el campo", TipoMensaje.Error);
            }
        }

        protected void gvCampos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCampos.PageIndex = e.NewPageIndex;
            llenarGrillaCampos();
        }

        protected void chbTieneValor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbTieneValor.Checked == true)
            {
                this.txbValor.Enabled = true;
                this.txbValor.ValidationGroup = this.rfvValor.ValidationGroup = "2";
            }
            else
            {
                this.txbValor.Enabled = false;
                this.txbValor.Text = String.Empty;
                this.txbValor.ValidationGroup = this.rfvValor.ValidationGroup = "3";
            }
        }

        protected void txbCodigoCuenta_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscar(false);
            btnLimpiar.Enabled = true;
        }

        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {
            btnLimpiar.Enabled = false;
            txbNombreCuentaB.Text = txbCodigoCuentaB.Text = String.Empty;
            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            mpeBusquedaBanco.Show();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            btnLimpiar.Enabled = false;
            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            mpeBusquedaBanco.Show();
        }

        protected void gvBusquedaBanco_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("sel"))
            {
                txbCodigoCuenta.Text = HttpUtility.HtmlDecode(gvBusquedaBanco.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text);
                txbNombreCuenta.Text = HttpUtility.HtmlDecode(gvBusquedaBanco.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                buscar(true);
            }
        }

        protected void gvBusquedaBanco_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBusquedaBanco.PageIndex = e.NewPageIndex;
            gvBusquedaBanco.DataSource = BancosEncontrados;
            gvBusquedaBanco.DataBind();

            mpeBusquedaBanco.Show();
        }

        private void buscar(bool esBusqPorTxb)
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();

            if (esBusqPorTxb)
            {
                objB.pIdCuentaBanco = txbCodigoCuenta.Text.Trim();
                objB.pTipoProceso = this.ddlTipoArchivo.SelectedValue;
                List<EN.Tablas.Banco> listaB = objBancoLN.consultar(objB);
                if (listaB.Count > 0)
                {
                    objB = listaB[0];
                    txbNombreCuenta.Text = objB.pNombreCuenta;
                    IdCuentaBanco = objB.pId.Value;
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la cuenta bancaria " + objB.pNombreCuenta + ".", TipoMensaje.Informacion);
                    //limpiarTodo(false);
                    llenarGrillaTablas();
                }
                else
                {
                    IdCuentaBanco = 0;
                    txbNombreCuenta.Text = String.Empty;
                    llenarGrillaTablas();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Favor crear primero la cuenta bancaria con código: " + this.txbCodigoCuenta.Text + ".", TipoMensaje.Error);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txbCodigoCuentaB.Text.Trim()))
                    objB.pIdCuentaBanco = txbCodigoCuentaB.Text.Trim();

                if (!String.IsNullOrEmpty(txbNombreCuentaB.Text.Trim()))
                    objB.pNombreCuenta = txbNombreCuentaB.Text.Trim();

                objB.pTipoProceso = this.ddlTipoArchivo.SelectedValue;

                BancosEncontrados = objBancoLN.consultar(objB);
                gvBusquedaBanco.DataSource = BancosEncontrados;
                gvBusquedaBanco.DataBind();

                mpeBusquedaBanco.Show();
            }
        }

        private void agregarTabla()
        {
            TablasEquivalencias objEntidad = new TablasEquivalencias();
            objEntidad.pNombre = this.txbNombreTabla.Text.Trim();
            objEntidad.pTipoArchivo = this.ddlTipoArchivo.SelectedValue;
            objEntidad.pIdCuentaBanco = IdCuentaBanco;
            int valor = 0;
            TablasEquivalenciasLN objEA = new TablasEquivalenciasLN();
            String add = "actualizado";
            if (IdTablasEquivalencias > 0)
            {
                objEntidad.pId = IdTablasEquivalencias;
                valor = objEA.actualizar(objEntidad);
            }
            else
            {
                valor = objEA.insertar(objEntidad);
                add = "agregado";
            }

            if (valor > 0)
            {
                UtilidadesWeb.ajustarMensaje(this.lbEstado, "Se ha " + add + " la tabla " + objEntidad.pNombre + ".", TipoMensaje.Informacion);
                llenarGrillaTablas();
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(this.lbEstado, "No se ha " + add + " la tabla " + objEntidad.pNombre + ".", TipoMensaje.Error);
            }
        }

        private void agregarCampo()
        {
            CamposEquivalencias objEntidad = new CamposEquivalencias();
            objEntidad.pTablasEquivalencias = IdTablasEquivalencias;
            objEntidad.pCodigo = this.txbCodigo.Text.Trim();
            objEntidad.pDescripcion = this.txbDescripcion.Text.Trim();
            objEntidad.pValor = this.txbValor.Text.Trim();
            objEntidad.pValorPorDefecto = this.chbTieneValor.Checked;

            int valor = 0;
            CamposEquivalenciasLN objEA = new CamposEquivalenciasLN();
            String add = "actualizado";
            if (IdCamposEquivalencias > 0)
            {
                objEntidad.pId = IdCamposEquivalencias;
                valor = objEA.actualizar(objEntidad);
            }
            else
            {
                valor = objEA.insertar(objEntidad);
                add = "agregado";
            }

            if (valor > 0)
            {
                UtilidadesWeb.ajustarMensaje(this.lbEstado, "Se ha " + add + " el campo " + objEntidad.pDescripcion + ".", TipoMensaje.Informacion);
                llenarGrillaCampos();
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(this.lbEstado, "No se ha " + add + " el campo " + objEntidad.pDescripcion + ".", TipoMensaje.Error);
            }
        }

        private void llenarGrillaTablas()
        {
            TablasEquivalencias objEntidad = new TablasEquivalencias();
            objEntidad.pTipoArchivo = IdTipoArchivo;
            objEntidad.pIdCuentaBanco = IdCuentaBanco;
            this.gvTablas.DataSource = new TablasEquivalenciasLN().consultar(objEntidad);
            this.gvTablas.DataBind();
            this.txbNombreTabla.Text = String.Empty;
            this.lbEditando.Text = "NO HAY TABLA SELECCIONADA";
            this.imgBtnAddTabla.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
            IdTablasEquivalencias = 0;
            LimpiarCampos();
            this.imgBtnAddField.Enabled = false;
            this.gvCampos.DataSource = null;
            this.gvCampos.DataBind();
        }

        private void llenarGrillaCampos()
        {
            CamposEquivalencias objEntidad = new CamposEquivalencias();
            objEntidad.pTablasEquivalencias = IdTablasEquivalencias;
            this.gvCampos.DataSource = new CamposEquivalenciasLN().consultar(objEntidad);
            this.gvCampos.DataBind();
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            IdCamposEquivalencias = 0;
            this.txbCodigo.Text = this.txbDescripcion.Text = this.txbValor.Text = String.Empty;
            this.txbValor.Enabled = false;
            this.chbTieneValor.Checked = false;
            this.imgBtnAddField.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
        }

    }
}