using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;

using Pagos.LN;
using Pagos.EN;
using Pagos.LN.Consulta;
using Pagos.EN.Tablas;
using Ionic.Zip;
using System.Globalization;
using System.Data;

using Renci.SshNet;
using SSH;
using System.Data;
using System.Configuration;

using Renci.SshNet; /*PAGOS*/
using SSH;
using System.Collections; /*PAGOS*/

/// <summary>
/// Desarrollado por: Nicolas Larrotta
/// Fecha ultima modificacion: 19/10/2018
/// Observaciones: Se incluye Fiducia 3
///
///  Desarrollado por: Nicolas Larrotta
/// Fecha ultima modificacion: 24/01/2019
/// Observaciones: Se incluye control de pagos para Fiducia
/// Palabra clave : FIDCON
/// </summary>
/// 
/// Desarrollado por: Nicolas Larrotta
/// Fecha ultima modificacion: 04/02/2019
/// Observaciones: Pagos Automaticos
/// Palabra clave : PAGOS
/// </summary>
/// 
/// Desarrollado por: Nicolas Larrotta
/// Fecha ultima modificacion: 04/02/2019
/// Observaciones: Identificación Primeras Inversiones
/// Palabra clave : PAGOPI
/// </summary>
/// 
/// Desarrollado por: Nicolas Larrotta
/// Fecha ultima modificacion: 04/02/2019
/// Observaciones: Pagos Automaticos
/// Palabra clave : /*PAGOS*/
/// </summary>
/// /// Desarrollado por: Duvan Ramírez
/// Fecha ultima modificacion: 19/07/2019
/// Observaciones: Correcion PAGLOS REPETIDOS
/// Palabra clave : /*PAGREP*/
/// </summary>
/// 
/// Desarrollo por Duvan Ramirez
/// Fecha ultima modificacion: 6/09/2019
/// Observaciones: Segunda parte para bancos
/// Palabra clave : /*VALFEC*/
/// </summary> 





namespace WebServiceBancos
{
    public class WsBancos : IWBancos
    {
        #region DECLARACION VARIABLES

        static StreamWriter RegistrosProcesados, ArchivoSico;
        string DigitoVerificacion, FormaPago, FechaRecaudo, calculodigito, Referencia, linea, exporasico, FecParSico;
        string RutaEpicor, RutaProceso, partefija, GuardaInconsistencia, Sucursal, fechaaasico, informacion;
        string NombreBanco, CodBanco, NombreArchivo, FechaSico, Correo, RutaOrigen, RutaDestino, franquicia, tipomovimiento, HoraArchivo, MinutosArchivo;
        Pagos.LN.WcfUtilidades Util = new Pagos.LN.WcfUtilidades();
        String CarpetaBanco = "";
        string fecha = "_" + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString() + "_" +
        DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";
        string encabezado = "";
        String EstadoContrato = "";
        int CantArchivoOrigen = 0;
        int CountSico = 0;
        int contrato;
        int Valor = 0;
        string RutaSico = ConfigurationManager.AppSettings["RutaFTP"].ToString();
        string UsuFTP = ConfigurationManager.AppSettings["UserFTP"].ToString();
        string PassFTP = ConfigurationManager.AppSettings["PassFTP"].ToString();

        string Repositorio = ConfigurationManager.AppSettings["Repositorio"].ToString();


        string NombreComando = ConfigurationManager.AppSettings["comando"].ToString();          /*PAGOS*/
        string NombrePrograma = ConfigurationManager.AppSettings["NombrePrograma"].ToString();  /*PAGOS*/
        string ServidorSico = ConfigurationManager.AppSettings["server"].ToString();            /*PAGOS*/
        string UsuarioSico = ConfigurationManager.AppSettings["user"].ToString();               /*PAGOS*/
        string PasswordSico = ConfigurationManager.AppSettings["password"].ToString();          /*PAGOS*/

        string grupo, numero, nivel;
        bool validacion;
        bool validacionCupo;
        bool ExisteRegstro;

        AfilLN objConsultaAfil = new AfilLN();


        DataTable DtAfilCupo = new DataTable();
        DataTable DtAfilCont = new DataTable();
        DataTable DtContBlanco = new DataTable();
        DataTable DtLupa = new DataTable();
        DataTable DtCupo = new DataTable();
        int ControlaHora = 0;
        int ControlaMinutos = 0;
        string NombreArchivoSico;
        SSHConect Conexion = new SSHConect();  /*PAGOS*/


        /*FIDCON*/
        String ContratoValida = String.Empty;
        String LupaFiducia = String.Empty;
        /*PAGOPI*/
        int npagosguardadosPI = 0;
        #endregion

        /*PAGREP*/
        RecaudoLN objRecaudo = new RecaudoLN();
        bool registroDuplicado = false;
        string pagosConError = "";

        /*Pagos Online Tarjeta Credito*/
        ArrayList registrosPagosTarjeta = new ArrayList();
        static StreamWriter ArchivoVisaMAstercardSICO, ArchivoAmexSICO, ArchivoDinnersSICO;
        String CodBancoVisaMarterCardSICO = "029";
        String CodBancoAmexSICO = "042";
        String CodBancoDinnersSico = "041";
        String CodBancoVisaMarterCardVentas = "043";
        String CodBancoAmexVentas = "045";
        String CodBancoDinnersVentas = "044";
        String CodBancoPSE = "66";
        String NombreArchivoVisaMAstercardSICO = "";
        String NombreArchivoAmexSICO = "";
        String NombreArchivoDinnersSICO = "";
        int CountVisaMarterCard = 0;
        int CountAmex = 0;
        int CountDinners = 0;
        int CountEfectivo = 0;
        int sumaVisaMastercard = 0;
        int sumaDinners = 0;
        int sumaAmex = 0;
        int sumaEfectivo = 0;
        String ultimoCodigoBancoOnline;
        DateTime horaBanco;
        DateTime horaSistema;
        TimeSpan sleep;

        int contPagoRecaudo = 0;

