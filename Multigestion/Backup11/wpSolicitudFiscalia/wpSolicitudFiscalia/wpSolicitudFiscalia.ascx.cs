using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using FrameworkIntercapIT.Utilities;
using System.Xml;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using System.Globalization;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;

namespace MultiFiscalia.wpSolicitudFiscalia.wpSolicitudFiscalia
{
    [ToolboxItemAttribute(false)]
    public partial class wpSolicitudFiscalia : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpSolicitudFiscalia()
        {
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "SolicitudFiscalia.aspx";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

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
                //validar que empresa tenga una persona con el cargo: Mandatario Judicial
                string ValidarMandatario = LN.ValidarMandatarioJudicial(objresumen.idEmpresa, objresumen.idOperacion, objresumen.desEmpresa);
                if (ValidarMandatario == "OK")
                {
                    ocultarDiv();
                    if (!Page.IsPostBack)
                    {
                        Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "SolicitudFiscalia", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            warning("Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.");
                            dvFormulario.Style.Add("display", "none");
                            btnGuardar.Style.Add("display", "none");
                        }

                        try
                        {
                            mostrarDatos();
                        }
                        catch (Exception ex)
                        {
                            LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                        }
                    }
                    inicializacionGrillas();

                    validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                    Control divFormulario = this.FindControl("dvFormulario");
                    bool TieneFiltro = true;

