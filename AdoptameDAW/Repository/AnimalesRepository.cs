using AdoptameDAW.Data;
using AdoptameDAW.Models;
using AdoptameDAW.Repositories.Interfaces;
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
        return await _context.Animales.FindAsync(id);
    }

    public async Task<IEnumerable<Animal>> GetAllAsync(int? protectoraId = null)
    {
        var query = _context.Animales.AsQueryable();

        if (protectoraId.HasValue)
            query = query.Where(a => a.ProtectoraId == protectoraId.Value);

        return await query.ToListAsync();
    }
}
