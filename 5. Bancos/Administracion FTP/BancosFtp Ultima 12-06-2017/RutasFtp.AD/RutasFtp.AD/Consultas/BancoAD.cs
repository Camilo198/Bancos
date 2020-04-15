using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using RutasFtp.AD.Conexion;
using RutasFtp.EN;
using RutasFtp.EN.Definicion;
using RutasFtp.EN.Tablas;
using log4net;

namespace RutasFtp.AD.Consultas
{
    public class BancoAD
    {

        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public BancoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Banco objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Banco", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdCuentaBanco", SqlDbType.VarChar));
                if (objEntidad.pIdCuentaBanco != null)
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = objEntidad.pIdCuentaBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoBanco", SqlDbType.VarChar));
                if (objEntidad.pCodigoBanco != null)
                {
                    adaptador.SelectCommand.Parameters["@pCodigoBanco"].Value = objEntidad.pCodigoBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFtp", SqlDbType.VarChar));
                if (objEntidad.pFtp != null)
                {
                    if (objEntidad.pFtp.Value)
                        adaptador.SelectCommand.Parameters["@pFtp"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pFtp"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFtp"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pClave", SqlDbType.VarChar));
                if (objEntidad.pClave != null)
                {
                    adaptador.SelectCommand.Parameters["@pClave"].Value = objEntidad.pClave;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pClave"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreCuenta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreCuenta))
                {
                    adaptador.SelectCommand.Parameters["@pNombreCuenta"].Value = objEntidad.pNombreCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreCuenta"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaArchivosEntrada", SqlDbType.VarChar));
                if (objEntidad.pRutaArchivosEntrada > 0)
                {
                    adaptador.SelectCommand.Parameters["@pRutaArchivosEntrada"].Value = objEntidad.pRutaArchivosEntrada;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRutaArchivosEntrada"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaArchivosSalida", SqlDbType.VarChar));
                if (objEntidad.pRutaArchivosSalida > 0)
                {
                    adaptador.SelectCommand.Parameters["@pRutaArchivosSalida"].Value = objEntidad.pRutaArchivosSalida;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRutaArchivosSalida"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRutaArchivosSalidaEpicor", SqlDbType.VarChar));
                if (objEntidad.pRutaArchivosSalidaEpicor > 0)
                {
                    adaptador.SelectCommand.Parameters["@pRutaArchivosSalidaEpicor"].Value = objEntidad.pRutaArchivosSalidaEpicor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRutaArchivosSalidaEpicor"].Value = DBNull.Value;
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


                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoControl", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoControl))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = objEntidad.pCorreoControl;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = string.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoEnvio", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoEnvio))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoEnvio"].Value = objEntidad.pCorreoEnvio;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoEnvio"].Value = string.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pActivo", SqlDbType.VarChar));
                if (objEntidad.pActivo != null)
                {
                    if (objEntidad.pActivo.Value)
                        adaptador.SelectCommand.Parameters["@pActivo"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pActivo"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pActivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRemitente", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRemitente))
                {
                    adaptador.SelectCommand.Parameters["@pRemitente"].Value = objEntidad.pRemitente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRemitente"].Value = string.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNumCuenta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNumCuenta))
                {
                    adaptador.SelectCommand.Parameters["@pNumCuenta"].Value = objEntidad.pNumCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNumCuenta"].Value = string.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoCuenta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoCuenta))
                {
                    adaptador.SelectCommand.Parameters["@pTipoCuenta"].Value = objEntidad.pTipoCuenta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoCuenta"].Value = string.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoProceso))
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = objEntidad.pTipoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = string.Empty;
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

        public List<Banco> consultar(Banco objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Banco> lista = new List<Banco>();
            Banco objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Banco();
                objEntidad2.pClave = Convertidor.aCadena(fila[BancoDEF.Clave]);
                objEntidad2.pFtp = Convertidor.aBooleano(fila[BancoDEF.Ftp]);
                objEntidad2.pRutaArchivosEntrada = Convertidor.aEntero32(fila[BancoDEF.RutaArchivosEntrada]);
                objEntidad2.pRutaArchivosSalida = Convertidor.aEntero32(fila[BancoDEF.RutaArchivosSalida]);
                objEntidad2.pRutaArchivosSalidaEpicor = Convertidor.aEntero32(fila[BancoDEF.RutaArchivosSalidaEpicor]);
                objEntidad2.pUrlFtp = Convertidor.aCadena(fila[BancoDEF.UrlFtp]);
                objEntidad2.pUsuarioFtp = Convertidor.aCadena(fila[BancoDEF.UsuarioFtp]);
                objEntidad2.pCorreoControl = Convertidor.aCadena(fila[BancoDEF.CorreoControl]);
                objEntidad2.pCorreoEnvio = Convertidor.aCadena(fila[BancoDEF.CorreoEnvio]);
                objEntidad2.pActivo = Convertidor.aBooleano(fila[BancoDEF.Activo]);
                objEntidad2.pRemitente = Convertidor.aCadena(fila[BancoDEF.Remitente]);
                objEntidad2.pId = Convertidor.aEntero32(fila[BancoDEF.Id]);

                objEntidad2.pCodigoBanco = Convertidor.aCadena(fila[BancoDEF.CodigoBanco]);
                objEntidad2.pNumCuenta = Convertidor.aCadena(fila[BancoDEF.NumCuenta]);
                objEntidad2.pTipoCuenta = Convertidor.aCadena(fila[BancoDEF.TipoCuenta]);
                objEntidad2.pIdCuentaBanco = Convertidor.aCadena(fila[BancoDEF.IdCuentaBanco]);
                objEntidad2.pNombreCuenta = Convertidor.aCadena(fila[BancoDEF.NombreCuenta]);

                objEntidad2.pTipoProceso = Convertidor.aCadena(fila[BancoDEF.TipoProceso]);
                lista.Add(objEntidad2);
            }

            return lista;
        }
                
        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }

        
    }
}
