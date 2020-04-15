using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;
using Bancos.EN;
using Bancos.EN.Definicion;

namespace Bancos.PS.Modulos.Banco
{
    public partial class ArchivoBanco : System.Web.UI.Page
    {
        #region "Propiedades: CamposEstructura, BancosEncontrados, OidArchivo, OidConfiguracion, OidEstructuraArchivo"
        private List<EstructuraArchivo> CamposEstructura
        {
            get
            {
                List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
                if (ViewState["CamposEstructura"] != null)
                    lista = (List<EstructuraArchivo>)ViewState["CamposEstructura"];
                return lista;
            }
            set
            {
                ViewState["CamposEstructura"] = value;
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

        private int OidArchivo
        {
            get
            {
                int valor = 0;
                if (ViewState["OidArchivo"] != null)
                    valor = (int)ViewState["OidArchivo"];
                return valor;
            }
            set
            {
                ViewState["OidArchivo"] = value;
            }
        }

        private int OidConfiguracion
        {
            get
            {
                int valor = 0;
                if (ViewState["OidConfiguracion"] != null)
                    valor = (int)ViewState["OidConfiguracion"];
                return valor;
            }
            set
            {
                ViewState["OidConfiguracion"] = value;
            }
        }

        private int OidEstructuraArchivo
        {
            get
            {
                int valor = 0;
                if (ViewState["OidEstructuraArchivo"] != null)
                    valor = (int)ViewState["OidEstructuraArchivo"];
                return valor;
            }
            set
            {
                ViewState["OidEstructuraArchivo"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlTipoDato.DataSource = new TipoDatoLN().consultar(new TipoDato());
                ddlTipoDato.DataTextField = "pTipoDato";
                ddlTipoDato.DataValueField = "pOid";
                ddlTipoDato.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlTipoDato);

                ddlTipoLinea.DataSource = new TipoLineaLN().consultar(new TipoLinea());
                ddlTipoLinea.DataTextField = "pNombre";
                ddlTipoLinea.DataValueField = "pOid";
                ddlTipoLinea.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlTipoLinea);

                this.ddlTipoProceso.DataSource = new ArchivoAsobancariaLN().consultar(new ArchivoAsobancaria());
                this.ddlTipoProceso.DataTextField = "pNombre";
                this.ddlTipoProceso.DataValueField = "pOid";
                this.ddlTipoProceso.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoProceso);
            }

        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Banco/ArchivoBanco.aspx?tv=bs%2fcg%2fab");
        }

        protected void txbCodigoCuenta_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        protected void ddlTipoDato_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarTipoDato();
        }

        protected void imgBtnAgregarCampo_Click(object sender, ImageClickEventArgs e)
        {
            agregarCampo();
        }

