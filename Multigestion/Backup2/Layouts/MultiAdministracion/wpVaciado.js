
var etiquetaValor;
var ref;
var etiquetaPond;
var cadena;


function SoloLectura(e) {
    // var unicode = e.charCode ? e.charCode : e.keyCode

    return false


}


function prueba(cad) {

    document.getElementById(cad + 'Label2').innerHTML = 100;
    document.getElementById(cad + 'Label3').innerHTML = 100;
    // var vparticipacion = document.getElementById(idParticipacion).value;
    document.getElementById(cad + 'TextBox2').value = document.getElementById(cad + 'TextBox1').value;
    document.getElementById(cad + 'TextBox3').value = 100;


}

function cal(combo, etiquetaValorcb, referencia, etiquetaPonderacion, cad) {
    etiquetaValor = etiquetaValorcb;
    etiquetaPond = etiquetaPonderacion;
    ref = referencia;
    cadena = cad;
    var query;
    var combo = document.getElementById(combo);
    var seleccion = combo.selectedIndex;
    seleccion = combo.options[seleccion].value;
    // alert(seleccion);
    document.getElementById(etiquetaValor).value = 0;

    if (seleccion == '-1') {
        document.getElementById(etiquetaValor).value = 0;
    }
    else {
        query = '<View><Query><Where><Eq><FieldRef Name=\'ID\'/>' + '<Value Type=\'Number\'>' + seleccion + '</Value></Eq></Where></Query></View>';
        lista = 'Indicadores';
        retrieveListItems(query, lista);
    }
    return false;
}

function calAct(combo, etiquetaValorcb, referencia, etiquetaPonderacion, cad) {
    etiquetaValor = etiquetaValorcb;
    etiquetaPond = etiquetaPonderacion;
    ref = referencia;
    cadena = cad;
    var query;
    var combo = document.getElementById(combo);
    var seleccion = combo.selectedIndex;
    seleccion = combo.options[seleccion].value;
    query = '<View><Query><Where><Eq><FieldRef Name=\'ID\'/>' + '<Value Type=\'Number\'>' + seleccion + '</Value></Eq></Where></Query></View>';
    lista = 'ActividadEconomica';
    retrieveListItems(query, lista);
    return false;
}

function retrieveListItems(query, lista) {
    var clientContext = new SP.ClientContext.get_current();
    var oList = clientContext.get_web().get_lists().getByTitle(lista);
    var camlQuery = new SP.CamlQuery();
    camlQuery.set_viewXml(query);
    this.collListItem = oList.getItems(camlQuery);
    clientContext.load(collListItem);
    var a = clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
    return false;
}

function onQuerySucceeded(sender, args) {
    var listItemEnumerator = collListItem.getEnumerator();
    while (listItemEnumerator.moveNext()) {
        var oListItem = listItemEnumerator.get_current();
        if (ref == 0) {
            document.getElementById(etiquetaValor).value = oListItem.get_item('Valor2');
        } else {

            if (checkDecimals(document.getElementById(etiquetaPond).value)) {

                if (ref == 1) {
                    if (checkDecimals(oListItem.get_item('Valor'))) {
                        document.getElementById(etiquetaValor).value = (parseFloat(oListItem.get_item('Valor')) * parseFloat(document.getElementById(etiquetaPond).value)) / 100;
                    }
                }
                else {
                    if (ref == 2) {
                        document.getElementById(etiquetaValor).value = (parseFloat(oListItem.get_item('Valor2')) * parseFloat(document.getElementById(etiquetaPond).value)) / 100;
                    }

                    if (ref == 3) {
                        document.getElementById(etiquetaValor).value = (parseFloat(oListItem.get_item('Puntaje')) * parseFloat(document.getElementById(etiquetaPond).value)) / 100;
                    }

                }

            }

        }
    }
    sumarPtje(cadena);
}


var indCaso;
var maxPond;

function Total(code) {
    var query = '';
    cadena = code;
    if (indCaso == 0) {
        var valor = parseInt(document.getElementById(cadena + 'lbSumatoria').value);
        //  alert('valor: ' + valor);
        query = '<View><Query><Where><And><Eq><FieldRef Name=\'CodigoPadre\'/>'
                    + '<Value Type=\'Text\'>PtjeTotalCont</Value></Eq><Leq><FieldRef Name=\'Title\'/>'
                    + '<Value Type=\'Text\'>' + valor + '</Value></Leq></And></Where><OrderBy><FieldRef Name=\'Title\' Ascending =\'FALSE\'/></OrderBy></Query></View>';
    }       //   Ascending='FALSE'   "<OrderBy><FieldRef Name='ID' Ascending='FALSE'/></OrderBy>";        
    if (indCaso == 1) {
        query = '<View><Query><Where><And><Eq><FieldRef Name=\'CodigoPadre\'/>'
                + '<Value Type=\'Text\'>PtjeTotalCont</Value></Eq><Leq><FieldRef Name=\'Valor2\'/>'
                + '<Value Type=\'Text\'>' + maxPond + '</Value></Leq></And></Where><OrderBy><FieldRef Name=\'Title\' Ascending =\'FALSE\'/></OrderBy></Query></View>';
    }
    if (indCaso == 2) {
        var valor1 = document.getElementById(cadena + 'lbPtjeLetra').value;
        //  alert('valor1: ' + valor1);
        query = '<View><Query><Where><And><Eq><FieldRef Name=\'CodigoPadre\'/>'
            + '<Value Type=\'text\'>PtjeTotalCont</Value></Eq><Eq><FieldRef Name=\'Descripcion\'/>' + '<Value Type=\'text\'>' + valor1 + '</Value></Eq></And></Where></Query></View>';
    }
    if (indCaso == 3) {
        var valor2 = parseInt(document.getElementById(cadena + 'lbSumatoria').value);
        // alert('valor2: ' + valor2);
        query = '<View><Query><Where><And><Eq><FieldRef Name=\'CodigoPadre\'/>'
            + '<Value Type=\'text\'>PtjeTotalCont</Value></Eq><Leq><FieldRef Name=\'Title\'/>'
            + '<Value Type=\'text\'>' + valor2 + '</Value></Leq></And></Where><OrderBy><FieldRef Name=\'Title\' Ascending =\'FALSE\'/></OrderBy></Query></View>';
    }
    if (query != '') {
        lista = 'Indicadores';
        retrieveListItemsTotal(query, lista);
    }
    return false;
}

