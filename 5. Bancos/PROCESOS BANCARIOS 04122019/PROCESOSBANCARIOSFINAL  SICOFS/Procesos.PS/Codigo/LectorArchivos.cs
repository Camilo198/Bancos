using Procesos.AD.Consultas;
using Procesos.EN.Tablas;
using Procesos.LN.Consultas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                File.Move(rutaArchivo, rutaDestino);
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
                lista.RemoveAll(x => x.Contains(".db"));
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
        public List<ArchivoEN> procesarArchivosFecha(List<String> archivo)
        {
            List<ArchivoEN> lista_ax = new List<ArchivoEN>();
            IList<ArchivoEN> lista_BD = new List<ArchivoEN>();
            IList<ArchivoEN> lista_eliminados = new List<ArchivoEN>();

            foreach (var item in archivo)
            {
                lista_ax.Add(new ArchivoEN()
                {
                    Fecha = File.GetCreationTime(item), //.ToString("yyyy-MM-dd")
                    RutaArchivo = item
                });
            }

            archivo.Clear();

            lista_ax.Sort(delegate (ArchivoEN x, ArchivoEN y)
            {
                if (x.Fecha == null && y.Fecha == null) return 0;
                else if (x.Fecha == null) return -1;
                else if (y.Fecha == null) return 1;
                else return x.Fecha.CompareTo(y.Fecha);
            });

            //Se comentarea porque en runtime se puede manejar
            // el control de los ficheros
            // la tabla no existe en PRD
            ArchivoEN archivoEN = new ArchivoEN();
            ArchivoLN archivoLN = new ArchivoLN();
            archivoEN.Fecha = DateTime.Now;
            archivoEN.RutaArchivo = "";

            lista_BD = archivoLN.consultarArchivoBolsaLN(archivoEN);
                
            if (lista_BD.Count > 0)
            {
                lista_eliminados = lista_BD.Where(x => !lista_ax.Any(y => x.RutaArchivo == y.RutaArchivo)).ToList();
                foreach (var item in lista_eliminados)
                {
                    archivoLN.eliminarArchivoBolsaLN(item, "D");
                }
            }

            foreach (var item in lista_ax)
            {
                archivo.Add(item.RutaArchivo);
                archivoEN.Fecha = item.Fecha;
                archivoEN.RutaArchivo = item.RutaArchivo;
                String res = archivoLN.insertarArchivoBolsaLN(archivoEN);
            }
            return lista_ax;
        }
    }
}
