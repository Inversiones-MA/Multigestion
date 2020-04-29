using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using FrameworkIntercapIT.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using MultigestionUtilidades;
using System.Web.Services;
using System.Collections.Generic;
using System.Text;
using DevExpress.Web;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;


namespace MultiAdministracion.wpPerfiles.wpPerfiles
{
    [ToolboxItemAttribute(false)]
    public partial class wpPerfiles : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        ////public wpClientes() { }
        Resumen objresumen = new Resumen();
        verificacionUsuarioMultiple vem = new verificacionUsuarioMultiple();
        Utilidades util = new Utilidades();
        public SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DACConnectionString"].ConnectionString);
        public string selTipoOpcion = "";
        public string selOpcion = "";
        LogicaNegocio MTO = new LogicaNegocio();
        DataTable dt = new DataTable();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }


        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtTipoOpciones = new DataTable();
            DataTable dtPermisos = new DataTable();
            LogicaNegocio MTO = new LogicaNegocio();
            string modulo = string.Empty;

            //PERMISOS USUARIOS
            Permisos permiso = new Permisos();
            string PermisoConfigurado = string.Empty;
            SPWeb app2 = SPContext.Current.Web;
            ValidarPermisos validar = new ValidarPermisos();
            DataTable dtpermisos = new DataTable("dt");
            validar.NombreUsuario = util.ObtenerValor(app2.CurrentUser.Name);
            validar.Pagina = "Perfiles.aspx";
            validar.Etapa = "";

            dtpermisos = permiso.ListarPerfil(validar);
            if (dtpermisos.Rows.Count > 0)
            {
                dt = MTO.ListarPerfiles();

                grid.DataSource = dt;
                grid.DataBind();

                dtTipoOpciones = MTO.ListarTipoOpciones(modulo);
                hdn_Opciones["hdn_Opciones"] = DataTableToJSONWithStringBuilder(dtTipoOpciones);

                dtPermisos = MTO.ListarPermisos(null, null);
                hdn_permisos["hdn_permisos"] = DataTableToJSONWithStringBuilder(dtPermisos);
            }
            else
            {
                dvFormulario.Style.Add("display", "none");
                dvWarning1.Style.Add("display", "block");
                lbWarning1.Text = "Usuario sin permisos configurados";
            }
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string a = string.Empty;
        }

        protected void grid_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            ASPxGridView Grid = (ASPxGridView)sender;
            int IdCargoAccion = 0;
            int Crud = 1; //insert

            string Descripcion = e.NewValues["Perfil"].ToString();
            int idTipoOpciones = Int32.Parse(e.NewValues["Tipo Opcion"].ToString());
            int idOpciones = Int32.Parse(e.NewValues["Opciones"].ToString());
            int idPermiso = Int32.Parse(e.NewValues["Permiso"].ToString());
            string Modulo = e.NewValues["Modulo"].ToString();

            bool resGuardar = MTO.GuardarPerfil(Descripcion, idOpciones, idPermiso, Modulo, true, Crud, IdCargoAccion);

            if (resGuardar)
            {
                //mostar mensaje
                grdCustomMessageText(Grid, "ingreso correcto");
                //Actualiza grilla
                dt = MTO.ListarPerfiles();
                grid.DataSource = dt;
                grid.DataBind();
            }
            else
            {
                //mostar mensaje
                grdCustomMessageText(Grid, "error al ingresar el dato");
            }

            e.Cancel = true;
            Grid.CancelEdit();
            //Grid.DataBind();
        }

        protected void grid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            ASPxGridView Grid = (ASPxGridView)sender;
            int IdCargoAccion = Int32.Parse(e.Keys["IdCargoAccion"].ToString());
            int Crud = 2; //update

            string Descripcion = e.NewValues["Perfil"].ToString();
            int idTipoOpciones = Int32.Parse(e.NewValues["Tipo Opcion"].ToString());
            int idOpciones = Int32.Parse(e.NewValues["Opciones"].ToString());
            int idPermiso = Int32.Parse(e.NewValues["Permiso"].ToString());
            string Modulo = e.NewValues["Modulo"].ToString();

            bool resGuardar = MTO.GuardarPerfil(Descripcion, idOpciones, idPermiso, Modulo, true, Crud, IdCargoAccion);

            if (resGuardar)
            {
                //mostar mensaje
                grdCustomMessageText(Grid, "actualizacion correcta");
                //Actualiza grilla
                dt = MTO.ListarPerfiles();
                grid.DataSource = dt;
                grid.DataBind();
            }
            else
            {
                //mostar mensaje
                grdCustomMessageText(Grid, "error al ingresar el dato");
            }

            e.Cancel = true;
            Grid.CancelEdit();
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

        protected void grid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            LogicaNegocio MTO = new LogicaNegocio();
            ASPxGridView Grid = (ASPxGridView)sender;
            int IdCargoAccion = Int32.Parse(e.Keys["IdCargoAccion"].ToString());
            int Crud = 3; //delete

            bool resGuardar = MTO.GuardarPerfil("", 0, 0, "", true, Crud, IdCargoAccion);

            if (resGuardar)
            {
                //Actualiza grilla
                dt = MTO.ListarPerfiles();
                grid.DataSource = dt;
                grid.DataBind();
                grdCustomMessageText(Grid, "se ha borrado el registro");
            }
            else
            {
                //mostar mensaje
                grdCustomMessageText(Grid, "error al borrar el registro");
            }

            e.Cancel = true;
            Grid.CancelEdit();
        }

        protected void grid_ContextMenuItemClick(object sender, ASPxGridViewContextMenuItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "ExportToPDF":
                    ASPxGridViewExporter1.WritePdfToResponse();
                    break;

                case "ExportToXLS":
                    ASPxGridViewExporter1.WriteXlsToResponse();
                    break;
            }
        }

        protected void grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            if (e.MenuType == GridViewContextMenuType.Rows)
            {
                var item = e.CreateItem("Export", "Export");
                item.BeginGroup = true;
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);

                AddMenuSubItem(item, "PDF", "ExportToPDF", @"/Images/doc_pdf.png", true);
                AddMenuSubItem(item, "XLS", "ExportToXLS", @"/Images/doc_excel_table.png", true);
            }
        }

        #endregion


        #region Metodos

        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

        public void asignacionResumen(ref Resumen objresumen)
        {
            if (ViewState["RES"] != null)
                objresumen = (Resumen)ViewState["RES"];
            else
                Page.Response.Redirect("Mensaje.aspx");
        }

        protected void btnAtras_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaClientes");
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect(objresumen.linkPrincial);
        }

        protected void lbClientes_Click(object sender, EventArgs e)
        {
            asignacionResumen(ref objresumen);
            vem.borrarRegistroEdicionFormularioUsuario(objresumen.idUsuario, objresumen.idEmpresa, objresumen.idOperacion, "EmpresaClientes");
            Page.Session["RESUMEN"] = objresumen;
            Page.Session["BUSQUEDA"] = ViewState["BUSQUEDA"];
            Page.Response.Redirect("EmpresaClientes.aspx");
        }

        private static void AddMenuSubItem(GridViewContextMenuItem parentItem, string text, string name, string imageUrl, bool isPostBack)
        {
            var exportToXlsItem = parentItem.Items.Add(text, name);
            exportToXlsItem.Image.Url = imageUrl;
        }

        #endregion

    
        #region "grillasWEB"

        protected void grid_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Modulo")
            {
                ASPxComboBox cmb = e.Editor as ASPxComboBox;
                cmb.Items.Add("Comercial", "Comercial");
                cmb.Items.Add("Riesgo", "Riesgo");
                cmb.Items.Add("Fiscalia", "Fiscalia");
                cmb.Items.Add("Operaciones", "Operaciones");
                cmb.Items.Add("Contabilidad", "Contabilidad");
                cmb.Items.Add("Seguimiento", "Seguimiento");
                cmb.Items.Add("Posicion", "Posicion");
                cmb.Items.Add("Configuracion", "Configuracion");
                hdn_modulo["hdn_modulo"] = cmb.Text;
            }
            if (e.Column.FieldName == "Tipo Opcion")
            {
                var auxItem = "";
                ASPxComboBox cmb = e.Editor as ASPxComboBox;
                SqlCommand command = new SqlCommand("sp_tipoOpciones", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@modulo", hdn_modulo["hdn_modulo"]);
                if (connection.State != ConnectionState.Open) connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["TipoOpcion"].ToString() != auxItem)
                        cmb.Items.Add(reader["TipoOpcion"].ToString(), reader["TipoOpcion"].ToString());
                    auxItem = reader["TipoOpcion"].ToString();
                }
                reader.Close();

                selTipoOpcion = cmb.Text;
            }
            if (e.Column.FieldName == "Opciones")
            {
                ASPxComboBox cmb = e.Editor as ASPxComboBox;
                SqlCommand command = new SqlCommand("sp_tipoOpciones", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@modulo", hdn_modulo["hdn_modulo"]);
                if (connection.State != ConnectionState.Open) connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //if (reader["TipoOpcion"].ToString() == selTipoOpcion)
                    cmb.Items.Add(reader["Opcion"].ToString().Trim(), reader["Opcion"].ToString());
                }
                reader.Close();
                selOpcion = cmb.Text;
            }
            if (e.Column.FieldName == "Permiso")
            {
                ASPxComboBox cmb = e.Editor as ASPxComboBox;
                SqlCommand command = new SqlCommand("sp_permisos", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@modulo", hdn_modulo["hdn_modulo"]);
                command.Parameters.AddWithValue("@opcion", selOpcion);
                if (connection.State != ConnectionState.Open) connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cmb.Items.Add(reader["Permiso"].ToString(), reader["Permiso"].ToString());
                }
                reader.Close();
            }
            if (e.Column.FieldName == "Perfil")
            {
                ASPxTextBox txt = e.Editor as ASPxTextBox;
                if (hdn_update["hdn_update"].ToString() == "1")
                {
                    if (txtPerfil.Value != "") txt.Text = txtPerfil.Value;
                }
            }
        }
        #endregion

    }
}
