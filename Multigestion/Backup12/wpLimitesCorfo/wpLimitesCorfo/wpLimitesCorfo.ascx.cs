using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using Microsoft.SharePoint;
using System.Data;
using System.Web.UI;
using DevExpress.Web;
using System.Globalization;
using System.Drawing;
using ClasesNegocio;
using Bd;

namespace MultiOperacion.wpLimitesCorfo.wpLimitesCorfo
{
    [ToolboxItemAttribute(false)]
    public partial class wpLimitesCorfo : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpLimitesCorfo()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        private static string pagina = "LimitesCorfo.aspx";
        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-CL");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-CL");

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

                    Page.Session["RESUMEN"] = null;

                }

                if (!Page.IsCallback)
                    CargarData();

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
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void CargarData()
        {
            LogicaNegocio ln = new LogicaNegocio();
            DataSet ds = new DataSet();

            ds = ln.LimitesCorfo(1);
            if (ds != null)
            {
                txtTotalAlfaopv.Text = ds.Tables[0].Rows[0]["NumeroOperacionesVigentes"].ToString();
                txtTotalRecoopv.Text = ds.Tables[0].Rows[1]["NumeroOperacionesVigentes"].ToString();
                txtTotalVictoriaopv.Text = ds.Tables[0].Rows[2]["NumeroOperacionesVigentes"].ToString();
                txtTotalBetaopv.Text = ds.Tables[0].Rows[3]["NumeroOperacionesVigentes"].ToString();
                txtTotal.Text = (int.Parse(txtTotalAlfaopv.Text) + int.Parse(txtTotalRecoopv.Text) + int.Parse(txtTotalVictoriaopv.Text) + int.Parse(txtTotalBetaopv.Text)).ToString();

                txtTotalAlfaDeudas.Text = ds.Tables[0].Rows[0]["NumeroClientesConDeudas"].ToString();
                txtTotalRecoDeudas.Text = ds.Tables[0].Rows[1]["NumeroClientesConDeudas"].ToString();
                txtTotalVictoriaDeudas.Text = ds.Tables[0].Rows[2]["NumeroClientesConDeudas"].ToString();
                txtTotalBetaDeudas.Text = ds.Tables[0].Rows[3]["NumeroClientesConDeudas"].ToString();
                txtTotalDeudas.Text = (int.Parse(txtTotalAlfaDeudas.Text) + int.Parse(txtTotalRecoDeudas.Text) + int.Parse(txtTotalVictoriaDeudas.Text) + int.Parse(txtTotalBetaDeudas.Text)).ToString();

                txtGaratias40Alfa.Text = ds.Tables[0].Rows[0]["EmpresasConGarantia40"].ToString();
                txtGarantias40Reco.Text = ds.Tables[0].Rows[1]["EmpresasConGarantia40"].ToString();
                txtGarantias40Victoria.Text = ds.Tables[0].Rows[2]["EmpresasConGarantia40"].ToString();
                txtGarantias40Beta.Text = ds.Tables[0].Rows[3]["EmpresasConGarantia40"].ToString();
                txtGarantias40total.Text = (int.Parse(txtGaratias40Alfa.Text) + int.Parse(txtGarantias40Reco.Text) + int.Parse(txtGarantias40Victoria.Text) + int.Parse(txtGarantias40Beta.Text)).ToString();

                txtSinGarantiaAlfa.Text = ds.Tables[0].Rows[0]["EmpresasSinGarantiaReal"].ToString();
                txtSinGarantiaReco.Text = ds.Tables[0].Rows[1]["EmpresasSinGarantiaReal"].ToString();
                txtSinGarantiaVictoria.Text = ds.Tables[0].Rows[2]["EmpresasSinGarantiaReal"].ToString();
                txtSinGarantiaBeta.Text = ds.Tables[0].Rows[3]["EmpresasSinGarantiaReal"].ToString();
                txtSinGarantiaTotal.Text = (int.Parse(txtSinGarantiaAlfa.Text) + int.Parse(txtSinGarantiaReco.Text) + int.Parse(txtSinGarantiaVictoria.Text) + int.Parse(txtSinGarantiaBeta.Text)).ToString();

                txtPorcSinGarantiaAlfa.Text = ds.Tables[0].Rows[0]["PorcentajeClientesSinGarantia"].ToString();
                txtPorcSinGarantiaReco.Text = ds.Tables[0].Rows[1]["PorcentajeClientesSinGarantia"].ToString();
                txtPorcSinGarantiaVictoria.Text = ds.Tables[0].Rows[2]["PorcentajeClientesSinGarantia"].ToString();
                txtPorcSinGarantiaBeta.Text = ds.Tables[0].Rows[3]["PorcentajeClientesSinGarantia"].ToString();
                txtPorcSinGarantiaTotal.Text = (((float.Parse(txtGarantias40total.Text) + float.Parse(txtSinGarantiaTotal.Text)) / float.Parse(txtTotalDeudas.Text)) * 100).ToString();

                txtPorcConGarantiaAlfa.Text = ds.Tables[0].Rows[0]["PorcentajeClientesConGarantia"].ToString();
                txtPorcConGarantiaReco.Text = ds.Tables[0].Rows[1]["PorcentajeClientesConGarantia"].ToString();
                txtPorcConGarantiaVictoria.Text = ds.Tables[0].Rows[2]["PorcentajeClientesConGarantia"].ToString();
                txtPorcConGarantiaBeta.Text = ds.Tables[0].Rows[3]["PorcentajeClientesConGarantia"].ToString();
                txtPorcConGarantiaTotal.Text = (100 - float.Parse(txtPorcSinGarantiaTotal.Text)).ToString();

                txtMaximoFianzaAlfa.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[0].Rows[0]["FianzaMaximaBeneficiarioUF"]), 0);
                txtMaximoFianzaReco.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[0].Rows[1]["FianzaMaximaBeneficiarioUF"]), 0);
                txtMaximoFianzaVictoria.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[0].Rows[2]["FianzaMaximaBeneficiarioUF"]), 0);
                txtMaximoFianzaBeta.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[0].Rows[3]["FianzaMaximaBeneficiarioUF"]), 0);

                txtMaximoFianzaVFAlfa.Text = ds.Tables[0].Rows[0]["PorcReglamentario"].ToString();
                txtMaximoFianzaVFReco.Text = ds.Tables[0].Rows[1]["PorcReglamentario"].ToString();
                txtMaximoFianzaVFVictoria.Text = ds.Tables[0].Rows[2]["PorcReglamentario"].ToString();
                txtMaximoFianzaVFBeta.Text = ds.Tables[0].Rows[3]["PorcReglamentario"].ToString();

                Page.Session["Multi1"] = ds.Tables[1];
                gvMulti1.DataSource = ds.Tables[1];
                gvMulti1.DataBind();

                Page.Session["Multi2"] = ds.Tables[2];
                gvMulti2.DataSource = ds.Tables[2];
                gvMulti2.DataBind();

                txtTotalFianzaVigenteAlfaDPL.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[3].Rows[0]["TotalFianzaVigenteAlfa"]), 0);
                txtTotalFianzaVigenteRecoDPL.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[4].Rows[0]["TotalFianzaVigenteReco"]), 0);
                txtTotalFianzaVigenteVictoriaDPL.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[5].Rows[0]["TotalFianzaVigenteVictoria"]), 0);
                txtTotalFianzaVigenteBetaDPL.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[6].Rows[0]["TotalFianzaVigenteBeta"]), 0);

                txtTotalFianzaVigenteAlfa.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[3].Rows[0]["TotalFianzaVigenteAlfa"]), 0);
                txtTotalFianzaVigenteReco.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[4].Rows[0]["TotalFianzaVigenteReco"]), 0);
                txtTotalFianzaVigenteVictoria.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[5].Rows[0]["TotalFianzaVigenteVictoria"]), 0);
                txtTotalFianzaVigenteBeta.Text = util.formatearNumero(Convert.ToDouble(ds.Tables[6].Rows[0]["TotalFianzaVigenteBeta"]), 0);
            }
        }

        protected void gvMulti2_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["Multi2"] != null)
            {
                gvMulti2.DataSource = (DataTable)Page.Session["Multi2"];
                gvMulti2.DataBind();
            }
        }

        protected void gvMulti1_PageIndexChanged(object sender, EventArgs e)
        {
            if (Page.Session["Multi1"] != null)
            {
                gvMulti1.DataSource = (DataTable)Page.Session["Multi1"];
                gvMulti1.DataBind();
            }
        }

        protected void gvMulti1_HeaderFilterFillItems(object sender, DevExpress.Web.ASPxGridViewHeaderFilterEventArgs e)
        {

        }

        protected void gvMulti1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "PorcentajeLinea")
            {
                //var a = float.Parse(e.CellValue.ToString());
                if (float.Parse(e.CellValue.ToString()) >= 5)
                    e.Cell.BackColor = Color.Yellow;
            }
        }

        protected void gvMulti2_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "PorcentajeLinea")
            {
                if (float.Parse(e.CellValue.ToString()) >= 5)
                    e.Cell.BackColor = Color.Yellow;
            }
        }
    }
}
