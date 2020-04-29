using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using MultigestionUtilidades;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using Bd;
using ClasesNegocio;

namespace MultiAdministracion.wpSeguimientoCompromisos.wpSeguimientoCompromisos
{
    [ToolboxItemAttribute(false)]
    public partial class wpSeguimientoCompromisos : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpSeguimientoCompromisos()
        {
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "SeguimientoCompromisos.aspx";
        bool aplicaFiltros = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            //LogicaNegocio Ln = new LogicaNegocio();

            //PERMISOS USUARIOS
            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    CargarEjecutivo(dt.Rows[0]["descCargo"].ToString(), app2.CurrentUser.Name);
                    CargarCanal();
                    CargarFondos();
                    DropDownList ddlMes = this.ddlMes;
                    CargarMes(ddlMes);
                    CargarEtapas();

                    if (Page.Session["BUSQUEDA"] != null)
                    {
                        Busqueda objBusqueda = new Busqueda();
                        objBusqueda = (Busqueda)Page.Session["BUSQUEDA"];

                        if (objBusqueda.idEtapa != -1)
                        {
                            string ETAPA = objBusqueda.idEtapa.ToString();
                            ddlEtapa.SelectedIndex = ddlEtapa.Items.IndexOf(ddlEtapa.Items.FindByValue(Convert.ToString(objBusqueda.idEtapa)));
                        }

                        txtRazonSocial.Text = objBusqueda.RazonSocial;
                        Page.Session["BUSQUEDA"] = null;
                    }
                }

                CargarResumen();
                CargarGrilla();
                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
           
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

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //e.Row.Cells[9].Text = "Fecha Cierre";
            //e.Row.Cells[10].Text = "% Comision";
            //e.Row.Cells[11].Text = "Plazo";
            //e.Row.Cells[9].Visible = false; //IDEmpresa
            //e.Row.Cells[10].Visible = false; //RazonSocial
            //e.Row.Cells[11].Visible = false; //IDOperacion
            //e.Row.Cells[12].Visible = false; //Desc_Producto(certificado fianza tecnica, certificado fianza comercial)

