<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpIngresoOperacion.ascx.cs" Inherits="MultiComercial.wpIngresoOperacion.wpIngresoOperacion.wpIngresoOperacion" %>


<script src="../../_layouts/15/MultiComercial/Validaciones.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    });

    function fnRetornar() {
        Dialogo();
        var area = document.getElementById("<%=hfArea.ClientID%>");
        window.location.href = "/SitePages/" + area.value;
    }

    function HabilitarEmpresa(cad) {

        var idPanel = cad + 'pnlEdit';

        var idGuardar = cad + 'btnGuardar';
        var idLimpiar = cad + 'btnLimpiar';

        document.getElementById(idGuardar).style.display = 'block';
        document.getElementById(idLimpiar).style.display = 'block';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].disabled = false;
        };

        return false;
    }

    function solonumerosCant(e, id) {
        var val = document.getElementById(id).value;
        var unicode = e.charCode ? e.charCode : e.keyCode
        if (unicode == 44) {
            val = val.replace(/[,]/gi, '');
            document.getElementById(id).value = val;
        }

        if (unicode != 8) {
            if (unicode < 48 || unicode > 57) {
                if (unicode != 46 && unicode != 44)
                    return false
            }
        }
    }

    function LimpiarVacido(cad) {

        var idPanel = cad + 'pnlOperacion';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].value = "";
        };

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("select");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].selectedIndex = 0;
        };


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
            <h4 class="page-title">Datos Empresa</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Creación de Operación</a>
                </li>
                <li class="active">Datos Empresa
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
                        <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong>
                    /
                    <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>


    <!-- filtros-->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <div class="form-horizontal" role="form">

                        <asp:Panel runat="server" ID="pnlOperacion">
                            <asp:HiddenField ID="hfArea" runat="server" />
                            <asp:HiddenField ID="hfDecimales" runat="server" />

                            <div class="row">
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
                            </div>

                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <asp:Label ID="Label1" CssClass="col-sm-5 control-label" runat="server" Text="Empresa" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lblEmpresa" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="Label4" CssClass="col-sm-5 control-label" runat="server" Text="Ejecutivo" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="LblEjecutivo" runat="server" CssClass="form-control"></asp:Label>
                                        <%--<asp:DropDownList ID="ddlEjecutivo" CssClass="form-control input-sm col-md-3" runat="server" Visible="false"/>--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label ID="Label3" CssClass="col-sm-5 control-label" runat="server" Text="Finalidad" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList runat="server" ID="ddlFinalidad" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label7" CssClass="col-sm-5 control-label" runat="server" Text="Monto Operación" Font-Bold="True" />
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtMntOper" MaxLength="12" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label9" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Estimada Cierre" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <SharePoint:DateTimeControl ID="dtcFecEmis" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-6">
                                <div class="form-group">
                                    <asp:Label ID="Label2" CssClass="col-sm-5 control-label" runat="server" Text="Producto" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList runat="server" ID="ddlProducto" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <%-- <div class="form-group" style="display: none">
                                        <asp:Label ID="Label5" CssClass="col-sm-5 control-label" runat="server" Text="Canal" Font-Bold="True"></asp:Label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList runat="server" ID="ddlCanal" Width="200px" CssClass="form-control input-sm">
                                            </asp:DropDownList>
                                        </div>
                                    </div>--%>
                                <div class="form-group">
                                    <asp:Label ID="Label6" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Moneda" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList runat="server" ID="ddlTipoM" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label8" CssClass="col-sm-5 control-label" runat="server" Text="Plazo(Meses)" Font-Bold="True" />
                                    <div class="col-sm-7">
                                        <asp:TextBox runat="server" ID="txtPlazo" MaxLength="3" CssClass="form-control" onKeyPress="return solonumeros(event)" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success pull-right" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </div>

</div>

<%--<div class="row clearfix margenBottom">
    <div class="col-md-12 column">
        <div class="tabbable" id="tabs-empRelacionadas">
            <div class="tab-content">
                <div class="tab-pane active" id="panel-tab2">
                   
                    <br />
                    <asp:LinkButton ID="lbEdit" runat="server" CssClass="fa fa-edit paddingIconos text-muted pull-right" ToolTip="Editar" Text=" Editar"></asp:LinkButton>--%>
                    <%--
                        
                        <div class="row clearfix margenBottom">
                            <div class="form-horizontal" role="form">
                                <div class="col-md-6">
                                                                     
                                  
                                </div>
                             
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row clearfix margenBottom">
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>--%>
