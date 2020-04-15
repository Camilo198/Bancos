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
    public class BancoDtlArchivosProcesadosLN
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        public List<BancoDtlArchivosProcesados> consultar(BancoDtlArchivosProcesados objEntidad)
        {
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            List<BancoDtlArchivosProcesados> lista = new List<BancoDtlArchivosProcesados>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(BancoDtlArchivosProcesados objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(BancoDtlArchivosProcesados objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int actualizar(BancoDtlArchivosProcesados objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarLineasErradas(DateTime fini, DateTime ffin)
        {
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            return objConsultor.consultarLineasErradas(fini, ffin);
        }

        public DataTable consultarLineasErradas(String banco, DateTime fini, DateTime ffin)
        {
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            return objConsultor.consultarLineasErradas(banco, fini, ffin);
        }
        public object consultarLineasErradas()
        {
            BancoDtlArchivosProcesadosAD objConsultor = new BancoDtlArchivosProcesadosAD();
            return objConsultor.consultarLineasErradas();
        }
    }
}