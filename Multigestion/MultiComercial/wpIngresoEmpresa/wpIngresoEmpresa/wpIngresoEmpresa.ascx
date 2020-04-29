<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpIngresoEmpresa.ascx.cs" Inherits="MultiComercial.wpIngresoEmpresa.wpIngresoEmpresa.wpIngresoEmpresa" %>

<script src="../../_layouts/15/MultiComercial/Validaciones.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.8.3.min.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery.mask.min.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    })

    function fnRetornar() {
        Dialogo();
        var area = document.getElementById("<%=hfArea.ClientID%>");
        window.location.href = "/SitePages/" + area.value;
    }

    function LimpiarVacido(cad) {

        var idPanel = cad + 'pnlEmpresa';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].value = "";
        };

        return false;
    }

    function DisableRightClick(event) {
        //For mouse right click 
        if (event.button == 2) {

        }
    }
    function DisableCtrlKey(e) {
        var code = (document.all) ? event.keyCode : e.which;
        // look for CTRL key press
        if (parseInt(code) == 17) {
            window.event.returnValue = false;
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
        <h4 class="page-title">Empresa</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Inicio</a>
            </li>
            <li class="active">Crear Empresa
            </li>
        </ol>
    </div>
</div>

<asp:HiddenField ID="hfArea" runat="server" />

<br />
  <div id="dvExito" runat="server" style="display: none" class="alert alert-success" role="alert">
                            <asp:Label ID="lbExito" runat="server"></asp:Label>
                        </div>

  <div id="dvInfo" runat="server" style="display: none" class="alert alert-info" role="alert">
                            <asp:Label ID="lbInfo" runat="server"></asp:Label>
                        </div>

  <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
                            <asp:Label ID="lbWarning" runat="server"></asp:Label>
                        </div>

  <div id="dvError" style="display: none" class="alert alert-danger" role="alert" runat="server">
                            <asp:Label ID="lbError" runat="server"></asp:Label>
                        </div>

  <div id="dvCustom" class="alert alert-danger col-md-12" role="alert" style="display: none;" runat="server">
                            <asp:Label ID="lblMsgVal" runat="server"></asp:Label>
                        </div>

<br />


<!-- filtros-->
<div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                        <div class="col-md-12">
                            <h4>Datos Empresa</h4>
                        </div>
                        
                        <div class="form-horizontal" role="form">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Rut</label>
                                    <div class="col-sm-7">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtRut" runat="server" MaxLength="8" CssClass="form-control" onKeyPress="return solonumeros(event)" Width="155px" oncopy="return false" onKeyDown="return DisableCtrlKey(event)">
                                                        
                                                    </asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtDV" MaxLength="1" runat="server" CssClass="form-control" onKeyPress="return solonumerosDV(event)" Width="30px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                 </div>
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Ejecutivo Cuentas</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList runat="server" ID="ddlEjecutivo" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Nombre o Razon Social</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtRazon" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-5 control-label">Fecha Inicio Actividad</label>
                                    <div class="col-sm-7">
                                        <SharePoint:DateTimeControl ID="dtcIniAct" Calendar="Gregorian" LocaleId="13322" DateOnly="true" runat="server" CssClassTextBox="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <h4>Datos Contacto Empresa</h4>
                        </div>
                     
                        <div class="form-horizontal" role="form">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">RUT:</label>
                                    <div class="col-sm-7">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_rutContacto" runat="server" MaxLength="8" CssClass="form-control" Width="155px" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txt_divRutContacto" runat="server" MaxLength="1" CssClass="form-control" Width="30px" onKeyPress="return solonumerosDV(event)"></asp:TextBox>

                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:LinkButton ID="ImageButton2" runat="server" CssClass="btn btn-default" OnClick="ImageButton2_Click1"><i class="fa fa-search"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="Label7" runat="server" Text="Tel. Fijo" CssClass="col-sm-5 control-label" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtFijo" MaxLength="9" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Cargo" CssClass="col-sm-5 control-label" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList runat="server" ID="ddlCargos" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label9" runat="server" Text="Principal" CssClass="col-sm-5 control-label" Font-Bold="True" />
                                    <div class="col-sm-7">
                                        <asp:CheckBox ID="chkDefecto" runat="server" Checked="true"/>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label6" runat="server" Text="Nombre / Razón Social" CssClass="col-sm-5 control-label" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtNombres" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label8" runat="server" Text="Email" CssClass="col-sm-5 control-label" Font-Bold="True" />
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtMail" MaxLength="50" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label10" runat="server" Text="Tel. Celular" CssClass="col-sm-5 control-label" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtCelu" MaxLength="9" CssClass="form-control" onKeyPress="return solonumeros(event)" ClientIDMode="Static">       
                                        </asp:TextBox>
                                    </div>
                                </div>

                            </div>
                     
                        </div>
                        <br />
                        <br />
                        
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn-warning pull-right" OnClick="btnLimpiar_Click" />
                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success pull-right" OnClientClick="return Dialogo();" OnClick="btnGuardar_Click" />
                                    </td>
                                </tr>
                            </table>
                      
                        <div id="dialog-Operacion" style="display: none">
                            <div class="ui-widget ui-widget-content ui-corner-all">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 1%">
                                            <div class="ui-state-default ui-corner-all" title=".ui-icon-alert">
                                                <span class="ui-icon ui-icon-alert"></span>
                                            </div>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Label ID="Lbl_Operacion" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    <asp:HiddenField ID="HfEtapa" runat="server" ClientIDMode="Static"/>
                </div>
            </div>
        </div>
    </div>
 </div>




