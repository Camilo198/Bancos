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

            String respuesta = new ArchivoAD().insertarArchivoBolsaAD(SP_QueryArchivoBolsa, objEntidad);
            return respuesta;
        }
        public IList<ArchivoEN> consultarArchivoBolsaLN(ArchivoEN objEntidad)
        {

            IList<ArchivoEN> lista = new ArchivoAD().consultarArchivoBolsaAD(SP_QueryArchivoBolsa, objEntidad);
            return lista;
        }
        public string eliminarArchivoBolsaLN(ArchivoEN objEntidad)
        {

            String respuesta = new ArchivoAD().elimiarArchivoBolsaAD(SP_QueryArchivoBolsa, objEntidad);
            return respuesta;
        }

    }
}
