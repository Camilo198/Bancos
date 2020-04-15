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
    public class ArchivoPlanoAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public ArchivoPlanoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(ArchivoPlano objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Archivo_Plano", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdCuentaBanco", SqlDbType.VarChar));
                if (objEntidad.pIdCuentaBanco != null)
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = objEntidad.pIdCuentaBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdCuentaBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoProceso", SqlDbType.VarChar));
                if (objEntidad.pTipoProceso != null)
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = objEntidad.pTipoProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoProceso"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pLineasExcluidasInicio", SqlDbType.VarChar));
                if (objEntidad.pLineasExcluidasInicio > 0)
                {
                    adaptador.SelectCommand.Parameters["@pLineasExcluidasInicio"].Value = objEntidad.pLineasExcluidasInicio;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pLineasExcluidasInicio"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pLineasExcluidasFin", SqlDbType.VarChar));
                if (objEntidad.pLineasExcluidasFin > 0)
                {
                    adaptador.SelectCommand.Parameters["@pLineasExcluidasFin"].Value = objEntidad.pLineasExcluidasFin;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pLineasExcluidasFin"].Value = String.Empty;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEsExcel", SqlDbType.VarChar));
                if (objEntidad.pEsExcel != null)
                {
                    if (objEntidad.pEsExcel.Value)
                        adaptador.SelectCommand.Parameters["@pEsExcel"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pEsExcel"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEsExcel"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNumeroHojaExcel", SqlDbType.VarChar));
                if (objEntidad.pNumeroHojaExcel > 0)
                {
                    adaptador.SelectCommand.Parameters["@pNumeroHojaExcel"].Value = objEntidad.pNumeroHojaExcel;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNumeroHojaExcel"].Value = String.Empty;
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
        public List<ArchivoPlano> consultar(ArchivoPlano objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);
            List<ArchivoPlano> lista = new List<ArchivoPlano>();
            ArchivoPlano objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new ArchivoPlano();
                objEntidad2.pIdCuentaBanco = Convertidor.aCadena(fila[ArchivoPlanoDEF.IdCuentaBanco]);
                objEntidad2.pTipoProceso  = Convertidor.aCadena(fila[ArchivoPlanoDEF.TipoProceso]);
                objEntidad2.pOid = Convertidor.aEntero32(fila[ArchivoPlanoDEF.Oid]);
                objEntidad2.pLineasExcluidasInicio  = Convertidor.aEntero32(fila[ArchivoPlanoDEF.LineasExcluidasInicio ]);
                objEntidad2.pLineasExcluidasFin  = Convertidor.aEntero32(fila[ArchivoPlanoDEF.LineasExcluidasFin ]);
                objEntidad2.pEsExcel = Convertidor.aBooleano(fila[ArchivoPlanoDEF.EsExcel]);
                objEntidad2.pNumeroHojaExcel  = Convertidor.aEntero32(fila[ArchivoPlanoDEF.NumeroHojaExcel]);
                objEntidad2.pNomHoja = Convertidor.aCadena(fila[ArchivoPlanoDEF.NomHoja]);
                objEntidad2.pCaracterDecimal= Convertidor.aCadena(fila[ArchivoPlanoDEF.CaracterDecimal]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(ArchivoPlano objEntidad)
        {
            int cuenta = -1;
            DataSet datos = ejecutarConsulta(objEntidad);
            try
            {
                cuenta = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0]["Cuenta"]);
            }
            catch(Exception ex)
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

        public DataTable consultarLineasCuentaBanco(String codigoCuenta, String TipoDProceso)
        {
            String query = "SELECT c.OID, tl.NOMBRE, c.Archivo_Plano, c.Tipo_Linea"
                + " FROM tb_BAN_ARCHIVO_PLANO ap, tb_BAN_CONFIGURACION c, tb_BAN_TIPO_LINEA tl"
                + " WHERE ap.OID = c.Archivo_Plano"
                + " AND c.Tipo_Linea = tl.OID"
                + " AND ap.Id_Cuenta_Banco = '" + codigoCuenta + "' AND ap.Tipo_Proceso = '" + TipoDProceso + "' "
                + " ORDER BY Tipo_Linea";

            return consultar(query);
        }

        public DataTable consultarLineasDisponibles(String codigoCuenta, String TipoDProceso)
        {
            String query = "SELECT OID, NOMBRE"
                + " FROM tb_BAN_TIPO_LINEA"
                + " WHERE OID NOT IN (SELECT c.Tipo_Linea"
                + " FROM tb_BAN_ARCHIVO_PLANO ap, tb_BAN_CONFIGURACION c, tb_BAN_TIPO_LINEA tl"
                + " WHERE ap.OID = c.Archivo_Plano"
                + " AND c.Tipo_Linea = tl.OID"
                + " AND ap.Id_Cuenta_Banco = '" + codigoCuenta + "' AND ap.Tipo_Proceso = '" + TipoDProceso + "')"
                + " ORDER BY OID";

            return consultar(query);
        }

        public DataTable consultarLineasAsobancaria(String TipoArchivo)
        {

            String query = "SELECT tl.OID as Tipo_Linea, tl.NOMBRE "
                + " FROM tb_BAN_TIPO_LINEA tl"
                + " ORDER BY Tipo_Linea";
            //String query = "SELECT c.OID, tl.NOMBRE, c.Archivo_Plano, c.Tipo_Linea"
            //    + " FROM tb_BAN_ARCHIVO_ASOBANCARIA aa, tb_BAN_CONFIGURACION c, tb_BAN_TIPO_LINEA tl"
            //    + " WHERE aa.OID = c.Archivo_Asobancaria"
            //    + " AND c.Tipo_Linea = tl.OID"
            //    + " AND aa.OID = '" + TipoArchivo + "'"
            //    + " ORDER BY Tipo_Linea";

            return consultar(query);
        }

        public DataTable consultarLineasDisponiblesAso(String TipoArchivo)
        {
            String query = "SELECT OID, NOMBRE"
                + " FROM tb_BAN_TIPO_LINEA"
                + " WHERE OID NOT IN (SELECT c.Tipo_Linea"
                + " FROM tb_BAN_ARCHIVO_ASOBANCARIA aa, tb_BAN_CONFIGURACION c, tb_BAN_TIPO_LINEA tl"
                + " WHERE aa.OID = c.Archivo_Asobancaria"
                + " AND c.Tipo_Linea = tl.OID"
                + " AND aa.OID = '"+TipoArchivo +"')"
                + " ORDER BY OID";

            return consultar(query);
        }
    }
}
