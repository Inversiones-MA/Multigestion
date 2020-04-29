<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPosicionClienteV2.ascx.cs" Inherits="MultiOperacion.wpPosicionClienteV2.wpPosicionClienteV2.wpPosicionClienteV2" %>

<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script type="text/javascript" src="../../_layouts/15/MultiOperacion/Operacion.js"></script>

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
            <h4 class="page-title">Posición Cliente</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Inicio</a>
                </li>
                <li class="active">Posición Cliente
                </li>
            </ol>
        </div>
    </div>


    <!-- Contenedor Tabs -->
    <asp:Panel ID="PnlBloqueado" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="card-box">
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
                    <table style="Width: 100%">
                        <tr>
                            <td>
                                <asp:Button ID="Button1" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                            </td>

                            <td>
                                <asp:Button ID="Button2" runat="server" Text="Imprimir" OnClick="Button2_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                            </td>
                        </tr>
                    </table>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-horizontal" role="form">
                                <asp:HiddenField ID="hdfidEmpresa" runat="server" />

                                <div class="col-md-12 col-sm-12">
                                    <h4>Datos Empresa</h4>
                                </div>

                                <!-- col 1/4 -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Razón Social</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblNombre" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">N° Empleados</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblNroEmpleados" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Tipo Empresa</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblTipoEmpresa" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Inicio Actividad</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblFechaIniAct" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Fin Contrato</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblFechaFinContra" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Email Corp</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblEmail" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Grupo Económico</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblGrupoE" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Bloqueado</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblBloqueo" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>


                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Rut</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblRut" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Años Experiencia</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblAnosExperiencia" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Actividad</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblActividad" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Inicio Contrato</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblFechaIniContra" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Ejecutivo</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblEjecutivo" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Télefono</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblTelefono" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Nombre Grupo Ecónomico</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblNombreGrupoE" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Motivo Bloqueo</label>
                                        <div class="col-sm-7">
                                            <asp:Label ID="lblMotivoBloqueo" CssClass="form-control" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Historia</label>
                                    <asp:TextBox ID="lblHistoria" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10"></asp:TextBox>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Direcciones</h4>
                                </div>

                                <!-- tabla / grilla -->

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridDirecciones" runat="server" GridLines="Horizontal" CssClass="table table-bordered table-hovered" RowHeaderColumn="0"
                                                            ShowHeaderWhenEmpty="True" PageSize="10" OnPageIndexChanging="gridDirectorios_PageIndexChanging">
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                            <!-- tabla / grilla -->
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Empresas Relacionadas</h4>
                                </div>

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridRelacionadas" runat="server" GridLines="Horizontal"
                                                            CssClass="table table-bordered table-hovered" RowHeaderColumn="0"
                                                            ShowHeaderWhenEmpty="True" PageSize="10"
                                                            OnPageIndexChanging="gridRelacionadas_PageIndexChanging">
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>

                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Socios y Accionistas</h4>
                                </div>

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridSocioa" runat="server" GridLines="Horizontal"
                                                            CssClass="table table-bordered table-hovered" RowHeaderColumn="0"
                                                            ShowHeaderWhenEmpty="True" PageSize="10"
                                                            OnPageIndexChanging="gridSocioa_PageIndexChanging">
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Directorio</h4>
                                </div>

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridDirectorios" runat="server" GridLines="Horizontal"
                                                            class="table table-bordered table-hovered" RowHeaderColumn="0"
                                                            ShowHeaderWhenEmpty="True" PageSize="10"
                                                            OnPageIndexChanging="gridDirectorios_PageIndexChanging">
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Grupo Económico</h4>
                                </div>

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridGrupoEconomico" runat="server" GridLines="Horizontal"
                                                            class="table table-bordered table-hovered" RowHeaderColumn="0"
                                                            ShowHeaderWhenEmpty="True" PageSize="10"
                                                            OnPageIndexChanging="gridDirectorios_PageIndexChanging">
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
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
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Garantías - Empresa</h4>
                                </div>

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridGarantiaEmpresa" runat="server" GridLines="Horizontal"
                                                            class="table table-bordered table-hovered" AutoGenerateColumns="false"
                                                            RowHeaderColumn="0" ShowHeaderWhenEmpty="True" PageSize="15" ShowFooter="true" EmptyDataText="Sin Información"
                                                            OnPageIndexChanging="gridGarantias_PageIndexChanging" OnRowDataBound="gridGarantiaEmpresa_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="Nro. Inscripción" HeaderText="Nro. Inscripción" />
                                                                <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                                                                <asp:BoundField DataField="Clase Garantía" HeaderText="Clase Garantía" />
                                                                <asp:BoundField DataField="Tipo Bien" HeaderText="Tipo Bien" />
                                                                <asp:BoundField DataField="Valor Ajustable UF" HeaderText="Valor Ajustado UF" />
                                                                <asp:BoundField DataField="Valor Comercial UF" HeaderText="Valor Comercial UF" />
                                                                <asp:BoundField DataField="Valor Asegurable UF" HeaderText="Valor Asegurable UF" />
                                                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                                <asp:BoundField DataField="Acción" HeaderText="Acción" />
                                                                <asp:BoundField DataField="IdGarantia" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Certificados Emitidos</h4>
                                </div>

                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridCertificados" runat="server" GridLines="Horizontal"
                                                            class="table table-bordered table-hovered" AutoGenerateColumns="false" RowHeaderColumn="0"
                                                            ShowHeaderWhenEmpty="True" ShowHeader="true" PageSize="20" ShowFooter="true" EmptyDataText="Sin Información"
                                                            OnPageIndexChanging="gridCertificados_PageIndexChanging" OnRowDataBound="gridCertificados_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="IdOperacion" HeaderText="N° Operación" />
                                                                <asp:BoundField DataField="Nº CF - Producto" HeaderText="Nº CF - Producto" />
                                                                <asp:BoundField DataField="Finalidad" HeaderText="Finalidad" />
                                                                <asp:BoundField DataField="Fondo" HeaderText="Fondo" />
                                                                <asp:BoundField DataField="Prog. Corfo" HeaderText="Prog. Corfo" />
                                                                <asp:BoundField DataField="% Cob. CF" HeaderText="% Cob. CF" />
                                                                <asp:BoundField DataField="Estado Cetificado / Crédito" HeaderText="Estado Cetificado / Crédito" />
                                                                <asp:BoundField DataField="Comisión CLP" HeaderText="Comisión CLP" />
                                                                <asp:BoundField DataField="Seguro CLP" HeaderText="Seguro CLP" />
                                                                <asp:BoundField DataField="Gastos Operacionales CLP" HeaderText="Gastos Operacionales CLP" />
                                                                <asp:BoundField DataField="Monto Operación UF" HeaderText="Monto Operación UF" />
                                                                <asp:BoundField DataField="Saldo Vigente UF" HeaderText="Saldo Vigente UF" />

                                                                <asp:BoundField DataField="Calendario" HeaderText="Calendario Pago" />
                                                                <asp:BoundField DataField="Acción" HeaderText="Acción" />
                                                                <asp:BoundField DataField="DescEjecutivo" />
                                                                <asp:BoundField DataField="DescEtapa" />
                                                                <asp:BoundField DataField="DescSubEtapa" />
                                                                <asp:BoundField DataField="NCertificado" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>


                            <div class="row">
                                <div class="form-horizontal" role="form">
                                    <div class="col-md-12 col-sm-12">
                                        <h4>Datos Propuesta Afianzamiento Actual Nº
                                <asp:Label ID="lblIdPAF" runat="server" Text="Label"></asp:Label>
                                        </h4>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Analista</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblAnalista" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Estado Linea</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblEdoLinea" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Ranking</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblRanking" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Fecha Clasificación</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblFechaClasificacion" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Fecha Aprobación</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblFechaAprobacion" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Nivel Atribución</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblNivelAribucion" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Clasificación Cliente</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblClasificacion" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Nivel Ventas UF</label>
                                            <div class="col-sm-7">
                                                <asp:Label ID="lblVentas" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <br />
                            <table class="table table-responsive table-hover table-bordered grid">
                                <tr>
                                    <td></td>
                                    <td><b>Monto Aprobado</b></td>
                                    <td><b>Monto Vigente</b></td>
                                    <td><b>Monto Propuesto </b></td>
                                </tr>
                                <tr>
                                    <td><b>Total Riesgo Equivalente UF </b></td>
                                    <td>
                                        <asp:Label ID="lblMontoAprobado" runat="server"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblMontoVigente" runat="server"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:Label ID="lblMontoPropuesto" runat="server"></asp:Label>

                                    </td>
                                </tr>

                            </table>
                            <br />
                            <table class="table table-responsive table-hover table-bordered grid">
                                <tr>
                                    <td></td>
                                    <td><b>Valor Comercial</b></td>
                                    <td><b>Valor Ajustado</b></td>

                                </tr>


                                <tr>
                                    <td><b>Cobertura Global Última PAF %</b></td>
                                    <td>
                                        <asp:TextBox ID="txtCoberturaComercial" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtCoberturaAjustado" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                </tr>
                            </table>
                            <br />

                            <table class="table table-responsive table-hover table-bordered grid">
                                <tr>
                                    <td></td>
                                    <td><b>Valor Comercial</b></td>
                                    <td><b>Valor Ajustado</b></td>

                                </tr>


                                <tr>
                                    <td><b>Cobertura Global Vigente %</b></td>
                                    <td>
                                        <asp:TextBox ID="txtCoberturaComercialVigente" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtCoberturaAjustadoVigente" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                </tr>
                            </table>


                        </div>
                    </div>


                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button ID="Button3" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                            </td>

                            <td>
                                <asp:Button ID="Button4" runat="server" Text="Imprimir" OnClick="btnImprimir_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </asp:Panel>
</div>
