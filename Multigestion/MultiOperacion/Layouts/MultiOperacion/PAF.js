

function CambiaTipo(or, de) {
    if (parseInt(document.getElementById(or).value) > 12)
        document.getElementById(de).value = 'Largo Plazo';
    else
        document.getElementById(de).value = 'Corto Plazo';
}

function FormatoMoneda(nStr) {
    var val = document.getElementById(nStr).value;
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

function Adjuntar(muestra) {
    var ficha = document.getElementById(muestra);
    document.getElementById('Botones').style.display = "none";
    ficha.style.display = "none";
    document.getElementById('DivArchivo').style.display = "block";
    return false;
}

function AsignarComite(valor, etiqueta) {
    var e = document.getElementById(valor);
    document.getElementById(etiqueta).innerHTML = e.options[e.selectedIndex].text;
}
function imprSelec(muestra) {
    //document.getElementById(muestra).display = none;
    //document.getElementById('imprimir2').display = block;

    var ficha = document.getElementById(muestra);
    ficha.style.display = "block";
    var ventimp = window.open(' ', 'popimpr');
    ventimp.document.write(ficha.innerHTML);
    ventimp.document.close();
    ventimp.print();
    ventimp.close();
    ficha.style.display = "none";
    return false;
}


function SumatoriaVaciado(celdaevento, celda, prefijo, numOpe, numGar, moneda) {
    var ind = celdaevento.indexOf("Comercial") != -1
    var preOpe = numOpe + 2;
    var preGar = numGar + 2;
    var cadpre = "";
    var garanp = "";




    if (preOpe < 10) {
        cadpre = prefijo + 'Propuesta_ctl0' + preOpe.toString() + "_";
      
    }
    else {
        cadpre = prefijo + 'Propuesta_ctl' + preOpe.toString() + "_";
      
    }

    if (preGar < 10) {
        garanp = prefijo + 'Garantias_ctl0' + preGar.toString() + "_";
       
    }
    else {
        garanp = prefijo + 'Garantias_ctl' + preGar.toString() + "_";

    }



    if (isNaN(parseFloat(document.getElementById(celdaevento).value)) || document.getElementById(celdaevento).value == "") {
        document.getElementById(celdaevento).value = 0;
    }
    if (isNaN(parseFloat(document.getElementById(celda).value)) || document.getElementById(celda).value == "") {
        document.getElementById(cadpre + 'TotalCLP3').value = 0;
        document.getElementById(cadpre + 'TotalUS3').value = 0;
        document.getElementById(cadpre + 'TotalUFO3').value = 0;
    }
    var auxTotal;
    if (moneda == 'CLP') {
        auxTotal = document.getElementById(cadpre + 'TotalCLP3').value;
        document.getElementById(cadpre + 'TotalCLP3').value = parseFloat(document.getElementById(celdaevento).value) + parseFloat(auxTotal);
    
        //document.getElementById(cadpre + 'TotalUS3').value = (Math.round((parseFloat(document.getElementById(cadpre + 'TotalCLP3').value) / parseFloat(document.getElementById(prefijo + 'txtDOLAR').value)) * 100) / 100).toFixed(2);
        document.getElementById(cadpre + 'TotalUF3').value = (((parseFloat(document.getElementById(cadpre + 'TotalCLP3').value) / parseFloat(document.getElementById(prefijo + 'txtUF').value)) * 100) / 100).toFixed(3);
       
    } else if (moneda == 'USD') {
        auxTotal = document.getElementById(cadpre + 'TotalUS3').value;
        document.getElementById(cadpre + 'TotalUS3').value = parseFloat(document.getElementById(celdaevento).value) + parseFloat(auxTotal);
    
        //document.getElementById(cadpre + 'TotalUS3').value = (Math.round((parseFloat(document.getElementById(cadpre + 'TotalCLP3').value) / parseFloat(document.getElementById(prefijo + 'txtDOLAR').value)) * 100) / 100).toFixed(2);
        document.getElementById(cadpre + 'TotalUF3').value = (((parseFloat(document.getElementById(cadpre + 'TotalUS3').value) / parseFloat(document.getElementById(prefijo + 'txtUF').value)) * 100) / 100).toFixed(3);

    } else if (moneda == 'UF') {
        auxTotal = document.getElementById(cadpre + 'TotalUFO3').value;
        document.getElementById(cadpre + 'TotalUFO3').value = parseFloat(document.getElementById(celdaevento).value) + parseFloat(auxTotal);
      
        //document.getElementById(cadpre + 'TotalUS3').value = (Math.round((parseFloat(document.getElementById(cadpre + 'TotalCLP3').value) / parseFloat(document.getElementById(prefijo + 'txtDOLAR').value)) * 100) / 100).toFixed(2);
        document.getElementById(cadpre + 'TotalUF3').value = (((parseFloat(document.getElementById(cadpre + 'TotalUFO3').value) / parseFloat(document.getElementById(prefijo + 'txtUF').value)) * 100) / 100).toFixed(3);
       
    }



    if (isNaN(parseFloat(document.getElementById(cadpre + 'TotalUF3').value)))
        document.getElementById(prefijo + 'clpoculto').value=0; 
    else  
        document.getElementById(prefijo + 'clpoculto').value = document.getElementById(cadpre + 'TotalUF3').value * parseFloat(document.getElementById(prefijo + 'txtUF').value);

    //document.getElementById(celdaevento).value = "";
    if (isNaN(parseFloat(document.getElementById(prefijo + 'clpoculto').value)) || (parseFloat(document.getElementById(prefijo + 'clpoculto').value)==0)) {
        document.getElementById(garanp + 'TotalCobertura0').value = 0;
        document.getElementById(garanp + 'TotalCobertura1').value = 0;
    } else {
        document.getElementById(garanp + 'TotalCobertura0').value = (((parseFloat(document.getElementById(prefijo + 'totalcomercialoculto').value) / parseFloat(document.getElementById(prefijo + 'clpoculto').value)) * 100)/100 ).toFixed(2);
        document.getElementById(garanp + 'TotalCobertura1').value = (((parseFloat(document.getElementById(prefijo + 'totalajustadooculto').value) / parseFloat(document.getElementById(prefijo + 'clpoculto').value)) * 100)/100 ).toFixed(2);
    }
   
    return false;

}

function vaciarCampo(celdaevento, celda, prefijo, numOpe, numGar, moneda) {
    var preOpe = numOpe + 2;
    var preGar = numGar + 2;
    var cadpre = "";
    var garanp = "";
    if (preOpe < 10)
        cadpre = prefijo + 'Propuesta_ctl0' + preOpe.toString() + "_";
    else
        cadpre = prefijo + 'Propuesta_ctl' + preOpe.toString() + "_";

    if (preGar < 10)
        garanp = prefijo + 'Garantias_ctl0' + preGar.toString() + "_";
    else
        garanp = prefijo + 'Garantias_ctl' + preGar.toString() + "_";


    if (isNaN(parseFloat(document.getElementById(celdaevento).value)) || document.getElementById(celdaevento).value == "") {
        document.getElementById(celdaevento).value = 0;
    }
    if (isNaN(parseFloat(document.getElementById(cadpre + 'TotalCLP3').value)) || document.getElementById(cadpre + 'TotalCLP3').value == "") {

        document.getElementById(cadpre + 'TotalCLP3').value = 0;
        //document.getElementById(cadpre + 'TotalUS3').value = 0;
        //document.getElementById(cadpre + 'TotalUFO3').value = 0;
    }
    if (isNaN(parseFloat(document.getElementById(cadpre + 'TotalUS3').value)) || document.getElementById(cadpre + 'TotalUS3').value == "") {

        //document.getElementById(cadpre + 'TotalCLP3').value = 0;
        document.getElementById(cadpre + 'TotalUS3').value = 0;
        //document.getElementById(cadpre + 'TotalUFO3').value = 0;
    }
    if (isNaN(parseFloat(document.getElementById(cadpre + 'TotalUFO3').value)) || document.getElementById(cadpre + 'TotalUFO3').value == "") {

        //document.getElementById(cadpre + 'TotalCLP3').value = 0;
        //document.getElementById(cadpre + 'TotalUS3').value = 0;
        document.getElementById(cadpre + 'TotalUFO3').value = 0;
    }
    var auxInd = document.getElementById(celdaevento).value;
    var auxTotal;
    if (moneda == 'CLP') {
        auxTotal = document.getElementById(cadpre + 'TotalCLP3').value;
        document.getElementById(cadpre + 'TotalCLP3').value = parseFloat(auxTotal) - parseFloat(auxInd);
        //parseFloat(document.getElementById(celdaevento).value) - parseFloat(auxTotal);
        //document.getElementById(cadpre + 'TotalUS3').value = (Math.round((parseFloat(document.getElementById(cadpre + 'TotalCLP3').value) / parseFloat(document.getElementById(prefijo + 'txtDOLAR').value)) * 100) / 100).toFixed(2);
        document.getElementById(cadpre + 'TotalUF3').value = (((parseFloat(document.getElementById(cadpre + 'TotalCLP3').value) / parseFloat(document.getElementById(prefijo + 'txtUF').value)) * 100) / 100).toFixed(3);
    } else if (moneda == 'USD') {
        auxTotal = document.getElementById(cadpre + 'TotalUS3').value;
        document.getElementById(cadpre + 'TotalUS3').value = parseFloat(auxTotal) - parseFloat(auxInd);
        //parseFloat(document.getElementById(celdaevento).value) - parseFloat(auxTotal);
        //document.getElementById(cadpre + 'TotalUSD3').value = (Math.round((parseFloat(document.getElementById(cadpre + 'TotalUSD3').value) / parseFloat(document.getElementById(prefijo + 'txtDOLAR').value)) * 100) / 100).toFixed(2);
        document.getElementById(cadpre + 'TotalUF3').value = (((parseFloat(document.getElementById(cadpre + 'TotalUS3').value) / parseFloat(document.getElementById(prefijo + 'txtUF').value)) * 100) / 100).toFixed(3);
    } else {
        auxTotal = document.getElementById(cadpre + 'TotalUFO3').value;
        document.getElementById(cadpre + 'TotalUFO3').value = parseFloat(auxTotal) - parseFloat(auxInd);
        //parseFloat(document.getElementById(celdaevento).value) - parseFloat(auxTotal);
        //document.getElementById(cadpre + 'TotalUSD3').value = (Math.round((parseFloat(document.getElementById(cadpre + 'TotalUF3').value) / parseFloat(document.getElementById(prefijo + 'txtDOLAR').value)) * 100) / 100).toFixed(2);
        document.getElementById(cadpre + 'TotalUF3').value = (((parseFloat(document.getElementById(cadpre + 'TotalUFO3').value) / parseFloat(document.getElementById(prefijo + 'txtUF').value)) * 100) / 100).toFixed(3);
    }

    if (isNaN(parseFloat(document.getElementById(cadpre + 'TotalUF3').value)))
        document.getElementById(prefijo + 'clpoculto').value = 0;
    else
        document.getElementById(prefijo + 'clpoculto').value = document.getElementById(cadpre + 'TotalUF3').value * parseFloat(document.getElementById(prefijo + 'txtUF').value);

    document.getElementById(celdaevento).value = "";

    if (isNaN(parseFloat(document.getElementById(prefijo + 'clpoculto').value)) || (parseFloat(document.getElementById(prefijo + 'clpoculto').value)==0)) {
        document.getElementById(garanp + 'TotalCobertura0').value
        document.getElementById(garanp + 'TotalCobertura1').value = 0;
    } else {
        document.getElementById(garanp + 'TotalCobertura0').value = (((parseFloat(document.getElementById(prefijo + 'totalcomercialoculto').value) / parseFloat(document.getElementById(prefijo + 'clpoculto').value)) * 100) / 100).toFixed(2);
        document.getElementById(garanp + 'TotalCobertura1').value = (((parseFloat(document.getElementById(prefijo + 'totalajustadooculto').value) / parseFloat(document.getElementById(prefijo + 'clpoculto').value)) * 100) / 100).toFixed(2);
    }
    return false;

}

function SumatoriaText(celda) {

    //alert(celda);
    //alert(document.getElementById(celda).value);
    if (document.getElementById(celda).value == "") {
        document.getElementById(celda).value = 0;
    } else {
        document.getElementById(celda).value = parseInt(document.getElementById(celda).value) + 1;
    }
    return false;
}


function Dialogo() {
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por favor espere...', 'procesando su operación', 120, 550);
    return true;
}

function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    //alert(unicode);
    if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
        if ((unicode < 46 || unicode > 57) && unicode != 47) //if not a number and .
            return false //disable key press
    }
}

