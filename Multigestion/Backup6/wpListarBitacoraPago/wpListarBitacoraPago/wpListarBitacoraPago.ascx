<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpListarBitacoraPago.ascx.cs" Inherits="MultiAdministracion.wpListarBitacoraPago.wpListarBitacoraPago.wpListarBitacoraPago" %>

<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script type="text/javascript" src="../../_layouts/15/MultiAdministracion/wpEmpresasRelacionadas.js"></script>

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
    <div class="row clearfix">
        <div class="col-md-6 column">
            <div class="btn-group btn-breadcrumb">
                <a class="btn btn-warning sinMargen"><i class="glyphicon glyphicon-home"></i>USTED ESTÁ EN:</a>
                <a class="btn btn-primary sinMargen">Contabilidad</a>
                <a class="btn btn-info sinMargen">Bitácora Siniestro</a>
            </div>
        </div>

    </div>

    <br />
    <table style="width:100%">
        <tr>
            <td class="auto-style1"></td>

            <td style="width:5%"></td>
            <td class="auto-style8">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td></td>
            <td align="right">
                <asp:LinkButton ID="lkHistorico" runat="server">Bitácora Facturación</asp:LinkButton>&nbsp; &nbsp;
                <asp:LinkButton ID="lkBitacoraPago" runat="server">Bitácora Siniestros</asp:LinkButton>
                &nbsp; &nbsp;
                <asp:LinkButton ID="lkRegistroBitacora" runat="server">Registrar Siniestro</asp:LinkButton>
            </td>
        </tr>

    </table>

    <br />
    <br />


    <div class="row clearfix">
        <div class="col-md-12 column form-horizontal">
            <div class="row clearfix">
                <!-- Grupo 1 -->
                <div class="col-md-4">
                    <div class="column form-group">
                        <label for="inputEmail4" class="col-sm-4 control-label">Acreedor:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlAcreedor" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">Rut:</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtRUT" class="form-control input-sm col-md4" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">Fecha Pago desde:</label>
                        <div class="col-sm-8">
                            <SharePoint:DateTimeControl ID="dtcFechaInicio" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md4" EnableViewState="true" DateOnly="true" />
                        </div>
                    </div>

                </div>
                <!-- Grupo 2 -->
                <div class="col-md-4">
                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">Causa:</label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlCausa" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">Razón Social:</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtRazonSocial" class="form-control input-sm col-md4" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-4 control-label">Fecha Pago hasta:</label>
                        <div class="col-sm-8">
                            <SharePoint:DateTimeControl ID="stcFechaFin" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md4" EnableViewState="true" DateOnly="true" />
                        </div>
                    </div>

                </div>
                <!-- Grupo 3 -->
                <div class="col-md-3">
                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-5 control-label">Motivo:</label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="ddlMotivo" CssClass="form-control input-sm" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="column form-group">
                        <label for="inputEmail3" class="col-sm-5 control-label">Nº Certificado:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtNCertificado" class="form-control input-sm col-md4" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Grupo 3 -->
                <div class="col-md-1">
                    <div class="column form-group">
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />

    <div id="dv_buscarEmpresas" runat="server">
        <div>
            <asp:GridView ID="ResultadosBusqueda" runat="server" OnRowDataBound="ResultadosBusqueda_RowDataBound"
                GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="ResultadosBusqueda_RowCreated"
                OnDataBound="ResultadosBusqueda_DataBound" class="table table-responsive table-hover table-bordered"
                OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" AllowPaging="True" OnPageIndexChanged="ResultadosBusqueda_PageIndexChanged">
                <PagerStyle CssClass="pagination-ys" />
            </asp:GridView>

        </div>
    </div>
    <br />

    <hr />
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

</div>
