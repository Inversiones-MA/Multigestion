using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using MultigestionUtilidades;
using ClasesNegocio;
using Bd;

namespace MultiOperacion.wpCartera.wpCartera
{
    [ToolboxItemAttribute(false)]
    public partial class wpCartera : WebPart
    {
        private static string pagina = "Cartera.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpCartera()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int pos;
        public static int pageSize = 100;
        public static int pageNro = 0;

        public static string USER;
        public static string buscarNCERT = string.Empty;
        public static string linkPrincipal = string.Empty;
        public static DataTable OPCIONESPERMISOS = new DataTable();
        Utilidades util = new Utilidades();


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();

            //LogicaNegocio Ln = new LogicaNegocio();
            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    buscarNCERT = Page.Request.QueryString["NCertificado"] as string;

                    if (buscarNCERT != "")
                        txtNCertificado.Text = buscarNCERT;

                    CargarAcreedor();
                    CargarFondos();
                    CargarEstadoCertificado();
                    CargarEjecutivo();
                    USER = app2.CurrentUser.Name;

                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString().Trim();
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString().Trim();
                    if (Page.Session["BUSQUEDA"] != null)
                    {
                        Busqueda objBusqueda = new Busqueda();
                        objBusqueda = (Busqueda)Page.Session["BUSQUEDA"];
                        ddlAcreedor.SelectedIndex = ddlAcreedor.Items.IndexOf(ddlAcreedor.Items.FindByValue(Convert.ToString(objBusqueda.idAcreedor)));
                        ddlEdoCertificado.SelectedIndex = ddlEdoCertificado.Items.IndexOf(ddlEdoCertificado.Items.FindByValue(Convert.ToString(objBusqueda.idEdoCertificado)));
                        ddlEjecutivo.SelectedIndex = ddlEjecutivo.Items.IndexOf(ddlEjecutivo.Items.FindByValue(Convert.ToString(objBusqueda.Otro)));

                        txtRUT.Text = objBusqueda.Rut;
                        txtRazonSocial.Text = objBusqueda.RazonSocial;
                        txtNCertificado.Text = objBusqueda.nCertificado;
                        Page.Session["BUSQUEDA"] = null;
                    }
                    CargarGrid();
                }
                else
                {
                    CargarGrid();
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;
                //Control divFiltros = this.FindControl("filtros");
                //Control divGrilla = this.FindControl("grilla");

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrid();
        }

