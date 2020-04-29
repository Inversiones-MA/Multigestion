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
using MultigestionUtilidades;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpRechazo.wpRechazo
{
    [ToolboxItemAttribute(false)]
    public partial class wpRechazo : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpRechazo()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "Rechazo.aspx";
        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            ////PERMISOS USUARIOS
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
                    btnGuardar.OnClientClick = "return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                    ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                    lbEmpresa.Text = objresumen.desEmpresa;
                    lbRut.Text = objresumen.rut;
                    lbOperacion.Text = objresumen.desOperacion;

                    CargarEdoRechazo(objresumen.idEtapa);

                    if (int.Parse(objresumen.idPermiso.ToString()) == Constantes.PERMISOS.SOLOLECTURA)
                    {
                        btnGuardar.Enabled = false;
                        warning(string.Format("{0}{1}{2}", Ln.buscarMensaje(Constantes.MENSAJES.ACCESOLIMITADO), " - La Operación se encuentra en otra atapa : ", objresumen.desEtapa));
                    }
                    else
                    {
                        warning("Si estas seguro de rechazar esa operación, por favor ingrese el estado de rechazo y el motivo del rechazo, a continuación presione 'ENVIAR'.");
                    }
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
                util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        private void warning(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            bool exito = false;
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);

            //nuevar version junio 
            //try
            //{
            //    if (cb_estados.SelectedValue == "-1")
            //        throw new Exception("Debe Seleccionar el estado de Rechazo");
            //    if (string.IsNullOrEmpty(txtComentarios.Text.ToString()))
            //        throw new Exception("Debe Ingresar un comentario de Rechazo");

            //    if (Ln.InsertarActualizarEstadosRechazo(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
            //                                   objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), cb_estados.SelectedValue.ToString()
            //                                   ,cb_estados.SelectedItem.ToString(), txtComentarios.Text.ToString()))
            //        Page.Response.Redirect(objresumen.linkPrincial);
            //}
            //catch (Exception ex)
            //{
            //    warning(ex.Message);
            //}

            if (objresumen.area.Trim() == "Riesgo")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            }
            else if (objresumen.area.Trim() == "Fiscalia")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "02");
            }
            else if (objresumen.area.Trim() == "Operaciones")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");
            }
            else if (objresumen.area.Trim() == "Contabilidad")
            {
                Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "04");
            }

            try
            {
                Dictionary<string, string> valoresAct = new Dictionary<string, string>();//tabla operaciones

                if (cb_estados.SelectedValue == "-1")
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "Debe Seleccionar el estado de Rechazo";
                    exito = false;
                }
                else if (txtComentarios.Text.ToString() == "")
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "Debe Ingresar un comentario de Rechazo";
                    exito = false;
                }
                else if (Ln.InsertarActualizarEstadosRechazo(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                                               objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), cb_estados.SelectedValue.ToString(), cb_estados.SelectedItem.ToString(),
                                               txtComentarios.Text.ToString()))
                {
                    Ln.ActualizarEstadoRechazo(objresumen.idOperacion.ToString(), cb_estados.SelectedValue.ToString());
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETEMPRESARELAC);
                    exito = true;
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTEMPRESARELAC);
                    exito = false;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                dvSuccess.Style.Add("display", "block");
                lbSuccess.Text = ex.Source.ToString() + ex.Message.ToString();

            }
            if (exito)
                Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        #endregion


        #region Metodos

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
        }

        private void CargarEdoRechazo(int IdEtapa)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtRechazo = Ln.ListarSubEtapas(IdEtapa, true, 1);
            util.CargaDDL(cb_estados, dtRechazo, "Nombre", "ID");
        }

        #endregion

    }
}

