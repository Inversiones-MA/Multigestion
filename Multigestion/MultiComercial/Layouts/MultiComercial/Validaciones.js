document.write("<script type='text/javascript' src='../../_layouts/15/MultiComercial/accounting.js'></script>");

var codMon;
var simbolo;
var cantDec;
var sepM = '.';
var sepD = ',';
var validoTel = false;

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

function validar(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    tecla = String.fromCharCode(tecla)
    return /^[0-9\-]+$/.test(tecla);
}

function Dialogo() {
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    return true;
}

function format(input) {
    var obj = input.id;
    var numero = document.getElementById(obj);

    var num = numero.value.replace(/\./g, '');

    if (!isNaN(num)) {
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split('').reverse().join('').replace(/^[\.]/, '');
        numero.value = num;
        numero.style.borderColor = "";
    }
    else {
        numero.style.borderColor = "red";
        numero.value = numero.value.replace(/[^\d\.]*/g, '');
    }
}

function formatPhone(obj) {
    var tel = obj.id;
    var numero = document.getElementById(tel);
    var numTel = '(';

    if (isNumeric(obj)) {
        for (var i = 0; i < numero.value.length; i++) {
            if (i == 3)
                numTel = numTel + ') ' + numero.value[i];
            else
                numTel = numTel + numero.value[i];
        }

        numero.value = numTel.substring(0, 14);
    }
    else
        validoTel = true;
}

function isAlphaNumeric(val) {
    if (val.match(/^[kK0-9]+$/)) {
        return true;
    }
    else {
        return false;
    }
}

function isNumeric(val) {
    var num = val.id;
    var numero = document.getElementById(num);

    if (numero.value.match(/^[0-9]+$/)) {
        numero.style.borderColor = "";
        return true;
    }
    else {
        numero.style.borderColor = "red";
        return false;
    }
}

String.prototype.lpad = function (padString, length) {
    var str = this;
    while (str.length < length)
        str = padString + str;
    return str;
}

function VerificaRut(rut) {
    if (rut.toString().trim() != '' && rut.toString().indexOf('-') > 0) {
        var caracteres = new Array();
        var serie = new Array(2, 3, 4, 5, 6, 7);
        var dig = rut.toString().substr(rut.toString().length - 1, 1);
        rut = rut.toString().substr(0, rut.toString().length - 2);

        for (var i = 0; i < rut.length; i++) {
            caracteres[i] = parseInt(rut.charAt((rut.length - (i + 1))));
        }

        var sumatoria = 0;
        var k = 0;
        var resto = 0;

        for (var j = 0; j < caractelength; j++) {
            if (k == 6) {
                k = 0;
            }
            sumatoria += parseInt(caracteres[j]) * parseInt(serie[k]);
            k++;
        }

        resto = sumatoria % 11;
        dv = 11 - resto;

        if (dv == 10) {
            dv = "K";
        }
        else if (dv == 11) {
            dv = 0;
        }

        if (dv.toString().trim().toUpperCase() == dig.toString().trim().toUpperCase())
            return true;
        else
            return false;
    }
    else {
        return false;
    }
}


function fnMoneda(obj, obj2) {
    var idM = obj2.id;
    var idMoneda;

    if (idM == undefined)
        idMoneda = document.getElementById(obj2);
    else
        idMoneda = document.getElementById(idM);

    var mon = obj.id;
    var moneda = document.getElementById(mon);

    if ((simbolo == undefined) || (cantDec == undefined))
        retrieveAllListsAllFields(obj2);

    if ((moneda != null) && (moneda.value != "") && (idMoneda != null)) {
        codMon = idMoneda.value;
        var result = accounting.formatMoney(moneda.value, simbolo, cantDec, sepM, sepD, '%s %v');
        moneda.value = result;
    }
}

function fnDecimal(obj) {
    var dec = obj.id;
    var decim = document.getElementById(dec);

    if (decim != null) {
        var result = accounting.formatNumber(decim.value, 2, sepM, sepD);
        decim.value = result;
    }
}

function retrieveAllListsAllFields(obj) {
    var codMon = obj.id;
    var codMoneda;
    var query;

    if (codMon != undefined)
        codMoneda = document.getElementById(codMon);
    else
        codMoneda = document.getElementById(obj);

    query = "<View><Query><Where><Eq><FieldRef Name='Title'/>" + "<Value Type='Text'>" + codMoneda.value + "</Value></Eq></Where></Query></View>";
    lista = 'Monedas';
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
        codMon = oListItem.get_item('Title')
        simbolo = oListItem.get_item('Abreviacion')

        if (simbolo == 'CLP')
            simbolo = "$";

        cantDec = oListItem.get_item('CantDecimales');
    }
}

