<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDeudaSBIF.ascx.cs" Inherits="MultiComercial.wpDeudaSBIF.wpDeudaSBIF.wpDeudaSBIF" %>

<script src="../../_layouts/15/MultiComercial/wpValidaciones.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    });

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
            <h4 class="page-title">Deuda SBIF</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Datos Adm. Empresa</a>
                </li>
                <li class="active">Deuda SBIF
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
                    <%--/ Negocio: <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>--%>
                / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=" "></asp:Label>
                </p>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li class="">
                    <a href="#tab1" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbProrrateo" runat="server" OnClientClick="return Dialogo();" OnClick="lbProrrateo_Click">Prorrateo de Garantías</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#tab2" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbDeudaSBIF" runat="server" OnClientClick="return Dialogo();" OnClick="lbDeudaSBIF_Click">Deuda SBIF</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab3" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbClientes" runat="server" OnClientClick="return Dialogo();" OnClick="lbClientes_Click">Clientes</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab4" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbAdministracion" runat="server" OnClientClick="return Dialogo();" OnClick="lbAdministracion_Click">Administración</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab5" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbProveedores" runat="server" OnClientClick="return Dialogo();" OnClick="lbProveedores_Click">Proveedores</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>

            <div class="tab-content">
                <div class="tab-pane active" id="tab2">
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

                    <div class="row">
                        <div class="col-md-12">
                            <ul class="nav nav-tabs navtab-bg nav-justified">
                                <li id="li1" runat="server" class="active"><a href="#panel-tab1" data-toggle="tab">
                                    <asp:Label ID="lbLiAct" runat="server" Text="Información Deuda"></asp:Label>
                                </a></li>
                                <li id="li2" runat="server"><a href="#panel-tab2" data-toggle="tab">
                                    <asp:Label ID="lbLi1" runat="server" Text="Crédito disponible"></asp:Label>
                                </a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="panel-tab1">
                                    <asp:Panel ID="pnFormularioAct" runat="server">
                                        <asp:HiddenField ID="HiddenField3" runat="server" />
                                        <div id="Div4" class="row clearfix">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="card-box">
                                                        <div class="row">

                                                            <asp:Panel ID="pnFormulario" runat="server">
                                                                <div class="form-horizontal" role="form">
                                                                    <!-- Grupo 1 -->
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label for="inputInstFinan" class="col-sm-5 control-label">Institución Financiera:</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txt_instFinancieraID" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label for="input" class="col-sm-5 control-label">Tipo de deuda:</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList runat="server" ID="ddlTipoDeuda" CssClass="form-control">
                                                                                    <asp:ListItem Selected="True" Text="Directa" Value="0" />
                                                                                    <asp:ListItem Text="Indirecta" Value="1" />
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label for="inputVigente" class="col-sm-5 control-label">Monto Vigente CLP:</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txt_MontoVigente" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <!-- Grupo 2 -->
                                                                        <div class="form-group">
                                                                            <label for="inputVigente" class="col-sm-5 control-label">Mora 30-90 días CLP:</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txt_Mora_30_90" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label for="inputVigente" class="col-sm-5 control-label">Mora 90 días y más CLP:</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txt_Mora90Mas" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12">
                                                <div class="card-box">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnAtrasID" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtrasID_Click" OnClientClick="return Dialogo();" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCancelarID" runat="server" Text="Cancelar" OnClick="btnCancelarID_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                                                <asp:Button ID="btnLimpiarID" runat="server" Text="Limpiar" OnClick="btnLimpiarID_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                                                                <asp:Button ID="btnGuardarID" runat="server" Text="Guardar" OnClick="btnGuardarID_Click" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
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
                                                                            <asp:HiddenField ID="hdnIdInformacionDeuda" runat="server" />
                                                                            <asp:GridView ID="ResultadosBusquedaID" runat="server" class="table table-bordered table-hover" GridLines="Horizontal" ShowFooter="true" OnRowCreated="ResultadosBusquedaID_RowCreated" OnRowDataBound="ResultadosBusquedaID_RowDataBound" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusquedaID_PageIndexChanging" PageSize="5">
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <br />

                                        </div>

                                    </asp:Panel>
                                </div>
                                <div class="tab-pane" id="panel-tab2">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <div id="Div7" class="row clearfix">

                                            <div class="col-md-12">
                                                <div class="card-box">
                                                    <div class="row">

                                                        <asp:Panel ID="Panel2" runat="server">
                                                            <div class="form-horizontal" role="form">
                                                                <!-- Grupo 1 -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label for="inputVigente" class="col-sm-5 control-label">Institución Financiera:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_instFinancieraCD" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="inputVigente" class="col-sm-5 control-label">Monto Disponible Directa CLP:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_montoDispDirecta" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Grupo 2 -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="inputVigente" class="col-sm-5 control-label">Monto Disponible Indirecta CLP:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_montoDispIndirecta" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                    </div>
                                                </div>
                                            </div>


                                            <br>
                                            <div class="col-md-12">
                                                <div class="card-box">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnVolverCD" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnVolverCD_Click" OnClientClick="return Dialogo();" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCancelarCD" runat="server" Text="Cancelar" OnClick="btnCancelarCD_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                                                <asp:Button ID="btnLimpiarCD" runat="server" Text="Limpiar" OnClick="btnLimpiarCD_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                                                                <asp:Button ID="btnGuardarCD" runat="server" Text="Guardar" OnClick="btnGuardarCD_Click" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="table-responsive">
                                                                    <div id="Div10" runat="server" class="row clearfix">
                                                                        <div class="col-md-12 column">
                                                                            <br />
                                                                            <asp:HiddenField ID="hdnIdCreditoDisponible" runat="server" />
                                                                            <asp:GridView ID="ResultadosBusquedaCD" runat="server" class="table table-bordered table-hover" GridLines="Horizontal" ShowFooter="true" OnRowCreated="ResultadosBusquedaCD_RowCreated" OnRowDataBound="ResultadosBusquedaCD_RowDataBound" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusquedaCD_PageIndexChanging" PageSize="5" OnDataBound="ResultadosBusquedaCD_DataBound">
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
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
