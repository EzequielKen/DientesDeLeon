using _01___modulos.MySQL;
using _02___sistemas._01___Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._04___Atencion
{
    public class cls_AdministrarAtencion
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        cls_ListaServicio listaServicio = new cls_ListaServicio();
        cls_funciones funciones = new cls_funciones();
        DataTable AtencionDeSalaBD;
        DataTable servicioBD;

        #region carga a base de datos
        public async Task eliminar_atencion_de_sala(string id_atencion)
        {
            string actualizar = "`activo`='0'";
            await consultas.actualizar_tabla("atencion_de_sala", actualizar, id_atencion);
        }
        public async Task cargar_atencion_de_sala(DataTable atencionNueva)
        {
            //verificar si ya existe
            var resultado = await verificar_si_existe(atencionNueva);
            if (resultado.existe)
            {
                //si existe actualizar  
                DataTable atencionExistente = resultado.atencionExistente;
                string id = atencionExistente.Rows[0]["id"].ToString();
                string actualizar = "`activo`='1'";
                await consultas.actualizar_tabla("atencion_de_sala", actualizar, id);
            }
            else
            {
                //si no existe cargar
                var query = funciones.armar_query_insertar(atencionNueva);
                await consultas.insertar_en_tabla("atencion_de_sala", query.columnas, query.valores);
            }
        }

        public async Task<(bool existe, DataTable atencionExistente)> verificar_si_existe(DataTable atencionNueva)
        {
            bool existe = false;
            string id_sala = atencionNueva.Rows[0]["id_Sala"].ToString();
            string id_servicio = atencionNueva.Rows[0]["id_Servicio"].ToString();
            string id_consultorio = atencionNueva.Rows[0]["id_Consultorio"].ToString();
            DataTable atencionExistente = await consultas.consultar_atencion(id_sala, id_servicio, id_consultorio);
            if (atencionExistente.Rows.Count > 0)
            {
                existe = true;
            }
            return (existe, atencionExistente);
        }
        #endregion

        #region consultas
        private async Task<DataTable> ConsultarAtencionDeSala(string id_sala, string id_consultorio)
        {
            AtencionDeSalaBD = await consultas.consultar_atencion_de_sala(id_sala, id_consultorio);
            DataTable atencionSala = new DataTable();
            atencionSala.Columns.Add("activo", typeof(string));
            atencionSala.Columns.Add("id_Sala", typeof(string));
            atencionSala.Columns.Add("id_Consultorio", typeof(string));
            atencionSala.Columns.Add("id_Servicio", typeof(string));
            atencionSala.Columns.Add("ID", typeof(string));
            atencionSala.Columns.Add("Servicio", typeof(string));

            return atencionSala;
        }
        private async Task ConsultarServiciosActivosDeConsultorio(string id_consultorio)
        {
            servicioBD = await listaServicio.getServiciosActivos(id_consultorio);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> getAtencionDeSala(string id_sala, string id_consultorio)
        {
            await ConsultarServiciosActivosDeConsultorio(id_consultorio);
            DataTable atencionSala = await ConsultarAtencionDeSala(id_sala, id_consultorio);
            int filaServicio;
            string id_servicio;
            int ultima_fila;
            for (int fila = 0; fila <= AtencionDeSalaBD.Rows.Count - 1; fila++)
            {
                id_servicio = AtencionDeSalaBD.Rows[fila]["id_Servicio"].ToString();
                filaServicio = funciones.buscar_fila_por_dato(id_servicio, "id", servicioBD);

                atencionSala.Rows.Add();
                ultima_fila = atencionSala.Rows.Count - 1;
                atencionSala.Rows[ultima_fila]["activo"] = AtencionDeSalaBD.Rows[fila]["activo"].ToString();
                atencionSala.Rows[ultima_fila]["id_Sala"] = AtencionDeSalaBD.Rows[fila]["id_Sala"].ToString();
                atencionSala.Rows[ultima_fila]["id_Consultorio"] = AtencionDeSalaBD.Rows[fila]["id_Consultorio"].ToString();
                atencionSala.Rows[ultima_fila]["id_Servicio"] = id_servicio;
                atencionSala.Rows[ultima_fila]["ID"] = AtencionDeSalaBD.Rows[fila]["id"].ToString();
                atencionSala.Rows[ultima_fila]["Servicio"] = servicioBD.Rows[filaServicio]["Servicio"].ToString();

            }
            return atencionSala;
        }
        public async Task<DataTable> getAtencion_porServicio(string id_servicio, string id_consultorio)
        {
            DataTable atencionSala = await consultas.consultar_atencion_por_servicio(id_servicio, id_consultorio);


            return atencionSala;
        }
        public async Task<DataTable> getClone()
        {
            DataTable clone = await consultas.consultar_tabla("atencion_de_sala");
            return clone.Clone();
        }

        public async Task<DataTable> BuscarAtencion(string atencion, string idSala, string id_consultorio)
        {

            // Preparo el término a buscar (ignoro nulos, espacios, mayúsculas y acentos)
            string term = (atencion ?? string.Empty).Trim();
            string normTerm = RemoveDiacritics(term).ToUpperInvariant();

            DataTable atencionSala = await getAtencionDeSala(idSala, id_consultorio);
            DataTable listaAtencion = atencionSala.Clone();
            // Si no hay término, devuelvo todo tal cual
            bool filtrar = !string.IsNullOrEmpty(normTerm);

            for (int fila = 0; fila < atencionSala.Rows.Count; fila++)
            {
                string servicioNombre = atencionSala.Rows[fila]["Servicio"]?.ToString() ?? string.Empty;

                // Normalizo el valor de la BD
                string normSalaNombre = RemoveDiacritics(servicioNombre).ToUpperInvariant();

                // ¿Coincide?
                bool coincide = !filtrar || normSalaNombre.Contains(normTerm);

                if (!coincide) continue;

                listaAtencion.Rows.Add();
                int ultima_fila = listaAtencion.Rows.Count - 1;
                listaAtencion.Rows[ultima_fila]["activo"] = atencionSala.Rows[fila]["activo"].ToString();
                listaAtencion.Rows[ultima_fila]["id_Sala"] = atencionSala.Rows[fila]["id_Sala"].ToString();
                listaAtencion.Rows[ultima_fila]["id_Consultorio"] = atencionSala.Rows[fila]["id_Consultorio"].ToString();
                listaAtencion.Rows[ultima_fila]["id_Servicio"] = atencionSala.Rows[fila]["id_Servicio"].ToString();
                listaAtencion.Rows[ultima_fila]["ID"] = atencionSala.Rows[fila]["id"].ToString();
                listaAtencion.Rows[ultima_fila]["Servicio"] = atencionSala.Rows[fila]["Servicio"].ToString();

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
