using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using log4net;

namespace Bancos.AD.Conexion
{
    public class QuerysAux
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public QuerysAux()
        {
            Registrador = LogManager.GetLogger(this.GetType());
            Error = String.Empty;
        }

        #region "Funcion: consultarDatos(String procedimientoAlmacenado, SqlParameterCollection parametros, String cadenaCx)"
        /// <summary>
        /// Consulta datos, ejecutando un procedimiento almacenado
        /// </summary>
        /// <param name="procedimientoAlmacenado">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros"></param>
        /// <param name="cadenaCx"></param>
        /// <returns>Datos que se recuprearon de la consulta</returns>
        public DataSet consultarDatos(String procedimientoAlmacenado, List<SqlParameter> parametros, String cadenaCx)
        {
            ConectorBDAux objConexionDB = ConectorBDAux.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion(cadenaCx);
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter(procedimientoAlmacenado, conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                adaptador.SelectCommand.Parameters.AddRange(parametros.ToArray());
                datos = new DataSet();
                adaptador.Fill(datos, "tabla");
                adaptador.Dispose();
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
                Registrador.Error(ex.Message);
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
