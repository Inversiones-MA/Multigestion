<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpSociosAccionistas.ascx.cs" Inherits="MultiComercial.wpSociosAccionistas.wpSociosAccionistas.wpSociosAccionistas" %>


<script src="../../_layouts/15/MultiComercial/wpSociosAccionistas.js"></script>

<script type="text/javascript">
    document.onkeydown = function () {
        //109->'-' 
        //56->'(' 
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
                        //else {
                        //    document.activeElement.value.keyCode = 13;
                        //} //Realiza el enter. 
                    }
                }
            }
        }
    }

</script>


 <div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
                                        <h4>
                                            <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
                                        </h4>
                                    </div>

									
									<div id="dvFormulario" runat="server">

<div class="row clearfix">
    <div class="col-md-6 column">
        <div class="btn-group btn-breadcrumb">
            <a class="btn btn-warning sinMargen"><i class="glyphicon glyphicon-home"></i>USTED ESTÁ EN:</a>
            <a class="btn btn-primary sinMargen">Datos Empresa</a>
            <a class="btn btn-info sinMargen">Socios y Accionistas</a>
        </div>
    </div>
    <div class="col-md-6 column" style="text-align: right;">
        <!-- Identificador de empresa -->
        <h3 class="sinMargen">Empresa:&nbsp;<asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></h3>

        <h5>RUT:&nbsp;<asp:Label ID="lbRut" runat="server" Text=""></asp:Label><br />
            <%--Negocio:&nbsp;<asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>--%>
            Ejecutivo:&nbsp;<asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
        </h5>
    </div>
</div>


<!-- Contenedor Tabs -->
<div class="row clearfix margenBottom">
    <div class="col-md-12 column">
        <div class="tabbable" id="tabs-empRelacionadas">
            <ul class="nav nav-tabs">
                <li>
                    <asp:LinkButton ID="lbDatosEmpresa" runat="server" OnClick="lbDatosEmpresa_Click" OnClientClick="return Dialogo();">Datos Empresa</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="lbHistoria" runat="server" OnClientClick="return Dialogo();" OnClick="lbHistoria_Click">Historia Empresa</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="lbEmpresaRelacionada" runat="server" OnClientClick="return Dialogo();" OnClick="lbEmpresaRelacionada_Click">Empresas Relacionadas</asp:LinkButton>
                </li>
                <li class="active">
                    <asp:LinkButton ID="lbSociosACCIONistas" runat="server" OnClientClick="return Dialogo();" OnClick="lbSociosACCIONistas_Click">Socios y Accionistas</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="lbDirectorio" runat="server" OnClientClick="return Dialogo();" OnClick="lbDirectorio_Click">Directorio</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="lbContacto" runat="server" OnClientClick="return Dialogo();" OnClick="lbContactos_Click">Contactos</asp:LinkButton>
                </li>

                <li>
                    <asp:LinkButton ID="lbDireccion" runat="server" OnClientClick="return Dialogo();" OnClick="lbDireccion_Click">Direcciones</asp:LinkButton>
                </li>
            </ul>
            <div class="tab-content">

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
                <div class="tab-pane active margenBottom" id="panel-tab3">
                    <asp:Panel ID="pnFormulario" runat="server">
                        <div class="form-horizontal" role="form">
                            <!-- Grupo 1 -->
                            <div class="col-md-6">

                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">RUT:</label>
                                    <div class="col-sm-7">

                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_rut" runat="server" MaxLength="8" CssClass="form-control" Width="155px" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txt_divRut" runat="server" MaxLength="1" CssClass="form-control" Width="30px" onKeyPress="return solonumerosDV(event)"></asp:TextBox>
                                                    <span style="color: red;" id="Span1"></span>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" Height="24px" ImageUrl="../../_layouts/15/images/MultiComercial/Buscar.png" Width="24px" OnClick="ImageButton1_Click" />
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                </div>





                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">Participación:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_participacion" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)" MaxLength="5"></asp:TextBox>
                                        <asp:TextBox ID="txt_participacionAux" runat="server" CssClass="form-control" Visible="false" Text="0"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">Protesto:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_protesto" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6">


                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">Nombre o Razón Social:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_nombre" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">Morosidad:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_morosidad" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <!-- fin  -->
            </div>
        </div>
    </div>
</div>


<table width="100%">
    <tr>
        <td>
            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
        </td>

        <td>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

            <asp:Button ID="btnCancel" runat="server" Text="Limpiar" OnClick="btnCancel_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
        </td>
    </tr>
</table>
<br />



<div id="dv_buscarEmpresas" runat="server" class="row clearfix">

    <div class="col-md-12 column">

        <br />
        <asp:GridView ID="ResultadosBusqueda" runat="server" GridLines="Horizontal" class="table table-responsive table-hover table-bordered" RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnRowCreated="ResultadosBusqueda_RowCreated" OnRowDataBound="ResultadosBusqueda_RowDataBound" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
        </asp:GridView>
    </div>
</div>

<asp:HiddenField ID="hdIdNegocio" runat="server" />

</div>