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
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Web.UI;
using MultigestionUtilidades;
using System.Drawing;

namespace MultiOperacion.wpPosicionClienteV2.wpPosicionClienteV2
{
    [ToolboxItemAttribute(false)]
    public partial class wpPosicionClienteV2 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPosicionClienteV2()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        int i;
        int j;
        Utilidades util = new Utilidades();

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Session["IdEmpresa"] != null)
            {
                LogicaNegocio Ln = new LogicaNegocio();
                DataSet res;
                res = Ln.ConsultaPosicionCliente(Page.Session["IdEmpresa"].ToString());

                if (res != null)
                {
                    i = 0;
                    j = 0;
                    if (res.Tables[0].Rows.Count > 0)
                    {
                        lblRut.Text = res.Tables[0].Rows[0]["RUT"].ToString();
                        lblNombre.Text = res.Tables[0].Rows[0]["RazonSocial"].ToString();
                        lblNroEmpleados.Text = res.Tables[0].Rows[0]["NumEmpleados"].ToString();
                        lblTipoEmpresa.Text = res.Tables[0].Rows[0]["DescTipoEmpresa"].ToString();
                        lblFechaIniAct.Text = res.Tables[0].Rows[0]["FecInicioActEco"].ToString();
                        lblFechaIniContra.Text = res.Tables[0].Rows[0]["FecIniContrato"].ToString();
                        lblFechaFinContra.Text = res.Tables[0].Rows[0]["FecFinContrato"].ToString();
                        lblEjecutivo.Text = res.Tables[0].Rows[0]["DescEjecutivo"].ToString();
                        lblActividad.Text = res.Tables[0].Rows[0]["DescActividad"].ToString();
                        lblTelefono.Text = res.Tables[0].Rows[0]["TelFijo1"].ToString();
                        lblEmail.Text = res.Tables[0].Rows[0]["EMail"].ToString();
                        lblGrupoE.Text = res.Tables[0].Rows[0]["PerteneceGrupo"].ToString();
                        lblNombreGrupoE.Text = res.Tables[0].Rows[0]["descGrupoEconomico"].ToString();

                        lblBloqueo.Text = res.Tables[0].Rows[0]["Bloqueado"].ToString();
                        lblMotivoBloqueo.Text = res.Tables[0].Rows[0]["MotivoBloqueo"].ToString();

                        hdfidEmpresa.Value = res.Tables[0].Rows[0]["idEmpresa"].ToString();
                    }

                    if (res.Tables[1].Rows.Count > 0)
                    {
                        lblAnosExperiencia.Text = res.Tables[1].Rows[0]["AnosExperiencia"].ToString();
                        lblHistoria.Text = res.Tables[1].Rows[0]["Historia"].ToString();
                    }

                    gridSocioa.DataSource = res.Tables[2];
                    gridSocioa.DataBind();

                    gridRelacionadas.DataSource = res.Tables[3];
                    gridRelacionadas.DataBind();

                    gridDirectorios.DataSource = res.Tables[4];
                    gridDirectorios.DataBind();

                    gridContactos.DataSource = res.Tables[5];
                    gridContactos.DataBind();

                    gridDirecciones.DataSource = res.Tables[6];
                    gridDirecciones.DataBind();

                    gridCertificados.DataSource = res.Tables[7];
                    gridCertificados.DataBind();
                    SumarCertificados(res.Tables[7]);

                    gridGrupoEconomico.DataSource = res.Tables[12];
                    gridGrupoEconomico.DataBind();

                    gridGarantiaEmpresa.DataSource = res.Tables[13];
                    gridGarantiaEmpresa.DataBind();
                    SumarGarantias(res.Tables[13]);

                    if (res.Tables[11].Rows.Count > 0)
                    {
                        lblIdPAF.Text = res.Tables[11].Rows[0]["idpaf"].ToString();
                        lblAnalista.Text = res.Tables[11].Rows[0]["descEjecutivo"].ToString();
                        lblFechaAprobacion.Text = res.Tables[11].Rows[0]["FechaRevision"].ToString();
                        lblEdoLinea.Text = res.Tables[11].Rows[0]["EstadoLinea"].ToString();
                        lblNivelAribucion.Text = res.Tables[11].Rows[0]["NivelAtribucion"].ToString();
                        lblRanking.Text = res.Tables[11].Rows[0]["ValorRank"].ToString();
                        lblClasificacion.Text = res.Tables[11].Rows[0]["ValorPtje"].ToString();
                        lblFechaClasificacion.Text = res.Tables[11].Rows[0]["FecCreacion"].ToString();
                        lblVentas.Text = res.Tables[11].Rows[0]["VentasMoviles"].ToString();

                        lblMontoAprobado.Text = res.Tables[11].Rows[0]["MontoAprobado"].ToString();
                        lblMontoVigente.Text = res.Tables[11].Rows[0]["MontoVigente"].ToString();
                        lblMontoPropuesto.Text = res.Tables[11].Rows[0]["MontoPropuesto"].ToString();

                        //cobertuta global ultima paf
                        txtCoberturaComercial.Text = res.Tables[11].Rows[0]["coberturaComercial"].ToString();
                        txtCoberturaAjustado.Text = res.Tables[11].Rows[0]["coberturaAjustado"].ToString();

                        //cobertura global vigente
                        txtCoberturaComercialVigente.Text = res.Tables[11].Rows[0]["coberturaComercialVigente"].ToString();
                        txtCoberturaAjustadoVigente.Text = res.Tables[11].Rows[0]["coberturaAjustadoVigente"].ToString();

                    }

                    //validar alertas de garantias(contribuciones, fechas tasaciones)
                    ValidarAlertasGarantias();
                }
            }
        }

        protected void ValidarAlertasGarantias()
        {
            //validar alertas de garantias(contribuciones, fechas tasaciones)
            LogicaNegocio Ln = new LogicaNegocio();
            string mensaje = string.Empty;
            DataTable dtProblemas = new DataTable("dtProblemas");
            dtProblemas = Ln.ConsultaValidacionesEmpresa(Page.Session["IdEmpresa"].ToString(), 2);
            mensaje = dtProblemas.Rows[0][0].ToString();

            if (mensaje != "OK")
            {
                lbWarning1.Text = util.Mensaje(mensaje);
                dvWarning1.Style.Add("display", "block");
                PnlBloqueado.Enabled = false;   
            }
        }

        protected void gridCertificados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridCertificados.PageIndex = e.NewPageIndex;
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            res = MTO.ConsultaPosicionCliente(Page.Session["IdEmpresa"].ToString());
            gridCertificados.DataSource = res.Tables[6];
            gridCertificados.DataBind();
        }

        protected void gridRelacionadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridRelacionadas.PageIndex = e.NewPageIndex;
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            res = MTO.ConsultaPosicionCliente(Page.Session["IdEmpresa"].ToString());
            gridRelacionadas.DataSource = res.Tables[3];
            gridRelacionadas.DataBind();
        }

        protected void gridSocioa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridSocioa.PageIndex = e.NewPageIndex;
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            res = MTO.ConsultaPosicionCliente(Page.Session["IdEmpresa"].ToString());
            gridSocioa.DataSource = res.Tables[2];
            gridSocioa.DataBind();
        }

        protected void gridDirectorios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridDirectorios.PageIndex = e.NewPageIndex;
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            res = MTO.ConsultaPosicionCliente(Page.Session["IdEmpresa"].ToString());
            gridDirectorios.DataSource = res.Tables[4];
            gridDirectorios.DataBind();
        }

        protected void gridContactos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridContactos.PageIndex = e.NewPageIndex;
            LogicaNegocio MTO = new LogicaNegocio();
            DataSet res;
            res = MTO.ConsultaPosicionCliente(Page.Session["IdEmpresa"].ToString()); gridContactos.DataSource = res.Tables[5];

            gridContactos.DataSource = res.Tables[5];
            gridContactos.DataBind();
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            if (Page.Session["urlAnteior"] == null)
            {
                Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                Page.Response.Redirect("busquedaposicioncliente.aspx");
            }
            else
                Page.Response.Redirect(Page.Session["urlAnteior"].ToString());
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            imprimir();
        }

        protected void gridCertificados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex > -1)
                {
                    //link calendario pago
                    LinkButton lbk = new LinkButton();
                    lbk.CommandName = "Redireccion";
                    lbk.ToolTip = "Calendario Pago";
                    lbk.CssClass = "glyphicon glyphicon-calendar paddingIconos";
                    lbk.CommandArgument = i.ToString();
                    lbk.OnClientClick = "return Dialogo();";
                    lbk.Command += lb_gridCertificadosCommand;
                    e.Row.Cells[12].Controls.Add(lbk);

                    //link operacion
                    LinkButton lb = new LinkButton();
                    lb.CssClass = ("glyphicon glyphicon-search");
                    lb.CommandName = "IrOperacion";
                    lb.CommandArgument = i.ToString();
                    i++;
                    lb.OnClientClick = "return Dialogo();";
                    lb.Command += lb_gridCertificadosCommand; ;
                    e.Row.Cells[13].Text = "";
                    e.Row.Cells[13].Controls.Add(lb);
                }

                //e.Row.Cells[12].Visible = false;
                //e.Row.Cells[13].Visible = false;
                e.Row.Cells[14].Visible = false;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = false;
                e.Row.Cells[17].Visible = false; //NroCertificado

                if (e.Row.RowIndex > -1)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[7].Text = Convert.ToDecimal(e.Row.Cells[7].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                        e.Row.Cells[8].Text = Convert.ToDecimal(e.Row.Cells[8].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (!string.IsNullOrWhiteSpace(e.Row.Cells[9].Text) && !string.IsNullOrEmpty(e.Row.Cells[9].Text) && e.Row.Cells[9].Text != "&nbsp;")
                            e.Row.Cells[9].Text = Convert.ToDecimal(e.Row.Cells[9].Text).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (!string.IsNullOrWhiteSpace(e.Row.Cells[10].Text) && !string.IsNullOrEmpty(e.Row.Cells[10].Text) && e.Row.Cells[10].Text != "&nbsp;")
                            e.Row.Cells[10].Text = Convert.ToDecimal(e.Row.Cells[10].Text).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                        if (!string.IsNullOrWhiteSpace(e.Row.Cells[11].Text) && !string.IsNullOrEmpty(e.Row.Cells[11].Text) && e.Row.Cells[11].Text != "&nbsp;")
                            e.Row.Cells[11].Text = Convert.ToDecimal(e.Row.Cells[11].Text).ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                    }


                }
            }
            catch
            {
            }
        }

        protected void gridGarantiaEmpresa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex > -1)
                {
                    LinkButton lb = new LinkButton();
                    lb.CssClass = ("glyphicon glyphicon-search");
                    lb.CommandName = "IrGarantia";
                    lb.CommandArgument = j.ToString();
                    j++;
                    lb.OnClientClick = "return Dialogo();";
                    lb.Command += lb_gridGarantiaEmpresa;
                    e.Row.Cells[8].Text = "";
                    e.Row.Cells[8].Controls.Add(lb);
                }

                e.Row.Cells[9].Visible = false;

                if (e.Row.RowIndex > -1)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //Formato valor ajustado
                        e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                        //formato valor Comercial
                        e.Row.Cells[5].Text = Convert.ToDecimal(e.Row.Cells[5].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));

                        //formato valor asegurable
                        if (!string.IsNullOrWhiteSpace(e.Row.Cells[6].Text) && !string.IsNullOrEmpty(e.Row.Cells[6].Text))
                            e.Row.Cells[6].Text = Convert.ToDecimal(e.Row.Cells[6].Text).ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                }
            }
            catch
            {
            }
        }

        private void lb_gridCertificadosCommand(object sender, CommandEventArgs e)
        {
            int index = int.Parse(e.CommandArgument.ToString());

            SPWeb app2 = SPContext.Current.Web;
            Resumen objResumen = new Resumen();
            objResumen.idOperacion = int.Parse(System.Web.HttpUtility.HtmlDecode(gridCertificados.Rows[index].Cells[0].Text.ToString()));
            objResumen.idEmpresa = int.Parse(hdfidEmpresa.Value.ToString());
            objResumen.desOperacion = System.Web.HttpUtility.HtmlDecode(gridCertificados.Rows[index].Cells[2].Text.ToString());
            objResumen.idPermiso = Constantes.PERMISOS.SOLOLECTURA.ToString();
            objResumen.idUsuario = app2.CurrentUser.Name;
            objResumen.descEjecutivo = lblEjecutivo.Text;
            objResumen.desEtapa = System.Web.HttpUtility.HtmlDecode(gridCertificados.Rows[index].Cells[14].Text.ToString());
            objResumen.area = "Posicion";
            objResumen.desEmpresa = lblNombre.Text;
            objResumen.rut = lblRut.Text;
            objResumen.desSubEtapa = System.Web.HttpUtility.HtmlDecode(gridCertificados.Rows[index].Cells[15].Text.ToString());
            objResumen.linkActual = "PosicionCliente.aspx";
            objResumen.linkPrincial = "PosicionCliente.aspx";
            Page.Session["RESUMEN"] = objResumen;

            if (e.CommandName == "IrOperacion")
            {
                Page.Response.Redirect("DatosOperacion.aspx");
            }
            if (e.CommandName == "Redireccion")
            {
                string NroCer = gridCertificados.Rows[index].Cells[17].Text.ToString();
                Page.Response.Redirect("CalendarioPago.aspx?NCertificado=" + gridCertificados.Rows[index].Cells[17].Text.ToString());
            }
        }

        private void lb_gridGarantiaEmpresa(object sender, CommandEventArgs e)
        {
            int index = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName == "IrGarantia")
            {
                SPWeb app2 = SPContext.Current.Web;
                Resumen objResumen = new Resumen();
                //string idGarantia = System.Web.HttpUtility.HtmlDecode(gridGarantiaEmpresa.Rows[index].Cells[9].Text.ToString());
                objResumen.idPermiso = Constantes.PERMISOS.SOLOLECTURA.ToString();
                objResumen.idEmpresa = int.Parse(hdfidEmpresa.Value.ToString());
                objResumen.idUsuario = app2.CurrentUser.Name;
                objResumen.descCargo = "";
                objResumen.desEmpresa = lblNombre.Text;
                objResumen.rut = lblRut.Text;
                objResumen.area = "Posicion";
                objResumen.idOperacion = -1;
                objResumen.desOperacion = "No aplica";
                objResumen.descEjecutivo = lblEjecutivo.Text;
                objResumen.linkActual = "PosicionCliente.aspx";
                Page.Session["RESUMEN"] = objResumen;
                Page.Response.Redirect("MantenedorGarantias.aspx");
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            imprimir();
        }

        protected void gridGarantias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gridGarantias.PageIndex = e.NewPageIndex;
            //LogicaNegocio MTO = new LogicaNegocio();
            //DataSet res;
            //res = MTO.buscarDatosEmpresa(Page.Session["IdEmpresa"].ToString()); gridContactos.DataSource = res.Tables[5];

            //gridGarantias.DataSource = res.Tables[7];
            //gridGarantias.DataBind();
        }

        #endregion


        #region Metodos

        private void SumarCertificados(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                // suma de columna  
                decimal ComisionClp = dt.AsEnumerable().Sum(row => row.Field<decimal>("Comisión CLP"));
                decimal Seguro = dt.AsEnumerable().Sum(row => row.Field<decimal>("Seguro CLP"));
                double GastosOperacionales = dt.AsEnumerable().Sum(row => row.Field<double>("Gastos Operacionales CLP"));
                double MontoOperacion = dt.AsEnumerable().Sum(row => row.Field<double>("Monto Operación UF"));
                decimal SaldoVigente = dt.AsEnumerable().Sum(row => row.Field<decimal>("Saldo Vigente UF"));

                gridCertificados.FooterRow.Cells[6].Text = "Total";
                gridCertificados.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                gridCertificados.FooterRow.Cells[6].ForeColor = Color.Black;
                gridCertificados.FooterRow.Cells[6].Font.Bold = true;

                gridCertificados.FooterRow.Cells[7].Text = ComisionClp.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                gridCertificados.FooterRow.Cells[7].ForeColor = Color.Black;
                gridCertificados.FooterRow.Cells[7].Font.Bold = true;
                gridCertificados.FooterRow.Cells[8].Text = Seguro.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                gridCertificados.FooterRow.Cells[8].ForeColor = Color.Black;
                gridCertificados.FooterRow.Cells[8].Font.Bold = true;
                gridCertificados.FooterRow.Cells[9].Text = GastosOperacionales.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"));
                gridCertificados.FooterRow.Cells[9].ForeColor = Color.Black;
                gridCertificados.FooterRow.Cells[9].Font.Bold = true;
                gridCertificados.FooterRow.Cells[10].Text = MontoOperacion.ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                gridCertificados.FooterRow.Cells[10].ForeColor = Color.Black;
                gridCertificados.FooterRow.Cells[10].Font.Bold = true;
                gridCertificados.FooterRow.Cells[11].Text = SaldoVigente.ToString("N3", CultureInfo.CreateSpecificCulture("es-ES"));
                gridCertificados.FooterRow.Cells[11].ForeColor = Color.Black;
                gridCertificados.FooterRow.Cells[11].Font.Bold = true;
            }
        }

        private void SumarGarantias(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                // suma de columna
                decimal totalValorAjustado = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Ajustable UF"));
                decimal totalValorComercial = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Comercial UF"));
                decimal totalValorsegurable = dt.AsEnumerable().Sum(row => row.Field<decimal>("Valor Asegurable UF"));

                gridGarantiaEmpresa.FooterRow.Cells[3].Text = "Total";
                gridGarantiaEmpresa.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                gridGarantiaEmpresa.FooterRow.Cells[3].ForeColor = Color.Black;
                gridGarantiaEmpresa.FooterRow.Cells[3].Font.Bold = true; //= Font.Bold;

                gridGarantiaEmpresa.FooterRow.Cells[4].Text = totalValorAjustado.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                gridGarantiaEmpresa.FooterRow.Cells[4].ForeColor = Color.Black;
                gridGarantiaEmpresa.FooterRow.Cells[4].Font.Bold = true;
                gridGarantiaEmpresa.FooterRow.Cells[5].Text = totalValorComercial.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                gridGarantiaEmpresa.FooterRow.Cells[5].ForeColor = Color.Black;
                gridGarantiaEmpresa.FooterRow.Cells[5].Font.Bold = true;
                gridGarantiaEmpresa.FooterRow.Cells[6].Text = totalValorsegurable.ToString("N4", CultureInfo.CreateSpecificCulture("es-ES"));
                gridGarantiaEmpresa.FooterRow.Cells[6].ForeColor = Color.Black;
                gridGarantiaEmpresa.FooterRow.Cells[6].Font.Bold = true;
            }
        }

        private void imprimir()
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            LogicaNegocio MTO = new LogicaNegocio();
            Dictionary<string, string> datos = new Dictionary<string, string>();
            datos["IdEmpresa"] = Page.Session["IdEmpresa"].ToString();

            string Reporte = "";
            Reporte = "Ficha_Comercial";

            byte[] archivo = GenerarReporte(Reporte, datos, "");
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
            else
            {
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = string.Format("Revise los siguientes datos: {0}{1}{0}{2}","<br />", " Tabla Direcciones (debe existir al menos una direccion principal)", " Tabla Personas (debe existir al menos un contacto principal)");
                //lbWarning.Text = string.Format("Revise los siguientes datos: {0}{1}", System.Environment.NewLine, " Tabla Direcciones (debe existir al menos una direccion principal)", " Tabla Personas (debe existir al menos un contacto principal)");
            }
        }



        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            try
            {
                String xml = String.Empty;

                //IF PARA CADA REPORTE
                DataSet res1 = new DataSet();
                if (sp == "Ficha_Comercial")
                {
                    res1 = MTO.ConsultaFichaComercial(datos["IdEmpresa"], "ReportePosicionCliente");
                    for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                    {
                        xml = xml + res1.Tables[0].Rows[i][0].ToString();
                    }
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");

                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        #endregion
 
    }
}
