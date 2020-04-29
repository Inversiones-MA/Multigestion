function Adjuntar(muestra,nombre) {
    var ficha = document.getElementById(muestra);
    
    document.getElementById('botones').style.display = "none";
    ficha.style.display = "none";
    document.getElementById('DivArchivo').style.display = "block";
    document.getElementById('NombreArchivo').innerHTML = "";
    document.getElementById('NombreArchivo').innerHTML = nombre;
    //return true;
}


function Dialogo() {
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    return true;
}

function ValidarSeleccionServicios(cad) {
    //alert('sdf');
    var valor = 0;
    //var idFechaInicio = cad + 'fechaInicio_fechaInicioDate';
    //var idFechaFin = cad + 'fechaFin_fechaFinDate';
    //document.getElementById(idFechaInicio).style.borderColor = "";
    //document.getElementById(idFechaFin).style.borderColor = "";

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
        valor = 1;
    } else {
        filedoc = true;
        document.getElementById(filedocumento).style.borderColor = "";
    }
    if (document.getElementById(ddltipo).options[document.getElementById(ddltipo).selectedIndex].text == 'Seleccione') {
        document.getElementById(ddltipo).style.borderColor = "red";
        //document.getElementById(ddltipo).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        valor = 1;
    } else {

        document.getElementById(ddltipo).style.borderColor = "";
        //document.getElementById(ddltipo).style.borderColor = "";
        // document.getElementById('errorNombre').innerHTML = "";
    }

    //if (!vacio(document.getElementById(idFechaInicio).value))
    //    if (validarFecha(document.getElementById(idFechaInicio).value)) {
    //        document.getElementById(idFechaInicio).style.borderColor = "";
    //    } else {
    //        valor = 1;
    //        document.getElementById(idFechaInicio).style.borderColor = "red";
    //    }
    //else {
    //    document.getElementById(idFechaInicio).style.borderColor = "red";
    //    valor = 1;
    //}

    //if (!vacio(document.getElementById(idFechaFin).value))
    //    if (validarFecha(document.getElementById(idFechaFin).value)) {
    //        document.getElementById(idFechaFin).style.borderColor = "";
    //    } else {
    //        valor = 1;
    //        document.getElementById(idFechaFin).style.borderColor = "red";
    //    }
    //else {
    //    document.getElementById(idFechaFin).style.borderColor = "red";
    //    valor = 1;
    //}

    if (valor == 0) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    }

    return false;

}