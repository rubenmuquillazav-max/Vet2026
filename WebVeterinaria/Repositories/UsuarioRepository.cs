using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .AsNoTracking()
                .OrderBy(u => u.Nombres)
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.IdUsuario == id);
        }

        public async Task<bool> UsuarioExistsAsync(string usuario, int? excludeId = null)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Usuario1 == usuario && (excludeId == null || u.IdUsuario != excludeId));
        }

        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            return await _context.Rols
                .AsNoTracking()
                .Where(r => r.Estado == true)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }
    }
}