            e.Row.Cells[13].Visible = false; //subetapa
            e.Row.Cells[14].Visible = false; //IdEmpresa
            e.Row.Cells[15].Visible = false; //RazonSocial
            e.Row.Cells[16].Visible = false; //IdOperacion
            e.Row.Cells[17].Visible = false; //Desc_Producto(certificado fianza tecnica, certificado fianza comercial)
            e.Row.Cells[18].Visible = false; //DescArea           
            e.Row.Cells[19].Visible = false; //Prioridad   
        }

        int i = 0;
        protected void ResultadosBusqueda_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

            }

            if (e.Row.RowIndex > -1)
            {
                LogicaNegocio LN = new LogicaNegocio();

                Label lbComentario = new Label();
                lbComentario.Text = e.Row.Cells[11].Text.ToString();
                e.Row.Cells[11].Controls.Add(lbComentario);
                e.Row.Cells[11].Controls.Add(new LiteralControl("<br/>"));
                TextBox txCom = new TextBox();
                txCom.TextMode = TextBoxMode.MultiLine;
                txCom.MaxLength = 1000;
                txCom.Columns = 20;
                txCom.Rows = 3;
                txCom.ID = "txtComentario";
                txCom.CssClass = "form-control";
                e.Row.Cells[11].Controls.Add(txCom);

                DateTimeControl cld = new DateTimeControl();
                cld.ID = "fechaCierre";
                cld.DateOnly = true;
                cld.AutoPostBack = false;
                cld.ClientIDMode = System.Web.UI.ClientIDMode.AutoID;
                cld.LocaleId = 13322;
                cld.Calendar = SPCalendarType.Gregorian;
                cld.ToolTip = "Fecha de cierre";

                ((TextBox)(cld.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");
                //((TextBox)(cld.Controls[0])).Attributes.Add("onblur", "OnIframeLoadFinish();");
                ((TextBox)(cld.Controls[0])).MaxLength = 10;
                ((TextBox)(cld.Controls[0])).CssClass = "form-control";
                ((TextBox)(cld.Controls[0])).Width = 100;
                ((TextBox)(cld.Controls[0])).AutoPostBack = false;
                e.Row.Cells[10].Controls.Add(cld);
                e.Row.Cells[10].Controls.Add(new LiteralControl("<br/>"));

                DropDownList ddlPrioridad = new DropDownList();
                ddlPrioridad.AutoPostBack = false;
                ddlPrioridad.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                ddlPrioridad.ToolTip = "Prioridad Emisión";
                ddlPrioridad.CssClass = "form-control";

                ListItem prioridad = new ListItem("Seleccione", "0");
                ddlPrioridad.Items.Add(prioridad);

                ListItem prioridad1 = new ListItem("Rojo", "1");
                prioridad1.Attributes.Add("style", "color: red;");
                ddlPrioridad.Items.Add(prioridad1);

                ListItem prioridad2 = new ListItem("Amarillo", "2");
                prioridad2.Attributes.Add("style", "color: #AEB404;");
                ddlPrioridad.Items.Add(prioridad2);

                ListItem prioridad3 = new ListItem("Verde", "3");
                prioridad3.Attributes.Add("style", "color: #298A08;");
                ddlPrioridad.Items.Add(prioridad3);

                ddlPrioridad.ID = "Prioridad";
                ddlPrioridad.Attributes["onChange"] = "ValidarColor('" + ddlPrioridad.ID + "');";
                e.Row.Cells[10].Controls.Add(ddlPrioridad);


                if (!string.IsNullOrEmpty(e.Row.Cells[19].Text.ToString()) && !string.IsNullOrWhiteSpace(e.Row.Cells[19].Text.ToString()) && e.Row.Cells[10].Text != "&nbsp;")
                {
                    string prioridadSeleccionada = e.Row.Cells[19].Text.ToString();
                    if (!string.IsNullOrEmpty(prioridadSeleccionada))
                    {
                        ddlPrioridad.SelectedIndex = ddlPrioridad.Items.IndexOf(ddlPrioridad.Items.FindByValue(prioridadSeleccionada));
                    }
                }


                LinkButton lb = new LinkButton();
                lb.CssClass = ("glyphicon glyphicon-plus-sign paddingIconos");
                lb.CommandName = "Agregar";
                lb.CommandArgument = i.ToString();
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusqueda_Command;
                lb.ToolTip = "Agregar";
                e.Row.Cells[12].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("glyphicon glyphicon-search paddingIconos");
                lb2.CommandName = "Ver";
                lb2.CommandArgument = i.ToString();
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;
                lb2.ToolTip = "Ver";
                e.Row.Cells[12].Controls.Add(lb2);

                if (!string.IsNullOrEmpty(e.Row.Cells[10].Text.ToString()) && !string.IsNullOrWhiteSpace(e.Row.Cells[10].Text.ToString()) && e.Row.Cells[10].Text != "&nbsp;")
                {
                    string fechaSalida = e.Row.Cells[10].Text.ToString();
                    DateTime? fechaOriginal = util.ValidarFecha(fechaSalida.Trim());

                    if (fechaOriginal != null)
                        cld.SelectedDate = fechaOriginal.Value;
                }

                i++;
            }

            for (int rowIndex = ResultadosBusqueda.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = ResultadosBusqueda.Rows[rowIndex];
                GridViewRow gvPreviousRow = ResultadosBusqueda.Rows[rowIndex + 1];
                for (int cellCount = 0; cellCount < 1; cellCount++)
                {
                    bool ban = false;
                    if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                    {
                        if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount].RowSpan = 2;

                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;

                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;
                        ban = true;
                    }

                    if (gvRow.Cells[cellCount + 1].Text == gvPreviousRow.Cells[cellCount + 1].Text && ban)
                    {
                        if (gvPreviousRow.Cells[cellCount + 1].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount + 1].RowSpan = 2;

                        }
                        else
                        {
                            gvRow.Cells[cellCount + 1].RowSpan = gvPreviousRow.Cells[cellCount + 1].RowSpan + 1;

                        }
                        gvPreviousRow.Cells[cellCount + 1].Visible = false;

                    }
                }
            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            ocultarDiv();
            int index1 = Convert.ToInt32(e.CommandArgument);
            int Prioridad = 0;
            LogicaNegocio MTO = new LogicaNegocio();
            bool resGuardar = false;
            bool ActualizarFecha = false;

            //ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");

            SPWeb app = SPContext.Current.Web;
            string usuario = app.CurrentUser.Name;
            int idOperacion = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[16].Text.ToString()));
            int idEmpresa = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[14].Text.ToString()));
            string ejecutivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[2].Text.ToString());
            string area = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[18].Text.ToString()).Trim();

            string etapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[1].Text.ToString()).Trim();
            string subEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[13].Text.ToString()).Trim();

            string razonSocial = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[3].Text.ToString());
            string operacion = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index1].Cells[17].Text.ToString());

            var FechaPago = ResultadosBusqueda.Rows[index1].Controls[0].FindControl("fechaCierre") as DateTimeControl;

            var PrioridadOperacion = ResultadosBusqueda.Rows[index1].Controls[0].FindControl("Prioridad") as DropDownList;
            //string mes = (ddlMes.SelectedItem != null) ? (ddlMes.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlMes.SelectedItem.Text : "";

            if (e.CommandName == "Agregar")
            {
                if (PrioridadOperacion != null)
                {
                    Prioridad = int.Parse(PrioridadOperacion.SelectedValue);
                }
                string com = (ResultadosBusqueda.Rows[index1].FindControl("txtComentario") as TextBox).Text.ToString();
                if (com.Trim() != "" || FechaPago != null)
                {
                    DateTime? fechaCierre;

                    if (FechaPago.IsDateEmpty)
                        fechaCierre = null;
                    else
                        fechaCierre = FechaPago.SelectedDate;

                    resGuardar = MTO.GuardarCompromiso(idOperacion, idEmpresa, ejecutivo, area, com, usuario, etapa, subEtapa, fechaCierre, Prioridad);
                }


                if (resGuardar || ActualizarFecha)
                {
                    dvSuccess.Style.Add("display", "block");
                    this.lbSuccess.Text = "Se ha modificado correctamente el registro.";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "Ha ocurrido un error.";
                }

                CargarGrilla();
                (ResultadosBusqueda.Rows[index1].FindControl("txtComentario") as TextBox).Text = "";
            }

            if (e.CommandName == "Ver")
            {
                //llenar session
                objresumen.idEmpresa = idEmpresa;
                objresumen.idOperacion = idOperacion;
                objresumen.descEjecutivo = ejecutivo;
                objresumen.desEmpresa = razonSocial;
                objresumen.desEtapa = etapa;
                objresumen.desSubEtapa = subEtapa;
                objresumen.desOperacion = operacion;
                objresumen.area = area;
                objresumen.linkPrincial = "SeguimientoCompromisos.aspx";

                Page.Session["RESUMEN"] = objresumen;
                //redireccionar
                Page.Response.Redirect("Compromisos.aspx");
            }
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> datos = new Dictionary<string, string>();
            string Reporte = "";
            Reporte = "SeguimientoSemanal";

            byte[] archivo = GenerarReporte(Reporte, datos, "");

            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
        }

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id)
        {
            LogicaNegocio MTO = new LogicaNegocio();

            try
            {
                String xml = String.Empty;
                string html = string.Empty;
                System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();

                DataSet res1 = new DataSet();

                string mes = (ddlMes.SelectedItem != null) ? (ddlMes.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlMes.SelectedItem.Text : " ";
                string etapa = (ddlEtapa.SelectedItem != null) ? (ddlEtapa.SelectedItem.Text.ToLower().Contains("seleccione")) ? "    " : ddlEtapa.SelectedItem.Text : "                          ";
                string ejecutivo = (ddlEjecutivo.SelectedItem != null) ? (ddlEjecutivo.SelectedItem.Text.ToLower().Contains("seleccione")) ? "                          " : ddlEjecutivo.SelectedItem.Text : "                          ";
                string empresa = (txtRazonSocial.Text != "") ? txtRazonSocial.Text : "                          ";
                string canal = (ddlCanal.SelectedItem != null) ? (ddlCanal.SelectedItem.Text.ToLower().Contains("seleccione")) ? "                          " : ddlCanal.SelectedItem.Text : "                          ";
                string fondo = (ddlFondo.SelectedItem != null) ? (ddlFondo.SelectedItem.Text.ToLower().Contains("seleccione")) ? "                          " : ddlFondo.SelectedItem.Text : "                          ";

                if (!aplicaFiltros)
                {
                    mes = "";
                    etapa = "";
                    ejecutivo = "";
                    empresa = "";
                    canal = "";
                    fondo = "";
                }

                res1 = MTO.ResporteSeguimientoSemanal(mes, etapa, ejecutivo, empresa, canal, fondo);

                for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }
                //xml.Element("Snippets").Add(root);


                int index2 = xml.IndexOf("\">");
                xml = xml.Insert(index2 + "\">".Length, "<Mes>" + mes + "</Mes>" +
                                                                                "<Etapa>" + etapa + "</Etapa>" +
                                                                                "<Ejecutivo>" + ejecutivo + "</Ejecutivo>" +
                                                                                "<Empresa>" + empresa + "</Empresa>" +
                                                                                "<Canal>" + canal + "</Canal>" +
                                                                                "<Fondo>" + fondo + "</Fondo>");

                XDocument newTree = new XDocument();

                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    //desarrollo 46185
                    //xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/46185/xsl/request/" + sp + ".xslt");
                    //certificación
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");
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
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        string generarXMLServiciosOperacion()
        {
            //asignacionResumen(ref objresumen);
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;

            for (int i = 0; i <= ResultadosBusqueda.Rows.Count - 1; i++)
            {
                if (System.Web.HttpUtility.HtmlDecode((ResultadosBusqueda.Rows[i].FindControl("txtComentario") as TextBox).Text) != "" || System.Web.HttpUtility.HtmlDecode((ResultadosBusqueda.Rows[i].FindControl("fechaCierre") as DateTimeControl).SelectedDate.ToString()) != null)
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode nodo = doc.CreateElement("idOperacion");
                    nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text)));
                    RespNode.AppendChild(nodo);

                    XmlNode nodo1 = doc.CreateElement("idEmpresa");
                    nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[14].Text)));
                    RespNode.AppendChild(nodo1);

                    XmlNode nodo2 = doc.CreateElement("ejecutivo");
                    nodo2.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[2].Text)));
                    RespNode.AppendChild(nodo2);

                    XmlNode nodo3 = doc.CreateElement("area");
                    nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text.Trim())));
                    RespNode.AppendChild(nodo3);

                    XmlNode nodo4 = doc.CreateElement("etapa");
                    nodo4.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[1].Text.Trim())));
                    RespNode.AppendChild(nodo4);

                    XmlNode nodo5 = doc.CreateElement("subetapa");
                    nodo5.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text.Trim())));
                    RespNode.AppendChild(nodo5);

                    XmlNode nodoC = doc.CreateElement("comentario");
                    nodoC.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusqueda.Rows[i].FindControl("txtComentario") as TextBox).Text)));
                    RespNode.AppendChild(nodoC);

                    XmlNode nodo6 = doc.CreateElement("fechaCierre");
                    nodo6.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusqueda.Rows[i].FindControl("fechaCierre") as DateTimeControl).SelectedDate.ToString("yyyyMMdd"))));
                    RespNode.AppendChild(nodo6);

                    XmlNode nodo7 = doc.CreateElement("prioridad");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusqueda.Rows[i].FindControl("Prioridad") as DropDownList).SelectedValue)));
                    RespNode.AppendChild(nodo7);

                    ValoresNode.AppendChild(RespNode);
                }
            }
            return doc.OuterXml;
        }

        protected void btnGuardarTodo_Click(object sender, EventArgs e)
        {
            SPWeb app2 = SPContext.Current.Web;
            string usuario = app2.CurrentUser.Name;
            string xml = generarXMLServiciosOperacion();
            LogicaNegocio MTO = new LogicaNegocio();
            bool respuesta;
            respuesta = MTO.GuardarTodoComentarios(usuario, xml);
            if (respuesta)
            {
                CargarGrilla();
                dvSuccess.Style.Add("display", "block");
                lbSuccess.Text = "Se han guardado correctamente los datos";
            }
            else
            {
                dvError.Style.Add("display", "block");
                lbError.Text = "Ha ocurrido un error.";
            }
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            Busqueda objBusqueda = new Busqueda();

            objBusqueda.idEtapa = int.Parse(ddlEtapa.SelectedValue);
            objBusqueda.RazonSocial = txtRazonSocial.Text.ToString();

            Page.Session["BUSQUEDA"] = objBusqueda;

            CargarGrilla();
        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {
            Page.Response.Write("<script>window.open('http://dw01wsr001/Reports/Pages/Report.aspx?ItemPath=%2fComercial%2fCompromisoSemanal&ViewMode=Detail','_blank');</script>");
        }

        protected void btnGuardarResumen_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio LN = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;
            string usuario = app2.CurrentUser.Name;
            bool OK = false;
            DataTable dt = (DataTable)ViewState["gvTotalizado"];
            if (dt.Rows.Count > 0)
            {
                var xml = util.generarXML(dt);
                OK = LN.GuardarPipeline(usuario, xml);
                CargarResumen();
            }
            else
                OK = false;

            if (OK)
            {
                dvSuccess.Style.Add("display", "block");
                lbSuccess.Text = "Se ha guardado correctamente la información.";
            }
            else
            {

            }
        }

        #endregion


        #region Metodos

        private void CargarResumen()
        {
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = new DataTable("dt");

            dt = LN.ResumenPipeline(DateTime.Now);

            gvTotalizado.DataSource = dt;
            gvTotalizado.DataBind();
            ViewState["gvTotalizado"] = dt;
        }

        private void CargarGrilla()
        {
            LogicaNegocio MTO = new LogicaNegocio();
            DataTable res;

            try
            {
                string mes = (ddlMes.SelectedItem != null) ? (ddlMes.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlMes.SelectedItem.Text : "";
                string etapa = (ddlEtapa.SelectedItem != null) ? (ddlEtapa.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlEtapa.SelectedItem.Text : "";
                string ejecutivo = (ddlEjecutivo.SelectedItem != null) ? (ddlEjecutivo.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlEjecutivo.SelectedItem.Text : "";
                string empresa = txtRazonSocial.Text;
                string canal = (ddlCanal.SelectedItem != null) ? (ddlCanal.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlCanal.SelectedItem.Text : "";
                string fondo = (ddlFondo.SelectedItem != null) ? (ddlFondo.SelectedItem.Text.ToLower().Contains("seleccione")) ? "" : ddlFondo.SelectedItem.Text : "";

                if (string.IsNullOrEmpty(mes) && string.IsNullOrEmpty(etapa) && string.IsNullOrEmpty(ejecutivo) && string.IsNullOrEmpty(empresa) && string.IsNullOrEmpty(canal) && string.IsNullOrEmpty(fondo))
                    aplicaFiltros = false;
                else
                    aplicaFiltros = true;


                res = MTO.ListarSeguimientoCompromisos(mes, etapa, ejecutivo, empresa, canal, fondo);
                ResultadosBusqueda.DataSource = res;
                //Page.Session["tabla"] = res;
                ResultadosBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void CargarEjecutivo(string cargo, string nombre)
        {
            ddlEjecutivo.Items.Clear();
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 1, "");
                util.CargaDDL(ddlEjecutivo, dt, "nombreApellido", "idUsuario");
                
                if (cargo == "Sub-Gerente Comercial")
                {
                    ddlEjecutivo.SelectedIndex = ddlEjecutivo.Items.IndexOf(ddlEjecutivo.Items.FindByText(nombre.Trim()));
                    ddlEjecutivo.Enabled = false;
                    //if (ddlEjecutivo != null)
                    //{
                    //    ddlEjecutivo.Text = nombre;
                    //    ddlEjecutivo. = true;
                    //}
                }
                //else
                //{
                //    ListEditItem TipoEjecutivo = ddlEjecutivo.Items.FindByText(nombre);
                //    if (TipoEjecutivo != null)
                //    {
                //        ddlEjecutivo.Text = nombre;
                //        ddlEjecutivo.ReadOnly = true;
                //    }
                //}         
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarCanal()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPListItemCollection items = app.Lists["Canales"].Items;
            util.CargaDDL(ddlCanal, items.GetDataTable(), "Nombre", "ID");
        }

        private void CargarEtapas()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPListItemCollection items = app.Lists["Etapas"].Items;
            util.CargaDDL(ddlEtapa, items.GetDataTable(), "Nombre", "ID");
        }

        private void CargarFondos()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["Fondos"];

            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='Visible'/><Value Type='Integer'>1</Value></Eq></Where>";
            SPListItemCollection collListItems = items.GetItems(query);
            util.CargaDDL(ddlFondo, collListItems.GetDataTable(), "Nombre", "ID");
        }

        private void CargarMes(DropDownList DDL)
        {
            ListItemCollection LstItem;
            LstItem = new ListItemCollection();
            //DDL.DataSource = dt;
            DDL.DataTextField = "Nombre";
            DDL.DataValueField = "ID";

            DDL.Items.Insert(0, new ListItem("Seleccione", "0"));

            DDL.Items.Insert(1, new ListItem("Enero", "1"));
            DDL.Items.Insert(2, new ListItem("Febrero", "2"));
            DDL.Items.Insert(3, new ListItem("Marzo", "3"));
            DDL.Items.Insert(4, new ListItem("Abril", "4"));
            DDL.Items.Insert(5, new ListItem("Mayo", "5"));
            DDL.Items.Insert(6, new ListItem("Junio", "6"));
            DDL.Items.Insert(7, new ListItem("Julio", "7"));
            DDL.Items.Insert(8, new ListItem("Agosto", "8"));
            DDL.Items.Insert(9, new ListItem("Septiembre", "9"));
            DDL.Items.Insert(10, new ListItem("Octubre", "10"));
            DDL.Items.Insert(11, new ListItem("Noviembre", "11"));
            DDL.Items.Insert(12, new ListItem("Diciembre", "12"));
            DDL.DataBind();
            DDL.SelectedIndex = 0;
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        #endregion

    }
}
