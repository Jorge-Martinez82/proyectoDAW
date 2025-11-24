using AdoptameDAW.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdoptameDAW.Repositories
{
    public interface ISolicitudesRepository
    {
        Task<Solicitud> CreateAsync(Solicitud solicitud);
        Task<Solicitud?> GetByIdAsync(int id);
        Task<(IEnumerable<Solicitud> solicitudes, int total)> GetByAdoptanteAsync(Guid usuarioAdoptanteUuid, int pageNumber, int pageSize);
        Task<(IEnumerable<Solicitud> solicitudes, int total)> GetByProtectoraAsync(Guid usuarioProtectoraUuid, int pageNumber, int pageSize);
        Task<bool> UpdateEstadoAsync(int solicitudId, string nuevoEstado, int usuarioProtectoraId);
    }
}