function onQueryFailed(sender, args) {
    alert('Error al obtener los datos de la lista');
}

function ModifyEnterKeyPressAsTab() {
    if (window.event && window.event.keyCode == 13) {
        window.event.keyCode = 9;
    }
}

function fnCambiaColor(obj) {
    var objeto = document.getElementById(obj);

    if (!validoTel) {
        if (objeto != null) {

            if (objeto.value != '')
                objeto.style.borderColor = "";
        }
        else {
            if (obj.value != '')
                obj.style.borderColor = "";
        }
    }
}

function fnOcultaDivs(obj, obj1, obj2) {
    var objeto = document.getElementById(obj);
    var objeto1 = document.getElementById(obj1);
    var objeto2 = document.getElementById(obj2);

    if (objeto != null)
        objeto.style.display = 'none';
    else
        obj.style.display = 'none';

    if (objeto1 != null)
        objeto1.style.display = 'none';
    else
        obj1.style.display = 'none';

    if (objeto2 != null)
        objeto2.style.display = 'none';
    else
        obj2.style.display = 'none';
}


function ValidarIngresoOperacion(cad, idFecha) {

    var idPlazo = cad + 'txtPlazo';
    var idTipoMoneda = cad + 'ddlTipoM';
    var idProducto = cad + 'ddlProducto';
    var idMontoOp = cad + 'txtMntOper';
    var idFinalidad = cad + 'ddlFinalidad';
    var idFechaCierre = cad + 'idFecha';

    document.getElementById(idPlazo).style.borderColor = "";
    document.getElementById(idTipoMoneda).style.borderColor = "";
    document.getElementById(idProducto).style.borderColor = "";
    document.getElementById(idMontoOp).style.borderColor = "";
    document.getElementById(idFinalidad).style.borderColor = "";
    document.getElementById(idFecha).style.borderColor = "";

    var Plazo = document.getElementById(idPlazo).value;
    var TipoMoneda = document.getElementById(idTipoMoneda).value;
    var Producto = document.getElementById(idProducto).value;
    var MontoOp = document.getElementById(idMontoOp).value;
    var Finalidad = document.getElementById(idFinalidad).value;
    var FechaCierre = document.getElementById(idFechaCierre).value;

    var aux = true;

    if (vacioPlazo(Plazo)) {
        document.getElementById(idPlazo).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idPlazo).style.borderColor = "";
    }

    if (vacioDdl(TipoMoneda)) {
        document.getElementById(idTipoMoneda).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idTipoMoneda).style.borderColor = "";
    }


    if (vacioDdl(Producto)) {
        document.getElementById(idProducto).style.borderColor = "red";
        aux = false;
    } else {

        document.getElementById(idProducto).style.borderColor = "";
    }

    if (vacio(MontoOp)) {
        document.getElementById(idMontoOp).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idMontoOp).style.borderColor = "";
    }

    if (vacioDdl(Finalidad)) {
        document.getElementById(idFinalidad).style.borderColor = "red";
        aux = false;

    } else {
        document.getElementById(idFinalidad).style.borderColor = "";
    }

    if (EsFecha(Fecha)) {
        document.getElementById(idFecha).style.borderColor = "red";
        aux = false;
    } else {
        if (Fecha <= Date.now()) {
            document.getElementById(idFecha).style.borderColor = "red";
            aux = false;
        }
        else
           document.getElementById(idFecha).style.borderColor = "";
    }

    if (aux) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}


