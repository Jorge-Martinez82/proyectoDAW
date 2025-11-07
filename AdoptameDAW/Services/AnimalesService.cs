using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories.Interfaces;
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

    public async Task<AnimalDto?> GetByIdAsync(int id)
    {
        var animal = await _repository.GetByIdAsync(id);
        return animal == null ? null : _mapper.Map<AnimalDto>(animal);
    }

    public async Task<IEnumerable<AnimalDto>> GetAllAsync(int? protectoraId = null)
    {
        var animales = await _repository.GetAllAsync(protectoraId);
        return _mapper.Map<IEnumerable<AnimalDto>>(animales);
    }
}
