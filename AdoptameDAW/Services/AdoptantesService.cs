using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Models;
using AutoMapper;
using AdoptameDAW.Repositories;

namespace AdoptameDAW.Services;

public class AdoptantesService
{
    private readonly IAdoptantesRepository _repository;
    private readonly IMapper _mapper;

    public AdoptantesService(IAdoptantesRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AdoptanteDto?> AdoptantesServiceGetById(Guid id)
    {
        var adoptante = await _repository.AdoptantesRepositoryGetById(id);
        return adoptante == null ? null : _mapper.Map<AdoptanteDto>(adoptante);
    }

    public async Task<object> AdoptantesServiceGetAll(int pageNumber, int pageSize)
    {
        pageSize = pageSize > 12 ? 12 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        var (adoptantes, total) = await _repository.AdoptantesRepositoryGetAll(
            pageNumber,
            pageSize);

        var adoptantesDto = _mapper.Map<IEnumerable<AdoptanteDto>>(adoptantes);

        return new
        {
            data = adoptantesDto,
            pageNumber,
            pageSize,
            totalCount = total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public async Task<bool> AdoptantesServiceUpdate(Guid id, AdoptanteDto dto)
    {
        var entidad = await _repository.AdoptantesRepositoryGetById(id);
        if (entidad == null)
            return false;
        _mapper.Map(dto, entidad);

        return await _repository.AdoptantesRepositoryUpdate(entidad);
    }
}
