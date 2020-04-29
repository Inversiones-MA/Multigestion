using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MultiComercial
{
    [DataContract(Name = "TipoBien")]
    public class TipoBien
    {
        public double TotalSeguro { get; set; }

        [DataMember(Name = "Nombre")]
        public string Nombre { get; set; }

        [DataMember(Name = "Id")]
        public int Id { get; set; }

        public TipoBien()
        {

        }
    }
}
