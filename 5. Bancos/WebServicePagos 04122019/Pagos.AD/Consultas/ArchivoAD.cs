using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.AD.Consultas
{
    public class ArchivoAD
    {
        WcfData wsc = new WcfData();
        public String InsertarLineasPagoAD(IList<ArchivoEN> parametros, String procedimiento)
        {
            DataTable dt = new DataTable();
            //Add columns  
            dt.Columns.Add(new DataColumn("codBanco", typeof(int)));
            dt.Columns.Add(new DataColumn("parteFija", typeof(string)));
            dt.Columns.Add(new DataColumn("fechaRecaudo", typeof(string)));
            dt.Columns.Add(new DataColumn("fechaProceso", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("numLinea", typeof(int)));
            dt.Columns.Add(new DataColumn("linea", typeof(string)));
            dt.Columns.Add(new DataColumn("cantPagos", typeof(int)));
            //Add rows  
            foreach (var item in parametros)
            {
                dt.Rows.Add(item.codBanco, item.parteFija, item.fechaRecaudo, item.fechaProceso, item.numLinea, item.linea, item.cantPagos);
            }
            // Ejecutar Query
            try
            {
                return wsc.ejecutarQueryDataTable(dt, procedimiento, "SQLBancos", "@tablaParametros");
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        public string EliminarLineasPagoAD(String procedimiento, ArchivoEN objEntidad, String Operacion)
        {
            try
            {
                string[,,] Param = new string[7, 3, 1];

                Param[0, 0, 0] = objEntidad.codBanco.ToString();
                Param[0, 1, 0] = "@CodBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = objEntidad.parteFija.ToString();
                Param[1, 1, 0] = "@parteFija";
                Param[1, 2, 0] = "varchar(max)";

                Param[2, 0, 0] = objEntidad.fechaRecaudo.ToString();
                Param[2, 1, 0] = "@fechaRecaudo";
                Param[2, 2, 0] = "date";

                Param[3, 0, 0] = objEntidad.fechaProceso.ToString();
                Param[3, 1, 0] = "@fechaProceso";
                Param[3, 2, 0] = "datetime";

                Param[4, 0, 0] = objEntidad.cantPagos.ToString();
                Param[4, 1, 0] = "@cantPagos";
                Param[4, 2, 0] = "int";

                Param[5, 0, 0] = objEntidad.numLinea.ToString();
                Param[5, 1, 0] = "@numLinea";
                Param[5, 2, 0] = "int";

                Param[6, 0, 0] = Operacion;
                Param[6, 1, 0] = "@pOperacion";
                Param[6, 2, 0] = "varchar(3)";

                return wsc.Ejecutar(Param, procedimiento, "SQLBancos");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
