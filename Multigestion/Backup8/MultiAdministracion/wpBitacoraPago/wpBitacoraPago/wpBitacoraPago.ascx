<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpBitacoraPago.ascx.cs" Inherits="MultiAdministracion.wpBitacoraPago.wpBitacoraPago.wpBitacoraPago" %>


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
        <h4 class="page-title">Registro Siniestro</h4>
        <ol class="breadcrumb">
            <li>
                <a href="#">Contabilidad</a>
            </li>
            <li class="active">Registro Siniestro
            </li>
        </ol>
    </div>
</div>

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

<br />


<div class="row clearfix margenBottom">
    <div class="form-horizontal" role="form">
        <div class="col-md-6">

            <div class="form-group">
                <asp:Label ID="Label10" CssClass="col-sm-5 control-label" runat="server" Text="Cliente" Font-Bold="True"></asp:Label>
                <div class="col-sm-7">
                    <asp:DropDownList runat="server" ID="ddlCliente" CssClass="form-control input-sm">
                        <asp:ListItem>Seleccione</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>



        </div>
        <div class="col-md-6">

            <div class="form-group">
                <asp:Label ID="Label12" CssClass="col-sm-5 control-label" runat="server" Text="Nº Certificado" Font-Bold="True"></asp:Label>
                <div class="col-sm-7">
                    <asp:DropDownList runat="server" ID="ddlNCertificado" CssClass="form-control input-sm">
                        <asp:ListItem>Seleccione</asp:ListItem>

                    </asp:DropDownList>
                </div>
            </div>

        </div>




    </div>
</div>


<div class="row clearfix margenBottom form-horizontal" role="form">
    <div class="col-md-4">
        <div class="form-group">

            <asp:Label ID="Label19" CssClass="col-sm-6 control-label" runat="server" Text="RUT" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtRut"  CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label20" CssClass="col-sm-6 control-label" runat="server" Text="Estado Crédito" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtEdoCredito" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label21" CssClass="col-sm-6 control-label" runat="server" Text="Fondo" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtFondo" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label27" CssClass="col-sm-6 control-label" runat="server" Text="Gastos Operacionales" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtGastosOperacionales" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label28" CssClass="col-sm-6 control-label" runat="server" Text="Fecha Facturación Comisión" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtFecFactComision" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

    </div>


    <div class="col-md-4">
        <div class="form-group">

            <asp:Label ID="Label22" CssClass="col-sm-6 control-label" runat="server" Text="Ejecutivo" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtEjecutivo" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label23" CssClass="col-sm-6 control-label" runat="server" Text="Acreedor" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtAcreedor" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label24" CssClass="col-sm-6 control-label" runat="server" Text="% Comisión Siniestralidad" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtPorcComSiniestralidad" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label29" CssClass="col-sm-6 control-label" runat="server" Text="Nº Factura Comisión" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtNroFactComision" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label30" CssClass="col-sm-6 control-label" runat="server" Text="Fecha Facturación Gastos Ope." Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtFecFactGastOperacionales" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="col-md-4">
        <div class="form-group">

            <asp:Label ID="Label25" CssClass="col-sm-6 control-label" runat="server" Text="Estado Cretificado" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtEdoCertificado" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label26" CssClass="col-sm-6 control-label" runat="server" Text="% Cobertura CF" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtPorcCobCF" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label31" CssClass="col-sm-6 control-label" runat="server" Text="Comisión" Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtComision" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="Label32" CssClass="col-sm-6 control-label" runat="server" Text="Nº Facturación Gastos Ope." Font-Bold="True"></asp:Label>
            <div class="col-sm-6">
                <asp:TextBox runat="server" ID="txtNroFactGastOpe" CssClass="form-control input-sm col-md4" ReadOnly="true"></asp:TextBox>
            </div>
        </div>

    </div>



</div>



