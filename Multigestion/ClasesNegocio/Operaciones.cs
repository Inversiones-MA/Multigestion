using Bd;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
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

        public string ValidarIngresoOperacion(Operaciones datosOperaciones)
        {
            string mensaje = string.Empty;

            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[7];
                sqlParam[0] = new SqlParameter("@descProducto", datosOperaciones._descProducto);
                sqlParam[1] = new SqlParameter("@DescMoneda", datosOperaciones._descMoneda);
                sqlParam[2] = new SqlParameter("@finalidad", datosOperaciones._finalidad);
                sqlParam[3] = new SqlParameter("@plazo", datosOperaciones._plazo);
                sqlParam[4] = new SqlParameter("@montoOper", datosOperaciones._montoOper);
                sqlParam[5] = new SqlParameter("@fecEmision", datosOperaciones._fecEmis);
                sqlParam[6] = new SqlParameter("@opcion", 1);

                return AD_Global.traerPrimeraColumna("sp_validacionesOperacion", sqlParam);
            }
            catch
            {
                return mensaje = "";
            }
        }


        public string IngresaActualizaOperaciones(Operaciones datosOperaciones, string accion, string usuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[36];
                sqlParam[0] = new SqlParameter("@idOperacion", datosOperaciones._idOperacion);
                sqlParam[1] = new SqlParameter("@idEmpresa", datosOperaciones._idEmpresa);
                sqlParam[2] = new SqlParameter("@descProducto", datosOperaciones._descProducto);
                sqlParam[3] = new SqlParameter("@descEtapa", datosOperaciones._descEtapa);
                sqlParam[4] = new SqlParameter("@descSubEtapa", datosOperaciones._descSubEtapa);
                sqlParam[5] = new SqlParameter("@finalidad", datosOperaciones._finalidad);
                sqlParam[6] = new SqlParameter("@montoOper", datosOperaciones._montoOper);
                sqlParam[7] = new SqlParameter("@plazo", datosOperaciones._plazo);
                sqlParam[8] = new SqlParameter("@idMoneda", datosOperaciones._idMoneda);
                sqlParam[9] = new SqlParameter("@idCanal", datosOperaciones._idCanal);
                sqlParam[10] = new SqlParameter("@DescMoneda", datosOperaciones._descMoneda);
                sqlParam[11] = new SqlParameter("@DescCanal", datosOperaciones._descCanal);
                sqlParam[12] = new SqlParameter("@idProducto", datosOperaciones._idProducto);
                sqlParam[13] = new SqlParameter("@idEtapa", datosOperaciones._idEtapa);
                sqlParam[14] = new SqlParameter("@idSubEtapa", datosOperaciones._idSubEtapa);
                sqlParam[15] = new SqlParameter("@idFinalidad", datosOperaciones._idFinalidad);
                sqlParam[16] = new SqlParameter("@accion", accion);
                sqlParam[17] = new SqlParameter("@usuario", usuario);
                sqlParam[18] = new SqlParameter("@validacion", datosOperaciones._validacion);
                sqlParam[19] = new SqlParameter("@idEstado", datosOperaciones._idEstado);
                sqlParam[20] = new SqlParameter("@descEstado", datosOperaciones._descEstado);
                sqlParam[21] = new SqlParameter("@descArea", datosOperaciones._descArea);
                sqlParam[22] = new SqlParameter("@habilitado", datosOperaciones._habilitado);
                sqlParam[23] = new SqlParameter("@fecEmision", datosOperaciones._fecEmis);
                sqlParam[24] = new SqlParameter("@idEjecutivo", datosOperaciones._idEjecutivo);
                sqlParam[25] = new SqlParameter("@descEjecutivo", datosOperaciones._descEjecutivo);
                sqlParam[26] = new SqlParameter("@periodo", datosOperaciones._periodo);
                sqlParam[27] = new SqlParameter("@idTipoCta", datosOperaciones._idTipoCta);
                sqlParam[28] = new SqlParameter("@descTipoCta", datosOperaciones._descTipoCta);
                sqlParam[29] = new SqlParameter("@idTipoAmort", datosOperaciones._idTipoAmort);
                sqlParam[30] = new SqlParameter("@descTipoAmort", datosOperaciones._descTipoAmort);
                sqlParam[31] = new SqlParameter("@numCta", datosOperaciones._nroCta);
                sqlParam[32] = new SqlParameter("@porcComision", datosOperaciones._comision);
                sqlParam[33] = new SqlParameter("@comisionCLP", datosOperaciones._comisionCLP);
                sqlParam[34] = new SqlParameter("@incluyeComis", datosOperaciones._tieneComis);
                sqlParam[35] = new SqlParameter("@idSP", datosOperaciones._idSH);

                return AD_Global.traerPrimeraColumna("InsertaActualizaOperaciones", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public string ValidarReglasNegocioOperacion(Operaciones datosOperaciones, string uf)
        {
            string mensaje = string.Empty;

            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[29];
                sqlParam[0] = new SqlParameter("@descProducto", datosOperaciones._descProducto);
                sqlParam[1] = new SqlParameter("@descEtapa", datosOperaciones._descEtapa);
                sqlParam[2] = new SqlParameter("@descSubEtapa", datosOperaciones._descSubEtapa);
                sqlParam[3] = new SqlParameter("@tipoCredito", datosOperaciones._tipoCredito);
                sqlParam[4] = new SqlParameter("@horizonte", datosOperaciones._horizonte);
                sqlParam[5] = new SqlParameter("@montoCuoton", datosOperaciones._montoCuoton);
                sqlParam[6] = new SqlParameter("@tieneFogape", datosOperaciones._tieneFogape);
                sqlParam[7] = new SqlParameter("@codigoSafio", datosOperaciones._codigoSafio);
                //sqlParam[8] = new SqlParameter("@garantia", datosOperaciones._garantia);
                sqlParam[8] = new SqlParameter("@estadoCertificado", datosOperaciones._estadoCertificado);
                sqlParam[9] = new SqlParameter("@fechaRecuperacion", datosOperaciones._fechaRecuperacion);
                sqlParam[10] = new SqlParameter("@montoRecuperoFogape", datosOperaciones._montoRecuperoFogape);
                sqlParam[11] = new SqlParameter("@motivoGarantiaFogape", datosOperaciones._motivoGarantiaFogape);
                sqlParam[12] = new SqlParameter("@coberturaFogape", datosOperaciones._coberturaFogape);
                sqlParam[13] = new SqlParameter("@garantiaFogapeVigente", datosOperaciones._garantiaFogapeVigente);
                sqlParam[14] = new SqlParameter("@descFondo", datosOperaciones._descFondo);
                sqlParam[15] = new SqlParameter("@idEmpresa", datosOperaciones._idEmpresa);
                sqlParam[16] = new SqlParameter("@idOperacion", datosOperaciones._idOperacion);
                sqlParam[17] = new SqlParameter("@comision", datosOperaciones._comision);
                sqlParam[18] = new SqlParameter("@descFinalidad", datosOperaciones._finalidad);
                sqlParam[19] = new SqlParameter("@totalPlazo", datosOperaciones._plazo);
                sqlParam[20] = new SqlParameter("@fechaVencimientoCredito", datosOperaciones._fecEmis);
                sqlParam[21] = new SqlParameter("@montoOperacion", datosOperaciones._montoOper);
                sqlParam[22] = new SqlParameter("@incluyeSafio", datosOperaciones._incluyeSafio);
                sqlParam[23] = new SqlParameter("@cobertura", datosOperaciones._cobertura);
                sqlParam[24] = new SqlParameter("@DescTipoMoneda", datosOperaciones._descMoneda);
                sqlParam[25] = new SqlParameter("@ComisionFogape", datosOperaciones._ComisionFogape);
                sqlParam[26] = new SqlParameter("@uf", uf);
                sqlParam[27] = new SqlParameter("@fechaEstCierre", datosOperaciones._fechaEstCierre);
                sqlParam[28] = new SqlParameter("@opcion", 2);

                return AD_Global.traerPrimeraColumna("sp_reglaNegocioOperacion", sqlParam);
            }
            catch
            {
                return mensaje = "";
            }
        }
    }
}
