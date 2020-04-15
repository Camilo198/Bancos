using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

using Bancos.AD.Administracion;
using log4net;

namespace Bancos.AD.Conexion
{
    internal class ConectorBD
    {
        public String Error { get; set; }
        public bool SeEstablecioConexion { get; set; }
        private ConectorBD() { }
        private static ConectorBD conexion = null;

        /// <summary>
        /// Establece la conexion con la base de datos.
        /// </summary>
        /// <returns>Conexion con la base de datos, para realizar transacciones</returns>
        public SqlConnection abrirConexion()
        {
            ILog log = LogManager.GetLogger("Bancos.AD.Administracion.AuxiliarCx");
            ConsultorXML objConsultorXML = new ConsultorXML();
            String cadenaConexion = objConsultorXML.leerCadenaConexion();
            SqlConnection objSqlConect = new SqlConnection();
            objSqlConect.ConnectionString = cadenaConexion;
            try
            {
                objSqlConect.Open();
                SeEstablecioConexion = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                log.Fatal(Error);
                SeEstablecioConexion = false;
            }
            return objSqlConect;
        }

        /// <summary>
        /// Retorna la instacia de la coneción actual, o la crea en caso de que este nula
        /// </summary>
        /// <returns>Instancia de la conexion actual</returns>
        public static ConectorBD obtenerInstancia()
        {
            if (conexion == null)
                conexion = new ConectorBD();
            return conexion;
        }
    }
}
