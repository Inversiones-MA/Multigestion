using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Reporting.WebForms;

namespace MultiOperacion.wpReporte.wpReporte
{
    [ToolboxItemAttribute(false)]
    public partial class wpReporte : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpReporte()
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
                    // Set the processing mode for the ReportViewer to Remote
                    ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                    ServerReport serverReport = ReportViewer1.ServerReport;

                    // Set the report server URL and report path
                    //serverReport.ReportServerUrl =
                    //    new Uri("http://dw01wsr001/reportserver");
                    //serverReport.ReportPath =
                    //    "/Reportes/SaldoEnMora";

                    // Create the sales order number report parameter
                    //ReportParameter salesOrderNumber = new ReportParameter();
                    //salesOrderNumber.Name = "SalesOrderNumber";
                    //salesOrderNumber.Values.Add("SO43661");
                    //
                    //// Set the report parameters for the report
                    //reportViewer.ServerReport.SetParameters(
                    //    new ReportParameter[] { salesOrderNumber });
                }
                catch (Exception ex)
                {
                    lbWarning1.Text = ex.Message + " 12345 " + ex.InnerException;
                }
            }
        }
    }
}
