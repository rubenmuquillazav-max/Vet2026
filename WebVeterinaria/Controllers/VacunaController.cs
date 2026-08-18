using Microsoft.AspNetCore.Mvc;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class VacunaController : Controller
    {
        private readonly IVacunaRepository _vacunaRepository;

        public VacunaController(IVacunaRepository vacunaRepository)
        {
            _vacunaRepository = vacunaRepository;
        }

        // GET: Vacuna
        public async Task<IActionResult> Index()
        {
            var vacunas = await _vacunaRepository.GetAllAsync();

            var dto = new List<VacunaDTO>();
            foreach (var v in vacunas)
            {
                dto.Add(new VacunaDTO
                {
                    IdVacuna = v.IdVacuna,
                    Nombre = v.Nombre ?? "",
                    Descripcion = v.Descripcion,
                    CantidadAplicaciones = await _vacunaRepository.ContarAplicacionesAsync(v.IdVacuna)
                });
            }

            return View(dto);
        }

        // GET: Vacuna/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var v = await _vacunaRepository.GetByIdAsync(id.Value);
            if (v == null) return NotFound();

            var dto = new VacunaDTO
            {
                IdVacuna = v.IdVacuna,
                Nombre = v.Nombre ?? "",
                Descripcion = v.Descripcion,
                CantidadAplicaciones = await _vacunaRepository.ContarAplicacionesAsync(v.IdVacuna)
            };

            return View(dto);
        }

        // GET: Vacuna/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vacuna/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacunaCreateDTO dto)
        {
            if (await _vacunaRepository.NombreExistsAsync(dto.Nombre))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe una vacuna con este nombre");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var vacuna = new Vacuna
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            await _vacunaRepository.AddAsync(vacuna);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vacuna/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var v = await _vacunaRepository.GetByIdAsync(id.Value);
            if (v == null) return NotFound();

            var dto = new VacunaEditDTO
            {
                IdVacuna = v.IdVacuna,
                Nombre = v.Nombre ?? "",
                Descripcion = v.Descripcion
            };

            return View(dto);
        }

        // POST: Vacuna/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VacunaEditDTO dto)
        {
            if (id != dto.IdVacuna) return NotFound();

            if (await _vacunaRepository.NombreExistsAsync(dto.Nombre, id))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe otra vacuna con este nombre");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (!await _vacunaRepository.ExistsAsync(id)) return NotFound();

            var vacuna = new Vacuna
            {
                IdVacuna = dto.IdVacuna,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            await _vacunaRepository.UpdateAsync(vacuna);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vacuna/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var v = await _vacunaRepository.GetByIdAsync(id.Value);
            if (v == null) return NotFound();

            var dto = new VacunaDTO
            {
                IdVacuna = v.IdVacuna,
                Nombre = v.Nombre ?? "",
                Descripcion = v.Descripcion,
                CantidadAplicaciones = await _vacunaRepository.ContarAplicacionesAsync(v.IdVacuna)
            };

            if (dto.CantidadAplicaciones > 0)
            {
                ViewBag.NoSePuedeEliminar = true;
                ViewBag.Mensaje = $"No se puede eliminar. Esta vacuna tiene {dto.CantidadAplicaciones} aplicación(es) registrada(s) en mascotas.";
            }

            return View(dto);
        }

        // POST: Vacuna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _vacunaRepository.TieneAplicacionesAsync(id))
            {
                TempData["Error"] = "No se puede eliminar la vacuna porque tiene aplicaciones registradas.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _vacunaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}