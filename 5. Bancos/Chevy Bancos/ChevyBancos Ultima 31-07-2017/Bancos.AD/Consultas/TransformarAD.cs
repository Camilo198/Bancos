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
    public class TransformarAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        
        public ILog Registrador { get; set; }

        public TransformarAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Transformar objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Transformar", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorAsobancaria", SqlDbType.VarChar));
                if (objEntidad.pValorAsobancaria > 0)
                {
                    adaptador.SelectCommand.Parameters["@pValorAsobancaria"].Value = objEntidad.pValorAsobancaria;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorAsobancaria"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorBanco", SqlDbType.VarChar));
                if (objEntidad.pValorBanco > 0)
                {
                    adaptador.SelectCommand.Parameters["@pValorBanco"].Value = objEntidad.pValorBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorBanco"].Value = DBNull.Value;
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
        public List<Transformar> consultar(Transformar objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Transformar> lista = new List<Transformar>();
            Transformar objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Transformar();
                objEntidad2.pValorAsobancaria = Convertidor.aEntero32(fila[TransformarDEF.ValorAsobancaria]);
                objEntidad2.pValorBanco = Convertidor.aEntero32(fila[TransformarDEF.ValorBanco]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Transformar objEntidad)
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

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="banco"></param>
        /// <param name="tablaAso"></param>
        /// <param name="tablaBanco"></param>
        /// <returns></returns>
        public List<Transformar> consultar(String banco, String tablaBanco, String tablaAso)
        {
            String query = "SELECT v.DESCRIPCION AS DescripcionBanco, v.CODIGO AS CodBanco, tr.Valor_Banco, v2.DESCRIPCION AS DescripcionAsobancaria, v2.CODIGO AS CodAso, tr.Valor_Asobancaria"
                + " FROM tb_BAN_TRANSFORMAR AS tr"
                + " INNER JOIN tb_BAN_VALOR AS v ON tr.Valor_Banco = v.OID"
                + " INNER JOIN tb_BAN_TABLA AS t ON v.Tabla = t.OID"
                + " INNER JOIN tb_BAN_VALOR AS v2 ON tr.Valor_Asobancaria = v2.OID"
                + " INNER JOIN tb_BAN_TABLA AS t2 ON v2.Tabla = t2.OID"
                + " WHERE (t.Banco = '" + banco + "') AND (t.OID = " + tablaBanco + ")"
                + " AND (t2.OID = " + tablaAso + ") AND (t2.ES_ASOBANCARIA = 1)";

            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;

            List<Transformar> lista = new List<Transformar>();
            Transformar objEntidad = null;
            foreach (DataRow fila in datos.Rows)
            {
                objEntidad = new Transformar();
                objEntidad.pCodigoAsobancaria = Convertidor.aCadena(fila["CodAso"]);
                objEntidad.pCodigoBanco = Convertidor.aCadena(fila["CodBanco"]);
                objEntidad.pDescripcionAsobancaria = Convertidor.aCadena(fila["DescripcionAsobancaria"]);
                objEntidad.pDescripcionBanco = Convertidor.aCadena(fila["DescripcionBanco"]);
                objEntidad.pValorAsobancaria = Convertidor.aEntero32(fila[TransformarDEF.ValorAsobancaria]);
                objEntidad.pValorBanco = Convertidor.aEntero32(fila[TransformarDEF.ValorBanco]);

                lista.Add(objEntidad);
            }

            return lista;
        }
    }
}
