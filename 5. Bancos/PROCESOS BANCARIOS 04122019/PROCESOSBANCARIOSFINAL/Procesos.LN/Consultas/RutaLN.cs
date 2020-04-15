using System;
using System.Collections.Generic;
using System.Text;

using Procesos.AD;
using Procesos.AD.Consultas;
using Procesos.EN;
using Procesos.EN.Tablas;

namespace Procesos.LN.Consultas
{
    public class RutaLN
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
        public List<Ruta> consultar(Ruta objEntidad)
        {
            RutaAD objConsultor = new RutaAD();
            List<Ruta> lista = new List<Ruta>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        ///// <summary>
        ///// Permite operar un registro especifico
        ///// </summary>
        ///// <param name="objEntidad">Datos del registro a operar</param>
        ///// <returns>Registros afectados</returns>
        //public int borrar(Ruta objEntidad)
        //{
        //    objEntidad.pOperacion = TiposConsultas.ELIMINAR;
        //    int cuenta = -1;
        //    RutaAD objConsultor = new RutaAD();
        //    cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
        //    Error = objConsultor.Error;
        //    return cuenta;
        //}

        ///// <summary>
        ///// Permite operar un registro especifico
        ///// </summary>
        ///// <param name="objEntidad">Datos del registro a operar</param>
        ///// <returns>Registros afectados</returns>
        //public int insertar(Ruta objEntidad)
        //{
        //    objEntidad.pOperacion = TiposConsultas.INSERTAR;
        //    int cuenta = -1;
        //    RutaAD objConsultor = new RutaAD();
        //    cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
        //    Error = objConsultor.Error;
        //    return cuenta;
        //}

        ///// <summary>
        ///// Permite operar un registro especifico
        ///// </summary>
        ///// <param name="objEntidad">Datos del registro a operar</param>
        ///// <returns>Registros afectados</returns>
        //public int actualizar(Ruta objEntidad)
        //{
        //    objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
        //    int cuenta = -1;
        //    RutaAD objConsultor = new RutaAD();
        //    cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
        //    Error = objConsultor.Error;
        //    return cuenta;
        //}

        ///// <summary>
        ///// Permite la consulta de una ruta
        ///// </summary>
        ///// <param name="codigoBanco"></param>
        ///// <returns>Ruta de Entrada</returns>
        //public String consultarRutaEntrada(String codigoBanco)
        //{
        //    return new RutaAD().consultarRutaEntrada(codigoBanco);
        //}

        ///// <summary>
        ///// Permite la consulta de una ruta
        ///// </summary>
        ///// <param name="codigoBanco"></param>
        ///// <returns>Ruta de Salida</returns>
        //public String consultarRutaSalida(String codigoBanco)
        //{
        //    return new RutaAD().consultarRutaSalida(codigoBanco);
        //}
    }
}
