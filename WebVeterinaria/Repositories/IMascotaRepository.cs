using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IMascotaRepository
    {
        Task<IEnumerable<Mascotum>> GetAllAsync(int? idDueno = null);
        Task<Mascotum?> GetByIdAsync(int id);
        Task AddAsync(Mascotum mascota);
        Task UpdateAsync(Mascotum mascota);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        Task<IEnumerable<Dueno>> GetDuenosAsync();
        Task<IEnumerable<Especie>> GetEspeciesAsync();
        Task<IEnumerable<Raza>> GetRazasAsync();
        Task<IEnumerable<Raza>> GetRazasPorEspecieAsync(int idEspecie);
    }
}