using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Servicios._05___Recepcion;
using System.Data;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._03___Recepcion
{

    public class RecepcionController : Controller
    {
        // GET: RecepcionController
        [Authorize(Roles = "admin,recepcion")]
        public async Task<ActionResult> RecepcionarPaciente(string pacienteId)
        {
            RecepcionServicio recepcionServicio = new RecepcionServicio();
            ViewBag.PacienteId = pacienteId;
            HttpContext.Session.SetString("PacienteId", pacienteId);
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            DataTable paciente = await recepcionServicio.GetPacientePorId(pacienteId);
            DataTable servicios = await recepcionServicio.GetServicios(id_Consultorio);
            DataTable serviciosSeleccionados = await recepcionServicio.getAtencionDePaciente(id_Consultorio, pacienteId);
            return View(Tuple.Create(paciente, servicios, serviciosSeleccionados));
        }

        [Authorize(Roles = "admin,recepcion")]
        [HttpPost]
        public async Task<ActionResult> EnviarPacienteAEspera(string idServicio)
        {
            RecepcionServicio recepcionServicio = new RecepcionServicio();
            //id_consultorio id_usuario id_servicio id_sala
            string id_consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_usuario = HttpContext.Session.GetString("PacienteId");
            string id_servicio = idServicio;
            //string id_sala = idSala;
            await recepcionServicio.EnviarPacienteAEspera(id_consultorio, id_usuario, id_servicio);
            return RedirectToAction("RecepcionarPaciente", new { pacienteId = id_usuario });
        }
        [Authorize(Roles = "admin,recepcion")]
        [HttpPost]
        public async Task<ActionResult> EliminarAtencion(string idAtencion)
        {
            RecepcionServicio recepcionServicio = new RecepcionServicio();
            string id_usuario = HttpContext.Session.GetString("PacienteId");
            await recepcionServicio.EliminarAtencion(idAtencion);
            return RedirectToAction("RecepcionarPaciente", new { pacienteId = id_usuario });
        }
        public async Task<ActionResult> BuscarServicio(string buscar)
        {
            RecepcionServicio recepcionServicio = new RecepcionServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_usuario = HttpContext.Session.GetString("PacienteId");
            ViewBag.PacienteId = id_usuario;
            DataTable paciente = await recepcionServicio.GetPacientePorId(id_usuario);
            DataTable servicios = await recepcionServicio.BuscarServicio(buscar, id_Consultorio);
            DataTable serviciosSeleccionados = await recepcionServicio.getAtencionDePaciente(id_Consultorio, id_usuario);
            return View("RecepcionarPaciente", Tuple.Create(paciente, servicios, serviciosSeleccionados));
        }
        public async Task<ActionResult> BuscarAtencionDePaciente(string BuscarAtencion)
        {
            RecepcionServicio recepcionServicio = new RecepcionServicio();
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            string id_usuario = HttpContext.Session.GetString("PacienteId");
            ViewBag.PacienteId = id_usuario;
            DataTable paciente = await recepcionServicio.GetPacientePorId(id_usuario);
            DataTable servicios = await recepcionServicio.GetServicios(id_Consultorio);
            DataTable serviciosSeleccionados = await recepcionServicio.BuscarAtencionDePaciente(BuscarAtencion, id_Consultorio, id_usuario);
            return View("RecepcionarPaciente", Tuple.Create(paciente, servicios, serviciosSeleccionados));
        }
    }
}
