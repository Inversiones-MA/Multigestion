<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpIngresoGarantia.ascx.cs" Inherits="MultiComercial.wpIngresoGarantia.wpIngresoGarantia.wpIngresoGarantia" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<script src="../../_layouts/15/MultiComercial/Validaciones.js"></script>
<script src="../../_layouts/15/MultiComercial/jquery-1.11.1.js"></script>

<script type="text/javascript">

    //function ValorAjustable(cad) {
    //    var valorA = cad + 'txtValorA';
    //    var valorC = cad + 'txtValorC';
    //    var hddAjuste = cad + 'hddAjuste';
    //    if (document.getElementById(valorC).value != "") {
    //        if (hddAjuste != "0") {
    //            document.getElementById(valorA).value = parseFloat(document.getElementById(valorC).value.replace(/\./g, '').replace(/\,/g, '.')) - (parseFloat(document.getElementById(valorC).value.replace(/\./g, '').replace(/\,/g, '.')) * (parseFloat(document.getElementById(hddAjuste).value) / 100)).toFixed(0);
    //            document.getElementById(valorA).value = (document.getElementById(valorA).value).replace(/\./g, ',');
    //            document.getElementById(valorA).disabled = true;
    //        } else {
    //            document.getElementById(valorA).value = document.getElementById(valorC).value;
    //            document.getElementById(valorA).disabled = true;
    //        }
    //    }
    //}

    function soloDecimales(e) {
        key = e.keyCode ? e.keyCode : e.which
        // 0-9
        if (key > 47 && key < 58) {
            return true
        }
        // ,
        if (key == 44) {
            return true
        }
        return false
    }

    function ValidarFup(index) {
        var i = parseInt(index) + 1;
        var table = document.getElementById('<%=GvTasacionHistorial.ClientID %>');
        var Row = table.rows[i];

        var c = Row.cells[7].childNodes[0];
        if (c.value.length < 4) {
            alert('Seleccione un archivo valido');
            return false;
        }
        else {
            var filename = c.value; //$arrT.eq(index).val();
            var extension = filename.replace(/^.*\./, '');

            var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp', 'pdf'];
            if ($.inArray(extension.toLowerCase(), fileExtension) == -1) {
                alert("Las extensiones permitidas son : " + fileExtension.join(', '));
            }
            else {
                var c = Row.cells[7].childNodes[1];
                c.style.display = 'block';
                check = '1';
            }
            return true;
        }

        //$("#<%=GvTasacionHistorial.ClientID%> tr").each(function () {
        //            contador++;
        //    if (check === '1')
        //        return false;
        //    $("#<%=GvTasacionHistorial.ClientID%> td").each(function () {
        //        idd = $(this).html();
        //        
        //        if (idd === index) {
        //            var Row = table.rows[contador];
        //            var c = Row.cells[7].childNodes[0];
        //            if (c.value.length < 4) {
        //                alert('Seleccione un archivo valido');
        //                return false;
        //            }
        //            else {
        //                var filename = c.value; //$arrT.eq(index).val();
        //                var extension = filename.replace(/^.*\./, '');
        //                
        //                var fileExtension = ['jpeg', 'jpg', 'png', 'gif', 'bmp', 'pdf'];
        //                 if ($.inArray(extension.toLowerCase(), fileExtension) == -1) {
        //                     alert("Las extensiones permitidas son : " + fileExtension.join(', '));
        //                  }
        //                  else {
        //                         var c = Row.cells[7].childNodes[1];
        //                         c.style.display = 'block';
        //                         check = '1';
        //                        }
        //                return true;
        //            } 
        //        }
        //    });
        //    return true;
        // });

        //for (var i = 0; i < table.rows.length - 1; i++) {
        //    var node = table[i];

        //    var a = '';

        //if (node != null && node.type == "checkbox") {
        //    alert("ok.");
        //    break;
        //}
    }

    function ValidarExt() {

    }

    function SetTab(panel) {
        var hdn = panel;
        if (hdn != null) {

            if (hdn.value != "") {
                var b = document.getElementById("myTab");

                switch (hdn) {
                    case 'li1':
                        $('#<%=pnTasacion.ClientID %>').hide();
                        $('#<%=pnOperaciones.ClientID %>').hide();
                        $('#historial').hide();
                        $('#li1').addClass('active');
                        $('#li2').removeClass('active');
                        $('#li3').removeClass('active');
                        $('#li4').removeClass('active');
                        $('#li5').removeClass('active');
                        $('#li6').removeClass('active');
                        $('#<%=pnComercial.ClientID %>').show();
                        $('#<%=pnGarantia.ClientID %>').show();
                        document.getElementById("hdnTab").value = "li1"
                        break;
                    case 'li2':
                        $('#<%=pnOperaciones.ClientID %>').hide();
                        $('#<%=pnGarantia.ClientID %>').hide();
                        $('#li1').removeClass('active');
                        $('#li2').addClass('active');
                        $('#li3').removeClass('active');
                        $('#li4').removeClass('active');
                        $('#li5').removeClass('active');
                        $('#li6').removeClass('active');
                        $('#historial').show();
                        $('#<%=pnComercial.ClientID %>').show();
                        $('#<%=pnTasacion.ClientID %>').show();
                        document.getElementById("hdnTab").value = "li2"
                        break;
                    case 'li3':
                        $('#historial').hide();
                        $('#divContribuciones').hide();
                        $('#<%=pnComercial.ClientID %>').hide();
                        $('#<%=pnGarantia.ClientID %>').hide();
                        $('#<%=pnTasacion.ClientID %>').hide();
                        $('#<%=pnCorfo.ClientID %>').hide();
                        $('#<%=pnConstitucion.ClientID %>').hide();
                        $('#<%=pnOperaciones.ClientID %>').show();
                        $('#<%=pnSeguro.ClientID %>').show()
                        $('#li3').addClass('active');
                        $('#li1').removeClass('active');
                        $('#li2').removeClass('active');
                        $('#li4').removeClass('active');
                        $('#li5').removeClass('active');
                        $('#li6').removeClass('active');
                        document.getElementById("hdnTab").value = "li3"
                        break;
                    case 'li4':
                        $('#historial').hide();
                        $('#divContribuciones').hide();
                        $('#<%=pnComercial.ClientID %>').hide();
                        $('#<%=pnGarantia.ClientID %>').hide();
                        $('#<%=pnTasacion.ClientID %>').hide();
                        $('#<%=pnCorfo.ClientID %>').hide();
                        $('#<%=pnSeguro.ClientID %>').hide()
                        $('#<%=pnOperaciones.ClientID %>').show();
                        $('#<%=pnConstitucion.ClientID %>').show();
                        $('#li4').addClass('active');
                        $('#li1').removeClass('active');
                        $('#li2').removeClass('active');
                        $('#li3').removeClass('active');
                        $('#li5').removeClass('active');
                        $('#li6').removeClass('active');
                        document.getElementById("hdnTab").value = "li4"
                        break;
                    case 'li5':
                        $('#historial').hide();
                        $('#divContribuciones').hide();
                        $('#<%=pnGarantia.ClientID %>').hide();
                        $('#<%=pnTasacion.ClientID %>').hide();
                        $('#<%=pnSeguro.ClientID %>').hide()
                        $('#<%=pnConstitucion.ClientID %>').hide();
                        $('#<%=pnOperaciones.ClientID %>').show();
                        $('#<%=pnCorfo.ClientID %>').show();
                        $('#li5').addClass('active');
                        $('#li1').removeClass('active');
                        $('#li2').removeClass('active');
                        $('#li3').removeClass('active');
                        $('#li4').removeClass('active');
                        $('#li6').removeClass('active');
                        document.getElementById("hdnTab").value = "li5"
                        break;
                    case 'li6':
                        $('#historial').hide();
                        $('#<%=pnGarantia.ClientID %>').hide();
                        $('#<%=pnTasacion.ClientID %>').hide();
                        $('#<%=pnSeguro.ClientID %>').hide()
                        $('#<%=pnConstitucion.ClientID %>').hide();
                        $('#<%=pnOperaciones.ClientID %>').show();
                        $('#<%=pnCorfo.ClientID %>').hide();
                        $('#<%=pnContribuciones.ClientID %>').show();
                        $('#divContribuciones').show();
                        $('#li6').addClass('active');
                        $('#li1').removeClass('active');
                        $('#li2').removeClass('active');
                        $('#li3').removeClass('active');
                        $('#li4').removeClass('active');
                        $('#li5').removeClass('active');
                        document.getElementById("hdnTab").value = "li6"
                        break;
                }
            }
        }
    }

    $(document).ready(function () {
        $("td.ms-dtinput a").addClass("btn btn-default").html("<i class='icon-calender'></i>");

        var hdn = document.getElementById("hdnTab")
        if (hdn.value != null && hdn.value != "") {
            var panel = hdn.value;
        }
        else {
            var panel = 'li1';
        }
        SetTab(panel);

        if ($('#<%=hdnArea.ClientID %>') != null) {
            var aa = $('#<%=hdnArea.ClientID %>').val();
            if ($('#<%=hdnArea.ClientID %>').val() != 'Jefe Operaciones' && $('#<%=hdnArea.ClientID %>').val() != 'Administrador' && $('#<%=hdnArea.ClientID %>').val() != 'Analista Operaciones' && $('#<%=hdnArea.ClientID %>').val() != 'Abogado') {
                var ctrl = document.getElementById('<%=pnConstitucion.ClientID %>').getElementsByTagName("*");

                for (var i = 0; i < ctrl.length; i++) {
                    //$('#<%=ddlEdoGarantia.ClientID %>').prop("disabled", true);
                  ctrl[i].readOnly = true;
              };
              $('#li5').hide();
          }
      }

       <%-- var ctrl1 = document.getElementById('<%=pnSeguro.ClientID %>').getElementsByTagName("*");
        for (var i = 0; i < ctrl1.length; i++) {
            if (ctrl1[i].id != 'ckbSeguro')
                ctrl1[i].disabled = true;
            else
                ctrl1[i].disabled = false;
        };--%>

    });

    function TabSeguro() {
        var ctrl1 = document.getElementById('<%=pnSeguro.ClientID %>').getElementsByTagName("*");
        var chk = document.getElementById('<%=ckbSeguro.ClientID %>');

       <%-- for (var i = 0; i < ctrl1.length; i++) {
            if (chk.checked) {
                $('#<%=hdnChk.ClientID %>').val('true');
                if (ctrl1[i].tagName === 'SELECT') {
                    ctrl1[i].selectedIndex == 0;
                }
                ctrl1[i].disabled = false;
            }
            else {
                if (ctrl1[i].id != 'ckbSeguro' && ctrl1[i].id != 'btnGuardarSeguro') {
                    //ctrl1[i].value = '';
                    ctrl1[i].disabled = true;
                }
                if (ctrl1[i].id === 'ckbSeguro')
                    $('#<%=hdnChk.ClientID %>').val('false');

                if (ctrl1[i].tagName === 'SELECT') {
                    ctrl1[i].selectedIndex == 0;
                }
            }
        }--%>
    }

    function ValidarTabSeguro(cad) {
        var montoSeguro = cad + 'txtMontoSeguro';
        document.getElementById(montoSeguro).style.borderColor = "";
        var DescripcionMontoSeguro = document.getElementById(montoSeguro).value;
        var aux = true;

        if (vacio(DescripcionMontoSeguro)) {
            document.getElementById(montoSeguro).style.borderColor = "red";
            aux = false;
        } else {
            document.getElementById(montoSeguro).style.borderColor = "";
        }

        if (aux) {
            SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showWaitScreenWithNoClose', 'Por Favor Espere...', 'procesando su requerimiento', 120, 550);
            return true;
        } else
            return false;
    }
