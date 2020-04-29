using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFiscalia
{
    class Constantes
    {
        public class CAMBIOESTADO
        {
            public const int CAMBIOETAPA = 1;
            public const int CAMBIOSUBETAPA = 2;
            public const int PROSPECTOSINCONTAC = 1;
            public const int PROSPECTOCONTAC = 2;
            public const int NEGOCIACIONANTEVAL = 3;
            public const int NEGOCIACIONCAD = 4;
            public const int NEGOCIACIONRECH = 5;
            public const int RIESGOPROC = 6;
            public const int RIESGORECH = 7;
            public const int RIESGOAPRO = 8;
            public const int RIESGOAPROREP = 9;
            public const int COMITEPROC = 10;
            public const int COMITEAPRO = 11;
            public const int COMITERECH = 12;
        }

        public class ESTADO
        {
            public const int PROSPECTO = 1;
            public const string DPROSPECTO = "Prospecto";
            public const int NEGOCIACION = 3;
            public const string DNEGOCIACION = "Negociación";
            public const int RIESGO = 4;
            public const string DRIESGO = "Riesgo";
            public const int COMITE = 6;
            public const string DCOMITE = "Comite";
        }

        public class SUBESTADO
        {
            public const int SINCONTACTAR = 1;
            public const string DSINCONTACTAR = "Sin Contactar";
            public const int CONTACTADO = 3;
            public const string DCONTACTADO = "Contactado";
            public const int ANTECEDENTESEVALUACION = 5;
            public const string DANTECEDENTESEVALUACION = "Antecedentes y Evaluación";
            public const int CADUCADO = 6;
            public const string DCADUCADO = "Caducado";
            public const int RECHAZADO = 3;
            public const string DRECHAZADO = "Rechazado";
            public const int PROCESANDO = 8;
            public const string Procesando = "Procesando";
            public const int APROBADO = 10;
            public const string DAPROBADO = "Aprobado";
            public const int APROVADOREPAROS = 11;
            public const string DAPROVADOREPAROS = "Aprovado con Reparos";
        }

        public class OPCION
        {
            public const string INSERTAR = "01";
            public const string LISTAGENERAL = "01";
        }

        public class PERMISOS
        {
            public const int SOLOLECTURA = 2;
        }

        public class MENSAJE
        {
            public const int EXITOINSERT = 4;
            public const int ERRORGENERAL = 5;
            public const int FISCINCOMPLETOS = 43;
            public static int EXITOINSET { get; set; }
        }

        public class MENSAJES
        {
            public const int FISCALIASOLICITUD = 42;
        }

    }
}
