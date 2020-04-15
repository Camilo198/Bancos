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
    public class HistorialArchivosSalidaAD
    {

        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public HistorialArchivosSalidaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(HistorialArchivosSalida objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Historial_Archivos_Salida", conexion);
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
                    adaptador.SelectCommand.Parameters["@pFecha"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdCuentaBanco", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pIdCuentaBanco))
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = objEntidad.pIdCuentaBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = objEntidad.pTipoArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoArchivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pConsecutivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pConsecutivo))
                {
                    adaptador.SelectCommand.Parameters["@pConsecutivo"].Value = objEntidad.pConsecutivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pConsecutivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pLineasArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pLineaDetalle))
                {
                    adaptador.SelectCommand.Parameters["@pLineasArchivo"].Value = objEntidad.pLineaDetalle;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pLineasArchivo"].Value = String.Empty;
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
        public List<HistorialArchivosSalida> consultar(HistorialArchivosSalida objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<HistorialArchivosSalida> lista = new List<HistorialArchivosSalida>();
            HistorialArchivosSalida objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new HistorialArchivosSalida();
                objEntidad2.pFecha  = Convertidor.aCadena(fila[HistorialArchivosSalidaDEF.Fecha ]);
                objEntidad2.pIdCuentaBanco = Convertidor.aCadena(fila[HistorialArchivosSalidaDEF.IdCuentaBanco]);
                objEntidad2.pTipoArchivo = Convertidor.aCadena(fila[HistorialArchivosSalidaDEF.TipoArchivo]);
                objEntidad2.pConsecutivo = Convertidor.aCadena(fila[HistorialArchivosSalidaDEF.Consecutivo]);
                objEntidad2.pLineaDetalle = Convertidor.aCadena(fila[HistorialArchivosSalidaDEF.LineaDetalle]);
                
                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(HistorialArchivosSalida objEntidad)
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
        //BUSQUEDA DE TODAS LAS FECHAS EN EL CUAL EXISTE UN ARCHIVO ASOBANCARIA CON COINCIDENCIA A UN BANCO
        public DataTable consultarFechasXBanco(String IdCuenta, String TipoArchivoS)
        {
            String query = "SELECT FECHA FROM tb_BAN_HISTORIAL_ARCHIVOS_SALIDA"
                + " WHERE ID_CUENTA_BANCO = '" + IdCuenta + "' AND TIPO_ARCHIVO = '" + TipoArchivoS + "' GROUP BY FECHA ORDER BY FECHA";
            return consultar(query);
        }
        //BUSQUEDA DE TODOS LOS CONSECUTIVOS DEL ARCHIVO ASOBANCARIA CON COINCIDENCIA A UN BANCO Y UNA FECHA
        public DataTable consultarConsecutivoXBanco(String IdCuenta, String TipoArchivoS, String Fecha)
        {
            String query = "SELECT CONSECUTIVO FROM tb_BAN_HISTORIAL_ARCHIVOS_SALIDA"
                + " WHERE ID_CUENTA_BANCO = '" + IdCuenta + "' AND TIPO_ARCHIVO = '" + TipoArchivoS 
                + "' AND FECHA = '" + Fecha + "' GROUP BY CONSECUTIVO ORDER BY CONSECUTIVO";
            return consultar(query);
        }
        // RETORNA LAS LINEAS DE UN ARCHIVO ASOBANCARIA CON COINCIDENCIA A UN BANCO, UNA FECHA Y CONSECUTIVO
        public DataTable consultarLineasConsecutivo(String IdCuenta, String TipoArchivoS, String Fecha, String Consecutivo)
        {
            String query = "SELECT LINEAS_ARCHIVO FROM tb_BAN_HISTORIAL_ARCHIVOS_SALIDA"
                + " WHERE ID_CUENTA_BANCO = '" + IdCuenta + "' AND TIPO_ARCHIVO = '" + TipoArchivoS 
                + "' AND FECHA = '" + Fecha + "' AND CONSECUTIVO = '" + Consecutivo + "'";
            return consultar(query);
        }

    }
}
