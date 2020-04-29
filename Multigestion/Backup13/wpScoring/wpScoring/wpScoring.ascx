<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpScoring.ascx.cs" Inherits="MultiRiesgo.wpScoring.wpScoring.wpScoring" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript" src="../../_layouts/15/MultiRiesgo/wpVaciado.js"></script>
<script type="text/javascript" src="../../_layouts/15/MultiRiesgo/jquery-1.8.3.min.js"></script>

<style type="text/css">
    .btn {
        font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important;
    }

    .btnInline {
        display: inline-table;
    }

    .btnFloat {
        float: right;
    }
</style>

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
        lbPtjeLetra.GetInputElement().readOnly = false;
        lbPuntaje.GetInputElement().readOnly = false;
    });

    function OnCallbackComplete(s, e) {
        var valores = e.result.split('~');
        if (valores[0] === 'comportamiento') {
            lbPondMorosidad.SetText(parseFloat(valores[1]).toFixed(3));
            //alert(valores[1]);
        }
        if (valores[0] === 'monto') {
            lbPondMonto.SetText(parseFloat(valores[1]).toFixed(3));
        }
        if (valores[0] === "actividad") {
            //alert(valores[1]);
            lbPtjeActividad.SetText(parseFloat((valores[1] / 100) * lbPondActividad.GetText()).toFixed(3));
            //checkDecimals(
            //.replace(/\./g, '')
        }
        if (valores[0] === 'antiguedad') {
            lbPtjeExperiencia.SetText(parseFloat((valores[1] / 100) * lbPondExperiencia.GetText()).toFixed(3));
        }
        if (valores[0] === 'concentracion') {
            lbPtjeConcentracion.SetText(parseFloat((valores[1] / 100) * lbPondConcentracion.GetText()).toFixed(3));
        }
        if (valores[0] === 'competencia') {
            lbPtjeCompetencia.SetText(parseFloat((valores[1] / 100) * lbPondCompetencia.GetText()).toFixed(3));
        }
        if (valores[0] === 'PasivoExigible/Ebitda') {
            lbPtjePasivo.SetText(parseFloat((valores[1] / 100) * lbPondPasivo.GetText()).toFixed(3));
        }
        if (valores[0] === 'pasivovts') {
            lbPtjePasivoVts.SetText(parseFloat((valores[1] / 100) * lbPondPasivoVts.GetText()).toFixed(3));
        }
        if (valores[0] === 'Ebitda/GtosFinancieros') {
            lbPtjeEbitda.SetText(parseFloat((valores[1] / 100) * lbPondEbitda.GetText()).toFixed(3));
        }
        if (valores[0] === 'liquidez') {
            lbPtjeLiquidez.SetText(parseFloat((valores[1] / 100) * lbPondLiquidez.GetText()).toFixed(3));
        }
        if (valores[0] === 'balance') {
            lbPtjeCalidadBalance.SetText(parseFloat((valores[1] / 100) * lbPondCalidadBalance.GetText()).toFixed(3));
        }
        if (valores[0] === 'leverage') {
            lbPtjeLeverage.SetText(parseFloat((valores[1] / 100) * lbPondLeverage.GetText()).toFixed(3));
        }
        if (valores[0] === 'tipogarantia') {
            lbPtjeTipoGarantia.SetText(parseFloat((valores[1] / 100) * lbPondTipoGarantia.GetText()).toFixed(3));
        }
        if (valores[0] === 'calidadgarantia') {
            lbPtjeCalidadGarantia.SetText(parseFloat((valores[1] / 100) * lbPondCalidadGarantia.GetText()).toFixed(3));
        }
        if (valores[0] === 'coberturagarantia') {
            lbPtjeCoberturaGarantia.SetText(parseFloat((valores[1] / 100) * lbPondCoberturaGarantia.GetText()).toFixed(3));
        }

        sumarPtje();
    }

    var count = 0;
    function sumarPtje() {
        var ban = 0;
        var lbsumatoria = 0;
        var a = lbPtjeCalidadBalance.GetValue();

        if (lbPtjeCalidadBalance.GetValue() != 0 && checkDecimals(lbPtjeCalidadBalance.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeCalidadBalance.GetValue());
        }
        else {
            ban = 1;
            cbCalidadBalance.SetSelectedIndex(-1);
            lbPtjeCalidadBalance.SetText(0);
        }

        if (lbPtjeCalidadGarantia.GetValue() != 0 && checkDecimals(lbPtjeCalidadGarantia.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeCalidadGarantia.GetValue());
        }
        else {
            ban = 1;
            cbCalidadGarantia.SetSelectedIndex(-1);
            lbPtjeCalidadGarantia.SetText(0);
        }

        if (lbPtjeCoberturaGarantia.GetValue() != 0 && checkDecimals(lbPtjeCoberturaGarantia.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeCoberturaGarantia.GetValue());
        }
        else {
            ban = 1;
            cbCoberturaGarantia.SetSelectedIndex(-1);
            lbPtjeCoberturaGarantia.SetText(0);
        }

        if (lbPtjeConcentracion.GetValue() != 0 && checkDecimals(lbPtjeConcentracion.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeConcentracion.GetValue());
        }
        else {
            ban = 1;
            cbConcentracion.SetSelectedIndex(-1);
            lbPtjeConcentracion.SetText(0);
        }

        if (lbPtjeCompetencia.GetValue() != 0 && checkDecimals(lbPtjeCompetencia.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeCompetencia.GetValue());
        }
        else {
            ban = 1;
            cbCompetencia.SetSelectedIndex(-1);
            lbPtjeCompetencia.SetText(0);
        }

        if (lbPtjeEbitda.GetValue() != 0 && checkDecimals(lbPtjeEbitda.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeEbitda.GetValue());
        }
        else {
            ban = 1;
            cbEbitda.SetSelectedIndex(-1);
            lbPtjeEbitda.SetText(0);
        }

        if (lbPtjeExperiencia.GetValue() != 0 && checkDecimals(lbPtjeExperiencia.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeExperiencia.GetValue());
        }
        else {
            ban = 1;
            cbExperiencia.SetSelectedIndex(-1);
            lbPtjeExperiencia.SetText(0);
        }

        if (lbPtjeLeverage.GetValue() != 0 && checkDecimals(lbPtjeLeverage.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeLeverage.GetValue());
        }
        else {
            ban = 1;
            cbLeverage.SetSelectedIndex(-1);
            lbPtjeLeverage.SetText(0)
        }

        if (lbPtjeLiquidez.GetValue() != 0 && checkDecimals(lbPtjeLiquidez.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeLiquidez.GetValue());
        }
        else {
            ban = 1;
            cbLiquidez.SetSelectedIndex(-1);
            lbPtjeLiquidez.SetText(0);
        }

        if (lbPtjePasivo.GetValue() != 0 && checkDecimals(lbPtjePasivo.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjePasivo.GetValue());
        }
        else {
            ban = 1;
            cbPasivo.SetSelectedIndex(-1);
            lbPtjePasivo.SetText(0);
        }

        if (lbPtjePasivoVts.GetValue() != 0 && checkDecimals(lbPtjePasivoVts.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjePasivoVts.GetValue());
        }
        else {
            ban = 1;
            cbPasivoVts.SetSelectedIndex(-1);
            lbPtjePasivoVts.SetText(0);
        }

        if (lbPtjeTipoGarantia.GetValue() != 0 && checkDecimals(lbPtjeTipoGarantia.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeTipoGarantia.GetValue());
        }
        else {
            ban = 1;
            cbTipoGarantia.SetSelectedIndex(-1);
            lbPtjeTipoGarantia.SetText(0);
        }

        if (lbPtjeActividad.GetValue() != 0 && checkDecimals(lbPtjeActividad.GetValue())) {
            lbsumatoria = parseFloat(lbsumatoria) + parseFloat(lbPtjeActividad.GetValue());
        }
        else {
            ban = 1;
            cbActividad.SetSelectedIndex(-1);
            lbPtjeActividad.SetText(0);
        }

        lbSumatoria.SetText(parseFloat(lbsumatoria).toFixed(3));
        if (ban == 0) {
            count = 0;
            sumarPtjeTotal();
        }
    }

    var indCaso;
    var maxPond;

    function sumarPtjeTotal() {
        if (cbMorosidad.GetText() == 'Sin Protestos ni Morosidad') {
            indCaso = 0;
            Total();
        }
        else {
            var a = lbPondMorosidad.GetValue();
            var b = lbPondMonto.GetValue();
            if (lbPondMorosidad.GetValue() > lbPondMonto.GetValue()) {
                maxPond = lbPondMorosidad.GetValue();
            }
            else {
                maxPond = lbPondMonto.GetValue();
            }
            indCaso = 1;
            Total();
        }
    }

    function Total() {

        if (indCaso == 0) {
            lbPtjeLetra.GetInputElement().readOnly = false;
            var item = "indCaso0";
            var valor = parseInt(lbSumatoria.GetValue());
            var parametros = new Array(item, valor);
            cbpGuardarScoring.PerformCallback(parametros);
        }
        if (indCaso == 1) {
            lbPuntaje.GetInputElement().readOnly = false;
            var item = "indCaso1";
            var valor = maxPond;
            var parametros = new Array(item, valor);
            cbpGuardarScoring.PerformCallback(parametros);
        }
        if (indCaso == 2) {
            lbPuntaje.GetInputElement().readOnly = false;
            var item = "indCaso2";
            var valor = lbPtjeLetra.GetValue();
            var parametros = new Array(item, valor);
            cbpGuardarScoring.PerformCallback(parametros);
        }
        if (indCaso == 3) {
            lbPtjeLetra.GetInputElement().readOnly = false;
            var item = "indCaso3";
            var valor = parseInt(lbSumatoria.GetValue());
            var parametros = new Array(item, valor);
            cbpGuardarScoring.PerformCallback(parametros);
        }
        count++;
    }

    function OnCallbackCompleteScoring(s, e) {
        var valores = e.result.split('~');

        if (valores[0] == 'indCaso1') {
            //if (valores[1].indexOf("Error") != -1)
            lbPtjeLetra.SetText(valores[1]);
            lbPtjeLetra.GetInputElement().readOnly = true;
            //    alert(valores[1]);
            //else
            //    lbPtjeLetra.SetText(valores[1]);
        }
        if (valores[0] == 'indCaso2') {
            //if (valores[1].indexOf("Error") != -1)
            lbPuntaje.SetText(valores[1]);
            lbPuntaje.GetInputElement().readOnly = true;
            //    alert(valores[1]);
            //else
            //    lbPuntaje.SetText(valores[1]);
        }
        if (valores[0] == 'indCaso3') {
            //if (valores[1].indexOf("Error") != -1)
            lbPuntaje.SetText(valores[1]);
            lbPuntaje.GetInputElement().readOnly = true;
            //alert(valores[1]);
            //else
            //     lbPuntaje.SetText(valores[1]);
        }
        if (valores[0] == 'indCaso0') {
            //if (valores[1].indexOf("Error") != -1)
            lbPtjeLetra.SetText(valores[1]);
            lbPtjeLetra.GetInputElement().readOnly = true;
            //    alert(valores[1]);
            //else
            //    lbPtjeLetra.SetText(valores[1]);
            //var d = oListItem.get_item('Descripcion') + '/' + oListItem.get_item('Valor');
            //lbPtjeLetra.SetText(oListItem.get_item('Descripcion') + '/' + oListItem.get_item('Valor'));
        }

        if (count < 3) {
            if (lbPtjeLetra.GetValue().indexOf('C') != -1 || lbPtjeLetra.GetValue().indexOf('C+')) {
                indCaso = 2;
                Total();
            }
            else {
                indCaso = 3;
                Total();
            }
        }
        else if ((count == 3) && (lbPtjeLetra.GetText() == 'no califica/0' || lbPtjeLetra.GetText() == '')) {
            lbPtjeLetra.SetText('C/A6');
            lbPuntaje.SetText('0.000');
        }
        CloseDlg();
    }


    function OnCallbackFinalizarScoring(s, e) {
        if (e.result !== null)
            alert(e.result);
        setTimeout(function () { CloseDlg(); }, 2000);
    }

    function checkDecimals(fieldValue) {
        decallowed = 3;
        if (typeof (fieldValue) === "undefined" || isNaN(fieldValue) || fieldValue == "" || fieldValue == null || fieldValue == " ") {
            return false;
        }
        else {
            if (fieldValue.indexOf('.') == -1) fieldValue += ".";
            dectext = fieldValue.substring(fieldValue.indexOf('.') + 1, fieldValue.length);
            if (dectext.length > decallowed) {
                return false;
            }
            else {
                return true;
            }
        }
    }

    function OnCambioMorosidad(s, e) {
        var key = 'comportamiento';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioMonto(s, e) {
        var key = 'monto';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioActividad(s, e) {
        var key = 'actividad';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioExperiencia(s, e) {
        var key = 'antiguedad';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioConcentracion(s, e) {
        var key = 'concentracion';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioCompetencia(s, e) {
        var key = 'competencia';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioPasivo(s, e) {
        var key = 'PasivoExigible/Ebitda';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioPasivoVts(s, e) {
        var key = 'pasivovts';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioEbitda(s, e) {
        var key = 'Ebitda/GtosFinancieros';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioLiquidez(s, e) {
        var key = 'liquidez';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioCalidadBalance(s, e) {
        var key = 'balance';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioLeverage(s, e) {
        var key = 'leverage';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioTipoGarantia(s, e) {
        var key = 'tipogarantia';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioCalidadGarantia(s, e) {
        var key = 'calidadgarantia';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function OnCambioCoberturaGaraantia(s, e) {
        var key = 'coberturagarantia';
        var value = s.GetSelectedItem().value.toString();
        var parametros = new Array(key, value);
        cbpScoring.PerformCallback(parametros);
        return true;
    }

    function finalizarScoring(s, e) {
        //if (window.ASPxClientEdit.ValidateGroup('scoring')) {
            cbpFinalizarScoring.PerformCallback('finalizar');
            OpenDlg();
        //}
        //else {
        //    alert("Debe Completar Los Datos Obligatorios");
        //}
    }

    function guardarScoring(s, e) {
        //try {
        //    var divValidar = document.getElementById("divValidar");
        //    var validar = true;
        //        // recorremos todos los campos que tiene el formulario
        //    $(':input', divValidar).each(function () {
        //        var type = this.type;
        //        var tag = this.tagName.toLowerCase();
        //        var value = this.value;
        //        //var texto = this.text;

        //        if (tag == 'select') {
        //            if (value <= 0) {
        //                this.style.borderColor = 'red';
        //                validar = false;
        //            }
        //            else
        //                this.style.borderColor = '';
        //        }
        //        else if (vacio(value)) {
        //            this.style.borderColor = 'red';
        //            validar = false;
        //        }
        //        else
        //            this.style.borderColor = '';
        //    });

        //    if (!validar)
        //        return false;
        //    else
        //        return true;
        //}
        //catch (e) {
        //    alert(e);
        //}


        if (window.ASPxClientEdit.ValidateGroup('scoring')) {
            cbpFinalizarScoring.PerformCallback('guardar');
            OpenDlg();
        }
        else {
            alert("Debe Completar Los Datos Obligatorios");
        }
    }

    function imprimirScoring(s, e) {
        if (window.ASPxClientEdit.ValidateGroup('scoring')) {
            cbpFinalizarScoring.PerformCallback('imprimir');            
        }
        else {
            alert("Debe Completar Los Datos Obligatorios");
        }
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
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
                <li class="active">Scoring
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

    <asp:LinkButton ID="lbEditar" runat="server" CssClass="fa fa-edit paddingIconos pull-right" ToolTip="Editar"> Editar</asp:LinkButton>

    <!-- Contenedor Tabs -->
    <div class="row" id="divValidar">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li class="">
                    <a href="#tab1" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbBalance" runat="server" OnClientClick="return Dialogo();" OnClick="lbBalance_Click">Balance</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li class="">
                    <a href="#tab2" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbEdoResultado" runat="server" OnClientClick="return Dialogo();" OnClick="lbEdoResultado_Click">Estado de Resultado</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li class="">
                    <a href="#tab3" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbVentas" runat="server" OnClientClick="return Dialogo();" OnClick="lbVentas_Click">IVA Ventas</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li class="">
                    <a href="#tab4" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbCompras" runat="server" OnClientClick="return Dialogo();" OnClick="lbCompras_Click">IVA Compras</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li class="active">
                    <a href="#tab5" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbScoring" runat="server" OnClientClick="return Dialogo();" OnClick="lbScoring_Click">Scoring</asp:LinkButton>
                        </span>
                    </a>
                </li>

                <li>
                    <a href="#tab6" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbPaf" runat="server" OnClientClick="return Dialogo();" OnClick="lbPaf_Click">PAF</asp:LinkButton>
                        </span>
                    </a>
                </li>

            </ul>

            <div class="tab-content">
                <div class="tab-pane active" id="tab5">
                    <asp:Panel ID="pnFormulario" runat="server">

                        <div class="row">
                            <div class="col-md-12">
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


                                <!--col 1-->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <h4>
                                            <label for="" class="">Factores</label>
                                        </h4>
                                    </div>
                                </div>

                                <!--col 2-->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <h4>
                                            <label for="" class="">Información</label>
                                        </h4>
                                    </div>
                                </div>

                                <!--col 3-->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <h4>
                                            <label for="" class="">Ptje.</label>
                                        </h4>
                                    </div>
                                </div>

                                <!--col 4-->
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <h4>
                                            <label for="" class="">Pond. (%)</label>
                                        </h4>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="card-box">

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <h4>
                                            <label for="" class="control-label" style="background-color: #D5ECEB">1. Comportamiento</label>
                                        </h4>
                                    </div>
                                </div>

                                <div class="form-group col-lg-3">
                                    <label>Protestos y Morosidad Vigente</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbMorosidad" runat="server" ClientInstanceName="cbMorosidad" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioMorosidad" />
                                    </dx:ASPxComboBox>
                                    <%-- <asp:DropDownList ID="cbMorosidad" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeMorosidad" runat="server" ReadOnly="true" ClientInstanceName="lbPtjeMorosidad" CssClass="form-control" Style="border: none" Text="0"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeMorosidad" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="form-control" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondMorosidad" runat="server" ClientInstanceName="lbPondMorosidad" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondMorosidad" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="form-control" BackColor="Transparent"></asp:TextBox>--%>
                                </div>





                                <div class="form-group col-lg-3">
                                    <label>Monto</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbMonto" runat="server" ClientInstanceName="cbMonto" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioMonto" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbMonto" runat="server" CssClass="form-control"></asp:DropDownList> --%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeMonto" runat="server" ReadOnly="true" ClientInstanceName="lbPtjeMonto" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeMonto" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondMonto" runat="server" ClientInstanceName="lbPondMonto" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondMonto" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>





                                <div class="form-group col-lg-3">
                                    <label>Sector / Actividad</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbActividad" runat="server" ClientInstanceName="cbActividad" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioActividad" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbActividad" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeActividad" runat="server" ClientInstanceName="lbPtjeActividad" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeActividad" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" ViewStateMode="Disabled" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondActividad" runat="server" Style="border: none" ClientInstanceName="lbPondActividad" CssClass="form-control" Text="3.5"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondActividad" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="3.5" CssClass="control-label" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="card-box">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <h4>
                                            <label for="inputEmail3" class="control-label" style="background-color: #D5ECEB">2. Simulación Capacidad de pago</label>
                                        </h4>
                                    </div>
                                </div>

                                <div class="form-group col-lg-12">
                                    <label>Calidad Propiedad y Administración</label>
                                </div>


                                <div class="form-group col-lg-3">
                                    <label>a) Experiencia de la Administración</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbExperiencia" runat="server" ClientInstanceName="cbExperiencia" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioExperiencia" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbExperiencia" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeExperiencia" runat="server" ClientInstanceName="lbPtjeExperiencia" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeExperiencia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondExperiencia" runat="server" ClientInstanceName="lbPondExperiencia" CssClass="form-control" Style="border: none" Text="3.5"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondExperiencia" runat="server" Style="border: none" Text="3.5" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>


                                <div class="form-group col-lg-3">
                                    <label>b) Empresa Familiar</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbFamiliar" runat="server" ClientInstanceName="cbFamiliar" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <Items>
                                            <dx:ListEditItem Value="1" Text="Si" />
                                            <dx:ListEditItem Value="0" Text="No" />
                                        </Items>
                                    </dx:ASPxComboBox>

                                    <%--<asp:DropDownList ID="cbFamiliar" runat="server" CssClass="form-control" OnSelectedIndexChanged="cbFamiliar_SelectedIndexChanged">
                                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                </asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeFamiliar" runat="server" ClientInstanceName="lbPtjeFamiliar" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeFamiliar" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondFamiliar" runat="server" ClientInstanceName="lbPondFamiliar" CssClass="form-control" Style="border: none" Text="0.0"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondFamiliar" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="0.0" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>


                            </div>
                        </div>


                        <div class="row">
                            <div class="card-box">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Situación del Mercado</label>
                                    </div>
                                </div>


                                <div class="form-group col-lg-3">
                                    <label>c) Grado Concentración del Cliente</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbConcentracion" runat="server" ClientInstanceName="cbConcentracion" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioConcentracion" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbConcentracion" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeConcentracion" runat="server" ClientInstanceName="lbPtjeConcentracion" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeConcentracion" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondConcentracion" runat="server" ClientInstanceName="lbPondConcentracion" CssClass="form-control" Style="border: none" Text="6.5"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondConcentracion" runat="server" Style="border: none" Text="6.5" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>




                                <div class="form-group col-lg-3">
                                    <label>d) Grado de Competencia del Mercado</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbCompetencia" runat="server" ClientInstanceName="cbCompetencia" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioCompetencia" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbCompetencia" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeCompetencia" runat="server" ClientInstanceName="lbPtjeCompetencia" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeCompetencia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondCompetencia" runat="server" ClientInstanceName="lbPondCompetencia" CssClass="form-control" Style="border: none" Text="3.5"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondCompetencia" runat="server" Style="border: none" Text="3.5" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="card-box">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Resultados</label>
                                    </div>
                                </div>


                                <div class="form-group col-lg-3">
                                    <label>e) Pasivo Exigible / Generación Bruta</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbPasivo" runat="server" ClientInstanceName="cbPasivo" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioPasivo" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbPasivo" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPtjePasivo" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPtjePasivo" runat="server" ClientInstanceName="lbPtjePasivo" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPondPasivo" runat="server" Style="border: none" Text="15.0" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPondPasivo" runat="server" ClientInstanceName="lbPondPasivo" CssClass="form-control" Style="border: none" Text="15.0"></dx:ASPxTextBox>
                                </div>



                                <div class="form-group col-lg-3">
                                    <label>f) Pasivo Circulante / Venta Promedio Mes</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbPasivoVts" runat="server" ClientInstanceName="cbPasivoVts" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioPasivoVts" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbPasivoVts" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPtjePasivoVts" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPtjePasivoVts" runat="server" ClientInstanceName="lbPtjePasivoVts" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPondPasivoVts" runat="server" Style="border: none" Text="9.0" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPondPasivoVts" runat="server" ClientInstanceName="lbPondPasivoVts" CssClass="form-control" Style="border: none" Text="9.0"></dx:ASPxTextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="card-box">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Flujo de caja</label>
                                    </div>
                                </div>


                                <div class="form-group col-lg-3">
                                    <label>g) Ebitda/Gastos Financieros</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbEbitda" runat="server" ClientInstanceName="cbEbitda" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioEbitda" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbEbitda" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeEbitda" runat="server" ClientInstanceName="lbPtjeEbitda" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeEbitda" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondEbitda" runat="server" ClientInstanceName="lbPondEbitda" CssClass="form-control" Style="border: none" Text="6.0"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondEbitda" runat="server" Style="border: none" Text="6.0" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>



                                <div class="form-group col-lg-3">
                                    <label>h) Razón Corriente</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbLiquidez" runat="server" ClientInstanceName="cbLiquidez" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioLiquidez" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbLiquidez" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeLiquidez" runat="server" ClientInstanceName="lbPtjeLiquidez" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeLiquidez" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondLiquidez" runat="server" ClientInstanceName="lbPondLiquidez" CssClass="form-control" Style="border: none" Text="8.0"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondLiquidez" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="8.0" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="card-box">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <h4>
                                            <label for="inputEmail3" class="control-label" style="background-color: #D5ECEB">3. Situación Patrimonial</label>
                                        </h4>
                                    </div>
                                </div>


                                <div class="col-md-12">
                                    <div class="form-group col-lg-3">
                                        <label>Calidad de la Información</label>
                                    </div>
                                </div>

                                <div class="form-group col-lg-3">
                                    <label>i) Calidad de los Balances</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbCalidadBalance" runat="server" ClientInstanceName="cbCalidadBalance" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioCalidadBalance" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbCalidadBalance" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeCalidadBalance" runat="server" ClientInstanceName="lbPtjeCalidadBalance" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeCalidadBalance" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondCalidadBalance" runat="server" ClientInstanceName="lbPondCalidadBalance" CssClass="form-control" Style="border: none" Text="7.0"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondCalidadBalance" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="7.0" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="card-box">

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Nivel de Endeudamiento</label>
                                    </div>
                                </div>

                                <div class="form-group col-lg-3">
                                    <label>j) Leverage</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbLeverage" runat="server" ClientInstanceName="cbLeverage" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioLeverage" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbLeverage" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeLeverage" runat="server" ClientInstanceName="lbPtjeLeverage" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeLeverage" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPondLeverage" runat="server" ClientInstanceName="lbPondLeverage" CssClass="form-control" Style="border: none" Text="15.0"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPondLeverage" runat="server" Style="border: none" Text="15.0" onkeydown="return SoloLectura(event)" CssClass="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="card-box">

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <h4>
                                            <label for="inputEmail3" class="control-label" style="background-color: #D5ECEB">4. Segunda Fuente de Pago</label>
                                        </h4>
                                    </div>
                                </div>


                                <div class="form-group col-lg-3">
                                    <label>k) Tipo de Garantía</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbTipoGarantia" runat="server" ClientInstanceName="cbTipoGarantia" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioTipoGarantia" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbTipoGarantia" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeTipoGarantia" runat="server" ClientInstanceName="lbPtjeTipoGarantia" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeTipoGarantia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPondTipoGarantia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="10.0" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPondTipoGarantia" runat="server" ClientInstanceName="lbPondTipoGarantia" CssClass="form-control" Style="border: none" Text="10.0"></dx:ASPxTextBox>
                                </div>



                                <div class="form-group col-lg-3">
                                    <label>l) Calidad de la Garantía</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:DropDownList ID="cbCalidadGarantia" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                    <dx:ASPxComboBox ID="cbCalidadGarantia" runat="server" ClientInstanceName="cbCalidadGarantia" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioCalidadGarantia" />
                                    </dx:ASPxComboBox>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxTextBox ID="lbPtjeCalidadGarantia" runat="server" ClientInstanceName="lbPtjeCalidadGarantia" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeCalidadGarantia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPondCalidadGarantia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="8.0" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPondCalidadGarantia" runat="server" ClientInstanceName="lbPondCalidadGarantia" CssClass="form-control" Style="border: none" Text="8.0"></dx:ASPxTextBox>
                                </div>



                                <div class="form-group col-lg-3">
                                    <label>m) Cobertura de la Garantía</label>
                                </div>

                                <div class="form-group col-lg-3">
                                    <dx:ASPxComboBox ID="cbCoberturaGarantia" runat="server" ClientInstanceName="cbCoberturaGarantia" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" NullText="--Seleccione--">
                                        <ClientSideEvents SelectedIndexChanged="OnCambioCoberturaGaraantia" />
                                    </dx:ASPxComboBox>
                                    <%--<asp:DropDownList ID="cbCoberturaGarantia" runat="server" CssClass="form-control"></asp:DropDownList>--%>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPtjeCoberturaGarantia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPtjeCoberturaGarantia" runat="server" ClientInstanceName="lbPtjeCoberturaGarantia" CssClass="form-control" Style="border: none"></dx:ASPxTextBox>
                                </div>

                                <div class="form-group col-lg-3">
                                    <%--<asp:TextBox ID="lbPondCoberturaGarantia" runat="server" Style="border: none" onkeydown="return SoloLectura(event)" Text="5.0" class="control-label" BackColor="Transparent"></asp:TextBox>--%>
                                    <dx:ASPxTextBox ID="lbPondCoberturaGarantia" runat="server" ClientInstanceName="lbPondCoberturaGarantia" CssClass="form-control" Style="border: none" Text="5.0"></dx:ASPxTextBox>
                                </div>

                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-6 col-sm-6">
                                <div class="form-group">
                                    <h4>
                                        <label>Puntaje Total</label>
                                    </h4>
                                </div>

                                <div class="form-group">
                                    <dx:ASPxTextBox ID="lbPtjeLetra" runat="server" ClientInstanceName="lbPtjeLetra" CssClass="form-control" Style="border: none" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-ErrorText="se deben seleccionar todas las variables del scoring" ClientIDMode="Static"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPtjeLetra" runat="server" ReadOnly="true" CssClass="control-label" Style="font-size: 20px;"></asp:TextBox>--%>
                                    <%--border: none; BackColor="Transparent"--%>
                                </div>

                                <div class="form-group">
                                    <dx:ASPxTextBox ID="lbPuntaje" runat="server" ClientInstanceName="lbPuntaje" CssClass="form-control" Style="border: none" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" ClientIDMode="Static"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbPuntaje" runat="server" Text="0%" ReadOnly="true" CssClass="control-label" onkeydown="return SoloLectura(event)" Style="font-size: 20px;"></asp:TextBox>--%>
                                </div>

                                <div class="form-group">
                                    <dx:ASPxTextBox ID="lbSumatoria" runat="server" ClientInstanceName="lbSumatoria" CssClass="form-control" Style="border: none" ValidationSettings-ValidationGroup="scoring" ValidationSettings-RequiredField-IsRequired="true" ClientIDMode="Static"></dx:ASPxTextBox>
                                    <%--<asp:TextBox ID="lbSumatoria" runat="server" ReadOnly="true" CssClass="control-label" Style="font-size: 20px;" Width="119px"></asp:TextBox>--%>
                                </div>

                            </div>

                        </div>


                    </asp:Panel>
                </div>
            </div>


            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <br>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" OnClick="btnAtras_Click" />
                            </td>

                            <td>
                                <%--<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" class="btn btn-warning pull-right" OnClick="btnImprimir_Click" />
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-warning pull-right" />--%>

                                <div style="margin-right: 100px">
                                    <dx:ASPxButton ID="btnImprimir" runat="server" Text="Imprimir" CssClass="btn btn-warning pull-right" ClientInstanceName="btnImprimir" AutoPostBack="false">
                                        <ClientSideEvents Click="imprimirScoring" />
                                        <%-- <Paddings Padding="10px" />
                                          <FocusRectPaddings Padding="0px" />--%>
                                    </dx:ASPxButton>
                                </div>

                                <div style="margin-left: 100px">
                                    <dx:ASPxButton ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success pull-right" ClientInstanceName="btnGuardar" AutoPostBack="false">
                                        <ClientSideEvents Click="guardarScoring" />
                                    </dx:ASPxButton>
                                </div>

                                <div style="padding-right: 100px">
                                    <dx:ASPxButton ID="btnFinalizar" runat="server" Text="Finalizar" CssClass="btn btn-success pull-right" ClientInstanceName="btnFinalizar" AutoPostBack="false" Visible="false">
                                        <ClientSideEvents Click="finalizarScoring" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                    </table>



                </div>
            </div>
        </div>

    </div>
</div>


<dx:ASPxCallback ID="cbpScoring" runat="server" ClientInstanceName="cbpScoring" OnCallback="cbpScoring_Callback">
    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
</dx:ASPxCallback>

<dx:ASPxCallback ID="cbpGuardarScoring" runat="server" ClientInstanceName="cbpGuardarScoring" OnCallback="cbpGuardarScoring_Callback">
    <ClientSideEvents CallbackComplete="OnCallbackCompleteScoring" />
</dx:ASPxCallback>

<dx:ASPxCallback ID="cbpFinalizarScoring" runat="server" ClientInstanceName="cbpFinalizarScoring" OnCallback="cbpFinalizarScoring_Callback">
    <ClientSideEvents CallbackComplete="OnCallbackFinalizarScoring" />
</dx:ASPxCallback>


