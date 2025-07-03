namespace EducationalPlatformApi.Core.Entities;
public class Instructor
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Bio { get; set; }
    public List<Course> Courses { get; set; } = new();
}