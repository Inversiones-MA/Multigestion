using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpIngresoEmpresa.wpIngresoEmpresa
{
    [ToolboxItemAttribute(false)]
    public partial class wpIngresoEmpresa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpIngresoEmpresa()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "MantenedorEmpresas.aspx";
        public Empresa entEmpresa;
        public string strRes = string.Empty;
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            OcultarDiv();
            SPUser oUser;
            txtCelu.Attributes.Add("onblur", "return valCel('" + txtCelu.ClientID + "');");
            btnLimpiar.OnClientClick = "return LimpiarVacido('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
            ((TextBox)(dtcIniAct.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");
            ((TextBox)(dtcIniAct.Controls[0])).MaxLength = 10;

            //PERMISOS USUARIOS
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = string.Empty,
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    dtcIniAct.MaxDate = DateTime.Today.Date;
                    CargarEjecutivo(dt.Rows[0]["descCargo"].ToString(), validar.NombreUsuario);
                    llenarDdlCargos();

                    using (SPWeb oWeb = SPContext.Current.Site.OpenWeb(SPContext.Current.Web.Url))
                    {
                        oUser = SPContext.Current.Web.CurrentUser;
                    }

                    if (Page.Session["Resumen"] != null)
                    {
                        objresumen = (Resumen)Page.Session["Resumen"];
                        ViewState["RES"] = objresumen;
                    }

                    Page.Session["Resumen"] = null;
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

                bloquearCampos();
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
            Page.Response.Redirect("ListarComercial.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ListarComercial.aspx");
        }

        protected void ImageButton2_Click1(object sender, EventArgs e)
        {
            DataTable res;
            LogicaNegocio Ln = new LogicaNegocio();
            res = Ln.buscarDatosRut(txt_rutContacto.Text.Replace(".", "") + txt_divRutContacto.Text);
            if (res != null)
            {
                if (res.Rows.Count > 0)
                {
                    txtNombres.Text = res.Rows[0]["Nombre"].ToString(); //Rows[0]["RazonSocial"].ToString();
                    txtFijo.Text = res.Rows[0]["Telefono"].ToString();
                    txtMail.Text = res.Rows[0]["Email"].ToString();
                    if (!string.IsNullOrEmpty(res.Rows[0]["Cargo"].ToString()))
                        ddlCargos.SelectedValue = System.Web.HttpUtility.HtmlDecode(res.Rows[0]["Cargo"].ToString());

                    txtCelu.Text = res.Rows[0]["Celular"].ToString();
                    ViewState["IdEmpresaR"] = res.Rows[0]["IdEmpresa"].ToString();
                    txtNombres.Enabled = false;
                }
                else
                {
                    txtNombres.Enabled = true;
                    txt_rutContacto.Text = txt_divRutContacto.Text = "";
                    dvError.Style.Add("display", "block");
                    lbError.Text = "RUT no registrado, por favor ingrese el nombre";
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiaControles();
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

        private void bloquearCampos()
        {
            chkDefecto.Enabled = false;
        }

        private void CargarEjecutivo(string cargo, string nombreUsuario)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                ddlEjecutivo.Items.Clear();
                DataTable dt = new DataTable("dt");

                if (cargo == "Sub-Gerente Comercial")
                    dt = Ln.ListarUsuarios(null, null, nombreUsuario);
                else
                    dt = Ln.ListarUsuarios(null, 1, "");

                ddlEjecutivo.DataSource = dt;
                ddlEjecutivo.DataValueField = "idUsuario";
                ddlEjecutivo.DataTextField = "nombreApellido";
                ddlEjecutivo.DataBind();

                if (cargo != "Sub-Gerente Comercial")
                {
                    ListItem LstItem;
                    LstItem = new ListItem("Seleccione", "0");
                    ddlEjecutivo.Items.Insert(0, LstItem);
                }

            }
            catch (Exception ex)
            {
                lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.EMPRESAERRORUSUARIO);
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void Grabar()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                entEmpresa = new Empresa();
                entEmpresa = CrearEmpresa;

                if (ViewState["Asistente"] != null && ViewState["Asistente"].ToString() != "1")
                    strRes = new Empresa { }.IngresaActualizaEmpresa(entEmpresa, "01", ddlEjecutivo.SelectedItem.Text, int.Parse(ddlEjecutivo.SelectedValue), int.Parse(ddlEjecutivo.SelectedValue), "");
                else
                    strRes = new Empresa { }.IngresaActualizaEmpresa(entEmpresa, "01", ddlEjecutivo.SelectedItem.Text, int.Parse(ddlEjecutivo.SelectedValue), int.Parse(ddlEjecutivo.SelectedValue), ddlEjecutivo.SelectedItem.Text);

                if (strRes.Contains("-"))
                {
                    lbError.Text = strRes;
                    dvError.Style.Add("display", "block");
                }
                else
                {
                    entEmpresa._idEmpresa = Convert.ToInt32(strRes);
                    int valContacto = 1;


                    strRes = Ln.InsertaActualizaContactoEmpresa("", "", txtNombres.Text, txtMail.Text, txtFijo.Text, txtCelu.Text,
                                                                        int.Parse(ddlCargos.SelectedItem.Value), ddlCargos.SelectedItem.Text, "01", 0, "", 0, "", 0, "",
                                                                        true, true, int.Parse(entEmpresa._idEmpresa.ToString()), Convert.ToInt32(txt_rutContacto.Text), txt_divRutContacto.Text, valContacto, "Administrador Total");
                    if (strRes.Contains("-") == false)
                    {
                        lbExito.Text = Ln.buscarMensaje(5) + " " + txtRazon.Text;
                        dvExito.Style.Add("display", "block");
                        limpiaControles();
                        Page.Response.Redirect("ListarComercial.aspx");
                    }
                    else
                    {
                        lbExito.Text = Ln.buscarMensaje(4);
                        dvExito.Style.Add("display", "block");
                    }
                }

                strRes = string.Empty;
            }
            catch (Exception ex)
            {
                lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.EMPRESAERRORGUARDAR);
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private Empresa CrearEmpresa
        {
            get
            {
                Empresa value = new Empresa();
                if (txtRut.Text == "")
                    value._rut = 0;
                else
                    value._rut = Convert.ToInt32(txtRut.Text.Trim().Replace(".", ""));
                value._dvRut = txtDV.Text.Trim();
                value._razonSocial = txtRazon.Text.Trim();
                value._idEjecutivo = Convert.ToInt32(ddlEjecutivo.SelectedItem.Value);
                value._fecIniAct = dtcIniAct.SelectedDate;
                if (ddlEjecutivo.Items.Count == 0)
                    value._descEjecutivo = "";
                else
                    value._descEjecutivo = ddlEjecutivo.SelectedItem.Text.Trim();

                 
                return value;
            }
        }

        public void OcultarDiv()
        {
            dvError.Style.Add("display", "none");
            dvExito.Style.Add("display", "none");
            dvInfo.Style.Add("display", "none");
            dvWarning.Style.Add("display", "none");
            dvCustom.Style.Add("display", "none");
        }

        private void llenarDdlCargos()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtCargoPersonas = Ln.ListarCargosPersonas();
            util.CargaDDL(ddlCargos, dtCargoPersonas, "Nombre", "Id");
        }
        
        //private SPListItemCollection ConsultaCargosPersonas()
       // {
            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;
            //SPList Lista = app.Lists["CargosPersonas"];
            //SPQuery oQuery = new SPQuery();
            //oQuery.Query = "<Where>" +
            //    "<Or>" +
            //     "<Eq><FieldRef Name='GrupoPersona'/><Value Type='Number'>1</Value></Eq>" +
            //     "<Eq><FieldRef Name='GrupoPersona'/><Value Type='Number'>2</Value></Eq>" +
            //     "</Or>" +
            //    "</Where>" +
            //    "<OrderBy>" + "<FieldRef Name='Title' Ascending='True' />" + "</OrderBy>";
            //SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            //return ColecLista;
        //}

        protected void limpiaControles()
        {
            txtRut.Text = "";
            txtDV.Text = "";
            txtNombres.Text = "";
            dtcIniAct.ClearSelection();
            txtMail.Text = "";
            txtFijo.Text = "";
            txtCelu.Text = "";
            txt_rutContacto.Text = "";
            txt_divRutContacto.Text = "";
            txtRazon.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            //VALIDAR ALERTAS DE INGRESO DE OPERACION
            entEmpresa = new Empresa();
            entEmpresa = CrearEmpresa;
            string mensaje = new Empresa { }.ValidarDatosEmpresa(entEmpresa, txt_rutContacto.Text, txt_divRutContacto.Text, txtNombres.Text, txtFijo.Text, txtMail.Text, ddlCargos.SelectedItem.Text, txtCelu.Text);
            if (mensaje == "OK")
                Grabar();
            else
            {
                mensaje = util.Mensaje(mensaje);
                lbError.Text = mensaje;
                dvError.Style.Add("display", "block");
            }
        }

        #endregion

    }
}
