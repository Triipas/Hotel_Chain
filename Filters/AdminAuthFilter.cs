using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Filters
{
    public class AdminAuthFilter : IAsyncActionFilter
    {
        private readonly IAdminAuthService _adminAuthService;

        public AdminAuthFilter(IAdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Verificar si es una acción de login (permitir sin autenticación)
            var action = context.ActionDescriptor.RouteValues["action"];
            var controller = context.ActionDescriptor.RouteValues["controller"];

            if (controller == "Admin" && action == "Login")
            {
                await next();
                return;
            }

            // Verificar autenticación para todas las demás acciones de admin
            var isAuthenticated = await _adminAuthService.IsAdminAuthenticatedAsync(context.HttpContext);

            if (!isAuthenticated)
            {
                // Redirigir al login de admin
                context.Result = new RedirectToActionResult("Login", "Admin", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            // Si está autenticado, continuar con la acción
            await next();
        }
    }
}