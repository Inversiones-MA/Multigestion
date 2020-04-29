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
using System.Globalization;
using System.Xml.Linq;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Xml.Xsl;
using System.IO;
using System.Web.UI;
using MultigestionUtilidades;

namespace MultiOperacion.wpProrrateoGarantias.wpProrrateoGarantias
{
    [ToolboxItemAttribute(false)]
    public partial class wpProrrateoGarantias : WebPart
    {
        private static string pagina = "ProrrateoGarantias.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpProrrateoGarantias()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();


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
                        lbEjecutivo.Text = objresumen.descEjecutivo;

                        llenargrid();
                        validarGuardar();

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias", "Empresa");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            btnGuardar.Enabled = false;
                            pnFormulario.Enabled = false;
                            btnCancel.Style.Add("display", "none");
                            btnGuardar.Style.Add("display", "none");
                        }

                    }
                    else { Page.Response.Redirect("MensajeSession.aspx"); }
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

        protected void lbProrrateo_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("ProrrateoGarantias.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["BUSQUEDA"] = null;
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            asignacionResumen(ref objresumen);
            bool permiteGuardar = validarExcesoGarantias();

            if (permiteGuardar)
            {
                for (int i = 0; i <= ResultadosBusquedaOperaciones.Rows.Count - 1; i++)
                {
                    string tcontra = "";
                    string fconstitucion = "";
                    string fconstitucionDef = "";
                    string fconstitucionAux = DateTime.Now.AddYears(-100).ToString("dd/MM/yyyy");
                    for (int j = 0; j <= ResultadosBusquedaGarantias.Rows.Count - 1; j++)
                    {
                        if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[10].Text)
                           == System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[15].Text)
                           || System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[13].Text) == "Compartida")
                        {
                            if ((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValHipoteca") as TextBox).Text.ToString() != "")
                                if (float.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValHipoteca") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0
                                    && System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[1].Text).Contains("Hipoteca"))
                                    if (!tcontra.Contains(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text))
                                    {
                                        tcontra = tcontra + System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text) + "/";
                                        fconstitucion = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[5].Text);

                                        if (fconstitucion != " ")
                                        {
                                            if (DateTime.ParseExact(fconstitucion, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(fconstitucionAux, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                            {
                                                fconstitucionAux = fconstitucion;
                                                fconstitucionDef = fconstitucion;
                                            }
                                        }
                                    }
                            if ((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValPrendas") as TextBox).Text.ToString() != "")
                                if (float.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValPrendas") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0
                                  && System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[1].Text).Contains("Prenda"))
                                    if (!tcontra.Contains(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text))
                                    {
                                        tcontra = tcontra + System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text) + "/";
                                        fconstitucion = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[5].Text);
                                        if (fconstitucion != " ")
                                        {
                                            if (DateTime.ParseExact(fconstitucion, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(fconstitucionAux, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                            {
                                                fconstitucionAux = fconstitucion;
                                                fconstitucionDef = fconstitucion;
                                            }
                                        }
                                    }
                            if ((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValOtros") as TextBox).Text.ToString() != "")
                                if (float.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValOtros") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0
                                  && System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[1].Text).Contains("Otras")
                                  && !System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[2].Text).Contains("Avales"))
                                    if (!tcontra.Contains(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text))
                                    {
                                        tcontra = tcontra + System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text) + "/";
                                        fconstitucion = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[5].Text);
                                        if (fconstitucion != " ")
                                        {
                                            if (DateTime.ParseExact(fconstitucion, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(fconstitucionAux, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                            {
                                                fconstitucionAux = fconstitucion;
                                                fconstitucionDef = fconstitucion;
                                            }
                                        }
                                    }


                            if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[1].Text).Contains("Otras")
                                  && System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[2].Text).Contains("Avales"))
                                if (!tcontra.Contains(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text))
                                {
                                    tcontra = tcontra + System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[14].Text) + "/";
                                    fconstitucion = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[j].Cells[5].Text);
                                    if (fconstitucion != " ")
                                    {
                                        if (DateTime.ParseExact(fconstitucion, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(fconstitucionAux, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                        {
                                            fconstitucionAux = fconstitucion;
                                            fconstitucionDef = fconstitucion;
                                        }
                                    }
                                }
                        }
                    }
                    //14 tipo contragarantia sola
                    ResultadosBusquedaOperaciones.Rows[i].Cells[14].Text = "";
                    if (tcontra.Length > 0)
                        ResultadosBusquedaOperaciones.Rows[i].Cells[14].Text = tcontra.Substring(0, tcontra.Length - 1);
                    if (fconstitucionDef.Length == 0)
                        ResultadosBusquedaOperaciones.Rows[i].Cells[14].Text = ResultadosBusquedaOperaciones.Rows[i].Cells[11].Text + ", " + "";
                    if (fconstitucionDef.Length > 0)
                        ResultadosBusquedaOperaciones.Rows[i].Cells[14].Text = ResultadosBusquedaOperaciones.Rows[i].Cells[11].Text + ", " + fconstitucionDef;

                    //22 tipo contragarantia fogape
                    if (tcontra.Length > 0)
                        ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text = tcontra.Substring(0, tcontra.Length - 1);
                    if (float.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0)
                        if (tcontra.Length > 0)
                            ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text = tcontra + "9184";
                        else
                            ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text = "9184";


                    if (float.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0)
                        if (fconstitucionDef.Length == 0)
                            ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text = ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text + ", " + ResultadosBusquedaOperaciones.Rows[i].Cells[23].Text;

                    if (float.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0)
                        if (fconstitucionDef.Length > 0)
                        {
                            if (DateTime.ParseExact(ResultadosBusquedaOperaciones.Rows[i].Cells[23].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(fconstitucionDef, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                            {
                                fconstitucionDef = ResultadosBusquedaOperaciones.Rows[i].Cells[23].Text;
                            }
                        }
                    ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text = ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text + ", " + fconstitucionDef;
                }

                string XMLGarantias = generarXMLGarantias();
                string XMLOperaciones = generarXMLServiciosOperacion();

                LogicaNegocio MTO = new LogicaNegocio();
                Boolean exito = true;

                exito = MTO.InsertarProrrateo(objresumen.idEmpresa.ToString(), XMLGarantias, XMLOperaciones, objresumen.idUsuario, objresumen.descCargo);
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = "Ingreso Correcto";
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = util.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE);
                }
                llenargrid();
                validarGuardar();
                //validarPoseeGarantia();
            }
        }

        protected void ResultadosBusquedaOperaciones_DataBound(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Page.Session["DTOperaciones"];

            double TotalHipoteca = 0;
            double TotalPrendas = 0;
            double TotalOtros = 0;
            double TotalFogape = 0;
            double TotalMontosVigentes = 0;
            double TotalMontoSiniestrado = 0;
            double TotalMontoRecuperado = 0;
            double TotalMontoDeudaTotal = 0;
            double coberturaBD = 0;
            string coberturaBD2 = "0";

            for (int i = 0; i <= ResultadosBusquedaOperaciones.Rows.Count - 1; i++)
            {
                if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoVigente"].ToString()) != "")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoVigente"].ToString());

                    TotalMontosVigentes = TotalMontosVigentes + double.Parse(dt.Rows[i]["MontoVigente"].ToString().Replace(".", "").Replace(",", "."));
                }
                else (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text = "0,000";
                ///////////////////////////////MontoSiniestrado
                if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoSiniestrado"].ToString()) != "")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoSiniestrado") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoSiniestrado"].ToString());

                    TotalMontoSiniestrado = TotalMontoSiniestrado + double.Parse(dt.Rows[i]["MontoSiniestrado"].ToString().Replace(".", "").Replace(",", "."));
                }
                else (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoSiniestrado") as TextBox).Text = "0,000";
                //////////////////////////////////MontoRecuperado
                if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoRecuperado"].ToString()) != "")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoRecuperado") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoRecuperado"].ToString());

                    TotalMontoRecuperado = TotalMontoRecuperado + double.Parse(dt.Rows[i]["MontoRecuperado"].ToString().Replace(".", "").Replace(",", "."));
                }
                else (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoRecuperado") as TextBox).Text = "0,000";
                /////////////////////////////MontoDeudaTotal
                if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoDeudaTotal"].ToString()) != "")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["MontoDeudaTotal"].ToString());

                    TotalMontoDeudaTotal = TotalMontoDeudaTotal + double.Parse(dt.Rows[i]["MontoDeudaTotal"].ToString().Replace(".", "").Replace(",", "."));
                }
                else (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text = "0,000";


                //8 porcentaje deuda
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[8].Text) != " ")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtDeuda") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Deuda"].ToString());

                }
                // 9 hipoteca
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[9].Text) != " ")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValHipoteca") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Hipotecas"].ToString());
                    if (dt.Rows[i]["Hipotecas"].ToString() != "")
                        TotalHipoteca = TotalHipoteca + double.Parse(dt.Rows[i]["Hipotecas"].ToString().Replace(".", "").Replace(",", "."));

                }
                // 10 prenda
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[10].Text) != " ")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValPrendas") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Prenda"].ToString());
                    if (dt.Rows[i]["Prenda"].ToString() != "")
                        TotalPrendas = TotalPrendas + double.Parse(dt.Rows[i]["Prenda"].ToString().Replace(".", "").Replace(",", "."));
                }
                // 11 otros
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[11].Text) != " ")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValOtros") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Otros"].ToString());
                    if (dt.Rows[i]["Otros"].ToString() != "")
                        TotalOtros = TotalOtros + double.Parse(dt.Rows[i]["Otros"].ToString().Replace(".", "").Replace(",", "."));
                }
                //19 fogape
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[19].Text) != " ")
                {
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtFogape") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Fogape"].ToString());
                    if (dt.Rows[i]["Fogape"].ToString() != "")
                        TotalFogape = TotalFogape + double.Parse(dt.Rows[i]["Fogape"].ToString().Replace(".", "").Replace(",", "."));
                }

                //12 total garantias uf
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[12].Text) != " ")
                {
                    double a = 0;
                    if (dt.Rows[i]["Hipotecas"].ToString() != "")
                        a = a + double.Parse(dt.Rows[i]["Hipotecas"].ToString().Replace(".", "").Replace(",", "."));
                    if (dt.Rows[i]["Prenda"].ToString() != "")
                        a = a + double.Parse(dt.Rows[i]["Prenda"].ToString().Replace(".", "").Replace(",", "."));
                    if (dt.Rows[i]["Otros"].ToString() != "")
                        a = a + double.Parse(dt.Rows[i]["Otros"].ToString().Replace(".", "").Replace(",", "."));

                    if (dt.Rows[i]["Otros"].ToString() != "")
                        a = a + double.Parse(dt.Rows[i]["Otros"].ToString().Replace(".", "").Replace(",", "."));

                    double b = 0;
                    if (dt.Rows[i]["Fogape"].ToString() != "")
                        b = a + double.Parse(dt.Rows[i]["Fogape"].ToString().Replace(".", "").Replace(",", "."));

                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantias") as TextBox).Text = Convert.ToDecimal(a).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantiasFogape") as TextBox).Text = Convert.ToDecimal(b).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

                }
                //14 tipo contragarantia
                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[14].Text) != "")
                {
                    if ((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantias") as TextBox).Text.ToString() != "" &&
                        double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantias") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0
                        && (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString() != ""
                        && double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0)
                    {
                        coberturaBD = double.Parse(dt.Rows[i]["Cobertura"].ToString().Replace(".", "").Replace(",", "."));
                        coberturaBD2 = Convert.ToDecimal((double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantias") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) / double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                        (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCobertura") as TextBox).Text = Convert.ToDecimal((double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantias") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) / double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                        //(ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCobertura") as TextBox).Text = Convert.ToDecimal((double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantias") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) / double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else
                        (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCobertura") as TextBox).Text = "0,000";
                }
                else
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCobertura") as TextBox).Text = "0,000";


                if (System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[21].Text) != " " || System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[21].Text) != "")
                {
                    if ((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantiasFogape") as TextBox).Text.ToString() != "" &&
                        double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantiasFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0
                        && (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString() != ""
                        && double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) > 0)
                    {
                        var TotalGarantiasFogape = double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantiasFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                        var txtMontoVigente = double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) == 0 ? 1 : double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));

                        //(ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCoberturaFogape") as TextBox).Text = Convert.ToDecimal(TotalGarantiasFogape / (txtMontoVigente * 100)).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                        (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCoberturaFogape") as TextBox).Text = Convert.ToDecimal((double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("TotalGarantiasFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")) / double.Parse((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else
                        (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCoberturaFogape") as TextBox).Text = "0,000";
                }


            }
            if (ResultadosBusquedaOperaciones.Rows.Count > 0)
            {
                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalHipotecas") as TextBox).Text = Convert.ToDecimal(TotalHipoteca.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalPrendas") as TextBox).Text = Convert.ToDecimal(TotalPrendas.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalOtros") as TextBox).Text = Convert.ToDecimal(TotalOtros.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalFogape") as TextBox).Text = Convert.ToDecimal(TotalFogape.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalGarantias") as TextBox).Text =
                    Convert.ToDecimal((TotalHipoteca + TotalPrendas + TotalOtros).ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalGarantiasFogape") as TextBox).Text =
                  Convert.ToDecimal((TotalHipoteca + TotalPrendas + TotalOtros + TotalFogape).ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));


                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalMontoVigente") as TextBox).Text =
                    Convert.ToDecimal(TotalMontosVigentes.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalMontoSiniestrado") as TextBox).Text =
                    Convert.ToDecimal(TotalMontoSiniestrado.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalMontoRecuperado") as TextBox).Text =
                    Convert.ToDecimal(TotalMontoRecuperado.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

                (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalMontoDeudaTotal") as TextBox).Text =
                    Convert.ToDecimal(TotalMontoDeudaTotal.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));


                if (TotalHipoteca + TotalPrendas + TotalOtros > 0 && TotalMontoDeudaTotal > 0)
                    (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalCobertura") as TextBox).Text = Convert.ToDecimal(((TotalHipoteca + TotalPrendas + TotalOtros) / TotalMontosVigentes) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                //Convert.ToDecimal(((TotalHipoteca + TotalPrendas + TotalOtros) / TotalMontoDeudaTotal) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalCobertura") as TextBox).Text = "0,000";

                if (TotalHipoteca + TotalPrendas + TotalOtros > 0 && TotalMontoDeudaTotal > 0 && TotalFogape > 0)
                    (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalCoberturaFogape") as TextBox).Text = Convert.ToDecimal(((TotalHipoteca + TotalPrendas + TotalOtros + TotalFogape) / TotalMontosVigentes) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                //Convert.ToDecimal(((TotalHipoteca + TotalPrendas + TotalOtros + TotalFogape) / TotalMontoDeudaTotal) * 100).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                else
                    (ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalCoberturaFogape") as TextBox).Text = "0,000";

            }
            for (int rowIndex = ResultadosBusquedaOperaciones.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow gvRow = ResultadosBusquedaOperaciones.Rows[rowIndex];
                GridViewRow gvPreviousRow = ResultadosBusquedaOperaciones.Rows[rowIndex + 1];
                for (int cellCount = 0; cellCount < 1; cellCount++)
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
            for (int i = 0; i <= ResultadosBusquedaOperaciones.Rows.Count - 1; i++)
            {
                (ResultadosBusquedaOperaciones.Rows[i].FindControl("Editar") as LinkButton).Attributes.Add("onClick", "return HabilitarText('" + ResultadosBusquedaOperaciones.Rows[i].ClientID + "');");


                //function CalculoHipoteca(IDgridOperaciones, cadenaGrid, cadena) {
                (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValOtros") as TextBox).Attributes.Add("onBlur", "return CalculoOtros('" + ResultadosBusquedaOperaciones.ClientID +
                "','" + ResultadosBusquedaOperaciones.Rows[i].ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + ResultadosBusquedaOperaciones.Rows[i].Cells[4].Text.ToString() + "');");

                (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValPrendas") as TextBox).Attributes.Add("onBlur", "return CalculoPrendas('" + ResultadosBusquedaOperaciones.ClientID +
               "','" + ResultadosBusquedaOperaciones.Rows[i].ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + ResultadosBusquedaOperaciones.Rows[i].Cells[4].Text.ToString() + "');");


                (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValHipoteca") as TextBox).Attributes.Add("onBlur", "return CalculoHipoteca('" + ResultadosBusquedaOperaciones.ClientID +
               "','" + ResultadosBusquedaOperaciones.Rows[i].ClientID + "','" + btnGuardar.ClientID.Substring(0, btnGuardar.ClientID.Length - 10) + "','" + ResultadosBusquedaOperaciones.Rows[i].Cells[4].Text.ToString() + "');");


            }
            for (int i = 0; i < ResultadosBusquedaOperaciones.Rows.Count; i++)
            {
                if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Cobertura"].ToString()) != "0,000")
                {
                    //bloquear textbox
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValHipoteca") as TextBox).Enabled = false;
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValPrendas") as TextBox).Enabled = false;
                    (ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValOtros") as TextBox).Enabled = false;
                }
            }
        }

        public double SumaAjustado = 0;
        public double SumaComercial = 0;
        public double SumaLimite = 0;

        protected void ResultadosBusquedaGarantias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                e.Row.Cells[0].Text = "Total";
                e.Row.Cells[5].Text = hddFechaContrato.Value;
                e.Row.Cells[2].Text = hddTipoContragarantia.Value;

                e.Row.Cells[7].Text = SumaAjustado.ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                e.Row.Cells[8].Text = SumaComercial.ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                e.Row.Cells[9].Text = SumaLimite.ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));

            }
            else
            {
                if (e.Row.RowType != DataControlRowType.Header)
                {//&nbsp;
                    if (e.Row.Cells[7].Text.ToString() != "&nbsp;")
                        SumaAjustado += double.Parse(e.Row.Cells[7].Text.ToString().Replace(".", "").Replace(",", "."));
                    if (e.Row.Cells[8].Text.ToString() != "&nbsp;")
                        SumaComercial += double.Parse(e.Row.Cells[8].Text.ToString().Replace(".", "").Replace(",", "."));
                    if (e.Row.Cells[9].Text.ToString() != "&nbsp;")
                        SumaLimite += double.Parse(e.Row.Cells[9].Text.ToString().Replace(".", "").Replace(",", "."));
                }
            }

        }

        protected void ActualizarOperaciones_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet DS = new DataSet();
            DS = MTO.ActualizarDatosProrrateo(objresumen.idEmpresa.ToString(), "0", objresumen.idUsuario, objresumen.descCargo);
            // Page.Session["DTResumen"] = DS.Tables[1];
            Page.Session["DTOperaciones"] = DS.Tables[2];
            ResultadosBusquedaOperaciones.DataSource = DS.Tables[2];
            ResultadosBusquedaOperaciones.DataBind();
            validarGuardar();
            // validarPoseeGarantia();
        }

        protected void ActualizarGarantias_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet DS = new DataSet();
            DS = MTO.ActualizarDatosProrrateo(objresumen.idEmpresa.ToString(), "0", objresumen.idUsuario, objresumen.descCargo);
            Page.Session["DTResumen"] = DS.Tables[1];
            // Page.Session["DTOperaciones"] = DS.Tables[2];
            gridResumen.DataSource = DS.Tables[1];
            gridResumen.DataBind();

            ResultadosBusquedaGarantias.DataSource = DS.Tables[0];
            ResultadosBusquedaGarantias.DataBind();
            validarGuardar();
            // validarPoseeGarantia();
        }

        protected void ResultadosBusquedaOperaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ResultadosBusquedaGarantias_DataBound(object sender, EventArgs e)
        {

        }

        protected void gridResumen_DataBound(object sender, EventArgs e)
        {
            // asignacionResumen(ref objresumen);
            DataTable dt = new DataTable();
            dt = (DataTable)Page.Session["DTResumen"];

            double TotalGarantiaHipoteca = 0;
            double TotalGarantiaPrendas = 0;
            double TotalGarantiaOtros = 0;

            for (int i = 0; i <= gridResumen.Rows.Count - 1; i++)
            {
                if (System.Web.HttpUtility.HtmlDecode(gridResumen.Rows[i].Cells[2].Text) != " ")
                {
                    if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Hipoteca"].ToString()) != "")
                    {
                        (gridResumen.Rows[i].FindControl("Hipoteca") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Hipoteca"].ToString());
                        TotalGarantiaHipoteca = TotalGarantiaHipoteca + double.Parse(dt.Rows[i]["Hipoteca"].ToString().Replace(".", "").Replace(",", "."));
                    }
                    else
                        (gridResumen.Rows[i].FindControl("Hipoteca") as TextBox).Text = "0,000";
                    if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Prendas"].ToString()) != "")
                    {
                        (gridResumen.Rows[i].FindControl("Prendas") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Prendas"].ToString());
                        TotalGarantiaPrendas = TotalGarantiaPrendas + double.Parse(dt.Rows[i]["Prendas"].ToString().Replace(".", "").Replace(",", "."));
                    }
                    else
                        (gridResumen.Rows[i].FindControl("Prendas") as TextBox).Text = "0,000";
                    if (System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Otros"].ToString()) != "")
                    {
                        (gridResumen.Rows[i].FindControl("Otros") as TextBox).Text = System.Web.HttpUtility.HtmlDecode(dt.Rows[i]["Otros"].ToString());
                        TotalGarantiaOtros = TotalGarantiaOtros + double.Parse(dt.Rows[i]["Otros"].ToString().Replace(".", "").Replace(",", "."));
                    }
                    else
                        (gridResumen.Rows[i].FindControl("Otros") as TextBox).Text = "0,000";
                }
            }

            if (gridResumen.Rows.Count > 0)
            {
                (gridResumen.FooterRow.FindControl("txtTotalHipoteca") as TextBox).Text = Convert.ToDecimal(TotalGarantiaHipoteca.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                (gridResumen.FooterRow.FindControl("txtTotalPrendas") as TextBox).Text = Convert.ToDecimal(TotalGarantiaPrendas.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                (gridResumen.FooterRow.FindControl("txtTotalOtros") as TextBox).Text = Convert.ToDecimal(TotalGarantiaOtros.ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
            }
        }

        protected void ResultadosBusquedaGarantias_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[10].Visible = false; //IdEmpresa
            e.Row.Cells[11].Visible = false; //IdGarantia
            e.Row.Cells[12].Visible = false; //OrdenEmpresa
            e.Row.Cells[13].Visible = false; //TipoEmpresa 
            e.Row.Cells[14].Visible = false; //TipoContragarantia  
        }

        protected void gridResumen_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //e.Row.Cells[12].Visible = false;
            //e.Row.Cells[13].Visible = false;
        }

        protected void ResultadosBusquedaOperaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[14].Visible = false;//tipo COntagarantia
            e.Row.Cells[15].Visible = false;//idEmpresa
            e.Row.Cells[16].Visible = false;//idoperacion
            e.Row.Cells[17].Visible = false;//ordenanexo
            e.Row.Cells[18].Visible = false;//ordenCorfo
            e.Row.Cells[23].Visible = false;//Fecha Emisión
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            asignacionResumen(ref objresumen);

            LogicaNegocio MTO = new LogicaNegocio();
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos["IdEmpresa"] = objresumen.idEmpresa.ToString();

            string Reporte = "";
            Reporte = "ProrrateoGarantias";

            byte[] archivo = GenerarReporte(Reporte, datos, "", objresumen);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }

        }

        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        {
            LogicaNegocio LN = new LogicaNegocio();
            try
            {
                String xml = String.Empty;

                //IF PARA CADA REPORTE
                DataSet res1 = new DataSet();
                if (sp == "ProrrateoGarantias")
                {
                    res1 = LN.ConsultaProrrateoGarantias(datos["IdEmpresa"], "DocumentoCurse", "admin", "ReporteProrrateoEmpresa");
                    for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                    {
                        xml = xml + res1.Tables[0].Rows[i][0].ToString();
                    }
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");
                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        protected void lbDeudaSBIF_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaDeudaSBIF.aspx");
        }

        protected void lbClientes_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaClientes.aspx");
        }

        protected void lbAdministracion_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaAdministracion.aspx");
        }

        protected void lbProveedores_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "ProrrateoGarantias");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaProveedores.aspx");
        }

        #endregion


        #region Metodos

        public bool validarExcesoGarantias()
        {
            double montoUsadoHipotecas = 0;
            double montoUsadoPrendas = 0; 
            double montoUsadoOtros = 0; 
            double montoUsadoFogape = 0;
            try
            {
                for (int j = 0; j <= ResultadosBusquedaOperaciones.Rows.Count - 1; j++)
                {
                    if ((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtValHipoteca") as TextBox).Text.ToString() != "")
                        montoUsadoHipotecas = montoUsadoHipotecas + float.Parse((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtValHipoteca") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                    if ((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtValPrendas") as TextBox).Text.ToString() != "")
                        montoUsadoPrendas = montoUsadoPrendas + float.Parse((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtValPrendas") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                    if ((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtValOtros") as TextBox).Text.ToString() != "")
                        montoUsadoOtros = montoUsadoOtros + float.Parse((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtValOtros") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                    if ((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtFogape") as TextBox).Text.ToString() != "")
                        montoUsadoFogape = montoUsadoFogape + float.Parse((ResultadosBusquedaOperaciones.Rows[j].FindControl("txtFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));

                }
                double totalGaratiasHipoteca = 0;
                double totalGaratiasPrendas = 0;
                double totalGaratiasOtros = 0;
                if (((gridResumen.FooterRow.FindControl("txtTotalHipoteca") as TextBox).Text.ToString() != ""))
                    totalGaratiasHipoteca = float.Parse((gridResumen.FooterRow.FindControl("txtTotalHipoteca") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                if (((gridResumen.FooterRow.FindControl("txtTotalPrendas") as TextBox).Text.ToString() != ""))
                    totalGaratiasPrendas = float.Parse((gridResumen.FooterRow.FindControl("txtTotalPrendas") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                if (((gridResumen.FooterRow.FindControl("txtTotalOtros") as TextBox).Text.ToString() != ""))
                    totalGaratiasOtros = float.Parse((gridResumen.FooterRow.FindControl("txtTotalOtros") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));

                double TotalTodasPrendas = totalGaratiasHipoteca + totalGaratiasPrendas + totalGaratiasOtros;

                if (TotalTodasPrendas < montoUsadoHipotecas)
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = " Se ha prorrateado más garantías de lo disponible en:";


                    //if (totalGaratiasHipoteca < montoUsadoHipotecas)
                    //{
                    //    dvError.Style.Add("display", "block");
                    //    lbError.Text = lbError.Text + " (Tipo Hipoteca)";
                    //}
                    //if (totalGaratiasPrendas < montoUsadoPrendas)
                    //{
                    //    dvError.Style.Add("display", "block");
                    //    lbError.Text = lbError.Text + " (Tipo Prendas)";
                    //}
                    //if (totalGaratiasOtros < montoUsadoOtros)
                    //{
                    //    dvError.Style.Add("display", "block");
                    //    lbError.Text = lbError.Text + " (Tipo Otras Garatías)";
                    //}
                    //lbError.Text = lbError.Text + ". Sus cambios no han sido guardados. ";


                    return false;
                }
                else { return true; }
            }
            catch (Exception)
            {
                if (gridResumen.Rows.Count < 1 && montoUsadoOtros + montoUsadoHipotecas + montoUsadoOtros == 0)
                {
                    return true;
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = "Sus cambios no han sido Guardados. No posee garantías para su distribución.";
                    return false;
                }
            }
        }

        public void validarGuardar()
        {
            try
            {
                double? totalGaratias = 0;
                double? montoUsadoGarantias = float.Parse((ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalHipotecas") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))
                      + float.Parse((ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalPrendas") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))
                      + float.Parse((ResultadosBusquedaOperaciones.FooterRow.FindControl("txtTotalOtros") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                try
                {
                    totalGaratias = float.Parse((gridResumen.FooterRow.FindControl("txtTotalHipoteca") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))
                         + float.Parse((gridResumen.FooterRow.FindControl("txtTotalPrendas") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))
                         + float.Parse((gridResumen.FooterRow.FindControl("txtTotalOtros") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."));
                }
                catch (Exception ex)
                {
                    totalGaratias = 0;
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }

                if (montoUsadoGarantias != null && totalGaratias != null)
                {
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = "El monto total de garantía disponible para el prorrateo es: " + Convert.ToDecimal((totalGaratias - montoUsadoGarantias).ToString()).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES")) + " UF";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public bool validarPoseeGarantia()
        {
            try
            {
                if (gridResumen.Rows.Count < 1)
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = " Usted no posee garantías para realizar la distribución. ";
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public void llenargrid()
        {
            LogicaNegocio MTO = new LogicaNegocio();
            asignacionResumen(ref objresumen);
            DataSet DS = new DataSet();
            DS = MTO.ActualizarDatosProrrateo(objresumen.idEmpresa.ToString(), "0", objresumen.idUsuario, objresumen.descCargo);
            Page.Session["DTResumen"] = DS.Tables[1];
            Page.Session["DTOperaciones"] = DS.Tables[2];
            gridResumen.DataSource = DS.Tables[1];
            gridResumen.DataBind();

            ResultadosBusquedaGarantias.DataSource = DS.Tables[0];
            ResultadosBusquedaGarantias.DataBind();

            ResultadosBusquedaOperaciones.DataSource = DS.Tables[2];
            ResultadosBusquedaOperaciones.DataBind();
        }

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

        string generarXMLGarantias()
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            for (int i = 0; i <= ResultadosBusquedaGarantias.Rows.Count - 1; i++)
            {
                XmlNode RespNodeCkb;
                RespNodeCkb = doc.CreateElement("Garantias");

                XmlNode nodo = doc.CreateElement("Empresa");
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[0].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("Clase");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo1);

                XmlNode nodo2 = doc.CreateElement("TipoBien");
                nodo2.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[2].Text)));
                RespNodeCkb.AppendChild(nodo2);

                XmlNode nodo3 = doc.CreateElement("Descripcion");
                nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[3].Text)));
                RespNodeCkb.AppendChild(nodo3);

                XmlNode nodo4 = doc.CreateElement("NroInscripcion");
                nodo4.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[4].Text)));
                RespNodeCkb.AppendChild(nodo4);

                XmlNode nodo5 = doc.CreateElement("FechaConstitucion");
                if (ResultadosBusquedaGarantias.Rows[i].Cells[5].Text == "&nbsp;")
                    nodo5.AppendChild(doc.CreateTextNode("-1"));
                else
                    nodo5.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[5].Text)));
                RespNodeCkb.AppendChild(nodo5);

                XmlNode nodo6 = doc.CreateElement("Estado");
                nodo6.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[6].Text)));
                RespNodeCkb.AppendChild(nodo6);

                XmlNode nodo7 = doc.CreateElement("ValorAjustado");
                nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[7].Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo7);

                XmlNode nodo8 = doc.CreateElement("ValorComercial");
                nodo8.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[8].Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("MontoLimite");
                nodo9.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[9].Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("IdEmpresa");
                nodo10.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[10].Text)));
                RespNodeCkb.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("IdGarantia");
                nodo11.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[11].Text)));
                RespNodeCkb.AppendChild(nodo11);

                //   ,OrdenEmpresa					,TipoEmpresa

                XmlNode nodo12 = doc.CreateElement("OrdenEmpresa");
                nodo12.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[12].Text)));
                RespNodeCkb.AppendChild(nodo12);

                XmlNode nodo13 = doc.CreateElement("TipoEmpresa");
                nodo13.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[13].Text)));
                RespNodeCkb.AppendChild(nodo13);

                XmlNode nodo14 = doc.CreateElement("TipoContragarantia");
                nodo14.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaGarantias.Rows[i].Cells[14].Text)));
                RespNodeCkb.AppendChild(nodo14);

                RespNode.AppendChild(RespNodeCkb);
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        string generarXMLServiciosOperacion()
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            for (int i = 0; i <= ResultadosBusquedaOperaciones.Rows.Count - 1; i++)
            {
                XmlNode RespNodeCkb;
                RespNodeCkb = doc.CreateElement("Operaciones");

                XmlNode nodo = doc.CreateElement("Empresa");
                nodo.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[0].Text)));
                RespNodeCkb.AppendChild(nodo);

                XmlNode nodo1 = doc.CreateElement("NCF_Producto");
                nodo1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[1].Text)));
                RespNodeCkb.AppendChild(nodo1);

                XmlNode nodo2 = doc.CreateElement("Estado");
                nodo2.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[2].Text)));
                RespNodeCkb.AppendChild(nodo2);

                XmlNode nodo3 = doc.CreateElement("Fondo");
                nodo3.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[3].Text)));
                RespNodeCkb.AppendChild(nodo3);

                XmlNode nodo4 = doc.CreateElement("MontoVigente");
                nodo4.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoVigente") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo4);

                XmlNode nodo3ms = doc.CreateElement("MontoSiniestrado");
                nodo3ms.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoSiniestrado") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo3ms);

                XmlNode nodo4ms = doc.CreateElement("MontoRecuperado");
                nodo4ms.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoRecuperado") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo4ms);

                XmlNode nodo5ms = doc.CreateElement("MontoDeudaTotal");
                nodo5ms.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtMontoDeudaTotal") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo5ms);

                XmlNode nodo5 = doc.CreateElement("Deuda");
                nodo5.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtDeuda") as TextBox).Text.ToString().Replace("%", "").Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo5);

                XmlNode ValorAnio1 = doc.CreateElement("Hipotecas");
                ValorAnio1.AppendChild(doc.CreateTextNode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValHipoteca") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".")));
                RespNodeCkb.AppendChild(ValorAnio1);

                XmlNode nodo7 = doc.CreateElement("Prendas");
                nodo7.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValPrendas") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo7);

                XmlNode nodo8 = doc.CreateElement("Otros");
                nodo8.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtValOtros") as TextBox).Text.ToString().Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("CoberturaPorc");
                nodo9.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCobertura") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".").Replace("%", ""))));
                RespNodeCkb.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("IdEmpresa");
                nodo10.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[15].Text)));
                RespNodeCkb.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("IdGOperacion");
                nodo11.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[16].Text)));
                RespNodeCkb.AppendChild(nodo11);

                XmlNode nodo12 = doc.CreateElement("OrdenAnexo");
                nodo12.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[17].Text)));
                RespNodeCkb.AppendChild(nodo12);

                XmlNode nodo13 = doc.CreateElement("OrdenCorfoInterno");
                nodo13.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[18].Text)));
                RespNodeCkb.AppendChild(nodo13);

                string[] words = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[14].Text).Split(',');

                XmlNode nodo14 = doc.CreateElement("TipoContragarantia");
                nodo14.AppendChild(doc.CreateTextNode(words[0]));
                RespNodeCkb.AppendChild(nodo14);

                if (words.Length > 1)
                {
                    XmlNode nodo15 = doc.CreateElement("FechaConstitucion");

                    if (words[1] == "")
                        nodo15.AppendChild(doc.CreateTextNode("-1"));
                    else
                        nodo15.AppendChild(doc.CreateTextNode(words[1]));
                    RespNodeCkb.AppendChild(nodo15);
                }


                //FOgape
                XmlNode nodoF1 = doc.CreateElement("Fogape");
                nodoF1.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtFogape") as TextBox).Text.ToString().Replace("-", "0").Replace(".", "").Replace(",", "."))));
                RespNodeCkb.AppendChild(nodoF1);
                //Cobertura Fogape
                XmlNode nodoF2 = doc.CreateElement("CoberturaFogape");
                nodoF2.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode((ResultadosBusquedaOperaciones.Rows[i].FindControl("txtCoberturaFogape") as TextBox).Text.ToString().Replace(".", "").Replace(",", ".").Replace("%", ""))));
                RespNodeCkb.AppendChild(nodoF2);
                //Tipo Contragarantia Fecha Constitucio Fogape
                string[] words2 = System.Web.HttpUtility.HtmlDecode(ResultadosBusquedaOperaciones.Rows[i].Cells[22].Text).Split(',');

                XmlNode nodoF4 = doc.CreateElement("TipoContragarantiaFogape");
                nodoF4.AppendChild(doc.CreateTextNode(words2[0]));
                RespNodeCkb.AppendChild(nodoF4);

                if (words2.Length > 1)
                {
                    XmlNode nodoF5 = doc.CreateElement("FechaConstitucionFogape");

                    if (words2[1] == "")
                        nodoF5.AppendChild(doc.CreateTextNode("-1"));
                    else
                        nodoF5.AppendChild(doc.CreateTextNode(words2[1]));
                    RespNodeCkb.AppendChild(nodoF5);
                }

                RespNode.AppendChild(RespNodeCkb);
            }

            ValoresNode.AppendChild(RespNode);
            return doc.OuterXml;
        }

        #endregion

    }
}
