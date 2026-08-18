using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebVeterinaria.DTOs;
using WebVeterinaria.Models;
using WebVeterinaria.Repositories;

namespace WebVeterinaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();

            var dto = usuarios.Select(u => new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres ?? "",
                Apellidos = u.Apellidos ?? "",
                Usuario = u.Usuario1,
                Correo = u.Correo,
                Estado = u.Estado ?? true,
                IdRol = u.IdRol,
                RolNombre = u.IdRolNavigation?.Nombre ?? ""
            }).ToList();

            return View(dto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var u = await _usuarioRepository.GetByIdAsync(id.Value);
            if (u == null) return NotFound();

            var dto = new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres ?? "",
                Apellidos = u.Apellidos ?? "",
                Usuario = u.Usuario1,
                Correo = u.Correo,
                Estado = u.Estado ?? true,
                IdRol = u.IdRol,
                RolNombre = u.IdRolNavigation?.Nombre ?? ""
            };

            return View(dto);
        }

        public async Task<IActionResult> Create()
        {
            await CargarRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateDTO dto)
        {
            if (await _usuarioRepository.UsuarioExistsAsync(dto.Usuario))
            {
                ModelState.AddModelError(nameof(dto.Usuario), "Este nombre de usuario ya está en uso");
            }

            if (!ModelState.IsValid)
            {
                await CargarRoles(dto.IdRol);
                return View(dto);
            }

            var usuario = new Usuario
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Usuario1 = dto.Usuario,
                Clave = BCrypt.Net.BCrypt.HashPassword(dto.Clave),
                Correo = dto.Correo,
                IdRol = dto.IdRol,
                Estado = true
            };

            await _usuarioRepository.AddAsync(usuario);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var u = await _usuarioRepository.GetByIdAsync(id.Value);
            if (u == null) return NotFound();

            var dto = new UsuarioEditDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres ?? "",
                Apellidos = u.Apellidos ?? "",
                Usuario = u.Usuario1,
                Correo = u.Correo,
                IdRol = u.IdRol,
                Estado = u.Estado ?? true
                // NuevaClave se deja null a propósito: nunca mostramos el hash actual
            };

            await CargarRoles(dto.IdRol);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioEditDTO dto)
        {
            if (id != dto.IdUsuario) return NotFound();

            if (await _usuarioRepository.UsuarioExistsAsync(dto.Usuario, id))
            {
                ModelState.AddModelError(nameof(dto.Usuario), "Este nombre de usuario ya está en uso");
            }

            if (!ModelState.IsValid)
            {
                await CargarRoles(dto.IdRol);
                return View(dto);
            }

            var usuarioActual = await _usuarioRepository.GetByIdAsync(id);
            if (usuarioActual == null) return NotFound();

            var usuario = new Usuario
            {
                IdUsuario = dto.IdUsuario,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Usuario1 = dto.Usuario,
                Correo = dto.Correo,
                IdRol = dto.IdRol,
                Estado = dto.Estado,
                // Si mandaron nueva clave, se re-hashea; si no, se conserva el hash existente
                Clave = string.IsNullOrWhiteSpace(dto.NuevaClave)
                    ? usuarioActual.Clave
                    : BCrypt.Net.BCrypt.HashPassword(dto.NuevaClave)
            };

            await _usuarioRepository.UpdateAsync(usuario);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var u = await _usuarioRepository.GetByIdAsync(id.Value);
            if (u == null) return NotFound();

            var dto = new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                Nombres = u.Nombres ?? "",
                Apellidos = u.Apellidos ?? "",
                Usuario = u.Usuario1,
                Correo = u.Correo,
                Estado = u.Estado ?? true,
                IdRol = u.IdRol,
                RolNombre = u.IdRolNavigation?.Nombre ?? ""
            };

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Evitar que el usuario logueado se elimine a sí mismo
            var idUsuarioSesion = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuarioSesion == id)
            {
                TempData["Error"] = "No puedes eliminar tu propio usuario mientras estás en sesión.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            await _usuarioRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarRoles(int? seleccionado = null)
        {
            var roles = await _usuarioRepository.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, "IdRol", "Nombre", seleccionado);
        }
    }
}