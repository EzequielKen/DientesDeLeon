using _02___sistemas;
using _02___sistemas._01___Servicios;
using _02___sistemas._06___Tecnico;
using System.Data;

namespace PaginaWeb.Servicios._06___Tecnico
{
    public class TecnicoServicio
    {
        cls_Tecnico tecnico = new cls_Tecnico();
        cls_ListaServicio listaServicio = new cls_ListaServicio();
        public async Task<DataTable> ObtenerServiciosParaAtencion(string id_Consultorio)
        {
            return await listaServicio.getServiciosActivos(id_Consultorio);
        }

        public async Task<DataTable> ObtenerServiciosDeTecnico(string id_tecnico, string id_consultorio)
        {
            return await tecnico.getServiciosDeTecnico(id_tecnico, id_consultorio);

        }

        public async Task<DataTable> BuscarServicioActivo(string servicio, string id_consultorio)
        {
            return await tecnico.BuscarServicioActivo(servicio, id_consultorio);
        }

        public async Task<DataTable> BuscarAtencionDePaciente(string Atencion, string id_consultorio, string id_usuario)
        {
            return await tecnico.BuscarAtencionDePaciente(Atencion, id_consultorio, id_usuario);
        }

        public async Task<bool> CargarServicio(string id_tecnico, string id_servicio, string id_consultorio)
        {
            DataTable atencion_de_tecnico = await tecnico.GetClone();
            atencion_de_tecnico.Rows.Add();
            atencion_de_tecnico.Rows[0]["id_tecnico"] = id_tecnico;
            atencion_de_tecnico.Rows[0]["id_servicio"] = id_servicio;
            atencion_de_tecnico.Rows[0]["id_consultorio"] = id_consultorio;

            return await tecnico.agregar_servicio_a_tecnico(atencion_de_tecnico);
        }

        public async Task<bool> EliminarServicio(string id)
        {
            return await tecnico.eliminar_atencion_de_tecnico(id);
        }
    }
}
