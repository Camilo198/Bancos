using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Procesos.LN.Consultas;
using Procesos.LN.Utilidades;
using Procesos.EN;
using Procesos.PS.Procesos;
using Procesos.EN.Tablas;
using System.Diagnostics;
using System.Threading;

using Procesos.AD.Administracion;
using Procesos.PS.Codigo;
using System.Configuration;

namespace Procesos.PS
{
    public partial class Form1 : Form
    {
        private wsenviocorreos.Service EnvioCorreo = new wsenviocorreos.Service();
        int opcion = 0;
        //opcion 1 = Nuevo
        //opcion 2 = Editar
        //opcion 3 = Eliminar
        String tiempo; // FECHA Y HORA QUE SE LE ENVIA AL PROCESO AL ACTUALIZAR O INSERTAR
        DateTime tiempoProceso; // LA FECHA Y HORA DEL PROCESO QUE ES INVOCADO
        DateTime tiempoServidor; // LA FECHA Y HORA DEL SERVIDOR   
        LogLN log = new LogLN(); // Logs de error

        private System.Windows.Forms.Timer tempCierra;
        private int contador = 12;
        private bool btnProcesos_Clicked = false;

        public Form1()
        {
            InitializeComponent();
            this.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.lbDesde.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            ConsultorXML prueba = new ConsultorXML();
            string resultado = prueba.encriptar(@"Data Source=SBOGCHE037V;Initial Catalog=Bancos;User ID=usuario_chevy;Password=Colombia*");
            prueba.leerCadenaConexion();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            //tiempo = Convert.ToString(this.dateTimePicker1.Value.ToString("yyyy-MM-dd") + " " + this.dateTimePicker2.Value.ToString("H:mm:ss"));
            tiempo = Convert.ToString(this.dateTimePicker1.Value.ToString("yyyy-dd-MM") + " " + this.dateTimePicker2.Value.ToString("H:mm:ss"));

            EN.Tablas.Tareas objEntidad = new EN.Tablas.Tareas();
            objEntidad.pNombreTarea = this.txbNombre.Text;
            objEntidad.pPeriodo = this.cmbPeriodo.Text;
            objEntidad.pInicio = tiempo;
            objEntidad.pProceso = this.txbProcedimiento.Text;
            if (cmbPeriodo.Text.Equals("Horas") || cmbPeriodo.Text.Equals("Minutos"))
                objEntidad.pTiempoIntervalo = this.nudHoras.Value.ToString();
            else
                objEntidad.pTiempoIntervalo = "0";
            objEntidad.pCorreoControl = this.txbCorreo.Text;

            TareaLN objTareasLN = new TareaLN();
            int valor = 0;

            if (opcion == 1)
            {
                valor = objTareasLN.insertar(objEntidad);
            }
            else if (opcion == 2)
            {
                objEntidad.pId = Convert.ToInt32(this.cmbProcesos.SelectedValue.ToString());
                valor = objTareasLN.actualizar(objEntidad);
            }

            if (valor > 0)
            {
                MessageBox.Show("El proceso se ha guardado con exito!!");
            }
            else
            {
                MessageBox.Show("Ocurrio un error al guardar el proceso");
            }
            limpiar();

        }

        private void cmbProcesos_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cargarInformacionProcesos();
        }

        private void cargarProcesos()
        {
            DataTable procesos = new TareaLN().consultar();
            this.cmbProcesos.DataSource = procesos;
            this.cmbProcesos.DisplayMember = "NombreTarea";
            this.cmbProcesos.ValueMember = "Id";
            cargarInformacionProcesos();
        }

        private void cargarInformacionProcesos()
        {
            if (this.tabControl1.SelectedIndex == 1)
            {
                if (this.cmbProcesos.Items.Count != 0)
                {
                    EN.Tablas.Tareas objEntidad = new EN.Tablas.Tareas();
                    EN.Tablas.Tareas objEntidad1 = new EN.Tablas.Tareas();
                    objEntidad.pId = Convert.ToInt32(this.cmbProcesos.SelectedValue.ToString());
                    objEntidad1 = new TareaLN().consultarProceso(objEntidad);
                    this.txbNombre.Text = objEntidad1.pNombreTarea;
                    this.txbProcedimiento.Text = objEntidad1.pProceso;
                    this.cmbPeriodo.Text = objEntidad1.pPeriodo;
                    this.txbCorreo.Text = objEntidad1.pCorreoControl;
                    mostrarIntervalo();
                    this.nudHoras.Value = Convert.ToDecimal(objEntidad1.pTiempoIntervalo);
                    this.dateTimePicker1.Text = objEntidad1.pInicio;
                    this.dateTimePicker2.Text = objEntidad1.pInicio;
                    this.gboxDatos.Enabled = false;
                    this.btnEditar.Enabled = this.btnEliminar.Enabled = true;
                }
            }
        }

