function Validar(cad) {

    var CtrlTipoDireccion = document.getElementById(cad + 'ddlTipo');
    var CtrlDireccion = document.getElementById(cad + 'txtdireccion');
    var CtrlProvincia = document.getElementById(cad + 'ddlProvincia');
    var CtrlRegion = document.getElementById(cad + 'ddlRegion');
    var CtrlComuna = document.getElementById(cad + 'ddlComunas');
    var CtrlNumero = document.getElementById(cad + 'txtNumero');
    
    resetStyle(CtrlTipoDireccion);
    resetStyle(CtrlDireccion);
    resetStyle(CtrlProvincia);
    resetStyle(CtrlRegion);
    resetStyle(CtrlComuna);
    resetStyle(CtrlNumero);

    var TipoDireccion, Direccion, Provincia, Verificacion, Region, Comuna, Numero, aux=true;

    TipoDireccion = SetControl(CtrlTipoDireccion);
    Direccion = SetControl(CtrlDireccion);
    Provincia = SetControl(CtrlProvincia);
    Numero = SetControl(CtrlNumero);
    Region = SetControl(CtrlRegion);
    Comuna = SetControl(CtrlComuna);

    if (TipoDireccion && Direccion && Provincia  && Region && Comuna && Numero ) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}

function SetControl(control) {
    control.style.borderColor = vacio(control.value.trim()) ? 'red' : '';
    return !vacio(control.value.trim());
}

function resetStyle(control) {
    control.style.borderColor = ""
}

function vacio(string) {
    if (string == "0" || string == "" || string == " ")
        return true;
    return false;
}

function solonumerosFechas(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            if (unicode != 45)
                return false
    }
}

function EsFecha(fecha) {
    var ExpReg = /^([0][1-9]|[12][0-9]|3[01])(\/|-)([0][1-9]|[1][0-2])\2(\d{4})$/;
    return ExpReg.test(fecha);
}