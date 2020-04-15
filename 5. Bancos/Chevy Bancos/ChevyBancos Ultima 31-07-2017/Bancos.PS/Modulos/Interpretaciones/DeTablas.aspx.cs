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
    public partial class DeTablas : System.Web.UI.Page
    {
        #region "Propiedades: TransformacionesDisponibles, BancosEncontrados"
        private List<Transformar> TransformacionesDisponibles
        {
            get
            {
                List<Transformar> lista = new List<Transformar>();
                if (ViewState["TransformacionesDisponibles"] != null)
                    lista = (List<Transformar>)ViewState["TransformacionesDisponibles"];
                return lista;
            }

            set
            {
                ViewState["TransformacionesDisponibles"] = value;
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
                iniciarlizarComponentes();
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar();
        }

        protected void txbCodigoBanco_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        protected void imgBtnAgregar_Click(object sender, ImageClickEventArgs e)
        {
            Transformar objEntidad = new Transformar();

            try
            {
                objEntidad.pValorAsobancaria = Convert.ToInt32(ddlRespuestaAsobancaria.SelectedValue);
                objEntidad.pValorBanco = Convert.ToInt32(ddlRespuestaBanco.SelectedValue);
            }
            catch { }

            TransformarLN objConsultor = new TransformarLN();
            int valor = objConsultor.insertar(objEntidad);

            if (valor > 0)
            {
                llenarGrilla();
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha agregado una reinterpretacion de la respuesta satisfactoriamente.", TipoMensaje.Informacion);
            }
            else
                UtilidadesWeb.ajustarMensaje(lbEstado, "No fue posible completar la tarea!!!", TipoMensaje.Error);
        }

        protected void gvValores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvValores.PageIndex = e.NewPageIndex;
            gvValores.DataSource = TransformacionesDisponibles;
            gvValores.DataBind();
        }

        protected void gvValores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Eliminar"))
            {
                Transformar objEntidad = new Transformar();
                try
                {
                    objEntidad.pValorAsobancaria = Convert.ToInt32(HttpUtility.HtmlDecode(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text));
                }
                catch
                {
                    objEntidad.pValorAsobancaria = 0;
                }

                try
                {
                    objEntidad.pValorBanco = Convert.ToInt32(HttpUtility.HtmlDecode(gvValores.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text));
                }
                catch (Exception)
                {
                    objEntidad.pValorBanco = 0;
                }

                TransformarLN objConsultor = new TransformarLN();
                int valor = objConsultor.borrar(objEntidad);

                if (valor == 0)
                {
                    llenarGrilla();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha eliminado la interpretación del valor.", TipoMensaje.Informacion);
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
                lbNombreBanco.Text = HttpUtility.HtmlDecode(gvBusquedaBanco.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text);
                buscar(true);
            }
        }

        protected void imgBtnFiltrar_Click(object sender, ImageClickEventArgs e)
        {
            ddlRespuestaAsobancaria.DataSource = obtenerRespuestas(ddlTablaASO.SelectedValue);
            ddlRespuestaAsobancaria.DataTextField = "pCodigo";
            ddlRespuestaAsobancaria.DataValueField = "pOid";
            ddlRespuestaAsobancaria.DataBind();


            ddlRespuestaBanco.DataSource = obtenerRespuestas(ddlTablaBanco.SelectedValue);
            ddlRespuestaBanco.DataTextField = "pCodigo";
            ddlRespuestaBanco.DataValueField = "pOid";
            ddlRespuestaBanco.DataBind();

            llenarGrilla();
        }

        private void iniciarlizarComponentes()
        {
            ddlTablaASO.DataSource = obtenerTablas("");
            ddlTablaASO.DataTextField = "pNombre";
            ddlTablaASO.DataValueField = "pOid";
            ddlTablaASO.DataBind();
            UtilidadesWeb.agregarSeleccioneDDL(ddlTablaASO);

            UtilidadesWeb.ajustarMensaje(lbEstado, "Listo...", TipoMensaje.Informacion);
        }

        private List<Valor> obtenerRespuestas(String tabla)
        {
            Valor objEntidad = new Valor();
            objEntidad.pTabla = Convert.ToInt32(tabla);

            return new ValorLN().consultar(objEntidad);
        }

        private List<Tabla> obtenerTablas(String codigoBanco)
        {
            Tabla objTabla = new Tabla();

            if (!String.IsNullOrEmpty(codigoBanco))
                objTabla.pBanco = txbCodigoBanco.Text.Trim();
            else
                objTabla.pEsAsobancaria = true;

            TablaLN objTablaLN = new TablaLN();
            return objTablaLN.consultarTablasValor(objTabla);
        }

        private void limpiar()
        {
            UtilidadesWeb.ajustarMensaje(lbEstado, new NotImplementedException().Message, TipoMensaje.Error);
        }

        private void llenarGrilla()
        {
            gvValores.DataSource = TransformacionesDisponibles = new TransformarLN().consultar(txbCodigoBanco.Text, ddlTablaBanco.SelectedValue, ddlTablaASO.SelectedValue);
            gvValores.DataBind();
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
                    //////lbNombreBanco.Text = objB.pNombre;
                    ddlTablaBanco.DataSource = obtenerTablas(objB.pCodigoBanco);
                    ddlTablaBanco.DataTextField = "pNombre";
                    ddlTablaBanco.DataValueField = "pOid";
                    ddlTablaBanco.DataBind();
                    UtilidadesWeb.agregarSeleccioneDDL(ddlTablaBanco);
                    //////UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la entidad bancaria llamada " + objB.pNombre + ".", TipoMensaje.Informacion);
                }
                else
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se puede crear la entidad bancaria con código " + objB.pCodigoBanco + ".", TipoMensaje.Informacion);
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
    }
}