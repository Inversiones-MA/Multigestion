<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDocumentosCurse.ascx.cs" Inherits="MultiOperacion.wpDocumentosCurse.wpDocumentosCurse.wpDocumentosCurse" %>

<style type="text/css">
    .btn {
        font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important;
    }
</style>

<script type="text/javascript" src="../../_layouts/15/MultiOperacion/Operacion.js"></script>
<script type="text/javascript" src="../../_layouts/15/MultiOperacion/FuncionesCliente.js"></script>
<script type="text/javascript">
    document.onkeydown = function () {
        //109->'-' 
        //56->'(' 
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
        }

        if (window.event && (window.event.keyCode == 8)) {
            valor = document.activeElement.value;
            if (valor == undefined) { return false; } //Evita Back en página. 

            else {
                if (document.activeElement.getAttribute('type') == 'select-one')
                { return false; } //Evita Back en select. 
                if (document.activeElement.getAttribute('type') == 'button')
                { return false; } //Evita Back en button. 
                if (document.activeElement.getAttribute('type') == 'radio')
                { return false; } //Evita Back en radio. 
                if (document.activeElement.getAttribute('type') == 'checkbox')
                { return false; } //Evita Back en checkbox. 
                if (document.activeElement.getAttribute('type') == 'file')
                { return false; } //Evita Back en file. 
                if (document.activeElement.getAttribute('type') == 'reset')
                { return false; } //Evita Back en reset. 
                if (document.activeElement.getAttribute('type') == 'submit')
                { return false; } //Evita Back en submit. 
                if (document.activeElement.getAttribute('type') == 'input')
                { return false; } //Evita Back en inout. 
                else //Text, textarea o password 
                {
                    if (document.activeElement.value.length == 0)
                    { return false; } //No realiza el backspace(largo igual a 0). 
                    else {
                        document.activeElement.value.keyCode = 8;
                    } //Realiza el backspace. 
                }
            }

            if (window.event && (window.event.keyCode == 13)) {
                valor = document.activeElement.value;
                if (valor == undefined) { return false; } //Evita Enter en página. 
                else {
                    if (document.activeElement.getAttribute('type') == 'select-one')
                    { return false; } //Evita Enter en select. 
                    if (document.activeElement.getAttribute('type') == 'button')
                    { return false; } //Evita Enter en button. 
                    if (document.activeElement.getAttribute('type') == 'radio')
                    { return false; } //Evita Enter en radio. 
                    if (document.activeElement.getAttribute('type') == 'checkbox')
                    { return false; } //Evita Enter en checkbox. 
                    if (document.activeElement.getAttribute('type') == 'file')
                    { return false; } //Evita Enter en file. 
                    if (document.activeElement.getAttribute('type') == 'reset')
                    { return false; } //Evita Enter en reset. 
                    if (document.activeElement.getAttribute('type') == 'submit')
                    { return false; } //Evita Enter en submit. 
                    if (document.activeElement.getAttribute('type') == 'input')
                    { return false; } //Evita Back en input. 
                    else //Text, textarea o password 
                    {
                        if (document.activeElement.value.length == 0)
                        { return false; } //No realiza el backspace(largo igual a 0). 
                        //else {
                        //    document.activeElement.value.keyCode = 13;
                        //} //Realiza el enter. 
                    }
                }
            }
        }
    }

</script>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Documentos Curse</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Operaciones</a>
                </li>
                <li class="active">Documentos Curse
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

    <!-- tabs -->
    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li>
                    <a href="#tab1" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbDistribucionFondo" runat="server" OnClientClick="return Dialogo();" OnClick="lbDistribucionFondo_Click">Distribución de Fondos</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#tab2" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lblDocumentoCurse" runat="server" OnClientClick="return Dialogo();" OnClick="lblDocumentoCurse_Click">Documentos de Curse</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane active" id="tab2">
                    <div class="row">
                        <!-- col 1/4 -->
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label for="">Acreedor:</label>
                                <asp:TextBox ID="txtAcreedor" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <!-- col 2/4 -->
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label for="">Tipo de Producto:</label>
                                <asp:TextBox ID="txtTipoProducto" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class="row">
                        <div class="col-md-12">
                            <table border="0" style="table-layout: fixed; width: 100%;">
                                <tr>
                                    <td>
                                        <div class="card-box">
                                            <div class="table-responsive" id="documentos" runat="server">
                                                <asp:GridView ID="gridDocumentosCurse" runat="server" GridLines="Horizontal" ShowHeaderWhenEmpty="true"
                                                    class="table table-bordered table-hover" OnRowCreated="gridDocumentosCurse_RowCreated" OnRowDataBound="gridDocumentosCurse_RowDataBound">
                                                    <HeaderStyle Height="40px" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <div id="DivArchivo" runat="server" visible="false">
                        <table class="table grid">
                            <tr>
                                <td style="text-align: right">
                                    <div id="NombreArchivo" style="text-align: right"></div>

                                </td>
                                <td style="text-align: left">
                                    <asp:FileUpload ID="fileDocumento" runat="server" /><br />
                                    <%--onchange="return CheckExtension(this);"--%>
                                    <span style="color: red;" id="errorArchivo"></span>
                                    <script>
                                        var validFilesTypes = ["pdf"];

                                        function CheckExtension(file) {
                                            /*global document: false */
                                            file.style.borderColor = "";
                                            document.getElementById('errorArchivo').innerHTML = "";
                                            var filePath = file.value;
                                            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
                                            var isValidFile = false;

                                            for (var i = 0; i < validFilesTypes.length; i++) {
                                                if (ext == validFilesTypes[i]) {
                                                    isValidFile = true;
                                                    break;
                                                }
                                            }

                                            if (!isValidFile) {
                                                file.value = null;
                                                file.style.borderColor = "red";
                                                document.getElementById('errorArchivo').innerHTML = "Solo formato pdf";
                                            }

                                            return isValidFile;
                                        }
                                    </script>

                                </td>
                                <td style="text-align: left">
                                    <asp:Button ID="btnAdjuntar" runat="server" Text="Adjuntar" class="btn btn-primary" OnClientClick="return Dialogo();" OnClick="btnAdjuntar_Click1" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="botones" runat="server" visible="true">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                                </td>

                                <td>
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                                    <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" OnClick="btnFinalizar_Click" />

                                    <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="return Dialogo();" OnClick="btnGuardar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>


            </div>
        </div>
    </div>


</div>





