using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using ListItem = System.Web.UI.WebControls.ListItem;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Drawing;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using MultigestionUtilidades;
using ClasesNegocio;
using Bd;

namespace MultiComercial.wpIngresoOperacion.wpIngresoOperacion
{
    [ToolboxItemAttribute(false)]
    public partial class wpIngresoOperacion : WebPart
    {
        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "MantenedorOperaciones.aspx";

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpIngresoOperacion()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }



        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            OcultarDiv();
            LogicaNegocio Ln = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            //btnGuardar.OnClientClick = "return ValidarIngresoOperacion('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + ((TextBox)(dtcFecEmis.Controls[0])).ClientID + "');";
            ((TextBox)(dtcFecEmis.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");
            ((TextBox)(dtcFecEmis.Controls[0])).MaxLength = 10;

            //PERMISOS USUARIOS
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = objresumen.area,
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    CargarProductos();
                    try
                    {
                        if (Page.Session["Resumen"] != null)
                        {
                            objresumen = (Resumen)Page.Session["Resumen"];
                            ViewState["RES"] = objresumen;
                            Page.Session["Resumen"] = null;
                            lblEmpresa.Text = objresumen.desEmpresa;

                            lbEmpresa.Text = objresumen.desEmpresa;
                            lbRut.Text = objresumen.rut;
                            LblEjecutivo.Text = objresumen.descEjecutivo;
                            LlenaMonedas();
                            string res = Ln.clienteBloquedo((int)objresumen.idEmpresa);
                            if (res != "0")
                            {
                                btnGuardar.Visible = false;
                                btnLimpiar.Visible = false;
                                pnlOperacion.Enabled = false;
                                OcultarDiv();
                                lbError.Text = "Cliente Bloqueado. Motivo: " + res + "- No se pueden generar nuevas operaciones en este estado, por favor comuniquese con el Gerente de área.";
                                dvError.Style.Add("display", "block");
                            }
                        }
                        else { Page.Response.Redirect("MensajeSession.aspx"); }
                    }
                    catch (Exception ex)
                    {
                        OcultarDiv();
                        lbError.Text = "Se produjo un error al obtener las empresas";
                        dvError.Style.Add("display", "block");
                        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    }
                }
                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;

                util.bloquear(divFormulario, dt.Rows[0]["Permiso"].ToString(), TieneFiltro);
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            ddlFinalidad.Items.Clear();

            DataTable dtFinalidad = Ln.ListarFinalidad(Convert.ToInt32(ddlProducto.SelectedItem.Value));
            util.CargaDDL(ddlFinalidad, dtFinalidad, "Nombre", "IdFinalidad");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //VALIDAR DATOS MINIMOS DE INGRESO DE OPERACION
            string mensaje = ValidarDatosOperacion();
            if (mensaje == "OK")
                Grabar();
            else
            {
                mensaje = util.Mensaje(mensaje);
                lbError.Text = mensaje;
                dvError.Style.Add("display", "block");
            }
        }

        protected void lbEdit_Click(object sender, EventArgs e)
        {
            pnlOperacion.Enabled = true;
            btnGuardar.Visible = true;
            btnLimpiar.Visible = true;
        }

