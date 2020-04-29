using FrameworkIntercapIT.Utilities;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    public class verificacionUsuarioMultiple
    {
        public string verificacionEdicionSimultanea(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario, string tipoFormularioIn)
        {
            //edicion multiple del webconfig
            bool permiteEdicionMultiplebool;
            string permiteEdicionMultiple;
            try
            {
                permiteEdicionMultiple = ConfigurationManager.AppSettings["permiteEdicionMultiple"].ToString();

                permiteEdicionMultiplebool = Convert.ToBoolean(permiteEdicionMultiple);
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                permiteEdicionMultiplebool = true;
            }
            //

            string tipoFormulario = tipoFormularioIn;

            //No Permite edición multiple
            if (!permiteEdicionMultiplebool)
            {
                if (tipoFormulario == "Empresa")
                {
                    SPListItemCollection ColecLista = existeUsuarioEnFormularioEmpresa(idEmpresa, tipoFormularioIn);
                    // Lista EdicionFormularioUsuario = EFU
                    string idEFU = String.Empty;
                    string idUsuarioEFU = String.Empty;
                    string idEmpresaEFU = String.Empty;
                    string idOperacionEFU = String.Empty;
                    string tipoFormularioEFU = String.Empty;
                    //Está en uso esa empresa
                    if (ColecLista.Count > 0)
                    {
                        foreach (SPListItem oListItem in ColecLista)
                        {
                            idEFU = oListItem["ID"].ToString();
                            idUsuarioEFU = oListItem["Editor"].ToString();
                            idEmpresaEFU = oListItem["IdEmpresa"].ToString();
                            idOperacionEFU = oListItem["IdOperacion"].ToString();
                            tipoFormularioEFU = oListItem["TipoFormulario"].ToString();
                        }
                        if (idUsuarioEFU.Contains("#"))
                        {
                            idUsuarioEFU = idUsuarioEFU.Split('#')[1];
                        }
                        //¿A que tipo pertenece?
                        if (tipoFormularioEFU == "Empresa")
                        {
                            //¿Quien lo está usando?
                            if (idUsuario == idUsuarioEFU)
                            {
                                //Actualizar
                                actualizarEdicionFormularioUsuarioEmp(idUsuario, idEmpresa, idOperacion, NombreFormulario, tipoFormularioIn);
                            }
                            else
                            {
                                return idUsuarioEFU;
                            }
                        }
                        else
                        {
                            escribirEdicionFormularioUsuarioEmp(idUsuario, idEmpresa, idOperacion, NombreFormulario);
                        }
                    }
                    else
                    {
                        escribirEdicionFormularioUsuarioEmp(idUsuario, idEmpresa, idOperacion, NombreFormulario);
                    }

                }
                else if (tipoFormulario == "Operacion")
                {
                    SPListItemCollection ColecLista = existeUsuarioEnFormularioOperacion(idOperacion, tipoFormularioIn);
                    // Lista EdicionFormularioUsuario = EFU
                    string idEFU = String.Empty;
                    string idUsuarioEFU = String.Empty;
                    string idEmpresaEFU = String.Empty;
                    string idOperacionEFU = String.Empty;
                    string tipoFormularioEFU = String.Empty;
                    //Está en uso esa empresa
                    if (ColecLista.Count > 0)
                    {
                        foreach (SPListItem oListItem in ColecLista)
                        {
                            idEFU = oListItem["ID"].ToString();
                            idUsuarioEFU = oListItem["Editor"].ToString();
                            idEmpresaEFU = oListItem["IdEmpresa"].ToString();
                            idOperacionEFU = oListItem["IdOperacion"].ToString();
                            tipoFormularioEFU = oListItem["TipoFormulario"].ToString();
                        }
                        if (idUsuarioEFU.Contains("#"))
                        {
                            idUsuarioEFU = idUsuarioEFU.Split('#')[1];
                        }
                        //¿A que tipo pertenece?
                        if (tipoFormularioEFU == "Operacion")
                        {
                            //¿Quien lo está usando?
                            if (idUsuario == idUsuarioEFU)
                            {
                                //Actualizar
                                actualizarEdicionFormularioUsuarioOp(idUsuario, idEmpresa, idOperacion, NombreFormulario, tipoFormularioIn);
                            }
                            else
                            {
                                return idUsuarioEFU;
                            }
                        }
                        else
                        {
                            escribirEdicionFormularioUsuarioOp(idUsuario, idEmpresa, idOperacion, NombreFormulario);
                        }
                    }
                    else
                    {
                        escribirEdicionFormularioUsuarioOp(idUsuario, idEmpresa, idOperacion, NombreFormulario);
                    }
                }
                //tipoFormulario = General
                else { }
            }
            //Permite edición multiple
            else
            {
                //nada
            }
            return string.Empty;

        }

        #region Empresa

        public SPListItemCollection existeUsuarioEnFormularioEmpresa(int idEmpresa, string tipoFormularioIn)
        {
            //Traer si existe la empresa
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList Lista = app.Lists["EdicionFormularioUsuario"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where>" +
                "<And>" +
                 "<Eq><FieldRef Name='IdEmpresa'/><Value Type='TEXT'>" + idEmpresa + "</Value></Eq>" +
                 "<Eq><FieldRef Name='TipoFormulario'/><Value Type='TEXT'>" + tipoFormularioIn + "</Value></Eq>" +
                 "</And>" +
                "</Where>" +
                "<OrderBy>" + "<FieldRef Name='Modified' Ascending='True' />" + "</OrderBy>";
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            return ColecLista;
        }

        public void escribirEdicionFormularioUsuarioEmp(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario)
        {
            try
            {
                //Registrar usuario, empresa, operacion y nombreFormulario 
                SPWeb app3 = SPContext.Current.Web;
                app3.AllowUnsafeUpdates = true;
                SPListItemCollection items = app3.Lists["EdicionFormularioUsuario"].Items;
                SPListItem newItem = items.Add();

                //newItem["IdUsuario"] = idUsuario;
                newItem["IdEmpresa"] = idEmpresa;
                newItem["IdOperacion"] = idOperacion;
                newItem["NombreFormulario"] = NombreFormulario;
                newItem["TipoFormulario"] = "Empresa";
                newItem.Update();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        public void actualizarEdicionFormularioUsuarioEmp(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario, string tipoFormularioIn)
        {
            try
            {
                SPListItemCollection ColecLista = existeUsuarioEnFormularioEmpresa(idEmpresa, tipoFormularioIn);

                //SPListItemCollection ColecLista = existeUsuarioEnFormulario(idUsuario, idEmpresa, idOperacion, NombreFormulario);
                //string idEdicionFormularioUsuario = String.Empty;
                //foreach (SPListItem oListItem in ColecLista)
                //{
                //    idEdicionFormularioUsuario = oListItem["ID"].ToString();
                //}
                ////Ingresa por primera vez al formulario
                //if (string.IsNullOrEmpty(idEdicionFormularioUsuario))
                //{
                //    //Registrar usuario, empresa, operacion y nombreFormulario 
                //    SPWeb app3 = SPContext.Current.Web;
                //    app3.AllowUnsafeUpdates = true;
                //    SPListItemCollection items = app3.Lists["EdicionFormularioUsuario"].Items;
                //    SPListItem newItem = items.Add();

                //    //newItem["IdUsuario"] = idUsuario;
                //    newItem["IdEmpresa"] = idEmpresa;
                //    newItem["IdOperacion"] = idOperacion;
                //    newItem["NombreFormulario"] = NombreFormulario;
                //    newItem["TipoFormulario"] = "Empresa";
                //    newItem.Update();
                //}
                //actualizar "nombre Formulario" (Reingreso al mismo formulario) para que la fecha cambie.
                //else
                //{
                foreach (SPListItem item in ColecLista)
                {
                    item["NombreFormulario"] = NombreFormulario;
                    item.Update();
                }
                //}
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        public SPListItemCollection existeUsuarioEnFormulario(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario)
        {
            //Se debe verificar que no exista el mismo usuario en EdicionFormularioUsuario
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList Lista = app.Lists["EdicionFormularioUsuario"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where>" +
                "<And>" +
                    "<And>" +
                        "<And>" +
                            "<Eq><FieldRef Name='NombreFormulario'/><Value Type='TEXT'>" + NombreFormulario + "</Value></Eq>" +
                            "<Eq><FieldRef Name='Editor'/><Value Type='TEXT'>" + idUsuario + "</Value></Eq>" +
                        "</And>" +
                        "<Eq><FieldRef Name='IdEmpresa'/><Value Type='TEXT'>" + idEmpresa + "</Value></Eq>" +
                    "</And>" +
                        "<Eq><FieldRef Name='IdOperacion'/><Value Type='TEXT'>" + idOperacion + "</Value></Eq>" +
                "</And>" +
                "</Where>" +
                "<OrderBy>" + "<FieldRef Name='ID' Ascending='True' />" + "</OrderBy>";
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            return ColecLista;
        }

        #endregion

        #region Operacion

        public SPListItemCollection existeUsuarioEnFormularioOperacion(int idOperacion, string tipoFormularioIn)
        {
            //Traer si existe la empresa
            SPWeb app = SPContext.Current.Web;
            app.AllowUnsafeUpdates = true;
            SPList Lista = app.Lists["EdicionFormularioUsuario"];
            SPQuery oQuery = new SPQuery();
            oQuery.Query = "<Where>" +
                "<And>" +
                 "<Eq><FieldRef Name='IdOperacion'/><Value Type='TEXT'>" + idOperacion + "</Value></Eq>" +
                 "<Eq><FieldRef Name='TipoFormulario'/><Value Type='TEXT'>" + tipoFormularioIn + "</Value></Eq>" +
                 "</And>" +
                "</Where>" +
                "<OrderBy>" + "<FieldRef Name='Modified' Ascending='True' />" + "</OrderBy>";
            SPListItemCollection ColecLista = Lista.GetItems(oQuery);
            return ColecLista;
        }

        public void escribirEdicionFormularioUsuarioOp(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario)
        {
            try
            {
                //Registrar usuario, empresa, operacion y nombreFormulario 
                SPWeb app3 = SPContext.Current.Web;
                app3.AllowUnsafeUpdates = true;
                SPListItemCollection items = app3.Lists["EdicionFormularioUsuario"].Items;
                SPListItem newItem = items.Add();

                //newItem["IdUsuario"] = idUsuario;
                newItem["IdEmpresa"] = idEmpresa;
                newItem["IdOperacion"] = idOperacion;
                newItem["NombreFormulario"] = NombreFormulario;
                newItem["TipoFormulario"] = "Operacion";
                newItem.Update();
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        public void actualizarEdicionFormularioUsuarioOp(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario, string tipoFormularioIn)
        {
            try
            {
                SPListItemCollection ColecLista = existeUsuarioEnFormularioOperacion(idOperacion, tipoFormularioIn);

                foreach (SPListItem item in ColecLista)
                {
                    item["NombreFormulario"] = NombreFormulario;
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

        #endregion

        public void borrarRegistroEdicionFormularioUsuario(string idUsuario, int idEmpresa, int idOperacion, string NombreFormulario)
        {
            try
            {
                SPWeb app = SPContext.Current.Web;
                SPList Lista = app.Lists["EdicionFormularioUsuario"];
                SPQuery oQuery = new SPQuery();
                oQuery.Query = "<Where>" +
                    "<And>" +
                        "<And>" +
                            "<And>" +
                                "<Eq><FieldRef Name='NombreFormulario'/><Value Type='TEXT'>" + NombreFormulario + "</Value></Eq>" +
                                "<Eq><FieldRef Name='Editor'/><Value Type='TEXT'>" + idUsuario + "</Value></Eq>" +
                            "</And>" +
                            "<Eq><FieldRef Name='IdEmpresa'/><Value Type='TEXT'>" + idEmpresa + "</Value></Eq>" +
                        "</And>" +
                            "<Eq><FieldRef Name='IdOperacion'/><Value Type='TEXT'>" + idOperacion + "</Value></Eq>" +
                    "</And>" +
                    "</Where>" +
                    "<OrderBy>" + "<FieldRef Name='ID' Ascending='True' />" + "</OrderBy>";
                SPListItemCollection ColecLista2 = Lista.GetItems(oQuery);
                int j = ColecLista2.Count;
                for (int i = 0; i < j; i++)
                {
                    ColecLista2[0].Delete();
                }
            }
            catch (Exception ex)
            {
                LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
            }

        }

    }
}
