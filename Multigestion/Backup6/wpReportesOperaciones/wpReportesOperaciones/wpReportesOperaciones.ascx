<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpReportesOperaciones.ascx.cs" Inherits="MultiOperacion.wpReportesOperaciones.wpReportesOperaciones.wpReportesOperaciones" %>


<style type="text/css">
    .auto-style1 {
        width: 131px;
    }

    .align {
        text-align: justify;
    }

    .auto-style2 {
        width: 9px;
    }

    .auto-style3 {
        width: 152px;
    }

    .auto-style4 {
        width: 149px;
        height: 22px;
    }

    .auto-style5 {
        width: 152px;
        height: 22px;
    }

    .auto-style6 {
        height: 22px;
    }

    .auto-style7 {
        height: 12.0pt;
        width: 60pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 700;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style8 {
        width: 126pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: left;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style9 {
        width: 9pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style10 {
        width: 71pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 700;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style11 {
        width: 60pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: left;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style12 {
        width: 60pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 700;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style13 {
        height: 12.0pt;
        color: white;
        font-size: 9.0pt;
        font-weight: 700;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: left;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: gray;
    }

    .auto-style14 {
        color: white;
        font-size: 9.0pt;
        font-weight: 700;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: gray;
    }

    .auto-style15 {
        height: 12.0pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: top;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style16 {
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: top;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style17 {
        width: 251pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: left;
        vertical-align: top;
        white-space: normal;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style18 {
        height: 12.0pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style19 {
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style20 {
        width: 71pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: top;
        white-space: normal;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style21 {
        width: 60pt;
        color: white;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: top;
        white-space: normal;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style22 {
        width: 60pt;
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: general;
        vertical-align: top;
        white-space: normal;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }

    .auto-style23 {
        color: black;
        font-size: 9.0pt;
        font-weight: 400;
        font-style: normal;
        text-decoration: none;
        font-family: Calibri, sans-serif;
        text-align: left;
        vertical-align: bottom;
        white-space: nowrap;
        border-style: none;
        border-color: inherit;
        border-width: medium;
        padding-left: 1px;
        padding-right: 1px;
        padding-top: 1px;
        background: white;
    }
    .auto-style34 {
        height: 23px;
    }
</style>
<asp:Button ID="Button8" runat="server" Text="certificado fianza tecnica" OnClick="Button8_Click" />
<br />
<asp:Button ID="Button1" runat="server" Text="certificado fianza comercial" OnClick="Button1_Click" />
<br />
<asp:Button ID="Button2" runat="server" Text="cata de garantias" OnClick="Button2_Click" />

<br />
<asp:Button ID="Button3" runat="server" Text="contrato de subfianza" OnClick="Button3_Click" />


<%--<br />
<asp:Button ID="Button4" runat="server" Text="Solicitud de Emision" OnClick="Button4_Click" />--%>


<br />
<asp:Button ID="Button5" runat="server" Text="Certificado Elegibilidad" OnClick="Button5_Click" />

<br />
<asp:Button ID="Button6" runat="server" Text="cata de garantias ITAU" OnClick="Button6_Click" />

<br />
<asp:Button ID="Button7" runat="server" Text="cata de garantias SNATANDER" OnClick="Button7_Click" />

<br />
<asp:Button ID="Button9" runat="server" Text="instruccion de curse" OnClick="Button9_Click" />

<br />
<asp:Button ID="Button10" runat="server" Text="cGBancoEstado" OnClick="Button10_Click" />

<br />
<asp:Button ID="Button11" runat="server" Text="CFBancoEstado " OnClick="Button11_Click" />

<br />
<asp:Button ID="Button12" runat="server" Text="prueba formatos " OnClick="Button12_Click" />
<br />
<asp:Button ID="Button13" runat="server" Text="Solicitud de Pago de Garantía" OnClick="Button13_Click" />


                



<table style="width: 100%;" class="table table-bordered sinBorde">
    <tr>
        <td style="text-align: center;">CERTIFICADO DE FIANZA</td>
    </tr>
    <tr>
        <td style="text-align: center;" class="auto-style6">NOMINATIVO - A LA VISTA</td>
    </tr>
    <tr>
        <td style="text-align: center;">LEY N° 20.179 </td>
    </tr>
    <tr>
        <td>
            <table style="width: 100%; border: 1px solid black;">
                <tr style="border: 1px solid black;">
                    <td colspan="2">Certificado Fianza Técnica</td>
                    <td colspan="2">@CertificadoFichaTecnica</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Afianzado:</td>
                    <td>@clienteRazonSocial</td>
                    <td class="auto-style2">R.U.T.:</td>
                    <td>@clienteRut</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Domicilio Afianzado:</td>
                    <td colspan="3">@clienteDireccion</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Acreedor:</td>
                    <td>@acreedorRazonSocial</td>
                    <td class="auto-style2">R.U.T.:</td>
                    <td>@acreedorRut</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Domicilio Acreedor:</td>
                    <td colspan="3">@acreedorDireccion</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Fiador:</td>
                    <td>MULTIAVAL S.A.G.R.</td>
                    <td class="auto-style2">R.U.T.:</td>
                    <td>76.138.346-9</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Domicilio Fiador:</td>
                    <td colspan="3">Santa Beatriz #100, Santiago, Providencia, RM Región Metropolitana</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style1">Fondo:</td>
                    <td colspan="3">@Fondo</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <br />

            <table style="width: 100%;">
                <tr>
                    <td class="align">En la ciudad de Santiago, a @dia días del @mes de diciembre de @año, MULTIAVAL S.A.G.R., en adelante "el Fiador",
                representada por su Gerente General Cristobal Brunetti J, en conjunto con e representante del Fondo de Garantía señalado,
                emite este certificado en conformidad a las disposiciones de la Ley N°20.179, del 20 de junio de 2007, constituyéndose en
                este acto en fiadores del Afianzado, a favor del Acreedor, con el objeto de garantizar a éste la siguiente obligación del
                Afianzado, en adelante "la Obligación Caucionada":</td>
                </tr>
            </table>
            <br />
            <br />
        </td>
    </tr>
    <tr>
        <td>
            <table style="width: 100%; border: 1px solid black;">

                <tr style="border: 1px solid black;">
                    <td class="auto-style5">Fecha de Emisión:</td>
                    <td class="auto-style6">@fechaInicio</td>
                    <td class="auto-style4">Fecha de Vencimiento:</td>
                    <td class="auto-style6">@fechaFin</td>
                </tr>
                <tr style="border: 1px solid black;">
                    <td class="auto-style3">Monto Operación:</td>
                    <td colspan="3">@clienteDireccion</td>
                </tr>

                <tr style="border: 1px solid black;">
                    <td colspan="4">@Observacion</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <br />
            <br />

            <table style="width: 100%;">
                <tr>
                    <td class="align">La fianza de que da cuenta este certificado se extiende con cobertura al 100% del capital más los reajustes, intereses
                pactados, intereses penales y demás prestaciones accesorias que correspondan de la Obligación Caucionada.

                        <br />
                        Esta Fianza no goza del beneficio de excusión, en conformidad a lo establecido en la Ley N° 20.179. Por otra parte, el
                Fiador renuncia en este acto, a los beneficios de división y a la excepción de subrogación, contemplados en los artículos 2.367
                y 2.355,respectivamente, del Código Civil.
                        <br />
                        En caso de incumplimiento de la Obligación Caucionada, el Acreedor requerirá de pago al Fiador mediante carta
                certificada dirigida al domicilio de éste señalado en este certificado, debiendo el Fiador pagar al Acreedor la totalidad de las
                Obligaciones Caucionadas dentro del plazo de 10 días corridos, contados desde la fecha del señalado requerimiento.</td>
                </tr>

                <tr>
                    <td class="align">&nbsp;</td>
                </tr>

            </table>
            <br />
            <br />
            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 446pt"
                width="594">
                <colgroup>
                    <col style="width: 60pt" width="80" />
                    <col style="mso-width-source: userset; mso-width-alt: 6144; width: 126pt" width="168" />
                    <col style="mso-width-source: userset; mso-width-alt: 438; width: 9pt" width="12" />
                    <col style="mso-width-source: userset; mso-width-alt: 3437; width: 71pt" width="94" />
                    <col span="3" style="width: 60pt" width="80" />
                </colgroup>
                <tr height="16">
                    <td class="auto-style7" height="16" width="80">Fecha:</td>
                    <td class="auto-style8" width="168">11/10/2014</td>
                    <td class="auto-style9" width="12">&nbsp;</td>
                    <td class="auto-style10" width="94">UF:</td>
                    <td class="auto-style11" width="80"><a name="RANGE!E1">24,391.36</a></td>
                    <td class="auto-style12" width="80">Dólar:</td>
                    <td class="auto-style11" width="80"><a name="RANGE!G1">0.00</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style13" colspan="5" height="16">ANTECEDENTES DEL CLIENTE</td>
                    <td class="auto-style14">N° Certificado:</td>
                    <td class="auto-style14"><a name="RANGE!G2">839</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style15" colspan="2" height="16" style="mso-ignore: colspan">Nombre / Razón Social</td>
                    <td class="auto-style16">:</td>
                    <td class="auto-style17" colspan="4" width="334"><a name="RANGE!D3">Inmobiliaria e Inversiones Gonvaldi Limitada</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style18" height="16">Rut</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style19">:</td>
                    <td class="auto-style20" width="94"><a name="RANGE!D4">76.428.819 - K</a></td>
                    <td class="auto-style21" width="80"><a name="RANGE!E4">76428819K</a></td>
                    <td class="auto-style22" width="80">&nbsp;</td>
                    <td class="auto-style22" width="80">&nbsp;</td>
                </tr>
                <tr height="16">
                    <td class="auto-style18" height="16">Dirección</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style19">:</td>
                    <td class="auto-style23" colspan="4"><a name="RANGE!D5">Euclides 1490</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style18" height="16">Comuna</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style19">:</td>
                    <td class="auto-style23" colspan="4"><a name="RANGE!D6">San Miguel</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style18" height="16">Provincia</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style19">:</td>
                    <td class="auto-style23" colspan="4"><a name="RANGE!D7">Santiago</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style18" height="16">Región</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style19">:</td>
                    <td class="auto-style23" colspan="4"><a name="RANGE!D8">RM Región Metropolitana de Santiago</a></td>
                </tr>
                <tr height="16">
                    <td class="auto-style18" height="16">Actividad</td>
                    <td class="auto-style19">&nbsp;</td>
                    <td class="auto-style19">:</td>
                    <td class="auto-style23" colspan="4">62 Comercio al por menor, restaurantes y hoteles</td>
                </tr>
            </table>
            <br />
            <br />
        </td>
    </tr>

</table>


<table style="width: 100%">
    <tr>
        <td class="auto-style13" colspan="6">ANTECEDENTES CONTRACTUALES</td>
    </tr>
    <tr>
        <td colspan="2">Fecha Contrato Garantía:</td>
        <td colspan="4">34/45/1209</td>
    </tr>
    <tr>
        <td colspan="2">Fecha Vencimiento</td>
        <td colspan="4">34/45/1209 </td>
    </tr>
</table>

<p>
    &nbsp;</p>