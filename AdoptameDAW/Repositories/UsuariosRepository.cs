using AdoptameDAW.Data;
using AdoptameDAW.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptameDAW.Repositories;

public class UsuariosRepository : IUsuariosRepository
{
    private readonly ApplicationDbContext _context;

    public UsuariosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<Usuario?> GetByUuidAsync(Guid uuid)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Uuid == uuid);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        usuario.Uuid = Guid.NewGuid();
        usuario.CreatedAt = DateTime.UtcNow;
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }
}
