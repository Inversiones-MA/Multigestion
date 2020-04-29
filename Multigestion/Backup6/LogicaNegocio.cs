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
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using MultigestionUtilidades;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using ExpertPdf.HtmlToPdf;
using Microsoft.SharePoint.Utilities;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace MultiRiesgo
{
    class LogicaNegocio
    {
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

        #region "wpIvaCompras"

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

        public Boolean finalizarIvaCOMPRAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVACOMPRAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                SqlParametros[6] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[6].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean finalizarIvaVENTAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVAVENTAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                SqlParametros[6] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[6].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
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

        public Boolean InsertarIvaCOMPRAS(string idEmpresa, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVACOMPRAS;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;
                SqlParametros[4] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[4].Value = anio;

                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarIvaVENTAS(string idEmpresa, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVAVENTAS;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;
                SqlParametros[4] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[4].Value = anio;

                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarIvaCOMPRAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVACOMPRAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[5].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarIvaVENTAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVAVENTAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[5].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarIVACompras(int idEmpresa, int Anio, int IdDocumentoContable)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Anio", SqlDbType.Int);
                SqlParametros[1].Value = Anio;
                SqlParametros[2] = new SqlParameter("@IdDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = IdDocumentoContable;
                dt = AD_Global.ejecutarConsultas("ConsultarIVA", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ConsultaReporteIvaCompraVenta(int idEmpresa)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                dt = AD_Global.ejecutarConsulta("ReporteIvaCompraVenta", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpListarRiesgo"

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

        public void CargarLista(ref DropDownList lista, string Nombre_Lista)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                string Nombre_Vista = Constantes.LISTAESTADO.VISTA;
                SPList listData = app.Lists[Nombre_Lista];
                SPView view = listData.Views[Nombre_Vista];
                view.Query = "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                // view.Update();
                lista.DataSource = listData.GetItems(view).GetDataTable();
                lista.DataTextField = listData.GetItems(view).Fields["Nombre"].ToString();
                lista.DataValueField = listData.GetItems(view).Fields["ID"].ToString();
                lista.DataBind();
                lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione " + Nombre_Lista + "--", "-1"));
                lista.SelectedIndex = 0;
                if (Nombre_Lista == "Estados")
                {
                    ListItem itemToRemove = lista.Items.FindByValue("5");
                    if (itemToRemove != null)
                    {
                        lista.Items.Remove(itemToRemove);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                //log.Error("ConexionDatos" + ex.ToString());
            }
            finally
            {
                // lista = null;

            }
        }

        public void CargarListaSubEtapa(ref DropDownList lista, string Nombre_Lista, string etapa)
        {
            try
            {

                lista.Items.Clear();
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList lis = app.Lists[Nombre_Lista];
                SPQuery view = new SPQuery();
                view.Query = "<Where><Eq><FieldRef Name='Etapa_x003a_ID'  /><Value Type='Lookup' >" + Convert.ToInt32(etapa) + "</Value></Eq></Where><OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                SPListItemCollection collListItems = lis.GetItems(view);


                lista.DataSource = collListItems.GetDataTable();
                lista.DataTextField = "Title";
                lista.DataValueField = "ID";
                lista.DataBind();
                lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione " + Nombre_Lista + "--", "-1"));
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            finally
            {
                //  lista = null;

            }
        }

        public DataSet ListarRiesgo(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[10];
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

                dt = AD_Global.ejecutarConsulta("ListarRiesgo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarCftRiesgo(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[10];
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

                dt = AD_Global.ejecutarConsulta("ListarRiesgoCFT", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpScoring"

        public void CargarVista(ref DropDownList lista, string Vista)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList listData = app.Lists[Constantes.LISTAINDICADORES.INDICADORES];
                SPView view = listData.Views[Vista];
                lista.DataSource = listData.GetItems(view).GetDataTable();
                lista.DataTextField = listData.GetItems(view).Fields["Title"].ToString();
                lista.DataValueField = listData.GetItems(view).Fields["ID"].ToString();
                lista.DataBind();
                lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                lista.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public Boolean InsertarScoring(string idEmpresa, string xmlData, string usuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[1].Value = 1;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[5].Value = perfil;

                return AD_Global.ejecutarAccion("GuardarScoring", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean finalizarScoring(string idEmpresa, string xmlData, string usuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[1].Value = 2;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[5].Value = perfil;

                return AD_Global.ejecutarAccion("GuardarScoring", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarScoring(string idEmpresa, string usuario, string perfil, string opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = opcion;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsultas("GeneracionScoring", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ListarIndicadoresScoring(string idEmpresa, string usuario, string perfil, string opcion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = opcion;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("GeneracionScoring", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        //public void CargarActividad(ref DropDownList lista, string Vista)
        //{
        //    try
        //    {
        //        SPWeb app = SPContext.Current.Web;
        //        app.AllowUnsafeUpdates = true;
        //        SPList listData = app.Lists[Constantes.LISTAINDICADORES.ACTIVIDAD];
        //        SPView view = listData.Views[Vista];
        //        // view.Query = "<OrderBy><FieldRef Name='Title' Ascending='TRUE' /></OrderBy>";
        //        //view.Update();
        //        lista.DataSource = listData.GetItems(view).GetDataTable();
        //        lista.DataTextField = listData.GetItems(view).Fields["Nombres"].ToString();
        //        lista.DataValueField = listData.GetItems(view).Fields["ID"].ToString();
        //        lista.DataBind();
        //        lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione--", "0"));
        //        lista.SelectedIndex = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //    }
        //}

        public DataSet ConsultaReporteScoring(int idEmpresa, string usuario, string perfil)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;

                dt = AD_Global.ejecutarConsulta("ReporteScoring", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpVaciadoBalance"

        public Boolean ModificarBalance(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarBalance(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idDocumentosContables", SqlDbType.Int);
                SqlParametros[0].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GeneracionBalance", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertarFinalizarBalance(string idEmpresa, string xmlData)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACION;

                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarBalance(string idEmpresa, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACIONINSERT;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean finalizarBalance(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ConsultaReporteVaciadoBalance(int idEmpresa)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                dt = AD_Global.ejecutarConsulta("ReporteNoResumen", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public void CargarListaVaciadoBalance(ref DropDownList lista)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList listData = app.Lists[Constantes.LISTATIPODOCUMET.TIPODOCUMENT];
                SPView view = listData.Views[Constantes.LISTATIPODOCUMET.VISTA];
                view.Query = "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                // view.Update();
                lista.DataSource = listData.GetItems(view).GetDataTable();
                lista.DataTextField = listData.GetItems(view).Fields["Nombre"].ToString();
                lista.DataValueField = listData.GetItems(view).Fields["ID"].ToString();
                lista.DataBind();
                // lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                //lista.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //log.Error("ConexionDatos" + ex.ToString());
            }
        }

        #endregion

        #region "wpPAF"

        public Boolean DarBajaOperacion(string idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;

                return AD_Global.ejecutarAccion("DarBajaOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ActualizarImpresionPAF(int idPAF)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[0].Value = idPAF;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[1].Value = Constantes.OPCIONIMPRESIONPAF.IMPRIMIR;
                SqlParametros[2] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[2].Value = Constantes.OPCIONIMPRESIONPAF.EDOIMPRESO;
                dt = AD_Global.ejecutarConsulta("GestionImpresionPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean ActualizarOperaciones(Dictionary<string, string> datos, Dictionary<string, string> estados)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[16];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(datos["IdOperacion"]);
                SqlParametros[1] = new SqlParameter("@Plazo", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(datos["Plazo"]);
                SqlParametros[2] = new SqlParameter("@ComisionP", SqlDbType.Float);
                SqlParametros[2].Value = float.Parse(datos["ComisionP"]);
                SqlParametros[3] = new SqlParameter("@Seguro", SqlDbType.Money);
                SqlParametros[3].Value = double.Parse(datos["Seguro"]);
                SqlParametros[4] = new SqlParameter("@Comision", SqlDbType.Money);
                SqlParametros[4].Value = double.Parse(datos["Comision"]);
                SqlParametros[5] = new SqlParameter("@MontoCredito", SqlDbType.Money);
                SqlParametros[5].Value = double.Parse(datos["MontoCredito"]);
                SqlParametros[6] = new SqlParameter("@MontoPropuesto", SqlDbType.Money);
                SqlParametros[6].Value = double.Parse(datos["MontoPropuesto"]);
                SqlParametros[7] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[7].Value = double.Parse(datos["IdPaf"]);
                SqlParametros[8] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[8].Value = 4;
                if (estados != null)
                {
                    SqlParametros[9] = new SqlParameter("@NIdEstado", SqlDbType.Int);
                    SqlParametros[9].Value = int.Parse(estados["IdEstado"]);
                    SqlParametros[10] = new SqlParameter("@NDescEstado", SqlDbType.NChar);
                    SqlParametros[10].Value = estados["DescEstado"].ToString();

                    SqlParametros[11] = new SqlParameter("@NIdEtapa", SqlDbType.Int);
                    SqlParametros[11].Value = int.Parse(estados["IdEtapa"]);
                    SqlParametros[12] = new SqlParameter("@NDescEtapa", SqlDbType.NChar);
                    SqlParametros[12].Value = estados["DescEtapa"].ToString();

                    SqlParametros[13] = new SqlParameter("@NIdSubEtapa", SqlDbType.Int);
                    SqlParametros[13].Value = int.Parse(estados["IdSubEtapa"]);
                    SqlParametros[14] = new SqlParameter("@NDescSubEtapa", SqlDbType.NChar);
                    SqlParametros[14].Value = estados["DescSubEtapa"].ToString();
                    SqlParametros[15] = new SqlParameter("@Area", SqlDbType.NChar);
                    SqlParametros[15].Value = "Comercial";
                }
                else
                {
                    SqlParametros[9] = new SqlParameter("@NIdEstado", SqlDbType.Int);
                    SqlParametros[9].Value = -1;
                    SqlParametros[10] = new SqlParameter("@NDescEstado", SqlDbType.NChar);
                    SqlParametros[10].Value = "-1";

                    SqlParametros[11] = new SqlParameter("@NIdEtapa", SqlDbType.Int);
                    SqlParametros[11].Value = -1;
                    SqlParametros[12] = new SqlParameter("@NDescEtapa", SqlDbType.NChar);
                    SqlParametros[12].Value = "-1";

                    SqlParametros[13] = new SqlParameter("@NIdSubEtapa", SqlDbType.Int);
                    SqlParametros[13].Value = -1;
                    SqlParametros[14] = new SqlParameter("@NDescSubEtapa", SqlDbType.NChar);
                    SqlParametros[14].Value = "-1";
                    SqlParametros[15] = new SqlParameter("@Area", SqlDbType.NChar);
                    SqlParametros[15].Value = datos["Area"].ToString();
                }

                bool a = AD_Global.ejecutarAccion("GestionOperaciones", SqlParametros);
                if (estados != null)
                {

                    SPWeb mySite = SPContext.Current.Web;
                    SPList list = mySite.Lists["Operaciones"];

                    SPListItemCollection items = list.GetItems(new SPQuery()
                    {
                        Query = @"<Where><Eq><FieldRef Name='IdSQL' /><Value Type='Number'>" + datos["IdOperacion"] + "</Value></Eq></Where>"
                    });

                    foreach (SPListItem item in items)
                    {
                        item["IdEtapa"] = estados["IdEtapa"];
                        item["IdSubEtapa"] = estados["IdSubEtapa"];
                        item["IdEstado"] = estados["IdEstado"];
                        //item["IdPaf"] = datos["IdPaf"];
                        item.Update();
                    }
                    list.Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Dictionary<string, string> BuscarIdsYValoresCambioDeEstado(string Indicador)
        {
            string mensaje = string.Empty;
            Dictionary<string, string> datos = new Dictionary<string, string>();
            try
            {

                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["CambiosEstados"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + Indicador + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    datos.Add("IdEstado", oListItem["Estado:ID"].ToString().Split('#')[1]);
                    datos.Add("DescEstado", oListItem["Estado"].ToString().Split('#')[1]);
                    datos.Add("IdEtapa", oListItem["Etapa:ID"].ToString().Split('#')[1]);
                    datos.Add("DescEtapa", oListItem["Etapa"].ToString().Split('#')[1]);
                    datos.Add("IdSubEtapa", oListItem["SubEtapa:ID"].ToString().Split('#')[1]);
                    datos.Add("DescSubEtapa", oListItem["SubEtapa"].ToString().Split('#')[1]);
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return datos;
            //return mensaje;
        }

        public string BuscarValorIndicador(DateTime fecha1)
        {
            string mensaje = string.Empty;
            try
            {
                string fecha = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString();
                string startDate = (SPUtility.CreateISO8601DateTimeFromSystemDateTime(Convert.ToDateTime(fecha1)));

                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["ValorMoneda"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<OrderBy>  <FieldRef Name='ID' Ascending ='FALSE'/>   </OrderBy> <Where> <Eq><FieldRef Name='Fecha' /><Value  Type='DateTime'>" + startDate + "</Value></Eq> </Where>";

                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    if (oListItem["MontoUSD"] == null) mensaje = "0"; else mensaje = oListItem["MontoUSD"].ToString();
                    if (oListItem["MontoUF"] == null) mensaje = mensaje + "/" + "0"; else mensaje = mensaje + "/" + oListItem["MontoUF"].ToString();
                    // mensaje = mensaje +"/" +oListItem["MontoUF"].ToString();
                    // mensaje = mensaje +"/"+ oListItem["MontoEURO"].ToString();
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return mensaje;
        }

        public DataSet ConsultarImpresionPAF(int idPAF)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[0].Value = idPAF;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[1].Value = Constantes.OPCIONIMPRESIONPAF.CONSULTAR;
                dt = AD_Global.ejecutarConsulta("GestionImpresionPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        //public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string ejecutivo)
        //{
        //    try
        //    {
        //        Utilidades util = new Utilidades();
        //        String xml = String.Empty;
        //        String xmlReporteNoResumen = String.Empty;
        //        String xmlReporteIVACompraVentas = String.Empty;
        //        String xmlIndicFinancieros = String.Empty;

        //        DataSet res1 = new DataSet();

        //        res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdPaf"], "Sistema", "Adminitrador", "ReportePAF");

        //        //Tabla 0 Contiene la cantidad de Empresas en el grupo Económico
        //        int cantEmpresas = int.Parse(res1.Tables[0].Rows[0][0].ToString());

        //        for (int i = 0; i < res1.Tables[1].Rows.Count; i++)
        //        {
        //            xml = xml + res1.Tables[1].Rows[i][0];
        //        }
        //        for (int i = 0; i < res1.Tables[2].Rows.Count; i++)
        //        {
        //            xmlReporteIVACompraVentas = xmlReporteIVACompraVentas + res1.Tables[2].Rows[i][0];
        //        }

        //        //Tabla 3 Contiene los datos para la creación de los gráficos

        //        for (int i = 0; i < res1.Tables[4].Rows.Count; i++)
        //        {
        //            xmlReporteNoResumen = xmlReporteNoResumen + res1.Tables[4].Rows[i][0];
        //        }

        //        for (int i = 0; i < res1.Tables[5].Rows.Count; i++)
        //        {
        //            xmlIndicFinancieros = xmlIndicFinancieros + res1.Tables[5].Rows[i][0];

        //        }
        //        xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<ResumenIVA>");
        //        xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("</Val></Valores>", "</ResumenIVA>");
        //        xmlReporteNoResumen = xmlReporteNoResumen.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<VaciadoBalance>");
        //        xmlReporteNoResumen = xmlReporteNoResumen.Replace("</Val></Valores>", "</VaciadoBalance>");
        
        //        xmlIndicFinancieros = xmlIndicFinancieros.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<IndicadoresFinancierosPadre>");
        //        xmlIndicFinancieros = xmlIndicFinancieros.Replace("</Val></Valores>", "</IndicadoresFinancierosPadre>");

        //        xml = xml.Replace("<ResumenIVA>-</ResumenIVA>", xmlReporteIVACompraVentas);
        //        xml = xml.Replace("<VaciadoBalance>--</VaciadoBalance>", xmlReporteNoResumen);
        //        xml = xml.Replace("<IndicadoresFinancierosPadre>-</IndicadoresFinancierosPadre>", xmlIndicFinancieros);

        //        /////Gráficos
        //        String nombreGrafico1 = String.Empty;
        //        String nombreGrafico2 = String.Empty;
        //        String rutaGraficos = String.Empty;

        //        nombreGrafico1 = "G1" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + datos["IdEmpresa"].ToString();
        //        nombreGrafico2 = "G2" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + datos["IdEmpresa"].ToString();
        //        rutaGraficos = "http://localhost:25698/graficas/";

        //        xml = xml.Replace("<Grafico1></Grafico1>", "<Grafico1>" + rutaGraficos + nombreGrafico1 + ".png" + "</Grafico1>");
        //        xml = xml.Replace("<Grafico2></Grafico2>", "<Grafico2>" + rutaGraficos + nombreGrafico2 + ".png" + "</Grafico2>");

        //        crearGrafico1(res1.Tables[3], nombreGrafico1);
        //        crearGrafico2(res1.Tables[3], nombreGrafico2);

        //        XDocument newTree = new XDocument();
        //        XslCompiledTransform xsltt = new XslCompiledTransform();

        //        using (XmlWriter writer = newTree.CreateWriter())
        //        {
        //            xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Propuesta_Afianzamiento" + ".xslt");
        //        }
        //        using (var sw = new StringWriter())
        //        using (var sr = new StringReader(xml))
        //        using (var xr = XmlReader.Create(sr))
        //        {
        //            xsltt.Transform(xr, null, sw);
        //            html = sw.ToString();
        //        }
        //        try
        //        {
        //            sDocumento.Append(html);
        //            return util.ConvertirAPDF_Control(sDocumento);
        //        }
        //        catch { }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //    }
        //    return null;
        //}


        public byte[] GenerarReporteOld(string Reporte, Dictionary<string, string> datos, string ejecutivo)
        {
            try
            {
                switch (Reporte)
                {
                    case "Propuesta_Afianzamiento_Express":
                        return generarPaf2Express(datos, ejecutivo);
                        //break;

                    case "Propuesta_Afianzamiento_Old":
                        return generarPaf1(datos, ejecutivo);
                        //break;

                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }


        private byte[] generarPaf1(Dictionary<string, string> datos, string ejecutivo)
        {
            String xml = String.Empty;
            DataSet res1 = new DataSet();
            Utilidades util = new Utilidades();

            res1 = ConsultaReporteBD_Old(datos["IdEmpresa"], datos["IdPaf"], "Systema", "Adminitrator", "PAF");
            xml = GenerarXMLContratoSubFianza(res1, ejecutivo, datos["Rank"], datos["Clasi"], datos["Ventas"]);

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Propuesta_Afianzamiento_Old" + ".xslt");
            }
            using (var sw = new StringWriter())
            using (var sr = new StringReader(xml))
            using (var xr = XmlReader.Create(sr))
            {
                xsltt.Transform(xr, null, sw);
                html = sw.ToString();
            }
            try
            {
                sDocumento.Append(html);
                return util.ConvertirAPDF_Control(sDocumento);
            }
            catch {
                return null;
            }
        }

        private byte[] generarPaf2Express(Dictionary<string, string> datos, string ejecutivo)
        {
            String xml = String.Empty;
            DataSet res1 = new DataSet();
            Utilidades util = new Utilidades();

            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdPaf"], 2, "Adminitrador", "ReportePAF");
            return GenerarPdfPAF(res1);
        }

        public byte[] GenerarPdfPAF(DataSet dsPaf)
        {
            Utilidades util = new Utilidades();
            byte[] archivoSalida = null;
            string resultadoDoc = string.Empty;
            string idEmpresa = string.Empty;
            string idPAF = string.Empty;
            string idImpresionPAF = string.Empty;
            string xml = string.Empty;
            string xmlReporteNoResumen = string.Empty;
            string xmlReporteIVACompraVentas = string.Empty;
            string xmlIndicFinancieros = string.Empty;

            if (dsPaf.Tables.Count == 6)
            {
                idEmpresa = dsPaf.Tables[0].Rows[0][1].ToString();
                idPAF = dsPaf.Tables[0].Rows[0][2].ToString();
                idImpresionPAF = dsPaf.Tables[0].Rows[0][3].ToString();

                int cantEmpresas = int.Parse(dsPaf.Tables[0].Rows[0][0].ToString());

                for (int i = 0; i < dsPaf.Tables[1].Rows.Count; i++)
                {
                    xml = xml + dsPaf.Tables[1].Rows[i][0];
                }
                for (int i = 0; i < dsPaf.Tables[2].Rows.Count; i++)
                {
                    xmlReporteIVACompraVentas = xmlReporteIVACompraVentas + dsPaf.Tables[2].Rows[i][0];
                }

                for (int i = 0; i < dsPaf.Tables[4].Rows.Count; i++)
                {
                    xmlReporteNoResumen = xmlReporteNoResumen + dsPaf.Tables[4].Rows[i][0];
                }

                for (int i = 0; i < dsPaf.Tables[5].Rows.Count; i++)
                {
                    xmlIndicFinancieros = xmlIndicFinancieros + dsPaf.Tables[5].Rows[i][0];
                }

                xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<ResumenIVA>");
                xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("</Val></Valores>", "</ResumenIVA>");
                xmlReporteNoResumen = xmlReporteNoResumen.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<VaciadoBalance>");
                xmlReporteNoResumen = xmlReporteNoResumen.Replace("</Val></Valores>", "</VaciadoBalance>");
                
                xmlIndicFinancieros = xmlIndicFinancieros.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<IndicadoresFinancierosPadre>");
                xmlIndicFinancieros = xmlIndicFinancieros.Replace("</Val></Valores>", "</IndicadoresFinancierosPadre>");

                if (xml.Contains("<ResumenIVA>"))
                    xml = xml.Replace("<ResumenIVA>-</ResumenIVA>", xmlReporteIVACompraVentas);
                if (xml.Contains("<VaciadoBalance>"))
                    xml = xml.Replace("<VaciadoBalance>--</VaciadoBalance>", xmlReporteNoResumen);
                if (xml.Contains("<IndicadoresFinancierosPadre>"))
                    xml = xml.Replace("<IndicadoresFinancierosPadre>-</IndicadoresFinancierosPadre>", xmlIndicFinancieros);

                /////Gráficos
                string nombreGrafico1 = string.Empty;
                string rutaGraficos = @"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/";

                nombreGrafico1 = "G1" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + idEmpresa;

                if (xml.Contains("<Grafico1>"))
                {
                    crearGrafico1(dsPaf.Tables[3], nombreGrafico1);         
                    xml = xml.Replace("<Grafico1>-</Grafico1>", "<Grafico1>" + rutaGraficos + nombreGrafico1 + ".png" + "</Grafico1>");
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Propuesta_Afianzamiento_Express" + ".xslt");
                }
                string html = string.Empty;
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    StringBuilder sDocumento = new StringBuilder();
                    DateTime dtime = new DateTime();
                    dtime = DateTime.Now;
                    sDocumento.Append(html);
                    archivoSalida =  util.ConvertirAPDF_Control(sDocumento);
                    string nombreArchivo = "ReportePAF_" + idPAF + "_" + dtime.Year.ToString() + dtime.Month.ToString() + dtime.Day + "_" + dtime.Hour.ToString() + dtime.Minute.ToString() + ".PDF";

                    bool actualizacionCorrecta = ActualizarPafExpress(int.Parse(idImpresionPAF), 0, "", "04", "CALCULADO"); //Cambiar estado a calculado
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                //ObtenerDatos.ActualizarImpresionPAFNoCalculado(int.Parse(idImpresionPAF));
            }

            return archivoSalida;
        }

        public Boolean ActualizarPafExpress(int idEmpresa, int idPAF, string idUsuario, string opcion, string estado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = idPAF;
                SqlParametros[2] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = opcion;
                SqlParametros[4] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[4].Value = estado;
                return AD_Global.ejecutarAccion("GestionImpresionPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ConsultaReporteBD_Old(string idEmpresa, string idPaf, string usuario, string perfil, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean GestionPAF(Dictionary<string, string> datos, string Garantias, string OperacionesNueva, string OperacionesVigente, string idUsuario, string CapacidadPago, string EvaluacionRiesgo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[28];
                SqlParametros[0] = new SqlParameter("@IdScoring", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(datos["IdScoring"]);
                SqlParametros[1] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(datos["IdEmpresa"]);
                SqlParametros[2] = new SqlParameter("@ObservacionPropuesta", SqlDbType.NVarChar);
                SqlParametros[2].Value = datos["ObservacionPropuesta"];
                SqlParametros[3] = new SqlParameter("@EstadoLinea", SqlDbType.NVarChar);
                SqlParametros[3].Value = datos["EstadoLinea"];
                SqlParametros[4] = new SqlParameter("@NivelAtribucion", SqlDbType.Int);
                SqlParametros[4].Value = datos["NivelAtribucion"];
                SqlParametros[5] = new SqlParameter("@Oficina", SqlDbType.Int);
                SqlParametros[5].Value = datos["Oficina"];
                SqlParametros[6] = new SqlParameter("@Fecha", SqlDbType.Date);
                SqlParametros[6].Value = datos["Fecha"];
                SqlParametros[7] = new SqlParameter("@FechaRevision", SqlDbType.Date);
                SqlParametros[7].Value = datos["FechaRevision"];
                SqlParametros[8] = new SqlParameter("@Ejecutivo", SqlDbType.NVarChar);
                SqlParametros[8].Value = datos["Ejecutivo"];
                SqlParametros[9] = new SqlParameter("@ObservacionComite", SqlDbType.NVarChar);
                SqlParametros[9].Value = datos["ObservacionesComite"];
                SqlParametros[10] = new SqlParameter("@Estatus", SqlDbType.Int);
                if (datos["estado"].ToString() != "2")
                    SqlParametros[10].Value = Constantes.OPCION.INSERTARPAF;
                else
                    SqlParametros[10].Value = Constantes.OPCION.PAFFINAL;
                SqlParametros[11] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[11].Value = int.Parse(datos["IdPaf"]);
                SqlParametros[12] = new SqlParameter("@TotalAprobadoREUF", SqlDbType.Float);
                SqlParametros[12].Value = float.Parse(datos["TotalAprobado"]);
                SqlParametros[13] = new SqlParameter("@TotalVigenteREUF", SqlDbType.Float);
                SqlParametros[13].Value = float.Parse(datos["TotalVigente"]);
                SqlParametros[14] = new SqlParameter("@TotalPropuestoREUF", SqlDbType.Float);
                SqlParametros[14].Value = float.Parse(datos["TotalPropuesto"]);

                SqlParametros[15] = new SqlParameter("@ValorUF", SqlDbType.Float);
                SqlParametros[15].Value = float.Parse(datos["ValorUF"]);
                SqlParametros[16] = new SqlParameter("@ValorUSD", SqlDbType.Float);
                SqlParametros[16].Value = float.Parse(datos["ValorDolar"]);
                SqlParametros[17] = new SqlParameter("@VentasMoviles", SqlDbType.Float);
                SqlParametros[17].Value = float.Parse(datos["VentasMoviles"]);

                SqlParametros[18] = new SqlParameter("@CoberturaVigenteComercial", SqlDbType.Float);
                SqlParametros[18].Value = float.Parse(datos["CoberturaVigenteComercial"]);
                SqlParametros[19] = new SqlParameter("@CoberturaVigenteAjustado", SqlDbType.Float);
                SqlParametros[19].Value = float.Parse(datos["CoberturaVigenteAjustado"]);
                SqlParametros[20] = new SqlParameter("@CoberturaGlobalComercial", SqlDbType.Float);
                SqlParametros[20].Value = float.Parse(datos["CoberturaGlobalComercial"]);
                SqlParametros[21] = new SqlParameter("@CoberturaGlobalAjustado", SqlDbType.Float);
                SqlParametros[21].Value = float.Parse(datos["CoberturaGlobalAjustado"]);

                SqlParametros[22] = new SqlParameter("@xmlDataGarantia", SqlDbType.NVarChar);
                SqlParametros[22].Value = Garantias;

                SqlParametros[23] = new SqlParameter("@xmlDataOperacionesNuevas", SqlDbType.NVarChar);
                SqlParametros[23].Value = OperacionesNueva;

                SqlParametros[24] = new SqlParameter("@xmlDataOperacionesVigentes", SqlDbType.NVarChar);
                SqlParametros[24].Value = OperacionesVigente;

                SqlParametros[25] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[25].Value = idUsuario;
                SqlParametros[26] = new SqlParameter("@CapacidadPago", SqlDbType.NVarChar);
                SqlParametros[26].Value = CapacidadPago;
                SqlParametros[27] = new SqlParameter("@EvaluacionRiesgo", SqlDbType.NVarChar);
                SqlParametros[27].Value = EvaluacionRiesgo;

                return AD_Global.ejecutarAccion("GestionPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarImpresionPAF(int idEmpresa, int idPAF, string idUsuario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = idPAF;
                SqlParametros[2] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = Constantes.OPCIONIMPRESIONPAF.INSERTAR;
                SqlParametros[4] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[4].Value = Constantes.OPCIONIMPRESIONPAF.EDOPENDIENTE;
                return AD_Global.ejecutarAccion("GestionImpresionPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public void ListarUsuarios(ref DropDownList combo)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["AprobadoresPAF"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "";
                combo.DataSource = Lista.GetItems(oQuery).GetDataTable();
                combo.DataTextField = Lista.GetItems(oQuery).Fields["Usuario"].ToString();
                combo.DataValueField = Lista.GetItems(oQuery).Fields["ID"].ToString();
                combo.DataBind();
                combo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione--", "-1"));
                combo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public DataSet ConsutarDatosEmpresaPAF(string idEmpresa, string idPaf, string usuario, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("ConsutarDatosEmpresaPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ObtenerDatosEmpresaPafActual(string idEmpresa, string idPaf, string usuario, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("ConsutarDatosEmpresaPAFActuales", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ConsutarDatosPAF(string idEmpresa, string usuario, string perfil, string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;

                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(idOperacion);

                dt = AD_Global.ejecutarConsulta("ConsutarDatosPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public bool RegistrarAprobadoresPAF(Dictionary<string, string> datos, string edoPAF, string fecAprobacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(datos["IdPaf"]);
                SqlParametros[1] = new SqlParameter("@IdSP1", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(datos["IdSP1"]);
                SqlParametros[2] = new SqlParameter("@Nombre1", SqlDbType.NVarChar);
                SqlParametros[2].Value = datos["Nombre1"];
                SqlParametros[3] = new SqlParameter("@IdSP2", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(datos["IdSP2"]);
                SqlParametros[4] = new SqlParameter("@Nombre2", SqlDbType.NVarChar);
                SqlParametros[4].Value = datos["Nombre2"];
                SqlParametros[5] = new SqlParameter("@IdSP3", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(datos["IdSP3"]);
                SqlParametros[6] = new SqlParameter("@Nombre3", SqlDbType.NVarChar);
                SqlParametros[6].Value = datos["Nombre3"];
                SqlParametros[7] = new SqlParameter("@IdSP4", SqlDbType.Int);
                SqlParametros[7].Value = int.Parse(datos["IdSP4"]);
                SqlParametros[8] = new SqlParameter("@Nombre4", SqlDbType.NVarChar);
                SqlParametros[8].Value = datos["Nombre4"];
                SqlParametros[9] = new SqlParameter("@IdSP5", SqlDbType.Int);
                SqlParametros[9].Value = int.Parse(datos["IdSP5"]);
                SqlParametros[10] = new SqlParameter("@Nombre5", SqlDbType.NVarChar);
                SqlParametros[10].Value = datos["Nombre5"];
                SqlParametros[11] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[11].Value = 1;
                SqlParametros[12] = new SqlParameter("@EdoPAF", SqlDbType.NVarChar);
                SqlParametros[12].Value = edoPAF;
                SqlParametros[13] = new SqlParameter("@FecAprobacion", SqlDbType.NVarChar);
                SqlParametros[13].Value = fecAprobacion;

                return AD_Global.ejecutarAccion("GestionAprobadoresPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean sinDocumentosContables(string IdEmpresa, string usuario, string perfil, string IdPaf, Boolean valor)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(IdEmpresa);
                SqlParametros[1] = new SqlParameter("@User", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;
                SqlParametros[3] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(IdPaf);

                SqlParametros[4] = new SqlParameter("@Accion", SqlDbType.Int);
                SqlParametros[4].Value = "1";

                SqlParametros[5] = new SqlParameter("@valor", SqlDbType.Bit);
                SqlParametros[5].Value = valor;

                return AD_Global.ejecutarAccion("sinDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ConsultaReporteBD(string idEmpresa, string idPaf, int idOpcion, string usuario, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idPaf);
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idEmpresa);
                SqlParametros[2] = new SqlParameter("@idOpcion", SqlDbType.Int);
                SqlParametros[2].Value = idOpcion;

                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }


        //private byte[] crearGrafico1(DataTable dt, string nombreGrafico1)
        private void crearGrafico1(DataTable dt, string nombreGrafico1)
        {
            //Gráfico 1;
            double[] anioAct = new double[12];
            double[] anio1 = new double[12];
            double[] anio2 = new double[12];
            double[] anio3 = new double[12];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                anioAct[i] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
                anio1[i] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
                anio2[i] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
                anio3[i] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
            }
            string[] meses = new string[12];
            int nroMes = 1;
            CultureInfo culture = new CultureInfo("es-ES");
            for (int i = 0; i < meses.Length; i++)
            {
                meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
                nroMes++;
            }

            Chart grafico1 = new Chart();
            grafico1.ChartAreas.Add(new ChartArea());
            grafico1.ChartAreas[0].AxisX.Interval = 1;
            grafico1.Width = 710;
            grafico1.Height = 300;
            Series serieAnioAct = new Series();
            Series serieAnio1 = new Series();
            Series serieAnio2 = new Series();
            Series serieAnio3 = new Series();

            serieAnioAct.Points.DataBindXY(meses, anioAct);
            serieAnio1.Points.DataBindXY(meses, anio1);
            serieAnio2.Points.DataBindXY(meses, anio2);
            serieAnio3.Points.DataBindXY(meses, anio3);

            serieAnioAct.Name = (DateTime.Now.Year).ToString();
            serieAnio1.Name = (DateTime.Now.Year - 1).ToString();
            serieAnio2.Name = (DateTime.Now.Year - 2).ToString();
            serieAnio3.Name = (DateTime.Now.Year - 3).ToString();

            //#99BE1A verde
            //#E85426 nar
            //#1E9CD6 azu
            //#9966CC mor
            serieAnioAct.Color = ColorTranslator.FromHtml("#1E9CD6");
            serieAnio1.Color = ColorTranslator.FromHtml("#9966CC");
            serieAnio2.Color = ColorTranslator.FromHtml("#E85426");
            serieAnio3.Color = ColorTranslator.FromHtml("#99BE1A");

            serieAnioAct.BorderWidth = 3;
            serieAnio1.BorderWidth = 3;
            serieAnio2.BorderWidth = 3;
            serieAnio3.BorderWidth = 3;

            serieAnioAct.ChartType = SeriesChartType.Spline;
            serieAnioAct.BorderDashStyle = ChartDashStyle.Dash;

            serieAnio1.ChartType = SeriesChartType.Spline;
            serieAnio1.BorderDashStyle = ChartDashStyle.Dot;

            serieAnio2.ChartType = SeriesChartType.Spline;
            serieAnio2.BorderDashStyle = ChartDashStyle.Solid;

            serieAnio3.ChartType = SeriesChartType.Spline;
            serieAnio3.BorderDashStyle = ChartDashStyle.DashDotDot;

            Title titulo = new Title();
            titulo.Name = "Titulo1";
            titulo.Text = "Ventas Mensuales por Período (M$)";
            titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
            grafico1.Titles.Add(titulo);

            Legend lg = new Legend();
            lg.Name = "Leyenda1";
            lg.Docking = Docking.Bottom;
            lg.Alignment = StringAlignment.Center;
            grafico1.Legends.Add(lg);
            serieAnioAct.Legend = "Leyenda1";

            grafico1.Series.Add(serieAnioAct);
            grafico1.Series.Add(serieAnio1);
            grafico1.Series.Add(serieAnio2);
            grafico1.Series.Add(serieAnio3);

            //using (var chartimage = new MemoryStream())
            //{
            //    grafico1.SaveImage(chartimage, ChartImageFormat.Png);
            //    return chartimage.GetBuffer();
            //}
            
            grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico1 + ".png", ChartImageFormat.Png);
        }

        private void crearGrafico2(DataTable dt, string nombreGrafico2)
        {
            double[] ventas = new double[48];

            int index = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
                index++;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
                index++;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
                index++;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
                index++;
            }

            string[] meses = new string[48];
            int nroMes = 1;
            CultureInfo culture = new CultureInfo("es-ES");
            for (int i = 0; i < meses.Length; i++)
            {
                if (nroMes == 13)
                {
                    nroMes = 1;
                }
                meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
                nroMes++;
            }

            Chart grafico1 = new Chart();
            grafico1.ChartAreas.Add(new ChartArea());
            grafico1.ChartAreas[0].AxisX.Interval = 1;
            grafico1.Width = 710;
            grafico1.Height = 300;
            Series serieVentas = new Series();

            serieVentas.Points.DataBindXY(meses, ventas);
            serieVentas.Name = (DateTime.Now.Year - 3).ToString() + ", " + (DateTime.Now.Year - 2).ToString() + ", " + (DateTime.Now.Year - 1).ToString() + ", " + (DateTime.Now.Year).ToString();
            serieVentas.Color = Color.OrangeRed;//ColorTranslator.FromHtml("#1E9CD6");
            serieVentas.BorderWidth = 3;
            serieVentas.ChartType = SeriesChartType.Spline;

            Title titulo = new Title();
            titulo.Name = "Titulo1";
            titulo.Text = "Ventas Todos los Períodos (M$)";
            titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
            grafico1.Titles.Add(titulo);

            Legend lg = new Legend();
            lg.Name = "Leyenda1";
            lg.Docking = Docking.Bottom;
            lg.Alignment = StringAlignment.Center;
            grafico1.Legends.Add(lg);
            serieVentas.Legend = "Leyenda1";

            grafico1.Series.Add(serieVentas);
            //grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico2 + ".png", ChartImageFormat.Png);
        }

        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;
        public string GenerarXMLContratoSubFianza(DataSet res1, string Ejecutivo, string rank, string clasi, string Ventas)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            XmlNode RespNodeCkb;
            RespNodeCkb = doc.CreateElement("General");

            XmlNode nodo = doc.CreateElement("RazonSocial");
            nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo);

            XmlNode nodo1 = doc.CreateElement("Direccion");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["direccion"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            XmlNode nodo2 = doc.CreateElement("Comuna");
            nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescComuna"].ToString()));
            RespNodeCkb.AppendChild(nodo2);


            for (int i = 0; i < res1.Tables[3].Rows.Count; i++)
            {
                DateTime fecha = DateTime.MinValue;

                DateTime fechaAux = res1.Tables[3].Rows[i]["FechaCreacion"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(res1.Tables[3].Rows[i]["FechaCreacion"].ToString());

                if (fechaAux >= fecha)
                {
                    XmlNode nodo122 = doc.CreateElement("Canal");
                    nodo122.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Canal"].ToString()));
                    RespNodeCkb.AppendChild(nodo122);
                    fecha = res1.Tables[3].Rows[i]["FechaCreacion"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(res1.Tables[3].Rows[i]["FechaCreacion"].ToString());
                }
            }

            string aux = res1.Tables[0].Rows[0]["GIROACT"].ToString();
            XmlNode nodo3 = doc.CreateElement("Actividad");
            nodo3.AppendChild(doc.CreateTextNode(aux.Substring(2, aux.Length - 2)));//.Substring(2, aux.Length-1)));
            RespNodeCkb.AppendChild(nodo3);

            DateTime thisDate = DateTime.Now;
            CultureInfo culture = new CultureInfo("es-ES");

            XmlNode nodo4 = doc.CreateElement("FechaHoy");
            nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
            RespNodeCkb.AppendChild(nodo4);

            XmlNode nodo5 = doc.CreateElement("RutEmpresa");
            nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString() + "-" + res1.Tables[0].Rows[0]["DivRut"].ToString()));
            RespNodeCkb.AppendChild(nodo5);

            XmlNode nodo6 = doc.CreateElement("Region");
            nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRegion"].ToString()));
            RespNodeCkb.AppendChild(nodo6);

            XmlNode nodo7 = doc.CreateElement("Telefono");
            nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TelFijo1"].ToString()));
            RespNodeCkb.AppendChild(nodo7);

            XmlNode nodo8 = doc.CreateElement("Ciudad");
            nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescProvincia"].ToString()));
            RespNodeCkb.AppendChild(nodo8);

            XmlNode nodoA1 = doc.CreateElement("AnoAct");
            nodoA1.AppendChild(doc.CreateTextNode(DateTime.Now.Year.ToString()));
            RespNodeCkb.AppendChild(nodoA1);

            XmlNode nodoA2 = doc.CreateElement("Ano-1");
            nodoA2.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 1).ToString()));
            RespNodeCkb.AppendChild(nodoA2);

            XmlNode nodoA3 = doc.CreateElement("Ano-2");
            nodoA3.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 2).ToString()));
            RespNodeCkb.AppendChild(nodoA3);

            XmlNode nodoA4 = doc.CreateElement("Ano-3");
            nodoA4.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 3).ToString()));
            RespNodeCkb.AppendChild(nodoA4);

            // asi con todos los nodos de3 este nievel
            RespNode.AppendChild(RespNodeCkb);
            ValoresNode.AppendChild(RespNode);

            //InsertarDatosPAF de la PAF
            XmlNode RespNodePropuestaAfianzamiento;
            //XmlNode root = doc.DocumentElement;
            RespNodePropuestaAfianzamiento = doc.CreateElement("PAF");

            XmlNode nodoP = doc.CreateElement("Oficina");
            nodoP.AppendChild(doc.CreateTextNode("1"));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP);

            XmlNode nodoP1 = doc.CreateElement("Fecha");
            nodoP1.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["fecha"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP1);

            XmlNode nodoP2 = doc.CreateElement("FechaRevision");
            nodoP2.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["FechaRevision"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP2);

            XmlNode nodoP3 = doc.CreateElement("Ejecutivo");
            nodoP3.AppendChild(doc.CreateTextNode(Ejecutivo));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP3);

            XmlNode nodoP4 = doc.CreateElement("EstadoLinea");
            nodoP4.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["EstadoLinea"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP4);

            XmlNode nodoP5 = doc.CreateElement("NivelAtribucion");
            nodoP5.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["NivelAtribucion"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP5);

            XmlNode nodoP6 = doc.CreateElement("ValorRank");
            nodoP6.AppendChild(doc.CreateTextNode(rank));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP6);

            XmlNode nodoP116 = doc.CreateElement("Clasificacion");
            nodoP116.AppendChild(doc.CreateTextNode(clasi));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP116);

            if (res1.Tables[5].Rows.Count > 0)
            {
                XmlNode nodoP7 = doc.CreateElement("ValorPond");
                nodoP7.AppendChild(doc.CreateTextNode(res1.Tables[5].Rows[0]["ValorPond"].ToString().Replace(".", ",")));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP7);

                XmlNode nodoP8 = doc.CreateElement("FechaScoring");
                nodoP8.AppendChild(doc.CreateTextNode((res1.Tables[5].Rows[0]["FecCreacion"].ToString())));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP8);
            }
            else
            {
                XmlNode nodoP7 = doc.CreateElement("ValorPond");
                nodoP7.AppendChild(doc.CreateTextNode(""));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP7);

                XmlNode nodoP8 = doc.CreateElement("FechaScoring");
                nodoP8.AppendChild(doc.CreateTextNode(("")));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP8);
            }

            XmlNode nodoP9 = doc.CreateElement("VentasMoviles");
            nodoP9.AppendChild(doc.CreateTextNode(Ventas));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP9);

            XmlNode nodoP10 = doc.CreateElement("ObservacionComite");
            nodoP10.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["ObservacionComite"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP10);

            XmlNode nodoP11 = doc.CreateElement("IdPaf");
            nodoP11.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["IdPaf"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP11);

            if (res1.Tables[6].Rows.Count > 0)
            {
                XmlNode nodoP12 = doc.CreateElement("Aprobador1");
                nodoP12.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP12);

                XmlNode nodoP13 = doc.CreateElement("Aprobador2");
                nodoP13.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador1"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP13);

                XmlNode nodoP14 = doc.CreateElement("Aprobador3");
                nodoP14.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador2"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP14);

                XmlNode nodoP15 = doc.CreateElement("Aprobador4");
                nodoP15.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador3"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP15);

                XmlNode nodoP16 = doc.CreateElement("Aprobador5");
                nodoP16.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador4"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP16);
            }
            ValoresNode.AppendChild(RespNodePropuestaAfianzamiento);

            //datos gaantia

            XmlNode RespNodeG;
            root = doc.DocumentElement;
            RespNodeG = doc.CreateElement("Garantias");
            double TotalGarantiaComercial = 0;
            double TotalGarantiaAjustado = 0;
            double TotalCoberturaAjustadoVigente = 0;
            double TotalCoberturaComercialVigente = 0;
            double TotalCoberturaAjustadoNuevo = 0;
            double TotalGarantiaNuevaComercial = 0;
            double coberturaCertificado = 0;

            for (int i = 0; i < res1.Tables[1].Rows.Count; i++)
            {
                if (int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 52 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 53 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 54 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 59)
                {
                    TotalGarantiaComercial = TotalGarantiaComercial + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                    TotalGarantiaAjustado = TotalGarantiaAjustado + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");

                    if (res1.Tables[1].Rows[i]["DescEstado"].ToString() == "Constituida" || res1.Tables[1].Rows[i]["DescEstado"].ToString() == "Tramite")
                    {
                        TotalCoberturaAjustadoVigente = TotalCoberturaAjustadoVigente + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        TotalCoberturaComercialVigente = TotalCoberturaComercialVigente + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                    }
                    else
                    {
                        TotalCoberturaAjustadoNuevo = TotalCoberturaAjustadoNuevo + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        TotalGarantiaNuevaComercial = TotalGarantiaNuevaComercial + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                    }
                }

                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Garantia");

                nodo = doc.CreateElement("N");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["N"].ToString()));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("TipoGarantia");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Tipo Garantia"].ToString()));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("Descripcion");
                nodo2.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Descripción"].ToString()));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("Comentarios");
                nodo3.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Comentarios"].ToString().Trim()));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("TipoMO");
                nodo4.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Tipo Moneda"].ToString()));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("ValorComercial");
                nodo5.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Valor Comercial"].ToString()));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("ValorAjustado");
                nodo6.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString()));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("AplicaSeguro");
                if (!string.IsNullOrEmpty(res1.Tables[1].Rows[i]["Seguro"].ToString()))
                    nodo7.AppendChild(doc.CreateTextNode((Boolean)res1.Tables[1].Rows[i]["Seguro"] ? "Aplica" : "No Aplica"));
                else
                    nodo7.AppendChild(doc.CreateTextNode("No se ha ingresado una opcion de seguro"));

                RespNodeGarantia.AppendChild(nodo7);

                RespNodeG.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeG);


            //aval -----------------------------------
            XmlNode RespNodeA;
            root = doc.DocumentElement;
            RespNodeA = doc.CreateElement("OperacionesVigentes");

            double totalAprobadoCLP = 0;
            double totalAprobadoCLP_Certificado = 0;
            double totalVigenteCLP = 0;
            double totalVigenteCLP_Certificado = 0;
            double totalPropuestoCLP = 0;
            double totalPropuestoCLP_Certificado = 0;

            double totalAprobadoUF = 0;
            double totalAprobadoUF_Certificado = 0;
            double totalVigenteUF = 0;
            double totalVigenteUF_Certificado = 0;
            double totalPropuestoUF = 0;
            double totalPropuestoUF_Certificado = 0;

            double totalAprobadoUSD = 0;
            double totalAprobadoUSD_Certificado = 0;
            double totalVigenteUSD = 0;
            double totalVigenteUSD_Certificado = 0;
            double totalPropuestoUSD = 0;
            double totalPropuestoUSD_Certificado = 0;

            //OPERACIONES VIGENTES
            for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["CoberturaCertificado"].ToString()))
                    coberturaCertificado = 1;
                else
                    coberturaCertificado = double.Parse(res1.Tables[2].Rows[i]["CoberturaCertificado"].ToString()) / 100;

                if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "UF")
                {
                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        totalAprobadoUF = totalAprobadoUF + 0;
                        totalAprobadoUF_Certificado = totalAprobadoUF_Certificado + 0;
                    }
                    else
                    {
                        totalAprobadoUF_Certificado = totalAprobadoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                        totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
                    {
                        totalVigenteUF = totalVigenteUF + 0;
                        totalVigenteUF_Certificado = totalVigenteUF_Certificado + 0;
                    }
                    else
                    {
                        totalVigenteUF_Certificado = totalVigenteUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                        totalVigenteUF = totalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        totalPropuestoUF = totalPropuestoUF + 0;
                        totalPropuestoUF_Certificado = totalPropuestoUF_Certificado + 0;
                    }
                    else
                    {
                        totalPropuestoUF_Certificado = totalPropuestoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                        totalPropuestoUF = totalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                    }
                }

                if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "CLP")
                {
                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        totalAprobadoCLP = totalAprobadoCLP + 0;
                        totalAprobadoCLP_Certificado = totalAprobadoCLP_Certificado + 0;
                    }
                    else
                    {
                        totalAprobadoCLP = totalAprobadoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        totalAprobadoCLP_Certificado = totalAprobadoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
                    {
                        totalVigenteCLP = totalVigenteCLP + 0;
                        totalVigenteCLP_Certificado = totalVigenteCLP_Certificado + 0;
                    }
                    else
                    {
                        totalVigenteCLP = totalVigenteCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        totalVigenteCLP_Certificado = totalVigenteCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        totalPropuestoCLP = totalPropuestoCLP + 0;
                        totalPropuestoCLP_Certificado = totalPropuestoCLP_Certificado + 0;
                    }
                    else
                    {
                        totalPropuestoCLP = totalPropuestoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        totalPropuestoCLP_Certificado = totalPropuestoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "USD")
                {
                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        totalAprobadoUSD = totalAprobadoUSD + 0;
                        totalAprobadoUSD_Certificado = totalAprobadoUSD_Certificado + 0;
                    }
                    else
                    {
                        totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        totalAprobadoUSD_Certificado = totalAprobadoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
                    {
                        totalVigenteUSD = totalVigenteUSD + 0;
                        totalVigenteUSD_Certificado = totalVigenteUSD_Certificado + 0;
                    }
                    else
                    {
                        totalVigenteUSD = totalVigenteUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        totalVigenteUSD_Certificado = totalVigenteUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        totalPropuestoUSD = totalPropuestoUSD + 0;
                        totalPropuestoUSD_Certificado = totalPropuestoUSD_Certificado + 0;
                    }
                    else
                    {
                        totalPropuestoUSD = totalPropuestoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        totalPropuestoUSD_Certificado = totalPropuestoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                XmlNode RespNodeAval;
                RespNodeAval = doc.CreateElement("OperacionVigente");

                nodo = doc.CreateElement("N");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["N"].ToString()));
                RespNodeAval.AppendChild(nodo);

                nodo1 = doc.CreateElement("TipoFinanciamiento");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Tipo Financiamiento"].ToString()));
                RespNodeAval.AppendChild(nodo1);

                nodo2 = doc.CreateElement("Producto");
                nodo2.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Producto"].ToString()));
                RespNodeAval.AppendChild(nodo2);

                nodo3 = doc.CreateElement("Finalidad");
                nodo3.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Finalidad"].ToString()));
                RespNodeAval.AppendChild(nodo3);

                nodo4 = doc.CreateElement("Plazo");
                nodo4.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Plazo"].ToString()));
                RespNodeAval.AppendChild(nodo4);

                nodo5 = doc.CreateElement("Comision");
                nodo5.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Comisión Min. %"].ToString()));
                RespNodeAval.AppendChild(nodo5);

                nodo6 = doc.CreateElement("Seguro");
                nodo6.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Seguro"].ToString()));
                RespNodeAval.AppendChild(nodo6);

                nodo7 = doc.CreateElement("ComisionCLP");
                nodo7.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Comisión"].ToString()));
                RespNodeAval.AppendChild(nodo7);

                nodo8 = doc.CreateElement("MontoCredito");
                nodo8.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Crédito"].ToString()));
                RespNodeAval.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("TipoMO");
                nodo9.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Tipo Moneda"].ToString()));
                RespNodeAval.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("MontoAprobado");
                nodo10.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()));
                RespNodeAval.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("MontoVigente");
                nodo11.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()));
                RespNodeAval.AppendChild(nodo11);

                XmlNode nodo12 = doc.CreateElement("MontoPropuesto");
                nodo12.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()));
                RespNodeAval.AppendChild(nodo12);

                XmlNode nodo13 = doc.CreateElement("Horizonte");
                nodo13.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Horizonte"].ToString()));
                RespNodeAval.AppendChild(nodo13);

                XmlNode nodo14 = doc.CreateElement("CoberturaCertificado");
                nodo14.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["coberturaCertificado"].ToString()));
                RespNodeAval.AppendChild(nodo14);

                RespNodeA.AppendChild(RespNodeAval);
            }
            ValoresNode.AppendChild(RespNodeA);

            XmlNode RespNodeB;
            root = doc.DocumentElement;
            RespNodeB = doc.CreateElement("OperacionesNuevas");

            double NtotalAprobadoCLP = 0;
            double NtotalAprobadoCLP_Certificado = 0;
            double NtotalVigenteCLP = 0;
            double NtotalVigenteCLP_Certificado = 0;
            double NtotalPropuestoCLP = 0;
            double NtotalPropuestoCLP_Certificado = 0;

            double NtotalAprobadoUF = 0;
            double NtotalAprobadoUF_Certificado = 0;
            double NtotalVigenteUF = 0;
            double NtotalVigenteUF_Certificado = 0;
            double NtotalPropuestoUF = 0;

            double NtotalAprobadoUSD = 0;
            double NtotalAprobadoUSD_Certificado = 0;
            double NtotalVigenteUSD = 0;
            double NtotalVigenteUSD_Certificado = 0;
            double NtotalPropuestoUSD = 0;
            double NtotalPropuestoUSD_Certificado = 0;

            //OPERACIONES NUEVAS
            for (int i = 0; i < res1.Tables[3].Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["CoberturaCertificado"].ToString()))
                    coberturaCertificado = 1;
                else
                    coberturaCertificado = double.Parse(res1.Tables[3].Rows[i]["CoberturaCertificado"].ToString()) / 100;

                if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "UF")
                {
                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        NtotalAprobadoUF = NtotalAprobadoUF + 0;
                        NtotalAprobadoUF_Certificado = NtotalAprobadoUF_Certificado + 0;
                    }
                    else
                    {
                        NtotalAprobadoUF = NtotalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalAprobadoUF_Certificado = NtotalAprobadoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
                    {
                        NtotalVigenteUF = NtotalVigenteUF + 0;
                        NtotalVigenteUF_Certificado = NtotalVigenteUF_Certificado + 0;
                    }
                    else
                    {
                        NtotalVigenteUF = NtotalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalVigenteUF_Certificado = NtotalVigenteUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        NtotalPropuestoUF = NtotalPropuestoUF + 0;
                    }
                    else
                    {
                        NtotalPropuestoUF = NtotalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                    }
                }

                if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "CLP")
                {
                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        NtotalAprobadoCLP = NtotalAprobadoCLP + 0;
                        NtotalAprobadoCLP_Certificado = NtotalAprobadoCLP_Certificado + 0;
                    }
                    else
                    {
                        NtotalAprobadoCLP = NtotalAprobadoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalAprobadoCLP_Certificado = NtotalAprobadoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
                    {
                        NtotalVigenteCLP = NtotalVigenteCLP + 0;
                        NtotalVigenteCLP_Certificado = NtotalVigenteCLP_Certificado + 0;
                    }
                    else
                    {
                        NtotalVigenteCLP = NtotalVigenteCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalVigenteCLP_Certificado = NtotalVigenteCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        NtotalPropuestoCLP = NtotalPropuestoCLP + 0;
                        NtotalPropuestoCLP_Certificado = NtotalPropuestoCLP_Certificado + 0;
                    }
                    else
                    {
                        NtotalPropuestoCLP = NtotalPropuestoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalPropuestoCLP_Certificado = NtotalPropuestoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "USD")
                {
                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        NtotalAprobadoUSD = NtotalAprobadoUSD + 0;
                        NtotalAprobadoUSD_Certificado = NtotalAprobadoUSD_Certificado + 0;
                    }
                    else
                    {
                        NtotalAprobadoUSD = NtotalAprobadoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalAprobadoUSD_Certificado = NtotalAprobadoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
                    {
                        NtotalVigenteUSD = NtotalVigenteUSD + 0;
                        NtotalVigenteUSD_Certificado = NtotalVigenteUSD_Certificado + 0;
                    }
                    else
                    {
                        NtotalVigenteUSD = NtotalVigenteUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalVigenteUSD_Certificado = NtotalVigenteUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        NtotalPropuestoUSD = NtotalPropuestoUSD + 0;
                        NtotalPropuestoUSD_Certificado = NtotalPropuestoUSD_Certificado + 0;
                    }
                    else
                    {
                        NtotalPropuestoUSD = NtotalPropuestoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalPropuestoUSD_Certificado = NtotalPropuestoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                XmlNode RespNodeAval;
                RespNodeAval = doc.CreateElement("OperacionNueva");

                nodo = doc.CreateElement("N");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["N"].ToString()));
                RespNodeAval.AppendChild(nodo);

                nodo1 = doc.CreateElement("TipoFinanciamiento");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Tipo Financiamiento"].ToString()));
                RespNodeAval.AppendChild(nodo1);

                nodo2 = doc.CreateElement("Producto");
                nodo2.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Producto"].ToString()));
                RespNodeAval.AppendChild(nodo2);

                nodo3 = doc.CreateElement("Finalidad");
                nodo3.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Finalidad"].ToString()));
                RespNodeAval.AppendChild(nodo3);

                nodo4 = doc.CreateElement("Plazo");
                nodo4.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Plazo"].ToString()));
                RespNodeAval.AppendChild(nodo4);

                nodo5 = doc.CreateElement("Comision");
                nodo5.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Comisión Min. %"].ToString()));
                RespNodeAval.AppendChild(nodo5);

                nodo6 = doc.CreateElement("Seguro");
                nodo6.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Seguro"].ToString()));
                RespNodeAval.AppendChild(nodo6);

                nodo7 = doc.CreateElement("ComisionCLP");
                nodo7.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Comisión"].ToString()));
                RespNodeAval.AppendChild(nodo7);

                nodo8 = doc.CreateElement("MontoCredito");
                nodo8.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Crédito"].ToString()));
                RespNodeAval.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("TipoMO");
                nodo9.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Tipo Moneda"].ToString()));
                RespNodeAval.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("MontoAprobado");
                nodo10.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()));
                RespNodeAval.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("MontoVigente");
                nodo11.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()));
                RespNodeAval.AppendChild(nodo11);

                XmlNode nodo12 = doc.CreateElement("MontoPropuesto");
                nodo12.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()));
                RespNodeAval.AppendChild(nodo12);

                XmlNode nodo13 = doc.CreateElement("Horizonte");
                nodo13.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Horizonte"].ToString()));
                RespNodeAval.AppendChild(nodo13);

                XmlNode nodo14 = doc.CreateElement("CoberturaCertificado");
                nodo14.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["coberturaCertificado"].ToString().Trim()));
                RespNodeAval.AppendChild(nodo14);

                RespNodeB.AppendChild(RespNodeAval);
            }
            ValoresNode.AppendChild(RespNodeB);
            //tablas indicadores

            XmlNode RespNodeI1;
            root = doc.DocumentElement;
            RespNodeI1 = doc.CreateElement("Indicadores1");

            for (int i = 0; i < res1.Tables[7].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador1");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("POR1");
                nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("POR2");
                nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("POR3");
                nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("POR4");
                nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo7);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[7].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI1.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI1);

            XmlNode RespNodeI2;
            root = doc.DocumentElement;
            RespNodeI2 = doc.CreateElement("Indicadores2");

            for (int i = 0; i < res1.Tables[8].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador2");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("POR1");
                nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("POR2");
                nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("POR3");
                nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("POR4");
                nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo7);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[8].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI2.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI2);

            XmlNode RespNodeI3;
            root = doc.DocumentElement;
            RespNodeI3 = doc.CreateElement("Indicadores3");

            for (int i = 0; i < res1.Tables[9].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador3");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("POR1");
                nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("POR2");
                nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("POR3");
                nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("POR4");
                nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo7);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[9].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI3.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI3);

            XmlNode RespNodeI4;
            root = doc.DocumentElement;
            RespNodeI4 = doc.CreateElement("Indicadores4");

            for (int i = 0; i < res1.Tables[10].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador4");
                if (res1.Tables[10].Rows[i]["Cuenta"].ToString() != "Capital de Trabajo (M$)")
                {
                    nodo = doc.CreateElement("VAL1");
                    nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo);

                    nodo2 = doc.CreateElement("VAL2");
                    nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo2);

                    nodo4 = doc.CreateElement("VAL3");
                    nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo4);

                    nodo6 = doc.CreateElement("VAL4");
                    nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo6);

                    nodo8 = doc.CreateElement("TEXTO");
                    nodo8.AppendChild(doc.CreateTextNode((res1.Tables[10].Rows[i]["Cuenta"].ToString())));
                    RespNodeGarantia.AppendChild(nodo8);
                }
                else
                {
                    nodo = doc.CreateElement("VAL1");
                    nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo);

                    nodo2 = doc.CreateElement("VAL2");
                    nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo2);

                    nodo4 = doc.CreateElement("VAL3");
                    nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo4);

                    nodo6 = doc.CreateElement("VAL4");
                    nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo6);

                    nodo8 = doc.CreateElement("TEXTO");
                    nodo8.AppendChild(doc.CreateTextNode((res1.Tables[10].Rows[i]["Cuenta"].ToString())));
                    RespNodeGarantia.AppendChild(nodo8);
                }
                RespNodeI4.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI4);

            XmlNode RespNodeI5;
            root = doc.DocumentElement;
            RespNodeI5 = doc.CreateElement("Indicadores5");

            for (int i = 0; i < res1.Tables[11].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador5");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[11].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI5.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI5);


            //TOTALES de la PAF
            XmlNode RespNodePAF;

            RespNodePAF = doc.CreateElement("TOTALESPAF");
            //operaciones vigentes

            XmlNode nodoPT = doc.CreateElement("totalAprobadoCLP");
            nodoPT.AppendChild(doc.CreateTextNode(totalAprobadoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT);

            XmlNode nodoPT1 = doc.CreateElement("totalVigenteCLP");
            nodoPT1.AppendChild(doc.CreateTextNode(totalVigenteCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT1);

            XmlNode nodoPT2 = doc.CreateElement("totalPropuestoCLP");
            nodoPT2.AppendChild(doc.CreateTextNode(totalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT2);

            XmlNode nodoPT3 = doc.CreateElement("totalAprobadoUF");
            nodoPT3.AppendChild(doc.CreateTextNode(totalAprobadoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT3);

            XmlNode nodoPT4 = doc.CreateElement("totalVigenteUF");
            nodoPT4.AppendChild(doc.CreateTextNode(totalVigenteUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT4);

            if (!string.IsNullOrEmpty(res1.Tables[4].Rows[0]["ValorUF"].ToString()))
            {
                XmlNode nodoPT5 = doc.CreateElement("totalPropuestoUF");
                nodoPT5.AppendChild(doc.CreateTextNode((totalPropuestoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodePAF.AppendChild(nodoPT5);
            }
            else
            {
                XmlNode nodoPT5 = doc.CreateElement("totalPropuestoUF");
                nodoPT5.AppendChild(doc.CreateTextNode("0"));
                RespNodePAF.AppendChild(nodoPT5);
            }

            XmlNode nodoPT6 = doc.CreateElement("totalAprobadoUSD");
            nodoPT6.AppendChild(doc.CreateTextNode(totalAprobadoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT6);

            XmlNode nodoPT7 = doc.CreateElement("totalVigenteUSD");
            nodoPT7.AppendChild(doc.CreateTextNode(totalVigenteUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT7);

            XmlNode nodoPT8 = doc.CreateElement("totalPropuestoUSD");
            nodoPT8.AppendChild(doc.CreateTextNode(totalPropuestoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT8);

            XmlNode nodoPT88 = doc.CreateElement("ValorUF");
            nodoPT88.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT88);

            XmlNode nodoPT89 = doc.CreateElement("ValorDolar");
            nodoPT89.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT89);

            //calculo RE
            double totalAprobadoREUF = totalAprobadoCLP > 0 ? (totalAprobadoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            totalAprobadoREUF = totalAprobadoUSD > 0 ? ((totalAprobadoUSD * (float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString()) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString()))) + totalAprobadoREUF) : totalAprobadoREUF;
            totalAprobadoREUF = totalAprobadoREUF + totalAprobadoUF;

            double totalVigenteREUF = totalVigenteCLP > 0 ? (totalVigenteCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            totalVigenteREUF = totalVigenteUSD > 0 ? ((totalVigenteUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + totalVigenteREUF : totalVigenteREUF;
            totalVigenteREUF = totalVigenteREUF + totalVigenteUF;

            double totalPropuestoREUF = totalPropuestoCLP > 0 ? (totalPropuestoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            totalPropuestoREUF = totalPropuestoUSD > 0 ? ((totalPropuestoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + totalPropuestoREUF : totalPropuestoREUF;
            totalPropuestoREUF = totalPropuestoREUF + totalPropuestoUF;

            XmlNode nodoPT9 = doc.CreateElement("totalAprobadoREUF");
            nodoPT9.AppendChild(doc.CreateTextNode(totalAprobadoREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT9);

            XmlNode nodoPT10 = doc.CreateElement("totalVigenteREUF");
            nodoPT10.AppendChild(doc.CreateTextNode(totalVigenteREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT10);

            XmlNode nodoPT11 = doc.CreateElement("totalPropuestoREUF");
            nodoPT11.AppendChild(doc.CreateTextNode((totalPropuestoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT11);

            //OperacionesNuevas
            XmlNode nodoNT = doc.CreateElement("NtotalAprobadoCLP");
            nodoNT.AppendChild(doc.CreateTextNode(NtotalAprobadoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT);

            XmlNode nodoNT1 = doc.CreateElement("NtotalVigenteCLP");
            nodoNT1.AppendChild(doc.CreateTextNode(NtotalVigenteCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT1);

            XmlNode nodoNT2 = doc.CreateElement("NtotalPropuestoCLP");
            nodoNT2.AppendChild(doc.CreateTextNode(NtotalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT2);

            XmlNode nodoNT3 = doc.CreateElement("NtotalAprobadoUF");
            nodoNT3.AppendChild(doc.CreateTextNode(NtotalAprobadoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT3);

            XmlNode nodoNT4 = doc.CreateElement("NtotalVigenteUF");
            nodoNT4.AppendChild(doc.CreateTextNode(NtotalVigenteUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT4);

            XmlNode nodoNT5 = doc.CreateElement("NtotalPropuestoUF");
            nodoNT5.AppendChild(doc.CreateTextNode(NtotalPropuestoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT5);

            XmlNode nodoNT6 = doc.CreateElement("NtotalAprobadoUSD");
            nodoNT6.AppendChild(doc.CreateTextNode(NtotalAprobadoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT6);

            XmlNode nodoNT7 = doc.CreateElement("NtotalVigenteUSD");
            nodoNT7.AppendChild(doc.CreateTextNode(NtotalVigenteUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT7);

            XmlNode nodoNT8 = doc.CreateElement("NtotalPropuestoUSD");
            nodoNT8.AppendChild(doc.CreateTextNode(NtotalPropuestoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT8);

            //calculos montos totales operaciones vigentes
            double NtotalAprobadoREUF = NtotalAprobadoCLP > 0 ? (NtotalAprobadoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            NtotalAprobadoREUF = NtotalAprobadoUSD > 0 ? ((NtotalAprobadoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalAprobadoREUF : NtotalAprobadoREUF;
            NtotalAprobadoREUF = NtotalAprobadoUF + NtotalAprobadoREUF;

            double NtotalVigenteREUF = NtotalVigenteCLP > 0 ? (NtotalVigenteCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            NtotalVigenteREUF = NtotalVigenteUSD > 0 ? ((NtotalVigenteUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalVigenteREUF : NtotalVigenteREUF;
            NtotalVigenteREUF = NtotalVigenteUF + NtotalVigenteREUF;

            double NtotalPropuestoREUF = NtotalPropuestoCLP > 0 ? (NtotalPropuestoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            NtotalPropuestoREUF = NtotalPropuestoUSD > 0 ? ((NtotalPropuestoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalPropuestoREUF : NtotalPropuestoREUF;
            NtotalPropuestoREUF = NtotalPropuestoREUF + NtotalPropuestoUF;

            XmlNode nodoNT9 = doc.CreateElement("NtotalAprobadoREUF");
            nodoNT9.AppendChild(doc.CreateTextNode((NtotalAprobadoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT9);

            XmlNode nodoNT10 = doc.CreateElement("NtotalVigenteREUF");
            nodoNT10.AppendChild(doc.CreateTextNode(NtotalVigenteREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT10);

            XmlNode nodoNT11 = doc.CreateElement("NtotalPropuestoREUF");
            nodoNT11.AppendChild(doc.CreateTextNode(NtotalPropuestoREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT11);

            //TotalLineaGlobal
            XmlNode nodoTGlobal = doc.CreateElement("GlobalCLPAprobado");
            nodoTGlobal.AppendChild(doc.CreateTextNode((NtotalAprobadoCLP + totalAprobadoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal);

            XmlNode nodoTGlobal1 = doc.CreateElement("GlobalCLPVigente");
            nodoTGlobal1.AppendChild(doc.CreateTextNode((NtotalVigenteCLP + totalVigenteCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal1);

            XmlNode nodoTGlobal2 = doc.CreateElement("GlobalCLPPropuesto");
            nodoTGlobal2.AppendChild(doc.CreateTextNode((NtotalPropuestoCLP + totalPropuestoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal2);

            XmlNode nodoTGlobal3 = doc.CreateElement("GlobalUFAprobado");
            nodoTGlobal3.AppendChild(doc.CreateTextNode((NtotalAprobadoUF + totalAprobadoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal3);

            XmlNode nodoTGlobal4 = doc.CreateElement("GlobalUFVigente");
            nodoTGlobal4.AppendChild(doc.CreateTextNode((NtotalVigenteUF + totalVigenteUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal4);

            XmlNode nodoTGlobal5 = doc.CreateElement("GlobalUFPropuesto");
            nodoTGlobal5.AppendChild(doc.CreateTextNode(((NtotalPropuestoUF + totalPropuestoUF) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal5);

            XmlNode nodoTGlobal6 = doc.CreateElement("GlobalUSDAprobado");
            nodoTGlobal6.AppendChild(doc.CreateTextNode((NtotalAprobadoUSD + totalAprobadoUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal6);

            XmlNode nodoTGlobal7 = doc.CreateElement("GlobalUSDPropuesto");
            nodoTGlobal7.AppendChild(doc.CreateTextNode((NtotalPropuestoUSD + totalPropuestoUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal7);

            XmlNode nodoTGlobal8 = doc.CreateElement("GlobalUSDVigente");
            nodoTGlobal8.AppendChild(doc.CreateTextNode((NtotalVigenteUSD + totalVigenteUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal8);

            XmlNode nodoTGlobal9 = doc.CreateElement("GlobalREUFAprobado");
            nodoTGlobal9.AppendChild(doc.CreateTextNode((NtotalAprobadoREUF + totalAprobadoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal9);

            XmlNode nodoTGlobal10 = doc.CreateElement("GlobalREUFVigente");
            nodoTGlobal10.AppendChild(doc.CreateTextNode((NtotalVigenteREUF + totalVigenteREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal10);

            XmlNode nodoTGlobal11 = doc.CreateElement("GlobalREUFPropuesto");
            nodoTGlobal11.AppendChild(doc.CreateTextNode((NtotalPropuestoREUF + totalPropuestoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal11);

            //garantias
            XmlNode nodoTG = doc.CreateElement("TotalGarantiaComercial");
            nodoTG.AppendChild(doc.CreateTextNode((TotalGarantiaComercial * float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG);

            XmlNode nodoTG1 = doc.CreateElement("TotalGarantiaAjustado");
            nodoTG1.AppendChild(doc.CreateTextNode((TotalGarantiaAjustado * float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG1);

            XmlNode nodoTG22 = doc.CreateElement("TotalGarantiaComercialUF");
            nodoTG22.AppendChild(doc.CreateTextNode((TotalGarantiaComercial).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG22);

            XmlNode nodoTG33 = doc.CreateElement("TotalGarantiaAjustadoUF");
            nodoTG33.AppendChild(doc.CreateTextNode((TotalGarantiaAjustado).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG33);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //totales vigente garantia COBERTURA VIGENTE
            XmlNode nodoTG2 = doc.CreateElement("TotalComercialCV");
            if ((NtotalVigenteREUF + totalVigenteREUF) > 0 && TotalCoberturaComercialVigente > 0)
            {
                var b = totalPropuestoUF_Certificado;
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());

                var a = totalVigenteCLP_Certificado;
                var bb = totalVigenteREUF;

                nodoTG2.AppendChild(doc.CreateTextNode((100.0 * TotalCoberturaComercialVigente / (totalVigenteREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG2.AppendChild(doc.CreateTextNode("0,00"));

            RespNodePAF.AppendChild(nodoTG2);

            XmlNode nodoTG3 = doc.CreateElement("TotalaAjustadoCV");
            if ((NtotalVigenteREUF + totalVigenteREUF) > 0 && TotalCoberturaAjustadoVigente > 0)
            {
                var b = totalPropuestoUF_Certificado;
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                nodoTG3.AppendChild(doc.CreateTextNode((100.0 * TotalCoberturaAjustadoVigente / (totalVigenteREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG3.AppendChild(doc.CreateTextNode("0,00"));
            RespNodePAF.AppendChild(nodoTG3);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //totales Globales garantia COBERTURA GLOBAL
            var A = TotalCoberturaAjustadoNuevo;
            var B = TotalGarantiaNuevaComercial;
            XmlNode nodoTG4 = doc.CreateElement("TotalComercialCG");
            if ((NtotalPropuestoREUF + totalPropuestoREUF) > 0.0 && TotalGarantiaComercial > 0.0)
            {
                double NclpCertificado = NtotalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                nodoTG4.AppendChild(doc.CreateTextNode((100.0 * TotalGarantiaComercial / (NtotalPropuestoREUF + totalPropuestoREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG4.AppendChild(doc.CreateTextNode("0,00"));
            RespNodePAF.AppendChild(nodoTG4);

            XmlNode nodoTG5 = doc.CreateElement("TotalAjustadoCG");
            if ((NtotalPropuestoREUF + totalPropuestoREUF) > 0.0 && TotalGarantiaAjustado > 0.0)
            {
                double NclpCertificado = NtotalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                nodoTG5.AppendChild(doc.CreateTextNode((100.0 * TotalGarantiaAjustado / (NtotalPropuestoREUF + totalPropuestoREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG5.AppendChild(doc.CreateTextNode("0,00"));
            RespNodePAF.AppendChild(nodoTG5);


            ValoresNode.AppendChild(RespNodePAF);
            return doc.OuterXml;
        }

        #endregion

        #region "wpMantenimientoRiesgo"

        public DataSet ListarRiesgo(string razonSocial, string rut, string descEjecutivo, string Perfil, string Area)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@FecInicioActEco", SqlDbType.NVarChar);
                SqlParametros[0].Value = " ";
                SqlParametros[1] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[1].Value = razonSocial;
                SqlParametros[2] = new SqlParameter("@Rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@DescEjecutivo", SqlDbType.NVarChar);
                SqlParametros[3].Value = descEjecutivo;
                SqlParametros[4] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = Perfil;
                SqlParametros[5] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[5].Value = Area;

                dt = AD_Global.ejecutarConsulta("ListarMantenimientoRiesgo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpIvaVenta"

        public DataTable ListarIVAVentas(int idEmpresa, int Anio, int IdDocumentoContable)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Anio", SqlDbType.Int);
                SqlParametros[1].Value = Anio;
                SqlParametros[2] = new SqlParameter("@IdDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = IdDocumentoContable;
                dt = AD_Global.ejecutarConsultas("ConsultarIVA", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpVaciadoEstadoResultado"

        public Boolean finalizarEstadoResultado(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarEstadoResultado(string idEmpresa, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACIONINSERT;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;

                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarFinalizarEstado(string idEmpresa, string xmlData)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACION;

                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarEstadoResultado(string idEmpresa)
        {

            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idDocumentosContables", SqlDbType.Int);
                SqlParametros[0].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GeneracionBalance", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean ModificarEstadoResultado(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public void CargarListaEstadoResultado(ref DropDownList lista)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList listData = app.Lists[Constantes.LISTATIPODOCUMET.TIPODOCUMENT];
                SPView view = listData.Views[Constantes.LISTATIPODOCUMET.VISTA];
                view.Query = "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                //view.Update();
                lista.DataSource = listData.GetItems(view).GetDataTable();
                lista.DataTextField = listData.GetItems(view).Fields["Nombre"].ToString();
                lista.DataValueField = listData.GetItems(view).Fields["ID"].ToString();
                lista.DataBind();
                // lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                //lista.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //log.Error("ConexionDatos" + ex.ToString());
            }
        }

        #endregion

        #region "BitacoraCobranza"

        public DataTable ListarBitacoraCobranza(string mora, string ejecutivo, string acreedor, string empresa)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@categoria", SqlDbType.NVarChar);
                SqlParametros[0].Value = mora;
                SqlParametros[1] = new SqlParameter("@ejecutivo", SqlDbType.NVarChar);
                SqlParametros[1].Value = ejecutivo;
                SqlParametros[2] = new SqlParameter("@acreedor", SqlDbType.NVarChar);
                SqlParametros[2].Value = acreedor;
                SqlParametros[3] = new SqlParameter("@empresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = empresa;

                return AD_Global.ejecutarConsultas("ListarBitacoraCobranza", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
        }


        public DataTable ListarSms(string Ncertificado)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Ncertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = Ncertificado;

                return AD_Global.ejecutarConsultas("ListarSms", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
        }

        #endregion

        #region "DetalleCarteraMorosos"

        public DataSet CargarDetalleCertificado(int idOperacion, int idEmpresa)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;

                dt = AD_Global.ejecutarConsulta("CargarDetalleCertificado", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean GestionBitacoraCobranza(int idEmpresa, int idOperacion, string NroCertificado, string Comentario, int IdAccionCobranza, DateTime FechaGestion, string MoraProyectada, bool Reprogramacion, string Usuario, int opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[2].Value = NroCertificado;
                SqlParametros[3] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Comentario;
                SqlParametros[4] = new SqlParameter("@IdAccionCobranza", SqlDbType.Int);
                SqlParametros[4].Value = IdAccionCobranza;         
                SqlParametros[5] = new SqlParameter("@FechaGestion", SqlDbType.DateTime);
                SqlParametros[5].Value = FechaGestion;
                SqlParametros[6] = new SqlParameter("@MoraProyectada", SqlDbType.NVarChar);
                SqlParametros[6].Value = MoraProyectada;
                SqlParametros[7] = new SqlParameter("@Reprogramacion", SqlDbType.Bit);
                SqlParametros[7].Value = Reprogramacion;
                SqlParametros[8] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[8].Value = Usuario;
                SqlParametros[9] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[9].Value = opcion;
                return AD_Global.ejecutarAccion("GestionBitacoraCobranza", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarGestionBitacoraCobranza(int idEmpresa, int idOperacion, string NroCertificado, string Comentario, DateTime? FechaGestion, string MoraProyectada, bool? Reprogramacion, string Usuario, int opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[9];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[2].Value = NroCertificado;
                SqlParametros[3] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Comentario;
                SqlParametros[4] = new SqlParameter("@FechaGestion", SqlDbType.DateTime);
                SqlParametros[4].Value = FechaGestion;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = Usuario;
                SqlParametros[6] = new SqlParameter("@MoraProyectada", SqlDbType.NVarChar);
                SqlParametros[6].Value = MoraProyectada;
                SqlParametros[7] = new SqlParameter("@Reprogramacion", SqlDbType.Bit);
                SqlParametros[7].Value = Reprogramacion;
                SqlParametros[8] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[8].Value = opcion;
                return AD_Global.ejecutarConsultas("GestionBitacoraCobranza", SqlParametros);
            }
            catch
            {
                return new DataTable();
            }
        }

        #endregion

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

    }
}
