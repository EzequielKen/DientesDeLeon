using _02___sistemas._01___Servicios;
using PaginaWeb.Models;
using System.Data;

namespace PaginaWeb.Servicios._01___Servicios
{
    public class CrearServicioServicio
    {
        
        cls_CrearServicio crearServicio = new cls_CrearServicio();
        public async Task<(string mensaje, bool resultado)> CrearServicio(ServicioViewModel servicioNuevo,string id_consultorio)
        {
            DataTable servicio_tabla = await crearServicio.getClone();

            servicio_tabla.Rows.Add();
            servicio_tabla.Rows[0]["id_Consultorio"] = id_consultorio;
            servicio_tabla.Rows[0]["Servicio"] = servicioNuevo.servicio;
            servicio_tabla.Rows[0]["Precio"] = servicioNuevo.precio;

            return await crearServicio.crear_servicio(servicio_tabla, id_consultorio);
        }
    }
}
