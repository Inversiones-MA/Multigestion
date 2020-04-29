using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using MultigestionUtilidades;
using System.Web.Services;
using System.Collections.Generic;
using System.Text;
using DevExpress.Web;
using System.Data.SqlClient;
using System.Web.UI;
using System.DirectoryServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Web.Configuration;
using Bd;
using ClasesNegocio;

namespace MultiAdministracion.wpUsuarios.wpUsuarios
{
    [ToolboxItemAttribute(false)]
    public partial class wpUsuarios : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        //Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "Usuarios.aspx";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ocultarDiv();
                LogicaNegocio Ln = new LogicaNegocio();
                Boolean Usuario = true;
                string userName = txt_nombre.Text.Trim();
                SPWeb app2 = SPContext.Current.Web;
                //string pass = "";
                //SPWeb app = SPContext.Current.Web;
                //int groupID = SPContext.Current.Web.Groups["MultiGestion Members"].ID;
                //bool isGroupMember = SPContext.Current.Web.IsCurrentUserMemberOfGroup(groupID);
                //SPGroup grupo = app.Groups.GetByID(groupID);

                //using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "INTERCAP"))
                //{
                //    // validate the credentials
                //    bool isValid = pc.ValidateCredentials("jmtoro@multiaval.cl", "Summer55");
                //}

                //Usuario = IsAuthenticated(userName, pass);

