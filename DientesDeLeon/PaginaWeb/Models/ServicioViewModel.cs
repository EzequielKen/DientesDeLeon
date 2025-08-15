using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PaginaWeb.Models
{
    public class ServicioViewModel
    {
        
        public string? id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Servicio")]
        public string servicio { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Precio")]
        public string precio { get; set; }
    }
}
