using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaginaWeb.Controllers._00___Landing
{
    public class LandingController : Controller
    {
        // GET: LandingController
        [Authorize(Roles = "admin")]
        public ActionResult LandingAdmin()
        {
            return View();
        }
    }
}
