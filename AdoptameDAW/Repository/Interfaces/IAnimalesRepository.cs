using AdoptameDAW.Models;

public interface IAnimalesRepository
{
    Task<Animal?> GetByIdAsync(int id);
    Task<(IEnumerable<Animal> animales, int total)> GetAllAsync(
        int pageNumber,
        int pageSize,
        int? protectoraId = null,
        string? tipo = null,
        string? provincia = null);
}
