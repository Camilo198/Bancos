using System;
using System.Collections.Generic;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;

namespace Bancos.LN.Consultas
{
    public class LupaLN
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos</returns>
        public List<Lupa> consultar(int FechaL)
        {
           // objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            LupaAD objConsultor = new LupaAD();
            List<Lupa> lista = new List<Lupa>();
            lista = objConsultor.consultar(FechaL);
            Error = objConsultor.Error;
            return lista;
        }

   }
}
