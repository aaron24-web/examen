using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Application.Services;

public class CourseService
{
    private readonly CourseRepository _courseRepository;
    private readonly InstructorRepository _instructorRepository; // Necesario para asociar instructores
    private readonly ModuleRepository _moduleRepository;
    private readonly LessonRepository _lessonRepository;
    private readonly IMapper _mapper;

    public CourseService(CourseRepository courseRepository, InstructorRepository instructorRepository, ModuleRepository moduleRepository, LessonRepository lessonRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
        _moduleRepository = moduleRepository;
        _lessonRepository = lessonRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
    {
        var course = _mapper.Map<Course>(createCourseDto);
        
        // Asocia los instructores existentes al curso
        if (createCourseDto.InstructorIds.Any())
        {
            // (Necesitaremos añadir un método GetByIdsAsync al InstructorRepository)
            // var instructors = await _instructorRepository.GetByIdsAsync(createCourseDto.InstructorIds);
            // course.Instructors.AddRange(instructors);
        }

        await _courseRepository.AddAsync(course);
        return _mapper.Map<CourseDto>(course);
    }
    
    public async Task AddModuleToCourseAsync(Guid courseId, CreateModuleDto createModuleDto)
    {
        var course = await _courseRepository.GetByIdWithIncludesAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");

        var newModule = _mapper.Map<Module>(createModuleDto);
        
        // Aplica la regla de negocio del Aggregate Root
        course.AddModule(newModule); 
        
        await _courseRepository.UpdateAsync(course);
    }

    public async Task PublishCourseAsync(Guid courseId)
    {
        var course = await _courseRepository.GetByIdWithIncludesAsync(courseId);
        if (course == null) throw new KeyNotFoundException("Course not found.");
        
        // Aplica la regla de negocio del Aggregate Root
        course.Publish();
        
        await _courseRepository.UpdateAsync(course);
    }
    
    // Aquí irían los métodos para añadir lecciones, obtener cursos, etc.
}