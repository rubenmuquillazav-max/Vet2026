using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;


namespace WebVeterinaria.Repositories
{
    public class RazaRepository : IRazaRepository
    {
        private readonly AppDbContext _context;

        public RazaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Raza>> GetAllAsync()
        {
            return await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .AsNoTracking()
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }

        public async Task<Raza?> GetByIdAsync(int id)
        {
            return await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRaza == id);
        }

        public async Task AddAsync(Raza raza)
        {
            _context.Razas.Add(raza);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Raza raza)
        {
            _context.Razas.Update(raza);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var raza = await _context.Razas.FindAsync(id);
            if (raza != null)
            {
                _context.Razas.Remove(raza);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Razas.AnyAsync(r => r.IdRaza == id);
        }

        public async Task<bool> NombreExistsAsync(string nombre, int idEspecie, int? excludeId = null)
        {
            return await _context.Razas.AnyAsync(r =>
                r.Nombre == nombre &&
                r.IdEspecie == idEspecie &&
                (excludeId == null || r.IdRaza != excludeId));
        }

        public async Task<IEnumerable<Especie>> GetEspeciesAsync()
        {
            return await _context.Especies
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<List<Raza>> GetByEspecieAsync(int idEspecie)
        {
            return await _context.Razas
                .Include(r => r.IdEspecieNavigation)
                .Where(r => r.IdEspecie == idEspecie)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }
    }
}