﻿
function valMaximo(cad, nombreCampo, valor) {
    document.getElementById(cad + nombreCampo).style.borderColor = "";
    if (parseFloat(document.getElementById(cad + nombreCampo).value) > parseFloat(valor)) {
        document.getElementById(cad + nombreCampo).focus();
        document.getElementById(cad + nombreCampo).style.borderColor = "red";
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
function Validar(cad) {

    var idNombre = cad + 'txt_nombre';
    var idRut = cad + 'txt_rut';
    var idTipo = cad + 'txt_tipo';
    var idPatrimonio = cad + 'txt_patrimonio';
    var idLRut = cad + 'txt_divRut';
    
 

    document.getElementById(idNombre).style.borderColor = "";
    document.getElementById(idRut).style.borderColor = "";
    document.getElementById(idTipo).style.borderColor = "";
    document.getElementById(idPatrimonio).style.borderColor = "";
    document.getElementById(idLRut).style.borderColor = "";


    var vnombre = document.getElementById(idNombre).value;
    var vrut = document.getElementById(idRut).value;
    var vtipo = document.getElementById(idTipo).value;
    var vpatrimonio = document.getElementById(idPatrimonio).value;
    var lrut = document.getElementById(idLRut).value;

    var nombre, rut, tipo, patrimonio;

    if (vacio(vnombre)) {
        document.getElementById(idNombre).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        nombre = false;
    } else {
        nombre = true;
        document.getElementById(idNombre).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }


   

    if (!VerificaRut(vrut + '-' + lrut)) {
        document.getElementById(idRut).style.borderColor = "red";
        document.getElementById(idLRut).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        rut = false;
    } else {
        rut = true;
        document.getElementById(idRut).style.borderColor = "";
        document.getElementById(idLRut).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }



    if (vacio(vtipo)) {
        document.getElementById(idTipo).style.borderColor = "red";
        // document.getElementById('errorParticipacion').innerHTML = "requerido";
        tipo = false;
    } else {
        tipo = true;
        document.getElementById(idTipo).style.borderColor = "";
        //document.getElementById('errorParticipacion').innerHTML = "";
    }

    if (vacio(vpatrimonio)) {
        document.getElementById(idPatrimonio).style.borderColor = "red";
        // document.getElementById('errorParticipacion').innerHTML = "requerido";
        patrimonio = false;
    } else {
        patrimonio = true;
        document.getElementById(idPatrimonio).style.borderColor = "";
        //document.getElementById('errorParticipacion').innerHTML = "";
    }

    if (nombre && rut && tipo && patrimonio) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        //waitDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose('Registro de Campaña', 'Por favor espere mientras procesamos su operación...', 100, 550);
        return true;
    } else
        return false;


}



function ShowServerInformation() {

    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);

    return false;
}

function Dialogo() {
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    return true;
}

function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            return false
    }
}

function vacio(string) {
    if (string != '')
        return false;
    return true;
}

function validarFecha(string) { //string estará en formato dd/mm/yyyy (dí­as < 32 y meses < 13)
    var ExpReg = /^([0]?[1-9]|[1][0-2])[./-]([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0-9]{4}|[0-9]{2})$/;
    return (ExpReg.test(string));
}


function validarAno(string) {
    var ExpReg = /^\d{4}$/;
    return ExpReg.test(string);
}


function CerrarMensaje() {
    waitDialog.close();
    return false;
}




//function revisarDigito(dvr) {
//    dv = dvr + ""
//    if (dv != '0' && dv != '1' && dv != '2' && dv != '3' && dv != '4' && dv != '5' && dv != '6' && dv != '7' && dv != '8' && dv != '9' && dv != 'k' && dv != 'K') {

//        document.getElementById('errorRut').innerHTML = "Debe ingresar un digito verificador valido";
//        return false;
//    }
//    return true;
//}

//function revisarDigito2(crut) {
//    largo = crut.length;
//    if (largo < 2) {
//        document.getElementById('errorRut').innerHTML = "Debe ingresar el rut completo";
//        return false;
//    }
//    if (largo > 2)
//        rut = crut.substring(0, largo - 1);
//    else
//        rut = crut.charAt(0);
//    dv = crut.charAt(largo - 1);
//    revisarDigito(dv);

//    if (rut == null || dv == null)
//        return 0

//    var dvr = '0'
//    suma = 0
//    mul = 2

//    for (i = rut.length - 1 ; i >= 0; i--) {
//        suma = suma + rut.charAt(i) * mul
//        if (mul == 7)
//            mul = 2
//        else
//            mul++
//    }
//    res = suma % 11
//    if (res == 1)
//        dvr = 'k'
//    else if (res == 0)
//        dvr = '0'
//    else {
//        dvi = 11 - res
//        dvr = dvi + ""
//    }
//    if (dvr != dv.toLowerCase()) {
//        document.getElementById('errorRut').innerHTML = "Rut incorrecto";
//        return false;

//    }

//    return true
//}

//function Rut(texto) {
//    var tmpstr = "";
//    for (i = 0; i < texto.length ; i++)
//        if (texto.charAt(i) != ' ' && texto.charAt(i) != '.' && texto.charAt(i) != '-')
//            tmpstr = tmpstr + texto.charAt(i);
//    texto = tmpstr;
//    largo = texto.length;

//    if (largo < 2) {
//        document.getElementById('errorRut').innerHTML = "Debe ingresar el rut completo";
//        return false;
//    }

//    for (i = 0; i < largo ; i++) {
//        if (texto.charAt(i) != "0" && texto.charAt(i) != "1" && texto.charAt(i) != "2" && texto.charAt(i) != "3" && texto.charAt(i) != "4" && texto.charAt(i) != "5" && texto.charAt(i) != "6" && texto.charAt(i) != "7" && texto.charAt(i) != "8" && texto.charAt(i) != "9" && texto.charAt(i) != "k" && texto.charAt(i) != "K") {

//            document.getElementById('errorRut').innerHTML = "El valor ingresado no corresponde a un R.U.T valido";
//            return false;
//        }
//    }

//    var invertido = "";
//    for (i = (largo - 1), j = 0; i >= 0; i--, j++)
//        invertido = invertido + texto.charAt(i);
//    var dtexto = "";
//    dtexto = dtexto + invertido.charAt(0);
//    dtexto = dtexto + '-';
//    cnt = 0;

//    for (i = 1, j = 2; i < largo; i++, j++) {
//        //alert("i=[" + i + "] j=[" + j +"]" );		
//        if (cnt == 3) {
//            dtexto = dtexto + '.';
//            j++;
//            dtexto = dtexto + invertido.charAt(i);
//            cnt = 1;
//        }
//        else {
//            dtexto = dtexto + invertido.charAt(i);
//            cnt++;
//        }
//    }

//    invertido = "";
//    for (i = (dtexto.length - 1), j = 0; i >= 0; i--, j++)
//        invertido = invertido + dtexto.charAt(i);

//    // window.document.form1.rut.value = invertido.toUpperCase()

//    if (revisarDigito2(texto))
//        return true;

//    return false;
//}
