using _02___sistemas._00___Perfil;
using Microsoft.AspNetCore.Mvc;
using PaginaWeb.Models;
using PaginaWeb.Servicios._00___Perfil;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // IWebHostEnvironment

namespace PaginaWeb.Controllers._00___Perfil
{
    public class PerfilController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly PerfilServicio _ServicioPerfil;

        public PerfilController(IWebHostEnvironment env, PerfilServicio servicioPerfil)
        {
            _env = env;
            _ServicioPerfil = servicioPerfil;
        }
        public async Task<IActionResult> EditarPerfil()
        {
            string UserId = User.FindFirst("UserId")?.Value;

            PerfilViewModel usuario = await _ServicioPerfil.get_usuario(UserId);

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(PerfilViewModel perfil)
        {
            if (perfil.Nombre != string.Empty && perfil.Apellido != string.Empty)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarFotoPerfil(PerfilViewModel perfil)
        {
            try
            {
                await _ServicioPerfil.actualizar_foto_usuario(perfil);
                // El servicio actualiza perfil.Foto; devolvemos la misma vista para ver el cambio
                return RedirectToAction("EditarPerfil");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("EditarPerfil");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarContraseña(string Contraseña, string RepetirContraseña)
        {
            if (Contraseña != string.Empty && RepetirContraseña != string.Empty)
            {
                if (Contraseña == RepetirContraseña)
                {
                    // Aquí puedes agregar la lógica para actualizar la contraseña del usuario en la base de datos
                    // Por ejemplo, llamar a un servicio que maneje la actualización
                    // Después de actualizar, redirige a una página de confirmación o al perfil actualizado
                    //await _ServicioPerfil.actualizar_contraseña(Contraseña);
                    await _ServicioPerfil.actualizar_contraseña_usuario(Contraseña, User.FindFirst("UserId")?.Value);
                    return RedirectToAction("EditarPerfil");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                    return View("EditarPerfil");

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Los campos de contraseña no pueden estar vacíos.");
                return View("EditarPerfil");
            }
            // Simplemente redirige a la acción EditarPerfil para recargar los datos originales
            return RedirectToAction("EditarPerfil");
        }

    }
}
