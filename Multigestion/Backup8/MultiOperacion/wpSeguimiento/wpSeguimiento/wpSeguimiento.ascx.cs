using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.Web;
using System.Web.Services;
using MultigestionUtilidades;
using System.Configuration;
using System.Diagnostics;
using FrameworkIntercapIT.Utilities;

namespace MultiOperacion.wpSeguimiento.wpSeguimiento
{
    [ToolboxItemAttribute(false)]
    public partial class wpSeguimiento : WebPart
    {
        private static string pagina = "Seguimiento.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpSeguimiento()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            LogicaNegocio LN = new LogicaNegocio();
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
                    Page.Session["RESUMEN"] = null;
                    ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                    ocultarDiv();
                    CargarAcreedor();
                    CargarEstadoCertificado();
                    CargarEjecutivo();
                }

                inicializacionGrillas();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
                //Control divFiltros = this.FindControl("filtros");
                //Control divGrilla = this.FindControl("grilla");
                //util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);

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
            inicializacionGrillas();
        }

        protected void lbCalendarioPago_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("CalendarioPago.aspx");
        }

        protected void lbSeguimiento_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Seguimiento.aspx");
        }

        protected void gridSeguimiento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 2).ToString().Replace(".", ","));
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lbk = new LinkButton();
                lbk.CommandName = "Redireccion";
                lbk.ToolTip = "Calendario Pago";
                lbk.CssClass = "glyphicon glyphicon-calendar paddingIconos";
                lbk.CommandArgument = e.Row.Cells[0].Text;
                lbk.OnClientClick = "return Dialogo();";
                lbk.Command += ResultadosBusqueda_Command1;
                e.Row.Cells[10].Controls.Add(lbk);
            }
        }

        protected void ResultadosBusqueda_Command1(object sender, CommandEventArgs e)
        {
            if ("Redireccion" == e.CommandName)
            {
                Page.Response.Redirect("CalendarioPago.aspx?NCertificado=" + e.CommandArgument);
            }
        }

        protected void btnAtras_Click1(object sender, EventArgs e)
        {
            Page.Response.Redirect("ListarOperaciones.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ListarOperaciones.aspx");
        }

        #endregion


        #region Metodos

        protected void inicializacionGrillas()
        {
            LogicaNegocio MTO = new LogicaNegocio();
            DataTable res;
            res = MTO.CP_GestionCalendarioPagp(ddlAcreedor.SelectedValue, ddlEdoCertificado.SelectedValue, ddlEjecutivo.SelectedItem.ToString(), false, false, false, false, false, "k", "k", "01");
            gridSeguimiento.DataSource = res;
            gridSeguimiento.DataBind();
        }

        private void CargarAcreedor()
        {
            SPWeb app = SPContext.Current.Web;
            SPList Lista = app.Lists["Acreedores"];
            app.AllowUnsafeUpdates = true;
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<OrderBy>  <FieldRef Name = 'Nombre' Ascending = 'TRUE'/> </OrderBy>";
            SPListItemCollection items = Lista.GetItems(oQuery);

            util.CargaDDL(ddlAcreedor, items.GetDataTable(), "Nombre", "ID");
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

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
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "cartera", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarEstadoCertificado()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPListItemCollection items = app.Lists["EstadoCertificado"].Items;
            util.CargaDDL(ddlEdoCertificado, items.GetDataTable(), "Nombre", "ID");
        }

        #endregion

    }
}
