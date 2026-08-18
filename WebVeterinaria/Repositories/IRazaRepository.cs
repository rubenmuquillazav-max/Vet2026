using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IRazaRepository
    {
        Task<IEnumerable<Raza>> GetAllAsync();
        Task<Raza?> GetByIdAsync(int id);
        Task AddAsync(Raza raza);
        Task UpdateAsync(Raza raza);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NombreExistsAsync(string nombre, int idEspecie, int? excludeId = null);
        Task<IEnumerable<Especie>> GetEspeciesAsync();
        Task<List<Raza>> GetByEspecieAsync(int idEspecie);
    }
}