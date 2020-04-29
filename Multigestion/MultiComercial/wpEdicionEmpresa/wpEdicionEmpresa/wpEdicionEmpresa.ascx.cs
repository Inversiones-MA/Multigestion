using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpEdicionEmpresa.wpEdicionEmpresa
{
    [ToolboxItemAttribute(false)]
    public partial class wpEdicionEmpresa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        public ListItem LstItem;
        public SPList listData;
        public SPWeb app;
        public Empresa datosEmpresa;
        public string strRes;
        //public static SPListItem carpetaCliente;
        public SPFolder TargetFolder;
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "EdicionEmpresas.aspx";

        public wpEdicionEmpresa()
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
            //LogicaNegocio Ln = new LogicaNegocio();
            lbEdit.OnClientClick = "return HabilitarEmpresa('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
            btnLimpiar.OnClientClick = "return LimpiarVacido('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
            btnGuardar.OnClientClick = "return ValidarEdicionEmpresa('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
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

            try
            {
                dt = validar.ListarPerfil(validar);
                if (dt.Rows.Count > 0)
                {
                    if (!Page.IsPostBack)
                    {
                        ((TextBox)(dtcFechaInicioContrato.Controls[0])).Attributes.Add("onblur", "calcularFecha();");
                        ((TextBox)(dtcFechaInicioContrato.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");
                        ((TextBox)(dtcIniciActEconomica.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");

                        if (Page.Session["Resumen"] != null)
                        {
                            ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                            Page.Session["BUSQUEDA"] = null;

                            CargarEjecutivo();
                            objresumen = new Resumen();
                            objresumen = ((Resumen)(Page.Session["RESUMEN"]));
                            Page.Session["RESUMEN"] = null;
                            ViewState["RES"] = objresumen;

                            lblRut.Text = objresumen.rut.Split('-')[0];
                            lblDv.Text = objresumen.rut.Split('-')[1];
                            lblRazon.Text = objresumen.desEmpresa;
                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbEjecutivo.Text = objresumen.descEjecutivo;
                            if (!string.IsNullOrEmpty(objresumen.descEjecutivo.Trim()) || !string.IsNullOrWhiteSpace(objresumen.descEjecutivo.Trim()))
                                ddlEjecutivo.SelectedValue = ddlEjecutivo.Items.FindByText(System.Web.HttpUtility.HtmlDecode(objresumen.descEjecutivo)) == null ? "0" : ddlEjecutivo.Items.FindByText(System.Web.HttpUtility.HtmlDecode(objresumen.descEjecutivo)).Value;
                                
                            lbRut.Text = objresumen.rut;
                            pnlEdit.Enabled = false;
                        }

                        CargaTipoEmpresa();
                        CargarMotivoBloqueo();
                        CargarGrupoEconomico();
                        ActividadEconomica();

                        datosEmpresa = new Empresa();
                        datosEmpresa = new Empresa { }.TraeEmpresasClase(objresumen.idEmpresa);
                        CargarDatosEmpresa();

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa", "Empresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            OcultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Style.Add("display", "none");
                            lbEdit.Enabled = false;
                            lbEdit.Visible = false;
                        }

                        //validar que el usuari sea administrador, abogado u jefe operacione
                        //if (dt.Rows[0]["descCargo"].ToString() == "Administrador" || dt.Rows[0]["descCargo"].ToString() == "Abogado" || dt.Rows[0]["descCargo"].ToString() == "Jefe Operaciones" || dt.Rows[0]["descCargo"].ToString() == "Gerente Legal")
                        if (objresumen.area == "Fiscalia")
                            divSII.Visible = true;
                        else
                            divSII.Visible = false;
                    }
                    else
                    {
                        pnlEdit.Enabled = true;
                        //pnlContrato.Enabled = true;
                        btnGuardar.Attributes.Add("style", "display:block");
                        btnLimpiar.Attributes.Add("style", "display:block");
                    }

                    validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                    validar.descCargo = dt.Rows[0]["descCargo"].ToString();
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
                    //bloquear panel pnlContrato si no es de area operaciones y perfil administrador
                    hfArea.Value = validar.descCargo;
                }
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    dvWarning1.Style.Add("display", "block");
                    lbWarning1.Text = "Usuario sin permisos configurados";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbSociosAccionistas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("SociosAccionistas.aspx");
        }

        protected void lbDirectorio_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DirectorioEmpresa.aspx");
        }

        protected void lbPersonas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("personas.aspx");
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Grabar();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(SPContext.Current.Web.Url + "/SitePages/" + objresumen.linkPrincial);
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(SPContext.Current.Web.Url + "/SitePages/" + objresumen.linkPrincial);
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        //protected void dtcFechaInicioContrato_DateChanged(object sender, EventArgs e)
        //{
        //    LogicaNegocio LN = new LogicaNegocio();
        //    if (!dtcFechaInicioContrato.IsDateEmpty)
        //    {
        //        DateTime? dt = dtcFechaInicioContrato.SelectedDate.AddYears(1);

        //        hfFecha.Value = txtFechaFinContrato.Text.Trim();
        //    }
        //}

        #endregion


        #region Metodos

        public void OcultarDiv()
        {
            dvError.Style.Add("display", "none");
            dvExito.Style.Add("display", "none");
            dvInfo.Style.Add("display", "none");
            dvWarning.Style.Add("display", "none");
            dvCustom.Style.Add("display", "none");
        }

        public void asignacionResumen(ref Resumen objres)
        {
            if (ViewState["RES"] != null)
                objres = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void CargarEjecutivo()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                DataTable dtUsuarios = Ln.ListarUsuarios(null, 1, "");
                util.CargaDDL(ddlEjecutivo, dtUsuarios, "nombreApellido", "idUsuario");
            }
            catch
            {
                Error(Ln.buscarMensaje(Constantes.MENSAJES.EMPRESAERRORUSUARIO));
            }
        }

        private void CargaTipoEmpresa()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                DataTable dtTiposEmpresa = Ln.ListarTiposEmpresa();
                util.CargaDDL(ddlTipoE, dtTiposEmpresa, "Descripcion", "Id");
            }
            catch
            {
                Error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFEMPRESACHIS));
            }
        }

        private void CargarMotivoBloqueo()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                DataTable dtMotivoBloqueo = Ln.ListarMotivoBloqueo();
                util.CargaDDL(ddlMotivoBloqueo, dtMotivoBloqueo, "Motivo", "Id");
            }
            catch
            {
                Error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFEMPRESACHIS));
            }
        }

        private void Error(string mensaje)
        {
            lbError.Text = mensaje;
            dvError.Style.Add("display", "block");
        }

        private void CargarGrupoEconomico()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                DataTable dtGrupoEconomico = Ln.ListarGrupoEconomico();
                util.CargaDDL(ddlGrupoEcono, dtGrupoEconomico, "Nombre", "Id");
            }
            catch
            {
                Error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFEMPRESACHIS));
            }
        }

        private void ActividadEconomica()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                DataTable dtActividadEconomica = Ln.ListarActividadEconomica();
                util.CargaDDL(ddlActividad, dtActividadEconomica, "Nombre", "Id");
            }
            catch
            {
                Error(Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFEMPRESACHIS));
            }
        }

        private void CargarDatosEmpresa()
        {
            txtEmpleados.Text = (datosEmpresa._numEmpleados == 0 ? "" : datosEmpresa._numEmpleados.ToString());
            ddlTipoE.SelectedValue = datosEmpresa._idTipoEmpresa.ToString();

            //Bloqueo Cliente
            ddlBloqueado.SelectedValue = datosEmpresa._bloqueado.ToString();
            ddlMotivoBloqueo.SelectedValue = datosEmpresa._idMotivoBloqueo.ToString();

            //GrupoEconomico
            ddlPerteneceGrupo.SelectedValue = datosEmpresa._PerteneceGrupo.ToString();
            ddlGrupoEcono.SelectedValue = datosEmpresa._idGrupoEconomico.ToString();

            ddlActividad.SelectedValue = datosEmpresa._idActividad.ToString();
            if (datosEmpresa._fecIniAct != null)
                dtcIniciActEconomica.SelectedDate = datosEmpresa._fecIniAct;

            if (datosEmpresa._fecIniContrato != null)
                dtcFechaInicioContrato.SelectedDate = datosEmpresa._fecIniContrato.Value;

            if (datosEmpresa._fecFinContrato != null)
            {
                txtFechaFinContrato.Text = datosEmpresa._fecFinContrato.Value.ToString("dd-MM-yyyy");
                ViewState["FechaFinContrato"] = txtFechaFinContrato.Text.Trim();
                //dtcFechaFinContrato.SelectedDate = datosEmpresa._fecFinContrato;
            }
            else
                ViewState["FechaFinContrato"] = "";

            txtFechaFinContrato.Enabled = false;
            txtFijo.Text = datosEmpresa._telefono1.ToString();
            //txtDir.Text = datosEmpresa._calle.ToString();
            txtMailC.Text = datosEmpresa._email.ToString();
            txtNombreEmpresa.Text = datosEmpresa._razonSocial.ToString();
            txtNombreAntiguo.Text = datosEmpresa._razonSocial.ToString();
            //clasificacion sbif y paf 02-03-2017
            txtSbif.Text = datosEmpresa._ClasificacionSBIF;
            //txtSbif.Enabled = false;
            txtPaf.Text = datosEmpresa._ClasificacionPAF;
            //txtPaf.Enabled = false;
            txtSII.Text = datosEmpresa._ObjetoSII;

        }

        protected void Grabar()
        {
            asignacionResumen(ref objresumen);

            datosEmpresa = new Empresa();
            datosEmpresa = new Empresa { }.TraeEmpresasClase(objresumen.idEmpresa);
            LogicaNegocio Ln = new LogicaNegocio();

            DataTable dtValidacion = new DataTable();
            DataTable dtReglaNegocio = new DataTable();
            DataTable dtAlertas = new DataTable();

            dtValidacion = Ln.ValidarEmpresa(objresumen.idEmpresa, objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo);
            dtReglaNegocio = Ln.ReglaNegocioEmpresa(objresumen.idEmpresa, objresumen.idOperacion);


            if (dtValidacion.Rows.Count > 0 && dtReglaNegocio.Rows.Count > 0)
            {
                if (dtValidacion.Rows[0][0].ToString() == "EXITO" && dtReglaNegocio.Rows[0][0].ToString() == "EXITO")
                {
                    if (datosEmpresa._idEmpresa == 0)
                        datosEmpresa._idEmpresa = objresumen.idEmpresa;
                    if (!string.IsNullOrEmpty(lbEmpresa.Text))
                        datosEmpresa._razonSocial = lbEmpresa.Text;

                    // EMPLEADOS
                    if (!string.IsNullOrEmpty(txtEmpleados.Text))
                        datosEmpresa._numEmpleados = Convert.ToInt32(txtEmpleados.Text);
                    else
                    {
                        //  valida = false;
                        datosEmpresa._numEmpleados = 0;
                    }

                    // TIPO EMPRESA
                    if (ddlTipoE.SelectedValue != "0")
                    {
                        datosEmpresa._idTipoEmpresa = Convert.ToInt32(ddlTipoE.SelectedValue);
                        datosEmpresa._tipoEmpresa = ddlTipoE.SelectedItem.Text;
                    }
                    else
                    {
                        datosEmpresa._idTipoEmpresa = 0;
                        datosEmpresa._tipoEmpresa = "";
                        //  valida = false;
                    }

                    if (ddlEjecutivo.SelectedValue != "0")
                    {
                        datosEmpresa._idEjecutivo = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ddlEjecutivo.SelectedValue));
                        datosEmpresa._descEjecutivo = System.Web.HttpUtility.HtmlDecode(ddlEjecutivo.SelectedItem.Text);
                    }
                    else
                    {
                        datosEmpresa._idEjecutivo = 0;
                        datosEmpresa._descEjecutivo = "";
                    }
                    datosEmpresa._bloqueado = Convert.ToInt32(ddlBloqueado.SelectedValue);
                    datosEmpresa._idMotivoBloqueo = Convert.ToInt32(ddlMotivoBloqueo.SelectedValue);
                    datosEmpresa._descMotivoBloqueo = ddlMotivoBloqueo.SelectedItem.Text;

                    datosEmpresa._PerteneceGrupo = Convert.ToInt32(ddlPerteneceGrupo.SelectedValue);
                    datosEmpresa._idGrupoEconomico = Convert.ToInt32(ddlGrupoEcono.SelectedValue);
                    datosEmpresa._descGrupoEconomico = ddlGrupoEcono.SelectedItem.Text;

                    // ACTIVIDAD
                    if (ddlActividad.SelectedValue != "0")
                    {
                        datosEmpresa._idActividad = Convert.ToInt32(ddlActividad.SelectedValue);
                        datosEmpresa._actividad = ddlActividad.SelectedItem.Text;
                    }
                    else
                    {
                        datosEmpresa._idActividad = 0;
                        datosEmpresa._actividad = "";
                    }

                    // TELEFONO
                    if (!string.IsNullOrEmpty(txtFijo.Text))
                        datosEmpresa._telefono1 = txtFijo.Text;
                    else
                    {
                        datosEmpresa._telefono1 = "";
                    }

                    // MAIL
                    if (!string.IsNullOrEmpty(txtMailC.Text))
                        datosEmpresa._email = txtMailC.Text;
                    else
                        datosEmpresa._email = "";

                    datosEmpresa._razonSocial = txtNombreEmpresa.Text;
                    datosEmpresa._fecIniAct = dtcIniciActEconomica.SelectedDate;

                    if (dtcFechaInicioContrato.IsDateEmpty)
                        datosEmpresa._fecIniContrato = null;
                    else
                        datosEmpresa._fecIniContrato = dtcFechaInicioContrato.SelectedDate;
                    if (!string.IsNullOrEmpty(hfFecha.Value.ToString()))
                        datosEmpresa._fecFinContrato = util.ValidarFecha(hfFecha.Value);

                    datosEmpresa._validacion = 1;

                    if (!string.IsNullOrEmpty(txtSII.Text))
                        datosEmpresa._ObjetoSII = txtSII.Text.Trim();

                    using (SPWeb oWeb = SPContext.Current.Site.OpenWeb(SPContext.Current.Web.Url))
                    {
                        SPUser oUser = SPContext.Current.Web.CurrentUser;
                        strRes = new Empresa { }.IngresaActualizaEmpresa(datosEmpresa, "02", oUser.Name, oUser.ID, 0, "");

                        objresumen.desEmpresa = datosEmpresa._razonSocial;
                        objresumen.descEjecutivo = datosEmpresa._descEjecutivo;
                        lbEmpresa.Text = datosEmpresa._razonSocial;
                        ViewState["RES"] = objresumen;

                        lblRut.Text = objresumen.rut.Split('-')[0];
                        lblDv.Text = objresumen.rut.Split('-')[1];
                        lblRazon.Text = objresumen.desEmpresa;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                    }

                    if (strRes.Contains("-"))
                        Error(strRes);
                    else
                    {
                        string nombreAntiguo = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(txtNombreAntiguo.Text));
                        nombreAntiguo = nombreAntiguo.Length > 100 ? nombreAntiguo.Substring(0, 100) : nombreAntiguo;
                        nombreAntiguo = lbRut.Text.Split('-')[0] + "_" + nombreAntiguo;

                        string nombreNuevo = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(txtNombreEmpresa.Text));
                        nombreNuevo = nombreNuevo.Length > 100 ? nombreNuevo.Substring(0, 100) : nombreNuevo;
                        nombreNuevo = lbRut.Text.Split('-')[0] + "_" + nombreNuevo;

                        SPSite site = SPContext.Current.Site;
                        {
                            using (SPWeb app = site.OpenWeb())
                            {
                                SPSecurity.RunWithElevatedPrivileges(delegate
                                {
                                    app.AllowUnsafeUpdates = true;
                                    var listData = app.Lists["Documentos"];
                                    var urlLibrary = app.Url + "/Documentos";

                                    if (!app.GetFolder(urlLibrary + "/" + util.RemoverSignosAcentos(nombreAntiguo)).Exists)
                                    {
                                        //carpetaCliente = listData.Folders.Add(urlLibrary, SPFileSystemObjectType.Folder, newFolder);
                                        //carpetaCliente.Update();
                                        //listData.Update();
                                    }
                                    else
                                    {
                                        TargetFolder = app.GetFolder(urlLibrary + "/" + util.RemoverSignosAcentos(nombreAntiguo));
                                        TargetFolder.Item["Name"] = util.RemoverSignosAcentos(nombreNuevo);
                                        TargetFolder.Item.Update();
                                        txtNombreAntiguo.Text = txtNombreEmpresa.Text;
                                    }
                                }
                             );
                            }
                        }

                        CargarDatosEmpresa();

                        lbExito.Text = "La empresa " + txtNombreEmpresa.Text + " se ha actualizado existosamente";
                        dvExito.Style.Add("display", "block");
                        pnlEdit.Enabled = false;
                        //pnlContrato.Enabled = false;
                        btnGuardar.Attributes.Add("style", "display:none");
                        btnLimpiar.Attributes.Add("style", "display:none");
                    }

                    datosEmpresa = new Empresa();
                }
                else
                {
                    String mensajeValidacion = String.Empty;
                    for (int h = 0; h < dtValidacion.Rows.Count; h++)
                    {
                        mensajeValidacion = mensajeValidacion + dtValidacion.Rows[h][0].ToString();
                    }
                    String mensajeReglaNegocio = String.Empty;
                    for (int i = 0; i < dtReglaNegocio.Rows.Count; i++)
                    {
                        mensajeReglaNegocio = mensajeReglaNegocio + dtReglaNegocio.Rows[i][0];
                    }
                    if (mensajeReglaNegocio != "EXITO")
                    {
                        lbError.Text = mensajeValidacion + " - " + mensajeReglaNegocio;
                    }
                    Error(mensajeValidacion);

                }
            }
            else
                Error("Se debe verificar la validación de los datos. Comuníquese con el administrador.");
        }

        //protected void LlenaFormulario()
        //{
        //    txtEmpleados.Text = datosEmpresa._numEmpleados.ToString();
        //    txtMailC.Text = datosEmpresa._email;
        //    txtFijo.Text = datosEmpresa._telefono1.ToString();
        //    ddlTipoE.SelectedValue = datosEmpresa._idTipoEmpresa.ToString();
        //    ddlActividad.SelectedValue = datosEmpresa._idActividad.ToString();
        //}

        //protected void lbContactos_Click(object sender, EventArgs e)
        //{
        //    asignacionResumen(ref objresumen);
        //    vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EdicionEmpresa");
        //    Page.Session["RESUMEN"] = objresumen;
        //    Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
        //    Page.Response.Redirect("ContactosEmpresa.aspx");
        //}

        #endregion

        //#region "bloquear"

        ////protected override void OnPreRender(EventArgs e)
        ////{
        ////    base.OnPreRender(e);
        ////    if (ViewState["validar"] != null)
        ////    {            
        ////        ValidarPermisos validar = ((ValidarPermisos)ViewState["validar"]);
        ////        BloquearControl(false, validar.Permiso);
        ////    }
        ////}

        //#endregion

    }
}
