﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiOperacion.wpSeguimiento.wpSeguimiento {
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
    
    
    public partial class wpSeguimiento {
        
        protected global::System.Web.UI.WebControls.Label lbWarning1;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning1;
        
        protected global::System.Web.UI.WebControls.Label lbSuccess;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvSuccess;
        
        protected global::System.Web.UI.WebControls.Label lbWarning;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning;
        
        protected global::System.Web.UI.WebControls.Label lbError;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvError;
        
        protected global::System.Web.UI.WebControls.LinkButton lbCalendarioPago;
        
        protected global::System.Web.UI.WebControls.LinkButton lbSeguimiento;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlAcreedor;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEjecutivo;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEdoCertificado;
        
        protected global::System.Web.UI.WebControls.Button btnBuscar;
        
        protected global::System.Web.UI.WebControls.GridView gridSeguimiento;
        
        protected global::System.Web.UI.WebControls.Panel pnFormulario;
        
        protected global::System.Web.UI.WebControls.Button btnAtras;
        
        protected global::System.Web.UI.WebControls.Button btnCancelar;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvFormulario;
        
        public static implicit operator global::System.Web.UI.TemplateControl(wpSeguimiento target) 
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    <h4>\r\n        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbWarning1();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </h4>\r\n"));
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    <h4>\r\n         "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbSuccess();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </h4>\r\n"));
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    <h4>\r\n        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbWarning();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </h4>\r\n"));
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    <h4>\r\n        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbError();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    </h4>\r\n"));
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
        private global::System.Web.UI.WebControls.LinkButton @__BuildControllbSeguimiento() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.lbSeguimiento = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbSeguimiento";
            @__ctrl.OnClientClick = "return Dialogo();";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("Seguimiento"));
            @__ctrl.Click -= new System.EventHandler(this.lbSeguimiento_Click);
            @__ctrl.Click += new System.EventHandler(this.lbSeguimiento_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlAcreedor() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlAcreedor = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlAcreedor";
            @__ctrl.CssClass = "form-control input-sm";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlEjecutivo() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlEjecutivo = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlEjecutivo";
            @__ctrl.CssClass = "form-control input-sm";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlEdoCertificado() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlEdoCertificado = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlEdoCertificado";
            @__ctrl.CssClass = "form-control input-sm";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnBuscar() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnBuscar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnBuscar";
            @__ctrl.Text = "Buscar";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "btn btn-primary btn-success pull-right");
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnBuscar_Click);
            @__ctrl.Click += new System.EventHandler(this.btnBuscar_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.GridView @__BuildControlgridSeguimiento() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.gridSeguimiento = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "gridSeguimiento";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "table table-responsive table-hover table-bordered");
            @__ctrl.RowDataBound -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.gridSeguimiento_RowDataBound);
            @__ctrl.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.gridSeguimiento_RowDataBound);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Panel @__BuildControlpnFormulario() {
            global::System.Web.UI.WebControls.Panel @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Panel();
            this.pnFormulario = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "pnFormulario";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                    <div class=""row"">
                        <div class=""col-md-12 column form-horizontal"">
                            <div class=""row clearfix"">
                                <!-- col 1/4 -->
                                <div class=""col-md-3 col-sm-6"">
                                    <div class=""form-group"">
                                        <label for="""">Acreedor:</label>
                                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl1;
            @__ctrl1 = this.@__BuildControlddlAcreedor();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                                    </div>
                                </div>
                                <!-- Grupo 2 -->
                                <div class=""col-md-3 col-sm-6"">
                                    <div class=""form-group"">
                                        <label for="""">Ejecutivo:</label>
                                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl2;
            @__ctrl2 = this.@__BuildControlddlEjecutivo();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                                    </div>
                                </div>
                                <!-- Grupo 3 -->
                                <div class=""col-md-3 col-sm-6"">
                                    <div class=""form-group"">
                                        <label for="""">Estado Certificado:</label>
                                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl3;
            @__ctrl3 = this.@__BuildControlddlEdoCertificado();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                                    </div>
                                </div>
                                <!-- Grupo 4 -->
                                <div class=""col-md-3 col-sm-6"">
                                    <div class=""form-group"">
                                        "));
            global::System.Web.UI.WebControls.Button @__ctrl4;
            @__ctrl4 = this.@__BuildControlbtnBuscar();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class=""row"">
                        <div class=""col-md-12"">
                            <div class=""card-box"">
                                <div class=""table-responsive"">
                                    "));
            global::System.Web.UI.WebControls.GridView @__ctrl5;
            @__ctrl5 = this.@__BuildControlgridSeguimiento();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                                </div>\r\n                            </div>\r\n   " +
                        "                     </div>\r\n                    </div>\r\n                "));
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
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "btn btn-primary pull-right");
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnAtras_Click);
            @__ctrl.Click += new System.EventHandler(this.btnAtras_Click);
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
        <h4 class=""page-title"">Seguimiento</h4>
        <ol class=""breadcrumb"">
            <li>
                <a href=""#"">Inicio</a>
            </li>
            <li class=""active"">Seguimiento
            </li>
        </ol>
    </div>
</div>

"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControldvSuccess();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl2;
            @__ctrl2 = this.@__BuildControldvWarning();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl3;
            @__ctrl3 = this.@__BuildControldvError();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"

<!-- Contenedor Tabs -->
<div class=""row"">
    <div class=""col-md-12"">
        <ul class=""nav nav-tabs navtab-bg nav-justified"">
            <li class=""active"">
                <a href=""#tab1"" data-toggle=""tab"" aria-expanded=""true"">
                    <span class=""visible-xs""><i class=""fa fa-home""></i></span>
                    <span class=""hidden-xs"">
                        "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl4;
            @__ctrl4 = this.@__BuildControllbCalendarioPago();
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
            @__ctrl5 = this.@__BuildControllbSeguimiento();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </span></li>\r\n        </ul>\r\n\r\n        <div class=\"tab-cont" +
                        "ent\">\r\n            <div class=\"tab-pane active\" id=\"tab1\">\r\n\r\n                "));
            global::System.Web.UI.WebControls.Panel @__ctrl6;
            @__ctrl6 = this.@__BuildControlpnFormulario();
            @__parser.AddParsedSubObject(@__ctrl6);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n<br />\r\n\r\n<table wi" +
                        "dth=\"100%\">\r\n    <tr>\r\n        <td>\r\n            "));
            global::System.Web.UI.WebControls.Button @__ctrl7;
            @__ctrl7 = this.@__BuildControlbtnAtras();
            @__parser.AddParsedSubObject(@__ctrl7);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n        </td>\r\n\r\n        <td>\r\n            "));
            global::System.Web.UI.WebControls.Button @__ctrl8;
            @__ctrl8 = this.@__BuildControlbtnCancelar();
            @__parser.AddParsedSubObject(@__ctrl8);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n        </td>\r\n    </tr>\r\n</table>\r\n\r\n                                       " +
                        " "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::MultiOperacion.wpSeguimiento.wpSeguimiento.wpSeguimiento @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n<script type=\"text/javascript\" src=\"../_layouts/15/Multiaval/Scripts/wp/Ope" +
                        "racion.js\"></script>\r\n<script type=\"text/javascript\">\r\n    document.onkeydown = " +
                        "function () {\r\n        //109->\'-\' \r\n        //56->\'(\' \r\n        if (window.event" +
                        " && (window.event.keyCode == 109 || window.event.keyCode == 56)) {\r\n            " +
                        "window.event.keyCode = 505;\r\n        }\r\n\r\n        if (window.event.keyCode == 50" +
                        "5) {\r\n            return false;\r\n        }\r\n\r\n        if (window.event && (windo" +
                        "w.event.keyCode == 8)) {\r\n            valor = document.activeElement.value;\r\n   " +
                        "         if (valor == undefined) { return false; } //Evita Back en página. \r\n\r\n " +
                        "           else {\r\n                if (document.activeElement.getAttribute(\'type" +
                        "\') == \'select-one\')\r\n                { return false; } //Evita Back en select. \r" +
                        "\n                if (document.activeElement.getAttribute(\'type\') == \'button\')\r\n " +
                        "               { return false; } //Evita Back en button. \r\n                if (d" +
                        "ocument.activeElement.getAttribute(\'type\') == \'radio\')\r\n                { return" +
                        " false; } //Evita Back en radio. \r\n                if (document.activeElement.ge" +
                        "tAttribute(\'type\') == \'checkbox\')\r\n                { return false; } //Evita Bac" +
                        "k en checkbox. \r\n                if (document.activeElement.getAttribute(\'type\')" +
                        " == \'file\')\r\n                { return false; } //Evita Back en file. \r\n         " +
                        "       if (document.activeElement.getAttribute(\'type\') == \'reset\')\r\n            " +
                        "    { return false; } //Evita Back en reset. \r\n                if (document.acti" +
                        "veElement.getAttribute(\'type\') == \'submit\')\r\n                { return false; } /" +
                        "/Evita Back en submit. \r\n                if (document.activeElement.getAttribute" +
                        "(\'type\') == \'input\')\r\n                { return false; } //Evita Back en inout. \r" +
                        "\n                else //Text, textarea o password \r\n                {\r\n         " +
                        "           if (document.activeElement.value.length == 0)\r\n                    { " +
                        "return false; } //No realiza el backspace(largo igual a 0). \r\n                  " +
                        "  else {\r\n                        document.activeElement.value.keyCode = 8;\r\n   " +
                        "                 } //Realiza el backspace. \r\n                }\r\n            }\r\n\r" +
                        "\n            if (window.event && (window.event.keyCode == 13)) {\r\n              " +
                        "  valor = document.activeElement.value;\r\n                if (valor == undefined)" +
                        " { return false; } //Evita Enter en página. \r\n                else {\r\n          " +
                        "          if (document.activeElement.getAttribute(\'type\') == \'select-one\')\r\n    " +
                        "                { return false; } //Evita Enter en select. \r\n                   " +
                        " if (document.activeElement.getAttribute(\'type\') == \'button\')\r\n                 " +
                        "   { return false; } //Evita Enter en button. \r\n                    if (document" +
                        ".activeElement.getAttribute(\'type\') == \'radio\')\r\n                    { return fa" +
                        "lse; } //Evita Enter en radio. \r\n                    if (document.activeElement." +
                        "getAttribute(\'type\') == \'checkbox\')\r\n                    { return false; } //Evi" +
                        "ta Enter en checkbox. \r\n                    if (document.activeElement.getAttrib" +
                        "ute(\'type\') == \'file\')\r\n                    { return false; } //Evita Enter en f" +
                        "ile. \r\n                    if (document.activeElement.getAttribute(\'type\') == \'r" +
                        "eset\')\r\n                    { return false; } //Evita Enter en reset. \r\n        " +
                        "            if (document.activeElement.getAttribute(\'type\') == \'submit\')\r\n      " +
                        "              { return false; } //Evita Enter en submit. \r\n                    i" +
                        "f (document.activeElement.getAttribute(\'type\') == \'input\')\r\n                    " +
                        "{ return false; } //Evita Back en input. \r\n                    else //Text, text" +
                        "area o password \r\n                    {\r\n                        if (document.ac" +
                        "tiveElement.value.length == 0)\r\n                        { return false; } //No r" +
                        "ealiza el backspace(largo igual a 0). \r\n                        //else {\r\n      " +
                        "                  //    document.activeElement.value.keyCode = 13;\r\n            " +
                        "            //} //Realiza el enter. \r\n                    }\r\n                }\r\n" +
                        "            }\r\n        }\r\n    }\r\n\r\n</script>\r\n\r\n\r\n "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControldvWarning1();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl2;
            @__ctrl2 = this.@__BuildControldvFormulario();
            @__parser.AddParsedSubObject(@__ctrl2);
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
