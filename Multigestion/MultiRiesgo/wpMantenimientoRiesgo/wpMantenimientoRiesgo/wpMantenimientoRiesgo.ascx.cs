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

namespace MultiRiesgo.wpMantenimientoRiesgo.wpMantenimientoRiesgo
{
    [ToolboxItemAttribute(false)]
    public partial class wpMantenimientoRiesgo : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpMantenimientoRiesgo()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int pos;
        public SPListItemCollection ListaEdicion;
        Utilidades util = new Utilidades();
        private static string pagina = "RiesgoEmpresa.aspx";

        #region Eventos

        protected void CargarGrid()
        {
            try
            {
                pos = 0;
                LogicaNegocio Ln = new LogicaNegocio();
                DataSet res;
                string razonSocial = txtRazonSocial.Text;
                string rut = txtRUT.Text;
                string descEjecutivo = ddlEjecutivo.SelectedItem.Text == "Seleccione" ? "" : ddlEjecutivo.SelectedItem.Text;

                res = Ln.ListarMantenimientoRiesgo(razonSocial, rut, descEjecutivo, ViewState["CARGO"].ToString(), ViewState["AREA"].ToString());

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

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            Boolean ban = false;
            LogicaNegocio Ln = new LogicaNegocio();
            ListaEdicion = Ln.ConsultaListaEdicionUsuario();
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
                    ViewState["USER"] = util.ObtenerValor(app2.CurrentUser.Name);
                    ViewState["CARGO"] = dt.Rows[0]["descCargo"].ToString();
                    ViewState["AREA"] = dt.Rows[0]["Etapa"].ToString();

                    ban = true;

                    if (!string.IsNullOrEmpty(ViewState["CARGO"].ToString()) && ban == true)
                    {
                        CargarEjecutivo();
                        CargarGrid();
                    }
                }
                else
                {
                    CargarEjecutivo();
                    CargarGrid();
                }

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
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

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
                        lbmenu[i].Command += ResultadosBusqueda_Command1;

                        lbmenu[i].Attributes.Add("cssclass", "text-custom");
                        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
                        lbmenu[i].Attributes.Add("data-placement", "top");
                        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

                        if (dt.Rows[i]["Opcion"].ToString() == "Compromisos")  //ocultar accion en la opcion DatosOperacion
                            lbmenu[i].Visible = false;

                        if (dt.Rows[i]["Opcion"].ToString() == "BitacoraComentarios") //ocultar accion en la opcion DatosOperacion
                            lbmenu[i].Visible = false;

                        if (dt.Rows[i]["Opcion"].ToString() == "Negocio") //ocultar accion en la opcion DatosOperacion
                            lbmenu[i].Visible = false;

                        if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR") // Val > 6 && siempre devar visible en avanzar
                        {
                            lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
                            lbmenu[i].Visible = true;
                        }

                        if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
                            lbmenu[i].Visible = false;

                        if (dt.Rows[i]["Opcion"].ToString() == "RechazoRiesgo")
                            lbmenu[i].Visible = false;

                        if (dt.Rows[i]["Opcion"].ToString() == "DevolucionOperacion")
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

                        if (e.Row.RowIndex > -1)
                        {
                            LinkButton lb = new LinkButton();
                            lb.CssClass = ("glyphicon glyphicon-search");
                            lb.CommandName = "PosicionCliente";
                            lb.CommandArgument = pos.ToString();
                            //i++;
                            lb.OnClientClick = "return Dialogo();";
                            lb.Command += ResultadosBusqueda_Command1;
                            //e.Row.Cells[19].Text = "";
                            //e.Row.Cells[19].Controls.Add(lb);
                            e.Row.Cells[18].Text = "";
                            e.Row.Cells[18].Controls.Add(lb);
                        }
                        else
                        {
                            e.Row.Cells[18].Text = "Posición Cliente";
                        }

                    }

                    pos++;

