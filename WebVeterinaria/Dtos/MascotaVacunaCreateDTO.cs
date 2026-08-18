using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class MascotaVacunaCreateDTO
    {
        [Required(ErrorMessage = "Seleccione una mascota")]
        public int? IdMascota { get; set; }

        [Required(ErrorMessage = "Seleccione una vacuna")]
        public int? IdVacuna { get; set; }

        [Required(ErrorMessage = "La fecha de aplicación es obligatoria")]
        public DateOnly? Fecha { get; set; }

        public DateOnly? ProximaDosis { get; set; }

        public string? Observaciones { get; set; }
    }
}