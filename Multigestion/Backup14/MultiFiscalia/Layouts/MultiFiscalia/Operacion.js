//****************************Documentos*********************************//


function LimpiarDocumentos(cad) {

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

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("select");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].selectedIndex = 0;
    };

    return false;
}

function ValidarDocumentos(cad) {

    var idTipo = cad + 'ddlTipo';
    var idComentario = cad + 'txtComentario';
    var idFechaInicio = cad + 'txtFechaInicio_txtFechaInicioDate';
    var idFechaFin = cad + 'txtFechaFin_txtFechaFinDate';
    var idDocumentos = cad + 'rdDocumento';
    // var idExitente = cad + 'rdExistente';
    var Documentos = cad + 'fileDocumento';
    // var Exitente = cad + 'ddlExistente';
    //var idArchivo = cad + 'ctrSubirArchivo';
    var vArchivo = document.getElementById(Documentos).value;
    document.getElementById(idTipo).style.borderColor = "";//List
    document.getElementById(idComentario).style.borderColor = "";//textArea
    document.getElementById(idFechaInicio).style.borderColor = "";//SharePoint:DateTimeControl
    document.getElementById(idFechaFin).style.borderColor = "";//SharePoint:DateTimeControl
    document.getElementById(Documentos).style.borderColor = "";//File
    //document.getElementById(Exitente).style.borderColor = "";//List
    //elem.style.borderColor = "";
    var valor = 0;

    if (document.getElementById(idTipo).options[document.getElementById(idTipo).selectedIndex].text == 'Seleccione') {
        document.getElementById(idTipo).style.borderColor = "red";
        valor = 1;
    } else {
        document.getElementById(idTipo).style.borderColor = "";
    }

    if (vacio(document.getElementById(idComentario).value)) {
        document.getElementById(idComentario).style.borderColor = "red";
        valor = 1;
    } else {
        document.getElementById(idComentario).style.borderColor = "";
    }




    if (!vacio(document.getElementById(idFechaInicio).value))
        if (validarFecha(document.getElementById(idFechaInicio).value)) {
            document.getElementById(idFechaInicio).style.borderColor = "";
        } else {
            valor = 1;
            document.getElementById(idFechaInicio).style.borderColor = "red";
        }
    else {
        document.getElementById(idFechaInicio).style.borderColor = "red";
        valor = 1;
    }

    if (!vacio(document.getElementById(idFechaFin).value))
        if (validarFecha(document.getElementById(idFechaFin).value)) {
            document.getElementById(idFechaFin).style.borderColor = "";
        } else {
            valor = 1;
            document.getElementById(idFechaFin).style.borderColor = "red";
        }
    else {
        document.getElementById(idFechaFin).style.borderColor = "red";
        valor = 1;
    }


    if (vacio(vArchivo)) {
        document.getElementById(Documentos).style.borderColor = "red";
        valor = 1;
    } else {
        document.getElementById(Documentos).style.borderColor = "";
    }

    //if (document.getElementsByName(idDocumentos).checked) {
    //    if (vacio(document.getElementById(Documentos).value)) {
    //        document.getElementById(Documentos).style.borderColor = "red";
    //        valor = 1;
    //    } else {
    //        document.getElementById(Documentos).style.borderColor = "";
    //    }

    //}
    //if (document.getElementsByName(idExitente).checked) {

    //    if (document.getElementById(Exitente).options[document.getElementById(Exitente).selectedIndex].text == 'Seleccione') {
    //        document.getElementById(Exitente).style.borderColor = "red";
    //        valor = 1;
    //    } else {
    //        document.getElementById(Exitente).style.borderColor = "";
    //    }

    //}

    if (valor == 0) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;


}

function HabilitarDocumentos(cad) {

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
    document.getElementById(idPanel).disabled = false;

    return false;
}

