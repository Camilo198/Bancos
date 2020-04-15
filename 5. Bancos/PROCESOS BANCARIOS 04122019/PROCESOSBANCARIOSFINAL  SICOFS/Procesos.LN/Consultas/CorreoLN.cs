using Procesos.AD.Consultas;
using Procesos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.LN.Consultas
{
    public class CorreoLN
    {
        public List<Correo> Consulta(Correo ObjEntidad)
        {
            List<Correo> lista = new List<Correo>();
            return lista = new CorreoAD().ConsultaCorreos(ObjEntidad);
        }

        public string UpdateDisponibilidadCorreo(Correo ObjEntidad)
        {

            return new CorreoAD().UpdateDisponiblidadCorreo("pa_ban_update_Disponibilidad_Correo", ObjEntidad);
        }
    }
}
