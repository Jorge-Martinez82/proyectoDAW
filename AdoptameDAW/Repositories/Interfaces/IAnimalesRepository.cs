using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IAnimalesRepository
{
    // obtiene un animal por uuid
    Task<Animal?> GetByUuidAsync(Guid id);
    // obtiene animales con filtros y paginacion
    Task<(IEnumerable<Animal> animales, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null);
    // elimina un animal por uuid
    Task<bool> DeleteAsync(Guid uuid);
    // crea un nuevo animal
    Task<Animal> CreateAsync(Animal animal);
}
