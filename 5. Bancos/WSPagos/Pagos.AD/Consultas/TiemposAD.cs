using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pagos.AD.Servicios;
using Pagos.EN.Tablas;

namespace Pagos.AD.Consultas

{
 public   class TiemposAD
    {

        WcfData wsc = new WcfData();

        //Retorna en una lista los parametros de la fecha de aplicacion del banco.
        public IList<ObjetoTablas> ConsultarHoraAplicacionBancoAD(string Procedimiento, ObjetoTablas objEntidad)
        {
            List<ObjetoTablas> listParametro = new List<ObjetoTablas>();
            List<string[,]> lista = new List<string[,]>();

            try
            {
                string[, ,] Param = new string[1, 3, 1]; // solo cuando el procedimiento almacenado tiene parametros


                Param[0, 0, 0] = objEntidad.pCodBanco.ToString();
                Param[0, 1, 0] = "@CodBanco";
                Param[0, 2, 0] = "varchar(10)";


                lista = wsc.LlenarLista(Param, Procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        ObjetoTablas objParametros = new ObjetoTablas();
                        Valida = lista[i];

                        objParametros.pCodBanco = Valida[0, 1].ToString();
                        objParametros.pHoraBancoAplicacion = Valida[1, 1].ToString();
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

        public IList<ObjetoTablas> ConsultarSleepAplicacionBancoAD(string Procedimiento, ObjetoTablas objEntidad)
        {
            List<ObjetoTablas> listParametro = new List<ObjetoTablas>();
            List<string[,]> lista = new List<string[,]>();

            try
            {
                string[, ,] Param = new string[1, 3, 1]; // solo cuando el procedimiento almacenado tiene parametros


                Param[0, 0, 0] = objEntidad.pCodBanco.ToString();
                Param[0, 1, 0] = "@CodBanco";
                Param[0, 2, 0] = "varchar(10)";


                lista = wsc.LlenarLista(Param, Procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        ObjetoTablas objParametros = new ObjetoTablas();
                        Valida = lista[i];

                        objParametros.pCodBanco = Valida[0, 1].ToString();
                        objParametros.pSleepMinutosAntes = Convert.ToInt32(Valida[1, 1].ToString());
                        objParametros.pSleepMinutosDespues = Convert.ToInt32(Valida[2, 1].ToString());
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
    }
}
