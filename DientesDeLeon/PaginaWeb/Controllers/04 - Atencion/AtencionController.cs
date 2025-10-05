using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using PaginaWeb.Servicios._04___Atencion;
using System.Data;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._04___Atencion
{
    public class AtencionController : Controller
    {
        // GET: AtencionController
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> AdministrarAtencion(string idSala)
        {
            ViewBag.IdSala = idSala;
            AdministrarAtencionServicio administrarAtencion = new AdministrarAtencionServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string NombreDeSala = await administrarAtencion.getNombreSala(idSala);
            DataTable servicios = await administrarAtencion.ObtenerServiciosParaAtencion(id_Consultorio);
            DataTable atencionDeSala = await administrarAtencion.ObtenerAtencionDeSala(idSala, id_Consultorio);
            return View(Tuple.Create(servicios, atencionDeSala, NombreDeSala));
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> BuscarServicio(string buscar, string idSala)
        {
            ViewBag.IdSala = idSala;

            AdministrarAtencionServicio administrarAtencion = new AdministrarAtencionServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            DataTable servicios = await administrarAtencion.BuscarServicio(buscar, id_Consultorio);
            DataTable atencionDeSala = await administrarAtencion.ObtenerAtencionDeSala(idSala, id_Consultorio);

            return View("AdministrarAtencion", Tuple.Create(servicios, atencionDeSala));
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CargarAtencion(string idServicio, string idSala)
        {
            AdministrarAtencionServicio administrarAtencion = new AdministrarAtencionServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            await administrarAtencion.CargarAtencion(idSala, idServicio, id_Consultorio);
            return RedirectToAction("AdministrarAtencion", "Atencion", new { idSala = idSala });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> EliminarAtencion(string idServicio,string idSala)
        {
            AdministrarAtencionServicio administrarAtencion = new AdministrarAtencionServicio();
            await administrarAtencion.EliminarAtencion(idServicio);
            return RedirectToAction("AdministrarAtencion", "Atencion", new { idSala = idSala });
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> BuscarAtencionSeleccionada(string BuscarAtencion, string idSala)
        {
            ViewBag.IdSala = idSala;

            AdministrarAtencionServicio administrarAtencion = new AdministrarAtencionServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            DataTable servicios = await administrarAtencion.ObtenerServiciosParaAtencion(id_Consultorio);
            DataTable atencionDeSala = await administrarAtencion.BuscarAtencion(BuscarAtencion, idSala,id_Consultorio);

            return View("AdministrarAtencion", Tuple.Create(servicios, atencionDeSala));
        }
    }
}
