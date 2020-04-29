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
using System.Web.UI;
using System.Web;
using System.Globalization;
using System.Xml;
using System.Drawing;
using MultigestionUtilidades;

namespace MultiFiscalia.wpListarFiscalia.wpListarFiscalia
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarFiscalia : WebPart
    {
        private static string pagina = "ListarFiscalia.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarFiscalia()
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
        public SPListItemCollection ListaUsuariosFiscalia;
        public DataTable dtUsuarioFiscalia = new DataTable("dt");
        Utilidades util = new Utilidades();
        public static string ESTADO = "-1";

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlEdoFiscalia.Attributes["onChange"] = "Dialogo();";
            LogicaNegocio MTO = new LogicaNegocio();
            //PERMISOS USUARIOS
            string buscar = string.Empty;
            Boolean ban = false;

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
                if (Page.Request.Form["__EVENTTARGET"] == "UsuarioSelected")
                {
                    string datos = Page.Request.Form["__EVENTARGUMENT"];
                    string[] words = datos.Split(';');
                    ddlUsuariosFiscalia_Selected(words[0], words[1]);
                }

                ocultarDiv();
                ListaEdicion = ConsultaListaEdicionUsuario();
                dtUsuarioFiscalia = ConsultaUsuariosFiscalia();
                try
                {
                    if (!Page.IsPostBack)
                    {
                        ddlEdoFiscalia.SelectedIndex = 0;
                        ViewState["USER"] = util.ObtenerValor(app2.CurrentUser.Name);
                        ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString();
                        ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString(); //"Fiscalia";  //

                        ban = true;
                        ESTADO = "-1";

                        if (Page.Session["BUSQUEDA"] != null)
                        {
                            Busqueda objBusqueda = new Busqueda();
                            objBusqueda = (Busqueda)Page.Session["BUSQUEDA"];

                            if (objBusqueda.idEstado != -1)
                            {
                                ESTADO = objBusqueda.idEstado.ToString();
                                ddlEdoFiscalia.SelectedIndex = ddlEdoFiscalia.Items.IndexOf(ddlEdoFiscalia.Items.FindByValue(Convert.ToString(objBusqueda.idEstado)));
                            }
                            txtBuscar.Text = string.IsNullOrEmpty(objBusqueda.RazonSocial) ? "" : objBusqueda.RazonSocial;
                            Page.Session["BUSQUEDA"] = null;
                        }

                        if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()) && ban == true)
                        {
                            ViewState["SUBETAPA"] = "";
                            ViewState["ETAPA"] = "";
                            ViewState["ESTADO"] = "";
                            ViewState["OPCIONESPERMISOS"] = null;

                            if (buscar != "#")
                                CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), buscar, pageSize, pageNro);
                            else
                                CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                        }
                    }
                    else
                    {
                        if (buscar != "#")
                            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), buscar, pageSize, pageNro);
                        else
                            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
                    }
                }

                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
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
            int totalDoc = 0;
            int totalDocSubidos = 0;
            val = 0;
            try
            {
                totalDoc = 0;
                totalDocSubidos = 0;
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

                        lbmenu[i].Command += ResultadosBusqueda_Command1;
                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

                        if (int.Parse(System.Web.HttpUtility.HtmlDecode((e.Row.Cells[11].Text.ToString()))) < 2 && dt.Rows[i]["Opcion"].ToString() == "ServicioFiscaliaEmpresa")
                        {
                            lbmenu[i].Enabled = false;
                            lbmenu[i].OnClientClick = "return false;";
                        }
                        if (int.Parse(System.Web.HttpUtility.HtmlDecode((e.Row.Cells[12].Text.ToString()))) < 2 && dt.Rows[i]["Opcion"].ToString() == "ServicioFiscaliaGarantia")
                        {
                            lbmenu[i].Enabled = false;
                            lbmenu[i].OnClientClick = "return false;";
                        }

                        if (dt.Rows[i]["Descripcion"].ToString().Trim() == "Acción")
                        {
                            if (int.Parse(System.Web.HttpUtility.HtmlDecode((e.Row.Cells[32].Text.ToString()))) == 1 && dt.Rows[i]["Opcion"].ToString() == "Devolver Operacion Flujo Fiscalia")
                            {
                                lbmenu[i].Visible = false;
                            }

                            if (int.Parse(System.Web.HttpUtility.HtmlDecode((e.Row.Cells[32].Text.ToString()))) > 1 && dt.Rows[i]["Opcion"].ToString() == "DevolucionOperacion")
                            {
                                lbmenu[i].Visible = false;
                            }
                        }

                        //SolicitudFiscalia
                        //if ((System.Web.HttpUtility.HtmlDecode((e.Row.Cells[11].Text.ToString())) == "2" || System.Web.HttpUtility.HtmlDecode((e.Row.Cells[12].Text.ToString())) == "2" ||
                        //    System.Web.HttpUtility.HtmlDecode((e.Row.Cells[11].Text.ToString())) == "3" || System.Web.HttpUtility.HtmlDecode((e.Row.Cells[12].Text.ToString())) == "3" ||
                        //    System.Web.HttpUtility.HtmlDecode((e.Row.Cells[11].Text.ToString())) == "4" || System.Web.HttpUtility.HtmlDecode((e.Row.Cells[12].Text.ToString())) == "4")
                        //    && dt.Rows[i]["Opcion"].ToString() == "SolicitudFiscalia")
                        //{
                        //    lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString()); val++;
                        //}

                        //if (val > 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        //{
                        //    lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                        //    lbmenu[i].Visible = true;
                        //}

                        //if (val <= 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        //    lbmenu[i].Visible = false;

                        //if (val <= 20 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        //{
                        //    if (System.Web.HttpUtility.HtmlDecode((e.Row.Cells[11].Text.ToString())) == "4" && System.Web.HttpUtility.HtmlDecode((e.Row.Cells[12].Text.ToString())) == "4")
                        //    {
                        //        totalDoc = validarTodosDocumentos(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[0].Text.ToString()), System.Web.HttpUtility.HtmlDecode(e.Row.Cells[28].Text.ToString()));
                        //        totalDocSubidos = validarDocumentosSubidos(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[28].Text.ToString()), System.Web.HttpUtility.HtmlDecode(e.Row.Cells[2].Text.ToString()), (System.Web.HttpUtility.HtmlDecode((e.Row.Cells[9].Text.ToString())) + "-" + System.Web.HttpUtility.HtmlDecode((e.Row.Cells[10].Text.ToString()))), System.Web.HttpUtility.HtmlDecode(e.Row.Cells[5].Text.ToString()));
                        //        if (totalDocSubidos >= totalDoc)
                        //            lbmenu[i].Visible = true;
                        //    }
                        //    else
                        //        lbmenu[i].Visible = false;
                        //}                           

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
                    DropDownList ddlUsuariosFiscalia = new DropDownList();
                    ddlUsuariosFiscalia.ID = "ddlUsuariosFiscalia";

                    //usuario responsable
                    String id;
                    ddlUsuariosFiscalia.Items.Clear();
                    ddlUsuariosFiscalia.DataSource = dtUsuarioFiscalia;
                    ddlUsuariosFiscalia.DataValueField = "idUsuario";
                    ddlUsuariosFiscalia.DataTextField = "nombreApellido";
                    ddlUsuariosFiscalia.DataBind();

                    ListItem li = new ListItem("Sin Asignar", "0");
                    ddlUsuariosFiscalia.Items.Insert(0, li);

                    foreach (DataRow row in dtUsuarioFiscalia.Rows)
                    {
                        id = row["idUsuario"].ToString().Trim();
                    }


                    if (string.IsNullOrWhiteSpace(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[29].Text)) || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[29].Text) == "Sin Asignar")
                        ddlUsuariosFiscalia.SelectedValue = "0";
                    else
                    {
                        ListItem itemAux = ddlUsuariosFiscalia.Items.FindByText(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[29].Text));
                        if (itemAux != null)
                            itemAux.Selected = true;
                    }
                    e.Row.Cells[29].Controls.Add(ddlUsuariosFiscalia);


                    if (!string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(e.Row.Cells[31].Text)))
                    {
                        Label cliente = new Label();
                        LinkButton lbPrioridad = new LinkButton();
                        switch (e.Row.Cells[31].Text)
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
                        if (e.Row.Cells[31].Text != "100" && e.Row.Cells[31].Text != "&nbsp;")
                        {
                            lbPrioridad.Attributes.CssStyle.Add("margin-right", "10px");
                            e.Row.Cells[3].Controls.Add(lbPrioridad);
                        }
                        e.Row.Cells[3].Controls.Add(cliente);
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void ddlUsuariosFiscalia_Selected(string idoperacion, string usuarioF)
        {
            if (idoperacion.Length > 0 && usuarioF.Length > 0)
            {
                LogicaNegocio lcGlobal = new LogicaNegocio();
                lcGlobal.GestionResponsableOperacion(int.Parse(idoperacion), usuarioF, ViewState["CARGO"].ToString(), "02");
                CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
            }
        }

        protected void ResultadosBusqueda_Command1(object sender, CommandEventArgs e)
        {
            Busqueda objBusqueda = new Busqueda();
            objBusqueda.idEstado = int.Parse(ddlEdoFiscalia.SelectedValue);
            objBusqueda.RazonSocial = txtBuscar.Text.ToString();

            Page.Session["BUSQUEDA"] = objBusqueda;

            LogicaNegocio LN = new LogicaNegocio();
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
                        resumenP.idPAF = int.Parse(ResultadosBusqueda.Rows[index].Cells[14].Text.ToString());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());

                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[28].Text.ToString());

                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[4].Text.ToString();

                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[3].Text.ToString();
                        resumenP.descEjecutivo = ResultadosBusqueda.Rows[index].Cells[17].Text.ToString();
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[9].Text.ToString()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.ToString());

                        resumenP.desEstado = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString());
                        resumenP.desEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text.ToString());
                        resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString());

                        resumenP.area = ViewState["AREA"].ToString(); //System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.Trim());
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarFiscalia.aspx";
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.DescEstadoFiscalia = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[19].Text.ToString());
                        resumenP.IdEstadoFiscalia = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[18].Text.ToString()));
                        resumenP.IdAcreedor = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[30].Text.ToString()));

                        resumenP.OrdenSubEtapaLegal = Convert.ToInt32(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[32].Text.ToString()));

                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        Page.Response.Redirect("ListarFiscalia.aspx");
                    }

                    if ("Documentos" == e.CommandName)
                    {
                        Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString())));
                    }
                    else
                        Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());
                }
            }
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[0].Visible = false;//IdEmpesa
                e.Row.Cells[1].Visible = false;//ID Empresa
                e.Row.Cells[2].Visible = false;//razon social
                e.Row.Cells[5].Visible = false;//etapa
                e.Row.Cells[6].Visible = false;//SubEtapa
                e.Row.Cells[7].Visible = false;//estado
                e.Row.Cells[8].Visible = false;//area
                e.Row.Cells[9].Visible = false;//rut
                e.Row.Cells[10].Visible = false;//div rut
                e.Row.Cells[11].Visible = false;//val fis empresa
                e.Row.Cells[12].Visible = false;//val fis garantia
                e.Row.Cells[13].Visible = false;//validacion operacion

                //e.Row.Cells[16].Visible = false;//fecha solicitud
                e.Row.Cells[17].Visible = false;//val detalle
                e.Row.Cells[18].Visible = false;//IdServiciosLegales
                //e.Row.Cells[19].Visible = false;//estado fiscalia
                e.Row.Cells[20].Visible = false;//val empresa
                e.Row.Cells[21].Visible = false;//val empresa detalle
                e.Row.Cells[22].Visible = false;//val empresa accionista
                e.Row.Cells[23].Visible = false;//val empresa directorio
                e.Row.Cells[24].Visible = false;//val empresa relacionada
                e.Row.Cells[25].Visible = false;//val garantia
                e.Row.Cells[26].Visible = false;//val contacto
                e.Row.Cells[27].Visible = false;//val documento
                e.Row.Cells[28].Visible = false;//id operacion 
                //e.Row.Cells[29].Visible = false;//Responsable
                e.Row.Cells[30].Visible = false;//id ACREEDOR
                e.Row.Cells[31].Visible = false;//prioridad
                e.Row.Cells[32].Visible = false;//OrdenSubEtapaLegal
            }
        }

        protected void ResultadosBusqueda_DataBound(object sender, EventArgs e)
        {
            String IdOperacionTemp, IdEmpresaTemp;

            for (int i = 0; i < ResultadosBusqueda.Rows.Count; i++)
            {
                IdOperacionTemp = ResultadosBusqueda.Rows[i].Cells[27].Text;
                IdEmpresaTemp = ResultadosBusqueda.Rows[i].Cells[0].Text;
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
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEdoFiscalia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrid(ViewState["ETAPA"].ToString(), ViewState["SUBETAPA"].ToString(), ViewState["ESTADO"].ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
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
                res = MTO.ListarFiscalia(etapa, subEtapa, estado, txtBuscar.Text.ToString(), ViewState["AREA"].ToString(), ViewState["CARGO"].ToString(), ViewState["USER"].ToString(), pageS, pageN, ddlEdoFiscalia.SelectedValue.ToString());
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
                                if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
                                {
                                    con++;
                                    res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
                                }
                                else
                                {
                                    if (res.Tables[0].Rows[i]["Descripcion"].ToString() != res.Tables[0].Rows[i - 1]["Descripcion"].ToString())
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
                            else if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
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
                        lbWarning.Text = "No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros";
                    }

                    AsignarUsuario();
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

        public int validarDocumentosSubidos(string idOperacion, string empresa, string rut, string area)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtArchivos = new DataTable("dtArchivos");
            int val = 0;

            //validar si existen documentos criticos, si existen no se puede mostrar boton aprobar hasta subir los documentos asociados
            dtArchivos = util.buscarArchivos(empresa.Trim(), rut.Trim(), area.Trim(), idOperacion.ToString());
            val = dtArchivos.Rows.Count;
            return val;
        }

        public int validarTodosDocumentos(string idEmpresa, string idOperacion)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtArchivosCriticos = new DataTable("dtArchivosCriticos");
            DataTable dtArchivos = new DataTable("dtArchivos");
            int val = 0;

            //validar si existen documentos criticos, si existen no se puede mostrar boton aprobar hasta subir los documentos asociados
            dtArchivosCriticos = Ln.validarDocCriticos(idEmpresa.Trim(), idOperacion, 4);
            if (dtArchivosCriticos.Rows.Count > 0)
            {
                //dtArchivos = util.buscarArchivos(empresa.Trim(), rut.Trim(), area.Trim(), idOperacion.ToString());
                val = dtArchivosCriticos.Rows.Count; //util.ValidarDocCriticos(dtArchivosCriticos, dtArchivos);
            }
            //else
            //    val = 0;

            return val;
        }

        private SPListItemCollection ConsultaListaEdicionUsuario()
        {
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

        private DataTable ConsultaUsuariosFiscalia()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 3, "");
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "cartera", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        private void AsignarUsuario()
        {
            foreach (GridViewRow row in ResultadosBusqueda.Rows)
            {
                DropDownList ddlUsuariosFiscalia = (DropDownList)row.Cells[28].FindControl("ddlUsuariosFiscalia");
                if (ddlUsuariosFiscalia != null)
                {
                    //var b = row.Cells[28].Text;
                    ddlUsuariosFiscalia.Attributes.Add("onchange", "jsSelectUsuarios(" + row.Cells[28].Text + ", this)");
                    ddlUsuariosFiscalia.AutoPostBack = true;
                }
            }
        }

        #endregion

    }
}
