using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class EspecialidadCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = "";
    }
}