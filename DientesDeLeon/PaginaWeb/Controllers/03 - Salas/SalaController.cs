using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using PaginaWeb.Servicios._01___Servicios;
using PaginaWeb.Servicios._03___Sala;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._03___Salas
{
    public class SalaController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult CrearSala(SalaViewModel SalaNueva)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CrearSalaNueva(SalaViewModel SalaNueva)
        {

            if (!ModelState.IsValid)
            {
                // Aquí se guardaría el servicio en la base de datos
                // Por ejemplo: _context.Servicios.Add(ServicioNuevo);
                // _context.SaveChanges();
                // Redirigir a la lista de servicios o a una vista de éxito
                return RedirectToAction("CrearSala", SalaNueva);
            }
            CrearSalaServicio crearSala = new CrearSalaServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            var resultado = await crearSala.CrearSala(SalaNueva, id_Consultorio);
            if (!resultado.resultado)
            {
                return RedirectToAction("CrearSala", SalaNueva);
            }
            return RedirectToAction("ListaSala");
        }

        [Authorize(Roles = "admin")]
        public ActionResult ListaSala()
        {
            return View();
        }

    }
}
