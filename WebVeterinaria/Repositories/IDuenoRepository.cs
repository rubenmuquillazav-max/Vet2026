using WebVeterinaria.DTOs;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IDuenoRepository
    {
        Task<IEnumerable<Dueno>> GetAllAsync();
        Task<Dueno?> GetByIdAsync(int id);
        Task AddAsync(Dueno dueno);
        Task UpdateAsync(Dueno dueno);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> DniExistsAsync(string dni, int? excludeId = null);

        Task<List<DuenoDTO>> BuscarAsync(string? busqueda);
    }
}