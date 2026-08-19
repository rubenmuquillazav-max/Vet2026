using WebVeterinaria.DTOs;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IAtencionRepository
    {
        Task<List<AtencionListDto>> GetAllAsync(
            int? idDueno = null,
            int? idVeterinario = null);

        Task<Atencion?> GetByIdAsync(int id);

        Task<AtencionEditDto?> GetForEditAsync(int id);

        Task AddAsync(Atencion atencion);

        Task UpdateAsync(Atencion atencion);

        Task DeleteAsync(int id);

        Task<List<CitaDisponibleDto>> GetCitasDisponiblesAsync(
            int? idCitaActual = null);

        Task<List<Dueno>> GetDuenosAsync();

        Task<List<Veterinario>> GetVeterinariosAsync();
    }
}