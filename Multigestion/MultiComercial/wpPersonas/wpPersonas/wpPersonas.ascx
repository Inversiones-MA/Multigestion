<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPersonas.ascx.cs" Inherits="MultiComercial.wpPersonas.wpPersonas.wpPersonas" %>


<script src="../../_layouts/15/MultiComercial/wpPersonas.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    });

    document.onkeydown = function () {
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
        }

        if (window.event && (window.event.keyCode == 8)) {
            valor = document.activeElement.value;
            if (valor == undefined) { return false; } //Evita Back en página. 

            else {
                if (document.activeElement.getAttribute('type') == 'select-one')
                { return false; } //Evita Back en select. 
                if (document.activeElement.getAttribute('type') == 'button')
                { return false; } //Evita Back en button. 
                if (document.activeElement.getAttribute('type') == 'radio')
                { return false; } //Evita Back en radio. 
                if (document.activeElement.getAttribute('type') == 'checkbox')
                { return false; } //Evita Back en checkbox. 
                if (document.activeElement.getAttribute('type') == 'file')
                { return false; } //Evita Back en file. 
                if (document.activeElement.getAttribute('type') == 'reset')
                { return false; } //Evita Back en reset. 
                if (document.activeElement.getAttribute('type') == 'submit')
                { return false; } //Evita Back en submit. 
                if (document.activeElement.getAttribute('type') == 'input')
                { return false; } //Evita Back en inout. 
                else //Text, textarea o password 
                {
                    if (document.activeElement.value.length == 0)
                    { return false; } //No realiza el backspace(largo igual a 0). 
                    else {
                        document.activeElement.value.keyCode = 8;
                    } //Realiza el backspace. 
                }
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
                    }
                }
            }
        }
    }

   <%-- $(document).ready(function () {
        ValidarIngresoRut();
    });

    function ValidarIngresoRut() {
        var contacto = $("#<%=ddlContacto.ClientID%>");
        var contactoSelect = contacto.find("option:selected").text();
        alert(contactoSelect);
    }--%>

    //function ValidarRegimen() {
      //  var producto = $("#<%=ddlEdoCivil.ClientID%>");
      //  var productoSelect = producto.find("option:selected").text();
   
      //  if (productoSelect !== null && productoSelect !== undefined) {
      //      if (productoSelect == 'Casado' || productoSelect == 'Conviviente Civil') {
      //          document.getElementById('divRegimen').style.display = 'block';
      //      }
      //  }
    //}
</script>

<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
     <h4>
           <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
     </h4>
</div>
									
<div id="dvFormulario" runat="server" clientidmode ="static">

