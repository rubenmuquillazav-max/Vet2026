using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IVacunaRepository
    {
        Task<IEnumerable<Vacuna>> GetAllAsync();
        Task<Vacuna?> GetByIdAsync(int id);
        Task AddAsync(Vacuna vacuna);
        Task UpdateAsync(Vacuna vacuna);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
        Task<bool> TieneAplicacionesAsync(int id);
        Task<int> ContarAplicacionesAsync(int id);
    }
}