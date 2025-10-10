using _00___acceso_a_base_de_datos;
using _02___sistemas._00___Perfil;
using Microsoft.AspNetCore.Identity;
using PaginaWeb.Models;
using Microsoft.Extensions.Options;
using System.IO;
using PaginaWeb.Options;

namespace PaginaWeb.Servicios._00___Perfil
{
    public class PerfilServicio
    {
        cls_Perfil perfil = new cls_Perfil();

        private readonly StorageOptions _storage;
        private readonly ILogger<PerfilServicio> _logger;

        public PerfilServicio(
            IOptions<StorageOptions> storageOptions,
            ILogger<PerfilServicio> logger)
        {
            _storage = storageOptions.Value;
            _logger = logger;
        }

        public async Task<PerfilViewModel> get_usuario(string id_usuario)
        {
            var usuarioDT = await perfil.get_usuario(id_usuario);
            if (usuarioDT.Rows.Count == 0)
            {
                return null; // O manejar el caso de usuario no encontrado según tus necesidades
            }
            var row = usuarioDT.Rows[0];
            var perfilViewModel = new PerfilViewModel
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                //Contraseña = row["Contraseña"].ToString(),
                //RepetirContraseña = row["Contraseña"].ToString(), // Asumiendo que quieres mostrar la misma contraseña
                //FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                Telefono = row["Telefono"].ToString(),
                Foto = row["Foto"].ToString()
            };
            return perfilViewModel;
        }

        public async Task actualizar_usuario(PerfilViewModel perfilViewModel)
        {
            var usuarioDT = await perfil.get_clone();
            var newRow = usuarioDT.NewRow();
            newRow["Nombre"] = perfilViewModel.Nombre;
            newRow["Apellido"] = perfilViewModel.Apellido;
            usuarioDT.Rows.Add(newRow);
            await perfil.actualizar_usuario(usuarioDT, perfilViewModel.Id.ToString());
        }

        public async Task actualizar_foto_usuario(PerfilViewModel perfilViewModel)
        {
            var file = perfilViewModel.FotoArchivo;
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("No se recibió ninguna imagen.");

            var cfg = _storage.PerfilFotos;

            // Validaciones
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(ext) || !cfg.AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("La imagen debe ser JPG, PNG, WEBP o GIF.");

            if (!file.ContentType.StartsWith("image/"))
                throw new InvalidOperationException("El archivo subido no parece ser una imagen.");

            if (file.Length > cfg.MaxBytes)
                throw new InvalidOperationException($"La imagen no puede superar los {cfg.MaxBytes / (1024 * 1024)} MB.");

            // --- Carpeta por usuario ---
            var userIdStr = perfilViewModel.Id.ToString(); // Invariant si querés: ToString(CultureInfo.InvariantCulture)
            var userDirPhysical = Path.Combine(cfg.PhysicalPath, userIdStr);
            Directory.CreateDirectory(userDirPhysical);

            // Nombre único y paths
            var fileName = $"{perfilViewModel.Id}_{Guid.NewGuid():N}{ext}";
            var destinoFisico = Path.Combine(userDirPhysical, fileName);

            // Guardado físico
            using (var stream = System.IO.File.Create(destinoFisico))
                await file.CopyToAsync(stream);

            // Ruta pública: /perfiles/{userId}/{fileName}
            var nuevaRutaPublica = $"{cfg.RequestPath.TrimEnd('/')}/{userIdStr}/{fileName}".Replace("\\", "/");

            // Borrar archivo anterior (maneja ambos casos: con o sin subcarpeta previa)
            //if (!string.IsNullOrWhiteSpace(perfilViewModel.Foto))
            //{
            //    try
            //    {
            //        // Caso esperado: /perfiles/{userId}/{nombre.ext}
            //        // Si la ruta anterior NO tenía subcarpeta, igual intentamos resolver el físico por nombre.
            //        var anteriorNombre = Path.GetFileName(perfilViewModel.Foto);
            //
            //        // Primero intentamos en la carpeta del usuario nueva convención
            //        var anteriorFisico = Path.Combine(userDirPhysical, anteriorNombre);
            //        if (!System.IO.File.Exists(anteriorFisico))
            //        {
            //            // Fallback por compatibilidad: carpeta raíz antigua
            //            var fallbackFisico = Path.Combine(cfg.PhysicalPath, anteriorNombre);
            //            if (System.IO.File.Exists(fallbackFisico))
            //                System.IO.File.Delete(fallbackFisico);
            //        }
            //        else
            //        {
            //            System.IO.File.Delete(anteriorFisico);
            //        }
            //    }
            //    catch (Exception delEx)
            //    {
            //        _logger.LogWarning(delEx, "No se pudo borrar la imagen anterior del usuario {UserId}", perfilViewModel.Id);
            //    }
            //}

            // Persistir en BD
            var usuarioDT = await perfil.get_clone();
            var newRow = usuarioDT.NewRow();
            newRow["Id"] = perfilViewModel.Id;
            newRow["Foto"] = nuevaRutaPublica;
            usuarioDT.Rows.Add(newRow);

            await perfil.actualizar_foto_usuario(usuarioDT, perfilViewModel.Id.ToString());

            // Actualizar VM para la vista
            perfilViewModel.Foto = nuevaRutaPublica;
        }

        public async Task actualizar_contraseña_usuario(string Contraseña, string Id)
        {
            await perfil.actualizar_contraseña_usuario(Contraseña, Id);
        }
    }
}
