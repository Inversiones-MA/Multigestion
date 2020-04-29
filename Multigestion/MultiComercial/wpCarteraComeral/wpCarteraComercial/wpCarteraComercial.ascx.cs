using Bd;
using ClasesNegocio;
using DevExpress.Web;
using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using MultigestionUtilidades;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace MultiComercial.wpCarteraComeral.wpCarteraComercial
{
    [ToolboxItemAttribute(false)]
    public partial class wpCarteraComercial : WebPart
    {
        Utilidades util = new Utilidades();
        private static string pagina = "CarteraComercial.aspx";
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpCarteraComercial()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //LogicaNegocio Ln = new LogicaNegocio();

            //PERMISOS USUARIOS
            string PermisoConfigurado = string.Empty;
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
                if (!Page.IsPostBack)
                {
                    CargarTipoCartera();
                    CargarAcreedores();
                    CargarEjecutivo(dt.Rows[0]["descCargo"].ToString(), validar.NombreUsuario);
                    Page.Session["dtCarteraEjecutivo"] = null;
                    CargarGvCartera();
                }

                if (Page.IsCallback)
                {
                    CargarGvCartera();
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        private void CargarTipoCartera()
        {
            cbxCartera.Items.Insert(0, new ListEditItem("Cartera Vigente", "1"));
            cbxCartera.Items.Insert(1, new ListEditItem("Cartera Ejecutada", "2"));
            cbxCartera.Items.Insert(2, new ListEditItem("Cartera Caduca", "3"));
            cbxCartera.Items.Insert(3, new ListEditItem("Cartera Sin Movimientos", "4"));
            cbxCartera.Items.Insert(4, new ListEditItem("Cartera Morosa", "5"));
            cbxCartera.Items.Insert(5, new ListEditItem("Cartera Subrogada", "6"));
            cbxCartera.SelectedIndex = 0;
        }
        private void CargarAcreedores()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();
            dt = Ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");
            util.CargaDDLDxx(cbxAcreedor, dt, "Nombre", "IdAcreedor");
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

        private void CargarGvCartera()
        {
            try
            {
                LogicaNegocio LN = new LogicaNegocio();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                string tipoCartera = string.IsNullOrEmpty(cbxCartera.Text.Trim()) ? "" : cbxCartera.Text.Trim();
                string ejecutivo = string.IsNullOrEmpty(cbxEjecutivo.Text.Trim()) ? "" : cbxEjecutivo.Text.Trim();
                string acreedor = string.IsNullOrEmpty(cbxAcreedor.Text.Trim()) ? "" : cbxAcreedor.Text.Trim();
                string empresa = string.IsNullOrEmpty(txtEmpresa.Text.Trim()) ? "" : txtEmpresa.Text.Trim();

                string estadoCF = string.Empty;
                int IdOpcion = 1;

                if (Page.Session["dtCarteraEjecutivo"] == null)
                {
                    dt = LN.ListarCarteraEjecutivoEstado(tipoCartera, ejecutivo, acreedor, empresa, estadoCF, IdOpcion);
                    Page.Session["dtCarteraEjecutivo"] = dt;

                    //ds = LN.ListarCarteraEjecutivoEstadods(tipoCartera, ejecutivo, acreedor, empresa, estadoCF, IdOpcion);
                    //Page.Session["ds"] = ds;
                    //Page.Session["dtCarteraEjecutivo"] = ds.Tables[1];
                }

                //CargarGrilla2();
                gvCarteraEjecutivo.DataSource = (DataTable)Page.Session["dtCarteraEjecutivo"];
                gvCarteraEjecutivo.DataBind();
            }
            catch (Exception)
            { }
        }

        //private void CargarGrilla2()
        //{
        //    DataSet res = (DataSet)Page.Session["ds"];
        //    int con = 0;
        //    Page.Session["OPCIONESPERMISOS"] = res.Tables[0];
        //    if (res.Tables[1] != null)
        //    {
        //        for (int i = 0; i <= res.Tables[0].Rows.Count - 1; i++)
        //        {
        //            if (i + 1 <= res.Tables[0].Rows.Count - 1)
        //            {
        //                if (i < res.Tables[0].Rows.Count - 2 && res.Tables[0].Rows[i]["Descripcion"].ToString() == res.Tables[0].Rows[i + 1]["Descripcion"].ToString())
        //                {
        //                    con++;
        //                    res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
        //                }
        //                else
        //                {
        //                    if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
        //                    {
        //                        con++;
        //                        res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
        //                    }
        //                    else
        //                    {
        //                        if (res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
        //                        {
        //                            con++;
        //                            res.Tables[0].Rows[i]["nroColum"] = res.Tables[1].Columns.Count;
        //                        }
        //                    }

        //                    if (con > 0)
        //                    {
        //                        res.Tables[1].Columns.Add(res.Tables[0].Rows[i]["Descripcion"].ToString().Trim(), typeof(string));
        //                        con = 0;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (i <= res.Tables[0].Rows.Count - 1 && res.Tables[0].Rows[i - 1]["Descripcion"].ToString() == res.Tables[0].Rows[i]["Descripcion"].ToString())
        //                {
        //                    con++;
        //                    res.Tables[0].Rows[i]["nroColum"] = res.Tables[0].Rows[i - 1]["nroColum"];
        //                }
        //            }
        //        }

        //        if (res.Tables[1].Rows.Count > 0)
        //        {
        //            ASPxGridView1.DataSource = res.Tables[1];
        //            ASPxGridView1.DataBind();
        //        }
        //        else
        //        {

        //        }
        //    }
        //}

        protected void gvCarteraEjecutivo_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtCarteraEjecutivo"] != null)
            {
                gvCarteraEjecutivo.DataSource = (DataTable)Page.Session["dtCarteraEjecutivo"];
                gvCarteraEjecutivo.DataBind();
            }
        }

        protected void gvCarteraEjecutivo_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
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

        protected void gvCarteraEjecutivo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Page.Session["dtCarteraEjecutivo"] = null;
            string[] rowValueItems = e.Parameters.Split(',');

            string tipoCartera = rowValueItems[0];
            string ejecutivo = rowValueItems[2];
            string acreedor = rowValueItems[1];
            string empresa = rowValueItems[3];

            CargarGvCartera();
        }

        protected void gvCarteraEjecutivo_DataBound(object sender, EventArgs e)
        {
            string tipoCartera = string.IsNullOrEmpty(cbxCartera.Text.Trim()) ? "" : cbxCartera.Text.Trim();
            switch (tipoCartera.Trim())
            {
                case "Cartera Vigente":
                    gvCarteraEjecutivo.Columns["Acciones"].Visible = true;
                    gvCarteraEjecutivo.Columns["Dias Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Tramo"].Visible = false;
                    gvCarteraEjecutivo.Columns["Capital Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Interes Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Monto En Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Cuotas Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Saldo Certificado"].Visible = true;

                    gvCarteraEjecutivo.Columns["Cuotas Pagadas"].Visible = true;
                    gvCarteraEjecutivo.Columns["Cuotas Pactadas"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fecha 1er Vencimiento"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fecha Emisión"].Visible = true;
                    gvCarteraEjecutivo.Columns["Acreedor"].Visible = true;
                    gvCarteraEjecutivo.Columns["Ultimo Compromiso"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fec Ultima Gestión"].Visible = true;

                    break;
                case "Cartera Ejecutada":
                    gvCarteraEjecutivo.Columns["Acciones"].Visible = false;
                    gvCarteraEjecutivo.Columns["Dias Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Tramo"].Visible = false;
                    gvCarteraEjecutivo.Columns["Capital Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Interes Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Monto En Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Cuotas Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Saldo Certificado"].Visible = true;
                    gvCarteraEjecutivo.Columns["Ultimo Compromiso"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fec Ultima Gestión"].Visible = true;
                    break;
                case "Cartera Caduca":
                    gvCarteraEjecutivo.Columns["Acciones"].Visible = false;
                    break;
                case "Cartera Sin Movimientos":
                    gvCarteraEjecutivo.Columns["Acciones"].Visible = true;
                    gvCarteraEjecutivo.Columns["Dias Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Tramo"].Visible = false;
                    gvCarteraEjecutivo.Columns["Capital Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Interes Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Monto En Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Cuotas Mora"].Visible = false;
                    gvCarteraEjecutivo.Columns["Saldo Certificado"].Visible = false;
                    gvCarteraEjecutivo.Columns["Cuotas Pagadas"].Visible = false;
                    gvCarteraEjecutivo.Columns["Cuotas Pactadas"].Visible = false;
                    gvCarteraEjecutivo.Columns["Fecha 1er Vencimiento"].Visible = false;
                    gvCarteraEjecutivo.Columns["Fecha Emisión"].Visible = false;
                    gvCarteraEjecutivo.Columns["Acreedor"].Visible = false;
                    gvCarteraEjecutivo.Columns["Ultimo Compromiso"].Visible = false;
                    gvCarteraEjecutivo.Columns["Fec Ultima Gestión"].Visible = false;

                    break;
                case "Cartera Morosa":
                    gvCarteraEjecutivo.Columns["Acciones"].Visible = true;
                    gvCarteraEjecutivo.Columns["Dias Mora"].Visible = true;
                    gvCarteraEjecutivo.Columns["Tramo"].Visible = true;
                    gvCarteraEjecutivo.Columns["Capital Mora"].Visible = true;
                    gvCarteraEjecutivo.Columns["Interes Mora"].Visible = true;
                    gvCarteraEjecutivo.Columns["Monto En Mora"].Visible = true;
                    gvCarteraEjecutivo.Columns["Cuotas Mora"].Visible = true;
                    gvCarteraEjecutivo.Columns["Saldo Certificado"].Visible = true;

                    gvCarteraEjecutivo.Columns["Cuotas Pagadas"].Visible = true;
                    gvCarteraEjecutivo.Columns["Cuotas Pactadas"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fecha 1er Vencimiento"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fecha Emisión"].Visible = true;
                    gvCarteraEjecutivo.Columns["Acreedor"].Visible = true;

                    gvCarteraEjecutivo.Columns["Ultimo Compromiso"].Visible = true;
                    gvCarteraEjecutivo.Columns["Fec Ultima Gestión"].Visible = true;
                    break;
                case "Cartera Subrogada":
                    break;
                default:
                    break;
            }
        }

        protected void cpGuardar_Callback(object source, CallbackEventArgs e)
        {

        }

        protected void gvSms_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["dtSms"] != null)
            {
                gvSms.DataSource = (DataTable)Page.Session["dtSms"];
                gvSms.DataBind();
            }
        }

        private void cargarSms(string Ncertificado)
        {
            LogicaNegocio LN = new LogicaNegocio();
            DataTable dt = LN.ListarSms(Ncertificado);
            Page.Session["dtSms"] = dt;
            gvSms.DataSource = dt;
            gvSms.DataBind();
        }

        protected void CpPcComentario_Callback(object sender, CallbackEventArgsBase e)
        {
            CargarBitacoraComentario(int.Parse(e.Parameter));
        }

        private void CargarBitacoraComentario(int idOperacion)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            var dt = Ln.ListarGestionBitacoraCobranza(0, idOperacion, null, null, null, null, null, null, 1);
            Page.Session["dtGestionBitacora"] = dt;
            gvBitacoraGestion.DataSource = dt;
            gvBitacoraGestion.DataBind();
        }

        protected void CpPcSms_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                cargarSms(e.Parameter);
        }

        protected void gvBitacoraGestion_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "NCertificado")
            {
                var grid = e.Column.Grid;
                if (!grid.IsNewRowEditing)
                {
                    e.Editor.ReadOnly = true;
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
                Page.Session["dtCarteraEjecutivo"] = null;
                CargarGvCartera();
            }
        }

        protected void gvCarteraEjecutivo_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Resumen resumenP = new Resumen();

            ASPxGridView gvBitacora = sender as ASPxGridView;

            int index = e.VisibleIndex;
            string Ncertificado = gvBitacora.GetRowValues(index, "NCertificado").ToString();

            resumenP.idOperacion = int.Parse(gvBitacora.GetRowValues(index, "IdOperacion").ToString());
            resumenP.idEmpresa = int.Parse(gvBitacora.GetRowValues(index, "IdEmpresa").ToString());
            resumenP.area = gvBitacora.GetRowValues(index, "DescArea").ToString();
            resumenP.linkActual = pagina;
            resumenP.linkPrincial = pagina;
            //resumenP.linkError = "Mensaje.aspx";

            SPWeb app2 = SPContext.Current.Web;
            resumenP.idUsuario = util.ObtenerValor(app2.CurrentUser.Name);

            Page.Session["RESUMEN"] = resumenP;

            if (e.ButtonID == "calendario")
                ASPxWebControl.RedirectOnCallback(string.Format("CalendarioPago.aspx?Ncertificado={0}", Ncertificado));
        }

        protected void ASPxGridView1_DataBound(object sender, EventArgs e)
        {

        }

        protected void ASPxGridView1_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //GridViewCommandColumn column = new GridViewCommandColumn();
            //column.VisibleIndex = 0;

            //for (int i = 0; i < 2; i++)
            //{
            //    GridViewCommandColumnCustomButton btn = new GridViewCommandColumnCustomButton();
            //    btn.ID = "customBtn" + i.ToString();
            //    btn.Text = "Custom Button " + i.ToString();
            //    btn.Visibility = GridViewCustomButtonVisibility.AllDataRows;
            //    column.CustomButtons.Add(btn);
            //}

            //ASPxGridView1.Columns.Add(column);
            //ASPxGridView1.DataBind();

            //GridViewCommandColumn column = new GridViewCommandColumn();
            //column.VisibleIndex = 0;

            //DataTable dt = new DataTable();
            //dt = (DataTable)Page.Session["OPCIONESPERMISOS"];

            //if (e.RowType == GridViewRowType.Data)
            //{
            //    GridViewCommandColumnCustomButton btn = new GridViewCommandColumnCustomButton();

            //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //    {
            //        btn = new GridViewCommandColumnCustomButton(); 
            //        btn.ID = "customBtn" + i.ToString();
            //        btn.Styles.Style.CssClass = dt.Rows[i]["Imagen"].ToString();
            //        btn.Text = string.Empty;
            //        //btn.Visibility = GridViewCustomButtonVisibility.AllDataRows;
            //        //btn.Text = dt.Rows[i]["ToolTip"].ToString();
            //        column.CustomButtons.Add(btn);
            //    }

            //    ASPxGridView1.Columns.Add(column);
            //    ASPxGridView1.DataBind();

            //}

            //int Val = 0, pos = 0;

            //if (e.RowType == GridViewRowType.Data)
            //{
            //    LinkButton[] lbmenu = new LinkButton[dt.Rows.Count];
            //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //    {
            //        lbmenu[i] = new LinkButton();
            //        lbmenu[i].CommandName = dt.Rows[i]["Opcion"].ToString();
            //        lbmenu[i].CssClass = (dt.Rows[i]["Imagen"].ToString());
            //        //lbmenu[i].CommandArgument = pos.ToString();
            //        //lbmenu[i].OnClientClick = "return Dialogo();";
            //        //lbmenu[i].Command += ResultadosBusqueda_Command1;

            //        lbmenu[i].Attributes.Add("cssclass", "text-custom");
            //        lbmenu[i].Attributes.Add("data-toggle", "tooltip");
            //        lbmenu[i].Attributes.Add("data-placement", "top");
            //        lbmenu[i].Attributes.Add("data-original-title", dt.Rows[i]["ToolTip"].ToString());

            //        //if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
            //        //{
            //        //    lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
            //        //    lbmenu[i].Visible = true;
            //        //}

            //        //if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[6].Text.ToString()) != "Negociacion" &&
            //        //  System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text.ToString()) != "Antecedente y Evaluación" &&
            //        //   dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
            //        //{
            //        //    lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
            //        //    lbmenu[i].Visible = true;
            //        //}

            //        //if (System.Web.HttpUtility.HtmlDecode(e.Row.Cells[7].Text.ToString()) == "Aprobación Fiscalía")
            //        //{
            //        //    if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
            //        //    {
            //        //        lbmenu[i].CssClass = (dt.Rows[i]["Imagen"].ToString());
            //        //        lbmenu[i].Visible = false;
            //        //    }
            //        //    if ((System.Web.HttpUtility.HtmlDecode((e.Row.Cells[15].Text.ToString())) == "2") && dt.Rows[i]["Opcion"].ToString() == "Negocio")
            //        //    { lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString()); Val++; }

            //        //    if (dt.Rows[i]["Opcion"].ToString() == "ENVIAR")
            //        //    {
            //        //        lbmenu[i].CssClass = (dt.Rows[i]["ImagenTrue"].ToString());
            //        //        lbmenu[i].Visible = true;
            //        //    }
            //        //}


            //        // opciones para operaciones y empresa
            //        try
            //        {
            //            if (int.Parse(dt.Rows[0]["nroColum"].ToString()) == int.Parse(dt.Rows[i]["nroColum"].ToString()))
            //            {
            //                e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]);
            //            }
            //            else
            //                e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]);


            //            //if (int.Parse(dt.Rows[i]["nroColum"].ToString()) > int.Parse(dt.Rows[0]["nroColum"].ToString()) && int.Parse(e.Row.Cells[0].Text.ToString()) > 0)
            //            //{
            //            //    e.Row.Cells[int.Parse(dt.Rows[i]["nroColum"].ToString())].Controls.Add(lbmenu[i]);
            //            //}
            //        }
            //        catch
            //        {
            //        }

            //    }
            //    pos++;
            //}

            ////for (int i = 0; i < e.Row.Cells.Count; i++)
            ////{
            ////    //celda Nro.Serie
            ////    if (i == 2)
            ////    {
            ////        e.Row.Cells[i].Attributes.Add("onmouseover", "OnCellMouseOver(this, event)");
            ////        e.Row.Cells[i].Attributes.Add("onmouseout", "OnCellMouseOut()");
            ////    }
            ////}
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            //GridViewCommandColumn column = new GridViewCommandColumn();
            //column.VisibleIndex = 0;

            //for (int i = 0; i < 2; i++)
            //{
            //    GridViewCommandColumnCustomButton btn = new GridViewCommandColumnCustomButton();
            //    btn.ID = "customBtn" + i.ToString();
            //    btn.Text = "Custom Button " + i.ToString();
            //    btn.Visibility = GridViewCustomButtonVisibility.AllDataRows;
            //    column.CustomButtons.Add(btn);
            //}

            //ASPxGridView1.Columns.Add(column);
            //ASPxGridView1.DataBind();
        }

        protected void ASPxGridView1_Init(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt = (DataTable)Page.Session["OPCIONESPERMISOS"];

            //GridViewCommandColumn column = new GridViewCommandColumn();
            //column.VisibleIndex = 0;

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    GridViewCommandColumnCustomButton btn = new GridViewCommandColumnCustomButton();
            //    btn.ID = "customBtn" + i.ToString();
            //    btn.Text = "Custom Button " + i.ToString();
            //    btn.Visibility = GridViewCustomButtonVisibility.AllDataRows;
            //    column.CustomButtons.Add(btn);
            //}

            //ASPxGridView1.Columns.Add(column);
            //ASPxGridView1.DataBind();
        }

    }
}
