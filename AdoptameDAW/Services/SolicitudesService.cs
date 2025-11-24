using AdoptameDAW.Models;
using AdoptameDAW.Models.DTOs;
using AdoptameDAW.Repositories;
using AutoMapper;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdoptameDAW.Services
{
    public class SolicitudesService
    {
        private readonly ISolicitudesRepository _solicitudesRepository;
        private readonly IAnimalesRepository _animalesRepository;
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IProtectorasRepository _protectorasRepository;
        private readonly IMapper _mapper;

        public SolicitudesService(
            ISolicitudesRepository solicitudesRepository,
            IAnimalesRepository animalesRepository,
            IUsuariosRepository usuariosRepository,
            IProtectorasRepository protectorasRepository,
            IMapper mapper)
        {
            _solicitudesRepository = solicitudesRepository;
            _animalesRepository = animalesRepository;
            _usuariosRepository = usuariosRepository;
            _protectorasRepository = protectorasRepository;
            _mapper = mapper;
        }

        public async Task<SolicitudDto?> CrearAsync(Guid usuarioAdoptanteUuid, Guid animalUuid, string comentario)
        {
            var usuarioAdoptante = await _usuariosRepository.GetByUuidAsync(usuarioAdoptanteUuid);
            if (usuarioAdoptante == null) return null;

            var animal = await _animalesRepository.AnimalesRepositoryGetById(animalUuid);
            if (animal == null) return null;

            var protectora = await _protectorasRepository.GetByUuidAsync(animal.Protectora.Uuid);
            if (protectora == null) return null;
            var usuarioProtectora = await _usuariosRepository.GetByUuidAsync(protectora.User.Uuid);
            if (usuarioProtectora == null) return null;

            var entidad = new Solicitud
            {
                Comentario = comentario,
                Estado = "pendiente",
                AnimalId = animal.Id,
                UsuarioAdoptanteId = usuarioAdoptante.Id,
                UsuarioProtectoraId = usuarioProtectora.Id
            };

            var creada = await _solicitudesRepository.CreateAsync(entidad);
            return _mapper.Map<SolicitudDto>(creada);
        }

        public async Task<object> GetByAdoptanteAsync(Guid usuarioUuid, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (solicitudes, total) = await _solicitudesRepository.GetByAdoptanteAsync(usuarioUuid, pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<SolicitudDto>>(solicitudes);

            return new
            {
                data = dtos,
                pageNumber,
                pageSize,
                totalCount = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        public async Task<object> GetByProtectoraAsync(Guid usuarioUuid, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 12 ? 12 : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var (solicitudes, total) = await _solicitudesRepository.GetByProtectoraAsync(usuarioUuid, pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<SolicitudDto>>(solicitudes);

            return new
            {
                data = dtos,
                pageNumber,
                pageSize,
                totalCount = total,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            };
        }

        public async Task<bool> ActualizarEstadoAsync(Guid usuarioProtectoraUuid, int solicitudId, string nuevoEstado)
        {
            if (nuevoEstado != "aceptada" && nuevoEstado != "rechazada")
                return false;

            var usuarioProtectora = await _usuariosRepository.GetByUuidAsync(usuarioProtectoraUuid);
            if (usuarioProtectora == null) return false;

            return await _solicitudesRepository.UpdateEstadoAsync(solicitudId, nuevoEstado, usuarioProtectora.Id);
        }
    }
}