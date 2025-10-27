using Hotel_chain.Configuration;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// 🔧 CONFIGURAR SERVICIOS
// ===============================
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureBusinessServices();
builder.Services.ConfigureApiServices();
builder.Services.ConfigureClientServices();
builder.Services.ConfigureAdminServices();

// 💳 NUEVO: Configuración de Mercado Pago
builder.Services.ConfigurePaymentServices(builder.Configuration);

// ===============================
// 🚀 CONSTRUIR APLICACIÓN
// ===============================
var app = builder.Build();

// ===============================
// 🌐 CONFIGURAR PIPELINE
// ===============================
app.ConfigurePipeline();

// ✅ RUTAS API Y MVC
app.MapControllers(); // Para que /api/pago funcione
app.MapDefaultControllerRoute();

// ===============================
// ▶️ EJECUTAR APP
// ===============================
app.Run();
