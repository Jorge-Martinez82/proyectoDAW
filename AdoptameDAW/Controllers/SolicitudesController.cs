using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdoptameDAW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitudesController : ControllerBase
{
    private readonly SolicitudesService _service;

    public SolicitudesController(SolicitudesService service)
    {
        _service = service;
    }

    public class CrearSolicitudRequest
    {
        public Guid AnimalUuid { get; set; }
        public string Comentario { get; set; } = string.Empty;
    }

    public class ActualizarEstadoRequest
    {
        public string Estado { get; set; } = string.Empty;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SolicitudDto>> Crear([FromBody] CrearSolicitudRequest request)
    {
        var userUuid = GetUserUuid();
        var rol = GetUserRole();
        if (userUuid == null) return Unauthorized();
        if (rol != "Adoptante") return Forbid();

        var creada = await _service.CreateAsync(userUuid.Value, request.AnimalUuid, request.Comentario);
        if (creada == null)
            return BadRequest(new { mensaje = "Datos inválidos para crear la solicitud." });

        return Created(string.Empty, creada);
    }

    [Authorize]
    [HttpGet("adoptante")]
    public async Task<ActionResult> GetMisSolicitudesAdoptante([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userUuid = GetUserUuid();
        var rol = GetUserRole();
        if (userUuid == null) return Unauthorized();
        if (rol != "Adoptante") return Forbid();

        var resultado = await _service.GetByAdoptanteAsync(userUuid.Value, pageNumber, pageSize);
        return Ok(resultado);
    }

    [Authorize]
    [HttpGet("protectora")]
    public async Task<ActionResult> GetSolicitudesProtectora([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userUuid = GetUserUuid();
        var rol = GetUserRole();
        if (userUuid == null) return Unauthorized();
        if (rol != "Protectora") return Forbid();

        var resultado = await _service.GetByProtectoraAsync(userUuid.Value, pageNumber, pageSize);
        return Ok(resultado);
    }

    [Authorize]
    [HttpPut("{id:int}/estado")]
    public async Task<ActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoRequest request)
    {
        var userUuid = GetUserUuid();
        var rol = GetUserRole();
        if (userUuid == null) return Unauthorized();
        if (rol != "Protectora") return Forbid();

        var ok = await _service.UpdateEstadoAsync(userUuid.Value, id, request.Estado.ToLower());
        if (!ok)
            return BadRequest(new { mensaje = "No se pudo actualizar el estado (verifique solicitud, permisos o valor)." });

        return NoContent();
    }

    private Guid? GetUserUuid()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (claim == null) return null;
        return Guid.TryParse(claim.Value, out var guid) ? guid : null;
    }

    private string? GetUserRole()
    {
        var claim = User.FindFirst(ClaimTypes.Role);
        return claim?.Value;
    }
}