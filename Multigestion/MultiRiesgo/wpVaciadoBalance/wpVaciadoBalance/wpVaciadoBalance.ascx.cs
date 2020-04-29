using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Xml;
using System.Web.UI.HtmlControls;
using MultigestionUtilidades;
using System.Globalization;
using System.Xml.Linq;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Xsl;
using System.IO;
using ExpertPdf.HtmlToPdf;
using System.Web.UI;
using ClasesNegocio;
using Bd;

namespace MultiRiesgo.wpVaciadoBalance.wpVaciadoBalance
{
    [ToolboxItemAttribute(false)]
    public partial class wpVaciadoBalance : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpVaciadoBalance()
        {
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "VaciadoBalance.aspx";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

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
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbOperacion.Text = objresumen.desOperacion;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            pnFormulario.Enabled = false;
                        }

                        llenargrid();
                    }
                }

                string msjError = string.Empty;
                if (((ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).Text = "0"; }
                if (((ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).Text = "0"; }
                if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).Text))
                {
                    msjError = msjError + " " + (DateTime.Now.Year).ToString() + " ";
                }

                if (((ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).Text = "0"; }
                if (((ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).Text = "0"; }
                if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).Text))
                {
                    msjError = msjError + " " + (DateTime.Now.Year - 1).ToString() + "  ";
                }

                if (((ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).Text = "0"; }
                if (((ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).Text = "0"; }
                if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).Text))
                {
                    msjError = msjError + " " + (DateTime.Now.Year - 2).ToString() + " ";
                }

                if (((ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text = "0"; }
                if (((ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text = "0"; }
                if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text))
                {
                    var Fila28 = float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text);   //Total Activos
                    var Fila57 = float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text);   //TOTAL PASIVO + PATRIMONIO
                    msjError = msjError + " " + (DateTime.Now.Year - 3).ToString() + " ";
                }

                if (msjError != "")
                {
                    ocultarDiv();
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = "Balance No cuadra, revisar año(s): " + msjError;
                }

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

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
            e.Row.Cells[9].Visible = false;

            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;
            e.Row.Cells[12].Visible = false;
            e.Row.Cells[13].Visible = false;

            e.Row.Cells[14].Visible = false;//IdEmpresaDocumentoContable

            e.Row.Cells[15].Visible = false; //<asp:BoundField DataField="Validacion" HeaderText="" />
            e.Row.Cells[16].Visible = false; //<asp:BoundField DataField="IdTipoDocumento1" HeaderText="" />
            e.Row.Cells[17].Visible = false; // <asp:BoundField DataField="IdTipoDocumento2" HeaderText="" />
            e.Row.Cells[18].Visible = false; //<asp:BoundField DataField="IdTipoDocumento3" HeaderText="" />
            e.Row.Cells[19].Visible = false; //<asp:BoundField DataField="IdTipoDocumentoAct" HeaderText="" />   

            e.Row.Cells[23].Visible = false; //<asp:BoundField DataField="IdTipoDocumento3" HeaderText="" />
            e.Row.Cells[24].Visible = false; //<asp:BoundField DataField="IdTipoDocumentoAct" HeaderText="" />   

            e.Row.Cells[20].Visible = false; // ACCIONgrupo
            e.Row.Cells[21].Visible = false; // ACCIONcuenta
            e.Row.Cells[22].Visible = false; // ACCIONsubcuenta    

            e.Row.Cells[25].Visible = false; // RequeridoActual   
            e.Row.Cells[26].Visible = false; // Requerido3 
            e.Row.Cells[27].Visible = false; // Requerido2   
            e.Row.Cells[28].Visible = false; // Requerido1 


            //aca se valida inicio de actividad economica de la empresa dependiendo esa fecha creara los periodos.... formaulario registro empresa, año inicio actividad economica
            if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 3).ToString()))
            {
                e.Row.Cells[3].Style.Add("display", "none");
                if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 2).ToString()))
                {
                    e.Row.Cells[4].Style.Add("display", "none");
                    if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 1).ToString()))
                    {
                        e.Row.Cells[5].Style.Add("display", "none");
                    }
                }
            }
        }

        private void CargarDocContable()
        {

        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DropDownList cmbTipoDocumentAux = new DropDownList();
                util.CargaDDL(cmbTipoDocumentAux, Ln.ListarTipoDocumentoContable(), "Nombre", "Id");

                (e.Row.FindControl("lbValor1") as Label).Text = "Dec - " + (System.DateTime.Now.Year - 3).ToString() + "";
                (e.Row.FindControl("cbValor1") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValor1") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;
                (e.Row.FindControl("cbValor1") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValor1") as DropDownList).DataBind();

                (e.Row.FindControl("lbValor2") as Label).Text = "Dec - " + (System.DateTime.Now.Year - 2).ToString() + "";
                (e.Row.FindControl("cbValor2") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValor2") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;
                (e.Row.FindControl("cbValor2") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValor2") as DropDownList).DataBind();

                (e.Row.FindControl("lbValor3") as Label).Text = (System.DateTime.Now.Year - 1).ToString() + "";
                (e.Row.FindControl("cbValor3") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValor3") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;

                //cbMes3
                (e.Row.FindControl("cbValor3") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValor3") as DropDownList).DataBind();

                (e.Row.FindControl("lbValorAct") as Label).Text = System.DateTime.Now.Year.ToString() + "";
                (e.Row.FindControl("cbValorActual") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValorActual") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;

                //cbMesAct
                (e.Row.FindControl("cbValorActual") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValorActual") as DropDownList).DataBind();
            }
            else
            {
                btnCancelar.OnClientClick = "return Limpiar('" + (ResultadosBusqueda.HeaderRow.FindControl("cbValor1") as DropDownList).ClientID + "');";
            }
        }

        protected void ResultadosBusqueda_Command(object sender, CommandEventArgs e)
        {

        }

        protected void ResultadosBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ResultadosBusqueda_DataBound(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            int bandera = 0;
            double TotalAct1 = 0;
            double TotalAct2 = 0;
            double TotalAct3 = 0;
            double TotalActAct = 0;
            double TotalPas1 = 0;
            double TotalPas2 = 0;
            double TotalPas3 = 0;
            double TotalPasAct = 0;
            ViewState["cbValor1"] = "0";
            ViewState["cbValor2"] = "0";
            ViewState["cbValor3"] = "0";
            ViewState["cbValorActual"] = "0";
            for (int i = 0; i <= ResultadosBusqueda.Rows.Count - 1; i++)
            {
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValor1") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text);
                    ViewState["cbValor1"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValor2") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text);
                    ViewState["cbValor2"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValor3") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text);
                    ViewState["cbValor3"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValorActual") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text);
                    ViewState["cbValorActual"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[20].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("cbMes3") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[20].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[25].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequeridoActual") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[25].Text) == "False") ? "0" : "1";
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[26].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido3") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[26].Text) == "False") ? "0" : "1";
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[27].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido2") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[27].Text) == "False") ? "0" : "1";
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[28].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido1") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[28].Text) == "False") ? "0" : "1";
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[21].Text) != " ")
                {
                    (ResultadosBusqueda.HeaderRow.FindControl("cbMesAct") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[21].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[10].Text) != " ")
                {
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[10].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[11].Text) != " ")
                {
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[11].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[12].Text) != " ")
                {
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[12].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text) != "0")
                {
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text);
                }


                if (ResultadosBusqueda.Rows[i].Cells[0].Text == ResultadosBusqueda.Rows[i].Cells[1].Text)
                {//las celdas que son como encabeado de lo que viene las coloco como bloqueadas y sin eventos

                    ResultadosBusqueda.Rows[i].CssClass = "bg-warning";

                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).ReadOnly = true;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).ReadOnly = true;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).ReadOnly = true;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).ReadOnly = true;

                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "Activos" || ResultadosBusqueda.Rows[i].Cells[1].Text == "Pasivos" || ResultadosBusqueda.Rows[i].Cells[1].Text == "Patrimonio")
                    {

                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Visible = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Visible = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Visible = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Visible = false;

                    }

                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "Interes Minoritario")
                    {
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Visible = true;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Visible = true;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Visible = true;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Visible = true;

                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Enabled = true;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Enabled = true;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Enabled = true;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Enabled = true;
                    }


                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "Total Activos Circulante" || ResultadosBusqueda.Rows[i].Cells[1].Text == "Total Activos Largo Plazo")
                    {
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Enabled = false;

                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text != "")
                        { TotalAct1 = TotalAct1 + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text); }
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text != "")
                        { TotalAct2 = TotalAct2 + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text); }
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text != "")
                        { TotalAct3 = TotalAct3 + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text); }
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text != "")
                        { TotalActAct = TotalActAct + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text); }

                    }


                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "Total Activos")
                    {
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Enabled = false;

                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text = TotalAct1.ToString("F");
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text = TotalAct2.ToString("F");
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text = TotalAct3.ToString("F");
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text = TotalActAct.ToString("F"); //double.Parse(TotalActAct.ToString()).ToString();

                    }

                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "Total Pasivo Circulante" || ResultadosBusqueda.Rows[i].Cells[1].Text == "Total Pasivo Largo Plazo" ||
                         ResultadosBusqueda.Rows[i].Cells[1].Text == "Total Patrimonio")
                    {
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text != "")
                        { TotalPas1 = TotalPas1 + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text); }

                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text != "")
                        { TotalPas2 = TotalPas2 + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text); }

                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text != "")
                        { TotalPas3 = TotalPas3 + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text); }

                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text != "")
                        { TotalPasAct = TotalPasAct + double.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text); }

                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Enabled = false;
                    }
                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "TOTAL PASIVO + PATRIMONIO")
                    {
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text = double.Parse(TotalPas1.ToString()).ToString();
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text = double.Parse(TotalPas2.ToString()).ToString();
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text = double.Parse(TotalPas3.ToString()).ToString();
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text = double.Parse(TotalPasAct.ToString()).ToString();

                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Enabled = false;
                        (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Enabled = false;
                    }
                    bandera = i;
                }
            }

            for (int rowIndex = ResultadosBusqueda.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = ResultadosBusqueda.Rows[rowIndex];
                GridViewRow gvPreviousRow = ResultadosBusqueda.Rows[rowIndex + 1];
                for (int cellCount = 0; cellCount < 2; cellCount++)
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

            //ActivoCirculanteT(celActCirculante, celActLargoPlazo, celTotalActivos, celVal1, celVal2, celVal3, celVal4, celVal5,
            //                    celVal6, celVal7, celVal8)
            for (int i = 1; i <= 8; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return ActivoCirculanteT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[27].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "');");


                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                    "return ActivoCirculanteT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[27].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio2") as TextBox).ClientID + "');");



                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                    "return ActivoCirculanteT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[27].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                  "return ActivoCirculanteT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[27].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "');");

            }

            //function ActivoLargoPlazoT(celActFijos, celActCirculante, celActLargoPlazo, celTotalActivos, celVal9, celVal10, celVal11, celVal12, celVal13,
            //                    celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21, celVal22, celVal23,
            //                    celVal24, celVal25)

            for (int i = 10; i <= 26; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return ActivoLargoPlazoT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[27].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[10].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[11].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[12].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[13].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[14].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[15].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[16].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[17].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                    + (ResultadosBusqueda.Rows[18].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                       + (ResultadosBusqueda.Rows[19].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                          + (ResultadosBusqueda.Rows[20].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                             + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                                + (ResultadosBusqueda.Rows[22].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                                   + (ResultadosBusqueda.Rows[23].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                                      + (ResultadosBusqueda.Rows[24].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                                         + (ResultadosBusqueda.Rows[25].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[26].FindControl("txtAnio1") as TextBox).ClientID + "');");


                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                   "return ActivoLargoPlazoT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[27].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[10].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[11].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[12].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[13].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[14].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[15].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[16].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[17].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                   + (ResultadosBusqueda.Rows[18].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                      + (ResultadosBusqueda.Rows[19].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                         + (ResultadosBusqueda.Rows[20].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                            + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                               + (ResultadosBusqueda.Rows[22].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                                  + (ResultadosBusqueda.Rows[23].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                                     + (ResultadosBusqueda.Rows[24].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                                        + (ResultadosBusqueda.Rows[25].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[26].FindControl("txtAnio2") as TextBox).ClientID + "');");


                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                "return ActivoLargoPlazoT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[27].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[10].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[11].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[12].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[13].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[14].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[15].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[16].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[17].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[18].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                   + (ResultadosBusqueda.Rows[19].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                      + (ResultadosBusqueda.Rows[20].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                         + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                            + (ResultadosBusqueda.Rows[22].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                               + (ResultadosBusqueda.Rows[23].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                                  + (ResultadosBusqueda.Rows[24].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                                     + (ResultadosBusqueda.Rows[25].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[26].FindControl("txtAnio3") as TextBox).ClientID + "');");


                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
              "return ActivoLargoPlazoT('" + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[27].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[10].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[11].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[12].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[13].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[14].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[15].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[16].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[17].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[18].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[19].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                    + (ResultadosBusqueda.Rows[20].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                       + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                          + (ResultadosBusqueda.Rows[22].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                             + (ResultadosBusqueda.Rows[23].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                                + (ResultadosBusqueda.Rows[24].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                                   + (ResultadosBusqueda.Rows[25].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                        + (ResultadosBusqueda.Rows[26].FindControl("txtAnioA") as TextBox).ClientID + "');");



            }

            //pasivocirculante
            //PasivoCirculanteT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin,
            //celVal26, celVal27, celVal28, celVal29, celVal30,
            //                        celVal31, celVal32, celVal33, celVal34, celVal35) 
            for (int i = 30; i <= 39; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return PasivoCirculanteT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[47].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[56].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[48].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[30].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[31].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[32].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[33].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[34].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[35].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[36].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[37].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                  + (ResultadosBusqueda.Rows[38].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[39].FindControl("txtAnio1") as TextBox).ClientID + "');");



                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                    "return PasivoCirculanteT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[47].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[56].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[48].FindControl("txtAnio2") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[30].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[31].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[32].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[33].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[34].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[35].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[36].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[37].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                  + (ResultadosBusqueda.Rows[38].FindControl("txtAnio2") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[39].FindControl("txtAnio2") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                  "return PasivoCirculanteT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[47].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[56].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[48].FindControl("txtAnio3") as TextBox).ClientID + "','"

                                            + (ResultadosBusqueda.Rows[30].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[31].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[32].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[33].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[34].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[35].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[36].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[37].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[38].FindControl("txtAnio3") as TextBox).ClientID + "','"

                                            + (ResultadosBusqueda.Rows[39].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                "return PasivoCirculanteT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[47].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[56].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[48].FindControl("txtAnioA") as TextBox).ClientID + "','"

                                          + (ResultadosBusqueda.Rows[30].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[31].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[32].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[33].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[34].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[35].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[36].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[37].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[38].FindControl("txtAnioA") as TextBox).ClientID + "','"

                                          + (ResultadosBusqueda.Rows[39].FindControl("txtAnioA") as TextBox).ClientID + "');");

            }

            //PasivoLargoPlazoT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, 
            //                  celTotalPasPatrimonio, celInteresMin, celVal36, celVal37, celVal38, celVal39, celVal40, celVal41)

            for (int i = 41; i <= 46; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return PasivoLargoPlazoT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[47].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[56].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[48].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[41].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[42].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[43].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[44].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[45].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[46].FindControl("txtAnio1") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                   "return PasivoLargoPlazoT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[47].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[56].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[48].FindControl("txtAnio2") as TextBox).ClientID + "','"

                                             + (ResultadosBusqueda.Rows[41].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[42].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[43].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[44].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[45].FindControl("txtAnio2") as TextBox).ClientID + "','"

                                             + (ResultadosBusqueda.Rows[46].FindControl("txtAnio2") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                 "return PasivoLargoPlazoT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[47].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[56].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[48].FindControl("txtAnio3") as TextBox).ClientID + "','"

                                           + (ResultadosBusqueda.Rows[41].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[42].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[43].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[44].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                           + (ResultadosBusqueda.Rows[45].FindControl("txtAnio3") as TextBox).ClientID + "','"

                                           + (ResultadosBusqueda.Rows[46].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
             "return PasivoLargoPlazoT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[47].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[56].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[48].FindControl("txtAnioA") as TextBox).ClientID + "','"

                                       + (ResultadosBusqueda.Rows[41].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[42].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[43].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[44].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                       + (ResultadosBusqueda.Rows[45].FindControl("txtAnioA") as TextBox).ClientID + "','"

                                       + (ResultadosBusqueda.Rows[46].FindControl("txtAnioA") as TextBox).ClientID + "');");

            }
            //PatrimonioT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin, 
            //          celVal42, celVal43, celVal44, celVal45, celVal46,  celVal47)

            for (int i = 50; i <= 56; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return PatrimonioT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[47].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[56].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[48].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[50].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[51].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[52].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[53].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[54].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[55].FindControl("txtAnio1") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                    "return PatrimonioT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[47].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[56].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[48].FindControl("txtAnio2") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[50].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[51].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[52].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[53].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[54].FindControl("txtAnio2") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[55].FindControl("txtAnio2") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                   "return PatrimonioT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[47].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[56].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[48].FindControl("txtAnio3") as TextBox).ClientID + "','"

                                             + (ResultadosBusqueda.Rows[50].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[51].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[52].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[53].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                             + (ResultadosBusqueda.Rows[54].FindControl("txtAnio3") as TextBox).ClientID + "','"

                                             + (ResultadosBusqueda.Rows[55].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                "return PatrimonioT('" + (ResultadosBusqueda.Rows[40].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[47].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[56].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[48].FindControl("txtAnioA") as TextBox).ClientID + "','"

                                          + (ResultadosBusqueda.Rows[50].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[51].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[52].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[53].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                          + (ResultadosBusqueda.Rows[54].FindControl("txtAnioA") as TextBox).ClientID + "','"

                                          + (ResultadosBusqueda.Rows[55].FindControl("txtAnioA") as TextBox).ClientID + "');");

            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
          
            Boolean exito;
            string xmlData = generarXML();
            string mensaje = string.Empty;
            Boolean res;
            if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) < int.Parse((DateTime.Now.Year - 3).ToString()))
            {
                res = xmlData.Contains((DateTime.Now.Year - 3).ToString());
                if (!res)
                    mensaje = mensaje + (DateTime.Now.Year - 3).ToString() + "  ";
                if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) < int.Parse((DateTime.Now.Year - 2).ToString()))
                {
                    res = xmlData.Contains((DateTime.Now.Year - 2).ToString());
                    if (!res)
                        mensaje = mensaje + (DateTime.Now.Year - 2).ToString() + "  ";
                    if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) < int.Parse((DateTime.Now.Year - 1).ToString()))
                    {
                        res = xmlData.Contains((DateTime.Now.Year - 1).ToString());
                        if (!res)
                            mensaje = mensaje + (DateTime.Now.Year - 1).ToString() + "  ";
                        if (DateTime.Now.Month > 5)
                        {
                            res = xmlData.Contains((DateTime.Now.Year).ToString());
                            if (!res)
                                mensaje = mensaje + (DateTime.Now.Year).ToString() + "  ";
                        }
                    }
                }
            }

            if (mensaje == string.Empty)//MENSAJE ME INDICA QUE HACE FALTA GUARDAR ALGUNOS AÑOS
            {
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[14].Text.ToString()).Trim() != "")
                {
                    exito = Ln.finalizarBalance(objresumen.idEmpresa.ToString(), System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[14].Text.ToString()).Trim(), xmlData, Constantes.RIESGO.VALIDACION);
                    pnFormulario.Enabled = false;
                    if (exito)
                    {
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                        ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        llenargrid();
                    }
                    else
                    {
                        ocultarDiv();
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                    }
                }
                else
                {

                    exito = Ln.InsertarFinalizarBalance(objresumen.idEmpresa.ToString(), xmlData);
                    pnFormulario.Enabled = false;
                    if (exito)
                    {
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                        ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        llenargrid();

                    }
                    else
                    {
                        ocultarDiv();
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                    }
                }
            }
            else
            {
                ocultarDiv();
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REGISTOSINCOMPLETOSDOCCONTABLE) + " " + mensaje;
            }


        }

        protected void lbBalance_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //BALANCE
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoBalance.aspx");
        }


        protected void lbEdoResultado_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //ESTADO DE RESULTADO
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoEdoResultado.aspx");
        }

        protected void lbVentas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //IVA VENTAS
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoVentas.aspx");
        }

        protected void lbCompras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //IVA COMPRAS
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoCompras.aspx");
        }

        protected void lbScoring_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //IVA COMPRAS
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Scoring.aspx");
        }

        protected void lbResumenPAF_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //RESUMEN PAF
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ResumenPaf.aspx");
        }

        protected void btnAtras_Click1(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoBalance");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnImprimirBalance_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            string Reporte = "VaciadoBalance";

            byte[] archivo = new Reportes { }.GenerarReporteVaciado(objresumen.idEmpresa);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            guardar();
        }

        #endregion


        #region Metodos

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res;
            res = Ln.ListarBalance(objresumen.idEmpresa.ToString());
            int con = 0;
            for (int i = 0; i <= res.Rows.Count - 1; i++)
            {
                if (res.Rows[i]["IdEmpresaDocumentoContable"].ToString() != "")
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                if (i < res.Rows.Count - 1 && res.Rows[i]["Grupo"].ToString() == res.Rows[i + 1]["Grupo"].ToString())
                {
                    con++;
                }
                else
                {
                    if (con > 0)
                    {
                        DataRow row = res.NewRow();
                        row["Grupo"] = res.Rows[i]["Grupo"].ToString();
                        row["Cuenta"] = res.Rows[i]["Grupo"].ToString();
                        res.Rows.InsertAt(row, i + 1);
                    }
                    con = 0;
                }
                if (res.Rows[i]["Cuenta"].ToString() == "" || res.Rows[i]["Cuenta"].ToString() == null)
                {
                    res.Rows[i]["Cuenta"] = res.Rows[i]["Grupo"].ToString();
                }
            }
            double valor1 = 0;
            double valor2 = 0;
            double valor3 = 0;
            double valorA = 0;
            for (int i = 1; i <= res.Rows.Count - 2; i++)
            {
                if (res.Rows[i]["idGrupoCuenta"].ToString() == res.Rows[i + 1]["IdGrupoCuenta"].ToString())
                {


                    if (res.Rows[i]["ACCIONSubcuenta"].ToString().Trim() != "2")
                    {
                        if (res.Rows[i]["Valor1"].ToString() != "")
                            valor1 = valor1 + double.Parse(res.Rows[i]["Valor1"].ToString());
                        if (res.Rows[i]["Valor2"].ToString() != "")
                            valor2 = valor2 + double.Parse(res.Rows[i]["Valor2"].ToString());
                        if (res.Rows[i]["Valor3"].ToString() != "")
                            valor3 = valor3 + double.Parse(res.Rows[i]["Valor3"].ToString());
                        if (res.Rows[i]["ValorAct"].ToString() != "")
                            valorA = valorA + double.Parse(res.Rows[i]["ValorAct"].ToString());
                    }
                    else
                    {

                        if (res.Rows[i]["Valor1"].ToString() != "")
                            valor1 = valor1 - double.Parse(res.Rows[i]["Valor1"].ToString());
                        if (res.Rows[i]["Valor2"].ToString() != "")
                            valor2 = valor2 - double.Parse(res.Rows[i]["Valor2"].ToString());
                        if (res.Rows[i]["Valor3"].ToString() != "")
                            valor3 = valor3 - double.Parse(res.Rows[i]["Valor3"].ToString());
                        if (res.Rows[i]["ValorAct"].ToString() != "")
                            valorA = valorA - double.Parse(res.Rows[i]["ValorAct"].ToString());
                    }
                }
                else
                {
                    if (res.Rows[i]["ACCIONSubcuenta"].ToString().Trim() != "2")
                    {
                        if (res.Rows[i]["Valor1"].ToString() != "")
                            valor1 = valor1 + double.Parse(res.Rows[i]["Valor1"].ToString());
                        if (res.Rows[i]["Valor2"].ToString() != "")
                            valor2 = valor2 + double.Parse(res.Rows[i]["Valor2"].ToString());
                        if (res.Rows[i]["Valor3"].ToString() != "")
                            valor3 = valor3 + double.Parse(res.Rows[i]["Valor3"].ToString());
                        if (res.Rows[i]["ValorAct"].ToString() != "")
                            valorA = valorA + double.Parse(res.Rows[i]["ValorAct"].ToString());
                    }
                    else
                    {
                        if (res.Rows[i]["Valor1"].ToString() != "")
                            valor1 = valor1 - double.Parse(res.Rows[i]["Valor1"].ToString());
                        if (res.Rows[i]["Valor2"].ToString() != "")
                            valor2 = valor2 - double.Parse(res.Rows[i]["Valor2"].ToString());
                        if (res.Rows[i]["Valor3"].ToString() != "")
                            valor3 = valor3 - double.Parse(res.Rows[i]["Valor3"].ToString());
                        if (res.Rows[i]["ValorAct"].ToString() != "")
                            valorA = valorA - double.Parse(res.Rows[i]["ValorAct"].ToString());

                    }
                    res.Rows[i + 1]["Valor1"] = valor1.ToString("F");
                    valor1 = 0;
                    res.Rows[i + 1]["Valor2"] = valor2.ToString("F");
                    valor2 = 0;
                    res.Rows[i + 1]["Valor3"] = valor3.ToString("F");
                    valor3 = 0;
                    res.Rows[i + 1]["ValorAct"] = valorA.ToString("F");
                    valorA = 0;
                    i++;
                }
            }
            try
            {
                ResultadosBusqueda.DataSource = res;
                ResultadosBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
            }
        }

        protected void ocultarBotones()
        {
            btnLimpiar.Style.Add("display", "none");
            btnlimpiari.Style.Add("display", "none");
            btnGuardari.Style.Add("display", "none");
            btnFinalizar.Style.Add("display", "none");
            btnFinalizari.Style.Add("display", "none");
        }

        private string generarXML()
        {
            asignacionResumen(ref objresumen);
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            XmlNode RespNode;

            for (int i = 0; i <= ResultadosBusqueda.Rows.Count - 1; i++)
            {
                //if (!(String.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text.ToString()))
                //    && (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text.ToString() != " "
                //    // && (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text.ToString() != "0"
                //    && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim() != "")
                var control1 = ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox;
                if (control1 != null)
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode IdCuentaNode = doc.CreateElement("IdCuenta");
                    IdCuentaNode.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdCuentaNode);

                    XmlNode IdSubGCNode = doc.CreateElement("IdSubGrupoCuenta");
                    IdSubGCNode.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()) ? null : System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdSubGCNode);

                    //cheques = string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString())) ? "" : System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString())
                    XmlNode IdAnioNode = doc.CreateElement("Anio");
                    IdAnioNode.AppendChild(doc.CreateTextNode((System.DateTime.Now.Year - 3).ToString()));
                    RespNode.AppendChild(IdAnioNode);

                    XmlNode IdMes = doc.CreateElement("Mes");
                    IdMes.AppendChild(doc.CreateTextNode("12"));
                    RespNode.AppendChild(IdMes);

                    XmlNode ValorAnio1 = doc.CreateElement("Valor");
                    ValorAnio1.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text.ToString()) ? "0" : (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text.ToString()));
                    //ValorAnio1.AppendChild(doc.CreateTextNode((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text.ToString()));
                    RespNode.AppendChild(ValorAnio1);

                    XmlNode IdTipo = doc.CreateElement("IdTipo");
                    IdTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValor1") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(IdTipo);


                    XmlNode DescTipo = doc.CreateElement("DescTipo");
                    DescTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValor1") as DropDownList).SelectedItem.ToString()));
                    RespNode.AppendChild(DescTipo);

                    XmlNode Requerido = doc.CreateElement("Requerido");
                    Requerido.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido3") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(Requerido);

                    ValoresNode.AppendChild(RespNode);
                }

                //if (!(String.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString()))
                //    && (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString() != " "
                //    // && (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString() != "0"
                //    && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim() != "")
                var control2 = ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox;
                if (control2 != null)
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode IdCuentaNode = doc.CreateElement("IdCuenta");
                    IdCuentaNode.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdCuentaNode);

                    XmlNode IdSubGCNode = doc.CreateElement("IdSubGrupoCuenta");
                    IdSubGCNode.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()) ? null : System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdSubGCNode);

                    XmlNode IdAnioNode = doc.CreateElement("Anio");
                    IdAnioNode.AppendChild(doc.CreateTextNode((System.DateTime.Now.Year - 2).ToString()));
                    RespNode.AppendChild(IdAnioNode);

                    XmlNode IdMes = doc.CreateElement("Mes");
                    IdMes.AppendChild(doc.CreateTextNode("12"));
                    RespNode.AppendChild(IdMes);

                    XmlNode ValorAnio2 = doc.CreateElement("Valor");
                    ValorAnio2.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString()) ? "0" : (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString()));
                    //ValorAnio1.AppendChild(doc.CreateTextNode((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString()));
                    RespNode.AppendChild(ValorAnio2);

                    XmlNode IdTipo = doc.CreateElement("IdTipo");
                    IdTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValor2") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(IdTipo);

                    XmlNode DescTipo = doc.CreateElement("DescTipo");
                    DescTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValor2") as DropDownList).SelectedItem.ToString()));
                    RespNode.AppendChild(DescTipo);

                    XmlNode Requerido = doc.CreateElement("Requerido");
                    Requerido.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido2") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(Requerido);

                    ValoresNode.AppendChild(RespNode);
                }

                //if (!(String.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString()))
                //    && (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString() != " "
                //    //  && (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString() != "0"
                //    && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim() != "")
                var control3 = ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox;
                if (control3 != null)
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode IdCuentaNode = doc.CreateElement("IdCuenta");
                    IdCuentaNode.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdCuentaNode);

                    XmlNode IdSubGCNode = doc.CreateElement("IdSubGrupoCuenta");
                    IdSubGCNode.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()) ? null : System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdSubGCNode);

                    XmlNode IdAnioNode = doc.CreateElement("Anio");
                    IdAnioNode.AppendChild(doc.CreateTextNode((System.DateTime.Now.Year - 1).ToString()));
                    RespNode.AppendChild(IdAnioNode);

                    XmlNode IdMes = doc.CreateElement("Mes");
                    IdMes.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbMes3") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(IdMes);

                    XmlNode ValorAnio3 = doc.CreateElement("Valor");
                    ValorAnio3.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString()) ? "0" : (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString()));
                    //ValorAnio3.AppendChild(doc.CreateTextNode((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString()));
                    RespNode.AppendChild(ValorAnio3);

                    XmlNode IdTipo = doc.CreateElement("IdTipo");
                    IdTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValor3") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(IdTipo);

                    XmlNode DescTipo = doc.CreateElement("DescTipo");
                    DescTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValor3") as DropDownList).SelectedItem.ToString()));
                    RespNode.AppendChild(DescTipo);

                    XmlNode Requerido = doc.CreateElement("Requerido");
                    Requerido.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido1") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(Requerido);

                    ValoresNode.AppendChild(RespNode);
                }

                //if (!(String.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text.ToString()))
                //    && (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text.ToString() != " "
                //    // && (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text.ToString() != "0"
                //    && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim() != "")
                var control4 = ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox;
                if (control4 != null)
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode IdCuentaNode = doc.CreateElement("IdCuenta");
                    IdCuentaNode.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdCuentaNode);

                    XmlNode IdSubGCNode = doc.CreateElement("IdSubGrupoCuenta");
                    IdSubGCNode.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty(System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()) ? null : System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[9].Text.ToString()).Trim()));
                    RespNode.AppendChild(IdSubGCNode);

                    XmlNode IdAnioNode = doc.CreateElement("Anio");
                    IdAnioNode.AppendChild(doc.CreateTextNode(System.DateTime.Now.Year.ToString()));
                    RespNode.AppendChild(IdAnioNode);

                    XmlNode IdMes = doc.CreateElement("Mes");
                    IdMes.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbMesAct") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(IdMes);

                    XmlNode ValorAnioAct = doc.CreateElement("Valor");
                    ValorAnioAct.AppendChild(doc.CreateTextNode(string.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text.ToString()) ? "0" : (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text.ToString()));
                    //ValorAnioAct.AppendChild(doc.CreateTextNode((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text.ToString()));
                    RespNode.AppendChild(ValorAnioAct);

                    XmlNode IdTipo = doc.CreateElement("IdTipo");
                    IdTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValorActual") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(IdTipo);

                    XmlNode DescTipo = doc.CreateElement("DescTipo");
                    DescTipo.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("cbValorActual") as DropDownList).SelectedItem.ToString()));
                    RespNode.AppendChild(DescTipo);

                    XmlNode Requerido = doc.CreateElement("Requerido");
                    Requerido.AppendChild(doc.CreateTextNode((ResultadosBusqueda.HeaderRow.FindControl("ddlRequeridoActual") as DropDownList).SelectedValue.ToString()));
                    RespNode.AppendChild(Requerido);

                    ValoresNode.AppendChild(RespNode);
                }
            }
            return doc.OuterXml;
        }

        private void guardar()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean exito;
            ocultarDiv();

            string xmlDatos = generarXML();
            string mensajeAniosReq = "Se requiere información en el(los) años(s): ";
            bool permiteGuardar = true;

            bool requerido3 = (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido3") as DropDownList).SelectedValue.ToString() == "0" ? false : true;
            bool requerido2 = (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido2") as DropDownList).SelectedValue.ToString() == "0" ? false : true;
            bool requerido1 = (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido1") as DropDownList).SelectedValue.ToString() == "0" ? false : true;
            bool requeridoActual = (ResultadosBusqueda.HeaderRow.FindControl("ddlRequeridoActual") as DropDownList).SelectedValue.ToString() == "0" ? false : true;

            bool auditado = (ViewState["cbValor1"].ToString() == "1" || ViewState["cbValor2"].ToString() == "1" || ViewState["cbValor3"].ToString() == "1" || ViewState["cbValorActual"].ToString() == "1") ? true : false;
            bool auditado3 = ViewState["cbValor1"].ToString() == "1" ? true : false;
            bool auditado2 = ViewState["cbValor2"].ToString() == "1" ? true : false;
            bool auditado1 = ViewState["cbValor3"].ToString() == "1" ? true : false;
            bool auditadoActual = ViewState["cbValorActual"].ToString() == "1" ? true : false;
            bool ComentarioExiste = true;//(!string.IsNullOrWhiteSpace(txtComentario.Text)) ? true : false;

            if (requerido3)
            {
                if (!xmlDatos.Contains((DateTime.Now.Year - 3).ToString()))
                {
                    mensajeAniosReq = mensajeAniosReq + (DateTime.Now.Year - 3).ToString() + " ";
                    permiteGuardar = false;
                }
            }
            if (requerido2)
            {
                if (!xmlDatos.Contains((DateTime.Now.Year - 2).ToString()))
                {
                    mensajeAniosReq = mensajeAniosReq + (DateTime.Now.Year - 2).ToString() + " ";
                    permiteGuardar = false;
                }
            }
            if (requerido1)
            {
                if (!xmlDatos.Contains((DateTime.Now.Year - 1).ToString()))
                {
                    mensajeAniosReq = mensajeAniosReq + (DateTime.Now.Year - 1).ToString() + " ";
                    permiteGuardar = false;
                }
            }
            if (requeridoActual)
            {
                if (!xmlDatos.Contains((DateTime.Now.Year).ToString()))
                {
                    mensajeAniosReq = mensajeAniosReq + (DateTime.Now.Year).ToString() + " ";
                    permiteGuardar = false;
                }
            }
            if ((auditado && ComentarioExiste) || (!auditado))
            {
                if (permiteGuardar)
                {
                    if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                    {
                        exito = Ln.InsertarBalance(objresumen.idEmpresa.ToString(), xmlDatos, objresumen.idUsuario, txtComentario.Text);
                        if (exito)
                        {
                            ocultarDiv();
                            dvSuccess.Style.Add("display", "block");
                            lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                            ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                            llenargrid();
                        }
                        else
                        {
                            ocultarDiv();
                            dvError.Style.Add("display", "block");
                            lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                        }
                    }
                    else
                    {
                        exito = Ln.ModificarBalance(objresumen.idEmpresa.ToString(), System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[14].Text.ToString()).Trim(), xmlDatos, objresumen.idUsuario, txtComentario.Text);
                        if (exito)
                        {
                            ocultarDiv();
                            dvSuccess.Style.Add("display", "block");
                            lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE);
                            llenargrid();
                        }
                        else
                        {
                            ocultarDiv();
                            dvError.Style.Add("display", "block");
                            lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE);
                        }
                    }
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = mensajeAniosReq;
                }
            }
            else
            {
                dvError.Style.Add("display", "block");
                lbError.Text = "Debe ingresar un comentario al final de la página cada vez que quiera guardar y un año esté previamente auditado";
            }


            string msjError = string.Empty;
            if (((ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).Text = "0"; }
            if (((ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).Text = "0"; }
            if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnioA") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnioA") as TextBox).Text))
            {
                msjError = msjError + " " + (DateTime.Now.Year).ToString() + " ";

            }

            if (((ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).Text = "0"; }
            if (((ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).Text = "0"; }
            if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio3") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio3") as TextBox).Text))
            {
                msjError = msjError + " " + (DateTime.Now.Year - 1).ToString() + "  ";

            }
            if (((ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).Text = "0"; }
            if (((ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).Text = "0"; }
            if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio2") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio2") as TextBox).Text))
            {
                msjError = msjError + " " + (DateTime.Now.Year - 2).ToString() + " ";

            }

            if (((ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text = "0"; }
            if (((ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text) == "") { (ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text = "0"; }
            if (float.Parse((ResultadosBusqueda.Rows[28].FindControl("txtAnio1") as TextBox).Text) != float.Parse((ResultadosBusqueda.Rows[57].FindControl("txtAnio1") as TextBox).Text))
            {
                msjError = msjError + " " + (DateTime.Now.Year - 3).ToString() + " ";

            }
            if (msjError != "")
            {
                // ocultarDiv();
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = "Balance No cuadra, revisar año(s): " + msjError;
            }

        }

        #endregion

    }
}
