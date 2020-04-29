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

namespace MultiComercial.wpEnviarComercial.wpEnviarComercial
{
    [ToolboxItemAttribute(false)]
    public partial class wpEnviarComercial : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpEnviarComercial()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "EnviarComercial.aspx";
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        Dictionary<string, string> NuevsVal = new Dictionary<string, string>();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            //PERMISOS USUARIOS
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
                            btnGuardar.OnClientClick = "return Dialogo(); return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                            objresumen = (Resumen)Page.Session["RESUMEN"];
                            Page.Session["RESUMEN"] = null;
                            ViewState["RES"] = objresumen;
                            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            lbOperacion.Text = objresumen.desOperacion;
                            lbEjecutivo.Text = objresumen.descEjecutivo;
                            if (objresumen.area.Trim() != "Riesgo")
                            {
                                tbRiesgo.Visible = false;
                            }

                            Dictionary<string, string> valores = new Dictionary<string, string>();
                            Dictionary<string, string> val = new Dictionary<string, string>();
                            Boolean ban = true;
                            DataTable resOperacion = new DataTable();
                            string MensajeWorkflow = string.Empty;

                            valores = Ln.BuscarEstados(objresumen.idOperacion.ToString());
                            val = Ln.BuscarEstadoActual(valores["Estado"].ToString(), valores["Etapa"].ToString(), valores["SubEtapa"].ToString());

                            //Se incluye validacion de workflow Express para la etapa:Negociación, SubEtapa:Antecedentes y Evaluación y Estado:Ingresado
                            if (Convert.ToBoolean(val["Workflow"]))
                            {
                                MensajeWorkflow = Ln.ValidarWorkflowExpress(objresumen.idEmpresa, objresumen.idOperacion);
                                if (MensajeWorkflow == "OK")
                                    NuevsVal = Ln.BuscarNuevoEstado(val["NuevaIdEtapa"], true); //NUMERO COLUMNA ORDEN, DE LA LISTA CambiosEstados
                                else
                                    NuevsVal = Ln.BuscarNuevoEstado(val["CambioID"].ToString(), false);
                            }
                            else
                                NuevsVal = Ln.BuscarNuevoEstado(val["CambioID"].ToString(), false);

