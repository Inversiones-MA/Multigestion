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
                    //SPWeb app = SPContext.Current.Web;
                    //SPList Lista = app.Lists["LMensajes"];
                    //SPQuery oQuery = new SPQuery();
                    //oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + HttpContext.Current.Session["ID"].ToString() + "</Value></Eq></Where>";
                    //SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                    //foreach (SPListItem oListItem in ColecLista)
                    //{
                    lblTitulo.Text = "Advertencia"; //"oListItem["Titulo"].ToString(); ;
                    lblMensaje.Text = "No se pudo culminar su solicitud, por favor comuniquese con el administrador del sistema.  Detalle: ";
                    if (Page.Session["Error"] != null)
                        lblMensaje.Text = lblMensaje.Text + Page.Session["Error"].ToString();//oListItem["Mensaje"].ToString();
                    //}
                    //HttpContext.Current.Session["DIRECCION"] = null;
                    //HttpContext.Current.Session["ID"] = null;
                }
                catch (Exception ex)
                {
                    lblTitulo.Text = "Error";
                    lblMensaje.Text = ex.Source.ToString() + ex.Message.ToString();
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                    //HttpContext.Current.Session["DIRECCION"] = null;
                    //HttpContext.Current.Session["ID"] = null;
                    //Page.Response.Redirect("_HomeCoraLegal.aspx");
                }


            }
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {

            //HttpContext.Current.Session["ID"] = null;
            //Page.Response.Redirect(HttpContext.Current.Session["Link"].ToString());

            Page.Response.Redirect("ListarComercial.aspx");
        }
    }
}
