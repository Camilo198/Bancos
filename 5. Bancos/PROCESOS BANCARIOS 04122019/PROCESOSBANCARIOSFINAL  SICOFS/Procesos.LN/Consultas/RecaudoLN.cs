using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Procesos.AD.Servicios;
namespace Procesos.LN.Consultas
{
    public class RecaudoLN
    {
        WcfData wsc = new WcfData();

        public List<string[,]> consultarDisponibilidad()
        {

            List<string[,]> listaDisponibiliad = new List<string[,]>();

            try
            {
                string[, ,] Valor = new string[0, 3, 1];



                return listaDisponibiliad = wsc.LlenarLista(Valor,"pa_ban_Consulta_disponibilidad_banco", "SQLBan", "SP", "Sql");

            }
            catch (Exception ex)
            {
                return listaDisponibiliad;
            }

        }
    }
}
