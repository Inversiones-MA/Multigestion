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
            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
                asignacionResumen(ref objresumen);
            }
           
            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            LogicaNegocio LN = new LogicaNegocio();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            ocultarDiv();
            dt = permiso.ListarPerfil(validar);
            Page.Session["dtRol"] = dt;
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["BUSQUEDA"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        //Page.Session["BUSQUEDA"] = null;
                    }

                    Certificado = Page.Request.QueryString["NCertificado"] as string;
                    if (!string.IsNullOrEmpty(Certificado))
                        txtCertificado.Text = Certificado;

                    Page.Session["RESUMEN"] = null;
                    ocultarDiv();
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

        //protected void gridCalendarioPago_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    if (e.Row.Cells.Count > 2)
        //    {
        //        e.Row.Cells[0].Visible = false;
        //        e.Row.Cells[15].Visible = false;
        //    }
        //}

        //protected void gridCalendarioPago_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    LogicaNegocio MTO = new LogicaNegocio();
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        e.Row.Cells[6].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[6].Text), 2).ToString().Replace(".", ","));
        //        e.Row.Cells[7].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[7].Text), 2).ToString().Replace(".", ","));
        //        e.Row.Cells[8].Text = util.actualizaMiles(Math.Round(Convert.ToDecimal(e.Row.Cells[8].Text), 2).ToString().Replace(".", ","));

        //        CheckBox ckb = new CheckBox();
        //        //ckb.CheckedChanged += new System.EventHandler(CheckBoxCkb_CheckedChanged);
        //        ckb.Checked = false;
        //        ckb.AutoPostBack = false;
        //        ckb.ID = 'C' + k.ToString();
        //        ckb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        //        ckb.Attributes["onChange"] = "rowno('" + ckb.ID + "');";
        //        //ckb.Attributes["onChange"] = "rowno(C'" + e.Row.RowIndex + "')";

        //        CheckBox ckbM = new CheckBox();
        //        //ckbM.CheckedChanged += new System.EventHandler(CheckBoxCkbM_CheckedChanged);
        //        ckbM.ID = 'M' + k.ToString();
        //        ckbM.Checked = false;
        //        ckbM.AutoPostBack = false;
        //        ckbM.Attributes["onChange"] = "rowno('" + ckbM.ID + "')";

        //        CheckBox ckbS = new CheckBox();
        //        //ckbS.CheckedChanged += new System.EventHandler(CheckBoxCkbS_CheckedChanged);
        //        ckbS.ID = 'S' + k.ToString();
        //        ckbS.Checked = false;
        //        ckbS.AutoPostBack = false;
        //        ckbS.Attributes["onChange"] = "rowno('" + ckbS.ID + "')";

        //        DateTimeControl cld = new DateTimeControl();
        //        //cld.DateChanged += new System.EventHandler(DateTimeControlcld_DateChanged);
        //        cld.ID = "D" + k.ToString();
        //        cld.DateOnly = true;
        //        cld.AutoPostBack = false;
        //        cld.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        //        cld.LocaleId = 13322;
        //        cld.Calendar = SPCalendarType.Gregorian;
        //        cld.ErrorMessage = "";
        //        ((TextBox)(cld.Controls[0])).Attributes.Add("onKeyPress", "return solonumerosFechas(event)");

        //        Button btn = new Button();
        //        btn.Click += new System.EventHandler(btn_Click);
        //        btn.ID = "B" + k.ToString();
        //        btn.Text = "Guardar";
        //        btn.CssClass = "btn btn-primary btn-success";
        //        btn.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        //        btn.OnClientClick = "Dialogo();";
        //        btn.CommandName = "Editar";

        //        //1 = Cliente
        //        if (e.Row.Cells[15].Text.Trim() == "1")
        //        {
        //            ckb.Checked = true;
        //            ckbM.Enabled = false;
        //            ckbS.Enabled = false;
        //        }

        //        //0 = Subrrogado
        //        if (e.Row.Cells[15].Text.Trim() == "0")
        //        {
        //            ckbM.Checked = true;
        //            ckb.Enabled = false;
        //            ckbS.Enabled = false;
        //        }

        //        // 2 = Siniestrado
        //        if (e.Row.Cells[15].Text.Trim() == "2")
        //        {
        //            ckbS.Checked = true;
        //            ckbM.Enabled = false;
        //            ckb.Enabled = false;
        //        }

        //        if (!string.IsNullOrEmpty(e.Row.Cells[13].Text.ToString()) && !string.IsNullOrWhiteSpace(e.Row.Cells[13].Text.ToString()) && e.Row.Cells[13].Text != "&nbsp;")
        //        {
        //            string fechaSalida = e.Row.Cells[13].Text.ToString();
        //            DateTime fechaOriginal = Convert.ToDateTime(e.Row.Cells[13].Text.ToString().Split('-')[1] + "-" + e.Row.Cells[13].Text.ToString().Split('-')[0] + "-" + e.Row.Cells[13].Text.ToString().Split('-')[2]); ;

        //            if (fechaOriginal != null)
        //                cld.SelectedDate = fechaOriginal;
        //        }

        //        e.Row.Cells[10].Controls.Add(ckb);
        //        e.Row.Cells[11].Controls.Add(ckbM);
        //        e.Row.Cells[12].Controls.Add(ckbS);
        //        e.Row.Cells[13].Controls.Add(cld);
        //        e.Row.Cells[14].Controls.Add(btn);

        //        k++;
        //    }
        //}

        //protected void btn_Click(object sender, EventArgs e)
        //{
            //Button btn = new Button();
            //btn = ((Button)sender);
            //int index = int.Parse(btn.ID.Substring(1, btn.ID.Length - 1));
            //int cont = 0;
            //DateTime? fecha;
            //DateTime fecha1;
            //LogicaNegocio MTO = new LogicaNegocio();
            //SPWeb app2 = SPContext.Current.Web;
            //string OrigenPago = string.Empty;

            //string usuario = util.ObtenerValor(app2.CurrentUser.Name);
            //var ID = "C" + index + "";

            //var checkCliente = gridCalendarioPago.Rows[index].Cells[10].Controls[0].FindControl("C" + index + "") as CheckBox;
            //if (checkCliente.Checked)
            //    cont = cont + 1;

            //var checkSubrrogado = gridCalendarioPago.Rows[index].Cells[11].Controls[0].FindControl("M" + index + "") as CheckBox;
            //if (checkSubrrogado.Checked)
            //    cont = cont + 1;

            //var checkSiniestrado = gridCalendarioPago.Rows[index].Cells[12].Controls[0].FindControl("S" + index + "") as CheckBox;
            //if (checkSiniestrado.Checked)
            //    cont = cont + 1;

            //var IDd = "D" + index + "";
            //var FechaPago = (DateTimeControl)gridCalendarioPago.Rows[index].Cells[14].Controls[0].FindControl("D" + index + "");

            //if (FechaPago != null && !FechaPago.IsDateEmpty)
            //{
            //    fecha = util.ValidarFecha(FechaPago.SelectedDate.ToString());
            //    if (DateTime.TryParse(fecha.ToString(), out fecha1))
            //    {
            //        ocultarDiv();

            //        if (cont == 0)
            //        {
            //            dvWarning.Style.Add("display", "block");
            //            lbWarning.Text = "debe seleccionar una forma de pago";
            //        }

            //        if (cont > 1)
            //        {
            //            dvWarning.Style.Add("display", "block");
            //            lbWarning.Text = "debe seleccionar solo una opcion de forma de pago";
            //        }

            //        if (cont == 1)
            //        {
            //            if (checkCliente.Checked)
            //                OrigenPago = "1";
            //            if (checkSubrrogado.Checked)
            //                OrigenPago = "0";
            //            if (checkSiniestrado.Checked)
            //                OrigenPago = "2";

            //            if (MTO.ActualizarDatos(gridCalendarioPago.Rows[index].Cells[0].Text.ToString(), gridCalendarioPago.Rows[index].Cells[6].Text.ToString(), gridCalendarioPago.Rows[index].Cells[7].Text.ToString(), gridCalendarioPago.Rows[index].Cells[8].Text.ToString(), OrigenPago, usuario, "Operaciones", FechaPago.SelectedDate))
            //            {
            //                dvSuccess.Style.Add("display", "block");
            //                lbSuccess.Text = "Certificado guardado";
            //            }
            //            else
            //            {
            //                dvError.Style.Add("display", "block");
            //                lbError.Text = "no se puede actualizar el certificado";
            //            }
            //        }

            //        inicializacionGrillas();
            //    }
            //    else
            //    {
            //        dvError.Style.Add("display", "block");
            //        lbError.Text = "debe seleccionar la fecha de pago";
            //    }
            //}
            //else
            //{
            //    dvWarning.Style.Add("display", "block");
            //    lbWarning.Text = "verifique el formato de la fecha";
            //}
        //}

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ocultarDiv();
            //inicializacionGrillas();
        }

        protected void lbCalendarioPago_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("CalendarioPago.aspx");
        }

        protected void lbSeguimiento_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Cartera.aspx");
        }

        protected void gridCalendarioPago_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gridCalendarioPago.PageIndex = e.NewPageIndex;
            //inicializacionGrillas();
        }

        protected void gvCalendarioPago_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            SPWeb app2 = SPContext.Current.Web;
            bool modificar = false;
            LogicaNegocio LN = new LogicaNegocio();

            var fechaPago = e.NewValues["FechaPago"] == null ? null : (DateTime?)Convert.ToDateTime(e.NewValues["FechaPago"].ToString());
            var origenPago = e.NewValues["IdOrigenPago"] == null ? null : (int?)int.Parse(e.NewValues["IdOrigenPago"].ToString());

            int IdCalendario = Convert.ToInt32(e.Keys[gvCalendarioPago.KeyFieldName].ToString());  //e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

            modificar = LN.CP_ActualizarDatos(IdCalendario, util.GetValorDecimal(e.NewValues["Interes"].ToString()), util.GetValorDecimal(e.NewValues["MontoCuota"].ToString()), util.GetValorDecimal(e.NewValues["Capital"].ToString()), origenPago, util.ObtenerValor(app2.CurrentUser.Name), "", fechaPago, e.NewValues["NCredito"].ToString(), e.NewValues["NCertificado"].ToString(), e.NewValues["CuotaN"].ToString(), e.NewValues["NCuota"].ToString(), DateTime.Parse(e.NewValues["FechaVencimiento"].ToString()), "06");

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

                        double A = (util.GetValorDouble(Capital) + util.GetValorDouble(Interes)).Redondear(4);

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
                    //dtCsv.Compute("Sum(Capital)", string.Empty);

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
            //if (ckbVencidos.Checked || ckbPagados.Checked || ckbPendiente.Checked || ckbProximo.Checked)
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
                if (!util.CargosPermitidos(dt.Rows[0]["descCargo"].ToString(), Cargos))
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
            if (!util.CargosPermitidos(dt.Rows[0]["descCargo"].ToString(), Cargos))
            {
                BtnPagoMasivo.Visible = false;
            }
        }

        protected void inicializacionGrillas(string[] parametros)
        {
            LogicaNegocio LN = new LogicaNegocio();
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
                res = LN.CP_GestionCalendarioPagp(parametros[1], parametros[3], parametros[2].Replace(".", "").Replace("-", "").Replace(" ", ""), Convert.ToBoolean(parametros[4]), Convert.ToBoolean(parametros[6]), Convert.ToBoolean(parametros[5]), Convert.ToBoolean(parametros[7]), false, "Operaciones", "Operaciones", "01");

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
                DataSet ds = new DataSet("calendario");
                ds.Tables.Add(dt);

                decimal capInicial = util.GetValorDecimal(txtCapitalInicial.Text.Trim());
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
            LogicaNegocio LN = new LogicaNegocio();

            DataRow[] result = null;
            SPWeb app2 = SPContext.Current.Web;
            util.ObtenerValor(app2.CurrentUser.Name);

            if (!string.IsNullOrEmpty(idCalendario.ToString()))
                result = dt.Select("IdCalendario = '" + idCalendario.ToString() + "'");

            if (result != null)
                insert = LN.CP_ActualizarDatos(int.Parse(idCalendario), util.GetValorDecimal(result[0]["Interes"].ToString()), util.GetValorDecimal(result[0]["MontoCuota"].ToString()), util.GetValorDecimal(result[0]["Capital"].ToString()), int.Parse(OrigenPagoFila.Value.ToString()), util.ObtenerValor(app2.CurrentUser.Name), "", DateTime.Parse(FechaPagoFila.Value.ToString()), "0", "0", "0", "0", DateTime.Now, "02");

            if (insert)
                resultado = "OK";

            return resultado;
        }

        protected string PagarMasivo()
        {
            string resultado = string.Empty;
            bool masivo = false;
            SPWeb app2 = SPContext.Current.Web;
            LogicaNegocio LN = new LogicaNegocio();

            masivo = LN.CP_ActualizarDatos(0, 0, 0, 0, int.Parse(OrigenPagoMasivo.Value.ToString()), util.ObtenerValor(app2.CurrentUser.Name), "", DateTime.Parse(FechaPagoMasivo.Value.ToString()), "0", txtCertificadoMasivo.Text.Trim(), "0", "0", DateTime.Now, "05");

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
            //Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
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
