using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Procesos.AD.Consultas;
using Procesos.EN.Tablas;

namespace Procesos.LN.Consultas
{
    public class BancoLN
    {
        public String Error { get; set; }

        //public DataTable consultarBancosAsobancaria()
        //{
        //    return new BancoAD().consultarBancosAsobancaria();
        //}

        public List<Banco> consultar(Banco objEntidad)
        {
            BancoAD objConsultor = new BancoAD();
            List<Banco> lista = new List<Banco>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }
    }
}
