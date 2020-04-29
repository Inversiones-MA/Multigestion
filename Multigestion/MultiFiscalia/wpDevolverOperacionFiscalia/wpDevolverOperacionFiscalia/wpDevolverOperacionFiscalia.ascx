<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDevolverOperacionFiscalia.ascx.cs" Inherits="MultiFiscalia.wpDevolverOperacionFiscalia.wpDevolverOperacionFiscalia.wpDevolverOperacionFiscalia" %>

<script type="text/javascript" src="../../_layouts/15/MultiFiscalia/Fiscalia.js"></script>
<script type="text/javascript" src="../../_layouts/15/MultiFiscalia/FuncionesCliente.js"></script>

<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Devolver Operación Fiscalía</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Fiscalía</a>
                </li>
                <li class="active">Devolver Operación Fiscalía
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
                        <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong>
                    /
                    <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div id="dvSuccess" runat="server" class="alert alert-success">
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


                <asp:Panel ID="Panel1" runat="server">

                    <div class="col-md-12" id="tbRiesgo" runat="server">
                        <div class="form-group">
                            <label>Comentarios/Motivo:</label>
                            <asp:TextBox ID="txtComentarios" TextMode="multiline" Columns="20" Rows="10" runat="server" CssClass="form-control" MaxLength="1000" />
                        </div>
                    </div>

                </asp:Panel>

                <table style="width: 100%">
                    <tr>
                        <td>
                            <div>
                                <asp:Button class="btn btn-default" ID="btnCancel" runat="server" Text=" < Volver Atrás" OnClick="btnCancel_Click" OnClientClick="return Dialogo();" />
                            </div>
                        </td>
                        <td>
                            <div>
                                <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Actualizar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>

</div>
