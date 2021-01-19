using Pagos.EN.Tablas;
using Pagos.LN;
using Pagos.LN.Consulta;
using Renci.SshNet;
using SSH;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceBancos
{
    public partial class Pruebas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Pagina para realizar debug al servicio
            WebServiceBancos.WsBancos prueba = new WebServiceBancos.WsBancos();

            //Descomentariar pruebas
            //string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE020V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_01012018_PO31122017_0000.txt", "S");
            //Comentariar a produccion
            //string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\Recaudo\BBVA\Recibidos\", @"20110522479CO.MR5", "N");

            //string res = prueba.ProcesarPagosSinSubir("", "");

            string resultado = prueba.PagosTarjeta("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\TC\Bancolombia\Procesados\", "00015_18012021_TC_01012021_172459_A.txt");


            //string ServidorSico = ConfigurationManager.AppSettings["server"].ToString();            /*PAGOS*/
            //string UsuarioSico = ConfigurationManager.AppSettings["user"].ToString();               /*PAGOS*/
            //string PasswordSico = ConfigurationManager.AppSettings["password"].ToString();          /*PAGOS*/

            //String comando = "/bin/sh /usr2/sico/backend/run_util.sh " + "itfvtasf3auS" + " 006331";
            //SSHConect Conexion = new SSHConect();  /*PAGOS*/
            //Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);

        }
        public void testArchivo()
        {
            string PathSystem = "/export/home/SYSTEM/"; // SAU 08.09.2020
            string ServidorSico = "172.16.30.7";
            //IR023201001220048 
            //R023201001220048
            string UsuarioSico = ConfigurationManager.AppSettings["user"].ToString();               /*PAGOS
            string PasswordSico = ConfigurationManager.AppSettings["password"].ToString();          /*PAGOS*/
            string NombreArchivoSico = "023201001220048.txt";
            string RutaSico = "ftp://172.16.30.7/";
            string UsuFTP = "chevyplan\\usaftpinfo";
            string PassFTP = "Chevy789";
            string Fechapago = "2020-10-01";
            int codigobanco = 23;
            WcfUtilidades util = new WcfUtilidades();
            IList<String> texto = null;

            SSHConect CON = new SSHConect();

            Pagos.LN.WcfUtilidades Util = new Pagos.LN.WcfUtilidades();
            RptPagosLN pagosLN = new RptPagosLN();
            pagosLN.almacenaRegistroSicoLN(Util, ServidorSico, NombreArchivoSico, PathSystem, UsuFTP, PassFTP,
                                        Convert.ToInt32("029"), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now, "00015_");
        }
        public void aplicaManualSico()
        {
            // Aplicacion de pagos en sico manual

            //SSHConect Conexion = new SSHConect();
            //Pagos.LN.WcfUtilidades Util = new Pagos.LN.WcfUtilidades();

            //string RutaEpicor = @"\\sbogche016v\ARCHPLANOS\Pagos\Fiducia1\TC\Codensa\Historico\";
            //string NombreArchivoSico = "071201221164222.txt";
            //string RutaSico = "ftp://172.16.30.7/";
            //string UsuFTP = "chevyplan\\usaftp";
            //string PassFTP = "Chevy789";
            //string informacion = "";
            //string comando = "";
            //string NombreComando = ConfigurationManager.AppSettings["comando"].ToString();          /*PAGOS*/
            //string NombrePrograma = ConfigurationManager.AppSettings["NombrePrograma"].ToString();  /*PAGOS*/
            //string ServidorSico = ConfigurationManager.AppSettings["server"].ToString();            /*PAGOS*/
            //string UsuarioSico = ConfigurationManager.AppSettings["user"].ToString();               /*PAGOS*/
            //string PasswordSico = ConfigurationManager.AppSettings["password"].ToString();          /*PAGOS*/

            //string exporasico = Util.UploadFTP(RutaEpicor + NombreArchivoSico, RutaSico, UsuFTP, PassFTP);
            //if (exporasico == "OK")
            //{
            //    informacion = "Nombre archivo Efectivo + Cheques para SICO: " + NombreArchivoSico + ". \n";
            //    /*PAGOS*/
            //    //Se encarga de aplicar directamente en SICO
            //    comando = NombreComando + NombrePrograma + " " + NombreArchivoSico;
            //    try
            //    {
            //        Conexion.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, comando);
            //    }
            //    catch (Exception ex) { }
            //}

        }
    }
}