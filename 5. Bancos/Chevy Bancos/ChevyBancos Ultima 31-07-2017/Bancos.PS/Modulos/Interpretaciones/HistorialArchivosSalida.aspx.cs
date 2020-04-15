using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bancos.EN;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;
using System.Data;
using System.IO;
using Bancos.LN.Utilidades;

namespace Bancos.PS.Modulos.Interpretaciones
{
    public partial class HistorialArchivosSalida : System.Web.UI.Page
    {
        
        String nombreArchivo = String.Empty; // CONTIENE NOMBRE DEL ARCHIVO ASOBANCARIA
        String Directorio = String.Empty; // CONTIENE DIRECTORIO DONDE SE GUARDARA EL ARCHIVO ASOBANCARIA     
        static StreamWriter sw ;

        private String ArchivoSalida
        {
            get
            {
                String codigo = String.Empty;
                if (ViewState["ArchivoSalida"] != null)
                    codigo = Convert.ToString(ViewState["ArchivoSalida"]);
                return codigo;
            }

            set
            {
                ViewState["ArchivoSalida"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }

        protected void ddlNombreCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.ddlFechas.Items.Clear();
            this.ddlConsecutivo.Items.Clear();
            this.ddlConsecutivo.Enabled = false;
            this.btnGenerar.Enabled = false;
            if (ddlNombreCuenta.SelectedIndex == 0)
            {
                this.ddlFechas.Enabled = this.txbArchivoSalida.Enabled = false;
                this.txbArchivoSalida.Text = String.Empty;
            }
            else
            {
                this.ddlFechas.Enabled = this.txbArchivoSalida.Enabled = true;
                ddlFechas.DataSource = new HistorialArchivosSalidaLN().consultarFechasXBanco(this.ddlNombreCuenta.Text, this.ddlTipoArchivo.Text);
                ddlFechas.DataTextField = "FECHA";
                ddlFechas.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlFechas);
                String rutaSalida = new BancoLN().consultarRutaSalida(this.ddlNombreCuenta.SelectedValue,ddlTipoArchivo.SelectedValue);
                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                objRuta.pOid = Convert.ToInt32(rutaSalida);
                this.txbArchivoSalida.Text = objRutaLN.consultar(objRuta)[0].pRuta;
            }

        }

        protected void ddlTipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.ddlFechas.Items.Clear();
            this.ddlConsecutivo.Items.Clear();
            this.txbArchivoSalida.Text = String.Empty;
            this.ddlConsecutivo.Enabled = this.ddlFechas.Enabled = this.txbArchivoSalida.Enabled
                                        = this.btnGenerar.Enabled = false;

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

        }

        protected void ddlFechas_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnGenerar.Enabled = false;
            this.ddlConsecutivo.Items.Clear();
            if (ddlFechas.SelectedIndex == 0)
            {        
                this.ddlConsecutivo.Enabled = false;
            }
            else
            {
                this.ddlConsecutivo.Enabled  = true;
                ddlConsecutivo.DataSource = new HistorialArchivosSalidaLN().consultarConsecutivoXBanco
                                            (this.ddlNombreCuenta.Text, this.ddlTipoArchivo.Text, this.ddlFechas.Text);
                ddlConsecutivo.DataTextField = "CONSECUTIVO";
                ddlConsecutivo.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(ddlConsecutivo);
            }
        }

        protected void ddlConsecutivo_SelectedIndexChanged(object sender, EventArgs e)
        {          
            if (ddlConsecutivo.SelectedIndex == 0)
            {
                this.btnGenerar.Enabled = false;
            }
            else
            {
                this.btnGenerar.Enabled = true;                
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            String men = String.Empty; 
            if (this.ddlTipoArchivo.SelectedIndex == 1)
            {
                try
                {
                    men = generarArchivo();
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Archivo generado correctamente", TipoMensaje.Informacion);
                }
                catch
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Ocurrio un error al crear el archivo", TipoMensaje.Error);
                }
            }
            else if (this.ddlTipoArchivo.SelectedIndex == 2)
            {
                try
                {
                    men = generarArchivo();
                    UtilidadesWeb.ajustarMensaje(lbEstado, men, TipoMensaje.Informacion);                    
                }
                catch
                {
                    UtilidadesWeb.ajustarMensaje(lbEstado, "Ocurrio un error al crear el archivo", TipoMensaje.Error);
                }
            }
        }

        private String generarArchivo()
        {
            Logs objL = new Logs();
            try
            {
            ArchivoSalida = this.txbArchivoSalida.Text;
            nombreArchivo = String.Concat("COPIA_" + this.ddlNombreCuenta.Text, "_", this.ddlFechas.Text.Replace("/", ""),
                                          this.ddlConsecutivo.Text, "_", writeMilitaryTime(DateTime.Now), ".txt");

            if (this.ddlTipoArchivo.SelectedIndex == 1)
            {
                Directorio = ArchivoSalida + "ArchivosAsobancaria\\";
            }
            else if (this.ddlTipoArchivo.SelectedIndex == 2)
            {
                Directorio = ArchivoSalida + "ArchivosTelefonoRojo\\";
            }

            //SE CREA LA CARPETA DONDE VAN A QUEDAR LOS ARCHIVOS SINO EXISTE
            if (!Directory.Exists(Directorio))
            {
                System.IO.Directory.CreateDirectory(Directorio);
            }
            
            sw = new StreamWriter(Directorio + nombreArchivo, false);

            DataTable tabla = new HistorialArchivosSalidaLN().consultarLineasConsecutivo
                              (this.ddlNombreCuenta.Text, this.ddlTipoArchivo.Text, this.ddlFechas.Text, this.ddlConsecutivo.Text);
            foreach (DataRow fila in tabla.Rows)
            {
                sw.WriteLine(Convertidor.aCadena(fila["LINEAS_ARCHIVO"]));                
            }           
            sw.Close();

            objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
            objL.pUsuario = HttpContext.Current.User.Identity.Name;
            objL.pDetalle = this.ddlNombreCuenta.SelectedItem.Text + " : Archivo " + ddlTipoArchivo.SelectedItem.Text + " fue generado correctamente";
            objL.pTipoArchivo = ddlTipoArchivo.Text;
            objL.pTipoProceso = "GEN";
            new LogsLN().insertar(objL);                  
            return "Archivo fue generado correctamente";

            }

            catch (Exception ex)
            {
                objL.pFecha = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("H:mm:ss"));
                objL.pUsuario = HttpContext.Current.User.Identity.Name;
                objL.pDetalle = this.ddlNombreCuenta.SelectedItem.Text + " , Archivo " + ddlTipoArchivo.SelectedItem.Text + " : " + ex.Message;
                objL.pTipoArchivo = ddlTipoArchivo.Text;
                objL.pTipoProceso = "GEN";
                new LogsLN().insertar(objL);
                return ex.Message;
            }


        }

        private String writeMilitaryTime(DateTime date)
        {
            string value = date.ToString("HHmm");
            return value;
        }

       

        
    }
}