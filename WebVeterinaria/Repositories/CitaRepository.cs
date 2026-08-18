using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Dtos;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class CitaRepository : ICitaRepository
    {
        private readonly AppDbContext _context;

        public CitaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CitaListDto>> GetAllAsync(
        int? idDueno = null,
        DateOnly? fecha = null,
        string? estado = null)
        {
            var query = _context.Cita
                .Include(c => c.IdMascotaNavigation)
                    .ThenInclude(m => m.IdDuenoNavigation)
                .Include(c => c.IdVeterinarioNavigation)
                .AsQueryable();

            // Filtro por dueño
            if (idDueno.HasValue)
            {
                query = query.Where(c =>
                    c.IdMascotaNavigation.IdDueno == idDueno.Value);
            }

            // Filtro por fecha
            if (fecha.HasValue)
            {
                query = query.Where(c =>
                    c.Fecha == fecha.Value);
            }

            // Filtro por estado
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(c =>
                    c.Estado == estado);
            }

            return await query
                .OrderByDescending(c => c.Fecha)
                .ThenBy(c => c.Hora)
                .Select(c => new CitaListDto
                {
                    IdCita = c.IdCita,

                    NombreDueno =
                        (c.IdMascotaNavigation.IdDuenoNavigation.Nombres ?? "")
                        + " " +
                        (c.IdMascotaNavigation.IdDuenoNavigation.Apellidos ?? ""),

                    NombreMascota =
                        c.IdMascotaNavigation.Nombre,

                    NombreVeterinario =
                        (c.IdVeterinarioNavigation.Nombres ?? "")
                        + " " +
                        (c.IdVeterinarioNavigation.Apellidos ?? ""),

                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Motivo = c.Motivo,
                    Estado = c.Estado
                })
                .ToListAsync();
        }

        public async Task<Citum?> GetByIdAsync(int id)
        {
            return await _context.Cita
                .Include(c => c.IdMascotaNavigation)
                .Include(c => c.IdVeterinarioNavigation)
                .Include(c => c.Atencion)
                .FirstOrDefaultAsync(c => c.IdCita == id);
        }

        public async Task<CitaEditDto?> GetForEditAsync(int id)
        {
            return await _context.Cita
                .Include(c => c.IdMascotaNavigation)
                .ThenInclude(m => m.IdDuenoNavigation)
                .Where(c => c.IdCita == id)
                .Select(c => new CitaEditDto
                {
                    IdCita = c.IdCita,
                    IdDueno = c.IdMascotaNavigation.IdDueno,
                    IdMascota = c.IdMascota,
                    IdVeterinario = c.IdVeterinario,
                    Fecha = c.Fecha,
                    Hora = c.Hora,
                    Motivo = c.Motivo,
                    Estado = c.Estado
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Citum cita)
        {
            cita.FechaRegistro = DateTime.Now;
            _context.Cita.Add(cita);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Citum cita)
        {
            _context.Cita.Update(cita);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita != null)
            {
                _context.Cita.Remove(cita);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteConflictoHorarioAsync(int idVeterinario, DateOnly fecha, TimeOnly hora, int? idCitaExcluir = null)
        {
            var query = _context.Cita.Where(c =>
                c.IdVeterinario == idVeterinario &&
                c.Fecha == fecha &&
                c.Hora == hora &&
                c.Estado != "Cancelada");

            if (idCitaExcluir.HasValue)
                query = query.Where(c => c.IdCita != idCitaExcluir.Value);

            return await query.AnyAsync();
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