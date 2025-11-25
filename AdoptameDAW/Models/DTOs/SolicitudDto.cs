using System;

namespace AdoptameDAW.Models.DTOs
{
    public class SolicitudDto
    {
        public int Id { get; set; }
        public Guid AnimalUuid { get; set; }
        public Guid UsuarioAdoptanteUuid { get; set; }
        public Guid UsuarioProtectoraUuid { get; set; }
        public string? Comentario { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public string? AdoptanteNombre { get; set; }
        public string? AdoptanteApellidos { get; set; }
        public string? AdoptanteTelefono { get; set; }
        public string? AdoptanteEmail { get; set; }
    }
}