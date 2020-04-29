using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento
{
    class LNplantilla
    {
        private Nullable<int> _IdPlantillaDocumento;
        private Nullable<int> _IdTipoProducto;
        private Nullable<int> _IdDocumentoTipo;
        private Nullable<int> _IdAcreedor;
        private string _NombrePlantilla;
        private Nullable<Boolean> _Vigente;
        private string _ContenidoHtml;
        private Nullable<Boolean> _EsGenerica;
        private string _UsuarioModificacion;
        private Nullable<DateTime> _FechaIngreso;
        private Nullable<DateTime> _FechaModificacion;

        public Nullable<int> IdPlantillaDocumento { get { return _IdPlantillaDocumento; } set { _IdPlantillaDocumento = value; } }
        public Nullable<int> IdTipoProducto { get { return _IdTipoProducto; } set { _IdTipoProducto = value; } }
        public Nullable<int> IdDocumentoTipo { get { return _IdDocumentoTipo; } set { _IdDocumentoTipo = value; } }
        public Nullable<int> IdAcreedor { get { return _IdAcreedor; } set { _IdAcreedor = value; } }
        public string NombrePlantilla { get { return _NombrePlantilla; } set { _NombrePlantilla = value; } }
        public Nullable<Boolean> Vigente { get { return _Vigente; } set { _Vigente = value; } }
        public string ContenidoHtml { get { return _ContenidoHtml; } set { _ContenidoHtml = value; } }
        public Nullable<Boolean> EsGenerica { get { return _EsGenerica; } set { _EsGenerica = value; } }
        public string UsuarioModificacion { get { return _UsuarioModificacion; } set { _UsuarioModificacion = value; } }
        public Nullable<DateTime> FechaIngreso { get { return _FechaIngreso; } set { _FechaIngreso = value; } }
        public Nullable<DateTime> FechaModificacion { get { return _FechaModificacion; } set { _FechaModificacion = value; } }
    }
}
