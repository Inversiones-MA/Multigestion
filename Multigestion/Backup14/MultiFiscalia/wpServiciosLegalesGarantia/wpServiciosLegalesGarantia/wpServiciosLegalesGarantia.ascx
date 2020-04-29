<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpServiciosLegalesGarantia.ascx.cs" Inherits="MultiFiscalia.wpServiciosLegalesGarantia.wpServiciosLegalesGarantia.wpServiciosLegalesGarantia" %>

<script type="text/javascript" src="../../_layouts/15/MultiFiscalia/Fiscalia.js"></script>
<script type="text/javascript" src="../../_layouts/15/MultiFiscalia/FuncionesCliente.js"></script>
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
        <h4 class="page-title">Servicios Legales </h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Fiscalía</a>
            </li>
            <li class="active">Servicios Legales Garantía
            </li>
        </ol>
    </div>
</div>

 <!-- Empresa -->
 <div class="row">
    <div class="col-md-12">
        <div class="alert alert-gray">
            <p><strong>Empresa: <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong> 
                / <asp:Label ID="lbRut" runat="server" Text=""></asp:Label> 
                / Negocio: <asp:Label ID="LbNegocio" runat="server" Text=""></asp:Label>
                / N° Certificado: <asp:Label ID="lblNCertificado" runat="server" Text=""></asp:Label>
               <%-- / Ejecutivo: <asp:Label ID="LbEjecutivo" runat="server" Text=""></asp:Label>--%>
            </p>
        </div>
    </div>
</div>


<div class="row">
    <div class="col-md-12">
        <ul class="nav nav-tabs navtab-bg nav-justified">
            <li>
                <a href="#tab1" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbServiciosLegalesEmpresa" runat="server" OnClientClick="return Dialogo();" OnClick="lbServiciosLegalesEmpresa_Click">Servicios Legales Empresa</asp:LinkButton>
                    </span>
                </a>
            </li>
            <li class="active">
                <a href="#tab2" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbServiciosLegalesGarantia" runat="server" OnClientClick="return Dialogo();" OnClick="lbServiciosLegalesGarantia_Click">Servicios Legales Garantia</asp:LinkButton>
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
                        <asp:Label ID="lbWarning" runat="server" Text=""></asp:Label>
                    </h4>
                </div>

                <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
                    <h4>
                        <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                    </h4>
                </div>

                <asp:Panel ID="pnFormulario" runat="server">
                    <h5>Empresa</h5>

                    <div class="row">
                        <div class="form-horizontal" role="form">
                            <!-- col 1/4 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="" class="col-sm-5 control-label">Nombre / Razón Social</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lblNombre" runat="server" class="form-control"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="" class="col-sm-5 control-label">Tipo de Empresa</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbTipo" class="form-control" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="" class="col-sm-5 control-label">Ejecutivo</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbEjecutivo" class="form-control" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Nro Operación </label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbOperacion" class="form-control" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Comentario Comite</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbComentarioComite" class="form-control" runat="server"></asp:Label>
                                    </div>
                                </div>

                            </div>


                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">RUT</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lblRut" class="control-label" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Monto Línea General</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbMonto" class="form-control" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Nro PAF</label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lbPAF" class="form-control" runat="server"></asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <h5>Operación(es)</h5>

                </asp:Panel>


                <h5>Garantía(es)</h5>

                <!-- tabla / grilla -->
                <div class="row">
                    <div class="col-md-12">
                        <table border="0" style="table-layout: fixed; width: 100%;">
                            <tr>
                                <td>
                                    <div class="card-box">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gridGarantia" runat="server" GridLines="Horizontal" class="table table-bordered table-hover" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" AllowPaging="False" OnRowCreated="gridGarantias_RowCreated" OnRowDataBound="gridGarantias_RowDataBound">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>


                <h5>Servicios Garantía</h5>

                <!-- tabla / grilla -->
                <div class="row">
                    <div class="col-md-12">
                        <table border="0" style="table-layout: fixed; width: 100%;">
                            <tr>
                                <td>
                                    <div class="card-box">
                                        <div class="table-responsive">
                                            <asp:GridView ID="tbServiciosGarantias1" runat="server" GridLines="Horizontal" class="table table-bordered table-hover"
                                                ShowHeaderWhenEmpty="True" PageSize="10" AutoGenerateColumns="False" OnDataBound="tbServiciosGarantias1_DataBound" OnRowCreated="tbServiciosGarantias1_RowCreated">
                                                <Columns>
                                                    <asp:TemplateField>

                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ckbServiciosEmpresa" runat="server" />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="ID" ItemStyle-ForeColor="White" HeaderText="" ItemStyle-Width="0px" ControlStyle-Width="0px" ItemStyle-Font-Size="0" />


                                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre Servicio" />
                                                    <asp:BoundField DataField="Estado" ItemStyle-ForeColor="White" HeaderText="" ItemStyle-Width="0px" ControlStyle-Width="0px" ItemStyle-Font-Size="0" />
                                                    <asp:BoundField DataField="Garantia" HeaderText="Garantía" />

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

                <h5>Documentos</h5>

                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Documento</label>
                                <div class="col-sm-7">
                                    <asp:FileUpload ID="fileDocumento" runat="server" class="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Tipo</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlTipo" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label></label>
                            <asp:Button ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" Text="Agregar" CssClass="btn btn-primary btn-success pull-left" />
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
                                            <asp:GridView ID="gridDocumentos" runat="server" GridLines="Horizontal"
                                                class="table table-bordered table-hover" RowHeaderColumn="0"
                                                ShowHeaderWhenEmpty="True" PageSize="5" OnRowCommand="gridDocumentos_Command" OnRowCreated="gridDocumentos_RowCreated" OnRowDataBound="gridDocumentos_RowDataBound">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <table style="width:100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                        </td>

                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" Visible="false"/>

                            <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar" OnClick="btnFinalizar_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</div>

<asp:HiddenField ID="hdDocumento" runat="server" />

</div>