function ValidarEdicionEmpresa(cad) {
    //var idTxtRut = cad + 'lblRut';
    //var idTxtDv = cad + 'lblDv';
    var idTxtEmpleados = cad + 'txtEmpleados';
    var idTxtNombre = cad + 'txtNombreEmpresa';
    var idDdlActividad = cad + 'ddlActividad';
    var idDdlProvincia = cad + 'ddlProvincia';
    var idTxtDir = cad + 'txtDir';
    //var idTxtComplemento = cad + 'txtComplemento';
    var idDdlTipoE = cad + 'ddlTipoE';
    var idTxtMailC = cad + 'txtMailC';
    var idDdlRegion = cad + 'ddlRegion';
    var idDdlComunas = cad + 'ddlComunas';
    //var idTxtNumero = cad + 'TxtNumero';
    var idDdlEjecutivo = cad + 'ddlEjecutivo';
    var idDdlRegion = cad + 'ddlRegion';

    //document.getElementById(idRut).style.borderColor = "";
    //document.getElementById(idDv).style.borderColor = "";
    document.getElementById(idTxtEmpleados).style.borderColor = "";
    document.getElementById(idTxtNombre).style.borderColor = "";
    document.getElementById(idDdlActividad).style.borderColor = "";
    document.getElementById(idDdlProvincia).style.borderColor = "";
    document.getElementById(idTxtDir).style.borderColor = "";
    //document.getElementById(idTxtComplemento).style.borderColor = "";
    document.getElementById(idDdlTipoE).style.borderColor = "";
    document.getElementById(idTxtMailC).style.borderColor = "";
    document.getElementById(idDdlRegion).style.borderColor = "";
    document.getElementById(idDdlComunas).style.borderColor = "";
    //document.getElementById(idTxtNumero).style.borderColor = "";
    document.getElementById(idDdlEjecutivo).style.borderColor = "";
    document.getElementById(idDdlRegion).style.borderColor = "";

    //var Rut = document.getElementById(idRut).value;
    //var Dv = document.getElementById(idDv).value;
    var TxtEmpleados = document.getElementById(idTxtEmpleados).value;
    var TxtNombre = document.getElementById(idTxtNombre).value;
    var DdlActividad = document.getElementById(idDdlActividad).value;
    var DdlProvincia = document.getElementById(idDdlProvincia).value;
    var TxtDir = document.getElementById(idTxtDir).value;
    //var TxtComplemento = document.getElementById(idTxtComplemento).value;
    var DdlTipoE = document.getElementById(idDdlTipoE).value;
    var TxtMailC = document.getElementById(idTxtMailC).value;
    var DdlRegion = document.getElementById(idDdlRegion).value;
    var DdlComunas = document.getElementById(idDdlComunas).value;
    //var TxtNumero = document.getElementById(idTxtNumero).value;
    var DdlEjecutivo = document.getElementById(idDdlEjecutivo).value;
    var DdlRegion = document.getElementById(idDdlRegion).value;

    var aux = true;

    if (vacio(DdlTipoE)) {
        document.getElementById(idDdlTipoE).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDdlTipoE).style.borderColor = "";
    }

    if (vacio(TxtEmpleados)) {
        document.getElementById(idTxtEmpleados).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idTxtEmpleados).style.borderColor = "";
    }

    if (vacio(TxtNombre)) {
        document.getElementById(idTxtNombre).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idTxtNombre).style.borderColor = "";
    }

    if (!vacio(TxtMailC)) {
        if (!validarEmail(TxtMailC)) {
            document.getElementById(idTxtMailC).style.borderColor = "red";
            aux = false;
        } else {
            document.getElementById(idTxtMailC).style.borderColor = "";
        }
    } else {
        document.getElementById(idTxtMailC).style.borderColor = "red";
        aux = false;
    }

    if (vacioDdl(DdlActividad)) {
        document.getElementById(idDdlActividad).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDdlActividad).style.borderColor = "";
    }

    if (vacioDdl(DdlRegion)) {
        document.getElementById(idDdlRegion).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDdlRegion).style.borderColor = "";
    }

    if (vacioDdl(DdlProvincia)) {
        document.getElementById(idDdlProvincia).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDdlProvincia).style.borderColor = "";
    }

    if (vacioDdl(DdlComunas)) {
        document.getElementById(idDdlComunas).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDdlComunas).style.borderColor = "";
    }

    if (vacioDdl(TxtDir)) {
        document.getElementById(idTxtDir).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idTxtDir).style.borderColor = "";
    }

    //if (vacioDdl(TxtNumero)) {
    //    document.getElementById(idTxtNumero).style.borderColor = "red";
    //    aux = false;
    //} else {
    //    document.getElementById(idTxtNumero).style.borderColor = "";
    //}

    if (vacioDdl(DdlEjecutivo)) {
        document.getElementById(idDdlEjecutivo).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDdlEjecutivo).style.borderColor = "";
    }

    if (aux) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}

