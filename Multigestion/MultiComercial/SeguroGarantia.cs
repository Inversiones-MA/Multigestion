using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MultiComercial
{
    public class SeguroGarantia
    {
        public int IdSeguro { get; set; }

        public Nullable<int> IdTipoBien { get; set; }

        public string Nombre { get; set; }

        public Nullable<double> ValorAsegurable { get; set; }

        public Nullable<double> PeriodoAnual { get; set; }

        public Nullable<double> TotalSeguros { get; set; }

        public int IdTemporal { get; set; }
        public Nullable<int> IdCotizacion { get; set; }

        public SeguroGarantia()
        {

        }
    }
}
