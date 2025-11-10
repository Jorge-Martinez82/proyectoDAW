using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdoptameDAW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalesController : ControllerBase
{
    private readonly AnimalesService _service;

    public AnimalesController(AnimalesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? protectoraId = null,
        [FromQuery] string? tipo = null,
        [FromQuery] string? provincia = null)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize, protectoraId, tipo, provincia);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalDto>> GetById(int id)
    {
        var animal = await _service.GetByIdAsync(id);
        if (animal == null)
            return NotFound(new { mensaje = "Animal no encontrado" });

        return Ok(animal);
    }
}
