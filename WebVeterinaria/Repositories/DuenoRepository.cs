using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class DuenoRepository : IDuenoRepository
    {
        private readonly AppDbContext _context;

        public DuenoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dueno>> GetAllAsync()
        {
            return await _context.Duenos
                .Include(d => d.Mascota)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Dueno?> GetByIdAsync(int id)
        {
            return await _context.Duenos
                .Include(d => d.Mascota)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDueno == id);
        }

        public async Task AddAsync(Dueno dueno)
        {
            _context.Duenos.Add(dueno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Dueno dueno)
        {
            _context.Duenos.Update(dueno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dueno = await _context.Duenos.FindAsync(id);
            if (dueno != null)
            {
                _context.Duenos.Remove(dueno);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Duenos.AnyAsync(d => d.IdDueno == id);
        }

        public async Task<bool> DniExistsAsync(string dni, int? excludeId = null)
        {
            return await _context.Duenos
                .AnyAsync(d => d.Dni == dni && (excludeId == null || d.IdDueno != excludeId));
        }

        public async Task<List<DuenoDTO>> BuscarAsync(string? busqueda)
        {
            var query = _context.Duenos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                busqueda = busqueda.Trim();

                query = query.Where(d =>
                    (d.Dni != null && d.Dni.Contains(busqueda)) ||
                    d.Apellidos.Contains(busqueda));
            }

            return await query
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .Select(d => new DuenoDTO
                {
                    IdDueno = d.IdDueno,
                    Dni = d.Dni,
                    Nombres = d.Nombres,
                    Apellidos = d.Apellidos,
                    Telefono = d.Telefono,
                    Correo = d.Correo,
                    Direccion = d.Direccion,
                    Estado = d.Estado
                })
                .ToListAsync();
        }
    }
}