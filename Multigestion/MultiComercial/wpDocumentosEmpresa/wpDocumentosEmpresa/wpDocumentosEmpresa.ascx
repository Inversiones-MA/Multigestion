<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDocumentosEmpresa.ascx.cs" Inherits="MultiComercial.wpDocumentosEmpresa.wpDocumentosEmpresa.wpDocumentosEmpresa" %>


<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script src="../../_layouts/15/MultiComercial/wpDocumentosEmpresa.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>
<script type="text/javascript">

    function SetTab(panel) {
        var hdn = panel;
        if (hdn != null) {

            if (hdn.value != "") {
                var b = document.getElementById("myTab");

                switch (hdn) {
                    case 'li1':
                        $('#li1').addClass('active');
                        $('#li2').removeClass('active');
                        $('#li3').removeClass('active');
                        $('#panel-tab1').addClass('active');
                        $('#panel-tab2').removeClass('active');
                        $('#panel-tab3').removeClass('active');
                        document.getElementById("hdnTab").value = "li1"
                        break;
                    case 'li2':
                        $('#li1').removeClass('active');
                        $('#li2').addClass('active');
                        $('#li3').removeClass('active');
                        $('#panel-tab1').removeClass('active');
                        $('#panel-tab2').addClass('active');
                        $('#panel-tab3').removeClass('active');
                        document.getElementById("hdnTab").value = "li2"
                        break;
                    case 'li3':
                        $('#li1').removeClass('active');
                        $('#li2').removeClass('active');
                        $('#li3').addClass('active');
                        $('#panel-tab1').removeClass('active');
                        $('#panel-tab2').removeClass('active');
                        $('#panel-tab3').addClass('active');
                        document.getElementById("hdnTab").value = "li3"
                        break;
                        }
                    }
                }
    }

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");

        var hdn = document.getElementById("hdnTab")
        if (hdn.value != null && hdn.value != "") {
            var panel = hdn.value;
        }
        else {
            var panel = 'li1';
        }
        SetTab(panel);
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
        <h4 class="page-title">Documentos</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Datos Empresa</a>
            </li>
            <li class="active">Documentos
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
            <li class="active">
                <a href="#tab3" data-toggle="tab" aria-expanded="true">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
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
            <li class="">
                <a href="#tab5" data-toggle="tab" aria-expanded="false">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
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

                <div id="DivHidden">
                    <asp:HiddenField ID="hdnTab" runat="server" Value="" ClientIDMode="Static" />
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <BR>
                        <div class="card-box">
                            <div class="row">
                                <ul class="nav nav-tabs navtab-bg nav-justified" id="myTab">
                                    <li id="li1" class="active"><a href="#panel-tab1" data-toggle="tab" onclick="SetTab('li1')">
                                        <asp:Label ID="lbLi1" runat="server" Text="Buscar"></asp:Label>
                                    </a></li>
                                    <li id="li2"><a href="#panel-tab2" data-toggle="tab" onclick="SetTab('li2')">
                                        <asp:Label ID="lbLi2" runat="server" Text="Carga Unitaria"></asp:Label>
                                    </a></li>
                                    <li id="li3"><a href="#panel-tab3" data-toggle="tab" onclick="SetTab('li3')">
                                        <asp:Label ID="lbLi3" runat="server" Text="Carga Masiva"></asp:Label>
                                    </a></li>
                                </ul>
                                <br>
                                <br>
                                <div class="tab-content">
                                    <div class="tab-pane active" id="panel-tab1">
                                        <asp:Panel ID="pnTab3" runat="server">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="col-md-6">
                                                        <div class="column form-group row">
                                                            <label for="inputArea" class="col-sm-5 control-label">Área</label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList ID="ddlAreaBuscar" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlAreaBuscar_SelectedIndexChanged" />
                                                            </div>
                                                        </div>
                                                        <div class="column form-group row">
                                                            <label for="inputOperacion" class="col-sm-5 control-label">Operacion</label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList ID="ddlOperacionBuscar" runat="server" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- Grupo 2 -->
                                                    <div class="col-md-6">
                                                        <div class="column form-group row">
                                                            <label for="inputFechaInicio" class="col-sm-5 control-label">Fecha</label>
                                                            <div class="col-sm-7">
                                                                <%--<asp:TextBox ID="txtFecha" class="form-control" runat="server" Enabled="false"></asp:TextBox>--%>
                                                                <SharePoint:DateTimeControl ID="dtcFechaBuscar" Calendar="Gregorian" LocaleId="13322" DateOnly="true" runat="server" CssClassTextBox="form-control" TabIndex="0" />
                                                            </div>
                                                        </div>
                                                        <div class="column form-group row">
                                                            <label for="inputTipoDocumento" class="col-sm-5 control-label">Tipo Documento</label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList ID="ddlTipoDocumentoBuscar" runat="server" CssClass="form-control" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <br>
                                        <br>
                                        <table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="brnAtras2" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />


                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout:fixed; width:100%;">
                                                    <tr>
                                                        <td>
                                                             <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <div class="row">
                                                                        <asp:GridView ID="ResultadoBusquedaBuscar" runat="server" GridLines="Horizontal" class="table table-bordered table-hover" 
                                                                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowDataBound="ResultadoBusquedaBuscar_RowDataBound" 
                                                                            OnPageIndexChanging="ResultadoBusquedaBuscar_PageIndexChanging" PageSize="10" AllowPaging="True">
                                                                            <PagerStyle CssClass="pagination-ys" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>


                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <asp:Label ID="Label5" runat="server" Text="" Visible="false"></asp:Label>
                                        <input type="radio" value="option1" id="Radio1" runat="server" name="optionsRadios" visible="False">
                                        <asp:DropDownList runat="server" ID="DropDownList4" CssClass="form-control input-sm" Visible="False" />
                                        <input type="radio" value="option2" id="Radio2" runat="server" name="optionsRadios" visible="False">
                                    </div>
                                    <div class="tab-pane" id="panel-tab2">
                                        <asp:Panel ID="pnFormulario" runat="server">
                                            <div class="row clearfix margenBottom">
                                                <div class="col-md-12 column form-horizontal">
                                                    <div class="row clearfix ">
                                                        <br />
                                                        <!-- Grupo 1 -->
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputArea" class="col-sm-5 control-label">Área</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="inputTipoDocumento" class="col-sm-5 control-label">Tipo Documento</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-control" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <asp:Label ID="lbComentario" CssClass="col-sm-5 control-label" Style="text-align: right" runat="server" Text="Comentario" Font-Bold="True"></asp:Label>
                                                                <div class="col-sm-7">
                                                                    <textarea rows="5" runat="server" cols="20" id="txtComentario" class="form-control"></textarea>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <!-- Grupo 2 -->
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputFechaInicio" class="col-sm-5 control-label">Fecha</label>
                                                                <div class="col-sm-7">
                                                                    <%--<asp:TextBox ID="txtFecha" class="form-control" runat="server" Enabled="false"></asp:TextBox>--%>
                                                                    <SharePoint:DateTimeControl ID="txtFecha" Calendar="Gregorian" LocaleId="13322" DateOnly="true" runat="server" CssClassTextBox="form-control" TabIndex="0" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="inputOperacion" class="col-sm-5 control-label">Operacion</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="ddlOperaciones" runat="server" CssClass="form-control" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                    <asp:Label ID="Label1" CssClass="col-sm-5 control-label" Style="text-align: right" runat="server" Text="Documento" Font-Bold="True"></asp:Label>
                                                                    <br />
                                                                    <div class="col-sm-7">
                                                                        <asp:FileUpload ID="fileDocumento" runat="server" CssClass="form-control" />
                                                                        <br />
                                                                        <span style="color: red;" id="errorArchivo"></span>

                                                                        <script>
                                                                            var validFilesTypes = ["pdf"];
                                                                            function CheckExtension(file) {
                                                                                file.style.borderColor = "";
                                                                                document.getElementById('errorArchivo').innerHTML = "";
                                                                                var filePath = file.value;
                                                                                var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
                                                                                var isValidFile = false;
                                                                                for (var i = 0; i < validFilesTypes.length; i++) {
                                                                                    if (ext == validFilesTypes[i]) {
                                                                                        isValidFile = true;
                                                                                        break;
                                                                                    }
                                                                                }
                                                                                if (!isValidFile) {
                                                                                    file.value = null;
                                                                                    file.style.borderColor = "red";
                                                                                    document.getElementById('errorArchivo').innerHTML = "Solo formato pdf";
                                                                                }
                                                                                return isValidFile;
                                                                            }
                                                                        </script>
                                                                    </div>
                                                                </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                                </td>

                                                <td>
                                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                               <table border="0" style="table-layout:fixed; width:100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="ResultadosBusqueda" runat="server" GridLines="Horizontal" class="table table-bordered table-hover" 
                                                                        RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="ResultadosBusqueda_RowCreated" 
                                                                        OnRowDataBound="ResultadosBusqueda_RowDataBound" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="10" 
                                                                        AllowPaging="True">
                                                                        <PagerStyle CssClass="pagination-ys" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdIdNegocio" runat="server" />
                                        <asp:HiddenField ID="hdDocumento" runat="server" />
                                        <asp:Label ID="lbDocumento" runat="server" Text="" Visible="false"></asp:Label>
                                        <input type="radio" value="option1" id="rdExistente" runat="server" name="optionsRadios" visible="False">
                                        <asp:DropDownList runat="server" ID="ddlExistente" CssClass="form-control input-sm" Visible="False" />
                                        <input type="radio" value="option2" id="rdDocumento" runat="server" name="optionsRadios" visible="False">
                                    </div>
                                    <div class="tab-pane" id="panel-tab3">
                                        <div class="row clearfix">
                                            <div class="col-md-10 column form-horizontal" style="padding-top: 12px;">
                                                <div class="form-group margenBottom">
                                                    <asp:Label ID="Label2" CssClass="col-sm-3 control-label" Style="text-align: right" runat="server" Text="Documento" Font-Bold="True"></asp:Label>
                                                    <div class="col-sm-5">
                                                        <asp:FileUpload ID="fileDocumentoM" AllowMultiple="true" runat="server" />
                                                        <br />
                                                        <span style="color: red;" id="Span1"></span>

                                                        <script>
                                                            var validFilesTypes = ["pdf"];
                                                            function CheckExtension(file) {
                                                                file.style.borderColor = "";
                                                                document.getElementById('errorArchivo').innerHTML = "";
                                                                var filePath = file.value;
                                                                var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
                                                                var isValidFile = false;
                                                                for (var i = 0; i < validFilesTypes.length; i++) {
                                                                    if (ext == validFilesTypes[i]) {
                                                                        isValidFile = true;
                                                                        break;
                                                                    }
                                                                }
                                                                if (!isValidFile) {
                                                                    file.value = null;
                                                                    file.style.borderColor = "red";
                                                                    document.getElementById('errorArchivo').innerHTML = "Solo formato pdf";
                                                                }
                                                                return isValidFile;
                                                            }
                                                        </script>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <table style="width:100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAtrasM" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtrasM_Click" OnClientClick="return Dialogo();" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCancelarM" runat="server" Text="Cancelar" OnClick="btnCancelarM_Click" class="btn btn-primary pull-right" />
                                                    <asp:Button ID="btnLimpiarM" runat="server" Text="Limpiar" OnClick="btnLimpiarM_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                                                    <asp:Button ID="btnCargarM" runat="server" Text="Cargar" OnClick="btnCargarM_Click" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" style="width: 56px" />
                                                    <asp:Button ID="BtnGuardarMasivo" runat="server" Text="Guardar" OnClick="BtnGuardarMasivo_Click" CssClass="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
                                                    <%--<asp:Button ID="btnGuardarM" runat="server" Text="Guardar" OnClick="btnGuardarM_Click" class="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />

                                        <!-- tabla / grilla -->
                                        <div >
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout:fixed; width:100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                    <div class="table-responsive">
                                                        <div  runat="server" class="row clearfix">
                                                        <div class="col-md-12 column">
                                                        <asp:GridView ID="ResultadoBusquedaM" runat="server" GridLines="Horizontal" class="table table-bordered table-hover" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="ResultadoBusquedaM_RowCreated" OnRowDataBound="ResultadoBusquedaM_RowDataBound" OnPageIndexChanging="ResultadoBusquedaM_PageIndexChanging" PageSize="10" AllowPaging="True" AutoGenerateColumns="false" Width="95%">
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-Width="5%" />
                                                                <asp:BoundField DataField="NombreAntiguo" HeaderText="Nombre Antiguo" HeaderStyle-Width="20%" />
                                                                <asp:TemplateField HeaderStyle-Width="15%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbArea" runat="server" Text="Área"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlAreaTemp" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="15%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbTipoDoc" runat="server" Text="Tipo Documento"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlTipoDocTemp" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="10%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbOperacion" runat="server" Text="Operación"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlOperacionesTemp" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="20%">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbComentario" runat="server" Text="Comentario"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtComentarioTemp" runat="server"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" HeaderStyle-Width="15%" />
                                                            </Columns>
                                                        </asp:GridView>
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
        </div>

    </div>
</div>

</div>
