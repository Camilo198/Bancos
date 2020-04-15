using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Bancos.AD;
using Bancos.AD.Conexion;
using Bancos.EN;
using Bancos.EN.Definicion;
using Bancos.EN.Tablas;
using log4net;

namespace Bancos.AD.Consultas
{
    public class ConfiguracionAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public ConfiguracionAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Configuracion objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Configuracion", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pArchivoAsobancaria", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pArchivoAsobancaria))
                {
                    adaptador.SelectCommand.Parameters["@pArchivoAsobancaria"].Value = objEntidad.pArchivoAsobancaria;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pArchivoAsobancaria"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pArchivoPlano", SqlDbType.VarChar));
                if (objEntidad.pArchivoPlano != null)
                {
                    adaptador.SelectCommand.Parameters["@pArchivoPlano"].Value = objEntidad.pArchivoPlano;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pArchivoPlano"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoLinea", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoLinea))
                {
                    adaptador.SelectCommand.Parameters["@pTipoLinea"].Value = objEntidad.pTipoLinea;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoLinea"].Value = DBNull.Value;
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
        public List<Configuracion> consultar(Configuracion objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Configuracion> lista = new List<Configuracion>();
            Configuracion objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Configuracion();
                objEntidad2.pArchivoAsobancaria = Convertidor.aCadena(fila[ConfiguracionDEF.ArchivoAsobancaria]);
                objEntidad2.pArchivoPlano = Convertidor.aEntero32(fila[ConfiguracionDEF.ArchivoPlano]);
                objEntidad2.pOid = Convertidor.aEntero32(fila[ConfiguracionDEF.Oid]);
                objEntidad2.pTipoLinea = Convertidor.aCadena(fila[ConfiguracionDEF.TipoLinea]);
                
                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Configuracion objEntidad)
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
    }
}
