using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using MultigestionUtilidades;
using System.Data;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using Bd;
using ClasesNegocio;

namespace MultiContabilidad.wpPagar.wpPagar
{
    [ToolboxItemAttribute(false)]
    public partial class wpPagar : WebPart
    {     
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpPagar()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        Utilidades util = new Utilidades();
        private static string pagina = "wpPagar.aspx";


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            LogicaNegocio LN = new LogicaNegocio();

            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dt2 = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = pagina;
            validar.Etapa = objresumen.area;

            dt2 = permiso.ListarPerfil(validar);
            if (dt2.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    //DataTable tabla = null;
                    //tabla = LN.ConsultarOperacion(int.Parse(objresumen.idOperacion.ToString()));
                    if (Page.Session["RESUMEN"] != null)
                    {
                        ViewState["BUSQUEDA"] = Page.Session["BUSQUEDA"];
                        Page.Session["BUSQUEDA"] = null;

                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;

                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbOperacion.Text = objresumen.desOperacion.ToString();

                        ocultarDiv();
                        string descTipoTransaccion = System.Web.HttpUtility.HtmlDecode(Page.Request.QueryString["idTT"] as string);

                        idTransaccion.Text = descTipoTransaccion;
                        lbTitulo.Text = descTipoTransaccion;
                        pnDatos.Visible = false;
                        pnDatosAsesoria.Visible = false;
                        pnDatosDevolucion.Visible = false;

                        CargarAcreedor();

                        DataSet dt = new DataSet();
                        dt = LN.ConsultaDatosContabilidad(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), descTipoTransaccion);

                        if (dt.Tables[1].Rows.Count > 0)
                        {
                            txtRanzonSocial.Text = dt.Tables[1].Rows[0]["RazonSocial"].ToString();
                            txtRut.Text = dt.Tables[1].Rows[0]["Rut"].ToString();
                            txtActEconomica.Text = dt.Tables[1].Rows[0]["DescActividad"].ToString();
                            txtDireccion.Text = dt.Tables[1].Rows[0]["direccion"].ToString();
                            txtComuna.Text = dt.Tables[1].Rows[0]["DescComuna"].ToString();
                            txtProvincia.Text = dt.Tables[1].Rows[0]["DescProvincia"].ToString();
                            txtRegion.Text = dt.Tables[1].Rows[0]["DescRegion"].ToString();
                            txtTelefonof.Text = dt.Tables[1].Rows[0]["TelFijo1"].ToString();
                            txtEmail.Text = dt.Tables[1].Rows[0]["EMail"].ToString();
                        }

                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            if (descTipoTransaccion != "1" && descTipoTransaccion != "3")
                            {
                                pnDatos.Visible = true;
                                txtCertificado.Text = dt.Tables[0].Rows[0]["NCertificado"].ToString();
                                txtMonto.Text = dt.Tables[0].Rows[0]["MontoOperacion"].ToString();
                                txtFechaEmision.Text = dt.Tables[0].Rows[0]["fecEmision"].ToString();
                                txtComisionCLp.Text = dt.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtProducto.Text = dt.Tables[0].Rows[0]["descProducto"].ToString();
                                txtGastosOperacionales.Text = dt.Tables[0].Rows[0]["gastosOperacionales"].ToString();
                                txtAcreedor.Text = dt.Tables[0].Rows[0]["Acreedor"].ToString();
                                txtSeguro.Text = dt.Tables[0].Rows[0]["costoSeguro"].ToString();
                                lbseguro.Text = dt.Tables[0].Rows[0]["incluido"].ToString();
                                lbcomision.Text = dt.Tables[0].Rows[0]["incluidoComision"].ToString();
                                lbgastosOpe.Text = dt.Tables[0].Rows[0]["incluidoGastosOperacionales"].ToString();
                                lbTitulo.Text = dt.Tables[0].Rows[0]["descTipoTransaccion"].ToString();

                                txtSeguroDesgravamen.Text = dt.Tables[0].Rows[0]["costoSeguroDesgravamen"].ToString();
                                lbSeguroDesgravamen.Text = dt.Tables[0].Rows[0]["incluidoDesgravamen"].ToString();

                                txtTimbreyEstAcreedor.Text = dt.Tables[0].Rows[0]["TimbreYEstampillaAcreedor"].ToString();
                                lbTimbreyEstAcreedor.Text = dt.Tables[0].Rows[0]["incluidoTimbreYEstampillaAcreedor"].ToString();

                                txtTimbreyEstMultiaval.Text = dt.Tables[0].Rows[0]["TimbreYEstampilla"].ToString();
                                lbTimbreyEstMultiaval.Text = dt.Tables[0].Rows[0]["incluidoTimbreYEstampilla"].ToString();

                                txtNotario.Text = dt.Tables[0].Rows[0]["Notario"].ToString();
                                lbNotario.Text = dt.Tables[0].Rows[0]["incluidoNotario"].ToString();
                            }

                            if (descTipoTransaccion == "3")
                            {
                                pnDatosDevolucion.Visible = true;
                                txtCertificadoD.Text = dt.Tables[0].Rows[0]["NCertificado"].ToString();
                                txtMontoD.Text = dt.Tables[0].Rows[0]["MontoOperacion"].ToString();
                                txtFechaEmisionD.Text = dt.Tables[0].Rows[0]["fecEmision"].ToString();
                                txtComisionCLPD.Text = dt.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtProductoD.Text = dt.Tables[0].Rows[0]["descProducto"].ToString();

                                txtGastosOperacionales.Text = dt.Tables[0].Rows[0]["gastosOperacionales"].ToString();
                                txtAcreedorD.Text = dt.Tables[0].Rows[0]["Acreedor"].ToString();
                                txtFondoD.Text = dt.Tables[0].Rows[0]["fondo"].ToString();
                                txtCostoFondoD.Text = dt.Tables[0].Rows[0]["costoFondo"].ToString();
                                txtNroFacturaComisión.Text = dt.Tables[0].Rows[0]["NroFactura"].ToString();
                                txtFechaFacturaComision.Text = dt.Tables[0].Rows[0]["FechaFactura"].ToString();
                                txtTipoContrato.Text = dt.Tables[0].Rows[0]["tipoContrato"].ToString();
                                txtDevolucionFinal.Text = dt.Tables[0].Rows[0]["devolucionFinal"].ToString();
                                txtDevolucionCostoFondo.Text = dt.Tables[0].Rows[0]["devolucionCostoFondo"].ToString();
                                txtGatosOperacionalesP.Text = dt.Tables[0].Rows[0]["GastosOperacionalPendiente"].ToString();
                                lbTitulo.Text = dt.Tables[0].Rows[0]["descTipoTransaccion"].ToString();
                            }

                            if (descTipoTransaccion == "1")
                            {
                                pnDatosAsesoria.Visible = true;
                                txtComisionA.Text = dt.Tables[0].Rows[0]["comisionCLP"].ToString();
                                txtProductoA.Text = dt.Tables[0].Rows[0]["descProducto"].ToString();
                                txtFechaEstimada.Text = dt.Tables[0].Rows[0]["fecEstimadaCierre"].ToString();
                                lbTitulo.Text = dt.Tables[0].Rows[0]["descTipoTransaccion"].ToString();
                            }
                        }

