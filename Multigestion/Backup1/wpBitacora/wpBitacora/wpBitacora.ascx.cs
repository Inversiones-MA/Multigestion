using Microsoft.SharePoint;
using MultigestionUtilidades;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System.Diagnostics;
using DevExpress.Web;
using System.Globalization;
using System.Linq;
//using System.Globalization;

namespace MultiContabilidad.wpBitacora.wpBitacora
{
    [ToolboxItemAttribute(false)]
    public partial class wpBitacora : WebPart
    {
        private static string pagina = "Bitacora.aspx";
        private static string[] Cargos = { "Administrador", "Analista Finanzas" };
        Utilidades util = new Utilidades();

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpBitacora()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-CL");

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
            Page.Session["dtRol"] = dt;

            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    CargarBitacora(null);
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void CargarBitacora(string[] parametros)
        {
            var certificado = parametros == null ? string.Empty : parametros[1];
            var razonsocial = parametros == null ? string.Empty : parametros[2];
            LogicaNegocio Ln = new LogicaNegocio();
            Page.Session["dtBitacora"] = Ln.GestionBitacoraPago(null, certificado, null, null, null, null, null, null, null, null, null, null, razonsocial, 1);
            gvBitacora.DataSource = (DataTable)Page.Session["dtBitacora"];
            gvBitacora.DataBind();
        }

        protected void gvBitacora_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                bool eliminar = false;
                LogicaNegocio LN = new LogicaNegocio();

                int IdBitacoraPago = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                if (IdBitacoraPago != 0)
                    eliminar = Ln.Gestion_BitacoraPago(null, null, null, null, null, null, null, null, null, null, IdBitacoraPago, util.ObtenerValor(app2.CurrentUser.Name), 4);

                e.Cancel = true;

