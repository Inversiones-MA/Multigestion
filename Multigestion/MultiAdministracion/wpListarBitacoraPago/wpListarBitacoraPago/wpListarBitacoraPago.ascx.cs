using System.Data;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using FrameworkIntercapIT.Utilities;
using Bd;

namespace MultiAdministracion.wpListarBitacoraPago.wpListarBitacoraPago
{
    [ToolboxItemAttribute(false)]
    public partial class wpListarBitacoraPago : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpListarBitacoraPago()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        public static string USER;
        public static string CARGO;
        public static string AREA;

        public static string ESTADO;
        public static string SUBETAPA;
        public static string ETAPA;
        public static DataTable OPCIONESPERMISOS;

        public static int pageSize = 100;
        public static int pageNro = 0;
        Utilidades util = new Utilidades();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //ocultarDiv();
            if (!Page.IsPostBack)
            {
                LogicaNegocio Ln = new LogicaNegocio();
                SPWeb app2 = SPContext.Current.Web;

                string Cargos;
                USER = app2.CurrentUser.Name;
                Cargos = Ln.Buscar_Usuario(app2.CurrentUser.Name);
                CARGO = util.ObtenerValor(Cargos);

                if (CARGO.ToString() == "Administrador Consulta")
                    CARGO = "Contabilidad";
                if (CARGO.ToString() == "Administrador")
                    CARGO = "Contabilidad";

                AREA = "Operaciones";
                USER = util.ObtenerValor(app2.CurrentUser.Name);

                Boolean ban = false;
                if (CARGO.ToString() == "Contabilidad")
                {
                    ban = true;
                }

                if (Cargos != "" && ban == true)
                {
                    ESTADO = "";
                    SUBETAPA = "";
                    ETAPA = "";
                    OPCIONESPERMISOS = null;
                    CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), string.Empty, pageSize, pageNro);
                }
                else
                {
                    dvFormulario.Style.Add("display", "none");         
                }
            }
            else
            {
                CargarGrid(ETAPA.ToString(), SUBETAPA.ToString(), ESTADO.ToString(), txtRazonSocial.Text.ToString(), pageSize, pageNro);
            }
        }

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        protected void CargarGrid(string etapa, string subEtapa, string estado, string buscar, int pageS, int pageN)
        {
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataSet res;
                res = Ln.ListarBitacoraPago(etapa, subEtapa, estado, buscar, AREA.ToString(), CARGO.ToString(), USER.ToString(), pageS, pageN);
              
                if (res.Tables[0].Rows.Count > 0)
                {
                    ResultadosBusqueda.Visible = true;
                    ocultarDiv();
                    ResultadosBusqueda.DataSource = res.Tables[0];
                    ResultadosBusqueda.DataBind();
                }
                else
                {
                    ResultadosBusqueda.Visible = false;
                    ocultarDiv();
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = "No se encontraron registro para la busqueda selecionada, por favor probar con otros filtros";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void ResultadosBusqueda_DataBound(object sender, EventArgs e)
        {

        }

        protected void ResultadosBusqueda_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ResultadosBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void ResultadosBusqueda_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                e.Row.Cells[1].Visible = false;//empresa

                e.Row.Cells[0].Visible = false;
            }
        }

        protected void ResultadosBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ImageButton1_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

        }
    }
}