        System.IO.StreamReader sr = null;
        string respu = "";
        /// <summary>
        /// Metodo que consulta la referencia cupo en SICO
        /// </summary>
        public bool ExisteCupo(string cupo)
        {
            DtAfilCupo.Clear();
            if (Convert.ToString(cupo).Length >= 8) //Toma grupos en formacion y definitivos
            {
                if (Convert.ToString(cupo).Length == 9)
                {
                    grupo = cupo.Substring(0, 4);
                    numero = cupo.Substring(4, 3);
                    nivel = cupo.Substring(7, 2);
                }
                else if (Convert.ToString(cupo).Length == 8)
                {
                    grupo = cupo.Substring(0, 3);
                    numero = cupo.Substring(3, 3);
                    nivel = cupo.Substring(6, 2);
                }
                else
                {
                    return false;
                }

                DtAfilCupo = objConsultaAfil.consultarExistenciaCupo(grupo,
                                                         numero,
                                                         nivel);

                if (DtAfilCupo.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Metodo que consulta la referencia contrato en SICO
        /// </summary>
        public bool ExisteContrato(string contrato)
        {
            DtContBlanco.Clear();
            DtAfilCont.Clear();

            if (Convert.ToString(contrato).Length == 6 || Convert.ToString(contrato).Length == 7)
            {
                DtContBlanco = objConsultaAfil.consultarExistenciaContratoBlanco(contrato);  //Busca en CONT

                if (DtContBlanco.Rows.Count > 0)
                {
                    EstadoContrato = "B"; //Estado blanco
                    return true;
                }
                else
                {
                    DtAfilCont = objConsultaAfil.consultarExistenciaContrato(contrato);

                    if (DtAfilCont.Rows.Count > 0)
                    {
                        EstadoContrato = "P"; //Estado procesado
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public DataTable ConsultarLUPA(String Codigo)
        {
            DtLupa.Clear();
            return DtLupa = objConsultaAfil.consultarLugarPago(Codigo);
        }

        public DataTable ConsultarCupo(String contrato)
        {
            DtCupo.Clear();
            return DtCupo = objConsultaAfil.consultarCupo(contrato);
        }

        /// <summary>
        /// Mètodo que procesa el recaudo diario de Pagos
        /// </summary>
        /// <param name="Usuario">Usuario el cual aun no se utiliza, que controlara los permisos para utilizar el metodo</param>
        /// <param name="Password">Password el cual aun no se utiliza, que controlara los permisos para utilizar el metodo</param>
        /// <param name="RutaArchivo">Ruta donde se encuentra el archivo que se procesara</param>
        /// <param name="NombreArchivo">Nombre del archivo que se procesara</param>
        /// <param name="PagosOnline">Indetificador "S" o "N" que identifica si es pago online</param>
        /// <returns></returns>
        public string LecturaPagos(string Usuario, string Password, string RutaArchivo, string NombreArchivo, string PagosOnline)
        {

            try
            {
                System.Threading.Thread.Sleep(1000);

                string hora = DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + ".txt";
                sr = new System.IO.StreamReader(RutaArchivo + NombreArchivo);

                PagosInconsistentesLN objInsertPagosIn = new PagosInconsistentesLN();
                CierreLN ConsultaExis = new CierreLN();
                PagosLN PagoValdLN = new PagosLN();
                ParaLN ObjParaLn = new ParaLN();
                ObjetoTablas ValdObjetos = new ObjetoTablas();
                List<string[,]> ConsultaCodigoBanco = new List<string[,]>();
                DataSet CodigoArchivos = new DataSet();
                DataSet Cupos = new DataSet();

                String mes = DateTime.Now.ToString("MMMM").ToUpper();
                String año = DateTime.Now.Year.ToString();
                String Directorio = String.Empty;
                String Fiducia = String.Empty;

                String[] RangoCont = RutaArchivo.ToString().Split('\\');
                DataTable parametro = new DataTable(); //VALFEC

                Fiducia = "F1";
                Directorio = Repositorio + @"\Fiducia1\Procesos\" + año + "\\" + mes + "\\";

                //Se comentaria porque ya no se manajera la fiducia 3
                //switch (RangoCont[5].ToString())
                //{
                //    case "Fiducia1":
                //        Fiducia = "F1";
                //        Directorio = Repositorio + @"\Fiducia1\Procesos\" + año + "\\" + mes + "\\";
                //        break;
                //    case "Fiducia3":
                //        Fiducia = "F3";
                //        Directorio = Repositorio + @"\Fiducia3\Procesos\" + año + "\\" + mes + "\\";
                //        break;
                //    default:
                //        break;
                //}

                int nregistrosprocesados = 0;
                int nreferenciadesconocida = 0;
                int ncupos = 0;
                int nregistrosexistentes = 0;
                int suma = 0;
                int notascredito = 0;
                int contnoinversion = 0;
                int notasdebito = 0;
                int nreferenciaerrada = 0;
                int npagosguardados = 0;
                int totnotascredito = 0;
                int totnotasdebito = 0;
                int totnreferenciaerrada = 0;
                int contadordelineas = 0;
                int contadorPagoCuotas = 0;

                if (!Directory.Exists(Directorio))
                {
                    System.IO.Directory.CreateDirectory(Directorio);
                }

                //Se comentaria porque ya no se manajera la fiducia 3
                ////Se encarga de validar a que fiducia pertenece
                //ObjetoTablas ObjPara = new ObjetoTablas();
                //ObjPara.pParametro = "Contratos Fiducia 3";
                //ObjPara.pVigente = "S";

                //List<ObjetoTablas> listaParametro = new PagosLN().ConsultaEstadoParametro(ObjPara, "ConsultaParametroEstado");

                //if (listaParametro.Count == 0)
                //    return "OCURRIO UN ERROR EN LA PARAMETRIZACIÓN DE FIDUCIA 3";

                /// <summary>
                /// Trae el lote maximo que existe y lo aumenta en 1
                /// </summary>
                List<string[,]> ListPagos = PagoValdLN.ConsultaLoteMaximo("ConsultaMaxLotePagos");
                string[,] ArregloLote = ListPagos[0];
                int LoteMaximo = Convert.ToInt32(ArregloLote[0, 1].ToString());
                LoteMaximo++;



                //PAGREP
                //buscar en la base de datos antes de empezar a recorrerlo 
                #region LECTURA DE ARCHIVO
                //Lee el archivo mientras no existan lineas

                while (sr.EndOfStream == false)
                {
                    linea = sr.ReadLine();

                    contnoinversion = 0;
                    notascredito = 0;
                    notasdebito = 0;
                    nreferenciaerrada = 0;
                    npagosguardadosPI = 0; /*PAGOPI*/


                    if (Convert.ToInt32(linea.Substring(0, 2)) == 01)// Obtener fecha recaudo SAU
                    {
                        // Fecha de recaudo para insertar en la tbl format date
                        FechaRecaudo = linea.Substring(12, 4) + "/" + linea.Substring(16, 2) + "/" + linea.Substring(18, 2);
                        FechaSico = linea.Substring(14, 6);
                        HoraArchivo = linea.Substring(48, 2);
                        MinutosArchivo = linea.Substring(50, 2);
                        ControlaHora = Convert.ToInt32(HoraArchivo);
                        ControlaMinutos = Convert.ToInt32(MinutosArchivo);

                        /*VALFEC*/
                        /*Aca vamos a validar que la fecha del dia se la fecha del archivo*/
                        parametro = ObjParaLn.consultarParametro();

                        if (parametro.Rows.Count > 0)
                        {

                            DataRow row = parametro.Rows[0];
                            FecParSico = row["ParaAnoPro"].ToString() + "/" + row["ParaMesPro"].ToString().PadLeft(2, '0');

                            if (!FechaRecaudo.Substring(0, 7).Equals(FecParSico))
                            {

                                sr.Close();
                                if (DateTime.Parse(FechaRecaudo.Substring(0, 7)) < DateTime.Parse(FecParSico))
                                {
                                    File.Delete(RutaArchivo + NombreArchivo);
                                    return " ARCHIVO ELIMINADO PORQUE NO ESTA DENTRO DEL PERIODO ACTIVO DE SICO : " + FechaRecaudo.Substring(0, 7) + ", CON LA DEL  PARAMETRO :" + FecParSico;
                                }

                                return "NO COINCIDE LA FECHA : " + FechaRecaudo.Substring(0, 7) + ", CON LA DEL  PARAMETRO :" + FecParSico;
                            }
                        }



                        if (PagosOnline == "S")
                            partefija = NombreArchivo; //Nombre del archivo del banco para pagos PSE
                        else
                            partefija = NombreArchivo.TrimStart('0'); //Nombre del archivo del banco para recaudo

                        #region NOMBRE ARCHIVOS
                        CodigoArchivos = PagoValdLN.ConsultaArchivo(Fiducia, "S");

                        if (CodigoArchivos.Tables["TablaBanPagos"].Rows.Count > 0)
                        {
                            for (int i = 0; i < CodigoArchivos.Tables["TablaBanPagos"].Rows.Count; i++)
                            {
                                if (NombreArchivo.Contains(CodigoArchivos.Tables["TablaBanPagos"].Rows[i]["ParteFija"].ToString()))
                                {
                                    ConsultaCodigoBanco = PagoValdLN.ValidaCodigoBanco(PagosOnline, CodigoArchivos.Tables["TablaBanPagos"].Rows[i]["ParteFija"].ToString(),
                                                                                       Fiducia, "ConsultaLugarPago");
                                    if (ConsultaCodigoBanco.Count > 0)
                                    {
                                        string[,] ArregloCodigo = ConsultaCodigoBanco[0];
                                        CodBanco = ArregloCodigo[0, 1].ToString();
                                        NombreBanco = ArregloCodigo[1, 1].ToString();
                                        RutaEpicor = ArregloCodigo[2, 1].ToString();
                                        RutaProceso = ArregloCodigo[3, 1].ToString();
                                    }
                                    else
                                    {
                                        return "OCURRIO UN ERROR EN LA CONSULTA DEL BANCO"; /*PAGOS*/
                                    }
                                }
                            }
                        }
                        else
                        {
                            return "OCURRIO UN ERROR CON EL NOMBRE DEL ARCHIVO"; /*PAGOS*/
                        }
                        //--------------


                        TiemposLN tln = new TiemposLN();
                        ObjetoTablas objt = new ObjetoTablas();
                        objt.pCodBanco = this.CodBanco;
                        IList<ObjetoTablas> listaHorasBancos = tln.ConsultarHoraAplicacionBancoLN(objt);
                        // Validacion Hora Banco 
                        if (listaHorasBancos.Count > 0)
                        {
                            this.horaSistema = Convert.ToDateTime(DateTime.Now.ToString("HH:mm"));
                            this.horaBanco = Convert.ToDateTime(listaHorasBancos[0].pHoraBancoAplicacion);

                            if (horaSistema < horaBanco)
                            {

                                sr.Close();

                                return " ARCHIVO ELIMINADO PORQUE EL BANCO " + this.CodBanco + " AUN NO ES SU HORA DE APLICACION, SE APLICARA A LAS " + listaHorasBancos[0].pHoraBancoAplicacion;
                            }
                        }
                        //    
                        //------

                        List<string[,]> res = objRecaudo.consultarDisponibilidad(CodBanco, partefija);/*DisponibilidadArchivos*/

                        if (res.Count > 0)
                        {

                            return "NO HAY DISPONIBILIDAD DE EJECUCION PARA ESTE BANCO"; /*DisponibilidadArchivos*/
                        }

                        string resp = objRecaudo.updateDisponibilidad(CodBanco, partefija, "1");

                        if (!resp.Equals("1"))
                        {
                            sr.Close();
                            return "NO SE ACTUALIZO EL CAMPO DE DISPONIBILIDAD";
                        }



                        #endregion

                        if (ConsultaCodigoBanco.Count == 0)
                        {
                            return "OCURRIO UN ERROR CON EL NOMBRE DEL ARCHIVO"; /*PAGOS*/
                        }

                        if (NombreBanco == String.Empty)
                        {
                            return "OCURRIO UN ERROR CON EL NOMBRE DEL ARCHIVO"; /*PAGOS*/
                        }




                        //Valida si ya existe 
                        /*PAGREP*/
                        //Consultamos si ya se encuentra ese pago 
                        ValdObjetos.pFecPago = FechaRecaudo.Trim() + " " + HoraArchivo + ":" + MinutosArchivo;
                        ValdObjetos.pParteFija = partefija;
                        List<string[,]> listRecaudo = objRecaudo.ValidaExisteRecaudo(ValdObjetos, "pa_Ban_Verifica_Recaudos");

                        if (listRecaudo.Count != 0)
                        {
                            insertarRecaudo = false;
                            sr.Close();
                            File.Delete(RutaArchivo + NombreArchivo);

                            objRecaudo.updateDisponibilidad(CodBanco, partefija, "0");// habilita de nuevo la disponibilidad
                            //registroDuplicado = true;
                            //pagosConError += ValdObjetos.pCodBanco + " " + ValdObjetos.pFecPago + " " + ValdObjetos.pContrato + " " + ValdObjetos.pValPago + " . \n ";
                            return "EL ARCHIVO YA SE ENCUENTRA PROCESADO"; /*REGREP*/
                        }
                        else
                        {
                            insertarRecaudo = true;
                        }


                        CarpetaBanco = Directorio + NombreBanco.Replace(" ", "") + "\\";


                        if (!Directory.Exists(CarpetaBanco))
                        {
                            System.IO.Directory.CreateDirectory(CarpetaBanco);
                        }

                        RegistrosProcesados = new StreamWriter(CarpetaBanco + NombreBanco + fecha + ".txt", false, Encoding.GetEncoding(1252));
                        if (!NombreArchivo.Contains("Acreedores"))
                        {
                            ArchivoSico = new StreamWriter(CarpetaBanco + "Pagos" + CodBanco.PadLeft(3, '0') + fecha, false, Encoding.GetEncoding(1252));
                            ArchivoVisaMAstercardSICO = new StreamWriter(CarpetaBanco + "Pagos" + CodBancoVisaMarterCardSICO.PadLeft(3, '0') + fecha, false, Encoding.GetEncoding(1252));
                            ArchivoAmexSICO = new StreamWriter(CarpetaBanco + "Pagos" + CodBancoAmexSICO.PadLeft(3, '0') + fecha, false, Encoding.GetEncoding(1252));
                            ArchivoDinnersSICO = new StreamWriter(CarpetaBanco + "Pagos" + CodBancoDinnersSico.PadLeft(3, '0') + fecha, false, Encoding.GetEncoding(1252));
                        }


                        DataTable DtCodLupa = ConsultarLUPA(CodBanco);
                        if (DtCodLupa.Rows.Count > 0)
                        {
                            DataRow rowlupa = DtCodLupa.Rows[0];
                            LupaFiducia = rowlupa["LupaFlagFiller"].ToString();
                        }
                        else
                        {
                            return "OCURRIO UN ERROR AL CONSULTAR LUPA";
                        }

                    }
                    else
                    {
                        if (Convert.ToInt32(linea.Substring(0, 2)) == 06)
                        {
                            contadordelineas++;


                            Referencia = linea.Substring(2, 47).TrimStart('0').Replace(" ", "").Replace("-", "");

                            DigitoVerificacion = linea.Substring(49, 1);

                            FormaPago = linea.Substring(66, 2) == "02" ? "CHEQUE" : "EFECTIVO";
                            Sucursal = linea.Substring(83, 4);
                            Valor = Convert.ToInt32(linea.Substring(52, 10));
                            tipomovimiento = linea.Substring(97, 2);

                            if (tipomovimiento != "ND")
                            {
                                suma = Valor + suma;
                            }
                            if (Referencia == "")
                                Referencia = "0";

                            //Pago Tarjeta
                            ValdObjetos.pNumAutorizacion = linea.Substring(74, 6);
                            if (ValdObjetos.pNumAutorizacion != "000000" && PagosOnline == "S")
                            {
                                ValdObjetos.pCodBanco = linea.Substring(80, 3);
                                ValdObjetos.pFecPago = FechaRecaudo.Trim() + " " + HoraArchivo + ":" + MinutosArchivo;
                                ValdObjetos.pValPago = linea.Substring(50, 12);
                                ValdObjetos.pForPago = "TARJETA";
                                ValdObjetos.pContrato = Convert.ToString(Referencia);
                                ValdObjetos.pEstado = "V";
                                ValdObjetos.pUsuProceso = "AUTOMATICO";
                                ValdObjetos.pContAnterior = "0";
                                ValdObjetos.pValComision = 0;
                                ValdObjetos.pValRetFuente = 0;
                                ValdObjetos.pValRetIva = 0;
                                ValdObjetos.pValRetIca = 0;
                                ValdObjetos.pNumAutorizacion = linea.Substring(74, 6);
                                ValdObjetos.pHoraPago = "0";
                                ValdObjetos.pNumLote = Convert.ToString(LoteMaximo.ToString());
                                ValdObjetos.pLegalizado = "N";
                                ValdObjetos.pPagOficina = linea.Substring(83, 4);
                                ValdObjetos.pNomArchivo = NombreArchivo;
                                ValdObjetos.pFecPagoBancos = FechaRecaudo.Trim();
                                FormaPago = "TARJETA";
                            }
                            //------
                            else
                            {
                                ValdObjetos.pCodBanco = CodBanco;
                                ValdObjetos.pFecPago = FechaRecaudo.Trim() + " " + HoraArchivo + ":" + MinutosArchivo;
                                ValdObjetos.pValPago = linea.Substring(50, 12);
                                ValdObjetos.pForPago = FormaPago;
                                ValdObjetos.pContrato = Convert.ToString(Referencia);
                                ValdObjetos.pEstado = "V";
                                ValdObjetos.pUsuProceso = "AUTOMATICO";
                                ValdObjetos.pContAnterior = "0";
                                ValdObjetos.pValComision = 0;
                                ValdObjetos.pValRetFuente = 0;
                                ValdObjetos.pValRetIva = 0;
                                ValdObjetos.pValRetIca = 0;
                                ValdObjetos.pNumAutorizacion = "0";
                                ValdObjetos.pHoraPago = "0";
                                ValdObjetos.pNumLote = Convert.ToString(LoteMaximo.ToString());
                                ValdObjetos.pLegalizado = "S";
                                ValdObjetos.pPagOficina = linea.Substring(83, 4);
                                ValdObjetos.pNomArchivo = NombreArchivo;
                                ValdObjetos.pFecPagoBancos = FechaRecaudo.Trim();
                            }

                            if (tipomovimiento != "ND")
                            {
                                if (ValdObjetos.pCodBanco == CodBancoVisaMarterCardVentas)
                                {
                                    sumaVisaMastercard = sumaVisaMastercard + Valor;
                                }
                                else if (ValdObjetos.pCodBanco == CodBancoDinnersVentas)
                                {
                                    sumaDinners = sumaDinners + Valor;
                                }
                                else if (ValdObjetos.pCodBanco == CodBancoAmexVentas)
                                {
                                    sumaAmex = sumaAmex + Valor;
                                }
                                else
                                {
                                    sumaEfectivo = sumaEfectivo + Valor;
                                }
                            }
                            // SAU Monto insertado
                            // Efectivo 66 - VisaMaster 29 - Diners 41 - Amex 42

                            if (insertarRecaudo)
                            {
                                if (Convert.ToString(Referencia).Length == 6 || Convert.ToString(Referencia).Length == 7)
                                {
                                    DataTable DtCupoCon = ConsultarCupo(Referencia);
                                    if (DtCupoCon.Rows.Count > 0)
                                    {
                                        DataRow rowlupa = DtCupoCon.Rows[0];
                                        ValdObjetos.pReferenciaPago = rowlupa["AfilGrupo"].ToString() + rowlupa["AfilNroAf"].ToString().PadLeft(3, '0') + rowlupa["AfilNivAf"].ToString().PadLeft(2, '0');

                                    }
                                    else
                                    {
                                        ValdObjetos.pReferenciaPago = Convert.ToString(Referencia);
                                    }
                                }
                                else
                                {
                                    ValdObjetos.pReferenciaPago = Convert.ToString(Referencia);
                                }

                                // SAU
                                /* Consultar si existe un banco con esa fecha de pago
                                 * si existe, retornar cantidad de pagos que tiene, cant_pagos = query SP
                                Si no existe Insert a tabla cod banco, fecha de pago, fecha proceso por SP(get date)
                                */
                                RptPagosEN pagosEN = new RptPagosEN();


                                RptPagosLN pagosLN = new RptPagosLN();

                                IList<RptPagosEN> arrPagos = null;

                                //PAGO TARJETA
                                if (ValdObjetos.pNumAutorizacion != "0" && PagosOnline == "S")
                                {
                                    ValdObjetos.pCodBanco = linea.Substring(83, 4);
                                    string respu = objRecaudo.insertaRecaudo(ValdObjetos, "pa_Ban_inserta_Recaudo");

                                    pagosEN.codigoBanco = this.CodBanco;
                                    pagosEN.fechaPago = this.FechaRecaudo;

                                    if (ValdObjetos.pCodBanco == CodBancoVisaMarterCardSICO)
                                    {
                                        pagosEN.valorMontoArchivo = sumaVisaMastercard;
                                    }
                                    else if (ValdObjetos.pCodBanco == CodBancoDinnersSico)
                                    {
                                        pagosEN.valorMontoArchivo = sumaDinners;
                                    }
                                    else if (ValdObjetos.pCodBanco == CodBancoAmexSICO)
                                    {
                                        pagosEN.valorMontoArchivo = sumaAmex;
                                    }
                                    else
                                    {
                                        pagosEN.valorMontoArchivo = sumaEfectivo;
                                    }

                                    arrPagos = pagosLN.ConsultarBancoFechaLN(pagosEN);
                                    if (arrPagos.Count > 0) //Si existe
                                    {
                                        try
                                        {
                                            int result = Convert.ToInt32(pagosLN.actualizarBancoCantPagosRecaudoLN(pagosEN));
                                            if (result == 0)
                                            {
                                                return "Error en la actualización";
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            return e.Message.ToString();
                                        }


                                    }
                                    else
                                    {
                                        try
                                        {
                                            int resultado = Convert.ToInt32(pagosLN.insertarBancoFechaLN(pagosEN));
                                            if (resultado == 0)
                                            {
                                                return "Ha ocurrido un error insertando la base de datos";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            return ex.Message.ToString();
                                        }
                                    }

                                    ValdObjetos.pCodBanco = linea.Substring(80, 3);
                                    if (respu.Substring(0, 1) == "0")
                                    {
                                        return "ERROR INSERTANDO RECAUDO " + respu;
                                    }

                                }
                                //-----
                                else
                                {
                                    string respu = objRecaudo.insertaRecaudo(ValdObjetos, "pa_Ban_inserta_Recaudo");

                                    pagosEN.codigoBanco = this.CodBanco;
                                    pagosEN.fechaPago = this.FechaRecaudo;
                                    pagosEN.valorMontoArchivo = sumaEfectivo;

                                    arrPagos = pagosLN.ConsultarBancoFechaLN(pagosEN);
                                    if (arrPagos.Count > 0) //Si existe
                                    {
                                        try
                                        {
                                            int result = Convert.ToInt32(pagosLN.actualizarBancoCantPagosRecaudoLN(pagosEN));
                                            if (result == 0)
                                            {
                                                return "Error en la actualización";
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            return e.Message.ToString();
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int resultado = Convert.ToInt32(pagosLN.insertarBancoFechaLN(pagosEN));
                                            if (resultado == 0)
                                            {
                                                return "Ha ocurrido un error insertando la base de datos";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            return ex.Message.ToString();
                                        }
                                    }

                                    if (respu.Substring(0, 1) == "0")
                                    {
                                        return "ERROR INSERTANDO RECAUDO " + respu;
                                    }
                                }
                            }

                            /// <summary>
                            /// Se encarga de evaluar si es de una fiducia errada y si lo es lo guarda en una tabla de Pagos Errados
                            /// </summary>
                            #region CONTROL FIDUCIAS
                            /*FIDCON*/
                            ExisteRegstro = false;
                            if (Convert.ToString(Referencia).Length >= 8)
                            {
                                validacionCupo = ExisteCupo(Referencia);

                                if (validacionCupo)
                                {
                                    DataRow row = DtAfilCupo.Rows[0];
                                    ContratoValida = row["AfilNroCon"].ToString();
                                    ExisteRegstro = true;
                                }
                            }
                            else
                            {
                                validacion = ExisteContrato(Referencia);

                                if (validacion)
                                {
                                    ContratoValida = Referencia;
                                    ExisteRegstro = true;
                                }
                            }


                            //Se comentarea porque ya no se utilizaa la fiducia 3
                            //if (ExisteRegstro)
                            //{
                            //    ObjetoTablas objParaResul = new ObjetoTablas();
                            //    objParaResul = listaParametro[0];
                            //    String[] RangoContrato = objParaResul.pValor.ToString().Split('-');
                            //    int fiduciaerrada = 0;

                            //    switch (LupaFiducia.Trim())
                            //    {
                            //        case "1":
                            //            if (Convert.ToInt32(ContratoValida) >= Convert.ToInt32(RangoContrato[0].ToString()) && Convert.ToInt32(ContratoValida) <= Convert.ToInt32(RangoContrato[1].ToString()))
                            //            {
                            //                fiduciaerrada++;
                            //            }
                            //            break;
                            //        case "3":
                            //            if (Convert.ToInt32(ContratoValida) < Convert.ToInt32(RangoContrato[0].ToString()) || Convert.ToInt32(ContratoValida) > Convert.ToInt32(RangoContrato[1].ToString()))
                            //            {
                            //                fiduciaerrada++;
                            //            }
                            //            break;
                            //        default:
                            //            break;
                            //    }

                            //    if (fiduciaerrada > 0)
                            //    {

                            //        ObjetoTablas objParaErrados = new ObjetoTablas();
                            //        objParaErrados.pProcesoErr = "PA";
                            //        objParaErrados.pReferenciaErr = ContratoValida;
                            //        objParaErrados.pCodBancoErr = ValdObjetos.pCodBanco;
                            //        objParaErrados.pValorPagoErr = ValdObjetos.pValPago;
                            //        objParaErrados.pFechaErr = Convert.ToDateTime(ValdObjetos.pFecPago).ToString("dd/MM/yyyy");
                            //        objParaErrados.pForPagoErr = ValdObjetos.pForPago;
                            //        objParaErrados.pUsuarioProcesoErr = "AUTOMATICO";


                            //        List<string[,]> ConsultPagosErrados = new PagosErradosLN().ValidaExistePagoErrado(objParaErrados, "ConsultaExistePagoErrado");

                            //        if (ConsultPagosErrados.Count == 0)
                            //        {
                            //            string GuardaPagoErrado = new PagosErradosLN().InsertPagosErrados(objParaErrados, "InsertPagosErrados");
                            //        }

                            //    }
                            //}



                            #endregion


                            /// <summary>
                            /// Si el codigo de verificación calculado es diferente al del plano, lo inserta en la tabla 
                            /// Pagos Inconsistentes y lo guarda en un archivo
                            /// </summary>
                            calculodigito = Util.calculoDigito(Convert.ToString(Referencia));

                            if (NombreBanco.Contains("DAVIVIENDA") && tipomovimiento == "NC")
                            {
                                if (Convert.ToString(Referencia).Length >= 6 || Referencia == "0")
                                {
                                    validacion = ExisteContrato(Referencia);

                                    validacionCupo = ExisteCupo(Referencia);

                                    if (validacion == true || validacionCupo == true)
                                    {
                                        contnoinversion = 0;
                                    }
                                    else
                                    {
                                        if (encabezado == "")
                                        {
                                            RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                            encabezado = "S";
                                        }
                                        RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " NOTA CREDITO");

                                        CrearArchivoSico(NombreArchivo);

                                        nregistrosprocesados++;
                                        notascredito++;
                                        contnoinversion++;
                                    }
                                }
                            }

                            /// <summary>
                            ///  //Valida si es nota debito para indetificarlo como referencia nota debito
                            /// </summary>
                            if (NombreBanco.Contains("DAVIVIENDA") && tipomovimiento == "ND")
                            {
                                if (encabezado == "")
                                {
                                    RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                    encabezado = "S";
                                }
                                RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " NOTA DEBITO");

                                CrearArchivoSico(NombreArchivo);

                                nregistrosprocesados++;
                                notasdebito++;
                                contnoinversion++;
                            }

                            if (NombreBanco.Contains("DAVIVIENDA"))
                            {
                                if (tipomovimiento == "DE")
                                {
                                    if (calculodigito != DigitoVerificacion)
                                    {
                                        List<string[,]> ConsultaPagoIncon = objInsertPagosIn.ValidaExistePagoInconsistente(ValdObjetos, "ConsultaExisPagoIncon");
                                        if (ConsultaPagoIncon.Count == 0)
                                        {
                                            GuardaInconsistencia = objInsertPagosIn.PagosIncon(ValdObjetos, "InsertPagosInconsistentes");
                                        }
                                        if (encabezado == "")
                                        {
                                            RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                            encabezado = "S";
                                        }
                                        RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " REFERENCIA ERRADA");

                                        CrearArchivoSico(NombreArchivo);

                                        nregistrosprocesados++;
                                        nreferenciaerrada++;
                                        contnoinversion++;
                                    }
                                }
                            }

                            if (!NombreBanco.Contains("DAVIVIENDA"))
                            {
                                if (calculodigito != DigitoVerificacion)
                                {
                                    List<string[,]> ConsultaPagoIncon = objInsertPagosIn.ValidaExistePagoInconsistente(ValdObjetos, "ConsultaExisPagoIncon");
                                    if (ConsultaPagoIncon.Count == 0)
                                    {
                                        GuardaInconsistencia = objInsertPagosIn.PagosIncon(ValdObjetos, "InsertPagosInconsistentes");
                                    }
                                    if (encabezado == "")
                                    {
                                        RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                        encabezado = "S";
                                    }
                                    RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " REFERENCIA ERRADA");

                                    CrearArchivoSico(NombreArchivo);

                                    nregistrosprocesados++;
                                    nreferenciaerrada++;
                                    contnoinversion++;
                                }
                            }

                            /// <summary>
                            /// Valida referencias desconocidas
                            /// </summary>
                            /// 
                            if (nreferenciaerrada == 0)
                            {
                                if (Convert.ToString(Referencia).Length >= 8)
                                {
                                    validacionCupo = ExisteCupo(Referencia);

                                    if (validacionCupo == true)
                                    {
                                        contnoinversion = 0;
                                    }
                                    else
                                    {
                                        if (notascredito == 0 && notasdebito == 0)
                                        {
                                            List<string[,]> ConsultaPagoIncon = objInsertPagosIn.ValidaExistePagoInconsistente(ValdObjetos, "ConsultaExisPagoIncon");
                                            if (ConsultaPagoIncon.Count == 0)
                                            {
                                                GuardaInconsistencia = objInsertPagosIn.PagosIncon(ValdObjetos, "InsertPagosInconsistentes");
                                            }

                                            if (encabezado == "")
                                            {
                                                RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                encabezado = "S";
                                            }
                                            RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " REFERENCIA DESCONOCIDA");

                                            CrearArchivoSico(NombreArchivo);

                                            nregistrosprocesados++;
                                            nreferenciadesconocida++;
                                            contnoinversion++;
                                        }
                                    }
                                }
                            }


                            /// <summary>
                            /// Entra a esta rutina si es un contrato y no presenta ninguna novedad
                            /// </summary>
                            /// 

                            if (contnoinversion == 0)
                            {
                                DigitoVerificacion = linea.Substring(49, 1);

                                if (Convert.ToString(Referencia).Length >= 8) //Valida si es un cupo y no un contrato
                                {
                                    if (encabezado == "")
                                    {
                                        RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                        encabezado = "S";
                                    }
                                    RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " PAGO A CUOTA");
                                    //PAGO TARJETA
                                    if (ValdObjetos.pNumAutorizacion != "0" && PagosOnline == "S")
                                    {
                                        string GuardaRegistrosPago = PagoValdLN.IUDPago(ValdObjetos, "InsertPagosCuotas");
                                        CrearArchivoSicoTarjeta(NombreArchivo, ValdObjetos.pCodBanco);
                                        contadorPagoCuotas++;
                                    }
                                    //----------
                                    else
                                    {
                                        CrearArchivoSico(NombreArchivo);
                                    }

                                    nregistrosprocesados++;
                                    ncupos++;
                                }
                                else  //Valida si es un contrato
                                {
                                    validacion = ExisteContrato(Referencia);
                                    if (validacion == true)
                                    {
                                        if (EstadoContrato == "B")
                                        {
                                            List<string[,]> VentaProcesada = ConsultaExis.ValidaExisteCierre(ValdObjetos, "ConsultaVentaProcesada");

                                            if (VentaProcesada.Count > 0)
                                            {
                                                if (encabezado == "")
                                                {
                                                    RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                    encabezado = "S";
                                                }
                                                RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " VENTA PROCESADA");

                                                CrearArchivoSico(NombreArchivo);

                                                nregistrosprocesados++;
                                                nregistrosexistentes++;
                                            }
                                            else
                                            {
                                                /// <summary>
                                                /// Si el estado del contrato es diferente de Vigente, lo guarda en un archivo 
                                                /// y pasa al otro contrato, si no valida en la tabla Pagos
                                                /// </summary>
                                                List<string[,]> ConsultaCierre = ConsultaExis.ValidaExisteCierre(ValdObjetos, "ConsultaExisteCierre");

                                                if (ConsultaCierre.Count > 0)
                                                {
                                                    if (encabezado == "")
                                                    {
                                                        RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                        encabezado = "S";
                                                    }
                                                    RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " VENTA NO VIGENTE");

                                                    CrearArchivoSico(NombreArchivo);

                                                    nregistrosprocesados++;
                                                    nregistrosexistentes++;
                                                }
                                                else
                                                {
                                                    /// <summary>
                                                    /// Valida en la tabla Pagos si el pago existe, si es asi lo guarda en un archivo, 
                                                    /// si no lo inserta en esta misma tabla y lo guarda en un archivo con el resumen de los pagos
                                                    /// </summary>

                                                    List<string[,]> ConsultPagos = PagoValdLN.ValidaExistePago(ValdObjetos, "ConsultaExistePago");

                                                    if (ConsultPagos.Count > 0)
                                                    {
                                                        if (encabezado == "")
                                                        {
                                                            RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                            encabezado = "S";
                                                        }
                                                        RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " PAGO YA EXISTE");

                                                        CrearArchivoSico(NombreArchivo);

                                                        nregistrosprocesados++;
                                                        nregistrosexistentes++;
                                                    }
                                                    else
                                                    {

                                                        string GuardaRegistrosPago = PagoValdLN.IUDPago(ValdObjetos, "InsertPagos");
                                                        if (GuardaRegistrosPago.Substring(0, 1) != "0")
                                                        {
                                                            if (encabezado == "")
                                                            {
                                                                RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                                encabezado = "S";
                                                            }
                                                            RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " PAGO A PRIMERA INVERSIÓN");

                                                            nregistrosprocesados++;
                                                            npagosguardados++;
                                                            /*PAGOPI*/
                                                            npagosguardadosPI++;

                                                            CrearArchivoSico(NombreArchivo);
                                                        }
                                                        else
                                                        {
                                                            if (encabezado == "")
                                                            {
                                                                RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                                encabezado = "S";
                                                            }
                                                            RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " OCURRIO UN ERROR AL GUARDAR");
                                                            nregistrosprocesados++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (encabezado == "")
                                            {
                                                RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                encabezado = "S";
                                            }
                                            RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " PAGO A CUOTA");

                                            CrearArchivoSico(NombreArchivo);

                                            nregistrosprocesados++;
                                            ncupos++;
                                        }
                                    }
                                    else
                                    {
                                        if (notascredito == 0)
                                        {
                                            List<string[,]> ConsultaPagoIncon = objInsertPagosIn.ValidaExistePagoInconsistente(ValdObjetos, "ConsultaExisPagoIncon");
                                            if (ConsultaPagoIncon.Count == 0)
                                            {
                                                GuardaInconsistencia = objInsertPagosIn.PagosIncon(ValdObjetos, "InsertPagosInconsistentes");
                                            }

                                            if (encabezado == "")
                                            {
                                                RegistrosProcesados.WriteLine("REFERENCIA".PadRight(13, ' ') + "VALOR".PadRight(10, ' ') + "FECHA PAGO".PadRight(11, ' ') + " OBSERVACION");
                                                encabezado = "S";
                                            }
                                            RegistrosProcesados.WriteLine(Convert.ToString(Referencia).PadLeft(10, '0') + "-" + DigitoVerificacion + " $" + Convert.ToString(Valor).PadRight(9, ' ') + FechaRecaudo.Replace("/", "").PadRight(11, ' ') + " REFERENCIA DESCONOCIDA");

                                            CrearArchivoSico(NombreArchivo);

                                            nregistrosprocesados++;
                                            nreferenciadesconocida++;
                                            contnoinversion++;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    /*
                     *   String CodBancoVisaMarterCardSICO = "029";
        String CodBancoAmexSICO = "042";
        String CodBancoDinnersSico = "041";
        String CodBancoVisaMarterCardVentas = "043";
        String CodBancoAmexVentas = "045";
        String CodBancoDinnersVentas = "044";
                     * */
                    if (Convert.ToInt32(linea.Substring(0, 2)) == 09)
                    {
                        RptPagosEN pagosEN = new RptPagosEN();

                        RptPagosLN pagosLN = new RptPagosLN();

                        IList<RptPagosEN> arrPagos = null;

                        CantArchivoOrigen = 0;
                        CantArchivoOrigen = Convert.ToInt32(linea.Substring(3, 8));


                        if (ValdObjetos.pCodBanco == CodBancoVisaMarterCardVentas || ValdObjetos.pCodBanco == CodBancoDinnersVentas
                            || ValdObjetos.pCodBanco == CodBancoAmexVentas)
                        {
                            ValdObjetos.pCodBanco = CodBancoPSE;
                        }

                        if (ValdObjetos.pCodBanco == CodBancoPSE)
                        {
                            // Monto archivo, SP sin parámetros UPDATE
                            pagosEN.fechaPago = this.FechaRecaudo;
                            pagosEN.codigoBanco = ValdObjetos.pCodBanco;
                            try
                            {
                                int result = Convert.ToInt32(pagosLN.actualizarCantPagosArchPSELN(pagosEN));
                                if (result == 0)
                                {
                                    return "Error en la actualización";
                                }
                            }
                            catch (Exception e)
                            {
                                return e.Message.ToString();
                            }

                        }
                        else
                        {
                            pagosEN.codigoBanco = this.CodBanco;
                            pagosEN.fechaPago = this.FechaRecaudo;
                            pagosEN.cantPagosArchivo = CantArchivoOrigen;
                            arrPagos = pagosLN.ConsultarBancoFechaLN(pagosEN);

                            if (arrPagos.Count > 0) //Si existe
                            {
                                try
                                {
                                    int result = Convert.ToInt32(pagosLN.actualizarBancoCantPagosRecaudoLN(pagosEN));
                                    if (result == 0)
                                    {
                                        return "Error en la actualización";
                                    }
                                }
                                catch (Exception e)
                                {
                                    return e.Message.ToString();
                                }


                            }
                            else
                            {
                                try
                                {
                                    int resultado = Convert.ToInt32(pagosLN.insertarBancoFechaLN(pagosEN));
                                    if (resultado == 0)
                                    {
                                        return "Ha ocurrido un error insertando la base de datos";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return ex.Message.ToString();
                                }
                            }
                            try
                            {   
                                int result = Convert.ToInt32(pagosLN.actualizarCantPagosArchPSELN(pagosEN));
                                if (result == 0)
                                {
                                    return "Error en la actualización";
                                }
                            }
                            catch (Exception e)
                            {
                                return e.Message.ToString();
                            }

                        }

                        break;
                    }

                    totnotascredito = notascredito + totnotascredito;
                    totnotasdebito = notasdebito + totnotasdebito;
                    totnreferenciaerrada = nreferenciaerrada + totnreferenciaerrada;
                }
                //CIERRE DEL WHILE
                //CAMBIAR CONTADOR DE RECUADOS AHORA CONSULTAMOS  
                List<string[,]> listRecaudo2 = objRecaudo.ConsultarRegistrosIngresados(ValdObjetos, "pa_Ban_Cuenta_Recaudos");
                if (CantArchivoOrigen != Convert.ToInt16(listRecaudo2[0][0, 1]))
                {

                    sr.Close();
                    Correo = Util.EnvioMail(" ", "Proceso Fallido" + NombreBanco, "Buen día, \n\n" +
                              "La cantidad de registros procesados no coincide con la cantidad que dice el archivo plano.",
                             ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                             ConfigurationManager.AppSettings["CorreoCC"].ToString());

                    return "PROCESO FINALIZADO INCONSISTENCIA DE DATOS";
                }

                /// <summary>
                /// Convierte en formato numero el valor total de los registros procesados
                /// </summary>
                double valor = Convert.ToDouble(suma);
                string resultsuma = Convert.ToString(suma);
                resultsuma = valor.ToString("0,0", CultureInfo.InvariantCulture);
                resultsuma = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", valor);

                RegistrosProcesados.WriteLine("TOTAL VALOR: $" + resultsuma);
                RegistrosProcesados.WriteLine("TOTAL REGISTROS: " + nregistrosprocesados);

                RegistrosProcesados.Close();
                //Cerrar la escritura de los archivos
                if (!NombreArchivo.Contains("Acreedores"))
                {
                    ArchivoSico.Close();
                    ArchivoVisaMAstercardSICO.Close();
                    ArchivoAmexSICO.Close();
                    ArchivoDinnersSICO.Close();

                }

                sr.Close();

                #endregion

                NombreArchivoSico = CodBanco.PadLeft(3, '0') + FechaRecaudo.Replace("/", "").Substring(2, 6) + hora;
                NombreArchivoVisaMAstercardSICO = CodBancoVisaMarterCardSICO.PadLeft(3, '0') + FechaRecaudo.Replace("/", "").Substring(2, 6) + hora;
                NombreArchivoAmexSICO = CodBancoAmexSICO.PadLeft(3, '0') + FechaRecaudo.Replace("/", "").Substring(2, 6) + hora;
                NombreArchivoDinnersSICO = CodBancoDinnersSico.PadLeft(3, '0') + FechaRecaudo.Replace("/", "").Substring(2, 6) + hora;

                if (!NombreArchivo.Contains("Acreedores"))
                {
                    if (tipomovimiento.Contains("DE") || tipomovimiento.Contains("ND") || tipomovimiento.Contains("NC"))
                    {
                        File.Delete(CarpetaBanco + "Pagos" + CodBanco.PadLeft(3, '0') + fecha);
                        File.Delete(CarpetaBanco + "Pagos" + CodBancoVisaMarterCardSICO.PadLeft(3, '0') + fecha);
                        File.Delete(CarpetaBanco + "Pagos" + CodBancoAmexSICO.PadLeft(3, '0') + fecha);
                        File.Delete(CarpetaBanco + "Pagos" + CodBancoDinnersSico.PadLeft(3, '0') + fecha);

                        /// <summary>
                        ///Si el proceso continuo correctamente,  mueve  el archivo de recibidos de la ruta origen a la ruta de procesados
                        /// </summary>
                        RutaOrigen = System.IO.Path.Combine(RutaArchivo + NombreArchivo);
                        RutaDestino = System.IO.Path.Combine(RutaProceso + NombreArchivo + fecha);
                        System.IO.File.Move(RutaOrigen, RutaDestino);

                    }
                    else
                    {
                        /// <summary>
                        /// Copiar a la ruta de epicor
                        /// </summary>
                        //Pagos Efectivo Contadores
                        RutaOrigen = System.IO.Path.Combine(CarpetaBanco, "Pagos" + CodBanco.PadLeft(3, '0') + fecha);
                        RutaDestino = System.IO.Path.Combine(RutaEpicor, NombreArchivoSico);
                        System.IO.File.Copy(RutaOrigen, RutaDestino, true);

                        string[] lines = File.ReadAllLines(RutaDestino);

                        CountEfectivo = 0;
                        foreach (string line in lines)
                        {
                            CountEfectivo++;
                        }

                        if (CountEfectivo == 0)
                        {
                            File.Delete(RutaDestino);

                        }

                        //PAgos Tarjeta Contadores
                        RutaOrigen = System.IO.Path.Combine(CarpetaBanco, "Pagos" + CodBancoVisaMarterCardSICO.PadLeft(3, '0') + fecha);
                        RutaDestino = System.IO.Path.Combine(RutaEpicor, NombreArchivoVisaMAstercardSICO);
                        System.IO.File.Copy(RutaOrigen, RutaDestino, true);
                        //Visa y MasterCards
                        string[] linesVisaMaster = File.ReadAllLines(RutaDestino);

                        CountVisaMarterCard = 0;
                        foreach (string line in linesVisaMaster)
                        {
                            CountVisaMarterCard++;
                        }

                        if (CountVisaMarterCard == 0)
                        {
                            File.Delete(RutaDestino);
                        }

                        //Amex
                        RutaOrigen = System.IO.Path.Combine(CarpetaBanco, "Pagos" + CodBancoAmexSICO.PadLeft(3, '0') + fecha);
                        RutaDestino = System.IO.Path.Combine(RutaEpicor, NombreArchivoAmexSICO);
                        System.IO.File.Copy(RutaOrigen, RutaDestino, true);

                        string[] linesAmex = File.ReadAllLines(RutaDestino);

                        CountAmex = 0;
                        foreach (string line in linesAmex)
                        {
                            CountAmex++;
                        }

                        if (CountAmex == 0)
                        {
                            File.Delete(RutaDestino);
                        }
                        //Dinners
                        RutaOrigen = System.IO.Path.Combine(CarpetaBanco, "Pagos" + CodBancoDinnersSico.PadLeft(3, '0') + fecha);
                        RutaDestino = System.IO.Path.Combine(RutaEpicor, NombreArchivoDinnersSICO);
                        System.IO.File.Copy(RutaOrigen, RutaDestino, true);

                        string[] linesDinners = File.ReadAllLines(RutaDestino);

                        CountDinners = 0;
                        foreach (string line in linesDinners)
                        {
                            CountDinners++;
                        }

                        if (CountDinners == 0)
                        {
                            File.Delete(RutaDestino);
                        }
                        //Elimina los archivos de Cache
                        File.Delete(CarpetaBanco + "Pagos" + CodBanco.PadLeft(3, '0') + fecha);
                        File.Delete(CarpetaBanco + "Pagos" + CodBancoVisaMarterCardSICO.PadLeft(3, '0') + fecha);
                        File.Delete(CarpetaBanco + "Pagos" + CodBancoAmexSICO.PadLeft(3, '0') + fecha);
                        File.Delete(CarpetaBanco + "Pagos" + CodBancoDinnersSico.PadLeft(3, '0') + fecha);

                        CountSico = 0;
                        CountSico = CountEfectivo + CountAmex + CountDinners + CountVisaMarterCard;
                        /// <summary>
                        ///Si el proceso continuo correctamente, lo mueve de la ruta origen a la ruta de procesados
                        /// </summary>
                        RutaOrigen = System.IO.Path.Combine(RutaArchivo + NombreArchivo);
                        RutaDestino = System.IO.Path.Combine(RutaProceso + NombreArchivo + fecha);
                        System.IO.File.Move(RutaOrigen, RutaDestino);
                    }
                }
                else
                {
                    // <summary>
                    ///Si el proceso continuo correctamente, lo mueve de la ruta origen a la ruta de procesados para Acreedores
                    /// </summary>
                    RutaOrigen = System.IO.Path.Combine(RutaArchivo + NombreArchivo);
                    RutaDestino = System.IO.Path.Combine(RutaEpicor + NombreArchivo + fecha);
                    System.IO.File.Move(RutaOrigen, RutaDestino);
                }

                if (!NombreArchivo.Contains("Acreedores"))
                {
                    if (tipomovimiento.Contains("DE") || tipomovimiento.Contains("ND") || tipomovimiento.Contains("NC"))
                    {
                    }
                    else
                    {
                        ///Valdia si el archivo del banco tiene la mismca cantidad que el archivo creado para SICO
                        if (CantArchivoOrigen != CountSico)
                        {
                            Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL CREAR EL ARCHIVO DE SICO DEL BANCO" + NombreBanco, "Buen día, \n\n" +
                              "Se presento un error al crear el archivo a SICO. Por favor validar.",
                             ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                             ConfigurationManager.AppSettings["CorreoCC"].ToString());
                            return "OCURRIO UN ERROR AL CREAR EL ARCHIVO DE SICO DEL BANCO";
                        }
                    }
                }

                if (tipomovimiento.Contains("DE") || tipomovimiento.Contains("ND") || tipomovimiento.Contains("NC"))
                {
                    String archi = "";
                    System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(RutaArchivo);
                    System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");

                    foreach (System.IO.FileInfo archivos in fileNames)
                    {
                        archi = archivos.Name;
                        if (archi.Contains(NombreArchivo))
                        {

                            RutaOrigen = System.IO.Path.Combine(RutaArchivo, archi);
                            RutaDestino = System.IO.Path.Combine(RutaEpicor, NombreArchivoSico);
                            System.IO.File.Copy(RutaOrigen, RutaDestino, true);
                            File.Delete(RutaArchivo + archi);
                            exporasico = Util.UploadFTP(RutaEpicor + NombreArchivoSico, RutaSico, UsuFTP, PassFTP);
                            if (exporasico == "OK")
                            {

                                informacion = "Nombre archivo para SICO: " + NombreArchivoSico + " \n";
                                /*PAGOS*/
                                //Se encarga de aplicar directamente en SICO
                                string comando = NombreComando + NombrePrograma + " " + NombreArchivoSico;
                                Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                            }
                            else
                            {
                                Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO AL FTP DE SICO DEL BANCO" + NombreBanco, "Buen día, \n\n" +
                                  "Se presento un error al crear el archivo a SICO. Por favor validar.",
                                 ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                 ConfigurationManager.AppSettings["CorreoCC"].ToString());
                            }
                        }
                    }
                }

                if (!NombreArchivo.Contains("Acreedores"))
                {
                    /// <summary>
                    /// Exportar archivo al ftp de SICO
                    /// </summary>
                    if (tipomovimiento.Contains("DE") || tipomovimiento.Contains("ND") || tipomovimiento.Contains("NC")) { }
                    else
                    {

                        ///



                        ////


                        TiemposLN tln = new TiemposLN();
                        ObjetoTablas objt = new ObjetoTablas();
                        objt.pCodBanco = this.CodBanco;
                        IList<ObjetoTablas> listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                        if (listaSleepBancos.Count > 0)
                        {
                            this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosAntes));
                            System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                        }

                        string comando;
                        if (CountEfectivo > 0)
                        {
                            // Pagos Efectivo
                            exporasico = Util.UploadFTP(RutaEpicor + NombreArchivoSico, RutaSico, UsuFTP, PassFTP);
                            if (exporasico == "OK")
                            {
                                informacion = "Nombre archivo Efectivo + Cheques para SICO: " + NombreArchivoSico + ". \n";
                                /*PAGOS*/
                                //Se encarga de aplicar directamente en SICO
                                comando = NombreComando + NombrePrograma + " " + NombreArchivoSico;
                                try
                                {
                                    Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                                }
                                catch (Exception ex)
                                {
                                    Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL Aplicar Los Pagos en Sico en la Libreria SSHConect " + CodBanco + "\n\n",
                                          "Se presento un error al aplicar los pagos en la libreria de Sico. Por favor validar." + ex.ToString(),
                                         ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                         ConfigurationManager.AppSettings["CorreoCC"].ToString());
                                }
                                ultimoCodigoBancoOnline = CodBanco;
                                objt.pCodBanco = ultimoCodigoBancoOnline;
                                listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                                if (listaSleepBancos.Count > 0)
                                {
                                    this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosDespues));
                                    System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                                }
                            }
                            else
                            {
                                Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO " + NombreArchivoSico + " AL FTP DE SICO DEL BANCO " + NombreBanco, "Buen día, \n\n" +
                                  "Se presento un error al crear el archivo a SICO. Por favor validar. " + exporasico,
                                 ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                 ConfigurationManager.AppSettings["CorreoCC"].ToString());
                            }
                        }

                        if (CountVisaMarterCard > 0)
                        {

                            objt.pCodBanco = this.CodBancoVisaMarterCardSICO; ;
                            listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                            if (listaSleepBancos.Count > 0)
                            {
                                this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosAntes));
                                System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                            }

                            //Pagos Tarjeta
                            //Visa y MAster Card
                            exporasico = Util.UploadFTP(RutaEpicor + NombreArchivoVisaMAstercardSICO, RutaSico, UsuFTP, PassFTP);
                            if (exporasico == "OK")
                            {
                                informacion = informacion + "Nombre archivo Tarjeta Visa + MAstercard para SICO : " + NombreArchivoVisaMAstercardSICO + ". \n";
                                /*PAGOS*/
                                //Se encarga de aplicar directamente en SICO
                                comando = NombreComando + NombrePrograma + " " + NombreArchivoVisaMAstercardSICO;
                                try
                                {
                                    Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                                }
                                catch (Exception ex)
                                {
                                    Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL Aplicar Los Pagos en Sico en la Libreria SSHConect " + CodBancoVisaMarterCardSICO + "\n\n",
                                          "Se presento un error al aplicar los pagos en la libreria de Sico. Por favor validar." + ex.ToString(),
                                         ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                         ConfigurationManager.AppSettings["CorreoCC"].ToString());
                                }
                                ultimoCodigoBancoOnline = CodBancoVisaMarterCardSICO;
                                objt.pCodBanco = ultimoCodigoBancoOnline;
                                listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                                if (listaSleepBancos.Count > 0)
                                {
                                    this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosDespues));
                                    System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                                }
                            }
                            else
                            {
                                Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO " + NombreArchivoVisaMAstercardSICO + " AL FTP DE SICO DEL BANCO " + NombreBanco, "Buen día, \n\n" +
                                  "Se presento un error al crear el archivo a SICO. Por favor validar." + exporasico,
                                 ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                 ConfigurationManager.AppSettings["CorreoCC"].ToString());
                            }
                        }
                        if (CountDinners > 0)
                        {
                            objt.pCodBanco = this.CodBancoDinnersSico;
                            listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                            if (listaSleepBancos.Count > 0)
                            {
                                this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosAntes));
                                System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                            }
                            //Dinners
                            exporasico = Util.UploadFTP(RutaEpicor + NombreArchivoDinnersSICO, RutaSico, UsuFTP, PassFTP);
                            if (exporasico == "OK")
                            {
                                informacion = informacion + "Nombre archivo Tarjeta Dinners para SICO: " + NombreArchivoDinnersSICO + ". \n";
                                /*PAGOS*/
                                //Se encarga de aplicar directamente en SICO
                                comando = NombreComando + NombrePrograma + " " + NombreArchivoDinnersSICO;
                                try
                                {
                                    Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                                }
                                catch (Exception ex)
                                {
                                    Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL Aplicar Los Pagos en Sico en la Libreria SSHConect " + CodBancoDinnersSico + "\n\n",
                                          "Se presento un error al aplicar los pagos en la libreria de Sico. Por favor validar." + ex.ToString(),
                                         ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                         ConfigurationManager.AppSettings["CorreoCC"].ToString());
                                }
                                ultimoCodigoBancoOnline = CodBancoDinnersSico;
                                objt.pCodBanco = ultimoCodigoBancoOnline;
                                listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                                if (listaSleepBancos.Count > 0)
                                {
                                    this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosDespues));
                                    System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                                }
                            }
                            else
                            {
                                Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO " + NombreArchivoDinnersSICO + " AL FTP DE SICO DEL BANCO " + NombreBanco, "Buen día, \n\n" +
                                  "Se presento un error al crear el archivo a SICO. Por favor validar." + exporasico,
                                 ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                 ConfigurationManager.AppSettings["CorreoCC"].ToString());
                            }
                        }
                        if (CountAmex > 0)
                        {
                            objt.pCodBanco = this.CodBancoAmexSICO;
                            listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                            if (listaSleepBancos.Count > 0)
                            {
                                this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosAntes));
                                System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                            }
                            //Amex
                            exporasico = Util.UploadFTP(RutaEpicor + NombreArchivoAmexSICO, RutaSico, UsuFTP, PassFTP);
                            if (exporasico == "OK")
                            {
                                informacion = informacion + "Nombre archivo Tarjeta Amex para SICO: " + NombreArchivoAmexSICO + ". \n";
                                /*PAGOS*/
                                //Se encarga de aplicar directamente en SICO
                                comando = NombreComando + NombrePrograma + " " + NombreArchivoAmexSICO;
                                try
                                {
                                    Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
                                }
                                catch (Exception ex)
                                {
                                    Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL Aplicar Los Pagos en Sico en la Libreria SSHConect " + CodBancoAmexSICO + "\n\n",
                                          "Se presento un error al aplicar los pagos en la libreria de Sico. Por favor validar." + ex.ToString(),
                                         ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                         ConfigurationManager.AppSettings["CorreoCC"].ToString());
                                }
                                ultimoCodigoBancoOnline = CodBancoAmexSICO;
                                objt.pCodBanco = ultimoCodigoBancoOnline;
                                listaSleepBancos = tln.ConsultarSleepAplicacionBancoLN(objt);
                                if (listaSleepBancos.Count > 0)
                                {
                                    this.sleep = TimeSpan.FromMinutes(Convert.ToDouble(listaSleepBancos[0].pSleepMinutosDespues));
                                    System.Threading.Thread.Sleep(sleep);//n minutos para que no se aplique a las 00:00
                                }
                            }
                            else
                            {
                                Correo = Util.EnvioMail(" ", "OCURRIO UN ERROR AL ENVIAR EL ARCHIVO " + NombreArchivoAmexSICO + " AL FTP DE SICO DEL BANCO" + NombreBanco, "Buen día, \n\n" +
                                  "Se presento un error al crear el archivo a SICO. Por favor validar." + exporasico,
                                 ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(),
                                 ConfigurationManager.AppSettings["CorreoCC"].ToString());
                            }
                        }
                    }
                }

                if (CountAmex > 0 || CountDinners > 0 || CountVisaMarterCard > 0)
                {
                    Correo = Util.EnvioMail(" ", "PAGOS MASIVOS DEL BANCO " + NombreBanco, "Buen día, \n\n" +
                    "A continuación se envia un detallado de los pagos procesados a traves del banco " + NombreBanco + " generado el "
                    + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString() + " a las "
                    + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + " \n\n" +
                    "1. Pagos aplicados a primera inversión: " + npagosguardados + " registros. \n" +
                    "2. Cupos para pagos a cuotas: " + ncupos + " registros. \n" +
                    "3. Referencias ya existentes en ventas: " + nregistrosexistentes + " registros. \n" +
                    "4. Referencias erradas: " + totnreferenciaerrada + " registros. \n" +
                    "5. Referencias notas debito: " + totnotasdebito + " registros. \n" +
                    "6. Referencias notas credito: " + totnotascredito + " registros. \n" +
                    "7. Referencias desconocidas: " + nreferenciadesconocida + " registros. \n" +
                    "8. Pagos en Efectivo: " + CountEfectivo + " registros. \n" +
                    "9. Pagos en Visa + Mastercard: " + CountVisaMarterCard + " registros. \n" +
                    "10. Pagos en Dinners: " + CountDinners + " registros. \n" +
                    "11. Pagos en Amex: " + CountAmex + " registros. \n" +
                    "12. Total Pagos: " + CountSico + " registros. \n" +
                    "13. Total Valor Efectivo y Chekes PSE $ " + sumaEfectivo + ". \n" +
                    "14. Total Valor Tarjetas Visa y MasterCard $ " + sumaVisaMastercard + ". \n" +
                    "15. Total Valor Tarjeta Dinners $ " + sumaDinners + ". \n" +
                    "16. Total Valor Tarjeta Amex $ " + sumaAmex + ". \n" +
                    "Total Valor $ " + resultsuma + ". \n" +
                    "Total registros procesados: " + nregistrosprocesados + " registros. \n" +
                    informacion +
                    "Número de lote: " + LoteMaximo + ".\n\n" +
                    "El archivo de resultados lo encontrara en la ruta: " + CarpetaBanco + ". \n\n" +
                    "La información contenida en este E-mail es confidencial y solo puede ser utilizada por el individuo o la compania " +
                    "a la cual esta dirigido. Si no es el receptor autorizado, cualquier retención, difusión, distribución o copia" +
                    "de este mensaje es prohibida y será sancionada por la ley. Si por error recibe este mensaje, " +
                    "favor reenviarlo al remitente y borrar el mensaje recibido inmediatamente.", ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(), ConfigurationManager.AppSettings["CorreoCC"].ToString());
                }

                else
                {
                    Correo = Util.EnvioMail(" ", "PAGOS MASIVOS DEL BANCO " + NombreBanco, "Buen día, \n\n" +
               "A continuación se envia un detallado de los pagos procesados a traves del banco " + NombreBanco + " generado el "
               + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString() + " a las "
               + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + " \n\n" +
               "1. Pagos aplicados a primera inversión: " + npagosguardados + " registros. \n" +
               "2. Cupos para pagos a cuotas: " + ncupos + " registros. \n" +
               "3. Referencias ya existentes en ventas: " + nregistrosexistentes + " registros. \n" +
               "4. Referencias erradas: " + totnreferenciaerrada + " registros. \n" +
               "5. Referencias notas debito: " + totnotasdebito + " registros. \n" +
               "6. Referencias notas credito: " + totnotascredito + " registros. \n" +
               "7. Referencias desconocidas: " + nreferenciadesconocida + " registros. \n" +
               "8. Total Pagos: " + CountSico + " registros. \n" +
               "Total Valor $ " + resultsuma + ". \n" +
               "Total registros procesados: " + nregistrosprocesados + " registros. \n" +
               informacion +
               "Número de lote: " + LoteMaximo + ".\n\n" +
               "El archivo de resultados lo encontrara en la ruta: " + CarpetaBanco + ". \n\n" +
               "La información contenida en este E-mail es confidencial y solo puede ser utilizada por el individuo o la compania " +
               "a la cual esta dirigido. Si no es el receptor autorizado, cualquier retención, difusión, distribución o copia" +
               "de este mensaje es prohibida y será sancionada por la ley. Si por error recibe este mensaje, " +
               "favor reenviarlo al remitente y borrar el mensaje recibido inmediatamente.", ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(), ConfigurationManager.AppSettings["CorreoCC"].ToString());


                }
                string respue = objRecaudo.updateDisponibilidad(CodBanco, partefija, "0");
                if (!respue.Equals("1"))
                {
                    sr.Close();
                    return "NO SE ACTUALIZO EL CAMPO DE DISPONIBILIDAD AL FINALZAR EL PROCESO";
                }

                return "PROCESO REALIZADO CON EXITO";

            }
            catch (Exception ex)
            {
                sr.Close();
                return ex.Message;
            }
        }


        /// <summary>
        /// Metodo que procesa los pagos aplicados por tarjeta de credito
        /// </summary>
        /// <param name="Usuario">Usuario el cual aun no se utiliza, que controlara los permisos para utilizar el metodo</param>
        /// <param name="Password">Password el cual aun no se utiliza, que controlara los permisos para utilizar el metodo</param>
        /// <param name="RutaArchivo">Ruta donde se encuentra el archivo que se procesara</param>
        /// <param name="NombreArchivo">Nombre del archivo que se procesara</param>
        /// <returns></returns>
        public string PagosTarjeta(string usuario, string password, string RutaArchivo, string NombreArchivo)
        {
            try
            {
                int TipoOperacion;
                System.IO.StreamReader sr = new System.IO.StreamReader(RutaArchivo + NombreArchivo);
                PagosTarjetaLN ConsultaPagosTarjeta = new PagosTarjetaLN();
                PagosLN PagoValdLN = new PagosLN();
                ObjetoTablas ValdObjetos = new ObjetoTablas();
                List<string[,]> ConsultaCodigoBanco = new List<string[,]>();

                String mes = DateTime.Now.ToString("MMMM").ToUpper();
                String año = DateTime.Now.Year.ToString();
                String Directorio = String.Empty;
                String Fiducia = String.Empty;

                String[] RangoCont = RutaArchivo.ToString().Split('\\');

                switch (RangoCont[5].ToString())
                {
                    case "Fiducia1":
                        Fiducia = "F1";
                        Directorio = Repositorio + @"\Fiducia1\Procesos\" + año + "\\" + mes + "\\";
                        break;
                    case "Fiducia3":
                        Fiducia = "F3";
                        Directorio = Repositorio + @"\Fiducia3\Procesos\" + año + "\\" + mes + "\\";
                        break;
                    default:
                        break;
                }

                int nregistrosprocesados = 0;
                int suma = 0;
                int nregistrosexistentes = 0;
                int npagosguardados = 0;
                int error = 0;

                if (!Directory.Exists(Directorio))
                {
                    System.IO.Directory.CreateDirectory(Directorio);
                }

                /// <summary>
                /// Trae el lote maximo que existe y lo aumenta en 1
                /// </summary>
                List<string[,]> ListPagos = PagoValdLN.ConsultaLoteMaximo("ConsultaMaxLotePagosTarjeta");
                string[,] ArregloLote = ListPagos[0];
                int LoteMaximo = Convert.ToInt32(ArregloLote[0, 1].ToString());
                LoteMaximo++;

                #region LECTURA DE ARCHIVO

                while (sr.EndOfStream == false)
                {
                    string linea = sr.ReadLine();

                    if (Convert.ToInt32(linea.Substring(0, 2)) == 01)
                    {
                        FechaRecaudo = linea.Substring(12, 4) + "/" + linea.Substring(16, 2) + "/" + linea.Substring(18, 2);
                        FechaSico = linea.Substring(14, 6);
                        partefija = NombreArchivo.Replace(".txt", "");
                        partefija = partefija.Substring(0, 6);

                        ConsultaCodigoBanco = ConsultaPagosTarjeta.ValidaCodigoBanco(partefija, Fiducia, "ConsultaLugarPagoTarjeta");

                        string[,] ArregloCodigo = ConsultaCodigoBanco[0];
                        CodBanco = ArregloCodigo[0, 1].ToString();
                        NombreBanco = ArregloCodigo[2, 1].ToString();
                        RutaEpicor = ArregloCodigo[4, 1].ToString();

                        CarpetaBanco = Directorio + "\\" + "PAGOSTARJETA" + "\\" + NombreBanco.Replace(" ", "") + "\\";

                        if (!Directory.Exists(CarpetaBanco))
                        {
                            System.IO.Directory.CreateDirectory(CarpetaBanco);
                        }

                        RegistrosProcesados = new StreamWriter(CarpetaBanco + NombreBanco + fecha + ".txt", false, Encoding.GetEncoding(1252));
                    }
                    else
                    {
                        if (Convert.ToInt32(linea.Substring(0, 2)) == 06)
                        {


                            franquicia = linea.Substring(159, 20);
                            Valor = Convert.ToInt32(linea.Substring(427, 12));
                            ValdObjetos.pCodBanco = CodBanco;
                            ValdObjetos.pValVenta = Convert.ToInt32(linea.Substring(50, 12));
                            ValdObjetos.pNumAutorizacion = linea.Substring(74, 12).TrimStart('0');
                            ValdObjetos.pCodUnico = Convert.ToInt32(linea.Substring(179, 20));
                            ValdObjetos.pCuenta = linea.Substring(199, 20);
                            ValdObjetos.pFecVale = linea.Substring(241, 4) + "/" + linea.Substring(245, 2) + "/" + linea.Substring(247, 2);
                            ValdObjetos.pFecProceso = linea.Substring(271, 4) + "/" + linea.Substring(275, 2) + "/" + linea.Substring(277, 2);
                            ValdObjetos.pFecAbono = linea.Substring(301, 4) + "/" + linea.Substring(305, 2) + "/" + linea.Substring(307, 2);
                            ValdObjetos.pNumTarjeta = linea.Substring(309, 30).TrimStart('0');
                            ValdObjetos.pHora = linea.Substring(339, 30).TrimStart('0');
                            ValdObjetos.pComprobante = linea.Substring(369, 11).TrimStart('0');
                            ValdObjetos.pTerminal = linea.Substring(384, 15);
                            ValdObjetos.pValIva = Convert.ToInt32(linea.Substring(399, 12));
                            ValdObjetos.pValPropina = Convert.ToInt32(linea.Substring(413, 12));
                            ValdObjetos.pValTotal = Valor;
                            ValdObjetos.pTipTarjeta = linea.Substring(455, 30);
                            ValdObjetos.pPlazo = linea.Substring(485, 3);
                            ValdObjetos.pPorComision = linea.Substring(488, 8);
                            TipoOperacion = Convert.ToInt32(linea.Substring(68, 6));

                            ValdObjetos.pNumLote = Convert.ToString(LoteMaximo.ToString());


                            if (ConfigurationManager.AppSettings["CodBanco"].ToString().Contains("," + Convert.ToInt32(CodBanco) + ","))
                            {

                                if (Convert.ToInt32(linea.Substring(157, 2)) > 0)
                                    ValdObjetos.pValRetFuente = Convert.ToInt32(linea.Substring(145, 14));
                                else
                                    ValdObjetos.pValRetFuente = Convert.ToInt32(linea.Substring(145, 12));

                                if (Convert.ToInt32(linea.Substring(115, 2)) > 0)
                                    ValdObjetos.pValComision = Convert.ToInt32(linea.Substring(103, 14));
                                else
                                    ValdObjetos.pValComision = Convert.ToInt32(linea.Substring(103, 12));

                                if (Convert.ToInt32(linea.Substring(129, 2)) > 0)
                                    ValdObjetos.pValRetIva = Convert.ToInt32(linea.Substring(117, 14));
                                else
                                    ValdObjetos.pValRetIva = Convert.ToInt32(linea.Substring(117, 12));

                                if (Convert.ToInt32(linea.Substring(143, 2)) > 0)
                                    ValdObjetos.pValRetIca = Convert.ToInt32(linea.Substring(131, 14));
                                else
                                    ValdObjetos.pValRetIca = Convert.ToInt32(linea.Substring(131, 12));

                                if (Convert.ToInt32(linea.Substring(453, 2)) > 0)
                                    ValdObjetos.pValAbono = Convert.ToInt32(linea.Substring(441, 14));
                                else
                                    ValdObjetos.pValAbono = Convert.ToInt32(linea.Substring(441, 12));
                            }
                            else
                            {
                                ValdObjetos.pValRetFuente = Convert.ToInt32(linea.Substring(145, 12));
                                ValdObjetos.pValComision = Convert.ToInt32(linea.Substring(103, 12));
                                ValdObjetos.pValRetIva = Convert.ToInt32(linea.Substring(117, 12));
                                ValdObjetos.pValRetIca = Convert.ToInt32(linea.Substring(131, 12));
                                ValdObjetos.pValAbono = Convert.ToInt32(linea.Substring(441, 12));
                            }


                            if (CodBanco == "43") //Tarjeta Occidente
                            {
                                switch (ValdObjetos.pNumTarjeta.Substring(0, 1))
                                {
                                    case "4":
                                        ValdObjetos.pFranquicia = "VNS";
                                        break;
                                    case "5":
                                        ValdObjetos.pFranquicia = "MNS";
                                        break;
                                    case "6":
                                        ValdObjetos.pFranquicia = "MNS";
                                        break;
                                    default:
                                        ValdObjetos.pFranquicia = "TARJETA OCCIDENTE";
                                        break;
                                }

                                if (TipoOperacion != 0)
                                    ValdObjetos.pLegalizado = "R";
                                else
                                    ValdObjetos.pLegalizado = "N";
                            }
                            else
                            {
                                ValdObjetos.pLegalizado = "N";
                                if (franquicia == " ".PadLeft(20, ' '))
                                    ValdObjetos.pFranquicia = NombreBanco;
                                else
                                    ValdObjetos.pFranquicia = franquicia.TrimEnd();
                            }

                            suma = Valor + suma;

                            /// <summary>
                            /// Valida si la referencia existe en pagos tarjeta y si no la inserta en la tabla pagos tarjeta
                            /// </summary>
                            List<string[,]> ConsultaTarjeta = ConsultaPagosTarjeta.ValidaExistePagoTarjeta(ValdObjetos, "ConsultaExistePagoTarjeta");

                            if (ConsultaTarjeta.Count > 0)
                            {
                                if (encabezado == "")
                                {
                                    RegistrosProcesados.WriteLine("BANCO".PadRight(7, ' ') + "AUTORIZACION".PadRight(15, ' ') + "VALOR".PadRight(16, ' ') + "FRANQUICIA".PadRight(20, ' ') + "FECHA".PadRight(11, ' ') + "OBSERVACIÓN");
                                    encabezado = "S";
                                }
                                RegistrosProcesados.WriteLine(CodBanco.PadRight(7, ' ') + ValdObjetos.pNumAutorizacion.PadRight(14, ' ') + " $" + Convert.ToString(Valor).PadRight(15, ' ') + ValdObjetos.pFranquicia.PadRight(20, ' ') + ValdObjetos.pFecVale.Replace("/", "") + "   REGISTRO YA EXISTE EN VENTAS");
                                nregistrosprocesados++;
                                nregistrosexistentes++;
                            }
                            else
                            {
                                string GuardaRegistrosPago = ConsultaPagosTarjeta.IUDPagosTarjeta(ValdObjetos, "InsertPagosTarjeta");
                                if (GuardaRegistrosPago.Substring(0, 1) != "0")
                                {
                                    if (encabezado == "")
                                    {
                                        RegistrosProcesados.WriteLine("BANCO".PadRight(7, ' ') + "AUTORIZACION".PadRight(15, ' ') + "VALOR".PadRight(16, ' ') + "FRANQUICIA".PadRight(20, ' ') + "FECHA".PadRight(11, ' ') + "OBSERVACIÓN");
                                        encabezado = "S";
                                    }

                                    RegistrosProcesados.WriteLine(CodBanco.PadRight(7, ' ') + ValdObjetos.pNumAutorizacion.PadRight(14, ' ') + " $" + Convert.ToString(Valor).PadRight(15, ' ') + ValdObjetos.pFranquicia.PadRight(20, ' ') + ValdObjetos.pFecVale.Replace("/", "") + "   SE GUARDO CORRECTAMENTE EN VENTAS");
                                    nregistrosprocesados++;
                                    npagosguardados++;
                                }
                                else
                                {
                                    RegistrosProcesados.WriteLine(CodBanco.PadRight(7, ' ') + ValdObjetos.pNumAutorizacion.PadRight(14, ' ') + " $" + Convert.ToString(Valor).PadRight(15, ' ') + ValdObjetos.pFranquicia.PadRight(20, ' ') + ValdObjetos.pFecVale.Replace("/", "") + "   OCURRIO UN ERROR AL GUARDAR");
                                    error++;
                                }

                            }
                        }
                    }
                }


                /// <summary>
                /// Convierte en formato numero el valor total de los registros procesados
                /// </summary>
                double valor = Convert.ToDouble(suma);
                string resultsuma = Convert.ToString(suma);
                resultsuma = valor.ToString("0,0", CultureInfo.InvariantCulture);
                resultsuma = String.Format(CultureInfo.InvariantCulture, "{0:0,0}", valor);

                RegistrosProcesados.WriteLine("TOTAL VALOR: $" + resultsuma);
                RegistrosProcesados.WriteLine("TOTAL REGISTROS: " + nregistrosprocesados);

                RegistrosProcesados.Close();
                sr.Close();
                string legalizados = (PagoValdLN.Legalizacionpagos("UpdateLegalizaciones"));
                String legalizados2 = (PagoValdLN.Legalizacionpagos("UpdateLegalizacionesPagoCuota"));

                int legalizacionFinal = Convert.ToInt32(legalizados) + Convert.ToInt32(legalizados2);

                #endregion


                /// <summary>
                ///Si el proceso continuo correctamente, lo mueve de la ruta origen a la ruta de procesados
                /// </summary>
                RutaOrigen = System.IO.Path.Combine(RutaArchivo + NombreArchivo);
                RutaDestino = System.IO.Path.Combine(RutaEpicor + NombreArchivo + fecha);
                System.IO.File.Move(RutaOrigen, RutaDestino);

                /// <summary>
                /// Envio de correo electronico
                /// </summary>
                Correo = Util.EnvioMail(" ", "PAGOS MASIVOS DEL BANCO " + NombreBanco, "Buen día, \n\n" +
                "A continuación se envia un detallado de los pagos procesados a traves del banco " + NombreBanco + " generado el "
                + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Now.Year.ToString() + " a las "
                + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Second.ToString().PadLeft(2, '0') + " \n\n" +
                "1. Registros nuevos a tarjeta de credito: " + npagosguardados + " registros. \n" +
                "2. Registros existentes en ventas: " + nregistrosexistentes + " registros. \n" +
                "3. Registros legalizados: " + legalizacionFinal + " registros. \n" +
                "4. Errores presentados: " + error + " registros. \n\n" +
                "Total Valor $ " + resultsuma + "\n\n" +
                "Total registros procesados: " + nregistrosprocesados + " registros. \n\n" +
                "Número de lote: " + LoteMaximo + ".\n\n" +
                "El archivo de resultados lo encontrara en la ruta: " + CarpetaBanco + ". \n\n" +
                "La información contenida en este E-mail es confidencial y solo puede ser utilizada por el individuo o la compania " +
                "a la cual esta dirigido. Si no es el receptor autorizado, cualquier retención, difusión, distribución o copia" +
                "de este mensaje es prohibida y será sancionada por la ley. Si por error recibe este mensaje, " +
                "favor reenviarlo al remitente y borrar el mensaje recibido inmediatamente.", ConfigurationManager.AppSettings["CorreoTo"].ToString(), ConfigurationManager.AppSettings["CorreoFrom"].ToString(), ConfigurationManager.AppSettings["CorreoCC"].ToString());

                return "PROCESO REALIZADO CON EXITO";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Metodo que crea el archivo de SICO con la estructura Davivienda
        /// </summary>
        public void CrearArchivoSico(string NombreArchivo)
        {

            if (ControlaHora >= 23)
            {
                ControlaHora = 1;
                if (ControlaMinutos >= 59)
                {
                    ControlaMinutos = 1;
                }
            }
            else
            {
                ControlaHora++;
                ControlaMinutos++;
            }


            tipomovimiento = linea.Substring(97, 2);


            if (!NombreArchivo.Contains("Acreedores"))
            {
                if (tipomovimiento.Contains("DE") || tipomovimiento.Contains("ND") || tipomovimiento.Contains("NC"))
                {
                }
                else
                {
                    /*PAGOPI*/
                    if (npagosguardadosPI > 0)
                    {
                        if (FormaPago == "EFECTIVO")
                            ArchivoSico.WriteLine(FechaSico + "P" + Sucursal.PadLeft(9, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        else
                            ArchivoSico.WriteLine(FechaSico + "P" + Sucursal.PadLeft(9, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "00" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                    }
                    else
                    {
                        if (FormaPago == "EFECTIVO")
                            ArchivoSico.WriteLine(FechaSico + Sucursal.PadLeft(10, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        else
                            ArchivoSico.WriteLine(FechaSico + Sucursal.PadLeft(10, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "00" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                    }



                }
            }
        }

        public void CrearArchivoSicoTarjeta(string NombreArchivo, String pCodBanco)
        {

            if (ControlaHora >= 23)
            {
                ControlaHora = 1;
                if (ControlaMinutos >= 59)
                {
                    ControlaMinutos = 1;
                }
            }
            else
            {
                ControlaHora++;
                ControlaMinutos++;
            }


            tipomovimiento = linea.Substring(97, 2);


            if (!NombreArchivo.Contains("Acreedores"))
            {
                if (tipomovimiento.Contains("DE") || tipomovimiento.Contains("ND") || tipomovimiento.Contains("NC"))
                {
                }
                else
                {
                    if (pCodBanco == CodBancoVisaMarterCardVentas)
                    {
                        /*PAGOPI*/
                        if (npagosguardadosPI > 0)
                        {

                            if (FormaPago == "TARJETA")
                                ArchivoVisaMAstercardSICO.WriteLine(FechaSico + "P" + Sucursal.PadLeft(9, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        }
                        else
                        {
                            if (FormaPago == "TARJETA")
                                ArchivoVisaMAstercardSICO.WriteLine(FechaSico + Sucursal.PadLeft(10, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        }
                    }

                    else if (pCodBanco == CodBancoAmexVentas)
                    {
                        /*PAGOPI*/
                        if (npagosguardadosPI > 0)
                        {

                            if (FormaPago == "TARJETA")
                                ArchivoAmexSICO.WriteLine(FechaSico + "P" + Sucursal.PadLeft(9, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        }
                        else
                        {
                            if (FormaPago == "TARJETA")
                                ArchivoAmexSICO.WriteLine(FechaSico + Sucursal.PadLeft(10, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        }
                    }

                    else if (pCodBanco == CodBancoDinnersVentas)
                    {
                        /*PAGOPI*/
                        if (npagosguardadosPI > 0)
                        {

                            if (FormaPago == "TARJETA")
                                ArchivoDinnersSICO.WriteLine(FechaSico + "P" + Sucursal.PadLeft(9, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        }
                        else
                        {
                            if (FormaPago == "TARJETA")
                                ArchivoDinnersSICO.WriteLine(FechaSico + Sucursal.PadLeft(10, '0') + "DE" + Convert.ToString(Valor).PadLeft(10, '0') + "".PadRight(12, '0') + "".PadRight(7, '0') + Convert.ToString(Referencia).PadLeft(10, '0') + DigitoVerificacion + Convert.ToString(ControlaHora).PadLeft(2, '0') + Convert.ToString(ControlaMinutos).PadLeft(2, '0') + "000000");
                        }
                    }


                }
            }
        }

        public bool insertarRecaudo { get; set; }
    }
}
