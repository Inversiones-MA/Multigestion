using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using MultigestionUtilidades;

namespace MultiFiscalia
{
    class LogicaNegocio
    {
        #region "wpSolicitudFiscalia"

        public Boolean GestionResponsableOperacion(int idOperacion, string usuario, string perfil, string opcion)
        {
            /*OPCIONES:
             * 01 =  Riesgo
             * 02 =  Fiscalia
             * 03 =  Operacion
             * 04 =  Contabilidad
            */
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                SqlParametros[1] = new SqlParameter("@usuario", SqlDbType.NChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NChar);
                SqlParametros[2].Value = perfil;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = opcion;
                return AD_Global.ejecutarAccion("GestionResponsablesOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarSolicitudFiscaliaOPE(string idEmpresa, string idOperacion, string xmlEmpresa, string xmlGarantia, string Comentario, string ComentarioF, string user, string perfil, string validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NChar);
                SqlParametros[2].Value = Comentario;

                SqlParametros[3] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlEmpresa;

                SqlParametros[4] = new SqlParameter("@xmlGarantía", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlGarantia;

                SqlParametros[5] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(idOperacion);

                SqlParametros[6] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;
               
                SqlParametros[8] = new SqlParameter("@ComentarioFiscalia", SqlDbType.NChar);
                SqlParametros[8].Value = ComentarioF;
            
                SqlParametros[9] = new SqlParameter("@validacion", SqlDbType.NChar);
                SqlParametros[9].Value = validacion;

                return AD_Global.ejecutarAccion("GuardarSolicitudFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ListarResumenOpe(string idEmpresa, string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@IdOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarServicios(string idEmpresa, string idPaf, string idoperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.NVarChar);
                SqlParametros[1].Value = idPaf;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = idoperacion;
                dt = AD_Global.ejecutarConsulta("ListarServiciosFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public string buscarMensaje(int idmensaje)
        {
            string mensaje = string.Empty;
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Mensajes"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + idmensaje + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    mensaje = oListItem["Mensaje"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return mensaje;
        }
   
        public DataSet ListarResumenTotal(string idEmpresa, string idPaf, string idoperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.NVarChar);
                SqlParametros[1].Value = idPaf;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@IdOperacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = idoperacion;

                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarResumen(string idEmpresa, string idOperacion, string idGarantia)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idGarantia", SqlDbType.NVarChar);
                SqlParametros[2].Value = idGarantia;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.INSERTAR;
                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarResumenPAF(string idEmpresa, string idPaf, string idoperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.NVarChar);
                SqlParametros[1].Value = idPaf;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@IdOperacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = idoperacion;

                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean InsertarSolicitudFiscalia(string idEmpresa, string xmlEmpresa, string xmlGarantia, string Comentario, string xmlComentario, string user, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NChar);
                SqlParametros[2].Value = Comentario;

                SqlParametros[3] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlEmpresa;

                SqlParametros[4] = new SqlParameter("@xmlGarantía", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlGarantia;

                SqlParametros[5] = new SqlParameter("@XmlComentarioG", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlGarantia;

                SqlParametros[6] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;
                return AD_Global.ejecutarAccion("GuardarSolicitudFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string ValidarMandatarioJudicial(int idEmpresa, int idOperacion, string descEmpresa)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@descEmpresa", SqlDbType.NVarChar);
                SqlParametros[2].Value = descEmpresa;

                return AD_Global.traerPrimeraColumna("ValidarMandatarioJudicial", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "";
            }
        }

        #endregion

        #region "wpServiciosLegalesGarantia"

        public DataTable ServiciosGarantia(string idEmpresa, string idpaf, string Perfil, string Usuario, string idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[1].Value = idpaf;
                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = Perfil;
                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Usuario;

                SqlParametros[4] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[4].Value = idOperacion;

                dt = AD_Global.ejecutarConsultas("ListarServiciosGarantias", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpServiciosLegalesEmpresa"

        public DataTable ServiciosOperacion(string idEmpresa, string idpaf, string Perfil, string Usuario, string idoperacion)
        {
            try
            {
                DataTable dt = new DataTable();

                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[1].Value = idpaf;
                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = Perfil;
                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Usuario;

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idoperacion;

                dt = AD_Global.ejecutarConsultas("ListarServiciosOperaciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ListarResumen(string idEmpresa, string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean ActualizarEstado(Boolean estado, string id, string idEmpresa)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@IdServicio", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(id);

                SqlParametros[2] = new SqlParameter("@FiscaliaEstado", SqlDbType.Int);
                SqlParametros[2].Value = estado;
                return AD_Global.ejecutarAccion("ActualizarEstadoOperaciones", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public int ActualizarEstado(String ID, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area)
        {
            try
            {
                SPWeb mySite = SPContext.Current.Web;
                SPList list = mySite.Lists["Operaciones"];

                SPListItemCollection items = list.GetItems(new SPQuery()
                {
                    Query = @"<Where><Eq><FieldRef Name='IdSQL' /><Value Type='Number'>" + ID + "</Value></Eq></Where>"
                });

                foreach (SPListItem item in items)
                {
                    item["IdEtapa"] = Etapa;
                    item["IdSubEtapa"] = subEtapa;
                    item["IdEstado"] = Estado;
                    item.Update();
                }
                list.Update();
                return 1;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return 0;
            }
        }

        public Boolean ActualizarSolicitudFiscaliaGarantia(string idEmpresa, string xmlEmpresa, string Validar, string idOperacion, string idUsuario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);    
                SqlParametros[1] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[1].Value = xmlEmpresa;

                SqlParametros[2] = new SqlParameter("@Validacion", SqlDbType.Int);
                SqlParametros[2].Value = Validar;

                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(idOperacion);

                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = idUsuario;

                return AD_Global.ejecutarAccion("ActualizarEstadoGarantia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public Boolean ActualizarSolicitudFiscalia(string idEmpresa, string xmlEmpresa, string Validar, string IdOperacion, string idUsuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[1].Value = xmlEmpresa;
                SqlParametros[2] = new SqlParameter("@Validacion", SqlDbType.Int);
                SqlParametros[2].Value = Validar;
                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(IdOperacion);
                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[4].Value = idUsuario;
                return AD_Global.ejecutarAccion("ActualizarEstadoOperaciones", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable validarDocCriticos(string idEmpresa, string IdOperacion, int accion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                SqlParametros[2] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[2].Value = accion;

                dt = AD_Global.ejecutarConsultas("validarDocCriticos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public int validacionCriticosFiscalia(string idEmpresa, string IdOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                dt = AD_Global.ejecutarConsultas("ValidarCriticoFiscalia", SqlParametros);
                if (dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return 0;
            }
        }

        public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[13];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;

                SqlParametros[5] = new SqlParameter("@idEstado", SqlDbType.Int);
                SqlParametros[5].Value = Estado;
                SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[6].Value = descEstado;

                SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
                SqlParametros[7].Value = Etapa;
                SqlParametros[8] = new SqlParameter("@descEtapa", SqlDbType.NVarChar);
                SqlParametros[8].Value = descEtapa;

                SqlParametros[9] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[9].Value = subEtapa;
                SqlParametros[10] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[10].Value = descsubEtapa;

                SqlParametros[11] = new SqlParameter("@Area", SqlDbType.NVarChar);
                SqlParametros[11].Value = Area;

                SqlParametros[12] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[12].Value = comentario;

                dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean BajarServicioOperacion(int idEmpresa, int idOperacion, int idServicio, int IdGarantia, string idUsuario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idServicio", SqlDbType.Int);
                SqlParametros[2].Value = idServicio;
                SqlParametros[3] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[3].Value = IdGarantia;
                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = idUsuario;

                dt = AD_Global.ejecutarAccion("BajarServicioOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarActualizarEstadosFiscalia(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string subEtapa, string descsubEtapa, string idPaf)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;
                SqlParametros[5] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(subEtapa);
                SqlParametros[6] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = descsubEtapa;
                SqlParametros[7] = new SqlParameter("@NroPaf", SqlDbType.Int);
                SqlParametros[7].Value = int.Parse(idPaf);
                return AD_Global.ejecutarAccion("ActualizarEstadosFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpListarFiscalia"

        public Boolean InsertarActualizarEstadosFiscalia(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string subEtapa, string descsubEtapa)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;

                SqlParametros[5] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[5].Value = subEtapa;
                SqlParametros[6] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = descsubEtapa;

                dt = AD_Global.ejecutarAccion("ActualizarEstadosFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string Buscar_Usuario(String Usuario)
        {
            string cargo = "";
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = currentSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        List<string> objColumns = new List<string>();
                        SPList list = web.Lists.TryGetList("Usuarios");
                        string sQuery = "<Where><Eq><FieldRef Name='Usuario'/><Value Type='Text'>" + Usuario + "</Value></Eq></Where>";
                        var oQuery = new SPQuery();
                        oQuery.Query = sQuery;
                        SPListItemCollection items = list.GetItems(oQuery);
                        SPListItemCollection collListItems = list.GetItems(oQuery);
                        foreach (SPListItem oListItem in collListItems)
                        {
                            cargo = oListItem["Cargo"] != null ? (oListItem["Cargo"].ToString()) : "";
                            break;
                        }
                    }
                }
            });
            return cargo;
        }

        public DataSet ListarFiscalia(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN, string edofiscalia)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@filtro", SqlDbType.NVarChar);
                SqlParametros[0].Value = buscar;
                SqlParametros[1] = new SqlParameter("@edoFiltro", SqlDbType.NVarChar);
                SqlParametros[1].Value = etapa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAGENERAL;
                SqlParametros[3] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[3].Value = area;
                SqlParametros[4] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargo;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[6].Value = pageS;
                SqlParametros[7] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[7].Value = pageN;
                SqlParametros[8] = new SqlParameter("@filtrosubestado", SqlDbType.NVarChar);
                SqlParametros[8].Value = subEtapa;
                SqlParametros[9] = new SqlParameter("@filtroetapa", SqlDbType.NVarChar);
                SqlParametros[9].Value = estado;
                SqlParametros[10] = new SqlParameter("@filtroEdoFiscalia", SqlDbType.NVarChar);
                SqlParametros[10].Value = edofiscalia;
        
                dt = AD_Global.ejecutarConsulta("ListarFiscalia", SqlParametros);
                
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarFiscaliaCFT(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN, string edofiscalia)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@filtro", SqlDbType.NVarChar);
                SqlParametros[0].Value = buscar;
                SqlParametros[1] = new SqlParameter("@edoFiltro", SqlDbType.NVarChar);
                SqlParametros[1].Value = etapa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAGENERAL;
                SqlParametros[3] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[3].Value = area;
                SqlParametros[4] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargo;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[6].Value = pageS;
                SqlParametros[7] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[7].Value = pageN;
                SqlParametros[8] = new SqlParameter("@filtrosubestado", SqlDbType.NVarChar);
                SqlParametros[8].Value = subEtapa;
                SqlParametros[9] = new SqlParameter("@filtroetapa", SqlDbType.NVarChar);
                SqlParametros[9].Value = estado;
                SqlParametros[10] = new SqlParameter("@filtroEdoFiscalia", SqlDbType.NVarChar);
                SqlParametros[10].Value = edofiscalia;

                dt = AD_Global.ejecutarConsulta("ListarFiscaliaCFT", SqlParametros);
                
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }
        

        #endregion

        #region "wpAprobacionSolicitud"

        public Boolean InsertarSolicitudFiscalia(string idEmpresa, string idOperacion, string xmlEmpresa, string xmlGarantia, string Comentario, string ComentarioF, string user, string perfil, string validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NChar);
                SqlParametros[2].Value = Comentario;

                SqlParametros[3] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlEmpresa;

                SqlParametros[4] = new SqlParameter("@xmlGarantía", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlGarantia;

                SqlParametros[5] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(idOperacion);

                SqlParametros[6] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;

                SqlParametros[8] = new SqlParameter("@ComentarioFiscalia", SqlDbType.NChar);
                SqlParametros[8].Value = ComentarioF;

                SqlParametros[9] = new SqlParameter("@validacion", SqlDbType.NChar);
                SqlParametros[9].Value = validacion;

                return AD_Global.ejecutarAccion("GuardarSolicitudFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "DocumentosFiscalia"

        public string ListarGarantiasAnexo1(int IdEmpresa)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                return mensaje = AD_Global.traerPrimeraColumna("ListarGarantiasAnexo1", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje;
            }
        }

        public string ListarRepresentantes(int IdEmpresa)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                return mensaje = AD_Global.traerPrimeraColumna("ListarRepresentantesEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje;
            }
        }

        public string ListarAvales(int IdEmpresa)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                return mensaje = AD_Global.traerPrimeraColumna("ListarAvalesEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje;
            }
        }
        
        public DataTable ConsultarDocumentosParametrizados(int IdProducto, int IdAcreedor, string area)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                SqlParametros[0].Value = IdProducto;
                SqlParametros[1] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[1].Value = IdAcreedor;
                SqlParametros[2] = new SqlParameter("@area", SqlDbType.NVarChar);
                SqlParametros[2].Value = area;

                dt = AD_Global.ejecutarConsultas("ConsultarDocumentosParametrizados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarClaseRepresentantes(string clase)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Clase", SqlDbType.NVarChar);
                SqlParametros[0].Value = clase;

                dt = AD_Global.ejecutarConsultas("ListarClaseRepresentantes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarRepresentantesId(int IdOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                dt = AD_Global.ejecutarConsultas("ListarRepresentantesId", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertaActualizaRepresentantes(int Id, int IdOperacion, int ClaseA, int ClaseB, string Usuario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                SqlParametros[0].Value = Id;
                SqlParametros[1] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[1].Value = IdOperacion;
                SqlParametros[2] = new SqlParameter("@ClaseA", SqlDbType.Int);
                SqlParametros[2].Value = ClaseA;
                SqlParametros[3] = new SqlParameter("@ClaseB", SqlDbType.Int);
                SqlParametros[3].Value = ClaseB;
                SqlParametros[4] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = Usuario;
          
                //SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                //SqlParametros[6].Value = descEstado;

                //SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
                //SqlParametros[7].Value = Etapa;


                dt = AD_Global.ejecutarAccion("InsertaActualizaRepresentantes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string TieneGarantiaFogape(int IdEmpresa)
        {
            try
            {
                string dt = string.Empty;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;

                dt = AD_Global.traerPrimeraColumna("TieneGarantiaFogape", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "0";
            }
        }

        public DataTable ListarPlantillaById(int IdPlantillaDocumento)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];

                SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
                SqlParametros[0].Value = IdPlantillaDocumento;

                dt = AD_Global.ejecutarConsultas("ListarPlantillaById", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet CargarDatosHtml(int IdOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                dt = AD_Global.ejecutarConsulta("CargarDatosPlantilla", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        public string ListarNumCertificado(int idOperacion)
        {
            string NumCertificado = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                return NumCertificado = AD_Global.traerPrimeraColumna("ListarNumCertificado", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return NumCertificado;
            }
        }

        public DataTable ListarUsuarios(int? IdCargo, int? IdDepartamento, string NombreUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[3];
                sqlParam[0] = new SqlParameter("@IdCargo", IdCargo);
                sqlParam[1] = new SqlParameter("@IdDepartamento", IdDepartamento);
                sqlParam[2] = new SqlParameter("@NombreUsuario", NombreUsuario);
                return AD_Global.ejecutarConsultas("ListarUsuario", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public DataTable ListarSubEtapaLegal(int Orden)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Orden", SqlDbType.Int);
                SqlParametros[0].Value = Orden;
               
                dt = AD_Global.ejecutarConsultas("ListarSubEtapaLegal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean GestionSubEtapaLegal(int IdOperacion, int Orden, string Comentario, int IdOpcion)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;
                SqlParametros[1] = new SqlParameter("@Orden", SqlDbType.Int);
                SqlParametros[1].Value = Orden;
                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[2].Value = Comentario;
                SqlParametros[3] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[3].Value = IdOpcion;

                dt = AD_Global.ejecutarAccion("GestionSubEtapaLegal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        
        
    }
}
