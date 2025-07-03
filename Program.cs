using EducationalPlatformApi.Application.Services;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Infrastructure.Data;
using EducationalPlatformApi.Infrastructure.Repositories;
using EducationalPlatformApi.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar la conexión a la base de datos (DbContext)
var connectionString = builder.Configuration.GetConnectionString("Supabase");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 3. Registrar los Repositorios y Servicios
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<LessonRepository>();

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<InstructorService>();


// 4. Servicios estándar de la API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Educational Platform API",
        Description = "API for managing courses, modules, and lessons."
    });
    // Aquí añadiremos la configuración del botón 'Authorize' cuando implementemos JWT
});


var app = builder.Build();

// 5. Configurar el pipeline de peticiones HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Educational Platform API V1");
        c.RoutePrefix = string.Empty; // Swagger en la página de inicio
    });
}

app.UseHttpsRedirection();

// Aquí irán UseAuthentication y UseAuthorization cuando implementemos JWT
app.MapControllers();

app.Run();