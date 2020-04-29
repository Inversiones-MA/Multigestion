using FrameworkIntercapIT.Utilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using res = MultiGestion;

namespace MultiAdministracion.wpAdministracion.wpAdministracion
{
    [ToolboxItemAttribute(false)]
    public partial class wpAdministracion : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpAdministracion()
        {
        }

        res.Resumen objresumen = new res.Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        int i = 0;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            if (!Page.IsPostBack)
            {
                asignacionesJS();
                if (Page.Session["RESUMEN"] != null)
                {
                    ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                    Page.Session["BUSQUEDA"] = null;
                    objresumen = (res.Resumen)Page.Session["RESUMEN"];
                    Page.Session["RESUMEN"] = null;
                    ViewState["RES"] = objresumen;
                    lbEmpresa.Text = objresumen.desEmpresa;
                    lbRut.Text = objresumen.rut;
                    lbEjecutivo.Text = objresumen.descEjecutivo;
                    //lbOperacion.Text =  objresumen.desOperacion;
                    ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                    ocultarDiv();
                    if (int.Parse(objresumen.idPermiso.ToString()) == Constantes.PERMISOS.SOLOLECTURA)
                    {
                        dvWarning.Style.Add("display", "block");
                        btnGuardar.Style.Add("display", "none");
                        btnLimpiar.Style.Add("display", "none");
                        lbWarning.Text = LN.buscarMensaje(Constantes.MENSAJES.ACCESOLIMITADO);
                        pnFormulario.Enabled = false;
                    }
                    //Verificación Edicion Simultanea        
                    string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion", "Empresa");
                    if (!string.IsNullOrEmpty(UsuarioFormulario))
                    {
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                        btnLimpiar.Style.Add("display", "none");
                        btnGuardar.Style.Add("display", "none");
                        pnFormulario.Enabled = false;
                    }
                }
                else
                {
                    Page.Response.Redirect("MensajeSession.aspx");
                }
            }
            else { ocultarDiv(); }
            llenargrid();
        }

        void asignacionesJS()
        {
            btnGuardar.OnClientClick = "return validarEmpresaAdministracion('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
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
                LogicaNegocio MTO = new LogicaNegocio();
                DataTable res;
                res = MTO.ListarAdministracionxEmpresa(objresumen.idEmpresa);
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

        public void asignacionResumen(ref  res.Resumen objresumen)
        {
            if (ViewState["RES"] != null)
            {
                objresumen = (res.Resumen)ViewState["RES"];
            }
            else
            {
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        protected void ImageButton2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //asignacionResumen(ref objresumen);
            //try
            //{
            //    LogicaNegocio MTO = new LogicaNegocio();
            //    DataTable res;
            //    res = MTO.BuscarAdministracionxRut(objresumen.idEmpresa, txt_rut.Text);
            //    ResultadosBusqueda.DataSource = res;
            //    ResultadosBusqueda.DataBind();
            //    i = 0;
            //}
            //catch (Exception ex)
            //{
            //    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            //    Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
            //    Page.Response.Redirect("Mensaje.aspx");
            //}
            DataTable res;
            LogicaNegocio MTO = new LogicaNegocio();
            res = MTO.buscarDatosRut(txt_rut.Text.Replace(".", "") + txt_divRut.Text);
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    ocultarDiv();
                    txt_nombre.Text = res.Rows[0]["RazonSocial"].ToString();
                    ViewState["IdEmpresaR"] = res.Rows[0]["IdEmpresa"].ToString();
                    txt_nombre.Enabled = false;
                }
                else
                {
                    ocultarDiv();
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
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            ocultarDiv();
            try
            {
                LogicaNegocio MTO = new LogicaNegocio();
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    if (MTO.InsertarAdministracionxEmpresa(objresumen.idEmpresa, txt_nombre.Text, txt_rut.Text, txt_divRut.Text, txt_profesion.Text, txt_cargo.Text, ddlEdoCivil.SelectedItem.Value, ddlEdoCivil.SelectedItem.Text, dtcFecNaci.SelectedDate.ToString(), txt_antiguedad.Text, txt_telefono.Text, txt_mail.Text, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha insertado la administración con éxito";
                        LimpiarCampos();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "La administración no se ha insertado correctamente";
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {
                    if (MTO.ModificarAdministracionxEmpresa(int.Parse(hdnIdEmpresaAdministracion.Value), txt_nombre.Text, txt_rut.Text, txt_divRut.Text, txt_profesion.Text, txt_cargo.Text, ddlEdoCivil.SelectedItem.Value, ddlEdoCivil.SelectedItem.Text, dtcFecNaci.SelectedDate.ToString(), txt_antiguedad.Text, txt_telefono.Text, txt_mail.Text, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha modificado la administración correctamente";
                        LimpiarCampos();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "No se ha completado la modificación de la administración";
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

        void LimpiarCampos()
        {
            txt_rut.Text = String.Empty;
            txt_divRut.Text = String.Empty;
            txt_nombre.Text = String.Empty;
            txt_profesion.Text = String.Empty;
            txt_cargo.Text = String.Empty;
            ddlEdoCivil.SelectedValue = "0";
            dtcFecNaci.SelectedDate = new DateTime();
            txt_antiguedad.Text = String.Empty;
            txt_telefono.Text = String.Empty;
            txt_mail.Text = String.Empty;
            hdnIdEmpresaAdministracion.Value = String.Empty;
        }

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[8].Visible = false; //idEdoCivil
            if (objresumen.idPermiso == Constantes.PERMISOS.SOLOLECTURA.ToString())
            {
                e.Row.Cells[11].Visible = false; //celda accion
            }
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
                e.Row.Cells[11].Text = "";
                e.Row.Cells[11].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += ResultadosBusqueda_Command;

                e.Row.Cells[11].Controls.Add(lb2);
            }
            else { e.Row.Cells[11].Text = "Acción"; }
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            asignacionResumen(ref objresumen);
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            llenargrid();
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            ocultarDiv();
            if (e.CommandName == "Eliminar")
            {
                int idEmpresaAdministracion = Convert.ToInt32(e.CommandArgument.ToString());
                if (MTO.EliminarAdministracionxEmpresa(idEmpresaAdministracion, objresumen.idUsuario))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Se ha eliminado correctamente la administración";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "No se ha eliminado la administración. Contáctese con el administrador del sitio";
                }
                llenargrid();
                //LimpiarCampos();
            }

            if (e.CommandName == "Editar")
            {
                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusqueda.Rows[index].CssClass = ("alert alert-danger");
                txt_rut.Text = ResultadosBusqueda.Rows[index].Cells[1].Text.Split('-')[0];
                txt_divRut.Text = ResultadosBusqueda.Rows[index].Cells[1].Text.Split('-')[1];
                txt_nombre.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text);
                txt_profesion.Text = ResultadosBusqueda.Rows[index].Cells[4].Text;
                txt_cargo.Text = ResultadosBusqueda.Rows[index].Cells[5].Text;
                ddlEdoCivil.SelectedValue = ResultadosBusqueda.Rows[index].Cells[8].Text;
                dtcFecNaci.SelectedDate = Convert.ToDateTime(ResultadosBusqueda.Rows[index].Cells[3].Text.Split('-')[1] + "-" + ResultadosBusqueda.Rows[index].Cells[3].Text.Split('-')[0] + "-" + ResultadosBusqueda.Rows[index].Cells[3].Text.Split('-')[2]);
                txt_antiguedad.Text = ResultadosBusqueda.Rows[index].Cells[6].Text;
                txt_telefono.Text = ResultadosBusqueda.Rows[index].Cells[7].Text;
                txt_mail.Text = ResultadosBusqueda.Rows[index].Cells[10].Text;
                hdnIdEmpresaAdministracion.Value = ResultadosBusqueda.Rows[index].Cells[0].Text;
                txt_nombre.Focus();
            }
        }

        protected void lbProrrateo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ProrrateoGarantias.aspx");
        }

        protected void lbDeudaSBIF_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaDeudaSBIF.aspx");
        }

        protected void lbClientes_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaClientes.aspx");
        }

        protected void lbAdministracion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaAdministracion.aspx");
        }

        protected void lbProveedores_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaAdministracion");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaProveedores.aspx");
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            ocultarDiv();
        }
    }
}
