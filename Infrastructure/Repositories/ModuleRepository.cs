using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EducationalPlatformApi.Infrastructure.Repositories;

public class ModuleRepository
{
    private readonly ApplicationDbContext _context;
    public ModuleRepository(ApplicationDbContext context) => _context = context;

    public async Task<Module?> GetByIdWithLessonsAsync(Guid id)
    {
         return await _context.Modules.Include(m => m.Lessons).FirstOrDefaultAsync(m => m.Id == id);
    }
}