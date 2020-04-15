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
    public class TablaAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TablaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Tabla objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Tabla", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pBanco", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pBanco))
                {
                    adaptador.SelectCommand.Parameters["@pBanco"].Value = objEntidad.pBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pBanco"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEsAsobancaria", SqlDbType.VarChar));
                if (objEntidad.pEsAsobancaria.HasValue)
                {
                    if (objEntidad.pEsAsobancaria.Value)
                        adaptador.SelectCommand.Parameters["@pEsAsobancaria"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pEsAsobancaria"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEsAsobancaria"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombre))
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = objEntidad.pNombre;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombre"].Value = DBNull.Value;
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
        public List<Tabla> consultar(Tabla objEntidad)
        {
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Tabla> lista = new List<Tabla>();
            Tabla objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Tabla();
                objEntidad2.pBanco = Convertidor.aCadena(fila[TablaDEF.Banco]);
                objEntidad2.pEsAsobancaria = Convertidor.aBooleano(fila[TablaDEF.EsAsobancaria]);
                objEntidad2.pNombre = Convertidor.aCadena(fila[TablaDEF.Nombre]);
                objEntidad2.pOid = Convertidor.aEntero32(fila[TablaDEF.Oid]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Tabla objEntidad)
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
