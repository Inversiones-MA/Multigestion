<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpBitacora.ascx.cs" Inherits="MultiContabilidad.wpBitacora.wpBitacora.wpBitacora" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript">

    function AgregarCC(s, e) {
        window.GvCuentaCorriente.AddNewRow();
    }

    function EndGvCuentaCorriente(s, e) {
        if (s.cpPanelMensaje != null) {
            alert(s.cpPanelMensaje);
            CloseDlg();
        }
        else {
            setTimeout(function () { CloseDlg(); }, 5000);
        }
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function OnBtnBuscarCuentaCorriente(s, e) {
        filtrarGrid();
    }

    function filtrarGrid() {
        var filtro = 'buscar';
        var certificado = TxtCertificado.GetText();
        var empresa = TxtRazonSocial.GetText();
        var parametros = new Array(filtro, certificado, empresa);

        if (parametros != null) {
            window.GvCuentaCorriente.PerformCallback(parametros);
        }
        return true;
    }

    function AgregarCargaMasiva() {
        BtnBorrarCargaCC.SetVisible(false);
        BtnSubirMasivoCC.SetVisible(false);
        BtnCargaCuentaCorriente.SetVisible(true);
        pcCargarMasivo.Show();
    }

    function OnBtnExcel(s, e) {
        var filtro = 'excel';
        var certificado = TxtCertificado.GetText();
        var empresa = TxtRazonSocial.GetText();
        var parametros = new Array(filtro, certificado, empresa);

        if (parametros != null) {
            OpenDlg();
            window.GvCuentaCorriente.PerformCallback(parametros);
        }
        //DescargarExcel();
    }


    function EndGvPreCargaMasivo(s, e) {
        if (s.cpPanelMensaje == "nuevoLimpio") {
            s.cpPanelMensaje = null;
            if (window.GvPreCargaMasivo.GetVisibleRowsOnPage() == 0) {
                BtnSubirMasivoCC.SetVisible(false);
                BtnBorrarCargaCC.SetVisible(false);
            }
            if (window.GvPreCargaMasivo.GetVisibleRowsOnPage() > 0) {
                ////BtnCargaCuentaCorriente.SetVisible(false);
                BtnSubirMasivoCC.SetVisible(true);
                BtnSubirMasivoCC.SetEnabled(true);
                BtnBorrarCargaCC.SetVisible(true);
            }
        }
        else {
            if (s.cpPanelMensaje != null) {
                var valores = s.cpPanelMensaje.split(';');
                s.cpPanelMensaje = null;
                if (valores[0] == 'OK') {
                    GvPreCargaMasivo.Refresh();
                    pcCargarMasivo.Hide();
                    alert(valores[1]);
                    filtrarGrid();
                    BtnSubirMasivoCC.SetEnabled(true);
                }
                else {
                    alert(valores[0]);
                }
            }
        }
        CloseDlg();
    }

    function OnFileUploadCuentaCorriente(s, e) {
        try {
            var valores = e.callbackData.split(';');
            if (e.isValid && valores[0] == "OK") {
                GvPreCargaMasivo.PerformCallback("nuevo");
            } else {
                alert(valores[0]);
            }
        } catch (e) {
            alert(e);
        }
    }

    function SubirExcelCC(s, e) {
        if (window.ASPxClientEdit.ValidateGroup('ValidarExcel') && UpSubirCC.GetText() != '') {
            UpSubirCC.Upload();
        }
        else {
            alert('Ingrese informacion');
        }
    }

    function BorrarMasivo() {
        ////alert('aaas');
        ////GvPreCargaMasivo.Refresh();
        GvPreCargaMasivo.PerformCallback("limpiar");
        BtnCargaCuentaCorriente.SetVisible(true);
    }

    function SubirCC(s, e) {
        BtnSubirMasivoCC.SetEnabled(false);
        GvPreCargaMasivo.PerformCallback("Insertar");
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

    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <!-- col 1/4 -->
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>N° Certificado:</label>
                            <dx:ASPxTextBox ID="TxtCertificado" runat="server" ClientInstanceName="TxtCertificado"
                                CssClass="form-control" MaxLength="10" Width="100%" ClientIDMode="Static">
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Razon Social:</label>
                            <dx:ASPxTextBox ID="TxtRazonSocial" runat="server" ClientInstanceName="TxtRazonSocial"
                                CssClass="form-control" MaxLength="100" Width="100%" ClientIDMode="Static">
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group" style="margin-top: 20px">
                            <dx:ASPxButton ID="BtnBuscarCuentaCorriente" runat="server" Text="Buscar" ClientInstanceName="BtnBuscarCuentaCorriente"
                                CssClass="btn btn-primary btn-success" AutoPostBack="false" ClientIDMode="Static">
                                <ClientSideEvents Click="OnBtnBuscarCuentaCorriente" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group" style="margin-top: 20px">
                            <dx:ASPxButton ID="BtnExcel" runat="server" ClientInstanceName="BtnExcel" Text="Exportar a Excel" CssClass="btn btn-primary btn-success" AutoPostBack="false" ClientIDMode="Static">
                                <ClientSideEvents Click="OnBtnExcel" />
                            </dx:ASPxButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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
                            <div class="table-responsive">
                                <dx:ASPxGridView ID="GvCuentaCorriente" runat="server" ClientInstanceName="GvCuentaCorriente" Width="95%"
                                    KeyFieldName="Id" ClientIDMode="Static" CssClass="table table-responsive table-hover table-bordered"
                                    AutoGenerateColumns="False"
                                    OnPageIndexChanged="GvCuentaCorriente_PageIndexChanged"
                                    OnCustomCallback="GvCuentaCorriente_CustomCallback"
                                    OnRowDeleting="GvCuentaCorriente_RowDeleting"
                                    OnRowUpdating="GvCuentaCorriente_RowUpdating"
                                    OnRowInserting="GvCuentaCorriente_RowInserting"
                                    OnCellEditorInitialize="GvCuentaCorriente_CellEditorInitialize"
                                    OnDataBound="GvCuentaCorriente_DataBound"
                                    OnCustomErrorText="GvCuentaCorriente_CustomErrorText"
                                    ClientSideEvents-EndCallback="EndGvCuentaCorriente">

                                    <SettingsBehavior ConfirmDelete="true" />
                                    <SettingsText ConfirmDelete="¿Esta seguro de borrar este registro?" />
                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ShowEditButton="true" ShowDeleteButton="true">
                                            <HeaderTemplate>
                                                <dx:ASPxButton ID="BtnNew" ClientInstanceName="BtnNew" runat="server" Text="Nuevo" RenderMode="Button"
                                                    AutoPostBack="false" CssClass="btn btn-block btn-warning btn-xs">
                                                    <ClientSideEvents Click="AgregarCC" />
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

                                        <dx:GridViewDataComboBoxColumn FieldName="IdConcepto" Caption="Concepto" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="Concepto"
                                                ValueField="IdConcepto" DataSourceID="dsConcepto">
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataComboBoxColumn FieldName="IdBanco" Caption="Banco" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="Nombre"
                                                ValueField="IdAcreedor" DataSourceID="dsAcreedor">
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataDateColumn FieldName="FechaCobro" Caption="Fecha Cobro" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                            PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <PropertiesDateEdit>
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataDateColumn FieldName="FechaPago" Caption="Fecha Pago" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                            PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <PropertiesDateEdit>
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn FieldName="NroDocumento" Caption="N° Documento" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center"
                                            ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="20" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn FieldName="Monto" Caption="Monto" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center"
                                            ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true"
                                            PropertiesTextEdit-DisplayFormatInEditMode="true"
                                            PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <PropertiesTextEdit DisplayFormatString="${0:n0}">
                                            </PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn FieldName="Cuota" Caption="Cuota" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center"
                                            ShowInCustomizationForm="true" PropertiesTextEdit-MaxLength="10" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataComboBoxColumn FieldName="IdAcreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="12" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="Nombre"
                                                ValueField="IdAcreedor" DataSourceID="dsAcreedor">
                                                <ValidationSettings>
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                        </dx:GridViewDataComboBoxColumn>

                                        <dx:GridViewDataTextColumn FieldName="Comentario" Caption="Comentario" HeaderStyle-Wrap="True" VisibleIndex="13" CellStyle-HorizontalAlign="Center" Width="15%">
                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            <DataItemTemplate>
                                                <dx:ASPxMemo runat="server" ID="txtComentario" ClientIDMode="Static" Rows="3" MaxLength="250" Width="100%" ClientInstanceName="txtComentario"
                                                    Columns="18" Text='<%# Eval("Comentario") %>' Border-BorderWidth="2px">
                                                </dx:ASPxMemo>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>

                                    </Columns>

                                    <SettingsBehavior AllowSort="true" AllowGroup="false" />
                                    <SettingsPager PageSize="30"></SettingsPager>

                                    <SettingsCommandButton>
                                        <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                        <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                        <UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success pull-right" Styles-Style-Width="10%"></UpdateButton>
                                        <CancelButton Text="Cancelar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-warning pull-right" Styles-Style-Width="10%"></CancelButton>
                                    </SettingsCommandButton>
                                    <SettingsEditing EditFormColumnCount="4" NewItemRowPosition="Top" />

                                </dx:ASPxGridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>


    <div>
        <dx:ASPxPopupControl ID="pcCargarMasivo" runat="server" ClientInstanceName="pcCargarMasivo"
            CloseAction="CloseButton" ClientIDMode="Static" PopupElementID="PopupControlContentControl1"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            HeaderText="Carga masiva"
            AllowDragging="True"
            PopupAnimationType="None" MinHeight="300px" MaxHeight="600px"
            MinWidth="900px" MaxWidth="1100px" ClientSideEvents-CloseUp="BorrarMasivo">            
            <SettingsLoadingPanel Enabled="true" Text="..." Delay="0" />
            <HeaderStyle Font-Bold="True" />
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-4">
                                <dx:ASPxUploadControl ID="UpSubirCC" ShowUploadButton="false" runat="server"
                                    ClientInstanceName="UpSubirCC" UploadMode="Standard"
                                    ShowProgressPanel="false" Theme="MetropolisBlue" FileUploadMode="OnPageLoad"
                                    ClientIDMode="Static" EncodeHtml="true"                                    
                                    OnFileUploadComplete="UpSubirCC_FileUploadComplete" CssClass="form-control">
                                    <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".csv" 
                                        MaxFileSizeErrorText="El tamaño del archivo excede el límite permitido"
                                        GeneralErrorText="Error al subir archivo"
                                        NotAllowedFileExtensionErrorText="Extension no permitida">
                                    </ValidationSettings>
                                   <ClientSideEvents
                                        FileUploadComplete="OnFileUploadCuentaCorriente" />
                                </dx:ASPxUploadControl>
                            </div>
                            <div class="col-md-4">
                                <dx:ASPxButton ID="BtnCargaCuentaCorriente" runat="server" ClientInstanceName="BtnCargaCuentaCorriente"
                                    Text="Vista Previa" AutoPostBack="false" ValidationGroup="ValidarExcel" ClientIDMode="Static"
                                    CssClass="btn btn-primary btn-success" ClientSideEvents-Click="SubirExcelCC">
                                </dx:ASPxButton>
                            </div>
                        </div>

                    </div>

                    <br />
                    
                    <div class="card-box" style="overflow-y: scroll; overflow-x:scroll">
                        <table border="0" style="table-layout: fixed; width: 100%;">
                            <tr>
                                <td>
                                    <dx:ASPxGridView ID="GvPreCargaMasivo" runat="server" ClientInstanceName="GvPreCargaMasivo"
                                        OnPageIndexChanged="GvPreCargaMasivo_PageIndexChanged" Width="100%"
                                        OnCustomCallback="GvPreCargaMasivo_CustomCallback"
                                        ClientSideEvents-EndCallback="EndGvPreCargaMasivo"
                                        PreviewEncodeHtml="true" EditFormLayoutProperties-EncodeHtml="true"
                                        AutoGenerateColumns="false" KeyFieldName="Id" ClientIDMode="Static">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="ncertificado" Caption="N° Certificado" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataColumn FieldName="motivo" Caption="Motivo" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataColumn FieldName="causa" Caption="Causa" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataTextColumn FieldName="fechacobro" Caption="Fecha cobro" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center"
                                                ShowInCustomizationForm="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="fechapago" Caption="Fecha Pago" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="banco" Caption="Banco" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit EncodeHtml="false" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="ndocumento" Caption="N° Documento" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="monto" Caption="Monto" HeaderStyle-Wrap="True" VisibleIndex="8" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <%--<PropertiesTextEdit DisplayFormatString="${0:n0}">
                                            </PropertiesTextEdit>--%>
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataColumn FieldName="cuota" Caption="Cuota" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataColumn>

                                            <dx:GridViewDataTextColumn FieldName="acreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit EncodeHtml="false" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="concepto" Caption="Concepto" HeaderStyle-Wrap="True" VisibleIndex="11" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit EncodeHtml="false" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="comentario" Caption="Comentario" HeaderStyle-Wrap="True" VisibleIndex="12" CellStyle-HorizontalAlign="Center">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <EditItemTemplate>
                                                    <dx:ASPxMemo runat="server" ID="txtComentario" ClientIDMode="Static" Rows="3" MaxLength="250" Width="100%" ClientInstanceName="txtComentario"
                                                        Columns="18" Border-BorderWidth="2px">
                                                    </dx:ASPxMemo>
                                                </EditItemTemplate>
                                            </dx:GridViewDataTextColumn>

                                        </Columns>

                                        <SettingsBehavior AllowSort="false" />
                                        <SettingsPager PageSize="8"></SettingsPager>
                                        <SettingsLoadingPanel ShowImage="true" Delay="0" Text="..." />
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <br />
                    
                    <div class="col-md-6">
                        <dx:ASPxButton ID="BtnBorrarCargaCC" runat="server" ClientInstanceName="BtnBorrarCargaCC" Text="Limpiar Carga"
                            AutoPostBack="false" ClientIDMode="Static"
                            CssClass="btn btn-warning">
                            <ClientSideEvents Click="BorrarMasivo" />
                        </dx:ASPxButton>
                    </div>
                    <div class="col-md-6">
                        <dx:ASPxButton ID="BtnSubirMasivoCC" runat="server" ClientInstanceName="BtnSubirMasivoCC" Text="Ingresar Carga Masiva"
                            AutoPostBack="false" ClientIDMode="Static"
                            CssClass="btn btn-primary btn-success">
                            <ClientSideEvents Click="SubirCC" />
                        </dx:ASPxButton>
                    </div>

                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
</div>

<asp:SqlDataSource runat="server" ID="dsMotivo" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM Motivo where habilitado = 1" SelectCommandType="Text"></asp:SqlDataSource>

<asp:SqlDataSource runat="server" ID="dsCausa" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM Causa where habilitado = 1" SelectCommandType="Text"></asp:SqlDataSource>

<asp:SqlDataSource runat="server" ID="dsConcepto" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdConcepto, Concepto from cc_concepto where habilitado = 1" SelectCommandType="Text"></asp:SqlDataSource>

<asp:SqlDataSource runat="server" ID="dsAcreedor" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdAcreedor, Nombre from Acreedores" SelectCommandType="Text"></asp:SqlDataSource>

