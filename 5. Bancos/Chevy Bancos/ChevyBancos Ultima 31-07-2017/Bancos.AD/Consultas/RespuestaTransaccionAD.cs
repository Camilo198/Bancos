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
    public class RespuestaTransaccionAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public RespuestaTransaccionAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(RespuestaTransaccion objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Respuesta_Transaccion", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCausal", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCausal))
                {
                    adaptador.SelectCommand.Parameters["@pCausal"].Value = objEntidad.pCausal;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCausal"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDescripcionEstandar", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDescripcionEstandar))
                {
                    adaptador.SelectCommand.Parameters["@pDescripcionEstandar"].Value = objEntidad.pDescripcionEstandar;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDescripcionEstandar"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDetalleAdicional", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDetalleAdicional))
                {
                    adaptador.SelectCommand.Parameters["@pDetalleAdicional"].Value = objEntidad.pDetalleAdicional;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDetalleAdicional"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOid", SqlDbType.VarChar));
                if (objEntidad.pOid > 0)
                {
                    adaptador.SelectCommand.Parameters["@pOid"].Value = objEntidad.pOid;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pOid"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPrenotificacion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pPrenotificacion))
                {
                    adaptador.SelectCommand.Parameters["@pPrenotificacion"].Value = objEntidad.pPrenotificacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pPrenotificacion"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTabla", SqlDbType.VarChar));
                if (objEntidad.pTabla > 0)
                {
                    adaptador.SelectCommand.Parameters["@pTabla"].Value = objEntidad.pTabla;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTabla"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTransaccionDebito", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTransaccionDebito))
                {
                    adaptador.SelectCommand.Parameters["@pTransaccionDebito"].Value = objEntidad.pTransaccionDebito;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTransaccionDebito"].Value = DBNull.Value;
                }

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
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos</returns>
        public List<RespuestaTransaccion> consultar(RespuestaTransaccion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<RespuestaTransaccion> lista = new List<RespuestaTransaccion>();
            RespuestaTransaccion objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new RespuestaTransaccion();
                objEntidad2.pCausal = Convertidor.aCadena(fila[RespuestaTransaccionDEF.Causal]);
                objEntidad2.pDescripcionEstandar = Convertidor.aCadena(fila[RespuestaTransaccionDEF.DescripcionEstandar]);
                objEntidad2.pDetalleAdicional = Convertidor.aCadena(fila[RespuestaTransaccionDEF.DetalleAdicional]);
                objEntidad2.pOid = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.Oid]);
                objEntidad2.pPrenotificacion = Convertidor.aCadena(fila[RespuestaTransaccionDEF.Prenotificacion]);
                objEntidad2.pTabla = Convertidor.aEntero32(fila[RespuestaTransaccionDEF.Tabla]);
                objEntidad2.pTransaccionDebito = Convertidor.aCadena(fila[RespuestaTransaccionDEF.TransaccionDebito]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(RespuestaTransaccion objEntidad)
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

        public DataTable obtenerCausales(String codigoBanco)
        {
            String query = "SELECT rtb.CAUSAL AS CausalBanco, rta.CAUSAL AS CausalAsobancaria"
                + " FROM tb_BAN_RESPUESTA_TRANSACCION AS rtb"
                + " INNER JOIN tb_BAN_TABLA AS tb ON rtb.Tabla = tb.OID"
                + " INNER JOIN tb_BAN_TRANS_RESPUESTA AS t ON rtb.OID = t.Respuesta_T_Banco"
                + " INNER JOIN tb_BAN_RESPUESTA_TRANSACCION AS rta"
                + " INNER JOIN tb_BAN_TABLA AS ta ON rta.Tabla = ta.OID ON t.Respuesta_T_Asoba = rta.OID"
                + " WHERE (tb.Banco = '" + codigoBanco + "') AND (ta.ES_ASOBANCARIA = 1)";

            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;

            return datos;
        }
    }
}
