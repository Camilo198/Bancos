using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Procesos.AD.Conexion;
using Procesos.EN;
using Procesos.EN.Definicion;
using Procesos.EN.Tablas;
using log4net;

namespace Procesos.AD.Consultas
{
    public class RutaAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public RutaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Ruta objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_BAN_Ruta", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRuta", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pRuta))
                {
                    adaptador.SelectCommand.Parameters["@pRuta"].Value = objEntidad.pRuta;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRuta"].Value = DBNull.Value;
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
        public List<Ruta> consultar(Ruta objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Ruta> lista = new List<Ruta>();
            Ruta objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Ruta();
                objEntidad2.pOid = Convertidor.aEntero32(fila[RutaDEF.Oid]);
                objEntidad2.pRuta = Convertidor.aCadena(fila[RutaDEF.Ruta]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Ruta objEntidad)
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

        public String consultarRutaEntrada(String codigoBanco)
        {
            String query = "SELECT R.RUTA"
                + " FROM tb_BAN_RUTA R, tb_BAN_BANCO B"
                + " WHERE R.OID = B.Ruta_Archivos_Entrada"
                + " AND B.CODIGO_BANCO = '" + codigoBanco + "'",
                ruta = String.Empty;

            try
            {
                ruta = Convertidor.aCadena(consultar(query).Rows[0][RutaDEF.Ruta]);
            }
            catch (Exception ex)
            {
                Registrador.Fatal(ex.Message);
            }
            
            return ruta;
        }

        public String consultarRutaSalida(String codigoBanco)
        {
            String query = "SELECT R.RUTA"
                + " FROM tb_BAN_RUTA R, tb_BAN_BANCO B"
                + " WHERE R.OID = B.Ruta_Archivos_Salida"
                + " AND B.CODIGO_BANCO = '" + codigoBanco + "'",
                ruta = String.Empty;
            try
            {
                ruta = Convertidor.aCadena(consultar(query).Rows[0][RutaDEF.Ruta]);
            }
            catch (Exception ex)
            {
                Registrador.Fatal(ex.Message);
            }

            return ruta;
        }
    }
}
