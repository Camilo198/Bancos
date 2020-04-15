using System;
using System.Collections.Generic;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;
using System.Data;

namespace Bancos.LN.Consultas
{
    public class EstructuraArchivoLN
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
        public List<EstructuraArchivo> consultar(EstructuraArchivo objEntidad)
        {
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int actualizar(EstructuraArchivo objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            EstructuraArchivoAD objConsultor = new EstructuraArchivoAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarCamposCuenta(String CodCuenta, String TipoLinea, String TipoProceso)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarCamposCuenta(CodCuenta, TipoLinea, TipoProceso);
            Error = objEAAD.Error;
            return tabla;
        }

        public DataTable consultarCamposAsobancaria(String TipoLinea, String tipoProceso, string CodCuenta)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarCamposAsobancaria(TipoLinea, tipoProceso,CodCuenta);
            Error = objEAAD.Error;
            return tabla;
        }

        public DataTable consultarEstructuraArchivoSalida(String tipoLinea, String tipoProceso)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarEstructuraArchivoSalida(tipoLinea, tipoProceso);
            Error = objEAAD.Error;
            return tabla;
        }

        //public DataTable consultarEstructuraAsobancaria()
        //{
        //    EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
        //    DataTable tabla = objEAAD.consultarEstructuraAsobancaria();
        //    Error = objEAAD.Error;
        //    return tabla;
        //}

        public DataTable consultarEstructuraBanco(String codBanco, String tipoLinea,String tipoProceso)
        {
            EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
            DataTable tabla = objEAAD.consultarEstructuraBanco(codBanco, tipoLinea,tipoProceso);
            Error = objEAAD.Error;
            return tabla;
        }

        //public DataTable consultarEstructuraBanco(String codBanco)
        //{
        //    EstructuraArchivoAD objEAAD = new EstructuraArchivoAD();
        //    DataTable tabla = objEAAD.consultarEstructuraBanco(codBanco);
        //    Error = objEAAD.Error;
        //    return tabla;
        //}
    }
}