using Bd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    public class Etapas
    {
        #region > Propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Orden { get; set; }
        #endregion


        public DataTable ListarEtapas(Etapas eta)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            return Ln.ListarEtapas(eta.Id);    
        }
    }
}
