namespace AdoptameDAW.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string TipoUsuario { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<Protectora> Protectoras { get; set; } = new List<Protectora>();
}
