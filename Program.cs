using System.Text;
using EducationalPlatformApi.Application.Services;
using EducationalPlatformApi.Infrastructure.Data;
using EducationalPlatformApi.Infrastructure.Middleware; // <-- Using para el Middleware
using EducationalPlatformApi.Infrastructure.Repositories;
using EducationalPlatformApi.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("https://tu-frontend.com") // En producción, pon aquí la URL de tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. DbContext y Repositorios/Servicios
var connectionString = builder.Configuration.GetConnectionString("Supabase");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<ModuleRepository>();
builder.Services.AddScoped<LessonRepository>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<InstructorService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 3. Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 4. Configuración de Swagger para que use JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Educational Platform API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// --- El orden del Pipeline de Peticiones es CRÍTICO ---

// 1. El Middleware de IP va PRIMERO para bloquear accesos no deseados inmediatamente.
app.UseMiddleware<IpWhitelistMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Educational Platform API V1"); c.RoutePrefix = string.Empty; });
}

app.UseHttpsRedirection();

// 2. CORS va después del routing pero antes de la autenticación/autorización.
app.UseCors(MyAllowSpecificOrigins);

// 3. Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();