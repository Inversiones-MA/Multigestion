using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using Microsoft.SharePoint.Utilities;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiOperacion.wpDocumentoEmision.wpDocumentoEmision
{
    [ToolboxItemAttribute(false)]
    public partial class wpDocumentoEmision : WebPart
    {
        private static string pagina = "DocumentoEmision.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDocumentoEmision()
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

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            ocultarDiv();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            DataTable tabla = null;
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

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        ViewState["RES"] = objresumen;
                        lbEmpresa.Text = objresumen.desEmpresa.ToString();
                        lbOperacion.Text = objresumen.desOperacion.ToString();

                        tabla = Ln.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        ocultarDiv();

                        if (tabla.Rows.Count > 0)
                        {
                            txtAcreedor.Text = tabla.Rows[0]["DescAcreedor"].ToString();
                            txtAcreedor.Enabled = false;

                            ViewState["IdProducto"] = tabla.Rows[0]["IdProducto"].ToString();
                            txtTipoProducto.Text = tabla.Rows[0]["DescProducto"].ToString();
                            txtTipoProducto.Enabled = false;
                            ViewState["Certificado"] = tabla.Rows[0]["NCertificado"].ToString();
                            ViewState["IdAcreedor"] = tabla.Rows[0]["IdAcreedor"].ToString();
                            ViewState["Fogape"] = tabla.Rows[0]["Fogape"].ToString();
                        }

                        //Verificación Edicion Simultanea               
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmision", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            MensajeAlerta("Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.");
                            btnGuardar.Enabled = false;
                        }

                        Page.Session["RESUMEN"] = null;
                    }

                    if (objresumen.linkPrincial != "ListarSeguimiento.aspx")
                    {
                        if (tabla.Rows[0]["ValOpe"].ToString() == "4") //si es valor distinto de 4 es por que no se ha realizado la distribucion de fondos sp = GestionDistribucionFondos
                        {
                            if (tabla.Rows[0]["DescAcreedor"].ToString() != "Seleccione")
                                addDocumentos();
                            else
                            {
                                MensajeAlerta("Debe seleccionar un acreedor en el formulario de Operacciones");
                                gridDocumentosCurse.Visible = false;
                            }
                        }
                        else
                        {
                            MensajeAlerta("Debe completar los datos en el formulario de Operación para visualizar los documentos");
                            gridDocumentosCurse.Visible = false;
                        }
                    }
                    else
                    {
                        if (tabla.Rows[0]["DescAcreedor"].ToString() != "Seleccione")
                            addDocumentos();
                    }
                }
                else
                {
                    asignacionResumen(ref objresumen);
                    tabla = Ln.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                }

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

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        private void AdjuntarDoc()
        {

        }

        private void addDocumentos()
        {
            CargarTipoDocumento();
        }

        private void CargarTipoDocumento()
        {
            DataTable dt = new DataTable("dt");
            LogicaNegocio Ln = new LogicaNegocio();
            dt = Ln.ConsultarDocumentosParametrizados(Convert.ToInt32(ViewState["IdProducto"]), Convert.ToInt32(ViewState["IdAcreedor"]), "Operaciones");

            if (dt.Rows.Count >= 0)
            {
                int i = 100;
                DataRow row;

                row = dt.NewRow();
                row["IdPlantillaDocumento"] = i;
                row["NombrePlantilla"] = "Solicitud de Emision";
                dt.Rows.Add(row);

                row = dt.NewRow();
                row["IdPlantillaDocumento"] = i + 1;
                row["NombrePlantilla"] = "Instruccion Curse";
                dt.Rows.Add(row);
            }

            gridDocumentosCurse.DataSource = dt;
            gridDocumentosCurse.DataBind();
            if (dt.Rows.Count > 0)
                ocultarDiv();
            else
                MensajeAlerta("No hay documentos asociados a: " + txtAcreedor.Text.Trim() + " ");
        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdjuntar_Click1(object sender, EventArgs e)
        {
            try
            {
                //LogicaNegocio LN = new LogicaNegocio();
                asignacionResumen(ref objresumen);
                String carpetaEmpresa = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(lbEmpresa.Text));
                carpetaEmpresa = carpetaEmpresa.Length > 100 ? carpetaEmpresa.Substring(0, 100) : carpetaEmpresa;
                carpetaEmpresa = lbRut.Text.Split('-')[0] + "_" + carpetaEmpresa;
                String carpetaArea = objresumen.area.ToUpper();
                String idOperacion = objresumen.idOperacion.ToString();
                cargarDocumento(carpetaEmpresa, carpetaArea, idOperacion);
                Page.Session["Resumen"] = objresumen;
                Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                Page.Response.Redirect("DocumentoEmision.aspx");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        protected void lbDistribucionFondo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmision");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DistribucionFondos.aspx");
        }

        protected void lblDocumentoCurse_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmision");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DocumentoEmision.aspx");
        }

        protected void gridDocumentosCurse_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();

            //se validara que la empresa tenga seleccionado un ejecutivo al momento de avanzar la operacion de Ingresado a Cursado
            string ejecutivoEmision = Ln.ValidarEjecutivoEmision(objresumen.idEmpresa);

            if (ejecutivoEmision != "")
            {
                if (Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), "8", "Cierre", "24", "Por Facturar", "5", "Cursado", "Contabilidad", "Por Facturar"))
                {
                    Ln.ActualizarEstado(objresumen.idOperacion.ToString(), "8", "Cierre", "24", "Por Facturar", "5", "Cursado", "Contabilidad");
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);

                }
                Page.Response.Redirect("ListarOperaciones.aspx");
            }
            else
                MensajeAlerta("La empresa no tiene un SubGerente Comercial asignado");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentoEmision");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void gridDocumentosCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gridDocumentosCurse.SelectedRow;
                int codDoc = Convert.ToInt32(gridDocumentosCurse.DataKeys[row.RowIndex].Values["IdPlantillaDocumento"]);
            }
            catch
            {

            }
        }

        protected void gridDocumentosCurse_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.CommandName == "Eliminar")
            {
                string nombre = new Documentos { }.ObtenerNombreArchivo(e.CommandArgument.ToString(), ViewState["Certificado"].ToString(), objresumen);
                if (new Documentos { }.EliminarArchivo(nombre, ViewState["Certificado"].ToString(), objresumen))
                {
                    //DataTable tabla = Ln.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                    addDocumentos();
                }
            }
            if (e.CommandName == "Descargar")
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");
                Dictionary<string, string> datos = new Dictionary<string, string>();
                datos["IdEmpresa"] = objresumen.idEmpresa.ToString();
                datos["IdOperacion"] = objresumen.idOperacion.ToString();

                //string indicador = util.BuscarValorIndicador(DateTime.Now.ToString());

                DataTable dtValorMoneda = new DataTable("dtValorMoneda");
                dtValorMoneda = Ln.GestionValorMoneda(DateTime.Now, 0, null, null, null, 1);

                datos["ValorUF"] = dtValorMoneda.Rows[0]["MontoUF"].ToString();
                datos["ValorUS"] = dtValorMoneda.Rows[0]["MontoUSD"].ToString();

                string[] arg = new string[2];
                arg = e.CommandArgument.ToString().Split(';');
                string DescPlantilla = arg[1];
                string idPlantilla = arg[0];

                if (DescPlantilla == "Solicitud de Emision")
                {
                    byte[] archivo = new Reportes { }.GenerarReporte("Solicitud_de_Emision", datos, "admin", objresumen);
                    if (archivo != null)
                    {
                        Page.Session["tipoDoc"] = "pdf";
                        Page.Session["binaryData"] = archivo;
                        Page.Session["Titulo"] = "Solicitud_de_Emision";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
                    }

                }
                else if (DescPlantilla == "Instruccion Curse")
                {
                    byte[] archivo = new Reportes { }.GenerarReporte("Instruccion_de_Curse", datos, "admin", objresumen);
                    if (archivo != null)
                    {
                        Page.Session["tipoDoc"] = "pdf";
                        Page.Session["binaryData"] = archivo;
                        Page.Session["Titulo"] = "Instruccion_de_Curse";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
                    }
                }
                else
                {
                    dt = Ln.ListarPlantillaById(Convert.ToInt32(idPlantilla));

                    if (dt.Rows.Count > 0)
                    {
                        ocultarDiv();
                        DataSet dtLista = new DataSet();
                        dtLista = Ln.CargarDatosHtml(Convert.ToInt32(objresumen.idOperacion.ToString()));

                        string codigoHtml = util.ReemplazarValores(dt.Rows[0]["ContenidoHtml"].ToString(), dtLista, "", "", "");
                        string NombrePlantilla = dt.Rows[0]["NombrePlantilla"].ToString();
                        if (!string.IsNullOrEmpty(codigoHtml))
                        {
                            //generar(NombrePlantilla, codigoHtml);
                            string contenido = HttpUtility.HtmlDecode(codigoHtml);
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append("<table width='1000px'><tr><td align='center'><table width='900px'><tr><td>" + contenido + "</td></tr></table></td></tr></table>");
                            byte[] archivo = util.ConvertirAPDF_Control(sb);
                            Page.Session["Titulo"] = NombrePlantilla;

                            if (archivo != null)
                            {
                                Page.Session["tipoDoc"] = dt.Rows[0]["TipoDocumento"].ToString();
                                Page.Session["binaryData"] = archivo;
                                Page.Session["Titulo"] = NombrePlantilla.Trim();
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
                            }
                        }
                    }

                    else
                        MensajeAlerta("Error al buscar el documento");
                }
            }
            if (e.CommandName == "Adjuntar")
            {
                DivArchivo.Visible = true;
                documentos.Visible = false;
                botones.Visible = false;
                tipo = e.CommandArgument.ToString();

                AdjuntarDoc();
            }
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

        private void MensajeAlerta(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje.Trim();
        }

        #endregion
     
        static string tipo = "";
        //public static SPListItem carpetaCliente;

        private bool cargarDocumento(String carpetaEmpresa, String carpetaArea, String idOperacion)
        {
            try
            {
                LogicaNegocio LN = new LogicaNegocio();
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                string urlApp = app.Url;
                SPUser usuario = SPContext.Current.Site.SystemAccount;

                if (fileDocumento.HasFile)
                {
                    String nombreArchivo = util.GetSharePointFriendlyName(fileDocumento.FileName);
                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa).Exists)
                    {
                        CrearCarpetaEnRaiz(carpetaEmpresa);
                    }
                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea).Exists)
                    {
                        CrearCarpetaArea(carpetaEmpresa, carpetaArea);
                    }
                    SPFolder path;
                    if (idOperacion != "Seleccione")
                    {
                        if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion).Exists)
                        {
                            String pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                            CrearCarpetaArea(pathEmpresaArea, idOperacion);
                        }
                        path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion);
                    }
                    else
                    {
                        String pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                        path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea);
                    }
                    SPFile cargaArchivo = path.Files.Add(path + "/" + nombreArchivo, fileDocumento.FileBytes, usuario, usuario, DateTime.Now, DateTime.Now);
                    cargaArchivo.Item.Update();
                    String nombreArchivoNuevo = tipo;
                    nombreArchivoNuevo = nombreArchivoNuevo + "_" + idOperacion;
                    nombreArchivoNuevo = util.RemoverSignosAcentos(nombreArchivoNuevo) + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm");
                    cargaArchivo.Item["Name"] = nombreArchivoNuevo;
                    cargaArchivo.Item["IdEmpresa"] = objresumen.idEmpresa;
                    cargaArchivo.Item["Area"] = carpetaArea;
                    cargaArchivo.Item["FechaInicio"] = DateTime.Now;
                    cargaArchivo.Item["TipoDocumento"] = tipo;
                    //cargaArchivo.Item["Comentario"] = "";
                    cargaArchivo.Item["Habilitado"] = true;
                    cargaArchivo.Item["IdOperacion"] = idOperacion;
                    cargaArchivo.Item.Update();

                    //file.Item["FechaInicio"] = txtFechaInicio.SelectedDate;
                    //file.Item["FechaFin"] = txtFechaFin.SelectedDate;
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        private bool CrearCarpetaEnRaiz(String nombreCarpeta)
        {
            try
            {
                //SPFolderCollection spfc = new SPFolderCollection();
                SPWeb app = SPContext.Current.Web;
                SPList docList = app.Lists["Documentos"];
                SPFolder newFolder = docList.RootFolder.SubFolders.Add(nombreCarpeta);
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        private void CrearCarpetaArea(String carpetaRaiz, String carpetaArea)
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList listData = app.Lists["Documentos"];
            var urlLibrary = app.Url + "/Documentos/" + carpetaRaiz;

            SPListItem carpetaCliente;

            if (!app.GetFolder(urlLibrary + "/" + carpetaArea).Exists)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    carpetaCliente = listData.Folders.Add(urlLibrary, SPFileSystemObjectType.Folder, carpetaArea);
                    carpetaCliente.Update();
                    listData.Update();
                });
            }
            else
            {
                var idLibrary = app.GetFolder(urlLibrary + "/" + carpetaArea).UniqueId;
                carpetaCliente = listData.Folders[idLibrary];
            }
        }

    }
}
