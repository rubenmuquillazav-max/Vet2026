using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IVeterinarioRepository
    {
        Task<IEnumerable<Veterinario>> GetAllAsync();
        Task<Veterinario?> GetByIdAsync(int id);
        Task AddAsync(Veterinario veterinario);
        Task UpdateAsync(Veterinario veterinario);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> DniExistsAsync(string dni, int? excludeId = null);
        Task<bool> CmpvExistsAsync(string cmpv, int? excludeId = null);
        Task<IEnumerable<Especialidad>> GetEspecialidadesAsync();
        Task<IEnumerable<Veterinario>> BuscarAsync(string? busqueda);
    }
}