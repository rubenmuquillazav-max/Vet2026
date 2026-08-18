using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;
using WebVeterinaria.Dtos;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;
using WebVeterinaria.Filters;

namespace WebVeterinaria.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaRepository _citaRepository;
        private readonly AppDbContext _context;

        public CitaController(
            ICitaRepository citaRepository,
            AppDbContext context)
        {
            _citaRepository = citaRepository;
            _context = context;
        }

        // GET: Cita
        public async Task<IActionResult> Index(
        int? idDueno,
        DateOnly? fecha,
        string? estado)
        {
            var citas = await _citaRepository.GetAllAsync(
                idDueno,
                fecha,
                estado);

            await CargarDuenosFiltroAsync(idDueno);

            ViewBag.FechaFiltro = fecha?.ToString("yyyy-MM-dd");
            ViewBag.EstadoFiltro = estado;

            return View(citas);
        }

        private async Task CargarDuenosFiltroAsync(int? idDueno = null)
        {
            var duenos = await _context.Duenos
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto =
                        d.Apellidos + " " + d.Nombres
                })
                .ToListAsync();

            ViewBag.DuenosFiltro = new SelectList(
                duenos,
                "IdDueno",
                "NombreCompleto",
                idDueno);
        }

        // GET: Cita/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);

            if (cita == null)
                return NotFound();

            return View(cita);
        }

        // GET: Cita/Create
        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();

            var dto = new CitaCreateDto
            {
                Fecha = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(dto);
        }

        // POST: Cita/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CitaCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                bool conflicto = await _citaRepository
                    .ExisteConflictoHorarioAsync(
                        dto.IdVeterinario,
                        dto.Fecha,
                        dto.Hora);

                if (conflicto)
                {
                    ModelState.AddModelError(
                        "",
                        "El veterinario ya tiene una cita programada en esa fecha y hora.");

                    await CargarListasAsync(dto.IdDueno);

                    return View(dto);
                }

                var cita = new Citum
                {
                    IdMascota = dto.IdMascota,
                    IdVeterinario = dto.IdVeterinario,
                    Fecha = dto.Fecha,
                    Hora = dto.Hora,
                    Motivo = dto.Motivo,
                    Estado = dto.Estado
                };

                await _citaRepository.AddAsync(cita);

                TempData["Success"] = "Cita registrada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync(dto.IdDueno);

            return View(dto);
        }

        // GET: Cita/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);

            if (cita == null)
                return NotFound();

            // No permitir editar citas cerradas
            if (cita.Estado == "CANCELADA" ||
                cita.Estado == "ATENDIDA")
            {
                TempData["Error"] =
                    "La cita no puede modificarse porque está CANCELADA o ATENDIDA.";

                return RedirectToAction(nameof(Index));
            }

            var dto = await _citaRepository.GetForEditAsync(id);

            if (dto == null)
                return NotFound();

            var mascota = await _context.Mascota
                .Include(m => m.IdDuenoNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == dto.IdMascota);

            if (mascota != null && mascota.IdDuenoNavigation != null)
            {
                ViewBag.NombreDueno =
                    mascota.IdDuenoNavigation.Nombres + " " +
                    mascota.IdDuenoNavigation.Apellidos;
            }
            else
            {
                ViewBag.NombreDueno = "Sin dueño";
            }

            // Dueños
            await CargarDueñosAsync(dto.IdDueno);

            // Mascotas SOLO del dueño de la cita
            var mascotas = await _citaRepository
                .GetMascotasPorDuenoAsync(dto.IdDueno);

            ViewBag.Mascotas = new SelectList(
                mascotas,
                "IdMascota",
                "Nombre",
                dto.IdMascota);

            // Veterinarios
            var veterinarios = await _context.Veterinarios
                .Where(v => v.Estado == true)
                .OrderBy(v => v.Nombres)
                .ToListAsync();

            ViewBag.Veterinarios = new SelectList(
                veterinarios.Select(v => new
                {
                    v.IdVeterinario,
                    NombreCompleto =
                        v.Nombres + " " + v.Apellidos
                }),
                "IdVeterinario",
                "NombreCompleto",
                dto.IdVeterinario);

            // Estados
            ViewBag.Estados = new SelectList(
                new[]
                {
            "PENDIENTE",
            "CANCELADA",
            "ATENDIDA"
                },
                dto.Estado);

            return View(dto);
        }

        // POST: Cita/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CitaEditDto dto)
        {
            if (id != dto.IdCita)
                return NotFound();

            // Buscar la cita actual en la BD
            var citaActual = await _citaRepository.GetByIdAsync(id);

            if (citaActual == null)
                return NotFound();

            // Bloquear modificación si ya fue atendida o cancelada
            if (citaActual.Estado == "CANCELADA" ||
                citaActual.Estado == "ATENDIDA")
            {
                TempData["Error"] =
                    "La cita no puede modificarse porque está CANCELADA o ATENDIDA.";

                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                bool conflicto = await _citaRepository.ExisteConflictoHorarioAsync(
                    dto.IdVeterinario,
                    dto.Fecha,
                    dto.Hora,
                    dto.IdCita);

                if (conflicto)
                {
                    ModelState.AddModelError(
                        "",
                        "El veterinario ya tiene otra cita programada en esa fecha y hora.");

                    await CargarListasAsync();

                    return View(dto);
                }

                citaActual.IdMascota = dto.IdMascota;
                citaActual.IdVeterinario = dto.IdVeterinario;
                citaActual.Fecha = dto.Fecha;
                citaActual.Hora = dto.Hora;
                citaActual.Motivo = dto.Motivo;
                citaActual.Estado = dto.Estado;

                await _citaRepository.UpdateAsync(citaActual);

                TempData["Success"] =
                    "Cita actualizada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync();

            return View(dto);
        }

        // GET: Cita/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);

            if (cita == null)
                return NotFound();

            return View(cita);
        }

        // POST: Cita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _citaRepository.DeleteAsync(id);

            TempData["Success"] = "Cita eliminada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // Obtener mascotas de un dueño
        [HttpGet]
        public async Task<IActionResult> ObtenerMascotasPorDueno(int idDueno)
        {
            var mascotas = await _context.Mascota
                .Where(m => m.IdDueno == idDueno && m.Estado == true)
                .OrderBy(m => m.Nombre)
                .Select(m => new
                {
                    idMascota = m.IdMascota,
                    nombre = m.Nombre
                })
                .ToListAsync();

            return Json(mascotas);
        }

        // Cargar combos
        private async Task CargarListasAsync(int? idDueno = null)
        {
            // Dueños activos
            var duenos = await _context.Duenos
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto = d.Apellidos + " " + d.Nombres
                })
                .ToListAsync();

            ViewBag.Duenos = new SelectList(
                duenos,
                "IdDueno",
                "NombreCompleto",
                idDueno);

            // Mascotas del dueño seleccionado
            var mascotas = new List<Mascotum>();

            if (idDueno.HasValue)
            {
                mascotas = await _context.Mascota
                    .Where(m =>
                        m.IdDueno == idDueno.Value &&
                        m.Estado == true)
                    .OrderBy(m => m.Nombre)
                    .ToListAsync();
            }

            ViewBag.Mascotas = new SelectList(
                mascotas,
                "IdMascota",
                "Nombre");

            // Veterinarios activos
            var veterinarios = await _context.Veterinarios
                .Where(v => v.Estado == true)
                .OrderBy(v => v.Nombres)
                .ToListAsync();

            ViewBag.Veterinarios = new SelectList(
                veterinarios.Select(v => new
                {
                    v.IdVeterinario,
                    NombreCompleto = $"{v.Nombres} {v.Apellidos}"
                }),
                "IdVeterinario",
                "NombreCompleto");

            // Estados
            ViewBag.Estados = new SelectList(new[]
            {
                "PENDIENTE",
                "CANCELADA",
                "ATENDIDA"
            });
        }

        private async Task CargarDueñosAsync(int? seleccionado = null)
        {
            var duenos = await _context.Duenos
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .ToListAsync();

            ViewBag.Duenos = new SelectList(
                duenos.Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto = d.Nombres + " " + d.Apellidos
                }),
                "IdDueno",
                "NombreCompleto",
                seleccionado);
        }

    }
}