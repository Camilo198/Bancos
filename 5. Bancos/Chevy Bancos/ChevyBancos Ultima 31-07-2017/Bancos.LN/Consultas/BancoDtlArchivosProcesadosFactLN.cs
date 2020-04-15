using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;

namespace Bancos.LN.Consultas
{
    public class BancoDtlArchivosProcesadosFactLN
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public List<BancoDtlArchivosProcesadosFact> consultar(BancoDtlArchivosProcesadosFact objEntidad)
        {
            BancoDtlArchivosProcesadosFactAD objConsultor = new BancoDtlArchivosProcesadosFactAD();
            List<BancoDtlArchivosProcesadosFact> lista = new List<BancoDtlArchivosProcesadosFact>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(BancoDtlArchivosProcesadosFact objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            BancoDtlArchivosProcesadosFactAD objConsultor = new BancoDtlArchivosProcesadosFactAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(BancoDtlArchivosProcesadosFact objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            BancoDtlArchivosProcesadosFactAD objConsultor = new BancoDtlArchivosProcesadosFactAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int actualizar(BancoDtlArchivosProcesadosFact objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            BancoDtlArchivosProcesadosFactAD objConsultor = new BancoDtlArchivosProcesadosFactAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarLineasErradas(DateTime fini, DateTime ffin)
        {
            BancoDtlArchivosProcesadosFactAD objConsultor = new BancoDtlArchivosProcesadosFactAD();
            return objConsultor.consultarLineasErradas(fini, ffin);
        }

        public object consultarLineasErradas()
        {
            BancoDtlArchivosProcesadosFactAD objConsultor = new BancoDtlArchivosProcesadosFactAD();
            return objConsultor.consultarLineasErradas();
        }
    }
}