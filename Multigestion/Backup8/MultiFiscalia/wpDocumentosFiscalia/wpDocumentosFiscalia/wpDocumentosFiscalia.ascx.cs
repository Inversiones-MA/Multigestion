using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint.Utilities;
using System.Web;
using System.Globalization;
using MultigestionUtilidades;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;
using System.Linq;

namespace MultiFiscalia.wpDocumentosFiscalia.wpDocumentosFiscalia
{
    [ToolboxItemAttribute(false)]
    public partial class wpDocumentosFiscalia : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDocumentosFiscalia()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "DocumentosFiscalia.aspx";
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            DataTable tabla = new DataTable("dt");
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = objresumen.area;

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];

                        lbEmpresa.Text = objresumen.desEmpresa.ToString();
                        lbOperacion.Text = objresumen.desOperacion.ToString();
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        ocultarDiv();
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                    }
                }

                CargarDocFiscalia();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                System.Web.UI.Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
                //Control divFiltros = this.FindControl("filtros");
                //Control divGrilla = this.FindControl("grilla");

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    warning("Usuario sin permisos configurados");
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                warning("Usuario sin permisos configurados");
            }
        }

        protected void gridDocumentosFiscalia_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }

        protected void gridDocumentosFiscalia_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        protected void gridDocumentosFiscalia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gridDocumentosFiscalia.SelectedRow;
                int codDoc = Convert.ToInt32(gridDocumentosFiscalia.DataKeys[row.RowIndex].Values["IdPlantillaDocumento"]);
            }
            catch
            {

            }
        }

        protected void gridDocumentosFiscalia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();

            if (e.CommandName == "Descargar")
            {
                DataTable dt = new DataTable("dt");
                Dictionary<string, string> datos = new Dictionary<string, string>();

                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                string DescPlantilla = arg[1];
                string idPlantilla = arg[0];
                string TotalRepresentantes = string.Empty;
                string TotalAvales = string.Empty;

                dt = LN.ListarPlantillaById(Convert.ToInt32(idPlantilla));

                if (dt.Rows.Count > 0)
                {
                    ocultarDiv();
                    DataSet ds = new DataSet();
                    ds = LN.CargarDatosHtml(Convert.ToInt32(objresumen.idOperacion.ToString()));
                    string ListarGarantiasAnexo1 = util.Mensaje(LN.ListarGarantiasAnexo1(objresumen.idEmpresa));
                    byte[] archivo = null;
        
                    TotalRepresentantes = LN.ListarRepresentantes(objresumen.idEmpresa);
                    TotalAvales = LN.ListarAvales(objresumen.idEmpresa);

                    string codigoHtml = util.ReemplazarValores(dt.Rows[0]["ContenidoHtml"].ToString(), ds, ListarGarantiasAnexo1, TotalRepresentantes, TotalAvales);
                    string NombrePlantilla = dt.Rows[0]["NombrePlantilla"].ToString();
                    if (!string.IsNullOrEmpty(codigoHtml))
                    {
                        string contenido = HttpUtility.HtmlDecode(codigoHtml);
                        Page.Session["Titulo"] = NombrePlantilla;
                        Page.Session["tipoDoc"] = dt.Rows[0]["TipoDocumento"].ToString();

                        if (dt.Rows[0]["TipoDocumento"].ToString().ToLower() == "docx")
                        {
                            //bajarWord(codigoHtml, NombrePlantilla);
                            archivo = util.bajarWord(codigoHtml, NombrePlantilla);
                            Page.Session["binaryData"] = archivo;
                            Page.Session["Titulo"] = NombrePlantilla.Trim();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
                        }
                        else
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append("<table width='1000px'><tr><td align='center'><table width='900px'><tr><td>" + contenido + "</td></tr></table></td></tr></table>");
                            Page.Session["tipoDoc"] = "pdf";
                            archivo = util.PdfFiscalia(sb);
                        }

                        if (archivo != null)
                        {
                            Page.Session["binaryData"] = archivo;
                            Page.Session["Titulo"] = NombrePlantilla.Trim();
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
                        }
                    }
                    else
                    {
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "revise la informacion necesaria: * Direccion Principal de la empresa";
                    }
                }
            }
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosCurse");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void warning(string mensaje)
        {
            dvWarning1.Style.Add("display", "block");
            lbWarning1.Text = mensaje;
        }

        private void CargarDocFiscalia()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = new DataTable("dt");
            string GarantiaFogape = "0";

            dt = LN.ConsultarDocumentosParametrizados(0, objresumen.IdAcreedor, "fiscalia");

            //verificar si la operacion se cursara con una garantia fogape
            GarantiaFogape = LN.TieneGarantiaFogape(objresumen.idEmpresa);

            if (GarantiaFogape == "0")
            {
                DataRow[] rows;
                rows = dt.Select("NombrePlantilla = 'DECLARACION JURADA FOGAPE' ");
                foreach (DataRow row in rows)
                    dt.Rows.Remove(row);
            }

            gridDocumentosFiscalia.DataSource = dt;
            gridDocumentosFiscalia.DataBind();

            if (dt.Rows.Count > 0)
                ocultarDiv();
            else
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = "No hay documentos asociados";
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        public void ApplyHeader(WordprocessingDocument doc)
        {
            MainDocumentPart mainDocPart = doc.MainDocumentPart;
            HeaderPart headerPart1 = mainDocPart.AddNewPart<HeaderPart>("r97");
            //ImagePart imagePart = mainDocPart.AddNewPart<ImagePart>("image/jpeg", fileName);

            ImagePart imagePart = mainDocPart.AddImagePart(ImagePartType.Jpeg);
            string fileName = @"C:\multigestion\MultiFiscalia\Images\MultiFiscalia\Enviar.png";  //System.Web.HttpContext.Current.Server.MapPath();

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                imagePart.FeedData(stream);
            }

            //var imagePart = doc.MainDocumentPart.AddNewPart<ImagePart>("image/jpeg", "rIdImagePart1");
            //GenerateImagePart(imagePart);

            MemoryStream ms = new MemoryStream();
            Header header1 = new Header();
            Paragraph paragraph1 = new Paragraph() { };
            Run run1 = new Run();

            Text text1 = new Text();
            text1.Text = "un texto cualquiera";

            //run1.Append(ms.ToArray());
            run1.Append(text1);
            paragraph1.Append(run1);

            header1.Append(paragraph1);
            headerPart1.Header = header1;

            SectionProperties sectionProperties1 = mainDocPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
            if (sectionProperties1 == null)
            {
                sectionProperties1 = new SectionProperties() { };
                mainDocPart.Document.Body.Append(sectionProperties1);
            }
            HeaderReference headerReference1 = new HeaderReference() { Type = HeaderFooterValues.Default, Id = "r97" };
            sectionProperties1.InsertAt(headerReference1, 0);


            //var element = "imagen";
            //mainDocPart.Document.Body.PrependChild(new Paragraph(new Run(element)));
        }

        #endregion
     
        //private void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        //{
        //    long imageWidthEMU = 3800000;
        //    long imageHeightEMU = 700000;
        //    double imageWidthInInches = imageWidthEMU / 914400.0;
        //    double imageHeightInInches = imageHeightEMU / 914400.0;
        //}

    }
}
