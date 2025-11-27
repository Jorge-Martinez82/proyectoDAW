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

    // metodo que llama al servicio para traer animales con filtros y paginacion
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

    // metodo que llama al servicio para traer un animal por su uuid
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnimalDto>> GetById(Guid id)
    {
        var animal = await _service.GetByIdAsync(id);
        if (animal == null)
            return NotFound(new { mensaje = "Animal no encontrado" });

        return Ok(animal);
    }

    // metodo que llama al servicio para eliminar un animal por su uuid
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        if (!ok)
            return NotFound(new { mensaje = "Animal no encontrado" });
        return NoContent();
    }

    // metodo que llama al servicio para crear un nuevo animal
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AnimalDto>> Create([FromBody] AnimalDto request)
    {
        var creado = await _service.CreateAsync(request);
        if (creado == null)
            return BadRequest(new { mensaje = "Datos inválidos" });
        return Created(string.Empty, creado);
    }
}
