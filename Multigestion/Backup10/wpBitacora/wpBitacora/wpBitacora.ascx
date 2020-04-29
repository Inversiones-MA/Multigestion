<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpBitacora.ascx.cs" Inherits="MultiContabilidad.wpBitacora.wpBitacora.wpBitacora" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<script src="../../_layouts/15/MultiOperacion/jquery-1.11.1.js"></script>

<script type="text/javascript">
    document.onkeydown = function () {
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event && (window.event.keyCode == 13)) {
            valor = document.activeElement.value;
            if (valor == undefined) { return false; } //Evita Enter en página. 
            else {
                if (txtCertificado.GetText() != '' || txtRazonSocial.GetText() != '') {
                    filtrarGrid();
                }

                //if (document.activeElement.getAttribute('type') == 'select-one')
                //{ return false; } //Evita Enter en select. 
                //if (document.activeElement.getAttribute('type') == 'button')
                //{ return false; } //Evita Enter en button. 
                //if (document.activeElement.getAttribute('type') == 'radio')
                //{ return false; } //Evita Enter en radio. 
                //if (document.activeElement.getAttribute('type') == 'checkbox')
                //{ return false; } //Evita Enter en checkbox. 
                //if (document.activeElement.getAttribute('type') == 'file')
                //{ return false; } //Evita Enter en file. 
                //if (document.activeElement.getAttribute('type') == 'reset')
                //{ return false; } //Evita Enter en reset. 
                //if (document.activeElement.getAttribute('type') == 'submit')
                //{ return false; } //Evita Enter en submit. 
                //if (document.activeElement.getAttribute('type') == 'input')
                //{ return false; } //Evita Back en input. 
                //else //Text, textarea o password 
                //{
                //    if (document.activeElement.value.length == 0)
                //    { return false; } //No realiza el backspace(largo igual a 0). 
                //    else {
                //        if (txtCertificado.GetText() != '' || txtRazonSocial.GetText() != '') {
                //            filtrarGrid();
                //        }
                //    } 
                //}
            }
        }
    }

    //$(document).ready(function () {
    //    //$("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
    //    filtrarGrid();
    //});

    function AgregarMov(s, e) {
        window.gvBitacora.AddNewRow();
    }

    function OnBtnBuscar(s, e) {
        filtrarGrid();
    }

    function filtrarGrid() {
        var filtro = 'buscar';
        var certificado = txtCertificado.GetText();
        var empresa = txtRazonSocial.GetText();
        var parametros = new Array(filtro, certificado, empresa);

        if (parametros != null) {
            window.gvBitacora.PerformCallback(parametros);
        }
        return false;
    }

    function OnBtnExcel(s, e) {
        DescargarExcel();
    }

    function DescargarExcel() {
        var filtro = 'excel';
        var certificado = txtCertificado.GetText();
        var empresa = txtRazonSocial.GetText();
        var parametros = new Array(filtro, certificado, empresa);

        if (parametros != null) {
            window.gvBitacora.PerformCallback(parametros);
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

    function EndGvBitacora(s, e) {
        //setTimeout(function () { CloseDlg(); }, 6000);
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function AgregarCargaMasiva() {
        btnBorrarCarga.SetVisible(false);
        btnSubirMasivo.SetVisible(false);
        btnCargarMasivamente.SetVisible(true);
        //gvPreCargaMasivo.Refresh();
        pcCargarMasivo.Show();
    }

    function EndgvPreCargaMasivo() {
        if (window.gvPreCargaMasivo.GetVisibleRowsOnPage() == 0) {
            btnSubirMasivo.SetVisible(false);
            btnBorrarCarga.SetVisible(false);
        }
        if (window.gvPreCargaMasivo.GetVisibleRowsOnPage() > 0) {
            btnCargarMasivamente.SetVisible(false);
        }
    }

    function OnFileUploadComplete(s, e) {
        try {
            var valores = e.callbackData.split(';');
            if (e.isValid && valores[0] == "OK") {
                gvPreCargaMasivo.PerformCallback("nuevo");
                btnSubirMasivo.SetVisible(true);
                btnBorrarCarga.SetVisible(true);
            } else {
                alert(valores[0]);
            }
        } catch (e) {
            alert(e);
        }
    }

    function SubirExcel(s, e) {
        if (window.ASPxClientEdit.ValidateGroup('ValidarExcel') && UpSubirMasivo.GetText() != '') {
            UpSubirMasivo.Upload();
        }
        else
            alert('Ingrese informacion');
    }

    function BorrarMasivo() {
        gvPreCargaMasivo.Refresh();
        gvPreCargaMasivo.PerformCallback("limpiar");
        btnCargarMasivamente.SetVisible(true);
    }

    var i = null;
    function EndCargaCC(s, e) {
        var valores = e.result.split(';');
        //alert('lalala');
        if (valores[0] == 'OK') {
            gvPreCargaMasivo.Refresh();
            pcCargarMasivo.Hide();
            alert(valores[1]);
            filtrarGrid();
        }
        else {
            alert(valores[0]);
        }
        i = null;
        CloseDlg();
    }

    function Subir(s, e) {
        if(i == null)
            ASPxCallback1.PerformCallback("InsertarCuenta");

        i = 0;
    }

    function WebForm_OnSubmit() {
        return false;
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
            <h4 class="page-title">Contabilidad</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Contabilidad</a>
                </li>
                <li class="active">Bitacora seguimiento
                </li>
            </ol>
        </div>
    </div>


    <!-- tabs -->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <!-- col 1/4 -->
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="">N° Certificado:</label>
                            <dx:ASPxTextBox ID="txtCertificado" runat="server" ClientInstanceName="txtCertificado" CssClass="form-control" MaxLength="10" Width="100%" ClientIDMode="Static"></dx:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label for="">Razon Social:</label>
                            <dx:ASPxTextBox ID="txtRazonSocial" runat="server" ClientInstanceName="txtRazonSocial" CssClass="form-control" MaxLength="100" Width="100%" ClientIDMode="Static"></dx:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group" style="margin-top: 20px">
                            <dx:ASPxButton ID="btnBuscar" runat="server" ClientInstanceName="btnBuscar" Text="Buscar" CssClass="btn btn-primary btn-success" AutoPostBack="false">
                                <ClientSideEvents Click="OnBtnBuscar" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group" style="margin-top: 20px">
                            <dx:ASPxButton ID="btnExcel" runat="server" ClientInstanceName="btnExcel" Text="Exportar a Excel" CssClass="btn btn-primary btn-success" AutoPostBack="false">
                                <ClientSideEvents Click="OnBtnExcel" />
                            </dx:ASPxButton>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>


    <!-- tabla / grilla -->
    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <br />
                <br />

                <dx:ASPxButton ID="BtnCargaMasiva" ClientInstanceName="BtnCargaMasiva" runat="server" Text="Carga Masiva" RenderMode="Button"
                    AutoPostBack="false" CssClass="btn btn-primary btn-success" Width="10%">
                    <ClientSideEvents Click="AgregarCargaMasiva" />
                </dx:ASPxButton>

                <br />
                <br />

                <table border="0" style="table-layout: fixed; width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gvBitacora" runat="server" ClientInstanceName="gvBitacora" Width="100%" KeyFieldName="Id" ClientIDMode="Static"
                                OnPageIndexChanged="gvBitacora_PageIndexChanged"
                                OnRowDeleting="gvBitacora_RowDeleting"
                                OnRowUpdating="gvBitacora_RowUpdating"
                                OnRowInserting="gvBitacora_RowInserting"
                                OnCustomCallback="gvBitacora_CustomCallback"
                                OnCellEditorInitialize="gvBitacora_CellEditorInitialize"
                                OnDataBound="gvBitacora_DataBound"
                                ClientSideEvents-EndCallback="EndGvBitacora">
                                <SettingsBehavior ConfirmDelete="true" />
                                <SettingsText ConfirmDelete="¿Esta seguro de borrar este registro?" />
                                <Columns>
                                    <dx:GridViewCommandColumn VisibleIndex="0" Width="5%" ShowEditButton="true" ShowDeleteButton="true">
                                        <HeaderTemplate>
                                            <dx:ASPxButton ID="BtnNew" ClientInstanceName="BtnNew" runat="server" Text="Nuevo Movimiento" RenderMode="Button"
                                                AutoPostBack="false" CssClass="btn btn-block btn-warning btn-xs" Width="10%">
                                                <ClientSideEvents Click="AgregarMov" />
                                            </dx:ASPxButton>
                                        </HeaderTemplate>
                                    </dx:GridViewCommandColumn>

                                    <dx:GridViewDataTextColumn FieldName="NCF" Caption="N° Certificado" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center" SortIndex="0" SortOrder="Ascending"
                                        ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="10" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn FieldName="RazonSocial" Caption="Razon Social" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center"
                                        ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="10" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <EditFormSettings Visible="False" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataComboBoxColumn FieldName="IdMotivo" Caption="Motivo" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="DescMotivo"
                                            ValueField="IdMotivoMov" DataSourceID="dsMotivo">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>

                                    <dx:GridViewDataComboBoxColumn FieldName="IdCausa" Caption="Causa" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="DescCausa"
                                            ValueField="IdCausa" DataSourceID="dsCausa">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>

                                    <dx:GridViewDataComboBoxColumn FieldName="IdBanco" Caption="Banco" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="Nombre"
                                            ValueField="IdAcreedor" DataSourceID="dsAcreedor">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>

                                    <dx:GridViewDataDateColumn FieldName="FechaCobro" Caption="Fecha Cobro" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                        PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesDateEdit>
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>

                                    <dx:GridViewDataDateColumn FieldName="FechaPago" Caption="Fecha Pago" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                        PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesDateEdit>
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>

                                    <dx:GridViewDataTextColumn FieldName="NroDocumento" Caption="N° Documento" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center"
                                        ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="20" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn FieldName="Monto" Caption="Monto" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center"
                                        ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true"
                                        PropertiesTextEdit-DisplayFormatInEditMode="true"
                                        PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesTextEdit DisplayFormatString="${0:n0}">
                                        </PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn FieldName="Cuota" Caption="Cuota" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center"
                                        ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="10" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataComboBoxColumn FieldName="IdAcreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="Nombre"
                                            ValueField="IdAcreedor" DataSourceID="dsAcreedor">
                                            <ValidationSettings>
                                                <RequiredField IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>

                                    <dx:GridViewDataTextColumn FieldName="Comentario" Caption="Comentario" HeaderStyle-Wrap="True" VisibleIndex="12" CellStyle-HorizontalAlign="Center" Width="15%">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        <DataItemTemplate>
                                            <dx:ASPxMemo runat="server" ID="txtComentario" ClientIDMode="Static" Rows="3" MaxLength="250" Width="100%" ClientInstanceName="txtComentario"
                                                Columns="18" Text='<%# Eval("Comentario") %>' Border-BorderWidth="2px">
                                            </dx:ASPxMemo>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                </Columns>

                                <SettingsBehavior AllowSort="true" />
                                <SettingsPager PageSize="30"></SettingsPager>
                                <SettingsCommandButton>
                                    <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                    <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                    <UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success pull-right" Styles-Style-Width="10%"></UpdateButton>
                                    <CancelButton Text="Cancelar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-warning pull-right" Styles-Style-Width="10%"></CancelButton>
                                </SettingsCommandButton>
                                <SettingsEditing EditFormColumnCount="4" NewItemRowPosition="Top" />

                            </dx:ASPxGridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <dx:ASPxPopupControl ID="pcCargarMasivo" runat="server" ClientInstanceName="pcCargarMasivo" CloseAction="CloseButton" ClientIDMode="Static"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            HeaderText="Carga masiva"
            AllowDragging="True"
            PopupAnimationType="None"
            Width="1000px" Height="700px">
            <HeaderStyle Font-Bold="True" />
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1">
                    <div class="row">

                        <div class="col-md-12">
                            <div class="col-md-4">
                                <dx:ASPxUploadControl ID="UpSubirMasivo" ShowUploadButton="false" runat="server" ClientInstanceName="UpSubirMasivo" UploadMode="Standard"
                                    ShowProgressPanel="false" Theme="MetropolisBlue" FileUploadMode="OnPageLoad" ClientIDMode="Static" EncodeHtml="true"
                                    OnFileUploadComplete="UpSubirMasivo_FileUploadComplete" CssClass="form-control">
                                    <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".csv" MaxFileSizeErrorText="El tamaño del archivo excede el límite permitido"
                                        GeneralErrorText="Error al subir archivo"
                                        NotAllowedFileExtensionErrorText="Extension no permitida">
                                    </ValidationSettings>
                                    <ClientSideEvents
                                        FileUploadComplete="OnFileUploadComplete" />
                                </dx:ASPxUploadControl>
                            </div>
                            <div class="col-md-4">
                                <dx:ASPxButton ID="btnCargarMasivamente" runat="server" ClientInstanceName="btnCargarMasivamente" Text="Vista Previa" AutoPostBack="false" ValidationGroup="ValidarExcel" ClientIDMode="Static"
                                    CssClass="btn btn-primary btn-success" ClientSideEvents-Click="SubirExcel">
                                </dx:ASPxButton>
                            </div>
                        </div>

                    </div>

                    <br />
                    <br />

                    <div class="card-box">
                        <table border="0" style="table-layout: fixed; width: 100%;">
                            <tr>
                                <td>
                                    <dx:ASPxGridView ID="gvPreCargaMasivo" runat="server" ClientInstanceName="gvPreCargaMasivo" OnPageIndexChanged="gvPreCargaMasivo_PageIndexChanged" Width="100%"
                                        OnCustomCallback="gvPreCargaMasivo_CustomCallback" ClientSideEvents-EndCallback="EndgvPreCargaMasivo" PreviewEncodeHtml="true" EditFormLayoutProperties-EncodeHtml="true"
                                        AutoGenerateColumns="false" KeyFieldName="Id" ClientIDMode="Static">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="NCertificado" Caption="N° Certificado" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataColumn FieldName="Motivo" Caption="Motivo" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataColumn FieldName="Causa" Caption="Causa" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataTextColumn FieldName="FechaCobro" Caption="Fecha cobro" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center"
                                                ShowInCustomizationForm="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="FechaPago" Caption="Fecha Pago" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="Banco" Caption="Banco" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit EncodeHtml="false" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="NDocumento" Caption="N° Documento" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="Monto" Caption="Monto" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <%--<PropertiesTextEdit DisplayFormatString="${0:n0}">
                                            </PropertiesTextEdit>--%>
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataColumn FieldName="Cuota" Caption="Cuota" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataTextColumn FieldName="Acreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit EncodeHtml="false" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="Comentario" Caption="Comentario" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <EditItemTemplate>
                                                    <dx:ASPxMemo runat="server" ID="txtComentario" ClientIDMode="Static" Rows="3" MaxLength="250" Width="100%" ClientInstanceName="txtComentario"
                                                        Columns="18" Border-BorderWidth="2px">
                                                    </dx:ASPxMemo>
                                                </EditItemTemplate>
                                            </dx:GridViewDataTextColumn>

                                        </Columns>

                                        <SettingsBehavior AllowSort="false" />
                                        <SettingsPager PageSize="10"></SettingsPager>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <br />
                    <br />

                    <div class="col-md-6">
                        <dx:ASPxButton ID="btnBorrarCarga" runat="server" ClientInstanceName="btnBorrarCarga" Text="Limpiar Carga" AutoPostBack="false" ClientIDMode="Static"
                            CssClass="btn btn-warning">
                            <ClientSideEvents Click="BorrarMasivo" />
                        </dx:ASPxButton>
                    </div>
                    <div class="col-md-6">
                        <dx:ASPxButton ID="btnSubirMasivo" runat="server" ClientInstanceName="btnSubirMasivo" Text="Ingresar Carga Masiva" AutoPostBack="false" ClientIDMode="Static"
                            CssClass="btn btn-primary btn-success">
                            <%--<ClientSideEvents Click="SubirCargaMasiva" />--%>
                            <ClientSideEvents Click="Subir" />
                        </dx:ASPxButton>
                    </div>

                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>

    <asp:SqlDataSource runat="server" ID="dsMotivo" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM Motivo where habilitado = 1" SelectCommandType="Text"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsCausa" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM Causa where habilitado = 1" SelectCommandType="Text"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsAcreedor" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdAcreedor, Nombre from Acreedores" SelectCommandType="Text"></asp:SqlDataSource>


    <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="ASPxCallback1" OnCallback="ASPxCallback1_Callback" ClientIDMode="Static">
        <ClientSideEvents CallbackComplete="EndCargaCC" />
    </dx:ASPxCallback>

</div>
