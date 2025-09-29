// Configuration/DatabaseConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;

namespace Hotel_chain.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("DefaultConnection") ?? 
                                throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))
            );

            return services;
        }
    }
}