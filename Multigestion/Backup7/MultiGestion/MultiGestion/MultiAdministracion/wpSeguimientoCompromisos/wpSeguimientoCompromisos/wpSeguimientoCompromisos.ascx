<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpSeguimientoCompromisos.ascx.cs" Inherits="MultiAdministracion.wpSeguimientoCompromisos.wpSeguimientoCompromisos.wpSeguimientoCompromisos" %>

<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script type='text/javascript' src="../../_layouts/15/MultiAdministracion/jquery-1.4.1.min.js"></script>


<style type="text/css">
    /*.WrapperDiv {
        width:auto;
    }        
    .WrapperDiv TH {
        position:relative;
    }
    .WrapperDiv TR 
    {
        height:0px;
    }*/ 
</style>

<script>
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");

        if (typeof(Sys.Browser.WebKit) == "undefined") {
            Sys.Browser.WebKit = {};
        }

        if (navigator.userAgent.indexOf("WebKit/") > -1) {
            Sys.Browser.agent = Sys.Browser.WebKit;
            Sys.Browser.version =
                parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
            Sys.Browser.name = "WebKit";
        }
    });

   function ValidarColor(fila) {
        var i = parseInt(fila) + 1;

        var grid = document.getElementById('<%=ResultadosBusqueda.ClientID %>');
        Row = grid.rows[i];

        //var a = Row.cells[10].childNodes[0].children[0].value();
        ///grid.rows[i].style.color = '#FE2E2E';
    }

    function solonumerosFechas(e) {
        var unicode = e.charCode ? e.charCode : e.keyCode
        if (unicode != 8) {
            if (unicode < 48 || unicode > 57)
                if (unicode != 45)
                    return false
        }
    }

    function Dialogo() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    }

  
</script>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>

<div id="dvFormulario" runat="server">
    <br />
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
    <br />

    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">

                    <div class="col-md-3 col-sm-6">
                        <label for="">Empresa</label>
                        <asp:TextBox ID="txtRazonSocial" placeholder="Empresa" CssClass="form-control" TextMode="Search" runat="server"></asp:TextBox>
                        <%--<div class="input-group"> 
                                   <span class="input-group-btn">
                                    <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-default" OnClientClick="return Dialogo();" OnClick="btnBuscar_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                </span>
                                   </div>
                        --%>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Mes</label>
                        <asp:DropDownList ID="ddlMes" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Etapa</label>
                        <asp:DropDownList ID="ddlEtapa" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label>Ejecutivo</label>
                        <asp:DropDownList ID="ddlEjecutivo" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>

                </div>
                <div class="row">

                    <div class="col-md-3 col-sm-6">
                        <label for="">Canal</label>
                        <asp:DropDownList ID="ddlCanal" class="form-control " runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label>Fondo</label>
                        <asp:DropDownList ID="ddlFondo" class="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-3 col-sm-6" style="padding-top: 10px">
                        <br />
                        <asp:Button ID="BtnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary btn-success" OnClientClick="return Dialogo();" OnClick="BtnBuscar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END FILTROS-->

    <!-- tabla / grilla -->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="table-responsive">
                    <div style="float: right; padding: 10px 5px;">
                        <asp:Button ID="btnGuardarResumen" runat="server" Text="Guardar" CssClass="btn btn-default" OnClientClick="return Dialogo();" OnClick="btnGuardarResumen_Click" />
                    </div>
                    <asp:GridView ID="gvTotalizado" runat="server" ClientIDMode="Static" CssClass="table table-responsive table-hover table-bordered"
                        EmptyDataText="No Hay Registros" AllowPaging="True" PageSize="50" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="IdControlPipeline" Visible="false" />
                            <asp:BoundField DataField="ConceptoDetalle" HeaderText="" />
                            <asp:BoundField DataField="MontoOperacionCLP" HeaderText="Monto Operación" />
                            <asp:BoundField DataField="ComisionCLP" HeaderText="Comisión CLP" />
                            <asp:BoundField DataField="PorcComision" HeaderText="% Comisión FLAT" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <table border="0" style="table-layout: fixed; width: 100%;">
                <tr>
                    <td>
                        <div class="card-box">
                            <div class="table-responsive">
                                <div style="float: right; padding: 10px 5px;">
                                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="btn btn-default" OnClientClick="return Dialogo();" OnClick="btnImprimir_Click" />
                                    <asp:Button ID="btnReporte" runat="server" Text="Reporte" CssClass="btn btn-default" OnClientClick="return Dialogo();" OnClick="btnReporte_Click" />
                                    <asp:Button ID="btnGuardarTodo" runat="server" Text="Guardar Todo" CssClass="btn btn-default" OnClientClick="return Dialogo();" OnClick="btnGuardarTodo_Click" />
                                </div>
                                <br />
                                <br />
                                <br />
                                <asp:GridView ID="ResultadosBusqueda" runat="server" ClientIDMode="Static" CssClass="table table-bordered table-hover"
                                    GridLines="Horizontal" ShowHeaderWhenEmpty="True"
                                    OnRowDataBound="ResultadosBusqueda_RowDataBound" OnRowCreated="ResultadosBusqueda_RowCreated"
                                    OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" RowStyle-Font-Size="Small">
                                </asp:GridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    
</div>





