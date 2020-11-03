using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class RecaudoLN
    {
        WcfData wsc = new Pagos.AD.Servicios.WcfData();

        public List<string[,]> ValidaExisteRecaudo(ObjetoTablas ObjRecaudo, string procedimiento)
        {
            ObjetoTablas objRecaudoValidacion = new ObjetoTablas();
            List<string[,]> listaRecaudoValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaExisteCierre = new List<ObjetoTablas>();
                string[, ,] Valor = new string[2, 3, 1];

                Valor[0, 0, 0] = ObjRecaudo.pFecPago.Replace('/', '-').Substring(0, 11);
                Valor[0, 1, 0] = "@Fecha";
                Valor[0, 2, 0] = "datetime";

                Valor[1, 0, 0] = ObjRecaudo.pParteFija;
                Valor[1, 1, 0] = "@ParteFija";
                Valor[1, 2, 0] = "varchar(max)";

                return listaRecaudoValida = wsc.LlenarLista(Valor, procedimiento, "SQLBancos", "SP", "Sql");

            }
            catch (Exception)
            {
                return listaRecaudoValida;
            }
        }

        public string insertaRecaudo(ObjetoTablas ValdObjetos, string procedimiento)
        {
            try
            {
                string[, ,] Valor = new string[6, 3, 1];

                Valor[0, 0, 0] = ValdObjetos.pCodBanco.ToString();
                Valor[0, 1, 0] = "@LugarDePago";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = ValdObjetos.pFecPagoBancos;
                Valor[1, 1, 0] = "@FecPago";
                Valor[1, 2, 0] = "date";

                Valor[2, 0, 0] = ValdObjetos.pValPago;
                Valor[2, 1, 0] = "@ValPago";
                Valor[2, 2, 0] = "numeric(18,0)";

                Valor[3, 0, 0] = ValdObjetos.pContrato;
                Valor[3, 1, 0] = "@Referencia";
                Valor[3, 2, 0] = "varchar(50)";

                Valor[4, 0, 0] = ValdObjetos.pReferenciaPago;
                Valor[4, 1, 0] = "@Cupo";
                Valor[4, 2, 0] = "varchar(50)";

                Valor[5, 0, 0] = ValdObjetos.pParteFija;
                Valor[5, 1, 0] = "@ParteFija";
                Valor[5, 2, 0] = "varchar(20)";



                return wsc.Ejecutar(Valor, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string[,]> ConsultarRegistrosIngresados(ObjetoTablas ObjRecaudo, string procedimiento)
        {
            ObjetoTablas objRecaudoValidacion = new ObjetoTablas();
            List<string[,]> listaRecaudoValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaExisteCierre = new List<ObjetoTablas>();
                string[, ,] Valor = new string[2, 3, 1];

                Valor[0, 0, 0] = ObjRecaudo.pFecPago.Replace('/', '-').Substring(0, 11);
                Valor[0, 1, 0] = "@Fecha";
                Valor[0, 2, 0] = "datetime";

                Valor[1, 0, 0] = ObjRecaudo.pParteFija;
                Valor[1, 1, 0] = "@ParteFija";
                Valor[1, 2, 0] = "varchar(max)";

                return listaRecaudoValida = wsc.LlenarLista(Valor, procedimiento, "SQLBancos", "SP", "Sql");

            }
            catch (Exception)
            {
                return new List<string[,]>();
            }
        }

        public List<string[,]> consultarDisponibilidad(string CodBanco, string partefija)
        {
            
            List<string[,]> listaDisponibiliad = new List<string[,]>();
          
            try
            {
                string[, ,] Valor = new string[2, 3, 1];

                Valor[0, 0, 0] = CodBanco;
                Valor[0, 1, 0] = "@LugarPago";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = partefija ;
                Valor[1, 1, 0] = "@ParteFija";
                Valor[1, 2, 0] = "varchar(25)";

           

                return listaDisponibiliad = wsc.LlenarLista(Valor, "pa_ban_Consulta_disponibilidad_banco", "SQLBancos", "SP", "Sql");
   
            }
            catch (Exception ex)
            {
                return listaDisponibiliad;
            }

        }


        public string updateDisponibilidad(string CodBanco, string partefija,string disponibilidad)
        {
            string res = "";
            try
            {
                string[, ,] Valor = new string[3, 3, 1];

                Valor[0, 0, 0] = CodBanco;
                Valor[0, 1, 0] = "@LugarPago";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = disponibilidad=="1" ? "True":"False";
                Valor[1, 1, 0] = "@disponibilidad";
                Valor[1, 2, 0] = "bool";

                Valor[2, 0, 0] = partefija;
                Valor[2, 1, 0] = "@ParteFija";
                Valor[2, 2, 0] = "varchar(25)";

                return res = wsc.Ejecutar(Valor, "pa_ban_update_disponibilidad_banco", "SQLBancos");
            }
            catch (Exception ex)
            {
                return res + " " + ex.Message;
            }

        }
    }
}
