using AdoptameDAW.Models.DTOs;
using AutoMapper;

namespace AdoptameDAW.Services;

public class AnimalesService
{
    private readonly IAnimalesRepository _repository;
    private readonly IMapper _mapper;

    public AnimalesService(IAnimalesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AnimalDto?> GetByIdAsync(Guid id)
    {
        var animal = await _repository.GetByIdAsync(id);
        return animal == null ? null : _mapper.Map<AnimalDto>(animal);
    }

    public async Task<object> GetAllAsync(
        int pageNumber,
        int pageSize,
        int? protectoraId = null,
        string? tipo = null,
        string? provincia = null)
    {

        pageSize = pageSize > 12 ? 12 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        var (animales, total) = await _repository.GetAllAsync(
            pageNumber,
            pageSize,
            protectoraId,
            tipo,
            provincia);

        var animalesDto = _mapper.Map<IEnumerable<AnimalDto>>(animales);

        return new
        {
            data = animalesDto,
            pageNumber,
            pageSize,
            totalCount = total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }
}
