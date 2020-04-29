<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPerfiles.ascx.cs" Inherits="MultiAdministracion.wpPerfiles.wpPerfiles.wpPerfiles" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<script src="../../_layouts/15/MultiAdministracion/wpValidaciones.js"></script>
<script src="../../_layouts/15/MultiAdministracion/jquery-1.8.3.min.js"></script>

<style type="text/css">
body {
}
.dxgvTitlePanel {display:none;}
.dxgvHeader a.dxgvCommandColumnItem,.dxgvHeader a.dxgvCommandColumnItem:hover,a.dxbButton, a.dxbButton:hover{
    color: #428bca !important;
    text-decoration: underline;
    font: 14px 'Noto Sans', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif !important;
}
.dxpLite,.dxp-num, .dxp-current{
    color: #797979 !important;
}
#grid{
    border: 1px Solid #c0c0c0;
    font: 14px 'Noto Sans', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif !important;
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
.dxgvControl, .dxgvDisabled { font: 14px 'Noto Sans', Helvetica, 'Droid Sans', Tahoma, Geneva, sans-serif !important; border: 1px solid #ebeff2 !important;}
.dxgvHeader td:hover{text-decoration:underline;}
.dxgvDataRow td.dxgv{padding:8px;}
.dxgvEditFormDisplayRow td.dxgv{padding:8px;}
.dxgvEditFormDisplayRow td.dxgvCommandColumn{border-right:0;}
.dxeTextBoxSys{width:100px;}
.dxgvEditForm{background-color:#f9f9f9;}
.dxeButtonEdit,.dxeTextBox{
    background-color: #FFF;
    border: 1px solid #c1c1c1;
    padding:2px 1px;
    border-radius:5px;
    height:20px;
}
.dxgvEditFormDisplayRow td{color:black;font:12px Tahoma, Geneva, sans-serif;}
.imgCell{float:right;cursor:pointer;}
.marginBottom {margin-bottom:15px;}
.dxgvControl, .dxgvDisabled {
    border: 1px Solid #d1d1d1;
    background-color: #F2F2F2;
    color: Black;
    cursor: default;
}
</style>


<script type="text/javascript">

    $(document).ready(function () {
        ocultarDiv();
    });

    function ocultarDiv() {
        var dvs = document.getElementById('dvSuccess');
        dvs.style.display = 'none';
        var dvw = document.getElementById('dvWarning');
        dvw.style.display = 'none';
        var dve = document.getElementById('dvError');
        dve.style.display = 'none';
    }

    function OnEndCallback(s, e) {
        if (s.cpMessage != null) {
            showMessage(s.cpMessage);
            s.cpMessage = null;
        }

        if (s.cpError != null) {
            showError(s.cpError);
            s.cpError = null;
        }
    }

    function showMessage(msg) {
        var el = document.getElementById('dvSuccess');
        el.style.display = "block";
        lbSuccess.SetText(msg);
    }

    function showError(msg) {
        var el = document.getElementById('dvWarning');
        el.style.display = "block";
        lbWarning.SetText(msg);
    }


    function nuevo(s, e) {
        grid.AddNewRow();
    }

    function btnSaveClick(s, e) {
        try {

            window.grid.PerformCallback('grabar');
            return true;                  
        } catch (e) {
            alert(e);
        }
    }

    function btnCancelClick(s, e) {
        grid.CancelEdit();
    }

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
                        { return false; } //No realiza el backspace(largo igual a 0). 
                    }
                }
            }
        }
    }

    function loadJson(obj) {
        var json = jQuery.parseJSON(hdn_Opciones.Get("hdn_Opciones")); //jQuery.parseJSON(document.getElementById("hdn_Opciones").value);
        var auxItem = "";
        $('#ddl_Opciones').children('option:not(:first)').remove();
        for (var i in json) {
            for (var j in json[i]) {
                if (parseInt(json[i]["idTipoOpcion"]) == obj) {
                    if (auxItem != json[i]["Descripcion"]) {
                        $('#ddl_Opciones')
                            .append($("<option></option>")
                            .attr("value", json[i]["idOpciones"])
                            .text(json[i]["Descripcion"]));
                    }
                    auxItem = json[i]["Descripcion"];
                }
            }
        }
    }

    function ChangeBatchEditorValue(s, e, obj) {
        ocultarDiv();
        $("#hdn_tipoOpcion").val($('#grid_DXEFL_DXEditor3_I').val())
        if (e.command == "ADDNEWROW") hdn_update.Set("hdn_update", 1);
        if (e.command == "STARTEDIT") {
            hdn_update.Set("hdn_update", 2);
        }
        if (e.command == "CANCELEDIT") hdn_update.Set("hdn_update", 0);

        //$("#hdn_tipoOpcion").val($('#grid_DXEFL_DXEditor3_I').val())
        //if (e.command == "ADDNEWROW") $("#hdn_update").val("1");
        //if (e.command == "STARTEDIT") {
        //    $("#hdn_update").val("2");
        //}
        //if (e.command == "CANCELEDIT") $("#hdn_update").val("0");
    }
    function passToFilter() {
        ocultarDiv();
        var txtFind = $("#txtPerfil").val();
        grid.AutoFilterByColumn("Perfil", txtFind);
    }
    function passToNew() {
        ocultarDiv();
        grid.AddNewRow();
        var txtFind = $("#txtPerfil").val();
        var passValue = setInterval(passLet, 333);
        function passLet() {
            if ($('#grid_DXEFL_DXEditor1_I')) {
                $('#grid_DXEFL_DXEditor1_I').val(txtFind); clearInterval(passValue);

            }
            if ($('#grid_DXEFL_DXEditor1_I').val() != "") setTimeout(clearInterval(passValue), 60000)
        }
    }
    function refreshFilter() {
        ocultarDiv();
        grid.AutoFilterByColumn("Perfil", "");
    }
    var lastCountry = null;

    function OnCmbChanged(myCmb, e, tipoCombo) {
        var json = jQuery.parseJSON(hdn_Opciones.Get("hdn_Opciones")); //jQuery.parseJSON(document.getElementById("hdn_Opciones").value);
        var modulo = hdn_modulo.Get("hdn_modulo"); //document.getElementById("hdn_modulo").value;
        var auxItem = "";
        //Se cargan TipoOpciones
        if (tipoCombo == 1) {
            cmbTipoOpcion.ClearItems();
            for (var i in json) {
                for (var j in json[i]) {
                    if (json[i]["Modulo"] == myCmb.GetValue()) {
                        if (auxItem != json[i]["TipoOpcion"]) {
                            cmbTipoOpcion.AddItem(json[i]["TipoOpcion"], json[i]["idTipoOpcion"]);
                        }
                        auxItem = json[i]["TipoOpcion"];
                    }
                }
            }
            cmbTipoOpcion.SetSelectedIndex(0);
            json = null;
            OnCmbChanged(cmbTipoOpcion, '', 2)
        }
        //Se cargan Opciones
        if (tipoCombo == 2) {
            cmbOpciones.ClearItems();
            for (var i in json) {
                for (var j in json[i]) {
                    if ((json[i]["idTipoOpcion"] == myCmb.GetValue()) && (cmbModulo.GetValue() == json[i]["Modulo"])) {
                        if (auxItem != json[i]["Opcion"]) {
                            cmbOpciones.AddItem(json[i]["Opcion"], json[i]["idOpcion"]);
                        }
                        auxItem = json[i]["Opcion"];
                    }
                }
            }
            cmbOpciones.SetSelectedIndex(0);
            json = null;
            OnCmbChanged(cmbOpciones, '', 3)
        }
        //Se cargan Permiso
        if (tipoCombo == 3) {
            var json = jQuery.parseJSON(hdn_permisos.Get("hdn_permisos")); //jQuery.parseJSON(document.getElementById("hdn_permisos").value);
            cmbPermisos.ClearItems();
            for (var i in json) {
                for (var j in json[i]) {

                    var a = json[i]["Opcion"];
                    var b = myCmb.GetValue();
                    var bb = myCmb.GetText();
                    var c = cmbModulo.GetValue;
                    var d = json[i]["Modulo"];

                    if ((json[i]["Opcion"] == myCmb.GetText()) && (cmbModulo.GetValue() == json[i]["Modulo"])) {
                        if (auxItem != json[i]["Permiso"]) {
                            cmbPermisos.AddItem(json[i]["Permiso"], json[i]["IdPermiso"]);
                        }
                        auxItem = json[i]["Permiso"];
                    }
                }
            }
            cmbPermisos.SetSelectedIndex(0);
            json = null;

            //OnCmbChanged(cmbPermisos, '', 4)
        }
        //Se cargan Permiso
        //if (tipoCombo == 4) {
        //    document.getElementById("dsModulo").value = cmbModulo.GetValue();//Set Modulo update/insert
        //    document.getElementById("dsTipoOpcion").value = cmbTipoOpcion.GetValue();//Set TipoOpcion update/insert
        //    document.getElementById("dsOpcion").value = cmbOpciones.GetValue();//Set Opcion update/insert
        //    document.getElementById("dsPermiso").value = cmbPermisos.GetValue();//Set Permiso update/insert
        //}
    }

    function OnContextMenuItemClick(sender, args) {
        if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
            args.processOnServer = true;
            args.usePostBack = true;
        }
    }
