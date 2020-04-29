using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Web;
using System.Data;
using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using Bd;
using ClasesNegocio;

namespace MultiFiscalia.Representantes.Representantes
{
    [ToolboxItemAttribute(false)]
    public partial class Representantes : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public Representantes()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "Representantes.aspx";
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            DataTable tabla = new DataTable("dt");
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
                        Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa.ToString();
                        lbOperacion.Text = objresumen.desOperacion.ToString();
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        DataTable dtRepresentantes = new DataTable();

                        dtRepresentantes = Ln.ListarRepresentantesId(objresumen.idOperacion);
                        if (dtRepresentantes.Rows.Count > 0)
                        {
                            CargarClaseA();
                            ddlClaseA.SelectedIndex = ddlClaseA.Items.IndexOf(ddlClaseA.Items.FindByValue(dtRepresentantes.Rows[0]["IdRepresentanteA"].ToString()));

                            CargarClaseB();
                            ddlClaseB.SelectedIndex = ddlClaseB.Items.IndexOf(ddlClaseB.Items.FindByValue(dtRepresentantes.Rows[0]["IdRepresentanteB"].ToString()));
                        }
                        else
                        {
                            CargarClaseA();
                            CargarClaseB();
                        }
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

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ListarFiscalia.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            SPWeb app2 = SPContext.Current.Web;
            LogicaNegocio Ln = new LogicaNegocio();
            bool ok = false;
            int id, claseA, claseB;

            id = 0;
            claseA = ddlClaseA.SelectedValue == null ? 0 : int.Parse(ddlClaseA.SelectedValue);
            claseB = ddlClaseB.SelectedValue == null ? 0 : int.Parse(ddlClaseB.SelectedValue);

            ok = Ln.InsertaActualizaRepresentantes(id, objresumen.idOperacion, claseA, claseB, app2.CurrentUser.Name);

            if (ok)
                exito("Ingreso correcto");
            else
                error("problemas al ingresar los representantes");
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

        private void CargarClaseA()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("ClaseA");

            dt = Ln.ListarClaseRepresentantes("A");

            ddlClaseA.DataSource = dt;
            ddlClaseA.DataValueField = "Id";
            ddlClaseA.DataTextField = "Nombre";
            ddlClaseA.DataBind();
            //ddlClaseA.Items.Insert(0, new ListItem("CLASE A", "0"));
        }

        private void CargarClaseB()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("ClaseB");

            dt = Ln.ListarClaseRepresentantes("B");

            ddlClaseB.DataSource = dt;
            ddlClaseB.DataValueField = "Id";
            ddlClaseB.DataTextField = "Nombre";
            ddlClaseB.DataBind();
            //ddlClaseB.Items.Insert(0, new ListItem("CLASE B", "0"));
        }

             
        private void exito(string mensaje)
        {
            dvSuccess.Style.Add("display", "block");
            lbSuccess.Text = mensaje;
        }

        private void error(string mensaje)
        {
            dvError.Style.Add("display", "block");
            lbError.Text = mensaje;
        }

        private void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        #endregion

    }
}
