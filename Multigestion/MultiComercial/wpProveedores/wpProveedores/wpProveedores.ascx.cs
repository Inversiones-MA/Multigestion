using FrameworkIntercapIT.Utilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpProveedores.wpProveedores
{
    [ToolboxItemAttribute(false)]
    public partial class wpProveedores : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpProveedores()
        {
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        int i = 0;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        //private static string pagina = "EmpresaProveedores.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["Resumen"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            //PERMISOS USUARIOS
            //Permisos permiso = new Permisos();
            //string PermisoConfigurado = string.Empty;
            //SPWeb app2 = SPContext.Current.Web;
            //ValidarPermisos validar = new ValidarPermisos();
            //DataTable dt = new DataTable("dt");
            //validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            //validar.Pagina = pagina;
            //validar.Etapa = objresumen.area;
       
            //dt = permiso.ListarPerfil(validar);
            //if (dt.Rows.Count > 0)
            //{
            //    if (!Page.IsPostBack)
            //    {
            //        asignacionesJS();
            //        if (Page.Session["RESUMEN"] != null)
            //        {
            //            ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
            //            Page.Session["BUSQUEDA"] = null;
            //            objresumen = (Resumen)Page.Session["RESUMEN"];
            //            Page.Session["RESUMEN"] = null;
            //            ViewState["RES"] = objresumen;
            //            lbEmpresa.Text = objresumen.desEmpresa;
            //            lbRut.Text = objresumen.rut;
            //            lbEjecutivo.Text = objresumen.descEjecutivo;
      
            //            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
            //            ocultarDiv();

            //            //Verificación Edicion Simultanea        
            //            string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores", "Empresa");
            //            if (!string.IsNullOrEmpty(UsuarioFormulario))
            //            {
            //                dvWarning.Style.Add("display", "block");
            //                lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
            //                btnLimpiar.Style.Add("display", "none");
            //                btnGuardar.Style.Add("display", "none");
            //                pnFormulario.Enabled = false;
            //            }
            //        }
            //        else
            //        {
            //            Page.Response.Redirect("MensajeSession.aspx");
            //        }
            //    }
            //    else { ocultarDiv(); }
            //    llenargrid();

            //    validar.Permiso = dt.Rows[0]["Permiso"].ToString();
            //    //ViewState["validar"] = validar;
            //    Control cc = this.FindControl("dvFormulario");

            //    if (cc != null)
            //        util.bloquear(cc, dt.Rows[0]["Permiso"].ToString());
            //    else
            //    {
            //        dvFormulario.Style.Add("display", "none");
            //        dvWarning1.Style.Add("display", "block");
            //        lbWarning1.Text = "Usuario sin permisos configurados";
            //    }
            //}
            //else
            //{
            //    dvFormulario.Style.Add("display", "none");
            //    dvWarning1.Style.Add("display", "block");
            //    lbWarning1.Text = "Usuario sin permisos configurados";
            //}
        }

        void asignacionesJS()
        {
            txt_porcVentas.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txt_porcVentas.ClientID + "');");
            txt_plazo.Attributes.Add("onKeyPress", "return solonumerosPorc(event,'" + txt_plazo.ClientID + "');");
            btnGuardar.OnClientClick = "return validarEmpresaProveedores('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        void llenargrid()
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable res;
                res = Ln.ListarProveedoresxEmpresa(objresumen.idEmpresa);
                ResultadosBusqueda.DataSource = res;
                ResultadosBusqueda.DataBind();
                i = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("Mensaje.aspx");
        }

        protected void ImageButton2_Click(object sender, EventArgs e)
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
                    txt_nombre.Enabled = false;
                }
                else
                {
                    txt_nombre.Enabled = true;
                    txt_nombre.Text = "";
                    dvError.Style.Add("display", "block");
                    lbError.Text = "RUT no registrado, por favor ingrese el nombre";
                }
            }
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
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
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    if (Ln.InsertarProveedorxEmpresa(objresumen.idEmpresa, txt_nombre.Text, txt_rut.Text, txt_divRut.Text, txt_porcVentas.Text.Replace(",", "."), txt_plazo.Text, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha insertado el proveedor con éxito";
                        LimpiarCampos();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "El proveedor no se ha insertado correctamente";
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {
                    if (Ln.ModificarProveedorxEmpresa(int.Parse(hdnIdEmpresaProvedores.Value), txt_nombre.Text, txt_rut.Text, txt_divRut.Text, txt_porcVentas.Text.Replace(",", "."), txt_plazo.Text, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha modificado el proveedor correctamente";
                        LimpiarCampos();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "No se ha completado la modificación del proveedor";
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
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
        }

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            //if (objresumen.idPermiso == Constantes.PERMISOS.SOLOLECTURA.ToString())
            //{
            //    e.Row.Cells[5].Visible = false;
            //}
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[4].Text = actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 2).ToString().Replace(".", ","));
                //e.Row.Cells[2].Text = actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[2].Text), 0).ToString()).Replace(",", "");
                //e.Row.Cells[2].Text = e.Row.Cells[2].Text + "-" + e.Row.Cells[7].Text;
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
                e.Row.Cells[5].Text = "";
                e.Row.Cells[5].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[5].Controls.Add(lb2);
            }
            else { e.Row.Cells[5].Text = "Acción"; }
        }

        void LimpiarCampos()
        {
            txt_rut.Text = String.Empty;
            txt_divRut.Text = String.Empty;
            txt_nombre.Text = String.Empty;
            txt_plazo.Text = String.Empty;
            txt_porcVentas.Text = String.Empty;
            hdnIdEmpresaProvedores.Value = String.Empty;
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            if (e.CommandName == "Eliminar")
            {
                int idEmpresaProveedores = Convert.ToInt32(e.CommandArgument.ToString());
                if (Ln.EliminarProveedorxEmpresa(idEmpresaProveedores, objresumen.idUsuario))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Se ha eliminado correctamente el proveedor";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "No se ha eliminado el proveedor. Contáctese con el administrador del sitio";
                }
                llenargrid();
                //LimpiarCampos();
            }

            if (e.CommandName == "Editar")
            {
                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");
                txt_rut.Text = ResultadosBusqueda.Rows[index].Cells[1].Text.ToString().Split('-')[0];
                txt_divRut.Text = ResultadosBusqueda.Rows[index].Cells[1].Text.ToString().Split('-')[1];
                txt_nombre.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString());
                txt_plazo.Text = ResultadosBusqueda.Rows[index].Cells[4].Text.ToString();
                txt_porcVentas.Text = ResultadosBusqueda.Rows[index].Cells[3].Text.ToString();
                hdnIdEmpresaProvedores.Value = ResultadosBusqueda.Rows[index].Cells[0].Text.ToString();
                txt_nombre.Focus();
            }
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            asignacionResumen(ref objresumen);
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            llenargrid();
        }

        protected void lbProrrateo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ProrrateoGarantias.aspx");
        }

        protected void lbDeudaSBIF_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaDeudaSBIF.aspx");
        }

        protected void lbClientes_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaClientes.aspx");
        }

        protected void lbAdministracion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaAdministracion.aspx");
        }

        protected void lbProveedores_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaProveedores");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaProveedoaspx");
        }

    }
}


