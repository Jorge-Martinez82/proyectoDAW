using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IUsuariosRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(Guid uuid);
    Task<Usuario> CreateAsync(Usuario usuario);
}

