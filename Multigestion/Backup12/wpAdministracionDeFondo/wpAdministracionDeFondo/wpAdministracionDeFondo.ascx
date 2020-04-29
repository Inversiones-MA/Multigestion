<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpAdministracionDeFondo.ascx.cs" Inherits="MultiOperacion.wpAdministracionDeFondo.wpAdministracionDeFondo.wpAdministracionDeFondo" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

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

        var a = window.event.keyCode;

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

                if (txtEmpresa.GetText() != '' || txtRut.GetText() != '' || txtNumCertificado.GetText() != '' || cbxEjecutivo.GetSelectedItem().text != 'Seleccione') {
                    GetBuscar();
                }

            }
        }
    }
 
    function MantenSesion() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'actualizando su solicitud', 120, 550);
        __doPostBack('', '');
        return false;
    }

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
        document.getElementById('divGvDetalle').style.visibility = "hidden";
    });

    function OnMoreInfoClick(s, e) {
        var rowVisibleIndex = e.visibleIndex;
        var rowKeyValue = s.GetRowKey(rowVisibleIndex);
        HfEmp.Set("value", rowKeyValue);
        OpenDlg();

        var key = rowKeyValue;
        var boton = e.buttonID;
        var parametros = new Array(key, boton);

        window.gvDetalle.PerformCallback(parametros);
    }

    function RefreshGv(s, e) {
        var rowKeyValue = HfEmp.Get("value");
        var key = rowKeyValue;
        var boton = 'detalle';
        var parametros = new Array(key, boton);

        if (rowKeyValue != null) {
            window.gvDetalle.PerformCallback(parametros);
            ExcluirReporte();
        }
    }

    function ExcluirReporte() {
        var rowKeyValue = HfKey.Get("value");
        var key = rowKeyValue;
        var boton = 'excluir';
        var parametros = new Array(key, boton);
        window.gvDetalleMov.PerformCallback(parametros);
    }

    function OnDetalle(s, e) {
        ocultarDiv();
        var HabCont = null;
        var rowVisibleIndex = e.visibleIndex;
        var rowKeyValue = s.GetRowKey(rowVisibleIndex);
        if (rowKeyValue != null) {
            HfMonto.Set("value", rowKeyValue.split("|")[1]);
            HabCont = rowKeyValue.split("|")[2];
        }
        else
            HabCont = 0;

        //if (HabCont != 0) {
            if (e.buttonID == 'mas') {
                gvDetalle.GetRowValues(e.visibleIndex, 'RazonSocial;NCertificado;SaldoNumCf', CargarHeaderP);
                HfKey.Set("value", rowKeyValue.split("|")[0]);
                //CargarAgregar();
            }
            else {
                gvDetalle.GetRowValues(e.visibleIndex, 'RazonSocial;NCertificado;SaldoNumCf', CargarHeaderD);
                HfKey.Set("value", rowKeyValue.split("|")[0]);
                CargarDetMov(rowKeyValue.split("|")[0]);
            }
        //}
        //else {
        //    alert('falta ingresar el movimiento en contabilidad');
        //}
    }

    function CargarHeaderP(values) {
        var valores = values;
        if (valores !== null) {
            lblEmpresaA.SetText(valores[0]);
            lblCertificadoA.SetText(valores[1]);
            lblTotalCer.SetText(valores[2]);
        }
    }

    function CargarHeaderD(values) {
        var valores = values;
        if (valores !== null) {
            lblEmpresaDet.SetText(valores[0]);
            lblCertificadoDet.SetText(valores[1]);
            lblTotalCer.SetText(valores[2]);
        }
    }

    function formato(s, e) {
        var monto = encodeHtml(s.GetText());
        if (monto != '') {
            monto = parseFloat(monto.replace(/\./g, '').replace(/\,/g, '.'))
            monto = formatNumber.new(parseFloat(monto).toFixed(0));
            s.SetText(monto);
        }
    }

    function CargarDetMov(rowKeyValue) {
        OpenDlg();

        var key = rowKeyValue;
        var boton = 'detalle';
        var parametros = new Array(key, boton);

        if (rowKeyValue != null) {
            window.gvDetalleMov.PerformCallback(parametros);
        }
    }

    function EndAdministracion() {
        if (window.gvDetalle.GetVisibleRowsOnPage() > 0)
            gvDetalle.SetVisible(true);
        CloseDlg();
    }

    function EndgvDetalleMov() {
        if (gvDetalleMov.cpReporte == 'reporte' || gvDetalleMov.cpReporte == undefined) {
            //gvDetalleMov.Refresh();
            pcDetalle.Hide();
        }
        else
            pcDetalle.Show();

        CloseDlg();
    }

    function OnReporte(s, e) {
        var rowVisibleIndex = e.visibleIndex;

        var key = s.GetRowKey(rowVisibleIndex);
        var boton = 'incluirReporte';
        var select = e.isSelected;

        var parametros = new Array(key, boton, select);

        window.gvDetalleMov.PerformCallback(parametros);
    }

 
    //function GetSelectedFieldValuesCallback(values) {
    //    selectedIDs = "";
    //    for (var index = 0; index < values.length; index++) {
    //        selectedIDs += values[index] + ",";
    //    }
    //    if (selectedIDs.length > 0)
    //        selectedIDs = selectedIDs.substring(0, selectedIDs.length - 1);
    //    alert(selectedIDs);
    //}

    function OnSelectionChanged(s, e) {
    //    alert(s.GetChecked());
    //    //SelectAllCheckBox.SetChecked(isAllSelected());
    }

    function EndDetalle() {
        if (window.gvDetalle.GetVisibleRowsOnPage() > 0)
            document.getElementById('divGvDetalle').style.visibility = "visible";
        CloseDlg();
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

    function GetBuscar(s, e) {
        try {
            document.getElementById('divGvDetalle').style.visibility = "hidden";
            ocultarDiv();
            var empresa = txtEmpresa.GetText(); //cmbxEmpresa.GetSelectedItem().value; 
            var rut = txtRut.GetText();
            var certificado = txtNumCertificado.GetText();
            var ejecu = cbxEjecutivo.GetSelectedItem().text;

            var parametros = new Array(empresa, rut, certificado, ejecu);
            window.gvAdministracionFondos.PerformCallback(parametros);
            OpenDlg();
            return false;
        }
        catch (e) {
            alert(e);
        }
    }

    function setGuardarComentario() {
        cbMensaje.PerformCallback('mensaje');
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
                lbWarning.SetText(e.result);
            }
        }
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

    function OnConceptoGvChanged(s, e) {
        var value = s.GetSelectedItem().value.toString();
        gvDetalleMov.GetEditor("IdDetalle").PerformCallback(value);
    }

    function OnClick(s, e) {
        gvDetalleMov.AddNewRow();
    }

    function EndCallbackDescargar(s, e) {
        CloseDlg();
    }

    function imprimir() {
        cbDescargar.PerformCallback('imprimir');
    }

    //evitar edicion formulario sharepoint
    function WebForm_OnSubmit() {
        return false;
    }

    function popComentario() {
        pcComentario.Show();
    }

    function EndCallbackMensaje() {
        ComentarioGral.SetText(null);
        pcComentario.Hide();
    }

