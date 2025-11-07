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
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAll([FromQuery] int? protectoraId)
    {
        var animales = await _service.GetAllAsync(protectoraId);
        return Ok(animales);
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
