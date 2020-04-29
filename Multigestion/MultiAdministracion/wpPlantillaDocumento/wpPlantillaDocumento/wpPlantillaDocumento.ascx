<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPlantillaDocumento.ascx.cs" Inherits="MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.wpPlantillaDocumento" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<script src="../../_layouts/15/MultiAdministracion/jquery-1.8.3.min.js"></script>

<script type="text/javascript">
    
        var isDXScript = function (script) {
        return script && script.id && script.id.indexOf("dx") === 0;
    }

    //if ($.clean) {
    //    var originalClean = $.clean;

    //    $.clean = function (elems, context, fragment, scripts) {
    //        var execResult = originalClean.call($, elems, context, fragment, scripts);
    //        if (scripts && scripts.length) {
    //            for (var i = scripts.length - 1; i >= 0; i--) {
    //                if (isDXScript(scripts[i]))
    //                    scripts.splice(i, 1);
    //            }
    //        }
    //        return execResult;
    //    };
    //}

    //$(function () {
    //    $("#tabs").tabs();
    //    AdjustDXControls();
    //});

    //function AdjustDXControls() {
    //    ////2
    //    HtmlEditorGenerica.AdjustControl();
    //}

    $(document).ready(function () {
        $('.oculto').hide();

        $('.list-unstyled').click(function () {
            $(this).find('ul').slideToggle();
        });

        $('.divide').click(function () {
            $('#datP').slideToggle();
        });

        //var nombre = $('#<%=HtmlEditorGenerica.ClientID%>').attr('id');
        //var res = nombre.substring(69, 51);
        var idd = 1;
        var cambio = false;
        Prueba(idd);   
     });


    function Prueba(idd) {
        ocultarDiv();
        if (idd == 1) {
            cambio = false;
            HdfC.Set("cambio", "no");
            ChkGenerica.SetChecked(false);
            ChkGenerica.SetEnabled(false);
            $('#panel-tab1').addClass('active');

            var panel = document.getElementById("panel-tab2");
            panel.style.visibility = 'hidden';
            panel.style.display = 'none';
            document.getElementById("Tab1").className = "active";
            document.getElementById("Tab2").className = "";
            BtnGuardarPlantilla.SetVisible(false);
            BtnCancelar.SetVisible(false);
        }
        else if (idd == 2) {
            if (!cambio) {
                HdfC.Set("cambio", "no");
                limpiarT();
            }
            ChkGenerica.SetEnabled(true);
            $('#panel-tab1').removeClass('active');
            var panel = document.getElementById("panel-tab2");
            panel.style.visibility = 'visible';
            panel.style.display = 'block';
            document.getElementById("Tab2").className = "active";
            document.getElementById("Tab1").className = "";
            HtmlEditorGenerica.AdjustControl();
            BtnGuardarPlantilla.SetVisible(true);
            BtnCancelar.SetVisible(true);
        }
    }

    function Slide(div, span) {
        $('#' + div).slideToggle();
        if ($("#" + span).text() == "+")
            $("#" + span).text("-");
        else
            $("#" + span).text("+");
    }

    function InsertaTxtGenerica() {
        var templateText = LstDatosEmpresaGenerica.GetSelectedItem().text;
        var valor = LstDatosEmpresaGenerica.GetSelectedItem().value;
        HtmlEditorGenerica.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, templateText);
    }

    function InsertaTxtOperacionGenerica() {
        var templateText = LstDatosOperacionGenerica.GetSelectedItem().text;
        var valor = LstDatosOperacionGenerica.GetSelectedItem().value;
        HtmlEditorGenerica.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, templateText);
    }

    function InsertaTxtDatosAcreedorGenerica() {
        var templateText = LstDatosAcreedorGenerica.GetSelectedItem().text;
        var valor = LstDatosAcreedorGenerica.GetSelectedItem().value;
        HtmlEditorGenerica.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, templateText);
    }

    function limpiarT() {
        HtmlEditorGenerica.SetHtml(null);
        CmbxAcreedorGenerica.SetValue(null);
        CmbxTipoProductoGenerica.SetValue(null);
        CmbxTipoDocumentoGenerica.SetValue(null);
        CmbxTipoDocumentoGenerica.SetText(null);
        TxtNombrePlantillaGenerica.SetText(null);
        ChkGenerica.SetChecked(false);
        CmbxTipoProductoGenerica.SetEnabled(true);
        CmbxTipoDocumentoGenerica.SetEnabled(true);
        CmbxAcreedorGenerica.SetEnabled(true);
        ChkGenerica.SetEnabled(true);
    }

    function OnbtnGuardar(s, e) {
         try {
            var chk = ChkGenerica.GetChecked();
            if (!chk) {
                var txt = TxtNombrePlantillaGenerica.GetText();
                var TipoPro = CmbxTipoProductoGenerica.GetText();
                var TipoDoc = CmbxTipoDocumentoGenerica.GetText();
                var Acre = CmbxAcreedorGenerica.GetText();
                var txthtml = HtmlEditorGenerica.GetHtml();
                if ((TipoPro != null && TipoPro && 'Seleccione' && TipoPro != "") && (TipoDoc != null && TipoDoc != 'Seleccione' && TipoDoc != "") && (Acre != null && Acre != 'Seleccione' && Acre != "") && (txt != null && txt != "") && (txthtml != null && txthtml != "")) {
                    TxtNombrePlantillaGenerica.GetMainElement().style.border = "";
                    CmbxAcreedorGenerica.GetMainElement().style.border = "";
                    HtmlEditorGenerica.GetMainElement().style.border = "";
                    ocultarDiv();
                    LblTipoDoc.SetText('3');
                    ASPxCallback1.PerformCallback('guardar');
                    OpenDlg();
                }
                else {
                    if (txt == "") {
                        TxtNombrePlantillaGenerica.GetMainElement().style.border = "solid red";
                    }
                    else
                        TxtNombrePlantillaGenerica.GetMainElement().style.border = "";

                    if (TipoPro == null || TipoPro == 'Seleccione' || TipoPro == "")
                        CmbxTipoProductoGenerica.GetMainElement().style.border = "solid red";
                    else
                        CmbxTipoProductoGenerica.GetMainElement().style.border = "";

                    if (TipoDoc == null || TipoDoc == 'Seleccione' || TipoDoc == "" || TipoDoc == "")
                        CmbxTipoDocumentoGenerica.GetMainElement().style.border = "solid red";
                    else
                        CmbxTipoDocumentoGenerica.GetMainElement().style.border = "";

                    if (Acre == null || Acre == 'Seleccione' || Acre == "")
                        CmbxAcreedorGenerica.GetMainElement().style.border = "solid red";
                    else
                        CmbxAcreedorGenerica.GetMainElement().style.border = "";

                    //if (txthtml == null || txthtml == "")
                    //    HtmlEditorGenerica.GetMainElement().style.border = "solid red";
                    //else
                    //    HtmlEditorGenerica.GetMainElement().style.border = "";

                    e.processOnServer = false;
                }
            }
            //plantilla generica
            else {
                var txt = TxtNombrePlantillaGenerica.GetText();
                var TipoPro = CmbxTipoProductoGenerica.GetText();
                var TipoDoc = CmbxTipoDocumentoGenerica.GetText();
                var txthtml = HtmlEditorGenerica.GetHtml();
                if ((TipoPro != null && TipoPro && 'Seleccione' && TipoPro != "") && (TipoDoc != null && TipoDoc != 'Seleccione' && TipoDoc != "") && (txt != null && txt != "") && (txthtml != null && txthtml != "")) {
                    TxtNombrePlantillaGenerica.GetMainElement().style.border = "";
                    HtmlEditorGenerica.GetMainElement().style.border = "";
                    ocultarDiv();
                    LblTipoDoc.SetText('3');
                    ASPxCallback1.PerformCallback('guardar');
                    OpenDlg();
                }
                else {
                    if (txt == "") {
                        TxtNombrePlantillaGenerica.GetMainElement().style.border = "solid red";
                    }
                    else
                        TxtNombrePlantillaGenerica.GetMainElement().style.border = "";

                    if (TipoPro == null || TipoPro == 'Seleccione' || TipoPro == "")
                        CmbxTipoProductoGenerica.GetMainElement().style.border = "solid red";
                    else
                        CmbxTipoProductoGenerica.GetMainElement().style.border = "";

                    if (TipoDoc == null || TipoDoc == 'Seleccione' || TipoDoc == "")
                        CmbxTipoDocumentoGenerica.GetMainElement().style.border = "solid red";
                    else
                        CmbxTipoDocumentoGenerica.GetMainElement().style.border = "";

                    //if (txthtml == null || txthtml == "")
                    //    HtmlEditorGenerica.GetMainElement().style.border = "solid red";
                    //else
                    //    HtmlEditorGenerica.GetMainElement().style.border = "";

                    e.processOnServer = false;
                }
            }

        } catch (e) {
            alert(e);
        }
    }

    function OnBtnBuscarBuscar(s, e) {
        try {
            var TipoPro = CmbxTipoProductoBuscar.GetText();
            var TipoDoc = CmbxTipoDocumentoBuscar.GetText();
            var Acre = CmbxAcreedorBuscar.GetText();
            if ((TipoPro != null && TipoPro && 'Seleccione' && TipoPro != "") && (TipoDoc != null && TipoDoc != 'Seleccione' && TipoDoc != "") && (Acre != null && Acre != 'Seleccione' && Acre != "")) {
                ocultarDiv();
                GvBuscarPlantilla.Refresh();
                GvBuscarPlantilla.PerformCallback("cargar");
            }
            else {
                if (TipoPro == null || TipoPro == 'Seleccione' || TipoPro == "")
                    CmbxTipoProductoBuscar.GetMainElement().style.border = "solid red";
                else
                    CmbxTipoProductoBuscar.GetMainElement().style.border = "";

                if (TipoDoc == null || TipoDoc == 'Seleccione' || TipoDoc == "")
                    CmbxTipoDocumentoBuscar.GetMainElement().style.border = "solid red";
                else
                    CmbxTipoDocumentoBuscar.GetMainElement().style.border = "";

                if (Acre == null || Acre == 'Seleccione' || Acre == "")
                    CmbxAcreedorBuscar.GetMainElement().style.border = "solid red";
                else
                    CmbxAcreedorBuscar.GetMainElement().style.border = "";

                e.processOnServer = false;       
            }
        } catch (e) {
            alert(e);
        }
    }


    function OnChangedTipoProductoBuscar(s, e) {
        var TipoPro = s.GetText();

        if (TipoPro == null || TipoPro == 'Seleccione' || TipoPro == "")
            CmbxTipoProductoBuscar.GetMainElement().style.border = "solid red";
        else
            CmbxTipoProductoBuscar.GetMainElement().style.border = "";

        CmbxTipoDocumentoBuscar.PerformCallback(s.GetSelectedItem().value.toString());

    }

    function OnChangedTipoProductoGenerica(s, e) {
        var TipoPro = s.GetText();

        if (TipoPro == null || TipoPro == 'Seleccione' || TipoPro == "")
            CmbxTipoProductoGenerica.GetMainElement().style.border = "solid red";
        else {
            CmbxTipoProductoGenerica.GetMainElement().style.border = "";
            CmbxTipoDocumentoGenerica.PerformCallback(s.GetSelectedItem().value.toString());
        }
    }

    function OnChangedAcreedorGenerica(s, e) {
        var Acreedor = s.GetText();

        if (Acreedor == null || Acreedor == 'Seleccione' || Acreedor == "")
            CmbxAcreedorGenerica.GetMainElement().style.border = "solid red";
        else
            CmbxAcreedorGenerica.GetMainElement().style.border = "";
    }

    function OnChangedAcreedorBuscar(s, e) {
        var Acreedor = s.GetText();

        if (Acreedor == null || Acreedor == 'Seleccione' || Acreedor == "")
            CmbxAcreedorBuscar.GetMainElement().style.border = "solid red";
        else
            CmbxAcreedorBuscar.GetMainElement().style.border = "";
    }

    //function OnChangedTipoDocumentoBuscar(s, e) {
    //    var TipoDocumento = s.GetText();

    //    if (TipoDocumento == null || TipoDocumento == 'Seleccione' || TipoDocumento == "")
    //        CmbxTipoDocumentoBuscar.GetMainElement().style.border = "solid red";
    //    else
    //        CmbxTipoDocumentoBuscar.GetMainElement().style.border = "";
    //}

    //function OnChangedTipoDocumentoGenerica(s, e) {
    //    var TipoDocumento = s.GetText();

    //    if (TipoDocumento == null || TipoDocumento == 'Seleccione' || TipoDocumento == "")
    //        CmbxTipoDocumentoGenerica.GetMainElement().style.border = "solid red";
    //    else
    //        CmbxTipoDocumentoGenerica.GetMainElement().style.border = "";
    //}

    function GetCheckBoxValue(s, e) {
        var el = document.getElementById('dvWarning');
        el.style.display = s.GetChecked() ? "block" : "none";
        if (el) {
            lbWarning.SetText('Esta opción creará un tipo de documento génerico para todo los acreedores');
            CmbxAcreedorGenerica.SetText(null);
            CmbxAcreedorGenerica.SetValue(null);
            CmbxAcreedorGenerica.SetEnabled(true);
        }
        else{
            lbWarning.SetText(null);
            CmbxAcreedorGenerica.SetEnabled(true);
        }
    }

    function InsertaTxtDatosGarantiaGenerica() {
        var templateText = LstDatosGarantiaGenerica.GetSelectedItem().text;
        var valor = LstDatosGarantiaGenerica.GetSelectedItem().value;
        HtmlEditorGenerica.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, templateText);
    }

    function InsertaTxtDatosAvalesGenerica() {
        var templateText = LstDatosAvalesGenerica.GetSelectedItem().text;
        var valor = LstDatosAvalesGenerica.GetSelectedItem().value;
        HtmlEditorGenerica.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, templateText);
    }

    function InsertaTxtOtrosGenerica() {
        var templateText = LstOtrosGenerica.GetSelectedItem().text;
        var valor = LstOtrosGenerica.GetSelectedItem().value;
        HtmlEditorGenerica.ExecuteCommand(ASPxClientCommandConsts.PASTEHTML_COMMAND, templateText);
    }

    function OnCallbackComplete(s, e) {
        var hdn = LblTipoDoc.GetText();

        if (hdn == "2") {
            if (e.result != null && e.result != "") {
                var datos = e.result.toString().split('~');
                TxtNombrePlantillaGenerica.SetText(datos[1]);
                HtmlEditorGenerica.SetHtml(datos[0]);
            }
            else {
                HtmlEditorGenerica.SetHtml(null);
                var el = document.getElementById('dvWarning');
                el.style.display = "block";
                lbWarning.SetText('No hay una plantilla generica configurada');
                TxtNombrePlantillaGenerica.SetText('');
            }
        }
        else if (hdn == "1") {
            if (e.result != null && e.result != "") {
                var datos = e.result.toString().split('~');
                TxtNombrePlantilla.SetText(datos[1]);
                HtmlEditor.SetHtml(datos[0]);
            }
            else {
                HtmlEditor.SetHtml(null);
                var el = document.getElementById('dvWarning');
                el.style.display = "block";
                lbWarning.SetText('No hay una plantilla personalizada configurada');
                TxtNombrePlantilla.SetText('');
            }
        }
        else if (hdn == "3") {
            if (e.result == null && e.result == "") {
                var el = document.getElementById('dvWarning');
                el.style.display = "block";
                lbWarning.SetText('Error al ingresar la plantilla');
            }
            else if (e.result == "ok") {
                var el = document.getElementById('dvSuccess');
                el.style.display = "block";
                lbSuccess.SetText('Se ha ingresado la plantilla ' + TxtNombrePlantillaGenerica.GetText());
            }
            else {
                //HtmlEditorGenerica.SetHtml(null);
                var el = document.getElementById('dvWarning');
                el.style.display = "block";
                lbWarning.SetText('Error al ingresar la plantilla');
            }
        }
        CloseDlg()
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
      //SP.SOD.commonModalDialogClose();
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function TxtNombrePlantillaGenerica_LostFocus(s, e) {
        if (s.GetText() == "" && s.GetText() == null)
            TxtNombrePlantillaGenerica.GetMainElement().style.border = "solid red";
        else
            TxtNombrePlantillaGenerica.GetMainElement().style.border = "";
    }

    //function GetValue(s, e) {
    //    if (s.GetHtml() == "" && s.GetHtml() == null)
    //        HtmlEditorGenerica.GetMainElement().style.border = "solid red";
    //    else
    //        HtmlEditorGenerica.GetMainElement().style.border = "";
    //}

    function ocultarDiv() {
        var dvs = document.getElementById('dvSuccess');
        dvs.style.display = 'none';
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'none';
        var dve = document.getElementById('dvError');
        dve.style.display = 'none';
    }

    function stringtoboolean(string){
        switch(string.toLowerCase().trim()){
            case "true": case "yes": case "1": return true;
            case "false": case "no": case "0": case null: return false;
            default: return Boolean(string);
        }
    }

    function OnEndCallback(s, e) {
        if (GvBuscarPlantilla.cpValores != null) {
            var valores = GvBuscarPlantilla.cpValores.split('~');
            OpenDlg();
            if (valores.length > 1) {
                cambio = true;
                HdfC.Set("cambio", "ok");
                HtmlEditorGenerica.SetHtml(valores[1]);
                CmbxTipoProductoGenerica.SetValue(valores[6]);
                OnChangedTipoProductoGenerica(CmbxTipoProductoGenerica);
                CmbxTipoDocumentoGenerica.SetValue(valores[3]);
                CmbxTipoDocumentoGenerica.SetText(valores[7]);
                TxtNombrePlantillaGenerica.SetText(valores[4]);
                TxtNombrePlantillaGenerica.SetValue(valores[4]);
                var valor = stringtoboolean(valores[5])
                ChkGenerica.SetChecked(valor);
                //CmbxTipoProductoGenerica.SetEnabled(false);
                //CmbxTipoDocumentoGenerica.SetEnabled(false);
                //CmbxAcreedorGenerica.SetEnabled(false);
                //ChkGenerica.SetEnabled(false);
                var chk = ChkGenerica.GetChecked();
                if (chk) {
                    Error('Esta plantilla es de tipo generico y los cambios afectaran a todos los acreedores');
                }
                else
                    CmbxAcreedorGenerica.SetValue(valores[2]);
                  
                Prueba(2);
                }
                else if (valores == 'Exito') {
                    
                }
                else if (valores == 'Error') {
                    Error('No se puede mostrar la informacion');
                }
                else if (valores == 'Modificacion') {
                    
                }
                else if (valores == 'eliminar') {
                    GvBuscarPlantilla.PerformCallback("cargar")
                }
            GvBuscarPlantilla.cpValores = null;
            CloseDlg();
        }
    }

    function Error(mensaje) {
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'block';
        lbWarning.SetText(mensaje);
    }

    function OnBtnLimpiar(s, e) {
        CmbxTipoProductoBuscar.SetText(null);
        CmbxTipoProductoBuscar.SetValue(null);
        CmbxTipoDocumentoBuscar.SetText(null);
        CmbxTipoDocumentoBuscar.SetValue(null);
        CmbxAcreedorBuscar.SetText(null);
        CmbxAcreedorBuscar.SetValue(null);
        GvBuscarPlantilla.PerformCallback("cargar");
    }

</script>

    

<!-- Page-Title -->
<div class="row marginInicio">
    <div class="col-sm-12">
        <h4 class="page-title">Creación de plantillas</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Inicio</a>
            </li>
            <li class="active">Creación de plantillas
            </li>
        </ol>
    </div>
</div>


<%-- Cuadros de Alerta --%>
<br />
<div id="dvSuccess" runat="server" clientidmode="Static" style="display: none" class="alert alert-success">
    <h4>
        <dx:ASPxLabel ID="lbSuccess" runat="server" ClientInstanceName="lbSuccess" ClientIDMode="Static" CssClass="alert alert-success" Text=""></dx:ASPxLabel>
    </h4>
</div>
<div id="dvWarning" runat="server" clientidmode="Static" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <dx:ASPxLabel ID="lbWarning" runat="server" ClientInstanceName="lbWarning" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
    </h4>
</div>
<div id="dvError" runat="server" clientidmode="Static" style="display: none" class="alert alert-danger">
    <h4>
        <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
    </h4>
</div>
<br />


<div class="row">
    <div class="col-md-12">
        <dx:ASPxHiddenField ID="HdfC" runat="server" ClientInstanceName="HdfC" ClientIDMode="Static"></dx:ASPxHiddenField>
        <dx:ASPxLabel ID="LblTipoDoc" runat="server" Text="" ClientInstanceName="LblTipoDoc" ClientVisible="false"></dx:ASPxLabel>

        <ul class="nav nav-tabs navtab-bg nav-justified">
            <li id="Tab1" class="active">
                <a href="#panel-tab1" data-toggle="tab" aria-expanded="true" onclick="Prueba(1)">
                    <span class="visible-xs"><i class="fa fa-home"></i></span>
                    <span class="hidden-xs">
                        <asp:Label ID="lbLi1" runat="server" Text="Buscar Plantilla" ClientIDMode="Static"></asp:Label>
                    </span>
                </a>
            </li>

            <li id="Tab2">
                <a href="#panel-tab2" data-toggle="tab" aria-expanded="false" onclick="Prueba(2)">
                    <span class="visible-xs"><i class="fa fa-user"></i></span>
                    <span class="hidden-xs">
                        <asp:Label ID="lbLi3" runat="server" Text="Creacion de plantilla" ClientIDMode="Static"></asp:Label>
                    </span>
                </a>
            </li>

        </ul>

        <div class="tab-content">
            <%--buscar plantillA--%>
            <div class="tab-pane" id="panel-tab1">            
                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <div class="row">    
                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label>Tipo producto:</label>
                                            <dx:ASPxComboBox ID="CmbxTipoProductoBuscar" runat="server" ClientInstanceName="CmbxTipoProductoBuscar" ClientIDMode="Static" CssClass="form-control" NullText="Seleccione">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnChangedTipoProductoBuscar(s); }" />
                                            </dx:ASPxComboBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Acreedor:</label>
                                            <dx:ASPxComboBox ID="CmbxAcreedorBuscar" runat="server" ClientInstanceName="CmbxAcreedorBuscar" ClientIDMode="Static" CssClass="form-control" NullText="Seleccione">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnChangedAcreedorBuscar(s); }" />
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-sm-6">
                                        <div class="form-group">
                                            <label for="ddlTipoDocumento">Tipo documento:</label>
                                            <dx:ASPxComboBox ID="CmbxTipoDocumentoBuscar" runat="server" ClientInstanceName="CmbxTipoDocumentoBuscar" ClientIDMode="Static" CssClass="form-control" OnCallback="CmbxTipoDocumentoBuscar_Callback" NullText="Seleccione">
                                                <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnChangedTipoDocumentoBuscar(s); }" /> --%>
                                            </dx:ASPxComboBox>
                                        </div>
                                        <div class="form-group">
                                            <label>Nombre de plantilla:</label>
                                            <dx:ASPxTextBox ID="TxtNombrePlantillaBuscar" ClientInstanceName="TxtNombrePlantillaBuscar" runat="server" ClientIDMode="Static" CssClass="form-control" Width="100%">
                                            </dx:ASPxTextBox>
                                        </div>
                                    </div>

                                    <table width="100%">
                                        <tr>
                                            <td style="width:80%"></td>

                                            <td>

                                                <dx:ASPxButton ID="BtnBuscarPlantilaBuscar" runat="server" ClientInstanceName="BtnBuscarPlantilaBuscar" Text="Buscar" CssClass="btn btn-primary btn-success pull-right" ClientIDMode="Static" AutoPostBack="false">
                                                    <ClientSideEvents Click="OnBtnBuscarBuscar" />
                                                </dx:ASPxButton>
                                             </td> 
                                            <td> 
                                                <dx:ASPxButton ID="BtnLimpiarBusqueda" runat="server" ClientInstanceName="BtnLimpiarBusqueda" Text="Limpiar busqueda" ClientIDMode="Static" CssClass="btn btn-primary pull-right" AutoPostBack="false">
                                                    <ClientSideEvents Click="OnBtnLimpiar" />
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <div class="row">
                                <div class="col-md-10" style="margin-left: 130px">
                                <div class="form-group">
                                    <dx:ASPxGridView ID="GvBuscarPlantilla" runat="server" ClientInstanceName="GvBuscarPlantilla" KeyFieldName="IdPlantillaDocumento" Width="100%" class="table table-responsive table-hover table-bordered" ClientIDMode="Static"
                                        OnPageIndexChanged="GvBuscarPlantilla_PageIndexChanged" OnCustomButtonCallback="GvBuscarPlantilla_CustomButtonCallback" OnCustomCallback="GvBuscarPlantilla_CustomCallback"
                                        ClientSideEvents-EndCallback="OnEndCallback">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="IdPlantillaDocumento" HeaderStyle-Wrap="True" VisibleIndex="1" Visible="false">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="NombreProducto" Caption="Tipo Producto" HeaderStyle-Wrap="True" VisibleIndex="2">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TipoDocumento" Caption="Tipo Documento" HeaderStyle-Wrap="True" VisibleIndex="3">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Acreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="4">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="NombrePlantilla" Caption="Nombre Plantilla" HeaderStyle-Wrap="True" VisibleIndex="5">
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="EsGenerica" HeaderStyle-Wrap="True" VisibleIndex="6" Visible="false">
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewCommandColumn Caption="Acciones" ButtonType="Image" VisibleIndex="7" Name="Acciones"
                                                CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Descargar">
                                                        <Image ToolTip="Descargar plantilla" Url="../../_layouts/15/images/MultiAdministracion/Editar.png"></Image>
                                                        <Styles Style-CssClass="fa fa-edit paddingIconos"></Styles>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Editar">
                                                        <Image ToolTip="Editar plantilla" Url="../../_layouts/15/images/MultiAdministracion/Editar.png"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="Eliminar">
                                                        <Image ToolTip="Eliminar plantilla" Url="../../_layouts/15/images/MultiAdministracion/Eliminar.png"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>
                                            </dx:GridViewCommandColumn>
                                        </Columns>
                                        <SettingsLoadingPanel Mode="Disabled" />
                                        <SettingsPager PageSize="5" />
                                        <SettingsBehavior AllowSort="false" AllowGroup="false" />
                                    </dx:ASPxGridView>
                                </div>
                            </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <%--plantillA generica--%>
            <div class="tab-pane" id="panel-tab2">   
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label for="CmbxTipoProductoGenerica">Tipo producto:</label>
                                <dx:ASPxComboBox ID="CmbxTipoProductoGenerica" runat="server" ClientInstanceName="CmbxTipoProductoGenerica" ClientIDMode="Static" CssClass="form-control" NullText="Seleccione" ValueType="System.Int32">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnChangedTipoProductoGenerica(s); }" />
                                </dx:ASPxComboBox>
                            </div>
                            <div class="form-group">
                                 <label>Acreedor:</label>
                                <dx:ASPxComboBox ID="CmbxAcreedorGenerica" runat="server" ClientInstanceName="CmbxAcreedorGenerica" ClientIDMode="Static" CssClass="form-control" NullText="Seleccione" ValueType="System.Int32">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnChangedAcreedorGenerica(s); }" />
                                </dx:ASPxComboBox>
                            </div>
                            <div class="form-group">
                                <label>Es genérica:</label>
                                <dx:ASPxCheckBox runat="server" ID="ChkGenerica" ClientInstanceName="ChkGenerica" ClientIDMode="Static" AutoPostBack="false" CssClass="form-control">
                                    <ClientSideEvents CheckedChanged="function(s, e) { 
                                                            GetCheckBoxValue(s, e); 
                                                        }" />
                                </dx:ASPxCheckBox>
                            </div>
                        </div>

                        <div class="col-md-6 col-sm-6">
                            <div class="form-group">
                                <label for="CmbxTipoDocumentoGenerica">Tipo documento:</label>
                                <dx:ASPxComboBox ID="CmbxTipoDocumentoGenerica" runat="server" ClientInstanceName="CmbxTipoDocumentoGenerica" ClientIDMode="Static" CssClass="form-control" OnCallback="CmbxTipoDocumentoGenerica_Callback" NullText="Seleccione" ValueType="System.Int32">
                                    <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnChangedTipoDocumentoGenerica(s); }" /> --%>
                                </dx:ASPxComboBox>
                            </div>
                            <div class="form-group">
                                <label for="TxtNombrePlantillaGenerica">Nombre de plantilla:</label>
                                <dx:ASPxTextBox ID="TxtNombrePlantillaGenerica" ClientInstanceName="TxtNombrePlantillaGenerica" runat="server" ClientIDMode="Static" CssClass="form-control">
                                    <%--<ClientSideEvents LostFocus="TxtNombrePlantillaGenerica_LostFocus" />--%>
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                    </div>
              
                    <div class="row" style=" padding-top: 20% !important">
                            <div class="col-md-2" style="margin-left: 30px">
                                <div class="form-group">
                                    <div class="span12">
                                        <div onclick="Slide('divDatosEmpresaGenerica', 'spanDatosEmpresaGenerica');">
                                            <label><span id="spanDatosEmpresaGenerica" style="width: 100px">+</span>Empresa</label>
                                        </div>
                                        <br />
                                        <div id="divDatosEmpresaGenerica" class="oculto">
                                            <dx:ASPxListBox ID="LstDatosEmpresaGenerica" ClientInstanceName="LstDatosEmpresaGenerica" runat="server" ClientIDMode="Static" ValueType="System.Int32" Border-BorderWidth="0">
                                                <ClientSideEvents ItemDoubleClick="function(s, e) { InsertaTxtGenerica(); }" />
                                            </dx:ASPxListBox>
                                        </div>
                                        <br />
                                        <div onclick="Slide('divDatosOperacionGenerica', 'spanDatosOperacionGenerica');">
                                            <label><span id="spanDatosOperacionGenerica">+</span>Operación</label>
                                        </div>
                                        <br />
                                        <div id="divDatosOperacionGenerica" class="oculto">
                                            <dx:ASPxListBox ID="LstDatosOperacionGenerica" ClientInstanceName="LstDatosOperacionGenerica" runat="server" ClientIDMode="Static" ValueType="System.Int32" Border-BorderWidth="0">
                                                <ClientSideEvents ItemDoubleClick="function(s, e) { InsertaTxtOperacionGenerica(); }" />
                                            </dx:ASPxListBox>
                                        </div>
                                        <br />
                                        <div onclick="Slide('divDatosAcreedorGenerica', 'spanDatosAcreedorGenerica');">
                                            <label><span id="spanDatosAcreedorGenerica">+</span>Acreedor</label>
                                        </div>
                                        <br />
                                        <div id="divDatosAcreedorGenerica" class="oculto">
                                            <dx:ASPxListBox ID="LstDatosAcreedorGenerica" ClientInstanceName="LstDatosAcreedorGenerica" runat="server" ClientIDMode="Static" ValueType="System.Int32" Border-BorderWidth="0">
                                                <ClientSideEvents ItemDoubleClick="function(s, e) { InsertaTxtDatosAcreedorGenerica(); }" />
                                            </dx:ASPxListBox>
                                        </div>
                                        <br />
                                        <div onclick="Slide('divDatosGarantiaGenerica', 'spanDatosGarantiaGenerica');">
                                            <label><span id="spanDatosGarantiaGenerica">+</span>Garantía</label>
                                        </div>
                                        <br />
                                        <div id="divDatosGarantiaGenerica" class="oculto">
                                            <dx:ASPxListBox ID="LstDatosGarantiaGenerica" runat="server" ClientInstanceName="LstDatosGarantiaGenerica" ClientIDMode="Static" ValueType="System.String" Border-BorderWidth="0">
                                                <ClientSideEvents ItemDoubleClick="function(s, e) { InsertaTxtDatosGarantiaGenerica(); }" />
                                            </dx:ASPxListBox>
                                        </div>
                                        <br />
                                        <div onclick="Slide('divDatosAvalesGenerica', 'spanDatosAvalesGenerica');">
                                            <label><span id="spanDatosAvalesGenerica">+</span>Avales</label>
                                        </div>
                                        <br />
                                        <div id="divDatosAvalesGenerica" class="oculto">
                                            <dx:ASPxListBox ID="LstDatosAvalesGenerica" ClientInstanceName="LstDatosAvalesGenerica" runat="server" ClientIDMode="Static" ValueType="System.Int32" Border-BorderWidth="0">
                                                <ClientSideEvents ItemDoubleClick="function(s, e) { InsertaTxtDatosAvalesGenerica(); }" />
                                            </dx:ASPxListBox>
                                        </div>
                                        <br />
                                        <div onclick="Slide('divOtrosGenerica', 'spanOtrosGenerica');">
                                            <label><span id="spanOtrosGenerica">+</span>Otros</label>
                                        </div>
                                        <br />
                                        <div id="divOtrosGenerica" class="oculto">
                                            <dx:ASPxListBox ID="LstOtrosGenerica" ClientInstanceName="LstOtrosGenerica" runat="server" ClientIDMode="Static" ValueType="System.Int32" Border-BorderWidth="0">
                                                <ClientSideEvents ItemDoubleClick="function(s, e) { InsertaTxtOtrosGenerica(); }" />
                                            </dx:ASPxListBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-9">
                                <div class="form-group">
                                    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="ASPxCallback1" OnCallback="ASPxCallback1_Callback" ClientIDMode="Static">
                                        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
                                    </dx:ASPxCallback>

                                    <dx:ASPxHtmlEditor ID="HtmlEditorGenerica" runat="server" Width="100%" Height="700px" ClientInstanceName = "HtmlEditorGenerica">
                                        <Settings AllowContextMenu="True" AllowDesignView="True" AllowHtmlView="False" AllowPreview="True"
                                            AllowInsertDirectImageUrls="true" />
                                        <SettingsDialogs>
                                            <InsertImageDialog>
                                                <SettingsImageUpload UploadFolder="~/UploadFiles/Images/">
                                                    <ValidationSettings AllowedFileExtensions=".jpe,.jpeg,.jpg,.gif,.png" MaxFileSize="500000">
                                                    </ValidationSettings>
                                                </SettingsImageUpload>
                                            </InsertImageDialog>
                                        </SettingsDialogs>
                                        <%--<SettingsLoadingPanel Text="Cargando" ImagePosition="Top" ShowImage="false" Enabled="false" />--%>
                              
                                        <%--<Toolbars>
                                            <dx:HtmlEditorToolbar Name="Toolbar11">
                                                <Items>
                                                    <dx:ToolbarCutButton>
                                                    </dx:ToolbarCutButton>
                                                    <dx:ToolbarCopyButton>
                                                    </dx:ToolbarCopyButton>
                                                    <dx:ToolbarPasteButton>
                                                    </dx:ToolbarPasteButton>
                                                    <dx:ToolbarPasteFromWordButton>
                                                    </dx:ToolbarPasteFromWordButton>
                                                    <dx:ToolbarUndoButton BeginGroup="True">
                                                    </dx:ToolbarUndoButton>
                                                    <dx:ToolbarRedoButton>
                                                    </dx:ToolbarRedoButton>
                                                    <dx:ToolbarRemoveFormatButton BeginGroup="True">
                                                    </dx:ToolbarRemoveFormatButton>
                                                    <dx:ToolbarSuperscriptButton BeginGroup="True">
                                                    </dx:ToolbarSuperscriptButton>
                                                    <dx:ToolbarSubscriptButton>
                                                    </dx:ToolbarSubscriptButton>
                                                    <dx:ToolbarInsertOrderedListButton BeginGroup="True">
                                                    </dx:ToolbarInsertOrderedListButton>
                                                    <dx:ToolbarInsertUnorderedListButton>
                                                    </dx:ToolbarInsertUnorderedListButton>
                                                    <dx:ToolbarIndentButton BeginGroup="True">
                                                    </dx:ToolbarIndentButton>
                                                    <dx:ToolbarOutdentButton>
                                                    </dx:ToolbarOutdentButton>
                                                    <dx:ToolbarInsertLinkDialogButton BeginGroup="True">
                                                    </dx:ToolbarInsertLinkDialogButton>
                                                    <dx:ToolbarUnlinkButton>
                                                    </dx:ToolbarUnlinkButton>
                                                    <dx:ToolbarInsertImageDialogButton>
                                                        <Image ToolTip="Busque Imagen">
                                                        </Image>
                                                    </dx:ToolbarInsertImageDialogButton>
                                                    <dx:ToolbarCheckSpellingButton BeginGroup="True">
                                                    </dx:ToolbarCheckSpellingButton>
                                                    <dx:ToolbarTableOperationsDropDownButton BeginGroup="True">
                                                        <Items>
                                                            <dx:ToolbarInsertTableDialogButton BeginGroup="True" ViewStyle="ImageAndText">
                                                            </dx:ToolbarInsertTableDialogButton>
                                                            <dx:ToolbarTablePropertiesDialogButton BeginGroup="True">
                                                            </dx:ToolbarTablePropertiesDialogButton>
                                                            <dx:ToolbarTableRowPropertiesDialogButton>
                                                            </dx:ToolbarTableRowPropertiesDialogButton>
                                                            <dx:ToolbarTableColumnPropertiesDialogButton>
                                                            </dx:ToolbarTableColumnPropertiesDialogButton>
                                                            <dx:ToolbarTableCellPropertiesDialogButton>
                                                            </dx:ToolbarTableCellPropertiesDialogButton>
                                                            <dx:ToolbarInsertTableRowAboveButton BeginGroup="True">
                                                            </dx:ToolbarInsertTableRowAboveButton>
                                                            <dx:ToolbarInsertTableRowBelowButton>
                                                            </dx:ToolbarInsertTableRowBelowButton>
                                                            <dx:ToolbarInsertTableColumnToLeftButton>
                                                            </dx:ToolbarInsertTableColumnToLeftButton>
                                                            <dx:ToolbarInsertTableColumnToRightButton>
                                                            </dx:ToolbarInsertTableColumnToRightButton>
                                                            <dx:ToolbarSplitTableCellHorizontallyButton BeginGroup="True">
                                                            </dx:ToolbarSplitTableCellHorizontallyButton>
                                                            <dx:ToolbarSplitTableCellVerticallyButton>
                                                            </dx:ToolbarSplitTableCellVerticallyButton>
                                                            <dx:ToolbarMergeTableCellRightButton>
                                                            </dx:ToolbarMergeTableCellRightButton>
                                                            <dx:ToolbarMergeTableCellDownButton>
                                                            </dx:ToolbarMergeTableCellDownButton>
                                                            <dx:ToolbarDeleteTableButton BeginGroup="True">
                                                            </dx:ToolbarDeleteTableButton>
                                                            <dx:ToolbarDeleteTableRowButton>
                                                            </dx:ToolbarDeleteTableRowButton>
                                                            <dx:ToolbarDeleteTableColumnButton>
                                                            </dx:ToolbarDeleteTableColumnButton>
                                                        </Items>
                                                    </dx:ToolbarTableOperationsDropDownButton>
                                                    <dx:ToolbarBoldButton BeginGroup="True">
                                                    </dx:ToolbarBoldButton>
                                                    <dx:ToolbarItalicButton>
                                                    </dx:ToolbarItalicButton>
                                                    <dx:ToolbarUnderlineButton>
                                                    </dx:ToolbarUnderlineButton>
                                                    <dx:ToolbarFontNameEdit>
                                                        <Items>
                                                            <dx:ToolbarListEditItem Text="Arial" Value="Arial" />
                                                            <dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
                                                            <dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
                                                            <dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
                                                        </Items>
                                                    </dx:ToolbarFontNameEdit>
                                                    <dx:ToolbarFontSizeEdit>
                                                        <Items>
                                                            <dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
                                                            <dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
                                                            <dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
                                                            <dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
                                                            <dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
                                                            <dx:ToolbarListEditItem Text="6 (22pt)" Value="6" />
                                                        </Items>
                                                    </dx:ToolbarFontSizeEdit>
                                                    <dx:ToolbarBackColorButton>
                                                    </dx:ToolbarBackColorButton>
                                                    <dx:ToolbarFontColorButton>
                                                    </dx:ToolbarFontColorButton>
                                                    <dx:ToolbarJustifyLeftButton BeginGroup="True">
                                                    </dx:ToolbarJustifyLeftButton>
                                                    <dx:ToolbarJustifyCenterButton>
                                                    </dx:ToolbarJustifyCenterButton>
                                                    <dx:ToolbarJustifyFullButton>
                                                    </dx:ToolbarJustifyFullButton>
                                                    <dx:ToolbarJustifyRightButton>
                                                    </dx:ToolbarJustifyRightButton>
                                                    <dx:ToolbarUndoButton>
                                                    </dx:ToolbarUndoButton>
                                                </Items>
                                            </dx:HtmlEditorToolbar>
                                        </Toolbars>--%>
                                        <%--<ClientSideEvents LostFocus="function(s, e) { GetValue(s, e); }" />--%>
                                    </dx:ASPxHtmlEditor>
                                   
                                </div>
                            </div>
                      </div>
                </div>

                <table width="100%">
                    <tr>
                        <td style="width:80%">
                            <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" Visible="false" />
                        </td>

                        <td>
                            <%--<dx:ASPxButton ID="BtnLimpiar" runat="server" Text="Imprimir en pdf" ClientInstanceName="BtnLimpiar" CssClass="btn btn-primary btn-success pull-right" ClientIDMode="Static" OnClick="BtnLimpiar_Click" Visible="false">
                            </dx:ASPxButton>--%>
                            <dx:ASPxButton ID="BtnCancelar" runat="server" Text="Cancelar" ClientInstanceName="BtnCancelar" CssClass="btn btn-primary pull-right" ClientIDMode="Static" AutoPostBack="false">
                                <ClientSideEvents Click="function(s, e) { Cancelar(s); }" />
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="BtnGuardarPlantilla" runat="server" Text="Guardar Plantilla" ClientInstanceName="BtnGuardarPlantilla" CssClass="btn btn-primary btn-success pull-right" ClientIDMode="Static" AutoPostBack="false">
                                <ClientSideEvents Click="OnbtnGuardar" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </div>
</div>


