<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Representantes.ascx.cs" Inherits="MultiFiscalia.Representantes.Representantes.Representantes" %>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>
									
<div id="dvFormulario" runat="server">

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Representantes</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Fiscalía</a>
                </li>
                <li class="active">Representantes
                </li>
            </ol>
        </div>
    </div>


    <!-- Empresa -->
    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-gray">
                <p>
                    <strong>Empresa:
                <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong> /
                <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                    / Negocio:
                <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <br />
    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
        <h4>
            <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
        </h4>
    </div>

    <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
        <h4>
            <asp:Label ID="lbWarning" runat="server" Text=""></asp:Label>
        </h4>
    </div>

    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
        <h4>
            <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
        </h4>
    </div>
    <br />

    <div class="row" id="divContenedor">
        <div class="col-md-12">
            <div class="row clearfix">
                <div class="form-horizontal" role="form">

                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="panel-heading" role="tab" id="Div1">
                                <h4 class="panel-title">
                                    <a role="button">Representantes
                                    </a>
                                </h4>
                            </div>
                        </div>
                    </div>

                    <div class="card-box">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Clase A</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlClaseA" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Clase B</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlClaseB" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                        </div>


                        <br />

                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" OnClick="btnGuardar_Click" />
                                </td>
                            </tr>
                        </table>

                    </div>





                </div>


            </div>
        </div>
    </div>

</div>
