using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class InstructorRepository
{
    private readonly ApplicationDbContext _context;

    public InstructorRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Instructors.AnyAsync(i => i.Name.ToLower() == name.ToLower());
    }
    
    public async Task AddAsync(Instructor instructor)
    {
        await _context.Instructors.AddAsync(instructor);
        await _context.SaveChangesAsync();
    }
    
    // Aquí irían los métodos GetAllAsync, GetByIdAsync, etc.
}