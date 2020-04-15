using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Procesos.AD;
using Procesos.AD.Consultas;
using Procesos.EN;
using Procesos.EN.Tablas;

namespace Procesos.LN.Consultas
{
    public class TareaLN
    {
        public String Error { get; set; }

        public int insertar(Tareas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.INSERTAR;
            int cuenta = -1;
            TareaAD objConsultor = new TareaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizar(Tareas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR;
            int cuenta = -1;
            TareaAD objConsultor = new TareaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarIncioProceso(Tareas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_2;
            int cuenta = -1;
            TareaAD objConsultor = new TareaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int actualizarIncioProcesoConError(Tareas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ACTUALIZAR_3;
            int cuenta = -1;
            TareaAD objConsultor = new TareaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public int eliminar(Tareas objEntidad)
        {
            objEntidad.pOperacion = TiposConsultas.ELIMINAR;
            int cuenta = -1;
            TareaAD objConsultor = new TareaAD();
            cuenta = objConsultor.ejecutarNoConsulta(objEntidad);
            Error = objConsultor.Error;
            return cuenta;
        }

        public Tareas consultarProceso(Tareas objEntidad)
        {
            TareaAD objConsultor = new TareaAD();
            Tareas objeto = new Tareas();
            objeto = objConsultor.consultarProceso(objEntidad);
            Error = objConsultor.Error;
            return objeto;
        }

        public DataTable consultar()
        {
            return new TareaAD().consultar();
        }

    }
}
