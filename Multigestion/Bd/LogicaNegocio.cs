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
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.DataVisualization.Charting;
using MultigestionUtilidades;


namespace Bd
{
    public class LogicaNegocio
    {
        public LogicaNegocio()
        {

        }

        public string buscarMensaje(int idmensaje)
        {
            string mensaje = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                SqlParametros[0].Value = idmensaje;
                dt = AD_Global.ejecutarConsultas("ListarMensajes", SqlParametros);
                return dt.Rows[0]["Mensaje"].ToString();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return mensaje;
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

        #region "wpPerfiles"

        public DataTable ListarBancos(string codigo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@codigo", SqlDbType.NVarChar);
                SqlParametros[0].Value = codigo;
                dt = AD_Global.ejecutarConsultas("ListarBancos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPermisos(int habilitado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@habilitado", SqlDbType.Int);
                SqlParametros[0].Value = habilitado;
                dt = AD_Global.ejecutarConsultas("sp_permisos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }
        public DataTable ListarTipoOpciones()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@modulo", SqlDbType.Int);
                SqlParametros[0].Value = 1;
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
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Cargo", SqlDbType.Int);
                SqlParametros[0].Value = cargo;
                dt = AD_Global.ejecutarConsultas("sp_cargos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoDocumentoContable()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@Cargo", SqlDbType.Int);
                //SqlParametros[0].Value = cargo;
                dt = AD_Global.ejecutarConsultas("ListarTipoDocumentoContable", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarUsuarios(int tipoOperacion, int ID, string nombreApellido, string Rut, int idDepartamento, string descDepartamento, int idBanco, string descBanco, string numeroCuenta, int idCargo, string descCargo, bool habilitado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[12];
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

                dt = AD_Global.ejecutarConsultas("sp_usuarios", SqlParametros);
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



        public Boolean GuardarPerfil(string Descripcion, int idTipoOpciones, int idOpciones, int idPermiso, int idModulo)
        {
            try
            {
                bool dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@Descripcion", SqlDbType.VarChar);
                SqlParametros[0].Value = Descripcion;
                SqlParametros[1] = new SqlParameter("@idTipoOpciones", SqlDbType.Int);
                SqlParametros[1].Value = idTipoOpciones;
                SqlParametros[2] = new SqlParameter("@idOpciones", SqlDbType.Int);
                SqlParametros[2].Value = idOpciones;
                SqlParametros[3] = new SqlParameter("@idPermiso", SqlDbType.Int);
                SqlParametros[3].Value = idPermiso;
                SqlParametros[4] = new SqlParameter("@idModulo", SqlDbType.Int);
                SqlParametros[4].Value = idModulo;
                dt = AD_Global.ejecutarAccion("GuardarPerfil", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public Boolean GuardarUsuario(int CRUD, int ID, string nombreApellido, string Rut, int idDepartamento, string descDepartamento, string idBanco, string descBanco, string numeroCuenta, int idCargo, string descCargo, bool habilitado)
        {
            try
            {
                bool dt;
                SqlParameter[] SqlParametros = new SqlParameter[12];
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
                SqlParametros[11] = new SqlParameter("@habilitado", SqlDbType.Bit);
                SqlParametros[11].Value = habilitado;
                dt = AD_Global.ejecutarAccion("sp_usuarios", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }
        #endregion


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

        public DataTable ListarContratantes(int Id)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                SqlParametros[0].Value = Id;
                dt = AD_Global.ejecutarConsultas("Contratante", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoSeguro(int Id)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                SqlParametros[0].Value = Id;
                dt = AD_Global.ejecutarConsultas("SeguroEstado", SqlParametros);
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

        public DataSet ReporteCompromisos(int idOperacion)
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

        public Boolean GuardarPipeline(string usuario, string xmlData)
        {
            try
            {
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

        public DataTable ListarTipoTransaccion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarTipoTransaccion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoContabilidad()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarEstadoContabilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
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

        public DataTable ListarPerfilesV2()
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


        //public DataTable ListarPerfiles(int idUser)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[1];
        //        SqlParametros[0] = new SqlParameter("@idUser", SqlDbType.Int);
        //        SqlParametros[0].Value = idUser;
        //        dt = AD_Global.ejecutarConsultas("sp_perfiles", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}


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

        public DataTable ListarFinalidad(int IdProducto)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                SqlParametros[0].Value = IdProducto;
                dt = AD_Global.ejecutarConsultas("ListarFinalidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPeriocidad()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                //SqlParametros[0].Value = IdProducto;
                dt = AD_Global.ejecutarConsultas("ListarPeriocidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoCertificado()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                //SqlParametros[0].Value = IdProducto;
                dt = AD_Global.ejecutarConsultas("ListarEstadoCertificado", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoCuota()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                //SqlParametros[0].Value = IdProducto;
                dt = AD_Global.ejecutarConsultas("ListarTipoCuota", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarMonedas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarMonedas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        public DataTable ListarProducto(int IdOpcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[0].Value = IdOpcion;
                dt = AD_Global.ejecutarConsultas("ListarProducto", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoCredito(int? IdProducto)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
                SqlParametros[0].Value = IdProducto;
                dt = AD_Global.ejecutarConsultas("ListarTipoCredito", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarPeriocidadDePago()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                //SqlParametros[0].Value = IdOpcion;
                dt = AD_Global.ejecutarConsultas("ListarPeriocidadDePago", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCanal(int? IdTipoCanal)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdTipoCanal", SqlDbType.Int);
                SqlParametros[0].Value = IdTipoCanal;
                dt = AD_Global.ejecutarConsultas("ListarCanal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoAmortizacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarTipoAmortizacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }



        public DataTable buscarDatosRutV2(string rut)
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
            if (Esworkflow)
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
            catch (Exception ex)
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
                return new DataSet();
            }
        }

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

        public DataTable ListarCompaniaSeguro()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarCompaniaSeguro", SqlParametros);
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

        public Boolean ModificarDireccionxEmpresa(string idDireccionEmpresa, string idEmpresa, string idTipo, string descTipo, bool principal, string direccion, string idRegion, string descRegion,
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
                if (xml == null)
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




        public string TraeNombreEjecutivo(int idEmpresa)
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

        public string clienteBloquedo(int idEmpresa)
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

        public DataSet TraeOperaciones(int idEmpresa, bool habilitado, string area)
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

        public Boolean ModificarOperacion(int idEmpresa, int IdOperacion, string xmlData, string usuario, string perfil, string opcion, string glosaComercial, string instruccionCurse, string observacion, string destinoSolicitud, string comentarios)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[9];

                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = opcion;

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

        public DataTable ValidarFondos(string idEmpresa, string idOperacion, string idPaf, string user, string perfil, DataTable datos, float costoFondos, int idFondo)
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
                SqlParametros[2].Value = Convert.ToString(datos.Rows[0]["Porcentaje"]).GetValorDouble();
                SqlParametros[3] = new SqlParameter("@FechaVencimiento", SqlDbType.NVarChar);
                SqlParametros[3].Value = datos.Rows[0]["FechaVencimiento"];
                SqlParametros[4] = new SqlParameter("@CostoFondo", SqlDbType.Float);
                SqlParametros[4].Value = datos.Rows[0]["CostoFondo"].ToString().GetValorDouble();
                SqlParametros[5] = new SqlParameter("@VentasUFMax", SqlDbType.Float);
                SqlParametros[5].Value = datos.Rows[0]["VentasUFMax"].ToString().GetValorDouble();
                SqlParametros[6] = new SqlParameter("@GarantiasCoberturaMin", SqlDbType.Float);
                SqlParametros[6].Value = datos.Rows[0]["GarantiasCoberturaMin"].ToString().GetValorDouble();
                SqlParametros[7] = new SqlParameter("@ValorFondo", SqlDbType.Float);
                SqlParametros[7].Value = datos.Rows[0]["ValorFondo"].ToString().GetValorDouble();
                SqlParametros[8] = new SqlParameter("@CostoFondoIngresado", SqlDbType.Float);
                SqlParametros[8].Value = costoFondos;
                SqlParametros[9] = new SqlParameter("@Fondo", SqlDbType.Float);
                SqlParametros[9].Value = idFondo;

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

        public DataTable ListarFondos(int? IdFondo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarFondos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoDocumentos(int? IdArea, int? IdTipoProducto)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@IdArea", SqlDbType.Int);
                SqlParametros[0].Value = IdArea;
                SqlParametros[1] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
                SqlParametros[1].Value = IdTipoProducto;

                dt = AD_Global.ejecutarConsultas("ListarTipoDocumentos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarAreas(int? IdArea)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                //SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarAreas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoVerificacionDireccion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarEstadoVerificacionDireccion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEmpresaTasadora()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarEmpresaTasadora", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarRegimen(int? IdEdoCivil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEdoCivil", SqlDbType.Int);
                SqlParametros[0].Value = IdEdoCivil;

                dt = AD_Global.ejecutarConsultas("ListarRegimen", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarLimite()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarLimite", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCaracter()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarCaracter", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadosGarantia()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarEstadosGarantia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarSubEstadosGarantia()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarSubEstadosGarantia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoCivil()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarEstadoCivil", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarParticipacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarParticipacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarGradoPreferencia()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarGradoPreferencia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTiposGarantia()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarTiposGarantia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTiposBienes(int? IdGarantia, int? IdTipoBienes)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[0].Value = IdGarantia;
                SqlParametros[1] = new SqlParameter("@IdTipoBienes", SqlDbType.Int);
                SqlParametros[1].Value = IdTipoBienes;
                dt = AD_Global.ejecutarConsultas("ListarTiposBienes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        public DataTable ListarGrupoEconomico()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarGrupoEconomico", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarActividadEconomica()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarActividadEconomica", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoDireccion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarTipoDireccion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCargosPersonas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarCargosPersonas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTiposEmpresa()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarTiposEmpresa", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarMotivoBloqueo()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarMotivoBloqueo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoCredito()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                //SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarEstadoCredito", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoOperacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                //SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarTipoOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarTipoObligacionAfianzada()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                //SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarTipoObligacionAfianzada", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadoPagare()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                //SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarEstadoPagare", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarMotivoFogapeVigente()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                //SqlParametros[0] = new SqlParameter("@IdFondo", SqlDbType.Int);
                //SqlParametros[0].Value = IdFondo;

                dt = AD_Global.ejecutarConsultas("ListarMotivoFogapeVigente", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }




        #endregion

        #region "wpAdministracion"

        //public DataTable ListarAdministracionxEmpresa(int idEmpresa)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[2];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[1].Value = Constantes.OPCION.LISTAR;
        //        dt = AD_Global.ejecutarConsultas("GestionEmpresaAdministracion", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}

        //public bool EliminarAdministracionxEmpresa(int idEmpresaAdministracion, string usuario)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[3];
        //        SqlParametros[0] = new SqlParameter("@idEmpresaAdministracion", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresaAdministracion;
        //        SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[1].Value = Constantes.OPCION.ELIMINAR;
        //        SqlParametros[2] = new SqlParameter("@user", SqlDbType.NChar);
        //        SqlParametros[2].Value = usuario;
        //        return AD_Global.ejecutarAccion("GestionEmpresaAdministracion", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        //public bool ModificarAdministracionxEmpresa(int idEmpresaAdministracion, string nombre, string rut, string divRut, string profesion, string cargo, string idEdoCivil,
        //   string descEdoCivil, string fecNac, string antiguedad, string telefono, string mail, string usuario)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[14];
        //        SqlParametros[0] = new SqlParameter("@idEmpresaAdministracion", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresaAdministracion;
        //        SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[1].Value = Constantes.OPCION.MODIFICAR;
        //        SqlParametros[2] = new SqlParameter("@rut", SqlDbType.NChar);
        //        SqlParametros[2].Value = rut;
        //        SqlParametros[3] = new SqlParameter("@divrut", SqlDbType.NChar);
        //        SqlParametros[3].Value = divRut;
        //        SqlParametros[4] = new SqlParameter("@nombre", SqlDbType.NChar);
        //        SqlParametros[4].Value = nombre;
        //        SqlParametros[5] = new SqlParameter("@profesion", SqlDbType.NChar);
        //        SqlParametros[5].Value = profesion;
        //        SqlParametros[6] = new SqlParameter("@cargo", SqlDbType.NChar);
        //        SqlParametros[6].Value = cargo;
        //        SqlParametros[7] = new SqlParameter("@idEdoCivil", SqlDbType.NChar);
        //        SqlParametros[7].Value = idEdoCivil;
        //        SqlParametros[8] = new SqlParameter("@descEdoCivil", SqlDbType.NChar);
        //        SqlParametros[8].Value = descEdoCivil;
        //        SqlParametros[9] = new SqlParameter("@fecNacim", SqlDbType.NChar);
        //        SqlParametros[9].Value = fecNac;
        //        SqlParametros[10] = new SqlParameter("@antiguedad", SqlDbType.NChar);
        //        SqlParametros[10].Value = antiguedad;
        //        SqlParametros[11] = new SqlParameter("@telefono", SqlDbType.NChar);
        //        SqlParametros[11].Value = telefono;
        //        SqlParametros[12] = new SqlParameter("@mail", SqlDbType.NChar);
        //        SqlParametros[12].Value = mail;
        //        SqlParametros[13] = new SqlParameter("@user", SqlDbType.NChar);
        //        SqlParametros[13].Value = usuario;
        //        return AD_Global.ejecutarAccion("GestionEmpresaAdministracion", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        //public bool InsertarAdministracionxEmpresa(int idEmpresa, string nombre, string rut, string divRut, string profesion, string cargo, string idEdoCivil,
        //  string descEdoCivil, string fecNac, string antiguedad, string telefono, string mail, string usuario)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[14];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[1].Value = Constantes.OPCION.INSERTAR;
        //        SqlParametros[2] = new SqlParameter("@rut", SqlDbType.NChar);
        //        SqlParametros[2].Value = rut;
        //        SqlParametros[3] = new SqlParameter("@divrut", SqlDbType.NChar);
        //        SqlParametros[3].Value = divRut;
        //        SqlParametros[4] = new SqlParameter("@nombre", SqlDbType.NChar);
        //        SqlParametros[4].Value = nombre;
        //        SqlParametros[5] = new SqlParameter("@profesion", SqlDbType.NChar);
        //        SqlParametros[5].Value = profesion;
        //        SqlParametros[6] = new SqlParameter("@cargo", SqlDbType.NChar);
        //        SqlParametros[6].Value = cargo;
        //        SqlParametros[7] = new SqlParameter("@idEdoCivil", SqlDbType.NChar);
        //        SqlParametros[7].Value = idEdoCivil;
        //        SqlParametros[8] = new SqlParameter("@descEdoCivil", SqlDbType.NChar);
        //        SqlParametros[8].Value = descEdoCivil;
        //        SqlParametros[9] = new SqlParameter("@fecNacim", SqlDbType.NChar);
        //        SqlParametros[9].Value = fecNac;
        //        SqlParametros[10] = new SqlParameter("@antiguedad", SqlDbType.NChar);
        //        SqlParametros[10].Value = antiguedad;
        //        SqlParametros[11] = new SqlParameter("@telefono", SqlDbType.NChar);
        //        SqlParametros[11].Value = telefono;
        //        SqlParametros[12] = new SqlParameter("@mail", SqlDbType.NChar);
        //        SqlParametros[12].Value = mail;
        //        SqlParametros[13] = new SqlParameter("@user", SqlDbType.NChar);
        //        SqlParametros[13].Value = usuario;
        //        return AD_Global.ejecutarAccion("GestionEmpresaAdministracion", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        #endregion


        #region "util"

        //public Boolean GestionResponsableOperacion(int idOperacion, string usuario, string perfil, string opcion)
        //{
        //    /*OPCIONES:
        //     * 01 =  Riesgo
        //     * 02 =  Fiscalia
        //     * 03 =  Operacion
        //     * 04 =  Contabilidad
        //    */
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[4];
        //        SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[0].Value = idOperacion;
        //        SqlParametros[1] = new SqlParameter("@usuario", SqlDbType.NChar);
        //        SqlParametros[1].Value = usuario;
        //        SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NChar);
        //        SqlParametros[2].Value = perfil;
        //        SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = opcion;

        //        return AD_Global.ejecutarAccion("GestionResponsablesOperacion", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        #endregion

        #region "Cotizador"

        //public void grdCustomErrorText(object sender, Exception ex)
        //{
        //    var innerMessage = ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
        //    var exx = new ASPxGridViewCustomErrorTextEventArgs(ex, GridErrorTextKind.General, innerMessage);
        //    //grdCustomErrorText(sender, exx);
        //}


        //protected void grdCustomMessageText(object sender, string msg)
        //{
        //    var grilla = sender as ASPxGridView;
        //    if (grilla != null)
        //    {
        //        if (!grilla.JSProperties.ContainsKey("cpMessage"))
        //            grilla.JSProperties.Add("cpMessage", msg);
        //    }
        //}

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

        #region "wpContabilidad"


        public Boolean InsertarActualizarFacturacion(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string nroFactura, string FecFactura, string DescTipoTransaccion, string IdIVA, string DescIVA, string IdTipoDocumento, string DescTipoDocumento)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[12];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;
                SqlParametros[4] = new SqlParameter("@FecFactura", SqlDbType.NVarChar);
                SqlParametros[4].Value = FecFactura;
                SqlParametros[5] = new SqlParameter("@nroFactura", SqlDbType.NVarChar);
                SqlParametros[5].Value = nroFactura;
                SqlParametros[6] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[6].Value = "01";
                SqlParametros[7] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[7].Value = DescTipoTransaccion;

                SqlParametros[8] = new SqlParameter("@IdIVA", SqlDbType.Int);
                SqlParametros[8].Value = IdIVA;
                SqlParametros[9] = new SqlParameter("@DescIVA", SqlDbType.NVarChar);
                SqlParametros[9].Value = DescIVA;
                SqlParametros[10] = new SqlParameter("@IdTipoDocumento", SqlDbType.Int);
                SqlParametros[10].Value = IdTipoDocumento;
                SqlParametros[11] = new SqlParameter("@DescTipoDocumento", SqlDbType.NVarChar);
                SqlParametros[11].Value = DescTipoDocumento;

                dt = AD_Global.ejecutarAccion("GestionFacturacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        //   public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
        //string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
        //   {
        //       try
        //       {
        //           Boolean dt;
        //           SqlParameter[] SqlParametros = new SqlParameter[13];
        //           SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
        //           SqlParametros[0].Value = idEmpresa;
        //           SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
        //           SqlParametros[1].Value = idOperacion;
        //           SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
        //           SqlParametros[2].Value = "01";
        //           SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
        //           SqlParametros[3].Value = idUsuario;
        //           SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
        //           SqlParametros[4].Value = cargoUser;

        //           SqlParametros[5] = new SqlParameter("@idEstado", SqlDbType.Int);
        //           SqlParametros[5].Value = Estado;
        //           SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
        //           SqlParametros[6].Value = descEstado;

        //           SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
        //           SqlParametros[7].Value = Etapa;
        //           SqlParametros[8] = new SqlParameter("@descEtapa", SqlDbType.NVarChar);
        //           SqlParametros[8].Value = descEtapa;

        //           SqlParametros[9] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
        //           SqlParametros[9].Value = subEtapa;
        //           SqlParametros[10] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
        //           SqlParametros[10].Value = descsubEtapa;

        //           SqlParametros[11] = new SqlParameter("@Area", SqlDbType.NVarChar);
        //           SqlParametros[11].Value = Area;

        //           SqlParametros[12] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
        //           SqlParametros[12].Value = comentario;

        //           dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
        //           return dt;
        //       }
        //       catch (Exception ex)
        //       {
        //           LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //           return false;
        //       }
        //   }

        public DataTable ConsultaFacturacion(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string descTipoTransaccion)
        {
            try
            {
                DataTable dt;
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;

                SqlParametros[3] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;


                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[4].Value = "02";

                SqlParametros[5] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[5].Value = descTipoTransaccion;

                dt = AD_Global.ejecutarConsultas("GestionFacturacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
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

        public DataSet ConsultaDatosContabilidadXML(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string descTipoTransaccion)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;

                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;

                SqlParametros[4] = new SqlParameter("@descTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[4].Value = descTipoTransaccion;

                dt = AD_Global.ejecutarConsulta("ConsultarDatosContabilidadXML", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ConsultaDatosContabilidad(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string descTipoTransaccion)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;

                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;

                SqlParametros[4] = new SqlParameter("@descTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[4].Value = descTipoTransaccion;

                dt = AD_Global.ejecutarConsulta("ConsultarDatosContabilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpListarContabilidad"

        public DataSet ListarContabilidad(string Perfil, string Etapa, string Usuario, string IdTransaccion, int IdSubEtapa, string IdNro,
           string Rut, string RazonSocial, string NCertificado, string fechaInicio, string fechaFin, int pageS, int pageN)
        {

            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[14];

                SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                SqlParametros[1] = new SqlParameter("@Etapa", SqlDbType.NChar);
                SqlParametros[1].Value = Etapa;

                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = Perfil;

                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Usuario;

                SqlParametros[4] = new SqlParameter("@PageSize", SqlDbType.Int);
                SqlParametros[4].Value = pageS;

                SqlParametros[5] = new SqlParameter("@PageNumber", SqlDbType.Int);
                SqlParametros[5].Value = pageN;

                SqlParametros[6] = new SqlParameter("@IdTransaccion", SqlDbType.NVarChar);
                SqlParametros[6].Value = IdTransaccion;

                SqlParametros[7] = new SqlParameter("@IdSubEtapa", SqlDbType.NVarChar);
                SqlParametros[7].Value = IdSubEtapa;

                SqlParametros[8] = new SqlParameter("@IdNro", SqlDbType.NVarChar);
                SqlParametros[8].Value = IdNro;

                SqlParametros[9] = new SqlParameter("@Rut", SqlDbType.NVarChar);
                SqlParametros[9].Value = Rut;

                SqlParametros[10] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[10].Value = RazonSocial;

                SqlParametros[11] = new SqlParameter("@NCertificado", SqlDbType.NVarChar);
                SqlParametros[11].Value = NCertificado;

                SqlParametros[12] = new SqlParameter("@fechaInicio", SqlDbType.NVarChar);
                SqlParametros[12].Value = fechaInicio;

                SqlParametros[13] = new SqlParameter("@fechaFin", SqlDbType.NVarChar);
                SqlParametros[13].Value = fechaFin;

                dt = AD_Global.ejecutarConsulta("ListarContabilidad", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        //public string Buscar_Usuario(String Usuario)
        //{
        //    string cargo = "";
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = currentSite.OpenWeb(SPContext.Current.Web.ID))
        //            {
        //                List<string> objColumns = new List<string>();
        //                SPList list = web.Lists.TryGetList("Usuarios");
        //                string sQuery = "<Where><Eq><FieldRef Name='Usuario'/><Value Type='Text'>" + Usuario + "</Value></Eq></Where>";
        //                var oQuery = new SPQuery();
        //                oQuery.Query = sQuery;
        //                SPListItemCollection items = list.GetItems(oQuery);
        //                SPListItemCollection collListItems = list.GetItems(oQuery);
        //                foreach (SPListItem oListItem in collListItems)
        //                {
        //                    cargo = oListItem["Cargo"] != null ? (oListItem["Cargo"].ToString()) : "";
        //                    break;
        //                }
        //            }
        //        }
        //    });
        //    return cargo;
        //}

        #endregion

        #region "wpListarContabilidadHistorico"

        public DataSet ListarContabilidadHistorial(string Perfil, string Usuario, string IdTransaccion, int IdSubEtapa, string IdNro,
        string Rut, string RazonSocial, string NCertificado, string fechaInicio, string fechaFin, int pageS, int pageN, string Etapa)
        {

            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[14];

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

                SqlParametros[13] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[13].Value = Etapa;

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

        #region "wpPagar"

        public Boolean InsertarActualizarPagoFacturacion(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
  string Comentario, string NroDocumentoPago, string idBanco, string descBanco, string fechaPago, string idTipoPago, string DescTipoPago, string DescTipoTransaccion)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[13];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
                SqlParametros[3].Value = cargoUser;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[4].Value = "04";

                SqlParametros[5] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[5].Value = Comentario;
                SqlParametros[6] = new SqlParameter("@NroDocumentoPago", SqlDbType.NVarChar);
                SqlParametros[6].Value = NroDocumentoPago;
                SqlParametros[7] = new SqlParameter("@idBanco", SqlDbType.NVarChar);
                SqlParametros[7].Value = idBanco;
                SqlParametros[8] = new SqlParameter("@descBanco", SqlDbType.NVarChar);
                SqlParametros[8].Value = descBanco;
                SqlParametros[9] = new SqlParameter("@fechaPago", SqlDbType.NVarChar);
                SqlParametros[9].Value = fechaPago;
                SqlParametros[10] = new SqlParameter("@idTipoPago", SqlDbType.NVarChar);
                SqlParametros[10].Value = idTipoPago;
                SqlParametros[11] = new SqlParameter("@DescTipoPago", SqlDbType.NVarChar);
                SqlParametros[11].Value = DescTipoPago;

                SqlParametros[12] = new SqlParameter("@DescTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[12].Value = DescTipoTransaccion;

                dt = AD_Global.ejecutarAccion("GestionFacturacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarActualizarEstadoPagar(string idEmpresa, string idOperacion, string idUsuario, string cargoUser,
      string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario, string id)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[14];
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

                SqlParametros[13] = new SqlParameter("@idTipoTransaccion", SqlDbType.NVarChar);
                SqlParametros[13].Value = id;

                dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpRendicionAAT"


        public DataTable ListarRendicionCliente(string Usuario)
        {
            try
            {
                DataTable dt = new DataTable();
                //AD_Global objDatos = new AD_Global();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@usuario", SqlDbType.NChar);
                SqlParametros[0].Value = Usuario;

                dt = AD_Global.ejecutarConsultas("BuscarRendicionesUsuario", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        //public DataTable ListarUsuarios(int? IdCargo, int? IdDepartamento, string NombreUsuario)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParam;
        //        sqlParam = new SqlParameter[3];
        //        sqlParam[0] = new SqlParameter("@IdCargo", IdCargo);
        //        sqlParam[1] = new SqlParameter("@IdDepartamento", IdDepartamento);
        //        sqlParam[2] = new SqlParameter("@NombreUsuario", NombreUsuario);
        //        return AD_Global.ejecutarConsultas("ListarUsuario", sqlParam);
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }
        //}

        #endregion


        #region "wpBitacora"

        //public DataTable GestionBitacoraPago(string xmlBitacora, string NroCertificado, int? IdMotivo, int? IdCausa, int? IdBanco, DateTime? FechaCobro, DateTime? FechaPago, string NroDocumento, int? Monto, string Comentario, int? IdBitacoraPago, string IdUsuario, string RazonSocial, int IdOpcion)
        public DataTable GestionBitacoraPago(string NroCertificado, string RazonSocial, int IdOpcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[0].Value = NroCertificado;
                SqlParametros[1] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[1].Value = RazonSocial;
                SqlParametros[2] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[2].Value = IdOpcion;

                //SqlParameter[] SqlParametros = new SqlParameter[14];
                //SqlParametros[0] = new SqlParameter("@xmlBitacora", SqlDbType.Xml);
                //SqlParametros[0].Value = xmlBitacora;
                //SqlParametros[1] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                //SqlParametros[1].Value = NroCertificado;
                //SqlParametros[2] = new SqlParameter("@IdMotivo", SqlDbType.Int);
                //SqlParametros[2].Value = IdMotivo;
                //SqlParametros[3] = new SqlParameter("@IdCausa", SqlDbType.Int);
                //SqlParametros[3].Value = IdCausa;
                //SqlParametros[4] = new SqlParameter("@IdBanco", SqlDbType.Int);
                //SqlParametros[4].Value = IdBanco;
                //SqlParametros[5] = new SqlParameter("@FechaCobro", SqlDbType.DateTime);
                //SqlParametros[5].Value = FechaCobro;
                //SqlParametros[6] = new SqlParameter("@FechaPago", SqlDbType.DateTime);
                //SqlParametros[6].Value = FechaPago;
                //SqlParametros[7] = new SqlParameter("@NroDocumento", SqlDbType.NVarChar);
                //SqlParametros[7].Value = NroDocumento;
                //SqlParametros[8] = new SqlParameter("@Monto", SqlDbType.Int);
                //SqlParametros[8].Value = Monto;
                //SqlParametros[9] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                //SqlParametros[9].Value = Comentario;
                //SqlParametros[10] = new SqlParameter("@IdBitacoraPago", SqlDbType.Int);
                //SqlParametros[10].Value = IdBitacoraPago;
                //SqlParametros[11] = new SqlParameter("@IdUsuario", SqlDbType.NVarChar);
                //SqlParametros[11].Value = IdUsuario;
                //SqlParametros[12] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                //SqlParametros[12].Value = RazonSocial;
                //SqlParametros[13] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                //SqlParametros[13].Value = IdOpcion;

                dt = AD_Global.ejecutarConsultas("GestionBitacoraPago", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }


        public Boolean Gestion_BitacoraPago(string xmlBitacora, string NroCertificado, int? IdMotivo, int? IdCausa, int? IdBanco, DateTime? FechaCobro, DateTime? FechaPago, string NroDocumento, int? Monto, string Cuota, int IdAcreedor, string Comentario, int? IdBitacoraPago, string IdUsuario, int IdOpcion, int IdConcepto = 0)
        {
            try
            {
                bool dt = false;
                SqlParameter[] SqlParametros = new SqlParameter[16];
                SqlParametros[0] = new SqlParameter("@xmlBitacora", SqlDbType.Xml);
                SqlParametros[0].Value = xmlBitacora;
                SqlParametros[1] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
                SqlParametros[1].Value = NroCertificado;
                SqlParametros[2] = new SqlParameter("@IdMotivo", SqlDbType.Int);
                SqlParametros[2].Value = IdMotivo;
                SqlParametros[3] = new SqlParameter("@IdCausa", SqlDbType.Int);
                SqlParametros[3].Value = IdCausa;
                SqlParametros[4] = new SqlParameter("@IdBanco", SqlDbType.Int);
                SqlParametros[4].Value = IdBanco;
                SqlParametros[5] = new SqlParameter("@FechaCobro", SqlDbType.DateTime);
                SqlParametros[5].Value = FechaCobro;
                SqlParametros[6] = new SqlParameter("@FechaPago", SqlDbType.DateTime);
                SqlParametros[6].Value = FechaPago;
                SqlParametros[7] = new SqlParameter("@NroDocumento", SqlDbType.NVarChar);
                SqlParametros[7].Value = NroDocumento;
                SqlParametros[8] = new SqlParameter("@Monto", SqlDbType.Int);
                SqlParametros[8].Value = Monto;
                SqlParametros[9] = new SqlParameter("@Cuota", SqlDbType.NVarChar);
                SqlParametros[9].Value = Cuota;
                SqlParametros[10] = new SqlParameter("@IdAcreedor", SqlDbType.NVarChar);
                SqlParametros[10].Value = IdAcreedor;
                SqlParametros[11] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[11].Value = Comentario;
                SqlParametros[12] = new SqlParameter("@IdBitacoraPago", SqlDbType.Int);
                SqlParametros[12].Value = IdBitacoraPago;
                SqlParametros[13] = new SqlParameter("@IdUsuario", SqlDbType.NVarChar);
                SqlParametros[13].Value = IdUsuario;
                SqlParametros[14] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[14].Value = IdOpcion;
                SqlParametros[15] = new SqlParameter("@IdConcepto", SqlDbType.Int);
                SqlParametros[15].Value = IdConcepto;

                dt = AD_Global.ejecutarAccionBool("GestionBitacoraPago", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #region "cuenta corriente"

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

        public DataTable ListarMotivos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];

                //SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                //SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                dt = AD_Global.ejecutarConsultas("ListarMotivos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarCausas()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];

                //SqlParametros[0] = new SqlParameter("@opcion", SqlDbType.NChar);
                //SqlParametros[0].Value = Constantes.OPCION.LISTAGENERAL;

                dt = AD_Global.ejecutarConsultas("ListarCausas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarConceptos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("CC_ListarConceptos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #endregion

        #region "wpSolicitudFiscalia"

        public Boolean InsertarSolicitudFiscaliaOPE(string idEmpresa, string idOperacion, string xmlEmpresa, string xmlGarantia, string Comentario, string ComentarioF, string user, string perfil, string validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NChar);
                SqlParametros[2].Value = Comentario;

                SqlParametros[3] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlEmpresa;

                SqlParametros[4] = new SqlParameter("@xmlGarantía", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlGarantia;

                SqlParametros[5] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(idOperacion);

                SqlParametros[6] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;

                SqlParametros[8] = new SqlParameter("@ComentarioFiscalia", SqlDbType.NChar);
                SqlParametros[8].Value = ComentarioF;

                SqlParametros[9] = new SqlParameter("@validacion", SqlDbType.NChar);
                SqlParametros[9].Value = validacion;

                return AD_Global.ejecutarAccion("GuardarSolicitudFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ListarResumenOpe(string idEmpresa, string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@IdOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarServicios(string idEmpresa, string idPaf, string idoperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.NVarChar);
                SqlParametros[1].Value = idPaf;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = idoperacion;
                dt = AD_Global.ejecutarConsulta("ListarServiciosFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarResumenTotal(string idEmpresa, string idPaf, string idoperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.NVarChar);
                SqlParametros[1].Value = idPaf;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@IdOperacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = idoperacion;

                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarResumenFiscalia(string idEmpresa, string idOperacion, string idGarantia)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idGarantia", SqlDbType.NVarChar);
                SqlParametros[2].Value = idGarantia;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.INSERTAR;
                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarResumenPAF(string idEmpresa, string idPaf, string idoperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.NVarChar);
                SqlParametros[1].Value = idPaf;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@IdOperacion", SqlDbType.NVarChar);
                SqlParametros[3].Value = idoperacion;

                dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean InsertarSolicitudFiscalia(string idEmpresa, string xmlEmpresa, string xmlGarantia, string Comentario, string xmlComentario, string user, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[8];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NChar);
                SqlParametros[2].Value = Comentario;

                SqlParametros[3] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlEmpresa;

                SqlParametros[4] = new SqlParameter("@xmlGarantía", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlGarantia;

                SqlParametros[5] = new SqlParameter("@XmlComentarioG", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlGarantia;

                SqlParametros[6] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;
                return AD_Global.ejecutarAccion("GuardarSolicitudFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        //public string ValidarMandatarioJudicial(int idEmpresa, int idOperacion, string descEmpresa)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[3];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresa;

        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[1].Value = idOperacion;

        //        SqlParametros[2] = new SqlParameter("@descEmpresa", SqlDbType.NVarChar);
        //        SqlParametros[2].Value = descEmpresa;

        //        return AD_Global.traerPrimeraColumna("ValidarMandatarioJudicial", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return "";
        //    }
        //}

        #endregion

        #region "wpServiciosLegalesGarantia"

        public DataTable ServiciosGarantia(string idEmpresa, string idpaf, string Perfil, string Usuario, string idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[1].Value = idpaf;
                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = Perfil;
                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Usuario;

                SqlParametros[4] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[4].Value = idOperacion;

                dt = AD_Global.ejecutarConsultas("ListarServiciosGarantias", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpServiciosLegalesEmpresa"

        public DataTable ServiciosOperacion(string idEmpresa, string idpaf, string Perfil, string Usuario, string idoperacion)
        {
            try
            {
                DataTable dt = new DataTable();

                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[1].Value = idpaf;
                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = Perfil;
                SqlParametros[3] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[3].Value = Usuario;

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idoperacion;

                dt = AD_Global.ejecutarConsultas("ListarServiciosOperaciones", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        //public DataSet ListarResumen(string idEmpresa, string idOperacion)
        //{
        //    try
        //    {
        //        DataSet dt = new DataSet();
        //        SqlParameter[] SqlParametros = new SqlParameter[3];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
        //        SqlParametros[1].Value = idOperacion;
        //        SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
        //        dt = AD_Global.ejecutarConsulta("ListarDatosEmpresaFiscalia", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataSet();
        //    }
        //}

        public Boolean ActualizarEstado(Boolean estado, string id, string idEmpresa)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@IdServicio", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(id);

                SqlParametros[2] = new SqlParameter("@FiscaliaEstado", SqlDbType.Int);
                SqlParametros[2].Value = estado;
                return AD_Global.ejecutarAccion("ActualizarEstadoOperaciones", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
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
                return 0;
            }
        }

        public Boolean ActualizarSolicitudFiscaliaGarantia(string idEmpresa, string xmlEmpresa, string Validar, string idOperacion, string idUsuario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[1].Value = xmlEmpresa;

                SqlParametros[2] = new SqlParameter("@Validacion", SqlDbType.Int);
                SqlParametros[2].Value = Validar;

                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(idOperacion);

                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = idUsuario;

                return AD_Global.ejecutarAccion("ActualizarEstadoGarantia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        public Boolean ActualizarSolicitudFiscalia(string idEmpresa, string xmlEmpresa, string Validar, string IdOperacion, string idUsuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[1].Value = xmlEmpresa;
                SqlParametros[2] = new SqlParameter("@Validacion", SqlDbType.Int);
                SqlParametros[2].Value = Validar;
                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(IdOperacion);
                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NChar);
                SqlParametros[4].Value = idUsuario;
                return AD_Global.ejecutarAccion("ActualizarEstadoOperaciones", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable validarDocCriticos(string idEmpresa, string IdOperacion, int accion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                SqlParametros[2] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[2].Value = accion;

                dt = AD_Global.ejecutarConsultas("validarDocCriticos", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public int validacionCriticosFiscalia(string idEmpresa, string IdOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(IdOperacion);

                dt = AD_Global.ejecutarConsultas("ValidarCriticoFiscalia", SqlParametros);
                if (dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return 0;
            }
        }

        //public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
        //{
        //    try
        //    {
        //        Boolean dt;
        //        SqlParameter[] SqlParametros = new SqlParameter[13];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
        //        SqlParametros[1].Value = idOperacion;
        //        SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
        //        SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = idUsuario;
        //        SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
        //        SqlParametros[4].Value = cargoUser;

        //        SqlParametros[5] = new SqlParameter("@idEstado", SqlDbType.Int);
        //        SqlParametros[5].Value = Estado;
        //        SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
        //        SqlParametros[6].Value = descEstado;

        //        SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
        //        SqlParametros[7].Value = Etapa;
        //        SqlParametros[8] = new SqlParameter("@descEtapa", SqlDbType.NVarChar);
        //        SqlParametros[8].Value = descEtapa;

        //        SqlParametros[9] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
        //        SqlParametros[9].Value = subEtapa;
        //        SqlParametros[10] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
        //        SqlParametros[10].Value = descsubEtapa;

        //        SqlParametros[11] = new SqlParameter("@Area", SqlDbType.NVarChar);
        //        SqlParametros[11].Value = Area;

        //        SqlParametros[12] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
        //        SqlParametros[12].Value = comentario;

        //        dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        public Boolean BajarServicioOperacion(int idEmpresa, int idOperacion, int idServicio, int IdGarantia, string idUsuario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[1].Value = idOperacion;
                SqlParametros[2] = new SqlParameter("@idServicio", SqlDbType.Int);
                SqlParametros[2].Value = idServicio;
                SqlParametros[3] = new SqlParameter("@IdGarantia", SqlDbType.Int);
                SqlParametros[3].Value = IdGarantia;
                SqlParametros[4] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = idUsuario;

                dt = AD_Global.ejecutarAccion("BajarServicioOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarActualizarEstadosFiscalia(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string subEtapa, string descsubEtapa, string idPaf)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[8];
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
                SqlParametros[5] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(subEtapa);
                SqlParametros[6] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = descsubEtapa;
                SqlParametros[7] = new SqlParameter("@NroPaf", SqlDbType.Int);
                SqlParametros[7].Value = int.Parse(idPaf);
                return AD_Global.ejecutarAccion("ActualizarEstadosFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "wpListarFiscalia"

        public Boolean InsertarActualizarEstadosFiscalia(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string subEtapa, string descsubEtapa)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[7];
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

                SqlParametros[5] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
                SqlParametros[5].Value = subEtapa;
                SqlParametros[6] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
                SqlParametros[6].Value = descsubEtapa;

                dt = AD_Global.ejecutarAccion("ActualizarEstadosFiscalia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        //public string Buscar_Usuario(String Usuario)
        //{
        //    string cargo = "";
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = currentSite.OpenWeb(SPContext.Current.Web.ID))
        //            {
        //                List<string> objColumns = new List<string>();
        //                SPList list = web.Lists.TryGetList("Usuarios");
        //                string sQuery = "<Where><Eq><FieldRef Name='Usuario'/><Value Type='Text'>" + Usuario + "</Value></Eq></Where>";
        //                var oQuery = new SPQuery();
        //                oQuery.Query = sQuery;
        //                SPListItemCollection items = list.GetItems(oQuery);
        //                SPListItemCollection collListItems = list.GetItems(oQuery);
        //                foreach (SPListItem oListItem in collListItems)
        //                {
        //                    cargo = oListItem["Cargo"] != null ? (oListItem["Cargo"].ToString()) : "";
        //                    break;
        //                }
        //            }
        //        }
        //    });
        //    return cargo;
        //}

        public DataSet ListarFiscalia(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN, string edofiscalia)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[11];
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
                SqlParametros[10] = new SqlParameter("@filtroEdoFiscalia", SqlDbType.NVarChar);
                SqlParametros[10].Value = edofiscalia;

                dt = AD_Global.ejecutarConsulta("ListarFiscalia", SqlParametros);

                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarFiscaliaCFT(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN, string edofiscalia)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[11];
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
                SqlParametros[10] = new SqlParameter("@filtroEdoFiscalia", SqlDbType.NVarChar);
                SqlParametros[10].Value = edofiscalia;

                dt = AD_Global.ejecutarConsulta("ListarFiscaliaCFT", SqlParametros);

                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }


        #endregion

        #region "wpAprobacionSolicitud"

        public Boolean InsertarSolicitudFiscalia(string idEmpresa, string idOperacion, string xmlEmpresa, string xmlGarantia, string Comentario, string ComentarioF, string user, string perfil, string validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[10];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);

                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NChar);
                SqlParametros[2].Value = Comentario;

                SqlParametros[3] = new SqlParameter("@xmlEmpresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlEmpresa;

                SqlParametros[4] = new SqlParameter("@xmlGarantía", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlGarantia;

                SqlParametros[5] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(idOperacion);

                SqlParametros[6] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[6].Value = user;

                SqlParametros[7] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[7].Value = perfil;

                SqlParametros[8] = new SqlParameter("@ComentarioFiscalia", SqlDbType.NChar);
                SqlParametros[8].Value = ComentarioF;

                SqlParametros[9] = new SqlParameter("@validacion", SqlDbType.NChar);
                SqlParametros[9].Value = validacion;

                return AD_Global.ejecutarAccion("GuardarSolicitudFiscalia", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        #endregion

        #region "DocumentosFiscalia"

        public string ListarGarantiasAnexo1(int IdEmpresa)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                return mensaje = AD_Global.traerPrimeraColumna("ListarGarantiasAnexo1", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje;
            }
        }

        public string ListarRepresentantes(int IdEmpresa)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                return mensaje = AD_Global.traerPrimeraColumna("ListarRepresentantesEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje;
            }
        }

        public string ListarAvales(int IdEmpresa)
        {
            string mensaje = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                return mensaje = AD_Global.traerPrimeraColumna("ListarAvalesEmpresa", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return mensaje;
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

        public DataTable ListarClaseRepresentantes(string clase)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Clase", SqlDbType.NVarChar);
                SqlParametros[0].Value = clase;

                dt = AD_Global.ejecutarConsultas("ListarClaseRepresentantes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarRepresentantesId(int IdOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;

                dt = AD_Global.ejecutarConsultas("ListarRepresentantesId", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertaActualizaRepresentantes(int Id, int IdOperacion, int ClaseA, int ClaseB, string Usuario)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                SqlParametros[0].Value = Id;
                SqlParametros[1] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[1].Value = IdOperacion;
                SqlParametros[2] = new SqlParameter("@ClaseA", SqlDbType.Int);
                SqlParametros[2].Value = ClaseA;
                SqlParametros[3] = new SqlParameter("@ClaseB", SqlDbType.Int);
                SqlParametros[3].Value = ClaseB;
                SqlParametros[4] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
                SqlParametros[4].Value = Usuario;

                //SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                //SqlParametros[6].Value = descEstado;

                //SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
                //SqlParametros[7].Value = Etapa;


                dt = AD_Global.ejecutarAccion("InsertaActualizaRepresentantes", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string TieneGarantiaFogape(int IdEmpresa)
        {
            try
            {
                string dt = string.Empty;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;

                dt = AD_Global.traerPrimeraColumna("TieneGarantiaFogape", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "0";
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

        #endregion

        public string ListarNumCertificado(int idOperacion)
        {
            string NumCertificado = string.Empty;
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                return NumCertificado = AD_Global.traerPrimeraColumna("ListarNumCertificado", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return NumCertificado;
            }
        }

        //public DataTable ListarUsuarios(int? IdCargo, int? IdDepartamento, string NombreUsuario)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParam;
        //        sqlParam = new SqlParameter[3];
        //        sqlParam[0] = new SqlParameter("@IdCargo", IdCargo);
        //        sqlParam[1] = new SqlParameter("@IdDepartamento", IdDepartamento);
        //        sqlParam[2] = new SqlParameter("@NombreUsuario", NombreUsuario);
        //        return AD_Global.ejecutarConsultas("ListarUsuario", sqlParam);
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }
        //}

        public DataTable ListarSubEtapaLegal(int Orden)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Orden", SqlDbType.Int);
                SqlParametros[0].Value = Orden;

                dt = AD_Global.ejecutarConsultas("ListarSubEtapaLegal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean GestionSubEtapaLegal(int IdOperacion, int Orden, string Comentario, int IdOpcion)
        {
            try
            {
                Boolean dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;
                SqlParametros[1] = new SqlParameter("@Orden", SqlDbType.Int);
                SqlParametros[1].Value = Orden;
                SqlParametros[2] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
                SqlParametros[2].Value = Comentario;
                SqlParametros[3] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[3].Value = IdOpcion;

                dt = AD_Global.ejecutarAccion("GestionSubEtapaLegal", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string DescargarNombreDoc(int idTasacion)
        {
            try
            {
                string nombreDoc = String.Empty;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idTasacion", SqlDbType.Int);
                SqlParametros[0].Value = idTasacion;

                nombreDoc = AD_Global.traerPrimeraColumna("DescargarNombreDoc", SqlParametros);
                return nombreDoc;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "";
            }
        }

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

        //public int CP_VerificarCertificado(string NroCertificado)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[1];
        //        SqlParametros[0] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
        //        SqlParametros[0].Value = NroCertificado;

        //        var resultado = AD_Global.traerPrimeraColumna("VerificarCertificado", SqlParametros);
        //        return int.Parse(resultado);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return -1;
        //    }
        //}

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
                //SqlParametros[14] = new SqlParameter("@TipoPagoMasivo", SqlDbType.NVarChar);
                //SqlParametros[14].Value = TipoPagoMasivo;

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
                DataTable dtFondos = ListarFondos(0);
                DataRow[] result = dtFondos.Select("Nombre = '" + ID + "'");
                foreach (DataRow row in result)
                {
                    return row["Porcentaje"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return "";
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

        //public DataTable ConsultarOperacion(int IdOperacion)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[2];
        //        SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[0].Value = IdOperacion;
        //        SqlParametros[1] = new SqlParameter("@accion", SqlDbType.Int);
        //        SqlParametros[1].Value = 6;

        //        dt = AD_Global.ejecutarConsultas("GestionOperaciones", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
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

        //public Boolean InsertarActualizarEstados(string idEmpresa, string idOperacion, string idUsuario, string cargoUser, string Etapa, string descEtapa, string subEtapa, string descsubEtapa, string Estado, string descEstado, string Area, string comentario)
        //{
        //    try
        //    {
        //        Boolean dt;
        //        SqlParameter[] SqlParametros = new SqlParameter[13];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.NVarChar);
        //        SqlParametros[1].Value = idOperacion;
        //        SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
        //        SqlParametros[2].Value = "01";
        //        SqlParametros[3] = new SqlParameter("@idUsuario", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = idUsuario;
        //        SqlParametros[4] = new SqlParameter("@descCargo", SqlDbType.NVarChar);
        //        SqlParametros[4].Value = cargoUser;

        //        SqlParametros[5] = new SqlParameter("@idEstado", SqlDbType.Int);
        //        SqlParametros[5].Value = Estado;
        //        SqlParametros[6] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
        //        SqlParametros[6].Value = descEstado;

        //        SqlParametros[7] = new SqlParameter("@idEtapa", SqlDbType.Int);
        //        SqlParametros[7].Value = Etapa;
        //        SqlParametros[8] = new SqlParameter("@descEtapa", SqlDbType.NVarChar);
        //        SqlParametros[8].Value = descEtapa;

        //        SqlParametros[9] = new SqlParameter("@idSubEtapa", SqlDbType.Int);
        //        SqlParametros[9].Value = subEtapa;
        //        SqlParametros[10] = new SqlParameter("@descSubEtapa", SqlDbType.NVarChar);
        //        SqlParametros[10].Value = descsubEtapa;

        //        SqlParametros[11] = new SqlParameter("@Area", SqlDbType.NVarChar);
        //        SqlParametros[11].Value = Area;

        //        SqlParametros[12] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
        //        SqlParametros[12].Value = comentario;

        //        dt = AD_Global.ejecutarAccion("ActualizarEstados", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        //public DataTable ConsultarDocumentosParametrizados(int IdProducto, int IdAcreedor, string area)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[3];
        //        SqlParametros[0] = new SqlParameter("@IdProducto", SqlDbType.Int);
        //        SqlParametros[0].Value = IdProducto;
        //        SqlParametros[1] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
        //        SqlParametros[1].Value = IdAcreedor;
        //        SqlParametros[2] = new SqlParameter("@area", SqlDbType.NVarChar);
        //        SqlParametros[2].Value = area;
        //        dt = AD_Global.ejecutarConsultas("ConsultarDocumentosParametrizados", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}

        //public DataSet CargarDatosHtml(int IdOperacion)
        //{
        //    try
        //    {
        //        DataSet dt = new DataSet();
        //        SqlParameter[] SqlParametros = new SqlParameter[1];
        //        SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
        //        SqlParametros[0].Value = IdOperacion;

        //        dt = AD_Global.ejecutarConsulta("CargarDatosPlantilla", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataSet();
        //    }
        //}


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

        //public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string id, Resumen objresumen)
        //{
        //    try
        //    {
        //        Utilidades util = new Utilidades();
        //        String xml = String.Empty;

        //        //IF PARA CADA REPORTE
        //        DataSet res1 = new DataSet();
        //        if (sp == "Contrato_de_Subfianza")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteContratoSubfianza", datos["ValorUF"], datos["ValorUS"]);
        //            xml = GenerarXMLContratoSubFianza(res1);
        //        }
        //        else if (sp == "Instruccion_de_Curse")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteInstruccionCurse", datos["ValorUF"], datos["ValorUS"]);
        //            xml = GenerarXMLInstruccionCurse(res1);
        //        }
        //        else if (sp == "Certificado_de_Fianza_Comercial" || sp == "Certificado_de_Fianza_Banco_Estado" || sp == "Certificado_de_Fianza_Banco_Security" || sp == "Certificado_de_Fianza_Factoring" || sp == "Certificado_de_Fianza_Itau" || sp == "Certificado_de_Fianza_BBVA")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
        //            foreach (DataRow fila in res1.Tables[0].Rows)
        //                xml = xml + fila[0];
        //            //xml = res1.Tables[0].Rows[0][0].ToString();
        //            if (sp == "Certificado_de_Fianza_Comercial")
        //                xml = GenerarCodigoBarra(res1, objresumen, res1.Tables[1].Rows[0][0].ToString());
        //        }
        //        else if (sp == "Certificado_de_Fianza_Tecnica")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
        //            xml = GenerarXMLCertificadoFianza(res1, id);
        //            xml = GenerarCodigoBarra(res1, objresumen, res1.Tables[1].Rows[0][0].ToString());
        //        }
        //        else if (sp == "Solicitud_de_Pago_de_Garantia")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoFianza", datos["ValorUF"], datos["ValorUS"]);
        //            xml = GenerarXMLCertificadoFianza(res1, id);
        //        }
        //        else if (sp == "Certificado_de_Elegibilidad")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCertificadoElegibilidad", datos["ValorUF"], datos["ValorUS"]);
        //            xml = GenerarXMLCertificadoFianza(res1, id);
        //        }
        //        else if (sp == "Carta_de_Garantia" || sp == "Carta_de_Garantia_ITAU" || sp == "Carta_de_Garantia_Santander" || sp == "Carta_de_Garantia_BBVA" || sp == "Carta_de_Garantia_Banco_Estado" || sp == "Carta_de_Garantia_Banco_Security" || sp == "Carta_de_Garantia_Factoring")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteCartaGarantias", datos["ValorUF"], datos["ValorUS"]);
        //            xml = GenerarXMLCartadeGarantia(res1);
        //            xml = xml.Replace("<RutAcreedor>RUTACREEDOR</RutAcreedor>", "<RutAcreedor>" + BuscarIdsYValoresCambioDeEstado(res1.Tables[0].Rows[0]["IdAcreedor"].ToString()).Split('|')[0] + "</RutAcreedor>");
        //            xml = xml.Replace("<ClasificacionSBIF></ClasificacionSBIF>", "<ClasificacionSBIF>" + BuscarParametro("MULTIAVAL S.A.G.R.") + "</ClasificacionSBIF>");
        //            xml = xml.Replace("<NombreFondoGarantia></NombreFondoGarantia>", "<NombreFondoGarantia>" + BuscarFondo(res1.Tables[0].Rows[0]["Fondo"].ToString()) + "</NombreFondoGarantia>");
        //        }
        //        else if (sp == "Solicitud_de_Emision")
        //        {
        //            res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdOperacion"], "DocumentoCurse", "admin", "ReporteSolicitudEmision", datos["ValorUF"], datos["ValorUS"]);
        //            foreach (DataRow fila in res1.Tables[0].Rows)
        //                xml = xml + fila[0];
        //            int ini = xml.IndexOf("<IdAcreedor>"), fin = xml.IndexOf("</IdAcreedor>");
        //            string idacreedor = xml.Substring(ini + 12, fin - (ini + 12));

        //            xml.Replace("<acreedorRUT></acreedorRUT>", "<acreedorRUT>" + BuscarIdsYValoresCambioDeEstado(idacreedor).Split('|')[0] + "</acreedorRUT>");
        //        }
        //        else if (sp == "Revision_Pagare")
        //        {
        //            //string Reporte = "Revision_Pagare_" + datos["IdOperacion"].ToString();
        //            //DataTable dtRes = new DataTable();
        //            LogicaNegocio LN = new LogicaNegocio();
        //            res1 = LN.GenerarXMLRevisionPagare(int.Parse(datos["IdOperacion"].ToString()));
        //            if (res1.Tables.Count >= 1)
        //            {
        //                foreach (DataRow fila in res1.Tables[0].Rows)
        //                {
        //                    xml = xml + fila[0];
        //                }
        //            }

        //            //byte[] archivo = GenerarReportePagare(int.Parse(datos["IdOperacion"].ToString()));
        //        }

        //        XDocument newTree = new XDocument();
        //        XslCompiledTransform xsltt = new XslCompiledTransform();

        //        using (XmlWriter writer = newTree.CreateWriter())
        //        {
        //            xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + sp + ".xslt");
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

        //public string GenerarXMLContratoSubFianza(DataSet res1)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //    doc.AppendChild(docNode);

        //    XmlNode ValoresNode = doc.CreateElement("Valores");
        //    doc.AppendChild(ValoresNode);

        //    XmlNode RespNode;
        //    XmlNode root = doc.DocumentElement;
        //    RespNode = doc.CreateElement("Val");

        //    XmlNode RespNodeCkb;
        //    RespNodeCkb = doc.CreateElement("General");

        //    XmlNode nodo = doc.CreateElement("Fondo");
        //    nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Fondo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo);

        //    XmlNode nodo1 = doc.CreateElement("acreedorRazonSocial");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["acreedorRazonSocial"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    XmlNode nodo2 = doc.CreateElement("NCertificado");
        //    nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NCertificado"].ToString()));
        //    RespNodeCkb.AppendChild(nodo2);

        //    XmlNode nodo3 = doc.CreateElement("Fogape");
        //    nodo3.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Fogape"].ToString()));
        //    RespNodeCkb.AppendChild(nodo3);

        //    DateTime thisDate = DateTime.Now;
        //    CultureInfo culture = new CultureInfo("es-ES");

        //    XmlNode nodo4 = doc.CreateElement("FechaHoy");
        //    nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
        //    RespNodeCkb.AppendChild(nodo4);

        //    nodo4 = doc.CreateElement("fechaEmision");
        //    nodo4.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fechaEmision"].ToString()));
        //    RespNodeCkb.AppendChild(nodo4);

        //    XmlNode nodo5 = doc.CreateElement("acreedorRUT");
        //    nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["acreedorRUT"].ToString()));
        //    RespNodeCkb.AppendChild(nodo5);

        //    XmlNode nodo6 = doc.CreateElement("acreedorDireccion");
        //    nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["acreedorDireccion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo6);

        //    XmlNode nodo7 = doc.CreateElement("nroCuotas");
        //    nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["nroCuotas"].ToString()));
        //    RespNodeCkb.AppendChild(nodo7);

        //    XmlNode nodo8 = doc.CreateElement("MontoMoneda");
        //    nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Monto"].ToString() + " " + res1.Tables[0].Rows[0]["Moneda"].ToString()));
        //    RespNodeCkb.AppendChild(nodo8);

        //    XmlNode nodo9 = doc.CreateElement("Tasa");
        //    nodo9.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tasa"].ToString()));
        //    RespNodeCkb.AppendChild(nodo9);

        //    XmlNode nodo10 = doc.CreateElement("Plazo");
        //    nodo10.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["plazo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo10);

        //    XmlNode nodo11 = doc.CreateElement("NDocumento");
        //    nodo11.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NDocumento"].ToString()));
        //    RespNodeCkb.AppendChild(nodo11);

        //    XmlNode nodo12 = doc.CreateElement("clienteRazonSocial");
        //    nodo12.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteRazonSocial"].ToString()));
        //    RespNodeCkb.AppendChild(nodo12);

        //    XmlNode nodo13 = doc.CreateElement("tipoOperacion");
        //    nodo13.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoOperacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo13);

        //    XmlNode nodo14 = doc.CreateElement("PeriodoGracia");
        //    nodo14.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["PeriodoGracia"].ToString()));
        //    RespNodeCkb.AppendChild(nodo14);

        //    //representante legal multiaval 17-01-2017
        //    XmlNode nodo15 = doc.CreateElement("DescRepresentanteLegal");
        //    nodo15.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRepresentanteLegal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo15);

        //    XmlNode nodo16 = doc.CreateElement("RutRepresentanteLegal");
        //    nodo16.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo16);

        //    XmlNode nodo17 = doc.CreateElement("DescRepresentanteFondo");
        //    nodo17.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRepresentanteFondo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo17);

        //    XmlNode nodo18 = doc.CreateElement("RutRepresentanteFondo");
        //    nodo18.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteFondo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo18);

        //    //datos de la empresa aval
        //    XmlNode nodo19 = doc.CreateElement("Rut");
        //    nodo19.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString()));
        //    RespNodeCkb.AppendChild(nodo19);

        //    XmlNode nodo20 = doc.CreateElement("SGR");
        //    nodo20.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["SGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo20);

        //    XmlNode nodo21 = doc.CreateElement("NombreSGR");
        //    nodo21.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreSGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo21);

        //    XmlNode nodo22 = doc.CreateElement("Domicilio");
        //    nodo22.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Domicilio"].ToString()));
        //    RespNodeCkb.AppendChild(nodo22);

        //    XmlNode nodo23 = doc.CreateElement("FechaEscrituraPublica");
        //    nodo23.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEscrituraPublica"].ToString()));
        //    RespNodeCkb.AppendChild(nodo23);

        //    XmlNode nodo24 = doc.CreateElement("Notaria");
        //    nodo24.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Notaria"].ToString()));
        //    RespNodeCkb.AppendChild(nodo24);

        //    XmlNode nodo25 = doc.CreateElement("NumeroRepertorio");
        //    nodo25.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroRepertorio"].ToString()));
        //    RespNodeCkb.AppendChild(nodo25);

        //    XmlNode nodo26 = doc.CreateElement("RepresentanteLegal");
        //    nodo26.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RepresentanteLegal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo26);

        //    XmlNode nodo27 = doc.CreateElement("RutRepresentanteLegalSGR");
        //    nodo27.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegalSGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo27);

        //    // asi con todos los nodos de3 este nievel
        //    RespNode.AppendChild(RespNodeCkb);
        //    ValoresNode.AppendChild(RespNode);

        //    XmlNode RespNodeG;
        //    root = doc.DocumentElement;
        //    RespNodeG = doc.CreateElement("Garantias");


        //    for (int i = 0; i <= res1.Tables[1].Rows.Count - 1; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Garantia");

        //        nodo = doc.CreateElement("NGarantia");
        //        nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["NGarantia"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo);

        //        RespNodeG.AppendChild(RespNodeGarantia);
        //    }

        //    ValoresNode.AppendChild(RespNodeG);


        //    //aval -----------------------------------
        //    XmlNode RespNodeA;
        //    root = doc.DocumentElement;
        //    RespNodeA = doc.CreateElement("Avales");


        //    for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
        //    {
        //        XmlNode RespNodeAval;
        //        RespNodeAval = doc.CreateElement("Aval");

        //        nodo = doc.CreateElement("Aval");
        //        nodo.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Aval"].ToString()));
        //        RespNodeAval.AppendChild(nodo);

        //        RespNodeA.AppendChild(RespNodeAval);
        //    }

        //    ValoresNode.AppendChild(RespNodeA);
        //    return doc.OuterXml;
        //}

        //public string GenerarXMLInstruccionCurse(DataSet res1)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //    doc.AppendChild(docNode);

        //    XmlNode ValoresNode = doc.CreateElement("Valores");
        //    doc.AppendChild(ValoresNode);

        //    XmlNode RespNode;
        //    XmlNode root = doc.DocumentElement;
        //    RespNode = doc.CreateElement("Val");

        //    XmlNode RespNodeCkb;
        //    RespNodeCkb = doc.CreateElement("General");

        //    XmlNode nodo1 = doc.CreateElement("NumeroCertificado");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroCertificado"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("NumeroCartaGarantia");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroCartaGarantia"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("FechaInstructivo");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaInstructivo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("FechaTC");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaTC"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("AcreedorRazonSocial");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["AcreedorRazonSocial"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("AcreedorID");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["AcreedorID"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("ClienteRazonSocial");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ClienteRazonSocial"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("ClienteRUT");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ClienteRUT"].ToString() + "-" + res1.Tables[0].Rows[0]["ClienteDivRUT"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoCredito");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoCredito"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);
        //    //--porcentajeComision

        //    nodo1 = doc.CreateElement("MontoCreditoCLP");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoCreditoCLP"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("porcentajeComision");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["porcentajeComision"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);


        //    nodo1 = doc.CreateElement("Moneda");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Moneda"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("FechaCurse");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaCurse"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("ValorUF");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ValorUF"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);


        //    nodo1 = doc.CreateElement("MontoCredito");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoCredito"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoComision");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoComision"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("GastosOperacionales");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["GastosOperacionales"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoSeguro");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoSeguro"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoSeguroD");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoSeguroD"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoNotario");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoNotario"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoComisionFogape");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoComisionFogape"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoTyEMultiaval");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoTyEMultiaval"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoTyEAcreedor");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoTyEAcreedor"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("InstruccionCurse");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["InstruccionCurse"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TipoOperacion");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TipoOperacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalMismoBanco");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMismoBanco"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalOtrosBancos");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalOtrosBancos"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalBancos");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalBancos"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalMismoBancoR");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMismoBancoR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalMultiavalL");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMultiavalL"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalMultiaval");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMultiaval"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalOtrosBancosR");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalOtrosBancosR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalMultiavalR");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMultiavalR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);


        //    nodo1 = doc.CreateElement("TotalMismoBancoL");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalMismoBancoL"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalOtrosBancosL");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalOtrosBancosL"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("MontoLiquidoCliente");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MontoLiquidoCliente"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("TotalFondosRetenidos");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TotalFondosRetenidos"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("LiberacionesTotal");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["LiberacionesTotal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("FechaVencimiento");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaVencimiento"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    nodo1 = doc.CreateElement("NombreSAGR");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreSAGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    RespNode.AppendChild(RespNodeCkb);
        //    ValoresNode.AppendChild(RespNode);
        //    //BANCO
        //    XmlNode RespNodeG;
        //    root = doc.DocumentElement;
        //    RespNodeG = doc.CreateElement("MismoBanco");

        //    for (int i = 0; i <= res1.Tables[1].Rows.Count - 1; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Banco");

        //        nodo1 = doc.CreateElement("NroCredito");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["NroCredito"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo1);

        //        nodo1 = doc.CreateElement("MontoCredito");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["MontoCredito"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo1);

        //        RespNodeG.AppendChild(RespNodeGarantia);
        //    }

        //    ValoresNode.AppendChild(RespNodeG);

        //    //OTROS BANCOS -----------------------------------
        //    XmlNode RespNodeA;
        //    root = doc.DocumentElement;
        //    RespNodeA = doc.CreateElement("OtrosBancos");

        //    for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
        //    {
        //        XmlNode RespNodeAval;
        //        RespNodeAval = doc.CreateElement("Banco");

        //        nodo1 = doc.CreateElement("DescAcreedor");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["DescAcreedor"].ToString()));
        //        RespNodeAval.AppendChild(nodo1);

        //        nodo1 = doc.CreateElement("MontoCredito");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["MontoCredito"].ToString()));
        //        RespNodeAval.AppendChild(nodo1);

        //        RespNodeA.AppendChild(RespNodeAval);
        //    }

        //    ValoresNode.AppendChild(RespNodeA);
        //    return doc.OuterXml;
        //}

        //public string GenerarXMLCertificadoFianza(DataSet res1, string id)
        //{
        //    string xmlt = "";
        //    res1.Tables[0].Rows[0][0].ToString().Replace("<acreedorRUT>acredor rut</acreedorRUT>", "<acreedorRUT>" + BuscarIdsYValoresCambioDeEstado(id) + "</acreedorRUT>");
        //    foreach (DataRow fila in res1.Tables[0].Rows)
        //        xmlt = xmlt + fila[0];

        //    return xmlt;
        //}

        //public string GenerarXMLCartadeGarantia(DataSet res1)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null);
        //    doc.AppendChild(docNode);

        //    XmlNode ValoresNode = doc.CreateElement("Empresas");
        //    doc.AppendChild(ValoresNode);

        //    XmlNode RespNode;
        //    XmlNode root = doc.DocumentElement;
        //    RespNode = doc.CreateElement("Empresa");

        //    XmlNode RespNodeCkb;
        //    RespNodeCkb = doc.CreateElement("General");

        //    XmlNode nodo = doc.CreateElement("fechaActual");
        //    nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fechaActual"].ToString()));
        //    RespNodeCkb.AppendChild(nodo);

        //    XmlNode nodo1 = doc.CreateElement("clienteRazonSocial");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteRazonSocial"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    XmlNode nodo2 = doc.CreateElement("clienteRUT");
        //    nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteRUT"].ToString() + "-" + res1.Tables[0].Rows[0]["clienteDivRUT"].ToString()));
        //    RespNodeCkb.AppendChild(nodo2);

        //    XmlNode nodo3 = doc.CreateElement("clienteDireccion");
        //    nodo3.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["clienteDireccion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo3);

        //    DateTime thisDate = DateTime.Now;
        //    CultureInfo culture = new CultureInfo("es-ES");

        //    XmlNode nodo4 = doc.CreateElement("fecha");
        //    nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
        //    RespNodeCkb.AppendChild(nodo4);

        //    thisDate = (DateTime.Now).AddDays(5);

        //    nodo4 = doc.CreateElement("fecha5");
        //    nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
        //    RespNodeCkb.AppendChild(nodo4);

        //    XmlNode nodo198 = doc.CreateElement("fechaContrato");
        //    nodo198.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fechaContrato"].ToString()));
        //    RespNodeCkb.AppendChild(nodo198);

        //    XmlNode nodo199 = doc.CreateElement("FechaEmisionC");
        //    nodo199.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEmisionC"].ToString()));
        //    RespNodeCkb.AppendChild(nodo199);

        //    nodo199 = doc.CreateElement("FechaEmisionC5");
        //    nodo199.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEmisionC5"].ToString()));
        //    RespNodeCkb.AppendChild(nodo199);

        //    XmlNode nodo5 = doc.CreateElement("Fondo");
        //    nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Fondo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo5);

        //    XmlNode nodo6 = doc.CreateElement("tipoOperacion");
        //    nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoOperacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo6);

        //    nodo6 = doc.CreateElement("tipoAmortizacion");
        //    nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoAmortizacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo6);

        //    XmlNode nodo7 = doc.CreateElement("periocidad");
        //    nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["periocidad"].ToString()));
        //    RespNodeCkb.AppendChild(nodo7);

        //    XmlNode nodo8 = doc.CreateElement("plazo");
        //    nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["plazo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo8);

        //    nodo8 = doc.CreateElement("plazoDias");
        //    nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["plazoDias"].ToString()));
        //    RespNodeCkb.AppendChild(nodo8);

        //    XmlNode nodo9 = doc.CreateElement("tasa");
        //    nodo9.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tasa"].ToString()));
        //    RespNodeCkb.AppendChild(nodo9);

        //    XmlNode nodo10 = doc.CreateElement("montoOperacion");
        //    nodo10.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["montoOperacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo10);

        //    XmlNode nodo11 = doc.CreateElement("periodoGracia");
        //    nodo11.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["periodoGracia"].ToString()));
        //    RespNodeCkb.AppendChild(nodo11);

        //    XmlNode nodo12 = doc.CreateElement("fogape");
        //    nodo12.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["fogape"].ToString()));
        //    RespNodeCkb.AppendChild(nodo12);

        //    XmlNode nodo13 = doc.CreateElement("coberturaFogape");
        //    nodo13.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["coberturaFogape"].ToString()));
        //    RespNodeCkb.AppendChild(nodo13);

        //    XmlNode nodo14 = doc.CreateElement("NumeroDocumento");
        //    nodo14.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroDocumento"].ToString()));
        //    RespNodeCkb.AppendChild(nodo14);

        //    XmlNode nodo15 = doc.CreateElement("Monto");
        //    nodo15.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Monto"].ToString()));
        //    RespNodeCkb.AppendChild(nodo15);

        //    XmlNode nodo16 = doc.CreateElement("FondoGarantia");
        //    nodo16.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FondoGarantia"].ToString()));
        //    RespNodeCkb.AppendChild(nodo16);

        //    XmlNode nodo17 = doc.CreateElement("NumeroCuotas");
        //    nodo17.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroCuotas"].ToString()));
        //    RespNodeCkb.AppendChild(nodo17);

        //    XmlNode nodo18 = doc.CreateElement("FondoCorfo");
        //    if (res1.Tables[0].Rows[0]["FondoCorfo"].ToString() == "1")
        //        nodo18.AppendChild(doc.CreateTextNode("OPERACIÓN AFIANZADA CONTRA FONDO CORFO"));
        //    else
        //        nodo18.AppendChild(doc.CreateTextNode(""));
        //    RespNodeCkb.AppendChild(nodo18);

        //    XmlNode nodo19 = doc.CreateElement("Acreedor");
        //    nodo19.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Acreedor"].ToString()));
        //    RespNodeCkb.AppendChild(nodo19);

        //    XmlNode nodo20 = doc.CreateElement("RutAcreedor");
        //    nodo20.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutAcreedor"].ToString()));
        //    RespNodeCkb.AppendChild(nodo20);

        //    XmlNode nodo21 = doc.CreateElement("DireccionAcreedor");
        //    nodo21.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DireccionAcreedor"].ToString()));
        //    RespNodeCkb.AppendChild(nodo21);

        //    XmlNode nodo22 = doc.CreateElement("FechaEmision");
        //    nodo22.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEmision"].ToString()));
        //    RespNodeCkb.AppendChild(nodo22);

        //    XmlNode nodo23 = doc.CreateElement("Moneda");
        //    nodo23.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Moneda"].ToString()));
        //    RespNodeCkb.AppendChild(nodo23);

        //    XmlNode nodo24 = doc.CreateElement("NumeroOperacion");
        //    nodo24.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroOperacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo24);

        //    XmlNode nodo25 = doc.CreateElement("IdAcreedor");
        //    nodo25.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["IdAcreedor"].ToString()));
        //    RespNodeCkb.AppendChild(nodo25);

        //    XmlNode nodo26 = doc.CreateElement("Tasa");
        //    nodo26.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Tasa"].ToString()));
        //    RespNodeCkb.AppendChild(nodo26);

        //    XmlNode nodo27 = doc.CreateElement("TipoTasa");
        //    nodo27.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TipoTasa"].ToString()));
        //    RespNodeCkb.AppendChild(nodo27);

        //    XmlNode nodo28 = doc.CreateElement("FechaPrimerVencimiento");
        //    nodo28.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaPrimerVencimiento"].ToString()));
        //    RespNodeCkb.AppendChild(nodo28);

        //    XmlNode nodo29 = doc.CreateElement("FechaUltimoVencimiento");
        //    nodo29.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaUltimoVencimiento"].ToString()));
        //    RespNodeCkb.AppendChild(nodo29);

        //    XmlNode nodo30 = doc.CreateElement("ClasificacionSBIF");
        //    nodo30.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["ClasificacionSBIF"].ToString()));
        //    RespNodeCkb.AppendChild(nodo30);

        //    XmlNode nodo31 = doc.CreateElement("NombreFondoGarantia");
        //    nodo31.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreFondoGarantia"].ToString()));
        //    RespNodeCkb.AppendChild(nodo31);

        //    XmlNode nodo32 = doc.CreateElement("TipoOperacion");
        //    nodo32.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TipoOperacion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo32);

        //    XmlNode nodo33 = doc.CreateElement("MonedaC");
        //    nodo33.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["MonedaC"].ToString()));
        //    RespNodeCkb.AppendChild(nodo33);

        //    XmlNode nodo34 = doc.CreateElement("Signo");
        //    nodo34.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Signo"].ToString()));
        //    RespNodeCkb.AppendChild(nodo34);
        //    // Avales
        //    XmlNode nodo35 = doc.CreateElement("Avales");
        //    nodo35.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Avales"].ToString()));
        //    RespNodeCkb.AppendChild(nodo35);
        //    // cobertura
        //    XmlNode nodo36 = doc.CreateElement("cobertura");
        //    nodo36.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["cobertura"].ToString()));
        //    RespNodeCkb.AppendChild(nodo36);

        //    XmlNode nodo37 = doc.CreateElement("tipoCuota");
        //    nodo37.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["tipoCuota"].ToString()));
        //    RespNodeCkb.AppendChild(nodo37);

        //    XmlNode nodo38 = doc.CreateElement("NroPagare");
        //    nodo38.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NroPagare"].ToString()));
        //    RespNodeCkb.AppendChild(nodo38);

        //    //representante legal multiaval 17-01-2017
        //    XmlNode nodo39 = doc.CreateElement("DescRepresentanteLegal");
        //    nodo39.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRepresentanteLegal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo39);

        //    XmlNode nodo40 = doc.CreateElement("RutRepresentanteLegal");
        //    nodo40.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo40);

        //    //datos de la empresa sagr asociada al fondo
        //    XmlNode nodo41 = doc.CreateElement("Rut");
        //    nodo41.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString()));
        //    RespNodeCkb.AppendChild(nodo41);

        //    XmlNode nodo42 = doc.CreateElement("SGR");
        //    nodo42.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["SGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo42);

        //    XmlNode nodo43 = doc.CreateElement("NombreSGR");
        //    nodo43.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NombreSGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo43);

        //    XmlNode nodo44 = doc.CreateElement("Domicilio");
        //    nodo44.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Domicilio"].ToString()));
        //    RespNodeCkb.AppendChild(nodo44);

        //    XmlNode nodo45 = doc.CreateElement("FechaEscrituraPublica");
        //    nodo45.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["FechaEscrituraPublica"].ToString()));
        //    RespNodeCkb.AppendChild(nodo45);

        //    XmlNode nodo46 = doc.CreateElement("Notaria");
        //    nodo46.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Notaria"].ToString()));
        //    RespNodeCkb.AppendChild(nodo46);

        //    XmlNode nodo47 = doc.CreateElement("NumeroRepertorio");
        //    nodo47.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["NumeroRepertorio"].ToString()));
        //    RespNodeCkb.AppendChild(nodo47);

        //    XmlNode nodo48 = doc.CreateElement("RepresentanteLegal");
        //    nodo48.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RepresentanteLegal"].ToString()));
        //    RespNodeCkb.AppendChild(nodo48);

        //    XmlNode nodo49 = doc.CreateElement("RutRepresentanteLegalSGR");
        //    nodo49.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RutRepresentanteLegalSGR"].ToString()));
        //    RespNodeCkb.AppendChild(nodo49);

        //    XmlNode nodo50 = doc.CreateElement("valorCuota");
        //    nodo50.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["valorCuota"].ToString()));
        //    RespNodeCkb.AppendChild(nodo50);

        //    //------------------------------------------------------------------------------------//

        //    RespNode.AppendChild(RespNodeCkb);
        //    ValoresNode.AppendChild(RespNode);

        //    XmlNode RespNodeG;
        //    root = doc.DocumentElement;
        //    RespNodeG = doc.CreateElement("Avales");

        //    for (int i = 0; i <= res1.Tables[1].Rows.Count - 1; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Aval");

        //        nodo = doc.CreateElement("Aval");
        //        nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Aval"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo);


        //        RespNodeG.AppendChild(RespNodeGarantia);
        //    }

        //    ValoresNode.AppendChild(RespNodeG);
        //    return doc.OuterXml;
        //}

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

        public string BuscarIdsYValoresCambioDeEstado(int ID)
        {
            try
            {
                DataTable dtAcreedores = CRUDAcreedores(7, string.Empty, ID, string.Empty, string.Empty, 0, 0, 0, 0, string.Empty);
                return dtAcreedores.Rows[0]["Rut"].ToString() + "|" + dtAcreedores.Rows[0]["Nombre"].ToString() + "|" + dtAcreedores.Rows[0]["Domicilio"].ToString();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return "";
        }

        public DataSet ConsultaReporteBD(string idEmpresa, string idPaf, int idOpcion, string usuario, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idPaf);
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idEmpresa);
                SqlParametros[2] = new SqlParameter("@idOpcion", SqlDbType.Int);
                SqlParametros[2].Value = idOpcion;

                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public string ListarEmpresaSagr(int IdEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Id", SqlDbType.Int);
                SqlParametros[0].Value = IdEmpresa;
                dt = AD_Global.ejecutarConsultas("ListarEmpresaSagr", SqlParametros);
                return dt.Rows[0]["Valor"].ToString() + " " + dt.Rows[0]["Observacion"].ToString();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return string.Empty;
            }
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

        //public string Buscar_Usuario(String Usuario)
        //{
        //    string cargo = "";
        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //    {
        //        using (SPSite currentSite = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = currentSite.OpenWeb(SPContext.Current.Web.ID))
        //            {
        //                List<string> objColumns = new List<string>();
        //                SPList list = web.Lists.TryGetList("Usuarios");
        //                string sQuery = "<Where><Eq><FieldRef Name='Usuario'/><Value Type='Text'>" + Usuario + "</Value></Eq></Where>";
        //                var oQuery = new SPQuery();
        //                oQuery.Query = sQuery;
        //                SPListItemCollection items = list.GetItems(oQuery);
        //                SPListItemCollection collListItems = list.GetItems(oQuery);
        //                foreach (SPListItem oListItem in collListItems)
        //                {
        //                    cargo = oListItem["Cargo"] != null ? (oListItem["Cargo"].ToString()) : "";
        //                    break;
        //                }
        //            }
        //        }
        //    });
        //    return cargo;
        //}


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
                return new DataSet();
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

        //public DataTable ConsultaValidacionesEmpresa(string idEmpresa, int opcion)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[2];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.NVarChar);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.Int);
        //        SqlParametros[1].Value = opcion;
        //        dt = AD_Global.ejecutarConsultas("ConsultaValidacionesEmpresa", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataTable();
        //    }
        //}

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

        public DataTable ListarDocumentoContable()
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[0];
                return AD_Global.ejecutarConsultas("ListarDocumentoContable", sqlParam);
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

        //public DataTable CRUDAcreedores(int Opcion, string RutAcreedor, int IdAcreedor, string NombreAcreedor, string DomicilioAcreedor, int IdTipoAcreedor, int IdRegion, int IdProvincia, int IdComuna, string IdUsuario)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParam;
        //        sqlParam = new SqlParameter[10];
        //        sqlParam[0] = new SqlParameter("@Opcion", Opcion);
        //        sqlParam[1] = new SqlParameter("@RutAcreedor", RutAcreedor.Trim());
        //        sqlParam[2] = new SqlParameter("@IdAcreedor", IdAcreedor);
        //        sqlParam[3] = new SqlParameter("@NombreAcreedor", NombreAcreedor);
        //        sqlParam[4] = new SqlParameter("@DomicilioAcreedor", DomicilioAcreedor);
        //        sqlParam[5] = new SqlParameter("@IdTipoAcreedor", IdTipoAcreedor);
        //        sqlParam[6] = new SqlParameter("@IdRegion", IdRegion);
        //        sqlParam[7] = new SqlParameter("@IdProvincia", IdProvincia);
        //        sqlParam[8] = new SqlParameter("@IdComuna", IdComuna);
        //        sqlParam[9] = new SqlParameter("@IdUsuario", IdUsuario);
        //        return AD_Global.ejecutarConsultas("CRUDAcreedores", sqlParam);
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }
        //}

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

        //public DataTable GestionValorMoneda(DateTime Fecha, int IdValorMoneda, double? MontoUF, double? @MontoUSD, double? @MontoEuro, int IdOpcion)
        //{
        //    try
        //    {
        //        SqlParameter[] sqlParam;
        //        sqlParam = new SqlParameter[6];
        //        sqlParam[0] = new SqlParameter("@Fecha", Fecha);
        //        sqlParam[1] = new SqlParameter("@IdValorMoneda", IdValorMoneda);
        //        sqlParam[2] = new SqlParameter("@MontoUF", MontoUF);
        //        sqlParam[3] = new SqlParameter("@MontoUSD", MontoUSD);
        //        sqlParam[4] = new SqlParameter("@MontoEuro", MontoEuro);
        //        sqlParam[5] = new SqlParameter("@IdOpcion", IdOpcion);
        //        return AD_Global.ejecutarConsultas("GestionValorMoneda", sqlParam);
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }
        //}

        #endregion

        #region "wpIvaCompras"

        //public Boolean GestionResponsableOperacion(int idOperacion, string usuario, string perfil, string opcion)
        //{
        //    /*OPCIONES:
        //     * 01 =  Riesgo
        //     * 02 =  Fiscalia
        //     * 03 =  Operacion
        //     * 04 =  Contabilidad
        //    */
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        SqlParameter[] SqlParametros = new SqlParameter[4];
        //        SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[0].Value = idOperacion;
        //        SqlParametros[1] = new SqlParameter("@usuario", SqlDbType.NChar);
        //        SqlParametros[1].Value = usuario;
        //        SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NChar);
        //        SqlParametros[2].Value = perfil;
        //        SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = opcion;
        //        return AD_Global.ejecutarAccion("GestionResponsablesOperacion", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        public Boolean finalizarIvaCOMPRAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVACOMPRAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                SqlParametros[6] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[6].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean finalizarIvaVENTAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVAVENTAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                SqlParametros[6] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[6].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarIvaCOMPRAS(string idEmpresa, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVACOMPRAS;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;
                SqlParametros[4] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[4].Value = anio;

                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarIvaVENTAS(string idEmpresa, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVAVENTAS;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;
                SqlParametros[4] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[4].Value = anio;

                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarIvaCOMPRAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVACOMPRAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[5].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean ModificarIvaVENTAS(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string anio)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.IVAVENTAS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@anio", SqlDbType.Int);
                SqlParametros[5].Value = anio;
                return AD_Global.ejecutarAccion("GuardarDocumentosContablesIVA", SqlParametros);

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarIVACompras(int idEmpresa, int Anio, int IdDocumentoContable)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Anio", SqlDbType.Int);
                SqlParametros[1].Value = Anio;
                SqlParametros[2] = new SqlParameter("@IdDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = IdDocumentoContable;
                dt = AD_Global.ejecutarConsultas("ConsultarIVA", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ConsultaReporteIvaCompraVenta(int idEmpresa)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                dt = AD_Global.ejecutarConsulta("ReporteIvaCompraVenta", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpListarRiesgo"

        public DataSet ListarRiesgo(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
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

                dt = AD_Global.ejecutarConsulta("ListarRiesgo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ListarCftRiesgo(string etapa, string subEtapa, string estado, string buscar, string area, string cargo, string user, int pageS, int pageN)
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

                dt = AD_Global.ejecutarConsulta("ListarRiesgoCFT", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpScoring"

        public Boolean InsertarScoring(string idEmpresa, string xmlData, string usuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[1].Value = 1;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[5].Value = perfil;

                return AD_Global.ejecutarAccion("GuardarScoring", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean finalizarScoring(string idEmpresa, string xmlData, string usuario, string perfil)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[1].Value = 2;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;

                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[4].Value = usuario;

                SqlParametros[5] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[5].Value = perfil;

                return AD_Global.ejecutarAccion("GuardarScoring", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarScoring(string idEmpresa, string usuario, string perfil, string opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = opcion;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsultas("GeneracionScoring", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ListarIndicadoresScoring(string idEmpresa, string usuario, string perfil, string opcion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[1].Value = opcion;
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("GeneracionScoring", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ConsultaReporteScoring(int idEmpresa, string usuario, string perfil)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;

                dt = AD_Global.ejecutarConsulta("ReporteScoring", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpVaciadoBalance"

        public Boolean ModificarBalance(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarBalance(string idEmpresa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idDocumentosContables", SqlDbType.Int);
                SqlParametros[0].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GeneracionBalance", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean InsertarFinalizarBalance(string idEmpresa, string xmlData)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACION;

                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarBalance(string idEmpresa, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACIONINSERT;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean finalizarBalance(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.BALANCEGENERAL;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ConsultaReporteVaciadoBalance(int idEmpresa)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;

                dt = AD_Global.ejecutarConsulta("ReporteNoResumen", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpPAF"

        public Boolean DarBajaOperacion(string idOperacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;

                return AD_Global.ejecutarAccion("DarBajaOperacion", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ActualizarImpresionPAF(int idPAF)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[0].Value = idPAF;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[1].Value = Constantes.OPCIONIMPRESIONPAF.IMPRIMIR;
                SqlParametros[2] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[2].Value = Constantes.OPCIONIMPRESIONPAF.EDOIMPRESO;
                dt = AD_Global.ejecutarConsulta("GestionImpresionPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean ActualizarOperaciones(Dictionary<string, string> datos, Dictionary<string, string> estados)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[16];
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(datos["IdOperacion"]);
                SqlParametros[1] = new SqlParameter("@Plazo", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(datos["Plazo"]);
                SqlParametros[2] = new SqlParameter("@ComisionP", SqlDbType.Float);
                SqlParametros[2].Value = float.Parse(datos["ComisionP"]);
                SqlParametros[3] = new SqlParameter("@Seguro", SqlDbType.Money);
                SqlParametros[3].Value = double.Parse(datos["Seguro"]);
                SqlParametros[4] = new SqlParameter("@Comision", SqlDbType.Money);
                SqlParametros[4].Value = double.Parse(datos["Comision"]);
                SqlParametros[5] = new SqlParameter("@MontoCredito", SqlDbType.Money);
                SqlParametros[5].Value = double.Parse(datos["MontoCredito"]);
                SqlParametros[6] = new SqlParameter("@MontoPropuesto", SqlDbType.Money);
                SqlParametros[6].Value = double.Parse(datos["MontoPropuesto"]);
                SqlParametros[7] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[7].Value = double.Parse(datos["IdPaf"]);
                SqlParametros[8] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[8].Value = 4;
                if (estados != null)
                {
                    SqlParametros[9] = new SqlParameter("@NIdEstado", SqlDbType.Int);
                    SqlParametros[9].Value = int.Parse(estados["IdEstado"]);
                    SqlParametros[10] = new SqlParameter("@NDescEstado", SqlDbType.NChar);
                    SqlParametros[10].Value = estados["DescEstado"].ToString();

                    SqlParametros[11] = new SqlParameter("@NIdEtapa", SqlDbType.Int);
                    SqlParametros[11].Value = int.Parse(estados["IdEtapa"]);
                    SqlParametros[12] = new SqlParameter("@NDescEtapa", SqlDbType.NChar);
                    SqlParametros[12].Value = estados["DescEtapa"].ToString();

                    SqlParametros[13] = new SqlParameter("@NIdSubEtapa", SqlDbType.Int);
                    SqlParametros[13].Value = int.Parse(estados["IdSubEtapa"]);
                    SqlParametros[14] = new SqlParameter("@NDescSubEtapa", SqlDbType.NChar);
                    SqlParametros[14].Value = estados["DescSubEtapa"].ToString();
                    SqlParametros[15] = new SqlParameter("@Area", SqlDbType.NChar);
                    SqlParametros[15].Value = "Comercial";
                }
                else
                {
                    SqlParametros[9] = new SqlParameter("@NIdEstado", SqlDbType.Int);
                    SqlParametros[9].Value = -1;
                    SqlParametros[10] = new SqlParameter("@NDescEstado", SqlDbType.NChar);
                    SqlParametros[10].Value = "-1";

                    SqlParametros[11] = new SqlParameter("@NIdEtapa", SqlDbType.Int);
                    SqlParametros[11].Value = -1;
                    SqlParametros[12] = new SqlParameter("@NDescEtapa", SqlDbType.NChar);
                    SqlParametros[12].Value = "-1";

                    SqlParametros[13] = new SqlParameter("@NIdSubEtapa", SqlDbType.Int);
                    SqlParametros[13].Value = -1;
                    SqlParametros[14] = new SqlParameter("@NDescSubEtapa", SqlDbType.NChar);
                    SqlParametros[14].Value = "-1";
                    SqlParametros[15] = new SqlParameter("@Area", SqlDbType.NChar);
                    SqlParametros[15].Value = datos["Area"].ToString();
                }

                bool a = AD_Global.ejecutarAccion("GestionOperaciones", SqlParametros);
                if (estados != null)
                {

                    SPWeb mySite = SPContext.Current.Web;
                    SPList list = mySite.Lists["Operaciones"];

                    SPListItemCollection items = list.GetItems(new SPQuery()
                    {
                        Query = @"<Where><Eq><FieldRef Name='IdSQL' /><Value Type='Number'>" + datos["IdOperacion"] + "</Value></Eq></Where>"
                    });

                    foreach (SPListItem item in items)
                    {
                        item["IdEtapa"] = estados["IdEtapa"];
                        item["IdSubEtapa"] = estados["IdSubEtapa"];
                        item["IdEstado"] = estados["IdEstado"];
                        //item["IdPaf"] = datos["IdPaf"];
                        item.Update();
                    }
                    list.Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Dictionary<string, string> BuscarIdsYValoresCambioDeEstadoV2(string Indicador)
        {
            string mensaje = string.Empty;
            Dictionary<string, string> datos = new Dictionary<string, string>();
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["CambiosEstados"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='TEXT'>" + Indicador + "</Value></Eq></Where>";
                SPListItemCollection ColecLista = Lista.GetItems(oQuery);
                foreach (SPListItem oListItem in ColecLista)
                {
                    datos.Add("IdEstado", oListItem["Estado:ID"].ToString().Split('#')[1]);
                    datos.Add("DescEstado", oListItem["Estado"].ToString().Split('#')[1]);
                    datos.Add("IdEtapa", oListItem["Etapa:ID"].ToString().Split('#')[1]);
                    datos.Add("DescEtapa", oListItem["Etapa"].ToString().Split('#')[1]);
                    datos.Add("IdSubEtapa", oListItem["SubEtapa:ID"].ToString().Split('#')[1]);
                    datos.Add("DescSubEtapa", oListItem["SubEtapa"].ToString().Split('#')[1]);
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                mensaje = ex.Source.ToString() + ex.Message.ToString();
            }
            return datos;
        }

        public DataSet ConsultarImpresionPAF(int idPAF)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[0].Value = idPAF;
                SqlParametros[1] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[1].Value = Constantes.OPCIONIMPRESIONPAF.CONSULTAR;
                dt = AD_Global.ejecutarConsulta("GestionImpresionPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        //public byte[] GenerarReporte(string sp, Dictionary<string, string> datos, string ejecutivo)
        //{
        //    try
        //    {
        //        Utilidades util = new Utilidades();
        //        String xml = String.Empty;
        //        String xmlReporteNoResumen = String.Empty;
        //        String xmlReporteIVACompraVentas = String.Empty;
        //        String xmlIndicFinancieros = String.Empty;

        //        DataSet res1 = new DataSet();

        //        res1 = ConsultaReporteBD(datos["IdEmpresa"], datos["IdPaf"], "Sistema", "Adminitrador", "ReportePAF");

        //        //Tabla 0 Contiene la cantidad de Empresas en el grupo Económico
        //        int cantEmpresas = int.Parse(res1.Tables[0].Rows[0][0].ToString());

        //        for (int i = 0; i < res1.Tables[1].Rows.Count; i++)
        //        {
        //            xml = xml + res1.Tables[1].Rows[i][0];
        //        }
        //        for (int i = 0; i < res1.Tables[2].Rows.Count; i++)
        //        {
        //            xmlReporteIVACompraVentas = xmlReporteIVACompraVentas + res1.Tables[2].Rows[i][0];
        //        }

        //        //Tabla 3 Contiene los datos para la creación de los gráficos

        //        for (int i = 0; i < res1.Tables[4].Rows.Count; i++)
        //        {
        //            xmlReporteNoResumen = xmlReporteNoResumen + res1.Tables[4].Rows[i][0];
        //        }

        //        for (int i = 0; i < res1.Tables[5].Rows.Count; i++)
        //        {
        //            xmlIndicFinancieros = xmlIndicFinancieros + res1.Tables[5].Rows[i][0];

        //        }
        //        xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<ResumenIVA>");
        //        xmlReporteIVACompraVentas = xmlReporteIVACompraVentas.Replace("</Val></Valores>", "</ResumenIVA>");
        //        xmlReporteNoResumen = xmlReporteNoResumen.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<VaciadoBalance>");
        //        xmlReporteNoResumen = xmlReporteNoResumen.Replace("</Val></Valores>", "</VaciadoBalance>");

        //        xmlIndicFinancieros = xmlIndicFinancieros.Replace("<Valores xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Val>", "<IndicadoresFinancierosPadre>");
        //        xmlIndicFinancieros = xmlIndicFinancieros.Replace("</Val></Valores>", "</IndicadoresFinancierosPadre>");

        //        xml = xml.Replace("<ResumenIVA>-</ResumenIVA>", xmlReporteIVACompraVentas);
        //        xml = xml.Replace("<VaciadoBalance>--</VaciadoBalance>", xmlReporteNoResumen);
        //        xml = xml.Replace("<IndicadoresFinancierosPadre>-</IndicadoresFinancierosPadre>", xmlIndicFinancieros);

        //        /////Gráficos
        //        String nombreGrafico1 = String.Empty;
        //        String nombreGrafico2 = String.Empty;
        //        String rutaGraficos = String.Empty;

        //        nombreGrafico1 = "G1" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + datos["IdEmpresa"].ToString();
        //        nombreGrafico2 = "G2" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + datos["IdEmpresa"].ToString();
        //        rutaGraficos = "http://localhost:25698/graficas/";

        //        xml = xml.Replace("<Grafico1></Grafico1>", "<Grafico1>" + rutaGraficos + nombreGrafico1 + ".png" + "</Grafico1>");
        //        xml = xml.Replace("<Grafico2></Grafico2>", "<Grafico2>" + rutaGraficos + nombreGrafico2 + ".png" + "</Grafico2>");

        //        crearGrafico1(res1.Tables[3], nombreGrafico1);
        //        crearGrafico2(res1.Tables[3], nombreGrafico2);

        //        XDocument newTree = new XDocument();
        //        XslCompiledTransform xsltt = new XslCompiledTransform();

        //        using (XmlWriter writer = newTree.CreateWriter())
        //        {
        //            xsltt.Load(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/" + "Propuesta_Afianzamiento" + ".xslt");
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


        //public byte[] GenerarReporteOld(string Reporte, Dictionary<string, string> datos, string ejecutivo)
        //{
        //    try
        //    {
        //        switch (Reporte)
        //        {
        //            case "Propuesta_Afianzamiento_Express":
        //                return generarPaf2Express(datos, ejecutivo);
        //            //break;

        //            case "Propuesta_Afianzamiento_Old":
        //                return generarPaf1(datos, ejecutivo);
        //            //break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //    }
        //    return null;
        //}




        public Boolean ActualizarPafExpress(int idEmpresa, int idPAF, string idUsuario, string opcion, string estado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = idPAF;
                SqlParametros[2] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = opcion;
                SqlParametros[4] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[4].Value = estado;
                return AD_Global.ejecutarAccion("GestionImpresionPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataSet ConsultaReporteBD_Old(string idEmpresa, string idPaf, string usuario, string perfil, string SP)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPaf", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);

                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta(SP, SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public Boolean GestionPAF(Dictionary<string, string> datos, string Garantias, string OperacionesNueva, string OperacionesVigente, string idUsuario, string CapacidadPago, string EvaluacionRiesgo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[28];
                SqlParametros[0] = new SqlParameter("@IdScoring", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(datos["IdScoring"]);
                SqlParametros[1] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(datos["IdEmpresa"]);
                SqlParametros[2] = new SqlParameter("@ObservacionPropuesta", SqlDbType.NVarChar);
                SqlParametros[2].Value = datos["ObservacionPropuesta"];
                SqlParametros[3] = new SqlParameter("@EstadoLinea", SqlDbType.NVarChar);
                SqlParametros[3].Value = datos["EstadoLinea"];
                SqlParametros[4] = new SqlParameter("@NivelAtribucion", SqlDbType.Int);
                SqlParametros[4].Value = datos["NivelAtribucion"];
                SqlParametros[5] = new SqlParameter("@Oficina", SqlDbType.Int);
                SqlParametros[5].Value = datos["Oficina"];
                SqlParametros[6] = new SqlParameter("@Fecha", SqlDbType.Date);
                SqlParametros[6].Value = datos["Fecha"];
                SqlParametros[7] = new SqlParameter("@FechaRevision", SqlDbType.Date);
                SqlParametros[7].Value = datos["FechaRevision"];
                SqlParametros[8] = new SqlParameter("@Ejecutivo", SqlDbType.NVarChar);
                SqlParametros[8].Value = datos["Ejecutivo"];
                SqlParametros[9] = new SqlParameter("@ObservacionComite", SqlDbType.NVarChar);
                SqlParametros[9].Value = datos["ObservacionesComite"];
                SqlParametros[10] = new SqlParameter("@Estatus", SqlDbType.Int);
                if (datos["estado"].ToString() != "2")
                    SqlParametros[10].Value = Constantes.OPCION.INSERTARPAF;
                else
                    SqlParametros[10].Value = Constantes.OPCION.PAFFINAL;
                SqlParametros[11] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[11].Value = int.Parse(datos["IdPaf"]);
                SqlParametros[12] = new SqlParameter("@TotalAprobadoREUF", SqlDbType.Float);
                SqlParametros[12].Value = float.Parse(datos["TotalAprobado"]);
                SqlParametros[13] = new SqlParameter("@TotalVigenteREUF", SqlDbType.Float);
                SqlParametros[13].Value = float.Parse(datos["TotalVigente"]);
                SqlParametros[14] = new SqlParameter("@TotalPropuestoREUF", SqlDbType.Float);
                SqlParametros[14].Value = float.Parse(datos["TotalPropuesto"]);

                SqlParametros[15] = new SqlParameter("@ValorUF", SqlDbType.Float);
                SqlParametros[15].Value = float.Parse(datos["ValorUF"]);
                SqlParametros[16] = new SqlParameter("@ValorUSD", SqlDbType.Float);
                SqlParametros[16].Value = float.Parse(datos["ValorDolar"]);
                SqlParametros[17] = new SqlParameter("@VentasMoviles", SqlDbType.Float);
                SqlParametros[17].Value = float.Parse(datos["VentasMoviles"]);

                SqlParametros[18] = new SqlParameter("@CoberturaVigenteComercial", SqlDbType.Float);
                SqlParametros[18].Value = float.Parse(datos["CoberturaVigenteComercial"]);
                SqlParametros[19] = new SqlParameter("@CoberturaVigenteAjustado", SqlDbType.Float);
                SqlParametros[19].Value = float.Parse(datos["CoberturaVigenteAjustado"]);
                SqlParametros[20] = new SqlParameter("@CoberturaGlobalComercial", SqlDbType.Float);
                SqlParametros[20].Value = float.Parse(datos["CoberturaGlobalComercial"]);
                SqlParametros[21] = new SqlParameter("@CoberturaGlobalAjustado", SqlDbType.Float);
                SqlParametros[21].Value = float.Parse(datos["CoberturaGlobalAjustado"]);

                SqlParametros[22] = new SqlParameter("@xmlDataGarantia", SqlDbType.NVarChar);
                SqlParametros[22].Value = Garantias;

                SqlParametros[23] = new SqlParameter("@xmlDataOperacionesNuevas", SqlDbType.NVarChar);
                SqlParametros[23].Value = OperacionesNueva;

                SqlParametros[24] = new SqlParameter("@xmlDataOperacionesVigentes", SqlDbType.NVarChar);
                SqlParametros[24].Value = OperacionesVigente;

                SqlParametros[25] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[25].Value = idUsuario;
                SqlParametros[26] = new SqlParameter("@CapacidadPago", SqlDbType.NVarChar);
                SqlParametros[26].Value = CapacidadPago;
                SqlParametros[27] = new SqlParameter("@EvaluacionRiesgo", SqlDbType.NVarChar);
                SqlParametros[27].Value = EvaluacionRiesgo;

                return AD_Global.ejecutarAccion("GestionPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarImpresionPAF(int idEmpresa, int idPAF, string idUsuario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = idPAF;
                SqlParametros[2] = new SqlParameter("@usuario", SqlDbType.NVarChar);
                SqlParametros[2].Value = idUsuario;
                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NVarChar);
                SqlParametros[3].Value = Constantes.OPCIONIMPRESIONPAF.INSERTAR;
                SqlParametros[4] = new SqlParameter("@descEstado", SqlDbType.NVarChar);
                SqlParametros[4].Value = Constantes.OPCIONIMPRESIONPAF.EDOPENDIENTE;
                return AD_Global.ejecutarAccion("GestionImpresionPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarAprobadores(int IdOpcion, int IdTipoRepresentante)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[0].Value = IdOpcion;
                SqlParametros[1] = new SqlParameter("@IdTipoRepresentante", SqlDbType.Int);
                SqlParametros[1].Value = IdTipoRepresentante;
                dt = AD_Global.ejecutarConsultas("ListarAprobadores", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataSet ConsutarDatosEmpresaPAF(string idEmpresa, string idPaf, string usuario, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("ConsutarDatosEmpresaPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ObtenerDatosEmpresaPafActual(string idEmpresa, string idPaf, string usuario, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("ConsutarDatosEmpresaPAFActuales", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataSet ConsutarDatosPAF(string idEmpresa, string usuario, string perfil, string idOperacion)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;
                SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(idOperacion);

                dt = AD_Global.ejecutarConsulta("ConsutarDatosPAF", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public bool RegistrarAprobadoresPAF(Dictionary<string, string> datos, string edoPAF, string fecAprobacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[14];
                SqlParametros[0] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(datos["IdPaf"]);
                SqlParametros[1] = new SqlParameter("@IdSP1", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(datos["IdSP1"]);
                SqlParametros[2] = new SqlParameter("@Nombre1", SqlDbType.NVarChar);
                SqlParametros[2].Value = datos["Nombre1"];
                SqlParametros[3] = new SqlParameter("@IdSP2", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(datos["IdSP2"]);
                SqlParametros[4] = new SqlParameter("@Nombre2", SqlDbType.NVarChar);
                SqlParametros[4].Value = datos["Nombre2"];
                SqlParametros[5] = new SqlParameter("@IdSP3", SqlDbType.Int);
                SqlParametros[5].Value = int.Parse(datos["IdSP3"]);
                SqlParametros[6] = new SqlParameter("@Nombre3", SqlDbType.NVarChar);
                SqlParametros[6].Value = datos["Nombre3"];
                SqlParametros[7] = new SqlParameter("@IdSP4", SqlDbType.Int);
                SqlParametros[7].Value = int.Parse(datos["IdSP4"]);
                SqlParametros[8] = new SqlParameter("@Nombre4", SqlDbType.NVarChar);
                SqlParametros[8].Value = datos["Nombre4"];
                SqlParametros[9] = new SqlParameter("@IdSP5", SqlDbType.Int);
                SqlParametros[9].Value = int.Parse(datos["IdSP5"]);
                SqlParametros[10] = new SqlParameter("@Nombre5", SqlDbType.NVarChar);
                SqlParametros[10].Value = datos["Nombre5"];
                SqlParametros[11] = new SqlParameter("@accion", SqlDbType.Int);
                SqlParametros[11].Value = 1;
                SqlParametros[12] = new SqlParameter("@EdoPAF", SqlDbType.NVarChar);
                SqlParametros[12].Value = edoPAF;
                SqlParametros[13] = new SqlParameter("@FecAprobacion", SqlDbType.NVarChar);
                SqlParametros[13].Value = fecAprobacion;

                return AD_Global.ejecutarAccion("GestionAprobadoresPAF", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean sinDocumentosContables(string IdEmpresa, string usuario, string perfil, string IdPaf, Boolean valor)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(IdEmpresa);
                SqlParametros[1] = new SqlParameter("@User", SqlDbType.NVarChar);
                SqlParametros[1].Value = usuario;
                SqlParametros[2] = new SqlParameter("@Perfil", SqlDbType.NVarChar);
                SqlParametros[2].Value = perfil;
                SqlParametros[3] = new SqlParameter("@IdPaf", SqlDbType.Int);
                SqlParametros[3].Value = int.Parse(IdPaf);
                SqlParametros[4] = new SqlParameter("@Accion", SqlDbType.Int);
                SqlParametros[4].Value = "1";
                SqlParametros[5] = new SqlParameter("@valor", SqlDbType.Bit);
                SqlParametros[5].Value = valor;

                return AD_Global.ejecutarAccion("sinDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }


        //private byte[] crearGrafico1(DataTable dt, string nombreGrafico1)
        //private void crearGrafico1(DataTable dt, string nombreGrafico1)
        //{
        //    //Gráfico 1;
        //    double[] anioAct = new double[12];
        //    double[] anio1 = new double[12];
        //    double[] anio2 = new double[12];
        //    double[] anio3 = new double[12];
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        anioAct[i] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
        //        anio1[i] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
        //        anio2[i] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
        //        anio3[i] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
        //    }
        //    string[] meses = new string[12];
        //    int nroMes = 1;
        //    CultureInfo culture = new CultureInfo("es-ES");
        //    for (int i = 0; i < meses.Length; i++)
        //    {
        //        meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
        //        nroMes++;
        //    }

        //    Chart grafico1 = new Chart();
        //    grafico1.ChartAreas.Add(new ChartArea());
        //    grafico1.ChartAreas[0].AxisX.Interval = 1;
        //    grafico1.Width = 710;
        //    grafico1.Height = 300;
        //    Series serieAnioAct = new Series();
        //    Series serieAnio1 = new Series();
        //    Series serieAnio2 = new Series();
        //    Series serieAnio3 = new Series();

        //    serieAnioAct.Points.DataBindXY(meses, anioAct);
        //    serieAnio1.Points.DataBindXY(meses, anio1);
        //    serieAnio2.Points.DataBindXY(meses, anio2);
        //    serieAnio3.Points.DataBindXY(meses, anio3);

        //    serieAnioAct.Name = (DateTime.Now.Year).ToString();
        //    serieAnio1.Name = (DateTime.Now.Year - 1).ToString();
        //    serieAnio2.Name = (DateTime.Now.Year - 2).ToString();
        //    serieAnio3.Name = (DateTime.Now.Year - 3).ToString();

        //    //#99BE1A verde
        //    //#E85426 nar
        //    //#1E9CD6 azu
        //    //#9966CC mor
        //    serieAnioAct.Color = ColorTranslator.FromHtml("#1E9CD6");
        //    serieAnio1.Color = ColorTranslator.FromHtml("#9966CC");
        //    serieAnio2.Color = ColorTranslator.FromHtml("#E85426");
        //    serieAnio3.Color = ColorTranslator.FromHtml("#99BE1A");

        //    serieAnioAct.BorderWidth = 3;
        //    serieAnio1.BorderWidth = 3;
        //    serieAnio2.BorderWidth = 3;
        //    serieAnio3.BorderWidth = 3;

        //    serieAnioAct.ChartType = SeriesChartType.Spline;
        //    serieAnioAct.BorderDashStyle = ChartDashStyle.Dash;

        //    serieAnio1.ChartType = SeriesChartType.Spline;
        //    serieAnio1.BorderDashStyle = ChartDashStyle.Dot;

        //    serieAnio2.ChartType = SeriesChartType.Spline;
        //    serieAnio2.BorderDashStyle = ChartDashStyle.Solid;

        //    serieAnio3.ChartType = SeriesChartType.Spline;
        //    serieAnio3.BorderDashStyle = ChartDashStyle.DashDotDot;

        //    Title titulo = new Title();
        //    titulo.Name = "Titulo1";
        //    titulo.Text = "Ventas Mensuales por Período (M$)";
        //    titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
        //    grafico1.Titles.Add(titulo);

        //    Legend lg = new Legend();
        //    lg.Name = "Leyenda1";
        //    lg.Docking = Docking.Bottom;
        //    lg.Alignment = StringAlignment.Center;
        //    grafico1.Legends.Add(lg);
        //    serieAnioAct.Legend = "Leyenda1";

        //    grafico1.Series.Add(serieAnioAct);
        //    grafico1.Series.Add(serieAnio1);
        //    grafico1.Series.Add(serieAnio2);
        //    grafico1.Series.Add(serieAnio3);

        //    //using (var chartimage = new MemoryStream())
        //    //{
        //    //    grafico1.SaveImage(chartimage, ChartImageFormat.Png);
        //    //    return chartimage.GetBuffer();
        //    //}

        //    grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico1 + ".png", ChartImageFormat.Png);
        //}

        //private void crearGrafico2(DataTable dt, string nombreGrafico2)
        //{
        //    double[] ventas = new double[48];

        //    int index = 0;
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas3"].ToString().Replace(".", ""));
        //        index++;
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas2"].ToString().Replace(".", ""));
        //        index++;
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ventas[index] = Convert.ToDouble(dt.Rows[i]["Ventas1"].ToString().Replace(".", ""));
        //        index++;
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        ventas[index] = Convert.ToDouble(dt.Rows[i]["VentasAct"].ToString().Replace(".", ""));
        //        index++;
        //    }

        //    string[] meses = new string[48];
        //    int nroMes = 1;
        //    CultureInfo culture = new CultureInfo("es-ES");
        //    for (int i = 0; i < meses.Length; i++)
        //    {
        //        if (nroMes == 13)
        //        {
        //            nroMes = 1;
        //        }
        //        meses[i] = culture.DateTimeFormat.GetMonthName(nroMes).Substring(0, 1).ToUpper();
        //        nroMes++;
        //    }

        //    Chart grafico1 = new Chart();
        //    grafico1.ChartAreas.Add(new ChartArea());
        //    grafico1.ChartAreas[0].AxisX.Interval = 1;
        //    grafico1.Width = 710;
        //    grafico1.Height = 300;
        //    Series serieVentas = new Series();

        //    serieVentas.Points.DataBindXY(meses, ventas);
        //    serieVentas.Name = (DateTime.Now.Year - 3).ToString() + ", " + (DateTime.Now.Year - 2).ToString() + ", " + (DateTime.Now.Year - 1).ToString() + ", " + (DateTime.Now.Year).ToString();
        //    serieVentas.Color = Color.OrangeRed;//ColorTranslator.FromHtml("#1E9CD6");
        //    serieVentas.BorderWidth = 3;
        //    serieVentas.ChartType = SeriesChartType.Spline;

        //    Title titulo = new Title();
        //    titulo.Name = "Titulo1";
        //    titulo.Text = "Ventas Todos los Períodos (M$)";
        //    titulo.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
        //    grafico1.Titles.Add(titulo);

        //    Legend lg = new Legend();
        //    lg.Name = "Leyenda1";
        //    lg.Docking = Docking.Bottom;
        //    lg.Alignment = StringAlignment.Center;
        //    grafico1.Legends.Add(lg);
        //    serieVentas.Legend = "Leyenda1";

        //    grafico1.Series.Add(serieVentas);
        //    //grafico1.SaveImage(@"C:/inetpub/wwwroot/wss/VirtualDirectories/80/xsl/request/graficas/" + nombreGrafico2 + ".png", ChartImageFormat.Png);
        //}

        //System.Text.StringBuilder sDocumento = new System.Text.StringBuilder();
        //string html = string.Empty;
        //public string GenerarXMLContratoSubFianza(DataSet res1, string Ejecutivo, string rank, string clasi, string Ventas)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    XmlNode docNode = doc.CreateXmlDeclaration("1.0", null, null); //doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //    doc.AppendChild(docNode);

        //    XmlNode ValoresNode = doc.CreateElement("Valores");
        //    doc.AppendChild(ValoresNode);

        //    XmlNode RespNode;
        //    XmlNode root = doc.DocumentElement;
        //    RespNode = doc.CreateElement("Val");

        //    XmlNode RespNodeCkb;
        //    RespNodeCkb = doc.CreateElement("General");

        //    XmlNode nodo = doc.CreateElement("RazonSocial");
        //    nodo.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["RazonSocial"].ToString()));
        //    RespNodeCkb.AppendChild(nodo);

        //    XmlNode nodo1 = doc.CreateElement("Direccion");
        //    nodo1.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["direccion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo1);

        //    XmlNode nodo2 = doc.CreateElement("Comuna");
        //    nodo2.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescComuna"].ToString()));
        //    RespNodeCkb.AppendChild(nodo2);


        //    for (int i = 0; i < res1.Tables[3].Rows.Count; i++)
        //    {
        //        DateTime fecha = DateTime.MinValue;

        //        DateTime fechaAux = res1.Tables[3].Rows[i]["FechaCreacion"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(res1.Tables[3].Rows[i]["FechaCreacion"].ToString());

        //        if (fechaAux >= fecha)
        //        {
        //            XmlNode nodo122 = doc.CreateElement("Canal");
        //            nodo122.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Canal"].ToString()));
        //            RespNodeCkb.AppendChild(nodo122);
        //            fecha = res1.Tables[3].Rows[i]["FechaCreacion"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(res1.Tables[3].Rows[i]["FechaCreacion"].ToString());
        //        }
        //    }

        //    string aux = res1.Tables[0].Rows[0]["GIROACT"].ToString();
        //    XmlNode nodo3 = doc.CreateElement("Actividad");
        //    nodo3.AppendChild(doc.CreateTextNode(aux.Substring(2, aux.Length - 2)));//.Substring(2, aux.Length-1)));
        //    RespNodeCkb.AppendChild(nodo3);

        //    DateTime thisDate = DateTime.Now;
        //    CultureInfo culture = new CultureInfo("es-ES");

        //    XmlNode nodo4 = doc.CreateElement("FechaHoy");
        //    nodo4.AppendChild(doc.CreateTextNode(thisDate.ToString("D", culture)));
        //    RespNodeCkb.AppendChild(nodo4);

        //    XmlNode nodo5 = doc.CreateElement("RutEmpresa");
        //    nodo5.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["Rut"].ToString() + "-" + res1.Tables[0].Rows[0]["DivRut"].ToString()));
        //    RespNodeCkb.AppendChild(nodo5);

        //    XmlNode nodo6 = doc.CreateElement("Region");
        //    nodo6.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescRegion"].ToString()));
        //    RespNodeCkb.AppendChild(nodo6);

        //    XmlNode nodo7 = doc.CreateElement("Telefono");
        //    nodo7.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["TelFijo1"].ToString()));
        //    RespNodeCkb.AppendChild(nodo7);

        //    XmlNode nodo8 = doc.CreateElement("Ciudad");
        //    nodo8.AppendChild(doc.CreateTextNode(res1.Tables[0].Rows[0]["DescProvincia"].ToString()));
        //    RespNodeCkb.AppendChild(nodo8);

        //    XmlNode nodoA1 = doc.CreateElement("AnoAct");
        //    nodoA1.AppendChild(doc.CreateTextNode(DateTime.Now.Year.ToString()));
        //    RespNodeCkb.AppendChild(nodoA1);

        //    XmlNode nodoA2 = doc.CreateElement("Ano-1");
        //    nodoA2.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 1).ToString()));
        //    RespNodeCkb.AppendChild(nodoA2);

        //    XmlNode nodoA3 = doc.CreateElement("Ano-2");
        //    nodoA3.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 2).ToString()));
        //    RespNodeCkb.AppendChild(nodoA3);

        //    XmlNode nodoA4 = doc.CreateElement("Ano-3");
        //    nodoA4.AppendChild(doc.CreateTextNode((DateTime.Now.Year - 3).ToString()));
        //    RespNodeCkb.AppendChild(nodoA4);

        //    // asi con todos los nodos de3 este nievel
        //    RespNode.AppendChild(RespNodeCkb);
        //    ValoresNode.AppendChild(RespNode);

        //    //InsertarDatosPAF de la PAF
        //    XmlNode RespNodePropuestaAfianzamiento;
        //    //XmlNode root = doc.DocumentElement;
        //    RespNodePropuestaAfianzamiento = doc.CreateElement("PAF");

        //    XmlNode nodoP = doc.CreateElement("Oficina");
        //    nodoP.AppendChild(doc.CreateTextNode("1"));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP);

        //    XmlNode nodoP1 = doc.CreateElement("Fecha");
        //    nodoP1.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["fecha"].ToString()));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP1);

        //    XmlNode nodoP2 = doc.CreateElement("FechaRevision");
        //    nodoP2.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["FechaRevision"].ToString()));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP2);

        //    XmlNode nodoP3 = doc.CreateElement("Ejecutivo");
        //    nodoP3.AppendChild(doc.CreateTextNode(Ejecutivo));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP3);

        //    XmlNode nodoP4 = doc.CreateElement("EstadoLinea");
        //    nodoP4.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["EstadoLinea"].ToString()));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP4);

        //    XmlNode nodoP5 = doc.CreateElement("NivelAtribucion");
        //    nodoP5.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["NivelAtribucion"].ToString()));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP5);

        //    XmlNode nodoP6 = doc.CreateElement("ValorRank");
        //    nodoP6.AppendChild(doc.CreateTextNode(rank));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP6);

        //    XmlNode nodoP116 = doc.CreateElement("Clasificacion");
        //    nodoP116.AppendChild(doc.CreateTextNode(clasi));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP116);

        //    if (res1.Tables[5].Rows.Count > 0)
        //    {
        //        XmlNode nodoP7 = doc.CreateElement("ValorPond");
        //        nodoP7.AppendChild(doc.CreateTextNode(res1.Tables[5].Rows[0]["ValorPond"].ToString().Replace(".", ",")));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP7);

        //        XmlNode nodoP8 = doc.CreateElement("FechaScoring");
        //        nodoP8.AppendChild(doc.CreateTextNode((res1.Tables[5].Rows[0]["FecCreacion"].ToString())));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP8);
        //    }
        //    else
        //    {
        //        XmlNode nodoP7 = doc.CreateElement("ValorPond");
        //        nodoP7.AppendChild(doc.CreateTextNode(""));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP7);

        //        XmlNode nodoP8 = doc.CreateElement("FechaScoring");
        //        nodoP8.AppendChild(doc.CreateTextNode(("")));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP8);
        //    }

        //    XmlNode nodoP9 = doc.CreateElement("VentasMoviles");
        //    nodoP9.AppendChild(doc.CreateTextNode(Ventas));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP9);

        //    XmlNode nodoP10 = doc.CreateElement("ObservacionComite");
        //    nodoP10.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["ObservacionComite"].ToString()));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP10);

        //    XmlNode nodoP11 = doc.CreateElement("IdPaf");
        //    nodoP11.AppendChild(doc.CreateTextNode(res1.Tables[4].Rows[0]["IdPaf"].ToString()));
        //    RespNodePropuestaAfianzamiento.AppendChild(nodoP11);

        //    if (res1.Tables[6].Rows.Count > 0)
        //    {
        //        XmlNode nodoP12 = doc.CreateElement("Aprobador1");
        //        nodoP12.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador"].ToString()));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP12);

        //        XmlNode nodoP13 = doc.CreateElement("Aprobador2");
        //        nodoP13.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador1"].ToString()));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP13);

        //        XmlNode nodoP14 = doc.CreateElement("Aprobador3");
        //        nodoP14.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador2"].ToString()));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP14);

        //        XmlNode nodoP15 = doc.CreateElement("Aprobador4");
        //        nodoP15.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador3"].ToString()));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP15);

        //        XmlNode nodoP16 = doc.CreateElement("Aprobador5");
        //        nodoP16.AppendChild(doc.CreateTextNode(res1.Tables[6].Rows[0]["Aprobador4"].ToString()));
        //        RespNodePropuestaAfianzamiento.AppendChild(nodoP16);
        //    }
        //    ValoresNode.AppendChild(RespNodePropuestaAfianzamiento);

        //    //datos gaantia

        //    XmlNode RespNodeG;
        //    root = doc.DocumentElement;
        //    RespNodeG = doc.CreateElement("Garantias");
        //    double TotalGarantiaComercial = 0;
        //    double TotalGarantiaAjustado = 0;
        //    double TotalCoberturaAjustadoVigente = 0;
        //    double TotalCoberturaComercialVigente = 0;
        //    double TotalCoberturaAjustadoNuevo = 0;
        //    double TotalGarantiaNuevaComercial = 0;
        //    double coberturaCertificado = 0;

        //    for (int i = 0; i < res1.Tables[1].Rows.Count; i++)
        //    {
        //        if (int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 52 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 53 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 54 || int.Parse(res1.Tables[1].Rows[i]["IdTipoBien"].ToString()) != 59)
        //        {
        //            TotalGarantiaComercial = TotalGarantiaComercial + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
        //            TotalGarantiaAjustado = TotalGarantiaAjustado + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");

        //            if (res1.Tables[1].Rows[i]["DescEstado"].ToString() == "Constituida" || res1.Tables[1].Rows[i]["DescEstado"].ToString() == "Tramite")
        //            {
        //                TotalCoberturaAjustadoVigente = TotalCoberturaAjustadoVigente + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
        //                TotalCoberturaComercialVigente = TotalCoberturaComercialVigente + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
        //            }
        //            else
        //            {
        //                TotalCoberturaAjustadoNuevo = TotalCoberturaAjustadoNuevo + Double.Parse(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Ajustado"].ToString().Replace(".", "").Replace(",", ".")) : "0");
        //                TotalGarantiaNuevaComercial = TotalGarantiaNuevaComercial + Double.Parse(res1.Tables[1].Rows[i]["Valor Comercial"].ToString() != "" ? (res1.Tables[1].Rows[i]["Valor Comercial"].ToString().Replace(".", "").Replace(",", ".")) : "0");
        //            }
        //        }

        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Garantia");

        //        nodo = doc.CreateElement("N");
        //        nodo.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["N"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo);

        //        nodo1 = doc.CreateElement("TipoGarantia");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Tipo Garantia"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo1);

        //        nodo2 = doc.CreateElement("Descripcion");
        //        nodo2.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Descripción"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo2);

        //        nodo3 = doc.CreateElement("Comentarios");
        //        nodo3.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Comentarios"].ToString().Trim()));
        //        RespNodeGarantia.AppendChild(nodo3);

        //        nodo4 = doc.CreateElement("TipoMO");
        //        nodo4.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Tipo Moneda"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo4);

        //        nodo5 = doc.CreateElement("ValorComercial");
        //        nodo5.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Valor Comercial"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo5);

        //        nodo6 = doc.CreateElement("ValorAjustado");
        //        nodo6.AppendChild(doc.CreateTextNode(res1.Tables[1].Rows[i]["Valor Ajustado"].ToString()));
        //        RespNodeGarantia.AppendChild(nodo6);

        //        nodo7 = doc.CreateElement("AplicaSeguro");
        //        if (!string.IsNullOrEmpty(res1.Tables[1].Rows[i]["Seguro"].ToString()))
        //            nodo7.AppendChild(doc.CreateTextNode((Boolean)res1.Tables[1].Rows[i]["Seguro"] ? "Aplica" : "No Aplica"));
        //        else
        //            nodo7.AppendChild(doc.CreateTextNode("No se ha ingresado una opcion de seguro"));

        //        RespNodeGarantia.AppendChild(nodo7);

        //        RespNodeG.AppendChild(RespNodeGarantia);
        //    }
        //    ValoresNode.AppendChild(RespNodeG);


        //    //aval -----------------------------------
        //    XmlNode RespNodeA;
        //    root = doc.DocumentElement;
        //    RespNodeA = doc.CreateElement("OperacionesVigentes");

        //    double totalAprobadoCLP = 0;
        //    double totalAprobadoCLP_Certificado = 0;
        //    double totalVigenteCLP = 0;
        //    double totalVigenteCLP_Certificado = 0;
        //    double totalPropuestoCLP = 0;
        //    double totalPropuestoCLP_Certificado = 0;

        //    double totalAprobadoUF = 0;
        //    double totalAprobadoUF_Certificado = 0;
        //    double totalVigenteUF = 0;
        //    double totalVigenteUF_Certificado = 0;
        //    double totalPropuestoUF = 0;
        //    double totalPropuestoUF_Certificado = 0;

        //    double totalAprobadoUSD = 0;
        //    double totalAprobadoUSD_Certificado = 0;
        //    double totalVigenteUSD = 0;
        //    double totalVigenteUSD_Certificado = 0;
        //    double totalPropuestoUSD = 0;
        //    double totalPropuestoUSD_Certificado = 0;

        //    //OPERACIONES VIGENTES
        //    for (int i = 0; i <= res1.Tables[2].Rows.Count - 1; i++)
        //    {
        //        if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["CoberturaCertificado"].ToString()))
        //            coberturaCertificado = 1;
        //        else
        //            coberturaCertificado = double.Parse(res1.Tables[2].Rows[i]["CoberturaCertificado"].ToString()) / 100;

        //        if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "UF")
        //        {
        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
        //            {
        //                totalAprobadoUF = totalAprobadoUF + 0;
        //                totalAprobadoUF_Certificado = totalAprobadoUF_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalAprobadoUF_Certificado = totalAprobadoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //                totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
        //            {
        //                totalVigenteUF = totalVigenteUF + 0;
        //                totalVigenteUF_Certificado = totalVigenteUF_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalVigenteUF_Certificado = totalVigenteUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //                totalVigenteUF = totalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
        //            {
        //                totalPropuestoUF = totalPropuestoUF + 0;
        //                totalPropuestoUF_Certificado = totalPropuestoUF_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalPropuestoUF_Certificado = totalPropuestoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //                totalPropuestoUF = totalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
        //            }
        //        }

        //        if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "CLP")
        //        {
        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
        //            {
        //                totalAprobadoCLP = totalAprobadoCLP + 0;
        //                totalAprobadoCLP_Certificado = totalAprobadoCLP_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalAprobadoCLP = totalAprobadoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
        //                totalAprobadoCLP_Certificado = totalAprobadoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
        //            {
        //                totalVigenteCLP = totalVigenteCLP + 0;
        //                totalVigenteCLP_Certificado = totalVigenteCLP_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalVigenteCLP = totalVigenteCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
        //                totalVigenteCLP_Certificado = totalVigenteCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
        //            {
        //                totalPropuestoCLP = totalPropuestoCLP + 0;
        //                totalPropuestoCLP_Certificado = totalPropuestoCLP_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalPropuestoCLP = totalPropuestoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
        //                totalPropuestoCLP_Certificado = totalPropuestoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }
        //        }

        //        if (res1.Tables[2].Rows[i]["Tipo Moneda"].ToString() == "USD")
        //        {
        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()))
        //            {
        //                totalAprobadoUSD = totalAprobadoUSD + 0;
        //                totalAprobadoUSD_Certificado = totalAprobadoUSD_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalAprobadoUF = totalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
        //                totalAprobadoUSD_Certificado = totalAprobadoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()))
        //            {
        //                totalVigenteUSD = totalVigenteUSD + 0;
        //                totalVigenteUSD_Certificado = totalVigenteUSD_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalVigenteUSD = totalVigenteUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
        //                totalVigenteUSD_Certificado = totalVigenteUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()))
        //            {
        //                totalPropuestoUSD = totalPropuestoUSD + 0;
        //                totalPropuestoUSD_Certificado = totalPropuestoUSD_Certificado + 0;
        //            }
        //            else
        //            {
        //                totalPropuestoUSD = totalPropuestoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
        //                totalPropuestoUSD_Certificado = totalPropuestoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }
        //        }

        //        XmlNode RespNodeAval;
        //        RespNodeAval = doc.CreateElement("OperacionVigente");

        //        nodo = doc.CreateElement("N");
        //        nodo.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["N"].ToString()));
        //        RespNodeAval.AppendChild(nodo);

        //        nodo1 = doc.CreateElement("TipoFinanciamiento");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Tipo Financiamiento"].ToString()));
        //        RespNodeAval.AppendChild(nodo1);

        //        nodo2 = doc.CreateElement("Producto");
        //        nodo2.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Producto"].ToString()));
        //        RespNodeAval.AppendChild(nodo2);

        //        nodo3 = doc.CreateElement("Finalidad");
        //        nodo3.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Finalidad"].ToString()));
        //        RespNodeAval.AppendChild(nodo3);

        //        nodo4 = doc.CreateElement("Plazo");
        //        nodo4.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Plazo"].ToString()));
        //        RespNodeAval.AppendChild(nodo4);

        //        nodo5 = doc.CreateElement("Comision");
        //        nodo5.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Comisión Min. %"].ToString()));
        //        RespNodeAval.AppendChild(nodo5);

        //        nodo6 = doc.CreateElement("Seguro");
        //        nodo6.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Seguro"].ToString()));
        //        RespNodeAval.AppendChild(nodo6);

        //        nodo7 = doc.CreateElement("ComisionCLP");
        //        nodo7.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Comisión"].ToString()));
        //        RespNodeAval.AppendChild(nodo7);

        //        nodo8 = doc.CreateElement("MontoCredito");
        //        nodo8.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Crédito"].ToString()));
        //        RespNodeAval.AppendChild(nodo8);

        //        XmlNode nodo9 = doc.CreateElement("TipoMO");
        //        nodo9.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Tipo Moneda"].ToString()));
        //        RespNodeAval.AppendChild(nodo9);

        //        XmlNode nodo10 = doc.CreateElement("MontoAprobado");
        //        nodo10.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Aprobado"].ToString()));
        //        RespNodeAval.AppendChild(nodo10);

        //        XmlNode nodo11 = doc.CreateElement("MontoVigente");
        //        nodo11.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Vigente"].ToString()));
        //        RespNodeAval.AppendChild(nodo11);

        //        XmlNode nodo12 = doc.CreateElement("MontoPropuesto");
        //        nodo12.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Monto Propuesto"].ToString()));
        //        RespNodeAval.AppendChild(nodo12);

        //        XmlNode nodo13 = doc.CreateElement("Horizonte");
        //        nodo13.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["Horizonte"].ToString()));
        //        RespNodeAval.AppendChild(nodo13);

        //        XmlNode nodo14 = doc.CreateElement("CoberturaCertificado");
        //        nodo14.AppendChild(doc.CreateTextNode(res1.Tables[2].Rows[i]["coberturaCertificado"].ToString()));
        //        RespNodeAval.AppendChild(nodo14);

        //        RespNodeA.AppendChild(RespNodeAval);
        //    }
        //    ValoresNode.AppendChild(RespNodeA);

        //    XmlNode RespNodeB;
        //    root = doc.DocumentElement;
        //    RespNodeB = doc.CreateElement("OperacionesNuevas");

        //    double NtotalAprobadoCLP = 0;
        //    double NtotalAprobadoCLP_Certificado = 0;
        //    double NtotalVigenteCLP = 0;
        //    double NtotalVigenteCLP_Certificado = 0;
        //    double NtotalPropuestoCLP = 0;
        //    double NtotalPropuestoCLP_Certificado = 0;

        //    double NtotalAprobadoUF = 0;
        //    double NtotalAprobadoUF_Certificado = 0;
        //    double NtotalVigenteUF = 0;
        //    double NtotalVigenteUF_Certificado = 0;
        //    double NtotalPropuestoUF = 0;

        //    double NtotalAprobadoUSD = 0;
        //    double NtotalAprobadoUSD_Certificado = 0;
        //    double NtotalVigenteUSD = 0;
        //    double NtotalVigenteUSD_Certificado = 0;
        //    double NtotalPropuestoUSD = 0;
        //    double NtotalPropuestoUSD_Certificado = 0;

        //    //OPERACIONES NUEVAS
        //    for (int i = 0; i < res1.Tables[3].Rows.Count; i++)
        //    {
        //        if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["CoberturaCertificado"].ToString()))
        //            coberturaCertificado = 1;
        //        else
        //            coberturaCertificado = double.Parse(res1.Tables[3].Rows[i]["CoberturaCertificado"].ToString()) / 100;

        //        if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "UF")
        //        {
        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
        //            {
        //                NtotalAprobadoUF = NtotalAprobadoUF + 0;
        //                NtotalAprobadoUF_Certificado = NtotalAprobadoUF_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalAprobadoUF = NtotalAprobadoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalAprobadoUF_Certificado = NtotalAprobadoUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
        //            {
        //                NtotalVigenteUF = NtotalVigenteUF + 0;
        //                NtotalVigenteUF_Certificado = NtotalVigenteUF_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalVigenteUF = NtotalVigenteUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalVigenteUF_Certificado = NtotalVigenteUF_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
        //            {
        //                NtotalPropuestoUF = NtotalPropuestoUF + 0;
        //            }
        //            else
        //            {
        //                NtotalPropuestoUF = NtotalPropuestoUF + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
        //            }
        //        }

        //        if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "CLP")
        //        {
        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
        //            {
        //                NtotalAprobadoCLP = NtotalAprobadoCLP + 0;
        //                NtotalAprobadoCLP_Certificado = NtotalAprobadoCLP_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalAprobadoCLP = NtotalAprobadoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalAprobadoCLP_Certificado = NtotalAprobadoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
        //            {
        //                NtotalVigenteCLP = NtotalVigenteCLP + 0;
        //                NtotalVigenteCLP_Certificado = NtotalVigenteCLP_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalVigenteCLP = NtotalVigenteCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalVigenteCLP_Certificado = NtotalVigenteCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
        //            {
        //                NtotalPropuestoCLP = NtotalPropuestoCLP + 0;
        //                NtotalPropuestoCLP_Certificado = NtotalPropuestoCLP_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalPropuestoCLP = NtotalPropuestoCLP + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalPropuestoCLP_Certificado = NtotalPropuestoCLP_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }
        //        }

        //        if (res1.Tables[3].Rows[i]["Tipo Moneda"].ToString() == "USD")
        //        {
        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()))
        //            {
        //                NtotalAprobadoUSD = NtotalAprobadoUSD + 0;
        //                NtotalAprobadoUSD_Certificado = NtotalAprobadoUSD_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalAprobadoUSD = NtotalAprobadoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalAprobadoUSD_Certificado = NtotalAprobadoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()))
        //            {
        //                NtotalVigenteUSD = NtotalVigenteUSD + 0;
        //                NtotalVigenteUSD_Certificado = NtotalVigenteUSD_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalVigenteUSD = NtotalVigenteUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalVigenteUSD_Certificado = NtotalVigenteUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }

        //            if (string.IsNullOrEmpty(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()))
        //            {
        //                NtotalPropuestoUSD = NtotalPropuestoUSD + 0;
        //                NtotalPropuestoUSD_Certificado = NtotalPropuestoUSD_Certificado + 0;
        //            }
        //            else
        //            {
        //                NtotalPropuestoUSD = NtotalPropuestoUSD + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", ".")));
        //                NtotalPropuestoUSD_Certificado = NtotalPropuestoUSD_Certificado + Convert.ToDouble(System.Web.HttpUtility.HtmlDecode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString().Replace(".", "").Replace(",", "."))) * coberturaCertificado;
        //            }
        //        }

        //        XmlNode RespNodeAval;
        //        RespNodeAval = doc.CreateElement("OperacionNueva");

        //        nodo = doc.CreateElement("N");
        //        nodo.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["N"].ToString()));
        //        RespNodeAval.AppendChild(nodo);

        //        nodo1 = doc.CreateElement("TipoFinanciamiento");
        //        nodo1.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Tipo Financiamiento"].ToString()));
        //        RespNodeAval.AppendChild(nodo1);

        //        nodo2 = doc.CreateElement("Producto");
        //        nodo2.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Producto"].ToString()));
        //        RespNodeAval.AppendChild(nodo2);

        //        nodo3 = doc.CreateElement("Finalidad");
        //        nodo3.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Finalidad"].ToString()));
        //        RespNodeAval.AppendChild(nodo3);

        //        nodo4 = doc.CreateElement("Plazo");
        //        nodo4.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Plazo"].ToString()));
        //        RespNodeAval.AppendChild(nodo4);

        //        nodo5 = doc.CreateElement("Comision");
        //        nodo5.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Comisión Min. %"].ToString()));
        //        RespNodeAval.AppendChild(nodo5);

        //        nodo6 = doc.CreateElement("Seguro");
        //        nodo6.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Seguro"].ToString()));
        //        RespNodeAval.AppendChild(nodo6);

        //        nodo7 = doc.CreateElement("ComisionCLP");
        //        nodo7.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Comisión"].ToString()));
        //        RespNodeAval.AppendChild(nodo7);

        //        nodo8 = doc.CreateElement("MontoCredito");
        //        nodo8.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Crédito"].ToString()));
        //        RespNodeAval.AppendChild(nodo8);

        //        XmlNode nodo9 = doc.CreateElement("TipoMO");
        //        nodo9.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Tipo Moneda"].ToString()));
        //        RespNodeAval.AppendChild(nodo9);

        //        XmlNode nodo10 = doc.CreateElement("MontoAprobado");
        //        nodo10.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Aprobado"].ToString()));
        //        RespNodeAval.AppendChild(nodo10);

        //        XmlNode nodo11 = doc.CreateElement("MontoVigente");
        //        nodo11.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Vigente"].ToString()));
        //        RespNodeAval.AppendChild(nodo11);

        //        XmlNode nodo12 = doc.CreateElement("MontoPropuesto");
        //        nodo12.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Monto Propuesto"].ToString()));
        //        RespNodeAval.AppendChild(nodo12);

        //        XmlNode nodo13 = doc.CreateElement("Horizonte");
        //        nodo13.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["Horizonte"].ToString()));
        //        RespNodeAval.AppendChild(nodo13);

        //        XmlNode nodo14 = doc.CreateElement("CoberturaCertificado");
        //        nodo14.AppendChild(doc.CreateTextNode(res1.Tables[3].Rows[i]["coberturaCertificado"].ToString().Trim()));
        //        RespNodeAval.AppendChild(nodo14);

        //        RespNodeB.AppendChild(RespNodeAval);
        //    }
        //    ValoresNode.AppendChild(RespNodeB);
        //    //tablas indicadores

        //    XmlNode RespNodeI1;
        //    root = doc.DocumentElement;
        //    RespNodeI1 = doc.CreateElement("Indicadores1");

        //    for (int i = 0; i < res1.Tables[7].Rows.Count; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Indicador1");

        //        nodo = doc.CreateElement("VAL1");
        //        nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo);

        //        nodo1 = doc.CreateElement("POR1");
        //        nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo1);

        //        nodo2 = doc.CreateElement("VAL2");
        //        nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo2);

        //        nodo3 = doc.CreateElement("POR2");
        //        nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo3);

        //        nodo4 = doc.CreateElement("VAL3");
        //        nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo4);

        //        nodo5 = doc.CreateElement("POR3");
        //        nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo5);

        //        nodo6 = doc.CreateElement("VAL4");
        //        nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo6);

        //        nodo7 = doc.CreateElement("POR4");
        //        nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[7].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo7);

        //        nodo8 = doc.CreateElement("TEXTO");
        //        nodo8.AppendChild(doc.CreateTextNode((res1.Tables[7].Rows[i]["Cuenta"].ToString())));
        //        RespNodeGarantia.AppendChild(nodo8);

        //        RespNodeI1.AppendChild(RespNodeGarantia);
        //    }
        //    ValoresNode.AppendChild(RespNodeI1);

        //    XmlNode RespNodeI2;
        //    root = doc.DocumentElement;
        //    RespNodeI2 = doc.CreateElement("Indicadores2");

        //    for (int i = 0; i < res1.Tables[8].Rows.Count; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Indicador2");

        //        nodo = doc.CreateElement("VAL1");
        //        nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo);

        //        nodo1 = doc.CreateElement("POR1");
        //        nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo1);

        //        nodo2 = doc.CreateElement("VAL2");
        //        nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo2);

        //        nodo3 = doc.CreateElement("POR2");
        //        nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo3);

        //        nodo4 = doc.CreateElement("VAL3");
        //        nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo4);

        //        nodo5 = doc.CreateElement("POR3");
        //        nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo5);

        //        nodo6 = doc.CreateElement("VAL4");
        //        nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo6);

        //        nodo7 = doc.CreateElement("POR4");
        //        nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[8].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo7);

        //        nodo8 = doc.CreateElement("TEXTO");
        //        nodo8.AppendChild(doc.CreateTextNode((res1.Tables[8].Rows[i]["Cuenta"].ToString())));
        //        RespNodeGarantia.AppendChild(nodo8);

        //        RespNodeI2.AppendChild(RespNodeGarantia);
        //    }
        //    ValoresNode.AppendChild(RespNodeI2);

        //    XmlNode RespNodeI3;
        //    root = doc.DocumentElement;
        //    RespNodeI3 = doc.CreateElement("Indicadores3");

        //    for (int i = 0; i < res1.Tables[9].Rows.Count; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Indicador3");

        //        nodo = doc.CreateElement("VAL1");
        //        nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo);

        //        nodo1 = doc.CreateElement("POR1");
        //        nodo1.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo1);

        //        nodo2 = doc.CreateElement("VAL2");
        //        nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo2);

        //        nodo3 = doc.CreateElement("POR2");
        //        nodo3.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo3);

        //        nodo4 = doc.CreateElement("VAL3");
        //        nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo4);

        //        nodo5 = doc.CreateElement("POR3");
        //        nodo5.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo5);

        //        nodo6 = doc.CreateElement("VAL4");
        //        nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo6);

        //        nodo7 = doc.CreateElement("POR4");
        //        nodo7.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[9].Rows[i]["POR4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo7);

        //        nodo8 = doc.CreateElement("TEXTO");
        //        nodo8.AppendChild(doc.CreateTextNode((res1.Tables[9].Rows[i]["Cuenta"].ToString())));
        //        RespNodeGarantia.AppendChild(nodo8);

        //        RespNodeI3.AppendChild(RespNodeGarantia);
        //    }
        //    ValoresNode.AppendChild(RespNodeI3);

        //    XmlNode RespNodeI4;
        //    root = doc.DocumentElement;
        //    RespNodeI4 = doc.CreateElement("Indicadores4");

        //    for (int i = 0; i < res1.Tables[10].Rows.Count; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Indicador4");
        //        if (res1.Tables[10].Rows[i]["Cuenta"].ToString() != "Capital de Trabajo (M$)")
        //        {
        //            nodo = doc.CreateElement("VAL1");
        //            nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo);

        //            nodo2 = doc.CreateElement("VAL2");
        //            nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo2);

        //            nodo4 = doc.CreateElement("VAL3");
        //            nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo4);

        //            nodo6 = doc.CreateElement("VAL4");
        //            nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo6);

        //            nodo8 = doc.CreateElement("TEXTO");
        //            nodo8.AppendChild(doc.CreateTextNode((res1.Tables[10].Rows[i]["Cuenta"].ToString())));
        //            RespNodeGarantia.AppendChild(nodo8);
        //        }
        //        else
        //        {
        //            nodo = doc.CreateElement("VAL1");
        //            nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL1"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo);

        //            nodo2 = doc.CreateElement("VAL2");
        //            nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL2"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo2);

        //            nodo4 = doc.CreateElement("VAL3");
        //            nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL3"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo4);

        //            nodo6 = doc.CreateElement("VAL4");
        //            nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[10].Rows[i]["VAL4"].ToString()).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //            RespNodeGarantia.AppendChild(nodo6);

        //            nodo8 = doc.CreateElement("TEXTO");
        //            nodo8.AppendChild(doc.CreateTextNode((res1.Tables[10].Rows[i]["Cuenta"].ToString())));
        //            RespNodeGarantia.AppendChild(nodo8);
        //        }
        //        RespNodeI4.AppendChild(RespNodeGarantia);
        //    }
        //    ValoresNode.AppendChild(RespNodeI4);

        //    XmlNode RespNodeI5;
        //    root = doc.DocumentElement;
        //    RespNodeI5 = doc.CreateElement("Indicadores5");

        //    for (int i = 0; i < res1.Tables[11].Rows.Count; i++)
        //    {
        //        XmlNode RespNodeGarantia;
        //        RespNodeGarantia = doc.CreateElement("Indicador5");

        //        nodo = doc.CreateElement("VAL1");
        //        nodo.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL1"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo);

        //        nodo2 = doc.CreateElement("VAL2");
        //        nodo2.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL2"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo2);

        //        nodo4 = doc.CreateElement("VAL3");
        //        nodo4.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL3"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo4);

        //        nodo6 = doc.CreateElement("VAL4");
        //        nodo6.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[11].Rows[i]["VAL4"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodeGarantia.AppendChild(nodo6);

        //        nodo8 = doc.CreateElement("TEXTO");
        //        nodo8.AppendChild(doc.CreateTextNode((res1.Tables[11].Rows[i]["Cuenta"].ToString())));
        //        RespNodeGarantia.AppendChild(nodo8);

        //        RespNodeI5.AppendChild(RespNodeGarantia);
        //    }
        //    ValoresNode.AppendChild(RespNodeI5);


        //    //TOTALES de la PAF
        //    XmlNode RespNodePAF;

        //    RespNodePAF = doc.CreateElement("TOTALESPAF");
        //    //operaciones vigentes

        //    XmlNode nodoPT = doc.CreateElement("totalAprobadoCLP");
        //    nodoPT.AppendChild(doc.CreateTextNode(totalAprobadoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT);

        //    XmlNode nodoPT1 = doc.CreateElement("totalVigenteCLP");
        //    nodoPT1.AppendChild(doc.CreateTextNode(totalVigenteCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT1);

        //    XmlNode nodoPT2 = doc.CreateElement("totalPropuestoCLP");
        //    nodoPT2.AppendChild(doc.CreateTextNode(totalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT2);

        //    XmlNode nodoPT3 = doc.CreateElement("totalAprobadoUF");
        //    nodoPT3.AppendChild(doc.CreateTextNode(totalAprobadoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT3);

        //    XmlNode nodoPT4 = doc.CreateElement("totalVigenteUF");
        //    nodoPT4.AppendChild(doc.CreateTextNode(totalVigenteUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT4);

        //    if (!string.IsNullOrEmpty(res1.Tables[4].Rows[0]["ValorUF"].ToString()))
        //    {
        //        XmlNode nodoPT5 = doc.CreateElement("totalPropuestoUF");
        //        nodoPT5.AppendChild(doc.CreateTextNode((totalPropuestoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //        RespNodePAF.AppendChild(nodoPT5);
        //    }
        //    else
        //    {
        //        XmlNode nodoPT5 = doc.CreateElement("totalPropuestoUF");
        //        nodoPT5.AppendChild(doc.CreateTextNode("0"));
        //        RespNodePAF.AppendChild(nodoPT5);
        //    }

        //    XmlNode nodoPT6 = doc.CreateElement("totalAprobadoUSD");
        //    nodoPT6.AppendChild(doc.CreateTextNode(totalAprobadoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT6);

        //    XmlNode nodoPT7 = doc.CreateElement("totalVigenteUSD");
        //    nodoPT7.AppendChild(doc.CreateTextNode(totalVigenteUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT7);

        //    XmlNode nodoPT8 = doc.CreateElement("totalPropuestoUSD");
        //    nodoPT8.AppendChild(doc.CreateTextNode(totalPropuestoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT8);

        //    XmlNode nodoPT88 = doc.CreateElement("ValorUF");
        //    nodoPT88.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT88);

        //    XmlNode nodoPT89 = doc.CreateElement("ValorDolar");
        //    nodoPT89.AppendChild(doc.CreateTextNode(float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString()).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT89);

        //    //calculo RE
        //    double totalAprobadoREUF = totalAprobadoCLP > 0 ? (totalAprobadoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
        //    totalAprobadoREUF = totalAprobadoUSD > 0 ? ((totalAprobadoUSD * (float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString()) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString()))) + totalAprobadoREUF) : totalAprobadoREUF;
        //    totalAprobadoREUF = totalAprobadoREUF + totalAprobadoUF;

        //    double totalVigenteREUF = totalVigenteCLP > 0 ? (totalVigenteCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
        //    totalVigenteREUF = totalVigenteUSD > 0 ? ((totalVigenteUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + totalVigenteREUF : totalVigenteREUF;
        //    totalVigenteREUF = totalVigenteREUF + totalVigenteUF;

        //    double totalPropuestoREUF = totalPropuestoCLP > 0 ? (totalPropuestoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
        //    totalPropuestoREUF = totalPropuestoUSD > 0 ? ((totalPropuestoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + totalPropuestoREUF : totalPropuestoREUF;
        //    totalPropuestoREUF = totalPropuestoREUF + totalPropuestoUF;

        //    XmlNode nodoPT9 = doc.CreateElement("totalAprobadoREUF");
        //    nodoPT9.AppendChild(doc.CreateTextNode(totalAprobadoREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT9);

        //    XmlNode nodoPT10 = doc.CreateElement("totalVigenteREUF");
        //    nodoPT10.AppendChild(doc.CreateTextNode(totalVigenteREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT10);

        //    XmlNode nodoPT11 = doc.CreateElement("totalPropuestoREUF");
        //    nodoPT11.AppendChild(doc.CreateTextNode((totalPropuestoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoPT11);

        //    //OperacionesNuevas
        //    XmlNode nodoNT = doc.CreateElement("NtotalAprobadoCLP");
        //    nodoNT.AppendChild(doc.CreateTextNode(NtotalAprobadoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT);

        //    XmlNode nodoNT1 = doc.CreateElement("NtotalVigenteCLP");
        //    nodoNT1.AppendChild(doc.CreateTextNode(NtotalVigenteCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT1);

        //    XmlNode nodoNT2 = doc.CreateElement("NtotalPropuestoCLP");
        //    nodoNT2.AppendChild(doc.CreateTextNode(NtotalPropuestoCLP.ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT2);

        //    XmlNode nodoNT3 = doc.CreateElement("NtotalAprobadoUF");
        //    nodoNT3.AppendChild(doc.CreateTextNode(NtotalAprobadoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT3);

        //    XmlNode nodoNT4 = doc.CreateElement("NtotalVigenteUF");
        //    nodoNT4.AppendChild(doc.CreateTextNode(NtotalVigenteUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT4);

        //    XmlNode nodoNT5 = doc.CreateElement("NtotalPropuestoUF");
        //    nodoNT5.AppendChild(doc.CreateTextNode(NtotalPropuestoUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT5);

        //    XmlNode nodoNT6 = doc.CreateElement("NtotalAprobadoUSD");
        //    nodoNT6.AppendChild(doc.CreateTextNode(NtotalAprobadoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT6);

        //    XmlNode nodoNT7 = doc.CreateElement("NtotalVigenteUSD");
        //    nodoNT7.AppendChild(doc.CreateTextNode(NtotalVigenteUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT7);

        //    XmlNode nodoNT8 = doc.CreateElement("NtotalPropuestoUSD");
        //    nodoNT8.AppendChild(doc.CreateTextNode(NtotalPropuestoUSD.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT8);

        //    //calculos montos totales operaciones vigentes
        //    double NtotalAprobadoREUF = NtotalAprobadoCLP > 0 ? (NtotalAprobadoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
        //    NtotalAprobadoREUF = NtotalAprobadoUSD > 0 ? ((NtotalAprobadoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalAprobadoREUF : NtotalAprobadoREUF;
        //    NtotalAprobadoREUF = NtotalAprobadoUF + NtotalAprobadoREUF;

        //    double NtotalVigenteREUF = NtotalVigenteCLP > 0 ? (NtotalVigenteCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
        //    NtotalVigenteREUF = NtotalVigenteUSD > 0 ? ((NtotalVigenteUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalVigenteREUF : NtotalVigenteREUF;
        //    NtotalVigenteREUF = NtotalVigenteUF + NtotalVigenteREUF;

        //    double NtotalPropuestoREUF = NtotalPropuestoCLP > 0 ? (NtotalPropuestoCLP / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) : 0;
        //    NtotalPropuestoREUF = NtotalPropuestoUSD > 0 ? ((NtotalPropuestoUSD * float.Parse(res1.Tables[4].Rows[0]["ValorDolar"].ToString())) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())) + NtotalPropuestoREUF : NtotalPropuestoREUF;
        //    NtotalPropuestoREUF = NtotalPropuestoREUF + NtotalPropuestoUF;

        //    XmlNode nodoNT9 = doc.CreateElement("NtotalAprobadoREUF");
        //    nodoNT9.AppendChild(doc.CreateTextNode((NtotalAprobadoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT9);

        //    XmlNode nodoNT10 = doc.CreateElement("NtotalVigenteREUF");
        //    nodoNT10.AppendChild(doc.CreateTextNode(NtotalVigenteREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT10);

        //    XmlNode nodoNT11 = doc.CreateElement("NtotalPropuestoREUF");
        //    nodoNT11.AppendChild(doc.CreateTextNode(NtotalPropuestoREUF.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoNT11);

        //    //TotalLineaGlobal
        //    XmlNode nodoTGlobal = doc.CreateElement("GlobalCLPAprobado");
        //    nodoTGlobal.AppendChild(doc.CreateTextNode((NtotalAprobadoCLP + totalAprobadoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal);

        //    XmlNode nodoTGlobal1 = doc.CreateElement("GlobalCLPVigente");
        //    nodoTGlobal1.AppendChild(doc.CreateTextNode((NtotalVigenteCLP + totalVigenteCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal1);

        //    XmlNode nodoTGlobal2 = doc.CreateElement("GlobalCLPPropuesto");
        //    nodoTGlobal2.AppendChild(doc.CreateTextNode((NtotalPropuestoCLP + totalPropuestoCLP).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal2);

        //    XmlNode nodoTGlobal3 = doc.CreateElement("GlobalUFAprobado");
        //    nodoTGlobal3.AppendChild(doc.CreateTextNode((NtotalAprobadoUF + totalAprobadoUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal3);

        //    XmlNode nodoTGlobal4 = doc.CreateElement("GlobalUFVigente");
        //    nodoTGlobal4.AppendChild(doc.CreateTextNode((NtotalVigenteUF + totalVigenteUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal4);

        //    XmlNode nodoTGlobal5 = doc.CreateElement("GlobalUFPropuesto");
        //    nodoTGlobal5.AppendChild(doc.CreateTextNode(((NtotalPropuestoUF + totalPropuestoUF) / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal5);

        //    XmlNode nodoTGlobal6 = doc.CreateElement("GlobalUSDAprobado");
        //    nodoTGlobal6.AppendChild(doc.CreateTextNode((NtotalAprobadoUSD + totalAprobadoUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal6);

        //    XmlNode nodoTGlobal7 = doc.CreateElement("GlobalUSDPropuesto");
        //    nodoTGlobal7.AppendChild(doc.CreateTextNode((NtotalPropuestoUSD + totalPropuestoUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal7);

        //    XmlNode nodoTGlobal8 = doc.CreateElement("GlobalUSDVigente");
        //    nodoTGlobal8.AppendChild(doc.CreateTextNode((NtotalVigenteUSD + totalVigenteUSD).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal8);

        //    XmlNode nodoTGlobal9 = doc.CreateElement("GlobalREUFAprobado");
        //    nodoTGlobal9.AppendChild(doc.CreateTextNode((NtotalAprobadoREUF + totalAprobadoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal9);

        //    XmlNode nodoTGlobal10 = doc.CreateElement("GlobalREUFVigente");
        //    nodoTGlobal10.AppendChild(doc.CreateTextNode((NtotalVigenteREUF + totalVigenteREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal10);

        //    XmlNode nodoTGlobal11 = doc.CreateElement("GlobalREUFPropuesto");
        //    nodoTGlobal11.AppendChild(doc.CreateTextNode((NtotalPropuestoREUF + totalPropuestoREUF).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTGlobal11);

        //    //garantias
        //    XmlNode nodoTG = doc.CreateElement("TotalGarantiaComercial");
        //    nodoTG.AppendChild(doc.CreateTextNode((TotalGarantiaComercial * float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTG);

        //    XmlNode nodoTG1 = doc.CreateElement("TotalGarantiaAjustado");
        //    nodoTG1.AppendChild(doc.CreateTextNode((TotalGarantiaAjustado * float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString())).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTG1);

        //    XmlNode nodoTG22 = doc.CreateElement("TotalGarantiaComercialUF");
        //    nodoTG22.AppendChild(doc.CreateTextNode((TotalGarantiaComercial).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTG22);

        //    XmlNode nodoTG33 = doc.CreateElement("TotalGarantiaAjustadoUF");
        //    nodoTG33.AppendChild(doc.CreateTextNode((TotalGarantiaAjustado).ToString("N0", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    RespNodePAF.AppendChild(nodoTG33);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    //totales vigente garantia COBERTURA VIGENTE
        //    XmlNode nodoTG2 = doc.CreateElement("TotalComercialCV");
        //    if ((NtotalVigenteREUF + totalVigenteREUF) > 0 && TotalCoberturaComercialVigente > 0)
        //    {
        //        var b = totalPropuestoUF_Certificado;
        //        double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());

        //        var a = totalVigenteCLP_Certificado;
        //        var bb = totalVigenteREUF;

        //        nodoTG2.AppendChild(doc.CreateTextNode((100.0 * TotalCoberturaComercialVigente / (totalVigenteREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    }
        //    else nodoTG2.AppendChild(doc.CreateTextNode("0,00"));

        //    RespNodePAF.AppendChild(nodoTG2);

        //    XmlNode nodoTG3 = doc.CreateElement("TotalaAjustadoCV");
        //    if ((NtotalVigenteREUF + totalVigenteREUF) > 0 && TotalCoberturaAjustadoVigente > 0)
        //    {
        //        var b = totalPropuestoUF_Certificado;
        //        double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
        //        nodoTG3.AppendChild(doc.CreateTextNode((100.0 * TotalCoberturaAjustadoVigente / (totalVigenteREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    }
        //    else nodoTG3.AppendChild(doc.CreateTextNode("0,00"));
        //    RespNodePAF.AppendChild(nodoTG3);

        //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    //totales Globales garantia COBERTURA GLOBAL
        //    var A = TotalCoberturaAjustadoNuevo;
        //    var B = TotalGarantiaNuevaComercial;
        //    XmlNode nodoTG4 = doc.CreateElement("TotalComercialCG");
        //    if ((NtotalPropuestoREUF + totalPropuestoREUF) > 0.0 && TotalGarantiaComercial > 0.0)
        //    {
        //        double NclpCertificado = NtotalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
        //        double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
        //        nodoTG4.AppendChild(doc.CreateTextNode((100.0 * TotalGarantiaComercial / (NtotalPropuestoREUF + totalPropuestoREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    }
        //    else nodoTG4.AppendChild(doc.CreateTextNode("0,00"));
        //    RespNodePAF.AppendChild(nodoTG4);

        //    XmlNode nodoTG5 = doc.CreateElement("TotalAjustadoCG");
        //    if ((NtotalPropuestoREUF + totalPropuestoREUF) > 0.0 && TotalGarantiaAjustado > 0.0)
        //    {
        //        double NclpCertificado = NtotalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
        //        double clpCertificado = totalPropuestoCLP_Certificado / float.Parse(res1.Tables[4].Rows[0]["ValorUF"].ToString());
        //        nodoTG5.AppendChild(doc.CreateTextNode((100.0 * TotalGarantiaAjustado / (NtotalPropuestoREUF + totalPropuestoREUF)).ToString("N", CultureInfo.CreateSpecificCulture("es-ES"))));
        //    }
        //    else nodoTG5.AppendChild(doc.CreateTextNode("0,00"));
        //    RespNodePAF.AppendChild(nodoTG5);


        //    ValoresNode.AppendChild(RespNodePAF);
        //    return doc.OuterXml;
        //}

        #endregion

        #region "wpMantenimientoRiesgo"

        public DataSet ListarMantenimientoRiesgo(string razonSocial, string rut, string descEjecutivo, string Perfil, string Area)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@FecInicioActEco", SqlDbType.NVarChar);
                SqlParametros[0].Value = " ";
                SqlParametros[1] = new SqlParameter("@RazonSocial", SqlDbType.NVarChar);
                SqlParametros[1].Value = razonSocial;
                SqlParametros[2] = new SqlParameter("@Rut", SqlDbType.NChar);
                SqlParametros[2].Value = rut;
                SqlParametros[3] = new SqlParameter("@DescEjecutivo", SqlDbType.NVarChar);
                SqlParametros[3].Value = descEjecutivo;
                SqlParametros[4] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[4].Value = Perfil;
                SqlParametros[5] = new SqlParameter("@Etapa", SqlDbType.NVarChar);
                SqlParametros[5].Value = Area;

                dt = AD_Global.ejecutarConsulta("ListarMantenimientoRiesgo", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        #endregion

        #region "wpIvaVenta"

        public DataTable ListarIVAVentas(int idEmpresa, int Anio, int IdDocumentoContable)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@Anio", SqlDbType.Int);
                SqlParametros[1].Value = Anio;
                SqlParametros[2] = new SqlParameter("@IdDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = IdDocumentoContable;
                dt = AD_Global.ejecutarConsultas("ConsultarIVA", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        #endregion

        #region "wpVaciadoEstadoResultado"

        public Boolean finalizarEstadoResultado(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, int validacion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[6];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[3].Value = validacion;

                SqlParametros[4] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[4].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[5] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[5].Value = xmlData;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarEstadoResultado(string idEmpresa, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACIONINSERT;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;

                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean InsertarFinalizarEstado(string idEmpresa, string xmlData)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;

                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.INSERTAR;
                SqlParametros[3] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[3].Value = xmlData;

                SqlParametros[4] = new SqlParameter("@validacion", SqlDbType.Int);
                SqlParametros[4].Value = Constantes.RIESGO.VALIDACION;

                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable ListarEstadoResultado(string idEmpresa)
        {

            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@idDocumentosContables", SqlDbType.Int);
                SqlParametros[0].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;
                SqlParametros[2] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[2].Value = Constantes.OPCION.LISTAR;
                dt = AD_Global.ejecutarConsultas("GeneracionBalance", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public Boolean ModificarEstadoResultado(string idEmpresa, string idEmpresaDocumentoContable, string xmlData, string user, string comentario)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[7];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = int.Parse(idEmpresa);
                SqlParametros[1] = new SqlParameter("@idDocumentoContable", SqlDbType.Int);
                SqlParametros[1].Value = Constantes.DOCUMENTOSCONTABLE.ESTADORESULTADOS;

                SqlParametros[2] = new SqlParameter("@IdEmpresaDocumentoContable", SqlDbType.Int);
                SqlParametros[2].Value = int.Parse(idEmpresaDocumentoContable);

                SqlParametros[3] = new SqlParameter("@opcion", SqlDbType.NChar);
                SqlParametros[3].Value = Constantes.OPCION.MODIFICAR;
                SqlParametros[4] = new SqlParameter("@xmlData", SqlDbType.NVarChar);
                SqlParametros[4].Value = xmlData;
                SqlParametros[5] = new SqlParameter("@user", SqlDbType.NChar);
                SqlParametros[5].Value = user;
                SqlParametros[6] = new SqlParameter("@comentario", SqlDbType.NChar);
                SqlParametros[6].Value = comentario;
                return AD_Global.ejecutarAccion("GuardarDocumentosContables", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }
    
        #endregion

        #region "BitacoraCobranza"

        public DataTable ListarBitacoraCobranza(string mora, string ejecutivo, string acreedor, string empresa)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@categoria", SqlDbType.NVarChar);
                SqlParametros[0].Value = mora;
                SqlParametros[1] = new SqlParameter("@ejecutivo", SqlDbType.NVarChar);
                SqlParametros[1].Value = ejecutivo;
                SqlParametros[2] = new SqlParameter("@acreedor", SqlDbType.NVarChar);
                SqlParametros[2].Value = acreedor;
                SqlParametros[3] = new SqlParameter("@empresa", SqlDbType.NVarChar);
                SqlParametros[3].Value = empresa;

                return AD_Global.ejecutarConsultas("ListarBitacoraCobranza", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
        }


        //public DataTable ListarSms(string Ncertificado)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[1];
        //        SqlParametros[0] = new SqlParameter("@Ncertificado", SqlDbType.NVarChar);
        //        SqlParametros[0].Value = Ncertificado;

        //        return AD_Global.ejecutarConsultas("ListarSms", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return dt;
        //    }
        //}

        #endregion

        #region "DetalleCarteraMorosos"

        public DataSet CargarDetalleCertificado(int idOperacion, int idEmpresa)
        {
            try
            {
                DataSet dt;
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@idOperacion", SqlDbType.Int);
                SqlParametros[0].Value = idOperacion;
                SqlParametros[1] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = idEmpresa;

                dt = AD_Global.ejecutarConsulta("CargarDetalleCertificado", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        //public Boolean GestionBitacoraCobranza(int idEmpresa, int idOperacion, string NroCertificado, string Comentario, int IdAccionCobranza, DateTime FechaGestion, string MoraProyectada, bool Reprogramacion, string Usuario, int opcion)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[10];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[1].Value = idOperacion;
        //        SqlParametros[2] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
        //        SqlParametros[2].Value = NroCertificado;
        //        SqlParametros[3] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = Comentario;
        //        SqlParametros[4] = new SqlParameter("@IdAccionCobranza", SqlDbType.Int);
        //        SqlParametros[4].Value = IdAccionCobranza;
        //        SqlParametros[5] = new SqlParameter("@FechaGestion", SqlDbType.DateTime);
        //        SqlParametros[5].Value = FechaGestion;
        //        SqlParametros[6] = new SqlParameter("@MoraProyectada", SqlDbType.NVarChar);
        //        SqlParametros[6].Value = MoraProyectada;
        //        SqlParametros[7] = new SqlParameter("@Reprogramacion", SqlDbType.Bit);
        //        SqlParametros[7].Value = Reprogramacion;
        //        SqlParametros[8] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
        //        SqlParametros[8].Value = Usuario;
        //        SqlParametros[9] = new SqlParameter("@Opcion", SqlDbType.Int);
        //        SqlParametros[9].Value = opcion;
        //        return AD_Global.ejecutarAccion("GestionBitacoraCobranza", SqlParametros);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return false;
        //    }
        //}

        //public DataTable ListarGestionBitacoraCobranza(int idEmpresa, int idOperacion, string NroCertificado, string Comentario, DateTime? FechaGestion, string MoraProyectada, bool? Reprogramacion, string Usuario, int opcion)
        //{
        //    try
        //    {
        //        SqlParameter[] SqlParametros = new SqlParameter[9];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = idEmpresa;
        //        SqlParametros[1] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[1].Value = idOperacion;
        //        SqlParametros[2] = new SqlParameter("@NroCertificado", SqlDbType.NVarChar);
        //        SqlParametros[2].Value = NroCertificado;
        //        SqlParametros[3] = new SqlParameter("@Comentario", SqlDbType.NVarChar);
        //        SqlParametros[3].Value = Comentario;
        //        SqlParametros[4] = new SqlParameter("@FechaGestion", SqlDbType.DateTime);
        //        SqlParametros[4].Value = FechaGestion;
        //        SqlParametros[5] = new SqlParameter("@Usuario", SqlDbType.NVarChar);
        //        SqlParametros[5].Value = Usuario;
        //        SqlParametros[6] = new SqlParameter("@MoraProyectada", SqlDbType.NVarChar);
        //        SqlParametros[6].Value = MoraProyectada;
        //        SqlParametros[7] = new SqlParameter("@Reprogramacion", SqlDbType.Bit);
        //        SqlParametros[7].Value = Reprogramacion;
        //        SqlParametros[8] = new SqlParameter("@Opcion", SqlDbType.Int);
        //        SqlParametros[8].Value = opcion;
        //        return AD_Global.ejecutarConsultas("GestionBitacoraCobranza", SqlParametros);
        //    }
        //    catch
        //    {
        //        return new DataTable();
        //    }
        //}

        #endregion

       
        public DataSet TraeDatosEmpresas(int idEmpresa, bool activo)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[2];
                sqlParam[0] = new SqlParameter("@idEmpresa", idEmpresa);
                sqlParam[1] = new SqlParameter("@habilitado", activo);
                return AD_Global.ejecutarConsulta("ListaDatosEmpresas", sqlParam);
            }
            catch
            {
                return null;
            }
        }

        public string IngresaActualizaEmpresa(decimal rut, string razonSocial, int idEjecutivo, string descEjecutivo, int numEmpleados, int idTipoEmpresa
                 , string tipoEmpresa, int idActividad, string actividad, string telefono1, string email, int idMotivoBloqueo, string descMotivoBloqueo, string accion
                 , int idEmpresa, string usuario, int idUsuario, int validacion, bool habEmpresa, bool habDirEmpresa, DateTime fecIniAct, int idAsistente, string descAsistente
                 , int bloqueado, int PerteneceGrupo, int idGrupoEconomico, string descGrupoEconomico, DateTime? fecIniContrato, DateTime? fecFinContrato, string ObjetoSII)
        {
            try
            {
                SqlParameter[] sqlParam;
                if (accion == "02")
                    sqlParam = new SqlParameter[30];
                else
                    sqlParam = new SqlParameter[27];

                sqlParam[0] = new SqlParameter("@rut", rut);
                sqlParam[1] = new SqlParameter("@razon", razonSocial);
                sqlParam[2] = new SqlParameter("@idEjecutivo", idEjecutivo);
                sqlParam[3] = new SqlParameter("@nomEje", descEjecutivo);
                sqlParam[4] = new SqlParameter("@numEmpleados", numEmpleados);
                sqlParam[5] = new SqlParameter("@tipoEmpresa", idTipoEmpresa);
                sqlParam[6] = new SqlParameter("@descTipoEmp", tipoEmpresa);
                sqlParam[7] = new SqlParameter("@idActividad", idActividad);
                sqlParam[8] = new SqlParameter("@actividad", actividad);
                sqlParam[9] = new SqlParameter("@telFijo", telefono1);
                sqlParam[10] = new SqlParameter("@eMail", email);
                sqlParam[11] = new SqlParameter("@idMotivo", idMotivoBloqueo);
                sqlParam[12] = new SqlParameter("@descMotivo", descMotivoBloqueo);
                sqlParam[13] = new SqlParameter("@accion", accion);
                sqlParam[14] = new SqlParameter("@idEmpresa", idEmpresa);
                sqlParam[15] = new SqlParameter("@usuario", usuario);
                sqlParam[16] = new SqlParameter("@IdUsuario", idUsuario);
                sqlParam[17] = new SqlParameter("@validacion", validacion);
                sqlParam[18] = new SqlParameter("@habEmpresa", habEmpresa);
                sqlParam[19] = new SqlParameter("@habDir", habDirEmpresa);
                sqlParam[20] = new SqlParameter("@fecIniAct", fecIniAct);
                sqlParam[21] = new SqlParameter("@idAsistente", idAsistente);
                sqlParam[22] = new SqlParameter("@descAsistente", descAsistente);
                sqlParam[23] = new SqlParameter("@Bloqueado", bloqueado);
                sqlParam[24] = new SqlParameter("@PerteneceGrupo", PerteneceGrupo);
                sqlParam[25] = new SqlParameter("@idGrupoEconomico", idGrupoEconomico);
                sqlParam[26] = new SqlParameter("@descGrupoEconomico", descGrupoEconomico);
                if (accion == "02")
                {
                    sqlParam[27] = new SqlParameter("@fecIniContrato", fecIniContrato);
                    sqlParam[28] = new SqlParameter("@fecFinContrato", fecFinContrato);
                    sqlParam[29] = new SqlParameter("@objetoSII", ObjetoSII);
                }
                return AD_Global.traerPrimeraColumna("InsertaActualizaEmpresas", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public string InsertaActualizaContactoEmpresa(string apPaterno, string apMaterno, string nombres, string mail, string telFijo, string telCelu, int idCargo, string desCargo,
                                                           string accion, int idReg, string descReg, int idProv, string descProv, int idComuna, string comuna,
                                                           bool habilitado, bool contactoDefecto, int idEmpresa, int rut, string dv, int valContacto, string idUsuario)
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[22];
                sqlParam[0] = new SqlParameter("@apPaterno", apPaterno);
                sqlParam[1] = new SqlParameter("@apMaterno", apMaterno);
                sqlParam[2] = new SqlParameter("@nombres", nombres);
                sqlParam[3] = new SqlParameter("@mail", mail);
                sqlParam[4] = new SqlParameter("@telFijo", telFijo);
                sqlParam[5] = new SqlParameter("@telCel", telCelu);
                sqlParam[6] = new SqlParameter("@desCargo", desCargo);
                sqlParam[7] = new SqlParameter("@accion", accion);
                sqlParam[8] = new SqlParameter("@idReg", idReg);
                sqlParam[9] = new SqlParameter("@descReg", descReg);
                sqlParam[10] = new SqlParameter("@idProv", idProv);
                sqlParam[11] = new SqlParameter("@desProv", descProv);
                sqlParam[12] = new SqlParameter("@idComuna", idComuna);
                sqlParam[13] = new SqlParameter("@comuna", comuna);
                sqlParam[14] = new SqlParameter("@habilitado", habilitado);
                sqlParam[15] = new SqlParameter("@contactoDef", contactoDefecto);
                sqlParam[16] = new SqlParameter("@idEmpresa", idEmpresa);
                sqlParam[17] = new SqlParameter("@rut", rut);
                sqlParam[18] = new SqlParameter("@dv", dv);
                sqlParam[19] = new SqlParameter("@valContacto", valContacto);
                sqlParam[20] = new SqlParameter("@idCargo", idCargo);
                sqlParam[21] = new SqlParameter("@user", idUsuario);
                return AD_Global.traerPrimeraColumna("InsertaActualizaContactoEmpresas", sqlParam);
            }
            catch
            {
                return "-1-Error interno.";
            }
        }

        public DataTable ListarEtapas(int? IdEtapa)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@IdEtapa", SqlDbType.Int);
                SqlParametros[0].Value = IdEtapa;
                dt = AD_Global.ejecutarConsultas("ListarEtapas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarEstadosOperacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarEstadosOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarSubEtapas(int IdEtapa, bool? Rechazo, int IdOpcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@IdEtapa", SqlDbType.Int);
                SqlParametros[0].Value = IdEtapa;
                SqlParametros[1] = new SqlParameter("@Rechazo", SqlDbType.Bit);
                SqlParametros[1].Value = Rechazo;
                SqlParametros[2] = new SqlParameter("@IdOpcion", SqlDbType.Int);
                SqlParametros[2].Value = IdOpcion;
                dt = AD_Global.ejecutarConsultas("ListarSubEtapas", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarServiciosOperacion()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[0];
                dt = AD_Global.ejecutarConsultas("ListarServiciosOperacion", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable ListarServiciosGarantia(int Tipo)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[1];
                SqlParametros[0] = new SqlParameter("@Tipo", SqlDbType.Int);
                SqlParametros[0].Value = Tipo;
                dt = AD_Global.ejecutarConsultas("ListarServiciosGarantia", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable CambioEstado(int Opcion, int OrdenId, bool Condicionado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[0].Value = Opcion;
                SqlParametros[1] = new SqlParameter("@OrdenId", SqlDbType.Int);
                SqlParametros[1].Value = OrdenId;
                SqlParametros[2] = new SqlParameter("@Condicionado", SqlDbType.Bit);
                SqlParametros[2].Value = Condicionado;
                dt = AD_Global.ejecutarConsultas("CambiosEstado", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public DataTable CambioEstado(int Opcion, int IdEstado, int IdEtapa, int IdSubEtapa, bool Condicionado)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[5];
                SqlParametros[0] = new SqlParameter("@Opcion", SqlDbType.Int);
                SqlParametros[0].Value = Opcion;
                SqlParametros[1] = new SqlParameter("@IdEstado", SqlDbType.Int);
                SqlParametros[1].Value = IdEstado;
                SqlParametros[2] = new SqlParameter("@IdEtapa", SqlDbType.Int);
                SqlParametros[2].Value = IdEtapa;
                SqlParametros[3] = new SqlParameter("@IdSubEtapa", SqlDbType.Int);
                SqlParametros[3].Value = IdSubEtapa;
                SqlParametros[4] = new SqlParameter("@Condicionado", SqlDbType.Bit);
                SqlParametros[4].Value = Condicionado;
                dt = AD_Global.ejecutarConsultas("CambiosEstado", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }

        public SPListItemCollection ConsultaListaEdicionUsuario()
        {
            //Traer lista EdicionFormularioUsuario
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList Lista = app.Lists["EdicionFormularioUsuario"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where>" +
                "<Or>" +
                 "<Eq><FieldRef Name='TipoFormulario'/><Value Type='TEXT'>" + "Empresa" + "</Value></Eq>" +
                 "<Eq><FieldRef Name='TipoFormulario'/><Value Type='TEXT'>" + "Operacion" + "</Value></Eq>" +
                 "</Or>" +
                "</Where>" +
                "<OrderBy>" + "<FieldRef Name='Modified' Ascending='True' />" + "</OrderBy>";
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            return ColecLista;
        }
    }
}
