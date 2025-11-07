using AdoptameDAW.Models.Enums;

namespace AdoptameDAW.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public TipoUsuario TipoUsuario { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Protectora> Protectoras { get; set; } = new List<Protectora>();
}
