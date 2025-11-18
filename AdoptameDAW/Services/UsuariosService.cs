using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdoptameDAW.Services;

public class UsuariosService
{
    private readonly IUsuariosRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UsuariosService(IUsuariosRepository repository, IMapper mapper, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequest request)
    {
        var usuario = await _repository.GetByEmailAsync(request.Email);

        if (usuario == null)
            return null;

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, usuario.PasswordHash))
            return null;

        var token = GenerateJwtToken(usuario);
        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

        return new LoginResponseDto
        {
            Token = token,
            Usuario = usuarioDto
        };
    }

    public async Task<UsuarioDto?> RegisterAsync(RegistroRequestDto request)
    {
        var existingUser = await _repository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            return null;

        if (request.TipoUsuario != "Protectora" && request.TipoUsuario != "Adoptante")
            throw new ArgumentException("Tipo de usuario inválido");

        var usuario = new Usuario
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 13),
            TipoUsuario = request.TipoUsuario
        };

        var createdUser = await _repository.CreateAsync(usuario);
        return _mapper.Map<UsuarioDto>(createdUser);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Uuid.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
