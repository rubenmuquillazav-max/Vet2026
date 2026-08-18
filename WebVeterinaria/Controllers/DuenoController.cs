using Microsoft.AspNetCore.Mvc;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class DuenoController : Controller
    {
        private readonly IDuenoRepository _duenoRepository;

        public DuenoController(IDuenoRepository duenoRepository)
        {
            _duenoRepository = duenoRepository;
        }

        // GET: Dueno
        public async Task<IActionResult> Index(string? busqueda)
        {
            var duenos = await _duenoRepository.BuscarAsync(busqueda);

            ViewBag.Busqueda = busqueda;

            return View(duenos);
        }
        /*
        public async Task<IActionResult> Index()
        {
            var duenos = await _duenoRepository.GetAllAsync();

            var dto = duenos.Select(d => new DuenoDTO
            {
                IdDueno = d.IdDueno,
                Dni = d.Dni,
                Nombres = d.Nombres,
                Apellidos = d.Apellidos,
                Telefono = d.Telefono,
                Correo = d.Correo,
                Direccion = d.Direccion,
                Estado = d.Estado ?? true,
                CantidadMascotas = d.Mascota.Count
            }).ToList();

            return View(dto);
        }

        */

        // GET: Dueno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var dueno = await _duenoRepository.GetByIdAsync(id.Value);
            if (dueno == null) return NotFound();

            var dto = new DuenoDTO
            {
                IdDueno = dueno.IdDueno,
                Dni = dueno.Dni,
                Nombres = dueno.Nombres,
                Apellidos = dueno.Apellidos,
                Telefono = dueno.Telefono,
                Correo = dueno.Correo,
                Direccion = dueno.Direccion,
                Estado = dueno.Estado ?? true,
                CantidadMascotas = dueno.Mascota.Count
            };

            return View(dto);
        }

        // GET: Dueno/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dueno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DuenoCreateDTO dto)
        {
            if (await _duenoRepository.DniExistsAsync(dto.Dni))
            {
                ModelState.AddModelError(nameof(dto.Dni), "Ya existe un dueño registrado con este DNI");
            }

            if (!ModelState.IsValid) return View(dto);

            var dueno = new Dueno
            {
                Dni = dto.Dni,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Direccion = dto.Direccion,
                Estado = true
            };

            await _duenoRepository.AddAsync(dueno);
            return RedirectToAction(nameof(Index));
        }

        // GET: Dueno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dueno = await _duenoRepository.GetByIdAsync(id.Value);
            if (dueno == null) return NotFound();

            var dto = new DuenoEditDTO
            {
                IdDueno = dueno.IdDueno,
                Dni = dueno.Dni,
                Nombres = dueno.Nombres,
                Apellidos = dueno.Apellidos,
                Telefono = dueno.Telefono,
                Correo = dueno.Correo,
                Direccion = dueno.Direccion,
                Estado = dueno.Estado ?? true
            };

            return View(dto);
        }

        // POST: Dueno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DuenoEditDTO dto)
        {
            if (id != dto.IdDueno) return NotFound();

            if (await _duenoRepository.DniExistsAsync(dto.Dni, id))
            {
                ModelState.AddModelError(nameof(dto.Dni), "Ya existe otro dueño registrado con este DNI");
            }

            if (!ModelState.IsValid) return View(dto);

            if (!await _duenoRepository.ExistsAsync(id)) return NotFound();

            var dueno = new Dueno
            {
                IdDueno = dto.IdDueno,
                Dni = dto.Dni,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                Correo = dto.Correo,
                Direccion = dto.Direccion,
                Estado = dto.Estado
            };

            await _duenoRepository.UpdateAsync(dueno);
            return RedirectToAction(nameof(Index));
        }

        // GET: Dueno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var dueno = await _duenoRepository.GetByIdAsync(id.Value);
            if (dueno == null) return NotFound();

            var dto = new DuenoDTO
            {
                IdDueno = dueno.IdDueno,
                Dni = dueno.Dni,
                Nombres = dueno.Nombres,
                Apellidos = dueno.Apellidos,
                Telefono = dueno.Telefono,
                Correo = dueno.Correo,
                Direccion = dueno.Direccion,
                Estado = dueno.Estado ?? true,
                CantidadMascotas = dueno.Mascota.Count
            };

            return View(dto);
        }

        // POST: Dueno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _duenoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}