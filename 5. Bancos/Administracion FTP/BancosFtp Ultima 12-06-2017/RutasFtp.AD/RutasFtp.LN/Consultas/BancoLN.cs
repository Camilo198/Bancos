using System;
using System.Collections.Generic;
using System.Text;

using RutasFtp.AD;
using RutasFtp.AD.Consultas;
using RutasFtp.EN;
using RutasFtp.EN.Tablas;
using System.Data;

namespace RutasFtp.LN.Consultas
{
    public class BancoLN
    {
        public String Error { get; set; }

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
