<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpContabilidad.ascx.cs" Inherits="MultiContabilidad.wpContabilidad.wpContabilidad.wpContabilidad" %>

<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>
<script type="text/javascript" src="../../_layouts/15/MultiContabilidad/wpDirectorio.js"></script>
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


    function ValidarFacturar() {
        try {
            var idPanel = $("#<%=pnFormulario.ClientID%>");
            var divPanel = document.getElementById("divPnFormulario");
            var validar = true;
            // recorremos todos los campos que tiene el formulario
            $(':input', idPanel).each(function () {
                var type = this.type;
                var tag = this.tagName.toLowerCase();
                var value = this.value;
                //var texto = this.text;

                if (tag == 'select') {
                    if (value <= 0) {
                        this.style.borderColor = 'red';
                        validar = false;
                    }
                    else
                        this.style.borderColor = '';

                    //var a = $("option:selected", this).attr("textContent");
                }
                else if (vacio(value)) {
                    this.style.borderColor = 'red';
                    validar = false;
                }
                else
                    this.style.borderColor = '';

                //limpiamos los valores de los campos…
                //if (type == 'text' || type == 'password' || tag == 'textarea')
                //    this.value = '';
                //    // excepto de los checkboxes y radios, le quitamos el checked
                //    // pero su valor no debe ser cambiado
                //else if (type == 'checkbox' || type == 'radio')
                //    this.checked = false;
                //    // los selects le ponesmos el indice a -
                //else if (tag == 'select')
                //    this.selectedIndex = -1;
            });

            if (!validar)
                return false;
            else
                return true;
        }
        catch (e) {
            alert(e);
        }
    }

    function ValidarConciliar() {
        try {
            var idPanel = $("#<%=pnFormularioConciliacion.ClientID%>");
             var validar = true;

             $(':input', idPanel).each(function () {
                 var type = this.type;
                 var tag = this.tagName.toLowerCase();
                 var value = this.value;

                 if (tag == 'select') {
                     if (value <= 0) {
                         this.style.borderColor = 'red';
                         validar = false;
                     }
                     else
                         this.style.borderColor = '';
                 }
                 else if (vacio(value)) {
                     this.style.borderColor = 'red';
                     validar = false;
                 }
                 else
                     this.style.borderColor = '';
             });

             if (!validar)
                 return false;
             else
                 return true;
         }
         catch (e) {
             alert(e);
         }
    }

    function SetControl(control) {
        control.style.borderColor = Controlvacio(control.value.trim()) ? 'red' : '';
        return !Controlvacio(control.value.trim());
    }

    function Controlvacio(string) {
        if (string != '' || string != 'Seleccione')
            return false;
        return true;
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
            <h4 class="page-title">Contabilidad</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Contabilidad</a>
                </li>
                <li class="active">Facturación
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


    <!-- Contenedor Tabs -->
    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li class="active">
                    <a href="#tab1" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="liGenerar" runat="server" OnClientClick="return Dialogo();" OnClick="lbGenerar_Click">Facturación y Conciliación</asp:LinkButton>
                        </span>
                    </a>
                </li>

               <%-- <li>
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="liPagar" runat="server" OnClientClick="return Dialogo();" OnClick="lbPagar_Click">Conciliación</asp:LinkButton>
                        </span>
                    </a>
                </li>--%>
            </ul>


            <div class="tab-content">
                <div class="tab-pane active" id="tab1">

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
                    <br />

                    <asp:Panel ID="pnDatosEmpresa" runat="server">
                        <div class="row">
                            <div class="form-horizontal" role="form">
                                <div class="col-md-12 col-sm-12">
                                    <asp:TextBox ID="idTransaccion" runat="server" Visible="false"></asp:TextBox>
                                    <h4>Datos Empresa</h4>
                                </div>
                                <!-- col 1/4 -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Empresa</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtRanzonSocial" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Actividad Económica:</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtActEconomica" runat="server" CssClass="form-control " ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Direccion</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Provincia</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtProvincia" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Telefono Fijo</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtTelefonof" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Rut</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtRut" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Comuna</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtComuna" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Region</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtRegion" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Email Corp</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnDatos" runat="server">
                        <div class="row">
                            <div class="form-horizontal" role="form">
                                <div class="col-md-12 col-sm-12">
                                    <h4>Datos Certificado</h4>
                                </div>
                                <!-- col 1/4 -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Nº Certificado</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCertificado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Producto</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtProducto" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Emision</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFechaEmision" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Plazo (meses)</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPlazo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Monto Comisiòn CLP</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtComisionCLp" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbcomision" runat="server" />
                                            Incluye comisión en el crédito
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="" class="col-sm-5 control-label">Monto Seguro Incendio</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtSeguro" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbseguro" runat="server" />
                                            Incluye seguro incendio en el crédito
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="" class="col-sm-5 control-label">Monto Seguro Desgravamen</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtSeguroDesgravamen" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbSeguroDesgravamen" runat="server" />
                                            Incluye seguro desgravamen
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Notario</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtNotario" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbNotario" runat="server" />
                                            Incluye notario
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Acreedor</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtAcreedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Monto Crédito</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Vencimiento</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFechaVencimiento" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="" class="col-sm-5 control-label">Fondo</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFondo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Servicios de Gestion Legal CLP</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtGastosOperacionales" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbgastosOpe" runat="server" />
                                            Incluye servicios de gestión legal en el crédito         
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Timbre Impuesto y Estampilla (Cargo Multiaval)</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtTimbreyEstMultiaval" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:Label ID="lbTimbreyEstAcreedor" runat="server" />
                                            Incluye Timbre Impuesto y Estampilla (Cargo Multiaval)
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Timbre Impuesto y Estampilla (Cargo Acreedor):</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtTimbreyEstAcreedor" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                            <asp:Label ID="lbTimbreyEstMultiaval" runat="server" />
                                            Incluye Timbre Impuesto y Estampilla (Cargo Acreedor)
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Comisión Fogape:</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtComisionFogape" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnDatosDevolucion" runat="server">
                        <div class="row">
                            <div class="form-horizontal" role="form">
                                <!-- col 1/4 -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Nº Certificado</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCertificadoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Producto</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtProductoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fondo</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFondoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Emisión</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFechaEmisionD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Monto Comisión CLP</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtComisionCLPD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Nro Factura Original</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtNroFacturaComisión" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Gastos Operacionales Pendientes</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtGatosOperacionalesP" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Devolución Final</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDevolucionFinal" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Monto Crédito</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtMontoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Acreedor</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtAcreedorD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Costo Fondo</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCostoFondoD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Factura</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFechaFacturaComision" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Monto Gastos Operacionales CLP</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtGastosOperacionalesD" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Tipo Contrato</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtTipoContrato" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Devolución Costo Fondo</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDevolucionCostoFondo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnDatosAsesoria" runat="server">
                        <div class="row">
                            <div class="form-horizontal" role="form">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="" class="col-sm-5 control-label">Fecha Estimada Cierre:</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtFechaEstimada" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="" class="col-sm-5 control-label">Producto:</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtProductoA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!-- Grupo 2 -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="" class="col-sm-5 control-label">Comisión:</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtComisionA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnFormulario" runat="server" ClientIDMode="Static">
                        <div class="row">
                            <div class="form-horizontal" role="form" id="divPnFormulario">
                                <div class="col-md-12 col-sm-12">
                                    <h4>
                                        <asp:Label ID="lbTitulo" runat="server" Text=""></asp:Label>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="glyphicon glyphicon-plus-sign"></asp:LinkButton>
                                    </h4>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Tipo Documento</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList runat="server" ID="sslTipoDoc" CssClass="form-control" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Monto CLP</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtMontoCLP" runat="server" CssClass="form-control" MaxLength="16" onKeyPress="return solonumeros(event)" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Fecha Factura</label>
                                        <div class="col-sm-7">
                                            <SharePoint:DateTimeControl ID="dctFechaFactura" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>

                                <!-- Grupo 2 -->
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">IVA</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList runat="server" ID="dllIVA" CssClass="form-control" ClientIDMode="Static">
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                <asp:ListItem Value="2">No</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Detalle</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDetalle" runat="server" CssClass="form-control" MaxLength="1000" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">N° Factura</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txt_Factura" runat="server" CssClass="form-control" MaxLength="10" onKeyPress="return solonumeros(event)" ClientIDMode="Static"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>


                    <div id="divConciliacion">

                        <%-- <asp:Panel ID="pnDatosTransaccion" runat="server" ClientIDMode="Static">
                            <div class="row">
                                <div class="form-horizontal" role="form">
                                    <div class="col-md-12 col-sm-12">
                                        <h4>
                                            <asp:Label ID="Label2" runat="server" Text="Label"> Datos Facturación</asp:Label></h4>
                                    </div>

                                    <!-- Grupo 1 -->
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Tipo Documento</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtTipoDoc" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Monto CLP</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Fecha Factura/ Nota Crédito</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txt_Fecha" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Grupo 2 -->
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">IVA</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtIVA" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Detalle</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Nro Factura/ Nota Crédito</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </asp:Panel>--%>


                        <asp:Panel ID="pnFormularioConciliacion" runat="server" ClientIDMode="Static">
                            <div class="row ">
                                <div class="form-horizontal" role="form">
                                    <div class="col-md-12 col-sm-12">
                                        <h4>
                                            <asp:Label ID="Label3" runat="server" Text="Label"> Datos Conciliación</asp:Label></h4>
                                    </div>

                                    <!-- Grupo 1 -->
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Banco:</label>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="ddlbanco" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">Fecha Pago</label>
                                            <div class="col-sm-7">
                                                <SharePoint:DateTimeControl ID="dctFechaPago" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                            </div>
                                        </div>

                                    </div>

                                    <!-- Grupo 2 -->
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">TipoPago</label>
                                            <div class="col-sm-7">
                                                <asp:DropDownList ID="ddlTipoPago" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                                    <asp:ListItem Value="1">Abono</asp:ListItem>
                                                    <asp:ListItem Value="2">Cheque</asp:ListItem>
                                                    <asp:ListItem Value="3">Otro</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-sm-5 control-label">N° Documento Pago</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtDocumentoPago" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <!-- Grupo 1 -->
                                    <div class="form-group">
                                        <label class="col-sm-5 control-label">Comentarios</label>
                                        <%--<div class="col-sm-7">--%>
                                        <asp:TextBox Style="text-align: left" ID="txt_Comentario" TextMode="multiline" Columns="20" Rows="5" runat="server" CssClass="form-control" MaxLength="1000" />
                                        <%--</div>--%>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <br />
                    <br />

                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                            </td>

                            <td>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />

                                <asp:Button ID="btnCancel" runat="server" Text="Limpiar" Visible="false" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" OnClick="btnCancel_Click" />

                                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" class="btn btn-warning pull-right" OnClick="btnImprimir_Click" />

                                <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Facturar" OnClientClick="return ValidarFacturar();" OnClick="btnGuardar_Click" />

                                <asp:Button class="btn btn-primary btn-success pull-right" ID="BtnConciliar" runat="server" Text="Conciliar" OnClientClick="return ValidarConciliar();" OnClick="BtnConciliar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

            </div>
        </div>
    </div>


</div>

