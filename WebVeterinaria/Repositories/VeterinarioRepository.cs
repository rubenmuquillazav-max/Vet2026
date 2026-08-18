using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class VeterinarioRepository : IVeterinarioRepository
    {
        private readonly AppDbContext _context;

        public VeterinarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Veterinario>> GetAllAsync()
        {
            return await _context.Veterinarios
                .Include(v => v.IdEspecialidadNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Veterinario?> GetByIdAsync(int id)
        {
            return await _context.Veterinarios
                .Include(v => v.IdEspecialidadNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.IdVeterinario == id);
        }

        public async Task AddAsync(Veterinario veterinario)
        {
            _context.Veterinarios.Add(veterinario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Veterinario veterinario)
        {
            _context.Veterinarios.Update(veterinario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var veterinario = await _context.Veterinarios.FindAsync(id);
            if (veterinario != null)
            {
                _context.Veterinarios.Remove(veterinario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Veterinarios.AnyAsync(v => v.IdVeterinario == id);
        }

        public async Task<bool> DniExistsAsync(string dni, int? excludeId = null)
        {
            return await _context.Veterinarios
                .AnyAsync(v => v.Dni == dni && (excludeId == null || v.IdVeterinario != excludeId));
        }

        public async Task<bool> CmpvExistsAsync(string cmpv, int? excludeId = null)
        {
            return await _context.Veterinarios
                .AnyAsync(v => v.Cmpv == cmpv && (excludeId == null || v.IdVeterinario != excludeId));
        }

        public async Task<IEnumerable<Especialidad>> GetEspecialidadesAsync()
        {
            return await _context.Especialidads
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Veterinario>> BuscarAsync(string? busqueda)
        {
            var query = _context.Veterinarios
                .Include(v => v.IdEspecialidadNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                busqueda = busqueda.Trim();

                query = query.Where(v =>
                    (v.Dni != null && v.Dni.Contains(busqueda)) ||
                    (v.Apellidos != null && v.Apellidos.Contains(busqueda)));
            }

            return await query
                .OrderBy(v => v.Apellidos)
                .ThenBy(v => v.Nombres)
                .ToListAsync();
        }
    }
}