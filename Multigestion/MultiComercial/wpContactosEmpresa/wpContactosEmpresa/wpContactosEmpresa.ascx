<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpContactosEmpresa.ascx.cs" Inherits="MultiComercial.wpContactosEmpresa.wpContactosEmpresa.wpContactosEmpresa" %>


<script type="text/javascript" src="../_layouts/15/MultiComercial/FuncionesCliente.js"></script>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
     <h4>
         <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
     </h4>
</div>

								
<div id="dvFormulario" runat="server">
<div class="row clearfix">
    <div class="col-md-6 column">
        <div class="btn-group btn-breadcrumb">
            <a class="btn btn-warning sinMargen" href="#"><i class="glyphicon glyphicon-home"></i>USTED ESTÁ EN:</a>
            <a class="btn btn-primary sinMargen">Datos Empresa</a>
            <a class="btn btn-info sinMargen">Contactos</a>
        </div>
    </div>
    <div class="col-md-6 column" style="text-align: right;">
        <!-- Identificador de empresa -->
        <h3 class="sinMargen">Empresa:&nbsp;<asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></h3>

        <h5>RUT:&nbsp;<asp:Label ID="lbRut" runat="server" Text=""></asp:Label><br />
            Ejecutivo:&nbsp;<asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label></h5>
    </div>
</div>

<ul class="nav nav-tabs">
    <li>
        <asp:LinkButton ID="lbDatosEmpresa" runat="server" OnClick="lbDatosEmpresa_Click" OnClientClick="return Dialogo();">Datos Empresa</asp:LinkButton>
    </li>
    <li>
        <asp:LinkButton ID="lbHistoria" runat="server" OnClientClick="return Dialogo();" OnClick="lbHistoria_Click">Historia Empresa</asp:LinkButton>

    </li>
    <li>
        <asp:LinkButton ID="lbEmpresaRelacionada" runat="server" OnClientClick="return Dialogo();" OnClick="lbEmpresaRelacionada_Click">Empresas Relacionadas</asp:LinkButton>
    </li>
    <li>
        <asp:LinkButton ID="lbSociosAccionistas" runat="server" OnClientClick="return Dialogo();" OnClick="lbSociosAccionistas_Click">Socios y Asociados</asp:LinkButton>
    </li>
    <li>
        <asp:LinkButton ID="lbDirectorio" runat="server" OnClientClick="return Dialogo();" OnClick="lbDirectorio_Click">Directorio</asp:LinkButton>
    </li>
    <li class="active">
        <asp:LinkButton ID="lbContacto" runat="server" OnClientClick="return Dialogo();" OnClick="lbContactos_Click">Contactos</asp:LinkButton>
    </li>
    <li>
        <asp:LinkButton ID="lbDirecciones" runat="server" OnClientClick="return Dialogo();" OnClick="lbDirecciones_Click">Direcciones</asp:LinkButton>
    </li>
</ul>


<div class="tab-content">


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
    <div class="tab-pane active margenBottom" id="panel-tab3">

        <asp:Panel ID="pnFormulario" runat="server">
            <div class="form-horizontal" role="form">
                <!-- Grupo 1 -->
                <div class="col-md-6">
                    <div class="form-group">

                        <label for="txt_rut" class="col-sm-5 control-label">Nombres:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txt_nombre" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>



                    </div>
                    <div class="form-group">
                        <label for="txt_apellidos" class="col-sm-5 control-label">Cargo:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txt_cargo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>


                <!-- Grupo 2 -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txt_nombre" class="col-sm-5 control-label">Apellidos:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txt_apellidos" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="txt_correo" class="col-sm-5 control-label">Email:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txt_email" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!-- Grupo 3 -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txt_nombre" class="col-sm-5 control-label">Tel.Fijo:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txt_fijo" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)" MaxLength="9"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txt_correo" class="col-sm-5 control-label"></label>
                        <div class="col-sm-7">
                        </div>
                    </div>
                </div>
                <!-- Grupo 4 -->
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="txt_nombre" class="col-sm-5 control-label">Tel.Celular:</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txt_celular" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)" MaxLength="9"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="chkprincipal" class="col-sm-5 control-label">Principal:</label>
                        <div class="col-sm-7">
                            <asp:CheckBox ID="chkprincipal" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <!-- fin  -->
</div>






<table style="width:100%">
    <tr>
        <td>
            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
        </td>
        <td>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" CssClass="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

            <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" CssClass="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

            <asp:Button CssClass="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
        </td>
    </tr>
</table>
<br />


<asp:HiddenField ID="HddIdContactoEmpresa" runat="server" />



<div id="dv_buscarEmpresas" runat="server" class="row clearfix">

    <div class="col-md-12 column">

        <br />
        <asp:GridView ID="ResultadosBusqueda" runat="server" CssClass="table table-responsive table-hover table-bordered" GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="5" OnRowCreated="ResultadosBusqueda_RowCreated" OnRowDataBound="ResultadosBusqueda_RowDataBound" ViewStateMode="Enabled">
        </asp:GridView>

    </div>
</div>

<asp:TextBox ID="txt_rut1" runat="server" MaxLength="8" CssClass="form-control" Visible="False"></asp:TextBox>

<asp:TextBox ID="txt_divRut" runat="server" MaxLength="1" CssClass="form-control" Width="30px" Visible="False"></asp:TextBox>
<span style="color: red;" id="errorRut"></span>

</div>