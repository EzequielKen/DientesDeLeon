using _02___sistemas._01___Servicios;
using _02___sistemas._03___Sala;
using PaginaWeb.Models;
using System.Data;

namespace PaginaWeb.Servicios._03___Sala
{
    public class EditarSalaServicio
    {
        cls_EditarSala editarSala = new cls_EditarSala();
        public async Task<(bool resultado, string mensaje)> EditarSala(SalaViewModel SalaEditar)
        {
            DataTable Sala = await editarSala.ObtenerSalaPorId(SalaEditar.id);
            Sala.Rows[0]["id"] = SalaEditar.id;
            Sala.Rows[0]["Sala"] = SalaEditar.Sala;

            return await editarSala.EditarSala(Sala);
        }

        public async Task<SalaViewModel> ObtenerSalaPorId(string idServicio)
        {
            DataTable salaBD = await editarSala.ObtenerSalaPorId(idServicio);
            SalaViewModel SalaEditar = new SalaViewModel();

            SalaEditar.id = salaBD.Rows[0]["id"].ToString();
            SalaEditar.Sala = salaBD.Rows[0]["Sala"].ToString();

            return SalaEditar;
        }
    }
}
