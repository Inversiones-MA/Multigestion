using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Web.UI.WebControls;
using MultigestionUtilidades;
using System.Web.UI.HtmlControls;
using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using DevExpress.Web;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using Bd;
using ClasesNegocio;

namespace MultiRiesgo.BitacoraCobranza
{
    [ToolboxItemAttribute(false)]
    public partial class BitacoraCobranza : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public BitacoraCobranza()
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Utilidades util = new Utilidades();
        private static string pagina = "BitacoraCobranza.aspx";

        #region eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();

            //PERMISOS USUARIOS
            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dt = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = "";

            dt = permiso.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    Page.Session["dtBitacora"] = null;
                    cargarDdl();
                    CargarEjecutivo(dt.Rows[0]["descCargo"].ToString(), validar.NombreUsuario);
                    CargarGvCobranza();
                }
                if (Page.IsCallback)
                    CargarGvCobranza();
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void gvBitacoraCobranza_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtBitacora"] != null)
            {
                gvBitacoraCobranza.DataSource = (DataTable)Page.Session["dtBitacora"];
                gvBitacoraCobranza.DataBind();
            }
        }

        protected void gvBitacoraCobranza_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameters))
            {
                Page.Session["dtBitacora"] = null;
                string[] rowValueItems = e.Parameters.Split(',');

                string mora = rowValueItems[0];
                string ejecutivo = rowValueItems[2];
                string acreedor = rowValueItems[1];
                string empresa = rowValueItems[3];

                CargarGvCobranza();
            }
        }

        protected void gvBitacoraCobranza_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Resumen resumenP = new Resumen();

            ASPxGridView gvBitacora = sender as ASPxGridView;

            int index = e.VisibleIndex;
            string Ncertificado = gvBitacora.GetRowValues(index, "NumCertificado").ToString();

            resumenP.idOperacion = int.Parse(gvBitacora.GetRowValues(index, "IdOperacion").ToString());
            resumenP.idEmpresa = int.Parse(gvBitacora.GetRowValues(index, "IdEmpresa").ToString());
            resumenP.area = gvBitacora.GetRowValues(index, "DescArea").ToString();
            resumenP.linkActual = "BitacoraCobranza.aspx";
            resumenP.linkPrincial = "BitacoraCobranza.aspx";
            resumenP.linkError = "Mensaje.aspx";

            SPWeb app2 = SPContext.Current.Web;
            resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);

            Page.Session["RESUMEN"] = resumenP;

            if (e.ButtonID == "calendario")
                ASPxWebControl.RedirectOnCallback(string.Format("CalendarioPago.aspx?Ncertificado={0}", Ncertificado));
            if (e.ButtonID == "detalle")
                ASPxWebControl.RedirectOnCallback(string.Format("DetalleCarteraMorosos.aspx?"));
            if (e.ButtonID == "reestructurar")
            {
                Ln.GestionBitacoraCobranza(int.Parse(gvBitacora.GetRowValues(index, "IdEmpresa").ToString()), int.Parse(gvBitacora.GetRowValues(index, "IdOperacion").ToString()), string.Empty, string.Empty, 0, DateTime.Now, string.Empty, false, util.ObtenerValor(app2.CurrentUser.Name), 3);
            }
        }

        protected void cpGuardar_Callback(object sender, CallbackEventArgs e)
        {
            try
            {
                string dato = e.Parameter;
                string resultado = string.Empty;
                if (dato == "guardar")
                {
                    resultado = GuardarGestion();
                    e.Result = resultado;
                }
                if (dato == "guardarTodo")
                {
                    resultado = GuardarTodo();
                    e.Result = resultado;
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        protected void gvSms_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtSms"] != null)
            {
                gvSms.DataSource = (DataTable)Page.Session["dtSms"];
                gvSms.DataBind();
            }
        }

        protected void CpPcComentario_Callback(object sender, CallbackEventArgsBase e)
        {
            CargarBitacoraComentario(int.Parse(e.Parameter));
            //if (!string.IsNullOrEmpty(e.Parameter))
            //    CargarDdlCuotas(e.Parameter);
        }

        protected void CpPcSms_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                cargarSms(e.Parameter);
        }

        protected void gvBitacoraCobranza_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "Tramo")
            {
                if (e.CellValue.ToString().Contains("0-30"))
                    e.Cell.BackColor = Color.Yellow;
                if (e.CellValue.ToString().Contains("60-90"))
                {
                    e.Cell.BackColor = Color.Red;
                    e.Cell.ForeColor = Color.White;
                }
                if (e.CellValue.ToString().Contains("+90"))
                {
                    e.Cell.BackColor = Color.DarkRed;
                    e.Cell.ForeColor = Color.White;
                }
            }
        }

        protected void gvBitacoraGestion_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtGestionBitacora"] != null)
            {
                gvBitacoraGestion.DataSource = (DataTable)Page.Session["dtGestionBitacora"];
                gvBitacoraGestion.DataBind();
            }
        }

        protected void gvBitacoraGestion_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            if (ASPxHiddenFielop["value"] != null)
            {
                SPWeb app2 = SPContext.Current.Web;
                bool insert = false;
                LogicaNegocio Ln = new LogicaNegocio();
                int IdOperacion = Convert.ToInt32(ASPxHiddenFielop["value"]);
                string comentario = (gvBitacoraGestion.FindEditRowCellTemplateControl(gvBitacoraGestion.Columns["Comentario"] as GridViewDataColumn, "txtUltimoMensaje") as ASPxMemo).Text;
                string NroCertificado = string.Empty;

                insert = Ln.GestionBitacoraCobranza(0, IdOperacion, NroCertificado, comentario, int.Parse(e.NewValues["IdAccionCobranza"].ToString()), DateTime.Parse(e.NewValues["FechaGestion"].ToString()), string.Empty, false, util.ObtenerValor(app2.CurrentUser.Name), 2);

                gvBitacoraGestion.CancelEdit();
                e.Cancel = true;

                CargarBitacoraComentario(IdOperacion);
            }
        }

        #endregion

        #region metodos

        private void CargarBitacoraComentario(int idOperacion)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            var dt = Ln.ListarGestionBitacoraCobranza(0, idOperacion, null, null, null, null, null, null, 1);
            Page.Session["dtGestionBitacora"] = dt;
            gvBitacoraGestion.DataSource = dt;
            gvBitacoraGestion.DataBind();
        }

        private void cargarDdl()
        {
            CargarMora();
            CargarAcreedores();
        }

        private void CargarEjecutivo(string cargo, string nombreUsuario)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                cbxEjecutivo.Items.Clear();
                DataTable dt = new DataTable("dt");

                if (cargo == "Sub-Gerente Comercial")
                    dt = Ln.ListarUsuarios(null, null, nombreUsuario);
                else
                    dt = Ln.ListarUsuarios(null, 1, "");

                util.CargaDDLDxx(cbxEjecutivo, dt, "nombreApellido", "idUsuario");

                if (cargo == "Sub-Gerente Comercial")
                {
                    ListEditItem TipoEjecutivo = cbxEjecutivo.Items.FindByText(nombreUsuario);
                    if (TipoEjecutivo != null)
                    {
                        cbxEjecutivo.Text = nombreUsuario;
                        cbxEjecutivo.ReadOnly = true;
                    }
                }
                else
                {
                    ListEditItem TipoEjecutivo = cbxEjecutivo.Items.FindByText(nombreUsuario);
                    if (TipoEjecutivo != null)
                    {
                        cbxEjecutivo.Text = nombreUsuario;
                        cbxEjecutivo.ReadOnly = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        private void CargarMora()
        {
            cbxMora.Items.Insert(0, new ListEditItem("Al Día", "0"));
            cbxMora.Items.Insert(1, new ListEditItem("Cierre Mes Actual", "1"));
            cbxMora.Items.Insert(1, new ListEditItem("Cierre Mes Anterior", "2"));

            cbxMora.SelectedIndex = 0;
        }

        private void CargarAcreedores()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();
            dt = Ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");
            util.CargaDDLDxx(cbxAcreedor, dt, "Nombre", "IdAcreedor");
        }

        private void CargarGvCobranza()
        {
            try
            {
                LogicaNegocio LN = new LogicaNegocio();
                DataTable dt = new DataTable();
                string MoraProyectada = string.IsNullOrEmpty(cbxMora.Text.Trim()) ? "" : cbxMora.Text.Trim();
                string ejecutivo = string.IsNullOrEmpty(cbxEjecutivo.Text.Trim()) ? "" : cbxEjecutivo.Text.Trim();
                string acreedor = string.IsNullOrEmpty(cbxAcreedor.Text.Trim()) ? "" : cbxAcreedor.Text.Trim();
                string empresa = string.IsNullOrEmpty(txtEmpresa.Text.Trim()) ? "" : txtEmpresa.Text.Trim();

                if (Page.Session["dtBitacora"] == null)
                {
                    dt = LN.ListarBitacoraCobranza(MoraProyectada, ejecutivo, acreedor, txtEmpresa.Text.Trim());
                    Page.Session["dtBitacora"] = dt;
                }

                gvBitacoraCobranza.DataSource = (DataTable)Page.Session["dtBitacora"];
                gvBitacoraCobranza.DataBind();
            }
            catch (Exception ex)
            { }
        }

        private void cargarSms(string Ncertificado)
        {
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = LN.ListarSms(Ncertificado);
            Page.Session["dtSms"] = dt;
            gvSms.DataSource = dt;
            gvSms.DataBind();
        }


        public string GuardarGestion()
        {
            bool insert = false;
            var certificado = hfIndex["value"];
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dtBitacora = new DataTable("dtBitacora");
            dtBitacora = (DataTable)Page.Session["dtBitacora"];

            DataRow[] result = null;
            SPWeb app2 = SPContext.Current.Web;
            util.ObtenerValor(app2.CurrentUser.Name);

            if (!string.IsNullOrEmpty(certificado.ToString()))
                result = dtBitacora.Select("NumCertificado = '" + certificado.ToString() + "'");

            //if (result != null)
            //    insert = LN.GestionBitacoraCobranza(Convert.ToInt32(result[0]["IdEmpresa"]), Convert.ToInt32(result[0]["IdOperacion"]), result[0]["NumCertificado"].ToString(), txtNuevaGestion.Text, Convert.ToDateTime(FechaGestion.Value), "", false, util.ObtenerValor(app2.CurrentUser.Name));

            //insert = LN.GuardarGestionCobranza(Convert.ToInt32(result[0]["IdEmpresa"]), Convert.ToInt32(result[0]["IdOperacion"]), result[0]["NumCertificado"].ToString(), txtNuevaGestion.Text, Convert.ToDateTime(FechaGestion.Value), cmbMoraProyectada.Value.ToString(), chkReprogramacion.Checked, util.ObtenerValor(app2.CurrentUser.Name));
            
            if (insert)
                return "ok";
            else
                return "";
        }

        public string GuardarTodo()
        {
            try
            {
                bool insert = false;
                LogicaNegocio LN = new LogicaNegocio();

                //DataRow[] result = null;
                SPWeb app2 = SPContext.Current.Web;
                util.ObtenerValor(app2.CurrentUser.Name);
                DataTable dtBitacora = new DataTable("dtBitacora");
                dtBitacora = (DataTable)Page.Session["dtBitacora"];
                //int i = 0;
                //foreach (DataRow row in dtBitacora.Rows)
                for (int i = 0; i <= gvBitacoraCobranza.VisibleRowCount; i++)
                {
                    var comentario = gvBitacoraCobranza.FindRowCellTemplateControl(i, (GridViewDataColumn)gvBitacoraCobranza.Columns["UltimoCompromiso"], "txtUltimoMensaje") as ASPxMemo;
                    //var b = comentario.Text.Trim();
                    //var cc = int.Parse(row["IdEmpresa"].ToString());
                    if (comentario != null)
                    {
                        if (comentario.Text != "")
                            insert = LN.GestionBitacoraCobranza(Convert.ToInt32(gvBitacoraCobranza.GetRowValues(i, "IdEmpresa").ToString()), Convert.ToInt32(gvBitacoraCobranza.GetRowValues(i, "IdOperacion").ToString()), gvBitacoraCobranza.GetRowValues(i, "NumCertificado").ToString(), comentario.Text.Trim(), int.Parse(gvBitacoraCobranza.GetRowValues(i, "IdAccionCobranza").ToString()), DateTime.Now, "0", false, util.ObtenerValor(app2.CurrentUser.Name), 2);
                    }
                    //i++;
                }

                if (insert)
                    return "ok";
                else
                    return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void gvBitacoraGestion_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "NroCertificado")
            {
                var grid = e.Column.Grid;
                if (!grid.IsNewRowEditing)
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        #endregion

        protected void gvBitacoraCobranza_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                if (e.RowType != GridViewRowType.Data) return;

                var value = e.KeyValue.ToString().Split('|');
                //bool hasError = value == null ? false : Convert.ToBoolean(value[3]);
                //if (hasError)
                //e.Row.BackColor = System.Drawing.Color.LightCyan;
            }
            catch (Exception ex)
            {

            }
        }

        protected void gvBitacoraCobranza_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            try
            {
                if (e.ButtonID == "reestructurar")
                {
                    ASPxGridView gvDetalleSol = sender as ASPxGridView;

                    bool EstadoSolicitud = gvDetalleSol.GetRowValues(e.VisibleIndex, "EsReestructuracion") != null ? Convert.ToBoolean(gvDetalleSol.GetRowValues(e.VisibleIndex, "EsReestructuracion")) : false;

                    if (EstadoSolicitud)
                        e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            catch (Exception)
            {

            }
        }

        //string GetImagePath(string state)
        //{
        //    switch (state)
        //    {
        //        case "deleted":
        //            return "~/Images/undo.gif";
        //        case "undone":
        //            return "~/Images/delete.gif";
        //        default:
        //            return string.Empty;
        //    }
        //}


    }
}
