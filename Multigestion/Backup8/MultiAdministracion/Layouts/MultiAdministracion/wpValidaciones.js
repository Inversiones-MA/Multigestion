function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
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

function solonumerosDV(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 107 && unicode != 75)
                return false
    }
}

function soloFechas(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 45)
                return false
    }
}

function Dialogo() {
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    return true;
}

function validarFecha(string) { //string estará en formato dd/mm/yyyy (dí¬as < 32 y meses < 13)
    var ExpReg = /^([0]?[1-9]|[1|2][0-9]|[3][0|1])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$/;
    return (ExpReg.test(string));
}

function validarEmail(email) {
    expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return (expr.test(email));
}

function validarEmpresaClientes(cad) {
    var CtrlRut = document.getElementById(cad + 'txt_rut');
    var CtrldivRut = document.getElementById(cad + 'txt_divRut');
    var Ctrlnombre = document.getElementById(cad + 'txt_nombre');
    var CtrlporcVentas = document.getElementById(cad + 'txt_porcVentas');
    var Ctrlplazo = document.getElementById(cad + 'txt_plazo');

    resetStyle(CtrlRut);
    resetStyle(CtrldivRut);
    resetStyle(Ctrlnombre);
    resetStyle(CtrlporcVentas);
    resetStyle(Ctrlplazo);

    var Rut, divRut, nombre, porcVentas, plazo, aux = true;
    //Pone en rojo los controles vacios
    Rut = SetControl(CtrlRut);
    divRut = SetControl(CtrldivRut);
    nombre = SetControl(Ctrlnombre);
    porcVentas = SetControl(CtrlporcVentas);
    plazo = SetControl(Ctrlplazo);

    if (!VerificaRut(CtrlRut.value + '-' + CtrldivRut.value)) {
        CtrlRut.style.borderColor = 'red';
        CtrldivRut.style.borderColor = 'red';
        aux = false;
    } else {
        resetStyle(CtrlRut);
        resetStyle(CtrldivRut);
    }

    if (Rut && divRut && nombre && porcVentas && plazo && aux) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}

function validarEmpresaAdministracion(cad) {
    var CtrlRut = document.getElementById(cad + 'txt_rut');
    var CtrldivRut = document.getElementById(cad + 'txt_divRut');
    var CtrlNombre = document.getElementById(cad + 'txt_nombre');
    var CtrlCargo = document.getElementById(cad + 'txt_cargo');
    var CtrlFecnac = document.getElementById(cad + 'dtcFecNaci_dtcFecNaciDate');
    var CtrlAntiguedad = document.getElementById(cad + 'txt_antiguedad');
    var CtrlTelefono = document.getElementById(cad + 'txt_telefono');
    var CtrlMail = document.getElementById(cad + 'txt_mail');
    var CtrlEdoCivil = document.getElementById(cad + 'ddlEdoCivil');

    resetStyle(CtrlRut);
    resetStyle(CtrldivRut);
    resetStyle(CtrlNombre);
    resetStyle(CtrlCargo);
    resetStyle(CtrlFecnac);
    resetStyle(CtrlAntiguedad);
    resetStyle(CtrlTelefono);
    resetStyle(CtrlMail);
    resetStyle(CtrlEdoCivil);

    var Rut, divRut, nombre, cargo, fecNac, antiguedad, telefono, mail, EdoCivil, auxRut = true, auxMail = true;
    //Pone en rojo los controles vacios
    Rut = SetControl(CtrlRut);
    divRut = SetControl(CtrldivRut);
    nombre = SetControl(CtrlNombre);
    cargo = SetControl(CtrlCargo);
    fecNac = SetControl(CtrlFecnac);
    antiguedad = SetControl(CtrlAntiguedad);
    telefono = SetControl(CtrlTelefono);
    mail = SetControl(CtrlMail);

    if (CtrlEdoCivil.value != "0") {
        EdoCivil = true;
    } else {
        EdoCivil = false;
        CtrlEdoCivil.style.borderColor = 'red';
    }


    if (!VerificaRut(CtrlRut.value + '-' + CtrldivRut.value)) {
        CtrlRut.style.borderColor = 'red';
        CtrldivRut.style.borderColor = 'red';
        auxRut = false;
    } else {
        resetStyle(CtrlRut);
        resetStyle(CtrldivRut);
    }

    if (!validarEmail(CtrlMail.value)) {
        CtrlMail.style.borderColor = 'red';
        auxMail = false;
    } else {
        resetStyle(CtrlMail);
    }

    if (Rut && divRut && nombre && fecNac && cargo && antiguedad && telefono && mail && auxRut && auxMail && EdoCivil) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;

}

