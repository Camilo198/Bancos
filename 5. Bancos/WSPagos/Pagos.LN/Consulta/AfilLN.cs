using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pagos.AD.Servicios;
using Pagos.EN;
using System.Data;
using Pagos.AD.Conexion;
using System.Data.SqlClient;

namespace Pagos.LN.Consulta
{
    public class AfilLN
    {

        WcfData wsc = new Pagos.AD.Servicios.WcfData();

        private DataTable consultar(String query)
        {
            Querys objQuery = new Querys();
            DataTable datos = objQuery.consultarDatosSICO(query).Tables["tabla"];
            return datos;
        }

        public DataTable consultarExistenciaCupo(String grupo, String afiliacion, String nivel)
        {
/*FIDCON*/  String query = "select AfilIdeNro,AfilNatJur,AfilNroCon from AFIL where AfilGrupo = " + grupo + " and AfilNroAf = " + afiliacion + " and AfilNivAf = " + nivel; 
            return consultar(query);
        }

        public DataTable consultarExistenciaContrato(String contrato)
        {
            String query = "select AfilIdeNro,AfilNatJur from AFIL where AfilNroCon = " + contrato;
            return consultar(query);
        }

        public DataTable consultarExistenciaContratoBlanco(String contrato)
        {
            String query = "SELECT CONT.ContNro, CONT.ContOfc, CONT.ContEstado FROM CONT WHERE (CONT.ContNro= " + contrato + ") AND (CONT.ContOfc>0) AND (CONT.ContEstado='B')";
            return consultar(query);
        }

        public DataTable consultarLugarPago(String Codigo)
        {
            String query = "SELECT LupaFlagFiller FROM LUPA WHERE LupaCodigo= " + Codigo + "";
            return consultar(query);
        }

        public DataTable consultarCupo(String contrato)
        {
            String query = "select AfilGrupo,AfilNroAf,AfilNivAf from AFIL where AfilEstado <> 'T' and AfilNroCon = " + contrato;
            return consultar(query);
        }
    }
}
