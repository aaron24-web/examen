using EducationalPlatformApi.Application.Services;
using EducationalPlatformApi.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPlatformApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly CourseService _courseService;

    public CoursesController(CourseService courseService)
    {
        _courseService = courseService;
    }

    // Endpoint para añadir un módulo a un curso existente
    // POST /api/courses/{courseId}/modules
    [HttpPost("{courseId}/modules")]
    public async Task<IActionResult> AddModule(Guid courseId, [FromBody] CreateModuleDto createModuleDto)
    {
        try
        {
            await _courseService.AddModuleToCourseAsync(courseId, createModuleDto);
            return Ok("Module added successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Captura el error si el curso ya está publicado
            return BadRequest(new { message = ex.Message });
        }
    }

    // Endpoint para publicar un curso
    // POST /api/courses/{courseId}/publish
    [HttpPost("{courseId}/publish")]
    public async Task<IActionResult> Publish(Guid courseId)
    {
        try
        {
            await _courseService.PublishCourseAsync(courseId);
            return NoContent(); // 204 No Content es una buena respuesta para una acción exitosa sin retorno de datos
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Captura el error si el curso no tiene módulos o ya está publicado
            return BadRequest(new { message = ex.Message });
        }
    }
}