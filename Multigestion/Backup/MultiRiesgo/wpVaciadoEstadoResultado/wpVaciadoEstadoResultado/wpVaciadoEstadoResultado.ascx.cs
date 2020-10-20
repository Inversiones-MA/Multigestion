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
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using ClasesNegocio;
using Bd;

namespace MultiRiesgo.wpVaciadoEstadoResultado.wpVaciadoEstadoResultado
{
    [ToolboxItemAttribute(false)]
    public partial class wpVaciadoEstadoResultado : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpVaciadoEstadoResultado()
        {
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "VaciadoEdoResultado.aspx";

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
                    btnLimpiar.OnClientClick = "return LimpiarVacido('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";
                    btnlimpiari.OnClientClick = "return LimpiarVacido('" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "');";

                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;


                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbOperacion.Text = objresumen.desOperacion;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Enabled = false;
                            pnFormulario.Enabled = false;
                        }
                        llenargrid();
                    }
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
            e.Row.Cells[24].Visible = false;

            e.Row.Cells[20].Visible = false; // ACCIONgrupo
            e.Row.Cells[21].Visible = false; // ACCIONcuenta
            e.Row.Cells[22].Visible = false; // ACCIONsubcuenta    

            e.Row.Cells[25].Visible = false; // RequeridoActual   
            e.Row.Cells[26].Visible = false; // Requerido3 
            e.Row.Cells[27].Visible = false; // Requerido2   
            e.Row.Cells[28].Visible = false; // Requerido1 

            if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 3).ToString()))
            {
                e.Row.Cells[3].Style.Add("display", "none");
                if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 2).ToString()))
                {
                    //e.Row.Cells[5].Enabled = false;
                    e.Row.Cells[4].Style.Add("display", "none");
                    if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 1).ToString()))
                    {
                        e.Row.Cells[5].Style.Add("display", "none");
                        //e.Row.Cells[6].Enabled = false;
                    }
                }
            }

        }

        private void CargarEstadoResultado()
        {

        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            asignacionResumen(ref objresumen);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                LogicaNegocio Ln = new LogicaNegocio();

                DropDownList cmbTipoDocumentAux = new DropDownList();
                DataTable dt = Ln.ListarTipoDocumentoContable();
                util.CargaDDL1(cmbTipoDocumentAux, dt, "Nombre", "Id");

                (e.Row.FindControl("lbValor1") as Label).Text = "Dec - " + (System.DateTime.Now.Year - 3).ToString() + "  ";
                (e.Row.FindControl("cbValor1") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValor1") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;
                (e.Row.FindControl("cbValor1") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValor1") as DropDownList).DataBind();

                (e.Row.FindControl("lbValor2") as Label).Text = "Dec - " + (System.DateTime.Now.Year - 2).ToString() + "  ";
                (e.Row.FindControl("cbValor2") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValor2") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;
                (e.Row.FindControl("cbValor2") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValor2") as DropDownList).DataBind();

                (e.Row.FindControl("lbValor3") as Label).Text = (System.DateTime.Now.Year - 1).ToString() + "  ";
                (e.Row.FindControl("cbValor3") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValor3") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;
                (e.Row.FindControl("cbValor3") as DropDownList).DataValueField = cmbTipoDocumentAux.DataValueField;
                (e.Row.FindControl("cbValor3") as DropDownList).DataBind();

                (e.Row.FindControl("lbValorAct") as Label).Text = System.DateTime.Now.Year.ToString() + "  ";
                (e.Row.FindControl("cbValorActual") as DropDownList).DataSource = cmbTipoDocumentAux.DataSource;
                (e.Row.FindControl("cbValorActual") as DropDownList).DataTextField = cmbTipoDocumentAux.DataTextField;
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            grabar();
        }

        protected void ResultadosBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ResultadosBusqueda_DataBound(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            int bandera = 0;

            float TotalRB1 = 0;
            float TotalRB2 = 0;
            float TotalRB3 = 0;
            float TotalRB4 = 0;
            ViewState["cbValor1"] = "0";
            ViewState["cbValor2"] = "0";
            ViewState["cbValor3"] = "0";
            ViewState["cbValorActual"] = "0";
            for (int i = 0; i <= ResultadosBusqueda.Rows.Count - 1; i++)
            {
                //cargo valores de cada una de las celdas
                // if (res.Rows[i]["idGrupoCuenta"].ToString() == res.Rows[i + 1]["IdGrupoCuenta"].ToString())
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text) != "0")
                {
                    //  ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValor1") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text);
                    ViewState["cbValor1"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[16].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text) != "0")
                {
                    // ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValor2") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text);
                    ViewState["cbValor2"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[17].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text) != "0")
                {
                    // ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValor3") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text);
                    ViewState["cbValor3"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != "0")
                {
                    // ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("cbValorActual") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text);
                    ViewState["cbValorActual"] = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[20].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text) != "0")
                {
                    //ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("cbMes3") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[20].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[25].Text) != " ")//&& System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != "0")
                {
                    //ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequeridoActual") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[25].Text) == "False") ? "0" : "1";

                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[26].Text) != " ")//&& System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != "0")
                {
                    //ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido3") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[26].Text) == "False") ? "0" : "1";

                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[27].Text) != " ")//&& System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != "0")
                {
                    //ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido2") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[27].Text) == "False") ? "0" : "1";

                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[28].Text) != " ")//&& System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[19].Text) != "0")
                {
                    //ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("ddlRequerido1") as DropDownList).SelectedValue = (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[28].Text) == "False") ? "0" : "1";

                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[21].Text) != " ")// && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[18].Text) != "0")
                {
                    //ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.HeaderRow.FindControl("cbMesAct") as DropDownList).SelectedValue = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[21].Text);
                }

                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[10].Text) != " " && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[10].Text) != "0")
                {
                    //  ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[10].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[11].Text) != " " && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[11].Text) != "0")
                {
                    // ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[11].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[12].Text) != " " && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[12].Text) != "0")
                {
                    //  ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[12].Text);
                }
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text) != " " && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text) != "0")
                {
                    // ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[13].Text);
                }


                if (ResultadosBusqueda.Rows[i].Cells[0].Text == ResultadosBusqueda.Rows[i].Cells[1].Text)
                {//las celdas que son como encabeado de lo que viene las coloco como bloqueadas y sin eventos

                    ResultadosBusqueda.Rows[i].CssClass = "bg-warning";

                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).ReadOnly = true;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).ReadOnly = true;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).ReadOnly = true;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).ReadOnly = true;

                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Enabled = false;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Enabled = false;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Enabled = false;
                    (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Enabled = false;

                    if (ResultadosBusqueda.Rows[i].Cells[1].Text == "RESULTADO BRUTO")
                    {
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text != "")
                        { TotalRB1 = TotalRB1 + float.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Text); }
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text != "")
                        { TotalRB2 = TotalRB2 + float.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text); }
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text != "")
                        { TotalRB3 = TotalRB3 + float.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text); }
                        if ((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text != "")
                        { TotalRB4 = TotalRB4 + float.Parse((ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Text); }
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

            //calculos
            //ResultadoBruto
            for (int i = 0; i <= 2; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return ResultadoBruto('" + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"

                                              + (ResultadosBusqueda.Rows[0].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "');");


                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                    "return ResultadoBruto('" + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                  + (ResultadosBusqueda.Rows[0].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "');");
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                    "return ResultadoBruto('" + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");
                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                    "return ResultadoBruto('" + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[0].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "');");
            }
            //ResultadoOperacional


            (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
               "return ResultadoOperacional('" + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[0].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "');");

            (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                          "return ResultadoOperacional('" + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                       + (ResultadosBusqueda.Rows[0].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "');");

            (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                   "return ResultadoOperacional('" + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");

            (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                   "return ResultadoOperacional('" + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[0].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "');");

            // ResultadoNoOperacional

            for (int i = 5; i <= 13; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return ResultadoNoOperacional('" + (ResultadosBusqueda.Rows[14].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[0].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                   "return ResultadoNoOperacional('" + (ResultadosBusqueda.Rows[14].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[0].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                "return ResultadoNoOperacional('" + (ResultadosBusqueda.Rows[14].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
            "return ResultadoNoOperacional('" + (ResultadosBusqueda.Rows[14].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                         + (ResultadosBusqueda.Rows[0].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "');");
                //txtAnio2
                //txtAnio3
                //txtAnioA
            }

            //ResultadoAntesImpuesto

            for (int i = 16; i <= 18; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return ResultadoAntesImpuesto('" + (ResultadosBusqueda.Rows[15].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[0].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                  "return ResultadoAntesImpuesto('" + (ResultadosBusqueda.Rows[15].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[0].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                  "return ResultadoAntesImpuesto('" + (ResultadosBusqueda.Rows[15].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                  "return ResultadoAntesImpuesto('" + (ResultadosBusqueda.Rows[15].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[0].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "');");
                //txtAnio2
                //txtAnio3
                //txtAnioA
            }

            //UtilidadPerdidaLiquida
            for (int i = 16; i <= 19; i++)
            {
                (ResultadosBusqueda.Rows[i].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                    "return UtilidadPerdidaLiquida('" + (ResultadosBusqueda.Rows[19].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[0].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "');");


                (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                   "return UtilidadPerdidaLiquida('" + (ResultadosBusqueda.Rows[19].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                               + (ResultadosBusqueda.Rows[0].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio2") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                   "return UtilidadPerdidaLiquida('" + (ResultadosBusqueda.Rows[19].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");

                (ResultadosBusqueda.Rows[i].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                   "return UtilidadPerdidaLiquida('" + (ResultadosBusqueda.Rows[19].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                                 + (ResultadosBusqueda.Rows[0].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "');");


                //txtAnio2
                //txtAnio3
                //txtAnioA
            }

            //UtilidadPerdidaEjercicio

            (ResultadosBusqueda.Rows[20].FindControl("txtAnio1") as TextBox).Attributes.Add("onblur",
                   "return UtilidadPerdidaEjercicio('" + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                                + (ResultadosBusqueda.Rows[0].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio1") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio1") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio1") as TextBox).ClientID + "');");

            (ResultadosBusqueda.Rows[20].FindControl("txtAnio2") as TextBox).Attributes.Add("onblur",
                 "return UtilidadPerdidaEjercicio('" + (ResultadosBusqueda.Rows[21].FindControl("txtAnio2") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");

            (ResultadosBusqueda.Rows[20].FindControl("txtAnio3") as TextBox).Attributes.Add("onblur",
                 "return UtilidadPerdidaEjercicio('" + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[0].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnio3") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnio3") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnio3") as TextBox).ClientID + "');");

            (ResultadosBusqueda.Rows[20].FindControl("txtAnioA") as TextBox).Attributes.Add("onblur",
                 "return UtilidadPerdidaEjercicio('" + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                            + (ResultadosBusqueda.Rows[0].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[1].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[2].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[3].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[4].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[5].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[6].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[7].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[8].FindControl("txtAnioA") as TextBox).ClientID + "','"
                                              + (ResultadosBusqueda.Rows[9].FindControl("txtAnioA") as TextBox).ClientID + "','"
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
                                              + (ResultadosBusqueda.Rows[21].FindControl("txtAnioA") as TextBox).ClientID + "');");


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
                    exito = Ln.finalizarEstadoResultado(objresumen.idEmpresa.ToString(), System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[14].Text.ToString()).Trim(), xmlData, Constantes.RIESGO.VALIDACION);
                    pnFormulario.Enabled = false;
                    if (exito)
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                        ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        llenargrid();
                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                    }
                }
                else
                {

                    exito = Ln.InsertarFinalizarEstado(objresumen.idEmpresa.ToString(), xmlData);
                    pnFormulario.Enabled = false;
                    if (exito)
                    {
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                        ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        llenargrid();

                    }
                    else
                    {
                        dvError.Style.Add("display", "block");
                        lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                    }
                }

            }
            else
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REGISTOSINCOMPLETOSDOCCONTABLE) + " " + mensaje;
            }


        }

        protected void lbBalance_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //BALANCE
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Session["RESUMEN"] = objresumen;
            Page.Response.Redirect("VaciadoBalance.aspx");
        }

        protected void lbEdoResultado_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //ESTADO DE RESULTADO
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoEdoResultado.aspx");
        }

        protected void lbVentas_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //IVA VENTAS
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoVentas.aspx");
        }

        protected void lbCompras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //IVA COMPRAS
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("VaciadoCompras.aspx");
        }

        protected void lbScoring_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //IVA COMPRAS
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("Scoring.aspx");
        }

        protected void lbResumenPAF_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            //RESUMEN PAF
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ResumenPaf.aspx");
        }

        protected void btnAtras_Click1(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "VaciadoEstadoResultado");
            Page.Response.Redirect(objresumen.linkPrincial);
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

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res;
            res = Ln.ListarEstadoResultado(objresumen.idEmpresa.ToString());
            int con = 0;

            for (int i = 0; i <= res.Rows.Count - 1; i++)
            {
                if (res.Rows[i]["IdEmpresaDocumentoContable"].ToString() != "")
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;

                if (i < res.Rows.Count - 1 && i > 0 && (res.Rows[i]["Grupo"].ToString() != res.Rows[i + 1]["Grupo"].ToString()
                    && res.Rows[i]["Grupo"].ToString() != res.Rows[i - 1]["Grupo"].ToString()
                    && res.Rows[i]["Cuenta"].ToString() != ""
                    ))
                { con++; }


                if (i == res.Rows.Count - 1 && i > 0 && (res.Rows[i]["Grupo"].ToString() != res.Rows[i - 1]["Grupo"].ToString()
                 && res.Rows[i]["Cuenta"].ToString() != ""
                 ))
                { con++; }


                if ((i < res.Rows.Count - 1 && ((res.Rows[i]["Grupo"].ToString() == res.Rows[i + 1]["Grupo"].ToString()))))
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


            //ResultadoBruto
            res.Rows[2]["Valor1"] = (double.Parse(res.Rows[0]["Valor1"].ToString() != "" ? res.Rows[0]["Valor1"].ToString() : "0") -
                                     double.Parse(res.Rows[1]["Valor1"].ToString() != "" ? res.Rows[1]["Valor1"].ToString() : "0"));

            res.Rows[2]["Valor2"] = (double.Parse(res.Rows[0]["Valor2"].ToString() != "" ? res.Rows[0]["Valor2"].ToString() : "0") -
                                     double.Parse(res.Rows[1]["Valor2"].ToString() != "" ? res.Rows[1]["Valor2"].ToString() : "0"));

            res.Rows[2]["Valor3"] = (double.Parse(res.Rows[0]["Valor3"].ToString() != "" ? res.Rows[0]["Valor3"].ToString() : "0") -
                                     double.Parse(res.Rows[1]["Valor3"].ToString() != "" ? res.Rows[1]["Valor3"].ToString() : "0"));

            res.Rows[2]["ValorAct"] = (double.Parse(res.Rows[0]["ValorAct"].ToString() != "" ? res.Rows[0]["ValorAct"].ToString() : "0") -
                                     double.Parse(res.Rows[1]["ValorAct"].ToString() != "" ? res.Rows[1]["ValorAct"].ToString() : "0"));
            //ResultadoOperacional
            res.Rows[4]["Valor1"] = (double.Parse(res.Rows[2]["Valor1"].ToString() != "" ? res.Rows[2]["Valor1"].ToString() : "0") -
                                     double.Parse(res.Rows[3]["Valor1"].ToString() != "" ? res.Rows[3]["Valor1"].ToString() : "0"));

            res.Rows[4]["Valor2"] = (double.Parse(res.Rows[2]["Valor2"].ToString() != "" ? res.Rows[2]["Valor2"].ToString() : "0") -
                                     double.Parse(res.Rows[3]["Valor2"].ToString() != "" ? res.Rows[3]["Valor2"].ToString() : "0"));

            res.Rows[4]["Valor3"] = (double.Parse(res.Rows[2]["Valor3"].ToString() != "" ? res.Rows[2]["Valor3"].ToString() : "0") -
                                     double.Parse(res.Rows[3]["Valor3"].ToString() != "" ? res.Rows[3]["Valor3"].ToString() : "0"));

            res.Rows[4]["ValorAct"] = (double.Parse(res.Rows[2]["ValorAct"].ToString() != "" ? res.Rows[2]["ValorAct"].ToString() : "0") -
                                     double.Parse(res.Rows[3]["ValorAct"].ToString() != "" ? res.Rows[3]["ValorAct"].ToString() : "0"));


            //Resultado No operacional

            res.Rows[14]["Valor1"] = (-double.Parse(res.Rows[5]["Valor1"].ToString() != "" ? res.Rows[5]["Valor1"].ToString() : "0") +
                                 double.Parse(res.Rows[6]["Valor1"].ToString() != "" ? res.Rows[6]["Valor1"].ToString() : "0") +
                                  double.Parse(res.Rows[7]["Valor1"].ToString() != "" ? res.Rows[7]["Valor1"].ToString() : "0") +
                                   double.Parse(res.Rows[8]["Valor1"].ToString() != "" ? res.Rows[8]["Valor1"].ToString() : "0") -
                                    double.Parse(res.Rows[9]["Valor1"].ToString() != "" ? res.Rows[9]["Valor1"].ToString() : "0") -
                                     double.Parse(res.Rows[10]["Valor1"].ToString() != "" ? res.Rows[10]["Valor1"].ToString() : "0") -
                                  double.Parse(res.Rows[11]["Valor1"].ToString() != "" ? res.Rows[11]["Valor1"].ToString() : "0") +
                                   double.Parse(res.Rows[12]["Valor1"].ToString() != "" ? res.Rows[12]["Valor1"].ToString() : "0") +
                                    double.Parse(res.Rows[13]["Valor1"].ToString() != "" ? res.Rows[13]["Valor1"].ToString() : "0")
                                 );

            res.Rows[14]["Valor2"] = (-double.Parse(res.Rows[5]["Valor2"].ToString() != "" ? res.Rows[5]["Valor2"].ToString() : "0") +
                            double.Parse(res.Rows[6]["Valor2"].ToString() != "" ? res.Rows[6]["Valor2"].ToString() : "0") +
                             double.Parse(res.Rows[7]["Valor2"].ToString() != "" ? res.Rows[7]["Valor2"].ToString() : "0") +
                              double.Parse(res.Rows[8]["Valor2"].ToString() != "" ? res.Rows[8]["Valor2"].ToString() : "0") -
                               double.Parse(res.Rows[9]["Valor2"].ToString() != "" ? res.Rows[9]["Valor2"].ToString() : "0") -
                                double.Parse(res.Rows[10]["Valor2"].ToString() != "" ? res.Rows[10]["Valor2"].ToString() : "0") -
                             double.Parse(res.Rows[11]["Valor2"].ToString() != "" ? res.Rows[11]["Valor2"].ToString() : "0") +
                              double.Parse(res.Rows[12]["Valor2"].ToString() != "" ? res.Rows[12]["Valor2"].ToString() : "0") +
                               double.Parse(res.Rows[13]["Valor2"].ToString() != "" ? res.Rows[13]["Valor2"].ToString() : "0")
                            );

            res.Rows[14]["Valor3"] = (-double.Parse(res.Rows[5]["Valor3"].ToString() != "" ? res.Rows[5]["Valor3"].ToString() : "0") +
                            double.Parse(res.Rows[6]["Valor3"].ToString() != "" ? res.Rows[6]["Valor3"].ToString() : "0") +
                             double.Parse(res.Rows[7]["Valor3"].ToString() != "" ? res.Rows[7]["Valor3"].ToString() : "0") +
                              double.Parse(res.Rows[8]["Valor3"].ToString() != "" ? res.Rows[8]["Valor3"].ToString() : "0") -
                               double.Parse(res.Rows[9]["Valor3"].ToString() != "" ? res.Rows[9]["Valor3"].ToString() : "0") -
                                double.Parse(res.Rows[10]["Valor3"].ToString() != "" ? res.Rows[10]["Valor3"].ToString() : "0") -
                             double.Parse(res.Rows[11]["Valor3"].ToString() != "" ? res.Rows[11]["Valor3"].ToString() : "0") +
                              double.Parse(res.Rows[12]["Valor3"].ToString() != "" ? res.Rows[12]["Valor3"].ToString() : "0") +
                               double.Parse(res.Rows[13]["Valor3"].ToString() != "" ? res.Rows[13]["Valor3"].ToString() : "0")
                            );

            res.Rows[14]["ValorAct"] = (-double.Parse(res.Rows[5]["ValorAct"].ToString() != "" ? res.Rows[5]["ValorAct"].ToString() : "0") +
                            double.Parse(res.Rows[6]["ValorAct"].ToString() != "" ? res.Rows[6]["ValorAct"].ToString() : "0") +
                             double.Parse(res.Rows[7]["ValorAct"].ToString() != "" ? res.Rows[7]["ValorAct"].ToString() : "0") +
                              double.Parse(res.Rows[8]["ValorAct"].ToString() != "" ? res.Rows[8]["ValorAct"].ToString() : "0") -
                               double.Parse(res.Rows[9]["ValorAct"].ToString() != "" ? res.Rows[9]["ValorAct"].ToString() : "0") -
                                double.Parse(res.Rows[10]["ValorAct"].ToString() != "" ? res.Rows[10]["ValorAct"].ToString() : "0") -
                             double.Parse(res.Rows[11]["ValorAct"].ToString() != "" ? res.Rows[11]["ValorAct"].ToString() : "0") +
                              double.Parse(res.Rows[12]["ValorAct"].ToString() != "" ? res.Rows[12]["ValorAct"].ToString() : "0") +
                               double.Parse(res.Rows[13]["ValorAct"].ToString() != "" ? res.Rows[13]["ValorAct"].ToString() : "0")
                            );

            //resultado antes de impuesto

            res.Rows[15]["Valor1"] = (double.Parse(res.Rows[4]["Valor1"].ToString() != "" ? res.Rows[4]["Valor1"].ToString() : "0") +
                                 double.Parse(res.Rows[14]["Valor1"].ToString() != "" ? res.Rows[14]["Valor1"].ToString() : "0"));

            res.Rows[15]["Valor2"] = (double.Parse(res.Rows[4]["Valor2"].ToString() != "" ? res.Rows[4]["Valor2"].ToString() : "0") +
                                double.Parse(res.Rows[14]["Valor2"].ToString() != "" ? res.Rows[14]["Valor2"].ToString() : "0"));

            res.Rows[15]["Valor3"] = (double.Parse(res.Rows[4]["Valor3"].ToString() != "" ? res.Rows[4]["Valor3"].ToString() : "0") +
                                double.Parse(res.Rows[14]["Valor3"].ToString() != "" ? res.Rows[14]["Valor3"].ToString() : "0"));

            res.Rows[15]["ValorAct"] = (double.Parse(res.Rows[4]["ValorAct"].ToString() != "" ? res.Rows[4]["ValorAct"].ToString() : "0") +
                                double.Parse(res.Rows[14]["ValorAct"].ToString() != "" ? res.Rows[14]["ValorAct"].ToString() : "0"));

            //utilidad perdida liquida

            res.Rows[19]["Valor1"] = (double.Parse(res.Rows[15]["Valor1"].ToString() != "" ? res.Rows[15]["Valor1"].ToString() : "0") -
                              double.Parse(res.Rows[16]["Valor1"].ToString() != "" ? res.Rows[16]["Valor1"].ToString() : "0") +
                               double.Parse(res.Rows[17]["Valor1"].ToString() != "" ? res.Rows[17]["Valor1"].ToString() : "0") +
                                double.Parse(res.Rows[18]["Valor1"].ToString() != "" ? res.Rows[18]["Valor1"].ToString() : "0")
                              );

            res.Rows[19]["Valor2"] = (double.Parse(res.Rows[15]["Valor2"].ToString() != "" ? res.Rows[15]["Valor2"].ToString() : "0") -
                           double.Parse(res.Rows[16]["Valor2"].ToString() != "" ? res.Rows[16]["Valor2"].ToString() : "0") +
                            double.Parse(res.Rows[17]["Valor2"].ToString() != "" ? res.Rows[17]["Valor2"].ToString() : "0") +
                             double.Parse(res.Rows[18]["Valor2"].ToString() != "" ? res.Rows[18]["Valor2"].ToString() : "0")
                           );

            res.Rows[19]["Valor3"] = (double.Parse(res.Rows[15]["Valor3"].ToString() != "" ? res.Rows[15]["Valor3"].ToString() : "0") -
                           double.Parse(res.Rows[16]["Valor3"].ToString() != "" ? res.Rows[16]["Valor3"].ToString() : "0") +
                            double.Parse(res.Rows[17]["Valor3"].ToString() != "" ? res.Rows[17]["Valor3"].ToString() : "0") +
                             double.Parse(res.Rows[18]["Valor3"].ToString() != "" ? res.Rows[18]["Valor3"].ToString() : "0")
                           );


            res.Rows[19]["ValorAct"] = (double.Parse(res.Rows[15]["ValorAct"].ToString() != "" ? res.Rows[15]["ValorAct"].ToString() : "0") -
                           double.Parse(res.Rows[16]["ValorAct"].ToString() != "" ? res.Rows[16]["ValorAct"].ToString() : "0") +
                            double.Parse(res.Rows[17]["ValorAct"].ToString() != "" ? res.Rows[17]["ValorAct"].ToString() : "0") +
                             double.Parse(res.Rows[18]["ValorAct"].ToString() != "" ? res.Rows[18]["ValorAct"].ToString() : "0")
                           );

            //utilidad perdida del ejercicio
            res.Rows[21]["Valor1"] = (double.Parse(res.Rows[19]["Valor1"].ToString() != "" ? res.Rows[19]["Valor1"].ToString() : "0") +
                              double.Parse(res.Rows[20]["Valor1"].ToString() != "" ? res.Rows[20]["Valor1"].ToString() : "0"));

            res.Rows[21]["Valor2"] = (double.Parse(res.Rows[19]["Valor2"].ToString() != "" ? res.Rows[19]["Valor2"].ToString() : "0") +
                            double.Parse(res.Rows[20]["Valor2"].ToString() != "" ? res.Rows[20]["Valor2"].ToString() : "0"));

            res.Rows[21]["Valor3"] = (double.Parse(res.Rows[19]["Valor3"].ToString() != "" ? res.Rows[19]["Valor3"].ToString() : "0") +
                            double.Parse(res.Rows[20]["Valor3"].ToString() != "" ? res.Rows[20]["Valor3"].ToString() : "0"));

            res.Rows[21]["ValorAct"] = (double.Parse(res.Rows[19]["ValorAct"].ToString() != "" ? res.Rows[19]["ValorAct"].ToString() : "0") +
                            double.Parse(res.Rows[20]["ValorAct"].ToString() != "" ? res.Rows[20]["ValorAct"].ToString() : "0"));


            try
            {
                ResultadosBusqueda.DataSource = res;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
            ResultadosBusqueda.DataBind();
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        protected void ocultarBotones()
        {
            btnLimpiar.Style.Add("display", "none");
            btnlimpiari.Style.Add("display", "none");
            btnGuardar.Style.Add("display", "none");
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
                //    //&& (ResultadosBusqueda.Rows[i].FindControl("txtAnio2") as TextBox).Text.ToString() != "0"
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

                ////if (!(String.IsNullOrEmpty((ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString()))
                ////    && (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString() != " "
                ////    //&& (ResultadosBusqueda.Rows[i].FindControl("txtAnio3") as TextBox).Text.ToString() != "0"
                ////    && System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[i].Cells[8].Text.ToString()).Trim() != " ")
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

        protected void grabar()
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
            bool ComentarioExiste = true; //(!string.IsNullOrWhiteSpace(txtComentario.Text)) ? true : false;

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
                        exito = Ln.InsertarEstadoResultado(objresumen.idEmpresa.ToString(), xmlDatos, objresumen.idUsuario, txtComentario.Text);
                        if (exito)
                        {
                            dvSuccess.Style.Add("display", "block");
                            lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                            ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                            llenargrid();
                        }
                        else
                        {
                            dvError.Style.Add("display", "block");
                            lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                        }
                    }
                    else
                    {
                        exito = Ln.ModificarEstadoResultado(objresumen.idEmpresa.ToString(), System.Web.HttpUtility.HtmlDecode(ResultadosBusqueda.Rows[0].Cells[14].Text.ToString()).Trim(), xmlDatos, objresumen.idUsuario, txtComentario.Text);
                        if (exito)
                        {
                            dvSuccess.Style.Add("display", "block");
                            lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE);
                            llenargrid();
                        }
                        else
                        {
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

        }

        #endregion

    }
}
