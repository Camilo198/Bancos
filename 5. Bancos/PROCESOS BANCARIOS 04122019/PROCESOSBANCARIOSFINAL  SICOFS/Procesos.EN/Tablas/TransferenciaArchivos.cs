using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procesos.EN.Tablas
{
    public class TransferenciaArchivos
    {
        public String id { get; set; }
        public String procesoTipo { get; set; }
        public String procesoDescripcion { get; set; }
        public String rutaOrigen { get; set; }
        public String rutaDestino { get; set; }
        public String rutaCifrado { get; set; }
        public String rutaRepositorio { get; set; }
        public String nomArchivo { get; set; }
        public String rutaPendientes { get; set; }
        public String rutaFTP { get; set; }
        public String descifra { get; set; }
        public String rango { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
