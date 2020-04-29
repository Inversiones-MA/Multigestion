using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultigestionUtilidades
{
    public class Enumeradores
    {
        public enum ETemplateCorreos
        {
            [Description("multigestion_cambioEtapa")]
            MultigestionCambioEtapa,
            [Description("cftonline_postulacion")]
            CftOnlinePostulacion,
        }
    }
}
