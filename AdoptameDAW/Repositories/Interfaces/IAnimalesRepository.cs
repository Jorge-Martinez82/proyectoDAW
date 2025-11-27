using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories;

public interface IAnimalesRepository
{
    Task<Animal?> GetByUuidAsync(Guid id);
    Task<(IEnumerable<Animal> animales, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null);
    Task<bool> DeleteAsync(Guid uuid);
    Task<Animal> CreateAsync(Animal animal);
}
