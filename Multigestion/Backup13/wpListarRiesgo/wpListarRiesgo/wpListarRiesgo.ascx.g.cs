﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiRiesgo.wpListarRiesgo.wpListarRiesgo {
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
    using System.CodeDom.Compiler;
    
    
    public partial class wpListarRiesgo {
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Label lbWarning1;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning1;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Label lbSuccess;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvSuccess;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Label lbWarning;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.Label lbError;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvError;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.TextBox txtBuscar;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.LinkButton BtnBuscar;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.DropDownList cb_estados;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.DropDownList cb_etapa;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.DropDownList cb_subetapa;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.WebControls.GridView ResultadosBusqueda;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvFormulario;
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebPartCodeGenerator", "12.0.0.0")]
        public static implicit operator global::System.Web.UI.TemplateControl(wpListarRiesgo target) 
        {
            return target == null ? null : target.TemplateControl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.TextBox @__BuildControltxtBuscar() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.txtBuscar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "txtBuscar";
            @__ctrl.CssClass = "form-control";
            @__ctrl.TextMode = global::System.Web.UI.WebControls.TextBoxMode.Search;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.LinkButton @__BuildControlBtnBuscar() {
            global::System.Web.UI.WebControls.LinkButton @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.LinkButton();
            this.BtnBuscar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "BtnBuscar";
            @__ctrl.CssClass = "btn btn-default";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("<i class=\"fa fa-search\"></i>"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlcb_estados() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.cb_estados = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "cb_estados";
            @__ctrl.CssClass = "form-control";
            @__ctrl.AutoPostBack = true;
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            @__ctrl.SelectedIndexChanged -= new System.EventHandler(this.cb_estados_SelectedIndexChanged);
            @__ctrl.SelectedIndexChanged += new System.EventHandler(this.cb_estados_SelectedIndexChanged);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlcb_etapa() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.cb_etapa = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "cb_etapa";
            @__ctrl.CssClass = "form-control";
            @__ctrl.AutoPostBack = true;
            @__ctrl.SelectedIndexChanged -= new System.EventHandler(this.cb_etapa_SelectedIndexChanged);
            @__ctrl.SelectedIndexChanged += new System.EventHandler(this.cb_etapa_SelectedIndexChanged);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlcb_subetapa() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.cb_subetapa = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "cb_subetapa";
            @__ctrl.CssClass = "form-control";
            @__ctrl.AutoPostBack = true;
            @__ctrl.SelectedIndexChanged -= new System.EventHandler(this.cb_subetapa_SelectedIndexChanged);
            @__ctrl.SelectedIndexChanged += new System.EventHandler(this.cb_subetapa_SelectedIndexChanged);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControl__control2(System.Web.UI.WebControls.TableItemStyle @__ctrl) {
            @__ctrl.CssClass = "pagination-ys";
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private global::System.Web.UI.WebControls.GridView @__BuildControlResultadosBusqueda() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.ResultadosBusqueda = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ResultadosBusqueda";
            @__ctrl.GridLines = global::System.Web.UI.WebControls.GridLines.Horizontal;
            @__ctrl.RowHeaderColumn = "0";
            @__ctrl.ShowHeaderWhenEmpty = true;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "table table-bordered table-hover");
            @__ctrl.PageSize = 15;
            @__ctrl.AllowPaging = true;
            this.@__BuildControl__control2(@__ctrl.PagerStyle);
            @__ctrl.RowDataBound -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowDataBound);
            @__ctrl.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowDataBound);
            @__ctrl.RowCreated -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowCreated);
            @__ctrl.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowCreated);
            @__ctrl.DataBound -= new System.EventHandler(this.ResultadosBusqueda_DataBound);
            @__ctrl.DataBound += new System.EventHandler(this.ResultadosBusqueda_DataBound);
            @__ctrl.PageIndexChanging -= new System.Web.UI.WebControls.GridViewPageEventHandler(this.ResultadosBusqueda_PageIndexChanging);
            @__ctrl.PageIndexChanging += new System.Web.UI.WebControls.GridViewPageEventHandler(this.ResultadosBusqueda_PageIndexChanging);
            @__ctrl.PageIndexChanged -= new System.EventHandler(this.ResultadosBusqueda_PageIndexChanged);
            @__ctrl.PageIndexChanged += new System.EventHandler(this.ResultadosBusqueda_PageIndexChanged);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
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
        <h4 class=""page-title"">Riesgo</h4>
        <ol class=""breadcrumb"">
            <li>
                <a href=""#"">Inicio</a>
            </li>
            <li class=""active"">Riesgo
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



    <!-- filtros-->
    <div class=""row"">
        <div class=""col-md-12"">
            <div class=""card-box"">
                <div class=""row"">
                  
                        <div class=""col-md-3 col-sm-6"">
                            <label for="""">Buscar</label>
                            <div class=""input-group"">
                                "));
            global::System.Web.UI.WebControls.TextBox @__ctrl4;
            @__ctrl4 = this.@__BuildControltxtBuscar();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                                <span class=\"input-group-btn\">\r\n               " +
                        "                     "));
            global::System.Web.UI.WebControls.LinkButton @__ctrl5;
            @__ctrl5 = this.@__BuildControlBtnBuscar();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                                </span>\r\n                            </div>\r\n  " +
                        "                      </div>\r\n                        <div class=\"col-md-3 col-s" +
                        "m-6\">\r\n                            <label for=\"\">Estado</label>\r\n               " +
                        "             "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl6;
            @__ctrl6 = this.@__BuildControlcb_estados();
            @__parser.AddParsedSubObject(@__ctrl6);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        </div>\r\n                        <div class=\"col-md-3 co" +
                        "l-sm-6\">\r\n                            <label for=\"\">Etapa</label>\r\n             " +
                        "               "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl7;
            @__ctrl7 = this.@__BuildControlcb_etapa();
            @__parser.AddParsedSubObject(@__ctrl7);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        </div>\r\n                        <div class=\"col-md-3 co" +
                        "l-sm-6\">\r\n                            <label for=\"\">SubEtapa</label>\r\n          " +
                        "                  "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl8;
            @__ctrl8 = this.@__BuildControlcb_subetapa();
            @__parser.AddParsedSubObject(@__ctrl8);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                        </div>
                    
                </div>
            </div>
        </div>
    </div>

    <!-- tabla / grilla -->
    <div class=""row"">
        <div class=""col-md-12"">
            <table border=""0"" style=""table-layout:fixed; width:100%;"">
                <tr>
                    <td>
                        <div class=""card-box"">
                            <div class=""table-responsive"">
                                "));
            global::System.Web.UI.WebControls.GridView @__ctrl9;
            @__ctrl9 = this.@__BuildControlResultadosBusqueda();
            @__parser.AddParsedSubObject(@__ctrl9);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                        </div>\r\n\t\t\t\t\t</td>\r" +
                        "\n\t\t\t\t</tr>\r\n\t\t\t</table>\r\n        </div>\r\n    </div>\r\n\r\n"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__BuildControlTree(global::MultiRiesgo.wpListarRiesgo.wpListarRiesgo.wpListarRiesgo @__ctrl) {
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
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void @__Render__control1(System.Web.UI.HtmlTextWriter @__w, System.Web.UI.Control parameterContainer) {
            @__w.Write("\r\n\r\n<link rel=\"stylesheet\" href=\"../../_layouts/15/PagerStyle.css\" />\r\n<script ty" +
                    "pe=\"text/javascript\">\r\n    document.onkeydown = function () {\r\n\r\n        if (win" +
                    "dow.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {\r\n   " +
                    "         window.event.keyCode = 505;\r\n        }\r\n\r\n        if (window.event.keyC" +
                    "ode == 505) {\r\n            return false;\r\n        }\r\n\r\n        if (window.event " +
                    "&& (window.event.keyCode == 13)) {\r\n            valor = document.activeElement.v" +
                    "alue;\r\n            if (valor == undefined) { return false; } //Evita Enter en pá" +
                    "gina. \r\n            else {\r\n                if (document.activeElement.getAttrib" +
                    "ute(\'type\') == \'select-one\')\r\n                { return false; } //Evita Enter en" +
                    " select. \r\n                if (document.activeElement.getAttribute(\'type\') == \'b" +
                    "utton\')\r\n                { return false; } //Evita Enter en button. \r\n          " +
                    "      if (document.activeElement.getAttribute(\'type\') == \'radio\')\r\n             " +
                    "   { return false; } //Evita Enter en radio. \r\n                if (document.acti" +
                    "veElement.getAttribute(\'type\') == \'checkbox\')\r\n                { return false; }" +
                    " //Evita Enter en checkbox. \r\n                if (document.activeElement.getAttr" +
                    "ibute(\'type\') == \'file\')\r\n                { return false; } //Evita Enter en fil" +
                    "e. \r\n                if (document.activeElement.getAttribute(\'type\') == \'reset\')" +
                    "\r\n                { return false; } //Evita Enter en reset. \r\n                if" +
                    " (document.activeElement.getAttribute(\'type\') == \'submit\')\r\n                { re" +
                    "turn false; } //Evita Enter en submit. \r\n                if (document.activeElem" +
                    "ent.getAttribute(\'type\') == \'input\')\r\n                { return false; } //Evita " +
                    "Back en input. \r\n                else //Text, textarea o password \r\n            " +
                    "    {\r\n                    if (document.activeElement.value.length == 0)\r\n      " +
                    "              { return false; } //No realiza el backspace(largo igual a 0). \r\n\r\n" +
                    "                    //else {\r\n\r\n                    //    document.activeElement" +
                    ".value.keyCode = 13;\r\n\r\n                    //} //Realiza el enter. \r\n          " +
                    "      }\r\n\r\n                if (document.getElementById(\'");
                                     @__w.Write(txtBuscar.ClientID );

            @__w.Write("\').val != \'\') {\r\n                    document.getElementById(\'");
                                     @__w.Write(BtnBuscar.ClientID );

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

<script type=""text/javascript"" src=""../../_layouts/15/MultiRiesgo/wpEmpresasRelacionadas.js""></script>

");
            parameterContainer.Controls[0].RenderControl(@__w);
            @__w.Write("\r\n\r\n\t\t\t\t\t\t\t\t\t\r\n");
            parameterContainer.Controls[1].RenderControl(@__w);
            @__w.Write("\r\n");
        }
        
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        private void InitializeControl() {
            this.@__BuildControlTree(this);
            this.Load += new global::System.EventHandler(this.Page_Load);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual object Eval(string expression) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        [GeneratedCodeAttribute("Microsoft.VisualStudio.SharePoint.ProjectExtensions.CodeGenerators.SharePointWebP" +
            "artCodeGenerator", "12.0.0.0")]
        protected virtual string Eval(string expression, string format) {
            return global::System.Web.UI.DataBinder.Eval(this.Page.GetDataItem(), expression, format);
        }
    }
}
