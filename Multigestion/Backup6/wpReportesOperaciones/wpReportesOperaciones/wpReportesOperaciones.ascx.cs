using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Xsl;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using MultigestionUtilidades;

namespace MultiOperacion.wpReportesOperaciones.wpReportesOperaciones
{
    [ToolboxItemAttribute(false)]
    public partial class wpReportesOperaciones : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpReportesOperaciones()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        string html = string.Empty;
        Utilidades util = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }

        }
        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();

        public static string TransformXMLToHTML(string inputXml, string xsltString)
        {
            XslCompiledTransform transform = GetAndCacheTransform(xsltString);
            System.IO.StringWriter results = new System.IO.StringWriter();
            using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(inputXml)))
            {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }

        private static Dictionary<String, XslCompiledTransform> cachedTransforms = new Dictionary<string, XslCompiledTransform>();
        private static XslCompiledTransform GetAndCacheTransform(String xslt)
        {
            XslCompiledTransform transform;
            if (!cachedTransforms.TryGetValue(xslt, out transform))
            {
                transform = new XslCompiledTransform();
                using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(xslt)))
                {
                    transform.Load(reader);
                }
                cachedTransforms.Add(xslt, transform);
            }
            return transform;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            string xml = string.Empty;

            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CertificadoFianzaComercial.xslt");
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CertificadoFianza.xslt
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CertificadoFianzaComercial.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }
       
        protected void Button2_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCartaGarantias("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }


            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CartaGarantias.xslt
                //xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/_app_bin/prueba.xslt");
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CartaGarantias.xslt");
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CartaGarantia.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();
            DataTable res = new DataTable();
            res = LN.ReporteContratoSubfianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CartaGarantias.xslt
                //xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/_app_bin/prueba.xslt");
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/ContratoSubfianza.xslt");
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=ContratoSubfianza.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }
        
      
        protected void Button5_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            string xml = string.Empty;

            DataTable res = new DataTable();
            res = LN.CertificadoElegibilidad("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CertificadoElegibilidad.xslt");
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=ContratoSubfianza.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);
                Page.Response.End();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCartaGarantias("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }


            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CartaGarantias.xslt
                //xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/_app_bin/prueba.xslt");C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CartaGarantiaITAU.xslt
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CartaGarantiaITAU.xslt");
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CartaGarantiaITAU.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCartaGarantias("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }


            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CartaGarantias.xslt
                //xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/_app_bin/prueba.xslt");
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CartaGarantiaSantander.xslt");
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CartaGarantia.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CertificadoFianzaTecnica.xslt");
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CertificadoFianza.xslt
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CertificadoFianzaTecnica.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/InstruccionCurse.xslt");
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CertificadoFianza.xslt
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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=InstruccionCurse.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CGBancoEstado.xslt");
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CGBancoEstado.xslt
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\

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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CGBancoEstado.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/CFBancoEstado.xslt");
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CFBancoEstado.xslt
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\

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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=CFBancoEstado.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();
            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/pruebaFormatos.xslt");
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CFBancoEstado.xslt
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\

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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=pruebaFormatos.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }

        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();

            DataTable res = new DataTable();
            res = LN.ReporteCertificadoFianza("86", "56", "keyla", "admin");
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + res.Rows[0][0].ToString();
                }
            }

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();
            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/SolicitudPagoGarantia.xslt");
                //C:\Users\keyla.sandoval\Pictures\Multiaval personal\Multiaval\Layouts\Multiaval\xsl\CFBancoEstado.xslt
                //{SharePointRoot}\Template\Layouts\Multiaval\xsl\

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
                Page.Response.Clear();
                byte[] pdfBytes1 = util.ConvertirAPDF_Control(sDocumento);
                Page.Response.ClearContent();
                Page.Response.ClearHeaders();
                Page.Response.AddHeader("Content-Type", "binary/octet-stream");
                Page.Response.AddHeader("Content-Disposition",
                    "attachment; filename=pruebaFormatos.pdf; size=" + pdfBytes1.Length.ToString());
                Page.Response.BinaryWrite(pdfBytes1);

                Page.Response.End();

            }
            catch { }

        }
    }
}
