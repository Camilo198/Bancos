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
    public class ValorAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public ValorAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Valor objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Valor", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCodigo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCodigo))
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = objEntidad.pCodigo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCodigo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pDescripcion", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pDescripcion))
                {
                    adaptador.SelectCommand.Parameters["@pDescripcion"].Value = objEntidad.pDescripcion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pDescripcion"].Value = DBNull.Value;
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTabla", SqlDbType.VarChar));
                if (objEntidad.pTabla > 0)
                {
                    adaptador.SelectCommand.Parameters["@pTabla"].Value = objEntidad.pTabla;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTabla"].Value = DBNull.Value;
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
        public List<Valor> consultar(Valor objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Valor> lista = new List<Valor>();
            Valor objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Valor();
                objEntidad2.pCodigo = Convertidor.aCadena(fila[ValorDEF.Codigo]);
                objEntidad2.pDescripcion = Convertidor.aCadena(fila[ValorDEF.Descripcion]);
                objEntidad2.pOid = Convertidor.aEntero32(fila[ValorDEF.Oid]);
                objEntidad2.pTabla = Convertidor.aEntero32(fila[ValorDEF.Tabla]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Valor objEntidad)
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

        public DataTable obtenerValores(String codigoBanco)
        {
            String query = "SELECT vb.CODIGO AS CodigoBanco, va.CODIGO AS CodigoAsobancaria"
                + " FROM tb_BAN_VALOR AS vb"
                + " INNER JOIN tb_BAN_TABLA AS tb ON vb.Tabla = tb.OID"
                + " INNER JOIN tb_BAN_TRANSFORMAR AS t ON vb.OID = t.Valor_Banco"
                + " INNER JOIN tb_BAN_VALOR AS va"
                + " INNER JOIN tb_BAN_TABLA AS ta ON va.Tabla = ta.OID ON t.Valor_Asobancaria = va.OID"
                + " WHERE (tb.Banco = '" + codigoBanco + "') AND (ta.ES_ASOBANCARIA = 1)";

            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;

            return datos;
        }
    }
}
