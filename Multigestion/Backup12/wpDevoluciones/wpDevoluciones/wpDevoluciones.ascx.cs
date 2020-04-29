using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
//using res = MultiGestion;
using System.Data;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using FrameworkIntercapIT.Utilities;
using System.Xml;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using System.Web;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using MultigestionUtilidades;
using ClasesNegocio;
using Bd;

namespace MultiOperacion.wpDevoluciones.wpDevoluciones
{
    [ToolboxItemAttribute(false)]
    public partial class wpDevoluciones : WebPart
    {
        private static string pagina = "Devoluciones.aspx?NCertificado=";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDevoluciones()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            string Certificado;
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
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
                        Certificado = Page.Request.QueryString["NCertificado"] as string;

                        if (!String.IsNullOrEmpty(Certificado))
                            txtnCertificado.Text = Certificado;

                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        ViewState["RES"] = objresumen;
                        //  ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbOperacion.Text = objresumen.desOperacion;
                        DataSet res;
                        res = LN.ConsultarDatosBasicoDevolucion(objresumen.idEmpresa, objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo);

                        if (res != null)
                        {
                            if (res.Tables[0].Rows.Count > 0)
                            {
                                txtnCertificado.Text = res.Tables[0].Rows[0]["nCertificado"].ToString();
                                txtMontoComision.Text = res.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtNumCredito.Text = res.Tables[0].Rows[0]["NroCredito"].ToString();
                                txtCoberturaC.Text = res.Tables[0].Rows[0]["Cobertura"].ToString();
                                txtFondo.Text = res.Tables[0].Rows[0]["Fondo"].ToString();
                                txtFechaEmision.Text = res.Tables[0].Rows[0]["fecEmision"].ToString();
                                txtFechaVencimiento.Text = res.Tables[0].Rows[0]["fechaVencimiento"].ToString();
                                txtPlazoAM.Text = res.Tables[0].Rows[0]["PlazoAnos"].ToString() + " / " + res.Tables[0].Rows[0]["PlazoMeses"].ToString();
                                txtAcreedor.Text = res.Tables[0].Rows[0]["Acreedor"].ToString();
                                txtCostoFondo.Text = res.Tables[0].Rows[0]["CostoFondo"].ToString();

                                txtCostoFondos.Text = res.Tables[0].Rows[0]["CostoFondo"].ToString();
                                txtUltimaCta.Text = res.Tables[0].Rows[0]["UltCtaPagada"].ToString();
                                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;

                                if (ddlContratoTipo.SelectedValue == "-1")
                                    divCalculo.Visible = false;
                            }
                            else
                            {
                                ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                                btnCancelar.Style.Add("display", "block");
                                btnImprimir.Style.Add("display", "block");
                                btnPrepagar.Style.Add("display", "block");
                            }
                            asignacionJS();
                        }

