<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpVaciadoBalance.ascx.cs" Inherits="MultiRiesgo.wpVaciadoBalance.wpVaciadoBalance.wpVaciadoBalance" %>

<style type="text/css">
    .btn {
        font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important;
    }
</style>

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

<script type="text/javascript" src="../../_layouts/15/MultiRiesgo/wpVaciado.js"></script>

<div id="dvWarning1" runat="server" style="display: none" cssclass="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">
    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Balance</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Documentos Comerciales</a>
                </li>
                <li class="active">Balance
                </li>
            </ol>
        </div>
    </div>

    <!-- Empresa -->
    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-gray">
                <p><strong>Empresa:
                    <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong> /
                    <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label></p>
            </div>
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
                            <asp:LinkButton ID="lbBalance" runat="server" OnClientClick="return Dialogo();" OnClick="lbBalance_Click">Balance</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li>
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbEdoResultado" runat="server" OnClientClick="return Dialogo();" OnClick="lbEdoResultado_Click">Estado de Resultados</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li>
                    <a href="#tab3" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbVentas" runat="server" OnClientClick="return Dialogo();" OnClick="lbVentas_Click">IVA Ventas</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li>
                    <a href="#tab4" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbCompras" runat="server" OnClientClick="return Dialogo();" OnClick="lbCompras_Click">IVA Compras</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li>
                    <a href="#tab5" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbScoring" runat="server" OnClientClick="return Dialogo();" OnClick="lbScoring_Click">Scoring</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li>
                    <a href="#tab6" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbResumenPAF" runat="server" OnClientClick="return Dialogo();" OnClick="lbResumenPAF_Click">Resumen PAF</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>

            <div class="tab-content">
                <div class="tab-pane active" id="tab1">

                    <!-- filtros-->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <div class="row">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAtrasi" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click1" OnClientClick="return Dialogo();" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnImprimirBalance" CssClass="btn btn-success pull-right" runat="server" Text="Imprimir Vaciado Balance" OnClick="btnImprimirBalance_Click" />
                                            </td>
                                            <td class="auto-style1">
                                                <asp:Button ID="btnGuardari" CssClass="btn btn-primary btn-info pull-right" runat="server" Text="Guardar" OnClientClick="return Dialogo();" OnClick="btnGuardar_Click" />
                                                <asp:Button ID="btnFinalizari" CssClass="btn  btn-success pull-right" runat="server" Text="Finalizar" OnClientClick="return Dialogo();" OnClick="btnFinalizar_Click" />

                                                <asp:Button ID="btnCancelari" CssClass="btn btn-primary pull-right" runat="server" Text="Cancelar" OnClientClick="return Dialogo();" />
                                                <asp:Button ID="btnlimpiari" CssClass="btn btn-warning pull-right" runat="server" Text="Limpiar" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>


                    <asp:Panel ID="pnFormulario" runat="server">
                        <asp:HiddenField ID="HddIdDirectorio" runat="server" />

                        <div id="dv_buscarEmpresas" runat="server" class="row clearfix">

                            <!-- tabla / grilla -->
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView CssClass="table table-responsive table-hover table-bordered" ID="ResultadosBusqueda" runat="server" GridLines="Horizontal" OnRowCreated="ResultadosBusqueda_RowCreated" OnRowDataBound="ResultadosBusqueda_RowDataBound" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnDataBound="ResultadosBusqueda_DataBound" OnSelectedIndexChanged="ResultadosBusqueda_SelectedIndexChanged" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField DataField="Grupo" HeaderText="" />
                                                                <asp:BoundField DataField="Cuenta" HeaderText="" />
                                                                <asp:BoundField DataField="Subcuenta" HeaderText="" />

                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbValor1" runat="server" Text="Label"></asp:Label>
                                                                        <br />
                                                                        <asp:DropDownList ID="cbValor1" runat="server"></asp:DropDownList>
                                                                        <br />
                                                                        Requerido
                                                <asp:DropDownList ID="ddlRequerido3" runat="server">
                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAnio1" runat="server" Width="90" MaxLength="16" onKeyPress="return solonumerosN(event)" TabIndex="100"></asp:TextBox>
                                                                        <%-- onkeyup="return format(this)" onchange="return format(this)" --%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbValor2" runat="server" Text="Label"></asp:Label>
                                                                        <br />
                                                                        <asp:DropDownList ID="cbValor2" runat="server"></asp:DropDownList>
                                                                        <br />
                                                                        Requerido
                                                <asp:DropDownList ID="ddlRequerido2" runat="server">
                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAnio2" runat="server" Width="90" MaxLength="16" onKeyPress="return solonumerosN(event)" TabIndex="200"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:DropDownList ID="cbMes3" runat="server">
                                                                            <asp:ListItem Value="1">Ene</asp:ListItem>
                                                                            <asp:ListItem Value="2">Feb</asp:ListItem>
                                                                            <asp:ListItem Value="3">Mar</asp:ListItem>
                                                                            <asp:ListItem Value="4">Abr</asp:ListItem>
                                                                            <asp:ListItem Value="5">May</asp:ListItem>
                                                                            <asp:ListItem Value="6">Jun</asp:ListItem>
                                                                            <asp:ListItem Value="7">Jul</asp:ListItem>
                                                                            <asp:ListItem Value="8">Ago</asp:ListItem>
                                                                            <asp:ListItem Value="9">Sep</asp:ListItem>
                                                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                                                            <asp:ListItem Value="12">Dic</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lbValor3" runat="server" Text="Label"></asp:Label>
                                                                        <br />
                                                                        <asp:DropDownList ID="cbValor3" runat="server"></asp:DropDownList>
                                                                        <br />
                                                                        Requerido
                                                <asp:DropDownList ID="ddlRequerido1" runat="server">
                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAnio3" runat="server" Width="90" MaxLength="16" onKeyPress="return solonumerosN(event)" TabIndex="300"></asp:TextBox>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:DropDownList ID="cbMesAct" runat="server">

                                                                            <asp:ListItem Value="1">Ene</asp:ListItem>
                                                                            <asp:ListItem Value="2">Feb</asp:ListItem>
                                                                            <asp:ListItem Value="3">Mar</asp:ListItem>
                                                                            <asp:ListItem Value="4">Abr</asp:ListItem>
                                                                            <asp:ListItem Value="5">May</asp:ListItem>
                                                                            <asp:ListItem Value="6">Jun</asp:ListItem>
                                                                            <asp:ListItem Value="7">Jul</asp:ListItem>
                                                                            <asp:ListItem Value="8">Ago</asp:ListItem>
                                                                            <asp:ListItem Value="9">Sep</asp:ListItem>
                                                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                                                            <asp:ListItem Value="12">Dic</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="lbValorAct" runat="server" Text="Label"></asp:Label>
                                                                        <br />
                                                                        <asp:DropDownList ID="cbValorActual" runat="server"></asp:DropDownList>
                                                                        <br />
                                                                        Requerido
                                                <asp:DropDownList ID="ddlRequeridoActual" runat="server">
                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                                    </HeaderTemplate>

                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAnioA" runat="server" Width="90" MaxLength="16" onKeyPress="return solonumerosN(event)" TabIndex="400"></asp:TextBox>
                                                                    </ItemTemplate>

                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="idGrupoCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="idCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="idSubGrupoCuenta" HeaderText="" />

                                                                <asp:BoundField DataField="Valor1" HeaderText="" />
                                                                <asp:BoundField DataField="Valor2" HeaderText="" />
                                                                <asp:BoundField DataField="Valor3" HeaderText="" />
                                                                <asp:BoundField DataField="ValorAct" HeaderText="" />
                                                                <asp:BoundField DataField="IdEmpresaDocumentoContable" HeaderText="" />


                                                                <asp:BoundField DataField="Validacion" HeaderText="" />

                                                                <asp:BoundField DataField="IdTipoDocumento1" HeaderText="" />
                                                                <asp:BoundField DataField="IdTipoDocumento2" HeaderText="" />
                                                                <asp:BoundField DataField="IdTipoDocumento3" HeaderText="" />
                                                                <asp:BoundField DataField="IdTipoDocumentoAct" HeaderText="" />

                                                                <asp:BoundField DataField="IdMes3" HeaderText="" />
                                                                <asp:BoundField DataField="IdMesAct" HeaderText="" />

                                                                <asp:BoundField DataField="ACCIONGrupo" HeaderText="" />
                                                                <asp:BoundField DataField="ACCIONCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="ACCIONSubcuenta" HeaderText="" />
                                                                <asp:BoundField DataField="RequeridoActual" HeaderText="" />
                                                                <asp:BoundField DataField="Requerido3" HeaderText="" />
                                                                <asp:BoundField DataField="Requerido2" HeaderText="" />
                                                                <asp:BoundField DataField="Requerido1" HeaderText="" />
                                                            </Columns>
                                                            <HeaderStyle Height="40px" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />

                            <!-- filtros-->
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card-box">
                                        <div class="row">

                                            <div class="col-md-2 col-sm-2">
                                                <label>
                                                    Comentario:
                                                </label>
                                            </div>
                                            <div class="col-md-10 col-sm-10">
                                                <asp:TextBox ID="txtComentario" runat="server" Style="width: 100%;" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="row">

                                            <br />
                                            <table style="width:100%">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click1" />
                                                    </td>
                                                    <td class="auto-style1">
                                                        <asp:Button ID="btnGuardar" CssClass="btn btn-primary btn-info pull-right" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                                                        <asp:Button ID="btnFinalizar" CssClass="btn  btn-success pull-right" runat="server" Text="Finalizar" OnClientClick="return Dialogo();" OnClick="btnFinalizar_Click" />

                                                        <asp:Button ID="btnCancelar" CssClass="btn btn-primary pull-right" runat="server" Text="Cancelar" OnClientClick="return Dialogo();" />
                                                        <asp:Button ID="btnLimpiar" CssClass="btn btn-warning pull-right" runat="server" Text="Limpiar" />
                                                    </td>
                                                </tr>
                                            </table>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </asp:Panel>
                    <%--<asp:LinkButton ID="lbEditar" runat="server" Text=" " CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"> Editar</asp:LinkButton>--%>
                </div>
            </div>
        </div>
    </div>


    <br />

    <asp:HiddenField ID="hddTotalActivo" runat="server" />
    <asp:HiddenField ID="hddTotalPasivo" runat="server" />

</div>