                string[] parametros = new string[] { "buscar", txtCertificado.Text.Trim(), txtRazonSocial.Text.Trim() };
                CargarBitacora(parametros);
            }
            catch (Exception)
            {

            }
        }

        protected void gvBitacora_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                bool modificar = false;
                LogicaNegocio LN = new LogicaNegocio();

                int IdBitacoraPago = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                if (IdBitacoraPago != 0)
                    modificar = Ln.Gestion_BitacoraPago(null, null, int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), int.Parse(e.NewValues["IdBanco"].ToString()), Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Comentario"].ToString(), IdBitacoraPago, util.ObtenerValor(app2.CurrentUser.Name), 2);
                //modificar = Ln.Gestion_BitacoraPago(null, null, int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), 0, Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Comentario"].ToString(), IdBitacoraPago, util.ObtenerValor(app2.CurrentUser.Name), 2);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvBitacora.CancelEdit();
            e.Cancel = true;

            string[] parametros = new string[] { "buscar", txtCertificado.Text.Trim(), txtRazonSocial.Text.Trim() };
            CargarBitacora(parametros);
        }

        protected void gvBitacora_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                bool ingresar = false;
                LogicaNegocio LN = new LogicaNegocio();

                //ingresar = Ln.Gestion_BitacoraPago(null, e.NewValues["NCF"].ToString(), int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), 0, Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Comentario"].ToString(), null, util.ObtenerValor(app2.CurrentUser.Name), 3);
                ingresar = Ln.Gestion_BitacoraPago(null, e.NewValues["NCF"].ToString(), int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), int.Parse(e.NewValues["IdBanco"].ToString()), Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Comentario"].ToString(), null, util.ObtenerValor(app2.CurrentUser.Name), 3);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            gvBitacora.CancelEdit();
            e.Cancel = true;

            string[] parametros = new string[] { "buscar", txtCertificado.Text.Trim(), txtRazonSocial.Text.Trim() };
            CargarBitacora(parametros);
        }

        protected void gvBitacora_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtBitacora"] != null)
            {
                gvBitacora.DataSource = (DataTable)Page.Session["dtBitacora"];
                gvBitacora.DataBind();
            }
        }

        protected void gvBitacora_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parametros = e.Parameters.Split(',');
            if (parametros[0] == "buscar")
            {
                CargarBitacora(parametros);
            }
            if (parametros[0] == "excel")
            {
                DescargarExcel(parametros);
            }
        }


        private void DescargarExcel(string[] parametros)
        {
            DataTable dt = (DataTable)Page.Session["dtBitacora"];
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Id");
                dt.Columns.Remove("IdMotivo");
                dt.Columns.Remove("IdCausa");
                dt.Columns.Remove("IdBanco");

                dt.Columns["NCF"].ColumnName = "N° CF";
                dt.Columns["RazonSocial"].ColumnName = "Razón Social";
                dt.Columns["DescMotivo"].ColumnName = "Motivo";
                dt.Columns["DescCausa"].ColumnName = "Causa";
            }

            Page.Session["tipoDoc"] = "excel";
            Page.Session["binaryData"] = util.convierteExcel(dt);
            Page.Session["Titulo"] = "Cuenta Corriente";
            ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
        }

        protected void gvBitacora_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "NCF")
            {
                var grid = e.Column.Grid;
                if (!grid.IsNewRowEditing)
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void gvBitacora_DataBound(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Page.Session["dtRol"];
            if (!util.CargosPermitidos(dt.Rows[0]["descCargo"].ToString(), Cargos))
            {
                //gvBitacora.Columns["Pagar"].Visible = false;
                gvBitacora.Columns[0].Visible = false;
            }
            else
                gvBitacora.Columns[0].Visible = true;

        }

        protected void gvPreCargaMasivo_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtArchivo"] != null)
            {
                gvPreCargaMasivo.DataSource = (DataTable)Page.Session["dtArchivo"];
                gvPreCargaMasivo.DataBind();
            }
        }

        protected void gvPreCargaMasivo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            switch (e.Parameters.Trim())
            {
                case "nuevo":
                    gvPreCargaMasivo.DataSource = (DataTable)Page.Session["dtArchivo"];
                    gvPreCargaMasivo.DataBind();
                    break;
                case "limpiar":
                    Page.Session["dtArchivo"] = null;
                    gvPreCargaMasivo.DataBind();
                    break;
                case "San Joaquin":
                    //com = "San Joaquín";
                    break;
                default:
                    break;
            }
        }

        protected void UpSubirMasivo_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (UpSubirMasivo.UploadedFiles != null && UpSubirMasivo.UploadedFiles.Length > 0)
                {
                    int validarExiste = 0;
                    LogicaNegocio ln = new LogicaNegocio();
                    DataTable dtMotivo = new DataTable("dtMotivo");
                    DataTable dtCausa = new DataTable("dtCausa");
                    DataTable dtAcreedores = new DataTable("dtAcreedores");

                    dtMotivo = ln.ListarMotivos();
                    dtCausa = ln.ListarCausas();
                    dtAcreedores = ln.CRUDAcreedores(1, "", 0, "", "", 0, 0, 0, 0, "");

                    var dtCsv = util.CSVtoDataTable(e.UploadedFile, ";");

                    for (var i = 0; i < dtCsv.Rows.Count - 1; i++)
                    {
                        var NroCertificado = dtCsv.Rows[i]["NCertificado"].ToString().Trim();
                        if (string.IsNullOrEmpty(NroCertificado.ToString()))
                            throw new Exception("El numero de certificado no puede estar vacio, linea " + i);

                        //validar si existe nro certificado en tabla operacion
                        validarExiste = ln.CP_VerificarCertificado(NroCertificado);
                        if (validarExiste == 0)
                            throw new Exception("El número de certificado no esta registrado, linea " + i);

                        var Motivo = dtCsv.Rows[i]["Motivo"].ToString().Trim();
                        if (string.IsNullOrEmpty(Motivo.ToString()))
                            throw new Exception("El Motivo ingresado no existe en la parametrización, linea " + i);

                        var Causa = dtCsv.Rows[i]["Causa"].ToString().Trim();
                        if (string.IsNullOrEmpty(Causa.ToString()))
                            throw new Exception("La causa ingresada no existe en la parametrización, linea " + i);

                        var Monto = dtCsv.Rows[i]["Monto"].ToString().Trim();
                        if (string.IsNullOrEmpty(Monto.ToString()) || Monto.ToString() == "0")
                            throw new Exception("El Monto no puede estar vacio o ser cero, linea " + i);

                        var FechaCobro = dtCsv.Rows[i]["FechaCobro"].ToString().Trim();
                        if (string.IsNullOrEmpty(FechaCobro.ToString()))
                            throw new Exception("La fecha de cobro no puede estar vacia, linea " + i);

                        var FechaPago = dtCsv.Rows[i]["FechaPago"].ToString().Trim();
                        if (string.IsNullOrEmpty(FechaPago.ToString()))
                            throw new Exception("La Fecha de Pago no puede estar vacía, linea " + i);

                        var Acreedor = dtCsv.Rows[i]["Acreedor"].ToString().Trim();
                        if (string.IsNullOrEmpty(Acreedor.ToString()))
                            throw new Exception("El acreedor no puede estar vacio, linea " + i);
                        //else
                        //    dtCsv.Rows[i]["Acreedor"] = System.Web.HttpUtility.HtmlEncode(dtCsv.Rows[i]["Acreedor"].ToString().Trim());

                        
                    }
                    //System.Web.HttpUtility.HtmlDecode(
                    Page.Session["dtArchivo"] = dtCsv; //HtmlDecodeDataTable(dtCsv);
                    e.CallbackData = "OK";
                }
            }
            catch (Exception ex)
            {
                e.CallbackData = ex.Message;
            }
        }

        public static DataTable HtmlDecodeDataTable(DataTable dTable)
        {
            foreach (DataRow drow in dTable.Rows)
            {
                for (int i = 0; i < drow.ItemArray.Length; i++)
                    if (drow[i].GetType() == typeof(System.String))
                    {
                        var texto = drow[i].ToString();
                        drow[i] = System.Web.HttpUtility.HtmlEncode((drow[i].ToString()));
                    }
            }
            dTable.AcceptChanges();
            return dTable;
        }

        protected void cbpCargarExcel_Callback(object source, CallbackEventArgs e)
        {

        }
    }
}
