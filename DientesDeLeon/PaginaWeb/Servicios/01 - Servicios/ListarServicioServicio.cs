using _02___sistemas;
using _02___sistemas._01___Servicios;
using Org.BouncyCastle.Asn1;
using PaginaWeb.Models;
using System.Data;
using System.Globalization;
using System.Text;

namespace PaginaWeb.Servicios._01___Servicios
{
    public class ListarServicioServicio
    {
        cls_ListaServicio listaServicio = new cls_ListaServicio();
        cls_funciones funciones = new cls_funciones();
        ServicioViewModel ServicioEditar = new ServicioViewModel();
        DataTable serviciosBD;
        private async Task<DataTable> consultar_servicios(string id_consultorio)
        {
            serviciosBD = await listaServicio.getServicios(id_consultorio);
            DataTable listaServicios = new DataTable();
            listaServicios.Columns.Add("activo", typeof(string));
            listaServicios.Columns.Add("ID", typeof(string)); 
            listaServicios.Columns.Add("id_Consultorio", typeof(string)); 
            listaServicios.Columns.Add("Servicio", typeof(string));
            listaServicios.Columns.Add("Precio", typeof(string));

            return listaServicios;
        }
        private async Task<DataTable> consultar_servicios_activos(string id_consultorio)
        {
            serviciosBD = await listaServicio.getServiciosActivos(id_consultorio);
            DataTable listaServicios = new DataTable();
            listaServicios.Columns.Add("activo", typeof(string));
            listaServicios.Columns.Add("ID", typeof(string));
            listaServicios.Columns.Add("id_Consultorio", typeof(string));
            listaServicios.Columns.Add("Servicio", typeof(string));
            listaServicios.Columns.Add("Precio", typeof(string));

            return listaServicios;
        }
        public async Task<DataTable> ObtenerTodosLosServicios(string id_consultorio)
        {
            DataTable listaServicios = await consultar_servicios(id_consultorio);

            int ultimaFila;
            for (int fila = 0; fila <= serviciosBD.Rows.Count - 1; fila++)
            {
                listaServicios.Rows.Add();
                ultimaFila = listaServicios.Rows.Count - 1;
                listaServicios.Rows[ultimaFila]["activo"] = serviciosBD.Rows[fila]["activo"].ToString();
                listaServicios.Rows[ultimaFila]["ID"] = serviciosBD.Rows[fila]["id"].ToString(); 
                listaServicios.Rows[ultimaFila]["id_Consultorio"] = serviciosBD.Rows[fila]["id_Consultorio"].ToString(); 
                listaServicios.Rows[ultimaFila]["Servicio"] = serviciosBD.Rows[fila]["Servicio"].ToString();
                listaServicios.Rows[ultimaFila]["Precio"] = serviciosBD.Rows[fila]["Precio"].ToString();
            }

            return listaServicios;
        }

        public async Task<DataTable> ObtenerTodosLosServiciosActivos(string id_consultorio)
        {
            DataTable listaServicios = await consultar_servicios_activos(id_consultorio);

            int ultimaFila;
            for (int fila = 0; fila <= serviciosBD.Rows.Count - 1; fila++)
            {
                listaServicios.Rows.Add();
                ultimaFila = listaServicios.Rows.Count - 1;
                listaServicios.Rows[ultimaFila]["activo"] = serviciosBD.Rows[fila]["activo"].ToString();
                listaServicios.Rows[ultimaFila]["ID"] = serviciosBD.Rows[fila]["id"].ToString();
                listaServicios.Rows[ultimaFila]["id_Consultorio"] = serviciosBD.Rows[fila]["id_Consultorio"].ToString();
                listaServicios.Rows[ultimaFila]["Servicio"] = serviciosBD.Rows[fila]["Servicio"].ToString();
                listaServicios.Rows[ultimaFila]["Precio"] = serviciosBD.Rows[fila]["Precio"].ToString();
            }

            return listaServicios;
        }

        public async Task<ServicioViewModel> ObtenerServicioPorId(string idServicio)
        {
            DataTable servicioBD = await listaServicio.getServicio_id(idServicio);
            ServicioEditar.id = servicioBD.Rows[0]["id"].ToString();
            ServicioEditar.servicio = servicioBD.Rows[0]["Servicio"].ToString();
            ServicioEditar.precio = servicioBD.Rows[0]["Precio"].ToString();
            return ServicioEditar;
        }

