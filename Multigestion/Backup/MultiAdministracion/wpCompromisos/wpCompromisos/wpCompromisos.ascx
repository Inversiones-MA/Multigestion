<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpCompromisos.ascx.cs" Inherits="MultiAdministracion.wpCompromisos.wpCompromisos.wpCompromisos" %>

<style type="text/css">
    .form-group label {
        text-align:right;
        padding-top:10px;
    }
</style>

 <div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
     <h4>
           <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
      </h4>
 </div>

									
<div id="dvFormulario" runat="server">
<!-- Page-Title -->
<div class="row marginInicio">
    <div class="col-sm-12">
        <h4 class="page-title">Compromisos</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Datos Operación</a>
            </li>
            <li class="active">Compromisos
            </li>
        </ol>
    </div>
</div>

 <!-- Empresa -->
 <div class="row">
    <div class="col-md-12">
        <div class="alert alert-gray">
            <p><strong>Empresa: <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong> / <asp:Label ID="lbRut" runat="server" Text=""></asp:Label> / Negocio: <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label> / Ejecutivo: <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label></p>
        </div>
    </div>
</div>


<div class="row">
    <div class="col-md-12">
        <ul class="nav nav-tabs navtab-bg nav-justified">
            <li class="active">
                <a href="#tab1" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbCompromisos" runat="server" OnClientClick="return Dialogo();" OnClick="lbCompromisos_Click">Compromisos</asp:LinkButton>
                    </span>
                </a>
            </li>

            <li>
                <a href="#tab2" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbBitacoraComent" runat="server" OnClientClick="return Dialogo();" OnClick="lbBitacoraComent_Click">Bitácora Comentarios</asp:LinkButton>
                    </span>
                </a>
            </li>
        </ul>

        <div class="tab-content">
            <div class="tab-pane active" id="tab1">
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

                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <div class="row">
                                <asp:Panel ID="pnFormulario" runat="server">

                                    <!-- Grupo 1 -->
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Razon Social:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtRazonSocial" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Ejecutivo:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtEjecutivo" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Canal:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtCanal" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Monto Operación:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtMontoOperacion" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Grupo 2 -->
                                    <div class="col-md-6">
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Operación:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtOperacion" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Etapa:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtEtapa" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Fondo:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtFondo" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label class="col-sm-5 control-label">Monto Comisión:</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtMontoComision" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <div class="col-md-12">
                                        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" OnClick="btnImprimir_Click" />
                                     </div>
                                </asp:Panel>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <!-- Grupo 1 -->
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Nuevo Comentario:</label>
                                            <asp:TextBox ID="txtComentario" TextMode="multiline" Columns="50" Rows="5" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Button ID="bt_Guardar" runat="server" Text="Agregar" class="btn btn-success pull-right" OnClientClick="return Dialogo();" OnClick="bt_Guardar_Click" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card-box">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                                </td>

                                                <td>
                                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="ResultadosBusqueda" runat="server" class="table table-bordered table-hover" GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="5"
                                                                        ViewStateMode="Enabled" OnRowDataBound="ResultadosBusqueda_RowDataBound" OnRowCreated="ResultadosBusqueda_RowCreated" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging">
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
                </div>

            </div>
        </div>

    </div>
</div>


</div>

       

       






       
    

