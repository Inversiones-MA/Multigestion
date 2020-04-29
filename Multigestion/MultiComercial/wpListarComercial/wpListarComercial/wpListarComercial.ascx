﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpListarComercial.ascx.cs" Inherits="MultiComercial.wpListarComercial.wpListarComercial.wpListarComercial" %>

<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script type="text/javascript" src="../../_layouts/15/MultiComercial/wpEmpresasRelacionadas.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>

<style>
 .table-bordered a.glyphicon-plus-sign {color:#5fbeaa !Important;}
 .table-bordered a.glyphicon-plus-sign:focus,.table-bordered a.glyphicon-plus-sign:hover {color:#23527c !Important;}

</style>

<script type="text/javascript">
    function setFocus(obj) {
        document.getElementById(obj).focus();
    }

    document.onkeydown = function () {
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {SERVICIOS INDUSTRIALES INTEGRA LTDA.
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
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
                    }
                    if (document.getElementById('<%=txtBuscar.ClientID %>').val != '') {
                        document.getElementById('<%=btnBuscar.ClientID %>').click();
                     }
                }
            }
    }

    var segundos = 0;
    function mostrarTiempoTranscurrido() {
        segundos++;
        document.getElementById("IndicadorTiempo").innerHTML = segundos.toString();
    }

    function MantenSesion() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'actualizando su solicitud', 120, 550);
        __doPostBack('', '');
        return false;
    }

    setInterval("MantenSesion()", (1200000));

    function onLoad() {
        if ($('#myStateInput').val() === '') // Load with no state.
            $('#myStateInput').val('already loaded'); // Set state
        else
            alert("Loaded with state. (Duplicate tab or Back + Forward)");
    }
</script>



<!-- Page-Title -->
<div class="row marginInicio">
    <div class="col-sm-12">
        <h4 class="page-title">Comercial</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Inicio</a>
            </li>
            <li class="active">Comercial
            </li>
        </ol>
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

<div id="dvFormulario" runat="server">
    <span id="IndicadorTiempo" visible="false"></span>
    <!-- filtros-->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Buscar</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-default" OnClick="btnBuscar_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Estado</label>
                            <asp:DropDownList ID="cb_estados" runat="server" OnSelectedIndexChanged="cb_estados_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label>Etapa</label>
                            <asp:DropDownList ID="cb_etapa" runat="server" OnSelectedIndexChanged="cb_etapa_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True">
                                <%--<asp:ListItem Value="-1">--Seleccione Etapas--</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label>SubEtapa</label>
                            <asp:DropDownList ID="cb_subetapa" runat="server" OnSelectedIndexChanged="cb_subetapa_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True">
                                <%--<asp:ListItem Value="-1">--Seleccione SubEtapa--</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="Panel1" runat="server">
        <div id="dv_buscarEmpresas" runat="server">
            <!-- tabla / grilla -->
            <div class="row">
                <div class="col-md-12">
                    <table border="0" style="table-layout: fixed; width: 100%;">
                        <tr>
                            <td>
                                <div class="card-box">
                                    <div class="table-responsive">
                                        <asp:GridView ID="ResultadosBusqueda" runat="server" OnRowDataBound="ResultadosBusqueda_RowDataBound"
                                            GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="ResultadosBusqueda_RowCreated"
                                            OnDataBound="ResultadosBusqueda_DataBound" CssClass="table table-bordered table-hover" PageSize="15"
                                            OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" AllowPaging="True">
                                            <PagerStyle CssClass="pagination-ys" />
                                        </asp:GridView>
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
