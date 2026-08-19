using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class AtencionController : Controller
    {
        private readonly IAtencionRepository _atencionRepository;

        public AtencionController(
            IAtencionRepository atencionRepository)
        {
            _atencionRepository = atencionRepository;
        }


        // =========================================================
        // GET: Atencion
        // =========================================================
        public async Task<IActionResult> Index(
    int? idDueno,
    int? idVeterinario)
        {
            var atenciones = await _atencionRepository.GetAllAsync(
                idDueno,
                idVeterinario);

            var duenos = await _atencionRepository.GetDuenosAsync();

            ViewBag.Duenos = new SelectList(
                duenos.Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto =
                        $"{d.Nombres} {d.Apellidos} - DNI {d.Dni}"
                }),
                "IdDueno",
                "NombreCompleto",
                idDueno
            );

            var veterinarios =
                await _atencionRepository.GetVeterinariosAsync();

            ViewBag.Veterinarios = new SelectList(
                veterinarios.Select(v => new
                {
                    v.IdVeterinario,
                    NombreCompleto =
                        $"Dr(a). {v.Nombres} {v.Apellidos}"
                }),
                "IdVeterinario",
                "NombreCompleto",
                idVeterinario
            );

            return View(atenciones);
        }


        // =========================================================
        // GET: Atencion/Details/5
        // =========================================================
        public async Task<IActionResult> Details(int id)
        {
            var atencion =
                await _atencionRepository.GetByIdAsync(id);

            if (atencion == null)
                return NotFound();

            return View(atencion);
        }


        // =========================================================
        // GET: Atencion/Create
        // =========================================================
        public async Task<IActionResult> Create()
        {
            await CargarCitasDisponibles();

            return View();
        }


        // =========================================================
        // POST: Atencion/Create
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            AtencionCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                var atencion = new Atencion
                {
                    IdCita = dto.IdCita,

                    Temperatura = dto.Temperatura,

                    Peso = dto.Peso,

                    Diagnostico = dto.Diagnostico,

                    Tratamiento = dto.Tratamiento,

                    Observaciones = dto.Observaciones
                };


                await _atencionRepository
                    .AddAsync(atencion);


                TempData["Success"] =
                    "Atención registrada correctamente.";


                return RedirectToAction(nameof(Index));
            }


            await CargarCitasDisponibles();

            return View(dto);
        }


        // =========================================================
        // GET: Atencion/Edit/5
        // =========================================================
        public async Task<IActionResult> Edit(int id)
        {
            var dto =
                await _atencionRepository
                    .GetForEditAsync(id);

            if (dto == null)
                return NotFound();


            await CargarCitasDisponibles(
                dto.IdCita);


            return View(dto);
        }


        // =========================================================
        // POST: Atencion/Edit/5
        // =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            AtencionEditDto dto)
        {
            if (id != dto.IdAtencion)
                return NotFound();


            if (ModelState.IsValid)
            {
                var atencion =
                    await _atencionRepository
                        .GetByIdAsync(id);


                if (atencion == null)
                    return NotFound();


                atencion.IdCita =
                    dto.IdCita;

                atencion.Temperatura =
                    dto.Temperatura;

                atencion.Peso =
                    dto.Peso;

                atencion.Diagnostico =
                    dto.Diagnostico;

                atencion.Tratamiento =
                    dto.Tratamiento;

                atencion.Observaciones =
                    dto.Observaciones;


                // La fecha original no se modifica


                await _atencionRepository
                    .UpdateAsync(atencion);


                TempData["Success"] =
                    "Atención actualizada correctamente.";


                return RedirectToAction(nameof(Index));
            }


            await CargarCitasDisponibles(
                dto.IdCita);


            return View(dto);
        }


        // =========================================================
        // GET: Atencion/Delete/5
        // =========================================================
        public async Task<IActionResult> Delete(int id)
        {
            var atencion =
                await _atencionRepository
                    .GetByIdAsync(id);


            if (atencion == null)
                return NotFound();


            return View(atencion);
        }


        // =========================================================
        // POST: Atencion/Delete/5
        // =========================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            await _atencionRepository
                .DeleteAsync(id);


            TempData["Success"] =
                "Atención eliminada correctamente.";


            return RedirectToAction(nameof(Index));
        }


        // =========================================================
        // CARGAR CITAS DISPONIBLES
        // =========================================================
        private async Task CargarCitasDisponibles(
            int? idCitaActual = null)
        {
            var citas =
                await _atencionRepository
                    .GetCitasDisponiblesAsync(
                        idCitaActual);


            ViewBag.Citas = new SelectList(
                citas,
                "IdCita",
                "Texto",
                idCitaActual
            );
        }
    }
}