using System.Linq;

namespace EducationalPlatformApi.Core.Entities;
public class Course
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublished { get; private set; } = false;
    public List<Module> Modules { get; set; } = [];
    public List<Instructor> Instructors { get; set; } = [];

    public void Publish()
    {
        if (IsPublished) throw new InvalidOperationException("Course is already published.");
        if (!Modules.Any()) throw new InvalidOperationException("Cannot publish a course with no modules.");
        IsPublished = true;
    }

    public void AddModule(Module module)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot add a module to a published course.");
        Modules.Add(module);
    }

     public void AddInstructor(Instructor instructor)
    {
         if (IsPublished) throw new InvalidOperationException("Cannot add an instructor to a published course.");
         if (!Instructors.Any(i => i.Id == instructor.Id)) Instructors.Add(instructor);
    }

     public void RemoveInstructor(Guid instructorId)
    {
        if (IsPublished) throw new InvalidOperationException("Cannot remove an instructor from a published course.");
        var instructor = Instructors.FirstOrDefault(i => i.Id == instructorId);
        if (instructor != null) Instructors.Remove(instructor);
    }
}