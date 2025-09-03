using _02___sistemas._04___Atencion;
using PaginaWeb.Servicios._01___Servicios;
using System.Data;

namespace PaginaWeb.Servicios._04___Atencion
{
    public class AdministrarAtencionServicio
    {
        cls_AdministrarAtencion administrarAtencion = new cls_AdministrarAtencion();
        ListarServicioServicio listarServicio = new ListarServicioServicio();

        public async Task CargarAtencion(string id_sala, string id_servicio, string  id_consultorio)
        {
            DataTable atencionNueva = await administrarAtencion.getClone();
            atencionNueva.Rows.Add();
            int ultima_fila = atencionNueva.Rows.Count - 1;
            atencionNueva.Rows[ultima_fila]["id_Sala"] = id_sala;
            atencionNueva.Rows[ultima_fila]["id_Servicio"] = id_servicio;
            atencionNueva.Rows[ultima_fila]["id_Consultorio"] = id_consultorio;
            await administrarAtencion.cargar_atencion_de_sala(atencionNueva);
        }   

        public async Task EliminarAtencion(string id_servicio)
        {
            await administrarAtencion.eliminar_atencion_de_sala(id_servicio);
        }

        public async Task<DataTable> ObtenerAtencionDeSala(string id_sala, string id_consultorio)
        {
            DataTable atencionDeSala = await administrarAtencion.getAtencionDeSala(id_sala, id_consultorio);
            return atencionDeSala;
        }   
        public async Task<DataTable> ObtenerServiciosParaAtencion(string id_consultorio)
        {
            DataTable serviciosParaAtencion = await listarServicio.ObtenerTodosLosServiciosActivos(id_consultorio);
            return serviciosParaAtencion;
        }

        public async Task<DataTable> BuscarServicio(string buscar, string id_consultorio)
        {
            DataTable servicios = await listarServicio.BuscarServicioActivo(buscar, id_consultorio);
            return servicios;
        }
        public async Task<DataTable> BuscarAtencion(string buscar, string id_sala, string id_consultorio)
        {
            DataTable atencion = await administrarAtencion.BuscarAtencion(buscar, id_sala, id_consultorio);
            return atencion;
        }
    }
}