        protected void StartDate_DateChanged(object sender, EventArgs e)
        {
            dtcFecEmis.MinDate = DateTime.Today.Date;
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        #endregion


        #region Metodos

        private void CargarProductos()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtProductos = Ln.ListarProducto(1);
                util.CargaDDL(ddlProducto, dtProductos, "NombreProducto", "IdProducto");
            }
            catch (Exception ex)
            {
                OcultarDiv();
                lbError.Text = "Se produjo un error al obtener los productos";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void LeeEstados()
        {
            SPWeb web = SPContext.Current.Web;
            SPList lstEstados = web.Lists["Estados"];
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        protected void Grabar()
        {
            asignacionResumen(ref objresumen);
            try
            {
                string strRes = string.Empty;
                Operaciones datosOper = new Operaciones();
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;

                SPListItemCollection items = app.Lists["Operaciones"].Items;
                SPListItem newItem = items.Add();

                // CARGA DE ESTADOS
                SPList ListaEstados = app.Lists["CambiosEstados"];
                SPQuery query = new SPQuery();
                if (objresumen.idOperacion.ToString() == "" || objresumen.idOperacion.ToString() == "0")
                    query.Query = "<Where><Eq><FieldRef Name='Orden'/><Value Type='Number'>2</Value></Eq></Where>";
                else
                    query.Query = "<Where><Eq><FieldRef Name='Orden'/><Value Type='Number'>2</Value></Eq></Where>";

                SPListItemCollection ItemsListColection = ListaEstados.GetItems(query);

                string idEtapa = "";
                string Etapa = "";
                string idEstado = "";
                string Estado = "";
                string SubEtapa = "";
                string idSubEtapa = "";
                string Area = "";

                try
                {
                    newItem["IdEmpresa"] = objresumen.idEmpresa;
                    newItem["IdProducto"] = Convert.ToInt32(ddlProducto.SelectedItem.Value);
                    newItem["IdFinalidad"] = Convert.ToInt32(ddlFinalidad.SelectedItem.Value);
                    newItem["Plazo"] = txtPlazo.Text;
                    newItem["MontoOperacion"] = txtMntOper.Text.Trim().GetValorDouble();

                    foreach (SPListItem listItem in ItemsListColection)
                    {
                        idEstado = listItem["Estado"].ToString().Split(';')[0];
                        Estado = listItem["Estado"].ToString().Split('#')[1];
                        idEtapa = listItem["Etapa"].ToString().Split(';')[0];
                        Etapa = listItem["Etapa"].ToString().Split('#')[1];
                        idSubEtapa = listItem["SubEtapa"].ToString().Split(';')[0];
                        SubEtapa = listItem["SubEtapa"].ToString().Split('#')[1];
                        Area = listItem["Area"].ToString();
                    }

                    newItem["IdEstado"] = idEstado;
                    newItem["IdEtapa"] = idEtapa;
                    newItem["IdSubEtapa"] = idSubEtapa;

                    newItem.Update();
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }

                datosOper = new Operaciones();
                datosOper._idEmpresa = objresumen.idEmpresa;
                datosOper._idProducto = Convert.ToInt32(ddlProducto.SelectedValue.ToString());
                datosOper._descProducto = ddlProducto.SelectedItem.Text;
                datosOper._idFinalidad = Convert.ToInt32(ddlFinalidad.SelectedValue.ToString());
                datosOper._finalidad = ddlFinalidad.SelectedItem.Text;
                datosOper._idMoneda = Convert.ToInt32(ddlTipoM.SelectedItem.Value);
                datosOper._descMoneda = ddlTipoM.SelectedItem.Text;
                datosOper._montoOper = Convert.ToDouble(txtMntOper.Text.Replace(".", "").Replace(",", "."));
                datosOper._plazo = txtPlazo.Text;
                datosOper._idEstado = int.Parse(idEstado);
                datosOper._descEstado = Estado;
                datosOper._descArea = Area;
                datosOper._idEtapa = int.Parse(idEtapa);
                datosOper._descEtapa = Etapa;
                datosOper._idSubEtapa = int.Parse(idSubEtapa);
                datosOper._descSubEtapa = SubEtapa;
                datosOper._fecEmis = dtcFecEmis.SelectedDate;
                datosOper._idSH = newItem.ID;

                using (SPWeb oWeb = SPContext.Current.Site.OpenWeb(app.Url))
                {
                    SPUser oUser = SPContext.Current.Web.CurrentUser;
                    strRes = datosOper.IngresaActualizaOperaciones(datosOper, "01", oUser.Name);
                }

                if (strRes.Contains("-"))
                {
                    OcultarDiv();
                    lbError.Text = strRes;
                    dvError.Style.Add("display", "block");
                }
                else
                {
                    if (!string.IsNullOrEmpty(strRes))
                    {
                        app = SPContext.Current.Web;
                        app.AllowUnsafeUpdates = true;
                        SPListItem spli = app.Lists["Operaciones"].GetItemById(newItem.ID);
                        spli["IdSQL"] = Convert.ToInt32(strRes);
                        spli.Update();
                    }

                    limpiarControles();
                    Page.Response.Redirect(objresumen.linkPrincial, false);
                }
            }
            catch (Exception ex)
            {
                OcultarDiv();
                lbError.Text = "Se ha producido un error al crear la Operación";
                dvError.Attributes.Add("style", "display:block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public void OcultarDiv()
        {
            dvError.Style.Add("display", "none");
            dvExito.Style.Add("display", "none");
            dvInfo.Style.Add("display", "none");
            dvWarning.Style.Add("display", "none");
            dvCustom.Style.Add("display", "none");
        }

        protected void limpiarControles()
        {
            ddlProducto.SelectedIndex = 0;
            ddlFinalidad.SelectedIndex = 0;
            ddlTipoM.SelectedIndex = 0;
            txtMntOper.Text = "";
            txtPlazo.Text = "";
            dtcFecEmis.ClearSelection();
        }

        public string ValidarDatosOperacion()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Operaciones datosOper = new Operaciones();

            datosOper._descProducto = ddlProducto.SelectedItem.Text;
            datosOper._descMoneda = ddlTipoM.SelectedItem.Text;
            if (ddlFinalidad.Items.Count == 0)
                datosOper._finalidad = "";
            else
                datosOper._finalidad = ddlFinalidad.SelectedItem.Text;
            datosOper._plazo = txtPlazo.Text;
            if (txtMntOper.Text == "")
                datosOper._montoOper = 0;
            else
                datosOper._montoOper = Convert.ToDouble(txtMntOper.Text.Replace(".", "").Replace(",", "."));
            if (((TextBox)(dtcFecEmis.Controls[0])).Text == "")
                datosOper._fecEmis = null;
            else
                datosOper._fecEmis = dtcFecEmis.SelectedDate;

            return datosOper.ValidarIngresoOperacion(datosOper);
        }

        protected void LlenaMonedas()
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dtTipoMoneda = Ln.ListarMonedas();
                util.CargaDDL(ddlTipoM, dtTipoMoneda, "Abreviacion", "IdMoneda");
            }
            catch (Exception ex)
            {
                OcultarDiv();
                lbError.Text = "Se produjo un error al obtener los tipos de Moneda";
                dvError.Style.Add("display", "block");
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        #endregion

    }
}


