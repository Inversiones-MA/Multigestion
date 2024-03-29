﻿using System;
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
using System.Xml;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiFiscalia.wpServiciosLegalesEmpresa.wpServiciosLegalesEmpresa
{
    [ToolboxItemAttribute(false)]
    public partial class wpServiciosLegalesEmpresa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpServiciosLegalesEmpresa()
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
        private static string pagina = "ServiciosLegalesEmpresa.aspx";

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

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
                btnAgregar.OnClientClick = "return ValidarSeleccionServicios('" + btnAgregar.ClientID.Substring(0, btnAgregar.ClientID.Length - 10) + "');";
                ocultarDiv();
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbOperacion.Text = objresumen.desOperacion;
                        LblEjecutivo.Text = objresumen.descEjecutivo;
                        lblNCertificado.Text = LN.ListarNumCertificado(objresumen.idOperacion);

                        try
                        {
                            mostrarDatos();
                            CargaDocumentos();
                        }
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ServiciosLegalesEmpresa", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Enabled = false;
                            pnFormulario.Enabled = false;
                            btnAgregar.Enabled = false;
                            btnGuardar.Enabled = false;
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                    validarCriticos();
                }

                //objresumen.area = dt.Rows[0]["Etapa"].ToString();  //"Fiscalia";
                try
                {
                    buscarArchivos();
                    tbServiciosOperaciones1.Columns[1].ItemStyle.Width = 0;
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;


                if (divFormulario != null)
                    util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    mensajeAlerta("Usuario sin permisos configurados");
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                mensajeAlerta("Usuario sin permisos configurados");
            }
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ServiciosLegalesEmpresa");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            int val = 0;
            DataTable dtArchivos = new DataTable("dtArchivos");
            DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
            LogicaNegocio Ln = new LogicaNegocio();

            string serviciosOperacion = generarXMLServiciosOperacion();
            //No requiere Servicios no obliga a tener un documento
            bool con = serviciosOperacion.Contains("No requiere Servicios");
            int sum = 0;

            if (con)
                sum = 1;
            else
                sum = 0;

            dtArchivosCriticos = Ln.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 2);
            if (dtArchivosCriticos.Rows.Count > 0)
            {
                dtArchivos = new Documentos { }.buscarArchivos(lbEmpresa.Text.Trim(), lbRut.Text.Trim(), objresumen.area.Trim(), objresumen.idOperacion.ToString());
                val = util.ValidarDocCriticos(dtArchivosCriticos, dtArchivos);
            }

            if (val >= dtArchivosCriticos.Rows.Count)
            {
                Boolean exito = true;

                exito = Ln.ActualizarSolicitudFiscalia(objresumen.idEmpresa.ToString(), serviciosOperacion, "4", objresumen.idOperacion.ToString(), objresumen.idUsuario);
                if (exito)
                {
                    ViewState["validacionFEmpresa"] = 4;
                    ocultarDiv();
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);
                    if (ViewState["validacionFEmpresa"].ToString() == "4" && ViewState["validacionFGarantia"].ToString() == "4")
                    {
                        mensajeExito("la etapa ya puede ser avanzada desde aprobación fiscalia");
                    }
                    else
                        mensajeAlerta("se deben validar los documentos criticos en : Servicios Legales Garantía, para finalizar la etapa legal");

                }
                else
                {
                    ocultarDiv();
                    mensajeError(Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                }
            }
            else
            {
                Boolean exito = true;
                exito = Ln.ActualizarSolicitudFiscaliaGarantia(objresumen.idEmpresa.ToString(), serviciosOperacion, "4", objresumen.idOperacion.ToString(), objresumen.idUsuario);
                if (exito)
                {
                    ViewState["validacionFEmpresa"] = 4;//1=finalizado 0 solo guardado
                    ocultarDiv();
                    mensajeAlerta(Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT) + " - " + "No se  finalizó la actividad, pues aún posee tareas pendientes");
                }
                else
                {
                    ocultarDiv();
                    mensajeError(Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                }
            }
        }

        protected void tbServiciosOperaciones1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[3].Visible = false;
        }

        protected void tbServiciosOperaciones1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gridDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-close");
                lb.CommandName = "Eliminar";
                lb.ToolTip = "Eliminar Archivo";
                lb.OnClientClick = "return Dialogo();";
                lb.CommandArgument = e.Row.RowIndex.ToString();
                lb.Command += gridDocumentos_Command;
                e.Row.Cells[7].Controls.Add(lb);

                LinkButton lbBajar = new LinkButton();
                lbBajar.CssClass = ("fa fa-download paddingIconos");
                lbBajar.CommandName = "Descargar";
                lbBajar.ToolTip = "Descargar Archivo";
                lbBajar.OnClientClick = "return Dialogo();";
                lbBajar.CommandArgument = e.Row.RowIndex.ToString();
                lbBajar.Command += gridDocumentos_Command;
                e.Row.Cells[7].Controls.Add(lbBajar);
            }
        }

        protected void gridDocumentos_Command(object sender, CommandEventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            bool borrar = false;
            int index = Convert.ToInt32(e.CommandArgument);
            string nombreArchivo = System.Web.HttpUtility.HtmlDecode(gridDocumentos.Rows[index].Cells[1].Text);
            string area = objresumen.area;
            string idOperacion = System.Web.HttpUtility.HtmlDecode(gridDocumentos.Rows[index].Cells[4].Text);

            if (e.CommandName == "Eliminar")
            {
                borrar = util.EliminarDocFiscalia(lbEmpresa.Text.Trim(), lbRut.Text.Trim(), objresumen.area, objresumen.idOperacion.ToString(), nombreArchivo);
            }
            if (borrar)
                buscarArchivos();
            else
                mensajeError("no se puede eliminar el archivo");

            if(e.CommandName == "Descargar")
            {
                string carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());

                string pathSinOperacion = string.Empty;
                string path = pathSinOperacion = carpetaEmpresa + "/" + area;
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

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + pathArchivo + "','_blank')", true);
            }

            validarCriticos();
        }

        protected void gridDocumentos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[6].Visible = false;
        }

        protected void tbServiciosOperaciones1_DataBound(object sender, EventArgs e)
        {
            // ejemplo... debe presenleccionar....pos 0 check 3 estado a preseleccionar
            for (int i = 0; i <= tbServiciosOperaciones1.Rows.Count - 1; i++)
            {
                if (System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[3].Text) != "")
                    (tbServiciosOperaciones1.Rows[i].FindControl("ckbServiciosEmpresa") as CheckBox).Checked = bool.Parse(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[3].Text));
                else
                    (tbServiciosOperaciones1.Rows[i].FindControl("ckbServiciosEmpresa") as CheckBox).Checked = false;
            }
        }

        protected void lbServiciosLegalesEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ServiciosLegalesEmpresa");
            Page.Response.Redirect("ServiciosLegalesEmpresa.aspx");
        }

        protected void lbServiciosLegalesGarantia_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ServiciosLegalesEmpresa");
            Page.Response.Redirect("ServiciosLegalesGarantia.aspx");
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                LogicaNegocio MTO = new LogicaNegocio();
                MTO.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "02");

                guardarArchivo();
                buscarArchivos();

                validarCriticos();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensajeError("Se ha producido un error al guardar el documento. Por favor validar que en nombre del archivo no contenga caracteres especiales.");
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

        private void validarCriticos()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
            DataTable dtArchivos = new DataTable("dtArchivos");
            int val = 0;

            //validar si existen documentos criticos, si existen no se puede mostrar boton aprobar en formulario "Aprobacion Soilicitud" hasta subir los documentos asociados
            dtArchivosCriticos = Ln.validarDocCriticos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), 2);
            if (dtArchivosCriticos.Rows.Count > 0)
            {
                dtArchivos = new Documentos { }.buscarArchivos(lbEmpresa.Text.Trim(), lbRut.Text.Trim(), objresumen.area.Trim(), objresumen.idOperacion.ToString());
                val = util.ValidarDocCriticos(dtArchivosCriticos, dtArchivos);
            }
            else
                btnFinalizar.Visible = true;

            if (val >= dtArchivosCriticos.Rows.Count)
                btnFinalizar.Visible = true;
            else
                btnFinalizar.Visible = false;

            if (dtArchivosCriticos.Rows.Count == 0)
            {
                //mensajeAlerta("Operacion sin archivos archivos criticos, puede finalizar la carga de documentos");
            }
            else
                mensajeAlerta("Se debe adjuntar el archivo critico antes de finalizar");
            //if (objresumen.desEtapa.ToLower() == "cierre")
            //{
            //    btnFinalizar.Visible = false;
            //}
        }

        protected void mostrarDatos()
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            DataSet res;
            res = LN.ListarResumenPAF(objresumen.idEmpresa.ToString(), objresumen.idPAF.ToString(), objresumen.idOperacion.ToString());//OJO
            lblRut.Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(res.Tables[0].Rows[0]["Rut"].ToString()), 0).ToString()).Replace(",", "") + '-' + res.Tables[0].Rows[0]["DivRut"].ToString();
            lblNombre.Text = res.Tables[0].Rows[0]["RazonSocial"].ToString();
            lbTipo.Text = res.Tables[0].Rows[0]["DescActividad"].ToString();
            lbEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();
            lbOperacion.Text = objresumen.idOperacion.ToString();
            lbOperacion1.Text = objresumen.idOperacion.ToString();
            if (res.Tables[0].Rows[0]["MontoAprobado"].ToString() != "")
                lbMonto.Text = "UF " + float.Parse(res.Tables[0].Rows[0]["MontoAprobado"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));

            lbPAF.Text = res.Tables[0].Rows[0]["NroPAF"].ToString();
            lbComentarioComite.Text = res.Tables[0].Rows[0]["ObservacionComite"].ToString();
            ViewState["Operaciones"] = res.Tables[1];

            try
            {
                if (!string.IsNullOrEmpty(res.Tables[3].Rows[0]["valEmpresa"].ToString()))
                    ViewState["validacionFEmpresa"] = int.Parse(res.Tables[3].Rows[0]["valEmpresa"].ToString());
                else
                    ViewState["validacionFEmpresa"] = "0";
                if (!string.IsNullOrEmpty(res.Tables[3].Rows[0]["valGarantia"].ToString()))
                    ViewState["validacionFGarantia"] = int.Parse(res.Tables[3].Rows[0]["valGarantia"].ToString());
                else
                    ViewState["validacionFGarantia"] = "0";
                if (!string.IsNullOrEmpty(res.Tables[3].Rows[0]["CGR"].ToString()))
                    ViewState["validacionCGR"] = int.Parse(res.Tables[3].Rows[0]["CGR"].ToString());
                else
                    ViewState["validacionCGR"] = "0";
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            servicios1();
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        Boolean vlidacion;
        private string generarXMLServiciosOperacion()
        {
            asignacionResumen(ref objresumen);
            vlidacion = true;
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
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
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("Descripcion");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[2].Text)));
                RespNodeCkb.AppendChild(nodo1);

                XmlNode nodoC = doc.CreateElement("Estado");
                nodoC.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((tbServiciosOperaciones1.Rows[i].FindControl("ckbServiciosEmpresa") as CheckBox).Checked.ToString())));
                RespNodeCkb.AppendChild(nodoC);

                if (!(tbServiciosOperaciones1.Rows[i].FindControl("ckbServiciosEmpresa") as CheckBox).Checked)
                    vlidacion = false;

                RespNode.AppendChild(RespNodeCkb);
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        private void servicios1()
        {
            LogicaNegocio LN = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            DataTable dt = new DataTable();
            DataTable dtServiciosOperacion = new DataTable();
            dt = LN.ServiciosOperacion(objresumen.idEmpresa.ToString(), objresumen.idPAF.ToString(), objresumen.descCargo, objresumen.idUsuario, objresumen.idOperacion.ToString()); //OJO
            tbServiciosOperaciones1.DataSource = dt;
            tbServiciosOperaciones1.DataBind();
        }

        private void mensajeError(string mensaje)
        {
            Control cc = this.FindControl("dvFormulario");
            dvError.Style.Add("display", "block");
            lbError.Text = mensaje;
        }

        private void mensajeAlerta(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje;
        }

        private void mensajeExito(string mensaje)
        {
            dvSuccess.Style.Add("display", "block");
            lbSuccess.Text = mensaje;
        }

        private void CargaDocumentos()
        {
            asignacionResumen(ref objresumen);
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList Lista = app.Lists["ServiciosOperacion"];
            SPQuery oQuery = new SPQuery();

            oQuery.Query = "<Where><Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq></Where>" +
                              "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
            SPListItemCollection items = Lista.GetItems(oQuery);
            util.CargaDDL(ddlTipo, items.GetDataTable(), "Nombre", "ID");
        }

        public void guardarArchivo()
        {
            asignacionResumen(ref objresumen);

            string carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            new Documentos { }.CargarDocFiscalia(carpetaEmpresa, objresumen.area, fileDocumento, objresumen.idEmpresa.ToString(), ddlTipo.SelectedItem.Text, lbOperacion.Text.Trim(), "Servicios Legales Empresa", Path.GetExtension(fileDocumento.FileName).ToLower());
        }

        private void buscarArchivos()
        {
            asignacionResumen(ref objresumen);
            DataTable dt = new DataTable();
            string carpetaEmpresa = util.carpetaEmpresa(lbEmpresa.Text.Trim(), lbRut.Text.Trim());
            dt = new Documentos { }.buscarArchivosFiscalia(carpetaEmpresa, objresumen.area.ToUpper(), objresumen.idOperacion.ToString());

            gridDocumentos.DataSource = dt;
            gridDocumentos.DataBind();
        }

        #endregion

    }
}
