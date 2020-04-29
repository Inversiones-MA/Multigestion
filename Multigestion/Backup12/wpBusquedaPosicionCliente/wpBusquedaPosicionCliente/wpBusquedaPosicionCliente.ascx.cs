using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
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
using System.Web.UI;
using System.Web;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiOperacion.wpBusquedaPosicionCliente.wpBusquedaPosicionCliente
{
    [ToolboxItemAttribute(false)]
    public partial class wpBusquedaPosicionCliente : WebPart
    {
        private static string pagina = "busquedaposicioncliente.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpBusquedaPosicionCliente()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //LogicaNegocio Ln = new LogicaNegocio();
            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    CargarEjecutivo();
                }
                else
                {
                    listar();
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    dvWarning1.Style.Add("display", "block");
                    lbWarning1.Text = "Usuario sin permisos configurados";
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        protected void gridResultado_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        int i;
        protected void gridResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("glyphicon glyphicon-search");
                lb.CommandName = "Consultar";
                lb.CommandArgument = i.ToString();
                i++;
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusqueda_Command;
                e.Row.Cells[4].Text = "";
                e.Row.Cells[4].Controls.Add(lb);
            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Page.Session["IdEmpresa"] = System.Web.HttpUtility.HtmlDecode(gridResultado.Rows[index].Cells[0].Text.ToString());

            Page.Response.Redirect("PosicionCliente.aspx");
        }

        protected void gridResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridResultado.PageIndex = e.NewPageIndex;
            listar();
        }


        #endregion


        #region Metodos

        private void CargarEjecutivo()
        {
            ddlEjecutivo.Items.Clear();
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 1, "");
                util.CargaDDL(ddlEjecutivo, dt, "nombreApellido", "idUsuario");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void listar()
        {
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            res = MTO.LitarDatosEmpresa(txtRUT.Text, txtRazonSocial.Text, ddlEjecutivo.SelectedValue.ToString(), ddlEjecutivo.SelectedItem.ToString());
            if (res != null)
            {
                if (res.Tables[0].Rows.Count > 0)
                {
                    gridResultado.DataSource = res.Tables[0];
                    gridResultado.DataBind();
                }
            }
        }

        #endregion

    }
}
