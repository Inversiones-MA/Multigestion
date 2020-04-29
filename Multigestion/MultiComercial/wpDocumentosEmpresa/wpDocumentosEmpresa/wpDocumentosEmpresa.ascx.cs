using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Text.RegularExpressions;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpDocumentosEmpresa.wpDocumentosEmpresa
{
    [ToolboxItemAttribute(false)]
    public partial class wpDocumentosEmpresa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDocumentosEmpresa()
        {
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "Documental.aspx";
        int i, j, k;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //LogicaNegocio Ln = new LogicaNegocio();

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
                ocultarDiv();
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        ViewState["RES"] = objresumen;
                        lbEmpresa.Text = util.LimpiarTexto(objresumen.desEmpresa);
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        asignacionesJS();
                        i = j = k = 0;
                        BtnGuardarMasivo.Visible = false;
                        CargarDdls();

                        //VerificarEdicionSimultanea
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "Documentos", "DocumentosEmpresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Style.Add("display", "none");
                            btnLimpiar.Style.Add("display", "none");
                            btnCargarM.Style.Add("display", "none");
                            BtnGuardarMasivo.Style.Add("display", "none");
                            btnLimpiarM.Style.Add("display", "none");
                            btnCancelarM.Style.Add("display", "none");
                            pnFormulario.Enabled = false;
                        }
                        if (objresumen.area == "Fiscalia")
                        {
                            ddlOperaciones.Enabled = false;
                            ddlOperacionBuscar.Enabled = false;
                        }
                    }
                    else
                        Page.Response.Redirect("MensajeSession.aspx");
                }

                cargarGrillas();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            hdnTab.Value = "li2";
            ocultarDiv();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosEmpresa");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            hdnTab.Value = "li2";
            asignacionResumen(ref objresumen);
            ocultarDiv();
            LogicaNegocio Ln = new LogicaNegocio();

            try
            {
                if (fileDocumento.HasFile)
                {
                    string nombreCarpetaRaiz = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
                    bool cargaCorrecta = new Documentos { }.cargarDocumento(nombreCarpetaRaiz, ddlArea.SelectedItem.Text.Trim().ToUpper(), ddlOperaciones.SelectedItem.Text.Trim(), objresumen, fileDocumento, ddlTipoDocumento.SelectedItem.Text, ddlOperaciones.SelectedItem.Text.Trim(), txtComentario.InnerText);

                    if (cargaCorrecta)
                    {
                        ViewState["AreaText"] = ddlArea.SelectedItem.Text;
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Su archivo fue cargado con éxito";
                        limpiar();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "Su archivo no pudo ser cargado";
                    }
                }
                else
                {
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = "Debe seleccionar un archivo";
                }
            }
            catch (Exception ex)
            {
                limpiar();
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            cargarGrillas();
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosEmpresa");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
            //ActualizacionCarpetas();
        }

        protected void ResultadosBusqueda_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            //ocultarDiv();
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-download paddingIconos");
                lb2.CommandName = "Descargar";
                lb2.ToolTip = "Descargar Archivo";
                lb2.CommandArgument = i.ToString();// e.Row.Cells[1].Text;
                lb2.Command += ResultadosBusqueda_Command;
                e.Row.Cells[7].Controls.Add(lb2);

                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-close");
                lb.CommandName = "Eliminar";
                lb.ToolTip = "Eliminar Archivo";
                lb.OnClientClick = "return Dialogo();";
                lb.CommandArgument = i.ToString();
                lb.Command += ResultadosBusqueda_Command;
                e.Row.Cells[7].Controls.Add(lb);

                i++;
            }
        }

        protected void ResultadoBusquedaBuscar_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-download paddingIconos");
                lb2.CommandName = "Descargar";
                lb2.ToolTip = "Descargar Archivo";
                lb2.CommandArgument = k.ToString();// e.Row.Cells[1].Text;
                lb2.Command += ResultadoBusquedaBuscar_Command;
                e.Row.Cells[7].Controls.Add(lb2);

                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-close");
                lb.CommandName = "Eliminar";
                lb.ToolTip = "Eliminar Archivo";
                lb.OnClientClick = "return Dialogo();";
                lb.CommandArgument = k.ToString();
                lb.Command += ResultadoBusquedaBuscar_Command;
                e.Row.Cells[7].Controls.Add(lb);

                k++;
            }
        }

        private void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();

            int index = int.Parse(e.CommandArgument.ToString());
            String nombreArchivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[1].Text);
            String area = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text);
            String idOperacion = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text);
            String carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());

            String pathSinOperacion;
            String path = pathSinOperacion = carpetaEmpresa + "/" + area;
            path = string.IsNullOrWhiteSpace(idOperacion) ? path : path + "/" + idOperacion;

            SPWeb app = SPContext.Current.Web;
            String url = app.Url + "/Documentos/" + path;
            String urlSinOperacion = app.Url + "/Documentos/" + pathSinOperacion;

            SPFolder carpeta = app.GetFolder(url);
            SPFolder carpetaSinOperacion = app.GetFolder(urlSinOperacion);

            string pathArchivo = app.ServerRelativeUrl + carpeta.ToString() + "/" + nombreArchivo;
            string pathArchivoSinOperacion = app.ServerRelativeUrl + carpetaSinOperacion.ToString() + "/" + nombreArchivo;

            SPFolder sp1 = app.GetFolder(app.ServerRelativeUrl + carpeta.ToString());
            SPFolder sp2 = app.GetFolder(app.ServerRelativeUrl + carpetaSinOperacion.ToString());
            string pathFinal = String.Empty;

            foreach (SPFile fileTemp in sp1.Files)
            {
                if (fileTemp.Name == nombreArchivo)
                {
                    pathFinal = pathArchivo;
                    break;
                }
            }
            foreach (SPFile fileTemp in sp2.Files)
            {
                if (fileTemp.Name == nombreArchivo)
                {
                    pathFinal = pathArchivoSinOperacion;
                    break;
                }
            }

            if (e.CommandName == "Descargar")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + pathArchivo + "','_blank')", true);
            }
            if (e.CommandName == "Eliminar")
            {
                try
                {
                    SPFileCollection collFiles = app.GetFolder(carpeta + "/").Files;
                    foreach (SPFile archivoTemp in collFiles)
                    {
                        if (archivoTemp.Name == nombreArchivo)
                            collFiles.Delete(archivoTemp.Url);
                    }
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }
            }
            hdnTab.Value = "li1";
            cargarGridResultadoBusqueda(carpetaEmpresa, ddlArea.SelectedItem.Text);
            cargarGridResultadoBusquedaBuscar(carpetaEmpresa, ddlAreaBuscar.SelectedItem.Text);
        }

        private void ResultadoBusquedaBuscar_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();

            int index = int.Parse(e.CommandArgument.ToString());
            String nombreArchivo = System.Web.HttpUtility.HtmlDecode(ResultadoBusquedaBuscar.Rows[index].Cells[1].Text);
            String area = System.Web.HttpUtility.HtmlDecode(ResultadoBusquedaBuscar.Rows[index].Cells[2].Text);
            String idOperacion = System.Web.HttpUtility.HtmlDecode(ResultadoBusquedaBuscar.Rows[index].Cells[4].Text);

            String carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            String pathSinOperacion;
            String path = pathSinOperacion = carpetaEmpresa + "/" + area;
            path = string.IsNullOrWhiteSpace(idOperacion) ? path : path + "/" + idOperacion;

            SPWeb app = SPContext.Current.Web;
            String url = app.Url + "/Documentos/" + path;
            String urlSinOperacion = app.Url + "/Documentos/" + pathSinOperacion;
            SPFolder carpeta = app.GetFolder(url);
            SPFolder carpetaSinOperacion = app.GetFolder(urlSinOperacion);

            string pathArchivo = app.ServerRelativeUrl + carpeta.ToString() + "/" + nombreArchivo;
            string pathArchivoSinOperacion = app.ServerRelativeUrl + carpetaSinOperacion.ToString() + "/" + nombreArchivo;

            SPFolder sp1 = app.GetFolder(app.ServerRelativeUrl + carpeta.ToString());
            SPFolder sp2 = app.GetFolder(app.ServerRelativeUrl + carpetaSinOperacion.ToString());

            string pathFinal = String.Empty;

            foreach (SPFile fileTemp in sp1.Files)
            {
                if (fileTemp.Name == nombreArchivo)
                {
                    pathFinal = pathArchivo;
                    break;
                }
            }
            foreach (SPFile fileTemp in sp2.Files)
            {
                if (fileTemp.Name == nombreArchivo)
                {
                    pathFinal = pathArchivoSinOperacion;
                    break;
                }
            }

            if (e.CommandName == "Descargar" && !string.IsNullOrWhiteSpace(pathFinal))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + pathFinal + "','_blank')", true);
            }

            if (e.CommandName == "Eliminar")
            {
                try
                {
                    SPFileCollection collFiles = app.GetFolder(carpeta + "/").Files;
                    foreach (SPFile archivoTemp in collFiles)
                    {
                        if (archivoTemp.Name == nombreArchivo)
                            collFiles.Delete(archivoTemp.Url);
                    }
                    SPFileCollection collFiles2 = app.GetFolder(carpetaSinOperacion + "/").Files;
                    foreach (SPFile archivoTemp in collFiles2)
                    {
                        if (archivoTemp.Name == nombreArchivo)
                            collFiles2.Delete(archivoTemp.Url);
                    }
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }
            }
            hdnTab.Value = "li1";
            cargarGridResultadoBusqueda(carpetaEmpresa, ddlAreaBuscar.SelectedItem.Text);
            cargarGridResultadoBusquedaBuscar(carpetaEmpresa, ddlAreaBuscar.SelectedItem.Text);
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            hdnTab.Value = "li1";
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            String carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            cargarGridResultadoBusqueda(carpetaEmpresa, ddlArea.SelectedItem.Text);
        }

        protected void ResultadoBusquedaBuscar_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            hdnTab.Value = "li1";
            ResultadoBusquedaBuscar.PageIndex = e.NewPageIndex;
            String carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            cargarGridResultadoBusquedaBuscar(carpetaEmpresa, ddlAreaBuscar.SelectedItem.Text);
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmpresa");
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
        }

        protected void lbPersonas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("personas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnTab.Value = "li2";
            String area = ddlArea.SelectedItem.Text;
            ddlTipoDocumento.Items.Clear();
            CargarDdlTipoDocumentoxArea(Convert.ToInt32(ddlArea.SelectedValue), ref ddlTipoDocumento);
            //if (area != "Seleccione")
            //{
            //    ViewState["AreaText"] = ddlArea.SelectedItem.Text;
            //    llenarGridTipoDocumento(area, ddlTipoDocumento);
            //}
        }

        protected void ddlAreaBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnTab.Value = "li1";
            ddlTipoDocumentoBuscar.Items.Clear();
            CargarDdlTipoDocumentoxArea(Convert.ToInt32(ddlAreaBuscar.SelectedValue), ref ddlTipoDocumentoBuscar);
            //ddlTipoDocumentoBuscar = CargarTipoDocumentoxArea(ddlArea.SelectedItem.Text.Trim());
            //if (ddlAreaBuscar.SelectedItem.Text != "Seleccione")
            //{
            //    llenarGridTipoDocumento(area, ddlTipoDocumentoBuscar);
            //}
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            hdnTab.Value = "li1";
            LogicaNegocio Ln = new LogicaNegocio();
            ocultarDiv();
            String carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            string idOperacion = ddlOperacionBuscar.SelectedItem.Text == "Seleccione" ? String.Empty : ddlOperacionBuscar.SelectedItem.Text;
            string area = ddlAreaBuscar.SelectedItem.Text;
            DateTime fechaConsulta = dtcFechaBuscar.SelectedDate;

            DataTable dt = new DataTable();
            dt = new Documentos { }.buscarArchivos(carpetaEmpresa, area);
            DataTable dtResultado = new DataTable();
            DataTable dtResultado2 = new DataTable();

            dtResultado.Columns.Add("ID", typeof(string));
            dtResultado.Columns.Add("Nombre", typeof(string));
            dtResultado.Columns.Add("Area", typeof(string));
            dtResultado.Columns.Add("Tipo Documento", typeof(string));
            dtResultado.Columns.Add("IdOperación", typeof(string));
            dtResultado.Columns.Add("Fecha", typeof(string));
            dtResultado.Columns.Add("Comentario", typeof(string));
            dtResultado.Columns.Add("Acción", typeof(string));

            dtResultado2.Columns.Add("ID", typeof(string));
            dtResultado2.Columns.Add("Nombre", typeof(string));
            dtResultado2.Columns.Add("Area", typeof(string));
            dtResultado2.Columns.Add("Tipo Documento", typeof(string));
            dtResultado2.Columns.Add("IdOperación", typeof(string));
            dtResultado2.Columns.Add("Fecha", typeof(string));
            dtResultado2.Columns.Add("Comentario", typeof(string));
            dtResultado2.Columns.Add("Acción", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrWhiteSpace(idOperacion))
                {
                    if (row["Fecha"].ToString().Contains("-"))
                    {
                        DateTime fechaOriginal = Convert.ToDateTime(row["Fecha"].ToString().Split('-')[1] + "-" + row["Fecha"].ToString().Split('-')[0] + "-" + row["Fecha"].ToString().Split('-')[2]);
                        //Menores o iguales a la fecha de Consulta
                        if (DateTime.Compare(fechaOriginal, fechaConsulta) == -1 || DateTime.Compare(fechaOriginal, fechaConsulta) == 0)
                            dtResultado.Rows.Add(row.ItemArray);
                    }
                    else
                        dtResultado.Rows.Add(row.ItemArray);
                }
                else
                {
                    if (idOperacion == row[4].ToString())
                    {
                        if (row["Fecha"].ToString().Contains("-"))
                        {
                            DateTime fechaOriginal = Convert.ToDateTime(row["Fecha"].ToString().Split('-')[1] + "-" + row["Fecha"].ToString().Split('-')[0] + "-" + row["Fecha"].ToString().Split('-')[2]); ;
                            //Menores o iguales a la fecha de Consulta
                            if (DateTime.Compare(fechaOriginal, fechaConsulta) == -1 || DateTime.Compare(fechaOriginal, fechaConsulta) == 0)
                                dtResultado.Rows.Add(row.ItemArray);
                        }
                        else
                            dtResultado.Rows.Add(row.ItemArray);
                    }
                }
            }

            string TipoDocumeto = ddlTipoDocumentoBuscar.SelectedItem.Text == "Seleccione" ? "" : ddlTipoDocumentoBuscar.SelectedItem.Text;
            if (string.IsNullOrWhiteSpace(TipoDocumeto))
            {
                ResultadoBusquedaBuscar.DataSource = dtResultado;
                ResultadoBusquedaBuscar.DataBind();
            }
            else
            {
                foreach (DataRow row in dtResultado.Rows)
                {
                    if (row["Tipo Documento"].ToString() == TipoDocumeto)
                    {
                        dtResultado2.Rows.Add(row.ItemArray);
                    }
                }
                ResultadoBusquedaBuscar.DataSource = dtResultado2;
                ResultadoBusquedaBuscar.DataBind();
            }
        }

        protected void btnCancelarM_Click(object sender, EventArgs e)
        {
            hdnTab.Value = "li3";
            ocultarDiv();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosEmpresa");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Script", "ocultarDivPrincipal()", true);
        }

        protected void btnAtrasM_Click(object sender, EventArgs e)
        {
            hdnTab.Value = "li3";
            ocultarDiv();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosEmpresa");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnLimpiarM_Click(object sender, EventArgs e)
        {
            hdnTab.Value = "li3";
            limpiarM();
            ocultarDiv();
        }

        protected void btnCargarM_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio LN = new LogicaNegocio();
            hdnTab.Value = "li3";
            List<byte[]> listaArchivos = new List<byte[]>();
            if (fileDocumentoM.HasFiles && fileDocumentoM.PostedFiles.Count <= 10)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add("NombreAntiguo", typeof(string));
                dt.Columns.Add("Fecha", typeof(string));
                //dt.Columns.Add("Acción", typeof(string));
                int ID = 0;
                foreach (HttpPostedFile archivoTemp in fileDocumentoM.PostedFiles)
                {
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(archivoTemp.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(archivoTemp.ContentLength);
                    }
                    listaArchivos.Add(fileData);
                    //listaNombresArchivos.Add(archivoTemp.FileName);
                    dt.Rows.Add(ID, util.GetSharePointFriendlyName(archivoTemp.FileName), DateTime.Now.ToString("dd-MM-yyyy"));
                    ID++;
                }
                ViewState["ListaArchivos"] = new List<byte[]>();
                ViewState["ListaArchivos"] = listaArchivos;
                BtnGuardarMasivo.Visible = true;
                ResultadoBusquedaM.DataSource = dt;
                ResultadoBusquedaM.DataBind();
            }
            else
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = "Debe Seleccionar entre 1 y 10 documentos.";
            }
        }

        protected void ResultadoBusquedaM_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ResultadoBusquedaM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowIndex > -1)
            {
                DropDownList ddlAreaTemp = (DropDownList)e.Row.Cells[2].FindControl("ddlAreaTemp");
                CargarDdlAreas(ref ddlAreaTemp);
                //ddlAreaTemp = llenarGridAreas(ddlAreaTemp);

                DropDownList ddlTipoDocTemp = (DropDownList)e.Row.Cells[3].FindControl("ddlTipoDocTemp");
                CargarDdlTipoDocumentoxArea(Convert.ToInt32(ddlArea.SelectedValue), ref ddlTipoDocTemp);
                //ddlTipoDocTemp = llenarGridTipoDocumento(objresumen.area, ddlTipoDocTemp);
                ddlTipoDocTemp.Width = new Unit("410px");

                DropDownList ddlOperacionesTemp = (DropDownList)e.Row.Cells[4].FindControl("ddlOperacionesTemp");
                CargarDdlOperaciones(ref ddlOperacionesTemp);
                //ddlOperacionesTemp = llenarGridOperaciones(ddlOperacionesTemp);

                j++;
            }
        }

        protected void ResultadoBusquedaM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //ResultadoBusquedaM.PageIndex = e.NewPageIndex;
        }

        protected void BtnGuardarMasivo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);

            ocultarDiv();
            LogicaNegocio LN = new LogicaNegocio();
            hdnTab.Value = "li3";
            bool flagTipoDoc = true;
            string nombreCarpetaRaiz = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());

            List<byte[]> listaArchivos = new List<byte[]>();
            listaArchivos = ViewState["ListaArchivos"] as List<byte[]>;
            string idOperacion = string.Empty;
            string nombreArchivo = string.Empty;
            string comentario = string.Empty;
            string tipoDocumento = string.Empty;
            string area = string.Empty;
            string extensionArchivo = string.Empty;
            string[] splitNombreAntiguo;
            for (int i = 0; i < ResultadoBusquedaM.Rows.Count; i++)
            {
                var a = (ResultadoBusquedaM.Rows[i].Cells[3].FindControl("ddlTipoDocTemp") as DropDownList);
                var b = ResultadoBusquedaM.Rows[i].Cells[3].Text;
                tipoDocumento = (ResultadoBusquedaM.Rows[i].Cells[3].FindControl("ddlTipoDocTemp") as DropDownList).SelectedItem.Text;
                flagTipoDoc = tipoDocumento == "Seleccione" ? false : true;
                if (!flagTipoDoc)
                    break;
            }
            if (flagTipoDoc)
            {
                for (int i = 0; i < ResultadoBusquedaM.Rows.Count; i++)
                {
                    area = (ResultadoBusquedaM.Rows[i].Cells[2].FindControl("ddlAreaTemp") as DropDownList).SelectedItem.Text.ToUpper();
                    tipoDocumento = (ResultadoBusquedaM.Rows[i].Cells[3].FindControl("ddlTipoDocTemp") as DropDownList).SelectedItem.Text;
                    idOperacion = (ResultadoBusquedaM.Rows[i].Cells[4].FindControl("ddlOperacionesTemp") as DropDownList).SelectedItem.Text;
                    comentario = (ResultadoBusquedaM.Rows[i].Cells[6].FindControl("txtComentarioTemp") as TextBox).Text;
                    idOperacion = idOperacion == "Seleccione" ? "" : idOperacion;
                    splitNombreAntiguo = (ResultadoBusquedaM.Rows[i].Cells[1].Text).Split('.');
                    extensionArchivo = splitNombreAntiguo[splitNombreAntiguo.Length - 1];
                    nombreArchivo = tipoDocumento + "_" + idOperacion + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm");

                    new Documentos { }.cargaMasivaDocumento(nombreArchivo, nombreCarpetaRaiz, area, idOperacion, tipoDocumento, comentario, i.ToString(), listaArchivos[i], extensionArchivo, objresumen);
                }
                limpiarM();
                dvSuccess.Style.Add("display", "block");
                lbSuccess.Text = "Su(s) archivo(s) han sido cargados con éxito.";
                BtnGuardarMasivo.Visible = false;
            }
            else
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = "Debe Seleccionar algún tipo de documento.";
            }
            cargarGridResultadoBusqueda(nombreCarpetaRaiz, ddlArea.SelectedItem.Text);
            cargarGridResultadoBusquedaBuscar(nombreCarpetaRaiz, ddlAreaBuscar.SelectedItem.Text);
        }

        #endregion


        #region Metodos

        private void CargarDdls()
        {
            CargarDdlAreas(ref ddlArea);
            CargarDdlAreas(ref ddlAreaBuscar);
            CargarDdlTipoDocumentoxArea(Convert.ToInt32(ddlArea.SelectedValue), ref ddlTipoDocumento);
            CargarDdlTipoDocumentoxArea(Convert.ToInt32(ddlAreaBuscar.SelectedValue), ref ddlTipoDocumentoBuscar);
            CargarDdlOperaciones(ref ddlOperaciones);
            CargarDdlOperaciones(ref ddlOperacionBuscar);
        }

        private void cargarGrillas()
        {
            string carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            cargarGridResultadoBusqueda(carpetaEmpresa, ddlArea.SelectedItem.Text);
            cargarGridResultadoBusquedaBuscar(carpetaEmpresa, ddlAreaBuscar.SelectedItem.Text);
        }

        private void cargarGridResultadoBusqueda(String carpetaEmpresa, String area)
        {
            DataTable dt = new DataTable();
            dt = new Documentos { }.buscarArchivos(carpetaEmpresa, area);
            ResultadosBusqueda.DataSource = dt;
            ResultadosBusqueda.DataBind();
        }

        private void cargarGridResultadoBusquedaBuscar(String carpetaEmpresa, String area)
        {
            DataTable dt = new DataTable();
            dt = new Documentos { }.buscarArchivos(carpetaEmpresa, area);
            ResultadoBusquedaBuscar.DataSource = dt;
            ResultadoBusquedaBuscar.DataBind();
        }

        private void limpiar()
        {
            //ddlArea = llenarGridAreas(ddlArea);
            ddlTipoDocumento.Items.Clear();
            txtComentario.InnerText = String.Empty;
        }

        private void asignacionesJS()
        {
            btnGuardar.OnClientClick = "return ValidarDocumentos('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }


        private void CargarDdlOperaciones(ref DropDownList ddl)
        {
            asignacionResumen(ref objresumen);
            ddl.Items.Clear();
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = LN.ListarOperacionesxEmpresa(objresumen.idEmpresa);
            string idoperacion = dt.Rows[i][0].ToString();
            util.CargaDDL(ddl, dt, "idOperacion", "idOperacion");
        }

        private void CargarDdlAreas(ref DropDownList ddl)
        {
            ddl.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtArea = Ln.ListarAreas(null);
            util.CargaDDL1(ddl, dtArea, "Nombre", "Id");
        }

        private void CargarDdlTipoDocumentoxArea(int area, ref DropDownList ddl)
        {
            ddl.Items.Clear();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtTipoDoc = Ln.ListarTipoDocumentos(area, null);
            util.CargaDDL(ddl, dtTipoDoc, "Nombre", "IdTipoDocumento");
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
            hdnTab.Value = "li2";
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void limpiarM()
        {
            hdnTab.Value = "li3";
            DataTable dt = new DataTable();
            ResultadoBusquedaM.DataSource = dt;
            ResultadoBusquedaM.DataBind();
            ViewState["ListaArchivos"] = null;
        }

        #endregion

    }
}

