<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpRendicionAAT.ascx.cs" Inherits="MultiContabilidad.wpRendicionAAT.wpRendicionAAT.wpRendicionAAT" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script src="../../_layouts/15/MultiContabilidad/wpValidaciones.js"></script>

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
</script>


 <div id="dvWarning1" runat="server" style="display: none" class="alert alert-warning" role="alert">
                                        <h4>
                                            <asp:Label ID="lbWarning1" runat="server" Text=""></asp:Label>
                                        </h4>
                                    </div>

									
									<div id="dvFormulario" runat="server">
<div class="row clearfix">
    <div class="col-md-6 column">
        <div class="btn-group btn-breadcrumb">
            <a class="btn btn-warning sinMargen"><i class="glyphicon glyphicon-home"></i>USTED ESTÁ EN:</a>
            <a class="btn btn-primary sinMargen">Rendiciones</a>
            <a class="btn btn-info sinMargen">Rendición AAT</a>
        </div>
    </div>
    <div class="col-md-6 column" style="text-align: right;">
        <!-- Identificador de empresa -->
        <h3 class="sinMargen">Usuario: &nbsp;<asp:Label ID="lbUsuario" runat="server" Text="Pedro Peréz"></asp:Label></h3>


        <h5>Area:&nbsp;<asp:Label ID="lbArea" runat="server" Text="Comercial"></asp:Label>
        </h5>
    </div>
</div>

<!-- Contenedor Tabs -->
<div class="row clearfix margenBottom">
    <div class="col-md-12 column">
        <div class="tabbable" id="tabs-empRelacionadas">
            <ul class="nav nav-tabs">
                <li class="active">
                    <asp:LinkButton ID="LinkButton8" runat="server" OnClientClick="return Dialogo();">Rendición AAT</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LinkButton9" runat="server" OnClientClick="return Dialogo();">Rendición Gastos</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LinkButton10" runat="server" OnClientClick="return Dialogo();">Servicios Fiscalía</asp:LinkButton>
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
                        <h5>Información de Rendición</h5>
                        <div class="form-horizontal" role="form">
                            <!-- Grupo 1 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">Número:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">Objetivo:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">Fecha Emisión:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">Tipo Fondo:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txt_porcVentas" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <h5>Información del Empleado</h5>

                        <div class="form-horizontal" role="form">
                            <!-- Grupo 1 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">Nombre:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">Departamento:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">RUT:</label>
                                    <div class="col-sm-7">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_rut" runat="server" MaxLength="8" CssClass="form-control" Width="155px" onKeyPress="return solonumeros(event)"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txt_divRut" runat="server" MaxLength="1" CssClass="form-control" Width="30px" onKeyPress="return solonumerosDV(event)"></asp:TextBox>

                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:ImageButton ID="BuscarRUT" runat="server" Height="24px" ImageUrl="../../_layouts/15/images/MultiContabilidad/Buscar.png" Width="24px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">Supervisor:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                        </div>


                        <h5>Información de Aprobación</h5>

                        <div class="form-horizontal" role="form">
                            <!-- Grupo 1 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtAprobador" class="col-sm-5 control-label">Aprobado Por:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txtDeposito" class="col-sm-5 control-label">Depositar en:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox8" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                            <!-- Grupo 2 -->
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-5 control-label">V° B° Supervisor:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox9" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="inputPassword3" class="col-sm-5 control-label">Cuenta:</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="TextBox11" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                        </div>
                     </asp:Panel>
                </div>
                <!-- fin  -->
            </div>
        </div>
    </div>
</div>

  <dx:ASPxGridView ID="Grid" runat="server" EnableRowsCache="false" 
     KeyFieldName="idDetalleResdicionATT" ClientInstanceName="grid" Width="100%" Theme="Material" Settings-HorizontalScrollBarMode="Auto">
        <Columns>
            <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="True" AllowTextTruncationInAdaptiveMode="true" FixedStyle="Left" />

            <dx:GridViewDataTextColumn FieldName="descTipoCompra" Caption="TIPO" FixedStyle="Left">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataTextColumn FieldName="descCuentaContable" Caption="CUENTA CONTABLE" AllowTextTruncationInAdaptiveMode="true" FixedStyle="Left" 
                HeaderStyle-Wrap="True" >
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataColumn FieldName="fechaCompra" Caption="FECHA  COMPRA"  HeaderStyle-Wrap="True">
                <EditFormSettings VisibleIndex="3" />
            </dx:GridViewDataColumn>

          

            <dx:GridViewDataTextColumn FieldName="descProveedor" Caption="PROVEEDOR"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="nroTransaccion" Caption="N° BOLETAS"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>


            <dx:GridViewDataMemoColumn FieldName="detalleCompra" Caption="DETALLE  COMPRA"  HeaderStyle-Wrap="True">
                <EditFormSettings RowSpan="2" Visible="True" VisibleIndex="2" />
            </dx:GridViewDataMemoColumn>


            <dx:GridViewDataTextColumn FieldName="rutProveedor" Caption="RUT CLIENTE"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="cliente" Caption="CLIENTE"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="codCuenta" Caption="COD. CTA."  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>
             
            <dx:GridViewDataTextColumn FieldName="centroCosto" Caption="CENTRO COSTO"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="medioPago" Caption="MEDIO PAGO"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="TotalCLP" Caption="TOTAL USD"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="TotalUSD" Caption="TOTAL CLP"  HeaderStyle-Wrap="True">
                <PropertiesTextEdit>
                    <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" />
                </PropertiesTextEdit>
            </dx:GridViewDataTextColumn>


        </Columns>
        <SettingsCommandButton>
            <NewButton Text="nuevo" RenderMode="Link"></NewButton>
            <DeleteButton Text="eliminar" RenderMode="Link"></DeleteButton>

        </SettingsCommandButton>
        <SettingsEditing Mode="Batch" />

        <Settings ShowFooter="True" />
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="idDetalleResdicionATT" SummaryType="Count" ShowInColumn="descTipoCompra"/>
            <dx:ASPxSummaryItem FieldName="TotalCLP" SummaryType="Sum"  ShowInColumn="TotalCLP"/>
           <dx:ASPxSummaryItem FieldName="TotalUSD" SummaryType="Sum"  ShowInColumn="TotalUSD"/>
        </TotalSummary>
    </dx:ASPxGridView>


<br />
<br />
<div id="dv_buscarEmpresas" runat="server" class="row clearfix">
    <div class="col-md-12 column">
        <br />
        <asp:HiddenField runat="server" ID="hdnIdEmpresaClientes" />

    </div>

</div>

                                        </div>