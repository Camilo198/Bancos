using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.AD.Consultas
{
    public class FechaUsuraAD
    {
        WcfData wsc = new WcfData();

        //Retorna en una lista los parametros de la fecha de aplicacion del banco.
        public IList<FechaUsuraEN> ConsultarFechaUsuraAD(string Procedimiento)
        {
            List<FechaUsuraEN> listParametro = new List<FechaUsuraEN>();
            List<string[,]> lista = new List<string[,]>();

            try
            {
                string[,,] Param = new string[0, 0, 0]; // solo cuando el procedimiento almacenado tiene parametros

                lista = wsc.LlenarLista(Param, Procedimiento, "SQLBancos", "SP", "Sql");
                string[,] Valida;

                if (lista.Count > 0)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        FechaUsuraEN objParametros = new FechaUsuraEN();
                        Valida = lista[i];

                        objParametros.fechaUsura = Valida[0, 1].ToString();
                        
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
