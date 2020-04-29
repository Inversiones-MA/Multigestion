using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using System.Configuration;
using FrameworkIntercapIT.Utilities;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Diagnostics;

namespace MultiContabilidad.wpContabilidad.wpContabilidad
{
    [ToolboxItemAttribute(false)]
    public partial class wpContabilidad : WebPart
    {

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpContabilidad()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        //private static string descTipoTransaccion = string.Empty;
        private static string pagina = "wpContabilidad.aspx";
        Utilidades util = new Utilidades();
        Resumen objresumen = new Resumen();

        #region Eventos

        /* ID TIPO TRANSACCION
           Comisión = 2
           Servicio de Gestión Legal = 4
           Timbre Impuesto y Estampilla (Cargo Multiaval) = 7
           Comisión Fogape = 8
           Fondo Retenido = 
        */

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            DataTable tabla = null;
            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dt2 = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = objresumen.area;

            dt2 = permiso.ListarPerfil(validar);
            if (dt2.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        //BtnConciliar.Visible = false;
                        //pnFormularioConciliacion.Visible = false;
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        ViewState["RES"] = objresumen;
                        tabla = LN.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                        Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbOperacion.Text = objresumen.desOperacion.ToString();

                        ocultarDiv();
                        DataSet dt = new DataSet();
                        string descTipoTransaccion = System.Web.HttpUtility.HtmlDecode(Page.Request.QueryString["idTT"] as string);

                        idTransaccion.Text = descTipoTransaccion;
                        //lbTitulo.Text = descTipoTransaccion;
                        pnDatos.Visible = false;
                        pnDatosAsesoria.Visible = false;
                        pnDatosDevolucion.Visible = false;
                        dt = LN.ConsultaDatosContabilidad(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, descTipoTransaccion);

                        if (dt.Tables[1].Rows.Count > 0)
                        {
                            txtRanzonSocial.Text = dt.Tables[1].Rows[0]["RazonSocial"].ToString();
                            txtRut.Text = dt.Tables[1].Rows[0]["Rut"].ToString();
                            txtActEconomica.Text = dt.Tables[1].Rows[0]["DescActividad"].ToString();
                            txtDireccion.Text = dt.Tables[1].Rows[0]["direccion"].ToString();
                            txtComuna.Text = dt.Tables[1].Rows[0]["DescComuna"].ToString();
                            txtProvincia.Text = dt.Tables[1].Rows[0]["DescProvincia"].ToString();
                            txtRegion.Text = dt.Tables[1].Rows[0]["DescRegion"].ToString();
                            txtTelefonof.Text = dt.Tables[1].Rows[0]["TelFijo1"].ToString();
                            txtEmail.Text = dt.Tables[1].Rows[0]["EMail"].ToString();
                        }

                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            if (descTipoTransaccion != "3" && descTipoTransaccion != "1")
                            {
                                pnDatos.Visible = true;
                                txtCertificado.Text = dt.Tables[0].Rows[0]["NCertificado"].ToString();
                                txtMonto.Text = dt.Tables[0].Rows[0]["MontoOperacion"].ToString();
                                txtFechaEmision.Text = dt.Tables[0].Rows[0]["fecEmision"].ToString();
                                txtFechaVencimiento.Text = dt.Tables[0].Rows[0]["fechaVencimiento"].ToString();
                                txtComisionCLp.Text = dt.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtFondo.Text = dt.Tables[0].Rows[0]["fondo"].ToString();
                                txtProducto.Text = dt.Tables[0].Rows[0]["descProducto"].ToString();
                                txtGastosOperacionales.Text = dt.Tables[0].Rows[0]["gastosOperacionales"].ToString();
                                lbTitulo.Text = dt.Tables[0].Rows[0]["descTipoTransaccion"].ToString();
                                txtAcreedor.Text = dt.Tables[0].Rows[0]["Acreedor"].ToString();
                                txtSeguro.Text = dt.Tables[0].Rows[0]["costoSeguro"].ToString();
                                lbseguro.Text = dt.Tables[0].Rows[0]["incluido"].ToString();
                                txtPlazo.Text = dt.Tables[0].Rows[0]["PlazoMeses"].ToString();
                                lbcomision.Text = dt.Tables[0].Rows[0]["incluidoComision"].ToString();
                                lbgastosOpe.Text = dt.Tables[0].Rows[0]["incluidoGastosOperacionales"].ToString();

                                txtSeguroDesgravamen.Text = dt.Tables[0].Rows[0]["costoSeguroDesgravamen"].ToString();
                                lbSeguroDesgravamen.Text = dt.Tables[0].Rows[0]["incluidoDesgravamen"].ToString();

                                txtTimbreyEstAcreedor.Text = dt.Tables[0].Rows[0]["TimbreYEstampillaAcreedor"].ToString();
                                lbTimbreyEstAcreedor.Text = dt.Tables[0].Rows[0]["incluidoTimbreYEstampillaAcreedor"].ToString();

                                txtComisionFogape.Text = dt.Tables[0].Rows[0]["MontoComisionFogape"].ToString();

                                txtTimbreyEstMultiaval.Text = dt.Tables[0].Rows[0]["TimbreYEstampilla"].ToString();
                                lbTimbreyEstMultiaval.Text = dt.Tables[0].Rows[0]["incluidoTimbreYEstampilla"].ToString();

                                txtNotario.Text = dt.Tables[0].Rows[0]["Notario"].ToString();
                                lbNotario.Text = dt.Tables[0].Rows[0]["incluidoNotario"].ToString();
                            }

                            if (descTipoTransaccion == "3")
                            {
                                pnDatosDevolucion.Visible = true;
                                txtCertificadoD.Text = dt.Tables[0].Rows[0]["NCertificado"].ToString();
                                txtMontoD.Text = dt.Tables[0].Rows[0]["MontoOperacion"].ToString();
                                txtFechaEmisionD.Text = dt.Tables[0].Rows[0]["fecEmision"].ToString();
                                txtComisionCLPD.Text = dt.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtProductoD.Text = dt.Tables[0].Rows[0]["descProducto"].ToString();

                                txtGastosOperacionales.Text = dt.Tables[0].Rows[0]["gastosOperacionales"].ToString();
                                txtAcreedorD.Text = dt.Tables[0].Rows[0]["Acreedor"].ToString();
                                txtFondoD.Text = dt.Tables[0].Rows[0]["fondo"].ToString();
                                txtCostoFondoD.Text = dt.Tables[0].Rows[0]["costoFondo"].ToString();
                                txtNroFacturaComisión.Text = dt.Tables[0].Rows[0]["NroFactura"].ToString();
                                txtFechaFacturaComision.Text = dt.Tables[0].Rows[0]["FechaFactura"].ToString();
                                txtTipoContrato.Text = dt.Tables[0].Rows[0]["tipoContrato"].ToString();
                                txtDevolucionFinal.Text = dt.Tables[0].Rows[0]["devolucionFinal"].ToString();
                                txtDevolucionCostoFondo.Text = dt.Tables[0].Rows[0]["devolucionCostoFondo"].ToString();
                                txtGatosOperacionalesP.Text = dt.Tables[0].Rows[0]["GastosOperacionalPendiente"].ToString();
                                lbTitulo.Text = "Devolución";
                                lbTitulo.Text = dt.Tables[0].Rows[0]["descTipoTransaccion"].ToString();
                            }

                            if (descTipoTransaccion == "1")
                            {
                                pnDatosAsesoria.Visible = true;
                                txtComisionA.Text = dt.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtProductoA.Text = dt.Tables[0].Rows[0]["descProducto"].ToString();
                                txtFechaEstimada.Text = dt.Tables[0].Rows[0]["fecEstimadaCierre"].ToString();
                                lbTitulo.Text = "Asesoría";
                                lbTitulo.Text = dt.Tables[0].Rows[0]["descTipoTransaccion"].ToString();
                            }
                        }

