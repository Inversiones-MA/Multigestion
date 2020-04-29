using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using FrameworkIntercapIT.Utilities;
using ExpertPdf.HtmlToPdf;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using System.Globalization;
using Microsoft.SharePoint.Utilities;
using System.Configuration;
using System.Diagnostics;
using DevExpress.Web;
using DevExpress.Web.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Web.Hosting.Administration;
using NotesFor.HtmlToOpenXml;
using System.Web.UI;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Xml;
using System.Reflection;
using System.ComponentModel;


namespace MultigestionUtilidades
{
    public class Utilidades
    {
        public Utilidades()
        {

        }

        #region "util"

        public string formatearNumero(double? valor, int decimales)
        {
            if (valor == null)
                return "0";

            return valor.Value.ToString(string.Format("{0}{1}", "N", decimales), CultureInfo.CreateSpecificCulture("es-ES"));
        }

        //public string Reemplazar(string monto)
        //{
        //    if (string.IsNullOrEmpty(monto))
        //        return "0";
        //    else
        //        monto = monto.Replace(",", ".");

        //    return monto;
        //}

        public double GetValorDouble(string valor)
        {
            double retorno = 0;
            if (string.IsNullOrEmpty(valor))
                return retorno;
            else
            {
                var str = valor.Replace(".", "").Replace(",", ".");
                retorno = Convert.ToDouble(str, CultureInfo.InvariantCulture);
            }
            return retorno;
        }

        //public decimal GetValorDecimal(string valor)
        //{
        //    decimal retorno = 0;
        //    if (string.IsNullOrEmpty(valor))
        //        return retorno;
        //    else
        //    {
        //        var str = valor.Replace(".", "").Replace(",", ".");
        //        retorno = Convert.ToDecimal(str, CultureInfo.InvariantCulture);
        //    }
        //    return retorno;
        //}

        public string Mensaje(string mensaje)
        {
            mensaje = mensaje.Replace("\n", "\n" + System.Environment.NewLine);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(mensaje);

            mensaje = sb.ToString().Replace(Environment.NewLine, "<br />");
            return mensaje;
        }

        public string ObtenerValor(String cadena)
        {
            return cadena.Substring(cadena.IndexOf("#") + 1);
        }

        public string actualizaMiles(object num)
        {
            string numdecimal = "";
            string str = (string)num;
            str = str.Replace(".", "");
            if (str.Contains(","))
            {
                numdecimal = str.Split(',')[1];
                str = str.Split(',')[0];
            }

            int index = str.IndexOf('-');
            str = str.Replace("-", "");

            int length = str.Length;
            int num4 = 0;
            string str2 = "";
            string str3 = "";
            if (length > 3)
            {
                for (int i = length - 1; i >= 0; i--)
                {
                    str3 = str.Substring(i, 1);
                    if (num4 == 3)
                    {
                        num4 = 0;
                        str2 = "." + str2;
                    }
                    str2 = str3 + str2;
                    num4++;
                }
            }
            else
                str2 = str;

            if (index > -1)
                return ("-" + str2);

            return str2 + "," + numdecimal;
        }

        //public string actualizaMilesSinDecimal(object num)
        //{
        //    string numdecimal = "";
        //    string str = (string)num;
        //    str = str.Replace(".", "");
        //    if (str.Contains(","))
        //    {
        //        numdecimal = str.Split(',')[1];
        //        str = str.Split(',')[0];
        //    }

        //    int index = str.IndexOf('-');
        //    str = str.Replace("-", "");

        //    int length = str.Length;
        //    int num4 = 0;
        //    string str2 = "";
        //    string str3 = "";
        //    if (length > 3)
        //    {
        //        for (int i = length - 1; i >= 0; i--)
        //        {
        //            str3 = str.Substring(i, 1);
        //            if (num4 == 3)
        //            {
        //                num4 = 0;
        //                str2 = "." + str2;
        //            }
        //            str2 = str3 + str2;
        //            num4++;
        //        }
        //    }
        //    else
        //        str2 = str;

        //    if (index > -1)
        //        return ("-" + str2);

        //    return str2;
        //}

        //public string FormateaMiles(double num)
        //{
        //    string numdecimal = "";
        //    string str = num.ToString();
        //    str = str.Replace(".", "");
        //    if (str.Contains(","))
        //    {
        //        numdecimal = str.Split(',')[1];
        //        str = str.Split(',')[0];
        //    }

        //    int index = str.IndexOf('-');
        //    str = str.Replace("-", "");

        //    int length = str.Length;
        //    int num4 = 0;
        //    string str2 = "";
        //    string str3 = "";
        //    if (length > 3)
        //    {
        //        for (int i = length - 1; i >= 0; i--)
        //        {
        //            str3 = str.Substring(i, 1);
        //            if (num4 == 3)
        //            {
        //                num4 = 0;
        //                str2 = "." + str2;
        //            }
        //            str2 = str3 + str2;
        //            num4++;
        //        }
        //    }
        //    else
        //        str2 = str;

        //    if (index > -1)
        //        return ("-" + str2);

        //    return str2; //+ "," + numdecimal;
        //}

        public string RemoverSignosAcentos(string texto)
        {
            string consignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇ.,<>()";
            string sinsignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcC      ";

            var textoSinAcentos = string.Empty;

            foreach (var caracter in texto)
            {
                var indexConAcento = consignos.IndexOf(caracter);
                if (indexConAcento > -1)
                    textoSinAcentos = textoSinAcentos + (sinsignos.Substring(indexConAcento, 1));
                else
                    textoSinAcentos = textoSinAcentos + (caracter);
            }
            return textoSinAcentos.Replace(" ", "_");
        }

        public void bloquear(System.Web.UI.Control ctr, string permiso, bool TieneFiltro)
        {
            bool status = false;
            if (permiso == "Lectura")
            {
                status = false;
            }
            else
                status = true;

            foreach (System.Web.UI.Control ctrl in ctr.Controls)
            {
                if (ctrl is TextBox && !TieneFiltro)
                    ((TextBox)ctrl).Enabled = status;
                if (ctrl is Button && !TieneFiltro)
                    OcultarBotones(permiso, ((Button)ctrl));
                if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Enabled = status;
                if (ctrl is ImageButton)
                    ((ImageButton)ctrl).Enabled = status;
                if (ctrl is System.Web.UI.WebControls.CheckBox)
                    ((System.Web.UI.WebControls.CheckBox)ctrl).Enabled = status;
                if (ctrl is DropDownList && !TieneFiltro)
                    ((DropDownList)ctrl).Enabled = status;
                if (ctrl is HyperLink)
                    ((HyperLink)ctrl).Enabled = status;
                if (ctrl is LinkButton)
                {
                    if (((LinkButton)ctrl).Text == "Editar" || ((LinkButton)ctrl).Text == "Buscar")
                        ((LinkButton)ctrl).Visible = status;
                    else
                        ((LinkButton)ctrl).Visible = status;
                }
                if (ctrl is GridView)
                {
                    BloquearGrilla(permiso, ((GridView)ctrl));
                    ((GridView)ctrl).CssClass = "table table-responsive table-hover table-bordered";
                }
                if (ctrl is Panel)
                    BloquearPanel(status, permiso, ((Panel)ctrl));
                if (ctrl is Microsoft.SharePoint.WebControls.DateTimeControl)
                    ((Microsoft.SharePoint.WebControls.DateTimeControl)ctrl).Enabled = status;
                if (ctrl is RadioButtonList)
                    ((RadioButtonList)ctrl).Enabled = false;
            }
        }

        private void BloquearPanel(bool status, string permiso, Panel panel)
        {
            foreach (System.Web.UI.Control ctrl in panel.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Enabled = status;
                if (ctrl is Button)
                {
                    OcultarBotones(permiso, ((Button)ctrl));
                }
                if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Enabled = status;
                if (ctrl is ImageButton)
                    ((ImageButton)ctrl).Enabled = status;
                if (ctrl is System.Web.UI.WebControls.CheckBox)
                    ((System.Web.UI.WebControls.CheckBox)ctrl).Enabled = status;
                if (ctrl is DropDownList)
                    ((DropDownList)ctrl).Enabled = status;
                if (ctrl is HyperLink)
                    ((HyperLink)ctrl).Enabled = status;
                if (ctrl is GridView)
                {
                    BloquearGrilla(permiso, ((GridView)ctrl));
                    ((GridView)ctrl).CssClass = "table table-responsive table-hover table-bordered";
                }
                if (ctrl is Panel)
                    BloquearPanel(status, permiso, ((Panel)ctrl));
                if (ctrl is LinkButton)
                    ((LinkButton)ctrl).Visible = status;
                if (ctrl is Microsoft.SharePoint.WebControls.DateTimeControl)
                    ((Microsoft.SharePoint.WebControls.DateTimeControl)ctrl).Enabled = status;
                if (ctrl is RadioButtonList)
                    ((RadioButtonList)ctrl).Enabled = false;
            }
        }

