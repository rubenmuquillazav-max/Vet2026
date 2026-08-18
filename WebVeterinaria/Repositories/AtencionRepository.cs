using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;

namespace WebVeterinaria.Repositories
{
    public class AtencionRepository : IAtencionRepository
    {
        private readonly AppDbContext _context;

        public AtencionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AtencionListDto>> GetAllAsync()
        {
            return await _context.Atencions
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdMascotaNavigation)
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdVeterinarioNavigation)
                .OrderByDescending(a => a.Fecha)
                .Select(a => new AtencionListDto
                {
                    IdAtencion = a.IdAtencion,
                    IdCita = a.IdCita ?? 0,
                    NombreMascota = a.IdCitaNavigation != null ? a.IdCitaNavigation.IdMascotaNavigation.Nombre : "",
                    NombreVeterinario = a.IdCitaNavigation != null
                        ? (a.IdCitaNavigation.IdVeterinarioNavigation.Nombres + " " + a.IdCitaNavigation.IdVeterinarioNavigation.Apellidos)
                        : "",
                    Fecha = a.Fecha,
                    Diagnostico = a.Diagnostico
                })
                .ToListAsync();
        }

        public async Task<Atencion?> GetByIdAsync(int id)
        {
            return await _context.Atencions
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdMascotaNavigation)
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdVeterinarioNavigation)
                .FirstOrDefaultAsync(a => a.IdAtencion == id);
        }

        public async Task<AtencionEditDto?> GetForEditAsync(int id)
        {
            return await _context.Atencions
                .Where(a => a.IdAtencion == id)
                .Select(a => new AtencionEditDto
                {
                    IdAtencion = a.IdAtencion,
                    IdCita = a.IdCita ?? 0,
                    Temperatura = a.Temperatura,
                    Peso = a.Peso,
                    Diagnostico = a.Diagnostico,
                    Tratamiento = a.Tratamiento,
                    Observaciones = a.Observaciones
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Atencion atencion)
        {
            atencion.Fecha = DateTime.Now;
            _context.Atencions.Add(atencion);

            if (atencion.IdCita.HasValue)
            {
                var cita = await _context.Cita.FindAsync(atencion.IdCita.Value);
                if (cita != null)
                {
                    cita.Estado = "ATENDIDA";
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Atencion atencion)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var atencion = await _context.Atencions.FindAsync(id);
            if (atencion != null)
            {
                _context.Atencions.Remove(atencion);
                await _context.SaveChangesAsync();
            }
        }

        // Citas que aún no tienen atención registrada (más la actual, si estamos editando)
        public async Task<List<CitaDisponibleDto>> GetCitasDisponiblesAsync(int? idCitaActual = null)
        {
            var citas = await _context.Cita
                .Include(c => c.IdMascotaNavigation)
                .Include(c => c.IdVeterinarioNavigation)
                .Where(c => (c.Estado == "PENDIENTE" || c.IdCita == idCitaActual)
                            && (c.Atencion == null || c.IdCita == idCitaActual))
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.Hora)
                .ToListAsync();

            return citas.Select(c => new CitaDisponibleDto
            {
                IdCita = c.IdCita,
                Texto = $"{c.Fecha:dd/MM/yyyy} {c.Hora:hh\\:mm} - {c.IdMascotaNavigation.Nombre} con Dr(a). {c.IdVeterinarioNavigation.Nombres} {c.IdVeterinarioNavigation.Apellidos}"
            }).ToList();
        }
    }
}