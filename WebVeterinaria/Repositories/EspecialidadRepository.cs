using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class EspecialidadRepository : IEspecialidadRepository
    {
        private readonly AppDbContext _context;

        public EspecialidadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Especialidad>> GetAllAsync()
        {
            return await _context.Especialidads
                .AsNoTracking()
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<Especialidad?> GetByIdAsync(int id)
        {
            return await _context.Especialidads
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.IdEspecialidad == id);
        }

        public async Task AddAsync(Especialidad especialidad)
        {
            _context.Especialidads.Add(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Especialidad especialidad)
        {
            _context.Especialidads.Update(especialidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var especialidad = await _context.Especialidads.FindAsync(id);
            if (especialidad != null)
            {
                _context.Especialidads.Remove(especialidad);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Especialidads.AnyAsync(e => e.IdEspecialidad == id);
        }

        public async Task<bool> NombreExistsAsync(string nombre, int? excludeId = null)
        {
            return await _context.Especialidads
                .AnyAsync(e => e.Nombre == nombre && (excludeId == null || e.IdEspecialidad != excludeId));
        }

        public async Task<bool> TieneVeterinariosAsync(int id)
        {
            return await _context.Veterinarios.AnyAsync(v => v.IdEspecialidad == id);
        }

        public async Task<int> ContarVeterinariosAsync(int id)
        {
            return await _context.Veterinarios.CountAsync(v => v.IdEspecialidad == id);
        }
    }
}