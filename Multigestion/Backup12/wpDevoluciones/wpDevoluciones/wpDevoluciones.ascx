<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDevoluciones.ascx.cs" Inherits="MultiOperacion.wpDevoluciones.wpDevoluciones.wpDevoluciones" %>

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
                        //else {
                        //    document.activeElement.value.keyCode = 13;
                        //} //Realiza el enter. 
                    }
                }
            }
        }
    }

    function Limpiar(cad) {

        var idPanel = cad + 'pnFormulario';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].value = "";
        };

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("textarea");
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

    function Habilitar(cad) {

        var idPanel = cad + 'pnFormulario';
        var idGuardar = cad + 'btnGuardar';
        var idLimpiar = cad + 'btnCancel';

        document.getElementById(idGuardar).style.display = 'block';
        document.getElementById(idLimpiar).style.display = 'block';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].disabled = false;
        };
        document.getElementById(idPanel).disabled = false;

        return false;
    }

    function Dialogo() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    }

    function vacio(string) {
        if (string != '')
            return false;
        return true;
    }

    function validarAno(string) {
        var ExpReg = /^\d{4}$/;
        return ExpReg.test(string);
    }

    function CerrarMensaje() {
        waitDialog.close();
        return false;
    }

    function solonumeros(e) {
        var unicode = e.charCode ? e.charCode : e.keyCode
        if (unicode != 8) {
            if (unicode < 48 || unicode > 57)
                return false
        }
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

    function FormatoMoneda(nStr) {
        var val = document.getElementById(nStr).value;
        val = val.replace(/[.]/gi, '');
        val += '';
        x = val.split(',');
        x1 = x[0];
        x2 = x.length > 1 ? ',' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + '.' + '$2');
        }
        document.getElementById(nStr).value = x1 + x2;
    }


    function calculoPorcentaje(porcentaje, monto, resultado, cad) {
        var porc = document.getElementById(porcentaje).value;
        var mont = document.getElementById(monto).value;
        if (porc > 0)
            document.getElementById(resultado).value = mont * (porc / 100);
    }

    function calculoPorcentajeDevolFijo(cad) {
        var porc = document.getElementById(cad + 'txtPorcDescuentoFijo').value;
        //document.getElementById(cad + 'ddlDescuentoFijo').options[document.getElementById(cad + 'ddlDescuentoFijo').selectedIndex].text

        var mont = document.getElementById(cad + 'txtDevolucionCLP').value;
        mont = mont.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        if (isNaN(mont) || mont == '' || mont == ',')
            mont = 0;

        porc = porc.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        if (porc > 0)
            document.getElementById(cad + 'txtDescuentoFijo').value = Math.floor(mont * (porc / 100));
        else
            document.getElementById(cad + 'txtDescuentoFijo').value = "0";

        document.getElementById(cad + 'txtDescuentoFijo').value = document.getElementById(cad + 'txtDescuentoFijo').value.replace(/[.]/gi, ',');
        FormatoMoneda(cad + 'txtDescuentoFijo');

        sumatoriaDevolucion(cad);
        calculoPorcentajeCostoFondo(cad);
        sumatoriaCalculoFondo(cad);
    }

    function calculoPorcentajeDevol(cad) {
        var porc = document.getElementById(cad + 'txtPorcDevolucion').value;
        //document.getElementById(cad + 'ddlPorcDevolucion').options[document.getElementById(cad + 'ddlPorcDevolucion').selectedIndex].text;
        document.getElementById(cad + 'txtPorcDevolucion2').value = porc;
        var mont = document.getElementById(cad + 'txtMontoComision').value;

        mont = mont.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        porc = porc.replace(/[.]/gi, '').replace(/[,]/gi, '.');

        if (porc > 0)
            document.getElementById(cad + 'txtDevolucionCLP').value = Math.floor(mont * (porc / 100));
        else
            document.getElementById(cad + 'txtDevolucionCLP').value = "0";

        document.getElementById(cad + 'txtDevolucionCLP').value = document.getElementById(cad + 'txtDevolucionCLP').value.replace(/[.]/gi, ',');
        FormatoMoneda(cad + 'txtDevolucionCLP');
        sumatoriaDevolucion(cad);
        calculoPorcentajeCostoFondo(cad);
        sumatoriaCalculoFondo(cad);
    }

    function calculoPorcentajeCostoFondo(cad) {
        var porc = document.getElementById(cad + 'txtPorcDevolucion').value;
        //document.getElementById(cad + 'ddlPorcDevolucion').options[document.getElementById(cad + 'ddlPorcDevolucion').selectedIndex].text;

        var mont = document.getElementById(cad + 'txtCostoFondo').value;
        mont = mont.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        porc = porc.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        if (porc > 0)
            document.getElementById(cad + 'txtDevolucionCostoFondo').value = Math.floor(mont * (porc / 100));
        else
            document.getElementById(cad + 'txtDevolucionCostoFondo').value = "0";
        document.getElementById(cad + 'txtDevolucionCostoFondo').value = document.getElementById(cad + 'txtDevolucionCostoFondo').value.replace(/[.]/gi, ',');
        FormatoMoneda(cad + 'txtDevolucionCostoFondo');
    }

    function sumatoriaDevolucion(cad) {

        var devol = document.getElementById(cad + 'txtDevolucionCLP').value;
        var desFijo = document.getElementById(cad + 'txtDescuentoFijo').value;
        devol = devol.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        desFijo = desFijo.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        if (isNaN(devol) || devol == "")
            devol = 0;
        if (isNaN(desFijo) || desFijo == "")
            desFijo = 0;

        document.getElementById(cad + 'txtDevolucion').value = parseFloat(devol) - parseFloat(desFijo);
        document.getElementById(cad + 'txtDevolucion').value = document.getElementById(cad + 'txtDevolucion').value.replace(/[.]/gi, ',');
        FormatoMoneda(cad + 'txtDevolucion');
        saldoCliente(cad);
    }

    function saldoCliente(cad) {

        var devol = document.getElementById(cad + 'txtDevolucion').value;
        var desFijo = document.getElementById(cad + 'txtSaldoGastos').value;
        devol = devol.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        desFijo = desFijo.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        if (isNaN(devol) || devol == "")
            devol = 0;
        if (isNaN(desFijo) || desFijo == "")
            desFijo = 0;

        document.getElementById(cad + 'txtSaldoCliente').value = parseFloat(devol) + parseFloat(desFijo);
        document.getElementById(cad + 'txtSaldoCliente').value = document.getElementById(cad + 'txtSaldoCliente').value.replace(/[.]/gi, ',');
        FormatoMoneda(cad + 'txtSaldoCliente');

    }

    function sumatoriaCalculoFondo(cad) {

        document.getElementById(cad + 'txtDevolucionCostoFondo').value = document.getElementById(cad + 'txtDevolucionCostoFondo').value;
    }

    function sumatoriaGastosOpe(cad) {
        var abon = document.getElementById(cad + 'txtAbonos').value.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        var gast = document.getElementById(cad + 'txtCargos').value.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        abon = abon.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        gast = gast.replace(/[.]/gi, '').replace(/[,]/gi, '.');
        if (isNaN(abon) || abon == '' || abon == ',')
            abon = 0;
        if (isNaN(gast) || gast == '' || gast == ',')
            gast = 0;
        document.getElementById(cad + 'txtSaldoGastos').value = parseFloat(abon) - parseFloat(gast);
        document.getElementById(cad + 'txtSaldoGastos').value = document.getElementById(cad + 'txtSaldoGastos').value.replace(/[.]/gi, ',');
        FormatoMoneda(cad + 'txtSaldoGastos');
        FormatoMoneda(cad + 'txtAbonos');
        FormatoMoneda(cad + 'txtCargos');
        saldoCliente(cad);
    }

    function validaForm() {
        var msg = "";
       
        if (document.getElementById("<%=ddlContratoTipo.ClientID%>").value == "-1") {
            msg += "-Seleccione contrato tipo.\n";
            document.getElementById("<%=ddlContratoTipo.ClientID%>").style.borderColor = 'red';
        }

        if (document.getElementById("<%=dtcFechaDevolucion.Controls[0].ClientID%>").value == "") {
            msg += "-Ingrese fecha de Devolucion.\n";
            document.getElementById("<%=dtcFechaDevolucion.Controls[0].ClientID%>").style.borderColor = 'red';
        }

        if (msg != "") {
            alert(msg);
            return false;
        } else {
            return true
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
            <h4 class="page-title">Devolución Comisión</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Operaciones</a>
                </li>
                <li class="active">Devolución Comisión
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


    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
        <h4>
            <asp:Label ID="lbSuccess" runat="server" Text="Exito."></asp:Label>
        </h4>
    </div>

    <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
        <h4>
            <asp:Label ID="lbWarning" runat="server" Text="warning"></asp:Label>
        </h4>
    </div>

    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
        <h4>
            <asp:Label ID="lbError" runat="server" Text="Error."></asp:Label>
        </h4>
    </div>


    <!-- filtros-->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <!--col 1/4 -->
                    <div class="col-md-6 col-sm-6">
                        <div class="form-group">
                            <label for="">Nº Certificado</label>
                            <asp:TextBox ID="txtnCertificado" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Monto Comisión</label>
                            <asp:TextBox ID="txtMontoComision" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Nº Crédito</label>
                            <asp:TextBox ID="txtNumCredito" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Cobertura Certificado</label>
                            <asp:TextBox ID="txtCoberturaC" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Fondo</label>
                            <asp:TextBox ID="txtFondo" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Última cuota pagada</label>
                            <asp:TextBox runat="server" ID="txtUltimaCta" MaxLength="16" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Fecha de Devolución</label>
                            <SharePoint:DateTimeControl ID="dtcFechaDevolucion" runat="server" Calendar="Gregorian" LocaleId="13322" ClientIDMode="Static"
                                CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" AutoPostBack="True" />
                        </div>
                    </div>

                    <!--col 2/4 -->
                    <div class="col-md-6 col-sm-6">
                        <div class="form-group">
                            <label for="">Fecha Curse</label>
                            <asp:TextBox ID="txtFechaEmision" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Fecha Vencimiento</label>
                            <asp:TextBox ID="txtFechaVencimiento" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Plazo Año/Meses</label>
                            <asp:TextBox ID="txtPlazoAM" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Acreedor</label>
                            <asp:TextBox ID="txtAcreedor" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Costo Fondo</label>
                            <asp:TextBox ID="txtCostoFondos" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="">Contrato Tipo</label>
                            <asp:DropDownList runat="server" ID="ddlContratoTipo" CssClass="form-control" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlContratoTipo_SelectedIndexChanged">
                                <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">CORFO</asp:ListItem>
                                <asp:ListItem Value="2">Tabla Anual</asp:ListItem>
                                <asp:ListItem Value="3">Tabla Especial</asp:ListItem>
                                <asp:ListItem Value="4">Proaval</asp:ListItem>
                                <asp:ListItem Value="5">Reverso</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div align="center" runat="server" id="divCalculo">
                    <div class="panel panel-primary" style="text-align: center; width: 40%">
                        <div class="panel-heading">Cálculos</div>
                        <div class="panel-body">

                            <div class="row clearfix  form-horizontal" role="form">

                                <div class="form-group" runat="server" id="dvAnoPrepago">
                                    <asp:Label ID="Label22" CssClass="control-label col-sm-5" runat="server" Text="Año Prepago" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtAnoPrepago" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvMesPrepago">
                                    <asp:Label ID="Label24" CssClass="control-label col-sm-5" runat="server" Text="Mes Prepago" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtMesPrepago" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>



                                <h4>Cálculo Devolución Comisión</h4>
                                <div class="form-group" runat="server" id="dvPorcDevolucion">
                                    <asp:Label ID="Label9" CssClass="control-label col-sm-5" runat="server" Text="Devolución %" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtPorcDevolucion" MaxLength="5" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvDevolucionCLPs">
                                    <asp:Label ID="Label1" CssClass="control-label col-sm-5" runat="server" Text="Devolución Pesos" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtDevolucionCLP" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="form-group" runat="server" id="dvPocDescuentoFijo">
                                    <asp:Label ID="Label4" CssClass="control-label col-sm-5" runat="server" Text="Descuento Fijo %" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtPorcDescuentoFijo" MaxLength="5" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvtxtDescuentoFijo">
                                    <asp:Label ID="Label7" CssClass="control-label col-sm-5" runat="server" Text="Descuento Fijo" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtDescuentoFijo" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvDevolucion">
                                    <asp:Label ID="Label8" CssClass="control-label col-sm-5" runat="server" Text="Devolución Final" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtDevolucion" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>


                                <h4>Cálculo Devolución Costo Fondo</h4>
                                <div class="form-group" runat="server" id="dvCostoFondo">
                                    <asp:Label ID="Label5" CssClass="control-label col-sm-5" runat="server" Text="Costo Fondo" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtCostoFondo" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvPocDevolucion2">
                                    <asp:Label ID="Label6" CssClass="control-label col-sm-5" runat="server" Text="Devolución %" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtPorcDevolucion2" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvDevolucionCostoFondo">
                                    <asp:Label ID="Label18" CssClass="control-label col-sm-5" runat="server" Text="Devolución Costo Fondo" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtDevolucionCostoFondo" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>


                                <h4>Saldo Cliente</h4>
                                <div class="form-group" runat="server" id="dvAbonos">
                                    <asp:Label ID="Label19" CssClass="control-label col-sm-5" runat="server" Text="Total Abonos" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtAbonos" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvCargos">
                                    <asp:Label ID="Label20" CssClass="control-label col-sm-5" runat="server" Text="Total Cargos" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtCargos" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="dvSaldoGastos">
                                    <asp:Label ID="Label21" CssClass="control-label col-sm-5" runat="server" Text="Saldo Cliente" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtSaldoGastos" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="Div1">
                                    <asp:Label ID="Label25" CssClass="control-label col-sm-5" runat="server" Text="Total Devolución Cliente" Font-Bold="True"></asp:Label>
                                    <div class="col-sm-6">
                                        <asp:TextBox runat="server" ID="txtSaldoCliente" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>

                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="if(validaForm()){return Dialogo();}else{return false;}" OnClick="btnCancelar_Click" />

                            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" class="btn btn-warning pull-right" OnClick="btnImprimir_Click" OnClientClick="if(validaForm()){return Dialogo();}else{return false;}" />
                            
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn  pull-right" OnClientClick="validar();" />

                            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnPrepagar" runat="server" Text="Prepagar" OnClientClick="if(validaForm()){return Dialogo();}else{return false;}" OnClick="btnPrepagar_Click" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>

</div>
          


