namespace EducationalPlatformApi.Core.Entities;
public class Module
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public List<Lesson> Lessons { get; set; } = [];
}