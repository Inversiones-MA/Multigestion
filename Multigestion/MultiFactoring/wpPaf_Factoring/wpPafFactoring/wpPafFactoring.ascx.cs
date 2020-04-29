using DevExpress.Web;
using System;
using System.Data;
using System.Net;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
//using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Linq;
//using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using MultigestionUtilidades;

namespace MultiFactoring.wpPaf_Factoring.wpPaf_Factoring
{
    [ToolboxItemAttribute(false)]
    public partial class wpPafFactoring : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPafFactoring()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Init(object sender, EventArgs e)
        {

        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "wpPafFactoring.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-CL");

            ////PERMISOS USUARIOS
            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = objresumen.area;

            dt = permiso.ListarPerfil(validar);

            try
            {
                if (!Page.IsPostBack)
                {

                }
            }
            catch (Exception)
            {
            }
        }

        protected void gvEmpresas_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parametros = e.Parameters.Split(',');
            if (parametros[0] == "buscar")
            {
                BuscarEmpresas(parametros);
            }
        }

        private void BuscarEmpresas(string[] parametros)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;
            DataTable res = Ln.ListarEmpresas(1, parametros[1].Trim(), util.ObtenerValor(app2.CurrentUser.Name));
            try
            {
                Page.Session["gvEmpresas"] = res;
                gvEmpresas.DataSource = res;
                gvEmpresas.DataBind();
            }
            catch (Exception)
            {

            }
        }

        protected void cbPaf_Callback(object source, CallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                string[] rowValueItems = e.Parameter.Split(',');
                int IdEmpresa = Convert.ToInt32(rowValueItems[1]);
                CargarDatosVigentesEmpresa(IdEmpresa);
            }
        }

        private void CargarDatosVigentesEmpresa(int IdEmpresa)
        {
            try
            {
                //asignacionResumen(ref objresumen);
                DataSet res = new DataSet();
                DataTable dt = new DataTable("dt");
                LogicaNegocio Ln = new LogicaNegocio();
                IdEmpresa = 1885;

                dt = Ln.GestionValorMoneda(DateTime.Now, 0, null, null, null, 1);
                if (dt.Rows.Count > 0)
                {
                    lblUf.Text = string.Format("{0:n3}", dt.Rows[0]["MontoUF"], new System.Globalization.CultureInfo("es-CL")); //util.actualizaMiles();
                    lblDolar.Text = string.Format("{0:n3}", dt.Rows[0]["MontoUSD"], new System.Globalization.CultureInfo("es-CL")); //util.actualizaMiles(dt.Rows[0]["MontoUSD"]);
                }
                else
                {
                    lblUf.Text = "ERROR AL OBTENER EL VALOR DE LA UF";
                    lblDolar.Text = "ERROR AL OBTENER EL VALOR DEL DOLAR";
                }

                res = Ln.ConsutarDatosEmpresaPAF(IdEmpresa, "0", "", "");

                if (res.Tables.Count > 0)
                {
                    lblNombre.Text = res.Tables[0].Rows[0]["RazonSocial"].ToString();
                    lblRut.Text = res.Tables[0].Rows[0]["Rut"].ToString() + "-" + res.Tables[0].Rows[0]["DivRut"].ToString();
                    lblDireccion.Text = res.Tables[0].Rows[0]["direccion"].ToString();
                    lblRegion.Text = res.Tables[0].Rows[0]["DescRegion"].ToString();
                    lblComuna.Text = res.Tables[0].Rows[0]["DescComuna"].ToString();
                    lblCiudad.Text = res.Tables[0].Rows[0]["DescProvincia"].ToString();
                    lblGiro.Text = res.Tables[0].Rows[0]["GIROACT"].ToString();
                    string aux = res.Tables[0].Rows[0]["GIROACT"].ToString();
                    if (!string.IsNullOrEmpty(aux))
                        lblGiro.Text = aux.Substring(2, aux.Length - 2);
                    else
                        lblGiro.Text = res.Tables[0].Rows[0]["GIROACT"].ToString();

                    lblTelefono.Text = res.Tables[0].Rows[0]["TelFijo1"].ToString();
                    lblEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();

                    Page.Session["gvGarantias"] = res.Tables[1];

                    DataTable dt_copy = new DataTable();
                    dt_copy.TableName = "CopyTable";
                    dt_copy = res.Tables[2].Copy();

                    foreach (DataRow row in dt_copy.Rows)
                    {
                        row["Monto Aprobado"] = row["Monto Aprobado"].ToString().GetValorDouble();
                        row["Monto Vigente"] = row["Monto Vigente"].ToString().GetValorDouble();
                        row["Monto Propuesto"] = row["Monto Propuesto"].ToString().GetValorDouble();
                    }

                    Page.Session["gvOperacionesVigentes"] = dt_copy;
                    gvOperacionesVigentes.DataSource = (DataTable)Page.Session["gvOperacionesVigentes"];
                    gvOperacionesVigentes.DataBind();
                }
            }
            catch (Exception)
            {

            }
        }

        #region "gvOperaciones Vigentes"

        protected string GetOpeVigentesTotalCLPMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = gvOperacionesVigentes.GetTotalSummaryValue(summaryItem).ToString().GetValorDouble();
                total = string.Format("{0:n0}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalUSDMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalUFMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalRiesgoUFMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalCLPMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = gvOperacionesVigentes.GetTotalSummaryValue(summaryItem).ToString().GetValorDouble();
                total = string.Format("{0:n0}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalUSDMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalUFMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalRiesgoUFMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalCLPMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem));
                total = string.Format("{0:n0}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalUSDMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalUFMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeVigentesTotalRiesgoUFMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesVigentes.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesVigentes.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected void gvOperacionesVigentes_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Monto Aprobado" || e.Column.FieldName == "Monto Vigente" || e.Column.FieldName == "Monto Propuesto")
                {
                    if (e.Value != null)
                    {
                        double valor = e.Value.ToString().GetValorDouble();
                        e.Value = string.Format("{0:n3}", valor, CultureInfo.InvariantCulture); //valor.ToString("N0");
                        e.DisplayText = string.Format("{0:n3}", valor, CultureInfo.InvariantCulture);  //valor.ToString("N0");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region "gv Operaciones Nuevas"

        private void CargarOperacionesFactoring(int IdEmpresa)
        {
            try
            {
                DataTable dt = new DataTable("dt");
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                //IdEmpresa = "1885";
                dt = Ln.FactoringOperacion(0, IdEmpresa, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, util.ObtenerValor(app2.CurrentUser.Name), 3);

                //if (ds.Tables.Count > 0)
                //{
                gvOperacionesNuevas.DataSource = dt;
                gvOperacionesNuevas.DataBind();
                //}
            }
            catch (Exception)
            {

            }
        }

        protected void gvOperacionesNuevas_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                string[] rowValueItems = e.Parameters.Split(',');

                int IdEmpresa = Convert.ToInt32(rowValueItems[1]);
                //string rut = rowValueItems[1];
                //string certificado = rowValueItems[2];
                //string ejecutivo = rowValueItems[3];

                CargarOperacionesFactoring(IdEmpresa);
            }
        }

        protected void gvOperacionesNuevas_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            int idEmpresa = 0;
            try
            {
                SPWeb app2 = SPContext.Current.Web;
                //bool modificar = false;
                LogicaNegocio LN = new LogicaNegocio();

                idEmpresa = Convert.ToInt32(HfEmp["value"]);
                int IdOperacion = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                LN.FactoringOperacion(IdOperacion, 0, Convert.ToInt32(e.NewValues["IdTipoFinanciamiento"]), Convert.ToInt32(e.NewValues["IdProducto"]),
                    Convert.ToInt32(e.NewValues["IdFinalidad"]), Convert.ToInt32(e.NewValues["Plazo"]), util.GetValorDouble(e.NewValues["AnticipoMax"].ToString())
                    , e.NewValues["PlazoMax"].ToString().GetValorDoubleGv(), e.NewValues["ConcentracionMax"].ToString().GetValorDoubleGv(), e.NewValues["TasaComision"].ToString().GetValorDoubleGv()
                    , Convert.ToInt32(e.NewValues["IdTipoMoneda"]), e.NewValues["MontoAprobado"].ToString().GetValorDoubleGv(), e.NewValues["MontoVigente"].ToString().GetValorDoubleGv()
                , e.NewValues["MontoPropuesto"].ToString().GetValorDoubleGv(), 1, util.ObtenerValor(app2.CurrentUser.Name), 2);
            }
            catch (Exception)
            {
                //LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvOperacionesNuevas.CancelEdit();
            e.Cancel = true;

            CargarOperacionesFactoring(idEmpresa);
        }

        protected void gvOperacionesNuevas_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            int idEmpresa = 0;
            try
            {
                SPWeb app2 = SPContext.Current.Web;
                //bool insertar = false;
                LogicaNegocio LN = new LogicaNegocio();

                idEmpresa = Convert.ToInt32(HfEmp["value"]);

                //LN.FactoringOperacion(0, int.Parse(idEmpresa.ToString()), Convert.ToInt32(e.NewValues["IdTipoFinanciamiento"]), Convert.ToInt32(e.NewValues["IdProducto"]),
                //    Convert.ToInt32(e.NewValues["IdFinalidad"]), Convert.ToInt32(e.NewValues["Plazo"]), util.GetValorDouble(e.NewValues["AnticipoMax"].ToString())
                //    , util.GetValorDouble(e.NewValues["PlazoMax"].ToString()), util.GetValorDouble(e.NewValues["ConcentracionMax"].ToString()), util.GetValorDouble(e.NewValues["TasaComision"].ToString())
                //    , Convert.ToInt32(e.NewValues["IdTipoMoneda"]), util.GetValorDouble(e.NewValues["MontoAprobado"].ToString()), util.GetValorDouble(e.NewValues["MontoVigente"].ToString())
                //, util.GetValorDouble(e.NewValues["MontoPropuesto"].ToString()), 1, util.ObtenerValor(app2.CurrentUser.Name), 1);

                LN.FactoringOperacion(0, int.Parse(idEmpresa.ToString()), Convert.ToInt32(e.NewValues["IdTipoFinanciamiento"]), Convert.ToInt32(e.NewValues["IdProducto"]),
                    Convert.ToInt32(e.NewValues["IdFinalidad"]), Convert.ToInt32(e.NewValues["Plazo"]), util.GetValorDouble(e.NewValues["AnticipoMax"].ToString())
                    , e.NewValues["PlazoMax"].ToString().GetValorDoubleGv(), e.NewValues["ConcentracionMax"].ToString().GetValorDoubleGv(), e.NewValues["TasaComision"].ToString().GetValorDoubleGv()
                    , Convert.ToInt32(e.NewValues["IdTipoMoneda"]), e.NewValues["MontoAprobado"].ToString().GetValorDoubleGv(), e.NewValues["MontoVigente"].ToString().GetValorDoubleGv()
                , e.NewValues["MontoPropuesto"].ToString().GetValorDoubleGv(), 1, util.ObtenerValor(app2.CurrentUser.Name), 1);

            }
            catch (Exception)
            {
                //LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvOperacionesNuevas.CancelEdit();
            e.Cancel = true;

            CargarOperacionesFactoring(idEmpresa);
        }

        protected void gvOperacionesNuevas_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            //bool borrar = false;
            LogicaNegocio LN = new LogicaNegocio();
            int Id = Convert.ToInt32(e.Values[gvOperacionesNuevas.KeyFieldName].ToString());
            var idEmpresa = HfEmp["value"];
            SPWeb app2 = SPContext.Current.Web;

            if (idEmpresa != null)
            {
                util.ObtenerValor(app2.CurrentUser.Name);

                LN.FactoringOperacion(Id, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, util.ObtenerValor(app2.CurrentUser.Name), 4);

                CargarOperacionesFactoring(Convert.ToInt32(idEmpresa));
                //if (borrar)
                //{
                // CargarDetMov(certificado.ToString());
                // CargarDetalle(idEmpresa.ToString());
                //}
            }
            e.Cancel = true;
        }

        protected void gvOperacionesNuevas_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
        }

        protected string GetOpeNuevasTotalCLPMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = gvOperacionesNuevas.GetTotalSummaryValue(summaryItem).ToString().GetValorDouble();
                total = string.Format("{0:n0}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalCLPMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem));
                total = string.Format("{0:n0}", result, new System.Globalization.CultureInfo("es-CL"));
                //total = gvOperacionesNuevas.GetTotalSummaryValue(summaryItem).ToString();
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalCLPMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem));
                total = string.Format("{0:n0}", result, new System.Globalization.CultureInfo("es-CL"));
                //total = gvOperacionesNuevas.GetTotalSummaryValue(summaryItem).ToString();
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalUSDMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalUSDMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalUSDMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblDolar.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalUFMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalUFMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalUFMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalRiesgoUFMA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoAprobado");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalRiesgoUFMV(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoVigente");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetOpeNuevasTotalRiesgoUFMP(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvOperacionesNuevas.TotalSummary.First(i => i.Tag == "MontoPropuesto");
                var result = double.IsNaN(Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble()) ? 0 : (Convert.ToDouble(gvOperacionesNuevas.GetTotalSummaryValue(summaryItem)) / lblUf.Text.GetValorDouble());
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        #endregion

        #region "gv Garantias"

        protected string GetGarantiaRealVC(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvGarantias.TotalSummary.First(i => i.Tag == "ValorComercial");
                double result = double.IsNaN(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem))) ? 0 : (Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)));
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetGarantiaRealVA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvGarantias.TotalSummary.First(i => i.Tag == "ValorAjustado");
                double result = double.IsNaN(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem))) ? 0 : (Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)));
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetGarantiaCoberturaVigenteVC(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvGarantias.TotalSummary.First(i => i.Tag == "ValorComercial");
                var result = double.IsNaN(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem))) ? 0 : (Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)));
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetGarantiaCoberturaVigenteVA(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvGarantias.TotalSummary.First(i => i.Tag == "ValorAjustado");
                var result = double.IsNaN(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem))) ? 0 : (Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)));
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetGarantiaRealTotal(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                //gvOperacionesNuevas.FindFooterCellTemplateControl
                var montoMov = (gvOperacionesNuevas.FindFooterCellTemplateControl(gvOperacionesNuevas.Columns["MontoPropuesto"] as GridViewDataTextColumn, "lblOpeNuevasTotalRiesgoUFMP") as ASPxLabel).Text;
                var summaryItem = gvGarantias.TotalSummary.First(i => i.Tag == "ValorComercial");
                var result = double.IsNaN(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)) / Convert.ToDouble(montoMov)) || double.IsInfinity(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)) / Convert.ToDouble(montoMov)) ? 0 : (Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)) / Convert.ToDouble(montoMov));
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        protected string GetGarantiaAjustadoTotal(GridViewFooterCellTemplateContainer container)
        {
            string total = "0";
            try
            {
                var summaryItem = gvGarantias.TotalSummary.First(i => i.Tag == "ValorAjustado");
                var result = double.IsNaN(Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem))) ? 0 : (Convert.ToDouble(gvGarantias.GetTotalSummaryValue(summaryItem)));
                total = string.Format("{0:n3}", result, new System.Globalization.CultureInfo("es-CL"));
            }
            catch (Exception)
            {
                return "0";
            }
            return total;
        }

        #endregion

        protected void cpnlPaf_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                string[] rowValueItems = e.Parameter.Split(',');
                //string accion = rowValueItems[0];
                int IdEmpresa = Convert.ToInt32(rowValueItems[1]);
                switch (rowValueItems[0])
                {
                    case "CargarPaf":
                        CargarDatosVigentesEmpresa(IdEmpresa);
                        CargarOperacionesFactoring(IdEmpresa);
                        CargarTotalGlobalLinea();
                        CargarGarantias();
                        ConsultarDatosPaf(IdEmpresa);
                        CargarIndicadores();
                        break;
                    case "ResetPaf":
                        Limpiar();
                        break;
                }
            }
        }

        private void CargarIndicadores()
        {
            gvPrimero.DataSource = (DataTable)Page.Session["dtPrimero"];
            gvPrimero.DataBind();
            gvSegundo.DataSource = (DataTable)Page.Session["dtSegundo"];
            gvSegundo.DataBind();
            gvTercero.DataSource = (DataTable)Page.Session["dtTercero"];
            gvTercero.DataBind();
            gvCuarto.DataSource = (DataTable)Page.Session["dtCuarto"];
            gvCuarto.DataBind();
            gvQuinto.DataSource = (DataTable)Page.Session["dtQuinto"];
            gvQuinto.DataBind();
            gvSexto.DataSource = (DataTable)Page.Session["dtSexto"];
            gvSexto.DataBind();
        }

        private void ConsultarDatosPaf(int IdEmpresa)
        {
            DataSet ds = new DataSet("dt");
            LogicaNegocio Ln = new LogicaNegocio();
            IdEmpresa = 1885;
            ds = Ln.ConsutarDatosPAF(IdEmpresa.ToString(), "", "", "0");

            if (ds.Tables.Count > 0)
            {
                Page.Session["dtPrimero"] = ds.Tables[3];
                Page.Session["dtSegundo"] = ds.Tables[4];
                Page.Session["dtTercero"] = ds.Tables[5];

                Page.Session["dtCuarto"] = ds.Tables[6];
                Page.Session["dtQuinto"] = ds.Tables[7];
                Page.Session["dtSexto"] = ds.Tables[8];
            }
        }

        private void CargarTotalGlobalLinea()
        {
            try
            {
                DataTable dtGlobalLinea = new DataTable("dtGlobalLinea");
                dtGlobalLinea.Columns.Add("Moneda", typeof(string));
                dtGlobalLinea.Columns.Add("MontoAprobado", typeof(double));
                dtGlobalLinea.Columns.Add("MontoVigente", typeof(double));
                dtGlobalLinea.Columns.Add("MontoPropuesto", typeof(double));
                //dtGlobalLinea.Columns.Add("fechaCreacion", typeof(DateTime));
                //DataRow[] result = null;

                var monto = (gvOperacionesNuevas.FindFooterCellTemplateControl(gvOperacionesNuevas.Columns["MontoPropuesto"] as GridViewDataTextColumn, "lblOpeNuevasTotalRiesgoUFMP") as ASPxLabel).Text;
                dtGlobalLinea.Rows.Add("Total CLP", monto, monto, monto);
                dtGlobalLinea.Rows.Add("Total USD", monto, monto, monto);
                dtGlobalLinea.Rows.Add("Total UF", monto, monto, monto);
                dtGlobalLinea.Rows.Add("Total Riesgo Equivalente UF", monto, monto, monto);

                Page.Session["GlobalLinea"] = dtGlobalLinea;
                gvtotalLinea.DataSource = dtGlobalLinea;
                gvtotalLinea.DataBind();
            }
            catch (Exception)
            {

            }
        }

        private void CargarGarantias()
        {
            gvGarantias.DataSource = (DataTable)Page.Session["gvGarantias"];
            gvGarantias.DataBind();
        }

        protected void gvPrimero_Init(object sender, EventArgs e)
        {
            ASPxGridView gridPrimero = sender as ASPxGridView;
            CrearColumnas(gridPrimero);
        }

        protected void gvSegundo_Init(object sender, EventArgs e)
        {
            ASPxGridView gridSegundo = sender as ASPxGridView;
            CrearColumnas(gridSegundo);
        }

        protected void gvTercero_Init(object sender, EventArgs e)
        {
            ASPxGridView gridTercero = sender as ASPxGridView;
            CrearColumnas(gridTercero);
        }

        protected void gvCuarto_Init(object sender, EventArgs e)
        {
            ASPxGridView gridCuarto = sender as ASPxGridView;
            CrearColumnas2(gridCuarto);
        }

        protected void gvQuinto_Init(object sender, EventArgs e)
        {
            ASPxGridView gridQuinto = sender as ASPxGridView;
            CrearColumnas2(gridQuinto);
        }

        protected void gvSexto_Init(object sender, EventArgs e)
        {
            ASPxGridView gridSexto = sender as ASPxGridView;
            CrearColumnas2(gridSexto);
            //string[] columnas = { "Indicadores", DateTime.Now.Year.ToString(), (DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year - 2).ToString(), (DateTime.Now.Year - 3).ToString() };
            //CrearColumnas(columnas, gvSexto);
        }

        private void CrearColumnas(ASPxGridView gv)
        {
            try
            {
                GridViewDataColumn col1 = new GridViewDataColumn();
                col1.Name = "(en M$)";
                col1.Caption = "(en M$)";
                col1.FieldName = "Cuenta";
                col1.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col1.VisibleIndex = 1;
                gv.Columns.Add(col1);

                GridViewDataColumn col2 = new GridViewDataColumn();
                col2.Name = DateTime.Now.Year.ToString();
                col2.Caption = DateTime.Now.Year.ToString();
                col2.FieldName = "VAL1";
                col2.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col2.VisibleIndex = 2;
                gv.Columns.Add(col2);

                GridViewDataColumn col3 = new GridViewDataColumn();
                col3.Name = "%";
                col3.Caption = "%";
                col3.FieldName = "POR1";
                col3.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col3.VisibleIndex = 3;
                gv.Columns.Add(col3);

                GridViewDataColumn col4 = new GridViewDataColumn();
                col4.Name = (DateTime.Now.Year - 1).ToString();
                col4.Caption = (DateTime.Now.Year - 1).ToString();
                col4.FieldName = "VAL2";
                col4.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col4.VisibleIndex = 4;
                gv.Columns.Add(col4);

                GridViewDataColumn col5 = new GridViewDataColumn();
                col5.Name = "%";
                col5.Caption = "%";
                col5.FieldName = "POR2";
                col5.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col5.VisibleIndex = 5;
                gv.Columns.Add(col5);

                GridViewDataColumn col6 = new GridViewDataColumn();
                col6.Name = (DateTime.Now.Year - 2).ToString();
                col6.Caption = (DateTime.Now.Year - 2).ToString();
                col6.FieldName = "VAL3";
                col6.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col6.VisibleIndex = 6;
                gv.Columns.Add(col6);

                GridViewDataColumn col7 = new GridViewDataColumn();
                col7.Name = "%";
                col7.Caption = "%";
                col7.FieldName = "POR3";
                col7.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col7.VisibleIndex = 7;
                gv.Columns.Add(col7);

                GridViewDataColumn col8 = new GridViewDataColumn();
                col8.Name = (DateTime.Now.Year - 3).ToString();
                col8.Caption = (DateTime.Now.Year - 3).ToString();
                col8.FieldName = "VAL4";
                col8.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col8.VisibleIndex = 8;
                gv.Columns.Add(col8);

                GridViewDataColumn col9 = new GridViewDataColumn();
                col9.Name = "%";
                col9.Caption = "%";
                col9.FieldName = "POR4";
                col9.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col9.VisibleIndex = 9;
                gv.Columns.Add(col9);
            }
            catch (Exception)
            { }
        }

        private void CrearColumnas2(ASPxGridView gv)
        {
            try
            {
                GridViewDataColumn col1 = new GridViewDataColumn();
                col1.Name = "Indicadores";
                col1.Caption = "Indicadores";
                col1.FieldName = "Cuenta";
                col1.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col1.VisibleIndex = 1;
                gv.Columns.Add(col1);

                GridViewDataColumn col2 = new GridViewDataColumn();
                col2.Name = DateTime.Now.Year.ToString();
                col2.Caption = DateTime.Now.Year.ToString();
                col2.FieldName = "VAL1";
                col2.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col2.VisibleIndex = 2;
                gv.Columns.Add(col2);

                GridViewDataColumn col3 = new GridViewDataColumn();
                col3.Name = (DateTime.Now.Year - 1).ToString();
                col3.Caption = (DateTime.Now.Year - 1).ToString();
                col3.FieldName = "VAL2";
                col3.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col3.VisibleIndex = 3;
                gv.Columns.Add(col3);

                GridViewDataColumn col4 = new GridViewDataColumn();
                col4.Name = (DateTime.Now.Year - 2).ToString();
                col4.Caption = (DateTime.Now.Year - 2).ToString();
                col4.FieldName = "VAL3";
                col4.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col4.VisibleIndex = 4;
                gv.Columns.Add(col4);

                GridViewDataColumn col5 = new GridViewDataColumn();
                col5.Name = (DateTime.Now.Year - 3).ToString();
                col5.Caption = (DateTime.Now.Year - 3).ToString();
                col5.FieldName = "VAL4";
                col5.HeaderStyle.Font.Bold = true;
                ////enM.HeaderStyle.HorizontalAlign = 
                col5.VisibleIndex = 8;
                gv.Columns.Add(col5);
            }
            catch (Exception)
            { }
        }

        private void Limpiar()
        {
            Page.Session["gvEmpresas"] = null;
            Page.Session["gvGarantias"] = null;
            Page.Session["gvOperacionesVigentes"] = null;
            Page.Session["GlobalLinea"] = null;
            Page.Session["dtPrimero"] = null;
            Page.Session["dtSegundo"] = null;
            Page.Session["dtTercero"] = null;
            Page.Session["dtCuarto"] = null;
            Page.Session["dtQuinto"] = null;
            Page.Session["dtSexto"] = null;

            //ASPxEdit.ClearEditorsInContainer(cpnlPaf);
        }


        //private void CrearColumnas(string[] columnas, string[] NombreColumna, ASPxGridView gv)
        //{
        //    int contador = 1;
        //    foreach (var c in columnas)
        //    {
        //        GridViewDataColumn col = new GridViewDataColumn();
        //        col.Name = c;
        //        col.Caption = c;
        //        col.FieldName = NombreColumna[contador];
        //        ////enM.HeaderStyle.HorizontalAlign = 
        //        col.VisibleIndex = contador;
        //        contador++;

        //        gv.Columns.Add(col);
        //    }
        //} 
    }
}
