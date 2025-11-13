using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IProtectorasRepository
{
    Task<Protectora?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Protectora> protectoras, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? provincia = null);
}
