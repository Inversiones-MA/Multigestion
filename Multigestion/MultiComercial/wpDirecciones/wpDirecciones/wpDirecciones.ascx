<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDirecciones.ascx.cs" Inherits="MultiComercial.wpDirecciones.wpDirecciones.wpDirecciones" %>

<style type="text/css">
    .form-group label {
        text-align:right;
        padding-top:10px;
    }
    td{
        white-space:normal;
    }
</style>

<script src="../../_layouts/15/MultiComercial/FuncionesClientes.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>


<script type="text/javascript">
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    });

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
            <h4 class="page-title">Direcciones</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Datos empresa</a>
                </li>
                <li class="active">Direcciones
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
                <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
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
                            <asp:LinkButton ID="lbDatosEmpresa" runat="server" OnClick="lbDatosEmpresa_Click" OnClientClick="return Dialogo();">Datos Empresa</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
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
                <li class="active">
                    <a href="#tab5" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
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
                <div class="tab-pane active" id="tab5">
                    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
                        <h4>
                            <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div id="dvWarning" runat="server" class="alert alert-warning" role="alert">
                        <h4>
                            <asp:Label ID="lbWarning" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>
                    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
                        <h4>
                            <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <div class="row">
                                    <asp:Panel ID="pnFormulario" runat="server">
                                        <!-- Grupo 1 -->
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label for="txt_rut" class="col-sm-5 control-label">Tipo:</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlTipo" CssClass="form-control" runat="server">
                                                        <%--<asp:ListItem Selected="True" Value="1">Comercial</asp:ListItem>
                                                        <asp:ListItem Value="2">Particular</asp:ListItem>
                                                        <asp:ListItem Value="3">Otro</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="txt_correo" class="col-sm-5 control-label">Región:</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList runat="server" ID="ddlRegion" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="txt_nombre" class="col-sm-5 control-label">Comuna:</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList runat="server" ID="ddlComunas" CssClass="form-control" />
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="txt_nombre" class="col-sm-5 control-label">Número:</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row" id="DivVerificaion" runat="server">
                                                <label for="chkprincipal" class="col-sm-5 control-label">Verificación:</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlVerificacion" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Grupo 2 -->
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label for="txt_apellidos" class="col-sm-5 control-label">Principal:</label>
                                                <div class="col-sm-7">
                                                    <asp:CheckBox ID="chkprincipal" runat="server" />
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="txt_nombre" class="col-sm-5 control-label">Provincia:</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList runat="server" ID="ddlProvincia" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="txt_nombre" class="col-sm-5 control-label">Calle:</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtdireccion" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="txt_nombre" class="col-sm-5 control-label">Complemento Dirección:</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtComplementoDireccion" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row" id="DivFechaVerificacion" runat="server">
                                                <label for="txt_nombre" class="col-sm-5 control-label">Fecha Verificación:</label>
                                                <div class="col-sm-7">
                                                    <SharePoint:DateTimeControl ID="dtcFechaVerificacion" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <br>
                                        <table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                                                    <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                        <table border="0" style="table-layout: fixed; width: 100%;">
                                            <tr>
                                                <td>
                                                    <div class="card-box">
                                                        <div class="table-responsive">
                                                            <div id="dv_buscarEmpresas" runat="server" class="row clearfix">
                                                                <div class="col-md-12 column">
                                                                    <asp:GridView ID="ResultadosBusqueda" runat="server" class="table table-bordered table-hover" GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="5" OnRowCreated="ResultadosBusqueda_RowCreated" OnRowDataBound="ResultadosBusqueda_RowDataBound" ViewStateMode="Enabled"></asp:GridView>
                                                                </div>
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

    </div>
    <asp:HiddenField ID="HddIdDireccionEmpresa" runat="server" />

</div>
