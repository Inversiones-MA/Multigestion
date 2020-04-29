<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpSimulacion.ascx.cs" Inherits="MultiComercial.wpSimulacion.wpSimulacion.wpSimulacion" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script src="../../_layouts/15/MultiComercial/Validaciones.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>

<style type="text/css">
    body {
    }

    .dxgvTitlePanel {
        display: none;
    }

    .dxgvHeader a.dxgvCommandColumnItem,
    .dxgvHeader a.dxgvCommandColumnItem:hover,
    a.dxbButton, a.dxbButton:hover {
        color: #FF8800;
        text-decoration: underline;
    }

    #grid {
        border: 1px Solid #c0c0c0;
        font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
        background-color: White;
        color: #555;
        cursor: default;
    }

    .dxgvHeader {
        font-weight: bolder;
        background-color: #FFFFFF;
        border: 0;
        border-bottom: 1px #CCC solid;
        padding: 8px;
    }

    .btnInline {
        display: inline-table;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");

        var tipoCredito = ddlTipoCredito.GetValue();
        var tipoCredito1 = ddlTipoCredito.GetText();
        if (tipoCredito == '4') {
            document.getElementById('divTxtHorizonte').style.display = 'block';
            txtHorizonte.ValidationSetting = true;
        }
        else
            document.getElementById('divTxtHorizonte').style.display = 'none';

        if (tipoCredito == '5') {
            tecnica(false);
        }
    });

    function formato(s, e) {
        var monto = encodeHtml(s.GetText());
        var moneda = ddlMoneda.GetText();
        if (monto != '') {
            monto = parseFloat(monto.replace(/\./g, '').replace(/\,/g, '.'))
            if (moneda != null) {
                if (moneda == "CLP")
                    monto = formatNumber.new(parseFloat(monto).toFixed(0));
                else
                    monto = formatNumber.new(parseFloat(monto).toFixed(2));

                s.SetText(monto);
            }
        }
    }

    function formatoPesos(s, e) {
        var monto = encodeHtml(s.GetText());
        if (monto != '') {
            monto = parseFloat(monto.replace(/\./g, '').replace(/\,/g, '.'))
            monto = formatNumber.new(parseFloat(monto).toFixed(0));
            s.SetText(monto);
        }
    }


    function validarFormato(s, e) {
        //var moneda = ddlMoneda.GetText();
        //var monto = encodeHtml(txtMontoLiquido.GetText());
        //if (moneda != null) {
        //    if (moneda == "CLP")
        //        monto = formatNumber.new(parseFloat(monto).toFixed(0));
        //    else
        //        monto = formatNumber.new(parseFloat(monto).toFixed(2));
        //}
        //else
        //    monto = formatNumber.new(parseFloat(monto).toFixed(0));

        //txtMontoLiquido.SetText(monto);
    }

    function ValorAjustable(cad) {
        var valorA = cad + 'txtValorA';
        var valorC = cad + 'txtValorC';
        var hddAjuste = cad + 'hddAjuste';
        if (document.getElementById(valorC).value != "") {
            if (hddAjuste != "0") {
                document.getElementById(valorA).value = parseFloat(document.getElementById(valorC).value.replace(/\./g, '').replace(/\,/g, '.')) - (parseFloat(document.getElementById(valorC).value.replace(/\./g, '').replace(/\,/g, '.')) * (parseFloat(document.getElementById(hddAjuste).value) / 100)).toFixed(0);
                document.getElementById(valorA).value = (document.getElementById(valorA).value).replace(/\./g, ',');
                document.getElementById(valorA).disabled = true;
            } else {
                document.getElementById(valorA).value = document.getElementById(valorC).value;
                document.getElementById(valorA).disabled = true;
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

    function ocultarDiv() {
        var dvs = document.getElementById('dvSuccess');
        dvs.style.display = 'none';
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'none';
        var dve = document.getElementById('dvError');
        dve.style.display = 'none';
    }

    function Alerta(mensaje) {
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'block';
        lbWarning.SetText(mensaje);
        $('divContenedor').scrollTop();
    }

    function Exito(mensaje) {
        var dvw = document.getElementById('dvSuccess');
        dvw.style.display = 'block';
        lbSuccess.SetText(mensaje);
        $('divContenedor').scrollTop();
    }

    function OnBtnCotizar(s, e) {
        try {
            if (window.ASPxClientEdit.ValidateGroup('ValidarCotizador')) {
                ocultarDiv();
                OpenDlg();
                ASPxCallback1.PerformCallback('guardar');
                return true;
            }
            else {
                Alerta('Ingrese informacion necesaria...');
                return false;
            }

        } catch (e) {
            alert(e);
        }
    }

    function OnBtnImprimir(s, e) {
        try {
            ocultarDiv();
            OpenDlg();
            ASPxCallback1.PerformCallback('Imprimir');

            setTimeout(function () { CloseDlg(); }, 8000);

        } catch (e) {
            alert(e);
        }
    }

    function OnBtnVolver(s, e) {
        try {
            ocultarDiv();
            OpenDlg();
            ASPxCallback1.PerformCallback('volver');
        } catch (e) {
            alert(e);
        }
    }

    function OnClick(s, e) {
        GvGarantias.AddNewRow();
    }

    function OnClickGvAvales(s, e) {
        var rowsCount = window.GvAvales.GetVisibleRowsOnPage();
        if (rowsCount == 0)
            GvAvales.AddNewRow();
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    //function GvGarantias_BatchEditStartEditing(s, e) {
    //    //Id
    //    var TipoBienColumn = s.GetColumnByField("Nombre");
    //    if (!e.rowValues.hasOwnProperty(TipoBienColumn.index))
    //        return;
    //    var cellInfo = e.rowValues[TipoBienColumn.index];
    //    cbxTipoBien.SetValue(cellInfo.value);
    //    if (e.focusedColumn === TipoBienColumn)
    //        cbxTipoBien.SetFocus();

    //    var ValorAsegurableColumn = s.GetColumnByField("ValorAsegurable");
    //    if (!e.rowValues.hasOwnProperty(ValorAsegurableColumn.index))
    //        return;
    //    var cellInfo = e.rowValues[ValorAsegurableColumn.index];
    //    txtValorAsegurable.SetValue(cellInfo.value);
    //    if (e.focusedColumn === ValorAsegurableColumn)
    //        txtValorAsegurable.SetFocus();

    //    var PeriodoAnualColumn = s.GetColumnByField("PeriodoAnual");
    //    if (!e.rowValues.hasOwnProperty(PeriodoAnualColumn.index))
    //        return;
    //    var cellInfo = e.rowValues[PeriodoAnualColumn.index];
    //    txtPeriodoAnual.SetValue(cellInfo.value);
    //    if (e.focusedColumn === PeriodoAnualColumn) {
    //        txtPeriodoAnual.SetFocus();
    //        txtPeriodoAnual.SetEnabled(false);
    //    }
    //}

    //function GvGarantias_BatchEditEndEditing(s, e) {
    //    var cmbxBien = null;
    //    var txtAsegurable = null;
    //    var TipoBienColumn = s.GetColumnByField("Nombre");
    //    var ValorAsegurableColumn = s.GetColumnByField("ValorAsegurable");
    //    var PeriodoAnualColumn = s.GetColumnByField("PeriodoAnual");

    //    if (!e.rowValues.hasOwnProperty(TipoBienColumn.index))
    //        return;
    //    else {
    //        var cellInfo = e.rowValues[TipoBienColumn.index];
    //        cellInfo.value = cbxTipoBien.GetValue();
    //        cellInfo.text = encodeHtml(cbxTipoBien.GetText());
    //    }

    //    if (!e.rowValues.hasOwnProperty(ValorAsegurableColumn.index))
    //        return;
    //    else {
    //        var cellInfo = e.rowValues[ValorAsegurableColumn.index];
    //        cellInfo.value = txtValorAsegurable.GetValue();
    //        cellInfo.text = encodeHtml(txtValorAsegurable.GetText());
    //    }
    //}

    //function GvGarantias_BatchEditRowValidating(s, e) {
    //    var TipoBienColumn = s.GetColumnByField("Nombre"); //Id
    //    var cellValidationInfo = e.validationInfo[TipoBienColumn.index];
    //    if (!cellValidationInfo) return;
    //    var value = cellValidationInfo.value;
    //    if (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
    //        cellValidationInfo.isValid = false;
    //        cellValidationInfo.errorText = "Tipo Bien es requerido";
    //    }

    //    var ValorAsegurableColumn = s.GetColumnByField("ValorAsegurable");
    //    var cellValidationInfo = e.validationInfo[ValorAsegurableColumn.index];
    //    if (!cellValidationInfo) return;
    //    var value = cellValidationInfo.value;
    //    if (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
    //        cellValidationInfo.isValid = false;
    //        cellValidationInfo.errorText = "Valor asegurable es requerido";
    //    }
    //}

    function encodeHtml(text) {
        if (text === undefined || text === null)
            return "";
        var replacements = [
            [/&amp;/g, '&ampx;'], [/&/g, '&amp;'], [/&quot;/g, '&quotx;'], [/"/g, '&quot;'],
            [/&lt;/g, '&ltx;'], [/</g, '&lt;'], [/&gt;/g, '&gtx;'], [/>/g, '&gt;']
        ];
        for (var i = 0; i < replacements.length; i++) {
            var repl = replacements[i];
            text = text.replace(repl[0], repl[1]);
        }
        return text;
    }

    //function GvAvales_BatchEditStartEditing(s, e) {
    //    var NumAvalesColumn = s.GetColumnByField("NumAvales");
    //    if (!e.rowValues.hasOwnProperty(NumAvalesColumn.index))
    //        return;
    //    var cellInfo = e.rowValues[NumAvalesColumn.index];
    //    txtNumAvales.SetValue(cellInfo.value);
    //    if (e.focusedColumn === NumAvalesColumn)
    //        txtNumAvales.SetFocus();

    //    var ValorAsegurableColumn = s.GetColumnByField("ValorAsegurable");
    //    if (!e.rowValues.hasOwnProperty(ValorAsegurableColumn.index))
    //        return;
    //    var cellInfo = e.rowValues[ValorAsegurableColumn.index];
    //    txtValorAsegurableAval.SetValue(cellInfo.value);
    //    if (e.focusedColumn === ValorAsegurableColumn)
    //        txtValorAsegurableAval.SetFocus();

    //    var PeriodoAnualColumn = s.GetColumnByField("PeriodoAnual");
    //    if (!e.rowValues.hasOwnProperty(PeriodoAnualColumn.index))
    //        return;
    //    var cellInfo = e.rowValues[PeriodoAnualColumn.index];
    //    txtPeriodoAnualAval.SetValue(cellInfo.value);
    //    if (e.focusedColumn === PeriodoAnualColumn) {
    //        txtPeriodoAnualAval.SetFocus();
    //        txtPeriodoAnualAval.SetEnabled(false);
    //    }
    //}

    //function GvAvales_BatchEditEndEditing(s, e) {
    //    var NumAvalesColumn = s.GetColumnByField("NumAvales");
    //    var ValorAsegurableColumn = s.GetColumnByField("ValorAsegurable");

    //    if (!e.rowValues.hasOwnProperty(NumAvalesColumn.index))
    //        return;
    //    else {
    //        var cellInfo = e.rowValues[NumAvalesColumn.index];
    //        cellInfo.value = txtNumAvales.GetValue();
    //        cellInfo.text = encodeHtml(txtNumAvales.GetText());
    //    }

    //    if (!e.rowValues.hasOwnProperty(ValorAsegurableColumn.index))
    //        return;
    //    else {
    //        var cellInfo = e.rowValues[ValorAsegurableColumn.index];
    //        cellInfo.value = txtValorAsegurableAval.GetValue();
    //        cellInfo.text = txtValorAsegurableAval.GetText();
    //        //cellInfo.text = formatNumber.new(parseFloat(cellInfo.text).toFixed(2));
    //    }
    //}

    //function GvAvales_BatchEditRowValidating(s, e) {
    //    var NumAvalesColumn = s.GetColumnByField("NumAvales");
    //    var cellValidationInfo = e.validationInfo[NumAvalesColumn.index];
    //    if (!cellValidationInfo) return;
    //    var value = cellValidationInfo.value;
    //    if (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
    //        cellValidationInfo.isValid = false;
    //        cellValidationInfo.errorText = "Tipo Bien es requerido";
    //    }

    //    var ValorAsegurableColumn = s.GetColumnByField("ValorAsegurable");
    //    var cellValidationInfo = e.validationInfo[ValorAsegurableColumn.index];
    //    if (!cellValidationInfo) return;
    //    var value = cellValidationInfo.value;
    //    if (!ASPxClientUtils.IsExists(value) || ASPxClientUtils.Trim(value) === "") {
    //        cellValidationInfo.isValid = false;
    //        cellValidationInfo.errorText = "Valor asegurable es requerido";
    //    }
    //}

    function ValidarFecha(s, e) {
        try {
            var fechaInicio = dtcFechaCurse.GetValue();
            var fechaFin = dtcFecPrimerVencimiento.GetValue();
            var fechaFinOperacion = dtcFechaVencimientoOperacion.GetValue();
            var estructuraPago = ddlEstructuraPlazo.GetText();
            var fechaInicio = dtcFechaCurse.GetValue();
            var fechaFin = dtcFecPrimerVencimiento.GetValue();
            var fechaFinOperacion = dtcFechaVencimientoOperacion.GetValue();
            //var Men = 30;
            //var Tri = 90;
            //var Sem = 180;
            //var Anual = 360;
            var date1_ms = null;
            var date2_ms = null;

            if ((fechaInicio != "" && fechaInicio != null && fechaInicio.text != "&nbsp;") && (fechaFin != "" && fechaFin != null && fechaFin.text != "&nbsp;")) {
                var toType = ({}).toString.call(fechaInicio).match(/\s([a-z|A-Z]+)/)[1].toLowerCase()
                if (toType == 'string') {
                    fechaInicio = fechaInicio.split(' ');
                    var aFecha1 = fechaInicio[0].split('-');
                    if (aFecha1.length < 2)
                        var aFecha1 = fechaInicio[0].split('/');

                    var fFecha1 = new Date(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
                    date1_ms = fFecha1.getTime();
                }
                else if (toType == 'date')
                    date1_ms = fechaInicio.getTime();
                else
                    date1_ms = fechaInicio.value.getTime();


                var toType = ({}).toString.call(fechaFin).match(/\s([a-z|A-Z]+)/)[1].toLowerCase()
                if (toType == 'string') {
                    fechaFin = fechaFin.split(' ');
                    var aFecha2 = fechaFin[0].split('-');
                    if (aFecha2.length < 2)
                        var aFecha2 = fechaFin[0].split('/');

                    var fFecha2 = new Date(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
                    var date2_ms = fFecha2.getTime();
                }
                else if (toType == 'date')
                    date2_ms = fechaFin.getTime();
                else
                    date2_ms = fechaFin.value.getTime();

                // The number of milliseconds in one day
                var ONE_DAY = 1000 * 60 * 60 * 24;
                var difference_ms = Math.ceil(date2_ms - date1_ms);
                dias = Math.ceil(difference_ms / ONE_DAY);

                if (dias != null && !isNaN(dias)) {
                    if (dias < 0) {
                        s.isValid = false;
                        s.errorText = 'fecha Primer vencimiento no puede ser igual o menor a fecha de curse';
                        dtcFecPrimerVencimiento.SetText(null);
                        dtcFecPrimerVencimiento.SetValue(null);
                        Alerta('fecha Primer vencimiento no puede ser igual o menor a fecha de curse');
                    }
                    else {
                        if (txtPlazo.GetValue() != '' && txtPlazo.GetValue() != null) {
                            var plazo = txtPlazo.GetValue();

                            //if (plazo % 2 === 0)
                            //    plazo = parseInt(plazo) + 1;

                            //var d = new Date(date1_ms);
                            //var dd = d.setMonth(d.getMonth() + parseInt(plazo));

                            //validar reemplazo de dia del primer vencimiento en fecha vencimiento operacion
                            //var diaVencimientoFinal = null;
                            //var dia1Vencimiento = fechaFin.getDate();
                            //if (fechaFinOperacion != null)
                            //    diaVencimientoFinal = fechaFinOperacion.getDate();

                            //if (dia1Vencimiento != diaVencimientoFinal) {
                            //    var day = new Date(dd).getDate();
                            //    var month = new Date(dd).getMonth() + 1;
                            //    var year = new Date(dd).getFullYear();

                            //    if (dia1Vencimiento < 10) {
                            //        dia1Vencimiento = '0' + dia1Vencimiento;
                            //    }
                            //    else
                            //        dia1Vencimiento = (dia1Vencimiento + 1);

                            //    if (month < 10) {
                            //        month = '0' + month;
                            //    }

                            var TotalGracia = 0;
                            var TotalPlazo = 0;
                            var gracia = 0;

                            if (estructuraPago == 'Anual') {
                                TotalGracia = (dias * 360) / 365;
                                TotalGracia = parseFloat((TotalGracia / 360) - 1).toFixed(0);
                            }
                            else if (estructuraPago == 'Semestral') {
                                TotalGracia = (dias * 360) / 365;
                                TotalGracia = parseFloat((TotalGracia / 180) - 1).toFixed(0);
                            }
                            else if (estructuraPago == 'Trimestral') {
                                TotalGracia = (dias * 360) / 365;
                                TotalGracia = parseFloat((TotalGracia / 90) - 1).toFixed(0);
                            }
                            else if (estructuraPago == 'Mensual') {
                                TotalGracia = (dias * 360) / 365;
                                TotalGracia = parseFloat((TotalGracia / 30) - 1).toFixed(0);
                            }


                            if (estructuraPago == 'Anual') {
                                TotalPlazo = (plazo / 12); // - TotalGracia;
                            }
                            else if (estructuraPago == 'Semestral') {
                                TotalPlazo = (plazo / 6); // - TotalGracia;
                            }
                            else if (estructuraPago == 'Trimestral') {
                                TotalPlazo = (plazo / 3); // - TotalGracia;
                            }
                            else if (estructuraPago == 'Mensual') {
                                TotalPlazo = (plazo / 1); // - TotalGracia;
                            }

                            var final = null;
                            var d_curse = new Date(date1_ms);
                            var d_curse1 = new Date(date1_ms);
                            var d_1Vcto = new Date(date2_ms);

                            if (estructuraPago == 'Mensual') {
                                // FECHA = (AÑO_FECHA_Curse ; MES_FECHA_Curse + (TotalPlazo) ; DIA_Fecha1Vcto)

                                var d_curse = d_curse.setMonth(new Date(d_curse).getMonth() + (1 + TotalPlazo));

                                var anioCurse = new Date(d_curse).getFullYear();
                                var mesCurse = new Date(d_curse).getMonth();
                                var dia1Vcto = new Date(d_1Vcto).getDate();

                                if (dia1Vcto < 10) {
                                    dia1Vcto = '0' + dia1Vcto;
                                }

                                if (mesCurse < 10) {
                                    mesCurse = '0' + mesCurse;
                                }

                                final = new Date(anioCurse + '-' + mesCurse + '-' + dia1Vcto);

                                if (new Date(final).getDate() != dia1Vcto) {
                                    dia1Vcto = parseInt(dia1Vcto) + 1;
                                    if (dia1Vcto < 10) {
                                        dia1Vcto = '0' + dia1Vcto;
                                    }
                                    final = new Date(anioCurse + '-' + mesCurse + '-' + dia1Vcto);
                                }
                            }
                            else {
                                // FECHA = (Año_FECHA_Curse ; MES(Fecha1Vcto + (TotalPlazo) * (Men o Tri o Sem o Anual) / 30 ; DIA_Fecha1Vcto)
                                var factorMes = 0;
                                if (estructuraPago == 'Trimestral')
                                    factorMes = 90;
                                else if (estructuraPago == 'Semestral')
                                    factorMes = 180;
                                else if (estructuraPago == 'Anual')
                                    factorMes = 360;
                                else
                                    factorMes = 0;

                                var d_curse = d_curse.setMonth(new Date(d_1Vcto).getMonth() + 1 + TotalPlazo * (factorMes / 30));

                                var anioCurse = new Date(d_curse).getFullYear();
                                var mes1Vcto = new Date(d_curse).getMonth();
                                var dia1Vcto = new Date(d_1Vcto).getDate();

                                if (dia1Vcto < 10) {
                                    dia1Vcto = '0' + dia1Vcto;
                                }

                                if (mes1Vcto < 10) {
                                    mes1Vcto = '0' + mes1Vcto;
                                }

                                final = new Date(anioCurse + '-' + mes1Vcto + '-' + dia1Vcto);

                                if (new Date(final).getDate() != dia1Vcto) {
                                    dia1Vcto = parseInt(dia1Vcto) + 1;
                                    if (dia1Vcto < 10) {
                                        dia1Vcto = '0' + dia1Vcto;
                                    }
                                    final = new Date(anioCurse + '-' + mes1Vcto + '-' + dia1Vcto);
                                }
                            }

                            // var date = new Date(year + '-' + month + '-' + (parseInt(dia1Vencimiento)));
                            if (!isNaN(final)) {
                                dtcFechaVencimientoOperacion.SetText(final);
                                dtcFechaVencimientoOperacion.SetValue(final);
                            }

                        }
                        else {
                            dtcFechaVencimientoOperacion.SetText(null);
                            dtcFechaVencimientoOperacion.SetValue(null);
                        }
                        ocultarDiv();
                    }
                }
            }
        }
        catch (e) {
        }
    }

    function ChangeDateTo(s, e) {
        var DateTo = new Date(s.GetDate());
        DateTo.setDate(DateTo.getDate() + 1);
        BoxDateTo.SetDate(DateTo);
    }

    function OnCallbackComplete(s, e) {
        ocultarDiv();
        if (e.result == "fin") {
            CloseDlg();
        }
        else if (e.result == "ok") {
            cpnl.PerformCallback('ver');
            Exito('revise y verifique los resultados de la simulacion');
            alert('revise y verifique los resultados de la simlacion');
        }
        else if (e.result == "session") {
            alert("error de sessión");
            window.location.reload();
        }
        else if (e.result == 'error') {
            Alerta('no existe una cotizacion');
            CloseDlg();
        }
    }

    //function OnCallbackComplete2(s, e) {
    //    ASPxCallback1.PerformCallback('guardar');
    //}

    function endcallbackpanel(s, e) {
        var tipoCredito = ddlTipoCredito.GetValue();
        var tipoCredito1 = ddlTipoCredito.GetText();
        if (tipoCredito == '4') {
            document.getElementById('divTxtHorizonte').style.display = 'block';
            txtHorizonte.ValidationSetting = true;
        }
        else
            document.getElementById('divTxtHorizonte').style.display = 'none';

        if (tipoCredito == '5') {
            tecnica(false);
        }
        CloseDlg();
    }

    function ValidarEliminar(s, e) {
        if (e.buttonID === "DeleteBtn")
            e.processOnServer = confirm('¿Desea borrar el registro?');
        else
            e.processOnServer = true;
    }

    function ValidarEliminarAvales(s, e) {
        if (e.buttonID === "DeleteBtnAvales")
            e.processOnServer = confirm('¿Desea borrar el registro?');
        else
            e.processOnServer = true;
    }

    function OnValidarCredito(s, e) {
        var tipoCredito = s.GetText();
        if (s.GetText() == 'Cuotas iguales con Cuotón') {
            document.getElementById('divTxtHorizonte').style.display = 'block';
            txtHorizonte.ValidationSetting = true;   //.ValidateGroup == 'ValidarCotizador';
        }
        else {
            document.getElementById('divTxtHorizonte').style.display = 'none';
            txtHorizonte.isValid = false;
            txtHorizonte.ValidateGroup = null;
        }

        if (s.GetValue() == 5) {
            tecnica(false);
        }
        else {
            tecnica(true);
        }
    }

    function tecnica(esTecnica) {
        if (esTecnica) {
            document.getElementById('divPlazo').style.display = 'block';
            document.getElementById('divPago').style.display = 'block';
        }
        else {
            document.getElementById('divPlazo').style.display = 'none';
            document.getElementById('divPago').style.display = 'none';
        }
        txtTasaBanco.ValidationSetting = esTecnica;
        txtTasaBanco.SetVisible(esTecnica);
        ddlPlazo.ValidationSetting = esTecnica;
        ddlPlazo.SetVisible(esTecnica);
        ddlEstructuraPlazo.ValidationSetting = esTecnica;
        ddlEstructuraPlazo.SetVisible(esTecnica);
    }

    function Validarhorizonte(s, e) {
        var valor = txtHorizonte.GetText();
        if (valor != null) {
            if (valor > 144) {
                txtHorizonte.SetText('');
                txtHorizonte.SetValue('');
                alert('el plazo maximo del horizonte es 144 meses');
            }
        }
    }

    //evitar edicion formulario sharepoint
    function WebForm_OnSubmit() {
        return false;
    }

    //function saveAval() {
    //    var to = txtNumAvales.GetText();
    //    var tal = txtValorAsegurableAval.GetText();

    //    if (txtNumAvales.GetText() != '' && txtValorAsegurableAval.GetText() != '') {
    //        for (var i = GvAvales.GetTopVisibleIndex() ; i < ASPxClientGridViewInstance.GvAvales() ; i++) {
    //            GvAvales.GetRowValues(i, "IdAvalCotizacion", OnCallback);
    //            GvAvales.GetRowValues(i, "NumAvales", OnCallback);
    //            GvAvales.GetRowValues(i, "ValorAsegurable", OnCallback);
    //        }
    //    }
    //}

    function OnCallback(values) {
        alert(values);
        ASPxCallback2.PerformCallback('Imprimir');
    }

    function esNumero(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^[0-9.\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

    function esDecimal(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^[0-9,\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
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
                    <a href="#">Simulador</a>
                </li>
                <li class="active">Simulador
                </li>
            </ol>
        </div>
    </div>

    <%-- Cuadros de Alerta --%>
    <br />
    <div id="dvSuccess" runat="server" clientidmode="Static" style="display: none" class="alert alert-success">
        <h4>
            <dx:ASPxLabel ID="lbSuccess" runat="server" ClientInstanceName="lbSuccess" ClientIDMode="Static" CssClass="alert alert-success" Text=""></dx:ASPxLabel>
        </h4>
    </div>
    <div id="dvWarning" runat="server" style="display: none" clientidmode="Static" class="alert alert-warning" role="alert">
        <h4>
            <dx:ASPxLabel ID="lbWarning" runat="server" ClientInstanceName="lbWarning" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
        </h4>
    </div>
    <div id="dvError" runat="server" style="display: none" clientidmode="Static" class="alert alert-danger">
        <h4>
            <dx:ASPxLabel ID="lbError" runat="server" ClientInstanceName="lbError" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
        </h4>
    </div>
    <br />

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal" role="form">

                <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server" ClientInstanceName="ASPxHiddenField1"></dx:ASPxHiddenField>
                <dx:ASPxHiddenField ID="ASPxHiddenFieldEmpresa" runat="server" ClientInstanceName="ASPxHiddenFieldEmpresa"></dx:ASPxHiddenField>

                <dx:ASPxCallbackPanel ID="cpnl" runat="server" Width="100%" ClientInstanceName="cpnl" Height="100%" OnCallback="cpnl_Callback" ClientSideEvents-EndCallback="endcallbackpanel" SettingsLoadingPanel-ShowImage="false" SettingsLoadingPanel-Enabled="false">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">
                            <!-- Indicadores -->
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="alert alert-gray">
                                        <p>
                                            <strong>UF:</strong>
                                            <dx:ASPxLabel ID="lbUf" runat="server" ClientIDMode="Static"></dx:ASPxLabel>
                                            <dx:ASPxTextBox ID="txtUf" runat="server" Width="170px" ReadOnly="true" ClientVisible="false"></dx:ASPxTextBox>
                                            <strong>USD:</strong>
                                            <dx:ASPxLabel ID="lblDolar" runat="server" ClientInstanceName="lblDolar" ClientIDMode="Static"></dx:ASPxLabel>
                                            <dx:ASPxTextBox ID="txtDolar" runat="server" Width="170px" ReadOnly="true" ClientVisible="false"></dx:ASPxTextBox>
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <!-- Contenedor Tabs -->
                            <div class="row" id="divContenedor">
                                <div class="col-md-12">
                                    <div class="card-box">

                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <div class="panel-heading" role="tab" id="Div1">
                                                    <h4 class="panel-title">
                                                        <a role="button">Datos Simulador
                                                        </a>
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card-box">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Cliente</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxTextBox ID="txtEmpresa" runat="server" ClientInstanceName="txtEmpresa" MaxLength="200" CssClass="form-control" Width="97%" ValidationSettings-ValidationGroup="ValidarCotizador">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Ejecutivo</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlEjecutivo" runat="server" ClientInstanceName="ddlEjecutivo" ValueType="System.String" CssClass="form-control" NullText="Seleccione" Width="98%" ValidationSettings-ValidationGroup="ValidarCotizador">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="card-box">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Monto Líquido</label>
                                                        <div class="col-sm-7">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 70%">
                                                                        <dx:ASPxTextBox ID="txtMontoLiquido" runat="server" ClientInstanceName="txtMontoLiquido" MaxLength="15" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" Width="95%">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                                <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                                <RequiredField IsRequired="true" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents LostFocus="formato" />
                                                                            <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                                        </dx:ASPxTextBox>
                                                                    </td>

                                                                    <td style="width: 20%">
                                                                        <dx:ASPxComboBox ID="ddlMoneda" runat="server" ClientInstanceName="ddlMoneda" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="95%">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                                <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                                <RequiredField IsRequired="true" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents SelectedIndexChanged="validarFormato" />
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Tipo de Crédito</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlTipoCredito" runat="server" ClientInstanceName="ddlTipoCredito" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents SelectedIndexChanged="OnValidarCredito" />
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" id="divPago">
                                                        <label class="col-sm-5 control-label">Estructura Pago</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlEstructuraPlazo" runat="server" ClientInstanceName="ddlEstructuraPlazo" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents SelectedIndexChanged="ValidarFecha" />
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Fecha Curse</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxDateEdit ID="dtcFechaCurse" ClientInstanceName="dtcFechaCurse" runat="server" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" EditFormat="Date" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Fecha 1° Vcto</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxDateEdit ID="dtcFecPrimerVencimiento" ClientInstanceName="dtcFecPrimerVencimiento" runat="server" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" EditFormat="Date" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents DateChanged="ValidarFecha" />
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Cobert. Certif.</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlCobCertificado" runat="server" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                <Items>
                                                                    <dx:ListEditItem Text="70" Value="8"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="80" Value="9"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="100" Value="11"></dx:ListEditItem>
                                                                </Items>
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Gastos Legales ( en Pesos $ )</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxTextBox ID="txtGastoLegal" runat="server" ClientInstanceName="txtGastoLegal" MaxLength="10" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents LostFocus="formatoPesos" />
                                                                <ClientSideEvents KeyPress="function(s,e){ esNumero(s,e);}" />
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Plazo (meses)</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxTextBox ID="txtPlazo" runat="server" MaxLength="3" CssClass="form-control" ClientInstanceName="txtPlazo" ValidationSettings-ValidationGroup="ValidarCotizador" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents LostFocus="ValidarFecha" />
                                                                <ClientSideEvents KeyPress="function(s,e){ esNumero(s,e);}" />
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" id="divTxtHorizonte" style="display: none">
                                                        <label class="col-sm-5 control-label">Plazo Horizonte (meses)</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxTextBox ID="txtHorizonte" ClientInstanceName="txtHorizonte" runat="server" MaxLength="3" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents LostFocus="Validarhorizonte" />
                                                                <ClientSideEvents KeyPress="function(s,e){ esNumero(s,e);}" />
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group" id="divPlazo">
                                                        <label class="col-sm-5 control-label">Tasa Banco (%)</label>
                                                        <div class="col-sm-7">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 55%">
                                                                        <dx:ASPxTextBox ID="txtTasaBanco" ClientInstanceName="txtTasaBanco" runat="server" MaxLength="6" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" Width="98%">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                                <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                                <RequiredField IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxTextBox>
                                                                    </td>
                                                                    <td style="width: 70%">
                                                                        <dx:ASPxComboBox ID="ddlPlazo" ClientInstanceName="ddlPlazo" runat="server" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="110%">
                                                                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                                <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                                <RequiredField IsRequired="true" />
                                                                            </ValidationSettings>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Capitaliza intereses</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlCapitalizaIntereses" runat="server" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                <Items>
                                                                    <dx:ListEditItem Selected="True" Text="Si" Value="1"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="No" Value="2"></dx:ListEditItem>
                                                                </Items>
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Fecha Vcto. Op.</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxDateEdit ID="dtcFechaVencimientoOperacion" ClientInstanceName="dtcFechaVencimientoOperacion" runat="server" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" EditFormat="Date" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Comisión incluida ?</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlComision" runat="server" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                <Items>
                                                                    <dx:ListEditItem Selected="True" Text="Si" Value="1"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="No" Value="2"></dx:ListEditItem>
                                                                </Items>
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Fogape %</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxComboBox ID="ddlFogape" runat="server" ValueType="System.String" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                <Items>
                                                                    <dx:ListEditItem Selected="True" Text="0" Value="1"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="50" Value="2"></dx:ListEditItem>
                                                                    <dx:ListEditItem Text="80" Value="3"></dx:ListEditItem>
                                                                </Items>
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-sm-5 control-label">Gasto Notario ( en Pesos $ )</label>
                                                        <div class="col-sm-7">
                                                            <dx:ASPxTextBox ID="txtGastosNotario" runat="server" ClientInstanceName="txtGastosNotario" MaxLength="10" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" Width="98%">
                                                                <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                    <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                                <ClientSideEvents LostFocus="formatoPesos" />
                                                                <ClientSideEvents KeyPress="function(s,e){ esNumero(s,e);}" />
                                                            </dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="panel-heading" role="tab" id="Div2">
                                                        <h4 class="panel-title">
                                                            <a role="button">Comisión y Seguros
                                                            </a>
                                                        </h4>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="card-box">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-5 control-label">Comisión Anual (%)</label>
                                                            <div class="col-sm-7">
                                                                <dx:ASPxTextBox ID="txtComisionAnual" runat="server" MaxLength="4" CssClass="form-control" ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" ValidationSettings-ValidationGroup="ValidarCotizador" Width="98%">
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                        <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                        <RequiredField IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxTextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="form-group">
                                                            <label class="col-sm-5 control-label">Fondo</label>
                                                            <div class="col-sm-7">
                                                                <dx:ASPxComboBox ID="ddlFondo" runat="server" ValueType="System.Int32" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" NullText="Seleccione" Width="98%">
                                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                                        <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                                        <RequiredField IsRequired="true" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <div class="panel-heading" role="tab" id="Div3">
                                                            <h4 class="panel-title">
                                                                <a role="button">Seguros (Tipo Bien / Valor  Asegurable)
                                                                </a>
                                                            </h4>
                                                        </div>

                                                    </div>
                                                </div>


                                                <div class="row">
                                                    <%--OnBatchUpdate="GvGarantias_BatchUpdate"--%>
                                                    <div class="col-md-12">
                                                        <table border="0" style="table-layout: fixed; width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <!--OnCustomButtonCallback="GvGarantias_CustomButtonCallback"  <ClientSideEvents CustomButtonClick="ValidarEliminar" /> -->
                                                                    <div class="card-box">
                                                                        <div class="table-responsive">
                                                                            <dx:ASPxGridView ID="GvGarantias" runat="server" ClientInstanceName="GvGarantias" Width="100%"
                                                                                CssClass="table table-responsive table-hover table-bordered"
                                                                                OnRowDeleting="GvGarantias_RowDeleting"
                                                                                OnRowInserting="GvGarantias_RowInserting"
                                                                                OnRowUpdating="GvGarantias_RowUpdating"
                                                                                KeyFieldName="IdSeguro;IdTemporal"
                                                                                AutoGenerateColumns="False"
                                                                                OnDataBound="GvGarantias_DataBound">
                                                                                <SettingsBehavior ConfirmDelete="true" />
                                                                                <SettingsText ConfirmDelete="¿Desea borrar el registro?" />
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn ShowInCustomizationForm="True" Width="5%" VisibleIndex="0" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                                                                        <HeaderTemplate>
                                                                                            <dx:ASPxButton ID="BtnNuevo" ClientInstanceName="BtnNuevo" runat="server" Text="Nuevo" RenderMode="Button" AutoPostBack="false" CssClass="btn btn-primary btn-success pull-right" Width="10%">
                                                                                                <ClientSideEvents Click="OnClick" />
                                                                                            </dx:ASPxButton>
                                                                                        </HeaderTemplate>
                                                                                    </dx:GridViewCommandColumn>
                                                                                    <dx:GridViewDataComboBoxColumn FieldName="IdTipoBien" Caption="Tipo Bien" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                                        <PropertiesComboBox ValueType="System.Int32" TextField="Nombre" ValueField="Id" DataSourceID="dsTipoBien">
                                                                                            <ValidationSettings>
                                                                                                <RequiredField IsRequired="True" />
                                                                                            </ValidationSettings>
                                                                                        </PropertiesComboBox>
                                                                                    </dx:GridViewDataComboBoxColumn>
                                                                                    <%--<dx:GridViewDataComboBoxColumn FieldName="Nombre" Caption="Tipo Bien" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                        <EditItemTemplate>
                                                                                            <dx:ASPxComboBox ID="cbxTipoBien" ClientInstanceName="cbxTipoBien" runat="server" Width="100%"
                                                                                                Value='<%# Bind("Id") %>' TextField="Nombre" ValueField="Id"
                                                                                                ValueType="System.Int32" OnInit="cbxTipoBien_Init">
                                                                                                <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" ValidationGroup="crearTipoBien"></ValidationSettings>
                                                                                            </dx:ASPxComboBox>
                                                                                        </EditItemTemplate>
                                                                                    </dx:GridViewDataComboBoxColumn>--%>
                                                                                    <dx:GridViewDataTextColumn FieldName="ValorAsegurable" Caption="Valor Asegurable (UF)" HeaderStyle-Wrap="True" VisibleIndex="2" PropertiesTextEdit-MaxLength="8" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>

                                                                                    <dx:GridViewDataTextColumn FieldName="PeriodoAnual" Caption="Periodo Anual (UF)" HeaderStyle-Wrap="True" VisibleIndex="3">
                                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                        <EditFormSettings Visible="False" />
                                                                                    </dx:GridViewDataTextColumn>

                                                                                </Columns>
                                                                                <Settings ShowTitlePanel="true" />
                                                                                <SettingsText Title="" />
                                                                                <SettingsPager PageSize="30" />
                                                                                <%--<SettingsEditing Mode="Inline" NewItemRowPosition="Top" BatchEditSettings-EditMode="Row">
                                                                                </SettingsEditing>--%>
                                                                                <%--<SettingsEditing EditFormColumnCount="2" NewItemRowPosition="Top" />--%>
                                                                                <SettingsCommandButton>
                                                                                    <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                                                                    <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                                                                    <UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success pull-right" Styles-Style-Width="10%"></UpdateButton>
                                                                                    <CancelButton Text="Cancelar" ButtonType="Button"></CancelButton>
                                                                                </SettingsCommandButton>
                                                                                <Settings ShowFooter="True" />

                                                                            </dx:ASPxGridView>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>

                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <div class="panel-heading" role="tab" id="Div4">
                                                            <h4 class="panel-title">
                                                                <a role="button">Seguro Desgravamen
                                                                </a>
                                                            </h4>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <table border="0" style="table-layout: fixed; width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <div class="card-box">

                                                                        <%--OnBatchUpdate="GvAvales_BatchUpdate" OnCustomButtonCallback="GvAvales_CustomButtonCallback"  <ClientSideEvents CustomButtonClick="ValidarEliminarAvales" />--%>
                                                                        <div class="table-responsive">
                                                                            <dx:ASPxGridView ID="GvAvales" runat="server" ClientInstanceName="GvAvales" KeyFieldName="IdAvalCotizacion" Width="100%"
                                                                                CssClass="table table-responsive table-hover table-bordered" ClientIDMode="Static" AutoGenerateColumns="false"
                                                                                OnRowInserting="GvAvales_RowInserting"
                                                                                OnRowUpdating="GvAvales_RowUpdating"
                                                                                OnRowDeleting="GvAvales_RowDeleting"
                                                                                OnDataBound="GvAvales_DataBound">
                                                                                <SettingsBehavior ConfirmDelete="true" />
                                                                                <SettingsText ConfirmDelete="¿Desea borrar el registro?" />
                                                                                <Columns>
                                                                                    <dx:GridViewCommandColumn VisibleIndex="0" Width="5%" ShowNewButtonInHeader="true" ShowEditButton="true" Name="btn"
                                                                                        ShowDeleteButton="true">
                                                                                        <HeaderTemplate>
                                                                                            <dx:ASPxButton ID="BtnNuevo" ClientInstanceName="BtnNuevo" runat="server" Text="Nuevo" RenderMode="Button" AutoPostBack="false" CssClass="btn btn-primary btn-success pull-right" Width="10%">
                                                                                                <ClientSideEvents Click="OnClickGvAvales" />
                                                                                            </dx:ASPxButton>
                                                                                        </HeaderTemplate>
                                                                                    </dx:GridViewCommandColumn>
                                                                                    <dx:GridViewDataColumn FieldName="IdAvalCotizacion" HeaderStyle-Wrap="True" VisibleIndex="1" Visible="false">
                                                                                    </dx:GridViewDataColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="NumAvales" Caption="N° Avales" HeaderStyle-Wrap="True" VisibleIndex="2" PropertiesTextEdit-NullText="0" PropertiesTextEdit-MaxLength="3" PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="ValorAsegurable" Caption="Valor Asegurable por Aval (UF)" HeaderStyle-Wrap="True" VisibleIndex="3" PropertiesTextEdit-MaxLength="8" PropertiesTextEdit-NullText="0" PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn FieldName="PeriodoAnual" Caption="Periodo Anual (UF)" HeaderStyle-Wrap="True" VisibleIndex="4">
                                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                        <EditFormSettings Visible="False" />
                                                                                    </dx:GridViewDataTextColumn>

                                                                                </Columns>

                                                                                <Settings ShowTitlePanel="true" />
                                                                                <SettingsText Title="" />
                                                                                <SettingsPager PageSize="30" />
                                                                                <%--<SettingsEditing Mode="Inline" NewItemRowPosition="Top">
                                                                                </SettingsEditing>--%>
                                                                                <SettingsEditing EditFormColumnCount="2" NewItemRowPosition="Top"></SettingsEditing>
                                                                                <SettingsCommandButton>
                                                                                    <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                                                                    <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                                                                    <UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success pull-right" Styles-Style-Width="10%"></UpdateButton>
                                                                                    <CancelButton Text="Cancelar" ButtonType="Button"></CancelButton>
                                                                                </SettingsCommandButton>
                                                                                <Settings ShowFooter="True" />
                                                                                <Settings ShowStatusBar="Hidden" />
                                                                                <SettingsBehavior AllowSort="false" AllowGroup="false" />
                                                                            </dx:ASPxGridView>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>


                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <div class="panel-heading" role="tab" id="Div7">
                                                            <h4 class="panel-title">
                                                                <a role="button">Resumen Operación
                                                                </a>
                                                            </h4>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="card-box">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Monto Bruto</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtMontoBruto" runat="server" MaxLength="15" CssClass="form-control" ReadOnly="true">
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Comisión MultiAval</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtComisionMultiAval" runat="server" MaxLength="15" CssClass="form-control" ReadOnly="true">
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Gastos Fogape</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtGastosFogape" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Seguro Desgravamen</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtSeguroDesgravamen" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Carga Financiera</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtCargaFinanciera" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Cuotas de Gracia</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtCuotasGracia" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true" Width="100%"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Impuesto Timbre</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtImpToTimbre" runat="server" MaxLength="15" CssClass="form-control" ReadOnly="true">
                                                                    </dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Gastos Legales</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtGastosLegales" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Gasto Notario Acreedor</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtGastoNotario" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Seguro Garantía</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtSeguroGarantia" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Descuento por Prepago</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtDescPrepago" runat="server" MaxLength="12" CssClass="form-control" ReadOnly="true"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="display: none">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">Devolución de Comisión por prepago</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtDevolucionComision" runat="server" MaxLength="12" CssClass="form-control"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label class="col-sm-5 control-label">% Comisión / Monto Bruto por % cobertura</label>
                                                                <div class="col-sm-7">
                                                                    <dx:ASPxTextBox ID="txtComisionMontoBruto" runat="server" MaxLength="12" CssClass="form-control"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="resumenOperacion" class="card-box">

                                                    <div class="row">
                                                        <div class="col-md-12 col-xs-12">
                                                            <div class="panel-heading" role="tab" id="Div5">
                                                                <h4 class="panel-title">
                                                                    <a role="button">Desarrollo de la Operación
                                                                    </a>
                                                                </h4>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <div class="card-box">
                                                                                <div class="table-responsive">

                                                                                    <dx:ASPxGridView ID="GvResumenOperacion" runat="server" ClientInstanceName="GvResumenOperacion" KeyFieldName="IdCotizacion" Width="100%"
                                                                                        OnPageIndexChanged="GvResumenOperacion_PageIndexChanged"
                                                                                        CssClass="table table-responsive table-hover table-bordered" ClientIDMode="Static">
                                                                                        <Columns>
                                                                                            <dx:GridViewDataColumn FieldName="Dias" Caption="Dìas" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="Mes" Caption="Mes" HeaderStyle-Wrap="True" VisibleIndex="2">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="FechaVencimiento" Caption="Fecha Vencimiento" HeaderStyle-Wrap="True" VisibleIndex="3">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="Amortizacion" Caption="Amortización" HeaderStyle-Wrap="True" VisibleIndex="4">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="Interes" Caption="Interes" HeaderStyle-Wrap="True" VisibleIndex="5">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="Cuota" Caption="Cuota" HeaderStyle-Wrap="True" VisibleIndex="6">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="SaldoK" Caption="Saldo K" HeaderStyle-Wrap="True" VisibleIndex="7">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                            <dx:GridViewDataColumn FieldName="Comision" Caption="Comisión" HeaderStyle-Wrap="True" VisibleIndex="8">
                                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                                            </dx:GridViewDataColumn>
                                                                                        </Columns>
                                                                                        <Settings ShowTitlePanel="true" />
                                                                                        <SettingsText Title="Cálculo del Duration" />
                                                                                        <SettingsPager PageSize="100" />
                                                                                        <Settings ShowFooter="True" />
                                                                                        <Settings ShowStatusBar="Hidden" />
                                                                                        <SettingsBehavior AllowSort="false" AllowGroup="false" />

                                                                                    </dx:ASPxGridView>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div class="col-md-6 col-xs-6" style="display: none">
                                                                <div class="form-group">
                                                                    <div class="panel-heading" role="tab" id="Div6">
                                                                        <h4 class="panel-title">
                                                                            <a role="button">Resumen Comisión
                                                                            </a>
                                                                        </h4>
                                                                    </div>

                                                                    <dx:ASPxGridView ID="GvResumenComision" runat="server" ClientInstanceName="GvResumenComision" KeyFieldName="IdCotizacion" Width="100%"
                                                                        OnPageIndexChanged="GvResumenComision_PageIndexChanged"
                                                                        CssClass="table table-responsive table-hover table-bordered" ClientIDMode="Static">
                                                                        <Columns>
                                                                            <dx:GridViewDataColumn FieldName="CuotaFactor" Caption="Cuota Factor" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="DiasFactor" Caption="Dias Factor" HeaderStyle-Wrap="True" VisibleIndex="2">
                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="TasaEfectiva" Caption="Tasa Efectiva" HeaderStyle-Wrap="True" VisibleIndex="3">
                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="Pitatoria" Caption="Pitatoria" HeaderStyle-Wrap="True" VisibleIndex="4">
                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                            </dx:GridViewDataColumn>
                                                                            <dx:GridViewDataColumn FieldName="AjSumat" Caption="Aj Sumat" HeaderStyle-Wrap="True" VisibleIndex="5">
                                                                                <HeaderStyle HorizontalAlign="Center" Font-Names="Arial" Font-Size="11" Font-Bold="true"></HeaderStyle>
                                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                            </dx:GridViewDataColumn>

                                                                        </Columns>
                                                                        <Settings ShowTitlePanel="true" />
                                                                        <SettingsText Title="" />
                                                                        <SettingsPager PageSize="100" />
                                                                        <Settings ShowFooter="True" />
                                                                        <Settings ShowStatusBar="Hidden" />
                                                                        <SettingsBehavior AllowSort="false" AllowGroup="false" />

                                                                    </dx:ASPxGridView>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>

                <div class="card-box">
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <div id="Botones">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" CssClass="btn btn-default pull-left" Text="< Volver Atrás" Width="10%">
                                                <ClientSideEvents Click="OnBtnVolver" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td>
                                            <dx:ASPxButton ID="btnImprimir" runat="server" Text="Imprimir" ClientInstanceName="btnImprimir" CssClass="btn btn-default pull-right" AutoPostBack="false">
                                                <ClientSideEvents Click="OnBtnImprimir" />
                                            </dx:ASPxButton>

                                            <dx:ASPxButton ID="btnGuardar" runat="server" Text="Calcular" ClientInstanceName="btnGuardar" CssClass="btn btn-success pull-right" ValidationGroup="ValidarCotizador" AutoPostBack="false">
                                                <ClientSideEvents Click="OnBtnCotizar" />
                                            </dx:ASPxButton>

                                        </td>
                                    </tr>

                                </table>
                            </div>
                        </div>

                    </div>
                </div>

                <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="ASPxCallback1" OnCallback="ASPxCallback1_Callback">
                    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
                </dx:ASPxCallback>

                <%-- <dx:ASPxCallback ID="ASPxCallback2" runat="server" ClientInstanceName="ASPxCallback2" OnCallback="ASPxCallback2_Callback">
                    <ClientSideEvents CallbackComplete="OnCallbackComplete2" />
                </dx:ASPxCallback>--%>

                <asp:SqlDataSource runat="server" ID="dsTipoBien" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select Id, Nombre from TipoBienesCotizador" SelectCommandType="Text"></asp:SqlDataSource>
            </div>
        </div>
    </div>
</div>
