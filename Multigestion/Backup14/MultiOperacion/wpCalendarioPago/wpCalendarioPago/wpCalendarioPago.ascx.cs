using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using MultigestionUtilidades;
using DevExpress.Web;
using System.IO;
using System.Text;
using DevExpress.Utils;
using System.Linq;
using System.Globalization;
using ClasesNegocio;
using Bd;

namespace MultiOperacion.wpCalendarioPago.wpCalendarioPago
{
    [ToolboxItemAttribute(false)]
    public partial class wpCalendarioPago : WebPart
    {
        private static string pagina = "CalendarioPago.aspx?NCertificado=";
        private static string[] Cargos = { "Administrador", "Jefe Operaciones", "Analista Operaciones" };
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpCalendarioPago()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int k;
        string Certificado;
        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();


        #region Eventos

        protected void Page_Init(object sender, EventArgs e)
        {
            CultureInfo newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            newCulture.NumberFormat.NumberGroupSeparator = ".";
            newCulture.NumberFormat.NumberDecimalSeparator = ",";

            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;

            txtCapitalInicial.JSProperties["cp_separator"] = ",";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
                asignacionResumen(ref objresumen);
            }
          
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = string.Empty,
            };

            dt = validar.ListarPerfil(validar);
            Page.Session["dtRol"] = dt;
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["BUSQUEDA"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                    }

                    Certificado = Page.Request.QueryString["NCertificado"] as string;
                    if (!string.IsNullOrEmpty(Certificado))
                        txtCertificado.Text = Certificado;

                    Page.Session["RESUMEN"] = null;
                    CargarAcreedor();
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

