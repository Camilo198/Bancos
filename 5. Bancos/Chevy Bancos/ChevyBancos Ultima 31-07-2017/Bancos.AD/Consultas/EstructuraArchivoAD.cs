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
    public class EstructuraArchivoAD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public ILog Registrador { get; set; }

        public EstructuraArchivoAD()
        {
            Registrador = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// Esta funcion es la encargada de llenar los datos y ejecutar un procedimiento almacenado
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Conjuntos de datos de respuesta de la ejecución del procedimiento almacenado</returns>
        protected DataSet ejecutarConsulta(EstructuraArchivo objEntidad)
        {
            ConectorBD objConexionDB = ConectorBD.obtenerInstancia();
            SqlDataAdapter adaptador;
            DataSet datos = null;
            SqlConnection conexion = null;
            try
            {
                conexion = objConexionDB.abrirConexion();
                Error = conexion.State.ToString();
                adaptador = new SqlDataAdapter("pa_Ban_Estructura_Archivo", conexion);
                adaptador.SelectCommand.CommandType = CommandType.StoredProcedure;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOperacion", SqlDbType.VarChar));
                adaptador.SelectCommand.Parameters["@pOperacion"].Value = objEntidad.pOperacion;

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pAlineacion", SqlDbType.VarChar));
                if (objEntidad.pAlineacion != null)
                {
                    adaptador.SelectCommand.Parameters["@pAlineacion"].Value = objEntidad.pAlineacion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pAlineacion"].Value = DBNull.Value;
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
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pConfiguracion", SqlDbType.VarChar));
                if (objEntidad.pConfiguracion > 0)
                {
                    adaptador.SelectCommand.Parameters["@pConfiguracion"].Value = objEntidad.pConfiguracion;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pConfiguracion"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCantidadDecimales", SqlDbType.VarChar));
                if (objEntidad.pCantidadDecimales >= 0)
                {
                    adaptador.SelectCommand.Parameters["@pCantidadDecimales"].Value = objEntidad.pCantidadDecimales;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCantidadDecimales"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pCaracterRelleno", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pCaracterRelleno))
                {
                    adaptador.SelectCommand.Parameters["@pCaracterRelleno"].Value = objEntidad.pCaracterRelleno;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pCaracterRelleno"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pEsContador", SqlDbType.VarChar));
                if (objEntidad.pEsContador != null)
                {
                    if (objEntidad.pEsContador.Value)
                        adaptador.SelectCommand.Parameters["@pEsContador"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pEsContador"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pEsContador"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pRequiereCambio", SqlDbType.VarChar));
                if (objEntidad.pRequiereCambio != null)
                {
                    if (objEntidad.pRequiereCambio.Value)
                        adaptador.SelectCommand.Parameters["@pRequiereCambio"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pRequiereCambio"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pRequiereCambio"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pFormatoFecha", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pFormatoFecha))
                {
                    adaptador.SelectCommand.Parameters["@pFormatoFecha"].Value = objEntidad.pFormatoFecha;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pFormatoFecha"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pLongitud", SqlDbType.VarChar));
                if (objEntidad.pLongitud > 0)
                {
                    adaptador.SelectCommand.Parameters["@pLongitud"].Value = objEntidad.pLongitud;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pLongitud"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pNombreCampo", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pNombreCampo))
                {
                    adaptador.SelectCommand.Parameters["@pNombreCampo"].Value = objEntidad.pNombreCampo;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pNombreCampo"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pOrdenColumna", SqlDbType.VarChar));
                if (objEntidad.pOrdenColumna > 0)
                {
                    adaptador.SelectCommand.Parameters["@pOrdenColumna"].Value = objEntidad.pOrdenColumna;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pOrdenColumna"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pTipoDato", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pTipoDato))
                {
                    adaptador.SelectCommand.Parameters["@pTipoDato"].Value = objEntidad.pTipoDato;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pTipoDato"].Value = DBNull.Value;
                }

                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValorPorDefecto", SqlDbType.VarChar));
                if (objEntidad.pValorPorDefecto != null)
                {
                    if (objEntidad.pValorPorDefecto.Value)
                        adaptador.SelectCommand.Parameters["@pValorPorDefecto"].Value = "1";
                    else
                        adaptador.SelectCommand.Parameters["@pValorPorDefecto"].Value = "0";
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValorPorDefecto"].Value = DBNull.Value;
                }
                adaptador.SelectCommand.Parameters.Add(new SqlParameter("@pValor", SqlDbType.VarChar));
                if (!String.IsNullOrEmpty(objEntidad.pValor))
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = objEntidad.pValor;
                }
                else
                {
                    adaptador.SelectCommand.Parameters["@pValor"].Value = String.Empty;
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
        public List<EstructuraArchivo> consultar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            DataSet datos = ejecutarConsulta(objEntidad);

            List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
            EstructuraArchivo objEntidad2 = null;
            foreach (DataRow fila in datos.Tables["tabla"].Rows)
            {
                objEntidad2 = new EstructuraArchivo();
                objEntidad2.pAlineacion = Convertidor.aCadena(fila[EstructuraArchivoDEF.Alineacion]);
                objEntidad2.pCantidadDecimales = Convertidor.aEntero32(fila[EstructuraArchivoDEF.CantidadDecimales]);
                objEntidad2.pCaracterRelleno = Convertidor.aCadena(fila[EstructuraArchivoDEF.CaracterRelleno]);
                objEntidad2.pConfiguracion = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Configuracion]);
                objEntidad2.pEsContador = Convertidor.aBooleano(fila[EstructuraArchivoDEF.EsContador]);
                objEntidad2.pFormatoFecha = Convertidor.aCadena(fila[EstructuraArchivoDEF.FormatoFecha]);
                objEntidad2.pLongitud = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Longitud]);
                objEntidad2.pNombreCampo = Convertidor.aCadena(fila[EstructuraArchivoDEF.NombreCampo]);
                objEntidad2.pOid = Convertidor.aEntero32(fila[EstructuraArchivoDEF.Oid]);
                objEntidad2.pOrdenColumna = Convertidor.aEntero32(fila[EstructuraArchivoDEF.OrdenColumna]);
                objEntidad2.pTipoDato = Convertidor.aCadena(fila[EstructuraArchivoDEF.TipoDato]);
                objEntidad2.pRequiereCambio = Convertidor.aBooleano(fila[EstructuraArchivoDEF.RequiereCambio]);
                objEntidad2.pValorPorDefecto = Convertidor.aBooleano(fila[EstructuraArchivoDEF.ValorPorDefecto]);
                objEntidad2.pValor = Convertidor.aCadena(fila[EstructuraArchivoDEF.Valor]);
                lista.Add(objEntidad2);
            }
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int ejecutarNoConsulta(EstructuraArchivo objEntidad)
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

        public DataTable consultarCamposCuenta(String CodCuenta, String TipoLinea, String TipoProceso)
        {
            String query = "SELECT ea.OID, ea.NOMBRE_CAMPO"
                + " FROM tb_BAN_ESTRUCTURA_ARCHIVO ea, tb_BAN_CONFIGURACION c, tb_BAN_ARCHIVO_PLANO ap"
                + " WHERE c.OID = ea.Configuracion"
                + " AND ap.OID = c.Archivo_Plano"
                + " AND c.Tipo_Linea = '" + TipoLinea + "'"
                + " AND ap.Id_Cuenta_Banco = '" + CodCuenta + "'"
                + " AND ap.Tipo_Proceso = '" + TipoProceso + "'"
                + " ORDER BY ea.NOMBRE_CAMPO";
            return consultar(query);
        }

        public DataTable consultarCamposAsobancaria(String TipoLinea, String tipoProceso, string CodCuenta)
        {
            String query = "SELECT ea.OID, ea.NOMBRE_CAMPO"
                + " FROM tb_BAN_ESTRUCTURA_ARCHIVO ea, tb_BAN_CONFIGURACION c, tb_BAN_ARCHIVO_ASOBANCARIA aa , tb_BAN_ARCHIVO_PLANO ap"
                + " WHERE c.OID = ea.Configuracion"
                + " AND aa.OID = c.Archivo_Asobancaria"
                + " and c.Archivo_Plano = ap.OID "
                + " AND aa.OID = '" + tipoProceso + "'"
                + " AND c.Tipo_Linea = '" + TipoLinea + "'"
                + " and c.Archivo_Asobancaria ='"  + tipoProceso + "'"
                + " and ap.Id_Cuenta_Banco = '00000'"  
                + " ORDER BY ea.NOMBRE_CAMPO";


               //+ " and ap.Id_Cuenta_Banco = '" + CodCuenta + "'" 
            return consultar(query);
        }

        public DataTable consultarEstructuraArchivoSalida(String tipoLinea,String tipoProceso)
        {
            String query = "SELECT ea.OID, ea.Configuracion, ea.Tipo_Dato, ea.NOMBRE_CAMPO, ea.ORDEN_COLUMNA, ea.LONGITUD,"
                + " ea.CARACTER_RELLENO, ea.ALINEACION, ea.CANTIDAD_DECIMALES, ea.FORMATO_FECHA, ea.ES_CONTADOR,"
                + " ea.REQUIERE_CAMBIO, c." + ConfiguracionDEF.TipoLinea + ","
                + " ea." + EstructuraArchivoDEF.ValorPorDefecto + ", ea." + EstructuraArchivoDEF.Valor
                + " FROM tb_BAN_ESTRUCTURA_ARCHIVO AS ea"
                + " INNER JOIN tb_BAN_CONFIGURACION AS c ON ea.Configuracion = c.OID"
                + " WHERE (c.Archivo_Asobancaria = '" + tipoProceso + "')"
                + " AND (c.Tipo_Linea = '" + tipoLinea + "')";
            if (tipoLinea == "3DT" && tipoProceso != "POL1")
            {
                if (tipoProceso == "ABT1")
                    query = query + " AND (c.Archivo_Plano = 39) ORDER BY ea.ORDEN_COLUMNA";
                else
                    query = query + " AND (c.Archivo_Plano = 44) ORDER BY ea.ORDEN_COLUMNA";
            }
            else
                query = query + " ORDER BY ea.ORDEN_COLUMNA";

                    return consultar(query);
        }

        //public DataTable consultarEstructuraAsobancaria() // no esta en uso
        //{
        //    String query = "SELECT ea.OID, ea.Configuracion, ea.Tipo_Dato, ea.NOMBRE_CAMPO, ea.ORDEN_COLUMNA, ea.LONGITUD,"
        //        + " ea.CARACTER_RELLENO, ea.ALINEACION, ea.CANTIDAD_DECIMALES, ea.FORMATO_FECHA, ea.ES_CONTADOR,"
        //        + " c." + ConfiguracionDEF.TipoLinea + ", ea.REQUIERE_CAMBIO,"
        //        + " ea." + EstructuraArchivoDEF.ValorPorDefecto + ", ea." + EstructuraArchivoDEF.Valor
        //        + " FROM tb_BAN_ESTRUCTURA_ARCHIVO AS ea"
        //        + " INNER JOIN tb_BAN_CONFIGURACION AS c ON ea.Configuracion = c.OID"
        //        + " WHERE (c.Archivo_Asobancaria = 'ABR')"
        //        + " ORDER BY ea.ORDEN_COLUMNA";
        //    return consultar(query);
        //}

        public DataTable consultarEstructuraBanco(String IdCuentaBanco, String tipoLinea,String tipoProceso)
        {
            String query = "SELECT ea.OID, ea.Configuracion, ea.Tipo_Dato, ea.NOMBRE_CAMPO, ea.ORDEN_COLUMNA, ea.LONGITUD,"
                + " ea.CARACTER_RELLENO, ea.ALINEACION, ea.CANTIDAD_DECIMALES, ea.FORMATO_FECHA, ea.ES_CONTADOR,"
                + " c." + ConfiguracionDEF.TipoLinea + ", ea.REQUIERE_CAMBIO,"
                + " ea." + EstructuraArchivoDEF.ValorPorDefecto + ", ea." + EstructuraArchivoDEF.Valor
                + " FROM tb_BAN_ARCHIVO_PLANO AS ap"
                + " INNER JOIN tb_BAN_CONFIGURACION AS c ON ap.OID = c.Archivo_Plano"
                + " INNER JOIN tb_BAN_ESTRUCTURA_ARCHIVO AS ea ON c.OID = ea.Configuracion";
            if(tipoLinea == "3DT" && tipoProceso == "TCR1")
                query = query + " WHERE (c.Tipo_Linea = '" + tipoLinea + "') AND (c.Archivo_Asobancaria = 'ABT1') AND (ap.Id_Cuenta_Banco = '" + IdCuentaBanco + "') AND (ap.Tipo_Proceso = '" + tipoProceso + "')"
                + " ORDER BY ea.ORDEN_COLUMNA";
            else
                query = query + " WHERE (c.Tipo_Linea = '" + tipoLinea + "') AND (c.Archivo_Asobancaria = '" + tipoProceso + "') AND (ap.Id_Cuenta_Banco = '" + IdCuentaBanco + "') AND (ap.Tipo_Proceso = '" + tipoProceso + "')"
                    + " ORDER BY ea.ORDEN_COLUMNA";
            return consultar(query);

        }

        //public DataTable consultarEstructuraBanco(String codBanco)
        //{
        //    String query = "SELECT ea.OID, ea.Configuracion, ea.Tipo_Dato, ea.NOMBRE_CAMPO, ea.ORDEN_COLUMNA, ea.LONGITUD,"
        //        + " ea.CARACTER_RELLENO, ea.ALINEACION, ea.CANTIDAD_DECIMALES, ea.FORMATO_FECHA, ea.ES_CONTADOR,"
        //        + " c." + ConfiguracionDEF.TipoLinea + ", ea.REQUIERE_CAMBIO,"
        //        + " ea." + EstructuraArchivoDEF.ValorPorDefecto + ", ea." + EstructuraArchivoDEF.Valor
        //        + " FROM tb_BAN_ARCHIVO_PLANO AS ap"
        //        + " INNER JOIN tb_BAN_CONFIGURACION AS c ON ap.OID = c." + ConfiguracionDEF.ArchivoPlano
        //        + " INNER JOIN tb_BAN_ESTRUCTURA_ARCHIVO AS ea ON c.OID = ea.Configuracion"
        //        + " WHERE (ap.Id_Cuenta_Banco = '" + codBanco + "' AND c.Tipo_Linea = '" + codBanco + "' )"
        //        + " ORDER BY c." + ConfiguracionDEF.TipoLinea + ", ea.ORDEN_COLUMNA";
        //    return consultar(query);
        //}
    }
}