function retrieveListItemsTotal(query, lista) {
    var clientContext = new SP.ClientContext.get_current();
    var oList = clientContext.get_web().get_lists().getByTitle(lista);
    var camlQuery = new SP.CamlQuery();
    camlQuery.set_viewXml(query);
    this.collListItem = oList.getItems(camlQuery);
    clientContext.load(collListItem);
    var a = clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceededTotal), Function.createDelegate(this, this.onQueryFailed));
    return false;
}

function onQuerySucceededTotal(sender, args) {

    try {
        var listItemEnumerator = this.collListItem.getEnumerator();
    }
    catch (err) {
        return false;
    }


    while (listItemEnumerator.moveNext()) {
        var oListItem = listItemEnumerator.get_current();

        //  alert( '\nID: ' + oListItem.get_id() + '\nTitle: ' + oListItem.get_item('Title'));

        if (indCaso == 1) {
            document.getElementById(cadena + 'lbPtjeLetra').value = oListItem.get_item('Descripcion');
        }
        if (indCaso == 2) {
            document.getElementById(cadena + 'lbPuntaje').value = oListItem.get_item('Valor2');
        }
        if (indCaso == 3) {
            document.getElementById(cadena + 'lbPuntaje').value = oListItem.get_item('Valor2');
        }
        if (indCaso == 0) {
            document.getElementById(cadena + 'lbPtjeLetra').value = oListItem.get_item('Descripcion') + '/' + oListItem.get_item('Valor');
        }
        if (document.getElementById(cadena + 'lbPtjeLetra').value == 'C' || document.getElementById(cadena + 'lbPtjeLetra').value == 'C+') {
            indCaso = 2;
            Total(cadena);

        } else {

            indCaso = 3;
            Total(cadena);
        }


        return false;

    }
}


