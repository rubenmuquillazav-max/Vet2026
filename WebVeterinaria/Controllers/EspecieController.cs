using Microsoft.AspNetCore.Mvc;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class EspecieController : Controller
    {
        private readonly IEspecieRepository _especieRepository;

        public EspecieController(IEspecieRepository especieRepository)
        {
            _especieRepository = especieRepository;
        }

        public async Task<IActionResult> Index()
        {
            var especies = await _especieRepository.GetAllAsync();

            var dto = especies.Select(e => new EspecieDTO
            {
                IdEspecie = e.IdEspecie,
                Nombre = e.Nombre,
                CantidadRazas = e.Razas.Count
            }).ToList();

            return View(dto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var e = await _especieRepository.GetByIdAsync(id.Value);
            if (e == null) return NotFound();

            var dto = new EspecieDTO
            {
                IdEspecie = e.IdEspecie,
                Nombre = e.Nombre,
                CantidadRazas = e.Razas.Count
            };

            return View(dto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EspecieFormDTO dto)
        {
            if (await _especieRepository.NombreExistsAsync(dto.Nombre))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe una especie con este nombre");
            }

            if (!ModelState.IsValid) return View(dto);

            var especie = new Especie { Nombre = dto.Nombre };
            await _especieRepository.AddAsync(especie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var e = await _especieRepository.GetByIdAsync(id.Value);
            if (e == null) return NotFound();

            var dto = new EspecieFormDTO { IdEspecie = e.IdEspecie, Nombre = e.Nombre };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EspecieFormDTO dto)
        {
            if (id != dto.IdEspecie) return NotFound();

            if (await _especieRepository.NombreExistsAsync(dto.Nombre, id))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe otra especie con este nombre");
            }

            if (!ModelState.IsValid) return View(dto);
            if (!await _especieRepository.ExistsAsync(id)) return NotFound();

            var especie = new Especie { IdEspecie = dto.IdEspecie, Nombre = dto.Nombre };
            await _especieRepository.UpdateAsync(especie);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var e = await _especieRepository.GetByIdAsync(id.Value);
            if (e == null) return NotFound();

            var dto = new EspecieDTO
            {
                IdEspecie = e.IdEspecie,
                Nombre = e.Nombre,
                CantidadRazas = e.Razas.Count
            };

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _especieRepository.TieneRazasAsync(id))
            {
                TempData["Error"] = "No se puede eliminar: esta especie tiene razas asociadas.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _especieRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}