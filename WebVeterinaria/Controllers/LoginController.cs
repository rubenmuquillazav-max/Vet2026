using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;

namespace WebVeterinaria.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Login
        public IActionResult Index()
        {
            // Si ya hay sesión activa, mándalo directo al dashboard
            if (HttpContext.Session.GetInt32("IdUsuario") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.Error = "Debes ingresar usuario y contraseña.";
                return View();
            }

            var user = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.Usuario1 == usuario && u.Estado == true);

            if (user == null || !BCrypt.Net.BCrypt.Verify(clave, user.Clave))
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            // Guardamos datos clave en sesión
            HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);
            HttpContext.Session.SetString("NombreUsuario", user.Nombres + " " + user.Apellidos);
            HttpContext.Session.SetString("Rol", user.IdRolNavigation.Nombre);
            HttpContext.Session.SetInt32("IdRol", user.IdRol);

            return RedirectToAction("Index", "Dashboard");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}