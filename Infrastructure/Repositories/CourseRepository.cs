using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class CourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdWithIncludesAsync(Guid id)
    {
        // Incluimos los módulos y los instructores al cargar un curso
        return await _context.Courses
            .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
            .Include(c => c.Instructors)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<IEnumerable<Course>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Courses
            .Include(c => c.Instructors)
            .OrderBy(c => c.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }
}