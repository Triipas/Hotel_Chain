// Program.cs
using Hotel_chain.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Configurar servicios usando clases de configuración
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureBusinessServices();
builder.Services.ConfigureApiServices();
builder.Services.ConfigureClientServices();
builder.Services.ConfigureAdminServices();

var app = builder.Build();

// 🔧 Configurar pipeline usando clase de configuración
app.ConfigurePipeline();

app.Run();