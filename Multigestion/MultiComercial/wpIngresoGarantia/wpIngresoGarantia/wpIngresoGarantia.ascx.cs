using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using MultigestionUtilidades;
using System.Xml;
using System.Xml.Linq;
using Microsoft.SharePoint.WebControls;
using System.Globalization;
using AjaxControlToolkit;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpIngresoGarantia.wpIngresoGarantia
{
    [ToolboxItemAttribute(false)]
    public partial class wpIngresoGarantia : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpIngresoGarantia()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string estado = string.Empty;
        private static string pagina = "MantenedorGarantias.aspx";
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            //LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                //ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
                Page.Session["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            ////PERMISOS USUARIOS
            string PermisoConfigurado = string.Empty;
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
                    if (Page.Session["RES"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        divAvales.Visible = false;

                        AsignacionesJS();
                        Page.Session["RESUMEN"] = null;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;

                        //ValidarRut
                        btnRUTAval.OnClientClick = "return ValidarRutAval('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                        btnRutGarante.OnClientClick = "return ValidarRutGarante('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

                        if (objresumen.area.Trim() != "Fiscalia")
                            lbOperacion.Text = objresumen.desOperacion.ToString();
                        else
                            lbOperaciones.Visible = false;

                        pnOperaciones.Style.Add("display", "none");
                        pnSeguro.Style.Add("display", "none");
                        pnTasacion.Style.Add("display", "block");

                        hddAjuste.Value = "0";
                        txtValorA.Enabled = false;

                        CargarListas();
                        if (objresumen.area.Trim() == "Operaciones")
                        {
                            pnOperaciones.Style.Add("display", "block");
                            pnSeguro.Style.Add("display", "block");
                        }
                        if (objresumen.linkActual != "PosicionCliente.aspx")
                        {
                            //Verificación Edicion Simultanea        
                            string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IngresoGarantia", "Operacion");
                            if (!string.IsNullOrEmpty(UsuarioFormulario))
                            {
                                warning("Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.");
                                pnSeguro.Style.Add("display", "none");
                            }
                        }
                        else
                        {
                            lbOperaciones.Visible = false;
                            btnLimpiar.Visible = false;
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }

                    hdnArea.Value = dt.Rows[0]["descCargo"].ToString();

                    LblInscripcion.Text = "N° Inscripción";
                    cargarCertificadosEmitir();
                    cargarGvContribuciones();

                    //formatearNroInscripcion(false);
                }
                llenargrid();
                //CargarTasacionHistorica();
                //CargarSegurosHistorico();
                ValidarAlertasGarantias();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    warning1("Usuario sin permisos configurados");
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                warning1("Usuario sin permisos configurados");
            }
        }

        protected void ValidarAlertasGarantias()
        {
            //validar alertas de garantias(contribuciones, fechas tasaciones)
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            string mensaje = string.Empty;
            DataTable dtProblemas = new DataTable("dtProblemas");
            dtProblemas = Ln.ConsultaValidacionesEmpresa(objresumen.idEmpresa.ToString(), 3);
            mensaje = dtProblemas.Rows[0][0].ToString();

            if (mensaje != "OK")
            {
                warning(util.Mensaje(mensaje.Trim()));
            }
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

        private void warning1(string mensaje)
        {
            dvWarning1.Style.Add("display", "block");
            lbWarning1.Text = mensaje;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);

            //VALIDAR DATOS OBLIGATORIOS
            string TipoBienes, Provincia, Comuna, NumPoliza, Contratante, CompaniaSeguro = string.Empty;
            bool ReqSeguro = false;
            TipoBienes = ddlTipoBienes.SelectedItem == null ? "" : ddlTipoBienes.SelectedItem.Text.Trim();
            Provincia = ddlProvincia.SelectedItem == null ? "" : ddlProvincia.SelectedItem.Text.Trim();
            Comuna = ddlComunas.SelectedItem == null ? "" : ddlComunas.SelectedItem.Text.Trim();
            //ReqSeguro = hdnChk.Value == "false" ? false : true;
            ReqSeguro = ckbSeguro.Checked;
            NumPoliza = txtPoliza.Text.Trim();
            Contratante = ddlContratante.SelectedItem == null ? "" : ddlContratante.SelectedItem.Text.Trim();
            CompaniaSeguro = ddlCompaniaSeguro.SelectedItem == null ? "" : ddlCompaniaSeguro.SelectedItem.Text.Trim();

            if (objresumen.area.Trim() == "Riesgo")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            }
            else if (objresumen.area.Trim() == "Operaciones")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");
            }
            if (txtValorC.Text != "")
            {
                if (hddAjuste.Value != "0")
                {
                    txtValorA.Text = ((txtValorC.Text.Trim().GetValorDouble()) - (txtValorC.Text.Trim().GetValorDouble() * (float.Parse(hddAjuste.Value)) / 100)).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                    txtValorA.Enabled = false;
                }
                else
                {
                    txtValorA.Text = txtValorC.Text;
                    txtValorA.Enabled = false;
                }
            }
            string Fogape = string.Empty;
            Fogape = ddlTipoBienes.SelectedItem == null ? "" : ddlTipoBienes.SelectedItem.Text.Trim();
            if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR && Fogape.Contains("FOGAPE"))
            {
                warning("No se puede guardar la garantía FOGAPE. Por favor realizar esta acción desde el formulario de Operación");
            }
            else
            {
                try
                {
                    if (objresumen.area.Trim() == "Comercial" || objresumen.area.Trim() == "Riesgo" || objresumen.area.Trim() == "Fiscalia")
                    {
                        string mensaje = Ln.ValidarGarantia(objresumen.idEmpresa, objresumen.idOperacion, objresumen.area, ddlTipoG.SelectedItem.Text.Trim(), TipoBienes, txtNroInscripcion.Text.Trim(), ddlRegiones.SelectedItem.Text.Trim(), Provincia, Comuna, txtDescP.Text.Trim(), txtValorC.Text.Trim(), txtValorA.Text.Trim(), txtValorAseg.Text.Trim(), LblInscripcion.Text.Trim(), ReqSeguro, NumPoliza, Contratante, CompaniaSeguro);
                        if (mensaje.Contains("OK"))
                            Guardar();
                        else
                            error(mensaje);
                    }
                    if (objresumen.area.Trim() == "Operaciones" || objresumen.area.Trim() == "Contabilidad")
                    {
                        if (txtMontoLimite.Text == "" || txtMontoLimite.Text == "0")
                        {
                            txtMontoLimite.Text = txtValorA.Text;
                        }
                        if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                        {
                            string mensaje = Ln.ValidarGarantia(objresumen.idEmpresa, objresumen.idOperacion, objresumen.area, ddlTipoG.SelectedItem.Text.Trim(), TipoBienes, txtNroInscripcion.Text.Trim(), ddlRegiones.SelectedItem.Text.Trim(), Provincia, Comuna, txtDescP.Text.Trim(), txtValorC.Text.Trim(), txtValorA.Text.Trim(), txtValorAseg.Text.Trim(), LblInscripcion.Text.Trim(), ReqSeguro, NumPoliza, Contratante, CompaniaSeguro);
                            if (mensaje.Contains("OK"))
                                Guardar();
                            else
                                error(mensaje);
                        }
                        if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                        {
                            string mensaje = Ln.ValidarGarantia(objresumen.idEmpresa, objresumen.idOperacion, objresumen.area, ddlTipoG.SelectedItem.Text.Trim(), TipoBienes, txtNroInscripcion.Text.Trim(), ddlRegiones.SelectedItem.Text.Trim(), Provincia, Comuna, txtDescP.Text.Trim(), txtValorC.Text.Trim(), txtValorA.Text.Trim(), txtValorAseg.Text.Trim(), LblInscripcion.Text.Trim(), ReqSeguro, NumPoliza, Contratante, CompaniaSeguro);
                            if (mensaje.Contains("OK"))
                                Actualizar(ViewState["GarantiaID"].ToString());
                            else
                                error(mensaje);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    error("Error al guardar la garantía");
                }
            }
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (objresumen.linkActual != "PosicionCliente.aspx")
                {
                    e.Row.Cells[8].Visible = true;
                }
            }

            if (e.Row.RowIndex > -1)
            {
                if (int.Parse(objresumen.idPermiso.ToString()) == Constantes.PERMISOS.SOLOLECTURA)
                {
                    if (objresumen.linkActual != "PosicionCliente.aspx")
                    {
                        e.Row.Cells[8].Visible = true;
                    }
                }
            }
        }

        protected void ResultadosBusquedaAlzada_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            asignacionResumen(ref objresumen);

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (objresumen.linkActual != "PosicionCliente.aspx")
                {
                    e.Row.Cells[8].Visible = true;
                }
            }

            if (e.Row.RowIndex > -1)
            {
                if (int.Parse(objresumen.idPermiso.ToString()) == Constantes.PERMISOS.SOLOLECTURA)
                {
                    if (objresumen.linkActual != "PosicionCliente.aspx")
                    {
                        e.Row.Cells[8].Visible = true;
                    }
                }
            }
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Formato valor ajustado
                e.Row.Cells[5].Text = Convert.ToDecimal(e.Row.Cells[5].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                //formato valor Comercial
                e.Row.Cells[6].Text = Convert.ToDecimal(e.Row.Cells[6].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                //formato valor asegurable
                if (!string.IsNullOrWhiteSpace(e.Row.Cells[7].Text) && !string.IsNullOrEmpty(e.Row.Cells[7].Text))
                    e.Row.Cells[7].Text = Convert.ToDecimal(e.Row.Cells[7].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = e.Row.Cells[0].Text + "/" + e.Row.RowIndex.ToString();
                lb.ToolTip = "Editar Garantía";
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusqueda_Command;
                e.Row.Cells[8].Controls.Add(lb);

                if (objresumen.linkActual != "PosicionCliente.aspx")
                {
                    LinkButton lb2 = new LinkButton();
                    lb2.CssClass = ("fa fa-close");
                    lb2.CommandName = "Eliminar";
                    lb2.ToolTip = "Eliminar Garantía";
                    lb2.CommandArgument = e.Row.Cells[0].Text;
                    lb2.OnClientClick = "return Dialogo();";
                    lb2.Command += ResultadosBusqueda_Command;

                    e.Row.Cells[8].Controls.Add(lb2);
                }
            }
        }

        protected void ResultadosBusquedaAlzada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Formato valor ajustado
                e.Row.Cells[5].Text = Convert.ToDecimal(e.Row.Cells[5].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                //formato valor Comercial
                e.Row.Cells[6].Text = Convert.ToDecimal(e.Row.Cells[6].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                //formato valor asegurable
                if (!string.IsNullOrWhiteSpace(e.Row.Cells[7].Text) && !string.IsNullOrEmpty(e.Row.Cells[7].Text))
                    e.Row.Cells[7].Text = Convert.ToDecimal(e.Row.Cells[7].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = e.Row.Cells[0].Text + "/" + e.Row.RowIndex.ToString();
                lb.ToolTip = "Editar Garantía";
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusquedaAlzada_Command;
                e.Row.Cells[8].Controls.Add(lb);

                if (objresumen.linkActual != "PosicionCliente.aspx")
                {
                    LinkButton lb2 = new LinkButton();
                    lb2.CssClass = ("fa fa-close");
                    lb2.CommandName = "Eliminar";
                    lb2.ToolTip = "Eliminar Garantía";
                    lb2.CommandArgument = e.Row.Cells[0].Text;
                    lb2.OnClientClick = "return Dialogo();";
                    lb2.Command += ResultadosBusquedaAlzada_Command;

                    e.Row.Cells[8].Controls.Add(lb2);
                }
            }
        }

        protected void ResultadosBusquedaAlzada_Command(object sender, CommandEventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                if (e.CommandName == "Eliminar")
                {
                    LogicaNegocio Ln = new LogicaNegocio();
                    if (Ln.EliminarGarantias(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), e.CommandArgument.ToString(), "1", objresumen.idUsuario.ToString(), objresumen.descCargo.ToString()))
                    {
                        succes(Ln.buscarMensaje(Constantes.MENSAJES.EXITOELIMEMSOCIOS));
                        llenargrid();
                        vaciar();
                    }
                    else
                        error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORELIMEMPRESARELAC));
                }

                if (e.CommandName == "Editar")
                {
                    string[] parametros = e.CommandArgument.ToString().Split('/');
                    string idGarantia = parametros[0];
                    int index = int.Parse(parametros[1].ToString());

                    ResultadosBusquedaAlzada.Rows[index].CssClass = ("alert alert-info");
                    estado = "editar";
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    ViewState["GarantiaID"] = idGarantia;

                    CargarDatosGarantia(idGarantia);
                    cargarCertificadosEmitir();
                    cargarGvContribuciones();
                    CargarTasacionHistorica();
                    CargarSegurosHistorico();
                }
            }
            catch
            {
            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                if (e.CommandName == "Eliminar")
                {
                    LogicaNegocio Ln = new LogicaNegocio();
                    if (Ln.EliminarGarantias(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), e.CommandArgument.ToString(), "1", objresumen.idUsuario.ToString(), objresumen.descCargo.ToString()))
                    {
                        succes(Ln.buscarMensaje(Constantes.MENSAJES.EXITOELIMEMSOCIOS));
                        llenargrid();
                        vaciar();
                    }
                    else
                        error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORELIMEMPRESARELAC));
                }

                if (e.CommandName == "Editar")
                {
                    //Paneles(true);
                    Control divFormulario = this.FindControl("dvFormulario");
                    util.Limpiar(divFormulario);
                    string[] parametros = e.CommandArgument.ToString().Split('/');
                    string idGarantia = parametros[0];
                    int index = int.Parse(parametros[1].ToString());

                    ResultadosBusqueda.Rows[index].CssClass = ("alert alert-info");
                    estado = "editar";
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    ViewState["GarantiaID"] = idGarantia;
                    CargarDatosGarantia(idGarantia);
                    cargarCertificadosEmitir();
                    cargarGvContribuciones();
                    CargarTasacionHistorica();
                    CargarSegurosHistorico();
                }
            }
            catch (Exception ex)
            {
                warning(string.Format("{0}{1}", "error al cargar el detallde de la garantia ", ex.Message));
            }
        }

        protected void ddlParticipacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            // CargarEmpresaCompartida(ddlParticipacion.SelectedItem.Text);
        }

        protected void ddlRegiones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegiones.SelectedItem.Value != "0")
                CargarProvincia(int.Parse(ddlRegiones.SelectedItem.Value));
            //CargarProvincia(ddlRegiones.SelectedValue);
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarComuna(int.Parse(ddlProvincia.SelectedItem.Value));
            //CargarComuna(ddlProvincia.SelectedValue);
        }

        protected void ddlEmpresaT_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTasador(ddlEmpresaT.SelectedValue);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);

            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IngresoGarantia");
                Page.Session["Resumen"] = objresumen;
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

            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Session["Resumen"] = objresumen;
            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IngresoGarantia");
                Page.Response.Redirect(objresumen.linkPrincial);
            }
            else
            {
                Page.Session["Resumen"] = objresumen;
                Page.Response.Redirect(objresumen.linkActual);
            }
        }

        protected void lbGarantias_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);

            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Session["Resumen"] = objresumen;
            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IngresoGarantia");
            }
            Page.Response.Redirect("MantenedorGarantias.aspx");
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);

            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Session["Resumen"] = objresumen;
            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IngresoGarantia");
            }
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
        }

        protected void lbOperacion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);

            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Session["Resumen"] = objresumen;
            if (objresumen.linkActual != "PosicionCliente.aspx")
            {
                vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IngresoGarantia");
            }
            Page.Response.Redirect("DatosOperacion.aspx");
        }

        protected void ddlTipoBienes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTipoBienValores(ddlTipoBienes.SelectedValue.ToString());
            if (ddlTipoBienes.SelectedValue.ToString() == "88")
            {
                divAvales.Visible = true;
            }
            else
            {
                divAvales.Visible = false;
            }

            CargarTipoIdentificador(ddlTipoBienes.SelectedValue.ToString());

            //if (ddlTipoBienes.SelectedItem != null)
            //{
            //    if (ddlTipoBienes.SelectedItem.Text == "Casas")
            //        formatearNroInscripcion(true);
            //    else
            //        formatearNroInscripcion(false);
            //}
        }

        protected void btnRutGarante_Click(object sender, EventArgs e)
        {
            DataTable res;
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.buscarDatosRut(txt_RutGarante.Text.Replace(".", "") + txt_DivRutGarante.Text);
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    txt_RazonSocialGarante.Text = res.Rows[0]["RazonSocial"].ToString();
                    ViewState["IdEmpresaGarante"] = res.Rows[0]["IdEmpresa"].ToString();
                }
                else
                    txt_RazonSocialGarante.Text = "";
            }
        }

        protected void txtValorC_TextChanged1(object sender, EventArgs e)
        {
            CargarTipoBienValores(ddlTipoBienes.SelectedValue.ToString());
        }

        protected void btnRUTAval_Click(object sender, EventArgs e)
        {
            DataTable res;
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.buscarDatosRut(txt_RUTAval.Text.Replace(".", "") + txt_DivAval.Text);
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    txtRazonSocialAval.Text = res.Rows[0]["RazonSocial"].ToString();
                    ViewState["IdEmpresaAval"] = res.Rows[0]["IdEmpresa"].ToString();
                }
                else
                    txtRazonSocialAval.Text = "";
            }
        }

        protected void GvTasacionHistorial_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GvTasacionHistorial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                asignacionResumen(ref objresumen);

                if (objresumen.linkActual != "PosicionCliente.aspx")
                {
                    string nombredoc = e.Row.Cells[11].Text.Trim();

                    if (!string.IsNullOrEmpty(nombredoc) && nombredoc != "&nbsp;")
                    {
                        LinkButton lb1 = new LinkButton();
                        lb1.CssClass = ("fa fa-download paddingIconos");
                        lb1.CommandName = "Descargar";
                        lb1.ToolTip = "Descargar Archivo";
                        lb1.CommandArgument = e.Row.RowIndex.ToString();
                        lb1.Command += GvTasacionHistorial_Command;
                        e.Row.Cells[9].Controls.Add(lb1);
                    }
                    else
                    {
                        FileUpload fup = new FileUpload();
                        fup.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        fup.ID = "FupTasacion";
                        fup.AllowMultiple = false;
                        fup.Attributes["onChange"] = "ValidarFup('" + e.Row.RowIndex + "');";  //"ValidarFup('" + e.Row.Cells[1].Text + "');";
                        fup.ToolTip = "Adjuntar Tasación";
                        fup.Width = 180;
                        e.Row.Cells[9].Controls.Add(fup);

                        LinkButton lb = new LinkButton();
                        //Button lb = new Button();
                        lb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                        lb.ID = "LbAgregar";
                        lb.Text = "Guardar";
                        lb.Width = 80;
                        lb.CssClass = ("btn btn-success pull-right"); // ("glyphicon glyphicon-plus-sign paddingIconos"); //
                        lb.CommandName = "Agregar";
                        lb.CommandArgument = e.Row.RowIndex.ToString(); //e.Row.Cells[0].Text;
                        lb.OnClientClick = "return Dialogo();";
                        lb.Command += GvTasacionHistorial_Command;
                        lb.ToolTip = "Agregar archivo Tasación";
                        e.Row.Cells[9].Controls.Add(lb);
                        lb.Attributes.Add("style", "display:none");
                    }

                    LinkButton lb3 = new LinkButton();
                    lb3.CssClass = ("fa fa-edit paddingIconos");
                    lb3.CommandName = "Editar";
                    lb3.ToolTip = "Editar Tasación";
                    lb3.CommandArgument = e.Row.RowIndex.ToString();
                    lb3.OnClientClick = "return Dialogo();";
                    lb3.Command += GvTasacionHistorial_Command;
                    e.Row.Cells[10].Controls.Add(lb3);

                    LinkButton lb2 = new LinkButton();
                    lb2.CssClass = ("fa fa-close");
                    lb2.CommandName = "Eliminar";
                    lb2.ToolTip = "Eliminar Tasación";
                    lb2.CommandArgument = e.Row.RowIndex.ToString();
                    lb2.OnClientClick = "return Dialogo();";
                    lb2.Command += GvTasacionHistorial_Command;
                    e.Row.Cells[10].Controls.Add(lb2);
                }
            }
        }

        protected void GvTasacionHistorial_Command(object sender, CommandEventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            try
            {
                if (e.CommandName == "Eliminar")
                {
                    if (ActualizarDocTasacionHistorica(e.CommandArgument.ToString(), "0", objresumen, "", "3"))
                    {
                        succes("Tasación Eliminada");
                        CargarTasacionHistorica();
                    }
                    else
                        error("Error al eliminar tasación");
                }

                //adjuntar tasacion
                if (e.CommandName == "Agregar")
                {
                    int index1 = Convert.ToInt32(e.CommandArgument);
                    string nombreCarpetaRaiz = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());

                    FileUpload Fup = (GvTasacionHistorial.Rows[index1].FindControl("FupTasacion") as FileUpload);
                    string IdTasacion = System.Web.HttpUtility.HtmlDecode(GvTasacionHistorial.Rows[index1].Cells[1].Text.ToString()).Trim();
                    string IdGarantia = "0"; System.Web.HttpUtility.HtmlDecode(GvTasacionHistorial.Rows[index1].Cells[2].Text.ToString()).Trim();

                    //bool ingreso = cargarDocumento(nombreCarpetaRaiz, "COMERCIAL", objresumen.idOperacion.ToString(), objresumen, Fup, "Tasación", objresumen.idOperacion.ToString(), "");
                    if (new Documentos { }.cargarDocumento(nombreCarpetaRaiz, "COMERCIAL", objresumen.idOperacion.ToString(), objresumen, Fup, "Tasación", objresumen.idOperacion.ToString(), ""))
                        if (ActualizarDocTasacionHistorica(IdTasacion, IdGarantia, objresumen, Fup.FileName.Trim(), "2"))
                        {
                            succes("Tasación Ingresada");
                        }

                    CargarTasacionHistorica();
                }

                if (e.CommandName == "Descargar")
                {
                    int index1 = Convert.ToInt32(e.CommandArgument);
                    string NombreDoc = Ln.DescargarNombreDoc(index1); //System.Web.HttpUtility.HtmlDecode(GvTasacionHistorial.Rows[index1 - 1].Cells[11].Text.ToString()).Trim();
                    string pathArchivo = util.DescargarArchivo(NombreDoc.Trim(), "COMERCIAL", objresumen.idOperacion.ToString(), lbEmpresa.Text.Trim(), objresumen.rut);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + pathArchivo + "','_blank')", true);
                }

                if (e.CommandName == "Editar")
                {
                    vaciar();
                    string[] parametros = e.CommandArgument.ToString().Split('/');
                    int idTasacion = int.Parse(parametros[0]);
                    int index = int.Parse(parametros[1].ToString());
                    ViewState["idTasacion"] = idTasacion;
                    CargarTabTasacion(idTasacion);
                }
            }
            catch
            {
            }
        }

        protected void btnGuardarTasacion_Click(object sender, EventArgs e)
        {
            //TAB TASACION
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            SPWeb app2 = SPContext.Current.Web;

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;

            //VALIDAR DATOS OBLIGATORIOS
            string TipoBienes, Provincia, Comuna, NumPoliza, Contratante, CompaniaSeguro = string.Empty;
            bool ReqSeguro = false;
            TipoBienes = ddlTipoBienes.SelectedItem == null ? "" : ddlTipoBienes.SelectedItem.Text.Trim();
            Provincia = ddlProvincia.SelectedItem == null ? "" : ddlProvincia.SelectedItem.Text.Trim();
            Comuna = ddlComunas.SelectedItem == null ? "" : ddlComunas.SelectedItem.Text.Trim();
            ReqSeguro = ckbSeguro.Checked;
            NumPoliza = txtPoliza.Text.Trim();
            Contratante = ddlContratante.SelectedItem == null ? "" : ddlContratante.SelectedItem.Text.Trim();
            CompaniaSeguro = ddlCompaniaSeguro.SelectedItem == null ? "" : ddlCompaniaSeguro.SelectedItem.Text.Trim();

            string mensaje = Ln.ValidarGarantia(objresumen.idEmpresa, objresumen.idOperacion, objresumen.area, ddlTipoG.SelectedItem.Text.Trim(), TipoBienes, txtNroInscripcion.Text.Trim(), ddlRegiones.SelectedItem.Text.Trim(), Provincia, Comuna, txtDescP.Text.Trim(), txtValorC.Text.Trim(), txtValorA.Text.Trim(), txtValorAseg.Text.Trim(), LblInscripcion.Text.Trim(), ReqSeguro, NumPoliza, Contratante, CompaniaSeguro);
            if (mensaje.Contains("OK"))
            {
                RespNode = doc.CreateElement("Val");
                RespNode = xmlTab(pnTasacion, doc, RespNode);
                RespNode = xmlTab(pnGarantia, doc, RespNode);

                ValoresNode.AppendChild(RespNode);
                int IdGarantia = 0;
                int idTasacion = 0;

                if (ViewState["idTasacion"] != null)
                    idTasacion = Convert.ToInt32(ViewState["idTasacion"]);
                if (ViewState["GarantiaID"] != null)
                    IdGarantia = Convert.ToInt32(ViewState["GarantiaID"]);

                bool ok = Ln.GuardarDatosHistoriaTasacion(IdGarantia, doc, objresumen.idEmpresa, objresumen.idOperacion, util.ObtenerValor(app2.CurrentUser.Name), idTasacion, 5);
            }
            else
                error(mensaje);
        }

        protected void btnGuardarSeguro_Click(object sender, EventArgs e)
        {
            //TAB SEGURO
            LogicaNegocio Ln = new LogicaNegocio();

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;

            RespNode = doc.CreateElement("Val");
            RespNode = xmlTab(pnSeguro, doc, RespNode);

            ValoresNode.AppendChild(RespNode);
            int IdGarantia = 0;
            int IdSeguro = 0;

            if (ViewState["idSeguro"] != null)
                IdSeguro = Convert.ToInt32(ViewState["idSeguro"]);
            if (ViewState["GarantiaID"] != null)
                IdGarantia = Convert.ToInt32(ViewState["GarantiaID"]);

            bool ok = Ln.GuardarDatosHistoriaSeguros(IdGarantia, doc, "", "", IdSeguro, 5);
            if (ok)
            {
                CargarSegurosHistorico();
                succes("Seguro Ingresado");
            }
            else
            {

            }
        }

        protected void ddlEdoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarRegimen(ddlEdoCivil.SelectedValue);
        }

        protected void GvHistorialSeguros_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }

        protected void GvHistorialSeguros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb3 = new LinkButton();
                lb3.CssClass = ("fa fa-edit paddingIconos");
                lb3.CommandName = "Editar";
                lb3.ToolTip = "Editar Seguro";
                lb3.CommandArgument = e.Row.Cells[0].Text + "/" + e.Row.RowIndex.ToString();
                lb3.OnClientClick = "return Dialogo();";
                lb3.Command += GvHistorialSeguros_Command;
                e.Row.Cells[9].Controls.Add(lb3);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.ToolTip = "Eliminar Seguro";
                lb2.CommandArgument = e.Row.RowIndex.ToString();
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += GvHistorialSeguros_Command;
                e.Row.Cells[9].Controls.Add(lb2);
            }
        }

        protected void GvTasacionHistorial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvTasacionHistorial.PageIndex = e.NewPageIndex;
            CargarTasacionHistorica();
        }

        protected void GvHistorialSeguros_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GvHistorialSeguros.PageIndex = e.NewPageIndex;
            CargarSegurosHistorico();
        }

        protected void gvContribuciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Formato valor cuota plazo
                e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                //formato total a pagar
                e.Row.Cells[6].Text = Convert.ToDecimal(e.Row.Cells[6].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));

                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].ForeColor = Color.Black;
                e.Row.Cells[1].Font.Bold = true;

                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].ForeColor = Color.Black;
                e.Row.Cells[2].Font.Bold = true;
            }
        }

        protected void gvContribuciones_DataBound(object sender, EventArgs e)
        {
            for (int rowIndex = gvContribuciones.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = gvContribuciones.Rows[rowIndex];
                GridViewRow gvPreviousRow = gvContribuciones.Rows[rowIndex + 1];
                for (int cellCount = 1; cellCount < 2; cellCount++)   //cellCount = celda que no se quiere repetir
                {
                    if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                    {
                        if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount].RowSpan = 2;
                            gvRow.Cells[1].RowSpan = 2;
                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                            gvRow.Cells[1].RowSpan = gvPreviousRow.Cells[1].RowSpan + 1;
                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;
                    }
                }

                for (int cellCount = 2; cellCount < 3; cellCount++)   //cellCount = celda que no se quiere repetir
                {
                    if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                    {
                        if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount].RowSpan = 2;
                            gvRow.Cells[1].RowSpan = 2;
                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                            gvRow.Cells[1].RowSpan = gvPreviousRow.Cells[1].RowSpan + 1;
                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;
                    }
                }

                gvPreviousRow.Cells[1].VerticalAlign = VerticalAlign.Middle;
                gvPreviousRow.Cells[1].ForeColor = Color.Black;
                gvPreviousRow.Cells[1].Font.Bold = true;
            }
        }

        protected void GvHistorialSeguros_Command(object sender, CommandEventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                LogicaNegocio LN = new LogicaNegocio();

                if (e.CommandName == "Eliminar")
                {

                }

                if (e.CommandName == "Editar")
                {
                    vaciar();
                    string[] parametros = e.CommandArgument.ToString().Split('/');
                    int idSeguro = int.Parse(parametros[0]);
                    int index = int.Parse(parametros[1].ToString());
                    ViewState["idSeguro"] = idSeguro;
                    CargarTabSeguro(idSeguro);
                    //GvHistorialSeguros.Rows[index].CssClass = ("alert alert-info");          
                }
            }
            catch
            {
            }
        }

        #endregion


        #region Metodos

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataSet res;
                res = Ln.ListaGarantias(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo);
                SumarGrid(res.Tables[0]);
                SumarGidAlzadas(res.Tables[1]);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
            }
        }

        private void CargarTasacionHistorica()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("dt");

            if (ViewState["GarantiaID"] == null)
                ViewState["GarantiaID"] = "0";

            dt = Ln.CargarDatosTasacionGarantias(Convert.ToInt32(ViewState["GarantiaID"]), null, objresumen.idEmpresa, objresumen.idOperacion, "", 0, 3);
            GvTasacionHistorial.DataSource = dt;
            GvTasacionHistorial.DataBind();
        }

        private void CargarSegurosHistorico()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("dt");

            if (ViewState["GarantiaID"] == null)
                ViewState["GarantiaID"] = "0";

            dt = Ln.CargarDatosHistoriaSeguros(Convert.ToInt32(ViewState["GarantiaID"]), "", "", "", 0, 3);
            GvHistorialSeguros.DataSource = dt;
            GvHistorialSeguros.DataBind();
        }

        private void SumarGidAlzadas(DataTable dt)
        {
            ResultadosBusquedaAlzada.DataSource = dt;
            ResultadosBusquedaAlzada.DataBind();
            if (dt.Rows.Count > 0)
            {
                // suma de columna
                decimal totalValorAjustado = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Ajustado"));
                decimal totalValorComercial = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Comercial"));
                decimal totalValorsegurable = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Asegurable"));

                ResultadosBusquedaAlzada.FooterRow.Cells[4].Text = "Total";
                ResultadosBusquedaAlzada.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                ResultadosBusquedaAlzada.FooterRow.Cells[4].ForeColor = Color.Black;
                ResultadosBusquedaAlzada.FooterRow.Cells[4].Font.Bold = true; //= Font.Bold;

                ResultadosBusquedaAlzada.FooterRow.Cells[5].Text = totalValorAjustado.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusquedaAlzada.FooterRow.Cells[5].ForeColor = Color.Black;
                ResultadosBusquedaAlzada.FooterRow.Cells[5].Font.Bold = true;
                ResultadosBusquedaAlzada.FooterRow.Cells[6].Text = totalValorComercial.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusquedaAlzada.FooterRow.Cells[6].ForeColor = Color.Black;
                ResultadosBusquedaAlzada.FooterRow.Cells[6].Font.Bold = true;
                ResultadosBusquedaAlzada.FooterRow.Cells[7].Text = totalValorsegurable.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusquedaAlzada.FooterRow.Cells[7].ForeColor = Color.Black;
                ResultadosBusquedaAlzada.FooterRow.Cells[7].Font.Bold = true;
            }
        }

        private void SumarGrid(DataTable dt)
        {
            ResultadosBusqueda.DataSource = dt;
            ResultadosBusqueda.DataBind();
            if (dt.Rows.Count > 0)
            {
                // suma de columna
                decimal totalValorAjustado = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Ajustado"));
                decimal totalValorComercial = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Comercial"));
                decimal totalValorsegurable = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Asegurable"));

                ResultadosBusqueda.FooterRow.Cells[4].Text = "Total";
                ResultadosBusqueda.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                ResultadosBusqueda.FooterRow.Cells[4].ForeColor = Color.Black;
                ResultadosBusqueda.FooterRow.Cells[4].Font.Bold = true; //= Font.Bold;

                ResultadosBusqueda.FooterRow.Cells[5].Text = totalValorAjustado.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusqueda.FooterRow.Cells[5].ForeColor = Color.Black;
                ResultadosBusqueda.FooterRow.Cells[5].Font.Bold = true;
                ResultadosBusqueda.FooterRow.Cells[6].Text = totalValorComercial.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusqueda.FooterRow.Cells[6].ForeColor = Color.Black;
                ResultadosBusqueda.FooterRow.Cells[6].Font.Bold = true;
                ResultadosBusqueda.FooterRow.Cells[7].Text = totalValorsegurable.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusqueda.FooterRow.Cells[7].ForeColor = Color.Black;
                ResultadosBusqueda.FooterRow.Cells[7].Font.Bold = true;
            }
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            //if (ViewState["RES"] != null)
            if (Page.Session["RES"] != null)
                objresumen = (Resumen)Page.Session["RES"]; //ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void cargarCertificadosEmitir()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();

            dt = Ln.CertificadosPorEmitir(objresumen.idEmpresa);
            util.CargaDDL(ddlCertificadosEmitir, dt, "Ncertificado", "Ncertificado");
        }

        protected void AsignacionesJS()
        {
            ddlTipoG.Attributes["onChange"] = "Dialogo();";
            ddlRegiones.Attributes["onChange"] = "Dialogo();";
            ddlProvincia.Attributes["onChange"] = "Dialogo();";
            ddlEmpresaT.Attributes["onChange"] = "Dialogo();";
            btnLimpiar.OnClientClick = "return LimpiarFormulario('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
            ddlTipoBienes.Attributes["onChange"] = "Dialogo();";
            ckbSeguro.Attributes["onChange"] = "TabSeguro();";
            btnGuardarSeguro.OnClientClick = "return ValidarTabSeguro('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
        }

        protected void CargarListas()
        {
            CargarClaseGarantia();
            CargarEdoGarantia();
            CargarSubEstadoGarantia();
            CargarRegion();
            CargarEmpresaTasadora();
            CargarCompaniaSeguro();
            CargarGradoPreferencia();
            CargarCaracter();
            CargarParticipacion();
            CargarLimite();
            CargarContratante();
            CargarEstadoSeguro();
            CargarEstadoCivil();
            CargarEmpresaAcreedora();
        }

        private void Paneles(bool estado)
        {
            pnGarantia.Enabled = estado;
            pnTasacion.Enabled = estado;
            pnSeguro.Enabled = estado;
            pnConstitucion.Enabled = estado;
            pnCorfo.Enabled = estado;
            pnContribuciones.Enabled = estado;
        }

        protected void CargarDatosGarantia(string Garantia)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res;

            if (objresumen.idOperacion == -1) //Cuando viene de PosicionCliente
            {
                res = Ln.DatosGarantias(objresumen.idEmpresa.ToString(), Garantia, objresumen.idUsuario, objresumen.descCargo);
            }
            else
            {
                res = Ln.DatosGarantias(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), Garantia, objresumen.idUsuario, objresumen.descCargo);
            }

            if (res != null)
            {
                ///  ESTADO GARANTIAS
                /*3	Alzada
                 *5	Eliminada
                 *4	Tramite
                 *0	Seleccione
                 *1	Sin Estado
                 *2	Constituida
                 */

                if (objresumen.area.Trim() != "Operaciones" && objresumen.area.Trim() != "Contabilidad")
                {
                    string[] estadosGarantias = new string[] { "0", "1" };
                    if (!util.EstaPermitido(res.Rows[0]["IdEstado"].ToString(), estadosGarantias))
                    {
                        ViewState["GarantiaID"] = null;
                        warning(string.Format("{0}{1}", "la garantía no se puede modificar por estar en un estado ", res.Rows[0]["DescEstado"].ToString()));
                        //Paneles(false);
                        return;
                    }
                }


                if (res.Rows.Count > 0)
                {
                    /////////////////////////////////////// PESTAÑA GARANTIAS ///////////////////////////////////////////////////////////////////////
                    //IdGarantia
                    lbNGtia.Text = res.Rows[0]["IdGarantia"].ToString();
                    //[NroInscripcion]
                    txtNroInscripcion.Text = res.Rows[0]["NroInscripcion"].ToString();
                    //[IdTipoGarantia]--claseGarantia
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdTipoGarantia"].ToString()))
                        ddlTipoG.SelectedIndex = ddlTipoG.Items.IndexOf(ddlTipoG.Items.FindByValue(Convert.ToString(res.Rows[0]["IdTipoGarantia"])));
                    //[IdTipoBien]
                    //CargarTipoBien(ddlTipoG.SelectedItem.Text);
                    CargarTipoBien(ddlTipoG.SelectedValue);
                    if (res.Rows[0]["IdTipoBien"].ToString() != "")
                        ddlTipoBienes.SelectedIndex = ddlTipoBienes.Items.IndexOf(ddlTipoBienes.Items.FindByValue(Convert.ToString(res.Rows[0]["IdTipoBien"])));
                    CargarTipoBienValores(ddlTipoBienes.SelectedValue);
                    //rutAval				divrutAval      razonSociaAval
                    txt_DivAval.Text = res.Rows[0]["divrutAval"].ToString();
                    if (res.Rows[0]["rutAval"].ToString() != "0")
                        txt_RUTAval.Text = res.Rows[0]["rutAval"].ToString();
                    txtRazonSocialAval.Text = res.Rows[0]["razonSociaAval"].ToString();

                    if (!string.IsNullOrEmpty(res.Rows[0]["NacionalidadAval"].ToString()))
                        txtNacionalidad.Text = res.Rows[0]["NacionalidadAval"].ToString().Trim();

                    if (!string.IsNullOrEmpty(res.Rows[0]["ProfesionAval"].ToString()))
                        txtProfesion.Text = res.Rows[0]["ProfesionAval"].ToString().Trim();

                    if (!string.IsNullOrEmpty(res.Rows[0]["IdEstadoCivilAval"].ToString()))
                    {
                        ddlEdoCivil.SelectedIndex = ddlEdoCivil.Items.IndexOf(ddlEdoCivil.Items.FindByValue(Convert.ToString(res.Rows[0]["IdEstadoCivilAval"])));
                        CargarRegimen(res.Rows[0]["IdEstadoCivilAval"].ToString().Trim());
                    }

                    if (!string.IsNullOrEmpty(res.Rows[0]["IdRegimen"].ToString()))
                        ddlRegimen.SelectedIndex = ddlRegimen.Items.IndexOf(ddlRegimen.Items.FindByValue(Convert.ToString(res.Rows[0]["IdRegimen"])));

                    txtDescP.Text = res.Rows[0]["DescPrenda"].ToString();

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////// PESTAÑA TASACION /////////////////////////////////////////////////////////////////////////////////////
                    //[ValorAjustado]
                    txtValorA.Text = Convert.ToDecimal(res.Rows[0]["ValorAjustado"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    //[ValorComercial]
                    txtValorC.Text = Convert.ToDecimal(res.Rows[0]["ValorComercial"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    //[ValorAsegurable]
                    txtValorAseg.Text = Convert.ToDecimal(res.Rows[0]["ValorAsegurable"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    // ,[NroTasacion]
                    txtNroTasacion.Text = res.Rows[0]["NroTasacion"].ToString();
                    // ,[Tasacion]
                    if (!string.IsNullOrEmpty(res.Rows[0]["Tasacion"].ToString()))
                        ckbTasacion.Checked = Convert.ToBoolean(res.Rows[0]["Tasacion"].ToString());
                    // ,[FecTasacion]
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecTasacion"].ToString()))
                        dtcFechaTasacion.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecTasacion"].ToString());
                    // ,[IdEmpresaTasadora]
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdEmpresaTasadora"].ToString()))
                        ddlEmpresaT.SelectedIndex = ddlEmpresaT.Items.IndexOf(ddlEmpresaT.Items.FindByValue(Convert.ToString(res.Rows[0]["IdEmpresaTasadora"])));
                    // ,[IdTasador]
                    CargarTasador(ddlTipoG.SelectedValue);
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdTasador"].ToString()))
                        DdlTasador.SelectedIndex = DdlTasador.Items.IndexOf(DdlTasador.Items.FindByValue(Convert.ToString(res.Rows[0]["IdTasador"])));
                    // ,[IdRegion]
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdRegion"].ToString()))
                        ddlRegiones.SelectedIndex = ddlRegiones.Items.IndexOf(ddlRegiones.Items.FindByValue(Convert.ToString(res.Rows[0]["IdRegion"])));
                    // ,[IdProvincia]
                    CargarProvincia(int.Parse(ddlRegiones.SelectedItem.Value));
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdProvincia"].ToString()))
                        ddlProvincia.SelectedIndex = ddlProvincia.Items.IndexOf(ddlProvincia.Items.FindByValue(Convert.ToString(res.Rows[0]["IdProvincia"])));
                    // ,[IdComuna]
                    //CargarComuna(ddlProvincia.SelectedValue);
                    CargarComuna(int.Parse(ddlProvincia.SelectedItem.Value));
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdComuna"].ToString()))
                        ddlComunas.SelectedIndex = ddlComunas.Items.IndexOf(ddlComunas.Items.FindByValue(Convert.ToString(res.Rows[0]["IdComuna"])));
                    // ,[Direccion]		
                    txtDireccion.Text = res.Rows[0]["Direccion"].ToString();

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////// PESTAÑA SEGUROS /////////////////////////////////////////////////////////////////////////////////////
                    //[NumPoliza]
                    txtPoliza.Text = res.Rows[0]["NumPoliza"].ToString();
                    //[Seguro]
                    if (!string.IsNullOrEmpty(res.Rows[0]["Seguro"].ToString()))
                        ckbSeguro.Checked = Convert.ToBoolean(res.Rows[0]["Seguro"].ToString());
                    //[IdContratante]		 
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdContratante"].ToString()))
                        ddlContratante.SelectedIndex = ddlContratante.Items.IndexOf(ddlContratante.Items.FindByValue(Convert.ToString(res.Rows[0]["IdContratante"])));
                    //[fecInicioSeguro]
                    if (!string.IsNullOrEmpty(res.Rows[0]["fecInicioSeguro"].ToString()))
                        dtcVigenciaSeguro.SelectedDate = Convert.ToDateTime(res.Rows[0]["fecInicioSeguro"].ToString());
                    //[fecVencSeguro]
                    if (!string.IsNullOrEmpty(res.Rows[0]["fecVencSeguro"].ToString()))
                        dtcVencSeguro.SelectedDate = Convert.ToDateTime(res.Rows[0]["fecVencSeguro"].ToString());
                    //[IdEmpresAseguradora]		 
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdEmpresAseguradora"].ToString()))
                        ddlCompaniaSeguro.SelectedIndex = ddlCompaniaSeguro.Items.IndexOf(ddlCompaniaSeguro.Items.FindByValue(Convert.ToString(res.Rows[0]["IdEmpresAseguradora"])));
                    //[ObservacionSeguro]
                    txtObsSeguro.InnerText = res.Rows[0]["ObservacionSeguro"].ToString();
                    //CoberturaIncendio	
                    if (!string.IsNullOrEmpty(res.Rows[0]["CoberturaIncendio"].ToString()))
                        ckbCoberturaSeguro.Items[1].Selected = Convert.ToBoolean(res.Rows[0]["CoberturaIncendio"].ToString());
                    //CoberturaTerremoto	
                    if (!string.IsNullOrEmpty(res.Rows[0]["CoberturaTerremoto"].ToString()))
                        ckbCoberturaSeguro.Items[2].Selected = Convert.ToBoolean(res.Rows[0]["CoberturaTerremoto"].ToString());
                    //CoberturaInundacion	
                    if (!string.IsNullOrEmpty(res.Rows[0]["CoberturaInundacion"].ToString()))
                        ckbCoberturaSeguro.Items[3].Selected = Convert.ToBoolean(res.Rows[0]["CoberturaInundacion"].ToString());
                    //CoberturaRobo
                    if (!string.IsNullOrEmpty(res.Rows[0]["CoberturaRobo"].ToString()))
                        ckbCoberturaSeguro.Items[0].Selected = Convert.ToBoolean(res.Rows[0]["CoberturaRobo"].ToString());
                    //EstadoSeguro
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdEstadoSeguro"].ToString()))
                        ddlEstadoSeg.SelectedIndex = ddlEstadoSeg.Items.IndexOf(ddlEstadoSeg.Items.FindByValue(Convert.ToString(res.Rows[0]["IdEstadoSeguro"])));

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////// PESTAÑA CONSTITUCION ////////////////////////////////////////////////////////////////////////////////
                    //[IdEstado]
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdEstado"].ToString()))
                        ddlEdoGarantia.SelectedIndex = ddlEdoGarantia.Items.IndexOf(ddlEdoGarantia.Items.FindByValue(Convert.ToString(res.Rows[0]["IdEstado"])));
                    //[IdSubEstado]
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdSubEstado"].ToString()))
                        ddlSubEdoGarantia.SelectedIndex = ddlSubEdoGarantia.Items.IndexOf(ddlSubEdoGarantia.Items.FindByValue(Convert.ToString(res.Rows[0]["IdSubEstado"])));
                    //[IdGradoPref]	
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdGradoPref"].ToString()))
                        dllGradoPrefe.SelectedIndex = dllGradoPrefe.Items.IndexOf(dllGradoPrefe.Items.FindByValue(Convert.ToString(res.Rows[0]["IdGradoPref"])));
                    //[IdCaracter]		
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdCaracter"].ToString()))
                        ddlCaracter.SelectedIndex = ddlCaracter.Items.IndexOf(ddlCaracter.Items.FindByValue(Convert.ToString(res.Rows[0]["IdCaracter"])));
                    //[IdLimite]		
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdLimite"].ToString()))
                        ddlLimite.SelectedIndex = ddlLimite.Items.IndexOf(ddlLimite.Items.FindByValue(Convert.ToString(res.Rows[0]["IdLimite"])));
                    //[MontoLimite]	 
                    if (!string.IsNullOrEmpty(res.Rows[0]["MontoLimite"].ToString()))
                        txtMontoLimite.Text = Convert.ToDecimal(res.Rows[0]["MontoLimite"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    //[IdParticipacion]		 
                    if (!string.IsNullOrEmpty(res.Rows[0]["IdParticipacion"].ToString()))
                        ddlParticipacion.SelectedIndex = ddlParticipacion.Items.IndexOf(ddlParticipacion.Items.FindByValue(Convert.ToString(res.Rows[0]["IdParticipacion"])));
                    //DivRutGarante 		RutGarante		razonSociaGarante
                    if (res.Rows[0]["RutGarante"].ToString() != "0")
                        txt_RutGarante.Text = res.Rows[0]["RutGarante"].ToString();
                    txt_DivRutGarante.Text = res.Rows[0]["DivRutGarante"].ToString();
                    txt_RazonSocialGarante.Text = res.Rows[0]["razonSociaGarante"].ToString();

                    dtcFechaInsc.SelectedDate = new DateTime();
                    //[FecInscripcionContrato]	
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecInscripcionContrato"].ToString()))
                        dtcFechaInsc.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecInscripcionContrato"].ToString());
                    //[FecConstitucionContrato]
                    dtcFechaConst.SelectedDate = new DateTime();
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecConstitucionContrato"].ToString()))
                        dtcFechaConst.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecConstitucionContrato"].ToString());
                    //[FecAlzamientoContrato]	
                    dtcFechaAlza.SelectedDate = new DateTime();
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecAlzamientoContrato"].ToString()))
                        dtcFechaAlza.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecAlzamientoContrato"].ToString());
                    //[FecContrato]
                    dtcFechaContrato.SelectedDate = new DateTime();
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecContrato"].ToString()))
                        dtcFechaContrato.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecContrato"].ToString());
                    //[Contrato]
                    if (!string.IsNullOrEmpty(res.Rows[0]["Contrato"].ToString()))
                        ckbContrato.Checked = Convert.ToBoolean(res.Rows[0]["Contrato"].ToString());
                    //[ObservacionEstado] 
                    txtObservacionEdo.InnerText = res.Rows[0]["ObservacionEstado"].ToString();
                    //SGRAcreedora
                    if (!string.IsNullOrEmpty(res.Rows[0]["SGRAcreedora"].ToString()))
                        ddlEmpresaAcreedora.SelectedIndex = ddlEmpresaAcreedora.Items.IndexOf(ddlEmpresaAcreedora.Items.FindByValue(res.Rows[0]["SGRAcreedora"].ToString()));
                    else
                        ddlEmpresaAcreedora.SelectedIndex = 0;
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    ///////////////////////////////////// PESTAÑA CORFO ////////////////////////////////////////////////////////////////////////////////
                    //[GiroCORFO]	
                    if (!string.IsNullOrEmpty(res.Rows[0]["GiroCORFO"].ToString()))
                        ckbGiroCORFO.Checked = Convert.ToBoolean(res.Rows[0]["GiroCORFO"].ToString());
                    //[FecGiroCORFO]	
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecGiroCORFO"].ToString()))
                        dtcGechaGiroCORFO.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecGiroCORFO"].ToString());
                    //[InformeCORFO]	
                    if (!string.IsNullOrEmpty(res.Rows[0]["InformeCORFO"].ToString()))
                        ckbInfCORFO.Checked = Convert.ToBoolean(res.Rows[0]["InformeCORFO"].ToString());
                    //[FecInformeCORFO]
                    if (!string.IsNullOrEmpty(res.Rows[0]["FecInformeCORFO"].ToString()))
                        dtcFechaInfCORFO.SelectedDate = Convert.ToDateTime(res.Rows[0]["FecInformeCORFO"].ToString());
                    //[Observacion]
                    txtObservacion.InnerText = res.Rows[0]["Observacion"].ToString();

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Identificador garantia
                    if (string.IsNullOrEmpty(res.Rows[0]["IdTipoBien"].ToString()))
                        CargarTipoIdentificador(res.Rows[0]["IdTipoBien"].ToString());
                    else
                        CargarTipoIdentificador(res.Rows[0]["IdTipoBien"].ToString());
                }
            }
        }

        protected void Guardar()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();

            DataTable dtAlertas = new DataTable();
            DataTable dtReglaNegocio = new DataTable();

            dtReglaNegocio = Ln.ReglaNegocioGarantia(objresumen.idEmpresa, objresumen.idOperacion);

            if (dtReglaNegocio.Rows.Count > 0)
            {
                if (dtReglaNegocio.Rows[0][0].ToString() == "EXITO")
                {
                    Boolean exito = true;
                    if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                    {
                        exito = Ln.InsertGarantias(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), "1", generarXMLComercial(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString());
                        if (exito)
                        {
                            succes(Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE));
                            vaciar();
                            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                            llenargrid();
                        }
                        else
                            error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE));
                    }
                    else if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                    {
                        exito = Ln.ActualizaGarantiasExpress(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), ViewState["GarantiaID"].ToString(), "1", generarXMLComercial(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString());
                        if (exito)
                        {
                            succes(Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE));
                            vaciar();
                            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                            llenargrid();
                        }
                        else
                            error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE));
                    }
                }
                else
                {
                    string mensajeValidacion = String.Empty;

                    string mensajeReglaNegocio = String.Empty;
                    for (int i = 0; i < dtReglaNegocio.Rows.Count; i++)
                    {
                        mensajeReglaNegocio = mensajeReglaNegocio + dtReglaNegocio.Rows[i][0];
                    }
                    if (mensajeReglaNegocio != "EXITO")
                    {
                        error(mensajeValidacion + " - " + mensajeReglaNegocio);
                    }

                    error(mensajeValidacion);
                }
            }
            else
                error("Se debe verificar la validación de los datos. Comuníquese con el administrador.");
        }

        string generarXMLComercial()
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;

            RespNode = doc.CreateElement("Val");

            foreach (Control Control in pnGarantia.Controls)
            {
                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (Control.ID == "txtNroInscripcion")
                        nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString()));
                    else
                        nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                    else
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(""));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(null));
                        RespNode.AppendChild(nodo1);
                    }
                }
            }

            //foreach en divAvales
            Control MyDiv = FindControl("divAvales");
            if (MyDiv != null)
            {
                foreach (Control Control in MyDiv.Controls)
                {
                    if (Control is TextBox)
                    {
                        XmlNode nodo = doc.CreateElement(Control.ID);
                        if (Control.ID == "txtNroInscripcion")
                            nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString()));
                        else
                            nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                        RespNode.AppendChild(nodo);
                    }

                    if (Control is DropDownList)
                    {
                        if (((DropDownList)Control).Items.Count > 0)
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                            RespNode.AppendChild(nodo1);
                        }
                        else
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(""));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(null));
                            RespNode.AppendChild(nodo1);
                        }
                    }
                }
            }

            Control MyDivRegimen = FindControl("divRegimen");
            if (MyDivRegimen != null)
            {
                foreach (Control Control in MyDivRegimen.Controls)
                {
                    if (Control is DropDownList)
                    {
                        if (((DropDownList)Control).Items.Count > 0)
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                            RespNode.AppendChild(nodo1);
                        }
                        else
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(""));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(null));
                            RespNode.AppendChild(nodo1);
                        }
                    }
                }
            }

            foreach (Control Control in pnTasacion.Controls)
            {
                if (Control is CheckBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((CheckBox)Control).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                if (Control is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (!((DateTimeControl)Control).IsDateEmpty)
                    {
                        string fecha = ((DateTimeControl)Control).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else nodo.AppendChild(doc.CreateTextNode("-1"));
                    RespNode.AppendChild(nodo);
                }
            }

            foreach (Control Control in pnConstitucion.Controls)
            {
                if (Control is CheckBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((CheckBox)Control).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                if (Control is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (!((DateTimeControl)Control).IsDateEmpty)
                    {
                        string fecha = ((DateTimeControl)Control).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else nodo.AppendChild(doc.CreateTextNode("-1"));
                    RespNode.AppendChild(nodo);
                }
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        protected void Actualizar(string idGarantia)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean exito = true;
            if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
            {
                exito = Ln.ActualizaGarantias(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), idGarantia, "1", generarXMLOperaciones(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), txtObservacionEdo.InnerText, txtObsSeguro.InnerText, txtObservacion.InnerText);
                if (exito)
                {
                    succes(Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE));
                    vaciar();
                    ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                    llenargrid();
                }
                else
                    error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE));
            }
        }

        string generarXMLOperaciones()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;

            RespNode = doc.CreateElement("Val");

            foreach (Control Control in pnGarantia.Controls)
            {
                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (Control.ID == "txtNroInscripcion")
                        nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString()));
                    else
                        nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                    else
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(""));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(null));
                        RespNode.AppendChild(nodo1);
                    }
                }
            }

            foreach (Control Control in pnTasacion.Controls)
            {
                if (Control is CheckBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((CheckBox)Control).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                if (Control is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (!((DateTimeControl)Control).IsDateEmpty)
                    {
                        string fecha = ((DateTimeControl)Control).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else nodo.AppendChild(doc.CreateTextNode("-1"));
                    RespNode.AppendChild(nodo);
                }
            }

            //panel seguro
            RespNode = xmlTab(pnSeguro, doc, RespNode);

            foreach (Control Control in pnConstitucion.Controls)
            {
                if (Control is CheckBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((CheckBox)Control).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                if (Control is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (!((DateTimeControl)Control).IsDateEmpty)
                    {
                        string fecha = ((DateTimeControl)Control).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else nodo.AppendChild(doc.CreateTextNode("-1"));
                    RespNode.AppendChild(nodo);
                }
            }

            foreach (Control Control in pnCorfo.Controls)
            {
                if (Control is CheckBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((CheckBox)Control).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                if (Control is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (!((DateTimeControl)Control).IsDateEmpty)
                    {
                        string fecha = ((DateTimeControl)Control).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else nodo.AppendChild(doc.CreateTextNode("-1"));
                    RespNode.AppendChild(nodo);
                }
            }

            //foreach en divAvales
            Control MyDiv = FindControl("divAvales");
            if (MyDiv != null)
            {
                foreach (Control Control in MyDiv.Controls)
                {
                    if (Control is TextBox)
                    {
                        XmlNode nodo = doc.CreateElement(Control.ID);
                        if (Control.ID == "txtNroInscripcion")
                            nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString()));
                        else
                            nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                        RespNode.AppendChild(nodo);
                    }

                    if (Control is DropDownList)
                    {
                        if (((DropDownList)Control).Items.Count > 0)
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                            RespNode.AppendChild(nodo1);
                        }
                        else
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(""));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(null));
                            RespNode.AppendChild(nodo1);
                        }
                    }
                }
            }

            Control MyDivRegimen = FindControl("divRegimen");
            if (MyDivRegimen != null)
            {
                foreach (Control Control in MyDivRegimen.Controls)
                {
                    if (Control is DropDownList)
                    {
                        if (((DropDownList)Control).Items.Count > 0)
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                            RespNode.AppendChild(nodo1);
                        }
                        else
                        {
                            XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                            nodo.AppendChild(doc.CreateTextNode(""));
                            RespNode.AppendChild(nodo);

                            XmlNode nodo1 = doc.CreateElement(Control.ID);
                            nodo1.AppendChild(doc.CreateTextNode(null));
                            RespNode.AppendChild(nodo1);
                        }
                    }
                }
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        public XmlNode xmlTab(Panel pnSeguro, XmlDocument doc, XmlNode RespNode)
        {
            foreach (Control Control in pnSeguro.Controls)
            {
                if (Control is CheckBox)
                {
                    string chk;
                    if (Control.ID == "ckbSeguro")
                        chk = hdnChk.Value == "false" ? "false" : "true";
                    else
                        chk = ((CheckBox)Control).Checked.ToString();

                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(chk));
                    //nodo.AppendChild(doc.CreateTextNode(((CheckBox)Control).Checked.ToString()));
                    RespNode.AppendChild(nodo);
                }

                if (Control is CheckBoxList)
                {
                    for (int i = 0; i < ((CheckBoxList)Control).Items.Count; i++)
                    {
                        XmlNode nodo = doc.CreateElement(Control.ID + i.ToString());
                        nodo.AppendChild(doc.CreateTextNode(((CheckBoxList)Control).Items[i].Selected.ToString()));
                        RespNode.AppendChild(nodo);
                    }
                }

                if (Control is TextBox)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    nodo.AppendChild(doc.CreateTextNode(((TextBox)Control).Text.ToString().Replace(".", "").Replace(",", ".")));
                    RespNode.AppendChild(nodo);
                }

                if (Control is DropDownList)
                {
                    if (((DropDownList)Control).Items.Count > 0)
                    {
                        XmlNode nodo = doc.CreateElement("ID" + Control.ID);
                        nodo.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedValue.ToString()));
                        RespNode.AppendChild(nodo);

                        XmlNode nodo1 = doc.CreateElement(Control.ID);
                        nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Control).SelectedItem.ToString()));
                        RespNode.AppendChild(nodo1);
                    }
                }

                if (Control is DateTimeControl)
                {
                    XmlNode nodo = doc.CreateElement(Control.ID);
                    if (!((DateTimeControl)Control).IsDateEmpty)
                    {
                        string fecha = ((DateTimeControl)Control).SelectedDate.Day.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Month.ToString() + "-" + ((DateTimeControl)Control).SelectedDate.Year.ToString();
                        nodo.AppendChild(doc.CreateTextNode(fecha));
                    }
                    else nodo.AppendChild(doc.CreateTextNode("-1"));
                    RespNode.AppendChild(nodo);
                }
            }

            return RespNode;
        }

        private void cargarGvContribuciones()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();
            string idGarantia = string.Empty;

            if (ViewState["GarantiaID"] == null)
                idGarantia = "0";
            else
                idGarantia = ViewState["GarantiaID"].ToString();

            dt = Ln.CargarContribuciones(int.Parse(idGarantia));

            if (dt.Rows.Count > 0)
                SumarGridContribuciones(dt);
            else
            {
                gvContribuciones.DataSource = dt;
                gvContribuciones.DataBind();
            }
        }

        private void SumarGridContribuciones(DataTable dt)
        {
            gvContribuciones.DataSource = dt;
            gvContribuciones.DataBind();

            // suma de columna
            decimal totalPagar = dt.AsEnumerable().Sum(row => row.Field<decimal>("TotalPagar"));

            gvContribuciones.FooterRow.Cells[5].Text = "Total a Pagar";
            gvContribuciones.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            gvContribuciones.FooterRow.Cells[5].ForeColor = Color.Black;
            gvContribuciones.FooterRow.Cells[5].Font.Bold = true;

            gvContribuciones.FooterRow.Cells[6].Text = totalPagar.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            gvContribuciones.FooterRow.Cells[6].ForeColor = Color.Black;
            gvContribuciones.FooterRow.Cells[6].Font.Bold = true;
        }

        public void vaciar()
        {
            lbNGtia.Text = string.Empty;
            txtNroInscripcion.Text = string.Empty;
            ddlTipoG.SelectedIndex = 0;
            if (ddlTipoBienes.Items.Count > 0)
                ddlTipoBienes.SelectedIndex = 0;
            txtValorA.Text = string.Empty;
            txtDescP.Text = string.Empty;
            txtValorC.Text = string.Empty;
            txtValorAseg.Text = string.Empty;
            dllGradoPrefe.SelectedIndex = 0;
            ddlCaracter.SelectedIndex = 0;
            ddlLimite.SelectedIndex = 0;
            txtMontoLimite.Text = string.Empty;
            ddlParticipacion.SelectedIndex = 0;

            ckbContrato.Checked = false;
            ddlEdoGarantia.SelectedIndex = 0;
            ddlSubEdoGarantia.SelectedIndex = 0;
            txtObservacionEdo.InnerText = string.Empty;
            ddlRegiones.SelectedIndex = 0;
            if (ddlProvincia.Items.Count > 0)
                ddlProvincia.SelectedIndex = 0;
            if (ddlComunas.Items.Count > 0)
                ddlComunas.SelectedIndex = 0;
            txtDireccion.Text = string.Empty;
            ckbGiroCORFO.Checked = false;
            ckbInfCORFO.Checked = false;
            txtObservacion.InnerText = string.Empty;
            txtNroTasacion.Text = string.Empty;
            ckbTasacion.Checked = false;
            if (ddlEmpresaT.Items.Count > 0)
                ddlEmpresaT.SelectedIndex = 0;
            if (DdlTasador.Items.Count > 0)
                DdlTasador.SelectedIndex = 0;
            txtPoliza.Text = string.Empty;
            ckbSeguro.Checked = false;
            ddlContratante.SelectedIndex = 0;

            ddlCompaniaSeguro.SelectedIndex = 0;
            txtObsSeguro.InnerText = string.Empty;
            ckbCoberturaSeguro.Items[1].Selected = false;
            ckbCoberturaSeguro.Items[2].Selected = false;
            ckbCoberturaSeguro.Items[3].Selected = false;
            ckbCoberturaSeguro.Items[0].Selected = false;

            dtcFechaInsc.SelectedDate = new DateTime();
            dtcFechaAlza.SelectedDate = new DateTime();
            dtcFechaConst.SelectedDate = new DateTime();
            dtcFechaInfCORFO.SelectedDate = new DateTime();
            dtcFechaContrato.SelectedDate = new DateTime();
            dtcFechaTasacion.SelectedDate = new DateTime();
            dtcGechaGiroCORFO.SelectedDate = new DateTime();
            dtcVencSeguro.SelectedDate = new DateTime();
            dtcVigenciaSeguro.SelectedDate = new DateTime();

            txt_DivAval.Text = "";
            txt_RUTAval.Text = "";
            txtRazonSocialAval.Text = "";
            txtNacionalidad.Text = "";
            txtProfesion.Text = "";
            if (ddlEdoCivil.Items.Count > 0)
                ddlEdoCivil.SelectedIndex = 0;
            divAvales.Visible = false;

            txt_RutGarante.Text = "";
            txt_DivRutGarante.Text = "";
            txt_RazonSocialGarante.Text = "";

            if (ddlTipoIdentificador.Items.Count > 0)
                ddlTipoIdentificador.SelectedIndex = 0;
            if (ddlEmpresaAcreedora.Items.Count > 0)
                ddlEmpresaAcreedora.SelectedIndex = 0;
        }

        public void vaciarGvHistorial()
        {
            GvTasacionHistorial.DataSource = new DataTable();
            GvTasacionHistorial.DataBind();
            GvHistorialSeguros.DataSource = new DataTable();
            GvHistorialSeguros.DataBind();
        }

        protected void ddlTipoG_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTipoBien(ddlTipoG.SelectedValue);
            ValidarSegurosActivos();
            divAvales.Visible = false;
        }

        void ValidarSegurosActivos()
        {
            if (!string.IsNullOrEmpty(ddlTipoG.SelectedItem.Text))
            {
                if (ddlTipoG.SelectedItem.Text == "Prenda sobre bienes corporales (excluido Warrant)")
                {
                    ckbCoberturaSeguro.Items[0].Enabled = true;
                    ckbCoberturaSeguro.Items[1].Selected = false;
                    ckbCoberturaSeguro.Items[1].Enabled = false;
                    ckbCoberturaSeguro.Items[2].Selected = false;
                    ckbCoberturaSeguro.Items[2].Enabled = false;
                    ckbCoberturaSeguro.Items[3].Selected = false;
                    ckbCoberturaSeguro.Items[3].Enabled = false;
                    ckbCoberturaSeguro.Items[4].Selected = false;
                    ckbCoberturaSeguro.Items[4].Enabled = false;
                }
                else if (ddlTipoG.SelectedItem.Text == "Hipoteca")
                {
                    ckbCoberturaSeguro.Items[0].Selected = false;
                    ckbCoberturaSeguro.Items[0].Enabled = false;

                    ckbCoberturaSeguro.Items[1].Enabled = true;
                    ckbCoberturaSeguro.Items[2].Enabled = true;
                    ckbCoberturaSeguro.Items[3].Enabled = true;
                    ckbCoberturaSeguro.Items[4].Enabled = true;
                }
                else if (ddlTipoG.SelectedItem.Text == "Otras garantías (reales o personales)" && ddlTipoBienes.SelectedItem.Text == "Avales y fianzas")
                {
                    ckbCoberturaSeguro.Items[1].Selected = false;
                    ckbCoberturaSeguro.Items[1].Enabled = false;
                    ckbCoberturaSeguro.Items[2].Selected = false;
                    ckbCoberturaSeguro.Items[2].Enabled = false;
                    ckbCoberturaSeguro.Items[3].Selected = false;
                    ckbCoberturaSeguro.Items[3].Enabled = false;

                    ckbCoberturaSeguro.Items[4].Enabled = true;
                }
                else
                {
                    ckbCoberturaSeguro.Items[1].Selected = false;
                    ckbCoberturaSeguro.Items[1].Enabled = false;
                    ckbCoberturaSeguro.Items[2].Selected = false;
                    ckbCoberturaSeguro.Items[2].Enabled = false;
                    ckbCoberturaSeguro.Items[3].Selected = false;
                    ckbCoberturaSeguro.Items[3].Enabled = false;
                    ckbCoberturaSeguro.Items[4].Selected = false;
                    ckbCoberturaSeguro.Items[4].Enabled = false;
                }
            }
        }

        public void CargarClaseGarantia()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoGarantia = Ln.ListarTiposGarantia();
            util.CargaDDL(ddlTipoG, dtTipoGarantia, "Nombre", "Id");
        }

        public void CargarTipoIdentificador(string TipoBien)
        {
            ddlTipoIdentificador.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoGarantia = Ln.ListarTiposBienes(null, Convert.ToInt32(TipoBien));
            util.CargaDDL(ddlTipoIdentificador, dtTipoGarantia, "IdentificadorGarantia", "IdentificadorGarantia");

            if (ddlTipoIdentificador.Items.Count > 1)
            {
                ddlTipoIdentificador.SelectedIndex = 1;
                LblInscripcion.Text = ddlTipoIdentificador.SelectedItem.Text;

                if (string.IsNullOrEmpty(LblInscripcion.Text))
                    LblInscripcion.Text = "N° Inscripción";
            }
            else
                LblInscripcion.Text = "N° Inscripción";

            ddlTipoIdentificador.Enabled = false;
        }

        public void CargarTipoBien(string ClaseGarantia)
        {
            ddlTipoBienes.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoGarantia = Ln.ListarTiposBienes(Convert.ToInt32(ClaseGarantia), null);
            util.CargaDDL(ddlTipoBienes, dtTipoGarantia, "Nombre", "Codigo");
        }

        public void CargarTipoBienValores(string IdTipoBien)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoGarantia = Ln.ListarTiposBienes(null, Convert.ToInt32(IdTipoBien));

            if (!dtTipoGarantia.SinDatos())
            {
                hddTasacion.Value = dtTipoGarantia.Rows[0]["Tasacion"].ToString();
                hddSeguro.Value = dtTipoGarantia.Rows[0]["Seguros"].ToString();
                hddAjuste.Value = dtTipoGarantia.Rows[0]["Ajuste"].ToString();

                if (hddTasacion.Value == "True")
                {
                    ckbTasacion.Checked = true;
                    ckbTasacion.Enabled = false;
                }
                else
                {
                    ckbTasacion.Checked = false;
                    ckbTasacion.Enabled = false;
                }

                //if (hddSeguro.Value == "True")
                //{
                //    ckbSeguro.Checked = true;
                //    ckbSeguro.Enabled = false;
                //}
                //else
                //{
                //    ckbSeguro.Checked = false;
                //    ckbSeguro.Enabled = false;
                //}
            }
            else
            {
                hddTasacion.Value = "0";
                hddSeguro.Value = "0";
                hddAjuste.Value = "0";
            }
            if (txtValorC.Text.Replace(" ", "") != "")
            {
                if (hddAjuste.Value != "0")
                {
                    txtValorA.Text = ((float.Parse(txtValorC.Text.Replace(" ", "").Replace(".", "").Replace(",", ".").Trim()) - (float.Parse(txtValorC.Text.Replace(" ", "").Replace(".", "").Replace(",", ".").Trim()) * (float.Parse(hddAjuste.Value)) / 100)).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES")));
                    txtValorA.Enabled = false;
                }
                else
                {
                    txtValorA.Text = txtValorC.Text;
                    txtValorA.Enabled = false;
                }
            }

            if (ddlTipoBienes.SelectedValue.ToString() == "88")
                divAvales.Visible = true;
            else
                divAvales.Visible = false;
        }

        public void CargarEmpresaCompartida(String Filtro)
        {
            //BD EMPRESA
            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;
            //SPListItemCollection items = app.Lists["CompaniaSeguro"].Items;
            //CargaDDL(ddlCompaniaSeguro, items.GetDataTable(), "Nombre", "ID");
        }

        public void CargarGradoPreferencia()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtGradoPreferencia = Ln.ListarGradoPreferencia();
            util.CargaDDL(dllGradoPrefe, dtGradoPreferencia, "Nombre", "Id");
        }

        public void CargarParticipacion()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtParticipacion = Ln.ListarParticipacion();
            util.CargaDDL(ddlParticipacion, dtParticipacion, "Nombre", "Id");
        }

        public void CargarContratante()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            var dt = Ln.ListarContratantes(0);
            util.CargaDDL(ddlContratante, dt, "Nombre", "Id");
        }

        public void CargarEstadoSeguro()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = Ln.ListarEstadoSeguro(0);
            util.CargaDDL(ddlEstadoSeg, dt, "Estado", "Id");
        }

        private void CargarEstadoCivil()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtEstCivil = Ln.ListarEstadoCivil();
                util.CargaDDL(ddlEdoCivil, dtEstCivil, "Nombre", "Id");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarEmpresaAcreedora()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = Ln.EmpresaAcreedora(2);
            util.CargaDDL(ddlEmpresaAcreedora, dt, "SGR", "Id");
        }

        public void CargarLimite()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtLimite = Ln.ListarLimite();
            util.CargaDDL(ddlLimite, dtLimite, "Nombre", "Id");
        }

        public void CargarCaracter()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtCaracter = Ln.ListarCaracter();
            util.CargaDDL(ddlCaracter, dtCaracter, "Nombre", "Id");
        }

        public void CargarEdoGarantia()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEdoGarantia = Ln.ListarEstadosGarantia();
            util.CargaDDL(ddlEdoGarantia, dtEdoGarantia, "Nombre", "Id");
        }

        public void CargarSubEstadoGarantia()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtSubEdoGarantia = Ln.ListarSubEstadosGarantia();
            util.CargaDDL(ddlSubEdoGarantia, dtSubEdoGarantia, "Nombre", "Id");
        }

        public void CargarRegion()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            var dt = Ln.ListarRegion();
            util.CargaDDL(ddlRegiones, dt, "NombreRegion", "IdRegion");
        }

        public void CargarProvincia(int Filtro)
        {
            ddlProvincia.Items.Clear();
            ddlComunas.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            var dt = Ln.ListarProvincia(Filtro);
            util.CargaDDL(ddlProvincia, dt, "DescCiudad", "IdCiudad");
        }

        public void CargarComuna(int idProv)
        {
            try
            {
                ddlComunas.Items.Clear();
                LogicaNegocio Ln = new LogicaNegocio();
                var dt = Ln.ListarComunas(idProv);
                util.CargaDDL(ddlComunas, dt, "NombreComuna", "IdComuna");
            }
            catch (Exception ex)
            {
                ocultarDiv();
                lbError.Text = "Ha ocurrido un error al cargar los datos de la comuna";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public void CargarEmpresaTasadora()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEmpTasadoras = Ln.ListarEmpresaTasadora();
            util.CargaDDL(ddlEmpresaT, dtEmpTasadoras, "Nombre", "Id");
        }

        public void CargarTasador(String Filtro)
        {
            DdlTasador.Items.Clear();
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["Tasadores"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='EmpresaTasadora' LookupId='TRUE'/><Value Type='Lookup'>" + Filtro + "</Value></Eq></Where><OrderBy><FieldRef Name='title' Ascending='TRUE' /></OrderBy>";
            util.CargaDDL(DdlTasador, items.GetItems(oQuery).GetDataTable(), "Title", "ID");
        }

        public void CargarCompaniaSeguro()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtCompaniaSeguro = Ln.ListarCompaniaSeguro();
            util.CargaDDL(ddlCompaniaSeguro, dtCompaniaSeguro, "Nombre", "Id");
        }

        public void ocultarDiv()
        {
            dvSuccess.Style.Add("display", "none");
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void CargarRegimen(string IdEstadoCivil)
        {
            try
            {
                ddlRegimen.Items.Clear();
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtRegimen = Ln.ListarRegimen(Convert.ToInt32(IdEstadoCivil));
                util.CargaDDL(ddlRegimen, dtRegimen, "Nombre", "Id");

                if (int.Parse(IdEstadoCivil) == 2 || int.Parse(IdEstadoCivil) == 6)
                    divRegimen.Style.Add("display", "block");
                else
                    divRegimen.Style.Add("display", "none");

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarTabTasacion(int idTasacion)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();
            dt = Ln.CargarDatosTasacionGarantias(Convert.ToInt32(ViewState["GarantiaID"]), null, objresumen.idEmpresa, objresumen.idOperacion, "", 0, 3);

            ///////////////////////////////////// PESTAÑA TASACION /////////////////////////////////////////////////////////////////////////////////////
            //[ValorAjustado]
            txtValorA.Text = Convert.ToDecimal(dt.Rows[0]["ValorAjustado"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //[ValorComercial]
            txtValorC.Text = Convert.ToDecimal(dt.Rows[0]["ValorComercial"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            //[ValorAsegurable]
            txtValorAseg.Text = Convert.ToDecimal(dt.Rows[0]["ValorAsegurable"].ToString()).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
            // ,[NroTasacion]
            txtNroTasacion.Text = dt.Rows[0]["NroTasacion"].ToString();
            // ,[Tasacion]
            if (dt.Rows[0]["Tasacion"].ToString() != "")
                ckbTasacion.Checked = Convert.ToBoolean(dt.Rows[0]["Tasacion"].ToString());
            // ,[FecTasacion]
            if (dt.Rows[0]["FecTasacion"].ToString() != "")
                dtcFechaTasacion.SelectedDate = Convert.ToDateTime(dt.Rows[0]["FecTasacion"].ToString());
            // ,[IdEmpresaTasadora]
            if (dt.Rows[0]["IdEmpresaTasadora"].ToString() != "")
                ddlEmpresaT.SelectedIndex = ddlEmpresaT.Items.IndexOf(ddlEmpresaT.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdEmpresaTasadora"].ToString())));
            // ,[IdTasador]
            CargarTasador(ddlTipoG.SelectedValue);
            if (dt.Rows[0]["IdTasador"].ToString() != "")
                DdlTasador.SelectedIndex = DdlTasador.Items.IndexOf(DdlTasador.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdTasador"].ToString())));
            // ,[IdRegion]
            if (dt.Rows[0]["IdRegion"].ToString() != "")
                ddlRegiones.SelectedIndex = ddlRegiones.Items.IndexOf(ddlRegiones.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdRegion"].ToString())));
            // ,[IdProvincia]
            CargarProvincia(int.Parse(ddlRegiones.SelectedItem.Value));
            if (dt.Rows[0]["IdProvincia"].ToString() != "")
                ddlProvincia.SelectedIndex = ddlProvincia.Items.IndexOf(ddlProvincia.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdProvincia"].ToString())));
            // ,[IdComuna]
            //CargarComuna(ddlProvincia.SelectedValue);
            CargarComuna(int.Parse(ddlProvincia.SelectedItem.Value));
            if (dt.Rows[0]["IdComuna"].ToString() != "")
                ddlComunas.SelectedIndex = ddlComunas.Items.IndexOf(ddlComunas.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdComuna"].ToString())));
            // ,[Direccion]		
            txtDireccion.Text = dt.Rows[0]["Direccion"].ToString();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        public Boolean ActualizarDocTasacionHistorica(string IdTasacion, string IdGarantia, Resumen objresumen, string NombreDoc, string opcion)
        {
            LogicaNegocio LN = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;
            if (string.IsNullOrEmpty(NombreDoc))
                NombreDoc = "";
            string ext = util.SoloExtension(NombreDoc);
            return LN.InsertarModifcarTasacionGarantias(IdTasacion, IdGarantia, objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), "", null, 0, "", 0, "", true, util.NombreArchivo("Tasación", objresumen.idOperacion.ToString()) + "." + ext, util.ObtenerValor(app2.CurrentUser.Name), opcion);
        }

        private void CargarTabSeguro(int idSeguro)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();

            dt = Ln.CargarDatosHistoriaSeguros(0, "", "", "", idSeguro, 4);

            //[NumPoliza]
            txtPoliza.Text = dt.Rows[0]["NumPoliza"].ToString();
            //[Seguro]
            if (dt.Rows[0]["RequiereSeguro"].ToString() != "")
                ckbSeguro.Checked = Convert.ToBoolean(dt.Rows[0]["RequiereSeguro"].ToString());
            //[IdContratante]		 
            if (dt.Rows[0]["IdContratante"].ToString() != "")
                ddlContratante.SelectedIndex = ddlContratante.Items.IndexOf(ddlContratante.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdContratante"].ToString())));
            //[fecInicioSeguro]
            if (dt.Rows[0]["fecInicioSeguro"].ToString() != "")
                dtcVigenciaSeguro.SelectedDate = Convert.ToDateTime(dt.Rows[0]["fecInicioSeguro"].ToString());
            //[fecVencSeguro]
            if (dt.Rows[0]["fecVencSeguro"].ToString() != "")
                dtcVencSeguro.SelectedDate = Convert.ToDateTime(dt.Rows[0]["fecVencSeguro"].ToString());
            //[IdEmpresAseguradora]		 
            if (dt.Rows[0]["IdEmpresAseguradora"].ToString() != "")
                ddlCompaniaSeguro.SelectedIndex = ddlCompaniaSeguro.Items.IndexOf(ddlCompaniaSeguro.Items.FindByValue(Convert.ToString(dt.Rows[0]["IdEmpresAseguradora"].ToString())));
            //[ObservacionSeguro]
            txtObsSeguro.InnerText = dt.Rows[0]["ObservacionSeguro"].ToString();
            //CoberturaIncendio	
            if (dt.Rows[0]["CoberturaIncendio"].ToString() != "")
                ckbCoberturaSeguro.Items[1].Selected = Convert.ToBoolean(dt.Rows[0]["CoberturaIncendio"].ToString());
            //CoberturaTerremoto	
            if (dt.Rows[0]["CoberturaTerremoto"].ToString() != "")
                ckbCoberturaSeguro.Items[2].Selected = Convert.ToBoolean(dt.Rows[0]["CoberturaTerremoto"].ToString());
            //CoberturaInundacion	
            if (dt.Rows[0]["CoberturaInundacion"].ToString() != "")
                ckbCoberturaSeguro.Items[3].Selected = Convert.ToBoolean(dt.Rows[0]["CoberturaInundacion"].ToString());
            //CoberturaRobo
            if (dt.Rows[0]["CoberturaRobo"].ToString() != "")
                ckbCoberturaSeguro.Items[0].Selected = Convert.ToBoolean(dt.Rows[0]["CoberturaRobo"].ToString());
            //CoberturaDesgravamen
            if (dt.Rows[0]["CoberturaDesgravamen"].ToString() != "")
                ckbCoberturaSeguro.Items[4].Selected = Convert.ToBoolean(dt.Rows[0]["CoberturaDesgravamen"].ToString());
        }

        #endregion

        //private void formatearNroInscripcion(bool formatear)
        //{
        //    MaskedEditExtender1.Enabled = true;
        //    MaskedEditExtender1.MessageValidatorTip = formatear ? true : false;
        //    MaskedEditExtender1.MaskType = formatear ? MaskedEditType.Number : MaskedEditType.None;
        //    //MaskedEditExtender1.Mask = formatear ? "9999-9999" : string.Empty; //"9999";
        //    MaskedEditExtender1.InputDirection = formatear ? MaskedEditInputDirection.RightToLeft : MaskedEditInputDirection.RightToLeft;
        //    MaskedEditExtender1.ClearMaskOnLostFocus = formatear ? false : false;
        //}
    }
}

