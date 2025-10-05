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
        public List<SelectListItem> GetProvincias()
        {
            var lista = new List<SelectListItem>();  

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
        public List<SelectListItem> GetLocalidades(string provinciaId)
        {
            var lista = new List<SelectListItem>();
            using var client = new HttpClient();
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