        private void BloquearGrilla(string permiso, GridView grid)
        {
            foreach (GridViewRow row in grid.Rows)
            {
                for (int j = 1; j < row.Cells.Count; j++)
                {
                    foreach (System.Web.UI.Control ctrl in row.Cells[j].Controls)
                    {
                        string aa = row.Cells[j].Text;

                        if (row.Cells[j].Text == "Editar" || row.Cells[j].Text == "&nbsp;" || row.Cells[j].Text == "" || row.Cells[j].Text == "Eliminar")
                        {
                            if (ctrl is LinkButton)
                            {
                                if (permiso == "Editar")
                                {
                                    if (((LinkButton)ctrl).CommandName == "Eliminar")
                                        ((LinkButton)ctrl).Visible = false;
                                }
                                if (permiso == "Eliminar")
                                {
                                    if (((LinkButton)ctrl).CommandName == "Editar")
                                        ((LinkButton)ctrl).Visible = false;
                                }
                                if (permiso == "Lectura")
                                {
                                    if (((LinkButton)ctrl).CommandName == "Editar")
                                        ((LinkButton)ctrl).Visible = true;
                                    if (((LinkButton)ctrl).CommandName == "Eliminar")
                                        ((LinkButton)ctrl).Visible = false;
                                }
                            }

                            if (permiso == "Lectura")
                            {
                                if (ctrl is Button)
                                    OcultarBotones(permiso, ((Button)ctrl));
                                if (ctrl is TextBox)
                                    ((TextBox)ctrl).ReadOnly = true;
                                if (ctrl is RadioButton)
                                    ((RadioButton)ctrl).Enabled = false;
                                if (ctrl is RadioButtonList)
                                    ((RadioButtonList)ctrl).Enabled = false;
                            }

                        }
                        //if (permiso == "Lectura")
                        //{
                        //    ((TextBox)ctrl).ReadOnly = true;
                        //    ((RadioButton)ctrl).Enabled = false;
                        //    ((DropDownList)ctrl).Enabled = false;
                        //    ((Button)ctrl).Enabled = false;
                        //    ((System.Web.UI.WebControls.CheckBox)ctrl).Enabled = false;
                        //}
                    }
                }
            }
        }

        private void OcultarBotones(string permiso, Button btn)
        {
            if (permiso == "Editar")
            {
                if (btn.Text == "Guardar")
                    btn.Visible = true;
                if (btn.Text == "Finalizar")
                    btn.Visible = false;
                if (btn.Text == " < Volver Atrás")
                    btn.Visible = true;
            }
            if (permiso == "Eliminar")
            {
                if (btn.Text == "Cancelar")
                    btn.Visible = false;
                if (btn.Text == "Guardar")
                    btn.Visible = false;
                if (btn.Text == "Limpiar")
                    btn.Visible = false;
                if (btn.Text == "Finalizar")
                    btn.Visible = false;
                if (btn.Text == " < Volver Atrás")
                    btn.Visible = true;
            }
            if (permiso == "Lectura")
            {
                if (btn.Text == "Cancelar")
                    btn.Visible = false;
                if (btn.Text == "Guardar")
                    btn.Visible = false;
                if (btn.Text == "Limpiar")
                    btn.Visible = false;
                if (btn.Text == "Finalizar")
                    btn.Visible = false;
                if (btn.Text == "Calcular")
                    btn.Visible = false;
                if (btn.Text == "Adjuntar")
                    btn.Visible = false;
                if (btn.Text == " < Volver Atrás")
                    btn.Visible = true;
            }
        }


        public void Limpiar(System.Web.UI.Control ctr)
        {
            foreach (System.Web.UI.Control ctrl in ctr.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text= string.Empty;
                //if (ctrl is Button)
                //    OcultarBotones(permiso, ((Button)ctrl));
                if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Checked = false;
                if (ctrl is System.Web.UI.WebControls.CheckBox)
                    ((System.Web.UI.WebControls.CheckBox)ctrl).Checked = false;
                if (ctrl is DropDownList)
                    ((DropDownList)ctrl).SelectedIndex = -1;
                if (ctrl is Panel)
                    LimpiarPanel(((Panel)ctrl));
                //if (ctrl is Microsoft.SharePoint.WebControls.DateTimeControl)
                //    ((Microsoft.SharePoint.WebControls.DateTimeControl)ctrl);
                if (ctrl is RadioButtonList)
                    ((RadioButtonList)ctrl).Items.Clear();
            }
        }

        private void LimpiarPanel(Panel panel)
        {
            foreach (System.Web.UI.Control ctrl in panel.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Checked = false;
                if (ctrl is System.Web.UI.WebControls.CheckBox)
                    ((System.Web.UI.WebControls.CheckBox)ctrl).Checked = false;
                if (ctrl is DropDownList)
                    ((DropDownList)ctrl).SelectedIndex = -1;
               //if (ctrl is Microsoft.SharePoint.WebControls.DateTimeControl)
               //     ((Microsoft.SharePoint.WebControls.DateTimeControl)ctrl).Enabled = status;
                if (ctrl is RadioButtonList)
                    ((RadioButtonList)ctrl).Items.Clear();
            }
        }


        private const int MAXFOLDERLENGTH = 128, MAXFILELENGTH = 123;
        //private int MAXURLLENGTH = 259;

        private Regex invalidCharsRegex = new Regex(@"[\*\?\|\\\t/:""'<>#{}%~&]", RegexOptions.Compiled);

        private Regex invalidRulesRegex = new Regex(@"\.{2,}", RegexOptions.Compiled);

        private Regex startEndRegex = new Regex(@"^[\. ]|[\. ]$", RegexOptions.Compiled);

        private Regex extraSpacesRegex = new Regex(" {2,}", RegexOptions.Compiled);

        public string GetSharePointFriendlyName(string original)
        {
            //, int currentPathLength, int maxItemLength
            // remove invalid characters and some initial replacements
            string friendlyName = extraSpacesRegex.Replace(
            invalidRulesRegex.Replace(
            invalidCharsRegex.Replace(
            original, String.Empty).Trim()
            , ".")
            , " ");

            return friendlyName;
        }

        public byte[] ConvertirAPDF_Control(System.Text.StringBuilder sDocumento)
        {
            byte[] bArchivoPDF = null;

            // Create the PDF converter. Optionally you can specify the virtual browser 
            // width as parameter. 1024 pixels is default, 0 means autodetect
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfDocumentOptions.CustomPdfPageSize = new SizeF(50, 50);
            // set the license key
            pdfConverter.LicenseKey = "wunw4vri8/b24vrs8uLx8+zz8Oz7+/v7";
            // set the converter options
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Best;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
            pdfConverter.OptimizePdfPageBreaks = true;
            // set if header and footer are shown in the PDF - optional - default is false 
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;
            // set to generate a pdf with selectable text or a pdf with embedded image - optional - default is true
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
            // set if the HTML content is resized if necessary to fit the PDF page width - optional - default is true
            pdfConverter.PdfDocumentOptions.FitWidth = true;

            // set the embedded fonts option - optional - default is false
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            pdfConverter.ScriptsEnabledInImage = false;
            pdfConverter.ActiveXEnabledInImage = false;
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
            //pdfConverter
            bArchivoPDF = pdfConverter.GetPdfBytesFromHtmlString(sDocumento.ToString());

            return bArchivoPDF;
        }

        public byte[] PdfFiscalia(System.Text.StringBuilder sDocumento)
        {
            byte[] bArchivoPDF = null;
            PdfConverter pdfConverter = new PdfConverter();

            pdfConverter.PdfDocumentOptions.CustomPdfPageSize = new SizeF(50, 50);
            // set the license key
            pdfConverter.LicenseKey = "wunw4vri8/b24vrs8uLx8+zz8Oz7+/v7";
            // set the converter options
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Best;
            pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
            pdfConverter.OptimizePdfPageBreaks = true;
            // set if header and footer are shown in the PDF - optional - default is false 
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = false;
            // set to generate a pdf with selectable text or a pdf with embedded image - optional - default is true
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
            // set if the HTML content is resized if necessary to fit the PDF page width - optional - default is true
            pdfConverter.PdfDocumentOptions.FitWidth = true;
            // set the embedded fonts option - optional - default is false
            pdfConverter.PdfDocumentOptions.EmbedFonts = false;
            pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
            pdfConverter.ScriptsEnabledInImage = false;
            pdfConverter.ActiveXEnabledInImage = false;
            pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;

            //pdfConverter.PdfFooterOptions.FooterText = "Footer text";
            pdfConverter.PdfFooterOptions.FooterBackColor = System.Drawing.Color.White;
            pdfConverter.PdfFooterOptions.FooterHeight = 5;
            pdfConverter.PdfFooterOptions.FooterTextColor = System.Drawing.Color.Black;
            pdfConverter.PdfFooterOptions.FooterTextFontType = PdfFontType.HelveticaOblique;
            pdfConverter.PdfFooterOptions.FooterTextFontSize = 8;
            pdfConverter.PdfFooterOptions.DrawFooterLine = false;

            //numero de pagina
            // pdfConverter.PdfDocumentOptions.ShowFooter = true;
            // // set the footer height in points
            // pdfConverter.PdfFooterOptions.FooterHeight = 60;
            // //write the page number
            //pdfConverter.PdfFooterOptions.TextArea = new TextArea(0, 30, "Pagina &p; de &P; ",
            //     new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));
            //pdfConverter.PdfFooterOptions.TextArea.EmbedTextFont = true;
            //pdfConverter.PdfFooterOptions.TextArea.TextAlign = HorizontalTextAlign.Center;

            bArchivoPDF = pdfConverter.GetPdfBytesFromHtmlString(sDocumento.ToString());

            return bArchivoPDF;
        }

