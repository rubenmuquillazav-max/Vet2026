using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> UsuarioExistsAsync(string usuario, int? excludeId = null);
        Task<IEnumerable<Rol>> GetRolesAsync();
    }
}