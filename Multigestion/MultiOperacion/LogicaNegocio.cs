using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using FrameworkIntercapIT.Utilities;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.Collections;
using Microsoft.SharePoint.Utilities;
using System.Text.RegularExpressions;
using MultigestionUtilidades;

namespace MultiOperacion
{
    class LogicaNegocio
    {
        public string CP_InformacionCertificado(string NroCertificado)
        {
            try
            {
                string retorno = string.Empty;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = NroCertificado;

                retorno = AD_Global.traerPrimeraColumna("CP_InformacionCertificado", SqlParametros);
                return retorno;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return string.Empty;
            }
        }

        public int CP_VerificarCertificado(string NroCertificado)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = NroCertificado;

                var resultado = AD_Global.traerPrimeraColumna("VerificarCertificado", SqlParametros);
                return int.Parse(resultado);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return -1;
            }
        }

        public string CP_VerificarMonedaOperacion(string NroCertificado)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = NroCertificado;

                return AD_Global.traerPrimeraColumna("VerificarMonedaOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return string.Empty;
            }
        }

        public DataTable CP_GestionCalendarioPagp(string acreedor, string certificado, string rut, Boolean filtroVencidos, Boolean filtroPendiente, Boolean filtroPagados, Boolean filtroProximos, Boolean filtroTodos, string usuario, string perfil, string opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@acreedor", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(acreedor);
                SqlParametros[1] = new SqlParameter("@certificado", SqlDbType.NChar);
                SqlParametros[1].Value = certificado;
                SqlParametros[2] = new SqlParameter("@rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@filtroVencidos", SqlDbType.Bit);
                SqlParametros[3].Value = filtroVencidos;
                SqlParametros[4] = new SqlParameter("@filtroPendiente", SqlDbType.Bit);
                SqlParametros[4].Value = filtroPendiente;
                SqlParametros[5] = new SqlParameter("@filtroPagados", SqlDbType.Bit);
                SqlParametros[5].Value = filtroPagados;
                SqlParametros[6] = new SqlParameter("@filtroProximos", SqlDbType.Bit);
                SqlParametros[6].Value = filtroProximos;
                SqlParametros[7] = new SqlParameter("@filtroTodos", SqlDbType.Bit);
                SqlParametros[7].Value = filtroTodos;
                SqlParametros[8] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[8].Value = opcion;
                SqlParametros[9] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[9].Value = usuario;
                SqlParametros[10] = new SqlParameter("@descCargo", SqlDbType.NChar);
                SqlParametros[10].Value = perfil;

                dt = AD_Global.ejecutarConsultas("GestionCalendarioPagp", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean CP_ActualizarDatos(int IdCalendario, decimal InteresPagad, decimal MontoCuotaPagad, decimal CapitalPagad, int? OrigenPago, string usuario, string perfil, DateTime? fechaPago, string NroCredito, string NroCertificado, string CuotaNro, string NroCuota, DateTime? FecVencimiento, string opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@IdCalendario", SqlDbType.Int);
                SqlParametros[0].Value = IdCalendario;
                SqlParametros[1] = new SqlParameter("@InteresPagad", SqlDbType.Float);
                SqlParametros[1].Value = InteresPagad;
                SqlParametros[2] = new SqlParameter("@MontoCuotaPagad", SqlDbType.Float);
                SqlParametros[2].Value = MontoCuotaPagad;
                SqlParametros[3] = new SqlParameter("@CapitalPagad", SqlDbType.Float);
                SqlParametros[3].Value = CapitalPagad;
                SqlParametros[4] = new SqlParameter("@OrigenPago", SqlDbType.Int);
                SqlParametros[4].Value = OrigenPago;
                SqlParametros[5] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[5].Value = usuario;
                SqlParametros[6] = new SqlParameter("@descCargo", SqlDbType.NChar);
                SqlParametros[6].Value = perfil;
                SqlParametros[7] = new SqlParameter("@fechaPago", SqlDbType.DateTime);
                SqlParametros[7].Value = fechaPago;
                SqlParametros[8] = new SqlParameter("@NroCredito", SqlDbType.NVarChar);
                SqlParametros[8].Value = NroCredito;
                SqlParametros[9] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[9].Value = NroCertificado;
                SqlParametros[10] = new SqlParameter("@CuotaNro", SqlDbType.NVarChar);
                SqlParametros[10].Value = CuotaNro;
                SqlParametros[11] = new SqlParameter("@NroCuota", SqlDbType.Int);
                SqlParametros[11].Value = NroCuota;
                SqlParametros[12] = new SqlParameter("@FecVencimiento", SqlDbType.DateTime);
                SqlParametros[12].Value = FecVencimiento;
                SqlParametros[13] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[13].Value = opcion;

                return AD_Global.ejecutarAccion("GestionCalendarioPagp", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public Boolean CP_InsertarDatos(string usuario, string perfil, string xmlCalendario, decimal CapitalInicial, string opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[0].Value = usuario;
                SqlParametros[1] = new SqlParameter("@descCargo", SqlDbType.NChar);
                SqlParametros[1].Value = perfil;
                SqlParametros[2] = new SqlParameter("@xmlCalendario", SqlDbType.Xml);
                SqlParametros[2].Value = xmlCalendario;
                SqlParametros[3] = new SqlParameter("@CapitalInicial", SqlDbType.Decimal);
                SqlParametros[3].Value = CapitalInicial;
                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = opcion;

                return AD_Global.ejecutarAccion("GestionCalendarioPagp", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ListarDatosCartera(string acreedor, string edoCertificado, string ejecutivo, string usuario, string perfil, string NCERT, string rut, string empresa, string etapa, string fondo)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@acreedor", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(acreedor);
                SqlParametros[1] = new SqlParameter("@edoCertificado", SqlDbType.NChar);
                SqlParametros[1].Value = edoCertificado;
                SqlParametros[2] = new SqlParameter("@ejecutivo", SqlDbType.NChar);
                SqlParametros[2].Value = ejecutivo;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = "01"; //listar
                SqlParametros[4] = new SqlParameter("@Usuario", SqlDbType.NChar);
                SqlParametros[4].Value = usuario;
                SqlParametros[5] = new SqlParameter("@Perfil", SqlDbType.NChar);
                SqlParametros[5].Value = perfil;
                SqlParametros[6] = new SqlParameter("@NCER", SqlDbType.NChar);
                SqlParametros[6].Value = NCERT;

                SqlParametros[7] = new SqlParameter("@filtroRut", SqlDbType.NChar);
                SqlParametros[7].Value = rut;

                SqlParametros[8] = new SqlParameter("@filtroEmpresa", SqlDbType.NChar);
                SqlParametros[8].Value = empresa;

                SqlParametros[9] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[9].Value = etapa;

                SqlParametros[10] = new SqlParameter("@Fondo", SqlDbType.NVarChar);
                SqlParametros[10].Value = fondo;

                dt = AD_Global.ejecutarConsulta("ListarCartera", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean GuardarDevolucion(int idempresa, int IdOperacion, string usuario, string cargo, string IdTipoTransaccion, string DescTipoTransaccion, string FechaTransaccion, string IdTipoContrato, string DescTipoContrato, string PorcDevolucion, string DevolucionCLP, string PorcDescuentoFijo, string DescuentoFijoCLP, string DevolucionCostoFondo, string Abonos, string Cargos, string accion)
        {
            try
            {
                Boolean dt = false;
                SqlParameter[] SqlParametros = new SqlParameter[17];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idempresa;
                SqlParametros[1] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[1].Value = IdOperacion;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargo;

                SqlParametros[4] = new SqlParameter("@IdTipoTransaccion", SqlDbType.Int);
                SqlParametros[4].Value = int.Parse(IdTipoTransaccion);
                SqlParametros[5] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[5].Value = DescTipoTransaccion;
                SqlParametros[6] = new SqlParameter("@FechaTransaccion", SqlDbType.NVarChar);
                SqlParametros[6].Value = FechaTransaccion;
                SqlParametros[7] = new SqlParameter("@IdTipoContrato", SqlDbType.Int);
                SqlParametros[7].Value = int.Parse(IdTipoContrato);

                SqlParametros[8] = new SqlParameter("@PorcDevolucion", SqlDbType.Float);
                SqlParametros[8].Value = PorcDevolucion == "" ? float.Parse("0") : float.Parse(PorcDevolucion);
                SqlParametros[9] = new SqlParameter("@DevolucionCLP", SqlDbType.Float);
                SqlParametros[9].Value = DevolucionCLP == "" ? float.Parse("0") : float.Parse(DevolucionCLP);
                SqlParametros[10] = new SqlParameter("@PorcDescuentoFijo", SqlDbType.Float);
                SqlParametros[10].Value = float.Parse(PorcDescuentoFijo);
                SqlParametros[11] = new SqlParameter("@DescuentoFijoCLP", SqlDbType.Float);
                SqlParametros[11].Value = DescuentoFijoCLP == "" ? float.Parse("0") : float.Parse(DescuentoFijoCLP);
                SqlParametros[12] = new SqlParameter("@DevolucionCostoFondo", SqlDbType.Float);
                SqlParametros[12].Value = DevolucionCostoFondo == "" ? float.Parse("0") : float.Parse(DevolucionCostoFondo);
                SqlParametros[13] = new SqlParameter("@Abonos", SqlDbType.Float);
                SqlParametros[13].Value = Abonos == "" ? float.Parse("0") : float.Parse(Abonos);

                SqlParametros[14] = new SqlParameter("@Cargos", SqlDbType.Float);
                SqlParametros[14].Value = Cargos == "" ? float.Parse("0") : float.Parse(Cargos);

                SqlParametros[15] = new SqlParameter("@DescTipoContrato", SqlDbType.NVarChar);
                SqlParametros[15].Value = DescTipoContrato;


                SqlParametros[16] = new SqlParameter("@accion", SqlDbType.NVarChar);
                SqlParametros[16].Value = accion;

                dt = AD_Global.ejecutarAccion("GuardarDevolucion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
            //ListaDatosEmpresas 35,1
        }

        public DataSet ConsultaReporteBDDevolucion(string idEmpresa, string idOperacion, string ncertificado, string usuario, string perfil, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                SqlParametros[4] = new SqlParameter("@ncertificado", SqlDbType.NVarChar);
                SqlParametros[4].Value = float.Parse(ncertificado);

                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ConsultarDatosBasicoDevolucion(int idempresa, int IdOperacion, string usuario, string cargo)
        {
            try
            {
                DataSet dt = new DataSet();

                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idempresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = IdOperacion;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargo;

                dt = AD_Global.ejecutarConsulta("ConsultarDatosDevolucion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
            //ListaDatosEmpresas 35,1
        }

        public string BuscarFondo(string ID)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Fondos"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='Nombre'/><Value Type='TEXT'>" + ID + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    return oListItem["Porcentaje"].ToString() + " %";
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return "";
            //return mensaje;
        }

        public DataSet ListarDistribucionFondos(string idEmpresa, string idOperacion, string user, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = "04";

                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = user;

                SqlParametros[4] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = perfil;

                dt = AD_Global.ejecutarConsulta("GestionDistribucionFondos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean InsertarDistribucionFondos(string idEmpresa, string idOperacion, string user, string perfil, string xmlDistFondo)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = "01";

                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = user;

                SqlParametros[4] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = perfil;

                SqlParametros[5] = new SqlParameter("@xmlFondos", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlDistFondo;

                return AD_Global.ejecutarAccion("GestionDistribucionFondos", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ConsultarOperacion(int IdOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;
                SqlParametros[1] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[1].Value = 6;

                dt = AD_Global.ejecutarConsultas("GestionOperaciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPlantillaById(int IdPlantillaDocumento)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];

                SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
                SqlParametros[0].Value = IdPlantillaDocumento;

                dt = AD_Global.ejecutarConsultas("ListarPlantillaById", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[13];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = "01";
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;

                SqlParametros[5] = new SqlParameter("@idEstado", SqlDbType.Int);
                SqlParametros[5].Value = Estado;
                SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[6].Value = descEstado;

                SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
                SqlParametros[7].Value = Etapa;
                SqlParametros[8] = new SqlParameter("@descEtapa", SqlDbType.NVarChar);
                SqlParametros[8].Value = descEtapa;

                SqlParametros[9] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[9].Value = subEtapa;
                SqlParametros[10] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[10].Value = descsubEtapa;

                SqlParametros[11] = new SqlParameter("@Area", SqlDbType.NVarChar);
                SqlParametros[11].Value = Area;

                SqlParametros[12] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[12].Value = comentario;

                dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ConsultarDocumentosParametrizados(int IdProducto, int IdAcreedor, string area)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                SqlParametros[0].Value = IdProducto;
                SqlParametros[1] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[1].Value = IdAcreedor;
                SqlParametros[2] = new SqlParameter("@area", SqlDbType.NVarChar);
                SqlParametros[2].Value = area;
                dt = AD_Global.ejecutarConsultas("ConsultarDocumentosParametrizados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet CargarDatosHtml(int IdOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                dt = AD_Global.ejecutarConsulta("CargarDatosPlantilla", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public int ActualizarEstado(String ID, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area)
        {
            try
            {
                SPWeb mySite = SPContext.Current.Web;
                SPList list = mySite.Lists["Operaciones"];

                SPListItemCollection items = list.GetItems(new SPQuery()
                {
                    Query = @"<Where><Eq><FieldRef Name='IdSQL' /><Value Type='Number'>" + ID + "</Value></Eq></Where>"
                });

                foreach (SPListItem item in items)
                {
                    item["IdEtapa"] = Etapa;
                    item["IdSubEtapa"] = subEtapa;
                    item["IdEstado"] = Estado;
                    item.Update();
                }
                list.Update();
                return 1;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                //logger.Debug("Error en " + ex.Source.ToString() + " : " + ex.Message.ToString());
                return 0;
            }
        }

        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;

        public DataTable ReporteSolicitudEmision(string idEmpresa, string idOperacion, string usuario, string perfil)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;

                dt = AD_Global.ejecutarConsultas("ReporteContratoSubfianza", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable CertificadoElegibilidad(string idEmpresa, string idOperacion, string usuario, string perfil)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;

                dt = AD_Global.ejecutarConsultas("ReporteCertificadoElegibilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ReporteCertificadoFianza(string idEmpresa, string idOperacion, string usuario, string perfil)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;

                dt = AD_Global.ejecutarConsultas("ReporteCertificadoFianza", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ReporteContratoSubfianza(string idEmpresa, string idOperacion, string usuario, string perfil)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;

                dt = AD_Global.ejecutarConsultas("ReporteContratoSubfianza", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        {
            try
            {
                Utilidades util = new Utilidades();
                String xml = String.Empty;

                //IF PARA CADA REPORTE
                DataSet res1 = new DataSet();
                if (sp == "Contrato_de_Subfianza")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteContratoSubfianza", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLContratoSubFianza(res1);
                }
                else if (sp == "Instruccion_de_Curse")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteInstruccionCurse", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLInstruccionCurse(res1);
                }
                else if (sp == "Certificado_de_Fianza_Comercial" || sp == "Certificado_de_Fianza_Banco_Estado" || sp == "Certificado_de_Fianza_Banco_Security" || sp == "Certificado_de_Fianza_Factoring" || sp == "Certificado_de_Fianza_Itau" || sp == "Certificado_de_Fianza_BBVA")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
                    foreach (DataRow fila in res1.Tables[0].Rows)
                        xml = xml + fila[0];
                    //xml = res1.Tables[0].Rows[0][0].ToString();
                    if (sp == "Certificado_de_Fianza_Comercial")
                        xml = GenerarCodigoBarra(res1, objresumen, res1.Tables[1].Rows[0][0].ToString());
                }
                else if (sp == "Certificado_de_Fianza_Tecnica")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCertificadoFianza(res1, id);
                    xml = GenerarCodigoBarra(res1, objresumen, res1.Tables[1].Rows[0][0].ToString());
                }
                else if (sp == "Solicitud_de_Pago_de_Garantia")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCertificadoFianza(res1, id);
                }
                else if (sp == "Certificado_de_Elegibilidad")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoElegibilidad", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCertificadoFianza(res1, id);
                }
                else if (sp == "Carta_de_Garantia" || sp == "Carta_de_Garantia_ITAU" || sp == "Carta_de_Garantia_Santander" || sp == "Carta_de_Garantia_BBVA" || sp == "Carta_de_Garantia_Banco_Estado" || sp == "Carta_de_Garantia_Banco_Security" || sp == "Carta_de_Garantia_Factoring")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCartaGarantias", datos["ValorUF"], datos["ValorUS"]);
                    xml = GenerarXMLCartadeGarantia(res1);
                    xml = xml.Replace("<RutAcreedor>RUTACREEDOR</RutAcreedor>", "<RutAcreedor>" + BuscarIdsYValoresCambioDeEstado(res1.Tables[0].Rows[0]["IdAcreedor"].ToString()).Split('|')[0] + "</RutAcreedor>");
                    xml = xml.Replace("<ClasificacionSBIF></ClasificacionSBIF>", "<ClasificacionSBIF>" + BuscarParametro("MULTIAVAL S.A.G.R.") + "</ClasificacionSBIF>");
                    xml = xml.Replace("<NombreFondoGarantia></NombreFondoGarantia>", "<NombreFondoGarantia>" + BuscarFondo(res1.Tables[0].Rows[0]["Fondo"].ToString()) + "</NombreFondoGarantia>");
                }
                else if (sp == "Solicitud_de_Emision")
                {
                    res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteSolicitudEmision", datos["ValorUF"], datos["ValorUS"]);
                    foreach (DataRow fila in res1.Tables[0].Rows)
                        xml = xml + fila[0];
                    int ini = xml.IndexOf("<IdAcreedor>"), fin = xml.IndexOf("</IdAcreedor>");
                    string idacreedor = xml.Substring(ini + 12, fin - (ini + 12));

                    xml.Replace("<acreedorRUT></acreedorRUT>", "<acreedorRUT>" + BuscarIdsYValoresCambioDeEstado(idacreedor).Split('|')[0] + "</acreedorRUT>");
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

        public byte[] CrearReporte(string Reporte, string sp, string nCertificado)
        {
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

        public DataSet ConsultaReporteBD(string idEmpresa, string idOperacion, string usuario, string perfil, string SP, string ValorUF, string ValorUS)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                SqlParametros[4] = new SqlParameter("@ValorUF", SqlDbType.Float);
                SqlParametros[4].Value = float.Parse(ValorUF);
                SqlParametros[5] = new SqlParameter("@ValorUS", SqlDbType.Float);
                SqlParametros[5].Value = float.Parse(ValorUS);

                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
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

        public string GenerarXMLCertificadoFianza(DataSet res1, string id)
        {
            string xmlt = "";
            res1.Tables[0].Rows[0][0].ToString().Replace("<acreedorRUT>acredor rut</acreedorRUT>", "<acreedorRUT>" + BuscarIdsYValoresCambioDeEstado(id) + "</acreedorRUT>");
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

        public string GenerarCodigoBarra(DataSet res1, Resumen objresumen, string nombrearchivo)
        {
            try
            {
                Code39 code = new Code39(nombrearchivo);

                code.Paint().Save("C:\\inetpub\\wwwroot\\wss\\VirtualDirectories\\80\\xsl\\request\\" + nombrearchivo + ".jpg", ImageFormat.Png);
                Bitmap bitmap1 = (Bitmap)Bitmap.FromFile(@"C:\\inetpub\\wwwroot\\wss\\VirtualDirectories\\80\\xsl\\request\\" + nombrearchivo + ".jpg");
                bitmap1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                bitmap1.Save(@"C:\\inetpub\\wwwroot\\wss\\VirtualDirectories\\80\\xsl\\request\\" + nombrearchivo + ".jpg");
                string xml1 = "";
                foreach (DataRow fila in res1.Tables[0].Rows)
                    xml1 = xml1 + fila[0];
                return xml1.Replace("<CodigoBarra></CodigoBarra>", "<CodigoBarra>" + "http://localhost:25698/" + nombrearchivo + ".jpg" + "</CodigoBarra>");

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //return new DataTable();
            }

            return "";
        }

        public DataSet GenerarXMLRevisionPagare(int idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[1];

                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;

                return AD_Global.ejecutarConsulta("ReporteRevisionPagare", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public string BuscarIdsYValoresCambioDeEstado(string ID)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Acreedores"];
                SPQuery oQuery = new SPQuery();

                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + ID + "</Value></Eq></Where>";

                //oQuery.Query = "<OrderBy>  <FieldRef Name = 'Nombre' Ascending = 'TRUE'/> </OrderBy>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    return oListItem["Rut"].ToString() + "|" + oListItem["Nombre"].ToString() + "|" + oListItem["Domicilio"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return "";
        }

        public bool ExisteArchivo(string tipo, string certificado, Resumen objresumen)
        {
            try
            {
                Utilidades util = new Utilidades();
                SPWeb app = SPContext.Current.Web;
                DataTable tabla = new DataTable();
                var listData = app.Lists["Documentos"];
                //SPWeb app = SPContext.Current.Web;
                var urlLibrary = "";
                var nomLibrary = "";
                SPFolder carpeta;
                urlLibrary = app.Url + "/Documentos/" + util.RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(objresumen.desEmpresa.ToString())) + "/Operaciones/" + "Certificado" + certificado;
                //urlLibrary = app.Url + "/Documentos/" + RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(objresumen.desEmpresa.ToString())) + "/" + RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(objresumen.desOperacion.ToString()));
                nomLibrary = objresumen.area.ToString();

                carpeta = app.GetFolder(urlLibrary + "/");


                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        //string SubFolderUrl = app.Url + "/" + carpeta.ToString();
                        string SubFolderUrl = web.ServerRelativeUrl + carpeta.ToString();
                        SPFolder SubFolder = web.GetFolder(SubFolderUrl);
                        SPQuery query = new SPQuery();
                        query.Query = "<Where><And><And><Eq><FieldRef Name='TipoDocumento'/><Value Type='Text'>" + tipo + "</Value></Eq><Eq><FieldRef Name='IdOperacion'/><Value Type='Text'>" + objresumen.idOperacion + "</Value></Eq></And><Eq><FieldRef Name='IdEmpresa'/><Value Type='Text'>" + objresumen.idEmpresa + "</Value></Eq></And></Where>";

                        query.Folder = SubFolder;  // This should restrict the query to the subfolder
                        SPDocumentLibrary list = SubFolder.DocumentLibrary;

                        SPListItemCollection files = list.GetItems(query);


                        foreach (SPListItem item in files)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return false;
        }

        public string BuscarParametro(string ID)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Parametros"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='TEXT'>" + ID + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    return oListItem["Valor"].ToString() + " " + oListItem["Observacion"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return "";
            //return mensaje;
        }

        #region "wpListarOperacion"

        public DataSet ListarOperaciones(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@filtro", SqlDbType.NVarChar);
                SqlParametros[0].Value = buscar;
                SqlParametros[1] = new SqlParameter("@edoFiltro", SqlDbType.NVarChar);
                SqlParametros[1].Value = etapa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAGENERAL;
                SqlParametros[3] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[3].Value = area;
                SqlParametros[4] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargo;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[6].Value = pageS;
                SqlParametros[7] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[7].Value = pageN;

                SqlParametros[8] = new SqlParameter("@filtrosubestado", SqlDbType.NVarChar);
                SqlParametros[8].Value = subEtapa;

                SqlParametros[9] = new SqlParameter("@filtroetapa", SqlDbType.NVarChar);
                SqlParametros[9].Value = estado;

                dt = AD_Global.ejecutarConsulta("ListarOperaciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public string Buscar_Usuario(String Usuario)
        {
            string cargo = "";
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = currentSite.OpenWeb(SPContext.Current.Web.ID))
                    {
                        List<string> objColumns = new List<string>();
                        SPList list = web.Lists.TryGetList("Usuarios");
                        string sQuery = "<Where><Eq><FieldRef Name='Usuario'/><Value Type='Text'>" + Usuario + "</Value></Eq></Where>";
                        var oQuery = new SPQuery();
                        oQuery.Query = sQuery;
                        SPListItemCollection items = list.GetItems(oQuery);
                        SPListItemCollection collListItems = list.GetItems(oQuery);
                        foreach (SPListItem oListItem in collListItems)
                        {
                            cargo = oListItem["Cargo"] != null ? (oListItem["Cargo"].ToString()) : "";
                            break;
                        }
                    }
                }
            });
            return cargo;
        }


        #endregion

        #region "wpListarSeguimiento"

        public DataSet ListarSeguimiento1(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@filtro", SqlDbType.NVarChar);
                SqlParametros[0].Value = buscar;
                SqlParametros[1] = new SqlParameter("@edoFiltro", SqlDbType.NVarChar);
                SqlParametros[1].Value = etapa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAGENERAL;
                SqlParametros[3] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[3].Value = area;
                SqlParametros[4] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargo;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[6].Value = pageS;
                SqlParametros[7] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[7].Value = pageN;

                SqlParametros[8] = new SqlParameter("@filtrosubestado", SqlDbType.NVarChar);
                SqlParametros[8].Value = subEtapa;

                SqlParametros[9] = new SqlParameter("@filtroetapa", SqlDbType.NVarChar);
                SqlParametros[9].Value = estado;

                dt = AD_Global.ejecutarConsulta("ListarSeguimiento", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //log.Error("Error metod Buscar" + ex.ToString());
                return new DataSet();
            }
        }

        public void CargarLista(ref DropDownList lista, string Nombre_Lista)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                string Nombre_Vista = Constantes.LISTAESTADO.VISTA;
                SPList listData = app.Lists[Nombre_Lista];
                SPView view = listData.Views[Nombre_Vista];
                view.Query = "<OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                // view.Update();
                lista.DataSource = listData.GetItems(view).GetDataTable();
                lista.DataTextField = listData.GetItems(view).Fields["Nombre"].ToString();
                lista.DataValueField = listData.GetItems(view).Fields["ID"].ToString();
                lista.DataBind();
                lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione " + Nombre_Lista + "--", "-1"));
                lista.SelectedIndex = 0;

                if (Nombre_Lista == "Estados")
                {
                    ListItem itemToRemove = lista.Items.FindByValue("5");
                    if (itemToRemove != null)
                    {
                        lista.Items.Remove(itemToRemove);
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                //log.Error("ConexionDatos" + ex.ToString());
            }
            finally
            {
                lista = null;

            }
        }

        public void CargarListaSubEtapa(ref DropDownList lista, string Nombre_Lista, string etapa)
        {
            try
            {

                lista.Items.Clear();
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPList lis = app.Lists[Nombre_Lista];
                SPQuery view = new SPQuery();
                view.Query = "<Where><Eq><FieldRef Name='Etapa_x003a_ID'  /><Value Type='Lookup' >" + Convert.ToInt32(etapa) + "</Value></Eq></Where><OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
                SPListItemCollection collListItems = lis.GetItems(view);


                lista.DataSource = collListItems.GetDataTable();
                lista.DataTextField = "Title";
                lista.DataValueField = "ID";
                lista.DataBind();
                lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione " + Nombre_Lista + "--", "-1"));
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            finally
            {
                lista = null;

            }
        }

        #endregion


        #region "wpBusquedaPosicionCliente"

        public DataSet LitarDatosEmpresa(string Rut, string RazonSocial, string idEjecutivo, string Ejecutivo)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[0].Value = Rut;
                SqlParametros[1] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[1].Value = RazonSocial;
                SqlParametros[2] = new SqlParameter("@idEjecutivo", SqlDbType.NVarChar);
                SqlParametros[2].Value = idEjecutivo;
                SqlParametros[3] = new SqlParameter("@Ejecutivo", SqlDbType.NVarChar);
                SqlParametros[3].Value = Ejecutivo;
                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsulta("LitarDatosPosicionCliente", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion


        #region "wpPosicionCliente"

        public DataSet ConsultaPosicionCliente(string idEmpresa)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsulta("ConsultaPosicionCliente", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable ConsultaValidacionesEmpresa(string idEmpresa, int opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.Int);
                SqlParametros[1].Value = opcion;
                dt = AD_Global.ejecutarConsultas("ConsultaValidacionesEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpPosicionClienteV2"

        public DataSet ConsultaFichaComercial(string idEmpresa, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }


        #endregion


        #region "wpProrrateoGarantias"

        public DataSet ConsultaProrrateoGarantias(string idEmpresa, string usuario, string perfil, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;

                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }


        public DataSet ActualizarDatosProrrateo(string idEmpresa, string idOperacion, string user, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idOperacion);
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idEmpresa);
                SqlParametros[2] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[2].Value = 5;
                SqlParametros[3] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = user;
                SqlParametros[4] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = perfil;

                dt = AD_Global.ejecutarConsulta("ActualizarGarantiasProrrateo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean InsertarProrrateo(string idEmpresa, string xmlDataGarantias, string xmlDataOperacion, string usuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@accion", SqlDbType.NChar);
                SqlParametros[1].Value = 1;

                SqlParametros[2] = new SqlParameter("@xmlDataGarantias", SqlDbType.NVarChar);
                SqlParametros[2].Value = xmlDataGarantias;

                SqlParametros[3] = new SqlParameter("@xmlDataOperaciones", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlDataOperacion;

                SqlParametros[4] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[5].Value = perfil;

                return AD_Global.ejecutarAccion("GestionGarantiasProrrateo", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ReporteCartaGarantias(string idEmpresa, string idOperacion, string usuario, string perfil)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = int.Parse(idOperacion);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;

                dt = AD_Global.ejecutarConsultas("ReporteCartaGarantias", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        public Boolean GestionResponsableOperacion(int idOperacion, string usuario, string perfil, string opcion)
        {
            /*OPCIONES:
             * 01 =  Riesgo
             * 02 =  Fiscalia
             * 03 =  Operacion
             * 04 =  Contabilidad
            */
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                SqlParametros[1] = new SqlParameter("@usuario", SqlDbType.NChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NChar);
                SqlParametros[2].Value = perfil;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = opcion;
                return AD_Global.ejecutarAccion("GestionResponsablesOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarUsuarios(int? IdCargo, int? IdDepartamento, string NombreUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[3];
                sqlParam[0] = new SqlParameter("@IdCargo", IdCargo);
                sqlParam[1] = new SqlParameter("@IdDepartamento", IdDepartamento);
                sqlParam[2] = new SqlParameter("@NombreUsuario", NombreUsuario);
                return AD_Global.ejecutarConsultas("ListarUsuario", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }


        public DataTable ListarAdministracionFondosEjecutivo()
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[0];
                //sqlParam[0] = new SqlParameter("@IdCargo", IdCargo);
                //sqlParam[1] = new SqlParameter("@IdDepartamento", IdDepartamento);
                //sqlParam[2] = new SqlParameter("@NombreUsuario", NombreUsuario);
                return AD_Global.ejecutarConsultas("ListarAdministracionFondosEjecutivo", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        #region "AdministracionFondo"

        //public DataTable ListarEmpresas()
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParam;
        //        sqlParam = new SqlParameter[0];
        //        //sqlParam[0] = new SqlParameter("@IdCargo", IdCargo);
        //        return AD_Global.ejecutarConsultas("ListarEmpresas", sqlParam);
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }
        //}

        public DataTable ListarAdministracionFondos(string Empresa, string rut, string NroCertificado, string Ejecutivo)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[4];
                sqlParam[0] = new SqlParameter("@Empresa", Empresa);
                sqlParam[1] = new SqlParameter("@Rut", rut);
                sqlParam[2] = new SqlParameter("@NumCertificado", NroCertificado);
                sqlParam[3] = new SqlParameter("@Ejecutivo", Ejecutivo);

                return AD_Global.ejecutarConsultas("ListarAdministracionFondos", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public DataTable ListarDetalleAdministracionFondos(int IdEmpresa)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@IdEmpresa", IdEmpresa);
                //sqlParam[1] = new SqlParameter("@NumCertificado", NroCertificado);
                return AD_Global.ejecutarConsultas("ListarDetalleAdministracionFondos", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public Boolean InsertarModificaFondos(int? IdAdministracionFondos, string NroCertificado, int IdOperacion, int IdTipoMov, string DescTipoMov, int IdDestino, string DescDestino, int IdDetalle, string DescDetalle, DateTime? FechaMovimiento, double MontoMovimiento, string Usuario, int Opcion, string Comentario, bool? IncluirReporte)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[15];
                SqlParametros[0] = new SqlParameter("@IdAdministracionFondos", SqlDbType.Int);
                SqlParametros[0].Value = IdAdministracionFondos;
                SqlParametros[1] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[1].Value = NroCertificado;
                SqlParametros[2] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[2].Value = IdOperacion;
                SqlParametros[3] = new SqlParameter("@IdTipoMov", SqlDbType.Int);
                SqlParametros[3].Value = IdTipoMov;
                SqlParametros[4] = new SqlParameter("@DescTipoMov", SqlDbType.NVarChar);
                SqlParametros[4].Value = DescTipoMov;
                SqlParametros[5] = new SqlParameter("@IdDestino", SqlDbType.Int);
                SqlParametros[5].Value = IdDestino;
                SqlParametros[6] = new SqlParameter("@DescDestino", SqlDbType.NVarChar);
                SqlParametros[6].Value = DescDestino;
                SqlParametros[7] = new SqlParameter("@IdDetalle", SqlDbType.Int);
                SqlParametros[7].Value = IdDetalle;
                SqlParametros[8] = new SqlParameter("@DescDetalle", SqlDbType.NVarChar);
                SqlParametros[8].Value = DescDetalle;
                SqlParametros[9] = new SqlParameter("@FechaMovimiento", SqlDbType.DateTime);
                SqlParametros[9].Value = FechaMovimiento;
                SqlParametros[10] = new SqlParameter("@MontoMovimiento", SqlDbType.Float);
                SqlParametros[10].Value = MontoMovimiento;
                SqlParametros[11] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[11].Value = Usuario;
                SqlParametros[12] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[12].Value = Opcion;
                SqlParametros[13] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[13].Value = Comentario;
                SqlParametros[14] = new SqlParameter("@IncluirReporte", SqlDbType.Bit);
                SqlParametros[14].Value = IncluirReporte;

                return AD_Global.ejecutarAccion("InsertarModificaFondos", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertaMensajeDistribucion(int IdOperacion, string Ncertificado, string Comentario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;
                SqlParametros[1] = new SqlParameter("@Ncertificado", SqlDbType.NVarChar);
                SqlParametros[1].Value = Ncertificado;
                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[2].Value = Comentario;

                return AD_Global.ejecutarAccion("InsertaMensajeDistribucion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }

        }

        public DataTable ListarDetalleMovimiento(string Ncertificado)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@Ncertificado", Ncertificado);
                return AD_Global.ejecutarConsultas("ListarDetalleMovimiento", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public DataTable ListarConceptoPago()
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[0];
                return AD_Global.ejecutarConsultas("ListarConceptoPago", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public DataTable ListarDetalleConceptoPago(int IdConcepto)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@IdConcepto", IdConcepto);
                return AD_Global.ejecutarConsultas("ListarDetalleConceptoPago", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public string ValidarEjecutivoEmision(int IdEmpresa)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@IdEmpresa", IdEmpresa);
                return AD_Global.traerPrimeraColumna("ValidarEjecutivoEmision", sqlParam);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return string.Empty;
            }

        }

        public DataTable CRUDAcreedores(int Opcion, string RutAcreedor, int IdAcreedor, string NombreAcreedor, string DomicilioAcreedor, int IdTipoAcreedor, int IdRegion, int IdProvincia, int IdComuna, string IdUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[10];
                sqlParam[0] = new SqlParameter("@Opcion", Opcion);
                sqlParam[1] = new SqlParameter("@RutAcreedor", RutAcreedor.Trim());
                sqlParam[2] = new SqlParameter("@IdAcreedor", IdAcreedor);
                sqlParam[3] = new SqlParameter("@NombreAcreedor", NombreAcreedor);
                sqlParam[4] = new SqlParameter("@DomicilioAcreedor", DomicilioAcreedor);
                sqlParam[5] = new SqlParameter("@IdTipoAcreedor", IdTipoAcreedor);
                sqlParam[6] = new SqlParameter("@IdRegion", IdRegion);
                sqlParam[7] = new SqlParameter("@IdProvincia", IdProvincia);
                sqlParam[8] = new SqlParameter("@IdComuna", IdComuna);
                sqlParam[9] = new SqlParameter("@IdUsuario", IdUsuario);
                return AD_Global.ejecutarConsultas("CRUDAcreedores", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        public Boolean Acreedores(int Opcion, string RutAcreedor, int IdAcreedor, string NombreAcreedor, string DomicilioAcreedor, int IdTipoAcreedor, int IdRegion, int IdProvincia, int IdComuna, string IdUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[10];
                sqlParam[0] = new SqlParameter("@Opcion", Opcion);
                sqlParam[1] = new SqlParameter("@RutAcreedor", RutAcreedor.Trim());
                sqlParam[2] = new SqlParameter("@IdAcreedor", IdAcreedor);
                sqlParam[3] = new SqlParameter("@NombreAcreedor", NombreAcreedor);
                sqlParam[4] = new SqlParameter("@DomicilioAcreedor", DomicilioAcreedor);
                sqlParam[5] = new SqlParameter("@IdTipoAcreedor", IdTipoAcreedor);
                sqlParam[6] = new SqlParameter("@IdRegion", IdRegion);
                sqlParam[7] = new SqlParameter("@IdProvincia", IdProvincia);
                sqlParam[8] = new SqlParameter("@IdComuna", IdComuna);
                sqlParam[9] = new SqlParameter("@IdUsuario", IdUsuario);

                return AD_Global.ejecutarAccionBool("CRUDAcreedores", sqlParam);
            }
            catch
            {
                return false;
            }
        }


        #endregion

        #region "limites corfo"
        public DataSet LimitesCorfo(int IdOpcion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[0].Value = IdOpcion;

                dt = AD_Global.ejecutarConsulta("LimitesCorfo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable GestionValorMoneda(DateTime Fecha, int IdValorMoneda, double? MontoUF, double? @MontoUSD, double? @MontoEuro, int IdOpcion)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[6];
                sqlParam[0] = new SqlParameter("@Fecha", Fecha);
                sqlParam[1] = new SqlParameter("@IdValorMoneda", IdValorMoneda);
                sqlParam[2] = new SqlParameter("@MontoUF", MontoUF);
                sqlParam[3] = new SqlParameter("@MontoUSD", MontoUSD);
                sqlParam[4] = new SqlParameter("@MontoEuro", MontoEuro);
                sqlParam[5] = new SqlParameter("@IdOpcion", IdOpcion);
                return AD_Global.ejecutarConsultas("GestionValorMoneda", sqlParam);
            }
            catch
            {
                return new DataTable();
            }
        }

        #endregion
    }
}
