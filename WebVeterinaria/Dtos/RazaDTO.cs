namespace WebVeterinaria.DTOs
{
    public class RazaDTO
    {
        public int IdRaza { get; set; }
        public string Nombre { get; set; } = null!;
        public int IdEspecie { get; set; }
        public string EspecieNombre { get; set; } = null!;
    }
}