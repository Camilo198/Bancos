using System;
using System.Collections.Generic;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;

namespace Bancos.LN.Consultas
{
    public class InterpreteArchivoLN
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
        public List<InterpreteArchivo> consultar(InterpreteArchivo objEntidad)
        {
            InterpreteArchivoAD objConsultor = new InterpreteArchivoAD();
            List<InterpreteArchivo> lista = new List<InterpreteArchivo>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(InterpreteArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            InterpreteArchivoAD objConsultor = new InterpreteArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(InterpreteArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            InterpreteArchivoAD objConsultor = new InterpreteArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(InterpreteArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            InterpreteArchivoAD objConsultor = new InterpreteArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="banco"></param>
        /// <param name="tipoLinea"></param>
        /// <param name="asobancaria"></param>
        /// <returns></returns>
        public List<InterpreteArchivo> consultar(String IdCuentaBancoEpicor, String asobancaria)
        {
            InterpreteArchivoAD objConsultor = new InterpreteArchivoAD();
            List<InterpreteArchivo> lista = objConsultor.consultar(IdCuentaBancoEpicor, asobancaria);
            Error = objConsultor.Error;
            return lista;
        }
    }
}
