

//****************************SolicitudFiscalia*********************************//

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
    }
}

function HabilitarOperacion(cad) {

    var idPanel = cad + 'pnFormulario';
    //var idPanelR = cad + 'pnRiesgo';
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


    return false;
}

function LimpiarOperacion(cad) {

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

function CerrarMensaje() {
    waitDialog.close();
    return false;
}


//***************************test************************//

function delrow(cad, rowindex) {

    var gridID = document.getElementById(cad + 'gridFiscalia');
    gridID.deleteRow(rowindex + 1);

    return false;
}

var cadena;
var cadenatabla;
var idtabla;
var indCaso;
var accion;
function AddRow(cadn, cad, ckb, texto, ID, IdGarantia) {
    cadena = cad;
    cadenatabla = cadn;
    var gridview = document.getElementById(cadn + 'gridFiscalia');
    var tr = document.createElement("tr");

    var cell1 = document.createElement("td");
    var cell2 = document.createElement("td");
    var cell3 = document.createElement("td");
    var cell4 = document.createElement("td");
    var cell5 = document.createElement("td");
    var cell6 = document.createElement("td");

    cell1.innerHTML = IdGarantia;
    cell2.innerHTML = ID;
    cell3.innerHTML = texto;
    cell4.innerHTML = '1';
    cell5.innerHTML = '1';//Valor
    cell6.innerHTML = '1';//Total

    tr.appendChild(cell1);
    tr.appendChild(cell2);
    tr.appendChild(cell3);
    tr.appendChild(cell4);
    tr.appendChild(cell5);
    tr.appendChild(cell6);

    tr.setAttribute("clase", "table-hover");

    gridview.appendChild(tr);

    var table = document.getElementById(cadn + 'gridFiscalia');
    idtabla = table.rows.length - 1;


    accion = 1;//suma
    
    if (parseInt(IdGarantia) > 0)
        indCaso = 1;//garantia
    else
        indCaso = 0;//Empresa
    ValorServicio(cadn, ID);
    //gridview.firstChild.appendChild(tr);

}


function addServicio(cadn, cad, ckb, texto, ID, IdGarantia) {
    cadena = cad;
    cadenatabla = cadn;
    var textos = '';
    var txt2 = "";
    var ban = 0;
    //ctl00
    cad = 'ctl00' + cad.replace(/ctl00/g, '');
    // str.replace(/abc/g, '');
    var table = document.getElementById(cadn + 'gridFiscalia');
    for (var i = 0; i < table.rows.length; i++) {

        var textos = table.rows[i].cells[2].innerHTML;
        if (textos == texto) {
            if (document.getElementById(cad + ckb).checked) {
                table.rows[i].cells[3].innerHTML = parseInt(table.rows[i].cells[3].innerHTML) + 1;
                table.rows[i].cells[0].innerHTML = table.rows[i].cells[0].innerHTML + " " + IdGarantia;
                accion = 1;//suma
                idtabla = i;
                if (parseInt(IdGarantia) > 0)
                    indCaso = 1;//garantia
                else
                    indCaso = 0;//Empresa
                ValorServicio(cadn, ID);
            } else {
                table.rows[i].cells[3].innerHTML = parseInt(table.rows[i].cells[3].innerHTML) - 1;
                table.rows[i].cells[0].innerHTML = table.rows[i].cells[0].innerHTML.replace(/IdGarantia/g, '');
                accion = 0;//resta;
                idtabla = i;
                if (parseInt(IdGarantia) > 0)
                    indCaso = 1;//garantia
                else
                    indCaso = 0;//empresa
                ValorServicio(cadn, ID);
            }

            ban = 1;
        }

    }
    if (ban == 0) {
        if (document.getElementById(cad + ckb).checked) {
            AddRow(cadn, cad, ckb, texto, ID, IdGarantia);
        }
    }



}




function ValorServicio(code, idServicio) {
    var query = '';
    cadena = code;
    if (indCaso == 0) {
        //  alert('valor: ' + valor);
        query = '<View><Query><Where><Eq><FieldRef Name=\'ID\'/>'
                    + '<Value Type=\'Text\'>' + idServicio + '</Value></Eq></Where></Query></View>';
        lista = 'ServiciosOperacion';
    }
    if (indCaso == 1) {
        query = '<View><Query><Where><Eq><FieldRef Name=\'ID\'/>'
                    + '<Value Type=\'Text\'>' + idServicio + '</Value></Eq></Where></Query></View>';
        lista = 'ServiciosGarantia';
    }
    retrieveListItemsTotal(query, lista);
    return false;
}

function retrieveListItemsTotal(query, lista) {
    var clientContext = new SP.ClientContext.get_current();
    var oList = clientContext.get_web().get_lists().getByTitle(lista);
    var camlQuery = new SP.CamlQuery();
    camlQuery.set_viewXml(query);
    this.collListItem = oList.getItems(camlQuery);
    clientContext.load(collListItem);
    var a = clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceededTotal), Function.createDelegate(this, this.onQueryFailed));
    return false;
}
var costo;
function onQuerySucceededTotal(sender, args) {

    try {
        var listItemEnumerator = this.collListItem.getEnumerator();
    }
    catch (err) {
        return false;
    }


    while (listItemEnumerator.moveNext()) {
        var oListItem = listItemEnumerator.get_current();
        costo = oListItem.get_item('Costo');
        var table = document.getElementById(cadenatabla + 'gridFiscalia');
        table.rows[idtabla].cells[4].innerHTML = costo;
        if (accion = 1)
            table.rows[idtabla].cells[5].innerHTML = parseInt(table.rows[idtabla].cells[3].innerHTML) * costo;

        if (accion = 0)
            table.rows[idtabla].cells[5].innerHTML = parseInt(table.rows[idtabla].cells[5].innerHTML) - costo;


        return false;

    }
}



function onQueryFailed(sender, args) {

    alert('Error de Memoria . ' + args.get_message() + '\n' + args.get_stackTrace());
}

function ValidarArchivos(cad) {
    var valor = 0;
    var idFechaInicio = cad + 'txtFechaInicio_txtFechaInicioDate';
    var idFechaFin = cad + 'txtFechaFin_txtFechaFinDate';
    document.getElementById(idFechaInicio).style.borderColor = "";//SharePoint:DateTimeControl
    document.getElementById(idFechaFin).style.borderColor = "";//SharePoint:DateTimeControl
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
    if (valor == 0) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        return true;
    } else
        return false;
}