using Hotel_chain.Data;
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Services.Implementation;
using Hotel_chain.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ DbContext con MySQL (mantener igual)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))
);

// ✅ Servicios MVC para el frontend cliente (mantener)
builder.Services.AddControllersWithViews();

// 🆕 Servicios API para los controladores API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar JSON para la API
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantener PascalCase
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 🆕 Registrar servicios de lógica de negocio
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHabitacionService, HabitacionService>(); // 🆕 Servicio de habitaciones
builder.Services.AddScoped<IAdminAuthService, AdminAuthService>(); // 🆕 Servicio de autenticación admin
// TODO: Agregar otros servicios cuando los creemos
// builder.Services.AddScoped<IReservaService, ReservaService>();
// builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// 🆕 Registrar filtros
builder.Services.AddScoped<AdminAuthFilter>(); // 🚫 Comentado temporalmente

// ✅ Servicios para sesiones (mantener para cliente)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// 🆕 CORS para permitir que el frontend admin consuma la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AdminPanel", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🆕 Configurar Areas para el panel admin

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🆕 Usar CORS
app.UseCors("AdminPanel");

// ✅ Middleware de sesión (mantener para cliente)
app.UseSession();

app.UseAuthorization();

// 🆕 Configurar rutas para API
app.MapControllers(); // Esto mapea automáticamente los controladores API con [ApiController]

// 🆕 Ruta específica para el panel admin
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin", action = "Index" }
);

// ✅ Ruta para controladores cliente (mantener funcionalidad existente)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();