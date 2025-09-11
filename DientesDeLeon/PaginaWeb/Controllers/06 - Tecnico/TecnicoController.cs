using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using PaginaWeb.Servicios._04___Atencion;
using PaginaWeb.Servicios._06___Tecnico;
using System.Data;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._06___Tecnico
{
    public class TecnicoController : Controller
    {
        [Authorize(Roles = "admin,tecnico")]
        [HttpGet]
        public async Task<ActionResult> ServiciosDeTecnico()
        {
            // guardás el idSala en ViewBag para que lo use la vista
            TecnicoServicio tecnicoServicio = new TecnicoServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_tecnico = User.FindFirst("UserId")?.Value;
            DataTable servicios = await tecnicoServicio.ObtenerServiciosParaAtencion(id_Consultorio);
            DataTable ServiciosSeleccionados = await tecnicoServicio.ObtenerServiciosDeTecnico(id_tecnico,id_Consultorio);
            return View(Tuple.Create(servicios, ServiciosSeleccionados));
        }

        [Authorize(Roles = "admin,tecnico")]
        [HttpGet]
        public async Task<ActionResult> BuscarServicio(string buscar)
        {
            TecnicoServicio tecnicoServicio = new TecnicoServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_tecnico = User.FindFirst("UserId")?.Value;
            DataTable servicios = await tecnicoServicio.BuscarServicioActivo(buscar, id_Consultorio);
            DataTable ServiciosSeleccionados = await tecnicoServicio.ObtenerServiciosDeTecnico(id_tecnico, id_Consultorio);

            return View("ServiciosDeTecnico", Tuple.Create(servicios, ServiciosSeleccionados));
        }

        [Authorize(Roles = "admin,tecnico")]
        [HttpGet]
        public async Task<ActionResult> BuscarAtencionDePaciente(string BuscarAtencion)
        {
            TecnicoServicio tecnicoServicio = new TecnicoServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_usuario = User.FindFirst("UserId")?.Value;
            DataTable servicios = await tecnicoServicio.ObtenerServiciosParaAtencion(id_Consultorio);
            DataTable ServiciosSeleccionados = await tecnicoServicio.BuscarAtencionDePaciente(BuscarAtencion, id_Consultorio, id_usuario);
            return View("ServiciosDeTecnico", Tuple.Create(servicios, ServiciosSeleccionados));
        }

        [Authorize(Roles = "admin,tecnico")]
        [HttpPost]
        public async Task<ActionResult> CargarServicio(string idServicio)
        {
            TecnicoServicio tecnicoServicio = new TecnicoServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_tecnico = User.FindFirst("UserId")?.Value;
            await tecnicoServicio.CargarServicio(id_tecnico, idServicio, id_Consultorio);
            return RedirectToAction("ServiciosDeTecnico", "Tecnico");
        }
        [Authorize(Roles = "admin,tecnico")]
        [HttpPost]
        public async Task<ActionResult> EliminarServicio(string idServicio)
        {
            TecnicoServicio tecnicoServicio = new TecnicoServicio();
            await tecnicoServicio.EliminarServicio(idServicio);
            return RedirectToAction("ServiciosDeTecnico", "Tecnico");
        }
    }
}
