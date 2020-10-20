function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            return false
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

function validarPersonas(cad) {
    var CtrlRut = document.getElementById(cad + 'txt_rut');
    var CtrlDivRut = document.getElementById(cad + 'txt_divRut');
    var CtrlNombre = document.getElementById(cad + 'txt_nombre');
    //var CtrlFecha = document.getElementById(cad + 'dtcFechaVerificacion_dtcFechaVerificacionDate');

    resetStyle(CtrlRut);
    resetStyle(CtrlDivRut);
    resetStyle(CtrlNombre);
    // resetStyle(CtrlVerificacion);

    var Rut, DivRut, Nombre;

    Rut = SetControl(CtrlRut);
    DivRut = SetControlDivRut(CtrlDivRut);
    Nombre = SetControl(CtrlNombre);
    // Verificacion = SetControl(CtrlVerificacion);
    if (!VerificaRut(CtrlRut.value + '-' + CtrlDivRut.value)) {
        CtrlRut.style.borderColor = "red";
        CtrlDivRut.style.borderColor = "red";    
        AuxRut = false;
    }
    else {
        resetStyle(CtrlRut);
        resetStyle(CtrlDivRut);
        AuxRut = true;
    }
    
    if (Rut && DivRut && Nombre && AuxRut) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}


function vacio(string) {
    if (string == "0" || string == "")
        return true;
    return false;
}

function vacioDivRut(string) {
    if (string == "")
        return true;
    return false;
}

function SetControl(control) {

    control.style.borderColor = vacio(control.value) ? 'red' : '';
    return !vacio(control.value);
}

function SetControlDivRut(control) {

    control.style.borderColor = vacioDivRut(control.value) ? 'red' : '';
    return !vacioDivRut(control.value);
}

function resetStyle(control) {
    control.style.borderColor = ""
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