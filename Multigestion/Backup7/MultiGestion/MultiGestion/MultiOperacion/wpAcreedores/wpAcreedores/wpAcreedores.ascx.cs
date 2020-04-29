using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using DevExpress.Web;
using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Data;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;

namespace MultiOperacion.wpAcreedores.wpAcreedores
{
    [ToolboxItemAttribute(false)]
    public partial class wpAcreedores : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpAcreedores()
        {
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "Acreedores.aspx";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Permisos permiso = new Permisos();
            ValidarPermisos validar = new ValidarPermisos();
            LogicaNegocio LN = new LogicaNegocio();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    if (Page.Session["BUSQUEDA"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["gvAcreedores"] = null;
                        Page.Session["BUSQUEDA"] = null;
                    }
                    CargarAcreedores("", "");
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void gvAcreedores_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["gvAcreedores"] != null)
            {
                gvAcreedores.DataSource = (DataTable)Page.Session["gvAcreedores"];
                gvAcreedores.DataBind();
            }
        }

        private void CargarAcreedores(string NombreAcreedor, string rutAcreedor)
        {
            LogicaNegocio ln = new LogicaNegocio();
            var dt = ln.CRUDAcreedores(1, rutAcreedor, 0, NombreAcreedor, "", 0, 0, 0, 0, "");
            Page.Session["gvAcreedores"] = dt;
            gvAcreedores.DataSource = dt;
            gvAcreedores.DataBind();
        }

        protected void gvAcreedores_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                string[] rowValueItems = e.Parameters.Split(',');

                string NombreAcreedor = rowValueItems[0];
                string rutAcreedor = rowValueItems[1];

                CargarAcreedores(NombreAcreedor, rutAcreedor);
            }
        }

        protected void gvAcreedores_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Idprovincia")
                {
                    var combo = (ASPxComboBox)e.Editor;
                    combo.Callback += new CallbackEventHandlerBase(DetalleCiudad);

                    var grid = e.Column.Grid;
                    if (!combo.IsCallback)
                    {
                        var idRegionTemp = -1;
                        if (!grid.IsNewRowEditing)
                        {
                            var a = grid.GetRowValues(e.VisibleIndex, "IdRegion").ToString();
                            if (!string.IsNullOrEmpty(a))
                                idRegionTemp = Convert.ToInt32(a.ToString());
                            //idRegionTemp = int.Parse(grid.GetRowValues(e.VisibleIndex, "IdRegion").ToString());


                            Ciudad(combo, idRegionTemp);
                        }
                    }
                }

                if (e.Column.FieldName == "IdComuna")
                {
                    var combo = (ASPxComboBox)e.Editor;
                    combo.Callback += new CallbackEventHandlerBase(DetalleComuna);

                    var grid = e.Column.Grid;
                    if (!combo.IsCallback)
                    {
                        var IdProvinciaTemp = -1;
                        if (!grid.IsNewRowEditing)
                        {
                            try
                            {
                                var a = grid.GetRowValues(e.VisibleIndex, "Idprovincia").ToString();
                                if (!string.IsNullOrEmpty(a))
                                    IdProvinciaTemp = Convert.ToInt32(a.ToString());
                            }
                            catch (Exception)
                            {
                            }

                            Comuna(combo, IdProvinciaTemp);
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }
        private void DetalleCiudad(object sender, CallbackEventArgsBase e)
        {
            var IdRegion = -1;
            string a = e.Parameter;
            Int32.TryParse(e.Parameter, out IdRegion);
            Ciudad(sender as ASPxComboBox, IdRegion);
        }

        protected void Ciudad(ASPxComboBox combo, int IdRegion)
        {
            combo.DataSourceID = "dsProvinciaPor";
            dsProvinciaPor.SelectParameters["IdRegion"].DefaultValue = IdRegion.ToString();
            combo.DataBindItems();
            combo.Items.Insert(0, new ListEditItem("", null));
        }


        private void DetalleComuna(object sender, CallbackEventArgsBase e)
        {
            var IdProvincia = -1;
            string a = e.Parameter;
            Int32.TryParse(e.Parameter, out IdProvincia);
            Comuna(sender as ASPxComboBox, IdProvincia);
        }

        protected void Comuna(ASPxComboBox combo, int IdProvincia)
        {
            combo.DataSourceID = "dsComunaPor";
            dsComunaPor.SelectParameters["IdCiudad"].DefaultValue = IdProvincia.ToString();
            combo.DataBindItems();
            combo.Items.Insert(0, new ListEditItem("", null));
        }

        protected void gvAcreedores_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                SPWeb app2 = SPContext.Current.Web;
                bool modificar = false;
                LogicaNegocio LN = new LogicaNegocio();

                int IdAcreedor = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                if (IdAcreedor != 0)
                    modificar = LN.Acreedores(2, e.NewValues["Rut"].ToString(), IdAcreedor, e.NewValues["Nombre"].ToString(), e.NewValues["Domicilio"].ToString(), Convert.ToInt32(e.NewValues["IdTipoAcreedor"]), Convert.ToInt32(e.NewValues["IdRegion"]), Convert.ToInt32(e.NewValues["Idprovincia"]), Convert.ToInt32(e.NewValues["IdComuna"]), util.ObtenerValor(app2.CurrentUser.Name));

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvAcreedores.CancelEdit();
            e.Cancel = true;

            CargarAcreedores(txtAcreedor.Text.Trim(), txtRutAcreedor.Text.Trim());
        }

        protected void gvAcreedores_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                SPWeb app2 = SPContext.Current.Web;
                bool crear = false;
                LogicaNegocio LN = new LogicaNegocio();

                crear = LN.Acreedores(3, e.NewValues["Rut"].ToString(), 0, e.NewValues["Nombre"].ToString(), e.NewValues["Domicilio"].ToString(), Convert.ToInt32(e.NewValues["IdTipoAcreedor"]), Convert.ToInt32(e.NewValues["IdRegion"]), Convert.ToInt32(e.NewValues["Idprovincia"]), Convert.ToInt32(e.NewValues["IdComuna"]), util.ObtenerValor(app2.CurrentUser.Name));
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvAcreedores.CancelEdit();
            e.Cancel = true;

            CargarAcreedores(txtAcreedor.Text.Trim(), txtRutAcreedor.Text.Trim());
        }

        protected void gvAcreedores_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            //validar rut de acreedor
            LogicaNegocio ln = new LogicaNegocio();
            bool existe = false;
            foreach (GridViewColumn column in gvAcreedores.Columns)
            {
                GridViewDataColumn dataColumn = column as GridViewDataColumn;
                if (dataColumn == null) continue;
                if (dataColumn.FieldName == "Rut")
                {
                    if (e.IsNewRow)
                    {
                        existe = ln.Acreedores(4, e.NewValues["Rut"].ToString(), 0, "", "", 0, 0, 0, 0, "");
                        if (existe)
                            e.Errors[dataColumn] = "El Rut Ingresado ya esta registrado como acreedor.";
                    }
                }
            }
        }

        protected void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }

    }
}
