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
    public class EquivalenciasAD
    {
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public EquivalenciasAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        protected DataSet ejecutarConsulta(Equivalencias objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;

            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_BAN_Equivalencias", conexion);
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
                    adaptador.SelectCommand.Parameters["@pId"].Value = "NULL";
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdEstructuraArchivo", SqlDbType.VarChar));
                if (objEntidad.pIdEstructuraArchivo > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdEstructuraArchivo"].Value = objEntidad.pIdEstructuraArchivo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdEstructuraArchivo"].Value = "NULL";
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdTablasEquivalencias", SqlDbType.VarChar));
                if (objEntidad.pIdTablasEquivalencias > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdTablasEquivalencias"].Value = objEntidad.pIdTablasEquivalencias;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdTablasEquivalencias"].Value = "NULL";
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pIdCamposEquivalencias", SqlDbType.VarChar));
                if (objEntidad.pIdCamposEquivalencias > 0)
                {
                    adaptador.SelectCommand.Parameters["@pIdCamposEquivalencias"].Value = objEntidad.pIdCamposEquivalencias;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pIdCamposEquivalencias"].Value = "NULL";
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

        public List<Equivalencias> consultar(Equivalencias objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<Equivalencias> lista = new List<Equivalencias>();
            Equivalencias objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new Equivalencias();
                objEntidad2.pId = Convertidor.aEntero32(fila[EquivalenciasDEF.Id]);
                objEntidad2.pIdEstructuraArchivo = Convertidor.aEntero32(fila[EquivalenciasDEF.IdEstructuraArchivo]);
                objEntidad2.pIdTablasEquivalencias = Convertidor.aEntero32(fila[EquivalenciasDEF.IdTablasEquivalencias]);
                objEntidad2.pIdCamposEquivalencias = Convertidor.aEntero32(fila[EquivalenciasDEF.IdCamposEquivalencias]);

                lista.Add(objEntidad2);
            }
            return lista;
        }

        public int ejecutarNoConsulta(Equivalencias objEntidad)
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

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatos(query).Tables["tabla"];
            Error = objQuery.Error;
            return datos;
        }

        public DataTable consultarEquivalencias(String Tipo_Proceso, String TipoLinea, String IdCuentaBanco)
        {
            String query = "select E.ID as ID,E.ID_ESTRUCTURA_ARCHIVO as IDEA,E.ID_TABLAS_EQUIVALENCIAS as IDTE,E.ID_CAMPOS_EQUIVALENCIAS as IDCE,"
                            + "EA.NOMBRE_CAMPO as NOMBREEA,TE.NOMBRE as NOMBRETE,CE.DESCRIPCION as DESCRIPCIONCE "
                            + "FROM tb_BAN_EQUIVALENCIAS as E "
                            + "INNER JOIN tb_BAN_ESTRUCTURA_ARCHIVO AS EA ON (EA.OID = E.ID_ESTRUCTURA_ARCHIVO) "
                            + "INNER JOIN tb_BAN_CONFIGURACION AS C ON (C.OID = EA.CONFIGURACION) "
                            + "INNER JOIN tb_BAN_ARCHIVO_PLANO AS AP ON (C.Archivo_Plano = AP.OID) "
                            + "FULL OUTER JOIN tb_BAN_TABLAS_EQUIVALENCIAS AS TE ON (TE.ID = E.ID_TABLAS_EQUIVALENCIAS) "
                            + "FULL OUTER JOIN tb_BAN_CAMPOS_EQUIVALENCIAS AS CE ON (CE.ID = E.ID_CAMPOS_EQUIVALENCIAS) "
                            + "WHERE (C.OID ='" + TipoLinea + "') AND (AP.Tipo_Proceso ='" + Tipo_Proceso + "') AND (AP.Id_Cuenta_Banco='" + IdCuentaBanco + "')";
            return consultar(query);
        }

        public DataTable consultarEquivalenciasXTipoArchivo(String Tipo_Proceso, String IdCuentaBanco)
        {
            String query = " select E.ID as ID,E.ID_ESTRUCTURA_ARCHIVO as IDEA,E.ID_TABLAS_EQUIVALENCIAS as IDTE,E.ID_CAMPOS_EQUIVALENCIAS as IDCE,"
                            + "EA.NOMBRE_CAMPO as NOMBREEA,TE.NOMBRE as NOMBRETE,CE.DESCRIPCION as DESCRIPCIONCE "
                            + "FROM tb_BAN_EQUIVALENCIAS as E "
                            + "INNER JOIN tb_BAN_ESTRUCTURA_ARCHIVO AS EA ON (EA.OID = E.ID_ESTRUCTURA_ARCHIVO) "
                            + "INNER JOIN tb_BAN_CONFIGURACION AS C ON (C.OID = EA.CONFIGURACION) "
                            + "INNER JOIN tb_BAN_ARCHIVO_PLANO AS AP ON (C.Archivo_Plano = AP.OID) "
                            + "FULL OUTER JOIN tb_BAN_TABLAS_EQUIVALENCIAS AS TE ON (TE.ID = E.ID_TABLAS_EQUIVALENCIAS) "
                            + "FULL OUTER JOIN tb_BAN_CAMPOS_EQUIVALENCIAS AS CE ON (CE.ID = E.ID_CAMPOS_EQUIVALENCIAS) "
                            + "WHERE (AP.Tipo_Proceso ='" + Tipo_Proceso + "') AND (AP.Id_Cuenta_Banco='" + IdCuentaBanco + "')";
            return consultar(query);
        }
    }
}
