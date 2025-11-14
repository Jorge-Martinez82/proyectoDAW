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
        [FromQuery] Guid? protectoraUuid = null,
        [FromQuery] string? tipo = null,
        [FromQuery] string? provincia = null)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize, protectoraUuid, tipo, provincia);
        return Ok(result);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnimalDto>> GetById(Guid id)
    {
        var animal = await _service.GetByIdAsync(id);
        if (animal == null)
            return NotFound(new { mensaje = "Animal no encontrado" });

        return Ok(animal);
    }

}
