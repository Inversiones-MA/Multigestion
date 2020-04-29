using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using MultigestionUtilidades;
using Bd;

namespace MultiComercial
{
    class LN_Empresa
    {
        public static Empresa TraeEmpresasClase(int idEmpresa)
        {
            Empresa empresa = new Empresa();
            DataSet ds = TraeDatosEmpresas(idEmpresa, true);

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
                    empresa._numEmpleados = (drc[0]["NumEmpleados"] != null && !string.IsNullOrEmpty(drc[0]["NumEmpleados"].ToString()) ? Convert.ToInt64(drc[0]["NumEmpleados"].ToString()) : 0);
                    empresa._telefono1 = (string.IsNullOrEmpty(drc[0]["TelFijo1"].ToString()) ? "" : drc[0]["TelFijo1"].ToString());
                    empresa._email = (string.IsNullOrEmpty(drc[0]["EMail"].ToString()) ? "" : drc[0]["EMail"].ToString());
                    empresa._idActividad = (drc[0]["IdActividad"] != null && !string.IsNullOrEmpty(drc[0]["IdActividad"].ToString()) ? Convert.ToInt64(drc[0]["IdActividad"].ToString()) : 0);
                    empresa._idTipoEmpresa = (drc[0]["IdTipoEmpresa"] != null && !string.IsNullOrEmpty(drc[0]["IdTipoEmpresa"].ToString()) ? Convert.ToInt64(drc[0]["IdTipoEmpresa"].ToString()) : 0);
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

        public static string IngresaActualizaEmpresa(Empresa datosEmpresa, string accion, string usuario, int idUsuario, int idAsistente, string descAsistente)
        {
            try
            {
                SqlParameter[] sqlParam;
                if (accion == "02")
                    sqlParam = new SqlParameter[30];
                else
                    sqlParam = new SqlParameter[27];

                sqlParam[0] = new SqlParameter("@rut", datosEmpresa._rut);
                sqlParam[1] = new SqlParameter("@razon", datosEmpresa._razonSocial);
                sqlParam[2] = new SqlParameter("@idEjecutivo", datosEmpresa._idEjecutivo);
                sqlParam[3] = new SqlParameter("@nomEje", datosEmpresa._descEjecutivo);
                sqlParam[4] = new SqlParameter("@numEmpleados", datosEmpresa._numEmpleados);
                sqlParam[5] = new SqlParameter("@tipoEmpresa", datosEmpresa._idTipoEmpresa);
                sqlParam[6] = new SqlParameter("@descTipoEmp", datosEmpresa._tipoEmpresa);
                sqlParam[7] = new SqlParameter("@idActividad", datosEmpresa._idActividad);
                sqlParam[8] = new SqlParameter("@actividad", datosEmpresa._actividad);
                sqlParam[9] = new SqlParameter("@telFijo", datosEmpresa._telefono1);
                sqlParam[10] = new SqlParameter("@eMail", datosEmpresa._email);
                sqlParam[11] = new SqlParameter("@idMotivo", datosEmpresa._idMotivoBloqueo);
                sqlParam[12] = new SqlParameter("@descMotivo", datosEmpresa._descMotivoBloqueo);
                sqlParam[13] = new SqlParameter("@accion", accion);
                sqlParam[14] = new SqlParameter("@idEmpresa", datosEmpresa._idEmpresa);
                sqlParam[15] = new SqlParameter("@usuario", usuario);
                sqlParam[16] = new SqlParameter("@IdUsuario", idUsuario);
                sqlParam[17] = new SqlParameter("@validacion", datosEmpresa._validacion);
                sqlParam[18] = new SqlParameter("@habEmpresa", datosEmpresa._habEmpresa);
                sqlParam[19] = new SqlParameter("@habDir", datosEmpresa._habDirEmpresa);
                sqlParam[20] = new SqlParameter("@fecIniAct", datosEmpresa._fecIniAct);
                sqlParam[21] = new SqlParameter("@idAsistente", idAsistente);
                sqlParam[22] = new SqlParameter("@descAsistente", descAsistente);
                sqlParam[23] = new SqlParameter("@Bloqueado", datosEmpresa._bloqueado);
                sqlParam[24] = new SqlParameter("@PerteneceGrupo", datosEmpresa._PerteneceGrupo);
                sqlParam[25] = new SqlParameter("@idGrupoEconomico", datosEmpresa._idGrupoEconomico);
                sqlParam[26] = new SqlParameter("@descGrupoEconomico", datosEmpresa._descGrupoEconomico);
                if (accion == "02")
                {
                    sqlParam[27] = new SqlParameter("@fecIniContrato", datosEmpresa._fecIniContrato);
                    sqlParam[28] = new SqlParameter("@fecFinContrato", datosEmpresa._fecFinContrato);
                    sqlParam[29] = new SqlParameter("@objetoSII", datosEmpresa._ObjetoSII);
                }
                return AD_Global.traerPrimeraColumna("InsertaActualizaEmpresas", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public static DataSet TraeDatosEmpresas(int idEmpresa, bool activo)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[2];
                sqlParam[0] = new SqlParameter("@idEmpresa", idEmpresa);
                sqlParam[1] = new SqlParameter("@habilitado", activo);
                return AD_Global.ejecutarConsulta("ListaDatosEmpresas", sqlParam);
            }
            catch
            {
                return null;
            }
        }

        public static string InsertaActualizaContactoEmpresa(string apPaterno, string apMaterno, string nombres, string mail, string telFijo, string telCelu, int idCargo, string desCargo,
                                                           string accion, int idReg, string descReg, int idProv, string descProv, int idComuna, string comuna,
                                                           bool habilitado, bool contactoDefecto, int idEmpresa, int rut, string dv, int valContacto, string idUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[22];
                sqlParam[0] = new SqlParameter("@apPaterno", apPaterno);
                sqlParam[1] = new SqlParameter("@apMaterno", apMaterno);
                sqlParam[2] = new SqlParameter("@nombres", nombres);
                sqlParam[3] = new SqlParameter("@mail", mail);
                sqlParam[4] = new SqlParameter("@telFijo", telFijo);
                sqlParam[5] = new SqlParameter("@telCel", telCelu);
                sqlParam[6] = new SqlParameter("@desCargo", desCargo);
                sqlParam[7] = new SqlParameter("@accion", accion);
                sqlParam[8] = new SqlParameter("@idReg", idReg);
                sqlParam[9] = new SqlParameter("@descReg", descReg);
                sqlParam[10] = new SqlParameter("@idProv", idProv);
                sqlParam[11] = new SqlParameter("@desProv", descProv);
                sqlParam[12] = new SqlParameter("@idComuna", idComuna);
                sqlParam[13] = new SqlParameter("@comuna", comuna);
                sqlParam[14] = new SqlParameter("@habilitado", habilitado);
                sqlParam[15] = new SqlParameter("@contactoDef", contactoDefecto);
                sqlParam[16] = new SqlParameter("@idEmpresa", idEmpresa);
                sqlParam[17] = new SqlParameter("@rut", rut);
                sqlParam[18] = new SqlParameter("@dv", dv);
                sqlParam[19] = new SqlParameter("@valContacto", valContacto);
                sqlParam[20] = new SqlParameter("@idCargo", idCargo);
                sqlParam[21] = new SqlParameter("@user", idUsuario);
                return AD_Global.traerPrimeraColumna("InsertaActualizaContactoEmpresas", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public DataTable buscarDatosRut(string rut)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@rut", SqlDbType.NVarChar);
                SqlParametros[0].Value = rut;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("BuscarDatosRut", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        
    }
}
