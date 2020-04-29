<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpCarteraComercial.ascx.cs" Inherits="MultiComercial.wpCarteraComeral.wpCarteraComercial.wpCarteraComercial" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css">
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

<script type="text/javascript">

    document.onkeydown = function () {
        if (txtEmpresa.GetText() != '' || cbxAcreedor.GetSelectedItem().text != '' || cbxEjecutivo.GetSelectedItem().text != '') {
            //GetBuscar();
        }
    }

    //evitar edicion formulario sharepoint
    function WebForm_OnSubmit() {
        return false;
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
            var tipoCartera = cbxCartera.GetSelectedItem().text;
            var acreedor = cbxAcreedor.GetSelectedItem() == null ? "" : cbxAcreedor.GetSelectedItem().text;
            var ejecutivo = cbxEjecutivo.GetSelectedItem() == null ? "" : cbxEjecutivo.GetSelectedItem().text;
            var empresa = txtEmpresa.GetText();
            var parametros = new Array(tipoCartera, acreedor, ejecutivo, empresa);
            window.gvCarteraEjecutivo.PerformCallback(parametros);
            return true;
        }
        catch (e) {
            alert(e);
        }
    }

    function clickAccion(s, e) {
        ocultarDiv();
        var keyValue = s.GetRowKey(e.visibleIndex);
        if (e.buttonID == 'gestion') {
            gvCarteraEjecutivo.GetRowValues(e.visibleIndex, 'NCertificado', OnGetRowValues);

            ASPxHiddenFielop.Set("value", keyValue.split("|")[2]);
            popup_Shown(keyValue.split("|")[2]);
            OpenDlg();
        }
        else if (e.buttonID == 'sms') {
            var keyValue = s.GetRowKey(e.visibleIndex);
            keyValue = keyValue.split("|")[0];
            gvCarteraEjecutivo.GetRowValues(e.visibleIndex, 'NCertificado', OnGetRowSms);
            popup_ShownSms(keyValue);
            OpenDlg();
        }
        else {
            OpenDlg();
            e.processOnServer = true;
        }

        return false;
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

    function popup_ShownSms(keyValue) {
        CpPcSms.PerformCallback(keyValue);
    }

    function popup_Shown(keyValue) {
        CpPcComentario.PerformCallback(keyValue);
    }

    function EndSms() {
        PcSMS.Show();
    }

    //function setGuardar() {
    //    if (window.ASPxClientEdit.ValidateGroup('crearGestion')) {
    //        ocultarDiv();
    //        OpenDlg();
    //        cpGuardar.PerformCallback('guardar');
    //    }
    //}

    function popupClose() {
        GetBuscar();
    }

    //function mostrarMensaje(e) {
    //    var value = e.result;
    //    if (value != null) {
    //        if (value == 'ok') {
    //            var el = document.getElementById('dvSuccess');
    //            el.style.display = "block";
    //            lbSuccess.SetText('Se ha ingresado el nuevo registro');
    //        }
    //        else {
    //            var dvw = document.getElementById('dvWarning');
    //            dvw.style.display = 'block';
    //            lbWarning.SetText('Error al ingresar el registro');
    //        }
    //    }
    //}

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

    function AgregarComentario(s, e) {
        window.gvBitacoraGestion.AddNewRow();
    }

    function OnInit(s, e) {
        ASPxClientUtils.AttachEventToElement(s.GetInputElement(), "click", function (event) {
            s.ShowDropDown();
        });
    }

    function EndGuardar(s, e) {
        CloseDlg();
    }

</script>

<dx:ASPxCallback ID="cpGuardar" ClientInstanceName="cpGuardar" runat="server" OnCallback="cpGuardar_Callback">
    <ClientSideEvents CallbackComplete="EndGuardar" />
</dx:ASPxCallback>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">
    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Comercial</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Cartera Ejecutivo</a>
                </li>
                <li class="active">Gesti&oacute;n Cartera Ejcutivo</li>
            </ol>
        </div>
    </div>


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

    <!-- filtros-->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <div class="col-md-3 col-sm-6">
                        <label for="">Visualización Cartera</label>
                        <dx:ASPxComboBox ID="cbxCartera" runat="server" ValueType="System.String" ClientInstanceName="cbxCartera" ClientIDMode="Static" CssClass="form-control" NullText="Seleccione">
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Acreedor</label>
                        <dx:ASPxComboBox ID="cbxAcreedor" runat="server" ValueType="System.String" ClientInstanceName="cbxAcreedor" ClientIDMode="Static" CssClass="form-control">
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Ejecutivo</label>
                        <dx:ASPxComboBox ID="cbxEjecutivo" runat="server" ValueType="System.String" ClientInstanceName="cbxEjecutivo" ClientIDMode="Static" CssClass="form-control">
                        </dx:ASPxComboBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Empresa</label>
                        <dx:ASPxTextBox ID="txtEmpresa" runat="server" ClientInstanceName="txtEmpresa" ClientIDMode="Static" CssClass="form-control" MaxLength="100"></dx:ASPxTextBox>
                    </div>
                    <div class="col-md-2 col-sm-6">
                        <label style="color: #FFF;">.</label><br>
                        <dx:ASPxButton ID="BtnBuscar" runat="server" Text="Buscar" AutoPostBack="false" ClientSideEvents-Click="GetBuscar" CssClass="btn btn-primary btn-success">
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <table border="0" style="table-layout: fixed; width: 100%;">
                <tr>

                    <td>
                        <div class="card-box">
                            <div class="table-responsive">
                                <dx:ASPxGridView ID="gvCarteraEjecutivo" runat="server" AutoGenerateColumns="true" Width="100%" ClientInstanceName="gvCarteraEjecutivo"
                                    ClientIDMode="Static" KeyFieldName="NCertificado;IdEmpresa;IdOperacion"
                                    OnPageIndexChanged="gvCarteraEjecutivo_PageIndexChanged"
                                    OnHtmlDataCellPrepared="gvCarteraEjecutivo_HtmlDataCellPrepared"
                                    OnCustomCallback="gvCarteraEjecutivo_CustomCallback"
                                    OnDataBound="gvCarteraEjecutivo_DataBound"
                                    OnCustomButtonCallback="gvCarteraEjecutivo_CustomButtonCallback"
                                    ClientSideEvents-EndCallback="EndCallback"
                                    ClientSideEvents-CustomButtonClick="clickAccion">
                                    <SettingsText EmptyDataRow="No registra datos"></SettingsText>
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="RazonSocial" Caption="Razon Social" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Ejecutivo" Caption="Ejecutivo" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Fondo" Caption="Fondo" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="NCertificado" Caption="N° Certificado" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Acreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="Dias Mora" Caption="Dias Mora" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Tramo" Caption="Tramo" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Capital Mora" Caption="Capital Mora" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Interes Mora" Caption="Interes Mora" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Monto En Mora" Caption="Monto En Mora" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Cuotas Mora" Caption="Cuotas Mora" HeaderStyle-Wrap="True" VisibleIndex="12" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Cuotas Pagadas" Caption="Cuotas Pagadas" HeaderStyle-Wrap="True" VisibleIndex="13" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Cuotas Pactadas" Caption="Cuotas Pactadas" HeaderStyle-Wrap="True" VisibleIndex="14" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Fecha Emisión" Caption="Fecha Emisión" HeaderStyle-Wrap="True" VisibleIndex="15" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="Fecha 1er Vencimiento" Caption="Fecha 1er Vencimiento" HeaderStyle-Wrap="True" VisibleIndex="16" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="Saldo Certificado" Caption="Saldo Certificado" HeaderStyle-Wrap="True" VisibleIndex="17" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="SaldoDeudaGlobal" Caption="Saldo Deuda Global" HeaderStyle-Wrap="True" VisibleIndex="18" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="Garantia VA" Caption="Garantia VA" HeaderStyle-Wrap="True" VisibleIndex="19" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Cob. Fogape" Caption="Cob. Fogape" HeaderStyle-Wrap="True" VisibleIndex="20" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Monto Fogape" Caption="Monto Fogape" HeaderStyle-Wrap="True" VisibleIndex="21" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="Ultimo Compromiso" Caption="Ultimo Compromiso" HeaderStyle-Wrap="True" VisibleIndex="22" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="Fec Ultima Gestión" Caption="Fec Ultima Gestión" HeaderStyle-Wrap="True" VisibleIndex="23" CellStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="IdEmpresa" Caption="IdEmpresa" HeaderStyle-Wrap="True" VisibleIndex="24" CellStyle-HorizontalAlign="Left" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="IdOperacion" Caption="IdOperacion" HeaderStyle-Wrap="True" VisibleIndex="25" CellStyle-HorizontalAlign="Left" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="DescArea" Caption="DescArea" HeaderStyle-Wrap="True" VisibleIndex="26" CellStyle-HorizontalAlign="Left" Visible="false">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewCommandColumn Caption="Acciones" Name="Acciones" CellStyle-HorizontalAlign="Center" VisibleIndex="27" HeaderStyle-HorizontalAlign="Center" ButtonType="Image">
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
                                                <dx:GridViewCommandColumnCustomButton ID="sms" Text="" Image-ToolTip="Sms">
                                                    <Image Url="../../_layouts/15/images/MultiGestion/email_remu.png" Height="25px" Width="25px">
                                                    </Image>
                                                    <Styles Style-CssClass="data-toggle">
                                                    </Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>

                                    </Columns>
                                    <SettingsLoadingPanel Mode="Disabled" />
                                    <SettingsBehavior AllowSort="true" AllowGroup="false" />
                                    <Styles>
                                        <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                    </Styles>
                                    <Settings ShowTitlePanel="true" />
                                    <SettingsText Title="Cartera" />
                                    <SettingsPager PageSize="20" />
                                </dx:ASPxGridView>
                            </div>

                        </div>

                    </td>
                </tr>
            </table>
        </div>
    </div>



    <%--<div class="row">
        <div class="col-md-12">
            <table border="0" style="table-layout: fixed; width: 100%;">
                <tr>

                    <td>
                        <div class="card-box">
                            <div class="table-responsive">
                                <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="true" Width="100%" ClientInstanceName="ASPxGridView1"
                                    ClientIDMode="Static" KeyFieldName="NCertificado;IdEmpresa;IdOperacion"
                                    OnDataBound="ASPxGridView1_DataBound"
                                    OnHtmlRowCreated="ASPxGridView1_HtmlRowCreated"  
                                    OnInit="ASPxGridView1_Init" >
                                   
                                </dx:ASPxGridView>
                            </div>

                        </div>

                    </td>
                </tr>
            </table>
        </div>
    </div>--%>
</div>

<%--Bitacora comentarios--%>
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
                                                                <dx:ASPxGridView ID="gvSms" runat="server" ClientInstanceName="gvSms" Width="95%"
                                                                    OnPageIndexChanged="gvSms_PageIndexChanged" ClientSideEvents-EndCallback="EndSms">
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

<dx:ASPxHiddenField ID="ASPxHiddenFielop" runat="server" ClientInstanceName="ASPxHiddenFielop"></dx:ASPxHiddenField>
<asp:SqlDataSource runat="server" ID="dsTipoAccion" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select ca.Id, ca.AccionCobranza from AreaAccion ac 
join CobranzaAccion ca on ca.Id = ac.IdAccion where ac.IdArea = 1 and ca.Habilitado = 1 order by ca.AccionCobranza asc" SelectCommandType="Text"></asp:SqlDataSource>


<%--SelectCommand="select Id, AccionCobranza from CobranzaAccion where Habilitado = 1 order by AccionCobranza asc--%>