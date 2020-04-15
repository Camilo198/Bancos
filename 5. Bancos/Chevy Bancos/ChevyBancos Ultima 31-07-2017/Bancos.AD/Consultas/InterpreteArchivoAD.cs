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
    public class InterpreteArchivoAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public InterpreteArchivoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(InterpreteArchivo objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_BAN_Interprete_Archivo", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOid", SqlDbType.VarChar));
                if (objEntidad.pId > 0)
                {
                    adaptador.SelectCommand.Parameters["@pOid"].Value = objEntidad.pId;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pOid"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCampoAsobancaria", SqlDbType.VarChar));
                if (objEntidad.pCampoAsobancaria != null)
                {
                    adaptador.SelectCommand.Parameters["@pCampoAsobancaria"].Value = objEntidad.pCampoAsobancaria;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCampoAsobancaria"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCampoBanco", SqlDbType.VarChar));
                if (objEntidad.pCampoBanco != null)
                {
                    adaptador.SelectCommand.Parameters["@pCampoBanco"].Value = objEntidad.pCampoBanco;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCampoBanco"].Value = DBNull.Value;
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
        public List<InterpreteArchivo> consultar(InterpreteArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<InterpreteArchivo> lista = new List<InterpreteArchivo>();
            InterpreteArchivo objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new InterpreteArchivo();
                objEntidad2.pId = Convertidor.aEntero32(fila[InterpreteArchivoDEF.Id]);
                objEntidad2.pCampoAsobancaria = Convertidor.aEntero32(fila[InterpreteArchivoDEF.CampoAsobancaria]);
                objEntidad2.pCampoBanco = Convertidor.aEntero32(fila[InterpreteArchivoDEF.CampoBanco]);

                lista.Add(objEntidad2);
            }

            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(InterpreteArchivo objEntidad)
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

        public List<InterpreteArchivo> consultar(String IdCuentaBancoEpicor, String asobancaria)
        {
            String query = "SELECT ea.NOMBRE_CAMPO AS NombreCampoBanco, ia.CAMPO_BANCO, ea2.NOMBRE_CAMPO AS NombreCampoAsobancaria, ia.CAMPO_ASOBANCARIA, ia.ID,"
                + "c2.Tipo_Linea as TipoLinea FROM tb_BAN_CONFIGURACION AS c2"
                + " INNER JOIN tb_BAN_ARCHIVO_ASOBANCARIA AS aa ON c2.Archivo_Asobancaria = aa.OID"
                + " INNER JOIN tb_BAN_ESTRUCTURA_ARCHIVO AS ea2 ON c2.OID = ea2.Configuracion"
                + " INNER JOIN tb_BAN_CONFIGURACION AS c"
                + " INNER JOIN tb_BAN_ARCHIVO_PLANO AS ap ON c.Archivo_Plano = ap.OID"
                + " INNER JOIN tb_BAN_ESTRUCTURA_ARCHIVO AS ea ON c.OID = ea.Configuracion"
                + " INNER JOIN tb_BAN_INTERPRETE_ARCHIVO AS ia ON ea.OID = ia.Campo_Banco ON ea2.OID = ia.CAMPO_ASOBANCARIA"
                + " WHERE (ap.Id_Cuenta_Banco = '" + IdCuentaBancoEpicor + "') AND ";
             if (asobancaria == "ABT1")
            {
               query = query + " (aa.OID = 'ABT1' or aa.OID = 'TCR1')";
            }
            else
                 query = query + " aa.OID = '" + asobancaria + "'";


            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;

            List<InterpreteArchivo> lista = new List<InterpreteArchivo>();
            InterpreteArchivo objEntidad2 = null;
            foreach (DataRow fila in datos.Rows)
            {
                objEntidad2 = new InterpreteArchivo();
                objEntidad2.pId = Convertidor.aEntero32(fila[InterpreteArchivoDEF.Id]);
                objEntidad2.pCampoAsobancaria = Convertidor.aEntero32(fila[InterpreteArchivoDEF.CampoAsobancaria]);
                objEntidad2.pCampoBanco = Convertidor.aEntero32(fila[InterpreteArchivoDEF.CampoBanco]);
                objEntidad2.pNombreCampoAsobancaria = Convertidor.aCadena(fila["NombreCampoAsobancaria"]);
                objEntidad2.pNombreCampoBanco = Convertidor.aCadena(fila["NombreCampoBanco"]);
                objEntidad2.pTipoLinea = Convertidor.aCadena(fila["TipoLinea"]);

                lista.Add(objEntidad2);
            }

            return lista;
        }
    }
}