        protected void lbCalendarioPago_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("CalendarioPago.aspx");
        }

        protected void lbSeguimiento_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Cartera.aspx");
        }

        protected void gridSeguimiento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //  if (e.Row.Cells[7].Text != "&nbsp;" && int.Parse(e.Row.Cells[7].Text) < 0)
                //      e.Row.Cells[7].Text = "";
                //}

                DataTable dt = new DataTable();
                dt = OPCIONESPERMISOS;
                if (e.Row.RowIndex > -1)
                {
                    LinkButton lbk = new LinkButton();
                    lbk.CommandName = "Redireccion";
                    lbk.ToolTip = "Calendario Pago";
                    lbk.CssClass = "glyphicon glyphicon-calendar paddingIconos";
                    lbk.CommandArgument = (pos + (gridSeguimiento.PageIndex * 20)).ToString();
                    lbk.OnClientClick = "return Dialogo();";
                    lbk.Command += gridSeguimiento_Command1;
                    e.Row.Cells[10].Controls.Add(lbk);


                    if (e.Row.Cells[6].Text == "Vigente")
                    {
                        LinkButton lbk1 = new LinkButton();
                        lbk1.CommandName = "Prepagar";
                        lbk1.ToolTip = "Prepago - Devolución de Comisión";
                        lbk1.CssClass = "glyphicon glyphicon-refresh paddingIconos";
                        lbk1.CommandArgument = (pos + (gridSeguimiento.PageIndex * 20)).ToString();
                        lbk1.OnClientClick = "return Dialogo();";
                        lbk1.Command += gridSeguimiento_Command1;
                        e.Row.Cells[10].Controls.Add(lbk1);
                    }

                    LinkButton[] lbmenu = new LinkButton[dt.Rows.Count];
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        lbmenu[i] = new LinkButton();
                        lbmenu[i].CommandName = dt.Rows[i]["Opcion"].ToString();
                        lbmenu[i].CssClass = (dt.Rows[i]["Imagen"].ToString());
                        lbmenu[i].CommandArgument = pos.ToString(); //(pos + (gridSeguimiento.PageIndex * 20)).ToString();
                        lbmenu[i].OnClientClick = "return Dialogo();";
                        lbmenu[i].Command += gridSeguimiento_Command1;

                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

                        if (int.Parse(dt.Rows[0]["nroColum"].ToString()) == int.Parse(dt.Rows[i]["nroColum"].ToString()))
                        { e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]); }

                        if (int.Parse(dt.Rows[i]["nroColum"].ToString()) > int.Parse(dt.Rows[0]["nroColum"].ToString()) && e.Row.Cells[0].Text.ToString() != "&nbsp;")
                        {
                            int sal;
                            if (int.TryParse(dt.Rows[i]["nroColum"].ToString(), out sal))
                                e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]);
                        }
                    }
                    pos++;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "cartera", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        protected void gridSeguimiento_Command1(object sender, CommandEventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Busqueda objBusqueda = new Busqueda();
            objBusqueda.idAcreedor = int.Parse(ddlAcreedor.SelectedValue);
            objBusqueda.Acreedor = ddlAcreedor.SelectedValue.ToString();
            objBusqueda.idEdoCertificado = int.Parse(ddlEdoCertificado.SelectedValue);
            objBusqueda.EdoCertificado = ddlEdoCertificado.SelectedValue.ToString();
            objBusqueda.Rut = txtRUT.Text.ToString();
            objBusqueda.RazonSocial = txtRazonSocial.Text.ToString();
            objBusqueda.nCertificado = txtNCertificado.Text.ToString();

            objBusqueda.Otro = (ddlEjecutivo.SelectedValue);
            Page.Session["BUSQUEDA"] = objBusqueda;

            Resumen resumenP = new Resumen();
            //DataSet res = new DataSet();
            int index = Convert.ToInt32(e.CommandArgument);
            //res = Ln.ListarDatosCartera(ddlAcreedor.SelectedValue, ddlEdoCertificado.SelectedValue, ddlEjecutivo.SelectedItem.ToString(), USER, ViewState["CARGO"].ToString(), txtNCertificado.Text, txtRUT.Text.Replace(".", ""), txtRazonSocial.Text, ViewState["AREA"].ToString(), ddlFondo.SelectedValue);

            DataTable dt = new DataTable();
            dt = OPCIONESPERMISOS;

            //int index = Convert.ToInt32(e.CommandArgument);
            //DataSet res = new DataSet();
            //res = MTO.ListarDatosCartera(ddlAcreedor.SelectedValue, ddlEdoCertificado.SelectedValue, ddlEjecutivo.SelectedItem.ToString(), USER, ViewState["CARGO"].ToString(), txtNCertificado.Text, txtRUT.Text.Replace(".", ""), txtRazonSocial.Text, ViewState["AREA"].ToString(), ddlFondo.SelectedValue);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if ("Redireccion" == e.CommandName)
                    Page.Response.Redirect("CalendarioPago.aspx?NCertificado=" + gridSeguimiento.Rows[index].Cells[0].Text.ToString());

                if ("Prepagar" == e.CommandName)
                {
                    //DataSet res1 = new DataSet();
                    //res1 = MTO.ListarDatosCartera(ddlAcreedor.SelectedValue, ddlEdoCertificado.SelectedValue, ddlEjecutivo.SelectedItem.ToString(), USER, ViewState["CARGO"].ToString(), txtNCertificado.Text, txtRUT.Text.Replace(".", ""), txtRazonSocial.Text, ViewState["AREA"].ToString(), ddlFondo.SelectedValue);
                    //int index1 = Convert.ToInt32(e.CommandArgument);
                    int.Parse(gridSeguimiento.Rows[index].Cells[0].Text.ToString());

                    resumenP.idOperacion = int.Parse(gridSeguimiento.Rows[index].Cells[16].Text.ToString());
                    resumenP.idEmpresa = int.Parse(gridSeguimiento.Rows[index].Cells[17].Text.ToString());
                    resumenP.desOperacion = gridSeguimiento.Rows[index].Cells[21].Text.ToString();
                    resumenP.desEmpresa = gridSeguimiento.Rows[index].Cells[20].Text.ToString();
                    resumenP.rut = gridSeguimiento.Rows[index].Cells[18].Text.ToString() + '-' + gridSeguimiento.Rows[index].Cells[19].Text.ToString();
                    resumenP.descEjecutivo = gridSeguimiento.Rows[index].Cells[35].Text.ToString();
                    resumenP.desEstado = gridSeguimiento.Rows[index].Cells[13].Text.ToString();
                    resumenP.desEtapa = gridSeguimiento.Rows[index].Cells[11].Text.ToString();
                    resumenP.desSubEtapa = gridSeguimiento.Rows[index].Cells[12].Text.ToString();
                    resumenP.area = ViewState["AREA"].ToString();
                    resumenP.linkActual = "Cartera.aspx";
                    resumenP.linkPrincial = "Cartera.aspx";
                    resumenP.linkError = "Mensaje.aspx";
                    resumenP.idPermiso = "1";
                    resumenP.desPermiso = "Editar";
                    resumenP.descCargo = "Seguimiento";

                    SPWeb app2 = SPContext.Current.Web;
                    resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);

                    Page.Session["RESUMEN"] = resumenP;

                    Page.Response.Redirect("devoluciones.aspx?NCertificado=" + gridSeguimiento.Rows[index].Cells[0].Text.ToString());
                }

                if (dt.Rows[i]["Opcion"].ToString() == e.CommandName)
                {
                    try
                    {
                        resumenP.idOperacion = int.Parse(gridSeguimiento.Rows[index].Cells[16].Text.ToString());
                        resumenP.idEmpresa = int.Parse(gridSeguimiento.Rows[index].Cells[17].Text.ToString());
                        resumenP.desOperacion = gridSeguimiento.Rows[index].Cells[2].Text.ToString();
                        resumenP.desEmpresa = gridSeguimiento.Rows[index].Cells[20].Text.ToString();
                        resumenP.rut = gridSeguimiento.Rows[index].Cells[18].Text.ToString() + '-' + gridSeguimiento.Rows[index].Cells[19].Text.ToString();
                        resumenP.descEjecutivo = gridSeguimiento.Rows[index].Cells[35].Text.ToString();
                        resumenP.desEstado = gridSeguimiento.Rows[index].Cells[13].Text.ToString();
                        resumenP.desEtapa = gridSeguimiento.Rows[index].Cells[11].Text.ToString();
                        resumenP.desSubEtapa = gridSeguimiento.Rows[index].Cells[12].Text.ToString();
                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "Cartera.aspx";
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        try
                        {
                            resumenP.fecInicioActEconomica = DateTime.Parse(gridSeguimiento.Rows[index].Cells[15].Text.ToString());
                        }
                        catch (Exception ex)
                        {
                            resumenP.fecInicioActEconomica = DateTime.Now;
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                        Page.Response.Redirect("Mensaje.aspx");
                    }

                    if ("Documentos" == e.CommandName)
                        Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(gridSeguimiento.Rows[index].Cells[20].Text.ToString())));
                    else
                        Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                }
            }
        }

        protected void gridSeguimiento_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false; // 
                e.Row.Cells[13].Visible = false;//val 
                e.Row.Cells[14].Visible = false; //val 
                e.Row.Cells[15].Visible = false; //
                e.Row.Cells[16].Visible = false; //
                e.Row.Cells[17].Visible = false; //
                e.Row.Cells[18].Visible = false; //
                e.Row.Cells[19].Visible = false; //
                e.Row.Cells[20].Visible = false; // 
                e.Row.Cells[21].Visible = false;// 
                e.Row.Cells[22].Visible = false; // 
                e.Row.Cells[23].Visible = false; //
                e.Row.Cells[24].Visible = false; //
                e.Row.Cells[25].Visible = false; //
                e.Row.Cells[26].Visible = false; //
                e.Row.Cells[27].Visible = false; //
                e.Row.Cells[28].Visible = false; // 
                e.Row.Cells[29].Visible = false;// 
                e.Row.Cells[30].Visible = false; // 
                e.Row.Cells[31].Visible = false; //
                e.Row.Cells[32].Visible = false; // 
                e.Row.Cells[33].Visible = false;// 
                e.Row.Cells[34].Visible = false; // 
                e.Row.Cells[35].Visible = false; //duenojecutivo
            }
        }

        protected void btnAtras_Click1(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = null;
            Page.Response.Redirect("ListarOperaciones.aspx");
        }

        protected void gridSeguimiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSeguimiento.PageIndex = e.NewPageIndex;
            CargarGrid();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ListarOperaciones.aspx");
        }

        #endregion


        #region Metodos

        protected void CargarGrid()
        {
            try
            {
                pos = 0;

                LogicaNegocio MTO = new LogicaNegocio();
                DataSet res = new DataSet();
                res = MTO.ListarDatosCartera(ddlAcreedor.SelectedValue, ddlEdoCertificado.SelectedValue, ddlEjecutivo.SelectedItem.ToString(), USER, ViewState["CARGO"].ToString(), txtNCertificado.Text, txtRUT.Text.Replace(".", ""), txtRazonSocial.Text, ViewState["AREA"].ToString(), ddlFondo.SelectedValue);
                OPCIONESPERMISOS = res.Tables[0];
                int con = 0;
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
                                if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
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
                            if (i == 0)
                            {
                                con++;
                                res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                            }
                            else if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                            {
                                //con++;
                                res.Tables[0].Rows[i]["nroColum"] = res.Tables[0].Rows[i - 1]["nroColum"];
                                //res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                            }
                            else
                            {
                                con++;
                                res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                            }

                            if (con > 0)
                            {
                                res.Tables[1].Columns.Add(res.Tables[0].Rows[i]["Descripcion"].ToString().Trim(), typeof(string));
                                con = 0;
                            }
                        }
                    }

                    if (res.Tables[1].Rows.Count > 0)
                    {
                        gridSeguimiento.Visible = true;
                        ocultarDiv();
                        gridSeguimiento.DataSource = res.Tables[1];
                        gridSeguimiento.DataBind();
                    }
                    else
                    {
                        gridSeguimiento.Visible = false;
                        ocultarDiv();
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros";
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

        private void CargarAcreedor()
        {
            SPWeb app = SPContext.Current.Web;
            SPList Lista = app.Lists["Acreedores"];
            app.AllowUnsafeUpdates = true;
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<OrderBy>  <FieldRef Name = 'Nombre' Ascending = 'TRUE'/> </OrderBy>";
            SPListItemCollection items = Lista.GetItems(oQuery);

            util.CargaDDL(ddlAcreedor, items.GetDataTable(), "Nombre", "ID");
        }


        private void CargarFondos()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["Fondos"];

            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='Visible'/><Value Type='Integer'>1</Value></Eq></Where>";
            SPListItemCollection collListItems = items.GetItems(query);
            util.CargaDDL(ddlFondo, collListItems.GetDataTable(), "Nombre", "ID");
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void CargarEjecutivo()
        {
            ddlEjecutivo.Items.Clear();
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 1, "");
                util.CargaDDL(ddlEjecutivo, dt, "nombreApellido", "idUsuario");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "cartera", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarEstadoCertificado()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPListItemCollection items = app.Lists["EstadoCertificado"].Items;
            util.CargaDDL(ddlEdoCertificado, items.GetDataTable(), "Nombre", "ID");
        }



        #endregion


    }
}