function solonumerossindecimal(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    //alert(unicode);
    if (unicode != 8) { //if the key isn't the backspace key (which we should allow)
        if (unicode < 46 || unicode > 57) //if not a number and .
            return false //disable key press
    }
}

function Vaciar(celdaevento) {
    document.getElementById(celdaevento).value = "";
    return false;
}

function AsignarCero(celda) {

    //alert(celda);
    //alert(document.getElementById(celda).value);
    if (document.getElementById(celda).value == "") {
        document.getElementById(celda).value = 0;
    }
}

function VerificaEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
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

        for (var j = 0; j < caracteres.length; j++) {
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


function vacio(string) {
    if (string != '')
        return false;
    return true;
}

function ValidarSeleccion(cad) {
    //alert('sdf');
    var filedocumento = cad + 'fileDocumento';
    var ddltipo = cad + 'ddlTipo';
    document.getElementById(filedocumento).style.borderColor = "";
    document.getElementById(ddltipo).style.borderColor = "";
    //var vfiledocumento = document.getElementById(filedocumento).value;
    var vddltipo = document.getElementById(ddltipo).value;
    var vArchivo = document.getElementById(filedocumento).value;
    //var vNombreArchivo = document.getElementById(idNombreArchivo).innerText;


    var filedoc, ddltip;
    if (vacio(vArchivo)) {
        document.getElementById(filedocumento).style.borderColor = "red";
        filedoc = false;
    } else {
        filedoc = true;
        document.getElementById(filedocumento).style.borderColor = "";
    }
    if (document.getElementById(ddltipo).options[document.getElementById(ddltipo).selectedIndex].text == 'Seleccione') {
        document.getElementById(ddltipo).style.borderColor = "red";
        //document.getElementById(ddltipo).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        ddltip = false;
    } else {
        ddltip = true;
        document.getElementById(ddltipo).style.borderColor = "";
        //document.getElementById(ddltipo).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }


    if (ddltip && filedoc) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        //waitDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose('Registro de Campaña', 'Por favor espere mientras procesamos su operación...', 100, 550);
        return true;
    } else
        return false;

}
function Validar(cad) {
    // var idRut = cad + 'txt_rut1';
    // var idLRut = cad + 'txt_divRut';
    var idApellidos = cad + 'txt_apellidos';
    var idNombre = cad + 'txt_nombre';
    var idFijo = cad + 'txt_fijo';
    var idCelular = cad + 'txt_celular';
    var idCargo = cad + 'txt_cargo';
    var idEmail = cad + 'txt_email';

    //document.getElementById(idRut).style.borderColor = "";
    //document.getElementById(idLRut).style.borderColor = "";
    document.getElementById(idApellidos).style.borderColor = "";
    document.getElementById(idNombre).style.borderColor = "";
    document.getElementById(idFijo).style.borderColor = "";
    document.getElementById(idCelular).style.borderColor = "";
    document.getElementById(idCargo).style.borderColor = "";
    document.getElementById(idEmail).style.borderColor = "";

    //var vrut = document.getElementById(idRut).value;
    // var lrut = document.getElementById(idLRut).value;
    var vapellidos = document.getElementById(idApellidos).value;
    var vnombre = document.getElementById(idNombre).value;
    var vfijo = document.getElementById(idFijo).value;
    var vcelular = document.getElementById(idCelular).value;
    var vcargo = document.getElementById(idCargo).value;
    var vemail = document.getElementById(idEmail).value;

    var rut = true, apellidos, nombre, fijo, celular, cargo, email;

    //if (!VerificaRut(vrut + '-' + lrut)) {
    //    document.getElementById(idRut).style.borderColor = "red";
    //    document.getElementById(idLRut).style.borderColor = "red";
    //    //document.getElementById('errorNombre').innerHTML = "requerido";
    //    rut = false;
    //} else {
    //    rut = true;
    //    document.getElementById(idRut).style.borderColor = "";
    //    document.getElementById(idLRut).style.borderColor = "";
    //    // document.getElementById('errorNombre').innerHTML = "";
    //}

    if (vacio(vnombre)) {
        document.getElementById(idNombre).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        nombre = false;
    } else {
        nombre = true;
        document.getElementById(idNombre).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }
    if (vacio(vfijo)) {
        document.getElementById(idFijo).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        fijo = false;
    } else {
        fijo = true;
        document.getElementById(idFijo).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }
    if (vacio(vcargo)) {
        document.getElementById(idCargo).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        cargo = false;
    } else {
        cargo = true;
        document.getElementById(idCargo).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }
    if (vacio(vcelular)) {
        document.getElementById(idCelular).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        celular = false;
    } else {
        celular = true;
        document.getElementById(idcelular).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }
    if (vacio(vapellidos)) {
        document.getElementById(idApellidos).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        apellido = false;
    } else {
        apellido = true;
        document.getElementById(idApellidos).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }
    if (!VerificaEmail(vemail)) {
        document.getElementById(idEmail).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        email = false;
    } else {
        email = true;
        document.getElementById(idEmail).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }
    if (apellidos && nombre && fijo && celular && cargo && email) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        //waitDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose('Registro de Campaña', 'Por favor espere mientras procesamos su operación...', 100, 550);
        return true;
    } else
        return false;
}

function mascara(o, f) {
    v_obj = o;
    v_fun = f;
    setTimeout("execmascara()", 1);
}

function execmascara() {
    v_obj.value = v_fun(v_obj.value);
}

function cpf(v) {
    v = v.replace(/([^0-9\.]+)/g, '');
    v = v.replace(/^[\.]/, '');
    v = v.replace(/[\.][\.]/g, '');
    v = v.replace(/\.(\d)(\d)(\d)/g, '.$1$2');
    v = v.replace(/\.(\d{1,2})\./g, '.$1');
    v = v.toString().split('').reverse().join('').replace(/(\d{3})/g, '$1,');
    v = v.split('').reverse().join('').replace(/^[\,]/, '');
    return v;
}

function SoloLectura(e) {
    // var unicode = e.charCode ? e.charCode : e.keyCode

    return false


}




