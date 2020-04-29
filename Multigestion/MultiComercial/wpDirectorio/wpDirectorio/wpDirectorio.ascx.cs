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
using MultigestionUtilidades;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpDirectorio.wpDirectorio
{
    [ToolboxItemAttribute(false)]
    public partial class wpDirectorio : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDirectorio()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int i;
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "Directorio.aspx";

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
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        ViewState["RES"] = objresumen;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;

                        //Verificación Edicion Simultanea    
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio", "Empresa");
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {

                    if (Ln.InsertarDirectorioxEmpresa(objresumen.idEmpresa.ToString(), txt_nombre.Text, txt_cargo.Text, txt_profesion.Text, txt_universidad.Text, txt_rut.Text, txt_divRut.Text, txt_antiguedad.Text, objresumen.idUsuario, objresumen.descCargo))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETEMPRESARELAC);
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTEMPRESARELAC) + "  -  " + ViewState["ACCION"].ToString();
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {

                    if (Ln.ModificarDirectorioxEmpresa(objresumen.idEmpresa.ToString(), HddIdDirectorio.Value.ToString(), txt_nombre.Text.ToString(), txt_cargo.Text.ToString(), txt_profesion.Text.ToString(), txt_universidad.Text, txt_rut.Text, txt_divRut.Text, txt_antiguedad.Text, objresumen.idUsuario, objresumen.descCargo))
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
            txt_nombre.Text = string.Empty;
            txt_rut.Text = string.Empty;
            txt_divRut.Text = string.Empty;
            txt_cargo.Text = string.Empty;
            txt_profesion.Text = string.Empty;
            txt_universidad.Text = string.Empty;
            txt_antiguedad.Text = string.Empty;
            HddIdDirectorio.Value = string.Empty;
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[9].Visible = false;
            e.Row.Cells[10].Visible = false;
            //if (objresumen.idPermiso == Constantes.PERMISOS.SOLOLECTURA.ToString())
            //{
            //    e.Row.Cells[8].Visible = false;
            //}
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[2].Text), 0).ToString()).Replace(",", "");
                e.Row.Cells[2].Text = e.Row.Cells[2].Text + "-" + e.Row.Cells[10].Text;
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
                e.Row.Cells[8].Text = "";
                e.Row.Cells[8].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[1].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[8].Controls.Add(lb2);
            }
            else
            {
                e.Row.Cells[8].Text = "Acción";

            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.CommandName == "Eliminar")
            {
                LogicaNegocio Ln = new LogicaNegocio();
                if (Ln.EliminarDirectorioxEmpresa(objresumen.idEmpresa.ToString(), e.CommandArgument.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString()))
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
                txt_nombre.Text = string.Empty;
                txt_rut.Text = string.Empty;
                txt_divRut.Text = string.Empty;
                txt_cargo.Text = string.Empty;
                txt_profesion.Text = string.Empty;
                txt_universidad.Text = string.Empty;
                txt_antiguedad.Text = string.Empty;
                HddIdDirectorio.Value = string.Empty;
            }

            if (e.CommandName == "Editar")
            {
                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");
                txt_nombre.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[3].Text.ToString());
                txt_rut.Text = ResultadosBusqueda.Rows[index].Cells[2].Text.ToString().Replace(".", "").Split('-')[0];
                txt_divRut.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.ToString());
                txt_cargo.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.ToString());
                txt_profesion.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.ToString());
                txt_universidad.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString());
                txt_antiguedad.Text = ResultadosBusqueda.Rows[index].Cells[7].Text.ToString();
                HddIdDirectorio.Value = ResultadosBusqueda.Rows[index].Cells[1].Text.ToString();
                txt_nombre.Focus();

            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Page.Response.Redirect(objresumen.linkPrincial);
            txt_nombre.Text = string.Empty;
            txt_rut.Text = string.Empty;
            txt_divRut.Text = string.Empty;
            txt_cargo.Text = string.Empty;
            txt_profesion.Text = string.Empty;
            txt_universidad.Text = string.Empty;
            txt_antiguedad.Text = string.Empty;
            HddIdDirectorio.Value = string.Empty;
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
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDirectorio_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DirectorioEmpresa.aspx");
        }

        protected void lbSociosACCIONistas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("SociosAccionistas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbContactos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ContactosEmpresa.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
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
                  txt_nombre.Text = "";
            }
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Directorio");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("DireccionEmpresa.aspx");
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

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            i = 0;
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res;
            try
            {
                res = Ln.ListarDirectorioxEmpresa(objresumen.idEmpresa.ToString());
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

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        #endregion
 
    }
}