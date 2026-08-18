using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class VeterinarioEditDTO
    {
        public int IdVeterinario { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(8, MinimumLength = 8)]
        public string Dni { get; set; } = null!;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = null!;

        [StringLength(20)]
        public string? Telefono { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El CMVP es obligatorio")]
        [StringLength(30)]
        public string Cmpv { get; set; } = null!;

        [Required(ErrorMessage = "Selecciona una especialidad")]
        public int IdEspecialidad { get; set; }

        public bool Estado { get; set; }
    }
}