using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class DuenoCreateDTO
    {
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string Dni { get; set; } = null!;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = null!;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Correo no válido")]
        [StringLength(120)]
        public string? Correo { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }
    }
}