<!-- Page-Title -->
<div class="row marginInicio">
    <div class="col-sm-12">
        <h4 class="page-title">Personas</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Datos empresa</a>
            </li>
            <li class="active">Personas
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
        <div class="tabbable" id="tabs-personas">
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
                <li class="">
                    <a href="#tab3" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
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
                <li class="active">
                    <a href="#tab6" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbPersonas" runat="server" OnClientClick="return Dialogo();" OnClick="lbPersonas_Click">Personas</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane active" id="tab6">
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
                                  
                                        <div class="tab-pane active margenBottom" id="panel-tab3">
                                            <asp:Panel ID="pnFormulario" runat="server">
                                                <div class="form-horizontal" role="form">
                                                    <!-- Grupo 1 -->
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label for="inputPassword3" class="col-sm-5 control-label">RUT:</label>
                                                            <div class="col-sm-7">
                                                                <table style="width:100%;">
                                                                    <tr>
                                                                        <td style="width:70%;">
                                                                            <asp:TextBox ID="txt_rut" runat="server" MaxLength="8" CssClass="form-control" style="width:100%;" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txt_divRut" runat="server" MaxLength="1" CssClass="form-control" onKeyPress="return solonumerosDV(event)"></asp:TextBox>
                                                                                <span style="color: red;" id="errorRut"></span>
                                                                                <span class="input-group-btn">
                                                                                    <asp:LinkButton ID="ImageButton2" runat="server" CssClass="btn btn-default" OnClick="ImageButton2_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                </span>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="inputNombre" class="col-sm-5 control-label">Nombre:</label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txt_nombre" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="inputProfesion" class="col-sm-5 control-label">Profesión:</label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txt_profesion" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="inputCargo" class="col-sm-5 control-label">Cargo:</label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList runat="server" ID="ddlCargos" CssClass="form-control"></asp:DropDownList>
                                                            </div>


                                                        </div>
                                                    </div>
                                                    <!-- Grupo 2 -->
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <asp:Label ID="inputFecNac" CssClass="col-sm-5 control-label" runat="server" Text="Fecha de Nacimiento:" Font-Bold="True"></asp:Label>
                                                            <div class="col-sm-7">
                                                                <SharePoint:DateTimeControl ID="dtcFecNaci" runat="server" LocaleId="13322" Calendar="Gregorian" DateOnly="true" CssClassTextBox="form-control" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="inputAntiguedad" class="col-sm-5 control-label">Antigüedad:</label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txt_antiguedad" runat="server" CssClass="form-control" MaxLength="5" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="inputUniversidad" class="col-sm-5 control-label">Universidad:</label>
                                                            <div class="col-sm-7">
                                                                <asp:TextBox ID="txt_universidad" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="inputEdoCivil" class="col-sm-5 control-label">Estado Civil:</label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList runat="server" ID="ddlEdoCivil" CssClass="form-control" OnSelectedIndexChanged="ddlEdoCivil_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group" id="divRegimen" style="display:none" runat="server">
                                                            <label for="inputRegimen" class="col-sm-5 control-label">Regimen:</label>
                                                            <div class="col-sm-7">
                                                                <asp:DropDownList runat="server" ID="ddlRegimen" CssClass="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <div class="col-md-12">
                                                        <div class="form-group">
                                                            <label for="inputSocios" class="col-sm-2 control-label">Socio/Accionista:</label>
                                                            <div class="col-sm-1">
                                                                <asp:DropDownList runat="server" ID="ddlSocios" CssClass="form-control">
                                                                    <asp:ListItem Selected="True" Text="No" Value="0" />
                                                                    <asp:ListItem Text="Si" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <label for="inputDirectorio" class="col-sm-2 control-label">Directorio:</label>
                                                            <div class="col-sm-1">
                                                                <asp:DropDownList runat="server" ID="ddlDirectorio" CssClass="form-control">
                                                                    <asp:ListItem Selected="True" Text="No" Value="0" />
                                                                    <asp:ListItem Text="Si" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <label for="inputContacto" class="col-sm-2 control-label">Contacto:</label>
                                                            <div class="col-sm-1">
                                                                <asp:DropDownList runat="server" ID="ddlContacto" CssClass="form-control">
                                                                    <asp:ListItem Selected="True" Text="No" Value="0" />
                                                                    <asp:ListItem Text="Si" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <label for="inputRteLegal" class="col-sm-2 control-label">Rte. Legal:</label>
                                                            <div class="col-sm-1">
                                                                <asp:DropDownList runat="server" ID="ddlRteLegal" CssClass="form-control">
                                                                    <asp:ListItem Selected="True" Text="No" Value="0" />
                                                                    <asp:ListItem Text="Si" Value="1" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                  
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                <div class="panel panel-default">
                                    <div class="panel-heading" role="tab" id="headingOne">
                                        <h4 class="panel-title">
                                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Socios y Accionistas
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="card-box">
                                                        <div class="row">
                                                            <div class="form-horizontal" role="form">
                                                                <!-- Grupo 1 -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label for="inputParticipacion" class="col-sm-5 control-label">Participación:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_participacion" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="inputPatrimonio" class="col-sm-5 control-label">Patrimonio:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_patrimonio" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Grupo 2 -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label for="inputProtesto" class="col-sm-5 control-label">Protesto:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_protesto" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="inputMorosidad" class="col-sm-5 control-label">Morosidad:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_morosidad" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
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
                                <div class="panel panel-default">
                                    <div class="panel-heading" role="tab" id="headingTwo">
                                        <h4 class="panel-title">
                                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Contacto
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="card-box">
                                                        <div class="row">
                                                            <div class="form-horizontal" role="form">
                                                                <!-- Grupo 1 -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label for="inputEmail" class="col-sm-5 control-label">Email:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_email" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="inputTelefonoFijo" class="col-sm-5 control-label">Teléfono Fijo:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_telFijo" runat="server" CssClass="form-control" MaxLength="15" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!-- Grupo 2 -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label for="inputCelular" class="col-sm-5 control-label">Celular:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txt_celular" runat="server" CssClass="form-control" MaxLength="15" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="inputPrincipal" class="col-sm-5 control-label">Principal:</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlContactoPrincipal" CssClass="form-control">
                                                                                <asp:ListItem Selected="True" Text="No" Value="0" />
                                                                                <asp:ListItem Text="Si" Value="1" />
                                                                            </asp:DropDownList>
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
                   
                                <table class="col-sm-12 col-md-12 col-lg-12">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" CssClass="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" CssClass="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />
                                        </td>
                                    </tr>
                                    <tr><td colspan="2">&nbsp;<br></td></tr>
                                    <tr>
                                        <td colspan="2">
                                            <table border="0" style="table-layout:fixed; width:100%;">
                                                <tr>
                                                    <td>
                                                        <div class="table-responsive">
                                                            <div class="row">
                                                                <asp:GridView ID="ResultadosBusqueda" runat="server" CssClass="table table-bordered table-hover" GridLines="Horizontal" OnRowCreated="ResultadosBusqueda_RowCreated" ClientIDMode="Static"
                                                                OnRowDataBound="ResultadosBusqueda_RowDataBound" ShowHeaderWhenEmpty="true" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5" AutoGenerateColumns="true" ShowHeader="true">
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                    <div class="col-md-12 column">                                        
                                        <asp:HiddenField runat="server" ID="hdnIdPersona" Value="-1" />
                                    </div>
                         </div>
                    
                </div>
            </div>
        </div>
    </div>
</div>

</div>