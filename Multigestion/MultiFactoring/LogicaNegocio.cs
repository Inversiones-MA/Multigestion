using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultigestionUtilidades;

namespace MultiFactoring
{
    class LogicaNegocio
    {
        //public DataSet ConsutarDatosPAF(string idEmpresa, string usuario, string perfil, string idOperacion)
        //{
        //    try
        //    {
        //        DataSet dt = new DataSet();
        //        SqlParameter[] SqlParametros = new SqlParameter[4];
        //        SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
        //        SqlParametros[0].Value = int.Parse(idEmpresa);
        //        SqlParametros[1] = new SqlParameter("@user", SqlDbType.NVarChar);
        //        SqlParametros[1].Value = usuario;
        //        SqlParametros[2] = new SqlParameter("@perfil", SqlDbType.NVarChar);
        //        SqlParametros[2].Value = perfil;

        //        SqlParametros[3] = new SqlParameter("@idOperacion", SqlDbType.Int);
        //        SqlParametros[3].Value = int.Parse(idOperacion);

        //        dt = AD_Global.ejecutarConsulta("ConsutarDatosPAF", SqlParametros);
        //        return dt;
        //    }
        //    catch (Exception)
        //    {
        //        //LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
        //        return new DataSet();
        //    }
        //}

        public DataSet FactoringGestion(int idEmpresa, double VentasMoviles, double ValorDolar, double ValorUf, string DescEjecutivo, double MontoAprobado, double MontoVigente, double Montopropuesto,
            string Comentarios, double CoberturaVigenteComercial, double CoberturaVigenteAjustado, double CoberturaGlobalComercial, double CoberturaGlobalAjustado, DateTime FechaRevision, int IdEstadoLinea,
            int IdNivelAtribucion, string UsuarioAccion, int IdPafFactoring, int opcion)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[18];
                SqlParametros[0] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@VentasMoviles", SqlDbType.Float);
                SqlParametros[1].Value = VentasMoviles;
                SqlParametros[2] = new SqlParameter("@ValorDolar", SqlDbType.Float);
                SqlParametros[2].Value = ValorDolar;
                SqlParametros[3] = new SqlParameter("@ValorUf", SqlDbType.Float);
                SqlParametros[3].Value = ValorUf;
                SqlParametros[4] = new SqlParameter("@DescEjecutivo", SqlDbType.NVarChar);
                SqlParametros[4].Value = DescEjecutivo;
                SqlParametros[5] = new SqlParameter("@MontoAprobado", SqlDbType.Float);
                SqlParametros[5].Value = MontoAprobado;
                SqlParametros[6] = new SqlParameter("@MontoVigente", SqlDbType.Float);
                SqlParametros[6].Value = MontoVigente;
                SqlParametros[7] = new SqlParameter("@Montopropuesto", SqlDbType.Float);
                SqlParametros[7].Value = Montopropuesto;
                SqlParametros[8] = new SqlParameter("@Comentarios", SqlDbType.Float);
                SqlParametros[8].Value = Comentarios;
                SqlParametros[9] = new SqlParameter("@CoberturaVigenteComercial", SqlDbType.Float);
                SqlParametros[9].Value = CoberturaVigenteComercial;
                SqlParametros[10] = new SqlParameter("@CoberturaVigenteAjustado", SqlDbType.Float);
                SqlParametros[10].Value = CoberturaVigenteAjustado;
                SqlParametros[11] = new SqlParameter("@CoberturaGlobalComercial", SqlDbType.Float);
                SqlParametros[11].Value = CoberturaGlobalComercial;
                SqlParametros[12] = new SqlParameter("@CoberturaGlobalAjustado", SqlDbType.Float);
                SqlParametros[12].Value = CoberturaGlobalAjustado;
                SqlParametros[13] = new SqlParameter("@FechaRevision", SqlDbType.DateTime);
                SqlParametros[13].Value = FechaRevision;
                SqlParametros[14] = new SqlParameter("@IdEstadoLinea", SqlDbType.Int);
                SqlParametros[14].Value = IdEstadoLinea;
                SqlParametros[15] = new SqlParameter("@IdNivelAtribucion", SqlDbType.Int);
                SqlParametros[15].Value = IdNivelAtribucion;
                SqlParametros[16] = new SqlParameter("@UsuarioAccion", SqlDbType.NVarChar);
                SqlParametros[16].Value = UsuarioAccion;
                SqlParametros[17] = new SqlParameter("@IdPafFactoring", SqlDbType.Int);
                SqlParametros[17].Value = IdPafFactoring;               
                SqlParametros[18] = new SqlParameter("@opcion", SqlDbType.Int);
                SqlParametros[18].Value = opcion;

                ds = AD_Global.ejecutarConsulta("Factoring_gestion", SqlParametros);
                return ds;
            }
            catch (Exception)
            {
                //LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
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
            catch (Exception)
            {
                //LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable FactoringOperacion(int IdOperacion, int IdEmpresa, int IdTipoFinanciamiento, int Idproducto, int IdFinalidad, int Plazo, double AnticipoMax, double PlazoMax, double ConcentracionMax,
          double TasaComision, int IdTipoMoneda, double MontoAprobado, double MontoVigente, double MontoPropuesto, int IdEstado, string UsuarioAccion, int opcion)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] SqlParametros = new SqlParameter[17];
        
