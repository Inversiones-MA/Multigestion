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
    [Serializable]
    public class ValidarPermisos
    {
        public int idUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int idCargo { get; set; }
        public string descCargo { get; set; }
        public string Permiso { get; set; }
        public string Pagina { get; set; }
        public string Url { get; set; }
        public string descMenu { get; set; }
        public string Etapa { get; set; }

        public ValidarPermisos()
        {

        }

        public DataTable ListarPerfil(ValidarPermisos validar)
        {
            try
            {
                DataTable dt = new DataTable("dt");
                SqlParameter[] SqlParametros = new SqlParameter[3];
                SqlParametros[0] = new SqlParameter("@NombreUsuario", SqlDbType.NVarChar);
                SqlParametros[0].Value = validar.NombreUsuario.Trim();
                SqlParametros[1] = new SqlParameter("@NombrePagina", SqlDbType.NVarChar);
                SqlParametros[1].Value = validar.Pagina.Trim();
                SqlParametros[2] = new SqlParameter("@Modulo", SqlDbType.NVarChar);
                SqlParametros[2].Value = validar.Etapa.Trim();

                dt = AD_Global.ejecutarConsultas("ConsultarPerfil", SqlParametros);
                return dt;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataTable();
            }
        }
    }
}
