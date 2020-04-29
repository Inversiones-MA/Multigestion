<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BitacoraCobranza.ascx.cs" Inherits="MultiRiesgo.BitacoraCobranza.BitacoraCobranza" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<script type="text/javascript">

    document.onkeydown = function () {
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
        }

        if (window.event && (window.event.keyCode == 13)) {
            valor = document.activeElement.value;
            if (valor == undefined) { return false; }
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
                    { return false; } //No realiza el backspace(largo igual a 0). 
                }
                if (txtEmpresa.GetText() != '') {
                    GetBuscar();
                }
            }
        }
    }

    function esNumero(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^[0-9.\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }


    function EndCallback(s, e) {
        try {
            CloseDlg();
            return true;
        }
        catch (e) {
            alert(e);
        }
    }

    function GetBuscar(s, e) {
        try {
            ocultarDiv();
            OpenDlg();
            var mora = cbxMora.GetSelectedItem().text;
            var acreedor = cbxAcreedor.GetSelectedItem() == null ? "" : cbxAcreedor.GetSelectedItem().text;
            var ejecutivo = cbxEjecutivo.GetSelectedItem() == null ? "" : cbxEjecutivo.GetSelectedItem().text;
            var empresa = txtEmpresa.GetText();
            var parametros = new Array(mora, acreedor, ejecutivo, empresa);
            window.gvBitacoraCobranza.PerformCallback(parametros);
            return true;
        }
        catch (e) {
            alert(e);
        }
    }

    function clikRedirecion(nombre, id) {
        var nombre = nombre;
        var id = id;
        var parametros = new Array(nombre, id);
        window.gvBitacoraCobranza.PerformCallback(parametros);
        return true;
    }

    function clickAccion(s, e) {
        ocultarDiv();
        var keyValue = s.GetRowKey(e.visibleIndex);
        hdfRees.Set("value", null);
        if (e.buttonID == 'gestion') {        
            //var boolValue = keyValue.split("|")[3].toLowerCase() == 'true' ? true : false;
            //if (boolValue)
            //    alert('Operacion reestructurada, no es posible realizar una nueva gestión');
            //else {
                gvBitacoraCobranza.GetRowValues(e.visibleIndex, 'NumCertificado', OnGetRowValues);

                ASPxHiddenFielop.Set("value", keyValue.split("|")[2]);
                popup_Shown(keyValue.split("|")[2]);
                OpenDlg();
            //}              
            //PcComentario.ShowAtElement();            
        }
        else if (e.buttonID == 'sms') {
            var keyValue = s.GetRowKey(e.visibleIndex);
            keyValue = keyValue.split("|")[0];
            gvBitacoraCobranza.GetRowValues(e.visibleIndex, 'NumCertificado', OnGetRowSms);
            popup_ShownSms(keyValue);
            OpenDlg();
        }
        else if (e.buttonID == 'reestructurar') {
            hdfRees.Set("value", "ok");
            var mensaje = new String;
            mensaje = mensaje.concat('¿Desea generar el proceso de Reestructuración para el certificado ', keyValue.split("|")[0], '?');
            e.processOnServer = confirm(mensaje);
        }
        else {
            OpenDlg();
            e.processOnServer = true;
        }

        return false;
    }

    function popup_ShownSms(keyValue) {
        CpPcSms.PerformCallback(keyValue);
    }

    function popup_Shown(keyValue) {
        CpPcComentario.PerformCallback(keyValue);
    }

    function EndSms() {
        PcSMS.Show();
    }

    function OnGetRowSms(value) {
        var NumCertificado = value;
        if (NumCertificado !== null)
            hfIdEmpresa.Set("value", NumCertificado);
    }

    function OnGetRowValues(value) {
        var numCertificado = value;
        if (numCertificado !== null)
            hfIndex.Set("value", numCertificado);
    }

    function setGuardar() {
        if (window.ASPxClientEdit.ValidateGroup('crearGestion')) {
            ocultarDiv();
            OpenDlg();
            cpGuardar.PerformCallback('guardar');
        }
    }

    function popupClose() {
        GetBuscar();
    }

    function EndGuardar(s, e) {
        CloseDlg();
        //mostrarMensaje(e);
    }

    function mostrarMensaje(e) {
        var value = e.result;
        if (value != null) {
            if (value == 'ok') {
                var el = document.getElementById('dvSuccess');
                el.style.display = "block";
                lbSuccess.SetText('Se ha ingresado el nuevo registro');
            }
            else {
                var dvw = document.getElementById('dvWarning');
                dvw.style.display = 'block';
                lbWarning.SetText('Error al ingresar el registro');
            }
        }
    }

    function ocultarDiv() {
        var dvs = document.getElementById('dvSuccess');
        dvs.style.display = 'none';
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'none';
        var dve = document.getElementById('dvError');
        dve.style.display = 'none';
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function EndPanel() {
        CloseDlg();
        PcComentario.Show();
    }

    function EndPanel2() {
        CloseDlg();
        PcSMS.Show();
    }

    function WebForm_OnSubmit() {
        return false;
    }

    function AgregarComentario(s, e) {
        window.gvBitacoraGestion.AddNewRow();
    }

    function OnInit(s, e) {
        ASPxClientUtils.AttachEventToElement(s.GetInputElement(), "click", function (event) {
            s.ShowDropDown();
        });
    }
</script>


<style type="text/css">
    /*.dxgvTitlePanel {display:none;} */
    /*.dxgvHeader a.dxgvCommandColumnItem,
.dxgvHeader a.dxgvCommandColumnItem:hover,
a.dxbButton, a.dxbButton:hover{color: #FF8800;text-decoration: underline;}
#grid{
    border: 1px Solid #c0c0c0;
    font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
    background-color: White;
    color: #555;
    cursor: default;
}*/
    .dxgvHeader {
        font-weight: bolder;
        background-color: #FFFFFF;
        border: 0;
        border-bottom: 1px #CCC solid;
        padding: 8px;
    }

    .dxeMemoEditAreaSys {
        padding: 3px 10px 10px 3px;
    }
</style>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">
    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Riesgo</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Bitacora Cobranza</a>
                </li>
                <li class="active">Gesti&oacute;n Mora</li>
            </ol>
        </div>
    </div>

    <%-- Cuadros de Alerta --%>
    <div class="row">
        <br />
        <div id="dvSuccess" style="display: none" class="alert alert-success">
            <h4>
                <dx:ASPxLabel ID="lbSuccess" runat="server" ClientInstanceName="lbSuccess" ClientIDMode="Static" CssClass="alert alert-success" Text=""></dx:ASPxLabel>
            </h4>
        </div>
        <div id="dvWarning" style="display: none" class="alert alert-warning" role="alert">
            <h4>
                <dx:ASPxLabel ID="lbWarning" runat="server" ClientInstanceName="lbWarning" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
            </h4>
        </div>
        <div id="dvError" style="display: none" class="alert alert-danger">
            <h4>
                <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
            </h4>
        </div>
        <br />
    </div>

    <dx:ASPxHiddenField ID="ASPxHiddenFielop" runat="server" ClientInstanceName="ASPxHiddenFielop"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="hdfRees" runat="server" ClientInstanceName="hdfRees"></dx:ASPxHiddenField>

    <!-- filtros-->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <div class="col-md-3 col-sm-6">
                        <label for="">Visualización de la mora</label>
                        <dx:ASPxComboBox ID="cbxMora" runat="server" ValueType="System.String" ClientInstanceName="cbxMora" ClientIDMode="Static" CssClass="form-control">
                            <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnMora(s); }" OnCallback="cbxMora_Callback" />--%>
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Acreedor</label>
                        <dx:ASPxComboBox ID="cbxAcreedor" runat="server" ValueType="System.String" ClientInstanceName="cbxAcreedor" ClientIDMode="Static" CssClass="form-control">
                            <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnAcreedor(s); }" OnCallback="cbxAcreedor_Callback" />--%>
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Ejecutivo</label>
                        <dx:ASPxComboBox ID="cbxEjecutivo" runat="server" ValueType="System.String" ClientInstanceName="cbxEjecutivo" ClientIDMode="Static" CssClass="form-control">
                            <%--OnCallback="cbxEjecutivo_Callback"--%>
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Empresa</label>
                        <dx:ASPxTextBox ID="txtEmpresa" runat="server" ClientInstanceName="txtEmpresa" ClientIDMode="Static" CssClass="form-control"></dx:ASPxTextBox>
                    </div>
                    <div class="col-md-2 col-sm-6">
                        <label style="color: #FFF;">.</label><br>
                        <dx:ASPxButton ID="BtnBuscar" runat="server" Text="Buscar" AutoPostBack="false" ClientSideEvents-Click="GetBuscar" CssClass="btn btn-primary btn-success"></dx:ASPxButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <dx:ASPxHiddenField ID="hfIndex" runat="server" ClientInstanceName="hfIndex"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="hfIdEmpresa" runat="server" ClientInstanceName="hfIdEmpresa"></dx:ASPxHiddenField>

    <dx:ASPxCallback ID="cpGuardar" ClientInstanceName="cpGuardar" runat="server" OnCallback="cpGuardar_Callback">
        <ClientSideEvents CallbackComplete="EndGuardar" />
    </dx:ASPxCallback>

    <div class="row">
        <div class="col-md-12">
            <table border="0" style="table-layout: fixed; width: 100%;">
                <tr>

                    <td>
                        <div class="card-box">
                            <div class="table-responsive">
                                <%--<div style="float: right; padding: 10px 5px;">
                                    <dx:ASPxButton ID="btnGuardarTodo" runat="server" Text="Guardar Comentarios" AutoPostBack="false" ClientSideEvents-Click="setGuardarTodo" CssClass="btn btn-primary btn-success"></dx:ASPxButton>
                                </div>--%>

                                <br />
                                <%--OnHtmlRowCreated="gvBitacoraCobranza_HtmlRowCreated"
                                    ;EsReestructuracion
                                    OnCustomButtonInitialize="gvBitacoraCobranza_CustomButtonInitialize"--%>
                                <dx:ASPxGridView ID="gvBitacoraCobranza" runat="server" AutoGenerateColumns="False" Width="100%" 
                                    ClientInstanceName="gvBitacoraCobranza" ClientIDMode="Static"
                                    KeyFieldName="NumCertificado;CantidadCuotasImpagas;IdOperacion"
                                    OnPageIndexChanged="gvBitacoraCobranza_PageIndexChanged"
                                    OnCustomCallback="gvBitacoraCobranza_CustomCallback"
                                    OnCustomButtonCallback="gvBitacoraCobranza_CustomButtonCallback" 
                                    OnHtmlDataCellPrepared="gvBitacoraCobranza_HtmlDataCellPrepared"
                                    OnHtmlRowPrepared="gvBitacoraCobranza_HtmlRowPrepared"
                                    OnCustomButtonInitialize="gvBitacoraCobranza_CustomButtonInitialize" 
                                    ClientSideEvents-EndCallback="EndCallback"
                                    ClientSideEvents-CustomButtonClick="clickAccion">
                                    <SettingsText EmptyDataRow="No registra datos"></SettingsText>
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="Ejecutivo" Caption="Ejecutivo" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="TipoPago" Caption="Tipo de Pago" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>                      
                                        <dx:GridViewDataColumn FieldName="RazonSocial" Caption="Empresa" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="NumCertificado" Caption="N° CF" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Fondo" Caption="Fondo" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="DescAcreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Tramo" Caption="Tramo" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <CellStyle Wrap="True" Font-Bold="true"></CellStyle>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="DiasMora" Caption="Dias de Mora" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center" SortIndex="0" SortOrder="Ascending">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="CuotasMora" Caption="Cuotas Mora" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Cuotas Pagadas" Caption="Cuotas Pagadas" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Cuotas Pactadas" Caption="Cuotas Pactadas" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="MontoEnMora" Caption="Monto Mora" HeaderStyle-Wrap="True" VisibleIndex="12" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <%--<PropertiesTextEdit DisplayFormatString="N0" />--%>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="SaldoCertificado" Caption="Saldo Certificado" HeaderStyle-Wrap="True" VisibleIndex="13" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <%--<PropertiesTextEdit DisplayFormatString="N0" />--%>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="SaldoDeudaGlobal" Caption="Saldo Deuda Global" HeaderStyle-Wrap="True" VisibleIndex="14" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="GarantiaVA" Caption="Garantía Valor Ajustado" HeaderStyle-Wrap="True" VisibleIndex="15" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="CobFOGAPE" Caption="Cobertura Fogape" HeaderStyle-Wrap="True" VisibleIndex="16" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                         <dx:GridViewDataColumn FieldName="MontoFogape" Caption="Monto Fogape" HeaderStyle-Wrap="True" VisibleIndex="17" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataTextColumn FieldName="UltimoCompromiso" Caption="Ultimo Compromiso" HeaderStyle-Wrap="True" VisibleIndex="18" CellStyle-HorizontalAlign="Center" Width="10%">
                                            <DataItemTemplate>
                                                <dx:ASPxMemo runat="server" ID="txtUltimoMensaje" ClientIDMode="Static" Rows="3" MaxLength="250" Width="100%" ClientInstanceName="txtUltimoMensaje"
                                                    Columns="16" Text='<%# Eval("UltimoCompromiso") %>' Border-BorderWidth="2px">
                                                </dx:ASPxMemo>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataColumn FieldName="UltimafecGestion" Caption="Fecha Gestión" HeaderStyle-Wrap="True" VisibleIndex="19" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <%--<PropertiesTextEdit DisplayFormatString="N0" />--%>
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="fec1erVencimiento" Caption="Fecha 1er Vencimiento" HeaderStyle-Wrap="True" VisibleIndex="20" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <%--<PropertiesTextEdit DisplayFormatString="N0" />--%>
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="fecEmision" Caption="Fecha Emisión" HeaderStyle-Wrap="True" VisibleIndex="21" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <%--<PropertiesTextEdit DisplayFormatString="N0" />--%>
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="EsReestructuracion" HeaderStyle-Wrap="True" VisibleIndex="22" CellStyle-HorizontalAlign="Center" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                                       
                                        <dx:GridViewCommandColumn Caption="Acciones" Name="Acciones" CellStyle-HorizontalAlign="Center" VisibleIndex="23" HeaderStyle-HorizontalAlign="Center" ButtonType="Image">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="gestion" Text="" Image-ToolTip="Gestión">
                                                    <Image Url="../../_layouts/15/images/MultiGestion/Personalize.png" Height="20px" Width="20px">
                                                    </Image>
                                                    <Styles Style-CssClass="data-toggle data-placement">
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="calendario" Text="" Image-ToolTip="Calendario">
                                                    <Image Url="../../_layouts/15/images/MultiGestion/CALENDAR.GIF" Height="20px" Width="20px">
                                                    </Image>
                                                    <Styles Style-CssClass="data-toggle data-placement data-original-title">
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="detalle" Text="" Image-ToolTip="Detalle">
                                                    <Image Url="../../_layouts/15/images/MultiGestion/DETAIL.GIF" Height="20px" Width="20px">
                                                    </Image>
                                                    <Styles Style-CssClass="data-toggle">
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="sms" Text="" Image-ToolTip="Sms">
                                                    <Image Url="../../_layouts/15/images/MultiGestion/email_remu.png" Height="25px" Width="25px">
                                                    </Image>
                                                    <Styles Style-CssClass="data-toggle">
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                         <%--    <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="reestructurar" Text="" Image-ToolTip="Reestructurar">
                                                    <Image Url="../../_layouts/15/images/MultiGestion/AlertWarning.png" Height="25px" Width="25px">
                                                    </Image>
                                                    <Styles Style-CssClass="data-toggle">
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>--%>
                                        </dx:GridViewCommandColumn>

                                        <dx:GridViewDataColumn FieldName="IdEmpresa" HeaderStyle-Wrap="True" VisibleIndex="22" CellStyle-HorizontalAlign="Center" Visible="false">
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="IdOperacion" HeaderStyle-Wrap="True" VisibleIndex="23" CellStyle-HorizontalAlign="Center" Visible="false">
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="DescArea" HeaderStyle-Wrap="True" VisibleIndex="24" CellStyle-HorizontalAlign="Center" Visible="false">
                                        </dx:GridViewDataColumn>
                                    </Columns>
                                    <SettingsLoadingPanel Mode="Disabled" />
                                    <SettingsBehavior AllowSort="true" AllowGroup="false" />
                                    <Styles>
                                        <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                    </Styles>
                                    <Settings ShowTitlePanel="true" />
                                    <SettingsText Title="Bitacora Cobranza" />
                                    <SettingsPager PageSize="20" />
                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <%--PopUp comentario ShowOnPageLoad="true"--%>
    <dx:ASPxPopupControl ID="PcComentario" runat="server" ClientInstanceName="PcComentario" CloseAction="CloseButton"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Ingrese nueva gestion" HeaderStyle-Font-Bold="true"
        AllowDragging="True"
        PopupAnimationType="None" EnableViewState="False"
        Width="700px" Height="400px">
        <HeaderStyle Font-Bold="True" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1"
                runat="server">
                <dx:ASPxCallbackPanel ID="CpPcComentario" runat="server" Width="700px" ClientInstanceName="CpPcComentario" OnCallback="CpPcComentario_Callback" ClientSideEvents-EndCallback="EndPanel">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">

                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <div style="float: right; padding: 10px 5px;">
                                                            <dx:ASPxButton ID="BtnNewComentario" runat="server" Text="Nueva Gestion" AutoPostBack="false" ClientSideEvents-Click="AgregarComentario" CssClass="btn btn-primary btn-success" ClientIDMode="Static"></dx:ASPxButton>
                                                        </div>
                                                        <br />
                                                        <dx:ASPxGridView ID="gvBitacoraGestion" runat="server" ClientInstanceName="gvBitacoraGestion" KeyFieldName="IdCobranza" ClientIDMode="Static" Width="100%"
                                                            OnRowInserting="gvBitacoraGestion_RowInserting" OnPageIndexChanged="gvBitacoraGestion_PageIndexChanged" 
                                                            OnCellEditorInitialize="gvBitacoraGestion_CellEditorInitialize">
                                                            <Columns>
                                                                <dx:GridViewDataColumn FieldName="IdOperacion" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center" Visible="false">
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="RazonSocial" Caption="Razon Social" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center" Width="20%">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                                    <EditFormSettings Visible="False" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="NroCertificado" Caption="Nro Certificado" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center" Width="10%">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                                    <EditFormSettings Visible="False" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataComboBoxColumn FieldName="IdAccionCobranza" Caption="Acción Multiaval" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                                    <PropertiesComboBox ValueType="System.Int32" TextField="AccionCobranza" ValueField="Id" DataSourceID="dsTipoAccion" NullText="Seleccione">
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesComboBox>
                                                                    <EditFormSettings ColumnSpan="2" />
                                                                </dx:GridViewDataComboBoxColumn>
                                                                <dx:GridViewDataDateColumn FieldName="FechaGestion" Caption="Fecha Gestión" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                                                    PropertiesDateEdit-EditFormatString="dd-MM-yyyy" Width="30%" PropertiesDateEdit-ClientSideEvents-Init="OnInit">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesDateEdit>
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesDateEdit>
                                                                    <EditFormSettings ColumnSpan="2" />
                                                                </dx:GridViewDataDateColumn>
                                                                <dx:GridViewDataTextColumn FieldName="Comentario" Caption="Comentario" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Width="30%">
                                                                    <EditItemTemplate>
                                                                        <dx:ASPxMemo runat="server" ID="txtUltimoMensaje" ClientIDMode="Static" Rows="3" MaxLength="250" ClientInstanceName="txtUltimoMensaje"
                                                                            Columns="72" Text='<%# Eval("Comentario") %>' Border-BorderWidth="2px">
                                                                        </dx:ASPxMemo>
                                                                    </EditItemTemplate>
                                                                    <PropertiesTextEdit>
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesTextEdit>
                                                                    <EditFormSettings ColumnSpan="2" />
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataColumn FieldName="Usuario" Caption="Usuario" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center" Width="10%">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                                    <EditFormSettings Visible="False" />
                                                                </dx:GridViewDataColumn>
                                                            </Columns>
                                                            <SettingsBehavior AllowSort="true" AllowGroup="false" />
                                                            <Styles>
                                                                <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                                            </Styles>
                                                            <Settings ShowTitlePanel="true" />
                                                            <SettingsText Title="Comentarios" />
                                                            <SettingsPager PageSize="5" />
                                                            <SettingsCommandButton>
                                                                <UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success pull-right" Styles-Style-Width="10%"></UpdateButton>
                                                                <CancelButton Text="Cancelar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-warning pull-right" Styles-Style-Width="10%"></CancelButton>
                                                            </SettingsCommandButton>
                                                            <SettingsEditing EditFormColumnCount="2" NewItemRowPosition="Top" />
                                                        </dx:ASPxGridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents Closing="popupClose" />
    </dx:ASPxPopupControl>

    <%--PopUp sms--%>
    <dx:ASPxPopupControl ID="PcSMS" runat="server" ClientInstanceName="PcSMS" CloseAction="CloseButton"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Detalle Notificaciones" HeaderStyle-Font-Bold="true"
        AllowDragging="True"
        PopupAnimationType="None" EnableViewState="False"
        Width="600px" Height="400px">
        <HeaderStyle Font-Bold="True" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2"
                runat="server">
                <dx:ASPxCallbackPanel ID="CpPcSms" runat="server" Width="600px" ClientInstanceName="CpPcSms" OnCallback="CpPcSms_Callback" ClientSideEvents-EndCallback="EndPanel2">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <div class="row">
                                <div class="col-md-12">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <div class="form-horizontal" role="form">
                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                    <label>SMS Enviados</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                    <dx:ASPxGridView ID="gvSms" runat="server" ClientInstanceName="gvSms" Width="95%" OnPageIndexChanged="gvSms_PageIndexChanged" ClientSideEvents-EndCallback="EndSms">
                                                                        <SettingsPager PageSize="10" />
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <asp:SqlDataSource runat="server" ID="dsTipoAccion" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="	select ca.Id, ca.AccionCobranza from AreaAccion ac 
join CobranzaAccion ca on ca.Id = ac.IdAccion where ac.IdArea = 2 and ca.Habilitado = 1 order by ca.AccionCobranza asc" SelectCommandType="Text"></asp:SqlDataSource>
</div>
