using Bd;
using FrameworkIntercapIT.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
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

        public int _idEmpresa { get; set; }
        public int _idEjecutivo { get; set; }
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
        public int _numEmpleados { get; set; }
        public DateTime? _fecIniContrato { get; set; }
        public DateTime? _fecFinContrato { get; set; }
        public string _tipoEmpresa { get; set; }
        public string _actividad { get; set; }
        public DateTime _fecIniAct { get; set; }
        public string _descEjecutivo { get; set; }
        public int _idActividad { get; set; }
        public int _idTipoEmpresa { get; set; }
        public int _validacion { get; set; }
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

        public Empresa TraeEmpresasClase(int idEmpresa)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Empresa empresa = new Empresa();
            DataSet ds = Ln.TraeDatosEmpresas(idEmpresa, true);

            if (ds.Tables.Count > 0)
            {
                DataRowCollection drc = ds.Tables[0].Rows;

                if (drc.Count > 0)
                {
                    try
                    {
                        if (drc[0]["FecInicioActEco"].ToString() != "")
                            empresa._fecIniAct = DateTime.Parse(drc[0]["FecInicioActEco"].ToString());
                        if (drc[0]["FecIniContrato"].ToString() != "")
                            empresa._fecIniContrato = DateTime.Parse(drc[0]["FecIniContrato"].ToString());
                        if (drc[0]["FecFinContrato"].ToString() != "")
                            empresa._fecFinContrato = DateTime.Parse(drc[0]["FecFinContrato"].ToString());
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    }
                    empresa._numEmpleados = (drc[0]["NumEmpleados"] != null && !string.IsNullOrEmpty(drc[0]["NumEmpleados"].ToString()) ? Convert.ToInt32(drc[0]["NumEmpleados"].ToString()) : 0);
                    empresa._telefono1 = (string.IsNullOrEmpty(drc[0]["TelFijo1"].ToString()) ? "" : drc[0]["TelFijo1"].ToString());
                    empresa._email = (string.IsNullOrEmpty(drc[0]["EMail"].ToString()) ? "" : drc[0]["EMail"].ToString());
                    empresa._idActividad = (drc[0]["IdActividad"] != null && !string.IsNullOrEmpty(drc[0]["IdActividad"].ToString()) ? Convert.ToInt32(drc[0]["IdActividad"].ToString()) : 0);
                    empresa._idTipoEmpresa = (drc[0]["IdTipoEmpresa"] != null && !string.IsNullOrEmpty(drc[0]["IdTipoEmpresa"].ToString()) ? Convert.ToInt32(drc[0]["IdTipoEmpresa"].ToString()) : 0);
                    empresa._razonSocial = drc[0]["RazonSocial"].ToString();
                    empresa._rut = (drc[0]["Rut"] != null && !string.IsNullOrEmpty(drc[0]["Rut"].ToString()) ? int.Parse(drc[0]["Rut"].ToString()) : 0);
                    empresa._bloqueado = (int)drc[0]["Bloqueado"];
                    if (drc[0]["idMotivo"].ToString() != "")
                        empresa._idMotivoBloqueo = (int)drc[0]["idMotivo"];
                    empresa._descMotivoBloqueo = drc[0]["descMotivo"].ToString();

                    empresa._PerteneceGrupo = (int)drc[0]["PerteneceGrupo"];
                    if (drc[0]["idGrupoEconomico"].ToString() != "")
                        empresa._idGrupoEconomico = (int)drc[0]["idGrupoEconomico"];
                    empresa._descGrupoEconomico = drc[0]["descGrupoEconomico"].ToString();
                    empresa._ClasificacionSBIF = drc[0]["ClasificacionSBIF"].ToString();
                    empresa._ClasificacionPAF = drc[0]["ClasificacionPAF"].ToString();
                    empresa._ObjetoSII = drc[0]["ObjetoSociedad"].ToString();
                }
            }
            return empresa;
        }

        //public string IngresaActualizaEmpresa(Empresa datosEmpresa, string accion, string usuario, int idUsuario, int idAsistente, string descAsistente)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParam;
        //        if (accion == "02")
        //            sqlParam = new SqlParameter[30];
        //        else
        //            sqlParam = new SqlParameter[27];

        //        sqlParam[0] = new SqlParameter("@rut", datosEmpresa._rut);
        //        sqlParam[1] = new SqlParameter("@razon", datosEmpresa._razonSocial);
        //        sqlParam[2] = new SqlParameter("@idEjecutivo", datosEmpresa._idEjecutivo);
        //        sqlParam[3] = new SqlParameter("@nomEje", datosEmpresa._descEjecutivo);
        //        sqlParam[4] = new SqlParameter("@numEmpleados", datosEmpresa._numEmpleados);
        //        sqlParam[5] = new SqlParameter("@tipoEmpresa", datosEmpresa._idTipoEmpresa);
        //        sqlParam[6] = new SqlParameter("@descTipoEmp", datosEmpresa._tipoEmpresa);
        //        sqlParam[7] = new SqlParameter("@idActividad", datosEmpresa._idActividad);
        //        sqlParam[8] = new SqlParameter("@actividad", datosEmpresa._actividad);
        //        sqlParam[9] = new SqlParameter("@telFijo", datosEmpresa._telefono1);
        //        sqlParam[10] = new SqlParameter("@eMail", datosEmpresa._email);
        //        sqlParam[11] = new SqlParameter("@idMotivo", datosEmpresa._idMotivoBloqueo);
        //        sqlParam[12] = new SqlParameter("@descMotivo", datosEmpresa._descMotivoBloqueo);
        //        sqlParam[13] = new SqlParameter("@accion", accion);
        //        sqlParam[14] = new SqlParameter("@idEmpresa", datosEmpresa._idEmpresa);
        //        sqlParam[15] = new SqlParameter("@usuario", usuario);
        //        sqlParam[16] = new SqlParameter("@IdUsuario", idUsuario);
        //        sqlParam[17] = new SqlParameter("@validacion", datosEmpresa._validacion);
        //        sqlParam[18] = new SqlParameter("@habEmpresa", datosEmpresa._habEmpresa);
        //        sqlParam[19] = new SqlParameter("@habDir", datosEmpresa._habDirEmpresa);
        //        sqlParam[20] = new SqlParameter("@fecIniAct", datosEmpresa._fecIniAct);
        //        sqlParam[21] = new SqlParameter("@idAsistente", idAsistente);
        //        sqlParam[22] = new SqlParameter("@descAsistente", descAsistente);
        //        sqlParam[23] = new SqlParameter("@Bloqueado", datosEmpresa._bloqueado);
        //        sqlParam[24] = new SqlParameter("@PerteneceGrupo", datosEmpresa._PerteneceGrupo);
        //        sqlParam[25] = new SqlParameter("@idGrupoEconomico", datosEmpresa._idGrupoEconomico);
        //        sqlParam[26] = new SqlParameter("@descGrupoEconomico", datosEmpresa._descGrupoEconomico);
        //        if (accion == "02")
        //        {
        //            sqlParam[27] = new SqlParameter("@fecIniContrato", datosEmpresa._fecIniContrato);
        //            sqlParam[28] = new SqlParameter("@fecFinContrato", datosEmpresa._fecFinContrato);
        //            sqlParam[29] = new SqlParameter("@objetoSII", datosEmpresa._ObjetoSII);
        //        }
        //        return AD_Global.traerPrimeraColumna("InsertaActualizaEmpresas", sqlParam);
        //    }
        //    catch
        //    {
        //        return "-1-Error interno.";
        //    }
        //}
  
        //public string InsertaActualizaContactoEmpresa(Empresa datosEmpresa, string accion, string usuario, int idUsuario, int idAsistente, string descAsistente)
        //{
        //    LogicaNegocio Ln = new LogicaNegocio();
        //    string retorno = Ln.InsertaActualizaContactoEmpresa(datosEmpresa._rut, datosEmpresa._razonSocial, datosEmpresa._idEjecutivo, datosEmpresa._descEjecutivo, datosEmpresa._numEmpleados
        //            , datosEmpresa._idTipoEmpresa, datosEmpresa._tipoEmpresa, datosEmpresa._idActividad, datosEmpresa._actividad, datosEmpresa._telefono1, datosEmpresa._email
        //            , datosEmpresa._idMotivoBloqueo, datosEmpresa._descMotivoBloqueo, accion, datosEmpresa._idEmpresa, usuario, idUsuario, datosEmpresa._validacion
        //            , datosEmpresa._habEmpresa, datosEmpresa._habDirEmpresa, datosEmpresa._fecIniAct, idAsistente, descAsistente, datosEmpresa._bloqueado
        //            , datosEmpresa._PerteneceGrupo, datosEmpresa._idGrupoEconomico, datosEmpresa._descGrupoEconomico, datosEmpresa._fecIniContrato.Value
        //            , datosEmpresa._fecFinContrato.Value, datosEmpresa._ObjetoSII);
        //    return retorno;
        //}

        public string IngresaActualizaEmpresa(Empresa datosEmpresa, string accion, string usuario, int idUsuario, int idAsistente, string descAsistente)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            string retorno = Ln.IngresaActualizaEmpresa(datosEmpresa._rut, datosEmpresa._razonSocial, datosEmpresa._idEjecutivo, datosEmpresa._descEjecutivo, datosEmpresa._numEmpleados
                    , datosEmpresa._idTipoEmpresa, datosEmpresa._tipoEmpresa, datosEmpresa._idActividad, datosEmpresa._actividad, datosEmpresa._telefono1, datosEmpresa._email
                    , datosEmpresa._idMotivoBloqueo, datosEmpresa._descMotivoBloqueo, accion, datosEmpresa._idEmpresa, usuario, idUsuario, datosEmpresa._validacion
                    , datosEmpresa._habEmpresa, datosEmpresa._habDirEmpresa, datosEmpresa._fecIniAct, idAsistente, descAsistente, datosEmpresa._bloqueado
                    , datosEmpresa._PerteneceGrupo, datosEmpresa._idGrupoEconomico, datosEmpresa._descGrupoEconomico, datosEmpresa._fecIniContrato
                    , datosEmpresa._fecFinContrato, datosEmpresa._ObjetoSII);
            return retorno;
        }


        public string ValidarDatosEmpresa(Empresa emp, string rutContacto, string dv, string nombreContacto, string telefonoFijo, string mail, string descCargo, string telefonoCelular)
        {
            return ValidarDatosCreacionEmpresa(emp, rutContacto.Trim(), dv.Trim(), nombreContacto.Trim(), telefonoFijo.Trim(), mail.Trim(), descCargo.Trim(), telefonoCelular.Trim());
        }

        public string ValidarDatosCreacionEmpresa(Empresa emp, string rutContacto, string dv, string nombreContacto, string telefonoFijo, string mail, string descCargo, string telefonoCelular)
        {
            string mensaje = string.Empty;

            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[12];
                sqlParam[0] = new SqlParameter("@rut", emp._rut);
                sqlParam[1] = new SqlParameter("@dvRut", emp._dvRut);
                sqlParam[2] = new SqlParameter("@razonSocial", emp._razonSocial);
                sqlParam[3] = new SqlParameter("@descEjecutivo", emp._descEjecutivo);
                sqlParam[4] = new SqlParameter("@fecIniAct", emp._fecIniAct);
                sqlParam[5] = new SqlParameter("@rutContacto", rutContacto);
                sqlParam[6] = new SqlParameter("@dvContacto", dv);
                sqlParam[7] = new SqlParameter("@nombreContacto", nombreContacto);
                sqlParam[8] = new SqlParameter("@telefonoFijo", telefonoFijo);
                sqlParam[9] = new SqlParameter("@mail", mail);
                sqlParam[10] = new SqlParameter("@descCargo", descCargo);
                sqlParam[11] = new SqlParameter("@telefonoCelular", telefonoCelular);

                return AD_Global.traerPrimeraColumna("sp_alertaEmpresa", sqlParam);
            }
            catch
            {
                return mensaje = "";
            }
        }

    }
}
