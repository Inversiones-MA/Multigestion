﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultiReportes.wpReporte1.wpReporte1 {
    using System.Web.UI.WebControls.Expressions;
    using System.Collections;
    using System.Text;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.SharePoint.WebControls;
    using System.Collections.Specialized;
    using System.Web.SessionState;
    using Microsoft.Reporting.WebForms;
    using Microsoft.SharePoint.WebPartPages;
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
    using System.Configuration;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;
    
    
    public partial class wpReporte1 {
        
        protected global::System.Web.UI.WebControls.Label lbWarning1;
        
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl dvWarning1;
        
        protected global::System.Web.UI.ScriptManagerProxy ScriptManagerProxy1;
        
        protected global::Microsoft.Reporting.WebForms.ReportViewer ReportViewer1;
        
        public static implicit operator global::System.Web.UI.TemplateControl(wpReporte1 target) 
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
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n      <h4>\r\n         "));
            global::System.Web.UI.WebControls.Label @__ctrl1;
            @__ctrl1 = this.@__BuildControllbWarning1();
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n     </h4>\r\n "));
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::System.Web.UI.ScriptManagerProxy @__BuildControlScriptManagerProxy1() {
            global::System.Web.UI.ScriptManagerProxy @__ctrl;
            @__ctrl = new global::System.Web.UI.ScriptManagerProxy();
            this.ScriptManagerProxy1 = @__ctrl;
            @__ctrl.ID = "ScriptManagerProxy1";
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControl__control2(Microsoft.Reporting.WebForms.ServerReport @__ctrl) {
            @__ctrl.ReportServerUrl = new System.Uri("", global::System.UriKind.Relative);
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private global::Microsoft.Reporting.WebForms.ReportViewer @__BuildControlReportViewer1() {
            global::Microsoft.Reporting.WebForms.ReportViewer @__ctrl;
            @__ctrl = new global::Microsoft.Reporting.WebForms.ReportViewer();
            this.ReportViewer1 = @__ctrl;
            @__ctrl.ApplyStyleSheetSkin(this.Page);
            @__ctrl.ID = "ReportViewer1";
            @__ctrl.Font.Names = new string[] {
                    "Verdana"};
            @__ctrl.Font.Size = new System.Web.UI.WebControls.FontUnit(new System.Web.UI.WebControls.Unit(8D, global::System.Web.UI.WebControls.UnitType.Point));
            @__ctrl.ProcessingMode = global::Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            @__ctrl.WaitMessageFont.Names = new string[] {
                    "Verdana"};
            @__ctrl.WaitMessageFont.Size = new System.Web.UI.WebControls.FontUnit(new System.Web.UI.WebControls.Unit(14D, global::System.Web.UI.WebControls.UnitType.Point));
            this.@__BuildControl__control2(@__ctrl.ServerReport);
            return @__ctrl;
        }
        
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void @__BuildControlTree(global::MultiReportes.wpReporte1.wpReporte1.wpReporte1 @__ctrl) {
            global::System.Web.UI.HtmlControls.HtmlGenericControl @__ctrl1;
            @__ctrl1 = this.@__BuildControldvWarning1();
            System.Web.UI.IParserAccessor @__parser = ((System.Web.UI.IParserAccessor)(@__ctrl));
            @__parser.AddParsedSubObject(@__ctrl1);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n"));
            global::System.Web.UI.ScriptManagerProxy @__ctrl2;
            @__ctrl2 = this.@__BuildControlScriptManagerProxy1();
            @__parser.AddParsedSubObject(@__ctrl2);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n"));
            global::Microsoft.Reporting.WebForms.ReportViewer @__ctrl3;
            @__ctrl3 = this.@__BuildControlReportViewer1();
            @__parser.AddParsedSubObject(@__ctrl3);
            @__parser.AddParsedSubObject(new System.Web.UI.LiteralControl("\r\n\r\n\r\n\r\n"));
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
