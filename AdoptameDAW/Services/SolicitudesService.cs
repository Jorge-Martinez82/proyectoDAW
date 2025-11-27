using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories;
using AutoMapper;


namespace AdoptameDAW.Services
{
    public class SolicitudesService
    {
        private readonly ISolicitudesRepository _solicitudesRepository;
        private readonly IAnimalesRepository _animalesRepository;
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IProtectorasRepository _protectorasRepository;
        private readonly IAdoptantesRepository _adoptantesRepository;
        private readonly IMapper _mapper;

        public SolicitudesService(
            ISolicitudesRepository solicitudesRepository,
            IAnimalesRepository animalesRepository,
            IUsuariosRepository usuariosRepository,
            IProtectorasRepository protectorasRepository,
            IAdoptantesRepository adoptantesRepository,
            IMapper mapper)
        {
            _solicitudesRepository = solicitudesRepository;
            _animalesRepository = animalesRepository;
            _usuariosRepository = usuariosRepository;
            _protectorasRepository = protectorasRepository;
            _adoptantesRepository = adoptantesRepository;
            _mapper = mapper;
        }

        // metodo que crea una nueva solicitud de adopcion
        public async Task<SolicitudDto?> CreateAsync(Guid usuarioAdoptanteUuid, Guid animalUuid, string comentario)
        {
            var usuarioAdoptante = await _usuariosRepository.GetByUuidAsync(usuarioAdoptanteUuid);
            if (usuarioAdoptante == null) return null;

            var animal = await _animalesRepository.GetByUuidAsync(animalUuid);
            if (animal == null) return null;

            var protectora = await _protectorasRepository.GetByUuidAsync(animal.Protectora.Uuid);
            if (protectora == null) return null;
            var usuarioProtectora = await _usuariosRepository.GetByUuidAsync(protectora.User.Uuid);
            if (usuarioProtectora == null) return null;

            var solicitud = new Solicitud
            {
                Comentario = comentario,
                Estado = "pendiente",
                AnimalId = animal.Id,
                UsuarioAdoptanteId = usuarioAdoptante.Id,
                UsuarioProtectoraId = usuarioProtectora.Id
            };

            var creada = await _solicitudesRepository.CreateAsync(solicitud);
            var dto = _mapper.Map<SolicitudDto>(creada);
            var adoptantePerfil = await _adoptantesRepository.GetByUsuarioUuidAsync(usuarioAdoptanteUuid);
            if (adoptantePerfil != null)
            {
                dto.AdoptanteNombre = adoptantePerfil.Nombre;
                dto.AdoptanteApellidos = adoptantePerfil.Apellidos;
                dto.AdoptanteTelefono = adoptantePerfil.Telefono;
                dto.AdoptanteEmail = adoptantePerfil.Email;
            }
            return dto;
        }

        // metodo que obtiene solicitudes del adoptante con paginacion
        public async Task<object> GetByAdoptanteAsync(Guid usuarioUuid, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (solicitudes, total) = await _solicitudesRepository.GetByAdoptanteAsync(usuarioUuid, pageNumber, pageSize);
            var dtos = _mapper.Map<List<SolicitudDto>>(solicitudes);

            var adoptantePerfil = await _adoptantesRepository.GetByUsuarioUuidAsync(usuarioUuid);
            if (adoptantePerfil != null)
            {
                foreach (var d in dtos)
                {
                    d.AdoptanteNombre = adoptantePerfil.Nombre;
                    d.AdoptanteApellidos = adoptantePerfil.Apellidos;
                    d.AdoptanteTelefono = adoptantePerfil.Telefono;
                    d.AdoptanteEmail = adoptantePerfil.Email;
                }
            }

            return new
            {
                data = dtos,
                pageNumber,
                pageSize,
                totalCount = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        // metodo que obtiene solicitudes recibidas por la protectora con paginacion
        public async Task<object> GetByProtectoraAsync(Guid usuarioUuid, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (solicitudes, total) = await _solicitudesRepository.GetByProtectoraAsync(usuarioUuid, pageNumber, pageSize);
            var dtos = _mapper.Map<List<SolicitudDto>>(solicitudes);

            var adoptantesUuids = dtos.Select(d => d.UsuarioAdoptanteUuid).Distinct().ToList();
            var cache = new Dictionary<Guid, Adoptante?>();
            foreach (var au in adoptantesUuids)
            {
                var perf = await _adoptantesRepository.GetByUsuarioUuidAsync(au);
                cache[au] = perf;
            }
            foreach (var d in dtos)
            {
                if (cache.TryGetValue(d.UsuarioAdoptanteUuid, out var perf) && perf != null)
                {
                    d.AdoptanteNombre = perf.Nombre;
                    d.AdoptanteApellidos = perf.Apellidos;
                    d.AdoptanteTelefono = perf.Telefono;
                    d.AdoptanteEmail = perf.Email;
                }
            }

            return new
            {
                data = dtos,
                pageNumber,
                pageSize,
                totalCount = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        // metodo que actualiza el estado de una solicitud de adopcion
        public async Task<bool> UpdateEstadoAsync(Guid usuarioProtectoraUuid, int solicitudId, string nuevoEstado)
        {
            if (nuevoEstado != "aceptada" && nuevoEstado != "rechazada")
                return false;

            var usuarioProtectora = await _usuariosRepository.GetByUuidAsync(usuarioProtectoraUuid);
            if (usuarioProtectora == null) return false;

            return await _solicitudesRepository.UpdateEstadoAsync(solicitudId, nuevoEstado, usuarioProtectora.Id);
        }
    }
}