using System.ComponentModel.DataAnnotations;

namespace WebVeterinaria.DTOs
{
    public class AtencionListDto
    {
        public int IdAtencion { get; set; }

        public int IdCita { get; set; }

        public int? IdDueno { get; set; }

        public int? IdVeterinario { get; set; }

        public string NombreMascota { get; set; } = string.Empty;

        public string NombreDueno { get; set; } = string.Empty;

        public string NombreVeterinario { get; set; } = string.Empty;

        public DateTime? Fecha { get; set; }

        public string? Diagnostico { get; set; }
    }


    public class AtencionCreateDto
    {
        [Required(ErrorMessage = "Seleccione una cita")]
        public int IdCita { get; set; }

        public decimal? Temperatura { get; set; }

        public decimal? Peso { get; set; }

        [Required(ErrorMessage = "El diagnóstico es obligatorio")]
        public string? Diagnostico { get; set; }

        public string? Tratamiento { get; set; }

        public string? Observaciones { get; set; }
    }


    public class AtencionEditDto
    {
        public int IdAtencion { get; set; }

        [Required]
        public int IdCita { get; set; }

        public decimal? Temperatura { get; set; }

        public decimal? Peso { get; set; }

        [Required(ErrorMessage = "El diagnóstico es obligatorio")]
        public string? Diagnostico { get; set; }

        public string? Tratamiento { get; set; }

        public string? Observaciones { get; set; }
    }


    // Para el combo de selección de citas disponibles
    public class CitaDisponibleDto
    {
        public int IdCita { get; set; }

        public string Texto { get; set; } = string.Empty;
    }
}