using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using System.Collections;
using Microsoft.SharePoint.Utilities;
using MultigestionUtilidades;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using DevExpress.Web;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using ClasesNegocio;

namespace MultiComercial
{
    class LogicaNegocio
    {
        public Boolean GestionBitacoraCobranza(int idEmpresa, int idOperacion, string NroCertificado, string Comentario, int IdAccionCobranza, DateTime FechaGestion, string MoraProyectada, bool Reprogramacion, string Usuario, int opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[2].Value = NroCertificado;
                SqlParametros[3] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Comentario;
                SqlParametros[4] = new SqlParameter("@IdAccionCobranza", SqlDbType.Int);
                SqlParametros[4].Value = IdAccionCobranza;
                SqlParametros[5] = new SqlParameter("@FechaGestion", SqlDbType.DateTime);
                SqlParametros[5].Value = FechaGestion;
                SqlParametros[6] = new SqlParameter("@MoraProyectada", SqlDbType.NVarChar);
                SqlParametros[6].Value = MoraProyectada;
                SqlParametros[7] = new SqlParameter("@Reprogramacion", SqlDbType.Bit);
                SqlParametros[7].Value = Reprogramacion;
                SqlParametros[8] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[8].Value = Usuario;
                SqlParametros[9] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[9].Value = opcion;
                return AD_Global.ejecutarAccion("GestionBitacoraCobranza", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }
        public DataTable ListarGestionBitacoraCobranza(int idEmpresa, int idOperacion, string NroCertificado, string Comentario, DateTime? FechaGestion, string MoraProyectada, bool? Reprogramacion, string Usuario, int opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[9];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[2].Value = NroCertificado;
                SqlParametros[3] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Comentario;
                SqlParametros[4] = new SqlParameter("@FechaGestion", SqlDbType.DateTime);
                SqlParametros[4].Value = FechaGestion;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = Usuario;
                SqlParametros[6] = new SqlParameter("@MoraProyectada", SqlDbType.NVarChar);
                SqlParametros[6].Value = MoraProyectada;
                SqlParametros[7] = new SqlParameter("@Reprogramacion", SqlDbType.Bit);
                SqlParametros[7].Value = Reprogramacion;
                SqlParametros[8] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[8].Value = opcion;
                return AD_Global.ejecutarConsultas("GestionBitacoraCobranza", SqlParametros);
            }
            catch
            {
                return new DataTable();
            }
        }
        public DataTable ListarSms(string Ncertificado)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Ncertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = Ncertificado;

                return AD_Global.ejecutarConsultas("ListarSms", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
        }
        public DataTable ListarCarteraEjecutivoEstado(string mora, string ejecutivo, string acreedor, string empresa, string estadoCF, int IdOpcion)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@categoria", SqlDbType.NVarChar);
                SqlParametros[0].Value = mora;
                SqlParametros[1] = new SqlParameter("@ejecutivo", SqlDbType.NVarChar);
                SqlParametros[1].Value = ejecutivo;
                SqlParametros[2] = new SqlParameter("@acreedor", SqlDbType.NVarChar);
                SqlParametros[2].Value = acreedor;
                SqlParametros[3] = new SqlParameter("@empresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = empresa;
                SqlParametros[4] = new SqlParameter("@estadoCF", SqlDbType.NVarChar);
                SqlParametros[4].Value = estadoCF;
                SqlParametros[5] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[5].Value = IdOpcion;

                return AD_Global.ejecutarConsultas("ListarCarteraEjecutivoEstado", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
        }

        public DataSet ListarCarteraEjecutivoEstadods(string mora, string ejecutivo, string acreedor, string empresa, string estadoCF, int IdOpcion)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@categoria", SqlDbType.NVarChar);
                SqlParametros[0].Value = mora;
                SqlParametros[1] = new SqlParameter("@ejecutivo", SqlDbType.NVarChar);
                SqlParametros[1].Value = ejecutivo;
                SqlParametros[2] = new SqlParameter("@acreedor", SqlDbType.NVarChar);
                SqlParametros[2].Value = acreedor;
                SqlParametros[3] = new SqlParameter("@empresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = empresa;
                SqlParametros[4] = new SqlParameter("@estadoCF", SqlDbType.NVarChar);
                SqlParametros[4].Value = estadoCF;
                SqlParametros[5] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[5].Value = IdOpcion;

                return AD_Global.ejecutarConsulta("ListarCarteraEjecutivoEstado", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return ds;
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

        public string ValidarDatosEmpresa(Empresa emp, string rutContacto, string dv, string nombreContacto, string telefonoFijo, string mail, string descCargo, string telefonoCelular)
        {
            return ValidarDatosCreacionEmpresa(emp, rutContacto.Trim(), dv.Trim(), nombreContacto.Trim(), telefonoFijo.Trim(), mail.Trim(), descCargo.Trim(), telefonoCelular.Trim());
        }

        public string ValidarDatosCreacionEmpresa(Empresa emp, string rutContacto, string dv, string nombreContacto, string telefonoFijo, string mail, string descCargo, string telefonoCelular)
        {
            string mensaje = string.Empty;

            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[12];
                sqlParam[0] = new SqlParameter("@rut", emp._rut);
                sqlParam[1] = new SqlParameter("@dvRut", emp._dvRut);
                sqlParam[2] = new SqlParameter("@razonSocial", emp._razonSocial);
                sqlParam[3] = new SqlParameter("@descEjecutivo", emp._descEjecutivo);
                sqlParam[4] = new SqlParameter("@fecIniAct", emp._fecIniAct);
                sqlParam[5] = new SqlParameter("@rutContacto", rutContacto);
                sqlParam[6] = new SqlParameter("@dvContacto", dv);
                sqlParam[7] = new SqlParameter("@nombreContacto", nombreContacto);
                sqlParam[8] = new SqlParameter("@telefonoFijo", telefonoFijo);
                sqlParam[9] = new SqlParameter("@mail", mail);
                sqlParam[10] = new SqlParameter("@descCargo", descCargo);
                sqlParam[11] = new SqlParameter("@telefonoCelular", telefonoCelular);

                return AD_Global.traerPrimeraColumna("sp_alertaEmpresa", sqlParam);
            }
            catch
            {
                return mensaje = "";
            }
        }

        public DataTable ListarClientesxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                //AD_Global objDatos = new AD_Global();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionEmpresaClientes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return new DataTable();
            }
        }

