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
    public class FtpLN
    {
        public String Error { get; set; }


        public List<Ftp> consultar(Ftp objEntidad)
        {
            FtpAD objConsultor = new FtpAD();
            List<Ftp> lista = new List<Ftp>();
            lista = objConsultor.consultar(objEntidad);
            Error = objConsultor.Error;
            return lista;
        }     

        public int insertar(Ftp objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            FtpAD objConsultor = new FtpAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Ftp objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            FtpAD objConsultor = new FtpAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarFecha(Ftp objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_2;
            int cuenta = -1;
            FtpAD objConsultor = new FtpAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int borrar(Ftp objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            FtpAD objConsultor = new FtpAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarFechaUltimaCopia(Ftp objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_2;
            int cuenta = -1;
            FtpAD objConsultor = new FtpAD();
            cuenta = objConsultor.ejecutarNoConsultaUltimaCopia(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }
    }
}