function checkDecimals(fieldValue) {
    decallowed = 3;
    //8
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


function onQueryFailed(sender, args) {

    alert('Error de Memoria . ' + args.get_message() + '\n' + args.get_stackTrace());
}


function LimpiarVacido1(cad) {

    var idPanel = cad + 'pnFormulario1';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
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



function LimpiarVacido2(cad) {

    var idPanel = cad + 'pnFormulario2';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
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



function LimpiarVacido3(cad) {

    var idPanel = cad + 'pnFormulario3';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
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



function LimpiarVacidoAct(cad) {

    var idPanel = cad + 'pnFormularioAct';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
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



function LimpiarVacido(cad) {

    var idPanel = cad + 'pnFormulario';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
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







function SumatoriaVacido(celdaevento, celda, ACCION, grupo, cad, sumCampo) {
    if (isNaN(document.getElementById(celdaevento).value) || document.getElementById(celdaevento).value == "") {
        document.getElementById(celdaevento).value = 0;
    }
    if (isNaN(document.getElementById(celda).value) || document.getElementById(celda).value == "") {
        document.getElementById(celda).value = 0;
    }
    var auxTotal = document.getElementById(celda).value;
    if (ACCION == 2)
        document.getElementById(celda).value = parseFloat(auxTotal) - parseFloat(document.getElementById(celdaevento).value);
    else

        document.getElementById(celda).value = parseFloat(document.getElementById(celdaevento).value) + parseFloat(auxTotal);


    if (isNaN(document.getElementById(cad + sumCampo).value) || document.getElementById(cad + sumCampo).value == "") {
        document.getElementById(cad + sumCampo).value = 0;
    }
    // if (grupo == 1)
    {
        var auxTotal1 = document.getElementById(cad + sumCampo).value;
        if (ACCION == 2)
            document.getElementById(cad + sumCampo).value = parseFloat(auxTotal1) - parseFloat(document.getElementById(celdaevento).value);
        else
            document.getElementById(cad + sumCampo).value = parseFloat(document.getElementById(celdaevento).value) + parseFloat(auxTotal1);

    }

    document.getElementById(celda).disabled = true;


    return false;

}



function vaciarCampo(celdaevento, celda, ACCION, grupo, cad, sumCampo) {
    if (isNaN(document.getElementById(celdaevento).value) || document.getElementById(celdaevento).value == "") {
        document.getElementById(celdaevento).value = 0;
    }
    if (isNaN(document.getElementById(celda).value) || document.getElementById(celda).value == "") {
        document.getElementById(celda).value = 0;
    }
    //alert("-" + document.getElementById(celdaevento).value + "-");
    //alert("-" + document.getElementById(celda).value + "-");
    var auxInd = document.getElementById(celdaevento).value;
    var auxTotal = document.getElementById(celda).value;
    if (ACCION == 2)
        document.getElementById(celda).value = parseFloat(auxTotal) + parseFloat(auxInd);
    else
        document.getElementById(celda).value = parseFloat(auxTotal) - parseFloat(auxInd);

    document.getElementById(celdaevento).value = "";
    document.getElementById(celda).disabled = true;


    if (isNaN(document.getElementById(cad + sumCampo).value) || document.getElementById(cad + sumCampo).value == "") {
        document.getElementById(cad + sumCampo).value = 0;
    }


    //if (grupo == 1) {
    var auxTotal1 = document.getElementById(cad + sumCampo).value;
    if (ACCION == 2)
        document.getElementById(cad + sumCampo).value = parseFloat(auxTotal1) + parseFloat(auxInd);
    else
        document.getElementById(cad + sumCampo).value = parseFloat(auxTotal1) - parseFloat(auxInd);

    // }

    return false;

}



function SumatoriaText(celda) {

    // alert(celda);
    //alert(document.getElementById(celda).value);
    if (document.getElementById(celda).value == "") {
        document.getElementById(celda).value = 0;
    } else {

        document.getElementById(celda).value = parseInt(document.getElementById(celda).value) + 1;
    }
    return false;
}



function HabilitarScoring(cad) {

    var idPanel = cad + 'pnFormulario';

    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnLimpiar';
    // var idFinalizar = cad + 'btnFinalizar';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    // document.getElementById(idFinalizar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    return false;
}


function HabilitarVaciado(cad) {

    var idPanel = cad + 'pnFormulario';

    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnLimpiar';
    var idFinalizar = cad + 'btnFinalizar';

    var idGuardari = cad + 'btnGuardari';
    var idLimpiari = cad + 'btnlimpiari';
    var idFinalizari = cad + 'btnFinalizari';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    document.getElementById(idFinalizar).style.display = 'block';

    document.getElementById(idGuardari).style.display = 'block';
    document.getElementById(idLimpiari).style.display = 'block';
    document.getElementById(idFinalizari).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
    var children = div_to_disable;
    ///   alert(children.length);
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    //var tabla = document.getElementById(cad + 'ResultadosBusqueda');


    //alert(tabla.rows.length);

    //for (i = 1; i < tabla.rows.length; i++) {
    //    if (i < 4) {


    //         alert(tabla.rows[i].cells[0].innerHTML);
    //        alert(tabla.rows[i].cells[1].innerHTML);
    //        alert(tabla.rows[i].cells[2].innerHTML);
    //        alert(tabla.rows[i].cells[3].innerHTML);
    //        alert(tabla.rows[i].cells[4].innerHTML);
    //        alert(tabla.rows[i].cells[5].innerHTML);

    //    }
    //}
    //var table = document.getElementById('tblPaso');
    //for (var i = 0; i < table.rows.length; i++) {
    //    //                for (var j = 0; j < table.rows[i].cells.length; j++) {
    //    //                    textos = textos + table.rows[i].cells[j].innerHTML + ' | ';
    //    //                }
    //    //                textos = textos + '\n';
    //    alert("td1=" + table.rows[i].cells[0].innerHTML);
    //    var textos = table.rows[i].getElementsByTagName('input');
    //    for (var z = 0; z < textos.length; z++) {
    //        if (table.rows[i].cells[0].innerHTML == "10") {
    //            textos[z].disabled = "disabled";
    //        }
    //    }
    //}


    return false;
}

function HabilitarVaciadoIvaAct(cad) {

    var idPanel = cad + 'pnFormularioAct';

    var idGuardar = cad + 'btnGuardarAct';
    var idLimpiar = cad + 'btnLimpiarAct';
    var idFinalizar = cad + 'btnFinalizarAct';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    document.getElementById(idFinalizar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    return false;
}

function HabilitarVaciadoIva1(cad) {

    var idPanel = cad + 'pnFormulario1';

    var idGuardar = cad + 'btnGuardar1';
    var idLimpiar = cad + 'btnLimpiar1';
    var idFinalizar = cad + 'btnFinalizar1';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    document.getElementById(idFinalizar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    return false;
}

function HabilitarVaciadoIva2(cad) {

    var idPanel = cad + 'pnFormulario2';

    var idGuardar = cad + 'btnGuardar2';
    var idLimpiar = cad + 'btnLimpiar2';
    var idFinalizar = cad + 'btnFinalizar2';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    document.getElementById(idFinalizar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    return false;
}

function HabilitarVaciadoIva3(cad) {

    var idPanel = cad + 'pnFormulario3';

    var idGuardar = cad + 'btnGuardar3';
    var idLimpiar = cad + 'btnLimpiar3';
    var idFinalizar = cad + 'btnFinalizar3';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    document.getElementById(idFinalizar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    return false;
}

function sumarPtjeTotal(cad) {

    var combo = document.getElementById(cad + 'cbMorosidad');
    var seleccion = combo.selectedIndex;
    seleccion = combo.options[seleccion].text;

    if (seleccion == 'Sin Protestos ni Morosidad') {
        indCaso = 0;
        Total(cad);
    }
    else {

        if (document.getElementById(cadena + 'lbPondMorosidad').value > document.getElementById(cadena + 'lbPondMonto').value) {
            maxPond = document.getElementById(cadena + 'lbPondMorosidad').value;
        }
        else {
            maxPond = document.getElementById(cadena + 'lbPondMonto').value;
        }
        indCaso = 1;
        Total(cad);
    }


}


function sumarPtje(cad) {
    var ban = 0;
    var lbsumatoria = 0;
    if (document.getElementById(cad + 'lbPtjeCalidadBalance').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeCalidadBalance').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeCalidadBalance').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbCalidadBalance').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeCalidadGarantia').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeCalidadGarantia').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeCalidadGarantia').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbCalidadGarantia').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeCoberturaGarantia').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeCoberturaGarantia').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeCoberturaGarantia').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbCoberturaGarantia').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeConcentracion').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeConcentracion').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeConcentracion').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbConcentracion').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeCompetencia').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeCompetencia').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeCompetencia').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbCompetencia').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeEbitda').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeEbitda').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeEbitda').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbEbitda').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeExperiencia').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeExperiencia').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeExperiencia').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbExperiencia').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeLeverage').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeLeverage').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeLeverage').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbLeverage').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeLiquidez').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeLiquidez').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeLiquidez').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbLiquidez').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjePasivo').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjePasivo').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjePasivo').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbPasivo').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjePasivoVts').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjePasivoVts').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjePasivoVts').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbPasivoVts').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeTipoGarantia').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeTipoGarantia').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeTipoGarantia').value);
    }
    else {
        ban = 1;
        document.getElementById(cad + 'cbTipoGarantia').selectedIndex = 0;
    }
    if (document.getElementById(cad + 'lbPtjeActividad').value != 0 && checkDecimals(document.getElementById(cad + 'lbPtjeActividad').value)) {
        lbsumatoria = parseFloat(lbsumatoria) + parseFloat(document.getElementById(cad + 'lbPtjeActividad').value);
    }
    else {
        //   document.getElementById(cad + 'cbCalidadBalance').options[0].selected = 'selected';
    }
    document.getElementById(cad + 'lbSumatoria').value = parseFloat(lbsumatoria).toFixed(3);
    if (ban == 0) {

        sumarPtjeTotal(cad);
    }

}
function Limpiar(cad) {


    // ctl00_ctl35_g_d7fb041d_f3fb_4ce3_982d_1eee7b8b3659_ResultadosBusqueda_ctl01_cbValor1
    Dialogo();
    var sel1 = document.getElementById(cad).innerHTML;
    sel1.selectedIndex = 1;

    //alert(cad);
    CerrarMensaje()
    //
    //alert(tabla.cells);
    //alert(tabla);
    //celdas = tabla.cells;
    //for (i = 0; i < celdas.length - 1; i++) {
    //    //if (celdas[i].firstChild.type == "checkbox"
    //    //&& celdas[i].firstChild.checked != newState) {
    //    //    celdas[i].firstChild.click();
    //    //}
    //    if (celdas[i].firstChild.type == "input")
    //    {
    //        alert("aqui");
    //    }

    //}



    //  var grid = document.getElementById(idNombre).value;

    // alert(celdas);
}


