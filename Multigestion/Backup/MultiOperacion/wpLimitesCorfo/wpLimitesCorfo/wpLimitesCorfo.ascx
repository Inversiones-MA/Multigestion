<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpLimitesCorfo.ascx.cs" Inherits="MultiOperacion.wpLimitesCorfo.wpLimitesCorfo.wpLimitesCorfo" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<script type="text/javascript">


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

    function formato(s, e) {
        var monto = encodeHtml(s.GetText());
        if (monto != '') {
            monto = parseFloat(monto.replace(/\./g, '').replace(/\,/g, '.'))
            monto = formatNumber.new(parseFloat(monto).toFixed(0));
            s.SetText(monto);
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

    function calculoApalancamientoAlfa(s, e) {
        var result = isFinite(txtTotalFianzaVigenteAlfa.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoAlfa.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteAlfa.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoAlfa.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtApalancamientoAlfa.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoApalancamientoReco(s, e) {
        var result = isFinite(txtTotalFianzaVigenteReco.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoReco.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteReco.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoReco.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtApalancamientoReco.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoApalancamientoVictoria(s, e) {
        var result = isFinite(txtTotalFianzaVigenteVictoria.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoVictoria.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteVictoria.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoVictoria.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtApalancamientoVictoria.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoApalancamientoBeta(s, e) {
        var result = isFinite(txtTotalFianzaVigenteBeta.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoBeta.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteBeta.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtValorFondoBeta.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtApalancamientoBeta.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoPalancaAlfa(s, e) {
        var result = isFinite(txtTotalFianzaVigenteAlfaDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleAlfa.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteAlfaDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleAlfa.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtPalancaAlfa.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoPalancaReco(s, e) {
        var result = isFinite(txtTotalFianzaVigenteRecoDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleReco.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteRecoDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleReco.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtPalancaReco.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoPalancaVictoria(s, e) {
        var result = isFinite(txtTotalFianzaVigenteVictoriaDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleVictoria.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteVictoriaDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleVictoria.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtPalancaVictoria.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
    }

    function calculoPalancaBeta(s, e) {
        var result = isFinite(txtTotalFianzaVigenteBetaDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleBeta.GetText().replace(/\./g, '').replace(/\,/g, '.')) ?
            txtTotalFianzaVigenteBetaDPL.GetText().replace(/\./g, '').replace(/\,/g, '.') / txtDisponibleBeta.GetText().replace(/\./g, '').replace(/\,/g, '.') : 0;

        txtPalancaBeta.SetText(formatNumber.new(parseFloat(result).toFixed(2)));
        formato(s, e);
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
            <h4 class="page-title">Limites Corfo</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Limites Corfo</a>
                </li>
                <li class="active">CORFO
                </li>
            </ol>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h4>I. APALANCAMIENTO DEL FONDO ACTUAL</h4>

            <!-- tabla / grilla -->
            <div class="row">
                <div class="col-md-12">
                    <table border="0" style="table-layout: fixed; width: 100%;">
                        <tr>
                            <td>
                                <div class="card-box">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover">
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center"><b>ALFA</b></td>
                                                <td style="text-align: center"><b>RECONSTRUCCIÓN</b></td>
                                                <td style="text-align: center"><b>VICTORIA</b></td>
                                                <td style="text-align: center"><b>BETA FONDO DE GARANTÍA</b></td>
                                            </tr>
                                            <tr>
                                                <td><b>Total Fianzas Vigentes</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteAlfa" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteReco" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteVictoria" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteBeta" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>VALOR ACTUAL DEL FONDO (V.F.)</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtValorFondoAlfa" runat="server" ClientInstanceName="txtValorFondoAlfa" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoApalancamientoAlfa" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtValorFondoReco" runat="server" ClientInstanceName="txtValorFondoReco" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoApalancamientoReco" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtValorFondoVictoria" runat="server" ClientInstanceName="txtValorFondoVictoria" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoApalancamientoVictoria" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtValorFondoBeta" runat="server" ClientInstanceName="txtValorFondoBeta" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoApalancamientoBeta" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>APALANCAMIENTO DEL FONDO ACTUAL</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtApalancamientoAlfa" runat="server" ClientInstanceName="txtApalancamientoAlfa" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtApalancamientoReco" runat="server" ClientInstanceName="txtApalancamientoReco" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtApalancamientoVictoria" runat="server" ClientInstanceName="txtApalancamientoVictoria" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtApalancamientoBeta" runat="server" ClientInstanceName="txtApalancamientoBeta" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h4>II. DISPONIBLE PALANCA LIQUIDA (D.P.L.)</h4>
            <div class="row">
                <div class="col-md-12">
                    <table border="0" style="table-layout: fixed; width: 100%;">
                        <tr>
                            <td>
                                <div class="card-box">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover">
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center"><b>ALFA</b></td>
                                                <td style="text-align: center"><b>RECONSTRUCCIÓN</b></td>
                                                <td style="text-align: center"><b>VICTORIA</b></td>
                                                <td style="text-align: center"><b>BETA FONDO DE GARANTÍA</b></td>
                                            </tr>
                                            <tr>
                                                <td><b>Total Fianzas Vigentes</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteAlfaDPL" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteRecoDPL" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteVictoriaDPL" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalFianzaVigenteBetaDPL" runat="server" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>DISPONIBLE Y V. NEGOCIABLES</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtDisponibleAlfa" runat="server" ClientInstanceName="txtDisponibleAlfa" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoPalancaAlfa" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtDisponibleReco" runat="server" ClientInstanceName="txtDisponibleReco" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoPalancaReco" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtDisponibleVictoria" runat="server" ClientInstanceName="txtDisponibleVictoria" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoPalancaVictoria" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtDisponibleBeta" runat="server" ClientInstanceName="txtDisponibleBeta" CssClass="form-control" ClientIDMode="Static" Width="150">
                                                        <ClientSideEvents KeyPress="function(s,e){ esDecimal(s,e);}" />
                                                        <ClientSideEvents KeyUp="calculoPalancaBeta" />
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>DISPONIBLE PALANCA LIQUIDA (D.P.L.)</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPalancaAlfa" runat="server" ClientInstanceName="txtPalancaAlfa" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPalancaReco" runat="server" ClientInstanceName="txtPalancaReco" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPalancaVictoria" runat="server" ClientInstanceName="txtPalancaVictoria" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPalancaBeta" runat="server" ClientInstanceName="txtPalancaBeta" ClientIDMode="Static" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h4>III. Márgenes Generales</h4>

            <!-- tabla / grilla -->
            <div class="row">
                <div class="col-md-12">
                    <table border="0" style="table-layout: fixed; width: 100%;">
                        <tr>
                            <td>
                                <div class="card-box">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-hover">
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center"><b>ALFA</b></td>
                                                <td style="text-align: center"><b>RECONSTRUCCIÓN</b></td>
                                                <td style="text-align: center"><b>VICTORIA</b></td>
                                                <td style="text-align: center"><b>BETA FONDO DE GARANTÍA</b></td>
                                                <td style="text-align: center"><b>TOTAL</b></td>
                                            </tr>
                                            <tr>
                                                <td><b>NUMEROS OPERACIONES VIGENTES</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalAlfaopv" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalRecoopv" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalVictoriaopv" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalBetaopv" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotal" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>NUMERO CLIENTES CON DEUDA VIGENTES</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalAlfaDeudas" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalRecoDeudas" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalVictoriaDeudas" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalBetaDeudas" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtTotalDeudas" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>CLIENTES CON GARANTÍA <40% DEUDA</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtGaratias40Alfa" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtGarantias40Reco" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtGarantias40Victoria" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtGarantias40Beta" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtGarantias40total" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>CLIENTES SIN GARANTÍA REAL (numero de casos)</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtSinGarantiaAlfa" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtSinGarantiaReco" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtSinGarantiaVictoria" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtSinGarantiaBeta" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtSinGarantiaTotal" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>PORCENTAJE DE CLIENTES SIN GTÍA (número de casos)</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcSinGarantiaAlfa" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcSinGarantiaReco" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcSinGarantiaVictoria" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcSinGarantiaBeta" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcSinGarantiaTotal" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td><b>Pocentaje de Casos con Garantías Reales >= 40%</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcConGarantiaAlfa" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcConGarantiaReco" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcConGarantiaVictoria" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcConGarantiaBeta" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtPorcConGarantiaTotal" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Máxima Fianza por Beneficiario UF (x 3)</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaAlfa" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaReco" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaVictoria" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaBeta" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaTotal" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Máximo Afianzamiento Individual/Valor Fondo</b></td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaVFAlfa" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaVFReco" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaVFVictoria" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaVFBeta" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaximoFianzaVFTotal" runat="server" Width="150" ReadOnly="true" Border-BorderColor="Transparent"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h4>IV. Concentraciòn por cliente fianza vigentes</h4>

            <div class="col-md-6">
                <h4>MULTIAVAL</h4>
                <table border="0" style="table-layout: fixed; width: 100%;">
                    <tr>
                        <td>
                            <div class="card-box">
                                <div class="table-responsive">
                                    <dx:ASPxGridView ID="gvMulti1" runat="server" ClientInstanceName="gvMulti1" AutoGenerateColumns="false" Width="100%"
                                        OnPageIndexChanged="gvMulti1_PageIndexChanged" OnHeaderFilterFillItems="gvMulti1_HeaderFilterFillItems"
                                        OnHtmlDataCellPrepared="gvMulti1_HtmlDataCellPrepared">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="Empresa" Caption="Empresa" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="SumaMontoInsoluto" Caption="Total Monto Insoluto" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit DisplayFormatString="c" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="PorcentajeLinea" Caption="Porcentaje utilización" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowSort="false" />
                                        <Settings ShowFooter="true" ShowHeaderFilterButton="true" />
                                        <SettingsPager PageSize="20"></SettingsPager>
                                    </dx:ASPxGridView>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="col-md-6">
                <h4>MULTIAVAL DOS</h4>
                <table border="0" style="table-layout: fixed; width: 100%;">
                    <tr>
                        <td>
                            <div class="card-box">
                                <div class="table-responsive">
                                    <dx:ASPxGridView ID="gvMulti2" runat="server" ClientInstanceName="gvMulti2" AutoGenerateColumns="false" Width="100%"
                                        OnPageIndexChanged="gvMulti2_PageIndexChanged" OnHtmlDataCellPrepared="gvMulti2_HtmlDataCellPrepared">
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="Empresa" Caption="Empresa" HeaderStyle-Wrap="True" VisibleIndex="1" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="SumaMontoInsoluto" Caption="Total Monto Insoluto" HeaderStyle-Wrap="True" VisibleIndex="2" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                                <PropertiesTextEdit DisplayFormatString="c" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataTextColumn FieldName="PorcentajeLinea" Caption="Porcentaje utilización" HeaderStyle-Wrap="True" VisibleIndex="3" CellStyle-HorizontalAlign="Center" ShowInCustomizationForm="true"
                                                PropertiesTextEdit-MaxLength="12" PropertiesTextEdit-ValidationSettings-RequiredField-IsRequired="true">
                                                <HeaderStyle HorizontalAlign="Center" Wrap="True" Font-Bold="true" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowSort="false" />
                                        <Settings ShowFooter="true" ShowHeaderFilterButton="true" />
                                        <SettingsPager PageSize="20"></SettingsPager>
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
        <div class="col-md-12 col-sm-12">
            <table style="width: 100%;">
                <tr>
                    <td>

                    </td>
                    <td>
                        <dx:ASPxButton ID="btnGuardar" runat="server" Text="Guardar Informacion" ClientInstanceName="btnGuardar" AutoPostBack="false" CssClass="btn btn-default pull-right">

                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</div>
