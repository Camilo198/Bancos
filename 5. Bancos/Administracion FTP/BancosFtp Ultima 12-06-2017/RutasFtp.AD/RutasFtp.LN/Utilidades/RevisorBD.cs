using System;

using RutasFtp.AD.Administracion;

namespace RutasFtp.LN.Utilidades
{
    public class RevisorBD
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public static String Error { get; set; }

        private RevisorBD() { }

        public static bool comprobarConectividad()
        {
            bool respuesta = AuxiliarCx.comprobarConectividad();
            Error = AuxiliarCx.Error;
            return respuesta;
        }
    }
}
