using System;
using System.Collections.Generic;
using System.Text;

using Bancos.AD;
using Bancos.AD.Consultas;
using Bancos.EN;
using Bancos.EN.Tablas;
using System.Data;

namespace Bancos.LN.Consultas
{
    public class PagosOnlineLN
    {
        /// <summary>
        /// Mensajes que se generan de la ejecucion de las funciones contenidas en esta clase
        /// </summary>
        public String Error { get; set; }

       
       
        public DataSet consultar()
        {
            PagosOnlineAd POL = new PagosOnlineAd();

            return POL.ejecutarConsulta();
        }


        public string Ejecutar()
        {
            PagosOnlineAd POL = new PagosOnlineAd();

            return POL.ejecutarUpdate();  
        }
  
    }
}
