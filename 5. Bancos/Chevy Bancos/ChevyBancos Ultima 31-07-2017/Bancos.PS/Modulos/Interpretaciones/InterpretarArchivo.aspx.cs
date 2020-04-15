using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bancos.EN;
using Bancos.EN.Definicion;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;

namespace Bancos.PS.Modulos.Interpretaciones
{
    public partial class InterpretarArchivo : System.Web.UI.Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ddlTipoProceso.DataSource = new ArchivoAsobancariaLN().consultar(new ArchivoAsobancaria());
                this.ddlTipoProceso.DataTextField = "pNombre";
                this.ddlTipoProceso.DataValueField = "pOid";
                this.ddlTipoProceso.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoProceso);

                UtilidadesWeb.ajustarMensaje(lbEstado, String.Empty, TipoMensaje.Informacion);
            }
           
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiarPagina();
        }

        protected void imgBtnAgregar_Click(object sender, ImageClickEventArgs e)
        {
            //if (!ddlTipoLinea.SelectedValue.Equals(ddlTipoLineaASO.SelectedValue))
            //{
            //    UtilidadesWeb.ajustarMensaje(lbEstado, "No se puede guardar la equivalencia. Campos deben pertener al mismo tipo de linea", TipoMensaje.Error);
            //    return;
            //}
            InterpreteArchivo objEntidad = new InterpreteArchivo();
            objEntidad.pCampoAsobancaria = Convert.ToInt32(ddlCampoAsobancaria.SelectedValue);
            objEntidad.pCampoBanco = Convert.ToInt32(ddlCampoBanco.SelectedValue);

            foreach (GridViewRow row in gvCampos.Rows)
            {
                if (ddlCampoAsobancaria.Text.Equals(row.Cells[3].Text))
                {
                    if (IdEquivalencias != Convert.ToInt32(row.Cells[6].Text))
                    {
                        UtilidadesWeb.ajustarMensaje(lbEstado, "Ya se guardo una equivalencia con el mismo campo Asobancaria en base de datos", TipoMensaje.Error);
                        return;
                    }
                }
            }

            int valor = 0;
            InterpreteArchivoLN objIArcLN = new InterpreteArchivoLN();
            String add = "actualizado";
            if (IdEquivalencias > 0)
            {
                objEntidad.pId = IdEquivalencias;
                valor = objIArcLN.actualizar(objEntidad);
            }
            else
            {
                valor = objIArcLN.insertar(objEntidad);
                add = "agregado";
            }

            IdEquivalencias = 0;

            if (valor > 0)
            {
                llenarGrilla();
                ddlCampoAsobancaria.SelectedIndex = 0;
                ddlCampoBanco.SelectedIndex = 0;
                imgBtnAgregar.ImageUrl = "~/MarcaVisual/iconos/agregar.png";            
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha " + add + " la equivalencia satisfactoriamente", TipoMensaje.Informacion);

            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No se ha " + add + " la equivalencia!!!", TipoMensaje.Error);
            }
            
        }

        protected void gvCampos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCampos.PageIndex = e.NewPageIndex;
            llenarGrilla();
        }

        protected void gvCampos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {

                IdEquivalencias = Convert.ToInt32(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text));

                if (ddlCampoBanco.Items.FindByText(Convert.ToString(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text))) != null)
                    ddlCampoBanco.SelectedValue = Convert.ToString(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                else
                    ddlCampoBanco.SelectedIndex = 0;

                if (ddlCampoAsobancaria.Items.FindByText(Convert.ToString(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text))) != null)
                    ddlCampoAsobancaria.SelectedValue = Convert.ToString(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text));
                else
                    ddlCampoAsobancaria.SelectedIndex = 0;

                imgBtnAgregar.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
                UtilidadesWeb.ajustarMensaje(lbEstado, string.Empty, TipoMensaje.Informacion);

            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                InterpreteArchivo objEntidad = new InterpreteArchivo();
                objEntidad.pId = Convert.ToInt32(HttpUtility.HtmlDecode(gvCampos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text));

                InterpreteArchivoLN objIArcLN = new InterpreteArchivoLN();
                int valor = objIArcLN.borrar(objEntidad);

                if (valor == 0)
                {
                    llenarGrilla();
                    ddlCampoAsobancaria.SelectedIndex = 0;
                    ddlCampoBanco.SelectedIndex = 0;
                    imgBtnAgregar.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha eliminado la interpretación de campo.", TipoMensaje.Informacion);
                }
                else
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible completar la tarea!!!", TipoMensaje.Error);
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
            txbCodigoCuentaB.Text = txbNombreCuentaB.Text = String.Empty;
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
                lbNombreCuenta.Text = HttpUtility.HtmlDecode(gvBusquedaBanco.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                buscar(true);
            }
        }

        private void llenarLineasDisponible()
        {
            ArchivoPlanoLN objAP = new ArchivoPlanoLN();
            ddlTipoLinea.DataSource = objAP.consultarLineasCuentaBanco(txbCodigoCuenta.Text.Trim(), this.ddlTipoProceso.SelectedValue);
            ddlTipoLinea.DataTextField = TipoLineaDEF.Nombre;
            ddlTipoLinea.DataValueField = ConfiguracionDEF.TipoLinea;
            ddlTipoLinea.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(ddlTipoLinea);
        }

        private void limpiarPagina()
        {
            Response.Redirect("~/Modulos/Interpretaciones/InterpretarArchivo.aspx?tv=bs%2fi%2fiab");
        }

        #region "Funcion: buscar()"
        private void buscar(bool esBusqPorTxb)
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();

            if (esBusqPorTxb)
            {
                objB.pIdCuentaBanco = txbCodigoCuenta.Text.Trim();
                objB.pTipoProceso = this.ddlTipoProceso.SelectedValue;
                List<EN.Tablas.Banco> listaB = objBancoLN.consultar(objB);
                if (listaB.Count > 0)
                {
                    objB = listaB[0];
                    txbCodigoCuenta.Text = objB.pIdCuentaBanco;
                    lbNombreCuenta.Text = objB.pNombreCuenta;
                    ddlTipoLineaASO.SelectedIndex = 0;
                    ddlCampoBanco.Items.Clear();
                    ddlCampoAsobancaria.Items.Clear();
                    imgBtnAgregar.Enabled = gvCampos.Enabled = false;
                    llenarLineasDisponible();
                    llenarGrilla();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la cuenta bancaria " + objB.pNombreCuenta + ".", TipoMensaje.Informacion);
                }
                else
                {
                    lbNombreCuenta.Text = String.Empty;
                    ddlTipoLineaASO.SelectedIndex = 0;
                    ddlCampoBanco.Items.Clear();
                    ddlCampoAsobancaria.Items.Clear();
                    imgBtnAgregar.Enabled = gvCampos.Enabled = false;
                    llenarLineasDisponible();
                    llenarGrilla();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Favor crear primero la cuenta bancaria con código: " + this.txbCodigoCuenta.Text  + ".", TipoMensaje.Error);
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
        #endregion
        
        private void llenarGrilla()
        {
            if (ddlTipoProceso.SelectedValue.ToString() == "TCR1")
                gvCampos.DataSource = new InterpreteArchivoLN().consultar(txbCodigoCuenta.Text.Trim(), "ABT1");
            else
                gvCampos.DataSource = new InterpreteArchivoLN().consultar(txbCodigoCuenta.Text.Trim(), this.ddlTipoProceso.SelectedValue);
            gvCampos.DataBind();
        }

        protected void txbCodigoCuenta_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        protected void ddlTipoProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            ddlTipoLineaASO.Items.Clear();
            ddlTipoLinea.Items.Clear();
            ddlCampoBanco.Items.Clear();
            ddlCampoAsobancaria.Items.Clear();
            this.lbNombreCuenta.Text = this.txbCodigoCuenta.Text =
            this.txbCodigoCuentaB.Text = this.txbNombreCuentaB.Text = String.Empty;
            llenarGrilla();
            imgBtnAgregar.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
            imgBtnAgregar.Enabled = gvCampos.Enabled = false;
            UtilidadesWeb.ajustarMensaje(lbEstado, String.Empty, TipoMensaje.Informacion);

            if (ddlTipoProceso.SelectedIndex != 0)
            {
                this.pnlDatos.Enabled = this.pnlEstructura.Enabled = true;
                ArchivoPlanoLN objAP = new ArchivoPlanoLN();
                ddlTipoLineaASO.DataSource = objAP.consultarLineasAsobancaria(ddlTipoProceso.SelectedValue);
                ddlTipoLineaASO.DataTextField = "NOMBRE";
                ddlTipoLineaASO.DataValueField = "Tipo_Linea";
                ddlTipoLineaASO.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlTipoLineaASO);
            }
            else
                this.pnlDatos.Enabled = this.pnlEstructura.Enabled = false;

        }
              
        protected void ddlTipoLinea_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdEquivalencias = 0;

            imgBtnAgregar.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
            if (ddlTipoLinea.SelectedIndex == 0)
            {
                ddlCampoBanco.Items.Clear();
                imgBtnAgregar.Enabled = gvCampos.Enabled = false;
            }
            else
            {
                EstructuraArchivoLN objEALN = new EstructuraArchivoLN();
                ddlCampoBanco.DataSource = objEALN.consultarCamposCuenta(txbCodigoCuenta.Text.Trim(), ddlTipoLinea.SelectedValue, ddlTipoProceso.SelectedValue);
                ddlCampoBanco.DataTextField = EstructuraArchivoDEF.NombreCampo;
                ddlCampoBanco.DataValueField = EstructuraArchivoDEF.Oid;
                ddlCampoBanco.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlCampoBanco);
                if (ddlTipoLineaASO.SelectedIndex != 0 && ddlTipoLineaASO.Items.Count != 0)
                    imgBtnAgregar.Enabled = gvCampos.Enabled = true;
            }

        }

        protected void ddlTipoLineaASO_SelectedIndexChanged(object sender, EventArgs e)
        {
            IdEquivalencias = 0;
            
            imgBtnAgregar.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
            if (ddlTipoLineaASO.SelectedIndex == 0)
            {
                ddlCampoAsobancaria.Items.Clear();
                imgBtnAgregar.Enabled = gvCampos.Enabled = false;
            }
            else
            {
                EstructuraArchivoLN objEALN = new EstructuraArchivoLN();
               
                if(ddlTipoProceso.SelectedValue == "TCR1")
                     ddlCampoAsobancaria.DataSource = objEALN.consultarCamposAsobancaria(ddlTipoLineaASO.SelectedValue,"ABT1", txbCodigoCuenta.Text.Trim());
                else
                     ddlCampoAsobancaria.DataSource = objEALN.consultarCamposAsobancaria(ddlTipoLineaASO.SelectedValue, ddlTipoProceso.SelectedValue, txbCodigoCuenta.Text.Trim());

               
                ddlCampoAsobancaria.DataTextField = EstructuraArchivoDEF.NombreCampo;
                ddlCampoAsobancaria.DataValueField = EstructuraArchivoDEF.Oid;
                ddlCampoAsobancaria.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlCampoAsobancaria);
                if (ddlTipoLinea.SelectedIndex != 0 && ddlTipoLinea.Items.Count != 0)
                    imgBtnAgregar.Enabled = gvCampos.Enabled = true;
            }
        }

    }
}