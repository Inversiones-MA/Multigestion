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
using System.Web;
using MultigestionUtilidades;

namespace MultiOperacion.wpDistribucionFondos.wpDistribucionFondos
{
    [ToolboxItemAttribute(false)]
    public partial class wpDistribucionFondos : WebPart
    {
        private static string pagina = "DistribucionFondos.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            DataTable tabla = null;
            btnAddBanco.OnClientClick = "return Dialogo();";
            btnFondoOtro.OnClientClick = "return Dialogo();";
            btnGuardarM.OnClientClick = "return Dialogo();";

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
            validar.Etapa = objresumen.area;

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        ocultarDiv();

                        tabla = LN.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));

                        if (tabla.Rows.Count > 0)
                        {
                            //lbAcreedor.Text = tabla.Rows[0]["DescAcreedor"].ToString();
                            //lbTipoProducto.Text = tabla.Rows[0]["DescProducto"].ToString();
                            ViewState["Certificado"] = tabla.Rows[0]["NCertificado"].ToString();
                            ViewState["IdAcreedor"] = tabla.Rows[0]["IdAcreedor"].ToString();
                            ViewState["Fogape"] = tabla.Rows[0]["Fogape"].ToString();

                            txtAcreedor.Text = tabla.Rows[0]["DescAcreedor"].ToString();
                            txtTyEMultiaval.Text = tabla.Rows[0]["TimbreYEstampilla"].ToString();
                            txtTyEAcreedor.Text = tabla.Rows[0]["TimbreYEstampillaAcreedor"].ToString();
                            txtTipoProducto.Text = tabla.Rows[0]["DescProducto"].ToString();
                            txtSeguroIncendio.Text = tabla.Rows[0]["costoSeguro"].ToString();
                            txtSeguroDesgravamen.Text = tabla.Rows[0]["costoSeguroDegravamen"].ToString();
                            txtNotario.Text = tabla.Rows[0]["Notario"].ToString();

                            if (tabla.Rows[0]["incluidoTimbreYEstampilla"].ToString() != "")
                            {
                                ddlTyEMultiaval.SelectedIndex = ddlTyEMultiaval.Items.IndexOf(ddlTyEMultiaval.Items.FindByValue(Convert.ToString(tabla.Rows[0]["incluidoTimbreYEstampilla"].ToString())));
                            }

                            if (tabla.Rows[0]["incluidoTimbreYEstampillaAcreedor"].ToString() != "")
                            {
                                ddlTyEAcreedor.SelectedIndex = ddlTyEAcreedor.Items.IndexOf(ddlTyEAcreedor.Items.FindByValue(Convert.ToString(tabla.Rows[0]["incluidoTimbreYEstampillaAcreedor"].ToString())));
                            }

                            if (tabla.Rows[0]["SegIncluido"].ToString() != "")
                            {
                                ddlSeguroIncendio.SelectedIndex = ddlSeguroIncendio.Items.IndexOf(ddlSeguroIncendio.Items.FindByValue(Convert.ToString(tabla.Rows[0]["SegIncluido"].ToString())));
                            }

                            if (tabla.Rows[0]["SegDesgavamenIncluido"].ToString() != "")
                            {
                                ddlSeguroDesgravamen.SelectedIndex = ddlSeguroDesgravamen.Items.IndexOf(ddlSeguroDesgravamen.Items.FindByValue(Convert.ToString(tabla.Rows[0]["SegDesgavamenIncluido"].ToString())));
                            }

                            if (tabla.Rows[0]["incluidoNotario"].ToString() != "")
                            {
                                ddlNotario.SelectedIndex = ddlNotario.Items.IndexOf(ddlNotario.Items.FindByValue(Convert.ToString(tabla.Rows[0]["incluidoNotario"].ToString())));
                            }

                            //Bloquear Elementos:
                            txtAcreedor.Enabled = false;
                            txtTyEMultiaval.Enabled = false;
                            txtTyEAcreedor.Enabled = false;
                            txtTipoProducto.Enabled = false;
                            txtSeguroIncendio.Enabled = false;
                            txtSeguroDesgravamen.Enabled = false;
                            txtNotario.Enabled = false;

                            ddlTyEMultiaval.Enabled = false;
                            ddlTyEAcreedor.Enabled = false;
                            ddlSeguroIncendio.Enabled = false;
                            ddlSeguroDesgravamen.Enabled = false;
                            ddlNotario.Enabled = false;
                        }
                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DistribucionFondos", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            //pnFormulario.Enabled = false;
                            btnGuardar.Style.Add("display", "none");
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                    mostrarDatos();
                    CargarAcreedor();
                }
                inicializacionGrillas();

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

        protected void gridDisFondoBanco_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = "Número Crédito";
                e.Row.Cells[3].Text = "Monto CLP";
                e.Row.Cells[8].Text = "";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = "CLP " + e.Row.Cells[3].Text;
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[2].Text;
                lb2.OnClientClick = "return Dialogo();";
                lb2.Command += Operaciones_Command;
                e.Row.Cells[9].Controls.Add(lb2);
            }
        }

        protected void gridDisFondoOtroBanco_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = "Número Crédito";
                e.Row.Cells[3].Text = "Monto CLP";
                e.Row.Cells[6].Text = "Acreedor";
                e.Row.Cells[8].Text = "";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Text = "CLP " + e.Row.Cells[3].Text;
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[2].Text;
                lb2.OnClientClick = "return Dialogo();";//kvsa
                lb2.Command += OperacionesOtros_Command;
                e.Row.Cells[9].Controls.Add(lb2);
            }
        }

        protected void gridDisFondoBanco_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
        }
        protected void gridDisFondoOtroBanco_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[7].Visible = false;
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DistribucionFondos");
            Page.Response.Redirect("ListarOperaciones.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //OJO Limpiar
            DataTable tabla = (DataTable)ViewState["DistribucionFondoBanco"];
            tabla.Rows.Clear();
            ViewState["DistribucionFondoBanco"] = tabla;

            tabla = (DataTable)ViewState["DistribucionFondoOtroBanco"];
            tabla.Rows.Clear();
            ViewState["DistribucionFondoOtroBanco"] = tabla;

            tabla = (DataTable)ViewState["DistribucionFondoMultiaval"];
            tabla.Rows.Clear();
            ViewState["DistribucionFondoMultiaval"] = tabla;

            Page.Session["RESUMEN"] = objresumen;
            inicializacionGrillas();
        }

        private void Operaciones_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DistribucionFondoBanco"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    if (dt1.Rows[i][2].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()))
                    {
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["DistribucionFondoBanco"] = dt1;
                gridDisFondoBanco.DataSource = (DataTable)ViewState["DistribucionFondoBanco"];
                gridDisFondoBanco.DataBind();
                dt1 = null;
            }
        }

        private void OperacionesOtros_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DistribucionFondoOtroBanco"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    if (dt1.Rows[i][2].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()))
                    {
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["DistribucionFondoOtroBanco"] = dt1;
                gridDisFondoOtroBanco.DataSource = (DataTable)ViewState["DistribucionFondoOtroBanco"];
                gridDisFondoOtroBanco.DataBind();
                dt1 = null;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();
            LN.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");

            String xmlDistFondo = generarXMLgenerarXMLDistribucionFondo();
            LogicaNegocio MTO = new LogicaNegocio();
            Boolean exito = true;
            if (objresumen.area.ToString() == "Operaciones")
            {
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    exito = MTO.InsertarDistribucionFondos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo, xmlDistFondo);
                    if (exito)
                    {
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = util.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);
                    }
                    else
                    {
                        ocultarDiv();
                        dvError.Style.Add("display", "block");
                        lbError.Text = util.buscarMensaje(Constantes.MENSAJE.ERRORGENERAL);
                    }
                }
            }
        }

        protected void btnAddBanco_Click(object sender, EventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                LogicaNegocio LN = new LogicaNegocio();
                LN.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");

                addDisFondoBanco();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
        }

        protected void btnFondoOtro_Click(object sender, EventArgs e)
        {
            try
            {
                asignacionResumen(ref objresumen);
                LogicaNegocio LN = new LogicaNegocio();
                LN.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");

                addDisFondoOtroBanco();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
        }

        protected void lbDistribucionFondos_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DistribucionFondos");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DistribucionFondos.aspx");
        }

        protected void lbDocumentoCurse_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DistribucionFondos");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("DocumentoEmision.aspx");
        }

        protected void btnAtras_Click1(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DistribucionFondos");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void GridMultiaval_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;

            //if (objresumen.idPermiso == Constantes.PERMISOS.SOLOLECTURA.ToString())
            //{
            //    e.Row.Cells[9].Visible = false;
            //}
        }

        protected void GridMultiaval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Text = "Tipo Documento";
                e.Row.Cells[4].Text = "Monto CLP";
                e.Row.Cells[8].Text = "";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Text = e.Row.Cells[4].Text;
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[2].Text + e.Row.Cells[4].Text;
                lb2.OnClientClick = "return Dialogo();";//kvsa
                lb2.Command += OperacionesMultiaval_Command;
                e.Row.Cells[9].Controls.Add(lb2);
            }
        }

        void OperacionesMultiaval_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DistribucionFondoMultiaval"];

                for (int i = 0; i <= dt1.Rows.Count - 1; i++)
                {
                    if (dt1.Rows[i][2].ToString() + dt1.Rows[i][4].ToString() == System.Web.HttpUtility.HtmlDecode(e.CommandArgument.ToString()))
                    {
                        dt1.Rows[i].Delete();
                    }
                }
                dt1.AcceptChanges();
                ViewState["DistribucionFondoMultiaval"] = dt1;
                GridMultiaval.DataSource = (DataTable)ViewState["DistribucionFondoMultiaval"]; ;
                GridMultiaval.DataBind();
                dt1 = null;
            }
        }

        protected void btnFondoMultiaval_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio LN = new LogicaNegocio();
            LN.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "03");

            addDisFondoMultiaval();
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

        private void CargarAcreedor()
        {
            LogicaNegocio ln = new LogicaNegocio();
            var dt = ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");

            util.CargaDDL(ddlAcreedor, dt, "Nombre", "IdAcreedor");

            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;

            ////Banco
            //SPList items = app.Lists["Acreedores"];
            //SPQuery query = new SPQuery();
            //query.Query = "<Where><Eq><FieldRef Name='TipoAcreedor'/><Value Type='Text'>Banco</Value></Eq></Where>";
            //SPListItemCollection collListItems = items.GetItems(query);
            //util.CargaDDL(ddlAcreedor, collListItems.GetDataTable(), "Nombre", "ID");
        }

        protected void mostrarDatos()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;

            // res = MTO.ListarResumenOpe(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString());//agregarle el idGarantia
            //lbEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();
            //lbMonto.Text = "CLP " + actualizaMiles(Math.Round(Convert.ToDecimal(res.Tables[0].Rows[0]["MontoAprobado"].ToString()), 2).ToString()).Replace(",", "");

            // inicializacionGrillas();

            res = MTO.ListarDistribucionFondos(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario, objresumen.descCargo);

            gridDisFondoBanco.DataSource = res.Tables[0];
            gridDisFondoBanco.DataBind();
            ViewState["DistribucionFondoBanco"] = res.Tables[0];

            gridDisFondoOtroBanco.DataSource = res.Tables[1];
            gridDisFondoOtroBanco.DataBind();
            ViewState["DistribucionFondoOtroBanco"] = res.Tables[1];

            GridMultiaval.DataSource = res.Tables[2];
            GridMultiaval.DataBind();
            ViewState["DistribucionFondoMultiaval"] = res.Tables[2];
        }

        protected void inicializacionGrillas()
        {
            if (ViewState["DistribucionFondoBanco"] != null)
            {
                gridDisFondoBanco.DataSource = (DataTable)ViewState["DistribucionFondoBanco"];
                gridDisFondoBanco.DataBind();
            }
            if (ViewState["DistribucionFondoOtroBanco"] != null)
            {
                gridDisFondoOtroBanco.DataSource = (DataTable)ViewState["DistribucionFondoOtroBanco"];
                gridDisFondoOtroBanco.DataBind();
            }
            if (ViewState["DistribucionFondoMultiaval"] != null)
            {
                GridMultiaval.DataSource = (DataTable)ViewState["DistribucionFondoMultiaval"];
                GridMultiaval.DataBind();
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        /// <summary>
        /// generarXMLDistribucion de Fondos
        /// </summary>
        /// <returns> xml </returns>
        string generarXMLgenerarXMLDistribucionFondo()
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

            if (ViewState["DistribucionFondoBanco"] != null)
            {
                DataTable dt;
                dt = (DataTable)ViewState["DistribucionFondoBanco"];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    RespNode = doc.CreateElement("Val");

                    //dt1.Columns.Add("NroCredito"); 2
                    XmlNode nodo2 = doc.CreateElement("NroCredito");
                    nodo2.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][2].ToString())));
                    RespNode.AppendChild(nodo2);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo3 = doc.CreateElement("MontoCredito");
                    nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][3].ToString())));
                    RespNode.AppendChild(nodo3);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo4 = doc.CreateElement("IdAcreedor");
                    nodo4.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][5].ToString())));
                    RespNode.AppendChild(nodo4);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo5 = doc.CreateElement("Acreedor");
                    nodo5.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][6].ToString())));
                    RespNode.AppendChild(nodo5);

                    //  dt1.Columns.Add("Comentario");5
                    XmlNode nodo7 = doc.CreateElement("Comentario");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][2].ToString())));
                    RespNode.AppendChild(nodo7);

                    //  dt1.Columns.Add("Comentario");5
                    nodo7 = doc.CreateElement("IdTipoFondo");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][7].ToString())));
                    RespNode.AppendChild(nodo7);

                    //  dt1.Columns.Add("Comentario");5
                    nodo7 = doc.CreateElement("DescTipoFondo");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][8].ToString())));
                    RespNode.AppendChild(nodo7);

                    //dt1.Columns.Add("Tipo"); 4
                    XmlNode nodo6 = doc.CreateElement("Tipo");
                    nodo6.AppendChild(doc.CreateTextNode("1"));
                    RespNode.AppendChild(nodo6);
                    ValoresNode.AppendChild(RespNode);
                }
            }

            if (ViewState["DistribucionFondoOtroBanco"] != null)
            {
                DataTable dt;
                dt = (DataTable)ViewState["DistribucionFondoOtroBanco"];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    RespNode = doc.CreateElement("Val");

                    //dt1.Columns.Add("NroCredito"); 2
                    XmlNode nodo2 = doc.CreateElement("NroCredito");
                    nodo2.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][2].ToString())));
                    RespNode.AppendChild(nodo2);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo3 = doc.CreateElement("MontoCredito");
                    nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][3].ToString())));
                    RespNode.AppendChild(nodo3);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo4 = doc.CreateElement("IdAcreedor");
                    nodo4.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][5].ToString())));
                    RespNode.AppendChild(nodo4);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo5 = doc.CreateElement("Acreedor");
                    nodo5.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][6].ToString())));
                    RespNode.AppendChild(nodo5);

                    //  dt1.Columns.Add("Comentario");5
                    XmlNode nodo7 = doc.CreateElement("Comentario");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][2].ToString())));
                    RespNode.AppendChild(nodo7);

                    //  dt1.Columns.Add("Comentario");5
                    nodo7 = doc.CreateElement("IdTipoFondo");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][7].ToString())));
                    RespNode.AppendChild(nodo7);

                    //  dt1.Columns.Add("Comentario");5
                    nodo7 = doc.CreateElement("DescTipoFondo");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][8].ToString())));
                    RespNode.AppendChild(nodo7);

                    //dt1.Columns.Add("Tipo"); 4
                    XmlNode nodo6 = doc.CreateElement("Tipo");
                    nodo6.AppendChild(doc.CreateTextNode("0"));
                    RespNode.AppendChild(nodo6);
                    ValoresNode.AppendChild(RespNode);
                }
            }

            if (ViewState["DistribucionFondoMultiaval"] != null)
            {
                DataTable dt;
                dt = (DataTable)ViewState["DistribucionFondoMultiaval"];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    RespNode = doc.CreateElement("Val");

                    //dt1.Columns.Add("NroCredito"); 2
                    XmlNode nodo2 = doc.CreateElement("NroCredito");
                    nodo2.AppendChild(doc.CreateTextNode(""));
                    RespNode.AppendChild(nodo2);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo3 = doc.CreateElement("MontoCredito");
                    nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][4].ToString())));
                    RespNode.AppendChild(nodo3);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo4 = doc.CreateElement("IdAcreedor");
                    nodo4.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][2].ToString())));
                    RespNode.AppendChild(nodo4);

                    //dt1.Columns.Add("MontoCredito"); 3
                    XmlNode nodo5 = doc.CreateElement("Acreedor");
                    nodo5.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][3].ToString())));
                    RespNode.AppendChild(nodo5);

                    //  dt1.Columns.Add("Comentario");5
                    XmlNode nodo7 = doc.CreateElement("Comentario");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][5].ToString())));
                    RespNode.AppendChild(nodo7);

                    //  dt1.Columns.Add("Comentario");5
                    nodo7 = doc.CreateElement("IdTipoFondo");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][7].ToString())));
                    RespNode.AppendChild(nodo7);

                    //  dt1.Columns.Add("Comentario");5
                    nodo7 = doc.CreateElement("DescTipoFondo");
                    nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(dt.Rows[i][8].ToString())));
                    RespNode.AppendChild(nodo7);

                    //dt1.Columns.Add("Tipo"); 4
                    XmlNode nodo6 = doc.CreateElement("Tipo");
                    nodo6.AppendChild(doc.CreateTextNode("2"));
                    RespNode.AppendChild(nodo6);
                    ValoresNode.AppendChild(RespNode);
                }
            }
            return doc.OuterXml;
        }

        private void addDisFondoBanco()
        {
            asignacionResumen(ref objresumen);
            if (ViewState["DistribucionFondoBanco"] == null) // si no existe el ViewState["ServiciosEmpresa"] lo creo.
            {
                DataTable dt1 = new DataTable();
                dt1.Clear();
                dt1.Columns.Add("IdDistribucionFondos");
                dt1.Columns.Add("IdOperacion");
                dt1.Columns.Add("NroCredito");
                dt1.Columns.Add("MontoCredito");
                dt1.Columns.Add("Tipo");
                dt1.Columns.Add("IdAcreedor");
                dt1.Columns.Add("DescAcreedor");
                dt1.Columns.Add("IdTipoFondo");
                dt1.Columns.Add("DescTipoFondo");
                dt1.Columns.Add("Acción");

                DataRow dr = dt1.NewRow();
                dr["IdDistribucionFondos"] = "";
                dr["IdOperacion"] = objresumen.idOperacion.ToString();
                dr["NroCredito"] = txtNroCreditoBanco.Text;
                dr["MontoCredito"] = txtMontoBanco.Text;
                dr["Tipo"] = 1;
                dr["IdAcreedor"] = ViewState["IdAcreedor"]; //ACREEDOR ACTUAL
                //dr["DescAcreedor"] = lbAcreedor.Text;//ACREEDOR ACTUAL
                dr["IdTipoFondo"] = ddlTipoFondoMB.SelectedValue;
                dr["DescTipoFondo"] = ddlTipoFondoMB.SelectedItem;

                dr["Acción"] = "";
                dt1.Rows.Add(dr);
                gridDisFondoBanco.DataSource = dt1;
                gridDisFondoBanco.DataBind();
                ViewState["DistribucionFondoBanco"] = dt1;
                dt1 = null;
            }
            else // de lo contratio edito la grilla segun sea necesario.
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DistribucionFondoBanco"];
                int ban = 0;
                for (int i = 0; i < dt1.Rows.Count && ban == 0; i++)
                {
                    if (dt1.Rows[i]["NroCredito"].ToString() == txtNroCreditoBanco.Text)
                        ban = 1;
                }
                if (ban == 0)
                {
                    DataRow dr = dt1.NewRow();
                    dr["IdDistribucionFondos"] = 0;
                    dr["IdOperacion"] = objresumen.idOperacion.ToString();
                    dr["NroCredito"] = txtNroCreditoBanco.Text;
                    dr["MontoCredito"] = txtMontoBanco.Text;
                    dr["Tipo"] = 1;
                    dr["IdAcreedor"] = ViewState["IdAcreedor"];
                    //dr["DescAcreedor"] = lbAcreedor.Text;
                    dr["IdTipoFondo"] = ddlTipoFondoMB.SelectedValue;
                    dr["DescTipoFondo"] = ddlTipoFondoMB.SelectedItem;
                    dr["Acción"] = "";

                    dt1.Rows.Add(dr);
                    gridDisFondoBanco.DataSource = dt1;
                    gridDisFondoBanco.DataBind();
                    ViewState["DistribucionFondoBanco"] = dt1;
                }
                dt1 = null;

            }

            txtNroCreditoBanco.Text = string.Empty;
            txtMontoBanco.Text = string.Empty;
        }

        private void addDisFondoOtroBanco()
        {
            asignacionResumen(ref objresumen);
            if (ViewState["DistribucionFondoOtroBanco"] == null) // si no ec}xiste el ViewState["ServiciosEmpresa"] lo creo.
            {
                DataTable dt1 = new DataTable();
                dt1.Clear();
                dt1.Columns.Add("IdDistribucionFondos");
                dt1.Columns.Add("IdOperacion");
                dt1.Columns.Add("NroCredito");
                dt1.Columns.Add("MontoCredito");
                dt1.Columns.Add("Tipo");
                dt1.Columns.Add("IdAcreedor");
                dt1.Columns.Add("DescAcreedor");
                dt1.Columns.Add("IdTipoFondo");
                dt1.Columns.Add("DescTipoFondo");
                dt1.Columns.Add("Acción");

                DataRow dr = dt1.NewRow();
                dr["IdDistribucionFondos"] = "";
                dr["IdOperacion"] = objresumen.idOperacion.ToString();
                dr["NroCredito"] = txtNroCreditoOtro.Text;
                dr["MontoCredito"] = txtMontoOtro.Text;
                dr["Tipo"] = 0;
                dr["IdAcreedor"] = ddlAcreedor.SelectedValue.ToString(); ;
                dr["DescAcreedor"] = ddlAcreedor.SelectedItem.ToString();
                dr["IdTipoFondo"] = ddlTipoFondoOB.SelectedValue;
                dr["DescTipoFondo"] = ddlTipoFondoOB.SelectedItem;
                dr["Acción"] = "";
                dt1.Rows.Add(dr);
                gridDisFondoOtroBanco.DataSource = dt1;
                gridDisFondoOtroBanco.DataBind();
                ViewState["DistribucionFondoOtroBanco"] = dt1;
                dt1 = null;
            }
            else // de lo contratio edito la grilla segun sea necesario.
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DistribucionFondoOtroBanco"];
                DataRow dr = dt1.NewRow();
                dr["IdDistribucionFondos"] = 0;
                dr["IdOperacion"] = objresumen.idOperacion.ToString();
                dr["NroCredito"] = txtNroCreditoOtro.Text;
                dr["MontoCredito"] = txtMontoOtro.Text;
                dr["Tipo"] = 0;

                dr["IdAcreedor"] = ddlAcreedor.SelectedValue.ToString(); ;
                dr["DescAcreedor"] = ddlAcreedor.SelectedItem.ToString();
                dr["IdTipoFondo"] = ddlTipoFondoOB.SelectedValue;
                dr["DescTipoFondo"] = ddlTipoFondoOB.SelectedItem;

                dr["Acción"] = "";
                dt1.Rows.Add(dr);
                gridDisFondoOtroBanco.DataSource = dt1;
                gridDisFondoOtroBanco.DataBind();
                ViewState["DistribucionFondoOtroBanco"] = dt1;
                dt1 = null;
            }

            txtNroCreditoOtro.Text = string.Empty;
            txtMontoOtro.Text = string.Empty;
        }

        private void addDisFondoMultiaval()
        {
            asignacionResumen(ref objresumen);
            if (ViewState["DistribucionFondoMultiaval"] == null) // si no ec}xiste el ViewState["ServiciosEmpresa"] lo creo.
            {
                DataTable dt1 = new DataTable();
                dt1.Clear();
                dt1.Columns.Add("IdDistribucionFondos");
                dt1.Columns.Add("IdOperacion");
                dt1.Columns.Add("IdAcreedor");
                dt1.Columns.Add("DescAcreedor");
                dt1.Columns.Add("MontoCredito");
                dt1.Columns.Add("Comentario");
                dt1.Columns.Add("Tipo");
                dt1.Columns.Add("IdTipoFondo");
                dt1.Columns.Add("DescTipoFondo");
                dt1.Columns.Add("Acción");

                DataRow dr = dt1.NewRow();
                dr["IdDistribucionFondos"] = dt1.Rows.Count;
                dr["IdOperacion"] = objresumen.idOperacion.ToString();
                dr["IdAcreedor"] = ddlTipoDoumento.SelectedValue.ToString();
                dr["DescAcreedor"] = ddlTipoDoumento.SelectedItem.ToString();
                dr["MontoCredito"] = txtMontoCLP.Text;
                dr["Comentario"] = txtComentario.Text;
                dr["Tipo"] = 2;
                dr["Acción"] = "";
                dr["IdTipoFondo"] = ddlTipoFondoM.SelectedValue;
                dr["DescTipoFondo"] = ddlTipoFondoM.SelectedItem;
                dt1.Rows.Add(dr);
                GridMultiaval.DataSource = dt1;
                GridMultiaval.DataBind();
                ViewState["DistribucionFondoMultiaval"] = dt1;
                dt1 = null;
            }
            else // de lo contratio edito la grilla segun sea necesario.
            {
                DataTable dt1 = new DataTable();
                dt1 = (DataTable)ViewState["DistribucionFondoMultiaval"];
                DataRow dr = dt1.NewRow();
                dr["IdDistribucionFondos"] = dt1.Rows.Count;
                dr["IdOperacion"] = objresumen.idOperacion.ToString();
                dr["IdAcreedor"] = ddlTipoDoumento.SelectedValue.ToString();
                dr["DescAcreedor"] = ddlTipoDoumento.SelectedItem.ToString();
                dr["MontoCredito"] = txtMontoCLP.Text;
                dr["Comentario"] = txtComentario.Text;
                dr["Tipo"] = 2;
                dr["Acción"] = "";
                dr["IdTipoFondo"] = ddlTipoFondoM.SelectedValue;
                dr["DescTipoFondo"] = ddlTipoFondoM.SelectedItem;
                dt1.Rows.Add(dr);
                GridMultiaval.DataSource = dt1;
                GridMultiaval.DataBind();
                ViewState["DistribucionFondoMultiaval"] = dt1;
                dt1 = null;
            }

            txtNroCreditoOtro.Text = string.Empty;
            txtMontoOtro.Text = string.Empty;
        }

        #endregion

    }
}
