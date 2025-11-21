using AdoptameDAW.Data;
using AdoptameDAW.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptameDAW.Repositories;

public class ProtectorasRepository : IProtectorasRepository
{
    private readonly ApplicationDbContext _context;

    public ProtectorasRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Protectora?> GetByUuidAsync(Guid uuid)
    {
        return await _context.Protectoras
            .Include(p => p.User)
            .Include(p => p.Animales)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Uuid == uuid);
    }

    public async Task<(IEnumerable<Protectora> protectoras, int total)> GetAllAsync(int pageNumber, int pageSize, string? provincia = null)
    {
        var query = _context.Protectoras
            .Include(p => p.User)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(provincia))
        {
            query = query.Where(p => p.Provincia != null && p.Provincia.ToLower() == provincia.ToLower());
        }

        var total = await query.CountAsync();

        var protectoras = await query
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (protectoras, total);
    }

    public async Task<Protectora> CreateAsync(Protectora protectora)
    {
        protectora.Uuid = Guid.NewGuid();
        protectora.CreatedAt = DateTime.UtcNow;
        await _context.Protectoras.AddAsync(protectora);
        await _context.SaveChangesAsync();
        return protectora;
    }

    public async Task<Protectora?> GetByUsuarioUuidAsync(Guid usuarioUuid)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Uuid == usuarioUuid);
        if (usuario == null) return null;

        return await _context.Protectoras
            .Include(p => p.User)
            .Include(p => p.Animales)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == usuario.Id);
    }

    public async Task<bool> UpdateAsync(Protectora protectora)
    {
        var entidad = await _context.Protectoras.FirstOrDefaultAsync(p => p.Uuid == protectora.Uuid);
        if (entidad == null) return false;

        entidad.Nombre = protectora.Nombre;
        entidad.Direccion = protectora.Direccion;
        entidad.Telefono = protectora.Telefono;
        entidad.Provincia = protectora.Provincia;
        entidad.Email = protectora.Email;
        entidad.Imagen = protectora.Imagen;

        _context.Protectoras.Update(entidad);
        var cambios = await _context.SaveChangesAsync();
        return cambios > 0;
    }
}