function validarEmpresaProveedores(cad) {
    var CtrlRut = document.getElementById(cad + 'txt_rut');
    var CtrldivRut = document.getElementById(cad + 'txt_divRut');
    var Ctrlnombre = document.getElementById(cad + 'txt_nombre');
    var CtrlporcVentas = document.getElementById(cad + 'txt_porcVentas');
    var Ctrlplazo = document.getElementById(cad + 'txt_plazo');

    resetStyle(CtrlRut);
    resetStyle(CtrldivRut);
    resetStyle(Ctrlnombre);
    resetStyle(CtrlporcVentas);
    resetStyle(Ctrlplazo);

    var Rut, divRut, nombre, porcVentas, plazo, aux = true;
    //Pone en rojo los controles vacios
    Rut = SetControl(CtrlRut);
    divRut = SetControl(CtrldivRut);
    nombre = SetControl(Ctrlnombre);
    porcVentas = SetControl(CtrlporcVentas);
    plazo = SetControl(Ctrlplazo);

    if (!VerificaRut(CtrlRut.value + '-' + CtrldivRut.value)) {
        CtrlRut.style.borderColor = 'red';
        CtrldivRut.style.borderColor = 'red';
        aux = false;
    } else {
        resetStyle(CtrlRut);
        resetStyle(CtrldivRut);
    }

    if (Rut && divRut && nombre && porcVentas && plazo && aux) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}

function SetControl(control) {
    control.style.borderColor = vacio(control.value) ? 'red' : '';
    return !vacio(control.value);
}

function resetStyle(control) {
    control.style.borderColor = ""
}

function vacio(string) {
    if (string == "")
        return true;
    return false;
}

function validarEmpresaInfDeuda(cad) {
    var CtrlInstFinanciera = document.getElementById(cad + 'txt_instFinancieraID');
    var CtrlMontoVigente = document.getElementById(cad + 'txt_MontoVigente');
    var CtrlMora3090 = document.getElementById(cad + 'txt_Mora_30_90');
    var CtrlMora90mas = document.getElementById(cad + 'txt_Mora90Mas');

    resetStyle(CtrlInstFinanciera);
    resetStyle(CtrlMontoVigente);
    resetStyle(CtrlMora3090);
    resetStyle(CtrlMora90mas);

    var InstFinanciera, MontoVigente, Mora3090, Mora90mas;
    //Pone en rojo los controles vacios
    InstFinanciera = SetControl(CtrlInstFinanciera);
    MontoVigente = SetControl(CtrlMontoVigente);
    Mora3090 = SetControl(CtrlMora3090);
    Mora90mas = SetControl(CtrlMora90mas);

    if (InstFinanciera && MontoVigente && Mora3090 && Mora90mas) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}

function validarEmpresaCredDisponible(cad) {
    var CtrlInstFinanciera = document.getElementById(cad + 'txt_instFinancieraCD');
    var CtrlMontoDispDirecta = document.getElementById(cad + 'txt_montoDispDirecta');
    var CtrlMontoDispIndirecta = document.getElementById(cad + 'txt_montoDispIndirecta');
    

    resetStyle(CtrlInstFinanciera);
    resetStyle(CtrlMontoDispDirecta);
    resetStyle(CtrlMontoDispIndirecta);

    var InstFinanciera, MontoDispDirecta, MontoDispIndirecta;
    //Pone en rojo los controles vacios
    InstFinanciera = SetControl(CtrlInstFinanciera);
    MontoDispDirecta = SetControl(CtrlMontoDispDirecta);
    MontoDispIndirecta = SetControl(CtrlMontoDispIndirecta);

    if (InstFinanciera && MontoDispDirecta && MontoDispIndirecta) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}