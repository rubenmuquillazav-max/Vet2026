using WebVeterinaria.DTOs;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public interface IMascotaVacunaRepository
    {
        Task<List<MascotaVacunaListDTO>> GetAllAsync(int? idDueno = null);

        Task<MascotaVacuna?> GetByIdAsync(int id);

        Task AddAsync(MascotaVacuna mascotaVacuna);

        Task UpdateAsync(MascotaVacuna mascotaVacuna);

        Task DeleteAsync(int id);

        Task<List<Mascotum>> GetMascotasAsync();

        Task<List<Mascotum>> GetMascotasPorDuenoAsync(int idDueno);

        Task<List<Vacuna>> GetVacunasAsync();
    }
}