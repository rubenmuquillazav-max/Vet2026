using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class UsuarioCreateDTO
    {
        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Correo no válido")]
        [StringLength(100)]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "Selecciona un rol")]
        public int IdRol { get; set; }
    }
}