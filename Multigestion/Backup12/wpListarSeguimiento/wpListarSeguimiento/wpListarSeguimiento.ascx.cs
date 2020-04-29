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
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
//using MultigestionUtilidades;
using ClasesNegocio;
using Bd;

namespace MultiOperacion.wpListarSeguimiento.wpListarSeguimiento
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarSeguimiento : WebPart
    {
        private static string pagina = "ListarSeguimiento.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarSeguimiento()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int pos;
        int Val = 0;
        public static int pageSize = 100;
        public static int pageNro = 0;
        public static string USER;
        public static string ESTADO;
        public static string SUBETAPA;
        public static string ETAPA;
        public static DataTable OPCIONESPERMISOS;
        public SPListItemCollection ListaEdicion;
        Resumen objresumen = new Resumen();
        //Utilidades util = new Utilidades();


        #region Eventos
   
        protected void Page_Load(object sender, EventArgs e)
        {
            string buscar = string.Empty;
            ocultarDiv();
            Boolean ban = false;
            //Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            LogicaNegocio Ln = new LogicaNegocio();
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

                if (!Page.IsPostBack)
                {
                    Page.Session["espdf"] = "0";
                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString();
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString();
                    USER = util.ObtenerValor(app2.CurrentUser.Name);

                    cb_estados.Attributes["onChange"] = "Dialogo();";
                    cb_etapa.Attributes["onChange"] = "Dialogo();";
                    cb_subetapa.Attributes["onChange"] = "Dialogo();";

                    string val = Page.Request.QueryString["Area"] as string;
                    buscar = Page.Request.QueryString["IdEmpresa"] as string + "#" + Page.Request.QueryString["IdOperacion"] as string;

                    ban = true;

                    if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()) && ban == true)
                    {
                        Ln.CargarLista(ref cb_estados, Constantes.LISTAESTADO.ESTADO);
                        Ln.CargarLista(ref cb_etapa, Constantes.LISTAETAPA.ETAPAS);
                        Ln.CargarListaSubEtapa(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA, "-1");

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
                warning("Usuario sin permisos configurados");   
            }
        }

        protected void cb_estados_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            SUBETAPA = "";
            ETAPA = "";
            //MTO.CargarLista(ref cb_etapa, Constante.LISTAETAPA.ETAPAS);
            //MTO.CargarListaSubEtapa(ref cb_subetapa, Constante.LISTASUBETAPA.SUBETAPA, "-1");
            ESTADO = cb_estados.SelectedValue.ToString();
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void cb_etapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            MTO.CargarListaSubEtapa(ref cb_subetapa, Constantes.LISTASUBETAPA.SUBETAPA, cb_etapa.SelectedValue.ToString());
            SUBETAPA = "";
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
            //int col = 0;
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

                        lbmenu[i].Command += ResultadosBusqueda_Command1;
                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

                        //if (Val > 6 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        //{
                        //    lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                        //    lbmenu[i].Visible = true;
                        //}

                        //if (Val <= 6 && dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                        //    lbmenu[i].Visible = false;

                        //OCULTAR EL ITEM Descarga de documentos si la etapa esta en Prospecto, Negociacion y Evaluacion de riesgo ya que la operacion no tiene una propuesta de afianzamiento
                        //if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) == "Prospecto" || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) == "Negociacion" || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) == "Evaluación Riesgo")
                        //    if (dt.Rows[i]["Descripcion"].ToString().Trim() == "Documentos")
                        //        e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Visible = false;

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

                    //OCULTAR EL ITEM Descarga de documentos si la etapa esta en Prospecto, Negociacion y Evaluacion de riesgo ya que la operacion no tiene una propuesta de afianzamiento
                    //if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) == "Prospecto" || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) == "Negociacion" || System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) == "Evaluación Riesgo")
                    //    e.Row.Cells[34].Controls.Clear();

                    if (e.Row.Cells[7].Text == "Sin Contactar")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-blanco");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text == "Contactado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-primary");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text) == "Antecedentes y Evaluación")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-info");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text == "Caducado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-default");
                        e.Row.Cells[7].Controls.Add(lbb1);
                        e.Row.Cells[34].Controls.Clear();
                    }

                    if (e.Row.Cells[7].Text == "Anulado")
                    {
                        e.Row.Cells[11].Controls.Clear();
                    }

                    if (e.Row.Cells[7].Text.ToString() == "Perdido")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-vacio");
                        e.Row.Cells[7].Controls.Add(lbb1);
                        e.Row.Cells[34].Controls.Clear();
                    }

                    if (e.Row.Cells[7].Text == "Excluido")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-default");
                        e.Row.Cells[7].Controls.Add(lbb1);
                        e.Row.Cells[34].Controls.Clear();
                    }

                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text) == "Por Emitir")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-warning");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text) == "Aprobado con Reparos")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-warning");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text == "Rechazado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-danger");
                        e.Row.Cells[7].Controls.Add(lbb1);
                        e.Row.Cells[11].Controls.Clear();
                    }

                    if (e.Row.Cells[7].Text == "Rechazada")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-danger");
                        e.Row.Cells[7].Controls.Add(lbb1);
                        e.Row.Cells[34].Controls.Clear();
                    }

                    if (e.Row.Cells[7].Text == "Aprobada")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-success");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text == "Pagado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-success");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }

                    if (e.Row.Cells[7].Text.ToString() == "Procesado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text.ToString();
                        lbb1.CssClass = ("label label-lila");
                        e.Row.Cells[7].Controls.Add(lbb1);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
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
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text);
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text);
                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[4].Text;
                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[2].Text;
                        resumenP.descEjecutivo = ResultadosBusqueda.Rows[index].Cells[32].Text;
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text);

                        resumenP.desEstado = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text);
                        resumenP.desEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text);
                        resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text);

                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarSeguimiento.aspx"; //
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();//IdPermiso
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.fecInicioActEconomica =  string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[12].Text.Trim())) ? (DateTime?) null : DateTime.Parse(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[12].Text));
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["RESUMEN"] = resumenP;
                    }
                    catch (Exception ex)
                    {
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        warning("Error al procesar la solicitud");
                    }

                    if ("Documentos" == e.CommandName)
                    {
                        Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim() + "?RootFolder=%2FDocumentos%2F" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString())));
                    }
                    else
                        Page.Response.Redirect(dt.Rows[i]["Url"].ToString().Trim());

                }
                if (e.CommandName == "AddNegocio")
                {
                    try
                    {
                        Resumen resumenP = new Resumen();
                        resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[1].Text.ToString());
                        resumenP.desOperacion = ResultadosBusqueda.Rows[index].Cells[4].Text.ToString();
                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[2].Text.ToString();
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[10].Text.ToString()) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[11].Text.ToString());
                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "ListarRiesgo.aspx";
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.fecInicioActEconomica = DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[12].Text.ToString());
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
                    Page.Response.Redirect("MantenedorOperaciones.aspx");
                }
            }
        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                e.Row.Cells[2].Visible = false;//empresa
                e.Row.Cells[1].Visible = false;//idOperacion
                e.Row.Cells[0].Visible = false;//IdEmpesa
                e.Row.Cells[8].Visible = false;//area
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
                e.Row.Cells[23].Visible = false; //rownumber
                e.Row.Cells[24].Visible = false;  //ValScoring,	
                e.Row.Cells[25].Visible = false;  //ValBalance,  
                e.Row.Cells[26].Visible = false;  //ValEstado,  	
                e.Row.Cells[27].Visible = false;  //ValVentas, 	
                e.Row.Cells[28].Visible = false;  //ValCompras 

                e.Row.Cells[29].Visible = false;  //PAF 
                e.Row.Cells[30].Visible = false; // dueño
                e.Row.Cells[31].Visible = false; //id
                //e.Row.Cells[32].Visible = false;  //dueno ejecutivo 
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

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void ResultadosBusqueda_PageIndexChanged(object sender, EventArgs e)
        { }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            ESTADO = cb_estados.SelectedValue.ToString();
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
        }

        protected void btnrefrescar_Click(object sender, EventArgs e)
        {
            CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtBuscar.Text.ToString(), pageSize, pageNro);
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

        protected void CargarGrid(string etapa, string subEtapa, string estado, string buscar, int pageS, int pageN)
        {
            try
            {
                pos = 0;
                LogicaNegocio MTO = new LogicaNegocio();
                DataSet res;

                res = MTO.ListarSeguimiento1(etapa, subEtapa, estado, buscar, ViewState["AREA"].ToString(), ViewState["CARGO"].ToString(), USER.ToString(), pageS, pageN);

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
                        ResultadosBusqueda.DataSource = res.Tables[1];
                        ResultadosBusqueda.DataBind();
                    }
                    else
                    {
                        ResultadosBusqueda.Visible = false;
                        warning("No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros");           
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void warning(string texto)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = texto.Trim();
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
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

        #endregion

    }
}
