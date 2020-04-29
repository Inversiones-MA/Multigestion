using System;
using System.Data;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using DevExpress.Web;
using System.Collections.Generic;
using Bd;
using ClasesNegocio;

namespace MultiOperacion.wpAdministracionDeFondo.wpAdministracionDeFondo
{
    [ToolboxItemAttribute(false)]
    public partial class wpAdministracionDeFondo : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpAdministracionDeFondo()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "AdministracionFondos.aspx";


        #region Metodos

        private void CargarTotalEjecutivo()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("dt");

            dt = Ln.ListarAdministracionFondosEjecutivo();

            Page.Session["gvTotalEje"] = dt;
            gvTotalEje.DataSource = dt;
            gvTotalEje.DataBind();
        }

        private void CargarEjecutivo()
        {
            cbxEjecutivo.Items.Clear();
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarUsuarios(null, 1, "");
                util.CargaDDLDxx(cbxEjecutivo, dt, "nombreApellido", "idUsuario");
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarGrilla(string empresa, string rut, string certificado, string ejecutivo)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dtAdministracion");

                dt = Ln.ListarAdministracionFondos(empresa, rut, certificado, ejecutivo);
                Page.Session["gvAdministracionFondos"] = dt;
                gvAdministracionFondos.DataSource = dt;
                gvAdministracionFondos.DataBind();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private DataTable CargarDdlConcepto()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("dt");
            dt = Ln.ListarConceptoPago();
            return dt;
        }

        private void CargarDetalle(string IdEmpresa)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable("dt");

            dt = Ln.ListarDetalleAdministracionFondos(int.Parse(IdEmpresa));
            Page.Session["gvDetalle"] = dt;
            gvDetalle.DataSource = dt;
            gvDetalle.DataBind();
        }

        private void CargarDetMov(string Ncertificado)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataTable dt = new DataTable("dt");

                dt = Ln.ListarDetalleMovimiento(Ncertificado);
                Page.Session["gvDetalleMov"] = dt;
                gvDetalleMov.DataSource = dt;
                gvDetalleMov.DataBind();
            }
            catch (Exception)
            {

            }
        }

        private void Detalle(object sender, CallbackEventArgsBase e)
        {
            var IdDetalle = -1;
            string a = e.Parameter;
            Int32.TryParse(e.Parameter, out IdDetalle);
            Concepto(sender as ASPxComboBox, IdDetalle);
        }

        protected void Concepto(ASPxComboBox combo, int DescDetalle)
        {
            combo.DataSourceID = "dsConceptoPagoPor";
            dsConceptoPagoPor.SelectParameters["IdDetalle"].DefaultValue = DescDetalle.ToString();
            combo.DataBindItems();
            combo.Items.Insert(0, new ListEditItem("", null));
        }

        private void DescargarInstruccionCurse(string nCertificado)
        {
            LogicaNegocio Ln = new LogicaNegocio();

            string Reporte = "CartaInstruccion";
            string sp = "ReporteCartaInstrucciones";

            byte[] archivo = new Reportes{ }.CrearReporteCurse(Reporte, sp, nCertificado);
            if (archivo != null)
            {
                //ExcluirReporteCurse(nCertificado);
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
            }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = string.Empty,
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["BUSQUEDA"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;
                    }
                    CargarEjecutivo();
                    CargarTotalEjecutivo();
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void gvAdministracionFondos_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["gvAdministracionFondos"] != null)
            {
                gvAdministracionFondos.DataSource = (DataTable)Page.Session["gvAdministracionFondos"];
                gvAdministracionFondos.DataBind();
            }
        }

        protected void gvDetalle_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["gvDetalle"] != null)
            {
                gvDetalle.DataSource = (DataTable)Page.Session["gvDetalle"];
                gvDetalle.DataBind();
            }
        }

        protected void gvAdministracionFondos_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                string[] rowValueItems = e.Parameters.Split(',');

                string empresa = rowValueItems[0];
                string rut = rowValueItems[1];
                string certificado = rowValueItems[2];
                string ejecutivo = rowValueItems[3];

                CargarGrilla(empresa, rut, certificado, ejecutivo);
            }
        }

        protected void gvDetalle_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                string[] rowValueItems = e.Parameters.Split(',');

                if(rowValueItems[1] == "detalle")
                    CargarDetalle(rowValueItems[0]);
                //if (rowValueItems[1] == "descargar")
                //    DescargarInstruccionCurse(rowValueItems[0]);
            }      
        }

        protected void gvDetalle_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
            if (e.Column.FieldName == "RazonSocial")
            {
                int prevIndex = e.VisibleIndex - 1;
                if (prevIndex >= 0)
                {
                    object prevValue = gridView.GetRowValues(prevIndex, e.Column.FieldName);
                    e.DisplayText = (prevValue.Equals(e.Value)) ? String.Empty : e.DisplayText;
                }
            }
        }

        protected void gvDetalleMov_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["gvDetalleMov"] != null)
            {
                gvDetalleMov.DataSource = (DataTable)Page.Session["gvDetalleMov"];
                gvDetalleMov.DataBind();
            }
        }

        protected void gvDetalleMov_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            bool borrar = false;
            LogicaNegocio LN = new LogicaNegocio();
            int? Id = Convert.ToInt32(e.Values[gvDetalleMov.KeyFieldName].ToString());
            var certificado = HfKey["value"];
            var idEmpresa = HfEmp["value"];
            SPWeb app2 = SPContext.Current.Web;

            if (certificado != null && idEmpresa != null)
            {
                util.ObtenerValor(app2.CurrentUser.Name);

                borrar = LN.InsertarModificaFondos(Id, "", 0, 0, "", 0, "", 0, "", null, 0, util.ObtenerValor(app2.CurrentUser.Name), 3, "", null);

                if (borrar)
                {
                    CargarDetMov(certificado.ToString());
                    CargarDetalle(idEmpresa.ToString());
                }
            }
            e.Cancel = true;
        }

        protected void gvTotalEje_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["gvTotalEje"] != null)
            {
                gvTotalEje.DataSource = (DataTable)Page.Session["gvTotalEje"];
                gvTotalEje.DataBind();
            }
        }

        protected void gvDetalleMov_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                string[] rowValueItems = e.Parameters.Split(',');

                string key = rowValueItems[0];
                string accion = rowValueItems[1];

                if (accion == "detalle")
                {
                    gvDetalleMov.JSProperties["cpReporte"] = "detalle";
                    CargarDetMov(key);
                }
                if (accion == "incluirReporte")
                {
                    IncluirExcluirReporte(key, Convert.ToBoolean(rowValueItems[2]));
                }
                if (accion == "excluir")
                {
                    ExcluirReporteCurse(key);
                    gvDetalleMov.JSProperties["cpReporte"] = "reporte";
                }
            }  
        }


        //protected void ddlConceptoPago_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter == "cargarConcepto")
        //    {
        //        DataTable dt = CargarDdlConcepto();
        //        util.CargaDDLDx(ddlConceptoPago, dt, "ConceptoPago", "IdDestino");
        //    }
        //}

        protected void gvDetalleMov_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "IdDetalle")
            {
                var combo = (ASPxComboBox)e.Editor;
                combo.Callback += new CallbackEventHandlerBase(Detalle);

                var grid = e.Column.Grid;
                if (!combo.IsCallback)
                {
                    var DescDetalle = -1;
                    if (!grid.IsNewRowEditing)
                        DescDetalle = (int)grid.GetRowValues(e.VisibleIndex, "IdDestino");

                    Concepto(combo, DescDetalle);
                }
            }
        }

        protected void gvDetalleMov_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            var certificado = HfKey["value"];
            try
            {
                SPWeb app2 = SPContext.Current.Web;
                bool modificar = false;
                LogicaNegocio LN = new LogicaNegocio();

                int IdAdministracionFondo = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                var idTipoMov = e.NewValues["IdTipoMov"];
                var idConceptoPago = e.NewValues["IdDestino"];
                var idDetallePago = e.NewValues["IdDetalle"];
                var FechaMov = e.NewValues["FechaMovimiento"];
         
                var montoMov = (gvDetalleMov.FindEditRowCellTemplateControl(gvDetalleMov.Columns["MontoMovimiento"] as GridViewDataColumn, "txtMontoMovimiento") as ASPxTextBox).Text;
                var comentario = (gvDetalleMov.FindEditRowCellTemplateControl(gvDetalleMov.Columns["Comentario"] as GridViewDataColumn, "txtComentario") as ASPxMemo).Text;

                if (idTipoMov != null && idConceptoPago != null && idDetallePago != null)
                    modificar = LN.InsertarModificaFondos(IdAdministracionFondo, "", 0, Convert.ToInt32(idTipoMov.ToString()), "", Convert.ToInt32(idConceptoPago.ToString()), "", Convert.ToInt32(idDetallePago.ToString()), "", Convert.ToDateTime(FechaMov), montoMov.Trim().GetValorDouble(), util.ObtenerValor(app2.CurrentUser.Name), 2, comentario.Trim(), null);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvDetalleMov.CancelEdit();
            e.Cancel = true;

            CargarDetMov(certificado.ToString());
        }

        protected void gvDetalleMov_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            var monto = HfMonto["value"];
            ASPxTextBox montoMov = null;
            ASPxMemo comentario = null;

            foreach (GridViewColumn column in gvDetalleMov.Columns)
            {
                GridViewDataColumn dataColumn = column as GridViewDataColumn;
                if (dataColumn == null) continue;
                if (dataColumn.FieldName != "MontoMovimiento" && dataColumn.FieldName != "Comentario" && dataColumn.FieldName != "Saldo" && dataColumn.FieldName != "" && dataColumn.FieldName != "IdOperacion" && dataColumn.FieldName != "IdAdministracionFondos")
                {
                    if (e.NewValues[dataColumn.FieldName] == null)
                        e.Errors[dataColumn] = "Valor no puede estar vacío.";
                }
                else
                {
                    montoMov = (gvDetalleMov.FindEditRowCellTemplateControl(gvDetalleMov.Columns["MontoMovimiento"] as GridViewDataColumn, "txtMontoMovimiento") as ASPxTextBox);
                    comentario = (gvDetalleMov.FindEditRowCellTemplateControl(gvDetalleMov.Columns["Comentario"] as GridViewDataColumn, "txtComentario") as ASPxMemo);
                }
            }

            if (e.Errors.Count > 0 || string.IsNullOrEmpty(montoMov.Text)) e.RowError = "Por favor, complete los datos.";

            if (e.NewValues["MontoMovimiento"] != null && e.NewValues["MontoMovimiento"].ToString() != "0")
            {
                AddError(e.Errors, gvDetalleMov.Columns["MontoMovimiento"], "Monto de movimiento debe ser mayor a cero.");
            }
            else
            {
                //validar que el monto del movimiento no sea mayor que el monto del certificado
                //double montoCertificado = util.GetValorDouble(monto.ToString());
                //if (Convert.ToDouble(e.NewValues["MontoMovimiento"]) > montoCertificado)
                //    AddError(e.Errors, gvDetalleMov.Columns["MontoMovimiento"], "Monto de movimiento debe ser menor o igual al monto disponible en el certificado");
            }
        }

        protected void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }

        #endregion

        protected void cbDescargar_Callback(object source, CallbackEventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            int startIndex = (gvDetalleMov.PageCount - 1) * gvDetalleMov.SettingsPager.PageSize;
            int endIndex = Math.Min(gvDetalleMov.VisibleRowCount, startIndex + gvDetalleMov.SettingsPager.PageSize);

            //for (int i = startIndex; i < endIndex; i++) 
            //{
            //    var chk = gvDetalleMov.FindRowCellTemplateControl(i, (GridViewDataColumn)gvDetalleMov.Columns["Instruccion"], "chkInstruccion") as ASPxCheckBox;
            //    if (chk.Checked)
            //    {
            //        var IdDestino = gvDetalleMov.GetRowValues(i, "IdDestino");
            //        var IdDetalle = gvDetalleMov.GetRowValues(i, "IdDetalle");
            //        var idOperacion = gvDetalleMov.GetRowValues(i, "IdOperacion");

            //        var IdAdministracion = gvDetalleMov.GetRowValues(i, "IdAdministracionFondos"); 

            //        LN.InsertarModificaFondos(int.Parse(IdAdministracion.ToString()), "", 0, 0, "", 0, "", 0, "", null, 0, "", 4, "", true);
            //    }
            //}

            for (int i = 0; i < gvDetalleMov.VisibleRowCount; i++)
            {
                //var chk = gvDetalleMov.FindRowCellTemplateControl(i, (GridViewDataCheckColumn)gvDetalleMov.Columns["Instruccion"], "chkInstruccion") as ASPxCheckBox;
                ////gvDetalleMov.GetRowValues(i, "Instruccion de Curse");
                //if (chk.Checked)
                //{
                //    var IdDestino = gvDetalleMov.GetRowValues(i, "IdDestino");
                //    var IdDetalle = gvDetalleMov.GetRowValues(i, "IdDetalle");
                //    var idOperacion = gvDetalleMov.GetRowValues(i, "IdOperacion");

                //    var IdAdministracion = gvDetalleMov.GetRowValues(i, "IdAdministracionFondos"); // Convert.ToInt32(e.Values[gvDetalleMov.KeyFieldName].ToString());

                //    LN.InsertarModificaFondos(int.Parse(IdAdministracion.ToString()), "", 0, 0, "", 0, "", 0, "", null, 0, "", 4, "", true);

                //    //DataTable dt = (DataTable)Page.Session["gvDetalleMov"];

                //    //if (dt.Rows.Count > 0)
                //    //{
                //    //    DataRow[] result = null;
                //    //    //string filtro = string.Format("{0}{1}{2}{3}{4}{5}", "IdOperacion = ", idOperacion, "IdDestino = ", IdDestino, "IdDetalle = ", IdDetalle);
                //    //    string filtro = string.Format("{0}{1}", "IdAdministracionFondos = ", IdAdministracion);
                //    //    result = dt.Select(filtro);
                //    //}   
                //}
            }

            if(HfKey["value"] != null)
                DescargarInstruccionCurse(HfKey["value"].ToString());
        }

        protected void gvDetalleMov_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            var idTipoMov = e.NewValues["IdTipoMov"];
            var idConceptoPago = e.NewValues["IdDestino"];
            var idDetallePago = e.NewValues["IdDetalle"];
            var FechaMov = e.NewValues["FechaMovimiento"];
            var montoMov = (gvDetalleMov.FindEditRowCellTemplateControl(gvDetalleMov.Columns["MontoMovimiento"] as GridViewDataColumn, "txtMontoMovimiento") as ASPxTextBox).Text;
            var comentario = (gvDetalleMov.FindEditRowCellTemplateControl(gvDetalleMov.Columns["Comentario"] as GridViewDataColumn, "txtComentario") as ASPxMemo).Text;

            bool insert = false;
            DataTable dt = (DataTable)Page.Session["gvDetalle"];
            var certificado = HfKey["value"];
            LogicaNegocio LN = new LogicaNegocio();

            DataRow[] result = null;
            SPWeb app2 = SPContext.Current.Web;
            util.ObtenerValor(app2.CurrentUser.Name);

            if (!string.IsNullOrEmpty(certificado.ToString()))
                result = dt.Select("NCertificado = '" + certificado.ToString() + "'");

            if (result != null)
                insert = LN.InsertarModificaFondos(null, result[0]["NCertificado"].ToString().Trim(), Convert.ToInt32(result[0]["IdOperacion"]), Convert.ToInt32(idTipoMov), "", Convert.ToInt32(idConceptoPago), "", Convert.ToInt32(idDetallePago), "", Convert.ToDateTime(FechaMov), montoMov.GetValorDouble(), util.ObtenerValor(app2.CurrentUser.Name), 1, comentario.ToString(), null);
            
            gvDetalleMov.CancelEdit();
            e.Cancel = true;

            CargarDetMov(certificado.ToString());
        }

        private void IncluirExcluirReporte(string idAdministracionFondos, bool seleccionado)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.InsertarModificaFondos(int.Parse(idAdministracionFondos.ToString()), "", 0, 0, "", 0, "", 0, "", null, 0, "", 4, "", seleccionado);
        }

        private void ExcluirReporteCurse(string Ncertificado)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.InsertarModificaFondos(0, Ncertificado, 0, 0, "", 0, "", 0, "", null, 0, "", 5, "", false);
        }

        protected void cbMensaje_Callback(object source, CallbackEventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();
            var certificado = HfKey["value"];
            if (certificado != null)
            {
                bool grabo = LN.InsertaMensajeDistribucion(0, certificado.ToString(), ComentarioGral.Text);
            }
        }

        //protected void pcDetalle_WindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    var idEmpresa = HfEmp["value"];
        //    if (idEmpresa != null)
        //        CargarDetalle(idEmpresa.ToString());
        //}

    }
}
