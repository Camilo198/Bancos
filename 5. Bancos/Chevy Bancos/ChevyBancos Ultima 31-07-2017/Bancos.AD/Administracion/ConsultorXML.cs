﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Bancos.EN;
using log4net;

namespace Bancos.AD.Administracion
{
    public class ConsultorXML
    {
        public String RutaXML { get; set; }
        public String Error { get; set; }
        public ILog Registrador { get; set; }

        public ConsultorXML()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Lee un campo del archivo xml de configuraciones
        /// </summary>
        /// <param name="objCamposXML">Valores del campo que se necesita consultar, tabla y campo</param>
        /// <returns>Valor del campo solicitado</returns>
        public String leerDatosXML(CamposXML objCamposXML)
        {
            DataSet dsDatos = new DataSet();
            String strValor = String.Empty;
            
            try
            {
                dsDatos.ReadXml(RutaXML);
                strValor = dsDatos.Tables[objCamposXML.pTabla].Rows[0][objCamposXML.pCampo].ToString();
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
                Registrador.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                Registrador.Error(ex.Message);
            }
            
            return strValor;
        }

        /// <summary>
        /// Modifica una atributo del xml de configuraciones
        /// </summary>
        /// <param name="objCamposXML">Valores del campo que se necesita modificar, tabla, campo y valor</param>
        public void modificarXML(CamposXML objCamposXML)
        {
            try
            {
                DataSet dsDatos = new DataSet();
                dsDatos.ReadXml(RutaXML);
                dsDatos.Tables[objCamposXML.pTabla].Rows[0][objCamposXML.pCampo] = objCamposXML.pValor;
                dsDatos.WriteXml(RutaXML);
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
                Registrador.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                Registrador.Error(ex.Message);
            }
        }

        /// <summary>
        /// Trae el valor de la cadena de conexion a la base de datos.
        /// </summary>
        /// <returns>Valor de la cadena de conexion</returns>
        public String leerCadenaConexion()
        {
            DataSet dsDatos = new DataSet();
            String strValor = String.Empty;

            try
            {
                dsDatos.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\Modulos\\XML\\Configuracion.xml");
                strValor = desencriptar(dsDatos.Tables["BD"].Rows[0]["A"].ToString());
            }
            catch (System.Security.SecurityException ex)
            {
                Error = "ERROR: " + ex.Message;
                Registrador.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                Registrador.Error(ex.Message);
            }
            
            return strValor;
        }

        /// <summary> 
        /// Método para desencriptar un texto encriptado
        /// </summary>
        /// <returns>Texto desencriptado</returns> 
        protected String desencriptar(String textoEncriptado)
        {
            Byte[] vectorInicialBytes = Encoding.ASCII.GetBytes("@045Yu)9T7aFD_N:");
            Byte[] valorRellenoBytes = Encoding.ASCII.GetBytes("94&6s#ts");
            Byte[] textoCifradoBytes = Convert.FromBase64String(textoEncriptado);

            PasswordDeriveBytes clave = new PasswordDeriveBytes("7sJ04LQ#$@453mAr4X", valorRellenoBytes, "MD5", 1);
            Byte[] claveBytes = clave.GetBytes(256 / 8);

            RijndaelManaged claveSimetrica = new RijndaelManaged()
            {
                Mode = CipherMode.CBC
            };

            ICryptoTransform desencriptador = claveSimetrica.CreateDecryptor(claveBytes, vectorInicialBytes);
            MemoryStream objMemoryStream = new MemoryStream(textoCifradoBytes);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, desencriptador, CryptoStreamMode.Read);

            Byte[] textoBytes = new Byte[textoCifradoBytes.Length];
            int decryptedByteCount = objCryptoStream.Read(textoBytes, 0, textoBytes.Length);

            objMemoryStream.Close();
            objCryptoStream.Close();

            String textoOriginal = Encoding.UTF8.GetString(textoBytes, 0, decryptedByteCount);
            return textoOriginal;
        }
    }
}
