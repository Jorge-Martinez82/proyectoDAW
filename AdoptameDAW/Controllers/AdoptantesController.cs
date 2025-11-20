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
    public async Task<ActionResult> AdoptantesControllerGetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.AdoptantesServiceGetAll(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AdoptanteDto>> AdoptantesControllerGetById(Guid id)
    {
        var adoptante = await _service.AdoptantesServiceGetById(id);
        if (adoptante == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });
        return Ok(adoptante);
    }

    [Authorize] 
    [HttpGet("me")]
    public async Task<ActionResult<AdoptanteDto>> AdoptantesControllerGetMe()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (userIdClaim == null)
            return Unauthorized();

        if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            return Unauthorized();

        var adoptante = await _service.AdoptantesServiceGetById(userId);
        if (adoptante == null)
            return NotFound(new { mensaje = "Adoptante no encontrado" });

        return Ok(adoptante);
    }
}
