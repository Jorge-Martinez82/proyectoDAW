using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdoptameDAW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtectorasController : ControllerBase
{
    private readonly ProtectorasService _service;

    public ProtectorasController(ProtectorasService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> ProtectorasControllerGetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? provincia = null)
    {
        var result = await _service.ProtectorasServiceGetAll(pageNumber, pageSize, provincia);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProtectoraDto>> ProtectorasControllerGetById(Guid id)
    {
        var protectora = await _service.ProtectorasServiceGetById(id);
        if (protectora == null)
            return NotFound(new { mensaje = "Protectora no encontrada" });

        return Ok(protectora);
    }

    
}
