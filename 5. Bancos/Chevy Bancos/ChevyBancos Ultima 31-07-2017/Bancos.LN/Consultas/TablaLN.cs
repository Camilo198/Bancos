using System;
using System.Collections.Generic;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;

namespace Bancos.LN.Consultas
{
    public class TablaLN
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
        public List<Tabla> consultarTablasValor(Tabla objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR;
            TablaAD objConsultor = new TablaAD();
            List<Tabla> lista = new List<Tabla>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="objEntidad">Entidad que contienen los datos a llenar en los parametros del procedimiento almacenado</param>
        /// <returns>Lista de datos</returns>
        public List<Tabla> consultarTablasRespuesta(Tabla objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.CONSULTAR_2;
            TablaAD objConsultor = new TablaAD();
            List<Tabla> lista = new List<Tabla>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(Tabla objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TablaAD objConsultor = new TablaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(Tabla objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TablaAD objConsultor = new TablaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar2(Tabla objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR_2;
            int cuenta = -1;
            TablaAD objConsultor = new TablaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int actualizar(Tabla objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TablaAD objConsultor = new TablaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
