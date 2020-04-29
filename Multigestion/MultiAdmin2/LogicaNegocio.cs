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
using MultigestionUtilidades;

namespace MultiAdmin2.wpadmin3
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

     
        #region "wpPlantillaDocumento"


        public int? InsertarPlantilla(LNplantilla plantilla)
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

        public int? VerificarPlantilla(MultiAdmin2.wpadmin3.LNplantilla plantilla)
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

        public int? ActualizarPlantilla(MultiAdmin2.wpadmin3.LNplantilla plantilla)
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

        public DataTable ListarPlantillas(MultiAdmin2.wpadmin3.LNplantilla plantilla)
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

        public DataTable ListarTodasPlantillas(MultiAdmin2.wpadmin3.LNplantilla plantilla)
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

        #region "wpPerfiles"


        public void ListarAreas(ref DropDownList combo)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["Areas"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "";
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

    }
}