                if (Usuario)
                {
                    int CRUD = (Int32.Parse(hdn_editar.Value) > 0) ? 3 : 1;
                    int ID = Int32.Parse(hdn_editar.Value);
                    string nombreApellido = txt_nombre.Text;
                    string Rut = txt_rut.Text;
                    int idDepartamento = Int32.Parse(ddl_departamento.SelectedValue);
                    string descDepartamento = ddl_departamento.SelectedItem.ToString();
                    string idBanco = ddl_banco.SelectedValue.ToString();
                    string descBanco = ddl_banco.SelectedItem.ToString();
                    string numeroCuenta = txt_numeroCuenta.Text;
                    int idCargo = Int32.Parse(ddl_cargo.SelectedValue);
                    string descCargo = ddl_cargo.SelectedItem.ToString();
                    bool habilitado = chk_Habilitado.Checked;
                    string UsuarioCreacion = util.ObtenerValor(app2.CurrentUser.Name);
                    bool EsResponsable = chkEsResponsable.Checked ? true : false;

                    Ln.GuardarUsuario(CRUD, ID, nombreApellido, Rut, idDepartamento, descDepartamento, idBanco, descBanco, numeroCuenta, idCargo, descCargo, UsuarioCreacion, habilitado, EsResponsable);
                    LimpiarCampos();
                    llenargrid(2);

                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Ingreso exitoso";
                }
                else
                {
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = "el usuario ingresado no existe en el directorio activo";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
            }
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            //LogicaNegocio Ln = new LogicaNegocio();
            //PERMISOS USUARIOS
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
              NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
              Pagina = pagina,
              Etapa = "",
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    CargarDepartamento();
                    CargarCargos();
                    CargarBancos();
                    llenargrid(2);
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            ocultarDiv();
        }

        #endregion


        #region Metodos

        private void CargarDepartamento()
        {
            DataTable dt = new DataTable();
            LogicaNegocio Ln = new LogicaNegocio();
            dt = Ln.ListarDepartamentos();

            util.CargaDDL(ddl_departamento, dt, "descripcion", "idDepartamento");
        }

        private void CargarCargos()
        {
            try
            {
                DataTable dt = new DataTable();
                LogicaNegocio Ln = new LogicaNegocio();
                dt = Ln.ListarCargos(0);

                util.CargaDDL(ddl_cargo, dt, "Descripcion", "idCargo");
            }
            catch (Exception ex)
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = ex.Message;
            }

        }

        private void CargarBancos()
        {
            DataTable dt = new DataTable();
            LogicaNegocio Ln = new LogicaNegocio();
            dt = Ln.ListarBancos("0");
            util.CargaDDL(ddl_banco, dt, "Nombre", "Codigo");
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void llenargrid(int tipoOperacion)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                int idDepartamento = ddl_departamento.SelectedValue == "-1" ? 0 : Convert.ToInt32(ddl_departamento.SelectedValue);
                string descDepartamento = ddl_departamento.SelectedIndex == 0 ? "" : ddl_departamento.SelectedItem.Text.Trim();
                int idBanco = ddl_banco.SelectedValue == "-1" ? 0 : Convert.ToInt32(ddl_banco.SelectedValue);
                string descBanco = ddl_banco.SelectedIndex == 0 ? "" : ddl_banco.SelectedItem.Text.Trim();
                int idCargo = ddl_cargo.SelectedValue == "-1" ? 0 : Convert.ToInt32(ddl_cargo.SelectedValue);
                string descCargo = ddl_cargo.SelectedIndex == 0 ? "" : ddl_cargo.SelectedItem.Text.Trim();
                bool habilitado = chk_Habilitado.Checked;

                DataTable res;
                res = Ln.ListarUsuarios(tipoOperacion, 0, txt_nombre.Text.Trim(), txt_rut.Text.Trim(), idDepartamento, descDepartamento, idBanco, descBanco, txt_numeroCuenta.Text.Trim(), idCargo, descCargo, habilitado, "");
                ResultadosBusqueda.DataSource = res;
                ResultadosBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        private void LimpiarCampos()
        {
            txt_nombre.Text = txt_numeroCuenta.Text = txt_rut.Text = "";
            ddl_banco.SelectedIndex = ddl_cargo.SelectedIndex = ddl_departamento.SelectedIndex = 0;
            hdn_editar.Value = "0";
            chk_Habilitado.Checked = false;
            chkEsResponsable.Checked = false;
        }

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenargrid(2);
        }

        protected void btn_editar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();
            int ID = Int32.Parse(hdn_editar.Value);
            dt = Ln.GetUsuarioPorId(ID);
            foreach (DataRow row in dt.Rows)
            {
                txt_nombre.Text = row["Nombre"].ToString().Trim();
                txt_rut.Text = row["rut"].ToString().Trim();
                txt_numeroCuenta.Text = row["NCuenta"].ToString().Trim();
                ddl_departamento.SelectedIndex = (ddl_departamento.Items.IndexOf(ddl_departamento.Items.FindByValue(row["idDepartamento"].ToString())) > 0) ? ddl_departamento.Items.IndexOf(ddl_departamento.Items.FindByValue(row["idDepartamento"].ToString())) : 0;
                ddl_banco.SelectedIndex = (ddl_banco.Items.IndexOf(ddl_banco.Items.FindByValue(row["idBanco"].ToString())) > 0) ? ddl_banco.Items.IndexOf(ddl_banco.Items.FindByValue(row["idBanco"].ToString())) : 0;
                ddl_cargo.SelectedIndex = (ddl_cargo.Items.IndexOf(ddl_cargo.Items.FindByValue(row["idCargo"].ToString())) > 0) ? ddl_cargo.Items.IndexOf(ddl_cargo.Items.FindByValue(row["idCargo"].ToString())) : 0;
                chk_Habilitado.Checked = Convert.ToBoolean(row["Habilitado"]); //== true ? true : false;
                chkEsResponsable.Checked = Convert.ToBoolean(row["EsResponsable"]);
            }
        }

        protected void btn_eliminar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;
            int CRUD = 4;
            int ID = Int32.Parse(hdn_editar.Value);
            string nombreApellido = "";
            string Rut = "";
            int idDepartamento = 0;
            string descDepartamento = "";
            string idBanco = "";
            string descBanco = "";
            string numeroCuenta = "";
            int idCargo = 0;
            string descCargo = "";
            bool habilitado = false;
            string UsuarioModificacion = util.ObtenerValor(app2.CurrentUser.Name);
            bool esResponsable = false;

            Ln.GuardarUsuario(CRUD, ID, nombreApellido, Rut, idDepartamento, descDepartamento, idBanco, descBanco, numeroCuenta, idCargo, descCargo, UsuarioModificacion, habilitado, esResponsable);
            LimpiarCampos();
            llenargrid(2);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            llenargrid(2);
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            llenargrid(2);
        }

        #endregion


        //validar grupo y usuario en grupo sharepoint
        private Boolean UsuarioActivo(SPGroup oGroupToTestFor, String sUserLoginName)
        {
            Boolean bUserIsInGroup = false;
            sUserLoginName = @"i:0#.w|multigestion\jtoro";

            SPUser oUser = null;
            try
            {
                oUser = SPContext.Current.Web.AllUsers[sUserLoginName];
            }
            catch { }
            if (oUser != null)
            {
                foreach (SPUser item in oGroupToTestFor.Users)
                {
                    if (item.UserToken == oUser.UserToken)
                    {
                        bUserIsInGroup = true;
                        break;
                    }
                }
            }

            return bUserIsInGroup;

        }


        public bool IsAuthenticated(string srvr, string usr, string pwd)
        {
            bool authenticated = false;

            //try
            //{
            //    DirectoryEntry entry = new DirectoryEntry(srvr, usr, pwd);
            //    object nativeObject = entry.NativeObject;
            //    authenticated = true;
            //}
            //catch (DirectoryServicesCOMException cex)
            //{

            //    //not authenticated; reason why is in cex
            //}
            //catch ()
            //{
            //    //not authenticated due to some other exception [this is optional]
            //}
            return authenticated;
        }

        public bool IsAuthenticated(String username, String pwd)
        {
            try
            {
                string _LDAP = WebConfigurationManager.AppSettings["ADConnString"];
                String domainAndUsername = username;
                DirectoryEntry entry = new DirectoryEntry(_LDAP, domainAndUsername, pwd);

                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