                        //DataTable dt1 = new DataTable();
                        //dt1 = LN.ConsultaFacturacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, descTipoTransaccion);

                        //if (dt1.Rows.Count > 0)
                        //{
                        //    if (dt1.Rows[0]["fecFactura"].ToString() != "")
                        //    {
                        //        dctFechaFactura.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["fecFactura"].ToString());
                        //    }

                        //    txt_Factura.Text = dt1.Rows[0]["numFactura"].ToString();
                        //}

                        CargarDatosConciliacion(descTipoTransaccion);
                        CargarDatosContable(descTipoTransaccion, dt.Tables[0].Rows[0]["fecEmision"].ToString(), dt.Tables[0].Rows[0]["NCertificado"].ToString(), lbTitulo.Text, dt.Tables[0].Rows[0]["IdIVA"].ToString(), dt.Tables[0].Rows[0]["IdTipoDocumento"].ToString());
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                }

                validar.Permiso = dt2.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt2.Rows[0]["Permiso"].ToString(), TieneFiltro);
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

        protected void CargarDatosConciliacion(string descTipoTransaccion)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt1 = new DataTable();
            dt1 = Ln.ConsultaFacturacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), descTipoTransaccion);

            if (dt1.Rows.Count > 0)
            {
                //if (dt1.Rows[0]["fecFactura"].ToString() != "")
                //{
                //    txt_Fecha.Text = dt1.Rows[0]["fecFactura"].ToString();
                //}

                txt_Factura.Text = dt1.Rows[0]["numFactura"].ToString();

                if (dt1.Rows[0]["FechaPago"].ToString() != "")
                    dctFechaPago.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["FechaPago"].ToString());

                if (!string.IsNullOrEmpty(dt1.Rows[0]["IdBanco"].ToString()))
                    ddlbanco.SelectedIndex = ddlbanco.Items.IndexOf(ddlbanco.Items.FindByValue(Convert.ToString(dt1.Rows[0]["IdBanco"].ToString())));

                if (dt1.Rows[0]["IdTipoPago"].ToString() != "")
                    ddlTipoPago.SelectedIndex = ddlTipoPago.Items.IndexOf(ddlTipoPago.Items.FindByValue(Convert.ToString(dt1.Rows[0]["IdTipoPago"].ToString())));

                txtDocumentoPago.Text = dt1.Rows[0]["NroDocumentoPago"].ToString();
                txt_Comentario.Text = dt1.Rows[0]["Comentario"].ToString();
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txt_Factura.Text = "";
            dctFechaFactura.SelectedDate = DateTime.Now;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //guarda valores
            //actualiza estados en ambos 
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "04");

            if (Ln.InsertarActualizarFacturacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                             objresumen.idUsuario, objresumen.descCargo, txt_Factura.Text,
                             (dctFechaFactura.SelectedDate.Day.ToString() + "-" + dctFechaFactura.SelectedDate.Month.ToString() + "-" + dctFechaFactura.SelectedDate.Year.ToString()), idTransaccion.Text, dllIVA.SelectedValue, dllIVA.SelectedItem.Text.Trim(), sslTipoDoc.SelectedValue, sslTipoDoc.SelectedItem.Text.Trim()
                ))

                if (Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), //actualizar etapas en tabla operacion
                                objresumen.idUsuario, objresumen.descCargo, "8", "Cierre", "25", "Facturado", "5", "Cursado", "Contabilidad", "Por Facturar"))
                {
                    Ln.ActualizarEstado(objresumen.idOperacion.ToString(), "8", "Cierre", "25", "Facturado", "5", "Cursado", "Contabilidad"); //LISTA DE SHAREPOINT
                    ocultarDiv();
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);

                    string descTipoTransaccion = System.Web.HttpUtility.HtmlDecode(Page.Request.QueryString["idTT"] as string);
                    CargarDatosConciliacion(descTipoTransaccion);
                    CargarAcreedores();
                    BtnConciliar.Visible = true;
                    pnFormularioConciliacion.Visible = true;
                }
                else
                {
                    ocultarDiv();
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL);
                }
        }

        protected void lbGenerar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("wpContabilidad.aspx" + "?idTT=" + idTransaccion.Text);
        }

        protected void lbPagar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("wpPagar.aspx" + "?idTT=" + idTransaccion.Text);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            asignacionResumen(ref objresumen);

            LogicaNegocio MTO = new LogicaNegocio();
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos["IdEmpresa"] = objresumen.idEmpresa.ToString();
            datos["IdOperacion"] = objresumen.idOperacion.ToString();

            string Reporte = "PorFacturacion";

            byte[] archivo = GenerarReporte(Reporte, datos, "", objresumen);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
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

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        private void CargarDatosContable(string descTipoTransaccion, string fechaEmision, string Ncertificado, string DetalleConcepto, string IdIva, string IdTipoDocumento)
        {
            //if (descTipoTransaccion != "2" && descTipoTransaccion != "4")
            //{
            CargarDocContable();
            sslTipoDoc.SelectedIndex = sslTipoDoc.Items.IndexOf(sslTipoDoc.Items.FindByValue(IdTipoDocumento));
            dllIVA.SelectedIndex = dllIVA.Items.IndexOf(dllIVA.Items.FindByValue(IdIva));
            if (dctFechaFactura.IsDateEmpty)
            {
                DateTime fechaEmi = Convert.ToDateTime(fechaEmision.Split('/')[1] + "-" + fechaEmision.Split('/')[0] + "-" + fechaEmision.Split('/')[2]); ;
                dctFechaFactura.SelectedDate = fechaEmi;
            }
            if (txt_Factura.Text == "")
                txt_Factura.Text = Ncertificado.Trim();
            //}
            //else
            //    CargarDocContable();

            txtDetalle.Text = DetalleConcepto.Trim();

            switch (DetalleConcepto.Trim())
            {
                case "Servicio de Gestión Legal":
                    txtMontoCLP.Text = txtGastosOperacionales.Text.Trim();
                    break;
                case "Comisión":
                    txtMontoCLP.Text = txtComisionCLp.Text.Trim();
                    break;
                case "Timbre Impuesto y Estampilla":
                    txtMontoCLP.Text = txtTimbreyEstMultiaval.Text.Trim();
                    break;
                case "Comisión Fogape":
                    txtMontoCLP.Text = txtComisionFogape.Text.Trim();
                    break;
                case "Seguro Desgravamen":
                    txtMontoCLP.Text = txtSeguroDesgravamen.Text.Trim();
                    break;
                case "Seguro Incendio":
                    txtMontoCLP.Text = txtSeguro.Text.Trim();
                    break;
                default:
                    txtMontoCLP.Text = "0";
                    break;
            }

            if (sslTipoDoc.SelectedIndex > 0)
            {
                CargarAcreedores();
                BtnConciliar.Visible = true;
                pnFormularioConciliacion.Visible = true;
            }
            else
            {
                BtnConciliar.Visible = false;
                pnFormularioConciliacion.Visible = false;
            }

        }

        private void CargarDocContable()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["DocumentoContable"];
            app.AllowUnsafeUpdates = true;

            SPQuery query = new SPQuery();
            query.Query = "<Where>" +
                             "<Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq>" +
                        "</Where>";

            SPListItemCollection collListItems = items.GetItems(query);
            util.CargaDDL(sslTipoDoc, collListItems.GetDataTable(), "Title", "Id");
        }

        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            try
            {
                String xml = String.Empty;
                string descTipoTransaccion = System.Web.HttpUtility.HtmlDecode(Page.Request.QueryString["idTT"] as string);

                //IF PARA CADA REPORTE
                DataSet res1 = new DataSet();
                if (sp == "PorFacturacion")
                {
                    res1 = MTO.ConsultaDatosContabilidadXML(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                               objresumen.idUsuario, objresumen.descCargo, descTipoTransaccion);
                    xml = res1.Tables[0].Rows[0][0].ToString(); ;//GenerarXMLContratoSubFianza(res1);
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


        private void CargarAcreedores()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();

            dt = Ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");
            util.CargaDDL(ddlbanco, dt, "Nombre", "IdAcreedor");
        }

        #endregion

        protected void BtnConciliar_Click(object sender, EventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                LogicaNegocio Ln = new LogicaNegocio();
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "04");

                if (!string.IsNullOrEmpty(txt_Factura.Text.Trim()))
                {
                    if (Ln.InsertarActualizarPagoFacturacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                               objresumen.idUsuario, objresumen.descCargo, txt_Comentario.Text, txtDocumentoPago.Text, ddlbanco.SelectedValue, ddlbanco.SelectedItem.ToString(),
                                (dctFechaPago.SelectedDate.Day.ToString() + "-" + dctFechaPago.SelectedDate.Month.ToString() + "-" + dctFechaPago.SelectedDate.Year.ToString()),
                                ddlTipoPago.SelectedValue, ddlTipoPago.SelectedItem.ToString(), idTransaccion.Text))
                    {
                        if (Ln.InsertarActualizarEstadoPagar(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, "8", "Cierre", "26", "Pagado", "5", "Cursado", "Operaciones", txt_Comentario.Text, idTransaccion.Text))
                        {
                            Ln.ActualizarEstado(objresumen.idOperacion.ToString(), "8", "Cierre", "26", "Pagado", "5", "Cursado", "Operaciones");  //actualizar lista sharepoint
                            //ocultarDiv();
                            //dvSuccess.Style.Add("display", "block");
                            //lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);
                            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                            Page.Response.Redirect("ListarContabilidad.aspx");
                        }
                    }
                }
                else
                {
                    ocultarDiv();
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = ("Datos Incompletos");
                }
            }
            catch
            {
                
            }
        }

    }
}
