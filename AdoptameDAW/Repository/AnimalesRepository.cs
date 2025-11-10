using AdoptameDAW.Data;
using AdoptameDAW.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptameDAW.Repositories;

public class AnimalesRepository : IAnimalesRepository
{
    private readonly ApplicationDbContext _context;

    public AnimalesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Animal?> GetByIdAsync(int id)
    {
        return await _context.Animales
            .Include(a => a.Protectora)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<(IEnumerable<Animal> animales, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        int? protectoraId = null,
        string? tipo = null,
        string? provincia = null)
    {
        var query = _context.Animales
            .Include(a => a.Protectora)
            .AsNoTracking();

        if (protectoraId.HasValue)
            query = query.Where(a => a.ProtectoraId == protectoraId.Value);
        if (!string.IsNullOrEmpty(tipo))
        {
            query = query.Where(a => a.Tipo.ToLower() == tipo.ToLower());
        }
        if (!string.IsNullOrEmpty(provincia))
        {
            query = query.Where(a => a.Protectora.Provincia.ToLower() == provincia.ToLower());
        }

        var total = await query.CountAsync();

        var animales = await query
            .OrderBy(a => a.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (animales, total);
    }
}
