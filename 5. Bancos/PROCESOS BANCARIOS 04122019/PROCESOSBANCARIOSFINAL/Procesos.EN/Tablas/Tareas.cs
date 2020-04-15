using System;


namespace Procesos.EN.Tablas
{
     [Serializable()]
    public class Tareas
    {
         public String pOperacion { get; set; }

         public int? pId { get; set; }

         public String pNombreTarea { get; set; }
         public String pPeriodo { get; set; }
         public String pTiempoIntervalo { get; set; }
         public String pInicio { get; set; }
         public String pProceso { get; set; }
         public String pCorreoControl { get; set; }
    }
}
