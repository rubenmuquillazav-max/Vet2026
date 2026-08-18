namespace WebVeterinaria.DTOs
{
    public class VeterinarioDTO
    {
        public int IdVeterinario { get; set; }
        public string Dni { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Cmpv { get; set; }
        public bool Estado { get; set; }

        public int IdEspecialidad { get; set; }
        public string EspecialidadNombre { get; set; } = null!;
    }
}