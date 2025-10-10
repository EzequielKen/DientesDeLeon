namespace PaginaWeb.Options
{
    public class StorageOptions
    {
        public PerfilFotosOptions PerfilFotos { get; set; } = new();
    }

    public class PerfilFotosOptions
    {
        public string PhysicalPath { get; set; } = string.Empty;  // D:\Datos\Perfiles
        public string RequestPath { get; set; } = string.Empty;  // /perfiles
        public long MaxBytes { get; set; } = 5 * 1024 * 1024;     // 5MB
        public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    }
}
