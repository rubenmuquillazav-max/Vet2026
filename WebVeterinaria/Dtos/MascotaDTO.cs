namespace WebVeterinaria.DTOs
{
    public class MascotaDTO
    {
        public int IdMascota { get; set; }
        public string Nombre { get; set; } = null!;
        public DateOnly? FechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string? Color { get; set; }
        public decimal? Peso { get; set; }
        public string? Alergias { get; set; }
        public string? Observaciones { get; set; }
        public bool Estado { get; set; }

        public int IdDueno { get; set; }
        public string DuenoNombreCompleto { get; set; } = null!;

        public int IdEspecie { get; set; }
        public string EspecieNombre { get; set; } = null!;

        public int IdRaza { get; set; }
        public string RazaNombre { get; set; } = null!;
    }
}