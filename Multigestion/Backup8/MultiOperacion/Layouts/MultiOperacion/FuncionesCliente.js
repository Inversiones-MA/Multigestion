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