function Validar(cad) {
    //// Limpiar();
    ////alert(cad);
    //var idNombre = cad + 'txt_nombre';


    //// document.getElementById('errorRut').innerHTML = "";

    //document.getElementById(idNombre).style.borderColor = "";

    //var vnombre = document.getElementById(idNombre).value;


    ////var varchivo = document.getElementById(idArchivo).value;
    //var nombre, cargo, profesion, universidad, rut, antiguedad;

    //if (vacio(vnombre)) {
    //    document.getElementById(idNombre).style.borderColor = "red";
    //    //document.getElementById('errorNombre').innerHTML = "requerido";
    //    nombre = false;
    //} else {
    //    nombre = true;
    //    document.getElementById(idNombre).style.borderColor = "";
    //    // document.getElementById('errorNombre').innerHTML = "";
    //}


    //if (nombre && cargo && rut && antiguedad) {
    //    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    //    //waitDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose('Registro de Campaña', 'Por favor espere mientras procesamos su operación...', 100, 550);
    //    return true;
    //} else
    //    return false;


}

function ShowServerInformation() {

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);

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


function format(input) {
    var num = input.value.replace(/\./g, '');
    if (!isNaN(num)) {
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split('').reverse().join('').replace(/^[\.]/, '');
        input.value = num;
    }

    else {
        //alert('Solo se permiten numeros');
        input.value = input.value.replace(/[^\d\.]*/g, '');
        return false;
    }
}

function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            return false
    }
}

function solonumerosN(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 45)
                return false
    }
}



