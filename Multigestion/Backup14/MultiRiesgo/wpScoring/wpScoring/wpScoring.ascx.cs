using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Data;
using System.Xml;
using ExpertPdf.HtmlToPdf;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Collections.Generic;
using System.Web.UI;
using System.Diagnostics;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using DevExpress.Web;
using ClasesNegocio;
using Bd;

namespace MultiRiesgo.wpScoring.wpScoring
{
    [ToolboxItemAttribute(false)]
    public partial class wpScoring : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpScoring()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "Scoring.aspx";


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                asignacionResumen(ref objresumen);
            }

            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = objresumen.area,
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    Page.Session["peritaje"] = null;
                    cargarCombo();
               
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;

                        lbEditar.Style.Add("display", "block");
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        //Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbOperacion.Text = objresumen.desOperacion;
                        lbEjecutivo.Text = objresumen.descEjecutivo;

                        cargarDatos();

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Enabled = false;
                            lbEditar.Style.Add("display", "none");
                            btnGuardar.Visible = false;
                        }
                    }
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
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

        protected void lbEdoResultado_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoEdoResultado.aspx");
        }

        protected void lbVentas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoVentas.aspx");
        }

        protected void lbCompras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoCompras.aspx");
        }

        protected void lbScoring_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Scoring.aspx");
        }

        protected void lbPaf_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ResumenPaf.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void lbBalance_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Scoring");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoBalance.aspx");
        }

        private string GuardarScoring()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            Boolean exito = false;

            if (lbPtjeLetra.Text.Contains("Error") || lbPuntaje.Text.Contains("Error") || lbSumatoria.Text.Contains("Error"))
                return "Error";

            exito = Ln.InsertarScoring(objresumen.idEmpresa.ToString(), generarXML(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString());
            exito = Ln.finalizarScoring(objresumen.idEmpresa.ToString(), generarXML(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString());
            if (exito)
                return Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
            else
                return Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
        }

        private string FinalizarScoring()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");    
            Boolean exito;

            exito = Ln.finalizarScoring(objresumen.idEmpresa.ToString(), generarXML(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString());
            if (exito)
                return Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
            else
                return Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
        }

        private void Imprimir()
        {
            asignacionResumen(ref objresumen);
            string Reporte = "ReporteScoring";

            byte[] archivo = GenerarReporte(objresumen.idEmpresa);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
            }
           //return "ok";
        }

        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (Page.Session["RESUMEN"] != null)
                objresumen = (Resumen)Page.Session["RESUMEN"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        protected void cargarCombo()
        {
            DataTable dt = new DataTable();
            LogicaNegocio Ln = new LogicaNegocio();
            DataSet ds = new DataSet();
            ds = Ln.ListarIndicadoresScoring("0", "", "", "01");

            if (ds != null)
            {
                util.CargaDDLDx(cbMorosidad, ds.Tables[0], "Titulo", "Id");
                util.CargaDDLDx(cbMonto, ds.Tables[1], "Titulo", "Id");
                util.CargaDDLDx(cbExperiencia, ds.Tables[2], "Titulo", "Id");
                util.CargaDDLDx(cbConcentracion, ds.Tables[3], "Titulo", "Id");
                util.CargaDDLDx(cbCompetencia, ds.Tables[4], "Titulo", "Id");
                util.CargaDDLDx(cbLiquidez, ds.Tables[5], "Titulo", "Id");
                util.CargaDDLDx(cbEbitda, ds.Tables[6], "Titulo", "Id");

                util.CargaDDLDx(cbPasivo, ds.Tables[7], "Titulo", "Id");
                util.CargaDDLDx(cbPasivoVts, ds.Tables[8], "Titulo", "Id");

                util.CargaDDLDx(cbCalidadBalance, ds.Tables[9], "Titulo", "Id");
                util.CargaDDLDx(cbLeverage, ds.Tables[10], "Titulo", "Id");

                util.CargaDDLDx(cbTipoGarantia, ds.Tables[11], "Titulo", "Id");
                util.CargaDDLDx(cbCalidadGarantia, ds.Tables[12], "Titulo", "Id");
                util.CargaDDLDx(cbCoberturaGarantia, ds.Tables[13], "Titulo", "Id");


                dt = Ln.ListarActividadEconomica();
                util.CargaDDLDx(cbActividad, dt, "Nombre", "Id");
            }
        }

        protected void cargarMorosidad()
        {
            DataTable dt = new DataTable();
            LogicaNegocio Ln = new LogicaNegocio();
            DataSet ds = new DataSet();
            ds = Ln.ListarIndicadoresScoring("0", "", "", "01");

            if (ds != null)
            {
                util.CargaDDLDxx1(cbMorosidad, ds.Tables[0], "Titulo", "Id");
            }
        }

        protected void cargarDatos()
        {
            try
            {
                Page.Session["peritaje"] = string.Empty;
                asignacionResumen(ref objresumen);
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable();
                dt = Ln.ListarScoring(objresumen.idEmpresa.ToString(), objresumen.idUsuario, objresumen.desPermiso, "04");
                if (dt.Rows.Count > 0)
                {
                    //123	cbActividad	11	Industriadeproductosquímicosyderivadosdelpetróleo,	0.21	3.5		1
                    //[IdScoring] ,[Descripcion] ,[idInformacion]  ,[DescInformacion] ,[ValorPtje] ,[ValorPond]  ,[ValorRank]  ,[Validacion]
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbActividad")
                        {
                            ListEditItem actividad = cbActividad.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (actividad != null)
                                cbActividad.SelectedIndex = actividad.Index;
                            //cbActividad.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondActividad.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeActividad.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbCalidadBalance")
                        {
                            ListEditItem calidadBalance = cbCalidadBalance.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (calidadBalance != null)
                                cbCalidadBalance.SelectedIndex = calidadBalance.Index;
                            //cbCalidadBalance.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondCalidadBalance.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeCalidadBalance.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbCalidadGarantia")
                        {
                            ListEditItem calidadGarantia = cbCalidadGarantia.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (calidadGarantia != null)
                                cbCalidadGarantia.SelectedIndex = calidadGarantia.Index;
                            //cbCalidadGarantia.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondCalidadGarantia.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeCalidadGarantia.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbCoberturaGarantia")
                        {
                            ListEditItem coberturaGarantia = cbCoberturaGarantia.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (coberturaGarantia != null)
                                cbCoberturaGarantia.SelectedIndex = coberturaGarantia.Index;
                            //cbCoberturaGarantia.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondCoberturaGarantia.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeCoberturaGarantia.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbCompetencia")
                        {
                            ListEditItem concentracion = cbCompetencia.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (concentracion != null)
                                cbCompetencia.SelectedIndex = concentracion.Index;
                            //cbCompetencia.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondCompetencia.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeCompetencia.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbConcentracion")
                        {
                            ListEditItem concentracion = cbConcentracion.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (concentracion != null)
                                cbConcentracion.SelectedIndex = concentracion.Index;
                            //cbConcentracion.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondConcentracion.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeConcentracion.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbEbitda")
                        {
                            ListEditItem ebitda = cbEbitda.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (ebitda != null)
                                cbEbitda.SelectedIndex = ebitda.Index;
                            //cbEbitda.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondEbitda.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeEbitda.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbExperiencia")
                        {
                            ListEditItem experiencia = cbExperiencia.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (experiencia != null)
                                cbExperiencia.SelectedIndex = experiencia.Index;
                            //cbExperiencia.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondExperiencia.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeExperiencia.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbFamiliar")
                        {
                            ListEditItem familiar = cbFamiliar.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (familiar != null)
                                cbFamiliar.SelectedIndex = familiar.Index;
                            //cbFamiliar.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondFamiliar.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeFamiliar.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbLeverage")
                        {
                            ListEditItem descripcion = cbLeverage.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (descripcion != null)
                                cbLeverage.SelectedIndex = descripcion.Index;
                            //cbLeverage.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondLeverage.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeLeverage.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbLiquidez")
                        {
                            ListEditItem liquidez = cbLiquidez.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (liquidez != null)
                                cbLiquidez.SelectedIndex = liquidez.Index;
                            //cbLiquidez.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondLiquidez.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeLiquidez.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }
                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbMonto")
                        {
                            ListEditItem monto = cbMonto.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (monto != null)
                                cbMonto.SelectedIndex = monto.Index;
                            //cbMonto.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondMonto.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeMonto.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbMorosidad")
                        {
                            //cargarMorosidad();
                            ListEditItem morosidad = cbMorosidad.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (morosidad != null)
                                cbMorosidad.SelectedIndex = morosidad.Index;
                                //cbMorosidad.Text = ds.Tables[0].Rows[0]["ComisionIncluida"].ToString();

                            //cbMorosidad.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondMorosidad.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeMorosidad.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbPasivo")
                        {
                            ListEditItem pasivo = cbPasivo.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (pasivo != null)
                                cbPasivo.SelectedIndex = pasivo.Index;
                            //cbPasivo.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondPasivo.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjePasivo.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbPasivoVts")
                        {
                            ListEditItem pasivoVts = cbPasivoVts.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (pasivoVts != null)
                                cbPasivoVts.SelectedIndex = pasivoVts.Index;
                            //cbPasivoVts.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondPasivoVts.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjePasivoVts.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "cbTipoGarantia")
                        {
                            ListEditItem tipoGarantia = cbTipoGarantia.Items.FindByText(dt.Rows[i]["DescInformacion"].ToString());
                            if (tipoGarantia != null)
                                cbTipoGarantia.SelectedIndex = tipoGarantia.Index;
                            //cbTipoGarantia.SelectedIndex = int.Parse((dt.Rows[i]["idInformacion"]).ToString());
                            lbPondTipoGarantia.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPtjeTipoGarantia.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                        }

                        if ((dt.Rows[i]["Descripcion"]).ToString() == "PuntajeTotal")
                        {
                            lbSumatoria.Text = (dt.Rows[i]["ValorPond"]).ToString();
                            lbPuntaje.Text = (dt.Rows[i]["ValorPtje"]).ToString();
                            Page.Session["peritaje"] = dt.Rows[i]["ValorRank"].ToString();
                            //ViewState["peritaje"] = dt.Rows[i]["ValorRank"].ToString();
                            if (dt.Rows[i]["ValorRank"].ToString().Contains("/"))
                                lbPtjeLetra.Text = util.GetText(dt.Rows[i]["ValorRank"].ToString(), "/");
                            else
                                lbPtjeLetra.Text = (dt.Rows[i]["ValorRank"]).ToString();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private string generarXML()
        {
            asignacionResumen(ref objresumen);
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            nodosXML(ref doc, ref ValoresNode, "cbActividad", cbActividad.Value.ToString().Trim(),
                cbActividad.Text.Trim(), lbPtjeActividad.Text.Trim(), lbPondActividad.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbCalidadBalance", cbCalidadBalance.Value.ToString(),
                cbCalidadBalance.Text.Trim(), lbPtjeCalidadBalance.Text.Trim(), lbPondCalidadBalance.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbCalidadGarantia", cbCalidadGarantia.Value.ToString(),
                cbCalidadGarantia.Text.Trim(), lbPtjeCalidadGarantia.Text.Trim(), lbPondCalidadGarantia.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbCoberturaGarantia", cbCoberturaGarantia.SelectedIndex.ToString(),
                cbCoberturaGarantia.Text.Trim(), lbPtjeCoberturaGarantia.Text.Trim(), lbPondCoberturaGarantia.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbCompetencia", cbCompetencia.Value.ToString().Trim(),
                cbCompetencia.Text.Trim(), lbPtjeCompetencia.Text.Trim(), lbPondCompetencia.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbConcentracion", cbConcentracion.Value.ToString(),
                cbConcentracion.Text.Trim(), lbPtjeConcentracion.Text.Trim(), lbPondConcentracion.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbEbitda", cbEbitda.Value.ToString(), cbEbitda.Text.Trim(),
                lbPtjeEbitda.Text.Trim(), lbPondEbitda.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbExperiencia", cbExperiencia.Value.ToString(), cbExperiencia.Text.Trim(),
                lbPtjeExperiencia.Text.Trim(), lbPondExperiencia.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbFamiliar", cbFamiliar.Value.ToString(), cbFamiliar.Text.Trim(),
                lbPtjeFamiliar.Text.Trim(), lbPondFamiliar.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbLeverage", cbLeverage.Value.ToString(), cbLeverage.Text.Trim(),
                lbPtjeLeverage.Text.Trim(), lbPondLeverage.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbLiquidez", cbLiquidez.Value.ToString(), cbLiquidez.Text.Trim(),
                lbPtjeLiquidez.Text.Trim(), lbPondLiquidez.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbMonto", cbMonto.Value.ToString(), cbMonto.Text.Trim(),
                lbPtjeMonto.Text.Trim(), lbPondMonto.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbMonto", cbMonto.Value.ToString(), cbMonto.Text.Trim(),
                lbPtjeMonto.Text.Trim(), lbPondMonto.Text.Trim(), "");

            //ListEditItem morosidad = cbMorosidad.Items.FindByText(cbMorosidad.Text.Trim());
            //if (morosidad != null)
            //    cbMorosidad.SelectedIndex = morosidad.Index;

            nodosXML(ref doc, ref ValoresNode, "cbMorosidad", cbMorosidad.Value.ToString(), cbMorosidad.Text.Trim(),
                lbPtjeMorosidad.Text.Trim(), lbPondMorosidad.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbPasivo", cbPasivo.Value.ToString(), cbPasivo.Text.Trim(),
                lbPtjePasivo.Text.Trim(), lbPondPasivo.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbPasivoVts", cbPasivoVts.Value.ToString(), cbPasivoVts.Text.Trim(),
                lbPtjePasivoVts.Text.Trim(), lbPondPasivoVts.Text.Trim(), "");

            nodosXML(ref doc, ref ValoresNode, "cbTipoGarantia", cbTipoGarantia.Value.ToString(), cbTipoGarantia.Text.Trim(),
                lbPtjeTipoGarantia.Text.Trim(), lbPondTipoGarantia.Text.Trim(), "");

            if (!string.IsNullOrEmpty(Page.Session["peritaje"].ToString()))
                nodosXML(ref doc, ref ValoresNode, "PuntajeTotal", "", "", lbPuntaje.Text.Trim(), lbSumatoria.Text.Trim(), Page.Session["peritaje"].ToString());
            else
                nodosXML(ref doc, ref ValoresNode, "PuntajeTotal", "", "", lbPuntaje.Text.Trim(), lbSumatoria.Text.Trim(), lbPtjeLetra.Text.ToString());

            return doc.OuterXml;
        }

        private void nodosXML(ref XmlDocument doc, ref XmlNode ValoresNode, string descripcion, string idInformacion, string DescInformacion, string ptje, string pond, string letra)
        {
            XmlNode RespNode;

            RespNode = doc.CreateElement("Val");

            XmlNode IdDescNode = doc.CreateElement("Descripcion");
            IdDescNode.AppendChild(doc.CreateTextNode(descripcion));
            RespNode.AppendChild(IdDescNode);

            XmlNode IdCb = doc.CreateElement("Id");
            IdCb.AppendChild(doc.CreateTextNode(idInformacion));
            RespNode.AppendChild(IdCb);

            XmlNode DescCb = doc.CreateElement("Desc");
            DescCb.AppendChild(doc.CreateTextNode(DescInformacion));
            RespNode.AppendChild(DescCb);

            XmlNode nodPtje = doc.CreateElement("Ptje");
            nodPtje.AppendChild(doc.CreateTextNode(ptje));
            RespNode.AppendChild(nodPtje);

            XmlNode nodoPond = doc.CreateElement("Pond");
            nodoPond.AppendChild(doc.CreateTextNode(pond));
            RespNode.AppendChild(nodoPond);

            XmlNode nodoLetra = doc.CreateElement("Letra");
            nodoLetra.AppendChild(doc.CreateTextNode(letra));
            RespNode.AppendChild(nodoLetra);

            ValoresNode.AppendChild(RespNode);
        }

        
        public byte[] GenerarReporte(int idEmpresa)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                String xml = String.Empty;
                DataSet res1 = new DataSet();

                res1 = Ln.ConsultaReporteScoring(idEmpresa, "usuario", "perfil");
                for (int i = 0; i < res1.Tables[0].Rows.Count; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Scoring" + ".xslt");
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

        #endregion

        protected void cbpScoring_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();

            if (e.Parameter.Split(',')[0] == "actividad")
            {
                DataRow[] result = null;
                dt = Ln.ListarActividadEconomica();
                result = dt.Select("ID = '" + e.Parameter.Split(',')[1].Trim() + "'");
                e.Result = string.Format("{0}{1}{2}", e.Parameter.Split(',')[0].Trim(), "~", result[0]["Puntaje"].ToString().Trim());
            }
            else
            {
                dt = Ln.ListarScoring(e.Parameter.Split(',')[1], "", "", "02");
                if (dt.Rows.Count > 0)
                {
                    e.Result = string.Format("{0}{1}{2}", e.Parameter.Split(',')[0], "~", dt.Rows[0]["Valor2"].ToString());
                }
                else
                    e.Result = string.Format("{0}{1}{2}", e.Parameter.Split(',')[0], "~", "0");
            }
        }

        protected void cbpGuardarScoring_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                e.Result = ValidarScoring(e.Parameter);
            }
        }

        protected string ValidarScoring(string Parameter)
        {
            string resultado = string.Empty;
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtPjeTotal = new DataTable("dtPjeTotal");
            DataRow[] result = null;

            if (Page.Session["dtPjeTotal"] == null)
            {
                dtPjeTotal = Ln.ListarScoring("0", "", "", "03");
                Page.Session["dtPjeTotal"] = dtPjeTotal;
            }
            else
                dtPjeTotal = (DataTable)Page.Session["dtPjeTotal"];


            string[] rowValueItems = Parameter.Split(',');

            if (rowValueItems[0] == "indCaso0")
            {
                result = dtPjeTotal.Select("Titulo = '" + int.Parse(rowValueItems[1].ToString()) + "'");
                if (result.Length > 0)
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", result[0]["Descripcion"].ToString() + "/" + result[0]["Valor1"].ToString());
                else
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", "Error al obtener el valor" + " " + rowValueItems[0]);
            }

            if (rowValueItems[0] == "indCaso3")
            {
                result = dtPjeTotal.Select("Titulo = '" + rowValueItems[1].ToString() + "'");
                if (result.Length > 0)
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", result[0]["Valor2"].ToString());
                else
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", "Error al obtener el valor" + " " + rowValueItems[0]);
            }

            if (rowValueItems[0] == "indCaso1")
            {
                result = dtPjeTotal.Select("Valor2 = '" + rowValueItems[1].ToString() + "'");
                if (result.Length > 0)
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", result[0]["Descripcion"].ToString() + "/" + result[0]["Valor1"].ToString());
                else
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", "Error al obtener el valor" + " " + rowValueItems[0]);
            }

            if (rowValueItems[0] == "indCaso2")
            {
                var descripcion = string.Empty;
                if (rowValueItems[1].Contains("/"))
                {
                    //int Start, End;
                    var offset = rowValueItems[1].IndexOf('/');
                    offset = rowValueItems[1].IndexOf('/', offset + 1);
                    var resultado1 = rowValueItems[1].IndexOf('/', offset + 1);
                    descripcion = rowValueItems[1].Substring(0, resultado1);
                }
                else
                    descripcion = rowValueItems[1];

                result = dtPjeTotal.Select("Descripcion = '" + descripcion + "'");
                if (result.Length > 0)
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", result[0]["Valor2"].ToString());
                else
                    resultado = string.Format("{0}{1}{2}", rowValueItems[0], "~", "Error al obtener el valor" + " " + rowValueItems[0]);
            }

            return resultado;
        }

        protected void cbpFinalizarScoring_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "guardar")
                e.Result = GuardarScoring();
            if (e.Parameter == "finalizar")
                e.Result = FinalizarScoring();
            if (e.Parameter == "imprimir")
                Imprimir();
        }

    }
}
