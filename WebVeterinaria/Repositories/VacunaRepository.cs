using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class VacunaRepository : IVacunaRepository
    {
        private readonly AppDbContext _context;

        public VacunaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vacuna>> GetAllAsync()
        {
            return await _context.Vacunas
                .AsNoTracking()
                .OrderBy(v => v.Nombre)
                .ToListAsync();
        }

        public async Task<Vacuna?> GetByIdAsync(int id)
        {
            return await _context.Vacunas
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IdVacuna == id);
        }

        public async Task AddAsync(Vacuna vacuna)
        {
            _context.Vacunas.Add(vacuna);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vacuna vacuna)
        {
            _context.Vacunas.Update(vacuna);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vacuna = await _context.Vacunas.FindAsync(id);
            if (vacuna != null)
            {
                _context.Vacunas.Remove(vacuna);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Vacunas.AnyAsync(v => v.IdVacuna == id);
        }

        public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
        {
            return await _context.Vacunas
                .AnyAsync(v => v.Nombre == nombre && (excludeId == null || v.IdVacuna != excludeId));
        }

        public async Task<bool> TieneAplicacionesAsync(int id)
        {
            return await _context.MascotaVacunas.AnyAsync(mv => mv.IdVacuna == id);
        }

        public async Task<int> ContarAplicacionesAsync(int id)
        {
            return await _context.MascotaVacunas.CountAsync(mv => mv.IdVacuna == id);
        }
    }
}