function ValidarIngresoEmpresa(cad, idFecha) {
    var idRut = cad + 'txtRut';
    var idDVRut = cad + 'txtDV';
    var idRazonSocial = cad + 'txtRazon';
    var idRutContacto = cad + 'txtRC';
    var idDVContacto = cad + 'txtDVC';
    var idNombres = cad + 'txtNombres';
    var idTelFijo = cad + 'txtFijo';
    var idCargo = cad + 'ddlCargos';
    var idEmail = cad + 'txtMail';
    var idCelular = 'txtCelu';
    var idEjecutivo = cad + 'ddlEjecutivo';
    var idRutContacto = cad + 'txt_rutContacto';
    var idDVRutContacto = cad + 'txt_divRutContacto';

    document.getElementById(idRut).style.borderColor = "";
    document.getElementById(idDVRut).style.borderColor = "";
    document.getElementById(idFecha).style.borderColor = "";
    document.getElementById(idRazonSocial).style.borderColor = "";
    document.getElementById(idNombres).style.borderColor = "";
    document.getElementById(idTelFijo).style.borderColor = "";
    document.getElementById(idCargo).style.borderColor = "";
    document.getElementById(idEmail).style.borderColor = "";
    document.getElementById(idEjecutivo).style.borderColor = "";
    document.getElementById(idRutContacto).style.borderColor = "";
    document.getElementById(idDVRutContacto).style.borderColor = "";
    document.getElementById(idCelular).style.borderColor = "";

    var Rut = document.getElementById(idRut).value;
    var DVRut = document.getElementById(idDVRut).value;
    var Fecha = document.getElementById(idFecha).value;
    var RazonSocial = document.getElementById(idRazonSocial).value;
    var Nombres = document.getElementById(idNombres).value;
    var TelFijo = document.getElementById(idTelFijo).value;
    var cargoContacto = document.getElementById(idCargo).value;
    var Email = document.getElementById(idEmail).value;
    var Ejecutivo = document.getElementById(idEjecutivo).value;
    var RutContacto = document.getElementById(idRutContacto).value;
    var DVRutContacto = document.getElementById(idDVRutContacto).value;
    var Celular = document.getElementById(idCelular).value;

    var aux = true;

    if (!VerificaRut(Rut + '-' + DVRut)) {
        document.getElementById(idRut).style.borderColor = "red";
        document.getElementById(idDVRut).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idRut).style.borderColor = "";
        document.getElementById(idDVRut).style.borderColor = "";
    }

    if (!VerificaRut(RutContacto + '-' + DVRutContacto)) {
        document.getElementById(idRutContacto).style.borderColor = "red";
        document.getElementById(idDVRutContacto).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idRutContacto).style.borderColor = "";
        document.getElementById(idDVRutContacto).style.borderColor = "";
    }

    if (vacio(RazonSocial)) {
        document.getElementById(idRazonSocial).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idRazonSocial).style.borderColor = "";
    }

    if (vacio(Nombres)) {
        document.getElementById(idNombres).style.borderColor = "red";
        aux = false;
    } else {

        document.getElementById(idNombres).style.borderColor = "";
    }

    if (vacio(TelFijo)) {
        document.getElementById(idTelFijo).style.borderColor = "red";
        aux = false;

    } else {
        document.getElementById(idTelFijo).style.borderColor = "";
    }

    if (vacioDdl(cargoContacto)) {
        document.getElementById(idCargo).style.borderColor = "red";
        aux = false;
    } else {

        document.getElementById(idCargo).style.borderColor = "";
    }

    if (vacio(Celular)) {
        //document.getElementById(idCelular).style.borderColor = "red";
        //aux = false;
    } else {
        if (valCel(Celular)) {

        }
    }

    if (!EsFecha(Fecha)) {
        document.getElementById(idFecha).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idFecha).style.borderColor = "";
    }

    if (!validarEmail(Email)) {
        document.getElementById(idEmail).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idEmail).style.borderColor = "";
    }

    if (aux) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}

function valCel(string) {
    var telefono = document.getElementById('txtCelu').value;
   //var telefono = document.getElementById(string).value;
    if (telefono !== null && telefono !== '') {
        if (telefono.length < 9) {
            //$("#<%=txtCelu.ClientID%>").val('minimo 9 numeros');
            //$('#<%=txtCelu.ClientID%>').attr("placeholder", "formatro invalido");
            document.getElementById('txtCelu').value = "minimo 9 numeros";
            document.getElementById('txtCelu').style.borderColor = "red";
            return false;
        }
        else {
            //validar que primer numeros ea nueve
            var primerNumero = document.getElementById('txtCelu').value[0];
            if (primerNumero !== '9') {
                document.getElementById('txtCelu').value = "el primer numero debe ser 9";
                document.getElementById('txtCelu').style.borderColor = "red";
                return false;
            }
            else {
                document.getElementById(string).style.borderColor = "";
                return true;
            }
        }
    }
    else {
        document.getElementById(string).style.borderColor = "";
        return false;
    }
}



