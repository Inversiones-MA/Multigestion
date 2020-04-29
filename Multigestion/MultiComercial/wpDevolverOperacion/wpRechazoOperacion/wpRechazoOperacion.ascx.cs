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

namespace MultiComercial.wpDevolverOperacion.wpRechazoOperacion
{
    [ToolboxItemAttribute(false)]
    public partial class wpRechazoOperacion : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpRechazoOperacion()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Dictionary<string, string> valores = new Dictionary<string, string>();
        Utilidades util = new Utilidades();
        private static string pagina = "DevolverOperacion.aspx";

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
                            btnGuardar.OnClientClick = "return Dialogo(); return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                            objresumen = (Resumen)Page.Session["RESUMEN"];
                            Page.Session["RESUMEN"] = null;
                            ViewState["RES"] = objresumen;
                            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            lbOperacion.Text = objresumen.desOperacion;
                            lbEjecutivo.Text = objresumen.descEjecutivo;

                            valores = Ln.BuscarEstados(objresumen.idOperacion.ToString());
                            Dictionary<string, string> val = new Dictionary<string, string>();
                            val = Ln.BuscarEstadoActual(valores["Estado"].ToString(), valores["Etapa"].ToString(), valores["SubEtapa"].ToString());
                            Dictionary<string, string> estadoAnterior = new Dictionary<string, string>();
                            estadoAnterior = DatosEstadoDevolucionOperacion(valores);

                            if (estadoAnterior.Count < 1)
                            {
                                ocultarDiv();
                                dvWarning.Style.Add("display", "block");
                                btnGuardar.Visible = false;
                                lbWarning.Text = "No puede realizar cambio a la etapa anterior, No posee una etapa anterior configurada a la cual devolver la operación.";
                            }
                            else
                            {
                                ocultarDiv();
                                dvSuccess.Style.Add("display", "block");
                                lbSuccess.Text = "   Estado:" + estadoAnterior["Estado"].ToString() + " - Etapa:" + estadoAnterior["Etapa"].ToString() + "  - SubEtapa:" + estadoAnterior["SubEtapa"].ToString() + "  - Area:" + estadoAnterior["Area"].ToString();
                            }

