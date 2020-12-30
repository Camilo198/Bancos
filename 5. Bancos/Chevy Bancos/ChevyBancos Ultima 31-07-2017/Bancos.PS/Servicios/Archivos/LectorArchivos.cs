using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Config;

namespace Bancos.PS.Servicios.Archivos
{
    public class LectorArchivos
    {
        private static readonly ILog objLog = LogManager.GetLogger(typeof(LectorArchivos));

        public LectorArchivos()
        {
            XmlConfigurator.Configure();
        }

        public List<String> leerArchivoTarjetas(String rutaArchivo)
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
                        listaLineas.Add(strLinea.Replace(@".", ""));//REVISAR ARCHIVOS AL QUITARLES EL PUNTO
                }
                objLector.Close();
            }
            catch (Exception ex)
            {
                objLog.Error(ex.Message);
            }
            return listaLineas;
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
            catch (Exception ex)
            {
                objLog.Error(ex.Message);
            }
            return listaLineas;
        }

        public List<String> leerArchivoTarjetasCSV(String rutaArchivo, List<Bancos.EN.Tablas.EstructuraArchivo> ListaDeEstructuraArchivoBanco)
        {
            List<String> listaLineas = new List<String>();
            try
            {
                StreamReader objLector = new StreamReader(rutaArchivo, System.Text.Encoding.GetEncoding(1252));
                String strLinea = "";
                int cont = 0;
                while (strLinea != null)
                {
                    strLinea = objLector.ReadLine();
                    if (strLinea != null && strLinea != String.Empty)
                    {
                        //strLinea = strLinea.Replace(@";", " ");
                        //strLinea = strLinea.Replace(@",", " ");

                        string[] listaLinea = strLinea.Split(',');

                        for (int i = 0; i < listaLinea.Length; i++)
                        {
                            foreach (var item in ListaDeEstructuraArchivoBanco)
                            {
                                cont = i + 1;
                                if (item.pOrdenColumna == cont)  // Se colocan espacios para completar de acuerdo a la long parametrizada
                                {
                                    var len = item.pLongitud;
                                    listaLinea[i] = listaLinea[i].PadRight(Convert.ToInt32(len), ' ');
                                    break;
                                }
                            }
                            cont = 0;
                        }
                        strLinea = "";
                        for (int i = 0; i < listaLinea.Length; i++)
                        {
                            strLinea = strLinea + listaLinea[i];
                        }
                        listaLineas.Add(strLinea);//REVISAR ARCHIVOS AL QUITARLES LA COMA
                    }
                }
                objLector.Close();
            }
            catch (Exception ex)
            {
                objLog.Error(ex.Message);
            }
            return listaLineas;
        }

        public void borrarArchivo(String rutaArchivo)
        {
            try
            {
                File.Delete(rutaArchivo);
            }
            catch (Exception ex)
            {
                objLog.Error(ex.Message);
            }
        }

        public void moverArchivo(String rutaArchivo, String RutaProcesado, String TipoArchivo, String NombreArchivo)
        {
            try
            {
                if (rutaArchivo.Contains(".xls"))
                    File.Move(rutaArchivo, RutaProcesado + "Recaudo_" + DateTime.Now.ToString("yyyyMMdd") + "_" + writeMilitaryTime(DateTime.Now) + DateTime.Now.ToString("ss") + ".xls");
                else
                    File.Move(rutaArchivo, RutaProcesado + "Recaudo_" + DateTime.Now.ToString("yyyyMMdd") + "_" + writeMilitaryTime(DateTime.Now) + DateTime.Now.ToString("ss") + NombreArchivo + ".txt");
            }
            catch
            {
                throw new Exception("Archivo " + rutaArchivo + " no se pudo mover a ruta de procesados, borrelo manualmente para evitar pagos duplicados");
                //objLog.Error(ex.Message);
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
                catch (Exception ex)
                {
                    objLog.Error(ex.Message);
                }
            }
            try
            {
                lista.AddRange(Directory.GetFiles(directorio, "*.txt"));
                lista.AddRange(Directory.GetFiles(directorio, "*.OW3"));
                lista.AddRange(Directory.GetFiles(directorio, "*.RDC"));
                lista.AddRange(Directory.GetFiles(directorio, "*."));
                lista.AddRange(Directory.GetFiles(directorio, "*.xls"));
                lista.AddRange(Directory.GetFiles(directorio, "*.csv"));
                lista.AddRange(Directory.GetFiles(directorio, "*.FIL"));
                lista.AddRange(Directory.GetFiles(directorio, "*.inf"));
            }
            catch (Exception ex)
            {
                objLog.Error(ex.Message);
            }
            return lista;
        }

        //SE CONVIERTE UNA HORA NORMAL A HORA MILITAR
        private String writeMilitaryTime(DateTime date)
        {
            string value = date.ToString("HHmm");
            return value;
        }


    }
}