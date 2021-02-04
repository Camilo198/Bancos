using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Bancos.EN;
using Bancos.EN.Tablas;
using Bancos.LN.Consultas;
using Bancos.PS.Codigo;


//*************
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using Bancos.LN.Utilidades;
using Bancos.EN.Definicion;
using Bancos.PS.Servicios.Ftp;
using Bancos.PS.Servicios.Correo;
using Bancos.PS.Servicios.Archivos;
using Bancos.PS.Servicios;
//************

using System.Data.Common;
using System.Data.OleDb;

namespace Bancos.PS.Modulos.Banco
{
    public partial class Banco : System.Web.UI.Page
    {
        String Remitente = "SistemaAsobancaria@chevyplan.com.co";
    
        #region "Propiedades: OidRutaEntrada, OidRutaSalida, CodigoBanco, ClaveFTP, BancosEncontrados"

        private int OidRutaEntrada
        {
            get
            {
                int oidRE = 0;
                if (ViewState["OidRutaEntrada"] != null)
                    oidRE = Convert.ToInt32(ViewState["OidRutaEntrada"]);
                return oidRE;
            }

            set
            {
                ViewState["OidRutaEntrada"] = value;
            }
        }

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

        private int OidRutaSalidaEpicor
        {
            get
            {
                int oidRE = 0;
                if (ViewState["OidRutaSalidaEpicor"] != null)
                    oidRE = Convert.ToInt32(ViewState["OidRutaSalidaEpicor"]);
                return oidRE;
            }

            set
            {
                ViewState["OidRutaSalidaEpicor"] = value;
            }
        }

        private int RecauFact
        {
            get
            {
                int oidRE = 0;
                if (ViewState["RecauFact"] != null)
                    oidRE = Convert.ToInt32(ViewState["RecauFact"]);
                return oidRE;
            }

            set
            {
                ViewState["RecauFact"] = value;
            }
        }

        private String ClaveFTP
        {
            get
            {
                String clave = String.Empty;
                if (ViewState["ClaveFTP"] != null)
                    clave = Convert.ToString(ViewState["ClaveFTP"]);
                return clave;
            }

            set
            {
                ViewState["ClaveFTP"] = value;
            }
        }

