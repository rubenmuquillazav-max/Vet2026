using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class EspecieFormDTO
    {
        public int IdEspecie { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;
    }
}