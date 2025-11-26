using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult> AnimalesControllerGetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? protectoraUuid = null,
        [FromQuery] string? tipo = null,
        [FromQuery] string? provincia = null)
    {
        var result = await _service.AnimalesServiceGetAll(pageNumber, pageSize, protectoraUuid, tipo, provincia);
        return Ok(result);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnimalDto>> AnimalesControllerGetById(Guid id)
    {
        var animal = await _service.AnimalesServiceGetById(id);
        if (animal == null)
            return NotFound(new { mensaje = "Animal no encontrado" });

        return Ok(animal);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> AnimalesControllerDelete(Guid id)
    {
        var ok = await _service.AnimalesServiceDelete(id);
        if (!ok)
            return NotFound(new { mensaje = "Animal no encontrado" });
        return NoContent();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AnimalDto>> AnimalesControllerCreate([FromBody] AnimalDto request)
    {
        var creado = await _service.AnimalesServiceCreate(request);
        if (creado == null)
            return BadRequest(new { mensaje = "Datos inválidos" });
        return Created(string.Empty, creado);
    }
}
