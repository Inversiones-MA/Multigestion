<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpEdicionEmpresa.ascx.cs" Inherits="MultiComercial.wpEdicionEmpresa.wpEdicionEmpresa.wpEdicionEmpresa" %>

<style type="text/css">
    .form-group label {
        text-align:right;
        padding-top:10px;
    }
</style>

<script src="../../_layouts/15/MultiComercial/Validaciones.js"></script>
<script src="../../_layouts/15/MultiComercial/FuncionesClientes.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>

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

    function HabilitarEmpresa(cad) {

        var idPanel = cad + 'pnlEdit';
      
        var idGuardar = cad + 'btnGuardar';
        var idLimpiar = cad + 'btnLimpiar';

        document.getElementById(idGuardar).style.display = 'block';
        document.getElementById(idLimpiar).style.display = 'block';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("*");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].disabled = false;
        };
        $('#txtFechaFinContrato').prop("disabled", true);
        $('#txtSbif').prop("disabled", true);
        $('#txtPaf').prop("disabled", true);

        bloquear();
        return false;
    }

    function LimpiarVacido(cad) {

        var idPanel = cad + 'pnlEdit';

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("input");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].value = "";
        };

        var div_to_disable = document.getElementById(idPanel).getElementsByTagName("select");
        var children = div_to_disable;
        for (var i = 0; i < children.length; i++) {
            children[i].selectedIndex = 0;
        };

        return false;
    }

    function bloquear() {

        var area = document.getElementById("<%=hfArea.ClientID%>");
        if (area.value == 'Jefe Operaciones' || area.value == 'Administrador' || area.value == 'Analista Operaciones') {
            var div1 = document.getElementById('<%=pnlContrato.ClientID %>').getElementsByTagName("*");
            var children1 = div1;
            for (var i = 0; i < children1.length; i++) {
                children1[i].visible = true;
                children1[i].disabled = false;
            };

        }
        else {
            var div1 = document.getElementById('<%=pnlContrato.ClientID %>').getElementsByTagName("*"); //document.getElementById(pnlcon).getElementsByTagName("*");
            var children1 = div1;
            for (var i = 0; i < children1.length; i++) {
                children1[i].style.display = 'none';
            };
        }
            
    }

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");
        document.getElementById('<%=txtFechaFinContrato.ClientID %>').disabled = true;
        document.getElementById('<%=txtSbif.ClientID %>').disabled = true;
        document.getElementById('<%=txtPaf.ClientID %>').disabled = true;
        document.getElementById('<%=ddlBloqueado.ClientID %>').disabled = true;
        document.getElementById('<%=ddlMotivoBloqueo.ClientID %>').disabled = true;
        document.getElementById('<%=ddlPerteneceGrupo.ClientID %>').disabled = true;
        document.getElementById('<%=ddlGrupoEcono.ClientID %>').disabled = true;
        bloquear();
    });

    function calcularFecha() {
        var fechaInicio = document.getElementById('<%=dtcFechaInicioContrato.Controls[0].ClientID %>');
        if ((fechaInicio != "" && fechaInicio != null && fechaInicio.text != "&nbsp;"))
        {
            var date1_ms = null;
            //var date2_ms = null;
            var fFecha1 = null;
            var aFecha1 = null;
            var toType = ({}).toString.call(fechaInicio.value).match(/\s([a-z|A-Z]+)/)[1].toLowerCase()
            if (toType == 'string') {
                fechaInicio = fechaInicio.value.split(' ');
                    aFecha1 = fechaInicio[0].split('-');
                if (aFecha1.length < 2)
                    aFecha1 = fechaInicio[0].split('/');

                fFecha1 = new Date(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);

                fFecha1.setFullYear(fFecha1.getFullYear() + 1);
                //alert(fFecha1);
            }
             
            if (fFecha1 != null) {
                var d = fFecha1,
                month = '' + (d.getMonth() + 1),
                day = '' + d.getDate(),
                year = d.getFullYear();

                if (month.length < 2) month = '0' + month;
                if (day.length < 2) day = '0' + day; {

                    $('#txtFechaFinContrato').prop("disabled", false);
                    document.getElementById('<%=txtFechaFinContrato.ClientID %>').value = [day, month, year].join('-');
                    document.getElementById('<%=txtFechaFinContrato.ClientID %>').innerHTML = [day, month, year].join('-');
                    document.getElementById('<%=hfFecha.ClientID %>').value = [day, month, year].join('-');
                    document.getElementById('<%=hfFecha.ClientID %>').innerHTML = [day, month, year].join('-');
                    $('#txtFechaFinContrato').prop("disabled", true);
                }
            }
        }
    }

