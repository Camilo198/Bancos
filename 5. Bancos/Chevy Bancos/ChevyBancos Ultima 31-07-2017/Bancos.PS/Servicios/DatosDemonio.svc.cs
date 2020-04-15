using System;
using System.Collections.Generic;
using System.Data;

using Bancos.LN.Consultas;
using Bancos.LN.Utilidades;
using Bancos.EN.Tablas;
using Bancos.EN.Definicion;

namespace Bancos.PS.Servicios
{
    public class DatosDemonio : IDatosDemonio
    {
        public String consultarRutaEntrada(String codigoBanco)
        {
            return new RutaLN().consultarRutaEntrada(codigoBanco);
        }

        public String consultarRutaSalida(String codigoBanco)
        {
            return new RutaLN().consultarRutaSalida(codigoBanco);
        }

        public List<Banco> consultarBancos()
        {
            List<Banco> lista = new List<Banco>();
            DataTable tabla = new BancoLN().consultar();
            Banco banco;
            foreach (DataRow fila in tabla.Rows)
            {
                banco = new Banco();
                banco.aClave = Convertidor.aCadena(fila["CLAVE"]);
                banco.aCodigoBanco = Convertidor.aCadena(fila["CODIGO_BANCO"]);
                banco.aFtp = Convertidor.aBooleano(fila["FTP"]);
                banco.aIntervaloTiempo = Convertidor.aEntero32(fila["INTERVALO_TIEMPO"]);
                banco.aNombre = Convertidor.aCadena(fila["NOMBRE"]);
                banco.aRutaArchivosEntrada = Convertidor.aCadena(fila["RutaEntrada"]);
                banco.aRutaArchivosSalida = Convertidor.aCadena(fila["RutaSalida"]);
                banco.aUrlFtp = Convertidor.aCadena(fila["URL_FTP"]);
                banco.aUsuarioFtp = Convertidor.aCadena(fila["USUARIO_FTP"]);

                banco.aCodigoEntidadFinanciera = Convertidor.aCadena(fila["CODIGO_TRANSITO"]);
                banco.aCorreoControl = Convertidor.aCadena(fila["CORREO_CONTROL"]);
                banco.aCorreoEnvio = Convertidor.aCadena(fila["CORREO_ENVIO"]);
                banco.aActivo = Convertidor.aBooleano(fila["ACTIVO"]);
                banco.aRecFac = Convertidor.aBooleano(fila["ENTRADA"]);
                banco.aRemitente = Convertidor.aCadena(fila["REMITENTE"]);
                banco.aNumeCuenta = Convertidor.aCadena(fila["NUM_CUENTA"]);
                banco.aNombCuenta = Convertidor.aCadena(fila["NOM_CUENTA"]);
                banco.aTipoCuenta = Convertidor.aCadena(fila["TIPO_CUENTA"]);

                lista.Add(banco);
            }

            return lista;
        }

        public List<EstructuraArchivo> consultarEstructuraArchivoAsobancaria(String tipoLinea,String tipoProceso)
        {
            List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
            DataTable tabla = new EstructuraArchivoLN().consultarEstructuraArchivoSalida(tipoLinea, tipoProceso);
            EstructuraArchivo entidad;
            foreach (DataRow fila in tabla.Rows)
            {
                entidad = new EstructuraArchivo();
                entidad.aAlineacion = Convertidor.aCadena(fila["ALINEACION"]);
                entidad.aCantidadDecimales = Convertidor.aEntero32(fila["CANTIDAD_DECIMALES"]);
                entidad.aCaracterRelleno = Convertidor.aCadena(fila["CARACTER_RELLENO"]);
                entidad.aConfiguracion = Convertidor.aEntero32(fila["Configuracion"]);
                entidad.aEsContador = Convertidor.aBooleano(fila["ES_CONTADOR"]);
                entidad.aFormatoFecha = Convertidor.aCadena(fila["FORMATO_FECHA"]).ToUpper();
                entidad.aLongitud = Convertidor.aEntero32(fila["LONGITUD"]);
                entidad.aNombreCampo = Convertidor.aCadena(fila["NOMBRE_CAMPO"]);
                entidad.aOid = Convertidor.aEntero32(fila["OID"]);
                entidad.aOrdenColumna = Convertidor.aEntero32(fila["ORDEN_COLUMNA"]);
                entidad.aRequiereCambio = Convertidor.aBooleano(fila[EstructuraArchivoDEF.RequiereCambio]);
                entidad.aTipoDato = Convertidor.aCadena(fila["Tipo_Dato"]);
                entidad.aValor = Convertidor.aCadena(fila[EstructuraArchivoDEF.Valor]);
                entidad.aValorPorDefecto = Convertidor.aBooleano(fila[EstructuraArchivoDEF.ValorPorDefecto]);
                entidad.aTipoLinea = tipoLinea;

                lista.Add(entidad);
            }

            return lista;
        }

