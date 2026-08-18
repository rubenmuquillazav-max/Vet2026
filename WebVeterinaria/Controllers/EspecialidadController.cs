using Microsoft.AspNetCore.Mvc;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly IEspecialidadRepository _especialidadRepository;

        public EspecialidadController(IEspecialidadRepository especialidadRepository)
        {
            _especialidadRepository = especialidadRepository;
        }

        // GET: Especialidad
        public async Task<IActionResult> Index()
        {
            var especialidades = await _especialidadRepository.GetAllAsync();

            var dto = new List<EspecialidadDTO>();
            foreach (var e in especialidades)
            {
                dto.Add(new EspecialidadDTO
                {
                    IdEspecialidad = e.IdEspecialidad,
                    Nombre = e.Nombre,
                    CantidadVeterinarios = await _especialidadRepository.ContarVeterinariosAsync(e.IdEspecialidad)
                });
            }

            return View(dto);
        }

        // GET: Especialidad/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var e = await _especialidadRepository.GetByIdAsync(id.Value);
            if (e == null) return NotFound();

            var dto = new EspecialidadDTO
            {
                IdEspecialidad = e.IdEspecialidad,
                Nombre = e.Nombre,
                CantidadVeterinarios = await _especialidadRepository.ContarVeterinariosAsync(e.IdEspecialidad)
            };

            return View(dto);
        }

        // GET: Especialidad/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especialidad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EspecialidadCreateDTO dto)
        {
            if (await _especialidadRepository.NombreExistsAsync(dto.Nombre))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe una especialidad con este nombre");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var especialidad = new Especialidad
            {
                Nombre = dto.Nombre
            };

            await _especialidadRepository.AddAsync(especialidad);
            return RedirectToAction(nameof(Index));
        }

        // GET: Especialidad/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var e = await _especialidadRepository.GetByIdAsync(id.Value);
            if (e == null) return NotFound();

            var dto = new EspecialidadEditDTO
            {
                IdEspecialidad = e.IdEspecialidad,
                Nombre = e.Nombre
            };

            return View(dto);
        }

        // POST: Especialidad/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EspecialidadEditDTO dto)
        {
            if (id != dto.IdEspecialidad) return NotFound();

            if (await _especialidadRepository.NombreExistsAsync(dto.Nombre, id))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe otra especialidad con este nombre");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (!await _especialidadRepository.ExistsAsync(id)) return NotFound();

            var especialidad = new Especialidad
            {
                IdEspecialidad = dto.IdEspecialidad,
                Nombre = dto.Nombre
            };

            await _especialidadRepository.UpdateAsync(especialidad);
            return RedirectToAction(nameof(Index));
        }

        // GET: Especialidad/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var e = await _especialidadRepository.GetByIdAsync(id.Value);
            if (e == null) return NotFound();

            var dto = new EspecialidadDTO
            {
                IdEspecialidad = e.IdEspecialidad,
                Nombre = e.Nombre,
                CantidadVeterinarios = await _especialidadRepository.ContarVeterinariosAsync(e.IdEspecialidad)
            };

            if (dto.CantidadVeterinarios > 0)
            {
                ViewBag.NoSePuedeEliminar = true;
                ViewBag.Mensaje = $"No se puede eliminar. Hay {dto.CantidadVeterinarios} veterinario(s) asignado(s) a esta especialidad.";
            }

            return View(dto);
        }

        // POST: Especialidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _especialidadRepository.TieneVeterinariosAsync(id))
            {
                TempData["Error"] = "No se puede eliminar la especialidad porque tiene veterinarios asignados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _especialidadRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}