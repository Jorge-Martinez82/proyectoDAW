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

    // metodo que llama al servicio para traer adoptantes con paginacion
    [HttpGet]
    public async Task<ActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(pageNumber, pageSize);
        return Ok(result);
    }

    // metodo que llama al servicio para traer un adoptante por su uuid
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AdoptanteDto>> GetByAdoptanteUuid(Guid id)
    {
        var adoptante = await _service.GetByAdoptanteUuidAsync(id);
        if (adoptante == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });
        return Ok(adoptante);
    }

    // metodo que devuelve el perfil del adoptante autenticado
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AdoptanteDto>> GetMe()
    {
        var adoptanteUuid = GetUserUuidFromToken();
        if (adoptanteUuid == null)
            return Unauthorized();

        var adoptante = await _service.GetByUsuarioUuidAsync(adoptanteUuid.Value);
        if (adoptante == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });

        return Ok(adoptante);
    }

    // metodo que actualiza el perfil del adoptante autenticado
    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult> UpdateMe([FromBody] AdoptanteDto dto)
    {
        var adoptanteUuid = GetUserUuidFromToken();
        if (adoptanteUuid == null)
            return Unauthorized();

        var existing = await _service.GetByUsuarioUuidAsync(adoptanteUuid.Value);
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
