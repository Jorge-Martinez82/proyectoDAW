using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IProtectorasRepository
{
    Task<Protectora?> GetByUuidAsync(Guid uuid);
    Task<(IEnumerable<Protectora> protectoras, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? provincia = null);
    Task<Protectora> CreateAsync(Protectora protectora);
    Task<Protectora?> GetByUsuarioUuidAsync(Guid usuarioUuid);
    Task<bool> UpdateAsync(Protectora protectora);
}
