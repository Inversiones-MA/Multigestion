using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.ClientServices;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Xml;
using System.Collections.Generic;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Microsoft.SharePoint;
using MultigestionUtilidades;
using ClasesNegocio;
using Bd;

namespace MultiRiesgo.wpPAF.wpPAF
{
    [ToolboxItemAttribute(false)]
    public partial class wpPAF : WebPart
    {
        private static string pagina = "ResumenPaf.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPAF()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        public SPListItem carpetaCliente;


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            //LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

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
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbOperacion.Text = objresumen.desOperacion;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        cargarListas();
                        this.ctrFecha.DateChanged += ctrFecha_DateChanged;
                        ((TextBox)this.ctrFecha.Controls[0]).TextChanged += ctrFecha_TextChanged;

                        try
                        {
                            cargaDatosPAF();
                        }
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }

                        if (objresumen.desEtapa == "Evaluación Riesgo")
                            btnAdjuntar.Visible = false;

                        if (objresumen.desEtapa == "Comité")
                            btnAdjuntar.Visible = true;
                    }
                    else
                        Page.Response.Redirect("MensajeSession.aspx");

                    try
                    {
                        cargarEmpresa();
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    }

                    if (string.IsNullOrEmpty(txtUFV.Text))
                    {
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = lbWarning.Text + " " + " No se encuentró moneda registrada, por favor comuniquese con el area de Operaciones.";
                    }

                    if (string.IsNullOrEmpty(lblFClasif.Text))
                    {
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = lbWarning.Text + " " + " Scoring Incompleto.";
                    }

                    ConsultarImpresionPAF();
                }
                inicializacionGrillas();

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

            //if(objresumen.area.ToLower() == "fiscalia")
            //    dvFormulario.Attributes.Remove("Disabled");
        }

        protected void ckSinDocumentos_CheckedChanged(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");

            LogicaNegocio Ln = new LogicaNegocio();
            Ln.sinDocumentosContables(objresumen.idEmpresa.ToString(), objresumen.idUsuario, objresumen.descCargo, ViewState["idp"].ToString(), ckSinDocumentos.Checked);
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("ResumenPaf.aspx");
        }

        protected void gridOperacionesNuevas_DataBound(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)ViewState["OperacionesNuevas"];
            double totalAprobadoCLP = 0;
            double totalVigenteCLP = 0;
            double totalPropuestoCLP = 0;

            double totalAprobadoUF = 0;
            double totalVigenteUF = 0;
            double totalPropuestoUF = 0;

            double totalPropuestoOperacionesNuevas = 0;

            double totalAprobadoUSD = 0;
            double totalVigenteUSD = 0;
            double totalPropuestoUSD = 0;

            double coberturaCertificado = 0;
            OpeNueva.Value = "0";
            try
            {
                for (int i = 0; i <= gridOperacionesNuevas.Rows.Count - 1; i++)
                {
                    if (int.Parse(dt1.Rows[i]["Plazo"].ToString()) > 12)
                        (gridOperacionesNuevas.Rows[i].FindControl("txtTipoFinanciamiento") as TextBox).Text = "Largo Plazo";
                    else
                        (gridOperacionesNuevas.Rows[i].FindControl("txtTipoFinanciamiento") as TextBox).Text = "Corto Plazo";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtPlazo") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Plazo"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Plazo"].ToString()) : "0";

                    (gridOperacionesNuevas.Rows[i].FindControl("txtHorizonte") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Horizonte"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Horizonte"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtCobertura") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["CoberturaCertificado"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["CoberturaCertificado"].ToString()) : "0";

                    (gridOperacionesNuevas.Rows[i].FindControl("txtComision") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Comisión Min. %"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Comisión Min. %"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtSeguro") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Seguro"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Seguro"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtComisionCLP") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Comisión"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Comisión"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtMontoCredito") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Crédito"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Crédito"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString()) : "0";
                    (gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString()) : "0";

                    (gridOperacionesNuevas.Rows[i].FindControl("lbTipoMoneda") as Label).Text = dt1.Rows[i]["Tipo Moneda"].ToString();

                    if (string.IsNullOrEmpty(dt1.Rows[i]["CoberturaCertificado"].ToString()))
                        coberturaCertificado = 1;
                    else
                        coberturaCertificado = double.Parse(dt1.Rows[i]["CoberturaCertificado"].ToString()) / 100;


                    if (dt1.Rows[i]["Tipo Moneda"].ToString() == "UF")
                    {
                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Aprobado"].ToString()))
                            totalAprobadoUF = totalAprobadoUF + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (string.IsNullOrEmpty(dt1.Rows[i]["Monto Vigente"].ToString()))
                            totalVigenteUF = totalVigenteUF + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Propuesto"].ToString()))
                            totalPropuestoUF = totalPropuestoUF + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString())); //.Replace(".", "").Replace(",", ".")));
                    }

                    if (dt1.Rows[i]["Tipo Moneda"].ToString() == "CLP")
                    {
                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Aprobado"].ToString()))
                            totalAprobadoCLP = totalAprobadoCLP + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Vigente"].ToString()))
                            totalVigenteCLP = totalVigenteCLP + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Propuesto"].ToString()))
                            totalPropuestoCLP = totalPropuestoCLP + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString())); //.Replace(".", "").Replace(",", ".")));
                    }

                    if (dt1.Rows[i]["Tipo Moneda"].ToString() == "USD")
                    {
                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Aprobado"].ToString()))
                            totalAprobadoUSD = totalAprobadoUSD + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (string.IsNullOrEmpty(dt1.Rows[i]["Monto Vigente"].ToString()))
                            totalVigenteUSD = totalVigenteUSD + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (string.IsNullOrEmpty(dt1.Rows[i]["Monto Propuesto"].ToString()))
                            totalPropuestoUSD = totalPropuestoUSD + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString())); //.Replace(".", "").Replace(",", ".")));
                    }

                    if (!string.IsNullOrEmpty(dt1.Rows[i]["CoberturaCertificado"].ToString()))
                        totalPropuestoOperacionesNuevas = totalPropuestoOperacionesNuevas + Double.Parse(dt1.Rows[i]["CoberturaCertificado"].ToString()) / 100 * Double.Parse(dt1.Rows[i]["Monto Propuesto"].ToString() != "" ? (dt1.Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")) : "0");

                    (gridOperacionesNuevas.Rows[i].FindControl("txtCobertura") as TextBox).Attributes.Add("onBlur", "return SumatoriaColumGridMP('" + gridOperacionesVigentes.ClientID +
                      "','" + gridOperacionesNuevas.ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + txtDOLAR.Text + "','" + txtUF.Text + "','" + Garantias.ClientID + "');");


                    (gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Attributes.Add("onBlur", "return SumatoriaColumGridMP('" + gridOperacionesVigentes.ClientID +
                      "','" + gridOperacionesNuevas.ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + txtDOLAR.Text + "','" + txtUF.Text + "','" + Garantias.ClientID + "');");


                    //(gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Attributes.Add("onBlur", "return SumatoriaColumGridMA('" + gridOperacionesVigentes.ClientID +
                    //  "','" + gridOperacionesNuevas.ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + txtDOLAR.Text + "','" + txtUF.Text + "','" + Garantias.ClientID + "');");


                    //(gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Attributes.Add("onBlur", "return SumatoriaColumGridMA('" + gridOperacionesVigentes.ClientID +
                    //  "','" + gridOperacionesNuevas.ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + txtDOLAR.Text + "','" + txtUF.Text + "','" + Garantias.ClientID + "');");

                    (gridOperacionesNuevas.Rows[i].FindControl("GaranElim") as LinkButton).CommandArgument = dt1.Rows[i]["IdOperacion"].ToString();
                    (gridOperacionesNuevas.Rows[i].FindControl("GaranElim") as LinkButton).Command += EliminarOperacionesNuevas_Command;
                }

                if (totalAprobadoCLP > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text = util.formatearNumero(totalAprobadoCLP, 0);
                //(totalAprobadoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text = "0,000";

                if (totalAprobadoUSD > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text = util.formatearNumero(totalAprobadoUSD, 4);
                //totalAprobadoUSD.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text = "0,000";

                if (totalAprobadoUF > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text = util.formatearNumero(totalAprobadoUF, 4);
                //totalAprobadoUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text = "0,000";

                if (totalAprobadoCLP > 0 && float.Parse(txtUF.Text) > 0)
                    totalAprobadoUF = totalAprobadoUF + (totalAprobadoCLP / float.Parse(txtUF.Text));
                if (totalAprobadoUSD > 0 && float.Parse(txtUF.Text) > 0 && float.Parse(txtDOLAR.Text) > 0)
                    totalAprobadoUF = totalAprobadoUF + (totalAprobadoUSD * float.Parse(txtDOLAR.Text) / float.Parse(txtUF.Text));

                if (totalAprobadoUF > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoREUF") as TextBox).Text = util.formatearNumero(totalAprobadoUF, 4);
                //(totalAprobadoUF).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoREUF") as TextBox).Text = "0,000";

                if (totalVigenteCLP > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text = util.formatearNumero(totalVigenteCLP, 0);
                //(totalVigenteCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text = "0,000";

                if (totalVigenteUSD > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text = util.formatearNumero(totalVigenteUSD, 4);
                //totalVigenteUSD.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text = "0,000";

                if (totalVigenteUF > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text = util.formatearNumero(totalVigenteUF, 4);
                //totalVigenteUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text = "0,000";

                if (totalVigenteCLP > 0 && float.Parse(txtUF.Text) > 0)
                    totalVigenteUF = totalVigenteUF + (totalVigenteCLP / float.Parse(txtUF.Text));
                if (totalVigenteUSD > 0 && float.Parse(txtUF.Text) > 0 && float.Parse(txtDOLAR.Text) > 0)
                    totalVigenteUF = totalVigenteUF + (totalVigenteUSD * float.Parse(txtDOLAR.Text) / float.Parse(txtUF.Text));

                if (totalVigenteUF > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text = util.formatearNumero(totalVigenteUF, 4);
                //(totalVigenteUF).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text = "0,000";

                if (totalPropuestoCLP > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text = util.formatearNumero(totalPropuestoCLP, 4);
                //totalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text = "0,000";

                if (totalPropuestoUSD > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text = util.formatearNumero(totalPropuestoUSD, 4);
                //totalPropuestoUSD.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text = "0,000";

                if (totalPropuestoUF > 0)
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text = util.formatearNumero(totalPropuestoUF, 4);
                //totalPropuestoUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES")); //(totalPropuestoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text = "0,000";

                //pasar a uf
                if (totalPropuestoCLP > 0 && float.Parse(txtUF.Text) > 0)
                    totalPropuestoUF = totalPropuestoUF + (totalPropuestoCLP / float.Parse(txtUF.Text));
                if (totalPropuestoUSD > 0 && float.Parse(txtUF.Text) > 0 && float.Parse(txtDOLAR.Text) > 0)
                    totalPropuestoUF = totalPropuestoUF + (totalPropuestoUSD * float.Parse(txtDOLAR.Text) / float.Parse(txtUF.Text));

                OpeNueva.Value = totalPropuestoUF.ToString();

                (gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text = util.formatearNumero(totalPropuestoUF, 4);
                //(totalPropuestoUF).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            dt1 = null;
        }

        protected void gridOperacionesNuevas_RowDataBound(object sender, GridViewRowEventArgs e)
        {


        }

        protected void gridOperacionesNuevas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[15].Visible = false;//IdOperacion no Visible
        }

        protected void EliminarOperacionesNuevas_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();

            if (e.CommandName == "Eliminar")
            {
                DataTable OperacionesNuevas = (DataTable)ViewState["OperacionesNuevas"];
                for (int i = 0; i < OperacionesNuevas.Rows.Count; i++)
                {
                    // dt1.Rows[i]["IdGarantia"].ToString();
                    if (e.CommandArgument.ToString() == OperacionesNuevas.Rows[i]["IdOperacion"].ToString())
                    {
                        Ln.DarBajaOperacion(OperacionesNuevas.Rows[i]["IdOperacion"].ToString());

                        OperacionesNuevas.Rows[i].Delete();
                        OperacionesNuevas.AcceptChanges();
                        i = OperacionesNuevas.Rows.Count;
                    }
                }
                gridOperacionesNuevas.DataSource = OperacionesNuevas;
                gridOperacionesNuevas.DataBind();
            }
        }

        protected void gridOperacionesVigentes_DataBound(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)ViewState["OperacionesVigentes"];
            double totalAprobadoCLP = 0;
            double totalVigenteCLP = 0;
            double totalPropuestoCLP = 0;
            double totalAprobadoUF = 0;
            double totalVigenteUF = 0;
            double totalPropuestoUF = 0;
            double totalAprobadoUSD = 0;
            double totalVigenteUSD = 0;
            double totalPropuestoUSD = 0;
            double totalPropuestoOperacionesVigentes = 0;
            double coberturaCertificado = 0;
            OpeVigente.Value = "0";
            try
            {
                for (int i = 0; i <= gridOperacionesVigentes.Rows.Count - 1; i++)
                {
                    (gridOperacionesVigentes.Rows[i].FindControl("txtMontoCredito") as Label).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Crédito"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Crédito"].ToString()) : "0";
                    (gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString()) : "0";
                    (gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString()) : "0";
                    (gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString()) : "0";

                    (gridOperacionesVigentes.Rows[i].FindControl("lbTipoMoneda") as Label).Text = dt1.Rows[i]["Tipo Moneda"].ToString();

                    if (string.IsNullOrEmpty(dt1.Rows[i]["CoberturaCertificado"].ToString()))
                        coberturaCertificado = 1;
                    else
                        coberturaCertificado = double.Parse(dt1.Rows[i]["CoberturaCertificado"].ToString()) / 100;

                    if (dt1.Rows[i]["Tipo Moneda"].ToString() == "UF")
                    {
                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Aprobado"].ToString()))
                            totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Vigente"].ToString()))
                            totalVigenteUF = totalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Propuesto"].ToString()))
                            totalPropuestoUF = totalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                    }

                    if (dt1.Rows[i]["Tipo Moneda"].ToString() == "CLP")
                    {
                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Aprobado"].ToString()))
                            totalAprobadoCLP = totalAprobadoCLP + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Vigente"].ToString()))
                            totalVigenteCLP = totalVigenteCLP + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Propuesto"].ToString()))
                            totalPropuestoCLP = totalPropuestoCLP + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString())); //.Replace(".", "").Replace(",", ".")));
                    }

                    if (dt1.Rows[i]["Tipo Moneda"].ToString() == "USD")
                    {
                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Aprobado"].ToString()))
                            totalAprobadoUSD = totalAprobadoUSD + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Aprobado"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Vigente"].ToString()))
                            totalVigenteUSD = totalVigenteUSD + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Vigente"].ToString())); //.Replace(".", "").Replace(",", ".")));

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Monto Propuesto"].ToString()))
                            totalPropuestoUSD = totalPropuestoUSD + util.GetValorDouble(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Monto Propuesto"].ToString())); //.Replace(".", "").Replace(",", ".")));
                    }

                    (gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Attributes.Add("onBlur", "return SumatoriaColumGrid('" +
                        gridOperacionesVigentes.ClientID + "','" +
                        gridOperacionesNuevas.ClientID + "','" +
                        btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" +
                        txtDOLAR.Text.ToString() + "','" +
                        txtUF.Text.ToString() + "','" + Garantias.ClientID + "');");

                    if (!string.IsNullOrEmpty(dt1.Rows[i]["CoberturaCertificado"].ToString()))
                        totalPropuestoOperacionesVigentes = totalPropuestoOperacionesVigentes + Double.Parse(dt1.Rows[i]["CoberturaCertificado"].ToString()) / 100 * Double.Parse(dt1.Rows[i]["Monto Propuesto"].ToString() != "" ? (dt1.Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")) : "0");

                }

                if (gridOperacionesVigentes.Rows.Count > 0)
                {
                    if (totalAprobadoCLP > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text = util.formatearNumero(totalAprobadoCLP, 0);
                    //(totalAprobadoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES")); // * float.Parse(txtUF.Text)
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text = "0,000";

                    if (totalAprobadoUSD > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text = util.formatearNumero(totalAprobadoUSD, 4);
                    //totalAprobadoUSD.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text = "0,000";

                    if (totalAprobadoUF > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text = util.formatearNumero(totalAprobadoUF, 4);
                    //totalAprobadoUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text = "0,000";

                    if (totalAprobadoCLP > 0 && float.Parse(txtUF.Text) > 0)
                        totalAprobadoUF = totalAprobadoUF + (totalAprobadoCLP / float.Parse(txtUF.Text));
                    if (totalAprobadoUSD > 0 && float.Parse(txtUF.Text) > 0 && float.Parse(txtDOLAR.Text) > 0)
                        totalAprobadoUF = totalAprobadoUF + (totalAprobadoUSD * float.Parse(txtDOLAR.Text) / float.Parse(txtUF.Text));

                    (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoREUF") as TextBox).Text = util.formatearNumero(totalAprobadoUF, 4);
                    //(totalAprobadoUF).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                    (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text = util.formatearNumero(totalVigenteCLP, 0);
                    //(totalVigenteCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text = util.formatearNumero(totalVigenteUSD, 4);
                    //totalVigenteUSD.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text = util.formatearNumero(totalVigenteUF, 4);
                    //totalVigenteUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES")); ;

                    if (totalVigenteCLP > 0 && float.Parse(txtUF.Text) > 0)
                        totalVigenteUF = totalVigenteUF + (totalVigenteCLP / float.Parse(txtUF.Text));
                    if (totalVigenteUSD > 0 && float.Parse(txtUF.Text) > 0 && float.Parse(txtDOLAR.Text) > 0)
                        totalVigenteUF = totalVigenteUF + (totalVigenteUSD * float.Parse(txtDOLAR.Text) / float.Parse(txtUF.Text));

                    if (totalVigenteUF > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text = util.formatearNumero(totalVigenteUF, 4);
                    //(totalVigenteUF).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text = "0,000";

                    if (totalPropuestoCLP > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text = util.formatearNumero(totalPropuestoCLP, 0);
                    //(totalPropuestoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text = "0,000";

                    if (totalPropuestoUSD > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text = util.formatearNumero(totalPropuestoUSD, 4);
                    //totalPropuestoUSD.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text = "0,000";

                    if (totalPropuestoUF > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text = util.formatearNumero(totalPropuestoUF, 4);
                    //totalPropuestoUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text = "0,000";

                    if (totalPropuestoCLP > 0 && float.Parse(txtUF.Text) > 0)
                        totalPropuestoUF = totalPropuestoUF + (totalPropuestoCLP / float.Parse(txtUF.Text));
                    if (totalPropuestoUSD > 0 && float.Parse(txtUF.Text) > 0 && float.Parse(txtDOLAR.Text) > 0)
                        totalPropuestoUF = totalPropuestoUF + (totalPropuestoUSD * float.Parse(txtDOLAR.Text) / float.Parse(txtUF.Text));

                    if (totalPropuestoUF > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text = util.formatearNumero(totalPropuestoUF, 4);
                    //totalPropuestoUF.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                    {
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text = "0,000";
                        OpeVigente.Value = "0";
                    }

                    //monto propuesto operaciones vigentes * cobertura certificado
                    if (totalPropuestoUF > 0)
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoVigente") as TextBox).Text = util.formatearNumero(totalPropuestoUF, 4);
                    //(totalPropuestoUF).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    else
                        (gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoVigente") as TextBox).Text = "0,000";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            dt1 = null;
        }

        protected void gridOperacionesVigentes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridOperacionesVigentes_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[15].Visible = false;//IdOperacion no Visible
        }

        protected void ActualizarOperacionVigentes_Click(object sender, EventArgs e)
        {
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            DataSet res = new DataSet();
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.ObtenerDatosEmpresaPafActual(objresumen.idEmpresa.ToString(), ViewState["idp"].ToString(), objresumen.idUsuario, objresumen.descCargo);
            if (res.Tables.Count > 0)
            {
                ViewState["OperacionesVigentes"] = res.Tables[2];
                gridOperacionesVigentes.DataSource = res.Tables[2];
                gridOperacionesVigentes.DataBind();
            }
        }

        protected void ActualizarOperacionesNuevas_Click(object sender, EventArgs e)
        {
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            DataSet res = new DataSet();
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.ObtenerDatosEmpresaPafActual(objresumen.idEmpresa.ToString(), ViewState["idp"].ToString(), objresumen.idUsuario, objresumen.descCargo);
            if (res.Tables.Count > 0)
            {
                ViewState["OperacionesNuevas"] = res.Tables[3];
                gridOperacionesNuevas.DataSource = res.Tables[3];
                gridOperacionesNuevas.DataBind();
            }
        }

        protected void ActualizarGarantias_Click(object sender, EventArgs e)
        {
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            DataSet res = new DataSet();
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.ObtenerDatosEmpresaPafActual(objresumen.idEmpresa.ToString(), ViewState["idp"].ToString(), objresumen.idUsuario, objresumen.descCargo);
            if (res.Tables.Count > 0)
            {
                ViewState["GarantiaTodas"] = res.Tables[1];
                Garantias.DataSource = res.Tables[1];
                Garantias.DataBind();
            }
        }

        protected void btnImprimirPAF2_Click(object sender, EventArgs e)
        {
            //asignacionResumen(ref objresumen);
            //LogicaNegocio Ln = new LogicaNegocio();
            //Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");

            //if (ViewState["idp"] == null)
            //    Page.Response.Redirect("MensajeSession.aspx");
            ////imprimir
            //LogicaNegocio Ln = new LogicaNegocio();
            //byte[] archivo = null;

            //if (lbFecImprimirPAF2.Text != "Por favor proceder a calcular la PAF 2")
            //{
            //    DataSet dt = Ln.ActualizarImpresionPAF(int.Parse(ViewState["idp"].ToString()));
            //    string NombreArchivo = dt.Tables[0].Rows[0][2].ToString();
            //    archivo = File.ReadAllBytes(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/archivosPDF/" + NombreArchivo);
            //}
            //else
            //{
            //    archivo = File.ReadAllBytes(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/archivosPDF/PAF_NoCalculada.pdf");
            //}

            //if (archivo != null)
            //{
            //    Page.Session["tipoDoc"] = "pdf";
            //    Page.Session["binaryData"] = archivo;
            //    Page.Session["Titulo"] = "ResumenPAF";
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            //}
        }

        protected void btnCalcular_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            //imprimir

            ocultarDiv();
            if (Ln.InsertarImpresionPAF(objresumen.idEmpresa, int.Parse(ViewState["idp"].ToString()), objresumen.idUsuario))
                mensajeExito("Su solicitud ha sido enviada con éxito");
            else
                mensajeAlerta("Su solicitud no ha sido enviada. Intente nuevamente o comuníquese con el administrador del sitio.");
        }

        protected void ConsultarImpresionPAF()
        {
            asignacionResumen(ref objresumen);
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            //imprimir
            LogicaNegocio Ln = new LogicaNegocio();
            DataSet dt = Ln.ConsultarImpresionPAF(int.Parse(ViewState["idp"].ToString()));

            if (dt.Tables[0].Rows.Count > 0)
                lbFecImprimirPAF2.Text = "La Fecha en que se generó por última vez el documento PAF fue: " + dt.Tables[0].Rows[0][1].ToString();
            else
                lbFecImprimirPAF2.Text = "Por favor proceder a calcular la PAF 2";
        }

        private void ctrFecha_TextChanged(object sender, EventArgs e)
        {
            ctrFechaRevision.SelectedDate = DateTime.ParseExact(ctrFecha.SelectedDate.Day.ToString("00") + ctrFecha.SelectedDate.Month.ToString("00") + (String)(ctrFecha.SelectedDate.Year + 1).ToString(), "ddMMyyyy", CultureInfo.InvariantCulture);
        }

        private void ctrFecha_DateChanged(object sender, EventArgs e)
        {
            ctrFechaRevision.SelectedDate = DateTime.ParseExact(ctrFecha.SelectedDate.Day.ToString("00") + ctrFecha.SelectedDate.Month.ToString("00") + (String)(ctrFecha.SelectedDate.Year + 1).ToString(), "ddMMyyyy", CultureInfo.InvariantCulture);
        }

        #region GRILLA_GARANTIAS

        protected void EliminarGarantia_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                DataTable GarantiaTodas = (DataTable)ViewState["GarantiaTodas"];
                for (int i = 0; i < GarantiaTodas.Rows.Count; i++)
                {
                    if (e.CommandArgument.ToString() == GarantiaTodas.Rows[i]["IdGarantia"].ToString())
                    {
                        GarantiaTodas.Rows[i].Delete();
                        GarantiaTodas.AcceptChanges();
                        i = GarantiaTodas.Rows.Count;
                    }
                }

                Garantias.DataSource = GarantiaTodas;
                Garantias.DataBind();
            }
        }

        protected void Garantias_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
        }

        protected void Garantias_DataBound(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)ViewState["GarantiaTodas"];
            double TotalGarantiaComercial = 0;
            double TotalGarantiaAjustado = 0;
            double TotalCoberturaAjustadoVigente = 0;
            double TotalCoberturaComercialVigente = 0;
            double TotalCoberturaAjustadoNuevo = 0;
            double TotalGarantiaNuevaComercial = 0;
            try
            {
                for (int i = 0; i <= Garantias.Rows.Count - 1; i++)
                {
                    //var check = (Garantias.Rows[i].FindControl("chkSeguros") as CheckBoxList);

                    //if (check != null)
                    //{
                    //    var text1 = check.Items[0].Text;
                    //    var text2 = check.Items[1].Text;

                    //    check.Items[0].Attributes.Add("onChange", "return ValidarCheck('" + i + "');");
                    //    check.Items[1].Attributes.Add("onChange", "return ValidarCheck('" + i + "');"); 
                    //}

                    //var checkAplica = (Garantias.Rows[i].FindControl("chkAplica") as CheckBox);
                    //var checkNoAplica = (Garantias.Rows[i].FindControl("chkNoAplica") as CheckBox);

                    //if (checkAplica != null)
                    //{
                    //    checkAplica.Attributes.Add("onChange", "return ValidarCheck('" + i + "');");
                    //    checkNoAplica.Attributes.Add("onChange", "return ValidarCheck('" + i + "');");
                    //}

                    var checkSeguros = (Garantias.Rows[i].FindControl("RbtSeguros") as RadioButtonList);
                    if (checkSeguros != null)
                    {
                        var text1 = checkSeguros.Items[0].Text;
                        var text2 = checkSeguros.Items[1].Text;

                        checkSeguros.Items[0].Attributes.Add("onChange", "return ValidarCheck('" + i + "');");
                        checkSeguros.Items[1].Attributes.Add("onChange", "return ValidarCheck('" + i + "');");

                        if (!string.IsNullOrEmpty(dt1.Rows[i]["Seguro"].ToString()))
                        {
                            var chek = (Boolean)dt1.Rows[i]["Seguro"];
                            if (chek)
                                checkSeguros.Items[0].Selected = true;
                            else
                                checkSeguros.Items[1].Selected = true;
                        }
                    }

                    if ((Garantias.Rows[i].FindControl("txtComentario") as TextBox).Text == "")
                        (Garantias.Rows[i].FindControl("txtComentario") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Comentarios"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Comentarios"].ToString().Trim()) : "";
                    (Garantias.Rows[i].FindControl("lbTipoMoneda") as Label).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Tipo Moneda"].ToString());
                    (Garantias.Rows[i].FindControl("txtValorComercial") as Label).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Valor Comercial"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Valor Comercial"].ToString()) : "0";
                    (Garantias.Rows[i].FindControl("txtValorAjustado") as Label).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Valor Ajustado"].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i]["Valor Ajustado"].ToString()) : "0";

                    if (int.Parse(dt1.Rows[i]["IdTipoBien"].ToString()) != 52 || int.Parse(dt1.Rows[i]["IdTipoBien"].ToString()) != 53 || int.Parse(dt1.Rows[i]["IdTipoBien"].ToString()) != 54 || int.Parse(dt1.Rows[i]["IdTipoBien"].ToString()) != 59)
                    {
                        TotalGarantiaComercial = TotalGarantiaComercial + Double.Parse(dt1.Rows[i]["Valor Comercial"].ToString() != "" ? (dt1.Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        TotalGarantiaAjustado = TotalGarantiaAjustado + Double.Parse(dt1.Rows[i]["Valor Ajustado"].ToString() != "" ? (dt1.Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");

                        if (dt1.Rows[i]["DescEstado"].ToString() == "Constituida" || dt1.Rows[i]["DescEstado"].ToString() == "Tramite")
                        {
                            TotalCoberturaAjustadoVigente = TotalCoberturaAjustadoVigente + Double.Parse(dt1.Rows[i]["Valor Ajustado"].ToString() != "" ? (dt1.Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                            TotalCoberturaComercialVigente = TotalCoberturaComercialVigente + Double.Parse(dt1.Rows[i]["Valor Comercial"].ToString() != "" ? (dt1.Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        }
                        else
                        {
                            TotalCoberturaAjustadoNuevo = TotalCoberturaAjustadoNuevo + Double.Parse(dt1.Rows[i]["Valor Ajustado"].ToString() != "" ? (dt1.Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                            TotalGarantiaNuevaComercial = TotalGarantiaNuevaComercial + Double.Parse(dt1.Rows[i]["Valor Comercial"].ToString() != "" ? (dt1.Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        }
                    }

                    (Garantias.Rows[i].FindControl("GaranElim") as LinkButton).CommandArgument = dt1.Rows[i]["IdGarantia"].ToString();
                    (Garantias.Rows[i].FindControl("GaranElim") as LinkButton).Command += EliminarGarantia_Command;

                }

                (Garantias.FooterRow.FindControl("txtTotalGarantiaComercial") as TextBox).Text = util.formatearNumero(TotalGarantiaComercial * float.Parse(txtUF.Text.Trim()), 0);
                //(TotalGarantiaComercial * float.Parse(txtUF.Text)).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                (Garantias.FooterRow.FindControl("txtTotalGarantiaAjustado") as TextBox).Text = util.formatearNumero(TotalGarantiaAjustado * float.Parse(txtUF.Text.Trim()), 0);
                //(TotalGarantiaAjustado * float.Parse(txtUF.Text)).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                (Garantias.FooterRow.FindControl("txtTotalGarantiaComercialUF") as TextBox).Text = util.formatearNumero(TotalGarantiaComercial, 0);
                //TotalGarantiaComercial.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                (Garantias.FooterRow.FindControl("txtTotalGarantiaAjustadoUF") as TextBox).Text = util.formatearNumero(TotalGarantiaAjustado, 0);
                //TotalGarantiaAjustado.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));


                //////////////////// calculo procentaje operaciones nuevas /////////////////////////////////////////////////////////////////////
                if (TotalGarantiaComercial > 0 && txtTotalMontoPropuestoREUF.Text != "" && txtTotalMontoPropuestoREUF.Text != "0,000")
                {
                    double total = 0;

                    if (double.IsInfinity(((TotalCoberturaComercialVigente + TotalGarantiaNuevaComercial) * 100) / util.GetValorDouble(txtTotalMontoPropuestoREUF.Text.Trim())))
                        total = 0;
                    else if (double.IsNaN(((TotalCoberturaComercialVigente + TotalGarantiaNuevaComercial) * 100) / util.GetValorDouble(txtTotalMontoPropuestoREUF.Text.Trim())))
                        total = 0;
                    else
                        total = (((TotalCoberturaComercialVigente + TotalGarantiaNuevaComercial) * 100) / util.GetValorDouble(txtTotalMontoPropuestoREUF.Text.Trim()));


                    (Garantias.FooterRow.FindControl("txtTotalMCoberturaComercialGlobal") as TextBox).Text = util.formatearNumero(total, 4);
                    //total.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                    //if (double.IsNaN((TotalCoberturaComercialVigente + TotalGarantiaNuevaComercial)) || double.IsInfinity((TotalCoberturaComercialVigente + TotalGarantiaNuevaComercial)))
                    //    GarComercialVigenteNueva.Value = "0";
                    //else
                    //    GarComercialVigenteNueva.Value = (TotalCoberturaComercialVigente + TotalGarantiaNuevaComercial).ToString();
                }
                else
                {
                    (Garantias.FooterRow.FindControl("txtTotalMCoberturaComercialGlobal") as TextBox).Text = "0,000";
                    //GarComercialVigenteNueva.Value = "0";
                }

                if (TotalGarantiaAjustado > 0 && txtTotalMontoPropuestoREUF.Text != "" && txtTotalMontoPropuestoREUF.Text != "0,000")
                {
                    double total = 0;
                    if (double.IsInfinity(((TotalCoberturaAjustadoVigente + TotalCoberturaAjustadoNuevo) * 100) / util.GetValorDouble(txtTotalMontoPropuestoREUF.Text.Trim())))
                        total = 0;
                    else if (double.IsNaN(((TotalCoberturaAjustadoVigente + TotalCoberturaAjustadoNuevo) * 100) / util.GetValorDouble(txtTotalMontoPropuestoREUF.Text.Trim())))
                        total = 0;
                    else
                        total = (((TotalCoberturaAjustadoVigente + TotalCoberturaAjustadoNuevo) * 100) / util.GetValorDouble(txtTotalMontoPropuestoREUF.Text.Trim()));

                    (Garantias.FooterRow.FindControl("txtTotalMCoberturaAjustadoGlobal") as TextBox).Text = util.formatearNumero(total, 4);
                    //total.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    //if (double.IsNaN(TotalCoberturaAjustadoVigente + TotalCoberturaAjustadoNuevo) || double.IsInfinity(TotalCoberturaAjustadoVigente + TotalCoberturaAjustadoNuevo))
                    //    GarAjustadoVigenteNueva.Value = "0";
                    //else
                    //    GarAjustadoVigenteNueva.Value = (TotalCoberturaAjustadoVigente + TotalCoberturaAjustadoNuevo).ToString();
                }
                else
                {
                    (Garantias.FooterRow.FindControl("txtTotalMCoberturaAjustadoGlobal") as TextBox).Text = "0,000";
                    //GarAjustadoVigenteNueva.Value = "0";
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //////////////////// calculo procentaje operaciones vigentes /////////////////////////////////////////////////////////////////////
                if (TotalGarantiaAjustado > 0 && txtTotalMontoVigenteREUF.Text != "" && txtTotalMontoVigenteREUF.Text != "0,000")
                {
                    double total = 0;

                    if (double.IsInfinity((TotalGarantiaAjustado * 100) / util.GetValorDouble(txtTotalMontoVigenteREUF.Text.Trim())))
                        total = 0;
                    else if (double.IsNaN((TotalGarantiaAjustado * 100) / util.GetValorDouble(txtTotalMontoVigenteREUF.Text.Trim())))
                        total = 0;
                    else
                        total = ((TotalGarantiaAjustado * 100) / util.GetValorDouble(txtTotalMontoVigenteREUF.Text.Trim()));

                    (Garantias.FooterRow.FindControl("txtTotalCoberturaAjustadoVigente") as TextBox).Text = util.formatearNumero(total, 4);
                    //total.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                else
                {
                    (Garantias.FooterRow.FindControl("txtTotalCoberturaAjustadoVigente") as TextBox).Text = "0,000";
                    //GarAjustadoVigente.Value = "0";
                }

                if (TotalGarantiaComercial > 0 && txtTotalMontoVigenteREUF.Text != "" && txtTotalMontoVigenteREUF.Text != "0,00")
                {
                    double total = 0;

                    if (double.IsInfinity((TotalGarantiaComercial * 100) / util.GetValorDouble(txtTotalMontoVigenteREUF.Text.Trim())))
                        total = 0;
                    else if (double.IsNaN((TotalGarantiaComercial * 100) / util.GetValorDouble(txtTotalMontoVigenteREUF.Text.Trim())))
                        total = 0;
                    else
                        total = ((TotalGarantiaComercial * 100) / util.GetValorDouble(txtTotalMontoVigenteREUF.Text.Trim()));

                    (Garantias.FooterRow.FindControl("txtTotalCoberturaComercialVigente") as TextBox).Text = util.formatearNumero(total, 4);
                    //total.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                else
                {
                    (Garantias.FooterRow.FindControl("txtTotalCoberturaComercialVigente") as TextBox).Text = "0,000";
                    //GarComercialVigente.Value = "0";
                }
            }


            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            dt1 = null;
        }

        protected void btnGuardar_Click2(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();
            LN.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            guardar();
        }

        #endregion

        #region GRILLA_RESUMEN BALANCE

        protected void Generico1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "(en M$)";
                e.Row.Cells[0].Width = new Unit("15%");
                e.Row.Cells[1].Text = DateTime.Now.Year.ToString();
                e.Row.Cells[1].Width = new Unit("7%");
                e.Row.Cells[2].Text = "%";
                e.Row.Cells[2].Width = new Unit("3%");
                e.Row.Cells[3].Text = (DateTime.Now.Year - 1).ToString();
                e.Row.Cells[3].Width = new Unit("7%");
                e.Row.Cells[4].Text = "%";
                e.Row.Cells[4].Width = new Unit("3%");
                e.Row.Cells[5].Text = (DateTime.Now.Year - 2).ToString();
                e.Row.Cells[5].Width = new Unit("7%");
                e.Row.Cells[6].Text = "%";
                e.Row.Cells[6].Width = new Unit("3%");
                e.Row.Cells[7].Text = (DateTime.Now.Year - 3).ToString();
                e.Row.Cells[7].Width = new Unit("7%");
                e.Row.Cells[8].Text = "%";
                e.Row.Cells[8].Width = new Unit("3%");
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                if (ckSinDocumentos.Checked)
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                    e.Row.Cells[8].Text = "";
                }
                else
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                    if (e.Row.Cells[1].Text != "&nbsp;")
                        e.Row.Cells[1].Text = Convert.ToDouble(e.Row.Cells[1].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    if (e.Row.Cells[2].Text != "&nbsp;")
                        e.Row.Cells[2].Text = Convert.ToDouble(e.Row.Cells[2].Text).ToString("F");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    if (e.Row.Cells[3].Text != "&nbsp;")
                        e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    if (e.Row.Cells[4].Text != "&nbsp;")
                        e.Row.Cells[4].Text = Convert.ToDouble(e.Row.Cells[4].Text).ToString("F");
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    if (e.Row.Cells[5].Text != "&nbsp;")
                        e.Row.Cells[5].Text = Convert.ToDouble(e.Row.Cells[5].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    if (e.Row.Cells[6].Text != "&nbsp;")
                        e.Row.Cells[6].Text = Convert.ToDouble(e.Row.Cells[6].Text).ToString("F");
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    if (e.Row.Cells[7].Text != "&nbsp;")
                        e.Row.Cells[7].Text = Convert.ToDouble(e.Row.Cells[7].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    if (e.Row.Cells[8].Text != "&nbsp;")
                        e.Row.Cells[8].Text = Convert.ToDouble(e.Row.Cells[8].Text).ToString("F");
                }
            }
        }

        protected void Generico2_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Indicadores";
                e.Row.Cells[0].Width = new Unit("15%");
                e.Row.Cells[1].Text = DateTime.Now.Year.ToString();
                e.Row.Cells[1].Width = new Unit("7%");
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Text = (DateTime.Now.Year - 1).ToString();
                e.Row.Cells[2].Width = new Unit("7%");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].Text = (DateTime.Now.Year - 2).ToString();
                e.Row.Cells[3].Width = new Unit("7%");
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].Text = (DateTime.Now.Year - 3).ToString();
                e.Row.Cells[4].Width = new Unit("7%");
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                if (ckSinDocumentos.Checked)
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                }
                else
                {
                    if (e.Row.Cells[0].Text.Contains("Capital de Trabajo"))
                    {
                        if (e.Row.Cells[1].Text != "&nbsp;")
                            e.Row.Cells[1].Text = Convert.ToDouble(e.Row.Cells[1].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (e.Row.Cells[2].Text != "&nbsp;")
                            e.Row.Cells[2].Text = Convert.ToDouble(e.Row.Cells[2].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (e.Row.Cells[3].Text != "&nbsp;")
                            e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (e.Row.Cells[4].Text != "&nbsp;")
                            e.Row.Cells[4].Text = Convert.ToDouble(e.Row.Cells[4].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                    }
                    else
                    {
                        if (e.Row.Cells[1].Text != "&nbsp;")
                            e.Row.Cells[1].Text = Convert.ToDouble(e.Row.Cells[1].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (e.Row.Cells[2].Text != "&nbsp;")
                            e.Row.Cells[2].Text = Convert.ToDouble(e.Row.Cells[2].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (e.Row.Cells[3].Text != "&nbsp;")
                            e.Row.Cells[3].Text = Convert.ToDouble(e.Row.Cells[3].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (e.Row.Cells[4].Text != "&nbsp;")
                            e.Row.Cells[4].Text = Convert.ToDouble(e.Row.Cells[4].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                }
            }
        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                LogicaNegocio Ln = new LogicaNegocio();
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");

                if (ViewState["idp"] == null)
                    Page.Response.Redirect("MensajeSession.aspx");
                if (cbEstado.SelectedItem.Text != "--Select--")
                {
                    string ga = generarXMLGarantias();
                    string ov = generarXMLOperacionesVigentes();
                    string on = generarXMLOperacionesNuevas();

                    Dictionary<string, string> datos = new Dictionary<string, string>();
                    datos = AsignarDatos();
                    datos.Add("estado", "2");
                    //ocultarDiv();

                    if (Ln.GestionPAF(datos, ga, on, ov, objresumen.idUsuario, txtCapacidadPago.InnerText, txtEvaluacionRiesgo.InnerText))
                    {
                        guardarArchivo();
                        //buscar ids y valores de cambio de estado
                        Dictionary<string, string> estados = new Dictionary<string, string>();
                        if (cbEstado.SelectedItem.Text == "Aprobado")
                            estados = Ln.BuscarIdsYValoresCambioDeEstadoV2("9");
                        else if (cbEstado.SelectedItem.Text == "Rechazado")
                            estados = Ln.BuscarIdsYValoresCambioDeEstadoV2("10");
                        else
                            estados = Ln.BuscarIdsYValoresCambioDeEstadoV2("11");
                        DataTable OperacionesNuevas = (DataTable)ViewState["OperacionesNuevas"];
                        for (int j = 0; j < OperacionesNuevas.Rows.Count; j++)
                        {
                            datos.Clear();
                            datos.Add("IdOperacion", OperacionesNuevas.Rows[j]["IdOperacion"].ToString());
                            datos.Add("Plazo", OperacionesNuevas.Rows[j]["Plazo"].ToString());
                            datos.Add("ComisionP", OperacionesNuevas.Rows[j]["Comisión Min. %"].ToString().Replace(".", "").Replace(",", "."));
                            datos.Add("Seguro", OperacionesNuevas.Rows[j]["Seguro"].ToString().Replace(".", "").Replace(",", "."));
                            datos.Add("Comision", OperacionesNuevas.Rows[j]["Comisión"].ToString().Replace(".", "").Replace(",", "."));
                            datos.Add("MontoCredito", OperacionesNuevas.Rows[j]["Monto Crédito"].ToString().Replace(".", "").Replace(",", "."));
                            datos.Add("MontoPropuesto", OperacionesNuevas.Rows[j]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."));
                            datos.Add("IdPaf", ViewState["idp"].ToString());
                            datos.Add("Area", "Comercial");
                            Ln.ActualizarOperaciones(datos, estados);
                        }
                        datos.Clear();
                        datos.Add("IdPaf", ViewState["idp"].ToString());
                        datos.Add("IdSP1", cbUsuarios1.SelectedValue);
                        datos.Add("Nombre1", cbUsuarios1.SelectedItem.Text);
                        datos.Add("IdSP2", cbUsuarios2.SelectedValue);
                        datos.Add("Nombre2", cbUsuarios2.SelectedItem.Text);
                        datos.Add("IdSP3", cbUsuarios3.SelectedValue);
                        datos.Add("Nombre3", cbUsuarios3.SelectedItem.Text);
                        datos.Add("IdSP4", cbUsuarios4.SelectedValue);
                        datos.Add("Nombre4", cbUsuarios4.SelectedItem.Text);
                        datos.Add("IdSP5", cbUsuarios5.SelectedValue);
                        datos.Add("Nombre5", cbUsuarios5.SelectedItem.Text);
                        Ln.RegistrarAprobadoresPAF(datos, cbEstado.SelectedValue.ToString(), dtcFecResolucion.SelectedDate.ToString());

                        Page.Response.Redirect(objresumen.linkPrincial);
                    }
                    else
                    {
                        throw new Exception("Error al actualizar Gestion Paf");
                    }
                }
                else
                {
                    throw new Exception("Faltan Ingresar Datos");
                }
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");

            guardar();
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            //imprimir
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos["IdEmpresa"] = objresumen.idEmpresa.ToString();
            datos["IdPaf"] = ViewState["idp"].ToString();
            datos["Rank"] = lblRank.Text;
            datos["Clasi"] = lblClasif.Text;
            datos["Ventas"] = lblVentas.Text;//lblVentas.Text;

            string Reporte = "";
            Reporte = "Propuesta_Afianzamiento_Old";

            byte[] archivo = new Reportes { }.GenerarReporteOld(Reporte, datos, objresumen.descEjecutivo);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void lbBalance_Click(object sender, EventArgs e)
        {
            //BALANCE 
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoBalance.aspx");
        }

        protected void lbEdoResultado_Click(object sender, EventArgs e)
        {
            //ESTADO DE RESULTADO
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoEdoResultado.aspx");
        }

        protected void lbVentas_Click(object sender, EventArgs e)
        {
            //IVA VENTAS
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoVentas.aspx");
        }

        protected void lbCompras_Click(object sender, EventArgs e)
        {
            //IVA COMPRAS
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoCompras.aspx");
        }

        protected void lbScoring_Click(object sender, EventArgs e)
        {
            //IVA COMPRAS
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Scoring.aspx");
        }

        protected void lbResumenPAF_Click(object sender, EventArgs e)
        {
            //RESUMEN PAF
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ResumenPaf.aspx");
        }

        #endregion



        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        public void calculoLinea()
        {
            asignacionResumen(ref objresumen);
            DataTable OperacionesVigentes = (DataTable)ViewState["OperacionesVigentes"];
            DataTable OperacionesNuevas = (DataTable)ViewState["OperacionesNuevas"];
            string Moneda = string.Empty;
            double txtTotalMontoAprobadoCLPVigente = 0;
            double txtTotalMontoAprobadoUSDVigente = 0;
            double txtTotalMontoAprobadoUFVigente = 0;
            double txtTotalMontoVigenteCLPVigente = 0;
            double txtTotalMontoVigenteUSDVigente = 0;
            double txtTotalMontoVigenteUFVigente = 0;
            double txtTotalMontoPropuestoCLPVigente = 0;
            double txtTotalMontoPropuestoUSDVigente = 0;
            double txtTotalMontoPropuestoUFVigente = 0;

            double txtTotalMontoAprobadoCLPNuevas = 0;
            double txtTotalMontoAprobadoUSDNuevas = 0;
            double txtTotalMontoAprobadoUFNuevas = 0;
            double txtTotalMontoVigenteCLPNuevas = 0;
            double txtTotalMontoVigenteUSDNuevas = 0;
            double txtTotalMontoVigenteUFNuevas = 0;
            double txtTotalMontoPropuestoCLPNuevas = 0;
            double txtTotalMontoPropuestoUSDNuevas = 0;
            double txtTotalMontoPropuestoUFNuevas = 0;

            //double txtTotalMontoAprobadoREUFVigente = 0;


            for (int i = 0; i <= OperacionesVigentes.Rows.Count - 1; i++)
            {
                var coberturaCertificado = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(gridOperacionesVigentes.Rows[i].Cells[6].Text)) ? 0 : Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(gridOperacionesVigentes.Rows[i].Cells[6].Text)) / 100;
                Moneda = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("lbTipoMoneda") as Label).Text)) ? "" : System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("lbTipoMoneda") as Label).Text);

                if (Moneda == "CLP")
                {
                    txtTotalMontoAprobadoCLPVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text)) ? 0 : txtTotalMontoAprobadoCLPVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text));
                    txtTotalMontoVigenteCLPVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text)) ? 0 : txtTotalMontoVigenteCLPVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text));
                    txtTotalMontoPropuestoCLPVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text)) ? 0 : txtTotalMontoPropuestoCLPVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text));
                }

                if (Moneda == "USD")
                {
                    txtTotalMontoAprobadoUSDVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text)) ? 0 : txtTotalMontoAprobadoUSDVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text));
                    txtTotalMontoVigenteUSDVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text)) ? 0 : txtTotalMontoVigenteUSDVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text));
                    txtTotalMontoPropuestoUSDVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text)) ? 0 : txtTotalMontoPropuestoUSDVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text));
                }

                if (Moneda == "UF")
                {
                    txtTotalMontoAprobadoUFVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text)) ? 0 : txtTotalMontoAprobadoUFVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoAprobado") as Label).Text));
                    txtTotalMontoVigenteUFVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text)) ? 0 : txtTotalMontoVigenteUFVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoVigente") as Label).Text));
                    txtTotalMontoPropuestoUFVigente = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text)) ? 0 : txtTotalMontoPropuestoUFVigente + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text));
                }
            }

            for (int i = 0; i <= OperacionesNuevas.Rows.Count - 1; i++)
            {
                var coberturaCertificado = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtCobertura") as TextBox).Text)) ? 0 : Convert.ToDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtCobertura") as TextBox).Text)) / 100;
                Moneda = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("lbTipoMoneda") as Label).Text)) ? "" : System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("lbTipoMoneda") as Label).Text);

                if (Moneda == "CLP")
                {
                    txtTotalMontoAprobadoCLPNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text)) ? 0 : txtTotalMontoAprobadoCLPNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text));
                    txtTotalMontoVigenteCLPNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Text)) ? 0 : txtTotalMontoVigenteCLPNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Text));
                    txtTotalMontoPropuestoCLPNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text)) ? 0 : txtTotalMontoPropuestoCLPNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text));
                }

                if (Moneda == "USD")
                {
                    txtTotalMontoAprobadoUSDNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text)) ? 0 : txtTotalMontoAprobadoUSDNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text));
                    txtTotalMontoVigenteUSDNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Text)) ? 0 : txtTotalMontoVigenteUSDNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Text));
                    txtTotalMontoPropuestoUSDNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text)) ? 0 : txtTotalMontoPropuestoUSDNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text));
                }

                if (Moneda == "UF")
                {
                    txtTotalMontoAprobadoUFNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text)) ? 0 : txtTotalMontoAprobadoUFNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text));
                    txtTotalMontoVigenteUFNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text)) ? 0 : txtTotalMontoVigenteUFNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text));
                    txtTotalMontoPropuestoUFNuevas = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text)) ? 0 : txtTotalMontoPropuestoUFNuevas + coberturaCertificado * util.GetValorDouble(System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text));
                }
            }


            //TOTALES POR LINEA Monto Aprobado
            txtTotalMontoAprobadoCLP.Text = util.formatearNumero(txtTotalMontoAprobadoCLPVigente + txtTotalMontoAprobadoCLPNuevas, 0);
            txtTotalMontoAprobadoUDS.Text = util.formatearNumero(txtTotalMontoAprobadoUSDVigente + txtTotalMontoAprobadoUSDNuevas, 0);
            txtTotalMontoAprobadoUF.Text = util.formatearNumero(txtTotalMontoAprobadoUFVigente + txtTotalMontoAprobadoUFNuevas, 2);
            txtTotalMontoAprobadoREUF.Text = util.formatearNumero(((txtTotalMontoAprobadoCLPVigente + txtTotalMontoAprobadoCLPNuevas) + ((txtTotalMontoAprobadoUSDVigente + txtTotalMontoAprobadoUSDNuevas) * Convert.ToDouble(txtDOLAR.Text.Trim())) + ((txtTotalMontoAprobadoUFVigente + txtTotalMontoAprobadoUFNuevas) * Convert.ToDouble(txtUF.Text.Trim()))) / Convert.ToDouble(txtUF.Text.Trim()), 2);

            //TOTALES POR LINEA Monto Vigente
            txtTotalMontoVigenteCLP.Text = util.formatearNumero(txtTotalMontoVigenteCLPVigente + txtTotalMontoVigenteCLPNuevas, 0);
            txtTotalMontoVigenteUDS.Text = util.formatearNumero(txtTotalMontoVigenteUSDVigente + txtTotalMontoVigenteUSDNuevas, 0);
            txtTotalMontoVigenteUF.Text = util.formatearNumero(txtTotalMontoVigenteUFVigente + txtTotalMontoVigenteUFNuevas, 2);
            txtTotalMontoVigenteREUF.Text = util.formatearNumero(((txtTotalMontoVigenteCLPVigente + txtTotalMontoVigenteCLPNuevas) + ((txtTotalMontoVigenteUSDVigente + txtTotalMontoVigenteUSDNuevas) * Convert.ToDouble(txtDOLAR.Text.Trim())) + ((txtTotalMontoVigenteUFVigente + txtTotalMontoVigenteUFNuevas) * Convert.ToDouble(txtUF.Text.Trim()))) / Convert.ToDouble(txtUF.Text.Trim()), 2);

            //TOTALES POR LINEA Monto Propuesto
            txtTotalMontoPropuestoCLP.Text = util.formatearNumero(txtTotalMontoPropuestoCLPVigente + txtTotalMontoPropuestoCLPNuevas, 0);
            txtTotalMontoPropuestoUDS.Text = util.formatearNumero(txtTotalMontoPropuestoUSDVigente + txtTotalMontoPropuestoUSDNuevas, 2);
            txtTotalMontoPropuestoUF.Text = util.formatearNumero(txtTotalMontoPropuestoUFVigente + txtTotalMontoPropuestoUFNuevas, 2);
            txtTotalMontoPropuestoREUF.Text = util.formatearNumero(((txtTotalMontoPropuestoCLPVigente + txtTotalMontoPropuestoCLPNuevas) + ((txtTotalMontoPropuestoUSDVigente + txtTotalMontoPropuestoUSDNuevas) * Convert.ToDouble(txtDOLAR.Text.Trim())) + ((txtTotalMontoPropuestoUFVigente + txtTotalMontoPropuestoUFNuevas) * Convert.ToDouble(txtUF.Text.Trim()))) / Convert.ToDouble(txtUF.Text.Trim()), 2);

            //if (OperacionesVigentes.Rows.Count > 0)
            //{
            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoAprobadoCLP.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                       double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES")); //* float.Parse(txtUF.Text)).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoAprobadoCLP.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoAprobadoUDS.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                       double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoAprobadoUDS.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoAprobadoUF.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                      double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoAprobadoUF.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoAprobadoREUF.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAProbadoREUF") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                        double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoREUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoAprobadoREUF.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoAprobadoREUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoVigenteCLP.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                      double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoVigenteCLP.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoVigenteUDS.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                      double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoVigenteUDS.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoVigenteUF.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                     double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoVigenteUF.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoVigenteREUF.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                   double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoVigenteREUF.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoPropuestoCLP.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                        double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoPropuestoCLP.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoPropuestoUDS.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                        double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoPropuestoUDS.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoPropuestoUF.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text.Replace(".", "").Replace(",", ".")) +
            //                                       double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoPropuestoUF.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }

            //    if (gridOperacionesNuevas.Rows.Count > 0)
            //    {
            //        txtTotalMontoPropuestoREUF.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text.Replace(".", "").Replace(",", ".")) + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //    else
            //    {
            //        txtTotalMontoPropuestoREUF.Text = (0 + double.Parse((gridOperacionesVigentes.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text.Replace(".", "").Replace(",", "."))).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    }
            //}
            //else
            //{
            //    txtTotalMontoAprobadoCLP.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoCLP") as TextBox).Text.Replace(".", "").Replace(",", ".")) * float.Parse(txtUF.Text)).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoAprobadoUDS.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUSD") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoAprobadoUF.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoUF") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoAprobadoREUF.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoAprobadoREUF") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

            //    txtTotalMontoVigenteCLP.Text = (double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteCLP") as TextBox).Text.Replace(".", "").Replace(",", ".")) * float.Parse(txtUF.Text)).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoVigenteUDS.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUSD") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoVigenteUF.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteUF") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoVigenteREUF.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoVigenteREUF") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

            //    txtTotalMontoPropuestoCLP.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoCLP") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoPropuestoUDS.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUSD") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoPropuestoUF.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoUF") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //    txtTotalMontoPropuestoREUF.Text = double.Parse((gridOperacionesNuevas.FooterRow.FindControl("txtTotalMontoPropuestoREUF") as TextBox).Text.Replace(".", "").Replace(",", ".")).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //}
        }

        public void cargarEmpresa()
        {
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            DataSet res = new DataSet();
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.ConsutarDatosEmpresaPAF(objresumen.idEmpresa.ToString(), ViewState["idp"].ToString(), objresumen.idUsuario, objresumen.descCargo);
            if (res.Tables.Count > 0)
            {
                lblNombre.Text = res.Tables[0].Rows[0]["RazonSocial"].ToString();
                lblRut.Text = res.Tables[0].Rows[0]["Rut"].ToString() + "-" + res.Tables[0].Rows[0]["DivRut"].ToString();
                lblDireccion.Text = res.Tables[0].Rows[0]["direccion"].ToString();
                lblRegion.Text = res.Tables[0].Rows[0]["DescRegion"].ToString();
                lblComuna.Text = res.Tables[0].Rows[0]["DescComuna"].ToString();
                lblCiudad.Text = res.Tables[0].Rows[0]["DescProvincia"].ToString();
                lblGiro.Text = res.Tables[0].Rows[0]["GIROACT"].ToString();
                string aux = res.Tables[0].Rows[0]["GIROACT"].ToString();
                if (!string.IsNullOrEmpty(aux))
                    lblGiro.Text = aux.Substring(2, aux.Length - 2);
                else
                    lblGiro.Text = res.Tables[0].Rows[0]["GIROACT"].ToString();

                lblTelefono.Text = res.Tables[0].Rows[0]["TelFijo1"].ToString();
                lblEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();

                ctrFecha.SelectedDate = DateTime.ParseExact(DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString(), "ddMMyyyy", CultureInfo.InvariantCulture);
                ctrFechaRevision.SelectedDate = DateTime.ParseExact(DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + (String)(DateTime.Now.Year + 1).ToString(), "ddMMyyyy", CultureInfo.InvariantCulture);
                ViewState["OperacionesVigentes"] = res.Tables[2];
                gridOperacionesVigentes.DataSource = res.Tables[2];
                gridOperacionesVigentes.DataBind();

                ViewState["OperacionesNuevas"] = res.Tables[3];
                gridOperacionesNuevas.DataSource = res.Tables[3];
                gridOperacionesNuevas.DataBind();

                ViewState["GarantiaTodas"] = res.Tables[1];
                Garantias.DataSource = res.Tables[1];
                Garantias.DataBind();
            }
        }

        protected void inicializacionGrillas()
        {
            asignacionResumen(ref objresumen);
            try
            {
                DataTable dt1 = new DataTable();
                DataTable OperacionesVigentes = (DataTable)ViewState["OperacionesVigentes"];
                if (OperacionesVigentes != null)
                {
                    dt1 = OperacionesVigentes;
                    for (int i = 0; i <= gridOperacionesVigentes.Rows.Count - 1; i++)
                    {
                        dt1.Rows[i]["Monto Propuesto"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesVigentes.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text);
                    }
                    OperacionesVigentes = dt1;
                    gridOperacionesVigentes.DataSource = OperacionesVigentes;
                    gridOperacionesVigentes.DataBind();
                    dt1 = null;
                }
                dt1 = null;

                if (OperacionesVigentes != null)
                {
                    DataTable OperacionesNuevas = (DataTable)ViewState["OperacionesNuevas"];
                    dt1 = OperacionesNuevas;
                    for (int i = 0; i <= gridOperacionesNuevas.Rows.Count - 1; i++)
                    {
                        dt1.Rows[i]["Plazo"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtPlazo") as TextBox).Text);
                        dt1.Rows[i]["Horizonte"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtHorizonte") as TextBox).Text);
                        dt1.Rows[i]["CoberturaCertificado"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtCobertura") as TextBox).Text);
                        dt1.Rows[i]["Comisión Min. %"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtComision") as TextBox).Text);
                        dt1.Rows[i]["Seguro"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtSeguro") as TextBox).Text);
                        dt1.Rows[i]["Comisión"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtComisionCLP") as TextBox).Text);
                        dt1.Rows[i]["Monto Propuesto"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text);
                        dt1.Rows[i]["Monto Vigente"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoVigente") as TextBox).Text);
                        dt1.Rows[i]["Monto Aprobado"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoAprobado") as TextBox).Text);
                        dt1.Rows[i]["Monto Crédito"] = System.Web.HttpUtility.HtmlDecode((gridOperacionesNuevas.Rows[i].FindControl("txtMontoCredito") as TextBox).Text);
                    }
                    OperacionesNuevas = dt1;
                    gridOperacionesNuevas.DataSource = OperacionesNuevas;
                    gridOperacionesNuevas.DataBind();
                    dt1 = null;
                }

                dt1 = null;
                if (OperacionesVigentes != null)
                    calculoLinea();

                DataTable GarantiaTodas = (DataTable)ViewState["GarantiaTodas"];
                if (GarantiaTodas != null)
                {
                    dt1 = GarantiaTodas;
                    for (int i = 0; i <= GarantiaTodas.Rows.Count - 1; i++)
                    {
                        dt1.Rows[i]["Comentarios"] = System.Web.HttpUtility.HtmlDecode((Garantias.Rows[i].FindControl("txtComentario") as TextBox).Text);

                        var chkSeguro = (Garantias.Rows[i].FindControl("RbtSeguros") as RadioButtonList);
                        if (chkSeguro != null)
                        {
                            foreach (ListItem li in chkSeguro.Items)
                            {
                                if (li.Value == "Aplica" && li.Selected)
                                    dt1.Rows[i]["Seguro"] = li.Selected.ToString();
                                else
                                    dt1.Rows[i]["Seguro"] = "false";

                                break;
                            }
                        }
                    }
                    GarantiaTodas = dt1;
                    Garantias.DataSource = GarantiaTodas;
                    Garantias.DataBind();
                    dt1 = null;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public void cargaDatosPAF()
        {
            asignacionResumen(ref objresumen);
            DataSet res = new DataSet();
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.ConsutarDatosPAF(objresumen.idEmpresa.ToString(), objresumen.idUsuario, objresumen.descCargo, objresumen.idOperacion.ToString());

            if (res.Tables[0].Rows.Count > 0)
            {
                //valor uf y dolar del dia en que se aprobo la paf
                if (string.IsNullOrEmpty(res.Tables[0].Rows[0]["Fecha"].ToString()))
                {
                    try
                    {
                        DataTable dtValorMoneda = new DataTable("dtValorMoneda");
                        dtValorMoneda = Ln.GestionValorMoneda(DateTime.Now, 0, null, null, null, 1);

                        txtDOLAR.Text = dtValorMoneda.Rows[0]["MontoUSD"].ToString();
                        txtUF.Text = dtValorMoneda.Rows[0]["MontoUF"].ToString();

                        txtDOLARV.Text = util.formatearNumero(Convert.ToDouble(dtValorMoneda.Rows[0]["MontoUSD"].ToString()), 3);
                        //ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                        txtUFV.Text = util.formatearNumero(Convert.ToDouble(dtValorMoneda.Rows[0]["MontoUF"].ToString()), 3);
                        //Convert.ToDouble(txtUF.Text).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES")); ;
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    }
                }
                else
                {
                    txtDOLAR.Text = Convert.ToDouble(res.Tables[0].Rows[0]["ValorDolar"]).ToString();
                    txtUF.Text = Convert.ToDouble(res.Tables[0].Rows[0]["ValorUF"]).ToString();
                    txtDOLARV.Text = Convert.ToDouble(res.Tables[0].Rows[0]["ValorDolar"]).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                    txtUFV.Text = Convert.ToDouble(res.Tables[0].Rows[0]["ValorUF"]).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                lblPaf.Text = res.Tables[0].Rows[0]["IdPaf"].ToString();
                ViewState["idp"] = Convert.ToInt32(res.Tables[0].Rows[0]["IdPaf"].ToString());

                if (res.Tables[0].Rows[0]["EstadoLinea"].ToString() == "1")
                    rbNormal.Checked = true;
                else
                    rbRevision.Checked = true;

                if (res.Tables[0].Rows[0]["NivelAtribucion"].ToString() == "1")
                    rbRiesgo.Checked = true;
                else
                    rbEjecucion.Checked = true;

                txtObservacionesComite.Value = HttpUtility.HtmlDecode(res.Tables[0].Rows[0]["ObservacionComite"].ToString());

                txtCapacidadPago.InnerText = res.Tables[0].Rows[0]["CapacidadPago"].ToString();
                txtEvaluacionRiesgo.InnerText = res.Tables[0].Rows[0]["EvaluacionRiesgo"].ToString();

                //--------------------------------------------SCORING VENTAS

                if (res.Tables[1].Rows[0]["ValorRank"].ToString().Contains("/"))
                    lblRank.Text = res.Tables[1].Rows[0]["ValorRank"].ToString().Substring(0, res.Tables[1].Rows[0]["ValorRank"].ToString().IndexOf("/"));
                else
                    lblRank.Text = res.Tables[1].Rows[0]["ValorRank"].ToString();

                if (!string.IsNullOrEmpty(lblRank.Text))
                    lblClasif.Text = res.Tables[1].Rows[0]["clasificacion"].ToString();

                ViewState["IdScorign"] = int.Parse(res.Tables[1].Rows[0]["IdScoring"].ToString());
                lblProf.Text = (Convert.ToDouble(res.Tables[1].Rows[0]["ValorPond"])).ToString("N", CultureInfo.CreateSpecificCulture("es-ES")) + "%";
                lblFClasif.Text = res.Tables[1].Rows[0]["FecCreacion"].ToString().ToString();
                ckSinDocumentos.Checked = (Boolean)res.Tables[0].Rows[0]["SinDocumentosContables"];
                try
                {
                    double AUX = 0;
                    AUX = (double.Parse(res.Tables[1].Rows[0]["Ventas"].ToString()) / double.Parse(txtUF.Text));
                    lblVentas.Text = AUX.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    lblVentas.Text = "0";
                }

                if (res.Tables[2].Rows.Count > 0)
                {
                    try
                    {
                        cbUsuarios1.SelectedIndex = cbUsuarios1.Items.IndexOf(cbUsuarios1.Items.FindByText(Convert.ToString(res.Tables[2].Rows[0][3].ToString())));
                        cbUsuarios2.SelectedIndex = cbUsuarios2.Items.IndexOf(cbUsuarios2.Items.FindByText(Convert.ToString(res.Tables[2].Rows[0][5].ToString())));
                        cbUsuarios3.SelectedIndex = cbUsuarios3.Items.IndexOf(cbUsuarios3.Items.FindByText(Convert.ToString(res.Tables[2].Rows[0][7].ToString())));
                        cbUsuarios4.SelectedIndex = cbUsuarios4.Items.IndexOf(cbUsuarios4.Items.FindByText(Convert.ToString(res.Tables[2].Rows[0][9].ToString())));
                        cbUsuarios5.SelectedIndex = cbUsuarios5.Items.IndexOf(cbUsuarios5.Items.FindByText(Convert.ToString(res.Tables[2].Rows[0][11].ToString())));
                    }
                    catch (Exception ex1)
                    {
                        LoggingError.PostEventRegister(ex1, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    }
                }

                if (res.Tables[3].Rows.Count > 0)
                {
                    Primero.DataSource = res.Tables[3];
                    Primero.DataBind();
                    Segundo.DataSource = res.Tables[4];
                    Segundo.DataBind();
                    Tercero.DataSource = res.Tables[5];
                    Tercero.DataBind();
                    Cuarto.DataSource = res.Tables[6];
                    Cuarto.DataBind();
                    Quinto.DataSource = res.Tables[7];
                    Quinto.DataBind();
                    Sexto.DataSource = res.Tables[8];
                    Sexto.DataBind();
                }
                else
                {
                    dvWarning.Visible = true;
                    btnGuardar.Visible = false;
                    btnImprimir.Visible = false;
                    btnImprimirPAF2.Visible = false;
                    btnCalcular.Visible = false;

                    mensajeAlerta(lbWarning.Text + Ln.buscarMensaje(26) + " Balance incompleto.");
                }
            }
            else
            {
                mensajeAlerta(lbWarning.Text + Ln.buscarMensaje(26) + " Balance, Estado Resultado, IVA Compras, IVA Ventas ó Scoring Incompleto(s).");
            }
        }

        public void cargarListas()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtUsuariosPaf = Ln.ListarAprobadores(1,0);
            util.CargaDDL(cbUsuarios1, dtUsuariosPaf, "Usuario", "Id");
            util.CargaDDL(cbUsuarios2, dtUsuariosPaf, "Usuario", "Id");
            util.CargaDDL(cbUsuarios3, dtUsuariosPaf, "Usuario", "Id");
            util.CargaDDL(cbUsuarios4, dtUsuariosPaf, "Usuario", "Id");
            util.CargaDDL(cbUsuarios5, dtUsuariosPaf, "Usuario", "Id");
            ////Ln.ListarUsuariosPaf(ref cbUsuarios1);
            ////Ln.ListarUsuariosPaf(ref cbUsuarios2);
            ////Ln.ListarUsuariosPaf(ref cbUsuarios3);
            ////Ln.ListarUsuariosPaf(ref cbUsuarios4);
            ////Ln.ListarUsuariosPaf(ref cbUsuarios5);
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void mensajeError(string mensaje)
        {
            dvError.Style.Add("display", "block");
            lbError.Text = mensaje;
        }

        private void mensajeAlerta(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje;
        }

        private void mensajeExito(string mensaje)
        {
            dvSuccess.Style.Add("display", "block");
            lbSuccess.Text = mensaje;
        }

        public void guardarArchivo()
        {
            String carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            bool cargaCorrecta = new Documentos { }.cargarDocumento(carpetaEmpresa, objresumen.area.ToUpper(), objresumen.idOperacion.ToString(), objresumen, fileDocumento, "PAF", objresumen.idOperacion.ToString(), "Documento adjunto PAF");
        }

        private string generarXMLGarantias()
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            DataTable GarantiaTodas = (DataTable)ViewState["GarantiaTodas"];
            if (GarantiaTodas != null)
            {
                for (int i = 0; i < GarantiaTodas.Rows.Count; i++)
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode nodo = doc.CreateElement("IdGarantias");
                    nodo.AppendChild(doc.CreateTextNode(GarantiaTodas.Rows[i]["IdGarantia"].ToString()));
                    RespNode.AppendChild(nodo);

                    XmlNode nodo1 = doc.CreateElement("Comentarios");
                    nodo1.AppendChild(doc.CreateTextNode(GarantiaTodas.Rows[i]["Comentarios"].ToString().Trim()));
                    RespNode.AppendChild(nodo1);

                    XmlNode nodo2 = doc.CreateElement("ValorComercial");
                    nodo2.AppendChild(doc.CreateTextNode(GarantiaTodas.Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo2);

                    XmlNode nodo3 = doc.CreateElement("ValorAjustado");
                    nodo3.AppendChild(doc.CreateTextNode(GarantiaTodas.Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo3);

                    var aplicaCheck = (Garantias.Rows[i].FindControl("RbtSeguros") as RadioButtonList);
                    if (aplicaCheck != null)
                    {
                        var aplica = aplicaCheck.Items[0].Selected ? true : false;
                        XmlNode nodo4 = doc.CreateElement("RequiereSeguro");
                        nodo4.AppendChild(doc.CreateTextNode(aplica.ToString()));
                        RespNode.AppendChild(nodo4);
                    }

                    ValoresNode.AppendChild(RespNode);
                }
                return doc.OuterXml;
            }
            else
                return doc.OuterXml;
        }

        private string generarXMLOperacionesVigentes()
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;

            XmlNode root = doc.DocumentElement;

            DataTable OperacionesVigentes = (DataTable)ViewState["OperacionesVigentes"];
            for (int i = 0; i < OperacionesVigentes.Rows.Count; i++)
            {
                RespNode = doc.CreateElement("Val");
                XmlNode nodo = doc.CreateElement("IdOperacion");
                nodo.AppendChild(doc.CreateTextNode(OperacionesVigentes.Rows[i]["IdOperacion"].ToString()));
                RespNode.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("MontoPropuesto");
                nodo1.AppendChild(doc.CreateTextNode(OperacionesVigentes.Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo1);
                ValoresNode.AppendChild(RespNode);

                XmlNode nodo2 = doc.CreateElement("MontoAprobado");
                nodo2.AppendChild(doc.CreateTextNode(OperacionesVigentes.Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo2);

                XmlNode nodo3 = doc.CreateElement("MontoVigente");
                nodo3.AppendChild(doc.CreateTextNode(OperacionesVigentes.Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo3);

                XmlNode nodo5 = doc.CreateElement("TipoMoneda");
                nodo5.AppendChild(doc.CreateTextNode(OperacionesVigentes.Rows[i]["Tipo Moneda"].ToString()));
                RespNode.AppendChild(nodo5);

                XmlNode nodo4 = doc.CreateElement("Refinanciamiento");
                nodo4.AppendChild(doc.CreateTextNode("0"));
                RespNode.AppendChild(nodo3);

                ValoresNode.AppendChild(RespNode);
            }
            return doc.OuterXml;
        }

        private string generarXMLOperacionesNuevas()
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;

            XmlNode root = doc.DocumentElement;

            DataTable OperacionesNuevas = (DataTable)ViewState["OperacionesNuevas"];
            for (int i = 0; i < OperacionesNuevas.Rows.Count; i++)
            {
                RespNode = doc.CreateElement("Val");

                XmlNode nodo = doc.CreateElement("IdOperacion");
                nodo.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["IdOperacion"].ToString()));
                RespNode.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("MontoCredito");
                nodo1.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Monto Crédito"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo1);

                XmlNode nodo2 = doc.CreateElement("MontoAprobado");
                nodo2.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo2);

                XmlNode nodo3 = doc.CreateElement("MontoVigente");
                nodo3.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo3);

                XmlNode nodo4 = doc.CreateElement("MontoPropuesto");
                nodo4.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo4);

                XmlNode nodo5 = doc.CreateElement("Plazo");
                nodo5.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Plazo"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo5);

                XmlNode nodo6 = doc.CreateElement("Comision");
                nodo6.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Comisión Min. %"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo6);

                XmlNode nodo7 = doc.CreateElement("Seguro");
                nodo7.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Seguro"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo7);

                XmlNode nodo8 = doc.CreateElement("ComisionCLP");
                nodo8.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Comisión"].ToString().Replace(".", "").Replace(",", ".")));
                RespNode.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("TipoMoneda");
                nodo9.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["Tipo Moneda"].ToString()));
                RespNode.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("CoberturaCertificado");
                nodo10.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["CoberturaCertificado"].ToString()));
                RespNode.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("horizonte");
                nodo11.AppendChild(doc.CreateTextNode(OperacionesNuevas.Rows[i]["horizonte"].ToString()));
                RespNode.AppendChild(nodo11);

                ValoresNode.AppendChild(RespNode);
            }

            return doc.OuterXml;
        }

        Dictionary<string, string> AsignarDatos()
        {
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos.Add("Oficina", "1");
            datos.Add("Fecha", ctrFecha.SelectedDate.ToString());
            datos.Add("FechaRevision", ctrFechaRevision.SelectedDate.ToString());
            datos.Add("Ejecutivo", lblEjecutivo.Text);
            datos.Add("EstadoLinea", rbRevision.Checked ? "2" : rbNormal.Checked ? "1" : "0");
            datos.Add("NivelAtribucion", rbRiesgo.Checked ? "1" : rbEjecucion.Checked ? "2" : "0");
            if (ViewState["IdScorign"] == null)
                throw new Exception("es necesario completar el scoring");

            datos.Add("IdScoring", ViewState["IdScorign"].ToString());
            datos.Add("IdEmpresa", objresumen.idEmpresa.ToString());
            datos.Add("ObservacionPropuesta", "ob1");//txtComentariosPropuesta.Value);
            datos.Add("ObservacionesComite", txtObservacionesComite.Value);
            datos.Add("IdPaf", ViewState["idp"].ToString());
            datos.Add("TotalAprobado", txtTotalMontoAprobadoREUF.Text.Replace(".", "").Replace(",", "."));
            datos.Add("TotalVigente", txtTotalMontoVigenteREUF.Text.Replace(".", "").Replace(",", "."));
            datos.Add("TotalPropuesto", txtTotalMontoPropuestoREUF.Text.Replace(".", "").Replace(",", "."));
            datos.Add("ValorDolar", txtDOLAR.Text);
            datos.Add("ValorUF", txtUF.Text);
            datos.Add("VentasMoviles", lblVentas.Text.Replace(".", "").Replace(",", "."));
            datos.Add("CoberturaVigenteComercial", (Garantias.FooterRow.FindControl("txtTotalCoberturaComercialVigente") as TextBox).Text.Trim().Replace(".", "").Replace(",", "."));
            datos.Add("CoberturaVigenteAjustado", (Garantias.FooterRow.FindControl("txtTotalCoberturaAjustadoVigente") as TextBox).Text.Trim().Replace(".", "").Replace(",", "."));
            datos.Add("CoberturaGlobalComercial", (Garantias.FooterRow.FindControl("txtTotalMCoberturaComercialGlobal") as TextBox).Text.Trim().Replace(".", "").Replace(",", "."));
            datos.Add("CoberturaGlobalAjustado", (Garantias.FooterRow.FindControl("txtTotalMCoberturaAjustadoGlobal") as TextBox).Text.Trim().Replace(".", "").Replace(",", "."));

            //datos.Add("CoberturaVigenteComercial", util.Reemplazar((Garantias.FooterRow.FindControl("txtTotalCoberturaComercialVigente") as TextBox).Text.Trim()));
            //datos.Add("CoberturaVigenteAjustado", util.Reemplazar((Garantias.FooterRow.FindControl("txtTotalCoberturaAjustadoVigente") as TextBox).Text.Trim()));
            //datos.Add("CoberturaGlobalComercial", util.Reemplazar((Garantias.FooterRow.FindControl("txtTotalMCoberturaComercialGlobal") as TextBox).Text.Trim()));
            //datos.Add("CoberturaGlobalAjustado", util.Reemplazar((Garantias.FooterRow.FindControl("txtTotalMCoberturaAjustadoGlobal") as TextBox).Text.Trim()));
            return datos;
        }

        public void guardar()
        {
            try
            {
                if (ValidarMontoPropuesto())
                {
                    if (ValidarCheckSeguros())
                    {
                        asignacionResumen(ref objresumen);
                        if (ViewState["idp"] == null)
                            Page.Response.Redirect("MensajeSession.aspx");

                        if (txtTotalMontoPropuestoREUF.Text == "0,00" || txtTotalMontoPropuestoREUF.Text == "" || txtTotalMontoPropuestoREUF.Text == "0")
                        {
                            mensajeAlerta("Total Riesgo equivalente Propuestos debe ser mayor a cero(0).");
                        }

                        string ga = generarXMLGarantias();
                        string ov = generarXMLOperacionesVigentes();
                        string on = generarXMLOperacionesNuevas();

                        LogicaNegocio Ln = new LogicaNegocio();
                        Dictionary<string, string> datos = new Dictionary<string, string>();
                        datos = AsignarDatos();
                        datos.Add("estado", "1");

                        if (Ln.GestionPAF(datos, ga, on, ov, objresumen.idUsuario, txtCapacidadPago.InnerText, txtEvaluacionRiesgo.InnerText))
                        {
                            datos.Clear();
                            datos.Add("IdPaf", ViewState["idp"].ToString());
                            datos.Add("IdSP1", cbUsuarios1.SelectedValue);
                            datos.Add("Nombre1", cbUsuarios1.SelectedItem.Text);
                            datos.Add("IdSP2", cbUsuarios2.SelectedValue);
                            datos.Add("Nombre2", cbUsuarios2.SelectedItem.Text);
                            datos.Add("IdSP3", cbUsuarios3.SelectedValue);
                            datos.Add("Nombre3", cbUsuarios3.SelectedItem.Text);
                            datos.Add("IdSP4", cbUsuarios4.SelectedValue);
                            datos.Add("Nombre4", cbUsuarios4.SelectedItem.Text);
                            datos.Add("IdSP5", cbUsuarios5.SelectedValue);
                            datos.Add("Nombre5", cbUsuarios5.SelectedItem.Text);
                            Ln.RegistrarAprobadoresPAF(datos, cbEstado.SelectedValue.ToString(), dtcFecResolucion.SelectedDate.ToString());
                            mensajeExito(Ln.buscarMensaje(4));
                        }
                        else
                        {
                            mensajeAlerta(Ln.buscarMensaje(5));
                        }
                    }
                    else
                        mensajeAlerta("Se debe seleccionar en todas las garantias 'si aplica / no aplica' seguro.");
                }
                else
                    mensajeAlerta("Total Riesgo equivalente Propuestos debe ser mayor a cero(0).");
            }
            catch (Exception ex)
            {
                ocultarDiv();
                mensajeAlerta(ex.Message.Trim());
            }

        }

        private bool ValidarMontoPropuesto()
        {
            for (int i = 0; i <= gridOperacionesNuevas.Rows.Count - 1; i++)
            {
                double ValorPropuesto = 0;
                var montoPropuesto = (gridOperacionesNuevas.Rows[i].FindControl("txtMontoPropuesto") as TextBox).Text;
                if (montoPropuesto != null)
                {
                    ValorPropuesto = util.GetValorDouble(montoPropuesto.ToString());
                    if (ValorPropuesto > 0)
                        continue;
                    else
                        return false;
                }
            }

            return true;
        }

        private bool ValidarCheckSeguros()
        {
            int Filasvalidar = 0;
            for (int i = 0; i <= Garantias.Rows.Count - 1; i++)
            {
                var chkSeguro = (Garantias.Rows[i].FindControl("RbtSeguros") as RadioButtonList);
                if (chkSeguro != null)
                {
                    foreach (ListItem li in chkSeguro.Items)
                    {
                        if (li.Selected)
                        {
                            Filasvalidar++;
                            break;
                        }
                    }
                }
            }

            if (Filasvalidar == Garantias.Rows.Count)
                return true;
            else
                return false;


            //for (int i = 0; i <= Garantias.Rows.Count - 1; i++)
            //{
            //    bool ValorPropuesto = false;
            //    var chkSeguro = (Garantias.Rows[i].FindControl("chkSeguros") as CheckBoxList);
            //    if (chkSeguro != null)
            //    {
            //        ValorPropuesto = chkSeguro.Items[i].Selected;
            //        if (ValorPropuesto)
            //            continue;
            //        else
            //            return false;
            //    }
            //}
        }

        #endregion

        protected void btnImprimirPAF2Express_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");

            guardar();
            if (ViewState["idp"] == null)
                Page.Response.Redirect("MensajeSession.aspx");
            //imprimir
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos["IdEmpresa"] = objresumen.idEmpresa.ToString();
            datos["IdPaf"] = ViewState["idp"].ToString();
            datos["Rank"] = lblRank.Text;
            datos["Clasi"] = lblClasif.Text;
            datos["Ventas"] = lblVentas.Text;

            string Reporte = "";
            Reporte = "Propuesta_Afianzamiento_Express";

            byte[] archivo = new Reportes { }.GenerarReporteOld(Reporte, datos, objresumen.descEjecutivo);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
            else
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = "Error al generar archivo paf";
            }

        }

    }
}
