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
using System.Xml;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using MultigestionUtilidades;

namespace MultiAdministracion
{
    class LogicaNegocio
    {
        public string buscarMensaje(int idmensaje)
        {
            string mensaje = string.Empty;
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Mensajes"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + idmensaje + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    mensaje = oListItem["Mensaje"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return mensaje;
        }

        public DataTable ListarAdministracionxEmpresa(int idEmpresa)
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

        public DataTable BuscarAdministracionxRut(int idEmpresa, string rut)
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
                dt = AD_Global.ejecutarConsultas("GestionEmpresaAdministracion", SqlParametros);
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
                dt = AD_Global.ejecutarConsultas("BuscarDatosRut", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);

                return new DataTable();
            }
        }


        #region "wpListarBitacoraPago"

        public DataSet ListarBitacoraPago(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
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

                dt = AD_Global.ejecutarConsulta("ListarBitacoraPago", SqlParametros);

                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                //log.Error("Error metod Buscar" + ex.ToString());
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

        #region "wpListarContabilidad"

        public DataSet ListarContabilidad(string Perfil, string Usuario, string IdTransaccion, int IdSubEtapa, string IdNro,
           string Rut, string RazonSocial, string NCertificado, string fechaInicio, string fechaFin, int pageS, int pageN)
        {

            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[13];

                SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                SqlParametros[1] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[1].Value = Perfil;

                SqlParametros[2] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = Usuario;

                SqlParametros[3] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[3].Value = pageS;

                SqlParametros[4] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[4].Value = pageN;

                SqlParametros[5] = new SqlParameter("@IdTransaccion", SqlDbType.NVarChar);
                SqlParametros[5].Value = IdTransaccion;

                SqlParametros[6] = new SqlParameter("@IdSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = IdSubEtapa;

                SqlParametros[7] = new SqlParameter("@IdNro", SqlDbType.NVarChar);
                SqlParametros[7].Value = IdNro;

                SqlParametros[8] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[8].Value = Rut;

                SqlParametros[9] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[9].Value = RazonSocial;

                SqlParametros[10] = new SqlParameter("@NCertificado", SqlDbType.NVarChar);
                SqlParametros[10].Value = NCertificado;

                SqlParametros[11] = new SqlParameter("@fechaInicio", SqlDbType.NVarChar);
                SqlParametros[11].Value = fechaInicio;

                SqlParametros[12] = new SqlParameter("@fechaFin", SqlDbType.NVarChar);
                SqlParametros[12].Value = fechaFin;

                dt = AD_Global.ejecutarConsulta("ListarContabilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

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

        #region "wpListarContabilidadHistorico"

        public DataSet ListarContabilidadHistorial(string Perfil, string Usuario, string IdTransaccion, int IdSubEtapa, string IdNro,
        string Rut, string RazonSocial, string NCertificado, string fechaInicio, string fechaFin, int pageS, int pageN)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[13];

                SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                SqlParametros[1] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[1].Value = Perfil;

                SqlParametros[2] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = Usuario;

                SqlParametros[3] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[3].Value = pageS;

                SqlParametros[4] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[4].Value = pageN;

                SqlParametros[5] = new SqlParameter("@IdTransaccion", SqlDbType.NVarChar);
                SqlParametros[5].Value = IdTransaccion;

                SqlParametros[6] = new SqlParameter("@IdSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = IdSubEtapa;

                SqlParametros[7] = new SqlParameter("@IdNro", SqlDbType.NVarChar);
                SqlParametros[7].Value = IdNro;

                SqlParametros[8] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[8].Value = Rut;

                SqlParametros[9] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[9].Value = RazonSocial;

                SqlParametros[10] = new SqlParameter("@NCertificado", SqlDbType.NVarChar);
                SqlParametros[10].Value = NCertificado;

                SqlParametros[11] = new SqlParameter("@fechaInicio", SqlDbType.NVarChar);
                SqlParametros[11].Value = fechaInicio;

                SqlParametros[12] = new SqlParameter("@fechaFin", SqlDbType.NVarChar);
                SqlParametros[12].Value = fechaFin;

                dt = AD_Global.ejecutarConsulta("ListarContabilidaHistorial", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        //#region "wpPlantillaDocumento"


        //public int? InsertarPlantilla(MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla plantilla)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[8];

        //        SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
        //        SqlParametros[0].Value = plantilla.IdPlantillaDocumento;
        //        //SqlParametros[0].Direction = ParameterDirection.Output;

        //        SqlParametros[1] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
        //        SqlParametros[1].Value = plantilla.IdTipoProducto;

        //        SqlParametros[2] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
        //        SqlParametros[2].Value = plantilla.IdDocumentoTipo;

        //        SqlParametros[3] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
        //        SqlParametros[3].Value = plantilla.IdAcreedor;

        //        SqlParametros[4] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
        //        SqlParametros[4].Value = plantilla.NombrePlantilla;

        //        SqlParametros[5] = new SqlParameter("@ContenidoHtml", SqlDbType.NVarChar);
        //        SqlParametros[5].Value = plantilla.ContenidoHtml;

        //        SqlParametros[6] = new SqlParameter("@EsGenerica", SqlDbType.Bit);
        //        SqlParametros[6].Value = plantilla.EsGenerica;

        //        SqlParametros[7] = new SqlParameter("@UsuarioModificacion", SqlDbType.NVarChar);
        //        SqlParametros[7].Value = plantilla.UsuarioModificacion;

        //        return AD_Global.ejecutarInsert("GuardarDocumento", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return null;
        //    }
        //}

        //public int? VerificarPlantilla(MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla plantilla)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[8];

        //        SqlParametros[0] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
        //        SqlParametros[0].Value = plantilla.IdTipoProducto;

        //        SqlParametros[1] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
        //        SqlParametros[1].Value = plantilla.IdDocumentoTipo;

        //        SqlParametros[2] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
        //        SqlParametros[2].Value = plantilla.IdAcreedor;

        //        return AD_Global.ejecutarInsert("GuardarDocumento", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return null;
        //    }
        //}

        //public int? ActualizarPlantilla(MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla plantilla)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[8];

        //        SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
        //        SqlParametros[0].Value = plantilla.IdPlantillaDocumento;

        //        SqlParametros[1] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
        //        SqlParametros[1].Value = plantilla.NombrePlantilla;

        //        SqlParametros[2] = new SqlParameter("@ContenidoHtml", SqlDbType.NVarChar);
        //        SqlParametros[2].Value = plantilla.ContenidoHtml;

        //        SqlParametros[3] = new SqlParameter("@EsGenerica", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = plantilla.EsGenerica;

        //        SqlParametros[4] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
        //        SqlParametros[4].Value = plantilla.IdDocumentoTipo;

        //        SqlParametros[5] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
        //        SqlParametros[5].Value = plantilla.IdAcreedor;

        //        SqlParametros[6] = new SqlParameter("@UsuarioModificacion", SqlDbType.NVarChar);
        //        SqlParametros[6].Value = plantilla.UsuarioModificacion;

        //        SqlParametros[7] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
        //        SqlParametros[7].Value = plantilla.IdTipoProducto;

        //        return AD_Global.ejecutarUpdate("ActualizarDocumento", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return null;
        //    }
        //}

        //public DataTable ListarPlantillas(MultiAdministracion.wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla plantilla)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[5];

        //        SqlParametros[0] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
        //        SqlParametros[0].Value = plantilla.IdTipoProducto;

        //        SqlParametros[1] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
        //        SqlParametros[1].Value = plantilla.IdDocumentoTipo;

        //        SqlParametros[2] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
        //        SqlParametros[2].Value = plantilla.IdAcreedor;

        //        SqlParametros[3] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = plantilla.NombrePlantilla;

        //        SqlParametros[4] = new SqlParameter("@EsGenerica", SqlDbType.Bit);
        //        SqlParametros[4].Value = plantilla.EsGenerica;

        //        return AD_Global.ejecutarConsultas("ListarPlantillas", SqlParametros);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public DataTable ListarTodasPlantillas(wpPlantillaDocumento.wpPlantillaDocumento.LNplantilla plantilla)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[4];

        //        SqlParametros[0] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
        //        SqlParametros[0].Value = plantilla.IdTipoProducto;

        //        SqlParametros[1] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
        //        SqlParametros[1].Value = plantilla.IdDocumentoTipo;

        //        SqlParametros[2] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
        //        SqlParametros[2].Value = plantilla.IdAcreedor;

        //        SqlParametros[3] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = plantilla.NombrePlantilla;

        //        return AD_Global.ejecutarConsultas("ListarTodasPlantillas", SqlParametros);
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

        //public bool EliminarPlantilla(int IdPlantilla, string usuario)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[2];
        //        SqlParametros[0] = new SqlParameter("@IdPlantilla", SqlDbType.Int);
        //        SqlParametros[0].Value = IdPlantilla;
        //        SqlParametros[1] = new SqlParameter("@usuario", SqlDbType.NChar);
        //        SqlParametros[1].Value = usuario;
        //        return AD_Global.ejecutarAccion("EliminarPlantilla", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        //public DataTable ListarPlantillaById(int IdPlantillaDocumento)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[1];

        //        SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
        //        SqlParametros[0].Value = IdPlantillaDocumento;

        //        dt = AD_Global.ejecutarConsultas("ListarPlantillaById", SqlParametros);
        //        return dt;

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}

        //#endregion

        #region "wpBitacoraComentarios"

        public Boolean GuardarCompromiso(int idOperacion, int idEmpresa, string ejecutivo, string area, string comentario, string usuario, string etapa, string subEtapa, DateTime? fechaCierre, int Prioridad)
        {
            try
            {
                bool dt;
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@IDOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                SqlParametros[1] = new SqlParameter("@IDEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                SqlParametros[2] = new SqlParameter("@Ejecutivo", SqlDbType.VarChar);
                SqlParametros[2].Value = ejecutivo;
                SqlParametros[3] = new SqlParameter("@Area", SqlDbType.VarChar);
                SqlParametros[3].Value = area;
                SqlParametros[4] = new SqlParameter("@Comentario", SqlDbType.VarChar);
                SqlParametros[4].Value = comentario;
                SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.VarChar);
                SqlParametros[5].Value = usuario;
                SqlParametros[6] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = etapa;
                SqlParametros[7] = new SqlParameter("@SubEtapa", SqlDbType.NVarChar);
                SqlParametros[7].Value = subEtapa;
                SqlParametros[8] = new SqlParameter("@fechaCierre", SqlDbType.DateTime);
                SqlParametros[8].Value = fechaCierre;
                SqlParametros[9] = new SqlParameter("@Prioridad", SqlDbType.Int);
                SqlParametros[9].Value = Prioridad;

                dt = AD_Global.ejecutarAccion("GuardarCompromiso", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpCompromisos"

        public DataSet ReporteCompromisos(string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IDOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                dt = AD_Global.ejecutarConsulta("ReporteCompromisos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarCompromisos(string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IDOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                dt = AD_Global.ejecutarConsulta("ListarCompromisos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable ListarComentarios(string idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IDOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                dt = AD_Global.ejecutarConsultas("ListaComentarios", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpSeguimientoCompromisos"

        public Boolean GuardarPipeline(string usuario, DataTable dt)
        {
            try
            {
                string xmlData = null;
                xmlData = generarXML(dt);

                if (!string.IsNullOrEmpty(xmlData))
                {
                    bool ok;
                    SqlParameter[] SqlParametros = new SqlParameter[2];
                    SqlParametros[0] = new SqlParameter("@Usuario", SqlDbType.VarChar);
                    SqlParametros[0].Value = usuario;
                    SqlParametros[1] = new SqlParameter("@xmlData", SqlDbType.VarChar);
                    SqlParametros[1].Value = xmlData;

                    ok = AD_Global.ejecutarAccion("GuardarPipeline", SqlParametros);
                    return ok;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string generarXML(DataTable dt)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode ValoresNode = doc.CreateElement("Valores");
            doc.AppendChild(ValoresNode);
            XmlNode RespNode;
            XmlNode root = doc.DocumentElement;

            foreach (DataRow row in dt.Rows)
            {
                RespNode = doc.CreateElement("Val");

                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column] != null)
                    {
                        var field1 = column.ColumnName; //row[column].ToString();
                        var text = row[column].ToString();

                        var t = row[column].GetType();
                        //if (row[column].GetType() == typeof(int))
                        var value = string.Empty;
                        if (column.ColumnName == "PorcComision")
                            value = row[column].ToString().Replace(",", ".");
                        else
                            value = row[column].ToString().Replace(".", "");

                        XmlNode nodo0 = doc.CreateElement(field1);
                        nodo0.AppendChild(doc.CreateTextNode(System.Web.HttpUtility.HtmlDecode(value)));
                        RespNode.AppendChild(nodo0);

                        ValoresNode.AppendChild(RespNode);
                    }
                }
            }
            return doc.OuterXml;
        }

        public DataSet ResporteSeguimientoSemanal(string mes, string etapa, string ejecutivo, string empresa, string canal, string fondo)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@Mes", SqlDbType.VarChar);
                SqlParametros[0].Value = mes;
                SqlParametros[1] = new SqlParameter("@Etapa", SqlDbType.VarChar);
                SqlParametros[1].Value = etapa;
                SqlParametros[2] = new SqlParameter("@Ejecutivo", SqlDbType.VarChar);
                SqlParametros[2].Value = ejecutivo;
                SqlParametros[3] = new SqlParameter("@Empresa", SqlDbType.VarChar);
                SqlParametros[3].Value = empresa;
                SqlParametros[4] = new SqlParameter("@Canal", SqlDbType.VarChar);
                SqlParametros[4].Value = canal;
                SqlParametros[5] = new SqlParameter("@Fondo", SqlDbType.VarChar);
                SqlParametros[5].Value = fondo;
                dt = AD_Global.ejecutarConsulta("ReporteSeguimientoSemanal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable ResumenPipeline(DateTime fechaHoy)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@fechaHoy", SqlDbType.DateTime);
                SqlParametros[0].Value = fechaHoy;

                dt = AD_Global.ejecutarConsultas("ResumenPipeline", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarSeguimientoCompromisos(string mes, string etapa, string ejecutivo, string empresa, string canal, string fondo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@Mes", SqlDbType.VarChar);
                SqlParametros[0].Value = mes;
                SqlParametros[1] = new SqlParameter("@Etapa", SqlDbType.VarChar);
                SqlParametros[1].Value = etapa;
                SqlParametros[2] = new SqlParameter("@Ejecutivo", SqlDbType.VarChar);
                SqlParametros[2].Value = ejecutivo;
                SqlParametros[3] = new SqlParameter("@Empresa", SqlDbType.VarChar);
                SqlParametros[3].Value = empresa;
                SqlParametros[4] = new SqlParameter("@Canal", SqlDbType.VarChar);
                SqlParametros[4].Value = canal;
                SqlParametros[5] = new SqlParameter("@Fondo", SqlDbType.VarChar);
                SqlParametros[5].Value = fondo;
                dt = AD_Global.ejecutarConsultas("ListaSeguimientoSemanal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean GuardarTodoComentarios(string usuario, string xmlData)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];

                SqlParametros[0] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[0].Value = xmlData;
                SqlParametros[1] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;

                return AD_Global.ejecutarAccion("GuardarTodoComentarios", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpPerfiles"


        public DataTable ListarDepartamentos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarDepartamentos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public void ListarBancos(ref DropDownList combo)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Bancos"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "";
                combo.DataSource = Lista.GetItems(oQuery).GetDataTable();
                combo.DataTextField = Lista.GetItems(oQuery).Fields["Nombre"].ToString();
                combo.DataValueField = Lista.GetItems(oQuery).Fields["Codigo"].ToString();
                combo.DataBind();
                combo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione--", "-1"));
                combo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }


        public void ListarCustomList(ref DropDownList combo, string List, string fieldFind, string txtFind)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists[List];
                SPQuery oQuery = new SPQuery();
                string sQuery = "<Where><Eq><FieldRef Name='" + fieldFind + "'/><Value Type='Text'>" + txtFind + "</Value></Eq></Where>";
                oQuery.Query = (txtFind == "") ? "" : sQuery;
                combo.DataSource = Lista.GetItems(oQuery).GetDataTable();
                combo.DataTextField = Lista.GetItems(oQuery).Fields["Nombre"].ToString();
                combo.DataValueField = Lista.GetItems(oQuery).Fields["ID"].ToString();
                combo.DataBind();
                combo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Seleccione--", "-1"));
                combo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
        }


        public DataTable ListarPermisos(string modulo, string opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@modulo", SqlDbType.VarChar);
                SqlParametros[0].Value = modulo;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.VarChar);
                SqlParametros[1].Value = opcion;
                dt = AD_Global.ejecutarConsultas("sp_permisos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoOpciones(string modulo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@modulo", SqlDbType.VarChar);
                SqlParametros[0].Value = modulo;
                dt = AD_Global.ejecutarConsultas("sp_tipoOpciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCargos(int cargo)
        {
            DataTable dt = new DataTable();
            SqlParameter[] SqlParametros = new SqlParameter[1];
            SqlParametros[0] = new SqlParameter("@Cargo", SqlDbType.Int);
            SqlParametros[0].Value = cargo;
            dt = AD_Global.ejecutarConsultas("sp_cargos", SqlParametros);
            return dt;
        }

        public DataTable ListarUsuarios(int tipoOperacion, int ID, string nombreApellido, string Rut, int idDepartamento, string descDepartamento, int idBanco, string descBanco, string numeroCuenta, int idCargo, string descCargo, bool habilitado, string Usuario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[13];
                SqlParametros[0] = new SqlParameter("@CRUD", SqlDbType.Int);
                SqlParametros[0].Value = tipoOperacion;
                SqlParametros[1] = new SqlParameter("@ID", SqlDbType.Int);
                SqlParametros[1].Value = ID;

                SqlParametros[2] = new SqlParameter("@nombreApellido", SqlDbType.NVarChar);
                SqlParametros[2].Value = nombreApellido;
                SqlParametros[3] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[3].Value = Rut;

                SqlParametros[4] = new SqlParameter("@idDepartamento", SqlDbType.Int);
                SqlParametros[4].Value = idDepartamento;
                SqlParametros[5] = new SqlParameter("@descDepartamento", SqlDbType.NVarChar);
                SqlParametros[5].Value = descDepartamento;

                SqlParametros[6] = new SqlParameter("@idBanco", SqlDbType.Int);
                SqlParametros[6].Value = idBanco;
                SqlParametros[7] = new SqlParameter("@descBanco", SqlDbType.NVarChar);
                SqlParametros[7].Value = descBanco;

                SqlParametros[8] = new SqlParameter("@numeroCuenta", SqlDbType.NVarChar);
                SqlParametros[8].Value = numeroCuenta;
                SqlParametros[9] = new SqlParameter("@idCargo", SqlDbType.Int);
                SqlParametros[9].Value = idCargo;

                SqlParametros[10] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[10].Value = descCargo;
                SqlParametros[11] = new SqlParameter("@habilitado", SqlDbType.Bit);
                SqlParametros[11].Value = habilitado;
                SqlParametros[12] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[12].Value = Usuario;

                dt = AD_Global.ejecutarConsultas("sp_usuarios", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPerfiles()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultaV2("sp_usuarios", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable GetUsuarioPorId(int IdUsuario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdUsuario", SqlDbType.Int);
                SqlParametros[0].Value = IdUsuario;

                dt = AD_Global.ejecutarConsultas("GetUsuarioPorId", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        //public DataTable ListarViewUsuarios(int ID)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        AD_Global objDatos = new AD_Global();
        //        SqlParameter[] SqlParametros = new SqlParameter[2];
        //        SqlParametros[0] = new SqlParameter("@CRUD", SqlDbType.Int);
        //        SqlParametros[1] = new SqlParameter("@ID", SqlDbType.Int);
        //        SqlParametros[0].Value = 2;
        //        SqlParametros[1].Value = ID;
        //        dt = AD_Global.ejecutarConsultas("sp_usuarios", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}

        public DataTable ListarOpciones(int idTipoOpcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idTipoOpcion", SqlDbType.Int);
                SqlParametros[0].Value = idTipoOpcion;
                dt = AD_Global.ejecutarConsultas("sp_opciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPerfiles(int idUser)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idUser", SqlDbType.Int);
                SqlParametros[0].Value = idUser;
                dt = AD_Global.ejecutarConsultas("sp_perfiles", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        public Boolean GuardarPerfil(string Perfil, int IdOpcion, int IdPermiso, string Modulo, bool Habilitado, int Crud, int IdCargoAccion)
        {
            try
            {
                bool dt;
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@Perfil", SqlDbType.VarChar);
                SqlParametros[0].Value = Perfil;
                //SqlParametros[1] = new SqlParameter("@idTipoOpciones", SqlDbType.Int);
                //SqlParametros[1].Value = idTipoOpciones;
                SqlParametros[1] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[1].Value = IdOpcion;
                SqlParametros[2] = new SqlParameter("@IdPermiso", SqlDbType.Int);
                SqlParametros[2].Value = IdPermiso;
                SqlParametros[3] = new SqlParameter("@Modulo", SqlDbType.VarChar);
                SqlParametros[3].Value = Modulo;
                SqlParametros[4] = new SqlParameter("@Habilitado", SqlDbType.Bit);
                SqlParametros[4].Value = Habilitado;
                SqlParametros[5] = new SqlParameter("@Crud", SqlDbType.Int);
                SqlParametros[5].Value = Crud;
                SqlParametros[6] = new SqlParameter("@IdCargoAccion", SqlDbType.Int);
                SqlParametros[6].Value = IdCargoAccion;
                dt = AD_Global.ejecutarAccion("GuardarPerfil", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public Boolean GuardarUsuario(int CRUD, int ID, string nombreApellido, string Rut, int idDepartamento, string descDepartamento, string idBanco, string descBanco, string numeroCuenta, int idCargo, string descCargo, string UsuarioCreacion, bool habilitado, bool EsResponsable)
        {
            try
            {
                bool dt;
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@CRUD", SqlDbType.Int);
                SqlParametros[0].Value = CRUD;
                SqlParametros[1] = new SqlParameter("@ID", SqlDbType.Int);
                SqlParametros[1].Value = ID;
                SqlParametros[2] = new SqlParameter("@nombreApellido", SqlDbType.VarChar);
                SqlParametros[2].Value = nombreApellido;
                SqlParametros[3] = new SqlParameter("@Rut", SqlDbType.VarChar);
                SqlParametros[3].Value = Rut;
                SqlParametros[4] = new SqlParameter("@idDepartamento", SqlDbType.Int);
                SqlParametros[4].Value = idDepartamento;
                SqlParametros[5] = new SqlParameter("@descDepartamento", SqlDbType.VarChar);
                SqlParametros[5].Value = descDepartamento;
                SqlParametros[6] = new SqlParameter("@idBanco", SqlDbType.VarChar);
                SqlParametros[6].Value = idBanco;
                SqlParametros[7] = new SqlParameter("@descBanco", SqlDbType.VarChar);
                SqlParametros[7].Value = descBanco;
                SqlParametros[8] = new SqlParameter("@numeroCuenta", SqlDbType.VarChar);
                SqlParametros[8].Value = numeroCuenta;
                SqlParametros[9] = new SqlParameter("@idCargo", SqlDbType.Int);
                SqlParametros[9].Value = idCargo;
                SqlParametros[10] = new SqlParameter("@descCargo", SqlDbType.VarChar);
                SqlParametros[10].Value = descCargo;
                SqlParametros[11] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[11].Value = UsuarioCreacion;
                SqlParametros[12] = new SqlParameter("@habilitado", SqlDbType.Bit);
                SqlParametros[12].Value = habilitado;
                SqlParametros[13] = new SqlParameter("@EsResponsable", SqlDbType.Bit);
                SqlParametros[13].Value = EsResponsable;

                dt = AD_Global.ejecutarAccion("sp_usuarios", SqlParametros);
                return dt;
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

        #endregion

    }
}
