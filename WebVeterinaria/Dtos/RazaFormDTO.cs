using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class RazaFormDTO
    {
        public int IdRaza { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Selecciona una especie")]
        public int IdEspecie { get; set; }
    }
}