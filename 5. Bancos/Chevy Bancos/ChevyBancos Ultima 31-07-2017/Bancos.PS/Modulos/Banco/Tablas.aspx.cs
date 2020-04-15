﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bancos.EN;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;

namespace Bancos.PS.Modulos.Banco
{
    public partial class Tablas : System.Web.UI.Page
    {
        #region "Propiedades: OIDTabla, OIDValor, TablasDisponibles, ValoresDisponibles, BancosEncontrados"
        private int OIDTabla
        {
            get
            {
                int valor = 0;
                if (ViewState["OIDTabla"] != null)
                    valor = (int)ViewState["OIDTabla"];
                return valor;
            }

            set
            {
                ViewState["OIDTabla"] = value;
            }
        }

        private int OIDValor
        {
            get
            {
                int valor = 0;
                if (ViewState["OIDValor"] != null)
                    valor = (int)ViewState["OIDValor"];
                return valor;
            }

            set
            {
                ViewState["OIDValor"] = value;
            }
        }

        private List<Tabla> TablasDisponibles
        {
            get
            {
                List<Tabla> lista = new List<Tabla>();
                if (ViewState["TablasDisponibles"] != null)
                    lista = (List<Tabla>)ViewState["TablasDisponibles"];
                return lista;
            }

            set
            {
                ViewState["TablasDisponibles"] = value;
            }
        }

        private List<Valor> ValoresDisponibles
        {
            get
            {
                List<Valor> lista = new List<Valor>();
                if (ViewState["ValoresDisponibles"] != null)
                    lista = (List<Valor>)ViewState["ValoresDisponibles"];
                return lista;
            }

            set
            {
                ViewState["ValoresDisponibles"] = value;
            }
        }

        public List<EN.Tablas.Banco> BancosEncontrados
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
                UtilidadesWeb.ajustarMensaje(lbEstado, "Listo...", TipoMensaje.Informacion);
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            agregarTabla();
        }

        protected void imgBtnAddTabla_Click(object sender, ImageClickEventArgs e)
        {
            agregarTabla();
        }

