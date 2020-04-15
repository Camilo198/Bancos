using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Collections;

namespace Procesos.PS.Procesos
{
    public class MoverArchivos
    {
        public String moverAFtp(ref bool procesoConError)
        {
            String mensaje = String.Empty;
            try
            {
                ServicioMoverArchivosFTP.moverArchivosFTP procesoMoverArchivos;
                procesoMoverArchivos = new ServicioMoverArchivosFTP.moverArchivosFTP();
                mensaje = procesoMoverArchivos.trasladarArchivos();

                if (!mensaje.Equals("Se han movido todos los archivos"))
                {
                    procesoConError = true;
                }

                return mensaje;
            }
            catch
            {
                procesoConError = true;
                return "Error en el servicio";
            }
        }
            

    }
}
