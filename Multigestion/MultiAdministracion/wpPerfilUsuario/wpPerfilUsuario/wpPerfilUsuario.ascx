<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpPerfilUsuario.ascx.cs" Inherits="MultiAdministracion.wpPerfilUsuario.wpPerfilUsuario.wpPerfilUsuario" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

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
    document.onkeydown = function () {
        //109->'-' 
        //56->'(' 
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
                        //else {
                        //    document.activeElement.value.keyCode = 13;
                        //} //Realiza el enter. 
                    }
                }
            }
        }
    }

    function loadJson(obj) {
        var json = jQuery.parseJSON(document.getElementById("hdn_Opciones").value);
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


    //function myAjax() {
    //    $.ajax({
    //        type: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        data: "{}",
    //        dataFilter: function (data)//makes it work with 2.0 or 3.5 .net
    //        {
    //            var msg;
    //            if (typeof (JSON) !== 'undefined' &&
    //            typeof (JSON.parse) === 'function')
    //                msg = JSON.parse(data);
    //            else
    //                msg = eval('(' + data + ')');
    //            if (msg.hasOwnProperty('d'))
    //                return msg.d;
    //            else
    //                return msg;
    //        },
    //        url: "../perfiles.aspx/HelloWorld",
    //        success: function (msg) {
    //            alert(msg);
    //        }
    //    });
    //}


    //var imgCell = setInterval(reImgCell, 100);
    //function reImgCell() {
    //    if ($('.imgCell')) {
    //        $(".imgCell").each(function () {
    //            if (parseInt(hv) > 0) {
    //                $(this).attr("src", "~/Content/img/Clipboard_Copy.png");
    //                console.log($(this))
    //            } else {
    //                $(this).attr("src", "");
    //                console.log(hv)
    //            }

    //        });
    //        clearInterval(imgCell);
    //    }
    //    setTimeout(clearInterval(imgCell), 60000)
    //}

    //var passValue = setInterval(imgCell, 100);
    //function imgCell() {
    //    if ($('.imgCell')) {
    //        grid.CancelEdit();
    //        clearInterval(passValue);
    //    }
    //    setTimeout(clearInterval(passValue), 60000)
    //}



    function ChangeBatchEditorValue(s, e, obj) {
        $("#hdn_tipoOpcion").val($('#grid_DXEFL_DXEditor3_I').val())
        if (e.command == "ADDNEWROW") $("#hdn_update").val("1");
        if (e.command == "STARTEDIT") {
            $("#hdn_update").val("2");
        }
        if (e.command == "CANCELEDIT") $("#hdn_update").val("0");
        if (e.command == "UPDATEEDIT") {
            if ($("#hdn_update").val()=="1") callUser(document.getElementById("hdn_modulo").value)
        }
        //if (e.command == "UPDATEEDIT")
    }
    function callUser(userName) {
        $.ajax({
            type: 'POST',
            url: '../../_layouts/15/MultiAdministracion/WebMethods.aspx/getUserProfile',
            data: "{ 'userName': '" + userName + "'}",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (msg) {
                if (msg.d!="") alert(msg.d)
            }
            });
    }
    function passToFilter() {
        var txtFind = $("#txtPerfil").val();
        grid.AutoFilterByColumn("Usuario", txtFind);
    }
    function passToNew() {
        grid.AddNewRow();
    }
    function refreshFilter() {
        grid.AutoFilterByColumn("Usuario", "");
        //var passValue = setInterval(closeUpdate, 100);
        //function closeUpdate() {
        //    if ($('#grid_DXEFL_DXCBtn7')) { grid.CancelEdit(); clearInterval(passValue); }
        //    setTimeout(clearInterval(passValue), 60000)
        //}
    }
    function OnCmbChanged(myCmb, e, tipoCombo) {
        document.getElementById("hdn_modulo").value = myCmb.GetText();
    }

    function nuevo(s, e) {
        grid.AddNewRow();
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
            <li class="active">Asignacion Roles Usuarios
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
                    <asp:LinkButton ID="LinkButton10" runat="server">Asignacion Roles Usuarios</asp:LinkButton>
                </li>
            </ul>
            <div class="tab-content">
                <br />
                <div id="dvSuccess" runat="server" style="display: none" class="alert alert-success">

                    <h4>
                        <asp:Label ID="lbSuccess" runat="server" Text=""></asp:Label>
                    </h4>
                </div>

                <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
                    <h4>
                        <asp:Label ID="lbWarning" runat="server" Text=" "></asp:Label>
                    </h4>
                </div>

                <div id="dvError" runat="server" style="display: none" class="alert alert-danger">
                    <h4>
                        <asp:Label ID="lbError" runat="server" Text=""></asp:Label>
                    </h4>
                </div>

                <div class="tab-pane active margenBottom" id="panel-tab3">
                    <asp:Panel ID="pnFormulario" runat="server">
                        <div class="form-horizontal" role="form">
                            <!-- Grupo 1 -->
                            <!-- CABECERA ERG.22-09-2016 -->
                            <!-- filtros-->
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card-box">
                                        <div class="row">

                                            <div class="col-md-5 col-sm-6">
                                                <label for="">Buscar Usuario:</label>
                                                <div class="input-group">
                                                    <input type="text" id="txtPerfil" runat="server" class="form-control" maxlength="150" clientidmode="Static" />
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClientClick="return true;" OnClick="btnSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
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
                                <asp:TextBox ID="txt_perfil" runat="server" class="form-control" MaxLength="150"></asp:TextBox>
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
                                <input type="hidden" id="hdn_Opciones" clientidmode="static" value="" runat="server" />
                            </div>
                            <input type="button" onclick="loadJson()" value="TEST" />
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="card-box">
                                    <div class="table-responsive">
                                        <div id="myDevExpress" runat="server" clientidmode="Static" class="row clearfix">
                                            <div class="col-md-12 column">
                                                <!-- NEW TGHE NEWS-->
                                                <dx:ASPxGridView ID="grid" ClientInstanceName="grid" class="dxgvControl_Metropolis" runat="server" KeyFieldName="idUsuarioPerfil"
                                                    AutoGenerateColumns="False" Width="100%"
                                                    OnCellEditorInitialize="grid_CellEditorInitialize"
                                                    DataSourceID="SqlDataSourcePerfiles"
                                                    OnCustomErrorText="grid_CustomErrorText"
                                                    EnableRowsCache="true"
                                                    EnableViewState="true">
                                                    <ClientSideEvents BeginCallback="function(s, e) { ChangeBatchEditorValue(s, e, s.id); }" />
                                                    <Columns>
                                                        <dx:GridViewCommandColumn
                                                            Name="btnAction"
                                                            ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true" VisibleIndex="1" Width="70"
                                                            CellStyle-BackgroundImage-HorizontalPosition="center"
                                                            ShowCancelButton="true">
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

                                                        <dx:GridViewDataTextColumn Name="idUsuarioPerfil" FieldName="idUsuarioPerfil" UnboundType="Integer" VisibleIndex="1" Visible="false" Width="1" PropertiesTextEdit-ClientInstanceName="myIDPerfil" EditFormSettings-Visible="false"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Name="ID" FieldName="ID" UnboundType="Integer" VisibleIndex="1" Visible="false" Width="1" PropertiesTextEdit-ClientInstanceName="myIDPerfil" EditFormSettings-Visible="false"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Name="RUT" FieldName="RUT" UnboundType="String" VisibleIndex="2" Visible="false" PropertiesTextEdit-ClientInstanceName="myIDPerfil" EditFormSettings-Visible="false"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataComboBoxColumn Name="Usuario" FieldName="Usuario" VisibleIndex="3" UnboundType="String" Width="200" PropertiesComboBox-ClientInstanceName="cmbUsuario">
                                                            <EditFormSettings VisibleIndex="2" />
                                                            <PropertiesComboBox EnableSynchronization="true">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCmbChanged(s,e,1); }"></ClientSideEvents>
                                                            </PropertiesComboBox>
                                                        </dx:GridViewDataComboBoxColumn>
                                                        <dx:GridViewDataComboBoxColumn Name="Perfil" FieldName="Perfil" VisibleIndex="4" UnboundType="String" Width="200" PropertiesComboBox-ClientInstanceName="cmbPerfil">
                                                            <EditFormSettings VisibleIndex="3" />
                                                        </dx:GridViewDataComboBoxColumn>
                                                    </Columns>
                                                    <Styles>
                                                        <RowHotTrack BackColor="#F5F5F5"></RowHotTrack>
                                                    </Styles>
                                                    <SettingsBehavior EnableRowHotTrack="true" />
                                                    <SettingsEditing EditFormColumnCount="4" />
                                                    <Settings ShowTitlePanel="true" />
                                                    <SettingsCustomizationWindow PopupVerticalAlign="Below" />
                                                    <SettingsCommandButton>
                                                        <UpdateButton Text="Guardar"></UpdateButton>
                                                    </SettingsCommandButton>
                                                    <SettingsPager Position="TopAndBottom" AlwaysShowPager="true">
                                                        <PageSizeItemSettings Items="10, 20, 50" />
                                                    </SettingsPager>

                                                    <SettingsCommandButton>
                                                        <EditButton Text="Editar" ButtonType="Link"></EditButton>
                                                        <DeleteButton Text="Borrar" ButtonType="Link"></DeleteButton>
                                                    </SettingsCommandButton>
                                                </dx:ASPxGridView>

                                                <asp:SqlDataSource
                                                    ID="SqlDataSourcePerfiles"
                                                    runat="server"
                                                    ConnectionString="<%$ ConnectionStrings:DACConnectionString %>"
                                                    SelectCommand=" select * from dbo.view_perfilUsuario order by 4,5 " SelectCommandType="Text"
                                                    InsertCommand="[dbo].[GuardarPerfilUsuario]" InsertCommandType="StoredProcedure"
                                                    UpdateCommand="[dbo].[GuardarPerfilUsuario]" UpdateCommandType="StoredProcedure"
                                                    DeleteCommand="[dbo].[GuardarPerfilUsuario]" DeleteCommandType="StoredProcedure">
                                                    <InsertParameters>
                                                        <asp:Parameter Name="idUsuarioPerfil" Type="Int32" />
                                                        <asp:Parameter Name="ID" Type="Int32" />
                                                        <asp:Parameter Name="RUT" Type="Char" />
                                                        <asp:Parameter Name="Usuario" Type="Int32" />
                                                        <asp:Parameter Name="Perfil" Type="Int32" />
                                                        <asp:Parameter Name="Crud" Type="Int32" DefaultValue="1" />
                                                    </InsertParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="idUsuarioPerfil" Type="Int32" />
                                                        <asp:Parameter Name="ID" Type="Int32" />
                                                        <asp:Parameter Name="RUT" Type="Char" />
                                                        <asp:Parameter Name="Usuario" Type="Int32" />
                                                        <asp:Parameter Name="Perfil" Type="Int32" />
                                                        <asp:Parameter Name="Crud" Type="Int32" DefaultValue="3" />
                                                    </UpdateParameters>
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="idUsuarioPerfil" Type="Int32" />
                                                        <asp:Parameter Name="ID" Type="Int32" DefaultValue="1" />
                                                        <asp:Parameter Name="RUT" Type="Char" DefaultValue="" />
                                                        <asp:Parameter Name="Usuario" Type="Int32" DefaultValue="1" />
                                                        <asp:Parameter Name="Perfil" Type="Int32" DefaultValue="1" />
                                                        <asp:Parameter Name="Crud" Type="Int32" DefaultValue="4" />
                                                    </DeleteParameters>
                                                </asp:SqlDataSource>

                                                <asp:HiddenField ID="hdn_update" ClientIDMode="Static" Value="0" runat="server" />
                                                <asp:HiddenField ID="hdn_tipoOpcion" ClientIDMode="Static" Value="" runat="server" />
                                                <input type="hidden" id="Hidden1" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="hdn_modulo" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="hdn_usuario" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="hdn_permisos" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="dsModulo" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="dsTipoOpcion" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="dsOpcion" clientidmode="static" value="" runat="server" />
                                                <input type="hidden" id="dsPermiso" clientidmode="static" value="" runat="server" />
                                            </div>
                                            <!-- END NEW THE NEWS-->
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <!-- fin  -->
        </div>
    </div>
</div>


</div>

