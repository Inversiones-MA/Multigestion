using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using MultigestionUtilidades;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Xml.Linq;
using ExpertPdf.HtmlToPdf;
using System.Xml.Xsl;
using System.IO;
using System.Web.UI;
using Bd;
using ClasesNegocio;

namespace MultiRiesgo.wpDetalleCarteraMorosos.DetalleCarteraMorosos
{
    [ToolboxItemAttribute(false)]
    public partial class DetalleCarteraMorosos : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DetalleCarteraMorosos()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Page.Session["RESUMEN"] != null)
                {
                    objresumen = (Resumen)Page.Session["RESUMEN"];
                    CargarDetalle(objresumen);
                }
            }
        }

        void CargarDetalle(Resumen objresumen)
        {
            DataSet ds = new DataSet();
            LogicaNegocio Ln = new LogicaNegocio();

            ds = Ln.CargarDetalleCertificado(objresumen.idOperacion, objresumen.idEmpresa);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtEmpresa.Text = ds.Tables[0].Rows[0]["Empresa"].ToString();
                txtNroCertificado.Text = ds.Tables[0].Rows[0]["NCF"].ToString();
                txtFondo.Text = ds.Tables[0].Rows[0]["Fondo"].ToString();
                txtAcreedor.Text = ds.Tables[0].Rows[0]["Acreedor"].ToString();
                txtGiro.Text = ds.Tables[0].Rows[0]["Giro"].ToString(); ;
                txtDiasMora.Text = ds.Tables[0].Rows[0]["DiasMora"].ToString();
                txtTramo.Text = ds.Tables[0].Rows[0]["Tramo"].ToString();
                txtCapital.Text = ds.Tables[0].Rows[0]["CapitalMora"].ToString();
                txtIntereses.Text = ds.Tables[0].Rows[0]["InteresMora"].ToString();
                txtMontoMora.Text = ds.Tables[0].Rows[0]["MontoMora"].ToString();
                txtCuotasMora.Text = ds.Tables[0].Rows[0]["CuotasMora"].ToString();
                txtCuotasPagadas.Text = ds.Tables[0].Rows[0]["CuotasPagadas"].ToString();
                txtCuotasPactadas.Text = ds.Tables[0].Rows[0]["CuotasPactadas"].ToString();
                txtSaldoCertificado.Text = ds.Tables[0].Rows[0]["SaldoCertificado"].ToString();
                txtGarantia.Text = ds.Tables[0].Rows[0]["GarantiaVA"].ToString();
                txtFogape.Text = ds.Tables[0].Rows[0]["Fogape"].ToString();
                txtCobertura.Text = ds.Tables[0].Rows[0]["Cobertura"].ToString();
                txtDeudaCF.Text = ds.Tables[0].Rows[0]["DeudaCartera"].ToString();
             
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                gridContactos.DataSource = ds.Tables[1];
                gridContactos.DataBind();
            }
        }

        protected void gridContactos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridContactos.PageIndex = e.NewPageIndex;
            //LogicaNegocio Ln = new LogicaNegocio();
            //DataSet res;
            //res = Ln.buscarDatosEmpresa(Page.Session["IdEmpresa"].ToString()); gridContactos.DataSource = res.Tables[5];

            //gridContactos.DataSource = res.Tables[5];
            //gridContactos.DataBind();
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("BitacoraCobranza.aspx");
        }

    }
}
