using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelectListItem = Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;

namespace _02___sistemas
{
    public class cls_GeoRef
    {
        /// <summary>
        /// Obtiene todas las provincias de Argentina.
        /// </summary>
        public List<SelectListItem> GetProvincias()
        {
            var lista = new List<SelectListItem>();  // aquí SelectListItem es Microsoft.AspNetCore.Mvc.Rendering

            using (var client = new HttpClient())
            {
                var resp = client.GetAsync("https://apis.datos.gob.ar/georef/api/provincias")
                                 .Result;
                resp.EnsureSuccessStatusCode();

                var json = resp.Content.ReadAsStringAsync().Result;
                var data = JObject.Parse(json);

                foreach (var item in data["provincias"])
                {
                    lista.Add(new SelectListItem
                    {
                        Value = (string)item["nombre"],
                        Text = (string)item["nombre"]
                    });
                }
            }

            return lista;
        }
        /// <summary>
        /// Obtiene las localidades de la provincia indicada.
        /// </summary>
        /// <param name="provinciaNombre">Nombre exacto de la provincia (ej. "Buenos Aires")</param>
        public List<SelectListItem> GetLocalidades(string provinciaId)
        {
            var lista = new List<SelectListItem>();
            using var client = new HttpClient();
            // Límite alto para devolver todas las localidades
            var url = $"https://apis.datos.gob.ar/georef/api/localidades?provincia={provinciaId}&max=500";
            var resp = client.GetAsync(url).Result;
            resp.EnsureSuccessStatusCode();
            var json = resp.Content.ReadAsStringAsync().Result;
            var data = JObject.Parse(json);
            foreach (var item in data["localidades"])
            {
                lista.Add(new SelectListItem
                {
                    Value = (string)item["id"],
                    Text = (string)item["nombre"]
                });
            }
            return lista;
        }
    }
}
