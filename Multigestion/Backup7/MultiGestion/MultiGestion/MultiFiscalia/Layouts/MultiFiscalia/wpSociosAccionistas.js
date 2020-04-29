
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

function HabilitarDatosEmpresa(cad) {

    var idPanel = cad + 'pnFormulario';
    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnCancel';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };

    return false;
}

function LimpiarDatosEmpresa(cad) {

    var idPanel = cad + 'pnFormulario';

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

    return false;
}

function Limpiar() {
    document.getElementById('errorNombre').innerHTML = "";
    document.getElementById('errorRut').innerHTML = "";
    document.getElementById('errorParticipacion').innerHTML = "";
  
}


function valMaximo(cad, nombreCampo, valor) {
    FormatoMoneda(cad + nombreCampo)
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
}

function Validar(cad) {
   // Limpiar();
    //alert(cad);

        var idNombre = cad + 'txt_nombre';
        var idRut = cad + 'txt_rut';
        var idParticipacion = cad + 'txt_participacion';
        var idLRut = cad + 'txt_divRut';
        //document.getElementById('errorRut').innerHTML = "";
        document.getElementById(idNombre).style.borderColor = "";
        document.getElementById(idRut).style.borderColor = "";
        document.getElementById(idParticipacion).style.borderColor = "";
        document.getElementById(idLRut).style.borderColor = "";

        var vnombre = document.getElementById(idNombre).value;
        var vrut = document.getElementById(idRut).value;
        var vparticipacion = document.getElementById(idParticipacion).value;
        var lrut = document.getElementById(idLRut).value;
        //var varchivo = document.getElementById(idArchivo).value;
        

        var nombre, rut, participacion;

       

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


        if (vacio(vparticipacion)) {
            document.getElementById(idParticipacion).style.borderColor = "red";
           // document.getElementById('errorParticipacion').innerHTML = "requerido";
            participacion = false;
        } else {
            participacion = true;
            document.getElementById(idParticipacion).style.borderColor = "";
            //document.getElementById('errorParticipacion').innerHTML = "";
        }
      
        if (nombre && rut && participacion) {
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

function solonumeros(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8) {
        if (unicode < 48 || unicode > 57)
            return false
    }
}

function CerrarMensaje() {
    waitDialog.close();
    return false;
}

