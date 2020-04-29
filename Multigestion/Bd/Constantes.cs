using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bd
{
    public class Constantes
    {
        public class OPCION
        {
            public const string INSERTAR = "01";
            public const string MODIFICAR = "02";
            public const string ELIMINAR = "03";
            public const string LISTAR = "04";
            public const string LISTAGENERAL = "01";
            public const string BUSCAxRUT = "06";

            public const string RECUPERAR = "05";
            public const int RECUPERA = 5;

            public const string LISTARCONTACTOS = "01";
            public const string ELIMINARCONTACTO = "02";
            public const string ACTUALIZARCONTACTO = "03";
            public const string INSERTARCONTACTO = "04";
            public const string INSERTARPAF = "1";
            public const string PAFFINAL = "2";
        }

        public class PERMISOS
        {
            public const int SOLOLECTURA = 2;
        }

        public class MENSAJE
        {
            public static int EXITOINSET { get; set; }
            public const int ERRORGENERAL = 5;
            public const int EXITOINSERT = 4;
            public const int FISCINCOMPLETOS = 43;
        }


        public class MENSAJES
        {
            public const int ACCESOLIMITADO = 2;//"Usuario Limitado - Solo Lectura";

            ////EMPRESA RELACIONADA
            public const int REQUERIDODATOSEMPRESA = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";

            public const int EXITOINSETEMPRESARELAC = 4; //"public const int EXITOINSETEMPRESARELAC = 4; //"EXITOINSETEMPRESARELAC";";
            public const int ERRORINSERTEMPRESARELAC = 5; //"EXITOINSETEMPRESARELAC";
            public const int EXITOMODIFEMPRESARELAC = 6;
            public const int ERRORMODIFEMPRESARELAC = 7;
            public const int EXITOELIMEMPRESARELAC = 8;
            public const int ERRORELIMEMPRESARELAC = 9;
            public const int CONTACTOYAREGISTRADO = 23;
            public const int ULTIMOCONTACTO = 24;
            public const int SINPRINCIPAL = 25;

            ////HISTORICO EMPRESA
            //public const int REQUERIDODATOSEMPRESAHIS = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";
            public const int EXITOINSETEMPRESACHIS = 4; // inserción exitosa
            public const int ERRORINSERTEMPRESACHIS = 5; // error al insertar, intente de nuevo
            public const int EXITOMODIFEMPRESACHIS = 6;// Modificacion exitosa
            public const int ERRORMODIFEMPRESACHIS = 5;// error al Modificar, intente de nuevo

            public const int EXITOELIMEMSOCIOS = 8;
            public const int EMPRESAERRORUSUARIO = 5;
            public const int EMPRESAERRORGUARDAR = 5;

            ////BALANCE
            public const int EXITOMODIFDOCCONTABLE = 6;
            public const int ERRORMODIFDOCCONTABLE = 5;
            public const int ENVIARSINPERMISO = 38;
            public const int CAMBIARETAPA = 39;
            public const int CAMBIARESTADO = 40;

            public const int EXITOINSETDOCCONTABLE = 4;
            public const int ERRORINSERTDOCCONTABLE = 5;
            public const int REGISTOSINCOMPLETOSDOCCONTABLE = 22;
        }

        public class LISTAETAPA
        {
            public const string ETAPAS = "Etapas";
            public const string VISTA = "All Items";
        }
        public class LISTASUBETAPA
        {
            public const string SUBETAPA = "SubEtapa";
        }
        public class LISTAESTADO
        {
            public const string ESTADO = "Estados";
            public const string VISTA = "All Items";
        }

        public class DOCUMENTOSCONTABLE
        {
            public const int BALANCEGENERAL = 1;
            public const int ESTADORESULTADOS = 2;
            public const int IVAVENTAS = 3;
            public const int IVACOMPRAS = 4;
        }

        public class LISTATIPODOCUMET
        {
            public const string TIPODOCUMENT = "TipoDocumentoContable";
            public const string VISTA = "All Items";
        }

        public class LISTAINDICADORES
        {
            public const string INDICADORES = "Indicadores";
            public const string ACTIVIDAD = "ActividadEconomica";
            //    public const string VISTAALL = "All Items";

            public const string VISTACOMPORTAMIENTO = "Comportamiento";
            public const string VISTAMONTO = "Monto";

            public const string VISTAANTIGUEDAD = "Antiguedad";
            public const string VISTACONCENTRACION = "Concentracion";
            public const string VISTACOMPETENCIA = "Competencia";
            public const string VISTACALIDAD = "Calidad";
            public const string VISTALIQUIDEZ = "Liquidez";
            public const string VISTAEBITDA = "Ebitda/GtosFinancieros";
            public const string VISTALEVERAGE = "Leverage";
            public const string VISTAPASIVOE = "PasivoExigible/Ebitda";
            public const string VISTAPASIOC = "PasivoCirculante/VtasProm";
            public const string VISTATIPOGARANTIA = "TipoGarantia";
            public const string VISTACALIDADGARANTIA = "CalidadGarantia";
            public const string VISTACOBERTURAGARANTIA = "CoberturaGarantia";
            public const string VISTASECTOR = "IndSector";
        }

        public class OPCIONIMPRESIONPAF
        {
            public const string INSERTAR = "01";
            public const string CONSULTAR = "02";
            public const string IMPRIMIR = "05";
            public const string EDOPENDIENTE = "PENDIENTE";
            //    public const string EDOCALCULADO = "CALCULADO";
            public const string EDOIMPRESO = "IMPRESO";
        }

        public class RIESGO
        {
            public const int VALIDACION = 2;
            public const int VALIDACIONINSERT = 1;
        }
    }
}
