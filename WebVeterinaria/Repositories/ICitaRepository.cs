using WebVeterinaria.Dtos;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface ICitaRepository
    {
        Task<List<CitaListDto>> GetAllAsync(
            int? idDueno = null,
            DateOnly? fecha = null,
            string? estado= null);
        Task<Citum?> GetByIdAsync(int id);
        Task<CitaEditDto?> GetForEditAsync(int id);
        Task AddAsync(Citum cita);
        Task UpdateAsync(Citum cita);
        Task DeleteAsync(int id);
        Task<bool> ExisteConflictoHorarioAsync(int idVeterinario, DateOnly fecha, TimeOnly hora, int? idCitaExcluir = null);
        Task<List<Mascotum>> GetMascotasPorDuenoAsync(int idDueno);
    }
}