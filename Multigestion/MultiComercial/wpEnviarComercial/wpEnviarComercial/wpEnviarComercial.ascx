<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpEnviarComercial.ascx.cs" Inherits="MultiComercial.wpEnviarComercial.wpEnviarComercial.wpEnviarComercial" %>

<script src="../../_layouts/15/MultiComercial/wpSociosAccionistas.js"></script>


<script type="text/javascript">
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

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Cambio de Estado</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Comercial</a>
                </li>
                <li class="active">Cambio de Estado
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
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <div class="card-box">

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

                <asp:Panel ID="Panel1" runat="server">

                    <div class="col-md-12">
                        <div class="form-group">
                            <label>Comentario</label>
                            <asp:TextBox ID="txtComentarios" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-6" id="tbRiesgo" runat="server">
                        <div class="form-group">
                            <label>Sub Estado:</label>
                            <asp:DropDownList ID="ddlRiesgoSubestado" runat="server" CssClass="form-control">
                                <asp:ListItem Value="22">Aprobado</asp:ListItem>
                                <asp:ListItem Value="9">Condicionado</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>

                </asp:Panel>

                <table style="width:100%">
                    <tr>
                        <td>
                            <div>
                                <asp:Button class="btn btn-default" ID="btnCancel" runat="server" Text=" < Volver Atrás" OnClick="btnCancel_Click" OnClientClick="return Dialogo();" />
                            </div>
                        </td>
                        <td>
                            <div>
                                <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Actualizar" OnClick="btnGuardar_Click" />
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>


    <div style="display: none">

        <h3>Resumen:</h3>

        <div class="row">
            <div class="col-md-6">
                <h4>Datos Empresa</h4>

                <div class="form-horizontal" role="form">
                    <!-- Grupo 1 -->
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label11" runat="server" Text="Rut:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbRutEmpresa" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="lbEmpresaNombre" runat="server" Text="Razón Social:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbRazonSocial" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label9" runat="server" Text="Fecha Ini. Act. Ecom:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbFecha" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label1" runat="server" Text="Teléfono"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbTelefono" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label10" runat="server" Text="Actividad"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbActividad" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label12" runat="server" Text="Nro. Empleados"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbEmpleados" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="col-md-6">
                <h4>Datos Operación</h4>

                <div class="form-horizontal" role="form">
                    <!-- Grupo 1 -->
                    <div class="col-md-12">
                        <div class="form-group">

                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label6" runat="server" Text="Producto:"></asp:Label></strong>
                            </div>

                            <div class="col-sm-10">
                                <asp:Label ID="lbProducto" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label7" runat="server" Text="Estado:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbEstado" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label8" runat="server" Text="Finalidad:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbFinalidad" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label3" runat="server" Text="Monto:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbMonto" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label4" runat="server" Text="Plazo:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbPlazo" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-2">
                                <strong>
                                    <asp:Label ID="Label5" runat="server" Text="Comisión:"></asp:Label></strong>
                            </div>
                            <div class="col-sm-10">
                                <asp:Label ID="lbComision" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>


        <div class="row clearfix">
            <div class="col-md-6 column">
                <h3>Garantias</h3>
                <div id="dvGarantias" runat="server" class="row clearfix">
                    <div class="col-md-12 column">
                        <br />
                        <asp:GridView ID="GridGarantias" runat="server" class="table table-responsive table-hover table-bordered" GridLines="Horizontal"
                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-md-6 column">
                <h3>Documentos</h3>
                <div id="dvDocumentos" runat="server" class="row clearfix">
                    <div class="col-md-12 column">
                        <br />
                        <asp:GridView ID="GridDocumentos" runat="server" class="table table-responsive table-hover table-bordered" GridLines="Horizontal"
                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>


        <div class="row clearfix">
            <div class="col-md-6 column">
                <h3>Socios y Accionistas</h3>
                <div id="dvSocios" runat="server" class="row clearfix">
                    <div class="col-md-12 column">
                        <br />
                        <asp:GridView ID="GridSocios" runat="server" class="table table-responsive table-hover table-bordered" GridLines="Horizontal"
                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-md-6 column">
                <h3>Empresas Relacionada</h3>
                <div id="dvEmpresaRelacionadas" runat="server" class="row clearfix">
                    <div class="col-md-12 column">
                        <br />
                        <asp:GridView ID="GridEmpresaRelacionada" runat="server" class="table table-responsive table-hover table-bordered" GridLines="Horizontal"
                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>


        <div class="row clearfix">
            <div class="col-md-6 column">
                <h3>Directorio</h3>
                <div id="dvDirectorio" runat="server" class="row clearfix">
                    <div class="col-md-12 column">
                        <br />
                        <asp:GridView ID="GridDirectorio" runat="server" class="table table-responsive table-hover table-bordered" GridLines="Horizontal"
                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging" PageSize="5">
                        </asp:GridView>
                    </div>
                </div>


            </div>

            <div class="col-md-6 column">
            </div>
        </div>


        <div id="dvEmpresa" runat="server" class="row clearfix">
            <div class="col-md-12 column">
                <br />
                <asp:GridView ID="GridEmpresa" runat="server" class="table table-bordered" GridLines="Horizontal"
                    RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridEmpresa_SelectedIndexChanged">
                </asp:GridView>
            </div>
        </div>


        <div id="dvNegocio" runat="server" class="row clearfix">
            <div class="col-md-12 column">
                <br />
                <asp:GridView ID="GridNegocio" runat="server" class="table table-bordered" GridLines="Horizontal"
                    RowHeaderColumn="0" ShowHeaderWhenEmpty="True" OnSelectedIndexChanged="GridNegocio_SelectedIndexChanged">
                </asp:GridView>
            </div>
        </div>


    </div>

</div>
