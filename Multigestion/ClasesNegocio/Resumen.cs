using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    [Serializable]
    public class Resumen
    {
        public string area { get; set; }
        public string descCargo { get; set; }
        public string descEjecutivo { get; set; }
        public string desEmpresa { get; set; }
        public string desEstado { get; set; }
        public string desEtapa { get; set; }
        public string desOperacion { get; set; }
        public string desPermiso { get; set; }
        public string desSubEtapa { get; set; }
        public DateTime? fecInicioActEconomica { get; set; }
        public string idCargo { get; set; }
        public int idEmpresa { get; set; }
        public int idEstado { get; set; }
        public int idEtapa { get; set; }
        public int idOperacion { get; set; }
        public int idPAF { get; set; }
        public string idPermiso { get; set; }
        public int idSubEtapa { get; set; }
        public string idUsuario { get; set; }
        public string linkActual { get; set; }
        public string linkError { get; set; }
        public string linkPrincial { get; set; }
        public string rut { get; set; }
        public string DescEstadoFiscalia { get; set; }
        public int IdEstadoFiscalia { get; set; }
        public int IdAcreedor { get; set; }
        public int IdCotizacion { get; set; }
        public int OrdenSubEtapaLegal { get; set; }
        public Resumen()
        {

        }
    }
}
