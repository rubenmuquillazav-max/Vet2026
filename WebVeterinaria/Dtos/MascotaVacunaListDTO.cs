namespace WebVeterinaria.DTOs
{
    public class MascotaVacunaListDTO
    {
        public int Id { get; set; }

        public string NombreMascota { get; set; } = string.Empty;

        public string NombreDueno { get; set; } = string.Empty;

        public string NombreVacuna { get; set; } = string.Empty;

        public DateOnly? Fecha { get; set; }

        public DateOnly? ProximaDosis { get; set; }

        public string? Observaciones { get; set; }
    }
}