                        DataTable dt1 = new DataTable();
                        dt1 = LN.ConsultaFacturacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(), objresumen.idUsuario.ToString(), objresumen.descCargo.ToString(), descTipoTransaccion);

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["fecFactura"].ToString() != "")
                            {
                                txt_Fecha.Text = dt1.Rows[0]["fecFactura"].ToString();
                            }

                            txt_Factura.Text = dt1.Rows[0]["numFactura"].ToString();

                            if (dt1.Rows[0]["FechaPago"].ToString() != "")
                                dctFechaPago.SelectedDate = Convert.ToDateTime(dt1.Rows[0]["FechaPago"].ToString());

                            if (!string.IsNullOrEmpty(dt1.Rows[0]["IdBanco"].ToString()))
                                ddlbanco.SelectedIndex = ddlbanco.Items.IndexOf(ddlbanco.Items.FindByValue(Convert.ToString(dt1.Rows[0]["IdBanco"].ToString())));

                            if (dt1.Rows[0]["IdTipoPago"].ToString() != "")
                                ddlTipoPago.SelectedIndex = ddlTipoPago.Items.IndexOf(ddlTipoPago.Items.FindByValue(Convert.ToString(dt1.Rows[0]["IdTipoPago"].ToString())));

                            txtDocumentoPago.Text = dt1.Rows[0]["NroDocumentoPago"].ToString();
                            txt_Comentario.Text = dt1.Rows[0]["Comentario"].ToString();
                        }
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                }

                Control divFormulario = this.FindControl("dvFormulario");
                bool TieneFiltro = true;

                if (divFormulario != null)
                    util.bloquear(divFormulario, dt2.Rows[0]["Permiso"].ToString(), TieneFiltro);
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
                        

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txt_Comentario.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio lcGlobal = new LogicaNegocio();
            lcGlobal.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "04");

            LogicaNegocio MTO = new LogicaNegocio();
            if (txt_Factura.Text.ToString() != "")
            {

                if (MTO.InsertarActualizarPagoFacturacion(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                           objresumen.idUsuario, objresumen.descCargo, txt_Comentario.Text, txtDocumentoPago.Text, ddlbanco.SelectedValue, ddlbanco.SelectedItem.ToString(),
                            (dctFechaPago.SelectedDate.Day.ToString() + "-" + dctFechaPago.SelectedDate.Month.ToString() + "-" + dctFechaPago.SelectedDate.Year.ToString()),
                            ddlTipoPago.SelectedValue, ddlTipoPago.SelectedItem.ToString(), idTransaccion.Text
                           )
                )
                    if (MTO.InsertarActualizarEstadoPagar(objresumen.idEmpresa.ToString(), objresumen.idOperacion.ToString(),
                                    objresumen.idUsuario, objresumen.descCargo, "8", "Cierre", "26", "Pagado", "5", "Cursado", "Operaciones", txt_Comentario.Text, idTransaccion.Text))
                    {
                        MTO.ActualizarEstado(objresumen.idOperacion.ToString(), "8", "Cierre", "26", "Pagado", "5", "Cursado", "Operaciones");
                        ocultarDiv();
                        dvSuccess.Style.Add("display", "block");
                        lbSuccess.Text = MTO.buscarMensaje(Constantes.MENSAJE.EXITOINSERT);
                        Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
                        Page.Response.Redirect("ListarContabilidad.aspx");
                    }
            }
            else
            {
                ocultarDiv();
                dvWarning.Style.Add("display", "block");
                lbWarning.Text = ("Datos Incompletos");
            }
        }

        protected void lbGenerar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("wpContabilidad.aspx" + "?idTT=" + idTransaccion.Text);
        }

        protected void lbPagar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("wpPagar.aspx" + "?idTT=" + idTransaccion.Text);
        }

        #endregion


        #region Metodos

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("MensajeSession.aspx");
        }

        private void CargarAcreedor()
        {
            LogicaNegocio Ln = new LogicaNegocio();
            DataTable dt = new DataTable();

            dt = Ln.CRUDAcreedores(5, "", 0, "", "", 0, 0, 0, 0, "");
            util.CargaDDL(ddlbanco, dt, "Nombre", "IdAcreedor");

            //SPWeb app = SPContext.Current.Web;
            //app.AllowUnsafeUpdates = true;

            ////Banco
            //SPList items = app.Lists["Acreedores"];
            //SPQuery query = new SPQuery();
            //query.Query = "<Where><Eq><FieldRef Name='TipoAcreedor'/><Value Type='Text'>Banco</Value></Eq></Where>";//<OrderBy>  <FieldRef Name = 'Nombre' Ascending = 'TRUE'/> </OrderBy>
            //SPListItemCollection collListItems = items.GetItems(query);
            //util.CargaDDL(ddlbanco, collListItems.GetDataTable(), "Nombre", "ID");
        }


        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
        }

        #endregion

    }
}
