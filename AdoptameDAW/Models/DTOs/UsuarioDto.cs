using AdoptameDAW.Models.Enums;

namespace AdoptameDAW.Models.DTOs;

public class UsuarioDto
{
    public Guid Uuid { get; set; }

    public string Email { get; set; } = null!;

    public TipoUsuario TipoUsuario { get; set; }

    public DateTime CreatedAt { get; set; }
}
