using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IEspecialidadRepository
    {
        Task<IEnumerable<Especialidad>> GetAllAsync();
        Task<Especialidad?> GetByIdAsync(int id);
        Task AddAsync(Especialidad especialidad);
        Task UpdateAsync(Especialidad especialidad);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NombreExistsAsync(string nombre, int? excludeId = null);
        Task<bool> TieneVeterinariosAsync(int id);
        Task<int> ContarVeterinariosAsync(int id);
    }
}