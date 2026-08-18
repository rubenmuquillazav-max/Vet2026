using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class MascotaRepository : IMascotaRepository
    {
        private readonly AppDbContext _context;

        public MascotaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mascotum>> GetAllAsync(int? idDueno = null)
        {
            var query = _context.Mascota
                .Include(m => m.IdDuenoNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .AsNoTracking()
                .AsQueryable();

            // Filtro por dueño
            if (idDueno.HasValue)
            {
                query = query.Where(m => m.IdDueno == idDueno.Value);
            }

            return await query
                .OrderBy(m => m.Nombre)
                .ToListAsync();
        }

        public async Task<Mascotum?> GetByIdAsync(int id)
        {
            return await _context.Mascota
                .Include(m => m.IdDuenoNavigation)
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdMascota == id);
        }

        public async Task AddAsync(Mascotum mascota)
        {
            _context.Mascota.Add(mascota);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Mascotum mascota)
        {
            _context.Mascota.Update(mascota);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascota.Remove(mascota);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Mascota.AnyAsync(m => m.IdMascota == id);
        }

        public async Task<IEnumerable<Dueno>> GetDuenosAsync()
        {
            return await _context.Duenos
                .AsNoTracking()
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Nombres)
                .ToListAsync();
        }

        public async Task<IEnumerable<Especie>> GetEspeciesAsync()
        {
            return await _context.Especies
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Raza>> GetRazasAsync()
        {
            return await _context.Razas
                .AsNoTracking()
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Raza>> GetRazasPorEspecieAsync(int idEspecie)
        {
            return await _context.Razas
                .AsNoTracking()
                .Where(r => r.IdEspecie == idEspecie)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }
    }
}