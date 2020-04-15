using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using RutasFtp.AD;
using RutasFtp.AD.Conexion;
using RutasFtp.EN;
using RutasFtp.EN.Definicion;
using RutasFtp.EN.Tablas;
using log4net;

namespace RutasFtp.AD.Consultas
{
    public class FtpAD
    {
         public String Error { get; set; }

        public ILog Registrador { get; set; }

        public FtpAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Ftp objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Ftp", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                if (objEntidad.pId > 0)
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = objEntidad.pId;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUrlFtp", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUrlFtp))
                {
                    adaptador.SelectCommand.Parameters["@pUrlFtp"].Value = objEntidad.pUrlFtp;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUrlFtp"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pUsuarioFtp", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pUsuarioFtp))
                {
                    adaptador.SelectCommand.Parameters["@pUsuarioFtp"].Value = objEntidad.pUsuarioFtp;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pUsuarioFtp"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pClaveFtp", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pClaveFtp))
                {
                    adaptador.SelectCommand.Parameters["@pClaveFtp"].Value = objEntidad.pClaveFtp;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pClaveFtp"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaDestino", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRutaDestino))
                {
                    adaptador.SelectCommand.Parameters["@pRutaDestino"].Value = objEntidad.pRutaDestino;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRutaDestino"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoProceso))
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = objEntidad.pTipoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPrefijo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pPrefijo))
                {
                    adaptador.SelectCommand.Parameters["@pPrefijo"].Value = objEntidad.pPrefijo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pPrefijo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFormato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFormato))
                {
                    adaptador.SelectCommand.Parameters["@pFormato"].Value = objEntidad.pFormato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFormato"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaUltimoIngreso", SqlDbType.DateTime));
                if (!String.IsNullOrEmpty(objEntidad.pFechaUltimoIngreso ))
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimoIngreso"].Value = objEntidad.pFechaUltimoIngreso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimoIngreso"].Value = DBNull.Value;
                }
                //Se agrega este campo para controlar la ultima copia del archivo que llega por FTP
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaUltimaCopia", SqlDbType.DateTime));
                if (!String.IsNullOrEmpty(objEntidad.pFechaUltimaCopia))
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimaCopia"].Value = objEntidad.pFechaUltimaCopia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimaCopia"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFtpSeguro", SqlDbType.VarChar));
                if (objEntidad.pFtpSeguro != null)
                {
                    if (objEntidad.pFtpSeguro.Value)
                        adaptador.SelectCommand.Parameters["@pFtpSeguro"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pFtpSeguro"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFtpSeguro"].Value = DBNull.Value;
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

        protected DataSet ejecutarConsultaUlimaCopia(Ftp objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Ftp_Update", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pId", SqlDbType.VarChar));
                if (objEntidad.pId > 0)
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = objEntidad.pId;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pId"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaUltimoIngreso", SqlDbType.DateTime));
                if (!String.IsNullOrEmpty(objEntidad.pFechaUltimoIngreso))
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimoIngreso"].Value = objEntidad.pFechaUltimoIngreso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimoIngreso"].Value = DBNull.Value;
                }
                //Se agrega este campo para controlar la ultima copia del archivo que llega por FTP
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaUltimaCopia", SqlDbType.DateTime));
                if (!String.IsNullOrEmpty(objEntidad.pFechaUltimaCopia))
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimaCopia"].Value = objEntidad.pFechaUltimaCopia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaUltimaCopia"].Value = DBNull.Value;
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
        public List<Ftp> consultar(Ftp objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Ftp> lista = new List<Ftp>();
            Ftp objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Ftp();
                objEntidad2.pId = Convertidor.aEntero32(fila[FtpDEF.Id]);
                objEntidad2.pUrlFtp = Convertidor.aCadena(fila[FtpDEF.UrlFtp]);
                objEntidad2.pUsuarioFtp = Convertidor.aCadena(fila[FtpDEF.UsuarioFtp]);
                objEntidad2.pClaveFtp = Convertidor.aCadena(fila[FtpDEF.ClaveFtp]);
                objEntidad2.pRutaDestino = Convertidor.aCadena(fila[FtpDEF.RutaDestino]);
                objEntidad2.pIdCuentaBanco  = Convertidor.aCadena(fila[FtpDEF.IdCuentaBanco ]);
                objEntidad2.pTipoProceso  = Convertidor.aCadena(fila[FtpDEF.TipoProceso]);
                objEntidad2.pFtpSeguro  = Convertidor.aBooleano(fila[FtpDEF.FtpSeguro]);

                objEntidad2.pPrefijo  = Convertidor.aCadena(fila[FtpDEF.Prefijo ]);
                objEntidad2.pFormato  = Convertidor.aCadena(fila[FtpDEF.Formato ]);
                objEntidad2.pFechaUltimoIngreso  = Convertidor.aCadena(fila[FtpDEF.FechaUltimoIngreso ]);
                objEntidad2.pFechaUltimaCopia = Convertidor.aCadena(fila[FtpDEF.FechaUltimaCopia]);
                
                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Ftp objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Error(ex.Message);
            }
            return cuenta;
        }

        public int ejecutarNoConsultaUltimaCopia(Ftp objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsultaUlimaCopia(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0]["Cuenta"]);
            }
            catch (Exception ex)
            {
                Registrador.Error(ex.Message);
            }
            return cuenta;
        }
    }
}
