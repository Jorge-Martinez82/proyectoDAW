using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IAdoptantesRepository
{
    Task<Adoptante> CreateAsync(Adoptante adoptante);
}
