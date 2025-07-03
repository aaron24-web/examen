namespace EducationalPlatformApi.Core.DTOs;

public class CreateCourseDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public List<Guid> InstructorIds { get; set; } = [];
}

public class CourseDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public List<InstructorDto> Instructors { get; set; } = [];
    public List<ModuleDto> Modules { get; set; } = [];
}