using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebVeterinaria.Data;

namespace WebVeterinaria.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Datos de sesión
            ViewBag.NombreUsuario =
                HttpContext.Session.GetString("NombreUsuario");

            ViewBag.Rol =
                HttpContext.Session.GetString("Rol");

            // Totales
            ViewBag.TotalDuenos = await _context.Duenos
                .CountAsync(d => d.Estado == true);

            ViewBag.TotalMascotas = await _context.Mascota
                .CountAsync(m => m.Estado == true);

            ViewBag.TotalVeterinarios = await _context.Veterinarios
                .CountAsync(v => v.Estado == true);

            // Fecha actual
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            // Citas de hoy
            ViewBag.CitasHoy = await _context.Cita
                .CountAsync(c => c.Fecha == hoy);

            // Estados de citas de hoy
            ViewBag.CitasPendientes = await _context.Cita
                .CountAsync(c =>
                    c.Fecha == hoy &&
                    c.Estado == "PENDIENTE");

            ViewBag.CitasAtendidas = await _context.Cita
                .CountAsync(c =>
                    c.Fecha == hoy &&
                    c.Estado == "ATENDIDA");

            ViewBag.CitasCanceladas = await _context.Cita
                .CountAsync(c =>
                    c.Fecha == hoy &&
                    c.Estado == "CANCELADA");

            return View();
        }
    }
}