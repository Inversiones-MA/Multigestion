using Bd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    public class SubEtapas
    {
        #region > Propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdEtapa { get; set; }
        public int Orden { get; set; }
        public bool? Rechazo { get; set; }
        public int IdOpcion { get; set; }
        #endregion

        public DataTable ListarSubEtapas(SubEtapas eta)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            return Ln.ListarSubEtapas(eta.IdEtapa, eta.Rechazo, eta.IdOpcion);
        }
    }
}
