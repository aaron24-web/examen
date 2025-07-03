namespace EducationalPlatformApi.Core.DTOs;

public class CreateLessonDto
{
    public required string Title { get; set; }
    public string? VideoUrl { get; set; }
}

public class LessonDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? VideoUrl { get; set; }
}