namespace WebVeterinaria.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public string? Correo { get; set; }
        public bool Estado { get; set; }

        public int IdRol { get; set; }
        public string RolNombre { get; set; } = null!;
    }
}