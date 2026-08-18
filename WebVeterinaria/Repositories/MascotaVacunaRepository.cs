using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class MascotaVacunaRepository : IMascotaVacunaRepository
    {
        private readonly AppDbContext _context;

        public MascotaVacunaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MascotaVacunaListDTO>> GetAllAsync()
        {
            return await _context.MascotaVacunas
                .Include(mv => mv.IdMascotaNavigation)
                    .ThenInclude(m => m.IdDuenoNavigation)
                .Include(mv => mv.IdVacunaNavigation)
                .OrderByDescending(mv => mv.Fecha)
                .Select(mv => new MascotaVacunaListDTO
                {
                    Id = mv.Id,

                    NombreMascota =
                        mv.IdMascotaNavigation != null
                            ? mv.IdMascotaNavigation.Nombre
                            : "",

                    NombreDueno =
                        mv.IdMascotaNavigation != null &&
                        mv.IdMascotaNavigation.IdDuenoNavigation != null
                            ? mv.IdMascotaNavigation.IdDuenoNavigation.Nombres
                              + " "
                              + mv.IdMascotaNavigation.IdDuenoNavigation.Apellidos
                            : "",

                    NombreVacuna =
                        mv.IdVacunaNavigation != null
                            ? mv.IdVacunaNavigation.Nombre ?? ""
                            : "",

                    Fecha = mv.Fecha,

                    ProximaDosis = mv.ProximaDosis,

                    Observaciones = mv.Observaciones
                })
                .ToListAsync();
        }

        public async Task<MascotaVacuna?> GetByIdAsync(int id)
        {
            return await _context.MascotaVacunas
                .Include(mv => mv.IdMascotaNavigation)
                    .ThenInclude(m => m.IdDuenoNavigation)
                .Include(mv => mv.IdVacunaNavigation)
                .FirstOrDefaultAsync(mv => mv.Id == id);
        }

        public async Task AddAsync(MascotaVacuna mascotaVacuna)
        {
            _context.MascotaVacunas.Add(mascotaVacuna);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MascotaVacuna mascotaVacuna)
        {
            _context.MascotaVacunas.Update(mascotaVacuna);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var registro = await _context.MascotaVacunas
                .FindAsync(id);

            if (registro != null)
            {
                _context.MascotaVacunas.Remove(registro);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Mascotum>> GetMascotasAsync()
        {
            return await _context.Mascota
                .Include(m => m.IdDuenoNavigation)
                .Where(m => m.Estado == true)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        public async Task<List<Vacuna>> GetVacunasAsync()
        {
            return await _context.Vacunas
                .OrderBy(v => v.Nombre)
                .ToListAsync();
        }

        public async Task<List<Mascotum>> GetMascotasPorDuenoAsync(int idDueno)
        {
            return await _context.Mascota
                .Where(m => m.IdDueno == idDueno && m.Estado == true)
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }
    }
}