        protected void gvTablas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OIDTabla = Convert.ToInt32(gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                txbCodigoBanco.Text = HttpUtility.HtmlDecode(gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                txbNombreTabla.Text = HttpUtility.HtmlDecode(gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text);
                chbEsAsobancaria.Checked = ((CheckBox)gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Controls[0]).Checked;
                chbEsAsobancaria.Enabled = false;
                imgBtnAddTabla.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
                lbTLEditando.Text = txbNombreTabla.Text;
                llenarGrillaValores();
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                Tabla objTabla = new Tabla();
                objTabla.pOid = Convert.ToInt32(gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                TablaLN objConsultor = new TablaLN();
                int valor = objConsultor.borrar(objTabla);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha borrado la tabla " + gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text, TipoMensaje.Informacion);
                    llenarGrillaTablas();
                }
                else
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible borrar la tabla " + gvTablas.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text + ", puede tener datos asociados.", TipoMensaje.Error);
                }
            }
        }

        protected void gvTablas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTablas.PageIndex = e.NewPageIndex;
            gvTablas.DataSource = TablasDisponibles;
            gvTablas.DataBind();
        }

        protected void imgBtnFiltrar_Click(object sender, ImageClickEventArgs e)
        {
            llenarGrillaTablas();
        }

        protected void chbEsAsobancaria_CheckedChanged(object sender, EventArgs e)
        {
            validarAsobancaria();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscar(false);
            btnLimpiar.Enabled = true;
        }

        protected void btnNuevaBusqueda_Click(object sender, EventArgs e)
        {
            btnLimpiar.Enabled = false;
            txbCodigoBancoB.Text = txbNombreBancoB.Text = String.Empty;
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
                txbCodigoBanco.Text = HttpUtility.HtmlDecode(gvBusquedaBanco.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text);
                buscar(true);
            }
        }

        protected void txbCodigoBanco_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        protected void gvValores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvValores.PageIndex = e.NewPageIndex;
            gvValores.DataSource = ValoresDisponibles;
            gvValores.DataBind();
        }

        protected void gvValores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                OIDValor = Convert.ToInt32(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                OIDTabla = Convert.ToInt32(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                txbCodigo.Text = HttpUtility.HtmlDecode(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text);
                txbDesc.Text = HttpUtility.HtmlDecode(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].Text);
                imgBtnAddValor.ImageUrl = "~/MarcaVisual/iconos/aceptar.png";
            }
            else if (e.CommandName.Equals("Eliminar"))
            {
                Valor objEntidad = new Valor();
                objEntidad.pOid = Convert.ToInt32(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);

                ValorLN objConsultor = new ValorLN();
                int valor = objConsultor.borrar(objEntidad);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha borrado el valor con codigo " + gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text, TipoMensaje.Informacion);
                    llenarGrillaValores();
                }
                else
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible borrar el valor con codigo " + gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[4].Text + ", puede tener datos asociados.", TipoMensaje.Error);
                }
            }
        }

        protected void imgBtnAddValor_Click(object sender, ImageClickEventArgs e)
        {
            if (OIDTabla > 0)
            {
                Valor objEntidad = new Valor();
                objEntidad.pCodigo = txbCodigo.Text.Trim();
                objEntidad.pDescripcion = txbDesc.Text.Trim();
                objEntidad.pTabla = OIDTabla;

                ValorLN objConsultor = new ValorLN();
                int valor = 0;

                if (OIDValor > 0)
                {
                    objEntidad.pOid = OIDValor;
                    valor = objConsultor.actualizar(objEntidad);
                }
                else
                    valor = objConsultor.insertar(objEntidad);

                if (valor > 0)
                {
                    OIDValor = 0;
                    txbCodigo.Text = txbDesc.Text = String.Empty;
                    llenarGrillaValores();
                    imgBtnAddValor.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Los datos se almacenaron.", TipoMensaje.Informacion);
                }
                else
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible almacenar la información.", TipoMensaje.Error);
                }
            }
            else
            {
                lbTLEditando.Text = "NO HAY TABLA SELECCIONADA";
                UtilidadesWeb.ajustarMensaje(lbEstado, "NO HAY TABLA SELECCIONADA", TipoMensaje.Error);
            }
        }

        #region "Funcion: buscar()"
        private void buscar(bool esBusqPorTxb)
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();

            if (esBusqPorTxb)
            {
                objB.pCodigoBanco = txbCodigoBanco.Text.Trim();
                List<EN.Tablas.Banco> listaB = objBancoLN.consultar(objB);
                if (listaB.Count > 0)
                {
                    objB = listaB[0];
                    txbCodigoBanco.Text = objB.pCodigoBanco;
                    llenarGrillaTablas();
                    //////UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la entidad bancaria llamada " + objB.pNombre + ".", TipoMensaje.Informacion);
                }
                else
                {
                    limpiarGrillas();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se puede crear la entidad bancaria con código " + objB.pCodigoBanco + ", dirijase a la pagina de bancos!!!", TipoMensaje.Informacion);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txbCodigoBancoB.Text.Trim()))
                    objB.pCodigoBanco = txbCodigoBancoB.Text.Trim();

                if (!String.IsNullOrEmpty(txbNombreBancoB.Text.Trim()))
                    //////objB.pNombre = txbNombreBancoB.Text.Trim();

                BancosEncontrados = objBancoLN.consultar(objB);
                gvBusquedaBanco.DataSource = BancosEncontrados;
                gvBusquedaBanco.DataBind();

                mpeBusquedaBanco.Show();
            }
        }
        #endregion

        private void llenarGrillaTablas()
        {
            Tabla objTabla = new Tabla();

            objTabla.pEsAsobancaria = chbEsAsobancaria.Checked;
            if (!chbEsAsobancaria.Checked)
                objTabla.pBanco = txbCodigoBanco.Text.Trim();

            TablaLN objTablaLN = new TablaLN();
            TablasDisponibles = objTablaLN.consultarTablasValor(objTabla);
            gvTablas.DataSource = TablasDisponibles;
            gvTablas.DataBind();
        }

        private void llenarGrillaValores()
        {
            Valor objEntidad = new Valor();
            objEntidad.pTabla = OIDTabla;

            ValorLN objValorLN = new ValorLN();
            ValoresDisponibles = objValorLN.consultar(objEntidad);
            gvValores.DataSource = ValoresDisponibles;
            gvValores.DataBind();
        }

        private void limpiar()
        {
            chbEsAsobancaria.Enabled = true;
            OIDTabla = OIDValor = 0;
            txbCodigo.Text = txbCodigoBanco.Text = txbCodigoBancoB.Text = txbDesc.Text = txbNombreBancoB.Text
                = txbNombreTabla.Text = String.Empty;

            chbEsAsobancaria.Checked = false;
            validarAsobancaria();

            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            gvTablas.DataSource = null;
            gvTablas.DataBind();

            gvValores.DataSource = null;
            gvValores.DataBind();

            imgBtnAddValor.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
            imgBtnAddTabla.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
            lbTLEditando.Text = "NO HAY TABLA SELECCIONADA";

            UtilidadesWeb.ajustarMensaje(lbEstado, "Listo", TipoMensaje.Informacion);
        }

        private void limpiarGrillas()
        {
            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();

            gvTablas.DataSource = null;
            gvTablas.DataBind();

            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();
        }

        private void agregarTabla()
        {
            Tabla objEntidad = new Tabla();
            objEntidad.pBanco = txbCodigoBanco.Text.Trim();
            objEntidad.pEsAsobancaria = chbEsAsobancaria.Checked;
            objEntidad.pNombre = txbNombreTabla.Text.Trim();

            if (chbEsAsobancaria.Checked)
                objEntidad.pBanco = String.Empty;

            TablaLN objConsultor = new TablaLN();
            int valor = 0;
            if (OIDTabla > 0)
            {
                objEntidad.pOid = OIDTabla;
                valor = objConsultor.actualizar(objEntidad);
            }
            else
            {
                if (chbEsAsobancaria.Checked)
                    valor = objConsultor.insertar2(objEntidad);
                else
                    valor = objConsultor.insertar(objEntidad);
            }

            if (valor > 0)
            {
                llenarGrillaTablas();
                chbEsAsobancaria.Enabled = true;
                txbNombreTabla.Text = String.Empty;
                OIDTabla = 0;
                imgBtnAddValor.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
                imgBtnAddTabla.ImageUrl = "~/MarcaVisual/iconos/agregar.png";
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha almacenado la información.", TipoMensaje.Informacion);
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible almacenar la información.", TipoMensaje.Error);
            }
        }

        private void validarAsobancaria()
        {
            if (chbEsAsobancaria.Checked)
            {
                txbCodigoBanco.ValidationGroup = rfvCodigoBanco.ValidationGroup = "0";
                txbCodigoBanco.Text = String.Empty;
                llenarGrillaTablas();
                OIDValor = 0;
                lbTLEditando.Text = "NO HAY TABLA SELECCIONADA";
            }
            else
                txbCodigoBanco.ValidationGroup = rfvCodigoBanco.ValidationGroup = "1";
        }
    }
}