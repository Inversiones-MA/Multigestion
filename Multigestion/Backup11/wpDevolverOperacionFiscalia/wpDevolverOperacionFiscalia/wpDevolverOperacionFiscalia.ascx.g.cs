﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiFiscalia.wpDevolverOperacionFiscalia.wpDevolverOperacionFiscalia {
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
    
    
    public partial class wpDevolverOperacionFiscalia {
        
        protected global::System.Web.UI.WebControls.Label lbWarning1;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning1;
        
        protected global::System.Web.UI.WebControls.Label lbEmpresa;
        
        protected global::System.Web.UI.WebControls.Label lbRut;
        
        protected global::System.Web.UI.WebControls.Label lbOperacion;
        
        protected global::System.Web.UI.WebControls.Label lbEjecutivo;
        
        protected global::System.Web.UI.WebControls.Label lbSuccess;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvSuccess;
        
        protected global::System.Web.UI.WebControls.Label lbWarning;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning;
        
        protected global::System.Web.UI.WebControls.Label lbError;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvError;
        
        protected global::System.Web.UI.WebControls.TextBox txtComentarios;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl tbRiesgo;
        
        protected global::System.Web.UI.WebControls.Panel Panel1;
        
        protected global::System.Web.UI.WebControls.Button btnCancel;
        
        protected global::System.Web.UI.WebControls.Button btnGuardar;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvFormulario;
        
        public static implicit operator global::System.Web.UI.TemplateControl(wpDevolverOperacionFiscalia target) 
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
        private global::System.Web.UI.WebControls.Label @__BuildControllbEmpresa() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbEmpresa = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbEmpresa";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbRut() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbRut = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbRut";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbOperacion() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbOperacion = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbOperacion";
            @__ctrl.Text = "";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Label @__BuildControllbEjecutivo() {
            global::System.Web.UI.WebControls.Label @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Label();
            this.lbEjecutivo = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "lbEjecutivo";
            @__ctrl.Text = "";
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
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "alert alert-success");
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    <h4>\r\n                        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbSuccess();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </h4>\r\n                "));
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    <h4>\r\n                        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbWarning();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </h4>\r\n                "));
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    <h4>\r\n                        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbError();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    </h4>\r\n                "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.TextBox @__BuildControltxtComentarios() {
            global::System.Web.UI.WebControls.TextBox @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.TextBox();
            this.txtComentarios = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "txtComentarios";
            @__ctrl.TextMode = global::System.Web.UI.WebControls.TextBoxMode.MultiLine;
            @__ctrl.Columns = 20;
            @__ctrl.Rows = 10;
            @__ctrl.CssClass = "form-control";
            @__ctrl.MaxLength = 1000;
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.HtmlControls.HtmlGenericControl @__BuildControltbRiesgo() {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl;
            @__ctrl = new global::System.Web.UI.HtmlControls.HtmlGenericControl("div");
            this.tbRiesgo = @__ctrl;
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "col-md-12");
            @__ctrl.ID = "tbRiesgo";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        <div class=\"form-group\">\r\n                            <" +
                        "label>Comentarios/Motivo:</label>\r\n                            "));
            global::System.Web.UI.WebControls.TextBox @__ctrl1;
            @__ctrl1 = this.@__BuildControltxtComentarios();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                        </div>\r\n                    "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Panel @__BuildControlPanel1() {
            global::System.Web.UI.WebControls.Panel @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Panel();
            this.Panel1 = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "Panel1";
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                    "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControltbRiesgo();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnCancel() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnCancel = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "btn btn-default");
            @__ctrl.ID = "btnCancel";
            @__ctrl.Text = " < Volver Atrás";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnCancel_Click);
            @__ctrl.Click += new System.EventHandler(this.btnCancel_Click);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.WebControls.Button @__BuildControlbtnGuardar() {
            global::System.Web.UI.WebControls.Button @__ctrl;
            @__ctrl = new global::System.Web.UI.WebControls.Button();
            this.btnGuardar = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            ((System.Web.UI.IAttributeAccessor)(@__ctrl)).SetAttribute("class", "btn btn-primary btn-success pull-right");
            @__ctrl.ID = "btnGuardar";
            @__ctrl.Text = "Actualizar";
            @__ctrl.OnClientClick = "return Dialogo();";
            @__ctrl.Click -= new System.EventHandler(this.btnGuardar_Click);
            @__ctrl.Click += new System.EventHandler(this.btnGuardar_Click);
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
            <h4 class=""page-title"">Devolver Operación Fiscalía</h4>
            <ol class=""breadcrumb"">
                <li>
                    <a href=""#"">Fiscalía</a>
                </li>
                <li class=""active"">Devolver Operación Fiscalía
                </li>
            </ol>
        </div>
    </div>

    <!-- Empresa -->
    <div class=""row"">
        <div class=""col-md-12"">
            <div class=""alert alert-gray"">
                <p>
                    <strong>Empresa:
                        "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbEmpresa();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("</strong>\r\n                    /\r\n                    "));
            global::System.Web.UI.WebControls.Label @__ctrl2;
            @__ctrl2 = this.@__BuildControllbRut();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    / Negocio:\r\n                    "));
            global::System.Web.UI.WebControls.Label @__ctrl3;
            @__ctrl3 = this.@__BuildControllbOperacion();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                    / Ejecutivo:\r\n                    "));
            global::System.Web.UI.WebControls.Label @__ctrl4;
            @__ctrl4 = this.@__BuildControllbEjecutivo();
            @__parser.AddParsedSubObject(@__ctrl4);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                </p>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n    <di" +
                        "v class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <div class=\"card-bo" +
                        "x\">\r\n                "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl5;
            @__ctrl5 = this.@__BuildControldvSuccess();
            @__parser.AddParsedSubObject(@__ctrl5);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl6;
            @__ctrl6 = this.@__BuildControldvWarning();
            @__parser.AddParsedSubObject(@__ctrl6);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                "));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl7;
            @__ctrl7 = this.@__BuildControldvError();
            @__parser.AddParsedSubObject(@__ctrl7);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n                "));
            global::System.Web.UI.WebControls.Panel @__ctrl8;
            @__ctrl8 = this.@__BuildControlPanel1();
            @__parser.AddParsedSubObject(@__ctrl8);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n                <table style=\"width: 100%\">\r\n                    <tr>\r\n      " +
                        "                  <td>\r\n                            <div>\r\n                     " +
                        "           "));
            global::System.Web.UI.WebControls.Button @__ctrl9;
            @__ctrl9 = this.@__BuildControlbtnCancel();
            @__parser.AddParsedSubObject(@__ctrl9);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                        </td>\r\n            " +
                        "            <td>\r\n                            <div>\r\n                           " +
                        "     "));
            global::System.Web.UI.WebControls.Button @__ctrl10;
            @__ctrl10 = this.@__BuildControlbtnGuardar();
            @__parser.AddParsedSubObject(@__ctrl10);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n                            </div>\r\n                        </td>\r\n            " +
                        "        </tr>\r\n                </table>\r\n\r\n            </div>\r\n        </div>\r\n " +
                        "   </div>\r\n\r\n"));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::MultiFiscalia.wpDevolverOperacionFiscalia.wpDevolverOperacionFiscalia.wpDevolverOperacionFiscalia @__ctrl) {
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n<script type=\"text/javascript\" src=\"../../_layouts/15/MultiFiscalia/Fiscalia." +
                        "js\"></script>\r\n<script type=\"text/javascript\" src=\"../../_layouts/15/MultiFiscal" +
                        "ia/FuncionesCliente.js\"></script>\r\n\r\n"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControldvWarning1();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n"));
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl2;
            @__ctrl2 = this.@__BuildControldvFormulario();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n"));
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