        private int IdCuenta
        {
            get
            {
                int idC = 0;
                if (ViewState["IdCuenta"] != null)
                    idC = Convert.ToInt32(ViewState["IdCuenta"]);
                return idC;
            }

            set
            {
                ViewState["IdCuenta"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
           //AQUI   

           String mens;
           try
           {
             ArrayList CorreosControl = new ArrayList();
            //    //CorreosControl.Add("marina.ramirez@Chevyplan.com.co");
             Bancos.PS.Servicios.PagosOnline procesoPayOnline = new Bancos.PS.Servicios.PagosOnline();
             mens = procesoPayOnline.ServicioPagosOnline("NombreCuenta", "00015", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\PagosOnline\\Recibidos\\", CorreosControl,
                                                         "00", "16525785990", "00", "Sistemas@chevyplan.com.co",
                                                         "TAREA PROGRAMADA", "POL1");


             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("TCR", "39", "00019", @"\\sbogche016v\ARCHPLANOS\Pagos\TC\Falabella\Recibidos\", @"\\sbogche016v\ARCHPLANOS\Pagos\TC\Falabella\Historico\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00", "0", "00", "marina.ramirez", @"\\sbogche016v\ARCHPLANOS\Pagos\TC\Falabella\Procesados\", "TCR1");


//Linea Principal Descomentariar con paso a produccion
             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("NombreCuenta", "00018", "00018", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\TC\\Exito\\Recibidos\\", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\TC\\Exito\\Procesados\\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00018", "16525785990", "1", "marina.ramirez", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\TC\\Exito\\Procesados\\", "TCR1");

             //Bancos.PS.Servicios.Recaudo Recaudo = new Bancos.PS.Servicios.Recaudo();
             //mens = Recaudo.ServicioRecaudoDiario("Lugar Pago 20", "49", "00012", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Bogota\\Parciales\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Bogota\\Recibidos\\", CorreosControl,
             //"Sistemas@chevyplan.com.co", "00", "1", "00", "marina.ramirez", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\Recaudo\\Bogota\\SICO\\", "ABR1");

             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("NombreCuenta", "00017", "00017", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Colsubsidio\\Recibidos\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Colsubsidio\\Procesados\\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00017", "16525785990", "1", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Colsubsidio\\Procesados\\", "TCR1");


             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("NombreCuenta", "6", "00010", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Davivienda\\Recibidos\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Davivienda\\Historico\\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00", "44", "00", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Davivienda\\Procesados\\", "TCR1");


             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("TCR", "6", "00010", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\TC\\Davivienda\\Recibidos\\", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\TC\\Davivienda\\Procesados\\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00", "44", "00", "marina.ramirez", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\TC\\Davivienda\\Historico\\", "TCR1");


             //Bancos.PS.Servicios.Recaudo Recaudo = new Bancos.PS.Servicios.Recaudo();
             //mens = Recaudo.ServicioRecaudoDiario("NombreCuenta", "00010", "00010", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\Recaudo\\Davivienda\\Parciales\\", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\Recaudo\\Davivienda\\Procesados\\", CorreosControl,
             //    "Sistemas@chevyplan.com.co", "00010", "16525785990", "1", "marina.ramirez", "\\\\sbogche020v\\ARCHPLANOS\\Pagos\\Recaudo\\Davivienda\\Procesados\\", "ABR1");

             //Bancos.PS.Servicios.Recaudo Recaudo = new Bancos.PS.Servicios.Recaudo();
            // mens = Recaudo.ServicioRecaudoDiario("NombreCuenta", "00012", "00012", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Bogota\\Parciales\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Bogota\\Procesados\\", CorreosControl,
             //    "Sistemas@chevyplan.com.co", "00012", "16525785990", "1", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Bogota\\Procesados\\", "ABR1");

             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("45", "14", "00015", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Bancolombia\\Recibidos\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Bancolombia\\Historico\\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00", "45", "00", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Bancolombia\\Procesados\\", "TCR1");

             //Bancos.PS.Servicios.TarjetasCredito TajetasCredito = new Bancos.PS.Servicios.TarjetasCredito();
             //mens = TajetasCredito.ServicioTarjetasCredito("45", "14", "00015", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Bancolombia\\Recibidos\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Bancolombia\\Historico\\", CorreosControl,
             //                                        "Sistemas@chevyplan.com.co", "00", "45", "00", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\TC\\Bancolombia\\Procesados\\", "TCR1");

             //Bancos.PS.Servicios.Recaudo Recaudo = new Bancos.PS.Servicios.Recaudo();
             //mens = Recaudo.ServicioRecaudoDiario("NombreCuenta", "00022", "00022", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\GNB\\Parciales\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\GNB\\Procesados\\", CorreosControl,
             //    "Sistemas@chevyplan.com.co", "00022", "16525785990", "1", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\GNB\\Procesados\\", "ABR1");

             //Bancos.PS.Servicios.Recaudo Recaudo = new Bancos.PS.Servicios.Recaudo();
             //mens = Recaudo.ServicioRecaudoDiario("NombreCuenta", "00097", "00097", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Acreedores\\Recibidos\\", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Acreedores\\Procesados\\", CorreosControl,
             //    "Sistemas@chevyplan.com.co", "00097", "16525785990", "1", "marina.ramirez", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Recaudo\\Acreedores\\Procesados\\", "ABR1");

           }
           catch (Exception ex)
           {

           }
            
            ////////
            
            if (!IsPostBack)
            {
                inicializarComponentes();
                Valor objV = new Valor();
                objV.pTabla = 8;
                ddlTipoCuenta.DataSource = new ValorLN().consultar(objV);
                ddlTipoCuenta.DataTextField = "pDescripcion";
                ddlTipoCuenta.DataValueField = "pCodigo";
                ddlTipoCuenta.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoCuenta);

                ddlCuentas.DataSource = new LupaLN().consultar(32000);
                ddlCuentas.DataTextField = "pLupaNombre";
                ddlCuentas.DataValueField = "pLupaCodigo";
                ddlCuentas.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlCuentas);


                this.ddlTipoProceso.DataSource = new ArchivoAsobancariaLN().consultar(new ArchivoAsobancaria());
                this.ddlTipoProceso.DataTextField = "pNombre";
                this.ddlTipoProceso.DataValueField = "pOid";
                this.ddlTipoProceso.DataBind();
                UtilidadesWeb.agregarSeleccioneDDL(this.ddlTipoProceso);


                this.txbRemitente.Text = this.Remitente.ToString();
                                
            }

        }

        protected void imgBtnNuevo_Click(object sender, ImageClickEventArgs e)
        {
            limpiar(true);
        }

        protected void imgBtnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            guardar();
        }

        protected void rb_CheckedChanged(object sender, EventArgs e)
        {
            this.txbRemitente.Text = this.Remitente.ToString();
            comprobarFTP(false);// OJO
        }

        private void inicializarComponentes()
        {
            lbEstado.Text = "Listo";
        }

        private void limpiar(bool todo)
        {
            if (todo)
                this.ddlCuentas.SelectedIndex = 0;

            UtilidadesWeb.ajustarMensaje(lbEstado, "Listo...", TipoMensaje.Informacion);
            OidRutaEntrada = OidRutaSalida = OidRutaSalidaEpicor = IdCuenta = 0;
            this.txbArchivoAsobancaria.Text = this.txbArchivoBanco.Text = this.txbArchivoEpicor.Text =
            this.txbCodBanco.Text = this.txbUrlFTP.Text = this.txbUsuarioFTP.Text =
            this.txbNumCuenta.Text = this.txbCodigoCuenta.Text = String.Empty;
            this.chbEstaActivo.Checked = false;
            this.rbFTP.Checked = true;
            this.rbCorreo.Checked = false;
            this.LtbCorreoControl.Items.Clear();
            comprobarFTP(false);
            this.txbClave.Attributes.Clear();
            this.ddlTipoCuenta.SelectedIndex = 0;

        }

        #region "Funcion: guardar()"
        private void guardar()
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            if (!String.IsNullOrEmpty(this.txbClave.Text.Trim()))
            {
                objB.pClave = this.txbClave.Text.Trim();
            }
            else
            {
                objB.pClave = ClaveFTP;
            }
            objB.pId = IdCuenta;
            objB.pIdCuentaBanco = this.ddlCuentas.SelectedValue;
            objB.pTipoProceso = this.ddlTipoProceso.SelectedValue;
            objB.pFtp = this.rbFTP.Checked;
            objB.pRutaArchivosEntrada = OidRutaEntrada;
            objB.pRutaArchivosSalida = OidRutaSalida;
            objB.pRutaArchivosSalidaEpicor = OidRutaSalidaEpicor;
            objB.pActivo = this.chbEstaActivo.Checked;

            String CorreoControlG = string.Empty;
            foreach (object itemCorreoC in this.LtbCorreoControl.Items)
            {
                CorreoControlG = CorreoControlG + itemCorreoC + ";";
            }
            String CorreoEnvioG = string.Empty;
            foreach (object itemCorreoE in this.LtbCorreoEnvio.Items)
            {
                CorreoEnvioG = CorreoEnvioG + itemCorreoE + ";";
            }
            objB.pCorreoControl = CorreoControlG;
            objB.pCorreoEnvio = CorreoEnvioG;
            objB.pRemitente = this.txbRemitente.Text;

            if (String.IsNullOrEmpty(this.txbUrlFTP.Text.Trim()))
                objB.pUrlFtp = " ";
            else
                objB.pUrlFtp = this.txbUrlFTP.Text.Trim();

            if (String.IsNullOrEmpty(this.txbUsuarioFTP.Text.Trim()))
                objB.pUsuarioFtp = " ";
            else
                objB.pUsuarioFtp = this.txbUsuarioFTP.Text.Trim();

            BancoLN objBancoLN = new BancoLN();
            RutaLN objRutaLN = new RutaLN();
            int valor = 0;
            if (IdCuenta == 0)
            {
                Ruta objRutas = new Ruta();
                objRutas.pRuta = this.txbArchivoBanco.Text.Trim();
                objB.pRutaArchivosEntrada = OidRutaEntrada = objRutaLN.insertar(objRutas);

                objRutas = new Ruta();
                objRutas.pRuta = this.txbArchivoAsobancaria.Text.Trim();
                objB.pRutaArchivosSalida = OidRutaSalida = objRutaLN.insertar(objRutas);

                objRutas = new Ruta();
                objRutas.pRuta = this.txbArchivoEpicor.Text.Trim();
                objB.pRutaArchivosSalidaEpicor = OidRutaSalidaEpicor = objRutaLN.insertar(objRutas);

                valor = objBancoLN.insertar(objB);
            }
            else
            {
                Ruta objRutas = new Ruta();
                objRutas.pOid = OidRutaEntrada;
                objRutas.pRuta = this.txbArchivoBanco.Text.Trim();
                objRutaLN.actualizar(objRutas);

                objRutas = new Ruta();
                objRutas.pOid = OidRutaSalida;
                objRutas.pRuta = this.txbArchivoAsobancaria.Text.Trim();
                objRutaLN.actualizar(objRutas);

                objRutas = new Ruta();
                objRutas.pOid = OidRutaSalidaEpicor;
                objRutas.pRuta = this.txbArchivoEpicor.Text.Trim();
                objRutaLN.actualizar(objRutas);

                valor = objBancoLN.actualizar(objB);
            }

            if (valor > 0)
            {
                limpiar(true);
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha guardado la informacion correctamente.", TipoMensaje.Informacion);
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "No Se ha guardado la informacion!!!", TipoMensaje.Error);
            }
        }
        #endregion

