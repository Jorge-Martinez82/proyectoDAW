using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Models;
using AdoptameDAW.Repositories;
using AutoMapper;

namespace AdoptameDAW.Services;

public class AnimalesService
{
    private readonly IAnimalesRepository _repository;
    private readonly IProtectorasRepository _protectorasRepository;
    private readonly IMapper _mapper;

    public AnimalesService(IAnimalesRepository repository, IMapper mapper, IProtectorasRepository protectorasRepository)
    {
        _repository = repository;
        _mapper = mapper;
        _protectorasRepository = protectorasRepository;
    }

    public async Task<AnimalDto?> AnimalesServiceGetById(Guid id)
    {
        var animal = await _repository.AnimalesRepositoryGetById(id);
        return animal == null ? null : _mapper.Map<AnimalDto>(animal);
    }

    public async Task<object> AnimalesServiceGetAll(
        int pageNumber,
        int pageSize,
        Guid? protectoraUuid = null,
        string? tipo = null,
        string? provincia = null)
    {

        pageSize = pageSize > 12 ? 12 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        var (animales, total) = await _repository.AnimalesRepositoryGetAll(
            pageNumber,
            pageSize,
            protectoraUuid,
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

    public async Task<bool> AnimalesServiceDelete(Guid uuid)
    {
        return await _repository.AnimalesRepositoryDelete(uuid);
    }

    public async Task<AnimalDto?> AnimalesServiceCreate(AnimalDto dto)
    {
        var protectora = await _protectorasRepository.GetByUuidAsync(dto.Uuid == Guid.Empty ? Guid.Empty : dto.Uuid);
        if (protectora == null && dto.ProtectoraId == 0) return null;

        var entity = new Animal
        {
            Uuid = Guid.NewGuid(),
            Nombre = dto.Nombre ?? string.Empty,
            Tipo = dto.Tipo ?? string.Empty,
            Raza = dto.Raza,
            Edad = dto.Edad,
            Genero = dto.Genero,
            Descripcion = dto.Descripcion,
            ProtectoraId = dto.ProtectoraId != 0 ? dto.ProtectoraId : (protectora?.Id ?? 0),
            ImagenPrincipal = dto.ImagenPrincipal
        };

        var creado = await _repository.AnimalesRepositoryCreate(entity);
        return _mapper.Map<AnimalDto>(creado);
    }
}
