<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpCalendarioPago.ascx.cs" Inherits="MultiOperacion.wpCalendarioPago.wpCalendarioPago.wpCalendarioPago" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />

<script type="text/javascript" src="../../_layouts/15/MultiOperacion/Operacion.js"></script>
<script src="../../_layouts/15/MultiOperacion/jquery-1.11.1.js"></script>

<script type="text/javascript">
    document.onkeydown = function () {
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
        }

        if (window.event && (window.event.keyCode == 8)) {
            valor = document.activeElement.value;
            if (valor == undefined) { return false; } //Evita Back en página. 

            else {
                if (document.activeElement.getAttribute('type') == 'select-one')
                { return false; } //Evita Back en select. 
                if (document.activeElement.getAttribute('type') == 'button')
                { return false; } //Evita Back en button. 
                if (document.activeElement.getAttribute('type') == 'radio')
                { return false; } //Evita Back en radio. 
                if (document.activeElement.getAttribute('type') == 'checkbox')
                { return false; } //Evita Back en checkbox. 
                if (document.activeElement.getAttribute('type') == 'file')
                { return false; } //Evita Back en file. 
                if (document.activeElement.getAttribute('type') == 'reset')
                { return false; } //Evita Back en reset. 
                if (document.activeElement.getAttribute('type') == 'submit')
                { return false; } //Evita Back en submit. 
                if (document.activeElement.getAttribute('type') == 'input')
                { return false; } //Evita Back en inout. 
                else //Text, textarea o password 
                {
                    if (document.activeElement.value.length == 0)
                    { return false; } //No realiza el backspace(largo igual a 0). 
                    else {
                        document.activeElement.value.keyCode = 8;
                    } //Realiza el backspace. 
                }
            }

            if (window.event && (window.event.keyCode == 13)) {
                valor = document.activeElement.value;
                if (valor == undefined) { return false; } //Evita Enter en página. 
                else {
                    if (document.activeElement.getAttribute('type') == 'select-one')
                    { return false; } //Evita Enter en select. 
                    if (document.activeElement.getAttribute('type') == 'button')
                    { return false; } //Evita Enter en button. 
                    if (document.activeElement.getAttribute('type') == 'radio')
                    { return false; } //Evita Enter en radio. 
                    if (document.activeElement.getAttribute('type') == 'checkbox')
                    { return false; } //Evita Enter en checkbox. 
                    if (document.activeElement.getAttribute('type') == 'file')
                    { return false; } //Evita Enter en file. 
                    if (document.activeElement.getAttribute('type') == 'reset')
                    { return false; } //Evita Enter en reset. 
                    if (document.activeElement.getAttribute('type') == 'submit')
                    { return false; } //Evita Enter en submit. 
                    if (document.activeElement.getAttribute('type') == 'input')
                    { return false; } //Evita Back en input. 
                    else //Text, textarea o password 
                    {
                        if (document.activeElement.value.length == 0)
                        { return false; }
                    }
                    if (document.getElementById('<%=txtRUT.ClientID %>').val != '' && document.getElementById('<%=txtCertificado.ClientID %>').val != '') {
                        document.getElementById('<%=btnBuscar.ClientID %>').click();
                    }
                }
            }
        }
    }

    function rowno(rowindex) {
        var cadena = rowindex.substr(0, 1);
        rowindex = rowindex.substr(1, rowindex.length - 1);
        var i, CellValue, Row, td, celltext;
        i = parseInt(rowindex) + 1;


        //Row = table.rows[i];

        //if (cadena == 'C') {
        //    Row.cells[10].childNodes[0].children[0].checked = false;
        //    Row.cells[11].childNodes[0].children[0].checked = false;
        //}

        //if (cadena == 'M') {
        //    Row.cells[9].childNodes[0].children[0].checked = false;
        //    Row.cells[11].childNodes[0].children[0].checked = false;
        //}

        //if (cadena == 'S'){
        //    Row.cells[9].childNodes[0].children[0].checked = false;
        //    Row.cells[10].childNodes[0].children[0].checked = false;
        //}

        return false;
    }

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");

        filtrarGrid();
    });

    function AgregarCalendario() {
        btnCargarCalendario.SetVisible(true)
        btnSubirCalendario.SetVisible(false);
        gvPreCargaCP.Refresh();
        pcCargarCalendario.Show();
    }

    function CargarCalendario(s, e) {
        cbpCargarExcel.PerformCallback('cargarExcel');
    }

    function OnReporte(s, e) {
        //var rowVisibleIndex = e.visibleIndex;

        ////var key = s.GetRowKey(rowVisibleIndex);
        ////var boton = 'incluirReporte';
        ////var select = e.isSelected;

        //var parametros = new Array(key, boton, select);

        //window.gvCalendarioPago.PerformCallback(parametros);
    }

    function EndgvCalendarioPago() {
        //if (gvCalendarioPago.cpGrilla == 'eliminar') {
        //    // || gvCalendarioPago.cpGrilla == 'modificar'
        //    filtrarGrid();
        //}
    }

    function OnFileUploadComplete(s, e) {
        try {
            btnCargarCalendario.SetVisible(true);
            var valores = e.callbackData.split(';');
            if (e.isValid && valores[0] == "OK") {
                gvPreCargaCP.PerformCallback("nuevo");
                btnSubirCalendario.SetVisible(true);
            } else {
                alert(valores[0]);
            }
        } catch (e) {
            alert(e);
        }
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function EndCargaExcel(s, e) {
        var valores = e.result.split(';');
        if (valores[0] == 'OK') {
            gvPreCargaCP.Refresh();
            txtCapitalInicial.SetText(null);
            pcCargarCalendario.Hide();
            alert(valores[1]);
        }
        else {
            alert(valores[0]);
        }
        CloseDlg();
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function SubirExcel(s, e) {
        if (window.ASPxClientEdit.ValidateGroup('ValidarExcel') && UpSubirCalendario.GetText() != '') {
            btnCargarCalendario.SetVisible(false);
            UpSubirCalendario.Upload();
        }
        else
            alert('Ingrese informacion');
    }

    function BorrarCalendario() {
        gvPreCargaCP.Refresh();
        btnSubirCalendario.SetVisible(false);
        gvPreCargaCP.PerformCallback("limpiar");
    }

    function SubirCalendario() {
        btnSubirCalendario.SetVisible(false);
        cbpCargarExcel.PerformCallback("InsertarCalendario");
    }

    function EndgvPreCargaCP() {

    }

    function esRutDV(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^([a-zA-Z0-9]+)$/;
        //var regex = /^(?=.*[0-9])([a-zA-Z0-9]+)$/;
        //var regex = /^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$/;
        //var regex = /^[0-9.\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

    function OnBtnBuscar(s, e) {
        filtrarGrid();
    }

    function filtrarGrid() {
        var acreedor = ddlAcreedor.GetSelectedItem().value;
        var rut = txtRUT.GetText();
        var certificado = txtCertificado.GetText();
        var chkVencidos = ckbVencidos.GetChecked();
        var chkPagados = ckbPagados.GetChecked();
        var chkPendientes = ckbPendiente.GetChecked();
        var chkProximos = ckbProximo.GetChecked();

        var parametros = new Array('buscar', acreedor, rut, certificado, chkVencidos, chkPagados, chkPendientes, chkProximos);

        if (parametros != null) {
            window.gvCalendarioPago.PerformCallback(parametros);
        }
    }

    //evitar edicion formulario sharepoint
    function WebForm_OnSubmit() {
        return false;
    }

    function esNumero(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^[0-9,.\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

    function formato(s, e) {
        var monto = encodeHtml(s.GetText());
        if (monto != '') {
            //monto = parseFloat(monto.replace(/\./g, '').replace(/\,/g, '.'));
            monto = parseFloat(monto.replace(/\./g, ''));
            //monto = parseFloat(monto);
            monto = formatNumber.new(parseFloat(monto).toFixed(0));
            s.SetText(monto);
        }
    }

    function formatoDecimal(s, e) {
        var monto = encodeHtml(s.GetText());
        if (monto != '') {
            monto = parseFloat(monto.replace(/\./g, ''));
            //monto = parseFloat(monto.replace(/\./g, ''));
            //monto = parseFloat(monto);
            monto = formatNumber.new(monto);
            //monto = formatNumber.new(parseFloat(monto).toFixed(4));
            s.SetText(monto);
        }
    }

    /**
    * Funcion que devuelve un numero separando los separadores de miles
    * Puede recibir valores negativos y con decimales
    */
    function numberFormat(s, e) {
        // Variable que contendra el resultado final
        var resultado = "";
        var numero = encodeHtml(s.GetText());

        // Si el numero empieza por el valor "-" (numero negativo)
        if (numero[0] == "-") {
            // Cogemos el numero eliminando los posibles puntos que tenga, y sin
            // el signo negativo
            nuevoNumero = numero.replace(/\./g, '').substring(1);
        } else {
            // Cogemos el numero eliminando los posibles puntos que tenga
            nuevoNumero = numero.replace(/\./g, '');
        }

        // Si tiene decimales, se los quitamos al numero
        if (numero.indexOf(",") >= 0)
            nuevoNumero = nuevoNumero.substring(0, nuevoNumero.indexOf(","));

        // Ponemos un punto cada 3 caracteres
        for (var j, i = nuevoNumero.length - 1, j = 0; i >= 0; i--, j++)
            resultado = nuevoNumero.charAt(i) + ((j > 0) && (j % 3 == 0) ? "." : "") + resultado;

        // Si tiene decimales, se lo añadimos al numero una vez forateado con 
        // los separadores de miles
        if (numero.indexOf(",") >= 0)
            resultado += numero.substring(numero.indexOf(","));

        if (numero[0] == "-") {
            // Devolvemos el valor añadiendo al inicio el signo negativo
            s.SetText("-" + resultado);
        } else {
            s.SetText(resultado);
        }
    }

    function encodeHtml(text) {
        if (text === undefined || text === null)
            return "";
        var replacements = [
            [/&amp;/g, '&ampx;'], [/&/g, '&amp;'], [/&quot;/g, '&quotx;'], [/"/g, '&quot;'],
            [/&lt;/g, '&ltx;'], [/</g, '&lt;'], [/&gt;/g, '&gtx;'], [/>/g, '&gt;']
        ];
        for (var i = 0; i < replacements.length; i++) {
            var repl = replacements[i];
            text = text.replace(repl[0], repl[1]);
        }
        return text;
    }

    var formatNumber = {
        separador: ".", // separador para los miles
        sepDecimal: ',', // separador para los decimales
        formatear: function (num) {
            num += '';
            var splitStr = num.split('.');
            var splitLeft = splitStr[0];
            var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
            var regx = /(\d+)(\d{3})/;
            while (regx.test(splitLeft)) {
                splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
            }
            return this.simbol + splitLeft + splitRight;
        },
        new: function (num, simbol) {
            this.simbol = simbol || '';
            return this.formatear(num);
        }
    }

    function BtnClick(s, e) {
        var boton = e.buttonID;
        var rowVisibleIndex = e.visibleIndex;
        HdfIndexPago.Set("value", s.GetRowKey(e.visibleIndex));

        if (boton == 'pagar') {
            pcPagarFila.Show();
        }

    }

    function PagarCuotaFila(s, e) {
        if (window.ASPxClientEdit.ValidateGroup('ValidarFila')) {
            cbpPagarFila.PerformCallback('pagarfila');
            //var parametros = new Array('pagarfila', HdfIndexPago.Get("value"), FechaPagoFila.GetText, OrigenPagoFila.GetSelectedItem().value);
            //window.gvCalendarioPago.PerformCallback(parametros);
        }
    }

    function EndPagarFila(s, e) {
        FechaPagoFila.SetText(null);
        OrigenPagoFila.SetSelectedIndex(-1);;
        var valores = e.result.split(';');
        if (valores[0] == 'OK') {
            filtrarGrid();
            pcPagarFila.Hide();
        }
        else {
            alert('Error al actulaizar el calendario');
        }
    }

    function PagarMasivo() {
        pcPagarMasivo.Show();
    }

    function PagarCuotaMasivo(s, e) {
        if (window.ASPxClientEdit.ValidateGroup('ValidarMasivo')) {
            cbpPagarMasivo.PerformCallback('pagarMasivo');
        }
    }

    function EndPagarMasivo(s, e) {
        var valores = e.result.split(';');
        if (valores[0] == 'OK') {
            txtCertificadoMasivo.SetText(null);
            FechaPagoMasivo.SetText(null);
            OrigenPagoMasivo.SetSelectedIndex(-1);;
            filtrarGrid();
            pcPagarMasivo.Hide();
        }
        else if (valores[0] == 'informacion') {
            lblInformacionPagoMasivo.SetText(valores[1]);
            //btnPagarCuotaMasivo.SetVisible = false;
        }
        else {
            txtCertificadoMasivo.SetText(null);
            FechaPagoMasivo.SetText(null);
            OrigenPagoMasivo.SetSelectedIndex(-1);;
            alert('Error al actulaizar la información');
        }
    }

    function buscarCertificado() {
        if (txtCertificadoMasivo.GetText() != '')
            cbpPagarMasivo.PerformCallback('informacionCertificado');
    }

    function OnGotFocus(s, e) {
        var formattedText = s.GetInputElement().value.trim();
        s.SetValue(formattedText);
    }

    function OnLostFocus(s, e) {
        var textValue = s.GetValue();
        var newValue = textValue.split(s.cp_separator).join("");
        s.SetValue(newValue);
    }

    function OnBtnVolver() {
        cbpVolver.PerformCallback('volver');
    }

    //function onChangeOrigenPago(s, e) {
    //    if (s.GetText() == 'Cliente') {
    //        var div = document.getElementById("divTipoPagoCalendario");
    //        div.style.visibility = 'visible';
    //        div.style.display = 'block';
    //        rdblTipoPagoCliente.ValidationSetting = true;       
    //    }
    //    else {
    //        var div = document.getElementById("divTipoPagoCalendario");
    //        div.style.visibility = 'hidden';
    //        div.style.display = 'none';
    //        rdblTipoPagoCliente.isValid = false;
    //        rdblTipoPagoCliente.ValidateGroup = null;
    //    }
    //}

    function LimpiarFormMasivo() {
        lblInformacionPagoMasivo.SetText(null);
        txtCertificadoMasivo.SetText(null);
        FechaPagoMasivo.SetText(null);
        OrigenPagoMasivo.SetSelectedIndex(-1);
        btnPagarCuotaMasivo.SetVisible = false;
        //rdblTipoPagoCliente.SetValue(null);
        //var div = document.getElementById("divTipoPagoCalendario");
        //div.style.visibility = 'hidden';
        //div.style.display = 'none';
    }

</script>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Calendario de Pago</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Inicio</a>
                </li>
                <li class="active">Calendario de Pago
                </li>
            </ol>
        </div>
    </div>

    <br />
    <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">
        <h4>
            <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
        </h4>
    </div>

    <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
        <h4>
            <asp:Label ID="lbWarning" runat="server" Text=""></asp:Label>
        </h4>
    </div>

    <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
        <h4>
            <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
        </h4>
    </div>
    <br />


    <!-- tabs -->
    <div class="row">
        <div class="col-md-12">
            <ul class="nav nav-tabs navtab-bg nav-justified">
                <li class="">
                    <a href="#tab1" data-toggle="tab" aria-expanded="false">
                        <span class="visible-xs"><i class="fa fa-user"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbSeguimiento" runat="server" OnClientClick="return Dialogo();" OnClick="lbSeguimiento_Click">Cartera</asp:LinkButton>
                        </span>
                    </a>
                </li>
                <li class="active">
                    <a href="#tab2" data-toggle="tab" aria-expanded="true">
                        <span class="visible-xs"><i class="fa fa-home"></i></span>
                        <span class="hidden-xs">
                            <asp:LinkButton ID="lbCalendarioPago" runat="server" OnClientClick="return Dialogo();" OnClick="lbCalendarioPago_Click">Calendario de Pago</asp:LinkButton>
                        </span>
                    </a>
                </li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane " id="tab1">
                </div>
                <div class="tab-pane active" id="tab2">
                    <div class="row">
                        <!-- col 1/4 -->
                        <div class="col-md-3 col-sm-6">
                            <div class="form-group">
                                <label for="ddlAcreedor">Acreedor:</label>
                                <dx:ASPxComboBox ID="ddlAcreedor" runat="server" ClientInstanceName="ddlAcreedor" ValueType="System.String" CssClass="form-control" NullText="Seleccione">
                                </dx:ASPxComboBox>
                            </div>

                            <div class="form-group">
                                <label for="ckbVencidos"></label>
                                <dx:ASPxCheckBox ID="ckbVencidos" runat="server" ClientInstanceName="ckbVencidos" Text="Vencidos" CssClass="form-control">
                                </dx:ASPxCheckBox>
                            </div>
                        </div>
                        <!-- col 2/4 -->
                        <div class="col-md-3 col-sm-6">
                            <div class="form-group">
                                <label for="txtRUT">Rut: (123456789-k)</label>
                                <dx:ASPxTextBox ID="txtRUT" runat="server" ClientInstanceName="txtRUT" CssClass="form-control" MaxLength="12">
                                    <%--<ClientSideEvents KeyPress="function(s,e){ esRutDV(s,e);}" />--%>
                                </dx:ASPxTextBox>
                            </div>
                            <div class="form-group">
                                <label for="ckbPagados"></label>
                                <dx:ASPxCheckBox ID="ckbPagados" runat="server" ClientInstanceName="ckbPagados" Text="Pagados" CssClass="form-control">
                                </dx:ASPxCheckBox>
                            </div>
                        </div>
                        <!-- col 3/4 -->
                        <div class="col-md-3 col-sm-6">
                            <div class="form-group">
                                <label for="txtCertificado">Certificado:</label>
                                <dx:ASPxTextBox ID="txtCertificado" runat="server" ClientInstanceName="txtCertificado" CssClass="form-control" 
                                 MaxLength="10"></dx:ASPxTextBox>
                            </div>
                            <div class="form-group">
                                <label for="ckbPendiente"></label>
                                <dx:ASPxCheckBox ID="ckbPendiente" runat="server" ClientInstanceName="ckbPendiente" Text="Pendiente Mes" 
                                 CssClass="form-control"></dx:ASPxCheckBox>
                            </div>
                        </div>
                        <!-- col 4/4 -->
                        <div class="col-md-2 col-sm-2">
                            <div class="form-group">
                                <br />
                                <br />
                                <br />                                                   
                            </div>
                            <div class="form-group">
                                <label for=""></label>
                                <dx:ASPxCheckBox ID="ckbProximo" runat="server" ClientInstanceName="ckbProximo" Text="Proximos a Vencer" CssClass="form-control"></dx:ASPxCheckBox>
                            </div>
                            <div class="form-group rpsForm">
                                <dx:ASPxButton ID="btnBuscar" runat="server" ClientInstanceName="btnBuscar" Text="Buscar" Style="margin-left: 0px;" CssClass="btn btn-primary btn-success">
                                    <ClientSideEvents Click="OnBtnBuscar" />
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <table style="width: 100%; table-layout: fixed;">
                                    <tr>
                                        <td>
                                            <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="true" CssClass="btn btn-default pull-left" Text="Volver Atrás" OnClick="ASPxButton1_Click" Width="5%" Height="5%">
                                                <ClientSideEvents Click="OnBtnVolver" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <!-- tabla / grilla -->
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card-box">
                                <dx:ASPxButton ID="BtnPagoMasivo" ClientInstanceName="BtnPagoMasivo" runat="server" Text="Pagar Totalidad Certificado" RenderMode="Button"
                                    AutoPostBack="false" CssClass="btn btn-primary btn-success" Width="10%">
                                    <ClientSideEvents Click="PagarMasivo" />
                                </dx:ASPxButton>

                                <br />
                                <br />

                                <table border="0" style="table-layout: fixed; width: 100%;">
                                    <tr>
                                        <td>
                                            <dx:ASPxGridView ID="gvCalendarioPago" runat="server" ClientInstanceName="gvCalendarioPago" OnPageIndexChanged="gvCalendarioPago_PageIndexChanged" Width="100%"
                                                AutoGenerateColumns="false" KeyFieldName="IdCalendario"
                                                ClientSideEvents-EndCallback="EndgvCalendarioPago"
                                                OnRowUpdating="gvCalendarioPago_RowUpdating"
                                                OnRowDeleting="gvCalendarioPago_RowDeleting"
                                                OnCustomCallback="gvCalendarioPago_CustomCallback"
                                                OnDataBound="gvCalendarioPago_DataBound">
                                                <ClientSideEvents CustomButtonClick="BtnClick" />
                                                <%--<ClientSideEvents SelectionChanged="OnReporte" />
                                                    OnRowInserting="gvCalendarioPago_RowInserting"
                                                    OnCellEditorInitialize="gvCalendarioPago_CellEditorInitialize"
                                                --%>
                                                <SettingsBehavior ConfirmDelete="true" />
                                                <SettingsText ConfirmDelete="¿Esta seguro de borrar este registro?" />
                                                <Columns>
                                                    <dx:GridViewCommandColumn VisibleIndex="0" Width="5%" ShowEditButton="true" ShowDeleteButton="true">
                                                        <HeaderTemplate>
                                                            <dx:ASPxButton ID="BtnNew" ClientInstanceName="BtnNew" runat="server" Text="Carga Masiva" RenderMode="Button"
                                                                AutoPostBack="false" CssClass="btn btn-block btn-warning btn-xs" Width="10%">
                                                                <ClientSideEvents Click="AgregarCalendario" />
                                                            </dx:ASPxButton>
                                                        </HeaderTemplate>
                                                    </dx:GridViewCommandColumn>

                                                    <dx:GridViewDataTextColumn FieldName="NCredito" Caption="N° Credito" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center"
                                                        ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="20" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="NCertificado" Caption="N° Certificado" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesTextEdit-MaxLength="6" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="CuotaN" Caption="Cuota N°" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesTextEdit-MaxLength="3" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true" PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="NCuota" Caption="N° Cuota" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesTextEdit-MaxLength="3" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true" PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                        <%--  <EditItemTemplate>
                                                            <dx:ASPxTextBox ID="txtTotalCuota" runat="server" Width="90%" ClientInstanceName="txtTotalCuota" MaxLength="3" Text='<%# Eval("NCuota") %>'>
                                                                <ClientSideEvents KeyPress="function(s,e){ esNumero(s,e);}" />
                                                                <ValidationSettings>
                                                                    <RequiredField IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxTextBox>
                                                        </EditItemTemplate>--%>
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataDateColumn FieldName="FechaVencimiento" Caption="Fecha Vencimiento" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" PropertiesDateEdit-ValidationSettings-RequiredField-IsRequired="true"
                                                        PropertiesDateEdit-NullText="Fecha de Vencimiento">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                    </dx:GridViewDataDateColumn>

                                                    <dx:GridViewDataTextColumn FieldName="MontoCuota" Caption="Valor Cuota" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true"
                                                        PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}"
                                                        PropertiesTextEdit-ClientSideEvents-KeyUp="function(s,e){ numberFormat(s,e);}">
                                                        <%--PropertiesTextEdit-ClientSideEvents-LostFocus="function(s,e){ formato(s,e);}"--%>
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />

                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="Capital" Caption="Capital" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true"
                                                        PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}"
                                                        PropertiesTextEdit-ClientSideEvents-KeyUp="function(s,e){ numberFormat(s,e);}">
                                                        <%--PropertiesTextEdit-ClientSideEvents-LostFocus="function(s,e){ formato(s,e);}"--%>
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="Interes" Caption="Interés" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                        PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true"
                                                        PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}"
                                                        PropertiesTextEdit-ClientSideEvents-KeyUp="function(s,e){ numberFormat(s,e);}">
                                                        <%--PropertiesTextEdit-ClientSideEvents-LostFocus="function(s,e){ formato(s,e);}">--%>
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataTextColumn FieldName="Moneda" Caption="Tipo Moneda" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                        <EditFormSettings Visible="False" />
                                                    </dx:GridViewDataTextColumn>

                                                    <dx:GridViewDataComboBoxColumn FieldName="IdOrigenPago" Caption="Origen de Pago" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.String" TextField="DescPago"
                                                            ValueField="IdOrigenPago" DataSourceID="dsOrigenPago">
                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesComboBox>
                                                    </dx:GridViewDataComboBoxColumn>

                                                    <dx:GridViewDataDateColumn FieldName="FechaPago" Caption="Fecha Pago" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                                        PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                        <PropertiesDateEdit>
                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="True" />
                                                            </ValidationSettings>
                                                        </PropertiesDateEdit>
                                                    </dx:GridViewDataDateColumn>

                                                    <dx:GridViewCommandColumn Caption="Pagar" VisibleIndex="12" Name="Acciones" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ButtonType="Image">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                        <CustomButtons>
                                                            <dx:GridViewCommandColumnCustomButton ID="pagar" Text="" Image-ToolTip="Pagar">
                                                                <Image Url="../../_layouts/15/images/MultiGestion/font-azul.png" Height="15px" Width="15px">
                                                                </Image>
                                                                <Styles Style-CssClass="data-toggle data-placement data-original-title">
                                                                </Styles>
                                                            </dx:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                    </dx:GridViewCommandColumn>

                                                </Columns>

                                                <SettingsBehavior AllowSort="false" />
                                                <SettingsPager PageSize="20"></SettingsPager>
                                                <SettingsCommandButton>
                                                    <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                                    <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                                </SettingsCommandButton>
                                                <SettingsCommandButton>
                                                    <UpdateButton Text="Guardar"></UpdateButton>
                                                    <CancelButton Text="Cancelar"></CancelButton>
                                                </SettingsCommandButton>
                                                <SettingsEditing EditFormColumnCount="4" NewItemRowPosition="Bottom" />

                                            </dx:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <dx:ASPxPopupControl ID="pcCargarCalendario" runat="server" ClientInstanceName="pcCargarCalendario" CloseAction="CloseButton"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        HeaderText="Carga masiva calendario" HeaderStyle-Font-Bold="true"
                        AllowDragging="True"
                        PopupAnimationType="None"
                        Width="1000px" Height="700px">
                        <HeaderStyle Font-Bold="True" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1">
                                <div class="row">

                                    <div class="col-md-12">
                                        <div class="col-md-4">
                                            <dx:ASPxUploadControl ID="UpSubirCalendario" ShowUploadButton="false" runat="server" ClientInstanceName="UpSubirCalendario" UploadMode="Standard"
                                                ShowProgressPanel="false" Theme="MetropolisBlue" FileUploadMode="OnPageLoad"
                                                OnFileUploadComplete="UpSubirCalendario_FileUploadComplete" CssClass="form-control">
                                                <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".csv" MaxFileSizeErrorText="El tamaño del archivo excede el límite permitido"
                                                    GeneralErrorText="Error al subir archivo"
                                                    NotAllowedFileExtensionErrorText="Extension no permitida">
                                                </ValidationSettings>
                                                <ClientSideEvents
                                                    FileUploadComplete="OnFileUploadComplete" />
                                            </dx:ASPxUploadControl>
                                        </div>
                                        <div class="col-md-4">
                                            <dx:ASPxTextBox ID="txtCapitalInicial" runat="server" ClientInstanceName="txtCapitalInicial" Width="100%" ValidationSettings-ValidationGroup="ValidarExcel"
                                                MaxLength="15" NullText="Ingrese Capital Inicial" ClientSideEvents-KeyUp="function(s,e){ numberFormat(s,e);}"
                                                CssClass="form-control">
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <dx:ASPxButton ID="btnCargarCalendario" runat="server" ClientInstanceName="btnCargarCalendario" Text="Cargar Calendario" AutoPostBack="false" ValidationGroup="ValidarExcel"
                                                CssClass="btn btn-primary btn-success" ClientSideEvents-Click="SubirExcel">
                                            </dx:ASPxButton>
                                        </div>
                                    </div>

                                    <br />
                                    <br />
                                    <br />

                                    <div class="card-box">
                                        <table border="0" style="table-layout: fixed; width: 100%;">
                                            <tr>
                                                <td>
                                                    <dx:ASPxGridView ID="gvPreCargaCP" runat="server" ClientInstanceName="gvPreCargaCP" OnPageIndexChanged="gvPreCargaCP_PageIndexChanged" Width="100%"
                                                        OnCustomCallback="gvPreCargaCP_CustomCallback" ClientSideEvents-EndCallback="EndgvPreCargaCP"
                                                        AutoGenerateColumns="false" KeyFieldName="Id">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="NCredito" Caption="N Credito" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="NCertificado" Caption="N Certificado" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="CuotaN" Caption="Cuota N" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="NCuota" Caption="N Cuota" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="FechaVencimiento" Caption="Fecha vencimiento" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center"
                                                                ShowInCustomizationForm="true">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="MontoCuota" Caption="Monto cuota" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="C2">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Capital" Caption="Capital" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="C2">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Interes" Caption="Interes" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="C2">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                            <dx:GridViewDataTextColumn FieldName="Moneda" Caption="Moneda" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center">
                                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>

                                                        <SettingsBehavior AllowSort="false" />
                                                        <SettingsPager PageSize="20"></SettingsPager>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                    <br />
                                    <br />
                                    <div class="col-md-6">
                                        <dx:ASPxButton ID="btnBorrarCarga" runat="server" ClientInstanceName="btnBorrarCarga" Text="Borrar Calendario" AutoPostBack="false"
                                            CssClass="btn btn-warning">
                                            <ClientSideEvents Click="BorrarCalendario" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="col-md-6">
                                        <dx:ASPxButton ID="btnSubirCalendario" runat="server" ClientInstanceName="btnSubirCalendario" Text="Ingresar Calendario" AutoPostBack="false"
                                            CssClass="btn btn-primary btn-success">
                                            <ClientSideEvents Click="SubirCalendario" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>


                    <dx:ASPxPopupControl ID="pcPagarFila" runat="server" ClientInstanceName="pcPagarFila" CloseAction="CloseButton"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        HeaderText="Pagar Cuota Calendario" HeaderStyle-Font-Bold="true"
                        AllowDragging="True"
                        PopupAnimationType="None"
                        Width="600px" Height="200px">
                        <HeaderStyle Font-Bold="True" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl2">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="">Fecha Pago:</label>
                                                <dx:ASPxDateEdit ID="FechaPagoFila" runat="server" ClientInstanceName="FechaPagoFila" DisplayFormatString="dd-MM-yyyy" EditFormat="Date" Width="100%"
                                                    EditFormatString="dd-MM-yyyy" ValidationSettings-ValidationGroup="ValidarFila" CssClass="form-control">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="">Origen de Pago:</label>
                                                <dx:ASPxComboBox ID="OrigenPagoFila" runat="server" ValueType="System.String" ClientInstanceName="OrigenPagoFila" CssClass="form-control" Width="100%"
                                                    DataSourceID="dsOrigenPagoFila" ValueField="IdOrigenPago" TextField="DescPago" NullText="Seleccione" ValidationSettings-ValidationGroup="ValidarFila">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>

                                        <br />
                                        <br />
                                        <br />

                                        <div class="col-md-12">
                                            <dx:ASPxButton ID="btnPagarCuotaFila" runat="server" ClientInstanceName="btnPagarCuotaFila" Text="Pagar Cuota" AutoPostBack="false" ValidationGroup="ValidarFila"
                                                CssClass="btn btn-primary btn-success" ClientSideEvents-Click="PagarCuotaFila">
                                            </dx:ASPxButton>
                                        </div>
                                    </div>

                                    <br />
                                    <br />
                                    <br />

                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>


                    <dx:ASPxPopupControl ID="pcPagarMasivo" runat="server" ClientInstanceName="pcPagarMasivo" CloseAction="CloseButton"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        HeaderText="Pagar totalidad de cuotas de un certificado" HeaderStyle-Font-Bold="true"
                        AllowDragging="True"
                        PopupAnimationType="None"
                        Width="600px" Height="200px">
                        <HeaderStyle Font-Bold="True" />
                        <ClientSideEvents Closing="LimpiarFormMasivo" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl3">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="">Ingrese Número de Certificado:</label>
                                                <dx:ASPxTextBox ID="txtCertificadoMasivo" runat="server" ClientInstanceName="txtCertificadoMasivo" Width="100%" ValidationSettings-ValidationGroup="ValidarMasivo"
                                                    MaxLength="10" NullText="Ingrese N° Certificado" ClientSideEvents-LostFocus="buscarCertificado" CssClass="form-control">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <dx:ASPxLabel ID="lblInformacionPagoMasivo" runat="server" Text="" ClientInstanceName="lblInformacionPagoMasivo"></dx:ASPxLabel>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="">Fecha Pago:</label>
                                                <dx:ASPxDateEdit ID="FechaPagoMasivo" runat="server" ClientInstanceName="FechaPagoMasivo" DisplayFormatString="dd-MM-yyyy" EditFormat="Date" Width="100%"
                                                    EditFormatString="dd-MM-yyyy" ValidationSettings-ValidationGroup="ValidarMasivo" CssClass="form-control">
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="">Origen de Pago:</label>
                                                <dx:ASPxComboBox ID="OrigenPagoMasivo" runat="server" ValueType="System.String" ClientInstanceName="OrigenPagoMasivo" CssClass="form-control" Width="100%"
                                                    DataSourceID="dsOrigenPagoTotalidad" ValueField="IdOrigenPago" TextField="DescPago" NullText="Seleccione" ValidationSettings-ValidationGroup="ValidarMasivo">
                                                    <%--<ClientSideEvents SelectedIndexChanged="onChangeOrigenPago" />--%>
                                                    <ValidationSettings>
                                                        <RequiredField IsRequired="True" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                    </div>
                                  <%--  <div class="col-md-12">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for=""></label>
                                            </div>
                                        </div>
                                        <div class="col-md-6" id="divTipoPagoCalendario" style="display: none">
                                            <div class="form-group">
                                                <label for="">Tipo Pago Calendario:</label>
                                                <dx:ASPxRadioButtonList ID="rdblTipoPagoCliente" ClientInstanceName="rdblTipoPagoCliente" runat="server"
                                                    ValueType="System.String" ValidationSettings-ValidationGroup="ValidarMasivo">
                                                    <Items>
                                                        <dx:ListEditItem Text="Cliente" Value="Cliente" />
                                                        <dx:ListEditItem Text="Prepago" Value="Prepago" />
                                                    </Items>
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                        <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                        <RequiredField IsRequired="true" />
                                                    </ValidationSettings>
                                                </dx:ASPxRadioButtonList>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <br />
                                    <br />
                                    <br />

                                    <div class="col-md-12">
                                        <dx:ASPxButton ID="btnPagarCuotaMasivo" runat="server" ClientInstanceName="btnPagarCuotaMasivo" Text="Pagar Cuota" AutoPostBack="false" ValidationGroup="ValidarMasivo"
                                            CssClass="btn btn-primary btn-success" ClientSideEvents-Click="PagarCuotaMasivo">
                                        </dx:ASPxButton>
                                    </div>

                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                </div>
            </div>
        </div>
    </div>

    <dx:ASPxCallback ID="cbpCargarExcel" runat="server" ClientInstanceName="cbpCargarExcel" ClientIDMode="Static" OnCallback="cbpCargarExcel_Callback">
        <ClientSideEvents CallbackComplete="EndCargaExcel" />
    </dx:ASPxCallback>

    <dx:ASPxCallback ID="cbpPagarFila" runat="server" ClientInstanceName="cbpPagarFila" ClientIDMode="Static" OnCallback="cbpPagarFila_Callback">
        <ClientSideEvents CallbackComplete="EndPagarFila" />
    </dx:ASPxCallback>

    <dx:ASPxCallback ID="cbpPagarMasivo" runat="server" ClientInstanceName="cbpPagarMasivo" ClientIDMode="Static" OnCallback="cbpPagarMasivo_Callback">
        <ClientSideEvents CallbackComplete="EndPagarMasivo" />
    </dx:ASPxCallback>

    <dx:ASPxCallback ID="cbpVolver" runat="server" ClientInstanceName="cbpVolver" ClientIDMode="Static" OnCallback="cbpVolver_Callback">
    </dx:ASPxCallback>

    <input type="hidden" id="HdfTabla" value="" runat="server" />
    <dx:ASPxHiddenField ID="HdfIndexPago" runat="server" ClientInstanceName="HdfIndexPago"></dx:ASPxHiddenField>

    <asp:SqlDataSource runat="server" ID="dsOrigenPago" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM CP_OrigenPago" SelectCommandType="Text"></asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsOrigenPagoTotalidad" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM CP_OrigenPago where habilitado = 1 and DescPago not in('Cliente', 'Subrogado')" SelectCommandType="Text"></asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="dsOrigenPagoFila" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM CP_OrigenPago where habilitado = 1 and DescPago not in('Siniestrado','Reestructurado', 'Prepago', 'Anulado')" SelectCommandType="Text"></asp:SqlDataSource>
</div>



