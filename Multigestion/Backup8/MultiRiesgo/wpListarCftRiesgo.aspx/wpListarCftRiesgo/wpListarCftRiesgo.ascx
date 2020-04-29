<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpListarCftRiesgo.ascx.cs" Inherits="MultiRiesgo.wpListarCftRiesgo.aspx.wpListarCftRiesgo.wpListarCftRiesgo" %>


<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script type="text/javascript">
    document.onkeydown = function () {

        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
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

                if (document.getElementById('<%=txtBuscar.ClientID %>').val != '') {
                    document.getElementById('<%=BtnBuscar.ClientID %>').click();

                }
            }
        }
    }


    function MantenSesion() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'actualizando su solicitud', 120, 550);
        __doPostBack('', '');
        return false;
    }

    setInterval("MantenSesion()", (1200000));

</script>

<script type="text/javascript" src="../../_layouts/15/MultiRiesgo/wpEmpresasRelacionadas.js"></script>

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
                <a href="#">Inicio</a>
            </li>
            <li class="active">Riesgo
            </li>
        </ol>
    </div>
</div>

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


    <!-- filtros-->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                        <div class="col-md-3 col-sm-6">
                            <label for="">Buscar</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" TextMode="Search"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="BtnBuscar" runat="server" CssClass="btn btn-default" OnClick="BtnBuscar_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-6">
                            <label for="">Estado</label>
                            <asp:DropDownList ID="cb_estados" runat="server" OnSelectedIndexChanged="cb_estados_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True" ClientIDMode="Static"></asp:DropDownList>
                        </div>
                        <div class="col-md-3 col-sm-6">
                            <label for="">Etapa</label>
                            <asp:DropDownList ID="cb_etapa" runat="server" OnSelectedIndexChanged="cb_etapa_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <div class="col-md-3 col-sm-6">
                            <label for="">SubEtapa</label>
                            <asp:DropDownList ID="cb_subetapa" runat="server" OnSelectedIndexChanged="cb_subetapa_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True"></asp:DropDownList>
                        </div>        
                </div>
            </div>
        </div>
    </div>

    <!-- tabla / grilla -->
    <div class="row">
        <div class="col-md-12">
            <table border="0" style="table-layout:fixed; width:100%;">
                <tr>
                    <td>
                        <div class="card-box">
                            <div class="table-responsive">
                                <asp:GridView ID="ResultadosBusqueda" runat="server" OnRowDataBound="ResultadosBusqueda_RowDataBound"
                                    GridLines="Horizontal" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="ResultadosBusqueda_RowCreated"
                                    OnDataBound="ResultadosBusqueda_DataBound" class="table table-bordered table-hover" PageSize="15"
                                    OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" AllowPaging="True" OnPageIndexChanged="ResultadosBusqueda_PageIndexChanged">
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
					</td>
				</tr>
			</table>
        </div>
    </div>

</div>