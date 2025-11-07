using AdoptameDAW.Models;

namespace AdoptameDAW.Repositories.Interfaces;

public interface IAnimalesRepository
{
    Task<Animal?> GetByIdAsync(int id);
    Task<IEnumerable<Animal>> GetAllAsync(int? protectoraId = null);
}
