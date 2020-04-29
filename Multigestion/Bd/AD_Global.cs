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
using System.Xml;

namespace Bd
{
    public class AD_Global
    {
        private static SqlConnection cnn;
        private static SqlCommand cmd;
        private static SqlDataAdapter da;
        private static string StrConn = ConfigurationManager.ConnectionStrings["DACConnectionString"].ConnectionString;

        public static DataTable ejecutarConsultas(string NombreSP, SqlParameter[] SqlParams)
        {
            DataTable dt = new DataTable();
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
            return dt;
        }

        public static DataSet ejecutarConsulta(string NombreSP, SqlParameter[] SqlParams)
        {
            DataSet ds = new DataSet();

            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandTimeout = 60;
                cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return ds;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }

            }
            return ds;
        }

        public static Boolean ejecutarAccion(string NombreSP, SqlParameter[] SqlParams)
        {
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }
                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
        }

        public static Boolean ejecutarAccionBool(string NombreSP, SqlParameter[] SqlParams)
        {
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }
                cnn.Open();
                bool output = Convert.ToBoolean(cmd.ExecuteScalar());
                cnn.Close();
                return output;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
        }


        public static string traerPrimeraColumna(string NombreSP, SqlParameter[] SqlParams)
        {
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }

                cnn.Open();
                String output = Convert.ToString(cmd.ExecuteScalar());
                cnn.Close();
                return output;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return "-1-Error interno.";
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
        }

        public static DataTable ejecutarConsultaV2(string NombreView, SqlParameter[] SqlParams)
        {
            //cons.Open();
            //DataTable dt = new DataTable();
            //cmd = new SqlCommand("select * view_SellectAll_Student", cons);
            //cmd.Connection = cons;
            //SqlDataAdapter adp = new SqlDataAdapter(cmd);
            //adp.Fill(dt);
            //cmd.Dispose();
            //cons.Close();
            //adp.Dispose();
            //return dt;

            DataTable dt = new DataTable();
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand("select * from view_perfiles", cnn);
                //cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return dt;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }

            }

            return dt;
        }
        public static int? ejecutarInsert(string NombreSP, SqlParameter[] SqlParams)
        {
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }
                cnn.Open();
                //cmd.ExecuteNonQuery();
                int? Id = Convert.ToInt32(cmd.ExecuteScalar());
                cnn.Close();
                return Id;

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }

            }
        }

        public static int? ejecutarUpdate(string NombreSP, SqlParameter[] SqlParams)
        {
            try
            {
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }
                cnn.Open();
                //cmd.ExecuteNonQuery();
                int Id = (int)cmd.ExecuteScalar();
                cnn.Close();
                return Id;

            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }

            }
        }

        public static XmlReader traerXml(string NombreSP, SqlParameter[] SqlParams)
        {
            try
            {
                XmlReader xml = null;
                cnn = new SqlConnection(StrConn);
                cmd = new SqlCommand(NombreSP, cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (SqlParams != null)
                {
                    for (int i = 0; i <= SqlParams.Length - 1; i++)
                    {
                        cmd.Parameters.Add(SqlParams[i]);
                    }
                }

                cnn.Open();
                xml = cmd.ExecuteXmlReader();
                cnn.Close();
                return xml;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return null;
            }
            finally
            {
                if (cnn != null && cnn.State != ConnectionState.Closed)
                {
                    cnn.Close();
                }
            }
        }
    }
}
