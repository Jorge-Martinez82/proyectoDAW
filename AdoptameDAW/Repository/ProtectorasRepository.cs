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

    public async Task<Protectora?> GetByIdAsync(Guid id)
    {
        return await _context.Protectoras
            .Include(p => p.User)
            .Include(p => p.Animales)
            .FirstOrDefaultAsync(p => p.Uuid == id);
    }

    public async Task<(IEnumerable<Protectora> protectoras, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? provincia = null)
    {
        var query = _context.Protectoras
            .Include(p => p.User)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(provincia))
        {
            query = query.Where(p => p.Provincia.ToLower() == provincia.ToLower());
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
        return protectora;
    }
}
