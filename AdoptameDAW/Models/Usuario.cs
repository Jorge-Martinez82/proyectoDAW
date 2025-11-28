namespace AdoptameDAW.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public Guid Uuid { get; set; } = Guid.NewGuid();

    public string Email { get; set; } = string.Empty;

    public string? PasswordHash { get; set; }

    public string TipoUsuario { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Protectora> Protectoras { get; set; } = new List<Protectora>();

    public ICollection<Adoptante> Adoptantes { get; set; } = new List<Adoptante>();
}
