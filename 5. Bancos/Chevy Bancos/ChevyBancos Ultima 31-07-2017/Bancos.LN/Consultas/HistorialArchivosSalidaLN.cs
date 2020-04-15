using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;

namespace Bancos.LN.Consultas
{
    public class HistorialArchivosSalidaLN
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
        public int insertar(HistorialArchivosSalida objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            HistorialArchivosSalidaAD objConsultor = new HistorialArchivosSalidaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public DataTable consultarFechasXBanco(String IdCuenta, String TipoArchivoS)
        {
            return new HistorialArchivosSalidaAD().consultarFechasXBanco(IdCuenta, TipoArchivoS);
        }

        public DataTable consultarConsecutivoXBanco(String IdCuenta, String TipoArchivoS, String Fecha)
        {
            return new HistorialArchivosSalidaAD().consultarConsecutivoXBanco(IdCuenta, TipoArchivoS, Fecha);
        }

        public DataTable consultarLineasConsecutivo(String IdCuenta, String TipoArchivoS, String Fecha, String Consecutivo)
        {
            return new HistorialArchivosSalidaAD().consultarLineasConsecutivo(IdCuenta, TipoArchivoS, Fecha, Consecutivo);
        }
              
    }
}
