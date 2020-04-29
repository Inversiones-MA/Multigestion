<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpDatosOperacion.ascx.cs" Inherits="MultiComercial.wpDatosOperacion.wpDatosOperacion.wpDatosOperacion" %>

<style type="text/css">
    .btn {
        font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important;
    }

    .shortCmb {
        width: 70px;
    }
</style>

<script src="../../_layouts/15/MultiComercial/wpDatosOperacion.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.8.3.min.js"></script>

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
                    }
                }
            }
        }
    }

    function ValidarTipoOperacion() {
        var producto = $("#<%=ddlProducto.ClientID%>");
        var productoSelect = producto.find("option:selected").text();
        var numCuotas = $("#<%=txtNroCuotas.ClientID%>").val();

        if ((productoSelect !== null && productoSelect !== undefined) && (numCuotas !== null && numCuotas !== undefined)) {
            if (productoSelect === 'Certificado Fianza Comercial') {
                if (numCuotas > 1) {
                    $("#<%=ddlTipoOperacion.ClientID%>").val('1');
                    //$("#<%=ddlTipoOperacion.ClientID%>").attr("readonly", true);
                }
                else {
                    $("#<%=ddlTipoOperacion.ClientID%>").val('2');
                    //$("#<%=ddlTipoOperacion.ClientID%>").attr("readonly", true);
                }
            }
        }
        //$("#<%=ddlTipoOperacion.ClientID%>").attr("disabled", "disabled");
        $("#<%=ddlTipoOperacion.ClientID%>").attr("readonly", true);
    }

    function ValidarHorizonte() {
        var producto = $("#<%=ddlProducto.ClientID%>");
        var tipoCredito = $("#<%=ddlTipoCredito.ClientID%>");
        var productoSelect = producto.find("option:selected").text();
        var credito = tipoCredito.find("option:selected").text();
        var NumCuotas = $("#<%=txtPlazo.ClientID%>");

        if ((productoSelect !== null && productoSelect !== undefined)) {
            if (productoSelect === 'Certificado Fianza Comercial') {
                if (credito === 'Cuotas iguales con Cuotón') {
                    $("#<%=txtHorizonte.ClientID%>").prop("disabled", false);
                    $("#<%=txtMontoCuoton.ClientID%>").prop("disabled", false);
                }
                else {
                    $("#<%=txtHorizonte.ClientID%>").val('');
                    //$("#<%=txtHorizonte.ClientID%>").attr('value', NumCuotas.val());
                    $("#<%=txtHorizonte.ClientID%>").val(NumCuotas.val());
                    $("#<%=txtMontoCuoton.ClientID%>").val('0');
                    $("#<%=txtHorizonte.ClientID%>").prop("readonly", true);
                    //$("#<%=txtHorizonte.ClientID%>").prop("disabled", true);
                    $("#<%=txtMontoCuoton.ClientID%>").prop("disabled", true);
                }

            }
            else {
                $("#<%=txtHorizonte.ClientID%>").val(NumCuotas.val());
            }
        }
    }

    function ValidarFogape() {
        var tieneFogape = $("#<%=ddlFogapeVigente.ClientID%>");
        var estadoCert = $("#<%=ddlEdoCertificado.ClientID%>");
        //var codigoSaf = $("#<%=txtCodigoSafio.ClientID%>").val();

        var fogapeSelect = tieneFogape.find("option:selected").text();
        var estadoCertSelect = estadoCert.find("option:selected").text();
        //var codigoSelect = codigoSaf.find("option:selected").text();

        if ((fogapeSelect !== null && fogapeSelect !== undefined)) {
            if (fogapeSelect === 'Si') {
                $("#<%=txtCodigoSafio.ClientID%>").prop("disabled", false);
                if (estadoCertSelect === 'Ejecutado') {
                    //$("#<%=dtcFechaRecuperacion.ClientID%>").prop("disabled", false);
                    $("#<%=TxtMontoRecuperoFogape.ClientID%>").prop("disabled", false);
                    $("#<%=ddlMotivoGFV.ClientID%>").prop("disabled", false);
                }
                else {
                    //$("#<%=dtcFechaRecuperacion.ClientID%>").prop("disabled", true);
                    $("#<%=TxtMontoRecuperoFogape.ClientID%>").prop("disabled", true);
                    $("#<%=ddlMotivoGFV.ClientID%>").prop("disabled", true);
                }
            }
            else {
                $("#<%=txtCodigoSafio.ClientID%>").prop("disabled", true);
                //$("#<%=dtcFechaRecuperacion.ClientID%>").prop("disabled", true);
                $("#<%=TxtMontoRecuperoFogape.ClientID%>").prop("disabled", true);
                $("#<%=ddlMotivoGFV.ClientID%>").prop("disabled", true);
                $("#<%=txtCoberturaFogape.ClientID%>").val('0');
            }
            CalcularComisionFogape();
        }
    }



    function CalcularComisionFogape() {

        var SetapaFo = $("#<%=HdfSubEtapa.ClientID%>").val();
        var etapaFo = $("#<%=HdfEtapa.ClientID%>").val();
        var are = $("#<%=HdfArea.ClientID%>").val();
        var licitacion = $("#<%=HdfCotizacion.ClientID%>").val();

        if (SetapaFo == 'Por Emitir' || are == 'Comercial' || are == 'Riesgo' || are == 'Fiscalia') {
            var tieneFogape = $("#<%=ddlFogapeVigente.ClientID%>");
            var fogapeSelect = tieneFogape.find("option:selected").text();
            var tasaComisonAnual = $("#<%=HdfTasaComisionAnual.ClientID%>").val() / 100;
            var plazo = $("#<%=txtPlazo.ClientID%>").val();
            var montoOperacionCLP = $("#<%=txtMontoOperacion.ClientID%>").val().replace(/\./g, '').replace(/\,/g, '.');
            var coberturaFogape = $("#<%=txtCoberturaFogape.ClientID%>").val().replace(/\./g, '').replace(/\,/g, '.');
            var TipoAmortizacion = $("#<%=ddlTipoAmortizacion.ClientID%>").val();
            var PeriodoGracia = $("#<%=txtPeriodoGracia.ClientID%>").val();
            var ddlFondo = $("#<%=ddlFondo.ClientID%>").val();

            if (ddlFondo != null && ddlFondo != "0") {
                if ((fogapeSelect !== null && fogapeSelect !== undefined)) {
                    if (fogapeSelect === 'Si' && coberturaFogape > 0) {
                        if (PeriodoGracia == '' || PeriodoGracia == null)
                            PeriodoGracia = 0;

                        //if (TipoAmortizacion == 5) //anual           
                        //    plazo = plazo / 12 - PeriodoGracia;

                        //if (TipoAmortizacion == 4) //semestral           
                        //    plazo = plazo / 6 - PeriodoGracia;

                        //if (TipoAmortizacion == 3) //trimestral           
                        //    plazo = plazo / 3 - PeriodoGracia;

                        //if (TipoAmortizacion == 2) //mensual           
                        //    plazo = plazo - PeriodoGracia;

                        if (licitacion == null) {
                            var montoComisionAnualFogape = tasaComisonAnual * montoOperacionCLP * (coberturaFogape / 100) * (plazo / 12);
                            if (!isNaN(montoComisionAnualFogape) && montoComisionAnualFogape != '')
                                $("#<%=txtComisionFogape.ClientID%>").val(formatNumber.new(parseFloat(montoComisionAnualFogape).toFixed(0)));
                            else
                                $("#<%=txtComisionFogape.ClientID%>").val('0');

                            $("#<%=txtLicitacionFogape.ClientID%>").val($("#<%=HdfLicitacionFogape.ClientID%>").val());

                            if (tasaComisonAnual == 0 && tasaComisonAnual == null)
                                alert('Es necesario actualizar la lista de licitaciones fogape');
                        }
                    }
                    else {
                        $("#<%=txtComisionFogape.ClientID%>").val('0');
                        $("#<%=txtLicitacionFogape.ClientID%>").val('0');
                    }
                }
            }
        }
        ValidarHorizonte();
    }

    function asignacionHdnMontoOPCLP(cad) {
        //$("#<%=txtComisionFogape.ClientID%>").prop("disabled", false);
        $("#<%=txtMontoOperacionCLP.ClientID%>").prop("disabled", false)
        $("#<%=txtMontoOperacionCLP.ClientID%>").prop('readonly', false);
        var bb = $("#<%=txtMontoOperacionCLP.ClientID%>").val();
        $("#<%=hdnMontoOperacionCLP.ClientID%>").val(bb);
        var cc = $("#<%=hdnMontoOperacionCLP.ClientID%>").val();
        $("#<%=txtMontoOperacionCLP.ClientID%>").val(bb);
        return true;
    }

    var formatNumber = {
        separador: ".", // separador para los miles
        sepDecimal: ',', // separador para los decimales
        formatear: function (num) {
            num += '';
            var splitStr = num.split('.');
            var splitLeft = splitStr[0];
            var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
            var regx = /(\d+)(\d{3})/;
            while (regx.test(splitLeft)) {
                splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
            }
            return this.simbol + splitLeft + splitRight;
        },
        new: function (num, simbol) {
            this.simbol = simbol || '';
            return this.formatear(num);
        }
    }

    function SetTab(panel) {

        var hdn = panel;
        if (hdn != null) {

            if (hdn.value != "") {
                var b = document.getElementById("myTab");

                switch (hdn) {
                    case 'panelComercial':
                        document.getElementById('<%=li_comercial.ClientID%>').className = "active";
                        document.getElementById('<%=li_riesgo.ClientID%>').className = "";

                        if (document.getElementById("HdfEtapa") == null || document.getElementById("HdfEtapa").value == '') {
                            document.getElementById('<%=li_operacion.ClientID%>').className = "";
                            document.getElementById('<%=li_pagare.ClientID%>').className = "";
                        }

                        //$('<%=li_comercial.ClientID%>').addClass('active');
                        $('#panel-riesgo').removeClass('active');
                        $('#panel-comercial').addClass('active');
                        $('#panel-operacion').removeClass('active');
                        $('#panel-pagare').removeClass('active');
                        document.getElementById("hdftabb").value = "panelComercial"
                        break;

                    case 'panelRiesgo':
                        document.getElementById('<%=li_comercial.ClientID%>').className = "";
                        document.getElementById('<%=li_riesgo.ClientID%>').className = "active";

                        if (document.getElementById("HdfEtapa").value == null || document.getElementById("HdfEtapa").value == '') {
                            document.getElementById('<%=li_operacion.ClientID%>').className = "";
                            document.getElementById('<%=li_pagare.ClientID%>').className = "";
                        }
                        //var nombre = $('#<%=li_riesgo.ClientID%>').attr('id');
                        //if (nombre.toLowerCase().indexOf("li_riesgo") >= 0) { }
                        $('#panel-riesgo').addClass('active');
                        $('#panel-comercial').removeClass('active');
                        $('#panel-operacion').removeClass('active');
                        $('#panel-pagare').removeClass('active');
                        document.getElementById("hdftabb").value = "panelRiesgo"
                        break;

                    case 'panelOperacion':
                        document.getElementById('<%=li_comercial.ClientID%>').className = "";
                        document.getElementById('<%=li_riesgo.ClientID%>').className = "";
                        document.getElementById('<%=li_operacion.ClientID%>').className = "active";
                        document.getElementById('<%=li_pagare.ClientID%>').className = "";
                        $('#panel-riesgo').removeClass('active');
                        $('#panel-comercial').removeClass('active');
                        $('#panel-operacion').addClass('active');
                        $('#panel-pagare').removeClass('active');
                        document.getElementById("hdftabb").value = "panelOperacion"
                        break;

                    case 'panelPagare':
                        document.getElementById('<%=li_comercial.ClientID%>').className = "";
                        document.getElementById('<%=li_riesgo.ClientID%>').className = "";
                        document.getElementById('<%=li_operacion.ClientID%>').className = "";
                        document.getElementById('<%=li_pagare.ClientID%>').className = "active";
                        $('#panel-riesgo').removeClass('active');
                        $('#panel-comercial').removeClass('active');
                        $('#panel-operacion').removeClass('active');
                        $('#panel-pagare').addClass('active');
                        document.getElementById("hdftabb").value = "panelPagare"
                        break;
                }
            }
        }
    }

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");

        var hdn = document.getElementById("hdftabb")
        if (hdn.value != null && hdn.value != "") {
            var panel = hdn.value;
        }
        else {
            var panel = 'panelComercial';
        }
        SetTab(panel);
        ddl_changed(document.getElementById('<%=ddlGarantiaFogapeVigente.ClientID %>'));
        ValidarHorizonte();
        CalcularComisionFogape();
        ValidarTipoOperacion();
    });

    function ddl_changed(ddl) {
        if (ddl !== null) {
            var valueFogape = document.getElementById('<%=ddlGarantiaFogapeVigente.ClientID %>').value;
            if (valueFogape === 'True') {
                document.getElementById('DivddlMotivoGFV').style.visibility = "hidden";
            }
            else {
                document.getElementById('DivddlMotivoGFV').style.visibility = "visible";
            }
        }
    }

    function OnIframeLoadFinish() {
        if (!!window.chrome) {
            ULSvmd:
            ;
            var picker;
            if (typeof this.Picker != 'undefined')
                picker = this.Picker;
            if (picker != null && typeof picker.readyState != 'undefined' && picker.readyState != null && picker.readyState == "complete") {
                document.body.scrollLeft = g_scrollLeft;
                document.body.scrollTop = g_scrollTop;
                g_scrollTop = document.getElementById('s4-workspace').scrollTop;
                picker.style.display = "block";
                if (typeof document.frames != 'undefined' && Boolean(document.frames)) {  /* "document.frames" doesn't work on chrome use "window.frames" instead*/
                    var frame = document.frames[picker.id];
                    if (frame != null && typeof frame.focus == 'function')
                        frame.focus();
                }
                else {
                    picker.focus();
                }
            }
            setTimeout(function () {
                document.getElementById('s4-workspace').scrollTop = g_scrollTop;
            }, 1);
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
            <h4 class="page-title">Comercial</h4>
            <ol class="breadcrumb">
                <li>
                    <a>Datos Operaci&oacute;n</a>
                </li>
                <li class="active">Edici&oacute;n de Operaci&oacute;n</li>
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
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblIdOperacion" runat="server" Text="" Visible="false"></asp:Label>
                </p>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <div class="tabbable" id="tabs-empRelacionadas">
                <%-- TAB --%>
                <ul class="nav nav-tabs navtab-bg nav-justified">
                    <li>
                        <asp:LinkButton ID="lbGarantias" runat="server" OnClick="lbGarantias_Click">Garantías</asp:LinkButton>
                    </li>
                    <li class="active">
                        <asp:LinkButton ID="lbRegistroOperacion" runat="server" OnClick="lbRegistroOperacion_Click">Registro Operación</asp:LinkButton>
                    </li>
                </ul>

                <div class="tab-content">
                    <div class="tab-pane active" id="panel-tab3">
                        <div class="row responsive">
                            <div class="col-md-12">
                                <div class="card-box">
                                    <%-- Cuadros de Alerta --%>
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
                                            <ul class="nav nav-tabs navtab-bg nav-justified" id="myTab">
                                                <li id="li_comercial" runat="server">
                                                    <a href="#panel-comercial" data-toggle="tab" onclick="SetTab('panelComercial')">
                                                        <asp:Label ID="lbLiAct" runat="server" Text="Antecedentes Comercial"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li_riesgo" runat="server">
                                                    <a href="#panel-riesgo" data-toggle="tab" onclick="SetTab('panelRiesgo')">
                                                        <asp:Label ID="lbLi1" runat="server" Text="Antecedentes Riesgo"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li_operacion" runat="server">
                                                    <a href="#panel-operacion" data-toggle="tab" onclick="SetTab('panelOperacion')">
                                                        <asp:Label ID="Label1" runat="server" Text="Antecedentes Operaci&oacute;n"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li_pagare" runat="server">
                                                    <a href="#panel-pagare" data-toggle="tab" onclick="SetTab('panelPagare')">
                                                        <asp:Label ID="Label2" runat="server" Text="Antecedentes Pagar&eacute;"></asp:Label>
                                                    </a>
                                                </li>
                                            </ul>

                                            <div class="tab-content">

                                                <div id="panel-comercial" class="tab-pane">
                                                    <!-- 1st FORMULARIO: ANTECEDENTES COMERCIAL -->
                                                    <asp:Panel runat="server" ID="pnComercial">
                                                        <br />
                                                        <br />
                                                        <div class="row clearfix margenBottom">
                                                            <div class="form-horizontal" role="form">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbProducto" CssClass="col-sm-5 control-label" runat="server" Text="Producto" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlProducto" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged" ClientIDMode="Static">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbMontoOperacion" CssClass="col-sm-5 control-label" runat="server" Text="Monto Operación" Font-Bold="True" />
                                                                        <div class="col-sm-4">
                                                                            <asp:TextBox runat="server" ID="txtMontoOperacion" MaxLength="12" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-sm-3">
                                                                            <asp:DropDownList runat="server" ID="ddlTipoMoneda" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label4" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Crédito" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlTipoCredito" CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbPlazo" CssClass="col-sm-5 control-label" runat="server" Text="Plazo(Meses)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtPlazo" MaxLength="3" CssClass="form-control" onKeyPress="return solonumeros(event)" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbNroCuotas" CssClass="col-sm-5 control-label" runat="server" Text="N° de Cuotas" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtNroCuotas" MaxLength="3" CssClass="form-control" onKeyPress="return solonumeros(event)" ClientIDMode="Static" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbTipoAmortizacion" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Amortización" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlTipoAmortizacion" CssClass="form-control" />
                                                                            <asp:HiddenField ID="hfObligaAmor" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbTasaInteres" CssClass="col-sm-5 control-label" runat="server" Text="Tasa de Interes (%)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtTasaInteres" MaxLength="7" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label3" CssClass="col-sm-5 control-label" runat="server" Text="Monto comisión (CLP)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtMontoComision" MaxLength="16" CssClass="form-control" OnTextChanged="txtMontoComision_TextChanged" AutoPostBack="True" />
                                                                            Incluida?
                                                                        <asp:DropDownList ID="ddlComisionIncluida" runat="server" CssClass="form-control shortCmb">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbAcreedor" CssClass="col-sm-5 control-label" runat="server" Text="Acreedor" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlAcreedor" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAcreedor_SelectedIndexChanged"></asp:DropDownList>
                                                                            <br />
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label5" CssClass="col-sm-5 control-label" runat="server" Text="Monto Seguro Desgravamen (CLP)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtMontoSeguroDesgravamen" MaxLength="16" CssClass="form-control" />
                                                                            Incluido?
                                                                        <asp:DropDownList ID="ddlSeguroDesgravamenIncluido" runat="server" CssClass="form-control shortCmb">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbTimbreEstampillaAcreedor" CssClass="col-sm-5 control-label" runat="server" Text="Impuesto Timbre y Estampilla (Cargo Acreedor)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtTimbreEstampillaAcreedor" runat="server" MaxLength="16" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            Incluido?
                                                                            <asp:DropDownList ID="ddlTimbreEstampillaAcreedorIncluido" runat="server" CssClass="form-control shortCmb">
                                                                                <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                                <asp:ListItem Value="True">Si</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label10" CssClass="col-sm-5 control-label" runat="server" Text="Notario" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtNotario" runat="server" MaxLength="16" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            Incluido?
                                                                        <asp:DropDownList ID="ddlNotarioIncluido" runat="server" CssClass="form-control shortCmb">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFogape" CssClass="col-sm-5 control-label" runat="server" Text="Cobertura FOGAPE" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtCoberturaFogape" runat="server" MaxLength="5" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            Requiere FOGAPE?
                                                                        <asp:DropDownList ID="ddlFogapeVigente" runat="server" CssClass="form-control shortCmb">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label14" CssClass="col-sm-5 control-label" runat="server" Text="Comisión Fogape" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtComisionFogape" runat="server" MaxLength="12" CssClass="form-control" onKeyPress="return solonumeros(event)" ClientIDMode="Static"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFinalidad" CssClass="col-sm-5 control-label" runat="server" Text="Finalidad" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlFinalidad" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label6" CssClass="col-sm-5 control-label" runat="server" Text="Monto Operación (CLP)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtMontoOperacionCLP" ReadOnly="true" MaxLength="20" CssClass="form-control" onKeyPress="return solonumeros(event)" ClientIDMode="Static" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbTipoCuota" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Cuota" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlTipoCuota" CssClass="form-control" ClientIDMode="Static" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbHorizonte" CssClass="col-sm-5 control-label" runat="server" Text="Horizonte" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtHorizonte" MaxLength="5" CssClass="form-control" ClientIDMode="Static" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbPeriodoGracia" CssClass="col-sm-5 control-label" runat="server" Text="Periodo Gracia (Meses)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtPeriodoGracia" MaxLength="4" CssClass="form-control" onKeyPress="return solonumeros(event)" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="txtPCobertura" CssClass="col-sm-5 control-label" runat="server" Text="Cobertura (%)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlPorceCobertura" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="0" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="10" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="20" Value="3"></asp:ListItem>
                                                                                <asp:ListItem Text="30" Value="4"></asp:ListItem>
                                                                                <asp:ListItem Text="40" Value="5"></asp:ListItem>
                                                                                <asp:ListItem Text="50" Value="6"></asp:ListItem>
                                                                                <asp:ListItem Text="60" Value="7"></asp:ListItem>
                                                                                <asp:ListItem Text="70" Value="8"></asp:ListItem>
                                                                                <asp:ListItem Text="80" Value="9"></asp:ListItem>
                                                                                <asp:ListItem Text="90" Value="10"></asp:ListItem>
                                                                                <asp:ListItem Text="100" Value="11"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbPeriocidad" CssClass="col-sm-5 control-label" runat="server" Text="Periocidad tasa interés" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlPeriocidad" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbPorcComision" CssClass="col-sm-5 control-label" runat="server" Text="Comisión (%)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtPorcComision" MaxLength="7" CssClass="form-control" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbCanal" CssClass="col-sm-5 control-label" runat="server" Text="Canal" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlCanal" CssClass="form-control" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbContactoAcreedor" CssClass="col-sm-5 control-label" runat="server" Text="Contacto Acreedor" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlContacAcreedor" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbMontoSeguroIncendio" CssClass="col-sm-5 control-label" runat="server" Text="Monto Seguro Incendio (CLP)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtMontoSeguroIncendio" MaxLength="16" CssClass="form-control" />
                                                                            Incluido?
                                                                            <asp:DropDownList ID="ddlSeguroIncendioIncluido" runat="server" CssClass="form-control shortCmb">
                                                                                <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                                <asp:ListItem Value="True">Si</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbTimbreYEstampillaMultiaval" CssClass="col-sm-5 control-label" runat="server" Text="Impuesto Timbre y Estampilla (Cargo Multiaval)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtTimbreEstampillaMultiaval" runat="server" MaxLength="16" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            Incluido?
                                                                        <asp:DropDownList ID="ddlTimbreEstampillaMultiavalIncluido" runat="server" CssClass="form-control shortCmb">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbServGestionLegal" CssClass="col-sm-5 control-label" runat="server" Text="Servicio de Gestión Legal" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtServicioGestionLegal" runat="server" MaxLength="16" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            Incluido?
                                                                        <asp:DropDownList ID="ddlServGestionLegalIncluido" runat="server" CssClass="form-control shortCmb">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbLicitacion" CssClass="col-sm-5 control-label" runat="server" Text="Licitación FOGAPE" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtLicitacionFogape" MaxLength="10" ReadOnly="true" CssClass="form-control" onKeyPress="return solonumeros(event)" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <br />
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFechacierre" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Estimada Cierre" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <SharePoint:DateTimeControl ID="dtcFechaCierre" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--  TEXAREAS -->
                                                                <div class="col-md-10">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbGlosa" CssClass="col-sm-3 control-label" Style="text-align: right" runat="server" Text="Glosa" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <textarea rows="5" cols="20" runat="server" id="txtGlosaComercial" class="form-control"></textarea>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label8" CssClass="col-sm-3 control-label" Style="text-align: right" runat="server" Text="Instrucción Curse" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <textarea rows="5" cols="20" runat="server" id="txtInstruccionCurse" class="form-control"></textarea>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label9" CssClass="col-sm-3 control-label" Style="text-align: right" runat="server" Text="Observación" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <textarea rows="5" cols="20" runat="server" id="txtObservacion" class="form-control"></textarea>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <!--  FIN TEXAREAS -->
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFechaEmi" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Emisión" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <SharePoint:DateTimeControl ID="dtcFechaEmi" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" ClientIDMode="Static" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label11" CssClass="col-sm-5 control-label" runat="server" Text="Fecha 1er Vencimiento" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <SharePoint:DateTimeControl ID="dtcFecha1erVenc" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label12" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Vencimiento" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <SharePoint:DateTimeControl ID="dtcFechaVenc" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" ClientIDMode="Static" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <!-- FIN ANTECEDENTES COMERCIAL -->
                                                </div>

                                                <div class="tab-pane" id="panel-riesgo">
                                                    <!-- 2nd FORMULARIO: ANTECEDENTES RIESGO -->
                                                    <asp:Panel runat="server" ID="pnRiesgo">
                                                        <br />
                                                        <div class="row clearfix margenBottom">
                                                            <div class="form-horizontal" role="form">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFondo" CssClass="col-sm-5 control-label" runat="server" Text="Fondo" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlFondo" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlFondo_SelectedIndexChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbCostoFondo" CssClass="col-sm-5 control-label" runat="server" Text="Costo Fondos" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtCostoFondo" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbpaf" CssClass="col-sm-5 control-label" runat="server" Text="N° PAF" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtNroPaf" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-10">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label13" CssClass="col-sm-3 control-label" Style="text-align: right" runat="server" Text="Destino Solicitud" Font-Bold="True"></asp:Label>

                                                                        <div class="col-sm-7">
                                                                            <textarea rows="5" cols="20" runat="server" id="txtDestinoSolicitud" class="form-control"></textarea>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <!-- FIN ANTECEDENTES RIESGO -->
                                                </div>

                                                <div class="tab-pane" id="panel-operacion">
                                                    <!-- 3rd FORMULARIO: ANTECEDENTES OPERACION -->
                                                    <asp:Panel runat="server" ID="pnOperaciones">
                                                        <br />
                                                        <br />
                                                        <div class="row clearfix margenBottom">
                                                            <div class="form-horizontal" role="form">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbNroCertificado" CssClass="col-sm-5 control-label" runat="server" Text="N° de Certificado" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtNroCertificado" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbNroCredito" CssClass="col-sm-5 control-label" runat="server" Text="N° Crédito" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtNroCredito" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbnroDocument" CssClass="col-sm-5 control-label" runat="server" Text="N° Documento" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtNroDocumento" CssClass="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbValorMonedaEmision" CssClass="col-sm-5 control-label" runat="server" Text="Valor Moneda Fecha de Emision" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtValorMonedaEmision" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbMontoCuota" CssClass="col-sm-5 control-label" runat="server" Text="Monto Cuota" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtMontoCuota" MaxLength="13" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label18" CssClass="col-sm-5 control-label" runat="server" Text="Monto Cuotón" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtMontoCuoton" MaxLength="15" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbCodigoSafio" CssClass="col-sm-5 control-label" runat="server" Text="Código Safio" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtCodigoSafio" runat="server" MaxLength="16" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                            Formalizó SAFIO?
									                                        <asp:DropDownList ID="ddlSafioIncluido" CssClass="form-control shortCmb" runat="server">
                                                                                <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                                <asp:ListItem Value="True">Si</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-sm-5 control-label">Monto Recupero Fogape</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="TxtMontoRecuperoFogape" runat="server" MaxLength="18" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label id="LblReLegal" class="col-sm-5 control-label">1° Representante Legal Multiaval</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlReLegal" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label id="Label15" class="col-sm-5 control-label">2° Representante Legal Multiaval</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlReLegal2" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbNroCFAnterior" CssClass="col-sm-5 control-label" runat="server" Text="N° CF Anterior" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:ListBox runat="server" ID="lboxCFAnterior" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label17" CssClass="col-sm-5 control-label" runat="server" Text="Saldo Capital" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtSaldoCapital" runat="server" MaxLength="16" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbEstadoCert" CssClass="col-sm-5 control-label" runat="server" Text="Estado Certificado" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlEdoCertificado" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbEstadoCredito" CssClass="col-sm-5 control-label" runat="server" Text="Estado Crédito" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlEdoCredito" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label19" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Operación" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlTipoOperacion" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbTipoObligacionAfiazada" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Obliación Afianzada" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlTipoObligaAfianz" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label7" CssClass="col-sm-5 control-label" runat="server" Text="Periocidad de Pago CORFO" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlPeriocidadPago" CssClass="form-control">
                                                                                <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                                                                <asp:ListItem Value="1">Semanal</asp:ListItem>
                                                                                <asp:ListItem Value="2">Quincenal</asp:ListItem>
                                                                                <asp:ListItem Value="3">Mensual</asp:ListItem>
                                                                                <asp:ListItem Value="4">Bi-Mensual</asp:ListItem>
                                                                                <asp:ListItem Value="5">Trimestral</asp:ListItem>
                                                                                <asp:ListItem Value="6">Semestral</asp:ListItem>
                                                                                <asp:ListItem Value="7">Cuatrimestral </asp:ListItem>
                                                                                <asp:ListItem Value="8">Anual</asp:ListItem>
                                                                                <asp:ListItem Value="9">Periocidad No Estructurada</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFechaRecuperacion" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Recuperación" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <SharePoint:DateTimeControl ID="dtcFechaRecuperacion" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label20" CssClass="col-sm-5 control-label" runat="server" Text="Garantía Fogape" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            Vigente?
									                                    <asp:DropDownList ID="ddlGarantiaFogapeVigente" CssClass="form-control shortCmb" runat="server" ClientIDMode="Static" onchange="ddl_changed(this)">
                                                                            <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                            <asp:ListItem Value="True">Si</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                            <br />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label class="col-sm-5 control-label"></label>
                                                                        <div class="col-sm-7">
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group" id="DivddlMotivoGFV">
                                                                        <asp:Label ID="lblMotivoFogape" CssClass="col-sm-5 control-label" runat="server" Text="Motivo Garantia Fogape" Font-Bold="True" ClientIDMode="Static"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlMotivoGFV" CssClass="form-control" runat="server" ClientIDMode="Static">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label id="lblReFondo" class="col-sm-5 control-label">1° Representante Fondo</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlReFondo" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label id="Label16" class="col-sm-5 control-label">2° Representante Fondo</label>
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlReFondo2" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbNroCFSucesor" CssClass="col-sm-5 control-label" runat="server" Text="N° CF Sucesor" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:ListBox runat="server" ID="lboxCFSucesor" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <!-- FIN ANTECEDENTES OPERACION -->
                                                </div>

                                                <div class="tab-pane" id="panel-pagare">
                                                    <!-- 4th FORMULARIO: ANTECEDENTES PAGARE -->
                                                    <asp:Panel runat="server" ID="pnPagare">
                                                        <br />
                                                        <div class="row clearfix margenBottom">
                                                            <div class="form-horizontal" role="form">
                                                                <div class="row clearfix">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label21" CssClass="col-sm-5 control-label" runat="server" Text="N° Pagaré" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txtNroPagare" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label22" CssClass="col-sm-5 control-label" runat="server" Text="¿Entregado?" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlCheck" runat="server" CssClass="form-control">
                                                                                    <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                                    <asp:ListItem Value="True">Si</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label23" CssClass="col-sm-5 control-label" runat="server" Text="Estado Pagaré" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlEdoPagare" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label24" CssClass="col-sm-5 control-label" runat="server" Text="Responsable" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlResponsable" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label25" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Aprobación" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaAprobacion" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbFechaRevision" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Revisión" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaRevision" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-10">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbObservacion" CssClass="col-sm-3 control-label" Style="text-align: right" runat="server" Text="Comentarios" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <textarea rows="5" cols="20" runat="server" id="txtComentarios" class="form-control"></textarea>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <!-- FIN ANTECEDENTES PAGARE -->
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <!-- BOTONES -->
                                    <br>
                                    <asp:Panel runat="server" ID="PnlBotones">
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-primary pull-right" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-warning pull-right" OnClientClick="return Dialogo();" OnClick="btnLimpiar_Click" />
                                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" OnClick="btnGuardar_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <!-- FIN BOTONES -->
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HdfCotizacion" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnSPIdOperacion" />
    <asp:HiddenField runat="server" ID="hdnMontoOperacionCLP" />
    <asp:HiddenField runat="server" ID="hdftabb" ClientIDMode="Static" Value="" />
    <asp:HiddenField runat="server" ID="HdfEtapa" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HdfSubEtapa" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HdfArea" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HdfTasaComisionAnual" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="HdfLicitacionFogape" ClientIDMode="Static" />
</div>

