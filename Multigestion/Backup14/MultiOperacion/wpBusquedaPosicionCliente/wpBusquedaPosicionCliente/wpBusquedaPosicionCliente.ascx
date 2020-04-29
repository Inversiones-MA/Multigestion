<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpBusquedaPosicionCliente.ascx.cs" Inherits="MultiOperacion.wpBusquedaPosicionCliente.wpBusquedaPosicionCliente.wpBusquedaPosicionCliente" %>


<style type="text/css">
    .btn { font:14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif !important; }
</style>
<script type="text/javascript">
    function setFocus(obj) {
        document.getElementById(obj).focus();
    }

    function setFocusEnter(obj, event) {

        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        if (keyCode == 13) {
            document.getElementById(obj).focus();
            return false;
        }
        else {
            return true;
        }

    }

    document.onkeydown = function () {
        //109->'-' 
        //56->'(' 
        if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
            window.event.keyCode = 505;
        }

        if (window.event.keyCode == 505) {
            return false;
        }

        //if (window.event && (window.event.keyCode == 8)) {
        //    valor = document.activeElement.value;
        //    if (valor == undefined) { return false; } //Evita Back en página. 

        //    else {
        //        if (document.activeElement.getAttribute('type') == 'select-one')
        //        { return false; } //Evita Back en select. 
        //        if (document.activeElement.getAttribute('type') == 'button')
        //        { return false; } //Evita Back en button. 
        //        if (document.activeElement.getAttribute('type') == 'radio')
        //        { return false; } //Evita Back en radio. 
        //        if (document.activeElement.getAttribute('type') == 'checkbox')
        //        { return false; } //Evita Back en checkbox. 
        //        if (document.activeElement.getAttribute('type') == 'file')
        //        { return false; } //Evita Back en file. 
        //        if (document.activeElement.getAttribute('type') == 'reset')
        //        { return false; } //Evita Back en reset. 
        //        if (document.activeElement.getAttribute('type') == 'submit')
        //        { return false; } //Evita Back en submit. 
        //        if (document.activeElement.getAttribute('type') == 'input')
        //        { return false; } //Evita Back en inout. 
        //        else //Text, textarea o password 
        //        {
        //            if (document.activeElement.value.length == 0)
        //            { return false; } //No realiza el backspace(largo igual a 0). 
        //            else {
        //                document.activeElement.value.keyCode = 8;
        //            } //Realiza el backspace. 
        //        }
        //    }

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
                    if (document.activeElement.getAttribute('type') == 'text')
                    {
                        document.getElementById('<%=btnBuscar.ClientID %>').click();
                        //alert('aaa');
                    }
                }
            }
        //}
    }




    //document.onkeydown = function () {
    //    //109->'-' 
    //    //56->'(' 
    //    if (window.event && (window.event.keyCode == 109 || window.event.keyCode == 56)) {
    //        window.event.keyCode = 505;
    //    }

    //    if (window.event.keyCode == 505) {
    //        return false;
    //    }

    //    if (window.event && (window.event.keyCode == 8)) {
    //        valor = document.activeElement.value;
    //        if (valor == undefined) { return false; } //Evita Back en página. 

    //        else {
    //            if (document.activeElement.getAttribute('type') == 'select-one')
    //            { return false; } //Evita Back en select. 
    //            if (document.activeElement.getAttribute('type') == 'button')
    //            { return false; } //Evita Back en button. 
    //            if (document.activeElement.getAttribute('type') == 'radio')
    //            { return false; } //Evita Back en radio. 
    //            if (document.activeElement.getAttribute('type') == 'checkbox')
    //            { return false; } //Evita Back en checkbox. 
    //            if (document.activeElement.getAttribute('type') == 'file')
    //            { return false; } //Evita Back en file. 
    //            if (document.activeElement.getAttribute('type') == 'reset')
    //            { return false; } //Evita Back en reset. 
    //            if (document.activeElement.getAttribute('type') == 'submit')
    //            { return false; } //Evita Back en submit. 
    //            if (document.activeElement.getAttribute('type') == 'input')
    //            { return false; } //Evita Back en inout. 
    //            else //Text, textarea o password 
    //            {
    //                if (document.activeElement.value.length == 0)
    //                { return false; } //No realiza el backspace(largo igual a 0). 
    //                else {
    //                    document.activeElement.value.keyCode = 8;
    //                } //Realiza el backspace. 
    //            }
    //        }

    //        if (window.event && (window.event.keyCode == 13)) {
    //            valor = document.activeElement.value;
    //            if (valor == undefined) { return false; } //Evita Enter en página. 
    //            else {
    //                if (document.activeElement.getAttribute('type') == 'select-one')
    //                { return false; } //Evita Enter en select. 
    //                if (document.activeElement.getAttribute('type') == 'button')
    //                { return false; } //Evita Enter en button. 
    //                if (document.activeElement.getAttribute('type') == 'radio')
    //                { return false; } //Evita Enter en radio. 
    //                if (document.activeElement.getAttribute('type') == 'checkbox')
    //                { return false; } //Evita Enter en checkbox. 
    //                if (document.activeElement.getAttribute('type') == 'file')
    //                { return false; } //Evita Enter en file. 
    //                if (document.activeElement.getAttribute('type') == 'reset')
    //                { return false; } //Evita Enter en reset. 
    //                if (document.activeElement.getAttribute('type') == 'submit')
    //                { return false; } //Evita Enter en submit. 
    //                if (document.activeElement.getAttribute('type') == 'input')
    //                { return false; } //Evita Back en input. 
    //                else //Text, textarea o password 
    //                {
    //                    if (document.activeElement.value.length == 0)
    //                    { return false; } //No realiza el backspace(largo igual a 0). 
    //                    //else {
    //                    //    document.activeElement.value.keyCode = 13;
    //                    //} //Realiza el enter. 
    //                }
    //            }
    //        }
    //    }
    //}
</script>


<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
       <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
   </h4>
</div>

<div id="dvFormulario" runat="server">

    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Posición Cliente</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Inicio</a>
                </li>
                <li class="active">Búsqueda Posición Cliente
                </li>
            </ol>
        </div>
    </div>

    <br />
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
    <br />

    <div class="row">
        <div class="col-md-12">
            <div class="card-box">
                <div class="row">

                    <div class="col-md-3 col-sm-6">
                        <label for="">Razón Social:</label>
                        <asp:TextBox ID="txtRazonSocial" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label for="">Rut:</label>
                        <asp:TextBox ID="txtRUT" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label>Ejecutivo:</label>
                        <asp:DropDownList ID="ddlEjecutivo" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-3 col-sm-6">
                        <label style="color: #FFF;">.</label><br>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Style="margin-left: 0px;" CssClass="btn btn-primary btn-success" OnClick="btnBuscar_Click" OnClientClick="return Dialogo();" />&nbsp;
                       
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="Div1" runat="server" class="row clearfix">
        <div class="row">
            <div class="col-md-12">
                <div class="card-box">
                    <div class="table-responsive">
                        <asp:GridView ID="gridResultado" CssClass="table table-bordered table-hover" runat="server" GridLines="Horizontal"
                            class="table table-responsive table-hover table-bordered"
                            AllowPaging="True" RowHeaderColumn="0"
                            PageSize="20"
                            ShowHeaderWhenEmpty="True" OnPageIndexChanging="gridResultado_PageIndexChanging"
                            OnRowCreated="gridResultado_RowCreated"
                            OnRowDataBound="gridResultado_RowDataBound">
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>

