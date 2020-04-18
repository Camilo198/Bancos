using Pagos.AD.Conexion;
using Pagos.AD.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
    public class ParaLN
    {
        WcfData wsc = new Pagos.AD.Servicios.WcfData();

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatosSICO(query).Tables["tabla"];
            return datos;
        }

        public DataTable consultarParametro()
        {
            //
            String query = "SELECT ParaCodigo, ParaAnoPro, ParaMesPro FROM PARA  WHERE (PARA.ParaCodigo='01')";
            return consultar(query);
        }


    }
}