</script>


<style type="text/css">
/*body{
    }

.dxgvHeader a.dxgvCommandColumnItem,
.dxgvHeader a.dxgvCommandColumnItem:hover,
a.dxbButton, a.dxbButton:hover{color: #FF8800;text-decoration: underline;}
#grid{
    border: 1px Solid #c0c0c0;
    font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
    background-color: White;
    color: #555;
    cursor: default;
}
.dxgvHeader{
    font-weight:bolder;   
    background-color:#FFFFFF; 
    border:0;
    border-bottom:1px #CCC solid;
    padding:8px;
}*/

body {
}
/*.dxgvTitlePanel {display:none;}*/
.dxgvHeader a.dxgvCommandColumnItem,
.dxgvHeader a.dxgvCommandColumnItem:hover,
a.dxbButton, a.dxbButton:hover{color: #FF8800;text-decoration: underline;}
#grid{
    border: 1px Solid #c0c0c0;
    font: 12px 'Segoe UI', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif;
    background-color: White;
    color: #555;
    cursor: default;
}
.dxgvHeader{
    font-weight:bolder;   
    background-color:#FFFFFF; 
    border:0;
    border-bottom:1px #CCC solid;
    padding:8px;
}
/*.dxgvHeader td:hover{text-decoration:underline;}
.dxgvDataRow td.dxgv{padding:8px;}
.dxgvEditFormDisplayRow td.dxgv{padding:8px;}
.dxgvEditFormDisplayRow td.dxgvCommandColumn{border-right:0;}
.dxeTextBoxSys{width:100px;}
.dxgvEditForm{background-color:#f9f9f9;}*/
.dxeButtonEdit,.dxeTextBox{
    background-color: #FFF;
    border: 1px solid #c1c1c1;
    padding:2px 1px;
   border-radius:5px;
     /*height:20px; */
}
.dxgvEditFormDisplayRow td{color:black;font:12px Tahoma, Geneva, sans-serif;}
.imgCell{float:right;cursor:pointer;}
.marginBottom {margin-bottom:15px;}
/*.dxgvControl, .dxgvDisabled {
    border: 1px Solid #d1d1d1;
    background-color: #F2F2F2;
    color: Black;
    cursor: default;
}*/

