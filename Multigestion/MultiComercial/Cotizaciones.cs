using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MultiComercial
{
    //[DataContract(Name = "Cotizaciones")]
    public class Cotizaciones
    {
        public Nullable<int> IdCotizacion { get; set; }
        //[DataMember(Name = "IdEmpresa")]
        public Nullable<int> IdEmpresa { get; set; }
        
        public Nullable<int> IdOperacion { get; set; }

        //[DataMember(Name = "RazonEmpresa")]
        public string RazonEmpresa { get; set; }

        //[DataMember(Name = "IdEjecutivo")]
        public Nullable<int> IdEjecutivo { get; set; }

        //[DataMember(Name = "Ejecutivo")]
        public string Ejecutivo { get; set; }

        public string Rut { get; set; }

        public string DivRut { get; set; }

        public Nullable<int> IdProducto { get; set; }

        public string DescProducto { get; set; }
        
        //[DataMember(Name = "IdTipoCredito")]
        public Nullable<int> IdTipoCredito { get; set; }

        //[DataMember(Name = "TipoCredito")]
        public string TipoCredito { get; set; }

        //[DataMember(Name = "MontoLiquido")]
        public Nullable<double> MontoLiquido { get; set; }

        //[DataMember(Name = "MontoBruto")]
        public Nullable<double> MontoBruto { get; set; }

        //[DataMember(Name = "IdTipoMoneda")]
        public Nullable<int> IdTipoMoneda { get; set; }

        //[DataMember(Name = "TipoMoneda")]
        public string TipoMoneda { get; set; }

        //[DataMember(Name = "Plazo")]
        public Nullable<int> Plazo { get; set; }

        //[DataMember(Name = "TasaBanco")]
        public Nullable<double> TasaBanco { get; set; }

        //[DataMember(Name = "IdPeriocidadTasa")]
        public Nullable<int> IdPeriocidadTasa { get; set; }

        //[DataMember(Name = "PeriocidadTasa")]
        public string PeriocidadTasa { get; set; }

        //[DataMember(Name = "IdEstructuraPago")]
        public Nullable<int> IdEstructuraPago { get; set; }

        //[DataMember(Name = "EstructuraPago")]
        public string EstructuraPago { get; set; }

        //[DataMember(Name = "FechaCurse")]
        public Nullable<DateTime> FechaCurse { get; set; }

        //[DataMember(Name = "Fecha1Vencimiento")]
        public Nullable<DateTime> Fecha1Vencimiento { get; set; }

        //[DataMember(Name = "FechaVencimiento")]
        public Nullable<DateTime> FechaVencimiento { get; set; }

        //[DataMember(Name = "PlazoHorizonte")]
        public Nullable<int> PlazoHorizonte { get; set; }

        //[DataMember(Name = "CoberturaCF")]
        public Nullable<int> CoberturaCF { get; set; }

        public Nullable<int> IdCoberturaCF { get; set; }
        
        //[DataMember(Name = "CuotasGracias")]
        public Nullable<int> CuotasGracias { get; set; }

        //[DataMember(Name = "CapitalizaInteres")]
        public bool CapitalizaInteres { get; set; }

        //[DataMember(Name = "ComisionIncluida")]
        public bool ComisionIncluida { get; set; }

        public bool NotarioIncluido { get; set; }
        
        //[DataMember(Name = "CoberturaFogape")]
        public Nullable<double> CoberturaFogape { get; set; }

        public Nullable<bool> GtiaFOGAPE { get; set; }
        //[DataMember(Name = "ComisionAnual")]
        public Nullable<double> ComisionAnual { get; set; }

        //[DataMember(Name = "IdFondo")]
        public Nullable<int> IdFondo { get; set; }

        //[DataMember(Name = "Fondo")]
        public string Fondo { get; set; }

        //[DataMember(Name = "GastosFogape")]
        public Nullable<double> GastosFogape { get; set; }

        //[DataMember(Name = "GastosLegales")]
        public Nullable<double> GastosLegales { get; set; }

        //[DataMember(Name = "GastosNotaria")]
        public Nullable<double> GastosNotaria { get; set; }

        public Nullable<double> UF { get; set; }

        public Nullable<double> USD { get; set; }

        public Nullable<double> ComisionMultiAval { get; set; }

        public Nullable<decimal> ImpuestoTimbre { get; set; }

        public Nullable<decimal> SeguroGarantia { get; set; }

        public Nullable<decimal> SeguroDesgravamen { get; set; }

        public Nullable<decimal> CargaFinanciera { get; set; }

        public Nullable<int> UsuarioCreacion { get; set; }

        //[DataMember(Name = "SeguroGarantias")]
        public List<SeguroGarantia> SeguroGarantias { get; set; }

        //[DataMember(Name = "SeguroAvales")]
        public List<SeguroAval> SeguroAvales { get; set; }

        public string TipoCotizacion { get; set; }

        public Cotizaciones()
        {

        }
    }
}
