using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;

namespace Pagos.AD.Conexion
{
    internal class ConectorBD
    {
        private ConectorBD() { }
        private static ConectorBD conexion = null;

        /// <summary>
        /// Establece la conexion con la base de datos.
        /// </summary>
        /// <returns>Conexion con la base de datos, para realizar transacciones</returns>
        public SqlConnection abrirConexionSQL()
        {
            SqlConnection objSqlConect = new SqlConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
                objSqlConect.ConnectionString = cadenaConexion;
                objSqlConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objSqlConect;
        }

        public SqlConnection abrirConexionSQLVentas()
        {
            SqlConnection objSqlConect = new SqlConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SQLVentas"].ConnectionString;
                objSqlConect.ConnectionString = cadenaConexion;
                objSqlConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objSqlConect;
        }

        public SqlConnection abrirConexionSQLChevysat()
        {
            SqlConnection objSqlConect = new SqlConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SQLCHEVY"].ConnectionString;
                objSqlConect.ConnectionString = cadenaConexion;
                objSqlConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objSqlConect;
        }

        public OdbcConnection abrirConexionVFP()
        {
            OdbcConnection objOdbcConect = new OdbcConnection();
            try
            {
                //String cadenaConexion = "Data Source=SBOGCHE001d;Initial Catalog=ventasdb;User ID=ventas;Password=ventas";
                String cadenaConexion = ConfigurationManager.ConnectionStrings["VFP"].ConnectionString;
                objOdbcConect.ConnectionString = cadenaConexion;
                objOdbcConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objOdbcConect;
        }

        public OdbcConnection abrirConexionVFP_HISTORICO()
        {
            OdbcConnection objOdbcConect = new OdbcConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["VFP_HISTORICO"].ConnectionString;
                objOdbcConect.ConnectionString = cadenaConexion;
                objOdbcConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objOdbcConect;
        }

        public OdbcConnection abrirConexionSICO()
        {
            OdbcConnection objOdbcConect = new OdbcConnection();
            try
            {
                String cadenaConexion = ConfigurationManager.ConnectionStrings["SICO"].ConnectionString;
                objOdbcConect.ConnectionString = cadenaConexion;
                objOdbcConect.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objOdbcConect;
        }

        /// <summary>
        /// Retorna la instacia de la coneción actual, o la crea en caso de que este nula
        /// </summary>
        /// <returns>Instancia de la conexion actual</returns>
        public static ConectorBD obtenerInstancia()
        {
            if (conexion == null)
                conexion = new ConectorBD();
            return conexion;
        }
    }
}
