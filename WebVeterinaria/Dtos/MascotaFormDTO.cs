using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class MascotaFormDTO
    {
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(80)]
        public string Nombre { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly? FechaNacimiento { get; set; }

        [StringLength(1)]
        public string? Sexo { get; set; }

        [StringLength(50)]
        public string? Color { get; set; }

        [Range(0, 999.99, ErrorMessage = "Peso no válido")]
        public decimal? Peso { get; set; }

        public string? Alergias { get; set; }

        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "Selecciona un dueño")]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "Selecciona una especie")]
        public int IdEspecie { get; set; }

        [Required(ErrorMessage = "Selecciona una raza")]
        public int IdRaza { get; set; }

        public bool Estado { get; set; } = true;
    }
}