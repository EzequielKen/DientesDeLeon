using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Servicios._07___Espera;
using System.Data;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._06___Espera
{
    public class EsperaController : Controller
    {
        [Authorize(Roles = "admin,tecnico")]
        public async Task<ActionResult> SalaDeEspera()
        {
            SalaDeEsperaServicio Sala_De_Espera = new SalaDeEsperaServicio();
            string id_tecnico = User.FindFirst("UserId")?.Value;
            string id_Consultorio = User.FindFirst("id_Consultorio")?.Value;
            DataTable pacientesEnEspera = await Sala_De_Espera.get_PacientesEnEspera(id_tecnico,id_Consultorio);
            return View();
        }
    }
}
