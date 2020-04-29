using System.Data;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using System.Drawing;
using System.Web.UI;
using MultigestionUtilidades;

namespace MultiOperacion.wpListarOperaciones.wpListarOperaciones
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarOperaciones : WebPart
    {
        private static string pagina = "ListarOperaciones.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarOperaciones()
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
        public SPListItemCollection ListaEdicion;
        public DataTable ListaUsuariosOperacion = new DataTable("dt");
        public static string USER;
        public static string ESTADO;
        public static string SUBETAPA;
        public static string ETAPA;
        public static DataTable OPCIONESPERMISOS;
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            string buscar = string.Empty;
            ocultarDiv();
            Boolean ban = false;

            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            LogicaNegocio MTO = new LogicaNegocio();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");

            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                ListaEdicion = ConsultaListaEdicionUsuario();
                ListaUsuariosOperacion = ConsultaUsuariosOperacion();

                if (!Page.IsPostBack)
                {
                    USER = util.ObtenerValor(app2.CurrentUser.Name);

                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString().Trim();
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString().Trim();

                    ban = true;
                    if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()) && ban == true)
                    {
                        ESTADO = "";
                        SUBETAPA = "";
                        ETAPA = "";
                        OPCIONESPERMISOS = null;

                        if (buscar != "#")
                            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), buscar, pageSize, pageNro);
                        else
                            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                    }
                }
                else
                {
                    if (buscar != "" && buscar != "#")
                        CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), buscar, pageSize, pageNro);
                    else
                        CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                }
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
                        lbmenu[i].CssClass = dt.Rows[i]["Imagen"].ToString();
                        lbmenu[i].CommandArgument = pos.ToString();
                        lbmenu[i].OnClientClick = "return Dialogo();";
                        lbmenu[i].Command += ResultadosBusqueda_Command1;

                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

                        if (val > 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        {
                            lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                            lbmenu[i].Visible = true;
                        }

                        if (val <= 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        {
                            lbmenu[i].Visible = false;
                        }

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
                    DropDownList ddlUsuariosOperacion = new DropDownList();
                    //String usuario, id;
                    ddlUsuariosOperacion.Items.Clear();

                    ddlUsuariosOperacion.DataSource = ListaUsuariosOperacion;
                    ddlUsuariosOperacion.DataValueField = "idUsuario";
                    ddlUsuariosOperacion.DataTextField = "nombreApellido";
                    ddlUsuariosOperacion.DataBind();

                    ListItem li = new ListItem("Sin Asignar", "0");
                    ddlUsuariosOperacion.Items.Insert(0, li);
                    ddlUsuariosOperacion.SelectedIndexChanged += ddlUsuariosOperacion_SelectedIndexChanged;
                    ddlUsuariosOperacion.AutoPostBack = true;
                    e.Row.Cells[23].Controls.Add(ddlUsuariosOperacion);
                    ddlUsuariosOperacion.ClearSelection();

                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[23].Text) == " " || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[23].Text) == "Sin Asignar")
                        ddlUsuariosOperacion.SelectedValue = "0";
                    else
                    {
                        ListItem itemAux = ddlUsuariosOperacion.Items.FindByText(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[23].Text));
                        if (itemAux != null)
                            itemAux.Selected = true;
                    }

                    e.Row.Cells[23].Controls.Add(ddlUsuariosOperacion);


                    Label cliente = new Label();
                    cliente.Text = e.Row.Cells[3].Text;
                    if (!string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[24].Text)))
                    {
                        LinkButton lbPrioridad = new LinkButton();
                        switch (e.Row.Cells[24].Text)
                        {
                            case "1":
                                lbPrioridad.ToolTip = "Prioridad Alta";
                                lbPrioridad.Attributes.Add("style", "color: red");
                                lbPrioridad.CssClass = ("glyphicon glyphicon-fire");
                                break;
                            case "2":
                                lbPrioridad.ToolTip = "Prioridad Media";
                                lbPrioridad.Attributes.Add("style", "color: #AEB404");
                                lbPrioridad.CssClass = ("glyphicon glyphicon-eye-open");
                                break;
                            case "3":
                                lbPrioridad.ToolTip = "Prioridad Baja";
                                lbPrioridad.Attributes.Add("style", "color: #298A08;");
                                lbPrioridad.CssClass = ("glyphicon glyphicon-leaf");
                                break;
                            default:
                                break;
                        }
                        
                        lbPrioridad.Enabled = false;
                        
                        if (e.Row.Cells[24].Text != "100" && e.Row.Cells[24].Text != "&nbsp;")
                        {
                            lbPrioridad.Attributes.CssStyle.Add("margin-right", "10px");
                            e.Row.Cells[3].Controls.Add(lbPrioridad);
                        }
                        //e.Row.Cells[3].Controls.Add(cliente);
                    }

                    //agregar icono de alerta
                    LinkButton lbAlertaCliente = new LinkButton();
                    Label texto = new Label();
                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[25].Text) != "OK")
                    {
                        texto.Text = util.Mensaje(e.Row.Cells[25].Text.Trim());
                        texto.Text = System.Web.HttpUtility.HtmlDecode(texto.Text.Replace("<br />",""));
                        lbAlertaCliente.CssClass = ("fa fa-ambulance");
                        lbAlertaCliente.Attributes.Add("style", "color: red");
                        
                        lbAlertaCliente.Attributes.Add("cssclass", "text-custom");
                        lbAlertaCliente.Attributes.Add("data-toggle", "tooltip");
                        lbAlertaCliente.Attributes.Add("data-placement", "top");
                        lbAlertaCliente.Attributes.Add("data-original-title", texto.Text);
                        lbAlertaCliente.ToolTip = texto.Text;
                        lbAlertaCliente.Attributes.CssStyle.Add("margin-left", "10px");

                        texto.ForeColor = Color.Red;
                        e.Row.Cells[25].Text = util.Mensaje(texto.Text);
                        //e.Row.Cells[3].Controls.Add(lbAlertaCliente);
                    }

                    e.Row.Cells[3].Controls.Add(cliente);

                    e.Row.Cells[3].Controls.Add(lbAlertaCliente);
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        void ddlUsuariosOperacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int indice = row.RowIndex;
            LogicaNegocio LN = new LogicaNegocio();
            LN.GestionResponsableOperacion(int.Parse(ResultadosBusqueda.Rows[indice].Cells[0].Text.ToString()), ddl.SelectedItem.Text, ViewState["CARGO"].ToString(), "03");
        }

        protected void ResultadosBusqueda_Command1(object sender, CommandEventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();
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
                        resumenP.idPAF = int.Parse(ResultadosBusqueda.Rows[index].Cells[20].Text.ToString());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text.ToString());

                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[2].Text.ToString();
                        resumenP.descEjecutivo = ResultadosBusqueda.Rows[index].Cells[22].Text.ToString();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());
                        resumenP.desOperacion = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.ToString());
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text.ToString()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.ToString());

                        resumenP.desEstado = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString());
                        resumenP.desEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.ToString());
                        resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString());

                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarOperaciones.aspx"; //
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        //resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[12].Text.ToString());
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        resumenP.IdCotizacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[26].Text.ToString());
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        ocultarDiv();
                        dvWarning.Style.Add("display", "block");
                        lbWarning.Text = "Registro seleccionado tiene inconsistencia en la data. Por favor Consultar al Administrador del Sistema";
                    }
                    if (ResultadosBusqueda.Rows[index].Cells[20].Text.ToString() != "&nbsp;")
                    {
                        if ("Documentos" == e.CommandName)
                        {
                            Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString())));
                        }
                        else Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                    }
                }

            }

        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[1].Visible = false;//empresa
                e.Row.Cells[0].Visible = false;//IdEmpesa
                e.Row.Cells[6].Visible = false;//area
                e.Row.Cells[2].Visible = false;//etapa
                //e.Row.Cells[4].Visible = false;//SubEtapa
                e.Row.Cells[5].Visible = false;//estado
                e.Row.Cells[7].Visible = false;//rut
                e.Row.Cells[8].Visible = false;//divrut
                e.Row.Cells[9].Visible = false;//val Empresa
                e.Row.Cells[10].Visible = false;//val detalle
                e.Row.Cells[11].Visible = false; //val accionista
                e.Row.Cells[12].Visible = false; //val directorio
                e.Row.Cells[13].Visible = false;//val relacionada
                e.Row.Cells[14].Visible = false; //val contacto
                e.Row.Cells[15].Visible = false; //valfiscaliaEmpresa
                e.Row.Cells[16].Visible = false; //valfiscaliaGarantia
                e.Row.Cells[17].Visible = false; //rownumber
                e.Row.Cells[18].Visible = false; //valfiscaliaGarantia
                e.Row.Cells[19].Visible = false; //rownumber
                e.Row.Cells[22].Visible = false; //ejecutivo dueño
                e.Row.Cells[24].Visible = false; //prioridad
                e.Row.Cells[26].Visible = false; //idcotizacion
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
            }
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanged(object sender, EventArgs e)
        {
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        #endregion

      
        #region Metodos

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        protected void CargarGrid(string etapa, string subEtapa, string estado, string buscar, int pageS, int pageN)
        {
            try
            {
                pos = 0;
                LogicaNegocio MTO = new LogicaNegocio();
                DataSet res;
                res = MTO.ListarOperaciones(etapa, subEtapa, estado, buscar, ViewState["AREA"].ToString(), ViewState["CARGO"].ToString(), USER.ToString(), pageS, pageN);
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

        private DataTable ConsultaUsuariosOperacion()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 4, "");
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
