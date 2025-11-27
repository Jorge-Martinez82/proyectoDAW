using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AdoptameDAW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuariosService _service;

    public UsuariosController(UsuariosService service)
    {
        _service = service;
    }

    // metodo que realiza login y devuelve token y usuario
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest(new { mensaje = "Email y contraseña son requeridos" });

        var result = await _service.LoginAsync(request);
        if (result == null)
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });

        return Ok(result);
    }

    // metodo que registra un nuevo usuario segun su rol
    [HttpPost("register")]
    public async Task<ActionResult<object>> Register([FromBody] RegistroRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest(new { mensaje = "Email y contraseña son requeridos" });

        if (request.Password.Length < 8)
            return BadRequest(new { mensaje = "La contraseña debe tener al menos 8 caracteres" });

        try
        {
            var usuario = await _service.RegisterAsync(request);
            if (usuario == null)
                return Conflict(new { mensaje = "El email ya está registrado" });

            return Ok(usuario);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
