using System.Data;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Web.UI.HtmlControls;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web;
using ClasesNegocio;
using Bd;

namespace MultiRiesgo.wpListarRiesgo.wpListarRiesgo
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarRiesgo : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarRiesgo()
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
        public DataTable dtUsuarioRiesgo = new DataTable("dt");
        Utilidades util = new Utilidades();
        private static string pagina = "ListarRiesgo.aspx";

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            string buscar = string.Empty;
            ocultarDiv();
            //Boolean ban = false;
            LogicaNegocio Ln = new LogicaNegocio();
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
                ListaEdicion = Ln.ConsultaListaEdicionUsuario();
                dtUsuarioRiesgo = ConsultarUsuarioRiesgo();

                if (!Page.IsPostBack)
                {
                    cb_estados.Attributes["onChange"] = "Dialogo();";
                    cb_etapa.Attributes["onChange"] = "Dialogo();";
                    cb_subetapa.Attributes["onChange"] = "Dialogo();";

                    ViewState["USER"] = util.ObtenerValor(app2.CurrentUser.Name);
                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString();
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString(); //"Riesgo";

                    if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()))
                    {

                        //Ln.CargarLista(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA);

                        CargarEstados();
                        CargarEtapas();
                        CargarSubEtapas();

                        ViewState["ESTADO"] = "";
                        ViewState["SUBETAPA"] = "";
                        ViewState["ETAPA"] = "";
                        ViewState["OPCIONESPERMISOS"] = null;

                        //CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                    }
                }
                //else
                //{
                CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                //}
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        private void CargarEstados()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEstados = Ln.ListarEstadosOperacion();
            util.CargaDDL(cb_estados, dtEstados, "Nombre", "Id");
            //Ln.CargarLista(ref cb_estados, Constantes.LISTAESTADO.ESTADO);       
        }

        private void CargarEtapas()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtEtapas = Ln.ListarEtapas(null);
            util.CargaDDL(cb_etapa, dtEtapas, "Descripcion", "Id");
            //Ln.CargarLista(ref cb_etapa, Constantes.LISTAETAPA.ETAPAS);
        }
        private void CargarSubEtapas()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtSubEtapas = Ln.ListarSubEtapas(int.Parse(cb_etapa.SelectedValue.ToString()), false, 1);
            util.CargaDDL(cb_subetapa, dtSubEtapas, "Nombre", "Id");
            //Ln.CargarListaSubEtapa(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA, "-1");
        }

        protected void cb_estados_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            ViewState["SUBETAPA"] = "";
            ViewState["ETAPA"] = "";
            //Ln.CargarLista(ref cb_etapa, Constantes.LISTAETAPA.ETAPAS);
            //Ln.CargarListaSubEtapa(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA, "-1");
            CargarSubEtapas();
            ViewState["ESTADO"] = cb_estados.SelectedValue.ToString();
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void cb_etapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LogicaNegocio Ln = new LogicaNegocio();
            //Ln.CargarListaSubEtapa(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA, cb_etapa.SelectedValue.ToString());
            CargarSubEtapas();
            ViewState["SUBETAPA"] = "";
            ViewState["ETAPA"] = cb_etapa.SelectedValue.ToString();
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void cb_subetapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["SUBETAPA"] = cb_subetapa.SelectedValue.ToString();
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Val = 0;
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["OPCIONESPERMISOS"];
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
                    DropDownList ddlUsuariosRiesgo = new DropDownList();
                    ddlUsuariosRiesgo.Items.Clear();

                    ListItem li = new ListItem("Sin Asignar", "0");
                    ddlUsuariosRiesgo.Items.Add(li);
                    ddlUsuariosRiesgo.DataSource = dtUsuarioRiesgo;
                    ddlUsuariosRiesgo.DataValueField = "idUsuario";
                    ddlUsuariosRiesgo.DataTextField = "nombreApellido";
                    ddlUsuariosRiesgo.DataBind();

                    ddlUsuariosRiesgo.SelectedIndexChanged += ddlUsuariosRiesgo_SelectedIndexChanged;
                    ddlUsuariosRiesgo.AutoPostBack = true;
                    ddlUsuariosRiesgo.Width = new Unit("120px");
                    ddlUsuariosRiesgo.ClearSelection();

                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[33].Text) == " " || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[33].Text) == "Sin Asignar")
                        ddlUsuariosRiesgo.SelectedValue = "0";
                    else
                    {
                        ListItem itemAux = ddlUsuariosRiesgo.Items.FindByText(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[33].Text));
                        if (itemAux != null)
                            itemAux.Selected = true;
                    }
                    e.Row.Cells[33].Controls.Add(ddlUsuariosRiesgo);

                    if (!string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[34].Text)))
                    {
                        Label cliente = new Label();
                        LinkButton lbPrioridad = new LinkButton();
                        switch (e.Row.Cells[34].Text)
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
                        cliente.Text = e.Row.Cells[3].Text;
                        if (e.Row.Cells[34].Text != "100" && e.Row.Cells[34].Text != "&nbsp;")
                        {
                            lbPrioridad.Attributes.CssStyle.Add("margin-right", "10px");
                            e.Row.Cells[4].Controls.Add(lbPrioridad);
                        }
                        e.Row.Cells[4].Controls.Add(cliente);
                    }


                    if (e.Row.Cells[7].Text.ToString() == "Caducado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-default");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text.ToString()) == "Aprobado con Reparos")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-warning");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text.ToString() == "Rechazado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-danger");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }
                    if (e.Row.Cells[7].Text.ToString() == "Aprobada" || e.Row.Cells[7].Text.ToString() == "Aprobado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-success");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text.ToString() == "Procesado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        //lbb1.CssClass = ("label label-lila");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        protected void ddlUsuariosRiesgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.Parent.Parent;
            int indice = row.RowIndex;
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(int.Parse(ResultadosBusqueda.Rows[indice].Cells[0].Text.ToString()), ddl.SelectedItem.Text, ViewState["CARGO"].ToString(), "01");
        }

        protected void ResultadosBusqueda_Command1(object sender, CommandEventArgs e)
        {
            if (ViewState["AREA"] == null || ViewState["CARGO"] == null || ViewState["USER"] == null)
            {
                Page.Response.Redirect("MensajeSession.aspx");
            }

            Boolean ban = true;
            int index = Convert.ToInt32(e.CommandArgument);
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["OPCIONESPERMISOS"];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["Opcion"].ToString() == e.CommandName)
                {
                    try
                    {
                        Resumen resumenP = new Resumen();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text.ToString());
                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[3].Text.ToString();
                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[5].Text.ToString();
                        resumenP.desEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString());
                        resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.ToString());
                        resumenP.desEstado = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text.ToString());

                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text.ToString()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[12].Text.ToString());
                        resumenP.descEjecutivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[31].Text.ToString());

                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarRiesgo.aspx"; //
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[13].Text.ToString());
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        resumenP.idEtapa = int.Parse(ResultadosBusqueda.Rows[index].Cells[37].Text.ToString());
                        resumenP.idSubEtapa = int.Parse(ResultadosBusqueda.Rows[index].Cells[32].Text.ToString());
                        resumenP.idEstado = int.Parse(ResultadosBusqueda.Rows[index].Cells[38].Text.ToString());

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
                            Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                }
            }

        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[0].Visible = false;  //idOperacion
                e.Row.Cells[1].Visible = false;  //IdEmpesa
                //e.Row.Cells[2].Visible = false; //Iniciales Ejecutivo
                e.Row.Cells[3].Visible = false; //empresa  
                //e.Row.Cells[4].Visible = false; //cliente
                //e.Row.Cells[5].Visible = false; //operacion (ncertificado + producto)     
                //e.Row.Cells[6].Visible = false; //monto  
                //e.Row.Cells[7].Visible = false; //etapa
                //e.Row.Cells[8].Visible = false; //sub etapa   

                e.Row.Cells[9].Visible = false;  //estado
                e.Row.Cells[10].Visible = false; //area
                e.Row.Cells[11].Visible = false; //rut
                e.Row.Cells[12].Visible = false; //div rut
                e.Row.Cells[13].Visible = false; //fecha inicio actividad economica
                e.Row.Cells[14].Visible = false; //fecha de cierre
                e.Row.Cells[15].Visible = false; //val Empresa
                e.Row.Cells[16].Visible = false; //val Empresa detalle
                e.Row.Cells[17].Visible = false; //val operacion
                e.Row.Cells[18].Visible = false; //val Empresa ACCIONista
                e.Row.Cells[19].Visible = false; //val Empresa directorio
                e.Row.Cells[20].Visible = false; //val Empresa relacionada
                e.Row.Cells[21].Visible = false; //val garantia
                e.Row.Cells[22].Visible = false; //val contacto
                e.Row.Cells[23].Visible = false; //val documentos
                e.Row.Cells[24].Visible = false;  //rownumber
                e.Row.Cells[25].Visible = false;  //ValScoring
                e.Row.Cells[26].Visible = false;  //ValBalance
                e.Row.Cells[27].Visible = false;  //ValEstado
                e.Row.Cells[28].Visible = false;  //ValVentas
                e.Row.Cells[29].Visible = false;  //ValCompras 
                e.Row.Cells[30].Visible = false;  //ID PAF
                e.Row.Cells[31].Visible = false;  //dueno ejecutivo
                e.Row.Cells[32].Visible = false;  //Id subETAPA
                //e.Row.Cells[33].Visible = false;  //Responsable
                e.Row.Cells[34].Visible = false;  //Prioridad
                //e.Row.Cells[35].Visible = false;  //Saldo Vigente
                e.Row.Cells[36].Visible = false;  //fecha ingreso a riesgo
                e.Row.Cells[37].Visible = false;  //IdEtapa
                e.Row.Cells[38].Visible = false;  //IdEstado
                e.Row.Cells[39].Visible = false;  //Bandeja

                //e.Row.Cells[38].Visible = false;  //datos Empresa
                //e.Row.Cells[39].Visible = false;  //adicional
                //e.Row.Cells[40].Visible = false;  //datos operacion
                //e.Row.Cells[41].Visible = false;  //documentos
                //e.Row.Cells[42].Visible = false;  //accion
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
                            gvRow.Cells[23].RowSpan = 2;
                        }
                        else
                        {
                            gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                            gvRow.Cells[23].RowSpan = gvPreviousRow.Cells[23].RowSpan + 1;
                        }
                        gvPreviousRow.Cells[cellCount].Visible = false;
                        gvPreviousRow.Cells[23].Visible = false;
                    }
                }
            }
        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ViewState["ESTADO"] = cb_estados.SelectedValue.ToString();
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanged(object sender, EventArgs e)
        {
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
                res = Ln.ListarRiesgo(etapa, subEtapa, estado, buscar, ViewState["AREA"].ToString(), ViewState["CARGO"].ToString(), ViewState["USER"].ToString(), pageS, pageN);
                ViewState["OPCIONESPERMISOS"] = res.Tables[0];
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
                                res.Tables[0].Rows[i]["nroColum"] = res.Tables[0].Rows[i - 1]["nroColum"];
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
                        lbWarning.Text = "No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros.";
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

        private DataTable ConsultarUsuarioRiesgo()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 2, "");
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
