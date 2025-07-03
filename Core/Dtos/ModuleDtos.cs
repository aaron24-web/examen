namespace EducationalPlatformApi.Core.DTOs;

public class CreateModuleDto
{
    public required string Title { get; set; }
}

public class ModuleDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public List<LessonDto> Lessons { get; set; } = [];
}