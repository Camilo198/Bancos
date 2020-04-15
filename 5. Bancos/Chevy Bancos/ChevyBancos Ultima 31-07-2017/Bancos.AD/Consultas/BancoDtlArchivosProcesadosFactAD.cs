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
    public class BancoDtlArchivosProcesadosFactAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public BancoDtlArchivosProcesadosFactAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }
        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(BancoDtlArchivosProcesadosFact objEntidad)
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
                    adaptador.SelectCommand.Parameters["@pOid"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipodeRegistro", SqlDbType.VarChar));
                if (objEntidad.pTipodeRegistro != null)
                {
                    adaptador.SelectCommand.Parameters["@pTipodeRegistro"].Value = objEntidad.pTipodeRegistro;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipodeRegistro"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pReferenciaP", SqlDbType.VarChar));
                if (objEntidad.pReferenciaP != null)
                {
                    adaptador.SelectCommand.Parameters["@pReferenciaP"].Value = objEntidad.pReferenciaP;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pReferenciaP"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pReferenciaS", SqlDbType.VarChar));
                if (objEntidad.pReferenciaS != null)
                {
                    adaptador.SelectCommand.Parameters["@pReferenciaS"].Value = objEntidad.pReferenciaS;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pReferenciaS"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPeriodoF", SqlDbType.VarChar));
                if (objEntidad.pPeriodoF != null)
                {
                    adaptador.SelectCommand.Parameters["@pPeriodoF"].Value = objEntidad.pPeriodoF;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pPeriodoF"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCiclo", SqlDbType.VarChar));
                if (objEntidad.pCiclo != null)
                {
                    adaptador.SelectCommand.Parameters["@pCiclo"].Value = objEntidad.pCiclo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCiclo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorServicioP", SqlDbType.VarChar));
                if (objEntidad.pValorServicioP != null)
                {
                    adaptador.SelectCommand.Parameters["@pValorServicioP"].Value = objEntidad.pValorServicioP;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorServicioP"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoServicioF", SqlDbType.VarChar));
                if (objEntidad.pCodigoServicioF != null)
                {
                    adaptador.SelectCommand.Parameters["@pCodigoServicioF"].Value = objEntidad.pCodigoServicioF;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoServicioF"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorServicioA", SqlDbType.VarChar));
                if (objEntidad.pValorServicioA != null)
                {
                    adaptador.SelectCommand.Parameters["@pValorServicioA"].Value = objEntidad.pValorServicioA;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorServicioA"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaVenc", SqlDbType.VarChar));
                if (objEntidad.pFechaVenc != null)
                {
                    adaptador.SelectCommand.Parameters["@pFechaVenc"].Value = objEntidad.pFechaVenc;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaVenc"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoEFR", SqlDbType.VarChar));
                if (objEntidad.pCodigoEFR != null)
                {
                    adaptador.SelectCommand.Parameters["@pCodigoEFR"].Value = objEntidad.pCodigoEFR;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoEFR"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNoCtaClienteRece", SqlDbType.VarChar));
                if (objEntidad.pNoCtaClienteRece != null)
                {
                    adaptador.SelectCommand.Parameters["@pNoCtaClienteRece"].Value = objEntidad.pNoCtaClienteRece;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNoCtaClienteRece"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoCtaClienteRece", SqlDbType.VarChar));
                if (objEntidad.pTipoCtaClienteRece != null)
                {
                    adaptador.SelectCommand.Parameters["@pTipoCtaClienteRece"].Value = objEntidad.pTipoCtaClienteRece;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoCtaClienteRece"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdentificacionCliente", SqlDbType.VarChar));
                if (objEntidad.pIdentificacionCliente != null)
                {
                    adaptador.SelectCommand.Parameters["@pIdentificacionCliente"].Value = objEntidad.pIdentificacionCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdentificacionCliente"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreCliente", SqlDbType.VarChar));
                if (objEntidad.pNombreCliente != null)
                {
                    adaptador.SelectCommand.Parameters["@pNombreCliente"].Value = objEntidad.pNombreCliente;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreCliente"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigoEFO", SqlDbType.VarChar));
                if (objEntidad.pCodigoEFO != null)
                {
                    adaptador.SelectCommand.Parameters["@pCodigoEFO"].Value = objEntidad.pCodigoEFO;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigoEFO"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pReservado", SqlDbType.VarChar));
                if (objEntidad.pReservado != null)
                {
                    adaptador.SelectCommand.Parameters["@pReservado"].Value = objEntidad.pReservado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pReservado"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivo", SqlDbType.VarChar));
                if (objEntidad.pNombreArchivo != null)
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = objEntidad.pNombreArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFechaP", SqlDbType.VarChar));
                if (objEntidad.pFechaP != null)
                {
                    adaptador.SelectCommand.Parameters["@pFechaP"].Value = objEntidad.pFechaP;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFechaP"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pHoraP", SqlDbType.VarChar));
                if (objEntidad.pHoraP != null)
                {
                    adaptador.SelectCommand.Parameters["@pHoraP"].Value = objEntidad.pHoraP;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pHoraP"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pProcesado", SqlDbType.VarChar));
                if (objEntidad.pProcesado != null)
                {
                    adaptador.SelectCommand.Parameters["@pProcesado"].Value = objEntidad.pProcesado;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pProcesado"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreArchivoProceso", SqlDbType.VarChar));
                if (objEntidad.pNombreArchivoProceso != null)
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivoProceso"].Value = objEntidad.pNombreArchivoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreArchivoProceso"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodBanco", SqlDbType.VarChar));
                if (objEntidad.pCodBanco != null)
                {
                    adaptador.SelectCommand.Parameters["@pCodBanco"].Value = objEntidad.pCodBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodError", SqlDbType.VarChar));
                if (objEntidad.pCodError != null)
                {
                    adaptador.SelectCommand.Parameters["@pCodError"].Value = objEntidad.pCodError;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodError"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDescripcionError", SqlDbType.VarChar));
                if (objEntidad.pDescripcionError != null)
                {
                    adaptador.SelectCommand.Parameters["@pDescripcionError"].Value = objEntidad.pDescripcionError;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDescripcionError"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorregido", SqlDbType.VarChar));
                if (objEntidad.pCorregido != null)
                {
                    adaptador.SelectCommand.Parameters["@pCorregido"].Value = objEntidad.pCorregido;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorregido"].Value = DBNull.Value;
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
        public List<BancoDtlArchivosProcesadosFact> consultar(BancoDtlArchivosProcesadosFact objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);
            List<BancoDtlArchivosProcesadosFact> lista = new List<BancoDtlArchivosProcesadosFact>();
            BancoDtlArchivosProcesadosFact objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new BancoDtlArchivosProcesadosFact();
                objEntidad2.pOid = Convertidor.aEntero32(fila[BancoDtlArchivosProcesadosFactDEF.Oid]);
                objEntidad2.pTipodeRegistro = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.TipodeRegistro]);
                objEntidad2.pReferenciaP = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.ReferenciaP]);
                objEntidad2.pReferenciaS = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.ReferenciaS]);
                objEntidad2.pPeriodoF = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.PeriodoF]);
                objEntidad2.pCiclo = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.Ciclo]);
                objEntidad2.pValorServicioP = Convertidor.aDecimal(fila[BancoDtlArchivosProcesadosFactDEF.ValorServicioP]);
                objEntidad2.pCodigoServicioF = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.CodigoServicioF]);
                objEntidad2.pValorServicioA = Convertidor.aDecimal(fila[BancoDtlArchivosProcesadosFactDEF.ValorServicioA]);
                objEntidad2.pFechaVenc = Convertidor.aFechaYHora(fila[BancoDtlArchivosProcesadosFactDEF.FechaVenc]);
                objEntidad2.pCodigoEFR = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.CodigoEFR]);
                objEntidad2.pNoCtaClienteRece = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.NoCtaClienteRece]);
                objEntidad2.pTipoCtaClienteRece = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.TipoCtaClienteRece]);
                objEntidad2.pIdentificacionCliente = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.IdentificacionCliente]);
                objEntidad2.pNombreCliente = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.NombreCliente]);
                objEntidad2.pCodigoEFO = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.CodigoEFO]);
                objEntidad2.pReservado = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.Reservado]);
                objEntidad2.pNombreArchivo = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.NombreArchivo]);
                objEntidad2.pFechaP = Convertidor.aFechaYHora(fila[BancoDtlArchivosProcesadosFactDEF.FechaP]);
                objEntidad2.pHoraP = Convertidor.aFechaYHora(fila[BancoDtlArchivosProcesadosFactDEF.HoraP]);
                objEntidad2.pProcesado = Convertidor.aBooleano(fila[BancoDtlArchivosProcesadosFactDEF.Procesado]);
                objEntidad2.pNombreArchivoProceso = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.NombreArchivoProceso]);
                objEntidad2.pCodBanco = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.CodBanco]);
                objEntidad2.pCodError = Convertidor.aEntero32(fila[BancoDtlArchivosProcesadosFactDEF.CodError]);
                objEntidad2.pDescripcionError = Convertidor.aCadena(fila[BancoDtlArchivosProcesadosFactDEF.DescripcionError]);
                objEntidad2.pCorregido = Convertidor.aBooleano(fila[BancoDtlArchivosProcesadosFactDEF.Corregido]);

                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(BancoDtlArchivosProcesadosFact objEntidad)
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
                + " WHERE FechaP BETWEEN '" + fini.ToString("yyyy-MM-dd") + "' AND '" + ffin.ToString("yyyy-MM-dd") + "'"
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