namespace EducationalPlatformApi.Core.DTOs;

public class CreateInstructorDto
{
    public required string Name { get; set; }
    public string? Bio { get; set; }
}

public class InstructorDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Bio { get; set; }
}