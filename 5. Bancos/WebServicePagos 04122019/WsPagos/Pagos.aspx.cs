using Pagos.LN;
using Pagos.LN.Consulta;
using Renci.SshNet;
using SSH;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            //string resultado = prueba.LecturaPagos("", "", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Fiducia1\\Recaudo\\Bogota\\Recibidos\\", "05003146.OW3", "N");
            //string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_06102020_PO06102020_0002_PRUEBA_SAU66.txt", "S");
            string resultado = prueba.PagosTarjeta("", "", @"\\SBOGCHE016V\ArchPlanos\PAGOS\Fiducia1\TC\Bancolombia\Procesados\", "00015_14102020_TC_21092020_174531_A.txt");
        }
        public void pruebaSSH()
        {
            string PathSystem = "/export/home/SYSTEM/"; // SAU 08.09.2020
            string ServidorSico = "172.16.30.7";
            //IR023201001220048 
            //R023201001220048
            string UsuarioSico = ConfigurationManager.AppSettings["user"].ToString();               /*PAGOS*/
            string PasswordSico = ConfigurationManager.AppSettings["password"].ToString();          /*PAGOS*/
            string NombreArchivoSico = "023201001220048.txt";
            string RutaSico = "ftp://172.16.30.7/";
            string UsuFTP = "chevyplan\\usaftpinfo";
            string PassFTP = "Chevy789";
            string Fechapago = "2020-10-01";
            int codigobanco = 23;
            WcfUtilidades util = new WcfUtilidades();
            IList<String> texto = null;
            KeyboardInteractiveAuthenticationMethod kauth = new KeyboardInteractiveAuthenticationMethod(UsuFTP);
            PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(UsuFTP, PassFTP);

            kauth.AuthenticationPrompt += new EventHandler<Renci.SshNet.Common.AuthenticationPromptEventArgs>(HandleKeyEvent);
            string[] myArray = new string[1];
            myArray[0] = UsuFTP;
            AuthenticationMethod[] auths = new AuthenticationMethod[1];
            auths[0] = new PasswordAuthenticationMethod(UsuFTP, PassFTP);
            ConnectionInfo connectionInfo = new ConnectionInfo(ServidorSico, "22", auths[0], pauth, kauth);

            SSHConect CON = new SSHConect();

            try
            {
                CON.conecta_Server(ServidorSico, UsuarioSico, PasswordSico, "cp /export/home/SYSTEM/IR066200928083125.txt /usr2/sico/mega/planos/");
                //using (SshCommand c = new SshCommand()) // SshClient client = new SshClient(...)
                //{
                //    client.Connect();

                //    using (SshCommand command = client.CreateCommand("cat printerimport_2014-02-28_03.21.41.log"))
                //    {
                //        label01.Text = command.Execute();
                //    }
                //}

                //SftpClient cliente = new SftpClient(ServidorSico, UsuFTP, PassFTP);
                //cliente.Connect();
                //texto = cliente.ReadAllLines(RutaSico + NombreArchivoSico);

                string res = util.DownloadFTP("\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Fiducia1\\", "IR066200928083125.txt", RutaSico, UsuFTP, PassFTP);

            }
            catch (Exception ex)
            {

                string error = ex.Message;
            }


            //util.LeerFicheroFTP(ServidorSico, "IR" + NombreArchivoSico, PathSystem, UsuFTP, PassFTP, Fechapago, codigobanco);

            //String RutaProcesado = @"\\sbogche016v\ArchPlanos\Pagos\Fiducia1\TC\Bancolombia\Procesados\";
            //System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(RutaProcesado);

            //System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            //foreach (System.IO.FileInfo archivos in fileNames)
            //{
            //    string resultado = prueba.PagosTarjeta("", "", RutaProcesado, archivos.Name);
            //}
        }
        public void HandleKeyEvent(Object sender, Renci.SshNet.Common.AuthenticationPromptEventArgs e)
        {
            foreach (Renci.SshNet.Common.AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = "prueba123";
                }
            }
        }
    }
}