//****************************Operaciones*********************************//

function validarTxtCkb(obj, cad) {
    if (obj.checked == true) {
        document.getElementById(cad + 'ddlFogapeVigente').disabled = true;
    } else {
        document.getElementById(cad + 'ddlFogapeVigente').disabled = false;
    }
}

function ValidaAmortizacion(cad) {
    document.getElementById(cad + 'ddlTipoAmortizacion').disabled = false;

    if (document.getElementById(cad + 'ddlFinalidad').selectedIndex == 4) {
        document.getElementById(cad + 'ddlTipoAmortizacion').selectedIndex = 6;
        document.getElementById(cad + 'ddlTipoAmortizacion').disabled = true;
        document.getElementById(cad + 'txtNroCuotas').value = 1;
        document.getElementById(cad + 'txtNroCuotas').disabled = true;
    }

    if (document.getElementById(cad + 'ddlProducto').selectedIndex == 2) {
        document.getElementById(cad + 'ddlTipoCredito').selectedIndex = 5;
        document.getElementById(cad + 'ddlTipoCredito').disabled = true;
    }
}

function LimpiarOperacion(idPanel) {

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


//****************************Generales*********************************//

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

function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            return false
    }
}

function valMaximo(cad, nombreCampo, valor) {
    FormatoMoneda(cad + nombreCampo);
    var val = document.getElementById(cad + nombreCampo).value;
    document.getElementById(cad + nombreCampo).value = val.replace(/[.]/gi, '');
    document.getElementById(cad + nombreCampo).style.borderColor = "";
    if (parseFloat(document.getElementById(cad + nombreCampo).value) > parseFloat(valor)) {
        document.getElementById(cad + nombreCampo).focus();
        document.getElementById(cad + nombreCampo).style.borderColor = "red";
    }
}

function solonumerosPorc(e, id) {
    var val = document.getElementById(id).value;
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode == 44) {
        val = val.replace(/[,]/gi, '');
        document.getElementById(id).value = val;
    }
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57) {
            if (unicode != 44)
                return false
        }
    }
}

function validarFecha(string) { //string estará en formato dd/mm/yyyy (dí¬as < 32 y meses < 13)
    var ExpReg = /^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$/;
    return (ExpReg.test(string));
}

function solonumerosDV(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 107 && unicode != 75)
                return false
    }
}

function solonumerosFechas(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 45)
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
    return true;
}

function FormatoMonedaOperacion(nStr, campoOpeCLP, campoMoneda) {
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
    calculoOperacionCLP(nStr, campoOpeCLP, campoMoneda);

}

function FormatoMonedaOperacionTodo(nStr, campoMoneda) {
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

function calculoOperacionCLP(campomontoOperacion, campoOpeCLP, campoMoneda) {
    //var val = document.getElementById(campoOpeCLP).value;
    CalcularConversion(campomontoOperacion, campoOpeCLP, campoMoneda);
}

var coperacion;
var coperacionCLP;
var cMoneda;
function CalcularConversion(campomontoOperacion, campoOpeCLP, campoMoneda) {
    coperacion = campomontoOperacion;
    coperacionCLP = campoOpeCLP;
    cMoneda = campoMoneda;
    var query;

    var f = new Date();
    query = '<View><Query><Where><Eq><FieldRef Name="Fecha"/><Value Type="DateTime" IncludeTimeValue="FALSE"><Today /></Value></Eq></Where></Query></View>';
    lista = 'ValorMoneda';
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
        var valor;
        if (document.getElementById(cMoneda).options[document.getElementById(cMoneda).selectedIndex].text == 'CLP') {
            valor = 1;
        }
        if (document.getElementById(cMoneda).options[document.getElementById(cMoneda).selectedIndex].text == 'EUR') {
            valor = oListItem.get_item('MontoEuro');
        }

        if (document.getElementById(cMoneda).options[document.getElementById(cMoneda).selectedIndex].text == 'UF') {
            valor = oListItem.get_item('MontoUF');
        }

        if (valor == null || isNaN(valor) || valor == '')
            valor = 0;

        document.getElementById(coperacionCLP).value = parseFloat(document.getElementById(coperacion).value.replace(/[.]/gi, '').replace(/[,]/gi, '.')) * parseFloat(valor);

        if (document.getElementById(cMoneda).options[document.getElementById(cMoneda).selectedIndex].text == 'USD') {
            valor = oListItem.get_item('MontoUSD');
            if (valor == null || isNaN(valor) || valor == '' || valor == '0')
                document.getElementById(coperacionCLP).value = 0;
            else
                document.getElementById(coperacionCLP).value = parseFloat(document.getElementById(coperacion).value.replace(/[.]/gi, '').replace(/[,]/gi, '.')) * parseFloat(valor);

        }

        document.getElementById(coperacionCLP).value = document.getElementById(coperacionCLP).value;
        document.getElementById(coperacionCLP).value = Math.floor(parseFloat(document.getElementById(coperacionCLP).value));
        document.getElementById(coperacionCLP).value = document.getElementById(coperacionCLP).value.replace(/[.]/gi, ',');

        FormatoMoneda(coperacionCLP)
        return true;
    }
}

function onQueryFailed(sender, args) {
    alert('Ha Ocurrido un error. Favor recargue el formulario.');
}
