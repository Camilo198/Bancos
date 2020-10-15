using System;
using System.Collections;
using System.Data;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.LN.Utilidades;
using Bancos.EN.Definicion;
using Bancos.PS.Servicios.Ftp;
using Bancos.PS.Servicios.Correo;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Bancos.EN;
using Bancos.PS.Codigo;
using System.Threading;




using Bancos.PS.Servicios.Archivos;


namespace Bancos.PS.Modulos.Banco
{
    public partial class ArchivoSalida : System.Web.UI.Page
    {
         String Remitente = "SistemaAsobancaria@chevyplan.com.co";

         #region VARIABLES
         private int OidRutaSalida
        {
            get
            {
                int oidRE = 0;
                if (ViewState["OidRutaSalida"] != null)
                    oidRE = Convert.ToInt32(ViewState["OidRutaSalida"]);
                return oidRE;
            }

            set
            {
                ViewState["OidRutaSalida"] = value;
            }
        }
         private String ClaveFtp
         {
             get
             {
                 String clave = String.Empty;
                 if (ViewState["ClaveFtp"] != null)
                     clave = Convert.ToString(ViewState["ClaveFtp"]);
                 return clave;
             }

             set
             {
                 ViewState["ClaveFtp"] = value;
             }
         }
         private String CodigoBanco
         {
             get
             {
                 String codigo = String.Empty;
                 if (ViewState["CodigoBanco"] != null)
                     codigo = Convert.ToString(ViewState["CodigoBanco"]);
                 return codigo;
             }

             set
             {
                 ViewState["CodigoBanco"] = value;
             }
         }
         private String ArchivodeSalida
         {
             get
             {
                 String codigo = String.Empty;
                 if (ViewState["ArchivodeSalida"] != null)
                     codigo = Convert.ToString(ViewState["ArchivodeSalida"]);
                 return codigo;
             }

             set
             {
                 ViewState["ArchivodeSalida"] = value;
             }
         }
         private String CodigoCuenta
         {
             get
             {
                 String codigo = String.Empty;
                 if (ViewState["CodigoCuenta"] != null)
                     codigo = Convert.ToString(ViewState["CodigoCuenta"]);
                 return codigo;
             }

             set
             {
                 ViewState["CodigoCuenta"] = value;
             }
         }
         private String NombreCuenta
         {
             get
             {
                 String nombre = String.Empty;
                 if (ViewState["NombreCuenta"] != null)
                     nombre = Convert.ToString(ViewState["NombreCuenta"]);
                 return nombre;
             }

             set
             {
                 ViewState["NombreCuenta"] = value;
             }
         }
         #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void ddlTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnEnviar.Enabled = false;
            this.txbRemitente.Text = String.Empty;
            this.txbCorreoControl.Enabled = this.LtbCorreoControl.Enabled = this.txbRemitente.Enabled = false;
            limpiar(); 

            if (ddlTipoArchivo.SelectedIndex != 0)
            {
                ddlNombreCuenta.DataSource = new BancoLN().consultarBancosAsobancaria(ddlTipoArchivo.SelectedValue);
                ddlNombreCuenta.DataTextField = "Description";
                ddlNombreCuenta.DataValueField = "ID_CUENTA_BANCO";
                ddlNombreCuenta.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlNombreCuenta);
                ddlNombreCuenta.Enabled = true;
            }
            else
            {
                ddlNombreCuenta.Items.Clear();
                ddlNombreCuenta.Enabled = false;
            }

