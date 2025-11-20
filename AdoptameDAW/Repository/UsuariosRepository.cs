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

    public async Task<Usuario?> UsuariosRepositoryGetByEmail(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<Usuario?> UsuariosRepositoryGetById(Guid uuid)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Uuid == uuid);
    }

    public async Task<Usuario> UsuariosRepositoryCreate(Usuario usuario)
    {
        usuario.Uuid = Guid.NewGuid();
        usuario.CreatedAt = DateTime.UtcNow;

        await _context.Usuarios.AddAsync(usuario);

        return usuario;
    }
}
