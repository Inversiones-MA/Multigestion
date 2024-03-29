﻿using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using MultigestionUtilidades;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using ExpertPdf.HtmlToPdf;
using System.Web.UI;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using Microsoft.SharePoint;
using System.Web.UI.DataVisualization.Charting;
using ClasesNegocio;
using Bd;

namespace MultiRiesgo.wpIvaVenta.wpIvaVenta
{
    [ToolboxItemAttribute(false)]
    public partial class wpIvaVenta : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public wpIvaVenta()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        private static string pagina = "VaciadoVentas.aspx";

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            ocultarDiv();
            if (Page.Session["RESUMEN"] != null)
            {
                ViewState["RES"] = (Resumen)Page.Session["RESUMEN"];
            }
            asignacionResumen(ref objresumen);

            SPWeb app2 = SPContext.Current.Web;
            DataTable dt = new DataTable("dt");
            ValidarPermisos validar = new ValidarPermisos
            {
                NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name),
                Pagina = pagina,
                Etapa = objresumen.area,
            };

            dt = validar.ListarPerfil(validar);
            if (dt.Rows.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    btnLimpiar1.OnClientClick = "return LimpiarVacido1('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    btnLimpiar2.OnClientClick = "return LimpiarVacido2('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    btnLimpiar3.OnClientClick = "return LimpiarVacido3('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    btnLimpiarAct.OnClientClick = "return LimpiarVacidoAct('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";

                    lbEditar1.OnClientClick = "return HabilitarVaciadoIva1('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    lbEditar2.OnClientClick = "return HabilitarVaciadoIva2('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    lbEditar3.OnClientClick = "return HabilitarVaciadoIva3('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    lbEditarAct.OnClientClick = "return HabilitarVaciadoIvaAct('" + btnGuardar1.ClientID.Substring(0, btnGuardar1.ClientID.Length - 11) + "');";
                    ocultarBotones();

                    if (Page.Session["RESUMEN"] != null)
                    {
                        pnFormulario1.Enabled = false;
                        // lbEditar1.Style.Add("display", "block");

                        pnFormulario2.Enabled = false;
                        //lbEditar2.Style.Add("display", "block");

                        pnFormulario3.Enabled = false;
                        // lbEditar3.Style.Add("display", "block");

                        pnFormularioAct.Enabled = false;
                        // lbEditarAct.Style.Add("display", "block");

                        // objresumen = (Resumen)Page.Session["RESUMEN"];

                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;
                        objresumen = (Resumen)Page.Session["RESUMEN"];
                        Page.Session["RESUMEN"] = null;

                        if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 3).ToString()))
                        {
                            lbLi3.Style.Add("display", "none");
                            if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 2).ToString()))
                            {
                                lbLi2.Style.Add("display", "none");
                                if (int.Parse(objresumen.fecInicioActEconomica.Value.Year.ToString()) > int.Parse((DateTime.Now.Year - 1).ToString()))
                                {
                                    lbLi1.Style.Add("display", "none");
                                }
                            }
                        }
                        lbEmpresa.Text = objresumen.desEmpresa;
                        lbRut.Text = objresumen.rut;
                        lbEjecutivo.Text = objresumen.descEjecutivo;
                        lbOperacion.Text = objresumen.desOperacion;

                        ViewState["ACCION"] = Constantes.OPCION.INSERTAR;

                        //Verificación Edicion Simultanea        
                        string UsuarioFormulario = vem.verificacionEdicionSimultanea(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta", "Operacion");
                        if (!string.IsNullOrEmpty(UsuarioFormulario))
                        {
                            ocultarDiv();
                            dvWarning.Style.Add("display", "block");
                            lbWarning.Text = "Formulario bloqueado por: " + UsuarioFormulario + ". Por favor, intente editar este formulario más tarde. De ser un caso urgente comuníquese con el administrador del sistema.";
                            DeshabilitarBotones();
                            lbEditar1.Style.Add("display", "none");
                            lbEditar2.Style.Add("display", "none");
                            lbEditar3.Style.Add("display", "none");
                            lbEditarAct.Style.Add("display", "none");
                        }
                        llenargrid();

                        // liAct.Visible = false;  //objresumen.fecInicioActEconomica
                        lbLiAct.Text = "Ventas " + (DateTime.Now.Year).ToString();
                        lbLi1.Text = "Ventas " + (DateTime.Now.Year - 1).ToString();
                        lbLi2.Text = "Ventas " + (DateTime.Now.Year - 2).ToString();
                        lbLi3.Text = "Ventas " + (DateTime.Now.Year - 3).ToString();
                    }
                    else
                    {
                        Page.Response.Redirect("MensajeSession.aspx");
                    }
                }

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

        protected void btnFinalizarAct_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            if (ViewState["IDIVAVENTAS"] != null)
            {
                Boolean exito;
                string xmlData = generarXML((DateTime.Now.Year).ToString(), ref gridIvaAct);
                string mensaje = string.Empty;
                Boolean res;

                if (DateTime.Now.Month > 5)
                {
                    res = xmlData.Contains((DateTime.Now.Year).ToString());
                    if (!res)
                        mensaje = mensaje + (DateTime.Now.Year).ToString() + "  ";
                }

                if (mensaje == string.Empty)
                {
                    exito = Ln.finalizarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), xmlData, Constantes.RIESGO.VALIDACION, (DateTime.Now.Year).ToString());
                    pnFormularioAct.Enabled = false;
                    llenargrid();
                }
                else
                {
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REGISTOSINCOMPLETOSDOCCONTABLE) + " " + mensaje;
                }
            }
            else { guardarAct(); }
        }

        protected void btnFinalizar1_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            if (ViewState["IDIVAVENTAS"] != null)
            {
                Boolean exito;
                string xmlData = generarXML((DateTime.Now.Year - 1).ToString(), ref gridIva1);
                string mensaje = string.Empty;
                Boolean res;

                res = xmlData.Contains((DateTime.Now.Year - 1).ToString());
                if (!res)
                    mensaje = mensaje + (DateTime.Now.Year - 1).ToString() + "  ";


                if (mensaje == string.Empty)
                {
                    exito = Ln.finalizarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), xmlData, Constantes.RIESGO.VALIDACION, (DateTime.Now.Year - 1).ToString());
                    pnFormularioAct.Enabled = false;
                    llenargrid();
                }
                else
                {
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REGISTOSINCOMPLETOSDOCCONTABLE) + " " + mensaje;
                }
            }
            else { guardar1(); }
        }

        protected void btnFinalizar2_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            if (ViewState["IDIVAVENTAS"] != null)
            {
                Boolean exito;
                string xmlData = generarXML((DateTime.Now.Year - 2).ToString(), ref gridIva2);
                string mensaje = string.Empty;
                Boolean res;


                res = xmlData.Contains((DateTime.Now.Year - 2).ToString());
                if (!res)
                    mensaje = mensaje + (DateTime.Now.Year - 2).ToString() + "  ";


                if (mensaje == string.Empty)
                {
                    exito = Ln.finalizarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), xmlData, Constantes.RIESGO.VALIDACION, (DateTime.Now.Year - 2).ToString());
                    pnFormularioAct.Enabled = false;
                    llenargrid();
                }
                else
                {
                    ocultarDiv();
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REGISTOSINCOMPLETOSDOCCONTABLE) + " " + mensaje;
                }
            }
            else { guardar3(); }
        }


        protected void btnFinalizar3_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            if (ViewState["IDIVAVENTAS"] != null)
            {
                Boolean exito;
                string xmlData = generarXML((DateTime.Now.Year - 3).ToString(), ref gridIva3);
                string mensaje = string.Empty;
                Boolean res;


                res = xmlData.Contains((DateTime.Now.Year - 3).ToString());
                if (!res)
                    mensaje = mensaje + (DateTime.Now.Year - 3).ToString() + "  ";


                if (mensaje == string.Empty)
                {
                    exito = Ln.finalizarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), xmlData, Constantes.RIESGO.VALIDACION, (DateTime.Now.Year - 3).ToString());
                    pnFormularioAct.Enabled = false;
                    llenargrid();
                }
                else
                {
                    ocultarDiv();
                    dvWarning.Style.Add("display", "block");
                    lbWarning.Text = Ln.buscarMensaje(Constantes.MENSAJES.REGISTOSINCOMPLETOSDOCCONTABLE) + " " + mensaje;
                }
            }
            else { guardar4(); }
        }

        protected void btnGuardarAct_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            guardarAct();
        }

        protected void btnGuardar1_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            guardar1();
        }

        protected void btnGuardar3_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            guardar3();
        }

        protected void btnGuardar4_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Ln.GestionResponsableOperacion(objresumen.idOperacion, objresumen.idUsuario, objresumen.descCargo, "01");
            guardar4();
        }

        protected void lbBalance_Click(object sender, EventArgs e)
        {
            //BALANCE
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect("VaciadoBalance.aspx");
        }


        protected void lbEdoResultado_Click(object sender, EventArgs e)
        {
            //ESTADO DE RESULTADO
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect("VaciadoEdoResultado.aspx");
        }

        protected void lbVentas_Click(object sender, EventArgs e)
        {
            //IVA VENTAS
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect("VaciadoVentas.aspx");
        }

        protected void lbCompras_Click(object sender, EventArgs e)
        {
            //IVA COMPRAS
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect("VaciadoCompras.aspx");
        }

        protected void lbScoring_Click(object sender, EventArgs e)
        {
            //IVA COMPRAS 
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect("Scoring.aspx");
        }


        protected void lbResumenPAF_Click(object sender, EventArgs e)
        {
            //RESUMEN PAF
            asignacionResumen(ref objresumen);
            Page.Session["RESUMEN"] = objresumen;
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect("ResumenPaf.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "IvaVenta");
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void gridIvaAct_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[14].Visible = false;
        }

        protected void gridIva1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[14].Visible = false;
        }

        protected void gridIva2_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[14].Visible = false;
        }

        protected void gridIva3_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[14].Visible = false;
        }

        protected void gridIvaAct_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gridIvaAct_DataBound(object sender, EventArgs e)
        {
            try
            {
                DataTable res = new DataTable();
                if (ViewState["res1"] == null)
                    Page.Response.Redirect("MensajeSession.aspx");
                res = (DataTable)ViewState["res1"];

                for (int i = 0; i <= gridIvaAct.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        if (res.Rows[0][14].ToString() != "-1" && ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                            ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        if (res.Rows[0][14].ToString() != "-1")
                            ViewState["IDIVAVENTAS"] = res.Rows[0][14].ToString();
                    }
                    (gridIvaAct.Rows[i].FindControl("txtMes1") as TextBox).Text = res.Rows[i][2].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes2") as TextBox).Text = res.Rows[i][3].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes3") as TextBox).Text = res.Rows[i][4].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes4") as TextBox).Text = res.Rows[i][5].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes5") as TextBox).Text = res.Rows[i][6].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes6") as TextBox).Text = res.Rows[i][7].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes7") as TextBox).Text = res.Rows[i][8].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes8") as TextBox).Text = res.Rows[i][9].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes9") as TextBox).Text = res.Rows[i][10].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes10") as TextBox).Text = res.Rows[i][11].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes11") as TextBox).Text = res.Rows[i][12].ToString();
                    (gridIvaAct.Rows[i].FindControl("txtMes12") as TextBox).Text = res.Rows[i][13].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }


        protected void gridIva1_DataBound(object sender, EventArgs e)
        {
            try
            {
                DataTable res = new DataTable();
                if (ViewState["res2"] == null)
                    Page.Response.Redirect("MensajeSession.aspx");
                res = (DataTable)ViewState["res2"];

                for (int i = 0; i <= gridIva1.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        if (res.Rows[0][14].ToString() != "-1" && ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                            ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        if (res.Rows[0][14].ToString() != "-1")
                            ViewState["IDIVAVENTAS"] = res.Rows[0][14].ToString();
                    }
                    (gridIva1.Rows[i].FindControl("txtMes1") as TextBox).Text = res.Rows[i][2].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes2") as TextBox).Text = res.Rows[i][3].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes3") as TextBox).Text = res.Rows[i][4].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes4") as TextBox).Text = res.Rows[i][5].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes5") as TextBox).Text = res.Rows[i][6].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes6") as TextBox).Text = res.Rows[i][7].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes7") as TextBox).Text = res.Rows[i][8].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes8") as TextBox).Text = res.Rows[i][9].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes9") as TextBox).Text = res.Rows[i][10].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes10") as TextBox).Text = res.Rows[i][11].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes11") as TextBox).Text = res.Rows[i][12].ToString();
                    (gridIva1.Rows[i].FindControl("txtMes12") as TextBox).Text = res.Rows[i][13].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void gridIva2_DataBound(object sender, EventArgs e)
        {
            try
            {
                DataTable res = new DataTable();
                if (ViewState["res3"] == null)
                    Page.Response.Redirect("MensajeSession.aspx");
                res = (DataTable)ViewState["res3"];

                for (int i = 0; i <= gridIva2.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        if (res.Rows[0][14].ToString() != "-1" && ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                            ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        if (res.Rows[0][14].ToString() != "-1")
                            ViewState["IDIVAVENTAS"] = res.Rows[0][14].ToString();
                    }
                    (gridIva2.Rows[i].FindControl("txtMes1") as TextBox).Text = res.Rows[i][2].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes2") as TextBox).Text = res.Rows[i][3].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes3") as TextBox).Text = res.Rows[i][4].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes4") as TextBox).Text = res.Rows[i][5].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes5") as TextBox).Text = res.Rows[i][6].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes6") as TextBox).Text = res.Rows[i][7].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes7") as TextBox).Text = res.Rows[i][8].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes8") as TextBox).Text = res.Rows[i][9].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes9") as TextBox).Text = res.Rows[i][10].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes10") as TextBox).Text = res.Rows[i][11].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes11") as TextBox).Text = res.Rows[i][12].ToString();
                    (gridIva2.Rows[i].FindControl("txtMes12") as TextBox).Text = res.Rows[i][13].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void gridIva3_DataBound(object sender, EventArgs e)
        {
            try
            {
                DataTable res = new DataTable();
                if (ViewState["res4"] == null)
                    Page.Response.Redirect("MensajeSession.aspx");
                res = (DataTable)ViewState["res4"];

                for (int i = 0; i <= gridIva3.Rows.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        if (res.Rows[0][14].ToString() != "-1" && ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
                            ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                        if (res.Rows[0][14].ToString() != "-1")
                            ViewState["IDIVAVENTAS"] = res.Rows[0][14].ToString();
                    }
                    (gridIva3.Rows[i].FindControl("txtMes1") as TextBox).Text = res.Rows[i][2].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes2") as TextBox).Text = res.Rows[i][3].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes3") as TextBox).Text = res.Rows[i][4].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes4") as TextBox).Text = res.Rows[i][5].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes5") as TextBox).Text = res.Rows[i][6].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes6") as TextBox).Text = res.Rows[i][7].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes7") as TextBox).Text = res.Rows[i][8].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes8") as TextBox).Text = res.Rows[i][9].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes9") as TextBox).Text = res.Rows[i][10].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes10") as TextBox).Text = res.Rows[i][11].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes11") as TextBox).Text = res.Rows[i][12].ToString();
                    (gridIva3.Rows[i].FindControl("txtMes12") as TextBox).Text = res.Rows[i][13].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }

        protected void btnImprimirReporteIva_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);

            string Reporte = "ResumenIVA";

            byte[] archivo = GenerarReporte(objresumen.idEmpresa);
            if (archivo != null)
            {
                Page.Session["tipoDoc"] = "pdf";
                Page.Session["binaryData"] = archivo;
                Page.Session["Titulo"] = Reporte;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('BajarDocumento.aspx','_blank')", true);
            }
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

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        protected void ocultarBotones()
        {
            btnLimpiar1.Style.Add("display", "none");
            btnLimpiar2.Style.Add("display", "none");
            btnLimpiar3.Style.Add("display", "none");
            btnLimpiarAct.Style.Add("display", "none");

            btnGuardar1.Style.Add("display", "none");
            btnGuardar2.Style.Add("display", "none");
            btnGuardar3.Style.Add("display", "none");
            btnGuardarAct.Style.Add("display", "none");

            btnFinalizar1.Style.Add("display", "none");
            btnFinalizar2.Style.Add("display", "none");
            btnFinalizar3.Style.Add("display", "none");
            btnFinalizarAct.Style.Add("display", "none");
        }

        protected void DeshabilitarBotones()
        {
            btnGuardar1.Enabled = false;
            btnGuardar2.Enabled = false;
            btnGuardar3.Enabled = false;
            btnGuardarAct.Enabled = false;

            pnFormulario1.Enabled = false;
            pnFormulario2.Enabled = false;
            pnFormulario3.Enabled = false;
            pnFormularioAct.Enabled = false;
        }

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();

            ViewState["res1"] = Ln.ListarIVAVentas(objresumen.idEmpresa, DateTime.Now.Year, Constantes.DOCUMENTOSCONTABLE.IVAVENTAS);
            ViewState["res2"] = Ln.ListarIVAVentas(objresumen.idEmpresa, DateTime.Now.Year - 1, Constantes.DOCUMENTOSCONTABLE.IVAVENTAS);
            ViewState["res3"] = Ln.ListarIVAVentas(objresumen.idEmpresa, DateTime.Now.Year - 2, Constantes.DOCUMENTOSCONTABLE.IVAVENTAS);
            ViewState["res4"] = Ln.ListarIVAVentas(objresumen.idEmpresa, DateTime.Now.Year - 3, Constantes.DOCUMENTOSCONTABLE.IVAVENTAS);

            gridIvaAct.DataSource = ViewState["res1"];
            gridIva1.DataSource = ViewState["res2"];
            gridIva2.DataSource = ViewState["res3"];
            gridIva3.DataSource = ViewState["res4"];

            gridIvaAct.DataBind();
            gridIva1.DataBind();
            gridIva2.DataBind();
            gridIva3.DataBind();
        }

        private string generarXML(String Anio, ref GridView grid)
        {
            DataTable res = new DataTable();
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            nodosXML("1", Anio, "txtMes1", ref doc, ref ValoresNode, ref grid);
            nodosXML("2", Anio, "txtMes2", ref doc, ref ValoresNode, ref grid);
            nodosXML("3", Anio, "txtMes3", ref doc, ref ValoresNode, ref grid);
            nodosXML("4", Anio, "txtMes4", ref doc, ref ValoresNode, ref grid);
            nodosXML("5", Anio, "txtMes5", ref doc, ref ValoresNode, ref grid);
            nodosXML("6", Anio, "txtMes6", ref doc, ref ValoresNode, ref grid);
            nodosXML("7", Anio, "txtMes7", ref doc, ref ValoresNode, ref grid);
            nodosXML("8", Anio, "txtMes8", ref doc, ref ValoresNode, ref grid);
            nodosXML("9", Anio, "txtMes9", ref doc, ref ValoresNode, ref grid);
            nodosXML("10", Anio, "txtMes10", ref doc, ref ValoresNode, ref grid);
            nodosXML("11", Anio, "txtMes11", ref doc, ref ValoresNode, ref grid);
            nodosXML("12", Anio, "txtMes12", ref doc, ref ValoresNode, ref grid);

            return doc.OuterXml;
        }

        private void nodosXML(string Mes, string Anio, string txtMes, ref XmlDocument doc, ref XmlNode ValoresNode, ref GridView grid)
        {
            //GridView grid = new GridView();
            XmlNode RespNode;
            for (int i = 0; i <= grid.Rows.Count - 1; i++)
            {
                if (!(String.IsNullOrEmpty((grid.Rows[i].FindControl(txtMes) as TextBox).Text))
                    && (grid.Rows[i].FindControl(txtMes) as TextBox).Text.ToString() != " "
                    && System.Web.HttpUtility.HtmlDecode(grid.Rows[i].Cells[8].Text).Trim() != " ")
                {
                    RespNode = doc.CreateElement("Val");

                    XmlNode IdCuentaNode = doc.CreateElement("IdCuenta");
                    IdCuentaNode.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(grid.Rows[i].Cells[0].Text).Trim()));
                    RespNode.AppendChild(IdCuentaNode);

                    XmlNode IdAnioNode = doc.CreateElement("Anio");
                    IdAnioNode.AppendChild(doc.CreateTextNode((Anio)));
                    RespNode.AppendChild(IdAnioNode);

                    XmlNode DescTipo = doc.CreateElement("Mes");
                    DescTipo.AppendChild(doc.CreateTextNode((Mes)));
                    RespNode.AppendChild(DescTipo);

                    XmlNode ValorAnio1 = doc.CreateElement("Valor");
                    ValorAnio1.AppendChild(doc.CreateTextNode((grid.Rows[i].FindControl(txtMes) as TextBox).Text.ToString()));
                    RespNode.AppendChild(ValorAnio1);

                    ValoresNode.AppendChild(RespNode);
                }
            }

        }

        protected void guardarAct()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean exito;

            if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
            {
                exito = Ln.InsertarIvaVENTAS(objresumen.idEmpresa.ToString(), generarXML((DateTime.Now.Year).ToString(), ref gridIvaAct), (DateTime.Now.Year).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    llenargrid();
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                }
            }
            else
            {
                exito = Ln.ModificarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), generarXML((DateTime.Now.Year).ToString(), ref gridIvaAct), (DateTime.Now.Year).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE);
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE);
                }
            }
        }

        protected void guardar1()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean exito;

            if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
            {
                exito = Ln.InsertarIvaVENTAS(objresumen.idEmpresa.ToString(), generarXML((DateTime.Now.Year - 1).ToString(), ref gridIva1), (DateTime.Now.Year - 1).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    llenargrid();
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                }
            }
            else
            {
                exito = Ln.ModificarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), generarXML((DateTime.Now.Year - 1).ToString(), ref gridIva1), (DateTime.Now.Year - 1).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE);
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE);
                }
            }
        }

        protected void guardar3()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean exito;

            if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
            {
                exito = Ln.InsertarIvaVENTAS(objresumen.idEmpresa.ToString(), generarXML((DateTime.Now.Year - 2).ToString(), ref gridIva2), (DateTime.Now.Year - 2).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    llenargrid();
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                }
            }
            else
            {
                exito = Ln.ModificarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), generarXML((DateTime.Now.Year - 2).ToString(), ref gridIva2), (DateTime.Now.Year - 2).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE);
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE);
                }
            }
        }

        protected void guardar4()
        {
            asignacionResumen(ref objresumen);
            LogicaNegocio Ln = new LogicaNegocio();
            Boolean exito;

            if (ViewState["ACCION"].ToString() == Constantes.OPCION.INSERTAR)
            {
                exito = Ln.InsertarIvaVENTAS(objresumen.idEmpresa.ToString(), generarXML((DateTime.Now.Year - 3).ToString(), ref gridIva3), (DateTime.Now.Year - 3).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOINSETDOCCONTABLE);
                    ViewState["ACCION"] = Constantes.OPCION.MODIFICAR;
                    llenargrid();
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORINSERTDOCCONTABLE);
                }
            }
            else
            {
                exito = Ln.ModificarIvaVENTAS(objresumen.idEmpresa.ToString(), ViewState["IDIVAVENTAS"].ToString(), generarXML((DateTime.Now.Year - 3).ToString(), ref gridIva3), (DateTime.Now.Year - 3).ToString());
                if (exito)
                {
                    dvSuccess.Style.Add("display", "block");
                    lbSuccess.Text = Ln.buscarMensaje(Constantes.MENSAJES.EXITOMODIFDOCCONTABLE);
                }
                else
                {
                    dvError.Style.Add("display", "block");
                    lbError.Text = Ln.buscarMensaje(Constantes.MENSAJES.ERRORMODIFDOCCONTABLE);
                }
            }
        }

        string html = string.Empty;
        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();

        public byte[] GenerarReporte(int idEmpresa)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                String xml = String.Empty;
                String nombreGrafico1 = String.Empty;
                String nombreGrafico2 = String.Empty;
                String rutaGraficos = String.Empty;

                nombreGrafico1 = "G1" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + idEmpresa.ToString();
                nombreGrafico2 = "G2" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + idEmpresa.ToString();
                rutaGraficos = "http://localhost:25698/graficas/";

                DataSet res1 = new DataSet();

                res1 = Ln.ConsultaReporteIvaCompraVenta(objresumen.idEmpresa);
                for (int i = 0; i < res1.Tables[0].Rows.Count; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }

                xml = xml.Replace("<Grafico1></Grafico1>", "<Grafico1>" + rutaGraficos + nombreGrafico1 + ".png" + "</Grafico1>");
                xml = xml.Replace("<Grafico2></Grafico2>", "<Grafico2>" + rutaGraficos + nombreGrafico2 + ".png" + "</Grafico2>");

                crearGrafico1(res1.Tables[1], nombreGrafico1);
                crearGrafico2(res1.Tables[1], nombreGrafico2);


                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "ResumenIVA" + ".xslt");
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
                catch { throw; }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        private void crearGrafico1(DataTable dt, string nombreGrafico1)
        {
            //Gráfico 1;
            double[] anioAct = new double[12];
            double[] anio1 = new double[12];
            double[] anio2 = new double[12];
            double[] anio3 = new double[12];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                anioAct[i] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
                anio1[i] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
                anio2[i] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
                anio3[i] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
            }
            string[] meses = new string[12];
            int nroMes = 1;
            CultureInfo culture = new CultureInfo("es-ES");
            for (int i = 0; i < meses.Length; i++)
            {
                //meses[i] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(nroMes); 
                meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
                nroMes++;
            }

            Chart grafico1 = new Chart();
            grafico1.ChartAreas.Add(new ChartArea());
            grafico1.ChartAreas[0].AxisX.Interval = 1;
            grafico1.Width = 710;
            grafico1.Height = 300;
            Series serieAnioAct = new Series();
            Series serieAnio1 = new Series();
            Series serieAnio2 = new Series();
            Series serieAnio3 = new Series();

            serieAnioAct.Points.DataBindXY(meses, anioAct);
            serieAnio1.Points.DataBindXY(meses, anio1);
            serieAnio2.Points.DataBindXY(meses, anio2);
            serieAnio3.Points.DataBindXY(meses, anio3);

            serieAnioAct.Name = (DateTime.Now.Year).ToString();
            serieAnio1.Name = (DateTime.Now.Year - 1).ToString();
            serieAnio2.Name = (DateTime.Now.Year - 2).ToString();
            serieAnio3.Name = (DateTime.Now.Year - 3).ToString();

            //#99BE1A verde
            //#E85426 nar
            //#1E9CD6 azu
            //#9966CC mor
            serieAnioAct.Color = ColorTranslator.FromHtml("#1E9CD6");
            serieAnio1.Color = ColorTranslator.FromHtml("#9966CC");
            serieAnio2.Color = ColorTranslator.FromHtml("#E85426");
            serieAnio3.Color = ColorTranslator.FromHtml("#99BE1A");

            serieAnioAct.BorderWidth = 3;
            serieAnio1.BorderWidth = 3;
            serieAnio2.BorderWidth = 3;
            serieAnio3.BorderWidth = 3;

            serieAnioAct.ChartType = SeriesChartType.Spline;
            serieAnio1.ChartType = SeriesChartType.Spline;
            serieAnio2.ChartType = SeriesChartType.Spline;
            serieAnio3.ChartType = SeriesChartType.Spline;
            Title titulo = new Title();
            titulo.Name = "Titulo1";
            titulo.Text = "Ventas Mensuales por Período (M$)";
            titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
            grafico1.Titles.Add(titulo);

            Legend lg = new Legend();
            lg.Name = "Leyenda1";
            lg.Docking = Docking.Bottom;
            lg.Alignment = StringAlignment.Center;
            grafico1.Legends.Add(lg);
            serieAnioAct.Legend = "Leyenda1";

            grafico1.Series.Add(serieAnioAct);
            grafico1.Series.Add(serieAnio1);
            grafico1.Series.Add(serieAnio2);
            grafico1.Series.Add(serieAnio3);

            grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico1 + ".png", ChartImageFormat.Png);
        }

        private void crearGrafico2(DataTable dt, string nombreGrafico2)
        {
            double[] ventas = new double[48];

            int index = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
                index++;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
                index++;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
                index++;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ventas[index] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
                index++;
            }

            string[] meses = new string[48];
            int nroMes = 1;
            CultureInfo culture = new CultureInfo("es-ES");
            for (int i = 0; i < meses.Length; i++)
            {
                //meses[i] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(nroMes); 
                if (nroMes == 13)
                {
                    nroMes = 1;
                }
                meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
                nroMes++;
            }

            Chart grafico1 = new Chart();
            grafico1.ChartAreas.Add(new ChartArea());
            grafico1.ChartAreas[0].AxisX.Interval = 1;
            grafico1.Width = 710;
            grafico1.Height = 300;

            Series serieVentas = new Series();

            serieVentas.Points.DataBindXY(meses, ventas);
            serieVentas.Name = (DateTime.Now.Year - 3).ToString() + ", " + (DateTime.Now.Year - 2).ToString() + ", " + (DateTime.Now.Year - 1).ToString() + ", " + (DateTime.Now.Year).ToString();
            serieVentas.Color = Color.OrangeRed;//ColorTranslator.FromHtml("#1E9CD6");
            serieVentas.BorderWidth = 3;
            serieVentas.ChartType = SeriesChartType.Spline;

            Title titulo = new Title();
            titulo.Name = "Titulo1";
            titulo.Text = "Ventas Todos los Períodos (M$)";
            titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
            grafico1.Titles.Add(titulo);

            Legend lg = new Legend();
            lg.Name = "Leyenda1";
            lg.Docking = Docking.Bottom;
            lg.Alignment = StringAlignment.Center;
            grafico1.Legends.Add(lg);
            serieVentas.Legend = "Leyenda1";

            grafico1.Series.Add(serieVentas);
            grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico2 + ".png", ChartImageFormat.Png);
        }

        #endregion

    }
}
