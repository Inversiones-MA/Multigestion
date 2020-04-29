<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpTest.ascx.cs" Inherits="MultiAdministracion.wpTest.wpTest.wpTest" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>





    <!-- tabla / grilla -->
   
    <div class="row">            						
        <div class="col-md-12">
            <div class="card-box">
               
                    <asp:GridView ID="myGridWiew" runat="server" DataSourceID="SqlDataSourcePerfiles"
                        AutoGenerateColumns="true"
                        CssClass="table table-bordered table-hover table-responsive"
                        ></asp:GridView>
			  
            </div>
        </div>
    </div>
   
    
    <asp:SqlDataSource 
    ID="SqlDataSourcePerfiles" 
    runat="server" 
    ConnectionString="<%$ ConnectionStrings:DACConnectionString %>"
    SelectCommand="select top 10 *, *, * from view_perfiles order by 3, 4, 5, 6, 7" SelectCommandType="Text">
    </asp:SqlDataSource>