                        if (res != null)
                        {
                            if (res.Tables[1].Rows.Count > 0)
                            {
                                // dtcFechaCierre.SelectedDate = Convert.ToDateTime(res.Rows[0]["fecEstimadaCierre"].ToString());
                                if (res.Tables[1].Rows[0]["FechaTransaccion"].ToString() != "")
                                {
                                    dtcFechaDevolucion.SelectedDate = Convert.ToDateTime(res.Tables[1].Rows[0]["FechaTransaccion"].ToString());
                                }

                                if (res.Tables[1].Rows[0]["IdTipoContrato"].ToString() != "")
                                    ddlContratoTipo.SelectedIndex = ddlContratoTipo.Items.IndexOf(ddlContratoTipo.Items.FindByValue(Convert.ToString(res.Tables[1].Rows[0]["IdTipoContrato"].ToString())));

                                //format(PorcDevolucion,'N0','es-es') as PorcDevolucion , 
                                //format(DevolucionCLP,'N','es-es') as DevolucionCLP, 
                                //format(PorcDescuentoFijo,'N0','es-es') as PorcDescuentoFijo, 
                                //format(DescuentoFijoCLP,'N','es-es') as DescuentoFijoCLP, 
                                //format(DevolucionCostoFondo,'N','es-es') as DevolucionCostoFondo,
                                if (res.Tables[1].Rows[0]["PorcDevolucion"].ToString() != "")
                                    txtPorcDevolucion.Text = Convert.ToString(res.Tables[1].Rows[0]["PorcDevolucion"].ToString());

                                if (res.Tables[1].Rows[0]["PorcDescuentoFijo"].ToString() != "")
                                    txtPorcDescuentoFijo.Text = Convert.ToString(res.Tables[1].Rows[0]["PorcDescuentoFijo"].ToString());

                                txtCargos.Text = res.Tables[1].Rows[0]["Cargos"].ToString();
                                txtAbonos.Text = res.Tables[1].Rows[0]["Abonos"].ToString();

                                ContratoTipo();
                                CalculoSegunTipoContrato();

                                if (res.Tables[1].Rows[0]["descEstado"].ToString() == "Facturado")
                                    divCalculo.Disabled = true;

                            }
                        }
                    }
                    else { Page.Response.Redirect("MensajeSession.aspx"); }
                }
                else
                {
                    ContratoTipo();
                    CalculoSegunTipoContrato();
                    ocultarDiv();
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
                //Control divFiltros = this.FindControl("filtros");
                //Control divGrilla = this.FindControl("grilla");

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

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnPrepagar_Click(object sender, EventArgs e)
        {
            prepagar("Prepagar");
        }

        protected void ddlContratoTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContratoTipo();
            CalculoSegunTipoContrato();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            prepagar("Guardar");
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

        public void asignacionJS()
        {
            txtCargos.Attributes.Add("onblur", "return sumatoriaGastosOpe('" + btnCancelar.ClientID.Substring(0, btnCancelar.ClientID.Length - 11) + "');");
            txtAbonos.Attributes.Add("onblur", "return sumatoriaGastosOpe('" + btnCancelar.ClientID.Substring(0, btnCancelar.ClientID.Length - 11) + "');");

            txtCargos.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtCargos.ClientID + "');");
            txtAbonos.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtAbonos.ClientID + "');");

            ddlContratoTipo.Attributes["onChange"] = "Dialogo();";//calculoPorcentaje(porcentaje, monto, resultado, cad) 
            txtPorcDevolucion.Attributes["onblur"] = "return calculoPorcentajeDevol('" + btnCancelar.ClientID.Substring(0, btnCancelar.ClientID.Length - 11) + "');";
            txtPorcDescuentoFijo.Attributes["onblur"] = "return calculoPorcentajeDevolFijo('" + btnCancelar.ClientID.Substring(0, btnCancelar.ClientID.Length - 11) + "');";

            txtDevolucionCLP.Attributes.Add("onblur", "return sumatoriaDevolucion('" + btnCancelar.ClientID.Substring(0, btnCancelar.ClientID.Length - 11) + "');");
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            prepagar("Imprimir");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos["IdEmpresa"] = objresumen.idEmpresa.ToString();
            datos["IdOperacion"] = objresumen.idOperacion.ToString();
            datos["Ncertificado"] = txtnCertificado.Text.ToString();

            string Reporte = "Prepago";

            byte[] archivo = GenerarReporte(Reporte, datos, "", objresumen);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
        }

        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        {
            LogicaNegocio LN = new LogicaNegocio();
            try
            {
                string xml = string.Empty;
                DataSet res1 = new DataSet();
                if (sp == "Prepago")
                {
                    res1 = LN.ConsultaReporteBDDevolucion(datos["IdEmpresa"], datos["IdOperacion"], datos["Ncertificado"], "DocumentoCurse", "admin", "ReporteDevoluciones");
                    xml = res1.Tables[0].Rows[0][0].ToString();   //GenerarXMLContratoSubFianza(res1);
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
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

        //IdTipoTransaccion	DescTipoTransaccion * LISTA TipoTransaccion *
        //1	Asesoría
        //2	Comisión
        //3	Devolución Comisión
        //4	GastosOperacionales (Servicio de Gestión Legal)
        //5	Seguro Incendio
        //6 Seguro Desgravamen
        //7 Timbre Impuesto y Estampilla
        //8 Comisión Fogape
        //9 Fondo Retenido

        public void prepagar(string accion)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            Boolean res;
            string fecha = "-1";

            if (!dtcFechaDevolucion.IsDateEmpty)
                fecha = dtcFechaDevolucion.SelectedDate.Day.ToString() + "-" + dtcFechaDevolucion.SelectedDate.Month.ToString() + "-" + dtcFechaDevolucion.SelectedDate.Year.ToString();
            //   string fecha = ((DateTimeControl)Controles).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Controles).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Controles).SelectedDate.Year.ToString();

            if (res = MTO.GuardarDevolucion(objresumen.idEmpresa, objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo,
                "3", "Devolución Comisión", fecha, ddlContratoTipo.SelectedValue, ddlContratoTipo.SelectedItem.ToString(),
                txtPorcDevolucion.Text.Replace(".", "").Replace(",", "."), txtDevolucionCLP.Text.Replace(".", "").Replace(",", "."),
                txtPorcDescuentoFijo.Text.Replace(".", "").Replace(",", "."),
                txtDescuentoFijo.Text.Replace(".", "").Replace(",", "."), txtDevolucionCostoFondo.Text.Replace(".", "").Replace(",", "."),
                txtAbonos.Text.Replace(".", "").Replace(",", "."), txtCargos.Text.Replace(".", "").Replace(",", "."), accion))
            {
                ocultarDiv();
                dvSuccess.Style.Add("display", "block");
                lbSuccess.Text = util.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);
            }
            else
            {
                ocultarDiv();
                dvError.Style.Add("display", "block");
                lbError.Text = util.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL);
            }
        }

        /// <summary>
        /// Habilita o deshabilita campos según el tipo de contrato seleccionado.
        /// </summary>
        public void ContratoTipo()
        {

            if (ddlContratoTipo.SelectedValue == "-1")
                divCalculo.Visible = false;

            if (ddlContratoTipo.SelectedValue == "1")//Corfo
            {
                divCalculo.Visible = true;
                dvAnoPrepago.Visible = false;
                dvMesPrepago.Visible = false;
                //dvUltimaCta.Visible = true;
                dvPorcDevolucion.Visible = false;
                dvDevolucionCLPs.Visible = true;
                dvPocDescuentoFijo.Visible = true;
                dvtxtDescuentoFijo.Visible = true;
                dvDevolucion.Visible = true;
                dvCostoFondo.Visible = true;
                dvPocDevolucion2.Visible = false;
                dvDevolucionCostoFondo.Visible = true;
                dvAbonos.Visible = true;
                dvCargos.Visible = true;
                dvSaldoGastos.Visible = true;
                txtAnoPrepago.Enabled = true;
                txtMesPrepago.Enabled = true;
                txtUltimaCta.Enabled = false;
                txtPorcDevolucion.Enabled = true;
                txtPorcDevolucion.Text = "0";
                txtDevolucionCLP.Enabled = true;
                txtPorcDescuentoFijo.Enabled = false;
                txtPorcDescuentoFijo.Text = "0";
                txtDescuentoFijo.Enabled = false;
                txtDevolucion.Enabled = false;
                txtCostoFondo.Enabled = false;
                txtPorcDevolucion2.Enabled = true;
                txtDevolucionCostoFondo.Enabled = false;
                txtAbonos.Enabled = true;
                txtCargos.Enabled = true;
                txtSaldoGastos.Enabled = false;
            }

            if (ddlContratoTipo.SelectedValue == "2")//tabla anual
            {
                divCalculo.Visible = true;
                dvAnoPrepago.Visible = true;
                dvMesPrepago.Visible = false;
                //dvUltimaCta.Visible = false;
                dvPorcDevolucion.Visible = true;
                dvDevolucionCLPs.Visible = true;
                dvPocDescuentoFijo.Visible = true;
                dvtxtDescuentoFijo.Visible = true;
                dvDevolucion.Visible = true;
                dvCostoFondo.Visible = true;
                dvPocDevolucion2.Visible = true;
                dvDevolucionCostoFondo.Visible = true;
                dvAbonos.Visible = true;
                dvCargos.Visible = true;
                dvSaldoGastos.Visible = true;
                txtAnoPrepago.Enabled = false;
                txtMesPrepago.Enabled = false;
                txtUltimaCta.Enabled = false;
                txtPorcDevolucion.Enabled = true;
                txtDevolucionCLP.Enabled = false;
                txtPorcDescuentoFijo.Enabled = true;
                txtDescuentoFijo.Enabled = false;
                txtDevolucion.Enabled = false;
                txtCostoFondo.Enabled = false;
                txtPorcDevolucion2.Enabled = false;
                txtDevolucionCostoFondo.Enabled = false;
                txtAbonos.Enabled = true;
                txtCargos.Enabled = true;
                txtSaldoGastos.Enabled = false;
            }

            if (ddlContratoTipo.SelectedValue == "3")//tabla especial
            {
                divCalculo.Visible = true;
                dvAnoPrepago.Visible = true;
                dvMesPrepago.Visible = false;
                //dvUltimaCta.Visible = false;
                dvPorcDevolucion.Visible = true;
                dvDevolucionCLPs.Visible = true;
                dvPocDescuentoFijo.Visible = true;
                dvtxtDescuentoFijo.Visible = true;
                dvDevolucion.Visible = true;
                dvCostoFondo.Visible = true;
                dvPocDevolucion2.Visible = true;
                dvDevolucionCostoFondo.Visible = true;
                dvAbonos.Visible = true;
                dvCargos.Visible = true;
                dvSaldoGastos.Visible = true;
                txtAnoPrepago.Enabled = false;
                txtMesPrepago.Enabled = false;
                txtUltimaCta.Enabled = false;
                txtPorcDevolucion.Enabled = true;
                txtDevolucionCLP.Enabled = false;
                txtPorcDescuentoFijo.Enabled = true;
                txtDescuentoFijo.Enabled = false;
                txtDevolucion.Enabled = false;
                txtCostoFondo.Enabled = false;
                txtPorcDevolucion2.Enabled = false;
                txtDevolucionCostoFondo.Enabled = false;
                txtAbonos.Enabled = true;
                txtCargos.Enabled = true;
                txtSaldoGastos.Enabled = false;
            }

            if (ddlContratoTipo.SelectedValue == "4")//Proaval
            {
                divCalculo.Visible = true;
                dvAnoPrepago.Visible = true;
                dvMesPrepago.Visible = true;
                //dvUltimaCta.Visible = false;
                dvPorcDevolucion.Visible = true;
                dvDevolucionCLPs.Visible = true;
                dvPocDescuentoFijo.Visible = true;
                dvtxtDescuentoFijo.Visible = true;
                dvDevolucion.Visible = true;
                dvCostoFondo.Visible = true;
                dvPocDevolucion2.Visible = true;
                dvDevolucionCostoFondo.Visible = true;
                dvAbonos.Visible = true;
                dvCargos.Visible = true;
                dvSaldoGastos.Visible = true;
                txtAnoPrepago.Enabled = false;
                txtMesPrepago.Enabled = false;
                txtUltimaCta.Enabled = false;
                txtPorcDevolucion.Enabled = true;
                txtDevolucionCLP.Enabled = false;
                txtPorcDescuentoFijo.Enabled = false;
                txtDescuentoFijo.Enabled = false;
                txtDevolucion.Enabled = false;
                txtCostoFondo.Enabled = false;
                txtPorcDevolucion2.Enabled = false;
                txtDevolucionCostoFondo.Enabled = false;
                txtAbonos.Enabled = true;
                txtCargos.Enabled = true;
                txtSaldoGastos.Enabled = false;
            }

            if (ddlContratoTipo.SelectedValue == "5")//Reverso
            {
                divCalculo.Visible = false;
            }
        }


        /// <summary>
        /// Cálculo  inicial
        /// </summary>
        public void CalculoSegunTipoContrato()
        {
            try
            {
                if (!dtcFechaDevolucion.IsDateEmpty || dtcFechaDevolucion.SelectedDate.ToString("dd-mm-yyyy") != DateTime.Now.ToString("dd-mm-yyyy"))
                {
                    try
                    {
                        string fecha = dtcFechaDevolucion.SelectedDate.Day.ToString() + "/" + dtcFechaDevolucion.SelectedDate.Month.ToString() + "/" + dtcFechaDevolucion.SelectedDate.Year.ToString();
                        DateTime ft = DateTime.ParseExact(txtFechaEmision.Text.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                        //DateTime.ParseExact(txtFechaEmision.Text.ToString(), "dd/MM/yyyy", null);

                        DateTime ft1 = DateTime.ParseExact(fecha, "d/M/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                        //DateTime.ParseExact(fecha, "dd/MM/yyyy", null);


                        txtAnoPrepago.Text = Math.Truncate(Convert.ToDouble((((ft1 - ft).Days) / 365).ToString())).ToString();
                        txtMesPrepago.Text = Math.Truncate(Convert.ToDouble((((ft1 - ft).Days) / 30).ToString())).ToString();
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    }
                }
                else
                {
                    txtAnoPrepago.Text = "0";
                    txtMesPrepago.Text = "0";
                }

                if (txtPorcDevolucion.Text.Replace(".", "") != "")
                {
                    txtDevolucionCLP.Text = ((Convert.ToDouble(txtPorcDevolucion.Text.Replace(".", "").Replace(",", ".")) / 100) * Convert.ToDouble(txtMontoComision.Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                    if (txtCostoFondo.Text.Replace(".", "").Replace(",", ".") != "" || txtCostoFondo.Text.Replace(".", "").Replace(",", ".") != "0")
                    {
                        txtCostoFondo.Text = txtCostoFondos.Text;
                        txtPorcDevolucion2.Text = txtPorcDevolucion.Text;
                        txtDevolucionCostoFondo.Text = ((Convert.ToDouble(txtPorcDevolucion.Text.Replace(".", "").Replace(",", ".")) / 100) * Convert.ToDouble(txtCostoFondo.Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else { txtCostoFondo.Text = "0"; txtDevolucionCostoFondo.Text = "0"; txtPorcDevolucion2.Text = "0"; }

                }

                if (txtPorcDescuentoFijo.Text.ToString() == "")
                    txtPorcDescuentoFijo.Text = "0";

                if (txtDevolucionCLP.Text.ToString() == "")
                    txtDevolucionCLP.Text = "0";

                if (txtPorcDescuentoFijo.Text.Replace(".", "") != "")
                {
                    txtDescuentoFijo.Text = ((Convert.ToDouble(txtPorcDescuentoFijo.Text.Replace(".", "").Replace(",", ".")) / 100) * Convert.ToDouble(txtDevolucionCLP.Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                if (txtDevolucionCLP.Text == "")
                    txtDevolucionCLP.Text = "0";
                double devol = double.Parse(txtDevolucionCLP.Text.Replace(".", "").Replace(",", "."));
                double descFijo = double.Parse(txtDescuentoFijo.Text.Replace(".", "").Replace(",", "."));
                txtDevolucion.Text = (devol - descFijo).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                if (txtCargos.Text == "")
                    txtCargos.Text = "0";
                if (txtAbonos.Text == "")
                    txtAbonos.Text = "0";

                txtSaldoGastos.Text = (Convert.ToDouble(txtCargos.Text.Replace(".", "").Replace(",", ".")) + Convert.ToDouble(txtAbonos.Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                txtSaldoCliente.Text = (Convert.ToDouble(txtDevolucion.Text.Replace(".", "").Replace(",", ".")) + Convert.ToDouble(txtSaldoGastos.Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                if (ddlContratoTipo.SelectedValue == "-1")//Seleccione
                    divCalculo.Visible = false;

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        #endregion

    }
}
