using AdoptameDAW.Models;

public interface IAnimalesRepository
{
    Task<Animal?> AnimalesRepositoryGetById(Guid id);
    Task<(IEnumerable<Animal> animales, int total)> AnimalesRepositoryGetAll(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null);
    Task<bool> AnimalesRepositoryDelete(Guid uuid);
}
