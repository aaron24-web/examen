using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;

namespace EducationalPlatformApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeos para Instructor
        CreateMap<Instructor, InstructorDto>();
        CreateMap<CreateInstructorDto, Instructor>();

        // Mapeos para Course
        CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>()
            .ForMember(dest => dest.Instructors, opt => opt.Ignore()); // Ignoramos los instructores aquí, los manejaremos en el servicio

        // Mapeos para Module
        CreateMap<Module, ModuleDto>();
        CreateMap<CreateModuleDto, Module>();

        // Mapeos para Lesson
        CreateMap<Lesson, LessonDto>();
        CreateMap<CreateLessonDto, Lesson>();
    }
}