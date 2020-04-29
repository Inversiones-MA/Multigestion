using FrameworkIntercapIT.Utilities;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Web;
using Bd;
using ClasesNegocio;

namespace MultiComercial.wpDeudaSBIF.wpDeudaSBIF
{
    [ToolboxItemAttribute(false)]
    public partial class wpDeudaSBIF : WebPart
    {

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpDeudaSBIF()
        {
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        int i, j;
        private static string pagina = "EmpresaDeudaSBIF.aspx";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            ////PERMISOS USUARIOS
            string PermisoConfigurado = string.Empty;
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
                    asignacionesJS();
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;
                        ViewState["RES"] = objresumen;
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        ocultarDiv();

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "DeudaSBIF", "Empresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnLimpiarCD.Style.Add("display", "none");
                            btnGuardarCD.Style.Add("display", "none");
                            btnLimpiarID.Style.Add("display", "none");
                            btnGuardarID.Style.Add("display", "none");
                            pnFormulario.Enabled = false;
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                }
                else
                {
                    ocultarDiv();
                }
                llenargridID();
                llenargridCD();

                validar.Permiso = dt.Rows[0]["Permiso"].ToString();
                ViewState["validar"] = validar;
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

        protected void ResultadosBusquedaCD_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            
            if (e.CommandName == "Eliminar")
            {
                int idEmpresaCreditoDisponible = Convert.ToInt32(e.CommandArgument.ToString());
                if (Ln.EliminarCreditoDispxEmpresa(idEmpresaCreditoDisponible, objresumen.idUsuario))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Se ha eliminado correctamente el crédito";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "No se ha eliminado el crédito. Contáctese con el administrador del sitio";
                }
                llenargridCD();
                //LimpiarCampos();
            }

