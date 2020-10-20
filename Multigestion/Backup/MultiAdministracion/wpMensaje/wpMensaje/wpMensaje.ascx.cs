using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;

namespace MultiAdministracion.wpMensaje.wpMensaje
{
    [ToolboxItemAttribute(false)]
    public partial class wpMensaje : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpMensaje()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    lblTitulo.Text = "Advertencia"; //"oListItem["Titulo"].ToString(); ;
                    lblMensaje.Text = "No se pudo culminar su solicitud, por favor comuniquese con el administrador del sistema.  Detalle: ";
                    if (Page.Session["Error"] != null)
                        lblMensaje.Text = lblMensaje.Text + Page.Session["Error"].ToString();
                }
                catch (Exception ex)
                {
                    lblTitulo.Text = "Error";
                    lblMensaje.Text = ex.Source.ToString() + ex.Message.ToString();
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }
            }
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ListarComercial.aspx");
        }
    }
}