                    if (divFormulario != null)
                        util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
                    else
                    {
                        dvFormulario.Style.Add("display", "none");
                        warning("Usuario sin permisos configurados");
                    }
                }
                else
                    warning(ValidarMandatario);
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                warning("Usuario sin permisos configurados");
            }
        }

        protected void gridGarantias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = "CLP " + util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[3].Text), 2).ToString().Replace(".", ","));
                e.Row.Cells[4].Text = "CLP " + util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 2).ToString().Replace(".", ","));
            }
        }

        protected void tbServiciosOperaciones1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
        }

        protected void tbServiciosOperaciones1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                //lb2.OnClientClick = "return Dialogo();";kvsa
                lb2.Command += Operaciones_Command;
                e.Row.Cells[2].Controls.Add(lb2);
            }
        }

        protected void Operaciones_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["ServiciosEmpresa"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    if (dt1.Rows[i][0].ToString() == e.CommandArgument.ToString())
                    {
                        FiscaliaCostos(dt1.Rows[i]["Nombre"].ToString(), dt1.Rows[i]["ID"].ToString(), false, false);
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["ServiciosEmpresa"] = dt1;
                tbServiciosOperaciones1.DataSource = (DataTable)ViewState["ServiciosEmpresa"]; ;
                tbServiciosOperaciones1.DataBind();
                dt1 = null;

            }
        }

        protected void tbServiciosGarantia1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
        }

        protected void tbServiciosGarantia1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-close");
                lb.CommandName = "Eliminar";
                lb.CommandArgument = e.Row.Cells[0].Text + "|" + e.Row.Cells[2].Text;
                lb.OnClientClick = "return Dialogo();";
                lb.Command += Garantia_Command;
                e.Row.Cells[3].Controls.Add(lb);
            }
        }

        protected void Garantia_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["ServiciosGarantia"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    //   ckb.ID = System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[0];
                    if (dt1.Rows[i][0].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[0] && dt1.Rows[i][2].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()).Split('|')[1])
                    {
                        FiscaliaCostos(dt1.Rows[i]["Nombre"].ToString(), dt1.Rows[i]["ID"].ToString(), false, false);
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["ServiciosGarantia"] = dt1;
                tbServiciosGarantia1.DataSource = (DataTable)ViewState["ServiciosGarantia"]; ;
                tbServiciosGarantia1.DataBind();
                dt1 = null;
            }
        }


        protected void gridFiscalia_RowCreated(object sender, GridViewRowEventArgs e)
        { }

        protected void gridFiscalia_RowDataBound(object sender, GridViewRowEventArgs e)
        { }

        protected void gridFiscalia_DataBound(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)ViewState["ServicioFiscalia"];
            float totalServicios = 0;
            for (int i = 0; i <= gridFiscalia.Rows.Count - 1; i++)
            {
                (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt1.Rows[i][3].ToString()) != "" ? System.Web.HttpUtility.HtmlDecode(dt1.Rows[i][3].ToString()) : "0";
                (gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).Text = (int.Parse(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Text)) * int.Parse(System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[i].Cells[2].Text))).ToString();
                totalServicios = totalServicios + (int.Parse(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).Text)));

                //CeldaEvento Celda Accion 1=suma
                (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Attributes.Add("onFocus", "return vaciarCampo('" + (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).ClientID +
                    "','" + (gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).ClientID + "','" + (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).ClientID + "','" + "1" + "');");

                (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Attributes.Add("onblur", "return SumatoriaVacido('" + (gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).ClientID +
                    "','" + (gridFiscalia.Rows[i].FindControl("txtTotal") as TextBox).ClientID + "','" + (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).ClientID + "','" + "1" + "','" + (System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[i].Cells[2].Text)) + "');");

            }
            (gridFiscalia.FooterRow.FindControl("txtTotalServicios") as TextBox).Text = totalServicios.ToString();
            dt1 = null;
        }

        protected void btnAddServicioGarantia_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            MTO.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "02");

            garantiaServicio();
        }

        protected void btnAddServiciosEmpresa_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            MTO.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "02");
            empresaServicio();
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "SolicitudFiscalia");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            DataTable tabla = (DataTable)ViewState["ServiciosGarantia"];
            tabla.Rows.Clear();
            ViewState["ServiciosGarantia"] = tabla;
            tabla = (DataTable)ViewState["ServiciosEmpresa"];
            tabla.Rows.Clear();
            ViewState["ServiciosEmpresa"] = tabla;
            if (ViewState["ServiciosFiscalia"] != null)
            {
                tabla = (DataTable)ViewState["ServiciosFiscalia"];
                tabla.Rows.Clear();
                ViewState["ServiciosFiscalia"] = tabla;
            }
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("SolicitudFiscalia.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "02");

            String Comentario = string.Empty;
            String xmlEmpres = generarXMLServiciosOperacion();
            String XMLComentario = string.Empty;
            String xmlGarantia = generarXMLServiciosGarantia();

            Boolean exito = true;
            if (objresumen.area.ToString() == "Comercial" && tbServiciosGarantia1.Rows.Count > 0 && tbServiciosOperaciones1.Rows.Count > 0)
            {
                {
                    exito = Ln.InsertarSolicitudFiscaliaOPE(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), xmlEmpres, xmlGarantia, txtComentarios.Text.ToString(), "", objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), "1");
                    if (exito)
                    {
                        //Ok(Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT));

                        if (Ln.InsertarActualizarEstados(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                        objresumen.idUsuario, objresumen.descCargo, "6", "Fiscalia", "17", "Aprobación Servicios", "1", "Ingresado", "Fiscalia", txtComentarios.Text.ToString()))
                        {
                            Ln.ActualizarEstado(objresumen.idOperacion.ToString(), "6", "Fiscalia", "17", "Aprobación Servicios", "1", "Ingresado", "Fiscalia");
                            Ok(Ln.buscarMensaje(Constantes.MENSAJE.EXITOINSERT));
                        }
                        else
                            Error(Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));

                        Page.Response.Redirect(objresumen.linkPrincial);
                    }
                    else
                        Error(Ln.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL));
                }
            }
            else
                Error(Ln.buscarMensaje(Constantes.MENSAJE.FISCINCOMPLETOS));
        }


        private void Error(string texto)
        {
            dvError.Style.Add("display", "block");
            lbError.Text = texto.Trim();
        }

        private void Ok(string texto)
        {
            dvSuccess.Style.Add("display", "block");
            lbSuccess.Text = texto.Trim();
        }


        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
            {
                dvFormulario.Style.Add("display", "none");
                warning("Error al cargar el formulario, intente nuevamente");
            }
        }

        private void warning(string mensaje)
        {
            dvWarning.Style.Add("display", "block");
            lbWarning.Text = mensaje;
        }

        protected void mostrarDatos()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();
            DataSet res;

            res = LN.ListarResumenOpe(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString());//agregarle el idGarantia
            //res = MTO.ListarResumen("86", "56");

            lblRut.Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(res.Tables[0].Rows[0]["Rut"].ToString()), 0).ToString()).Replace(",", "") + '-' + res.Tables[0].Rows[0]["DivRut"].ToString();
            lblNombre.Text = res.Tables[0].Rows[0]["RazonSocial"].ToString();
            lbTipo.Text = res.Tables[0].Rows[0]["DescActividad"].ToString();
            lbEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();
            if (res.Tables[0].Rows[0]["MontoAprobado"].ToString() != "")
                lbMonto.Text = "UF " + float.Parse(res.Tables[0].Rows[0]["MontoAprobado"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));
            lbPAF.Text = res.Tables[0].Rows[0]["NroPAF"].ToString();
            lbOperacion.Text = objresumen.idOperacion.ToString();
            lbComentarioComite.Text = res.Tables[0].Rows[0]["ObservacionComite"].ToString();
            gridGarantias.DataSource = res.Tables[2];
            gridGarantias.DataBind();

            servicios();
            garantias();
            ViewState["Operaciones"] = res.Tables[1];
            garantiasExistentes(res.Tables[2]);

            res = LN.ListarServicios(objresumen.idEmpresa.ToString(), objresumen.idPAF.ToString(), objresumen.idOperacion.ToString());//agregarle el idPAF

            if (res.Tables.Count > 1)
            {
                ViewState["ServiciosEmpresa"] = res.Tables[1];
                ViewState["ServiciosGarantia"] = res.Tables[0];
                ViewState["ServicioFiscalia"] = res.Tables[2];
            }
        }

        protected void inicializacionGrillas()
        {
            asignacionResumen(ref objresumen);
            try
            {
                if (ViewState["ServiciosEmpresa"] != null)
                {
                    tbServiciosOperaciones1.DataSource = (DataTable)ViewState["ServiciosEmpresa"]; ;
                    tbServiciosOperaciones1.DataBind();
                }
                if (ViewState["ServiciosGarantia"] != null)
                {
                    tbServiciosGarantia1.DataSource = (DataTable)ViewState["ServiciosGarantia"]; ;
                    tbServiciosGarantia1.DataBind();
                }
                if (ViewState["ServicioFiscalia"] != null)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["ServicioFiscalia"];

                    for (int i = 0; i <= gridFiscalia.Rows.Count - 1; i++)
                    {
                        dt1.Rows[i]["Valor"] = System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[i].FindControl("txtValor") as TextBox).Text);

                    }
                    ViewState["ServicioFiscalia"] = dt1;
                    dt1 = null;

                    gridFiscalia.DataSource = (DataTable)ViewState["ServicioFiscalia"];
                    gridFiscalia.DataBind();
                }
            }

            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void garantiasExistentes(DataTable dtServiciosGarantia)
        {
            util.CargaDDL(ddlServicioEmpresa, dtServiciosGarantia, "Nro. Garantía", "Nro. Garantía");
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        private void servicios()
        {
            DataTable dt = new DataTable();
            DataTable dtServiciosOperacion = new DataTable();
            dt = ServiciosOperacion();
            util.CargaDDL(ddlServicioEmpresa, dt, "Nombre", "ID");
        }

        protected DataTable ServiciosOperacion()
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList items = app.Lists["ServiciosOperacion"];
                SPQuery query = new SPQuery();
                query.Query = "<Where><Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq></Where>" +
                                "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                SPListItemCollection collListItems = items.GetItems(query);
                return collListItems.GetDataTable();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        private void garantias()
        {
            DataTable dtServiciosEmpresa = new DataTable();
            dtServiciosEmpresa = ServiciosGarantia("1");
            ListItem LstItem;
            ddlServiciosGarantia.DataSource = dtServiciosEmpresa;
            ddlServiciosGarantia.DataTextField = "Nombre";
            ddlServiciosGarantia.DataValueField = "ID";
            ddlServiciosGarantia.DataBind();
            LstItem = new ListItem("Seleccione", "0");
            ddlServiciosGarantia.Items.Insert(0, LstItem);
        }

        protected DataTable ServiciosGarantia(string Tipo)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList items = app.Lists["ServiciosGarantia"];
                SPQuery query = new SPQuery();
                query.Query = "<Where><And><Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq><Eq><FieldRef Name='Tipo'/><Value Type='TEXT'>" + Tipo + "</Value></Eq></And></Where>" +
                                "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                SPListItemCollection collListItems = items.GetItems(query);
                return collListItems.GetDataTable();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        /// <summary>
        /// FiscaliaCostos
        /// </summary>
        /// <param name="servicio"> nombre del servicio</param>
        /// <param name="idServicios"> id del servicio</param>
        /// <param name="agrega"> agregar nuevos parametros a la grilla de fiscalia</param>
        /// <param name="suma"> indica cuando modificar(suma - resta al campo Cantidad) o eliminar parametros del grid de fiscalia, </param>
        private void FiscaliaCostos(string servicio, string idServicios, Boolean agrega, Boolean suma)
        {
            asignacionResumen(ref objresumen);
            DataTable dt1 = new DataTable();
            //si no existe el viewstate ServicioFiscalia, crea la tabla y agrega valor
            if (ViewState["ServicioFiscalia"] == null)
            {
                dt1.Clear();
                dt1.Columns.Add("IdServicio");
                dt1.Columns.Add("TipoServicio");
                dt1.Columns.Add("Cantidad");
                dt1.Columns.Add("Valor");
                dt1.Columns.Add("Total");
                dt1.Columns.Add("Critico");

                DataRow dr = dt1.NewRow();
                dr["IdServicio"] = idServicios;
                dr["TipoServicio"] = servicio;
                dr["Cantidad"] = 1;
                dr["Valor"] = 0;
                dr["Total"] = 0;
                dr["Critico"] = 0;

                dt1.Rows.Add(dr);
            }
            else // de lo contrario modifica la tabla existente.
            {
                dt1 = (DataTable)ViewState["ServicioFiscalia"];

                // si agrega, siempre es uno nuevo por lo tanto la cantidad es 1
                if (agrega)
                {
                    DataRow dr = dt1.NewRow();
                    dr["IdServicio"] = idServicios;
                    dr["TipoServicio"] = servicio;
                    dr["Cantidad"] = 1;
                    dr["Valor"] = 0;
                    dr["Total"] = 0;
                    dr["Critico"] = 0;
                    dt1.Rows.Add(dr);
                }
                // si no est agregando pero si sumando, a cantidad se suma 1
                if (!agrega && suma)
                {
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][1].ToString() == servicio && dt1.Rows[i][0].ToString() == idServicios)
                        {
                            dt1.Rows[i]["Cantidad"] = int.Parse(dt1.Rows[i]["Cantidad"].ToString()) + 1;
                        }
                    }
                }
                //si no esta agregando y tampoco sumando a la cantidad le resto 1 y si e}hay solo uno, elimino el que está.
                if (!agrega && !suma)
                {
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][1].ToString() == servicio && dt1.Rows[i][0].ToString() == idServicios)
                        {
                            if (dt1.Rows[i]["Cantidad"].ToString() == "1")
                            {
                                dt1.Rows[i].Delete();
                            }
                            else
                            {
                                dt1.Rows[i]["Cantidad"] = int.Parse(dt1.Rows[i]["Cantidad"].ToString()) - 1;
                            }
                        }
                    }
                    dt1.AcceptChanges();
                }


            }
            ViewState["ServicioFiscalia"] = dt1;
            gridFiscalia.DataSource = dt1;
            gridFiscalia.DataBind();
            dt1 = null;

            //odos los combos vuelven a su valor predeterminado
            ddlGarantias.SelectedIndex = 0;
            ddlServicioEmpresa.SelectedIndex = 0;
            ddlServiciosGarantia.SelectedIndex = 0;
        }


        /// <summary>
        /// garantiaServicio se encarga de agregar, modificar o eliminar los registrso correspondientes a la grilla servicios Garantia, y realiza  llamado a FiscaliaCostos en donde se hace lo mismo pero para la grilla de fiscalia
        /// </summary>
        private void garantiaServicio()
        {
            asignacionResumen(ref objresumen);
            if (ddlServiciosGarantia.SelectedValue.ToString() != "0" && ddlGarantias.SelectedValue.ToString() != "0") // si lo seleccionado en el combo de servicios es igual a "0"(Seleccione) entonces no haga NADA
            {
                if (ViewState["ServiciosGarantia"] == null)
                {
                    DataTable dt1 = new DataTable();
                    dt1.Clear();
                    dt1.Columns.Add("ID");
                    dt1.Columns.Add("Nombre");
                    dt1.Columns.Add("Garantía");
                    dt1.Columns.Add("Acción");

                    DataRow dr = dt1.NewRow();
                    dr["ID"] = ddlServiciosGarantia.SelectedValue.ToString();
                    dr["Nombre"] = ddlServiciosGarantia.SelectedItem.ToString();
                    dr["Garantía"] = ddlGarantias.SelectedValue.ToString();
                    dr["Acción"] = "";
                    dt1.Rows.Add(dr);

                    tbServiciosGarantia1.DataSource = dt1;
                    tbServiciosGarantia1.DataBind();
                    ViewState["ServiciosGarantia"] = dt1;

                    FiscaliaCostos(ddlServiciosGarantia.SelectedItem.ToString(), ddlServiciosGarantia.SelectedValue.ToString(), true, true);
                    dt1 = null;
                }
                else
                {
                    Boolean ban = true;
                    Boolean banF = true;
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["ServiciosGarantia"];
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][0].ToString() == ddlServiciosGarantia.SelectedValue.ToString() && !(dt1.Rows[i][2].ToString() == ddlGarantias.SelectedValue.ToString()))
                        {
                            banF = false;
                        }
                        if (dt1.Rows[i][0].ToString() == ddlServiciosGarantia.SelectedValue.ToString() && dt1.Rows[i][2].ToString() == ddlGarantias.SelectedValue.ToString())
                        {
                            ban = false;
                        }
                    }
                    if (ban == true)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["ID"] = ddlServiciosGarantia.SelectedValue.ToString();
                        dr["Nombre"] = ddlServiciosGarantia.SelectedItem.ToString();
                        dr["Garantía"] = ddlGarantias.SelectedValue.ToString();
                        dr["Acción"] = "";
                        dt1.Rows.Add(dr);
                        tbServiciosGarantia1.DataSource = dt1;
                        tbServiciosGarantia1.DataBind();
                        ViewState["ServiciosGarantia"] = dt1;

                        FiscaliaCostos(ddlServiciosGarantia.SelectedItem.ToString(), ddlServiciosGarantia.SelectedValue.ToString(), banF, true);
                    }
                    dt1 = null;
                }
            }
        }

        /// <summary>
        /// empresaServicio se encarga de agregar, modificar o eliminar los registrso correspondientes a la grilla servicios Operaciones, y realiza  llamado a FiscaliaCostos en donde se hace lo mismo pero para la grilla de fiscalia
        /// </summary>
        private void empresaServicio()
        {
            asignacionResumen(ref objresumen);
            if (ddlServicioEmpresa.SelectedValue.ToString() != "0")
            {
                if (ViewState["ServiciosEmpresa"] == null) // si no existe el ViewState["ServiciosEmpresa"] lo creo.
                {
                    DataTable dt1 = new DataTable();
                    dt1.Clear();
                    dt1.Columns.Add("ID");
                    dt1.Columns.Add("Nombre");
                    dt1.Columns.Add("Acción");
                    DataRow dr = dt1.NewRow();
                    dr["ID"] = ddlServicioEmpresa.SelectedValue.ToString();
                    dr["Nombre"] = ddlServicioEmpresa.SelectedItem.ToString();
                    dr["Acción"] = "";
                    dt1.Rows.Add(dr);
                    tbServiciosOperaciones1.DataSource = dt1;
                    tbServiciosOperaciones1.DataBind();
                    ViewState["ServiciosEmpresa"] = dt1;

                    FiscaliaCostos(ddlServicioEmpresa.SelectedItem.ToString(), ddlServicioEmpresa.SelectedValue.ToString(), true, true);
                    dt1 = null;
                }
                else // de lo contratio edito la grilla segun sea necesario.
                {
                    Boolean ban = true;
                    DataTable dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["ServiciosEmpresa"];
                    for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                    {
                        if (dt1.Rows[i][0].ToString() == ddlServicioEmpresa.SelectedValue.ToString())
                        {
                            ban = false;
                            break;
                        }
                    }
                    if (ban == true)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["ID"] = ddlServicioEmpresa.SelectedValue.ToString();
                        dr["Nombre"] = ddlServicioEmpresa.SelectedItem.ToString();
                        dr["Acción"] = "";
                        dt1.Rows.Add(dr);
                        tbServiciosOperaciones1.DataSource = dt1;
                        tbServiciosOperaciones1.DataBind();
                        ViewState["ServiciosEmpresa"] = dt1;
                        FiscaliaCostos(ddlServicioEmpresa.SelectedItem.ToString(), ddlServicioEmpresa.SelectedValue.ToString(), ban, true); //agrega, suma
                    }
                    dt1 = null;
                }
            }
        }

        /// <summary>
        /// generarXMLServiciosOperacion 
        /// </summary>
        /// <returns> xml </returns>
        private string generarXMLServiciosOperacion()
        {
            asignacionResumen(ref objresumen);
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            for (int i = 0; i <= tbServiciosOperaciones1.Rows.Count - 1; i++)
            {
                XmlNode RespNodeCkb;
                RespNodeCkb = doc.CreateElement("Ckb");

                XmlNode nodo = doc.CreateElement("ID");
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[0].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("Descripcion");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo1);
                for (int j = 0; j <= gridFiscalia.Rows.Count - 1; j++)
                {
                    if (System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[j].Cells[1].Text) == (System.Web.HttpUtility.HtmlDecode(tbServiciosOperaciones1.Rows[i].Cells[1].Text)))
                    {
                        XmlNode nodoC = doc.CreateElement("Costo");
                        nodoC.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[j].FindControl("txtValor") as TextBox).Text)));
                        RespNodeCkb.AppendChild(nodoC);
                        break;
                    }
                }
                RespNode.AppendChild(RespNodeCkb);
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        private string generarXMLServiciosGarantia()
        {
            asignacionResumen(ref objresumen);
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            for (int i = 0; i <= tbServiciosGarantia1.Rows.Count - 1; i++)
            {
                XmlNode RespNodeCkb;
                RespNodeCkb = doc.CreateElement("Ckb");

                XmlNode nodo = doc.CreateElement("ID");
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[0].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("Descripcion");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo1);

                XmlNode nodoG = doc.CreateElement("IdGarantia");
                nodoG.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[2].Text)));
                RespNodeCkb.AppendChild(nodoG);


                for (int j = 0; j <= gridFiscalia.Rows.Count - 1; j++)
                {
                    if (System.Web.HttpUtility.HtmlDecode(gridFiscalia.Rows[j].Cells[1].Text) == (System.Web.HttpUtility.HtmlDecode(tbServiciosGarantia1.Rows[i].Cells[1].Text)))
                    {
                        XmlNode nodoC = doc.CreateElement("Costo");
                        nodoC.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((gridFiscalia.Rows[j].FindControl("txtValor") as TextBox).Text)));
                        RespNodeCkb.AppendChild(nodoC);
                        break;
                    }
                }
                RespNode.AppendChild(RespNodeCkb);
            }
            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }


        #endregion

    }
}
