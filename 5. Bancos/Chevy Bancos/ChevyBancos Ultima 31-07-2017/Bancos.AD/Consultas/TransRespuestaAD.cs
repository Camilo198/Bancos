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
    public class TransRespuestaAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TransRespuestaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(TransRespuesta objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Trans_Respuesta", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRespuestaTAsoba", SqlDbType.VarChar));
                if (objEntidad.pRespuestaTAsoba > 0)
                {
                    adaptador.SelectCommand.Parameters["@pRespuestaTAsoba"].Value = objEntidad.pRespuestaTAsoba;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRespuestaTAsoba"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRespuestaTBanco", SqlDbType.VarChar));
                if (objEntidad.pRespuestaTBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pRespuestaTBanco"].Value = objEntidad.pRespuestaTBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRespuestaTBanco"].Value = DBNull.Value;
                }

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

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos</returns>
        public List<TransRespuesta> consultar(TransRespuesta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<TransRespuesta> lista = new List<TransRespuesta>();
            TransRespuesta objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new TransRespuesta();
                objEntidad2.pRespuestaTAsoba = Convertidor.aEntero32(fila[TransRespuestaDEF.RespuestaTAsoba]);
                objEntidad2.pRespuestaTBanco = Convertidor.aEntero32(fila[TransRespuestaDEF.RespuestaTBanco]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="banco"></param>
        /// <param name="tablaAso"></param>
        /// <param name="tablaBanco"></param>
        /// <returns></returns>
        public List<TransRespuesta> consultar(String banco, String tablaBanco, String tablaAso)
        {
            String query = "SELECT rt.DESCRIPCION_ESTANDAR AS DescEstandarBanco, rt.CAUSAL AS CausalBanco, tr.Respuesta_T_Banco,"
                + " rt2.DESCRIPCION_ESTANDAR AS DescEstandarAsobancaria, rt2.CAUSAL AS CausalAso, tr.Respuesta_T_Asoba"
                + " FROM tb_BAN_TRANS_RESPUESTA AS tr"
                + " INNER JOIN tb_BAN_RESPUESTA_TRANSACCION AS rt ON tr.Respuesta_T_Banco = rt.OID"
                + " INNER JOIN tb_BAN_TABLA AS t ON rt.Tabla = t.OID"
                + " INNER JOIN tb_BAN_RESPUESTA_TRANSACCION AS rt2 ON tr.Respuesta_T_Asoba = rt2.OID"
                + " INNER JOIN tb_BAN_TABLA AS t2 ON rt2.Tabla = t2.OID"
                + " WHERE (t.Banco = '" + banco + "') AND (t.OID = " + tablaBanco + ") AND (t2.OID = " + tablaAso + ") AND (t2.ES_ASOBANCARIA = 1)";

            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;

            List<TransRespuesta> lista = new List<TransRespuesta>();
            TransRespuesta objEntidad = null;
            foreach (DataRow fila in datos.Rows)
            {
                objEntidad = new TransRespuesta();
                objEntidad.pCodigoRespuestaTAsoba = Convertidor.aCadena(fila["CausalAso"]);
                objEntidad.pCodigoRespuestaTBanco = Convertidor.aCadena(fila["CausalBanco"]);
                objEntidad.pDetalleRespuestaTAsoba = Convertidor.aCadena(fila["DescEstandarAsobancaria"]);
                objEntidad.pDetalleRespuestaTBanco = Convertidor.aCadena(fila["DescEstandarBanco"]);
                objEntidad.pRespuestaTAsoba = Convertidor.aEntero32(fila[TransRespuestaDEF.RespuestaTAsoba]);
                objEntidad.pRespuestaTBanco = Convertidor.aEntero32(fila[TransRespuestaDEF.RespuestaTBanco]);

                lista.Add(objEntidad);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(TransRespuesta objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Warn(ex.Message);
            }
            return cuenta;
        }
    }
}
