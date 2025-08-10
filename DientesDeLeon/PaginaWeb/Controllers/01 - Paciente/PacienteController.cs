using _02___sistemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using PaginaWeb.Servicios._01___Paciente;
using System.Data;

namespace PaginaWeb.Controllers._01___Paciente
{
    public class PacienteController : Controller
    {
        // GET: PacienteController
        [Authorize(Roles = "admin")]
        public ActionResult CrearPaciente(PacienteViewModel PacienteNuevo)
        {
            cls_Twilio twilio = new cls_Twilio();
            PacienteNuevo.CodigosPais = twilio.ObtenerCodigosTelefonicosPorPais();
            // --- Asignamos el valor por defecto AR (+54) ---
            PacienteNuevo.CodigoPais = "+54";

            return View("CrearPaciente",PacienteNuevo);
        }

        [HttpPost]
        public async Task<ActionResult> CrearPacienteNuevo(PacienteViewModel PacienteNuevo)
        {
            // 1. Validación básica
            if (!ModelState.IsValid)
            {
                // recarga listas antes de volver a la vista
                return RedirectToAction("CrearPaciente", PacienteNuevo);
            }

            // Aquí llamamos al servicio para crear el negocio
            CrearPacienteServicio servicio = new CrearPacienteServicio();
            // Verificamos el número de teléfono en formato E.164
            bool verificado = servicio.verificarNumeroE164(PacienteNuevo.CodigoPais, PacienteNuevo.CodigoArea, PacienteNuevo.Telefono);
            // servicio.Crear(negocioNuevo); // Lógica de creación del negocio
            // Redirigir a la lista de negocios o mostrar un mensaje de éxito

            var resultado = await servicio.CrearPaciente(PacienteNuevo);
            if (!resultado.resultado)
            {
                // Si hubo un error, redirigimos a la vista de creación con el mensaje de error
                ModelState.AddModelError("", resultado.mensaje);
                return RedirectToAction("CrearPaciente", PacienteNuevo);
            }
            return RedirectToAction("LandingAdmin", "Landing");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult> ListaPacientes()
        {
            DataTable pacientes = new DataTable();
            ListarPacienteServicio listarPacientesServicio = new ListarPacienteServicio();
            pacientes = await listarPacientesServicio.ObtenerTodosPacientes();
            return View(pacientes);
        }

        [HttpGet]
        public async Task<ActionResult> BuscarPacientes(string buscar)
        {
            ListarPacienteServicio listarPacientesServicio = new ListarPacienteServicio();
            DataTable pacientes = await listarPacientesServicio.ObtenerPaciente(buscar);
            return View("ListaPacientes",pacientes);
        }

    }
}