                            ////////Verificación Edicion Simultanea        
                            //////string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "RechazoOperacion", "Operacion");
                            //////if (!string.IsNullOrEmpty(UsuarioFormulario))
                            //////{
                            //////    ocultarDiv();
                            //////    dvWarning.Style.Add("display", "block");
                            //////    lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            //////    btnGuardar.Style.Add("display", "none");
                            //////}

                        }

                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            btnGuardar.Visible = false;
                            lbWarning.Text = "No puede realizar cambio a la etapa anterior, Operacion con inconsistencia de datos";
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
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
        //    string PermisoConfigurado = string.Empty;
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

        //                    lbEmpresa.Text = objresumen.desEmpresa;
        //                    lbRut.Text = objresumen.rut;
        //                    lbOperacion.Text = objresumen.desOperacion;
        //                    lbEjecutivo.Text = objresumen.descEjecutivo;

        //                    //version 2.0 
        //                    DataTable NuevasVal = null;
        //                    DataTable dtValorActual = Ln.CambioEstado(2, objresumen.idEstado, objresumen.idEtapa, objresumen.idSubEtapa, false);
        //                    if (dtValorActual.Rows[0]["NombreArea"].ToString().ToLower() != objresumen.area.Trim().ToLower())
        //                    {
        //                        warning(util.buscarMensaje(Constantes.MENSAJES.ENVIARSINPERMISO));
        //                        Panel1.Visible = false;
        //                        btnGuardar.Visible = false;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        //si operacion viene de comite --> aprobado, retroceder a comite condicionado ordernid = 7
        //                        if (objresumen.idEtapa == 5 && objresumen.idSubEtapa == 15)
        //                            NuevasVal = Ln.CambioEstado(1, Convert.ToInt32(dtValorActual.Rows[0]["Orden"]) - 1, true);
        //                        else if(objresumen.idEtapa == 6 && objresumen.idSubEtapa == 17)
        //                            NuevasVal = Ln.CambioEstado(1, Convert.ToInt32(dtValorActual.Rows[0]["Orden"]) - 1, true);
        //                        else
        //                            NuevasVal = Ln.CambioEstado(1, Convert.ToInt32(dtValorActual.Rows[0]["Orden"]) - 1, false);
        //                        Page.Session["NuevasVal"] = NuevasVal;
        //                        dvSuccess.Style.Add("display", "block");
        //                        lbSuccess.Text = "   Estado:" + NuevasVal.Rows[0]["DescripcionEstado"].ToString() + " - Etapa:" + NuevasVal.Rows[0]["DescripcionEtapa"].ToString() + "  - SubEtapa:" + NuevasVal.Rows[0]["NombreSubEtapa"].ToString() + "  - Area:" + NuevasVal.Rows[0]["NombreArea"].ToString();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //                    btnGuardar.Visible = false;
        //                    warning("No puede realizar cambio a la etapa anterior, Operacion con inconsistencia de datos");
        //                }
        //            }
        //            else
        //                Page.Response.Redirect("MensajeSession.aspx");
        //        }

        //        validar.Permiso = dt.Rows[0]["Permiso"].ToString();
        //        Control divFormulario = this.FindControl("dvFormulario");
        //        bool TieneFiltro = true;
        //        util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
        //    }
        //    else
        //    {
        //        dvFormulario.Style.Add("display", "none");
        //        dvWarning1.Style.Add("display", "block");
        //        lbWarning1.Text = "Usuario sin permisos configurados";
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

            //nueva version junnio
            //try
            //{
            //    if(Page.Session["NuevasVal"] == null)
            //        throw new Exception("No puede realizar cambio a la etapa anterior, No posee una etapa anterior a la cual devolver.");
                
            //    DataTable dt = (DataTable)Page.Session["NuevasVal"];
            //    Ln.DevolverEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
            //            objresumen.idUsuario, objresumen.descCargo, dt.Rows[0]["IdEtapa"].ToString(),
            //            dt.Rows[0]["Etapa"].ToString(), dt.Rows[0]["IdSubEtapa"].ToString(), dt.Rows[0]["SubEtapa"].ToString(),
            //            dt.Rows[0]["IdEstado"].ToString(), dt.Rows[0]["Estado"].ToString(), dt.Rows[0]["Area"].ToString()
            //            ,txtComentarios.Text.ToString());
            //}
            //catch(Exception ex)
            //{
            //    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            //    warning(ex.Message);
            //}


            if (objresumen.area.Trim() == "Riesgo")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            }
            else if (objresumen.area.Trim() == "Operaciones")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");
            }
            else if (objresumen.area.Trim() == "Fiscalia")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "02");
            }
            else if (objresumen.area.Trim() == "Contabilidad")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "04");
            }

            try
            {
                valores = Ln.BuscarEstados(objresumen.idOperacion.ToString());

                Dictionary<string, string> estadoAnterior = new Dictionary<string, string>();
                estadoAnterior = DatosEstadoDevolucionOperacion(valores);

                if (estadoAnterior.Count < 1)
                {
                    ocultarDiv();
                    dvWarning.Style.Add("display", "block");
                    btnGuardar.Visible = false;
                    lbWarning.Text = "No puede realizar cambio a la etapa anterior, No posee una etapa anterior a la cual devolver.";
                }
                else
                    if (Ln.DevolverEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                        objresumen.idUsuario, objresumen.descCargo, estadoAnterior["IdEtapa"].ToString(),
                        estadoAnterior["Etapa"].ToString(), estadoAnterior["IdSubEtapa"].ToString(), estadoAnterior["SubEtapa"].ToString(),
                        estadoAnterior["IdEstado"].ToString(), estadoAnterior["Estado"].ToString(), estadoAnterior["Area"].ToString(), txtComentarios.Text.ToString()))
                    {
                        Ln.ActualizarEstado(objresumen.idOperacion.ToString(), estadoAnterior["IdEtapa"].ToString(),
                        estadoAnterior["Etapa"].ToString(), estadoAnterior["IdSubEtapa"].ToString(), estadoAnterior["SubEtapa"].ToString(),
                        estadoAnterior["IdEstado"].ToString(), estadoAnterior["Estado"].ToString(), estadoAnterior["Area"].ToString());
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
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "RechazoOperacion");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        #endregion


        #region Metodos

        public Dictionary<string, string> DatosEstadoDevolucionOperacion(Dictionary<string, string> estadoActual)
        {
            Dictionary<string, string> estadoAnterior = new Dictionary<string, string>();

            if (estadoActual["Estado"].ToString() == "5" && estadoActual["Etapa"].ToString() == "8" && estadoActual["SubEtapa"].ToString() == "26")
            {
                estadoAnterior.Add("IdEstado", "5");
                estadoAnterior.Add("Estado", "Cursado");
                estadoAnterior.Add("IdEtapa", "8");
                estadoAnterior.Add("Etapa", "Cierre");
                estadoAnterior.Add("IdSubEtapa", "25");
                estadoAnterior.Add("SubEtapa", "Facturado");
                estadoAnterior.Add("Area", "Contabilidad");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "5" && estadoActual["Etapa"].ToString() == "8" && estadoActual["SubEtapa"].ToString() == "25")
            {
                estadoAnterior.Add("IdEstado", "5");
                estadoAnterior.Add("Estado", "Cursado");
                estadoAnterior.Add("IdEtapa", "8");
                estadoAnterior.Add("Etapa", "Cierre");
                estadoAnterior.Add("IdSubEtapa", "24");
                estadoAnterior.Add("SubEtapa", "Por Facturar");
                estadoAnterior.Add("Area", "Contabilidad");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "5" && estadoActual["Etapa"].ToString() == "8" && estadoActual["SubEtapa"].ToString() == "24")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "8");
                estadoAnterior.Add("Etapa", "Cierre");
                estadoAnterior.Add("IdSubEtapa", "23");
                estadoAnterior.Add("SubEtapa", "Por Emitir");
                estadoAnterior.Add("Area", "Operaciones");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "8" && estadoActual["SubEtapa"].ToString() == "23")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "6");
                estadoAnterior.Add("Etapa", "Fiscalia");
                estadoAnterior.Add("IdSubEtapa", "19");
                estadoAnterior.Add("SubEtapa", "Aprobación Fiscalía");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "6" && estadoActual["SubEtapa"].ToString() == "19")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "6");
                estadoAnterior.Add("Etapa", "Fiscalia");
                estadoAnterior.Add("IdSubEtapa", "18");
                estadoAnterior.Add("SubEtapa", "Confección Contratos");
                estadoAnterior.Add("Area", "Fiscalia");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }


            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "6" && estadoActual["SubEtapa"].ToString() == "18")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "6");
                estadoAnterior.Add("Etapa", "Fiscalia");
                estadoAnterior.Add("IdSubEtapa", "17");
                estadoAnterior.Add("SubEtapa", "Aprobación Servicios");
                estadoAnterior.Add("Area", "Fiscalia");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }


            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "6" && estadoActual["SubEtapa"].ToString() == "17")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "5");
                estadoAnterior.Add("Etapa", "Comité");
                estadoAnterior.Add("IdSubEtapa", "13");
                estadoAnterior.Add("SubEtapa", "Condicionado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            //-------------

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "5" && estadoActual["SubEtapa"].ToString() == "13")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "9");
                estadoAnterior.Add("SubEtapa", "Condicionado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }


            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "5" && estadoActual["SubEtapa"].ToString() == "10")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "9");
                estadoAnterior.Add("SubEtapa", "Condicionado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "5" && estadoActual["SubEtapa"].ToString() == "15")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "9");
                estadoAnterior.Add("SubEtapa", "Condicionado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "5" && estadoActual["SubEtapa"].ToString() == "12")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "9");
                estadoAnterior.Add("SubEtapa", "Condicionado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            //-------------
            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "3" && estadoActual["SubEtapa"].ToString() == "9")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "8");
                estadoAnterior.Add("SubEtapa", "Procesado");
                estadoAnterior.Add("Area", "Riesgo");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }


            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "3" && estadoActual["SubEtapa"].ToString() == "22")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "8");
                estadoAnterior.Add("SubEtapa", "Procesado");
                estadoAnterior.Add("Area", "Riesgo");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "5" && estadoActual["SubEtapa"].ToString() == "22")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "3");
                estadoAnterior.Add("Etapa", "Evaluación Riesgo");
                estadoAnterior.Add("IdSubEtapa", "8");
                estadoAnterior.Add("SubEtapa", "Procesado");
                estadoAnterior.Add("Area", "Riesgo");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            //-------------

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "3" && estadoActual["SubEtapa"].ToString() == "8")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "2");
                estadoAnterior.Add("Etapa", "Negociación");
                estadoAnterior.Add("IdSubEtapa", "5");
                estadoAnterior.Add("SubEtapa", "Antecedentes y Evaluación");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "2" && estadoActual["SubEtapa"].ToString() == "5")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "1");
                estadoAnterior.Add("Etapa", "Prospecto");
                estadoAnterior.Add("IdSubEtapa", "2");
                estadoAnterior.Add("SubEtapa", "Contactado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            if (estadoActual["Estado"].ToString() == "1" && estadoActual["Etapa"].ToString() == "1" && estadoActual["SubEtapa"].ToString() == "2")
            {
                estadoAnterior.Add("IdEstado", "1");
                estadoAnterior.Add("Estado", "Ingresado");
                estadoAnterior.Add("IdEtapa", "1");
                estadoAnterior.Add("Etapa", "Prospecto");
                estadoAnterior.Add("IdSubEtapa", "1");
                estadoAnterior.Add("SubEtapa", "Sin Contactado");
                estadoAnterior.Add("Area", "Comercial");
                estadoAnterior.Add("ID", estadoActual["ID"].ToString());
            }

            return estadoAnterior;
        }

        public void asignacionResumen(ref  Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
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
