using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using FrameworkIntercapIT.Utilities;
using System.Web.UI;
using System.Web;
using System.Globalization;
using System.Xml;
using MultigestionUtilidades;

namespace MultiFiscalia.wpAprobacionSolicitud.wpAprobacionSolicitud
{
    [ToolboxItemAttribute(false)]
    public partial class wpAprobacionSolicitud : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpAprobacionSolicitud()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        public bool val = false;      
        private static string pagina = "AprobacionFiscalia.aspx";

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            ////PERMISOS USUARIOS
            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = objresumen.area;

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;

                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        ocultarDiv();

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "AprobacionSolicitud", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            pnFormulario.Enabled = false;
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                    try
                    {
                        mostrarDatos();
                    }

                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                    }
                }
                inicializacionGrillas();


                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
                //Control divFiltros = this.FindControl("filtros");
                //Control divGrilla = this.FindControl("grilla");

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    MensajePermisos("Usuario sin permisos configurados");
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                MensajePermisos("Usuario sin permisos configurados");
            }
        }

        protected void gridGarantias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[3].Text), 2).ToString().Replace(".", ","));
                e.Row.Cells[4].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 2).ToString().Replace(".", ","));
            }
        }

        protected void tbServiciosOperaciones1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
        }

        protected void tbServiciosOperaciones1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                //   if (((DataTable)ViewState["ServiciosEmpresa"]).Rows.Count > 1)
                {
                    LinkButton lb2 = new LinkButton();
                    lb2.CssClass = ("fa fa-close");
                    lb2.CommandName = "Eliminar";
                    lb2.CommandArgument = e.Row.Cells[0].Text;
                    if (val)
                        lb2.Enabled = false;
                    //lb2.OnClientClick = "return Dialogo();";kvsa
                    lb2.Command += Operaciones_Command;
                    e.Row.Cells[2].Controls.Add(lb2);
                }
            }
        }

        private void Operaciones_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["ServiciosEmpresa"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    if (dt1.Rows[i][0].ToString() == e.CommandArgument.ToString())
                    {
                        FiscaliaCostos(dt1.Rows[i]["Nombre"].ToString(), dt1.Rows[i]["ID"].ToString(), false, false);
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["ServiciosEmpresa"] = dt1;
                tbServiciosOperaciones1.DataSource = (DataTable)ViewState["ServiciosEmpresa"];
                tbServiciosOperaciones1.DataBind();
                dt1 = null;
            }
        }

        protected void tbServiciosGarantia1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        protected void tbServiciosGarantia1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                //if (((DataTable)ViewState["ServiciosGarantia"]).Rows.Count > 1)
                {
                    LinkButton lb = new LinkButton();
                    lb.CssClass = ("fa fa-close");
                    lb.CommandName = "Eliminar";
                    lb.CommandArgument = e.Row.Cells[0].Text + "|" + e.Row.Cells[2].Text;
                    // lb.OnClientClick = "return Dialogo();";
                    if (val)
                        lb.Enabled = false;
                    lb.Command += Garantia_Command;
                    e.Row.Cells[3].Controls.Add(lb);
                }
            }
        }

        protected void Garantia_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["ServiciosGarantia"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    if (dt1.Rows[i][0].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[0] && dt1.Rows[i][2].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[1])
                    {
                        FiscaliaCostos(dt1.Rows[i]["Nombre"].ToString(), dt1.Rows[i]["ID"].ToString(), false, false);
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["ServiciosGarantia"] = dt1;
                tbServiciosGarantia1.DataSource = (DataTable)ViewState["ServiciosGarantia"]; ;
                tbServiciosGarantia1.DataBind();
                dt1 = null;
            }
        }

        protected void gridFiscalia_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);

            if (objresumen.descCargo != "Administrador")
                e.Row.Cells[10].Visible = false;

            e.Row.Cells[0].Visible = false;
            e.Row.Cells[9].Visible = false;
        }

        protected void gridFiscalia_DataBound(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)ViewState["ServicioFiscalia"];
            double totalServicios = 0;
            for (int i = 0; i <= gridFiscalia.Rows.Count - 1; i++)
            {
                (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i][3].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i][3].ToString()) : "0";
                (gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).Text = (double.Parse(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Text)) * double.Parse(System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[i].Cells[2].Text))).ToString();

                totalServicios = totalServicios + (double.Parse(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).Text)));

                (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Attributes.Add("onFocus", "return vaciarCampo('" + (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).ClientID +
                    "','" + (gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).ClientID + "','" + (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).ClientID + "','" + "1" + "');");

                (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Attributes.Add("onblur", "return SumatoriaVacido('" + (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).ClientID +
                    "','" + (gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).ClientID + "','" + (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).ClientID + "','" + "1" + "','" + (System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[i].Cells[2].Text)) + "');");

                if (System.Web.HttpUtility.HtmlDecode(dt1.Rows[i][6].ToString()) != "")
                    (gridFiscalia.Rows[i].FindControl("ckCritico") as CheckBox).Checked = bool.Parse(System.Web.HttpUtility.HtmlDecode(dt1.Rows[i][6].ToString()));
                else
                    (gridFiscalia.Rows[i].FindControl("ckCritico") as CheckBox).Checked = false;

                if (val)
                {
                    (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Enabled = false;
                    (gridFiscalia.Rows[i].FindControl("ckCritico") as CheckBox).Enabled = false;
                }
            }
            (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).Text = totalServicios.ToString();

            if (val)
            {
                (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).Enabled = false;
            }

            dt1 = null;
        }

        protected void btnAddServicioGarantia_Click(object sender, EventArgs e)
        {
            garantiaServicio();
        }

        /// <summary>
        /// empresaServicio se encarga de agregar, modificar o eliminar los registrso correspondientes a la grilla servicios Operaciones, y realiza  llamado a FiscaliaCostos en donde se hace lo mismo pero para la grilla de fiscalia
        /// </summary>
        private void empresaServicio()
        {
            asignacionResumen(ref objresumen);
            if (ddlServicioEmpresa.SelectedValue.ToString() != "0") // si lo seleccionado en el combo de servicios es igual a "0"(Seleccione) entonces no haga NADA
            {
                if (ViewState["ServiciosEmpresa"] == null) // si no ec}xiste el ViewState["ServiciosEmpresa"] lo creo.
                {
                    DataTable dt1 = new DataTable();
                    dt1.Clear();
                    dt1.Columns.Add("ID");
                    dt1.Columns.Add("Nombre");
                    dt1.Columns.Add("Acción");
                    DataRow dr = dt1.NewRow();
                    dr["ID"] = ddlServicioEmpresa.SelectedValue.ToString();
                    dr["Nombre"] = ddlServicioEmpresa.SelectedItem.ToString();
                    dr["Acción"] = "";
                    dt1.Rows.Add(dr);
                    tbServiciosOperaciones1.DataSource = dt1;
                    tbServiciosOperaciones1.DataBind();
                    ViewState["ServiciosEmpresa"] = dt1;

                    FiscaliaCostos(ddlServicioEmpresa.SelectedItem.ToString(), ddlServicioEmpresa.SelectedValue.ToString(), true, true);
                    dt1 = null;
                }
                else // de lo contratio edito la grilla segun sea necesario.
                {
                    Boolean ban = true;
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["ServiciosEmpresa"];
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][0].ToString() == ddlServicioEmpresa.SelectedValue.ToString())
                        {
                            ban = false;
                            break;
                        }
                    }
                    if (ban == true)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["ID"] = ddlServicioEmpresa.SelectedValue.ToString();
                        dr["Nombre"] = ddlServicioEmpresa.SelectedItem.ToString();
                        dr["Acción"] = "";
                        dt1.Rows.Add(dr);
                        tbServiciosOperaciones1.DataSource = dt1;
                        tbServiciosOperaciones1.DataBind();
                        ViewState["ServiciosEmpresa"] = dt1;
                        FiscaliaCostos(ddlServicioEmpresa.SelectedItem.ToString(), ddlServicioEmpresa.SelectedValue.ToString(), ban, true); //agrega, suma
                    }
                    dt1 = null;
                }
            }
        }

        protected void btnAddServiciosEmpresa_Click(object sender, EventArgs e)
        {
            empresaServicio();
        }


        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "AprobacionSolicitud");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void gridFiscalia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                {
                    LinkButton lb2 = new LinkButton();
                    lb2.CssClass = ("fa fa-close");
                    lb2.CommandName = "Eliminar";
                    lb2.CommandArgument = e.Row.Cells[0].Text + "|" + e.Row.Cells[9].Text;
                    lb2.OnClientClick = "return Dialogo();";
                    lb2.Command += Fiscalia_Command;
                    e.Row.Cells[10].Controls.Add(lb2);
                }
            }
        }

        protected void Fiscalia_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                LogicaNegocio Ln = new LogicaNegocio();
                asignacionResumen(ref objresumen);
                string Llave = System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[0];
                string IdGarantia = System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[1];
                bool borrar = false;
                try
                {
                    borrar = Ln.BajarServicioOperacion(objresumen.idEmpresa, objresumen.idOperacion, int.Parse(Llave), int.Parse(IdGarantia), objresumen.idUsuario);
                }
                catch
                {

                }
            }
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            string validacionServicio = String.Empty;
            String xmlEmpres = generarXMLServiciosOperacion();
            String xmlGarantia = generarXMLServiciosGarantia();
            DataTable dtArchivos = new DataTable("dtArchivos");
            DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
            try
            {
                validacionServicio = "3";  //valor 3 es que los servicios estan aprobados, y los documentos criticos fueron subidos al sistema
                //validar que si existen servicios criticos chekeados, deben existir documentos asociados a el servicio para avanzar de confeccion contratos (area fiscalia) hacia Aprobacion Fiscalia(area comercial)
                dtArchivosCriticos = Ln.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 1);
                if (dtArchivosCriticos.Rows.Count > 0)
                {
                    int val = 0;
                    //consultar si existe el tipo de documento en la lista de documentos de sharepoint
                    dtArchivos = util.buscarArchivos(lbEmpresa.Text.Trim(), lbRut.Text.Trim(), objresumen.desEtapa.Trim(), objresumen.idOperacion.ToString());
                    val = util.ValidarDocCriticos(dtArchivosCriticos, dtArchivos);

                    if (val == dtArchivosCriticos.Rows.Count)
                    {
                        Ln.InsertarSolicitudFiscalia(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), xmlEmpres, xmlGarantia, txtComentarios.Text.ToString(), txtComentariosFiscalia.Text.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), validacionServicio); //actualiza e ingresa servicios operaciones y garantias
                        if (Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, "6", "Fiscalia", "19", "Aprobación Fiscalía", "1", "Ingresado", "Comercial", txtComentarios.Text.ToString()))
                        {
                            Ln.ActualizarEstado(objresumen.idOperacion.ToString(), "6", "Fiscalia", "19", "Aprobación Fiscalía", "1", "Ingresado", "Comercial");  //ACTUALIZA ESTADO LISTA OPERACIONES
                            MensajeOk("La validación de documentos criticos esta correcta, la operación se encuentra en el area comercial");
                        }
                        else
                            MensajeError(Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                    }
                    else
                    {
                        MensajeError("Necesita adjuntar todos los documentos asociados a los servicios criticos, total servicios criticos = " + dtArchivosCriticos.Rows.Count + " ");
                    }
                }
                else
                {
                    Ln.InsertarSolicitudFiscalia(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), xmlEmpres, xmlGarantia, txtComentarios.Text.ToString(), txtComentariosFiscalia.Text.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), validacionServicio);
                    if (Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, "6", "Fiscalia", "19", "Aprobación Fiscalía", "1", "Ingresado", "Comercial", txtComentarios.Text.ToString()))
                    {
                        Ln.ActualizarEstado(objresumen.idOperacion.ToString(), "6", "Fiscalia", "19", "Aprobación Fiscalía", "1", "Ingresado", "Comercial");  //ACTUALIZA ESTADO LISTA OPERACIONES
                        MensajeOk("La validación de documentos criticos esta correcta, la operación se encuentra en el area comercial");
                    }
                    else
                        MensajeError(Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
                String Comentario = string.Empty;
                String xmlEmpres = generarXMLServiciosOperacion();
                String XMLComentario = string.Empty;
                String xmlGarantia = generarXMLServiciosGarantia();
                string validacionServicio = "2";  //significa que los servicios fueron aprobados, asi se habilita la opcion para subir los documentos en fiscalia.

                LogicaNegocio MTO = new LogicaNegocio();
                Boolean exito = true;
                dtArchivosCriticos = MTO.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 1);
                if ((objresumen.desEtapa.ToLower() == "fiscalia") && tbServiciosGarantia1.Rows.Count > 0 && tbServiciosOperaciones1.Rows.Count > 0)
                {
                    if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                    {
                        exito = MTO.InsertarSolicitudFiscalia(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), xmlEmpres, xmlGarantia, txtComentarios.Text.ToString(), txtComentariosFiscalia.Text.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), validacionServicio);
                        if (exito)
                        {
                            ocultarDiv();
                            MensajeOk(MTO.buscarMensaje(Constantes.MENSAJE.EXITOINSERT));

                            if (MTO.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, "6", "Fiscalia", "18", "Confección Contratos", "1", "Ingresado", "Fiscalia", txtComentarios.Text.ToString()))
                            {
                                MTO.ActualizarEstado(objresumen.idOperacion.ToString(), "6", "Fiscalia", "18", "Confección Contratos", "1", "Ingresado", "Fiscalia"); //ACTUALIZA ESTADO LISTA OPERACIONES
                                MensajeOk("Se realizaron los cambios de forma Exitosa, la operación se encuentra en la etapa Confección Contratos");
                                if (ViewState["validacionEmpresa"].ToString() != "4" && ViewState["validacionGarantia"].ToString() != "4" && dtArchivosCriticos.Rows.Count == 0)
                                    MensajeAlerta("al no existir documentos criticos, solo se debe finalizar las etapas en Servicios Legales Empresa y Servicios Legales Garantia");
                            }
                            else
                                MensajeError(MTO.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                        }
                        else
                        {
                            ocultarDiv();
                            MensajeError(MTO.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                        }

                        ValidarDocCriticos(int.Parse(ViewState["validacionEmpresa"].ToString()), int.Parse(ViewState["validacionGarantia"].ToString()));
                    }
                }
                else
                {
                    ocultarDiv();
                    MensajeError(MTO.buscarMensaje(Constantes.MENSAJE.FISCINCOMPLETOS));
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void btnFinLegal_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
                LogicaNegocio Ln = new LogicaNegocio();
                //Boolean exito = true;
                dtArchivosCriticos = Ln.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 1);

                if (Ln.InsertarActualizarEstadosFiscalia(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, "20", "Cierre Legal", objresumen.idPAF.ToString()))
                    Page.Response.Redirect(objresumen.linkPrincial);
                else
                    MensajeError("no se puede finalizar la operación");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                MensajeError(ex.Message);
            }
        }

        //protected void gridFiscalia_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    LogicaNegocio Ln = new LogicaNegocio();
        //    asignacionResumen(ref objresumen);
        //    string Llave = Convert.ToString(gridFiscalia.DataKeys[e.RowIndex].Values["IdServicio"]).Trim();
        //    int IdGarantia = Convert.ToInt32(gridFiscalia.DataKeys[e.RowIndex].Values["IdGarantia"]);
        //    bool borrar = false;
        //    try
        //    {
        //        borrar = Ln.BajarServicioOperacion(objresumen.idEmpresa, objresumen.idOperacion, int.Parse(Llave), IdGarantia, objresumen.idUsuario);

        //    }
        //    catch
        //    {

        //    }
        //}

        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        protected void mostrarDatos()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            ViewState["validacionEmpresa"] = 0;
            ViewState["validacionGarantia"] = 0;

            res = MTO.ListarResumenTotal(objresumen.idEmpresa.ToString(), objresumen.idPAF.ToString(), objresumen.idOperacion.ToString());//agregarle el idGarantia
            lblRut.Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(res.Tables[0].Rows[0]["Rut"].ToString()), 0).ToString()).Replace(",", "") + '-' + res.Tables[0].Rows[0]["DivRut"].ToString();
            lblNombre.Text = res.Tables[0].Rows[0]["RazonSocial"].ToString();
            lbTipo.Text = res.Tables[0].Rows[0]["DescActividad"].ToString();
            lbEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();
            lbOperacion.Text = objresumen.idOperacion.ToString();
            if (res.Tables[0].Rows[0]["MontoAprobado"].ToString() != "")
                lbMonto.Text = "UF " + float.Parse(res.Tables[0].Rows[0]["MontoAprobado"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));
            lbPAF.Text = res.Tables[0].Rows[0]["NroPAF"].ToString();

            lbOperacion.Text = objresumen.idOperacion.ToString();
            lbComentarioComite.Text = res.Tables[0].Rows[0]["ObservacionComite"].ToString();
            txtComentarios.Text = res.Tables[0].Rows[0]["ObservacionServicios"].ToString();
            txtComentariosFiscalia.Text = res.Tables[0].Rows[0]["ObservacionFiscalia"].ToString();
            gridGarantias.DataSource = res.Tables[2];
            //  ViewState["Operaciones"] = res.Tables[1];
            garantiasExistentes(res.Tables[2]);
            gridGarantias.DataBind();
            servicios();
            garantias();

            if (!string.IsNullOrEmpty(res.Tables[3].Rows[0]["valEmpresa"].ToString()))
                ViewState["validacionEmpresa"] = int.Parse(res.Tables[3].Rows[0]["valEmpresa"].ToString());
            if (!string.IsNullOrEmpty(res.Tables[3].Rows[0]["valGarantia"].ToString()))
                ViewState["validacionGarantia"] = int.Parse(res.Tables[3].Rows[0]["valGarantia"].ToString());
            //if (!string.IsNullOrEmpty(res.Tables[3].Rows[0]["CGR"].ToString()))
            //    ViewState["validacionCGR"] = int.Parse(res.Tables[3].Rows[0]["CGR"].ToString());

            res = MTO.ListarServicios(objresumen.idEmpresa.ToString(), objresumen.idPAF.ToString(), objresumen.idOperacion.ToString());//agregarle el idPAF

            if (res.Tables.Count > 1)
            {
                ViewState["ServiciosEmpresa"] = res.Tables[1];
                ViewState["ServiciosGarantia"] = res.Tables[0];
                ViewState["ServicioFiscalia"] = res.Tables[2];
    
                ValidarDocCriticos(int.Parse(ViewState["validacionEmpresa"].ToString()), int.Parse(ViewState["validacionGarantia"].ToString()));
            }
        }

        protected void inicializacionGrillas()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                if (ViewState["ServiciosEmpresa"] != null)
                {
                    tbServiciosOperaciones1.DataSource = (DataTable)ViewState["ServiciosEmpresa"];
                    tbServiciosOperaciones1.DataBind();
                }
                if (ViewState["ServiciosGarantia"] != null)
                {
                    tbServiciosGarantia1.DataSource = (DataTable)ViewState["ServiciosGarantia"];
                    tbServiciosGarantia1.DataBind();
                }
                if (ViewState["ServicioFiscalia"] != null)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["ServicioFiscalia"];

                    for (int i = 0; i <= gridFiscalia.Rows.Count - 1; i++)
                    {
                        dt1.Rows[i]["Valor"] = System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Text);
                        dt1.Rows[i]["Critico"] = System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("ckCritico") as CheckBox).Checked.ToString());
                    }
                    ViewState["ServicioFiscalia"] = dt1;

                    gridFiscalia.DataSource = (DataTable)ViewState["ServicioFiscalia"];
                    gridFiscalia.DataBind();

                    dt1 = null;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// ESTADO FLUJO FISCALIA
        /// //Aprobación Servicios	17
        /// //Confección Contratos	18
        /// //Aprobación Fiscalía	19
        /// //Cierre Legal	20
        /// </summary>
        /// <param name="valEmpresa"></param>
        /// <param name="valGarantia"></param>
        private void ValidarDocCriticos(int valEmpresa, int valGarantia)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
            DataTable dtTotalArchivos = new DataTable("dtTotalArchivos");
            DataTable dtArchivosEnShare = new DataTable("dtArchivos");
            int val = 0;
            //validar si existen documentos criticos, si existen no se puede mostrar boton aprobar hasta subir los documentos asociados
            dtArchivosCriticos = Ln.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 1);
            dtTotalArchivos = Ln.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 4);
            dtArchivosEnShare = util.buscarArchivos(lbEmpresa.Text.Trim(), lbRut.Text.Trim(), "FISCALIA", objresumen.idOperacion.ToString());
            val = util.ValidarDocCriticos(dtArchivosCriticos, dtArchivosEnShare);

            if (objresumen.IdEstadoFiscalia >= 19)
            {
                Button1.Visible = false;
                Button2.Visible = false;
                btnGuardar.Visible = false;
                if (valEmpresa == 1 || valGarantia == 1)
                {
                    btnAprobar.Visible = false;
                    btnFinLegal.Visible = false;
                    if (valEmpresa == 1)
                        MensajeAlerta("debe finalizar la etapa en formulario Servicios Empresa");
                    if (valGarantia == 1)
                        MensajeAlerta("debe finalizar la etapa en formulario Servicios Garantía");
                }
            }

            if (objresumen.IdEstadoFiscalia == 17 || objresumen.IdEstadoFiscalia == 18)
            {
                btnAprobar.Visible = false;
                btnFinLegal.Visible = false;

                if (objresumen.IdEstadoFiscalia == 17)
                    MensajeAlerta("Al presionar el boton guardar la operacion pasara a la etapa Confeccion de contratos");
                if (objresumen.IdEstadoFiscalia == 18)
                    MensajeAlerta("Al presionar el boton guardar la operacion pasara a la etapa Aprobacion fiscalia");
            }
         
            
            if (objresumen.IdEstadoFiscalia >= 19 && valEmpresa == 4 && valGarantia == 4)
            {
                if (objresumen.desEtapa.ToLower() == "cierre")
                {
                    btnAprobar.Visible = false;
                }
                else
                {
                    btnAprobar.Visible = true;
                    MensajeAlerta("Si presiona el boton Aprobar la operacion pasara al area Comercial");
                }
            }
            else if (objresumen.IdEstadoFiscalia >= 19 && valEmpresa > 1 && valEmpresa < 4)
            {
                btnAprobar.Visible = false;
                btnFinLegal.Visible = false;
                btnGuardar.Visible = false;
                 MensajeAlerta("debe finalizar la etapa en formulario Servicios Empresa");
            }
            else if (objresumen.IdEstadoFiscalia >= 19 && valGarantia > 1 && valGarantia < 4)
            {
                btnAprobar.Visible = false;
                btnFinLegal.Visible = false;
                btnGuardar.Visible = false;
                MensajeAlerta("debe finalizar la etapa en formulario Servicios Garantía");
            }
            else
                btnAprobar.Visible = false;

            //validar si docs subidos en lista de sharepoint, son igual al total de archivos solicitados, aunque no sean criticos
            if (dtArchivosEnShare.Rows.Count >= dtTotalArchivos.Rows.Count && objresumen.IdEstadoFiscalia >= 19)
            {
                btnFinLegal.Visible = true;
                MensajeAlerta("al presionar el boton finalizar area legal la operacion saldra del flujo de fiscalia");    
            }
            else if (dtTotalArchivos.Rows[0]["DescServicio"].ToString().ToLower().Contains("no requiere") && objresumen.IdEstadoFiscalia >= 19 && valEmpresa == 4 && valGarantia == 4)
            {
                btnFinLegal.Visible = true;
                MensajeAlerta("al presionar el boton finalizar area legal la operacion saldra del flujo de fiscalia");
            }
            else
            {
                btnFinLegal.Visible = false;
                if(objresumen.IdEstadoFiscalia >= 19 && valEmpresa == 4 && valGarantia == 4)
                   MensajeAlerta("debe subir la totalidad de documentos para finalizar el flujo de fiscalia");
            }

            //if (dtTotalArchivos.Rows.Count >= dtArchivosEnShare.Rows.Count)
            //{
            //    if (dtTotalArchivos.Rows[0]["DescServicio"].ToString().ToLower().Contains("no requiere") && objresumen.IdEstadoFiscalia >= 19 && valEmpresa == 4 && valGarantia == 4)
            //    {
            //        btnFinLegal.Visible = true;
            //        MensajeAlerta("al presionar el boton finalizar area legal la operacion saldra del flujo de fiscalia");
            //    }
            //    else
            //        btnFinLegal.Visible = false;
            //}

            //if (objresumen.desSubEtapa.ToLower() == "aprobación fiscalía" || objresumen.desEtapa.ToLower() == "cierre")
            //    gridFiscalia.Enabled = false;

            if (objresumen.desEtapa.ToLower() == "cierre")
            {
                gridFiscalia.Enabled = false;
                if (objresumen.IdEstadoFiscalia == 19 && objresumen.desEtapa.ToLower() == "fiscalia" && objresumen.area.ToLower() == "fiscalia")
                    btnAprobar.Visible = false;
                else
                    btnAprobar.Visible = false;

                if(objresumen.IdEstadoFiscalia == 17 || objresumen.IdEstadoFiscalia == 18)
                    btnGuardar.Visible = true;
                else
                    btnGuardar.Visible = false;

            }

        }

        private void garantiasExistentes(DataTable dtServiciosGarantia)
        {
            DataTable dtServiciosEmpresa = new DataTable();
            ListItem LstItem;
            ddlGarantias.DataSource = dtServiciosGarantia;
            ddlGarantias.DataTextField = "Nro. Garantía";
            ddlGarantias.DataValueField = "Nro. Garantía";
            ddlGarantias.DataBind();
            LstItem = new ListItem("Seleccione", "0");
            ddlGarantias.Items.Insert(0, LstItem);
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        private void servicios()
        {
            DataTable dt = new DataTable();
            DataTable dtServiciosOperacion = new DataTable();
            dt = ServiciosOperacion();
            ListItem LstItem;
            ddlServicioEmpresa.DataSource = dt;
            ddlServicioEmpresa.DataTextField = "Nombre";
            ddlServicioEmpresa.DataValueField = "ID";
            ddlServicioEmpresa.DataBind();
            LstItem = new ListItem("Seleccione", "0");
            ddlServicioEmpresa.Items.Insert(0, LstItem);
        }

        protected DataTable ServiciosOperacion()
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList items = app.Lists["ServiciosOperacion"];
                SPQuery query = new SPQuery();
                query.Query = "<Where><Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq></Where>" +
                              "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                SPListItemCollection collListItems = items.GetItems(query);
                return collListItems.GetDataTable();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        private void garantias()
        {
            DataTable dtServiciosEmpresa = new DataTable();
            dtServiciosEmpresa = ServiciosGarantia("1");
            ListItem LstItem;
            ddlServiciosGarantia.DataSource = dtServiciosEmpresa;
            ddlServiciosGarantia.DataTextField = "Nombre";
            ddlServiciosGarantia.DataValueField = "ID";

            ddlServiciosGarantia.DataBind();
            LstItem = new ListItem("Seleccione", "0");
            ddlServiciosGarantia.Items.Insert(0, LstItem);
        }

        protected DataTable ServiciosGarantia(string Tipo)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList items = app.Lists["ServiciosGarantia"];
                SPQuery query = new SPQuery();
                query.Query = "<Where><And><Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Tipo'/><Value Type='TEXT'>" + Tipo + "</Value></Eq></And></Where>" +
                              "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                SPListItemCollection collListItems = items.GetItems(query);
                return collListItems.GetDataTable();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        /// <summary>
        /// FiscaliaCostos
        /// </summary>
        /// <param name="servicio"> nombre del servicio</param>
        /// <param name="idServicios"> id del servicio</param>
        /// <param name="agrega"> agregar nuevos parametros a la grilla de fiscalia</param>
        /// <param name="suma"> indica cuando modificar(suma - resta al campo Cantidad) o eliminar parametros del grid de fiscalia, </param>
        private void FiscaliaCostos(string servicio, string idServicios, Boolean agrega, Boolean suma)
        {
            asignacionResumen(ref objresumen);
            DataTable dt1 = new DataTable();
            //si no existe el viewstate ServicioFiscalia, crea la tabla y agrega valor
            if (ViewState["ServicioFiscalia"] == null)
            {
                dt1.Clear();
                dt1.Columns.Add("IdServicio");
                dt1.Columns.Add("TipoServicio");
                dt1.Columns.Add("Cantidad");
                dt1.Columns.Add("Valor");
                dt1.Columns.Add("Total");
                dt1.Columns.Add("Validacion");
                dt1.Columns.Add("Critico");
                DataRow dr = dt1.NewRow();
                dr["IdServicio"] = idServicios;
                dr["TipoServicio"] = servicio;
                dr["Cantidad"] = 1;
                dr["Valor"] = 0;
                dr["Total"] = 0;
                dr["Validacion"] = 0;
                dr["Critico"] = 0;

                dt1.Rows.Add(dr);
            }
            else // de lo contrario modifica la tabla existente.
            {
                dt1 = (DataTable)ViewState["ServicioFiscalia"];

                // si agrega, siempre es uno nuevo por lo tanto la cantidad es 1
                if (agrega)
                {
                    DataRow dr = dt1.NewRow();
                    dr["IdServicio"] = idServicios;
                    dr["TipoServicio"] = servicio;
                    dr["Cantidad"] = 1;
                    dr["Valor"] = 0;
                    dr["Total"] = 0;
                    dr["Validacion"] = 0;
                    dr["Critico"] = 0;
                    dt1.Rows.Add(dr);

                }
                // si no est agregando pero si sumando, a cantidad se suma 1
                if (!agrega && suma)
                {
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][1].ToString() == servicio && dt1.Rows[i][0].ToString() == idServicios)
                        {
                            dt1.Rows[i]["Cantidad"] = int.Parse(dt1.Rows[i]["Cantidad"].ToString()) + 1;
                        }
                    }
                }
                //si no esta agregando y tampoco sumando a la cantidad le resto 1 y si e}hay solo uno, elimino el que está.
                if (!agrega && !suma)
                {
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][1].ToString() == servicio && dt1.Rows[i][0].ToString() == idServicios)
                        {
                            if (dt1.Rows[i]["Cantidad"].ToString() == "1")
                            {
                                dt1.Rows[i].Delete();
                            }
                            else
                            {
                                dt1.Rows[i]["Cantidad"] = int.Parse(dt1.Rows[i]["Cantidad"].ToString()) - 1;
                            }
                        }
                    }
                    dt1.AcceptChanges();
                }
            }
            ViewState["ServicioFiscalia"] = dt1;
            gridFiscalia.DataSource = dt1;
            gridFiscalia.DataBind();
            dt1 = null;

            //odos los combos vuelven a su valor predeterminado
            ddlGarantias.SelectedIndex = 0;
            ddlServicioEmpresa.SelectedIndex = 0;
            ddlServiciosGarantia.SelectedIndex = 0;
        }

        /// <summary>
        /// garantiaServicio se encarga de agregar, modificar o eliminar los registrso correspondientes a la grilla servicios Garantia, y realiza  llamado a FiscaliaCostos en donde se hace lo mismo pero para la grilla de fiscalia
        /// </summary>
        private void garantiaServicio()
        {
            asignacionResumen(ref objresumen);
            if (ddlServiciosGarantia.SelectedValue.ToString() != "0" && ddlGarantias.SelectedValue.ToString() != "0") // si lo seleccionado en el combo de servicios es igual a "0"(Seleccione) entonces no haga NADA
            {
                if (ViewState["ServiciosGarantia"] == null)
                {
                    DataTable dt1 = new DataTable();
                    dt1.Clear();
                    dt1.Columns.Add("ID");
                    dt1.Columns.Add("Nombre");
                    dt1.Columns.Add("Garantía");
                    dt1.Columns.Add("Acción");

                    DataRow dr = dt1.NewRow();
                    dr["ID"] = ddlServiciosGarantia.SelectedValue.ToString();
                    dr["Nombre"] = ddlServiciosGarantia.SelectedItem.ToString();
                    dr["Garantía"] = ddlGarantias.SelectedValue.ToString();
                    dr["Acción"] = "";
                    dt1.Rows.Add(dr);

                    tbServiciosGarantia1.DataSource = dt1;
                    tbServiciosGarantia1.DataBind();
                    ViewState["ServiciosGarantia"] = dt1;

                    FiscaliaCostos(ddlServiciosGarantia.SelectedItem.ToString(), ddlServiciosGarantia.SelectedValue.ToString(), true, true);
                    dt1 = null;
                }
                else
                {
                    Boolean ban = true;
                    Boolean banF = true;
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["ServiciosGarantia"];
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][0].ToString() == ddlServiciosGarantia.SelectedValue.ToString() && !(dt1.Rows[i][2].ToString() == ddlGarantias.SelectedValue.ToString()))
                        {
                            banF = false;
                        }
                        if (dt1.Rows[i][0].ToString() == ddlServiciosGarantia.SelectedValue.ToString() && dt1.Rows[i][2].ToString() == ddlGarantias.SelectedValue.ToString())
                        {
                            ban = false;
                        }
                    }
                    if (ban == true)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["ID"] = ddlServiciosGarantia.SelectedValue.ToString();
                        dr["Nombre"] = ddlServiciosGarantia.SelectedItem.ToString();
                        dr["Garantía"] = ddlGarantias.SelectedValue.ToString();
                        dr["Acción"] = "";
                        dt1.Rows.Add(dr);
                        tbServiciosGarantia1.DataSource = dt1;
                        tbServiciosGarantia1.DataBind();
                        ViewState["ServiciosGarantia"] = dt1;

                        FiscaliaCostos(ddlServiciosGarantia.SelectedItem.ToString(), ddlServiciosGarantia.SelectedValue.ToString(), banF, true);
                    }
                    dt1 = null;
                }
            }
        }


        private string generarXMLServiciosOperacion()
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

            for (int i = 0; i <= tbServiciosOperaciones1.Rows.Count - 1; i++)
            {
                XmlNode RespNodeCkb;
                RespNodeCkb = doc.CreateElement("Ckb");

                XmlNode nodo = doc.CreateElement("ID");
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[0].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("Descripcion");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo1);
                for (int j = 0; j <= gridFiscalia.Rows.Count - 1; j++)
                {
                    if (System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[j].Cells[1].Text) == (System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[1].Text)))
                    {
                        XmlNode nodoC = doc.CreateElement("Costo");
                        nodoC.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[j].FindControl("txtValor") as TextBox).Text)));
                        RespNodeCkb.AppendChild(nodoC);

                        XmlNode nodo3 = doc.CreateElement("Critico");
                        nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[j].FindControl("ckCritico") as CheckBox).Checked.ToString())));
                        RespNodeCkb.AppendChild(nodo3);

                        break;
                    }
                }
                RespNode.AppendChild(RespNodeCkb);
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        private string generarXMLServiciosGarantia()
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

            for (int i = 0; i <= tbServiciosGarantia1.Rows.Count - 1; i++)
            {
                XmlNode RespNodeCkb;
                RespNodeCkb = doc.CreateElement("Ckb");

                XmlNode nodo = doc.CreateElement("ID");
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[0].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("Descripcion");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo1);

                XmlNode nodoG = doc.CreateElement("IdGarantia");
                nodoG.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[2].Text)));
                RespNodeCkb.AppendChild(nodoG);

                for (int j = 0; j <= gridFiscalia.Rows.Count - 1; j++)
                {
                    if (System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[j].Cells[1].Text) == (System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[1].Text)))
                    {
                        XmlNode nodoC = doc.CreateElement("Costo");
                        nodoC.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[j].FindControl("txtValor") as TextBox).Text)));
                        RespNodeCkb.AppendChild(nodoC);

                        XmlNode nodo3 = doc.CreateElement("Critico");
                        //  System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("ckCritico") as CheckBox).Checked.ToString());
                        nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[j].FindControl("ckCritico") as CheckBox).Checked.ToString())));
                        RespNodeCkb.AppendChild(nodo3);

                        break;
                    }
                }

                RespNode.AppendChild(RespNodeCkb);
            }
            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        private void MensajeAlerta(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje.Trim();
        }

        private void MensajePermisos(string mensaje)
        {
            dvWarning1.Style.Add("display", "block");
            lbWarning1.Text = mensaje.Trim();
        }

        private void MensajeOk(string mensaje)
        {
            dvSuccess.Style.Add("display", "block");
            lbSuccess.Text = mensaje.Trim();
        }

        private void MensajeError(string mensaje)
        {
            dvError.Style.Add("display", "block");
            lbError.Text = mensaje.Trim();
        }

        #endregion
      
    }
}
