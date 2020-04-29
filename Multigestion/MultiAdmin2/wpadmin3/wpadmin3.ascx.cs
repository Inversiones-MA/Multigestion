using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Dsp;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using DevExpress.Web;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Configuration;
using MultigestionUtilidades;
using Bd;
using ClasesNegocio;


namespace MultiAdmin2.wpadmin3
{
    [ToolboxItemAttribute(false)]
    public partial class wpadmin3 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpadmin3()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        public readonly string pagina = "MantenedorPlantilla.aspx";


        #region Eventos

        protected void Page_PreInit(object sender, EventArgs e)
        {
            ASPxWebControl.SetIECompatibilityModeEdge();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilidades util = new Utilidades();
            //PERMISOS USUARIOS
            DataTable dt = new DataTable("dt");
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = "",
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsCallback)
                {
                    Page.Session["IdValue"] = null;
                    CargarTipoProducto();
                    CargarAcreedores();
                    CargarArea();
                    CargarDatosEmpresa();
                    CargarDatosEmpresaPersonas();
                    CargarDatosOperacion();
                    CargarDatosAcreedor();
                    CargarDatosAvales();
                    CargarDatosGarantias();
                    CargarDatosOtros();
                    CargarDatosFiscalia();
                    CargarDatosRepresentantes();
                    CargarDocumentos(new LnPlantilla());
                }
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void CmbxTipoDocumentoGenerica_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                CargarTipoDocumento(e.Parameter);
        }


        protected void ASPxCallback1_Callback(object source, CallbackEventArgs e)
        {
            string html = string.Empty;
            //int i = 0;
            string[] Valores = new string[2];
            string retorno = string.Empty;
            switch (e.Parameter)
            {
                case "buscarGenerica":
                //retorno = BuscarPlantillaGenerica();
                //if (retorno != null)
                //{
                //    //foreach (string s in Valores)
                //    //{
                //    //    html = s.Trim() + "~" + html;
                //    //}
                //    e.Result = retorno;
                //}
                //else
                //    e.Result = "";
                //break;
                case "buscarPersonalizada":
                    //retorno = BuscarPlantillaPersonalizada();
                    //if (retorno != null)
                    //{
                    //    e.Result = retorno;
                    //}
                    //else
                    //    e.Result = "";
                    break;
                case "buscarPersonalizadaBuscar":
                    LnPlantilla plantilla = new LnPlantilla();
                    plantilla.IdAcreedor = Convert.ToInt32(CmbxAcreedorBuscar.Value);
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoBuscar.Value);
                    plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoBuscar.Value);
                    CargarDocumentos(plantilla);
                    break;
                case "guardar":
                    e.Result = GuardarPlantilla();
                    break;
            }
        }

        protected void CmbxTipoDocumentoBuscar_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
                CargarTipoDocumento(e.Parameter);
        }


        protected void GvBuscarPlantilla_PageIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GvBuscarPlantilla.DataSource = Page.Session["GvBuscarPlantilla"];
                GvBuscarPlantilla.DataBind();
            }
            catch
            {
            }
        }

        protected void GvBuscarPlantilla_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            string a = e.ButtonID;
            int index = e.VisibleIndex;
            int IdPlantilla = (int)GvBuscarPlantilla.GetRowValues(index, "IdPlantillaDocumento");
            switch (e.ButtonID)
            {
                case "Editar":
                    Page.Session["IdValue"] = IdPlantilla.ToString();
                    string datosPlantilla = String.Empty;
                    datosPlantilla = CargarPlantilla(IdPlantilla);
                    if (!string.IsNullOrEmpty(datosPlantilla))
                        GvBuscarPlantilla.JSProperties["cpValores"] = "Editar" + "~" + datosPlantilla;
                    else
                        GvBuscarPlantilla.JSProperties["cpValores"] = "Error";

                    break;
                case "Descargar":
                    DescargarPlantilla(IdPlantilla);
                    break;
                case "Eliminar":
                    Boolean eliminar = EliminarPlantilla(IdPlantilla);
                    if (eliminar)
                        GvBuscarPlantilla.JSProperties["cpValores"] = "eliminar";
                    else
                        GvBuscarPlantilla.JSProperties["cpValores"] = "Error";
                    break;
            }
        }

        protected void GvBuscarPlantilla_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string nombre = e.Parameters;
                if (nombre == "cargar")
                {
                    LnPlantilla plantilla = new LnPlantilla();
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoBuscar.Value);
                    plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoBuscar.Value);
                    plantilla.IdAcreedor = Convert.ToInt32(CmbxAcreedorBuscar.Value);
                    plantilla.NombrePlantilla = TxtNombrePlantillaBuscar.Text.Trim();
                    CargarDocumentos(plantilla);
                }
            }
            catch
            {
            }
        }

        #endregion


        #region Metodos

        private void CargarTipoProducto()
        {
            Utilidades util = new Utilidades();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtProductos = Ln.ListarProducto(2);
            util.CargaDDLDxx2(CmbxTipoProductoGenerica, dtProductos, "NombreProducto", "IdProducto");
            util.CargaDDLDxx2(CmbxTipoProductoBuscar, dtProductos, "NombreProducto", "IdProducto");

            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;
            //SPList items = app.Lists["Productos"];
            //SPQuery query = new SPQuery();
            //query.Query = "<Where>" +
            //                "<Or>" +
            //                   "<Eq><FieldRef Name='TipoProducto'/><Value Type='Number'>1</Value></Eq>" +
            //                   "<Eq><FieldRef Name='TipoProducto'/><Value Type='Number'>2</Value></Eq>" +
            //                 "</Or>" +
            //              "</Where>" +
            //              "<OrderBy>" + "<FieldRef Name='Nombre' Ascending='True' />" + "</OrderBy>";


            //query.Query = "<OrderBy><FieldRef Name='Nombre' Ascending='True' /></OrderBy>";
            //SPListItemCollection collListItems = items.GetItems(query);

            //CmbxTipoProductoGenerica.DataSource = collListItems.GetDataTable();
            //CmbxTipoProductoGenerica.TextField = "Title";
            //CmbxTipoProductoGenerica.ValueField = "ID";
            //CmbxTipoProductoGenerica.DataBind();

            //CmbxTipoProductoBuscar.DataSource = collListItems.GetDataTable();
            //CmbxTipoProductoBuscar.TextField = "Title";
            //CmbxTipoProductoBuscar.ValueField = "ID";
            //CmbxTipoProductoBuscar.DataBind();
        }

        private void CargarTipoDocumento(string IdTipoProducto)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            //int IdTipoProducto = 3;
            //string CFT = string.Empty;
            DataTable dtTipoDoc = Ln.ListarTipoDocumentos(null, Convert.ToInt32(IdTipoProducto));
            
            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;
            //SPList items = app.Lists["TipoDocumento"];
            //SPQuery query = new SPQuery();

            //query.Query = "<Where>" +
            //    "<And>" +
            //    "<And>" +
            //    "<Or>" +
            //        "<And>" +
            //            "<And>" +
            //                "<Eq><FieldRef Name='Area'/><Value Type='Lookup'>Operaciones</Value></Eq>" +
            //                "<Eq><FieldRef Name='Habilitado'/><Value Type='Boolean'>1</Value></Eq>" +
            //            "</And>" +
            //                "<Eq><FieldRef Name='IdTipoProducto'/><Value Type='Number'>" + tipoProducto + "</Value></Eq>" +
            //        "</And>" +
            //          "<Eq><FieldRef Name='IdTipoProducto'/><Value Type='Number'>" + IdTipoProducto + "</Value></Eq>" +
            //    "</Or>" +
            //    "<Neq><FieldRef Name='Nombre'/><Value Type='Text'>Instrucción de Curse</Value></Neq>" +
            //    "</And>" +
            //    "<Neq><FieldRef Name='Nombre'/><Value Type='Text'>Solicitud de Emisión</Value></Neq>" +
            //    "</And>" +
            //    "</Where>";

            //SPListItemCollection collListItems = items.GetItems(query);

            CmbxTipoDocumentoGenerica.DataSource = dtTipoDoc; //collListItems.GetDataTable();
            CmbxTipoDocumentoGenerica.TextField = "Nombre";
            CmbxTipoDocumentoGenerica.ValueField = "IdTipoDocumento";
            CmbxTipoDocumentoGenerica.DataBind();

            CmbxTipoDocumentoBuscar.DataSource = dtTipoDoc; //collListItems.GetDataTable();
            CmbxTipoDocumentoBuscar.TextField = "Nombre";
            CmbxTipoDocumentoBuscar.ValueField = "IdTipoDocumento";
            CmbxTipoDocumentoBuscar.DataBind();

            //foreach (ListEditItem item in CmbxTipoDocumentoBuscar.Items)
            //{
            //    if (item.Text == "Solicitud de Emisión")
            //        CmbxTipoDocumentoBuscar.Items.Remove(item);
            //    if (item.Text == "Instruccion Curse")
            //        CmbxTipoDocumentoBuscar.Items.Remove(item);
            //}
        }

        private void CargarArea()
        {
            Utilidades util = new Utilidades();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtAreas = Ln.ListarAreas(null);
            util.CargaDDLDx(cbxArea, dtAreas, "Nombre", "Nombre");
        }

        private void CargarAcreedores()
        {
            Utilidades util = new Utilidades();
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dtAcreedores = Ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");
            util.CargaDDLDx(CmbxAcreedorGenerica, dtAcreedores, "Nombre", "IdAcreedor");
            util.CargaDDLDx(CmbxAcreedorBuscar, dtAcreedores, "Nombre", "IdAcreedor");
        }

        private void CargarDatosEmpresa()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Fecha_Contrato}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Razon_social}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Rut}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Direccion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{Calle}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Complemento_Direccion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 7;
            row["Descripcion"] = "{Numero}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Region_Empresa}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 9;
            row["Descripcion"] = "{Comuna_Empresa}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 10;
            row["Descripcion"] = "{Tipo_Empresa}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{Objeto_Sociedad_SII}";
            dt.Rows.Add(row);

            LstDatosEmpresaGenerica.DataSource = dt;
            LstDatosEmpresaGenerica.ValueField = "Codigo";
            LstDatosEmpresaGenerica.TextField = "Descripcion";
            LstDatosEmpresaGenerica.DataBind();
        }

        private void CargarDatosEmpresaPersonas()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Total_Representantes}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{Nombres_Representantes}";
            dt.Rows.Add(row);

            LstDatosEmpresaPersonas.DataSource = dt;
            LstDatosEmpresaPersonas.ValueField = "Codigo";
            LstDatosEmpresaPersonas.TextField = "Descripcion";
            LstDatosEmpresaPersonas.DataBind();
        }

        private void CargarDatosOperacion()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Acreedor}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Banco_Factoring}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Cobertura_Fogape}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Cobertura_Afianzamiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Cobertura_Certificado}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{Comision_MultiAval}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Finalidad}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Fogape_Estado}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 7;
            row["Descripcion"] = "{Fondo}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Fecha_Emision_Larga}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 9;
            row["Descripcion"] = "{Fecha_Emision_Corta}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 10;
            row["Descripcion"] = "{Fecha_Emision_Cursada}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{Fecha_Primer_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 12;
            row["Descripcion"] = "{Fecha_Ultimo_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 13;
            row["Descripcion"] = "{Fondo_Garantia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 15;
            row["Descripcion"] = "{Glosa}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 16;
            row["Descripcion"] = "{Monto_Operacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 17;
            row["Descripcion"] = "{Monto_Operacion_CLP}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 18;
            row["Descripcion"] = "{Moneda_Descripcion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 19;
            row["Descripcion"] = "{Moneda_Signo}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 20;
            row["Descripcion"] = "{Numero_Cuotas}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 21;
            row["Descripcion"] = "{Numero_Certificado}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 22;
            row["Descripcion"] = "{Numero_Operacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 23;
            row["Descripcion"] = "{Numero_Pagare}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 24;
            row["Descripcion"] = "{Plazo}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 25;
            row["Descripcion"] = "{Plazo_Dias}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 26;
            row["Descripcion"] = "{Periodo_Gracia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 27;
            row["Descripcion"] = "{Periodicidad}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 28;
            row["Descripcion"] = "{Tipo_Operacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 29;
            row["Descripcion"] = "{Tipo_Amortizacion}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 30;
            row["Descripcion"] = "{Tipo_Cuota}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 31;
            row["Descripcion"] = "{Tasa_Interes}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 32;
            row["Descripcion"] = "{Tipo_Moneda}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 33;
            row["Descripcion"] = "{Valor_Cuotas}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 34;
            row["Descripcion"] = "{Representante_Legal_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 35;
            row["Descripcion"] = "{Rut_Representante_Legal_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 36;
            row["Descripcion"] = "{Representante_Fondo_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 37;
            row["Descripcion"] = "{Rut_Representante_Fondo_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 38;
            row["Descripcion"] = "{Representante_Legal_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 39;
            row["Descripcion"] = "{Rut_Representante_Legal_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 40;
            row["Descripcion"] = "{Representante_Fondo_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 41;
            row["Descripcion"] = "{Rut_Representante_Fondo_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 42;
            row["Descripcion"] = "{Gastos_Legales}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 43;
            row["Descripcion"] = "{Impuesto_Timbre_Estampilla}";
            dt.Rows.Add(row);

            LstDatosOperacionGenerica.DataSource = dt;
            LstDatosOperacionGenerica.ValueField = "Codigo";
            LstDatosOperacionGenerica.TextField = "Descripcion";
            LstDatosOperacionGenerica.DataBind();
        }

        private void CargarDatosAcreedor()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Rut_Acreedor}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Acreedor_Razon_Social}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Direccion_Acreedor}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Region}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{Comuna}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Provincia}";
            dt.Rows.Add(row);

            LstDatosAcreedorGenerica.DataSource = dt;
            LstDatosAcreedorGenerica.ValueField = "Codigo";
            LstDatosAcreedorGenerica.TextField = "Descripcion";
            LstDatosAcreedorGenerica.DataBind();
        }

        private void CargarDatosGarantias()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Datos_Garantias}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Garantias_Fiscalia}";
            dt.Rows.Add(row);

            LstDatosGarantiaGenerica.DataSource = dt;
            LstDatosGarantiaGenerica.ValueField = "Codigo";
            LstDatosGarantiaGenerica.TextField = "Descripcion";
            LstDatosGarantiaGenerica.DataBind();
        }

        private void CargarDatosAvales()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Avales_DatosPersonales_Pagare}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Avales_Direccion_Pagare}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Nombres_Avales}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Avales_DatosPersonales_CGR}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 9;
            row["Descripcion"] = "{Avales_Direccion_CGR}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 10;
            row["Descripcion"] = "{Datos_Avales}";
            dt.Rows.Add(row);

            LstDatosAvalesGenerica.DataSource = dt;
            LstDatosAvalesGenerica.ValueField = "Codigo";
            LstDatosAvalesGenerica.TextField = "Descripcion";
            LstDatosAvalesGenerica.DataBind();
        }

        private void CargarDatosFiscalia()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 0;
            row["Descripcion"] = "{Monto_Operacion_Palabra}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 0;
            row["Descripcion"] = "{Dia_Primer_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Mes_Primer_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Año_Primer_Vencimiento}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Representante_Legal_Empresa}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{Parrafo_Capital_Intereses}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Finalidad_Fiscalia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 7;
            row["Descripcion"] = "{Impuesto_Pagare_13_meses}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Impuesto_Pagare_12_meses}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 9;
            row["Descripcion"] = "{Impuesto_Pagare_Sin_Plazo}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 10;
            row["Descripcion"] = "{Total_Impuesto_Timbre}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{Firmas_Representantes}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 12;
            row["Descripcion"] = "{Firmas_Avales}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{Firmas_Representantes_Rut}";
            dt.Rows.Add(row);

            LstDatosFiscalia.DataSource = dt;
            LstDatosFiscalia.ValueField = "Codigo";
            LstDatosFiscalia.TextField = "Descripcion";
            LstDatosFiscalia.DataBind();
        }

        private void CargarDatosRepresentantes()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Nombre_Representante_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Nacionalidad_Representante_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{EstadoCivil_Representante_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{profesion_Representante_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{RutTexto_Representante_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{RutNumero_Representante_1}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 7;
            row["Descripcion"] = "{Nombre_Representante_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Nacionalidad_Representante_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 9;
            row["Descripcion"] = "{EstadoCivil_Representante_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 10;
            row["Descripcion"] = "{profesion_Representante_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 11;
            row["Descripcion"] = "{RutTexto_Representante_2}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 12;
            row["Descripcion"] = "{RutNumero_Representante_2}";
            dt.Rows.Add(row);

            LstDatosRepresentantes.DataSource = dt;
            LstDatosRepresentantes.ValueField = "Codigo";
            LstDatosRepresentantes.TextField = "Descripcion";
            LstDatosRepresentantes.DataBind();
        }

        private void CargarDatosOtros()
        {
            DataTable dt = new DataTable("dt");
            dt.Columns.Add("Codigo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            DataRow row = dt.NewRow();

            row = dt.NewRow();
            row["Codigo"] = 0;
            row["Descripcion"] = "{Dia}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 0;
            row["Descripcion"] = "{Mes}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 0;
            row["Descripcion"] = "{Año}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 1;
            row["Descripcion"] = "{Fecha_Hoy}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 2;
            row["Descripcion"] = "{Domicilio S.A.G.R.}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 3;
            row["Descripcion"] = "{Rut S.A.G.R.}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 4;
            row["Descripcion"] = "{Nombre S.A.G.R.}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 5;
            row["Descripcion"] = "{Representante_Legal_S.A.G.R}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 6;
            row["Descripcion"] = "{Rut_Representante_Legal_S.A.G.R}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 7;
            row["Descripcion"] = "{Monto_Propuesto_Paf}";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Codigo"] = 8;
            row["Descripcion"] = "{Rut_S.A.G.R._Palabras}";
            dt.Rows.Add(row);

            LstOtrosGenerica.DataSource = dt;
            LstOtrosGenerica.ValueField = "Codigo";
            LstOtrosGenerica.TextField = "Descripcion";
            LstOtrosGenerica.DataBind();
        }

        public string GuardarPlantilla()
        {
            Utilidades util = new Utilidades();
            LnPlantilla plantilla = new LnPlantilla();
            try
            {
                Resumen resumenP = new Resumen();
                SPWeb app2 = SPContext.Current.Web;
                resumenP.idUsuario = ""; // util.ObtenerValor(app2.CurrentUser.Name);

                if (ChkGenerica.Checked)
                {
                    int? Id = null;
                    if (HdfC["cambio"].ToString() == "no" || Page.Session["IdValue"] == null)
                        plantilla.IdPlantillaDocumento = null;
                    else
                        plantilla.IdPlantillaDocumento = Convert.ToInt32(Page.Session["IdValue"]);

                    plantilla.NombrePlantilla = TxtNombrePlantillaGenerica.Text;

                    if (HtmlEditorGenerica.Html.Contains("../UploadFiles/Images/"))
                        HtmlEditorGenerica.Html = HtmlEditorGenerica.Html.Replace("../UploadFiles/Images/", "/UploadFiles/Images/");

                    plantilla.ContenidoHtml = HtmlEditorGenerica.Html; //HtmlEditorGenerica.Html.Replace("/UploadFiles/Images/", @"C:\inetpub\wwwroot\wss\VirtualDirectories\" + ConfigurationManager.AppSettings["PuertoSitio"].ToString() + @"\UploadFiles\Images\");

                    plantilla.EsGenerica = ChkGenerica.Checked;
                    plantilla.UsuarioModificacion = resumenP.idUsuario;
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoGenerica.Value);
                    if (CmbxTipoProductoGenerica.Text.Trim() == "Documento Interno")
                    {
                        plantilla.IdDocumentoTipo = 0;
                        plantilla.Area = cbxArea.Text.Trim();
                    }
                    else
                    {
                        plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoGenerica.Value);
                        plantilla.Area = "Operaciones";
                    }

                    plantilla.IdAcreedor = 0; //Convert.ToInt32(CmbxAcreedorGenerica.Value);

                    if (plantilla.IdPlantillaDocumento == null)
                    {
                        Id = plantilla.InsertarPlantilla(plantilla);
                        if (Id == null || Id == 0)
                            return "error";
                        else
                            Page.Session["IdValue"] = Id;
                    }
                    else
                    {
                        Id = plantilla.ActualizarPlantilla(plantilla);
                        if (Id == null || Id == 0)
                            return "error";
                        else
                            Page.Session["IdValue"] = Id;
                    }
                }
                else
                {
                    int? Id = null;
                    if (HdfC["cambio"].ToString() == "no" || Page.Session["IdValue"] == null)
                        plantilla.IdPlantillaDocumento = null;
                    else
                        plantilla.IdPlantillaDocumento = Convert.ToInt32(Page.Session["IdValue"]);


                    plantilla.NombrePlantilla = TxtNombrePlantillaGenerica.Text;

                    if (HtmlEditorGenerica.Html.Contains("../UploadFiles/Images/"))
                        HtmlEditorGenerica.Html = HtmlEditorGenerica.Html.Replace("../UploadFiles/Images/", "/UploadFiles/Images/");

                    plantilla.ContenidoHtml = HtmlEditorGenerica.Html; //.Replace("/UploadFiles/Images/", "C:/inetpub/wwwroot/wss/VirtualDirectories/" + ConfigurationManager.AppSettings["PuertoSitio"].ToString() + "/UploadFiles/Images/");

                    plantilla.EsGenerica = ChkGenerica.Checked;
                    plantilla.UsuarioModificacion = resumenP.idUsuario;
                    plantilla.IdTipoProducto = Convert.ToInt32(CmbxTipoProductoGenerica.Value);
                    if (CmbxTipoProductoGenerica.Text.Trim() == "Documento Interno")
                    {
                        plantilla.IdDocumentoTipo = 0;
                        plantilla.Area = cbxArea.Text.Trim();
                    }
                    else
                    {
                        plantilla.IdDocumentoTipo = Convert.ToInt32(CmbxTipoDocumentoGenerica.Value);
                        plantilla.Area = "Operaciones";
                    }

                    plantilla.IdAcreedor = Convert.ToInt32(CmbxAcreedorGenerica.Value);
                    if (plantilla.IdPlantillaDocumento == null)
                    {
                        Id = plantilla.InsertarPlantilla(plantilla);
                        if (Id == null || Id == 0)
                            return "error";
                        else
                            Page.Session["IdValue"] = Id;
                    }
                    else
                    {
                        Id = plantilla.ActualizarPlantilla(plantilla);
                        if (Id == null || Id == 0)
                            return "error";
                        else
                            Page.Session["IdValue"] = Id;
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString() + "~" + plantilla.IdPlantillaDocumento.ToString() + "~" + Convert.ToString(CmbxTipoProductoGenerica.Value) + "~" + Convert.ToString(CmbxTipoDocumentoGenerica.Value) + "~" + Convert.ToString(CmbxAcreedorGenerica.Value) + "~" + TxtNombrePlantillaGenerica.Text.Trim() + "~" + plantilla.ContenidoHtml + "~" + plantilla.EsGenerica.ToString() + "~" + plantilla.UsuarioModificacion;
            }
        }


        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        public Boolean EliminarPlantilla(int IdPlantilla)
        {
            Utilidades util = new Utilidades();
            LnPlantilla plantilla = new LnPlantilla();
            SPWeb app2 = SPContext.Current.Web;
            string Usuario = util.ObtenerValor(app2.CurrentUser.Name);
            return plantilla.EliminarPlantilla(IdPlantilla, Usuario);
            //return true;
        }

        public string CargarPlantilla(int IdPlantilla)
        {
            string datos = string.Empty;
            string html = string.Empty;
            LnPlantilla plantilla = new LnPlantilla();
            DataTable dt = new DataTable("dt");
            dt = plantilla.ListarPlantillaById(IdPlantilla);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ContenidoHtml"].ToString().Contains("../UploadFiles/Images/"))
                    html = dt.Rows[0]["ContenidoHtml"].ToString().Replace("../UploadFiles/Images/", "/UploadFiles/Images/");
                else
                    html = dt.Rows[0]["ContenidoHtml"].ToString();

                datos = html + "~" + dt.Rows[0]["IdAcreedor"] + "~" + dt.Rows[0]["IdDocumentoTipo"] + "~" + dt.Rows[0]["NombrePlantilla"] + "~" + Convert.ToBoolean(dt.Rows[0]["EsGenerica"]) + "~" + dt.Rows[0]["IdTipoProducto"] + "~" + dt.Rows[0]["Nombre"] + "~" + dt.Rows[0]["Area"];
                return datos;
            }
            else
                return null;
        }

        private void DescargarPlantilla(int IdPlantilla)
        {
            Utilidades util = new Utilidades();
            LnPlantilla plantilla = new LnPlantilla();
            DataTable dt = new DataTable("dt");
            dt = plantilla.ListarPlantillaById(IdPlantilla);

            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0]["ContenidoHtml"].ToString()))
                {
                    string contenido = HttpUtility.HtmlDecode(dt.Rows[0]["ContenidoHtml"].ToString());
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<table width='1000px'><tr><td align='center'><table width='900px'><tr><td>" + contenido + "</td></tr></table></td></tr></table>");
                    byte[] archivo = util.ConvertirAPDF_Control(sb);
                    Page.Session["Titulo"] = dt.Rows[0]["NombrePlantilla"].ToString();

                    if (archivo != null)
                    {
                        Page.Session["tipoDoc"] = "pdf";
                        Page.Session["binaryData"] = archivo;
                        Page.Session["Titulo"] = dt.Rows[0]["NombrePlantilla"].ToString().Trim();
                        ASPxWebControl.RedirectOnCallback("BajarDocumento.aspx");
                    }
                }
            }
        }

        private void CargarDocumentos(LnPlantilla plantilla)
        {
            DataTable dt = new DataTable("dt");
            dt = plantilla.ListarTodasPlantillas(plantilla);
            Page.Session["GvBuscarPlantilla"] = dt;
            GvBuscarPlantilla.DataSource = dt;
            GvBuscarPlantilla.DataBind();
        }

        #endregion


        //protected void BtnLimpiar_Click(object sender, EventArgs e)
        //{
        //    //using (MemoryStream stream = new MemoryStream())
        //    //{
        //    //    ExporToStream(HtmlEditor.Html, stream);

        //    //    HttpContext.Current.Response.Clear();

        //    //    HttpContext.Current.Response.Buffer = false;
        //    //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    //    HttpContext.Current.Response.AppendHeader("Content-Transfer-Encoding", "binary");
        //    //    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=document.pdf");
        //    //    HttpContext.Current.Response.BinaryWrite(stream.ToArray());
        //    //    HttpContext.Current.Response.End();
        //    //}
        //}

        //void ExporToStream(string html, Stream stream)
        //{
        //    using (RichEditDocumentServer server = new RichEditDocumentServer())
        //    {
        //        server.HtmlText = html;

        //        string text = "<b>SOME SAMPLE TEXT</b>";
        //        AddHeaderToDocument(server.Document, text);
        //        AddFooterToDocument(server.Document, text);

        //        using (PrintingSystemBase ps = new PrintingSystemBase())
        //        {
        //            using (PrintableComponentLinkBase pcl = new PrintableComponentLinkBase(ps))
        //            {
        //                pcl.Component = server;
        //                pcl.CreateDocument();
        //                ps.ExportToPdf(stream);
        //            }
        //        }
        //    }
        //}

        //void AddHeaderToDocument(DevExpress.XtraRichEdit.API.Native.Document document, string htmlText)
        //{
        //    SubDocument doc = document.Sections[0].BeginUpdateHeader();
        //    doc.AppendHtmlText(htmlText);
        //    document.Sections[0].EndUpdateHeader(doc);
        //}

        //void AddFooterToDocument(DevExpress.XtraRichEdit.API.Native.Document document, string htmlText)
        //{
        //    SubDocument doc = document.Sections[0].BeginUpdateFooter();
        //    doc.AppendHtmlText(htmlText);
        //    document.Sections[0].EndUpdateFooter(doc);
        //}

    }
}
