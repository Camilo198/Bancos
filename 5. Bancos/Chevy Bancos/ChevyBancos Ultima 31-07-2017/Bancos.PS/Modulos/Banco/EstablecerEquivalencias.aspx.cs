using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;
using Bancos.EN;
using Bancos.EN.Definicion;

namespace Bancos.PS.Modulos.Banco
{
    public partial class EstablecerEquivalencias : System.Web.UI.Page
    {

        #region "Propiedades: IdTipoArchivo,IdTablasEquivalencias,IdConfiguracion,IdEquivalencias,CamposEquivalencias,IdCuentaBanco"

        private DataTable CamposEquivalencias
        {
            get
            {
                DataTable lista = new DataTable();
                if (ViewState["Equivalencias"] != null)
                    lista = (DataTable)ViewState["Equivalencias"];
                return lista;
            }
            set
            {
                ViewState["Equivalencias"] = value;
            }
        }
        //ENTERO QUE REPRESENTA EL ID DE LA TABLA TIPO_ARCHIVO
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
        //ENTERO QUE REPRESENTA EL ID DE LA TABLA TABLAS_EQUIVALENCIAS
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
        //ENTERO QUE REPRESENTA EL CAMPO CONFIGURACION DE LA TABLA ESTRUCTURA_ARCHIVO
        private int IdConfiguracion
        {
            get
            {
                int valor = 0;
                if (ViewState["IdConfiguracion"] != null)
                    valor = (int)ViewState["IdConfiguracion"];
                return valor;
            }
            set
            {
                ViewState["IdConfiguracion"] = value;
            }
        }
        //ENTERO QUE REPRESENTA EL ID DE LAS TABLA EQUIVALENCIAS
        private int IdEquivalencias
        {
            get
            {
                int valor = 0;
                if (ViewState["IdEquivalencias"] != null)
                    valor = (int)ViewState["IdEquivalencias"];
                return valor;
            }
            set
            {
                ViewState["IdEquivalencias"] = value;
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
            Response.Redirect("~/Modulos/Banco/EstablecerEquivalencias.aspx?tv=bs%2fcg%2fcgee");            
        }

        private void limpiarlineastablas()
        {
            this.ddlTipoLinea.Items.Clear();
            this.ddlTablas.Items.Clear();
            this.ddlTipoLinea.Enabled = this.ddlTablas.Enabled = false;
           
            this.gvEquivalencias.DataSource = null;
            this.gvEquivalencias.DataBind();
            this.imgAgregarEquivalencia.ImageUrl = "~/MarcaVisual/iconos/agregar.png";

            this.ddlCamposLineas.Items.Clear();
            this.ddlCamposTablas.Items.Clear();
            this.ddlCamposLineas.Enabled = this.ddlCamposTablas.Enabled = false;
        }

        protected void ddlTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {

            limpiarlineastablas();
            limpiarTodo(true);
            IdTipoArchivo = this.ddlTipoArchivo.SelectedValue;
          
        }

        private void limpiarTodo(bool limpiarBanco)
        {

            BancosEncontrados = null;
            if (limpiarBanco)
            {
                txbCodigoCuenta.Text = txbNombreCuenta.Text = String.Empty;
            }
            txbNombreCuentaB.Text = txbCodigoCuentaB.Text = String.Empty;
            
            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            lbEstado.Text = string.Empty;
        }

        private void llenarLineasTablas()
        {
            ArchivoPlanoLN objAP = new ArchivoPlanoLN();
            ddlTipoLinea.DataSource = objAP.consultarLineasCuentaBanco(txbCodigoCuenta.Text.Trim(), this.ddlTipoArchivo.SelectedValue);
            ddlTipoLinea.DataTextField = TipoLineaDEF.Nombre;
            ddlTipoLinea.DataValueField = TipoLineaDEF.Oid;
            ddlTipoLinea.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(ddlTipoLinea);

            TablasEquivalencias objEntidad = new TablasEquivalencias();
            objEntidad.pTipoArchivo = IdTipoArchivo;
            objEntidad.pIdCuentaBanco = IdCuentaBanco;
            this.ddlTablas.DataSource = new TablasEquivalenciasLN().consultar(objEntidad);
            this.ddlTablas.DataTextField = "pNombre";
            this.ddlTablas.DataValueField = "pId";
            this.ddlTablas.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(this.ddlTablas);
        }

        protected void ddlTipoLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlTipoLinea.SelectedIndex == 0)
            {
                this.ddlTablas.SelectedIndex = 0;

                this.gvEquivalencias.DataSource = null;
                this.gvEquivalencias.DataBind();
                this.imgAgregarEquivalencia.ImageUrl = "~/MarcaVisual/iconos/agregar.png";

                this.ddlCamposLineas.Items.Clear();
                this.ddlCamposTablas.Items.Clear();
                this.ddlCamposLineas.Enabled = this.ddlCamposTablas.Enabled = false;
            }
            else
            {
                this.ddlCamposLineas.Enabled = true;
                IdConfiguracion = Convert.ToInt32(this.ddlTipoLinea.SelectedValue);
                EstructuraArchivo objEA = new EstructuraArchivo();
                objEA.pConfiguracion = IdConfiguracion;
                objEA.pRequiereCambio = true;
                this.ddlCamposLineas.Items.Clear();
                this.ddlCamposLineas.DataSource = new EstructuraArchivoLN().consultar(objEA);
                this.ddlCamposLineas.DataTextField = "pNombreCampo";
                this.ddlCamposLineas.DataValueField = "pOid";
                this.ddlCamposLineas.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlCamposLineas);
                llenarGrillaEquivalencias();
            }
        }

        private void llenarGrillaEquivalencias()
        {
            this.gvEquivalencias.DataSource = CamposEquivalencias = new EquivalenciasLN().consultarEquivalencias(IdTipoArchivo, Convert.ToString(IdConfiguracion),this.txbCodigoCuenta.Text);
            this.gvEquivalencias.DataBind();
        }

        protected void ddlTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlTablas.SelectedIndex == 0)
            {
                this.ddlCamposTablas.Enabled = false;
                this.ddlCamposTablas.Items.Clear();
            }
            else
            {
                llenarDDLCamposTabla();
            }
        }

        private void llenarDDLCamposTabla()
        {
            this.ddlCamposTablas.Enabled = true;
            IdTablasEquivalencias = Convert.ToInt32(this.ddlTablas.SelectedValue);
            CamposEquivalencias objCE = new CamposEquivalencias();
            objCE.pTablasEquivalencias = IdTablasEquivalencias;
            this.ddlCamposTablas.Items.Clear();
            this.ddlCamposTablas.DataSource = new CamposEquivalenciasLN().consultar(objCE);
            this.ddlCamposTablas.DataTextField = "pDescripcion";
            this.ddlCamposTablas.DataValueField = "pId";
            this.ddlCamposTablas.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(this.ddlCamposTablas);
        }

        protected void gvEquivalencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {

                IdEquivalencias = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                this.ddlCamposLineas.SelectedValue = HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                if (HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text).Trim().Length == 0)
                    this.ddlTablas.SelectedValue = "0";
                else
                    this.ddlTablas.SelectedValue = HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);

                llenarDDLCamposTabla();

                if (HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text).Trim().Length == 0)
                    this.ddlCamposTablas.SelectedValue = "0";
                else
                    this.ddlCamposTablas.SelectedValue = HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text);

                this.imgAgregarEquivalencia.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";

            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                Equivalencias objEA = new Equivalencias();
                objEA.pId = Convert.ToInt32(HttpUtility.HtmlDecode(this.gvEquivalencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                int valor = new EquivalenciasLN().borrar(objEA);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(this.lbEstado, "Se eliminó la equivalencia del campo exitosamente!", TipoMensaje.Informacion);
                    limpiarCampos();
                    llenarGrillaEquivalencias();
                }
                else
                    UtilidadesWeb.ajustarMensaje(this.lbEstado, "No fue posible eliminar la equivalencia del campo", TipoMensaje.Error);
            }
        }

        private void limpiarCampos()
        {
            IdEquivalencias = 0;

            this.ddlTablas.SelectedIndex = this.ddlCamposLineas.SelectedIndex = 0;
            this.ddlCamposTablas.Items.Clear();
            this.ddlCamposTablas.Enabled = false;
            this.imgAgregarEquivalencia.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
        }

        protected void gvEquivalencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvEquivalencias.PageIndex = e.NewPageIndex;
            this.gvEquivalencias.DataSource = CamposEquivalencias;
            this.gvEquivalencias.DataBind();
        }

        protected void imgAgregarEquivalencia_Click(object sender, ImageClickEventArgs e)
        {
            agregarEquivalencia();
        }

        private void agregarEquivalencia()
        {
            Equivalencias objEq = new Equivalencias();
            objEq.pIdEstructuraArchivo = Convert.ToInt32(this.ddlCamposLineas.SelectedValue);
            objEq.pIdTablasEquivalencias = Convert.ToInt32(this.ddlTablas.SelectedValue);
            objEq.pIdCamposEquivalencias = Convert.ToInt32(this.ddlCamposTablas.SelectedValue);

            int valor = 0;
            EquivalenciasLN objEA = new EquivalenciasLN();
            String add = "actualizado";
            if (IdEquivalencias > 0)
            {
                objEq.pId = IdEquivalencias;
                valor = objEA.actualizar(objEq);
            }
            else
            {
                valor = objEA.insertar(objEq);
                add = "agregado";
            }

            if (valor > 0)
            {
                UtilidadesWeb.ajustarMensaje(this.lbEstado, "Se ha " + add + " la equivalencia del campo : " + this.ddlCamposLineas.SelectedItem.Text + ".", TipoMensaje.Informacion);
                limpiarCampos();
                llenarGrillaEquivalencias();
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(this.lbEstado, "No se ha " + add + " la equivalencia del campo : " + this.ddlCamposLineas.SelectedItem.Text + ".", TipoMensaje.Error);
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

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
            limpiarlineastablas();

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
                    llenarLineasTablas();
                    this.ddlTipoLinea.Enabled = true;
                    this.ddlTablas.Enabled = true;
                }
                else
                {
                    IdCuentaBanco = 0;
                    txbNombreCuenta.Text = String.Empty;
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

    }
}