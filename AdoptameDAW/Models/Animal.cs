namespace AdoptameDAW.Models;

public partial class Animal
{
    public int Id { get; set; }

    public Guid Uuid { get; set; } = Guid.NewGuid();

    public string Nombre { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public string? Raza { get; set; }

    public int? Edad { get; set; }

    public string? Genero { get; set; }

    public string? Descripcion { get; set; }

    public int ProtectoraId { get; set; }

    public string? ImagenPrincipal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Protectora? Protectora { get; set; }
}
