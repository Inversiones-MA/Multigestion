using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesNegocio
{
    public class MultiEmpresa
    {
        #region > Propiedades
        public int IdEmpresa { get; set; }
        public int IdEjecutivo { get; set; }
        public string DescEjecutivo { get; set; }
        public decimal Rut { get; set; }
        public string DivRut { get; set; }
        public string RazonSocial { get; set; }
        public DateTime? FecInicioActEco { get; set; }
        public string NombreFantasia { get; set; }
        public string IdGiro { get; set; }
        public string TelFijo1 { get; set; }
        public string TelFijo2 { get; set; }
        public string Fax1 { get; set; }
        public string Fax2 { get; set; }
        public string EMail { get; set; }
        public int IdActividad { get; set; }
        public string DescActividad { get; set; }
        public int IdTipoEmpresa { get; set; }
        public string DescTipoEmpresa { get; set; }
        public int NumEmpleados { get; set; }
        public int idAsistente { get; set; }
        public string DescAsistente { get; set; }
        public DateTime? FecIniContrato { get; set; }
        public DateTime? FecFinContrato { get; set; }
        public bool? Bloqueado { get; set; }
        public int idMotivoBloqueo { get; set; }
        public string descMotivoBloqueo { get; set; }
        public bool? PerteneceGrupo { get; set; }
        public int idGrupoEconomico { get; set; }
        public string descGrupoEconomico { get; set; }
        public string disponibilidad { get; set; }
        public int Validacion { get; set; }
        public bool? Habilitado { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string ClasificacionSBIF { get; set; }
        public string ClasificacionPAF { get; set; }
        public string ObjetoSociedad { get; set; }
        public string EmailFacturacion { get; set; }

        #endregion

        #region > Constructor
        public MultiEmpresa()
        {
            Init();
        }
        #endregion

        #region > Métodos
        //public bool Grabar()
        //{
        //    bool grabo = false;
        //    try
        //    {
        //        DALC.Proveedor p = new DALC.Proveedor();
        //        if (!ExisteProveedor())
        //        {
        //            p.Rut = Rut;
        //            p.RazonSocial = RazonSocial.ToUpper();
        //            if (!string.IsNullOrEmpty(Direccion) || !string.IsNullOrEmpty(Contacto) || !string.IsNullOrEmpty(Fono) || !string.IsNullOrEmpty(Correo))
        //            {
        //                p.Direccion = Direccion;
        //                p.Contacto = UtilString.FormatearContacto(Contacto);
        //                p.Correo = Correo;
        //                p.Fono = Fono;
        //                p.Actualizado = true;
        //                p.RevisionInicial = false;
        //            }
        //            //  RC 20180802: Agrego datos bancarios
        //            p.IdBanco = IdBanco == null ? p.IdBanco : IdBanco;
        //            p.IdCuentaBancariaTipo = IdCuentaBancariaTipo == null ? p.IdCuentaBancariaTipo : IdCuentaBancariaTipo; ;
        //            p.NumeroCuentaBancaria = string.IsNullOrEmpty(NumeroCuentaBancaria) ? p.NumeroCuentaBancaria : NumeroCuentaBancaria;
        //            p.RutCuentaBancaria = string.IsNullOrEmpty(RutCuentaBancaria) ? p.RutCuentaBancaria : RutCuentaBancaria;
        //            p.NombreCuentaBancaria = string.IsNullOrEmpty(NombreCuentaBancaria) ? p.NombreCuentaBancaria : NombreCuentaBancaria;
        //            p.CorreoCuentaBancaria = string.IsNullOrEmpty(CorreoCuentaBancaria) ? p.CorreoCuentaBancaria : CorreoCuentaBancaria;
        //            p.ExisteChileProveedores = ExisteEnChileProveedores();

        //            Common.ModeloEntidad.Proveedor.Add(p);
        //            Common.ModeloEntidad.SaveChanges();

        //            grabo = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "Grabar"
        //        };
        //        log.Grabar();
        //    }
        //    return grabo;
        //}

        //public bool GrabarFromFicha()
        //{
        //    bool grabo = false;
        //    try
        //    {
        //        DALC.Proveedor p = new DALC.Proveedor();
        //        if (!ExisteProveedor())
        //        {
        //            p.RazonSocial = RazonSocial;
        //            p.Rut = Rut;
        //            p.SitioWeb = SitioWeb;
        //            p.Contacto = Contacto;
        //            p.Fono = Fono;
        //            p.Correo = Correo;
        //            p.Direccion = Direccion;
        //            p.Comuna = Comuna;
        //            p.Region = Region;
        //            p.UrlFichaMP = UrlFichaMP;
        //            p.ExisteChileProveedores = ExisteChileProveedores;
        //            p.Actualizado = Actualizado;
        //            p.VentasHistoricasMP = VentasHistoricasMP;
        //            p.VentasAñoActualMP = VentasAñoActualMP;

        //            Common.ModeloEntidad.Proveedor.Add(p);
        //            Common.ModeloEntidad.SaveChanges();

        //            grabo = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "Grabar"
        //        };
        //        log.Grabar();
        //    }
        //    return grabo;
        //}

        //public bool GrabarPostulacion()
        //{
        //    bool grabo = false;
        //    try
        //    {
        //        DALC.Proveedor p = new DALC.Proveedor();
        //        if (!ExisteProveedor())
        //        {
        //            p.Rut = Rut;
        //            p.RazonSocial = RazonSocial.ToUpper();
        //            p.TipoEmpresa = TipoEmpresa;
        //            p.Direccion = Direccion;
        //            p.ComplementoDireccion = ComplementoDireccion;
        //            p.NumeroDireccion = NumeroDireccion;
        //            p.Region = Region;
        //            p.Comuna = Comuna;
        //            p.Ciudad = Ciudad;
        //            p.Contacto = UtilString.FormatearContacto(Contacto);
        //            p.Correo = Correo;
        //            p.Fono = Fono;
        //            p.ExisteChileProveedores = ExisteChileProveedores;
        //            p.Actualizado = true;
        //            p.RevisionInicial = false;
        //            p.IdActividad = IdActividad;

        //            Common.ModeloEntidad.Proveedor.Add(p);
        //            Common.ModeloEntidad.SaveChanges();

        //            grabo = true;
        //        }
        //        else
        //        {
        //            ActualizarDatosFormulario();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "Grabar"
        //        };
        //        log.Grabar();
        //    }
        //    return grabo;
        //}

        //public bool ExisteEnChileProveedores()
        //{
        //    bool existe = false;
        //    try
        //    {
        //        existe = Common.ModeloEntidad.ChileProveedor.Count(cp => cp.Proveedor == RazonSocial) == 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "ExisteEnChileProveedores"
        //        };
        //        log.Grabar();
        //    }
        //    return existe;
        //}

        //public bool Actualizar()
        //{
        //    bool actualizo = false;
        //    try
        //    {
        //        DALC.Proveedor p = Common.ModeloEntidad.Proveedor.SingleOrDefault(pr => pr.Rut == Rut);

        //        if (p != null)
        //        {
        //            p.RazonSocial = string.IsNullOrEmpty(RazonSocial) ? p.RazonSocial : RazonSocial;
        //            p.NombreSII = string.IsNullOrEmpty(NombreSii) ? p.NombreSII : NombreSii;
        //            p.IdNivelVentas = IdNivelVentas == null ? p.IdNivelVentas : (int?)IdNivelVentas;
        //            p.Direccion = string.IsNullOrEmpty(Direccion) ? p.Direccion : Direccion;
        //            p.Contacto = string.IsNullOrEmpty(Contacto) ? p.Contacto : UtilString.FormatearContacto(Contacto);
        //            p.Fono = string.IsNullOrEmpty(Fono) ? p.Fono : Fono;
        //            p.Correo = string.IsNullOrEmpty(Correo) ? p.Correo : Correo;
        //            p.ExisteChileProveedores = ExisteEnChileProveedores();
        //            p.Actualizado = true;
        //            p.RevisionInicial = RevisionInicial;
        //            p.Region = string.IsNullOrEmpty(Region) ? p.Region : Region;
        //            p.Comuna = string.IsNullOrEmpty(Comuna) ? p.Comuna : Comuna;
        //            p.TipoEmpresa = string.IsNullOrEmpty(TipoEmpresa) ? p.TipoEmpresa : TipoEmpresa;
        //            p.NumeroDireccion = string.IsNullOrEmpty(NumeroDireccion) ? p.NumeroDireccion : NumeroDireccion;
        //            p.CorreoFacturacion = string.IsNullOrEmpty(CorreoFacturacion) ? p.CorreoFacturacion : CorreoFacturacion;
        //            p.Cargo = string.IsNullOrEmpty(Cargo) ? p.Cargo : Cargo;
        //            p.ComplementoDireccion = string.IsNullOrEmpty(ComplementoDireccion) ? p.ComplementoDireccion : ComplementoDireccion;
        //            p.UrlFichaMP = string.IsNullOrEmpty(UrlFichaMP) ? p.UrlFichaMP : UrlFichaMP;
        //            p.SitioWeb = string.IsNullOrEmpty(SitioWeb) ? p.SitioWeb : SitioWeb;
        //            p.VentasHistoricasMP = string.IsNullOrEmpty(VentasHistoricasMP) ? p.VentasHistoricasMP : VentasHistoricasMP;
        //            p.VentasAñoActualMP = string.IsNullOrEmpty(VentasAñoActualMP) ? p.VentasAñoActualMP : VentasAñoActualMP;
        //            p.IdActividad = IdActividad == null ? p.IdActividad : IdActividad;
        //            p.Ciudad = string.IsNullOrEmpty(Ciudad) ? p.Ciudad : Ciudad;
        //            p.ValidarSiisaOperacion = ValidarSiisaOperacion;
        //            p.IdBanco = IdBanco == null ? p.IdBanco : IdBanco;
        //            p.IdCuentaBancariaTipo = IdCuentaBancariaTipo == null ? p.IdCuentaBancariaTipo : IdCuentaBancariaTipo; ;
        //            p.NumeroCuentaBancaria = string.IsNullOrEmpty(NumeroCuentaBancaria) ? p.NumeroCuentaBancaria : NumeroCuentaBancaria;
        //            p.RutCuentaBancaria = string.IsNullOrEmpty(RutCuentaBancaria) ? p.RutCuentaBancaria : RutCuentaBancaria;
        //            p.NombreCuentaBancaria = string.IsNullOrEmpty(NombreCuentaBancaria) ? p.NombreCuentaBancaria : NombreCuentaBancaria;
        //            p.CorreoCuentaBancaria = string.IsNullOrEmpty(CorreoCuentaBancaria) ? p.CorreoCuentaBancaria : CorreoCuentaBancaria;
        //        }

        //        Common.ModeloEntidad.SaveChanges();
        //        actualizo = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "Actualizar"
        //        };
        //        log.Grabar();
        //    }
        //    return actualizo;
        //}

        //public bool ActualizarDatosFormulario()
        //{
        //    bool actualizo = false;
        //    try
        //    {
        //        DALC.Proveedor p = Common.ModeloEntidad.Proveedor.SingleOrDefault(pr => pr.Rut == Rut);

        //        if (p != null)
        //        {
        //            p.RazonSocial = RazonSocial;
        //            p.Direccion = Direccion;
        //            p.Contacto = UtilString.FormatearContacto(Contacto);
        //            p.Fono = Fono;
        //            p.Correo = Correo;
        //            p.Region = Region;
        //            p.Comuna = Comuna;
        //            p.Ciudad = Ciudad;
        //            p.NumeroDireccion = NumeroDireccion;
        //            p.TipoEmpresa = TipoEmpresa;
        //            p.IdActividad = IdActividad;
        //            p.NombreSII = string.IsNullOrEmpty(NombreSii) ? p.NombreSII : NombreSii;
        //            p.CorreoFacturacion = CorreoFacturacion;
        //            p.Cargo = Cargo;
        //            p.ComplementoDireccion = ComplementoDireccion;
        //            p.ExisteChileProveedores = ExisteChileProveedores;
        //        }

        //        Common.ModeloEntidad.SaveChanges();
        //        actualizo = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "ActualizarDatosFormulario"
        //        };
        //        log.Grabar();
        //    }
        //    return actualizo;
        //}

        //public bool ActualizarNivelVenta()
        //{
        //    bool actualizo = false;
        //    try
        //    {
        //        DALC.Proveedor p = Common.ModeloEntidad.Proveedor.SingleOrDefault(pr => pr.Rut == Rut);

        //        if (p != null)
        //        {
        //            p.IdNivelVentas = IdNivelVentas == 0 ? p.IdNivelVentas : IdNivelVentas;
        //        }

        //        Common.ModeloEntidad.SaveChanges();
        //        actualizo = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "ActualizarNivelVenta"
        //        };
        //        log.Grabar();
        //    }
        //    return actualizo;
        //}

        //public bool ActualizarValidacionSiiSaOperacion()
        //{
        //    bool actualizo = false;
        //    try
        //    {
        //        DALC.Proveedor p = Common.ModeloEntidad.Proveedor.SingleOrDefault(pr => pr.Rut == Rut);

        //        if (p != null)
        //        {
        //            p.ValidarSiisaOperacion = ValidarSiisaOperacion != null ? ValidarSiisaOperacion : p.ValidarSiisaOperacion;
        //        }

        //        Common.ModeloEntidad.SaveChanges();
        //        actualizo = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "ActualizarValidacionSiiSaOperacion"
        //        };
        //        log.Grabar();
        //    }
        //    return actualizo;
        //}

        //public bool ExisteProveedor()
        //{
        //    bool existe = false;
        //    try
        //    {
        //        if (Common.ModeloEntidad.Proveedor.AsNoTracking().Count(p => p.Rut == this.Rut) == 1)
        //        {
        //            existe = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log log = new Log
        //        {
        //            Tipo = "Exception",
        //            Source = ex.Source,
        //            Message = ex.Message,
        //            StackTrace = ex.StackTrace,
        //            Clase = "Proveedor",
        //            Accion = "ExisteProveedor"
        //        };
        //        log.Grabar();
        //    }
        //    return existe;
        //}

        //public MultiEmpresa Leer()
        //{
        //    try
        //    {
        //        //var c = Common.ModeloEntidad.Proveedor.AsNoTracking().SingleOrDefault(ca => ca.Rut == Rut);
        //        //if (c != null)
        //        //{
        //        //    Rut = c.Rut;
        //        //    RazonSocial = c.RazonSocial;
        //        //    Direccion = c.Direccion;
        //        //    Contacto = c.Contacto;
        //        //    Fono = c.Fono;
        //        //    Correo = c.Correo;
        //        //    ExisteChileProveedores = c.ExisteChileProveedores;
        //        //    Actualizado = c.Actualizado;
        //        //    NumeroDireccion = c.NumeroDireccion;
        //        //    Region = c.Region;
        //        //    Comuna = c.Comuna;
        //        //    TipoEmpresa = c.TipoEmpresa;
        //        //    IdActividad = c.IdActividad;
        //        //    CorreoFacturacion = c.CorreoFacturacion;
        //        //    IdNivelVentas = c.IdNivelVentas;
        //        //    Cargo = c.Cargo;
        //        //    ComplementoDireccion = c.ComplementoDireccion;
        //        //    Actualizado = c.Actualizado;
        //        //    clasificacion = c.IdNivelVentas != null ? new ClasificacionNivelVenta { Id = c.IdNivelVentas.Value }.Leer() : null;
        //        //    IdBanco = c.IdBanco;
        //        //    IdCuentaBancariaTipo = c.IdCuentaBancariaTipo;
        //        //    NumeroCuentaBancaria = c.NumeroCuentaBancaria;
        //        //    RutCuentaBancaria = c.RutCuentaBancaria;
        //        //    NombreCuentaBancaria = c.NombreCuentaBancaria;
        //        //    CorreoCuentaBancaria = c.CorreoCuentaBancaria;
        //        //    Ciudad = c.Ciudad;
        //        //    ValidarSiisaOperacion = c.ValidarSiisaOperacion;

        //            return this;
        //        //}
        //        //else
        //        //{
        //        //    return null;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return null;
        //}


        private void Init()
        {
            IdEmpresa = 0;
            IdEjecutivo = 0;
            DescEjecutivo = string.Empty;
            Rut = 0;
            DivRut = string.Empty;
            RazonSocial = string.Empty;
            FecInicioActEco = null;
            NombreFantasia = string.Empty;
            IdGiro = string.Empty;
            TelFijo1 = string.Empty;
            TelFijo2 = string.Empty;
            Fax1 = string.Empty;
            Fax2 = string.Empty;
            EMail = string.Empty;
            IdActividad = 0;
            DescActividad = string.Empty;
            IdTipoEmpresa = 0;
            DescTipoEmpresa = string.Empty;
            NumEmpleados = 0;
            idAsistente = 0;
            DescAsistente = string.Empty;
            FecIniContrato = null;
            FecFinContrato = null;
            Bloqueado = null;
            idMotivoBloqueo = 0;
            descMotivoBloqueo = string.Empty;
            PerteneceGrupo = null;
            idGrupoEconomico = 0;
            descGrupoEconomico = string.Empty;
            disponibilidad = string.Empty;
            Validacion = 0;
            Habilitado = null;
            UsuarioModificacion = string.Empty;
            FechaModificacion = null;
            UsuarioCreacion = string.Empty;
            FechaCreacion = null;
            ClasificacionSBIF = string.Empty;
            ClasificacionPAF = string.Empty;
            ObjetoSociedad = string.Empty;
            EmailFacturacion = string.Empty;
        }

        #endregion
    }
}
