using System;
using System.Collections.Generic;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;

namespace Bancos.LN.Consultas
{
    public class TransformarLN
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
        public List<Transformar> consultar(Transformar objEntidad)
        {
            TransformarAD objConsultor = new TransformarAD();
            List<Transformar> lista = new List<Transformar>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int borrar(Transformar objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TransformarAD objConsultor = new TransformarAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int insertar(Transformar objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TransformarAD objConsultor = new TransformarAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        public int actualizar(Transformar objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TransformarAD objConsultor = new TransformarAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        /// <summary>
        /// Permite la consulta de los ajustes existentes en la base de datos
        /// </summary>
        /// <param name="banco"></param>
        /// <param name="tablaAso"></param>
        /// <param name="tablaBanco"></param>
        /// <returns></returns>
        public List<Transformar> consultar(String banco, String tablaBanco, String tablaAso)
        {
            TransformarAD objConsultor = new TransformarAD();
            List<Transformar> lista = new List<Transformar>();
            lista = objConsultor.consultar(banco, tablaBanco, tablaAso);
            Error = objConsultor.Error;
            return lista;
        }
    }
}
