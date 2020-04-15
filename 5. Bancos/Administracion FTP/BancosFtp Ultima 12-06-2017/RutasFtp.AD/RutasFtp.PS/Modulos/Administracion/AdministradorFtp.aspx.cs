using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RutasFtp.PS.Codigo;
using RutasFtp.EN;
using RutasFtp.EN.Tablas;
using RutasFtp.LN.Consultas;
using RutasFtp.EN.Definicion;

namespace RutasFtp.PS.Modulos.Administracion
{
    public partial class AdministradorFtp : System.Web.UI.Page
    {
        private int IdEntidad
        {
            get
            {
                int clave = 0;
                if (ViewState["IdEntidad"] != null)
                    clave = Convert.ToInt32(ViewState["IdEntidad"]);
                return clave;
            }

            set
            {
                ViewState["IdEntidad"] = value;
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

        private String FechaUltimoIngreso {get; set;}
        //{
        //    get
        //    {
        //        String clave = 0;
        //        if (ViewState["FechaUltimoIngreso"] != null)
        //            clave = Convert.ToString(ViewState["FechaUltimoIngreso"]);
        //        return clave;
        //    }

        //    set
        //    {
        //        ViewState["FechaUltimoIngreso"] = value;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ////Codigo usado Para pruebas
               Servicios.moverArchivosFTP move = new Servicios.moverArchivosFTP();
               move.trasladarArchivos();
                
                this.ddlTipoProceso.DataSource = new ArchivoAsobancariaLN().consultar(new ArchivoAsobancaria());
                this.ddlTipoProceso.DataTextField = "pNombre";
                this.ddlTipoProceso.DataValueField = "pOid";
                this.ddlTipoProceso.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoProceso);

                //ArrayList archivos = new ArrayList();
                //archivos = ConectorFTP.listarArchivos("ftp://172.16.20.40/", "transfer.chevyplan", "Ep1c0r1*");
            }
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            Ftp objFtp = new Ftp();
            objFtp.pId = IdEntidad;
            objFtp.pUrlFtp = this.txbUrlFTP.Text;
            objFtp.pUsuarioFtp = this.txbUsuarioFTP.Text;
            objFtp.pClaveFtp = this.txbClave.Text;
            objFtp.pRutaDestino = this.txbRutaSalida.Text;
            objFtp.pIdCuentaBanco = this.txbCodigoCuenta.Text;
            objFtp.pTipoProceso = this.ddlTipoProceso.SelectedValue;

            objFtp.pPrefijo =this.txbPrefijo.Text ;
            objFtp.pFormato =this.txbFormato.Text;
            if (this.chbFTPSeguro.Checked)
                objFtp.pFtpSeguro  = true;
            else
                objFtp.pFtpSeguro = false;

            //objFtp.pFechaUltimoIngreso = FechaUltimoIngreso;

            int valor = 0;
            FtpLN objELN = new FtpLN();
            if (IdEntidad <= 0)
            {
                valor = objELN.insertar(objFtp);
            }
            else
            {
                valor = objELN.actualizar(objFtp);
            }

            if (valor > 0)
            {
                this.txbCodigoCuenta.Text = this.txbNombreCuenta.Text = String.Empty;
                limpiar();
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha guardado la informacion correctamente.", TipoMensaje.Informacion);
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No se ha guardado la informacion!!!", TipoMensaje.Error);
            }
        }

        protected void txbCodigoCuenta_TextChanged(object sender, EventArgs e)
        {
            buscar(true);
        }

        private void buscar(bool esBusqPorTxb)
        {
            Banco objB = new Banco();
            BancoLN objBancoLN = new BancoLN();

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

                    Ftp objFtp = new Ftp();
                    FtpLN objFtpLN = new FtpLN();
                    objFtp.pIdCuentaBanco = objB.pIdCuentaBanco;
                    objFtp.pTipoProceso = this.ddlTipoProceso.SelectedValue;
                    List<Ftp> listaFtp = objFtpLN.consultar(objFtp);
                    if (listaFtp.Count > 0)
                    {
                        objFtp = listaFtp[0];
                        IdEntidad = objFtp.pId.Value;
                        this.txbUrlFTP.Text = objFtp.pUrlFtp;
                        this.txbUsuarioFTP.Text = objFtp.pUsuarioFtp;
                        this.txbClave.Attributes.Add("Value", objFtp.pClaveFtp);
                        this.txbRutaSalida.Text = objFtp.pRutaDestino;
                        this.txbPrefijo.Text = objFtp.pPrefijo;
                        this.txbFormato.Text = objFtp.pFormato;
                        if (objFtp.pFtpSeguro.Value)
                            this.chbFTPSeguro.Checked = true;
                        else
                            this.chbFTPSeguro.Checked = false;
                        //FechaUltimoIngreso = objFtp.pFechaUltimoIngreso;
                    }
                    else
                    {
                        limpiar();//no hay q borrar cuen y nom
                        UtilidadesWeb.ajustarMensaje(lbEstado, "Cuenta no contiene configuración FTP", TipoMensaje.Informacion);
                    }

                }
                else
                {
                    this.txbNombreCuenta.Text = String.Empty;
                    limpiar();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No se ha encontrado ninguna cuenta bancaria con este tipo de proceso", TipoMensaje.Error);
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

        private void limpiar()
        {
            IdEntidad = 0;
            this.txbUrlFTP.Text = this.txbUsuarioFTP.Text =
            this.txbClave.Text = this.txbRutaSalida.Text = 
            this.txbPrefijo.Text = this.txbFormato.Text = String.Empty;
            this.chbFTPSeguro.Checked = false;
            this.txbClave.Attributes.Clear();
            gvBusquedaBanco.DataSource = null;
            gvBusquedaBanco.DataBind();
        }

        protected void imgBtnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            if (IdEntidad != 0)
            {
                Ftp objFtp = new Ftp();
                objFtp.pId = IdEntidad;

                int valor = 0;
                valor = new FtpLN().borrar(objFtp);

                if (valor == 0)
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha borrado la configuración Ftp con éxito.", TipoMensaje.Informacion);
                    limpiar();
                    this.txbCodigoCuenta.Text = this.txbNombreCuenta.Text = String.Empty;
                }
                else
                    UtilidadesWeb.ajustarMensaje(lbEstado, "No se pudo borrar la configuración Ftp!!!", TipoMensaje.Error);
            }
        }

        protected void ddlTipoProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txbCodigoCuenta.Text = this.txbNombreCuenta.Text = String.Empty;
            limpiar();
            if (ddlTipoProceso.SelectedIndex == 0)
            {
                pnlFTP.Enabled = false;
            }
            else
            {
                pnlFTP.Enabled = true;
            }
        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Modulos/Administracion/AdministradorFtp.aspx?tv=ftp%2fa%2faaf");
        }

    }
}