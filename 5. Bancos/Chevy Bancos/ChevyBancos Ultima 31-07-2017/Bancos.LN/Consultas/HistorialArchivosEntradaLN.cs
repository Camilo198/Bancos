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
    public class HistorialArchivosEntradaLN
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }
        /// <summary>
        /// Permite operar un registro especifico
        /// </summary>
        /// <param name="objEntidad">Datos del registro a operar</param>
        /// <returns>Registros afectados</returns>
        ///
        public int insertar(HistorialArchivosEntrada objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            HistorialArchivosEntradaAD objConsultor = new HistorialArchivosEntradaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarConsecutivoXBanco(String IdCuenta, String TipoArchivoS, String Fecha, String FechaTransaccion)
        {
            return new HistorialArchivosEntradaAD().consultarConsecutivoXBanco(IdCuenta, TipoArchivoS, Fecha, FechaTransaccion);
        }

    }
}