        private int insertarRuta(String ruta)
        {
            int valor = 0;
            Ruta objR = new Ruta();
            objR.pRuta = ruta;
            valor = new RutaLN().insertar(objR);
            return valor;
        }

        #region "Funcion: buscar()"
        private void buscar()
        {
            EN.Tablas.Banco objB = new EN.Tablas.Banco();
            BancoLN objBancoLN = new BancoLN();

            objB.pTipoProceso = this.ddlTipoProceso.SelectedValue;
            objB.pIdCuentaBanco = this.ddlCuentas.SelectedValue;
            List<EN.Tablas.Banco> listaB = objBancoLN.consultar(objB);
            if (listaB.Count > 0)
            {
                objB = listaB[0];
                this.txbClave.Attributes.Add("Value", objB.pClave);
                ClaveFTP = objB.pClave;
                IdCuenta = objB.pId.Value;
                if (objB.pFtp == true)
                {
                    this.rbFTP.Checked = true;
                    this.rbCorreo.Checked = false;
                }
                else
                {
                    this.rbCorreo.Checked = true;
                    this.rbFTP.Checked = false;
                }

                OidRutaEntrada = objB.pRutaArchivosEntrada.Value;
                OidRutaSalida = objB.pRutaArchivosSalida.Value;
                OidRutaSalidaEpicor = objB.pRutaArchivosSalidaEpicor.Value;
                this.txbUrlFTP.Text = objB.pUrlFtp;
                this.txbUsuarioFTP.Text = objB.pUsuarioFtp;
                this.chbEstaActivo.Checked = objB.pActivo.Value;

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
                objRuta.pOid = OidRutaEntrada;
                this.txbArchivoBanco.Text = objRutaLN.consultar(objRuta)[0].pRuta;

                objRuta.pOid = OidRutaSalida;
                this.txbArchivoAsobancaria.Text = objRutaLN.consultar(objRuta)[0].pRuta;

                objRuta.pOid = OidRutaSalidaEpicor;
                this.txbArchivoEpicor.Text = objRutaLN.consultar(objRuta)[0].pRuta;

                UtilidadesWeb.ajustarMensaje(lbEstado, "Se ha encontrado la cuenta bancaria " + this.ddlCuentas.SelectedItem + ".", TipoMensaje.Informacion);
            }
            else
            {
                UtilidadesWeb.ajustarMensaje(lbEstado, "Se puede crear la cuenta bancaria " + this.ddlCuentas.SelectedItem + ".", TipoMensaje.Informacion);
            }

            comprobarFTP(true);

        }
        #endregion

