namespace EducationalPlatformApi.Core.Entities;
public class Lesson
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? VideoUrl { get; set; }
    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;
}