        public bool EliminarClientexEmpresa(int idEmpresaClientes, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresaClientes", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaClientes;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaClientes", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool InsertarClientexEmpresa(int idEmpresa, string razonSocial, string rut, string divRut, string porcVentas, string plazoConvenioPago, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[2] = new SqlParameter("@Rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[3].Value = divRut;
                SqlParametros[4] = new SqlParameter("@razonSocial", SqlDbType.NChar);
                SqlParametros[4].Value = razonSocial;
                SqlParametros[5] = new SqlParameter("@porcVentas", SqlDbType.NChar);
                SqlParametros[5].Value = porcVentas;
                SqlParametros[6] = new SqlParameter("@plazoConvenio", SqlDbType.NChar);
                SqlParametros[6].Value = plazoConvenioPago;
                SqlParametros[7] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[7].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaClientes", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool ModificarClientexEmpresa(int idEmpresaClientes, string razonSocial, string rut, string divRut, string porcVentas, string plazoConvenioPago, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresaClientes", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaClientes;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[2] = new SqlParameter("@Rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[3].Value = divRut;
                SqlParametros[4] = new SqlParameter("@razonSocial", SqlDbType.NChar);
                SqlParametros[4].Value = razonSocial;
                SqlParametros[5] = new SqlParameter("@porcVentas", SqlDbType.NChar);
                SqlParametros[5].Value = porcVentas;
                SqlParametros[6] = new SqlParameter("@plazoConvenio", SqlDbType.NChar);
                SqlParametros[6].Value = plazoConvenioPago;
                SqlParametros[7] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[7].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaClientes", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable BuscarClientesxRut(int idEmpresa, string rut)
        {
            try
            {
                DataTable dt = new DataTable();
                //AD_Global objDatos = new AD_Global();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.BUSCAxRUT;
                SqlParametros[2] = new SqlParameter("@rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                dt = AD_Global.ejecutarConsultas("GestionEmpresaClientes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return new DataTable();
            }
        }

        public DataTable buscarDatosRut(string rut)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@rut", SqlDbType.NVarChar);
                SqlParametros[0].Value = rut;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("BuscarDatosRutEmpresaPersona", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return new DataTable();
            }
        }

        public DataTable ObtenerDetalleEmpresa(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionDetalleEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertarDetalleEmpresa(string idEmpresa, string anos, string historia, string idUsuario, string cargo, string AntecedentesAdicionales)
        {
            try
            {
                Boolean ress;
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@historia", SqlDbType.NVarChar);
                SqlParametros[1].Value = historia;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@Cargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargo;

                if (anos == "")
                {
                    SqlParametros[5] = new SqlParameter("@anos", SqlDbType.Int);
                    SqlParametros[5].Value = null;
                }
                else
                {
                    SqlParametros[5] = new SqlParameter("@anos", SqlDbType.Int);
                    SqlParametros[5].Value = int.Parse(anos);
                }

                SqlParametros[6] = new SqlParameter("@AntecedentesAdicionales", SqlDbType.NVarChar);
                SqlParametros[6].Value = AntecedentesAdicionales;

                ress = AD_Global.ejecutarAccion("GestionDetalleEmpresa", SqlParametros);
                return ress;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return false;
            }
        }

        public Boolean ModificarDetalleEmpresa(string idEmpresa, string idDetalleEmpresa, string anos, string historia, string idUsuario, string cargo, string AntecedentesAdicionales)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDetEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idDetalleEmpresa);

                SqlParametros[2] = new SqlParameter("@historia", SqlDbType.NVarChar);
                SqlParametros[2].Value = historia;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = idUsuario;
                SqlParametros[5] = new SqlParameter("@Cargo", SqlDbType.NVarChar);
                SqlParametros[5].Value = cargo;

                if (anos == "")
                {
                    SqlParametros[6] = new SqlParameter("@anos", SqlDbType.Int);
                    SqlParametros[6].Value = null;
                }
                else
                {
                    SqlParametros[6] = new SqlParameter("@anos", SqlDbType.Int);
                    SqlParametros[6].Value = int.Parse(anos);
                }

                SqlParametros[7] = new SqlParameter("@AntecedentesAdicionales", SqlDbType.NVarChar);
                SqlParametros[7].Value = AntecedentesAdicionales;

                return AD_Global.ejecutarAccion("GestionDetalleEmpresa", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool ModificarDeudaSBIFxEmpresa(int idEmpresaDeudaSBIF, string idTipoDeuda, string descTipoDeuda, string instFinanciera, string vigente, string mora3090, string mora90Mas, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[9];
                SqlParametros[0] = new SqlParameter("@idEmpresaDeudaSBIF", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaDeudaSBIF;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[2] = new SqlParameter("@idTipoDeuda", SqlDbType.NChar);
                SqlParametros[2].Value = idTipoDeuda;
                SqlParametros[3] = new SqlParameter("@descTipoDeuda", SqlDbType.NChar);
                SqlParametros[3].Value = descTipoDeuda;
                SqlParametros[4] = new SqlParameter("@instFinanciera", SqlDbType.NChar);
                SqlParametros[4].Value = instFinanciera;
                SqlParametros[5] = new SqlParameter("@vigente", SqlDbType.NChar);
                SqlParametros[5].Value = vigente;
                SqlParametros[6] = new SqlParameter("@mora3090", SqlDbType.NChar);
                SqlParametros[6].Value = mora3090;
                SqlParametros[7] = new SqlParameter("@mora90Mas", SqlDbType.NChar);
                SqlParametros[7].Value = mora90Mas;
                SqlParametros[8] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[8].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaDeudaSBIF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool ModificarCreditoDispxEmpresa(int idEmpresaCreditoDisp, string instFinanciera, string montoDisponibleDirecta, string montoDisponibleIndirecta, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresaCreditoDisponible", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaCreditoDisp;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[2] = new SqlParameter("@instFinanciera", SqlDbType.NChar);
                SqlParametros[2].Value = instFinanciera;
                SqlParametros[3] = new SqlParameter("@montoDispDirecta", SqlDbType.NChar);
                SqlParametros[3].Value = montoDisponibleDirecta;
                SqlParametros[4] = new SqlParameter("@montoDispIndirecta", SqlDbType.NChar);
                SqlParametros[4].Value = montoDisponibleIndirecta;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaCreditoDisponible", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarDeudaSBIFxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionEmpresaDeudaSBIF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCreditoDispxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionEmpresaCreditoDisponible", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public bool InsertarDeudaSBIFxEmpresa(int idEmpresa, string idTipoDeuda, string descTipoDeuda, string instFinanciera, string vigente, string mora3090, string mora90Mas, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[9];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[2] = new SqlParameter("@idTipoDeuda", SqlDbType.NChar);
                SqlParametros[2].Value = idTipoDeuda;
                SqlParametros[3] = new SqlParameter("@descTipoDeuda", SqlDbType.NChar);
                SqlParametros[3].Value = descTipoDeuda;
                SqlParametros[4] = new SqlParameter("@instFinanciera", SqlDbType.NChar);
                SqlParametros[4].Value = instFinanciera;
                SqlParametros[5] = new SqlParameter("@vigente", SqlDbType.NChar);
                SqlParametros[5].Value = vigente;
                SqlParametros[6] = new SqlParameter("@mora3090", SqlDbType.NChar);
                SqlParametros[6].Value = mora3090;
                SqlParametros[7] = new SqlParameter("@mora90Mas", SqlDbType.NChar);
                SqlParametros[7].Value = mora90Mas;
                SqlParametros[8] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[8].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaDeudaSBIF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool InsertarCreditoDispxEmpresa(int idEmpresa, string instFinanciera, string montoDisponibleDirecta, string montoDisponibleIndirecta, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[2] = new SqlParameter("@instFinanciera", SqlDbType.NChar);
                SqlParametros[2].Value = instFinanciera;
                SqlParametros[3] = new SqlParameter("@montoDispDirecta", SqlDbType.NChar);
                SqlParametros[3].Value = montoDisponibleDirecta;
                SqlParametros[4] = new SqlParameter("@montoDispIndirecta", SqlDbType.NChar);
                SqlParametros[4].Value = montoDisponibleIndirecta;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaCreditoDisponible", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool EliminarDeudaSBIFxEmpresa(int idEmpresaDeudaSBIF, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresaDeudaSBIF", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaDeudaSBIF;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaDeudaSBIF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool EliminarCreditoDispxEmpresa(int idEmpresaCreditoDisp, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresaCreditoDisponible", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaCreditoDisp;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaCreditoDisponible", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #region "Rechazo operacion"

        public Boolean DevolverEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
        string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
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
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
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

                dt = AD_Global.ejecutarAccion("DevolverEstados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return false;
            }
        }


        public Dictionary<string, string> BuscarEstados(string ID)
        {
            Utilidades util = new Utilidades();
            SPWeb app = SPContext.Current.Web;
            SPList Lista = app.Lists["Operaciones"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><Eq><FieldRef Name='IdSQL'/><Value Type='Number'>" + ID.Trim() + "</Value></Eq></Where>";
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            Dictionary<string, string> valores = new Dictionary<string, string>();
            foreach (SPListItem oListItem in ColecLista)
            {
                valores.Add("Estado", ((oListItem["IdEstado"] != null) ? util.ObtenerValor(oListItem["IdEstado"].ToString()) : ""));
                valores.Add("Etapa", ((oListItem["IdEtapa"] != null) ? util.ObtenerValor(oListItem["IdEtapa"].ToString()) : ""));
                valores.Add("SubEtapa", ((oListItem["IdSubEtapa"] != null) ? util.ObtenerValor(oListItem["IdSubEtapa"].ToString()) : ""));
                valores.Add("ID", ((oListItem["ID"] != null) ? (oListItem["ID"].ToString()) : ""));
                return valores;
            }
            return valores;
        }

        public Dictionary<string, string> BuscarEstadoActual(string estado, string etapa, string subetapa)
        {
            Utilidades util = new Utilidades();
            SPWeb app = SPContext.Current.Web;
            String LoginU = app.CurrentUser.Name.ToString();

            SPList Lista = app.Lists["CambiosEstados"];

            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where><And> <And><Eq><FieldRef Name='Estado_x003a_ID' /><Value Type='Text'>" + estado + "</Value></Eq> <Eq><FieldRef Name='Etapa_x003a_ID'/><Value Type='Text'>" + etapa + "</Value></Eq></And> <Eq><FieldRef Name='SubEtapa_x003a_ID' /><Value Type='Text'>" + subetapa + "</Value></Eq></And></Where>";
   
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);

            Dictionary<string, string> valores = new Dictionary<string, string>();
            foreach (SPListItem oListItem in ColecLista)
            {
                valores.Add("CambioID", ((oListItem["Orden"] != null) ? util.ObtenerValor(oListItem["Orden"].ToString()) : ""));
                valores.Add("Area", ((oListItem["Area"] != null) ? util.ObtenerValor(oListItem["Area"].ToString()) : ""));
                valores.Add("Workflow", ((oListItem["WorkflowExpress"] != null) ? (oListItem["WorkflowExpress"].ToString()) : ""));
                valores.Add("NuevaIdEtapa", ((oListItem["OrdenId"] != null) ? (oListItem["OrdenId"].ToString()) : ""));
                return valores;
            }
            return valores;
        }


        public int ExisteOperacionLista(int IdOperacion)
        {
            try
            {
                SPWeb mySite = SPContext.Current.Web;
                SPList list = mySite.Lists["Operaciones"];

                SPListItemCollection items = list.GetItems(new SPQuery()
                {
                    Query = @"<Where><Eq><FieldRef Name='IdSQL' /><Value Type='Number'>" + IdOperacion + "</Value></Eq></Where>"
                });

                if (items.Count > 0)
                    return 1;
                else
                    return 0;
                //foreach (SPListItem item in items)
                //{
                //    //item["IdSubEtapa"] = subEtapa;
                //    //item.Update();
                //}
                ////list.Update();
                //return 1;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return 0;
            }
        }

        public int ActualizarListaOperacion(Dictionary<string, object> valores)
        {
            try
            {
                SPWeb mySite = SPContext.Current.Web;
                SPList list = mySite.Lists["Operaciones"];

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(mySite.Site.ID))
                    {
                        //  implementation details omitted
                        SPListItemCollection items = list.GetItems(new SPQuery()
                        {
                            Query = @"<Where><Eq><FieldRef Name='IdSQL' /><Value Type='Number'>" + valores["IdSQL"].ToString() + "</Value></Eq></Where>"
                        });

                        foreach (SPListItem item in items)
                        {
                            item["IdEmpresa"] = valores["IdEmpresa"].ToString();
                            item["IdProducto"] = valores["IdProducto"].ToString();
                            item["IdFinalidad"] = valores["IdFinalidad"].ToString();
                            item["Plazo"] = valores["Plazo"].ToString();
                            item["MontoOperacion"] = valores["MontoOperacion"].ToString();
                            item["IdEtapa"] = valores["IdEtapa"].ToString();
                            item["IdSubEtapa"] = valores["IdSubEtapa"].ToString();
                            item["IdEstado"] = valores["IdEstado"].ToString();
                            item.Update();
                        }
                        list.Update();
                    }
                });

                return 1;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return 0;
            }
        }

        public int InsertarListaOperacion(Dictionary<string, object> valores)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                SPListItemCollection items = app.Lists["Operaciones"].Items;
                SPListItem newItem = items.Add();

                newItem["IdEmpresa"] = valores["IdEmpresa"].ToString();
                newItem["IdProducto"] = valores["IdProducto"].ToString();
                newItem["IdFinalidad"] = valores["IdFinalidad"].ToString();
                newItem["Plazo"] = valores["Plazo"].ToString();
                newItem["MontoOperacion"] = valores["MontoOperacion"].ToString();
                newItem["IdEtapa"] = valores["IdEtapa"].ToString();
                newItem["IdEstado"] = valores["IdEstado"].ToString();
                newItem["IdSubEtapa"] = valores["IdSubEtapa"].ToString();
                newItem["IdSQL"] = valores["IdSQL"].ToString();

                newItem.Update();

                return 1;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return 0;
            }
        }

        public int ActualizarEstado(String ID, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area)
        {
            try
            {
                SPWeb mySite = SPContext.Current.Web;
                SPList list = mySite.Lists["Operaciones"];

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(mySite.Site.ID))
                    {
                        //  implementation details omitted
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
                    }
                });

                return 1;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //logger.Debug("Error en " + ex.Source.ToString() + " : " + ex.Message.ToString());
                return 0;
            }
        }

        #endregion

        #region "wpDirectorio"

        public Boolean InsertarDirectorioxEmpresa(string idEmpresa, string nombre, string cargo, string profesion, string universidad, string rut, string dvrut, string antiguedad, string idUsuario, string cargoUser)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@nombre", SqlDbType.NVarChar);
                SqlParametros[1].Value = nombre;
                SqlParametros[2] = new SqlParameter("@cargo", SqlDbType.NVarChar);
                SqlParametros[2].Value = cargo;
                SqlParametros[3] = new SqlParameter("@profesion", SqlDbType.NVarChar);
                SqlParametros[3].Value = profesion;
                SqlParametros[4] = new SqlParameter("@rut", SqlDbType.NVarChar);
                SqlParametros[4].Value = rut.Replace(".", "");
                SqlParametros[5] = new SqlParameter("@antiguedad", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(antiguedad);
                SqlParametros[6] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[6].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[7] = new SqlParameter("@universidad ", SqlDbType.NChar);
                SqlParametros[7].Value = universidad;
                SqlParametros[8] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[8].Value = idUsuario;
                SqlParametros[9] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[9].Value = cargoUser;
                SqlParametros[10] = new SqlParameter("@divRut", SqlDbType.NVarChar);
                SqlParametros[10].Value = dvrut;
                dt = AD_Global.ejecutarAccion("GestionDirectorio", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarDirectorioxEmpresa(string idEmpresa, string idDirectorio, string nombre, string cargo, string profesion, string universidad, string rut, string dvrut, string antiguedad, string idUsuario, string cargoUser)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[12];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDirectorio", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idDirectorio);
                SqlParametros[2] = new SqlParameter("@nombre", SqlDbType.NVarChar);
                SqlParametros[2].Value = nombre;
                SqlParametros[3] = new SqlParameter("@cargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargo;
                SqlParametros[4] = new SqlParameter("@profesion", SqlDbType.NVarChar);
                SqlParametros[4].Value = profesion;
                SqlParametros[5] = new SqlParameter("@rut", SqlDbType.NVarChar);
                SqlParametros[5].Value = rut.Replace(".", "");
                SqlParametros[6] = new SqlParameter("@antiguedad", SqlDbType.NVarChar);
                SqlParametros[6].Value = int.Parse(antiguedad);
                SqlParametros[7] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[7].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[8] = new SqlParameter("@universidad ", SqlDbType.NChar);
                SqlParametros[8].Value = universidad;
                SqlParametros[9] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[9].Value = idUsuario;
                SqlParametros[10] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[10].Value = cargoUser;
                SqlParametros[11] = new SqlParameter("@divRut", SqlDbType.NVarChar);
                SqlParametros[11].Value = dvrut;
                dt = AD_Global.ejecutarAccion("GestionDirectorio", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarDirectorioxEmpresa(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionDirectorio", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean EliminarDirectorioxEmpresa(string idEmpresa, string idDirectorio, string idUsuario, string cargoUser)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idDirectorio", SqlDbType.NVarChar);
                SqlParametros[1].Value = idDirectorio;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;
                dt = AD_Global.ejecutarAccion("GestionDirectorio", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpDocumentosEmpresa"

        public DataTable ListarOperacionesxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[0].Value = "01";
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.NChar);
                SqlParametros[1].Value = idEmpresa;
                dt = AD_Global.ejecutarConsultas("GestionDocumentosEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpEmpresaRelacionada"

        public Boolean ModificarRelacionadasxEmpresa(string idEmpresa, string idRelacionada, string nombre, string tipoRelacion, string rut, string patrimonio, string idUsuario, string cargoUser, string divRut
           , string nroDocProtestos, string montoProtestos, string nroDocMorosidades, string montoMorosidades, string montoInfraccionesLab, string montoInfraccionesPrev)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[16];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idRelacionada", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idRelacionada);
                SqlParametros[2] = new SqlParameter("@nombre", SqlDbType.NVarChar);
                SqlParametros[2].Value = nombre;
                SqlParametros[3] = new SqlParameter("@tipoRelacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = tipoRelacion;
                SqlParametros[4] = new SqlParameter("@rut", SqlDbType.Int);
                SqlParametros[4].Value = int.Parse(rut);
                SqlParametros[5] = new SqlParameter("@divRut", SqlDbType.NChar);
                SqlParametros[5].Value = int.Parse(divRut);

                SqlParametros[6] = new SqlParameter("@patrimonio", SqlDbType.Float);
                SqlParametros[6].Value = float.Parse(patrimonio.Replace(".", "").Replace(",", "."));
                SqlParametros[7] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[7].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[8] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[8].Value = idUsuario;
                SqlParametros[9] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[9].Value = cargoUser;
                SqlParametros[10] = new SqlParameter("@MontoProtestos", SqlDbType.NVarChar);
                SqlParametros[10].Value = montoProtestos;
                SqlParametros[11] = new SqlParameter("@NroDocMorosidades", SqlDbType.NVarChar);
                SqlParametros[11].Value = nroDocMorosidades;
                SqlParametros[12] = new SqlParameter("@MontoMorosidades", SqlDbType.NVarChar);
                SqlParametros[12].Value = montoMorosidades;
                SqlParametros[13] = new SqlParameter("@MontoInfraccionesLaborales", SqlDbType.NVarChar);
                SqlParametros[13].Value = montoInfraccionesLab;
                SqlParametros[14] = new SqlParameter("@MontoInfraccionesPrevisionales", SqlDbType.NVarChar);
                SqlParametros[14].Value = montoInfraccionesPrev;
                SqlParametros[15] = new SqlParameter("@NroDocProtestos", SqlDbType.NVarChar);
                SqlParametros[15].Value = nroDocProtestos;
                return AD_Global.ejecutarAccion("GestionRelacionada", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarRelacionadasxEmpresa(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionRelacionada", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertarRelacionadasxEmpresa(string idEmpresa, string nombre, string tipoRelacion, string rut, string patrimonio, string idUsuario, string cargoUser, string divRut, string nroDocProtestos, string montoProtestos, string nroDocMorosidades, string montoMorosidades, string montoInfraccionesLab, string montoInfraccionesPrev)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[15];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@nombre", SqlDbType.NVarChar);
                SqlParametros[1].Value = nombre;
                SqlParametros[2] = new SqlParameter("@tipoRelacion", SqlDbType.NVarChar);
                SqlParametros[2].Value = tipoRelacion;
                SqlParametros[3] = new SqlParameter("@rut", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(rut);
                SqlParametros[4] = new SqlParameter("@divRut", SqlDbType.NChar);
                SqlParametros[4].Value = divRut;
                SqlParametros[5] = new SqlParameter("@patrimonio", SqlDbType.Float);
                SqlParametros[5].Value = patrimonio == "" ? 0 : float.Parse(patrimonio.Replace(".", "").Replace(",", "."));
                SqlParametros[6] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[6].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[7] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[7].Value = idUsuario;
                SqlParametros[8] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[8].Value = cargoUser;
                SqlParametros[9] = new SqlParameter("@NroDocProtestos", SqlDbType.NVarChar);
                SqlParametros[9].Value = nroDocProtestos;
                SqlParametros[10] = new SqlParameter("@MontoProtestos", SqlDbType.Float);
                SqlParametros[10].Value = montoProtestos == "" ? 0 : float.Parse(montoProtestos.Replace(".", "").Replace(",", "."));
                SqlParametros[11] = new SqlParameter("@NroDocMorosidades", SqlDbType.NVarChar);
                SqlParametros[11].Value = nroDocMorosidades;
                SqlParametros[12] = new SqlParameter("@MontoMorosidades", SqlDbType.Float);
                SqlParametros[12].Value = montoMorosidades == "" ? 0 : float.Parse(montoMorosidades.Replace(".", "").Replace(",", "."));
                SqlParametros[13] = new SqlParameter("@MontoInfraccionesLaborales", SqlDbType.Float);
                SqlParametros[13].Value = montoInfraccionesLab == "" ? 0 : float.Parse(montoInfraccionesLab.Replace(".", "").Replace(",", "."));
                SqlParametros[14] = new SqlParameter("@MontoInfraccionesPrevisionales", SqlDbType.Float);
                SqlParametros[14].Value = montoInfraccionesPrev == "" ? 0 : float.Parse(montoInfraccionesPrev.Replace(".", "").Replace(",", "."));
                return AD_Global.ejecutarAccion("GestionRelacionada", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean EliminarRelacionadasxEmpresa(string idEmpresa, string idRelacionada, string idUsuario, string cargoUser)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idRelacionada", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idRelacionada);
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;
                return AD_Global.ejecutarAccion("GestionRelacionada", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpEnviarComercial"

        public DataTable ValidarCambio(string idEmpresa, string idOperacion, string usuario, string cargo, string idEtapa, string idSubEtapa, string idEstado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@Usuario", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@Cargo", SqlDbType.NChar);
                SqlParametros[3].Value = cargo;
                SqlParametros[4] = new SqlParameter("@idEtapa", SqlDbType.Int);
                SqlParametros[4].Value = int.Parse(idEtapa);
                SqlParametros[5] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(idSubEtapa);
                SqlParametros[6] = new SqlParameter("@idEstado", SqlDbType.Int);
                SqlParametros[6].Value = int.Parse(idEstado);

                dt = AD_Global.ejecutarConsultas("ValidarCambio", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ListarResumen(string idEmpresa, string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsulta("ListarResumenComercial", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
        {
            try
            {
                Boolean dt = false;
                SqlParameter[] SqlParametros = new SqlParameter[13];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
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

        public Dictionary<string, string> BuscarNuevoEstado(string ID, bool Esworkflow)
        {
            Utilidades util = new Utilidades();
            SPWeb app = SPContext.Current.Web;
            String LoginU = app.CurrentUser.Name.ToString();
            SPList Lista = app.Lists["CambiosEstados"];

            SPQuery oQuery = new SPQuery();
            if(Esworkflow)
                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + (int.Parse(ID)).ToString() + "</Value></Eq></Where>";
            else
                oQuery.Query = "<Where><Eq><FieldRef Name='Orden'/><Value Type='TEXT'>" + (int.Parse(ID) + 1).ToString() + "</Value></Eq></Where>";

            SPListItemCollection ColecLista = Lista.GetItems(oQuery);

            Dictionary<string, string> valores = new Dictionary<string, string>();
            foreach (SPListItem oListItem in ColecLista)
            {
                valores.Add("CambioID", ((oListItem["ID"] != null) ? (oListItem["ID"].ToString()) : ""));
                valores.Add("IdEtapa", ((oListItem["Etapa:ID"] != null) ? util.ObtenerValor(oListItem["Etapa:ID"].ToString()) : ""));
                valores.Add("Etapa", ((oListItem["Etapa"] != null) ? util.ObtenerValor(oListItem["Etapa"].ToString()) : ""));
                valores.Add("IdSubEtapa", ((oListItem["SubEtapa:ID"] != null) ? util.ObtenerValor(oListItem["SubEtapa:ID"].ToString()) : ""));
                valores.Add("SubEtapa", ((oListItem["SubEtapa"] != null) ? util.ObtenerValor(oListItem["SubEtapa"].ToString()) : ""));
                valores.Add("IdEstado", ((oListItem["Estado:ID"] != null) ? util.ObtenerValor(oListItem["Estado:ID"].ToString()) : ""));
                valores.Add("Estado", ((oListItem["Estado"] != null) ? util.ObtenerValor(oListItem["Estado"].ToString()) : ""));

                valores.Add("Area", ((oListItem["Area"] != null) ? util.ObtenerValor(oListItem["Area"].ToString()) : ""));

                valores.Add("Cambio", ((oListItem["TipoCambio"] != null) ? util.ObtenerValor(oListItem["TipoCambio"].ToString()) : ""));
                valores.Add("Admin", ((oListItem["Administrador"] != null) ? util.ObtenerValor(oListItem["Administrador"].ToString()) : ""));
                return valores;
            }
            return valores;
        }

        public DataTable BuscarDatosOperacion(string idEmpresa, string idOperacion)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@ACCION", SqlDbType.NChar);
                SqlParametros[2].Value = 5;

                dt = AD_Global.ejecutarConsultas("GestionOperaciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public string ValidarWorkflowExpress(int idEmpresa, int IdOperacionActual)
        {
            try
            {
                string Mensaje = String.Empty;

                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacionActual", SqlDbType.Int);
                SqlParametros[1].Value = IdOperacionActual;

                Mensaje = AD_Global.traerPrimeraColumna("ValidacionWorkFlow", SqlParametros);
                return Mensaje;        
            }
            catch(Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "NO";
            }

        }


        #endregion

        #region "listarcomercial"

        public DataSet ListarComercial(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
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

                dt = AD_Global.ejecutarConsulta("ListarComercial", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //log.Error("Error metod Buscar" + ex.ToString());
                return new DataSet();
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
        }

        public void CargarLista(ref DropDownList lista, string Nombre_Lista)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                //string Nombre_Vista = wpListarNegocio.Interfaz.Constante.LISTAESTADO.VISTA;
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

        //public void CargarListaSubEtapa(ref DropDownList lista, string Nombre_Lista, string etapa)
        //{
        //    try
        //    {

        //        lista.Items.Clear();
        //        SPWeb app = SPContext.Current.Web;
        //        app.AllowUnsafeUpdates = true;
        //        SPList lis = app.Lists[Nombre_Lista];
        //        SPQuery view = new SPQuery();
        //        view.Query = "<Where><Eq><FieldRef Name='Etapa_x003a_ID'  /><Value Type='Lookup' >" + Convert.ToInt32(etapa) + "</Value></Eq></Where><OrderBy><FieldRef Name='Nombre' Ascending='TRUE' /></OrderBy>";
        //        SPListItemCollection collListItems = lis.GetItems(view);


        //        lista.DataSource = collListItems.GetDataTable();
        //        lista.DataTextField = "Title";
        //        lista.DataValueField = "ID";
        //        lista.DataBind();
        //        lista.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione " + Nombre_Lista + "--", "-1"));
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //    }
        //    finally
        //    {
        //        //  lista = null;

        //    }
        //}


        #endregion

        #region "wpPersonas"

        public DataTable AlertaPersonas(int idEmpresa, int idOperacion, string idUsuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsultas("sp_alertaPersonas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ValidarPersonas(int idEmpresaPersonas, int idEmpresa, string Rut, string DivRut, string FechaNacimiento, string Nombre, string IdEdoCivil, string DescEdoCivil, string Cargo, string Antiguedad, string Profesion,
           string Universidad, string Participacion, string Protesto, string Morosidad, string Patrimonio, string Email, string Celular, string Telefono, string Principal, string IdSocioAccionista,
           string IdDirectorio, string IdContacto, string IdRteLegal, string usuario, string opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[26];

                SqlParametros[25] = new SqlParameter("@IdEmpresaPersonas", SqlDbType.Int);
                SqlParametros[25].Value = idEmpresaPersonas;
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Rut", SqlDbType.Int);
                SqlParametros[1].Value = string.IsNullOrEmpty(Rut) ? 0 : int.Parse(Rut);
                SqlParametros[2] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[2].Value = DivRut;
                SqlParametros[3] = new SqlParameter("@FechaNacimiento", SqlDbType.NChar);
                SqlParametros[3].Value = FechaNacimiento;
                SqlParametros[4] = new SqlParameter("@Nombre", SqlDbType.NChar);
                SqlParametros[4].Value = Nombre;
                SqlParametros[5] = new SqlParameter("@IdEdoCivil", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(IdEdoCivil);
                SqlParametros[6] = new SqlParameter("@DescEdoCivil", SqlDbType.NVarChar);
                SqlParametros[6].Value = DescEdoCivil;
                SqlParametros[7] = new SqlParameter("@Cargo", SqlDbType.NVarChar);
                SqlParametros[7].Value = Cargo;
                SqlParametros[8] = new SqlParameter("@Antiguedad", SqlDbType.Int);
                SqlParametros[8].Value = int.Parse(Antiguedad);
                SqlParametros[9] = new SqlParameter("@Profesion", SqlDbType.NVarChar);
                SqlParametros[9].Value = Profesion;
                SqlParametros[10] = new SqlParameter("@Universidad", SqlDbType.NVarChar);
                SqlParametros[10].Value = Universidad;
                SqlParametros[11] = new SqlParameter("@Participacion", SqlDbType.Float);
                SqlParametros[11].Value = float.Parse(Participacion);
                SqlParametros[12] = new SqlParameter("@Protesto", SqlDbType.Float);
                SqlParametros[12].Value = float.Parse(Protesto);
                SqlParametros[13] = new SqlParameter("@Morosidad", SqlDbType.Float);
                SqlParametros[13].Value = float.Parse(Morosidad);
                SqlParametros[14] = new SqlParameter("@Patrimonio", SqlDbType.Float);
                SqlParametros[14].Value = float.Parse(Patrimonio);
                SqlParametros[15] = new SqlParameter("@Email", SqlDbType.NVarChar);
                SqlParametros[15].Value = Email;
                SqlParametros[16] = new SqlParameter("@Celular", SqlDbType.NVarChar);
                SqlParametros[16].Value = Celular;
                SqlParametros[17] = new SqlParameter("@Telefono", SqlDbType.NVarChar);
                SqlParametros[17].Value = Telefono;
                SqlParametros[18] = new SqlParameter("@Principal", SqlDbType.Int);
                SqlParametros[18].Value = int.Parse(Principal);
                SqlParametros[19] = new SqlParameter("@IdSocioAccionista", SqlDbType.NChar);
                SqlParametros[19].Value = IdSocioAccionista;
                SqlParametros[20] = new SqlParameter("@IdDirectorio", SqlDbType.NChar);
                SqlParametros[20].Value = IdDirectorio;
                SqlParametros[21] = new SqlParameter("@IdContacto", SqlDbType.NChar);
                SqlParametros[21].Value = IdContacto;
                SqlParametros[22] = new SqlParameter("@IdRteLegal", SqlDbType.NChar);
                SqlParametros[22].Value = IdRteLegal;
                SqlParametros[23] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[23].Value = usuario;
                SqlParametros[24] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[24].Value = opcion;

                dt = AD_Global.ejecutarConsultas("ValidarPersonas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ReglaNegocioPersonas(int idEmpresa, int idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                dt = AD_Global.ejecutarConsultas("sp_reglaNegocioPersonas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPersonasxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = "04";
                dt = AD_Global.ejecutarConsultas("GestionEmpresaPersonas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public bool InsertarPersonaxEmpresa(int idEmpresa, string Rut, string DivRut, string FechaNacimiento, string Nombre, string IdEdoCivil, string DescEdoCivil, string idCargo, string Cargo, string Antiguedad, string Profesion,
           string Universidad, string Participacion, string Protesto, string Morosidad, string Patrimonio, string Email, string Celular, string Telefono, string Principal, string IdSocioAccionista, string IdDirectorio, string IdContacto, string IdRteLegal, string usuario, int idRegimen, string descRegimen)
        {
            try
            {

                SqlParameter[] SqlParametros = new SqlParameter[28];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Rut", SqlDbType.Int);
                SqlParametros[1].Value = string.IsNullOrEmpty(Rut) ? 0 : int.Parse(Rut);
                SqlParametros[2] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[2].Value = DivRut;
                SqlParametros[3] = new SqlParameter("@FechaNacimiento", SqlDbType.NChar);
                SqlParametros[3].Value = FechaNacimiento;
                SqlParametros[4] = new SqlParameter("@Nombre", SqlDbType.NChar);
                SqlParametros[4].Value = Nombre;
                SqlParametros[5] = new SqlParameter("@IdEdoCivil", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(IdEdoCivil.Trim());
                SqlParametros[6] = new SqlParameter("@DescEdoCivil", SqlDbType.NVarChar);
                SqlParametros[6].Value = DescEdoCivil;
                SqlParametros[7] = new SqlParameter("@Cargo", SqlDbType.NVarChar);
                SqlParametros[7].Value = Cargo;
                SqlParametros[8] = new SqlParameter("@Antiguedad", SqlDbType.Int);
                SqlParametros[8].Value = int.Parse(Antiguedad);
                SqlParametros[9] = new SqlParameter("@Profesion", SqlDbType.NVarChar);
                SqlParametros[9].Value = Profesion;
                SqlParametros[10] = new SqlParameter("@Universidad", SqlDbType.NVarChar);
                SqlParametros[10].Value = Universidad;
                SqlParametros[11] = new SqlParameter("@Participacion", SqlDbType.Float);
                SqlParametros[11].Value = float.Parse(Participacion.Trim());
                SqlParametros[12] = new SqlParameter("@Protesto", SqlDbType.Float);
                SqlParametros[12].Value = float.Parse(Protesto.Trim());
                SqlParametros[13] = new SqlParameter("@Morosidad", SqlDbType.Float);
                SqlParametros[13].Value = float.Parse(Morosidad.Trim());
                SqlParametros[14] = new SqlParameter("@Patrimonio", SqlDbType.Float);
                SqlParametros[14].Value = float.Parse(Patrimonio.Trim());
                SqlParametros[15] = new SqlParameter("@Email", SqlDbType.NVarChar);
                SqlParametros[15].Value = Email.Trim();
                SqlParametros[16] = new SqlParameter("@Celular", SqlDbType.NVarChar);
                SqlParametros[16].Value = Celular.Trim();
                SqlParametros[17] = new SqlParameter("@Telefono", SqlDbType.NVarChar);
                SqlParametros[17].Value = Telefono.Trim();
                SqlParametros[18] = new SqlParameter("@Principal", SqlDbType.Int);
                SqlParametros[18].Value = int.Parse(Principal.Trim());
                SqlParametros[19] = new SqlParameter("@IdSocioAccionista", SqlDbType.NChar);
                SqlParametros[19].Value = IdSocioAccionista;
                SqlParametros[20] = new SqlParameter("@IdDirectorio", SqlDbType.NChar);
                SqlParametros[20].Value = IdDirectorio;
                SqlParametros[21] = new SqlParameter("@IdContacto", SqlDbType.NChar);
                SqlParametros[21].Value = IdContacto;
                SqlParametros[22] = new SqlParameter("@IdRteLegal", SqlDbType.NChar);
                SqlParametros[22].Value = IdRteLegal;
                SqlParametros[23] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[23].Value = usuario;
                SqlParametros[24] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[24].Value = "01";
                SqlParametros[25] = new SqlParameter("@idCargo", SqlDbType.Int);
                SqlParametros[25].Value = int.Parse(idCargo);
                SqlParametros[26] = new SqlParameter("@IdRegimen", SqlDbType.Int);
                SqlParametros[26].Value = idRegimen;
                SqlParametros[27] = new SqlParameter("@DescRegimen", SqlDbType.NVarChar);
                SqlParametros[27].Value = descRegimen;
                return AD_Global.ejecutarAccion("GestionEmpresaPersonas", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool ModificarPersonaxEmpresa(int idEmpresaPersonas, int idEmpresa, string Rut, string DivRut, string FechaNacimiento, string Nombre, string IdEdoCivil, string DescEdoCivil, string idCargo, string Cargo, string Antiguedad, string Profesion,
          string Universidad, string Participacion, string Protesto, string Morosidad, string Patrimonio, string Email, string Celular, string Telefono, string Principal, string IdSocioAccionista,
          string IdDirectorio, string IdContacto, string IdRteLegal, string usuario, int idRegimen, string descRegimen)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[29];

                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Rut", SqlDbType.Int);
                SqlParametros[1].Value = string.IsNullOrEmpty(Rut) ? 0 : int.Parse(Rut);
                SqlParametros[2] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[2].Value = DivRut;
                SqlParametros[3] = new SqlParameter("@FechaNacimiento", SqlDbType.NChar);
                SqlParametros[3].Value = FechaNacimiento;
                SqlParametros[4] = new SqlParameter("@Nombre", SqlDbType.NChar);
                SqlParametros[4].Value = Nombre;
                SqlParametros[5] = new SqlParameter("@IdEdoCivil", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(IdEdoCivil);
                SqlParametros[6] = new SqlParameter("@DescEdoCivil", SqlDbType.NVarChar);
                SqlParametros[6].Value = DescEdoCivil;
                SqlParametros[7] = new SqlParameter("@Cargo", SqlDbType.NVarChar);
                SqlParametros[7].Value = Cargo.Trim();
                SqlParametros[8] = new SqlParameter("@Antiguedad", SqlDbType.Int);
                SqlParametros[8].Value = int.Parse(Antiguedad.Trim());
                SqlParametros[9] = new SqlParameter("@Profesion", SqlDbType.NVarChar);
                SqlParametros[9].Value = Profesion.Trim();
                SqlParametros[10] = new SqlParameter("@Universidad", SqlDbType.NVarChar);
                SqlParametros[10].Value = Universidad.Trim();
                SqlParametros[11] = new SqlParameter("@Participacion", SqlDbType.Float);
                SqlParametros[11].Value = float.Parse(Participacion.Trim());
                SqlParametros[12] = new SqlParameter("@Protesto", SqlDbType.Float);
                SqlParametros[12].Value = float.Parse(Protesto.Trim());
                SqlParametros[13] = new SqlParameter("@Morosidad", SqlDbType.Float);
                SqlParametros[13].Value = float.Parse(Morosidad.Trim());
                SqlParametros[14] = new SqlParameter("@Patrimonio", SqlDbType.Float);
                SqlParametros[14].Value = float.Parse(Patrimonio.Trim());
                SqlParametros[15] = new SqlParameter("@Email", SqlDbType.NVarChar);
                SqlParametros[15].Value = Email.Trim();
                SqlParametros[16] = new SqlParameter("@Celular", SqlDbType.NVarChar);
                SqlParametros[16].Value = Celular.Trim();
                SqlParametros[17] = new SqlParameter("@Telefono", SqlDbType.NVarChar);
                SqlParametros[17].Value = Telefono.Trim();
                SqlParametros[18] = new SqlParameter("@Principal", SqlDbType.Int);
                SqlParametros[18].Value = int.Parse(Principal);
                SqlParametros[19] = new SqlParameter("@IdSocioAccionista", SqlDbType.NChar);
                SqlParametros[19].Value = IdSocioAccionista;
                SqlParametros[20] = new SqlParameter("@IdDirectorio", SqlDbType.NChar);
                SqlParametros[20].Value = IdDirectorio;
                SqlParametros[21] = new SqlParameter("@IdContacto", SqlDbType.NChar);
                SqlParametros[21].Value = IdContacto;
                SqlParametros[22] = new SqlParameter("@IdRteLegal", SqlDbType.NChar);
                SqlParametros[22].Value = IdRteLegal;
                SqlParametros[23] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[23].Value = usuario;
                SqlParametros[24] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[24].Value = "02";
                SqlParametros[25] = new SqlParameter("@IdEmpresaPersonas", SqlDbType.Int);
                SqlParametros[25].Value = idEmpresaPersonas;
                SqlParametros[26] = new SqlParameter("@idCargo", SqlDbType.Int);
                SqlParametros[26].Value = int.Parse(idCargo);
                SqlParametros[27] = new SqlParameter("@IdRegimen", SqlDbType.Int);
                SqlParametros[27].Value = idRegimen;
                SqlParametros[28] = new SqlParameter("@DescRegimen", SqlDbType.NVarChar);
                SqlParametros[28].Value = descRegimen;

                return AD_Global.ejecutarAccion("GestionEmpresaPersonas", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool EliminarPersonaxEmpresa(int idEmpresaPersonas, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdEmpresaPersonas", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaPersonas;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = "03";
                SqlParametros[2] = new SqlParameter("@usuario", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaPersonas", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpProveedores"

        public bool InsertarProveedorxEmpresa(int idEmpresa, string razonSocial, string rut, string divRut, string porcVentas, string plazoConvenioPago, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[2] = new SqlParameter("@Rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[3].Value = divRut;
                SqlParametros[4] = new SqlParameter("@razonSocial", SqlDbType.NChar);
                SqlParametros[4].Value = razonSocial;
                SqlParametros[5] = new SqlParameter("@porcVentas", SqlDbType.NChar);
                SqlParametros[5].Value = porcVentas;
                SqlParametros[6] = new SqlParameter("@plazoConvenio", SqlDbType.NChar);
                SqlParametros[6].Value = plazoConvenioPago;
                SqlParametros[7] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[7].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaProveedores", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarProveedoresxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionEmpresaProveedores", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return new DataTable();
            }
        }

        public bool EliminarProveedorxEmpresa(int idEmpresaProveedores, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresaProveedores", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaProveedores;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaProveedores", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public bool ModificarProveedorxEmpresa(int idEmpresaProveedores, string razonSocial, string rut, string divRut, string porcVentas, string plazoConvenioPago, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresaProveedores", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaProveedores;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[2] = new SqlParameter("@Rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@DivRut", SqlDbType.NChar);
                SqlParametros[3].Value = divRut;
                SqlParametros[4] = new SqlParameter("@razonSocial", SqlDbType.NChar);
                SqlParametros[4].Value = razonSocial;
                SqlParametros[5] = new SqlParameter("@porcVentas", SqlDbType.NChar);
                SqlParametros[5].Value = porcVentas;
                SqlParametros[6] = new SqlParameter("@plazoConvenio", SqlDbType.NChar);
                SqlParametros[6].Value = plazoConvenioPago;
                SqlParametros[7] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[7].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaProveedores", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpRechazo"

        public Boolean InsertarActualizarEstadosRechazo(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string subEtapa, string descsubEtapa, string comentario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = idUsuario;
                SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[4].Value = cargoUser;

                SqlParametros[5] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[5].Value = subEtapa;
                SqlParametros[6] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = descsubEtapa;

                SqlParametros[7] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[7].Value = comentario;

                dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public int ActualizarEstadoRechazo(String ID, string subEtapa)
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
                    item["IdSubEtapa"] = subEtapa;
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

        #endregion

        #region "wpSociosAccionistas"

        public Boolean InsertarSociosxEmpresa(string idEmpresa, string rut, string dvrut, string nombre, string porcentaje, string protesto, string morosidad)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@rut", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(rut.Replace(".", ""));
                SqlParametros[2] = new SqlParameter("@nombre", SqlDbType.NVarChar);
                SqlParametros[2].Value = nombre;
                SqlParametros[3] = new SqlParameter("@porcentaje", SqlDbType.Float);
                SqlParametros[3].Value = float.Parse(porcentaje.Replace(".", "").Replace(",", "."));
                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.INSERTAR;
                //@divRut varchar(1)=null,
                SqlParametros[5] = new SqlParameter("@divRut", SqlDbType.NChar);
                SqlParametros[5].Value = dvrut;
                SqlParametros[6] = new SqlParameter("@protesto", SqlDbType.Float);
                SqlParametros[6].Value = float.Parse(protesto.Replace(".", "").Replace(",", "."));
                SqlParametros[7] = new SqlParameter("@morosidad", SqlDbType.Float);
                SqlParametros[7].Value = float.Parse(morosidad.Replace(".", "").Replace(",", "."));
                return AD_Global.ejecutarAccion("GestionSocios", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarSociosxEmpresa(string idEmpresa, string idSociosEmpresa, string rut, string dvrut, string nombre, string porcentaje, string protesto, string morosidad)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[9];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idSociosEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idSociosEmpresa);
                SqlParametros[2] = new SqlParameter("@rut", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(rut.Replace(".", ""));
                SqlParametros[3] = new SqlParameter("@nombre", SqlDbType.NVarChar);
                SqlParametros[3].Value = nombre;
                SqlParametros[4] = new SqlParameter("@porcentaje", SqlDbType.Float);
                SqlParametros[4].Value = float.Parse(porcentaje.Replace(".", "").Replace(",", "."));
                SqlParametros[5] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[5].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[6] = new SqlParameter("@divRut", SqlDbType.NChar);
                SqlParametros[6].Value = dvrut;
                SqlParametros[7] = new SqlParameter("@protesto", SqlDbType.Float);
                SqlParametros[7].Value = float.Parse(protesto.Replace(".", "").Replace(",", "."));
                SqlParametros[8] = new SqlParameter("@morosidad", SqlDbType.Float);
                SqlParametros[8].Value = float.Parse(morosidad.Replace(".", "").Replace(",", "."));
                return AD_Global.ejecutarAccion("GestionSocios", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarSociosxEmpresa(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionSocios", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean EliminarSociosxEmpresa(string idEmpresa, string idSociosEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idSociosEmpresa", SqlDbType.NVarChar);
                SqlParametros[1].Value = idSociosEmpresa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.ELIMINAR;
                return AD_Global.ejecutarAccion("GestionSocios", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        #endregion

        #region "wpDirecciones"

        public DataTable ListarRegion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];

                dt = AD_Global.ejecutarConsultas("ListarRegion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        public DataTable ListarProvincia(int IdRegion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdRegion", SqlDbType.Int);
                SqlParametros[0].Value = IdRegion;

                dt = AD_Global.ejecutarConsultas("ListarProvincia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarComunas(int IdProvincia)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdProvincia", SqlDbType.Int);
                SqlParametros[0].Value = IdProvincia;

                dt = AD_Global.ejecutarConsultas("ListarComuna", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }
        

        public DataTable ListarComuna(int IdProvincia)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdProvincia", SqlDbType.Int);
                SqlParametros[0].Value = IdProvincia;

                dt = AD_Global.ejecutarConsultas("ListarComuna", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean ModificarDireccionxEmpresa(string idDireccionEmpresa, string idEmpresa, string idTipo, string descTipo,bool principal, string direccion, string idRegion, string descRegion,
          string idProvincia, string descProvincia, string idComuna, string descComuna, int idVerificacion, string descVerificacion, string fechaVerificacion, string usuario, string perfil, string Numero, string ComplementoDireccion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[20];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@principal", SqlDbType.Int);
                if (principal)
                    SqlParametros[1].Value = 1;
                else
                    SqlParametros[1].Value = 0;

                SqlParametros[2] = new SqlParameter("@idTipo", SqlDbType.NVarChar);
                SqlParametros[2].Value = idTipo;
                SqlParametros[3] = new SqlParameter("@descTipo", SqlDbType.NVarChar);
                SqlParametros[3].Value = descTipo;

                SqlParametros[4] = new SqlParameter("@idregion", SqlDbType.NVarChar);
                SqlParametros[4].Value = idRegion;
                SqlParametros[5] = new SqlParameter("@descRegion", SqlDbType.NVarChar);
                SqlParametros[5].Value = descRegion;

                SqlParametros[6] = new SqlParameter("@idProvincia", SqlDbType.NVarChar);
                SqlParametros[6].Value = idProvincia;
                SqlParametros[7] = new SqlParameter("@descProvincia", SqlDbType.NVarChar);
                SqlParametros[7].Value = descProvincia;

                SqlParametros[8] = new SqlParameter("@idComuna", SqlDbType.NVarChar);
                SqlParametros[8].Value = idComuna;
                SqlParametros[9] = new SqlParameter("@descComuna", SqlDbType.NVarChar);
                SqlParametros[9].Value = descComuna;

                SqlParametros[10] = new SqlParameter("@idVerificacion", SqlDbType.Int);
                SqlParametros[10].Value = idVerificacion;

                SqlParametros[11] = new SqlParameter("@descVerificacion", SqlDbType.NVarChar);
                SqlParametros[11].Value = descVerificacion;

                SqlParametros[12] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[12].Value = Constantes.OPCION.ACTUALIZARCONTACTO;

                SqlParametros[13] = new SqlParameter("@fechaVerificacion", SqlDbType.NVarChar);
                SqlParametros[13].Value = fechaVerificacion;

                SqlParametros[14] = new SqlParameter("@direccion", SqlDbType.NVarChar);
                SqlParametros[14].Value = direccion;

                SqlParametros[15] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[15].Value = usuario;

                SqlParametros[16] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[16].Value = perfil;

                SqlParametros[17] = new SqlParameter("@IdEmpresaDireccion", SqlDbType.Int);
                SqlParametros[17].Value = int.Parse(idDireccionEmpresa);

                SqlParametros[18] = new SqlParameter("@Numero", SqlDbType.NVarChar);
                SqlParametros[18].Value = Numero;

                SqlParametros[19] = new SqlParameter("@ComplementoDireccion", SqlDbType.NVarChar);
                SqlParametros[19].Value = ComplementoDireccion;

                return AD_Global.ejecutarAccion("GestionDireccionEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean EliminarDireccionxEmpresa(string idEmpresa, string idEmpresaDireccion, string usuario, string perfil)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@IdEmpresaDireccion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idEmpresaDireccion);

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[2].Value = Constantes.OPCION.ELIMINARCONTACTO;

                SqlParametros[3] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[3].Value = usuario;

                SqlParametros[4] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = perfil;
                return AD_Global.ejecutarAccion("GestionDireccionEmpresa", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarDireccionxEmpresa(string idEmpresa, string idTipo, string descTipo, bool principal, string direccion, string idRegion, string descRegion, string idProvincia, string descProvincia, string idComuna, string descComuna,
                                                 int idVerificacion, string descVerificacion, string fechaVerificacion, string usuario, string perfil, string Numero, string ComplementoDireccion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[19];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@principal", SqlDbType.Int);
                if (principal)
                    SqlParametros[1].Value = 1;
                else
                    SqlParametros[1].Value = 0;

                SqlParametros[2] = new SqlParameter("@idTipo", SqlDbType.NVarChar);
                SqlParametros[2].Value = idTipo;
                SqlParametros[3] = new SqlParameter("@descTipo", SqlDbType.NVarChar);
                SqlParametros[3].Value = descTipo;

                SqlParametros[4] = new SqlParameter("@idregion", SqlDbType.NVarChar);
                SqlParametros[4].Value = idRegion;
                SqlParametros[5] = new SqlParameter("@descRegion", SqlDbType.NVarChar);
                SqlParametros[5].Value = descRegion;

                SqlParametros[6] = new SqlParameter("@idProvincia", SqlDbType.NVarChar);
                SqlParametros[6].Value = idProvincia;
                SqlParametros[7] = new SqlParameter("@descProvincia", SqlDbType.NVarChar);
                SqlParametros[7].Value = descProvincia;

                SqlParametros[8] = new SqlParameter("@idComuna", SqlDbType.NVarChar);
                SqlParametros[8].Value = idComuna;
                SqlParametros[9] = new SqlParameter("@descComuna", SqlDbType.NVarChar);
                SqlParametros[9].Value = descComuna;

                SqlParametros[10] = new SqlParameter("@idVerificacion", SqlDbType.Int);
                SqlParametros[10].Value = idVerificacion;

                SqlParametros[11] = new SqlParameter("@descVerificacion", SqlDbType.NVarChar);
                SqlParametros[11].Value = descVerificacion;

                SqlParametros[12] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[12].Value = Constantes.OPCION.INSERTARCONTACTO;

                SqlParametros[13] = new SqlParameter("@fechaVerificacion", SqlDbType.NVarChar);
                SqlParametros[13].Value = fechaVerificacion;

                SqlParametros[14] = new SqlParameter("@direccion", SqlDbType.NVarChar);
                SqlParametros[14].Value = direccion;

                SqlParametros[15] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[15].Value = usuario;

                SqlParametros[16] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[16].Value = perfil;

                SqlParametros[17] = new SqlParameter("@Numero", SqlDbType.NVarChar);
                SqlParametros[17].Value = Numero;

                SqlParametros[18] = new SqlParameter("@ComplementoDireccion", SqlDbType.NVarChar);
                SqlParametros[18].Value = ComplementoDireccion;

                return AD_Global.ejecutarAccion("GestionDireccionEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarDireccionxEmpresa(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTARCONTACTOS;
                dt = AD_Global.ejecutarConsultas("GestionDireccionEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpEdicionEmpresa"

        public DataTable ValidarEmpresa(int idEmpresa, int idOperacion, string idUsuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                return AD_Global.ejecutarConsultas("sp_validacionesEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ReglaNegocioEmpresa(int idEmpresa, int idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                dt = AD_Global.ejecutarConsultas("sp_reglaNegocioEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        //public DataTable AlertaEmpresa(int idEmpresa, int idOperacion, string idUsuario, string perfil)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[4];

        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[1].Value = idOperacion;
        //        SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NChar);
        //        SqlParametros[2].Value = idUsuario;
        //        SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NChar);
        //        SqlParametros[3].Value = perfil;
        //        dt = AD_Global.ejecutarConsultas("sp_alertaEmpresa", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}


        #endregion

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


        #region "wpIngresoGarantia"


        public DataTable CargarContribuciones(int idGarantia)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idGarantia", SqlDbType.Int);
                SqlParametros[0].Value = idGarantia;

                return AD_Global.ejecutarConsultas("ListarContribucionesImpagas", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
         }
        
        public DataTable CertificadosPorEmitir(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                return AD_Global.ejecutarConsultas("cargarCertificadosPorEmitir", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
         }
        
        public Boolean InsertGarantias(string idEmpresa, string IdOperacion, string validacion, string xmlData, string user, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                SqlParametros[2] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(validacion);

                SqlParametros[3] = new SqlParameter("@accion", SqlDbType.NChar);
                SqlParametros[3].Value = 1;

                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;

                SqlParametros[5] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = user;

                SqlParametros[6] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[6].Value = perfil;

                return AD_Global.ejecutarAccion("GestionGarantiasNew", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ActualizaGarantiasExpress(string idEmpresa, string IdOperacion, string idGarantia, string validacion, string xmlData, string user, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                SqlParametros[2] = new SqlParameter("@idGarantia", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idGarantia);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(validacion);

                SqlParametros[4] = new SqlParameter("@accion", SqlDbType.NChar);
                SqlParametros[4].Value = 4;//act express

                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;

                SqlParametros[6] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;

                return AD_Global.ejecutarAccion("GestionGarantiasNew", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ActualizaGarantias(string idEmpresa, string IdOperacion, string idGarantia, string validacion, string xmlData, string user, string perfil, string ObservacionEstado, string ObservacionSeguro, string Observacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                SqlParametros[2] = new SqlParameter("@idGarantia", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idGarantia);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(validacion);

                SqlParametros[4] = new SqlParameter("@accion", SqlDbType.NChar);
                SqlParametros[4].Value = 2;

                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;

                SqlParametros[6] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;

                SqlParametros[8] = new SqlParameter("@ObservacionEstado", SqlDbType.NVarChar);
                SqlParametros[8].Value = ObservacionEstado;

                SqlParametros[9] = new SqlParameter("@ObservacionSeguro", SqlDbType.NVarChar);
                SqlParametros[9].Value = ObservacionSeguro;

                SqlParametros[10] = new SqlParameter("@Observacion", SqlDbType.NVarChar);
                SqlParametros[10].Value = Observacion;

                return AD_Global.ejecutarAccion("GestionGarantiasNew", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean EliminarGarantias(string idEmpresa, string IdOperacion, string idGarantia, string validacion, string user, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                SqlParametros[2] = new SqlParameter("@idGarantia", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idGarantia);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(validacion);

                SqlParametros[4] = new SqlParameter("@accion", SqlDbType.NChar);
                SqlParametros[4].Value = 6;

                SqlParametros[5] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[5].Value = user;

                SqlParametros[6] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[6].Value = perfil;

                return AD_Global.ejecutarAccion("GestionGarantiasNew", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ListaGarantias(string idEmpresa, string idOperacion, string user, string perfil)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@accion", SqlDbType.NChar);
                SqlParametros[1].Value = 5;

                SqlParametros[2] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = user;

                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;

                SqlParametros[4] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[4].Value = int.Parse(idOperacion);

                return AD_Global.ejecutarConsulta("GestionGarantiasNew", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable DatosGarantias(string idEmpresa, string idOperacion, string idGarantia, string usuario, string perfil)
        {
            DataTable dt = new DataTable();
            SqlParameter[] SqlParametros = new SqlParameter[6];
            SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
            //SqlParametros[0].Value = int.Parse(idOperacion);
            SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
            SqlParametros[1].Value = int.Parse(idEmpresa);

            SqlParametros[2] = new SqlParameter("@idGarantia", SqlDbType.Int);
            SqlParametros[2].Value = int.Parse(idGarantia);

            SqlParametros[3] = new SqlParameter("@accion", SqlDbType.Int);
            SqlParametros[3].Value = 3;

            SqlParametros[4] = new SqlParameter("@usuario", SqlDbType.NVarChar);
            SqlParametros[4].Value = usuario;

            SqlParametros[5] = new SqlParameter("@perfil", SqlDbType.NVarChar);
            SqlParametros[5].Value = perfil;

            dt = AD_Global.ejecutarConsultas("GestionGarantiasNew", SqlParametros);
            return dt;
        }

        public DataTable DatosGarantias(string idEmpresa, string idGarantia, string usuario, string perfil)
        {
            DataTable dt = new DataTable();
            SqlParameter[] SqlParametros = new SqlParameter[5];
            SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
            SqlParametros[0].Value = int.Parse(idEmpresa);

            SqlParametros[1] = new SqlParameter("@idGarantia", SqlDbType.Int);
            SqlParametros[1].Value = int.Parse(idGarantia);

            SqlParametros[2] = new SqlParameter("@accion", SqlDbType.Int);
            SqlParametros[2].Value = 3;

            SqlParametros[3] = new SqlParameter("@usuario", SqlDbType.NVarChar);
            SqlParametros[3].Value = usuario;

            SqlParametros[4] = new SqlParameter("@perfil", SqlDbType.NVarChar);
            SqlParametros[4].Value = perfil;

            dt = AD_Global.ejecutarConsultas("GestionGarantiasNew", SqlParametros);
            return dt;
        }

        public DataTable ReglaNegocioGarantia(int idEmpresa, int idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                dt = AD_Global.ejecutarConsultas("sp_reglaNegocioGarantia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable AlertaGarantia(int idEmpresa, int idOperacion, string idUsuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NChar);
                SqlParametros[3].Value = perfil;

                dt = AD_Global.ejecutarConsultas("sp_alertaGarantia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public string ValidarGarantia(int idEmpresa, int idOperacion, string area, string ddlTipoG, string ddlTipoBienes, string txtNroInscripcion, string ddlRegiones, string ddlProvincia, string ddlComunas, string txtDescP, string txtValorC, string txtValorA, string txtValorAseg, string identificador, bool ReqSeguro, string NumPoliza, string Contratante, string CompaniaSeguro)
        {
            // DateTime VigenciaSeguro, DateTime VencimientoSeguro,
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[18];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@area", SqlDbType.NVarChar);
                SqlParametros[2].Value = area;

                SqlParametros[3] = new SqlParameter("@ddlTipoG", SqlDbType.NVarChar);
                SqlParametros[3].Value = ddlTipoG;

                SqlParametros[4] = new SqlParameter("@ddlTipoBienes", SqlDbType.NVarChar);
                SqlParametros[4].Value = ddlTipoBienes;

                SqlParametros[5] = new SqlParameter("@txtNroInscripcion", SqlDbType.NVarChar);
                SqlParametros[5].Value = txtNroInscripcion;

                SqlParametros[6] = new SqlParameter("@ddlRegiones", SqlDbType.NVarChar);
                SqlParametros[6].Value = ddlRegiones;

                SqlParametros[7] = new SqlParameter("@ddlProvincia", SqlDbType.NVarChar);
                SqlParametros[7].Value = ddlProvincia;

                SqlParametros[8] = new SqlParameter("@ddlComunas", SqlDbType.NVarChar);
                SqlParametros[8].Value = ddlComunas;
                
                SqlParametros[9] = new SqlParameter("@txtDescP", SqlDbType.NVarChar);
                SqlParametros[9].Value = txtDescP;

                SqlParametros[10] = new SqlParameter("@txtValorC", SqlDbType.NVarChar);
                SqlParametros[10].Value = txtValorC;

                SqlParametros[11] = new SqlParameter("@txtValorA", SqlDbType.NVarChar);
                SqlParametros[11].Value = txtValorA;

                SqlParametros[12] = new SqlParameter("@txtValorAseg", SqlDbType.NVarChar);
                SqlParametros[12].Value = txtValorAseg;

                SqlParametros[13] = new SqlParameter("@Identificador", SqlDbType.NVarChar);
                SqlParametros[13].Value = identificador;

                SqlParametros[14] = new SqlParameter("@ReqSeguro", SqlDbType.Bit);
                SqlParametros[14].Value = ReqSeguro;

                SqlParametros[15] = new SqlParameter("@NumPoliza", SqlDbType.NVarChar);
                SqlParametros[15].Value = NumPoliza;

                SqlParametros[16] = new SqlParameter("@Contratante", SqlDbType.NVarChar);
                SqlParametros[16].Value = Contratante;

                //SqlParametros[17] = new SqlParameter("@VigenciaSeguro", SqlDbType.DateTime);
                //SqlParametros[17].Value = VigenciaSeguro;

                //SqlParametros[18] = new SqlParameter("@VencimientoSeguro", SqlDbType.DateTime);
                //SqlParametros[18].Value = VencimientoSeguro;

                SqlParametros[17] = new SqlParameter("@CompaniaSeguro", SqlDbType.NVarChar);
                SqlParametros[17].Value = CompaniaSeguro;

                return AD_Global.traerPrimeraColumna("sp_validacionesGarantia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "";
            }
        }

        public Boolean InsertarModifcarTasacionGarantias(string IdTasacion, string IdGarantia, string IdEmpresa, string Idoperacion, string NroTasacion, DateTime? FechaTasacion, int IdEmpresaTasadora, string DescEmpresaTasadora, int IdTasador, string DescTasador, bool EstadoTasacion, string NombreDoc, string user, string opcion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@IdTasacion", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(IdTasacion);

                SqlParametros[1] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[1].Value = IdGarantia;

                SqlParametros[2] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(IdEmpresa);

                SqlParametros[3] = new SqlParameter("@Idoperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(Idoperacion);

                SqlParametros[4] = new SqlParameter("@NroTasacion", SqlDbType.NVarChar);
                SqlParametros[4].Value = NroTasacion;

                SqlParametros[5] = new SqlParameter("@FechaTasacion", SqlDbType.DateTime);
                SqlParametros[5].Value = FechaTasacion;

                SqlParametros[6] = new SqlParameter("@IdEmpresaTasadora", SqlDbType.Int);
                SqlParametros[6].Value = IdEmpresaTasadora;

                SqlParametros[7] = new SqlParameter("@DescEmpresaTasadora", SqlDbType.NVarChar);
                SqlParametros[7].Value = DescEmpresaTasadora;

                SqlParametros[8] = new SqlParameter("@IdTasador", SqlDbType.Int);
                SqlParametros[8].Value = IdTasador;

                SqlParametros[9] = new SqlParameter("@DescTasador", SqlDbType.NVarChar);
                SqlParametros[9].Value = DescTasador;

                SqlParametros[10] = new SqlParameter("@EstadoTasacion", SqlDbType.Bit);
                SqlParametros[10].Value = EstadoTasacion;

                SqlParametros[11] = new SqlParameter("@NombreDoc", SqlDbType.NVarChar);
                SqlParametros[11].Value = NombreDoc;

                SqlParametros[12] = new SqlParameter("@UsuarioCreacion", SqlDbType.NVarChar);
                SqlParametros[12].Value = user;

                SqlParametros[13] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[13].Value = opcion;

                return AD_Global.ejecutarAccion("InsertarModifcarTasacionGarantias", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean GuardarDatosHistoriaTasacion(int IdGarantia, XmlDocument xml, int idEmpresa, int idOperacion, string usuario, int idTasacion, int accion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[0].Value = IdGarantia;

                SqlParametros[1] = new SqlParameter("@Xml", SqlDbType.Xml);
                SqlParametros[1].Value = new StringReader(xml.OuterXml);

                SqlParametros[2] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[2].Value = idEmpresa;

                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = idOperacion;
                
                SqlParametros[4] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@idTasacion", SqlDbType.Int);
                SqlParametros[5].Value = idTasacion;

                SqlParametros[6] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[6].Value = accion;

                return AD_Global.ejecutarAccion("HistorialTasacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable CargarDatosTasacionGarantias(int IdGarantia, XmlDocument xml, int idEmpresa, int idOperacion, string usuario, int idTasacion, int accion)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[0].Value = IdGarantia;

                SqlParametros[1] = new SqlParameter("@Xml", SqlDbType.Xml);
                if(xml == null)
                    SqlParametros[1].Value = null;
                else
                    SqlParametros[1].Value = new StringReader(xml.OuterXml);

                SqlParametros[2] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[2].Value = idEmpresa;

                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = idOperacion;

                SqlParametros[4] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@idTasacion", SqlDbType.Int);
                SqlParametros[5].Value = idTasacion;

                SqlParametros[6] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[6].Value = accion;

                dt = AD_Global.ejecutarConsultas("HistorialTasacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable CargarDatosHistoriaSeguros(int IdGarantia, string Xml, string ObservacionSeguro, string usuario, int IdSeguro, int accion)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[0].Value = IdGarantia;

                SqlParametros[1] = new SqlParameter("@Xml", SqlDbType.NVarChar);
                SqlParametros[1].Value = Xml;

                SqlParametros[2] = new SqlParameter("@ObservacionSeguro", SqlDbType.NVarChar);
                SqlParametros[2].Value = ObservacionSeguro;

                SqlParametros[3] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = usuario;

                SqlParametros[4] = new SqlParameter("@IdSeguro", SqlDbType.Int);
                SqlParametros[4].Value = IdSeguro;

                SqlParametros[5] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[5].Value = accion;

                dt = AD_Global.ejecutarConsultas("HistorialSeguros", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean GuardarDatosHistoriaSeguros(int IdGarantia, XmlDocument xml, string ObservacionSeguro, string usuario, int IdSeguro, int accion)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[0].Value = IdGarantia;

                SqlParametros[1] = new SqlParameter("@Xml", SqlDbType.Xml);
                SqlParametros[1].Value = new StringReader(xml.OuterXml); //new StringReader();

                SqlParametros[2] = new SqlParameter("@ObservacionSeguro", SqlDbType.NVarChar);
                SqlParametros[2].Value = ObservacionSeguro;

                SqlParametros[3] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = usuario;

                SqlParametros[4] = new SqlParameter("@IdSeguro", SqlDbType.Int);
                SqlParametros[4].Value = IdSeguro;

                SqlParametros[5] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[5].Value = accion;

                return AD_Global.ejecutarAccion("HistorialSeguros", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpIngresoOperacion"

        public string ValidarIngresoOperacion(Operaciones datosOperaciones)
        {
            string mensaje = string.Empty;

            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[7];
                sqlParam[0] = new SqlParameter("@descProducto", datosOperaciones._descProducto);
                sqlParam[1] = new SqlParameter("@DescMoneda", datosOperaciones._descMoneda);
                sqlParam[2] = new SqlParameter("@finalidad", datosOperaciones._finalidad);
                sqlParam[3] = new SqlParameter("@plazo", datosOperaciones._plazo);
                sqlParam[4] = new SqlParameter("@montoOper", datosOperaciones._montoOper);
                sqlParam[5] = new SqlParameter("@fecEmision", datosOperaciones._fecEmis);
                sqlParam[6] = new SqlParameter("@opcion", 1);

                return AD_Global.traerPrimeraColumna("sp_validacionesOperacion", sqlParam);
            }
            catch
            {
                return mensaje = "";
            }
        }

        public static string IngresaActualizaOperaciones(Operaciones datosOperaciones, string accion, string usuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[36];
                sqlParam[0] = new SqlParameter("@idOperacion", datosOperaciones._idOperacion);
                sqlParam[1] = new SqlParameter("@idEmpresa", datosOperaciones._idEmpresa);
                sqlParam[2] = new SqlParameter("@descProducto", datosOperaciones._descProducto);
                sqlParam[3] = new SqlParameter("@descEtapa", datosOperaciones._descEtapa);
                sqlParam[4] = new SqlParameter("@descSubEtapa", datosOperaciones._descSubEtapa);
                sqlParam[5] = new SqlParameter("@finalidad", datosOperaciones._finalidad);
                sqlParam[6] = new SqlParameter("@montoOper", datosOperaciones._montoOper);
                sqlParam[7] = new SqlParameter("@plazo", datosOperaciones._plazo);
                sqlParam[8] = new SqlParameter("@idMoneda", datosOperaciones._idMoneda);
                sqlParam[9] = new SqlParameter("@idCanal", datosOperaciones._idCanal);
                sqlParam[10] = new SqlParameter("@DescMoneda", datosOperaciones._descMoneda);
                sqlParam[11] = new SqlParameter("@DescCanal", datosOperaciones._descCanal);
                sqlParam[12] = new SqlParameter("@idProducto", datosOperaciones._idProducto);
                sqlParam[13] = new SqlParameter("@idEtapa", datosOperaciones._idEtapa);
                sqlParam[14] = new SqlParameter("@idSubEtapa", datosOperaciones._idSubEtapa);
                sqlParam[15] = new SqlParameter("@idFinalidad", datosOperaciones._idFinalidad);
                sqlParam[16] = new SqlParameter("@accion", accion);
                sqlParam[17] = new SqlParameter("@usuario", usuario);
                sqlParam[18] = new SqlParameter("@validacion", datosOperaciones._validacion);
                sqlParam[19] = new SqlParameter("@idEstado", datosOperaciones._idEstado);
                sqlParam[20] = new SqlParameter("@descEstado", datosOperaciones._descEstado);
                sqlParam[21] = new SqlParameter("@descArea", datosOperaciones._descArea);
                sqlParam[22] = new SqlParameter("@habilitado", datosOperaciones._habilitado);
                sqlParam[23] = new SqlParameter("@fecEmision", datosOperaciones._fecEmis);
                sqlParam[24] = new SqlParameter("@idEjecutivo", datosOperaciones._idEjecutivo);
                sqlParam[25] = new SqlParameter("@descEjecutivo", datosOperaciones._descEjecutivo);
                sqlParam[26] = new SqlParameter("@periodo", datosOperaciones._periodo);
                sqlParam[27] = new SqlParameter("@idTipoCta", datosOperaciones._idTipoCta);
                sqlParam[28] = new SqlParameter("@descTipoCta", datosOperaciones._descTipoCta);
                sqlParam[29] = new SqlParameter("@idTipoAmort", datosOperaciones._idTipoAmort);
                sqlParam[30] = new SqlParameter("@descTipoAmort", datosOperaciones._descTipoAmort);
                sqlParam[31] = new SqlParameter("@numCta", datosOperaciones._nroCta);
                sqlParam[32] = new SqlParameter("@porcComision", datosOperaciones._comision);
                sqlParam[33] = new SqlParameter("@comisionCLP", datosOperaciones._comisionCLP);
                sqlParam[34] = new SqlParameter("@incluyeComis", datosOperaciones._tieneComis);
                sqlParam[35] = new SqlParameter("@idSP", datosOperaciones._idSH);

                return AD_Global.traerPrimeraColumna("InsertaActualizaOperaciones", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public static string TraeNombreEjecutivo(int idEmpresa)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@idEmpresa", idEmpresa);
                return AD_Global.traerPrimeraColumna("TraeNombreEjecutivo", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public static string clienteBloquedo(int idEmpresa)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@idEmpresa", idEmpresa);
                return AD_Global.traerPrimeraColumna("clienteBloquedo", sqlParam);
            }
            catch
            {
                return "-1";
            }
        }

        public static DataSet TraeOperaciones(int idEmpresa, bool habilitado, string area)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[3];
                sqlParam[0] = new SqlParameter("@idEmpresa", idEmpresa);
                sqlParam[1] = new SqlParameter("@habilitado", habilitado);
                sqlParam[2] = new SqlParameter("@area", area);
                return AD_Global.ejecutarConsulta("ListaOperaciones", sqlParam);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region "wpContactoEmpresa"

        public DataTable ListarContactosxEmpresa(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTARCONTACTOS;
                dt = AD_Global.ejecutarConsultas("GestionContactosEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertarContactoxEmpresa(string idEmpresa, string rut, string divrut, string nombres, string apellidopaterno, string apellidomaterno, string email, string telfijo, string telcelular, string cargo, bool principal)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@principal", SqlDbType.Int);
                if (principal)
                    SqlParametros[1].Value = 1;
                else
                    SqlParametros[1].Value = 0;

                SqlParametros[2] = new SqlParameter("@nombres", SqlDbType.NVarChar);
                SqlParametros[2].Value = nombres;
                SqlParametros[3] = new SqlParameter("@apellidopaterno", SqlDbType.NVarChar);
                SqlParametros[3].Value = apellidopaterno;
                SqlParametros[4] = new SqlParameter("@apellidomaterno", SqlDbType.NVarChar);
                SqlParametros[4].Value = apellidomaterno;
                SqlParametros[5] = new SqlParameter("@email", SqlDbType.NVarChar);
                SqlParametros[5].Value = email;
                SqlParametros[6] = new SqlParameter("@telfijo", SqlDbType.NVarChar);
                SqlParametros[6].Value = telfijo;
                SqlParametros[7] = new SqlParameter("@telcelular", SqlDbType.NVarChar);
                SqlParametros[7].Value = telcelular;
                SqlParametros[8] = new SqlParameter("@cargo", SqlDbType.NVarChar);
                SqlParametros[8].Value = cargo;
                SqlParametros[9] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[9].Value = Constantes.OPCION.INSERTARCONTACTO;
                return AD_Global.ejecutarAccion("GestionContactosEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public Boolean EliminarContactoxEmpresa(string idEmpresa, string idEmpresaContacto)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@IdEmpresaContacto", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idEmpresaContacto);
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[2].Value = Constantes.OPCION.ELIMINARCONTACTO;
                return AD_Global.ejecutarAccion("GestionContactosEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarRelacionadasxEmpresa(string idEmpresa, string idcontactoempresa, string rut, string divrut, string nombres, string apellidopaterno, string apellidomaterno, string email, string telfijo, string telcelular, string cargo, bool principal)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[11];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@principal", SqlDbType.Int);
                if (principal)
                    SqlParametros[1].Value = 1;
                else
                    SqlParametros[1].Value = 0;

                SqlParametros[2] = new SqlParameter("@nombres", SqlDbType.NVarChar);
                SqlParametros[2].Value = nombres;
                SqlParametros[3] = new SqlParameter("@apellidopaterno", SqlDbType.NVarChar);
                SqlParametros[3].Value = apellidopaterno;
                SqlParametros[4] = new SqlParameter("@apellidomaterno", SqlDbType.NVarChar);
                SqlParametros[4].Value = apellidomaterno;
                SqlParametros[5] = new SqlParameter("@email", SqlDbType.NVarChar);
                SqlParametros[5].Value = email;
                SqlParametros[6] = new SqlParameter("@telfijo", SqlDbType.NVarChar);
                SqlParametros[6].Value = telfijo;
                SqlParametros[7] = new SqlParameter("@telcelular", SqlDbType.NVarChar);
                SqlParametros[7].Value = telcelular;
                SqlParametros[8] = new SqlParameter("@cargo", SqlDbType.NVarChar);
                SqlParametros[8].Value = cargo;
                SqlParametros[9] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[9].Value = Constantes.OPCION.ACTUALIZARCONTACTO;
                SqlParametros[10] = new SqlParameter("@IdEmpresaContacto", SqlDbType.Int);
                SqlParametros[10].Value = int.Parse(idcontactoempresa);

                return AD_Global.ejecutarAccion("GestionContactosEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        #endregion

        #region "wpDatosOperacion"

        public string ValidarReglasNegocioOperacion(Operaciones datosOperaciones, string uf)
        {
            string mensaje = string.Empty;

            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[29];
                sqlParam[0] = new SqlParameter("@descProducto", datosOperaciones._descProducto);
                sqlParam[1] = new SqlParameter("@descEtapa", datosOperaciones._descEtapa);
                sqlParam[2] = new SqlParameter("@descSubEtapa", datosOperaciones._descSubEtapa);            
                sqlParam[3] = new SqlParameter("@tipoCredito", datosOperaciones._tipoCredito);
                sqlParam[4] = new SqlParameter("@horizonte", datosOperaciones._horizonte);
                sqlParam[5] = new SqlParameter("@montoCuoton", datosOperaciones._montoCuoton);
                sqlParam[6] = new SqlParameter("@tieneFogape", datosOperaciones._tieneFogape);
                sqlParam[7] = new SqlParameter("@codigoSafio", datosOperaciones._codigoSafio);
                //sqlParam[8] = new SqlParameter("@garantia", datosOperaciones._garantia);
                sqlParam[8] = new SqlParameter("@estadoCertificado", datosOperaciones._estadoCertificado);
                sqlParam[9] = new SqlParameter("@fechaRecuperacion", datosOperaciones._fechaRecuperacion);
                sqlParam[10] = new SqlParameter("@montoRecuperoFogape", datosOperaciones._montoRecuperoFogape);
                sqlParam[11] = new SqlParameter("@motivoGarantiaFogape", datosOperaciones._motivoGarantiaFogape);
                sqlParam[12] = new SqlParameter("@coberturaFogape", datosOperaciones._coberturaFogape);
                sqlParam[13] = new SqlParameter("@garantiaFogapeVigente", datosOperaciones._garantiaFogapeVigente);
                sqlParam[14] = new SqlParameter("@descFondo", datosOperaciones._descFondo);
                sqlParam[15] = new SqlParameter("@idEmpresa", datosOperaciones._idEmpresa);
                sqlParam[16] = new SqlParameter("@idOperacion", datosOperaciones._idOperacion);
                sqlParam[17] = new SqlParameter("@comision", datosOperaciones._comision);
                sqlParam[18] = new SqlParameter("@descFinalidad", datosOperaciones._finalidad);
                sqlParam[19] = new SqlParameter("@totalPlazo", datosOperaciones._plazo);
                sqlParam[20] = new SqlParameter("@fechaVencimientoCredito", datosOperaciones._fecEmis);
                sqlParam[21] = new SqlParameter("@montoOperacion", datosOperaciones._montoOper);
                sqlParam[22] = new SqlParameter("@incluyeSafio", datosOperaciones._incluyeSafio);
                sqlParam[23] = new SqlParameter("@cobertura", datosOperaciones._cobertura);
                sqlParam[24] = new SqlParameter("@DescTipoMoneda", datosOperaciones._descMoneda);
                sqlParam[25] = new SqlParameter("@ComisionFogape", datosOperaciones._ComisionFogape);
                sqlParam[26] = new SqlParameter("@uf", uf);
                sqlParam[27] = new SqlParameter("@fechaEstCierre", datosOperaciones._fechaEstCierre);  
                sqlParam[28] = new SqlParameter("@opcion", 2);

                return AD_Global.traerPrimeraColumna("sp_reglaNegocioOperacion", sqlParam);
            }
            catch
            {
                return mensaje = "";
            }
        }

        public DataTable api_LicitacionFogape(string IdFondo)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];

                SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                SqlParametros[0].Value = IdFondo;

                return AD_Global.ejecutarConsultas("api_LicitacionFogape", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
        }

        public string AlertaOperacion(int idEmpresa, int idOperacion, string idUsuario, string perfil)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NChar);
                SqlParametros[3].Value = perfil;

                return AD_Global.traerPrimeraColumna("sp_alertaOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje = "";
            }
        }

        public DataSet CargarDatosOperacion(int idEmpresa, int idOperacion, string area)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@area", SqlDbType.NChar);
                SqlParametros[2].Value = area;
                dt = AD_Global.ejecutarConsulta("CargarDatosOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean ModificarOperacion(int idEmpresa, int IdOperacion, string xmlData, string usuario, string perfil, string area, string glosaComercial, string instruccionCurse, string observacion, string destinoSolicitud, string comentarios)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[9];
   
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = area;

                SqlParametros[2] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[2].Value = xmlData;

                SqlParametros[3] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = usuario;

                SqlParametros[4] = new SqlParameter("@glosaComercial", SqlDbType.NVarChar);
                SqlParametros[4].Value = glosaComercial;

                SqlParametros[5] = new SqlParameter("@instruccionCurse", SqlDbType.NVarChar);
                SqlParametros[5].Value = instruccionCurse;

                SqlParametros[6] = new SqlParameter("@observacion", SqlDbType.NVarChar);
                SqlParametros[6].Value = observacion;

                SqlParametros[7] = new SqlParameter("@destinoSolicitud", SqlDbType.NVarChar);
                SqlParametros[7].Value = destinoSolicitud;

                SqlParametros[8] = new SqlParameter("@comentarios", SqlDbType.NVarChar);
                SqlParametros[8].Value = comentarios;

                //SqlParametros[9] = new SqlParameter("@DescRepresentanteLegal", SqlDbType.NVarChar);
                //SqlParametros[9].Value = DescRepresentanteLegal;

                //SqlParametros[10] = new SqlParameter("@DescRepresentanteFondo", SqlDbType.NVarChar);
                //SqlParametros[10].Value = DescRepresentanteFondo;

                return AD_Global.ejecutarAccion("ModificarOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool GuardarCertificados(int idOperacion, string NCertificados, string opcion, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                SqlParametros[1] = new SqlParameter("@NCertificados", SqlDbType.NChar);
                SqlParametros[1].Value = NCertificados;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[2].Value = opcion;
                SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = usuario;
                return AD_Global.ejecutarAccion("GestionCertificadosOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ReglaNegocioOperacion(int idEmpresa, int idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                dt = AD_Global.ejecutarConsultas("sp_reglaNegocioOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ValidarFondos(string idEmpresa, string idOperacion, string idPaf, string user, string perfil, Dictionary<string, string> datos, string costoFondos, string idFondo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idOperacion);

                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idEmpresa);

                SqlParametros[2] = new SqlParameter("@Porcentaje", SqlDbType.Float);
                SqlParametros[2].Value = float.Parse(datos["Porcentaje"]);

                SqlParametros[3] = new SqlParameter("@FechaVencimiento", SqlDbType.NVarChar);
                SqlParametros[3].Value = datos["FechaVencimiento"];

                SqlParametros[4] = new SqlParameter("@CostoFondo", SqlDbType.Float);
                SqlParametros[4].Value = float.Parse(datos["CostoFondo"]);

                SqlParametros[5] = new SqlParameter("@VentasUFMax", SqlDbType.Float);
                SqlParametros[5].Value = float.Parse(datos["VentasUFMax"]);

                SqlParametros[6] = new SqlParameter("@GarantiasCoberturaMin", SqlDbType.Float);
                SqlParametros[6].Value = float.Parse(datos["GarantiasCoberturaMin"]);

                SqlParametros[7] = new SqlParameter("@ValorFondo", SqlDbType.Float);
                SqlParametros[7].Value = float.Parse(datos["ValorFondo"]);

                SqlParametros[8] = new SqlParameter("@CostoFondoIngresado", SqlDbType.Float);
                SqlParametros[8].Value = float.Parse(costoFondos);

                SqlParametros[9] = new SqlParameter("@Fondo", SqlDbType.Float);
                SqlParametros[9].Value = int.Parse(idFondo);

                dt = AD_Global.ejecutarConsultas("ValidarFondo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean ModificarFondoOperacion(int IdOperacion, string fondo, int idfondo, double costoFondo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                SqlParametros[1] = new SqlParameter("@fondo", SqlDbType.NChar);
                SqlParametros[1].Value = fondo;

                SqlParametros[2] = new SqlParameter("@idfondo", SqlDbType.Int);
                SqlParametros[2].Value = idfondo;

                SqlParametros[3] = new SqlParameter("@costoFondo", SqlDbType.Float);
                SqlParametros[3].Value = costoFondo;

                return AD_Global.ejecutarAccion("ModificarFondoOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable DatosFondosPorId(int IdFondo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("DatosFondosPorId", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
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

        #endregion

        #region "wpAdministracion"

        public DataTable ListarAdministracionxEmpresa(int idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GestionEmpresaAdministracion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public bool EliminarAdministracionxEmpresa(int idEmpresaAdministracion, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresaAdministracion", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaAdministracion;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.ELIMINAR;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[2].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaAdministracion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool ModificarAdministracionxEmpresa(int idEmpresaAdministracion, string nombre, string rut, string divRut, string profesion, string cargo, string idEdoCivil,
           string descEdoCivil, string fecNac, string antiguedad, string telefono, string mail, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@idEmpresaAdministracion", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresaAdministracion;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[2] = new SqlParameter("@rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@divrut", SqlDbType.NChar);
                SqlParametros[3].Value = divRut;
                SqlParametros[4] = new SqlParameter("@nombre", SqlDbType.NChar);
                SqlParametros[4].Value = nombre;
                SqlParametros[5] = new SqlParameter("@profesion", SqlDbType.NChar);
                SqlParametros[5].Value = profesion;
                SqlParametros[6] = new SqlParameter("@cargo", SqlDbType.NChar);
                SqlParametros[6].Value = cargo;
                SqlParametros[7] = new SqlParameter("@idEdoCivil", SqlDbType.NChar);
                SqlParametros[7].Value = idEdoCivil;
                SqlParametros[8] = new SqlParameter("@descEdoCivil", SqlDbType.NChar);
                SqlParametros[8].Value = descEdoCivil;
                SqlParametros[9] = new SqlParameter("@fecNacim", SqlDbType.NChar);
                SqlParametros[9].Value = fecNac;
                SqlParametros[10] = new SqlParameter("@antiguedad", SqlDbType.NChar);
                SqlParametros[10].Value = antiguedad;
                SqlParametros[11] = new SqlParameter("@telefono", SqlDbType.NChar);
                SqlParametros[11].Value = telefono;
                SqlParametros[12] = new SqlParameter("@mail", SqlDbType.NChar);
                SqlParametros[12].Value = mail;
                SqlParametros[13] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[13].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaAdministracion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool InsertarAdministracionxEmpresa(int idEmpresa, string nombre, string rut, string divRut, string profesion, string cargo, string idEdoCivil,
          string descEdoCivil, string fecNac, string antiguedad, string telefono, string mail, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[2] = new SqlParameter("@rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@divrut", SqlDbType.NChar);
                SqlParametros[3].Value = divRut;
                SqlParametros[4] = new SqlParameter("@nombre", SqlDbType.NChar);
                SqlParametros[4].Value = nombre;
                SqlParametros[5] = new SqlParameter("@profesion", SqlDbType.NChar);
                SqlParametros[5].Value = profesion;
                SqlParametros[6] = new SqlParameter("@cargo", SqlDbType.NChar);
                SqlParametros[6].Value = cargo;
                SqlParametros[7] = new SqlParameter("@idEdoCivil", SqlDbType.NChar);
                SqlParametros[7].Value = idEdoCivil;
                SqlParametros[8] = new SqlParameter("@descEdoCivil", SqlDbType.NChar);
                SqlParametros[8].Value = descEdoCivil;
                SqlParametros[9] = new SqlParameter("@fecNacim", SqlDbType.NChar);
                SqlParametros[9].Value = fecNac;
                SqlParametros[10] = new SqlParameter("@antiguedad", SqlDbType.NChar);
                SqlParametros[10].Value = antiguedad;
                SqlParametros[11] = new SqlParameter("@telefono", SqlDbType.NChar);
                SqlParametros[11].Value = telefono;
                SqlParametros[12] = new SqlParameter("@mail", SqlDbType.NChar);
                SqlParametros[12].Value = mail;
                SqlParametros[13] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[13].Value = usuario;
                return AD_Global.ejecutarAccion("GestionEmpresaAdministracion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion


        #region "util"

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

        #endregion

        #region "Cotizador"

        public void grdCustomErrorText(object sender, Exception ex)
        {
            var innerMessage = ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            var exx = new ASPxGridViewCustomErrorTextEventArgs(ex, GridErrorTextKind.General, innerMessage);
            //grdCustomErrorText(sender, exx);
        }


        protected void grdCustomMessageText(object sender, string msg)
        {
            var grilla = sender as ASPxGridView;
            if (grilla != null)
            {
                if (!grilla.JSProperties.ContainsKey("cpMessage"))
                    grilla.JSProperties.Add("cpMessage", msg);
            }
        }

        public string ValidarMandatarioJudicial(int idEmpresa, int idOperacion, string descEmpresa)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@descEmpresa", SqlDbType.NVarChar);
                SqlParametros[2].Value = descEmpresa;

                return AD_Global.traerPrimeraColumna("ValidarMandatarioJudicial", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "";
            }
        }

        System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        string html = string.Empty;
        public byte[] GenerarReporteCFC(int IdCotizacion, string sp)
        {
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

                if(sp == "ListarDetalleCFT")
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

        public DataSet CargarCotizacion(int IdCotizacion)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@IdCotizacion", IdCotizacion);

                return AD_Global.ejecutarConsulta("CargarCotizacion", sqlParam);
            }
            catch
            {
                return null;
            }
        }

        public DataTable ListaEmpresaByRut(int rut)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[1];
                sqlParam[0] = new SqlParameter("@Rut", rut);
                return AD_Global.ejecutarConsultas("api_EmpresasByRut", sqlParam);
            }
            catch
            {
                return new DataTable();
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

        public DataTable EmpresaAcreedora(int IdOpcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[0].Value = IdOpcion;

                dt = AD_Global.ejecutarConsultas("ActualizarEmpresaSAGR", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion
    }
}