.divClass {
  width: 30px;
  height: 30px;
  border: 0px solid
}
</style>

 <div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
      <h4>
         <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
     </h4>
 </div>

									
<div id="dvFormulario" runat="server">

    <dx:ASPxHiddenField ID="HfKey" runat="server" ClientInstanceName="HfKey" ClientIDMode="Static"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="HfEmp" runat="server" ClientInstanceName="HfEmp" ClientIDMode="Static"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="HfMonto" runat="server" ClientInstanceName="HfMonto" ClientIDMode="Static"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="HfFechaMov" runat="server" ClientInstanceName="HfFechaMov" ClientIDMode="Static"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="HfMovimiento" runat="server" ClientInstanceName="HfMovimiento" ClientIDMode="Static"></dx:ASPxHiddenField>

    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Administración de Fondos</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Inicio</a>
                </li>
                <li class="active">Administración de Fondos
                </li>
            </ol>
        </div>
    </div>

    <div class="row">
        <br />
        <div id="dvSuccess" style="display: none">
            <h4>
                <dx:ASPxLabel ID="lbSuccess" runat="server" ClientInstanceName="lbSuccess" ClientIDMode="Static" CssClass="alert alert-success" Text=""></dx:ASPxLabel>
            </h4>
        </div>
        <div id="dvWarning" style="display: none">
            <h4>
                <dx:ASPxLabel ID="lbWarning" runat="server" ClientInstanceName="lbWarning" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
            </h4>
        </div>
        <div id="dvError" style="display: none">
            <h4>
                <dx:ASPxLabel ID="lbError" runat="server" ClientInstanceName="lbError" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
            </h4>
        </div>
        <br />
    </div>

    <div class="row" id="divContenedor">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">
                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Empresa</label>
                            <dx:ASPxTextBox ID="txtEmpresa" runat="server" ClientInstanceName="txtEmpresa" CssClass="form-control" MaxLength="200"></dx:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Rut</label>
                            <dx:ASPxTextBox ID="txtRut" runat="server" ClientInstanceName="txtRut" CssClass="form-control" MaxLength="15"></dx:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Número Certificado</label>
                            <dx:ASPxTextBox ID="txtNumCertificado" runat="server" ClientInstanceName="txtNumCertificado" CssClass="form-control" MaxLength="5"></dx:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Ejecutivo</label>
                            <dx:ASPxComboBox ID="cbxEjecutivo" runat="server" ValueType="System.String" ClientInstanceName="cbxEjecutivo" ClientIDMode="Static" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>

                        <div class="form-group">
                            <label for=""></label>
                            <dx:ASPxButton ID="btnBuscar" runat="server" ClientInstanceName="btnBuscar" Text="Buscar" AutoPostBack="false" ClientSideEvents-Click="GetBuscar" CssClass="btn btn-primary btn-success"></dx:ASPxButton>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-12">
                    <div class="card-box">
                        <table border="0" style="table-layout: fixed; width: 100%;">
                            <tr>
                                <td>
                                    <div class="card-box">
                                        <div class="table-responsive">
                                            <dx:ASPxGridView ID="gvTotalEje" runat="server" AutoGenerateColumns="False" Width="90%" ClientInstanceName="gvTotalEje" KeyFieldName="IdEjecutivo"
                                                OnPageIndexChanged="gvTotalEje_PageIndexChanged">
                                                <Columns>
                                                    <dx:GridViewDataColumn FieldName="IdEjecutivo" Caption="Empresa" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Left" Visible="false">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="DescEjecutivo" Caption="Ejecutivo" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataColumn FieldName="SumaCF" Caption="Total Ejecutivo" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Left">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                    </dx:GridViewDataColumn>
                                                </Columns>

                                                <SettingsBehavior AllowSort="false" AllowGroup="false" />
                                                <%--<SettingsLoadingPanel ShowImage="false" Mode="Disabled" />--%>
                                                <Styles>
                                                    <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                                </Styles>
                                                <Settings ShowTitlePanel="true" />
                                                <SettingsText Title="Total Ejecutivo" />
                                                <SettingsPager PageSize="15" />
                                            </dx:ASPxGridView>

                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>

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
                                        <dx:ASPxGridView ID="gvAdministracionFondos" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="gvAdministracionFondos" KeyFieldName="IdEmpresa"
                                            OnPageIndexChanged="gvAdministracionFondos_PageIndexChanged"
                                            OnCustomCallback="gvAdministracionFondos_CustomCallback"
                                            ClientSideEvents-EndCallback="EndAdministracion">
                                            <ClientSideEvents CustomButtonClick="OnMoreInfoClick" />

                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RazonSocial" Caption="Empresa" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Rut" Caption="Rut" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="DescEjecutivo" Caption="Ejecutivo Empresa" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="CF" Caption="Certificados" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center" Width="30%">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataColumn FieldName="SumaCF" Caption="Saldo" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>

                                                <dx:GridViewCommandColumn Caption="Acciones" VisibleIndex="8" Name="Acciones" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ButtonType="Image">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                    <CustomButtons>

                                                        <dx:GridViewCommandColumnCustomButton ID="detalle" Text="" Image-ToolTip="Ver Detalle">
                                                            <Image Url="../../_layouts/15/images/MultiGestion/font-azul.png" Height="15px" Width="15px">
                                                            </Image>
                                                            <Styles Style-CssClass="data-toggle data-placement data-original-title">
                                                            </Styles>
                                                        </dx:GridViewCommandColumnCustomButton>

                                                    </CustomButtons>
                                                </dx:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsBehavior AllowSort="false" AllowGroup="false" />

                                            <Styles>
                                                <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                            </Styles>
                                            <Settings ShowTitlePanel="true" />
                                            <SettingsText Title="Administración de Fondos" />
                                            <SettingsPager PageSize="10" />

                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div id="divGvDetalle" class="row">
                <div class="col-md-12">
                    <table border="0" style="table-layout: fixed; width: 100%;">
                        <tr>
                            <td>
                                <div class="card-box">
                                    <div class="table-responsive">
                                        <dx:ASPxGridView ID="gvDetalle" runat="server" ClientInstanceName="gvDetalle" AutoGenerateColumns="False" Width="100%" KeyFieldName="NCertificado;SaldoNumCf;Contabilidad"
                                            OnPageIndexChanged="gvDetalle_PageIndexChanged"
                                            OnCustomCallback="gvDetalle_CustomCallback"
                                            ClientSideEvents-EndCallback="EndDetalle"
                                            OnCustomColumnDisplayText="gvDetalle_CustomColumnDisplayText">
                                            <ClientSideEvents CustomButtonClick="OnDetalle" />
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="RazonSocial" Caption="Empresa" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Acreedor" Caption="Acreedor" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="NCertificado" Caption="Numero Certificado" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="SaldoNumCf" Caption="Saldo" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="IdOperacion" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center" Visible="false">
                                                </dx:GridViewDataColumn>

                                                <dx:GridViewDataColumn FieldName="FecMovimiento" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center" Visible="false">
                                                </dx:GridViewDataColumn>

                                                <dx:GridViewCommandColumn Caption="Acciones" VisibleIndex="7" Name="Acciones" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ButtonType="Image">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                    <CustomButtons>
                                                        <dx:GridViewCommandColumnCustomButton ID="ver" Text="" Image-ToolTip="Ver Movimientos">
                                                            <Image Url="../../_layouts/15/images/MultiGestion/resize-azul.png" Height="15px" Width="15px">
                                                            </Image>
                                                            <Styles Style-CssClass="data-toggle">
                                                            </Styles>
                                                        </dx:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dx:GridViewCommandColumn>
                                            </Columns>
                                            <Styles>
                                                <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                            </Styles>
                                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                                            <Settings ShowTitlePanel="true" />
                                            <SettingsText Title="Detalle" />
                                            <SettingsPager PageSize="20" />
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

    <%--OnWindowCallback="pcDetalle_WindowCallback"--%>
    <dx:ASPxPopupControl ID="pcDetalle" runat="server" ClientInstanceName="pcDetalle" CloseAction="CloseButton"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Detalle de Movimientos" HeaderStyle-Font-Bold="true"
        AllowDragging="True"
        PopupAnimationType="None"
        Width="1000px" Height="700px">
        <HeaderStyle Font-Bold="True" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1">
                <div class="row">

                    <div class="col-md-12">
                        <div class="form-horizontal" role="form">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="divOkP" style="display: none">
                                            <h2>
                                                <dx:ASPxLabel ID="lblOkDet" runat="server" ClientInstanceName="lblOkDet" ClientIDMode="Static" CssClass="alert alert-success" Text=""></dx:ASPxLabel>
                                            </h2>
                                        </div>
                                        <div id="divWarningP" style="display: none">
                                            <h2>
                                                <dx:ASPxLabel ID="lblWarningDet" runat="server" ClientInstanceName="lblWarningDet" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
                                            </h2>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="alert alert-gray">
                                            <p>
                                                <strong>Empresa:
                                                                   <dx:ASPxLabel ID="lblEmpresaDet" runat="server" Text="" ClientInstanceName="lblEmpresaDet"></dx:ASPxLabel>
                                                </strong>
                                                / 
                                                                <strong>Certificado:
                                                                   <dx:ASPxLabel ID="lblCertificadoDet" runat="server" Text="" ClientInstanceName="lblCertificadoDet"></dx:ASPxLabel>
                                                                </strong>
                                                / 
                                                                <strong>Total Certificado:
                                                                   <dx:ASPxLabel ID="lblTotalCer" runat="server" Text="" ClientInstanceName="lblTotalCer"></dx:ASPxLabel>
                                                                </strong>
                                            </p>
                                            <dx:ASPxButton ID="AddComentario" runat="server" ClientInstanceName="AddComentario" Text="Agregar Comentario" AutoPostBack="false" CssClass="btn btn-primary btn-success">
                                                <ClientSideEvents Click="popComentario" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                                <br />

                                <div class="col-md-12">
                                    <div class="form-group">
                                        <dx:ASPxGridView ID="gvDetalleMov" runat="server" ClientInstanceName="gvDetalleMov" Width="100%" OnPageIndexChanged="gvDetalleMov_PageIndexChanged"
                                            ClientSideEvents-EndCallback="EndgvDetalleMov"
                                            KeyFieldName="IdAdministracionFondos" AutoGenerateColumns="false"
                                            OnCellEditorInitialize="gvDetalleMov_CellEditorInitialize"
                                            OnRowDeleting="gvDetalleMov_RowDeleting"
                                            OnRowUpdating="gvDetalleMov_RowUpdating"
                                            OnRowValidating="gvDetalleMov_RowValidating"
                                            OnCustomCallback="gvDetalleMov_CustomCallback"
                                            OnRowInserting="gvDetalleMov_RowInserting">
                                            <ClientSideEvents SelectionChanged="OnReporte" />
                                            <SettingsBehavior ConfirmDelete="true" />
                                            <SettingsText ConfirmDelete="¿Esta seguro de borrar este registro?" />
                                            <Columns>

                                                <dx:GridViewCommandColumn VisibleIndex="0" Width="5%" ShowEditButton="true" ShowDeleteButton="true">
                                                    <HeaderTemplate>
                                                        <dx:ASPxButton ID="BtnNew" ClientInstanceName="BtnNew" runat="server" Text="Nuevo" RenderMode="Button"
                                                            AutoPostBack="false" CssClass="btn btn-block btn-warning btn-xs" Width="10%">
                                                            <ClientSideEvents Click="OnClick" />
                                                        </dx:ASPxButton>
                                                    </HeaderTemplate>
                                                </dx:GridViewCommandColumn>

                                                <dx:GridViewDataComboBoxColumn FieldName="IdTipoMov" Caption="Tipo Movimiento" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center">
                                                    <PropertiesComboBox ValueType="System.Int32" TextField="DescTipoMov" ValueField="IdTipoMov" DataSourceID="dsTipoMov">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                </dx:GridViewDataComboBoxColumn>

                                                <dx:GridViewDataComboBoxColumn FieldName="IdDestino" Caption="Concepto Pago" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center">
                                                    <PropertiesComboBox ValueType="System.Int32" TextField="ConceptoPago" ValueField="IdDestino" DataSourceID="dsConceptoPago">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnConceptoGvChanged(s,e); }"></ClientSideEvents>
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                </dx:GridViewDataComboBoxColumn>

                                                <dx:GridViewDataComboBoxColumn FieldName="IdDetalle" Caption="Detalle" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center">
                                                    <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="Detalle" ValueField="IdDetalle" DataSourceID="dsDetalleConceptoPago">
                                                        <ValidationSettings>
                                                            <RequiredField IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                </dx:GridViewDataComboBoxColumn>

                                                <dx:GridViewDataDateColumn FieldName="FechaMovimiento" Caption="Fecha Movimiento" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormat="Date"
                                                    PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                                                </dx:GridViewDataDateColumn>

                                                <dx:GridViewDataTextColumn FieldName="MontoMovimiento" Caption="Monto Movimiento" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Center">
                                                    <EditItemTemplate>
                                                        <dx:ASPxTextBox ID="txtMontoMovimiento" runat="server" Width="90%" ClientInstanceName="txtMontoMovimiento" Text='<%# Eval("MontoMovimiento") %>'>
                                                            <ClientSideEvents LostFocus="formato" />
                                                            <ClientSideEvents KeyPress="function(s,e){ esNumero(s,e);}" />
                                                        </dx:ASPxTextBox>
                                                    </EditItemTemplate>
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="Comentario" Caption="Comentario" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Center">
                                                    <EditItemTemplate>
                                                        <dx:ASPxMemo runat="server" ID="txtComentario" ClientIDMode="Static" Rows="3" MaxLength="200" ClientInstanceName="txtComentario" Columns="20" Text='<%# Eval("Comentario") %>' HorizontalAlign="Left">
                                                            <ValidationSettings>
                                                                <RequiredField IsRequired="true" />
                                                            </ValidationSettings>
                                                        </dx:ASPxMemo>
                                                    </EditItemTemplate>
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewDataTextColumn FieldName="Saldo" Caption="Monto Insoluto" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Center">
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataTextColumn>

                                                <dx:GridViewCommandColumn VisibleIndex="8" Caption="Instruccion" Name="Instruccion" ShowSelectCheckbox="true" ShowClearFilterButton="false"
                                                    SelectAllCheckboxMode="Page">
                                                    <%--<HeaderTemplate>
                                                        <dx:ASPxCheckBox ID="chkInstruccion" ClientInstanceName="chkInstruccion" runat="server" AllowGrayed="false" CssClass="countCB"
                                                            ValueChecked="true" ValueUnchecked="false">
                                                            <ClientSideEvents CheckedChanged="OnSelectionChanged" />
                                                        </dx:ASPxCheckBox>
                                                    </HeaderTemplate>   --%>
                                                </dx:GridViewCommandColumn>

                                                <%--<dx:GridViewDataCheckColumn UnboundType="Boolean" VisibleIndex="8" Caption="Instruccion de Curse" Name="Instruccion">
                                                    <DataItemTemplate>
                                                        <dx:ASPxCheckBox ID="chkInstruccion" ClientInstanceName="chkInstruccion" runat="server" AllowGrayed="false" CssClass="countCB"
                                                            ValueChecked="true" ValueUnchecked="false">
                                                            <ClientSideEvents CheckedChanged="OnSelectionChanged" />
                                                        </dx:ASPxCheckBox>
                                                    </DataItemTemplate>
                                                    <EditFormSettings Visible="False" />
                                                </dx:GridViewDataCheckColumn>--%>

                                                <dx:GridViewDataColumn FieldName="IdOperacion" HeaderStyle-Wrap="True" VisibleIndex="9" CellStyle-HorizontalAlign="Left" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>

                                                <dx:GridViewDataColumn FieldName="IdAdministracionFondos" HeaderStyle-Wrap="True" VisibleIndex="10" CellStyle-HorizontalAlign="Left" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                </dx:GridViewDataColumn>

                                            </Columns>
                                            <SettingsBehavior AllowSort="false" />
                                            <SettingsPager PageSize="10"></SettingsPager>
                                            <SettingsCommandButton>
                                                <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                                <DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>
                                            </SettingsCommandButton>
                                            <SettingsCommandButton>
                                                <UpdateButton Text="Guardar"></UpdateButton>
                                                <CancelButton Text="Cancelar"></CancelButton>
                                            </SettingsCommandButton>
                                            <SettingsEditing EditFormColumnCount="2" NewItemRowPosition="Bottom" />
                                        </dx:ASPxGridView>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <dx:ASPxButton ID="btnImprimirInstruccion" runat="server" ClientInstanceName="btnImprimirInstruccion" Text="Descargar Instruccion Curse" AutoPostBack="false"
                                        CssClass="btn btn-primary btn-success">
                                        <ClientSideEvents Click="imprimir" />
                                    </dx:ASPxButton>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="function(s, e) {RefreshGv(); }" />
    </dx:ASPxPopupControl>

    <asp:SqlDataSource runat="server" ID="dsTipoMov" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM TipoMovimiento" SelectCommandType="Text"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsConceptoPago" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM ConceptoPago Where Habilitado = 1" SelectCommandType="Text"></asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsDetalleConceptoPago" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM DetallePorConcepto Where Habilitado = 1" SelectCommandType="Text">
    </asp:SqlDataSource>

    <asp:SqlDataSource runat="server" ID="dsConceptoPagoPor" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="SELECT * FROM DetallePorConcepto where IdConceptoPago = @IdDetalle" SelectCommandType="Text">
        <SelectParameters>
            <asp:Parameter Name="IdDetalle" Type="Int32" Direction="Input" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>

    <dx:ASPxPopupControl ID="pcComentario" runat="server" ClientInstanceName="pcComentario" CloseAction="CloseButton"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Agregar Comentario" HeaderStyle-Font-Bold="true"
        AllowDragging="True"
        PopupAnimationType="None"
        Width="500px" Height="400px">
        <HeaderStyle Font-Bold="True" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3">
                <div class="row">
                    <div class="col-md-12">
                        <dx:ASPxMemo ID="ComentarioGral" runat="server" ClientInstanceName="ComentarioGral" CssClass="form-control" Border-BorderWidth="1px" Rows="20"></dx:ASPxMemo>

                        <br />
                        <br />

                        <div class="form-group">
                            <div class="col-sm-7">
                                <dx:ASPxButton ID="btnGuardarComentario" runat="server" Text="Guardar Comentario" AutoPostBack="false" ClientInstanceName="btnGuardarComentario" ClientSideEvents-Click="setGuardarComentario" CssClass="btn btn-primary btn-success"></dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <%--    <dx:ASPxCallback ID="cpGuardar" ClientInstanceName="cpGuardar" runat="server" OnCallback="cpGuardar_Callback">
        <ClientSideEvents CallbackComplete="EndGuardar" />
    </dx:ASPxCallback>--%>

    <dx:ASPxCallback ID="cbDescargar" runat="server" ClientInstanceName="cbDescargar" OnCallback="cbDescargar_Callback" ClientIDMode="Static">
        <ClientSideEvents CallbackComplete="EndCallbackDescargar" />
    </dx:ASPxCallback>

    <dx:ASPxCallback ID="cbMensaje" runat="server" ClientInstanceName="cbMensaje" OnCallback="cbMensaje_Callback" ClientIDMode="Static">
        <ClientSideEvents CallbackComplete="EndCallbackMensaje" />
    </dx:ASPxCallback>

</div>  
        
