using _02___sistemas._00___Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace PaginaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string contrase�a)
        {
            cls_Login login = new cls_Login();
            // 1) Valid�s credenciales y obtienes tu DataTable:
            var logueo = await login.login(usuario, contrase�a);
            if (logueo.logeo)
            {
                DataTable dt = logueo.usuario; // Aqu� tienes tu DataTable con los datos del usuario
                // 2) Constru�s los Claims que quieras conservar:
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario),
                    // Por ejemplo, un claim con el ID o el rol desde tu DataTable:
                    new Claim("UserId", dt.Rows[0]["Id"].ToString()),
                    new Claim(ClaimTypes.Role, dt.Rows[0]["Rol"].ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,  // �Recordarme�
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                // 3) Firm�s la cookie:
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("LandingAdmin", "Landing");

            }
            else
            {
                ViewBag.Error = "Usuario o contrase�a incorrectos.";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