        public List<EstructuraArchivo> consultarEstructuraArchivoBanco(String codBanco)
        {
            List<EstructuraArchivo> lista = new List<EstructuraArchivo>();
            DataTable tabla = new EstructuraArchivoLN().consultarEstructuraBanco(codBanco,"xxx","");
            EstructuraArchivo entidad;
            foreach (DataRow fila in tabla.Rows)
            {
                entidad = new EstructuraArchivo();
                entidad.aAlineacion = Convertidor.aCadena(fila["ALINEACION"]);
                entidad.aCantidadDecimales = Convertidor.aEntero32(fila["CANTIDAD_DECIMALES"]);
                entidad.aCaracterRelleno = Convertidor.aCadena(fila["CARACTER_RELLENO"]);
                entidad.aConfiguracion = Convertidor.aEntero32(fila["Configuracion"]);
                entidad.aEsContador = Convertidor.aBooleano(fila["ES_CONTADOR"]);
                entidad.aFormatoFecha = Convertidor.aCadena(fila["FORMATO_FECHA"]).ToUpper();
                entidad.aLongitud = Convertidor.aEntero32(fila["LONGITUD"]);
                entidad.aNombreCampo = Convertidor.aCadena(fila["NOMBRE_CAMPO"]);
                entidad.aOid = Convertidor.aEntero32(fila["OID"]);
                entidad.aOrdenColumna = Convertidor.aEntero32(fila["ORDEN_COLUMNA"]);
                entidad.aRequiereCambio = Convertidor.aBooleano(fila[EstructuraArchivoDEF.RequiereCambio]);
                entidad.aTipoDato = Convertidor.aCadena(fila["Tipo_Dato"]);
                entidad.aTipoLinea = Convertidor.aCadena(fila[ConfiguracionDEF.TipoLinea]);
                entidad.aValor = Convertidor.aCadena(fila[EstructuraArchivoDEF.Valor]);
                entidad.aValorPorDefecto = Convertidor.aBooleano(fila[EstructuraArchivoDEF.ValorPorDefecto]);

                lista.Add(entidad);
            }

            return lista;
        }

        public List<Equivalencias> obtenerCausales(String banco)
        {
            List<Equivalencias> lista = new List<Equivalencias>();
            DataTable datos = new RespuestaTransaccionLN().obtenerCausales(banco);

            Equivalencias objEqv;
            foreach (DataRow fila in datos.Rows)
            {
                objEqv = new Equivalencias();
                objEqv.aAsobancaria = Convertidor.aCadena(fila["CausalAsobancaria"]);
                objEqv.aBanco = Convertidor.aCadena(fila["CausalBanco"]);
                lista.Add(objEqv);
            }

            return lista;
        }

        public List<Equivalencias> obtenerValores(String banco)
        {
            List<Equivalencias> lista = new List<Equivalencias>();
            DataTable datos = new ValorLN().obtenerValores(banco);

            Equivalencias objEqv;
            foreach (DataRow fila in datos.Rows)
            {
                objEqv = new Equivalencias();
                objEqv.aAsobancaria = Convertidor.aCadena(fila["CodigoAsobancaria"]);
                objEqv.aBanco = Convertidor.aCadena(fila["CodigoBanco"]);
                lista.Add(objEqv);
            }

            return lista;
        }

        public List<EquivalenciasArchivo> obtenerTransformacionArchivo(String banco)
        {
            List<EquivalenciasArchivo> lista = new List<EquivalenciasArchivo>();
            List<InterpreteArchivo> datos = new InterpreteArchivoLN().consultar(banco, "TCR");

            EquivalenciasArchivo objEqv;
            foreach (InterpreteArchivo fila in datos)
            {
                objEqv = new EquivalenciasArchivo();
                objEqv.aAsobancaria = fila.pCampoAsobancaria;
                objEqv.aBanco = fila.pCampoBanco;
                lista.Add(objEqv);
            }

            return lista;
        }

        public List<String> obtenerLineasBanco(String banco)
        {
            return new TipoLineaLN().consultarLineasBanco(banco);
        }
    }
}
