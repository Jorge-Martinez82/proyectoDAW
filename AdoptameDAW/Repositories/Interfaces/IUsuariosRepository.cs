using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IUsuariosRepository
{
    // obtiene un usuario por email
    Task<Usuario?> GetByEmailAsync(string email);
    // obtiene un usuario por uuid
    Task<Usuario?> GetByUuidAsync(Guid uuid);
    // crea un nuevo usuario
    Task<Usuario> CreateAsync(Usuario usuario);
}
