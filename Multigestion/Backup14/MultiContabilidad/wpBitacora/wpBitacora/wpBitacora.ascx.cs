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
using Bd;
using ClasesNegocio;

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

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    CargarBitacora(null);
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-CL");

            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = "",
            };

            dt = validar.ListarPerfil(validar);
            Page.Session["dtRol"] = dt;

            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {                           
                    CargarBitacora(null);
                }
                else
                {
                    if (GvCuentaCorriente.IsCallback)
                        CargarBitacora(null);
                    //if (!Page.IsCallback)
                    //{
                    //    CargarBitacora(null);
                    //}
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        #region "bitacora"
        protected void CargarBitacora(string[] parametros)
        {
            var certificado = parametros == null ? string.Empty : parametros[1];
            var razonsocial = parametros == null ? string.Empty : parametros[2];
            LogicaNegocio Ln = new LogicaNegocio();
            Page.Session["dtBitacora"] = Ln.GestionBitacoraPago(certificado, razonsocial, 1);
            GvCuentaCorriente.DataSource = (DataTable)Page.Session["dtBitacora"];
            GvCuentaCorriente.DataBind();
        }

        protected void GvCuentaCorriente_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtBitacora"] != null)
            {
                GvCuentaCorriente.DataSource = (DataTable)Page.Session["dtBitacora"];
                GvCuentaCorriente.DataBind();
            }
        }

        protected void GvCuentaCorriente_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView cPanel = (ASPxGridView)sender;
            cPanel.JSProperties["cpPanelMensaje"] = null;

            var parametros = e.Parameters.Split(',');
            if (parametros[0] == "buscar")
            {
                CargarBitacora(parametros);
            }
            if (parametros[0] == "excel")
            {
                cPanel.JSProperties["cpPanelMensaje"] = null;
                bool descarga = DescargarExcel(parametros);
                if (!descarga)
                    cPanel.JSProperties["cpPanelMensaje"] = "no hay datos para descargar";
            }
        }

        protected void GvCuentaCorriente_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                bool ingresar = false;

                //ingresar = Ln.Gestion_BitacoraPago(null, e.NewValues["NCF"].ToString(), int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), 0, Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Comentario"].ToString(), null, util.ObtenerValor(app2.CurrentUser.Name), 3);
                ingresar = Ln.Gestion_BitacoraPago(null, e.NewValues["NCF"].ToString(), int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), int.Parse(e.NewValues["IdBanco"].ToString()), Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Cuota"].ToString(), int.Parse(e.NewValues["IdAcreedor"].ToString()), e.NewValues["Comentario"].ToString(), null, util.ObtenerValor(app2.CurrentUser.Name), 3, int.Parse(e.NewValues["IdConcepto"].ToString()));

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            GvCuentaCorriente.CancelEdit();
            e.Cancel = true;

            string[] parametros = new string[] { "buscar", TxtCertificado.Text.Trim(), TxtRazonSocial.Text.Trim() };
            CargarBitacora(parametros);
        }

        protected void GvCuentaCorriente_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
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

        protected void GvCuentaCorriente_DataBound(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Page.Session["dtRol"];
            if (!util.EstaPermitido(dt.Rows[0]["descCargo"].ToString(), Cargos))
            {
                //gvBitacora.Columns["Pagar"].Visible = false;
                GvCuentaCorriente.Columns[0].Visible = false;
            }
            else
                GvCuentaCorriente.Columns[0].Visible = true;

        }

        protected void GvCuentaCorriente_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {
            //if (e.Exception is NullReferenceException)
            //    e.ErrorText = e.Exception.Message;
            //else if (e.Exception is NotSupportedException)
            //    e.ErrorText = e.Exception.Message;
            //else
            //    e.ErrorText = "Ha ocurrido un error";
        }

        protected void GvCuentaCorriente_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                bool modificar = false;
                LogicaNegocio LN = new LogicaNegocio();

                int IdBitacoraPago = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                if (IdBitacoraPago != 0)
                {
                    modificar = Ln.Gestion_BitacoraPago(null, null, int.Parse(e.NewValues["IdMotivo"].ToString()), int.Parse(e.NewValues["IdCausa"].ToString()), int.Parse(e.NewValues["IdBanco"].ToString()), Convert.ToDateTime(e.NewValues["FechaCobro"]), Convert.ToDateTime(e.NewValues["FechaPago"].ToString()), e.NewValues["NroDocumento"].ToString(), int.Parse(e.NewValues["Monto"].ToString()), e.NewValues["Cuota"].ToString(), int.Parse(e.NewValues["IdAcreedor"].ToString()), e.NewValues["Comentario"].ToString(), IdBitacoraPago, util.ObtenerValor(app2.CurrentUser.Name), 2, int.Parse(e.NewValues["IdConcepto"].ToString()));
                    if (!modificar)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

            GvCuentaCorriente.CancelEdit();
            e.Cancel = true;

            string[] parametros = new string[] { "buscar", TxtCertificado.Text.Trim(), TxtRazonSocial.Text.Trim() };
            CargarBitacora(parametros);
        }

        protected void GvCuentaCorriente_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;
                bool eliminar = false;
                LogicaNegocio LN = new LogicaNegocio();

                int IdBitacoraPago = e.Keys[0] == null ? 0 : Convert.ToInt32(e.Keys[0]);

                if (IdBitacoraPago != 0)
                    eliminar = Ln.Gestion_BitacoraPago(null, null, null, null, null, null, null, null, null, null, 0, null, IdBitacoraPago, util.ObtenerValor(app2.CurrentUser.Name), 4);

                e.Cancel = true;

                string[] parametros = new string[] { "buscar", TxtCertificado.Text.Trim(), TxtRazonSocial.Text.Trim() };
                CargarBitacora(parametros);
            }
            catch (Exception)
            {

            }
        }

        private bool DescargarExcel(string[] parametros)
        {
            bool Descarga = false;
            DataTable dt = (DataTable)Page.Session["dtBitacora"];
            string certificado = parametros == null ? string.Empty : parametros[1];
            string razonsocial = parametros == null ? string.Empty : parametros[2];
            DataTable dtFilter = null;

            //var aa = dt.AsEnumerable().Where(r => r.Field<string>("NCF").Equals(string.IsNullOrEmpty(certificado) ? r.Field<string>("NCF") : certificado)
            //                               && r.Field<string>("RazonSocial").Contains(string.IsNullOrEmpty(razonsocial) ? r.Field<string>("RazonSocial") : razonsocial)
            //                               ).Any();

            //var bb = dt.AsEnumerable().Where(r => r.Field<string>("NCF").Equals(string.IsNullOrEmpty(certificado) ? r.Field<string>("NCF") : certificado)
            //                                   && r.Field<string>("RazonSocial").Contains(string.IsNullOrEmpty(razonsocial) ? r.Field<string>("RazonSocial") : razonsocial)
            //                                   );

            dtFilter = !dt.AsEnumerable().Where(r => r.Field<string>("NCF").Equals(string.IsNullOrEmpty(certificado) ? r.Field<string>("NCF") : certificado)
                                               && r.Field<string>("RazonSocial").Contains(string.IsNullOrEmpty(razonsocial) ? r.Field<string>("RazonSocial") : razonsocial)
                                               ).Any()
                                               ? new DataTable()
                                               : dt.AsEnumerable().Where(r => r.Field<string>("NCF").Equals(string.IsNullOrEmpty(certificado) ? r.Field<string>("NCF") : certificado)
                                               && r.Field<string>("RazonSocial").Contains(string.IsNullOrEmpty(razonsocial) ? r.Field<string>("RazonSocial") : razonsocial)
                                               ).CopyToDataTable();

            if (dtFilter.Rows.Count > 0)
            {
                Descarga = true;
                dtFilter.Columns.Remove("Id");
                dtFilter.Columns.Remove("IdMotivo");
                dtFilter.Columns.Remove("IdCausa");
                dtFilter.Columns.Remove("IdBanco");
                dtFilter.Columns.Remove("IdAcreedor");
                dtFilter.Columns.Remove("IdConcepto");

                dtFilter.Columns["NCF"].ColumnName = "N° CF";
                dtFilter.Columns["RazonSocial"].ColumnName = "Razón Social";
                dtFilter.Columns["DescMotivo"].ColumnName = "Motivo";
                dtFilter.Columns["DescCausa"].ColumnName = "Causa";

                Page.Session["tipoDoc"] = "excel";
                Page.Session["binaryData"] = util.convierteExcel(dtFilter);
                Page.Session["Titulo"] = "Cuenta Corriente";
                ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
            }

            return Descarga;
        }
        #endregion

        #region "carga masiva"

        protected void GvPreCargaMasivo_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtArchivo"] != null)
            {
                GvPreCargaMasivo.DataSource = (DataTable)Page.Session["dtArchivo"];
                GvPreCargaMasivo.DataBind();
            }
        }

        protected void GvPreCargaMasivo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ASPxGridView cPanel = (ASPxGridView)sender;
            cPanel.JSProperties["cpPanelMensaje"] = null;

            switch (e.Parameters.Trim())
            {
                case "nuevo":
                    cPanel.JSProperties["cpPanelMensaje"] = "nuevoLimpio";
                    GvPreCargaMasivo.DataSource = (DataTable)Page.Session["dtArchivo"];
                    GvPreCargaMasivo.DataBind();
                    break;
                case "limpiar":
                    cPanel.JSProperties["cpPanelMensaje"] = "nuevoLimpio";
                    Page.Session["dtArchivo"] = null;
                    GvPreCargaMasivo.DataBind();
                    break;
                case "Insertar":
                    cPanel.JSProperties["cpPanelMensaje"] = InsertarCuentaCorriente();
                    break;
                default:
                    break;
            }
        }

        protected void UpSubirCC_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            try
            {
                if (e.IsValid)
                {
                    int validarExiste = 0;
                    LogicaNegocio Ln = new LogicaNegocio();
                    DataTable dtMotivo = new DataTable("dtMotivo");
                    DataTable dtCausa = new DataTable("dtCausa");
                    DataTable dtAcreedores = new DataTable("dtAcreedores");
                    DataTable dtConceptos = new DataTable("dtConceptos");

                    dtMotivo = Ln.ListarMotivos();
                    dtCausa = Ln.ListarCausas();
                    dtConceptos = Ln.ListarConceptos();
                    dtAcreedores = Ln.CRUDAcreedores(1, "", 0, "", "", 0, 0, 0, 0, "");
                    int BuscarMotivo = 0;
                    int BuscarCausa = 0;
                    int BuscarBanco = 0;
                    int BuscarAcreedor = 0;
                    int BuscarConcepto = 0;

                    DataTable dtCsv = util.CSVtoDataTable(e.UploadedFile, ";");
                    dtCsv.CaseSensitive = false;

                    foreach (DataColumn column in dtCsv.Columns)
                        column.ColumnName = column.ColumnName.ToLower().Trim();

                    for (var i = 0; i < dtCsv.Rows.Count - 1; i++)
                    {
                        //n° cf
                        string NroCertificado = dtCsv.Rows[i]["NCertificado"].ToString().Trim();
                        if (string.IsNullOrEmpty(NroCertificado))
                            throw new Exception("El numero de certificado no puede estar vacio, linea " + (i + 1));
                        validarExiste = Ln.CP_VerificarCertificado(NroCertificado);
                        if (validarExiste == 0)
                            throw new Exception("El número de certificado no esta registrado, cft " + NroCertificado);

                        //motivo
                        string Motivo = dtCsv.Rows[i]["Motivo"].ToString().Trim();
                        if (string.IsNullOrEmpty(Motivo))
                            throw new Exception("El Motivo ingresado no puede estar vacío, linea " + (i + 1));
                        BuscarMotivo = dtMotivo.Select("DescMotivo = '" + Motivo + "'").Length;
                        if (BuscarMotivo <= 0)
                            throw new Exception("El motivo no existe en la parametrización, linea " + (i + 1));

                        //causa
                        string Causa = dtCsv.Rows[i]["Causa"].ToString().Trim();
                        if (string.IsNullOrEmpty(Causa))
                            throw new Exception("La causa ingresada no puede estar vacía, linea " + (i + 1));
                        string CausaExiste = string.Format("{0}", "DescCausa = '" + Causa + "'");
                        BuscarCausa = dtCausa.Select(CausaExiste).Length;
                        if (BuscarCausa <= 0)
                            throw new Exception("La causa no existe en la parametrización, linea " + (i + 1));

                        //concepto
                        string Concepto = dtCsv.Rows[i]["Concepto"].ToString().Trim();
                        if (string.IsNullOrEmpty(Concepto))
                            throw new Exception("el concepto ingresado no puede estar vacío, linea " + (i + 1));
                        string ConceptoExiste = string.Format("{0}", "Concepto = '" + Concepto + "'");
                        BuscarConcepto = dtConceptos.Select(ConceptoExiste).Length;
                        if (BuscarConcepto <= 0)
                            throw new Exception("El concepto ingresado no existe en la parametrización de conceptos, linea " + (i + 1));

                        //fecha cobro
                        string FechaCobro = dtCsv.Rows[i]["FechaCobro"].ToString().Trim();
                        if (string.IsNullOrEmpty(FechaCobro))
                            throw new Exception("La fecha de cobro no puede estar vacia, linea " + (i + 1));

                        //fecha pago
                        string FechaPago = dtCsv.Rows[i]["FechaPago"].ToString().Trim();
                        if (string.IsNullOrEmpty(FechaPago))
                            throw new Exception("La Fecha de Pago no puede estar vacía, linea " + (i + 1));

                        //banco
                        string banco = System.Web.HttpUtility.HtmlDecode(dtCsv.Rows[i]["Banco"].ToString().Trim());
                        if (string.IsNullOrEmpty(banco))
                            throw new Exception("El banco no puede estar vacio o ser cero, linea " + (i + 1));

                        //string BancoExiste = string.Format("{0}", "NombreFinanzas = '" + banco.ToUpper() + "'");
                        //BuscarBanco = dtAcreedores.Select(BancoExiste).Length;
                        //if (BuscarBanco <= 0)
                        //    throw new Exception("El banco no existe en la parametrización, linea " + (i + 1));

                        //monto
                        string Monto = dtCsv.Rows[i]["Monto"].ToString().Trim();
                        if (string.IsNullOrEmpty(Monto) || Monto == "0")
                            throw new Exception("El Monto no puede estar vacio o ser cero, linea " + (i + 1));
                        dtCsv.Rows[i]["Monto"] = (dtCsv.Rows[i]["Monto"]).ToString().GetFormatearNumero(0);

                        //acreedor
                        string Acreedor = dtCsv.Rows[i]["Acreedor"].ToString().Trim();
                        if (string.IsNullOrEmpty(Acreedor))
                            throw new Exception("El acreedor no puede estar vacio, linea " + (i + 1));
                        string AcreedorExiste = string.Format("{0}", "NombreFinanzas = '" + Acreedor.ToUpper() + "'");
                        BuscarAcreedor = dtAcreedores.Select(AcreedorExiste).Length;
                        if (BuscarAcreedor <= 0)
                            throw new Exception("El acreedor no existe en la parametrización, linea " + (i + 1));

                    }
                    Page.Session["dtArchivo"] = dtCsv;
                    e.CallbackData = "OK";
                }
                else
                    throw new Exception("Error de archivo");
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

        protected string InsertarCuentaCorriente()
        {
            string mensaje = string.Empty;
            LogicaNegocio Ln = new LogicaNegocio();
            SPWeb app2 = SPContext.Current.Web;

            if (Page.Session["dtArchivo"] != null)
            {
                DataTable dt = new DataTable("dtArchivo");
                dt = (DataTable)Page.Session["dtArchivo"];
                DataTable dtCopy = dt.Copy();
                DataSet ds = new DataSet("cuentaCorriente");
                ds.Tables.Add(dtCopy);

                var xml = util.DatasetToXml(ds);
                if (xml != null)
                {
                    if (Ln.Gestion_BitacoraPago(xml, null, null, null, null, null, null, null, null, null, 0, null, null, util.ObtenerValor(app2.CurrentUser.Name), 6))
                        mensaje = string.Format("{0}{1}{2}", "OK", ";", "ingreso correcto");
                    else
                        mensaje = "error al intentar cargar la información";
                }
            }
            else
                mensaje = "error al intentar cargar la información";

            return mensaje;
        }

        #endregion

    }
}