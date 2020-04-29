using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiComercial
{
    class Constantes
    {
        public class OPCION
        {
            public const string INSERTAR = "01";
            public const string MODIFICAR = "02";
            public const string ELIMINAR = "03";
            public const string LISTAR = "04";
            public const string RECUPERAR = "05";
            public const int RECUPERA = 5;
            public const string LISTAGENERAL = "01";
            public const string BUSCAxRUT = "06";

            public const string LISTARCONTACTOS = "01";
            public const string ELIMINARCONTACTO = "02";
            public const string ACTUALIZARCONTACTO = "03";
            public const string INSERTARCONTACTO = "04";
            public const string INSERTARPAF = "1";
            public const string PAFFINAL = "2";
        }

        public class PERMISOS
        {
            //public const int ADMINISTRADOR = 3;
            public const int SOLOLECTURA = 2;
        }

        public class MENSAJE
        {
            //public const int EXITOINSERT = 4;
            //public const int EXITOMODIF = 6;
            //public const int EXITOELIM = 8;
            public const int ERRORGENERAL = 5;
            public static int EXITOINSET { get; set; }
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

            ////Directorio
            //public const int REQUERIDODATOSDIRECTORIO = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";
            //public const int EXITOINSETDIRECTORIO = 4; //"public const int EXITOINSETEMPRESARELAC = 4; //"EXITOINSETEMPRESARELAC";";
            //public const int ERRORINSERTDIRECTORIO = 5; //"EXITOINSETEMPRESARELAC";
            //public const int EXITOMODIFDIRECTORIO = 6;
            //public const int ERRORMODIFDIRECTORIO = 5;

            //public const int EXITOELIMEMDIRECTORIO = 8;
            //public const int ERRORELIMEMDIRECTORIO = 5;

            ////SOCIOS
            //public const int REQUERIDODATOSSOCIOS = 3; //"Para pasar a la etapa de riesgo, debera ingresar los datos de la empresa";
            //public const int EXITOINSETSOCIOS = 4; //"public const int EXITOINSETEMPRESARELAC = 4; //"EXITOINSETEMPRESARELAC";";
            //public const int ERRORINSERTSOCIOS = 5; //"EXITOINSETEMPRESARELAC";

            //public const int EXITOMODIFSOCIOS = 6;
            //public const int ERRORMODIFSOCIOS = 5;

            public const int EXITOELIMEMSOCIOS = 8;
            //public const int ERRORELIMEMSOCIOS = 5;

            //public const int GARANTIACREADA = 4;
            //public const int GARANTIAACTUALIZADA = 6;
            //public const int GARANTIAELIMINADA = 8;
            public const int EMPRESAERRORUSUARIO = 5;
            public const int EMPRESAERRORGUARDAR = 5;

            ////BALANCE
            //public const int REQUERIDODATOSDOCCONTABLE = 3;

            //public const int EXITOINSETDOCCONTABLE = 4;
            //public const int ERRORINSERTDOCCONTABLE = 5;

            public const int EXITOMODIFDOCCONTABLE = 6;
            public const int ERRORMODIFDOCCONTABLE = 5;

            //public const int REGISTOSINCOMPLETOSDOCCONTABLE = 22;

            public const int ENVIARSINPERMISO = 38;
            public const int CAMBIARETAPA = 39;
            public const int CAMBIARESTADO = 40;
        }

        public class LISTAETAPA
        {
            public const string ETAPAS = "Etapas";
            public const string VISTA = "All Items";
        }
        public class LISTASUBETAPA
        {
            public const string SUBETAPA = "SubEtapa";
            //public const string VISTA = "All Items";
        }
        public class LISTAESTADO
        {
            public const string ESTADO = "Estados";
            public const string VISTA = "All Items";
        }
       
    }
}