        private void comprobarFTP(bool esConsulta)
        {
            if (this.rbFTP.Checked == true)
            {
                this.txbUrlFTP.Enabled = this.txbClave.Enabled = this.txbUsuarioFTP.Enabled = true;
                this.txbCorreoControl.Text = this.txbCorreoEnvio.Text = String.Empty;
                this.txbCorreoEnvio.Enabled = this.LtbCorreoEnvio.Enabled = false;
                this.LtbCorreoEnvio.Items.Clear();
                this.txbUrlFTP.ValidationGroup = this.txbClave.ValidationGroup = this.txbUsuarioFTP.ValidationGroup
                    = this.rfvUrlFTP.ValidationGroup = this.rfvClave.ValidationGroup = this.rfvUsuarioFTP.ValidationGroup = "1";
            }
            else
            {
                this.txbUrlFTP.Enabled = this.txbClave.Enabled = this.txbUsuarioFTP.Enabled = false;
                this.txbCorreoEnvio.Enabled = this.LtbCorreoEnvio.Enabled = true;
                this.txbUrlFTP.Text = this.txbUsuarioFTP.Text = String.Empty;
                this.txbClave.Attributes.Clear();
                this.txbUrlFTP.ValidationGroup = this.txbClave.ValidationGroup = this.txbUsuarioFTP.ValidationGroup =
                this.rfvUrlFTP.ValidationGroup = this.rfvClave.ValidationGroup = this.rfvUsuarioFTP.ValidationGroup = "10";
            }
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

        protected void ddlCuentas_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpiar(false);
            if (ddlCuentas.SelectedIndex != 0)
            {
              // AQUI ServicioContrato.ChevyplanWS descuentas = new ServicioContrato.ChevyplanWS();
                DataSet informacionCuenta = new DataSet();
              // AQUI informacionCuenta = descuentas.Chevyplan_CuentasInformacion(ddlCuentas.SelectedValue);
                this.txbCodigoCuenta.Text = ddlCuentas.SelectedValue;
              // Aqui  this.ddlTipoCuenta.SelectedItem.Text = informacionCuenta.Tables[0].Rows[0].ItemArray[1].ToString();
              // Aqui this.txbCodBanco.Text = informacionCuenta.Tables[0].Rows[0].ItemArray[2].ToString();
              //  this.txbNumCuenta.Text = informacionCuenta.Tables[0].Rows[0].ItemArray[4].ToString();
                buscar();
            }
        }

        protected void ddlTipoProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            limpiar(true);
            if (ddlTipoProceso.SelectedIndex != 0)
                this.pnlDatos.Enabled = this.pnlFTP.Enabled = this.pnlRutas.Enabled = true;
            else
                this.pnlDatos.Enabled = this.pnlFTP.Enabled = this.pnlRutas.Enabled = false;
        }     

    }



}