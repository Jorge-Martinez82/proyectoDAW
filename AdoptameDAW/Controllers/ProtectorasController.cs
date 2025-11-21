using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    public async Task<ActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? provincia = null)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize, provincia);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProtectoraDto>> GetByUuid(Guid id)
    {
        var protectora = await _service.GetByUuidAsync(id);
        if (protectora == null)
            return NotFound(new { mensaje = "Protectora no encontrada" });

        return Ok(protectora);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<ProtectoraDto>> GetMe()
    {
        var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdToken == null) return Unauthorized();
        if (!Guid.TryParse(userIdToken.Value, out var userUuid)) return Unauthorized();

        var protectora = await _service.GetByUsuarioUuidAsync(userUuid);
        if (protectora == null)
            return NotFound(new { mensaje = "Protectora no encontrada" });

        return Ok(protectora);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult> UpdateMe([FromBody] ProtectoraDto dto)
    {
        var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdToken == null) return Unauthorized();
        if (!Guid.TryParse(userIdToken.Value, out var userUuid)) return Unauthorized();

        var existing = await _service.GetByUsuarioUuidAsync(userUuid);
        if (existing == null)
            return NotFound(new { mensaje = "Protectora no encontrada" });

        var ok = await _service.UpdateAsync(existing.Uuid, dto);
        if (!ok)
            return BadRequest(new { mensaje = "No se pudo actualizar la protectora" });

        var actualizado = await _service.GetByUuidAsync(existing.Uuid);
        return Ok(actualizado);
    }
}
