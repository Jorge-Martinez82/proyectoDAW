using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdoptameDAW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdoptantesController : ControllerBase
{
    private readonly AdoptantesService _service;

    public AdoptantesController(AdoptantesService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AdoptanteDto>> GetByAdoptanteUuid(Guid id)
    {
        var adoptante = await _service.GetByAdoptanteUuidAsync(id);
        if (adoptante == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });
        return Ok(adoptante);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AdoptanteDto>> GetMe()
    {
        var userUuid = GetUserUuidFromToken();
        if (userUuid == null)
            return Unauthorized();

        var adoptante = await _service.GetByUsuarioUuidAsync(userUuid.Value);
        if (adoptante == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });

        return Ok(adoptante);
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult> UpdateMe([FromBody] AdoptanteDto dto)
    {
        var userUuid = GetUserUuidFromToken();
        if (userUuid == null)
            return Unauthorized();

        var existing = await _service.GetByUsuarioUuidAsync(userUuid.Value);
        if (existing == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });

        var ok = await _service.UpdateAsync(existing.Uuid, dto);
        if (!ok)
            return BadRequest(new { mensaje = "No se pudo actualizar el adoptante" });

        var actualizado = await _service.GetByAdoptanteUuidAsync(existing.Uuid);
        return Ok(actualizado);
    }

    private Guid? GetUserUuidFromToken()
    {
        var userIdToken = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdToken == null) return null;
        return Guid.TryParse(userIdToken.Value, out var guid) ? guid : null;
    }
}
