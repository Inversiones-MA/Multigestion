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
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiOperacion.wpDocumentosCurse.wpDocumentosCurse
{
    [ToolboxItemAttribute(false)]
    public partial class wpDocumentosCurse : WebPart
    {
        private static string pagina = "DocumentoCurse.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDocumentosCurse()
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

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            DataTable tabla = null;
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
                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        ViewState["RES"] = objresumen;
                        lbEmpresa.Text = objresumen.desEmpresa.ToString();
                        lbOperacion.Text = objresumen.desOperacion.ToString();
                
                        tabla = MTO.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                        Page.Session["RESUMEN"] = null;
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
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosCurse", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Enabled = false;
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                }
                else
                {
                    asignacionResumen(ref objresumen);
                    tabla = MTO.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                }
        
                try
                {
                    if (tabla.Rows[0]["ValOpe"].ToString() == "4") //están completos los datos Operacion
                        addDocumentos();
                    else
                    {
                        ocultarDiv();
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "Debe completar los datos en el formulario de Operación para visualizar los documentos";
                        gridDocumentosCurse.Visible = false;
                    }

                // ValDisFon
                    if (txtTipoProducto.Text != "Certificado Fianza Técnica" && tabla.Rows[0]["ValOpe"].ToString() == "4")
                    {
                        if (tabla.Rows[0]["ValDisFon"].ToString() == "4") //están completos los datos Operacion
                            addDocumentos();
                        else
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Debe completar los datos en el formulario de Distribucion de Fondos para visualizar los documentos";
                            gridDocumentosCurse.Visible = false;
                        }   

                    }
                        
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
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

        private void addDocumentos()
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            if (ViewState["DocumentosCurse"] == null) // si no ec}xiste el ViewState["ServiciosEmpresa"] lo creo.
            {
                DataTable dt1 = new DataTable();
                dt1.Clear();

                dt1.Columns.Add("IdDocumento");
                dt1.Columns.Add("Documento");
                dt1.Columns.Add("Habilitado");
                dt1.Columns.Add("Accion");
                DataRow dr = dt1.NewRow();

                dr["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Solicitud de Emisión"));
                dr["Documento"] = "Solicitud de Emisión";
                dr["Habilitado"] = "1";
                dr["Accion"] = "";
                dt1.Rows.Add(dr);

                DataRow dr1 = dt1.NewRow();
                dr1["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Elegibilidad"));
                dr1["Documento"] = "Certificado de Elegibilidad";
                dr1["Habilitado"] = "1";
                dr1["Accion"] = "";
                dt1.Rows.Add(dr1);
                if (ViewState["Fogape"].ToString() == "True")
                {
                    dr1 = dt1.NewRow();
                    dr1["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Solicitud de Pago de Garantia"));
                    dr1["Documento"] = "Solicitud de Pago de Garantia";
                    dr1["Habilitado"] = "1";
                    dr1["Accion"] = "";
                    dt1.Rows.Add(dr1);
                }
                DataRow dr2 = dt1.NewRow();

                if (txtTipoProducto.Text == "Certificado Fianza Comercial")
                {
                    if (txtAcreedor.Text.Contains("Itaú") || txtAcreedor.Text.Contains("Itau"))
                    {
                        dr2["Documento"] = "Carta de Garantía ITAU";
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantía ITAU"));
                    }
                    else if (txtAcreedor.Text.Contains("Santander"))
                    {
                        dr2["Documento"] = "Carta de Garantía Santander";
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantía Santander"));
                    }
                    else if (txtAcreedor.Text.Contains("BBVA"))
                    {
                        dr2["Documento"] = "Carta de Garantía BBVA";

                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantia BBVA"));
                    }
                    else if (txtAcreedor.Text == "Banco Del Estado De Chile")
                    {
                        dr2["Documento"] = "Carta de Garantía Banco Estado";
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantía Banco Estado"));
                    }
                    else if (txtAcreedor.Text == "Banco Security")
                    {
                        dr2["Documento"] = "Carta de Garantía Banco Security";
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantía Banco Security"));
                    }
                    else if (txtAcreedor.Text.Contains("Factoring"))
                    {
                        dr2["Documento"] = "Carta de Garantía Factoring";
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantía Factoring"));
                    }
                    else
                    {
                        dr2["Documento"] = "Carta de Garantía";
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Carta de Garantía"));
                    }
                    dr2["Habilitado"] = "0";
                    dr2["Accion"] = "";
                    dt1.Rows.Add(dr2);
                    if (txtAcreedor.Text == "Banco Security")
                    {
                        DataRow dr3 = dt1.NewRow();
                        dr3["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Contrato de Subfianza"));
                        dr3["Documento"] = "Contrato de Subfianza";
                        dr3["Habilitado"] = "0";
                        dr3["Accion"] = "";
                        dt1.Rows.Add(dr3);
                    }
                    //else if (lbAcreedor.Text == "Banco Del Estado De Chile")
                    //{
                    //    DataRow dr3 = dt1.NewRow();
                    //    dr3["IdDocumento"] = RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Contrato de Subfianza Banco Estado"));
                    //    dr3["Documento"] = "Contrato de Subfianza Banco Estado";
                    //    dr3["Habilitado"] = "0";
                    //    dr3["Accion"] = "";
                    //    dt1.Rows.Add(dr3);
                    //}


                    DataRow dr4 = dt1.NewRow();
                    dr4["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Instruccion de Curse"));
                    dr4["Documento"] = "Instruccion de Curse";
                    dr4["Habilitado"] = "0";
                    dr4["Accion"] = "";
                    dt1.Rows.Add(dr4);

                    DataRow dr5 = dt1.NewRow();
                    if (txtAcreedor.Text.Contains("Itaú") || txtAcreedor.Text.Contains("Itau"))
                    {
                        dr5["Documento"] = "Certificado de Fianza Comercial Itaú";
                        dr5["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza Itau"));
                    }
                    else if (txtAcreedor.Text.Contains("BBVA"))
                    {
                        dr5["Documento"] = "Certificado de Fianza Comercial BBVA";
                        dr5["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza BBVA"));
                    }
                    else if (txtAcreedor.Text == "Banco Del Estado De Chile")
                    {
                        dr5["Documento"] = "Certificado de Fianza Comercial Banco Estado";
                        dr5["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza Banco Estado"));
                    }
                    else if (txtAcreedor.Text == "Banco Security")
                    {
                        dr5["Documento"] = "Certificado de Fianza Comercial Banco Security";
                        dr5["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza Banco Security"));
                    }
                    else if (txtAcreedor.Text.Contains("Factoring"))
                    {
                        dr5["Documento"] = "Certificado de Fianza Comercial Factoring";
                        dr5["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza Factoring"));
                    }
                    else
                    {
                        dr5["Documento"] = "Certificado de Fianza Comercial";
                        dr5["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza Comercial"));
                    }
                    dr5["Habilitado"] = "0";
                    dr5["Accion"] = "";
                    dt1.Rows.Add(dr5);
                }
                else if (txtTipoProducto.Text == "Certificado Fianza Técnica")
                {

                    {
                        dr2["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Certificado de Fianza Técnica"));
                        dr2["Documento"] = "Certificado de Fianza Técnica";

                    } dr2["Habilitado"] = "0";
                    dr2["Accion"] = "";
                    dt1.Rows.Add(dr2);

                }
                DataRow dr6 = dt1.NewRow();
                dr6["IdDocumento"] = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode("Revision Pagare"));
                dr6["Documento"] = "Revision Pagare";
                dr6["Habilitado"] = "1";
                dr6["Accion"] = "";
                dt1.Rows.Add(dr6);

                gridDocumentosCurse.DataSource = dt1;
                gridDocumentosCurse.DataBind();
                ViewState["DocumentosCurse"] = dt1;
                dt1 = null;
            }
            else // de lo contratio edito la grilla segun sea necesario.
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DocumentosCurse"];
                DataRow dr = dt1.NewRow();

                //dt1.Rows.Add(dr);
                gridDocumentosCurse.DataSource = dt1;
                gridDocumentosCurse.DataBind();
                ViewState["DocumentosCurse"] = dt1;
                dt1 = null;
            }
        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridDocumentosCurse_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowIndex > -1)
            {
                //LogicaNegocio Ln = new LogicaNegocio();
                bool existe = new Documentos { }.ExisteArchivo(e.Row.Cells[0].Text, ViewState["Certificado"].ToString(), objresumen);

                LinkButton lb = new LinkButton();
                //aqui verficar si existe para bloquear descarga.....

                if (!existe)
                {
                    lb.CssClass = ("glyphicon glyphicon-save paddingIconos");
                }
                else
                {
                    lb.CssClass = ("glyphicon glyphicon-save paddingIconos text-muted");
                    //lb.Enabled = false;
                }
                lb.CommandName = "Descargar"; //Eliminar adjunto
                lb.CommandArgument = e.Row.Cells[0].Text;
                lb.Command += Operaciones_Command;
                e.Row.Cells[3].Controls.Add(lb);

                LinkButton lb1 = new LinkButton();
                lb1.CssClass = ("glyphicon glyphicon-open paddingIconos");
                lb1.CommandName = "Adjuntar"; //Eliminar adjunto
                lb1.CommandArgument = e.Row.Cells[0].Text;
                lb1.OnClientClick = "return Dialogo();";
                lb1.Command += Operaciones_Command;
                if (!existe)
                {
                    lb1.CssClass = ("glyphicon glyphicon-open paddingIconos");
                    //lb2.Enabled = false;
                }
                else
                {
                    lb1.CssClass = ("glyphicon glyphicon-open paddingIconos text-muted");
                }
                //lb1.OnClientClick = "return Adjuntar('documentos','" + e.Row.Cells[1].Text + "')";
                e.Row.Cells[3].Controls.Add(lb1);

                //LinkButton lb2 = new LinkButton();
                //lb2.CommandName = "Eliminar"; //Eliminar adjunto
                //lb2.CommandArgument = e.Row.Cells[0].Text;
                ////lb2.OnClientClick = "return Dialogo();";kvsa
                //lb2.Command += Operaciones_Command;
                //e.Row.Cells[3].Controls.Add(lb2);
            }
        }

        protected void gridDocumentosCurse_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[2].Visible = false;
        }
        static string tipo = "";

       void Operaciones_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.CommandName == "Eliminar")
            {
                ///LogicaNegocio Ln = new LogicaNegocio();
                string nombre = new Documentos { }.ObtenerNombreArchivo(e.CommandArgument.ToString(), ViewState["Certificado"].ToString(), objresumen);
                if (new Documentos { }.EliminarArchivo(nombre, ViewState["Certificado"].ToString(), objresumen))
                {
                    //DataTable tabla = MTO.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                    addDocumentos();
                }
            }
            if (e.CommandName == "Descargar")
            {
                //si no lo ha generado (no tiene documento) lo genero y descargo, si ya está, descargar el que esté registrado en la carpeta
                LogicaNegocio Ln = new LogicaNegocio();
                Dictionary<string, string> datos = new Dictionary<string, string>();
                datos["IdEmpresa"] = objresumen.idEmpresa.ToString();
                datos["IdOperacion"] = objresumen.idOperacion.ToString();

                DataTable dtValorMoneda = new DataTable("dtValorMoneda");
                dtValorMoneda = Ln.GestionValorMoneda(DateTime.Now, 0, null, null, null, 1);

                datos["ValorUF"] = dtValorMoneda.Rows[0]["MontoUSD"].ToString();
                datos["ValorUS"] = dtValorMoneda.Rows[0]["MontoUF"].ToString();

                string Reporte = "";
                Reporte = e.CommandArgument.ToString();

                byte[] archivo = new Reportes { }.GenerarReporte(Reporte, datos, ViewState["IdAcreedor"].ToString(), objresumen);
                if (archivo != null)
                {
                    Page.Session["tipoDoc"] = "pdf";
                    Page.Session["binaryData"] = archivo;
                    Page.Session["Titulo"] = Reporte;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
                }
                //addDocumentos();
            }
            if (e.CommandName == "Adjuntar")
            {
                DivArchivo.Visible = true;
                documentos.Visible = false;
                botones.Visible = false;
                tipo = e.CommandArgument.ToString();

                addDocumentos();
            }
        }

        protected void CrearCarpeta(string Lista, string Ruta, string newFolder, ref SPListItem carpetaCliente)
        {
            asignacionResumen(ref objresumen);
            SPWeb app;
            app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            var listData = app.Lists[Lista];
            var urlLibrary = app.Url + Ruta;

            // var nomLibrary = RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(objresumen.desEmpresa.ToString()));
            /*Validación y Creación de Carpeta con el nombre de la empresa*/
            if (!app.GetFolder(urlLibrary + "/" + newFolder).Exists)
            {
                carpetaCliente = listData.Folders.Add(urlLibrary, SPFileSystemObjectType.Folder, newFolder);
                carpetaCliente.Update();
                listData.Update();
            }
            else
            {
                var idLibrary = app.GetFolder(urlLibrary + "/" + newFolder).UniqueId;
                carpetaCliente = listData.Folders[idLibrary];
            }

        }

        public static SPListItem carpetaCliente;

        private bool cargarDocumento(String carpetaEmpresa, String carpetaArea, String idOperacion)
        {
            try
            {
                asignacionResumen(ref objresumen);
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

        protected void btnAdjuntar_Click1(object sender, EventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                String carpetaEmpresa = util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(lbEmpresa.Text));
                carpetaEmpresa = carpetaEmpresa.Length > 100 ? carpetaEmpresa.Substring(0, 100) : carpetaEmpresa;
                carpetaEmpresa = lbRut.Text.Split('-')[0] + "_" + carpetaEmpresa;
                String carpetaArea = objresumen.area.ToUpper();
                String idOperacion = objresumen.idOperacion.ToString();
                cargarDocumento(carpetaEmpresa, carpetaArea, idOperacion);
                Page.Session["Resumen"] = objresumen;
                Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                Page.Response.Redirect("DocumentoCurse.aspx");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        protected void lbDistribucionFondo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosCurse");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DistribucionFondos.aspx");
        }

        protected void lblDocumentoCurse_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosCurse");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DocumentoCurse.aspx");
        }

        public string buscarMensaje(int idmensaje)
        {
            string mensaje = string.Empty;
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Mensajes"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + idmensaje + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    mensaje = (oListItem["Mensaje"].ToString());
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                mensaje = ex.Source.ToString() + ex.Message.ToString();

            }
            return mensaje.Replace("<p>", "");
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);

            LogicaNegocio MTO = new LogicaNegocio();

            if (MTO.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                            objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), "8", "Cierre", "24", "Por Facturar", "5", "Cursado", "Contabilidad", "Por Facturar"))
            {
                MTO.ActualizarEstado(objresumen.idOperacion.ToString(), "8", "Cierre", "24", "Por Facturar", "5", "Cursado", "Contabilidad");
                ocultarDiv();
                dvSuccess.Style.Add("display", "block");
                lbSuccess.Text = util.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);
            }
            Page.Response.Redirect("ListarOperaciones.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DocumentosCurse");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }
    }
}
