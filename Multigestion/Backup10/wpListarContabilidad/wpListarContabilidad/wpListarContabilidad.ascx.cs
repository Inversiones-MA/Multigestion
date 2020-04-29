using System.Data;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using System.Drawing;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiContabilidad.wpListarContabilidad.wpListarContabilidad
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarContabilidad : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarContabilidad()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int pos;

        public string ESTADO;
        public string SUBETAPA;
        public string ETAPA;
        public static DataTable OPCIONESPERMISOS;
        public static int pageSize = 1000;
        public static int pageNro = 0;
        public SPListItemCollection ListaEdicion;
        public DataTable ListaUsuariosContabilidad = new DataTable("dt");
        Utilidades util = new Utilidades();
        private static string pagina = "ListarContabilidad.aspx";


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            Boolean ban = false;

            LogicaNegocio MTO = new LogicaNegocio();
            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                ListaEdicion = ConsultaListaEdicionUsuario();
                ListaUsuariosContabilidad = ConsultarUsuariosContabilidad();

                if (!Page.IsPostBack)
                {
                    ViewState["USER"] = util.ObtenerValor(app2.CurrentUser.Name);
                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString();
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString();
                    CargarListas();
                    ban = true;

                    if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()) && ban == true)
                    {
                        ESTADO = "";
                        SUBETAPA = "";
                        ETAPA = "";
                        OPCIONESPERMISOS = null;
                        CargarGrid("", "", "", string.Empty, pageSize, pageNro);
                    }
                }
                else
                {
                    CargarGrid("", "", "", txtBuscar.Text.ToString(), pageSize, pageNro);
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
                //BloquearControl(true, dt.Rows[0]["Permiso"].ToString());
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }


        int val;
        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            val = 0;
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)OPCIONESPERMISOS;
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

                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());
                        lbmenu[i].Command += ResultadosBusqueda_Command1;

                        if (val > 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        {
                            lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                            lbmenu[i].Visible = true;
                        }

                        if (val <= 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                            lbmenu[i].Visible = false;

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
                    pos++;

                    //Añadir DDL
                    DropDownList ddlUsuariosContabilidad = new DropDownList();
                    //String usuario, id;         
                    ddlUsuariosContabilidad.Items.Clear();

                    ddlUsuariosContabilidad.DataSource = ListaUsuariosContabilidad;
                    ddlUsuariosContabilidad.DataValueField = "idUsuario";
                    ddlUsuariosContabilidad.DataTextField = "nombreApellido";
                    ddlUsuariosContabilidad.DataBind();

                    ListItem li = new ListItem("Sin Asignar", "0");
                    ddlUsuariosContabilidad.Items.Insert(0, li);

                    ddlUsuariosContabilidad.SelectedIndexChanged += ddlUsuariosContabilidad_SelectedIndexChanged;
                    ddlUsuariosContabilidad.AutoPostBack = true;
                    ddlUsuariosContabilidad.ClearSelection();

                    if (string.IsNullOrWhiteSpace(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[31].Text)) || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[31].Text) == "Sin Asignar")
                        ddlUsuariosContabilidad.SelectedValue = "0";
                    else
                    {
                        ListItem itemAux = ddlUsuariosContabilidad.Items.FindByText(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[31].Text));
                        if (itemAux != null)
                            itemAux.Selected = true;
                    }
                    e.Row.Cells[31].Controls.Add(ddlUsuariosContabilidad);

                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        protected void ResultadosBusqueda_Command1(object sender, CommandEventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean ban = true;
            string TipoTransaccion = "-1";
            string idTipoTransaccion = "-1";
            int index = Convert.ToInt32(e.CommandArgument);
            DataTable dt = new DataTable();
            dt = (DataTable)OPCIONESPERMISOS;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Opcion"].ToString() == e.CommandName)
                {
                    try
                    {
                        Resumen resumenP = new Resumen();
                        if (ResultadosBusqueda.Rows[index].Cells[23].Text.ToString() != "&nbsp;")
                            resumenP.idPAF = int.Parse(ResultadosBusqueda.Rows[index].Cells[23].Text);

                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text);

                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[2].Text.ToString();
                        resumenP.descEjecutivo = ResultadosBusqueda.Rows[index].Cells[25].Text.ToString();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());
                        resumenP.desOperacion = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text.ToString());
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.ToString());
                        TipoTransaccion = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[26].Text.ToString());
                        idTipoTransaccion = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[29].Text.ToString());
                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarContabilidad.aspx"; //
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();//IdPermiso
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[32].Text.ToString());
                        //resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[12].Text.ToString());
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["RESUMEN"] = resumenP;

                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        ocultarDiv();
                        ban = false;
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "Registro seleccionado tiene inconsistencia en la data. Por favor Consultar al Administrador del Sistema";
                    }

                    if ("Documentos" == e.CommandName)
                    {
                        if (ban)
                            Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString())));
                    }
                    else
                        if (ban)
                        {

                            if ("Facturar" == e.CommandName || "Pagar" == e.CommandName)
                                Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?idTT=" + idTipoTransaccion);
                            else
                                Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                        }
                }

            }
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                e.Row.Cells[1].Visible = false;//empresa
                e.Row.Cells[0].Visible = false;//IdEmpesa

                e.Row.Cells[2].Visible = false;//empresa
                e.Row.Cells[4].Visible = false;//rut

                e.Row.Cells[5].Visible = false;//etapa
                e.Row.Cells[6].Visible = false;//sub etapa
                e.Row.Cells[7].Visible = false;//estado
                e.Row.Cells[8].Visible = false;//area
                e.Row.Cells[9].Visible = false;//Empresa

                e.Row.Cells[11].Visible = false; //rut div
                e.Row.Cells[12].Visible = false; //val empresa
                e.Row.Cells[13].Visible = false;//val empresa detalle
                e.Row.Cells[14].Visible = false; //val emp accionnista
                e.Row.Cells[15].Visible = false; //val emp directorio
                e.Row.Cells[16].Visible = false; //val emp relacionada
                e.Row.Cells[17].Visible = false; //cal contacto

                e.Row.Cells[18].Visible = false; //val garantia
                e.Row.Cells[19].Visible = false; //val documento
                e.Row.Cells[20].Visible = false; //garantia

                e.Row.Cells[21].Visible = false; //comision
                e.Row.Cells[22].Visible = false; //plazo
                e.Row.Cells[23].Visible = false; //paf
                e.Row.Cells[24].Visible = false; //val ooperacion
                e.Row.Cells[25].Visible = false; //val dueño
                e.Row.Cells[29].Visible = false; //val dueño

            }
        }

        protected void ResultadosBusqueda_DataBound(object sender, EventArgs e)
        {
            String IdOperacionTemp, IdEmpresaTemp;
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
                    if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                    {
                        if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount].RowSpan = 2;

                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;

                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;

                    }
                }


                for (int cellCount = 10; cellCount < 11; cellCount++)
                {
                    if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                    {
                        if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                        {
                            gvRow.Cells[cellCount].RowSpan = 2;

                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;

                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;

                    }
                }
            }
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            // ESTADO = cb_estados.SelectedValue.ToString();
            CargarGrid("", "", "", txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrid("", "", "", txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrid("", "", "", txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        #endregion


        #region Metodos

        protected void CargarGrid(string etapa, string subEtapa, string estado, string buscar, int pageS, int pageN)
        {
            try
            {
                pos = 0;
                LogicaNegocio MTO = new LogicaNegocio();
                DataSet res;

                string fechai = string.Empty;
                string fechaf = string.Empty;
                if (!dtcFechaInicio.IsDateEmpty)
                    fechai = dtcFechaInicio.SelectedDate.Day.ToString() + "-" + dtcFechaInicio.SelectedDate.Month.ToString() + "-" + dtcFechaInicio.SelectedDate.Year.ToString();
                if (!dtcFechaFin.IsDateEmpty)
                    fechaf = dtcFechaFin.SelectedDate.Day.ToString() + "-" + dtcFechaFin.SelectedDate.Month.ToString() + "-" + dtcFechaFin.SelectedDate.Year.ToString();

                res = MTO.ListarContabilidad(ViewState["CARGO"].ToString(), ViewState["AREA"].ToString(), ViewState["USER"].ToString(), ddlTipoTransaccion.SelectedValue, int.Parse(ddlEstado.SelectedValue), txtNroTransaccion.Text, txtRUT.Text.ToString().Replace(".", "").Replace("-", "").Replace(" ", ""), txtBuscar.Text, txtNCertificado.Text, fechai, fechaf, pageS, pageN);
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
                                if (i <= res.Tables[0].Rows.Count - 1 || res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
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
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros";
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        private void ddlUsuariosContabilidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int indice = row.RowIndex;
            LogicaNegocio lcGlobal = new LogicaNegocio();
            lcGlobal.GestionResponsableOperacion(int.Parse(ResultadosBusqueda.Rows[indice].Cells[0].Text.ToString()), ddl.SelectedItem.Text, ViewState["CARGO"].ToString(), "04");
        }

        private void CargarListas()
        {
            CargarConceptoContabilidad();
            CargarEstadoContabilidad();
        }

        private void CargarConceptoContabilidad()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList items = app.Lists["TipoTransaccion"];
            SPQuery query = new SPQuery();
            query.Query = "<Where>" +
                            "<Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq>" +
                          "</Where>" +
                          "<OrderBy><FieldRef Name='orden' Ascending='TRUE' /></OrderBy>";

            SPListItemCollection collListItems = items.GetItems(query);
            util.CargaDDL(ddlTipoTransaccion, collListItems.GetDataTable(), "Title", "IdTransaccion");
        }

        private void CargarEstadoContabilidad()
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;

            SPList items = app.Lists["EstadoContabilidad"];
            SPQuery query = new SPQuery();
            query.Query = "<Where>" +
                            "<Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq>" +
                          "</Where>" +
                          "<OrderBy><FieldRef Name='orden' Ascending='TRUE' /></OrderBy>";


            SPListItemCollection collListItems = items.GetItems(query);
            util.CargaDDL(ddlEstado, collListItems.GetDataTable(), "Title", "ID");
        }

        private SPListItemCollection ConsultaListaEdicionUsuario()
        {
            //Traer lista EdicionFormularioUsuario
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList Lista = app.Lists["EdicionFormularioUsuario"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where>" +
                "<Or>" +
                 "<Eq><FieldRef Name='TipoFormulario'/><Value Type='TEXT'>" + "Empresa" + "</Value></Eq>" +
                 "<Eq><FieldRef Name='TipoFormulario'/><Value Type='TEXT'>" + "Operacion" + "</Value></Eq>" +
                 "</Or>" +
                "</Where>" +
                "<OrderBy>" + "<FieldRef Name='Modified' Ascending='True' />" + "</OrderBy>";
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            return ColecLista;
        }

        private DataTable ConsultarUsuariosContabilidad()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 5, "");
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "cartera", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

    }
}
