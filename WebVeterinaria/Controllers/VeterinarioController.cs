using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class VeterinarioController : Controller
    {
        private readonly IVeterinarioRepository _veterinarioRepository;

        public VeterinarioController(IVeterinarioRepository veterinarioRepository)
        {
            _veterinarioRepository = veterinarioRepository;
        }

        // GET: Veterinario
        public async Task<IActionResult> Index(string? busqueda)
        {
            var veterinarios = await _veterinarioRepository.BuscarAsync(busqueda);

            var dto = veterinarios.Select(v => new VeterinarioDTO
            {
                IdVeterinario = v.IdVeterinario,
                Dni = v.Dni ?? "",
                Nombres = v.Nombres ?? "",
                Apellidos = v.Apellidos ?? "",
                Telefono = v.Telefono,
                Correo = v.Correo,
                Cmpv = v.Cmpv,
                Estado = v.Estado ?? true,
                IdEspecialidad = v.IdEspecialidad ?? 0,
                EspecialidadNombre = v.IdEspecialidadNavigation?.Nombre ?? "Sin especialidad"
            }).ToList();

            ViewBag.Busqueda = busqueda;

            return View(dto);
        }

        // GET: Veterinario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var v = await _veterinarioRepository.GetByIdAsync(id.Value);
            if (v == null) return NotFound();

            var dto = new VeterinarioDTO
            {
                IdVeterinario = v.IdVeterinario,
                Dni = v.Dni ?? "",
                Nombres = v.Nombres ?? "",
                Apellidos = v.Apellidos ?? "",
                Telefono = v.Telefono,
                Correo = v.Correo,
                Cmpv = v.Cmpv,
                Estado = v.Estado ?? true,
                IdEspecialidad = v.IdEspecialidad ?? 0,
                EspecialidadNombre = v.IdEspecialidadNavigation?.Nombre ?? "Sin especialidad"
            };

            return View(dto);
        }

        // GET: Veterinario/Create
        public async Task<IActionResult> Create()
        {
            await CargarEspecialidades();
            return View();
        }

        // POST: Veterinario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VeterinarioCreateDTO dto)
        {
            if (await _veterinarioRepository.DniExistsAsync(dto.Dni))
            {
                ModelState.AddModelError(nameof(dto.Dni), "Ya existe un veterinario registrado con este DNI");
            }

            if (await _veterinarioRepository.CmpvExistsAsync(dto.Cmpv))
            {
                ModelState.AddModelError(nameof(dto.Cmpv), "Ya existe un veterinario registrado con este CMVP");
            }

            if (!ModelState.IsValid)
            {
                await CargarEspecialidades(dto.IdEspecialidad);
                return View(dto);
            }

            var veterinario = new Veterinario
            {
                Dni = dto.Dni,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Cmpv = dto.Cmpv,
                IdEspecialidad = dto.IdEspecialidad,
                Estado = true
            };

            await _veterinarioRepository.AddAsync(veterinario);
            return RedirectToAction(nameof(Index));
        }

        // GET: Veterinario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var v = await _veterinarioRepository.GetByIdAsync(id.Value);
            if (v == null) return NotFound();

            var dto = new VeterinarioEditDTO
            {
                IdVeterinario = v.IdVeterinario,
                Dni = v.Dni ?? "",
                Nombres = v.Nombres ?? "",
                Apellidos = v.Apellidos ?? "",
                Telefono = v.Telefono,
                Correo = v.Correo,
                Cmpv = v.Cmpv ?? "",
                IdEspecialidad = v.IdEspecialidad ?? 0,
                Estado = v.Estado ?? true
            };

            await CargarEspecialidades(dto.IdEspecialidad);
            return View(dto);
        }

        // POST: Veterinario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VeterinarioEditDTO dto)
        {
            if (id != dto.IdVeterinario) return NotFound();

            if (await _veterinarioRepository.DniExistsAsync(dto.Dni, id))
            {
                ModelState.AddModelError(nameof(dto.Dni), "Ya existe otro veterinario registrado con este DNI");
            }

            if (await _veterinarioRepository.CmpvExistsAsync(dto.Cmpv, id))
            {
                ModelState.AddModelError(nameof(dto.Cmpv), "Ya existe otro veterinario registrado con este CMVP");
            }

            if (!ModelState.IsValid)
            {
                await CargarEspecialidades(dto.IdEspecialidad);
                return View(dto);
            }

            if (!await _veterinarioRepository.ExistsAsync(id)) return NotFound();

            var veterinario = new Veterinario
            {
                IdVeterinario = dto.IdVeterinario,
                Dni = dto.Dni,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Cmpv = dto.Cmpv,
                IdEspecialidad = dto.IdEspecialidad,
                Estado = dto.Estado
            };

            await _veterinarioRepository.UpdateAsync(veterinario);
            return RedirectToAction(nameof(Index));
        }

        // GET: Veterinario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var v = await _veterinarioRepository.GetByIdAsync(id.Value);
            if (v == null) return NotFound();

            var dto = new VeterinarioDTO
            {
                IdVeterinario = v.IdVeterinario,
                Dni = v.Dni ?? "",
                Nombres = v.Nombres ?? "",
                Apellidos = v.Apellidos ?? "",
                Telefono = v.Telefono,
                Correo = v.Correo,
                Cmpv = v.Cmpv,
                Estado = v.Estado ?? true,
                IdEspecialidad = v.IdEspecialidad ?? 0,
                EspecialidadNombre = v.IdEspecialidadNavigation?.Nombre ?? "Sin especialidad"
            };

            return View(dto);
        }

        // POST: Veterinario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _veterinarioRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarEspecialidades(int? seleccionado = null)
        {
            var especialidades = await _veterinarioRepository.GetEspecialidadesAsync();
            ViewBag.Especialidades = new SelectList(especialidades, "IdEspecialidad", "Nombre", seleccionado);
        }
    }
}