using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class MascotaController : Controller
    {
        private readonly IMascotaRepository _mascotaRepository;

        public MascotaController(IMascotaRepository mascotaRepository)
        {
            _mascotaRepository = mascotaRepository;
        }

        public async Task<IActionResult> Index(int? idDueno)
        {
            var mascotas = await _mascotaRepository.GetAllAsync(idDueno);

            var dto = mascotas.Select(MapToDTO).ToList();

            var duenos = await _mascotaRepository.GetDuenosAsync();

            ViewBag.DuenosFiltro = new SelectList(
                duenos.Select(d => new
                {
                    d.IdDueno,
                    NombreCompleto = $"{d.Nombres} {d.Apellidos} - DNI {d.Dni}"
                }),
                "IdDueno",
                "NombreCompleto",
                idDueno);

            ViewBag.DuenoFiltro = idDueno;

            return View(dto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var m = await _mascotaRepository.GetByIdAsync(id.Value);
            if (m == null) return NotFound();

            return View(MapToDTO(m));
        }

        public async Task<IActionResult> Create()
        {
            await CargarCombos();
            return View();
        }

        /* -------------- */
        // GET: Mascota/RazasPorEspecie/3
        [HttpGet]
        public async Task<JsonResult> RazasPorEspecie(int idEspecie)
        {
            var razas = await _mascotaRepository.GetRazasPorEspecieAsync(idEspecie);
            var resultado = razas.Select(r => new { id = r.IdRaza, nombre = r.Nombre });
            return Json(resultado);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MascotaFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(dto.IdDueno, dto.IdEspecie, dto.IdRaza);
                return View(dto);
            }

            var mascota = new Mascotum
            {
                Nombre = dto.Nombre,
                FechaNacimiento = dto.FechaNacimiento,
                Sexo = dto.Sexo,
                Color = dto.Color,
                Peso = dto.Peso,
                Alergias = dto.Alergias,
                Observaciones = dto.Observaciones,
                IdDueno = dto.IdDueno,
                IdEspecie = dto.IdEspecie,
                IdRaza = dto.IdRaza,
                Estado = true
            };

            await _mascotaRepository.AddAsync(mascota);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var m = await _mascotaRepository.GetByIdAsync(id.Value);
            if (m == null) return NotFound();

            var dto = new MascotaFormDTO
            {
                IdMascota = m.IdMascota,
                Nombre = m.Nombre,
                FechaNacimiento = m.FechaNacimiento,
                Sexo = m.Sexo,
                Color = m.Color,
                Peso = m.Peso,
                Alergias = m.Alergias,
                Observaciones = m.Observaciones,
                IdDueno = m.IdDueno,
                IdEspecie = m.IdEspecie,
                IdRaza = m.IdRaza,
                Estado = m.Estado ?? true
            };

            await CargarCombos(dto.IdDueno, dto.IdEspecie, dto.IdRaza);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MascotaFormDTO dto)
        {
            if (id != dto.IdMascota) return NotFound();

            if (!ModelState.IsValid)
            {
                await CargarCombos(dto.IdDueno, dto.IdEspecie, dto.IdRaza);
                return View(dto);
            }

            if (!await _mascotaRepository.ExistsAsync(id)) return NotFound();

            var mascota = new Mascotum
            {
                IdMascota = dto.IdMascota,
                Nombre = dto.Nombre,
                FechaNacimiento = dto.FechaNacimiento,
                Sexo = dto.Sexo,
                Color = dto.Color,
                Peso = dto.Peso,
                Alergias = dto.Alergias,
                Observaciones = dto.Observaciones,
                IdDueno = dto.IdDueno,
                IdEspecie = dto.IdEspecie,
                IdRaza = dto.IdRaza,
                Estado = dto.Estado
            };

            await _mascotaRepository.UpdateAsync(mascota);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var m = await _mascotaRepository.GetByIdAsync(id.Value);
            if (m == null) return NotFound();

            return View(MapToDTO(m));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mascotaRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private static MascotaDTO MapToDTO(Mascotum m)
        {
            return new MascotaDTO
            {
                IdMascota = m.IdMascota,
                Nombre = m.Nombre,
                FechaNacimiento = m.FechaNacimiento,
                Sexo = m.Sexo,
                Color = m.Color,
                Peso = m.Peso,
                Alergias = m.Alergias,
                Observaciones = m.Observaciones,
                Estado = m.Estado ?? true,
                IdDueno = m.IdDueno,
                DuenoNombreCompleto = m.IdDuenoNavigation != null
                    ? $"{m.IdDuenoNavigation.Nombres} {m.IdDuenoNavigation.Apellidos}"
                    : "",
                IdEspecie = m.IdEspecie,
                EspecieNombre = m.IdEspecieNavigation?.Nombre ?? "",
                IdRaza = m.IdRaza,
                RazaNombre = m.IdRazaNavigation?.Nombre ?? ""
            };
        }

        private async Task CargarCombos(int? idDueno = null, int? idEspecie = null, int? idRaza = null)
        {
            var duenos = await _mascotaRepository.GetDuenosAsync();
            ViewBag.Duenos = new SelectList(
                duenos.Select(d => new { d.IdDueno, NombreCompleto = $"{d.Nombres} {d.Apellidos} - DNI {d.Dni}" }),
                "IdDueno", "NombreCompleto", idDueno);

            var especies = await _mascotaRepository.GetEspeciesAsync();
            ViewBag.Especies = new SelectList(especies, "IdEspecie", "Nombre", idEspecie);

            // Si ya hay una especie seleccionada (Edit, o Create con error de validación),
            // cargamos solo las razas de esa especie. Si no, el combo arranca vacío.
            if (idEspecie.HasValue && idEspecie.Value > 0)
            {
                var razas = await _mascotaRepository.GetRazasPorEspecieAsync(idEspecie.Value);
                ViewBag.Razas = new SelectList(razas, "IdRaza", "Nombre", idRaza);
            }
            else
            {
                ViewBag.Razas = new SelectList(Enumerable.Empty<Raza>(), "IdRaza", "Nombre");
            }
        }
    }
}