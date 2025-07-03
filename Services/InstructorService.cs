using AutoMapper;
using EducationalPlatformApi.Core.DTOs;
using EducationalPlatformApi.Core.Entities;
using EducationalPlatformApi.Infrastructure.Repositories;

namespace EducationalPlatformApi.Application.Services;

public class InstructorService
{
    private readonly InstructorRepository _instructorRepository;
    private readonly IMapper _mapper;

    public InstructorService(InstructorRepository instructorRepository, IMapper mapper)
    {
        _instructorRepository = instructorRepository;
        _mapper = mapper;
    }

    public async Task<InstructorDto> CreateInstructorAsync(CreateInstructorDto createInstructorDto)
    {
        // Regla de Negocio: No permitir instructores con nombres repetidos
        if (await _instructorRepository.ExistsByNameAsync(createInstructorDto.Name))
        {
            throw new InvalidOperationException("An instructor with this name already exists.");
        }

        var instructor = _mapper.Map<Instructor>(createInstructorDto);
        await _instructorRepository.AddAsync(instructor);
        
        return _mapper.Map<InstructorDto>(instructor);
    }

    // Aquí irían los métodos para obtener, actualizar y eliminar instructores
}