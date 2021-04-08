using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Data.Odbc;

namespace Pagos.AD.Conexion
{
    public class Querys
    {
       
        #region "Funcion: consultarDatos VFP(String odbc)"
        /// <summary>
        /// Consulta datos del comando sql enviado
        /// </summary>
        /// <param name="sql">Comando sql</param>
        /// <returns>Datos que se recuprearon de la consulta</returns>
        public DataSet consultarDatosVFP(String odbc)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            OdbcDataAdapter adaptador;
            DataSet datos = null;
            OdbcConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexionVFP();
                adaptador = new OdbcDataAdapter();
                adaptador.SelectCommand = new OdbcCommand(odbc, conexion);
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }
        #endregion

        #region "Funcion: consultarDatos SICO(String odbc)"
        /// <summary>
        /// Consulta datos del comando sql enviado
        /// </summary>
        /// <param name="sql">Comando sql</param>
        /// <returns>Datos que se recuprearon de la consulta</returns>
        public DataSet consultarDatosSICO(String odbc)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            OdbcDataAdapter adaptador;
            DataSet datos = null;
            OdbcConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexionSICO();
                adaptador = new OdbcDataAdapter();
                adaptador.SelectCommand = new OdbcCommand(odbc, conexion);
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }
        #endregion

        #region "Funcion: consultarDatos SQL(String sql)"
        /// <summary>
        /// Consulta datos del comando sql enviado
        /// </summary>
        /// <param name="sql">Comando sql</param>
        /// <returns>Datos que se recuprearon de la consulta</returns>
        public DataSet consultarDatosSQL(String sql)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection  conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexionSQL();
                adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = new SqlCommand(sql, conexion);
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }
        
        #endregion

        #region "Funcion: consultarDatos SQLVentas(String sql)"
        /// <summary>
        /// Consulta datos del comando sql enviado
        /// </summary>
        /// <param name="sql">Comando sql</param>
        /// <returns>Datos que se recuprearon de la consulta</returns>
        public DataSet consultarDatosSQLVentas(String sql)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexionSQLVentas();
                adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = new SqlCommand(sql, conexion);
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }

        public DataSet consultarDatosSQLChevysat(String sql)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexionSQLChevysat();
                adaptador = new SqlDataAdapter();
                adaptador.SelectCommand = new SqlCommand(sql, conexion);
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }

        #endregion

    }
}
