using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.Dtos
{
    public class CitaListDto
    {
        public int IdCita { get; set; }
        public string NombreDueno { get; set; } = string.Empty;
        public string NombreMascota { get; set; } = string.Empty;
        public string NombreVeterinario { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string? Motivo { get; set; }
        public string? Estado { get; set; }
    }

    public class CitaCreateDto
    {
        [Required(ErrorMessage = "Seleccione un dueño")]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "Seleccione una mascota")]
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "Seleccione un veterinario")]
        public int IdVeterinario { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        public TimeOnly Hora { get; set; }

        public string? Motivo { get; set; }

        public string Estado { get; set; } = "Pendiente";
    }

    public class CitaEditDto
    {
        public int IdCita { get; set; }

        [Required]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "Seleccione una mascota")]
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "Seleccione un veterinario")]
        public int IdVeterinario { get; set; }

        [Required]
        public DateOnly Fecha { get; set; }

        [Required]
        public TimeOnly Hora { get; set; }

        public string? Motivo { get; set; }

        public string? Estado { get; set; }
    }
}