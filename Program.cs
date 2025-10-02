// Program.cs - ACTUALIZADO con AWS S3
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using Hotel_chain.Configuration;
using Hotel_chain.Models.Configuration;
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Configurar servicios usando clases de configuración
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureBusinessServices();
builder.Services.ConfigureApiServices();
builder.Services.ConfigureClientServices();
builder.Services.ConfigureAdminServices();

// 🆕 Configurar AWS S3
var awsConfig = builder.Configuration.GetSection("AWS:S3").Get<AwsS3Config>();
builder.Services.Configure<AwsS3Config>(builder.Configuration.GetSection("AWS:S3"));

// Configurar cliente de S3
builder.Services.AddAWSService<IAmazonS3>(new AWSOptions
{
    Region = Amazon.RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"] ?? "us-east-2"),
    Credentials = new Amazon.Runtime.BasicAWSCredentials(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["AWS:SecretKey"]
    )
});

// Registrar servicio S3
builder.Services.AddScoped<IS3Service, S3Service>();

var app = builder.Build();

// 🔧 Configurar pipeline usando clase de configuración
app.ConfigurePipeline();

app.Run();