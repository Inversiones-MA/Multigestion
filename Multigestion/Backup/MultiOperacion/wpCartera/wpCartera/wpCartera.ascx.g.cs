﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiOperacion.wpCartera.wpCartera {
    using System.Web.UI.WebControls.Expressions;
    using System.Web.UI.HtmlControls;
    using System.Collections;
    using System.Text;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint.WebPartPages;
    using System.Web.SessionState;
    using System.Configuration;
    using Microsoft.SharePoint;
    using System.Web;
    using System.Web.DynamicData;
    using System.Web.Caching;
    using System.Web.Profile;
    using System.ComponentModel.DataAnnotations;
    using System.Web.UI.WebControls;
    using System.Web.Security;
    using System;
    using Microsoft.SharePoint.Utilities;
    using System.Text.RegularExpressions;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint.WebControls;
    
    
    public partial class wpCartera {
        
        protected global::System.Web.UI.WebControls.Label lbWarning1;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning1;
        
        protected global::System.Web.UI.WebControls.Label lbSuccess;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvSuccess;
        
        protected global::System.Web.UI.WebControls.Label lbWarning;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning;
        
        protected global::System.Web.UI.WebControls.Label lbError;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvError;
        
        protected global::System.Web.UI.WebControls.LinkButton lbSeguimiento;
        
        protected global::System.Web.UI.WebControls.LinkButton lbCalendarioPago;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlAcreedor;
        
        protected global::System.Web.UI.WebControls.TextBox txtRUT;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEjecutivo;
        
        protected global::System.Web.UI.WebControls.TextBox txtRazonSocial;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEdoCertificado;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlFondo;
        
        protected global::System.Web.UI.WebControls.TextBox txtNCertificado;
        
        protected global::System.Web.UI.WebControls.Button btnBuscar;
        
        protected global::System.Web.UI.WebControls.GridView gridSeguimiento;
        
        protected global::System.Web.UI.WebControls.Button btnAtras;
        
        protected global::System.Web.UI.WebControls.Button btnCancelar;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvFormulario;
        
        public static implicit operator global::System.Web.UI.TemplateControl(wpCartera target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbWarning1() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbWarning1 = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbWarning1";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldvWarning1() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dvWarning1 = @__ctrl;
            @__ctrl.ID = "dvWarning1";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "display: none");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "alert alert-warning");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("role", "alert");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n       <h4>\r\n        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbWarning1();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n      </h4>\r\n "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbSuccess() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbSuccess = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbSuccess";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldvSuccess() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dvSuccess = @__ctrl;
            @__ctrl.ID = "dvSuccess";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "display: none");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "alert alert-success");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        <h4>\r\n            "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbSuccess();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        </h4>\r\n    "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbWarning() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbWarning = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbWarning";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldvWarning() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dvWarning = @__ctrl;
            @__ctrl.ID = "dvWarning";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "display: none");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "alert alert-warning");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("role", "alert");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        <h4>\r\n            "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbWarning();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        </h4>\r\n    "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbError() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbError = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbError";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldvError() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dvError = @__ctrl;
            @__ctrl.ID = "dvError";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("style", "display: none");
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "alert alert-danger");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        <h4>\r\n            "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbError();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        </h4>\r\n    "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControllbSeguimiento() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.lbSeguimiento = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbSeguimiento";
            @__ctrl.OnClientClick = "return Dialogo();";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Cartera"));
            @__ctrl.Click -= new System.EventHandler(this.lbSeguimiento_Click);
            @__ctrl.Click += new System.EventHandler(this.lbSeguimiento_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControllbCalendarioPago() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.lbCalendarioPago = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbCalendarioPago";
            @__ctrl.OnClientClick = "return Dialogo();";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Calendario de Pago"));
            @__ctrl.Click -= new System.EventHandler(this.lbCalendarioPago_Click);
            @__ctrl.Click += new System.EventHandler(this.lbCalendarioPago_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlAcreedor() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlAcreedor = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlAcreedor";
            @__ctrl.CssClass = "form-control";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.TextBox @__BuildControltxtRUT() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.txtRUT = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "txtRUT";
            @__ctrl.CssClass = "form-control";
            @__ctrl.MaxLength = 20;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlEjecutivo() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlEjecutivo = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlEjecutivo";
            @__ctrl.CssClass = "form-control";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.TextBox @__BuildControltxtRazonSocial() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.txtRazonSocial = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "txtRazonSocial";
            @__ctrl.CssClass = "form-control";
            @__ctrl.MaxLength = 200;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlEdoCertificado() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlEdoCertificado = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlEdoCertificado";
            @__ctrl.CssClass = "form-control";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlFondo() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlFondo = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlFondo";
            @__ctrl.CssClass = "form-control";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.TextBox @__BuildControltxtNCertificado() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.txtNCertificado = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "txtNCertificado";
            @__ctrl.CssClass = "form-control";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnBuscar() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnBuscar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnBuscar";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("Style", "margin-left: 0px;");
            @__ctrl.Text = "Buscar";
            @__ctrl.CssClass = "btn btn-primary btn-success";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnBuscar_Click);
            @__ctrl.Click += new System.EventHandler(this.btnBuscar_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControl__control2(System.Web.UI.WebControls.TableItemStyle @__ctrl) {
            @__ctrl.CssClass = "pagination-ys";
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.GridView @__BuildControlgridSeguimiento() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.gridSeguimiento = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "gridSeguimiento";
            @__ctrl.GridLines = global::System.Web.UI.WebControls.GridLines.Horizontal;
            @__ctrl.CssClass = "table table-bordered table-hover";
            @__ctrl.AllowPaging = true;
            @__ctrl.RowHeaderColumn = "0";
            @__ctrl.PageSize = 15;
            @__ctrl.ShowHeaderWhenEmpty = true;
            this.@__BuildControl__control2(@__ctrl.PagerStyle);
            @__ctrl.RowDataBound -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.gridSeguimiento_RowDataBound);
            @__ctrl.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.gridSeguimiento_RowDataBound);
            @__ctrl.RowCreated -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.gridSeguimiento_RowCreated);
            @__ctrl.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(this.gridSeguimiento_RowCreated);
            @__ctrl.PageIndexChanging -= new System.Web.UI.WebControls.GridViewPageEventHandler(this.gridSeguimiento_PageIndexChanging);
            @__ctrl.PageIndexChanging += new System.Web.UI.WebControls.GridViewPageEventHandler(this.gridSeguimiento_PageIndexChanging);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnAtras() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnAtras = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnAtras";
            @__ctrl.CssClass = "btn btn-default pull-left";
            @__ctrl.Text = " < Volver Atrás";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnAtras_Click1);
            @__ctrl.Click += new System.EventHandler(this.btnAtras_Click1);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnCancelar() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnCancelar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnCancelar";
            @__ctrl.Text = "Cancelar";
            @__ctrl.CssClass = "btn btn-primary pull-right";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnCancelar_Click);
            @__ctrl.Click += new System.EventHandler(this.btnCancelar_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldvFormulario() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dvFormulario = @__ctrl;
            @__ctrl.ID = "dvFormulario";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"

    <!-- Page-Title -->
    <div class=""row marginInicio"">
        <div class=""col-sm-12"">
            <h4 class=""page-title"">Cartera</h4>
            <ol class=""breadcrumb"">
                <li>
                    <a href=""#"">Inicio</a>
                </li>
                <li class=""active"">Cartera
                </li>
            </ol>
        </div>
    </div>

    <br />
    "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControldvSuccess();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n    "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl2;
            @__ctrl2 = this.@__BuildControldvWarning();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n    "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl3;
            @__ctrl3 = this.@__BuildControldvError();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
    <br />


    <!-- tabs -->
    <div class=""row"">
        <div class=""col-md-12"">
            <ul class=""nav nav-tabs navtab-bg nav-justified"">
                <li class=""active"">
                    <a href=""#tab1"" data-toggle=""tab"" aria-expanded=""true"">
                        <span class=""visible-xs""><i class=""fa fa-home""></i></span>
                        <span class=""hidden-xs"">
                            "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl4;
            @__ctrl4 = this.@__BuildControllbSeguimiento();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                        </span>
                    </a>
                </li>
                <li class="""">
                    <a href=""#tab2"" data-toggle=""tab"" aria-expanded=""false"">
                        <span class=""visible-xs""><i class=""fa fa-user""></i></span>
                        <span class=""hidden-xs"">
                            "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl5;
            @__ctrl5 = this.@__BuildControllbCalendarioPago();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                        </span>
                    </a>
                </li>
            </ul>


            <div class=""tab-content"">
                <div class=""tab-pane active"" id=""tab1"">
                    <div class=""row"" id=""filtros"">
                        <!-- col 1/4 -->
                        <div class=""col-md-3 col-sm-6"">
                            <div class=""form-group"">
                                <label for="""">Acreedor:</label>
                                "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl6;
            @__ctrl6 = this.@__BuildControlddlAcreedor();
            @__parser.AddParsedSubObject(@__ctrl6);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                            <div class=\"for" +
                        "m-group\">\r\n                                <label for=\"\">Rut:</label>\r\n         " +
                        "                       "));
            global::System.Web.UI.WebControls.TextBox @__ctrl7;
            @__ctrl7 = this.@__BuildControltxtRUT();
            @__parser.AddParsedSubObject(@__ctrl7);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                            </div>
                        </div>
                        <!-- col 2/4 -->
                        <div class=""col-md-3 col-sm-6"">
                            <div class=""form-group"">
                                <label for="""">Ejecutivo:</label>
                                "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl8;
            @__ctrl8 = this.@__BuildControlddlEjecutivo();
            @__parser.AddParsedSubObject(@__ctrl8);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                            <div class=\"for" +
                        "m-group\">\r\n                                <label for=\"\">Razon social:</label>\r\n" +
                        "                                "));
            global::System.Web.UI.WebControls.TextBox @__ctrl9;
            @__ctrl9 = this.@__BuildControltxtRazonSocial();
            @__parser.AddParsedSubObject(@__ctrl9);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                            </div>
                        </div>
                        <!-- col 3/4 -->
                        <div class=""col-md-3 col-sm-6"">
                            <div class=""form-group"">
                                <label for="""">Edo. Certificado:</label>
                                "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl10;
            @__ctrl10 = this.@__BuildControlddlEdoCertificado();
            @__parser.AddParsedSubObject(@__ctrl10);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                            <div class=\"for" +
                        "m-group\">\r\n                                <label for=\"\">Fondo:</label>\r\n       " +
                        "                         "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl11;
            @__ctrl11 = this.@__BuildControlddlFondo();
            @__parser.AddParsedSubObject(@__ctrl11);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                            </div>
                        </div>
                        <!-- col 4/4 -->
                        <div class=""col-md-2 col-sm-6"">
                            <div class=""form-group"">
                                <label for="""">Nº Certificado:</label>
                                "));
            global::System.Web.UI.WebControls.TextBox @__ctrl12;
            @__ctrl12 = this.@__BuildControltxtNCertificado();
            @__parser.AddParsedSubObject(@__ctrl12);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                            <div class=\"for" +
                        "m-group rpsForm\">\r\n                                <label style=\"color: transpar" +
                        "ent;\">.</label><br>\r\n                                "));
            global::System.Web.UI.WebControls.Button @__ctrl13;
            @__ctrl13 = this.@__BuildControlbtnBuscar();
            @__parser.AddParsedSubObject(@__ctrl13);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                            </div>
                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class=""row"" id=""grilla"">
                        <div class=""col-md-12"">
                            <table border=""0"" style=""table-layout: fixed; width: 100%;"">
                                <tr>
                                    <td>
                                        <div class=""card-box"">
                                            <div class=""table-responsive"">
                                                "));
            global::System.Web.UI.WebControls.GridView @__ctrl14;
            @__ctrl14 = this.@__BuildControlgridSeguimiento();
            @__parser.AddParsedSubObject(@__ctrl14);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <br />
                    <table style=""width:100%"">
                        <tr>
                            <td>
                                "));
            global::System.Web.UI.WebControls.Button @__ctrl15;
            @__ctrl15 = this.@__BuildControlbtnAtras();
            @__parser.AddParsedSubObject(@__ctrl15);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </td>\r\n\r\n                            <td>\r\n        " +
                        "                        "));
            global::System.Web.UI.WebControls.Button @__ctrl16;
            @__ctrl16 = this.@__BuildControlbtnCancelar();
            @__parser.AddParsedSubObject(@__ctrl16);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                            </td>\r\n                        </tr>\r\n           " +
                        "         </table>\r\n\r\n                </div>\r\n\r\n            </div>\r\n        </div" +
                        ">\r\n    </div>\r\n\r\n"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::MultiOperacion.wpCartera.wpCartera.wpCartera @__ctrl) {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControldvWarning1();
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(@__ctrl1);
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl2;
            @__ctrl2 = this.@__BuildControldvFormulario();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__ctrl.SetRenderMethodDelegate(new System.Web.UI.RenderMethod(this.@__Render__control1));
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__Render__control1(System.Web.UI.HtmlTextWriter @__w, System.Web.UI.Control parameterContainer) {
            @__w.Write("\r\n\r\n<link rel=\"stylesheet\" href=\"../../_layouts/15/PagerStyle.css\" />\r\n<style typ" +
                    "e=\"text/css\">\r\n    .btn { font:14px \'Noto Sans\', \'Helvetica Neue\', Helvetica, Ar" +
                    "ial, sans-serif !important; }\r\n    .cDateBox { width:100px; }\r\n    .rpsForm{text" +
                    "-align:left;}\r\n</style>\r\n\r\n<script type=\"text/javascript\" src=\"../../_layouts/15" +
                    "/MultiOperacion/Operacion.js\"></script>\r\n<script type=\"text/javascript\">\r\n    do" +
                    "cument.onkeydown = function () {\r\n        if (window.event && (window.event.keyC" +
                    "ode == 109 || window.event.keyCode == 56)) {\r\n            window.event.keyCode =" +
                    " 505;\r\n        }\r\n\r\n        if (window.event.keyCode == 505) {\r\n            retu" +
                    "rn false;\r\n        }\r\n\r\n        if (window.event && (window.event.keyCode == 13)" +
                    ") {\r\n            valor = document.activeElement.value;\r\n            if (valor ==" +
                    " undefined) { return false; } //Evita Enter en página. \r\n            else {\r\n   " +
                    "             if (document.activeElement.getAttribute(\'type\') == \'select-one\')\r\n " +
                    "               { return false; } //Evita Enter en select. \r\n                if (" +
                    "document.activeElement.getAttribute(\'type\') == \'button\')\r\n                { retu" +
                    "rn false; } //Evita Enter en button. \r\n                if (document.activeElemen" +
                    "t.getAttribute(\'type\') == \'radio\')\r\n                { return false; } //Evita En" +
                    "ter en radio. \r\n                if (document.activeElement.getAttribute(\'type\') " +
                    "== \'checkbox\')\r\n                { return false; } //Evita Enter en checkbox. \r\n " +
                    "               if (document.activeElement.getAttribute(\'type\') == \'file\')\r\n     " +
                    "           { return false; } //Evita Enter en file. \r\n                if (docume" +
                    "nt.activeElement.getAttribute(\'type\') == \'reset\')\r\n                { return fals" +
                    "e; } //Evita Enter en reset. \r\n                if (document.activeElement.getAtt" +
                    "ribute(\'type\') == \'submit\')\r\n                { return false; } //Evita Enter en " +
                    "submit. \r\n                if (document.activeElement.getAttribute(\'type\') == \'in" +
                    "put\')\r\n                { return false; } //Evita Back en input. \r\n              " +
                    "  else //Text, textarea o password \r\n                {\r\n                    if (" +
                    "document.activeElement.value.length == 0)\r\n\r\n                    { return false;" +
                    " } //No realiza el backspace(largo igual a 0). \r\n                }\r\n\r\n          " +
                    "      if (document.getElementById(\'");
                                     @__w.Write(txtNCertificado.ClientID );

            @__w.Write("\').val != \'\' && document.getElementById(\'");
                                                                                                            @__w.Write(txtRUT.ClientID );

            @__w.Write("\').val != \'\' && document.getElementById(\'");
                                                                                                                                                                          @__w.Write(txtRazonSocial.ClientID );

            @__w.Write("\').val != \'\') {\r\n                        document.getElementById(\'");
                                         @__w.Write(btnBuscar.ClientID );

            @__w.Write(@"').click();
                }
            }
        }
    }

    function MantenSesion() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'actualizando su solicitud', 120, 550);
        __doPostBack('', '');
        return false;
    }

    setInterval(""MantenSesion()"", (1200000));
</script>


 ");
            parameterContainer.Controls[0].RenderControl(@__w);
            @__w.Write("\r\n\r\n");
            parameterContainer.Controls[1].RenderControl(@__w);
            @__w.Write("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
        }
        
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