                            resOperacion = Ln.BuscarDatosOperacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString());

                            if (resOperacion.Rows[0]["IdProducto"].ToString() == "4" && (objresumen.desEtapa == "Negociacion" || objresumen.desEtapa == "Negociación"))
                            {
                                //-Cursado	5	Cierre	8	Por Facturar	24	1	0	Operaciones
                                Dictionary<string, string> valores1 = new Dictionary<string, string>();
                                valores1.Add("CambioID", "17");
                                valores1.Add("IdEtapa", "8");
                                valores1.Add("Etapa", "Cierre");
                                valores1.Add("IdSubEtapa", "24");
                                valores1.Add("SubEtapa", "Por Facturar");
                                valores1.Add("IdEstado", "5");
                                valores1.Add("Estado", "Cursado");
                                valores1.Add("Area", "Contabilidad");
                                valores1.Add("Cambio", "1");
                                valores1.Add("Admin", "0");
                                ban = false;

                                NuevsVal = valores1;
                            }

                            ViewState["Nuevo"] = NuevsVal;

                            //Cambio 1 indica si cambia de estado
                            if (NuevsVal["Cambio"].ToString() == "1")
                            {
                                ocultarDiv();
                                dvWarning.Style.Add("display", "block");
                                lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.CAMBIARESTADO).Replace("estado", "estado - " + NuevsVal["SubEtapa"].ToString());
                            }

                            //cambio 2 indica que cambia de Area.
                            if (NuevsVal["Cambio"].ToString() == "2")
                            {
                                ocultarDiv();
                                dvWarning.Style.Add("display", "block");
                                lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.CAMBIARETAPA).Replace("etapa", "etapa - " + NuevsVal["Etapa"].ToString());
                            }

                            if (val["Area"].ToString() != objresumen.area.Trim())
                            {
                                ocultarDiv();
                                dvWarning.Style.Add("display", "block");
                                lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.ENVIARSINPERMISO);
                                Panel1.Visible = false;
                                btnGuardar.Visible = false;
                            }

                            DataTable resValidacion = new DataTable();
                            resValidacion = Ln.ValidarCambio(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, NuevsVal["IdEtapa"].ToString(), NuevsVal["IdSubEtapa"].ToString(), NuevsVal["IdEstado"].ToString());

                            if (resValidacion != null && resValidacion.Rows.Count > 0)
                            {
                                string mensaje = resValidacion.Rows[0]["Mensaje"].ToString();
                                if (mensaje != "EXITO" && mensaje != "" && ban)
                                {
                                    ocultarDiv();
                                    dvWarning.Style.Add("display", "block");
                                    lbWarning.Text = mensaje;
                                    txtComentarios.Enabled = false;
                                    btnGuardar.Enabled = false;
                                }
                            }

                            //////////Verificación Edicion Simultanea        
                            ////////string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EnviarComercial", "Operacion");
                            ////////if (!string.IsNullOrEmpty(UsuarioFormulario))
                            ////////{
                            ////////    ocultarDiv();
                            ////////    dvWarning.Style.Add("display", "block");
                            ////////    lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            ////////    btnGuardar.Enabled = false;
                            ////////    txtComentarios.Enabled = false;
                            ////////}
                        }

                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            btnGuardar.Visible = false;
                            lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.ENVIARSINPERMISO);
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                }
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

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    ocultarDiv();
        //    LogicaNegocio Ln = new LogicaNegocio();

        //    if (Page.Session["RESUMEN"] != null)
        //    {
        //        ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
        //    }
        //    asignacionResumen(ref objresumen);

        //    //PERMISOS USUARIOS
        //    SPWeb app2 = SPContext.Current.Web;
        //    DataTable dt = new DataTable("dt");
        //    ValidarPermisos validar = new ValidarPermisos
        //    {
        //        NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
        //        Pagina = pagina,
        //        Etapa = objresumen.area,
        //    };

        //    dt = validar.ListarPerfil(validar);
        //    if (dt.Rows.Count > 0)
        //    {
        //        if (!Page.IsPostBack)
        //        {
        //            Page.Session["NuevasVal"] = null;
        //            if (Page.Session["RESUMEN"] != null)
        //            {
        //                try
        //                {
        //                    btnGuardar.OnClientClick = "return Dialogo(); return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
        //                    objresumen = (Resumen)Page.Session["RESUMEN"];
        //                    Page.Session["RESUMEN"] = null;
        //                    ViewState["RES"] = objresumen;
        //                    lbEmpresa.Text = objresumen.desEmpresa;
        //                    lbRut.Text = objresumen.rut;
        //                    lbOperacion.Text = objresumen.desOperacion;
        //                    lbEjecutivo.Text = objresumen.descEjecutivo;
        //                    if (objresumen.area.Trim() != "Riesgo")
        //                    {
        //                        tbRiesgo.Visible = false;
        //                    }

        //                    //version 2.0
        //                    // estado actual segun tabla cambioEstados
        //                    DataTable dtValorActual = Ln.CambioEstado(2, objresumen.idEstado, objresumen.idEtapa, objresumen.idSubEtapa, false);
        //                    if (dtValorActual.Rows[0]["NombreArea"].ToString().ToLower() != objresumen.area.Trim().ToLower())
        //                    {
        //                        warning(util.buscarMensaje(Constantes.MENSAJES.ENVIARSINPERMISO));
        //                        Panel1.Visible = false;
        //                        btnGuardar.Visible = false;
        //                        return;
        //                    }

        //                    DataTable NuevasVal = Ln.CambioEstado(1, Convert.ToInt32(dtValorActual.Rows[0]["Orden"]) + 1, false);
        //                    DataTable resValidacion = Ln.ValidarCambio(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, NuevasVal.Rows[0]["IdEtapa"].ToString(), NuevasVal.Rows[0]["IdSubEtapa"].ToString(), NuevasVal.Rows[0]["IdEstado"].ToString());
        //                    string mensaje = resValidacion.Rows[0]["Mensaje"].ToString();
        //                    if (mensaje != "EXITO" && mensaje != "")
        //                    {
        //                        warning(mensaje);
        //                        txtComentarios.Enabled = false;
        //                        btnGuardar.Enabled = false;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        //validar si hay cambio de area
        //                        if (NuevasVal.Rows[0]["NombreArea"].ToString() != objresumen.area)
        //                            warning(util.buscarMensaje(Constantes.MENSAJES.CAMBIARETAPA).Replace("etapa", "etapa - " + NuevasVal.Rows[0]["DescripcionEtapa"].ToString()));
        //                        else
        //                            warning(util.buscarMensaje(Constantes.MENSAJES.CAMBIARESTADO).Replace("estado", "estado - " + NuevasVal.Rows[0]["NombreSubEtapa"].ToString()));
        //                        Page.Session["NuevasVal"] = NuevasVal;
        //                    }

        //                    //DataTable dtSubEtapa = new DataTable("dtSubEtapa");
        //                    //var SiguienteSubEtapa = new SubEtapas
        //                    //{
        //                    //    IdEtapa = objresumen.idEtapa,
        //                    //    Rechazo = false,
        //                    //    IdOpcion = 1,
        //                    //};
        //                    //dtSubEtapa = new SubEtapas { }.ListarSubEtapas(SiguienteSubEtapa);
        //                    //if (!dtSubEtapa.SinDatos())
        //                    //   warning(util.buscarMensaje(Constantes.MENSAJES.CAMBIARESTADO).Replace("estado", "estado - " + dtSubEtapa.Rows[0]["Nombre"].ToString()));
        //                }
        //                catch (Exception ex)
        //                {
        //                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //                    btnGuardar.Visible = false;
        //                    warning(util.buscarMensaje(Constantes.MENSAJES.ENVIARSINPERMISO));
        //                }
        //            }
        //            else
        //            {
        //                Page.Response.Redirect("MensajeSession.aspx");
        //            }
        //        }
        //        validar.Permiso = dt.Rows[0]["Permiso"].ToString();
        //        Control divFormulario = this.FindControl("dvFormulario");
        //        bool TieneFiltro = true;
        //        util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
        //    }
        //    else
        //    {
        //        dvFormulario.Style.Add("display", "none");
        //        warning("Usuario sin permisos configurados");
        //    }
        //}

        private void warning(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EnviarComercial");
            //nueva version junio
            //try
            //{
            //    if (Page.Session["NuevasVal"] == null)
            //        throw new Exception(util.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));

            //    DataTable dt = (DataTable)Page.Session["NuevasVal"];
            //    if (objresumen.area.Trim() == "Riesgo")
            //    {
            //        //        NuevsVal["IdSubEtapa"] = ddlRiesgoSubestado.SelectedValue.ToString(); //22
            //        //        NuevsVal["SubEtapa"] = ddlRiesgoSubestado.SelectedItem.ToString();  //aprobado
            //    }

            //    Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
            //        objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), dt.Rows[0]["IdEtapa"].ToString(),
            //        dt.Rows[0]["Etapa"].ToString(), dt.Rows[0]["IdSubEtapa"].ToString(), dt.Rows[0]["SubEtapa"].ToString(),
            //        dt.Rows[0]["IdEstado"].ToString(), dt.Rows[0]["Estado"].ToString(), dt.Rows[0]["Area"].ToString(), txtComentarios.Text.ToString());
            //}
            //catch(Exception ex)
            //{

            //}


            try
            {
                NuevsVal = (Dictionary<string, string>)ViewState["Nuevo"];

                //comentario el 22-05-2017, si la evaluacion la avanza riesgo se asigana la subEtapa 22 ??
                if (objresumen.area.Trim() == "Riesgo")
                {
                    NuevsVal["IdSubEtapa"] = ddlRiesgoSubestado.SelectedValue.ToString(); //22
                    NuevsVal["SubEtapa"] = ddlRiesgoSubestado.SelectedItem.ToString();  //aprobado
                }

                if (Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                    objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), NuevsVal["IdEtapa"].ToString(),
                    NuevsVal["Etapa"].ToString(), NuevsVal["IdSubEtapa"].ToString(), NuevsVal["SubEtapa"].ToString(),
                    NuevsVal["IdEstado"].ToString(), NuevsVal["Estado"].ToString(), NuevsVal["Area"].ToString(), txtComentarios.Text.ToString()))
                {
                    Ln.ActualizarEstado(objresumen.idOperacion.ToString(), NuevsVal["IdEtapa"].ToString(),
                    NuevsVal["Etapa"].ToString(), NuevsVal["IdSubEtapa"].ToString(), NuevsVal["SubEtapa"].ToString(),
                    NuevsVal["IdEstado"].ToString(), NuevsVal["Estado"].ToString(), NuevsVal["Area"].ToString());
                    ocultarDiv();
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSET);
                }
                else
                {
                    ocultarDiv();
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL);
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EnviarComercial");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void GridEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        { }

        protected void GridNegocio_SelectedIndexChanged(object sender, EventArgs e)
        { }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        { }

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
            LogicaNegocio Ln = new LogicaNegocio();
            DataSet res;
            try
            {
                res = Ln.ListarResumen(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString());
                //GridEmpresa.DataSource = Tables[0];
                //GridNegocio.DataSource = Tables[1];

                lbRutEmpresa.Text = res.Tables[1].Rows[0]["Rut"].ToString();
                lbRazonSocial.Text = res.Tables[1].Rows[0]["RazonSocial"].ToString();
                lbFecha.Text = res.Tables[1].Rows[0]["FecInicioActEco"].ToString();
                lbTelefono.Text = res.Tables[1].Rows[0]["telefono"].ToString();
                lbActividad.Text = res.Tables[1].Rows[0]["DescActividad"].ToString();
                lbEmpleados.Text = res.Tables[1].Rows[0]["NumEmpleados"].ToString();

                lbProducto.Text = res.Tables[2].Rows[0]["DescProducto"].ToString();
                lbEstado.Text = res.Tables[2].Rows[0]["estado"].ToString();
                lbFinalidad.Text = res.Tables[2].Rows[0]["DescFinalidad"].ToString();
                lbMonto.Text = res.Tables[2].Rows[0]["monto"].ToString();
                lbPlazo.Text = res.Tables[2].Rows[0]["Plazo"].ToString();
                lbComision.Text = res.Tables[2].Rows[0]["Comision"].ToString();

                GridGarantias.DataSource = res.Tables[7];
                GridDocumentos.DataSource = res.Tables[3];
                GridSocios.DataSource = res.Tables[4];
                GridDirectorio.DataSource = res.Tables[5];
                GridEmpresaRelacionada.DataSource = res.Tables[6];
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            GridEmpresa.DataBind();
            GridNegocio.DataBind();
            GridGarantias.DataBind();
            GridDocumentos.DataBind();
            GridSocios.DataBind();
            GridDirectorio.DataBind();
            GridEmpresaRelacionada.DataBind();
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        #endregion

    }
}