                SqlParametros[0] = new SqlParameter("@IdOperacion", SqlDbType.Int);
                SqlParametros[0].Value = IdOperacion;
                SqlParametros[1] = new SqlParameter("@IdEmpresa", SqlDbType.Int);
                SqlParametros[1].Value = IdEmpresa;
                SqlParametros[2] = new SqlParameter("@IdTipoFinanciamiento", SqlDbType.Int);
                SqlParametros[2].Value = IdTipoFinanciamiento;
                SqlParametros[3] = new SqlParameter("@Idproducto", SqlDbType.Int);
                SqlParametros[3].Value = Idproducto;
                SqlParametros[4] = new SqlParameter("IdFinalidad", SqlDbType.Int);
                SqlParametros[4].Value = IdFinalidad;
                SqlParametros[5] = new SqlParameter("@Plazo", SqlDbType.Int);
                SqlParametros[5].Value = Plazo;
                SqlParametros[6] = new SqlParameter("@AnticipoMax", SqlDbType.Float);
                SqlParametros[6].Value = AnticipoMax;
                SqlParametros[7] = new SqlParameter("@PlazoMax", SqlDbType.Float);
                SqlParametros[7].Value = PlazoMax;
                SqlParametros[8] = new SqlParameter("@ConcentracionMax", SqlDbType.Float);
                SqlParametros[8].Value = ConcentracionMax;
                SqlParametros[9] = new SqlParameter("@TasaComision", SqlDbType.Float);
                SqlParametros[9].Value = TasaComision;
                SqlParametros[10] = new SqlParameter("@IdTipoMoneda", SqlDbType.Int);
                SqlParametros[10].Value = IdTipoMoneda;
                SqlParametros[11] = new SqlParameter("@MontoAprobado", SqlDbType.Float);
                SqlParametros[11].Value = MontoAprobado;
                SqlParametros[12] = new SqlParameter("@MontoVigente", SqlDbType.Float);
                SqlParametros[12].Value = MontoVigente;
                SqlParametros[13] = new SqlParameter("@MontoPropuesto", SqlDbType.Float);
                SqlParametros[13].Value = MontoPropuesto;
                SqlParametros[14] = new SqlParameter("@IdEstado", SqlDbType.Int);
                SqlParametros[14].Value = IdEstado;
                SqlParametros[15] = new SqlParameter("@UsuarioAccion", SqlDbType.NVarChar);
                SqlParametros[15].Value = UsuarioAccion;
                SqlParametros[16] = new SqlParameter("@opcion", SqlDbType.Int);
                SqlParametros[16].Value = opcion;

                dt = AD_Global.ejecutarConsultas("Factoring_Operacion_gestion", SqlParametros);
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }


        public DataSet ConsutarDatosEmpresaPAF(int idEmpresa, string idPaf, string usuario, string perfil)
        {
            try
            {
                DataSet dt = new DataSet();
                SqlParameter[] SqlParametros = new SqlParameter[4];
                SqlParametros[0] = new SqlParameter("@idEmpresa", SqlDbType.Int);
                SqlParametros[0].Value = idEmpresa;
                SqlParametros[1] = new SqlParameter("@idPAF", SqlDbType.Int);
                SqlParametros[1].Value = int.Parse(idPaf);
                SqlParametros[2] = new SqlParameter("@user", SqlDbType.NVarChar);
                SqlParametros[2].Value = usuario;
                SqlParametros[3] = new SqlParameter("@perfil", SqlDbType.NVarChar);
                SqlParametros[3].Value = perfil;
                dt = AD_Global.ejecutarConsulta("ConsutarDatosEmpresaPAF", SqlParametros);
                return dt;
            }
            catch (Exception)
            {
                //LoggingError.PostEventRegister(ex, ConfigurationManager.AppSettings["pathLog"].ToString(), "", "", ConfigurationManager.AppSettings["logName"].ToString(), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledLog"].ToString()), Convert.ToBoolean(ConfigurationManager.AppSettings["enabledEventViewer"].ToString()), ConfigurationManager.AppSettings["registerEventsTypes"].ToString(), EventLogEntryType.Error);
                return new DataSet();
            }
        }

        public DataTable ListarEmpresas(int idOpcion, string RazonSocial, string descEjecutivo) 
        {
            try
            {
                SqlParameter[] sqlParam;
                sqlParam = new SqlParameter[3];
                sqlParam[0] = new SqlParameter("@idOpcion", idOpcion);
                sqlParam[1] = new SqlParameter("@RazonSocial", RazonSocial);
                sqlParam[2] = new SqlParameter("@descEjecutivo", descEjecutivo);
                return AD_Global.ejecutarConsultas("ListarEmpresas", sqlParam);
            }
            catch
            {
                return new DataTable();
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
            catch (Exception)
            {
                return new DataSet();
            }
        }
    }
}
