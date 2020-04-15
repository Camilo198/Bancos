using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Bancos.EN;

namespace Bancos.PS.Codigo
{
    public class UtilidadesWeb
    {
        /// <summary>
        /// Imprime un mensaje en un label de acuerdo a unas configuraciones.
        /// </summary>
        /// <param name="etiqueta">Label a ajustar</param>
        /// <param name="mensaje">Mensaje a imprimir</param>
        /// <param name="tipoMensaje">Tipo de mensaje</param>
        public static void ajustarMensaje(Label etiqueta, String mensaje, TipoMensaje tipoMsg)
        {
            if (tipoMsg.Equals(TipoMensaje.Informacion))
            {
                etiqueta.Font.Bold = false;
                etiqueta.ForeColor = Color.Black;

                mensaje = "<b>[INFO]</b> " + mensaje;
            }
            else if (tipoMsg.Equals(TipoMensaje.Error))
            {
                etiqueta.Font.Bold = true;
                etiqueta.ForeColor = Color.Red;

                mensaje = "<b>[ERROR]</b> " + mensaje;
            }
            etiqueta.Text = mensaje;
        }

        /// <summary>
        /// Agrega el valor de seleccione a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarSeleccioneDDL(DropDownList lista)
        {
            lista.Items.Insert(0, new ListItem("[Seleccione]", "0"));
        }

        /// <summary>
        /// Agrega el valor de seleccione a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarSeleccioneDDL(AjaxControlToolkit.ComboBox lista)
        {
            lista.Items.Insert(0, new ListItem("[Seleccione]", "0"));
        }

        /// <summary>
        /// Agrega el valor de todos a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarTodosDDL(DropDownList lista)
        {
            lista.Items.Insert(1, new ListItem("[Todo]", "TODO"));
        }

        /// <summary>
        /// Agrega el valor de todos a una lista desplegable.
        /// </summary>
        /// <param name="lista">Lista desplegable</param>
        public static void agregarTodosDDL(AjaxControlToolkit.ComboBox lista)
        {
            lista.Items.Insert(1, new ListItem("[Todo]", "TODO"));
        }
    }
}