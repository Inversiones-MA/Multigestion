<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPafFactoring.ascx.cs" Inherits="MultiFactoring.wpPaf_Factoring.wpPaf_Factoring.wpPafFactoring" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css">
    body {
    }

    .dxgvTitlePanel {
        display: none;
    }

    .dxgvHeader a.dxgvCommandColumnItem,
    .dxgvHeader a.dxgvCommandColumnItem:hover,
    a.dxbButton, a.dxbButton:hover {
        color: #FF8800;
        text-decoration: underline;
    }

    #grid {
        border: 1px Solid #c0c0c0;
        font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
        background-color: White;
        color: #555;
        cursor: default;
    }

    .dxgvHeader {
        font-weight: bolder;
        background-color: #FFFFFF;
        border: 0;
        border-bottom: 1px #CCC solid;
        padding: 8px;
    }

    .btnInline {
        display: inline-table;
    }
</style>



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
                    { return false; } //No realiza el backspace(largo igual a 0). 
                }
                if (txtEmpresa.GetText() !== '') {
                    filtrarGrid();
                }
            }
        }
    }

    function WebForm_OnSubmit() {
        return false;
    }

    function OcultarDivPaf() {
        var el = document.getElementById('divPaf');
        el.style.display = "none";
    }

    function filtrarGrid() {
        if (window.ASPxClientEdit.ValidateGroup('buscarEmpresa')) {
            OcultarDivPaf();
            //var acreedor = ddlAcreedor.GetSelectedItem().value;
            var empresa = txtEmpresa.GetText();
            var parametros = new Array('buscar', empresa);

            if (empresa != null) {
                window.gvEmpresas.PerformCallback(parametros);
            }
        }
        else
            alert('Ingrese empresa')
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function EndgvEmpresas() {
        CloseDlg();
    }

    function OnDetalle(s, e) {
        var rowVisibleIndex = e.visibleIndex;
        //alert(e.buttonID);
        var rowKeyValue = s.GetRowKey(rowVisibleIndex);

        if (e.buttonID == 'CrearPaf') {
            OcultarDivPaf();
            OpenDlg();
            gvEmpresas.GetRowValues(e.visibleIndex, 'RazonSocial;RutEmpresa;DescEjecutivo', CargarOficina);
            var key = 'CargarPaf';
            HfEmp.Set("value", rowKeyValue);
            var empresa = rowKeyValue;
            var parametros = new Array(key, empresa);
            cpnlPaf.PerformCallback(parametros); 
        }
    }

    function endcallbackpanel(s, e) {
        var el = document.getElementById('divPaf');
        el.style.display = "block";
        CloseDlg();
    }

    function CargarOficina(values) {
        var valores = values;
        if (valores !== null) {
            //lblEjecutivo.SetText(valores[0]);
            //lblEjecutivo.SetText(valores[1]);
            lblEjecutivo.SetText(valores[2]);
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

    function esDecimal(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^[0-9,\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

    function formatoPesos(s, e) {
        var monto = encodeHtml(s.GetText());
        if (monto != '') {
            monto = parseFloat(monto.replace(/\./g, '').replace(/\,/g, '.'))
            monto = formatNumber.new(parseFloat(monto).toFixed(0));
            s.SetText(monto);
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

    function OnClick(s, e) {
        gvOperacionesNuevas.AddNewRow();
    }

    function EndCallbackPaf(s, e) {
        alert('ok');
    }

    //function EndgvOperacionesVigentes(s, e) {
    //}

    function EndgvOperacionesNuevas(s, e) {

    }

    //function EndgvGarantias(s, e) {

    //}


    function OnBtnLimpiar() {
        OpenDlg();
        var key = 'ResetPaf';
        var empresa = null;
        var parametros = new Array(key, empresa);
        cpnlPaf.PerformCallback(parametros);
    }

</script>

<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>


<div id="dvFormulario" runat="server">
    <dx:ASPxHiddenField ID="HfEmp" runat="server" ClientInstanceName="HfEmp" ClientIDMode="Static"></dx:ASPxHiddenField>

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">PAF FACTORING</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">PAF FACTORING</a>
                </li>
                <li class="active">PAF FACTORING
                </li>
            </ol>
        </div>
    </div>


    <div class="card-box">
        <div class="row">

            <div class="col-md-6">
                <div class="form-group">
                    <label class="col-sm-5 control-label">Cliente</label>
                    <div class="col-sm-7">
                        <dx:ASPxTextBox ID="txtEmpresa" runat="server" ClientInstanceName="txtEmpresa" MaxLength="200" CssClass="form-control" ValidationSettings-ValidationGroup="buscarEmpresa" Width="97%">
                            <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                <RequiredField IsRequired="true" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-2">
                <div class="form-group">
                    <label class="col-sm-5 control-label"></label>
                    <div class="col-sm-7">

                        <dx:ASPxButton ID="btnBuscarEmpresa" runat="server" ClientInstanceName="btnBuscarEmpresa" Text="Buscar Empresa" CssClass="btn btn-success pull-right" AutoPostBack="false" ValidationGroup="buscarEmpresa">
                            <ClientSideEvents Click="filtrarGrid" />
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
                                <dx:ASPxGridView ID="gvEmpresas" runat="server" ClientInstanceName="gvEmpresas" CssClass="table table-bordered table-hover" Width="100%" ClientIDMode="Static"
                                    AutoGenerateColumns="false" KeyFieldName="IdEmpresa"
                                    OnCustomCallback="gvEmpresas_CustomCallback"
                                    ClientSideEvents-EndCallback="EndgvEmpresas">
                                    <ClientSideEvents CustomButtonClick="OnDetalle" />
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="IdEmpresa" HeaderStyle-Wrap="True" VisibleIndex="0" Visible="false">
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="RazonSocial" Caption="Razon Social" HeaderStyle-Wrap="True" VisibleIndex="1">
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="RutEmpresa" Caption="Rut Empresa" HeaderStyle-Wrap="True" VisibleIndex="2">
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="DescEjecutivo" Caption="Ejecutivo" HeaderStyle-Wrap="True" VisibleIndex="3">
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewCommandColumn Caption="Crear PAF" ButtonType="Image" VisibleIndex="4" Name="Crear PAF"
                                            CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="CrearPaf">
                                                    <Image ToolTip="Descargar plantilla" Url="../../_layouts/15/images/MultiAdministracion/Download.png"></Image>
                                                    <Styles Style-CssClass="fa fa-edit paddingIconos"></Styles>
                                                </dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>

                                    </Columns>
                                    <Settings ShowTitlePanel="true" />
                                    <SettingsText Title="" />
                                    <SettingsPager PageSize="30" />

                                </dx:ASPxGridView>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div class="row" id="divPaf" style="display: none">
        <div class="col-md-12">
            <div class="card-box">

                <dx:ASPxCallbackPanel ID="cpnlPaf" runat="server" Width="100%" ClientInstanceName="cpnlPaf" Height="100%" OnCallback="cpnlPaf_Callback" ClientSideEvents-EndCallback="endcallbackpanel" SettingsLoadingPanel-ShowImage="false" SettingsLoadingPanel-Enabled="false">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" SupportsDisabledAttribute="True">

                            <div class="col-md-12 col-sm-12" style="text-align: center">
                                <h3>PROPUESTA DE AFIANZAMIENTO</h3>
                                <h4>Ficha N°
                                    <dx:ASPxLabel ID="lblPaf" runat="server" ClientInstanceName="lblPaf"></dx:ASPxLabel>
                                </h4>
                            </div>

                            <div class="col-md-12">
                                <div class="col-md-3 col-sm-6">
                                    <table class="table table-bordered" style="font-size: small;">
                                        <tr>
                                            <td class="auto-style11"><b>Oficina:</b></td>
                                            <td class="auto-style13">1</td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style11"><b>Fecha:</b></td>
                                            <td class="auto-style13">
                                                <dx:ASPxDateEdit ID="ctrFecha" ClientInstanceName="ctrFecha" runat="server" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" EditFormat="Date" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" Width="98%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                        <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                        <RequiredField IsRequired="true" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style11"><b>Fe.Revisión:</b></td>
                                            <td class="auto-style13">
                                                <dx:ASPxDateEdit ID="ctrFechaRevision" ClientInstanceName="ctrFechaRevision" runat="server" CssClass="form-control" ValidationSettings-ValidationGroup="ValidarCotizador" EditFormat="Date" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" Width="98%">
                                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                                        <ErrorFrameStyle ForeColor="Red"></ErrorFrameStyle>
                                                        <RequiredField IsRequired="true" />
                                                    </ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style11"><b>Ejecutivo:</b></td>
                                            <td class="auto-style13">
                                                <dx:ASPxLabel ID="lblEjecutivo" runat="server" ClientInstanceName="lblEjecutivo" CssClass="form-control"></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="col-md-3 col-sm-6">
                                    <table class="table table-bordered" style="font-size: small;">
                                        <tr>
                                            <td colspan="2"><b>Estado Línea</b></td>
                                            <td class="auto-style17" rowspan="3"></td>
                                            <td colspan="2"><b>Nivel Atribución</b></td>
                                        </tr>
                                        <tr>
                                            <td>Normal</td>
                                            <td>
                                                <dx:ASPxRadioButton ID="rbNormal" runat="server" ClientInstanceName="rbNormal" GroupName="estadolinea"></dx:ASPxRadioButton>
                                                <%--<asp:RadioButton ID="rbNormal" runat="server" GroupName="estadolinea" />--%>
                                            </td>
                                            <td>C.Riesgo</td>
                                            <td>
                                                <dx:ASPxRadioButton ID="rbRiesgo" runat="server" ClientInstanceName="rbRiesgo" GroupName="nivelatribucion"></dx:ASPxRadioButton>
                                                <%--<asp:RadioButton ID="rbRiesgo" runat="server" GroupName="nivelatribucion" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Rev.Espe.</td>
                                            <td>
                                                <dx:ASPxRadioButton ID="rbRevision" runat="server" ClientInstanceName="rbRevision" GroupName="estadolinea"></dx:ASPxRadioButton>
                                                <%-- <asp:RadioButton ID="rbRevision" runat="server" GroupName="estadolinea" /></td>--%>
                                            <td>C.Ejec.</td>
                                            <td>
                                                <dx:ASPxRadioButton ID="rbEjecucion" runat="server" ClientInstanceName="rbEjecucion" GroupName="nivelatribucion"></dx:ASPxRadioButton>
                                                <%--<asp:RadioButton ID="rbEjecucion" runat="server" GroupName="nivelatribucion" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="1"><b>Canal</b></td>
                                            <td colspan="4"><b>Multiaval Servicios Financieros</b></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="col-md-3 col-sm-6">
                                    <table class="table table-bordered" style="font-size: small;">
                                        <tr>
                                            <td colspan="2"><b></b></td>
                                            <td class="auto-style17" rowspan="2"></td>
                                            <td colspan="2"><b>Nivel Atribución</b></td>
                                        </tr>
                                        <tr>
                                            <td>Rev.Espe.</td>
                                            <td>

                                            <td>C.Ejec.</td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="col-md-3 col-sm-6">
                                    <table class="table table-bordered" style="font-size: small;">
                                        <tr>
                                            <td colspan="2"><b>Clasificación Riesgo</b></td>
                                        </tr>
                                        <tr>
                                            <td><b>Ventas Anuales UF</b></td>
                                            <td>
                                                <dx:ASPxTextBox ID="txtVentas" runat="server" Width="170px" ClientInstanceName="txtVentas" CssClass="form-control" MaxLength="15" ValidationSettings-ValidationGroup="ValidarCotizador">
                                                    <ClientSideEvents LostFocus="formatoPesos" />
                                                    <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />

                            <div class="col-md-8 col-sm-8">
                                <h4>Identificación del Cliente</h4>
                                <table class="table table-bordered" style="font-size: small;">
                                    <tr>
                                        <td><b>Razón Social:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblNombre" runat="server" Text="" ClientInstanceName="lblNombre" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                        <td></td>
                                        <td><b>RUT:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblRut" runat="server" Text="" ClientInstanceName="lblRut" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Dirección:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblDireccion" runat="server" Text="" ClientInstanceName="lblDireccion" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                        <td></td>
                                        <td><b>Región:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblRegion" runat="server" Text="" ClientInstanceName="lblRegion" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Comuna:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblComuna" runat="server" Text="" ClientInstanceName="lblComuna" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                        <td></td>
                                        <td><b>Ciudad:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblCiudad" runat="server" Text="" ClientInstanceName="lblCiudad" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Actividad:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblGiro" runat="server" Text="" ClientInstanceName="lblGiro" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                        <td></td>
                                        <td><b>Telefono:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblTelefono" runat="server" Text="" ClientInstanceName="lblTelefono" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div class="col-md-4 col-sm-4">
                                <h4>Datos Financieros</h4>
                                <table class="table table-bordered" style="font-size: small;">
                                    <tr>
                                        <td><b>Valor Dolar:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblDolar" runat="server" Text="" ClientInstanceName="txtDolar" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Valor UF:</b></td>
                                        <td>
                                            <dx:ASPxLabel ID="lblUf" runat="server" Text="" ClientInstanceName="TxtUf" CssClass="form-control"></dx:ASPxLabel>
                                        </td>
                                    </tr>
                                </table>
                            </div>


                            <div class="row">
                                <div class="col-md-12">
                                    <h4>Operaciones Vigentes</h4>
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <%--OnCustomCallback="gvOperacionesVigentes_CustomCallback"
                                                        ClientSideEvents-EndCallback="EndgvOperacionesVigentes"
                                                        OnSummaryDisplayText="gvOperacionesVigentes_SummaryDisplayText"--%>
                                                        <dx:ASPxGridView ID="gvOperacionesVigentes" runat="server" ClientInstanceName="gvOperacionesVigentes" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                            AutoGenerateColumns="false" KeyFieldName="IdOperacion" OnCustomColumnDisplayText="gvOperacionesVigentes_CustomColumnDisplayText">
                                                            <Columns>
                                                                <dx:GridViewDataColumn FieldName="N" Caption="N°" HeaderStyle-Wrap="True" VisibleIndex="0">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Tipo Financiamiento" Caption="Tipo Financiamiento" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Producto" Caption="Producto" HeaderStyle-Wrap="True" VisibleIndex="2">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Finalidad" Caption="Finalidad" HeaderStyle-Wrap="True" VisibleIndex="3">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Fondo" Caption="Fondo" HeaderStyle-Wrap="True" VisibleIndex="4">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="plazo" Caption="Plazo(meses)" HeaderStyle-Wrap="True" VisibleIndex="5">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Horizonte" Caption="Horizonte(meses)" HeaderStyle-Wrap="True" VisibleIndex="6">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="coberturaCertificado" Caption="Cobertura Certificado" HeaderStyle-Wrap="True" VisibleIndex="7">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Comisión Min. %" Caption="Tasa Comisión" HeaderStyle-Wrap="True" VisibleIndex="8">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Comisión" Caption="Comisión(CLP)" HeaderStyle-Wrap="True" VisibleIndex="9">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Tipo Moneda" Caption="Tipo Moneda" HeaderStyle-Wrap="True" VisibleIndex="10">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">Total CLP:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">Total USD:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">Total UF:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right;">Total Riesgo UF:</p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Monto Aprobado" Caption="Monto Aprobado" HeaderStyle-Wrap="True" VisibleIndex="11">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <%--<PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>--%>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalCLPMA"
                                                                                ClientInstanceName="lblOpeVigentesTotalCLPMA" Text="<%# GetOpeVigentesTotalCLPMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalUSDMA"
                                                                                ClientInstanceName="lblOpeVigentesTotalUSDMA" Text="<%# GetOpeVigentesTotalUSDMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalUFMA"
                                                                                ClientInstanceName="lblOpeVigentesTotalUFMA" Text="<%# GetOpeVigentesTotalUFMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalRiesgoUFMA"
                                                                                ClientInstanceName="lblOpeVigentesTotalRiesgoUFMA" Text="<%# GetOpeVigentesTotalRiesgoUFMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Monto Vigente" Caption="Monto Vigente" HeaderStyle-Wrap="True" VisibleIndex="12">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <%--<PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>--%>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalCLPMV"
                                                                                ClientInstanceName="lblOpeVigentesTotalCLPMV" Text="<%# GetOpeVigentesTotalCLPMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalUSDMV"
                                                                                ClientInstanceName="lblOpeVigentesTotalUSDMV" Text="<%# GetOpeVigentesTotalUSDMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalUFMV"
                                                                                ClientInstanceName="lblOpeVigentesTotalUFMV" Text="<%# GetOpeVigentesTotalUFMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalRiesgoUFMV"
                                                                                ClientInstanceName="lblOpeVigentesTotalRiesgoUFMV" Text="<%# GetOpeVigentesTotalRiesgoUFMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Monto Propuesto" Caption="Monto Propuesto" HeaderStyle-Wrap="True" VisibleIndex="13">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <%--<PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>--%>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalCLPMP"
                                                                                ClientInstanceName="lblOpeVigentesTotalCLPMP" Text="<%# GetOpeVigentesTotalCLPMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalUSDMP"
                                                                                ClientInstanceName="lblOpeVigentesTotalUSDMP" Text="<%# GetOpeVigentesTotalUSDMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalUFMP"
                                                                                ClientInstanceName="lblOpeVigentesTotalUFMP" Text="<%# GetOpeVigentesTotalUFMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeVigentesTotalRiesgoUFMP"
                                                                                ClientInstanceName="lblOpeVigentesTotalRiesgoUFMP" Text="<%# GetOpeVigentesTotalRiesgoUFMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                            </Columns>
                                                            <TotalSummary>
                                                                <dx:ASPxSummaryItem FieldName="Monto Aprobado" SummaryType="Sum" Tag="MontoAprobado" />
                                                                <dx:ASPxSummaryItem FieldName="Monto Vigente" SummaryType="Sum" Tag="MontoVigente" />
                                                                <dx:ASPxSummaryItem FieldName="Monto Propuesto" SummaryType="Sum" Tag="MontoPropuesto" />
                                                            </TotalSummary>
                                                            <Settings ShowTitlePanel="true" />
                                                            <SettingsText Title="" />
                                                            <SettingsPager PageSize="30" />
                                                            <Settings ShowFooter="True" />
                                                        </dx:ASPxGridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />


                            <div class="row">
                                <div class="col-md-12">
                                    <h4>Operaciones Nuevas</h4>
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <dx:ASPxGridView ID="gvOperacionesNuevas" runat="server" ClientInstanceName="gvOperacionesNuevas" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                            AutoGenerateColumns="false" KeyFieldName="IdOperacion"
                                                            OnCustomCallback="gvOperacionesNuevas_CustomCallback"
                                                            OnRowUpdating="gvOperacionesNuevas_RowUpdating"
                                                            OnRowInserting="gvOperacionesNuevas_RowInserting"
                                                            OnRowDeleting="gvOperacionesNuevas_RowDeleting"
                                                            OnSummaryDisplayText="gvOperacionesNuevas_SummaryDisplayText"
                                                            ClientSideEvents-EndCallback="EndgvOperacionesNuevas">

                                                            <SettingsBehavior ConfirmDelete="true" />
                                                            <SettingsText ConfirmDelete="¿Desea borrar el registro?" />
                                                            <Columns>
                                                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" Width="5%" VisibleIndex="0" ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true">
                                                                    <HeaderTemplate>
                                                                        <dx:ASPxButton ID="BtnNuevo" ClientInstanceName="BtnNuevo" runat="server" Text="Nuevo" RenderMode="Button" AutoPostBack="false" CssClass="btn btn-primary btn-success pull-right" Width="10%">
                                                                            <ClientSideEvents Click="OnClick" />
                                                                        </dx:ASPxButton>
                                                                    </HeaderTemplate>
                                                                </dx:GridViewCommandColumn>

                                                                <dx:GridViewDataColumn FieldName="IdEmpresa" HeaderStyle-Wrap="True" VisibleIndex="1" Visible="false">
                                                                </dx:GridViewDataColumn>

                                                                <dx:GridViewDataComboBoxColumn FieldName="IdTipoFinanciamiento" Caption="Tipo Financiamiento" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesComboBox ValueType="System.Int32" TextField="NombreProducto" ValueField="IdProducto" DataSourceID="dsProducto" NullText="Seleccione">
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesComboBox>
                                                                </dx:GridViewDataComboBoxColumn>

                                                                <dx:GridViewDataComboBoxColumn FieldName="IdProducto" Caption="Producto" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesComboBox ValueType="System.Int32" TextField="NombreTipoProducto" ValueField="IdTipoProducto" DataSourceID="dsProducto" NullText="Seleccione">
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesComboBox>
                                                                </dx:GridViewDataComboBoxColumn>

                                                                <dx:GridViewDataComboBoxColumn FieldName="IdFinalidad" Caption="Finalidad" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesComboBox ValueType="System.Int32" TextField="Nombre" ValueField="IdFinalidad" DataSourceID="dsFinalidad" NullText="Seleccione">
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesComboBox>
                                                                </dx:GridViewDataComboBoxColumn>

                                                                <dx:GridViewDataTextColumn FieldName="Plazo" Caption="Plazo(meses)" HeaderStyle-Wrap="True" VisibleIndex="5" PropertiesTextEdit-MaxLength="3"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumero(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn FieldName="AnticipoMax" Caption="Anticipo Máximo por Factura" HeaderStyle-Wrap="True" VisibleIndex="6" PropertiesTextEdit-MaxLength="3"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn FieldName="PlazoMax" Caption="Plazo Máx Factura" HeaderStyle-Wrap="True" VisibleIndex="7" PropertiesTextEdit-MaxLength="3"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn FieldName="ConcentracionMax" Caption="Concentrac. Max por Deudor (%)" HeaderStyle-Wrap="True" VisibleIndex="8" PropertiesTextEdit-MaxLength="3"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn FieldName="TasaComision" Caption="Tasa + Comisión (%) " HeaderStyle-Wrap="True" VisibleIndex="9" PropertiesTextEdit-MaxLength="3"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataComboBoxColumn FieldName="IdTipoMoneda" Caption="Tipo Moneda" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesComboBox ValueType="System.Int32" TextField="Abreviacion" ValueField="IdMoneda" DataSourceID="dsMoneda" NullText="Seleccione">
                                                                        <ValidationSettings>
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </PropertiesComboBox>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">Total CLP:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">Total USD:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">Total UF:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right;">Total Riesgo UF:</p>
                                                                        <%--<span style="font-size: 6px;"><%# GetSummaryText(Container)%></span>
                                                            <span><%# GetSummaryValue(Container)%></span>--%>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataComboBoxColumn>

                                                                <dx:GridViewDataTextColumn FieldName="MontoAprobado" Caption="Monto Aprobado" HeaderStyle-Wrap="True" VisibleIndex="11" PropertiesTextEdit-MaxLength="12"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="c0"></PropertiesTextEdit>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalCLPMA"
                                                                                ClientInstanceName="lblOpeNuevasTotalCLPMA" Text="<%# GetOpeNuevasTotalCLPMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalUSDMA"
                                                                                ClientInstanceName="lblOpeNuevasTotalUSDMA" Text="<%# GetOpeNuevasTotalUSDMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalUFMA"
                                                                                ClientInstanceName="lblOpeNuevasTotalUFMA" Text="<%# GetOpeNuevasTotalUFMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalRiesgoUFMA"
                                                                                ClientInstanceName="lblOpeNuevasTotalRiesgoUFMA" Text="<%# GetOpeNuevasTotalRiesgoUFMA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <%--<span style="font-size: 6px;"><%# GetSummaryText(Container)%></span>
                                                            <span><%# GetSummaryValue(Container)%></span>--%>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn FieldName="MontoVigente" Caption="Monto Vigente" HeaderStyle-Wrap="True" VisibleIndex="12" PropertiesTextEdit-MaxLength="12"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="c0"></PropertiesTextEdit>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalCLPMV"
                                                                                ClientInstanceName="lblOpeNuevasTotalCLPMV" Text="<%# GetOpeNuevasTotalCLPMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalUSDMV"
                                                                                ClientInstanceName="lblOpeNuevasTotalUSDMV" Text="<%# GetOpeNuevasTotalUSDMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalUFMV"
                                                                                ClientInstanceName="lblOpeNuevasTotalUFMV" Text="<%# GetOpeNuevasTotalUFMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalRiesgoUFMV"
                                                                                ClientInstanceName="lblOpeNuevasTotalRiesgoUFMV" Text="<%# GetOpeNuevasTotalRiesgoUFMV(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataTextColumn>

                                                                <dx:GridViewDataTextColumn FieldName="MontoPropuesto" Caption="Monto Propuesto" HeaderStyle-Wrap="True" VisibleIndex="13" PropertiesTextEdit-MaxLength="12"
                                                                    PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esDecimal(s,e);}" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="c0"></PropertiesTextEdit>
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalCLPMP"
                                                                                ClientInstanceName="lblOpeNuevasTotalCLPMP" Text="<%# GetOpeNuevasTotalCLPMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalUSDMP"
                                                                                ClientInstanceName="lblOpeNuevasTotalUSDMP" Text="<%# GetOpeNuevasTotalUSDMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalUFMP"
                                                                                ClientInstanceName="lblOpeNuevasTotalUFMP" Text="<%# GetOpeNuevasTotalUFMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblOpeNuevasTotalRiesgoUFMP"
                                                                                ClientInstanceName="lblOpeNuevasTotalRiesgoUFMP" Text="<%# GetOpeNuevasTotalRiesgoUFMP(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataTextColumn>

                                                            </Columns>
                                                            <TotalSummary>
                                                                <dx:ASPxSummaryItem FieldName="MontoAprobado" SummaryType="Sum" Tag="MontoAprobado" />
                                                                <dx:ASPxSummaryItem FieldName="MontoVigente" SummaryType="Sum" Tag="MontoVigente" />
                                                                <dx:ASPxSummaryItem FieldName="MontoPropuesto" SummaryType="Sum" Tag="MontoPropuesto" />
                                                                <%--<dx:ASPxSummaryItem FieldName="MarkToModelClp" SummaryType="Sum" Tag="Total" DisplayFormat="Total: {0}" />--%>
                                                            </TotalSummary>
                                                            <Settings ShowTitlePanel="true" />
                                                            <SettingsText Title="" />
                                                            <SettingsPager PageSize="30" />
                                                            <SettingsEditing EditFormColumnCount="2" NewItemRowPosition="Top" />

                                                            <SettingsCommandButton>
                                                                <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                                                <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                                                <%--<UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success pull-right" Styles-Style-Width="10%"></UpdateButton>
                                                    <CancelButton Text="Cancelar" ButtonType="Button"></CancelButton>--%>
                                                            </SettingsCommandButton>
                                                            <Settings ShowFooter="True" />
                                                        </dx:ASPxGridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />

                            <!-- tabla / grilla -->
                            <div class="row">
                                <div class="col-md-12">
                                    <h4>Total Global de la Línea</h4>
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <div class="table-responsive">
                                                        <dx:ASPxGridView ID="gvtotalLinea" runat="server" ClientInstanceName="gvtotalLinea" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                            AutoGenerateColumns="false">
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn FieldName="Moneda" Caption="Moneda" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="MontoAprobado" Caption="Monto Aprobado" HeaderStyle-Wrap="True" VisibleIndex="2">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="MontoVigente" Caption="Monto Vigente" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="MontoPropuesto" Caption="Monto Propuesto" HeaderStyle-Wrap="True" VisibleIndex="2">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <PropertiesTextEdit DisplayFormatString="${0:n0}"></PropertiesTextEdit>
                                                                </dx:GridViewDataTextColumn>
                                                            </Columns>
                                                        </dx:ASPxGridView>
                                                        <%--<table class="table table-bordered table-hover">
                                                            <tr>
                                                                <td></td>
                                                                <td><b>Monto Aprobado</b></td>
                                                                <td><b>Monto Vigente</b></td>
                                                                <td><b>Monto Propuesto </b></td>
                                                            </tr>
                                                            <tr>
                                                                <td><b>Total CLP </b></td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoAprobadoCLP" runat="server" ClientInstanceName="txtTotalMontoAprobadoCLP" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoVigenteCLP" runat="server" ClientInstanceName="txtTotalMontoVigenteCLP" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoPropuestoCLP" runat="server" ClientInstanceName="txtTotalMontoPropuestoCLP" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td><b>Total USD </b></td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoAprobadoUDS" runat="server" ClientInstanceName="txtTotalMontoAprobadoUDS" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoVigenteUDS" runat="server" ClientInstanceName="txtTotalMontoVigenteUDS" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoPropuestoUDS" runat="server" ClientInstanceName="txtTotalMontoPropuestoUDS" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td><b>Total UF </b></td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoAprobadoUF" runat="server" ClientInstanceName="txtTotalMontoAprobadoUF" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoVigenteUF" runat="server" ClientInstanceName="txtTotalMontoVigenteUF" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoPropuestoUF" runat="server" ClientInstanceName="txtTotalMontoPropuestoUF" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td><b>Total Riesgo Equivalente UF </b></td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoAprobadoREUF" runat="server" ClientInstanceName="txtTotalMontoAprobadoREUF" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoVigenteREUF" runat="server" ClientInstanceName="txtTotalMontoVigenteREUF" Width="150" MaxLength="18" ReadOnly="true" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxTextBox ID="txtTotalMontoPropuestoREUF" runat="server" ClientInstanceName="txtTotalMontoPropuestoREUF" Width="150" MaxLength="18" BackColor="Transparent"></dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>--%>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />

                            <div class="row">
                                <div class="col-md-12">
                                    <h4>Resumen Valor de Garantías</h4>
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="card-box">
                                                    <%-- OnCustomCallback="gvGarantias_CustomCallback"
                                                            OnSummaryDisplayText="gvGarantias_SummaryDisplayText"
                                                    ClientSideEvents-EndCallback="EndgvGarantias"--%>
                                                    <div class="table-responsive">
                                                        <dx:ASPxGridView ID="gvGarantias" runat="server" ClientInstanceName="gvGarantias" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                            AutoGenerateColumns="false" KeyFieldName="IdGarantia">
                                                            <Columns>
                                                                <dx:GridViewDataColumn FieldName="Tipo Garantia" Caption="Tipo Garantía" HeaderStyle-Wrap="True" VisibleIndex="1">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Descripción" Caption="Descripción" HeaderStyle-Wrap="True" VisibleIndex="2">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataTextColumn FieldName="Comentarios" Caption="Comentarios" HeaderStyle-Wrap="True" VisibleIndex="3">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <DataItemTemplate>
                                                                        <dx:ASPxMemo runat="server" ID="txtUltimoComentario" ClientIDMode="Static" Rows="3" MaxLength="250" Width="100%" ClientInstanceName="txtUltimoComentario" Columns="20"
                                                                            Text='<%# Eval("Comentarios") %>' Border-BorderWidth="2px">
                                                                        </dx:ASPxMemo>
                                                                    </DataItemTemplate>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataColumn FieldName="Tipo Moneda" Caption="Tipo Moneda" HeaderStyle-Wrap="True" VisibleIndex="4">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">Total Garantía Real(UF):</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">Total Cobertura Vigente:</p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">Total Cobertura Global:</p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Valor Comercial" Caption="Valor Comercial" HeaderStyle-Wrap="True" VisibleIndex="5">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblGarantiaRealVC"
                                                                                ClientInstanceName="lblGarantiaRealVC" Text="<%# GetGarantiaRealVC(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblGarantiaCoberturaVigenteVC"
                                                                                ClientInstanceName="lblGarantiaCoberturaVigenteVC" Text="<%# GetGarantiaCoberturaVigenteVC(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblGarantiaRealTotal"
                                                                                ClientInstanceName="lblGarantiaRealTotal" Text="<%# GetGarantiaRealTotal(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="Valor Ajustado" Caption="Valor Ajustado" HeaderStyle-Wrap="True" VisibleIndex="6">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                                    <FooterTemplate>
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblGarantiaRealVA"
                                                                                ClientInstanceName="lblGarantiaRealVA" Text="<%# GetGarantiaRealVA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblGarantiaCoberturaVigenteVA"
                                                                                ClientInstanceName="lblGarantiaCoberturaVigenteVA" Text="<%# GetGarantiaCoberturaVigenteVA(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                        <br />
                                                                        <p style="font-size: 12px; text-align: right">
                                                                            <dx:ASPxLabel runat="server" ID="lblGarantiaAjustadoTotal"
                                                                                ClientInstanceName="lblGarantiaAjustadoTotal" Text="<%# GetGarantiaAjustadoTotal(Container)%>">
                                                                            </dx:ASPxLabel>
                                                                        </p>
                                                                    </FooterTemplate>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataColumn FieldName="DescEstado" Caption="Estado Garantía" HeaderStyle-Wrap="True" VisibleIndex="7">
                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />

                                                                </dx:GridViewDataColumn>
                                                            </Columns>
                                                            <TotalSummary>
                                                                <dx:ASPxSummaryItem FieldName="Valor Comercial" SummaryType="Sum" Tag="ValorComercial" />
                                                                <dx:ASPxSummaryItem FieldName="Valor Ajustado" SummaryType="Sum" Tag="ValorAjustado" />
                                                            </TotalSummary>
                                                            <Settings ShowTitlePanel="true" />
                                                            <SettingsText Title="" />
                                                            <SettingsPager PageSize="30" />
                                                            <Settings ShowFooter="True" />
                                                        </dx:ASPxGridView>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />

                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table table-bordered" style="font-size: small;">
                                        <tr>
                                            <td><b>COMENTARIOS</b></td>
                                            <td>
                                                <dx:ASPxMemo ID="mmComentario" runat="server" Columns="232" Rows="10" ClientInstanceName="mmComentario" ValidationSettings-ValidationGroup="ValidarCotizador"></dx:ASPxMemo>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />
                            <br />


                            <div class="row">
                                <div class="col-md-12">
                                    <h4>VII. Indicadores Financieros</h4>
                                    <div class="col-md-12 column">

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <dx:ASPxGridView ID="gvPrimero" runat="server" ClientInstanceName="gvPrimero" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                                        AutoGenerateColumns="false" OnInit="gvPrimero_Init">
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <br />

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <dx:ASPxGridView ID="gvSegundo" runat="server" ClientInstanceName="gvSegundo" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                                        AutoGenerateColumns="false" KeyFieldName="Cuenta" OnInit="gvSegundo_Init">
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <br />

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <dx:ASPxGridView ID="gvTercero" runat="server" ClientInstanceName="gvTercero" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                                        AutoGenerateColumns="false" OnInit="gvTercero_Init">
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>


                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <dx:ASPxGridView ID="gvCuarto" runat="server" ClientInstanceName="gvCuarto" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                                        AutoGenerateColumns="false" OnInit="gvCuarto_Init">
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <br />
                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <dx:ASPxGridView ID="gvQuinto" runat="server" ClientInstanceName="gvQuinto" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                                        AutoGenerateColumns="false" OnInit="gvQuinto_Init">
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <br />

                                        <!-- tabla / grilla -->
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <div class="table-responsive">
                                                                    <dx:ASPxGridView ID="gvSexto" runat="server" ClientInstanceName="gvSexto" Width="100%" ClientIDMode="Static" Theme="Moderno"
                                                                        AutoGenerateColumns="false" OnInit="gvSexto_Init">
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <br />
                            <br />

                            <div class="row">
                                <div class="col-md-12">
                                    <table class="table table-bordered" style="font-size: small;">
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="BtnLimpiar" runat="server" ClientInstanceName="BtnLimpiar" AutoPostBack="false" CssClass="btn btn-default pull-left" Text="Limpiar">
                                                    <ClientSideEvents Click="OnBtnLimpiar" />
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btnGuardarPaf" runat="server" Text="Guardar PAF" ClientInstanceName="btnGuardarPaf" CssClass="btn btn-success pull-right" AutoPostBack="false"
                                                    ValidationGroup="ValidarCotizador">
                                                    <%--<ClientSideEvents Click="OnBtnCotizar" />--%>
                                                </dx:ASPxButton>

                                                <dx:ASPxButton ID="btnImprimirPaf" runat="server" Text="Imprimir PAF" ClientInstanceName="btnImprimirPaf" CssClass="btn btn-success pull-right" AutoPostBack="false"
                                                    ValidationGroup="ValidarCotizador">
                                                    <%--<ClientSideEvents Click="OnBtnCotizar" />--%>
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <br />
                            <br />

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>



                <asp:SqlDataSource runat="server" ID="dsProducto" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT p.IdProducto, p.NombreProducto, tp.IdTipoProducto, tp.NombreTipoProducto FROM PRODUCTOS p JOIN TipoProducto tp on p.IdTipoProducto = tp.IdTipoProducto where tp.IdTipoProducto = 5" SelectCommandType="Text"></asp:SqlDataSource>
                <asp:SqlDataSource runat="server" ID="dsFinalidad" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdFinalidad,Nombre from Finalidad where Habilitado = 1 and IdProducto = 10" SelectCommandType="Text"></asp:SqlDataSource>
                <asp:SqlDataSource runat="server" ID="dsMoneda" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdMoneda, Abreviacion from Monedas where Visible = 1" SelectCommandType="Text"></asp:SqlDataSource>

                <dx:ASPxCallback ID="cbPaf" runat="server" ClientInstanceName="cbPaf" OnCallback="cbPaf_Callback" ClientIDMode="Static">
                    <ClientSideEvents CallbackComplete="EndCallbackPaf" />
                </dx:ASPxCallback>

            </div>
        </div>
    </div>

</div>
