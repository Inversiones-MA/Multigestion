using System.Data;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Web.UI;
using System.Web;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpListarComercial.wpListarComercial
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarComercial : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarComercial()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private readonly string pagina = "ListarComercial.aspx";
        int pos;
        int Val = 0;
        public static int pageSize = 10000;
        public static int pageNro = 0;
        public static DataTable OPCIONESPERMISOS;
        Utilidades util = new Utilidades();
        public static string ESTADO = "-1";
        public static string SUBETAPA = "-1";
        public static string ETAPA = "-1";
        public SPListItemCollection ListaEdicion;

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            LogicaNegocio Ln = new LogicaNegocio();

            ////PERMISOS USUARIOS
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = "",
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                ListaEdicion = Ln.ConsultaListaEdicionUsuario();
                if (!Page.IsPostBack)
                {
                    cb_estados.Attributes["onChange"] = "Dialogo();";
                    cb_etapa.Attributes["onChange"] = "Dialogo();";
                    cb_subetapa.Attributes["onChange"] = "Dialogo();";

                    ViewState["USER"] = app2.CurrentUser.Name;
                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString(); //"Administrador"; //
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString(); //"Comercial";

                    if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()))
                    {

                        CargarEstados();
                        CargarEtapas();
                        CargarSubEtapas();
                        ESTADO = "-1";
                        SUBETAPA = "-1";
                        ETAPA = "-1";
                        OPCIONESPERMISOS = null;

                        if (Page.Session["BUSQUEDA"] != null)
                        {
                            Busqueda objBusqueda = new Busqueda();
                            objBusqueda = (Busqueda)Page.Session["BUSQUEDA"];
                            if (objBusqueda.idEstado != -1)
                            {
                                ESTADO = objBusqueda.idEstado.ToString();
                                cb_estados.SelectedIndex = cb_estados.Items.IndexOf(cb_estados.Items.FindByValue(Convert.ToString(objBusqueda.idEstado)));
                            }
                            if (objBusqueda.idEtapa != -1)
                            {
                                ETAPA = objBusqueda.idEtapa.ToString();
                                cb_etapa.SelectedIndex = cb_etapa.Items.IndexOf(cb_etapa.Items.FindByValue(Convert.ToString(objBusqueda.idEtapa)));
                                if (objBusqueda.idSubEtapa != -1)
                                {
                                    SUBETAPA = objBusqueda.idSubEtapa.ToString();
                                    //Ln.CargarListaSubEtapa(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA, cb_etapa.SelectedValue.ToString());
                                    CargarSubEtapas();
                                    cb_subetapa.SelectedIndex = cb_subetapa.Items.IndexOf(cb_subetapa.Items.FindByValue(Convert.ToString(objBusqueda.idSubEtapa)));
                                }
                            }
                            txtBuscar.Text = objBusqueda.RazonSocial;
                            Page.Session["BUSQUEDA"] = null;
                        }

                        //CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                    }
                }
                CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                warning("Usuario sin permisos configurados");
            }

            //dvWarning.DivAlerta();
            //lbWarning.MensajeAlerta("alalalala");
        }

        private void CargarEstados()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEstados = Ln.ListarEstadosOperacion();
            util.CargaDDL(cb_estados, dtEstados, "Nombre", "Id");
        }
        private void CargarEtapas()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEtapas = Ln.ListarEtapas(null);
            util.CargaDDL(cb_etapa, dtEtapas, "Descripcion", "Id");
        }

        private void CargarSubEtapas()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtSubEtapas = Ln.ListarSubEtapas(int.Parse(cb_etapa.SelectedValue.ToString()), false, 1);
            util.CargaDDL(cb_subetapa, dtSubEtapas, "Nombre", "Id");
        }
        protected void cb_estados_SelectedIndexChanged(object sender, EventArgs e)
        {
            ESTADO = cb_estados.SelectedValue.ToString();
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void cb_etapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarSubEtapas();
            ETAPA = cb_etapa.SelectedValue.ToString();
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);

        }
        protected void cb_subetapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            SUBETAPA = cb_subetapa.SelectedValue.ToString();
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Val = 0;
            try
            {
                DataTable dt = new DataTable();
                dt = OPCIONESPERMISOS;
                if (e.Row.RowIndex > -1)
                {
                    LinkButton[] lbmenu = new LinkButton[dt.Rows.Count];
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        lbmenu[i] = new LinkButton();
                        lbmenu[i].CommandName = dt.Rows[i]["Opcion"].ToString();
                        lbmenu[i].CssClass = (dt.Rows[i]["Imagen"].ToString());
                        lbmenu[i].CommandArgument = pos.ToString();
                        lbmenu[i].OnClientClick = "return Dialogo();";
                        lbmenu[i].Command += ResultadosBusqueda_Command1;

                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

                        if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        {
                            lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                            lbmenu[i].Visible = true;
                        }

                        if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) != "Negociacion" &&
                          System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text.ToString()) != "Antecedente y Evaluación" &&
                           dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        {
                            lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                            lbmenu[i].Visible = true;
                        }

                        if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text.ToString()) == "Aprobación Fiscalía")
                        {
                            if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                            {
                                lbmenu[i].CssClass = (dt.Rows[i]["Imagen"].ToString());
                                lbmenu[i].Visible = false;
                            }
                            if ((System.Web.HttpUtility.HtmlDecode((e.Row.Cells[15].Text.ToString())) == "2") && dt.Rows[i]["Opcion"].ToString() == "Negocio")
                            { lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString()); Val++; }

                            if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                            {
                                lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                                lbmenu[i].Visible = true;
                            }
                        }

                        // si todos los iconos estan en verde, entonces el enviar se habilita
                        if ((dt.Rows[i]["Opcion"].ToString() == "PAF" && System.Web.HttpUtility.HtmlDecode(e.Row.Cells[23].Text.ToString()) == " "))
                        {
                            //		System.Web.HttpUtility.HtmlDecode(e.Row.Cells[23].Text.ToString())	" "	string
                            lbmenu[i].Visible = false;
                        }


                        if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR" && ViewState["CARGO"].ToString() == "Ejecutivo Comercial Consulta")
                        {
                            lbmenu[i].Visible = false;
                        }

                        if (dt.Rows[i]["Opcion"].ToString() == "RechazoComercial" && ViewState["CARGO"].ToString() == "Ejecutivo Comercial Consulta")
                        {
                            lbmenu[i].Visible = false;
                        }

                        // opciones para operaciones y empresa
                        try
                        {
                            if (int.Parse(dt.Rows[0]["nroColum"].ToString()) == int.Parse(dt.Rows[i]["nroColum"].ToString()))
                            { e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]); }

                            if (int.Parse(dt.Rows[i]["nroColum"].ToString()) > int.Parse(dt.Rows[0]["nroColum"].ToString()) && int.Parse(e.Row.Cells[0].Text.ToString()) > 0)
                            { e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]); }
                        }
                        catch
                        {
                        }

                    }

                    if (ViewState["AREA"].ToString() == "Comercial")
                    {
                        LinkButton lb = new LinkButton();

                        lb.ToolTip = "Agregar Nuevo Negocio";
                        lb.CommandName = "AddNegocio";
                        lb.CssClass = ("glyphicon glyphicon-plus-sign");
                        lb.CommandArgument = pos.ToString();
                        lb.Attributes.Add("data-toggle", "tooltip");
                        lb.Attributes.Add("data-placement", "top");
                        lb.Attributes.Add("data-original-title", "Agregar un negocio");
                        lb.Command += ResultadosBusqueda_Command1;
                        lb.OnClientClick = "return Dialogo();";
                        lb.Attributes.CssStyle.Add("margin-right", "10px");
                        e.Row.Cells[3].Controls.Add(lb);

                        Label lbb = new Label();
                        Label lbb1 = new Label();

                        //////////////////////////
                        if (e.Row.Cells[26].Text.ToString() != "0")
                        {
                            LinkButton lb2 = new LinkButton();

                            lb2.ToolTip = "Modificar Cotización";
                            lb2.CommandName = "AddCotizacion";
                            lb2.CssClass = ("glyphicon glyphicon-briefcase");
                            lb2.CommandArgument = pos.ToString();
                            lb2.Attributes.Add("data-toggle", "tooltip");
                            lb2.Attributes.Add("data-placement", "top");
                            lb2.Attributes.Add("data-original-title", "Modificar Cotización");
                            lb2.Command += ResultadosBusqueda_Command1;
                            lb2.OnClientClick = "return Dialogo();";
                            e.Row.Cells[4].Controls.Add(lb2);
                            lb2.Attributes.CssStyle.Add("margin-right", "10px");
                        }

                        lbb1.Text = "  " + e.Row.Cells[4].Text;
                        e.Row.Cells[4].Controls.Add(lbb1);

                        lbb.Text = "  " + e.Row.Cells[3].Text;
                        e.Row.Cells[3].Controls.Add(lbb);
                    }

                    pos++;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void ResultadosBusqueda_Command1(object sender, CommandEventArgs e)
        {
            Utilidades util = new Utilidades();
            LogicaNegocio Ln = new LogicaNegocio();
            Busqueda objBusqueda = new Busqueda();
            objBusqueda.idEstado = int.Parse(cb_estados.SelectedValue);
            objBusqueda.idEtapa = int.Parse(cb_etapa.SelectedValue);
            objBusqueda.idSubEtapa = int.Parse(cb_subetapa.SelectedValue);
            objBusqueda.RazonSocial = txtBuscar.Text.ToString();

            Page.Session["BUSQUEDA"] = objBusqueda;

            if (ViewState["AREA"] == null || ViewState["CARGO"] == null || ViewState["USER"] == null)
            {
                Page.Response.Redirect("MensajeSession.aspx");
            }

            Boolean ban = true;
            DataTable dt = new DataTable();
            dt = OPCIONESPERMISOS;

            int index = Convert.ToInt32(e.CommandArgument);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Opcion"].ToString() == e.CommandName)
                {
                    try
                    {
                        Resumen resumenP = new Resumen();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text.ToString());
                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[4].Text.ToString();
                        resumenP.desEmpresa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString());
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.ToString()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text.ToString());

                        resumenP.desEstado = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.Trim());
                        resumenP.desEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.Trim());
                        resumenP.idEtapa = ResultadosBusqueda.Rows[index].Cells[27].Text.Trim() == "&nbsp;" ? 0 : Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[27].Text.Trim()));
                        resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.Trim());
                        resumenP.descEjecutivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[25].Text.Trim());

                        resumenP.idSubEtapa = ResultadosBusqueda.Rows[index].Cells[24].Text.Trim() == "&nbsp;" ? 0 : Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[24].Text.Trim()));
                        resumenP.idEstado = ResultadosBusqueda.Rows[index].Cells[28].Text.Trim() == "&nbsp;" ? 0 : Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[28].Text.Trim()));

                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarComercial.aspx"; //
                        resumenP.linkError = "Mensaje.aspx";

                        if ((resumenP.desEtapa != "Prospecto" && resumenP.desEtapa != "Negociacion" && resumenP.desEtapa != "Negociación" && resumenP.desEtapa != "") && "ENVIAR" != e.CommandName && ViewState["CARGO"].ToString() != "Ejecutivo Comercial Gerente")
                        {
                            resumenP.idPermiso = "2";
                        }
                        else
                            resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();

                        if (resumenP.desSubEtapa.Trim() == "Aprobación Fiscalía" && "Negocio" == e.CommandName || resumenP.desSubEtapa.Trim() == "")
                        {
                            resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        }

                        if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text.ToString()).Trim() == "Comercial")
                            resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();

                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        if (ResultadosBusqueda.Rows[index].Cells[12].Text.ToString() == "&nbsp;")
                            resumenP.fecInicioActEconomica = DateTime.Now;
                        else
                            resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[12].Text.ToString());
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        //idcotizacion
                        resumenP.IdCotizacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[26].Text.ToString());
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        ocultarDiv();
                        ban = false;
                        warning("Registro seleccionado tiene inconsistencia en la data. Por favor Consultar al Administrador del Sistema");
                    }


                    if ("ENVIAR" == e.CommandName)
                    {
                        if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString()) == "Comité" && (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString()) == "Aprobada" || System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString()) == "Aprobado") && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.ToString()) == "Ingresado")
                        {
                            if (ban)
                            {
                                Page.Response.Redirect("SolicitudFiscalia.aspx");
                            }
                        }
                        else
                        {
                            if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString()) == "Comité" && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString()) == "Condicionado" && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.ToString()) == "Ingresado")
                            {
                                if (ban)
                                {
                                    Page.Response.Redirect("SolicitudFiscalia.aspx");
                                }
                            }
                            else
                            {
                                if (ban)
                                    Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                            }
                        }
                    }
                    else
                    {
                        if ("Documentos" == e.CommandName)
                        {
                            Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString())));
                        }
                        else
                        {
                            if (ban)
                                Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                        }
                    }
                }
                if (e.CommandName == "AddNegocio")
                {
                    try
                    {
                        Resumen resumenP = new Resumen();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.Trim());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text.Trim());
                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[4].Text.Trim();
                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[2].Text.Trim();
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.Trim()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text.Trim());
                        resumenP.area = resumenP.area = ViewState["AREA"].ToString();

                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarComercial.aspx";
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.descEjecutivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[25].Text.Trim());
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();

                        if (ResultadosBusqueda.Rows[index].Cells[12].Text.ToString() != "&nbsp;")
                            resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[12].Text.Trim());
                        else
                            resumenP.fecInicioActEconomica = DateTime.Now;

                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        ban = false;
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        ocultarDiv();
                        warning("Registro seleccionado tiene inconsistencia en la data. Por favor Consultar al Administrador del Sistema");
                    }
                    if (ban)
                        Page.Response.Redirect("MantenedorOperaciones.aspx");
                }

                if (e.CommandName == "AddCotizacion")
                {
                    try
                    {
                        Resumen resumenP = new Resumen();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.Trim());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text.Trim());
                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[4].Text.Trim();
                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[2].Text.Trim();
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.Trim()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text.Trim());
                        resumenP.area = resumenP.area = ViewState["AREA"].ToString();

                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarComercial.aspx";
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.descEjecutivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[25].Text.Trim());
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.IdCotizacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[26].Text.Trim());
                        resumenP.idEtapa = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[27].Text.Trim()));
                        resumenP.idSubEtapa = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[24].Text.Trim()));
                        resumenP.idEstado = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[28].Text.Trim()));
                        if (ResultadosBusqueda.Rows[index].Cells[12].Text.Trim() != "&nbsp;")
                            resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[12].Text.Trim());
                        else
                            resumenP.fecInicioActEconomica = DateTime.Now;

                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        ban = false;
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        ocultarDiv();
                        warning("Registro seleccionado tiene inconsistencia en la data. Por favor Consultar al Administrador del Sistema");
                    }

                    if (ban)
                        Page.Response.Redirect("Cotizador.aspx");
                }
            }
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[2].Visible = false;//empresa
                e.Row.Cells[1].Visible = false;//IdEmpesa
                e.Row.Cells[0].Visible = false;//idOperacion
                e.Row.Cells[8].Visible = false;//Estado
                //e.Row.Cells[9].Visible = false;//area
                // e.Row.Cells[6].Visible = false;//etapa
                e.Row.Cells[10].Visible = false;//rut
                e.Row.Cells[11].Visible = false;//div rut
                e.Row.Cells[12].Visible = false;//fecha inicio operacion
                e.Row.Cells[13].Visible = false;//fecha de cierre
                e.Row.Cells[14].Visible = false;//val Empresa
                e.Row.Cells[15].Visible = false;//val detalle
                e.Row.Cells[16].Visible = false; //val operacion
                e.Row.Cells[17].Visible = false; //val accionista
                e.Row.Cells[18].Visible = false; //val directorio
                e.Row.Cells[19].Visible = false;//val relacionada
                e.Row.Cells[20].Visible = false; //val garantia
                e.Row.Cells[21].Visible = false; //val contacto
                e.Row.Cells[22].Visible = false; //val documentos
                e.Row.Cells[23].Visible = false; //idpaf
                e.Row.Cells[24].Visible = false; //id  ID sub etapa
                e.Row.Cells[25].Visible = false; //ejecutivo
                e.Row.Cells[26].Visible = false; //idCotizacion
                e.Row.Cells[27].Visible = false; //IdEtapa
                e.Row.Cells[28].Visible = false; //IdEstado
            }
        }

        protected void ResultadosBusqueda_DataBound(object sender, EventArgs e)
        {
            string IdOperacionTemp, IdEmpresaTemp;
            for (int i = 0; i < ResultadosBusqueda.Rows.Count; i++)
            {
                IdOperacionTemp = ResultadosBusqueda.Rows[i].Cells[0].Text;
                IdEmpresaTemp = ResultadosBusqueda.Rows[i].Cells[1].Text;
                foreach (SPListItem item in ListaEdicion)
                {
                    if (item["IdEmpresa"].ToString() == IdEmpresaTemp && item["TipoFormulario"].ToString() == "Empresa")
                    {
                        ResultadosBusqueda.Rows[i].BackColor = Color.FromName("#E0FFFF");
                    }
                    if (item["IdOperacion"].ToString() == IdOperacionTemp && item["TipoFormulario"].ToString() == "Operacion")
                    {
                        ResultadosBusqueda.Rows[i].BackColor = Color.FromName("#FFF5EE");
                    }
                }
            }
            for (int rowIndex = ResultadosBusqueda.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = ResultadosBusqueda.Rows[rowIndex];
                GridViewRow gvPreviousRow = ResultadosBusqueda.Rows[rowIndex + 1];
                for (int cellCount = 3; cellCount < 4; cellCount++)
                {
                    if (gvRow.Cells[cellCount].Text.Trim() == gvPreviousRow.Cells[cellCount].Text.Trim())
                    {
                        if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount].RowSpan = 2;
                            if (ViewState["AREA"].ToString() == "Riesgo")
                                gvRow.Cells[23].RowSpan = 2;
                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                            if (ViewState["AREA"].ToString() == "Riesgo")
                                gvRow.Cells[23].RowSpan = gvPreviousRow.Cells[23].RowSpan + 1;
                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;
                        if (ViewState["AREA"].ToString() == "Riesgo")
                            gvPreviousRow.Cells[23].Visible = false;
                    }
                }
            }
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        #endregion


        #region Metodos

        protected void CargarGrid(string etapa, string subEtapa, string estado, string buscar, int pageS, int pageN)
        {
            try
            {
                pos = 0;
                LogicaNegocio Ln = new LogicaNegocio();
                DataSet res;
                SPWeb app2 = SPContext.Current.Web;

                res = Ln.ListarComercial(etapa, subEtapa, estado, buscar, ViewState["AREA"].ToString(), ViewState["CARGO"].ToString(), ViewState["USER"].ToString(), pageS, pageN);

                OPCIONESPERMISOS = res.Tables[0];
                int con = 0;
                //if (res.Tables[1] != null)
                //{
                //    for (int i = 0; i <= res.Tables[0].Rows.Count - 1; i++)
                //    {
                //        if (i + 1 <= res.Tables[0].Rows.Count - 1)
                //        {
                //            if (i < res.Tables[0].Rows.Count - 2 && res.Tables[0].Rows[i]["Descripcion"].ToString() == res.Tables[0].Rows[i + 1]["Descripcion"].ToString())
                //            {
                //                con++;
                //                res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                //            }
                //            else
                //            {
                //                if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                //                {
                //                    con++;
                //                    res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                //                }
                //                else
                //                {
                //                    if (res.Tables[0].Rows[i]["Descripcion"].ToString() != res.Tables[0].Rows[i - 1]["Descripcion"].ToString())
                //                    {
                //                        con++;
                //                        res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                //                    }
                //                }

                //                if (con > 0)
                //                {
                //                    res.Tables[1].Columns.Add(res.Tables[0].Rows[i]["Descripcion"].ToString().Trim(), typeof(string));
                //                    con = 0;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (i == 0)
                //            {
                //                con++;
                //                res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                //            }
                //            else if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                //            {
                //                res.Tables[0].Rows[i]["nroColum"] = res.Tables[0].Rows[i - 1]["nroColum"];
                //            }
                //            else
                //            {
                //                con++;
                //                res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                //            }

                //            if (con > 0)
                //            {
                //                res.Tables[1].Columns.Add(res.Tables[0].Rows[i]["Descripcion"].ToString().Trim(), typeof(string));
                //                con = 0;
                //            }
                //        }
                //    }

                if (res.Tables[1] != null)
                {
                    for (int i = 0; i <= res.Tables[0].Rows.Count - 1; i++)
                    {
                        if (i + 1 <= res.Tables[0].Rows.Count - 1)
                        {
                            if (i < res.Tables[0].Rows.Count - 2 && res.Tables[0].Rows[i]["Descripcion"].ToString() == res.Tables[0].Rows[i + 1]["Descripcion"].ToString())
                            {
                                con++;
                                res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                            }
                            else
                            {
                                if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                                {
                                    con++;
                                    res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                                }
                                else
                                {
                                    if (res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                                    {
                                        con++;
                                        res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                                    }
                                }

                                if (con > 0)
                                {
                                    res.Tables[1].Columns.Add(res.Tables[0].Rows[i]["Descripcion"].ToString().Trim(), typeof(string));
                                    con = 0;
                                }
                            }
                        }
                        else
                        {
                            if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                            {
                                con++;
                                res.Tables[0].Rows[i]["nroColum"] = res.Tables[0].Rows[i - 1]["nroColum"];
                            }
                        }
                    }

                    if (res.Tables[1].Rows.Count > 0)
                    {
                        ResultadosBusqueda.Visible = true;
                        ocultarDiv();
                        ResultadosBusqueda.DataSource = res.Tables[1];
                        ResultadosBusqueda.DataBind();
                    }
                    else
                    {
                        ResultadosBusqueda.Visible = false;
                        ocultarDiv();
                        warning("No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros");
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        private void warning(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje;
        }

        #endregion

    }
}
