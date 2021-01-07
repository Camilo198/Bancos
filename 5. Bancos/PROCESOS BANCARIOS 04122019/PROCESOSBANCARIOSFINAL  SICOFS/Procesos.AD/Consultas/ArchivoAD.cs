using Procesos.AD.Servicios;
using Procesos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Procesos.AD.Consultas
{
    public class ArchivoAD
    {
        WcfData wsc = new WcfData();
        public String insertarArchivoBolsaAD(String procedimiento, ArchivoEN objEntidad)
        {
            try
            {
                string[,,] Valor = new string[3, 3, 1];

                Valor[0, 0, 0] = objEntidad.Fecha.ToString();
                Valor[0, 1, 0] = "@inFecha";
                Valor[0, 2, 0] = "datetime";

                Valor[1, 0, 0] = objEntidad.RutaArchivo;
                Valor[1, 1, 0] = "@inRutaArchivo";
                Valor[1, 2, 0] = "varchar(max)";

                Valor[2, 0, 0] = "I";
                Valor[2, 1, 0] = "@inOperacion";
                Valor[2, 2, 0] = "varchar(2)";


                return wsc.Ejecutar(Valor, procedimiento, "SQLBancos");

            }
            catch (Exception ex)
            {
              return (ex.Message);
            }
        }
        public IList<ArchivoEN> consultarArchivoBolsaAD(String procedimiento, ArchivoEN objEntidad)
        {
            List<ArchivoEN> listParametro = new List<ArchivoEN>();
            List<string[,]> lista = new List<string[,]>();
            try
            {
                string[,,] Param = new string[3, 3, 1];

                Param[0, 0, 0] = objEntidad.Fecha.ToString();
                Param[0, 1, 0] = "@inFecha";
                Param[0, 2, 0] = "datetime";

                Param[1, 0, 0] = objEntidad.RutaArchivo;
                Param[1, 1, 0] = "@inRutaArchivo";
                Param[1, 2, 0] = "varchar(max)";

                Param[2, 0, 0] = "C";
                Param[2, 1, 0] = "@inOperacion";
                Param[2, 2, 0] = "varchar(2)";

                lista = wsc.LlenarLista(Param, procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        ArchivoEN objParametros = new ArchivoEN();
                        Valida = lista[i];

                        objParametros.Fecha = Convert.ToDateTime(Valida[0, 1]);
                        objParametros.RutaArchivo = Valida[1, 1].ToString();
                        listParametro.Add(objParametros);
                    }
                }

                return listParametro;
            }
            catch (Exception)
            {
                return listParametro;
            }
        }
        public String elimiarArchivoBolsaAD(String procedimiento, ArchivoEN objEntidad, String Operacion)
        {
            try
            {
                string[,,] Param = new string[3, 3, 1];

                Param[0, 0, 0] = objEntidad.Fecha.ToString();
                Param[0, 1, 0] = "@inFecha";
                Param[0, 2, 0] = "date";

                Param[1, 0, 0] = objEntidad.RutaArchivo;
                Param[1, 1, 0] = "@inRutaArchivo";
                Param[1, 2, 0] = "varchar(max)";

                Param[2, 0, 0] = Operacion;
                Param[2, 1, 0] = "@inOperacion";
                Param[2, 2, 0] = "varchar(2)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
