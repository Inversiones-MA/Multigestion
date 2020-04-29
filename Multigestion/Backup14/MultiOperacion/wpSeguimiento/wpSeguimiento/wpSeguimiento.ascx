<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpSeguimiento.ascx.cs" Inherits="MultiOperacion.wpSeguimiento.wpSeguimiento.wpSeguimiento" %>


<script type="text/javascript" src="../_layouts/15/Multiaval/Scripts/wp/Operacion.js"></script>
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
        <h4 class="page-title">Seguimiento</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Inicio</a>
            </li>
            <li class="active">Seguimiento
            </li>
        </ol>
    </div>
</div>

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

<!-- Contenedor Tabs -->
<div class="row">
    <div class="col-md-12">
        <ul class="nav nav-tabs navtab-bg nav-justified">
            <li class="active">
                <a href="#tab1" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbCalendarioPago" runat="server" OnClientClick="return Dialogo();" OnClick="lbCalendarioPago_Click">Calendario de Pago</asp:LinkButton>
                    </span>
                </a>

            </li>
            <li class="">
                <a href="#tab2" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbSeguimiento" runat="server" OnClientClick="return Dialogo();" OnClick="lbSeguimiento_Click">Seguimiento</asp:LinkButton>
                    </span></li>
        </ul>

        <div class="tab-content">
            <div class="tab-pane active" id="tab1">

                <asp:Panel ID="pnFormulario" runat="server">
                    <div class="row">
                        <div class="col-md-12 column form-horizontal">
                            <div class="row clearfix">
                                <!-- col 1/4 -->
                                <div class="col-md-3 col-sm-6">
                                    <div class="form-group">
                                        <label for="">Acreedor:</label>
                                        <asp:DropDownList ID="ddlAcreedor" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- Grupo 2 -->
                                <div class="col-md-3 col-sm-6">
                                    <div class="form-group">
                                        <label for="">Ejecutivo:</label>
                                        <asp:DropDownList ID="ddlEjecutivo" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- Grupo 3 -->
                                <div class="col-md-3 col-sm-6">
                                    <div class="form-group">
                                        <label for="">Estado Certificado:</label>
                                        <asp:DropDownList ID="ddlEdoCertificado" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <!-- Grupo 4 -->
                                <div class="col-md-3 col-sm-6">
                                    <div class="form-group">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-success pull-right" OnClick="btnBuscar_Click" OnClientClick="return Dialogo();" />
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <div class="table-responsive">
                                    <asp:GridView ID="gridSeguimiento" runat="server"
                                        class="table table-responsive table-hover table-bordered" OnRowDataBound="gridSeguimiento_RowDataBound">
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
</div>


<br />

<table width="100%">
    <tr>
        <td>
            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click1" />
        </td>

        <td>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

        </td>
    </tr>
</table>

                                        </div>