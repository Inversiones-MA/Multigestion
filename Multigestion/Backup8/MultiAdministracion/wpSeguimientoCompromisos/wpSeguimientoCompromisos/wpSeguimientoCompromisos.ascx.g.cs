﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiAdministracion.wpSeguimientoCompromisos.wpSeguimientoCompromisos {
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
    
    
    public partial class wpSeguimientoCompromisos {
        
        protected global::System.Web.UI.WebControls.Label lbWarning1;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning1;
        
        protected global::System.Web.UI.WebControls.Label lbSuccess;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvSuccess;
        
        protected global::System.Web.UI.WebControls.Label lbWarning;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning;
        
        protected global::System.Web.UI.WebControls.Label lbError;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvError;
        
        protected global::System.Web.UI.WebControls.TextBox txtRazonSocial;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlMes;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEtapa;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlEjecutivo;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlCanal;
        
        protected global::System.Web.UI.WebControls.DropDownList ddlFondo;
        
        protected global::System.Web.UI.WebControls.Button BtnBuscar;
        
        protected global::System.Web.UI.WebControls.Button btnGuardarResumen;
        
        protected global::System.Web.UI.WebControls.GridView gvTotalizado;
        
        protected global::System.Web.UI.WebControls.Button btnImprimir;
        
        protected global::System.Web.UI.WebControls.Button btnReporte;
        
        protected global::System.Web.UI.WebControls.Button btnGuardarTodo;
        
        protected global::System.Web.UI.WebControls.GridView ResultadosBusqueda;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvFormulario;
        
        public static implicit operator global::System.Web.UI.TemplateControl(wpSeguimientoCompromisos target) 
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
        private global::System.Web.UI.WebControls.TextBox @__BuildControltxtRazonSocial() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.txtRazonSocial = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "txtRazonSocial";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("placeholder", "Empresa");
            @__ctrl.CssClass = "form-control";
            @__ctrl.TextMode = global::System.Web.UI.WebControls.TextBoxMode.Search;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlMes() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlMes = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlMes";
            @__ctrl.CssClass = "form-control";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlEtapa() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlEtapa = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlEtapa";
            @__ctrl.CssClass = "form-control";
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
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlCanal() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlCanal = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlCanal";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "form-control ");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.DropDownList @__BuildControlddlFondo() {
            global::System.Web.UI.WebControls.DropDownList @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.DropDownList();
            this.ddlFondo = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ddlFondo";
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "form-control");
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlBtnBuscar() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.BtnBuscar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "BtnBuscar";
            @__ctrl.Text = "Buscar";
            @__ctrl.CssClass = "btn btn-primary btn-success";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.BtnBuscar_Click);
            @__ctrl.Click += new System.EventHandler(this.BtnBuscar_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnGuardarResumen() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnGuardarResumen = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnGuardarResumen";
            @__ctrl.Text = "Guardar";
            @__ctrl.CssClass = "btn btn-default";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnGuardarResumen_Click);
            @__ctrl.Click += new System.EventHandler(this.btnGuardarResumen_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control3() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "IdControlPipeline";
            @__ctrl.Visible = false;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control4() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "ConceptoDetalle";
            @__ctrl.HeaderText = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control5() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "MontoOperacionCLP";
            @__ctrl.HeaderText = "Monto Operación";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control6() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "ComisionCLP";
            @__ctrl.HeaderText = "Comisión CLP";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.BoundField @__BuildControl__control7() {
            global::System.Web.UI.WebControls.BoundField @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.BoundField();
            @__ctrl.DataField = "PorcComision";
            @__ctrl.HeaderText = "% Comisión FLAT";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControl__control2(System.Web.UI.WebControls.DataControlFieldCollection @__ctrl) {
            global::System.Web.UI.WebControls.BoundField @__ctrl1;
            @__ctrl1 = this.@__BuildControl__control3();
            @__ctrl.Add(@__ctrl1);
            global::System.Web.UI.WebControls.BoundField @__ctrl2;
            @__ctrl2 = this.@__BuildControl__control4();
            @__ctrl.Add(@__ctrl2);
            global::System.Web.UI.WebControls.BoundField @__ctrl3;
            @__ctrl3 = this.@__BuildControl__control5();
            @__ctrl.Add(@__ctrl3);
            global::System.Web.UI.WebControls.BoundField @__ctrl4;
            @__ctrl4 = this.@__BuildControl__control6();
            @__ctrl.Add(@__ctrl4);
            global::System.Web.UI.WebControls.BoundField @__ctrl5;
            @__ctrl5 = this.@__BuildControl__control7();
            @__ctrl.Add(@__ctrl5);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.GridView @__BuildControlgvTotalizado() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.gvTotalizado = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "gvTotalizado";
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            @__ctrl.CssClass = "table table-responsive table-hover table-bordered";
            @__ctrl.EmptyDataText = "No Hay Registros";
            @__ctrl.AllowPaging = true;
            @__ctrl.PageSize = 50;
            @__ctrl.AutoGenerateColumns = false;
            this.@__BuildControl__control2(@__ctrl.Columns);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnImprimir() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnImprimir = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnImprimir";
            @__ctrl.Text = "Imprimir";
            @__ctrl.CssClass = "btn btn-default";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnImprimir_Click);
            @__ctrl.Click += new System.EventHandler(this.btnImprimir_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnReporte() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnReporte = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnReporte";
            @__ctrl.Text = "Reporte";
            @__ctrl.CssClass = "btn btn-default";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnReporte_Click);
            @__ctrl.Click += new System.EventHandler(this.btnReporte_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnGuardarTodo() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnGuardarTodo = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "btnGuardarTodo";
            @__ctrl.Text = "Guardar Todo";
            @__ctrl.CssClass = "btn btn-default";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnGuardarTodo_Click);
            @__ctrl.Click += new System.EventHandler(this.btnGuardarTodo_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.GridView @__BuildControlResultadosBusqueda() {
            global::System.Web.UI.WebControls.GridView @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.GridView();
            this.ResultadosBusqueda = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ResultadosBusqueda";
            @__ctrl.ClientIDMode = global::System.Web.UI.ClientIDMode.Static;
            @__ctrl.CssClass = "table table-bordered table-hover";
            @__ctrl.GridLines = global::System.Web.UI.WebControls.GridLines.Horizontal;
            @__ctrl.ShowHeaderWhenEmpty = true;
            @__ctrl.RowStyle.Font.Size = global::System.Web.UI.WebControls.FontUnit.Small;
            @__ctrl.RowDataBound -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowDataBound);
            @__ctrl.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowDataBound);
            @__ctrl.RowCreated -= new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowCreated);
            @__ctrl.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(this.ResultadosBusqueda_RowCreated);
            @__ctrl.PageIndexChanging -= new System.Web.UI.WebControls.GridViewPageEventHandler(this.ResultadosBusqueda_PageIndexChanging);
            @__ctrl.PageIndexChanging += new System.Web.UI.WebControls.GridViewPageEventHandler(this.ResultadosBusqueda_PageIndexChanging);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControldvFormulario() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.dvFormulario = @__ctrl;
            @__ctrl.ID = "dvFormulario";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n    <br />\r\n    "));
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

    <div class=""row"">
        <div class=""col-md-12"">
            <div class=""card-box"">
                <div class=""row"">

                    <div class=""col-md-3 col-sm-6"">
                        <label for="""">Empresa</label>
                        "));
            global::System.Web.UI.WebControls.TextBox @__ctrl4;
            @__ctrl4 = this.@__BuildControltxtRazonSocial();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        \r\n                    </div>\r\n                    <div " +
                        "class=\"col-md-3 col-sm-6\">\r\n                        <label for=\"\">Mes</label>\r\n " +
                        "                       "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl5;
            @__ctrl5 = this.@__BuildControlddlMes();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </div>\r\n                    <div class=\"col-md-3 col-sm-6\">" +
                        "\r\n                        <label for=\"\">Etapa</label>\r\n                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl6;
            @__ctrl6 = this.@__BuildControlddlEtapa();
            @__parser.AddParsedSubObject(@__ctrl6);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </div>\r\n                    <div class=\"col-md-3 col-sm-6\">" +
                        "\r\n                        <label>Ejecutivo</label>\r\n                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl7;
            @__ctrl7 = this.@__BuildControlddlEjecutivo();
            @__parser.AddParsedSubObject(@__ctrl7);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </div>\r\n\r\n                </div>\r\n                <div clas" +
                        "s=\"row\">\r\n\r\n                    <div class=\"col-md-3 col-sm-6\">\r\n               " +
                        "         <label for=\"\">Canal</label>\r\n                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl8;
            @__ctrl8 = this.@__BuildControlddlCanal();
            @__parser.AddParsedSubObject(@__ctrl8);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </div>\r\n                    <div class=\"col-md-3 col-sm-6\">" +
                        "\r\n                        <label>Fondo</label>\r\n                        "));
            global::System.Web.UI.WebControls.DropDownList @__ctrl9;
            @__ctrl9 = this.@__BuildControlddlFondo();
            @__parser.AddParsedSubObject(@__ctrl9);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </div>\r\n                    <div class=\"col-md-3 col-sm-6\" " +
                        "style=\"padding-top: 10px\">\r\n                        <br />\r\n                    " +
                        "    "));
            global::System.Web.UI.WebControls.Button @__ctrl10;
            @__ctrl10 = this.@__BuildControlBtnBuscar();
            @__parser.AddParsedSubObject(@__ctrl10);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END FILTROS-->

    <!-- tabla / grilla -->
    <div class=""row"">
        <div class=""col-md-12"">
            <div class=""card-box"">
                <div class=""table-responsive"">
                    <div style=""float: right; padding: 10px 5px;"">
                        "));
            global::System.Web.UI.WebControls.Button @__ctrl11;
            @__ctrl11 = this.@__BuildControlbtnGuardarResumen();
            @__parser.AddParsedSubObject(@__ctrl11);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </div>\r\n                    "));
            global::System.Web.UI.WebControls.GridView @__ctrl12;
            @__ctrl12 = this.@__BuildControlgvTotalizado();
            @__parser.AddParsedSubObject(@__ctrl12);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl(@"
                </div>
            </div>
        </div>
    </div>

    <div class=""row"">
        <div class=""col-md-12"">
            <table border=""0"" style=""table-layout: fixed; width: 100%;"">
                <tr>
                    <td>
                        <div class=""card-box"">
                            <div class=""table-responsive"">
                                <div style=""float: right; padding: 10px 5px;"">
                                    "));
            global::System.Web.UI.WebControls.Button @__ctrl13;
            @__ctrl13 = this.@__BuildControlbtnImprimir();
            @__parser.AddParsedSubObject(@__ctrl13);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                                    "));
            global::System.Web.UI.WebControls.Button @__ctrl14;
            @__ctrl14 = this.@__BuildControlbtnReporte();
            @__parser.AddParsedSubObject(@__ctrl14);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                                    "));
            global::System.Web.UI.WebControls.Button @__ctrl15;
            @__ctrl15 = this.@__BuildControlbtnGuardarTodo();
            @__parser.AddParsedSubObject(@__ctrl15);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                                </div>\r\n                                <br />\r" +
                        "\n                                <br />\r\n                                <br />\r" +
                        "\n                                "));
            global::System.Web.UI.WebControls.GridView @__ctrl16;
            @__ctrl16 = this.@__BuildControlResultadosBusqueda();
            @__parser.AddParsedSubObject(@__ctrl16);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                        </div>\r\n           " +
                        "         </td>\r\n                </tr>\r\n            </table>\r\n        </div>\r\n   " +
                        " </div>\r\n\r\n    \r\n"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::MultiAdministracion.wpSeguimientoCompromisos.wpSeguimientoCompromisos.wpSeguimientoCompromisos @__ctrl) {
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
            @__w.Write(@"

<link rel=""stylesheet"" href=""../../_layouts/15/PagerStyle.css"" />
<script type='text/javascript' src=""../../_layouts/15/MultiAdministracion/jquery-1.4.1.min.js""></script>


<style type=""text/css"">
    /*.WrapperDiv {
        width:auto;
    }        
    .WrapperDiv TH {
        position:relative;
    }
    .WrapperDiv TR 
    {
        height:0px;
    }*/ 
</style>

<script>
    $(document).ready(function () {
        $(""td.ms-dtinput a"").addClass(""btn btn-default"").html(""<i class='icon-calender'></i>"");

        if (typeof(Sys.Browser.WebKit) == ""undefined"") {
            Sys.Browser.WebKit = {};
        }

        if (navigator.userAgent.indexOf(""WebKit/"") > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version =
                parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = ""WebKit"";
        }
    });

   function ValidarColor(fila) {
        var i = parseInt(fila) + 1;

        var grid = document.getElementById('");
                                    @__w.Write(ResultadosBusqueda.ClientID );

            @__w.Write(@"');
        Row = grid.rows[i];

        //var a = Row.cells[10].childNodes[0].children[0].value();
        ///grid.rows[i].style.color = '#FE2E2E';
    }

    function solonumerosFechas(e) {
        var unicode = e.charCode ? e.charCode : e.keyCode
        if (unicode != 8) {
            if (unicode < 48 || unicode > 57)
                if (unicode != 45)
                    return false
        }
    }

    function Dialogo() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    }

  
</script>


");
            parameterContainer.Controls[0].RenderControl(@__w);
            @__w.Write("\r\n\r\n");
            parameterContainer.Controls[1].RenderControl(@__w);
            @__w.Write("\r\n\r\n\r\n\r\n\r\n\r\n");
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
