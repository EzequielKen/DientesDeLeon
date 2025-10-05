using _02___sistemas._01___Servicios;
using _02___sistemas._03___Sala;
using System.Data;
using System.Globalization;
using System.Text;

namespace PaginaWeb.Servicios._03___Sala
{
    public class ListarSalaServicio
    {
        cls_ListaSala listaSalas = new cls_ListaSala();

        public async Task<DataTable> ObtenerTodasLasSalas(string id_consultorio)
        {
            DataTable salasBD = await this.listaSalas.getSalas(id_consultorio);
            DataTable listaSalas = new DataTable();
            listaSalas.Columns.Add("activo", typeof(string));
            listaSalas.Columns.Add("id_consultorio", typeof(string));
            listaSalas.Columns.Add("ID", typeof(string));
            listaSalas.Columns.Add("Sala", typeof(string));

            int ultimaFila;
            for (int fila = 0; fila <= salasBD.Rows.Count - 1; fila++)
            {
                listaSalas.Rows.Add();
                ultimaFila = listaSalas.Rows.Count - 1;
                listaSalas.Rows[ultimaFila]["activo"] = salasBD.Rows[fila]["activo"].ToString();
                listaSalas.Rows[ultimaFila]["id_consultorio"] = salasBD.Rows[fila]["id_consultorio"].ToString();
                listaSalas.Rows[ultimaFila]["ID"] = salasBD.Rows[fila]["id"].ToString();
                listaSalas.Rows[ultimaFila]["Sala"] = salasBD.Rows[fila]["Sala"].ToString();
            }

            return listaSalas;
        }
        public async Task<DataTable> BuscarSala(string sala, string id_consultorio)
        {
            DataTable salasBD = await this.listaSalas.getSalas(id_consultorio);

            string term = (sala ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable listaSalas = new DataTable();
            listaSalas.Columns.Add("activo", typeof(string));
            listaSalas.Columns.Add("id_consultorio", typeof(string));
            listaSalas.Columns.Add("ID", typeof(string));
            listaSalas.Columns.Add("Sala", typeof(string));

            bool filtrar = !string.IsNullOrEmpty(normTerm);

            for (int fila = 0; fila < salasBD.Rows.Count; fila++)
            {
                string salaNombre = salasBD.Rows[fila]["Sala"]?.ToString() ?? string.Empty;

                string normSalaNombre = RemoveDiacritics(salaNombre).ToUpperInvariant();

                bool coincide = !filtrar || normSalaNombre.Contains(normTerm);

                if (!coincide) continue;

                int nueva = listaSalas.Rows.Add().Table.Rows.Count - 1;
                listaSalas.Rows[nueva]["activo"] = salasBD.Rows[fila]["activo"]?.ToString();
                listaSalas.Rows[nueva]["id_consultorio"] = salasBD.Rows[fila]["id_consultorio"]?.ToString();
                listaSalas.Rows[nueva]["ID"] = salasBD.Rows[fila]["id"]?.ToString();
                listaSalas.Rows[nueva]["Sala"] = salaNombre;
            }

            return listaSalas;

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
