using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Starksoft.Cryptography.OpenPGP;
using System.IO;
using System.Configuration;

namespace Procesos.PS.Procesos
{
   public class DesEnciptar
    {
        private GnuPG gpg = new GnuPG();
        private string appPath;

        #region Constructors

        public DesEnciptar()
        {

        }

        public DesEnciptar(string appPath)
        {
            this.appPath = appPath;
        }

        #endregion
       /// <summary>
       /// Este codigo no funciona 
       /// </summary>
       /// <param name="encryptedSourceFile"></param>
       /// <param name="decryptedFile"></param>
       /// <returns></returns>
        public FileInfo DecryptFile(string encryptedSourceFile, string decryptedFile)
        {
            // check parameters
            if (string.IsNullOrEmpty(encryptedSourceFile))
                throw new ArgumentException("encryptedSourceFile parameter is either empty or null", "encryptedSourceFile");
            if (string.IsNullOrEmpty(decryptedFile))
                throw new ArgumentException("decryptedFile parameter is either empty or null", "decryptedFile");
            try
            {
                using (FileStream encryptedSourceFileStream = new FileStream(encryptedSourceFile, FileMode.Open))
                {
                    //  make sure the stream is at the start.
                    encryptedSourceFileStream.Position = 0;

                    using (FileStream decryptedFileStream = new FileStream(decryptedFile, FileMode.Create))
                    {
                        //  Specify the directory containing gpg.exe (again, not sure why).
                        gpg.BinaryPath = Path.GetDirectoryName(@"C:\Program Files (x86)\GNU\GnuPG\pub\");
                        //gpg.BinaryPath = Path.GetDirectoryName(appPath);
                        gpg.Recipient = @"\\sbogche016v\ARCHPLANOS\Pagos\Recaudo\GNB\Prueba\";
                        //  Decrypt
                       // gpg.Decrypt (encryptedSourceFileStream, decryptedFileStream);
                        gpg.DecryptAsync(encryptedSourceFileStream, decryptedFileStream);
                    }
                }
                return new FileInfo(decryptedFile);
            }
            catch (Exception exc)
            {
                return new FileInfo(decryptedFile);
            }
        }

        public FileInfo EncryptFile(string keyUserId, string sourceFile, string encryptedFile)
        {
            // check parameters
            if (string.IsNullOrEmpty(keyUserId))
                throw new ArgumentException("keyUserId parameter is either empty or null", "keyUserId");
            if (string.IsNullOrEmpty(sourceFile))
                throw new ArgumentException("sourceFile parameter is either empty or null", "sourceFile");
            if (string.IsNullOrEmpty(encryptedFile))
                throw new ArgumentException("encryptedFile parameter is either empty or null", "encryptedFile");


            // Create streams - for each of the unencrypted source file and decrypted destination file
            using (Stream sourceFileStream = new FileStream(sourceFile, FileMode.Open))
            {
                using (Stream encryptedFileStream = new FileStream(encryptedFile, FileMode.Create))
                {
                    //  Specify the directory containing gpg.exe (not sure why).
                    gpg.BinaryPath = Path.GetDirectoryName(appPath);
                    gpg.Recipient = keyUserId;

                    //  Perform encryption
                    gpg.Encrypt(sourceFileStream, encryptedFileStream);
                    return new FileInfo(encryptedFile);
                }
            }
        }
    }
}
