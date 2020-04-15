using System;
using System.Web.UI;
using Bancos.EN;
using Bancos.LN.Utilidades;
using Bancos.PS.Codigo;

namespace Bancos.PS.Modulos.Administracion
{
    public partial class CadenaCX : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                inicializarComponentes();
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardarDatos();
        }

        private void inicializarComponentes()
        {
            txbCadenaCX.Attributes.Add("onkeypress", "validarCaracteres(this, 200);");
            txbCadenaCX.Attributes.Add("onkeyup", "validarCaracteres(this, 200);");
            cargarDatosIniciales();
        }

        private void cargarDatosIniciales()
        {
            txbCadenaCX.Text = leerXML("A");
            txbClave.Text = leerXML("B");
            txbCorreo.Text = leerXML("C");
            txbDominio.Text = leerXML("D");
            txbServidor.Text = leerXML("E");
            txbServidorDA.Text = leerXML("F");
            txbUsuario.Text = leerXML("G");
        }

        private void guardarDatos()
        {
            escribirXML("A", txbCadenaCX.Text.Trim());
            if (!String.IsNullOrEmpty(txbClave.Text.Trim()))
                escribirXML("B", txbClave.Text.Trim());
            escribirXML("C", txbCorreo.Text.Trim());
            escribirXML("D", txbDominio.Text.Trim());
            escribirXML("E", txbServidor.Text.Trim());
            escribirXML("F", txbServidorDA.Text.Trim());
            escribirXML("G", txbUsuario.Text.Trim());

            UtilidadesWeb.ajustarMensaje(lbEstado, "Datos guardados.", TipoMensaje.Informacion);
        }

        private String leerXML(String Campo)
        {
            String respuesta = String.Empty;

            CamposXML objCampos = new CamposXML();
            objCampos.pTabla = "BD";
            objCampos.pCampo = Campo;

            LectorXML objLector = new LectorXML();
            objLector.RutaXML = Server.MapPath("~") + "\\Modulos\\XML\\Configuracion.xml";
            respuesta = objLector.leerDatosXML(objCampos);

            return respuesta;
        }

        private void escribirXML(String Campo, String Valor)
        {
            CamposXML objCampos = new CamposXML();
            objCampos.pTabla = "BD";
            objCampos.pCampo = Campo;
            objCampos.pValor = Valor;

            String valorOriginal = leerXML(Campo);

            if (!valorOriginal.Equals(Valor))
            {
                LectorXML objLector = new LectorXML();
                objLector.RutaXML = Server.MapPath("~") + "\\Modulos\\XML\\Configuracion.xml";
                objLector.modificarXML(objCampos);
            }
        }
    }
}