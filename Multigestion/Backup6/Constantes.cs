using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiRiesgo
{
    class Constantes
    {

        //public class CAMBIOESTADO
        //{
        //    public const int CAMBIOETAPA = 1;
        //    public const int CAMBIOSUBETAPA = 2;

        //    public const int PROSPECTOSINCONTAC = 1;
        //    public const int PROSPECTOCONTAC = 2;
        //    public const int NEGOCIACIONANTEVAL = 3;
        //    public const int NEGOCIACIONCAD = 4;
        //    public const int NEGOCIACIONRECH = 5;
        //    public const int RIESGOPROC = 6;
        //    public const int RIESGORECH = 7;
        //    public const int RIESGOAPRO = 8;
        //    public const int RIESGOAPROREP = 9;
        //    public const int COMITEPROC = 10;
        //    public const int COMITEAPRO = 11;
        //    public const int COMITERECH = 12;
        //}

        //public class ESTADO
        //{
        //    public const int PROSPECTO = 1;
        //    public const string DPROSPECTO = "Prospecto";
        //    public const int NEGOCIACION = 3;
        //    public const string DNEGOCIACION = "Negociación";
        //    public const int RIESGO = 4;
        //    public const string DRIESGO = "Riesgo";
        //    public const int COMITE = 6;
        //    public const string DCOMITE = "Comite";
        //}

        //public class SUBESTADO
        //{
        //    public const int SINCONTACTAR = 1;
        //    public const string DSINCONTACTAR = "Sin Contactar";
        //    public const int CONTACTADO = 3;
        //    public const string DCONTACTADO = "Contactado";
        //    public const int ANTECEDENTESEVALUACION = 5;
        //    public const string DANTECEDENTESEVALUACION = "Antecedentes y Evaluación";
        //    public const int CADUCADO = 6;
        //    public const string DCADUCADO = "Caducado";
        //    public const int RECHAZADO = 3;
        //    public const string DRECHAZADO = "Rechazado";
        //    public const int PROCESANDO = 8;
        //    public const string Procesando = "Procesando";
        //    public const int APROBADO = 10;
        //    public const string DAPROBADO = "Aprobado";
        //    public const int APROVADOREPAROS = 11;
        //    public const string DAPROVADOREPAROS = "Aprovado con Reparos";
        //}

        public class OPCION
        {
            public const string INSERTAR = "01";
            public const string MODIFICAR = "02";
        //    public const string ELIMINAR = "03";
            public const string LISTAR = "04";
        //    public const string RECUPERAR = "05";
        //    public const int RECUPERA = 5;
            public const string LISTAGENERAL = "01";

        //    public const int ACTUALIZAR = 5;

        //    public const string LISTARCONTACTOS = "01";
        //    public const string ELIMINARCONTACTO = "02";
        //    public const string ACTUALIZARCONTACTO = "03";
        //    public const string INSERTARCONTACTO = "04";
            public const string INSERTARPAF = "1";
            public const string PAFFINAL = "2";
        }

        //public class OPCIONOPERACION
        //{
        //    public const string COMERCIAL = "01";
        //    public const string RIESGO = "02";
        //    public const string CMEROPERACIONES = "03";
        //    public const string OPERACIONES = "04";
        //}

        //public class PERMISOS
        //{
        //    public const int ADMINISTRADOR = 3;
        //    public const int SOLOLECTURA = 2;
        //}

        //public class MENSAJE
        //{
        //    public const int EXITOINSERT = 4;
        //    public const int EXITOMODIF = 6;
        //    public const int EXITOELIM = 8;
        //    public const int ERRORGENERAL = 5;

        //    public static int EXITOINSET { get; set; }
        //}

        public class DOCUMENTOSCONTABLE
        {
            public const int BALANCEGENERAL = 1;
            public const int ESTADORESULTADOS = 2;
            public const int IVAVENTAS = 3;
            public const int IVACOMPRAS = 4;
        }

        public class RIESGO
        {
            public const int VALIDACION = 2;
            public const int VALIDACIONINSERT = 1;
        }

        public class MENSAJES
        {
        //    public const int ACCESOLIMITADO = 2;//"Usuario Limitado - Solo Lectura";

        //    //EMPRESA RELACIONADA
        //    public const int REQUERIDODATOSEMPRESA = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";

        //    public const int EXITOINSETEMPRESARELAC = 4; //"public const int EXITOINSETEMPRESARELAC = 4; //"EXITOINSETEMPRESARELAC";";
        //    public const int ERRORINSERTEMPRESARELAC = 5; //"EXITOINSETEMPRESARELAC";

        //    public const int EXITOMODIFEMPRESARELAC = 6;
        //    public const int ERRORMODIFEMPRESARELAC = 5;

        //    public const int EXITOELIMEMPRESARELAC = 8;
        //    public const int ERRORELIMEMPRESARELAC = 5;

        //    //HISTORICO EMPRESA

        //    public const int REQUERIDODATOSEMPRESAHIS = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";

        //    public const int EXITOINSETEMPRESACHIS = 4; // inserción exitosa
        //    public const int ERRORINSERTEMPRESACHIS = 5; // error al insertar, intente de nuevo

        //    public const int EXITOMODIFEMPRESACHIS = 6;// Modificacion exitosa
        //    public const int ERRORMODIFEMPRESACHIS = 5;// error al Modificar, intente de nuevo

        //    //Directorio
        //    public const int REQUERIDODATOSDIRECTORIO = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";

        //    public const int EXITOINSETDIRECTORIO = 4; //"public const int EXITOINSETEMPRESARELAC = 4; //"EXITOINSETEMPRESARELAC";";
        //    public const int ERRORINSERTDIRECTORIO = 5; //"EXITOINSETEMPRESARELAC";

        //    public const int EXITOMODIFDIRECTORIO = 6;
        //    public const int ERRORMODIFDIRECTORIO = 5;

        //    public const int EXITOELIMEMDIRECTORIO = 8;
        //    public const int ERRORELIMEMDIRECTORIO = 5;


        //    //SOCIOS
        //    public const int REQUERIDODATOSSOCIOS = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";

        //    public const int EXITOINSETSOCIOS = 4; //"public const int EXITOINSETEMPRESARELAC = 4; //"EXITOINSETEMPRESARELAC";";
        //    public const int ERRORINSERTSOCIOS = 5; //"EXITOINSETEMPRESARELAC";

        //    public const int EXITOMODIFSOCIOS = 6;
        //    public const int ERRORMODIFSOCIOS = 5;

        //    public const int EXITOELIMEMSOCIOS = 8;
        //    public const int ERRORELIMEMSOCIOS = 5;

        //    //BALANCE
        //    public const int REQUERIDODATOSDOCCONTABLE = 3;

            public const int EXITOINSETDOCCONTABLE = 4;
            public const int ERRORINSERTDOCCONTABLE = 5;

            public const int EXITOMODIFDOCCONTABLE = 6;
            public const int ERRORMODIFDOCCONTABLE = 5;

            public const int REGISTOSINCOMPLETOSDOCCONTABLE = 22;
        //    public const int CONTACTOYAREGISTRADO = 23;

        //    public const int ENVIARSINPERMISO = 38;
        //    public const int CAMBIARETAPA = 39;
        //    public const int CAMBIARESTADO = 40;
        }

        //class LINK
        //{
        //    public const string REDIRECCION = "_EmpresasDetalle.aspx";
        //}

        public class LISTAETAPA
        {
            public const string ETAPAS = "Etapas";
        //    public const string VISTA = "All Items";
        }

        public class LISTASUBETAPA
        {
            public const string SUBETAPA = "SubEtapa";
        //    public const string VISTA = "All Items";
        }

        public class LISTAESTADO
        {
            public const string ESTADO = "Estados";
            public const string VISTA = "All Items";
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

    }
}
