using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Bancos.AD.Conexion;
using Bancos.EN;
using Bancos.EN.Definicion;
using Bancos.EN.Tablas;
using log4net;

namespace Bancos.AD.Consultas
{
    public class PagosOnlineAd
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public PagosOnlineAd()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        public DataSet ejecutarConsulta()
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_PagosOnLine", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFecLimite", SqlDbType.Date));
                adaptador.SelectCommand.Parameters["@pFecLimite"].Value = DateTime.Now.Date;
                
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
                Registrador.Error(Error);
            }
            finally
            {
                if (conexion.State != ConnectionState.Closed)
                    conexion.Close();
            }

            return datos;
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        public string  ejecutarUpdate()
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlConnection conexion = null;
            conexion = objConexionDB.abrirConexion();
            int rowCount = 0;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "pa_ban_Actualiza_PagosOnLine";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
          
            try
            {
                SqlParameter Parameter = cmd.Parameters.Add("@RowCount", SqlDbType.Int);
                Parameter.Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                rowCount = (Int32)cmd.Parameters["@RowCount"].Value;
                return rowCount.ToString().Trim();
            }
            catch (Exception e)
            {
               return "0" + e.Message;
            }
            finally
            {
                cmd.Dispose();
                conexion.Close();
                conexion.Dispose();
            }
        }
    }
}
