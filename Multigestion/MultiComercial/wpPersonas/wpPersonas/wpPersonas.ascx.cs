using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpPersonas.wpPersonas
{
    [ToolboxItemAttribute(false)]
    public partial class wpPersonas : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPersonas()
        {
        }

        private static string pagina = "Personas.aspx";
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        int i;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            //PERMISOS USUARIOS
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
                    asignacionesJS();
                    ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                        objresumen = (Resumen)Page.Session["RESUMEN"];

                        ViewState["RES"] = objresumen;
                        Page.Session["RESUMEN"] = null;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        llenarDdlCargos();
                        CargarEstadoCivil();
                        txt_nombre.ToolTip = "Debe Incluir todos los nombres y dos apellidos";
                        
                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas", "Empresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnLimpiar.Style.Add("display", "none");
                            btnGuardar.Style.Add("display", "none");
                            objresumen.idPermiso = Constantes.PERMISOS.SOLOLECTURA.ToString();
                            pnFormulario.Enabled = false;
                        }
                    }
                    else
                        Page.Response.Redirect("MensajeSession.aspx");
                }
                else
                    ocultarDiv();
                
                llenarGrid();

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

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            ocultarDiv();
            try
            {
                string descRegimen = string.Empty;
                int Regimen = 0;
                string montoProtesto = txt_protesto.Text.Replace(".", "").Replace(",", ".");
                string montoMorosidad = txt_morosidad.Text.Replace(".", "").Replace(",", ".");
                string montoPatrimonio = txt_patrimonio.Text.Replace(".", "").Replace(",", ".");
                string montoParticipacion = txt_participacion.Text.Replace(".", "").Replace(",", ".");
                string antiguedad = txt_antiguedad.Text;
                string fecha = dtcFecNaci.SelectedDate.ToString("MM-dd-yyyy");
                montoProtesto = string.IsNullOrWhiteSpace(montoProtesto) ? "0" : montoProtesto;
                montoMorosidad = string.IsNullOrWhiteSpace(montoMorosidad) ? "0" : montoMorosidad;
                montoPatrimonio = string.IsNullOrWhiteSpace(montoPatrimonio) ? "0" : montoPatrimonio;
                montoParticipacion = string.IsNullOrWhiteSpace(montoParticipacion) ? "0" : montoParticipacion;
                antiguedad = string.IsNullOrWhiteSpace(antiguedad) ? "0" : antiguedad;

                if (ddlRegimen.SelectedItem == null)
                {
                    descRegimen = "";
                    Regimen = 0;
                }
                else
                {
                    descRegimen = ddlRegimen.SelectedItem.Text == "Seleccione" || ddlRegimen.SelectedItem == null ? "" : ddlRegimen.SelectedItem.Text.Trim();
                    Regimen = ddlRegimen.SelectedValue == "0" || ddlRegimen.SelectedValue == null ? 0 : int.Parse(ddlRegimen.SelectedValue);
                }

                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtValidacion = new DataTable();
                dtValidacion = Ln.ValidarPersonas(int.Parse(hdnIdPersona.Value), objresumen.idEmpresa, txt_rut.Text.Trim(), txt_divRut.Text.Trim(), fecha, txt_nombre.Text.Trim(), ddlEdoCivil.SelectedItem.Value, ddlEdoCivil.SelectedItem.Text, ddlCargos.SelectedItem.Text, antiguedad, txt_profesion.Text.Trim(), txt_universidad.Text.Trim(), montoParticipacion, montoProtesto, montoMorosidad, montoPatrimonio, txt_email.Text.Trim(), txt_celular.Text.Trim(), txt_telFijo.Text.Trim(), ddlContactoPrincipal.SelectedItem.Value, ddlSocios.SelectedItem.Value, ddlDirectorio.SelectedItem.Value, ddlContacto.SelectedItem.Value, ddlRteLegal.SelectedItem.Value, objresumen.idUsuario, ViewState["ACCION"].ToString());
                
                if (dtValidacion.Rows.Count > 0)
                {
                    if (dtValidacion.Rows[0][0].ToString() == "EXITO")
                    {

                        if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                        {
                            if (Ln.InsertarPersonaxEmpresa(objresumen.idEmpresa, txt_rut.Text.Trim(), txt_divRut.Text.Trim(), dtcFecNaci.SelectedDate.ToString(), txt_nombre.Text.Trim(), ddlEdoCivil.SelectedItem.Value, ddlEdoCivil.SelectedItem.Text, ddlCargos.SelectedItem.Value, ddlCargos.SelectedItem.Text, antiguedad, txt_profesion.Text.Trim(), txt_universidad.Text.Trim(), montoParticipacion, montoProtesto, montoMorosidad, montoPatrimonio, txt_email.Text.Trim(), txt_celular.Text.Trim(), txt_telFijo.Text.Trim(), ddlContactoPrincipal.SelectedItem.Value, ddlSocios.SelectedItem.Value, ddlDirectorio.SelectedItem.Value, ddlContacto.SelectedItem.Value, ddlRteLegal.SelectedItem.Value, objresumen.idUsuario, Regimen, descRegimen))
                            {
                                dvSuccess.Style.Add("display", "block");
                                lbSuccess.Text = "Se ha insertado la Persona con éxito";
                                ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                                hdnIdPersona.Value = "-1";
                                LimpiarCampos();
                            }
                            else
                            {
                                dvError.Style.Add("display", "block");
                                lbError.Text = "La Persona no se ha insertado correctamente";
                            }
                        }
                        if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                        {
                            if (Ln.ModificarPersonaxEmpresa(int.Parse(hdnIdPersona.Value), objresumen.idEmpresa, txt_rut.Text.Trim(), txt_divRut.Text.Trim(), dtcFecNaci.SelectedDate.ToString(), txt_nombre.Text.Trim(), ddlEdoCivil.SelectedItem.Value, ddlEdoCivil.SelectedItem.Text, ddlCargos.SelectedItem.Value, ddlCargos.SelectedItem.Text, antiguedad, txt_profesion.Text, txt_universidad.Text.Trim(), montoParticipacion, montoProtesto, montoMorosidad, montoPatrimonio, txt_email.Text.Trim(), txt_celular.Text.Trim(), txt_telFijo.Text.Trim(), ddlContactoPrincipal.SelectedItem.Value, ddlSocios.SelectedItem.Value, ddlDirectorio.SelectedItem.Value, ddlContacto.SelectedItem.Value, ddlRteLegal.SelectedItem.Value, objresumen.idUsuario, Regimen, descRegimen))
                            {
                                dvSuccess.Style.Add("display", "block");
                                lbSuccess.Text = "Se ha modificado la Persona disponible correctamente";
                                ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                                hdnIdPersona.Value = "-1";
                                LimpiarCampos();
                            }
                            else
                            {
                                dvError.Style.Add("display", "block");
                                lbError.Text = "No se ha completado la modificación de la Persona";
                            }
                        }
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        String mensajeValidacion = String.Empty;
                        for (int h = 0; h < dtValidacion.Rows.Count; h++)
                        {
                            mensajeValidacion = mensajeValidacion + dtValidacion.Rows[h][0].ToString();
                        }
                        //String mensajeReglaNegocio = String.Empty;
                        //for (int i = 0; i < dtReglaNegocio.Rows.Count; i++)
                        //{
                        //    mensajeReglaNegocio = mensajeReglaNegocio + dtReglaNegocio.Rows[i][0];
                        //}
                        //if (mensajeReglaNegocio != "EXITO")
                        //{
                        //    lbError.Text = mensajeValidacion + " - " + mensajeReglaNegocio;
                        //}
                        lbError.Text = mensajeValidacion;
                    }
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "Se debe verificar la validación de los datos. Comuníquese con el administrador.";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            llenarDdlCargos();
            llenarGrid();
        }

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[14].Visible = false;
            e.Row.Cells[15].Visible = false;
            e.Row.Cells[16].Visible = false;
            e.Row.Cells[17].Visible = false;
            e.Row.Cells[18].Visible = false;
            e.Row.Cells[19].Visible = false;
            e.Row.Cells[20].Visible = false;

            //if (objresumen.idPermiso == Constantes.PERMISOS.SOLOLECTURA.ToString())
            //{
            //    e.Row.Cells[25].Visible = false;
            //}
            e.Row.Cells[26].Visible = false;
            e.Row.Cells[27].Visible = false;
            e.Row.Cells[28].Visible = false;
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.ID = "Editar";
                lb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = i.ToString();
                i++;
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusqueda_Command;
                e.Row.Cells[25].Text = "Editar";
                e.Row.Cells[25].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.ID = "Eliminar";
                lb2.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.OnClientClick = "return Dialogo();";

                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[25].Controls.Add(lb2);
            }

            //if (objresumen.idPermiso == Constantes.PERMISOS.SOLOLECTURA.ToString())
            //{
            //    asignacionResumen(ref objresumen);
            //    e.Row.Cells[25].Visible = false;
            //}
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            llenarGrid();
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            if (e.CommandName == "Eliminar")
            {
                int idEmpresaPersonas = Convert.ToInt32(e.CommandArgument.ToString());
                if (Ln.EliminarPersonaxEmpresa(idEmpresaPersonas, objresumen.idUsuario))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Se ha eliminado correctamente la Persona";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "No se ha eliminado la Persona. Contáctese con el administrador del sitio";
                }
                llenarGrid();
                LimpiarCampos();
            }

            if (e.CommandName == "Editar")
            {
                try
                {
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    int index = Convert.ToInt32(e.CommandArgument);
                    ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");

                    txt_rut.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text).Trim();
                    txt_divRut.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[3].Text).Trim();
                    //4 Rut+DivRut
                    //5 Fecha ->
                    String srtFecha = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text).Trim();

                    if (!string.IsNullOrWhiteSpace(srtFecha))
                    {
                        dtcFecNaci.SelectedDate = DateTime.ParseExact(srtFecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                    txt_nombre.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text).Trim();

                    //ESATDO CIVIL
                    if ((ResultadosBusqueda.Rows[index].Cells[7].Text.ToString()) != "&nbsp;")
                    {
                        ddlEdoCivil.SelectedIndex = ddlEdoCivil.Items.IndexOf(ddlEdoCivil.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text)));
                        //ddlEdoCivil.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text);
                        CargarEstadoCivil();
                    }


                    if ((ResultadosBusqueda.Rows[index].Cells[27].Text.ToString()) != "&nbsp;")
                    {
                        ddlRegimen.SelectedIndex = ddlRegimen.Items.IndexOf(ddlRegimen.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[27].Text)));
                        //ddlRegimen.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[27].Text);
                        CargarRegimen(ResultadosBusqueda.Rows[index].Cells[27].Text);                  
                    }

                    if (!string.IsNullOrWhiteSpace(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text)))
                    {
                        llenarDdlCargos();
                        ddlCargos.SelectedIndex = ddlCargos.Items.IndexOf(ddlCargos.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text)));
                        //ddlCargos.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text);
                    }
                    else
                        llenarDdlCargos();

                    txt_antiguedad.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text).Trim();
                    txt_profesion.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text).Trim();
                    txt_universidad.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[12].Text).Trim();
                    txt_participacion.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[13].Text).Trim();
                    txt_protesto.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[14].Text).Trim();
                    txt_morosidad.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[15].Text).Trim();
                    txt_patrimonio.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[16].Text).Trim();
                    txt_email.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[17].Text).Trim();
                    txt_celular.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[18].Text).Trim();
                    txt_telFijo.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[19].Text).Trim();
                    ddlContactoPrincipal.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[20].Text) == "Si" ? "1" : "0";
                    ddlSocios.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[21].Text) == "Si" ? "1" : "0";
                    ddlDirectorio.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[22].Text) == "Si" ? "1" : "0";
                    ddlContacto.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[23].Text) == "Si" ? "1" : "0";
                    ddlRteLegal.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[24].Text) == "Si" ? "1" : "0";

                    hdnIdPersona.Value = ResultadosBusqueda.Rows[index].Cells[0].Text.Trim();
                    txt_nombre.Focus();
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                    Page.Response.Redirect("Mensaje.aspx");
                }
            }
        }

        public void ImageButton2_Click(object sender, EventArgs e)
        {
            DataTable res;
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.buscarDatosRutV2(txt_rut.Text.Replace(".", "") + txt_divRut.Text);
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    txt_nombre.Text = res.Rows[0]["Nombre"].ToString(); //Rows[0]["RazonSocial"].ToString();
                    txt_profesion.Text = res.Rows[0]["Profesion"].ToString();
                    txt_antiguedad.Text = res.Rows[0]["Antiguedad"].ToString();
                    if (!string.IsNullOrEmpty(res.Rows[0]["Cargo"].ToString()))
                        ddlCargos.SelectedIndex = ddlCargos.Items.IndexOf(ddlCargos.Items.FindByText(System.Web.HttpUtility.HtmlDecode(res.Rows[0]["Cargo"].ToString())));
                        //ddlCargos.SelectedValue = System.Web.HttpUtility.HtmlDecode(res.Rows[0]["Cargo"].ToString());

                    if (!string.IsNullOrEmpty(res.Rows[0]["IdEdoCivil"].ToString()))
                        ddlEdoCivil.SelectedIndex = ddlEdoCivil.Items.IndexOf(ddlEdoCivil.Items.FindByText(System.Web.HttpUtility.HtmlDecode(res.Rows[0]["IdEdoCivil"].ToString())));
                        //ddlEdoCivil.SelectedValue = System.Web.HttpUtility.HtmlDecode(res.Rows[0]["IdEdoCivil"].ToString());

                    txt_universidad.Text = res.Rows[0]["Universidad"].ToString();
                    ViewState["IdEmpresaR"] = res.Rows[0]["IdEmpresa"].ToString();
                }
                else
                {
                    ocultarDiv();
                    txt_nombre.Text = "";
                    dvError.Style.Add("display", "block");
                    lbError.Text = "RUT no registrado";
                }
            }
        }

        protected void lbPersonas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("personas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Personas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
            //Page.Response.Redirect("Documental.aspx");
        }

        protected void ddlEdoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarRegimen(ddlEdoCivil.SelectedValue);
        }

        #endregion


        #region Metodos

        private void asignacionesJS()
        {
            txt_morosidad.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txt_morosidad.ClientID + "');");
            txt_protesto.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txt_protesto.ClientID + "');");
            txt_participacion.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txt_protesto.ClientID + "');");
            txt_patrimonio.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txt_protesto.ClientID + "');");
            ddlContacto.Attributes["onchange"] = "ValidarIngresoRut();";
            btnGuardar.OnClientClick = "return validarPersonas('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
        }

        public void asignacionResumen(ref  Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
            {
                Page.Session["Error"] = "Session offline";
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        private void llenarGrid()
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable res;

                res = Ln.ListarPersonasxEmpresa(objresumen.idEmpresa);
                if (res.Rows.Count > 0)
                {
                    ResultadosBusqueda.DataSource = res;
                    ResultadosBusqueda.DataBind();
                    i = 0;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = "llenarGrid Personas" + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        private void CargarEstadoCivil()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEdoCivil = Ln.ListarEstadoCivil();
            util.CargaDDL(ddlEdoCivil, dtEdoCivil, "Nombre", "Id");
        }

        private void CargarRegimen(string IdEstadoCivil)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtRegimen = Ln.ListarRegimen(Convert.ToInt32(IdEstadoCivil));
            util.CargaDDL(ddlRegimen, dtRegimen, "Nombre", "Id");

            if (int.Parse(IdEstadoCivil) == 2 || int.Parse(IdEstadoCivil) == 6)
                divRegimen.Style.Add("display", "block");
            else
                divRegimen.Style.Add("display", "none");
        }

        private void llenarDdlCargos()
        {
            ddlCargos.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtCargos = Ln.ListarCargosPersonas();
            util.CargaDDL(ddlCargos, dtCargos, "Nombre", "Id");

            //SPListItemCollection listaCargos = ConsultaCargosPersonas();
            //ddlCargos.Items.Clear();
            //ListItem li = new ListItem("Seleccione", "0");
            //ddlCargos.Items.Add(li);
            //foreach (SPListItem item in listaCargos)
            //{
            //    ListItem liTemp = new ListItem(item["Title"].ToString(), item["ID"].ToString());
            //    ddlCargos.Items.Add(liTemp);
            //}
        }

        private void LimpiarCampos()
        {
            txt_antiguedad.Text = String.Empty;
            txt_celular.Text = String.Empty;
            txt_divRut.Text = String.Empty;
            txt_email.Text = String.Empty;
            txt_morosidad.Text = String.Empty;
            txt_nombre.Text = String.Empty;
            txt_participacion.Text = String.Empty;
            txt_patrimonio.Text = String.Empty;
            txt_profesion.Text = String.Empty;
            txt_protesto.Text = String.Empty;
            txt_rut.Text = String.Empty;
            txt_telFijo.Text = String.Empty;
            txt_universidad.Text = String.Empty;
            dtcFecNaci.SelectedDate = new DateTime();
            ddlEdoCivil.SelectedIndex = 0;
            divRegimen.Style.Add("display", "none");
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
