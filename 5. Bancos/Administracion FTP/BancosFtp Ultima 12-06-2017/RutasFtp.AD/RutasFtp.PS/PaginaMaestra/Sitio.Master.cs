using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RutasFtp.EN;
using RutasFtp.LN.Utilidades;
using RutasFtp.PS.Codigo;

namespace RutasFtp.PS.PaginaMaestra
{
    public partial class Sitio : System.Web.UI.MasterPage
    {
        private List<ServicioITA.Modulo> ModulosUsuario
        {
            get
            {
                List<ServicioITA.Modulo> lista = null;
                if (Session["01ModulosUsuario"] != null)
                    lista = (List<ServicioITA.Modulo>)Session["01ModulosUsuario"];
                return lista;
            }

            set
            {
                Session["01ModulosUsuario"] = value;
            }
        }

        private ServicioITA.GrupoUsuario GrupoUsuario
        {
            get
            {
                ServicioITA.GrupoUsuario grupo = null;
                if (Session["02GrupoUsuario"] != null)
                    grupo = (ServicioITA.GrupoUsuario)Session["02GrupoUsuario"];
                return grupo;
            }

            set
            {
                Session["02GrupoUsuario"] = value;
            }
        }
              
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            LectorXML objLectorXML = new LectorXML();
            objLectorXML.RutaXML = Server.MapPath("~") + "\\Modulos\\XML\\Configuracion.xml";

            CamposXML objCamposXML = new CamposXML();
            objCamposXML.pTabla = "BD";

            objCamposXML.pCampo = "G";
            String usuario = objLectorXML.leerDatosXML(objCamposXML);

            objCamposXML.pCampo = "B";
            String clave = objLectorXML.leerDatosXML(objCamposXML);

            if (usuario.Equals(txbUsuario.Text.Trim()) && clave.Equals(txbClave.Text.Trim()))
            {
                Response.Redirect("~/Modulos/Administracion/CadenaCX.aspx");
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbInforme, "Usuario o<br />contraseña no validos!", TipoMensaje.Error);
                mpeLogin.Show();
            }
        }

        private void expandirNodoSeleccionado()
        {
            String valor = HttpUtility.UrlDecode(Request.QueryString["tv"]);

            if (!String.IsNullOrEmpty(valor))
            {
                TreeNode nodo = tvMenu.FindNode(valor);
                if (null != nodo)
                {
                    nodo.Text = "<b>" + nodo.Text + "</b>";
                    nodo.ImageUrl = "~/MarcaVisual/iconos/nodo_abierto.png";

                    while ((nodo.Parent != null) && (nodo.Parent.Depth != -1))
                    {
                        nodo = seleccionarPadre(nodo);
                        nodo.Expand();

                        if (nodo.Parent != null)
                        {
                            nodo.ImageUrl = "~/MarcaVisual/iconos/nodo_abierto.png";
                        }
                    }
                }
            }
        }

        private TreeNode seleccionarPadre(TreeNode nodo)
        {
            return nodo.Parent;
        }

        private void cargarMenu()
        {
            String usuario = String.Empty,
                app = ConfigurationManager.AppSettings["IdentificadorAPP"];
            try
            {
                tvMenu.Nodes.Clear();
                usuario = HttpContext.Current.User.Identity.Name.Split('\\')[1];

                ServicioITA.ServicioIntegrador objServicio = new ServicioITA.ServicioIntegrador();

                if (ModulosUsuario.Count == 0)
                {
                    ModulosUsuario.AddRange(objServicio.consultarModulos(usuario, app));
                }

                if (String.IsNullOrEmpty(GrupoUsuario.pApp))
                {
                    GrupoUsuario = objServicio.consultarGrupoUsuario(usuario, app);
                }

                objServicio.Dispose();
                llenarHijos(ModulosUsuario);
            }
            catch { }
        }

        private void llenarHijos(List<ServicioITA.Modulo> listaModulos)
        {
            if (listaModulos.Count > 0)
            {
                List<ServicioITA.Modulo> listaErrados = new List<ServicioITA.Modulo>();
                foreach (ServicioITA.Modulo modulo in listaModulos)
                {
                    if (String.IsNullOrEmpty(modulo.pRutaPadre))
                    {
                        TreeNode nodo = new TreeNode();
                        nodo.NavigateUrl = modulo.pUrlNodo;
                        nodo.Value = modulo.pValorModulo;
                        nodo.Text = modulo.pTextoNodo;
                        nodo.NavigateUrl = modulo.pUrlNodo;

                        tvMenu.Nodes.Add(nodo);
                    }
                    else
                    {
                        TreeNode nodoPadre = tvMenu.FindNode(modulo.pRutaPadre);

                        TreeNode nodo = new TreeNode();
                        if (!modulo.pUrlNodo.Equals("-"))
                            nodo.NavigateUrl = modulo.pUrlNodo;
                        nodo.Text = modulo.pTextoNodo;
                        nodo.Value = modulo.pValorModulo;

                        if (!String.IsNullOrEmpty(nodo.NavigateUrl))
                            nodo.NavigateUrl = modulo.pUrlNodo + "?tv=" + (modulo.pRutaPadre + "/" + modulo.pValorModulo).Replace("/", "%2f");

                        try
                        {
                            nodoPadre.ChildNodes.Add(nodo);
                        }
                        catch
                        {
                            listaErrados.Add(modulo);
                        }
                    }
                }
                llenarHijos(listaErrados);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ModulosUsuario = new List<ServicioITA.Modulo>();
                GrupoUsuario = new ServicioITA.GrupoUsuario();
            }

            if (RevisorBD.comprobarConectividad())
            {
                if (!IsPostBack)
                {
                    cargarMenu();
                    expandirNodoSeleccionado();
                    lbUsuario.Text = HttpContext.Current.User.Identity.Name;
                }

                //GrupoUsuario = PermisosUsuario.Find(mu => mu.pApp.Equals(ConfigurationManager.AppSettings["IdentificadorAPP"]));
                if (GrupoUsuario == null)
                    mpPermisos.Show();
            }
            else
            {
                lbUsuario.Text = HttpContext.Current.User.Identity.Name;
                String error = RevisorBD.Error;
                if (error.Length > 59)
                    error = error.Substring(0, 59);
                UtilidadesWeb.ajustarMensaje(lbInforme, error, TipoMensaje.Error);

                if (!Page.Title.Equals("FTP - Cadenas de Conexión y Otros"))
                    mpeLogin.Show();
            }
        }
    }
}