</script>

 <div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>

<div id="dvFormulario" runat="server">
    <asp:HiddenField ID="hfFecha" runat="server" ClientIDMode="Static" />
    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Comercial</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Datos Empresa</a>
                </li>
                <li class="active">Edici&oacute;n de Empresa
         </li>
            </ol>
        </div>
    </div>

    <!-- Empresa -->
    <div class="row">
        <div class="col-md-12">
            <div class="alert alert-gray">
                <p>
                    <strong>Empresa: 
              
                        <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label>
                    </strong>
                    / 
           
                    <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                    <%--/ Negocio: 
            <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>--%>
            / Ejecutivo: 
           
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <!-- Contenedor Tabs -->
    <div class="row">
        <div class="col-md-12 column">
            <div class="tabbable" id="tabs-empRelacionadas">
                <ul class="nav nav-tabs navtab-bg nav-justified">
                    <li class="active">
                        <a href="#tab1" data-toggle="tab" aria-expanded="true">
                            <span class="visible-xs"><i class="fa fa-home"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbDatosEmpresa" runat="server" OnClick="lbDatosEmpresa_Click" OnClientClick="return Dialogo();">Datos Empresa</asp:LinkButton>
                            </span>
                        </a>
                    </li>
                    <li class="">
                        <a href="#tab2" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbHistoria" runat="server" OnClientClick="return Dialogo();" OnClick="lbHistoria_Click">Historia Empresa</asp:LinkButton>
                            </span>
                        </a>
                    </li>
                    <li class="">
                        <a href="#tab3" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbDocumentos" runat="server" OnClientClick="return Dialogo();" OnClick="lbDocumentos_Click">Documentos</asp:LinkButton>
                            </span>
                        </a>
                    </li>
                    <li class="">
                        <a href="#tab4" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbEmpresaRelacionada" runat="server" OnClientClick="return Dialogo();" OnClick="lbEmpresaRelacionada_Click">Empresas Relacionadas</asp:LinkButton>
                            </span>
                        </a>
                    </li>
                    <li class="">
                        <a href="#tab5" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbDireccion" runat="server" OnClientClick="return Dialogo();" OnClick="lbDireccion_Click">Direcciones</asp:LinkButton>
                            </span>
                        </a>
                    </li>
                    <li class="">
                        <a href="#tab6" data-toggle="tab" aria-expanded="false">
                            <span class="visible-xs"><i class="fa fa-user"></i></span>
                            <span class="hidden-xs">
                                <asp:LinkButton ID="lbPersonas" runat="server" OnClientClick="return Dialogo();" OnClick="lbPersonas_Click">Personas</asp:LinkButton>
                            </span>
                        </a>
                    </li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active" id="tab1">

                        <div id="dvExito" runat="server" style="display: none" class="alert alert-success" role="alert">
                            <h4>
                                <asp:Label ID="lbExito" runat="server"></asp:Label>
                            </h4>
                        </div>
                        <div id="dvInfo" runat="server" style="display: none" class="alert alert-info" role="alert">
                            <h4>
                                <asp:Label ID="lbInfo" runat="server"></asp:Label>
                            </h4>
                        </div>
                        <div id="dvWarning" runat="server" style="display: none" class="alert alert-warning" role="alert">
                            <h4>
                                <asp:Label ID="lbWarning" runat="server"></asp:Label>
                            </h4>
                        </div>
                        <div id="dvError" style="display: none" class="alert alert-danger" role="alert" runat="server">
                            <h4>
                                <asp:Label ID="lbError" runat="server"></asp:Label>
                            </h4>
                        </div>
                        <div id="dvCustom" class="alert alert-danger col-md-12" role="alert" style="display: none;" runat="server">
                            <asp:Label ID="lblMsgVal" runat="server"></asp:Label>
                        </div>
                        <br />
                        <asp:LinkButton ID="lbEdit" runat="server" CssClass="fa fa-edit paddingIconos text-muted pull-right" ToolTip="Editar" Text="Editar"></asp:LinkButton>
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <div class="card-box">
                                    <asp:Panel runat="server" ID="pnlEdit">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Rut</label>
                                                    <div class="col-sm-7">
                                                        <asp:Label runat="server" ID="lblRut" MaxLength="10" CssClass="form-control" Style="float: left; width: 80%;" ></asp:Label>
                                                        <asp:Label runat="server" ID="lblDv" MaxLength="1" CssClass="form-control" Style="text-align: center; width: 17%; margin-left: 2%; float: right;"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">N° Empleados</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtEmpleados" runat="server" MaxLength="5" CssClass="form-control" onKeyPress="return solonumeros(event)" validationMessage="*" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Nombre Empresa</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtNombreEmpresa" runat="server" MaxLength="200" CssClass="form-control" />
                                                        <asp:TextBox ID="txtNombreAntiguo" Visible="false" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Actividad</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlActividad" runat="server" CssClass="form-control" />
                                                    </div>
                                                </div>                                                     
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Fecha Inicio Actividad</label>
                                                    <div class="col-sm-7">
                                                        <SharePoint:DateTimeControl ID="dtcIniciActEconomica" runat="server" LocaleId="13322" Calendar="Gregorian" DateOnly="true" CssClassTextBox="form-control" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Fecha Inicio Contrato</label>
                                                    <div id="FechaInicio" class="col-sm-7">
                                                        <SharePoint:DateTimeControl ID="dtcFechaInicioContrato" runat="server" LocaleId="13322" Calendar="Gregorian" DateOnly="true" CssClassTextBox="form-control" ClientIDMode="Static"/>
                                                        <%--OnDateChanged="dtcFechaInicioContrato_DateChanged"--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group row" style="display: none">
                                                    <label class="col-sm-5 control-label">Razón Social</label>
                                                    <div class="col-sm-7">
                                                        <asp:Label runat="server" ID="lblRazon" CssClass="form-control"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Tipo Empresa</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlTipoE" runat="server" CssClass="form-control" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Tel. Fijo</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtFijo" runat="server" CssClass="form-control" onKeyPress="return solonumeros(event)" MaxLength="9" />
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Email Corp</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtMailC" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Ejecutivo</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList runat="server" ID="ddlEjecutivo" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <%--<asp:TextBox ID="txtEjecutivo" runat="server" CssClass="form-control input-sm col-md-5"></asp:TextBox>--%>
                                                    </div>
                                                </div>

                                                <!-- CLASIFICACION SBIF 02-03-2017 DEL ULTIMO SCORING Clasificación SBIF / PAF-->
                                                <div class="form-group row">
                                                    <label for="inputPassword3" class="col-sm-5 control-label">Clasificación PAF</label>
                                                    <div class="col-sm-7">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td style="width: 100%; ">
                                                                    <asp:TextBox ID="txtSbif" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                                </td>
                                                                <td>&nbsp;&nbsp;&nbsp;</td>
                                                                <td style="width: 0%; display:none">
                                                                    <asp:TextBox ID="txtPaf" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>

                                                 <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Fecha Fin Contrato</label>
                                                    <div class="col-sm-7">
                                                        <%--<SharePoint:DateTimeControl ID="dtcFechaFinContrato" runat="server" LocaleId="13322" Calendar="Gregorian" DateOnly="true" CssClassTextBox="form-control" ClientIDMode="Static" EnableViewState="true" ReadOnly="true"/>--%>
                                                        <asp:TextBox ID="txtFechaFinContrato" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="row">
                                             <div id="divSII" runat="server" class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Objeto de la Sociedad SII</label>
                                                    <div class="col-sm-7" >
                                                        <asp:TextBox ID="txtSII" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="pnlContrato" ClientIDMode="Static">
                                        <asp:HiddenField ID="hfArea" runat="server" />
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Bloqueado</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlBloqueado" CssClass="form-control" runat="server">
                                                            <asp:ListItem Selected="True" Value="0">NO</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Pertenece a un grupo Económico</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlPerteneceGrupo" CssClass="form-control" runat="server">
                                                            <asp:ListItem Selected="True" Value="0">NO</asp:ListItem>
                                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Motivo Bloqueo</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlMotivoBloqueo" CssClass="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group row">
                                                    <label class="col-sm-5 control-label">Grupo Económico</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlGrupoEcono" CssClass="form-control" runat="server">
                                                            <asp:ListItem Value="0">Seleccione</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                             </div>
                        </div>

                    </div>
                    <table style="width:100%">
                        <tr>
                            <td>
                                <asp:Button ID="btnAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-warning pull-right" OnClientClick="return Dialogo();" Style="font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif;" />
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success pull-right" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" Style="font: 14px 'Noto Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

</div>

