<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPagar.ascx.cs" Inherits="MultiContabilidad.wpPagar.wpPagar.wpPagar" %>

<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>
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

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
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
            <h4 class="page-title">Contabilidad</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Contabilidad</a>
                </li>
                <li class="active">Generar Transacción
                </li>
            </ol>
        </div>
    </div>

    <!-- Empresa -->
    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-gray">
                <p><strong>Empresa:
                    <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong> /
                    <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label></p>
            </div>
        </div>
    </div>



    <!-- Contenedor Tabs -->
    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li>
                    <a href="#tab1" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="liGenerar" runat="server" OnClientClick="return Dialogo();" OnClick="lbGenerar_Click">Facturación</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#tab2" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="liPagar" runat="server" OnClientClick="return Dialogo();" OnClick="lbPagar_Click">Conciliación</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>

            <div class="tab-content">
                <div class="tab-pane active" id="tab2">
                    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
                        <h4>
                            <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>

                    <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
                        <h4>
                            <asp:Label ID="lbWarning" runat="server" Text=" "></asp:Label>
                        </h4>
                    </div>

                    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
                        <h4>
                            <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>


                    <asp:Panel ID="pnDatosEmpresa" runat="server" Enabled="true">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <asp:TextBox ID="idTransaccion" runat="server" Visible="false"></asp:TextBox>
                                <h4>
                                    <asp:Label ID="lbTitulo1" runat="server" Text="Label"> Datos Empresa</asp:Label></h4>
                            </div>
                            <!-- col 1/4 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label for="">Empresa</label>
                                    <asp:TextBox ID="txtRanzonSocial" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="">Actividad Económica:</label>
                                    <asp:TextBox ID="txtActEconomica" runat="server" CssClass="form-control " ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="">Direccion</label>
                                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Provincia</label>
                                    <asp:TextBox ID="txtProvincia" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Telefono Fijo</label>
                                    <asp:TextBox ID="txtTelefonof" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>

                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Rut</label>
                                    <asp:TextBox ID="txtRut" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Comuna</label>
                                    <asp:TextBox ID="txtComuna" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Region</label>
                                    <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Email Corp</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnDatos" runat="server">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <h4>
                                    <asp:Label ID="lbTituloh" runat="server" Text="Label"> Datos Certificado</asp:Label></h4>
                            </div>

                            <!-- Grupo 1 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Nº Certificado:</label>
                                    <asp:TextBox ID="txtCertificado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Producto:</label>
                                    <asp:TextBox ID="txtProducto" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Fecha Emisión:</label>
                                    <asp:TextBox ID="txtFechaEmision" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Plazo (meses):</label>
                                    <asp:TextBox ID="txtPlazo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Monto Comisión CLP:</label>
                                    <asp:TextBox ID="txtComisionCLp" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbcomision" runat="server" />
                                    Incluye comisión en el crédito
                                </div>
                                <div class="form-group">
                                    <label>Monto Seguro Incendio:</label>
                                    <asp:TextBox ID="txtSeguro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbseguro" runat="server" />
                                    Incluye seguro incendio
                                </div>
                                <div class="form-group">
                                    <label>Monto Seguro Desgravamen:</label>
                                    <asp:TextBox ID="txtSeguroDesgravamen" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbSeguroDesgravamen" runat="server" />
                                    Incluye seguro desgravamen
                                </div>
                                <div class="form-group">
                                    <label>Notario:</label>
                                    <asp:TextBox ID="txtNotario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbNotario" runat="server" />
                                    Incluye notario
                                </div>
                            </div>

                            <!-- Grupo 2 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Acreedor:</label>
                                    <asp:TextBox ID="txtAcreedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Monto Crédito:</label>
                                    <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Fecha Vencimiento:</label>
                                    <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Fondo:</label>
                                    <asp:TextBox ID="txtFondo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Servicios de Gestion Legal CLP:</label>
                                    <asp:TextBox ID="txtGastosOperacionales" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbgastosOpe" runat="server" />
                                    Incluye servicios de gestión legal en el crédito
                                </div>
                                <div class="form-group">
                                    <label>Timbre Impuesto y Estampilla (Cargo Multiaval):</label>
                                    <asp:TextBox ID="txtTimbreyEstMultiaval" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbTimbreyEstMultiaval" runat="server" />
                                    Incluye Timbre Impuesto y Estampilla (Cargo Acreedor)
                                </div>
                                <div class="form-group">
                                    <label>Timbre Impuesto y Estampilla (Cargo Acreedor):</label>
                                    <asp:TextBox ID="txtTimbreyEstAcreedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:Label ID="lbTimbreyEstAcreedor" runat="server" />
                                    Incluye Timbre Impuesto y Estampilla (Cargo Multiaval)
                                </div>
                            </div>
                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnDatosDevolucion" runat="server">
                        <div class="row">
                            <!-- Grupo 1 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Nº Certificado:</label>
                                    <asp:TextBox ID="txtCertificadoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Producto:</label>
                                    <asp:TextBox ID="txtProductoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Fondo:</label>
                                    <asp:TextBox ID="txtFondoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Fecha Emisión:</label>
                                    <asp:TextBox ID="txtFechaEmisionD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Monto Comisión CLP:</label>
                                    <asp:TextBox ID="txtComisionCLPD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Nro Factura Original:</label>
                                    <asp:TextBox ID="txtNroFacturaComisión" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Gastos Operacionales Pendientes:</label>
                                    <asp:TextBox ID="txtGatosOperacionalesP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Devolución Final:</label>
                                    <asp:TextBox ID="txtDevolucionFinal" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Monto Crédito:</label>
                                    <asp:TextBox ID="txtMontoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Acreedor:</label>
                                    <asp:TextBox ID="txtAcreedorD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Costo Fondo:</label>
                                    <asp:TextBox ID="txtCostoFondoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Fecha Factura:</label>
                                    <asp:TextBox ID="txtFechaFacturaComision" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Monto Gastos Operacionales CLP:</label>
                                    <asp:TextBox ID="txtGastosOperacionalesD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Tipo Contrato:</label>
                                    <asp:TextBox ID="txtTipoContrato" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Devolución Costo Fondo:</label>
                                    <asp:TextBox ID="txtDevolucionCostoFondo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnDatosAsesoria" runat="server">
                        <div class="row">
                            <!-- Grupo 1 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Fecha Estimada Cierre:</label>
                                    <asp:TextBox ID="txtFechaEstimada" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Producto:</label>
                                    <asp:TextBox ID="txtProductoA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>


                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Comision:</label>
                                    <asp:TextBox ID="txtComisionA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnDatosTransaccion" runat="server">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <h4>
                                    <asp:Label ID="lbTitulo" runat="server" Text="Label"></asp:Label>

                                </h4>
                                <h4>
                                    <asp:Label ID="Label2" runat="server" Text="Label"> Datos Facturación</asp:Label></h4>
                            </div>

                            <!-- Grupo 1 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Tipo Documento</label>
                                    <asp:TextBox ID="txtTipoDoc" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Monto CLP</label>
                                    <asp:TextBox ID="txtMontoCLP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Fecha Factura/ Nota Crédito:</label>
                                    <asp:TextBox ID="txt_Fecha" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>IVA</label>
                                    <asp:TextBox ID="txtIVA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Detalle</label>
                                    <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label>Nro Factura/ Nota Crédito:</label>
                                    <asp:TextBox ID="txt_Factura" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>

                            </div>

                        </div>

                    </asp:Panel>


                    <asp:Panel ID="pnFormulario" runat="server">
                        <div class="row ">
                            <div class="col-md-12 col-sm-12">
                                <h4>
                                    <asp:Label ID="Label1" runat="server" Text="Label"> Datos Conciliación</asp:Label></h4>
                            </div>
                            <!-- Grupo 1 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>Banco:</label>

                                    <asp:DropDownList ID="ddlbanco" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                    </asp:DropDownList>

                                </div>

                                <div class="form-group">
                                    <label>Fecha Pago:</label>
                                    <SharePoint:DateTimeControl ID="dctFechaPago" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                </div>

                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <label>TipoPago:</label>
                                    <asp:DropDownList ID="ddlTipoPago" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                        <asp:ListItem Value="1">Abono</asp:ListItem>
                                        <asp:ListItem Value="2">Cheque</asp:ListItem>
                                        <asp:ListItem Value="3">Otro</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group">
                                    <label>N° Documento Pago:</label>
                                    <asp:TextBox ID="txtDocumentoPago" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <!-- Grupo 1 -->
                                <div class="form-group">
                                    <label>Comentarios:</label>
                                    <asp:TextBox Style="text-align: left" ID="txt_Comentario" TextMode="multiline" Columns="20" Rows="5" runat="server" CssClass="form-control" MaxLength="1000" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </div>
    </div>



    <table style="width:100%">
        <tr>
            <td>
                <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
            </td>

            <td>
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />

                <asp:Button ID="btnCancel" runat="server" Text="Limpiar" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" OnClick="btnCancel_Click" />

                <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Pagar" OnClientClick="return Dialogo();" OnClick="btnGuardar_Click" />
            </td>
        </tr>
    </table>

</div>
