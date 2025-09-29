// Configuration/ClientConfiguration.cs
namespace Hotel_chain.Configuration
{
    public static class ClientConfiguration
    {
        public static IServiceCollection ConfigureClientServices(this IServiceCollection services)
        {
            // Servicios MVC para el frontend cliente
            services.AddControllersWithViews();

            // Servicios para sesiones (mantener para cliente)
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
