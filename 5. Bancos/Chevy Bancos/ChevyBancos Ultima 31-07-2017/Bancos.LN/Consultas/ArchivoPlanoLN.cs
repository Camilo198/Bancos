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
    public class ArchivoPlanoLN
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos</returns>
        public List<ArchivoPlano> consultar(ArchivoPlano objEntidad)
        {
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            List<ArchivoPlano> lista = new List<ArchivoPlano>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(ArchivoPlano objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(ArchivoPlano objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int actualizar(ArchivoPlano objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarLineasCuentaBanco(String codigoCuenta, String TipoDProceso)
        {
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            return objConsultor.consultarLineasCuentaBanco(codigoCuenta, TipoDProceso);
        }

        public DataTable consultarLineasDisponibles(String codigoCuenta, String TipoDProceso)
        {
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            return objConsultor.consultarLineasDisponibles(codigoCuenta, TipoDProceso);
        }

        public DataTable consultarLineasAsobancaria(String TipoArchivo)
        {
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            return objConsultor.consultarLineasAsobancaria(TipoArchivo);
        }

        public DataTable consultarLineasDisponiblesAso(String TipoArchivo)
        {
            ArchivoPlanoAD objConsultor = new ArchivoPlanoAD();
            return objConsultor.consultarLineasDisponiblesAso(TipoArchivo);
        }
    }
}