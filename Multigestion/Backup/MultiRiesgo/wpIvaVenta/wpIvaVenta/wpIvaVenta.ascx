<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpIvaVenta.ascx.cs" Inherits="MultiRiesgo.wpIvaVenta.wpIvaVenta.wpIvaVenta" %>

<style type="text/css">
    .btn { font:14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important; }
</style>
<script type="text/javascript" src="../../_layouts/15/MultiRiesgo/wpVaciado.js"></script>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
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
            <li class="active">Iva Ventas
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

<!-- Contenedor Tabs -->
<div class="row" id="tabs">
    <div class="col-md-12">
        <ul class="nav nav-tabs navtab-bg nav-justified">
            <li class="">
                <a href="#tab1" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbBalance" runat="server" OnClientClick="return Dialogo();" OnClick="lbBalance_Click">Balance</asp:LinkButton>
                    </span>
                </a>
            </li>

            <li class="">
                <a href="#tab2" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbEdoResultado" runat="server" OnClientClick="return Dialogo();" OnClick="lbEdoResultado_Click">Estado de Resultados</asp:LinkButton>
                    </span>
                </a>
            </li>

            <li class="active">
                <a href="#tab3" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
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
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:LinkButton ID="lbResumenPAF" runat="server" OnClientClick="return Dialogo();" OnClick="lbResumenPAF_Click">Resumen PAF</asp:LinkButton>
                    </span>
                </a>
            </li>

        </ul>

        <div class="tab-content" id="content">
            <div class="tab-pane active" id="tab3">
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
                        <ul class="nav nav-tabs navtab-bg nav-justified">
                            <li id="liAct" runat="server"><a href="#panel-tab1" data-toggle="tab">
                                <asp:Label ID="lbLiAct" runat="server" Text="Ventas"></asp:Label>
                            </a></li>

                            <li id="li1" runat="server"><a href="#panel-tab2" data-toggle="tab">
                                <asp:Label ID="lbLi1" runat="server" Text="Ventas"></asp:Label>
                            </a></li>

                            <li id="li2" runat="server"><a href="#panel-tab3" data-toggle="tab">
                                <asp:Label ID="lbLi2" runat="server" Text="Ventas"></asp:Label>
                            </a></li>

                            <li id="li3" runat="server"><a href="#panel-tab4" data-toggle="tab">
                                <asp:Label ID="lbLi3" runat="server" Text="Ventas"></asp:Label>
                            </a></li>
                        </ul>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnImprimirReporteIva" runat="server" Text="Imprimir Reporte IVA" class="btn btn-warning pull-right" OnClick="btnImprimirReporteIva_Click" />
                    </div>
                </div>
                <div class="tab-content">
                    <div class="tab-pane " id="panel-tab1">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:LinkButton ID="lbEditarAct" runat="server" Text=" " CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"> Editar</asp:LinkButton>
                            </div>
                        </div>
                        <asp:Panel ID="pnFormularioAct" runat="server">

                            <asp:HiddenField ID="HiddenField3" runat="server" />
                            <!-- tabla / grilla -->
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout:fixed; width:100%;">
							            <tr>
								            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView class="table table-bordered table-hover" ID="gridIvaAct" runat="server"
                                                            GridLines="Horizontal"
                                                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" OnRowCreated="gridIvaAct_RowCreated" OnDataBound="gridIvaAct_DataBound" OnRowDataBound="gridIvaAct_RowDataBound">

                                                            <Columns>
                                                                <asp:BoundField DataField="idCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="Cuenta" HeaderText="" />
                                                                <asp:TemplateField HeaderText="Ene">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes1" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Feb">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes2" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Mar">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes3" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Abr">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes4" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="May">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes5" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Jun">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes6" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Jul">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes7" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Agos">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes8" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sep">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes9" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Oct">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes10" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nov">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes11" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dic">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes12" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="idEmpresaDocumentoContable" HeaderText="" />
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

                        </asp:Panel>

                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnVolverAtrasAct" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                </td>
                                <td>

                                    <asp:Button ID="btnGuardarAct" CssClass="btn btn-primary btn-info pull-right" runat="server" Text="Guardar" OnClick="btnGuardarAct_Click" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnFinalizarAct" CssClass="btn  btn-success pull-right" runat="server" Text="Finalizar" OnClientClick="return Dialogo();" OnClick="btnFinalizarAct_Click" />
                                    <asp:Button ID="btnCancelarAct" CssClass="btn btn-primary pull-right" runat="server" Text="Cancelar" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                    <asp:Button ID="btnLimpiarAct" CssClass="btn btn-warning pull-right" runat="server" Text="Limpiar" />

                                </td>
                            </tr>
                        </table>

                    </div>

                    <div class="tab-pane " id="panel-tab2">
                        <br />
                        <asp:LinkButton ID="lbEditar1" runat="server" Text=" " CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"> Editar</asp:LinkButton>
                        <br />
                        <asp:Panel ID="pnFormulario1" runat="server">

                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <!-- tabla / grilla -->
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout:fixed; width:100%;">
							            <tr>
								            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView class="table table-bordered table-hover" ID="gridIva1" runat="server"
                                                            GridLines="Horizontal"
                                                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" OnRowCreated="gridIva1_RowCreated" OnDataBound="gridIva1_DataBound">

                                                            <Columns>

                                                                <asp:BoundField DataField="idCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="Cuenta" HeaderText="" />
                                                                <asp:TemplateField HeaderText="Ene">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes1" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Feb">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes2" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Mar">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes3" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Abr">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes4" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="May">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes5" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Jun">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes6" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Jul">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes7" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Agos">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes8" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sep">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes9" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Oct">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes10" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nov">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes11" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dic">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes12" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="idEmpresaDocumentoContable" HeaderText="" />
                                                                <%--<asp:BoundField DataField="idCuenta" HeaderText="" Visible="false" />--%>
                                                            </Columns>
                                                            <HeaderStyle Height="40px" />
                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                </div>
							                </td>
							            </tr>
						            </table>
                                </div>
                            </div>

                        </asp:Panel>

                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnVolverAtras1" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                </td>
                                <td>

                                    <asp:Button ID="btnGuardar1" CssClass="btn btn-primary btn-info pull-right" runat="server" Text="Guardar" OnClick="btnGuardar1_Click" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnFinalizar1" CssClass="btn  btn-success pull-right" runat="server" Text="Finalizar" OnClientClick="return Dialogo();" OnClick="btnFinalizar1_Click" />


                                    <asp:Button ID="btnCancelar1" CssClass="btn btn-primary pull-right" runat="server" Text="Cancelar" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                    <asp:Button ID="btnLimpiar1" CssClass="btn btn-warning pull-right" runat="server" Text="Limpiar" />

                                </td>
                            </tr>
                        </table>
                    </div>

                    <!-- Comienzo contenedor TAB-3 -->
                    <div class="tab-pane " id="panel-tab3">
                        <br />
                        <asp:LinkButton ID="lbEditar2" runat="server" Text=" " CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"> Editar</asp:LinkButton>
                        <br />

                        <asp:Panel ID="pnFormulario2" runat="server">

                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <!-- tabla / grilla -->
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout:fixed; width:100%;">
							            <tr>
								            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView class="table table-bordered table-hover" ID="gridIva2" runat="server"
                                                            GridLines="Horizontal"
                                                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" OnRowCreated="gridIva2_RowCreated" OnDataBound="gridIva2_DataBound">

                                                            <Columns>

                                                                <asp:BoundField DataField="idCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="Cuenta" HeaderText="" />
                                                                <asp:TemplateField HeaderText="Ene">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes1" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Feb">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes2" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Mar">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes3" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Abr">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes4" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="May">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes5" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Jun">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes6" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Jul">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes7" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Agos">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes8" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sep">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes9" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Oct">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes10" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nov">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes11" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dic">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes12" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="idEmpresaDocumentoContable" HeaderText="" />
                                                                <%--<asp:BoundField DataField="idCuenta" HeaderText="" Visible="false" />--%>
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

                        </asp:Panel>

                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnVolverAtras2" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                </td>
                                <td>

                                    <asp:Button ID="btnGuardar2" CssClass="btn btn-primary btn-info pull-right" runat="server" Text="Guardar" OnClick="btnGuardar3_Click" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnFinalizar2" CssClass="btn  btn-success pull-right" runat="server" Text="Finalizar" OnClientClick="return Dialogo();" OnClick="btnFinalizar2_Click" />


                                    <asp:Button ID="btnCancelar2" CssClass="btn btn-primary pull-right" runat="server" Text="Cancelar" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                    <asp:Button ID="btnLimpiar2" CssClass="btn btn-warning pull-right" runat="server" Text="Limpiar" />

                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="tab-pane " id="panel-tab4">
                        <br />
                        <asp:LinkButton ID="lbEditar3" runat="server" Text=" " CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"> Editar</asp:LinkButton>
                        <br />
                        <asp:Panel ID="pnFormulario3" runat="server">

                            <asp:HiddenField ID="HiddenField4" runat="server" />
                            <div class="row">
                                <div class="col-md-12">
                                     <table border="0" style="table-layout:fixed; width:100%;">
							            <tr>
								            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView class="table table-bordered table-hover" ID="gridIva3" runat="server"
                                                            GridLines="Horizontal"
                                                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" OnRowCreated="gridIva3_RowCreated" OnDataBound="gridIva3_DataBound">

                                                            <Columns>

                                                                <asp:BoundField DataField="idCuenta" HeaderText="" />
                                                                <asp:BoundField DataField="Cuenta" HeaderText="" />
                                                                <asp:TemplateField HeaderText="Ene">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes1" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Feb">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes2" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Mar">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes3" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Abr">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes4" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="May">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes5" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Jun">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes6" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Jul">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes7" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Agos">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes8" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sep">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes9" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Oct">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes10" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Nov">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes11" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Dic">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMes12" runat="server" class="form-control input10px" MaxLength="18" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="idEmpresaDocumentoContable" HeaderText="" />
                                                                <%--<asp:BoundField DataField="idCuenta" HeaderText="" Visible="false" />--%>
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
                        </asp:Panel>

                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnVolverAtras3" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                </td>
                                <td>

                                    <asp:Button ID="btnGuardar3" CssClass="btn btn-primary btn-info pull-right" runat="server" Text="Guardar" OnClick="btnGuardar4_Click" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnFinalizar3" CssClass="btn  btn-success pull-right" runat="server" Text="Finalizar" OnClientClick="return Dialogo();" OnClick="btnFinalizar3_Click" />

                                    <asp:Button ID="btnCancelar3" CssClass="btn btn-primary pull-right" runat="server" Text="Cancelar" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                    <asp:Button ID="btnLimpiar3" CssClass="btn btn-warning pull-right" runat="server" Text="Limpiar" />

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
