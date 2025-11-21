using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IAdoptantesRepository
{
    Task<Adoptante?> GetByUuidAsync(Guid adoptanteUuid);
    Task<Adoptante?> GetByIdAsync(int id);
    Task<Adoptante?> GetByUsuarioUuidAsync(Guid usuarioUuid);
    Task<(IEnumerable<Adoptante> adoptantes, int total)> GetAllAsync(int pageNumber, int pageSize);
    Task<Adoptante> CreateAsync(Adoptante adoptante);
    Task<bool> UpdateAsync(Adoptante adoptante);
}