        private void AddFooter(PdfConverter pdfConverter)
        {
            //string thisPageURL = HttpContext.Current.Request.Url.AbsoluteUri;
            //string headerAndFooterHtmlUrl = thisPageURL.Substring(0, thisPageURL.LastIndexOf('/')) + "/HeaderAndFooterHtml.htm";

            //enable footer
            pdfConverter.PdfDocumentOptions.ShowFooter = true;
            // set the footer height in points
            pdfConverter.PdfFooterOptions.FooterHeight = 60;
            //write the page number
            pdfConverter.PdfFooterOptions.TextArea = new TextArea(0, 30, "This is page &p; of &P; ",
                new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));
            pdfConverter.PdfFooterOptions.TextArea.EmbedTextFont = true;
            pdfConverter.PdfFooterOptions.TextArea.TextAlign = HorizontalTextAlign.Right;
            // set the footer HTML area
            //pdfConverter.PdfFooterOptions.HtmlToPdfArea = new HtmlToPdfArea(headerAndFooterHtmlUrl);
            //pdfConverter.PdfFooterOptions.HtmlToPdfArea.EmbedFonts = cbEmbedFonts.Checked;
        }

        //public byte[] bajarWord(string codigoHtml, string NombrePlantilla)
        //{
        //    byte[] bArchivoDOC = null;
        //    //string contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //    string html = codigoHtml; //File.ReadAllText(Server.MapPath("~/Template.html"));

        //    using (MemoryStream generatedDocument = new MemoryStream())
        //    {
        //        using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
        //        {
        //            MainDocumentPart mainPart = package.MainDocumentPart;
        //            if (mainPart == null)
        //            {
        //                mainPart = package.AddMainDocumentPart();
        //                new Document(new Body()).Save(mainPart);
        //            }

        //            HtmlConverter converter = new HtmlConverter(mainPart);

        //            //converter.BaseImageUrl = new Uri(System.Web.UI.Page.Request.Url.Scheme + "://" + Page.Request.Url.Authority);

        //            Body body = mainPart.Document.Body;

        //            var paragraphs = converter.Parse(html);
        //            for (int i = 0; i < paragraphs.Count; i++)
        //            {
        //                body.Append(paragraphs[i]);
        //            }

        //            mainPart.Document.Save();
        //        }

        //        bArchivoDOC = generatedDocument.ToArray(); // simpler way of converting to array
        //        generatedDocument.Close();

        //        return bArchivoDOC;
        //    }
        //}

        public string QuitarDiv(string cadena)
        {
            int limInf = cadena.IndexOf(">") + 1;
            return cadena.Substring(limInf, cadena.IndexOf("<", limInf + 1) - limInf);
        }

        public string CreateCAMLQuery(List<string> parameters, string orAndCondition, bool isIncludeWhereClause)
        {
            StringBuilder sb = new StringBuilder();

            if (parameters.Count == 0)
            {
                AppendEQ(sb, "all");
            }

            int j = 0;
            for (int i = 0; i < parameters.Count; i++)
            {
                if (!string.IsNullOrEmpty(parameters[i].Split(';')[3]))
                {
                    AppendEQ(sb, parameters[i]);

                    if (i > 0 && j > 0)
                    {
                        sb.Insert(0, "<" + orAndCondition + ">");
                        sb.Append("</" + orAndCondition + ">");
                    }
                    j++;
                }
            }
            if (isIncludeWhereClause)
            {
                sb.Insert(0, "<Where>");
                sb.Append("</Where>");
            }
            return @sb.ToString();
        }

        public void AppendEQ(StringBuilder sb, string value)
        {
            string[] field = value.Split(';');

            sb.AppendFormat("<{0}>", field[2].ToString());
            sb.AppendFormat("<FieldRef Name='{0}'></FieldRef>", field[0].ToString());
            sb.AppendFormat("<Value Type='{0}'>{1}</Value>", field[1].ToString(), field[3].ToString());
            sb.AppendFormat("</{0}>", field[2].ToString());
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

        public DateTime? ValidarFecha(string FechaIngreso)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            string sDateTime = FechaIngreso.ToString();
            string[] limpiar = sDateTime.Split(' ');

            var format = "d";
            var provider = CultureInfo.InvariantCulture;
            DateTime fecha;
            DateTime fechaSalida;
            sDateTime = FechaIngreso.ToString();

            if (DateTime.TryParseExact(limpiar[0].Trim(), format, new CultureInfo("es-CL"), DateTimeStyles.AssumeLocal, out fecha))
                fechaSalida = DateTime.Parse(limpiar[0].ToString(), new CultureInfo("es-CL"));
            else
                fechaSalida = Convert.ToDateTime(limpiar[0]);

            if (fechaSalida.ToString() != "01-01-0001 0:00:00" || string.IsNullOrEmpty(fechaSalida.ToString()))
            {
                int dia = fechaSalida.Date.Day;
                int mes = fechaSalida.Date.Month;
                int anio = fechaSalida.Date.Year;
                DateTime fechaInicioSet = new DateTime(anio, mes, dia);
                fechaSalida = Convert.ToDateTime(fechaInicioSet, new CultureInfo("es-CL"));

                return fechaSalida;
            }
            else
                return null;
        }

