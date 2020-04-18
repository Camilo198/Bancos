using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pagos.AD.Servicios;
using Pagos.EN.Tablas;

namespace Pagos.LN.Consulta
{
    public class PagosTarjetaLN
    {
        WcfData wsc = new Pagos.AD.Servicios.WcfData();

        public List<string[,]> ValidaCodigoBanco(string partefija, string Fiducia, string procedimiento)
        {
            ObjetoTablas objValidacion = new ObjetoTablas();
            List<string[,]> listaValida = new List<string[,]>();
            try
            {
                List<ObjetoTablas> ListValidaCampoPago = new List<ObjetoTablas>();
                string[, ,] Param = new string[2, 3, 1];

                Param[0, 0, 0] = partefija;
                Param[0, 1, 0] = "@partefija";
                Param[0, 2, 0] = "varchar(10)";

                Param[1, 0, 0] = Fiducia;
                Param[1, 1, 0] = "@Fiducia";
                Param[1, 2, 0] = "varchar(3)";

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

        public List<string[,]> ValidaExistePagoTarjeta(ObjetoTablas PagosTarjeta, string procedimiento)
        {
            ObjetoTablas objPagoValidacion = new ObjetoTablas();
            List<string[,]> listaPagoValida = new List<string[,]>();
            try
            {

                List<ObjetoTablas> ListValidaCampoPago = new List<ObjetoTablas>();
                string[, ,] Param = new string[5, 3, 1];

                Param[0, 0, 0] = PagosTarjeta.pCodBanco;
                Param[0, 1, 0] = "@CodBanco";
                Param[0, 2, 0] = "int";

                Param[1, 0, 0] = PagosTarjeta.pNumAutorizacion;
                Param[1, 1, 0] = "@NumAutorizacion";
                Param[1, 2, 0] = "varchar(20)";

                Param[2, 0, 0] = PagosTarjeta.pValTotal.ToString();
                Param[2, 1, 0] = "@ValTotal";
                Param[2, 2, 0] = "numeric(18,2)";

                Param[3, 0, 0] = PagosTarjeta.pFranquicia;
                Param[3, 1, 0] = "@Franquicia";
                Param[3, 2, 0] = "varchar(20)";

                Param[4, 0, 0] = PagosTarjeta.pFecVale;
                Param[4, 1, 0] = "@FecVale";
                Param[4, 2, 0] = "datetime";


                listaPagoValida = wsc.LlenarLista(Param, procedimiento, "SQLVentas", "SP", "Sql");
                string[,] ValidaPago_;
                for (int l = 0; l < listaPagoValida.Count; l++)
                {
                    ValidaPago_ = listaPagoValida[l];
                    objPagoValidacion.pCodBanco = ValidaPago_[0, 1].ToString();
                    objPagoValidacion.pNumAutorizacion = ValidaPago_[1, 1].ToString();
                    ListValidaCampoPago.Add(objPagoValidacion);
                }

                return listaPagoValida;
            }
            catch (Exception)
            {
                return listaPagoValida;
            }
        }

        public string IUDPagosTarjeta(ObjetoTablas ObjPago, string procedimiento)
        {
            try
            {
                string[, ,] Valor = new string[26, 3, 1];


                Valor[0, 0, 0] = ObjPago.pCodBanco;
                Valor[0, 1, 0] = "@CodBanco";
                Valor[0, 2, 0] = "int";

                Valor[1, 0, 0] = ObjPago.pCodUnico.ToString();
                Valor[1, 1, 0] = "@CodUnico";
                Valor[1, 2, 0] = "varchar(20)";

                Valor[2, 0, 0] = ObjPago.pFranquicia;
                Valor[2, 1, 0] = "@Franquicia";
                Valor[2, 2, 0] = "varchar(50)";

                Valor[3, 0, 0] = ObjPago.pCuenta.ToString();
                Valor[3, 1, 0] = "@Cuenta";
                Valor[3, 2, 0] = "varchar(50)";

                Valor[4, 0, 0] = ObjPago.pFecVale;
                Valor[4, 1, 0] = "@FecVale";
                Valor[4, 2, 0] = "datetime";

                Valor[5, 0, 0] = ObjPago.pFecProceso;
                Valor[5, 1, 0] = "@FecProceso";
                Valor[5, 2, 0] = "datetime";

                Valor[6, 0, 0] = ObjPago.pFecAbono;
                Valor[6, 1, 0] = "@FecAbono";
                Valor[6, 2, 0] = "datetime";

                Valor[7, 0, 0] = ObjPago.pNumTarjeta.ToString();
                Valor[7, 1, 0] = "@NumTarjeta";
                Valor[7, 2, 0] = "varchar(30)";

                Valor[8, 0, 0] = ObjPago.pHora;
                Valor[8, 1, 0] = "@Hora";
                Valor[8, 2, 0] = "char(10)";

                Valor[9, 0, 0] = ObjPago.pComprobante;
                Valor[9, 1, 0] = "@Comprobante";
                Valor[9, 2, 0] = "varchar(50)";

                Valor[10, 0, 0] = ObjPago.pNumAutorizacion;
                Valor[10, 1, 0] = "@NumAutorizacion";
                Valor[10, 2, 0] = "varchar(50)";

                Valor[11, 0, 0] = ObjPago.pTerminal;
                Valor[11, 1, 0] = "@Terminal";
                Valor[11, 2, 0] = "varchar(50)";

                Valor[12, 0, 0] = ObjPago.pValVenta.ToString();
                Valor[12, 1, 0] = "@ValVenta";
                Valor[12, 2, 0] = "numeric(18,2)";

                Valor[13, 0, 0] = ObjPago.pValIva.ToString();
                Valor[13, 1, 0] = "@ValIva";
                Valor[13, 2, 0] = "numeric(18,2)";

                Valor[14, 0, 0] = ObjPago.pValPropina.ToString();
                Valor[14, 1, 0] = "@ValPropina";
                Valor[14, 2, 0] = "numeric(18,2)";

                Valor[15, 0, 0] = ObjPago.pValTotal.ToString();
                Valor[15, 1, 0] = "@ValTotal";
                Valor[15, 2, 0] = "numeric(18,2)";

                Valor[16, 0, 0] = ObjPago.pValComision.ToString();
                Valor[16, 1, 0] = "@ValComision";
                Valor[16, 2, 0] = "numeric(18,2)";

                Valor[17, 0, 0] = ObjPago.pValRetIva.ToString();
                Valor[17, 1, 0] = "@ValRetIva";
                Valor[17, 2, 0] = "numeric(18,2)";

                Valor[18, 0, 0] = ObjPago.pValRetIca.ToString();
                Valor[18, 1, 0] = "@ValRetIca";
                Valor[18, 2, 0] = "numeric(18,2)";

                Valor[19, 0, 0] = ObjPago.pValRetFuente.ToString();
                Valor[19, 1, 0] = "@ValRetFuente";
                Valor[19, 2, 0] = "numeric(18,2)";

                Valor[20, 0, 0] = ObjPago.pValAbono.ToString();
                Valor[20, 1, 0] = "@ValAbono";
                Valor[20, 2, 0] = "numeric(18,2)";

                Valor[21, 0, 0] = ObjPago.pTipTarjeta.ToString();
                Valor[21, 1, 0] = "@TipTarjeta";
                Valor[21, 2, 0] = "varchar(30)";

                Valor[22, 0, 0] = ObjPago.pPlazo;
                Valor[22, 1, 0] = "@Plazo";
                Valor[22, 2, 0] = "int";

                Valor[23, 0, 0] = ObjPago.pPorComision;
                Valor[23, 1, 0] = "@PorComision";
                Valor[23, 2, 0] = "varchar(10)";

                Valor[24, 0, 0] = ObjPago.pLegalizado;
                Valor[24, 1, 0] = "@Legalizado";
                Valor[24, 2, 0] = "char(1)";

                Valor[25, 0, 0] = ObjPago.pNumLote;
                Valor[25, 1, 0] = "@NumLote";
                Valor[25, 2, 0] = "int";

                return wsc.Ejecutar(Valor, procedimiento, "SQLVentas");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
