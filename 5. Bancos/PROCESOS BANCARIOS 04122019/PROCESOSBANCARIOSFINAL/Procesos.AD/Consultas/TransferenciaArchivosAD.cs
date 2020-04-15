using Procesos.AD.Servicios;
using Procesos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.AD.Consultas
{
    public class TransferenciaArchivosAD
    {
        WcfData wsc = new AD.Servicios.WcfData();
        IList<String[,]> lista = new List<String[,]>();


        public string ActualizaRutas(TransferenciaArchivos objEntidad)
        {

            string[, ,] Valor = new string[2, 3, 1];

            Valor[0, 0, 0] = objEntidad.id;
            Valor[0, 1, 0] = "@id";
            Valor[0, 2, 0] = "int";

            Valor[1, 0, 0] = objEntidad.nomArchivo;
            Valor[1, 1, 0] = "@lote";
            Valor[1, 2, 0] = "varchar(6)";

            return wsc.Ejecutar(Valor, "ActualizaRutas", "SQLTransArchivos");

        }


        public string ActualizaDescifrado(TransferenciaArchivos objEntidad)
        {

            string[, ,] Valor = new string[4, 3, 1];

            Valor[0, 0, 0] = objEntidad.id;
            Valor[0, 1, 0] = "@id";
            Valor[0, 2, 0] = "int";

            Valor[1, 0, 0] = objEntidad.nomArchivo;
            Valor[1, 1, 0] = "@rango";
            Valor[1, 2, 0] = "varchar(300)";

            Valor[2, 0, 0] = "1";
            Valor[2, 1, 0] = "@descifra";
            Valor[2, 2, 0] = "int";

            Valor[3, 0, 0] = objEntidad.rutaRepositorio;
            Valor[3, 1, 0] = "@rutaDestino";
            Valor[3, 2, 0] = "varchar(300)";

            return wsc.Ejecutar(Valor, "ActualizaRutaDescifrar", "SQLTransArchivos");


        }



        public List<TransferenciaArchivos> consultaDisponibilidadDescifrado(TransferenciaArchivos objEntidad)
        {
            List<TransferenciaArchivos> listaRutas = new List<TransferenciaArchivos>();

            try 
	{	        
		 string[, ,] Valor = new string[1, 3, 1];

            Valor[0, 0, 0] = objEntidad.id;
            Valor[0, 1, 0] = "@id";
            Valor[0, 2, 0] = "int";

            lista=wsc.LlenarLista(Valor, "ConsultaDisponibilidadDescifrar", "SQLTransArchivos", "SP", "Sql");
            string[,] Validalista_;
            if (lista.Count>0)
            {
                TransferenciaArchivos objEntidadAux= new TransferenciaArchivos();
                Validalista_ = lista[0];
                objEntidadAux.id=Validalista_[0, 1].ToString();
                objEntidadAux.descifra = Validalista_[10, 1].ToString();

                listaRutas.Add(objEntidadAux);
                

            }

	}
	catch (Exception ex)
	{

        return listaRutas;
	}


            return listaRutas;

        }
    }
    }

