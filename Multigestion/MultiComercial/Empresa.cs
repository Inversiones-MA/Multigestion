using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiComercial
{
   public class Empresa
    {
        public Empresa()
        {
            this._nombreFantasia = string.Empty;
            this._giro = string.Empty;
            this._telefono1 = "";
            this._telefono2 = "";
            this._fax1 = 0;
            this._fax2 = 0;
            this._email = string.Empty;
            this._numEmpleados = 0;
            //this._fecIniContrato;
            //this._fecFinContrato; ;
            this._tipoEmpresa = string.Empty;
            this._actividad = string.Empty;
            this._fecIniAct = Convert.ToDateTime("01/01/1900");
            this._descEjecutivo = string.Empty;
            this._idActividad = 0;
            this._idTipoEmpresa = 0;
            this._validacion = 0;
            this._habEmpresa = true;
            this._habDirEmpresa = true;
            //this._idRegion = 0;
            //this._descRegion = string.Empty;
            //this._idProvincia = 0;
            //this._descProv = string.Empty;
            //this._idComuna = 0;
            //this._descComuna = string.Empty;
            //this._calle = string.Empty;
            //this._numero = string.Empty;
            this._nombreEmpresa = string.Empty;
            this._bloqueado = 0;
            this._idMotivoBloqueo = 0;
            this._descMotivoBloqueo = "";

            this._PerteneceGrupo = 0;
            this._idGrupoEconomico = 0;
            this._descGrupoEconomico = "";
            this._ObjetoSII = "";
            //03-03-2017
            //this._NumeroCasa = string.Empty;
            //this._ComplementoDireccion = string.Empty;
        }

        public long _idEmpresa { get; set; }
        public long _idEjecutivo { get; set; }
        public decimal _rut { get; set; }
        public string _dvRut { get; set; }
        public string _razonSocial { get; set; }
        public string _nombreFantasia { get; set; }
        public string _giro { get; set; }
        public string _telefono1 { get; set; }
        public string _telefono2 { get; set; }
        public decimal _fax1 { get; set; }
        public decimal _fax2 { get; set; }
        public string _email { get; set; }
        public long _numEmpleados { get; set; }
        public DateTime? _fecIniContrato { get; set; }
        public DateTime? _fecFinContrato { get; set; }
        public string _tipoEmpresa { get; set; }
        public string _actividad { get; set; }
        public DateTime _fecIniAct { get; set; }
        public string _descEjecutivo { get; set; }
        public long _idActividad { get; set; }
        public long _idTipoEmpresa { get; set; }
        public long _validacion { get; set; }
        public bool _habEmpresa { get; set; }
        public bool _habDirEmpresa { get; set; }
        //public long _idRegion { get; set; }
        //public long _idProvincia { get; set; }
        //public long _idComuna { get; set; }
        //public string _descRegion { get; set; }
        //public string _descProv { get; set; }
        //public string _descComuna { get; set; }
        //public string _calle { get; set; }
        //public string _numero { get; set; }
        public string _nombreEmpresa { get; set; }
        public int _bloqueado { get; set; }

        public int _idMotivoBloqueo { get; set; }
        public string _descMotivoBloqueo { get; set; }

        public int _PerteneceGrupo { get; set; }

        public int _idGrupoEconomico { get; set; }
        public string _descGrupoEconomico { get; set; }
        
        //02-03-2017
        public string _ClasificacionSBIF { get; set; }
        public string _ClasificacionPAF { get; set; }

        public string _ObjetoSII { get; set; }

       ////03-03-2017
       // public string _NumeroCasa { get; set; }
       // public string _ComplementoDireccion { get; set; }
    }
}