            UtilidadesWeb.ajustarMensaje(lbEstado, "Sasas", TipoMensaje.Informacion);
        }

        protected void ddlNombreCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNombreCuenta.SelectedIndex == 0)
            {
                this.btnEnviar.Enabled = false;
                this.txbRemitente.Text = String.Empty;
                this.txbCorreoControl.Enabled = this.LtbCorreoControl.Enabled = this.txbRemitente.Enabled = false;
                limpiar();
            }
            else
            {
                this.txbCorreoControl.Enabled = this.LtbCorreoControl.Enabled = this.txbRemitente.Enabled = true;
                buscar();
            }            

        }

        private void buscar()
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco(); // OBJETO QUE SE USARA PARA CARGAR DATOS EN EL FORMULARIO
            EN.Tablas.Banco objB1 = new EN.Tablas.Banco(); // OBJETO QUE SERA ENVIADO AL SERVICIO WEB
            BancoLN objBancoLN = new BancoLN();
            objB.pIdCuentaBanco = ddlNombreCuenta.SelectedValue;
            objB.pTipoProceso = ddlTipoArchivo.SelectedValue;

            List<EN.Tablas.Banco> listaB = objBancoLN.consultar(objB);
            if (listaB.Count > 0)
            {
                objB = listaB[0];
                objB1 = objB;

                if (objB.pFtp == true)
                {
                    rbFTP.Checked = true;
                    rbCorreo.Checked = false;
                }
                else
                { rbCorreo.Checked = true;
                rbFTP.Checked = false;
                }

                CodigoBanco = objB.pCodigoBanco; 
                OidRutaSalida = objB.pRutaArchivosSalida.Value;
                txbUrlFtp.Text = objB.pUrlFtp;
                txbUsuarioFtp.Text = objB.pUsuarioFtp;
                this.txbClaveFtp.Attributes.Add("Value", objB.pClave);
                ClaveFtp = objB.pClave;
              
                string[] CorreoControlB = objB.pCorreoControl.Split(';');
                string[] CorreoEnvioB = objB.pCorreoEnvio.Split(';');
                this.LtbCorreoControl.Items.Clear();
                this.LtbCorreoEnvio.Items.Clear();
                foreach (string cc in CorreoControlB)
                {
                    if (string.IsNullOrEmpty(cc)) break;
                    this.LtbCorreoControl.Items.Add(cc);
                }
                foreach (string ce in CorreoEnvioB)
                {
                    if (string.IsNullOrEmpty(ce)) break;
                    this.LtbCorreoEnvio.Items.Add(ce);
                }
                this.txbRemitente.Text = objB.pRemitente;  

                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                objRuta.pOid = OidRutaSalida;
                txbArchivoSalidaAso.Text = objRutaLN.consultar(objRuta)[0].pRuta;
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la cuenta bancaria " + objB.pNombreCuenta + ".", TipoMensaje.Informacion);
                this.btnEnviar.Enabled = true; 
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No Se ha encontrado la cuenta bancaria", TipoMensaje.Error);
                this.btnEnviar.Enabled = false; 
            }
            this.txbArchivoSalidaAso.Enabled = this.rbFTP.Enabled = this.rbCorreo.Enabled = true;
            comprobarFTP(true);
        }
               
        protected void imgAgregarCorreoControl_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txbCorreoControl.Text != string.Empty)
            {
                this.LtbCorreoControl.Items.Add(this.txbCorreoControl.Text);
                this.txbCorreoControl.Text = string.Empty;
            }
        }

        protected void imgBorrarCorreoControl_Click(object sender, ImageClickEventArgs e)
        {
            if (this.LtbCorreoControl.SelectedIndex != -1)
            {
                this.LtbCorreoControl.Items.Remove(this.LtbCorreoControl.SelectedItem);
            }
        }

        protected void imgAgregarCorreoEnvio_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txbCorreoEnvio.Text != string.Empty)
            {
                this.LtbCorreoEnvio.Items.Add(this.txbCorreoEnvio.Text);
                this.txbCorreoEnvio.Text = string.Empty;
            }
        }

        protected void imgBorrarCorreoEnvio_Click(object sender, ImageClickEventArgs e)
        {
            if (this.LtbCorreoEnvio.SelectedIndex != -1)
            {
                this.LtbCorreoEnvio.Items.Remove(this.LtbCorreoEnvio.SelectedItem);
            }
        }

        private void limpiar()
        {
            UtilidadesWeb.ajustarMensaje(lbEstado, String.Empty , TipoMensaje.Informacion);
            OidRutaSalida = 0;
            this.txbUrlFtp.Text = this.txbUsuarioFtp.Text 
                           = this.txbArchivoSalidaAso.Text = this.txbCorreoControl.Text
                           = this.txbCorreoEnvio.Text = String.Empty;
            this.LtbCorreoControl.Items.Clear();
            this.LtbCorreoEnvio.Items.Clear();
            this.txbUrlFtp.Enabled = this.txbUsuarioFtp.Enabled = this.txbClaveFtp.Enabled
                           = this.txbArchivoSalidaAso.Enabled = this.txbCorreoEnvio.Enabled 
                           = this.LtbCorreoControl.Enabled
                           = this.LtbCorreoEnvio.Enabled
                           = this.rbFTP.Enabled = this.rbCorreo.Enabled = false;
            this.rbFTP.Checked = false;
            this.rbCorreo.Checked = false;
            this.txbClaveFtp.Attributes.Clear(); 
        }

        private void comprobarFTP(bool esConsulta)
        {
            if (this.rbFTP.Checked == true)
            {
                this.txbUrlFtp.Enabled = this.txbClaveFtp.Enabled = this.txbUsuarioFtp.Enabled = true;
                this.txbCorreoControl.Text = this.txbCorreoEnvio.Text = String.Empty;
                this.txbCorreoEnvio.Enabled = this.LtbCorreoEnvio.Enabled = false;              
                this.LtbCorreoEnvio.Items.Clear();
                this.txbUrlFtp.ValidationGroup = this.txbClaveFtp.ValidationGroup = this.txbUsuarioFtp.ValidationGroup
                     = this.rfvUrlFtp.ValidationGroup = this.rfvClaveFtp.ValidationGroup = this.rfvUsuarioFtp.ValidationGroup = "1";                
            }
            else
            {
                this.txbUrlFtp.Enabled = this.txbClaveFtp.Enabled = this.txbUsuarioFtp.Enabled = false;
                this.txbCorreoEnvio.Enabled = this.LtbCorreoEnvio.Enabled = true;                
                this.txbUrlFtp.Text = this.txbUsuarioFtp.Text = String.Empty;
                this.txbClaveFtp.Attributes.Clear(); 
                this.txbUrlFtp.ValidationGroup = this.txbClaveFtp.ValidationGroup = this.txbUsuarioFtp.ValidationGroup
                    = this.rfvUrlFtp.ValidationGroup = this.rfvClaveFtp.ValidationGroup = this.rfvUsuarioFtp.ValidationGroup = "10";               
            }
        }

        protected void chbEsFTP_CheckedChanged(object sender, EventArgs e)
        {           
            comprobarFTP(false);
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {                
            ArrayList correo = new ArrayList();
            ArrayList correoe = new ArrayList();

            foreach (object correos in LtbCorreoControl.Items)
            {
                correo.Add(Convert.ToString(correos));
            }

            foreach (object correos in LtbCorreoEnvio.Items)
            {
                correoe.Add(Convert.ToString(correos));
            }
            String[] ArCorreo = (String[])correo.ToArray(typeof(String));
            String[] ArCorreoe = (String[])correoe.ToArray(typeof(String));

            CodigoCuenta = this.ddlNombreCuenta.SelectedValue;
            NombreCuenta = this.ddlNombreCuenta.SelectedItem.ToString();
            ArchivodeSalida = this.txbArchivoSalidaAso.Text;
            String mens = String.Empty;
     
            if (this.ddlTipoArchivo.SelectedValue.Equals("ABF1"))
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "Servicio Asobancaria", TipoMensaje.Informacion);
                try
                {
                  //  ServicioAsoBancaria.AsoBancaria Guardar = new ServicioAsoBancaria.AsoBancaria();
                  //  mens = Guardar.ServicioAso(NombreCuenta, CodigoCuenta, ArchivodeSalida, this.rbFTP.Checked,
                  //                                this.rbFTP.Checked, this.txbUrlFtp.Text, this.txbUsuarioFtp.Text,
                  //                                this.txbClaveFtp.Text, ArCorreo, ArCorreoe, CodigoBanco, this.txbRemitente.Text,
                  //                                HttpContext.Current.User.Identity.Name, this.ddlTipoArchivo.SelectedValue);
                  //UtilidadesWeb.ajustarMensaje(lbEstado, mens, TipoMensaje.Informacion);
                }
                catch
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Error al llamar el servicio", TipoMensaje.Error);
                }
            }
         
            //try
            //{
                                
            //    mens = ServicioPagosOnline(NombreBanco, CodigoBanco, ArchivodeSalida, correo,
            //                             CodigoTransito, "0123XX456", "1", this.txbRemitente.Text);
            //    UtilidadesWeb.ajustarMensaje(lbEstado, mens, TipoMensaje.Informacion);
            //}
            //catch
            //{
            //    UtilidadesWeb.ajustarMensaje(lbEstado, "Error al llamar el servicio", TipoMensaje.Error);
            //}
            

        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            this.txbRemitente.Text = this.Remitente.ToString();
            comprobarFTP(false);
        }

        
    }
} 
            