function ResultadoBruto(celAct, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,
                                celVal12, celVal13, celVal14, celVal15, celVal16,
                                celVal17, celVal18, celVal19, celVal20, celVal21) {
    if (isNaN(document.getElementById(celVal0).value) || document.getElementById(celVal0).value == "") {
        document.getElementById(celVal0).value = 0;
    }
    if (isNaN(document.getElementById(celVal1).value) || document.getElementById(celVal1).value == "") {
        document.getElementById(celVal1).value = 0;
    }
    document.getElementById(celAct).value = parseFloat(document.getElementById(celVal0).value) - parseFloat(document.getElementById(celVal1).value);

    // ResultadoBruto(celVal2, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    ResultadoOperacional(celVal4, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,celVal12, celVal13, celVal14, celVal15, celVal16,celVal17, celVal18, celVal19, celVal20, celVal21);
    ResultadoNoOperacional(celVal14, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    ResultadoAntesImpuesto(celVal15, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaLiquida(celVal19, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaEjercicio(celVal21, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    return false;
}

function ResultadoOperacional(celAct, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,
                                celVal12, celVal13, celVal14, celVal15, celVal16,
                                celVal17, celVal18, celVal19, celVal20, celVal21) {

    if (isNaN(document.getElementById(celVal2).value) || document.getElementById(celVal2).value == "") {
        document.getElementById(celVal2).value = 0;
    }
    if (isNaN(document.getElementById(celVal3).value) || document.getElementById(celVal3).value == "") {
        document.getElementById(celVal3).value = 0;
    }
    document.getElementById(celAct).value = parseFloat(document.getElementById(celVal2).value) - parseFloat(document.getElementById(celVal3).value);


    // ResultadoBruto(celVal2, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    // ResultadoOperacional(celVal4, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    ResultadoNoOperacional(celVal14, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    ResultadoAntesImpuesto(celVal15, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaLiquida(celVal19, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaEjercicio(celVal21, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    return false;
}


function ResultadoNoOperacional(celAct, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,
                                celVal12, celVal13, celVal14, celVal15, celVal16,
                                celVal17, celVal18, celVal19, celVal20, celVal21) {

    if (isNaN(document.getElementById(celVal5).value) || document.getElementById(celVal5).value == "") {
        document.getElementById(celVal5).value = 0;
    }
    if (isNaN(document.getElementById(celVal6).value) || document.getElementById(celVal6).value == "") {
        document.getElementById(celVal6).value = 0;
    }
    if (isNaN(document.getElementById(celVal7).value) || document.getElementById(celVal7).value == "") {
        document.getElementById(celVal7).value = 0;
    }
    if (isNaN(document.getElementById(celVal8).value) || document.getElementById(celVal8).value == "") {
        document.getElementById(celVal8).value = 0;
    }
    if (isNaN(document.getElementById(celVal9).value) || document.getElementById(celVal9).value == "") {
        document.getElementById(celVal9).value = 0;
    }
    if (isNaN(document.getElementById(celVal10).value) || document.getElementById(celVal10).value == "") {
        document.getElementById(celVal10).value = 0;
    }
    if (isNaN(document.getElementById(celVal11).value) || document.getElementById(celVal11).value == "") {
        document.getElementById(celVal11).value = 0;
    }
    if (isNaN(document.getElementById(celVal12).value) || document.getElementById(celVal12).value == "") {
        document.getElementById(celVal12).value = 0;
    }
    if (isNaN(document.getElementById(celVal13).value) || document.getElementById(celVal13).value == "") {
        document.getElementById(celVal13).value = 0;
    }
    document.getElementById(celAct).value = -parseFloat(document.getElementById(celVal5).value) +
                                                parseFloat(document.getElementById(celVal6).value) +
                                                 parseFloat(document.getElementById(celVal7).value) +
                                                  parseFloat(document.getElementById(celVal8).value) -
                                                   parseFloat(document.getElementById(celVal9).value) -
                                                    parseFloat(document.getElementById(celVal10).value) -
                                                     parseFloat(document.getElementById(celVal11).value) +
                                                      parseFloat(document.getElementById(celVal12).value) +
                                                        parseFloat(document.getElementById(celVal13).value);
    // ResultadoBruto(celVal2, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //  ResultadoOperacional(celVal4, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    // ResultadoNoOperacional(celVal14, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    ResultadoAntesImpuesto(celVal15, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaLiquida(celVal19, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaEjercicio(celVal21, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);

    return false;
}



function ResultadoAntesImpuesto(celAct, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,
                                celVal12, celVal13, celVal14, celVal15, celVal16,
                                celVal17, celVal18, celVal19, celVal20, celVal21) {
    if (isNaN(document.getElementById(celVal4).value) || document.getElementById(celVal4).value == "") {
        document.getElementById(celVal4).value = 0;
    }
    if (isNaN(document.getElementById(celVal14).value) || document.getElementById(celVal14).value == "") {
        document.getElementById(celVal14).value = 0;
    }
    document.getElementById(celAct).value = parseFloat(document.getElementById(celVal4).value) + parseFloat(document.getElementById(celVal14).value);
    //  ResultadoBruto(celVal2, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //  ResultadoOperacional(celVal4, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //  ResultadoNoOperacional(celVal14, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    // ResultadoAntesImpuesto(celVal15, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaLiquida(celVal19, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaEjercicio(celVal21, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
   
    return false;
}



function UtilidadPerdidaLiquida(celAct, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,
                                celVal12, celVal13, celVal14, celVal15, celVal16,
                                celVal17, celVal18, celVal19, celVal20, celVal21) {
    if (isNaN(document.getElementById(celVal15).value) || document.getElementById(celVal15).value == "") {
        document.getElementById(celVal15).value = 0;
    }
    if (isNaN(document.getElementById(celVal16).value) || document.getElementById(celVal16).value == "") {
        document.getElementById(celVal16).value = 0;
    }
    if (isNaN(document.getElementById(celVal17).value) || document.getElementById(celVal17).value == "") {
        document.getElementById(celVal17).value = 0;
    }
    if (isNaN(document.getElementById(celVal18).value) || document.getElementById(celVal18).value == "") {
        document.getElementById(celVal18).value = 0;
    }
    document.getElementById(celAct).value =   parseFloat(document.getElementById(celVal15).value)
                                            - parseFloat(document.getElementById(celVal16).value)
                                            + parseFloat(document.getElementById(celVal17).value)
                                            + parseFloat(document.getElementById(celVal18).value);
    // ResultadoBruto(celVal2, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    // ResultadoOperacional(celVal4, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //ResultadoNoOperacional(celVal14, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    // ResultadoAntesImpuesto(celVal15, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //UtilidadPerdidaLiquida(celVal19, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    UtilidadPerdidaEjercicio(celVal21, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    return false;
}




function UtilidadPerdidaEjercicio(celAct, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8, celVal9, celVal10, celVal11,
                                celVal12, celVal13, celVal14, celVal15, celVal16,
                                celVal17, celVal18, celVal19, celVal20, celVal21) {
    if (isNaN(document.getElementById(celVal19).value) || document.getElementById(celVal19).value == "") {
        document.getElementById(celVal19).value = 0;
    }
    if (isNaN(document.getElementById(celVal20).value) || document.getElementById(celVal20).value == "") {
        document.getElementById(celVal20).value = 0;
    }
    document.getElementById(celAct).value = parseFloat(document.getElementById(celVal19).value) + parseFloat(document.getElementById(celVal20).value);

    //ResultadoBruto(celVal2, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //ResultadoOperacional(celVal4, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //ResultadoNoOperacional(celVal14, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //ResultadoAntesImpuesto(celVal15, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    //UtilidadPerdidaLiquida(celVal19, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    // UtilidadPerdidaEjercicio(celVal21, celVal0, celVal1, celVal2, celVal3, celVal4, celVal5, celVal6, celVal7, celVal8, celVal9, celVal10, celVal11, celVal12, celVal13, celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21);
    return false;
}


//-----------------------------------------



function ActivoCirculanteT(celActCirculante, celActLargoPlazo, celTotalActivos, celVal1, celVal2, celVal3, celVal4, celVal5,
                                celVal6, celVal7, celVal8) {

    if (isNaN(document.getElementById(celVal1).value) || document.getElementById(celVal1).value == "") {
        document.getElementById(celVal1).value = 0;
    }
    if (isNaN(document.getElementById(celVal2).value) || document.getElementById(celVal2).value == "") {
        document.getElementById(celVal2).value = 0;
    }
    if (isNaN(document.getElementById(celVal3).value) || document.getElementById(celVal3).value == "") {
        document.getElementById(celVal3).value = 0;
    }
    if (isNaN(document.getElementById(celVal4).value) || document.getElementById(celVal4).value == "") {
        document.getElementById(celVal4).value = 0;
    }
    if (isNaN(document.getElementById(celVal5).value) || document.getElementById(celVal5).value == "") {
        document.getElementById(celVal5).value = 0;
    }
    if (isNaN(document.getElementById(celVal6).value) || document.getElementById(celVal6).value == "") {
        document.getElementById(celVal6).value = 0;
    }
    if (isNaN(document.getElementById(celVal7).value) || document.getElementById(celVal7).value == "") {
        document.getElementById(celVal7).value = 0;
    }
    if (isNaN(document.getElementById(celVal8).value) || document.getElementById(celVal8).value == "") {
        document.getElementById(celVal8).value = 0;
    }
                                    

    document.getElementById(celActCirculante).value = parseFloat(document.getElementById(celVal1).value)
                                                + parseFloat(document.getElementById(celVal2).value)
                                                + parseFloat(document.getElementById(celVal3).value)
                                                + parseFloat(document.getElementById(celVal4).value)
                                                + parseFloat(document.getElementById(celVal5).value)
                                                + parseFloat(document.getElementById(celVal6).value)
                                                + parseFloat(document.getElementById(celVal7).value)
                                                + parseFloat(document.getElementById(celVal8).value);
    TotalActivoT(celActCirculante, celActLargoPlazo, celTotalActivos);

    return false;
}



function ActivoLargoPlazoT(celActCirculante, celActLargoPlazo, celTotalActivos, celVal9, celVal10, celVal11, celVal12, celVal13,
                                celVal14, celVal15, celVal16, celVal17, celVal18, celVal19, celVal20, celVal21, celVal22, celVal23,
                                celVal24, celVal25) {

    if (isNaN(document.getElementById(celVal9).value) || document.getElementById(celVal9).value == "") {
        document.getElementById(celVal9).value = 0;
    }
    if (isNaN(document.getElementById(celVal10).value) || document.getElementById(celVal10).value == "") {
        document.getElementById(celVal10).value = 0;
    }
    if (isNaN(document.getElementById(celVal11).value) || document.getElementById(celVal11).value == "") {
        document.getElementById(celVal11).value = 0;
    }
    if (isNaN(document.getElementById(celVal12).value) || document.getElementById(celVal12).value == "") {
        document.getElementById(celVal12).value = 0;
    }
    if (isNaN(document.getElementById(celVal13).value) || document.getElementById(celVal13).value == "") {
        document.getElementById(celVal13).value = 0;
    }
    if (isNaN(document.getElementById(celVal14).value) || document.getElementById(celVal14).value == "") {
        document.getElementById(celVal14).value = 0;
    }
    if (isNaN(document.getElementById(celVal15).value) || document.getElementById(celVal15).value == "") {
        document.getElementById(celVal15).value = 0;
    }
    if (isNaN(document.getElementById(celVal16).value) || document.getElementById(celVal16).value == "") {
        document.getElementById(celVal16).value = 0;
    }
    if (isNaN(document.getElementById(celVal17).value) || document.getElementById(celVal17).value == "") {
        document.getElementById(celVal17).value = 0;
    }
    if (isNaN(document.getElementById(celVal18).value) || document.getElementById(celVal18).value == "") {
        document.getElementById(celVal18).value = 0;
    }

        if (isNaN(document.getElementById(celVal19).value) || document.getElementById(celVal19).value == "") {
            document.getElementById(celVal19).value = 0;
        }
        if (isNaN(document.getElementById(celVal20).value) || document.getElementById(celVal20).value == "") {
            document.getElementById(celVal20).value = 0;
        }
        if (isNaN(document.getElementById(celVal21).value) || document.getElementById(celVal21).value == "") {
            document.getElementById(celVal21).value = 0;
        }
        if (isNaN(document.getElementById(celVal22).value) || document.getElementById(celVal22).value == "") {
            document.getElementById(celVal22).value = 0;
        }
        if (isNaN(document.getElementById(celVal23).value) || document.getElementById(celVal23).value == "") {
            document.getElementById(celVal23).value = 0;
        }
        if (isNaN(document.getElementById(celVal24).value) || document.getElementById(celVal24).value == "") {
            document.getElementById(celVal24).value = 0;
        }
        if (isNaN(document.getElementById(celVal25).value) || document.getElementById(celVal25).value == "") {
            document.getElementById(celVal25).value = 0;
        }
                                   

        document.getElementById(celActLargoPlazo).value = parseFloat(document.getElementById(celVal19).value)
                    + parseFloat(document.getElementById(celVal20).value)
                    + parseFloat(document.getElementById(celVal21).value)
                    + parseFloat(document.getElementById(celVal22).value)
                    + parseFloat(document.getElementById(celVal23).value)
                    + parseFloat(document.getElementById(celVal24).value)
                    + parseFloat(document.getElementById(celVal25).value)
                    + parseFloat(document.getElementById(celVal9).value)
                    + parseFloat(document.getElementById(celVal10).value)
                    + parseFloat(document.getElementById(celVal11).value)
                    + parseFloat(document.getElementById(celVal12).value)
                    + parseFloat(document.getElementById(celVal13).value)
                    + parseFloat(document.getElementById(celVal14).value)
                    - parseFloat(document.getElementById(celVal15).value)
                    - parseFloat(document.getElementById(celVal16).value)
                    - parseFloat(document.getElementById(celVal17).value)
                    - parseFloat(document.getElementById(celVal18).value);
  
        TotalActivoT(celActCirculante, celActLargoPlazo, celTotalActivos);

        return false;
    }

    function TotalActivoT(celActCirculante, celActLargoPlazo, celTotalActivos) {

        if (isNaN(document.getElementById(celActCirculante).value) || document.getElementById(celActCirculante).value == "") {
            document.getElementById(celActCirculante).value = 0;
        }
        if (isNaN(document.getElementById(celActLargoPlazo).value) || document.getElementById(celActLargoPlazo).value == "") {
            document.getElementById(celActLargoPlazo).value = 0;
        }
        document.getElementById(celTotalActivos).value = parseFloat(document.getElementById(celActCirculante).value) + parseFloat(document.getElementById(celActLargoPlazo).value);

        return false;
    }



    //------------------------------------------

    function PasivoCirculanteT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin,
        celVal26, celVal27, celVal28, celVal29, celVal30,
                                    celVal31, celVal32, celVal33, celVal34, celVal35) {

        if (isNaN(document.getElementById(celVal26).value) || document.getElementById(celVal26).value == "") {
            document.getElementById(celVal26).value = 0;
        }
        if (isNaN(document.getElementById(celVal27).value) || document.getElementById(celVal27).value == "") {
            document.getElementById(celVal27).value = 0;
        }
        if (isNaN(document.getElementById(celVal28).value) || document.getElementById(celVal28).value == "") {
            document.getElementById(celVal28).value = 0;
        }
        if (isNaN(document.getElementById(celVal29).value) || document.getElementById(celVal29).value == "") {
            document.getElementById(celVal29).value = 0;
        }
        if (isNaN(document.getElementById(celVal30).value) || document.getElementById(celVal30).value == "") {
            document.getElementById(celVal30).value = 0;
        }
        if (isNaN(document.getElementById(celVal31).value) || document.getElementById(celVal31).value == "") {
            document.getElementById(celVal31).value = 0;
        }
        if (isNaN(document.getElementById(celVal32).value) || document.getElementById(celVal32).value == "") {
            document.getElementById(celVal32).value = 0;
        }
        if (isNaN(document.getElementById(celVal33).value) || document.getElementById(celVal33).value == "") {
            document.getElementById(celVal33).value = 0;
        }
        if (isNaN(document.getElementById(celVal34).value) || document.getElementById(celVal34).value == "") {
            document.getElementById(celVal34).value = 0;
        }
        if (isNaN(document.getElementById(celVal35).value) || document.getElementById(celVal35).value == "") {
            document.getElementById(celVal35).value = 0;
        }

        document.getElementById(celPasCirculante).value = parseFloat(document.getElementById(celVal26).value)
                                                    + parseFloat(document.getElementById(celVal27).value)
                                                    + parseFloat(document.getElementById(celVal28).value)
                                                    + parseFloat(document.getElementById(celVal29).value)
                                                    + parseFloat(document.getElementById(celVal30).value)
                                                    + parseFloat(document.getElementById(celVal31).value)
                                                    + parseFloat(document.getElementById(celVal32).value)
                                                    + parseFloat(document.getElementById(celVal33).value)
                                                    + parseFloat(document.getElementById(celVal34).value)
                                                    + parseFloat(document.getElementById(celVal35).value);
 
        TotalPasivoPatrimonioT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin);
        return false;
    }


    function PasivoLargoPlazoT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin, celVal36, celVal37, celVal38, celVal39, celVal40,
                                    celVal41) {

        if (isNaN(document.getElementById(celVal36).value) || document.getElementById(celVal36).value == "") {
            document.getElementById(celVal36).value = 0;
        }
        if (isNaN(document.getElementById(celVal37).value) || document.getElementById(celVal37).value == "") {
            document.getElementById(celVal37).value = 0;
        }
        if (isNaN(document.getElementById(celVal38).value) || document.getElementById(celVal38).value == "") {
            document.getElementById(celVal38).value = 0;
        }
        if (isNaN(document.getElementById(celVal39).value) || document.getElementById(celVal39).value == "") {
            document.getElementById(celVal39).value = 0;
        }
        if (isNaN(document.getElementById(celVal40).value) || document.getElementById(celVal40).value == "") {
            document.getElementById(celVal40).value = 0;
        }
        if (isNaN(document.getElementById(celVal41).value) || document.getElementById(celVal41).value == "") {
            document.getElementById(celVal41).value = 0;
        }
   

        document.getElementById(celPasLargoPlazo).value = parseFloat(document.getElementById(celVal36).value)
                                                    + parseFloat(document.getElementById(celVal37).value)
                                                    + parseFloat(document.getElementById(celVal38).value)
                                                    + parseFloat(document.getElementById(celVal39).value)
                                                    + parseFloat(document.getElementById(celVal40).value)
                                                    + parseFloat(document.getElementById(celVal41).value) ;

        TotalPasivoPatrimonioT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin);
        return false;
    }


    function PatrimonioT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin, celVal42, celVal43, celVal44, celVal45, celVal46,
                                    celVal47) {

        if (isNaN(document.getElementById(celVal42).value) || document.getElementById(celVal42).value == "") {
            document.getElementById(celVal42).value = 0;
        }
        if (isNaN(document.getElementById(celVal43).value) || document.getElementById(celVal43).value == "") {
            document.getElementById(celVal43).value = 0;
        }
        if (isNaN(document.getElementById(celVal44).value) || document.getElementById(celVal44).value == "") {
            document.getElementById(celVal44).value = 0;
        }
        if (isNaN(document.getElementById(celVal45).value) || document.getElementById(celVal45).value == "") {
            document.getElementById(celVal45).value = 0;
        }
        if (isNaN(document.getElementById(celVal46).value) || document.getElementById(celVal46).value == "") {
            document.getElementById(celVal46).value = 0;
        }
        if (isNaN(document.getElementById(celVal47).value) || document.getElementById(celVal47).value == "") {
            document.getElementById(celVal47).value = 0;
        }


        document.getElementById(celTotalPatrimonio).value = parseFloat(document.getElementById(celVal42).value)
                                                    + parseFloat(document.getElementById(celVal43).value)
                                                    + parseFloat(document.getElementById(celVal44).value)
                                                    + parseFloat(document.getElementById(celVal45).value)
                                                    + parseFloat(document.getElementById(celVal46).value)
                                                    + parseFloat(document.getElementById(celVal47).value);

        TotalPasivoPatrimonioT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin);
        return false;
    }


    function TotalPasivoPatrimonioT(celPasCirculante, celPasLargoPlazo, celTotalPatrimonio, celTotalPasPatrimonio, celInteresMin) {

        if (isNaN(document.getElementById(celPasCirculante).value) || document.getElementById(celPasCirculante).value == "") {
            document.getElementById(celPasCirculante).value = 0;
        }
        if (isNaN(document.getElementById(celPasLargoPlazo).value) || document.getElementById(celPasLargoPlazo).value == "") {
            document.getElementById(celPasLargoPlazo).value = 0;
        }

        if (isNaN(document.getElementById(celInteresMin).value) || document.getElementById(celInteresMin).value == "") {
            document.getElementById(celInteresMin).value = 0;
        }
        if (isNaN(document.getElementById(celTotalPatrimonio).value) || document.getElementById(celTotalPatrimonio).value == "") {
            document.getElementById(celTotalPatrimonio).value = 0;
        }

        document.getElementById(celTotalPasPatrimonio).value = parseFloat(document.getElementById(celPasCirculante).value)
            + parseFloat(document.getElementById(celPasLargoPlazo).value)
            + parseFloat(document.getElementById(celInteresMin).value)
            + parseFloat(document.getElementById(celTotalPatrimonio).value);

        return false;
    }