        public bool EliminarDoc(string Empresa, string Rut, string Area, string IdOperacion, string NombreArchivo)
        {
            SPWeb app = SPContext.Current.Web;
            string RutaEmpresa = carpetaEmpresa(Empresa, Rut);
            string path = RutaEmpresa + "/" + Area;
            //path = string.IsNullOrWhiteSpace(IdOperacion) ? path : path + "/" + IdOperacion;
            String url = app.Url + "/Documentos/" + path;
            SPFolder carpeta = app.GetFolder(url);

            try
            {
                SPFileCollection collFiles = app.GetFolder(carpeta + "/").Files;
                foreach (SPFile archivoTemp in collFiles)
                {
                    if (archivoTemp.Name == NombreArchivo)
                        collFiles.Delete(archivoTemp.Url);
                    break;
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool EliminarDocFiscalia(string Empresa, string Rut, string Area, string IdOperacion, string NombreArchivo)
        {
            SPWeb app = SPContext.Current.Web;
            string RutaEmpresa = carpetaEmpresa(Empresa, Rut);
            string path = RutaEmpresa + "/" + Area;
            path = string.IsNullOrWhiteSpace(IdOperacion) ? path : path + "/" + IdOperacion;
            String url = app.Url + "/Documentos/" + path;
            SPFolder carpeta = app.GetFolder(url);

            try
            {
                SPFileCollection collFiles = app.GetFolder(carpeta + "/").Files;
                foreach (SPFile archivoTemp in collFiles)
                {
                    if (archivoTemp.Name == NombreArchivo)
                    {
                        collFiles.Delete(archivoTemp.Url);
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string SoloExtension(string NombreArchivo)
        {
            string sSoloExtension = NombreArchivo;
            if ((string.IsNullOrEmpty(sSoloExtension) == false))
            {
                int iPunto = NombreArchivo.LastIndexOf(".");
                if ((iPunto > -1))
                {
                    sSoloExtension = NombreArchivo.Substring((iPunto + 1));
                }
            }

            return sSoloExtension;
        }

        public string NombreArchivo(string TipoDocumento, string ddlOperacion)
        {
            String nombreArchivoNuevo = TipoDocumento.Trim();
            nombreArchivoNuevo = ddlOperacion == "Seleccione" ? nombreArchivoNuevo : nombreArchivoNuevo + "_" + ddlOperacion.Trim();
            nombreArchivoNuevo = RemoverSignosAcentos(nombreArchivoNuevo) + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm");

            return nombreArchivoNuevo;
        }

        public string DescargarArchivo(string nombreArchivo, string area, string idOperacion, string empresa, string rut)
        {
            String carpetaEmpresa = RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(empresa));
            carpetaEmpresa = carpetaEmpresa.Length > 100 ? carpetaEmpresa.Substring(0, 100) : carpetaEmpresa;
            carpetaEmpresa = rut.Split('-')[0] + "_" + carpetaEmpresa;
            String pathSinOperacion;
            String path = pathSinOperacion = carpetaEmpresa + "/" + area;
            path = string.IsNullOrWhiteSpace(idOperacion) ? path : path + "/" + idOperacion;

            SPWeb app = SPContext.Current.Web;
            String url = app.Url + "/Documentos/" + path;
            String urlSinOperacion = app.Url + "/Documentos/" + pathSinOperacion;

            SPFolder carpeta = app.GetFolder(url);
            SPFolder carpetaSinOperacion = app.GetFolder(urlSinOperacion);

            string pathArchivo = app.ServerRelativeUrl + carpeta.ToString() + "/" + nombreArchivo;
            string pathArchivoSinOperacion = app.ServerRelativeUrl + carpetaSinOperacion.ToString() + "/" + nombreArchivo;

            SPFolder sp1 = app.GetFolder(app.ServerRelativeUrl + carpeta.ToString());
            SPFolder sp2 = app.GetFolder(app.ServerRelativeUrl + carpetaSinOperacion.ToString());
            string pathFinal = String.Empty;

            foreach (SPFile fileTemp in sp1.Files)
            {
                if (fileTemp.Name == nombreArchivo)
                {
                    pathFinal = pathArchivo;
                    break;
                }
            }
            foreach (SPFile fileTemp in sp2.Files)
            {
                if (fileTemp.Name == nombreArchivo)
                {
                    pathFinal = pathArchivoSinOperacion;
                    break;
                }
            }

            return pathFinal;
        }

        public string GetText(string text, string stopAt)
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);
            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
            else
                return "";
        }

        public string carpetaEmpresa(string NombreEmpresa, string rutEmpresa)
        {
            string carpetaEmpresa = RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(NombreEmpresa));
            carpetaEmpresa = carpetaEmpresa.Length > 100 ? carpetaEmpresa.Substring(0, 100) : carpetaEmpresa;
            carpetaEmpresa = rutEmpresa.Split('-')[0].Trim() + "_" + carpetaEmpresa;

            return carpetaEmpresa;
        }

        
        public string ReemplazarValores(string texto, DataSet ds, string ListarGarantiasAnexo1, string TotalRepresentantes, string TotalAvales)
        {
            string textoSinAcentos = string.Empty;
            DataTable dt = new DataTable("dt1");
            DataTable dt2 = new DataTable("dt2");
            DataTable dt3 = new DataTable("dt3");

            dt = ds.Tables[0];
            if (ds.Tables[1].Rows.Count > 0)
                dt2 = ds.Tables[1];

            if (ds.Tables[2].Rows.Count > 0)
                dt3 = ds.Tables[2];

            try
            {
                if (dt.Rows.Count > 0)
                {
                    #region datos empresa

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FecIniContrato"].ToString()))
                        texto = texto.Replace("{Fecha_Contrato}", dt.Rows[0]["FecIniContrato"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Contrato}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RazonSocial"].ToString()))
                        texto = texto.Replace("{Razon_social}", dt.Rows[0]["RazonSocial"].ToString().Trim());
                    else
                        texto = texto.Replace("{Razon_social}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Rut"].ToString()))
                    {
                        int rut = Convert.ToInt32(dt.Rows[0]["Rut"]);
                        texto = texto.Replace("{Rut}", rut.ToString("N0", new CultureInfo("es-CL")) + "-" + dt.Rows[0]["DivRut"]);
                    }
                    else
                        texto = texto.Replace("{Rut}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Direccion"].ToString()))
                        texto = texto.Replace("{Direccion}", dt.Rows[0]["Direccion"].ToString().Trim());
                    else
                        texto = texto.Replace("{Direccion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescTipoEmpresa"].ToString()))
                        texto = texto.Replace("{Tipo_Empresa}", dt.Rows[0]["DescTipoEmpresa"].ToString().Trim());
                    else
                        texto = texto.Replace("{Tipo_Empresa}", "[S/D]");

                    #endregion

                    #region datos operacion

                    if (!string.IsNullOrEmpty(dt.Rows[0]["fogapeEstado"].ToString()))
                    {
                        texto = texto.Replace("{Fogape_Estado}", dt.Rows[0]["fogapeEstado"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Fogape_Estado}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["NroPagare"].ToString()))
                    {
                        texto = texto.Replace("{Numero_Pagare}", dt.Rows[0]["NroPagare"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Numero_Pagare}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["numCredito"].ToString()))
                    {
                        texto = texto.Replace("{Numero_Operacion}", dt.Rows[0]["numCredito"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Numero_Operacion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Banco/Factoring"].ToString()))
                        texto = texto.Replace("{Banco_Factoring}", dt.Rows[0]["Banco/Factoring"].ToString().Trim());
                    else
                        texto = texto.Replace("{Banco_Factoring}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["MonedaC"].ToString()))
                        texto = texto.Replace("{Moneda_Descripcion}", dt.Rows[0]["MonedaC"].ToString().Trim());
                    else
                        texto = texto.Replace("{Moneda_Descripcion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Signo"].ToString()))
                        texto = texto.Replace("{Moneda_Signo}", dt.Rows[0]["Signo"].ToString().Trim());
                    else
                        texto = texto.Replace("{Moneda_Signo}", "[S/D]");


                    if (!string.IsNullOrEmpty(dt.Rows[0]["descPorcCobCF"].ToString()))
                        texto = texto.Replace("{Cobertura_Certificado}", dt.Rows[0]["descPorcCobCF"].ToString().Trim());
                    else
                        texto = texto.Replace("{Cobertura_Certificado}", "[S/D]");


                    if (!string.IsNullOrEmpty(dt.Rows[0]["FechaEmisionDia"].ToString()))
                        texto = texto.Replace("{Fecha_Emision_Dia}", dt.Rows[0]["FechaEmisionDia"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Emision_Dia}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FechaEmisionMes"].ToString()))
                        texto = texto.Replace("{Fecha_Emision_Mes}", dt.Rows[0]["FechaEmisionMes"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Emision_Mes}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FechaEmisionAnio"].ToString()))
                        texto = texto.Replace("{Fecha_Emision_Año}", dt.Rows[0]["FechaEmisionAnio"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Emision_Año", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["descFondo"].ToString()))
                        texto = texto.Replace("{Fondo_Garantia}", dt.Rows[0]["descFondo"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fondo_Garantia}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["descPorcCobCF"].ToString()))
                        texto = texto.Replace("{Cobertura_Afianzamiento}", dt.Rows[0]["descPorcCobCF"].ToString().Trim());
                    else
                        texto = texto.Replace("{Cobertura_Afianzamiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["descCobFOGAPE"].ToString()))
                        texto = texto.Replace("{Cobertura_Fogape}", dt.Rows[0]["descCobFOGAPE"].ToString().Trim());
                    else
                        texto = texto.Replace("{Cobertura_Fogape}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["TasaIntBco"].ToString()))
                        texto = texto.Replace("{Tasa_Interes}", dt.Rows[0]["TasaIntBco"].ToString().Trim());
                    else
                        texto = texto.Replace("{Tasa_Interes}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["descTipoCta"].ToString()))
                        texto = texto.Replace("{Tipo_Cuota}", dt.Rows[0]["descTipoCta"].ToString().Trim());
                    else
                        texto = texto.Replace("{Tipo_Cuota}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["desctipoM"].ToString()))
                        texto = texto.Replace("{Tipo_Moneda}", dt.Rows[0]["desctipoM"].ToString().Trim());
                    else
                        texto = texto.Replace("{Tipo_Moneda}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["descTipoAmort"].ToString().Trim()))
                        texto = texto.Replace("{Tipo_Amortizacion}", dt.Rows[0]["descTipoAmort"].ToString());
                    else
                        texto = texto.Replace("{Tipo_Amortizacion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["NCertificado"].ToString()))
                        texto = texto.Replace("{Numero_Certificado}", dt.Rows[0]["NCertificado"].ToString().Trim());
                    else
                        texto = texto.Replace("{Numero_Certificado}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FechaEmisionC"].ToString()))
                        texto = texto.Replace("{Fecha_Emision_Larga}", dt.Rows[0]["FechaEmisionC"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Emision_Larga}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FecEmision"].ToString()))
                        texto = texto.Replace("{Fecha_Emision_Corta}", dt.Rows[0]["FecEmision"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Emision_Corta}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FechaEmisionC5"].ToString()))
                        texto = texto.Replace("{Fecha_Emision_Cursada}", dt.Rows[0]["FechaEmisionC5"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Emision_Cursada}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["fechaVencimiento"].ToString()))
                        texto = texto.Replace("{Fecha_Vencimiento}", dt.Rows[0]["fechaVencimiento"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Vencimiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["MontoOperacion"].ToString()))
                        texto = texto.Replace("{Monto_Operacion}", dt.Rows[0]["MontoOperacion"].ToString().Trim());
                    else
                        texto = texto.Replace("{Monto_Operacion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["MontoOperacionCLP"].ToString()))
                        texto = texto.Replace("{Monto_Operacion_CLP}", dt.Rows[0]["MontoOperacionCLP"].ToString().Trim());
                    else
                        texto = texto.Replace("{Monto_Operacion_CLP}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["numCredito"].ToString()))
                        texto = texto.Replace("{Es_Operacion}", dt.Rows[0]["numCredito"].ToString().Trim());
                    else
                        texto = texto.Replace("{Numero_Operacion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["descFondo"].ToString()))
                        texto = texto.Replace("{Fondo}", dt.Rows[0]["descFondo"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fondo}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescTipoOperacion"].ToString()))
                        texto = texto.Replace("{Tipo_Operacion}", dt.Rows[0]["DescTipoOperacion"].ToString().Trim());
                    else
                        texto = texto.Replace("{Tipo_Operacion}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["TasaIntBco"].ToString()))
                        texto = texto.Replace("{Tasa}", dt.Rows[0]["TasaIntBco"].ToString().Trim());
                    else
                        texto = texto.Replace("{Tasa}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Plazo"].ToString()))
                        texto = texto.Replace("{Plazo}", dt.Rows[0]["Plazo"].ToString().Trim());
                    else
                        texto = texto.Replace("{Plazo}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["plazoDias"].ToString()))
                        texto = texto.Replace("{Plazo_Dias}", dt.Rows[0]["plazoDias"].ToString().Trim());
                    else
                        texto = texto.Replace("{Plazo_Dias}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescPeriodic"].ToString()))
                        texto = texto.Replace("{Periodicidad}", dt.Rows[0]["DescPeriodic"].ToString().Trim());
                    else
                        texto = texto.Replace("{Periodicidad}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["fec1erVencimiento"].ToString()))
                        texto = texto.Replace("{Fecha_Primer_Vencimiento}", dt.Rows[0]["fec1erVencimiento"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Primer_Vencimiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["numCuotas"].ToString()))
                        texto = texto.Replace("{Numero_Cuotas}", dt.Rows[0]["numCuotas"].ToString().Trim());
                    else
                        texto = texto.Replace("{Numero_Cuotas}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["fechaVencimiento"].ToString()))
                        texto = texto.Replace("{Fecha_Ultimo_Vencimiento}", dt.Rows[0]["fechaVencimiento"].ToString().Trim());
                    else
                        texto = texto.Replace("{Fecha_Ultimo_Vencimiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["valorCuota"].ToString()))
                        texto = texto.Replace("{Valor_Cuotas}", dt.Rows[0]["valorCuota"].ToString().Trim());
                    else
                        texto = texto.Replace("{Valor_Cuotas}", "[S/D]");

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescRepresentanteLegal"].ToString()))
                        texto = texto.Replace("{Representante_Legal_1}", dt.Rows[0]["DescRepresentanteLegal"].ToString().Trim());
                    else
                        texto = texto.Replace("{Representante_Legal_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescRepresentanteFondo"].ToString()))
                        texto = texto.Replace("{Representante_Fondo_1}", dt.Rows[0]["DescRepresentanteFondo"].ToString().Trim());
                    else
                        texto = texto.Replace("{Representante_Fondo_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteLegal"].ToString()))
                        texto = texto.Replace("{Rut_Representante_Legal_1}", dt.Rows[0]["RutRepresentanteLegal"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Representante_Legal_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteFondo"].ToString()))
                        texto = texto.Replace("{Rut_Representante_Fondo_1}", dt.Rows[0]["RutRepresentanteFondo"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Representante_Fondo_1}", "[S/D]");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescRepresentanteLegal2"].ToString()))
                        texto = texto.Replace("{Representante_Legal_2}", dt.Rows[0]["DescRepresentanteLegal2"].ToString().Trim());
                    else
                        texto = texto.Replace("{Representante_Legal_2}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescRepresentanteFondo2"].ToString()))
                        texto = texto.Replace("{Representante_Fondo_2}", dt.Rows[0]["DescRepresentanteFondo2"].ToString().Trim());
                    else
                        texto = texto.Replace("{Representante_Fondo_2}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteLegal2"].ToString()))
                        texto = texto.Replace("{Rut_Representante_Legal_2}", dt.Rows[0]["RutRepresentanteLegal2"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Representante_Legal_2}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteFondo2"].ToString()))
                        texto = texto.Replace("{Rut_Representante_Fondo_2}", dt.Rows[0]["RutRepresentanteFondo2"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Representante_Fondo_2}", "[S/D]");

                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (!string.IsNullOrEmpty(dt.Rows[0]["periodoGracia"].ToString()))
                        texto = texto.Replace("{Periodo_Gracia}", dt.Rows[0]["periodoGracia"].ToString().Trim());
                    else
                        texto = texto.Replace("{Periodo_Gracia}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["plazoDias"].ToString()))
                        texto = texto.Replace("{Plazo_Dias}", dt.Rows[0]["plazoDias"].ToString().Trim());
                    else
                        texto = texto.Replace("{Plazo_Dias}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["glosa"].ToString()))
                        texto = texto.Replace("{Glosa}", dt.Rows[0]["glosa"].ToString().Trim());
                    else
                        texto = texto.Replace("{Glosa}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ObjetoSociedad"].ToString()))
                        texto = texto.Replace("{Objeto_Sociedad_SII}", dt.Rows[0]["ObjetoSociedad"].ToString().Trim());
                    else
                        texto = texto.Replace("{Objeto_Sociedad_SII}", "");

                    #endregion

                    #region datos acreedor

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutAcreedor"].ToString()))
                        texto = texto.Replace("{Rut_Acreedor}", dt.Rows[0]["RutAcreedor"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Acreedor}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["AcreedorRazonSocial"].ToString()))
                        texto = texto.Replace("{Acreedor_Razon_Social}", dt.Rows[0]["AcreedorRazonSocial"].ToString().Trim());
                    else
                        texto = texto.Replace("{Acreedor_Razon_Social}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DireccionAcreedor"].ToString()))
                        texto = texto.Replace("{Direccion_Acreedor}", dt.Rows[0]["DireccionAcreedor"].ToString().Trim());
                    else
                        texto = texto.Replace("{Direccion_Acreedor}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RegionAcreedor"].ToString()))
                        texto = texto.Replace("{Region}", dt.Rows[0]["RegionAcreedor"].ToString().Trim());
                    else
                        texto = texto.Replace("{Region}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ComunaAcreedor"].ToString()))
                        texto = texto.Replace("{Comuna}", dt.Rows[0]["ComunaAcreedor"].ToString().Trim());
                    else
                        texto = texto.Replace("{Comuna}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ProvinciaAcreedor"].ToString()))
                        texto = texto.Replace("{Provincia}", dt.Rows[0]["ProvinciaAcreedor"].ToString().Trim());
                    else
                        texto = texto.Replace("{Provincia}", "[S/D]");


                    #endregion

                    #region "datos avales"

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Avales"].ToString()))
                        texto = texto.Replace("{Avales_DatosPersonales_Pagare}", dt.Rows[0]["Avales"].ToString());
                    else
                        texto = texto.Replace("{Avales_DatosPersonales_Pagare}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Avales"].ToString()))
                        texto = texto.Replace("{Datos_Avales}", dt.Rows[0]["Avales"].ToString());
                    else
                        texto = texto.Replace("{Datos_Avales}", "");

                    if (dt2.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt2.Rows[0]["NombreAvales"].ToString()))
                            texto = texto.Replace("{Nombres_Avales}", dt2.Rows[0]["NombreAvales"].ToString());
                        else
                            texto = texto.Replace("{Nombres_Avales}", "");
                    }

                    if (dt3.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt3.Rows[0]["NombreRepresentantes"].ToString()))
                            texto = texto.Replace("{Nombres_Representantes}", dt3.Rows[0]["NombreRepresentantes"].ToString());
                        else
                            texto = texto.Replace("{Nombres_Representantes}", "");
                    }

                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[3].Rows[0]["FirmasRepresentantes"].ToString()))
                            texto = texto.Replace("{Firmas_Representantes}", Mensaje(ds.Tables[3].Rows[0]["FirmasRepresentantes"].ToString()));
                        else
                            texto = texto.Replace("{Firmas_Representantes}", "");
                    }

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[4].Rows[0]["FirmasAvales"].ToString()))
                            texto = texto.Replace("{Firmas_Avales}", Mensaje(ds.Tables[4].Rows[0]["FirmasAvales"].ToString()));
                        else
                            texto = texto.Replace("{Firmas_Avales}", "");
                    }

                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[5].Rows[0]["AvalesDireccion"].ToString()))
                            texto = texto.Replace("{Avales_Direccion_CGR}", Mensaje(ds.Tables[5].Rows[0]["AvalesDireccion"].ToString()));
                        else
                            texto = texto.Replace("{Avales_Direccion_CGR}", "");
                    }


                    if (ds.Tables[6].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[6].Rows[0]["FirmasRepresentantesRut"].ToString()))
                            texto = texto.Replace("{Firmas_Representantes_Rut}", Mensaje(ds.Tables[6].Rows[0]["FirmasRepresentantesRut"].ToString()));
                        else
                            texto = texto.Replace("{Firmas_Representantes_Rut}", "");
                    }


                    #endregion

                    #region garantias

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Ngarantias"].ToString()))
                    {
                        texto = texto.Replace("{Datos_Garantias}", dt.Rows[0]["Ngarantias"].ToString());
                    }
                    else
                        texto = texto.Replace("{Datos_Garantias}", "[S/D]");

                    #endregion

                    #region "otros"

                    if (!string.IsNullOrEmpty(dt.Rows[0]["FechaHoy"].ToString()))
                    {
                        texto = texto.Replace("{Fecha_Hoy}", dt.Rows[0]["FechaHoy"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Fecha_Hoy}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["dia"].ToString()))
                    {
                        texto = texto.Replace("{Dia}", dt.Rows[0]["dia"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Dia}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Mes"].ToString()))
                    {
                        texto = texto.Replace("{Mes}", dt.Rows[0]["Mes"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Mes}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["anio"].ToString()))
                    {
                        texto = texto.Replace("{Año}", dt.Rows[0]["anio"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Año}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Domicilio"].ToString()))
                    {
                        texto = texto.Replace("{Domicilio S.A.G.R.}", dt.Rows[0]["Domicilio"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Domicilio S.A.G.R.}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutSAGR"].ToString()))
                    {
                        texto = texto.Replace("{Rut S.A.G.R.}", dt.Rows[0]["RutSAGR"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Rut S.A.G.R.}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["SGR"].ToString()))
                    {
                        texto = texto.Replace("{Nombre S.A.G.R.}", dt.Rows[0]["SGR"].ToString().Trim());
                    }
                    else
                        texto = texto.Replace("{Nombre S.A.G.R.}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RepresentanteLegal"].ToString()))
                        texto = texto.Replace("{Representante_Legal_S.A.G.R}", dt.Rows[0]["RepresentanteLegal"].ToString().Trim());
                    else
                        texto = texto.Replace("{Representante_Legal_S.A.G.R}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteLegalSGR"].ToString()))
                        texto = texto.Replace("{Rut_Representante_Legal_S.A.G.R}", dt.Rows[0]["RutRepresentanteLegalSGR"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Representante_Legal_S.A.G.R}", "[S/D]");


                    #endregion

                    #region "datos fiscalia"

                    if (!string.IsNullOrEmpty(dt.Rows[0]["gastosOperacionales"].ToString()))
                        texto = texto.Replace("{Gastos_Legales}", dt.Rows[0]["gastosOperacionales"].ToString().Trim());
                    else
                        texto = texto.Replace("{Gastos_Legales}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["TimbreYEstampilla"].ToString()))
                        texto = texto.Replace("{Impuesto_Timbre_Estampilla}", dt.Rows[0]["TimbreYEstampilla"].ToString().Trim());
                    else
                        texto = texto.Replace("{Impuesto_Timbre_Estampilla}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RegionEmpresa"].ToString()))
                        texto = texto.Replace("{Region_Empresa}", dt.Rows[0]["RegionEmpresa"].ToString().Trim());
                    else
                        texto = texto.Replace("{Region_Empresa}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ComunaEmpresa"].ToString()))
                        texto = texto.Replace("{Comuna_Empresa}", dt.Rows[0]["ComunaEmpresa"].ToString().Trim());
                    else
                        texto = texto.Replace("{Comuna_Empresa}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescFinalidad"].ToString()))
                        texto = texto.Replace("{Finalidad}", dt.Rows[0]["DescFinalidad"].ToString().Trim());
                    else
                        texto = texto.Replace("{Finalidad}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DescFinalidadFiscalia"].ToString()))
                        texto = texto.Replace("{Finalidad_Fiscalia}", dt.Rows[0]["DescFinalidadFiscalia"].ToString().Trim());
                    else
                        texto = texto.Replace("{Finalidad_Fiscalia}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Comision"].ToString()))
                        texto = texto.Replace("{Comision_MultiAval}", dt.Rows[0]["Comision"].ToString().Trim());
                    else
                        texto = texto.Replace("{Comision_MultiAval}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["montoPropuestoPaf"].ToString()))
                        texto = texto.Replace("{Monto_Propuesto_Paf}", dt.Rows[0]["montoPropuestoPaf"].ToString().Trim());
                    else
                        texto = texto.Replace("{Monto_Propuesto_Paf}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["MontoOperacionPalabra"].ToString()))
                        texto = texto.Replace("{Monto_Operacion_Palabra}", dt.Rows[0]["MontoOperacionPalabra"].ToString().Trim());
                    else
                        texto = texto.Replace("{Monto_Operacion_Palabra}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DiaPrimerVencimiento"].ToString()))
                        texto = texto.Replace("{Dia_Primer_Vencimiento}", dt.Rows[0]["DiaPrimerVencimiento"].ToString().Trim());
                    else
                        texto = texto.Replace("{Dia_Primer_Vencimiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["MesPrimerVencimiento"].ToString()))
                        texto = texto.Replace("{Mes_Primer_Vencimiento}", dt.Rows[0]["MesPrimerVencimiento"].ToString().Trim());
                    else
                        texto = texto.Replace("{Mes_Primer_Vencimiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["AñoPrimerVencimiento"].ToString()))
                        texto = texto.Replace("{Año_Primer_Vencimiento}", dt.Rows[0]["AñoPrimerVencimiento"].ToString().Trim());
                    else
                        texto = texto.Replace("{Año_Primer_Vencimiento}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RepresentanteLegalEmpresa"].ToString()))
                        texto = texto.Replace("{Representante_Legal_Empresa}", Mensaje(dt.Rows[0]["RepresentanteLegalEmpresa"].ToString().Trim()));
                    else
                        texto = texto.Replace("{Representante_Legal_Empresa}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Rep_Legal_1"].ToString()))
                        texto = texto.Replace("{Rep_Legal_Empresa_1}", dt.Rows[0]["Rep_Legal_1"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rep_Legal_Empresa_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Rut_Rep_Legal_1"].ToString()))
                        texto = texto.Replace("{Rut_Rep_Legal_Empresa_1}", dt.Rows[0]["Rut_Rep_Legal_1"].ToString().Trim());
                    else
                        texto = texto.Replace("{Rut_Rep_Legal_Empresa_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Estado_Civil_Rep_Legal_1"].ToString()))
                        texto = texto.Replace("{Estado_Civil_Rep_Legal_Empresa_1}", dt.Rows[0]["Estado_Civil_Rep_Legal_1"].ToString().Trim());
                    else
                        texto = texto.Replace("{Estado_Civil_Rep_Legal_Empresa_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Profesion_Rep_Legal_1"].ToString()))
                        texto = texto.Replace("{Profesion_Rep_Legal_Empresa_1}", dt.Rows[0]["Profesion_Rep_Legal_1"].ToString().Trim());
                    else
                        texto = texto.Replace("{Profesion_Rep_Legal_Empresa_1}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Regimen"].ToString()))
                        texto = texto.Replace("{Regimen_Rep_Legal_Empresa_1}", dt.Rows[0]["Regimen"].ToString().Trim());
                    else
                        texto = texto.Replace("{Regimen_Rep_Legal_Empresa_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RepresentanteAvalEmpresa"].ToString()))
                        texto = texto.Replace("{Avales_Direccion_Pagare}", Mensaje(dt.Rows[0]["RepresentanteAvalEmpresa"].ToString().Trim()));
                    else
                        texto = texto.Replace("{Avales_Direccion_Pagare}", "[S/D]");

                    string mensaje = ListarGarantiasAnexo1;
                    if (mensaje != "-1-Error interno.")
                        texto = texto.Replace("{Garantias_Fiscalia}", mensaje);
                    else
                        texto = texto.Replace("{Garantias_Fiscalia}", "[S/D]");

                    if (!string.IsNullOrEmpty(TotalRepresentantes))
                        texto = texto.Replace("{Total_Representantes}", TotalRepresentantes);
                    else
                        texto = texto.Replace("{Total_Representantes}", "");

                    if (!string.IsNullOrEmpty(TotalAvales))
                        texto = texto.Replace("{Avales_DatosPersonales_CGR}", TotalAvales);
                    else
                        texto = texto.Replace("{Avales_DatosPersonales_CGR}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["DecFiscalia"].ToString()))
                        texto = texto.Replace("{Parrafo_Capital_Intereses}", dt.Rows[0]["DecFiscalia"].ToString().Trim());
                    else
                        texto = texto.Replace("{Parrafo_Capital_Intereses}", "");

                    ////
                    if (!string.IsNullOrEmpty(dt.Rows[0]["NombreA"].ToString()))
                        texto = texto.Replace("{Nombre_Representante_1}", dt.Rows[0]["NombreA"].ToString().Trim());
                    else
                        texto = texto.Replace("{Nombre_Representante_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["NacionalidadA"].ToString()))
                        texto = texto.Replace("{Nacionalidad_Representante_1}", dt.Rows[0]["NacionalidadA"].ToString().Trim());
                    else
                        texto = texto.Replace("{Nacionalidad_Representante_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["EstadoCivilA"].ToString()))
                        texto = texto.Replace("{EstadoCivil_Representante_1}", dt.Rows[0]["EstadoCivilA"].ToString().Trim());
                    else
                        texto = texto.Replace("{EstadoCivil_Representante_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ProfesionA"].ToString()))
                        texto = texto.Replace("{profesion_Representante_1}", dt.Rows[0]["ProfesionA"].ToString().Trim());
                    else
                        texto = texto.Replace("{profesion_Representante_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutTextoA"].ToString()))
                        texto = texto.Replace("{RutTexto_Representante_1}", dt.Rows[0]["RutTextoA"].ToString().Trim());
                    else
                        texto = texto.Replace("{RutTexto_Representante_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutNumeroA"].ToString()))
                        texto = texto.Replace("{RutNumero_Representante_1}", dt.Rows[0]["RutNumeroA"].ToString().Trim());
                    else
                        texto = texto.Replace("{RutNumero_Representante_1}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["NombreB"].ToString()))
                        texto = texto.Replace("{Nombre_Representante_2}", dt.Rows[0]["NombreB"].ToString().Trim());
                    else
                        texto = texto.Replace("{Nombre_Representante_2}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["NacionalidadB"].ToString()))
                        texto = texto.Replace("{Nacionalidad_Representante_2}", dt.Rows[0]["NacionalidadB"].ToString().Trim());
                    else
                        texto = texto.Replace("{Nacionalidad_Representante_2}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["EstadoCivilB"].ToString()))
                        texto = texto.Replace("{EstadoCivil_Representante_2}", dt.Rows[0]["EstadoCivilB"].ToString().Trim());
                    else
                        texto = texto.Replace("{EstadoCivil_Representante_2}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ProfesionB"].ToString()))
                        texto = texto.Replace("{profesion_Representante_2}", dt.Rows[0]["ProfesionB"].ToString().Trim());
                    else
                        texto = texto.Replace("{profesion_Representante_2}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutTextoB"].ToString()))
                        texto = texto.Replace("{RutTexto_Representante_2}", dt.Rows[0]["RutTextoB"].ToString().Trim());
                    else
                        texto = texto.Replace("{RutTexto_Representante_2}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutNumeroB"].ToString()))
                        texto = texto.Replace("{RutNumero_Representante_2}", dt.Rows[0]["RutNumeroB"].ToString().Trim());
                    else
                        texto = texto.Replace("{RutNumero_Representante_2}", "");

                    #endregion

                    #region "impuesto timbre"

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ImpuestoPagare13Meses"].ToString()))
                        texto = texto.Replace("{Impuesto_Pagare_13_meses}", dt.Rows[0]["ImpuestoPagare13Meses"].ToString().Trim());
                    else
                        texto = texto.Replace("{Impuesto_Pagare_13_meses}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ImpuestoPagare12Meses"].ToString()))
                        texto = texto.Replace("{Impuesto_Pagare_12_meses}", dt.Rows[0]["ImpuestoPagare12Meses"].ToString().Trim());
                    else
                        texto = texto.Replace("{Impuesto_Pagare_12_meses}", "");


                    if (!string.IsNullOrEmpty(dt.Rows[0]["ImpuestoPagareSP"].ToString()))
                        texto = texto.Replace("{Impuesto_Pagare_Sin_Plazo}", dt.Rows[0]["ImpuestoPagareSP"].ToString().Trim());
                    else
                        texto = texto.Replace("{Impuesto_Pagare_Sin_Plazo}", "");


                    if (!string.IsNullOrEmpty(dt.Rows[0]["TotalImpuestoTimbre"].ToString()))
                        texto = texto.Replace("{Total_Impuesto_Timbre}", dt.Rows[0]["TotalImpuestoTimbre"].ToString().Trim());
                    else
                        texto = texto.Replace("{Total_Impuesto_Timbre}", "");


                    #endregion

                    #region "empresa personas"

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Calle"].ToString()))
                        texto = texto.Replace("{Calle}", dt.Rows[0]["Calle"].ToString().Trim());
                    else
                        texto = texto.Replace("{Calle}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ComplementoDireccion"].ToString()))
                        texto = texto.Replace("{Complemento_Direccion}", dt.Rows[0]["ComplementoDireccion"].ToString().Trim());
                    else
                        texto = texto.Replace("{Complemento_Direccion}", "");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Numero"].ToString()))
                    {
                        if (dt.Rows[0]["Numero"].ToString().IsNumeric())
                            texto = texto.Replace("{Numero}", "número" + " " + dt.Rows[0]["Numero"].ToString().Trim());
                        else
                            texto = texto.Replace("{Numero}", "");
                    }
                    else
                    {
                        texto = texto.Replace("{Numero}", "");
                    }


                    if (!string.IsNullOrEmpty(dt.Rows[0]["ComunaEmpresa"].ToString()))
                        texto = texto.Replace("{ComunaEmpresa}", dt.Rows[0]["ComunaEmpresa"].ToString().Trim());
                    else
                        texto = texto.Replace("{ComunaEmpresa}", "[S/D]");


                    if (!string.IsNullOrEmpty(dt.Rows[0]["RegionEmpresa"].ToString()))
                        texto = texto.Replace("{RegionEmpresa}", dt.Rows[0]["RegionEmpresa"].ToString().Trim());
                    else
                        texto = texto.Replace("{RegionEmpresa}", "[S/D]");

                    if (!string.IsNullOrEmpty(dt.Rows[0]["ComplementoDireccion"].ToString()))
                        texto = texto.Replace("{ComplementoDireccion}", dt.Rows[0]["ComplementoDireccion"].ToString().Trim());
                    else
                        texto = texto.Replace("{ComplementoDireccion}", "[S/D]");

                    #endregion
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return textoSinAcentos = texto;
        }

        public string GetSAMAccountName(string Nombre, string Ldap)
        {
            var oroot = new System.DirectoryServices.DirectoryEntry(Ldap);
            var osearcher = new System.DirectoryServices.DirectorySearcher(oroot);
            osearcher.Filter = string.Format("(&(DisplayName={0}))", Nombre);
            //osearcher.Filter = string.Format("(&(sAMAccountName={0}))", sAMAccountName);
            var oresult = osearcher.FindAll();

            if (oresult.Count == 0) throw new InvalidOperationException(string.Format("Usuario no encontrado {0} in LDAP.", Nombre));
            if (oresult.Count > 1) throw new InvalidOperationException(string.Format("There are {0} items with sAMAccountName {1} in LDAP.", oresult.Count, Nombre));

            return oresult[0].Properties["sAMAccountName"][0].ToString();
        }

        public DataTable GenerarDataTable(ASPxGridView gv)
        {
            DataTable dt = new DataTable();

            foreach (GridViewColumn col in gv.Columns)
            {
                GridViewDataColumn dataColumn = col as GridViewDataColumn;

                if (dataColumn == null) continue;

                dt.Columns.Add(dataColumn.FieldName);
            }
            for (int i = 0; i < gv.VisibleRowCount; i++)
            {
                DataRow row = dt.Rows.Add();
                foreach (DataColumn col in dt.Columns)
                    row[col.ColumnName] = gv.GetRowValues(i, col.ColumnName);
            }
            return dt;
        }

        public string generarXML(DataTable dt)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            Utilidades util = new Utilidades();

            foreach (DataRow row in dt.Rows)
            {
                RespNode = doc.CreateElement("Val");

                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column] != null)
                    {
                        var field1 = column.ColumnName; //row[column].ToString();
                        var text = row[column].ToString();

                        var t = row[column].GetType();
                        //if (row[column].GetType() == typeof(int))
                        var value = string.Empty;
                        if (column.ColumnName == "PorcComision")
                            value = row[column].ToString().Replace(",", ".");
                        else
                            value = row[column].ToString().Replace(".", "");

                        XmlNode nodo0 = doc.CreateElement(field1);
                        nodo0.AppendChild(doc.CreateTextNode(util.LimpiarTexto(value)));
                        RespNode.AppendChild(nodo0);

                        ValoresNode.AppendChild(RespNode);
                    }
                }
            }
            return doc.OuterXml;
        }

        public byte[] convierteExcel(DataTable table)
        {
            //IWorkbook workbook;
            byte[] ret = null;
            HSSFWorkbook workbook = new HSSFWorkbook();
            //workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
            var sheet = workbook.CreateSheet("Cuenta Corriente");
            var headStyle = workbook.CreateCellStyle();
            NPOI.SS.UserModel.IFont font = workbook.CreateFont();

            font.Boldweight = Convert.ToInt16(NPOI.SS.UserModel.FontBoldWeight.Bold);
            headStyle.SetFont(font);
            //headStyle.FillBackgroundColor()
            // CABECERA
            var rowh = sheet.CreateRow(0);
            for (int i = 0; (i <= (table.Columns.Count - 1)); i++)
            {
                var cell = rowh.CreateCell(i);
                cell.SetCellValue(table.Columns[i].ColumnName);
                cell.CellStyle = headStyle;
                // cell.CellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index2
            }

            // CONTENIDO
            var cellStyle = workbook.CreateCellStyle();
            NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
            cellStyle.SetFont(cellfont);
            for (int j = 0; (j <= (table.Rows.Count - 1)); j++)
            {
                var rowc = sheet.CreateRow((1 + j));
                for (int i = 0; (i <= (table.Columns.Count - 1)); i++)
                {
                    NPOI.SS.UserModel.ICell cell = rowc.CreateCell(i);
                    cell.CellStyle = cellStyle;
                    double dbl = 0;
                    if (double.TryParse(table.Rows[j][i].ToString(), out dbl))
                    {
                        cell.SetCellValue(dbl);
                    }
                    else
                    {
                        cell.SetCellValue(table.Rows[j][i].ToString());
                    }
                }
            }

            using (MemoryStream exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                ret = exportData.GetBuffer();
            }

            return ret;
        }

        //public string getBetween(string strSource, string strStart, string strEnd)
        //{
        //    int Start, End;
        //    if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        //    {
        //        Start = strSource.IndexOf(strStart, 0) + strStart.Length;
        //        End = strSource.IndexOf(strEnd, Start);
        //        return strSource.Substring(Start, End - Start);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        public byte[] bajarWord(string codigoHtml, string NombrePlantilla)
        {
            try
            {
                string myString = string.Empty;
                UTF8Encoding utf8 = new UTF8Encoding();
                string html = codigoHtml;
                MemoryStream generatedDocument = null;
                MainDocumentPart mainPart = null;
                using (generatedDocument = new MemoryStream(1000000))
                {
                    using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                    {
                        mainPart = package.MainDocumentPart;
                        if (mainPart == null)
                        {
                            mainPart = package.AddMainDocumentPart();
                            new Document(new Body()).Save(mainPart);
                        }

                        HtmlConverter converter = new HtmlConverter(mainPart);

                        //converter.BaseImageUrl = new Uri(Page.Request.Url.Scheme + "://" + Page.Request.Url.Authority);
                        Body body = mainPart.Document.Body;
                        Paragraph pr = new Paragraph();

                        //byte[] bytes = Encoding.Default.GetBytes(html);
                        //html = Encoding.UTF8.GetString(bytes);
                        var paragraphs = converter.Parse(html);



                        for (int i = 0; i < paragraphs.Count; i++)
                        {
                            body.Append(paragraphs[i]);
                        }

                        mainPart.Document.Save();
                    }

                    generatedDocument.Close();

                    MemoryStream dolly = new MemoryStream(generatedDocument.ToArray());
                    dolly.Seek(0, SeekOrigin.Begin);
                    byte[] bytesInStream = dolly.ToArray();
                    var msDoc = new MemoryStream(1000000);
                    msDoc.Write(bytesInStream, 0, int.Parse(dolly.Length.ToString()));
                    msDoc.Position = 0;

                    return msDoc.ToArray();

                    //using (WordprocessingDocument doc = WordprocessingDocument.Open(msDoc, true))
                    //{
                    //    //str.Write(1000000, 0, str.Length);
                    //    str.Position = 0;
                    //    var mainDocPart = doc.MainDocumentPart;
                    //    if (doc == null)
                    //    {
                    //        mainDocPart = doc.AddMainDocumentPart();
                    //    }

                    //    if (mainDocPart.Document == null)
                    //    {
                    //        mainDocPart.Document = new Document();
                    //    }

                    //    ApplyHeader(doc);
                    //    //ApplyFooter(doc);
                    //}


                    //Page.Response.Clear();
                    //Page.Response.ContentType = contentType;
                    //Page.Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".docx");

                    //Page.Response.BinaryWrite(msDoc.ToArray());
                    //Page.Response.Flush();
                    //Page.Response.SuppressContent = true;
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                //HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();
                //string headerPartId = mainPart.GetIdOfPart(headerPart);
                //GenerateHeaderPartContent(headerPart);

                //IEnumerable<SectionProperties> sections = mainPart.Document.Body.Elements<SectionProperties>();  //body.Elements<SectionProperties>(); 

                //IEnumerable<DocumentFormat.OpenXml.Wordprocessing.SectionProperties> sectPrs = mainPart.Document.Body.Elements<SectionProperties>();

                //foreach (var section in sections)
                //{
                //    // Delete existing references to headers and footers
                //    section.RemoveAllChildren<HeaderReference>();
                //    //section.RemoveAllChildren<FooterReference>();

                //    // Create the new header and footer reference node
                //    section.PrependChild<HeaderReference>(new HeaderReference() { Id = headerPartId });
                //    //section.PrependChild<FooterReference>(new FooterReference() { Id = footerPartId });
                //}

            }
            catch
            {
                return null;
                //warning(ex.Message);
            }
        }


        public void CargaDDLDx(ASPxComboBox DDL, DataTable dt, string text, string value)
        {
            DDL.DataSource = dt;
            DDL.TextField = text;
            DDL.ValueField = value;
            DDL.DataBindItems();
            //DDL.SelectedIndex = 0;
        }

        public void CargaDDLDxx(ASPxComboBox DDL, DataTable dt, string text, string value)
        {
            DDL.DataSource = dt;
            DDL.TextField = text;
            DDL.ValueField = value;
            DDL.DataBind();
            //DDL.DataBindItems();
            DDL.Items.Insert(0, new ListEditItem("", "0"));
            DDL.SelectedIndex = 0;
        }

        public void CargaDDLDxx1(ASPxComboBox DDL, DataTable dt, string text, string value)
        {
            DDL.DataSource = dt;
            DDL.TextField = text;
            DDL.ValueField = value;
            DDL.DataBind();
            DDL.Items.Insert(0, new ListEditItem("--Seleccione--", "0"));
            DDL.SelectedIndex = 0;
        }

        public void CargaDDLDxx2(ASPxComboBox DDL, DataTable dt, string text, string value)
        {
            DDL.DataSource = dt;
            DDL.TextField = text;
            DDL.ValueField = value;
            DDL.DataBind();
        }

        public void CargaDDL(DropDownList DDL, DataTable dt, string text, string value)
        {
            DDL.Items.Clear();
            DDL.DataSource = dt;
            DDL.DataTextField = text;
            DDL.DataValueField = value;
            DDL.DataBind();
            DDL.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Seleccione", "0"));
        }

        public void CargaDDL1(DropDownList DDL, DataTable dt, string text, string value)
        {
            DDL.Items.Clear();
            DDL.DataSource = dt;
            DDL.DataTextField = text;
            DDL.DataValueField = value;
            DDL.DataBind();
        }

        public int ValidarDocCriticos(DataTable dtArchivosCriticos, DataTable dtArchivos)
        {
            int val = 0;
            string DescTipoServicio = String.Empty;
            foreach (DataRow row in dtArchivosCriticos.Rows)
            {
                DescTipoServicio = row["DescServicio"].ToString().Trim();

                foreach (DataRow rowArch in dtArchivos.Rows)
                {
                    if (rowArch["Tipo Documento"].ToString().Trim().ToLower() == DescTipoServicio.ToLower())
                        val++;
                }
            }

            return val;
        }

        public bool EstaPermitido(string valor, string[] permitidos)
        {
            bool Autorizado = false;
            foreach (var c in permitidos)
            {
                if (valor == c)
                    Autorizado = true;
            }

            return Autorizado;
        }

        ////void mensajeError(System.Web.UI.Control dvError, Label lbError, string mensaje)
        ////{
        ////    dvError.Style.Add("display", "block");
        ////    lbError.Text = mensaje;
        ////}

        ////void mensajeAlerta(System.Web.UI.Control dvWarning, Label lbWarning, string mensaje)
        ////{
        ////    dvWarning.Style.Add("display", "block");
        ////    lbWarning.Text = mensaje;
        ////}

        ////void mensajeExito(System.Web.UI.Control dvSuccess, Label lbSuccess, string mensaje)
        ////{
        ////    dvSuccess.Style.Add("display", "block");
        ////    lbSuccess.Text = mensaje;
        ////}

        public DataTable CSVtoDataTable(UploadedFile upFile, string delimiter)
        {
            try
            {
                //byte[] data = System.Text.Encoding.UTF8.GetBytes(upFile.FileBytes.ToString());
                var buffer = new byte[upFile.FileContent.Length];
                upFile.PostedFile.InputStream.Seek(0, SeekOrigin.Begin);
                upFile.PostedFile.InputStream.Read(buffer, 0, Convert.ToInt32(upFile.FileContent.Length));
                Stream stream2 = new MemoryStream(buffer);

                return ExportarArchivoCSVaDataTable(stream2, delimiter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ExportarArchivoCSVaDataTable(Stream stream, string delimitador)
        {
            var csvData = new DataTable();
            try
            {
                using (var csvReader = new TextFieldParser(stream, System.Text.Encoding.GetEncoding("iso-8859-1")))
                {
                    csvReader.SetDelimiters(new string[] { delimitador });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        var datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();

                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            fieldData[i] = HttpUtility.HtmlDecode(fieldData[i]).Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return csvData;
        }

        public string DatasetToXml(DataSet ds)
        {
            Xml result = new Xml();
            return ds.GetXml();
        }

        public string LimpiarTexto(string strIn)
        {
            return HttpUtility.HtmlDecode(strIn);
        }

        public string TraeDescripcionEnum(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        #endregion
    }
}
