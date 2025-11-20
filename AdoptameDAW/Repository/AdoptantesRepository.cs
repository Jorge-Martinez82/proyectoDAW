using AdoptameDAW.Data;
using AdoptameDAW.Models;
using AdoptameDAW.Repositories;

namespace AdoptameDAW.Repository
{
    public class AdoptantesRepository : IAdoptantesRepository
    {
        private readonly ApplicationDbContext _context;

        public AdoptantesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Adoptante> CreateAsync(Adoptante adoptante)
        {
            adoptante.Uuid = Guid.NewGuid();

            adoptante.CreatedAt = DateTime.UtcNow;

            await _context.Adoptantes.AddAsync(adoptante);


            return adoptante;
        }
    }
}
