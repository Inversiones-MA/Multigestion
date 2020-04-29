using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpDatosEmpresa.wpDatosEmpresa
{
    [ToolboxItemAttribute(false)]
    public partial class wpDatosEmpresa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDatosEmpresa()
        {
        }

        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "DatosEmpresa.aspx";


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
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                        lbEditar.OnClientClick = "return HabilitarDatosEmpresa('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                        btnCancel.OnClientClick = "return LimpiarDatosEmpresa('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

                        lbEditar.Style.Add("display", "block");
                        objresumen = new Resumen();
                        objresumen = ((Resumen)(Page.Session["RESUMEN"]));

                        Page.Session["RESUMEN"] = null;
                        ViewState["RES"] = objresumen;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        DataTable res;
                        res = Ln.ObtenerDetalleEmpresa(objresumen.idEmpresa.ToString());

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa", "Empresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            pnFormulario.Enabled = false;
                            btnGuardar.Style.Add("display", "none");
                            lbEditar.Visible = false;
                        }

                        if (res != null)
                        {
                            if (res.Rows.Count > 0)
                            {
                                txtAnos.Text = res.Rows[0]["AnosExperiencia"].ToString();
                                txtHistoria.Text = res.Rows[0]["Historia"].ToString();
                                txtAntecedentesEmpresa.Text = res.Rows[0]["AntecedentesAdicionales"].ToString();
                                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                                ViewState["IDDETALLEEMPRESA"] = res.Rows[0]["IdDetEmpresa"].ToString();
                                pnFormulario.Enabled = false;
                                btnCancel.Style.Add("display", "none");
                                btnGuardar.Style.Add("display", "none");
                                if (int.Parse(objresumen.idPermiso.ToString()) != Constantes.PERMISOS.SOLOLECTURA)
                                    lbEditar.Style.Add("display", "block");
                                //display:none;
                            }
                            else
                            {
                                ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                                btnCancel.Style.Add("display", "block");
                                btnGuardar.Style.Add("display", "block");
                            }
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            objresumen = (Resumen)ViewState["RES"];
            LogicaNegocio Ln = new LogicaNegocio();

            Boolean res;
            if ((Constantes.OPCION.INSERTAR).CompareTo(ViewState["ACCION"].ToString()) == 0)
            {
                if (res = Ln.InsertarDetalleEmpresa(objresumen.idEmpresa.ToString(), txtAnos.Text.ToString(), txtHistoria.Text.Trim(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), txtAntecedentesEmpresa.Text.Trim()))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETEMPRESACHIS);
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTEMPRESACHIS);
                }
            }
            else
            {
                if (Ln.ModificarDetalleEmpresa(objresumen.idEmpresa.ToString(), ViewState["IDDETALLEEMPRESA"].ToString(), txtAnos.Text, txtHistoria.Text, objresumen.idUsuario, objresumen.descCargo, txtAntecedentesEmpresa.Text.Trim()))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFEMPRESACHIS);
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFEMPRESACHIS);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtAnos.Text = "";
            txtHistoria.Text = "";
            LogicaNegocio Ln = new LogicaNegocio();
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REQUERIDODATOSEMPRESA);
        }

        protected void lbDatosEmpresa_Click(object sender, EventArgs e)
        {
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EdicionEmpresas.aspx");
        }

        protected void lbHistoria_Click(object sender, EventArgs e)
        {
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DatosEmpresa.aspx");
        }

        protected void lbDocumentos_Click(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Documental.aspx" + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(objresumen.desEmpresa));
        }

        protected void lbDirectorio_Click(object sender, EventArgs e)
        {
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DirectorioEmpresa.aspx");
        }

        protected void lbSociosACCIONistas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("SociosAccionistas.aspx");
        }

        protected void lbEmpresaRelacionada_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresasRelacionadas.aspx");
        }

        protected void lbContactos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ContactosEmpresa.aspx");
        }

        protected void lbDireccion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("DireccionEmpresa.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            objresumen = (Resumen)ViewState["RES"];
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void lbPersonas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            objresumen = (Resumen)ViewState["RES"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DatosEmpresa");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("personas.aspx");
        }

        #endregion


        #region Metodos

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
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

        #endregion
   
    }
}