</script>



<div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
    <h4>
        <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
    </h4>
</div>

<div id="dvFormulario" runat="server">
    <div id="DivHidden">
        <asp:HiddenField ID="hdnTab" runat="server" Value="" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnArea" runat="server" Value="" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnChk" runat="server" Value="" ClientIDMode="Static" />
    </div>

    <!-- Page-Title -->
    <div class="row marginInicio">
        <div class="col-sm-12">
            <h4 class="page-title">Comercial</h4>
            <ol class="breadcrumb">
                <li>
                    <a href="#">Datos Operacion</a>
                </li>
                <li class="active">Garant&iacute;as
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
                        <asp:Label ID="lbEmpresa" runat="server" Text=""></asp:Label></strong>
                    /
                    <asp:Label ID="lbRut" runat="server" Text=""></asp:Label>
                    / Negocio:
                    <asp:Label ID="lbOperacion" runat="server" Text=""></asp:Label>
                    / Ejecutivo:
                    <asp:Label ID="lbEjecutivo" runat="server" Text=""></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <!-- Contenedor Tabs -->
    <div class="row">
        <div class="col-md-12">
            <div class="tabbable" id="tabs-empRelacionadas">
                <%-- TAB --%>
                <ul class="nav nav-tabs navtab-bg nav-justified">
                    <li class="active">
                        <asp:LinkButton ID="lbGarantias" runat="server" OnClick="lbGarantias_Click">Garantías</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lbOperaciones" runat="server" OnClick="lbOperacion_Click">Registro Operación</asp:LinkButton>
                    </li>
                </ul>

                <div class="tab-content">
                    <div class="tab-pane active" id="panel-tab">
                        <div class="row responsive">
                            <div class="col-md-12">
                                <div class="card-box">
                                    <%-- Cuadros de Alerta --%>
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

                                    <div class="row">
                                        <div class="col-md-12">
                                            <ul class="nav nav-tabs navtab-bg nav-justified" id="myTab">
                                                <li id="li1">
                                                    <a href="#panel-tab1" data-toggle="tab" onclick="SetTab('li1')">
                                                        <asp:Label ID="lbLi1" runat="server" Text="Garantía"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li2">
                                                    <a href="#panel-tab2" data-toggle="tab" onclick="SetTab('li2')">
                                                        <asp:Label ID="lbLi2" runat="server" Text="Tasación"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li3">
                                                    <a href="#panel-tab3" data-toggle="tab" onclick="SetTab('li3')">
                                                        <asp:Label ID="lbLi3" runat="server" Text="Seguro"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li4">
                                                    <a href="#panel-tab4" data-toggle="tab" onclick="SetTab('li4')">
                                                        <asp:Label ID="lbLi4" runat="server" Text="Constitución"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li5">
                                                    <a href="#panel-tab5" data-toggle="tab" onclick="SetTab('li5')">
                                                        <asp:Label ID="lbLi5" runat="server" Text="Corfo"></asp:Label>
                                                    </a>
                                                </li>
                                                <li id="li6">
                                                    <a href="#panel-tab6" data-toggle="tab" onclick="SetTab('li6')">
                                                        <asp:Label ID="lbLi6" runat="server" Text="Contribuciones"></asp:Label>
                                                    </a>
                                                </li>
                                            </ul>

                                            <div class="tab-content">
                                                <asp:Panel ID="pnComercial" runat="server">
                                                    <asp:Panel ID="pnGarantia" runat="server" ClientIDMode="Static">
                                                        <div class="row clearfix margenBottom">
                                                            <div class="form-horizontal" role="form">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label18" CssClass="col-sm-5 control-label" runat="server" Text="Clase Garantía" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlTipoG" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoG_SelectedIndexChanged" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="LblInscripcion" CssClass="col-sm-5 control-label" runat="server" Text="" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <table style="width: 100%;">
                                                                                <tr>
                                                                                    <td style="width: 50%;">
                                                                                        <asp:TextBox runat="server" ID="txtNroInscripcion" MaxLength="20" CssClass="form-control" ClientIDMode="Static" />
                                                                                        <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" InputDirection="LeftToRight" 
                                                                                            TargetControlID="txtNroInscripcion" OnFocusCssClass="MaskedEditFocus"/>
                                                                                         
                                                                                             Mask="9999-9999" 
                                                                                             MaskType="Number"
                                                                                              MessageValidatorTip="true"
                                                                                            OnFocusCssClass="MaskedEditFocus"
                                                                                            OnInvalidCssClass="MaskedEditError"
                                                                                            InputDirection="RightToLeft"
                                                                                            AcceptNegative="Left"
                                                                                            DisplayMoney="Left"
                                                                                            ErrorTooltipEnabled="True"/>--%>


                                                                                    </td>
                                                                                    <td>&nbsp;&nbsp;&nbsp;</td>
                                                                                    <td style="width: 50%;">
                                                                                        <asp:DropDownList ID="ddlTipoIdentificador" runat="server" CssClass="form-control" AutoPostBack="false" Width="100%" />
                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label19" CssClass="col-sm-5 control-label" runat="server" Text="Tipo Bienes" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList runat="server" ID="ddlTipoBienes" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoBienes_SelectedIndexChanged" />
                                                                            <asp:HiddenField ID="hddTasacion" runat="server" />
                                                                            <asp:HiddenField ID="hddSeguro" runat="server" />
                                                                            <asp:HiddenField ID="hddAjuste" runat="server" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label14" CssClass="col-sm-5 control-label" runat="server" Text="N° Garantía" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:Label runat="server" ID="lbNGtia" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <%--avales--%>
                                                        <div class="form-horizontal" role="form">
                                                            <div class="row clearfix margenBottom" id="divAvales" runat="server">
                                                                <div class="form-horizontal" role="form">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbRutAval" CssClass="col-sm-5 control-label" runat="server" Text="RUT Aval" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txt_RUTAval" runat="server" MaxLength="8" CssClass="form-control" Width="155px" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                                        </td>

                                                                                        <td>
                                                                                            <asp:TextBox ID="txt_DivAval" runat="server" MaxLength="1" CssClass="form-control" Width="30px" onKeyPress="return solonumerosDV(event)"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>
                                                                                            <asp:LinkButton ID="btnRUTAval" runat="server" CssClass="btn btn-default" OnClick="btnRUTAval_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label id="Label33" runat="server" class="col-sm-5 control-label">Nacionalidad</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txtNacionalidad" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label id="Label35" runat="server" class="col-sm-5 control-label">Profesión</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txtProfesion" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>


                                                                    <div class="col-md-6">
                                                                        <div class="form-group" style="padding-bottom: 6px">
                                                                            <label id="lbRazonSocialtAval" runat="server" class="col-sm-5 control-label">Razón Social Aval</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txtRazonSocialAval" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label id="Label34" runat="server" class="col-sm-5 control-label">Estado Civil</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList runat="server" ID="ddlEdoCivil" CssClass="form-control" ClientIDMode="Static" OnSelectedIndexChanged="ddlEdoCivil_SelectedIndexChanged" AutoPostBack="true">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group" id="divRegimen" style="display: none" runat="server">
                                                                            <label for="inputRegimen" class="col-sm-5 control-label">Regimen:</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList runat="server" ID="ddlRegimen" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-horizontal" role="form">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <asp:Label ID="Label7" CssClass="col-sm-5 control-label" runat="server" Text="Región" Font-Bold="True" />
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList ID="ddlRegiones" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlRegiones_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <asp:Label ID="Label13" CssClass="col-sm-5 control-label" runat="server" Text="Comuna" Font-Bold="True" />
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList runat="server" ID="ddlComunas" CssClass="form-control" />
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <asp:Label ID="Label21" CssClass="col-sm-5 control-label" runat="server" Text="Descripción" Font-Bold="True" />
                                                                    <div class="col-sm-7">
                                                                        <asp:TextBox runat="server" ID="txtDescP" MaxLength="250" TextMode="MultiLine" CssClass="form-control" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-horizontal" role="form">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <asp:Label ID="Label12" CssClass="col-sm-5 control-label" runat="server" Text="Provincia" Font-Bold="True" />
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList runat="server" ID="ddlProvincia" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <asp:Label ID="Label8" CssClass="col-sm-5 control-label" runat="server" Text="Dirección" Font-Bold="True" />
                                                                    <div class="col-sm-7">
                                                                        <asp:TextBox runat="server" ID="txtDireccion" MaxLength="250" CssClass="form-control" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>

                                                    <asp:Panel ID="pnTasacion" runat="server" ClientIDMode="Static">
                                                        <div class="row clearfix margenBottom">
                                                            <div class="form-horizontal" role="form">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label22" CssClass="col-sm-5 control-label" runat="server" Text="Valor Comercial (UF)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtValorC" MaxLength="14" CssClass="form-control" onKeyPress="return soloDecimales(event)" OnTextChanged="txtValorC_TextChanged1" AutoPostBack="true" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label23" CssClass="col-sm-5 control-label" runat="server" Text="Valor Asegurable (UF)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtValorAseg" MaxLength="14" CssClass="form-control" onKeyPress="return soloDecimales(event)" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <asp:Label ID="lb1" CssClass="col-sm-5 control-label" runat="server" Text="N° Tasación" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox ID="txtNroTasacion" runat="server" MaxLength="16" CssClass="form-control"></asp:TextBox>
                                                                            <asp:CheckBox ID="ckbTasacion" runat="server" ToolTip="Requiere Tasación?" />
                                                                            Requiere Tasación?
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label10" CssClass="col-sm-5 control-label" runat="server" Text="Empresa Tasadora" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="ddlEmpresaT" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpresaT_SelectedIndexChanged" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label24" CssClass="col-sm-5 control-label" runat="server" Text="Valor Ajustado (UF)" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:TextBox runat="server" ID="txtValorA" CssClass="form-control" MaxLength="14" onKeyPress="return soloDecimales(event)" ReadOnly="true" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label20" CssClass="col-sm-5 control-label" runat="server" Text="" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <br />
                                                                            <br />
                                                                        </div>
                                                                    </div>

                                                                    <br />

                                                                    <div class="form-group">
                                                                        <asp:Label ID="lbFechaEmi" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Tasación" Font-Bold="True"></asp:Label>
                                                                        <div class="col-sm-7">
                                                                            <SharePoint:DateTimeControl ID="dtcFechaTasacion" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                    <div class="form-group">
                                                                        <asp:Label ID="Label15" CssClass="col-sm-5 control-label" runat="server" Text="Tasador" Font-Bold="True" />
                                                                        <div class="col-sm-7">
                                                                            <asp:DropDownList ID="DdlTasador" runat="server" CssClass="form-control" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>


                                                    <div class="row clearfix margenBottom" style="display:none">
                                                        <div id="historial" class="form-horizontal" role="form" style="margin-left: 12%;">
                                                            <div class="col-md-12 col-sm-12">
                                                                <h4>Historial Tasaciones</h4>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnGuardarTasacion" runat="server" Text="Guardar Tasacion" CssClass="btn btn-success pull-right" OnClientClick="return Dialogo();" OnClick="btnGuardarTasacion_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>

                                                            <div class="col-md-12">
                                                                <div class="form-group">
                                                                    <asp:Label ID="Label26" CssClass="col-sm-7 control-label" runat="server" />
                                                                    <div class="col-sm-12">
                                                                        <table border="0" style="width: 100%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="card-box">
                                                                                        <br>
                                                                                        <div class="table-responsive">
                                                                                            <%--OnRowCreated="GvTasacionHistorial_RowCreated"--%>
                                                                                            <asp:GridView ID="GvTasacionHistorial" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="IdTasacion" ClientIDMode="Static"
                                                                                                OnRowDataBound="GvTasacionHistorial_RowDataBound" Width="100%" ShowHeaderWhenEmpty="true" ShowHeader="true"
                                                                                                PageSize="5" PagerSettings-Visible="true" PagerSettings-Mode="NextPreviousFirstLast" AllowPaging="true" OnPageIndexChanging="GvTasacionHistorial_PageIndexChanging"
                                                                                                OnRowCreated="GvTasacionHistorial_RowCreated">
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="id" Visible="false" />
                                                                                                    <asp:BoundField DataField="IdTasacion" />
                                                                                                    <asp:BoundField DataField="IdGarantia" Visible="false" />
                                                                                                    <asp:BoundField DataField="NroTasacion" HeaderText="N° Tasación" />
                                                                                                    <asp:BoundField DataField="Fechatasacion" HeaderText="Fecha Tasación" />
                                                                                                    <asp:BoundField DataField="DescEmpresaTasadora" HeaderText="Empresa Tasadora" />
                                                                                                    <asp:BoundField DataField="ValorComercial" HeaderText="Valor Comercial" />
                                                                                                    <asp:BoundField DataField="ValorAjustado" HeaderText="Valor Ajustado" />
                                                                                                    <asp:BoundField DataField="ValorAsegurable" HeaderText="Valor Asegurable" />
                                                                                                    <asp:BoundField DataField="Adjuntar" HeaderText="Adjuntar / Descargar Tasación" />
                                                                                                    <asp:BoundField DataField="Accion" HeaderText="Acción" />

                                                                                                    <asp:BoundField DataField="NombreDoc" HeaderText="Documento" Visible="false" />
                                                                                                </Columns>
                                                                                            </asp:GridView>
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

                                                </asp:Panel>


                                                <asp:Panel runat="server" ID="pnOperaciones" CssClass="tab-pane">
                                                    <div id="panel-tab3" class="tab-pane">
                                                        <asp:Panel runat="server" ID="pnSeguro" ClientIDMode="Static">
                                                            <div class="row clearfix margenBottom">
                                                                <div class="form-horizontal" role="form">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label1" CssClass="col-sm-5 control-label" runat="server" Text="N° Poliza" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txtPoliza" runat="server" MaxLength="16" CssClass="form-control" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                                <asp:CheckBox ID="ckbSeguro" runat="server" ToolTip="Requiere Seguro?" ClientIDMode="Static" />
                                                                                Requiere Seguro?
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label3" CssClass="col-sm-5 control-label" runat="server" Text="Vigencia Seguro" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcVigenciaSeguro" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label5" CssClass="col-sm-5 control-label" runat="server" Text="Compañía Seguro" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlCompaniaSeguro" runat="server" CssClass="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label2" CssClass="col-sm-5 control-label" runat="server" Text="Contratante" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlContratante" runat="server" CssClass="form-control">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label4" CssClass="col-sm-5 control-label" runat="server" Text="Vencimiento Seguro" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcVencSeguro" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label6" CssClass="col-sm-5 control-label" runat="server" Text="Cobertura Seguro" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:CheckBoxList ID="ckbCoberturaSeguro" runat="server">
                                                                                    <asp:ListItem>Robo</asp:ListItem>
                                                                                    <asp:ListItem>Incendio</asp:ListItem>
                                                                                    <asp:ListItem>Terremoto</asp:ListItem>
                                                                                    <asp:ListItem>Inundación</asp:ListItem>
                                                                                    <asp:ListItem>Desgravamen</asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <label class="col-sm-5 control-label">Monto Seguro</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox runat="server" ID="txtMontoSeguro" MaxLength="14" CssClass="form-control" onKeyPress="return soloDecimales(event)"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <label class="col-sm-5 control-label">N° Certificado</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlCertificadosEmitir" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                     <div class="form-group">
                                                                            <label class="col-sm-5 control-label">Estado Seguro</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlEstadoSeg" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                                                                             </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbObservacioS" CssClass="col-sm-5 control-label" Style="text-align: right" runat="server" Text="Observación Seguro" Font-Bold="True"></asp:Label>

                                                                            <div class="col-sm-7">
                                                                                <textarea rows="5" cols="20" runat="server" id="txtObsSeguro" class="form-control"></textarea>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row clearfix margenBottom" style="visibility:hidden">
                                                                <div id="Div1" class="form-horizontal" role="form" style="margin-left: 12%;">
                                                                    <div class="col-md-12 col-sm-12">
                                                                        <h4>Historial Seguros</h4>
                                                                    </div>

                                                                    <div class="col-md-12" style="visibility:hidden">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnGuardarSeguro" runat="server" Text="Guardar Seguro" CssClass="btn btn-success pull-right" OnClientClick="return Dialogo();" OnClick="btnGuardarSeguro_Click" ClientIDMode="Static" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>

                                                                    <div class="col-md-12" style="visibility:hidden">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label31" CssClass="col-sm-7 control-label" runat="server" />
                                                                            <div class="col-sm-12">
                                                                                <table border="0" style="width: 100%;">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <div class="card-box">
                                                                                                <br>
                                                                                                <div class="table-responsive">
                                                                                                    <asp:GridView ID="GvHistorialSeguros" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="IdSeguro" ClientIDMode="Static"
                                                                                                        OnRowDataBound="GvHistorialSeguros_RowDataBound" Width="100%" ShowHeaderWhenEmpty="true" PagerSettings-Visible="true" PagerSettings-Mode="NextPreviousFirstLast" PageSize="10"
                                                                                                        OnPageIndexChanging="GvHistorialSeguros_PageIndexChanging"
                                                                                                        OnRowCreated="GvHistorialSeguros_RowCreated">
                                                                                                        <Columns>
                                                                                                            <asp:BoundField DataField="IdSeguro" />
                                                                                                            <asp:BoundField DataField="IdGarantia" />
                                                                                                            <asp:BoundField DataField="NumPoliza" HeaderText="N° Poliza" />
                                                                                                            <asp:BoundField DataField="DescContratante" HeaderText="Empresa Contratante" />
                                                                                                            <asp:BoundField DataField="DescEmpresAseguradora" HeaderText="Empresa Aseguradora" />
                                                                                                            <asp:BoundField DataField="NroCertificado" HeaderText="Nª Certificado" />
                                                                                                            <asp:BoundField DataField="MontoSeguro" HeaderText="Valor Seguro" />
                                                                                                            <asp:BoundField DataField="fecInicioSeguro" HeaderText="fecha Inicio Seguro" />
                                                                                                            <asp:BoundField DataField="fecVencSeguro" HeaderText="fecha Vencimiento Seguro" />
                                                                                                            <asp:BoundField DataField="Accion" HeaderText="Accion" />
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
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
                                                        </asp:Panel>
                                                    </div>

                                                    <div id="panel-tab4" class="tab-pane">
                                                        <asp:Panel runat="server" ID="pnConstitucion" ClientIDMode="Static">
                                                            <div class="row clearfix margenBottom">
                                                                <div class="form-horizontal" role="form">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbeg" CssClass="col-sm-5 control-label" runat="server" Text="Estado Garantía" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList runat="server" ID="ddlEdoGarantia" CssClass="form-control" />
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label17" CssClass="col-sm-5 control-label" runat="server" Text="Límite" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlLimite" CssClass="form-control" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label25" CssClass="col-sm-5 control-label" runat="server" Text="Participacion" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlParticipacion" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlParticipacion_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label32" CssClass="col-sm-5 control-label" runat="server" Text="RUT Garante" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <table style="width: 100%;">
                                                                                    <tr>
                                                                                        <td style="width: 70%;">
                                                                                            <asp:TextBox ID="txt_RutGarante" runat="server" MaxLength="8" CssClass="form-control" Style="width: 90%;" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>
                                                                                            <div class="input-group">
                                                                                                <asp:TextBox ID="txt_DivRutGarante" runat="server" MaxLength="1" CssClass="form-control" Style="width: 90%" onKeyPress="return solonumerosDV(event)"></asp:TextBox>
                                                                                                <span style="color: red;" id="errorRut"></span>
                                                                                                <span class="input-group-btn">
                                                                                                    <asp:LinkButton ID="btnRutGarante" runat="server" Text="Buscar" OnClick="btnRutGarante_Click" CssClass="btn btn-default" OnClientClick=" return Dialogo();">
                                                                                                        <i class="fa fa-search"></i>
                                                                                                    </asp:LinkButton>
                                                                                                </span>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbFechaVencimiento" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Inscripción" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaInsc" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label28" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Alzamiento" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaAlza" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label></label>
                                                                            <div class="col-sm-7">
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label class="col-sm-5 control-label">Empresa Acreedora</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList runat="server" ID="ddlEmpresaAcreedora" CssClass="form-control" ClientIDMode="Static" />
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label class="col-sm-5 control-label">Observación Estado</label>
                                                                            <div class="col-sm-7">
                                                                                <textarea rows="5" runat="server" id="txtObservacionEdo" class="form-control"></textarea>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbtipooperac" CssClass="col-sm-5 control-label" runat="server" Text="Grado Preferencia" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="dllGradoPrefe" CssClass="form-control" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label16" CssClass="col-sm-5 control-label" runat="server" Text="Carácter" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList ID="ddlCaracter" CssClass="form-control" runat="server">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbNroCredito" CssClass="col-sm-5 control-label" runat="server" Text="Monto Límite" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txtMontoLimite" CssClass="form-control" MaxLength="18" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <label for="inputEmail3" class="col-sm-5 control-label">Razón Social Garante</label>
                                                                            <div class="col-sm-7">
                                                                                <asp:TextBox ID="txt_RazonSocialGarante" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label27" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Constitucion" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaConst" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label29" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Contrato Garantía" Font-Bold="True" />
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaContrato" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control input-sm col-md5" EnableViewState="true" DateOnly="true" />
                                                                                <asp:CheckBox ID="ckbContrato" runat="server" ToolTip="Requiere Contrato" />
                                                                                Requiere Contrato Garantía?
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label30" CssClass="col-sm-5 control-label" runat="server" Text="Sub Estado Garantía" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <asp:DropDownList runat="server" ID="ddlSubEdoGarantia" CssClass="form-control" />
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div id="panel-tab5" class="tab-pane">
                                                        <asp:Panel runat="server" ID="pnCorfo" ClientIDMode="Static">
                                                            <div class="row clearfix margenBottom">
                                                                <div class="form-horizontal" role="form">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label9" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Informe a CORFO" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcFechaInfCORFO" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                                <asp:CheckBox ID="ckbInfCORFO" runat="server" ToolTip="Utiliza para solicitar Informe a CORFO" />
                                                                                Utilizada para solicitar Informe a CORFO?
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group">
                                                                            <asp:Label ID="Label11" CssClass="col-sm-5 control-label" Style="text-align: right" runat="server" Text="Observación" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <textarea rows="5" cols="20" runat="server" id="txtObservacion" class="form-control"></textarea>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <div class="form-group">
                                                                            <asp:Label ID="lbCodigoSafio" CssClass="col-sm-5 control-label" runat="server" Text="Fecha Giro CORFO" Font-Bold="True"></asp:Label>
                                                                            <div class="col-sm-7">
                                                                                <SharePoint:DateTimeControl ID="dtcGechaGiroCORFO" runat="server" Calendar="Gregorian" LocaleId="13322" CssClassTextBox="form-control" EnableViewState="true" DateOnly="true" />
                                                                                <asp:CheckBox ID="ckbGiroCORFO" runat="server" ToolTip="Utiliza para solicitar giro a CORFO" />
                                                                                Utilizada para solicitar giro a CORFO?
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                    <div id="panel-tab6" class="tab-pane">
                                                        <asp:Panel ID="pnContribuciones" runat="server">
                                                            <div class="row clearfix margenBottom">
                                                                <div class="form-horizontal" role="form">
                                                                    <div id="divContribuciones" class="col-md-12">
                                                                        <table border="0" style="table-layout: fixed; width: 100%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="card-box">
                                                                                        <br>
                                                                                        <div class="table-responsive">
                                                                                            <%--ShowFooter="true" --%>
                                                                                            <asp:GridView ID="gvContribuciones" runat="server" PageSize="20" ShowHeaderWhenEmpty="true" CssClass="table table-bordered table-hover"
                                                                                                EmptyDataText="No registra contribuciones impagas" ShowHeader="true" AutoGenerateColumns="false"
                                                                                                OnRowDataBound="gvContribuciones_RowDataBound" OnDataBound="gvContribuciones_DataBound">
                                                                                                <Columns>
                                                                                                    <asp:BoundField HeaderText="Region" DataField="DescRegion" Visible="false" />
                                                                                                    <asp:BoundField HeaderText="Comuna" DataField="DescComuna" />
                                                                                                    <asp:BoundField HeaderText="Rol" DataField="Rol" />
                                                                                                    <asp:BoundField HeaderText="N° Cuota Año" DataField="CuotaAnio" />
                                                                                                    <asp:BoundField HeaderText="Valor Cuota dentro del plazo" DataField="ValorCuotaPlazo" />
                                                                                                    <asp:BoundField HeaderText="Fecha Vencimiento" DataField="FechaVencimiento" />
                                                                                                    <asp:BoundField HeaderText="Total a Pagar" DataField="TotalPagar" />
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>

                                                </asp:Panel>

                                            </div>
                                        </div>
                                    </div>

                                    <asp:Panel ID="PnlBotones" runat="server">
                                        <div class="row" style="padding: 10px;">
                                            <div class="col-md-12">
                                                <table border="0" style="table-layout: fixed; width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <div class="card-box">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="Button1" CssClass="btn btn-default pull-left" runat="server" Text=" < Volver Atrás" OnClick="btnAtras_Click" OnClientClick="return Dialogo();" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-primary pull-right" OnClientClick="return Dialogo();" />
                                                                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-warning pull-right" />
                                                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success pull-right" OnClick="btnGuardar_Click" OnClientClick="return Dialogo();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br>

                                                                <div>
                                                                    <div class="form-group">
                                                                        <div class="panel-heading" role="tab" id="headingOne">
                                                                            <h4 class="panel-title">
                                                                                <a role="button">Garantías Constituidas / En Tramite / Sin Estado
                                                                                </a>
                                                                            </h4>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="card-box">
                                                                    <div class="row">
                                                                        <div class="table-responsive">
                                                                            <asp:GridView ID="ResultadosBusqueda" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="false" ShowFooter="true"
                                                                                OnRowCreated="ResultadosBusqueda_RowCreated" OnRowDataBound="ResultadosBusqueda_RowDataBound" ShowHeaderWhenEmpty="true"
                                                                                EmptyDataText="Sin Datos">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="IdGarantia" HeaderText="IdGarantia" />
                                                                                    <asp:BoundField DataField="Nro. Inscripción" HeaderText="Nro. Inscripción" />
                                                                                    <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                                                                                    <asp:BoundField DataField="Clase Garantía" HeaderText="Clase Garantía" />
                                                                                    <asp:BoundField DataField="Tipo Bien" HeaderText="Tipo Bien" />
                                                                                    <asp:BoundField DataField="Valor Ajustado" HeaderText="Valor Ajustado" />
                                                                                    <asp:BoundField DataField="Valor Comercial" HeaderText="Valor Comercial" />
                                                                                    <asp:BoundField DataField="Valor Asegurable" HeaderText="Valor Asegurable" />

                                                                                    <asp:BoundField DataField="Acción" HeaderText="Acción" />
                                                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                                                    <asp:BoundField DataField="Preferencia" HeaderText="Grado Preferencia" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div>
                                                                    <div class="form-group">
                                                                        <div class="panel-heading" role="tab" id="Div2">
                                                                            <h4 class="panel-title">
                                                                                <a role="button">Garantías Alzadas
                                                                                </a>
                                                                            </h4>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="card-box">
                                                                    <div class="row">
                                                                        <div class="table-responsive">
                                                                            <asp:GridView ID="ResultadosBusquedaAlzada" runat="server" CssClass="table table-bordered table-hover" ShowFooter="True" AutoGenerateColumns="false"
                                                                                OnRowCreated="ResultadosBusquedaAlzada_RowCreated" OnRowDataBound="ResultadosBusquedaAlzada_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataText="Sin Datos">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="IdGarantia" HeaderText="IdGarantia" />
                                                                                    <asp:BoundField DataField="Nro. Inscripción" HeaderText="Nro. Inscripción" />
                                                                                    <asp:BoundField DataField="Descripción" HeaderText="Descripción" />
                                                                                    <asp:BoundField DataField="Clase Garantía" HeaderText="Clase Garantía" />
                                                                                    <asp:BoundField DataField="Tipo Bien" HeaderText="Tipo Bien" />
                                                                                    <asp:BoundField DataField="Valor Ajustado" HeaderText="Valor Ajustado" />
                                                                                    <asp:BoundField DataField="Valor Comercial" HeaderText="Valor Comercial" />
                                                                                    <asp:BoundField DataField="Valor Asegurable" HeaderText="Valor Asegurable" />

                                                                                    <asp:BoundField DataField="Acción" HeaderText="Acción" />
                                                                                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                                                    <asp:BoundField DataField="Preferencia" HeaderText="Grado Preferencia" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</div>
