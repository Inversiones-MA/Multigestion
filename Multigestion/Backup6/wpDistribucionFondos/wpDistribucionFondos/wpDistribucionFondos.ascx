<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDistribucionFondos.ascx.cs" Inherits="MultiOperacion.wpDistribucionFondos.wpDistribucionFondos.wpDistribucionFondos" %>

<style type="text/css">
    .btn { font:14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important; }
    .rpsnSpace { margin-right:10px !important; }
</style>

<script type="text/javascript">
    document.onkeydown = function () {
 
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
            <h4 class="page-title">Distribución de Fondos</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Datos Operación</a>
                </li>
                <li class="active">Distribución de Fondos
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
                <li class="active">
                    <a href="#tab1" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbDistribucionFondos" runat="server" OnClientClick="return Dialogo();" OnClick="lbDistribucionFondos_Click">Distribución de Fondos</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbDocumentoCurse" runat="server" OnClientClick="return Dialogo();" OnClick="lbDocumentoCurse_Click">Documentos de Curse</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane active" id="tab1">
                    <div class="row">
                        <!-- col 1/4 -->
                        <h4>Distribución de Fondos</h4>
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label for="">Acreedor:</label>
                                <asp:TextBox ID="txtAcreedor" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="">Impuesto Timbre y Estampilla (Cargo Multiaval):</label>
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtTyEMultiaval" MaxLength="16" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="">Incluida?</label>
                                        <asp:DropDownList ID="ddlTyEMultiaval" runat="server">
                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="">Impuesto Timbre y Estampilla (Cargo Acreedor):</label>
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtTyEAcreedor" MaxLength="16" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="">Incluida?</label>
                                        <asp:DropDownList ID="ddlTyEAcreedor" runat="server">
                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="">Notario:</label>
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtNotario" MaxLength="16" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="">Incluida?</label>
                                        <asp:DropDownList ID="ddlNotario" runat="server">
                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- col 2/4 -->
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label for="">Tipo de Producto:</label>
                                <asp:TextBox ID="txtTipoProducto" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label for="">Seguro Incendio:</label>
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtSeguroIncendio" MaxLength="16" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="">Incluida?</label>
                                        <asp:DropDownList ID="ddlSeguroIncendio" runat="server">
                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="">Seguro Desgravamen:</label>
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtSeguroDesgravamen" MaxLength="16" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <label for="">Incluida?</label>
                                        <asp:DropDownList ID="ddlSeguroDesgravamen" runat="server">
                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- col 3/4 -->
                        <div class="col-md-3 col-sm-6">
                        </div>
                        <!-- col 4/4 -->
                        <div class="col-md-3 col-sm-6">
                        </div>
                    </div>

                    <div class="row">
                        <!-- col 1/4 -->
                        <h4>Prepago Crédito mismo Banco</h4>
                        <div class="col-md-4 col-sm-6">
                            <div class="form-group">
                                <label for="">N° Crédito:</label>
                                <asp:TextBox ID="txtNroCreditoBanco" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label for="">Tipo Fondo:</label>
                                <asp:DropDownList runat="server" ID="ddlTipoFondoMB" CssClass="form-control">
                                    <asp:ListItem Value="1">Retenido</asp:ListItem>
                                    <asp:ListItem Value="2">Liberado</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>

                        <!-- col 2/4 -->
                        <div class="col-md-4 col-sm-6">
                            <div class="form-group">
                                <label for="">Monto CLP:</label>
                                <asp:TextBox ID="txtMontoBanco" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- col 3/4 -->
                        <div class="col-md-4 col-sm-6">
                            <div class="row">
                                <asp:Button class="btn btn-primary btn-success pull-right rpsnSpace" ID="btnAddBanco" runat="server" Text="Agregar" OnClientClick="return Dialogo();" OnClick="btnAddBanco_Click" />
                            </div>
                        </div>

                        <!-- tabla / grilla -->
                        <div class="col-md-10 col-sm-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridDisFondoBanco" runat="server" GridLines="Horizontal"
                                                            class="table table-bordered table-hover" RowHeaderColumn="0" ShowHeaderWhenEmpty="True"
                                                            OnRowCreated="gridDisFondoBanco_RowCreated" OnRowDataBound="gridDisFondoBanco_RowDataBound">
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <h4>Prepago Crédito otro Acreedor</h4>
                        <div class="col-md-4 col-sm-6">
                            <div class="form-group">
                                <label for="">N° Crédito:</label>
                                <asp:TextBox ID="txtNroCreditoOtro" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label>Acreedor:</label>
                                <asp:DropDownList runat="server" ID="ddlAcreedor" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-6">
                            <div class="form-group">
                                <label for="">Monto CLP:</label>
                                <asp:TextBox ID="txtMontoOtro" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label>Tipo Fondo:</label>
                                <asp:DropDownList runat="server" ID="ddlTipoFondoOB" CssClass="form-control">
                                    <asp:ListItem Value="1">Retenido</asp:ListItem>
                                    <asp:ListItem Value="2">Liberado</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-12">
                            <div class="row">
                                <asp:Button class="btn btn-primary btn-success pull-right rpsnSpace" ID="btnFondoOtro" runat="server" Text="Agregar" OnClientClick="return Dialogo();" Style="width: 68px" OnClick="btnFondoOtro_Click" />
                            </div>
                        </div>

                        <!-- tabla / grilla -->
                        <div class="col-md-10 col-sm-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridDisFondoOtroBanco" runat="server" GridLines="Horizontal"
                                                            class="table table-responsive table-hover table-bordered" RowHeaderColumn="0" ShowHeaderWhenEmpty="True"
                                                            OnRowCreated="gridDisFondoOtroBanco_RowCreated" OnRowDataBound="gridDisFondoOtroBanco_RowDataBound">
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <h4>Multiaval</h4>
                        <div class="col-md-4 col-sm-6">
                            <div class="form-group">
                                <label for="">Tipo Documento:</label>
                                <asp:DropDownList runat="server" ID="ddlTipoDoumento" CssClass="form-control">
                                    <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                    <asp:ListItem Value="1">Vale Vista</asp:ListItem>
                                    <asp:ListItem Value="2">Depósito a plazo</asp:ListItem>
                                    <asp:ListItem Value="3">Otros</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <label>Comentario:</label>
                                <asp:TextBox ID="txtComentario" TextMode="multiline" Rows="3" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-6">
                            <div class="form-group">
                                <label for="">Monto CLP:</label>
                                <asp:TextBox ID="txtMontoCLP" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label>Tipo Fondo:</label>
                                <asp:DropDownList runat="server" ID="ddlTipoFondoM" CssClass="form-control">
                                    <asp:ListItem Value="1">Retenido</asp:ListItem>
                                    <asp:ListItem Value="2">Liberado</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-4 col-sm-6">
                            <div class="row">
                                <asp:Button class="btn btn-primary btn-success pull-right rpsnSpace" ID="btnGuardarM" runat="server" Text="Agregar" OnClientClick="return Dialogo();" Style="width: 68px" OnClick="btnFondoMultiaval_Click" />
                            </div>
                        </div>

                        <div class="col-md-10 col-sm-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="GridMultiaval" runat="server" GridLines="Horizontal"
                                                            class="table table-bordered table-hover" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="GridMultiaval_RowCreated" OnRowDataBound="GridMultiaval_RowDataBound">
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding: 0px 6px;">
                            <table style="width:100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click1" />
                                    </td>

                                    <td>
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                                        <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                                        <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" Style="width: 68px" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="tab-pane" id="tab2">
                </div>
                <div class="tab-pane" id="tab3">
                </div>
                <div class="tab-pane" id="tab4">
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdIdNegocio" runat="server" />
    <asp:HiddenField ID="hdDocumento" runat="server" />

</div>
