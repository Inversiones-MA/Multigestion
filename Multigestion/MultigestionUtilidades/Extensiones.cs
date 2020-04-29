using DevExpress.Web;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace MultigestionUtilidades
{
   public static class Extensiones
    {
       public static decimal GetValorDecimal(this string valor)
       {
           decimal retorno = 0;
           if (string.IsNullOrEmpty(valor))
               return retorno;
           else
           {
               var str = valor.Replace(".", "").Replace(",", ".");
               retorno = Convert.ToDecimal(str, CultureInfo.InvariantCulture);
           }
           return retorno;
       }
       public static string GetFormatearNumero(this string valor, int decimales)
       {
           if (valor == null)
               return "0";
           double NuevoValor = Convert.ToDouble(valor);

           return NuevoValor.ToString(string.Format("{0}{1}", "N", decimales), CultureInfo.CreateSpecificCulture("es-ES"));
       }
       public static double Redondear(this double valor, int round)
       {
           return Math.Round(valor, round);
       }

       public static bool IsNumeric(this string s)
       {
           float output;
           return float.TryParse(s, out output);
       }

       public static double GetValorDouble(this string valor)
       {
           double retorno = 0;
           if (string.IsNullOrEmpty(valor))
               return retorno;
           else
           {
               var str = valor.Replace(".", "").Replace(",", ".");
               retorno = Convert.ToDouble(str, CultureInfo.InvariantCulture);
           }
           return retorno;
       }

       public static float GetValorFloat(this string valor)
       {
           float retorno = 0;
           if (string.IsNullOrEmpty(valor))
               return retorno;
           else
           {
               var str = valor.Replace(".", "").Replace(",", ".");
               retorno = float.Parse(str, CultureInfo.InvariantCulture);
           }
           return retorno;
       }

       public static float GetValorFloat2(this string valor)
       {
           float retorno = 0;
           if (string.IsNullOrEmpty(valor))
               return retorno;
           else
           {
               var str = valor.Replace(",", ".");
               retorno = float.Parse(str, CultureInfo.InvariantCulture);
           }
           return retorno;
       }
       //public static string GetFormatearValor(this string valor)
       //{
       //    double retorno = 0;
       //    if (string.IsNullOrEmpty(valor))
       //        return retorno;
       //    else
       //    {
       //        var str = valor.Replace(".", "").Replace(",", ".");
       //        retorno = Convert.ToDouble(str, CultureInfo.InvariantCulture);
       //    }
       //    return retorno;
       //}

       public static double GetValorDoubleGv(this string valor)
       {
           double retorno = 0;
           if (string.IsNullOrEmpty(valor))
               return retorno;
           else
           {
               var str = valor.Replace(",", ".");
               retorno = Convert.ToDouble(str, CultureInfo.InvariantCulture);
           }
           return retorno;
       }

       public static int GetValorInteger(this string valor)
       {
           int retorno = 0;
           if (string.IsNullOrEmpty(valor))
               return retorno;
           else
           {
               var str = valor.Replace(".", "").Replace(",", ".");
               retorno = Convert.ToInt32(str, CultureInfo.InvariantCulture);
           }
           return retorno;
       }

       public static bool SinDatos(this DataTable obj)
       {
           if (obj == null || obj.Rows.Count == 0)
               return true;
           else
               return false;
       }

       //public static void DivAlerta(this System.Web.UI.HtmlControls.HtmlGenericControl div)
       //{
       //    div.Style.Add("display", "block");      
       //}

       //public static void MensajeAlerta(this Label lbl, string mensaje)
       //{
       //    lbl.Text = mensaje;
       //}
    }
}
