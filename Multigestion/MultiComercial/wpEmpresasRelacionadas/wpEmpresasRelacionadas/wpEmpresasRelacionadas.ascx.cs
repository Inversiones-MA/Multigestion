using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpEmpresasRelacionadas.wpEmpresasRelacionadas
{
    [ToolboxItemAttribute(false)]
    public partial class wpEmpresasRelacionadas : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpEmpresasRelacionadas()
        {
        }

        int i;

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "EmpresasRelacionadas.aspx";

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
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                        btnGuardar.OnClientClick = "return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

                        txt_patrimonio.Attributes.Add("onblur", "return FormatoMoneda('" + txt_patrimonio.ClientID + "');");
                        txt_montoInfraccionesLab.Attributes.Add("onblur", "return FormatoMoneda('" + txt_montoInfraccionesLab.ClientID + "');");
                        txt_montoInfraccionesPrev.Attributes.Add("onblur", "return FormatoMoneda('" + txt_montoInfraccionesPrev.ClientID + "');");
                        txt_montoMorosidades.Attributes.Add("onblur", "return FormatoMoneda('" + txt_montoMorosidades.ClientID + "');");
                        txt_MontoProtestos.Attributes.Add("onblur", "return FormatoMoneda('" + txt_MontoProtestos.ClientID + "');");
                        txt_patrimonio.Attributes.Add("onKeyPress", "return solonumerosCant(event,'" + txt_patrimonio.ClientID + "');");
                        //dvWarning.Style.Add("display", "block");
                        btnGuardar.OnClientClick = "return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        ViewState["RES"] = objresumen;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas", "Empresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnCancel.Style.Add("display", "none");
                            btnGuardar.Style.Add("display", "none");
                            pnFormulario.Enabled = false;
                            objresumen.idPermiso = Constantes.PERMISOS.SOLOLECTURA.ToString();
                            //lbEditar.Visible = false;
                        }
                    }
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    if (!EstaRegistrado(txt_rut.Text))
                    {
                        if (Ln.InsertarRelacionadasxEmpresa(objresumen.idEmpresa.ToString(), txt_nombre.Text, txt_tipo.Text, txt_rut.Text, txt_patrimonio.Text, objresumen.idUsuario, objresumen.descCargo, txt_divRut.Text, txt_nroDocProtestos.Text, txt_MontoProtestos.Text, txt_nroDocMorosidades.Text, txt_montoMorosidades.Text, txt_montoInfraccionesLab.Text, txt_montoInfraccionesPrev.Text))
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
                    else
                    {
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.CONTACTOYAREGISTRADO);
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {//ojo quitar e 1
                    if (Ln.ModificarRelacionadasxEmpresa(objresumen.idEmpresa.ToString(), HddIdEmpresaRelacionada.Value.ToString(), txt_nombre.Text.ToString(), txt_tipo.Text, txt_rut.Text, txt_patrimonio.Text, objresumen.idUsuario, objresumen.descCargo, txt_divRut.Text, txt_nroDocProtestos.Text, txt_MontoProtestos.Text.Replace(".", ""), txt_nroDocMorosidades.Text, txt_montoMorosidades.Text.Replace(".", ""), txt_montoInfraccionesLab.Text.Replace(".", ""), txt_montoInfraccionesPrev.Text.Replace(".", "")))
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
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            llenargrid();
            LimpiarCampos();
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[3].Visible = false;
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = e.Row.Cells[2].Text + "-" + e.Row.Cells[3].Text;
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = i.ToString();
                i++;
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusqueda_Command;
                e.Row.Cells[13].Text = "";
                e.Row.Cells[13].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[1].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[13].Controls.Add(lb2);
            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);

            if (e.CommandName == "Eliminar")
            {
                LogicaNegocio Ln = new LogicaNegocio();
                if (Ln.EliminarRelacionadasxEmpresa(objresumen.idEmpresa.ToString(), e.CommandArgument.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString()))
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
                LimpiarCampos();
            }

            if (e.CommandName == "Editar")
            {

                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");
                txt_nombre.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.ToString());
                txt_rut.Text = ResultadosBusqueda.Rows[index].Cells[2].Text.ToString().Replace(".", "").Split('-')[0];
                txt_tipo.Text = ResultadosBusqueda.Rows[index].Cells[5].Text.ToString();
                txt_patrimonio.Text = ResultadosBusqueda.Rows[index].Cells[6].Text.ToString();
                txt_divRut.Text = ResultadosBusqueda.Rows[index].Cells[3].Text.ToString();

                txt_nroDocProtestos.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text);
                txt_MontoProtestos.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text);
                txt_nroDocMorosidades.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text);
                txt_montoMorosidades.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text);
                txt_montoInfraccionesLab.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text);
                txt_montoInfraccionesPrev.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[12].Text);
                HddIdEmpresaRelacionada.Value = ResultadosBusqueda.Rows[index].Cells[1].Text.ToString();
                txt_nombre.Focus();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            LogicaNegocio Ln = new LogicaNegocio();
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REQUERIDODATOSEMPRESA);
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            llenargrid();
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDirectorio_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DirectorioEmpresa.aspx");
        }

        protected void lbSociosACCIONistas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("SociosAccionistas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbContactos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ContactosEmpresa.aspx");
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        protected void lbPersonas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("personas.aspx");
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresasRelacionadas");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnBuscarEmpresa_Click(object sender, EventArgs e)
        {
            DataTable res;
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.buscarDatosRut(txt_rut.Text.Replace(".", "") + txt_divRut.Text);
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    txt_nombre.Text = res.Rows[0]["RazonSocial"].ToString();
                    ViewState["IdEmpresaR"] = res.Rows[0]["IdEmpresa"].ToString();
                }
                else
                {
                    txt_nombre.Text = "";
                    dvError.Style.Add("display", "block");
                    lbError.Text = "RUT no registrado, por favor ir al modulo de Creación de Empresa antes de realizar esta operación";
                }
            }
        }

        #endregion


        #region Metodos

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");

        }

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res;

            try
            {
                res = Ln.ListarRelacionadasxEmpresa(objresumen.idEmpresa.ToString());
                ResultadosBusqueda.DataSource = res;
                ResultadosBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        private bool EstaRegistrado(string rut)
        {

            int i;
            try
            {
                for (i = 0; i < ResultadosBusqueda.Rows.Count - 1; i++)
                    if (ResultadosBusqueda.Rows[i].Cells[2].Text.ToString() == rut)
                        return true;
                return false;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return false;
            }
        }

        private void LimpiarCampos()
        {
            txt_nombre.Text = string.Empty;
            txt_rut.Text = string.Empty;
            txt_tipo.Text = string.Empty;
            txt_patrimonio.Text = string.Empty;
            txt_divRut.Text = string.Empty;

            txt_nroDocProtestos.Text = string.Empty;
            txt_MontoProtestos.Text = string.Empty;
            txt_nroDocMorosidades.Text = string.Empty;
            txt_montoMorosidades.Text = string.Empty;
            txt_montoInfraccionesLab.Text = string.Empty;
            txt_montoInfraccionesPrev.Text = string.Empty;
        }

        #endregion

    }
}

