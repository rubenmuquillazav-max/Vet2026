using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class EspecieRepository : IEspecieRepository
    {
        private readonly AppDbContext _context;

        public EspecieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especie>> GetAllAsync()
        {
            return await _context.Especies
                .Include(e => e.Razas)
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<Especie?> GetByIdAsync(int id)
        {
            return await _context.Especies
                .Include(e => e.Razas)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdEspecie == id);
        }

        public async Task AddAsync(Especie especie)
        {
            _context.Especies.Add(especie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Especie especie)
        {
            _context.Especies.Update(especie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var especie = await _context.Especies.FindAsync(id);
            if (especie != null)
            {
                _context.Especies.Remove(especie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Especies.AnyAsync(e => e.IdEspecie == id);
        }

        public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
        {
            return await _context.Especies
                .AnyAsync(e => e.Nombre == nombre && (excludeId == null || e.IdEspecie != excludeId));
        }

        public async Task<bool> TieneRazasAsync(int id)
        {
            return await _context.Razas.AnyAsync(r => r.IdEspecie == id);
        }
    }
}