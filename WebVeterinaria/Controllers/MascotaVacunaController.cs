using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebVeterinaria.Data;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebVeterinaria.Controllers
{
    public class MascotaVacunaController : Controller
    {
        private readonly IMascotaVacunaRepository _repository;
        private readonly AppDbContext _context;

        public MascotaVacunaController(
            IMascotaVacunaRepository repository,
            AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IActionResult> Index(int? idDueno)
        {
            var registros = await _repository.GetAllAsync(idDueno);

            var duenos = await _context.Duenos
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .ToListAsync();

            ViewBag.Duenos = new SelectList(
                duenos.Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto = $"{d.Apellidos} {d.Nombres} - DNI {d.Dni}"
                }),
                "IdDueno",
                "NombreCompleto",
                idDueno
            );

            ViewBag.IdDueno = idDueno;

            return View(registros);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasAsync();

            var dto = new MascotaVacunaCreateDTO
            {
                Fecha = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MascotaVacunaCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasAsync();
                return View(dto);
            }

            var registro = new MascotaVacuna
            {
                IdMascota = dto.IdMascota,
                IdVacuna = dto.IdVacuna,
                Fecha = dto.Fecha,
                ProximaDosis = dto.ProximaDosis,
                Observaciones = dto.Observaciones
            };

            await _repository.AddAsync(registro);

            TempData["Success"] =
                "Vacuna registrada correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: MascotaVacuna/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var registro = await _repository.GetByIdAsync(id);

            if (registro == null)
                return NotFound();

            var dto = new MascotaVacunaCreateDTO
            {
                IdMascota = registro.IdMascota ?? 0,
                IdVacuna = registro.IdVacuna ?? 0,
                Fecha = registro.Fecha,
                ProximaDosis = registro.ProximaDosis,
                Observaciones = registro.Observaciones
            };

            ViewBag.IdRegistro = registro.Id;

            ViewBag.NombreMascota =
                registro.IdMascotaNavigation?.Nombre ?? "";

            ViewBag.NombreDueno =
                registro.IdMascotaNavigation?.IdDuenoNavigation != null
                ? registro.IdMascotaNavigation.IdDuenoNavigation.Nombres
                  + " " +
                  registro.IdMascotaNavigation.IdDuenoNavigation.Apellidos
                : "";

            var vacunas = await _repository.GetVacunasAsync();

            ViewBag.Vacunas = new SelectList(
                vacunas,
                "IdVacuna",
                "Nombre",
                dto.IdVacuna
            );

            return View(dto);
        }

        // POST: MascotaVacuna/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    MascotaVacunaCreateDTO dto)
        {
            var registro = await _repository.GetByIdAsync(id);

            if (registro == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.IdRegistro = registro.Id;

                ViewBag.NombreMascota =
                    registro.IdMascotaNavigation?.Nombre ?? "";

                ViewBag.NombreDueno =
                    registro.IdMascotaNavigation?.IdDuenoNavigation != null
                        ? registro.IdMascotaNavigation.IdDuenoNavigation.Nombres
                          + " " +
                          registro.IdMascotaNavigation.IdDuenoNavigation.Apellidos
                        : "";

                var vacunas = await _repository.GetVacunasAsync();

                ViewBag.Vacunas = new SelectList(
                    vacunas,
                    "IdVacuna",
                    "Nombre",
                    dto.IdVacuna
                );

                return View(dto);
            }

            // IMPORTANTE:
            // NO modificamos IdMascota.
            // Se conserva la mascota original.

            registro.IdVacuna = dto.IdVacuna;
            registro.Fecha = dto.Fecha;
            registro.ProximaDosis = dto.ProximaDosis;
            registro.Observaciones = dto.Observaciones;

            await _repository.UpdateAsync(registro);

            TempData["Success"] =
                "Registro de vacuna actualizado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        // GET: MascotaVacuna/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var registro = await _repository.GetByIdAsync(id);

            if (registro == null)
                return NotFound();

            return View(registro);
        }

        // POST: MascotaVacuna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);

            TempData["Success"] =
                "Registro de vacuna eliminado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMascotasPorDueno(int idDueno)
        {
            var mascotas = await _repository.GetMascotasPorDuenoAsync(idDueno);

            var resultado = mascotas.Select(m => new
            {
                idMascota = m.IdMascota,
                nombre = m.Nombre
            });

            return Json(resultado);
        }

        private async Task CargarListasAsync()
        {
            // Dueños activos
            var duenos = await _context.Duenos
                .Where(d => d.Estado == true)
                .OrderBy(d => d.Apellidos)
                .ThenBy(d => d.Nombres)
                .ToListAsync();

            ViewBag.Duenos = new SelectList(
                duenos.Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto = d.Apellidos + " " + d.Nombres
                }),
                "IdDueno",
                "NombreCompleto"
            );

            // Vacunas
            var vacunas = await _repository.GetVacunasAsync();

            ViewBag.Vacunas = new SelectList(
                vacunas,
                "IdVacuna",
                "Nombre"
            );
        }


    }
}