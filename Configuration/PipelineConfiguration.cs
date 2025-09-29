// Configuration/PipelineConfiguration.cs
namespace Hotel_chain.Configuration
{
    public static class PipelineConfiguration
    {
        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Usar CORS
            app.UseCors("AdminPanel");

            // Middleware de sesión (mantener para cliente)
            app.UseSession();
            app.UseAuthorization();

            // Configurar rutas
            ConfigureRoutes(app);

            return app;
        }

        private static void ConfigureRoutes(WebApplication app)
        {
            // Rutas para API
            app.MapControllers(); // Mapea automáticamente los controladores API con [ApiController]

            // Ruta específica para el panel admin
            app.MapControllerRoute(
                name: "admin",
                pattern: "admin/{action=Index}/{id?}",
                defaults: new { controller = "Admin", action = "Index" }
            );

            // Ruta para controladores cliente (mantener funcionalidad existente)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
        }
    }
}