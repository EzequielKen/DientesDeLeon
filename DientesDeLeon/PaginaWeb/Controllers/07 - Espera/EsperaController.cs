using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaginaWeb.Controllers._06___Espera
{
    public class EsperaController : Controller
    {
        [Authorize(Roles = "admin,tecnico")]
        public ActionResult SalaDeEspera()
        {
            return View();
        }
    }
}