                validarRol(dt);
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

  
        protected void lbCalendarioPago_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("CalendarioPago.aspx");
        }

        protected void lbSeguimiento_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Cartera.aspx");
        }

        protected void gvCalendarioPago_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            SPWeb app2 = SPContext.Current.Web;
            bool modificar = false;
            LogicaNegocio Ln = new LogicaNegocio();

            var fechaPago = e.NewValues["FechaPago"] == null ? null : (DateTime?)Convert.ToDateTime(e.NewValues["FechaPago"].ToString());
            var origenPago = e.NewValues["IdOrigenPago"] == null ? null : (int?)int.Parse(e.NewValues["IdOrigenPago"].ToString());

            int IdCalendario = Convert.ToInt32(e.Keys[gvCalendarioPago.KeyFieldName].ToString());  //e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

            modificar = Ln.CP_ActualizarDatos(IdCalendario, e.NewValues["Interes"].ToString().GetValorDecimal(), e.NewValues["MontoCuota"].ToString().GetValorDecimal(), e.NewValues["Capital"].ToString().GetValorDecimal(), origenPago, util.ObtenerValor(app2.CurrentUser.Name), "", fechaPago, e.NewValues["NCredito"].ToString(), e.NewValues["NCertificado"].ToString(), e.NewValues["CuotaN"].ToString(), e.NewValues["NCuota"].ToString(), DateTime.Parse(e.NewValues["FechaVencimiento"].ToString()), "06");

            gvCalendarioPago.CancelEdit();
            e.Cancel = true;

            string[] parametros = (string[])Page.Session["filtos"];
            inicializacionGrillas(parametros);
        }

        protected void gvCalendarioPago_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            bool borrar = false;
            LogicaNegocio LN = new LogicaNegocio();
            int Id = Convert.ToInt32(e.Values[gvCalendarioPago.KeyFieldName].ToString());
            SPWeb app2 = SPContext.Current.Web;
            util.ObtenerValor(app2.CurrentUser.Name);

            borrar = LN.CP_ActualizarDatos(Id, 0, 0, 0, 0, util.ObtenerValor(app2.CurrentUser.Name), "0", DateTime.Now, "0", "0", "0", "0", DateTime.Now, "04");
            e.Cancel = true;
            string[] parametros = (string[])Page.Session["filtos"];
            inicializacionGrillas(parametros);
            //gvCalendarioPago.JSProperties["cpGrilla"] = "eliminar";
        }

        protected void gvCalendarioPago_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parametros = e.Parameters.Split(',');
            if (parametros[0] == "buscar")
            {
                inicializacionGrillas(parametros);
            }
        }

        protected void gvCalendarioPago_PageIndexChanged(object sender, EventArgs e)
        {
            gvCalendarioPago.DataSource = (DataTable)Page.Session["gvCalendario"];
            gvCalendarioPago.DataBind();
        }

        protected void UpSubirCalendario_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            try
            {
                if (UpSubirCalendario.UploadedFiles != null && UpSubirCalendario.UploadedFiles.Length > 0)
                {
                    int validarExiste = 0;
                    LogicaNegocio ln = new LogicaNegocio();

                    //var dt = new DataTable();
                    //var a = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
                    //var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "File1.xlsx");
                    //var query = "SELECT * C FROM [Hoja1$]";
                    //using (OleDbConnection cn = new OleDbConnection { ConnectionString = ConnectionString(fileName, "No") })
                    //{
                    //    using (OleDbCommand cmd = new OleDbCommand { CommandText = query, Connection = cn })
                    //    {
                    //        cn.Open();

                    //        OleDbDataReader dr = cmd.ExecuteReader();
                    //        dt.Load(dr);
                    //    }

                    //}
                    //if (dt.Rows.Count > 1)
                    //{
                    //    // remove header
                    //    dt.Rows[0].Delete();
                    //}
                    //dt.AcceptChanges();
                    //
                    //DataRow dr = new DataRow();
                    string MonedaCertificado = string.Empty;
                    var dtCsv = util.CSVtoDataTable(e.UploadedFile, ";");

                    for (var i = 0; i < dtCsv.Rows.Count - 1; i++)
                    {
                        var NroCertificado = dtCsv.Rows[i]["NCertificado"].ToString().Trim();
                        if (string.IsNullOrEmpty(NroCertificado.ToString()))
                            throw new Exception("El numero de certificado no puede estar vacio, linea " + i);

                        //validar si existe nro certificado en tabla calendario pago
                        if (i > 0)
                        {
                            if (NroCertificado != dtCsv.Rows[i - 1]["NCertificado"].ToString().Trim())
                            {
                                validarExiste = ln.CP_VerificarCertificado(NroCertificado);
                                //if(validarExiste != 0)
                                //    throw new Exception("El número de certificado ya existe en la tabla calendario de pago, linea " + i);
                            }
                        }
                        else
                            validarExiste = ln.CP_VerificarCertificado(NroCertificado);

                        if (validarExiste != 0)
                            throw new Exception("El número de certificado ya existe en la tabla calendario de pago, linea " + i);

                        var NroCredito = dtCsv.Rows[i]["NCredito"].ToString().Trim();
                        if (string.IsNullOrEmpty(NroCredito.ToString()))
                            throw new Exception("El numero de credito no puede estar vacio, linea " + i);

                        var CuotaNro = dtCsv.Rows[i]["CuotaN"].ToString().Trim();
                        if (string.IsNullOrEmpty(CuotaNro.ToString()))
                           throw new Exception("El numero de cuota no puede estar vacio, linea " + i);
                       
                        var NroCuotas = dtCsv.Rows[i]["NCuota"].ToString().Trim();
                        if (string.IsNullOrEmpty(NroCuotas.ToString()))
                            throw new Exception("El total de cuotas no puede estar vacio, linea " + i);

                        var FechaVencimiento = dtCsv.Rows[i]["FechaVencimiento"].ToString().Trim();
                        if (string.IsNullOrEmpty(FechaVencimiento.ToString()))
                            throw new Exception("La fecha de vencimiento no puede estar vacia, linea " + i);

                        var ValorCuota = dtCsv.Rows[i]["MontoCuota"].ToString().Trim();
                        if (string.IsNullOrEmpty(ValorCuota.ToString()))
                            throw new Exception("El monto de la cuota no puede estar vacio, linea " + i);

                        var Capital = dtCsv.Rows[i]["Capital"].ToString().Trim();
                        if (string.IsNullOrEmpty(Capital.ToString()))
                            throw new Exception("El valor capital no puede estar vacio, linea " + i);

                        var Interes = dtCsv.Rows[i]["Interes"].ToString().Trim();
                        if (string.IsNullOrEmpty(Interes.ToString()))
                            throw new Exception("El valor Interes no puede estar vacio, linea " + i);

                        //double A = (util.GetValorDouble(Capital) + util.GetValorDouble(Interes)).Redondear(4);

                        if ((Capital.GetValorDouble() + Interes.GetValorDouble()).Redondear(4) != ValorCuota.GetValorDouble().Redondear(4))
                            throw new Exception("La suma de capital e interes es distinto del monto cuota, linea " + i);

                        var TipoMoneda = dtCsv.Rows[i]["Moneda"].ToString().Trim();
                        if (string.IsNullOrEmpty(TipoMoneda.ToString()))
                            throw new Exception("El valor Moneda no puede estar vacio, linea " + i);

                        if (i == 0)
                        {
                            MonedaCertificado = ln.CP_VerificarMonedaOperacion(dtCsv.Rows[i]["NCertificado"].ToString().Trim());

                            if (MonedaCertificado.ToLower().Trim() != dtCsv.Rows[i]["Moneda"].ToString().ToLower().Trim())
                                throw new Exception("La moneda especificada en la operacion es distinta de la ingresada en el archivo excel");
                        }
                    }

                    //validar texto capital inicial igual a sumatoria de capital
                    double totalCapital = dtCsv.AsEnumerable().Sum(x => x["Capital"].ToString().GetValorDouble());
                    if (totalCapital.Redondear(4) != txtCapitalInicial.Text.Trim().GetValorDouble().Redondear(4))
                    {
                        throw new Exception("el capital inicial ingresado debe ser igual a la sumatoria de la columna capital del excel");
                    }

                    Page.Session["dtArchivo"] = dtCsv;

                    gvPreCargaCP.DataSource = dtCsv;
                    gvPreCargaCP.DataBind();
                    e.CallbackData = "OK";
                }
            }
            catch (Exception ex)
            {
                e.CallbackData = ex.Message;
            }
        }

        protected void gvPreCargaCP_PageIndexChanged(object sender, EventArgs e)
        {
            gvPreCargaCP.DataSource = (DataTable)Page.Session["dtArchivo"];
            gvPreCargaCP.DataBind();
        }

        protected void cbpCargarExcel_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            if (e.Parameter == "InsertarCalendario")
            {
                e.Result = InsertarCalendario();
            }
        }

        protected void gvPreCargaCP_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "nuevo")
            {
              gvPreCargaCP.DataSource = (DataTable)Page.Session["dtArchivo"];
              gvPreCargaCP.DataBind();
            }

            if (e.Parameters == "limpiar")
            {
               Page.Session["dtArchivo"] = null;
               gvPreCargaCP.DataBind();
            }
        }

        protected void gvCalendarioPago_DataBound(object sender, EventArgs e)
        {
            if (ckbPagados.Checked)
            {
                gvCalendarioPago.Columns["Pagar"].Visible = false;
                gvCalendarioPago.Columns["IdOrigenPago"].Visible = true;
                gvCalendarioPago.Columns["FechaPago"].Visible = true;
            }
            else
            {
                //VALIDAR ROL
                DataTable dt = (DataTable)Page.Session["dtRol"];
                if (!util.EstaPermitido(dt.Rows[0]["descCargo"].ToString(), Cargos))
                {
                    gvCalendarioPago.Columns["Pagar"].Visible = false;
                    gvCalendarioPago.Columns[0].Visible = false;
                }
                else
                    gvCalendarioPago.Columns["Pagar"].Visible = true;

                gvCalendarioPago.Columns["IdOrigenPago"].Visible = false;
                gvCalendarioPago.Columns["FechaPago"].Visible = false;
            }
        }

        protected void cbpPagarFila_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter == "pagarfila")
            {
                e.Result = PagarFila();
            }
        }

        protected void cbpPagarMasivo_Callback(object source, CallbackEventArgs e)
        {
            if (e.Parameter == "pagarMasivo")
            {
                e.Result = PagarMasivo();
            }

            if (e.Parameter == "informacionCertificado")
            {
                e.Result = buscarinformacion();
            }
        }

        #endregion


        #region Metodos

        protected void validarRol(DataTable dt)
        {
            if (!util.EstaPermitido(dt.Rows[0]["descCargo"].ToString(), Cargos))
            {
                BtnPagoMasivo.Visible = false;
            }
        }

        protected void inicializacionGrillas(string[] parametros)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable res;
            try
            {
                //res = LN.GestionCalendarioPagp(ddlAcreedor.Value.ToString(), txtCertificado.Text.ToString(), txtRUT.Text.ToString().Replace(".", "").Replace("-", "").Replace(" ", ""), ckbVencidos.Checked, ckbPendiente.Checked, ckbPagados.Checked, ckbProximo.Checked, false, "Operaciones", "Operaciones");
                //gridCalendarioPago.DataSource = res;
                //gridCalendarioPago.DataBind();
                //k = 0;
                //HdfTabla.Value = util.DataTableToJSONWithStringBuilder(res);
                Page.Session["filtos"] = parametros;
                gvCalendarioPago.JSProperties["cpGrilla"] = null;
                res = Ln.CP_GestionCalendarioPagp(parametros[1], parametros[3], parametros[2].Replace(".", "").Replace("-", "").Replace(" ", ""), Convert.ToBoolean(parametros[4]), Convert.ToBoolean(parametros[6]), Convert.ToBoolean(parametros[5]), Convert.ToBoolean(parametros[7]), false, "Operaciones", "Operaciones", "01");

                Page.Session["gvCalendario"] = res;
                gvCalendarioPago.DataSource = res;
                gvCalendarioPago.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

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

            util.CargaDDLDxx(ddlAcreedor, dt, "Nombre", "IdAcreedor");
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        #endregion

        protected string InsertarCalendario()
        {
            string mensaje = string.Empty;
            LogicaNegocio Ln = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;

            if (Page.Session["dtArchivo"] != null)
            {
                DataTable dt = new DataTable("dtArchivo");
                dt = (DataTable)Page.Session["dtArchivo"];

                foreach (DataRow row in dt.Rows)
                {
                    row["MontoCuota"] = row["MontoCuota"].ToString().GetValorDecimal();
                    row["Capital"] = row["Capital"].ToString().GetValorDecimal();
                    row["Interes"] = row["Interes"].ToString().GetValorDecimal();
                }

                DataSet ds = new DataSet("calendario");
                ds.Tables.Add(dt);

                decimal capInicial = txtCapitalInicial.Text.Trim().GetValorDecimal();
                var xml = util.DatasetToXml(ds);

                if (xml != null)
                {
                    if (Ln.CP_InsertarDatos(util.ObtenerValor(app2.CurrentUser.Name), "", xml, capInicial, "03"))
                        mensaje = string.Format("{0}{1}{2}", "OK", ";", "ingreso correcto");
                }
            }
            else
            {
                mensaje = "error al intentar cargar el calendario de pago";
            }

            return mensaje;
        }

        protected string PagarFila()
        {
            string resultado = string.Empty;
            bool insert = false;
            DataTable dt = (DataTable)Page.Session["gvCalendario"];
            var idCalendario = HdfIndexPago["value"].ToString();
            LogicaNegocio Ln = new LogicaNegocio();

            DataRow[] result = null;
            SPWeb app2 = SPContext.Current.Web;
            util.ObtenerValor(app2.CurrentUser.Name);

            if (!string.IsNullOrEmpty(idCalendario.ToString()))
                result = dt.Select("IdCalendario = '" + idCalendario.ToString() + "'");

            if (result != null)
                insert = Ln.CP_ActualizarDatos(int.Parse(idCalendario), result[0]["Interes"].ToString().GetValorDecimal(), result[0]["MontoCuota"].ToString().GetValorDecimal(), result[0]["Capital"].ToString().GetValorDecimal(), int.Parse(OrigenPagoFila.Value.ToString()), util.ObtenerValor(app2.CurrentUser.Name), "", DateTime.Parse(FechaPagoFila.Value.ToString()), "0", "0", "0", "0", DateTime.Now, "02");

            if (insert)
                resultado = "OK";

            return resultado;
        }

        protected string PagarMasivo()
        {
            string resultado = string.Empty;
            bool masivo = false;
            SPWeb app2 = SPContext.Current.Web;
            LogicaNegocio Ln = new LogicaNegocio();

            masivo = Ln.CP_ActualizarDatos(0, 0, 0, 0, int.Parse(OrigenPagoMasivo.Value.ToString()), util.ObtenerValor(app2.CurrentUser.Name), "", DateTime.Parse(FechaPagoMasivo.Value.ToString()), "0", txtCertificadoMasivo.Text.Trim(), "0", "0", DateTime.Now, "05");

            if (masivo)
                resultado = "OK";

            return resultado;
        }

        private string buscarinformacion()
        {
            LogicaNegocio Ln = new LogicaNegocio();

            string retorno = string.Format("{0}{1}{2}", "informacion", ";", Ln.CP_InformacionCertificado(txtCertificadoMasivo.Text.Trim()));
            return retorno;
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            if (ViewState["RES"] != null)
            {
                asignacionResumen(ref objresumen);
                Page.Response.Redirect(objresumen.linkPrincial);
            }
            else
                Page.Response.Redirect("Cartera.aspx");
        }

        protected void cbpVolver_Callback(object source, CallbackEventArgs e)
        {
            if (ViewState["RES"] != null)
            {
                asignacionResumen(ref objresumen);
                ASPxWebControl.RedirectOnCallback(objresumen.linkPrincial);
            }
            else
                ASPxWebControl.RedirectOnCallback("Cartera.aspx");
        }

    }
}
