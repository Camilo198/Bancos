using Procesos.AD.Consultas;
using Procesos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Procesos.LN.Consultas
{
    public class ArchivoLN
    {
        private String SP_QueryArchivoBolsa = "pa_BAN_ARCHIVOS_BOLSA";

        public string insertarArchivoBolsaLN(ArchivoEN objEntidad)
        {
            return new ArchivoAD().insertarArchivoBolsaAD(SP_QueryArchivoBolsa, objEntidad);
        }
        public IList<ArchivoEN> consultarArchivoBolsaLN(ArchivoEN objEntidad)
        {
            return new ArchivoAD().consultarArchivoBolsaAD(SP_QueryArchivoBolsa, objEntidad);
        }
        public string eliminarArchivoBolsaLN(ArchivoEN objEntidad, String Operacion)
        {
            return new ArchivoAD().elimiarArchivoBolsaAD(SP_QueryArchivoBolsa, objEntidad, Operacion);
        }

    }
}
