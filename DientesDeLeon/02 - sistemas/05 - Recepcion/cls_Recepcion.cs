using _01___modulos.MySQL;
using _02___sistemas._01___Paciente;
using _02___sistemas._01___Servicios;
using _02___sistemas._04___Atencion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._05___Recepcion
{
    public class cls_Recepcion
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        cls_funciones funciones = new cls_funciones();
        cls_ListaServicio listaServicio = new cls_ListaServicio();
        cls_AdministrarAtencion administrarAtencion = new cls_AdministrarAtencion();
        DataTable paciente;
        DataTable atencionDePaciente;

        #region carga a base de datos
        public async Task<bool> eliminar_atencion(string id)
        {
            string actualizar = "`activo`='0'";
            return await consultas.actualizar_tabla("atencion", actualizar, id);
        }
        public async Task<bool> enviar_paciente_a_espera(DataTable PacienteEspera)
        {
            bool retorno = false;
            string id_servicio = PacienteEspera.Rows[0]["id_servicio"].ToString();

            DataTable servicio = await listaServicio.getServicio_id(id_servicio);
            PacienteEspera.Rows[0]["precio"] = servicio.Rows[0]["Precio"].ToString();
            PacienteEspera.Rows[0]["servicio"] = servicio.Rows[0]["servicio"].ToString();


            //id_servicio id_consultorio 
            string id_consultorio = PacienteEspera.Rows[0]["id_consultorio"].ToString();
            DataTable AtencionDeSalaBD = await administrarAtencion.getAtencion_porServicio(id_servicio, id_consultorio);
            PacienteEspera.Rows[0]["id_sala"] = AtencionDeSalaBD.Rows[0]["id_Sala"].ToString();

            var query = funciones.armar_query_insertar(PacienteEspera);
            retorno = await consultas.insertar_en_tabla("atencion", query.columnas, query.valores);
            return retorno;
        }
        #endregion

        #region metodos consultas
        private async Task consultar_paciente_por_id(string Id)
        {
            paciente = await consultas.consultar_paciente_por_id(Id);
        }
        private async Task consultar_atencion_de_paciente(string id_consultorio, string id_usuario)
        {
            atencionDePaciente = await consultas.consultar_atencion_de_usuario(id_consultorio,id_usuario);
        }
        #endregion

        #region metodos get/set
        public async Task<DataTable> getPacientePorId(string Id)
        {
            await consultar_paciente_por_id(Id);
            return paciente;
        }
        public async Task<DataTable> getServicios(string id_consultorio)
        {
            return await listaServicio.getServiciosActivos(id_consultorio);
        }
        public async Task<DataTable> getAtencionDePaciente(string id_consultorio, string id_usuario)
        {
            await consultar_atencion_de_paciente(id_consultorio, id_usuario);
            return atencionDePaciente;
        }
        #endregion

        #region metodos busca
        public async Task<DataTable> BuscarServicioActivo(string servicio, string id_consultorio)
        {

            // Preparo el término a buscar (ignoro nulos, espacios, mayúsculas y acentos)
            string term = (servicio ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable serviciosBD = await listaServicio.getServicios(id_consultorio);
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

            DataTable atencionBD = await getAtencionDePaciente(id_consultorio, id_usuario);
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
