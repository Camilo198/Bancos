using System;
using System.Collections.Generic;
using System.IO;

namespace Procesos.PS.Codigo
{
    public class LectorArchivos
    {     
        public LectorArchivos()
        {
            //XmlConfigurator.Configure();
        }

        public List<String> leerArchivo(String rutaArchivo)
        {
            List<String> listaLineas = new List<String>();
            try
            {
                StreamReader objLector = new StreamReader(rutaArchivo, System.Text.Encoding.GetEncoding(1252));
                String strLinea = "";
                while (strLinea != null)
                {
                    strLinea = objLector.ReadLine();
                    if (strLinea != null && strLinea != String.Empty)
                        listaLineas.Add(strLinea);
                }
                objLector.Close();
            }
            catch 
            {
                throw new Exception(); 
            }
            return listaLineas;
        }

        public void borrarArchivo(String rutaArchivo)
        {
            try
            {
                File.Delete(rutaArchivo);
            }
            catch
            {
                throw new Exception();
            }
        }

        public void moverArchivo(String rutaArchivo, String rutaDestino)
        {
            try
            {
                File.Move (rutaArchivo,rutaDestino);
            }
            catch
            {
                throw new Exception();
            }
        }

        public List<String> listarDirectorio(String directorio)
        {
            List<String> lista = new List<String>();

            if (!Directory.Exists(directorio))
            {
                try
                {
                    Directory.CreateDirectory(directorio);
                }
                catch
                {
                    throw new Exception();
                }
            }
            try
            {
                lista.AddRange(Directory.GetFiles(directorio));
                //lista.AddRange(Directory.GetFiles(directorio, "*.txt"));
            }
            catch
            {
                throw new Exception();
            }
            return lista;
        }

        public List<String> listarDirectorioPGP(String directorio)
        {
            List<String> lista = new List<String>();

            if (!Directory.Exists(directorio))
            {
                try
                {
                    Directory.CreateDirectory(directorio);
                }
                catch
                {
                    throw new Exception();
                }
            }
            try
            {
                lista.AddRange(Directory.GetFiles(directorio, "*.pgp"));
            }
            catch
            {
                throw new Exception();
            }
            return lista;
        }

    }
}
