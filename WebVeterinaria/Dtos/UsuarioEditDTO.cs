using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class UsuarioEditDTO
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100)]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(50)]
        public string Usuario { get; set; } = null!;

        // Opcional: si se llena, se re-hashea la clave. Si se deja vacío, se conserva la actual.
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string? NuevaClave { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "Selecciona un rol")]
        public int IdRol { get; set; }

        public bool Estado { get; set; }
    }
}