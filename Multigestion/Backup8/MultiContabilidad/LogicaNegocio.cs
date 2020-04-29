using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using MultigestionUtilidades;

namespace MultiContabilidad
{
    class LogicaNegocio
    {

        #region "wpContabilidad"

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

        public DataTable CRUDAcreedores(int Opcion, string RutAcreedor, int IdAcreedor, string NombreAcreedor, string DomicilioAcreedor, int IdTipoAcreedor, int IdRegion, int IdProvincia, int IdComuna, string IdUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[10];
                sqlParam[0] = new SqlParameter("@Opcion", Opcion);
                sqlParam[1] = new SqlParameter("@RutAcreedor", RutAcreedor.Trim());
                sqlParam[2] = new SqlParameter("@IdAcreedor", IdAcreedor);
                sqlParam[3] = new SqlParameter("@NombreAcreedor", NombreAcreedor);
                sqlParam[4] = new SqlParameter("@DomicilioAcreedor", DomicilioAcreedor);
                sqlParam[5] = new SqlParameter("@IdTipoAcreedor", IdTipoAcreedor);
                sqlParam[6] = new SqlParameter("@IdRegion", IdRegion);
                sqlParam[7] = new SqlParameter("@IdProvincia", IdProvincia);
                sqlParam[8] = new SqlParameter("@IdComuna", IdComuna);
                sqlParam[9] = new SqlParameter("@IdUsuario", IdUsuario);
                return AD_Global.ejecutarConsultas("CRUDAcreedores", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public Boolean InsertarActualizarFacturacion(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string nroFactura, string FecFactura, string DescTipoTransaccion, string IdIVA, string DescIVA, string IdTipoDocumento, string DescTipoDocumento)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[12];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;
                SqlParametros[4] = new SqlParameter("@FecFactura", SqlDbType.NVarChar);
                SqlParametros[4].Value = FecFactura;
                SqlParametros[5] = new SqlParameter("@nroFactura", SqlDbType.NVarChar);
                SqlParametros[5].Value = nroFactura;
                SqlParametros[6] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[6].Value = "01";
                SqlParametros[7] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[7].Value = DescTipoTransaccion;

                SqlParametros[8] = new SqlParameter("@IdIVA", SqlDbType.Int);
                SqlParametros[8].Value = IdIVA;
                SqlParametros[9] = new SqlParameter("@DescIVA", SqlDbType.NVarChar);
                SqlParametros[9].Value = DescIVA;
                SqlParametros[10] = new SqlParameter("@IdTipoDocumento", SqlDbType.Int);
                SqlParametros[10].Value = IdTipoDocumento;
                SqlParametros[11] = new SqlParameter("@DescTipoDocumento", SqlDbType.NVarChar);
                SqlParametros[11].Value = DescTipoDocumento;

                dt = AD_Global.ejecutarAccion("GestionFacturacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
     string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
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
                SqlParametros[2].Value = "01";
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

        public DataTable ConsultaFacturacion(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string descTipoTransaccion)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;

                SqlParametros[3] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;


                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[4].Value = "02";

                SqlParametros[5] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[5].Value = descTipoTransaccion;

                dt = AD_Global.ejecutarConsultas("GestionFacturacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ConsultarOperacion(int IdOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;
                SqlParametros[1] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[1].Value = 6;

                dt = AD_Global.ejecutarConsultas("GestionOperaciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ConsultaDatosContabilidadXML(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string descTipoTransaccion)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;

                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;

                SqlParametros[4] = new SqlParameter("@descTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[4].Value = descTipoTransaccion;

                dt = AD_Global.ejecutarConsulta("ConsultarDatosContabilidadXML", SqlParametros);
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
                    mensaje = (oListItem["Mensaje"].ToString());
                }


            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                mensaje = ex.Source.ToString() + ex.Message.ToString();

            }
            return mensaje.Replace("<p>", "");
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

                //logger.Debug("Error en " + ex.Source.ToString() + " : " + ex.Message.ToString());
                return 0;
            }
        }

        public DataSet ConsultaDatosContabilidad(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string descTipoTransaccion)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;

                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;

                SqlParametros[4] = new SqlParameter("@descTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[4].Value = descTipoTransaccion;

                dt = AD_Global.ejecutarConsulta("ConsultarDatosContabilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpListarContabilidad"

        public DataSet ListarContabilidad(string Perfil, string Etapa, string Usuario, string IdTransaccion, int IdSubEtapa, string IdNro,
           string Rut, string RazonSocial, string NCertificado, string fechaInicio, string fechaFin, int pageS, int pageN)
        {

            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[14];

                SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                SqlParametros[1] = new SqlParameter("@Etapa", SqlDbType.NChar);
                SqlParametros[1].Value = Etapa;

                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = Perfil;

                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Usuario;

                SqlParametros[4] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[4].Value = pageS;

                SqlParametros[5] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[5].Value = pageN;

                SqlParametros[6] = new SqlParameter("@IdTransaccion", SqlDbType.NVarChar);
                SqlParametros[6].Value = IdTransaccion;

                SqlParametros[7] = new SqlParameter("@IdSubEtapa", SqlDbType.NVarChar);
                SqlParametros[7].Value = IdSubEtapa;

                SqlParametros[8] = new SqlParameter("@IdNro", SqlDbType.NVarChar);
                SqlParametros[8].Value = IdNro;

                SqlParametros[9] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[9].Value = Rut;

                SqlParametros[10] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[10].Value = RazonSocial;

                SqlParametros[11] = new SqlParameter("@NCertificado", SqlDbType.NVarChar);
                SqlParametros[11].Value = NCertificado;

                SqlParametros[12] = new SqlParameter("@fechaInicio", SqlDbType.NVarChar);
                SqlParametros[12].Value = fechaInicio;

                SqlParametros[13] = new SqlParameter("@fechaFin", SqlDbType.NVarChar);
                SqlParametros[13].Value = fechaFin;

                dt = AD_Global.ejecutarConsulta("ListarContabilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
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

        #endregion

        #region "wpListarContabilidadHistorico"

        public DataSet ListarContabilidadHistorial(string Perfil, string Usuario, string IdTransaccion, int IdSubEtapa, string IdNro,
        string Rut, string RazonSocial, string NCertificado, string fechaInicio, string fechaFin, int pageS, int pageN, string Etapa)
        {

            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[14];

                SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                SqlParametros[1] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[1].Value = Perfil;

                SqlParametros[2] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = Usuario;

                SqlParametros[3] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[3].Value = pageS;

                SqlParametros[4] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[4].Value = pageN;

                SqlParametros[5] = new SqlParameter("@IdTransaccion", SqlDbType.NVarChar);
                SqlParametros[5].Value = IdTransaccion;

                SqlParametros[6] = new SqlParameter("@IdSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = IdSubEtapa;

                SqlParametros[7] = new SqlParameter("@IdNro", SqlDbType.NVarChar);
                SqlParametros[7].Value = IdNro;

                SqlParametros[8] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[8].Value = Rut;

                SqlParametros[9] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[9].Value = RazonSocial;

                SqlParametros[10] = new SqlParameter("@NCertificado", SqlDbType.NVarChar);
                SqlParametros[10].Value = NCertificado;

                SqlParametros[11] = new SqlParameter("@fechaInicio", SqlDbType.NVarChar);
                SqlParametros[11].Value = fechaInicio;

                SqlParametros[12] = new SqlParameter("@fechaFin", SqlDbType.NVarChar);
                SqlParametros[12].Value = fechaFin;

                SqlParametros[13] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[13].Value = Etapa;

                dt = AD_Global.ejecutarConsulta("ListarContabilidaHistorial", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpPagar"

        public Boolean InsertarActualizarPagoFacturacion(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
  string Comentario, string NroDocumentoPago, string idBanco, string descBanco, string fechaPago, string idTipoPago, string DescTipoPago, string DescTipoTransaccion)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[13];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[4].Value = "04";

                SqlParametros[5] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[5].Value = Comentario;
                SqlParametros[6] = new SqlParameter("@NroDocumentoPago", SqlDbType.NVarChar);
                SqlParametros[6].Value = NroDocumentoPago;
                SqlParametros[7] = new SqlParameter("@idBanco", SqlDbType.NVarChar);
                SqlParametros[7].Value = idBanco;
                SqlParametros[8] = new SqlParameter("@descBanco", SqlDbType.NVarChar);
                SqlParametros[8].Value = descBanco;
                SqlParametros[9] = new SqlParameter("@fechaPago", SqlDbType.NVarChar);
                SqlParametros[9].Value = fechaPago;
                SqlParametros[10] = new SqlParameter("@idTipoPago", SqlDbType.NVarChar);
                SqlParametros[10].Value = idTipoPago;
                SqlParametros[11] = new SqlParameter("@DescTipoPago", SqlDbType.NVarChar);
                SqlParametros[11].Value = DescTipoPago;

                SqlParametros[12] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[12].Value = DescTipoTransaccion;

                dt = AD_Global.ejecutarAccion("GestionFacturacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarActualizarEstadoPagar(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
      string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario, string id)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = "01";
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

                SqlParametros[13] = new SqlParameter("@idTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[13].Value = id;

                dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpRendicionAAT"


        public DataTable ListarRendicionCliente(string Usuario)
        {
            try
            {
                DataTable dt = new DataTable();
                //AD_Global objDatos = new AD_Global();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@usuario", SqlDbType.NChar);
                SqlParametros[0].Value = Usuario;

                dt = AD_Global.ejecutarConsultas("BuscarRendicionesUsuario", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
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

        #endregion


        #region "wpBitacora"

        //public DataTable GestionBitacoraPago(string xmlBitacora, string NroCertificado, int? IdMotivo, int? IdCausa, int? IdBanco, DateTime? FechaCobro, DateTime? FechaPago, string NroDocumento, int? Monto, string Comentario, int? IdBitacoraPago, string IdUsuario, string RazonSocial, int IdOpcion)
        public DataTable GestionBitacoraPago(string NroCertificado, string RazonSocial, int IdOpcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = NroCertificado;
                SqlParametros[1] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[1].Value = RazonSocial;
                SqlParametros[2] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[2].Value = IdOpcion;

                //SqlParameter[] SqlParametros = new SqlParameter[14];
                //SqlParametros[0] = new SqlParameter("@xmlBitacora", SqlDbType.Xml);
                //SqlParametros[0].Value = xmlBitacora;
                //SqlParametros[1] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                //SqlParametros[1].Value = NroCertificado;
                //SqlParametros[2] = new SqlParameter("@IdMotivo", SqlDbType.Int);
                //SqlParametros[2].Value = IdMotivo;
                //SqlParametros[3] = new SqlParameter("@IdCausa", SqlDbType.Int);
                //SqlParametros[3].Value = IdCausa;
                //SqlParametros[4] = new SqlParameter("@IdBanco", SqlDbType.Int);
                //SqlParametros[4].Value = IdBanco;
                //SqlParametros[5] = new SqlParameter("@FechaCobro", SqlDbType.DateTime);
                //SqlParametros[5].Value = FechaCobro;
                //SqlParametros[6] = new SqlParameter("@FechaPago", SqlDbType.DateTime);
                //SqlParametros[6].Value = FechaPago;
                //SqlParametros[7] = new SqlParameter("@NroDocumento", SqlDbType.NVarChar);
                //SqlParametros[7].Value = NroDocumento;
                //SqlParametros[8] = new SqlParameter("@Monto", SqlDbType.Int);
                //SqlParametros[8].Value = Monto;
                //SqlParametros[9] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                //SqlParametros[9].Value = Comentario;
                //SqlParametros[10] = new SqlParameter("@IdBitacoraPago", SqlDbType.Int);
                //SqlParametros[10].Value = IdBitacoraPago;
                //SqlParametros[11] = new SqlParameter("@IdUsuario", SqlDbType.NVarChar);
                //SqlParametros[11].Value = IdUsuario;
                //SqlParametros[12] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                //SqlParametros[12].Value = RazonSocial;
                //SqlParametros[13] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                //SqlParametros[13].Value = IdOpcion;

                dt = AD_Global.ejecutarConsultas("GestionBitacoraPago", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        public Boolean Gestion_BitacoraPago(string xmlBitacora, string NroCertificado, int? IdMotivo, int? IdCausa, int? IdBanco, DateTime? FechaCobro, DateTime? FechaPago, string NroDocumento, int? Monto, string Cuota, int IdAcreedor, string Comentario, int? IdBitacoraPago, string IdUsuario, int IdOpcion)
        {
            try
            {
                bool dt = false;
                SqlParameter[] SqlParametros = new SqlParameter[15];
                SqlParametros[0] = new SqlParameter("@xmlBitacora", SqlDbType.Xml);
                SqlParametros[0].Value = xmlBitacora;
                SqlParametros[1] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[1].Value = NroCertificado;
                SqlParametros[2] = new SqlParameter("@IdMotivo", SqlDbType.Int);
                SqlParametros[2].Value = IdMotivo;
                SqlParametros[3] = new SqlParameter("@IdCausa", SqlDbType.Int);
                SqlParametros[3].Value = IdCausa;
                SqlParametros[4] = new SqlParameter("@IdBanco", SqlDbType.Int);
                SqlParametros[4].Value = IdBanco;
                SqlParametros[5] = new SqlParameter("@FechaCobro", SqlDbType.DateTime);
                SqlParametros[5].Value = FechaCobro;
                SqlParametros[6] = new SqlParameter("@FechaPago", SqlDbType.DateTime);
                SqlParametros[6].Value = FechaPago;
                SqlParametros[7] = new SqlParameter("@NroDocumento", SqlDbType.NVarChar);
                SqlParametros[7].Value = NroDocumento;
                SqlParametros[8] = new SqlParameter("@Monto", SqlDbType.Int);
                SqlParametros[8].Value = Monto;

                SqlParametros[9] = new SqlParameter("@Cuota", SqlDbType.NVarChar);
                SqlParametros[9].Value = Cuota;

                SqlParametros[10] = new SqlParameter("@IdAcreedor", SqlDbType.NVarChar);
                SqlParametros[10].Value = IdAcreedor;

                SqlParametros[11] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[11].Value = Comentario;


                SqlParametros[12] = new SqlParameter("@IdBitacoraPago", SqlDbType.Int);
                SqlParametros[12].Value = IdBitacoraPago;
                SqlParametros[13] = new SqlParameter("@IdUsuario", SqlDbType.NVarChar);
                SqlParametros[13].Value = IdUsuario;
                SqlParametros[14] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[14].Value = IdOpcion;

                dt = AD_Global.ejecutarAccionBool("GestionBitacoraPago", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #region "cuenta corriente"

        public int CP_VerificarCertificado(string NroCertificado)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = NroCertificado;

                var resultado = AD_Global.traerPrimeraColumna("VerificarCertificado", SqlParametros);
                return int.Parse(resultado);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return -1;
            }
        }

        public DataTable ListarMotivos()
        {
           try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];

                //SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                //SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                dt = AD_Global.ejecutarConsultas("ListarMotivos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCausas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];

                //SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                //SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                dt = AD_Global.ejecutarConsultas("ListarCausas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #endregion

    }
}
