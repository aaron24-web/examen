using EducationalPlatformApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Le decimos a EF Core qué tablas existen en nuestra base de datos
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Instructor> Instructors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Nombres de tablas en minúscula/snake_case para coincidir con Supabase
        modelBuilder.Entity<Course>().ToTable("courses");
        modelBuilder.Entity<Module>().ToTable("modules");
        modelBuilder.Entity<Lesson>().ToTable("lessons");
        modelBuilder.Entity<Instructor>().ToTable("instructors");

        // Configuración de la relación Muchos a Muchos entre Course e Instructor
        // EF Core usará la tabla 'course_instructors' que creamos en la base de datos
        modelBuilder.Entity<Course>()
            .HasMany(c => c.Instructors)
            .WithMany(i => i.Courses)
            .UsingEntity("course_instructors");
    }
}