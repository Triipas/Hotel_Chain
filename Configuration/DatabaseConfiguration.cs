// Configuration/DatabaseConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;

namespace Hotel_chain.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Primero intenta leer variable de entorno
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (!string.IsNullOrEmpty(connectionString))
            {
                // Usar cadena de conexión de Render (AWS)
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySQL(connectionString));
            }
            else
            {
                // Usar appsettings.json (localhost)
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySQL(configuration.GetConnectionString("DefaultConnection") ??
                                     throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));
            }

            return services;
        }
    }
}