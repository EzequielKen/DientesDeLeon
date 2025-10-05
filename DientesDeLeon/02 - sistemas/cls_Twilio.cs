using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PhoneNumbers;
using System.Web.Mvc;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;


namespace _02___sistemas
{
    public class cls_Twilio
    {
       
        public List<SelectListItem> ObtenerCodigosTelefonicosPorPais()
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var supportedRegions = phoneUtil.GetSupportedRegions();

            var culturasValidas = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => new RegionInfo(ci.Name).TwoLetterISORegionName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var lista = new List<SelectListItem>();

            var regionesFiltradas = supportedRegions
                .Where(r => culturasValidas.Contains(r))
                .OrderBy(r => r, StringComparer.OrdinalIgnoreCase);

            foreach (var region in regionesFiltradas)
            {
                var codigo = phoneUtil.GetCountryCodeForRegion(region);
                lista.Add(new SelectListItem
                {
                    Text = $"{region} (+{codigo})",
                    Value = $"+{codigo}"
                });
            }

            return lista;
        }
       
        public (bool verificado, string E164) FormatearNumeroE164(string codigoPais, string codigoArea, string numeroLocal)
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();

            if (!int.TryParse(codigoPais.Trim().TrimStart('+'), out int prefijo))
            {
                return (false, "Código de país inválido");
            }

            var region = phoneUtil.GetRegionCodeForCountryCode(prefijo);
            if (string.IsNullOrEmpty(region))
            {
                return (false, "No se pudo determinar la región para el código de país");
            }

            var nacional = $"{codigoArea.Trim()}{numeroLocal.Trim()}";

            var number = phoneUtil.Parse(nacional, region);

            if (!phoneUtil.IsValidNumber(number))
            {
                return (false, "El número ingresado no es válido para la región especificada");
            }

            return (true, phoneUtil.Format(number, PhoneNumberFormat.E164).Replace("+", ""));
        }
    }
}
