<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPAF.ascx.cs" Inherits="MultiRiesgo.wpPAF.wpPAF.wpPAF" %>

<style type="text/css">
    .btn {
        font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important;
    }

    .cDateBox {
        width: 100px;
    }
</style>

<script src="../../_layouts/15/MultiRiesgo/Validaciones.js"></script>
<script src="../../_layouts/15/MultiRiesgo/PAF.js"></script>
<script src="../../_layouts/15/MultiRiesgo/jquery-1.11.1.js"></script>

<script>
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    });

    function Adjuntar(muestra) {
        var ficha = document.getElementById(muestra);
        document.getElementById('Botones').style.display = "none";
        ficha.style.display = "none";
        document.getElementById('DivArchivo').style.display = "block";
        return false;
    }

    function soloDecimales(e) {
        key = e.keyCode ? e.keyCode : e.which
        // 0-9
        if (key > 47 && key < 58) {
            return true
        }
        // ,
        if (key == 44) {
            return true
        }
        return false
    }

    ////////grilla operaciones vigentes MONTO PROPUESTO//////////////////////////////////////////////////////////////////////////   
    function SumatoriaColumGrid(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, valorDolar, valorUF, IdGridGarantias) {
        var MontoPropuestoCertificadoVigente = 0;
        var MontoPropuestoCertificadoVigente_2 = 0;

        var MontoVigenteCertificadoVigente = 0;
        var MontoVigenteCertificadoVigente_2 = 0;

        var tabla = document.getElementById(IDgridOperacionesVigentes);
        var coberturaCertificado = 0;
        var div_to_disable = document.getElementById(IDgridOperacionesVigentes).getElementsByTagName("input");
        var children = div_to_disable;
        var acumCLP = 0;
        var acumUSD = 0;
        var acumUF = 0;
        $("#<%=OpeVigente.ClientID%>").val(0);
        var moneda;
        var total = tabla.rows.length - 1;
        for (var i = 0; i < total - 1; i++) {
            if (i > 7) {
                moneda = document.getElementById(IDgridOperacionesVigentes + '_ctl' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
                coberturaCertificado = isNaN(tabla.rows[parseInt(i) + 1].cells[6].childNodes[0].nodeValue / 100) ? 0 : tabla.rows[parseInt(i) + 1].cells[6].childNodes[0].nodeValue / 100;
            }
            else {
                moneda = document.getElementById(IDgridOperacionesVigentes + '_ctl0' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
                coberturaCertificado = isNaN(tabla.rows[parseInt(i) + 1].cells[6].childNodes[0].nodeValue / 100) ? 0 : tabla.rows[parseInt(i) + 1].cells[6].childNodes[0].nodeValue / 100;
            }

            if (moneda == 'CLP') {
                acumCLP = acumCLP + (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoVigente = MontoPropuestoCertificadoVigente + ((parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF));
                MontoPropuestoCertificadoVigente_2 = MontoPropuestoCertificadoVigente_2 + (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF);

                $("#<%=OpeVigente.ClientID%>").val(MontoPropuestoCertificadoVigente_2)
                //acumCLP = acumCLP + (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')));
                //MontoPropuestoCertificadoVigente = MontoPropuestoCertificadoVigente + ((parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF));
                //MontoPropuestoCertificadoVigente_2 = MontoPropuestoCertificadoVigente_2 + (coberturaCertificado * (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF));
            }
            if (moneda == 'UF') {
                acumUF = acumUF + (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoVigente = MontoPropuestoCertificadoVigente + (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoVigente_2 = MontoPropuestoCertificadoVigente_2 + parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.'));

                $("#<%=OpeVigente.ClientID%>").val(MontoPropuestoCertificadoVigente_2)
            }
            if (moneda == 'USD') {
                acumUSD = acumUSD + (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoVigente = MontoPropuestoCertificadoVigente + (((parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')) * valorDolar) / valorUF));
                MontoPropuestoCertificadoVigente_2 = MontoPropuestoCertificadoVigente_2 + ((parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')) * valorDolar) / valorUF);

                $("#<%=OpeVigente.ClientID%>").val(MontoPropuestoCertificadoVigente_2)
            }

        };

        var nro = parseInt(total) + 1;
        if (nro < 10)
            nro = '0' + nro;

        document.getElementById(IDgridOperacionesVigentes + '_ctl' + nro + '_txtTotalMontoPropuestoCLP').value = formatNumber.new(parseFloat(acumCLP).toFixed(0));
        document.getElementById(IDgridOperacionesVigentes + '_ctl' + nro + '_txtTotalMontoPropuestoUSD').value = formatNumber.new(parseFloat(acumUSD).toFixed(2));
        document.getElementById(IDgridOperacionesVigentes + '_ctl' + nro + '_txtTotalMontoPropuestoUF').value = formatNumber.new(parseFloat(acumUF).toFixed(2));
        var UFacumUSD = isNaN((acumUSD * valorDolar) / valorUF) ? 0 : (acumUSD * valorDolar) / valorUF;
        var UFacumCLP = isNaN(acumCLP / valorUF) ? 0 : acumCLP / valorUF;
        document.getElementById(IDgridOperacionesVigentes + '_ctl' + nro + '_txtTotalMontoPropuestoREUF').value = formatNumber.new(parseFloat(acumUF + UFacumUSD + UFacumCLP).toFixed(2));

        SumatoriaLinea(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, IdGridGarantias, valorDolar, valorUF);
    }


    ///////grilla operaciones nuevas MONTO PROPUESTO/////////////////////////////////////////////////////////////////////////////
    function SumatoriaColumGridMP(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, valorDolar, valorUF, IdGridGarantias) {
        var MontoPropuestoCertificadoNueva = 0;
        var MontoPropuestoCertificadoNueva_2 = 0;
        var tabla = document.getElementById(IDgridOperacionesNuevas);
        var coberturaCertificado = 0;
        var acumCLP = 0;
        var acumUSD = 0;
        var acumUF = 0;
        var moneda;
        $("#<%=OpeNueva.ClientID%>").val(0);
        var div_to_disable = document.getElementById(IDgridOperacionesNuevas).getElementsByTagName("input");
        var children = div_to_disable;
        var total = tabla.rows.length - 1;
        for (var i = 0; i < total - 1; i++) {
            if (i > 7)
                moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
            else
                moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl0' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;

            var j = (parseInt(i) + 2);
            if (j < 10)
                j = '0' + (parseInt(i) + 2);

            coberturaCertificado = isNaN(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtCobertura').value.replace(/\./g, '').replace(/\,/g, '.') / 100) ? 0 : document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtCobertura').value.replace(/\./g, '').replace(/\,/g, '.') / 100;

            if (moneda == 'CLP') {
                acumCLP = acumCLP + (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoNueva = MontoPropuestoCertificadoNueva + (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF);
                MontoPropuestoCertificadoNueva_2 = MontoPropuestoCertificadoNueva_2 + (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF);

                //acumCLP = acumCLP + coberturaCertificado * (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')));
                //MontoPropuestoCertificadoNueva = MontoPropuestoCertificadoNueva + coberturaCertificado * (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF);
                //MontoPropuestoCertificadoNueva_2 = MontoPropuestoCertificadoNueva_2 + coberturaCertificado * (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')) / valorUF);

                //document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value = formatNumber.new(parseFloat(acumCLP).toFixed(0));
                $("#<%=OpeNueva.ClientID%>").val(MontoPropuestoCertificadoNueva_2)
            }

            if (moneda == 'UF') {
                acumUF = acumUF + (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoNueva = MontoPropuestoCertificadoNueva + isNaN((parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')))) ? 0 : (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoNueva_2 = MontoPropuestoCertificadoNueva_2 + isNaN((parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')))) ? 0 : (parseFloat(children[i].value.replace(/\./g, '').replace(/\,/g, '.')));

                //document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value = formatNumber.new(parseFloat(acumUF).toFixed(0));
                $("#<%=OpeNueva.ClientID%>").val(MontoPropuestoCertificadoNueva_2)
            }

            if (moneda == 'USD') {
                acumUSD = acumUSD + (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')));
                MontoPropuestoCertificadoNueva = MontoPropuestoCertificadoNueva + isNaN((parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')) * valorDolar) / valorUF) ? 0 : (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.') * valorDolar) / valorUF);
                MontoPropuestoCertificadoNueva_2 = MontoPropuestoCertificadoNueva_2 + isNaN((parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.')) * valorDolar) / valorUF) ? 0 : (parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.') * valorDolar) / valorUF);

                //document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value = formatNumber.new(parseFloat(acumUSD).toFixed(0));
                $("#<%=OpeNueva.ClientID%>").val(MontoPropuestoCertificadoNueva_2)
            }

        };
        var nro = parseInt(total) + 1;
        if (nro < 10)
            nro = '0' + nro;

        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoPropuestoCLP').value = formatNumber.new(parseFloat(acumCLP).toFixed(0));
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoPropuestoUSD').value = formatNumber.new(parseFloat(acumUSD).toFixed(2));
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoPropuestoUF').value = formatNumber.new(parseFloat(acumUF).toFixed(2));  //formatNumber.new(parseFloat(acumCLP / valorUF).toFixed(2));
        var UFacumUSD = isNaN((acumUSD * valorDolar) / valorUF) ? 0 : (acumUSD * valorDolar) / valorUF;
        var UFacumCLP = isNaN(acumCLP / valorUF) ? 0 : acumCLP / valorUF;
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoPropuestoREUF').value = formatNumber.new(parseFloat(acumUF + UFacumUSD + UFacumCLP).toFixed(2));

        SumatoriaLinea(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, IdGridGarantias, valorDolar, valorUF);
    }

    function SumatoriaColumGridMA(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, valorDolar, valorUF, IdGridGarantias) {

        var tabla = document.getElementById(IDgridOperacionesNuevas);
        var acumCLP = 0;
        var acumUSD = 0;
        var acumUF = 0;

        var moneda;
        var total = tabla.rows.length - 1;
        for (var i = 0; i < total - 1; i++) {
            if (i > 7)
                moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
            else
                moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl0' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
            var j = (parseInt(i) + 2);
            if (j < 10)
                j = '0' + (parseInt(i) + 2);

            if (moneda == 'CLP')
                acumCLP = acumCLP + parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoAprobado').value.replace(/\./g, '').replace(/\,/g, '.'));
            if (moneda == 'UF')
                acumUF = acumUF + parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoAprobado').value.replace(/\./g, '').replace(/\,/g, '.'));
            if (moneda == 'USD')
                acumUSD = acumUSD + parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoAprobado').value.replace(/\./g, '').replace(/\,/g, '.'));

        };
        var nro = parseInt(total) + 1;
        if (nro < 10)
            nro = '0' + nro;

        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoAprobadoCLP').value = formatNumber.new(parseFloat(acumCLP).toFixed(0));
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoAprobadoUSD').value = formatNumber.new(parseFloat(acumUSD).toFixed(2));
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoAprobadoUF').value = formatNumber.new(parseFloat(acumUF).toFixed(2)); //formatNumber.new(parseFloat(acumCLP / valorUF).toFixed(2));
        var UFacumUSD = isNaN((acumUSD * valorDolar) / valorUF) ? 0 : (acumUSD * valorDolar) / valorUF;
        var UFacumCLP = isNaN(acumCLP / valorUF) ? 0 : acumCLP / valorUF;
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoAprobadoREUF').value = formatNumber.new(parseFloat(acumUF + UFacumUSD + UFacumCLP).toFixed(2));

        SumatoriaLinea(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, IdGridGarantias, valorDolar, valorUF);
    }

    function SumatoriaColumGridMV(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, valorDolar, valorUF, IdGridGarantias) {

        var tabla = document.getElementById(IDgridOperacionesNuevas);
        var acumCLP = 0;
        var acumUSD = 0;
        var acumUF = 0;

        var moneda;
        var total = tabla.rows.length - 1;
        for (var i = 0; i < total - 1; i++) {
            if (i > 7)
                moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
            else
                moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl0' + (parseInt(i) + 2) + '_lbTipoMoneda').innerHTML;
            var j = (parseInt(i) + 2);
            if (j < 10)
                j = '0' + (parseInt(i) + 2);

            if (moneda == 'CLP')
                acumCLP = acumCLP + parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'));
            if (moneda == 'UF')
                acumUF = acumUF + parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'));
            if (moneda == 'USD')
                acumUSD = acumUSD + parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'));

        };
        var nro = parseInt(total) + 1;
        if (nro < 10)
            nro = '0' + nro;

        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoVigenteCLP').value = formatNumber.new(parseFloat(acumCLP).toFixed(0));
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoVigenteUSD').value = formatNumber.new(parseFloat(acumUSD).toFixed(2));
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoVigenteUF').value = formatNumber.new(parseFloat(acumUF).toFixed(2)); //formatNumber.new(parseFloat(acumCLP / valorUF).toFixed(2));
        var UFacumUSD = isNaN((acumUSD * valorDolar) / valorUF) ? 0 : (acumUSD * valorDolar) / valorUF;
        var UFacumCLP = isNaN(acumCLP / valorUF) ? 0 : acumCLP / valorUF;
        document.getElementById(IDgridOperacionesNuevas + '_ctl' + nro + '_txtTotalMontoVigenteREUF').value = formatNumber.new(parseFloat(acumUF + UFacumUSD + UFacumCLP).toFixed(2));

        SumatoriaLinea(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, IdGridGarantias, valorDolar, valorUF);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function SumatoriaLinea(IDgridOperacionesVigentes, IDgridOperacionesNuevas, cadena, IdGridGarantias, valorDolar, valorUF) {
        var tabla1 = document.getElementById(IDgridOperacionesVigentes);
        var tabla2 = document.getElementById(IDgridOperacionesNuevas);
        if (tabla1 == null)
            var total1 = 0;
        else
            var total1 = tabla1.rows.length;

        if (tabla2 == null)
            var total2 = 0;
        else
            var total2 = tabla2.rows.length;

        if (total1 < 10)
            total1 = '0' + total1;
        if (total2 < 10)
            total2 = '0' + total2;

        var txtTotalMontoAprobadoCLPVigente = 0;
        var txtTotalMontoAprobadoUSDVigente = 0;
        var txtTotalMontoAprobadoUFVigente = 0;
        //var txtTotalMontoAprobadoREUF = 0;
        var txtTotalMontoVigenteCLPVigente = 0;
        var txtTotalMontoVigenteUSDVigente = 0;
        var txtTotalMontoVigenteUFVigente = 0;
        //var txtTotalMontoVigenteREUF = 0;
        var txtTotalMontoPropuestoCLPVigente = 0;
        var txtTotalMontoPropuestoUSDVigente = 0;
        var txtTotalMontoPropuestoUFVigente = 0;
        //var txtTotalMontoPropuestoREUF = 0;

        var txtTotalMontoAprobadoCLPNuevas = 0;
        var txtTotalMontoAprobadoUSDNuevas = 0;
        var txtTotalMontoAprobadoUFNuevas = 0;
        //var txtTotalMontoAprobadoREUF = 0;
        var txtTotalMontoVigenteCLPNuevas = 0;
        var txtTotalMontoVigenteUSDNuevas = 0;
        var txtTotalMontoVigenteUFNuevas = 0;
        //var txtTotalMontoVigenteREUF = 0;
        var txtTotalMontoPropuestoCLPNuevas = 0;
        var txtTotalMontoPropuestoUSDNuevas = 0;
        var txtTotalMontoPropuestoUFNuevas = 0;

        //MontoAprobado

        var moneda = null;
        var uf = parseFloat(document.getElementById(cadena + 'txtUFV').value.replace(/\./g, '').replace(/\,/g, '.'));
        var usd = parseFloat(document.getElementById(cadena + 'txtDOLARV').value.replace(/\./g, '').replace(/\,/g, '.'));

        var totalVigentes = tabla1 == null ? 0 : tabla1.rows.length - 1;

        for (var i = 0; i < totalVigentes - 1; i++) {
            var coberturaCertificadoVigente = 0;
            var j = (parseInt(i) + 2);
            if (j < 10)
                j = '0' + (parseInt(i) + 2);

            coberturaCertificadoVigente = isNaN(tabla1.rows[parseInt(i) + 1].cells[6].childNodes[0].nodeValue / 100) ? 0 : tabla1.rows[parseInt(i) + 1].cells[6].childNodes[0].nodeValue / 100;
            //alert(coberturaCertificadoVigente);
            moneda = document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_lbTipoMoneda').innerHTML.trim();
            //alert(moneda);

            if (moneda == 'CLP') {
                txtTotalMontoAprobadoCLPVigente = txtTotalMontoAprobadoCLPVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoAprobado').innerHTML.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoVigenteCLPVigente = txtTotalMontoVigenteCLPVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoVigente').innerHTML.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoCLPVigente = txtTotalMontoPropuestoCLPVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.'));
            }

            if (moneda == 'USD') {
                txtTotalMontoVigenteUSDVigente = txtTotalMontoVigenteUSDVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoAprobado').innerHTML.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoUSDVigente = txtTotalMontoPropuestoUSDVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoVigente').innerHTML.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoUSDVigente = txtTotalMontoPropuestoUSDVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.'));
            }

            if (moneda == 'UF') {
                txtTotalMontoAprobadoUFVigente = txtTotalMontoAprobadoUFVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoAprobado').innerHTML.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoVigenteUFVigente = txtTotalMontoVigenteUFVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoVigente').innerHTML.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoUFVigente = txtTotalMontoPropuestoUFVigente + coberturaCertificadoVigente * parseFloat(document.getElementById(IDgridOperacionesVigentes + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.'));
            }
        }

        var totalNuevos = tabla2 == null ? 0 : tabla2.rows.length - 1;

        for (var i = 0; i < totalNuevos - 1; i++) {
            var coberturaCertificadoNuevo = 0;
            var j = (parseInt(i) + 2);
            if (j < 10)
                j = '0' + (parseInt(i) + 2);

            coberturaCertificadoNuevo = isNaN(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtCobertura').value.replace(/\./g, '').replace(/\,/g, '.') / 100) ? 0 : document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtCobertura').value.replace(/\./g, '').replace(/\,/g, '.') / 100;
            moneda = document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_lbTipoMoneda').innerHTML.trim();

            if (moneda == 'CLP') {
                txtTotalMontoAprobadoCLPNuevas = txtTotalMontoAprobadoCLPNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoAprobado').value.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoVigenteCLPNuevas = txtTotalMontoVigenteCLPNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoCLPNuevas = txtTotalMontoPropuestoCLPNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.'));
            }

            if (moneda == 'USD') {
                txtTotalMontoVigenteUSDNuevas = txtTotalMontoVigenteUSDNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoAprobado').value.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoUSDNuevas = txtTotalMontoPropuestoUSDNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoUSDNuevas = txtTotalMontoPropuestoUSDNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.'));
            }

            if (moneda == 'UF') {
                txtTotalMontoAprobadoUFNuevas = txtTotalMontoAprobadoUFNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoAprobado').value.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoVigenteUFNuevas = txtTotalMontoVigenteUFNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoVigente').value.replace(/\./g, '').replace(/\,/g, '.'));
                txtTotalMontoPropuestoUFNuevas = txtTotalMontoPropuestoUFNuevas + coberturaCertificadoNuevo * parseFloat(document.getElementById(IDgridOperacionesNuevas + '_ctl' + j + '_txtMontoPropuesto').value.replace(/\./g, '').replace(/\,/g, '.'));
            }
        }

        var txtTotalMontoPropuestoCLPTemp = parseFloat(txtTotalMontoPropuestoCLPVigente + txtTotalMontoPropuestoCLPNuevas);
        var txtTotalMontoPropuestoUDSTemp = parseFloat(txtTotalMontoPropuestoUSDVigente + txtTotalMontoPropuestoUSDNuevas);
        var txtTotalMontoPropuestoUFTemp = parseFloat(txtTotalMontoPropuestoUFVigente + txtTotalMontoPropuestoUFNuevas);

        document.getElementById(cadena + 'txtTotalMontoPropuestoCLP').value = formatNumber.new(txtTotalMontoPropuestoCLPTemp.toFixed(0));
        document.getElementById(cadena + 'txtTotalMontoPropuestoUDS').value = formatNumber.new(txtTotalMontoPropuestoUDSTemp.toFixed(2));
        document.getElementById(cadena + 'txtTotalMontoPropuestoUF').value = formatNumber.new(txtTotalMontoPropuestoUFTemp.toFixed(2));

        //document.getElementById(cadena + 'txtTotalMontoAprobadoREUF').value = parseFloat(document.getElementById(cadena + 'txtTotalMontoPropuestoCLP').value.replace(/\./g, '').replace(/\,/g, '.'));
        //document.getElementById(cadena + 'txtTotalMontoVigenteREUF').value = 0;
        document.getElementById(cadena + 'txtTotalMontoPropuestoREUF').value = formatNumber.new(((txtTotalMontoPropuestoCLPTemp + (txtTotalMontoPropuestoUDSTemp * usd) + (txtTotalMontoPropuestoUFTemp * uf)) / uf).toFixed(2));


        Cobertura(cadena, IdGridGarantias, valorDolar, valorUF, IDgridOperacionesVigentes, IDgridOperacionesNuevas);
    }


    //modificado el 15-06-2017
    function Cobertura(cadena, IdGridGarantias, valorDolar, valorUF, IDgridOperacionesVigentes, IDgridOperacionesNuevas) {
        if ($("#<%=OpeVigente.ClientID%>") != null && $("#<%=OpeNueva.ClientID%>") != null) {
            var tabla1 = document.getElementById(IdGridGarantias);
            var tabla2 = document.getElementById(IDgridOperacionesVigentes);
            var tabla3 = document.getElementById(IDgridOperacionesNuevas);

            if (tabla1 != null) {
                if (tabla1 == null)
                    var total1 = 0;
                else
                    var total1 = tabla1.rows.length;
                if (total1 < 10)
                    total1 = '0' + total1;
            }
            else
                var total1 = 0;

            if (tabla2 != null) {
                var total2 = tabla2.rows.length;
                if (total2 < 10)
                    total2 = '0' + total2;
            }
            else
                var total2 = 0;

            if (tabla3 != null) {
                var total3 = tabla3.rows.length;
                if (total3 < 10)
                    total3 = '0' + total3;
            }
            else
                total3 = 0;

            //Cobertura Vigente
            if (tabla1 != null) {
                //var dd = isNaN(parseFloat($("#<%=OpeVigente.ClientID%>").val())) ? 0 : parseFloat($("#<%=OpeVigente.ClientID%>").val()); //total operaciones vigentes
                //alert($("#<%=OpeVigente.ClientID%>").val());
                var dd = document.getElementById(cadena + 'txtTotalMontoVigenteREUF').value == null ? 0 : parseFloat(document.getElementById(cadena + 'txtTotalMontoVigenteREUF').value.replace(/\./g, '').replace(/\,/g, '.'));
                var ee = parseFloat(document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalGarantiaComercialUF').value.replace(/\./g, '').replace(/\,/g, '.'));   //total garantias vigentes

                var cc = 0;
                if (dd == 0)
                    cc = 0;
                else
                    cc = isNaN((ee / dd) * 100) ? 0 : (ee / dd) * 100;
                document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalCoberturaComercialVigente').value = formatNumber.new(parseFloat(cc).toFixed(4));
                                                                                                                        
                //ValorAjustado
                var aa = parseFloat(document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalGarantiaAjustadoUF').value.replace(/\./g, '').replace(/\,/g, '.'));
                if (cc == 0)
                    cc = 0;
                else
                    cc = isNaN((aa / dd) * 100) ? 0 : (aa / dd) * 100;
                document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalCoberturaAjustadoVigente').value = formatNumber.new(parseFloat(cc).toFixed(4));
            }

            //Cobertura global = vigentes + nuevas
            if (parseFloat(document.getElementById(cadena + 'txtTotalMontoPropuestoREUF').value.replace(/\./g, '').replace(/\,/g, '.')) > 0) {
                //alert($("#<%=OpeNueva.ClientID%>").val());
                var bb = parseFloat(document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalGarantiaComercialUF').value.replace(/\./g, '').replace(/\,/g, '.'))
                var aa = parseFloat(document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalGarantiaAjustadoUF').value.replace(/\./g, '').replace(/\,/g, '.'));

                var vigente = 0;
                var nueva = 0;

                if (tabla2 != null)
                    vigente = document.getElementById(cadena + 'txtTotalMontoVigenteREUF').value == null ? 0 : parseFloat(document.getElementById(cadena + 'txtTotalMontoVigenteREUF').value.replace(/\./g, '').replace(/\,/g, '.'));
                else
                    vigente = 0;

                if (tabla3 != null)
                    nueva = $("#<%=OpeNueva.ClientID%>") == null ? 0 : parseFloat($("#<%=OpeNueva.ClientID%>").val());
                        //document.getElementById(cadena + 'txtTotalMontoPropuestoREUF').value == null ? 0 : parseFloat(document.getElementById(cadena + 'txtTotalMontoPropuestoREUF').value.replace(/\./g, '').replace(/\,/g, '.'));
                else
                    nueva = 0;

                //alert(nueva);
                var cc = parseFloat(vigente) + parseFloat(nueva);
                //ValorComercial global
                    var dd = 0;
                    if (cc == 0)
                        dd = 0;
                    else
                        dd = isNaN((bb / cc) * 100) ? 0 : (bb / cc) * 100;
                    document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalMCoberturaComercialGlobal').value = formatNumber.new(parseFloat(dd).toFixed(4));

                //ValorAjustado global
                    if (cc == 0)
                        dd = 0;
                    else
                        dd = isNaN((aa / cc) * 100) ? 0 : (aa / cc) * 100;
                    document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalMCoberturaAjustadoGlobal').value = formatNumber.new(parseFloat(dd).toFixed(4));
                }
                else {
                    document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalMCoberturaComercialGlobal').value = '0,00';
                    document.getElementById(IdGridGarantias + '_ctl' + total1 + '_txtTotalMCoberturaAjustadoGlobal').value = '0,00';
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

        function ValidarCheck(rowindex) {
            var i = parseInt(rowindex) + 1;
            var table = document.getElementById('<%=Garantias.ClientID %>');

        // $("#<%=Garantias.ClientID%> tr").each(function () {
        //     var $checkBox = $(this).find("input[type='radio']");
        //     if ($checkBox.is(':checked')) {
        //         $checkBox.prop('checked', true);
        //         alert($checkBox);
        //     }
        //  });


        $("#<%=Garantias.ClientID%> input[id*='RbtSeguros']:radio").each(function (index) {
            if ($(this).is(':checked')) {
                $(this).attr('checked', true);
                $('input:radio')[0].checked = true;
                //$(this).prop('checked', 'checked');
            }
            //if($(this).index.is(':checked'))
        });


        //var checked_radio = $("[id*='RbtSeguros'] input[type='radio']");
        //        var value = checked_radio.val();
        //        var text = checked_radio.closest("td").find("label").html();
        //        alert("Text: " + text + " Value: " + value);
        //return false;

        //var table, tbody, i, rowLen, row, j, colLen, cell;

        //table = document.getElementById('<%=Garantias.ClientID %>');
        //tbody = table.tBodies[0];

        //for (i = 1, rowLen = tbody.rows.length - 1; i < rowLen; i++) {
        //    row = tbody.rows[i];
        //    for (j = 0, colLen = row.cells.length; j < colLen; j++) {
        //        cell = row.cells[j];
        //        alert(row.cells[j].children);
        //        if (j == 8) {
        //            for (k = 0; k < cell.childNodes.length; k++) {
        //                var valorCheck = cell.childNodes[1].val();
        //                alert(valorcheck);
        //            }
        //       }
        //if (j == 1) {
        //    alert("--Found gender: " + cell.innerHTML);
        //} else if (j == 2) {
        //    console.log("--Found age: " + cell.innerHTML);
        //}
        //   }
        // }


        //  $('#<%=Garantias.ClientID%> input[type=radio]').click(function (index) {
        //      var checked = $(this).is(':checked');
        //      if (checked) {
        //          $(this).prop('checked', 'checked');
        //          alert(3);
        //      }
        //  });
    }

    function ValidarCheck1(rowindex) {
        var checkedCheckBox;
        var dataGrid = document.getElementById('<%=Garantias.ClientID %>');
        var rows = dataGrid.rows;
        for (var index = 1; index < rows.length; index++) {
            var checkBox = rows[index].cells[8].children;
            if (checkBox.Checked) {
                checkedCheckBox = checkBox;
                //alert('ok');
            }
        }
        //return checkedCheckBox;
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
            <h4 class="page-title">Balance</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Documentos Comerciales</a>
                </li>
                <li class="active">PAF
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
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
        <h4>
            <asp:Label ID="lbSuccess" runat="server"></asp:Label>
        </h4>
    </div>

    <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
        <h4>
            <asp:Label ID="lbWarning" runat="server"></asp:Label>
        </h4>
    </div>

    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
        <h4>
            <asp:Label ID="lbError" runat="server"></asp:Label>
        </h4>
    </div>


    <asp:Panel ID="MenuTabs" runat="server">
        <div class="row">
            <div class="col-md-12">
                <ul class="nav nav-tabs navtab-bg nav-justified">
                    <li>
                        <a href="#tab1" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbBalance" runat="server" OnClientClick="return Dialogo();" OnClick="lbBalance_Click">Balance</asp:LinkButton>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#tab2" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbEdoResultado" runat="server" OnClientClick="return Dialogo();" OnClick="lbEdoResultado_Click">Estado de Resultados</asp:LinkButton>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#tab3" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbCompras" runat="server" OnClientClick="return Dialogo();" OnClick="lbCompras_Click">IVA Compras</asp:LinkButton>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#tab4" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbVentas" runat="server" OnClientClick="return Dialogo();" OnClick="lbVentas_Click">IVA Ventas</asp:LinkButton>
                            </span>
                        </a>
                    </li>

                    <li>
                        <a href="#tab5" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbScoring" runat="server" OnClientClick="return Dialogo();" OnClick="lbScoring_Click">Scoring</asp:LinkButton>
                            </span>
                        </a>
                    </li>

                    <li class="active">
                        <a href="#tab6" data-toggle="tab" aria-expanded="true">
                            <span class="visible-xs"><i class="fa fa-home"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbResumenPAF" runat="server" OnClientClick="return Dialogo();" OnClick="lbResumenPAF_Click">Resumen PAF</asp:LinkButton>
                            </span>
                        </a>

                    </li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="tab6">
                        <div id="imprimir">

                            <div class="row">
                                <div class="col-md-12">

                                    <div>
                                        <asp:CheckBox ID="ckSinDocumentos" runat="server" AutoPostBack="True" OnCheckedChanged="ckSinDocumentos_CheckedChanged" OnClientClick="return Dialogo();" Text="Empresa sin Documentos Contables" />
                                    </div>

                                    <div class="col-md-12 col-sm-12" style="text-align: center">
                                        <h3>PROPUESTA DE AFIANZAMIENTO</h3>
                                        <h4>Ficha N°
                                            <asp:Label ID="lblPaf" runat="server" Text="0"></asp:Label>
                                        </h4>
                                    </div>

                                    <div class="col-md-3 col-sm-6">
                                        <table class="table table-bordered" style="font-size: small;">
                                            <tr>
                                                <td class="auto-style11"><b>Oficina:</b></td>
                                                <td class="auto-style13">1</td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style11"><b>Fecha:</b></td>
                                                <td class="auto-style13">
                                                    <SharePoint:DateTimeControl ID="ctrFecha" runat="server" DateOnly="True" AutoPostBack="True" CssClassTextBox="cDateBox" MinDate=""
                                                        LocaleId="13322" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style11"><b>Fe.Revisión:</b></td>
                                                <td class="auto-style13">
                                                    <SharePoint:DateTimeControl ID="ctrFechaRevision" runat="server" DateOnly="True" LocaleId="13322" CssClassTextBox="cDateBox" AutoPostBack="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style11"><b>Ejecutivo:</b></td>
                                                <td class="auto-style13">
                                                    <asp:Label ID="lblEjecutivo" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="col-md-3 col-sm-6">
                                        <table class="table table-bordered" style="font-size: small;">
                                            <tr>
                                                <td colspan="2"><b>Estado Línea</b></td>
                                                <td class="auto-style17" rowspan="3"></td>
                                                <td colspan="2"><b>Nivel Atribución</b></td>
                                            </tr>
                                            <tr>
                                                <td>Normal</td>
                                                <td>

                                                    <asp:RadioButton ID="rbNormal" runat="server" GroupName="estadolinea" />

                                                </td>
                                                <td>C.Riesgo</td>
                                                <td>
                                                    <asp:RadioButton ID="rbRiesgo" runat="server" GroupName="nivelatribucion" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Rev.Espe.</td>
                                                <td>
                                                    <asp:RadioButton ID="rbRevision" runat="server" GroupName="estadolinea" /></td>
                                                <td>C.Ejec.</td>
                                                <td>
                                                    <asp:RadioButton ID="rbEjecucion" runat="server" GroupName="nivelatribucion" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="col-md-3 col-sm-6">
                                        <table class="table table-bordered" style="font-size: small;">
                                            <tr>
                                                <td colspan="2"><b></b></td>
                                                <td class="auto-style17" rowspan="2"></td>
                                                <td colspan="2"><b>Nivel Atribución</b></td>
                                            </tr>
                                            <tr>
                                                <td>Rev.Espe.</td>
                                                <td>

                                                <td>C.Ejec.</td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </div>

                                    <div class="col-md-3 col-sm-6">
                                        <table class="table table-bordered" style="font-size: small;">
                                            <tr>
                                                <td colspan="2"><b>Clasificación Riesgo</b></td>
                                            </tr>
                                            <tr>
                                                <td><b>Rank</b></td>
                                                <td>
                                                    <asp:Label ID="lblRank" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td><b>Clasif.</b></td>
                                                <td>
                                                    <asp:Label ID="lblClasif" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Prov</b></td>
                                                <td>
                                                    <asp:Label ID="lblProf" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>F.Clasif.</b></td>
                                                <td>
                                                    <asp:Label ID="lblFClasif" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Ventas</b></td>
                                                <td>
                                                    <asp:Label ID="lblVentas" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12">

                                    <h4>I.Identificación del Cliente</h4>
                                    <asp:Panel ID="Panel1" runat="server" BorderStyle="None">
                                        <table style="font-size: small; width: 100%;">
                                            <tr>
                                                <td><b>Razón Social:</b></td>
                                                <td>
                                                    <asp:Label ID="lblNombre" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td><b>RUT:</b></td>
                                                <td>
                                                    <asp:Label ID="lblRut" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Dirección:</b></td>
                                                <td>
                                                    <asp:Label ID="lblDireccion" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td><b>Región:</b></td>
                                                <td>
                                                    <asp:Label ID="lblRegion" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Comuna:</b></td>
                                                <td>
                                                    <asp:Label ID="lblComuna" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td><b>Ciudad:</b></td>
                                                <td>
                                                    <asp:Label ID="lblCiudad" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Actividad:</b></td>
                                                <td>
                                                    <asp:Label ID="lblGiro" runat="server"></asp:Label>
                                                </td>
                                                <td></td>
                                                <td><b>Telefono:</b></td>
                                                <td>
                                                    <asp:Label ID="lblTelefono" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />

                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-12">

                                    <h4>II. Datos Financieros al
                                <asp:Label ID="lbFecha" runat="server" Text=""> día de Hoy</asp:Label></h4>

                                    <table class="table table-bordered table-hover">
                                        <tr>
                                            <td>VALOR DOLAR:
                                                <asp:TextBox ID="txtDOLAR" runat="server" ReadOnly="True" Width="96px" BorderStyle="None" Visible="false"></asp:TextBox>
                                                <asp:TextBox ID="txtDOLARV" runat="server" ReadOnly="True" Width="96px" BorderStyle="None"></asp:TextBox>
                                            </td>

                                            <td>VALOR UF: 
                                                <asp:TextBox ID="txtUF" runat="server" ReadOnly="True" Width="96px" BorderStyle="None" Visible="false"></asp:TextBox>
                                                <asp:TextBox ID="txtUFV" runat="server" ReadOnly="True" Width="96px" BorderStyle="None"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-12">

                                    <h4>III. Propuesta</h4>
                                    <asp:Panel ID="Panel2" runat="server" BorderStyle="None">
                                        <h4>III. Operaciones Vigentes 
                <asp:LinkButton ID="ActualizarOperacionVigentes" runat="server" CssClass="glyphicon glyphicon-refresh" OnClick="ActualizarOperacionVigentes_Click"></asp:LinkButton>
                                        </h4>

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="gridOperacionesVigentes" runat="server" GridLines="Horizontal" CssClass="table table-bordered table-hover"
                                                                        RowHeaderColumn="0" ShowFooter="True" AutoGenerateColumns="False" OnDataBound="gridOperacionesVigentes_DataBound" OnRowDataBound="gridOperacionesVigentes_RowDataBound" OnRowCreated="gridOperacionesVigentes_RowCreated">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="N" HeaderText="N" />
                                                                            <asp:BoundField DataField="Tipo Financiamiento" HeaderText="Tipo Fina" />
                                                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                                            <asp:BoundField DataField="Finalidad" HeaderText="Finalidad" />
                                                                            <asp:BoundField DataField="Plazo" HeaderText="Plazo" />
                                                                            <asp:BoundField DataField="Horizonte" HeaderText="Horizonte" />
                                                                            <asp:BoundField DataField="coberturaCertificado" HeaderText="Cobertura Certificado %" />
                                                                            <asp:BoundField DataField="Comisión Min. %" HeaderText="Comisión Min. %" />
                                                                            <asp:BoundField DataField="Seguro" HeaderText="Seguro" />
                                                                            <asp:BoundField DataField="Comisión" HeaderText="Comisión" />


                                                                            <asp:TemplateField HeaderStyle-Width="10%">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoCredito" runat="server" Text="Monto Crédito"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="txtMontoCredito" runat="server"></asp:Label>
                                                                                </ItemTemplate>

                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="Label1" runat="server" Text="Total CLP"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label2" runat="server" Text="Total USD"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label3" runat="server" Text="Total UF"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label4" runat="server" Text="Total Riesgo UF"></asp:Label>
                                                                                </FooterTemplate>

                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="descTipoM" runat="server" Text="Tipo MO"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbTipoMoneda" runat="server" Text=""></asp:Label>
                                                                                </ItemTemplate>

                                                                                <FooterStyle />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoAprobado" runat="server" Text="Monto Aprobado"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="txtMontoAprobado" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoCLP" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoUSD" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoUF" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoREUF" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoVigente" runat="server" Text="Monto Vigente (Aprobado - capital Pagado)"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="txtMontoVigente" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteCLP" runat="server" Width="100" MaxLength="18" Font-Size="Small" onKeyDown="return SoloLectura(event)" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteUSD" runat="server" Width="100" MaxLength="18" Font-Size="Small" onKeyDown="return SoloLectura(event)" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteUF" runat="server" Width="100" MaxLength="18" Font-Size="Small" onKeyDown="return SoloLectura(event)" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteREUF" runat="server" Width="100" MaxLength="18" Font-Size="Small" onKeyDown="return SoloLectura(event)" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoPropuesto" runat="server" Text="Monto Propuesto"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtMontoPropuesto" runat="server" Width="90" MaxLength="18" onKeyPress="return soloDecimales(event)" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoCLP" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoUSD" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoUF" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoREUF" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <div style="display: none">
                                                                                        <asp:TextBox ID="txtTotalMontoPropuestoVigente" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    </div>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="IdOperacion" HeaderText="IdOperacion" />

                                                                        </Columns>
                                                                        <HeaderStyle />
                                                                        <FooterStyle BorderStyle="None" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <h4>III. Operaciones Nuevas
                                        <asp:LinkButton ID="ActualizarOperacionesNuevas" runat="server" CssClass="glyphicon glyphicon-refresh" OnClick="ActualizarOperacionesNuevas_Click"></asp:LinkButton></h4>

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="gridOperacionesNuevas" runat="server" GridLines="Horizontal" CssClass="table table-bordered table-hover"
                                                                        RowHeaderColumn="0" ShowFooter="True" AutoGenerateColumns="False" OnRowDataBound="gridOperacionesNuevas_RowDataBound" OnDataBound="gridOperacionesNuevas_DataBound" OnRowCreated="gridOperacionesNuevas_RowCreated">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="N" HeaderText="N" />
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbFinanciamiento" runat="server" Width="80" Text="Tipo Fina"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTipoFinanciamiento" runat="server" Width="80" onKeyDown="return SoloLectura(event)" AutoPostBack="False" ViewStateMode="Enabled" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>


                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                                            <asp:BoundField DataField="Finalidad" HeaderText="Finalidad" />

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbPlazo" runat="server" Text="Plazo"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtPlazo" runat="server" Width="45" MaxLength="3" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbHorizonte" runat="server" Text="Horizonte"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtHorizonte" runat="server" Width="45" MaxLength="3" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbCobertura" runat="server" Text="Cobertura Certificado %"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtCobertura" runat="server" Width="45" MaxLength="3" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <%--<asp:TemplateField>
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="descTipoM" runat="server" Text="Tipo MO"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbTipoMoneda" runat="server" Text=""></asp:Label>
                                                                            </ItemTemplate>

                                                                            <FooterStyle />

                                                                        </asp:TemplateField>--%>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbComision" runat="server" Text="Comisión Min. %"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtComision" runat="server" Width="50" MaxLength="10" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbSeguro" runat="server" Text="Seguro"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtSeguro" runat="server" Width="80" MaxLength="16" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbComisionCLP" runat="server" Text="Comisión"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtComisionCLP" runat="server" Width="80" MaxLength="16" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderStyle-Width="10%">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoCredito" runat="server" Text="Monto Crédito"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtMontoCredito" runat="server" Width="80" MaxLength="16" onKeyPress="return solonumeros(event)" AutoPostBack="False" ViewStateMode="Enabled" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>

                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="Label1" runat="server" Text="Total CLP"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label2" runat="server" Text="Total USD"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label3" runat="server" Text="Total UF"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label4" runat="server" Text="Total Riesgo UF"></asp:Label>
                                                                                </FooterTemplate>

                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="descTipoM" runat="server" Text="Tipo MO"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbTipoMoneda" runat="server" Text=""></asp:Label>
                                                                                </ItemTemplate>

                                                                                <FooterStyle />

                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoAprobado" runat="server" Text="Monto Aprobado"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtMontoAprobado" runat="server" Width="90" MaxLength="18" onKeyPress="return soloDecimales(event)" CssClass="form-control input10px" ReadOnly="true"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoCLP" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoUSD" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoUF" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoAprobadoREUF" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                </FooterTemplate>

                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoVigente" runat="server" Text="Monto Vigente"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtMontoVigente" runat="server" Width="90" MaxLength="18" onKeyPress="return soloDecimales(event)" CssClass="form-control input10px" ReadOnly="true"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteCLP" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteUSD" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteUF" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoVigenteREUF" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbMontoPropuesto" runat="server" Text="Monto Propuesto"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtMontoPropuesto" runat="server" Width="90" MaxLength="18" onKeyPress="return soloDecimales(event)" CssClass="form-control input10px"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoCLP" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoUSD" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoUF" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtTotalMontoPropuestoREUF" runat="server" Width="90" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <div style="display: none">
                                                                                        <asp:TextBox ID="txtTotalMontoPropuestoNueva" runat="server" Width="100" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    </div>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="IdOperacion" HeaderText="IdOperacion" />

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="GaranElim" runat="server" CssClass="fa fa-close" CommandName="Eliminar" OnClientClick="return Dialogo();"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                        </Columns>
                                                                        <HeaderStyle />
                                                                        <FooterStyle BorderStyle="None" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>


                                        <h4>III. Total Exposición Ajustado a cobertura de certificado</h4>

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <table class="table table-bordered table-hover">
                                                                        <tr>
                                                                            <td></td>
                                                                            <td><b>Monto Aprobado</b></td>
                                                                            <td><b>Monto Vigente</b></td>
                                                                            <td><b>Monto Propuesto </b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><b>Total CLP </b></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoAprobadoCLP" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoVigenteCLP" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoPropuestoCLP" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><b>Total USD </b></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoAprobadoUDS" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoVigenteUDS" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoPropuestoUDS" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><b>Total UF </b></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoAprobadoUF" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoVigenteUF" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoPropuestoUF" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><b>Total Riesgo Equivalente UF </b></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoAprobadoREUF" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoVigenteREUF" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTotalMontoPropuestoREUF" runat="server" Width="150" MaxLength="18" ReadOnly="true" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                    </asp:Panel>


                                </div>

                            </div>


                            <div class="row">
                                <div class="col-md-12">
                                    <h4>IV. Resumen Valor de Garantías 
                            <asp:LinkButton ID="ActualizarGarantias" runat="server" CssClass="glyphicon glyphicon-refresh" OnClick="ActualizarGarantias_Click"></asp:LinkButton></h4>
                                    <asp:Panel ID="Panel4" runat="server" BorderStyle="None">

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="Garantias" CssClass="table table-bordered table-hover" Style="font-size: small;"
                                                                        runat="server" OnRowCreated="Garantias_RowCreated" ShowFooter="True"
                                                                        AutoGenerateColumns="False" OnDataBound="Garantias_DataBound">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="N" HeaderText="N" />
                                                                            <asp:BoundField DataField="Tipo Garantia" HeaderText="Tipo Garantia" />
                                                                            <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbco" runat="server" Text="Comentarios"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtComentario" runat="server" Width="250" Font-Size="Small" AutoPostBack="False" EnableViewState="True" ViewStateMode="Enabled"></asp:TextBox>
                                                                                </ItemTemplate>

                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="Label5" runat="server" Text="Total Garantía Real (UF)"></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label2" runat="server" Text="Total Cobertura Vigente "></asp:Label>
                                                                                    <br />
                                                                                    <asp:Label ID="Label3" runat="server" Text="Total Cobertura Global "></asp:Label>
                                                                                </FooterTemplate>

                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="descTipoM" runat="server" Text="Tipo MO"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbTipoMoneda" runat="server" Text=""></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterStyle />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbValorComercial" runat="server" Text="Valor Comercial"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="txtValorComercial" runat="server"></asp:Label>
                                                                                </ItemTemplate>

                                                                                <FooterTemplate>
                                                                                    <div style="display: none">
                                                                                        <asp:TextBox ID="txtTotalGarantiaComercial" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    </div>
                                                                                    <asp:TextBox ID="txtTotalGarantiaComercialUF" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <div class="input-group m-b">
                                                                                        <asp:TextBox ID="txtTotalCoberturaComercialVigente" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                        <span class="input-group-addon">%</span>
                                                                                    </div>
                                                                                    <div class="input-group m-b">
                                                                                        <asp:TextBox ID="txtTotalMCoberturaComercialGlobal" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                        <span class="input-group-addon">%</span>
                                                                                    </div>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lbValorAjustado" runat="server" Text="Valor Ajustado"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="txtValorAjustado" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <div style="display: none">
                                                                                        <asp:TextBox ID="txtTotalGarantiaAjustado" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                    </div>
                                                                                    <asp:TextBox ID="txtTotalGarantiaAjustadoUF" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>

                                                                                    <div class="input-group m-b">
                                                                                        <asp:TextBox ID="txtTotalCoberturaAjustadoVigente" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                        <span class="input-group-addon">%</span>
                                                                                    </div>
                                                                                    <div class="input-group m-b">
                                                                                        <asp:TextBox ID="txtTotalMCoberturaAjustadoGlobal" runat="server" MaxLength="18" onKeyDown="return SoloLectura(event)" Font-Size="Small" BorderColor="Transparent" BackColor="Transparent"></asp:TextBox>
                                                                                        <span class="input-group-addon">%</span>
                                                                                    </div>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="IdGarantia" />
                                                                            <asp:BoundField DataField="IdTipoBien" />
                                                                            <asp:BoundField DataField="DescEstado" HeaderText="Estado" />

                                                                            <asp:TemplateField HeaderStyle-Width="10%">
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="AplicaSeguro" runat="server" Text="Aplica Seguro"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <%--<asp:CheckBox ID="chkAplica" Text="Aplica" runat="server" ClientIDMode="Static" />
                                                                                    <br />
                                                                                    <asp:CheckBox ID="chkNoAplica" Text="No Aplica" runat="server" ClientIDMode="Static" />
                                                                                    <br />
                                                                                    <br />

                                                                                    <asp:CheckBoxList ID="chkSeguros" runat="server" Width="100%" ClientIDMode="Static">
                                                                                        <asp:ListItem>Aplica</asp:ListItem>
                                                                                        <asp:ListItem>No Aplica</asp:ListItem>
                                                                                    </asp:CheckBoxList>--%>

                                                                                    <asp:RadioButtonList ID="RbtSeguros" runat="server" CssClass="rbl" ClientIDMode="Static">
                                                                                        <asp:ListItem Selected="False">Aplica</asp:ListItem>
                                                                                        <asp:ListItem Selected="False">No Aplica</asp:ListItem>
                                                                                    </asp:RadioButtonList>

                                                                                </ItemTemplate>
                                                                                <FooterStyle />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Borrar Garantía">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="GaranElim" runat="server" CssClass="fa fa-close" CommandName="Eliminar" OnClientClick="return Dialogo();"></asp:LinkButton>
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
                                    <br />

                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-12">

                                    <h4>V. Observaciones Comité</h4>
                                    <div class="col-md-12 col-sm-12">
                                        <textarea id="txtObservacionesComite" cols="150" name="txtObservacionesComite" rows="3" runat="server" style="width: 100%"></textarea>
                                    </div>

                                    <br />
                                    <hr />
                                    <h4>VI. Aprobación Comité</h4>
                                    <asp:Panel ID="Panel5" runat="server" BorderStyle="None">
                                        <div class="col-md-2 col-sm-6">
                                            <asp:DropDownList ID="cbUsuarios1" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-6">
                                            <asp:DropDownList ID="cbUsuarios2" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-6">
                                            <asp:DropDownList ID="cbUsuarios3" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-6">
                                            <asp:DropDownList ID="cbUsuarios4" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-sm-6">
                                            <asp:DropDownList ID="cbUsuarios5" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>

                                    </asp:Panel>
                                    <br />

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12">

                                    <h4>VII. Indicadores Financieros</h4>
                                    <div id="dvEmpresa" runat="server" class="row clearfix" style="font-size: xx-small;">
                                        <div class="col-md-12 column">

                                            <!-- tabla / grilla -->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="card-box">
                                                                    <div class="table-responsive">
                                                                        <asp:GridView ID="Primero" runat="server" OnRowDataBound="Generico1_RowDataBound" CssClass="table table-bordered table-hover">
                                                                            <HeaderStyle BackColor="WhiteSmoke" HorizontalAlign="Center" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>

                                            <br />

                                            <!-- tabla / grilla -->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="card-box">
                                                                    <div class="table-responsive">
                                                                        <asp:GridView ID="Segundo" runat="server" OnRowDataBound="Generico1_RowDataBound" CssClass="table table-bordered table-hover">
                                                                            <HeaderStyle BackColor="WhiteSmoke" HorizontalAlign="Center" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <br />

                                            <!-- tabla / grilla -->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="card-box">
                                                                    <div class="table-responsive">
                                                                        <asp:GridView ID="Tercero" runat="server" OnRowDataBound="Generico1_RowDataBound" CssClass="table table-bordered table-hover">
                                                                            <HeaderStyle BackColor="WhiteSmoke" HorizontalAlign="Center" />
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
                                                <div class="col-md-12">
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="card-box">
                                                                    <div class="table-responsive">
                                                                        <asp:GridView ID="Cuarto" runat="server" CssClass="table table-bordered table-hover" OnRowDataBound="Generico2_RowDataBound">
                                                                            <HeaderStyle BackColor="WhiteSmoke" HorizontalAlign="Center" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>

                                            <br />
                                            <!-- tabla / grilla -->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="card-box">
                                                                    <div class="table-responsive">
                                                                        <asp:GridView ID="Quinto" runat="server" CssClass="table table-bordered table-hover" OnRowDataBound="Generico2_RowDataBound">
                                                                            <HeaderStyle BackColor="WhiteSmoke" HorizontalAlign="Center" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <br />

                                            <!-- tabla / grilla -->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <div class="card-box">
                                                                    <div class="table-responsive">
                                                                        <asp:GridView ID="Sexto" runat="server" CssClass="table table-bordered table-hover" OnRowDataBound="Generico2_RowDataBound">
                                                                            <HeaderStyle BackColor="WhiteSmoke" HorizontalAlign="Center" />
                                                                        </asp:GridView>
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

                            <div class="row">
                                <div class="col-md-12">

                                    <h4>VIII. Datos Evaluación</h4>
                                    <asp:Panel ID="Panel6" runat="server" BorderStyle="None">

                                        <div class="col-md-12 col-sm-12">
                                            <div class="form-group">
                                                <asp:Label ID="lbCapacidadDePago" CssClass="col-sm-3 control-label" runat="server" Text="Capacidad de Pago" Font-Bold="True"></asp:Label>
                                                <textarea rows="5" runat="server" cols="20" id="txtCapacidadPago" class="form-control"></textarea>
                                            </div>
                                        </div>
                                        <br />

                                        <div class="col-md-12 col-sm-12">
                                            <div class="form-group">
                                                <asp:Label ID="lbEvaluacionRiesgo" CssClass="col-sm-3 control-label" runat="server" Text="Evaluación de Riesgo" Font-Bold="True"></asp:Label>
                                                <textarea rows="5" runat="server" cols="20" id="txtEvaluacionRiesgo" class="form-control"></textarea>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                </div>
                            </div>

                        </div>
                        <div id="DivArchivo" style="display: none;">
                            <table style="width: 100%;">
                                <tr>
                                    <td>Documento:</td>
                                    <td>
                                        <asp:FileUpload ID="fileDocumento" runat="server" CssClass="form-control" />
                                        <span style="color: red;" id="errorArchivo"></span>
                                        <script>
                                            var validFilesTypes = ["pdf"];

                                            function CheckExtension(file) {
                                                /*global document: false */
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
                                    </td>

                                    <td class="auto-style33"></td>
                                    <td class="auto-style34">RESOLUCIÓN:</td>
                                    <td>
                                        <asp:DropDownList ID="cbEstado" runat="server" CssClass="form-control input-sm">
                                            <asp:ListItem>--Select--</asp:ListItem>
                                            <asp:ListItem>Aprobado</asp:ListItem>
                                            <asp:ListItem>Aprobado con Reparo</asp:ListItem>
                                            <asp:ListItem>Rechazado</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style34">FECHA:</td>
                                    <td class="auto-style34">
                                        <SharePoint:DateTimeControl ID="dtcFecResolucion" runat="server" DateOnly="True" CssClassTextBox="form-control input-sm col-md-3" />
                                    </td>

                                    <td class="auto-style35"></td>
                                    <td>
                                        <asp:Button CssClass="btn  btn-success pull-right" ID="btnAdjuntarFinal" runat="server" Text="Adjuntar" OnClick="btnAdjuntar_Click" OnClientClick="return Dialogo();" /></td>
                                </tr>
                            </table>


                            <br />
                        </div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                <div id="Botones">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                                            </td>
                                            <td>
                                                <%--<asp:Button CssClass="btn  btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="if(retrieveListItems()) return Dialogo(); else return Dialogo();" OnClick="btnGuardar_Click2" />--%>
                                                <asp:Button CssClass="btn  btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="return Dialogo()" OnClick="btnGuardar_Click2" />
                                                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir PAF 1.0" CssClass="btn btn-default pull-right" OnClick="btnImprimir_Click" OnClientClick="return Dialogo();" />

                                                <asp:Button ID="btnAdjuntar" runat="server" Text="Adjuntar" CssClass="btn btn-default pull-right" OnClientClick="return Adjuntar('imprimir');" />

                                                <asp:Button ID="btnImprimirPAF2" runat="server" CssClass="btn btn-default pull-right" Text="Imprimir PAF 2.0" OnClick="btnImprimirPAF2_Click" OnClientClick="return Dialogo();" Visible="false" />

                                                <asp:Button ID="btnImprimirPAF2Express" runat="server" CssClass="btn btn-default pull-right" Text="Imprimir PAF 2.0 Express" OnClick="btnImprimirPAF2Express_Click" OnClientClick="return Dialogo();" />

                                                <asp:Button ID="btnCalcular" runat="server" CssClass="btn btn-primary pull-right" Text="Calcular" OnClick="btnCalcular_Click" OnClientClick="return Dialogo();" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-primary pull-right" OnClientClick="return Dialogo();" OnClick="btnCancelar_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lbFecImprimirPAF2" CssClass="pull-right" Style="padding-top: 10px;" runat="server" Text="Fecha"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>

        </div>
    </asp:Panel>

    <%--    <asp:HiddenField ID="GarComercialVigente" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="GarComercialVigenteNueva" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="GarAjustadoVigente" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="GarAjustadoVigenteNueva" runat="server" ClientIDMode="Static" />--%>

    <asp:HiddenField ID="OpeVigente" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="OpeVigenteAprobado" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="OpeVigenteVigente" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="OpeVigentePropuesto" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="OpeNueva" runat="server" ClientIDMode="Static" Value="0" />

</div>
