using MailChimp;
using MailChimp.Types;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultigestionUtilidades;

namespace ClasesServicios
{
    public class MandrillMail
    {
        private MandrillApi MandrillApi { get; set; }
        public Enum TemplateTipoCorreo { get; set; }
        private Mandrill.Messages.Recipient[] Destintarios { get; set; }

        //public MandrillMail()
        //{
        //    //MandrillApi = new MandrillApi(Configuracion.Mandrill.ApiKey);
        //}

        public MandrillMail(Enumeradores.ETemplateCorreos PlantillaCorreo)
        {
            MandrillApi = new MandrillApi(new ConfiguracionMailchimp { }.ApiKey);
            TemplateTipoCorreo = PlantillaCorreo;
        }

        public MVList<Mandrill.Messages.SendResult> EnviarCorreo(string email, string nombre, Mandrill.NameContentList<string> contenidoTemplate, bool copiaOculta = false)
        {
            Utilidades util = new Utilidades();
            Destintarios = new[] { new Mandrill.Messages.Recipient(email, nombre) };

            var mensaje = new Mandrill.Messages.Message
            {
                //Definidos en Template
                //Html = string.Empty,
                //Text = string.Empty,
                //Subject = subject,
                //FromEmail = string.Empty,
                //FromName = string.Empty,
                To = Destintarios,
                Headers = Opt<MCDict<Mandrill.Messages.Header>>.None,
                Important = false,
                TrackOpens = true,
                TrackClicks = true,
                AutoText = false,
                AutoHtml = true,
                InlineCss = true,
                PreserveRecipients = false,
                TrackingDomain = "https://www.multigarantias.cl",
                BccAddress = "estebansanchezl@gmail.com",
            };

            var resultado = MandrillApi.SendTemplate(util.TraeDescripcionEnum(TemplateTipoCorreo), contenidoTemplate, mensaje, FSharpOption<bool>.None);
            return resultado;
        }

        //public MVList<Mandrill.Messages.SendResult> EnviarCorreoProcesoBatch(string email, string nombre, Mandrill.NameContentList<string> contenidoTemplate, bool copiaOculta, string subject)
        //{
        //    var template = UtilString.TraeDescripcionEnum(Template);
        //    Destintarios = new[] { new Mandrill.Messages.Recipient(email, nombre) };

        //    var mensaje = new Mandrill.Messages.Message
        //    {
        //        Subject = subject,
        //        To = Destintarios,
        //        Headers = Opt<MCDict<Mandrill.Messages.Header>>.None,
        //        Important = false,
        //        TrackOpens = true,
        //        TrackClicks = true,
        //        AutoText = false,
        //        AutoHtml = true,
        //        InlineCss = true,
        //        PreserveRecipients = false,
        //        TrackingDomain = "https://www.multigarantias.cl",
        //        BccAddress = copiaOculta ? "sistemas@multiaval.cl" : string.Empty,
        //    };

        //    var resultado = MandrillApi.SendTemplate(template, contenidoTemplate, mensaje, FSharpOption<bool>.None);
        //    return resultado;
        //}

        //public MVList<Mandrill.Messages.SendResult> EnviarCorreoError(string mensajeError, string plataforma, string asunto)
        //{
        //    var email = "incidentes@multisf.cl";
        //    var nombre = "SoporteTI";
        //    var template = UtilString.TraeDescripcionEnum(Enumeradores.ETemplate.MultiGarantiasError);
        //    var contenidoTemplate = new Mandrill.NameContentList<string>{
        //        {"nombreSistema", plataforma},
        //        {"msgError", mensajeError},
        //    };
        //    Destintarios = new[] { new Mandrill.Messages.Recipient(email, nombre) };

        //    var mensaje = new Mandrill.Messages.Message
        //    {
        //        To = Destintarios,
        //        Subject = asunto,
        //        Headers = Opt<MCDict<Mandrill.Messages.Header>>.None,
        //        Important = false,
        //        TrackOpens = true,
        //        TrackClicks = true,
        //        AutoText = false,
        //        AutoHtml = true,
        //        InlineCss = true,
        //        PreserveRecipients = false,
        //        TrackingDomain = "https://www.multigarantias.cl",
        //    };

        //    var resultado = MandrillApi.SendTemplate(template, contenidoTemplate, mensaje);
        //    return resultado;
        //}

        //public MVList<Mandrill.Messages.SendResult> EnviarCorreoReemision(Mandrill.NameContentList<string> contenidoTemplate, string asunto, string correoEjecutiva)
        //{
        //    var email = correoEjecutiva;
        //    var nombre = "Reemitir CF";
        //    var template = UtilString.TraeDescripcionEnum(Enumeradores.ETemplate.SolicitudReemitirCF);
        //    Destintarios = new[] { new Mandrill.Messages.Recipient(email, nombre) };

        //    var mensaje = new Mandrill.Messages.Message
        //    {
        //        To = Destintarios,
        //        BccAddress = "jnunez@multisf.cl",
        //        Subject = asunto,
        //        Headers = Opt<MCDict<Mandrill.Messages.Header>>.None,
        //        Important = false,
        //        TrackOpens = true,
        //        TrackClicks = true,
        //        AutoText = false,
        //        AutoHtml = true,
        //        InlineCss = true,
        //        PreserveRecipients = false,
        //        TrackingDomain = "https://www.multigarantias.cl",
        //    };

        //    var resultado = MandrillApi.SendTemplate(template, contenidoTemplate, mensaje);
        //    return resultado;
        //}

        //public MVList<Mandrill.Messages.SendResult> EnviarCorreoCotizacion(Mandrill.NameContentList<string> contenidoTemplate, string email)
        //{
        //    var nombre = "Usuario";
        //    //var asunto = "Prueba";
        //    var template = UtilString.TraeDescripcionEnum(Enumeradores.ETemplate.CFTCotizacion);
        //    Destintarios = new[] { new Mandrill.Messages.Recipient(email, nombre) };

        //    var mensaje = new Mandrill.Messages.Message
        //    {
        //        To = Destintarios,
        //        //Subject = asunto,
        //        Headers = Opt<MCDict<Mandrill.Messages.Header>>.None,
        //        Important = false,
        //        TrackOpens = true,
        //        TrackClicks = true,
        //        AutoText = false,
        //        AutoHtml = true,
        //        InlineCss = true,
        //        PreserveRecipients = false,
        //        TrackingDomain = "https://www.multigarantias.cl",
        //    };

        //    var resultado = MandrillApi.SendTemplate(template, contenidoTemplate, mensaje);
        //    return resultado;
        //}
    }

    public class ConfiguracionMailchimp
    {
        public string UrlWeb { get; set; }
        public string UrlApi { get; set; }
        public string ApiKey { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }

        public ConfiguracionMailchimp()
        {
            UrlApi = string.Empty;
            ApiKey = "5816e54762e42eadc3723916f137fb09-us16";
            User = string.Empty;
            Pass = string.Empty;
            UrlWeb = "http://developer.mailchimp.com/";
        }
    }
}
