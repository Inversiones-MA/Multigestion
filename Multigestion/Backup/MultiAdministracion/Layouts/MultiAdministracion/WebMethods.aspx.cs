using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MultiAdministracion
{
    public partial class WebMethods : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string getUserProfile(string userName) {
            string result = "";
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DACConnectionString"].ConnectionString);
                SqlCommand command = new SqlCommand("sp_perfilUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mode", 3);
                if (connection.State != ConnectionState.Open) connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read()){
                    if (userName.Trim() == reader["Usuario"].ToString().Trim()) result = "El Usuario ya Tiene un perfil asignado!!!.";
                }
                reader.Close();
                if (connection.State == ConnectionState.Open) connection.Close();
            }
            catch (Exception e)
            {
                result = e.Message.Replace("'", "");
            }
            return result;
        }
    }
}
