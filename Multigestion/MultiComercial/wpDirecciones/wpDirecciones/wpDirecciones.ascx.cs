using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using FrameworkIntercapIT.Utilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using System.Web.UI;
using System.Web;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpDirecciones.wpDirecciones
{
    [ToolboxItemAttribute(false)]
    public partial class wpDirecciones : WebPart
    {
        private static string[] Cargos = { "Administrador", "Jefe Operaciones", "Analista Operaciones" };
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        int i;
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        //public ListItem LstItem;
        //public SPView view;
        //public SPList listData;
        //public SPListItemCollection items;
        //public SPWeb app;
        private static string pagina = "DireccionEmpresa.aspx";

        public wpDirecciones()
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
            ((TextBox)(dtcFechaVerificacion.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");
            ((TextBox)(dtcFechaVerificacion.Controls[0])).MaxLength = 10;

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
                        try
                        {
                            btnGuardar.OnClientClick = "return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                            objresumen = (Resumen)Page.Session["RESUMEN"];
                            Page.Session["RESUMEN"] = null;
                            CargaRegion();
                            CargarTipoDireccion();
                            CargarEstadoVerificacion();

                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            lbEjecutivo.Text = objresumen.descEjecutivo;
                            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;

                            //Verificación Edicion Simultanea        
                            string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones", "Empresa");
                            if (!string.IsNullOrEmpty(UsuarioFormulario))
                            {
                                dvWarning.Style.Add("display", "block");
                                lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                                btnCancel.Style.Add("display", "none");
                                btnGuardar.Style.Add("display", "none");
                                pnFormulario.Enabled = false;
                                objresumen.idPermiso = Constantes.PERMISOS.SOLOLECTURA.ToString();
                                //lbEditar.Visible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }

                        if (!util.EstaPermitido(dt.Rows[0]["descCargo"].ToString(), Cargos))
                        {
                            DivVerificaion.Visible = false;
                            DivFechaVerificacion.Visible = false;
                        }
                    }
                    else
                        Page.Response.Redirect("MensajeSession.aspx");
                }
                llenargrid();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    warning("Usuario sin permisos configurados");
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                warning("Usuario sin permisos configurados");
            }
        }

        private void warning(string mensaje)
        {
            dvWarning1.Style.Add("display", "block");
            lbWarning1.Text = mensaje.Trim();
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            string fecha;
            if (!dtcFechaVerificacion.IsDateEmpty)
                fecha = dtcFechaVerificacion.SelectedDate.Day.ToString() + "-" + dtcFechaVerificacion.SelectedDate.Month.ToString() + "-" + dtcFechaVerificacion.SelectedDate.Year.ToString();
            else
                fecha = "-1";

            try
            {
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    //id nodo.AppendChild(doc.CreateTextNode(((DropDownList)Controles).SelectedValue.ToString()));
                    //desc nodo1.AppendChild(doc.CreateTextNode(((DropDownList)Controles).SelectedItem.ToString()));
                    bool ec = Ln.InsertarDireccionxEmpresa(objresumen.idEmpresa.ToString(), ddlTipo.SelectedValue.ToString(), ddlTipo.SelectedItem.ToString(),
                        chkprincipal.Checked, txtdireccion.Text, ddlRegion.SelectedValue.ToString(), ddlRegion.SelectedItem.ToString(),
                        ddlProvincia.SelectedValue.ToString(), ddlProvincia.SelectedItem.ToString(), ddlComunas.SelectedValue.ToString(), ddlComunas.SelectedItem.ToString(),
                         int.Parse(ddlVerificacion.SelectedValue), System.Web.HttpUtility.HtmlDecode(ddlVerificacion.SelectedItem.Text), fecha, objresumen.idUsuario, objresumen.desPermiso, txtNumero.Text.Trim(), txtComplementoDireccion.Text.Trim());
                    if (ec)
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETEMPRESARELAC);
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTEMPRESARELAC);
                    }

                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {//ojo quitar e 1 HddIdContactoEmpresa.Value.ToString(),
                    bool ec = Ln.ModificarDireccionxEmpresa(HddIdDireccionEmpresa.Value.ToString(), objresumen.idEmpresa.ToString(), ddlTipo.SelectedValue.ToString(), ddlTipo.SelectedItem.ToString(),
                            chkprincipal.Checked, txtdireccion.Text, ddlRegion.SelectedValue.ToString(), ddlRegion.SelectedItem.ToString(),
                            ddlProvincia.SelectedValue.ToString(), ddlProvincia.SelectedItem.ToString(), ddlComunas.SelectedValue.ToString(), ddlComunas.SelectedItem.ToString(),
                             int.Parse(ddlVerificacion.SelectedValue), System.Web.HttpUtility.HtmlDecode(ddlVerificacion.SelectedItem.Text), fecha, objresumen.idUsuario, objresumen.desPermiso, txtNumero.Text.Trim(), txtComplementoDireccion.Text.Trim());
                    if (ec)
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFEMPRESARELAC);
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFEMPRESARELAC);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = " Ha ocurrido un error interno al guardar dirección Empresa.  Detalle: " + ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            llenargrid();
            CargaRegion();
            Limpiar();
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
        }

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false; //idempresadireccion
            e.Row.Cells[1].Visible = false;//idempresa
            e.Row.Cells[2].Visible = false;//id tipo
            e.Row.Cells[3].Visible = false;//id region
            e.Row.Cells[4].Visible = false;//id comuna
            e.Row.Cells[5].Visible = false;//idprovincia
            //e.Row.Cells[12].Visible = false; //DescVerificacion (Duplicado)
            e.Row.Cells[15].Visible = false;//IdVerificacion
            e.Row.Cells[16].Visible = false;//DescVerificacion
            e.Row.Cells[17].Visible = false;//numero
            e.Row.Cells[18].Visible = false;//ComplementoDireccion
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = i.ToString();
                i++;
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusqueda_Command;
                e.Row.Cells[14].Text = "";
                e.Row.Cells[14].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[14].Controls.Add(lb2);
            }
            else
                e.Row.Cells[14].Text = "Acción";
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                if (e.CommandName == "Eliminar")
                {
                    LogicaNegocio Ln = new LogicaNegocio();
                    bool principal = false;
                    if (!EsElUltimoContacto(ref principal, Convert.ToInt32(e.CommandArgument)))
                    {
                        if (Ln.EliminarDireccionxEmpresa(objresumen.idEmpresa.ToString(), e.CommandArgument.ToString(), objresumen.idUsuario, objresumen.idCargo))
                        {
                            dvSuccess.Style.Add("display", "block");
                            lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOELIMEMPRESARELAC);
                        }
                        else
                        {
                            dvError.Style.Add("display", "block");
                            lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORELIMEMPRESARELAC);
                        }

                        llenargrid();
                        Limpiar();
                    }
                    else
                    {
                        dvWarning.Style.Add("display", "block");
                        if (!principal)
                            lbWarning.Text = Ln.buscarMensaje(24);//Constantes.MENSAJES.ULTIMOCONTACTO);
                        else
                            lbWarning.Text = Ln.buscarMensaje(25);//Constantes.MENSAJES.SINPRINCIPAL);
                    }
                }

                if (e.CommandName == "Editar")
                {
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    int index = Convert.ToInt32(e.CommandArgument);
                    HddIdDireccionEmpresa.Value = ResultadosBusqueda.Rows[index].Cells[0].Text;
                    ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");
                    if ((ResultadosBusqueda.Rows[index].Cells[2].Text.Trim()) != "&nbsp;")
                    {
                        ddlTipo.SelectedIndex = ddlTipo.Items.IndexOf(ddlTipo.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.Trim())));
                        //ddlTipo.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString());
                    }

                    if ((ResultadosBusqueda.Rows[index].Cells[3].Text) != "&nbsp;")
                    {
                        //ddlRegion.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[3].Text.Trim());
                        ddlRegion.SelectedIndex = ddlRegion.Items.IndexOf(ddlRegion.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[3].Text.Trim())));
                        CargaProvincia(int.Parse(ddlRegion.SelectedItem.Value));
                    }
                    if ((ResultadosBusqueda.Rows[index].Cells[5].Text) != "&nbsp;")
                    {
                        ddlProvincia.SelectedIndex = ddlProvincia.Items.IndexOf(ddlProvincia.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.Trim())));
                        if (ddlProvincia.SelectedIndex > 0)
                        {
                            ddlProvincia.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.Trim());
                            CargaComunas(int.Parse(ddlProvincia.SelectedItem.Value));
                        }
                        //if (ddlProvincia.SelectedItem.Value.Contains(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.Trim())))
                        //{
                        //    ddlProvincia.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.Trim());
                        //    CargaComunas(int.Parse(ddlProvincia.SelectedItem.Value));
                        //}
                    }

                    if ((ResultadosBusqueda.Rows[index].Cells[4].Text) != "&nbsp;")
                        ddlComunas.SelectedIndex = ddlComunas.Items.IndexOf(ddlComunas.Items.FindByValue(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.Trim())));
                    //ddlComunas.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.Trim());

                    if (!string.IsNullOrEmpty(ResultadosBusqueda.Rows[index].Cells[7].Text.Trim()))
                        txtdireccion.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.Trim());

                    if (ResultadosBusqueda.Rows[index].Cells[11].Text != "&nbsp;")
                    {
                        string dia = ((ResultadosBusqueda.Rows[index].Cells[11].Text.Trim()).Split('-'))[0];
                        string mes = ((ResultadosBusqueda.Rows[index].Cells[11].Text.Trim()).Split('-'))[1];
                        string año = ((ResultadosBusqueda.Rows[index].Cells[11].Text.Trim()).Split('-'))[2];
                        DateTime fecha = Convert.ToDateTime(mes + "-" + dia + "-" + año);

                        dtcFechaVerificacion.SelectedDate =
                            DateTime.ParseExact(fecha.Day.ToString("00") + fecha.Month.ToString("00") + fecha.Year.ToString(), "ddMMyyyy", CultureInfo.InvariantCulture);
                    }

                    if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[13].Text) == "SI")
                        chkprincipal.Checked = true;
                    else
                        chkprincipal.Checked = false;

                    if ((ResultadosBusqueda.Rows[index].Cells[15].Text) != "&nbsp;")
                    {
                        CargarEstadoVerificacion();
                        ddlVerificacion.SelectedIndex = ddlVerificacion.Items.IndexOf(ddlVerificacion.Items.FindByText(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[15].Text.Trim())));
                        //ddlVerificacion.SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[15].Text.Trim());
                    }

                    if (!string.IsNullOrEmpty(ResultadosBusqueda.Rows[index].Cells[17].Text))
                        txtNumero.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[17].Text.Trim());

                    if (!string.IsNullOrEmpty(ResultadosBusqueda.Rows[index].Cells[18].Text))
                        txtComplementoDireccion.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[18].Text.Trim());
                }
            }
            catch (Exception)
            {

            }
        }

        protected void lbContactos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ContactosEmpresa.aspx");
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDirectorio_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DirectorioEmpresa.aspx");
        }

        protected void lbPersonas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("personas.aspx");
        }

        protected void lbSociosAccionistas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("SociosAccionistas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        protected void lbProrrateo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ProrrateoGarantias.aspx");
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Direcciones");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProvincia.Items.Clear();
            ddlComunas.Items.Clear();

            if (ddlRegion.SelectedItem.Value != "0")
                CargaProvincia(int.Parse(ddlRegion.SelectedItem.Value));
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlComunas.Items.Clear();
            if (ddlProvincia.SelectedIndex > 0)
                CargaComunas(int.Parse(ddlProvincia.SelectedItem.Value));
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
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void Limpiar()
        {
            ddlTipo.SelectedValue = "0";
            ddlComunas.SelectedValue = "0";
            ddlProvincia.SelectedValue = "0";
            ddlRegion.SelectedValue = "0";
            ddlProvincia.Items.Clear();
            ddlComunas.Items.Clear();
            dtcFechaVerificacion.SelectedDate = new DateTime();
            txtdireccion.Text = "";
            chkprincipal.Checked = false;
            ddlVerificacion.SelectedIndex = 0;
            txtNumero.Text = "";
            txtComplementoDireccion.Text = "";
            ((TextBox)(dtcFechaVerificacion.Controls[0])).Text = "";
        }

        private bool EstaRegistrado(string rut)
        {
            DataTable t = new DataTable();
            t = (DataTable)Page.Session["tabla"];
            int i, lim = t.Rows.Count;
            try
            {
                for (i = 0; i < lim; i++)
                    if (ResultadosBusqueda.Rows[i].Cells[1].Text.ToString() == rut)
                        return true;
                return false;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        private bool EsElUltimoContacto(ref bool principal, int id)
        {
            DataTable t = new DataTable();
            t = (DataTable)Page.Session["tabla"];
            int i, lim = t.Rows.Count;
            if (lim < 2)
                return true;
            for (i = 0; i < lim; i++)
                if (ResultadosBusqueda.Rows[i].Cells[8].Text.ToString() == "SI" && ResultadosBusqueda.Rows[i].Cells[0].Text.ToString() == id.ToString())
                {
                    principal = true;
                    return true;
                }
            principal = false;
            return false;
        }

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res = new DataTable();
            i = 0;
            try
            {
                res = Ln.ListarDireccionxEmpresa(objresumen.idEmpresa.ToString());
                ResultadosBusqueda.DataSource = res;
                Page.Session["tabla"] = res;
                ResultadosBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        private void CargaRegion()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                var dt = Ln.ListarRegion();

                util.CargaDDL(ddlRegion, dt, "NombreRegion", "IdRegion");
            }
            catch (Exception ex)
            {
                lbError.Text = "Ha ocurrido un error al cargar los datos de la Región";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarTipoDireccion()
        {
            try
            {
                ddlTipo.Items.Clear();
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtTipoDireccion = Ln.ListarTipoDireccion();
                util.CargaDDL(ddlTipo, dtTipoDireccion, "Nombre", "Id");
            }
            catch (Exception ex)
            {
                lbError.Text = "Ha ocurrido un error al cargar los datos del tipo de direccion";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargaProvincia(int idregion)
        {
            try
            {
                ddlProvincia.Items.Clear();
                ddlComunas.Items.Clear();

                LogicaNegocio Ln = new LogicaNegocio();
                var dt = Ln.ListarProvincia(idregion);

                util.CargaDDL(ddlProvincia, dt, "DescCiudad", "IdCiudad");
            }
            catch (Exception ex)
            {
                lbError.Text = "Ha ocurrido un error al cargar los datos de la Provincia";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }


        private void CargaComunas(int idProv)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                var dt = Ln.ListarComunas(idProv);

                util.CargaDDL(ddlComunas, dt, "NombreComuna", "IdComuna");
            }
            catch (Exception ex)
            {
                lbError.Text = "Ha ocurrido un error al cargar los datos de la comuna";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarEstadoVerificacion()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEstVerificacion = Ln.ListarEstadoVerificacionDireccion();
            util.CargaDDL(ddlVerificacion, dtEstVerificacion, "Nombre", "Id");
        }

        #endregion

    }
}
