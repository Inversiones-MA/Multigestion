using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using DevExpress.Web;
using System.Data;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
//using Multiaval.wpRendicionAAT.Interfaz;
using System.Diagnostics;
using Bd;

namespace MultiContabilidad.wpRendicionAAT.wpRendicionAAT
{
    [ToolboxItemAttribute(false)]
    public partial class wpRendicionAAT : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpRendicionAAT()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //llenargrid();

            ////Grid.SettingsEditing.BatchEditSettings.EditMode = (GridViewBatchEditMode)Enum.Parse(typeof(GridViewBatchEditMode), EditModeCombo.Text, true);
            ////Grid.SettingsEditing.BatchEditSettings.StartEditAction = (GridViewBatchStartEditAction)Enum.Parse(typeof(GridViewBatchStartEditAction), StartEditActionCombo.Text, true);
        }

        void llenargrid()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable res;
                res = Ln.ListarRendicionCliente("Keyla Sandoval");
                Grid.DataSource = res;
                Grid.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }
    }
}