        protected void gvCampos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCampos.PageIndex = e.NewPageIndex;
            gvCampos.DataSource = CamposEstructura;
            gvCampos.DataBind();
        }

        protected void gvCampos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OidEstructuraArchivo = Convert.ToInt32(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                ddlTipoDato.SelectedValue = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[8].Text);
                ddlAlineacion.SelectedValue = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[7].Text);
                txbOrdenColumna.Text = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                txbNombreCampo.Text = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text);
                txbLongitud.Text = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);
                ddlCaracterRelleno.SelectedValue = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text);
                txbCantidadDecimales.Text = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[9].Text);
                if (HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[10].Text).Equals(" "))
                    ddlFormatoFecha.SelectedIndex = 0;
                else
                    ddlFormatoFecha.SelectedValue = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[10].Text);
                chbEsContador.Checked = ((CheckBox)gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[11].Controls[0]).Checked;
                chbRequiereCambio.Checked = ((CheckBox)gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[12].Controls[0]).Checked;
                chbTieneValor.Checked = ((CheckBox)gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[13].Controls[0]).Checked;
                txbValor.Text = HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[14].Text);

                imgBtnAgregarCampo.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
                validarTipoDato();
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                EstructuraArchivo objEA = new EstructuraArchivo();
                objEA.pOid = Convert.ToInt32(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                int valor = new EstructuraArchivoLN().borrar(objEA);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se eliminó el campo exitosamente!", TipoMensaje.Informacion);
                    llenarGrillaCampos();
                }
                else
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible eliminar el campo, es posible que tenga traducciones asociadas!!!", TipoMensaje.Error);
            }
        }

        protected void gvTipoLinea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTipoLinea.PageIndex = e.NewPageIndex;
            gvTipoLinea.DataSource = new ArchivoPlanoLN().consultarLineasCuentaBanco(txbCodigoCuenta.Text.Trim(), this.ddlTipoProceso.SelectedValue);
            gvTipoLinea.DataBind();
        }

        protected void gvTipoLinea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OidConfiguracion = Convert.ToInt32(HttpUtility.HtmlDecode(gvTipoLinea.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                lbTLEditando.Text = HttpUtility.HtmlDecode(gvTipoLinea.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);
                llenarGrillaCampos();
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                Configuracion objCfg = new Configuracion();
                objCfg.pOid = Convert.ToInt32(HttpUtility.HtmlDecode(gvTipoLinea.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                int valor = new ConfiguracionLN().borrar(objCfg);

                ArchivoPlano objApl = new ArchivoPlano();
                objApl.pOid = Convert.ToInt32(HttpUtility.HtmlDecode(gvTipoLinea.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text));
                int valor1 = new ArchivoPlanoLN().borrar(objApl);

                if (valor == 0)
                {
                    llenarLineasDisponible();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se eliminó la línea exitosamente!", TipoMensaje.Informacion);
                }
                else
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible eliminar la línea, puede que tenga campos asociados!!!", TipoMensaje.Error);
            }
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

        protected void gvBusquedaBanco_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBusquedaBanco.PageIndex = e.NewPageIndex;
            gvBusquedaBanco.DataSource = BancosEncontrados;
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

        protected void imgAgregarTipoLinea_Click(object sender, ImageClickEventArgs e)
        {
            agregarTipoLinea();
        }

        private void agregarTipoLinea()
        {
            if (!String.IsNullOrEmpty(txbNombreCuenta.Text.Trim()))
            {
                int valor = 0;
                if (OidArchivo > 0)
                {
                    valor = insertarConfiguracion();
                }
                else
                {
                    ArchivoPlano objAP = new ArchivoPlano();
                    objAP.pIdCuentaBanco = txbCodigoCuenta.Text.Trim();
                    objAP.pTipoProceso = ddlTipoProceso.SelectedValue;
                    objAP.pEsExcel = rbExcel.Checked;
                    objAP.pNumeroHojaExcel = Convert.ToInt32(txbNumExcel.Text);
                    objAP.pLineasExcluidasInicio = Convert.ToInt32(txbNExluidasInicio.Text);
                    objAP.pLineasExcluidasFin = Convert.ToInt32(txbNExluidasFinal.Text);
                    OidArchivo = valor = new ArchivoPlanoLN().insertar(objAP);

                    valor = insertarConfiguracion();
                }

                if (valor > 0 && OidArchivo > 0)
                {
                    llenarLineasDisponible();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se registro la linea exitosamente!", TipoMensaje.Informacion);
                }
                else
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible registrar la linea, quiza la cuenta del banco no esta registrado!", TipoMensaje.Error);
                }
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "Ingresar Cuenta del banco valida!", TipoMensaje.Error);
            }
        }

        private int insertarConfiguracion()
        {
            int valor = 0;
            Configuracion objCfg = new Configuracion();
            objCfg.pArchivoPlano = OidArchivo;
            objCfg.pTipoLinea = ddlTipoLinea.SelectedValue;
            ConfiguracionLN objCfgLN = new ConfiguracionLN();
            valor = objCfgLN.insertarCfgArchivoPlano(objCfg);
            return valor;
        }

        private void buscar(bool esBusqPorTxb)
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();
            ArchivoPlano objAP = new ArchivoPlano();
            objAP.pIdCuentaBanco = txbCodigoCuenta.Text.Trim();
            objAP.pTipoProceso = this.ddlTipoProceso.SelectedValue;
            List<ArchivoPlano> lista = new ArchivoPlanoLN().consultar(objAP);
            if (lista.Count > 0)
            {
                OidArchivo = lista[0].pOid.Value;
                this.txbNExluidasInicio.Text = lista[0].pLineasExcluidasInicio.Value.ToString();
                this.txbNExluidasFinal.Text = lista[0].pLineasExcluidasFin.Value.ToString();
                this.txbNumExcel.Text = lista[0].pNumeroHojaExcel.Value.ToString();
                if (lista[0].pEsExcel.Value)
                {
                    this.rbExcel.Checked = true;
                    this.rbPlano.Checked = false;
                }
                else
                {
                    this.rbPlano.Checked = true;
                    this.rbExcel.Checked = false;
                }
            }

            if (OidArchivo > 0)
                imgBtnAgregarFormato.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
            else
                imgBtnAgregarFormato.ImageUrl = "~/MarcaVisual/iconos/agregar.png";

            if (esBusqPorTxb)
            {
                objB.pIdCuentaBanco = txbCodigoCuenta.Text.Trim();
                objB.pTipoProceso = this.ddlTipoProceso.SelectedValue;
                List<EN.Tablas.Banco> listaB = objBancoLN.consultar(objB);
                if (listaB.Count > 0)
                {
                    objB = listaB[0];
                    txbNombreCuenta.Text = objB.pNombreCuenta;
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la cuenta bancaria " + objB.pNombreCuenta + ".", TipoMensaje.Informacion);
                    limpiarTodo(false);
                    llenarLineasDisponible();
                }
                else
                {
                    txbNombreCuenta.Text = String.Empty;
                    limpiarTodo(true);
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Favor crear primero la cuenta bancaria con código: " + objB.pIdCuentaBanco + ".", TipoMensaje.Error);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txbCodigoCuentaB.Text.Trim()))
                    objB.pIdCuentaBanco = txbCodigoCuentaB.Text.Trim();

                if (!String.IsNullOrEmpty(txbNombreCuentaB.Text.Trim()))
                    objB.pNombreCuenta = txbNombreCuentaB.Text.Trim();

                objB.pTipoProceso = this.ddlTipoProceso.SelectedValue;

                BancosEncontrados = objBancoLN.consultar(objB);
                gvBusquedaBanco.DataSource = BancosEncontrados;
                gvBusquedaBanco.DataBind();

                mpeBusquedaBanco.Show();
            }
        }

        private void llenarLineasDisponible()
        {
            ArchivoPlanoLN objAP = new ArchivoPlanoLN();
            ddlTipoLinea.DataSource = objAP.consultarLineasDisponibles(txbCodigoCuenta.Text.Trim(), this.ddlTipoProceso.SelectedValue);
            ddlTipoLinea.DataTextField = TipoLineaDEF.Nombre;
            ddlTipoLinea.DataValueField = TipoLineaDEF.Oid;
            ddlTipoLinea.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(ddlTipoLinea);

            gvTipoLinea.DataSource = objAP.consultarLineasCuentaBanco(txbCodigoCuenta.Text.Trim(), this.ddlTipoProceso.SelectedValue);
            gvTipoLinea.DataBind();
        }

        private void llenarGrillaCampos()
        {
            EstructuraArchivo objEA = new EstructuraArchivo();
            objEA.pConfiguracion = OidConfiguracion;
            gvCampos.DataSource = CamposEstructura = new EstructuraArchivoLN().consultar(objEA);
            gvCampos.DataBind();
        }

        private void agregarCampo()
        {
            EstructuraArchivo objEntidad = new EstructuraArchivo();
            objEntidad.pAlineacion = ddlAlineacion.SelectedValue;

            if (ddlTipoDato.SelectedValue.Equals("DE"))
                objEntidad.pCantidadDecimales = Convert.ToInt32(txbCantidadDecimales.Text.Trim());
            else
                objEntidad.pCantidadDecimales = 0;

            objEntidad.pCaracterRelleno = ddlCaracterRelleno.SelectedValue;
            objEntidad.pConfiguracion = OidConfiguracion;
            objEntidad.pEsContador = chbEsContador.Checked;

            if (ddlTipoDato.SelectedValue.Equals("FE"))
                objEntidad.pFormatoFecha = ddlFormatoFecha.SelectedValue;
            else
                objEntidad.pFormatoFecha = " ";

            objEntidad.pLongitud = Convert.ToInt32(txbLongitud.Text.Trim());

            objEntidad.pNombreCampo = txbNombreCampo.Text.Trim();
            objEntidad.pOrdenColumna = Convert.ToInt32(txbOrdenColumna.Text.Trim());
            objEntidad.pTipoDato = ddlTipoDato.SelectedValue;
            objEntidad.pRequiereCambio = chbRequiereCambio.Checked;
            objEntidad.pValorPorDefecto = chbTieneValor.Checked;
            objEntidad.pValor = txbValor.Text.Trim();

            int valor = 0;
            EstructuraArchivoLN objEA = new EstructuraArchivoLN();
            String add = "actualizado";
            if (OidEstructuraArchivo > 0)
            {
                objEntidad.pOid = OidEstructuraArchivo;
                valor = objEA.actualizar(objEntidad);
            }
            else
            {
                valor = objEA.insertar(objEntidad);
                add = "agregado";
            }
            if (valor > 0)
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha " + add + " el campo " + objEntidad.pNombreCampo + ".", TipoMensaje.Informacion);
                limpiarCampos();
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No se logro agregar el campo " + objEntidad.pNombreCampo + ".", TipoMensaje.Error);
            }
        }

        private void limpiarCampos()
        {
            llenarGrillaCampos();
            OidEstructuraArchivo = 0;

            ddlAlineacion.SelectedIndex = ddlCaracterRelleno.SelectedIndex = ddlTipoDato.SelectedIndex = ddlFormatoFecha.SelectedIndex = 0;
            txbOrdenColumna.Text = txbLongitud.Text = txbNombreCampo.Text = txbCantidadDecimales.Text = "";

            chbEsContador.Checked = chbRequiereCambio.Checked = chbTieneValor.Checked = false;
            txbValor.Text = String.Empty;

            imgBtnAgregarCampo.ImageUrl = "~/MarcaVisual/iconos/agregar.png";

            validarTipoDato();
        }

        private void validarTipoDato()
        {
            if (ddlTipoDato.SelectedValue.Equals("DE"))
            {
                lbCDoFF.Text = "Cantidad de Decimales:";
                lbCDoFF.Visible = txbCantidadDecimales.Visible = true;
                ddlFormatoFecha.Visible = false;

                txbCantidadDecimales.ValidationGroup = rfvCantidadDecimales.ValidationGroup = "3";
                ddlFormatoFecha.ValidationGroup = rfvFormatoFecha.ValidationGroup = "4";
            }
            else if (ddlTipoDato.SelectedValue.Equals("FE"))
            {
                lbCDoFF.Text = "Formato de Fecha:";
                lbCDoFF.Visible = ddlFormatoFecha.Visible = true;
                txbCantidadDecimales.Visible = false;

                ddlFormatoFecha.ValidationGroup = rfvFormatoFecha.ValidationGroup = "3";
                txbCantidadDecimales.ValidationGroup = rfvCantidadDecimales.ValidationGroup = "4";
            }
            else
            {
                lbCDoFF.Visible = ddlFormatoFecha.Visible = txbCantidadDecimales.Visible = false;
                ddlFormatoFecha.ValidationGroup = rfvFormatoFecha.ValidationGroup
                    = txbCantidadDecimales.ValidationGroup = rfvCantidadDecimales.ValidationGroup = "4";
            }
        }

        private void limpiarTodo(bool limpiarBanco)
        {
            OidConfiguracion = OidEstructuraArchivo = 0;
            CamposEstructura = null;
            BancosEncontrados = null;
            if (limpiarBanco)
            {
                OidArchivo = 0;
                imgBtnAgregarFormato.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
                this.txbNumExcel.Text = this.txbNExluidasInicio.Text = this.txbNExluidasFinal.Text = String.Empty; 
                txbCodigoCuenta.Text = txbNombreCuenta.Text = String.Empty;
            }
            txbCantidadDecimales.Text = txbNombreCuentaB.Text = txbLongitud.Text
                = txbCodigoCuentaB.Text = txbNombreCampo.Text = txbOrdenColumna.Text = String.Empty;

            ddlAlineacion.SelectedIndex = ddlCaracterRelleno.SelectedIndex = ddlTipoDato.SelectedIndex = ddlFormatoFecha.SelectedIndex = 0;

            chbEsContador.Checked = chbRequiereCambio.Checked = chbTieneValor.Checked = false;

            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            gvCampos.DataSource = null;
            gvCampos.DataBind();

            gvTipoLinea.DataSource = null;
            gvTipoLinea.DataBind();

            lbTLEditando.Text = "NO HAY LINEA SELECCIONADA";
            lbEstado.Text = string.Empty;
        }

        protected void ddlTipoProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpiarTodo(true);
            if (ddlTipoProceso.SelectedIndex != 0)
                this.pnlDatos.Enabled = this.pnlEstructura.Enabled = this.pnlArchivo.Enabled = this.pnlFormato.Enabled = true;
            else
                this.pnlDatos.Enabled = this.pnlEstructura.Enabled = this.pnlArchivo.Enabled = this.pnlFormato.Enabled = false;
        }

        protected void imgBtnAgregarFormato_Click(object sender, ImageClickEventArgs e)
        {

            
            int valor = 0;
            ArchivoPlano objAP = new ArchivoPlano();
            objAP.pIdCuentaBanco = txbCodigoCuenta.Text.Trim();
            objAP.pTipoProceso = ddlTipoProceso.SelectedValue;
            objAP.pEsExcel = rbExcel.Checked;
            objAP.pNumeroHojaExcel = Convert.ToInt32(txbNumExcel.Text);
            objAP.pLineasExcluidasInicio = Convert.ToInt32(txbNExluidasInicio.Text);
            objAP.pLineasExcluidasFin = Convert.ToInt32(txbNExluidasFinal.Text);

            if (OidArchivo > 0)
            {
                objAP.pOid = OidArchivo;
                valor = new ArchivoPlanoLN().actualizar(objAP);
            }
            else
                OidArchivo = valor = new ArchivoPlanoLN().insertar(objAP);

            if (valor > 0 && OidArchivo > 0)
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se guardo la información exitosamente!", TipoMensaje.Informacion);
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible guardar la información!", TipoMensaje.Error);
            }

        }


    }
}