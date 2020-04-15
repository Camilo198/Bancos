using Procesos.AD.Servicios;
using Procesos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.AD.Consultas
{
    public class CorreoAD
    {
        WcfData clientdb = new WcfData();
        IList<String[,]> lista = new List<String[,]>();

        public List<Correo> ConsultaCorreos(Correo objEntidad)
        {
            List<Correo> listaCorreos = new List<Correo>();

            try
            {
                String[, ,] Param = new String[1, 3, 1];

                Param[0, 0, 0] = objEntidad.id.ToString();
                Param[0, 1, 0] = "@Id";
                Param[0, 2, 0] = "int";

                lista = clientdb.LlenarLista(Param, "pa_Ban_ConsultaCorreos", "SQLBan", "SP", "Sql");
                string[,] Validalista_;

                if (lista.Count > 0)
                {
                    Correo objCorreos = new Correo();

                    Validalista_ = lista[0];

                    objCorreos.id = Convert.ToInt32(Validalista_[0, 1].ToString());
                    objCorreos.mailFrom = Validalista_[1, 1].ToString();
                    objCorreos.mailTo = Validalista_[2, 1].ToString();
                    objCorreos.mailCC = Validalista_[3, 1].ToString();
                    objCorreos.mailInfra = Validalista_[4, 1].ToString();
                    objCorreos.mailSopo = Validalista_[5, 1].ToString();
                    objCorreos.contromail = Validalista_[6, 1].ToString();
                    listaCorreos.Add(objCorreos);
                }

                return listaCorreos;
            }
            catch (Exception)
            {
                return listaCorreos;
            }

        }

        public string UpdateDisponiblidadCorreo(string p, Correo objEntidad)
        {
            try
            {
                string[, ,] Valor = new string[2, 3, 1];

                Valor[0, 0, 0] = objEntidad.id.ToString();
                Valor[0, 1, 0] = "@Id";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = objEntidad.contromail;
                Valor[1, 1, 0] = "@valor";
                Valor[1, 2, 0] = "bit";


                return clientdb.Ejecutar(Valor, p, "SQLBan");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


       
        
    }
}