            if (e.CommandName == "Editar")
            {
                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusquedaCD.Rows[index].CssClass = ("alert alert-danger");
                txt_instFinancieraCD.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaCD.Rows[index].Cells[1].Text);
                txt_montoDispDirecta.Text = ResultadosBusquedaCD.Rows[index].Cells[2].Text;
                txt_montoDispIndirecta.Text = ResultadosBusquedaCD.Rows[index].Cells[3].Text;
                hdnIdCreditoDisponible.Value = ResultadosBusquedaCD.Rows[index].Cells[0].Text;
                txt_instFinancieraCD.Focus();
            }
        }

        protected void ResultadosBusquedaID_Command(object sender, CommandEventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            if (e.CommandName == "Eliminar")
            {
                int idEmpresaInfDeuda = Convert.ToInt32(e.CommandArgument.ToString());
                if (Ln.EliminarDeudaSBIFxEmpresa(idEmpresaInfDeuda, objresumen.idUsuario))
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Se ha eliminado correctamente la información de la deuda";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "No se ha eliminado la información de la deuda. Contáctese con el administrador del sitio";
                }
                llenargridID();
            }

            if (e.CommandName == "Editar")
            {
                ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                int index = Convert.ToInt32(e.CommandArgument);
                ResultadosBusquedaID.Rows[index].CssClass = ("alert alert-danger");
                txt_instFinancieraID.Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaID.Rows[index].Cells[3].Text);
                ddlTipoDeuda.SelectedValue = ResultadosBusquedaID.Rows[index].Cells[1].Text;
                txt_MontoVigente.Text = ResultadosBusquedaID.Rows[index].Cells[4].Text;
                txt_Mora_30_90.Text = ResultadosBusquedaID.Rows[index].Cells[5].Text;
                txt_Mora90Mas.Text = ResultadosBusquedaID.Rows[index].Cells[6].Text;
                hdnIdInformacionDeuda.Value = ResultadosBusquedaID.Rows[index].Cells[0].Text;
                txt_instFinancieraID.Focus();
            }
        }

        protected void lbProrrateo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ProrrateoGarantias.aspx");
        }

        protected void lbDeudaSBIF_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaDeudaSBIF.aspx");
        }

        protected void lbClientes_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaClientes.aspx");
        }

        protected void lbAdministracion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaAdministracion.aspx");
        }

        protected void lbProveedores_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaProveedoaspx");
        }

        protected void btnCancelarCD_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnLimpiarCD_Click(object sender, EventArgs e)
        {
            LimpiarCamposCD();
        }

        protected void btnGuardarCD_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                string montoDispDirecta = txt_montoDispDirecta.Text.Replace(".", "");
                string montoDispIndirecta = txt_montoDispIndirecta.Text.Replace(".", "");
                LogicaNegocio Ln = new LogicaNegocio();
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    if (Ln.InsertarCreditoDispxEmpresa(objresumen.idEmpresa, txt_instFinancieraCD.Text, montoDispDirecta, montoDispIndirecta, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha insertado el crédito disponible con éxito";
                        LimpiarCamposCD();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "El crédito disponible no se ha insertado correctamente";
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {
                    if (Ln.ModificarCreditoDispxEmpresa(int.Parse(hdnIdCreditoDisponible.Value), txt_instFinancieraCD.Text, montoDispDirecta, montoDispIndirecta, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha modificado el crédito disponible correctamente";
                        LimpiarCamposCD();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "No se ha completado la modificación del crédito disponible";
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            llenargridCD();
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
        }

        protected void ResultadosBusquedaCD_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
        }

        protected void ResultadosBusquedaCD_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Cells[4].Text = actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[4].Text), 2).ToString().Replace(".", ","));
                //e.Row.Cells[2].Text = actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[2].Text), 0).ToString()).Replace(",", "");
                //e.Row.Cells[2].Text = e.Row.Cells[2].Text + "-" + e.Row.Cells[7].Text;
            }
            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = j.ToString();
                j++;
                lb.OnClientClick = "return Dialogo();";
                lb.Command += ResultadosBusquedaCD_Command;
                e.Row.Cells[4].Text = "";
                e.Row.Cells[4].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.OnClientClick = "return Dialogo();";

                lb2.Command += ResultadosBusquedaCD_Command;

                e.Row.Cells[4].Controls.Add(lb2);
            }
        }

        protected void ResultadosBusquedaCD_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            asignacionResumen(ref objresumen);
            ResultadosBusquedaCD.PageIndex = e.NewPageIndex;
            llenargridCD();
        }

        protected void btnLimpiarID_Click(object sender, EventArgs e)
        {
            LimpiarCamposID();
        }

        protected void btnCancelarID_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnGuardarID_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            try
            {
                string montoVigente = txt_MontoVigente.Text.Replace(".", "");
                string mora3090 = txt_Mora_30_90.Text.Replace(".", "");
                string mora90mas = txt_Mora90Mas.Text.Replace(".", "");

                LogicaNegocio Ln = new LogicaNegocio();
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                {
                    if (Ln.InsertarDeudaSBIFxEmpresa(objresumen.idEmpresa, ddlTipoDeuda.SelectedItem.Value, ddlTipoDeuda.SelectedItem.Text, txt_instFinancieraID.Text, montoVigente, mora3090, mora90mas, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha insertado la información de la deuda con éxito";
                        LimpiarCamposID();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "La información de la deuda no se ha insertado correctamente";
                    }
                }
                if (ViewState["ACCION"].ToString() == Constantes.OPCION.MODIFICAR)
                {
                    if (Ln.ModificarDeudaSBIFxEmpresa(int.Parse(hdnIdInformacionDeuda.Value), ddlTipoDeuda.SelectedItem.Value, ddlTipoDeuda.SelectedItem.Text, txt_instFinancieraID.Text, montoVigente, mora3090, mora90mas, objresumen.idUsuario))
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = "Se ha modificado la información de la deuda correctamente";
                        LimpiarCamposID();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = "No se ha completado la modificación de la información de la deuda";
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            llenargridID();
            ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
        }

        protected void btnAtrasID_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void ResultadosBusquedaID_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
        }

        int totalVigente = 0, totalMora30_90 = 0, totalMora90 = 0, Totales = 0;
        protected void ResultadosBusquedaID_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totalVigente += int.Parse(e.Row.Cells[4].Text.Replace(".", ""));
                totalMora30_90 += int.Parse(e.Row.Cells[5].Text.Replace(".", ""));
                totalMora90 += int.Parse(e.Row.Cells[6].Text.Replace(".", ""));
                Totales += int.Parse(e.Row.Cells[7].Text.Replace(".", ""));
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = "Totales";
                e.Row.Cells[4].Text = totalVigente.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                e.Row.Cells[5].Text = totalMora30_90.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                e.Row.Cells[6].Text = totalMora90.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                e.Row.Cells[7].Text = Totales.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                //e.Row.Cells[8].Text = "";
            }

            if (e.Row.RowIndex > -1)
            {
                LinkButton lb = new LinkButton();
                lb.CssClass = ("fa fa-edit paddingIconos");
                lb.CommandName = "Editar";
                lb.CommandArgument = i.ToString();
                i++;
                lb.Command += ResultadosBusquedaID_Command;
                e.Row.Cells[8].Text = "";
                e.Row.Cells[8].Controls.Add(lb);

                LinkButton lb2 = new LinkButton();
                lb2.CssClass = ("fa fa-close");
                lb2.CommandName = "Eliminar";
                lb2.CommandArgument = e.Row.Cells[0].Text;
                lb2.Command += ResultadosBusquedaID_Command;

                e.Row.Cells[8].Controls.Add(lb2);
            }
            //else { e.Row.Cells[8].Text = "Acción"; }
        }

        protected void ResultadosBusquedaID_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            asignacionResumen(ref objresumen);
            ResultadosBusquedaID.PageIndex = e.NewPageIndex;
            llenargridID();
        }

        protected void btnVolverCD_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaDeudaSBIF");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void ResultadosBusquedaCD_DataBound(object sender, EventArgs e)
        {
            int totalMontoDispDirecta = 0, totalMontoDispIndirecta = 0;
            for (int i = 0; i < ResultadosBusquedaCD.Rows.Count; i++)
            {
                totalMontoDispDirecta += int.Parse(ResultadosBusquedaCD.Rows[i].Cells[2].Text.Replace(".", ""));
                totalMontoDispIndirecta += int.Parse(ResultadosBusquedaCD.Rows[i].Cells[3].Text.Replace(".", ""));
            }
            if (ResultadosBusquedaCD.Rows.Count > 0)
            {
                ResultadosBusquedaCD.FooterRow.Cells[1].Text = "Totales";
                ResultadosBusquedaCD.FooterRow.Cells[2].Text = totalMontoDispDirecta.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                ResultadosBusquedaCD.FooterRow.Cells[3].Text = totalMontoDispIndirecta.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
            }
        }

        #endregion


        #region Metodos

        private void asignacionesJS()
        {
            btnGuardarID.OnClientClick = "return validarEmpresaInfDeuda('" + btnGuardarID.ClientID.Substring(0, btnGuardarID.ClientID.Length - 12) + "');";
            btnGuardarCD.OnClientClick = "return validarEmpresaCredDisponible('" + btnGuardarCD.ClientID.Substring(0, btnGuardarCD.ClientID.Length - 12) + "');";
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("Mensaje.aspx");
        }

        private void llenargridID()
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable res;
                res = Ln.ListarDeudaSBIFxEmpresa(objresumen.idEmpresa);
                ResultadosBusquedaID.DataSource = res;
                ResultadosBusquedaID.DataBind();
                i = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        private void llenargridCD()
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable res;
                res = Ln.ListarCreditoDispxEmpresa(objresumen.idEmpresa);
                ResultadosBusquedaCD.DataSource = res;
                ResultadosBusquedaCD.DataBind();
                j = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void LimpiarCamposCD()
        {
            txt_instFinancieraCD.Text = String.Empty;
            txt_montoDispDirecta.Text = String.Empty;
            txt_montoDispIndirecta.Text = String.Empty;
            hdnIdCreditoDisponible.Value = String.Empty;
        }

        private void LimpiarCamposID()
        {
            txt_MontoVigente.Text = String.Empty;
            txt_instFinancieraID.Text = String.Empty;
            txt_Mora_30_90.Text = String.Empty;
            txt_Mora90Mas.Text = String.Empty;
            ddlTipoDeuda.SelectedValue = "0";
            hdnIdInformacionDeuda.Value = String.Empty;
        }

        #endregion
        
    }
}
