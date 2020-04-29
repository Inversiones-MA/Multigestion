using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Dsp;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using MultigestionUtilidades;
using System.Configuration;

namespace MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento
{
    [ToolboxItemAttribute(false)]
    public partial class wpPlantillaDocumento : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPlantillaDocumento()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ASPxWebControl.SetIECompatibilityModeEdge();
        }

        Utilidades util = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            //HtmlEditorGenerica.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            //string id = HtmlEditorGenerica.ClientID.ToString();
            //HtmlEditorGenerica.ClientInstanceName = "HtmlEditorGenerica"; //id.Substring(51,18);
            
            if (!Page.IsCallback)
            {
                CargarTipoProducto();
                CargarAcreedores();
                CargarDatosEmpresa();
                CargarDatosOperacion();
                CargarDatosAcreedor();
                CargarDatosAvales();
                CargarDatosGarantias();
                CargarDatosOtros();
                CargarDocumentos(new LNplantilla());
            }
        }

        private void CargarTipoProducto()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["Productos"];
            SPQuery query = new SPQuery();
            query.Query = "<Where>" +
                            "<Eq><FieldRef Name='TipoProducto'/><Value Type='Number'>1</Value></Eq>" +
                          "</Where>";
            SPListItemCollection collListItems = items.GetItems(query);

            CmbxTipoProductoGenerica.DataSource = collListItems.GetDataTable();
            CmbxTipoProductoGenerica.TextField = "Title";
            CmbxTipoProductoGenerica.ValueField = "ID";
            CmbxTipoProductoGenerica.DataBind();

            CmbxTipoProductoBuscar.DataSource = collListItems.GetDataTable();
            CmbxTipoProductoBuscar.TextField = "Title";
            CmbxTipoProductoBuscar.ValueField = "ID";
            CmbxTipoProductoBuscar.DataBind();
        }

        private void CargarTipoDocumento(string tipoProducto)
        {
            string IdTipoProducto = "3";
            string CFT = string.Empty;
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["TipoDocumento"];
            SPQuery query = new SPQuery();

            query.Query = "<Where>" +
                "<And>" +
                "<And>" +
                "<Or>" +
                    "<And>" +
                        "<And>" +
                            "<Eq><FieldRef Name='Area'/><Value Type='Lookup'>Operaciones</Value></Eq>" +
                            "<Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq>" +
                        "</And>" +
                            "<Eq><FieldRef Name='IdTipoProducto'/><Value Type='Number'>" + tipoProducto + "</Value></Eq>" +
                    "</And>" +
                      "<Eq><FieldRef Name='IdTipoProducto'/><Value Type='Number'>" + IdTipoProducto + "</Value></Eq>" +
                "</Or>" +
                "<Neq><FieldRef Name='Nombre'/><Value Type='Text'>Instrucción de Curse</Value></Neq>" +
                "</And>" +
                "<Neq><FieldRef Name='Nombre'/><Value Type='Text'>Solicitud de Emisión</Value></Neq>" +
                "</And>" +
                "</Where>";

            SPListItemCollection collListItems = items.GetItems(query);

            CmbxTipoDocumentoGenerica.DataSource = collListItems.GetDataTable();  //dtcamltest; //
            CmbxTipoDocumentoGenerica.TextField = "Nombre";
            CmbxTipoDocumentoGenerica.ValueField = "ID";
            CmbxTipoDocumentoGenerica.DataBind();

            CmbxTipoDocumentoBuscar.DataSource = collListItems.GetDataTable(); //dtcamltest;
            CmbxTipoDocumentoBuscar.TextField = "Nombre";
            CmbxTipoDocumentoBuscar.ValueField = "ID";
            CmbxTipoDocumentoBuscar.DataBind();

            //foreach (ListEditItem item in CmbxTipoDocumentoBuscar.Items)
            //{
            //    if (item.Text == "Solicitud de Emisión")
            //        CmbxTipoDocumentoBuscar.Items.Remove(item);
            //    if (item.Text == "Instruccion Curse")
            //        CmbxTipoDocumentoBuscar.Items.Remove(item);
            //}
        }

        private void CargarAcreedores()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["Acreedores"];
            SPQuery query = new SPQuery();

            query.Query = "<Where>"+
                             "<Or>"+
                               "<Eq><FieldRef Name='TipoAcreedor'/><Value Type='Choice'>Intermediario</Value></Eq>" +
                               "<Eq><FieldRef Name='TipoAcreedor'/><Value Type='Choice'>Banco</Value></Eq>" +
                             "</Or>"+      
                          "</Where>" +
                          "<OrderBy>" + "<FieldRef Name='TipoAcreedor' Ascending='True' />" + "</OrderBy>";

            SPListItemCollection collListItems = items.GetItems(query);

            CmbxAcreedorGenerica.DataSource = collListItems.GetDataTable();
            CmbxAcreedorGenerica.TextField = "Nombre";
            CmbxAcreedorGenerica.ValueField = "ID";
            CmbxAcreedorGenerica.DataBind();

            CmbxAcreedorBuscar.DataSource = collListItems.GetDataTable();
            CmbxAcreedorBuscar.TextField = "Nombre";
            CmbxAcreedorBuscar.ValueField = "ID";
            CmbxAcreedorBuscar.DataBind();         
        }

        private void CargarDatosEmpresa()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Direccion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Fecha_Contrato}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Razon_social}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Rut}";
            dt.Rows.Add(row);

            LstDatosEmpresaGenerica.DataSource = dt;
            LstDatosEmpresaGenerica.ValueField = "Codigo";
            LstDatosEmpresaGenerica.TextField = "Descripcion";
            LstDatosEmpresaGenerica.DataBind();
        }

        private void CargarDatosOperacion()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Acreedor}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 30;
            row["Descripcion"] = "{Banco_Factoring}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Cobertura_Afianzamiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Cobertura_Fogape}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Cobertura_Certificado}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Fogape}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Fogape_Estado}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{Fondo}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Fecha_Emision}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 7;
            row["Descripcion"] = "{Fecha_Emision_Dia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Fecha_Emision_Mes}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 9;
            row["Descripcion"] = "{Fecha_Emision_Año}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 10;
            row["Descripcion"] = "{Fecha_Primer_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{Fecha_Ultimo_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 12;
            row["Descripcion"] = "{Fondo_Garantia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 13;
            row["Descripcion"] = "{Monto_Operacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 14;
            row["Descripcion"] = "{Monto_Operacion_CLP}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 16;
            row["Descripcion"] = "{Numero_Cuotas}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 17;
            row["Descripcion"] = "{Numero_Documento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 18;
            row["Descripcion"] = "{Numero_Certificado}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 19;
            row["Descripcion"] = "{Numero_Operacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 19;
            row["Descripcion"] = "{Plazo}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 20;
            row["Descripcion"] = "{Plazo_Dias}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 21;
            row["Descripcion"] = "{Periodo_Gracia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 22;
            row["Descripcion"] = "{Periodicidad}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 23;
            row["Descripcion"] = "{Tipo_Operacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 24;
            row["Descripcion"] = "{Tipo_Amortizacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 25;
            row["Descripcion"] = "{Tipo_Cuota}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 26;
            row["Descripcion"] = "{Tasa_Interes}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 27;
            row["Descripcion"] = "{Tipo_Moneda}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 28;
            row["Descripcion"] = "{Valor_Cuotas}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 29;
            row["Descripcion"] = "{Moneda_Descripcion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 30;
            row["Descripcion"] = "{Moneda_Signo}";
            dt.Rows.Add(row);

            LstDatosOperacionGenerica.DataSource = dt;
            LstDatosOperacionGenerica.ValueField = "Codigo";
            LstDatosOperacionGenerica.TextField = "Descripcion";
            LstDatosOperacionGenerica.DataBind();
        }

        private void CargarDatosAcreedor()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Rut_Acreedor}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Acreedor_Razon_Social}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Direccion_Acreedor}";
            dt.Rows.Add(row);

            LstDatosAcreedorGenerica.DataSource = dt;
            LstDatosAcreedorGenerica.ValueField = "Codigo";
            LstDatosAcreedorGenerica.TextField = "Descripcion";
            LstDatosAcreedorGenerica.DataBind();
        }

        private void CargarDatosGarantias()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Fecha_Hoy}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Dia_Hoy}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Mes_Hoy}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Año_Hoy}";
            dt.Rows.Add(row);

            LstDatosGarantiaGenerica.DataSource = dt;
            LstDatosGarantiaGenerica.ValueField = "Codigo";
            LstDatosGarantiaGenerica.TextField = "Descripcion";
            LstDatosGarantiaGenerica.DataBind();
        }

        private void CargarDatosAvales()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Datos_Avales}";
            dt.Rows.Add(row);

            LstDatosAvalesGenerica.DataSource = dt;
            LstDatosAvalesGenerica.ValueField = "Codigo";
            LstDatosAvalesGenerica.TextField = "Descripcion";
            LstDatosAvalesGenerica.DataBind();
        }

        private void CargarDatosOtros()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Fecha_Hoy}";
            dt.Rows.Add(row);

            LstOtrosGenerica.DataSource = dt;
            LstOtrosGenerica.ValueField = "Codigo";
            LstOtrosGenerica.TextField = "Descripcion";
            LstOtrosGenerica.DataBind();
        }

        public string GuardarPlantilla()
        {
            LNplantilla plantilla = new LNplantilla();
            LogicaNegocio LN = new LogicaNegocio();
            try
            {
                Resumen resumenP = new Resumen();
                SPWeb app2 = SPContext.Current.Web;
                resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);

                //MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla plantilla = new MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla();
                
                if (ChkGenerica.Checked)
                {
                    int? Id = null;
                    if (HdfC["cambio"].ToString() == "no")
                         plantilla.IdPlantillaDocumento = null;
                    else
                        plantilla.IdPlantillaDocumento = Convert.ToInt32(Page.Session["IdValue"]);
                    
                    plantilla.NombrePlantilla = "nombre de plantilla"; //TxtNombrePlantillaGenerica.Text;
                    
                    //if (HtmlEditorGenerica.Html.Contains("../UploadFiles/Images/"))
                            //HtmlEditorGenerica.Html = HtmlEditorGenerica.Html.Replace("../UploadFiles/Images/", "/UploadFiles/Images/");
                          
                    //plantilla.ContenidoHtml = HtmlEditorGenerica.Html.Replace("/UploadFiles/Images/", "C:/inetpub/wwwroot/wss/VirtualDirectories/" + ConfigurationManager.AppSettings["PuertoSitio"].ToString() + "/UploadFiles/Images/");

                    plantilla.ContenidoHtml = System.Web.HttpUtility.HtmlDecode(HtmlEditorGenerica.Html);
                    plantilla.EsGenerica = ChkGenerica.Checked;
                    plantilla.UsuarioModificacion = resumenP.idUsuario;
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoGenerica.Value);
                    plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoGenerica.Value);
                    plantilla.IdAcreedor = 0; //Convert.ToInt32(CmbxAcreedorGenerica.Value);

                    if (plantilla.IdPlantillaDocumento == null)
                     {
                        Id = LN.InsertarPlantilla(plantilla);
                                if (Id == null || Id == 0)
                                  return "error";  
                     }
                    else
                     {
                        Id = LN.ActualizarPlantilla(plantilla);
                        if (Id == null || Id == 0)
                             return "error";
                        }
                }
                else
                {
                    int? Id = null;
                    if (HdfC["cambio"].ToString() == "no")
                        plantilla.IdPlantillaDocumento = null;
                    else
                        plantilla.IdPlantillaDocumento = Convert.ToInt32(Page.Session["IdValue"]);


                    plantilla.NombrePlantilla = TxtNombrePlantillaGenerica.Text;
                    
                    //if (HtmlEditorGenerica.Html.Contains("../UploadFiles/Images/"))
                    //            HtmlEditorGenerica.Html = HtmlEditorGenerica.Html.Replace("../UploadFiles/Images/", "/UploadFiles/Images/");
                    
                    //plantilla.ContenidoHtml = HtmlEditorGenerica.Html.Replace("/UploadFiles/Images/", "C:/inetpub/wwwroot/wss/VirtualDirectories/" + ConfigurationManager.AppSettings["PuertoSitio"].ToString() + "/UploadFiles/Images/");

                    plantilla.ContenidoHtml = System.Web.HttpUtility.HtmlDecode(HtmlEditorGenerica.Html);
                    plantilla.EsGenerica = ChkGenerica.Checked;
                    plantilla.UsuarioModificacion = resumenP.idUsuario;
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoGenerica.Value);
                    plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoGenerica.Value);
                    plantilla.IdAcreedor = Convert.ToInt32(CmbxAcreedorGenerica.Value);

                    if (plantilla.IdPlantillaDocumento == null)
                    {
                        Id = LN.InsertarPlantilla(plantilla);
                        if (Id == null || Id == 0)
                             return "error";
                    }
                    else   
                    {
                       Id = LN.ActualizarPlantilla(plantilla);
                       if (Id == null || Id == 0)
                             return "error";
                    }   
                }
                return "ok";
            }
            catch(Exception ex)
            {
                //return "error";
                return ex.Message.ToString() + "~" + plantilla.IdPlantillaDocumento.ToString() + "~" + Convert.ToString(CmbxTipoProductoGenerica.Value) + "~" + Convert.ToString(CmbxTipoDocumentoGenerica.Value) + "~" + Convert.ToString(CmbxAcreedorGenerica.Value) + "~" + TxtNombrePlantillaGenerica.Text.Trim() + "~" + plantilla.ContenidoHtml + "~" + plantilla.EsGenerica.ToString() + "~" + plantilla.UsuarioModificacion;
                //html = s.Trim() + "~" + html;
            }
        }

        private static string PuertoSitio = ConfigurationManager.ConnectionStrings["PuertoSitio"].ConnectionString;
        

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        //protected void BtnLimpiar_Click(object sender, EventArgs e)
        //{
        //    //using (MemoryStream stream = new MemoryStream())
        //    //{
        //    //    ExporToStream(HtmlEditor.Html, stream);

        //    //    HttpContext.Current.Response.Clear();

        //    //    HttpContext.Current.Response.Buffer = false;
        //    //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    //    HttpContext.Current.Response.AppendHeader("Content-Transfer-Encoding", "binary");
        //    //    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=document.pdf");
        //    //    HttpContext.Current.Response.BinaryWrite(stream.ToArray());
        //    //    HttpContext.Current.Response.End();
        //    //}
        //}

        //void ExporToStream(string html, Stream stream)
        //{
        //    using (RichEditDocumentServer server = new RichEditDocumentServer())
        //    {
        //        server.HtmlText = html;

        //        string text = "<b>SOME SAMPLE TEXT</b>";
        //        AddHeaderToDocument(server.Document, text);
        //        AddFooterToDocument(server.Document, text);

        //        using (PrintingSystemBase ps = new PrintingSystemBase())
        //        {
        //            using (PrintableComponentLinkBase pcl = new PrintableComponentLinkBase(ps))
        //            {
        //                pcl.Component = server;
        //                pcl.CreateDocument();
        //                ps.ExportToPdf(stream);
        //            }
        //        }
        //    }
        //}

        //void AddHeaderToDocument(DevExpress.XtraRichEdit.API.Native.Document document, string htmlText)
        //{
        //    SubDocument doc = document.Sections[0].BeginUpdateHeader();
        //    doc.AppendHtmlText(htmlText);
        //    document.Sections[0].EndUpdateHeader(doc);
        //}

        //void AddFooterToDocument(DevExpress.XtraRichEdit.API.Native.Document document, string htmlText)
        //{
        //    SubDocument doc = document.Sections[0].BeginUpdateFooter();
        //    doc.AppendHtmlText(htmlText);
        //    document.Sections[0].EndUpdateFooter(doc);
        //}

        protected void CmbxTipoDocumentoGenerica_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                CargarTipoDocumento(e.Parameter);
        }


        protected void ASPxCallback1_Callback(object source, CallbackEventArgs e)
        {
            string html = string.Empty;
            //int i = 0;
            string[] Valores = new string[2];
            string retorno = string.Empty;
            switch (e.Parameter)
            {
                case "buscarGenerica":
                    //retorno = BuscarPlantillaGenerica();
                    //if (retorno != null)
                    //{
                    //    //foreach (string s in Valores)
                    //    //{
                    //    //    html = s.Trim() + "~" + html;
                    //    //}
                    //    e.Result = retorno;
                    //}
                    //else
                    //    e.Result = "";
                    //break;
                case "buscarPersonalizada":
                    //retorno = BuscarPlantillaPersonalizada();
                    //if (retorno != null)
                    //{
                    //    e.Result = retorno;
                    //}
                    //else
                    //    e.Result = "";
                    break;
                case "buscarPersonalizadaBuscar":
                    LNplantilla plantilla = new LNplantilla();
                    plantilla.IdAcreedor = Convert.ToInt32(CmbxAcreedorBuscar.Value);
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoBuscar.Value);
                    plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoBuscar.Value);
                    CargarDocumentos(plantilla);
                    break;
                case "guardar":
                    e.Result = GuardarPlantilla();
                    break;
            }
        }

        protected void CmbxTipoDocumentoBuscar_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                CargarTipoDocumento(e.Parameter);
        }


        protected void GvBuscarPlantilla_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GvBuscarPlantilla.DataSource = Page.Session["GvBuscarPlantilla"];
                GvBuscarPlantilla.DataBind();
            }
            catch
            {
            }
        }

        protected void GvBuscarPlantilla_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string a = e.ButtonID;
            int index = e.VisibleIndex;
            int IdPlantilla = (int)GvBuscarPlantilla.GetRowValues(index, "IdPlantillaDocumento");
            switch (e.ButtonID)
            {
                case "Editar":
                    Page.Session["IdValue"] = IdPlantilla.ToString();
                    string datosPlantilla = String.Empty;
                    datosPlantilla = CargarPlantilla(IdPlantilla);
                    if (!string.IsNullOrEmpty(datosPlantilla))
                        GvBuscarPlantilla.JSProperties["cpValores"] = "Editar" + "~" + datosPlantilla;
                    else
                        GvBuscarPlantilla.JSProperties["cpValores"] = "Error";

                    break;
                case "Descargar":
                    DescargarPlantilla(IdPlantilla);
                    break;
                case "Eliminar":
                    Boolean eliminar = EliminarPlantilla(IdPlantilla);
                    if (eliminar)
                        GvBuscarPlantilla.JSProperties["cpValores"] = "eliminar";
                    else
                        GvBuscarPlantilla.JSProperties["cpValores"] = "Error";
                    break;
            }
        }

        public Boolean EliminarPlantilla(int IdPlantilla)
        {
            LogicaNegocio LN = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;
            string Usuario = util.ObtenerValor(app2.CurrentUser.Name);
            return LN.EliminarPlantilla(IdPlantilla, Usuario);
        }

        public string CargarPlantilla(int IdPlantilla)
        {
            string datos = string.Empty;
            string html = string.Empty;
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = new DataTable("dt");
            dt = LN.ListarPlantillaById(IdPlantilla);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ContenidoHtml"].ToString().Contains("../UploadFiles/Images/"))
                    html = dt.Rows[0]["ContenidoHtml"].ToString().Replace("../UploadFiles/Images/", "/UploadFiles/Images/");
                else
                    html = dt.Rows[0]["ContenidoHtml"].ToString();

                html = html.Replace("/UploadFiles/Images/", "C:/inetpub/wwwroot/wss/VirtualDirectories/" + ConfigurationManager.AppSettings["PuertoSitio"].ToString() + "/UploadFiles/Images/");
                //CmbxAcreedorGenerica.Value = dt.Rows[0]["IdAcreedor"];
                //CmbxTipoDocumentoGenerica.Value = dt.Rows[0]["IdDocumentoTipo"];

                datos = html + "~" + dt.Rows[0]["IdAcreedor"] + "~" + dt.Rows[0]["IdDocumentoTipo"] + "~" + dt.Rows[0]["NombrePlantilla"] + "~" + Convert.ToBoolean(dt.Rows[0]["EsGenerica"]) + "~" + dt.Rows[0]["IdTipoProducto"] + "~" + dt.Rows[0]["Nombre"];
                return datos;
            }
            else
                return null;
        }

        void DescargarPlantilla(int IdPlantilla)
        {
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = new DataTable("dt");
            dt = LN.ListarPlantillaById(IdPlantilla);

            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["ContenidoHtml"].ToString()))
                {
                    string contenido = HttpUtility.HtmlDecode(dt.Rows[0]["ContenidoHtml"].ToString());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<table width='1000px'><tr><td align='center'><table width='900px'><tr><td>" + contenido + "</td></tr></table></td></tr></table>");
                    byte[] archivo = util.ConvertirAPDF_Control(sb);
                    Page.Session["Titulo"] = dt.Rows[0]["NombrePlantilla"].ToString();

                    if (archivo != null)
                    {
                        Page.Session["binaryData"] = archivo;
                        Page.Session["Titulo"] = dt.Rows[0]["NombrePlantilla"].ToString().Trim();
                        ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
                    }
                }
            }
        }

        protected void GvBuscarPlantilla_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string nombre = e.Parameters;
                if (nombre == "cargar")
                {
                    LNplantilla plantilla = new LNplantilla();
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoBuscar.Value);
                    plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoBuscar.Value);
                    plantilla.IdAcreedor = Convert.ToInt32(CmbxAcreedorBuscar.Value);   
                    plantilla.NombrePlantilla = TxtNombrePlantillaBuscar.Text.Trim();
                    CargarDocumentos(plantilla);
                }
            }
            catch
            {
            }
        }

        void CargarDocumentos(LNplantilla plantilla)
        {
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = new DataTable("dt");
            dt = LN.ListarTodasPlantillas(plantilla);
            Page.Session["GvBuscarPlantilla"] = dt;
            GvBuscarPlantilla.DataSource = dt;
            GvBuscarPlantilla.DataBind();
        }

    }
}