function ValidarGarantia(cad) {

    var idDescripcionPrenda = cad + 'txtDescP';
    var idInscripcion = cad + 'txtNroInscripcion';
    var idValorAjustado = cad + 'txtValorA';
    var idTipoGarantia = cad + 'ddlTipoG';
    var idTipoBienes = cad + 'ddlTipoBienes';
    var idValorComercial = cad + 'txtValorC';
    var idValorAsegurado = cad + 'txtValorAseg';
    var idEmpresaTasadora = cad + 'ddlEmpresaT';
    var idTasador = cad + 'DdlTasador';

    document.getElementById(idDescripcionPrenda).style.borderColor = "";
    document.getElementById(idInscripcion).style.borderColor = "";
    document.getElementById(idValorAjustado).style.borderColor = "";
    document.getElementById(idTipoGarantia).style.borderColor = "";
    document.getElementById(idTipoBienes).style.borderColor = "";
    document.getElementById(idValorComercial).style.borderColor = "";
    document.getElementById(idValorAsegurado).style.borderColor = "";

    //document.getElementById(idEmpresaTasadora).style.borderColor = "";
    //document.getElementById(idTasador).style.borderColor = "";

    var DescripcionPrenda = document.getElementById(idDescripcionPrenda).value;
    var Inscripcion = document.getElementById(idInscripcion).value;
    var ValorAjustado = document.getElementById(idValorAjustado).value;
    var TipoGarantia = document.getElementById(idTipoGarantia).value;
    var TipoBienes = document.getElementById(idTipoBienes).value;
    var ValorComercial = document.getElementById(idValorComercial).value;
    var ValorAsegurado = document.getElementById(idValorAsegurado).value;

    //var EmpresaTasadora = document.getElementById(idEmpresaTasadora).value;
    //var Tasador = document.getElementById(idTasador).value;


    var aux = true;

    if (vacio(DescripcionPrenda)) {
        document.getElementById(idDescripcionPrenda).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idDescripcionPrenda).style.borderColor = "";
    }

    if (vacio(Inscripcion)) {
        document.getElementById(idInscripcion).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idInscripcion).style.borderColor = "";
    }
    if (vacio(ValorAjustado)) {
        document.getElementById(idValorAjustado).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idValorAjustado).style.borderColor = "";
    }

    if (vacioDdl(TipoGarantia)) {
        document.getElementById(idTipoGarantia).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idTipoGarantia).style.borderColor = "";
    }

    if (vacioDdl(TipoBienes)) {
        document.getElementById(idTipoBienes).style.borderColor = "red";
        aux = false;
    } else {

        document.getElementById(idTipoBienes).style.borderColor = "";
    }

    //if (vacioDdl(EmpresaTasadora)) {
    //    document.getElementById(idEmpresaTasadora).style.borderColor = "red";
    //    aux = false;
    //} else {

    //    document.getElementById(idEmpresaTasadora).style.borderColor = "";
    //}

    //if (vacioDdl(Tasador)) {
    //    document.getElementById(idTasador).style.borderColor = "red";
    //    aux = false;
    //} else {

    //    document.getElementById(idTasador).style.borderColor = "";
    //}


    if (vacio(ValorComercial)) {
        document.getElementById(idValorComercial).style.borderColor = "red";
        aux = false;
    } else {
        document.getElementById(idValorComercial).style.borderColor = "";
    }

    if (vacio(ValorAsegurado)) {
        document.getElementById(idValorAsegurado).style.borderColor = "red";
        aux = false;

    } else {
        document.getElementById(idValorAsegurado).style.borderColor = "";
    }



    if (aux) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}


function vacio(string) {
    if (string != '')
        return false;
    return true;
}

function vacioDdl(string) {
    if (string != '' && string != '0')
        return false;
    return true;
}

function validarEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function EsFecha(fecha) {
    var ExpReg = /^([0][1-9]|[12][0-9]|3[01])(\/|-)([0][1-9]|[1][0-2])\2(\d{4})$/;
    return ExpReg.test(fecha);
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

function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)

            return false
    }
}

function solonumerosTelefono(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 40 && unicode != 41)
                return false
    }
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

function vacioPlazo(plazo) {
    if (!vacio(plazo)) {
        if(parseInt(plazo) >= 144)
           return true;
        else
           return false;
    }
    else
        return false;
}
