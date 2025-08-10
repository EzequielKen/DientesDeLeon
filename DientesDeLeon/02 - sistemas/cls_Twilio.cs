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
        /// <summary>
        /// Devuelve un DataTable con columnas “Pais” y “Codigo”,
        /// donde “Pais” es la sigla ISO combinada con el prefijo (p.ej. AR(+54))
        /// y “Codigo” es el prefijo con “+” (p.ej. +54), todo ordenado por la sigla.
        /// </summary>
        public List<SelectListItem> ObtenerCodigosTelefonicosPorPais()
        {
            var phoneUtil = PhoneNumberUtil.GetInstance();
            var supportedRegions = phoneUtil.GetSupportedRegions();

            // Códigos ISO válidos para RegionInfo
            var culturasValidas = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(ci => new RegionInfo(ci.Name).TwoLetterISORegionName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Lista de salida
            var lista = new List<SelectListItem>();

            // Filtrar, ordenar y poblar la lista
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
        /// <summary>
        /// Toma código de país (“+54”), código de área (“11”) y número local (“12345678”)
        /// y devuelve el número en formato E.164 (“+541112345678”).
        /// </summary>
        public (bool verificado, string E164) FormatearNumeroE164(string codigoPais, string codigoArea, string numeroLocal)
        {
            // Instancia de libphonenumber
            var phoneUtil = PhoneNumberUtil.GetInstance();

            // Convertir "+54" → 54
            if (!int.TryParse(codigoPais.Trim().TrimStart('+'), out int prefijo))
            {
                return (false, "Código de país inválido");
            }

            // Obtener región ISO para el prefijo (p.ej. "AR" para 54)
            var region = phoneUtil.GetRegionCodeForCountryCode(prefijo);
            if (string.IsNullOrEmpty(region))
            {
                return (false, "No se pudo determinar la región para el código de país");
            }

            // Construir la parte nacional: área + número local
            var nacional = $"{codigoArea.Trim()}{numeroLocal.Trim()}";

            // Parsear como número nacional de esa región
            var number = phoneUtil.Parse(nacional, region);

            // Validar
            if (!phoneUtil.IsValidNumber(number))
            {
                return (false, "El número ingresado no es válido para la región especificada");
            }

            // Formatear en E.164
            return (true, phoneUtil.Format(number, PhoneNumberFormat.E164).Replace("+", ""));
        }
    }
}
