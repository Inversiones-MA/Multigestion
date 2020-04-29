<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpAcreedores.ascx.cs" Inherits="MultiOperacion.wpAcreedores.wpAcreedores.wpAcreedores" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript">

    function OnClick(s, e) {
        gvAcreedores.AddNewRow();
    }

    function ocultarDiv() {
        var dvs = document.getElementById('dvSuccess');
        dvs.style.display = 'none';
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'none';
        var dve = document.getElementById('dvError');
        dve.style.display = 'none';
    }

    function GetBuscar(s, e) {
        try {
            ocultarDiv();
            OpenDlg();
            var NombreAcreedor = txtAcreedor.GetText();
            var rutAcreedor = txtRutAcreedor.GetText();

            var parametros = new Array(NombreAcreedor, rutAcreedor);
            window.gvAcreedores.PerformCallback(parametros);
            return false;
        }
        catch (e) {
            alert(e);
        }
    }

    function EndGvAcreedores() {
        CloseDlg();
    }

    function OpenDlg() {
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
    }

    function CloseDlg() {
        SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Cancel);
    }

    function OnRegionGvChanged(s, e) {
        var value = s.GetSelectedItem().value.toString();
        gvAcreedores.GetEditor("Idprovincia").PerformCallback(value);
    }

    function OnProvinciaGvChanged(s, e) {
        var value = s.GetSelectedItem().value.toString();
        gvAcreedores.GetEditor("IdComuna").PerformCallback(value);
    }

    function esNumeroDv(s, e) {
        var theEvent = e.htmlEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
        if (key.length == 0) return;
        var regex = /^[0-9Kk.\b]+$/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }

    function validar(s, e) {
        if (s.GetText() != '') {
            var result = validaRut(s);
            if (!result) {
                alert("Rut Invalido");
                s.SetText(null);
            }
        }
    }

    function validaRut(a) {
        var rut = a.GetText();
        rut = rut.replace(/(\.|-)/g, "");
        var largo = rut.length;
        var largoAux = largo - 2;
        var cont = 2;
        var i = 0;
        var acum = 0;
        var resto = 0;
        var aux = "", respValida = 'txtrespRut';
        var dv = rut.charAt(largo - 1);
        var resp = "FALSE";
        try {
            var rutSinDigito = parseInt(rut.substring(0, rut.length - 1));
            if (rutSinDigito < 100000) {
                return false;
            }
        }
        catch (err) { }

        if (largo > 10 || rut == "") {
            return false;
        }
        else {
            for (i = 0; i < largo - 1; i++, largoAux--) {
                if (cont == 8)
                    cont = 2;
                acum += parseInt(rut.charAt(largoAux)) * cont;
                cont++;
            }
            resto = acum % 11;
            resto = 11 - resto;
            if (resto == 11) {
                if (dv != 0)
                    resp = "FALSE";
                else {
                    resp = "TRUE";
                }
            }
            else if (resto == 10) {
                if (dv != 'k' && dv != 'K')
                    resp = "FALSE";
                else {
                    resp = "TRUE";
                }
            }
            else if (resto == dv) {
                resp = "TRUE";
            }
            else
                resp = "FALSE";
            if (resp == "TRUE") {

                var tmp = rut;
                var ruttt = tmp.substring(0, tmp.length - 1), f = "";
                while (ruttt.length > 3) {
                    f = '.' + ruttt.substr(ruttt.length - 3) + f;
                    ruttt = ruttt.substring(0, ruttt.length - 3);
                }
                var asd = ($.trim(ruttt) == '') ? '' : ruttt + f + "-" + tmp.charAt(tmp.length - 1);
                a.SetText(asd);

                return true;
            }
            else {
                return false;
            }
        }
        return false;
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

    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Mantenedor de Acreedores</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Inicio</a>
                </li>
                <li class="active">Mantenedor de Acreedores
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
                <div class="row" style="margin-left:25px">

                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Acreedor</label>
                            <dx:ASPxTextBox ID="txtAcreedor" runat="server" ClientInstanceName="txtAcreedor" CssClass="form-control" MaxLength="200"></dx:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div class="form-group">
                            <label for="">Rut Acreedor</label>
                            <dx:ASPxTextBox ID="txtRutAcreedor" runat="server" ClientInstanceName="txtRutAcreedor" CssClass="form-control" MaxLength="15" ClientSideEvents-KeyPress="function(s,e){ esNumeroDv(s,e);}"
                                ClientSideEvents-LostFocus="function(s,e){ validar(s,e);}" ></dx:ASPxTextBox>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6">
                        <div class="form-group" style="margin-top:20px" >
                            <label for=""></label>
                            <dx:ASPxButton ID="btnBuscar" runat="server" ClientInstanceName="btnBuscar" Text="Buscar" AutoPostBack="false" ClientSideEvents-Click="GetBuscar" CssClass="btn btn-primary btn-success"></dx:ASPxButton>
                        </div>
                    </div>

                </div>

                <br />
                <br />

                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <table border="0" style="table-layout: fixed; width: 100%;">
                                <tr>
                                    <td>
                                        <div class="card-box">
                                            <div class="table-responsive">

                                                <dx:ASPxGridView ID="gvAcreedores" runat="server" ClientInstanceName="gvAcreedores" Width="100%" Theme="Moderno"
                                                    OnPageIndexChanged="gvAcreedores_PageIndexChanged" KeyFieldName="IdAcreedor" OnCustomCallback="gvAcreedores_CustomCallback"
                                                    OnCellEditorInitialize="gvAcreedores_CellEditorInitialize" OnRowUpdating="gvAcreedores_RowUpdating" OnRowInserting="gvAcreedores_RowInserting"
                                                    OnRowValidating="gvAcreedores_RowValidating"
                                                    ClientSideEvents-EndCallback="EndGvAcreedores">

                                                    <Columns>
                                                        <dx:GridViewCommandColumn VisibleIndex="0" Width="5%" ShowEditButton="true" ShowNewButtonInHeader="true">
                                                            <HeaderTemplate>
                                                                <dx:ASPxButton ID="BtnNew" ClientInstanceName="BtnNew" runat="server" Text="Nuevo" RenderMode="Button"
                                                                    AutoPostBack="false" CssClass="btn btn-block btn-warning btn-xs" Width="10%">
                                                                    <ClientSideEvents Click="OnClick" />
                                                                </dx:ASPxButton>
                                                            </HeaderTemplate>
                                                        </dx:GridViewCommandColumn>

                                                        <dx:GridViewDataTextColumn FieldName="Rut" Caption="Rut" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Left" PropertiesTextEdit-MaxLength="15" Width="8%"
                                                            PropertiesTextEdit-ClientSideEvents-KeyPress="function(s,e){ esNumeroDv(s,e);}" PropertiesTextEdit-ClientSideEvents-LostFocus="function(s,e){ validar(s,e);}"
                                                            PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                            <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="Nombre" Caption="Nombre Acreedor" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Left" PropertiesTextEdit-MaxLength="250"
                                                            PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="Domicilio" Caption="Domicilio" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Left" PropertiesTextEdit-MaxLength="250"
                                                            PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />                                                   
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataComboBoxColumn FieldName="IdTipoAcreedor" Caption="Tipo Acreedor" HeaderStyle-Wrap="True" VisibleIndex="4" CellStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                            <PropertiesComboBox ValueType="System.Int32" TextField="NombreAcredor" ValueField="IdTipoAcreedor" DataSourceID="dsTipoAcreedor">
                                                                <ValidationSettings>
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>

                                                        <dx:GridViewDataComboBoxColumn FieldName="IdRegion" Caption="Región" HeaderStyle-Wrap="True" VisibleIndex="5" CellStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                            <PropertiesComboBox ValueType="System.Int32" TextField="NombreRegion" ValueField="IdRegion" DataSourceID="dsRegion">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnRegionGvChanged(s,e); }"></ClientSideEvents>
                                                                <ValidationSettings>
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>

                                                         <dx:GridViewDataComboBoxColumn FieldName="Idprovincia" Caption="Provincia" HeaderStyle-Wrap="True" VisibleIndex="6" CellStyle-HorizontalAlign="Left" PropertiesComboBox-NullText="">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                            <PropertiesComboBox ValueType="System.Int32" TextField="DescCiudad" ValueField="IdCiudad" DataSourceID="dsProvincia">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnProvinciaGvChanged(s,e); }"></ClientSideEvents>
                                                                <ValidationSettings>
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>

                                                       <dx:GridViewDataComboBoxColumn FieldName="IdComuna" Caption="Comuna" HeaderStyle-Wrap="True" VisibleIndex="7" CellStyle-HorizontalAlign="Left">
                                                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                            <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith" ValueType="System.Int32" TextField="NombreComuna" 
                                                                ValueField="IdComuna" DataSourceID="dsComuna">
                                                                <ValidationSettings>
                                                                    <RequiredField IsRequired="True" />
                                                                </ValidationSettings>
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>
                                                    </Columns>

                                                    <SettingsBehavior AllowSort="false" AllowGroup="false" />
                                                    <Styles>
                                                        <CommandColumn HorizontalAlign="Center"></CommandColumn>
                                                    </Styles>
                                                    <Settings ShowTitlePanel="true" />
                                                    <SettingsText Title="Acreedores" />
                                                    <SettingsPager PageSize="30" />
                                                    <SettingsCommandButton>
                                                        <EditButton Text="Editar" ButtonType="Link" Styles-Style-Font-Size="8"></EditButton>
                                                        <%--<DeleteButton Text="Borrar" ButtonType="Link" Styles-Style-Font-Size="8"></DeleteButton>--%>
                                                        <UpdateButton Text="Guardar" ButtonType="Button" Styles-Style-CssClass="btn btn-primary btn-success" Styles-Style-Width="10%"></UpdateButton>
                                                        <CancelButton Text="Cancelar" ButtonType="Button" ></CancelButton>
                                                    </SettingsCommandButton>
                                                </dx:ASPxGridView>


                                                <asp:SqlDataSource runat="server" ID="dsTipoAcreedor" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdTipoAcreedor, NombreAcredor from TipoAcreedores order by NombreAcredor asc " SelectCommandType="Text"></asp:SqlDataSource>

                                                <asp:SqlDataSource runat="server" ID="dsRegion" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdRegion, NombreRegion from Region order by NombreRegion asc" SelectCommandType="Text"></asp:SqlDataSource>

                                                 <asp:SqlDataSource runat="server" ID="dsProvincia" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdCiudad, DescCiudad from Ciudad order by DescCiudad asc" SelectCommandType="Text">
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource runat="server" ID="dsProvinciaPor" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdCiudad, DescCiudad from Ciudad where IdRegion = @IdRegion order by DescCiudad asc" SelectCommandType="Text">
                                                    <SelectParameters>
                                                        <asp:Parameter Name="IdRegion" Type="Int32" Direction="Input" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource runat="server" ID="dsComuna" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdComuna, NombreComuna from Comuna order by NombreComuna asc" SelectCommandType="Text">
                                                </asp:SqlDataSource>

                                                <asp:SqlDataSource runat="server" ID="dsComunaPor" ConnectionString="<%$ ConnectionStrings:DACConnectionString %>" SelectCommand="select IdComuna, NombreComuna from Comuna where IdCiudad = @IdCiudad order by NombreComuna asc" SelectCommandType="Text">
                                                    <SelectParameters>
                                                        <asp:Parameter Name="IdCiudad" Type="Int32" Direction="Input" DefaultValue="0" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>

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
    </div>

</div>
