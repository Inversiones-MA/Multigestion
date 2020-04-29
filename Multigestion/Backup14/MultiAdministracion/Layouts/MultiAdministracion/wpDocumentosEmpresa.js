

function ValidarDocumentos(cad) {

    var CtrlArea = document.getElementById(cad + 'ddlArea');
    var CtrlTipoDoc = document.getElementById(cad + 'ddlTipoDocumento');
    var CtrlComentario = document.getElementById(cad + 'txtComentario');

    resetStyle(CtrlArea);
    resetStyle(CtrlTipoDoc);
    resetStyle(CtrlComentario);

    var Area, TipoDoc, Comentario;


    Area = SetControl(CtrlArea);
    TipoDoc = SetControl(CtrlTipoDoc);
    Comentario = SetControl(CtrlComentario);

    //if (TipoDireccion && Direccion && Provincia && Region && Comuna) {
    if (Area && TipoDoc && Comentario) {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
        //waitDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose('Registro de Campaña', 'Por favor espere mientras procesamos su operación...', 100, 550);
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
    if (string == "0" || string == "")
        return true;
    return false;
}

function SetTab() {

    var hdn = document.getElementById("DivHidden").children[0];
    if (hdn != null) {

        if (hdn.value != "") {

            $('#myTab').each(function () {
                $(this).find('li').each(function () {

                    $(this).removeClass('active');

                    switch (hdn.value) {
                        case 'li1':
                            if ($(this).context.id == 'li1') {
                                $(this).addClass('active');
                                $('#panel-tab1').addClass('active');
                                $('#panel-tab2').removeClass('active');
                                $('#panel-tab4').removeClass('active');
                            }
                            break;
                        case 'li2':
                            if ($(this).context.id == 'li2') {
                                $(this).addClass('active');
                                $('#panel-tab2').addClass('active');
                                $('#panel-tab1').removeClass('active');
                                $('#panel-tab4').removeClass('active');
                            }
                            break;
                        case 'li3':
                            if ($(this).context.id == 'li3') {
                                $(this).addClass('active');
                                $('#panel-tab4').addClass('active');
                                $('#panel-tab1').removeClass('active');
                                $('#panel-tab2').removeClass('active');
                            }
                            break;
                    }
                });
            });

        }
    }
    document.getElementById("DivPrincipal").style.display = "block";
}

document.addEventListener("DOMContentLoaded", function (event) {
    SetTab();
});
