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


        // =========================================================
        // LISTAR ATENCIONES + FILTRO POR DUEÑO Y VETERINARIO
        // =========================================================
        public async Task<List<AtencionListDto>> GetAllAsync(
    int? idDueno = null,
    int? idVeterinario = null)
        {
            var consulta = _context.Atencions
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdMascotaNavigation)
                        .ThenInclude(m => m.IdDuenoNavigation)
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdVeterinarioNavigation)
                .AsQueryable();

            // FILTRO POR DUEÑO
            if (idDueno.HasValue)
            {
                consulta = consulta.Where(a =>
                    a.IdCitaNavigation != null &&
                    a.IdCitaNavigation.IdMascotaNavigation != null &&
                    a.IdCitaNavigation.IdMascotaNavigation.IdDueno == idDueno.Value);
            }

            // FILTRO POR VETERINARIO
            if (idVeterinario.HasValue)
            {
                consulta = consulta.Where(a =>
                    a.IdCitaNavigation != null &&
                    a.IdCitaNavigation.IdVeterinario == idVeterinario.Value);
            }

            return await consulta
                .OrderByDescending(a => a.Fecha)
                .Select(a => new AtencionListDto
                {
                    IdAtencion = a.IdAtencion,

                    IdCita = a.IdCita ?? 0,

                    IdDueno = a.IdCitaNavigation != null
                        ? a.IdCitaNavigation.IdMascotaNavigation.IdDueno
                        : null,

                    IdVeterinario = a.IdCitaNavigation != null
                        ? a.IdCitaNavigation.IdVeterinario
                        : null,

                    NombreMascota = a.IdCitaNavigation != null
                        ? a.IdCitaNavigation.IdMascotaNavigation.Nombre
                        : "",

                    NombreDueno = a.IdCitaNavigation != null
                        ? a.IdCitaNavigation.IdMascotaNavigation
                            .IdDuenoNavigation.Nombres
                            + " " +
                          a.IdCitaNavigation.IdMascotaNavigation
                            .IdDuenoNavigation.Apellidos
                        : "",

                    NombreVeterinario = a.IdCitaNavigation != null
                        ? a.IdCitaNavigation.IdVeterinarioNavigation.Nombres
                            + " " +
                          a.IdCitaNavigation.IdVeterinarioNavigation.Apellidos
                        : "",

                    Fecha = a.Fecha,

                    Diagnostico = a.Diagnostico
                })
                .ToListAsync();
        }

        // =========================================================
        // OBTENER ATENCION POR ID
        // =========================================================
        public async Task<Atencion?> GetByIdAsync(int id)
        {
            return await _context.Atencions
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdMascotaNavigation!)
                        .ThenInclude(m => m.IdDuenoNavigation)
                .Include(a => a.IdCitaNavigation!)
                    .ThenInclude(c => c.IdVeterinarioNavigation)
                .FirstOrDefaultAsync(a => a.IdAtencion == id);
        }


        // =========================================================
        // OBTENER DTO PARA EDITAR
        // =========================================================
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


        // =========================================================
        // REGISTRAR ATENCION
        // =========================================================
        public async Task AddAsync(Atencion atencion)
        {
            atencion.Fecha = DateTime.Now;

            _context.Atencions.Add(atencion);


            // Cambiar estado de la cita a ATENDIDA
            if (atencion.IdCita.HasValue)
            {
                var cita = await _context.Cita
                    .FindAsync(atencion.IdCita.Value);

                if (cita != null)
                {
                    cita.Estado = "ATENDIDA";
                }
            }

            await _context.SaveChangesAsync();
        }


        // =========================================================
        // ACTUALIZAR ATENCION
        // =========================================================
        public async Task UpdateAsync(Atencion atencion)
        {
            await _context.SaveChangesAsync();
        }


        // =========================================================
        // ELIMINAR ATENCION
        // =========================================================
        public async Task DeleteAsync(int id)
        {
            var atencion = await _context.Atencions
                .FindAsync(id);

            if (atencion != null)
            {
                _context.Atencions.Remove(atencion);

                await _context.SaveChangesAsync();
            }
        }


        // =========================================================
        // CITAS DISPONIBLES
        // =========================================================
        public async Task<List<CitaDisponibleDto>> GetCitasDisponiblesAsync(
            int? idCitaActual = null)
        {
            var citas = await _context.Cita
                .Include(c => c.IdMascotaNavigation)
                .Include(c => c.IdVeterinarioNavigation)
                .Where(c =>
                    (c.Estado == "PENDIENTE" ||
                     c.IdCita == idCitaActual)
                    &&
                    (c.Atencion == null ||
                     c.IdCita == idCitaActual))
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.Hora)
                .ToListAsync();


            return citas.Select(c => new CitaDisponibleDto
            {
                IdCita = c.IdCita,

                Texto =
                    $"{c.Fecha:dd/MM/yyyy} " +
                    $"{c.Hora:hh\\:mm} - " +
                    $"{c.IdMascotaNavigation.Nombre} " +
                    $"con Dr(a). " +
                    $"{c.IdVeterinarioNavigation.Nombres} " +
                    $"{c.IdVeterinarioNavigation.Apellidos}"

            }).ToList();
        }


        // =========================================================
        // OBTENER DUEÑOS ACTIVOS
        // =========================================================
        public async Task<List<Dueno>> GetDuenosAsync()
        {
            return await _context.Duenos
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .ToListAsync();
        }


        // =========================================================
        // OBTENER VETERINARIOS ACTIVOS
        // =========================================================
        public async Task<List<Veterinario>> GetVeterinariosAsync()
        {
            return await _context.Veterinarios
                .Where(v => v.Estado == true)
                .OrderBy(v => v.Apellidos)
                .ThenBy(v => v.Nombres)
                .ToListAsync();
        }
    }
}