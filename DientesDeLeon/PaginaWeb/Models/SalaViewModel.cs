using System.ComponentModel.DataAnnotations;

namespace PaginaWeb.Models
{
    public class SalaViewModel
    {

        public string? id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre de sala")]
        public string Sala { get; set; }

    }
}
