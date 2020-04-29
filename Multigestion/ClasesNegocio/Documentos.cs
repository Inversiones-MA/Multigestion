using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultigestionUtilidades;
using System.Web.UI.WebControls;

namespace ClasesNegocio
{
    public class Documentos
    {
        public Documentos()
        { 
        
        }

        public bool cargarDocumento(string carpetaEmpresa, string carpetaArea, string idOperacion, Resumen objresumen, FileUpload fileDocumento, string TipoDocumento, string ddlOperacion, string comentario)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                string urlApp = app.Url;
                SPUser usuario = SPContext.Current.Site.SystemAccount;
                carpetaEmpresa = new Utilidades { }.GetSharePointFriendlyName(carpetaEmpresa);
                carpetaArea = new Utilidades { }.GetSharePointFriendlyName(carpetaArea);
                SPFolder path;

                if (fileDocumento.HasFile)
                {
                    string nombreArchivo = new Utilidades { }.GetSharePointFriendlyName(fileDocumento.FileName);

                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa).Exists)
                    {
                        CrearCarpetaEnRaiz(carpetaEmpresa);
                    }
                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea).Exists)
                    {
                        CrearCarpetaAreas(carpetaEmpresa, carpetaArea);
                    }

                    if (idOperacion != "Seleccione" && carpetaArea != "FISCALIA")
                    {
                        if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion).Exists)
                        {
                            string pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                            CrearCarpetaAreas(pathEmpresaArea, idOperacion);
                        }
                        path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion);
                    }
                    else
                    {
                        string pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                        path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea);
                    }

                    SPFile cargaArchivo = path.Files.Add(path + "/" + nombreArchivo, fileDocumento.FileBytes, usuario, usuario, DateTime.Now, DateTime.Now);
                    cargaArchivo.Item.Update();

                    string nombreArchivoNuevo = string.Empty;

                    if (carpetaArea != "FISCALIA")
                        nombreArchivoNuevo = new Utilidades { }.NombreArchivo(TipoDocumento, ddlOperacion);
                    else
                        nombreArchivoNuevo = TipoDocumento.Trim();

                    cargaArchivo.Item["Name"] = nombreArchivoNuevo;
                    cargaArchivo.Item["IdEmpresa"] = objresumen.idEmpresa;
                    cargaArchivo.Item["Area"] = carpetaArea;
                    cargaArchivo.Item["FechaInicio"] = DateTime.Now;
                    cargaArchivo.Item["TipoDocumento"] = TipoDocumento.Trim();
                    cargaArchivo.Item["Comentario"] = comentario;
                    cargaArchivo.Item["Habilitado"] = true;
                    //if (carpetaArea != "FISCALIA")
                    cargaArchivo.Item["IdOperacion"] = idOperacion == "Seleccione" ? "" : idOperacion;
                    cargaArchivo.Item.Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public string ObtenerNombreArchivo(string tipo, string certificado, Resumen objresumen)
        {
            SPWeb app = SPContext.Current.Web;
            DataTable tabla = new DataTable();
            var listData = app.Lists["Documentos"];
            Utilidades util = new Utilidades();
            var urlLibrary = "";
            var nomLibrary = "";
            SPFolder carpeta;
            urlLibrary = app.Url + "/Documentos/" + util.RemoverSignosAcentos(util.LimpiarTexto(objresumen.desEmpresa.ToString())) + "/Operaciones/" + "Certificado" + certificado;
            //urlLibrary = app.Url + "/Documentos/" + RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(objresumen.desEmpresa.ToString())) + "/" + RemoverSignosAcentos(System.Web.HttpUtility.HtmlDecode(objresumen.desOperacion.ToString()));
            nomLibrary = objresumen.area.Trim();

            carpeta = app.GetFolder(urlLibrary + "/");

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    string SubFolderUrl = app.Url + "/" + carpeta.ToString();
                    SPFolder SubFolder = web.GetFolder(SubFolderUrl);
                    SPQuery query = new SPQuery();
                    query.Query = "<Where><And><And><Eq><FieldRef Name='TipoDocumento'/><Value Type='Text'>" + tipo + "</Value></Eq><Eq><FieldRef Name='IdOperacion'/><Value Type='Text'>" + objresumen.idOperacion + "</Value></Eq></And><Eq><FieldRef Name='IdEmpresa'/><Value Type='Text'>" + objresumen.idEmpresa + "</Value></Eq></And></Where>";

                    query.Folder = SubFolder;  // This should restrict the query to the subfolder
                    SPDocumentLibrary list = SubFolder.DocumentLibrary;

                    SPListItemCollection files = list.GetItems(query);

                    foreach (SPListItem item in files)
                    {
                        return item.Name;
                    }
                }
            }

            return "";
        }

        public bool EliminarArchivo(string tipo, string certificado, Resumen objresumen)
        {
            Utilidades util = new Utilidades();
            SPWeb app = SPContext.Current.Web;
            var urlLibrary = app.Url + "/Documentos/" + util.RemoverSignosAcentos(util.LimpiarTexto(objresumen.desEmpresa.Trim())) + "/Operaciones/" + "Certificado" + certificado;
            var nomLibrary = tipo;// objresumen.area.Trim();
            var carpeta = app.GetFolder(urlLibrary + "/");
            SPFileCollection collFiles = app.GetFolder(carpeta + "/").Files;
            for (int intIndex = collFiles.Count - 1; intIndex > -1; intIndex--)
            {
                if (collFiles[intIndex].Name == nomLibrary)
                {
                    string strDelUrl = collFiles[intIndex].Url;
                    try
                    {
                        collFiles.Delete(strDelUrl);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

            }
            return false;
        }

        public bool cargaMasivaDocumento(string nombreArchivo, string carpetaEmpresa, string carpetaArea, string idOperacion, string tipoDocumento, string comentario, string contador, byte[] bytesArchivo, string extension, Resumen objresumen)
        {
            try
            {
                Utilidades util = new Utilidades();
                nombreArchivo = util.GetSharePointFriendlyName(nombreArchivo);
                carpetaEmpresa = util.GetSharePointFriendlyName(carpetaEmpresa);
                carpetaArea = util.GetSharePointFriendlyName(carpetaArea);

                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                string urlApp = app.Url;
                SPUser usuario = SPContext.Current.Site.SystemAccount;

                if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa).Exists)
                {
                    CrearCarpetaEnRaiz(carpetaEmpresa);
                }
                if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea).Exists)
                {
                    CrearCarpetaAreas(carpetaEmpresa, carpetaArea);
                }
                SPFolder path;
                if (idOperacion != "Seleccione")
                {
                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion).Exists)
                    {
                        String pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                        CrearCarpetaAreas(pathEmpresaArea, idOperacion);
                    }
                    path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion);
                }
                else
                {
                    String pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                    path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea);
                }
                SPFile cargaArchivo = path.Files.Add(path + "/" + nombreArchivo + "." + extension, bytesArchivo, usuario, usuario, DateTime.Now, DateTime.Now);
                cargaArchivo.Item.Update();
                //nombreArchivoNuevo = nombreArchivo;
                //nombreArchivoNuevo = idOperacion == "Seleccione" ? nombreArchivoNuevo : nombreArchivoNuevo + "_" + idOperacion;
                String nombreArchivoNuevo = nombreArchivo + "_" + contador + "." + extension; // Se agrega un contador para que no coincidan los nombres de archivos.
                cargaArchivo.Item["Name"] = nombreArchivoNuevo;
                cargaArchivo.Item["IdEmpresa"] = objresumen.idEmpresa;
                cargaArchivo.Item["Area"] = carpetaArea;
                cargaArchivo.Item["FechaInicio"] = DateTime.Now;
                cargaArchivo.Item["TipoDocumento"] = tipoDocumento;
                cargaArchivo.Item["Comentario"] = comentario;
                cargaArchivo.Item["Habilitado"] = true;
                cargaArchivo.Item["IdOperacion"] = idOperacion == "Seleccione" ? "" : idOperacion;
                cargaArchivo.Item.Update();

                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool CrearCarpetaEnRaiz(string nombreCarpeta)
        {
            try
            {
                Utilidades util = new Utilidades();
                nombreCarpeta = util.GetSharePointFriendlyName(nombreCarpeta);
                SPWeb app = SPContext.Current.Web;
                SPList docList = app.Lists["Documentos"];
                SPFolder newFolder = docList.RootFolder.SubFolders.Add(nombreCarpeta);
                return true;
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return false;
            }
        }

        public bool CargarDocFiscalia(string carpetaEmpresa, string carpetaArea, FileUpload fileDocumento, string idEmpresa, string TipoDocumento, string idOperacion, string comentario, string extension)
        {
            try
            {
                Utilidades util = new Utilidades();
                SPWeb app = SPContext.Current.Web;
                app.AllowUnsafeUpdates = true;
                string urlApp = app.Url;
                SPUser usuario = SPContext.Current.Site.SystemAccount;
                carpetaEmpresa = util.GetSharePointFriendlyName(carpetaEmpresa);
                carpetaArea = util.GetSharePointFriendlyName(carpetaArea);
                SPFolder path;

                if (fileDocumento.HasFile)
                {
                    string nombreArchivo = util.GetSharePointFriendlyName(fileDocumento.FileName);
                    string nombreArchivoNuevo = string.Empty;

                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa).Exists)
                    {
                        CrearCarpetaEnRaiz(carpetaEmpresa);
                    }
                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea).Exists)
                    {
                        CrearCarpetaAreas(carpetaEmpresa, carpetaArea);
                    }

                    if (!app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion).Exists)
                    {
                        string pathEmpresaArea = carpetaEmpresa + "/" + carpetaArea;
                        CrearCarpetaAreas(pathEmpresaArea, idOperacion);
                    }
                    path = app.GetFolder(urlApp + "/Documentos/" + carpetaEmpresa + "/" + carpetaArea + "/" + idOperacion);

                    //if (carpetaArea != "FISCALIA")
                    nombreArchivoNuevo = util.NombreArchivo(TipoDocumento, idOperacion);
                    //else
                    //nombreArchivoNuevo = TipoDocumento.Trim() + extension;

                    //SPFile cargaArchivo = path.Files.Add(path + "/" + nombreArchivo, fileDocumento.FileBytes, usuario, usuario, DateTime.Now, DateTime.Now);
                    SPFile cargaArchivo = path.Files.Add(path + "/" + nombreArchivoNuevo + extension, fileDocumento.FileBytes, usuario, usuario, DateTime.Now, DateTime.Now);
                    cargaArchivo.Item.Update();
                    cargaArchivo.Item["Name"] = nombreArchivoNuevo + extension;
                    cargaArchivo.Item["IdEmpresa"] = idEmpresa;
                    cargaArchivo.Item["Area"] = carpetaArea;
                    cargaArchivo.Item["FechaInicio"] = DateTime.Now;
                    cargaArchivo.Item["TipoDocumento"] = TipoDocumento.Trim();
                    cargaArchivo.Item["Comentario"] = comentario;
                    cargaArchivo.Item["Habilitado"] = true;

                    //if (carpetaArea != "FISCALIA")
                    cargaArchivo.Item["IdOperacion"] = idOperacion == "Seleccione" ? "" : idOperacion;
                    cargaArchivo.Item.Update();

                    return true;
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


        public void CrearCarpetaAreas(string carpetaRaiz, string carpetaArea)
        {
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList listData = app.Lists["Documentos"];
            var urlLibrary = app.Url + "/Documentos/" + carpetaRaiz;

            SPListItem carpetaCliente;

            if (!app.GetFolder(urlLibrary + "/" + carpetaArea).Exists)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate
                {
                    carpetaCliente = listData.Folders.Add(urlLibrary, SPFileSystemObjectType.Folder, carpetaArea);
                    carpetaCliente.Update();
                    listData.Update();
                });
            }
            else
            {
                var idLibrary = app.GetFolder(urlLibrary + "/" + carpetaArea).UniqueId;
                carpetaCliente = listData.Folders[idLibrary];
            }
        }

        public bool ExisteArchivo(string tipo, string certificado, Resumen objresumen)
        {
            try
            {
                Utilidades util = new Utilidades();
                SPWeb app = SPContext.Current.Web;
                DataTable tabla = new DataTable();
                var listData = app.Lists["Documentos"];
                //SPWeb app = SPContext.Current.Web;
                var urlLibrary = "";
                var nomLibrary = "";
                SPFolder carpeta;
                urlLibrary = app.Url + "/Documentos/" + util.RemoverSignosAcentos(util.LimpiarTexto(objresumen.desEmpresa.Trim())) + "/Operaciones/" + "Certificado" + certificado;                
                nomLibrary = objresumen.area.Trim();

                carpeta = app.GetFolder(urlLibrary + "/");


                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        //string SubFolderUrl = app.Url + "/" + carpeta.ToString();
                        string SubFolderUrl = web.ServerRelativeUrl + carpeta.ToString();
                        SPFolder SubFolder = web.GetFolder(SubFolderUrl);
                        SPQuery query = new SPQuery();
                        query.Query = "<Where><And><And><Eq><FieldRef Name='TipoDocumento'/><Value Type='Text'>" + tipo + "</Value></Eq><Eq><FieldRef Name='IdOperacion'/><Value Type='Text'>" + objresumen.idOperacion + "</Value></Eq></And><Eq><FieldRef Name='IdEmpresa'/><Value Type='Text'>" + objresumen.idEmpresa + "</Value></Eq></And></Where>";

                        query.Folder = SubFolder;  // This should restrict the query to the subfolder
                        SPDocumentLibrary list = SubFolder.DocumentLibrary;

                        SPListItemCollection files = list.GetItems(query);


                        foreach (SPListItem item in files)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }
            return false;
        }

        public DataTable buscarArchivos(string carpetaEmpresa, string area)
        {
            Utilidades util = new Utilidades();
            carpetaEmpresa = util.GetSharePointFriendlyName(carpetaEmpresa);
            area = util.GetSharePointFriendlyName(area);
            DataTable dt = new DataTable();

            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList listData = app.Lists["Documentos"];
            String path = app.Url + "/Documentos/" + carpetaEmpresa + "/" + area;

            SPFolder pathCarpeta = app.GetFolder(path);
            SPQuery query = new SPQuery();
            query.Query = "<Where></Where>";
            query.Folder = pathCarpeta;
            SPDocumentLibrary libreria = pathCarpeta.DocumentLibrary;
            SPListItemCollection files = libreria.GetItems(query);
            List<SPListItem> archivosResultantes = new List<SPListItem>();
            String nombreArchivo;

            foreach (SPListItem item in files)
            {
                nombreArchivo = item["Name"].ToString();
                Console.WriteLine(nombreArchivo);
                if (app.GetFolder(path + "/" + nombreArchivo).Exists)
                {
                    SPQuery query2 = new SPQuery();
                    query2.Query = "<Where></Where>";
                    query2.Folder = app.GetFolder(path + "/" + nombreArchivo);

                    SPDocumentLibrary list2 = pathCarpeta.DocumentLibrary;
                    SPListItemCollection files2 = libreria.GetItems(query2);
                    foreach (SPListItem item2 in files2)
                    {
                        archivosResultantes.Add(item2);
                    }
                }
                else
                    archivosResultantes.Add(item);
            }

            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Tipo Documento", typeof(string));
            dt.Columns.Add("IdOperación", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Comentario", typeof(string));
            dt.Columns.Add("Acción", typeof(string));
            foreach (SPListItem item in archivosResultantes)
            {
                try
                {
                    String idTemp = item["ID"] == null ? "" : item["ID"].ToString();
                    String nombreTemp = item["Name"] == null ? "" : item["Name"].ToString();
                    String areaTemp = item["Area"] == null ? "" : item["Area"].ToString();
                    String tipoDocTemp = item["TipoDocumento"] == null ? "" : item["TipoDocumento"].ToString();
                    String idOperacionTemp = item["IdOperacion"] == null ? "" : item["IdOperacion"].ToString();
                    String FechaTemp = item["FechaInicio"] == null ? "" : Convert.ToDateTime(item["FechaInicio"].ToString()).ToString("dd-MM-yyyy");
                    String Comentario = item["Comentario"] == null ? "" : item["Comentario"].ToString();
                    dt.Rows.Add(idTemp, nombreTemp, areaTemp.ToUpper(), tipoDocTemp, idOperacionTemp, FechaTemp, Comentario, "");
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    dt = new DataTable();
                }
            }
            return dt;
        }

        public DataTable buscarArchivosFiscalia(string carpetaEmpresa, string area, string idOperacion)
        {
            Utilidades util = new Utilidades();
            carpetaEmpresa = util.GetSharePointFriendlyName(carpetaEmpresa);
            area = util.GetSharePointFriendlyName(area);
            DataTable dt = new DataTable();

            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList listData = app.Lists["Documentos"];
            String path = app.Url + "/Documentos/" + carpetaEmpresa + "/" + area + "/" + idOperacion;

            SPFolder pathCarpeta = app.GetFolder(path);
            SPQuery query = new SPQuery();
            query.Query = "<Where></Where>";
            query.Folder = pathCarpeta;
            SPDocumentLibrary libreria = pathCarpeta.DocumentLibrary;
            SPListItemCollection files = libreria.GetItems(query);
            List<SPListItem> archivosResultantes = new List<SPListItem>();
            String nombreArchivo;

            foreach (SPListItem item in files)
            {
                nombreArchivo = item["Name"].ToString();
                Console.WriteLine(nombreArchivo);
                if (app.GetFolder(path + "/" + nombreArchivo).Exists)
                {
                    SPQuery query2 = new SPQuery();
                    query2.Query = "<Where></Where>";
                    query2.Folder = app.GetFolder(path + "/" + nombreArchivo);

                    SPDocumentLibrary list2 = pathCarpeta.DocumentLibrary;
                    SPListItemCollection files2 = libreria.GetItems(query2);
                    foreach (SPListItem item2 in files2)
                    {
                        archivosResultantes.Add(item2);
                    }
                }
                else
                {
                    archivosResultantes.Add(item);
                }
            }

            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Tipo Documento", typeof(string));
            dt.Columns.Add("IdOperación", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Comentario", typeof(string));
            dt.Columns.Add("Acción", typeof(string));
            foreach (SPListItem item in archivosResultantes)
            {
                try
                {
                    String idTemp = item["ID"] == null ? "" : item["ID"].ToString();
                    String nombreTemp = item["Name"] == null ? "" : item["Name"].ToString();
                    String areaTemp = item["Area"] == null ? "" : item["Area"].ToString();
                    String tipoDocTemp = item["TipoDocumento"] == null ? "" : item["TipoDocumento"].ToString();
                    String idOperacionTemp = item["IdOperacion"] == null ? "" : item["IdOperacion"].ToString();
                    String FechaTemp = item["FechaInicio"] == null ? "" : Convert.ToDateTime(item["FechaInicio"].ToString()).ToString("dd-MM-yyyy");
                    String Comentario = item["Comentario"] == null ? "" : item["Comentario"].ToString();
                    dt.Rows.Add(idTemp, nombreTemp, areaTemp.ToUpper(), tipoDocTemp, idOperacionTemp, FechaTemp, Comentario, "");
                }
                catch (Exception ex)
                {
                    LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                    dt = new DataTable();
                }
            }
            return dt;
        }

        public DataTable buscarArchivos(string Empresa, string Rut, string area, string idOperacion)
        {
            Utilidades util = new Utilidades();
            DataTable dt = new DataTable();
            string carpeta = util.carpetaEmpresa(Empresa.Trim(), Rut.Trim());
            dt = buscarArchivosFiscalia(carpeta, area.ToUpper(), idOperacion);

            return dt;
        }
    }
}
