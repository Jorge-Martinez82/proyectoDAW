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
    private readonly IUsuariosRepository _usuariosRepository;
    private readonly IAdoptantesRepository _adoptantesRepository;
    private readonly IProtectorasRepository _protectorasRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UsuariosService(
        IUsuariosRepository usuariosRepository,
        IAdoptantesRepository adoptantesRepository,
        IProtectorasRepository protectorasRepository,
        IMapper mapper,
        IConfiguration configuration)
    {
        _usuariosRepository = usuariosRepository;
        _adoptantesRepository = adoptantesRepository;
        _protectorasRepository = protectorasRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    // metodo que valida credenciales y genera un token jwt
    public async Task<LoginResponseDto?> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuariosRepository.GetByEmailAsync(request.Email);
        if (usuario == null) return null;
        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, usuario.PasswordHash)) return null;

        var token = GenerateJwtToken(usuario);
        var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

        return new LoginResponseDto
        {
            Token = token,
            Usuario = usuarioDto
        };
    }

    // metodo que registra un nuevo usuario y su perfil asociado
    public async Task<object?> RegisterAsync(RegistroRequestDto request)
    {
        var existeUsuario = await _usuariosRepository.GetByEmailAsync(request.Email);
        if (existeUsuario != null) return null;

        if (request.TipoUsuario != "Protectora" && request.TipoUsuario != "Adoptante")
            throw new ArgumentException("Tipo de usuario inválido");

        var usuario = new Usuario
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 13),
            TipoUsuario = request.TipoUsuario
        };
        await _usuariosRepository.CreateAsync(usuario);

        if (request.TipoUsuario == "Adoptante")
        {
            var adoptante = new Adoptante
            {
                Nombre = request.Nombre!,
                Apellidos = request.Apellidos!,
                Direccion = request.Direccion ?? string.Empty,
                CodigoPostal = request.CodigoPostal ?? string.Empty,
                Poblacion = request.Poblacion ?? string.Empty,
                Provincia = request.Provincia ?? string.Empty,
                Telefono = request.Telefono ?? string.Empty,
                Email = request.Email,
                UserId = usuario.Id
            };
            await _adoptantesRepository.CreateAsync(adoptante);
            return _mapper.Map<AdoptanteDto>(adoptante);
        }
        else
        {
            var protectora = new Protectora
            {
                Nombre = request.NombreProtectora!,
                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Provincia = request.Provincia,
                Email = request.Email,
                UserId = usuario.Id
            };
            await _protectorasRepository.CreateAsync(protectora);
            return _mapper.Map<ProtectoraDto>(protectora);
        }
    }

    // metodo que genera el token para el ususario
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
