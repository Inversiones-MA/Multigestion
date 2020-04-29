using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    public class SeguroAval
    {
        public int IdAvalCotizacion { get; set; }

        public Nullable<int> IdCotizacion { get; set; }

        public Nullable<double> ValorAsegurable { get; set; }

        public Nullable<double> PeriodoAnual { get; set; }

        public Nullable<int> NumAvales { get; set; }

        public SeguroAval()
        {

        }
    }
}
