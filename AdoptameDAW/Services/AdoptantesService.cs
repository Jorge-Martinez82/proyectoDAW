using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories;
using AutoMapper;

namespace AdoptameDAW.Services
{
    public class AdoptantesService
    {
        private readonly IAdoptantesRepository _repository;
        private readonly IMapper _mapper;

        public AdoptantesService(IAdoptantesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // metodo que obtiene el perfil de adoptante por uuid de usuario
        public async Task<AdoptanteDto?> GetByUsuarioUuidAsync(Guid usuarioUuid)
        {
            var adoptante = await _repository.GetByUsuarioUuidAsync(usuarioUuid);
            return adoptante == null ? null : _mapper.Map<AdoptanteDto>(adoptante);
        }

        // metodo que obtiene adoptante por su uuid
        public async Task<AdoptanteDto?> GetByAdoptanteUuidAsync(Guid adoptanteUuid)
        {
            var adoptante = await _repository.GetByUuidAsync(adoptanteUuid);
            return adoptante == null ? null : _mapper.Map<AdoptanteDto>(adoptante);
        }

        // metodo que devuelve adoptantes con paginacion
        public async Task<object> GetAllAsync(int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (adoptantes, total) = await _repository.GetAllAsync(pageNumber, pageSize);
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

        // metodo que actualiza un adoptante
        public async Task<bool> UpdateAsync(Guid adoptanteUuid, AdoptanteDto dto)
        {
            var adoptante = await _repository.GetByUuidAsync(adoptanteUuid);
            if (adoptante == null) return false;

            _mapper.Map(dto, adoptante);
            return await _repository.UpdateAsync(adoptante);
        }

        // metodo que crea un adoptante
        public async Task<AdoptanteDto> CreateAsync(AdoptanteDto dto)
        {
            var adoptante = _mapper.Map<Adoptante>(dto);
            var creado = await _repository.CreateAsync(adoptante);
            return _mapper.Map<AdoptanteDto>(creado);
        }
    }
}
