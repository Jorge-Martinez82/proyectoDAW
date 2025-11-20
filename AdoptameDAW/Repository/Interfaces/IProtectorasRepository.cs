using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IProtectorasRepository
{
    Task<Protectora?> ProtectorasRepositoryGetById(Guid id);
    Task<(IEnumerable<Protectora> protectoras, int total)> ProtectorasRepositoryGetAll(
        int pageNumber,
        int pageSize,
        string? provincia = null);
    Task<Protectora> ProtectorasRepositoryCreate(Protectora protectora);

}
