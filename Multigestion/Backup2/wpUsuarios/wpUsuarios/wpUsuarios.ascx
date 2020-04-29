<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpUsuarios.ascx.cs" Inherits="MultiAdministracion.wpUsuarios.wpUsuarios.wpUsuarios" %>

<link rel="stylesheet" href="../../_layouts/15/PagerStyle.css" />
<style type="text/css">
body {
}
.dxgvTitlePanel {display:none;}
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
        //if (e.command == "UPDATEEDIT")
    }
    function passToFilter() {
        var txtFind = $("#txtPerfil").val();
        grid.AutoFilterByColumn("Perfil", txtFind);
    }
    function passToNew() {
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
        grid.AutoFilterByColumn("Perfil", "");
    }
    var lastCountry = null;

    function OnCmbChanged(myCmb, e, tipoCombo) {
        var json = jQuery.parseJSON(document.getElementById("hdn_Opciones").value);
        var modulo = document.getElementById("hdn_modulo").value;
        var auxItem = "";
        //Se cargan TipoOpciones
        if (tipoCombo == 1) {
            cmbTipoOpcion.ClearItems();
            for (var i in json) {
                for (var j in json[i]) {
                    if (json[i]["Modulo"] == myCmb.GetValue()) {
                        if (auxItem != json[i]["TipoOpcion"]) {
                            cmbTipoOpcion.AddItem(json[i]["TipoOpcion"], json[i]["TipoOpcion"]);
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
                    if ((json[i]["TipoOpcion"] == myCmb.GetValue()) && (cmbModulo.GetValue() == json[i]["Modulo"])) {
                        if (auxItem != json[i]["Opcion"]) {
                            cmbOpciones.AddItem(json[i]["Opcion"], json[i]["Opcion"]);
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
            var json = jQuery.parseJSON(document.getElementById("hdn_permisos").value);
            cmbPermisos.ClearItems();
            for (var i in json) {
                for (var j in json[i]) {
                    if ((json[i]["Opcion"] == myCmb.GetValue()) && (cmbModulo.GetValue() == json[i]["Modulo"])) {
                        if (auxItem != json[i]["Permiso"]) {
                            cmbPermisos.AddItem(json[i]["Permiso"], json[i]["Permiso"]);
                        }
                        auxItem = json[i]["Permiso"];
                    }
                }
            }
            cmbPermisos.SetSelectedIndex(0);
            json = null;

            OnCmbChanged(cmbPermisos, '', 4)
        }
        //Se cargan Permiso
        if (tipoCombo == 4) {
            document.getElementById("dsModulo").value = cmbModulo.GetValue();//Set Modulo update/insert
            document.getElementById("dsTipoOpcion").value = cmbTipoOpcion.GetValue();//Set TipoOpcion update/insert
            document.getElementById("dsOpcion").value = cmbOpciones.GetValue();//Set Opcion update/insert
            document.getElementById("dsPermiso").value = cmbPermisos.GetValue();//Set Permiso update/insert
        }
    }
    function meEdit(e, crud) {
        var myVal = $(e).attr('text');
        document.getElementById("hdn_editar").value = myVal;
        if (crud == 3) $("#btn_editar").click()
        if (crud == 4) $("#btn_eliminar").click()
    }
    function validaForm() {
        var msg = "";
        if (document.getElementById("txt_nombre").value == "") msg += "-Ingrese Nombre.\n";
        if (document.getElementById("txt_rut").value == "") msg += "-Ingrese rut.\n";
        if (!VerificaRut(document.getElementById("txt_rut").value)) msg += " Ingrese Rut Valido.\n";
        if (document.getElementById("ddl_departamento").value == "-1") msg += "-Seleccione Departamento.\n";
        if ((document.getElementById("ddl_cargo").value).indexOf('Seleccione') > -1) msg += "-Seleccione Cargo.\n";
        if (document.getElementById("ddl_banco").value == "-1") msg += "-Seleccione Banco.\n";
        if (document.getElementById("txt_numeroCuenta").value == "") msg += "-Ingrese N° Cuenta.\n";
        if (msg != "") { alert(msg); } else { return true }
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
            <li class="active">Mantenedor Usuarios
            </li>
        </ol>
    </div>
</div>

<div class="row">
    <div class="col-md-12">
        <div class="tab-content">
            <div class="tab-pane active">
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
                <!-- CABECERA ERG.22-09-2016 -->
                <!-- filtros-->
                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <div class="row">
                                <asp:Panel ID="pnFormulario" runat="server">
                                    <div class="form-horizontal" role="form">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Nombre:</label>
                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txt_nombre" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <span class="input-group-btn">
                                                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-default" OnClick="btnSearch_Click" OnClientClick="if($('#txt_nombre').val().trim()==''){$('#txt_nombre').val(''); return false;} return Dialogo();"><i class="fa fa-search"></i></asp:LinkButton>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Rut:</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txt_rut" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Habilitado:</label>
                                                <div class="col-sm-8">
                                                    <asp:CheckBox ID="chk_Habilitado" runat="server" ClientIDMode="Static" />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Grupo 2 -->
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Departamento:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="ddl_departamento" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Perfil:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="ddl_cargo" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Es Responsable:</label>
                                                <div class="col-sm-8">
                                                    <asp:CheckBox ID="chkEsResponsable" runat="server" ClientIDMode="Static" />
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Grupo 3 -->
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">Banco:</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="ddl_banco" DataTextField="Banco" DataValueField="ID" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    <%--DataSourceID="SqlDataSourceBancos"--%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-4 control-label">N° Cuenta:</label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txt_numeroCuenta" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- fin  -->

                <!-- tabla / grilla -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="card-box">
                            <div class="row">
                                <table style="width:100%">
                                    <tr>
                                         <td>

                                            <asp:Button ID="btnBuscar" runat="server" Text="Cancelar" OnClick="btnBuscar_Click" Style="display: none;" CssClass="btn btn-primary btn-success pull-right" OnClientClick="return Dialogo();" />

                                            <%--<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />--%>

                                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" class="btn btn-warning pull-right" OnClientClick="return Dialogo();" />

                                            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" OnClientClick="if(validaForm()){return Dialogo();}else{return false;}" />
                                            <asp:Button ID="btn_editar" ClientIDMode="Static" runat="server" CommandArgument="nothing" OnClick="btn_editar_Click" class="btn btn-warning pull-right" Style="display: none;" />
                                            <asp:Button ID="btn_eliminar" ClientIDMode="Static" runat="server" CommandArgument="nothing" OnClick="btn_eliminar_Click" class="btn btn-warning pull-right" Style="display: none;" />
                                            <input type="hidden" id="hdn_editar" clientidmode="static" value="0" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" style="table-layout: fixed; width: 100%;">
                                    <tr>
                                        <td>
                                            <div class="table-responsive">
                                                <div id="dv_buscarEmpresas" runat="server" class="row clearfix">
                                                    <div class="col-md-12 column">
                                                        <br />
                                                        <asp:HiddenField runat="server" ID="hdnIdEmpresaClientes" />
                                                        <asp:GridView ID="ResultadosBusqueda" runat="server" class="table table-bordered table-hover"
                                                            GridLines="Horizontal"
                                                            PageSize="10"
                                                            AllowPaging="true"
                                                            AutoGenerateColumns="false"
                                                            ShowHeaderWhenEmpty="True"
                                                            OnPageIndexChanging="ResultadosBusqueda_PageIndexChanging">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>ID</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("ID") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>Nombre</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("Nombre") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>RUT</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("rut") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>Departamento</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("Departamento") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>Perfil</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("Perfil") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>Banco</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("Banco") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>N° Cuenta</HeaderTemplate>
                                                                    <ItemTemplate><%# Eval("NCuenta") %></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>Accion</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <a id="A1" runat="server" text='<%# Eval("ID") %>' onclick="meEdit(this,3)" style="cursor: pointer;">Editar</a>
                                                                        <a id="A2" runat="server" text='<%# Eval("ID") %>' onclick="meEdit(this,4)" style="cursor: pointer;">Eliminar</a>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                        </asp:GridView>
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
                <!-- fin  -->
            </div>
        </div>
    </div>
</div>

</div>



