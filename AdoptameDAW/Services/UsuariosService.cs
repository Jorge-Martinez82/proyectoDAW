using AdoptameDAW.Data;
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
    private readonly IAdoptantesRepository _adoptantesRepository;
    private readonly IProtectorasRepository _protectorasRepository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UsuariosService(
        IUsuariosRepository repository,
        IAdoptantesRepository adoptantesRepository,
        IProtectorasRepository protectorasRepository,
        ApplicationDbContext context,
        IMapper mapper,
        IConfiguration configuration)
    {
        _repository = repository;
        _adoptantesRepository = adoptantesRepository;
        _protectorasRepository = protectorasRepository;
        _context = context;
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

    public async Task<object?> RegistroAsync(RegistroRequestDto request)
    {
        var existingUser = await _repository.GetByEmailAsync(request.Email);
        if (existingUser != null)
            return null;

        if (request.TipoUsuario != "Protectora" && request.TipoUsuario != "Adoptante")
            throw new ArgumentException("Tipo de usuario inválido");

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var usuario = new Usuario
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 13),
                TipoUsuario = request.TipoUsuario
            };

            await _repository.CreateAsync(usuario);
            await _context.SaveChangesAsync();

            if (request.TipoUsuario == "Adoptante")
            {
                var adoptante = new Adoptante
                {
                    Nombre = request.Nombre!,
                    Apellidos = request.Apellidos!,
                    Direccion = request.Direccion,
                    CodigoPostal = request.CodigoPostal,
                    Poblacion = request.Poblacion,
                    Provincia = request.Provincia,
                    Telefono = request.Telefono,
                    Email = request.Email,
                    UserId = usuario.Id 
                };

                await _adoptantesRepository.CreateAsync(adoptante);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

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
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<ProtectoraDto>(protectora);
            }
        }
        catch (Exception)
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
            throw;
        }
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
