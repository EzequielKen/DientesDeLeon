using _02___sistemas._01___Servicios;
using PaginaWeb.Models;
using System.Data;

namespace PaginaWeb.Servicios._01___Servicios
{
    public class EditarServicioServicio
    {
        cls_EditarServicio editarServicio = new cls_EditarServicio();
        public async Task<(bool resultado, string mensaje)> EditarServicio(ServicioViewModel ServicioEditar)
        {
            DataTable servicio = await editarServicio.ObtenerServicioPorId(ServicioEditar.id);
            servicio.Rows[0]["id"] = ServicioEditar.id;
            servicio.Rows[0]["servicio"] = ServicioEditar.servicio;
            servicio.Rows[0]["precio"] = ServicioEditar.precio;

            return await editarServicio.EditarServicio(servicio);
        }

        public async Task<ServicioViewModel> ObtenerServicioPorId(string idServicio)
        {
            DataTable servicioBD = await editarServicio.ObtenerServicioPorId(idServicio);
            ServicioViewModel servicioEditar = new ServicioViewModel();

            servicioEditar.id = servicioBD.Rows[0]["id"].ToString();
            servicioEditar.servicio = servicioBD.Rows[0]["servicio"].ToString();
            servicioEditar.precio = servicioBD.Rows[0]["precio"].ToString();

            return servicioEditar;
        }
    }
}
