using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace ClasesServicios
{
    public class Siisa
    {
        private ConsultaReporteComercialMultiaval ConsultaReporte { get; set; }
        public ConsultaReporteComercialMultiavalData ResultadoReporte { get; set; }

        public Siisa()
        {
            ConsultaReporte = new ConsultaReporteComercialMultiaval();
        }

        [GeneratedCode("wsdl", "4.6.1055.0")]
        [DebuggerStepThrough()]
        [DesignerCategory("code")]
        [WebServiceBinding(Name = "ConsultaReporteComercialMultiavalSoap11Binding", Namespace = "http://ReporteComercialMultiaval.Informes.ws.siisa.com")]
        public partial class ConsultaReporteComercialMultiaval : SoapHttpClientProtocol
        {
            private SendOrPostCallback getReporteComercialOperationCompleted;

            public ConsultaReporteComercialMultiaval()
            {
                Url = "http://216.72.170.107:9071/publico2/services/ConsultaReporteComercialMultiaval.Co" + "nsultaReporteComercialMultiavalHttpSoap11Endpoint/";
            }

            public event getReporteComercialCompletedEventHandler getReporteComercialCompleted;

            [SoapDocumentMethod("urn:getReporteComercial", RequestNamespace = "http://ReporteComercialMultiaval.Informes.ws.siisa.com",
                ResponseNamespace = "http://ReporteComercialMultiaval.Informes.ws.siisa.com",
                Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]

            [return: XmlElement("return", IsNullable = true)]
            public ConsultaReporteComercialMultiavalData getReporteComercial([XmlElement(IsNullable = true)] string RutCli, [XmlElement(IsNullable = true)] string DigCli,
                [XmlElement(IsNullable = true)] string RutUser, [XmlElement(IsNullable = true)] string DigUser,
                [XmlElement(IsNullable = true)] string Password, [XmlElement(IsNullable = true)] string RutCons,
                [XmlElement(IsNullable = true)] string DigCons, [XmlElement(IsNullable = true)] string CodAuditoria)
            {
                object[] results = Invoke("getReporteComercial", new object[] {
                RutCli,
                DigCli,
                RutUser,
                DigUser,
                Password,
                RutCons,
                DigCons,
                CodAuditoria});
                return ((ConsultaReporteComercialMultiavalData)(results[0]));
            }

            public IAsyncResult BegingetReporteComercial(string RutCli, string DigCli, string RutUser, string DigUser, string Password, string RutCons, string DigCons, string CodAuditoria, AsyncCallback callback, object asyncState)
            {
                return BeginInvoke("getReporteComercial", new object[] {
                RutCli,
                DigCli,
                RutUser,
                DigUser,
                Password,
                RutCons,
                DigCons,
                CodAuditoria}, callback, asyncState);
            }

            public ConsultaReporteComercialMultiavalData EndgetReporteComercial(IAsyncResult asyncResult)
            {
                object[] results = EndInvoke(asyncResult);
                return ((ConsultaReporteComercialMultiavalData)(results[0]));
            }

            public void getReporteComercialAsync(string RutCli, string DigCli, string RutUser, string DigUser, string Password, string RutCons, string DigCons, string CodAuditoria)
            {
                getReporteComercialAsync(RutCli, DigCli, RutUser, DigUser, Password, RutCons, DigCons, CodAuditoria, null);
            }

            public void getReporteComercialAsync(string RutCli, string DigCli, string RutUser, string DigUser, string Password, string RutCons, string DigCons, string CodAuditoria, object userState)
            {
                if ((getReporteComercialOperationCompleted == null))
                {
                    getReporteComercialOperationCompleted = new System.Threading.SendOrPostCallback(OngetReporteComercialOperationCompleted);
                }
                InvokeAsync("getReporteComercial", new object[] {
                RutCli,
                DigCli,
                RutUser,
                DigUser,
                Password,
                RutCons,
                DigCons,
                CodAuditoria}, getReporteComercialOperationCompleted, userState);
            }

            private void OngetReporteComercialOperationCompleted(object arg)
            {
                if ((getReporteComercialCompleted != null))
                {
                    InvokeCompletedEventArgs invokeArgs = ((InvokeCompletedEventArgs)(arg));
                    getReporteComercialCompleted(this, new getReporteComercialCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
                }
            }

            public new void CancelAsync(object userState)
            {
                base.CancelAsync(userState);
            }
        }

        [GeneratedCode("wsdl", "4.6.1055.0")]
        [Serializable()]
        [DebuggerStepThrough()]
        [DesignerCategory("code")]
        [XmlType(Namespace = "http://ReporteComercialMultiaval.Informes.ws.siisa.com/xsd")]
        public partial class ConsultaReporteComercialMultiavalData
        {
            private Giros[] detalleGirosField;
            private ResumenReporte resumenField;

            [XmlElement("detalleGiros", IsNullable = true)]
            public Giros[] detalleGiros
            {
                get
                {
                    return detalleGirosField;
                }
                set
                {
                    detalleGirosField = value;
                }
            }

            [XmlElement(IsNullable = true)]
            public ResumenReporte resumen
            {
                get
                {
                    return resumenField;
                }
                set
                {
                    resumenField = value;
                }
            }
        }

        [GeneratedCode("wsdl", "4.6.1055.0")]
        [Serializable()]
        [DebuggerStepThrough()]
        [DesignerCategory("code")]
        [XmlType(Namespace = "http://Estructuras.ws.siisa.com/xsd")]
        public partial class Giros
        {
            private string codigoGiroField;
            private string nombreGiroField;

            [XmlElement(IsNullable = true)]
            public string codigoGiro
            {
                get
                {
                    return codigoGiroField;
                }
                set
                {
                    codigoGiroField = value;
                }
            }

            [XmlElement(IsNullable = true)]
            public string nombreGiro
            {
                get
                {
                    return nombreGiroField;
                }
                set
                {
                    nombreGiroField = value;
                }
            }
        }

        [GeneratedCode("wsdl", "4.6.1055.0")]
        [Serializable()]
        [DebuggerStepThrough()]
        [DesignerCategory("code")]
        [XmlType(Namespace = "http://ReporteComercialMultiaval.Informes.ws.siisa.com/xsd")]
        public partial class ResumenReporte
        {
            /*
         => Vigencia Actualización: 
         - Boletin Comercial : Cheques, Letras, Pagares y Cuotas Morosas => Semanal
         - Laboral/Infracciones : Dirección del Trabajo (Multas e Infracciones) => Diario
         - Infocom : Camara Comercial (Retail, Casas Comerciales, Casa Compensación, Etc.) => En Linea
         - MIC : Morosidad Industria Comercial (SIISA / Sinacofi / Facturas) => Diario
         */

            private string boletinComercialField; //Si o No
            private string boletinLaboralField; //Si o No
            private string cantidadBoletinComercialField; //Números de documentos publicados
            private string cantidadInfocomField; //Números de documentos publicados
            private string cantidadInfraccionesField; //Números de documentos publicados
            private string cantidadMICField; //Números de documentos publicados
            private string cantidadMultasField; //Números de documentos publicados
            private string codigoAuditoriaField; // 0 Default : Codigo autorización Persona Natural
            private string errorInfocomField; // != 0 Error ... Interno Infocom (No contesta)
            private string fechaField; //Fecha que se realiza la consulta
            private string fechaInicioActividadField; // Fecha Inicio de Actividades
            private string horaField; //Hora que se realiza la consulta
            private string infocomField; //Si o No
            private string mensajeLegalField; //Mensaje Interno Legal, Ley.
            private string montoBoletinComercialField; //En caso Si => Monto Boletin Comercial
            private string montoInfocomField; //En caso Si => Monto Infocom
            private string montoInfraccionesField; //En caso Si => Monto Infracción
            private string montoMICField; //En caso Si => Monto MIC
            private string montoMultasField; //En caso Si => Monto Multas
            private string morosidadIndustriaComercioField; // MIC: Si o No 
            private string nombreField; //Razon Social o Nombre de la Empresa
            private string numReporteField; //Número Interno de Reporte.
            private string rutField; //Rut Razon Social o Nombre de la Empresa
            private string segmentoContribuyenteField; //Nomesclatura != 
            private string tramoField;
            private string transaccionesDisponiblesField;

            [XmlElement(IsNullable = true)]
            public string boletinComercial
            {
                get
                {
                    return boletinComercialField;
                }
                set
                {
                    boletinComercialField = value;
                }
            }

            [XmlElement(IsNullable = true)]
            public string boletinLaboral
            {
                get
                {
                    return boletinLaboralField;
                }
                set
                {
                    boletinLaboralField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string cantidadBoletinComercial
            {
                get
                {
                    return cantidadBoletinComercialField;
                }
                set
                {
                    cantidadBoletinComercialField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string cantidadInfocom
            {
                get
                {
                    return cantidadInfocomField;
                }
                set
                {
                    cantidadInfocomField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string cantidadInfracciones
            {
                get
                {
                    return cantidadInfraccionesField;
                }
                set
                {
                    cantidadInfraccionesField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string cantidadMIC
            {
                get
                {
                    return cantidadMICField;
                }
                set
                {
                    cantidadMICField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string cantidadMultas
            {
                get
                {
                    return cantidadMultasField;
                }
                set
                {
                    cantidadMultasField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string codigoAuditoria
            {
                get
                {
                    return codigoAuditoriaField;
                }
                set
                {
                    codigoAuditoriaField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string errorInfocom
            {
                get
                {
                    return errorInfocomField;
                }
                set
                {
                    errorInfocomField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string fecha
            {
                get
                {
                    return fechaField;
                }
                set
                {
                    fechaField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string fechaInicioActividad
            {
                get
                {
                    return fechaInicioActividadField;
                }
                set
                {
                    fechaInicioActividadField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string hora
            {
                get
                {
                    return horaField;
                }
                set
                {
                    horaField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string infocom
            {
                get
                {
                    return infocomField;
                }
                set
                {
                    infocomField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string mensajeLegal
            {
                get
                {
                    return mensajeLegalField;
                }
                set
                {
                    mensajeLegalField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string montoBoletinComercial
            {
                get
                {
                    return montoBoletinComercialField;
                }
                set
                {
                    montoBoletinComercialField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string montoInfocom
            {
                get
                {
                    return montoInfocomField;
                }
                set
                {
                    montoInfocomField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string montoInfracciones
            {
                get
                {
                    return montoInfraccionesField;
                }
                set
                {
                    montoInfraccionesField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string montoMIC
            {
                get
                {
                    return montoMICField;
                }
                set
                {
                    montoMICField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string montoMultas
            {
                get
                {
                    return montoMultasField;
                }
                set
                {
                    montoMultasField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string morosidadIndustriaComercio
            {
                get
                {
                    return morosidadIndustriaComercioField;
                }
                set
                {
                    morosidadIndustriaComercioField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string nombre
            {
                get
                {
                    return nombreField;
                }
                set
                {
                    nombreField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string numReporte
            {
                get
                {
                    return numReporteField;
                }
                set
                {
                    numReporteField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string rut
            {
                get
                {
                    return rutField;
                }
                set
                {
                    rutField = value;
                }
            }


            [XmlElement(IsNullable = true)]
            public string segmentoContribuyente
            {
                get
                {
                    return segmentoContribuyenteField;
                }
                set
                {
                    segmentoContribuyenteField = value;
                }
            }

            [XmlElement(IsNullable = true)]
            public string transaccionesDisponibles
            {
                get
                {
                    return transaccionesDisponiblesField;
                }
                set
                {
                    transaccionesDisponiblesField = value;
                }
            }

            [XmlElement(IsNullable = true)]
            public string tramo
            {
                get
                {
                    return tramoField;
                }
                set
                {
                    tramoField = value;
                }
            }
        }

        [GeneratedCode("wsdl", "4.6.1055.0")]
        public delegate void getReporteComercialCompletedEventHandler(object sender, getReporteComercialCompletedEventArgs e);

        [GeneratedCode("wsdl", "4.6.1055.0")]
        [DebuggerStepThrough()]
        [DesignerCategory("code")]
        public partial class getReporteComercialCompletedEventArgs : AsyncCompletedEventArgs
        {

            private object[] results;

            internal getReporteComercialCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
                : base(exception, cancelled, userState)
            {
                results = results;
            }

            public ConsultaReporteComercialMultiavalData Result
            {
                get
                {
                    RaiseExceptionIfNecessary();
                    return ((ConsultaReporteComercialMultiavalData)(results[0]));
                }
            }

        }
    }
}