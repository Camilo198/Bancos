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
    public class BancoDtlArchivosProcesadosAD
    {

        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public BancoDtlArchivosProcesadosAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }
        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(BancoDtlArchivosProcesados objEntidad)
        {

            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Detalles_Archivos_Procesados", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOid", SqlDbType.VarChar));
                if (objEntidad.pOid > 0)
                {
                    adaptador.SelectCommand.Parameters["@pOid"].Value = objEntidad.pOid;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pOid"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoRegistro", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoRegistro))
                {
                    adaptador.SelectCommand.Parameters["@pTipoRegistro"].Value = objEntidad.pTipoRegistro;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoRegistro"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pReferenciaPrincipal", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pReferenciaPrincipal))
                {
                    adaptador.SelectCommand.Parameters["@pReferenciaPrincipal"].Value = objEntidad.pReferenciaPrincipal;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pReferenciaPrincipal"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorRecaudado", SqlDbType.VarChar));
                if (objEntidad.pValorRecaudado > 0)
                {
                    adaptador.SelectCommand.Parameters["@pValorRecaudado"].Value = objEntidad.pValorRecaudado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorRecaudado"].Value = 0;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pProcedenciaPago", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pProcedenciaPago))
                {
                    adaptador.SelectCommand.Parameters["@pProcedenciaPago"].Value = objEntidad.pProcedenciaPago;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pProcedenciaPago"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pMediosPago", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pMediosPago))
                {
                    adaptador.SelectCommand.Parameters["@pMediosPago"].Value = objEntidad.pMediosPago;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pMediosPago"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNoOperacion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNoOperacion))
                {
                    adaptador.SelectCommand.Parameters["@pNoOperacion"].Value = objEntidad.pNoOperacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNoOperacion"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNoAutorizacion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNoAutorizacion))
                {
                    adaptador.SelectCommand.Parameters["@pNoAutorizacion"].Value = objEntidad.pNoAutorizacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNoAutorizacion"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoEFD", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigoEFD))
                {
                    adaptador.SelectCommand.Parameters["@pCodigoEFD"].Value = objEntidad.pCodigoEFD;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoEFD"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoSucursal", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigoSucursal))
                {
                    adaptador.SelectCommand.Parameters["@pCodigoSucursal"].Value = objEntidad.pCodigoSucursal;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoSucursal"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pSecuencia", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pSecuencia))
                {
                    adaptador.SelectCommand.Parameters["@pSecuencia"].Value = objEntidad.pSecuencia;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pSecuencia"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCausalDevo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCausalDevo))
                {
                    adaptador.SelectCommand.Parameters["@pCausalDevo"].Value = objEntidad.pCausalDevo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCausalDevo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pReservado", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pReservado))
                {
                    adaptador.SelectCommand.Parameters["@pReservado"].Value = objEntidad.pReservado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pReservado"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreArchivo))
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = objEntidad.pNombreArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaP", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaP))
                {
                    adaptador.SelectCommand.Parameters["@pFechaP"].Value = objEntidad.pFechaP;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaP"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pProcesado", SqlDbType.VarChar));
                if (objEntidad.pProcesado != null)
                {
                    if (objEntidad.pProcesado.Value)
                        adaptador.SelectCommand.Parameters["@pProcesado"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pProcesado"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pProcesado"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivoProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreArchivoProceso))
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivoProceso"].Value = objEntidad.pNombreArchivoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivoProceso"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodBanco", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodBanco))
                {
                    adaptador.SelectCommand.Parameters["@pCodBanco"].Value = objEntidad.pCodBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodBanco"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodError", SqlDbType.VarChar));
                if (objEntidad.pCodError > 0)
                {
                    adaptador.SelectCommand.Parameters["@pCodError"].Value = objEntidad.pCodError.Value;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodError"].Value = 0;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDescripcionError", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDescripcionError))
                {
                    adaptador.SelectCommand.Parameters["@pDescripcionError"].Value = objEntidad.pDescripcionError;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDescripcionError"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorregido", SqlDbType.VarChar));
                if (objEntidad.pCorregido != null)
                {
                    if (objEntidad.pCorregido.Value)
                        adaptador.SelectCommand.Parameters["@pCorregido"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pCorregido"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorregido"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDatafono", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDatafono))
                {
                    adaptador.SelectCommand.Parameters["@pDatafono"].Value = objEntidad.pDatafono;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDatafono"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaRecaudo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFechaRecaudo))
                {
                    adaptador.SelectCommand.Parameters["@pFechaRecaudo"].Value = objEntidad.pFechaRecaudo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaRecaudo"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoProceso))
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = objEntidad.pTipoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = String.Empty;
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
        public List<BancoDtlArchivosProcesados> consultar(BancoDtlArchivosProcesados objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);
            List<BancoDtlArchivosProcesados> lista = new List<BancoDtlArchivosProcesados>();
            BancoDtlArchivosProcesados objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new BancoDtlArchivosProcesados();
                objEntidad2.pOid = Convertidor.aEntero32(fila[BancoDtlArchivosProcesadosDEF.Oid]);
                objEntidad2.pTipoRegistro = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.TipoRegistro]);
                objEntidad2.pReferenciaPrincipal = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.ReferenciaPrincipal]);
                objEntidad2.pValorRecaudado = Convertidor.aDecimal(fila[BancoDtlArchivosProcesadosDEF.ValorRecaudado]);
                objEntidad2.pProcedenciaPago = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.ProcedenciaPago]);
                objEntidad2.pMediosPago = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.MediosPago]);
                objEntidad2.pNoOperacion = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.NoOperacion]);
                objEntidad2.pNoAutorizacion = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.NoAutorizacion]);
                objEntidad2.pCodigoEFD = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.CodigoEFD]);
                objEntidad2.pCodigoSucursal = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.CodigoSucursal]);
                objEntidad2.pSecuencia = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.Secuencia]);
                objEntidad2.pCausalDevo = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.CausalDevo]);
                objEntidad2.pReservado = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.Reservado]);
                objEntidad2.pNombreArchivo = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.NombreArchivo]);
                objEntidad2.pFechaP = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.FechaP]);
                objEntidad2.pProcesado = Convertidor.aBooleano(fila[BancoDtlArchivosProcesadosDEF.Procesado]);
                objEntidad2.pNombreArchivoProceso = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.NombreArchivoProceso]);
                objEntidad2.pCodBanco = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.CodBanco]);
                objEntidad2.pCodError = Convertidor.aEntero32(fila[BancoDtlArchivosProcesadosDEF.CodError]);
                objEntidad2.pDescripcionError = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.DescripcionError]);
                objEntidad2.pCorregido = Convertidor.aBooleano(fila[BancoDtlArchivosProcesadosDEF.Corregido]);

                objEntidad2.pDatafono = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.Datafono]);
                objEntidad2.pFechaRecaudo = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.FechaRecaudo]);
                objEntidad2.pTipoProceso = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosDEF.TipoProceso]);

                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(BancoDtlArchivosProcesados objEntidad)
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

        public DataTable consultarLineasErradas()
        {
            String query = "SELECT *"
                + " FROM tb_BAN_DETALLES_ARCHIVOS_PROCESADOS"
                + " WHERE CodError <> 0"
                + " AND Corregido = 0"
                + " ORDER BY ID";

            return consultar(query);
        }

        public DataTable consultarLineasErradas(DateTime fini, DateTime ffin)
        {
            String query = "SELECT *"
                + " FROM tb_BAN_DETALLES_ARCHIVOS_PROCESADOS"
                + " WHERE FechaP BETWEEN '" + fini.ToString("yyyy-MM-dd") + " 00:00:00.000" + "' AND '" + ffin.ToString("yyyy-MM-dd") + " 23:59:00.000" + "'"
                + " AND CodError <> 0 AND Corregido = 0"
                + " ORDER BY ID";

            return consultar(query);
        }

        public DataTable consultarLineasErradas(String banco, DateTime fini, DateTime ffin)
        {
            String query = "SELECT *"
                + " FROM tb_BAN_DETALLES_ARCHIVOS_PROCESADOS"
                + " WHERE CodBanco = " + banco + " AND  FechaP BETWEEN '" + fini.ToString("yyyy-MM-dd") + " 00:00:00.000" + "' AND '" + ffin.ToString("yyyy-MM-dd") + " 23:59:00.000" + "'"
                + " AND CodError <> 0 AND Corregido = 0"
                + " ORDER BY ID";

            return consultar(query);
        }

        public DataTable Cargar(DateTime FInicia, DateTime FFinal)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataTable datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                using (conexion = new SqlConnection())
                {
                    conexion.Open();
                    adaptador = new SqlDataAdapter("pa_Ban_Detalles_Archivos_Procesados1", conexion);
                    adaptador.SelectCommand = new SqlCommand();
                    adaptador.SelectCommand.Connection = conexion;
                    adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adaptador.SelectCommand.CommandText = "pa_Ban_Detalles_Archivos_Procesados1";

                    //Agregar parametros
                    adaptador.SelectCommand.Parameters.Add(new SqlParameter("@FInicio", SqlDbType.Date));
                    adaptador.SelectCommand.Parameters["@FInicio"].Value = FInicia;
                    adaptador.SelectCommand.Parameters.Add(new SqlParameter("@FFin", SqlDbType.Date));
                    adaptador.SelectCommand.Parameters["@FFin"].Value = FFinal;

                    //Llenar ds
                    adaptador.Fill(datos);
                    adaptador.Dispose();
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("Falla la consulta Sql " + ex.Message);
            }
            finally
            {
                conexion.Close();
                conexion.Dispose();
            }
            return datos;
        }


    }
}