</script>

<div id="dvWarning1" runat="server" clientidmode="Static" style="display: none" class="alert alert-warning" role="alert">
      <h4>
           <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
      </h4>
</div>

<div id="dvFormulario" runat="server">  

<!-- Page-Title -->
<div class="row marginInicio">
    <div class="col-sm-12">
        <h4 class="page-title">Administrador</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Permisos</a>
            </li>
            <li class="active">Mantenedor Roles
            </li>
        </ol>
    </div>
</div>

<!-- Contenedor Tabs -->
<div class="row clearfix marginBottom">
    <div class="col-md-12 column">
        <div class="tabbable" id="tabs-empRelacionadas">
            <ul class="nav nav-tabs">
                <li class="active">
                    <asp:LinkButton ID="LinkButton10" runat="server" OnClientClick="return Dialogo();" OnClick="lbClientes_Click">Mantenedor Roles</asp:LinkButton>
                </li>
            </ul>
            <div class="tab-content">
                <br />
                <div id="dvSuccess" style="display: none" class="alert alert-success">
                    <h4>
                        <dx:ASPxLabel ID="lbSuccess" runat="server" ClientInstanceName="lbSuccess" ClientIDMode="Static" CssClass="alert alert-success" Text=""></dx:ASPxLabel>
                        <%--<asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>--%>
                    </h4>
                </div>

                <div id="dvWarning" style="display: none" class="alert alert-warning" role="alert">
                    <h4>
                        <dx:ASPxLabel ID="lbWarning" runat="server" ClientInstanceName="lbWarning" ClientIDMode="Static" CssClass="alert alert-warning" Text=""></dx:ASPxLabel>
                        <%--<asp:Label ID="lbWarning" runat="server" Text=" "></asp:Label>--%>
                    </h4>
                </div>

                <div id="dvError" style="display: none" class="alert alert-danger">
                    <h4>
                        <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                    </h4>
                </div>

                <div class="tab-pane active margenBottom" id="panel-tab3">
                        <div class="form-horizontal" role="form">
                            <!-- Grupo 1 -->
                            <!-- CABECERA ERG.22-09-2016 -->
                            <!-- filtros-->
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card-box">
                                        <div class="row">

                                            <div class="col-md-5 col-sm-6">
                                                <label for="">Perfil:</label>
                                                <div class="input-group">
                                                    <input type="text" id="txtPerfil" runat="server" class="form-control" maxlength="150" clientidmode="Static" />
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClientClick="passToFilter();return false;"><i class="fa fa-search"></i></asp:LinkButton>
                                                    </span>
                                                </div>
                                                <div class="input-group" style="float: right;">
                                                    <a class="dxbButton dxgvCommandColumnItem dxbButtonSys" data-args="[['Reload'],1]" id="reLoad" href="javascript:refreshFilter();"><span>Refrescar tabla</span></a>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">

                                            <div class="col-md-5 col-sm-6">
                                                <input type="button" id="btnNew" value=" Nuevo " style="font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif;" class="btn btn-default" onclick="javacript: passToNew();" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- END CABECERA ERG.22-09-2016 -->
                            <div class="col-sm-7" style="display: none;">
                                <asp:TextBox ID="txt_perfil" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                            </div>
                            <div class="form-group" style="display: none;">
                                <label for="inputEmail3" class="col-sm-5 control-label">Modulo:</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddl_modulo" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <label for="inputEmail3" class="col-sm-5 control-label">Permiso:</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddl_permiso" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <!-- Grupo 2 -->
                        <div class="col-md-6" style="width: 30%; display: none;">
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-5 control-label">Tipo Opcion:</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddl_tipoOpcion" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="inputEmail3" class="col-sm-5 control-label">Opciones:</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddl_Opciones" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <dx:ASPxHiddenField ID="hdn_Opciones" ClientInstanceName="hdn_Opciones" runat="server"></dx:ASPxHiddenField>
                                <%--<input type="hidden" id="hdn_Opciones" clientidmode="static" value="" runat="server" />--%>
                            </div>
                            <input type="button" onclick="loadJson()" value="TEST" />
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>
                                <div class="card-box">
                                    <table border="0" style="table-layout: fixed; width: 100%;">
                                        <tr>
                                            <td>
                                                <div class="table-responsive">
                                                    <div id="Div1" class="row clearfix">
                                                        <div class="col-md-12 column">
                                                                <div class="col-md-12 column">
                                                                    <dx:ASPxGridView ID="grid" ClientInstanceName="grid" class="dxgvControl_Metropolis" runat="server" KeyFieldName="IdCargoAccion"
                                                                        AutoGenerateColumns="False" Width="100%" SettingsBehavior-AllowDragDrop="false"
                                                                        OnCellEditorInitialize="grid_CellEditorInitialize" OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating"
                                                                        OnCustomCallback="grid_CustomCallback" ClientSideEvents-EndCallback="OnEndCallback" OnRowDeleting="grid_RowDeleting"
                                                                        OnContextMenuItemClick="grid_ContextMenuItemClick" 
                                                                        OnFillContextMenuItems="grid_FillContextMenuItems" >
                                                                        <ClientSideEvents BeginCallback="function(s, e) { ChangeBatchEditorValue(s, e, s.id); }" />
                                                                        <Columns>
                                                                            <dx:GridViewCommandColumn
                                                                                Name="btnAction"
                                                                                ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true" ShowCancelButton="false" VisibleIndex="1" Width="120"
                                                                                CellStyle-BackgroundImage-HorizontalPosition="center">
                                                                                <HeaderTemplate>
                                                                                    <dx:ASPxButton
                                                                                        AutoPostBack="false"
                                                                                        ID="btnNuevo"
                                                                                        RenderMode="Link"
                                                                                        runat="server"
                                                                                        Text="Nuevo">
                                                                                        <ClientSideEvents Click="function(s, e){nuevo(s, e);}" />
                                                                                    </dx:ASPxButton>
                                                                                </HeaderTemplate>
                                                                            </dx:GridViewCommandColumn>
                                                                            <dx:GridViewDataTextColumn Name="IdCargoAccion" FieldName="IdCargoAccion" UnboundType="Integer" VisibleIndex="1" Visible="false" Width="1" PropertiesTextEdit-ClientInstanceName="myIDPerfil" EditFormSettings-Visible="false"></dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Name="idPerfil" FieldName="idPerfil" UnboundType="Integer" VisibleIndex="2" Visible="false" PropertiesTextEdit-ClientInstanceName="myIDPerfil" EditFormSettings-Visible="false"></dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn FieldName="Perfil" VisibleIndex="3" Width="500" UnboundType="String" PropertiesTextEdit-ClientInstanceName="myPerfil">
                                                                                <EditFormSettings VisibleIndex="1" />
                                                                                <DataItemTemplate>
                                                                                    <%#Eval("Perfil")%>
                                                                                    <dx:ASPxImage ID="image1" CssClass="imgCell" runat="server"></dx:ASPxImage>
                                                                                </DataItemTemplate>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataComboBoxColumn Name="Modulo" FieldName="Modulo" VisibleIndex="4" UnboundType="String" Width="200" PropertiesComboBox-ClientInstanceName="cmbModulo">
                                                                                <EditFormSettings VisibleIndex="2" />
                                                                                <PropertiesComboBox EnableSynchronization="true">
                                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbChanged(s,e,1); }"></ClientSideEvents>
                                                                                </PropertiesComboBox>
                                                                            </dx:GridViewDataComboBoxColumn>
                                                                            <dx:GridViewDataComboBoxColumn Name="TipoOpcion" FieldName="Tipo Opcion" VisibleIndex="5" UnboundType="String" Width="200" PropertiesComboBox-ClientInstanceName="cmbTipoOpcion">
                                                                                <EditFormSettings VisibleIndex="3" />
                                                                                <PropertiesComboBox EnableSynchronization="true">
                                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbChanged(s,e,2); }"></ClientSideEvents>
                                                                                </PropertiesComboBox>
                                                                            </dx:GridViewDataComboBoxColumn>
                                                                            <dx:GridViewDataComboBoxColumn Name="Opcion" FieldName="Opciones" VisibleIndex="6" UnboundType="String" Width="200" PropertiesComboBox-ClientInstanceName="cmbOpciones">
                                                                                <EditFormSettings VisibleIndex="4" />
                                                                                <PropertiesComboBox EnableSynchronization="true">
                                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbChanged(s,e,3); }"></ClientSideEvents>
                                                                                </PropertiesComboBox>
                                                                            </dx:GridViewDataComboBoxColumn>
                                                                            <dx:GridViewDataComboBoxColumn Name="Permiso" FieldName="Permiso" VisibleIndex="7" UnboundType="String" PropertiesComboBox-ClientInstanceName="cmbPermisos">
                                                                                <EditFormSettings VisibleIndex="5" />
                                                                                <PropertiesComboBox EnableSynchronization="true">
                                                                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbChanged(s,e,4); }"></ClientSideEvents>
                                                                                </PropertiesComboBox>
                                                                            </dx:GridViewDataComboBoxColumn>
                                                                        </Columns>

                                                                        <SettingsCommandButton>
                                                                            <EditButton Text="Editar" ButtonType="Link"></EditButton>
                                                                            <DeleteButton Text="Borrar" ButtonType="Link"></DeleteButton>
                                                                        </SettingsCommandButton>

                                                                        <Styles>
                                                                            <RowHotTrack BackColor="#F5F5F5"></RowHotTrack>
                                                                        </Styles>

                                                                        <SettingsBehavior EnableRowHotTrack="true" />
                                                                        <SettingsEditing EditFormColumnCount="4" />

                                                                        <Settings ShowTitlePanel="true"
                                                                            ShowHeaderFilterButton="true"
                                                                            ShowFilterBar="Auto"
                                                                            AutoFilterCondition="Contains" />
                                                                        <SettingsCustomizationWindow PopupVerticalAlign="Below" />

                                                                       <SettingsCommandButton>
                                                                            <UpdateButton Text="Guardar">
                                                                            </UpdateButton>
                                                                            <CancelButton Text="Cancelar">
                                                                            </CancelButton>
                                                                        </SettingsCommandButton>
                                                                                                                                         
                                                                        <SettingsPager Position="TopAndBottom" AlwaysShowPager="true">
                                                                            <PageSizeItemSettings Items="10, 20, 50" />
                                                                        </SettingsPager>

                                                                        <SettingsContextMenu Enabled="true" RowMenuItemVisibility-NewRow="false" RowMenuItemVisibility-DeleteRow="false"
                                                                            RowMenuItemVisibility-EditRow="false">
                                                                            <RowMenuItemVisibility NewRow="False" EditRow="False" DeleteRow="False"></RowMenuItemVisibility>
                                                                        </SettingsContextMenu>
                                                                        <ClientSideEvents ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e); }" />
                                                                    </dx:ASPxGridView>

                                                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                                                                            TypeName="MultiGestion, $SharePoint.Project.AssemblyFullName$" >
                                                                    </asp:ObjectDataSource>

                                                                    <dx:ASPxHiddenField ID="hdn_update" ClientInstanceName="hdn_update" runat="server" ClientIDMode="Static"></dx:ASPxHiddenField>

                                                                    <asp:HiddenField ID="hdn_tipoOpcion" ClientIDMode="Static" Value="" runat="server" />
                                                                    
                                                                    <dx:ASPxHiddenField ID="hdn_modulo" ClientInstanceName="hdn_modulo" runat="server"></dx:ASPxHiddenField>

                                                                    <dx:ASPxHiddenField ID="hdn_permisos" ClientInstanceName="hdn_permisos" runat="server"></dx:ASPxHiddenField>

                                                                    <input type="hidden" id="dsModulo" clientidmode="static" value="" runat="server" />
                                                                    <input type="hidden" id="dsTipoOpcion" clientidmode="static" value="" runat="server" />
                                                                    <input type="hidden" id="dsOpcion" clientidmode="static" value="" runat="server" />
                                                                    <input type="hidden" id="dsPermiso" clientidmode="static" value="" runat="server" />
                                                                </div>
                                                        </div>
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
</div>


</div>
