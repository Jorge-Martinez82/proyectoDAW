using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IAdoptantesRepository
{
    // obtiene un adoptante por uuid
    Task<Adoptante?> GetByUuidAsync(Guid adoptanteUuid);
    // obtiene un adoptante por id
    Task<Adoptante?> GetByIdAsync(int id);
    // obtiene el perfil de adoptante por uuid de usuario
    Task<Adoptante?> GetByUsuarioUuidAsync(Guid usuarioUuid);
    // obtiene adoptantes con paginacion
    Task<(IEnumerable<Adoptante> adoptantes, int total)> GetAllAsync(int pageNumber, int pageSize);
    // crea un nuevo adoptante
    Task<Adoptante> CreateAsync(Adoptante adoptante);
    // actualiza un adoptante
    Task<bool> UpdateAsync(Adoptante adoptante);
}
