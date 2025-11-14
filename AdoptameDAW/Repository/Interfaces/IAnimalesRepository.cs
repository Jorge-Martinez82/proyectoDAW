using AdoptameDAW.Models;

public interface IAnimalesRepository
{
    Task<Animal?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Animal> animales, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null);
}
