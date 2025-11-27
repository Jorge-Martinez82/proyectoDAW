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

    public async Task<Animal?> GetByUuidAsync(Guid id)
    {
        return await _context.Animales
            .Include(a => a.Protectora)
            .FirstOrDefaultAsync(a => a.Uuid == id);
    }

    public async Task<(IEnumerable<Animal> animales, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null)
    {
        var query = _context.Animales
            .Include(a => a.Protectora)
            .AsNoTracking();

        if (protectoraUuid.HasValue)
            query = query.Where(a => a.Protectora.Uuid == protectoraUuid.Value);
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

    public async Task<bool> DeleteAsync(Guid uuid)
    {
        var entity = await _context.Animales.FirstOrDefaultAsync(a => a.Uuid == uuid);
        if (entity == null) return false;
        _context.Animales.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Animal> CreateAsync(Animal animal)
    {
        animal.CreatedAt = DateTime.UtcNow;
        await _context.Animales.AddAsync(animal);
        await _context.SaveChangesAsync();
        return await _context.Animales.Include(a => a.Protectora).FirstAsync(a => a.Id == animal.Id);
    }
}
