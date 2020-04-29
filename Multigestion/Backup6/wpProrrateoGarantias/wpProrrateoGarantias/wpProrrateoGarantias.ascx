<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpProrrateoGarantias.ascx.cs" Inherits="MultiOperacion.wpProrrateoGarantias.wpProrrateoGarantias.wpProrrateoGarantias" %>

<style type="text/css">
    .btn { font:14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important; }
</style>

<script src="../../_layouts/15/MultiOperacion/Validaciones.js"></script>

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

    function CalculoOtros(IDgridOperaciones, cadenaGrid, cadena, montoVigente) {
        var totaGOtros = 50000; //parseFloat((document.getElementById(cadena + 'txtTotalOtros').value).replace(/\./g, '').replace(/\,/g, '.'));
        var diponibleOtros;//= parseFloat((document.getElementById(cadena + 'txtDisponibleOtros').value).replace(/\./g, '').replace(/\,/g, '.'));
        diponibleOtros = 50000;
        var valIngresado = parseFloat((document.getElementById(cadenaGrid + '_txtValOtros').value).replace(/\./g, '').replace(/\,/g, '.'));
        // document.getElementById(cadena + 'txtDisponibleOtros').style.borderColor = "Transparent";
        document.getElementById(cadenaGrid + '_txtValOtros').style.borderColor = "";
        if (totaGOtros == '' || totaGOtros == '0' || valIngresado == '' || valIngresado == '0' || isNaN(valIngresado)) {
            // document.getElementById(cadenaGrid + '_txtPorOtros').value = '0,000 %';
            document.getElementById(cadenaGrid + '_txtValOtros').value = '0,000';
        }


        CalculoTotalGarantias(IDgridOperaciones, cadenaGrid, cadena, montoVigente);
        CalculosTotales(IDgridOperaciones, cadenaGrid, cadena, montoVigente);

        if (parseFloat((document.getElementById(cadenaGrid + '_txtValOtros').value).replace(/\./g, '').replace(/\,/g, '.')) != '')
            document.getElementById(cadenaGrid + '_txtValOtros').value = formatNumber.new(parseFloat(parseFloat((document.getElementById(cadenaGrid + '_txtValOtros').value).replace(/\./g, '').replace(/\,/g, '.'))).toFixed(3));


    }

    function CalculoHipoteca(IDgridOperaciones, cadenaGrid, cadena, montoVigente) {

        var totaGHipoteca = 50000; //parseFloat((document.getElementById(cadena + 'txtTotalHip').value).replace(/\./g, '').replace(/\,/g, '.'));
        var diponibleHipoteca;//= parseFloat((document.getElementById(cadena + 'txtDisponibleHip').value).replace(/\./g, '').replace(/\,/g, '.'));
        diponibleHipoteca = 50000;
        var valIngresado = parseFloat((document.getElementById(cadenaGrid + '_txtValHipoteca').value).replace(/\./g, '').replace(/\,/g, '.'));
        // document.getElementById(cadena + 'txtDisponibleHip').style.borderColor = "Transparent";
        document.getElementById(cadenaGrid + '_txtValHipoteca').style.borderColor = "";
        if (totaGHipoteca == '' || totaGHipoteca == '0' || valIngresado == '' || valIngresado == '0' || isNaN(valIngresado)) {
            //  document.getElementById(cadenaGrid + '_txtPorHipoteca').value = '0,000 %';
            document.getElementById(cadenaGrid + '_txtValHipoteca').value = '0,000';
        }


        CalculoTotalGarantias(IDgridOperaciones, cadenaGrid, cadena, montoVigente);
        CalculosTotales(IDgridOperaciones, cadenaGrid, cadena, montoVigente);

        if (parseFloat((document.getElementById(cadenaGrid + '_txtValHipoteca').value).replace(/\./g, '').replace(/\,/g, '.')) != '')
            document.getElementById(cadenaGrid + '_txtValHipoteca').value = formatNumber.new(parseFloat(parseFloat((document.getElementById(cadenaGrid + '_txtValHipoteca').value).replace(/\./g, '').replace(/\,/g, '.'))).toFixed(3));

    }

    function CalculoPrendas(IDgridOperaciones, cadenaGrid, cadena, montoVigente) {
        var totaGPrenda = 50000;// parseFloat((document.getElementById(cadena + 'txtTotalPrendas').value).replace(/\./g, '').replace(/\,/g, '.'));
        var diponiblePrendas //= parseFloat((document.getElementById(cadena + 'txtDisponiblePrendas').value).replace(/\./g, '').replace(/\,/g, '.'));
        diponiblePrendas = 50000;
        var valIngresado = parseFloat((document.getElementById(cadenaGrid + '_txtValPrendas').value).replace(/\./g, '').replace(/\,/g, '.'));
        //  document.getElementById(cadena + 'txtDisponiblePrendas').style.borderColor = "Transparent";
        document.getElementById(cadenaGrid + '_txtValPrendas').style.borderColor = "";
        if (totaGPrenda == '' || totaGPrenda == '0' || valIngresado == '' || valIngresado == '0' || isNaN(valIngresado)) {
            //document.getElementById(cadenaGrid + '_txtPorPrendas').value = '0,000 %';
            document.getElementById(cadenaGrid + '_txtValPrendas').value = '0,000';
        }

        CalculoTotalGarantias(IDgridOperaciones, cadenaGrid, cadena, montoVigente);
        CalculosTotales(IDgridOperaciones, cadenaGrid, cadena, montoVigente);
        if (parseFloat((document.getElementById(cadenaGrid + '_txtValPrendas').value).replace(/\./g, '').replace(/\,/g, '.')) != '')
            document.getElementById(cadenaGrid + '_txtValPrendas').value = formatNumber.new(parseFloat(parseFloat((document.getElementById(cadenaGrid + '_txtValPrendas').value).replace(/\./g, '').replace(/\,/g, '.'))).toFixed(3));
    }

    function CalculoTotalGarantias(IDgridOperaciones, cadenaGrid, cadena, montoVigente) {
        var valIngresadoH = parseFloat((document.getElementById(cadenaGrid + '_txtValHipoteca').value).replace(/\./g, '').replace(/\,/g, '.'));
        if (isNaN(valIngresadoH) || valIngresadoH == '') valIngresadoH = 0;

        var valIngresadoP = parseFloat((document.getElementById(cadenaGrid + '_txtValPrendas').value).replace(/\./g, '').replace(/\,/g, '.'));
        if (isNaN(valIngresadoP) || valIngresadoP == '') valIngresadoP = 0;

        var valIngresadoO = parseFloat((document.getElementById(cadenaGrid + '_txtValOtros').value).replace(/\./g, '').replace(/\,/g, '.'));
        if (isNaN(valIngresadoO) || valIngresadoO == '') valIngresadoO = 0;

        var valIngresadoF = parseFloat((document.getElementById(cadenaGrid + '_txtFogape').value).replace(/\./g, '').replace(/\,/g, '.'));
        if (isNaN(valIngresadoF) || valIngresadoF == '') valIngresadoF = 0;

        var MontoVigente = parseFloat((document.getElementById(cadenaGrid + '_txtMontoVigente').value).replace(/\./g, '').replace(/\,/g, '.')); //parseFloat((document.getElementById(cadenaGrid + '_txtMontoDeudaTotal').value).replace(/\./g, '').replace(/\,/g, '.'));
        if (isNaN(MontoVigente) || MontoVigente == '') MontoVigente = 0;

        document.getElementById(cadenaGrid + '_TotalGarantias').value = formatNumber.new((valIngresadoH + valIngresadoP + valIngresadoO).toFixed(3));
        document.getElementById(cadenaGrid + '_TotalGarantiasFogape').value = formatNumber.new((valIngresadoH + valIngresadoP + valIngresadoO + valIngresadoF).toFixed(3));

        //calculo % cobertura
        if (MontoVigente != 0 || MontoVigente != '' || (valIngresadoH + valIngresadoP + valIngresadoO) != 0) {
            document.getElementById(cadenaGrid + '_txtCobertura').value = (((valIngresadoH + valIngresadoP + valIngresadoO) / parseFloat(MontoVigente)) * 100).toFixed(3).replace(/\./g, ',') + '%';
            var cc = document.getElementById(cadenaGrid + '_txtCobertura').value;
        }
        else
            document.getElementById(cadenaGrid + '_txtCobertura').value = '0,000';
        //calculo % cobertura con fogape
        if (MontoVigente != 0 || MontoVigente != '' || (valIngresadoH + valIngresadoP + valIngresadoO + valIngresadoF) != 0)
            document.getElementById(cadenaGrid + '_txtCoberturaFogape').value = (((valIngresadoH + valIngresadoP + valIngresadoO + valIngresadoF) / parseFloat(MontoVigente)) * 100).toFixed(3).replace(/\./g, ',') + '%';
        else
            document.getElementById(cadenaGrid + '_txtCoberturaFogape').value = '0,000';

    }


    function CalculosTotales(IDgridOperaciones, cadenaGrid, cadena, montoVigente) {

        var tabla = document.getElementById(IDgridOperaciones);
        var acumHipotecas = 0;
        var acumPrendas = 0;
        var acumOtros = 0;
        var acumGarantias = 0;
        var acumGarantiasFogape = 0;
        var total = tabla.rows.length - 1;
        for (var i = 0; i < total - 1; i++) {
            //var textos = tabla.rows[i].cells[0].innerHTML;
            //alert(textos);
            var j = (parseInt(i) + 2);
            if (j < 10)
                j = '0' + (parseInt(i) + 2);
            if (!isNaN(document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValHipoteca').value.replace(/\./g, '').replace(/\,/g, '.')) && document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValHipoteca').value != '') {
                acumHipotecas = acumHipotecas + parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValHipoteca').value.replace(/\./g, '').replace(/\,/g, '.'));
            }

            if (!isNaN(document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValPrendas').value.replace(/\./g, '').replace(/\,/g, '.')) && document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValPrendas').value != '') {
                acumPrendas = acumPrendas + parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValPrendas').value.replace(/\./g, '').replace(/\,/g, '.'));
            }
            if (!isNaN(document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValOtros').value.replace(/\./g, '').replace(/\,/g, '.')) && document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValOtros').value != '') {
                acumOtros = acumOtros + parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + j + '_txtValOtros').value.replace(/\./g, '').replace(/\,/g, '.'));
            }
            if (!isNaN(document.getElementById(IDgridOperaciones + '_ctl' + j + '_TotalGarantias').value.replace(/\./g, '').replace(/\,/g, '.')) && document.getElementById(IDgridOperaciones + '_ctl' + j + '_TotalGarantias').value != '') {
                acumGarantias = acumGarantias + parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + j + '_TotalGarantias').value.replace(/\./g, '').replace(/\,/g, '.'));

            }
            if (!isNaN(document.getElementById(IDgridOperaciones + '_ctl' + j + '_TotalGarantiasFogape').value.replace(/\./g, '').replace(/\,/g, '.')) && document.getElementById(IDgridOperaciones + '_ctl' + j + '_TotalGarantiasFogape').value != '') {
                acumGarantiasFogape = acumGarantiasFogape + parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + j + '_TotalGarantiasFogape').value.replace(/\./g, '').replace(/\,/g, '.'));

            }
        };
        var nro = parseInt(total) + 1;
        if (nro < 10)
            nro = '0' + nro;
        // txtTotalMontoVigente
        document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalHipotecas').value = formatNumber.new(parseFloat(acumHipotecas).toFixed(3));
        document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalPrendas').value = formatNumber.new(parseFloat(acumPrendas).toFixed(3));
        document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalOtros').value = formatNumber.new(parseFloat(acumOtros).toFixed(3));
        document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalGarantias').value = formatNumber.new(parseFloat(acumGarantias).toFixed(3));
        if (acumGarantias != 0)
            document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalCobertura').value = formatNumber.new(parseFloat((acumGarantias / parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'))) * 100).toFixed(3));
        else
            document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalCobertura').value = '0,000';
        document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalCoberturaFogape').value = formatNumber.new(parseFloat((acumGarantiasFogape / parseFloat(document.getElementById(IDgridOperaciones + '_ctl' + nro + '_txtTotalMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'))) * 100).toFixed(3));
    }

    function solonumerosN(e) {
        var unicode = e.charCode ? e.charCode : e.keyCode
        //if (unicode != 8) {
        //    if (unicode < 48 || unicode > 57)
        //        if (unicode != 45)
        //            return false
        //}
        if (unicode != 8) {
            if (unicode < 48 || unicode > 57) {
                if (unicode != 46 && unicode != 44)
                    return false
            }
        }

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


    function HabilitarText(cadenaGrid) {
        document.getElementById(cadenaGrid + '_txtValHipoteca').disabled = false;
        document.getElementById(cadenaGrid + '_txtValPrendas').disabled = false;
        document.getElementById(cadenaGrid + '_txtValOtros').disabled = false;
        return false;
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
            <h4 class="page-title">Prorrateo de Garantías</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Datos Empresa</a>
                </li>
                <li class="active">Prorrateo de Garantías
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

    <!-- tabs -->
    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li class="active">
                    <a href="#tab1" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton8" runat="server" OnClientClick="return Dialogo();" OnClick="lbProrrateo_Click">Prorrateo de Garantías</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton9" runat="server" OnClientClick="return Dialogo();" OnClick="lbDeudaSBIF_Click">Deuda SBIF</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab3" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton10" runat="server" OnClientClick="return Dialogo();" OnClick="lbClientes_Click">Clientes</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab4" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton11" runat="server" OnClientClick="return Dialogo();" OnClick="lbAdministracion_Click">Administración</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="">
                    <a href="#tab5" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="LinkButton12" runat="server" OnClientClick="return Dialogo();" OnClick="lbProveedores_Click">Proveedores</asp:LinkButton>
                        </span>
                    </a>
                </li>
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
                            <asp:Label ID="lbWarning" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>

                    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
                        <h4>
                            <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                        </h4>
                    </div>

                    <div class="row">
                        <!-- col 1/4 -->
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <h4>Garantías
                                <asp:LinkButton ID="ActualizarGarantias" runat="server" CssClass="glyphicon glyphicon-refresh" OnClick="ActualizarGarantias_Click"></asp:LinkButton>
                                </h4>
                            </div>
                        </div>
                        <!-- col 2/4 -->
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <!-- tabla / grilla -->
                        <div class="row" style="width: 95% !important">
                            <div class="col-md-12">
                                <table border="0" style="table-layout: fixed; width: 100%;">
                                    <tr>
                                        <td>
                                            <div class="card-box">
                                                <div class="table-responsive">
                                                    <asp:GridView ID="ResultadosBusquedaGarantias" runat="server" Width="100%" Font-Size="Smaller" class="table table-bordered table-hover" GridLines="Horizontal"
                                                        ShowHeaderWhenEmpty="True" OnRowDataBound="ResultadosBusquedaGarantias_RowDataBound" ShowFooter="True" OnDataBound="ResultadosBusquedaGarantias_DataBound" OnRowCreated="ResultadosBusquedaGarantias_RowCreated">
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <asp:Panel ID="pnFormulario" runat="server">

                            <!-- tabla / grilla -->
                            <div class="row" style="width: 95% !important">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gridResumen" runat="server"
                                                            class="table table-bordered table-hover"
                                                            GridLines="Horizontal"
                                                            Font-Size="Smaller" Width="100%"
                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
                                                            ShowFooter="True" OnDataBound="gridResumen_DataBound" OnRowCreated="gridResumen_RowCreated">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText=" ">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbClase" runat="server" Text='<%# Bind("Descripcion")%>'>   </asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lbTotalG" runat="server" Text="Total Garantías"></asp:Label>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Hipoteca (UF)">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="Hipoteca" runat="server" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="120"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalHipoteca" runat="server" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="120"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Prendas (UF)">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="Prendas" runat="server" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="120"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalPrendas" runat="server" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="120"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Otras Garantías (UF)">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="Otros" runat="server" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="120"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalOtros" runat="server" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="120"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                        </asp:Panel>
                    </div>

                    <div class="row">
                        <h4>Operaciones
                    <asp:LinkButton ID="ActualizarOperaciones" runat="server" CssClass="glyphicon glyphicon-refresh" OnClick="ActualizarOperaciones_Click"></asp:LinkButton>
                        </h4>
                    </div>
                    <br />

                    <div class="row">
                        <asp:Panel ID="pnFormularioAct" runat="server">

                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <!-- tabla / grilla -->
                            <div class="row" style="width: 95% !important">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="ResultadosBusquedaOperaciones" runat="server" Width="100%"
                                                            Font-Size="Smaller"
                                                            class="table table-bordered table-hover"
                                                            GridLines="Horizontal"
                                                            ShowHeaderWhenEmpty="True" AutoGenerateColumns="False"
                                                            OnDataBound="ResultadosBusquedaOperaciones_DataBound"
                                                            OnRowDataBound="ResultadosBusquedaOperaciones_RowDataBound" ShowFooter="True"
                                                            OnRowCreated="ResultadosBusquedaOperaciones_RowCreated">
                                                            <Columns>
                                                                <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                                                <asp:BoundField DataField="Nº CF - Producto" HeaderText="Nº CF - Producto" />
                                                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                                <asp:BoundField DataField="Fondo" HeaderText="Fondo - Programa - %Cob. CF" />
                                                                <asp:TemplateField HeaderText="Monto Vigente UF">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtMontoVigente" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalMontoVigente" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Monto Siniestrado">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtMontoSiniestrado" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalMontoSiniestrado" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Monto Recuperado">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtMontoRecuperado" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalMontoRecuperado" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Deuda Total">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtMontoDeudaTotal" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalMontoDeudaTotal" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="% Deuda">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtDeuda" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalDeuda" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Hipotecas UF">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPorHipoteca" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                        <asp:TextBox ID="txtValHipoteca" runat="server" MaxLength="16" Width="80" class="form-control input10px" onKeyPress="return solonumerosN(event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalHipotecas" runat="server" class="input10px" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Prendas UF">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPorPrendas" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                        <asp:TextBox ID="txtValPrendas" runat="server" class="form-control input10px" MaxLength="16" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalPrendas" runat="server" class="input10px" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Otros UF">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPorOtros" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                        <asp:TextBox ID="txtValOtros" runat="server" MaxLength="16" Width="80" class="form-control input10px"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalOtros" runat="server" class="input10px" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total Garantías UF">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="TotalGarantias" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalGarantias" runat="server" class="input10px" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="% Cobertura">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtCobertura" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalCobertura" class="input10px" runat="server" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:BoundField DataField="TipoContragarantia" HeaderText="Tipo Contragarantia, Fecha Const." />
                                                                <asp:BoundField DataField="IdEmpresa" HeaderText="IdEmpresa" />
                                                                <asp:BoundField DataField="IdOperacion" HeaderText="IdOperacion" />
                                                                <asp:BoundField DataField="OrdenAnexo" HeaderText="OrdenAnexo" />
                                                                <asp:BoundField DataField="OrdenCorfoInterno" HeaderText="OrdenCorfoInterno" />
                                                                <asp:TemplateField HeaderText="Fogape">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtFogape" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalFogape" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Total Garantías UF con Fogape">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="TotalGarantiasFogape" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalGarantiasFogape" runat="server" class="input10px" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="% Cobertura con Fogape">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:TextBox ID="txtCoberturaFogape" runat="server" class="input10px" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent" Width="80"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtTotalCoberturaFogape" class="input10px" runat="server" Width="80" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="TipoContragarantiaFogape" HeaderText="Tipo Contragarantia , Fecha Const. Con Fogape" />

                                                                <asp:BoundField DataField="FechaEmision" HeaderText="" />
                                                                <asp:TemplateField HeaderText="">
                                                                    <ItemTemplate>
                                                                        <br />
                                                                        <asp:LinkButton ID="Editar" runat="server" CssClass="glyphicon glyphicon-pencil" CommandName="Editar"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <br />

                    <table style="width: 95%">
                        <tr>
                            <td>
                                <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnAtras_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

                                <asp:Button ID="btnCancel" runat="server" Text="Imprimir" OnClick="btnImprimir_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                                <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                            </td>
                        </tr>
                    </table>


                    <asp:HiddenField ID="hddTipoContragarantia" runat="server" />
                    <asp:HiddenField ID="hddFechaContrato" runat="server" />

                </div>
            </div>
        </div>
    </div>



    <br />
    <br />
    <br />
    * Las conversiones a UF  se realizan en base a:
    <br />
    Garantías: Fecha de Tasación.
    <br />
    Operaciones Vigentes: Fecha Cierre.

</div>