                    if (e.Row.Cells[7].Text == "Caducado")
                    {
                        Label lbb1 = new Label();
                        lbb1.Text = e.Row.Cells[7].Text;
                        lbb1.CssClass = ("label label-default");
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
            LogicaNegocio LN = new LogicaNegocio();

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
                        //resumenP.idOperacion = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text.ToString());
                        resumenP.idEmpresa = int.Parse(ResultadosBusqueda.Rows[index].Cells[0].Text);
                        resumenP.desOperacion = "No aplica";
                        resumenP.desEmpresa = ResultadosBusqueda.Rows[index].Cells[1].Text;
                        resumenP.rut = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[3].Text) + '-' + System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[4].Text.ToString());
                        resumenP.descEjecutivo = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[5].Text);
                        //resumenP.desEstado = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[8].Text.ToString());
                        //resumenP.desEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[6].Text.ToString());
                        //resumenP.desSubEtapa = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[7].Text.ToString());

                        resumenP.area = ViewState["AREA"].ToString();
                        resumenP.linkActual = dt.Rows[i]["Url"].ToString().Trim();
                        resumenP.linkPrincial = "RiesgoEmpresa.aspx"; //
                        resumenP.linkError = "Mensaje.aspx";
                        resumenP.idPermiso = dt.Rows[i]["IdPermiso"].ToString().Trim();
                        resumenP.desPermiso = dt.Rows[i]["Permiso"].ToString().Trim();//IdPermiso
                        resumenP.descCargo = dt.Rows[i]["Cargo"].ToString().Trim();
                        resumenP.idCargo = dt.Rows[i]["IdCargo"].ToString().Trim();
                        resumenP.fecInicioActEconomica = util.ValidarFecha(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString());
                        //DateTime.Parse(ResultadosBusqueda.Rows[index].Cells[2].Text.ToString());
                        //resumenP.idPAF = int.Parse(ResultadosBusqueda.Rows[index].Cells[18].Text);
                        SPWeb app2 = SPContext.Current.Web;
                        resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);
                        Page.Session["Resumen"] = resumenP;
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
                if (e.CommandName == "PosicionCliente")
                {
                    Page.Session["IdEmpresa"] = int.Parse(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[index].Cells[0].Text));
                    Page.Session["urlAnteior"] = "RiesgoEmpresa.aspx";
                    Page.Response.Redirect("PosicionCliente.aspx");
                }
            }

        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 2)
            {
                //e.Row.Cells[0].Visible = false; //idEmpresa
                ////e.Row.Cells[1].Visible = false; //razon social
                ////e.Row.Cells[2].Visible = false; //fecha inicio act economica
                ////e.Row.Cells[3].Visible = false; //rut completo
                //e.Row.Cells[4].Visible = false; //div rut
                ////e.Row.Cells[5].Visible = false; //ejecutivo
                //e.Row.Cells[6].Visible = false; //valEmpresa
                //e.Row.Cells[7].Visible = false; //valEmpresaDetalle
                //e.Row.Cells[8].Visible = false; //valEmpresaRelacionada
                //e.Row.Cells[9].Visible = false; //ValEmpAccionista
                //e.Row.Cells[10].Visible = false; //ValEmpDirectotio
                //e.Row.Cells[11].Visible = false; //ValContacto
                //e.Row.Cells[12].Visible = false; //ValDireccion
                //e.Row.Cells[13].Visible = false; //ValBalance
                //e.Row.Cells[14].Visible = false; //ValEstado
                //e.Row.Cells[15].Visible = false; //ValCompras
                //e.Row.Cells[16].Visible = false; //ValVentas
                //e.Row.Cells[17].Visible = false; //ValScoring
                //e.Row.Cells[18].Visible = false; //id paf
                ////e.Row.Cells[19].Visible = false; //Posicion Cliente
                //e.Row.Cells[20].Visible = false; //Bandeja

                ////e.Row.Cells[21].Visible = false; //Datos Empresa
                ////e.Row.Cells[22].Visible = false; //Adicional
                ////e.Row.Cells[23].Visible = false; //Datos Operacion
                ////e.Row.Cells[24].Visible = false; //Documentos
                //e.Row.Cells[25].Visible = false; //Accion


                e.Row.Cells[0].Visible = false; //idEmpresa
                //e.Row.Cells[1].Visible = false; //razon social
                //e.Row.Cells[2].Visible = false; //fecha inicio act economica
                //e.Row.Cells[3].Visible = false; //rut completo
                e.Row.Cells[4].Visible = false; //div rut
                //e.Row.Cells[5].Visible = false; //ejecutivo
                e.Row.Cells[6].Visible = false; //valEmpresa
                e.Row.Cells[7].Visible = false; //valEmpresaDetalle
                e.Row.Cells[8].Visible = false; //valEmpresaRelacionada
                e.Row.Cells[9].Visible = false; //ValEmpAccionista
                e.Row.Cells[10].Visible = false; //ValEmpDirectotio
                e.Row.Cells[11].Visible = false; //ValContacto
                e.Row.Cells[12].Visible = false; //ValDireccion
                e.Row.Cells[13].Visible = false; //ValBalance
                e.Row.Cells[14].Visible = false; //ValEstado
                e.Row.Cells[15].Visible = false; //ValCompras
                e.Row.Cells[16].Visible = false; //ValVentas
                e.Row.Cells[17].Visible = false; //ValScoring
                //e.Row.Cells[18].Visible = false; //Posicion Cliente
                e.Row.Cells[19].Visible = false; //Bandeja

                //e.Row.Cells[20].Visible = false; //Datos Empresa
                //e.Row.Cells[21].Visible = false; //Adicional
                //e.Row.Cells[22].Visible = false; //Datos Operacion
                //e.Row.Cells[23].Visible = false; //Documentos

                if (ViewState["CARGO"].ToString() == "Administrador" || ViewState["CARGO"].ToString().Contains("Gerente"))
                {
                    e.Row.Cells[24].Visible = false; //Accion
                }
                else
                {
                    e.Row.Cells[24].Visible = false;
                    //e.Row.Cells[23].Visible = false; //documentos comerciales
                }
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
        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ResultadosBusqueda.PageIndex = e.NewPageIndex;
            CargarGrid();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string a = txtRazonSocial.Text;
            string b = txtRUT.Text;
            CargarGrid();
        }

        #endregion


        #region Metodos

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
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        #endregion
  
    }
}