        public async Task<DataTable> CambiarEstadoDeServicio(string idServicio)
        {
            DataTable serviciosBD = await listaServicio.getServicio_id(idServicio);

            string estado = serviciosBD.Rows[0]["activo"].ToString();
            string cambiarEstado;
            if (estado == "1")
            {
                cambiarEstado = "0"; // Cambiar a inactivo
            }
            else
            {
                cambiarEstado = "1"; // Cambiar a activo
            }
            listaServicio.cambiar_estado_servicio(idServicio, cambiarEstado);

            return serviciosBD;
        }

        public async Task<DataTable> BuscarServicio(string servicio, string id_consultorio)
        {

            // Preparo el término a buscar (ignoro nulos, espacios, mayúsculas y acentos)
            string term = (servicio ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable listaServicios = await consultar_servicios(id_consultorio);

            // Si no hay término, devuelvo todo tal cual
            bool filtrar = !string.IsNullOrEmpty(normTerm);

            for (int fila = 0; fila < serviciosBD.Rows.Count; fila++)
            {
                string servicioNombre = serviciosBD.Rows[fila]["Servicio"]?.ToString() ?? string.Empty;

                // Normalizo el valor de la BD
                string normSalaNombre = RemoveDiacritics(servicioNombre).ToUpperInvariant();

                // ¿Coincide?
                bool coincide = !filtrar || normSalaNombre.Contains(normTerm);

                if (!coincide) continue;

                int nueva = listaServicios.Rows.Add().Table.Rows.Count - 1;
                listaServicios.Rows[nueva]["activo"] = serviciosBD.Rows[fila]["activo"]?.ToString();
                listaServicios.Rows[nueva]["id_consultorio"] = serviciosBD.Rows[fila]["id_consultorio"]?.ToString();
                listaServicios.Rows[nueva]["ID"] = serviciosBD.Rows[fila]["id"]?.ToString();
                listaServicios.Rows[nueva]["Servicio"] = servicioNombre;
                listaServicios.Rows[nueva]["Precio"] = serviciosBD.Rows[fila]["Precio"].ToString();

            }

            return listaServicios;

            // ==== Helpers ====
            static string RemoveDiacritics(string text)
            {
                if (string.IsNullOrEmpty(text)) return text;
                var normalized = text.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder(capacity: normalized.Length);
                foreach (var ch in normalized)
                {
                    var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                    if (uc != UnicodeCategory.NonSpacingMark)
                        sb.Append(ch);
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }
        }
        public async Task<DataTable> BuscarServicioActivo(string servicio, string id_consultorio)
        {

            // Preparo el término a buscar (ignoro nulos, espacios, mayúsculas y acentos)
            string term = (servicio ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable listaServicios = await consultar_servicios_activos(id_consultorio);

            // Si no hay término, devuelvo todo tal cual
            bool filtrar = !string.IsNullOrEmpty(normTerm);

            for (int fila = 0; fila < serviciosBD.Rows.Count; fila++)
            {
                string servicioNombre = serviciosBD.Rows[fila]["Servicio"]?.ToString() ?? string.Empty;

                // Normalizo el valor de la BD
                string normSalaNombre = RemoveDiacritics(servicioNombre).ToUpperInvariant();

                // ¿Coincide?
                bool coincide = !filtrar || normSalaNombre.Contains(normTerm);

                if (!coincide) continue;

                int nueva = listaServicios.Rows.Add().Table.Rows.Count - 1;
                listaServicios.Rows[nueva]["activo"] = serviciosBD.Rows[fila]["activo"]?.ToString();
                listaServicios.Rows[nueva]["id_consultorio"] = serviciosBD.Rows[fila]["id_consultorio"]?.ToString();
                listaServicios.Rows[nueva]["ID"] = serviciosBD.Rows[fila]["id"]?.ToString();
                listaServicios.Rows[nueva]["Servicio"] = servicioNombre;
                listaServicios.Rows[nueva]["Precio"] = serviciosBD.Rows[fila]["Precio"].ToString();

            }

            return listaServicios;

            // ==== Helpers ====
            static string RemoveDiacritics(string text)
            {
                if (string.IsNullOrEmpty(text)) return text;
                var normalized = text.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder(capacity: normalized.Length);
                foreach (var ch in normalized)
                {
                    var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                    if (uc != UnicodeCategory.NonSpacingMark)
                        sb.Append(ch);
                }
                return sb.ToString().Normalize(NormalizationForm.FormC);
            }
        }

    }
}
