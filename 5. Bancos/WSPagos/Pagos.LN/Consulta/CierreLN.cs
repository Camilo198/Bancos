using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pagos.AD.Servicios;

namespace Pagos.LN.Consulta
{
    public class CierreLN
    {
           WcfData wsc = new Pagos.AD.Servicios.WcfData();

        public List<string[,]> ValidaExisteCierre(ObjetoTablas ObjCierre, string procedimiento)
        {
            ObjetoTablas objCierreValidacion = new ObjetoTablas();
            List<string[,]> listaCierreValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaExisteCierre = new List<ObjetoTablas>();
                string[, ,] Valor = new string[1, 3, 1];

                Valor[0, 0, 0] = ObjCierre.pContrato;
                Valor[0, 1, 0] = "@Contrato";
                Valor[0, 2, 0] = "varchar(15)";

                listaCierreValida = wsc.LlenarLista(Valor, procedimiento, "SQLVentas", "SP", "Sql");
                string[,] ValidaCierre_;
                for (int l = 0; l < listaCierreValida.Count; l++)
                {
                    ValidaCierre_ = listaCierreValida[l];
                    objCierreValidacion.pContrato = ValidaCierre_[0, 1].ToString();
                    objCierreValidacion.pEstado = ValidaCierre_[1, 1].ToString();
                    objCierreValidacion.pidtitular = ValidaCierre_[2, 1].ToString();
                    ListValidaExisteCierre.Add(objCierreValidacion);
                }

                return listaCierreValida;
            }
            catch (Exception)
            {
                return listaCierreValida;
            }
        }
    }
}
