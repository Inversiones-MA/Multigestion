using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpContactosEmpresa.wpContactosEmpresa
{
    [ToolboxItemAttribute(false)]
    public partial class wpContactosEmpresa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpContactosEmpresa()
        {
        }

        int i;
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "ContactosEmpresa.aspx";

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
                    if (Page.Session["RESUMEN"] != null)
                    {
                        try
                        {
                            btnGuardar.OnClientClick = "return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                            dvWarning.Style.Add("display", "block");
                            objresumen = (Resumen)Page.Session["RESUMEN"];
                            ViewState["RES"] = objresumen;
                            Page.Session["RESUMEN"] = null;

                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            lbEjecutivo.Text = objresumen.descEjecutivo;
                            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                            ocultarDiv();

                            //Verificación Edicion Simultanea        
                            string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa", "Empresa");
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
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
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

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txt_rut1.Text = string.Empty;
            txt_apellidos.Text = string.Empty;
            txt_cargo.Text = string.Empty;
            txt_celular.Text = string.Empty;
            txt_divRut.Text = string.Empty;
            txt_email.Text = string.Empty;
            txt_fijo.Text = string.Empty;
            txt_nombre.Text = string.Empty;
            txt_rut1.Text = string.Empty;
            chkprincipal.Enabled = true;
            chkprincipal.Checked = false;
            ocultarDiv();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                string apellidopa = "", apellidoma = "";
                apellidopa = (txt_apellidos.Text.IndexOf(' ') != -1 ? txt_apellidos.Text.Substring(0, txt_apellidos.Text.IndexOf(' ')) : txt_apellidos.Text);
                apellidoma = (txt_apellidos.Text.IndexOf(' ') != -1 ? txt_apellidos.Text.Substring(txt_apellidos.Text.IndexOf(' ') + 1) : " ");
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    //if (!EstaRegistrado(txt_rut1.Text +"-"+ txt_divRut.Text))
                    //{
                    if (Ln.InsertarContactoxEmpresa(objresumen.idEmpresa.ToString(), txt_rut1.Text, txt_divRut.Text, txt_nombre.Text, apellidopa, apellidoma, txt_email.Text, txt_fijo.Text, txt_celular.Text, txt_cargo.Text, chkprincipal.Checked))
                    {
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETEMPRESARELAC);
                    }
                    else
                    {
                        ocultarDiv();
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTEMPRESARELAC);
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {//ojo quitar e 1
                    if (Ln.ModificarRelacionadasxEmpresa(objresumen.idEmpresa.ToString(), HddIdContactoEmpresa.Value.ToString(), "", "", txt_nombre.Text, apellidopa, apellidoma, txt_email.Text, txt_fijo.Text, txt_celular.Text, txt_cargo.Text, chkprincipal.Checked))
                    {
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFEMPRESARELAC);
                    }
                    else
                    {
                        ocultarDiv();
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

            //txt_rut1.Text = string.Empty;
            txt_apellidos.Text = string.Empty;
            txt_cargo.Text = string.Empty;
            txt_celular.Text = string.Empty;
            //txt_divRut.Text = string.Empty;
            txt_email.Text = string.Empty;
            txt_fijo.Text = string.Empty;
            txt_nombre.Text = string.Empty;
            txt_rut1.Text = string.Empty;
            chkprincipal.Enabled = true;
            chkprincipal.Checked = false;
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
            chkprincipal.Checked = false;
        }

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
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
                e.Row.Cells[9].Text = "";
                e.Row.Cells[9].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[9].Controls.Add(lb2);
            }
            else
            {
                e.Row.Cells[9].Text = "Acción";
            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.CommandName == "Eliminar")
            {
                LogicaNegocio Ln = new LogicaNegocio();
                bool principal = false;
                if (!EsElUltimoContacto(ref principal, Convert.ToInt32(e.CommandArgument)))
                {
                    if (Ln.EliminarContactoxEmpresa(objresumen.idEmpresa.ToString(), e.CommandArgument.ToString()))
                    {
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOELIMEMPRESARELAC);
                    }
                    else
                    {
                        ocultarDiv();
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORELIMEMPRESARELAC);
                    }

                    llenargrid();
                    txt_nombre.Text = string.Empty;
                    txt_rut1.Text = string.Empty;
                }
                else
                {
                    dvWarning.Style.Add("display", "block");
                    if (!principal)
                        lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.ULTIMOCONTACTO);
                    else
                        lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.SINPRINCIPAL);
                }
            }

            if (e.CommandName == "Editar")
            {

                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");

                //txt_rut1.Text = ResultadosBusqueda.Rows[index].Cells[1].Text.Substring(0, ResultadosBusqueda.Rows[index].Cells[1].Text.IndexOf('-'));
                //txt_divRut.Text = ResultadosBusqueda.Rows[index].Cells[1].Text.Substring(ResultadosBusqueda.Rows[index].Cells[1].Text.IndexOf('-') + 1, 1);
                txt_nombre.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString());
                txt_apellidos.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[3].Text.ToString());
                txt_email.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.ToString());
                txt_fijo.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.ToString());
                txt_celular.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString());
                txt_cargo.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString());
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.ToString()) == "SI")
                {
                    chkprincipal.Checked = true;
                    chkprincipal.Enabled = false;
                }
                else
                {
                    chkprincipal.Checked = false;
                    chkprincipal.Enabled = true;
                }
                HddIdContactoEmpresa.Value = ResultadosBusqueda.Rows[index].Cells[0].Text.ToString();
                txt_nombre.Focus();
            }

        }

        protected void lbContactos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("ContactosEmpresa.aspx");
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }


        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDirectorio_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("DirectorioEmpresa.aspx");
        }

        protected void lbDirecciones_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        protected void lbSociosAccionistas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("SociosAccionistas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ContactosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
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
            DataTable res;
            i = 0;
            try
            {
                res = Ln.ListarContactosxEmpresa(objresumen.idEmpresa.ToString());
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

        #endregion
     
    }
}