function validarBotonRadioFile(cad) {
    var idDocumentos = cad + 'rdDocumento';
    // var idExitente = cad + 'rdExistente';
    var Documentos = cad + 'fileDocumento';
    //var Exitente = cad + 'ddlExistente';
    if (document.getElementById(idDocumentos).checked) {
        document.getElementById(Documentos).style.display = 'block';
        //document.getElementById(Exitente).style.display = 'none';
    }
    return false;

}

function validarBotonRadioExist(cad) {
    var idDocumentos = cad + 'rdDocumento';
    //var idExitente = cad + 'rdExistente';
    var Documentos = cad + 'fileDocumento';
    //var Exitente = cad + 'ddlExistente';
    //if (document.getElementById(idExitente).checked) {
    //    //document.getElementById(Exitente).style.display = 'block';
    //    document.getElementById(Documentos).style.display = 'none';
    //}
    return false;

}


//****************************Operaciones*********************************//

function validarTxtCkb(obj, cad) {

    if (obj.checked == true) {
        document.getElementById(cad + 'ddlFOGAPE').disabled = true;
    } else {
        document.getElementById(cad + 'ddlFOGAPE').disabled = false;
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

function HabilitarOperacion(cad) {

    var idPanel = cad + 'pnFormulario';
    //  var idPanelR = cad + 'pnRiesgo';
    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnCancel';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };
    document.getElementById(idPanel).disabled = false;

    //var div_to_disable = document.getElementById(idPanelR).getElementsByTagName("*");
    //var children = div_to_disable;
    //for (var i = 0; i < children.length; i++) {
    //    children[i].disabled = false;
    //};
    //document.getElementById(idPanelR).disabled = false;
    document.getElementById(cad + 'txtMontoOpeCLP').readOnly = true;
    ValidaAmortizacion(cad);
    return false;
}

function HabilitarOperacionR(cad) {
    var idPanelR = cad + 'pnRiesgo';

    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnCancel';
   
    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';
    
    document.getElementById(cad + 'txtFogape').disabled = false;
    document.getElementById(cad + 'ddlFogape').disabled = false;
    document.getElementById(cad + 'ddlFondo').disabled = false;



    document.getElementById(idPanelR).disabled = false;
    return false;
}

function HabilitarOperacionCO(cad) {
    HabilitarOperacion(cad);
    var idPanelR = cad + 'pnComercial';
    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnCancel';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanelR).getElementsByTagName("*");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };
    document.getElementById(idPanelR).disabled = false;


    document.getElementById(cad + 'txtTimbreYEstampilla').disabled = true;
    if (document.getElementById(cad + 'txtTimbreYEstampilla').value == '' || document.getElementById(cad + 'txtTimbreYEstampilla').value == 0) {
        document.getElementById(cad + 'ddlTimbreYEstampilla').disabled = true;
    }
    else {
        //Bloqueo ITyEAcreedor
        document.getElementById(cad + 'txtTimbreYEstampillaAcreedor').readOnly = true;
        //document.getElementById(cad + 'ckbTimbreYEstampillaAcreedor').disabled = true;
        document.getElementById(cad + 'ddlTimbreYEstampillaAcreedor').disabled = true;
    }
    return false;
}

function HabilitarOperacionCOR(cad) {
    
   
    document.getElementById(cad + 'txtFogape').disabled = false;
    document.getElementById(cad + 'ddlFogape').disabled = false;
    document.getElementById(cad + 'pnRiesgo').disabled = false;
 HabilitarOperacionCO(cad);

}

function HabilitarOperacionO(cad) {
    HabilitarOperacion(cad);
    HabilitarOperacionR(cad);
    HabilitarOperacionCO(cad);
    var idPanelR = cad + 'pnOperaciones';
    var idGuardar = cad + 'btnGuardar';
    var idLimpiar = cad + 'btnCancel';

    document.getElementById(idGuardar).style.display = 'block';
    document.getElementById(idLimpiar).style.display = 'block';

    var div_to_disable = document.getElementById(idPanelR).getElementsByTagName("*");
    var children = div_to_disable;
    for (var i = 0; i < children.length; i++) {
        children[i].disabled = false;
    };
    document.getElementById(idPanelR).disabled = false;
    return false;
}

function LimpiarOperacion(idPanel) {

    //var idPanel = cad + 'pnFormulario';

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

function ValidarOperacion(cad) {

    var idNombre = cad + 'txt_nombre';
    var idCargo = cad + 'txt_cargo';
    var idProfesion = cad + 'txt_profesion';
    var idUniversidad = cad + 'txt_universidad';
    var idRut = cad + 'txt_rut';
    var idAntiguedad = cad + 'txt_antiguedad';
    var idLRut = cad + 'txt_divRut';


    // document.getElementById('errorRut').innerHTML = "";

    document.getElementById(idNombre).style.borderColor = "";
    document.getElementById(idCargo).style.borderColor = "";
    document.getElementById(idProfesion).style.borderColor = "";
    document.getElementById(idUniversidad).style.borderColor = "";
    document.getElementById(idRut).style.borderColor = "";
    document.getElementById(idAntiguedad).style.borderColor = "";
    document.getElementById(idLRut).style.borderColor = "";


    var vnombre = document.getElementById(idNombre).value;
    var vcargo = document.getElementById(idCargo).value;
    var vprofesion = document.getElementById(idProfesion).value;
    var vuniversidad = document.getElementById(idUniversidad).value;
    var vrut = document.getElementById(idRut).value;
    var vantiguedad = document.getElementById(idAntiguedad).value;

    var lrut = document.getElementById(idLRut).value;
    //var varchivo = document.getElementById(idArchivo).value;
    var nombre, cargo, profesion, universidad, rut, antiguedad;

    if (vacio(vnombre)) {
        document.getElementById(idNombre).style.borderColor = "red";
        //document.getElementById('errorNombre').innerHTML = "requerido";
        nombre = false;
    } else {
        nombre = true;
        document.getElementById(idNombre).style.borderColor = "";
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

    if (vacio(vantiguedad)) {
        document.getElementById(idAntiguedad).style.borderColor = "red";
        // document.getElementById('errorParticipacion').innerHTML = "requerido";
        aniguedad = false;
    } else {
        antiguedad = true;
        document.getElementById(idAntiguedad).style.borderColor = "";
        //document.getElementById('errorParticipacion').innerHTML = "";
    }

    if (nombre && cargo && rut && antiguedad) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        //waitDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose('Registro de Campaña', 'Por favor espere mientras procesamos su operación...', 100, 550);
        return true;
    } else
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
    var val = document.getElementById(campoOpeCLP).value;
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

    
    //  query = '<View><Query><Where><Eq><FieldRef Name=\'ID\'/>' + '<Value Type=\'Number\'>' + seleccion + '</Value></Eq></Where></Query></View>';
   // query = '<View><Query><OrderBy><FieldRef Name=\'ID\' Ascending =\'FALSE\'/></OrderBy><rowLimit>1</rowLimit></Query></View>';
    //  oQuery.Query = "<OrderBy>  <FieldRef Name='ID' Ascending ='FALSE'/>   </OrderBy> <Where> <Eq><FieldRef Name='Fecha' /><Value  Type='DateTime'>" + startDate + "</Value></Eq> </Where>";

    query = '<View><Query><Where><Eq><FieldRef Name="Fecha"/><Value Type="DateTime" IncludeTimeValue="FALSE"><Today /></Value></Eq></Where></Query></View>';
        //'<View><Where> <Eq><FieldRef Name=\'Fecha\' /><Value  Type=\'DateTime\'>' + f.getFullYear() + (f.getMonth() + 1) + "-" + f.getDate() + 'T00:00:00+01:00' + '</Value></Eq> </Where> </View>';

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

    alert('Error de Memoria . ' + args.get_message() + '\n' + args.get_stackTrace());
}