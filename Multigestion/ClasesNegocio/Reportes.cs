using Bd;
using FrameworkIntercapIT.Utilities;
using MultigestionUtilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace ClasesNegocio
{
    public class Reportes
    {
        public Reportes()
        {

        }

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            try
            {
                LogicaNegocio Ln = new LogicaNegocio();
                Utilidades util = new Utilidades();
                String xml = String.Empty;

                //IF PARA CADA REPORTE
                DataSet res1 = new DataSet();
                if (sp == "Contrato_de_Subfianza")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteContratoSubfianza", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLContratoSubFianza(res1);
                }
                else if (sp == "Instruccion_de_Curse")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteInstruccionCurse", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLInstruccionCurse(res1);
                }
                else if (sp == "Certificado_de_Fianza_Comercial" || sp == "Certificado_de_Fianza_Banco_Estado" || sp == "Certificado_de_Fianza_Banco_Security" || sp == "Certificado_de_Fianza_Factoring" || sp == "Certificado_de_Fianza_Itau" || sp == "Certificado_de_Fianza_BBVA")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
                    foreach (DataRow fila in res1.Tables[0].Rows)
                        xml = xml + fila[0];
                    //xml = res1.Tables[0].Rows[0][0].ToString();
                    if (sp == "Certificado_de_Fianza_Comercial")
                        xml = new Code39 { }.GenerarCodigoBarra(res1, objresumen, res1.Tables[1].Rows[0][0].ToString());
                }
                else if (sp == "Certificado_de_Fianza_Tecnica")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCertificadoFianza(res1, id);
                    xml = new Code39 { }.GenerarCodigoBarra(res1, objresumen, res1.Tables[1].Rows[0][0].ToString());
                }
                else if (sp == "Solicitud_de_Pago_de_Garantia")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCertificadoFianza(res1, id);
                }
                else if (sp == "Certificado_de_Elegibilidad")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoElegibilidad", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCertificadoFianza(res1, id);
                }
                else if (sp == "Carta_de_Garantia" || sp == "Carta_de_Garantia_ITAU" || sp == "Carta_de_Garantia_Santander" || sp == "Carta_de_Garantia_BBVA" || sp == "Carta_de_Garantia_Banco_Estado" || sp == "Carta_de_Garantia_Banco_Security" || sp == "Carta_de_Garantia_Factoring")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCartaGarantias", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCartadeGarantia(res1);
                    xml = xml.Replace("<RutAcreedor>RUTACREEDOR</RutAcreedor>", "<RutAcreedor>" + Ln.BuscarIdsYValoresCambioDeEstado(Convert.ToInt32(res1.Tables[0].Rows[0]["IdAcreedor"].ToString().Split('|')[0])) + "</RutAcreedor>");
                    xml = xml.Replace("<ClasificacionSBIF></ClasificacionSBIF>", "<ClasificacionSBIF>" + Ln.ListarEmpresaSagr(1) + "</ClasificacionSBIF>");
                    xml = xml.Replace("<NombreFondoGarantia></NombreFondoGarantia>", "<NombreFondoGarantia>" + Ln.BuscarFondo(res1.Tables[0].Rows[0]["Fondo"].ToString()) + "</NombreFondoGarantia>");
                }
                else if (sp == "Solicitud_de_Emision")
                {
                    res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteSolicitudEmision", datos["ValorUF"], datos["ValorUS"]);
                    foreach (DataRow fila in res1.Tables[0].Rows)
                        xml = xml + fila[0];
                    int ini = xml.IndexOf("<IdAcreedor>"), fin = xml.IndexOf("</IdAcreedor>");
                    int idacreedor = Convert.ToInt32(xml.Substring(ini + 12, fin - (ini + 12)));

                    xml.Replace("<acreedorRUT></acreedorRUT>", "<acreedorRUT>" + Ln.BuscarIdsYValoresCambioDeEstado(idacreedor).Split('|')[0] + "</acreedorRUT>");
                }
                else if (sp == "Revision_Pagare")
                {
                    //string Reporte = "Revision_Pagare_" + datos["IdOperacion"].ToString();
                    //DataTable dtRes = new DataTable();
                    LogicaNegocio LN = new LogicaNegocio();
                    res1 = LN.GenerarXMLRevisionPagare(int.Parse(datos["IdOperacion"].ToString()));
                    if (res1.Tables.Count >= 1)
                    {
                        foreach (DataRow fila in res1.Tables[0].Rows)
                        {
                            xml = xml + fila[0];
                        }
                    }

                    //byte[] archivo = GenerarReportePagare(int.Parse(datos["IdOperacion"].ToString()));
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");
                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public string GenerarXMLInstruccionCurse(DataSet res1)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            XmlNode RespNodeCkb;
            RespNodeCkb = doc.CreateElement("General");

            XmlNode nodo1 = doc.CreateElement("NumeroCertificado");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroCertificado"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("NumeroCartaGarantia");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroCartaGarantia"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("FechaInstructivo");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaInstructivo"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("FechaTC");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaTC"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("AcreedorRazonSocial");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["AcreedorRazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("AcreedorID");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["AcreedorID"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("ClienteRazonSocial");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ClienteRazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("ClienteRUT");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ClienteRUT"].ToString() + "-" + res1.Tables[0].Rows[0]["ClienteDivRUT"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoCredito");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoCredito"].ToString()));
            RespNodeCkb.AppendChild(nodo1);
            //--porcentajeComision

            nodo1 = doc.CreateElement("MontoCreditoCLP");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoCreditoCLP"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("porcentajeComision");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["porcentajeComision"].ToString()));
            RespNodeCkb.AppendChild(nodo1);


            nodo1 = doc.CreateElement("Moneda");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Moneda"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("FechaCurse");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaCurse"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("ValorUF");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ValorUF"].ToString()));
            RespNodeCkb.AppendChild(nodo1);


            nodo1 = doc.CreateElement("MontoCredito");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoCredito"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoComision");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoComision"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("GastosOperacionales");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["GastosOperacionales"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoSeguro");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoSeguro"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoSeguroD");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoSeguroD"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoNotario");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoNotario"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoComisionFogape");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoComisionFogape"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoTyEMultiaval");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoTyEMultiaval"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoTyEAcreedor");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoTyEAcreedor"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("InstruccionCurse");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["InstruccionCurse"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TipoOperacion");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TipoOperacion"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalMismoBanco");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMismoBanco"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalOtrosBancos");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalOtrosBancos"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalBancos");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalBancos"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalMismoBancoR");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMismoBancoR"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalMultiavalL");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMultiavalL"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalMultiaval");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMultiaval"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalOtrosBancosR");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalOtrosBancosR"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalMultiavalR");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMultiavalR"].ToString()));
            RespNodeCkb.AppendChild(nodo1);


            nodo1 = doc.CreateElement("TotalMismoBancoL");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMismoBancoL"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalOtrosBancosL");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalOtrosBancosL"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("MontoLiquidoCliente");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoLiquidoCliente"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("TotalFondosRetenidos");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalFondosRetenidos"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("LiberacionesTotal");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["LiberacionesTotal"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("FechaVencimiento");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaVencimiento"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            nodo1 = doc.CreateElement("NombreSAGR");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreSAGR"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            RespNode.AppendChild(RespNodeCkb);
            ValoresNode.AppendChild(RespNode);
            //BANCO
            XmlNode RespNodeG;
            root = doc.DocumentElement;
            RespNodeG = doc.CreateElement("MismoBanco");

            for (int i = 0; i <= res1.Tables[1].Rows.Count - 1; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Banco");

                nodo1 = doc.CreateElement("NroCredito");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["NroCredito"].ToString()));
                RespNodeGarantia.AppendChild(nodo1);

                nodo1 = doc.CreateElement("MontoCredito");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["MontoCredito"].ToString()));
                RespNodeGarantia.AppendChild(nodo1);

                RespNodeG.AppendChild(RespNodeGarantia);
            }

            ValoresNode.AppendChild(RespNodeG);

            //OTROS BANCOS -----------------------------------
            XmlNode RespNodeA;
            root = doc.DocumentElement;
            RespNodeA = doc.CreateElement("OtrosBancos");

            for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
            {
                XmlNode RespNodeAval;
                RespNodeAval = doc.CreateElement("Banco");

                nodo1 = doc.CreateElement("DescAcreedor");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["DescAcreedor"].ToString()));
                RespNodeAval.AppendChild(nodo1);

                nodo1 = doc.CreateElement("MontoCredito");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["MontoCredito"].ToString()));
                RespNodeAval.AppendChild(nodo1);

                RespNodeA.AppendChild(RespNodeAval);
            }

            ValoresNode.AppendChild(RespNodeA);
            return doc.OuterXml;
        }


        public string GenerarXMLContratoSubFianza(DataSet res1)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            XmlNode RespNodeCkb;
            RespNodeCkb = doc.CreateElement("General");

            XmlNode nodo = doc.CreateElement("Fondo");
            nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Fondo"].ToString()));
            RespNodeCkb.AppendChild(nodo);

            XmlNode nodo1 = doc.CreateElement("acreedorRazonSocial");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["acreedorRazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            XmlNode nodo2 = doc.CreateElement("NCertificado");
            nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NCertificado"].ToString()));
            RespNodeCkb.AppendChild(nodo2);

            XmlNode nodo3 = doc.CreateElement("Fogape");
            nodo3.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Fogape"].ToString()));
            RespNodeCkb.AppendChild(nodo3);

            DateTime thisDate = DateTime.Now;
            CultureInfo culture = new CultureInfo("es-ES");

            XmlNode nodo4 = doc.CreateElement("FechaHoy");
            nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
            RespNodeCkb.AppendChild(nodo4);

            nodo4 = doc.CreateElement("fechaEmision");
            nodo4.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fechaEmision"].ToString()));
            RespNodeCkb.AppendChild(nodo4);

            XmlNode nodo5 = doc.CreateElement("acreedorRUT");
            nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["acreedorRUT"].ToString()));
            RespNodeCkb.AppendChild(nodo5);

            XmlNode nodo6 = doc.CreateElement("acreedorDireccion");
            nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["acreedorDireccion"].ToString()));
            RespNodeCkb.AppendChild(nodo6);

            XmlNode nodo7 = doc.CreateElement("nroCuotas");
            nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["nroCuotas"].ToString()));
            RespNodeCkb.AppendChild(nodo7);

            XmlNode nodo8 = doc.CreateElement("MontoMoneda");
            nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Monto"].ToString() + " " + res1.Tables[0].Rows[0]["Moneda"].ToString()));
            RespNodeCkb.AppendChild(nodo8);

            XmlNode nodo9 = doc.CreateElement("Tasa");
            nodo9.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tasa"].ToString()));
            RespNodeCkb.AppendChild(nodo9);

            XmlNode nodo10 = doc.CreateElement("Plazo");
            nodo10.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["plazo"].ToString()));
            RespNodeCkb.AppendChild(nodo10);

            XmlNode nodo11 = doc.CreateElement("NDocumento");
            nodo11.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NDocumento"].ToString()));
            RespNodeCkb.AppendChild(nodo11);

            XmlNode nodo12 = doc.CreateElement("clienteRazonSocial");
            nodo12.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteRazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo12);

            XmlNode nodo13 = doc.CreateElement("tipoOperacion");
            nodo13.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoOperacion"].ToString()));
            RespNodeCkb.AppendChild(nodo13);

            XmlNode nodo14 = doc.CreateElement("PeriodoGracia");
            nodo14.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["PeriodoGracia"].ToString()));
            RespNodeCkb.AppendChild(nodo14);

            //representante legal multiaval 17-01-2017
            XmlNode nodo15 = doc.CreateElement("DescRepresentanteLegal");
            nodo15.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRepresentanteLegal"].ToString()));
            RespNodeCkb.AppendChild(nodo15);

            XmlNode nodo16 = doc.CreateElement("RutRepresentanteLegal");
            nodo16.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegal"].ToString()));
            RespNodeCkb.AppendChild(nodo16);

            XmlNode nodo17 = doc.CreateElement("DescRepresentanteFondo");
            nodo17.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRepresentanteFondo"].ToString()));
            RespNodeCkb.AppendChild(nodo17);

            XmlNode nodo18 = doc.CreateElement("RutRepresentanteFondo");
            nodo18.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteFondo"].ToString()));
            RespNodeCkb.AppendChild(nodo18);

            //datos de la empresa aval
            XmlNode nodo19 = doc.CreateElement("Rut");
            nodo19.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString()));
            RespNodeCkb.AppendChild(nodo19);

            XmlNode nodo20 = doc.CreateElement("SGR");
            nodo20.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["SGR"].ToString()));
            RespNodeCkb.AppendChild(nodo20);

            XmlNode nodo21 = doc.CreateElement("NombreSGR");
            nodo21.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreSGR"].ToString()));
            RespNodeCkb.AppendChild(nodo21);

            XmlNode nodo22 = doc.CreateElement("Domicilio");
            nodo22.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Domicilio"].ToString()));
            RespNodeCkb.AppendChild(nodo22);

            XmlNode nodo23 = doc.CreateElement("FechaEscrituraPublica");
            nodo23.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEscrituraPublica"].ToString()));
            RespNodeCkb.AppendChild(nodo23);

            XmlNode nodo24 = doc.CreateElement("Notaria");
            nodo24.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Notaria"].ToString()));
            RespNodeCkb.AppendChild(nodo24);

            XmlNode nodo25 = doc.CreateElement("NumeroRepertorio");
            nodo25.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroRepertorio"].ToString()));
            RespNodeCkb.AppendChild(nodo25);

            XmlNode nodo26 = doc.CreateElement("RepresentanteLegal");
            nodo26.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RepresentanteLegal"].ToString()));
            RespNodeCkb.AppendChild(nodo26);

            XmlNode nodo27 = doc.CreateElement("RutRepresentanteLegalSGR");
            nodo27.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegalSGR"].ToString()));
            RespNodeCkb.AppendChild(nodo27);

            // asi con todos los nodos de3 este nievel
            RespNode.AppendChild(RespNodeCkb);
            ValoresNode.AppendChild(RespNode);

            XmlNode RespNodeG;
            root = doc.DocumentElement;
            RespNodeG = doc.CreateElement("Garantias");


            for (int i = 0; i <= res1.Tables[1].Rows.Count - 1; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Garantia");

                nodo = doc.CreateElement("NGarantia");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["NGarantia"].ToString()));
                RespNodeGarantia.AppendChild(nodo);

                RespNodeG.AppendChild(RespNodeGarantia);
            }

            ValoresNode.AppendChild(RespNodeG);


            //aval -----------------------------------
            XmlNode RespNodeA;
            root = doc.DocumentElement;
            RespNodeA = doc.CreateElement("Avales");


            for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
            {
                XmlNode RespNodeAval;
                RespNodeAval = doc.CreateElement("Aval");

                nodo = doc.CreateElement("Aval");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Aval"].ToString()));
                RespNodeAval.AppendChild(nodo);

                RespNodeA.AppendChild(RespNodeAval);
            }

            ValoresNode.AppendChild(RespNodeA);
            return doc.OuterXml;
        }



        private byte[] generarPaf2Express(Dictionary<string, string> datos, string ejecutivo)
        {
            String xml = String.Empty;
            DataSet res1 = new DataSet();
            LogicaNegocio Ln = new LogicaNegocio();

            res1 = Ln.ConsultaReporteBD(datos["IdEmpresa"], datos["IdPaf"], 2, "Adminitrador", "ReportePAF");
            return GenerarPdfPAF(res1);
        }

        public byte[] GenerarReporteOld(string Reporte, Dictionary<string, string> datos, string ejecutivo)
        {
            try
            {
                switch (Reporte)
                {
                    case "Propuesta_Afianzamiento_Express":
                        return generarPaf2Express(datos, ejecutivo);
                    //break;

                    case "Propuesta_Afianzamiento_Old":
                        return generarPaf1(datos, ejecutivo);
                    //break;

                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        private byte[] generarPaf1(Dictionary<string, string> datos, string ejecutivo)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            String xml = String.Empty;
            DataSet res1 = new DataSet();
            Utilidades util = new Utilidades();
            LogicaNegocio bd = new LogicaNegocio();

            res1 = bd.ConsultaReporteBD_Old(datos["IdEmpresa"], datos["IdPaf"], "Systema", "Adminitrator", "PAF");
            xml = GenerarXMLContratoSubFianza(res1, ejecutivo, datos["Rank"], datos["Clasi"], datos["Ventas"]);

            XDocument newTree = new XDocument();
            XslCompiledTransform xsltt = new XslCompiledTransform();

            using (XmlWriter writer = newTree.CreateWriter())
            {
                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Propuesta_Afianzamiento_Old" + ".xslt");
            }
            using (var sw = new StringWriter())
            using (var sr = new StringReader(xml))
            using (var xr = XmlReader.Create(sr))
            {
                xsltt.Transform(xr, null, sw);
                html = sw.ToString();
            }
            try
            {
                sDocumento.Append(html);
                return util.ConvertirAPDF_Control(sDocumento);
            }
            catch
            {
                return null;
            }
        }

        //private byte[] generarPaf2Express(Dictionary<string, string> datos, string ejecutivo)
        //{
        //    String xml = String.Empty;
        //    DataSet res1 = new DataSet();
        //    //Utilidades util = new Utilidades();

        //    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdPaf"], 2, "Adminitrador", "ReportePAF");
        //    return GenerarPdfPAF(res1);
        //}

        public byte[] GenerarPdfPAF(DataSet dsPaf)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Utilidades util = new Utilidades();
            byte[] archivoSalida = null;
            string resultadoDoc = string.Empty;
            string idEmpresa = string.Empty;
            string idPAF = string.Empty;
            string idImpresionPAF = string.Empty;
            string xml = string.Empty;
            string xmlReporteNoResumen = string.Empty;
            string xmlReporteIVACompraVentas = string.Empty;
            string xmlIndicFinancieros = string.Empty;

            if (dsPaf.Tables.Count == 6)
            {
                idEmpresa = dsPaf.Tables[0].Rows[0][1].ToString();
                idPAF = dsPaf.Tables[0].Rows[0][2].ToString();
                idImpresionPAF = dsPaf.Tables[0].Rows[0][3].ToString();

                int cantEmpresas = int.Parse(dsPaf.Tables[0].Rows[0][0].ToString());

                for (int i = 0; i < dsPaf.Tables[1].Rows.Count; i++)
                {
                    xml = xml + dsPaf.Tables[1].Rows[i][0];
                }
                for (int i = 0; i < dsPaf.Tables[2].Rows.Count; i++)
                {
                    xmlReporteIVACompraVentas = xmlReporteIVACompraVentas + dsPaf.Tables[2].Rows[i][0];
                }

                for (int i = 0; i < dsPaf.Tables[4].Rows.Count; i++)
                {
                    xmlReporteNoResumen = xmlReporteNoResumen + dsPaf.Tables[4].Rows[i][0];
                }

                for (int i = 0; i < dsPaf.Tables[5].Rows.Count; i++)
                {
                    xmlIndicFinancieros = xmlIndicFinancieros + dsPaf.Tables[5].Rows[i][0];
                }

                xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<ResumenIVA>");
                xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("</Val></Valores>", "</ResumenIVA>");
                xmlReporteNoResumen = xmlReporteNoResumen.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<VaciadoBalance>");
                xmlReporteNoResumen = xmlReporteNoResumen.Replace("</Val></Valores>", "</VaciadoBalance>");

                xmlIndicFinancieros = xmlIndicFinancieros.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<IndicadoresFinancierosPadre>");
                xmlIndicFinancieros = xmlIndicFinancieros.Replace("</Val></Valores>", "</IndicadoresFinancierosPadre>");

                if (xml.Contains("<ResumenIVA>"))
                    xml = xml.Replace("<ResumenIVA>-</ResumenIVA>", xmlReporteIVACompraVentas);
                if (xml.Contains("<VaciadoBalance>"))
                    xml = xml.Replace("<VaciadoBalance>--</VaciadoBalance>", xmlReporteNoResumen);
                if (xml.Contains("<IndicadoresFinancierosPadre>"))
                    xml = xml.Replace("<IndicadoresFinancierosPadre>-</IndicadoresFinancierosPadre>", xmlIndicFinancieros);

                /////Gráficos
                string nombreGrafico1 = string.Empty;
                string rutaGraficos = @"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/";

                nombreGrafico1 = "G1" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + idEmpresa;

                if (xml.Contains("<Grafico1>"))
                {
                    crearGrafico1(dsPaf.Tables[3], nombreGrafico1);
                    xml = xml.Replace("<Grafico1>-</Grafico1>", "<Grafico1>" + rutaGraficos + nombreGrafico1 + ".png" + "</Grafico1>");
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Propuesta_Afianzamiento_Express" + ".xslt");
                }
                string html = string.Empty;
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    StringBuilder sDocumento = new StringBuilder();
                    DateTime dtime = new DateTime();
                    dtime = DateTime.Now;
                    sDocumento.Append(html);
                    archivoSalida = util.ConvertirAPDF_Control(sDocumento);
                    string nombreArchivo = "ReportePAF_" + idPAF + "_" + dtime.Year.ToString() + dtime.Month.ToString() + dtime.Day + "_" + dtime.Hour.ToString() + dtime.Minute.ToString() + ".PDF";

                    bool actualizacionCorrecta = Ln.ActualizarPafExpress(int.Parse(idImpresionPAF), 0, "", "04", "CALCULADO"); //Cambiar estado a calculado
                }
                catch (Exception)
                {
                }
            }
            else
            {
                //ObtenerDatos.ActualizarImpresionPAFNoCalculado(int.Parse(idImpresionPAF));
            }

            return archivoSalida;
        }

        private void crearGrafico1(DataTable dt, string nombreGrafico1)
        {
            //Gráfico 1;
            double[] anioAct = new double[12];
            double[] anio1 = new double[12];
            double[] anio2 = new double[12];
            double[] anio3 = new double[12];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                anioAct[i] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
                anio1[i] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
                anio2[i] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
                anio3[i] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
            }
            string[] meses = new string[12];
            int nroMes = 1;
            CultureInfo culture = new CultureInfo("es-ES");
            for (int i = 0; i < meses.Length; i++)
            {
                meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
                nroMes++;
            }

            Chart grafico1 = new Chart();
            grafico1.ChartAreas.Add(new ChartArea());
            grafico1.ChartAreas[0].AxisX.Interval = 1;
            grafico1.Width = 710;
            grafico1.Height = 300;
            Series serieAnioAct = new Series();
            Series serieAnio1 = new Series();
            Series serieAnio2 = new Series();
            Series serieAnio3 = new Series();

            serieAnioAct.Points.DataBindXY(meses, anioAct);
            serieAnio1.Points.DataBindXY(meses, anio1);
            serieAnio2.Points.DataBindXY(meses, anio2);
            serieAnio3.Points.DataBindXY(meses, anio3);

            serieAnioAct.Name = (DateTime.Now.Year).ToString();
            serieAnio1.Name = (DateTime.Now.Year - 1).ToString();
            serieAnio2.Name = (DateTime.Now.Year - 2).ToString();
            serieAnio3.Name = (DateTime.Now.Year - 3).ToString();

            //#99BE1A verde
            //#E85426 nar
            //#1E9CD6 azu
            //#9966CC mor
            serieAnioAct.Color = ColorTranslator.FromHtml("#1E9CD6");
            serieAnio1.Color = ColorTranslator.FromHtml("#9966CC");
            serieAnio2.Color = ColorTranslator.FromHtml("#E85426");
            serieAnio3.Color = ColorTranslator.FromHtml("#99BE1A");

            serieAnioAct.BorderWidth = 3;
            serieAnio1.BorderWidth = 3;
            serieAnio2.BorderWidth = 3;
            serieAnio3.BorderWidth = 3;

            serieAnioAct.ChartType = SeriesChartType.Spline;
            serieAnioAct.BorderDashStyle = ChartDashStyle.Dash;

            serieAnio1.ChartType = SeriesChartType.Spline;
            serieAnio1.BorderDashStyle = ChartDashStyle.Dot;

            serieAnio2.ChartType = SeriesChartType.Spline;
            serieAnio2.BorderDashStyle = ChartDashStyle.Solid;

            serieAnio3.ChartType = SeriesChartType.Spline;
            serieAnio3.BorderDashStyle = ChartDashStyle.DashDotDot;

            Title titulo = new Title();
            titulo.Name = "Titulo1";
            titulo.Text = "Ventas Mensuales por Período (M$)";
            titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
            grafico1.Titles.Add(titulo);

            Legend lg = new Legend();
            lg.Name = "Leyenda1";
            lg.Docking = Docking.Bottom;
            lg.Alignment = StringAlignment.Center;
            grafico1.Legends.Add(lg);
            serieAnioAct.Legend = "Leyenda1";

            grafico1.Series.Add(serieAnioAct);
            grafico1.Series.Add(serieAnio1);
            grafico1.Series.Add(serieAnio2);
            grafico1.Series.Add(serieAnio3);

            grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico1 + ".png", ChartImageFormat.Png);
        }

        public string GenerarXMLCertificadoFianza(DataSet res1, string id)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            string xmlt = "";
            int IdTemp = Convert.ToInt32(id);
            res1.Tables[0].Rows[0][0].ToString().Replace("<acreedorRUT>acredor rut</acreedorRUT>", "<acreedorRUT>" + Ln.BuscarIdsYValoresCambioDeEstado(IdTemp) + "</acreedorRUT>");
            foreach (DataRow fila in res1.Tables[0].Rows)
                xmlt = xmlt + fila[0];

            return xmlt;
        }

        public string GenerarXMLCartadeGarantia(DataSet res1)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(docNode);

            XmlNode ValoresNode = doc.CreateElement("Empresas");
            doc.AppendChild(ValoresNode);

            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Empresa");

            XmlNode RespNodeCkb;
            RespNodeCkb = doc.CreateElement("General");

            XmlNode nodo = doc.CreateElement("fechaActual");
            nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fechaActual"].ToString()));
            RespNodeCkb.AppendChild(nodo);

            XmlNode nodo1 = doc.CreateElement("clienteRazonSocial");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteRazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            XmlNode nodo2 = doc.CreateElement("clienteRUT");
            nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteRUT"].ToString() + "-" + res1.Tables[0].Rows[0]["clienteDivRUT"].ToString()));
            RespNodeCkb.AppendChild(nodo2);

            XmlNode nodo3 = doc.CreateElement("clienteDireccion");
            nodo3.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteDireccion"].ToString()));
            RespNodeCkb.AppendChild(nodo3);

            DateTime thisDate = DateTime.Now;
            CultureInfo culture = new CultureInfo("es-ES");

            XmlNode nodo4 = doc.CreateElement("fecha");
            nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
            RespNodeCkb.AppendChild(nodo4);

            thisDate = (DateTime.Now).AddDays(5);

            nodo4 = doc.CreateElement("fecha5");
            nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
            RespNodeCkb.AppendChild(nodo4);

            XmlNode nodo198 = doc.CreateElement("fechaContrato");
            nodo198.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fechaContrato"].ToString()));
            RespNodeCkb.AppendChild(nodo198);

            XmlNode nodo199 = doc.CreateElement("FechaEmisionC");
            nodo199.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEmisionC"].ToString()));
            RespNodeCkb.AppendChild(nodo199);

            nodo199 = doc.CreateElement("FechaEmisionC5");
            nodo199.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEmisionC5"].ToString()));
            RespNodeCkb.AppendChild(nodo199);

            XmlNode nodo5 = doc.CreateElement("Fondo");
            nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Fondo"].ToString()));
            RespNodeCkb.AppendChild(nodo5);

            XmlNode nodo6 = doc.CreateElement("tipoOperacion");
            nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoOperacion"].ToString()));
            RespNodeCkb.AppendChild(nodo6);

            nodo6 = doc.CreateElement("tipoAmortizacion");
            nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoAmortizacion"].ToString()));
            RespNodeCkb.AppendChild(nodo6);

            XmlNode nodo7 = doc.CreateElement("periocidad");
            nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["periocidad"].ToString()));
            RespNodeCkb.AppendChild(nodo7);

            XmlNode nodo8 = doc.CreateElement("plazo");
            nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["plazo"].ToString()));
            RespNodeCkb.AppendChild(nodo8);

            nodo8 = doc.CreateElement("plazoDias");
            nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["plazoDias"].ToString()));
            RespNodeCkb.AppendChild(nodo8);

            XmlNode nodo9 = doc.CreateElement("tasa");
            nodo9.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tasa"].ToString()));
            RespNodeCkb.AppendChild(nodo9);

            XmlNode nodo10 = doc.CreateElement("montoOperacion");
            nodo10.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["montoOperacion"].ToString()));
            RespNodeCkb.AppendChild(nodo10);

            XmlNode nodo11 = doc.CreateElement("periodoGracia");
            nodo11.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["periodoGracia"].ToString()));
            RespNodeCkb.AppendChild(nodo11);

            XmlNode nodo12 = doc.CreateElement("fogape");
            nodo12.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fogape"].ToString()));
            RespNodeCkb.AppendChild(nodo12);

            XmlNode nodo13 = doc.CreateElement("coberturaFogape");
            nodo13.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["coberturaFogape"].ToString()));
            RespNodeCkb.AppendChild(nodo13);

            XmlNode nodo14 = doc.CreateElement("NumeroDocumento");
            nodo14.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroDocumento"].ToString()));
            RespNodeCkb.AppendChild(nodo14);

            XmlNode nodo15 = doc.CreateElement("Monto");
            nodo15.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Monto"].ToString()));
            RespNodeCkb.AppendChild(nodo15);

            XmlNode nodo16 = doc.CreateElement("FondoGarantia");
            nodo16.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FondoGarantia"].ToString()));
            RespNodeCkb.AppendChild(nodo16);

            XmlNode nodo17 = doc.CreateElement("NumeroCuotas");
            nodo17.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroCuotas"].ToString()));
            RespNodeCkb.AppendChild(nodo17);

            XmlNode nodo18 = doc.CreateElement("FondoCorfo");
            if (res1.Tables[0].Rows[0]["FondoCorfo"].ToString() == "1")
                nodo18.AppendChild(doc.CreateTextNode("OPERACIÓN AFIANZADA CONTRA FONDO CORFO"));
            else
                nodo18.AppendChild(doc.CreateTextNode(""));
            RespNodeCkb.AppendChild(nodo18);

            XmlNode nodo19 = doc.CreateElement("Acreedor");
            nodo19.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Acreedor"].ToString()));
            RespNodeCkb.AppendChild(nodo19);

            XmlNode nodo20 = doc.CreateElement("RutAcreedor");
            nodo20.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutAcreedor"].ToString()));
            RespNodeCkb.AppendChild(nodo20);

            XmlNode nodo21 = doc.CreateElement("DireccionAcreedor");
            nodo21.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DireccionAcreedor"].ToString()));
            RespNodeCkb.AppendChild(nodo21);

            XmlNode nodo22 = doc.CreateElement("FechaEmision");
            nodo22.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEmision"].ToString()));
            RespNodeCkb.AppendChild(nodo22);

            XmlNode nodo23 = doc.CreateElement("Moneda");
            nodo23.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Moneda"].ToString()));
            RespNodeCkb.AppendChild(nodo23);

            XmlNode nodo24 = doc.CreateElement("NumeroOperacion");
            nodo24.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroOperacion"].ToString()));
            RespNodeCkb.AppendChild(nodo24);

            XmlNode nodo25 = doc.CreateElement("IdAcreedor");
            nodo25.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["IdAcreedor"].ToString()));
            RespNodeCkb.AppendChild(nodo25);

            XmlNode nodo26 = doc.CreateElement("Tasa");
            nodo26.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Tasa"].ToString()));
            RespNodeCkb.AppendChild(nodo26);

            XmlNode nodo27 = doc.CreateElement("TipoTasa");
            nodo27.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TipoTasa"].ToString()));
            RespNodeCkb.AppendChild(nodo27);

            XmlNode nodo28 = doc.CreateElement("FechaPrimerVencimiento");
            nodo28.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaPrimerVencimiento"].ToString()));
            RespNodeCkb.AppendChild(nodo28);

            XmlNode nodo29 = doc.CreateElement("FechaUltimoVencimiento");
            nodo29.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaUltimoVencimiento"].ToString()));
            RespNodeCkb.AppendChild(nodo29);

            XmlNode nodo30 = doc.CreateElement("ClasificacionSBIF");
            nodo30.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ClasificacionSBIF"].ToString()));
            RespNodeCkb.AppendChild(nodo30);

            XmlNode nodo31 = doc.CreateElement("NombreFondoGarantia");
            nodo31.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreFondoGarantia"].ToString()));
            RespNodeCkb.AppendChild(nodo31);

            XmlNode nodo32 = doc.CreateElement("TipoOperacion");
            nodo32.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TipoOperacion"].ToString()));
            RespNodeCkb.AppendChild(nodo32);

            XmlNode nodo33 = doc.CreateElement("MonedaC");
            nodo33.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MonedaC"].ToString()));
            RespNodeCkb.AppendChild(nodo33);

            XmlNode nodo34 = doc.CreateElement("Signo");
            nodo34.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Signo"].ToString()));
            RespNodeCkb.AppendChild(nodo34);
            // Avales
            XmlNode nodo35 = doc.CreateElement("Avales");
            nodo35.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Avales"].ToString()));
            RespNodeCkb.AppendChild(nodo35);
            // cobertura
            XmlNode nodo36 = doc.CreateElement("cobertura");
            nodo36.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["cobertura"].ToString()));
            RespNodeCkb.AppendChild(nodo36);

            XmlNode nodo37 = doc.CreateElement("tipoCuota");
            nodo37.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoCuota"].ToString()));
            RespNodeCkb.AppendChild(nodo37);

            XmlNode nodo38 = doc.CreateElement("NroPagare");
            nodo38.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NroPagare"].ToString()));
            RespNodeCkb.AppendChild(nodo38);

            //representante legal multiaval 17-01-2017
            XmlNode nodo39 = doc.CreateElement("DescRepresentanteLegal");
            nodo39.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRepresentanteLegal"].ToString()));
            RespNodeCkb.AppendChild(nodo39);

            XmlNode nodo40 = doc.CreateElement("RutRepresentanteLegal");
            nodo40.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegal"].ToString()));
            RespNodeCkb.AppendChild(nodo40);

            //datos de la empresa sagr asociada al fondo
            XmlNode nodo41 = doc.CreateElement("Rut");
            nodo41.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString()));
            RespNodeCkb.AppendChild(nodo41);

            XmlNode nodo42 = doc.CreateElement("SGR");
            nodo42.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["SGR"].ToString()));
            RespNodeCkb.AppendChild(nodo42);

            XmlNode nodo43 = doc.CreateElement("NombreSGR");
            nodo43.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreSGR"].ToString()));
            RespNodeCkb.AppendChild(nodo43);

            XmlNode nodo44 = doc.CreateElement("Domicilio");
            nodo44.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Domicilio"].ToString()));
            RespNodeCkb.AppendChild(nodo44);

            XmlNode nodo45 = doc.CreateElement("FechaEscrituraPublica");
            nodo45.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEscrituraPublica"].ToString()));
            RespNodeCkb.AppendChild(nodo45);

            XmlNode nodo46 = doc.CreateElement("Notaria");
            nodo46.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Notaria"].ToString()));
            RespNodeCkb.AppendChild(nodo46);

            XmlNode nodo47 = doc.CreateElement("NumeroRepertorio");
            nodo47.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroRepertorio"].ToString()));
            RespNodeCkb.AppendChild(nodo47);

            XmlNode nodo48 = doc.CreateElement("RepresentanteLegal");
            nodo48.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RepresentanteLegal"].ToString()));
            RespNodeCkb.AppendChild(nodo48);

            XmlNode nodo49 = doc.CreateElement("RutRepresentanteLegalSGR");
            nodo49.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegalSGR"].ToString()));
            RespNodeCkb.AppendChild(nodo49);

            XmlNode nodo50 = doc.CreateElement("valorCuota");
            nodo50.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["valorCuota"].ToString()));
            RespNodeCkb.AppendChild(nodo50);

            //------------------------------------------------------------------------------------//

            RespNode.AppendChild(RespNodeCkb);
            ValoresNode.AppendChild(RespNode);

            XmlNode RespNodeG;
            root = doc.DocumentElement;
            RespNodeG = doc.CreateElement("Avales");

            for (int i = 0; i <= res1.Tables[1].Rows.Count - 1; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Aval");

                nodo = doc.CreateElement("Aval");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Aval"].ToString()));
                RespNodeGarantia.AppendChild(nodo);


                RespNodeG.AppendChild(RespNodeGarantia);
            }

            ValoresNode.AppendChild(RespNodeG);
            return doc.OuterXml;
        }

        public string GenerarXMLContratoSubFianza(DataSet res1, string Ejecutivo, string rank, string clasi, string Ventas)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);

            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;
            RespNode = doc.CreateElement("Val");

            XmlNode RespNodeCkb;
            RespNodeCkb = doc.CreateElement("General");

            XmlNode nodo = doc.CreateElement("RazonSocial");
            nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RazonSocial"].ToString()));
            RespNodeCkb.AppendChild(nodo);

            XmlNode nodo1 = doc.CreateElement("Direccion");
            nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["direccion"].ToString()));
            RespNodeCkb.AppendChild(nodo1);

            XmlNode nodo2 = doc.CreateElement("Comuna");
            nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescComuna"].ToString()));
            RespNodeCkb.AppendChild(nodo2);


            for (int i = 0; i < res1.Tables[3].Rows.Count; i++)
            {
                DateTime fecha = DateTime.MinValue;

                DateTime fechaAux = res1.Tables[3].Rows[i]["FechaCreacion"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(res1.Tables[3].Rows[i]["FechaCreacion"].ToString());

                if (fechaAux >= fecha)
                {
                    XmlNode nodo122 = doc.CreateElement("Canal");
                    nodo122.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Canal"].ToString()));
                    RespNodeCkb.AppendChild(nodo122);
                    fecha = res1.Tables[3].Rows[i]["FechaCreacion"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(res1.Tables[3].Rows[i]["FechaCreacion"].ToString());
                }
            }

            string aux = res1.Tables[0].Rows[0]["GIROACT"].ToString();
            XmlNode nodo3 = doc.CreateElement("Actividad");
            nodo3.AppendChild(doc.CreateTextNode(aux.Substring(2, aux.Length - 2)));//.Substring(2, aux.Length-1)));
            RespNodeCkb.AppendChild(nodo3);

            DateTime thisDate = DateTime.Now;
            CultureInfo culture = new CultureInfo("es-ES");

            XmlNode nodo4 = doc.CreateElement("FechaHoy");
            nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
            RespNodeCkb.AppendChild(nodo4);

            XmlNode nodo5 = doc.CreateElement("RutEmpresa");
            nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString() + "-" + res1.Tables[0].Rows[0]["DivRut"].ToString()));
            RespNodeCkb.AppendChild(nodo5);

            XmlNode nodo6 = doc.CreateElement("Region");
            nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRegion"].ToString()));
            RespNodeCkb.AppendChild(nodo6);

            XmlNode nodo7 = doc.CreateElement("Telefono");
            nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TelFijo1"].ToString()));
            RespNodeCkb.AppendChild(nodo7);

            XmlNode nodo8 = doc.CreateElement("Ciudad");
            nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescProvincia"].ToString()));
            RespNodeCkb.AppendChild(nodo8);

            XmlNode nodoA1 = doc.CreateElement("AnoAct");
            nodoA1.AppendChild(doc.CreateTextNode(DateTime.Now.Year.ToString()));
            RespNodeCkb.AppendChild(nodoA1);

            XmlNode nodoA2 = doc.CreateElement("Ano-1");
            nodoA2.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 1).ToString()));
            RespNodeCkb.AppendChild(nodoA2);

            XmlNode nodoA3 = doc.CreateElement("Ano-2");
            nodoA3.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 2).ToString()));
            RespNodeCkb.AppendChild(nodoA3);

            XmlNode nodoA4 = doc.CreateElement("Ano-3");
            nodoA4.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 3).ToString()));
            RespNodeCkb.AppendChild(nodoA4);

            // asi con todos los nodos de3 este nievel
            RespNode.AppendChild(RespNodeCkb);
            ValoresNode.AppendChild(RespNode);

            //InsertarDatosPAF de la PAF
            XmlNode RespNodePropuestaAfianzamiento;
            //XmlNode root = doc.DocumentElement;
            RespNodePropuestaAfianzamiento = doc.CreateElement("PAF");

            XmlNode nodoP = doc.CreateElement("Oficina");
            nodoP.AppendChild(doc.CreateTextNode("1"));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP);

            XmlNode nodoP1 = doc.CreateElement("Fecha");
            nodoP1.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["fecha"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP1);

            XmlNode nodoP2 = doc.CreateElement("FechaRevision");
            nodoP2.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["FechaRevision"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP2);

            XmlNode nodoP3 = doc.CreateElement("Ejecutivo");
            nodoP3.AppendChild(doc.CreateTextNode(Ejecutivo));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP3);

            XmlNode nodoP4 = doc.CreateElement("EstadoLinea");
            nodoP4.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["EstadoLinea"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP4);

            XmlNode nodoP5 = doc.CreateElement("NivelAtribucion");
            nodoP5.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["NivelAtribucion"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP5);

            XmlNode nodoP6 = doc.CreateElement("ValorRank");
            nodoP6.AppendChild(doc.CreateTextNode(rank));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP6);

            XmlNode nodoP116 = doc.CreateElement("Clasificacion");
            nodoP116.AppendChild(doc.CreateTextNode(clasi));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP116);

            if (res1.Tables[5].Rows.Count > 0)
            {
                XmlNode nodoP7 = doc.CreateElement("ValorPond");
                nodoP7.AppendChild(doc.CreateTextNode(res1.Tables[5].Rows[0]["ValorPond"].ToString().Replace(".", ",")));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP7);

                XmlNode nodoP8 = doc.CreateElement("FechaScoring");
                nodoP8.AppendChild(doc.CreateTextNode((res1.Tables[5].Rows[0]["FecCreacion"].ToString())));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP8);
            }
            else
            {
                XmlNode nodoP7 = doc.CreateElement("ValorPond");
                nodoP7.AppendChild(doc.CreateTextNode(""));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP7);

                XmlNode nodoP8 = doc.CreateElement("FechaScoring");
                nodoP8.AppendChild(doc.CreateTextNode(("")));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP8);
            }

            XmlNode nodoP9 = doc.CreateElement("VentasMoviles");
            nodoP9.AppendChild(doc.CreateTextNode(Ventas));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP9);

            XmlNode nodoP10 = doc.CreateElement("ObservacionComite");
            nodoP10.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["ObservacionComite"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP10);

            XmlNode nodoP11 = doc.CreateElement("IdPaf");
            nodoP11.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["IdPaf"].ToString()));
            RespNodePropuestaAfianzamiento.AppendChild(nodoP11);

            if (res1.Tables[6].Rows.Count > 0)
            {
                XmlNode nodoP12 = doc.CreateElement("Aprobador1");
                nodoP12.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP12);

                XmlNode nodoP13 = doc.CreateElement("Aprobador2");
                nodoP13.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador1"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP13);

                XmlNode nodoP14 = doc.CreateElement("Aprobador3");
                nodoP14.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador2"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP14);

                XmlNode nodoP15 = doc.CreateElement("Aprobador4");
                nodoP15.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador3"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP15);

                XmlNode nodoP16 = doc.CreateElement("Aprobador5");
                nodoP16.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador4"].ToString()));
                RespNodePropuestaAfianzamiento.AppendChild(nodoP16);
            }
            ValoresNode.AppendChild(RespNodePropuestaAfianzamiento);

            //datos gaantia

            XmlNode RespNodeG;
            root = doc.DocumentElement;
            RespNodeG = doc.CreateElement("Garantias");
            double TotalGarantiaComercial = 0;
            double TotalGarantiaAjustado = 0;
            double TotalCoberturaAjustadoVigente = 0;
            double TotalCoberturaComercialVigente = 0;
            double TotalCoberturaAjustadoNuevo = 0;
            double TotalGarantiaNuevaComercial = 0;
            double coberturaCertificado = 0;

            for (int i = 0; i < res1.Tables[1].Rows.Count; i++)
            {
                if (int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 52 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 53 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 54 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 59)
                {
                    TotalGarantiaComercial = TotalGarantiaComercial + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                    TotalGarantiaAjustado = TotalGarantiaAjustado + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");

                    if (res1.Tables[1].Rows[i]["DescEstado"].ToString() == "Constituida" || res1.Tables[1].Rows[i]["DescEstado"].ToString() == "Tramite")
                    {
                        TotalCoberturaAjustadoVigente = TotalCoberturaAjustadoVigente + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        TotalCoberturaComercialVigente = TotalCoberturaComercialVigente + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                    }
                    else
                    {
                        TotalCoberturaAjustadoNuevo = TotalCoberturaAjustadoNuevo + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                        TotalGarantiaNuevaComercial = TotalGarantiaNuevaComercial + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
                    }
                }

                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Garantia");

                nodo = doc.CreateElement("N");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["N"].ToString()));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("TipoGarantia");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Tipo Garantia"].ToString()));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("Descripcion");
                nodo2.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Descripción"].ToString()));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("Comentarios");
                nodo3.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Comentarios"].ToString().Trim()));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("TipoMO");
                nodo4.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Tipo Moneda"].ToString()));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("ValorComercial");
                nodo5.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Valor Comercial"].ToString()));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("ValorAjustado");
                nodo6.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString()));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("AplicaSeguro");
                if (!string.IsNullOrEmpty(res1.Tables[1].Rows[i]["Seguro"].ToString()))
                    nodo7.AppendChild(doc.CreateTextNode((Boolean)res1.Tables[1].Rows[i]["Seguro"] ? "Aplica" : "No Aplica"));
                else
                    nodo7.AppendChild(doc.CreateTextNode("No se ha ingresado una opcion de seguro"));

                RespNodeGarantia.AppendChild(nodo7);

                RespNodeG.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeG);


            //aval -----------------------------------
            XmlNode RespNodeA;
            root = doc.DocumentElement;
            RespNodeA = doc.CreateElement("OperacionesVigentes");

            double totalAprobadoCLP = 0;
            double totalAprobadoCLP_Certificado = 0;
            double totalVigenteCLP = 0;
            double totalVigenteCLP_Certificado = 0;
            double totalPropuestoCLP = 0;
            double totalPropuestoCLP_Certificado = 0;

            double totalAprobadoUF = 0;
            double totalAprobadoUF_Certificado = 0;
            double totalVigenteUF = 0;
            double totalVigenteUF_Certificado = 0;
            double totalPropuestoUF = 0;
            double totalPropuestoUF_Certificado = 0;

            double totalAprobadoUSD = 0;
            double totalAprobadoUSD_Certificado = 0;
            double totalVigenteUSD = 0;
            double totalVigenteUSD_Certificado = 0;
            double totalPropuestoUSD = 0;
            double totalPropuestoUSD_Certificado = 0;

            //OPERACIONES VIGENTES
            for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
            {
                if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["CoberturaCertificado"].ToString()))
                    coberturaCertificado = 1;
                else
                    coberturaCertificado = double.Parse(res1.Tables[2].Rows[i]["CoberturaCertificado"].ToString()) / 100;

                if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "UF")
                {
                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        totalAprobadoUF = totalAprobadoUF + 0;
                        totalAprobadoUF_Certificado = totalAprobadoUF_Certificado + 0;
                    }
                    else
                    {
                        totalAprobadoUF_Certificado = totalAprobadoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                        totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
                    {
                        totalVigenteUF = totalVigenteUF + 0;
                        totalVigenteUF_Certificado = totalVigenteUF_Certificado + 0;
                    }
                    else
                    {
                        totalVigenteUF_Certificado = totalVigenteUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                        totalVigenteUF = totalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        totalPropuestoUF = totalPropuestoUF + 0;
                        totalPropuestoUF_Certificado = totalPropuestoUF_Certificado + 0;
                    }
                    else
                    {
                        totalPropuestoUF_Certificado = totalPropuestoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                        totalPropuestoUF = totalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                    }
                }

                if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "CLP")
                {
                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        totalAprobadoCLP = totalAprobadoCLP + 0;
                        totalAprobadoCLP_Certificado = totalAprobadoCLP_Certificado + 0;
                    }
                    else
                    {
                        totalAprobadoCLP = totalAprobadoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        totalAprobadoCLP_Certificado = totalAprobadoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
                    {
                        totalVigenteCLP = totalVigenteCLP + 0;
                        totalVigenteCLP_Certificado = totalVigenteCLP_Certificado + 0;
                    }
                    else
                    {
                        totalVigenteCLP = totalVigenteCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        totalVigenteCLP_Certificado = totalVigenteCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        totalPropuestoCLP = totalPropuestoCLP + 0;
                        totalPropuestoCLP_Certificado = totalPropuestoCLP_Certificado + 0;
                    }
                    else
                    {
                        totalPropuestoCLP = totalPropuestoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        totalPropuestoCLP_Certificado = totalPropuestoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "USD")
                {
                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        totalAprobadoUSD = totalAprobadoUSD + 0;
                        totalAprobadoUSD_Certificado = totalAprobadoUSD_Certificado + 0;
                    }
                    else
                    {
                        totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        totalAprobadoUSD_Certificado = totalAprobadoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
                    {
                        totalVigenteUSD = totalVigenteUSD + 0;
                        totalVigenteUSD_Certificado = totalVigenteUSD_Certificado + 0;
                    }
                    else
                    {
                        totalVigenteUSD = totalVigenteUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        totalVigenteUSD_Certificado = totalVigenteUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        totalPropuestoUSD = totalPropuestoUSD + 0;
                        totalPropuestoUSD_Certificado = totalPropuestoUSD_Certificado + 0;
                    }
                    else
                    {
                        totalPropuestoUSD = totalPropuestoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        totalPropuestoUSD_Certificado = totalPropuestoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                XmlNode RespNodeAval;
                RespNodeAval = doc.CreateElement("OperacionVigente");

                nodo = doc.CreateElement("N");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["N"].ToString()));
                RespNodeAval.AppendChild(nodo);

                nodo1 = doc.CreateElement("TipoFinanciamiento");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Tipo Financiamiento"].ToString()));
                RespNodeAval.AppendChild(nodo1);

                nodo2 = doc.CreateElement("Producto");
                nodo2.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Producto"].ToString()));
                RespNodeAval.AppendChild(nodo2);

                nodo3 = doc.CreateElement("Finalidad");
                nodo3.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Finalidad"].ToString()));
                RespNodeAval.AppendChild(nodo3);

                nodo4 = doc.CreateElement("Plazo");
                nodo4.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Plazo"].ToString()));
                RespNodeAval.AppendChild(nodo4);

                nodo5 = doc.CreateElement("Comision");
                nodo5.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Comisión Min. %"].ToString()));
                RespNodeAval.AppendChild(nodo5);

                nodo6 = doc.CreateElement("Seguro");
                nodo6.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Seguro"].ToString()));
                RespNodeAval.AppendChild(nodo6);

                nodo7 = doc.CreateElement("ComisionCLP");
                nodo7.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Comisión"].ToString()));
                RespNodeAval.AppendChild(nodo7);

                nodo8 = doc.CreateElement("MontoCredito");
                nodo8.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Crédito"].ToString()));
                RespNodeAval.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("TipoMO");
                nodo9.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Tipo Moneda"].ToString()));
                RespNodeAval.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("MontoAprobado");
                nodo10.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()));
                RespNodeAval.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("MontoVigente");
                nodo11.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()));
                RespNodeAval.AppendChild(nodo11);

                XmlNode nodo12 = doc.CreateElement("MontoPropuesto");
                nodo12.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()));
                RespNodeAval.AppendChild(nodo12);

                XmlNode nodo13 = doc.CreateElement("Horizonte");
                nodo13.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Horizonte"].ToString()));
                RespNodeAval.AppendChild(nodo13);

                XmlNode nodo14 = doc.CreateElement("CoberturaCertificado");
                nodo14.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["coberturaCertificado"].ToString()));
                RespNodeAval.AppendChild(nodo14);

                RespNodeA.AppendChild(RespNodeAval);
            }
            ValoresNode.AppendChild(RespNodeA);

            XmlNode RespNodeB;
            root = doc.DocumentElement;
            RespNodeB = doc.CreateElement("OperacionesNuevas");

            double NtotalAprobadoCLP = 0;
            double NtotalAprobadoCLP_Certificado = 0;
            double NtotalVigenteCLP = 0;
            double NtotalVigenteCLP_Certificado = 0;
            double NtotalPropuestoCLP = 0;
            double NtotalPropuestoCLP_Certificado = 0;

            double NtotalAprobadoUF = 0;
            double NtotalAprobadoUF_Certificado = 0;
            double NtotalVigenteUF = 0;
            double NtotalVigenteUF_Certificado = 0;
            double NtotalPropuestoUF = 0;

            double NtotalAprobadoUSD = 0;
            double NtotalAprobadoUSD_Certificado = 0;
            double NtotalVigenteUSD = 0;
            double NtotalVigenteUSD_Certificado = 0;
            double NtotalPropuestoUSD = 0;
            double NtotalPropuestoUSD_Certificado = 0;

            //OPERACIONES NUEVAS
            for (int i = 0; i < res1.Tables[3].Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["CoberturaCertificado"].ToString()))
                    coberturaCertificado = 1;
                else
                    coberturaCertificado = double.Parse(res1.Tables[3].Rows[i]["CoberturaCertificado"].ToString()) / 100;

                if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "UF")
                {
                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        NtotalAprobadoUF = NtotalAprobadoUF + 0;
                        NtotalAprobadoUF_Certificado = NtotalAprobadoUF_Certificado + 0;
                    }
                    else
                    {
                        NtotalAprobadoUF = NtotalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalAprobadoUF_Certificado = NtotalAprobadoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
                    {
                        NtotalVigenteUF = NtotalVigenteUF + 0;
                        NtotalVigenteUF_Certificado = NtotalVigenteUF_Certificado + 0;
                    }
                    else
                    {
                        NtotalVigenteUF = NtotalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalVigenteUF_Certificado = NtotalVigenteUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        NtotalPropuestoUF = NtotalPropuestoUF + 0;
                    }
                    else
                    {
                        NtotalPropuestoUF = NtotalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                    }
                }

                if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "CLP")
                {
                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        NtotalAprobadoCLP = NtotalAprobadoCLP + 0;
                        NtotalAprobadoCLP_Certificado = NtotalAprobadoCLP_Certificado + 0;
                    }
                    else
                    {
                        NtotalAprobadoCLP = NtotalAprobadoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalAprobadoCLP_Certificado = NtotalAprobadoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
                    {
                        NtotalVigenteCLP = NtotalVigenteCLP + 0;
                        NtotalVigenteCLP_Certificado = NtotalVigenteCLP_Certificado + 0;
                    }
                    else
                    {
                        NtotalVigenteCLP = NtotalVigenteCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalVigenteCLP_Certificado = NtotalVigenteCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        NtotalPropuestoCLP = NtotalPropuestoCLP + 0;
                        NtotalPropuestoCLP_Certificado = NtotalPropuestoCLP_Certificado + 0;
                    }
                    else
                    {
                        NtotalPropuestoCLP = NtotalPropuestoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalPropuestoCLP_Certificado = NtotalPropuestoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "USD")
                {
                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
                    {
                        NtotalAprobadoUSD = NtotalAprobadoUSD + 0;
                        NtotalAprobadoUSD_Certificado = NtotalAprobadoUSD_Certificado + 0;
                    }
                    else
                    {
                        NtotalAprobadoUSD = NtotalAprobadoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalAprobadoUSD_Certificado = NtotalAprobadoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
                    {
                        NtotalVigenteUSD = NtotalVigenteUSD + 0;
                        NtotalVigenteUSD_Certificado = NtotalVigenteUSD_Certificado + 0;
                    }
                    else
                    {
                        NtotalVigenteUSD = NtotalVigenteUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalVigenteUSD_Certificado = NtotalVigenteUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }

                    if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
                    {
                        NtotalPropuestoUSD = NtotalPropuestoUSD + 0;
                        NtotalPropuestoUSD_Certificado = NtotalPropuestoUSD_Certificado + 0;
                    }
                    else
                    {
                        NtotalPropuestoUSD = NtotalPropuestoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
                        NtotalPropuestoUSD_Certificado = NtotalPropuestoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
                    }
                }

                XmlNode RespNodeAval;
                RespNodeAval = doc.CreateElement("OperacionNueva");

                nodo = doc.CreateElement("N");
                nodo.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["N"].ToString()));
                RespNodeAval.AppendChild(nodo);

                nodo1 = doc.CreateElement("TipoFinanciamiento");
                nodo1.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Tipo Financiamiento"].ToString()));
                RespNodeAval.AppendChild(nodo1);

                nodo2 = doc.CreateElement("Producto");
                nodo2.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Producto"].ToString()));
                RespNodeAval.AppendChild(nodo2);

                nodo3 = doc.CreateElement("Finalidad");
                nodo3.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Finalidad"].ToString()));
                RespNodeAval.AppendChild(nodo3);

                nodo4 = doc.CreateElement("Plazo");
                nodo4.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Plazo"].ToString()));
                RespNodeAval.AppendChild(nodo4);

                nodo5 = doc.CreateElement("Comision");
                nodo5.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Comisión Min. %"].ToString()));
                RespNodeAval.AppendChild(nodo5);

                nodo6 = doc.CreateElement("Seguro");
                nodo6.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Seguro"].ToString()));
                RespNodeAval.AppendChild(nodo6);

                nodo7 = doc.CreateElement("ComisionCLP");
                nodo7.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Comisión"].ToString()));
                RespNodeAval.AppendChild(nodo7);

                nodo8 = doc.CreateElement("MontoCredito");
                nodo8.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Crédito"].ToString()));
                RespNodeAval.AppendChild(nodo8);

                XmlNode nodo9 = doc.CreateElement("TipoMO");
                nodo9.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Tipo Moneda"].ToString()));
                RespNodeAval.AppendChild(nodo9);

                XmlNode nodo10 = doc.CreateElement("MontoAprobado");
                nodo10.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()));
                RespNodeAval.AppendChild(nodo10);

                XmlNode nodo11 = doc.CreateElement("MontoVigente");
                nodo11.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()));
                RespNodeAval.AppendChild(nodo11);

                XmlNode nodo12 = doc.CreateElement("MontoPropuesto");
                nodo12.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()));
                RespNodeAval.AppendChild(nodo12);

                XmlNode nodo13 = doc.CreateElement("Horizonte");
                nodo13.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Horizonte"].ToString()));
                RespNodeAval.AppendChild(nodo13);

                XmlNode nodo14 = doc.CreateElement("CoberturaCertificado");
                nodo14.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["coberturaCertificado"].ToString().Trim()));
                RespNodeAval.AppendChild(nodo14);

                RespNodeB.AppendChild(RespNodeAval);
            }
            ValoresNode.AppendChild(RespNodeB);
            //tablas indicadores

            XmlNode RespNodeI1;
            root = doc.DocumentElement;
            RespNodeI1 = doc.CreateElement("Indicadores1");

            for (int i = 0; i < res1.Tables[7].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador1");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("POR1");
                nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("POR2");
                nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("POR3");
                nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("POR4");
                nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo7);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[7].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI1.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI1);

            XmlNode RespNodeI2;
            root = doc.DocumentElement;
            RespNodeI2 = doc.CreateElement("Indicadores2");

            for (int i = 0; i < res1.Tables[8].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador2");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("POR1");
                nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("POR2");
                nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("POR3");
                nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("POR4");
                nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo7);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[8].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI2.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI2);

            XmlNode RespNodeI3;
            root = doc.DocumentElement;
            RespNodeI3 = doc.CreateElement("Indicadores3");

            for (int i = 0; i < res1.Tables[9].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador3");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo1 = doc.CreateElement("POR1");
                nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo1);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo3 = doc.CreateElement("POR2");
                nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo3);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo5 = doc.CreateElement("POR3");
                nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo5);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo7 = doc.CreateElement("POR4");
                nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo7);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[9].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI3.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI3);

            XmlNode RespNodeI4;
            root = doc.DocumentElement;
            RespNodeI4 = doc.CreateElement("Indicadores4");

            for (int i = 0; i < res1.Tables[10].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador4");
                if (res1.Tables[10].Rows[i]["Cuenta"].ToString() != "Capital de Trabajo (M$)")
                {
                    nodo = doc.CreateElement("VAL1");
                    nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo);

                    nodo2 = doc.CreateElement("VAL2");
                    nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo2);

                    nodo4 = doc.CreateElement("VAL3");
                    nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo4);

                    nodo6 = doc.CreateElement("VAL4");
                    nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo6);

                    nodo8 = doc.CreateElement("TEXTO");
                    nodo8.AppendChild(doc.CreateTextNode((res1.Tables[10].Rows[i]["Cuenta"].ToString())));
                    RespNodeGarantia.AppendChild(nodo8);
                }
                else
                {
                    nodo = doc.CreateElement("VAL1");
                    nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo);

                    nodo2 = doc.CreateElement("VAL2");
                    nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo2);

                    nodo4 = doc.CreateElement("VAL3");
                    nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo4);

                    nodo6 = doc.CreateElement("VAL4");
                    nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
                    RespNodeGarantia.AppendChild(nodo6);

                    nodo8 = doc.CreateElement("TEXTO");
                    nodo8.AppendChild(doc.CreateTextNode((res1.Tables[10].Rows[i]["Cuenta"].ToString())));
                    RespNodeGarantia.AppendChild(nodo8);
                }
                RespNodeI4.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI4);

            XmlNode RespNodeI5;
            root = doc.DocumentElement;
            RespNodeI5 = doc.CreateElement("Indicadores5");

            for (int i = 0; i < res1.Tables[11].Rows.Count; i++)
            {
                XmlNode RespNodeGarantia;
                RespNodeGarantia = doc.CreateElement("Indicador5");

                nodo = doc.CreateElement("VAL1");
                nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo);

                nodo2 = doc.CreateElement("VAL2");
                nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo2);

                nodo4 = doc.CreateElement("VAL3");
                nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo4);

                nodo6 = doc.CreateElement("VAL4");
                nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodeGarantia.AppendChild(nodo6);

                nodo8 = doc.CreateElement("TEXTO");
                nodo8.AppendChild(doc.CreateTextNode((res1.Tables[11].Rows[i]["Cuenta"].ToString())));
                RespNodeGarantia.AppendChild(nodo8);

                RespNodeI5.AppendChild(RespNodeGarantia);
            }
            ValoresNode.AppendChild(RespNodeI5);


            //TOTALES de la PAF
            XmlNode RespNodePAF;

            RespNodePAF = doc.CreateElement("TOTALESPAF");
            //operaciones vigentes

            XmlNode nodoPT = doc.CreateElement("totalAprobadoCLP");
            nodoPT.AppendChild(doc.CreateTextNode(totalAprobadoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT);

            XmlNode nodoPT1 = doc.CreateElement("totalVigenteCLP");
            nodoPT1.AppendChild(doc.CreateTextNode(totalVigenteCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT1);

            XmlNode nodoPT2 = doc.CreateElement("totalPropuestoCLP");
            nodoPT2.AppendChild(doc.CreateTextNode(totalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT2);

            XmlNode nodoPT3 = doc.CreateElement("totalAprobadoUF");
            nodoPT3.AppendChild(doc.CreateTextNode(totalAprobadoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT3);

            XmlNode nodoPT4 = doc.CreateElement("totalVigenteUF");
            nodoPT4.AppendChild(doc.CreateTextNode(totalVigenteUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT4);

            if (!string.IsNullOrEmpty(res1.Tables[4].Rows[0]["ValorUF"].ToString()))
            {
                XmlNode nodoPT5 = doc.CreateElement("totalPropuestoUF");
                nodoPT5.AppendChild(doc.CreateTextNode((totalPropuestoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
                RespNodePAF.AppendChild(nodoPT5);
            }
            else
            {
                XmlNode nodoPT5 = doc.CreateElement("totalPropuestoUF");
                nodoPT5.AppendChild(doc.CreateTextNode("0"));
                RespNodePAF.AppendChild(nodoPT5);
            }

            XmlNode nodoPT6 = doc.CreateElement("totalAprobadoUSD");
            nodoPT6.AppendChild(doc.CreateTextNode(totalAprobadoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT6);

            XmlNode nodoPT7 = doc.CreateElement("totalVigenteUSD");
            nodoPT7.AppendChild(doc.CreateTextNode(totalVigenteUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT7);

            XmlNode nodoPT8 = doc.CreateElement("totalPropuestoUSD");
            nodoPT8.AppendChild(doc.CreateTextNode(totalPropuestoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT8);

            XmlNode nodoPT88 = doc.CreateElement("ValorUF");
            nodoPT88.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT88);

            XmlNode nodoPT89 = doc.CreateElement("ValorDolar");
            nodoPT89.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT89);

            //calculo RE
            double totalAprobadoREUF = totalAprobadoCLP > 0 ? (totalAprobadoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            totalAprobadoREUF = totalAprobadoUSD > 0 ? ((totalAprobadoUSD * (float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString()) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString()))) + totalAprobadoREUF) : totalAprobadoREUF;
            totalAprobadoREUF = totalAprobadoREUF + totalAprobadoUF;

            double totalVigenteREUF = totalVigenteCLP > 0 ? (totalVigenteCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            totalVigenteREUF = totalVigenteUSD > 0 ? ((totalVigenteUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + totalVigenteREUF : totalVigenteREUF;
            totalVigenteREUF = totalVigenteREUF + totalVigenteUF;

            double totalPropuestoREUF = totalPropuestoCLP > 0 ? (totalPropuestoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            totalPropuestoREUF = totalPropuestoUSD > 0 ? ((totalPropuestoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + totalPropuestoREUF : totalPropuestoREUF;
            totalPropuestoREUF = totalPropuestoREUF + totalPropuestoUF;

            XmlNode nodoPT9 = doc.CreateElement("totalAprobadoREUF");
            nodoPT9.AppendChild(doc.CreateTextNode(totalAprobadoREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT9);

            XmlNode nodoPT10 = doc.CreateElement("totalVigenteREUF");
            nodoPT10.AppendChild(doc.CreateTextNode(totalVigenteREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT10);

            XmlNode nodoPT11 = doc.CreateElement("totalPropuestoREUF");
            nodoPT11.AppendChild(doc.CreateTextNode((totalPropuestoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoPT11);

            //OperacionesNuevas
            XmlNode nodoNT = doc.CreateElement("NtotalAprobadoCLP");
            nodoNT.AppendChild(doc.CreateTextNode(NtotalAprobadoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT);

            XmlNode nodoNT1 = doc.CreateElement("NtotalVigenteCLP");
            nodoNT1.AppendChild(doc.CreateTextNode(NtotalVigenteCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT1);

            XmlNode nodoNT2 = doc.CreateElement("NtotalPropuestoCLP");
            nodoNT2.AppendChild(doc.CreateTextNode(NtotalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT2);

            XmlNode nodoNT3 = doc.CreateElement("NtotalAprobadoUF");
            nodoNT3.AppendChild(doc.CreateTextNode(NtotalAprobadoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT3);

            XmlNode nodoNT4 = doc.CreateElement("NtotalVigenteUF");
            nodoNT4.AppendChild(doc.CreateTextNode(NtotalVigenteUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT4);

            XmlNode nodoNT5 = doc.CreateElement("NtotalPropuestoUF");
            nodoNT5.AppendChild(doc.CreateTextNode(NtotalPropuestoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT5);

            XmlNode nodoNT6 = doc.CreateElement("NtotalAprobadoUSD");
            nodoNT6.AppendChild(doc.CreateTextNode(NtotalAprobadoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT6);

            XmlNode nodoNT7 = doc.CreateElement("NtotalVigenteUSD");
            nodoNT7.AppendChild(doc.CreateTextNode(NtotalVigenteUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT7);

            XmlNode nodoNT8 = doc.CreateElement("NtotalPropuestoUSD");
            nodoNT8.AppendChild(doc.CreateTextNode(NtotalPropuestoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT8);

            //calculos montos totales operaciones vigentes
            double NtotalAprobadoREUF = NtotalAprobadoCLP > 0 ? (NtotalAprobadoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            NtotalAprobadoREUF = NtotalAprobadoUSD > 0 ? ((NtotalAprobadoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalAprobadoREUF : NtotalAprobadoREUF;
            NtotalAprobadoREUF = NtotalAprobadoUF + NtotalAprobadoREUF;

            double NtotalVigenteREUF = NtotalVigenteCLP > 0 ? (NtotalVigenteCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            NtotalVigenteREUF = NtotalVigenteUSD > 0 ? ((NtotalVigenteUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalVigenteREUF : NtotalVigenteREUF;
            NtotalVigenteREUF = NtotalVigenteUF + NtotalVigenteREUF;

            double NtotalPropuestoREUF = NtotalPropuestoCLP > 0 ? (NtotalPropuestoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
            NtotalPropuestoREUF = NtotalPropuestoUSD > 0 ? ((NtotalPropuestoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalPropuestoREUF : NtotalPropuestoREUF;
            NtotalPropuestoREUF = NtotalPropuestoREUF + NtotalPropuestoUF;

            XmlNode nodoNT9 = doc.CreateElement("NtotalAprobadoREUF");
            nodoNT9.AppendChild(doc.CreateTextNode((NtotalAprobadoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT9);

            XmlNode nodoNT10 = doc.CreateElement("NtotalVigenteREUF");
            nodoNT10.AppendChild(doc.CreateTextNode(NtotalVigenteREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT10);

            XmlNode nodoNT11 = doc.CreateElement("NtotalPropuestoREUF");
            nodoNT11.AppendChild(doc.CreateTextNode(NtotalPropuestoREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoNT11);

            //TotalLineaGlobal
            XmlNode nodoTGlobal = doc.CreateElement("GlobalCLPAprobado");
            nodoTGlobal.AppendChild(doc.CreateTextNode((NtotalAprobadoCLP + totalAprobadoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal);

            XmlNode nodoTGlobal1 = doc.CreateElement("GlobalCLPVigente");
            nodoTGlobal1.AppendChild(doc.CreateTextNode((NtotalVigenteCLP + totalVigenteCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal1);

            XmlNode nodoTGlobal2 = doc.CreateElement("GlobalCLPPropuesto");
            nodoTGlobal2.AppendChild(doc.CreateTextNode((NtotalPropuestoCLP + totalPropuestoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal2);

            XmlNode nodoTGlobal3 = doc.CreateElement("GlobalUFAprobado");
            nodoTGlobal3.AppendChild(doc.CreateTextNode((NtotalAprobadoUF + totalAprobadoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal3);

            XmlNode nodoTGlobal4 = doc.CreateElement("GlobalUFVigente");
            nodoTGlobal4.AppendChild(doc.CreateTextNode((NtotalVigenteUF + totalVigenteUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal4);

            XmlNode nodoTGlobal5 = doc.CreateElement("GlobalUFPropuesto");
            nodoTGlobal5.AppendChild(doc.CreateTextNode(((NtotalPropuestoUF + totalPropuestoUF) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal5);

            XmlNode nodoTGlobal6 = doc.CreateElement("GlobalUSDAprobado");
            nodoTGlobal6.AppendChild(doc.CreateTextNode((NtotalAprobadoUSD + totalAprobadoUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal6);

            XmlNode nodoTGlobal7 = doc.CreateElement("GlobalUSDPropuesto");
            nodoTGlobal7.AppendChild(doc.CreateTextNode((NtotalPropuestoUSD + totalPropuestoUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal7);

            XmlNode nodoTGlobal8 = doc.CreateElement("GlobalUSDVigente");
            nodoTGlobal8.AppendChild(doc.CreateTextNode((NtotalVigenteUSD + totalVigenteUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal8);

            XmlNode nodoTGlobal9 = doc.CreateElement("GlobalREUFAprobado");
            nodoTGlobal9.AppendChild(doc.CreateTextNode((NtotalAprobadoREUF + totalAprobadoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal9);

            XmlNode nodoTGlobal10 = doc.CreateElement("GlobalREUFVigente");
            nodoTGlobal10.AppendChild(doc.CreateTextNode((NtotalVigenteREUF + totalVigenteREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal10);

            XmlNode nodoTGlobal11 = doc.CreateElement("GlobalREUFPropuesto");
            nodoTGlobal11.AppendChild(doc.CreateTextNode((NtotalPropuestoREUF + totalPropuestoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTGlobal11);

            //garantias
            XmlNode nodoTG = doc.CreateElement("TotalGarantiaComercial");
            nodoTG.AppendChild(doc.CreateTextNode((TotalGarantiaComercial * float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG);

            XmlNode nodoTG1 = doc.CreateElement("TotalGarantiaAjustado");
            nodoTG1.AppendChild(doc.CreateTextNode((TotalGarantiaAjustado * float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG1);

            XmlNode nodoTG22 = doc.CreateElement("TotalGarantiaComercialUF");
            nodoTG22.AppendChild(doc.CreateTextNode((TotalGarantiaComercial).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG22);

            XmlNode nodoTG33 = doc.CreateElement("TotalGarantiaAjustadoUF");
            nodoTG33.AppendChild(doc.CreateTextNode((TotalGarantiaAjustado).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
            RespNodePAF.AppendChild(nodoTG33);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //totales vigente garantia COBERTURA VIGENTE
            XmlNode nodoTG2 = doc.CreateElement("TotalComercialCV");
            if ((NtotalVigenteREUF + totalVigenteREUF) > 0 && TotalCoberturaComercialVigente > 0)
            {
                var b = totalPropuestoUF_Certificado;
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());

                var a = totalVigenteCLP_Certificado;
                var bb = totalVigenteREUF;

                nodoTG2.AppendChild(doc.CreateTextNode((100.0 * TotalCoberturaComercialVigente / (totalVigenteREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG2.AppendChild(doc.CreateTextNode("0,00"));

            RespNodePAF.AppendChild(nodoTG2);

            XmlNode nodoTG3 = doc.CreateElement("TotalaAjustadoCV");
            if ((NtotalVigenteREUF + totalVigenteREUF) > 0 && TotalCoberturaAjustadoVigente > 0)
            {
                var b = totalPropuestoUF_Certificado;
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                nodoTG3.AppendChild(doc.CreateTextNode((100.0 * TotalCoberturaAjustadoVigente / (totalVigenteREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG3.AppendChild(doc.CreateTextNode("0,00"));
            RespNodePAF.AppendChild(nodoTG3);

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //totales Globales garantia COBERTURA GLOBAL
            var A = TotalCoberturaAjustadoNuevo;
            var B = TotalGarantiaNuevaComercial;
            XmlNode nodoTG4 = doc.CreateElement("TotalComercialCG");
            if ((NtotalPropuestoREUF + totalPropuestoREUF) > 0.0 && TotalGarantiaComercial > 0.0)
            {
                double NclpCertificado = NtotalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                nodoTG4.AppendChild(doc.CreateTextNode((100.0 * TotalGarantiaComercial / (NtotalPropuestoREUF + totalPropuestoREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG4.AppendChild(doc.CreateTextNode("0,00"));
            RespNodePAF.AppendChild(nodoTG4);

            XmlNode nodoTG5 = doc.CreateElement("TotalAjustadoCG");
            if ((NtotalPropuestoREUF + totalPropuestoREUF) > 0.0 && TotalGarantiaAjustado > 0.0)
            {
                double NclpCertificado = NtotalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
                nodoTG5.AppendChild(doc.CreateTextNode((100.0 * TotalGarantiaAjustado / (NtotalPropuestoREUF + totalPropuestoREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
            }
            else nodoTG5.AppendChild(doc.CreateTextNode("0,00"));
            RespNodePAF.AppendChild(nodoTG5);


            ValoresNode.AppendChild(RespNodePAF);
            return doc.OuterXml;
        }

        public byte[] GenerarReporteVaciado(int idEmpresa)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            LogicaNegocio Ln = new LogicaNegocio();
            Utilidades util = new Utilidades();
            try
            {
                String xml = String.Empty;
                DataSet res1 = new DataSet();

                res1 = Ln.ConsultaReporteVaciadoBalance(idEmpresa);
                for (int i = 0; i < res1.Tables[0].Rows.Count; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }
                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "VaciadoBalance" + ".xslt");
                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public byte[] GenerarReportePosicionCliente(string sp, Dictionary<string, string> datos, string id)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            Utilidades util = new Utilidades();
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                String xml = String.Empty;

                //IF PARA CADA REPORTE
                DataSet res1 = new DataSet();
                if (sp == "Ficha_Comercial")
                {
                    res1 = Ln.ConsultaFichaComercial(datos["IdEmpresa"], "ReportePosicionCliente");
                    for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                    {
                        xml = xml + res1.Tables[0].Rows[i][0].ToString();
                    }
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");

                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public byte[] GenerarReporteScor(int idEmpresa)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            Utilidades util = new Utilidades();
            LogicaNegocio Ln = new LogicaNegocio();
            try
            {
                String xml = String.Empty;
                DataSet res1 = new DataSet();

                res1 = Ln.ConsultaReporteScoring(idEmpresa, "usuario", "perfil");
                for (int i = 0; i < res1.Tables[0].Rows.Count; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Scoring" + ".xslt");
                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public byte[] GenerarReporteCompromisos(string sp, Dictionary<string, string> datos, string id, int IdOperacion)
        {
            LogicaNegocio Ln = new LogicaNegocio();
            Utilidades util = new Utilidades();
            try
            {
                String xml = String.Empty;
                string html = string.Empty;
                System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();

                DataSet res1 = new DataSet();

                res1 = Ln.ReporteCompromisos(IdOperacion);

                for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");
                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public byte[] CrearReporteCurse(string Reporte, string sp, string nCertificado)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            try
            {
                Utilidades util = new Utilidades();
                String xml = String.Empty;
                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Ncertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = nCertificado;

                xml = AD_Global.traerPrimeraColumna(sp, SqlParametros);

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + Reporte + ".xslt");
                }

                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public byte[] GenerarReporteCFC(int IdCotizacion, string sp)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            try
            {
                string xml = string.Empty;
                Utilidades util = new Utilidades();

                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdCotizacion", SqlDbType.Int);
                SqlParametros[0].Value = IdCotizacion;

                xml = AD_Global.traerPrimeraColumna(sp, SqlParametros);

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                if (sp == "ListarDetalleCFT")
                {
                    using (XmlWriter writer = newTree.CreateWriter())
                    {
                        xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "CFT" + ".xslt");
                    }
                }
                else
                {
                    using (XmlWriter writer = newTree.CreateWriter())
                    {
                        xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "CFC" + ".xslt");
                    }
                }

                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        public byte[] GenerarReporteProrrateo(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        {
            System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
            string html = string.Empty;
            try
            {
                String xml = String.Empty;
                DataSet res1 = new DataSet();
                LogicaNegocio Ln = new LogicaNegocio();
                Utilidades util = new Utilidades();

                res1 = Ln.ConsultaProrrateoGarantias(datos["IdEmpresa"], "DocumentoCurse", "admin", "ReporteProrrateoEmpresa");
                for (int i = 0; i <= res1.Tables[0].Rows.Count - 1; i++)
                {
                    xml = xml + res1.Tables[0].Rows[i][0].ToString();
                }

                XDocument newTree = new XDocument();
                XslCompiledTransform xsltt = new XslCompiledTransform();

                using (XmlWriter writer = newTree.CreateWriter())
                {
                    xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");
                }
                using (var sw = new StringWriter())
                using (var sr = new StringReader(xml))
                using (var xr = XmlReader.Create(sr))
                {
                    xsltt.Transform(xr, null, sw);
                    html = sw.ToString();
                }
                try
                {
                    sDocumento.Append(html);
                    return util.ConvertirAPDF_Control(sDocumento);
                }
                catch { }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return null;
        }

        //public byte[] GenerarReporteCFC(int IdCotizacion, string sp)
        //{
        //    System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        //    string html = string.Empty;
        //    try
        //    {
        //        string xml = string.Empty;
        //        Utilidades util = new Utilidades();

        //        SqlParameter[] SqlParametros = new SqlParameter[1];
        //        SqlParametros[0] = new SqlParameter("@IdCotizacion", SqlDbType.Int);
        //        SqlParametros[0].Value = IdCotizacion;

        //        xml = AD_Global.traerPrimeraColumna(sp, SqlParametros);

        //        XDocument newTree = new XDocument();
        //        XslCompiledTransform xsltt = new XslCompiledTransform();

        //        if (sp == "ListarDetalleCFT")
        //        {
        //            using (XmlWriter writer = newTree.CreateWriter())
        //            {
        //                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "CFT" + ".xslt");
        //            }
        //        }
        //        else
        //        {
        //            using (XmlWriter writer = newTree.CreateWriter())
        //            {
        //                xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "CFC" + ".xslt");
        //            }
        //        }

        //        using (var sw = new StringWriter())
        //        using (var sr = new StringReader(xml))
        //        using (var xr = XmlReader.Create(sr))
        //        {
        //            xsltt.Transform(xr, null, sw);
        //            html = sw.ToString();
        //        }
        //        try
        //        {
        //            sDocumento.Append(html);
        //            return util.ConvertirAPDF_Control(sDocumento);
        //        }
        //        catch { }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //    }
        //    return null;
        //}

    }
}
