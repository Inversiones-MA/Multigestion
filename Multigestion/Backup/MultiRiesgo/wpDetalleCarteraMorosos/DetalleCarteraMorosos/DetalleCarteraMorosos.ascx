<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetalleCarteraMorosos.ascx.cs" Inherits="MultiRiesgo.wpDetalleCarteraMorosos.DetalleCarteraMorosos.DetalleCarteraMorosos" %>


<script type="text/javascript">



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
            <h4 class="page-title">Riesgo</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Detalle Cartera Morosos</a>
                </li>
                <li class="active">Detalle Cartera Morosos</li>
            </ol>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <h4>Detale Certificado</h4>
                    </div>

                    <div class="form-horizontal" role="form">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="col-sm-5 control-label">Empresa</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                           <%-- <div class="form-group">
                                <label class="col-sm-5 control-label">Dirección</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>--%>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">N° Certificado</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtNroCertificado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Fondo</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtFondo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Dias Mora</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDiasMora" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Capital</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCapital" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Monto Mora</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMontoMora" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Cuotas Pagadas</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCuotasPagadas" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Saldo Certificado</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSaldoCertificado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Fogape(SM)</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtFogape" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-sm-5 control-label">Deuda CF / Valor Fondo</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeudaCF" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="col-md-6">

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Giro</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtGiro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <%--<div class="form-group">
                                    <label class="col-sm-5 control-label">Telefono</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Acreedor</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAcreedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Tramo</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtTramo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Intereses</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtIntereses" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Cuotas Mora</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtCuotasMora" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Cuotas Pactadas</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtCuotasPactadas" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Cuotas Pactasdas</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Garantia VA</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtGarantia" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Deuda CF / Deuda Total</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-sm-5 control-label">% Cobertura</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtCobertura" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>


                        </div>

                    </div>
                </div>


                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <h4>Contactos</h4>
                    </div>

                    <div class="col-md-12">
                        <table border="0" style="table-layout: fixed; width: 100%;">
                            <tr>
                                <td>
                                    <div class="card-box">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gridContactos" runat="server" GridLines="Horizontal"
                                                class="table table-bordered table-hovered"
                                                RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="10"
                                                OnPageIndexChanging="gridContactos_PageIndexChanging">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <!-- BOTONES -->
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>

            </div>
        </div>
    </div>



</div>
