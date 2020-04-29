using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace MultiAdministracion.wpPDF.wpPDF
{
    [ToolboxItemAttribute(false)]
    public partial class wpPDF : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPDF()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] archivo = (byte[])Page.Session["binaryData"];
            if (archivo != null)
            {
                Page.Response.Clear();
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.Charset = encoding.EncodingName;
                if (Page.Session["tipoDoc"].ToString().ToLower() == "docx")
                {
                    Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    Page.Response.AddHeader("content-disposition", "attachment;filename=" + Page.Session["Titulo"].ToString() + ".docx");
                    Page.Response.BinaryWrite(archivo);
                    Page.Response.Flush();
                    Page.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "close();", true);
                }
                else if (Page.Session["tipoDoc"].ToString().ToLower() == "excel")
                {
                    Page.Response.Clear();
                    Page.Response.ContentType = "Application/x-msexcel";
                    //Page.Response.AddHeader("content-disposition", "attachment;filename=myfile.xlsx");

                    //Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Page.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", Page.Session["Titulo"].ToString()+".xls"));
                    Page.Response.BinaryWrite(archivo);
                    Page.Response.Flush();
                    Page.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "close();", true);
                }
                else
                {
                    Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                    Page.Response.AddHeader("Content-Disposition", "attachment; filename=" + Page.Session["Titulo"].ToString() + ".pdf; size=" + archivo.Length.ToString());
                    Page.Response.BinaryWrite(archivo);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "close();", true);
                }            
            }
        }
    }
}
