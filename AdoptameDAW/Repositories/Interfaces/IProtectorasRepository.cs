using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IProtectorasRepository
{
    // obtiene una protectora por uuid
    Task<Protectora?> GetByUuidAsync(Guid uuid);
    // obtiene protectorass con paginacion y filtro de provincia
    Task<(IEnumerable<Protectora> protectoras, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? provincia = null);
    // crea una nueva protectora
    Task<Protectora> CreateAsync(Protectora protectora);
    // obtiene la protectora por uuid de usuario
    Task<Protectora?> GetByUsuarioUuidAsync(Guid usuarioUuid);
    // actualiza una protectora
    Task<bool> UpdateAsync(Protectora protectora);
}
