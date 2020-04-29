using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Linq;
using System.IO;
using System.Text;
using System.Globalization;
using TridentGoalSeek;
using Microsoft.Office.Tools;
using ClasesNegocio;
using Bd;

namespace MultiComercial.wpDatosOperacion.wpDatosOperacion
{
    [ToolboxItemAttribute(false)]
    public partial class wpDatosOperacion : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "DatosOperacion.aspx";
        private static string[] Cargos = { "Administrador", "Jefe Operaciones", "Analista Operaciones" };

        public wpDatosOperacion()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            //PERMISOS USUARIOS
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
                        Page.Session["RESUMEN"] = null;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbOperacion.Text = objresumen.desOperacion.ToString();
                        lblIdOperacion.Text = objresumen.idOperacion.ToString();

                        ocultarDiv();
                        validarFechas();
                        cargarDatosOperacion();
                        bloquearTextAreas();

                        if (objresumen.area == "Riesgo")
                        {
                            HdfEtapa.Value = "Riesgo";
                            pnComercial.Visible = true;
                            pnRiesgo.Visible = pnRiesgo.Enabled = true;
                            txtDestinoSolicitud.Disabled = false;
                            pnOperaciones.Visible = false;
                            pnPagare.Visible = false;
                            li_comercial.Visible = li_riesgo.Visible = true;
                            li_operacion.Visible = li_pagare.Visible = false;
                        }

                        if (objresumen.area == "Fiscalia")
                        {
                            HdfEtapa.Value = "Fiscalia";
                            pnComercial.Visible = true;
                            pnRiesgo.Visible = true;
                            pnOperaciones.Visible = false;
                            pnPagare.Visible = false;
                            li_comercial.Visible = li_riesgo.Visible = true;
                            li_operacion.Visible = li_pagare.Visible = false;
                        }

                        if (objresumen.area == "Comercial")
                        {
                            HdfEtapa.Value = "Comercial";
                            if (objresumen.desEtapa == "Fiscalia" || objresumen.desEtapa == "Fiscalía")
                            {
                                pnRiesgo.Visible = true;
                                pnComercial.Visible = pnComercial.Enabled = true;
                                activarComercial();
                                pnOperaciones.Visible = false;
                                pnPagare.Visible = false;
                                li_comercial.Visible = li_riesgo.Visible = true;
                                li_operacion.Visible = li_pagare.Visible = false;
                            }
                            else
                            {
                                pnComercial.Visible = pnComercial.Enabled = true;
                                activarComercial();
                                pnOperaciones.Visible = false;
                                pnPagare.Visible = false;
                                li_comercial.Visible = true;
                                li_operacion.Visible = li_pagare.Visible = false;
                            }
                        }

