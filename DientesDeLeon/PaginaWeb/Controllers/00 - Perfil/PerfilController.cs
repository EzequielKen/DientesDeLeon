using _02___sistemas._00___Perfil;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using PaginaWeb.Servicios._00___Perfil;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaginaWeb.Controllers._00___Perfil
{
    public class PerfilController : Controller
    {
        private PerfilServicio _ServicioPerfil;
        public PerfilController(PerfilServicio ServicioPerfil)
        {
            _ServicioPerfil = ServicioPerfil;
        }
        public async Task<IActionResult> EditarPerfil()
        {
            string UserId = User.FindFirst("UserId")?.Value;

            PerfilViewModel usuario = await _ServicioPerfil.get_usuario(UserId);

            return View(usuario);
        }

        public async Task<IActionResult> ActualizarPerfil(PerfilViewModel perfil)
        {
            if (perfil.Nombre != string.Empty && perfil.Apellido!=string.Empty)
            {
                // Aquí puedes agregar la lógica para actualizar el perfil del usuario en la base de datos
                // Por ejemplo, llamar a un servicio que maneje la actualización
                // Después de actualizar, redirige a una página de confirmación o al perfil actualizado
                await _ServicioPerfil.actualizar_usuario(perfil);
                return RedirectToAction("EditarPerfil");
            }
            // Si el modelo no es válido, vuelve a mostrar el formulario con los errores
            return View("EditarPerfil", perfil);
        }
    }
}
