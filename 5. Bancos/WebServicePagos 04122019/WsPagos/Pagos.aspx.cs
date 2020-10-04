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

            //Descomentariar
            // string resultado = prueba.LecturaPagos("", "", @"\\SBOGCHE020V\ArchPlanos\PAGOS\Fiducia1\Recaudo\PagosOnline\Recibidos\", "00015_13022020_PO12022020_0000.txt", "S");
            //Comentariar a produccion
            //string resultado = prueba.LecturaPagos("", "", "\\\\sbogche016v\\ARCHPLANOS\\Pagos\\Fiducia1\\Recaudo\\Bogota\\Recibidos\\", "05003146.OW3", "N");
            string PathSystem = "/export/home/SYSTEM/"; // SAU 08.09.2020
            string ServidorSico = "172.16.30.5";
            //IR023201001220048 
            //R023201001220048
            string NombreArchivoSico = "023201001220048.txt";
            string RutaSico = "ftp://172.16.30.5/";
            string UsuFTP = "userbackend";
            string PassFTP = "prueba123";
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
            CON.conecta_Server(ServidorSico, UsuFTP, PassFTP, "cat /export/home/SYSTEM/IR023201001220048.txt");
            try
            {
 
                SftpClient cliente = new SftpClient(ServidorSico, UsuFTP, PassFTP);
                cliente.Connect();
                texto = cliente.ReadAllLines(RutaSico + NombreArchivoSico);
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