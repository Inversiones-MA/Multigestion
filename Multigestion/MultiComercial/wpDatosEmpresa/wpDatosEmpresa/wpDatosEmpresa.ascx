<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDatosEmpresa.ascx.cs" Inherits="MultiComercial.wpDatosEmpresa.wpDatosEmpresa.wpDatosEmpresa" %>

<script src="../../_layouts/15/MultiComercial/Validaciones.js"></script>
<script type="text/javascript" src="../../_layouts/15/MultiComercial/wpSociosAccionistas.js"></script>
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
        <h4 class="page-title">Comercial</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Datos empresa</a>
            </li>
            <li class="active">Historia Empresa
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
                / Ejecutivo:
                <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
            </p>
        </div>
    </div>
</div>


<!-- Contenedor Tabs -->
<div class="row">
    <div class="col-xs-12 col-md-12">
        <ul class="nav nav-tabs navtab-bg nav-justified">
            <li class="">
                <a href="#tab1" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbDatosEmpresa" runat="server" OnClick="lbDatosEmpresa_Click" OnClientClick="return Dialogo();">Datos Empresa</asp:LinkButton>
                    </span>
                </a>
            </li>
            <li class="active">
                <a href="#tab2" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbHistoria" runat="server" OnClientClick="return Dialogo();" OnClick="lbHistoria_Click">Historia Empresa</asp:LinkButton>
                    </span>
                </a>
            </li>
            <li class="">
                <a href="#tab3" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbDocumentos" runat="server" OnClick="lbDocumentos_Click">Documentos</asp:LinkButton>
                    </span>
                </a>
            </li>
            <li class="">
                <a href="#tab4" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbEmpresaRelacionada" runat="server" OnClientClick="return Dialogo();" OnClick="lbEmpresaRelacionada_Click">Empresas Relacionadas</asp:LinkButton>
                    </span>
                </a>
            </li>
            <li class="">
                <a href="#tab5" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbDireccion" runat="server" OnClientClick="return Dialogo();" OnClick="lbDireccion_Click">Direcciones</asp:LinkButton>
                    </span>
                </a>
            </li>
            <li class="">
                <a href="#tab6" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbPersonas" runat="server" OnClientClick="return Dialogo();" OnClick="lbPersonas_Click">Personas</asp:LinkButton>
                    </span>
                </a>
            </li>
        </ul>

        <div class="tab-content">
            <div class="tab-pane active" id="tab2">

                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <div class="row">
                                
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

                                    <asp:LinkButton ID="lbEditar" runat="server" Text="Editar" CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"></asp:LinkButton>

                                    <asp:Panel ID="pnFormulario" runat="server">
                                        <div class="form-horizontal" role="form">
                                            <div class="row clearfix">
                                                <div class="col-md-6 column">
                                                    <div class="form-group">
                                                        <label class="col-sm-5 col-md-4 col-lg-4" for="inputEmail3">Años de Experiencia en el negocio </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtAnos" runat="server" Width="60px" MaxLength="3" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <!-- LABEL comentario dentro del DIV columna izquierda -->

                                                </div>

                                            </div>
                                            <div class="row clearfix">
                                                <div class="col-xs-10 col-md-12 column">
                                                    <div class="form-group">

                                                        <label class="col-xs-6 col-md-2" for="inputEmail3">Historia de la Empresa </label>
                                                        <div class="col-xs-9 col-sm-6 col-md-9">
                                                            <asp:TextBox ID="txtHistoria" TextMode="multiline" Rows="15" runat="server" CssClass="form-control" style="max-width:800px;" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                             <div class="row clearfix">
                                                <div class="col-xs-10 col-md-12 column">
                                                    <div class="form-group">

                                                        <label class="col-xs-6 col-md-2" for="inputEmail3">Antecedentes adicionales de la empresa</label>
                                                        <div class="col-xs-9 col-sm-6 col-md-9">
                                                            <asp:TextBox ID="txtAntecedentesEmpresa" TextMode="multiline" Rows="5" runat="server" CssClass="form-control" style="max-width:800px;" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>

                <table style="width:100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                        </td>

                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                            <asp:Button ID="btnCancel" runat="server" Text="Limpiar" class="btn btn-warning pull-right" />

                            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

    </div>
</div>
    </div>

