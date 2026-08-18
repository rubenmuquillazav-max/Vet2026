using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebVeterinaria.Filters
{
    public class SessionAuthFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var idUsuario = context.HttpContext.Session.GetInt32("IdUsuario");

            var controller = context.RouteData.Values["controller"]?.ToString();
            // Dejamos pasar libremente el controlador de Login
            if (controller == "Login") return;

            if (idUsuario == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}