<asp:Panel runat="server" ID="pnFormulario">
    <div class="row clearfix margenBottom">
        <div class="form-horizontal" role="form">
            <div class="col-md-6">



                <div class="form-group">
                    <asp:Label ID="Label2" CssClass="col-sm-5 control-label" runat="server" Text="Motivo" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlMotivo" CssClass="form-control input-sm">
                            <asp:ListItem>Seleccione</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="lbFechaDevolución" CssClass="col-sm-5 control-label" runat="server" Text="Cuota Nº" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlCuotaN" CssClass="form-control input-sm">
                            <asp:ListItem>Seleccione</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group">
                    <asp:Label ID="Label14" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Cobro" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <SharePoint:DateTimeControl ID="dtcFechaCobro" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />

                    </div>
                </div>



                <div class="form-group">
                    <asp:Label ID="Label11" CssClass="col-sm-5 control-label" runat="server" Text="Banco" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlBanco" CssClass="form-control input-sm">
                            <asp:ListItem>Seleccione</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="lbContratoTipo" CssClass="col-sm-5 control-label" runat="server" Text="Nº Documento Pago" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="txtNroDocumento" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

            </div>
            <div class="col-md-6">



                <div class="form-group">
                    <asp:Label ID="Label13" CssClass="col-sm-5 control-label" runat="server" Text="Causa" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <asp:DropDownList runat="server" ID="ddlCausa" CssClass="form-control input-sm">
                            <asp:ListItem>Seleccione</asp:ListItem>
                            <asp:ListItem Value="1">Gasto</asp:ListItem>
                            <asp:ListItem Value="2">Devolución</asp:ListItem>
                            <asp:ListItem Value="3">Ingreso</asp:ListItem>
                            <asp:ListItem Value="4">Otro</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group">
                    <asp:Label ID="Label18" CssClass="col-sm-5 control-label" runat="server" Text="" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                    </div>
                </div>


                <div class="form-group">
                    <asp:Label ID="Label3" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Pago" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <SharePoint:DateTimeControl ID="dtcFechaPago" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />

                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label9" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Mora" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <SharePoint:DateTimeControl ID="dtcFechaMora" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />

                    </div>
                </div>



                <div class="form-group">
                    <asp:Label ID="Label17" CssClass="col-sm-5 control-label" runat="server" Text="Monto" Font-Bold="True"></asp:Label>
                    <div class="col-sm-7">
                        <asp:TextBox runat="server" ID="txtValor" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>



            </div>
        </div>
    </div>

    <div class="row clearfix margenBottom">
        <div class="form-horizontal" role="form">
            <!-- Grupo 1 -->

            <label for="inputEmail3" class="col-sm-2 control-label">Comentarios:</label>
            <div class="col-sm-10">
                <asp:TextBox ID="txt_Comentario" TextMode="multiline" Columns="20" Rows="5" runat="server" CssClass="col-md-12 control-label" MaxLength="1000" />
            </div>

        </div>
    </div>

</asp:Panel>



<table width="100%">
    <tr>
        <td>
            <asp:Button ID="btnVolverAtras" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClientClick="return Dialogo();" />
        </td>

        <td>
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-primary pull-right" OnClientClick="return Dialogo();" />

            <asp:Button ID="btnImprimir" runat="server" Text="Limpiar" class="btn btn-warning pull-right" />

            <asp:Button class="btn btn-primary btn-success pull-right" ID="btnGuardar" runat="server" Text="Guardar" OnClientClick="return Dialogo();" />
        </td>
    </tr>
</table>


<br />
<br />

<div class="panel panel-warning" style="text-align: center; width: 100%">
    <div class="panel-heading">Valores Cálculados</div>
    <div class="panel-body">
        <br />
        <div class="row clearfix margenBottom form-horizontal" role="form">
            <div class="col-md-4">
                <div class="form-group">

                    <asp:Label ID="Label4" CssClass="col-sm-6 control-label" runat="server" Text="Monto Asumido Acreedor" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtmontoAsumidoAcreedor" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label5" CssClass="col-sm-6 control-label" runat="server" Text="Monto Reintegrado FC" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtMontoReintegradoFC" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label15" CssClass="col-sm-6 control-label" runat="server" Text="% Siniestro Asumido FG" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtPSinietroAsumidoFG" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

            </div>


            <div class="col-md-4">
                <div class="form-group">

                    <asp:Label ID="Label1" CssClass="col-sm-6 control-label" runat="server" Text="Monto Asumido FG Gastos" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtMontoAsumidoFGGastos" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label6" CssClass="col-sm-6 control-label" runat="server" Text="Monto FG Sinietro" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtMontoFGSiniestro" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label16" CssClass="col-sm-6 control-label" runat="server" Text="% Siniestro Asumido IGR" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtPSiniesrtoAsumidoIGR" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="form-group">

                    <asp:Label ID="Label7" CssClass="col-sm-6 control-label" runat="server" Text="Monto Asumido IGR" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtmontoAsumidoIGR" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label8" CssClass="col-sm-6 control-label" runat="server" Text="% Siniestro Asumido Acreedor" Font-Bold="True"></asp:Label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtPSiniestoAsumidoAcreedor" MaxLength="16" CssClass="form-control input-sm col-md4"></asp:TextBox>
                    </div>
                </div>

            </div>



        </div>
    </div>
</div>

                                        </div>