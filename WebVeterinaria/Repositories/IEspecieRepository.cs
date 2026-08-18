using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IEspecieRepository
    {
        Task<IEnumerable<Especie>> GetAllAsync();
        Task<Especie?> GetByIdAsync(int id);
        Task AddAsync(Especie especie);
        Task UpdateAsync(Especie especie);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
        Task<bool> TieneRazasAsync(int id);
    }
}