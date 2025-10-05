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
        [Authorize(Roles = "admin,recepcion")]
        public ActionResult CrearPaciente(PacienteViewModel PacienteNuevo)
        {
            cls_Twilio twilio = new cls_Twilio();
            PacienteNuevo.CodigosPais = twilio.ObtenerCodigosTelefonicosPorPais();
            PacienteNuevo.CodigoPais = "+54";

            return View("CrearPaciente",PacienteNuevo);
        }
        [Authorize(Roles = "admin,recepcion")]
        [HttpPost]
        public async Task<ActionResult> CrearPacienteNuevo(PacienteViewModel PacienteNuevo)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CrearPaciente", PacienteNuevo);
            }

            CrearPacienteServicio servicio = new CrearPacienteServicio();
            bool verificado = servicio.verificarNumeroE164(PacienteNuevo.CodigoPais, PacienteNuevo.CodigoArea, PacienteNuevo.Telefono);

            var resultado = await servicio.CrearPaciente(PacienteNuevo);
            if (!resultado.resultado)
            {
                ModelState.AddModelError("", resultado.mensaje);
                return RedirectToAction("CrearPaciente", PacienteNuevo);
            }
            return RedirectToAction("LandingAdmin", "Landing");
        }

        [Authorize(Roles = "admin,recepcion")]
        [HttpGet]
        public async Task<ActionResult> ListaPacientes()
        {
            DataTable pacientes = new DataTable();
            ListarPacienteServicio listarPacientesServicio = new ListarPacienteServicio();
            pacientes = await listarPacientesServicio.ObtenerTodosPacientes();
            return View(pacientes);
        }
        [Authorize(Roles = "admin,recepcion")]
        [HttpGet]
        public async Task<ActionResult> BuscarPacientes(string buscar)
        {
            ListarPacienteServicio listarPacientesServicio = new ListarPacienteServicio();
            DataTable pacientes = await listarPacientesServicio.ObtenerPaciente(buscar);
            return View("ListaPacientes",pacientes);
        }

    }
}
