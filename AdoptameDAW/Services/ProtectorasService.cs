using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Models;
using AutoMapper;
using AdoptameDAW.Repositories;

namespace AdoptameDAW.Services;

public class ProtectorasService
{
    private readonly IProtectorasRepository _repository;
    private readonly IMapper _mapper;

    public ProtectorasService(IProtectorasRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProtectoraDto?> GetByUuidAsync(Guid uuid)
    {
        var protectora = await _repository.GetByUuidAsync(uuid);
        return protectora == null ? null : _mapper.Map<ProtectoraDto>(protectora);
    }

    public async Task<object> GetAllAsync(int pageNumber, int pageSize, string? provincia = null)
    {
        pageSize = pageSize > 12 ? 12 : pageSize;
        pageNumber = pageNumber < 1 ? 1 : pageNumber;

        var (protectoras, total) = await _repository.GetAllAsync(pageNumber, pageSize, provincia);
        var protectorasDto = _mapper.Map<IEnumerable<ProtectoraDto>>(protectoras);

        return new
        {
            data = protectorasDto,
            pageNumber,
            pageSize,
            totalCount = total,
            totalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public async Task<ProtectoraDto?> GetByUsuarioUuidAsync(Guid usuarioUuid)
    {
        var protectora = await _repository.GetByUsuarioUuidAsync(usuarioUuid);
        return protectora == null ? null : _mapper.Map<ProtectoraDto>(protectora);
    }

    public async Task<bool> UpdateAsync(Guid protectoraUuid, ProtectoraDto dto)
    {
        var entidad = await _repository.GetByUuidAsync(protectoraUuid);
        if (entidad == null) return false;

        _mapper.Map(dto, entidad);
        return await _repository.UpdateAsync(entidad);
    }
}
