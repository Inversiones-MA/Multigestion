using Bd;
using ClasesNegocio;
using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using MultigestionUtilidades;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls.WebParts;

namespace MultiFiscalia.wpDevolverOperacionFiscalia.wpDevolverOperacionFiscalia
{
    [ToolboxItemAttribute(false)]
    public partial class wpDevolverOperacionFiscalia : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDevolverOperacionFiscalia()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "DevolverOperacionFiscalia.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

   
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = string.Empty,
            };

            dt = validar.ListarPerfil(validar);
            Page.Session["dtRol"] = dt;
            if (dt.Rows.Count > 0)
            {
                ocultarDiv();

                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        try
                        {
                            //btnGuardar.OnClientClick = "return Dialogo(); return Validar('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                            objresumen = (Resumen)Page.Session["RESUMEN"];
                            Page.Session["RESUMEN"] = null;

                            //ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            lbOperacion.Text = objresumen.desOperacion;
                            lbEjecutivo.Text = objresumen.descEjecutivo;
                            CargarInfoSubEtapa(objresumen);         
                    
                        }
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                            dvWarning.Style.Add("display", "block");
                            btnGuardar.Visible = false;
                            lbWarning.Text = "No puede realizar cambio a la etapa anterior, Operacion con inconsistencia de datos";
                        }
                    }
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        private void CargarInfoSubEtapa(Resumen objresumen)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            var dtSubEtapa = Ln.ListarSubEtapaLegal(objresumen.OrdenSubEtapaLegal - 1);
            lbSuccess.Text = "SubEtapa Legal: " + dtSubEtapa.Rows[0]["EdoServiciosLegales"].ToString();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
            {
                objresumen = (Resumen)ViewState["RES"];
            }
            else
            {
                Page.Response.Redirect("MensajeSession.aspx");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);

            var devolver = Ln.GestionSubEtapaLegal(objresumen.idOperacion, objresumen.OrdenSubEtapaLegal, txtComentarios.Text.Trim(), 1);
            if(devolver)
                Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            //dvSuccess.Style.Add("display", "none");
        }
    }
}
