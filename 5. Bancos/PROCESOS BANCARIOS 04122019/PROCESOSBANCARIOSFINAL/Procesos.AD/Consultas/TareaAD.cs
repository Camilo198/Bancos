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
    public class TareaAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public TareaAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(Tareas objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Tareas", conexion);
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

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreTarea", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreTarea))
                {
                    adaptador.SelectCommand.Parameters["@pNombreTarea"].Value = objEntidad.pNombreTarea;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreTarea"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pPeriodo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pPeriodo))
                {
                    adaptador.SelectCommand.Parameters["@pPeriodo"].Value = objEntidad.pPeriodo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pPeriodo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTiempoIntervalo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTiempoIntervalo))
                {
                    adaptador.SelectCommand.Parameters["@pTiempoIntervalo"].Value = objEntidad.pTiempoIntervalo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTiempoIntervalo"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pInicio", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pInicio))
                {
                    adaptador.SelectCommand.Parameters["@pInicio"].Value = objEntidad.pInicio;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pInicio"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pProceso", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pProceso))
                {
                    adaptador.SelectCommand.Parameters["@pProceso"].Value = objEntidad.pProceso;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pProceso"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCorreoControl", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCorreoControl))
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = objEntidad.pCorreoControl;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCorreoControl"].Value = DBNull.Value;
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
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(Tareas objEntidad)
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

        public DataTable consultar()
        {
            String query = "SELECT Id,NombreTarea,Periodo,TiempoIntervalo,Inicio,Proceso,CorreoControl"
                + " FROM tb_BAN_TAREAS";
            return consultar(query);
        }

        public Tareas consultarProceso(Tareas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);
            EN.Tablas.Tareas objEntidad1 = new EN.Tablas.Tareas();            
            objEntidad1.pId = Convertidor.aEntero32(datos.Tables["tabla"].Rows[0][TareaDEF.Id]);
            objEntidad1.pNombreTarea = Convertidor.aCadena(datos.Tables["tabla"].Rows[0][TareaDEF.NombreTarea]);
            objEntidad1.pPeriodo = Convertidor.aCadena(datos.Tables["tabla"].Rows[0][TareaDEF.Periodo]);
            objEntidad1.pTiempoIntervalo = Convertidor.aCadena(datos.Tables["tabla"].Rows[0][TareaDEF.TiempoIntervalo]);
            objEntidad1.pInicio = Convertidor.aCadena(datos.Tables["tabla"].Rows[0][TareaDEF.Inicio]);
            objEntidad1.pProceso = Convertidor.aCadena(datos.Tables["tabla"].Rows[0][TareaDEF.Proceso]);
            objEntidad1.pCorreoControl  = Convertidor.aCadena(datos.Tables["tabla"].Rows[0][TareaDEF.CorreoControl]);
            return objEntidad1;
        }

        public DataTable consultarTarea(int pId)
        {
            String query = "SELECT Id,NombreTarea,Periodo,TiempoIntervalo,Inicio,Proceso,CorreoControl"
                + " FROM tb_BAN_TAREAS"
                + " WHERE Id = '" + pId.ToString() + "'";
            return consultar(query);
        }
    }
}
