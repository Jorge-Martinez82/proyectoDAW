using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IUsuariosRepository
{
    Task<Usuario?> UsuariosRepositoryGetByEmail(string email);
    Task<Usuario?> UsuariosRepositoryGetById(Guid uuid);
    Task<Usuario> UsuariosRepositoryCreate(Usuario usuario);
}

