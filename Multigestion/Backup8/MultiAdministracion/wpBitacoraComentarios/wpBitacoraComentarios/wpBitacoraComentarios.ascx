<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpBitacoraComentarios.ascx.cs" Inherits="MultiAdministracion.wpBitacoraComentarios.wpBitacoraComentarios.wpBitacoraComentarios" %>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
      <h4>
          <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
      </h4>
</div>

									
<div id="dvFormulario" runat="server">

<!-- Page-Title -->
<div class="row marginInicio">
    <div class="col-sm-12">
        <h4 class="page-title">Bitácora Comentarios</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Datos Operación</a>
            </li>
            <li class="active">Bitácora Comentarios
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
            <li>
                <a href="#tab1" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbCompromisos" runat="server" OnClientClick="return Dialogo();" OnClick="lbCompromisos_Click">Compromisos</asp:LinkButton>
                    </span>
                </a>
            </li>

            <li class="active">
                <a href="#tab2" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbBitacoraComent" runat="server" OnClientClick="return Dialogo();" OnClick="lbBitacoraComent_Click">Bitácora Comentarios</asp:LinkButton>
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

                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                                    </td>
                                    <%--        <td>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  class="btn btn-primary pull-right" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                </td>--%>
                                </tr>
                            </table>
                            
                            <div class="row">
                                <div class="col-md-12">
                                    <!-- Grupo 1 -->
                                    <div class="col-md-12 col-sm-12">

                                           <!-- tabla / grilla -->
                        <div class="row">            						
                        	<div class="col-md-12">
                                   <table border="0" style="table-layout:fixed; width:100%;">
                                        <tr>
                                            <td>
                        		                <div class="card-box">
                        			                <div class="table-responsive">
                                                        <asp:GridView ID="ResultadosBusqueda" runat="server" class="table table-bordered table-hover" GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="5"
                                                            ViewStateMode="Enabled" OnRowDataBound="ResultadosBusqueda_RowDataBound">
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

