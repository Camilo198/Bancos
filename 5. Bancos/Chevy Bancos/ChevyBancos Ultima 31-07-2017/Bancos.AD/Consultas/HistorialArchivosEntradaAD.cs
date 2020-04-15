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
    public class HistorialArchivosEntradaAD
    {

          /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public HistorialArchivosEntradaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }


        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(HistorialArchivosEntrada objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Historial_Archivos_Entrada", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFecha", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFecha))
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = objEntidad.pFecha;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaTransaccion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaTransaccion))
                {
                    adaptador.SelectCommand.Parameters["@pFechaTransaccion"].Value = objEntidad.pFechaTransaccion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaTransaccion"].Value = DBNull.Value;
                }


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdCuentaBanco", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pIdCuentaBanco))
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = objEntidad.pIdCuentaBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = objEntidad.pTipoArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pConsecutivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pConsecutivo))
                {
                    adaptador.SelectCommand.Parameters["@pConsecutivo"].Value = objEntidad.pConsecutivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pConsecutivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pLineasArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pLineaDetalle))
                {
                    adaptador.SelectCommand.Parameters["@pLineasArchivo"].Value = objEntidad.pLineaDetalle;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pLineasArchivo"].Value = DBNull.Value;
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
        public List<HistorialArchivosEntrada> consultar(HistorialArchivosEntrada objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<HistorialArchivosEntrada> lista = new List<HistorialArchivosEntrada>();
            HistorialArchivosEntrada objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new HistorialArchivosEntrada();
                objEntidad2.pFecha = Convertidor.aCadena(fila[HistorialArchivosEntradaDEF.Fecha]);
                objEntidad2.pFechaTransaccion = Convertidor.aCadena(fila[HistorialArchivosEntradaDEF.FechaTransaccion]);
                objEntidad2.pIdCuentaBanco = Convertidor.aCadena(fila[HistorialArchivosEntradaDEF.IdCuentaBanco]);
                objEntidad2.pTipoArchivo = Convertidor.aCadena(fila[HistorialArchivosEntradaDEF.TipoArchivo]);
                objEntidad2.pConsecutivo = Convertidor.aCadena(fila[HistorialArchivosEntradaDEF.Consecutivo]);
                objEntidad2.pLineaDetalle = Convertidor.aCadena(fila[HistorialArchivosEntradaDEF.LineaDetalle]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        public int ejecutarNoConsulta(HistorialArchivosEntrada objEntidad)
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

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }

        //BUSQUEDA DE TODOS LOS CONSECUTIVOS DEL ARCHIVO PAGOS ONLINE CON COINCIDENCIA A UN BANCO, FECHA Y FECHA_TRANSACCION
        public DataTable consultarConsecutivoXBanco(String IdCuenta, String TipoArchivoS, String Fecha, String FechaTransaccion)
        {
            String query = "SELECT CONSECUTIVO FROM tb_BAN_HISTORIAL_ARCHIVOS_ENTRADA"
                + " WHERE ID_CUENTA_BANCO = '" + IdCuenta + "' AND TIPO_ARCHIVO = '" + TipoArchivoS
                + "' AND FECHA = '" + Fecha + "' AND FECHA_TRANSACCION = '" + FechaTransaccion 
                + "' GROUP BY CONSECUTIVO ORDER BY CONSECUTIVO";
            return consultar(query);
        }

    }
}
