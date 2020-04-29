using Bd;
using FrameworkIntercapIT.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    public class LnPlantilla
    {

        private Nullable<int> _IdPlantillaDocumento;
        private Nullable<int> _IdTipoProducto;
        private Nullable<int> _IdDocumentoTipo;
        private Nullable<int> _IdAcreedor;
        private string _NombrePlantilla;
        private Nullable<Boolean> _Vigente;
        private string _ContenidoHtml;
        private Nullable<Boolean> _EsGenerica;
        private string _UsuarioModificacion;
        private Nullable<DateTime> _FechaIngreso;
        private Nullable<DateTime> _FechaModificacion;
        private string _Area;

        public Nullable<int> IdPlantillaDocumento { get { return _IdPlantillaDocumento; } set { _IdPlantillaDocumento = value; } }
        public Nullable<int> IdTipoProducto { get { return _IdTipoProducto; } set { _IdTipoProducto = value; } }
        public Nullable<int> IdDocumentoTipo { get { return _IdDocumentoTipo; } set { _IdDocumentoTipo = value; } }
        public Nullable<int> IdAcreedor { get { return _IdAcreedor; } set { _IdAcreedor = value; } }
        public string NombrePlantilla { get { return _NombrePlantilla; } set { _NombrePlantilla = value; } }
        public Nullable<Boolean> Vigente { get { return _Vigente; } set { _Vigente = value; } }
        public string ContenidoHtml { get { return _ContenidoHtml; } set { _ContenidoHtml = value; } }
        public Nullable<Boolean> EsGenerica { get { return _EsGenerica; } set { _EsGenerica = value; } }
        public string UsuarioModificacion { get { return _UsuarioModificacion; } set { _UsuarioModificacion = value; } }
        public Nullable<DateTime> FechaIngreso { get { return _FechaIngreso; } set { _FechaIngreso = value; } }
        public Nullable<DateTime> FechaModificacion { get { return _FechaModificacion; } set { _FechaModificacion = value; } }
        public string Area { get { return _Area; } set { _Area = value; } }

        #region "metodos"

        public int? InsertarPlantilla(LnPlantilla plantilla)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[9];

                SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
                SqlParametros[0].Value = plantilla.IdPlantillaDocumento;
                //SqlParametros[0].Direction = ParameterDirection.Output;

                SqlParametros[1] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
                SqlParametros[1].Value = plantilla.IdTipoProducto;

                SqlParametros[2] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
                SqlParametros[2].Value = plantilla.IdDocumentoTipo;

                SqlParametros[3] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[3].Value = plantilla.IdAcreedor;

                SqlParametros[4] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
                SqlParametros[4].Value = plantilla.NombrePlantilla;

                SqlParametros[5] = new SqlParameter("@ContenidoHtml", SqlDbType.NVarChar);
                SqlParametros[5].Value = plantilla.ContenidoHtml;

                SqlParametros[6] = new SqlParameter("@EsGenerica", SqlDbType.Bit);
                SqlParametros[6].Value = plantilla.EsGenerica;

                SqlParametros[7] = new SqlParameter("@UsuarioModificacion", SqlDbType.NVarChar);
                SqlParametros[7].Value = plantilla.UsuarioModificacion;

                SqlParametros[8] = new SqlParameter("@Area", SqlDbType.NVarChar);
                SqlParametros[8].Value = plantilla.Area;

                return AD_Global.ejecutarInsert("GuardarDocumento", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        public int? VerificarPlantilla(LnPlantilla plantilla)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[8];

                SqlParametros[0] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
                SqlParametros[0].Value = plantilla.IdTipoProducto;

                SqlParametros[1] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
                SqlParametros[1].Value = plantilla.IdDocumentoTipo;

                SqlParametros[2] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[2].Value = plantilla.IdAcreedor;

                return AD_Global.ejecutarInsert("GuardarDocumento", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        public int? ActualizarPlantilla(LnPlantilla plantilla)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[8];

                SqlParametros[0] = new SqlParameter("@IdPlantillaDocumento", SqlDbType.Int);
                SqlParametros[0].Value = plantilla.IdPlantillaDocumento;

                SqlParametros[1] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
                SqlParametros[1].Value = plantilla.NombrePlantilla;

                SqlParametros[2] = new SqlParameter("@ContenidoHtml", SqlDbType.NVarChar);
                SqlParametros[2].Value = plantilla.ContenidoHtml;

                SqlParametros[3] = new SqlParameter("@EsGenerica", SqlDbType.NVarChar);
                SqlParametros[3].Value = plantilla.EsGenerica;

                SqlParametros[4] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
                SqlParametros[4].Value = plantilla.IdDocumentoTipo;

                SqlParametros[5] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[5].Value = plantilla.IdAcreedor;

                SqlParametros[6] = new SqlParameter("@UsuarioModificacion", SqlDbType.NVarChar);
                SqlParametros[6].Value = plantilla.UsuarioModificacion;

                SqlParametros[7] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
                SqlParametros[7].Value = plantilla.IdTipoProducto;

                return AD_Global.ejecutarUpdate("ActualizarDocumento", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }
        }

        public DataTable ListarPlantillas(LnPlantilla plantilla)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[5];

                SqlParametros[0] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
                SqlParametros[0].Value = plantilla.IdTipoProducto;

                SqlParametros[1] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
                SqlParametros[1].Value = plantilla.IdDocumentoTipo;

                SqlParametros[2] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[2].Value = plantilla.IdAcreedor;

                SqlParametros[3] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
                SqlParametros[3].Value = plantilla.NombrePlantilla;

                SqlParametros[4] = new SqlParameter("@EsGenerica", SqlDbType.Bit);
                SqlParametros[4].Value = plantilla.EsGenerica;

                return AD_Global.ejecutarConsultas("ListarPlantillas", SqlParametros);
            }
            catch
            {
                return null;
            }

        }

        public DataTable ListarTodasPlantillas(LnPlantilla plantilla)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[4];

                SqlParametros[0] = new SqlParameter("@IdTipoProducto", SqlDbType.Int);
                SqlParametros[0].Value = plantilla.IdTipoProducto;

                SqlParametros[1] = new SqlParameter("@IdDocumentoTipo", SqlDbType.Int);
                SqlParametros[1].Value = plantilla.IdDocumentoTipo;

                SqlParametros[2] = new SqlParameter("@IdAcreedor", SqlDbType.Int);
                SqlParametros[2].Value = plantilla.IdAcreedor;

                SqlParametros[3] = new SqlParameter("@NombrePlantilla", SqlDbType.NVarChar);
                SqlParametros[3].Value = plantilla.NombrePlantilla;

                return AD_Global.ejecutarConsultas("ListarTodasPlantillas", SqlParametros);
            }
            catch
            {
                return null;
            }

        }

        public bool EliminarPlantilla(int IdPlantilla, string usuario)
        {
            try
            {
                SqlParameter[] SqlParametros = new SqlParameter[2];
                SqlParametros[0] = new SqlParameter("@IdPlantilla", SqlDbType.Int);
                SqlParametros[0].Value = IdPlantilla;
                SqlParametros[1] = new SqlParameter("@usuario", SqlDbType.NChar);
                SqlParametros[1].Value = usuario;
                return AD_Global.ejecutarAccion("EliminarPlantilla", SqlParametros);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
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

        #endregion

    }
}
