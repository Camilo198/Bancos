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
    public partial class EstructuraArchivos : System.Web.UI.Page
    {
        #region "Propiedades: CamposEstructura, OidArchivoAsobancaria, OidConfiguracion, OidEstructuraArchivo"

        string campos = string.Empty;

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

        private String OidArchivoAsobancaria
        {
            get
            {
                String valor = string.Empty;
                if (ViewState["OidArchivoAsobancaria"] != null)
                    valor = (String)ViewState["OidArchivoAsobancaria"];
                return valor;
            }
            set
            {
                ViewState["OidArchivoAsobancaria"] = value;
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

                this.ddlTipoArchivo.DataSource = new ArchivoAsobancariaLN().consultar(new ArchivoAsobancaria());
                this.ddlTipoArchivo.DataTextField = "pNombre";
                this.ddlTipoArchivo.DataValueField = "pOid";
                this.ddlTipoArchivo.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoArchivo);

            }
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Banco/ArchivoBanco.aspx?tv=bs%2fcg%2fab");
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            UtilidadesWeb.ajustarMensaje(lbEstado, "Información Guardada", TipoMensaje.Informacion);
        }

        protected void txbCodigoBanco_TextChanged(object sender, EventArgs e)
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
                campos = (Convert.ToString(HttpUtility.HtmlDecode(this.gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text)));
                int valor = new EstructuraArchivoLN().borrar(objEA);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se eliminó el campo exitosamente!", TipoMensaje.Informacion);
                    llenarGrillaCampos();
                    Log(3, 2);
                    campos = string.Empty;
                }
                else
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible eliminar el campo, es posible que tenga traducciones asociadas!!!", TipoMensaje.Error);
            }
        }

        protected void gvTipoLinea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTipoLinea.PageIndex = e.NewPageIndex;
            gvTipoLinea.DataSource = new ArchivoPlanoLN().consultarLineasAsobancaria(OidArchivoAsobancaria);
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
                campos = (Convert.ToString(HttpUtility.HtmlDecode(this.gvTipoLinea.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text)));
                int valor = new ConfiguracionLN().borrar(objCfg);

                if (valor == 0)
                {
                    llenarLineasDisponibles();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se eliminó la línea exitosamente!", TipoMensaje.Informacion);
                    Log(3, 1);
                    campos = string.Empty;
                }
                else
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible eliminar la línea, puede que tenga campos asociados!!!", TipoMensaje.Error);
            }
        }

        protected void imgAgregarTipoLinea_Click(object sender, ImageClickEventArgs e)
        {
            agregarTipoLinea();
        }

        private void agregarTipoLinea()
        {
            int valor = insertarConfiguracion();

            if (valor > 0)
            {
                llenarLineasDisponibles();
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se registro la linea exitosamente!", TipoMensaje.Informacion);
                Log(1, 1);
                campos = string.Empty;
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible registrar la linea, quiza el codigo del banco no esta registrado!", TipoMensaje.Error);
            }
        }

        private int insertarConfiguracion()
        {
            int valor = 0;

            Configuracion objCfg = new Configuracion();
            objCfg.pArchivoAsobancaria = OidArchivoAsobancaria;
            objCfg.pTipoLinea = ddlTipoLinea.SelectedValue;
            campos = this.ddlTipoLinea.SelectedItem.Text;
            ConfiguracionLN objCfgLN = new ConfiguracionLN();
            valor = objCfgLN.insertarCfgAsobancaria(objCfg);

            return valor;
        }

        private void buscar(bool esBusqPorTxb)
        {
            llenarLineasDisponibles();
        }

        private void llenarLineasDisponibles()
        {
            ArchivoPlanoLN objAP = new ArchivoPlanoLN();
            ddlTipoLinea.DataSource = objAP.consultarLineasDisponiblesAso(OidArchivoAsobancaria);
            ddlTipoLinea.DataTextField = TipoLineaDEF.Nombre;
            ddlTipoLinea.DataValueField = TipoLineaDEF.Oid;
            ddlTipoLinea.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(ddlTipoLinea);

            gvTipoLinea.DataSource = objAP.consultarLineasAsobancaria(OidArchivoAsobancaria);
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
               
                #region (INFORMACION PARA LOG)
                campos = string.Concat(objEntidad.pNombreCampo, " con ORDEN COLUMNA:",
                         Convert.ToString(objEntidad.pOrdenColumna.Value), ", LONGITUD:",
                         Convert.ToString(objEntidad.pLongitud.Value), ", REQUIERE CAMBIO:",
                         Convert.ToString(objEntidad.pRequiereCambio.Value).ToUpper(), ", ES CONTADOR:",
                         Convert.ToString(objEntidad.pEsContador.Value).ToUpper(), ", ALINEACION:",
                         this.ddlAlineacion.SelectedItem.Text.ToUpper(), ", CARACTER RELLENO:",
                         this.ddlCaracterRelleno.SelectedItem.Text.ToUpper(), ", VALOR POR DEFECTO:",
                         Convert.ToString(objEntidad.pValorPorDefecto.Value).ToUpper(), ", VALOR:",
                         objEntidad.pValor, ", TIPO DATO:",
                         this.ddlTipoDato.SelectedItem.Text.ToUpper(), ", CANTIDAD DE DECIMALES:",
                         Convert.ToString(objEntidad.pCantidadDecimales.Value), ", FORMATO FECHA:",
                         objEntidad.pFormatoFecha);
                #endregion

                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha " + add + " el campo " + objEntidad.pNombreCampo + ".", TipoMensaje.Informacion);
                limpiarCampos();
                if (add.Equals("actualizado"))
                    Log(2, 2);
                else
                    Log(1, 2);
                campos = string.Empty;
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

            ddlAlineacion.SelectedIndex = ddlCaracterRelleno.SelectedIndex = 
            ddlTipoDato.SelectedIndex = ddlFormatoFecha.SelectedIndex = 0;
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

        private void limpiarTodo()
        {
            OidConfiguracion = OidEstructuraArchivo = 0;
            CamposEstructura = null;

            txbCantidadDecimales.Text = txbLongitud.Text
                = txbNombreCampo.Text = txbOrdenColumna.Text = String.Empty;

            ddlAlineacion.SelectedIndex = ddlCaracterRelleno.SelectedIndex = ddlTipoDato.SelectedIndex =
                                          ddlFormatoFecha.SelectedIndex = 0;

            chbEsContador.Checked = false;

            gvCampos.DataSource = null;
            gvCampos.DataBind();

            gvTipoLinea.DataSource = null;
            gvTipoLinea.DataBind();

            lbTLEditando.Text = "NO HAY LINEA SELECCIONADA";
        }

        // LAS OPCIONES SON SELECCIONE,RECAUDO Y FACTURACION . SIEMPRE QUE SE CAMBIE EL INDEX DEL ddlTipoArchivo LOS 
        //GRIDVIEW  gvTipoLinea Y gvCampos SE VACIARAN Y TAMBIEN CAMBIARA LA VARIABLE OidArchivoAsobancaria PARA ESTABLECERCE
        // COMO FACTURACION O RECAUDO.
        protected void ddlTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoArchivo.SelectedIndex == 0)
            {
                OidArchivoAsobancaria = String.Empty;
                gvTipoLinea.DataSource = gvCampos.DataSource = null;
                gvTipoLinea.DataBind();
                gvCampos.DataBind();
                ddlTipoLinea.Enabled = false;
            }
            else
            {
                gvTipoLinea.DataSource = gvCampos.DataSource = null;
                gvTipoLinea.DataBind();
                gvCampos.DataBind();
                ddlTipoLinea.Enabled = true;
                OidArchivoAsobancaria = ddlTipoArchivo.SelectedValue;
                llenarLineasDisponibles();
            }   
        }

        //opcion 1:Insertar,2:Actualizar,3:Eliminar
        //dato 1:Linea,2:Campo
        private void Log(int opcion, int dato)
        {

            string fecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
            Logs objL = new Logs();
            switch (opcion)
            {
                case 1:
                    if (dato == 1)
                    {
                        objL.pFecha = fecha;
                        objL.pUsuario = HttpContext.Current.User.Identity.Name;
                        objL.pDetalle = "Se creó la linea " + campos;
                        objL.pTipoArchivo = OidArchivoAsobancaria;
                        objL.pTipoProceso = "EST";
                        new LogsLN().insertar(objL);
                    }
                    else
                    {
                        objL.pFecha = fecha;
                        objL.pUsuario = HttpContext.Current.User.Identity.Name;
                        objL.pDetalle = "Se creó el campo " + campos;
                        objL.pTipoArchivo = OidArchivoAsobancaria;
                        objL.pTipoProceso = "EST";
                        new LogsLN().insertar(objL);
                    }
                    break;

                case 2:
                    if (dato == 1)
                    {
                        objL.pFecha = fecha;
                        objL.pUsuario = HttpContext.Current.User.Identity.Name;
                        objL.pDetalle = "Se actualizó la linea " + campos;
                        objL.pTipoArchivo = OidArchivoAsobancaria;
                        objL.pTipoProceso = "EST";
                        new LogsLN().insertar(objL);
                    }
                    else
                    {
                        objL.pFecha = fecha;
                        objL.pUsuario = HttpContext.Current.User.Identity.Name;
                        objL.pDetalle = "Se actualizó el campo " + campos;
                        objL.pTipoArchivo = OidArchivoAsobancaria;
                        objL.pTipoProceso = "EST";
                        new LogsLN().insertar(objL);
                    }
                    break;

                case 3:
                    if (dato == 1)
                    {
                        objL.pFecha = fecha;
                        objL.pUsuario = HttpContext.Current.User.Identity.Name;
                        objL.pDetalle = "Se eliminó la linea " + campos;
                        objL.pTipoArchivo = OidArchivoAsobancaria;
                        objL.pTipoProceso = "EST";
                        new LogsLN().insertar(objL);
                    }
                    else
                    {
                        objL.pFecha = fecha;
                        objL.pUsuario = HttpContext.Current.User.Identity.Name;
                        objL.pDetalle = "Se eliminó el campo " + campos;
                        objL.pTipoArchivo = OidArchivoAsobancaria;
                        objL.pTipoProceso = "EST";
                        new LogsLN().insertar(objL);
                    }
                    break;
            }

        }

        protected void gvTipoLinea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }

}