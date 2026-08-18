namespace WebVeterinaria.DTOs
{
    public class DuenoDTO
    {
        public int IdDueno { get; set; }
        public string Dni { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public bool? Estado { get; set; }

        // Útil para mostrar en el Index sin cargar toda la colección de Mascota
        public int CantidadMascotas { get; set; }
    }
}