                        if (objresumen.area == "Operaciones")
                        {
                            pnComercial.Enabled = pnComercial.Visible = true;
                            pnRiesgo.Enabled = pnRiesgo.Visible = true;
                            pnPagare.Enabled = pnPagare.Visible = true;
                            pnOperaciones.Enabled = pnOperaciones.Visible = true;
                            txtDestinoSolicitud.Disabled = false;
                            txtComentarios.Disabled = false;
                            li_comercial.Visible = li_riesgo.Visible = true;
                            li_operacion.Visible = li_pagare.Visible = true;
                            activarComercial();
                        }
                    }
                    else
                        Page.Response.Redirect("MensajeSession.aspx");

                    if (objresumen.linkActual != "PosicionCliente.aspx")
                    {
                        //VerificarEdicionSimultanea
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Operacion", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            warning("Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.");
                            btnGuardar.Style.Add("display", "none");
                            btnLimpiar.Style.Add("display", "none");
                            btnCancelar.Style.Add("display", "none");
                            pnComercial.Enabled = pnRiesgo.Enabled = pnPagare.Enabled = pnOperaciones.Enabled = false;
                            bloquearTextAreas();
                        }
                    }
                    else
                    {
                        pnComercial.Visible = true;
                        pnRiesgo.Visible = true;
                        pnPagare.Visible = true;
                        pnOperaciones.Visible = true;
                        li_comercial.Visible = li_riesgo.Visible = true;
                        li_operacion.Visible = li_pagare.Visible = true;
                    }
                }

                AsignacionesJS();
                validacionTimbreEstampilla();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();

                Control cc = this.FindControl("dvFormulario");

                if (cc != null)
                {
                    //if (objresumen.linkPrincial == "ListarSeguimiento.aspx" && util.CargosPermitidos(dt.Rows[0]["descCargo"].ToString(), Cargos))
                    //{
                    //    pnComercial.Enabled = pnComercial.Visible = true;
                    //    pnRiesgo.Enabled = pnRiesgo.Visible = true;
                    //    pnPagare.Enabled = pnPagare.Visible = true;
                    //    pnOperaciones.Enabled = pnOperaciones.Visible = true;
                    //    txtDestinoSolicitud.Disabled = false;
                    //    txtComentarios.Disabled = false;
                    //    li_comercial.Visible = li_riesgo.Visible = true;
                    //    li_operacion.Visible = li_pagare.Visible = true;
                    //    activarComercial();
                    //}
                    //else
                    //{
                    Control divFormulario = this.FindControl("dvFormulario");
                    bool TieneFiltro = true;

                    if (divFormulario != null)
                        util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                    //}
                }
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    dvWarning1.Style.Add("display", "block");
                    lbWarning1.Text = "Usuario sin permisos configurados";
                }

                HdfEtapa.Value = objresumen.desEtapa;
                HdfSubEtapa.Value = objresumen.desSubEtapa;
                HdfArea.Value = objresumen.area;

            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }

            if (hdftabb.Value == "panelComercial")
                li_comercial.Attributes.Add("class", "active");
            if (hdftabb.Value == "panelRiesgo")
            {
                li_comercial.Attributes.Add("class", "");
                li_operacion.Attributes.Add("class", "");
                li_pagare.Attributes.Add("class", "");
                li_riesgo.Attributes.Add("class", "active");
            }

            if (dt.Rows.Count > 0)
                bloquearCampos(dt.Rows[0]["descCargo"].ToString());
        }

        private void warning(string mensaje)
        {
            lbWarning.Text = util.Mensaje(mensaje);
            dvWarning.Style.Add("display", "block");
        }

        private void error(string mensaje)
        {
            lbError.Text = util.Mensaje(mensaje);
            dvError.Style.Add("display", "block");
        }

        private void succes(string mensaje)
        {
            lbSuccess.Text = util.Mensaje(mensaje);
            dvSuccess.Style.Add("display", "block");
        }

        protected void lbGarantias_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Operacion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("MantenedorGarantias.aspx");
        }

        protected void lbRegistroOperacion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Operacion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosOperacion.aspx");
        }

        protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Limpio valores anteriores
            ValidarProductos();
            CargarAcreedores(ddlProducto.SelectedItem.Text.Trim());
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Operacion");
                Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                Page.Response.Redirect(objresumen.linkPrincial);
            }
            else
            {
                Page.Session["Resumen"] = objresumen;
                Page.Response.Redirect(objresumen.linkActual);
            }
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Operacion");
                Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                Page.Response.Redirect(objresumen.linkPrincial);
            }
            else
            {
                Page.Session["Resumen"] = objresumen;
                Page.Response.Redirect(objresumen.linkActual);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlFondo_SelectedIndexChanged(object sender, EventArgs e)
        {
            validarFondo(true);
            ddlFondo.Enabled = true;
            ddlFogapeVigente.Enabled = true;
            hdftabb.Value = "panelRiesgo";
        }

        protected void txtMontoComision_TextChanged(object sender, EventArgs e)
        {
            validarFondo(false);
            ddlFondo.Enabled = true;
            ddlFogapeVigente.Enabled = true;
            hdftabb.Value = "panelComercial";
        }

        protected void ddlAcreedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            String nombreAcreedor = String.Empty;
            DropDownList ddl = (DropDownList)sender;
            nombreAcreedor = ddl.SelectedItem.Text;
            CargarContactoAcreedor(nombreAcreedor);
        }

        #endregion


        #region Metodos

        public void asignacionResumen(ref  Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void bloquearTextAreas()
        {
            txtGlosaComercial.Disabled = true;
            txtInstruccionCurse.Disabled = true;
            txtObservacion.Disabled = true;
            txtDestinoSolicitud.Disabled = true;
            txtComentarios.Disabled = true;
            lboxCFAnterior.Enabled = false;
            lboxCFSucesor.Enabled = false;
            lboxCFAnterior.Attributes.Add("disabled", "");
            lboxCFSucesor.Attributes.Add("disabled", "");
        }

        private void validarFechas()
        {
            dtcFechaCierre.MinDate = DateTime.Today.Date;
            dtcFechaVenc.MinDate = DateTime.Today.Date;
        }

        private void validacionTimbreEstampilla()
        {
            //Impuesto Timbre y Estampilla (Cargo Acreedor) – Si no posee a cargo de Multiaval se puede podrá ingresar, caso contrario bloquear.
            if (String.IsNullOrWhiteSpace(txtTimbreEstampillaMultiaval.Text) || (txtTimbreEstampillaMultiaval.Text.Trim() == "0") || String.IsNullOrEmpty(txtTimbreEstampillaMultiaval.Text))
            {
                txtTimbreEstampillaAcreedor.Enabled = true;
                txtTimbreEstampillaMultiaval.Enabled = false;
            }
            else
            {
                txtTimbreEstampillaAcreedor.Enabled = false;
                txtTimbreEstampillaMultiaval.Enabled = true;
                //ddlTimbreEstampillaMultiavalIncluido.SelectedItem.Value = "0";
            }
        }

        private void activarComercial()
        {
            txtGlosaComercial.Disabled = false;
            txtInstruccionCurse.Disabled = false;
            txtObservacion.Disabled = false;
            lboxCFAnterior.Enabled = true;
            lboxCFSucesor.Enabled = true;
            lboxCFAnterior.Attributes.Remove("disabled");
            lboxCFSucesor.Attributes.Remove("disabled");
        }

        private void bloquearCampos(string desccargo)
        {
            asignacionResumen(ref objresumen);
            txtServicioGestionLegal.Enabled = false;
            txtNroPaf.Enabled = false;
            //ddlTipoOperacion = false;
            if (ddlProducto.Items.Count > 0 && ddlTipoCredito.Items.Count > 0)
            {
                if (ddlProducto.SelectedItem.Text == "Certificado Fianza Comercial" && ddlTipoCredito.SelectedItem.Text == "Cuotas iguales con Cuotón")
                    txtHorizonte.ReadOnly = false;
            }
            if (ddlFogapeVigente.SelectedItem.Text == "Si")
            {
                txtCodigoSafio.Enabled = true;
                if (ddlEdoCertificado.SelectedItem.Text == "Ejecutado")
                {
                    dtcFechaRecuperacion.Enabled = true;
                    TxtMontoRecuperoFogape.Enabled = true;
                }
                else
                {
                    dtcFechaRecuperacion.Enabled = false;
                    TxtMontoRecuperoFogape.Enabled = false;
                }
            }
            else
            {
                txtCodigoSafio.Enabled = false;
                dtcFechaRecuperacion.Enabled = false;
                TxtMontoRecuperoFogape.Enabled = false;
            }

            if (objresumen.desSubEtapa.ToLower() == "por facturar" || objresumen.desSubEtapa.ToLower() == "facturado" || objresumen.desSubEtapa.ToLower() == "pagado")
                ddlFondo.Enabled = false;
            else
                ddlFondo.Enabled = true;

            if (objresumen.IdCotizacion.ToString() != "0")
            {
                HdfCotizacion.Value = objresumen.IdCotizacion.ToString();

                if (!util.EstaPermitido(desccargo, Cargos))
                {
                    bloquearCamposCotizacion();
                }
            }
        }

        private void bloquearCamposCotizacion()
        {
            ddlProducto.Attributes.Add("disabled", "true");
            txtMontoOperacion.ReadOnly = true;
            ddlTipoMoneda.Enabled = false;
            //ddlTipoMoneda.Attributes.Add("disabled", "disabled");
            //ddlTipoCredito.Attributes.Add("disabled", "true");
            ddlTipoCredito.Enabled = false;
            txtPlazo.ReadOnly = true;

            if (ddlProducto.SelectedItem.Text.ToLower() != "certificado fianza técnica")
                ddlTipoAmortizacion.Enabled = false;

            ddlPorceCobertura.Enabled = false;
            txtTasaInteres.ReadOnly = true;
            //ddlPeriocidad.Attributes.Add("disabled", "true");
            ddlPeriocidad.Enabled = false;
            txtMontoComision.ReadOnly = true;
            txtPorcComision.ReadOnly = true;
            //ddlComisionIncluida.Attributes.Add("disabled", "true");
            ddlComisionIncluida.Enabled = false;
            txtNroCuotas.ReadOnly = true;
            txtHorizonte.ReadOnly = true;
            txtPeriodoGracia.ReadOnly = true;
            txtNotario.ReadOnly = true;
            //ddlNotarioIncluido.Attributes.Add("disabled", "true");
            ddlNotarioIncluido.Enabled = false;
            //ddlFogapeVigente.Attributes.Add("disabled", "true");
            //txtComisionFogape.ReadOnly = true;
        }

        private void cargarDatosOperacion()
        {
            ViewState["NuevoIdAcreedor"] = "0";
            CargarCombos();
            LogicaNegocio LN = new LogicaNegocio();
            DataSet dtSet = new DataSet();
            asignacionResumen(ref objresumen);
            dtSet = LN.CargarDatosOperacion(objresumen.idEmpresa, objresumen.idOperacion, objresumen.area);
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            if (dtSet.Tables.Count >= 1)
            {
                dt = dtSet.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    #region Operaciones
                    hdnSPIdOperacion.Value = dt.Rows[0]["SP_IdOperacion"].ToString();
                    txtMontoOperacion.Text = dt.Rows[0]["MontoOperacion"].ToString();
                    txtPlazo.Text = dt.Rows[0]["Plazo"].ToString();
                    txtNroCuotas.Text = dt.Rows[0]["numCuotas"].ToString();
                    txtTasaInteres.Text = dt.Rows[0]["TasaIntBco"].ToString();
                    txtMontoComision.Text = dt.Rows[0]["comisionCLP"].ToString();

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["fecEstimadaCierre"].ToString()))
                    {
                        dtcFechaCierre.SelectedDate = DateTime.Parse(dt.Rows[0]["fecEstimadaCierre"].ToString());
                    }

                    txtMontoSeguroDesgravamen.Text = dt.Rows[0]["costoSeguroDegravamen"].ToString();
                    txtCoberturaFogape.Text = dt.Rows[0]["descCobFOGAPE"].ToString();
                    txtTimbreEstampillaAcreedor.Text = dt.Rows[0]["TimbreYEstampillaAcreedor"].ToString();
                    txtNotario.Text = dt.Rows[0]["Notario"].ToString();

                    //txtMontoOperacionCLP.Text = hdnMontoOperacionCLP.Value = dt.Rows[0]["MontoOperacionCLP"].ToString();

                    if (dt.Rows[0]["Horizonte"].ToString() != "0")
                        txtHorizonte.Text = dt.Rows[0]["Horizonte"].ToString();
                    txtPeriodoGracia.Text = dt.Rows[0]["periodoGracia"].ToString();
                    txtPorcComision.Text = dt.Rows[0]["PorcComision"].ToString();
                    txtMontoSeguroIncendio.Text = dt.Rows[0]["montoSeguroIncendio"].ToString();
                    txtLicitacionFogape.Text = dt.Rows[0]["LicitFOGAPE"].ToString();
                    txtTimbreEstampillaMultiaval.Text = dt.Rows[0]["TimbreYEstampillaMultiaval"].ToString();
                    txtServicioGestionLegal.Text = dt.Rows[0]["servicioGestionLegal"].ToString();
                    txtGlosaComercial.InnerText = dt.Rows[0]["glosaComercial"].ToString();
                    txtInstruccionCurse.InnerText = dt.Rows[0]["InstCurse"].ToString();
                    txtObservacion.InnerText = dt.Rows[0]["observaciones"].ToString();

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["fecEmision"].ToString()))
                    {
                        dtcFechaEmi.SelectedDate = DateTime.Parse(dt.Rows[0]["fecEmision"].ToString());
                    }

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["fec1erVencimiento"].ToString()))
                    {
                        dtcFecha1erVenc.SelectedDate = DateTime.Parse(dt.Rows[0]["fec1erVencimiento"].ToString());
                    }

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["FechaVencimiento"].ToString()))
                    {
                        dtcFechaVenc.SelectedDate = DateTime.Parse(dt.Rows[0]["FechaVencimiento"].ToString());
                    }

                    TxtMontoRecuperoFogape.Text = dt.Rows[0]["MontoRecuperoFogape"].ToString();

                    if (dtSet.Tables[4].Rows.Count > 0)
                    {
                        txtComisionFogape.Text = dt.Rows[0]["MontoComisionFogape"].ToString();
                        HdfTasaComisionAnual.Value = dtSet.Tables[4].Rows[0]["ComisionAnual"].ToString();
                    }
                    else
                    {
                        txtComisionFogape.Text = dt.Rows[0]["MontoComisionFogape"] == null ? "0" : dt.Rows[0]["MontoComisionFogape"].ToString();
                    }
                    #endregion

                    #region Riesgo
                    txtCostoFondo.Text = dt.Rows[0]["costoFondo"].ToString();
                    txtNroPaf.Text = dt.Rows[0]["IdPaf"].ToString();
                    txtDestinoSolicitud.InnerText = dt.Rows[0]["destinoSolicitud"].ToString();
                    txtNroCertificado.Text = dt.Rows[0]["NCertificado"].ToString();
                    txtNroCredito.Text = dt.Rows[0]["numCredito"].ToString();
                    txtNroDocumento.Text = dt.Rows[0]["NDocumento"].ToString();
                    if (string.IsNullOrEmpty(dt.Rows[0]["valorMonedaEmision"].ToString()))
                        txtValorMonedaEmision.Text = "26550";
                    else
                        txtValorMonedaEmision.Text = dt.Rows[0]["valorMonedaEmision"].ToString();
                    txtMontoCuota.Text = dt.Rows[0]["MontoCta"].ToString();
                    txtMontoCuoton.Text = dt.Rows[0]["MontoCuoton"].ToString();
                    txtCodigoSafio.Text = dt.Rows[0]["codSAFIO"].ToString();

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["fecRecuperacion"].ToString()))
                    {
                        dtcFechaRecuperacion.SelectedDate = DateTime.Parse(dt.Rows[0]["fecRecuperacion"].ToString());
                    }

                    #endregion

                    #region Pagaré
                    txtNroPagare.Text = dt.Rows[0]["NroPagare"].ToString();

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["FecAprobacionPagare"].ToString()))
                    {
                        dtcFechaAprobacion.SelectedDate = DateTime.Parse(dt.Rows[0]["FecAprobacionPagare"].ToString());
                    }

                    if (!String.IsNullOrWhiteSpace(dt.Rows[0]["FechaRevision"].ToString()))
                    {
                        dtcFechaRevision.SelectedDate = DateTime.Parse(dt.Rows[0]["FechaRevision"].ToString());
                    }

                    txtComentarios.InnerText = dt.Rows[0]["ComentarioPagare"].ToString();

                    #endregion

                    #region cargaDDLs

                    if (dt.Rows[0]["descTipoM"].ToString() != "")
                    {
                        ddlTipoMoneda.SelectedIndex = ddlTipoMoneda.Items.IndexOf(ddlTipoMoneda.Items.FindByText(dt.Rows[0]["descTipoM"].ToString()));
                        if (dt.Rows[0]["descTipoM"].ToString() == "CLP")
                        {
                            txtMontoOperacionCLP.Text = txtMontoOperacion.Text.Trim();
                            hdnMontoOperacionCLP.Value = txtMontoOperacion.Text.Trim();
                        }
                        else if (dt.Rows[0]["descTipoM"].ToString() == "UF")
                        {
                            var a = float.Parse(txtMontoOperacion.Text.Trim().Replace(".", "").Replace(",", ".")) * float.Parse(txtValorMonedaEmision.Text.Trim().Replace(".", "").Replace(",", "."));
                            txtMontoOperacionCLP.Text = a.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                            hdnMontoOperacionCLP.Value = (double.Parse(txtMontoOperacion.Text.Trim().Replace(".", "").Replace(",", ".")) * double.Parse(txtValorMonedaEmision.Text.Trim().Replace(".", "").Replace(",", "."))).ToString();
                        }
                        else if (dt.Rows[0]["descTipoM"].ToString() == "USD")
                        {
                            var a = txtMontoOperacion.Text.Trim().GetValorDouble() * txtValorMonedaEmision.Text.Trim().GetValorDouble();
                            txtMontoOperacionCLP.Text = a.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                            hdnMontoOperacionCLP.Value = (txtMontoOperacion.Text.Trim().GetValorDouble() * txtValorMonedaEmision.Text.Trim().GetValorDouble()).ToString();
                        }
                        else
                        {
                            txtMontoOperacionCLP.Text = "0";
                            hdnMontoOperacionCLP.Value = "0";
                        }
                    }
                    if (dt.Rows[0]["IdProducto"].ToString() != "")
                    {
                        ddlProducto.SelectedIndex = ddlProducto.Items.IndexOf(ddlProducto.Items.FindByValue(dt.Rows[0]["IdProducto"].ToString()));
                        CargarTipoCredito(ddlProducto.SelectedItem.Text);
                        //Page.Session["RutAcreedor"] = string.IsNullOrEmpty(dt.Rows[0]["RutAcreedor"].ToString()) ? "0" : dt.Rows[0]["RutAcreedor"].ToString();
                        CargarAcreedores(ddlProducto.SelectedItem.Text.Trim());
                    }

                    CargarFinalidad();

                    if (dt.Rows[0]["IdFinalidad"].ToString() != "")
                    {
                        ddlFinalidad.SelectedIndex = ddlFinalidad.Items.IndexOf(ddlFinalidad.Items.FindByValue(dt.Rows[0]["IdFinalidad"].ToString()));
                    }
                    if (dt.Rows[0]["IdTipoCredito"].ToString() != "")
                    {
                        ddlTipoCredito.SelectedIndex = ddlTipoCredito.Items.IndexOf(ddlTipoCredito.Items.FindByValue(dt.Rows[0]["IdTipoCredito"].ToString()));
                    }
                    if (dt.Rows[0]["IdTipoCta"].ToString() != "")
                    {
                        ddlTipoCuota.SelectedIndex = ddlTipoCuota.Items.IndexOf(ddlTipoCuota.Items.FindByValue(dt.Rows[0]["IdTipoCta"].ToString()));
                    }
                    if (dt.Rows[0]["IdTipoAmort"].ToString() != "")
                    {
                        ddlTipoAmortizacion.SelectedIndex = ddlTipoAmortizacion.Items.IndexOf(ddlTipoAmortizacion.Items.FindByValue(dt.Rows[0]["IdTipoAmort"].ToString()));
                    }
                    if (dt.Rows[0]["IdPorcCobCF"].ToString() != "")
                    {
                        ddlPorceCobertura.SelectedIndex = ddlPorceCobertura.Items.IndexOf(ddlPorceCobertura.Items.FindByValue(dt.Rows[0]["IdPorcCobCF"].ToString()));
                    }
                    if (dt.Rows[0]["IdFondo"].ToString() != "")
                    {
                        ddlFondo.SelectedIndex = ddlFondo.Items.IndexOf(ddlFondo.Items.FindByValue(dt.Rows[0]["IdFondo"].ToString()));
                    }
                    if (dt.Rows[0]["IdPeriodic"].ToString() != "")
                    {
                        ddlPeriocidad.SelectedIndex = ddlPeriocidad.Items.IndexOf(ddlPeriocidad.Items.FindByValue(dt.Rows[0]["IdPeriodic"].ToString()));
                    }
                    if (dt.Rows[0]["comisionIncluida"].ToString() != "")
                    {
                        ddlComisionIncluida.SelectedIndex = ddlComisionIncluida.Items.IndexOf(ddlComisionIncluida.Items.FindByValue(dt.Rows[0]["comisionIncluida"].ToString()));
                    }
                    if (dt.Rows[0]["IdPeriocidadPago"].ToString() != "")
                    {
                        ddlPeriocidadPago.SelectedIndex = ddlPeriocidadPago.Items.IndexOf(ddlPeriocidadPago.Items.FindByValue(dt.Rows[0]["IdPeriocidadPago"].ToString()));
                    }
                    if (dt.Rows[0]["IdAcreedor"].ToString() != "")
                    {
                        var id = ViewState["NuevoIdAcreedor"].ToString();
                        if (dt.Rows[0]["IdAcreedor"].ToString() == "0" && ViewState["NuevoIdAcreedor"].ToString() != "0")
                            ddlAcreedor.SelectedIndex = ddlAcreedor.Items.IndexOf(ddlAcreedor.Items.FindByValue(ViewState["NuevoIdAcreedor"].ToString()));
                        else
                            ddlAcreedor.SelectedIndex = ddlAcreedor.Items.IndexOf(ddlAcreedor.Items.FindByValue(dt.Rows[0]["IdAcreedor"].ToString()));
                    }

                    CargarContactoAcreedor(ddlAcreedor.SelectedItem.Text);

                    if (dt.Rows[0]["IdContactAcreedor"].ToString() != "" && ddlContacAcreedor.Items.Count > 0)
                    {
                        ddlContacAcreedor.SelectedIndex = ddlContacAcreedor.Items.IndexOf(ddlContacAcreedor.Items.FindByValue(dt.Rows[0]["IdContactAcreedor"].ToString()));
                    }
                    if (dt.Rows[0]["IdCanal"].ToString() != "")
                    {
                        ddlCanal.SelectedIndex = ddlCanal.Items.IndexOf(ddlCanal.Items.FindByValue(dt.Rows[0]["IdCanal"].ToString()));
                    }
                    if (dt.Rows[0]["SegDesgavamenIncluido"].ToString() != "")
                    {
                        ddlSeguroDesgravamenIncluido.SelectedIndex = ddlSeguroDesgravamenIncluido.Items.IndexOf(ddlSeguroDesgravamenIncluido.Items.FindByValue(dt.Rows[0]["SegDesgavamenIncluido"].ToString()));
                    }
                    if (dt.Rows[0]["SegIncluido"].ToString() != "")
                    {
                        ddlSeguroIncendioIncluido.SelectedIndex = ddlSeguroIncendioIncluido.Items.IndexOf(ddlSeguroIncendioIncluido.Items.FindByValue(dt.Rows[0]["SegIncluido"].ToString()));
                    }
                    if (dt.Rows[0]["incluidoTimbreYEstampillaAcreedor"].ToString() != "")
                    {
                        ddlTimbreEstampillaAcreedorIncluido.SelectedIndex = ddlTimbreEstampillaAcreedorIncluido.Items.IndexOf(ddlTimbreEstampillaAcreedorIncluido.Items.FindByValue(dt.Rows[0]["incluidoTimbreYEstampillaAcreedor"].ToString()));
                    }
                    if (dt.Rows[0]["incluidoTimbreYEstampilla"].ToString() != "")
                    {
                        ddlTimbreEstampillaMultiavalIncluido.SelectedIndex = ddlTimbreEstampillaMultiavalIncluido.Items.IndexOf(ddlTimbreEstampillaMultiavalIncluido.Items.FindByValue(dt.Rows[0]["incluidoTimbreYEstampilla"].ToString()));
                    }
                    if (dt.Rows[0]["incluidoNotario"].ToString() != "")
                    {
                        ddlNotarioIncluido.SelectedIndex = ddlNotarioIncluido.Items.IndexOf(ddlNotarioIncluido.Items.FindByValue(dt.Rows[0]["incluidoNotario"].ToString()));
                    }
                    if (dt.Rows[0]["incluidoGastosOpe"].ToString() != "")
                    {
                        ddlServGestionLegalIncluido.SelectedIndex = ddlServGestionLegalIncluido.Items.IndexOf(ddlServGestionLegalIncluido.Items.FindByValue(dt.Rows[0]["incluidoGastosOpe"].ToString()));
                    }
                    if (dt.Rows[0]["IdEstCertificado"].ToString() != "")
                    {
                        ddlEdoCertificado.SelectedIndex = ddlEdoCertificado.Items.IndexOf(ddlEdoCertificado.Items.FindByValue(dt.Rows[0]["IdEstCertificado"].ToString()));
                    }
                    if (dt.Rows[0]["IdEdoCredito"].ToString() != "")
                    {
                        ddlEdoCredito.SelectedIndex = ddlEdoCredito.Items.IndexOf(ddlEdoCredito.Items.FindByValue(dt.Rows[0]["IdEdoCredito"].ToString()));
                    }
                    if (dt.Rows[0]["IdTipoOperacion"].ToString() != "")
                    {
                        ddlTipoOperacion.SelectedIndex = ddlTipoOperacion.Items.IndexOf(ddlTipoOperacion.Items.FindByValue(dt.Rows[0]["IdTipoOperacion"].ToString()));
                    }
                    if (dt.Rows[0]["IdTipoObligAfianz"].ToString() != "")
                    {
                        ddlTipoObligaAfianz.SelectedIndex = ddlTipoObligaAfianz.Items.IndexOf(ddlTipoObligaAfianz.Items.FindByValue(dt.Rows[0]["IdTipoObligAfianz"].ToString()));
                    }
                    if (dt.Rows[0]["FormalizSAFIO"].ToString() != "")
                    {
                        ddlSafioIncluido.SelectedIndex = ddlSafioIncluido.Items.IndexOf(ddlSafioIncluido.Items.FindByValue(dt.Rows[0]["FormalizSAFIO"].ToString()));
                    }
                    if (dt.Rows[0]["GarantiaFogapeVigente"].ToString() != "")
                    {
                        ddlGarantiaFogapeVigente.SelectedIndex = ddlFogapeVigente.Items.IndexOf(ddlFogapeVigente.Items.FindByValue(dt.Rows[0]["GarantiaFogapeVigente"].ToString()));

                        if (ddlGarantiaFogapeVigente.SelectedValue.ToString() == "True")
                        {
                            //lblMotivoFogape.Style.Add("display", "none");
                            //ddlMotivoGFV.Style.Add("display", "none");
                        }
                        else
                        {
                            if (dt.Rows[0]["IdMotivoFogapeVigente"].ToString() != "" || dt.Rows[0]["IdMotivoFogapeVigente"].ToString() != "0")
                                ddlMotivoGFV.SelectedIndex = ddlMotivoGFV.Items.IndexOf(ddlMotivoGFV.Items.FindByValue(dt.Rows[0]["IdMotivoFogapeVigente"].ToString()));
                        }
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteLegal"].ToString()))
                    {
                        ddlReLegal.SelectedIndex = ddlReLegal.Items.IndexOf(ddlReLegal.Items.FindByValue(dt.Rows[0]["RutRepresentanteLegal"].ToString()));
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteFondo"].ToString()))
                    {
                        ddlReFondo.SelectedIndex = ddlReFondo.Items.IndexOf(ddlReFondo.Items.FindByValue(dt.Rows[0]["RutRepresentanteFondo"].ToString()));
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteLegal2"].ToString()))
                    {
                        ddlReLegal2.SelectedIndex = ddlReLegal2.Items.IndexOf(ddlReLegal2.Items.FindByValue(dt.Rows[0]["RutRepresentanteLegal2"].ToString()));
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[0]["RutRepresentanteFondo2"].ToString()))
                    {
                        ddlReFondo2.SelectedIndex = ddlReFondo2.Items.IndexOf(ddlReFondo2.Items.FindByValue(dt.Rows[0]["RutRepresentanteFondo2"].ToString()));
                    }

                    if (dt.Rows[0]["GtiaFOGAPE"].ToString() != "")
                    {
                        ddlFogapeVigente.SelectedIndex = ddlFogapeVigente.Items.IndexOf(ddlFogapeVigente.Items.FindByValue(dt.Rows[0]["GtiaFOGAPE"].ToString()));
                        if (ddlFogapeVigente.SelectedItem.Text == "Si")
                        {
                            if (dtSet.Tables[4].Rows.Count > 0)
                            {
                                txtLicitacionFogape.Text = dtSet.Tables[4].Rows[0]["NroLicitacion"].ToString();
                                HdfLicitacionFogape.Value = dtSet.Tables[4].Rows[0]["NroLicitacion"].ToString();
                            }
                        }
                    }
                    if (dt.Rows[0]["IdCheck"].ToString() != "")
                    {
                        ddlCheck.SelectedIndex = ddlCheck.Items.IndexOf(ddlCheck.Items.FindByValue(dt.Rows[0]["IdCheck"].ToString()));
                    }
                    if (dt.Rows[0]["idEdoPagare"].ToString() != "")
                    {
                        ddlEdoPagare.SelectedIndex = ddlEdoPagare.Items.IndexOf(ddlEdoPagare.Items.FindByValue(dt.Rows[0]["idEdoPagare"].ToString()));
                    }
                    if (dt.Rows[0]["ResponsablePagare"].ToString() != "")
                    {
                        ddlResponsable.SelectedIndex = ddlResponsable.Items.IndexOf(ddlResponsable.Items.FindByText(dt.Rows[0]["ResponsablePagare"].ToString()));
                    }

                    #endregion
                }
            }
            if (dtSet.Tables.Count >= 2)
            {
                dt2 = dtSet.Tables[1];
                lboxCFAnterior.Items.Clear();
                lboxCFSucesor.Items.Clear();
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    ListItem liTemp = new ListItem(dt2.Rows[i]["NCertificado"].ToString(), (i + 1).ToString());
                    ListItem liTemp2 = new ListItem(dt2.Rows[i]["NCertificado"].ToString(), (i + 1).ToString());
                    lboxCFAnterior.Items.Add(liTemp);
                    lboxCFSucesor.Items.Add(liTemp2);
                }
            }
            if (dtSet.Tables.Count >= 3)
            {
                DataTable dt3 = new DataTable();
                dt3 = dtSet.Tables[2];
                foreach (ListItem item in lboxCFSucesor.Items)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        if (item.Text == dt3.Rows[i]["NCertificado"].ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }

            }
            if (dtSet.Tables.Count >= 4)
            {
                DataTable dt4 = new DataTable();
                dt4 = dtSet.Tables[3];
                foreach (ListItem item in lboxCFAnterior.Items)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        if (item.Text == dt4.Rows[i]["NCertificado"].ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }

            txtSaldoCapital.Text = dt.Rows[0]["SaldoCapital"].ToString();
        }

        private void ValidarProductos()
        {
            ddlFinalidad.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtFinalidad = Ln.ListarFinalidad(Convert.ToInt32(ddlProducto.SelectedItem.Value));
            util.CargaDDL(ddlFinalidad, dtFinalidad, "Nombre", "IdFinalidad");


            //por defecto coloco los campo que depende de tipo producto habilitado
            ddlTipoCredito.Enabled = true;
            ddlTipoAmortizacion.Enabled = true;
            txtNroCuotas.Enabled = true;
            ddlPeriocidad.Enabled = true;
            ddlTipoCuota.Enabled = true;

            // si es CFT ID =2
            if (ddlProducto.SelectedItem.Text == "Certificado Fianza Técnica" || ddlProducto.SelectedItem.Text == "Línea Certificado Fianza Técnica")
            {
                //preseleccionar Contingente ID = 6 y deshabilitado
                ddlTipoAmortizacion.SelectedIndex = ddlTipoAmortizacion.Items.IndexOf(ddlTipoAmortizacion.Items.FindByValue(Convert.ToString(6)));
                ddlTipoAmortizacion.Enabled = false;
                //por defecto nun cuotas =1 y deshabilitado
                txtNroCuotas.Text = "1";
                txtNroCuotas.Enabled = false;
                //por defecto 5 Fianza Tecnica y deshabilitado
                CargarTipoCredito(ddlProducto.SelectedItem.Text);
                if (ddlTipoCredito.Items.Count > 1)
                {
                    ddlTipoCredito.SelectedIndex = 1;
                    ddlTipoCredito.Enabled = false;
                }

                // por defecto periocidad todo el periodo id=7 y deshabilitado
                ddlPeriocidad.SelectedIndex = ddlPeriocidad.Items.IndexOf(ddlPeriocidad.Items.FindByValue(Convert.ToString(7)));
                ddlPeriocidad.Enabled = false;
                //por defecto tipo cuota No aplica ID=3  y deshabilitado
                ddlTipoCuota.SelectedIndex = ddlTipoCuota.Items.IndexOf(ddlTipoCuota.Items.FindByValue(Convert.ToString(3)));
                ddlTipoCuota.Enabled = false;

                ddlTipoOperacion.SelectedIndex = 2;
                //ddlTipoOperacion.Enabled = false;
                txtHorizonte.Text = txtPlazo.Text.Trim();
                //txtHorizonte.Enabled = false;
            }

            //si es CFC ID = 1
            if (ddlProducto.SelectedItem.Text == "Certificado Fianza Comercial")
            {
                CargarTipoCredito(ddlProducto.SelectedItem.Text);
                if (Convert.ToInt32(txtNroCuotas.Text) > 1)
                    ddlTipoOperacion.SelectedIndex = 1;
                else
                    ddlTipoOperacion.SelectedIndex = 2;

                //ddlTipoOperacion.Enabled = false;

            }

            if (ddlProducto.SelectedItem.Text == "Asesoría")
                ddlTipoCredito.Items.Clear();
        }

        private void CargarCombos()
        {
            CargarCanal();
            CargarEdoCertificado();
            CargarEdoCredito();
            CargarEdoPagare();
            CargarFinalidad();
            CargarFondos();
            CargarMonedas();
            CargarPeriocidad();
            CargarPeriocidadDePago();
            CargarProducto();
            CargarResponsable();
            CargarTipoAmortizacion();
            CargarTipoCuota();
            CargarTipoObligacionAfianzada();
            CargarTipoOperacion();
            CargarMotivoFogape();
            CargarRepresentanteLegal();
            CargarRepresentanteLegal2();
            CargarRepresentanteFondo();
            CargarRepresentanteFondo2();
        }

        private void CargarCanal()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtCanal = Ln.ListarCanal(null);
            util.CargaDDL(ddlCanal, dtCanal, "Nombre", "Id");
        }

        private void CargarPeriocidadDePago()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtPeriocidad = Ln.ListarPeriocidadDePago();
            util.CargaDDL(ddlPeriocidadPago, dtPeriocidad, "Nombre", "Id");
        }

        private void CargarRepresentanteLegal()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtRepLegal1 = Ln.ListarAprobadores(2, 1);
            util.CargaDDL(ddlReLegal, dtRepLegal1, "Usuario", "Rut");
        }

        private void CargarRepresentanteLegal2()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtRepLegal2 = Ln.ListarAprobadores(2, 1);
            util.CargaDDL(ddlReLegal2, dtRepLegal2, "Usuario", "Rut");
        }

        private void CargarRepresentanteFondo()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtReFondo = Ln.ListarAprobadores(3, 1);
            util.CargaDDL(ddlReFondo, dtReFondo, "Usuario", "Rut");
        }

        private void CargarRepresentanteFondo2()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtRepFondo = Ln.ListarAprobadores(3, 1);
            util.CargaDDL(ddlReFondo2, dtRepFondo, "Usuario", "Rut");
        }

        private void CargarTipoAmortizacion()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtProductos = Ln.ListarTipoAmortizacion();
            util.CargaDDL(ddlTipoAmortizacion, dtProductos, "Nombre", "Id");
        }

        private void CargarProducto()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtProductos = Ln.ListarProducto(1);
            util.CargaDDL(ddlProducto, dtProductos, "NombreProducto", "IdProducto");
        }

        private void CargarTipoCredito(string producto)
        {
            ddlTipoCredito.Items.Clear();
            if (producto != "Seleccione")
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtTipoCredito = Ln.ListarTipoCredito(Convert.ToInt32(ddlProducto.SelectedValue));
                util.CargaDDL(ddlTipoCredito, dtTipoCredito, "Nombre", "Id");
            }
            else
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtTipoCredito = Ln.ListarTipoCredito(null);
                util.CargaDDL(ddlTipoCredito, dtTipoCredito, "Nombre", "Id");
            }
        }

        private void CargarFinalidad()
        {
            if (ddlProducto.SelectedIndex > 0)
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtFinalidad = Ln.ListarFinalidad(Convert.ToInt32(ddlProducto.SelectedItem.Value));
                util.CargaDDL(ddlFinalidad, dtFinalidad, "Nombre", "IdFinalidad");
            }
        }

        private void CargarAcreedores(string producto)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();

            if (ddlProducto.SelectedItem.Text.Contains("Fianza Comercial"))
            {
                dt = Ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");
            }
            else
            {
                dt = Ln.CRUDAcreedores(6, "", 0, "", "", 0, 0, 0, 0, "");
            }

            util.CargaDDL(ddlAcreedor, dt, "Nombre", "IdAcreedor");
        }

        private void CargarMonedas()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoMoneda = Ln.ListarMonedas();
            util.CargaDDL(ddlTipoMoneda, dtTipoMoneda, "Abreviacion", "IdMoneda");
        }

        private void CargarPeriocidad()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtPeriocidad = Ln.ListarPeriocidad();
            util.CargaDDL(ddlPeriocidad, dtPeriocidad, "Nombre", "Id");
        }

        private void CargarEdoCertificado()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEdoCertificado = Ln.ListarEstadoCertificado();
            util.CargaDDL(ddlEdoCertificado, dtEdoCertificado, "Nombre", "Id");
        }

        private void CargarTipoCuota()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoCuota = Ln.ListarTipoCuota();
            util.CargaDDL(ddlTipoCuota, dtTipoCuota, "Nombre", "Id");
        }

        private void CargarFondos()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtFondos = Ln.ListarFondos(null);
            util.CargaDDL(ddlFondo, dtFondos, "Nombre", "IdFondo");
        }

        private void CargarEdoCredito()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEdoCredito = Ln.ListarEstadoCredito();
            util.CargaDDL(ddlEdoCredito, dtEdoCredito, "Nombre", "ID");
        }

        private void CargarTipoOperacion()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoOperacion = Ln.ListarTipoOperacion();
            util.CargaDDL(ddlTipoOperacion, dtTipoOperacion, "Nombre", "Id");
        }

        private void CargarTipoObligacionAfianzada()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoObligacion = Ln.ListarTipoObligacionAfianzada();
            util.CargaDDL(ddlTipoObligaAfianz, dtTipoObligacion, "Nombre", "Id");
        }

        private void CargarContactoAcreedor(String nombreAcreedor)
        {
            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;
            //SPList items = app.Lists["ContactoAcreedor"];
            //SPQuery query = new SPQuery();
            //query.Query = "<Where>" +
            //    "<Eq><FieldRef Name='Acreedor'/><Value Type='Lookup'>" + nombreAcreedor + "</Value></Eq>" +
            //    "</Where>";

            //SPListItemCollection collListItems = items.GetItems(query);
            //util.CargaDDL(ddlContacAcreedor, collListItems.GetDataTable(), "Nombre", "ID");

            //if (ddlContacAcreedor.Items.Count == 0)
            //{
            //    ddlContacAcreedor.Items.Clear();
            //    ddlContacAcreedor.Items.Insert(0, new ListItem("Seleccione", "0"));
            //}
        }

        private void CargarEdoPagare()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEdoPagare = Ln.ListarEstadoPagare();
            util.CargaDDL(ddlEdoPagare, dtEdoPagare, "Nombre", "Id");
        }

        private void CargarResponsable()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 3, "");
                util.CargaDDL(ddlResponsable, dt, "nombreApellido", "idUsuario");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarMotivoFogape()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtFogapeVigente = Ln.ListarMotivoFogapeVigente();
            util.CargaDDL(ddlMotivoGFV, dtFogapeVigente, "Nombre", "Id");
        }

        private string generarXML(Panel panelInfo)
        {
            asignacionResumen(ref objresumen);
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;

            XmlNode root = doc.DocumentElement;

            RespNode = doc.CreateElement("Val");

            foreach (Control Controles in panelInfo.Controls)
            {
                if (Controles is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Controles.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Controles).Text.Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }
                else if (Controles is CheckBox)
                {
                    XmlNode nodo = doc.CreateElement(Controles.ID);
                    nodo.AppendChild(doc.CreateTextNode(((CheckBox)Controles).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                else if (Controles is DropDownList)
                {
                    if (((DropDownList)Controles).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Controles.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Controles).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Controles.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Controles).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                else if (Controles is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Controles.ID);
                    if (!((DateTimeControl)Controles).IsDateEmpty)
                    // string fecha = ((DateTimeControl)Controles).SelectedDate.Year.ToString() + "-" + ((DateTimeControl)Controles).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Controles).SelectedDate.Day.ToString();
                    {
                        string fecha = ((DateTimeControl)Controles).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Controles).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Controles).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else
                        nodo.AppendChild(doc.CreateTextNode("-1"));

                    RespNode.AppendChild(nodo);
                }
            }
            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtValorMoneda = new DataTable("dtValorMoneda");
            dtValorMoneda = Ln.GestionValorMoneda(DateTime.Now, 0, null, null, null, 1);

            //obtener uf para calcular el monto ventas de la empresa
            //string indicador = util.BuscarValorIndicador(DateTime.Now);
            string txtUF = string.Empty;
            txtUF = dtValorMoneda.Rows[0]["MontoUF"].ToString();

            Operaciones op = new Operaciones();
            op = datosOperacion;
            string mensaje = new Operaciones { }.ValidarReglasNegocioOperacion(op, txtUF);
            if (mensaje == "OK")
                Grabar();
            else
                error(mensaje);
        }

        private Operaciones datosOperacion
        {
            get
            {
                LogicaNegocio LN = new LogicaNegocio();
                asignacionResumen(ref objresumen);
                Operaciones value = new Operaciones();

                value._descEtapa = objresumen.desEtapa;
                value._descSubEtapa = objresumen.desSubEtapa;
                value._descProducto = ddlProducto.SelectedItem.Text;
                value._tipoCredito = ddlTipoCredito.SelectedItem.Text;
                value._horizonte = txtHorizonte.Text.Replace(".", "").Replace(",", ".");
                value._montoCuoton = txtMontoCuoton.Text == "" ? "0" : txtMontoCuoton.Text.Replace(".", "").Replace(",", ".");
                value._tieneFogape = ddlFogapeVigente.SelectedItem.Text;
                value._codigoSafio = txtCodigoSafio.Text == "" ? "0" : txtCodigoSafio.Text;
                value._garantia = "";
                value._estadoCertificado = ddlEdoCertificado.SelectedItem.Text;
                value._fechaRecuperacion = dtcFechaRecuperacion.SelectedDate;
                value._montoRecuperoFogape = TxtMontoRecuperoFogape.Text.Replace(".", "").Replace(",", ".");
                value._motivoGarantiaFogape = ddlMotivoGFV.SelectedItem.Text;
                value._coberturaFogape = txtCoberturaFogape.Text.Replace(".", "").Replace(",", ".");
                value._garantiaFogapeVigente = ddlGarantiaFogapeVigente.SelectedItem.Text;
                value._descFondo = ddlFondo.SelectedItem.Text;
                value._idEmpresa = objresumen.idEmpresa;
                value._idOperacion = objresumen.idOperacion;
                value._cobertura = ddlPorceCobertura.SelectedItem.Text;
                value._descMoneda = ddlTipoMoneda.SelectedItem.Text;
                if (txtPorcComision.Text == "")
                    value._comision = 0;
                else
                    value._comision = Convert.ToDecimal(txtPorcComision.Text.Replace(".", "").Replace(",", "."));

                value._finalidad = ddlFinalidad.SelectedItem.Text;
                value._plazo = txtPlazo.Text.Trim();
                value._fecEmis = dtcFechaEmi.SelectedDate;
                if (txtMontoOperacion.Text == "")
                    value._montoOper = 0;
                else
                    value._montoOper = Convert.ToDouble(txtMontoOperacion.Text.Replace(".", "").Replace(",", "."));

                value._incluyeSafio = ddlSafioIncluido.SelectedItem.Text;
                if (!string.IsNullOrEmpty(txtComisionFogape.Text))
                    value._ComisionFogape = Convert.ToDecimal(txtComisionFogape.Text.Replace(".", "").Replace(",", "."));
                else
                    value._ComisionFogape = 0;

                value._fechaEstCierre = ((TextBox)(dtcFechaCierre.Controls[0])).Text != string.Empty ? dtcFechaCierre.SelectedDate : (DateTime?)null;
                return value;
            }
        }

        private void Grabar()
        {
            asignacionResumen(ref objresumen);
            txtMontoOperacionCLP.Text = hdnMontoOperacionCLP.Value;
            Boolean exito = true;
            String MsjeExito = String.Empty;
            String MsjeError = String.Empty;
            string MsjAlerta = string.Empty;

            LogicaNegocio Ln = new LogicaNegocio();

            if (objresumen.area == "Comercial" || objresumen.area == "Operaciones" || objresumen.area == "Seguimiento")
            {
                if (lblIdOperacion.Text.Trim() != objresumen.idOperacion.ToString())
                    MsjeError = MsjeError + "Error al validar la operación";
                else
                    exito = Ln.ModificarOperacion(objresumen.idEmpresa, objresumen.idOperacion, generarXML(pnComercial), objresumen.idUsuario, objresumen.descCargo, "01", txtGlosaComercial.InnerText, txtInstruccionCurse.InnerText, txtObservacion.InnerText, txtDestinoSolicitud.InnerText, txtComentarios.InnerText);

                if (exito)
                {
                    actualizarLista();
                    String NCFAnterior = ListboxToString(lboxCFAnterior);
                    String NCFSucesor = ListboxToString(lboxCFSucesor);
                    bool exitoCFAnt, exitoCFSuc = true;
                    exitoCFAnt = Ln.GuardarCertificados(objresumen.idOperacion, NCFAnterior, "05", objresumen.idUsuario);
                    exitoCFSuc = Ln.GuardarCertificados(objresumen.idOperacion, NCFSucesor, "01", objresumen.idUsuario);

                    if (exitoCFAnt && exitoCFSuc)
                    {
                        MsjeExito = MsjeExito + "Comercial - ";
                    }
                    else
                    {
                        MsjeError = MsjeError + "(Nro. Certificados Comercial) - ";
                    }
                }
                else
                {
                    MsjeError = MsjeError + "Comercial - ";
                }
            }

            if (objresumen.area.Trim() == "Riesgo" || objresumen.area.Trim() == "Operaciones" || objresumen.desEtapa == "Negociacion" || objresumen.desEtapa == "Negociación" || objresumen.area == "Seguimiento")
            {
                if (lblIdOperacion.Text.Trim() != objresumen.idOperacion.ToString())
                    MsjeError = MsjeError + "Error al validar el id de la operación";
                else
                {
                    //CalcularCostoFondo();
                    exito = Ln.ModificarOperacion(objresumen.idEmpresa, objresumen.idOperacion, generarXML(pnRiesgo), objresumen.idUsuario, objresumen.descCargo, "02", txtGlosaComercial.InnerText, txtInstruccionCurse.InnerText, txtObservacion.InnerText, txtDestinoSolicitud.InnerText, txtComentarios.InnerText);//, ddlReLegal.SelectedItem.Text.Trim(), ddlReFondo.SelectedItem.Text.Trim()
                }

                if (exito)
                {
                    actualizarLista();
                    MsjeExito = MsjeExito + "Riesgo - ";
                }
                else
                {
                    MsjeError = MsjeError + "Riesgo - ";
                }
            }

            Boolean exito2 = true;

            if (objresumen.area == "Operaciones" || objresumen.area.Trim() == "Comercial" || objresumen.area == "Seguimiento" && objresumen.desEtapa == "Fiscalia" || objresumen.desEtapa == "Cierre")
            {
                //se crea el nroCertificado
                if (lblIdOperacion.Text.Trim() != objresumen.idOperacion.ToString())
                    MsjeError = MsjeError + "Error al validar el id de la operación";
                else
                {
                    exito = Ln.ModificarOperacion(objresumen.idEmpresa, objresumen.idOperacion, generarXML(pnOperaciones), objresumen.idUsuario, objresumen.descCargo, "03", txtGlosaComercial.InnerText, txtInstruccionCurse.InnerText, txtObservacion.InnerText, txtDestinoSolicitud.InnerText, txtComentarios.InnerText); //, ddlReLegal.SelectedItem.Text.Trim(), ddlReFondo.SelectedItem.Text.Trim()
                }

                if (exito)
                {
                    actualizarLista();
                    MsjeExito = MsjeExito + "Operación - ";
                }

                else
                {
                    MsjeError = MsjeError + "Operación - ";
                }

                exito2 = Ln.ModificarOperacion(objresumen.idEmpresa, objresumen.idOperacion, generarXML(pnPagare), objresumen.idUsuario, objresumen.descCargo, "04", txtGlosaComercial.InnerText, txtInstruccionCurse.InnerText, txtObservacion.InnerText, txtDestinoSolicitud.InnerText, txtComentarios.InnerText); //, ddlReLegal.SelectedItem.Text.Trim(), ddlReFondo.SelectedItem.Text.Trim()
                if (exito2)
                {
                    actualizarLista();
                    MsjeExito = MsjeExito + "Pagaré - ";
                }
                else
                {
                    MsjeError = MsjeError + " Pagaré - ";
                }
            }

            if (!string.IsNullOrEmpty(MsjeError))
                error("Las Secciones: " + MsjeError + " no se han guardado con éxito.");
            else
                succes("Se han guardado exitosamente sus cambios");

            //verificar si hay campos que son necesarios pero no obligatorios
            MsjAlerta = Ln.AlertaOperacion(objresumen.idEmpresa, objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo);
            if (MsjAlerta != "OK")
                warning(MsjAlerta);
        }

        protected Boolean actualizarLista()
        {
            asignacionResumen(ref objresumen);
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPListItem listData = app.Lists["Operaciones"].GetItemById(int.Parse(hdnSPIdOperacion.Value.ToString()));
                listData["IdProducto"] = Convert.ToInt32(ddlProducto.SelectedItem.Value);
                listData["IdFinalidad"] = Convert.ToInt32(ddlFinalidad.SelectedItem.Value);
                listData["Plazo"] = txtPlazo.Text;
                objresumen.desOperacion = objresumen.idOperacion.ToString() + " - " + ddlProducto.SelectedItem.Value.ToString();
                listData.Update();
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        protected void AsignacionesJS()
        {
            asignacionResumen(ref objresumen);
            txtNroCuotas.Attributes["onChange"] = "ValidarTipoOperacion()";
            ddlTipoCredito.Attributes["onchange"] = "ValidarHorizonte();";
            ddlFogapeVigente.Attributes["onchange"] = "ValidarFogape();";
            ddlEdoCertificado.Attributes["onchange"] = "ValidarFogape();";
            txtCoberturaFogape.Attributes["onchange"] = "CalcularComisionFogape();";
            //txtPlazo.Attributes.Add("onblur", "ValidarHorizonte();");
            txtPlazo.Attributes["onchange"] = "CalcularComisionFogape();";
            txtPeriodoGracia.Attributes["onchange"] = "CalcularComisionFogape();";
            ddlTipoAmortizacion.Attributes["onchange"] = "CalcularComisionFogape();";
            txtMontoOperacion.Attributes["onchange"] = "CalcularComisionFogape();";

            ((TextBox)(dtcFechaEmi.Controls[0])).Attributes.Add("onblur", "CalcularComisionFogape();");
            ((TextBox)(dtcFechaVenc.Controls[0])).Attributes.Add("onblur", "CalcularComisionFogape();");

            if (objresumen.area == "Comercial" && objresumen.desEtapa == "Fiscalia")
            {
                btnLimpiar.OnClientClick = "return LimpiarOperacion('" + pnComercial.ClientID + "');";
                btnCancelar.OnClientClick = "return LimpiarOperacion('" + pnComercial.ClientID + "');";
            }

            if (objresumen.area == "Comercial" && objresumen.desEtapa != "Fiscalia")
            {
                btnLimpiar.OnClientClick = "return LimpiarOperacion('" + pnComercial.ClientID + "');";
                btnCancelar.OnClientClick = "return LimpiarOperacion('" + pnComercial.ClientID + "');";
            }
            if (objresumen.area == "Riesgo")
            {
                btnLimpiar.OnClientClick = "return LimpiarOperacion('" + pnRiesgo.ClientID + "');";
                btnCancelar.OnClientClick = "return LimpiarOperacion('" + pnRiesgo.ClientID + "');";
            }
            if (objresumen.desEtapa == "Negociacion" || objresumen.desEtapa == "Negociación")
            {
                btnLimpiar.OnClientClick = "return LimpiarOperacion('" + pnRiesgo.ClientID + "');";
                btnCancelar.OnClientClick = "return LimpiarOperacion('" + pnRiesgo.ClientID + "');";
            }

            if (objresumen.area == "Operaciones")
            {
                btnLimpiar.OnClientClick = "return LimpiarOperacion('" + pnOperaciones.ClientID + "');";
                btnCancelar.OnClientClick = "return LimpiarOperacion('" + pnOperaciones.ClientID + "');";
            }

            ddlFinalidad.Attributes["onChange"] = "ValidaAmortizacion('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

            string ValPorcentaje = "100"; //ojo lo coloque aqui por si deciden hacer la tabla de indicadores
            txtPorcComision.Attributes.Add("onblur", "return valMaximo('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','txtPorcentajeComision','" + ValPorcentaje + "');");
            txtPorcComision.ToolTip = "Valor Máximo: " + ValPorcentaje;

            txtTasaInteres.Attributes.Add("onblur", "return valMaximo('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','txtTasaInteres','" + ValPorcentaje + "');");
            txtTasaInteres.ToolTip = "Valor Máximo: " + ValPorcentaje;

            //txtPtjCobertura.Attributes.Add("onblur", "return valMaximo('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','txtPtjCobertura','" + ValPorcentaje + "');");
            //txtPtjCobertura.ToolTip = "Valor Máximo: " + ValPorcentaje;

            string ValPlazo = "144"; //ojo lo coloque aqui por si deciden hacer la tabla de indicadores
            txtPlazo.Attributes.Add("onblur", "return valMaximo('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','txtPlazo','" + ValPlazo + "');");
            txtPlazo.ToolTip = "Valor Máximo: " + ValPlazo;
            txtMontoOperacion.Attributes.Add("onblur", "return FormatoMonedaOperacion('" + txtMontoOperacion.ClientID + "','" + txtMontoOperacionCLP.ClientID + "','" + ddlTipoMoneda.ClientID + "');");

            //txtHorizonte.Attributes.Add("onblur", "return FormatoMoneda('" + txtHorizonte.ClientID + "');");
            txtMontoCuoton.Attributes.Add("onblur", "return FormatoMoneda('" + txtMontoCuoton.ClientID + "');");

            txtMontoCuota.Attributes.Add("onblur", "return FormatoMonedaOperacionTodo('" + txtMontoCuota.ClientID + "','" + ddlTipoMoneda.ClientID + "');");
            ddlTipoMoneda.Attributes.Add("onchange", "return calculoOperacionCLP('" + txtMontoOperacion.ClientID + "','" + txtMontoOperacionCLP.ClientID + "','" + ddlTipoMoneda.ClientID + "');");

            txtMontoSeguroIncendio.Attributes.Add("onblur", "return FormatoMoneda('" + txtMontoSeguroIncendio.ClientID + "');");
            txtMontoSeguroDesgravamen.Attributes.Add("onblur", "return FormatoMoneda('" + txtMontoSeguroDesgravamen.ClientID + "');");

            txtMontoComision.Attributes.Add("onblur", "return FormatoMoneda('" + txtMontoComision.ClientID + "');");

            txtMontoOperacion.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtMontoOperacion.ClientID + "');");
            //txtHorizonte.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtHorizonte.ClientID + "');");

            txtCostoFondo.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtCostoFondo.ClientID + "');");
            txtMontoCuota.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtMontoCuota.ClientID + "');");
            txtMontoCuoton.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtMontoCuoton.ClientID + "');");
            txtMontoSeguroIncendio.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtMontoSeguroIncendio.ClientID + "');");
            txtMontoSeguroDesgravamen.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtMontoSeguroDesgravamen.ClientID + "');");
            txtMontoComision.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txtMontoComision.ClientID + "');");

            txtPorcComision.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txtPorcComision.ClientID + "');");
            txtTasaInteres.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txtTasaInteres.ClientID + "');");
            btnGuardar.OnClientClick = "return asignacionHdnMontoOPCLP('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

            //btnGuardar.OnClientClick = "return FormatoMonedaOperacion('" + txtMontoOperacion.ClientID + "','" + txtMontoOperacionCLP.ClientID + "','" + ddlTipoMoneda.ClientID + "');";

            TxtMontoRecuperoFogape.Attributes.Add("onblur", "return FormatoMoneda('" + TxtMontoRecuperoFogape.ClientID + "');");

            if (objresumen.area == "Riesgo")
            {
                //OJO ckbFogape.Attributes.Add("onclick", "return valMaximo(this,'" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');");
            }
        }

        private DataTable CargarDatosFondo(int IdFondo)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtFondos = Ln.ListarFondos(IdFondo);
            return dtFondos;
        }

        public bool validarFondo(bool cambioFondo)
        {
            if (ddlFondo.SelectedValue == "0")
            {
                txtCostoFondo.Text = "0";
                return true;
            }
            LogicaNegocio Ln = new LogicaNegocio();
            string menj = String.Empty;
            asignacionResumen(ref objresumen);
            DataTable dt = new DataTable();
            DataTable fondo = new DataTable();
            DataTable datosFondo = CargarDatosFondo(Convert.ToInt32(ddlFondo.SelectedValue));

            if (string.IsNullOrEmpty(txtMontoComision.Text))
                txtMontoComision.Text = "0";

            txtCostoFondo.Text = ddlFondo.SelectedValue == "6"
                                        ? (txtMontoComision.Text.GetValorFloat() * (0.005 / (txtPorcComision.Text.Trim().GetValorDouble() / 100))).ToString().GetFormatearNumero(0)
                                        : ((datosFondo.Rows[0]["CostoFondo"].ToString().GetValorFloat() / 100) * txtMontoComision.Text.GetValorFloat()).ToString().GetFormatearNumero(0);

            var a = txtMontoComision.Text.GetValorDouble();
            var b = datosFondo.Rows[0]["CostoFondo"].ToString().GetValorFloat2();
            var c = (txtPorcComision.Text.GetValorDouble() / 100);

            var d = (txtMontoComision.Text.GetValorFloat() * (datosFondo.Rows[0]["CostoFondo"].ToString().GetValorFloat2() / (txtPorcComision.Text.GetValorDouble() / 100))).ToString().GetFormatearNumero(0);

            txtCostoFondo.Text = ddlFondo.SelectedValue == "6"
                            ? (txtMontoComision.Text.GetValorFloat() * ((datosFondo.Rows[0]["CostoFondo"].ToString().GetValorFloat2()) * (txtPorcComision.Text.GetValorDouble() / 100))).ToString().GetFormatearNumero(0)
                            : (txtMontoComision.Text.GetValorFloat() * ((datosFondo.Rows[0]["CostoFondo"].ToString().GetValorFloat2() / 100) * (txtPorcComision.Text.GetValorFloat() / 100))).ToString().GetFormatearNumero(0);

            dt = Ln.ValidarFondos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idPAF.ToString(),
                objresumen.idUsuario, objresumen.descCargo, datosFondo, txtCostoFondo.Text.GetValorFloat(), Convert.ToInt32(ddlFondo.SelectedValue));

            //modificar estructura de ncertificado si cambian el fondo
            if (objresumen.desEtapa.ToLower() == "cierre" && cambioFondo)
            {
                //actualizar fondo en operacion
                Ln.ModificarFondoOperacion(objresumen.idOperacion, ddlFondo.SelectedItem.Text.Trim(), int.Parse(ddlFondo.SelectedValue), txtCostoFondo.Text.Trim().GetValorDouble());
                cargarDatosOperacion();
            }

            fondo = Ln.api_LicitacionFogape(ddlFondo.SelectedValue);

            if (fondo.Rows.Count > 0)
            {
                HdfLicitacionFogape.Value = fondo.Rows[0]["NroLicitacion"].ToString();
                HdfTasaComisionAnual.Value = fondo.Rows[0]["ComisionAnual"].ToString();
            }

            if (dt.Rows.Count > 0)
            {
                string mensaje = dt.Rows[0]["Mensaje"].ToString();
                if (mensaje != "EXITO" && mensaje != "")
                {
                    menj = mensaje;
                }
                else
                    return true;
            }

            if (menj == "")
            {
                menj = "No hay datos para realizar la validación FOGAPE";
            }
            return false;
        }

        protected string ListboxToString(ListBox lbox)
        {
            string items = String.Empty;
            var Seleccionados = from ListItem item in lbox.Items
                                where item.Selected
                                select item;

            foreach (ListItem item in Seleccionados)
            {
                items += items == string.Empty ? item.Text : "," + item.Text;
            }
            return items;
        }

        //private void CalcularCostoFondo()
        //{
        //    Dictionary<string, string> datosFondo = new Dictionary<string, string>();
        //    datosFondo = CargarDatosFondo(ddlFondo.SelectedValue);

        //    if (datosFondo.Count > 0)
        //    {
        //        if (string.IsNullOrEmpty(txtMontoComision.Text))
        //            txtMontoComision.Text = "0";

        //        if (string.IsNullOrEmpty(txtPorcComision.Text))
        //            txtPorcComision.Text = "0";

        //        float costoFondo = float.Parse(datosFondo["Porcentaje"]) / 100;

        //        txtCostoFondo.Text = ddlFondo.SelectedValue == "6"
        //                                    ? (float.Parse(txtMontoComision.Text.Replace(".", "").Replace(",", ".")) * (0.005 / ((float.Parse(txtPorcComision.Text.Replace(".", "").Replace(",", "."))) / 100))).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))
        //                                    : (costoFondo * float.Parse(txtMontoComision.Text.Replace(".", "").Replace(",", "."))).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));
        //    }
        //}

        #endregion

    }
}

