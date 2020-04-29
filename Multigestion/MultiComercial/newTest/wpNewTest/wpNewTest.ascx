<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpNewTest.ascx.cs" Inherits="MultiComercial.newTest.wpNewTest.wpNewTest" %>

    <body class="fixed-left">

        <!-- Begin page -->
        <div id="wrapper">

            <!-- Top Bar Start -->

            <!-- ========== Left Sidebar Start ========== -->


            <!-- Left Sidebar End -->



            <!-- ============================================================== -->
            <!-- Cuerpo -->
            <!-- ============================================================== -->
            <div class="content-page" style="margin-left:0px;">
                <!-- Start content -->
                <div class="content">
                    <div class="container">

                        <!-- Page-Title -->
                        <div class="row marginInicio">
                            <div class="col-sm-12">
                                <h4 class="page-title">Comercial</h4>
                                <ol class="breadcrumb">
									<li>
										<a href="#">Inicio</a>
									</li>
									<li class="active">
										Comercial
									</li>
								</ol>
                            </div>
                        </div>

                        <!-- botones -->
                        <!--div class="row">
	                        <div class="col-md-12 text-right">
                                <div class="card-box">
    	                        	<button type="button" class="btn btn-default">
                                	</span>Compromisos Semanal</button>
                                	<button type="button" class="btn btn-default">
                                	<span class="btn-label"><i class="fa fa-plus"></i>
                                	</span>Crear Nueva Empresa</button>
                                </div>
                            </div>
                        </div-->

						
						<!-- CABECERA ERG.22-09-2016 -->
							<!-- filtros-->
							<div class="row">
								<div class="col-md-12">
									<div class="card-box">
										<div class="row">
										
											<div class="col-md-3 col-sm-6">
												<label for="">Buscar</label>
												<div class="input-group">
													<input id="" name="" class="form-control" placeholder="Buscar" type="text">
													<span class="input-group-btn">
													<button type="button" class="btn btn-default"><i class="fa fa-search"></i></button>
													</span>
												</div>
											</div>
											<div class="col-md-3 col-sm-6">
												<label for="">Estado</label>
												<select class="form-control">
													<option>- Seleccionar Estado -</option>
													<option>1</option>
													<option>2</option>
													<option>3</option>
												</select>
											</div>
											<div class="col-md-3 col-sm-6">
												<label>Etapa</label>
												<select class="form-control">
													<option>- Seleccionar Etapa -</option>
													<option>1</option>
													<option>2</option>
													<option>3</option>
												</select>
											</div>
											<div class="col-md-3 col-sm-6">
												<label>SubEtapa</label>
												<select class="form-control">
													<option>- Seleccionar SubEtapa -</option>
													<option>1</option>
													<option>2</option>
													<option>3</option>
												</select>
											</div>
									
										</div>
									</div>
								</div>
							</div>
						<!-- END CABECERA ERG.22-09-2016 -->
						
                        <!-- tabla / grilla -->
                        <div class="row">            						
                        	<div class="col-md-12">
                        		<div class="card-box">
                        			<div class="table-responsive">
	                        			<table class="table table-bordered table-hover">
											<thead>
												<tr>
													<th>
														Cliente
													</th>
													<th>
														Operación
													</th>
													<th>
														Monto
													</th>
													<th>
														Etapa
													</th>
													<th>
														SubEtapa
													</th>
													<th>
														Estado
													</th>
													<th>
														Datos Empresa
													</th>
													<th>
														Datos Operación
													</th>
													<th>
														Datos Adm. Empresa
													</th>
													<th>
														Acción
													</th>
												</tr>
											</thead>
											<tbody>
												<tr>
													<td>
														<a href="" class="text-custom"><i class="glyphicon glyphicon-plus-sign" data-toggle="tooltip" data-placement="top" data-original-title="Agregar un negocio"></i></a>
														Sociedad Agrícola Montero y Montero Limitada
													</td>
													<td>
														5452 - Certificado Fianza Comercial
													</td>
													<td>
														CLP 216.000.000
													</td>
													<td>
														Prospecto
													</td>
													<td>
														<span class="label label-inverse">Caducado</span>
													</td>
													<td>
														Ingresado
													</td>
													<td>
														<a href="" class="text-custom"><i class="md md-info-outline padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Información de la Empresa"></i></a>
														<a href="" class="text-custom"><i class="md md-assignment padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Historia Empresa"></i></a>
														<a href="" class="text-custom"><i class="md md-business padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Empresas Relacionadas"></i></a>
														<a href="" class="text-custom"><i class="md md-loop padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Socios y Asociados"></i></a>
														<a href="" class="text-custom"><i class="md md-work padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Directorio"></i></a>
													</td>
													<td>
														<a href="" class="text-custom"><i class="md md-local-atm padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Garantías"></i></a>
														<a href="" class="text-custom"><i class="md md-folder-open padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Documentos"></i></a>
														<a href="" class="text-custom"><i class="ion-gear-b padIconos" data-toggle="tooltip" data-placement="top" data-original-title="Configuración"></i></a>
													</td>
													<td>

													</td>
													<td>

													</td>
												</tr>
												<tr>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
													<td>
														[Contenido]
													</td>
												</tr>
											</tbody>
										</table>
									</div>
                        		</div>
                        	</div>
                        </div>
                        <!-- end row -->

                    </div> <!-- container -->

                </div> <!-- content -->
            </div>


            <!-- ============================================================== -->
            <!-- End Right content here -->
            <!-- ============================================================== -->

        </div>
        <!-- END wrapper -->

    </body>