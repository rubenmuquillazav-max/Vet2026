namespace WebVeterinaria.DTOs
{
    public class VacunaDTO
    {
        public int IdVacuna { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public int CantidadAplicaciones { get; set; }
    }
}