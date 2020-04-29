using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultigestionUtilidades
{
    [Serializable]
    public class Busqueda
    {
        public string Acreedor { get; set; }
        public string descEstado { get; set; }
        public string EdoCertificado { get; set; }
        public string Etapa { get; set; }
        public string FechaFin { get; set; }
        public string FechaInicio { get; set; }
        public int idAcreedor { get; set; }
        public int idEdoCertificado { get; set; }
        public int idEstado { get; set; }
        public int idEtapa { get; set; }
        public int idSubEtapa { get; set; }
        public int idTipoOperacion { get; set; }
        public string nCertificado { get; set; }
        public string Otro { get; set; }
        public bool Pagados { get; set; }
        public bool Pendientes { get; set; }
        public bool Proximos { get; set; }
        public string RazonSocial { get; set; }
        public string Rut { get; set; }
        public string SubEtapa { get; set; }
        public string TipoOperacion { get; set; }
        public bool Todos { get; set; }
        public bool Vencidos { get; set; }
    }
}
