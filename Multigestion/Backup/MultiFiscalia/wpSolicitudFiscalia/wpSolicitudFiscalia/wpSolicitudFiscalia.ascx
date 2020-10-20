<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpSolicitudFiscalia.ascx.cs" Inherits="MultiFiscalia.wpSolicitudFiscalia.wpSolicitudFiscalia.wpSolicitudFiscalia" %>

<script type="text/javascript" src="../../_layouts/15/MultiFiscalia/Fiscalia.js"></script>
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
                        //else {
                        //    document.activeElement.value.keyCode = 13;
                        //} //Realiza el enter. 
                    }
                }
            }
        }
    }

    function SumatoriaVacido(celdaevento, celda, celdaFooter, accion, cantidad) {

        if (isNaN(document.getElementById(celdaevento).value) || document.getElementById(celdaevento).value == "") {
            document.getElementById(celdaevento).value = 0;
        }
        if (isNaN(document.getElementById(celda).value) || document.getElementById(celda).value == "") {
            document.getElementById(celda).value = 0;
        }
        var auxTotal = document.getElementById(celda).value;
        if (accion == 1) {

            document.getElementById(celda).value = parseFloat(document.getElementById(celdaevento).value) * cantidad;

            document.getElementById(celdaFooter).value = parseFloat(document.getElementById(celdaFooter).value) + parseFloat(document.getElementById(celda).value);
        }
        else
            document.getElementById(celda).value = parseFloat(auxTotal) - parseFloat(document.getElementById(celdaevento).value);

        return false;

    }

    function vaciarCampo(celdaevento, celda, celdaFooter, accion) {
        if (isNaN(document.getElementById(celdaevento).value) || document.getElementById(celdaevento).value == "") {
            document.getElementById(celdaevento).value = 0;
        }
        if (isNaN(document.getElementById(celda).value) || document.getElementById(celda).value == "") {
            document.getElementById(celda).value = 0;
        }

        var auxInd = document.getElementById(celdaevento).value;
        var auxTotal = document.getElementById(celda).value;
        var celdaf = document.getElementById(celdaFooter).value;
        if (accion == 1)
            //document.getElementById(celda).value = parseFloat(auxTotal) - parseFloat(auxInd);
            document.getElementById(celdaFooter).value = parseFloat(celdaf) - parseFloat(auxTotal);


        document.getElementById(celdaevento).value = "";
        document.getElementById(celda).value = "0";
        return false;

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
            <h4 class="page-title">Solicitud Fiscalía</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Servicios</a>
                </li>
                <li class="active">Solicitud Fiscalía
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
                </p>
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


    <%-- <asp:Panel ID="pnFormulario" runat="server">--%>
    <h5>Empresa</h5>
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">

                    <!-- col 1/4 -->
                    <div class="col-md-6 col-sm-6">
                        <div class="form-group">
                            <label for="">Nombre / Razón Social</label>
                            <asp:Label ID="lblNombre" runat="server" class="form-control"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label for="">Tipo de Empresa</label>
                            <asp:Label ID="lbTipo" class="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label for="">Ejecutivo</label>
                            <asp:Label ID="lbEjecutivo" class="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>Nro Operación </label>
                            <asp:Label ID="lbOperacion" class="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>Comentario Comite</label>
                            <asp:Label ID="lbComentarioComite" class="form-control" runat="server"></asp:Label>
                        </div>

                    </div>


                    <div class="col-md-6 col-sm-6">
                        <div class="form-group">
                            <label>RUT</label>
                            <asp:Label ID="lblRut" class="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>Monto Línea General</label>
                            <asp:Label ID="lbMonto" class="form-control" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label>Nro PAF</label>
                            <asp:Label ID="lbPAF" class="form-control" runat="server"></asp:Label>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>

    <br />




    <h5>Garantía(s)</h5>

    <!-- tabla / grilla -->
    <div class="row">
        <div class="col-md-12">
            <table border="0" style="table-layout: fixed; width: 100%;">
                <tr>
                    <td>
                        <div class="card-box">
                            <div class="table-responsive">
                                <asp:GridView ID="gridGarantias" runat="server" GridLines="Horizontal" class="table table-bordered table-hover" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" AllowPaging="False" OnRowDataBound="gridGarantias_RowDataBound">
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">

                    <div class="col-md-6 col-xs-12">
                        <!-- col 1/4 -->
                        <div class="col-md-6 col-sm-6">
                            <h5>Servicios Empresa</h5>
                            <div class="form-group">
                                <label for="">Servicios</label>
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddlServicioEmpresa" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 rpsForm">
                                        <asp:Button class="btn btn-primary btn-success btn-agrega" Style="margin-left: 0px;" ID="Button1" runat="server" Text="Agregar" OnClick="btnAddServiciosEmpresa_Click" OnClientClick="return Dialogo();" />
                                    </div>
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
                                                <div class="table-responsive">
                                                    <asp:GridView class="table table-bordered table-hover"
                                                        ID="tbServiciosOperaciones1" runat="server"
                                                        RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="10"
                                                        OnRowDataBound="tbServiciosOperaciones1_RowDataBound" OnRowCreated="tbServiciosOperaciones1_RowCreated">
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 col-xs-12">
                        <h5>Servicios Garantía</h5>
                        <div class="col-md-3 col-sm-6">
                            <div class="form-group">
                                <label>Garantía</label>
                                <asp:DropDownList ID="ddlGarantias" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>

                        </div>

                        <div class="col-md-3 col-sm-6">
                            <label>Servicios</label>
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlServiciosGarantia" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-4 rpsForm">
                                    <asp:Button class="btn btn-primary btn-success btn-agrega" Style="margin-left: 0px;" ID="Button2" runat="server" Text="Agregar" OnClientClick="return Dialogo();" OnClick="btnAddServicioGarantia_Click" />
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
                                                <div class="table-responsive">
                                                    <asp:GridView ID="tbServiciosGarantia1" runat="server" GridLines="Horizontal" class="table table-bordered table-hover"
                                                        RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="50"
                                                        OnRowDataBound="tbServiciosGarantia1_RowDataBound" OnRowCreated="tbServiciosGarantia1_RowCreated">
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
            </div>
        </div>

    </div>

    <!-- Comentarios -->
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label for="">Comentarios:</label>
                <div class="card-box row">
                    <asp:TextBox ID="txtComentarios" TextMode="multiline" Columns="20" Rows="5" runat="server" CssClass="col-md-12 control-label" MaxLength="1000" />
                </div>
            </div>
        </div>
    </div>



    <h5>Fiscalía</h5>
    <!-- tabla / grilla -->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <table border="0" style="table-layout: fixed; width: 100%;">
                    <tr>
                        <td>
                            <div class="table-responsive">
                                <asp:GridView ID="gridFiscalia" runat="server" GridLines="Horizontal" class="table table-bordered table-hover"
                                    RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="10"
                                    OnRowCreated="gridFiscalia_RowCreated" OnRowDataBound="gridFiscalia_RowDataBound" ShowFooter="True" AutoGenerateColumns="False" OnDataBound="gridFiscalia_DataBound">
                                    <Columns>
                                        <asp:BoundField DataField="IdServicio" HeaderText="IdServicio" Visible="false" />
                                        <asp:BoundField DataField="TipoServicio" HeaderText="Tipo Servicio" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" Visible="false" />
                                        <asp:BoundField DataField="Total" HeaderText="Total" Visible="false" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbh1" runat="server" Text="Valor"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:TextBox ID="txtValor" runat="server" Width="100" MaxLength="18" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lbfooter" runat="server" Text="Total Gastos Operaciones"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lbh2" runat="server" Text="Total"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <asp:TextBox ID="txtTotal" runat="server" Width="80" MaxLength="18" onKeyPress="return solonumeros(event)" BorderColor="Transparent" ReadOnly="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTotalServicios" runat="server" Width="80" MaxLength="18" onKeyPress="return solonumeros(event)" BorderColor="Transparent" ReadOnly="true" BackColor="Transparent"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <HeaderStyle Height="40px" />
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <br>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                            <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Solicitar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdDocumento" runat="server" />

</div>
