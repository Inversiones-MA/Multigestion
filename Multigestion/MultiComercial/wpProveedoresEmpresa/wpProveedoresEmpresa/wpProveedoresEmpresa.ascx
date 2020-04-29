<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpProveedoresEmpresa.ascx.cs" Inherits="MultiComercial.wpProveedoresEmpresa.wpProveedoresEmpresa.wpProveedoresEmpresa" %>


<script src="../../_layouts/15/MultiComercial/wpValidaciones.js"></script>

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
                    <a href="#">Datos Adm. Empresa</a>
                </li>
                <li class="active">Proveedores
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
                    <%--/ Negocio:
                <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>--%>
                / Ejecutivo:
               
                    <asp:Label ID="lbEjecutivo" runat="server" Text=" "></asp:Label>
                </p>
            </div>
        </div>
    </div>


    <!-- Contenedor Tabs -->
    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li class="">
                    <a href="#tab1" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton8" runat="server" OnClientClick="return Dialogo();" OnClick="lbProrrateo_Click">Prorrateo de Garantías</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton9" runat="server" OnClientClick="return Dialogo();" OnClick="lbDeudaSBIF_Click">Deuda SBIF</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab3" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton10" runat="server" OnClientClick="return Dialogo();" OnClick="lbClientes_Click">Clientes</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab4" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton11" runat="server" OnClientClick="return Dialogo();" OnClick="lbAdministracion_Click">Administración</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#tab5" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton12" runat="server" OnClientClick="return Dialogo();" OnClick="lbProveedores_Click">Proveedores</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>

            <div class="tab-content">
                <div class="tab-pane active" id="tab4">

                    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
                        <h4>
                            <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>

                    <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
                        <h4>
                            <asp:Label ID="lbWarning" runat="server" Text=" "></asp:Label>
                        </h4>
                    </div>

                    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
                        <h4>
                            <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>

                    <!-- filtros-->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <div class="row">

                                    <asp:Panel ID="pnFormulario" runat="server">
                                        <div class="form-horizontal" role="form">
                                            <!-- Grupo 1 -->
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="inputPassword3" class="col-sm-5 control-label">RUT:</label>
                                                    <div class="col-sm-7">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txt_rut" runat="server" MaxLength="8" CssClass="form-control" Width="155px" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_divRut" runat="server" MaxLength="1" CssClass="form-control" Width="30px" onKeyPress="return solonumerosDV(event)"></asp:TextBox>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td>
                                                                    <asp:LinkButton ID="ImageButton2" runat="server" CssClass="btn btn-default" OnClick="ImageButton2_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-5 control-label">Nombre o Razón Social:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txt_nombre" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- Grupo 2 -->
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label for="inputEmail3" class="col-sm-5 control-label">Porcentaje Ventas:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txt_porcVentas" runat="server" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="inputPassword3" class="col-sm-5 control-label">Plazo Convenio Pago:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txt_plazo" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <br>
                                <table style="width:100%">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                                            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="table-layout: fixed; width: 100%;">
                                    <tr>
                                        <td>
                                            <div class="table-responsive">
                                                <div id="dv_buscarEmpresas" runat="server" class="row clearfix">
                                                    <div class="col-md-12 column">
                                                        <br />
                                                        <asp:HiddenField runat="server" ID="hdnIdEmpresaProvedores" />
                                                        <asp:GridView ID="ResultadosBusqueda" runat="server" class="table table-bordered table-hover" GridLines="Horizontal" OnRowCreated="ResultadosBusqueda_RowCreated" OnRowDataBound="ResultadosBusqueda_RowDataBound" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
