using Pagos.AD.Servicios;
using Pagos.EN.Tablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.LN.Consulta
{
   public class PagosLN
    {
       WcfData wsc = new Pagos.AD.Servicios.WcfData();
       private SqlParameter[] parametro;
        public List<string[,]> ConsultaLoteMaximo(string procedimiento)
        {
            ObjetoTablas objConsultaLote = new ObjetoTablas();
            List<string[,]> ListaPagos = new List<string[,]>();
            try
            {
                string[, ,] param = new string[0, 0, 0];
                return ListaPagos = wsc.LlenarLista(param, procedimiento, "SQLVentas", "SP", "Sql");
            }
            catch (Exception)
            {
                return ListaPagos;
            }
        }

        public string IUDPago(ObjetoTablas ObjPago, string procedimiento)
        {
            try
            {
                string[, ,] Valor = new string[16, 3, 1];

                Valor[0, 0, 0] = ObjPago.pCodBanco.ToString();
                Valor[0, 1, 0] = "@CodBanco";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = ObjPago.pFecPago;
                Valor[1, 1, 0] = "@FecPago";
                Valor[1, 2, 0] = "datetime";

                Valor[2, 0, 0] = ObjPago.pValPago;
                Valor[2, 1, 0] = "@ValPago";
                Valor[2, 2, 0] = "numeric(18,2)";

                Valor[3, 0, 0] = ObjPago.pForPago;
                Valor[3, 1, 0] = "@ForPago";
                Valor[3, 2, 0] = "varchar(10)";

                Valor[4, 0, 0] = ObjPago.pContrato;
                Valor[4, 1, 0] = "@Contrato";
                Valor[4, 2, 0] = "numeric(10,0)";

                Valor[5, 0, 0] = ObjPago.pEstado.ToString();
                Valor[5, 1, 0] = "@Estado";
                Valor[5, 2, 0] = "char(1)";

                Valor[6, 0, 0] = ObjPago.pUsuProceso.ToString();
                Valor[6, 1, 0] = "@UsuProceso";
                Valor[6, 2, 0] = "varchar(30)";

                Valor[7, 0, 0] = ObjPago.pLegalizado;
                Valor[7, 1, 0] = "@Legalizado";
                Valor[7, 2, 0] = "char(1)";

                Valor[8, 0, 0] = ObjPago.pContAnterior;
                Valor[8, 1, 0] = "@ContAnterior";
                Valor[8, 2, 0] = "numeric(10,0)";

                Valor[9, 0, 0] = ObjPago.pValComision.ToString();
                Valor[9, 1, 0] = "@ValComision";
                Valor[9, 2, 0] = "numeric(18,2)";

                Valor[10, 0, 0] = ObjPago.pValRetFuente.ToString();
                Valor[10, 1, 0] = "@ValRetFuente";
                Valor[10, 2, 0] = "numeric(18,2)";

                Valor[11, 0, 0] = ObjPago.pValRetIva.ToString();
                Valor[11, 1, 0] = "@ValRetIva";
                Valor[11, 2, 0] = "numeric(18,2)";

                Valor[12, 0, 0] = ObjPago.pValRetIca.ToString();
                Valor[12, 1, 0] = "@ValRetIca";
                Valor[12, 2, 0] = "numeric(18,2)";

                Valor[13, 0, 0] = ObjPago.pNumAutorizacion;
                Valor[13, 1, 0] = "@NumAutorizacion";
                Valor[13, 2, 0] = "varchar(20)";

                Valor[14, 0, 0] = ObjPago.pHoraPago;
                Valor[14, 1, 0] = "@HoraPago";
                Valor[14, 2, 0] = "char(4)";

                Valor[15, 0, 0] = ObjPago.pNumLote;
                Valor[15, 1, 0] = "@NumLote";
                Valor[15, 2, 0] = "numeric(18,0)";

                return wsc.Ejecutar(Valor, procedimiento, "SQLVentas");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string[,]> ValidaExistePago(ObjetoTablas Pagos, string procedimiento)
        {
            ObjetoTablas objPagoValidacion = new ObjetoTablas();
            List<string[,]> listaPagoValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaCampoPago = new List<ObjetoTablas>();
                string[, ,] Param = new string[6, 3, 1];

                Param[0, 0, 0] = Pagos.pContrato.ToString();
                Param[0, 1, 0] = "@Contrato";
                Param[0, 2, 0] = "numeric";

                Param[1, 0, 0] = Pagos.pFecPago;
                Param[1, 1, 0] = "@FecPago";
                Param[1, 2, 0] = "datetime";

                Param[2, 0, 0] = Pagos.pValPago;
                Param[2, 1, 0] = "@ValPago";
                Param[2, 2, 0] = "numeric";

                Param[3, 0, 0] = Pagos.pCodBanco;
                Param[3, 1, 0] = "@CodBanco";
                Param[3, 2, 0] = "int";

                Param[4, 0, 0] = Pagos.pForPago;
                Param[4, 1, 0] = "@ForPago";
                Param[4, 2, 0] = "varchar(10)";

                Param[5, 0, 0] = Pagos.pNumAutorizacion;
                Param[5, 1, 0] = "@NumAutorizacion";
                Param[5, 2, 0] = "varchar(20)";

                listaPagoValida = wsc.LlenarLista(Param, procedimiento, "SQLVentas", "SP", "Sql");
                string[,] ValidaPago_;
                for (int l = 0; l < listaPagoValida.Count; l++)
                {
                    ValidaPago_ = listaPagoValida[l];
                    objPagoValidacion.pContrato = ValidaPago_[0, 1].ToString();
                    objPagoValidacion.pFecPago = ValidaPago_[1, 1].ToString();
                    objPagoValidacion.pValPago = ValidaPago_[2, 1].ToString();
                    objPagoValidacion.pCodBanco = ValidaPago_[3, 1].ToString();
                    objPagoValidacion.pNumAutorizacion = ValidaPago_[4, 1].ToString();
                    objPagoValidacion.pForPago = ValidaPago_[5, 1].ToString();
                    ListValidaCampoPago.Add(objPagoValidacion);
                }

                return listaPagoValida;
            }
            catch (Exception)
            {
                return listaPagoValida;
            }
        }

        public List<string[,]> ValidaCodigoBanco(string PagosOnline, string partefija, string Fiducia, string procedimiento)
        {
            ObjetoTablas objValidacion = new ObjetoTablas();
            List<string[,]> listaValida = new List<string[,]>();
            try
            {
                List<ObjetoTablas> ListValidaCampoPago = new List<ObjetoTablas>();
                string[, ,] Param = new string[3, 3, 1];

                Param[0, 0, 0] = PagosOnline;
                Param[0, 1, 0] = "@pagosonline";
                Param[0, 2, 0] = "varchar(1)";

                Param[1, 0, 0] = partefija;
                Param[1, 1, 0] = "@partefija";
                Param[1, 2, 0] = "varchar(10)";

                Param[2, 0, 0] = Fiducia;
                Param[2, 1, 0] = "@Fiducia";
                Param[2, 2, 0] = "varchar(3)";

                listaValida = wsc.LlenarLista(Param, procedimiento, "SQLVentas", "SP", "Sql");
                string[,] ValidaPago_;
                for (int l = 0; l < listaValida.Count; l++)
                {
                    ValidaPago_ = listaValida[l];
                    objValidacion.pNumCuenta = ValidaPago_[0, 1].ToString();
                    ListValidaCampoPago.Add(objValidacion);
                }

                return listaValida;
            }
            catch (Exception)
            {
                return listaValida;
            }
        }

        public string Legalizacionpagos(string procedimiento)
        {
            try
            {
                string[, ,] Valor = new string[0, 0, 0];

                return wsc.Ejecutar(Valor, procedimiento, "SQLVentas");

            }
            catch (Exception)
            {
                return "0";
            }

        }

        public DataSet ConsultaArchivo(string Fiducia, string Recuado)
        {
            DataSet NombreArchivo = new DataSet();
            try
            {
                parametro = new SqlParameter[2];
                parametro[0] = new SqlParameter("@Fiducia", Fiducia);
                parametro[1] = new SqlParameter("@Recaudo", Recuado);

                return NombreArchivo = wsc.consultaRegistro("ConsultaNombreArchivo", parametro, wsc.ConectaSql("SqlVentas"), "TablaBanPagos");
            }

            catch (Exception)
            {
                return NombreArchivo;
            }
        }

        public List<ObjetoTablas> ConsultaEstadoParametro(ObjetoTablas Parametro, string Procedimiento)
        {
            List<string[,]> listaParametro = new List<string[,]>();
            try
            {
                List<ObjetoTablas> ListParametros = new List<ObjetoTablas>();

                string[, ,] Param = new string[2, 3, 1];
                Param[0, 0, 0] = Parametro.pParametro;
                Param[0, 1, 0] = "@Parametros";
                Param[0, 2, 0] = "varchar(20)";

                Param[1, 0, 0] = Parametro.pVigente;
                Param[1, 1, 0] = "@Estado";
                Param[1, 2, 0] = "varchar(1)";

                listaParametro = wsc.LlenarLista(Param, Procedimiento, "SQLVentas", "SP", "Sql");
                string[,] ValidaParametro;
                for (int l = 0; l < listaParametro.Count; l++)
                {
                    ObjetoTablas objParametros = new ObjetoTablas();
                    ValidaParametro = listaParametro[l];
                    objParametros.pValor = ValidaParametro[0, 1].ToString();
                    ListParametros.Add(objParametros);
                }

                return ListParametros;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

   }
}
