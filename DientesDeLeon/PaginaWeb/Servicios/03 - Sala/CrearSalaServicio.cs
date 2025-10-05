using _02___sistemas._01___Servicios;
using _02___sistemas._03___Sala;
using PaginaWeb.Models;
using System.Data;

namespace PaginaWeb.Servicios._03___Sala
{
    public class CrearSalaServicio
    {
        cls_CrearSala crearSala= new cls_CrearSala();
        public async Task<(string mensaje, bool resultado)> CrearSala(SalaViewModel SalaNueva, string id_consultorio)
        {
            DataTable servicio_tabla = await crearSala.getClone();

            servicio_tabla.Rows.Add();
            servicio_tabla.Rows[0]["id_Consultorio"] = id_consultorio;
            servicio_tabla.Rows[0]["Sala"] = SalaNueva.Sala;

            return await crearSala.crear_sala(servicio_tabla);
        }
    }
}
