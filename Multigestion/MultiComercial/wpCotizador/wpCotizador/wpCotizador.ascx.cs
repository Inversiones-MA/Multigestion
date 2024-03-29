﻿using DevExpress.Web;
using System;
using System.Data;
using System.Net;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Linq;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ClasesNegocio;
using Bd;

namespace MultiComercial.wpCotizador.wpCotizador
{
    [ToolboxItemAttribute(false)]
    public partial class wpCotizador : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpCotizador()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Utilidades util = new Utilidades();
        Resumen objresumen = new Resumen();

        private string cargo = string.Empty;
        private string nombre = string.Empty;
        private static string pagina = "Cotizador.aspx";

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////PERMISOS USUARIOS
                SPWeb app2 = SPContext.Current.Web;
                DataTable dt = new DataTable("dt");
                ValidarPermisos validar = new ValidarPermisos
                {
                    NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                    Pagina = pagina,
                    Etapa = "",
                };

                dt = validar.ListarPerfil(validar);
                if (dt.Rows.Count > 0)
                {
                    cargo = dt.Rows[0]["descCargo"].ToString().Trim();
                    nombre = validar.NombreUsuario;
                    if (!Page.IsPostBack)
                    {
                        Page.Session["TipoBien"] = null;
                        Page.Session["SeguroAval"] = null;

                        if (Page.Session["RESUMEN"] != null)
                        {
                            asignacionResumen(ref objresumen);
                            datosEmpresa.Visible = true;
                            lbEmpresa.Text = System.Web.HttpUtility.HtmlDecode(objresumen.desEmpresa);
                            lbRut.Text = objresumen.rut;
                            lbEjecutivo.Text = System.Web.HttpUtility.HtmlDecode(objresumen.descEjecutivo);
                            lbOperacion.Text = System.Web.HttpUtility.HtmlDecode(objresumen.desOperacion);
                            Page.Session["IdEmpresa"] = objresumen.idEmpresa;
                            Page.Session["IdCotizacion"] = objresumen.IdCotizacion.ToString();
                        }
                        else
                        {
                            Page.Session["IdCotizacion"] = "0";
                            Page.Session["IdEmpresa"] = "0";
                            lbEmpresa.Text = string.Empty;
                            lbRut.Text = string.Empty;
                            lbEjecutivo.Text = string.Empty;
                            lbOperacion.Text = string.Empty;
                            datosEmpresa.Visible = false;
                            //CargarGarantias(0);

                            CargarGarantiasList(new List<SeguroGarantia>());
                            CargarAvalList(new List<SeguroAval>());
                        }

                        if (!Page.IsCallback)
                        {
                            CargarEjecutivo(dt.Rows[0]["descCargo"].ToString().Trim(), validar.NombreUsuario);
                            CargarTipoMoneda();
                            CargarTipoCredito();
                            CargarTipoAmortizacion();
                            CargarPlazo();
                            CargarFondo();
                            CargarValorMoneda();
                        }

                        if (objresumen.IdCotizacion.ToString() != "0")
                        {
                            Page.Session["IdCotizacion"] = objresumen.IdCotizacion.ToString();
                            CargarCotizacion(objresumen.IdCotizacion);
                        }

                        //CargarCotizacion(732);
                    }
                    else
                    {
                        if (Page.IsCallback)
                        {
                            CargarEjecutivo(dt.Rows[0]["descCargo"].ToString(), validar.NombreUsuario);
                            CargarTipoMoneda();
                            CargarTipoCredito();
                            CargarTipoAmortizacion();
                            CargarPlazo();
                            CargarFondo();
                            CargarValorMoneda();
                        }
                    }
                    validar.idUsuario = int.Parse(dt.Rows[0]["idUsuario"].ToString().Trim());
                    Page.Session["validar"] = validar;
                }
                else
                {
                    dvFormulario.Style.Add("display", "none");
                    warning("Usuario sin permisos configurados");
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        //protected void cbxTipoBien_Init(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxComboBox cbxTipoBien = (ASPxComboBox)sender;
        //        List<TipoBien> TipoBien = new List<TipoBien>();
        //        TipoBien = CargarTipoBien();

        //        cbxTipoBien.DataSource = TipoBien;
        //        cbxTipoBien.TextField = "Nombre";
        //        cbxTipoBien.ValueField = "Id";
        //        cbxTipoBien.ValueType = typeof(System.Int32);
        //        cbxTipoBien.DataBindItems();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //    }
        //}

        protected void ASPxCallback1_Callback(object source, CallbackEventArgs e)
        {
            string resultado = string.Empty;
            switch (e.Parameter)
            {
                case "guardar":
                    Cotizaciones cotizacion = new Cotizaciones();
                    insertCotizacion(ref cotizacion);
                    if (cotizacion != null)
                    {
                        if (cotizacion.IdCotizacion == 0)
                            resultado = EnviarCotizacion(cotizacion);
                        else
                            resultado = ActualizarCotizacion(cotizacion);

                        if (resultado.IsNumeric())
                            e.Result = "ok";
                        else
                            e.Result = "error";
                    }
                    else
                        e.Result = "session";
                    break;
                case "Imprimir":
                    if (Page.Session["IdCotizacion"].ToString() != "0")
                    {
                        string Reporte, NombreReporte = string.Empty;
                        if (ddlTipoCredito.Text.Contains("Técnica"))
                        {
                            Reporte = "ListarDetalleCFT";
                            NombreReporte = "Simulador de Fianza Técnica Multiaval";
                        }
                        else
                        {
                            Reporte = "ListarDetalleCFC";
                            NombreReporte = "Simulador Crédito Fianza Multiaval";
                        }

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        byte[] archivo = new Reportes{}.GenerarReporteCFC(int.Parse(Page.Session["IdCotizacion"].ToString()), Reporte);

                        if (archivo != null)
                        {
                            Page.Session["tipoDoc"] = "pdf";
                            Page.Session["binaryData"] = archivo;
                            Page.Session["Titulo"] = NombreReporte;
                            ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
                            e.Result = "fin";
                        }
                        else
                        {
                            e.Result = "error";
                        }
                    }
                    else
                    {
                        e.Result = "error";
                    }
                    break;

                case "volver":
                    Page.Session["RESUMEN"] = null;
                    Page.Session["GvResumenComision"] = null;
                    Page.Session["GvResumenOperacion"] = null;
                    ASPxWebControl.RedirectOnCallback("ListarComercial.aspx");
                    break;
            }
        }

        protected void cpnl_Callback(object sender, CallbackEventArgsBase e)
        {
            switch (e.Parameter)
            {
                case "ver":
                    CargarCotizacion(int.Parse(Page.Session["IdCotizacion"].ToString()));
                    break;
                //case "verRut":
                //    VerificarEmpresa();
                //    break;
            }
        }

        protected void GvResumenOperacion_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GvResumenOperacion.DataSource = Page.Session["GvResumenOperacion"];
                GvResumenOperacion.DataBind();
            }
            catch
            {
            }
        }

        protected void GvResumenComision_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GvResumenComision.DataSource = Page.Session["GvResumenComision"];
                GvResumenComision.DataBind();
            }
            catch
            {
            }
        }

        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (Page.Session["RESUMEN"] != null)
                objresumen = (Resumen)Page.Session["RESUMEN"];
        }

        private void warning(string mensaje)
        {
            dvWarning1.Style.Add("display", "block");
            lbWarning1.Text = mensaje;
        }

        private void error(string mensaje)
        {
            dvError.Style.Add("display", "block");
            lbError.Text = mensaje;
        }

        public List<TipoBien> CargarTipoBien()
        {
            List<TipoBien> TipoBien = new List<TipoBien>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/TipoBienesCotizador").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        TipoBien = JsonConvert.DeserializeObject<List<TipoBien>>(res.Result);
                    }
                }
                return TipoBien;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }

        }

        private void CargarEjecutivo(string cargo, string nombre)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/Ejecutivo").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<Ejecutivo> ejecutivo = JsonConvert.DeserializeObject<List<Ejecutivo>>(res.Result);

                        ddlEjecutivo.DataSource = ejecutivo;
                        ddlEjecutivo.ValueField = "Id";
                        ddlEjecutivo.TextField = "Nombre";
                        ddlEjecutivo.DataBind();

                        //cargo = "Sub-Gerente Comercial";
                        //nombre = "Karen Ruiz Nilo";

                        if (cargo == "Sub-Gerente Comercial")
                        {
                            ListEditItem TipoEjecutivo = ddlEjecutivo.Items.FindByText(nombre);
                            if (TipoEjecutivo != null)
                            {
                                ddlEjecutivo.Text = nombre;
                                ddlEjecutivo.ReadOnly = true;
                            }
                        }
                        else
                        {
                            ListEditItem TipoEjecutivo = ddlEjecutivo.Items.FindByText(nombre);
                            if (TipoEjecutivo != null)
                            {
                                ddlEjecutivo.Text = nombre;
                                ddlEjecutivo.ReadOnly = true;
                            }
                        }
                    }
                    else
                    {
                        //    Console.Write("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarTipoMoneda()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/TipoMoneda").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<TipoMoneda> TipoMoneda = JsonConvert.DeserializeObject<List<TipoMoneda>>(res.Result);

                        ddlMoneda.DataSource = TipoMoneda;
                        ddlMoneda.ValueField = "Id";
                        ddlMoneda.TextField = "Nombre";
                        ddlMoneda.DataBind();
                    }
                    else
                    {
                        //    Console.Write("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarTipoCredito()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/TipoCredito").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<TipoCredito> TipoCredito = JsonConvert.DeserializeObject<List<TipoCredito>>(res.Result);

                        ddlTipoCredito.DataSource = TipoCredito;
                        ddlTipoCredito.ValueField = "Id";
                        ddlTipoCredito.TextField = "tipoCredito";
                        ddlTipoCredito.DataBind();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarPlazo()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/TipoAmortizacion").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<TipoAmortizacion> TipoAmortizacion = JsonConvert.DeserializeObject<List<TipoAmortizacion>>(res.Result);

                        ddlPlazo.DataSource = TipoAmortizacion;
                        ddlPlazo.ValueField = "_Id";
                        ddlPlazo.TextField = "_TipoAmortizacion";
                        ddlPlazo.DataBind();
                    }
                    else
                    {
                        //    Console.Write("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarTipoAmortizacion()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/TipoAmortizacion").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<TipoAmortizacion> TipoAmortizacion = JsonConvert.DeserializeObject<List<TipoAmortizacion>>(res.Result);

                        ddlEstructuraPlazo.DataSource = TipoAmortizacion;
                        ddlEstructuraPlazo.ValueField = "_Id";
                        ddlEstructuraPlazo.TextField = "_TipoAmortizacion";
                        ddlEstructuraPlazo.DataBind();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarFondo()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/Fondos").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<Fondos> fondo = JsonConvert.DeserializeObject<List<Fondos>>(res.Result);

                        ddlFondo.ValueType = typeof(int);
                        ddlFondo.DataSource = fondo;
                        ddlFondo.ValueField = "Id";
                        ddlFondo.TextField = "Fondo";
                        ddlFondo.DataBind();
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarValorMoneda()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.GetAsync("api/ValorMonedas").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> res = response.Content.ReadAsStringAsync();
                        List<ValorMonedas> ValorMonedas = JsonConvert.DeserializeObject<List<ValorMonedas>>(res.Result);

                        if (ValorMonedas != null)
                        {
                            lbUf.Text = ValorMonedas[0].UF.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"));
                            txtUf.Text = ValorMonedas[0].UF.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"));
                            lblDolar.Text = ValorMonedas[0].USD.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"));
                            txtDolar.Text = ValorMonedas[0].USD.ToString("N2", CultureInfo.CreateSpecificCulture("es-ES"));
                        }
                        else
                        {
                            lbUf.Text = "26.900.85";
                            txtUf.Text = "26.900.85";
                            lblDolar.Text = "0";
                            txtDolar.Text = "0";

                            warning("Imposible obtener informacion de la uf");
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                lbUf.Text = "26655.85";
                txtUf.Text = "26655.85";
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                error("error al consumir servicio valor moneda");
            }
        }

        public List<SeguroAval> InsertnewItemAval(OrderedDictionary values, ref List<SeguroAval> SeguroAval)
        {
            SeguroAval obj = new SeguroAval();

            obj.IdAvalCotizacion = 0;
            obj.NumAvales = Convert.ToInt32(values[0].ToString());
            obj.ValorAsegurable = values[1].ToString().GetValorDouble();
            obj.IdCotizacion = Page.Session["IdCotizacion"] == null ? 0 : int.Parse(Page.Session["IdCotizacion"].ToString());

            if (SeguroAval == null) SeguroAval = new List<SeguroAval>();
            SeguroAval.Add(obj);

            return SeguroAval;
        }

        protected SeguroAval LoadUpdateValues(SeguroAval item, OrderedDictionary values)
        {
            item.NumAvales = Convert.ToInt32(values["NumAvales"]);
            item.ValorAsegurable = values["ValorAsegurable"].ToString().GetValorDoubleGv();

            return item;
        }

        protected SeguroGarantia LoadUpdateValuesSeguro(SeguroGarantia item, OrderedDictionary values)
        {
            item.IdTipoBien = (int)values["IdTipoBien"];
            item.ValorAsegurable = values["ValorAsegurable"].ToString().GetValorDoubleGv();

            return item;
        }

        public List<SeguroGarantia> InsertNewItem(OrderedDictionary values, ref List<SeguroGarantia> TipoBien)
        {
            SeguroGarantia obj = new SeguroGarantia();

            obj.IdSeguro = 0;
            obj.IdTipoBien = Convert.ToInt32(values[0].ToString());
            obj.ValorAsegurable = values[1].ToString().GetValorDoubleGv();
            obj.IdTemporal = TipoBien == null ? 0 : TipoBien.Count + 1;
            obj.IdCotizacion = Page.Session["IdCotizacion"] == null ? 0 : int.Parse(Page.Session["IdCotizacion"].ToString());

            if (TipoBien == null) TipoBien = new List<SeguroGarantia>();
            TipoBien.Add(obj);

            return TipoBien;
        }

        private void CargarCotizacion(int IdCotizacion)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataSet ds = new DataSet("dsResumen");
            try
            {
                ds = Ln.CargarCotizacion(IdCotizacion);

                if (ds.Tables.Count > 0)
                {
                    txtRutEmpresa.Text = ds.Tables[0].Rows[0]["RutEmpresa"].ToString();
                    txtRutEmpresa.ReadOnly = true;
                    txtDivEmpresa.Text = ds.Tables[0].Rows[0]["DivRut"].ToString();
                    txtDivEmpresa.ReadOnly = true;
                    txtEmpresa.Text = ds.Tables[0].Rows[0]["RazonEmpresa"].ToString();

                    CargarEjecutivo(cargo, ds.Tables[0].Rows[0]["Ejecutivo"].ToString());
                    //ListEditItem TipoEjecutivo = ddlEjecutivo.Items.FindByText(ds.Tables[0].Rows[0]["Ejecutivo"].ToString());
                    //if (TipoEjecutivo != null)
                    //    ddlEjecutivo.Text = ds.Tables[0].Rows[0]["Ejecutivo"].ToString();

                    txtMontoLiquido.Text = ds.Tables[0].Rows[0]["MontoLiquido"].ToString();
                    CargarTipoMoneda();
                    ListEditItem TipoMoneda = ddlMoneda.Items.FindByText(ds.Tables[0].Rows[0]["TipoMoneda"].ToString());
                    if (TipoMoneda != null)
                        ddlMoneda.Text = ds.Tables[0].Rows[0]["TipoMoneda"].ToString();

                    txtMontoBruto.Text = ds.Tables[0].Rows[0]["MontoBruto"].ToString();

                    CargarTipoCredito();
                    ListEditItem TipoCredito = ddlTipoCredito.Items.FindByText(ds.Tables[0].Rows[0]["TipoCredito"].ToString());
                    if (TipoCredito != null)
                    {
                        ddlTipoCredito.Text = ds.Tables[0].Rows[0]["TipoCredito"].ToString();
                        ddlTipoCredito.Value = ds.Tables[0].Rows[0]["IdTipoCredito"].ToString();
                    }

                    txtPlazo.Text = ds.Tables[0].Rows[0]["Plazo"].ToString();
                    txtHorizonte.Text = ds.Tables[0].Rows[0]["PlazoHorizonte"].ToString();

                    CargarPlazo();
                    ListEditItem TipoPeriodicidadTasa = ddlPlazo.Items.FindByText(ds.Tables[0].Rows[0]["PeriocidadTasa"].ToString());
                    if (TipoPeriodicidadTasa != null)
                        ddlPlazo.Text = ds.Tables[0].Rows[0]["PeriocidadTasa"].ToString();

                    CargarTipoAmortizacion();
                    ListEditItem TipoAmortizacion = ddlEstructuraPlazo.Items.FindByText(ds.Tables[0].Rows[0]["EstructuraPago"].ToString());
                    if (TipoAmortizacion != null)
                        ddlEstructuraPlazo.Text = ds.Tables[0].Rows[0]["EstructuraPago"].ToString();

                    txtTasaBanco.Text = ds.Tables[0].Rows[0]["TasaBanco"].ToString();

                    dtcFechaCurse.Text = ds.Tables[0].Rows[0]["FechaCurse"].ToString();
                    dtcFechaVencimientoOperacion.Text = ds.Tables[0].Rows[0]["FechaVencimiento"].ToString();
                    dtcFecPrimerVencimiento.Text = ds.Tables[0].Rows[0]["Fecha1Vencimiento"].ToString();

                    ListEditItem TipoComision = ddlComision.Items.FindByText(ds.Tables[0].Rows[0]["ComisionIncluida"].ToString());
                    if (TipoComision != null)
                        ddlComision.Text = ds.Tables[0].Rows[0]["ComisionIncluida"].ToString();

                    ListEditItem TipoCoberturaCertificado = ddlCobCertificado.Items.FindByText(ds.Tables[0].Rows[0]["CoberturaCF"].ToString());
                    if (TipoCoberturaCertificado != null)
                        ddlCobCertificado.Text = ds.Tables[0].Rows[0]["CoberturaCF"].ToString();

                    ListEditItem TipoFogape = ddlFogape.Items.FindByText(ds.Tables[0].Rows[0]["CoberturaFogape"].ToString());
                    if (TipoComision != null)
                        ddlFogape.Text = ds.Tables[0].Rows[0]["CoberturaFogape"].ToString();

                    ListEditItem CapitalizaInteres = ddlCapitalizaIntereses.Items.FindByText(ds.Tables[0].Rows[0]["CapitalizaInteres"].ToString());
                    if (CapitalizaInteres != null)
                        ddlCapitalizaIntereses.Text = ds.Tables[0].Rows[0]["CapitalizaInteres"].ToString();

                    txtComisionAnual.Text = ds.Tables[0].Rows[0]["ComisionAnual"].ToString();

                    CargarFondo();
                    ListEditItem lm = ddlFondo.Items.FindByText(ds.Tables[0].Rows[0]["Fondo"].ToString());
                    if (lm != null)
                        ddlFondo.Text = ds.Tables[0].Rows[0]["Fondo"].ToString();

                    txtComisionMultiAval.Text = ds.Tables[0].Rows[0]["ComisionMultiAval"].ToString();
                    txtCuotasGracia.Text = ds.Tables[0].Rows[0]["CuotasGracias"].ToString();

                    txtGastosLegales.Text = ds.Tables[0].Rows[0]["GastosLegalesMoneda"].ToString();
                    txtGastoLegal.Text = ds.Tables[0].Rows[0]["GastosLegales"].ToString();
                    txtGastosFogape.Text = ds.Tables[0].Rows[0]["GastosFogape"].ToString();

                    txtGastoNotario.Text = ds.Tables[0].Rows[0]["GastosNotariaMoneda"].ToString();
                    txtGastosNotario.Text = ds.Tables[0].Rows[0]["GastosNotaria"].ToString();

                    txtGastosFogape.Text = ds.Tables[0].Rows[0]["GastosFogape"].ToString();

                    txtImpToTimbre.Text = ds.Tables[0].Rows[0]["ImpuestoTimbre"].ToString();
                    txtSeguroDesgravamen.Text = ds.Tables[0].Rows[0]["SeguroDesgravamen"].ToString();
                    txtSeguroGarantia.Text = ds.Tables[0].Rows[0]["SeguroGarantia"].ToString(); ;
                    txtCargaFinanciera.Text = ds.Tables[0].Rows[0]["CargaFinanciera"].ToString();
                    txtDescPrepago.Text = ds.Tables[0].Rows[0]["DescuentoPrepago"].ToString();

                    //ASPxHiddenFieldEmpresa["value"] = ds.Tables[0].Rows[0]["IdEmpresa"].ToString();
                    Page.Session["IdEmpresa"] = ds.Tables[0].Rows[0]["IdEmpresa"].ToString();

                    GvResumenOperacion.DataSource = ds.Tables[1];
                    GvResumenOperacion.DataBind();
                    Page.Session["GvResumenOperacion"] = ds.Tables[1];

                    GvResumenComision.DataSource = ds.Tables[2];
                    GvResumenComision.DataBind();
                    Page.Session["GvResumenComision"] = ds.Tables[2];

                    Page.Session["SeguroAval"] = ListAval(ds.Tables[3]);
                    CargarAvalList((List<SeguroAval>)Page.Session["SeguroAval"]);

                    Page.Session["TipoBien"] = ListGarantias(ds.Tables[4]);
                    CargarGarantiasList((List<SeguroGarantia>)Page.Session["TipoBien"]);

                    CrearListaOperacion(int.Parse(ds.Tables[0].Rows[0]["IdEmpresa"].ToString()), int.Parse(ds.Tables[0].Rows[0]["TipoProducto"].ToString()), int.Parse(ds.Tables[0].Rows[0]["Plazo"].ToString()), ds.Tables[0].Rows[0]["MontoBruto"].ToString().GetValorDouble(), int.Parse(ds.Tables[0].Rows[0]["IdOperacion"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarAvalList(List<SeguroAval> list)
        {
            GvAvales.DataSource = list;
            GvAvales.DataBind();
        }

        private void CargarGarantiasList(List<SeguroGarantia> list)
        {
            GvGarantias.DataSource = list;
            GvGarantias.DataBind();
        }

        private List<SeguroGarantia> ListGarantias(DataTable dt)
        {
            var List = (from rw in dt.AsEnumerable()
                        select new SeguroGarantia()
                        {
                            IdSeguro = Convert.ToInt32(rw["IdSeguro"]),
                            IdTipoBien = Convert.ToInt32(rw["Id"]),
                            Nombre = Convert.ToString(rw["Nombre"]),
                            ValorAsegurable = Convert.ToDouble(rw["ValorAsegurable"]),
                            PeriodoAnual = Convert.ToDouble(rw["PeriodoAnual"]),
                            IdTemporal = Convert.ToInt32(rw["IdTemporal"]),
                            IdCotizacion = Convert.ToInt32(rw["IdCotizacion"])
                        }).ToList();

            return List;
        }


        private List<SeguroAval> ListAval(DataTable dt)
        {
            var List = (from rw in dt.AsEnumerable()
                        select new SeguroAval()
                        {
                            IdAvalCotizacion = Convert.ToInt32(rw["IdAvalCotizacion"]),
                            NumAvales = Convert.ToInt32(rw["NumAvales"]),
                            ValorAsegurable = Convert.ToDouble(rw["ValorAsegurable"]),
                            PeriodoAnual = Convert.ToDouble(rw["PeriodoAnual"]),
                            IdCotizacion = Convert.ToInt32(rw["IdCotizacion"])
                        }).ToList();

            return List;
        }


        public void CrearListaOperacion(int IdEmpresa, int IdProducto, int Plazo, double MontoOperacion, int IdOperacion)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPListItemCollection items = app.Lists["Operaciones"].Items;
                SPListItem newItem = items.Add();

                Dictionary<string, object> valores = new Dictionary<string, object>();
                valores.Add("IdEmpresa", IdEmpresa);
                valores.Add("IdProducto", IdProducto);
                valores.Add("IdFinalidad", 0);
                valores.Add("IdAcreedor", 0);
                valores.Add("IdEtapa", 1);
                valores.Add("IdSubEtapa", 2);
                valores.Add("MontoOperacion", MontoOperacion);
                valores.Add("Plazo", Plazo);
                valores.Add("IdEstado", 1);
                valores.Add("IdSQL", IdOperacion);

                //validar si existe operacion en lista operaciones.
                int existe = Ln.ExisteOperacionLista(IdOperacion);
                if (existe == 1)
                {
                    Ln.ActualizarListaOperacion(valores);
                }
                else
                {
                    Ln.InsertarListaOperacion(valores);
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        public Cotizaciones insertCotizacion(ref Cotizaciones datos)
        {
            asignacionResumen(ref objresumen);

            if (Page.Session["IdCotizacion"] == null)
                return null;

            datos.IdCotizacion = (Page.Session["IdCotizacion"] != null) ? Convert.ToInt32(Page.Session["IdCotizacion"].ToString()) : 0;
            if (Page.Session["IdEmpresa"].ToString() == "0")
                datos.IdEmpresa = objresumen.idEmpresa != 0 ? objresumen.idEmpresa : 0;
            else
                datos.IdEmpresa = Page.Session["IdEmpresa"] == null ? 0 : int.Parse(Page.Session["IdEmpresa"].ToString());
            datos.IdOperacion = objresumen.idOperacion != 0 ? objresumen.idOperacion : 0;
            datos.RazonEmpresa = txtEmpresa.Text.Trim().ToUpper();
            datos.Rut = txtRutEmpresa.Text.Trim() != "" ? txtRutEmpresa.Text.Trim().Replace(".", "") : "";
            datos.DivRut = txtDivEmpresa.Text.Trim();
            datos.IdProducto = ddlTipoCredito.Text.Contains("Fianza Técnica") ? 2 : 1;
            datos.DescProducto = ddlTipoCredito.Text.Contains("Fianza Técnica") ? "Certificado Fianza Técnica" : "Certificado Fianza Comercial";
            datos.IdEjecutivo = Convert.ToInt32(ddlEjecutivo.Value);
            datos.Ejecutivo = ddlEjecutivo.Text.Trim();
            datos.IdTipoCredito = Convert.ToInt32(ddlTipoCredito.Value);
            datos.TipoCredito = ddlTipoCredito.Text.Trim();
            datos.MontoLiquido = !string.IsNullOrEmpty(txtMontoLiquido.Text.Trim()) ? txtMontoLiquido.Text.Trim().GetValorDouble() : 0;
            datos.MontoBruto = 0;
            datos.IdTipoMoneda = Convert.ToInt32(ddlMoneda.Value);
            datos.TipoMoneda = ddlMoneda.Text.Trim();
            datos.Plazo = !string.IsNullOrEmpty(txtPlazo.Text.Trim()) ? Convert.ToInt32(txtPlazo.Text.Trim()) : 0;
            datos.TasaBanco = !string.IsNullOrEmpty(txtTasaBanco.Text.Trim()) ? txtTasaBanco.Text.Trim().GetValorDouble() : 0;
            datos.IdPeriocidadTasa = Convert.ToInt32(ddlPlazo.Value);
            datos.PeriocidadTasa = ddlPlazo.Text.Trim();
            datos.IdEstructuraPago = Convert.ToInt32(ddlEstructuraPlazo.Value);
            datos.EstructuraPago = ddlEstructuraPlazo.Text.Trim();
            datos.FechaCurse = (dtcFechaCurse.Value != null) ? (DateTime?)dtcFechaCurse.Value : null;
            datos.Fecha1Vencimiento = (dtcFecPrimerVencimiento.Value != null) ? (DateTime?)dtcFecPrimerVencimiento.Value : null;
            datos.FechaVencimiento = (dtcFechaVencimientoOperacion.Value != null) ? (DateTime?)dtcFechaVencimientoOperacion.Value : null;
            datos.PlazoHorizonte = !string.IsNullOrEmpty(txtHorizonte.Text.Trim()) ? Convert.ToInt32(txtHorizonte.Text.Trim()) : 0;
            datos.CoberturaCF = !string.IsNullOrEmpty(ddlCobCertificado.Text.Trim()) ? ddlCobCertificado.Text.Trim().GetValorInteger() : 0;
            datos.IdCoberturaCF = Convert.ToInt32(ddlCobCertificado.Value);
            datos.CuotasGracias = !string.IsNullOrEmpty(txtCuotasGracia.Text.Trim()) ? Convert.ToInt32(txtCuotasGracia.Text.Trim()) : 0;
            datos.CapitalizaInteres = ddlCapitalizaIntereses.Text == "Si" ? true : false;
            datos.ComisionIncluida = ddlComision.Text == "Si" ? true : false;
            datos.CoberturaFogape = ddlFogape.Text.Trim() != null ? ddlFogape.Text.Trim().Replace("%", "").GetValorDouble() : 0;
            datos.GtiaFOGAPE = ddlFogape.Text == "0" ? false : true;
            datos.ComisionAnual = !string.IsNullOrEmpty(txtComisionAnual.Text.Trim()) ? txtComisionAnual.Text.Trim().GetValorDouble() : 0;
            datos.IdFondo = Convert.ToInt32(ddlFondo.Value);
            datos.Fondo = ddlFondo.Text.Trim();
            datos.GastosFogape = !string.IsNullOrEmpty(txtGastosFogape.Text.Trim()) ? txtGastosFogape.Text.Trim().GetValorDouble() : 0;
            datos.GastosLegales = !string.IsNullOrEmpty(txtGastoLegal.Text.Trim()) ? txtGastoLegal.Text.Trim().GetValorDouble() : 0;
            datos.GastosNotaria = !string.IsNullOrEmpty(txtGastosNotario.Text.Trim()) ? txtGastosNotario.Text.Trim().GetValorDouble() : 0;
            if (txtGastosNotario.Text != "")
                datos.NotarioIncluido = txtGastosNotario.Text.Trim().GetValorDouble() > 0 ? true : false;
            else
                datos.NotarioIncluido = false;
            datos.UF = txtUf.Text.Trim() != "" ? txtUf.Text.Trim().GetValorDouble() : 0;
            datos.USD = txtDolar.Text.Trim() != "" ? txtDolar.Text.Trim().GetValorDouble() : 0;

            ValidarPermisos validar = ((ValidarPermisos)Page.Session["validar"]);
            datos.UsuarioCreacion = validar != null ? validar.idUsuario : 0;
            datos.SeguroGarantias = (List<SeguroGarantia>)Page.Session["TipoBien"];
            datos.SeguroAvales = (List<SeguroAval>)Page.Session["SeguroAval"];
            datos.TipoCotizacion = "Cotizacion";

            return datos;
        }

        public string EnviarCotizacion(Cotizaciones cotizacion)
        {
            string result = "";
            string url = ConfigurationManager.AppSettings["Url"].ToString().Trim() + "api/Cotizaciones";
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                var data = JsonConvert.SerializeObject(cotizacion, settings);

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Encoding = ASCIIEncoding.UTF8;
                    result = wc.UploadString(url, "POST", data);

                    var Id = JsonConvert.DeserializeObject(System.Web.HttpUtility.HtmlDecode(result));

                    if (!Id.ToString().IsNumeric())
                        return "error al cargar cotizacion";
                    else
                    {
                        Page.Session["IdCotizacion"] = Id.ToString();
                        return Page.Session["IdCotizacion"].ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                return "error";
            }
        }

        //private void CargarGarantias(int IdCotizacion)
        //{
        //    JsonSerializerSettings settings = new JsonSerializerSettings();
        //    var data = JsonConvert.SerializeObject(IdCotizacion, settings);
        //    List<SeguroGarantia> seg = new List<SeguroGarantia>();

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
        //        var response = client.GetAsync("api/SegurosCotizador/" + IdCotizacion + "").Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Task<string> res = response.Content.ReadAsStringAsync();
        //            seg = JsonConvert.DeserializeObject<List<SeguroGarantia>>(res.Result);

        //            GvGarantias.DataSource = seg;
        //            GvGarantias.DataBind();
        //        }
        //    }
        //}

        private void CargarAvales(int IdCotizacion)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            var data = JsonConvert.SerializeObject(IdCotizacion, settings);
            List<SeguroAval> seg = new List<SeguroAval>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                var response = client.GetAsync("api/Aval/" + IdCotizacion + "").Result;
                if (response.IsSuccessStatusCode)
                {
                    Task<string> res = response.Content.ReadAsStringAsync();
                    seg = JsonConvert.DeserializeObject<List<SeguroAval>>(res.Result);
                }

                GvAvales.DataSource = seg;
                GvAvales.DataBind();
            }
        }

        public string ActualizarCotizacion(Cotizaciones cotizacion)
        {
            try
            {
                string result = "";
                string url = ConfigurationManager.AppSettings["Url"].ToString().Trim() + "api/Cotizaciones";

                JsonSerializerSettings settings = new JsonSerializerSettings();
                var data = JsonConvert.SerializeObject(cotizacion, settings);

                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Encoding = ASCIIEncoding.UTF8;
                    result = wc.UploadString(url, "PUT", data);

                    Page.Session["IdCotizacion"] = JsonConvert.DeserializeObject(System.Web.HttpUtility.HtmlDecode(result));

                    var aa = Page.Session["IdCotizacion"];

                    if (!Page.Session["IdCotizacion"].ToString().IsNumeric())
                        return "error al cargar cotizacion";
                    else
                        return Page.Session["IdCotizacion"].ToString();
                }

            }
            catch
            {
                return "error";
            }
        }

        private string VerificarEmpresa()
        {
            List<Empresa> emp = new List<Empresa>();
            emp = GetEmpresaPorRut();
            string retorno = string.Empty;

            if (emp != null && emp.Count > 0)
            {
                if (!emp[0].Bloqueado.Value)
                {
                    txtRutEmpresa.Text = emp[0].Rut.HasValue ? emp[0].Rut.ToString().Trim() : "";
                    txtDivEmpresa.Text = emp[0].DivRut != "" ? emp[0].DivRut.Trim() : "";

                    txtEmpresa.Text = emp[0].Cliente != "" ? emp[0].Cliente.Trim() : "";

                    Page.Session["IdEmpresa"] = emp[0].IdEmpresa != 0 ? emp[0].IdEmpresa : 0;
                    if (!string.IsNullOrEmpty(emp[0].IdEjecutivo.ToString()))
                    {
                        CargarEjecutivo(cargo, nombre);
                        ListEditItem TipoEjecutivo = ddlEjecutivo.Items.FindByValue(emp[0].IdEjecutivo.ToString().Trim());
                        if (TipoEjecutivo != null)
                        {
                            ddlEjecutivo.SelectedIndex = emp[0].IdEjecutivo == null ? -1 : ddlEjecutivo.Items.IndexOf(ddlEjecutivo.Items.FindByValue(emp[0].IdEjecutivo.ToString().Trim()));
                        }
                    }
                    else
                    {
                        CargarEjecutivo(cargo, nombre);
                    }

                    retorno = string.Format("{0}{1}{2}{1}{3}", "ok", ";", ddlEjecutivo.SelectedIndex, emp[0].Cliente);
                }
                else
                {
                    retorno = emp[0].descMotivoBloqueo;
                }
            }
            else
            {
                txtEmpresa.Text = "";
                retorno = string.Format("{0}", "ok");
            }

            return retorno;
        }

        public List<Empresa> GetEmpresaPorRut()
        {
            List<Empresa> emp = new List<Empresa>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                var response = client.GetAsync("api/EmpresasByRut?rut=" + (txtRutEmpresa.Text != "" ? int.Parse(txtRutEmpresa.Text.Trim().Replace(".", "")) : 0) + "").Result;
                if (response.IsSuccessStatusCode)
                {
                    Task<string> res = response.Content.ReadAsStringAsync();
                    emp = JsonConvert.DeserializeObject<List<Empresa>>(res.Result);
                }
                else
                {
                    emp = null;
                }
            }

            return emp;

        }

        private void BorrarGarantia(string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.DeleteAsync("api/SegurosCotizador/" + id + "").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //    Console.Write("Success");
                    }
                    else
                    {
                        //    Console.Write("Error");
                    }
                }
            }
            catch
            {
            }
        }

        private void BorrarAval(string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["Url"].ToString().Trim());
                    var response = client.DeleteAsync(string.Format("{0}{1}", "api/Aval/", id)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //    Console.Write("Success");
                    }
                    else
                    {
                        //    Console.Write("Error");
                    }
                }
            }
            catch
            {
            }
        }

        #endregion


        #region "clases"

        public class Empresa
        {
            public string Cliente { get; set; }
            public Nullable<int> Rut { get; set; }
            public string DivRut { get; set; }
            public int IdEmpresa { get; set; }
            public Nullable<int> IdEjecutivo { get; set; }
            public string Ejecutivo { get; set; }
            public Nullable<bool> Bloqueado { get; set; }
            public string descMotivoBloqueo { get; set; }
        }

        [DataContract(Name = "Ejecutivo")]
        public class Ejecutivo
        {
            [DataMember(Name = "Area")]
            public string Area { get; set; }

            [DataMember(Name = "Nombre")]
            public string Nombre { get; set; }

            [DataMember(Name = "Id")]
            public string Id { get; set; }
        }

        [DataContract(Name = "TipoMoneda")]
        public class TipoMoneda
        {
            [DataMember(Name = "TipoMoneda")]
            public string Nombre { get; set; }

            [DataMember(Name = "Id")]
            public string Id { get; set; }
        }

        [DataContract(Name = "TipoCredito")]
        public class TipoCredito
        {
            [DataMember(Name = "TipoCredito")]
            public string tipoCredito { get; set; }

            [DataMember(Name = "Id")]
            public string Id { get; set; }
        }

        [DataContract(Name = "TipoAmortizacion")]
        public class TipoAmortizacion
        {
            [DataMember(Name = "Producto")]
            public string _TipoAmortizacion { get; set; }

            [DataMember(Name = "Id")]
            public string _Id { get; set; }
        }


        [DataContract(Name = "Fondos")]
        public class Fondos
        {
            [DataMember(Name = "AfianzamientoApalancado")]
            public string AfianzamientoApalancado { get; set; }

            [DataMember(Name = "Fondo")]
            public string Fondo { get; set; }

            [DataMember(Name = "Id")]
            public string Id { get; set; }

            [DataMember(Name = "MaximoAfianzamiento")]
            public string MaximoAfianzamiento { get; set; }
        }

        [DataContract(Name = "ValorMonedas")]
        public class ValorMonedas
        {
            [DataMember(Name = "EUR")]
            public string EUR { get; set; }

            [DataMember(Name = "Fecha")]
            public string Fecha { get; set; }

            [DataMember(Name = "UF")]
            public float UF { get; set; }

            [DataMember(Name = "USD")]
            public float USD { get; set; }
        }

        #endregion

        protected void cbVerificarRut_Callback(object source, CallbackEventArgs e)
        {
            e.Result = VerificarEmpresa();
        }

        protected void GvGarantias_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var dt = Page.Session["TipoBien"];
            List<SeguroGarantia> SeguroGarantia = (List<SeguroGarantia>)Page.Session["TipoBien"];
            InsertNewItem(e.NewValues, ref SeguroGarantia);
            Page.Session["TipoBien"] = SeguroGarantia;

            GvGarantias.CancelEdit();
            e.Cancel = true;

            CargarGarantiasList((List<SeguroGarantia>)Page.Session["TipoBien"]);
        }

        protected void GvGarantias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                List<SeguroGarantia> SeguroGarantia = (List<SeguroGarantia>)Page.Session["TipoBien"];

                int IdSeguro = (int)e.Keys[0];
                int IdTemporal = (int)e.Keys[1];

                var item = SeguroGarantia.First(i => i.IdSeguro == IdSeguro);
                LoadUpdateValuesSeguro(item, e.NewValues);

                Page.Session["TipoBien"] = SeguroGarantia;

                GvGarantias.CancelEdit();
                e.Cancel = true;

                CargarGarantiasList((List<SeguroGarantia>)Page.Session["TipoBien"]);
            }
            catch (Exception)
            { }
        }

        protected void GvGarantias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                string IdSeguro = e.Keys[0].ToString();
                int IdTemporal = (int)e.Keys[1];

                if (IdSeguro != "0")
                {
                    BorrarGarantia(IdSeguro);
                }
                List<SeguroGarantia> SeguroGarantia = (List<SeguroGarantia>)Page.Session["TipoBien"];
                var itemToRemove = SeguroGarantia.Single(r => r.IdSeguro == int.Parse(IdSeguro));
                if (SeguroGarantia.Remove(itemToRemove))
                {
                    Page.Session["TipoBien"] = SeguroGarantia;
                    CargarGarantiasList((List<SeguroGarantia>)Page.Session["TipoBien"]);
                }

                e.Cancel = true;
            }
            catch (Exception)
            {

            }
        }

        protected void GvGarantias_DataBound(object sender, EventArgs e)
        {
            if (Page.Session["IdCotizacion"].ToString() == "0")
                GvGarantias.Columns["PeriodoAnual"].Visible = false;
            else
                GvGarantias.Columns["PeriodoAnual"].Visible = true;
        }

        protected void GvAvales_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            List<SeguroAval> SeguroAval = (List<SeguroAval>)Page.Session["SeguroAval"];
            InsertnewItemAval(e.NewValues, ref SeguroAval);
            Page.Session["SeguroAval"] = SeguroAval;

            GvAvales.CancelEdit();
            e.Cancel = true;

            CargarAvalList((List<SeguroAval>)Page.Session["SeguroAval"]);
        }

        protected void GvAvales_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            List<SeguroAval> SeguroAval = (List<SeguroAval>)Page.Session["SeguroAval"]; ;

            int IdAval = (int)e.Keys[0];

            var item = SeguroAval.First(i => i.IdAvalCotizacion == IdAval);
            LoadUpdateValues(item, e.NewValues);
            Page.Session["SeguroAval"] = SeguroAval;

            GvAvales.CancelEdit();
            e.Cancel = true;

            CargarAvalList((List<SeguroAval>)Page.Session["SeguroAval"]);
        }

        protected void GvAvales_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                int IdAval = int.Parse(e.Keys[0].ToString());

                if (IdAval != 0)
                {
                    BorrarAval(IdAval.ToString());
                }

                List<SeguroAval> SeguroAval = (List<SeguroAval>)Page.Session["SeguroAval"];
                var itemToRemove = SeguroAval.Single(r => r.IdAvalCotizacion == IdAval);
                if (SeguroAval.Remove(itemToRemove))
                {
                    Page.Session["SeguroAval"] = SeguroAval;
                    CargarAvalList((List<SeguroAval>)Page.Session["SeguroAval"]);
                }

                e.Cancel = true;
            }
            catch (Exception)
            {

            }
        }

        protected void GvAvales_DataBound(object sender, EventArgs e)
        {
            if (Page.Session["IdCotizacion"].ToString() == "0")
                GvAvales.Columns["PeriodoAnual"].Visible = false;
            else
                GvAvales.Columns["PeriodoAnual"].Visible = true;
        }

        //protected void GvGarantias_DataBinding(object sender, EventArgs e)
        //{
        //    GvGarantias.ForceDataRowType(typeof(DataRow));
        //}




        //protected void GvGarantias_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        //{
        //    int index = e.VisibleIndex;
        //    string KeyId = Convert.ToString(GvGarantias.GetRowValues(index, "IdSeguro"));
        //    if (e.ButtonID == "DeleteBtn")
        //    {
        //        var aa = Page.Session["IdCotizacion"];
        //        BorrarGarantia(KeyId);
        //        CargarGarantias(int.Parse(Page.Session["IdCotizacion"].ToString()));
        //    }
        //}

        //protected void GvAvales_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        //{
        //    LogicaNegocio Ln = new LogicaNegocio();
        //    List<SeguroAval> SeguroAval = null;
        //    try
        //    {
        //        foreach (var args in e.InsertValues)
        //            SeguroAval = InsertnewItemAval(args.NewValues, ref SeguroAval);

        //        foreach (var args in e.UpdateValues)
        //            SeguroAval = UpdateItem(args.NewValues, args.Keys);

        //        Page.Session["SeguroAval"] = SeguroAval;
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.Session["SeguroAval"] = SeguroAval;
        //        Ln.grdCustomErrorText(GvGarantias, ex);
        //    }
        //    e.Handled = true;
        //}

        //protected void GvGarantias_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        //{
        //    LogicaNegocio Ln = new LogicaNegocio();
        //    List<SeguroGarantia> TipoBien = null;
        //    try
        //    {
        //        foreach (var args in e.InsertValues)
        //            TipoBien = InsertNewItem(args.NewValues, ref TipoBien);

        //        foreach (var args in e.UpdateValues)
        //            TipoBien = UpdateItemSeguro(args.NewValues, args.Keys);

        //        Page.Session["TipoBien"] = TipoBien;
        //        //Enviar(TipoBien)
        //    }
        //    catch (Exception ex)
        //    {
        //        Page.Session["TipoBien"] = TipoBien;
        //        Ln.grdCustomErrorText(GvGarantias, ex);
        //    }

        //    e.Handled = true;
        //}

        //protected void GvAvales_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        //{
        //    int index = e.VisibleIndex;
        //    string KeyId = Convert.ToString(GvAvales.GetRowValues(index, "IdAvalCotizacion"));
        //    if (e.ButtonID == "DeleteBtnAvales")
        //    {
        //        var aa = Page.Session["IdCotizacion"];
        //        BorrarAval(KeyId);
        //        CargarAvales(int.Parse(Page.Session["IdCotizacion"].ToString()));
        //    }
        //}


        //protected void CancelEditing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    //GvAvales.CancelEdit();
        //}


        //protected List<SeguroAval> UpdateItem(OrderedDictionary newValues, OrderedDictionary keys)
        //{
        //    List<SeguroAval> oADValuation = new List<SeguroAval>();
        //    List<DataRow> rows = ((DataTable)util.GenerarDataTable(GvAvales)).AsEnumerable().ToList();
        //    int con = 0;
        //    foreach (DataRow fila in rows)
        //    {
        //        SeguroAval obj = new SeguroAval();

        //        obj.IdAvalCotizacion = int.Parse(GvAvales.GetRowValues(con, GvAvales.KeyFieldName).ToString());

        //        if (!string.IsNullOrEmpty(fila["NumAvales"].ToString()))
        //            obj.NumAvales = int.Parse(fila["NumAvales"].ToString());
        //        if (!string.IsNullOrEmpty(fila["ValorAsegurable"].ToString()))
        //            obj.ValorAsegurable = fila["ValorAsegurable"].ToString().GetValorDoubleGv();
        //        if (!string.IsNullOrEmpty(fila["PeriodoAnual"].ToString()))
        //            obj.PeriodoAnual = fila["PeriodoAnual"].ToString().GetValorDouble();

        //        oADValuation.Add(obj);
        //        con++;
        //    }

        //    GvAvales.DataSource = oADValuation;
        //    GvAvales.DataBind();

        //    var id = Convert.ToInt32(keys["IdAvalCotizacion"]);
        //    var item = oADValuation.First(i => i.IdAvalCotizacion == id);
        //    LoadUpdateValues(item, newValues);
        //    return oADValuation;
        //}

        //protected List<SeguroGarantia> UpdateItemSeguro(OrderedDictionary newValues, OrderedDictionary keys)
        //{
        //    List<SeguroGarantia> oADValuation = new List<SeguroGarantia>();
        //    List<DataRow> rows = ((DataTable)util.GenerarDataTable(GvGarantias)).AsEnumerable().ToList();
        //    int con = 0;
        //    foreach (DataRow fila in rows)
        //    {
        //        SeguroGarantia obj = new SeguroGarantia();

        //        obj.IdSeguro = int.Parse(GvGarantias.GetRowValues(con, GvGarantias.KeyFieldName).ToString());
        //        if (!string.IsNullOrEmpty(fila["Id"].ToString()))
        //            obj.Id = Convert.ToInt32(fila["Id"].ToString());
        //        if (!string.IsNullOrEmpty(fila["Nombre"].ToString()))
        //            obj.Nombre = fila["Nombre"].ToString();
        //        if (!string.IsNullOrEmpty(fila["ValorAsegurable"].ToString()))
        //            obj.ValorAsegurable = fila["ValorAsegurable"].ToString().GetValorDoubleGv();

        //        oADValuation.Add(obj);
        //        con++;
        //    }

        //    GvGarantias.DataSource = oADValuation;
        //    GvGarantias.DataBind();

        //    var id = Convert.ToInt32(keys["IdSeguro"]);
        //    var item = oADValuation.First(i => i.IdSeguro == id);
        //    LoadUpdateValuesSeguro(item, newValues);
        //    return oADValuation;
        //}

    }
}
