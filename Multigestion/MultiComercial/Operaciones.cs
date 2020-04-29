using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiComercial
{
    public class Operaciones
    {
        public Operaciones()
        {
            this._idEtapa = 1;
            this._descEtapa = "Prospecto";
            this._idSubEtapa = 1;
            this._descSubEtapa = "No Contactado";
            this._idOperacion = 0;
            this._validacion = 0;
            this._habilitado = true;
            this._periodo = 0;
            this._idTipoCta = 0;
            this._descTipoCta = "";
            this._idTipoAmort = 0;
            this._descTipoAmort = "";
            this._nroCta = 0;
            this._comision = 0;
            this._comisionCLP = 0;
            this._tieneComis = false;
            this._fecEmis = new DateTime(1900, 1, 1);
            this._fechaEstCierre = null;
        }

        public int _idOperacion { get; set; }
        public int _idEmpresa { get; set; }
        public int _idProducto { get; set; }
        public string _descProducto { get; set; }
        public int _idEtapa { get; set; }
        public string _descEtapa { get; set; }
        public int _idSubEtapa { get; set; }
        public string _descSubEtapa { get; set; }
        public int _idFinalidad { get; set; }
        public string _finalidad { get; set; }
        public double _montoOper { get; set; }
        public string _plazo { get; set; }
        public int _idMoneda { get; set; }
        public string _descMoneda { get; set; }
        public int _idCanal { get; set; }
        public string _descCanal { get; set; }
        public int _validacion { get; set; }
        public int _idEstado { get; set; }
        public string _descEstado { get; set; }
        public string _descArea { get; set; }
        public bool _habilitado { get; set; }
        public Nullable<DateTime> _fecEmis { get; set; }
        public int _periodo { get; set; }
        public int _idTipoCta { get; set; }
        public string _descTipoCta { get; set; }
        public int _idTipoAmort { get; set; }
        public string _descTipoAmort { get; set; }
        public int _nroCta { get; set; }
        public decimal _comision { get; set; }
        public decimal _comisionCLP { get; set; }
        public bool _tieneComis { get; set; }
        public int _idEjecutivo { get; set; }
        public string _descEjecutivo { get; set; }
        public int _idSH { get; set; }
        public string _tipoCredito { get; set; }
        public string _horizonte { get; set; }
        public string _montoCuoton { get; set; }
        public string _tieneFogape { get; set; }
        public string _estadoCertificado { get; set; }
        public Nullable<DateTime> _fechaRecuperacion { get; set; }
        public string _montoRecuperoFogape { get; set; }
        public string _motivoGarantiaFogape { get; set; }
        public string _codigoSafio { get; set; }
        public string _garantia { get; set; }
        public string _coberturaFogape { get; set; }
        public string _garantiaFogapeVigente { get; set; }
        public string _descFondo { get; set; }
        public string _incluyeSafio { get; set; }
        public string _cobertura { get; set; }
        public Nullable<decimal> _ComisionFogape { get; set; }
        public Nullable<DateTime> _fechaEstCierre { get; set; }
    }
}
