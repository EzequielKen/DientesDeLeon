using _01___modulos.MySQL;
using _02___sistemas._01___Servicios;
using _02___sistemas._04___Atencion;
using Mysqlx.Datatypes;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._06___Tecnico
{
    public class cls_Tecnico
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        cls_ListaServicio listaServicio = new cls_ListaServicio();
        cls_AdministrarAtencion administrarAtencion = new cls_AdministrarAtencion();
        cls_funciones funciones = new cls_funciones();
        DataTable serviciosDeTecnicoBD;

        #region carga a base de datos
        public async Task<bool> eliminar_atencion_de_tecnico(string id)
        {
            string actualizar = "`activo`='0'";
            return await consultas.actualizar_tabla("atencion_de_tecnico", actualizar, id);
        }
        public async Task<bool> agregar_servicio_a_tecnico(DataTable servicioDeTecnico)
        {
            bool retorno = false;
            var query = funciones.armar_query_insertar(servicioDeTecnico);
            retorno = await consultas.insertar_en_tabla("atencion_de_tecnico", query.columnas, query.valores);
            return retorno;
        }
        #endregion

        #region metodos consultas
        public async Task consultar_servicios_de_tecnico(string id_tecnico, string id_consultorio)
        {
            serviciosDeTecnicoBD = await consultas.consultar_atencion_de_tecnico(id_tecnico, id_consultorio);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> GetClone()
        {
            DataTable atencion_de_tecnico = await consultas.consultar_tabla("atencion_de_tecnico");
            return atencion_de_tecnico.Clone();
        }
        public async Task<DataTable> getServiciosDeTecnico(string id_tecnico, string id_consultorio)
        {
            await consultar_servicios_de_tecnico(id_tecnico, id_consultorio);
            serviciosDeTecnicoBD.Columns.Add("servicio", typeof(string));
            serviciosDeTecnicoBD.Columns.Add("sala", typeof(string));

            DataTable servicios = await listaServicio.getServiciosActivos(id_consultorio);
            DataTable atencion_de_sala = await consultas.consultar_tabla("atencion_de_sala"); 
            DataTable salas = await consultas.consultar_tabla("salas"); 

            string id_servicio, id_sala;
            int fila_servicio,fila_sala;
            for (int fila = 0; fila <= serviciosDeTecnicoBD.Rows.Count - 1; fila++)
            {
                id_servicio = serviciosDeTecnicoBD.Rows[fila]["id_servicio"].ToString();
                fila_servicio = funciones.buscar_fila_por_dato(id_servicio, "id", servicios);
                if (fila_servicio != -1)
                {
                    serviciosDeTecnicoBD.Rows[fila]["Servicio"] = servicios.Rows[fila_servicio]["Servicio"].ToString();
                    fila_sala = funciones.buscar_fila_por_dato(id_servicio, "id_servicio", atencion_de_sala);
                    id_sala = atencion_de_sala.Rows[fila_sala]["id_sala"].ToString();
                    fila_sala = funciones.buscar_fila_por_dato(id_sala, "id", salas);
                    serviciosDeTecnicoBD.Rows[fila]["Sala"] = salas.Rows[fila_sala]["Sala"].ToString();
                }
            }
            return serviciosDeTecnicoBD;
        }

        public async Task<DataTable> BuscarServicioActivo(string servicio, string id_consultorio)
        {

            // Preparo el término a buscar (ignoro nulos, espacios, mayúsculas y acentos)
            string term = (servicio ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable serviciosBD = await listaServicio.getServiciosActivos(id_consultorio);
            DataTable listaServicios = serviciosBD.Clone();

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

        public async Task<DataTable> BuscarAtencionDePaciente(string Atencion, string id_consultorio, string id_usuario)
        {

            // Preparo el término a buscar (ignoro nulos, espacios, mayúsculas y acentos)
            string term = (Atencion ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable atencionBD = await getServiciosDeTecnico(id_usuario, id_consultorio);
            DataTable listaAtencion = atencionBD.Clone();

            // Si no hay término, devuelvo todo tal cual
            bool filtrar = !string.IsNullOrEmpty(normTerm);

            for (int fila = 0; fila < atencionBD.Rows.Count; fila++)
            {
                string servicioNombre = atencionBD.Rows[fila]["Servicio"]?.ToString() ?? string.Empty;

                // Normalizo el valor de la BD
                string normSalaNombre = RemoveDiacritics(servicioNombre).ToUpperInvariant();

                // ¿Coincide?
                bool coincide = !filtrar || normSalaNombre.Contains(normTerm);

                if (!coincide) continue;

                DataRow row = atencionBD.Rows[fila];
                listaAtencion.ImportRow(row);

            }

            return listaAtencion;

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
        #endregion
    }
}
