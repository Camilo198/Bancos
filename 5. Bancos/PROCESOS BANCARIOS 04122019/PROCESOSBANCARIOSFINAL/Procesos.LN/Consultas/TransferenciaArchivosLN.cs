using Procesos.AD.Consultas;
using Procesos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.LN.Consultas
{
    public class TransferenciaArchivosLN
    {
        
        public string ActualizaRutas(TransferenciaArchivos objEntidad)
        {
            TransferenciaArchivosAD objAdjunto = new TransferenciaArchivosAD();
            return objAdjunto.ActualizaRutas(objEntidad);

        }
        public string ActualizaDescifrado(TransferenciaArchivos objEntidad)
        {
            TransferenciaArchivosAD objAdjunto = new TransferenciaArchivosAD();
            return objAdjunto.ActualizaDescifrado(objEntidad);

        }

        public string consultaDisponibilidadDescifrado(TransferenciaArchivos objEntidad)
        {
            TransferenciaArchivosAD objAdjunto = new TransferenciaArchivosAD();
            return objAdjunto.consultaDisponibilidadDescifrado(objEntidad)[0].descifra.ToString();
        }
    }
    }

