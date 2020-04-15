using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using log4net;

namespace RutasFtp.AD.Conexion
{
    internal class ConectorBDAux
    {
        public bool SeEstablecioConexion { get; set; }
        private ConectorBDAux() { }
        private static ConectorBDAux conexion = null;

        /// <summary>
        /// Establece la conexion con la base de datos.
        /// </summary>
        /// <returns>Conexion con la base de datos, para realizar transacciones</returns>
        public SqlConnection abrirConexion(String cadenaCx)
        {
            String cadenaConexion = cadenaCx;
            SqlConnection objSqlConect = new SqlConnection();
            objSqlConect.ConnectionString = cadenaConexion;
            try
            {
                objSqlConect.Open();
                SeEstablecioConexion = true;
            }
            catch
            {
                SeEstablecioConexion = false;
            }
            return objSqlConect;
        }

        /// <summary>
        /// Retorna la instacia de la coneción actual, o la crea en caso de que este nula
        /// </summary>
        /// <returns>Instancia de la conexion actual</returns>
        public static ConectorBDAux obtenerInstancia()
        {
            if (conexion == null)
                conexion = new ConectorBDAux();
            return conexion;
        }
    }
}
