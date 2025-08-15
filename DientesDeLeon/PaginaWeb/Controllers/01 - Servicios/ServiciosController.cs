using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using PaginaWeb.Servicios._01___Servicios;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._01___Servicios
{
    public class ServiciosController : Controller
    {
        // GET: ServiciosController
        [Authorize(Roles = "admin")]
        public ActionResult CrearServicio(ServicioViewModel ServicioNuevo)
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditarServicio(string idServicio)
        {
            EditarServicioServicio ServicioEditar = new EditarServicioServicio();
            ServicioViewModel ServicioAEditar = await ServicioEditar.ObtenerServicioPorId(idServicio);
            return View(ServicioAEditar);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> EditarServicioSeleccionado(ServicioViewModel ServicioEditar,string ServicioId)
        {
            if (!ModelState.IsValid)
            {
                // Aquí se guardaría el servicio editado en la base de datos
                // Por ejemplo: _context.Servicios.Update(ServicioEditar);
                // _context.SaveChanges();
                // Redirigir a la lista de servicios o a una vista de éxito
                return RedirectToAction("EditarServicio", ServicioEditar);
            }
            ServicioEditar.id = ServicioId;
            EditarServicioServicio EditorServicio = new EditarServicioServicio();
            var resultado = await EditorServicio.EditarServicio(ServicioEditar);
            if (!resultado.resultado)
            {
                return RedirectToAction("EditarServicio", ServicioEditar);
            }
            return RedirectToAction("ListaServicio");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CrearServicioNuevo(ServicioViewModel ServicioNuevo)
        {
            if (!ModelState.IsValid)
            {
                // Aquí se guardaría el servicio en la base de datos
                // Por ejemplo: _context.Servicios.Add(ServicioNuevo);
                // _context.SaveChanges();
                // Redirigir a la lista de servicios o a una vista de éxito
                return RedirectToAction("CrearServicio", ServicioNuevo);
            }
            CrearServicioServicio crearServicio = new CrearServicioServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            var resultado = await crearServicio.CrearServicio(ServicioNuevo, id_Consultorio);
            if (!resultado.resultado)
            {
                return RedirectToAction("CrearServicio", ServicioNuevo);
            }
            return RedirectToAction("CrearServicio", ServicioNuevo);

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> ListaServicio()
        {
            ListarServicioServicio listarServicio = new ListarServicioServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            var servicios = await listarServicio.ObtenerTodosLosServicios(id_Consultorio);
            return View(servicios);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeshabilitarServicio(string idServicio)
        {
            ListarServicioServicio listarServicio = new ListarServicioServicio();
            var resultado = await listarServicio.CambiarEstadoDeServicio(idServicio);
            return RedirectToAction("ListaServicio");
        }
    }
}
