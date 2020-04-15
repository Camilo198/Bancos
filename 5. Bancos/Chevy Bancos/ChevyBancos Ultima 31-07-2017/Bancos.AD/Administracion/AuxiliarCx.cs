using System;
using System.Data;
using System.Data.SqlClient;

using Bancos.AD.Conexion;
using log4net;

namespace Bancos.AD.Administracion
{
    public class AuxiliarCx
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public static String Error { get; set; }

        private AuxiliarCx() { }

        public static bool comprobarConectividad()
        {
            ILog log = LogManager.GetLogger("Bancos.AD.Administracion.AuxiliarCx");
            ConectorBD objCx = ConectorBD.obtenerInstancia();
            SqlConnection objSqlCx = null;
            try
            {
                objSqlCx = objCx.abrirConexion();
                Error = objCx.Error;
            }
            catch (SqlException ex)
            {
                Error = ex.Message;
                log.Error(Error);
            }
            finally
            {
                if (objSqlCx.State != ConnectionState.Closed)
                    objSqlCx.Close();
            }
            return objCx.SeEstablecioConexion;
        }
    }
}