using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class VacunaEditDTO
    {
        public int IdVacuna { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = "";

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}