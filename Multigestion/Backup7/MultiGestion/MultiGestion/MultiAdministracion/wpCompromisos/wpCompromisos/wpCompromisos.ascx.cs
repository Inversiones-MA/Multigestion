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

namespace MultiAdministracion.wpCompromisos.wpCompromisos
{
    [ToolboxItemAttribute(false)]
    public partial class wpCompromisos : WebPart
    {
        private static string pagina = "Compromisos.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpCompromisos()
        {
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
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
                        try
                        {
                            objresumen = (Resumen)Page.Session["RESUMEN"];
                            //ViewState["RES"] = objresumen;
                            Page.Session["RESUMEN"] = null;

                            ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                            Page.Session["BUSQUEDA"] = null;
                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            lbEjecutivo.Text = objresumen.descEjecutivo;
                            lbOperacion.Text = objresumen.desOperacion.ToString();
                        }
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }
                    }
                    else
                        Page.Response.Redirect("MensajeSession.aspx");
                }
                llenarGridCompromisos();
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

        private void CargaDDL(DropDownList DDL, DataTable dt, string text, string value)
        {
            ListItem LstItem;
            DDL.DataSource = dt;
            DDL.DataTextField = text;
            DDL.DataValueField = value;
            DDL.DataBind();
            LstItem = new ListItem("Seleccione", "0");

            DDL.Items.Insert(0, LstItem);
            DDL.SelectedIndex = 0;
        }

        private void llenarGridCompromisos()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;

            try
            {
                res = MTO.ListarCompromisos(objresumen.idOperacion.ToString());

                //Cargar datos de form superior
                string razonSocial = res.Tables[1].Rows[0]["RazonSocial"].ToString();

                string operacion = res.Tables[1].Rows[0]["Operacion"].ToString();
                string ejecutivo = res.Tables[1].Rows[0]["Ejecutivo"].ToString();
                string etapa = res.Tables[1].Rows[0]["Etapa"].ToString();
                string canal = res.Tables[1].Rows[0]["Canal"].ToString();
                string fondo = res.Tables[1].Rows[0]["Fondo"].ToString();
                string montoOper = res.Tables[1].Rows[0]["MontoOperacion"].ToString();
                string montoCom = res.Tables[1].Rows[0]["MontoComision"].ToString();

                this.txtRazonSocial.Text = razonSocial;
                this.txtOperacion.Text = operacion;
                this.txtEjecutivo.Text = ejecutivo;
                this.txtEtapa.Text = etapa;
                this.txtFondo.Text = fondo;
                this.txtMontoOperacion.Text = montoOper;
                this.txtMontoComision.Text = montoCom;
                this.txtCanal.Text = canal;

                if (res.Tables[0].Rows.Count > 0)
                {
                    ResultadosBusqueda.DataSource = res.Tables[0];
                    ResultadosBusqueda.DataBind();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowIndex > -1)
            //{
            //    TextBox tx = new TextBox();

            //    e.Row.Cells[6].Text = "";
            //    e.Row.Cells[6].Controls.Add(tx);
            //}
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenarGridCompromisos();
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void bt_Guardar_Click(object sender, EventArgs e)
        {
            string com = this.txtComentario.Text;

            if (com.Trim() != "")
            {
                SPWeb app = SPContext.Current.Web;
                string usuario = app.CurrentUser.Name;
                int idOperacion = objresumen.idOperacion;
                //Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[13].Text.ToString()));
                int idEmpresa = objresumen.idEmpresa;
                //Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[12].Text.ToString()));
                string ejecutivo = txtEjecutivo.Text;
                string area = objresumen.area;
                string etapa = objresumen.desEtapa;
                string subetapa = objresumen.desSubEtapa;
                //System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[14].Text.ToString());

                LogicaNegocio MTO = new LogicaNegocio();
                bool resGuardar = MTO.GuardarCompromiso(idOperacion, idEmpresa, ejecutivo, area, com, usuario, etapa, subetapa, null, 0);

                if (resGuardar)
                {
                    //mostar mensaje
                    this.lbSuccess.Text = "Se ha guardado correctamente un comentario.";
                    dvSuccess.Style.Add("display", "block");

                    this.txtComentario.Text = "";
                    llenarGridCompromisos();
                }
                else
                {
                    //mostar mensaje
                    this.lbError.Text = "Ha ocurrido un error al intentar guardar el comentario.";
                    dvError.Style.Add("display", "block");
                }
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> datos = new Dictionary<string, string>();
            string Reporte = "";
            Reporte = "Compromisos";

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

                res1 = MTO.ReporteCompromisos(objresumen.idOperacion.ToString());

                for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }

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

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            //            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
            llenarGridCompromisos();
        }

        protected void lbCompromisos_Click(object sender, EventArgs e)
        {
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("Compromisos.aspx");
        }

        protected void lbBitacoraComent_Click(object sender, EventArgs e)
        {
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("BitacoraComentarios.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

    }
}