        private void mostrarIntervalo()
        {
            if (cmbPeriodo.Text.Equals("Horas") || cmbPeriodo.Text.Equals("Minutos"))
            {
                nudHoras.Visible = true;
                nudHoras.Value = 0;
            }
            else
            {
                nudHoras.Visible = false;
                nudHoras.Value = 0;
            }
        }

        private void limpiar()
        {
            this.cmbProcesos.DataSource = null;
            this.txbNombre.Text = this.txbProcedimiento.Text = this.txbCorreo.Text = String.Empty;
            this.cmbPeriodo.SelectedIndex = 0;
            this.dateTimePicker1.Value = this.dateTimePicker2.Value = DateTime.Today;
            this.gboxDatos.Enabled = this.gboxProcesos.Enabled = this.btnEditar.Enabled = this.btnEliminar.Enabled = false;
            mostrarIntervalo();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            opcion = 1;
            limpiar();
            this.gboxDatos.Enabled = true;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            opcion = 2;
            this.gboxDatos.Enabled = true;
            this.gboxProcesos.Enabled = false;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            opcion = 3;
            if (MessageBox.Show("Se borrara toda la información del Proceso, desea continuar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                TareaLN objTareasLN = new TareaLN();
                EN.Tablas.Tareas objEntidad = new EN.Tablas.Tareas();
                objEntidad.pId = Convert.ToInt32(this.cmbProcesos.SelectedValue.ToString());
                int valor = 0;

                valor = objTareasLN.eliminar(objEntidad);

                if (valor.Equals(0))
                {
                    MessageBox.Show("El proceso se ha eliminado con exito!!");
                }
                else
                {
                    MessageBox.Show("Ocurrio un error al eliminar el proceso");
                }
                limpiar();
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            btnProcesos_Clicked = true;
            cargarProcesos();
            this.gboxDatos.Enabled = false;
            this.gboxProcesos.Enabled = true;
            mostrarIntervalo();
        }

        public void Pausa(int segundos)
        {
            long esperaTmp = Environment.TickCount + (segundos * 500);
            while (esperaTmp > Environment.TickCount)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private void Temporizador_Tick(object sender, EventArgs e)
        {
            String RutaEntrada;
            try
            {
                //CARGA TODOS LOS PROCESOS GUARDADOS EN BAN_TAREAS
                this.Temporizador.Enabled = false;
                this.btnStop.Enabled = false;
                DataTable procesos = new TareaLN().consultar();
                Tareas Tarea;
                tiempoServidor = DateTime.Now;

                //OBTIENE LAS RUTAS DE LOS BANCOS
                RutaLN objRutaLN = new RutaLN();
                Ruta objRuta = new Ruta();
                BancoLN objBancoLN = new BancoLN();
                LectorArchivos objLector_ax = new LectorArchivos();

                List<String> listaRutaArchivos_ax = new List<string>();
                List<ArchivoEN> listaRutaArchivosOrdenados = new List<ArchivoEN>();
                List<Banco> lista = new List<Banco>();
                List<Banco> lista_ax = new List<Banco>();

                ArchivoLN archivoLN = new ArchivoLN();
                ArchivoEN archivoEN = new ArchivoEN();

                if (procesos != null)
                {

                    foreach (DataRow fila in procesos.Rows)
                    {
                        Tarea = new Tareas();
                        Tarea.pId = Convertidor.aEntero32(fila["Id"]);
                        Tarea.pNombreTarea = Convertidor.aCadena(fila["NombreTarea"]);
                        Tarea.pPeriodo = Convertidor.aCadena(fila["Periodo"]);
                        Tarea.pTiempoIntervalo = Convertidor.aCadena(fila["TiempoIntervalo"]);
                        Tarea.pInicio = Convertidor.aCadena(fila["Inicio"]);
                        Tarea.pOperacion = Convertidor.aCadena(fila["Proceso"]);
                        Tarea.pCorreoControl = Convertidor.aCadena(fila["CorreoControl"]);

                        Codigo.EnviarCorreo enviarNotifiacion = new Codigo.EnviarCorreo();

                        tiempoProceso = Convert.ToDateTime(Tarea.pInicio);
                        if (tiempoProceso <= tiempoServidor)
                        {
                            #region SAU Pagos cierre mes - orden de pagos por fecha de la bolsa de archivos
                            EN.Tablas.Banco objB = new EN.Tablas.Banco();

                            objB.pActivo = true;
                            objB.pTipoProceso = "POL_";  // Pagos Online

                            lista = objBancoLN.consultar(objB);

                            objB.pTipoProceso = "ABR_"; // Recaudo diario

                            lista_ax = objBancoLN.consultar(objB);

                            lista.AddRange(lista_ax);

                            foreach (Banco bank in lista)
                            {
                                objRuta.pOid = bank.pRutaArchivosEntrada;
                                RutaEntrada = objRutaLN.consultar(objRuta)[0].pRuta;
                                List<String> listaRutaArchivos = objLector_ax.listarDirectorio(RutaEntrada);
                                if (listaRutaArchivos_ax.Find(x => x.Contains(RutaEntrada)) == null)
                                {
                                    listaRutaArchivos_ax.AddRange(listaRutaArchivos);
                                }
                            }
                            listaRutaArchivosOrdenados = objLector_ax.procesarArchivosFecha(listaRutaArchivos_ax);
                            objRuta.pOid = 0;
                            #endregion
                            #region TAREA ASOBANCAIRA SIN USO


                            //**********************************************************
                            //SE REALIZA EL PROCESO DE ASOBANCARIA PARA TODOS LOS BANCOS
                            if (Tarea.pOperacion.Equals("Asobancaria"))
                            {
                                bool procesoConError = false;
                                List<String> RespuestaProceso = new List<string>();
                                this.label10.Text = Tarea.pNombreTarea;
                                Pausa(2);
                                Asobancaria objAso = new Asobancaria();
                                RespuestaProceso = objAso.obtenerBancosAsobancaria(ref procesoConError);

                                TareaLN objTareasLN = new TareaLN();
                                int valor = 0;

                                if (RespuestaProceso.Count > 0)
                                {
                                    foreach (String procesoBanco in RespuestaProceso)
                                    {
                                        this.listBox1.Items.Add(this.label10.Text + " " + procesoBanco + " Hora: " + DateTime.Now.ToString());
                                    }

                                    if (procesoConError == true)
                                    {
                                        // AQUI  Tarea.pTiempoIntervalo = "30";
                                        valor = objTareasLN.actualizarIncioProcesoConError(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar pero con errores!! Hora: " + DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        valor = objTareasLN.actualizarIncioProceso(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar con exito!!! Hora: " + DateTime.Now.ToString());
                                    }
                                }
                                Pausa(6);
                                this.label10.Text = "";

                            }
                            #endregion

                            #region TAREA PAGOSONLINE

                            //************************************************************
                            //SE REALIZA EL PROCESO DE PAGOS ONLINE PARA TODOS LOS BANCOS
                            if (Tarea.pOperacion.Equals("PagosOnline"))
                            {
                                bool procesoConError = false;
                                List<String> RespuestaProceso = new List<string>();
                                this.label10.Text = Tarea.pNombreTarea;
                                Pausa(2);
                                PagosOnline objPagOnline = new PagosOnline();
                                RespuestaProceso = objPagOnline.obtenerBancosPagosOnline(ref procesoConError, listaRutaArchivosOrdenados);

                                #region Limpia bolsa archivos pagos
                                listaRutaArchivosOrdenados.Clear();
                                listaRutaArchivos_ax.Clear();
                                lista.Clear();
                                #endregion

                                TareaLN objTareasLN = new TareaLN();
                                int valor = 0;

                                if (RespuestaProceso.Count > 0)
                                {
                                    foreach (String procesoBanco in RespuestaProceso)
                                    {
                                        this.listBox1.Items.Add(this.label10.Text + " " + procesoBanco + " Hora: " + DateTime.Now.ToString());
                                    }

                                    if (procesoConError == true)
                                    {
                                        // AQUI Tarea.pTiempoIntervalo = "30";
                                        valor = objTareasLN.actualizarIncioProcesoConError(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar pero con errores!! Hora: " + DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        valor = objTareasLN.actualizarIncioProceso(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar con exito!!! Hora: " + DateTime.Now.ToString());
                                    }
                                }
                                Pausa(6);
                                this.label10.Text = "";
                            }
                            #endregion

                            #region TAREA MOVERFTP

                            //************************************************************
                            //SE REALIZA EL PROCESO DE MOVER ARCHIVOS QUE SE ENCUENTRAN EN UNA FTP A CUALQUIER RUTA ESTABLECIDA
                            if (Tarea.pOperacion.Equals("MoverFtp"))
                            {

                                bool procesoConError = false;
                                String RespuestaProceso = String.Empty;
                                this.label10.Text = Tarea.pNombreTarea;
                                Pausa(2);
                                MoverArchivos objmFTP = new MoverArchivos();
                                RespuestaProceso = objmFTP.moverAFtp(ref procesoConError);

                                TareaLN objTareasLN = new TareaLN();
                                int valor = 0;

                                if (procesoConError == true)
                                {
                                    this.listBox1.Items.Add(this.label10.Text + " " + RespuestaProceso + " Hora: " + DateTime.Now.ToString());

                                    // AQUI Tarea.pTiempoIntervalo = "30";
                                    valor = objTareasLN.actualizarIncioProcesoConError(Tarea);

                                    if (valor <= 0)
                                        this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                    else
                                        this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar pero con errores!! Hora: " + DateTime.Now.ToString());
                                }
                                else
                                {
                                    valor = objTareasLN.actualizarIncioProceso(Tarea);
                                    if (valor <= 0)
                                        this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                    else
                                        this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar con exito!!! Hora: " + DateTime.Now.ToString());
                                }

                                Pausa(6);
                                this.label10.Text = "";
                            }
                            #endregion

                            #region TAREA TARJETAS CREDITO

                            //************************************************************
                            //SE REALIZA EL PROCESAMIENTO DE LOS ARCHIVOS TARJETAS DE CREDITO PARA TODOS LOS BANCOS
                            if (Tarea.pOperacion.Equals("TarjetasCredito"))
                            {

                                bool procesoConError = false;
                                List<String> RespuestaProceso = new List<string>();
                                this.label10.Text = Tarea.pNombreTarea;
                                Pausa(2);
                                TarjetaCredito objRecaudo = new TarjetaCredito();
                                RespuestaProceso = objRecaudo.obtenerBancosTarjetasCredito(ref procesoConError);

                                TareaLN objTareasLN = new TareaLN();
                                int valor = 0;

                                if (RespuestaProceso.Count > 0)
                                {
                                    foreach (String procesoBanco in RespuestaProceso)
                                    {
                                        this.listBox1.Items.Add(this.label10.Text + " " + procesoBanco + " Hora: " + DateTime.Now.ToString());
                                    }

                                    if (procesoConError == true)
                                    {
                                        // AQUI Tarea.pTiempoIntervalo = "30";
                                        valor = objTareasLN.actualizarIncioProcesoConError(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar pero con errores!! Hora: " + DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        valor = objTareasLN.actualizarIncioProceso(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar con exito!!! Hora: " + DateTime.Now.ToString());
                                    }
                                }

                                Pausa(6);
                                this.label10.Text = "";
                            }
                            #endregion

                            #region TAREA RECUADO DIARIO

                            //************************************************************
                            //SE REALIZA EL PROCESAMIENTO DE LOS ARCHIVOS DE RECAUDO DIARIO PARA TODOS LOS BANCOS
                            if (Tarea.pOperacion.Equals("RecaudoDiario"))
                            {
                                bool procesoConError = false;
                                List<String> RespuestaProceso = new List<string>();
                                this.label10.Text = Tarea.pNombreTarea;
                                Pausa(2);
                                Recaudo objRecaudo = new Recaudo();
                                RespuestaProceso = objRecaudo.obtenerBancosRecaudoDiario(ref procesoConError, listaRutaArchivosOrdenados);
                                #region Limpia bolsa archivos pagos
                                listaRutaArchivosOrdenados.Clear();
                                listaRutaArchivos_ax.Clear();
                                lista.Clear();
                                #endregion
                                TareaLN objTareasLN = new TareaLN();
                                int valor = 0;

                                if (RespuestaProceso.Count > 0)
                                {
                                    foreach (String procesoBanco in RespuestaProceso)
                                    {
                                        this.listBox1.Items.Add(this.label10.Text + " " + procesoBanco + " Hora: " + DateTime.Now.ToString());
                                    }
                                    if (procesoConError == true)
                                    {
                                        // AQUI  Tarea.pTiempoIntervalo = "30";
                                        valor = objTareasLN.actualizarIncioProcesoConError(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar pero con errores!! Hora: " + DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        valor = objTareasLN.actualizarIncioProceso(Tarea);
                                        if (valor <= 0)
                                            this.listBox1.Items.Add(this.label10.Text + " ¡¡Ocurrio un Error al actualizar la Fecha!! Hora: " + DateTime.Now.ToString());
                                        else
                                            this.listBox1.Items.Add(this.label10.Text + " se termino de ejecutar con exito!!! Hora: " + DateTime.Now.ToString());
                                    }
                                }
                                Pausa(6);
                                this.label10.Text = "";
                            }
                            #endregion

                        }
                    }
                }
                Temporizador.Enabled = true;
                this.btnStop.Enabled = true;
                this.label10.Text = "";

            }
            catch (Exception ex)
            {
                log.insertaLogErroresLN("Procesos Bancarios: " + ex.ToString(), DateTime.Now.ToString());
                //this.listBox1.Items.Add(this.label10.Text + ex.Message + " Hora: " + DateTime.Now.ToString());
                string email = ConfigurationManager.AppSettings["correoSoporte"];
                string email_dev = ConfigurationManager.AppSettings["correoSoporte"];
                EnvioMail("", "Error Gral: Procesos Bancarios", "Error Gral: Procesos: " + ex.ToString() + " Hora: " + DateTime.Now.ToString(), email, email, email_dev);
                // return
                Temporizador.Enabled = true;
                this.btnStop.Enabled = true;
                this.label10.Text = "";
            }
            finally
            {
                Temporizador.Enabled = true;
                this.btnStop.Enabled = true;
                this.label10.Text = "";
            }

        }

        //BOTON PARA INICIAR LAS TAREAS
        private void btnStart_Click(object sender, EventArgs e)
        {
            tempCierra.Stop();
            btnProcesos_Clicked = false;
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            Temporizador.Enabled = true;
            this.tabPage2.Parent = null;
            this.Visible = false;
            ArchivoLN archivoLN = new ArchivoLN();
            ArchivoEN archivoEN = new ArchivoEN();
            archivoEN.Fecha = System.DateTime.Now;
            archivoEN.RutaArchivo = "";
            archivoLN.eliminarArchivoBolsaLN(archivoEN, "T");
        }
        //BOTON PARA PAUSAR LAS TAREAS
        private void btnStop_Click(object sender, EventArgs e)
        {

            if (Temporizador.Enabled == true)
            {
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
                Temporizador.Enabled = false;
                this.tabPage2.Parent = this.tabControl1;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                limpiar();
            }
        }

        private void cmbPeriodo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            mostrarIntervalo();
        }

        private void EnvioMail(string _Adjunto, string _Asunto, string _Mensaje, string _Para, string _Desde, string _Copia)
        {
            DataRow rowC;
            try
            {
                DataSet DsCorreos = new DataSet();
                DsCorreos.Tables.Add("Reportes");
                DsCorreos.Tables["Reportes"].Columns.Add("strTo");
                DsCorreos.Tables["Reportes"].Columns.Add("strCc");
                DsCorreos.Tables["Reportes"].Columns.Add("strCo");
                DsCorreos.Tables["Reportes"].Columns.Add("strSubject");
                DsCorreos.Tables["Reportes"].Columns.Add("strMessaje");
                DsCorreos.Tables["Reportes"].Columns.Add("strPath");

                rowC = DsCorreos.Tables["Reportes"].NewRow();
                rowC["strTo"] = _Para;
                rowC["strCo"] = _Copia;
                rowC["strSubject"] = _Asunto;
                rowC["strMessaje"] = _Mensaje;
                rowC["strPath"] = _Adjunto.Trim();
                DsCorreos.Tables["Reportes"].Rows.Add(rowC);
                EnvioCorreo.EnvioCorreos(DsCorreos, _Desde, false);
            }
            catch (Exception)
            {

            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            tempCierra = new System.Windows.Forms.Timer();
            tempCierra.Tick += new EventHandler(tempEspera_Tick);
            tempCierra.Interval = 1000; // 1 second
            tempCierra.Start();
            lblEspera.Text = contador.ToString();
        }
        private void tempEspera_Tick(object sender, EventArgs e)
        {
            if (btnProcesos_Clicked)
            {
                contador = 12;
            }
            else
            {
                contador--;
            }
            if (contador == 0)
            {
                tempCierra.Stop();
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = true;
                Temporizador.Enabled = true;
                this.tabPage2.Parent = null;
                this.Visible = false;
                ArchivoLN archivoLN = new ArchivoLN();
                ArchivoEN archivoEN = new ArchivoEN();
                archivoEN.Fecha = System.DateTime.Now;
                archivoEN.RutaArchivo = "";
                archivoLN.eliminarArchivoBolsaLN(archivoEN, "T");
            }

            lblEspera.Text = contador.ToString();
        }
    }
}

