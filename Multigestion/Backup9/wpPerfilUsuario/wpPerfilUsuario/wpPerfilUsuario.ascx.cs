using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using MultigestionUtilidades;
using System.Web.Services;
using System.Collections.Generic;
using System.Text;
using DevExpress.Web;
using System.Data.SqlClient;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using Bd;
using ClasesNegocio;

namespace MultiAdministracion.wpPerfilUsuario.wpPerfilUsuario
{
    [ToolboxItemAttribute(false)]
    public partial class wpPerfilUsuario : WebPart
    {
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DACConnectionString"].ConnectionString);
        public string selTipoOpcion = "";
        public string selOpcion = "";
        LogicaNegocio Ln = new LogicaNegocio();
        DataTable dt = new DataTable();
        Resumen objresumen = new Resumen();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        #region Eventos
        
        protected void Page_Load(object sender, EventArgs e)
        {
            hdn_Opciones.Value = "created";
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("sp_tipoOpciones", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@modulo", 0);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            GridViewDataComboBoxColumn cmdTipoOpcion = grid.Columns["TipoOpcion"] as GridViewDataComboBoxColumn;
            while (reader.Read())
            {
                ListEditItem item = new ListEditItem(reader["Descripcion"].ToString(), int.Parse(reader["idTipoOpcion"].ToString()));
                cmdTipoOpcion.PropertiesComboBox.Items.Add(item);
            }
            reader.Close();

            if (connection.State == ConnectionState.Open) connection.Close();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string mySearch = txtPerfil.Value;
            SqlDataSourcePerfiles.SelectCommand = " select * from dbo.view_perfilUsuario where usuario like '%" + mySearch + "%' order by 4,5 ";
        }

        #endregion


        #region Metodos

        protected void ocultarDiv()
        {
            dvWarning.Style.Add("display", "none");
            dvSuccess.Style.Add("display", "none");
            dvError.Style.Add("display", "none");
        }

        private void llenargrid()
        {
            asignacionResumen(ref objresumen);
            try
            {
                LogicaNegocio MTO = new LogicaNegocio();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                Page.Session["Error"] = ex.Source.ToString() + ": " + ex.Message.ToString();
                Page.Response.Redirect("Mensaje.aspx");
            }
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("Mensaje.aspx");
        }

        private void LimpiarCampos()
        {
            txt_perfil.Text = String.Empty;
            ddl_modulo.SelectedIndex = ddl_tipoOpcion.SelectedIndex = ddl_Opciones.SelectedIndex = ddl_permiso.SelectedIndex = 0;
        }

      
        #endregion

     
        #region "grillasWEB"

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Usuario")
            {
                int c = 0;
                ASPxComboBox cmb = e.Editor as ASPxComboBox;
                SqlCommand command = new SqlCommand("sp_perfilUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mode", 1);
                if (connection.State != ConnectionState.Open) connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cmb.Items.Add(reader["Nombre"].ToString(), Int32.Parse(reader["ID"].ToString()));
                    if (reader["Nombre"].ToString() == cmb.Text) cmb.SelectedIndex = c;
                    c++;
                }
                reader.Close();
                //if (cmb.Text == "") cmb.SelectedIndex = 0;
                //if (hdn_update.Value == "1") 
                //    if (txtPerfil.Value != "") cmb.Text = txtPerfil.Value;
            }
            if (e.Column.FieldName == "Perfil")
            {
                int c = 0;
                ASPxComboBox cmb = e.Editor as ASPxComboBox;
                SqlCommand command = new SqlCommand("sp_perfilUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@mode", 2);
                if (connection.State != ConnectionState.Open) connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cmb.Items.Add(reader["Perfil"].ToString(), Int32.Parse(reader["ID"].ToString()));
                    if (reader["Perfil"].ToString() == cmb.Text) cmb.SelectedIndex = c;
                    c++;
                }
                reader.Close();
                //if (cmb.Text == "") cmb.SelectedIndex = 0;
            }
            if (connection.State != ConnectionState.Closed) connection.Close();
        }

        protected void grid_CustomErrorText(object sender, ASPxGridViewCustomErrorTextEventArgs e)
        {
            if (e.Exception is NullReferenceException)
                e.ErrorText = "NullReferenceExceptionText";
            else if (e.Exception is InvalidOperationException)
                e.ErrorText = "InvalidOperationExceptionText";
            else if (e.Exception.Message.IndexOf("Cannot insert the value") != -1)
                e.ErrorText = "Error al ingrear el REGISTRO, Seleccione 'Usuario' y 'Perfil' antes de GUARDAR... ";
        }

        #endregion

    } 

}
