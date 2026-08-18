using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class RazaController : Controller
    {
        private readonly IRazaRepository _razaRepository;

        public RazaController(IRazaRepository razaRepository)
        {
            _razaRepository = razaRepository;
        }

        public async Task<IActionResult> Index(int? idEspecie)
        {
            var especies = await _razaRepository.GetEspeciesAsync();

            IEnumerable<Raza> razas;

            if (idEspecie.HasValue)
            {
                razas = await _razaRepository.GetByEspecieAsync(idEspecie.Value);
            }
            else
            {
                razas = await _razaRepository.GetAllAsync();
            }

            var dto = razas.Select(r => new RazaDTO
            {
                IdRaza = r.IdRaza,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                EspecieNombre = r.IdEspecieNavigation?.Nombre ?? ""
            }).ToList();

            ViewBag.Especies = new SelectList(
                especies,
                "IdEspecie",
                "Nombre",
                idEspecie
            );

            ViewBag.IdEspecieSeleccionada = idEspecie;

            return View(dto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var r = await _razaRepository.GetByIdAsync(id.Value);
            if (r == null) return NotFound();

            var dto = new RazaDTO
            {
                IdRaza = r.IdRaza,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                EspecieNombre = r.IdEspecieNavigation?.Nombre ?? ""
            };

            return View(dto);
        }

        public async Task<IActionResult> Create()
        {
            await CargarEspecies();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RazaFormDTO dto)
        {
            if (await _razaRepository.NombreExistsAsync(dto.Nombre, dto.IdEspecie))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe esta raza para la especie seleccionada");
            }

            if (!ModelState.IsValid)
            {
                await CargarEspecies(dto.IdEspecie);
                return View(dto);
            }

            var raza = new Raza { Nombre = dto.Nombre, IdEspecie = dto.IdEspecie };
            await _razaRepository.AddAsync(raza);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var r = await _razaRepository.GetByIdAsync(id.Value);
            if (r == null) return NotFound();

            var dto = new RazaFormDTO { IdRaza = r.IdRaza, Nombre = r.Nombre, IdEspecie = r.IdEspecie };
            await CargarEspecies(dto.IdEspecie);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RazaFormDTO dto)
        {
            if (id != dto.IdRaza) return NotFound();

            if (await _razaRepository.NombreExistsAsync(dto.Nombre, dto.IdEspecie, id))
            {
                ModelState.AddModelError(nameof(dto.Nombre), "Ya existe otra raza con este nombre para la especie seleccionada");
            }

            if (!ModelState.IsValid)
            {
                await CargarEspecies(dto.IdEspecie);
                return View(dto);
            }

            if (!await _razaRepository.ExistsAsync(id)) return NotFound();

            var raza = new Raza { IdRaza = dto.IdRaza, Nombre = dto.Nombre, IdEspecie = dto.IdEspecie };
            await _razaRepository.UpdateAsync(raza);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var r = await _razaRepository.GetByIdAsync(id.Value);
            if (r == null) return NotFound();

            var dto = new RazaDTO
            {
                IdRaza = r.IdRaza,
                Nombre = r.Nombre,
                IdEspecie = r.IdEspecie,
                EspecieNombre = r.IdEspecieNavigation?.Nombre ?? ""
            };

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _razaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarEspecies(int? seleccionado = null)
        {
            var especies = await _razaRepository.GetEspeciesAsync();
            ViewBag.Especies = new SelectList(especies, "IdEspecie", "Nombre", seleccionado);
        }

        
    }
}