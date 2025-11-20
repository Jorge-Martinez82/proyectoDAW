using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IAdoptantesRepository
{
    Task<Adoptante?> AdoptantesRepositoryGetById(Guid id);
    Task<(IEnumerable<Adoptante> adoptantes, int total)> AdoptantesRepositoryGetAll(
        int pageNumber,
        int pageSize);
    Task<Adoptante> AdoptantesRepositoryCreate(Adoptante adoptante);
    Task<bool> AdoptantesRepositoryUpdate(Adoptante adoptante);
}
